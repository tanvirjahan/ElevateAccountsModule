Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports ClosedXML.Excel
Imports System.IO
Imports System.Linq

Partial Class RptHotelProfitability
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
#End Region

#Region "Web Methods"

    <System.Web.Script.Services.ScriptMethod()> _
 <System.Web.Services.WebMethod()> _
    Public Shared Function GetCountryGroup(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim countryGroup As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select countrygroupcode,countrygroupname from countrygroup(nolock) where active=1 and countrygroupname like '%" & prefixText & "%' " &
            "order by countrygroupname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    countryGroup.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("countrygroupname").ToString(), myDS.Tables(0).Rows(i)("countrygroupcode").ToString()))
                Next
            End If
            Return countryGroup
        Catch ex As Exception
            Return countryGroup
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetCustomers(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim customers As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            Dim strdivcode As String = ""
            If contextKey.Trim <> "" Then
                strdivcode = " and divcode='" + contextKey + "'"
            End If
            strSqlQry = "select agentcode,agentname from agentmast where active=1 and agentname like '%" & prefixText & "%'" + strdivcode + " order by agentname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    customers.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("agentcode").ToString()))
                Next
            End If
            Return customers
        Catch ex As Exception
            Return customers
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetAccounts(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Accts As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            Dim strdivcode As String = ""
            If contextKey.Trim <> "" Then
                strdivcode = contextKey
            Else
                strdivcode = "01"
            End If
            strSqlQry = "select acctcode,ltrim(rtrim(acctname)) acctname,acctorder from view_acctmast(nolock) where div_code='" + strdivcode + "' and (acctorder= 3 or acctorder=4) and  acctname like  '%" & Trim(prefixText) & "%'  order by acctname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Accts.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString() & "|" & myDS.Tables(0).Rows(i)("acctorder").ToString()))
                Next
            End If
            Return Accts
        Catch ex As Exception
            Return Accts
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetSalesPersons(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim sperson As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select userCode,ltrim(rtrim(UserName)) UserName from usermaster(nolock) where active=1 and userName like  '%" & Trim(prefixText) & "%'  order by userName"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    sperson.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("UserName").ToString(), myDS.Tables(0).Rows(i)("Usercode").ToString()))
                Next
            End If
            Return sperson
        Catch ex As Exception
            Return sperson
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetSuppliers(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppliers As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select partycode,partyname from partymast(nolock) where active=1 and partyname like '%" & prefixText & "%' order by partyname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    suppliers.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))
                Next
            End If
            Return suppliers
        Catch ex As Exception
            Return suppliers
        End Try
    End Function
#End Region

#Region "Protected Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit"
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Page.IsPostBack = False Then
            Dim appid As String = ""
            If Request.QueryString("appid") Is Nothing = False Then
                appid = CType(Request.QueryString("appid"), String)
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

#Region "Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then

                If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                Dim strappid As String = ""
                Dim strappname As String = ""

                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)

                If AppId.Value Is Nothing = False Then
                    strappid = AppId.Value
                End If
                strappname = objUser.GetAppName(Session("dbconnectionName"), strappid)

                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\rptHotelProfitability.aspx?appid=" + strappid.Trim, btnAddNew, btnLoadReport, _
                                                       btnPrint, gvSearch:=gvSearchResult)
                txtDivcode.Text = ViewState("divcode")

                'ddlGroupBy.Items.Add(New ListItem("Nationality", "Nationality"))
                'ddlGroupBy.Items.Add(New ListItem("Agent", "Customer"))
                'ddlGroupBy.Items.Add(New ListItem("Account", "Account"))
                'ddlGroupBy.Items.Add(New ListItem("Sales Person", "Sales Person"))
                'ddlGroupBy.SelectedIndex = 0

                ddlBookingType.Items.Add(New ListItem("All", "All"))
                ddlBookingType.Items.Add(New ListItem("Hotel", "Hotel"))
                ddlBookingType.Items.Add(New ListItem("Airport MA", "Airport MA"))
                ddlBookingType.Items.Add(New ListItem("Transfers", "Transfers"))
                ddlBookingType.Items.Add(New ListItem("Tours", "Tours"))
                ddlBookingType.Items.Add(New ListItem("Visa", "Visa"))
                ddlBookingType.Items.Add(New ListItem("Others", "Others"))
                ddlBookingType.SelectedIndex = 0
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptHotalProfitability.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnLoadReport_Click(sender As Object, e As System.EventArgs) Handles btnLoadReport.Click"
    Protected Sub btnLoadReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadReport.Click
        Try
            If Not IsDate(txtFromDt.Text) Then
                ModalPopupLoading.Hide()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Check from date" & "' );", True)
                Exit Sub
            End If
            If Not IsDate(txtToDt.Text) Then
                ModalPopupLoading.Hide()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Check to date" & "' );", True)
                Exit Sub
            End If

            If Convert.ToDateTime(txtFromDt.Text) > Convert.ToDateTime(txtToDt.Text) Then
                ModalPopupLoading.Hide()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "To date should be greater than from date" & "' );", True)
                Exit Sub
            End If
            Dim fromDate As String = Convert.ToDateTime(txtFromDt.Text).ToString("dd/MM/yyyy")
            Dim toDate As String = Convert.ToDateTime(txtToDt.Text).ToString("dd/MM/yyyy")
            Dim ctryGroupCode As String = ""
            If txtCtryGroup.Text.Trim <> "" And txtCtryGroupCode.Text.Trim <> "" Then
                ctryGroupCode = txtCtryGroupCode.Text.Trim
            End If
            Dim customerCode As String = ""
            If txtCust.Text.Trim <> "" And txtCustCode.Text.Trim <> "" Then
                customerCode = txtCustCode.Text.Trim
            End If
            Dim acctCode As String = ""
            Dim AcctOrder As Integer = Nothing
            If txtAcct.Text.Trim <> "" And txtAcctCode.Text.Trim <> "" Then
                Dim accts() As String = txtAcctCode.Text.Trim.Split("|")
                If accts.Count = 2 Then
                    acctCode = accts(0)
                    AcctOrder = accts(1)
                Else
                    acctCode = ""
                    AcctOrder = Nothing
                End If
            End If
            Dim salesPersonCode As String = ""
            If txtSalePerson.Text.Trim <> "" And txtSalePersonCode.Text.Trim <> "" Then
                salesPersonCode = txtSalePersonCode.Text.Trim
            End If
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myCommand = New SqlCommand("sp_rep_profitability_hoteltable", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            If IsDate(txtFromDt.Text) Then
                myCommand.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDt.Text).ToString("yyyy/MM/dd")
            Else
                myCommand.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = DBNull.Value
            End If
            If IsDate(txtToDt.Text) Then
                myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDt.Text).ToString("yyyy/MM/dd")
            Else
                myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = DBNull.Value
            End If
            myCommand.Parameters.Add(New SqlParameter("@countrygroup", SqlDbType.VarChar, 20)).Value = ctryGroupCode
            myCommand.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = customerCode
            myCommand.Parameters.Add(New SqlParameter("@acctcode", SqlDbType.VarChar, 20)).Value = acctCode
            myCommand.Parameters.Add(New SqlParameter("@acctorder", SqlDbType.Int)).Value = AcctOrder
            myCommand.Parameters.Add(New SqlParameter("@salesperson", SqlDbType.VarChar, 20)).Value = salesPersonCode
            myCommand.Parameters.Add(New SqlParameter("@groupby", SqlDbType.VarChar, 20)).Value = "Nationality"      ' Convert.ToString(ddlGroupBy.SelectedValue)
            myCommand.Parameters.Add(New SqlParameter("@bookingservice", SqlDbType.VarChar, 20)).Value = Convert.ToString(ddlBookingType.SelectedValue)
            myCommand.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = txtDivcode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = txtPartyCode.Text.Trim
            myDataAdapter = New SqlDataAdapter(myCommand)
            Dim myDs As New DataSet()
            myDataAdapter.Fill(myDs)
            Dim resultDt As DataTable = myDs.Tables(0)
            Dim TotalDt As DataTable = myDs.Tables(1)
            Dim InvDt As DataTable = myDs.Tables(2)
            Dim NonInvDt As DataTable = myDs.Tables(3)
            Dim gopDt As DataTable = myDs.Tables(4)
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)
            DetailsReport(resultDt, TotalDt, InvDt, NonInvDt, gopDt)
        Catch ex As Exception
            ModalPopupLoading.Hide()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptHotalProfitability.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub btnReset_Click(sender As Object, e As System.EventArgs) Handles btnReset.Click"
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        txtFromDt.Text = ""
        txtToDt.Text = ""
        txtCtryGroup.Text = ""
        txtCtryGroupCode.Text = ""
        txtCust.Text = ""
        txtCustCode.Text = ""
        txtAcct.Text = ""
        txtAcctCode.Text = ""
        txtSalePerson.Text = ""
        txtSalePersonCode.Text = ""
        txtParty.Text = ""
        txtPartyCode.Text = ""
        'ddlGroupBy.SelectedIndex = 0
        ddlBookingType.SelectedIndex = 0
        txtFromDt.Focus()
    End Sub
#End Region

#Region "Protected Sub DetailsReport(ByVal resultDt As DataTable, ByVal TotalDt As DataTable, ByVal InvDt As DataTable, ByVal NonInvDt As DataTable, ByVal gopDt As DataTable)"
    Protected Sub DetailsReport(ByVal resultDt As DataTable, ByVal TotalDt As DataTable, ByVal InvDt As DataTable, ByVal NonInvDt As DataTable, ByVal gopDt As DataTable)
        Try
            If resultDt.Rows.Count > 0 Then
                'Dim highlightRow = (From n In resultDt.AsEnumerable Where n.Field(Of Integer)("highlightcost") = 1 Select n)
                'If highlightRow.Count > 0 Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Costs have not been updated for other services in the yellow marked Booking' );", True)
                'End If
                Dim excelLastRow As Integer
                Dim wbook As XLWorkbook = New XLWorkbook
                Dim companyname As String
                Dim wsprofit As IXLWorksheet = wbook.Worksheets.Add("HotelProfitabilityReport")
                wsprofit.PageSetup.PaperSize = XLPaperSize.A4Paper

                wsprofit.Style.Font.SetFontName("Trebuchet MS")

                If txtDivcode.Text.Trim <> "" Then
                    companyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & txtDivcode.Text.Trim & "'"), String)
                Else
                    companyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
                End If
                Dim cols As String = "L"
                Dim titlecolor As XLColor = XLColor.FromArgb(0, 72, 145)
                excelLastRow = 1
                Dim rngTitle As IXLRange
                rngTitle = wsprofit.Range("A" + Convert.ToString(excelLastRow) + ":" + cols + Convert.ToString(excelLastRow))
                rngTitle.Merge()
                wsprofit.Cell(excelLastRow, 1).Value = companyname
                wsprofit.Cell(excelLastRow, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                wsprofit.Cell(excelLastRow, 1).Style.Font.SetBold(True).Font.SetFontSize(14)
                wsprofit.Cell(excelLastRow, 1).Style.Fill.SetBackgroundColor(titlecolor)
                wsprofit.Cell(excelLastRow, 1).Style.Font.SetFontColor(XLColor.Ivory)

                excelLastRow = 2
                Dim rngap As IXLRange
                rngap = wsprofit.Range("A" + Convert.ToString(excelLastRow) + ":" + cols + Convert.ToString(excelLastRow))
                rngap.Merge()
                wsprofit.Row(excelLastRow).Height = 5

                Dim subtitlecolor As XLColor = XLColor.FromArgb(0, 128, 192)
                Dim accttitlecolor As XLColor = XLColor.FromArgb(153, 227, 253)
                excelLastRow = 3
                Dim rngSubTitle As IXLRange
                rngSubTitle = wsprofit.Range("A" + Convert.ToString(excelLastRow) + ":" + cols + Convert.ToString(excelLastRow))
                rngSubTitle.Merge()
                wsprofit.Cell(excelLastRow, 1).Value = "Hotel Profitability Report"
                wsprofit.Cell(excelLastRow, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                wsprofit.Cell(excelLastRow, 1).Style.Font.SetBold(True).Font.SetFontSize(12)
                wsprofit.Cell(excelLastRow, 1).Style.Fill.SetBackgroundColor(subtitlecolor)
                wsprofit.Cell(excelLastRow, 1).Style.Font.SetFontColor(XLColor.Ivory)

                excelLastRow = 4
                rngap = wsprofit.Range("A" + Convert.ToString(excelLastRow) + ":" + cols + Convert.ToString(excelLastRow))
                rngap.Merge()
                wsprofit.Row(excelLastRow).Height = 5

                excelLastRow = 5
                Dim rngDtTitle As IXLRange
                rngDtTitle = wsprofit.Range("A" + Convert.ToString(excelLastRow) + ":" + cols + Convert.ToString(excelLastRow + 1))
                rngDtTitle.Merge()
                wsprofit.Cell(excelLastRow, 1).Value = "From Date :" + Convert.ToDateTime(txtFromDt.Text).ToString("dd/MM/yyyy") + " - To Date :" + Convert.ToDateTime(txtToDt.Text).ToString("dd/MM/yyyy")
                wsprofit.Cell(excelLastRow, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                wsprofit.Cell(excelLastRow, 1).Style.Font.SetBold(True).Font.SetFontSize(10)

                excelLastRow = 7

                'If ddlGroupBy.SelectedValue <> "Account" Then
                MakeDetailExcel(wsprofit, excelLastRow, resultDt, TotalDt, InvDt, NonInvDt, gopDt)
                'Else
                '    Dim acctCode As String = ""
                '    Dim acctOrder As Integer = 0
                '    If txtAcctCode.Text.Trim <> "" Then
                '        Dim codes() As String = txtAcctCode.Text.Trim.Split("|")
                '        acctCode = codes(0)
                '        acctOrder = codes(1)
                '    End If
                '    Dim filterIncome = (From n In resultDt.AsEnumerable Where n.Field(Of String)("accountType") = "Income" Select n)
                '    If filterIncome.Count > 0 And acctOrder <> 4 Then
                '        Dim incomeDt As DataTable = filterIncome.CopyToDataTable()
                '        Dim filterTotalDt As DataTable = (From n In TotalDt.AsEnumerable Where n.Field(Of String)("accountType") = "Income" Select n).CopyToDataTable()
                '        Dim filterInvDt As DataTable = (From n In InvDt.AsEnumerable Where n.Field(Of String)("accountType") = "Income" Select n).CopyToDataTable()
                '        Dim filterNonInv = (From n In NonInvDt.AsEnumerable Where n.Field(Of String)("accountType") = "Income" Select n)
                '        Dim filterNonInvDt As New DataTable
                '        If filterNonInv.Count > 0 Then
                '            filterNonInvDt = filterNonInv.CopyToDataTable()
                '        End If
                '        Dim filterGopDt As DataTable = (From n In gopDt.AsEnumerable Where n.Field(Of String)("accountType") = "Income" Select n).CopyToDataTable()
                '        Dim rngIncomeTitle As IXLRange
                '        rngIncomeTitle = wsprofit.Range("A" + Convert.ToString(excelLastRow) + ":H" + Convert.ToString(excelLastRow))
                '        rngIncomeTitle.Merge()
                '        wsprofit.Cell(excelLastRow, 1).Value = "Income Accounts"
                '        wsprofit.Cell(excelLastRow, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                '        wsprofit.Cell(excelLastRow, 1).Style.Font.SetBold(True).Font.SetFontSize(12)
                '        wsprofit.Cell(excelLastRow, 1).Style.Fill.SetBackgroundColor(accttitlecolor)
                '        wsprofit.Cell(excelLastRow, 1).Style.Font.SetFontColor(XLColor.Black)
                '        excelLastRow += 1
                '        MakeDetailExcel(wsprofit, excelLastRow, incomeDt, filterTotalDt, filterInvDt, filterNonInvDt, filterGopDt, "Income")
                '    End If

                '    Dim filterCostSale = (From n In resultDt.AsEnumerable Where n.Field(Of String)("accountType") = "CostOfSale" Select n)
                '    If filterCostSale.Count > 0 And acctOrder <> 3 Then
                '        Dim CostSaleDt As DataTable = filterCostSale.CopyToDataTable()
                '        Dim filterTotalDt As DataTable = (From n In TotalDt.AsEnumerable Where n.Field(Of String)("accountType") = "CostOfSale" Select n).CopyToDataTable()
                '        Dim filterInvDt As DataTable = (From n In InvDt.AsEnumerable Where n.Field(Of String)("accountType") = "CostOfSale" Select n).CopyToDataTable()
                '        Dim filterNonInv = (From n In NonInvDt.AsEnumerable Where n.Field(Of String)("accountType") = "CostOfSale" Select n)
                '        Dim filterNonInvDt As New DataTable
                '        If filterNonInv.Count > 0 Then
                '            filterNonInvDt = filterNonInv.CopyToDataTable()
                '        End If
                '        Dim filterGopDt As DataTable = (From n In gopDt.AsEnumerable Where n.Field(Of String)("accountType") = "CostOfSale" Select n).CopyToDataTable()
                '        Dim rngCostTitle As IXLRange
                '        rngCostTitle = wsprofit.Range("A" + Convert.ToString(excelLastRow) + ":H" + Convert.ToString(excelLastRow))
                '        rngCostTitle.Merge()
                '        wsprofit.Cell(excelLastRow, 1).Value = "Cost Of Sales Accounts"
                '        wsprofit.Cell(excelLastRow, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                '        wsprofit.Cell(excelLastRow, 1).Style.Font.SetBold(True).Font.SetFontSize(12)
                '        wsprofit.Cell(excelLastRow, 1).Style.Fill.SetBackgroundColor(accttitlecolor)
                '        wsprofit.Cell(excelLastRow, 1).Style.Font.SetFontColor(XLColor.Black)
                '        excelLastRow += 1
                '        MakeDetailExcel(wsprofit, excelLastRow, CostSaleDt, filterTotalDt, filterInvDt, filterNonInvDt, filterGopDt, "CostOfSale")
                '    End If
                'End If

                wsprofit.Columns("1:3").AdjustToContents()
                wsprofit.Columns("5:8").Width = 8
                wsprofit.Columns("9:12").Width = 15
                wsprofit.Columns("9:10").Style.Alignment.SetWrapText(True)

                Dim FileNameNew As String = "HotelProfitPrint_" & Now.Year & Now.Month.ToString("00") & Now.Day.ToString("00") & Now.Hour & Now.Minute & Now.Second & ".xlsx"
                Using MyMemoryStream As New MemoryStream()
                    wbook.SaveAs(MyMemoryStream)
                    wbook.Dispose()
                    Response.Clear()
                    Response.Buffer = True
                    Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
                    Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Cookies.Add(New HttpCookie("DownloadHotelProfit", "True"))
                    Response.Flush()
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                End Using
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Records not found, Please redefine search criteria');", True)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Protected Sub MakeDetailExcel(ByRef wsprofit As IXLWorksheet, ByRef excelLastRow As Integer, ByVal resultDt As DataTable, ByVal TotalDt As DataTable, ByVal InvDt As DataTable, ByVal NonInvDt As DataTable, ByVal gopDt As DataTable, Optional ByVal accountType As String = "")"
    Protected Sub MakeDetailExcel(ByRef wsprofit As IXLWorksheet, ByRef excelLastRow As Integer, ByVal resultDt As DataTable, ByVal TotalDt As DataTable, ByVal InvDt As DataTable, ByVal NonInvDt As DataTable, ByVal gopDt As DataTable, Optional ByVal accountType As String = "")
        Dim decno As Integer = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), Integer)
        Dim decPlaces As String
        If decno = 3 Then
            decPlaces = "#,##0.000;[Red](#,##0.000)"
        Else
            decPlaces = "#,##0.00;[Red](#,##0.00)"
        End If
        
        Dim runningRow As Integer = excelLastRow
        Dim cols As String = "L"
        Dim highlightRow = (From n In resultDt.AsEnumerable Where n.Field(Of Integer)("highlightcost") = 1 Select n)
        If highlightRow.Count > 0 Then
            Dim rnghighlight As IXLRange = wsprofit.Range("A" + runningRow.ToString + ":" + cols + runningRow.ToString)
            rnghighlight.Merge().Style.Fill.SetBackgroundColor(XLColor.FromArgb(234, 247, 148)).Font.SetBold(True)
            rnghighlight.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(10)
            rnghighlight.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin)
            wsprofit.Cell(runningRow, 1).Value = "Note : Costs have not been updated for other services in the yellow marked Booking"
            runningRow += 1
            excelLastRow = runningRow
        End If
        Dim totAdult As Integer = 0
        Dim totChild As Integer = 0
        Dim totPax As Integer = 0
        Dim totNights As Integer = 0
        Dim TblTitle() As String = {"Invoice No.", "Invoice Date", "Request ID", "Agent Name", "Adult", "Child", "Pax", "Nights", "Sales Value" + vbCr + "Excl. VAT", "Cost Value" + vbCr + "Excl. VAT", "Profit", "%"}
        For i = 0 To TblTitle.GetUpperBound(0)
            wsprofit.Cell(runningRow, i + 1).Value = TblTitle(i)
        Next
        wsprofit.Row(runningRow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(True)
        Dim groupDt As New DataTable
        Dim groupDr = (From n In resultDt.AsEnumerable() Order By n.Field(Of String)("partyname") Group By groupby = n.Field(Of String)("partycode").Trim().ToUpper() Into group = Group Select groupby)
        If groupDr.Count > 0 Then
            For i = 0 To groupDr.Count - 1
                Dim filterId As String = Convert.ToString(groupDr(i))
                Dim filterName As String = (From n In resultDt.AsEnumerable() Where n.Field(Of String)("partycode").ToUpper() = filterId Select n.Field(Of String)("partyname")).Distinct.FirstOrDefault()
                runningRow = runningRow + 1
                Dim rnggrpTitle As IXLRange
                rnggrpTitle = wsprofit.Range("A" + Convert.ToString(runningRow) + ":" + cols + Convert.ToString(runningRow))
                rnggrpTitle.Merge().Style.Font.SetBold(True)
                rnggrpTitle.Style.Fill.SetBackgroundColor(XLColor.LightGray)
                wsprofit.Cell(runningRow, 1).Value = filterName
                Dim filterDt As DataTable = (From n In resultDt.AsEnumerable() Where n.Field(Of String)("partycode").ToUpper() = filterId Select n).CopyToDataTable()
                Dim tmpSalevalue As Decimal = 0
                Dim tmpcostvalue As Decimal = 0
                Dim tmpAdult As Integer = 0
                Dim tmpChild As Integer = 0
                Dim tmpPax As Integer = 0
                Dim tmpNights As Integer = 0
                For Each filterDr As DataRow In filterDt.Rows
                    Dim ColNo As Integer = 0
                    runningRow = runningRow + 1
                    ColNo = 1
                    wsprofit.Cell(runningRow, ColNo).Value = filterDr("invoiceno")

                    ColNo = ColNo + 1
                    wsprofit.Cell(runningRow, ColNo).Style.NumberFormat.Format = "dd/MM/yyyy"
                    wsprofit.Cell(runningRow, ColNo).DataType = XLCellValues.Text
                    wsprofit.Cell(runningRow, ColNo).Value = filterDr("checkout")

                    ColNo = ColNo + 1
                    wsprofit.Cell(runningRow, ColNo).Value = filterDr("requestid")

                    ColNo = ColNo + 1
                    wsprofit.Column(ColNo).Width = 30   'agentname
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(True)
                    wsprofit.Cell(runningRow, ColNo).Value = filterDr("agentname")

                    ColNo = ColNo + 1
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    wsprofit.Cell(runningRow, ColNo).Value = filterDr("adult")

                    ColNo = ColNo + 1
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    wsprofit.Cell(runningRow, ColNo).Value = filterDr("child")

                    ColNo = ColNo + 1
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    wsprofit.Cell(runningRow, ColNo).Value = filterDr("pax")

                    ColNo = ColNo + 1
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    wsprofit.Cell(runningRow, ColNo).Value = filterDr("nights")

                    ColNo = ColNo + 1
                    If accountType <> "CostOfSale" Then
                        wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        wsprofit.Cell(runningRow, ColNo).Value = Math.Round(Convert.ToDecimal(filterDr("salevalue")), decno)
                    End If

                    ColNo = ColNo + 1
                    If accountType <> "Income" Then
                        wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        wsprofit.Cell(runningRow, ColNo).Value = Math.Round(Convert.ToDecimal(filterDr("costvalue")), decno)
                    End If

                    If accountType = "" Then
                        ColNo = ColNo + 1
                        wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        wsprofit.Cell(runningRow, ColNo).Value = Math.Round(Convert.ToDecimal(filterDr("profit")), decno)

                        ColNo = ColNo + 1
                        wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        wsprofit.Cell(runningRow, ColNo).Value = Math.Round(Convert.ToDecimal(filterDr("percentage")), 2)
                    End If

                    If Convert.ToInt32(filterDr("highlightcost")) = 1 Then
                        wsprofit.Range("A" + runningRow.ToString + ":" + cols + runningRow.ToString).Style.Fill.SetBackgroundColor(XLColor.FromArgb(234, 247, 148))
                    End If
                    tmpAdult += filterDr("adult")
                    tmpChild += filterDr("child")
                    tmpPax += filterDr("pax")
                    tmpNights += filterDr("nights")
                    tmpSalevalue += filterDr("salevalue")
                    tmpcostvalue += filterDr("costvalue")

                    wsprofit.Range("I" + runningRow.ToString + ":K" + runningRow.ToString).Style.NumberFormat.Format = decPlaces
                    wsprofit.Range("L" + runningRow.ToString + ":L" + runningRow.ToString).Style.NumberFormat.Format = "0.00;[Red](0.00)"
                Next
                runningRow = runningRow + 1
                wsprofit.Range("A" + runningRow.ToString + ":D" + runningRow.ToString).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold(True)
                wsprofit.Cell(runningRow, 1).Value = "Sub Total :  "
                wsprofit.Cell(runningRow, 5).Value = tmpAdult.ToString()
                wsprofit.Cell(runningRow, 6).Value = tmpChild.ToString()
                wsprofit.Cell(runningRow, 7).Value = tmpPax.ToString()
                wsprofit.Cell(runningRow, 8).Value = tmpNights.ToString()
                If accountType <> "CostOfSale" Then wsprofit.Cell(runningRow, 9).Value = Math.Round(tmpSalevalue, decno)
                If accountType <> "Income" Then wsprofit.Cell(runningRow, 10).Value = Math.Round(tmpcostvalue, decno)
                If accountType = "" Then
                    wsprofit.Cell(runningRow, 11).Value = Math.Round(tmpSalevalue - tmpcostvalue, decno)
                    If tmpSalevalue = 0 Then
                        wsprofit.Cell(runningRow, 12).Value = 0
                    Else
                        wsprofit.Cell(runningRow, 12).Value = Math.Round((tmpSalevalue - tmpcostvalue) / tmpSalevalue * 100, 2)
                    End If
                End If
                totAdult += tmpAdult
                totChild += tmpChild
                totPax += tmpPax
                totNights += tmpNights
                wsprofit.Range("E" + runningRow.ToString + ":H" + runningRow.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(True)
                wsprofit.Range("I" + runningRow.ToString + ":K" + runningRow.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold(True).NumberFormat.Format = decPlaces
                wsprofit.Range("L" + runningRow.ToString + ":L" + runningRow.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold(True).NumberFormat.Format = "0.00;[Red](0.00)"
            Next
            If TotalDt.Rows.Count > 0 Then
                Dim totalDr As DataRow = TotalDt.Rows(0)
                runningRow = runningRow + 1
                wsprofit.Range("A" + runningRow.ToString + ":D" + runningRow.ToString).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold(True)
                wsprofit.Cell(runningRow, 1).Value = "Total :  "
                wsprofit.Cell(runningRow, 5).Value = totAdult.ToString()
                wsprofit.Cell(runningRow, 6).Value = totChild.ToString()
                wsprofit.Cell(runningRow, 7).Value = totPax.ToString()
                wsprofit.Cell(runningRow, 8).Value = totNights.ToString()
                If accountType <> "CostOfSale" Then wsprofit.Cell(runningRow, 9).Value = Math.Round(totalDr("salevalue"), decno)
                If accountType <> "Income" Then wsprofit.Cell(runningRow, 10).Value = Math.Round(totalDr("costvalue"), decno)
                If accountType = "" Then
                    wsprofit.Cell(runningRow, 11).Value = Math.Round(totalDr("profit"), decno)
                    wsprofit.Cell(runningRow, 12).Value = Math.Round(totalDr("percentage"), 2)
                End If
                wsprofit.Range("E" + runningRow.ToString + ":H" + runningRow.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(True)
                wsprofit.Range("I" + runningRow.ToString + ":K" + runningRow.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold(True).NumberFormat.Format = decPlaces
                wsprofit.Range("L" + runningRow.ToString + ":L" + runningRow.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold(True).NumberFormat.Format = "0.00;[Red](0.00)"
            End If
            Dim tblRange As IXLRange
            tblRange = wsprofit.Range("A" + Convert.ToString(excelLastRow) + ":" + cols + Convert.ToString(runningRow))
            tblRange.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(10)
            tblRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin)
        End If

        'Invoice summary
        If InvDt.Rows.Count > 0 Then
            runningRow += 3
            excelLastRow = runningRow
            Dim rngInvTitle As IXLRange
            rngInvTitle = wsprofit.Range("D" + Convert.ToString(runningRow) + ":" + cols + Convert.ToString(runningRow))
            rngInvTitle.Merge()
            wsprofit.Cell(runningRow, 4).Value = "Hotel Summary (With Invoices)"
            wsprofit.Cell(runningRow, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            wsprofit.Cell(runningRow, 4).Style.Font.SetBold(True).Font.SetFontSize(12)

            runningRow = runningRow + 1
            Dim TblInvTitle() As String = {"Hotel Name", "Sales Value" + vbCr + "Excl. VAT", "Cost Value" + vbCr + "Excl. VAT", "Profit", "%"}
            For i = 0 To TblInvTitle.GetUpperBound(0)
                If i = 0 Then
                    wsprofit.Cell(runningRow, i + 4).Value = TblInvTitle(i)
                    wsprofit.Range("D" + Convert.ToString(runningRow) + ":H" + Convert.ToString(runningRow)).Merge()
                Else
                    wsprofit.Cell(runningRow, i + 8).Value = TblInvTitle(i)
                End If
            Next
            wsprofit.Row(runningRow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(True)

            Dim tmpSalevalue As Decimal = 0
            Dim tmpcostvalue As Decimal = 0
            For Each invDr As DataRow In InvDt.Rows
                Dim ColNo As Integer = 0
                runningRow = runningRow + 1
                ColNo = 4
                wsprofit.Cell(runningRow, ColNo).Value = invDr("groupname")
                wsprofit.Range("D" + Convert.ToString(runningRow) + ":H" + Convert.ToString(runningRow)).Merge().Style.Alignment.WrapText = True
                ColNo = ColNo + 5
                If accountType <> "CostOfSale" Then
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    wsprofit.Cell(runningRow, ColNo).Value = Math.Round(Convert.ToDecimal(invDr("salevalue")), decno)
                End If

                ColNo = ColNo + 1
                If accountType <> "Income" Then
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    wsprofit.Cell(runningRow, ColNo).Value = Math.Round(Convert.ToDecimal(invDr("costvalue")), decno)
                End If

                If accountType = "" Then
                    ColNo = ColNo + 1
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    wsprofit.Cell(runningRow, ColNo).Value = Math.Round(Convert.ToDecimal(invDr("profit")), decno)

                    ColNo = ColNo + 1
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    wsprofit.Cell(runningRow, ColNo).Value = Math.Round(Convert.ToDecimal(invDr("percentage")), 2)
                End If

                tmpSalevalue += invDr("salevalue")
                tmpcostvalue += invDr("costvalue")
            Next
            runningRow = runningRow + 1
            'wsprofit.Cell(runningRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold(True)
            wsprofit.Range("D" + Convert.ToString(runningRow) + ":H" + Convert.ToString(runningRow)).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold(True)
            wsprofit.Cell(runningRow, 4).Value = "Total :  "

            If accountType <> "CostOfSale" Then wsprofit.Cell(runningRow, 9).Value = Math.Round(tmpSalevalue, decno)
            If accountType <> "Income" Then wsprofit.Cell(runningRow, 10).Value = Math.Round(tmpcostvalue, decno)
            If accountType = "" Then
                wsprofit.Cell(runningRow, 11).Value = Math.Round(tmpSalevalue - tmpcostvalue, decno)
                If tmpSalevalue = 0 Then
                    wsprofit.Cell(runningRow, 12).Value = 0
                Else
                    wsprofit.Cell(runningRow, 12).Value = Math.Round((tmpSalevalue - tmpcostvalue) / tmpSalevalue * 100, 2)
                End If
            End If
            wsprofit.Range("I" + runningRow.ToString + ":" + cols + runningRow.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold(True)
            Dim rnginv As IXLRange = wsprofit.Range("I" + Convert.ToString(excelLastRow + 2) + ":K" + Convert.ToString(runningRow))
            rnginv.DataType = XLCellValues.Number
            rnginv.Style.NumberFormat.Format = decPlaces
            wsprofit.Range("L" + Convert.ToString(excelLastRow + 2) + ":" + cols + Convert.ToString(runningRow)).Style.NumberFormat.Format = "##0.00;[Red](##0.00)"

            Dim tblRange As IXLRange
            tblRange = wsprofit.Range("D" + Convert.ToString(excelLastRow) + ":" + cols + Convert.ToString(runningRow))
            tblRange.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(10)
            tblRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin)
        End If

        'Without invoice summary                
        If NonInvDt.Rows.Count > 0 Then
            runningRow += 3
            excelLastRow = runningRow
            Dim rngNonInvTitle As IXLRange
            rngNonInvTitle = wsprofit.Range("D" + Convert.ToString(runningRow) + ":" + cols + Convert.ToString(runningRow))
            rngNonInvTitle.Merge()
            wsprofit.Cell(runningRow, 4).Value = "Hotel Summary (Without Invoices)"
            wsprofit.Cell(runningRow, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            wsprofit.Cell(runningRow, 4).Style.Font.SetBold(True).Font.SetFontSize(12)

            runningRow = runningRow + 1
            Dim TblNonInvTitle() As String = {"Hotel Name", "Sales Value" + vbCr + "Excl. VAT", "Cost Value" + vbCr + "Excl. VAT", "Profit", "%"}
            For i = 0 To TblNonInvTitle.GetUpperBound(0)
                If i = 0 Then
                    wsprofit.Cell(runningRow, i + 4).Value = TblNonInvTitle(i)
                    wsprofit.Range("D" + Convert.ToString(runningRow) + ":H" + Convert.ToString(runningRow)).Merge()
                Else
                    wsprofit.Cell(runningRow, i + 8).Value = TblNonInvTitle(i)
                End If
            Next
            wsprofit.Row(runningRow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(True)

            Dim tmpSalevalue As Decimal = 0
            Dim tmpcostvalue As Decimal = 0
            For Each NonInvDr As DataRow In NonInvDt.Rows
                Dim ColNo As Integer = 0
                runningRow = runningRow + 1
                ColNo = 4
                wsprofit.Cell(runningRow, ColNo).Value = NonInvDr("groupname")
                wsprofit.Range("D" + Convert.ToString(runningRow) + ":H" + Convert.ToString(runningRow)).Merge().Style.Alignment.WrapText = True
                ColNo = ColNo + 5
                If accountType <> "CostOfSale" Then
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    wsprofit.Cell(runningRow, ColNo).Value = Math.Round(Convert.ToDecimal(NonInvDr("salevalue")), decno)
                End If

                ColNo = ColNo + 1
                If accountType <> "Income" Then
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    wsprofit.Cell(runningRow, ColNo).Value = Math.Round(Convert.ToDecimal(NonInvDr("costvalue")), decno)
                End If

                If accountType = "" Then
                    ColNo = ColNo + 1
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    wsprofit.Cell(runningRow, ColNo).Value = Math.Round(Convert.ToDecimal(NonInvDr("profit")), decno)

                    ColNo = ColNo + 1
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    wsprofit.Cell(runningRow, ColNo).Value = Math.Round(Convert.ToDecimal(NonInvDr("percentage")), 2)
                End If

                tmpSalevalue += NonInvDr("salevalue")
                tmpcostvalue += NonInvDr("costvalue")
            Next
            runningRow = runningRow + 1
            wsprofit.Range("D" + Convert.ToString(runningRow) + ":H" + Convert.ToString(runningRow)).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold(True)
            wsprofit.Cell(runningRow, 4).Value = "Total :  "
            If accountType <> "CostOfSale" Then wsprofit.Cell(runningRow, 9).Value = Math.Round(tmpSalevalue, decno)
            If accountType <> "Income" Then wsprofit.Cell(runningRow, 10).Value = Math.Round(tmpcostvalue, decno)
            If accountType = "" Then
                wsprofit.Cell(runningRow, 11).Value = Math.Round(tmpSalevalue - tmpcostvalue, decno)
                If tmpSalevalue = 0 Then
                    wsprofit.Cell(runningRow, 12).Value = 0
                Else
                    wsprofit.Cell(runningRow, 12).Value = Math.Round((tmpSalevalue - tmpcostvalue) / tmpSalevalue * 100, 2)
                End If
            End If
            wsprofit.Range("I" + runningRow.ToString + ":" + cols + runningRow.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold(True)
            Dim rngnoninv As IXLRange = wsprofit.Range("I" + Convert.ToString(excelLastRow + 2) + ":K" + Convert.ToString(runningRow))
            rngnoninv.DataType = XLCellValues.Number
            rngnoninv.Style.NumberFormat.Format = decPlaces
            wsprofit.Range("L" + Convert.ToString(excelLastRow + 2) + ":L" + Convert.ToString(runningRow)).Style.NumberFormat.Format = "##0.00;[Red](##0.00)"

            Dim tblRange As IXLRange
            tblRange = wsprofit.Range("D" + Convert.ToString(excelLastRow) + ":" + cols + Convert.ToString(runningRow))
            tblRange.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(10)
            tblRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin)
        End If

        'Net GOP
        'If ddlGroupBy.SelectedValue <> "Account" Then
        If gopDt.Rows.Count > 0 Then
            runningRow += 3
            excelLastRow = runningRow
            Dim rnggopTitle As IXLRange
            rnggopTitle = wsprofit.Range("D" + Convert.ToString(runningRow) + ":" + cols + Convert.ToString(runningRow))
            rnggopTitle.Merge()
            wsprofit.Cell(runningRow, 4).Value = "Net GOP"
            wsprofit.Cell(runningRow, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            wsprofit.Cell(runningRow, 4).Style.Font.SetBold(True).Font.SetFontSize(12)

            runningRow = runningRow + 1
            Dim TblGopTitle() As String = {"Hotel Name", "Sales Value" + vbCr + "Excl. VAT", "Cost Value" + vbCr + "Excl. VAT", "Profit", "%"}
            For i = 0 To TblGopTitle.GetUpperBound(0)
                If i = 0 Then
                    wsprofit.Cell(runningRow, i + 4).Value = TblGopTitle(i)
                    wsprofit.Range("D" + Convert.ToString(runningRow) + ":H" + Convert.ToString(runningRow)).Merge()
                Else
                    wsprofit.Cell(runningRow, i + 8).Value = TblGopTitle(i)
                End If
            Next
            wsprofit.Row(runningRow).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(True)

            Dim tmpSalevalue As Decimal = 0
            Dim tmpcostvalue As Decimal = 0
            For Each gopDr As DataRow In gopDt.Rows
                Dim ColNo As Integer = 0
                runningRow = runningRow + 1
                ColNo = 4
                wsprofit.Cell(runningRow, ColNo).Value = gopDr("groupname")
                wsprofit.Range("D" + Convert.ToString(runningRow) + ":H" + Convert.ToString(runningRow)).Merge().Style.Alignment.WrapText = True
                ColNo = ColNo + 5
                If accountType <> "CostOfSale" Then
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    wsprofit.Cell(runningRow, ColNo).Value = Math.Round(Convert.ToDecimal(gopDr("salevalue")), decno)
                End If

                ColNo = ColNo + 1
                If accountType <> "Income" Then
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    wsprofit.Cell(runningRow, ColNo).Value = Math.Round(Convert.ToDecimal(gopDr("costvalue")), decno)
                End If

                If accountType = "" Then
                    ColNo = ColNo + 1
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    wsprofit.Cell(runningRow, ColNo).Value = Math.Round(Convert.ToDecimal(gopDr("profit")), decno)

                    ColNo = ColNo + 1
                    wsprofit.Cell(runningRow, ColNo).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    wsprofit.Cell(runningRow, ColNo).Value = Math.Round(Convert.ToDecimal(gopDr("percentage")), 2)
                End If

                tmpSalevalue += gopDr("salevalue")
                tmpcostvalue += gopDr("costvalue")
            Next
            runningRow = runningRow + 1
            wsprofit.Range("D" + Convert.ToString(runningRow) + ":H" + Convert.ToString(runningRow)).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold(True)
            wsprofit.Cell(runningRow, 4).Value = "Total :  "
            If accountType <> "CostOfSale" Then wsprofit.Cell(runningRow, 9).Value = Math.Round(tmpSalevalue, decno)
            If accountType <> "Income" Then wsprofit.Cell(runningRow, 10).Value = Math.Round(tmpcostvalue, decno)
            If accountType = "" Then
                wsprofit.Cell(runningRow, 11).Value = Math.Round(tmpSalevalue - tmpcostvalue, decno)
                If tmpSalevalue = 0 Then
                    wsprofit.Cell(runningRow, 12).Value = 0
                Else
                    wsprofit.Cell(runningRow, 12).Value = Math.Round((tmpSalevalue - tmpcostvalue) / tmpSalevalue * 100, 2)
                End If
            End If
            wsprofit.Range("I" + runningRow.ToString + ":" + cols + runningRow.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold(True)
            Dim rnggop As IXLRange = wsprofit.Range("I" + Convert.ToString(excelLastRow + 2) + ":K" + Convert.ToString(runningRow))
            rnggop.DataType = XLCellValues.Number
            rnggop.Style.NumberFormat.Format = decPlaces
            wsprofit.Range("L" + Convert.ToString(excelLastRow + 2) + ":" + cols + Convert.ToString(runningRow)).Style.NumberFormat.Format = "##0.00;[Red](##0.00)"

            Dim tblRange As IXLRange
            tblRange = wsprofit.Range("D" + Convert.ToString(excelLastRow) + ":" + cols + Convert.ToString(runningRow))
            tblRange.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(10)
            tblRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin)
        End If
        'End If
        excelLastRow = runningRow + 5
    End Sub
#End Region

End Class
