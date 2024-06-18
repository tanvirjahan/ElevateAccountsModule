Imports System.Data
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Data.SqlClient
Partial Class AccountsModule_RptVatAccrueBreakup
    Inherits System.Web.UI.Page
    Dim month, lastday As String
    Dim max As Integer

    Private Shared divcode As String

    Dim objUtils As New clsUtils
    Dim objDate As New clsDateTime
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim strappid As String = ""
    Dim strappname As String = ""
    Dim day As Date
    Dim objUser As New clsUser
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Dim strfromcode, strtocode As Integer

        Dim frmdate As DateTime
        Dim MyCultureInfo As New CultureInfo("fr-Fr")
        Dim ds As DataSet

        Try
            If txtFromDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtFromDate.ClientID + "');", True)
                SetFocus(txtFromDate)
                ValidatePage = False
                Exit Function
            End If
         
            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function
    Protected Sub btnvataccrued_Click(ByVal sender As Object, ByVal e As System.EventArgs)
      
        Try
            If ValidatePage() = False Then
                Exit Sub
            End If

            Dim strfromdate, strtodate, strclosing, strlevel, strwithmovmt As String
            Dim stracccodefrom As String = ""
            Dim stracccodeto As String = ""
            Dim rptvalue As Integer
            Dim rptype As Integer = 0

            strclosing = 0 'ddlclosing.SelectedIndex.ToString


            strlevel = 0 ' ddlselect.SelectedValue.ToString
            strwithmovmt = 0 ' ddlwithmovmt.SelectedIndex.ToString
            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = IIf(strwithmovmt = 0, Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10), strfromdate)

            'rptype = IIf(chkTree.Checked, 1, 0)
            Dim strpop As String = ""
            strpop = "window.open('TransactionReports.aspx?printId=VATAccrued&reportsType=excel&fromdate=" & strfromdate & "&divid=" & ViewState("divcode") & "','RepVATAcc' );"
            'strpop = "window.open('TransactionReports.aspx?printId=VATAccrued&reportsType=excel&fromdate=" & strfromdate & "&todate=" & strtodate & "&closing=" & strclosing & "&level=" & strlevel & "&divid=" & ViewState("divcode") & "&withmov=" & strwithmovmt & "&fromname=" & stracccodeto & "&acccodefrom=" & stracccodefrom & "&acccodeto=" & stracccodeto & "&rptype=" & rptype & "&rptval=" & rptvalue & "','RepTB' );"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptTrialBalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    '#End Region
    'Try
    '    Dim parm(3) As SqlParameter
    '    Dim i As Integer = 0

    '    'If validation() = False Then
    '    '    Exit Sub
    '    'End If

    '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
    '    mySqlCmd = New SqlCommand
    '    sqlTrans = mySqlConn.BeginTransaction()
    '    mySqlCmd.Connection = mySqlConn
    '    mySqlCmd.Transaction = sqlTrans
    '    mySqlCmd.CommandType = CommandType.StoredProcedure
    '    mySqlCmd.CommandText = "sp_mod_sealing_master"

    '    parm(0) = New SqlParameter("@sealdate", Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"))
    '    parm(1) = New SqlParameter("@adddate", objDate.GetSystemDateTime(Session("dbconnectionName")))
    '    parm(2) = New SqlParameter("@divcode", ViewState("divcode"))
    '    parm(3) = New SqlParameter("@adduser", CType(Session("GlobalUserName"), String))
    '    For i = 0 To 3
    '        mySqlCmd.Parameters.Add(parm(i))
    '    Next

    '    mySqlCmd.ExecuteNonQuery()
    '    sqlTrans.Commit()    'SQl Tarn Commit
    '    sqlTrans.Dispose()
    '    mySqlConn.Close()
    '    mySqlConn.Dispose()

    '    description()
    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('UnSealing completed');", True)

    'Catch ex As Exception
    '    If mySqlConn.State = ConnectionState.Open Then
    '        sqlTrans.Rollback()
    '    End If
    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '    objUtils.WritErrorLog("sealdata.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

    ' End Try

End Class
