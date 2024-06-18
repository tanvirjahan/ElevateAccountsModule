#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
#End Region

Partial Class AccountsModule_unSealdata
    Inherits System.Web.UI.Page

    Dim objUtils As New clsUtils
    Dim objDate As New clsDateTime

    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim mySqlCmd As SqlCommand
    Dim strappid As String = ""
    Dim strappname As String = ""
#Region "Global Declarations"
    Dim msealdate As Date
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
            Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
            Dim appidnew As String = CType(Request.QueryString("appid"), String)

            '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
            '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            ViewState.Add("divcode", divid)

            If Page.IsPostBack = False Then
                '----------------------------- Default Dates
                txtFromDate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy")
                txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")
                description()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("sealdate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub description()
        Dim desc As String
        Try
            desc = "Warning-All Data from  the date given above will be Unsealed and can be Edited or Deleted.Also can make after to the date given above once unsealing is done."

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select top 1  * from sealing_master where div_code='" & ViewState("divcode") & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.Read Then
                If IsDBNull(mySqlReader("sealdate")) = False Then
                    msealdate = CType(mySqlReader("sealdate"), Date)
                    txtpdate.Text = CType(mySqlReader("sealdate"), String)
                    desc = desc + "Last sealed date : " + CType(msealdate, String)
                End If
            Else
                txtpdate.Text = "01/" + "01/" + Year(CType(txtFromDate.Text, String)).ToString
            End If

            DIV1.InnerText = desc

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("sealdata.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try

    End Sub

    Protected Sub btnseal_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim parm(3) As SqlParameter
            Dim i As Integer = 0

            'If validation() = False Then
            '    Exit Sub
            'End If

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand
            sqlTrans = mySqlConn.BeginTransaction()
            mySqlCmd.Connection = mySqlConn
            mySqlCmd.Transaction = sqlTrans
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandText = "sp_mod_sealing_master"

            parm(0) = New SqlParameter("@sealdate", Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"))
            parm(1) = New SqlParameter("@adddate", objDate.GetSystemDateTime(Session("dbconnectionName")))
            parm(2) = New SqlParameter("@divcode", ViewState("divcode"))
            parm(3) = New SqlParameter("@adduser", CType(Session("GlobalUserName"), String))
            For i = 0 To 3
                mySqlCmd.Parameters.Add(parm(i))
            Next

            mySqlCmd.ExecuteNonQuery()
            sqlTrans.Commit()    'SQl Tarn Commit
            sqlTrans.Dispose()
            mySqlConn.Close()
            mySqlConn.Dispose()

            description()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('UnSealing completed');", True)

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("sealdata.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

#Region "Public Function validation() As Boolean"
    Public Function validation() As Boolean
        Dim parms As New List(Of SqlParameter)
        Dim parm(1) As SqlParameter

        Dim frmdate As DateTime
        Dim todate As DateTime
        Dim MyCultureInfo As New CultureInfo("fr-Fr")
        frmdate = DateTime.Parse(txtpdate.Text, MyCultureInfo, DateTimeStyles.None)
        todate = DateTime.Parse(txtFromDate.Text, MyCultureInfo, DateTimeStyles.None)


        validation = True

        If txtFromDate.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Seal Date cannot be empty.');", True)
            SetFocus(txtFromDate)
            validation = False
            Exit Function
        End If

        If txtpdate.Text <> "" Then
            If todate <= frmdate Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Sealing already done upto  " + CType(txtpdate.Text, String) + " choose a later date');", True)
                validation = False
                Exit Function
            End If
        End If

        mySqlCmd = New SqlCommand()
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        mySqlCmd.Connection = mySqlConn
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.CommandText = "sp_validate_Posted"
        parm(0) = New SqlParameter("@sealdate", Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"))
        parm(1) = New SqlParameter("@errmsg", SqlDbType.VarChar, 500)
        parm(1).Direction = ParameterDirection.Output
        mySqlCmd.Parameters.Add(parm(0))
        mySqlCmd.Parameters.Add(parm(1))
        mySqlCmd.ExecuteNonQuery()
        Dim strError As String = ""
        strError = parm(1).Value.ToString()
        If strError <> "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + strError + "');", True)
            validation = False
            Exit Function
        End If
        mySqlConn.Close()
        mySqlConn.Dispose()

    End Function

#End Region
End Class
