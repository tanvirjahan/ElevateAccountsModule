Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

Partial Class rptReport
    Inherits System.Web.UI.Page
    Dim repDeocument As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
        Dim strReportName As String = ""
        If CType(Session("BackPageName"), String) = "" Then
            Response.Redirect("~/Login.aspx", False)
            Exit Sub
        Else
            If CType(Session("Pageame"), String) = "" Then
                Response.Redirect(CType(Session("BackPageName"), String), False)
                Exit Sub
            Else
                Select Case CType(Session("Pageame"), String)
                    Case "Currency"
                        strReportName = CType(Server.MapPath("~\Report\rptCurrencies.rpt"), String)
                        rptreportname = "Report - Currencies"
                        Exit Select
                    Case "Market"
                        strReportName = CType(Server.MapPath("~\Report\rptMarkets.rpt"), String)
                        rptreportname = "Report - Markets"
                        Exit Select
                    Case "Country"
                        strReportName = CType(Server.MapPath("~\Report\rptCountries.rpt"), String)
                        rptreportname = "Report - Countries"
                        Exit Select
                    Case "City"
                        strReportName = CType(Server.MapPath("~\Report\rptCities.rpt"), String)
                        rptreportname = "Report - Cities"
                        Exit Select
                    Case "Sector"
                        strReportName = CType(Server.MapPath("~\Report\rptSector.rpt"), String)
                        rptreportname = "Report -Sectors"
                        Exit Select
                    Case "Supplier Type"
                        strReportName = CType(Server.MapPath("~\Report\rptSupplierType.rpt"), String)
                        rptreportname = "Report -Supplier Types"
                        Exit Select
                    Case "Supplier Category"
                        strReportName = CType(Server.MapPath("~\Report\rptSupplierCategories.rpt"), String)
                        rptreportname = "Report -Supplier Categories"
                        Exit Select
                    Case "Supplier Selling Category"
                        strReportName = CType(Server.MapPath("~\Report\rptSupplierSellingCategories.rpt"), String)
                        rptreportname = "Report -Supplier Selling Categories"
                        Exit Select
                    Case "Meal Plan"
                        strReportName = CType(Server.MapPath("~\Report\rptMealPlans.rpt"), String)
                        rptreportname = "Report - Meal Plans"
                        Exit Select
                    Case "Other Service Group"
                        strReportName = CType(Server.MapPath("~\Report\rptOtherServiceGroups.rpt"), String)
                        rptreportname = "Report - Other Service Groups"
                        Exit Select
                    Case "Other service Type"
                        strReportName = CType(Server.MapPath("~\Report\rptOtherServiceTypes.rpt"), String)
                        rptreportname = "Report - Other Service Types"
                        Exit Select
                    Case "Other Service Category"
                        strReportName = CType(Server.MapPath("~\Report\rptOtherServiceCategories.rpt"), String)
                        rptreportname = "Report - Other Service Categories"
                        Exit Select
                    Case "Special Event Extras"
                        strReportName = CType(Server.MapPath("~\Report\rptSpecialEventsorExtras.rpt"), String)
                        rptreportname = "Report - Special Events/Extras"
                        Exit Select
                    Case "Season"
                        strReportName = CType(Server.MapPath("~\Report\rptSeasons.rpt"), String)
                        rptreportname = "Report - Seasons"
                        Exit Select
                    Case "Sub Season"
                        strReportName = CType(Server.MapPath("~\Report\rptSubSeasons.rpt"), String)
                        rptreportname = "Report - Sub Seasons"
                        Exit Select
                    Case "Cancellation Type"
                        strReportName = CType(Server.MapPath("~\Report\rptCancellationTypes.rpt"), String)
                        rptreportname = "Report - Cancellation Types"
                        Exit Select
                    Case "Selling Price Type"
                        strReportName = CType(Server.MapPath("~\Report\rptSellPriceType.rpt"), String)
                        rptreportname = "Report - Selling Price Type"
                        Exit Select
                    Case "Other Service Selling Type"
                        strReportName = CType(Server.MapPath("~\Report\rptOtherServicesSellingTypes.rpt"), String)
                        rptreportname = "Report - Other Service Selling Types"
                        Exit Select
                    Case "Complusary Remark"
                        strReportName = CType(Server.MapPath("~\Report\rptCompulsoryRemarks.rpt"), String)
                        rptreportname = "Report - Complusary Remarks"
                        Exit Select
                    Case "Minimum Nights"
                        strReportName = CType(Server.MapPath("~\Report\rptMinimumNights.rpt"), String)
                        rptreportname = "Report - Minimum Nights"
                        Exit Select
                    Case "Customer Category"
                        strReportName = CType(Server.MapPath("~\Report\rptCustomerCategories.rpt"), String)
                        rptreportname = "Report - Customer Categories"
                        Exit Select
                    Case "Customer Sector"
                        strReportName = CType(Server.MapPath("~\Report\rptSect.rpt"), String)
                        rptreportname = "Report - Customer Sectors"
                        Exit Select
                    Case "Ticket Selling Type"
                        strReportName = CType(Server.MapPath("~\Report\rptTicketSellingTypes.rpt"), String)
                        rptreportname = "Report - Ticket Selling Types"
                        Exit Select
                    Case "Flights Master"
                        strReportName = CType(Server.MapPath("~\Report\rptFlightMaster.rpt"), String)
                        rptreportname = "Report - Flights Masters"
                        Exit Select
                    Case "Flight Class Master"
                        strReportName = CType(Server.MapPath("~\Report\rptFlightClassMaster.rpt"), String)
                        rptreportname = "Report - Flight Class Masters"
                        Exit Select
                    Case "Fare Type"
                        strReportName = CType(Server.MapPath("~\Report\rptFareTypes.rpt"), String)
                        rptreportname = "Report - Fare Types"
                        Exit Select
                    Case "Hotel Construction"
                        strReportName = CType(Server.MapPath("~\Report\rptHotelsConstruction.rpt"), String)
                        rptreportname = "Report - Hotel Constructions"
                        Exit Select
                    Case "Room Type"
                        strReportName = CType(Server.MapPath("~\Report\rptRoomTypes.rpt"), String)
                        rptreportname = "Report - Room Types"
                        Exit Select
                    Case "Room Category"
                        strReportName = CType(Server.MapPath("~\Report\rptRoomCategories.rpt"), String)
                        rptreportname = "Report - Room Categories"
                        Exit Select
                    Case "General Policy"
                        strReportName = CType(Server.MapPath("~\Report\rptGeneralPolicy.rpt"), String)
                        rptreportname = "Report - General Policies"
                        Exit Select
                    Case "Max Accomodation"
                        strReportName = CType(Server.MapPath("~\Report\rptMaxAccomodation.rpt"), String)
                        rptreportname = "Report -Max Accomodations"
                        Exit Select
                    Case "Customers"
                        strReportName = CType(Server.MapPath("~\Report\rptCustomers.rpt"), String)
                        rptreportname = "Report - Customers"
                        Exit Select
                    Case "Block Full Sales"
                        strReportName = CType(Server.MapPath("~\Report\rptBlockFullSales.rpt"), String)
                        rptreportname = "Report - Block Full Sales"
                        Exit Select
                    Case "Supplier Agent"
                        strReportName = CType(Server.MapPath("~\Report\rptSupplierAgents.rpt"), String)
                        rptreportname = "Report - Supplier Agents"
                        Exit Select
                    Case "Supplier"
                        strReportName = CType(Server.MapPath("~\Report\rptSuppliers.rpt"), String)
                        rptreportname = "Report - Suppliers"
                        Exit Select
                    Case "SellingFormulaForCategories"
                        strReportName = CType(Server.MapPath("~\Report\rptSellingformulaforCategories.rpt"), String)
                        rptreportname = "Report - Selling Formula for Categories"
                        Exit Select
                    Case "Selling Price Formulas"
                        strReportName = CType(Server.MapPath("~\Report\rptSellingpriceformula.rpt"), String)
                        rptreportname = "Report - Selling Price Formulas"
                        Exit Select
                    Case "Cancellation Policy"
                        strReportName = CType(Server.MapPath("~\Report\rptcancellationplcy.rpt"), String)
                        rptreportname = "Report - Cancellation Policies"
                        Exit Select
                    Case "Child Policy"
                        strReportName = CType(Server.MapPath("~\Report\rptchildpolicy.rpt"), String)
                        rptreportname = "Report - Child Policies"
                        Exit Select
                    Case "SellingFormulaforSuppliers"
                        strReportName = CType(Server.MapPath("~\Report\rptsellingformulaforSuppliers.rpt"), String)
                        rptreportname = "Report - Selling Formula for Suppliers"
                        Exit Select
                    Case "Promotion"
                        strReportName = CType(Server.MapPath("~\Report\rptpromotions.rpt"), String)
                        rptreportname = "Report - Promotions"
                        Exit Select
                    Case "Early Promotion"
                        strReportName = CType(Server.MapPath("~\Report\rptearlybirdpromotion.rpt"), String)
                        rptreportname = "Report - Early Bird Promotions"
                        Exit Select
                    Case "Currency Conversion Rates"
                        strReportName = CType(Server.MapPath("~\Report\rptcurrencyconversionrates.rpt"), String)
                        rptreportname = "Report - Currency Conversion Rates"
                        Exit Select
                    Case "Competitor Rates"
                        strReportName = CType(Server.MapPath("~\Report\rptCompetitorRates.rpt"), String)
                        rptreportname = "Report - Competitors Rates"
                        Exit Select
                    Case "Other Services Policy"
                        strReportName = CType(Server.MapPath("~\Report\rptOtherServicesPolicy.rpt"), String)
                        rptreportname = "Report - Other Services Policy"
                        Exit Select
                    Case "Other Services Price List"
                        strReportName = CType(Server.MapPath("~\Report\rptOtherServicesPriceList.rpt"), String)
                        rptreportname = "Report - Other Services Price List"
                        Exit Select
                        ' Admin Module
                    Case "Define Group"
                        strReportName = CType(Server.MapPath("~\Report\rptUserGroups.rpt"), String)
                        rptreportname = "Report - Define User Groups"
                        Exit Select
                    Case "Application Rights"
                        strReportName = CType(Server.MapPath("~\Report\rptGroupApplicationRights.rpt"), String)
                        rptreportname = "Report - Application Rights for User Groups"
                        Exit Select
                    Case "Group Rights"
                        strReportName = CType(Server.MapPath("~\Report\rptGroupMenuRights.rpt"), String)
                        rptreportname = "Report - Group Rights for Menus"
                        Exit Select

                    Case "Department Master"
                        strReportName = CType(Server.MapPath("~\Report\rptDeptMaster.rpt"), String)
                        rptreportname = "Report - Department Master"
                        Exit Select
                    Case "User Master"
                        strReportName = CType(Server.MapPath("~\Report\rptUserMaster.rpt"), String)
                        rptreportname = "Report - User Master"
                        Exit Select
                    Case "Privilege Rights"
                        strReportName = CType(Server.MapPath("~\Report\rptGroupPrivilegeRights.rpt"), String)
                        rptreportname = "Report - Privilege Rights"
                        Exit Select

                        ' Accounts
                    Case "Account Group"
                        strReportName = CType(Server.MapPath("~\Report\rptAcctGroup.rpt"), String)
                        rptreportname = "Report - Account Group Master"
                        Exit Select
                    Case "Accounts Master"
                        strReportName = CType(Server.MapPath("~\Report\rptAcctCode.rpt"), String)
                        rptreportname = "Report - Account Master"
                        Exit Select

                    Case "Bank Type"
                        strReportName = CType(Server.MapPath("~\Report\bank_master_type.rpt"), String)
                        rptreportname = "Report - Bank Master Type"
                        Exit Select
                    Case "Profit Center Master"
                        strReportName = CType(Server.MapPath("~\Report\rptProfitCenterMaster.rpt"), String)
                        rptreportname = "Report - Profit Center Master"
                        Exit Select
                    Case "Cost Center Group"
                        strReportName = CType(Server.MapPath("~\Report\rptCostCenterGroupMaster.rpt"), String)
                        rptreportname = "Report - Cost Center Group Master"
                        Exit Select
                    Case "Cost Center"
                        strReportName = CType(Server.MapPath("~\Report\rptCostCenterMaster.rpt"), String)
                        rptreportname = "Report - Cost Center Master"
                        Exit Select
                    Case "Narration Template"
                        strReportName = CType(Server.MapPath("~\Report\rptNarrationTemplate.rpt"), String)
                        rptreportname = "Report - Narration Template"
                        Exit Select
                    Case "OtherBankMaster"
                        strReportName = CType(Server.MapPath("~\Report\rptOtherBankMaster.rpt"), String)
                        rptreportname = "Report - Other Bank Master"
                        Exit Select
                    Case "Journal"
                        strReportName = CType(Server.MapPath("~\Report\Journalreport.rpt"), String)
                        rptreportname = "Report - Journal"
                        Exit Select

                End Select
                If strReportName = "" Then
                    Response.Redirect(CType(Session("BackPageName"), String), False)
                    Exit Sub
                Else
                    BindReport(strReportName, CType(Session("SelectionFormula"), String), CType(Session("ReportTitle"), String))
                End If

            End If
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub BindReport(ByVal ReportName As String, ByVal strSelectionFormula As String, ByVal strReportTitle As String)
        Try

       
            Dim pnames As ParameterFieldDefinitions
            Dim pname As ParameterFieldDefinition
            Dim param As New ParameterValues
            Dim paramvalue As New ParameterDiscreteValue

            'If Session("Rep") Is Nothing Then
            Dim ConnInfo As New ConnectionInfo
            'With ConnInfo
            '    .ServerName = ConfigurationManager.AppSettings("dbServerName")
            '    .DatabaseName = ConfigurationManager.AppSettings("dbDatabaseName")
            '    .UserID = ConfigurationManager.AppSettings("dbUserName")
            '    .Password = ConfigurationManager.AppSettings("dbPassword")
            'End With
            With ConnInfo
                .ServerName = Session("dbServerName")
                .DatabaseName = Session("dbDatabaseName")
                .UserID = Session("dbUserName")
                .Password = Session("dbPassword")
            End With
            repDeocument.Load(ReportName)

            Me.CRVReport.ReportSource = repDeocument
            Dim RepTbls As Tables = repDeocument.Database.Tables

            For Each RepTbl As Table In RepTbls
                Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
                RepTblLogonInfo.ConnectionInfo = ConnInfo
                RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
            Next

            repDeocument.SummaryInfo.ReportTitle = strReportTitle
            pnames = repDeocument.DataDefinition.ParameterFields

            pname = pnames.Item("CompanyName")
            paramvalue.Value = rptcompanyname
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("ReportName")
            paramvalue.Value = rptreportname
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            If Not Session("ColReportParams") Is Nothing Then
                Dim p As Integer
                Dim colreport As New Collection
                colreport = Session("ColReportParams")
                Dim creport As New clsReportParam
                For p = 1 To colreport.Count
                    creport = colreport.Item(p)
                    pname = pnames.Item(creport.rep_parametername)
                    paramvalue.Value = creport.rep_parametervalue
                    param = pname.CurrentValues
                    param.Add(paramvalue)
                    pname.ApplyCurrentValues(param)

                Next

            End If
            Me.CRVReport.ReportSource = repDeocument
            If strSelectionFormula <> "" Then
                CRVReport.SelectionFormula = strSelectionFormula
            End If
            Session.Add("ReportSource", repDeocument)
            Me.CRVReport.DataBind()
            CRVReport.HasCrystalLogo = False
        Catch ex As Exception

        End Try


    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If CType(Session("BackPageName"), String) = "" Then
            Response.Redirect("MainPage.aspx", False)
            Exit Sub
        Else
            Session("ColReportParams") = Nothing
            Response.Redirect(CType(Session("BackPageName"), String), False)
        End If



    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If Page.IsPostBack = True Then
            repDeocument.Close()
            repDeocument.Dispose()
            ' Session("ColReportParams") = Nothing
        End If
    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Session.Add("ReportSource", repDeocument)
    '    Dim strpop As String = ""
    '    'strpop = "window.open('../PriceListModule/RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
    '    strpop = "<script language=""javascript"">var win=window.open('../PriceListModule/RptPrintPage.aspx','printdoc','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
    '    'ScriptStr = "<script language=""javascript"">var win=window.open('PrintDoc.aspx','mywindow2','toolbar=0,width=250,height=150,top=100,left=200,scrollbars=yes,resizable=no,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, False)
    'End Sub
End Class

