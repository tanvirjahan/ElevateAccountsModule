#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

#End Region
Partial Class PriceListModule_PrintDoc
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim repDeocument As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim strReportName As String = ""
    Dim objutils As New clsUtils
#End Region
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Response.Redirect(CType(Session("BackPageName"), String), False)
        'Dim ScriptStr As String
        'ScriptStr = "<script language=""javascript"">var win=window.close();</script>"
        Try
            Session.Add("PrintDoc", "Searchwindow")
            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('ChildWindowPostBack', '');window.opener.focus();window.close();"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "printdoc", strscript, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("PrintDoc.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'Dim Ds As DataSet
            'Ds = CType(Session("PrinDocDS"), DataSet)
            'If Ds.Tables(0).Rows.Count > 1 Then
            '    Dim dtt As New Table
            '    Dim dr As New TableRow
            '    Dim td As New TableCell
            '    Dim th As New TableHeaderCell
            '    Dim i As Integer
            '    For i = 0 To Ds.Tables(0).Rows.Count - 1

            '    Next

            'End If

            BindReport(strReportName, CType(Session("SelectionFormula"), String), CType(Session("ReportTitle"), String))

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("PrintDoc.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If IsPostBack = False Then
            'lblHeading.Text = CType(Session("PrinDocTitle"), String)
            'lblDocId.Text = CType(Session("RefCode"), String)
            rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)

            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If

            If CType(Session("BackPageName"), String) = "" Then
                Response.Redirect("Login.aspx", False)
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
                        Case "CustomerSector"
                            strReportName = CType(Server.MapPath("~\Report\rptCustomerSector.rpt"), String)
                            rptreportname = "Report - Customer Sector"
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
                            strReportName = CType(Server.MapPath("`~\Report\rptNarrationTemplate.rpt"), String)
                            rptreportname = "Report - Narration Template"
                            Exit Select
                        Case "OpeningTrailBalance"
                            strReportName = CType(Server.MapPath("~\Report\rptOpeningTrialbalance.rpt"), String)
                            rptreportname = "Report - Opening Trail Balance"
                            Exit Select
                        Case "SupplierOpeningTrailBalance"
                            strReportName = CType(Server.MapPath("~\Report\rptSupplierOpeningbalance.rpt"), String)
                            If Session("OpenType") = "S" Then
                                rptreportname = "Report - Supplier Opening  Balance"
                            ElseIf Session("OpenType") = "C" Then
                                rptreportname = "Report - Customer Opening  Balance"
                            ElseIf Session("OpenType") = "A" Then
                                rptreportname = "Report - Supplier Agent Opening Balance"
                            End If
                        Case "DefineControlAccountsSupplierAgents"
                            strReportName = CType(Server.MapPath("~\Report\rptDefAccSupplieragent.rpt"), String)
                            rptreportname = "Report - SupplierAgents Control Accounts "
                            Exit Select
                        Case "DebitNoteDoc"
                            strReportName = CType(Server.MapPath("~\Report\rptDebitNoteDoc.rpt"), String)
                            If Session("CNDNOpen_type") = "DN" Then
                                rptreportname = "Report - Debit Note"
                            ElseIf Session("CNDNOpen_type") = "CN" Then
                                rptreportname = "Report - Credit Note"
                            End If
                        Case "DebitNoteBrief"
                            strReportName = CType(Server.MapPath("~\Report\rptDebitNoteBrief.rpt"), String)
                            If Session("CNDNOpen_type") = "DN" Then
                                rptreportname = "Report - Debit Note"
                            ElseIf Session("CNDNOpen_type") = "CN" Then
                                rptreportname = "Report - Credit Note"
                            End If
                        Case "DebitNoteDetail"
                            strReportName = CType(Server.MapPath("~\Report\rptDebitNoteDetail.rpt"), String)
                            If Session("CNDNOpen_type") = "DN" Then
                                rptreportname = "Report - Debit Note"
                            ElseIf Session("CNDNOpen_type") = "CN" Then
                                rptreportname = "Report - Credit Note"
                            End If
                            '---------------receipt
                        Case "ReceiptDoc"
                            If Session("RVPVTranType") = "RV" Then
                                rptreportname = "Receipt Voucher"
                                strReportName = CType(Server.MapPath("~\Report\rptReceipt.rpt"), String)
                            ElseIf Session("RVPVTranType") = "PV" Then
                                rptreportname = Session("PrinDocTitle")
                                strReportName = CType(Server.MapPath("~\Report\rptPayment.rpt"), String)
                            End If
                        Case "JournalDoc"
                            rptreportname = "Journal Voucher"
                            strReportName = CType(Server.MapPath("~\Report\rptJournal.rpt"), String)
                            Exit Select
                        Case "MatchOutstandingDoc"
                            rptreportname = "Match Outstanding"
                            strReportName = CType(Server.MapPath("~\Report\rptMatchOutstand.rpt"), String)
                            Exit Select
                        Case "PurchaseInvoiceDoc"
                            rptreportname = "Purchase Invoice"
                            strReportName = CType(Server.MapPath("~\Report\rptPurchaseInvoice.rpt"), String)
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
        End If
    End Sub
    Private Sub BindReport(ByVal ReportName As String, ByVal strSelectionFormula As String, ByVal strReportTitle As String)

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
            .ServerName = Session("dbServerName")        'ConfigurationManager.AppSettings("dbServerName")
            .DatabaseName = Session("dbDatabaseName")    'ConfigurationManager.AppSettings("dbDatabaseName")
            .UserID = Session("dbUserName")              'ConfigurationManager.AppSettings("dbUserName")
            .Password = Session("dbPassword")            'ConfigurationManager.AppSettings("dbPassword")
        End With

        repDeocument.Load(ReportName)

        '  Me.CRVReport.ReportSource = repDeocument
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

        If CType(Session("Pageame"), String) = "ReceiptDoc" Or CType(Session("Pageame"), String) = "JournalDoc" Or CType(Session("Pageame"), String) = "PurchaseInvoiceDoc" Then
            pname = pnames.Item("currency")
            paramvalue.Value = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
        End If


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

        'CRVReport.SeparatePages = False
        'CRVReport.DisplayGroupTree = False
        'CRVReport.DisplayBottomToolbar = False
        'CRVReport.DisplayToolbar = False
        ' Me.CRVReport.ReportSource = repDeocument
        repDeocument.RecordSelectionFormula = strSelectionFormula

        Response.Buffer = False
        Response.ClearContent()
        Response.ClearHeaders()
        repDeocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, False, "Report")

        'If strSelectionFormula <> "" Then
        '    CRVReport.SelectionFormula = strSelectionFormula
        'End If
        'Me.CRVReport.DataBind()
        ' CRVReport.HasCrystalLogo = False
        ' CRVReport.HasToggleGroupTreeButton = False

        'repDeocument.PrintToPrinter(1, False, 0, 0)

    End Sub

   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim ScriptStr As String
        'ScriptStr = "<script language=""javascript"">   ReprintDoc();</script>"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)
        'If Page.IsPostBack = False Then
        '    Dim repDeocument As New ReportDocument
        '    repDeocument = CType(Session("doc"), ReportDocument)
        '    Response.Buffer = False
        '    Response.ClearContent()
        '    Response.ClearHeaders()
        '    repDeocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, False, "Report")
        '    Session.Add("doc", "")
        'End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
'Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
'    'Response.Redirect(CType(Session("BackPageName"), String), False)
'    'Dim ScriptStr As String
'    'ScriptStr = "<script language=""javascript"">var win=window.close();</script>"
'    Try
'        Session.Add("PrintDoc", "Searchwindow")
'        Dim strscript As String = ""
'        strscript = "window.opener.__doPostBack('ChildWindowPostBack', '');window.opener.focus();window.close();"

'        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "printdoc", strscript, True)

'    Catch ex As Exception
'        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
'        objUtils.WritErrorLog("PrintDoc.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'    End Try

'End Sub
'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint.Click
'    Try
'        ' Response.Redirect("~\PriceListModule\rptReport.aspx", False)
'        'Dim ScriptStr As String
'        'ScriptStr = "<script language=""javascript"">var win=window.open('../PriceListModule/rptReport.aspx','mywindow2','toolbar=0,width=250,height=150,top=100,left=200,scrollbars=yes,resizable=no,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
'        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)
'        Session.Add("PrintDoc", "PrintDoc")
'        Dim strscript As String = ""
'        strscript = "window.opener.__doPostBack('ChildWindowPostBack', '');window.opener.focus();window.close();"

'        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "printdoc", strscript, True)

'    Catch ex As Exception
'        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
'        objUtils.WritErrorLog("PrintDoc.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
'    End Try
'End Sub

'Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
'    lblHeading.Text = CType(Session("PrinDocTitle"), String)
'    lblDocId.Text = CType(Session("RefCode"), String)
'End Sub

' Response.Redirect("~\PriceListModule\rptReport.aspx", False)
'Dim ScriptStr As String
'ScriptStr = "<script language=""javascript"">var win=window.open('../PriceListModule/rptReport.aspx','mywindow2','toolbar=0,width=250,height=150,top=100,left=200,scrollbars=yes,resizable=no,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)
'Session.Add("PrintDoc", "PrintDoc")
'Dim strscript As String = ""
'strscript = "window.opener.__doPostBack('ChildWindowPostBack', '');window.opener.focus();window.close();"

'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "printdoc", strscript, True)

