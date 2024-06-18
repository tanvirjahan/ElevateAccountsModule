#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
#End Region

Partial Class AccountsModule_Resetdocgen
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
                Dim desc As String = ""

                txtDivCode.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
                Dim typ As Type
                typ = GetType(DropDownList)
                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                End If

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("sealdate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub btnseal_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim parm(0) As SqlParameter

            If validation() = False Then
                Exit Sub
            End If

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand
            sqlTrans = mySqlConn.BeginTransaction()
            mySqlCmd.Connection = mySqlConn
            mySqlCmd.Transaction = sqlTrans
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandText = "sp_newdocgen"

            parm(0) = New SqlParameter("@year", CType(txtyear.Text, Integer))
            mySqlCmd.Parameters.Add(parm(0))

            mySqlCmd.ExecuteNonQuery()
            sqlTrans.Commit()    'SQl Tarn Commit
            sqlTrans.Dispose()
            mySqlConn.Close()
            mySqlConn.Dispose()

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No. Generation has Completed');", True)
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)                      ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)                       'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)                   'connection close
            End If
        End Try
    End Sub

#Region "Public Function validation() As Boolean"
    Public Function validation() As Boolean
        validation = True

        If txtyear.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Closing Year cannot be empty.');", True)
            SetFocus(txtyear)
            validation = False
            Exit Function
        End If


        Dim ds As DataSet
        ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select 't' enddate from closeyear where  year(enddate)='" + txtyear.Text + "' ")
        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Year Already exists');", True)
                validation = False
                Exit Function
            End If
        End If

        Dim dsval As DataSet
        dsval = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select year(max(enddate)) enddate  from closeyear ")
        If dsval.Tables.Count > 0 Then
            If dsval.Tables(0).Rows.Count > 0 Then
                If txtyear.Text < dsval.Tables(0).Rows(0)("enddate") Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Year cannot be less than last closing year');", True)
                    validation = False
                    Exit Function
                End If
            End If
        End If

    End Function
#End Region
End Class
