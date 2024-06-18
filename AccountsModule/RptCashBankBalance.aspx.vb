
Partial Class RptCashBankBalance
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
#End Region

#Region "Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit"
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If IsPostBack = False Then
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

                txtDivcode.Value = ViewState("divcode")

                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\RptCashBankBalance.aspx?appid=" + strappid, btnAddNew, btnExcelReport, _
                                                       btnPdfReport, gvSearch:=gv_SearchResult)

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("RptCashBankBalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click"
    Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
        Try
            If Not IsDate(txtFromDate.Text) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select valid date');", True)
                Exit Sub
            End If
            Dim strfromdate As String = txtFromDate.Text.Trim
            Dim divid As String = txtDivcode.Value
            Dim strpop As String = ""
            strpop = "window.open('TransactionReports.aspx?printId=CashBankBalance&reportsType=pdf&fromdate=" & strfromdate & "&divid=" & divid & "','cashbankbalance')"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptCashBankBalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region

#Region "Protected Sub btnExcelReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelReport.Click"
    Protected Sub btnExcelReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelReport.Click
        Try
            If Not IsDate(txtFromDate.Text) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select valid date');", True)
                Exit Sub
            End If
            Dim strfromdate As String = txtFromDate.Text.Trim
            Dim divid As String = txtDivcode.Value
            Dim reportsType As String = "excel"
            Dim bytes() As Byte
            bytes = {}
            Dim objCashbankBal As ClsCashBankBalancePdf = New ClsCashBankBalancePdf()
            objCashbankBal.GenerateReport(reportsType, strfromdate, divid, bytes, "download")
            Response.Clear()
            Response.Buffer = True
            Dim FileNameNew As String = "CashBankBalance@" + DateTime.Now.ToString("ddMMyyyy@HHmmss") + ".xlsx"
            Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
            Response.AddHeader("Content-Length", bytes.Length.ToString())
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.BinaryWrite(bytes)
            Response.Flush()
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptCashBankBalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click"
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click

    End Sub
#End Region

End Class
