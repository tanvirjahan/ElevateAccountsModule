#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
#End Region

Partial Class AccountsModule_unclosingyear
    Inherits System.Web.UI.Page

    Dim objUtils As New clsUtils
    Dim objDate As New clsDateTime
    Dim strSqlQry As String
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim mySqlCmd As SqlCommand
#Region "Global Declarations"
    Dim msealdate As Date
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                '----------------------------- Default Dates
                Dim clyear As String
                Dim desc As String = ""
                Dim fdate As String = ""
                Dim enddate As String
                Dim yearend As String = ""


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlyearclosing, "tran_id", "tran_id", "select tran_id from closeyear ", True)
                txtDivCode.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

                Dim ds As DataSet
                ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select max(tran_id) tran_id from closeyear ")
                If ds.Tables.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)(0)) = False Then
                        ddlyearclosing.Value = ds.Tables(0).Rows(0)("tran_id")
                    End If
                End If


                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlyearclosing.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

                ClientScript.GetPostBackEventReference(Me, String.Empty)
                If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepTBWindowPostBack") Then
                    btnseal_Click(sender, e)
                End If


                desc = desc + " All closing Data will reverse for the above transaction"
                DIV1.InnerText = desc
            Else
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlyearclosing, "tran_id", "tran_id", "select tran_id from closeyear ", True, ddlyearclosing.Value)
                ''description()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("sealdate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub description()
        Dim desc As String
        Try
            desc = "You are about to close the previous year, a journal entry for the closing will be passed automatically as on the date shown above."
            desc = desc + "Warning - All Data Upto the Date given above will be sealed and cannot be edited or deleted. Also no entries can be made prior to the date given above."

            DIV1.InnerText = desc

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("sealdata.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnseal_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim parm(3) As SqlParameter
            Dim i As Integer = 0
            Dim jvno As String = ""
            Dim mbasecurrency As String = ""

            If validation() = False Then
                Exit Sub
            End If

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand
            sqlTrans = mySqlConn.BeginTransaction()
            mySqlCmd.Connection = mySqlConn
            mySqlCmd.Transaction = sqlTrans
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandText = "sp_uncloseyear"

            parm(0) = New SqlParameter("@jvno", CType(ddlyearclosing.Value, String))
            parm(1) = New SqlParameter("@moddate", Format(objDate.GetSystemDateTime(Session("dbconnectionName")), "yyyy/MM/dd"))
            parm(2) = New SqlParameter("@moduser", CType(Session("GlobalUserName"), String))
            mySqlCmd.Parameters.Add(parm(0))
            mySqlCmd.Parameters.Add(parm(1))
            mySqlCmd.Parameters.Add(parm(2))
            mySqlCmd.ExecuteNonQuery()

            sqlTrans.Commit()    'SQl Tarn Commit
            sqlTrans.Dispose()
            mySqlConn.Close()
            mySqlConn.Dispose()

            '' description()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('UnClosing year completed');", True)

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
        Dim MyCultureInfo As New CultureInfo("fr-Fr")


        validation = True


        If ddlyearclosing.Value = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select Closing Year to be Reverse');", True)
            validation = False
            Exit Function
        End If

        Dim ds As DataSet

        ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select enddate  from closeyear where tran_id='" + CType(ddlyearclosing.Value, String) + "' ")
        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                If IsDBNull(ds.Tables(0).Rows(0)(0)) = False Then
                    If frmdate < ds.Tables(0).Rows(0)("enddate") Then
                        frmdate = ds.Tables(0).Rows(0)("enddate")
                    End If
                End If
            End If
        End If


        ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select max(enddate) enddate from closeyear ")
        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                If IsDBNull(ds.Tables(0).Rows(0)(0)) = False Then
                    If frmdate < ds.Tables(0).Rows(0)("enddate") Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Your are selecting old closing transaction. Select later date');", True)
                        validation = False
                        Exit Function
                    End If
                End If
            End If
        End If


    End Function

#End Region
End Class
