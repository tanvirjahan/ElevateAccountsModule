Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.IO
Imports System.Collections.Generic


Partial Class RptSerialityControl
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUser As New clsUser
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
    Dim sqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region

#Region "Web Service Methods"
    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function GetRequestId(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim reqids As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select requestid from booking_header(nolock) where div_code='" & contextKey.Trim & "' and requestid like  '%" & prefixText & "%' order by requestid"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")         'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    reqids.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("requestid").ToString(), myDS.Tables(0).Rows(i)("requestid").ToString()))
                Next
            End If

            Return reqids
        Catch ex As Exception
            Return reqids
        End Try
    End Function
#End Region

#Region "Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit"
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Page.IsPostBack = False Then
            Dim appid As String = CType(Request.QueryString("appid"), String)
            If Request.QueryString("appid") Is Nothing = False Then
                Select Case appid
                    Case 1
                        Me.MasterPageFile = "~/PriceListMaster.master"
                    Case 2
                        Me.MasterPageFile = "~/RoomBlock.master"
                    Case 3
                        Me.MasterPageFile = "~/ReservationMaster.master"
                    Case 4
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 5
                        Me.MasterPageFile = "~/UserAdminMaster.master"
                    Case 6
                        Me.MasterPageFile = "~/WebAdminMaster.master"
                    Case 7
                        Me.MasterPageFile = "~/TransferHistoryMaster.master"
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 16
                        Me.MasterPageFile = "~/AccountsMaster.master"   '' Added shahul MCP accounts
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
            Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
            Dim strappname As String = ""
            strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(displayName,'') as displayname from appmaster a inner join division_master d on a.displayname=d.accountsmodulename where a.appid='" & appid & "'")
            ViewState("Appname") = strappname
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")
            ViewState.Add("divcode", divid)
        End If
    End Sub
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                Dim strappid As String = ""
                Dim strappname As String = ""

                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim AppId As String = CType(Request.QueryString("appid"), String)

                If AppId Is Nothing = False Then
                    strappid = AppId
                End If
                If AppName Is Nothing = False Then
                    strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(displayName,'') as displayname from appmaster a inner join division_master d on a.displayname=d.accountsmodulename where a.appid='" & AppId & "'")
                    If strappname = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Accounts display name does not match with accounts module name in division master' );", True)
                        Exit Sub
                    End If
                End If

                txtDivcode.Text = ViewState("divcode")

                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\RptSerialityControl.aspx?appid=" + strappid, btnAddNew, btnExcelReport, _
                                                       btnPdfReport, gvSearch:=gv_SearchResult)
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("RptSerialityControl.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Protected Sub btnExcelReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelReport.Click"
    Protected Sub btnExcelReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelReport.Click
        Try

            Dim strfromval As String = txtFromCode.Text.Trim
            Dim strToVal As String = txtToCode.Text.Trim
            Dim divid As String = txtDivcode.Text
            Dim reportsType As String = "excel"
            Dim bytes() As Byte
            bytes = {}
            Dim objSerialControl As clsSerialityControl = New clsSerialityControl()
            objSerialControl.GenerateReport(reportsType, strfromval, strToVal, divid, bytes, "download")
            Response.Clear()
            Response.Buffer = True
            Dim FileNameNew As String = "SerialityControl@" + DateTime.Now.ToString("ddMMyyyy@HHmmss") + ".xlsx"
            Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
            Response.AddHeader("Content-Length", bytes.Length.ToString())
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.BinaryWrite(bytes)
            Response.Cookies.Add(New HttpCookie("DownloadSeriality", "True"))
            Response.Flush()
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptCashBankBalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click"
    Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
        Try
            'If (txtFromCode.Text = "" And txtToCode.Text <> "") Or (txtFromCode.Text <> "" And txtToCode.Text = "") Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select request id ');", True)
            '    Exit Sub
            'End If

            Dim strfromval As String = txtFromCode.Text.Trim
            Dim strToVal As String = txtToCode.Text.Trim
            Dim divid As String = txtDivcode.Text
            Dim strpop As String = ""
            strpop = "window.open('TransactionReports.aspx?printId=SerialityControl&reportsType=pdf&fromVal=" & strfromval & "&toVal=" & strToVal & "&divid=" & divid & "','serialityControl')"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptSerialityControl.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

End Class
