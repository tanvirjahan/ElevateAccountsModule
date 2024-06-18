Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO




Partial Class rptReportNew
    Inherits System.Web.UI.Page
    Dim repDeocument As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim strSelectionFormula As String = ""
    Dim strReportTitle As String = ""

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim strReportName As String = ""

        ViewState.Add("Pageame", Request.QueryString("Pageame"))
        ViewState.Add("BackPageName", Request.QueryString("BackPageName"))
        ViewState.Add("divcode", Request.QueryString("divid"))


        If ViewState("divcode") <> "" Then
            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & ViewState("divcode") & "'"), String)
        Else
            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
        End If


        If CType(ViewState("BackPageName"), String) = "" Then
            'Response.Redirect("Login.aspx", False)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)

        Else
            If CType(ViewState("Pageame"), String) = "" Then
                'Response.Redirect(CType(Session("BackPageName"), String), False)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
                Exit Sub
            Else
                Select Case CType(ViewState("Pageame"), String)
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
                        rptreportname = "Report - Compulsary Remarks"
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
                        rptreportname = "Report - Flight Masters"
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
                    Case "OpeningTrailBalance"
                        ViewState.Add("TranID", Request.QueryString("TranID"))
                        ViewState.Add("TranType", Request.QueryString("TranType"))
                        ViewState.Add("Type", Request.QueryString("Type"))

                        strReportName = CType(Server.MapPath("~\Report\rptOpeningTrialbalance.rpt"), String)
                        rptreportname = "Report - Opening Trail Balance"
                        Exit Select

                    Case "SupplierOpeningTrailBalance"
                        ViewState.Add("opentype", Request.QueryString("opentype"))
                        If Request.QueryString("TranID") <> "" Then
                            If Trim(strSelectionFormula) = "" Then
                                strReportTitle = "Doc No : " & Request.QueryString("TranID")
                                strSelectionFormula = " {openparty_master.div_id}='" & ViewState("divcode") & "' and {openparty_master.div_id}='" & ViewState("divcode") & "' and ucase({openparty_master.tran_id}) LIKE  '" & Request.QueryString("TranID") & "*'"
                            Else
                                strReportTitle = strReportTitle & "Doc No : " & Request.QueryString("TranID")
                                strSelectionFormula = strSelectionFormula & " {openparty_master.div_id}='" & ViewState("divcode") & "' and ucase({openparty_master.tran_id}) LIKE   '" & Request.QueryString("TranID") & "*'"
                            End If
                        End If
                        If Request.QueryString("Supplier") <> "[Select]" Then
                            If Trim(strSelectionFormula) = "" Then
                                strReportTitle = " Supplier: " & Request.QueryString("Supplier")
                                strSelectionFormula = " {openparty_master.div_id}='" & ViewState("divcode") & "' and {openparty_master.open_code} = '" & Request.QueryString("Supplier") & "'"
                            Else
                                strReportTitle = strReportTitle & "; Supplier: " & Request.QueryString("Supplier")
                                strSelectionFormula = strSelectionFormula & " and {openparty_master.div_id}='" & ViewState("divcode") & "' and  {openparty_master.open_code} = '" & Request.QueryString("Supplier") & "'"
                            End If
                        End If
                        If Request.QueryString("Curr") <> "[Select]" Then
                            If Trim(strSelectionFormula) = "" Then
                                strReportTitle = "Currency " & Request.QueryString("Curr")
                                strSelectionFormula = " {openparty_master.div_id}='" & ViewState("divcode") & "' and {openparty_master.currcode} = '" & Request.QueryString("Curr") & "'"
                            Else
                                strReportTitle = strReportTitle & "; Currency: " & Request.QueryString("Curr")
                                strSelectionFormula = strSelectionFormula & " AND {openparty_master.div_id}='" & ViewState("divcode") & "' and {openparty_master.currcode} = '" & Request.QueryString("Curr") & "'"
                            End If
                        End If
                        If Request.QueryString("ConvRate") <> "" Then
                            If Trim(strSelectionFormula) = "" Then
                                strReportTitle = "Conversion Rate  " & Request.QueryString("ConvRate")
                                strSelectionFormula = " {openparty_master.div_id}='" & ViewState("divcode") & "' and {openparty_master.currrate} = " & Request.QueryString("ConvRate") & ""
                            Else
                                strReportTitle = strReportTitle & "; Conversion Rate: " & Request.QueryString("ConvRate")
                                strSelectionFormula = strSelectionFormula & " AND {openparty_master.div_id}='" & ViewState("divcode") & "' and {openparty_master.currrate} = " & Request.QueryString("ConvRate") & ""
                            End If
                        End If
                        If Trim(strSelectionFormula) = "" Then
                            strSelectionFormula = "  {openparty_master.open_type} = '" & ViewState("opentype") & "'"
                        Else
                            strSelectionFormula = strSelectionFormula & " AND {openparty_master.div_id}='" & ViewState("divcode") & "' and {openparty_master.open_type} = '" & ViewState("opentype") & "'"
                        End If

                        strReportName = CType(Server.MapPath("~\Report\rptSupplierOpeningbalance.rpt"), String)
                        If ViewState("opentype") = "S" Then
                            rptreportname = "Report - Supplier Opening  Balance"
                        ElseIf ViewState("opentype") = "C" Then
                            rptreportname = "Report - Customer Opening  Balance"
                        ElseIf ViewState("opentype") = "A" Then
                            rptreportname = "Report - Supplier Agent Opening Balance"
                        End If
                       Case "DebitNoteDoc"
                        If Request.QueryString("TranId") <> "" Then
                            If Request.QueryString("TranId") <> "" Then
                                If Trim(strSelectionFormula) = "" Then
                                    strSelectionFormula = " {trdpurchase_master.tran_id}='" & Request.QueryString("TranId") & "'"
                                Else
                                    strSelectionFormula = strSelectionFormula & " AND {trdpurchase_master.tran_id}='" & Request.QueryString("TranId") & "'"
                                End If
                            End If

                        End If

                        If Trim(strSelectionFormula) = "" Then
                            strSelectionFormula = " {trdpurchase_master.tran_type} = '" & Request.QueryString("TranType") & "' " & _
                            " and  {trdpurchase_master.div_id} = '" & ViewState("divcode") & "'"
                        Else
                            strSelectionFormula = strSelectionFormula & " AND {trdpurchase_master.tran_type} = '" & Request.QueryString("TranType") & "'" & _
                            " and  {trdpurchase_master.div_id} = '" & ViewState("divcode") & "'"
                        End If
                        If Request.QueryString("TranType") = "DN" Then
                            rptreportname = "Report - Debit Note"
                        ElseIf Request.QueryString("TranType") = "CN" Then
                            rptreportname = "Report - Credit Note"
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptDebitNoteDoc.rpt"), String)
                        Exit Select
                    Case "ReceiptDoc"
                        If Request.QueryString("TranId") <> "" Then
                            If Trim(strSelectionFormula) = "" Then
                                strSelectionFormula = "  {receipt_master_new.tran_id}='" & Request.QueryString("TranId") & "'"
                            Else
                                strSelectionFormula = strSelectionFormula & " AND  {receipt_master_new.tran_id}='" & Request.QueryString("Tranid") & "'"
                            End If
                        End If

                        If Trim(strSelectionFormula) = "" Then
                            strSelectionFormula = " {receipt_master_new.tran_type}= '" & Request.QueryString("TranType") & "' " & _
                            " and  {receipt_master_new.div_id} = '" & CType(objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511), String) & "'"
                        Else
                            strSelectionFormula = strSelectionFormula & " AND {receipt_master_new.tran_type}= '" & Request.QueryString("TranType") & "'" & _
                            " and {receipt_master_new.div_id}= '" & CType(objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511), String) & "'"
                        End If

                        If Request.QueryString("CashBankType") <> "" Then
                            If Trim(strSelectionFormula) = "" Then
                                strSelectionFormula = " {receipt_master_new.receipt_cashbank_type}='" & Request.QueryString("CashBankType") & "'"
                            Else
                                strSelectionFormula = strSelectionFormula & " AND {receipt_master_new.receipt_cashbank_type}='" & Request.QueryString("CashBankType") & "'"
                            End If
                        End If
                        rptreportname = Request.QueryString("PrinDocTitle")
                        Session("ColReportParams") = Nothing
                        If Request.QueryString("TranType") = "RV" Then
                            strReportName = CType(Server.MapPath("~\Report\rptReceipt.rpt"), String)
                        ElseIf Request.QueryString("TranType") = "CPV" Then
                            strReportName = CType(Server.MapPath("~\Report\rptCashPayment.rpt"), String)
                        ElseIf Request.QueryString("TranType") = "BPV" Then
                            strReportName = CType(Server.MapPath("~\Report\rptBankPayment.rpt"), String)
                        Else 'If Request.QueryString("TranType") = "PV" Then
                            strReportName = CType(Server.MapPath("~\Report\rptPayment.rpt"), String)
                        End If

                        Exit Select
                    Case "JournalDoc"
                        If Request.QueryString("TranId") <> "" Then
                            If Trim(strSelectionFormula) = "" Then
                                strSelectionFormula = "  {journal_master.tran_id}='" & Request.QueryString("TranId") & "'"
                            Else
                                strSelectionFormula = strSelectionFormula & " AND  {journal_master.tran_id}='" & Request.QueryString("Tranid") & "'"
                            End If
                        End If

                        If Trim(strSelectionFormula) = "" Then
                            strSelectionFormula = " {journal_master.tran_type} = '" & Request.QueryString("TranType") & "' " & _
                            " and  {journal_master.journal_div_id} = '" & CType(objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511), String) & "'"
                        Else
                            strSelectionFormula = strSelectionFormula & " and {journal_master.tran_type}= '" & Request.QueryString("TranType") & "'" & _
                            " and  {journal_master.journal_div_id} = '" & CType(objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511), String) & "' "
                        End If
                        Session("ColReportParams") = Nothing
                        rptreportname = "Journal Voucher"
                        strReportName = CType(Server.MapPath("~\Report\rptJournal.rpt"), String)
                        Exit Select
                    Case "MatchOutstandingDoc"
                        If Request.QueryString("TranId") <> "" Then
                            If Trim(strSelectionFormula) = "" Then
                                strSelectionFormula = " {matchos_master.tran_id}='" & Request.QueryString("TranId") & "'"
                            Else
                                strSelectionFormula = strSelectionFormula & " AND {matchos_master.tran_id}='" & Request.QueryString("TranId") & "'"
                            End If
                        End If
                        If Trim(strSelectionFormula) = "" Then
                            strSelectionFormula = " {matchos_master.tran_type} = '" & Request.QueryString("TranType") & "' " & _
                            " and  {matchos_master.div_id} = '" & CType(objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511), String) & "'"
                        Else
                            strSelectionFormula = strSelectionFormula & " AND {matchos_master.tran_type} = '" & Request.QueryString("TranType") & "'" & _
                            " and  {matchos_master.div_id} = '" & CType(objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511), String) & "'"
                        End If

                        rptreportname = "Match Outstanding"
                        If Request.QueryString("Curr") <> "" Then
                            strReportName = CType(Server.MapPath("~\Report\rptMatchOutstandclient.rpt"), String)
                        Else
                            strReportName = CType(Server.MapPath("~\Report\rptMatchOutstand.rpt"), String)
                        End If

                        Exit Select
                    Case "Account Group"
                        If Request.QueryString("AcctgrpCode") <> "" Then
                            strReportTitle = "Account Group Code: " & Request.QueryString("AcctgrpCode")
                            strSelectionFormula = " {acctgroup.div_code}='" & ViewState("divcode") & "' and {acctgroup.acctcode} LIKE '" & Request.QueryString("AcctgrpCode") & "'"
                        ElseIf Request.QueryString("AcctgrpName") <> "" Then
                            strReportTitle = "Account Group Name: " & Request.QueryString("AcctgrpName")
                            strSelectionFormula = " {acctgroup.div_code} ='" & ViewState("divcode") & "' and {acctgroup.acctname} LIKE '" & Request.QueryString("AcctgrpName") & "'"
                        End If

                        strReportName = CType(Server.MapPath("~\Report\rptAcctGroup.rpt"), String)
                        rptreportname = "Report - Account Group Master"
                        Exit Select


                    Case "BankDetails"
                        If Request.QueryString("bankcode") <> "[Select]" Then
                            ' strReportTitle = "Bank: " & Request.QueryString("Bankname")
                            ' strSelectionFormula = " {bankdetails_master.bankcode} = '" & Request.QueryString("bankcode") & "'"
                        ElseIf Request.QueryString("acctName") <> "" Then
                            strReportTitle = "Account  Name : " & Server.UrlDecode(Request.QueryString("acctName"))
                            strSelectionFormula = " {bankdetails_master.div_id} ='" & ViewState("divcode") & "' and  {bankdetails_master.accountname} LIKE '" & Request.QueryString("acctName") & "'"
                        End If

                        strReportName = CType(Server.MapPath("~\Report\rptbankdetailsmaster.rpt"), String)
                        rptreportname = "Report - Bank Details Master"
                        Exit Select

                    Case "Logdetails"
                        If Request.QueryString("Tid") IsNot Nothing Then
                            If Request.QueryString("Tid") <> "" Then
                                strReportTitle = "Log:" & Request.QueryString("Tid")
                                'strSelectionFormula = "{acc_trn_amend_log.tran_id}='" & Request.QueryString("Tid") & "'"

                                strSelectionFormula = " {acc_trn_amend_log.tran_id} = '" & Request.QueryString("Tid") & "'"
                            End If
                        End If

                        If Request.QueryString("User") IsNot Nothing Then

                            If Request.QueryString("User") <> "[Select]" Then


                                If strSelectionFormula <> "" Then
                                    strReportTitle = strReportTitle & " ; User Name : " & Request.QueryString("User")
                                    strSelectionFormula = strSelectionFormula & " and {acc_trn_amend_log.moduser}= '" & Request.QueryString("User") & "'"
                                Else
                                    strReportTitle = "User Name: " & Request.QueryString("User")
                                    strSelectionFormula = " {acc_trn_amend_log.moduser} = '" & Request.QueryString("User") & "'"
                                End If

                            End If

                        End If

                        If Request.QueryString("Ttype1") IsNot Nothing Then

                            If Request.QueryString("Ttype1") <> "[Select]" Then
                                If strSelectionFormula <> "" Then
                                    strReportTitle = strReportTitle & " ; Transaction Type : " & Request.QueryString("Ttype1")
                                    strSelectionFormula = strSelectionFormula & " and {acc_trn_amend_log.tran_type} ='" & Request.QueryString("Ttype1") & "'"
                                Else
                                    strReportTitle = "Transaction Type: " & Request.QueryString("Ttype1")
                                    strSelectionFormula = " {acc_trn_amend_log.tran_type} = '" & Request.QueryString("Ttype1") & "'"
                                End If
                            End If
                        End If
                        If Request.QueryString("Fromdate") IsNot Nothing Then


                            If Request.QueryString("Fromdate") <> "" Then

                                If strSelectionFormula <> "" Then

                                    strReportTitle = strReportTitle & Environment.NewLine & " From  :" & Format(CType(Request.QueryString("Fromdate"), Date), "dd/MM/yyyy") & " To  :" & Format(CType(Request.QueryString("Todate"), Date), "dd/MM/yyyy")
                                    strSelectionFormula = strSelectionFormula & "  and {acc_trn_amend_log.moddate} >=CDateTime( '" & Request.QueryString("Fromdate") & "') and {acc_trn_amend_log.moddate} <=CDateTime( '" & Request.QueryString("Todate") & "')"

                                Else


                                    strReportTitle = " From : " & Format(CType(Request.QueryString("Fromdate"), Date), "dd/MM/yyyy") & " To : " & Format(CType((Request.QueryString("Todate")), Date), "dd/MM/yyyy")
                                    strSelectionFormula = "{acc_trn_amend_log.moddate} >=CDateTime( '" & Request.QueryString("Fromdate") & "') and {acc_trn_amend_log.moddate} <=CDateTime( '" & Request.QueryString("Todate") & "')"
                                End If

                            End If
                        End If

                        strReportName = CType(Server.MapPath("~\Report\rptaccounts_Log.rpt"), String)
                        rptreportname = "Report - Log"

                        Exit Select



                    Case "Accounts Master"
                        If Request.QueryString("AcctCode") <> "" Then
                            strReportTitle = "Account Code: " & Request.QueryString("AcctCode")
                            strSelectionFormula = " {acctgroup.div_code}='" & ViewState("divcode") & "' and {acctgroup.acctcode} LIKE '" & Request.QueryString("AcctCode") & "*'"
                        ElseIf Request.QueryString("AcctName") <> "" Then
                            strReportTitle = "Account Name: " & Request.QueryString("AcctName")
                            strSelectionFormula = " {acctgroup.div_code}='" & ViewState("divcode") & "' and {acctgroup.acctname} LIKE '" & Request.QueryString("AcctName") & "*'"
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptAcctCode.rpt"), String)
                        rptreportname = "Report - Account Master"
                        Exit Select

                    Case "Bank Type"
                        If Request.QueryString("BanktypeCode") <> "" Then
                            strReportTitle = "Bank Type Code : " & Request.QueryString("BanktypeCode")
                            strSelectionFormula = "{bank_master_type.bank_master_type_code} LIKE '" & Request.QueryString("BanktypeCode") & "*'"


                        End If
                        If Request.QueryString("BanktypeName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Bank Type Name : " & Request.QueryString("BanktypeName")
                                strSelectionFormula = strSelectionFormula & " and {bank_master_type.bank_master_type_des} LIKE '" & CType(Request.QueryString("BanktypeName"), String) & "*'"
                            Else
                                strReportTitle = "Bank Type Name : " & Request.QueryString("BanktypeName")
                                strSelectionFormula = "{bank_master_type.bank_master_type_des} LIKE '" & Request.QueryString("BanktypeName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\bank_master_type.rpt"), String)
                        rptreportname = "Report - Bank Master Type"
                        Exit Select

                    Case "Profit Center Master"
                        If Request.QueryString("ScatCode") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;Service Category : " & CType(Request.QueryString("ScatCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {profitcentremast.servicecat}  = '" & CType(Request.QueryString("ScatCode"), String) & "'"
                            Else
                                strReportTitle = "Service Category : " & CType(Request.QueryString("ScatCode"), String)
                                strSelectionFormula = "{profitcentremast.servicecat}  = '" & CType(Request.QueryString("ScatCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("DispName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;Display Name : " & CType(Request.QueryString("DispName"), String)
                                strSelectionFormula = strSelectionFormula & " and {profitcentremast.dispname} ='" & CType(Request.QueryString("DispName"), String) & "'"
                            Else
                                strReportTitle = "Display Name : " & CType(Request.QueryString("DispName"), String)
                                strSelectionFormula = " {profitcentremast.dispname}  = '" & CType(Request.QueryString("DispName"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("IncomeCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;Income Code : " & CType(Request.QueryString("IncomeCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {profitcentremast.incomecode} = '" & CType(Request.QueryString("IncomeCode"), String) & "'"
                            Else
                                strReportTitle = "Income Code : " & CType(Request.QueryString("IncomeCode"), String)
                                strSelectionFormula = " {profitcentremast.incomecode} ='" & CType(Request.QueryString("IncomeCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("CostCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;Cost of Sales Code : " & CType(Request.QueryString("CostCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {profitcentremast.costcode} ='" & CType(Request.QueryString("CostCode"), String) & "'"
                            Else
                                strReportTitle = "Cost of Sales Code : " & CType(Request.QueryString("CostCode"), String)
                                strSelectionFormula = " {profitcentremast.costcode} ='" & CType(Request.QueryString("CostCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("RefIncomeCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;Refund Income Code : " & CType(Request.QueryString("RefIncomeCode"), String)
                                strSelectionFormula = strSelectionFormula & "and {profitcentremast.refundincomecode} = '" & CType(Request.QueryString("RefIncomeCode"), String) & "'"
                            Else
                                strReportTitle = "Refund Income Code : " & CType(Request.QueryString("RefIncomeCode"), String)
                                strSelectionFormula = " {profitcentremast.refundincomecode} ='" & CType(Request.QueryString("RefIncomeCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("RefCostCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;Refund Cost Code : " & CType(Request.QueryString("RefCostCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {profitcentremast.refundcostcode} = '" & CType(Request.QueryString("RefCostCode"), String) & "'"
                            Else
                                strReportTitle = "Refund Cost Code : " & CType(Request.QueryString("RefCostCode"), String)
                                strSelectionFormula = " {profitcentremast.refundcostcode} = '" & CType(Request.QueryString("RefCostCode"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptProfitCenterMaster.rpt"), String)
                        rptreportname = "Report - Profit Center Master"
                        Exit Select

                    Case "Cost Center Group"
                        If Request.QueryString("Code") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;Cost Center Group Code: " & Request.QueryString("Code")
                                strSelectionFormula = strSelectionFormula & " and {costcentergroup_master.costcentergrp_code} LIKE '" & Request.QueryString("Code") & "*'"
                            Else
                                strReportTitle = " Cost Center Group Code: " & Request.QueryString("Code")
                                strSelectionFormula = "{costcentergroup_master.costcentergrp_code} LIKE '" & Request.QueryString("Code") & "*'"
                            End If
                        End If

                        If Request.QueryString("Name") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;Cost Center Group Name: " & Request.QueryString("Name")
                                strSelectionFormula = strSelectionFormula & " and {costcentergroup_master.costcentergrp_name} LIKE '" & Request.QueryString("Name") & "*'"
                            Else
                                strReportTitle = " Cost Center Group Name: " & Request.QueryString("Name")
                                strSelectionFormula = "{costcentergroup_master.costcentergrp_name} LIKE '" & Request.QueryString("Name") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptCostCenterGroupMaster.rpt"), String)
                        rptreportname = "Report - Cost Center Group Master"
                        Exit Select

                    Case "Cost Center"
                        If Request.QueryString("CCCode") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;Cost Center Code: " & Request.QueryString("CCCode")
                                strSelectionFormula = strSelectionFormula & " and {costcenter_master.costcenter_code} LIKE '" & Request.QueryString("CCCode") & "*'"
                            Else
                                strReportTitle = " Cost Center Code: " & Request.QueryString("CCCode")
                                strSelectionFormula = "{costcenter_master.costcenter_code} LIKE '" & Request.QueryString("CCCode") & "*'"
                            End If
                        End If

                        If Request.QueryString("CCName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;Cost Center Name: " & Request.QueryString("CCName")
                                strSelectionFormula = strSelectionFormula & " and {costcenter_master.costcenter_name} LIKE '" & Request.QueryString("CCName") & "*'"
                            Else
                                strReportTitle = " Cost Center Name: " & Request.QueryString("CCName")
                                strSelectionFormula = "{costcenter_master.costcenter_name} LIKE '" & Request.QueryString("CCName") & "*'"
                            End If
                        End If

                        If Request.QueryString("GrpCode") <> "[Select]" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;Cost Center Group : " & Request.QueryString("GrpCode")
                                strSelectionFormula = strSelectionFormula & " and {costcenter_master.costcentergrp_code} LIKE '" & Request.QueryString("GrpCode") & "*'"
                            Else
                                strReportTitle = " Cost Center Group : " & Request.QueryString("GrpCode")
                                strSelectionFormula = "{costcenter_master.costcentergrp_code} LIKE '" & Request.QueryString("GrpCode") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptCostCenterMaster.rpt"), String)
                        rptreportname = "Report - Cost Center Master"
                        Exit Select

                    Case "Narration Template"
                        If Request.QueryString("Code") <> "" Then
                            strReportTitle = "Narration Code: " & CType(Request.QueryString("Code"), String)
                            strSelectionFormula = "{narration.code} LIKE '" & CType(Request.QueryString("Code"), String) & "*'"
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptNarrationTemplate.rpt"), String)
                        rptreportname = "Report - Narration Template"
                        Exit Select

                    Case "DefineControlAccountsSupplierAgents"
                        If Request.QueryString("SupAgentCode") <> "[Select]" Then
                            If Trim(strSelectionFormula) = "" Then
                                strReportTitle = "Supplier Agent : " & CType(Request.QueryString("SupAgentCode"), String)
                                strSelectionFormula = " {supplier_agents.supagentcode} = '" & CType(Request.QueryString("SupAgentCode"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & "Supplier Agent : " & CType(Request.QueryString("SupAgentCode"), String)
                                strSelectionFormula = strSelectionFormula & " AND {supplier_agents.supagentcode}  = '" & CType(Request.QueryString("SupAgentCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SPtypeCode") <> "[Select]" Then
                            If Trim(strSelectionFormula) = "" Then
                                strReportTitle = "Sp Type Code " & CType(Request.QueryString("SPtypeCode"), String)
                                strSelectionFormula = " {supplier_agents.sptypecode}  = '" & CType(Request.QueryString("SPtypeCode"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & " ;Sp Type Code " & CType(Request.QueryString("SPtypeCode"), String)
                                strSelectionFormula = strSelectionFormula & " AND {supplier_agents.sptypecode}  = '" & CType(Request.QueryString("SPtypeCode"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptDefAccSupplieragent.rpt"), String)
                        rptreportname = "Report - SupplierAgents Control Accounts "
                        Exit Select

                    Case "DefineControlAccountsSuppliers"
                        If Request.QueryString("SupCode") <> "[Select]" Then
                            If Trim(strSelectionFormula) = "" Then
                                strReportTitle = "Supplier: " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = " {partymast.partycode} = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & "Supplier: " & CType(Request.QueryString("SupCode"), String)
                                strSelectionFormula = strSelectionFormula & " AND {partymast.partycode}  = '" & CType(Request.QueryString("SupCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SPtypeCode") <> "[Select]" Then
                            If Trim(strSelectionFormula) = "" Then
                                strReportTitle = "Sp Type Code " & CType(Request.QueryString("SPtypeCode"), String)
                                strSelectionFormula = " {partymast.sptypecode}  = '" & CType(Request.QueryString("SPtypeCode"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & " ;Sp Type Code " & CType(Request.QueryString("SPtypeCode"), String)
                                strSelectionFormula = strSelectionFormula & " AND {partymast.sptypecode}  = '" & CType(Request.QueryString("SPtypeCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("SupCat") <> "[Select]" Then
                            If Trim(strSelectionFormula) = "" Then
                                strReportTitle = "Category Code " & CType(Request.QueryString("SupCat"), String)
                                strSelectionFormula = " {partymast.catcode}  = '" & CType(Request.QueryString("SupCat"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & " ;Category Code " & CType(Request.QueryString("SupCat"), String)
                                strSelectionFormula = strSelectionFormula & " AND {partymast.catcode}  = '" & CType(Request.QueryString("SupCat"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptDefAccSupplier.rpt"), String)
                        rptreportname = "Report - Suppliers Control Accounts "
                        Exit Select

                    Case "DefineControlAccountsCustomers"
                        If Request.QueryString("CustCode") <> "[Select]" Then
                            If Trim(strSelectionFormula) = "" Then
                                strReportTitle = "Customer: " & CType(Request.QueryString("CustCode"), String)
                                strSelectionFormula = " {agentmast.agentcode} = '" & CType(Request.QueryString("CustCode"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & "Customer: " & CType(Request.QueryString("CustCode"), String)
                                strSelectionFormula = strSelectionFormula & " AND {agentmast.agentcode}  = '" & CType(Request.QueryString("CustCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("CustCat") <> "[Select]" Then
                            If Trim(strSelectionFormula) = "" Then
                                strReportTitle = "Category Code " & CType(Request.QueryString("CustCat"), String)
                                strSelectionFormula = " {agentmast.catcode}  = '" & CType(Request.QueryString("CustCat"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & " ;Category Code " & CType(Request.QueryString("CustCat"), String)
                                strSelectionFormula = strSelectionFormula & " AND {agentmast.catcode}  = '" & CType(Request.QueryString("CustCat"), String) & "'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptDefAccCustomer.rpt"), String)
                        rptreportname = "Report - Customers Control Accounts "
                        Exit Select

                    Case "OtherBankMaster"
                        If Request.QueryString("OthBankCode") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;Bank Code: " & Request.QueryString("OthBankCode")
                                'strSelectionFormula = strSelectionFormula & " and {agentcatmast.agentcatname} LIKE '" & txtCusName.Text.Trim & "*'"
                                strSelectionFormula = strSelectionFormula & " and {customer_bank_master.other_bank_master_code} LIKE '" & Request.QueryString("OthBankCode") & "*'"
                            Else
                                strReportTitle = "Bank Code: " & Request.QueryString("OthBankCode")
                                strSelectionFormula = "{customer_bank_master.other_bank_master_code} LIKE '" & Request.QueryString("OthBankCode") & "*'"
                            End If
                        End If
                        If Request.QueryString("OthBankName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ;Other Bank Name : " & Request.QueryString("OthBankName")
                                strSelectionFormula = strSelectionFormula & " and {customer_bank_master.other_bank_master_des} LIKE '" & Request.QueryString("OthBankName") & "*'"
                            Else
                                strReportTitle = "Other Bank Name : " & Request.QueryString("OthBankName")
                                strSelectionFormula = "{customer_bank_master.other_bank_master_des} LIKE '" & Request.QueryString("OthBankName") & "*'"
                            End If
                        End If
                        strReportName = CType(Server.MapPath("~\Report\rptOtherBankMaster.rpt"), String)
                        rptreportname = "Report - Other Bank Master"
                        Exit Select

                    Case "Journal"
                        ViewState.Add("poststate", Request.QueryString("poststate"))

                        'If strSelectionFormula <> "" Then
                        '    strReportTitle = strReportTitle & " ;Journal Code: " & Request.QueryString("tran_Id")
                        '    strSelectionFormula = strSelectionFormula & " and {journal_detail.tran_id} LIKE '" & Request.QueryString("JVTranId") & "'"
                        'Else
                        '    strReportTitle = "Journal Code: " & Request.QueryString("JVTranId")
                        '    strSelectionFormula = "{journal_detail.tran_id} LIKE '" & Request.QueryString("JVTranId") & "'"
                        'End If



                        'If strSelectionFormula <> "" Then
                        '    If ViewState("poststate") = "P" Then
                        '        strReportTitle = strReportTitle & "; State: Posted "
                        '        strSelectionFormula = strSelectionFormula & " and {journal_master.post_state} LIKE '" & Request.QueryString("poststate") & "'"
                        '    ElseIf ViewState("poststate") = "U" Then
                        '        strReportTitle = strReportTitle & "; State: UnPosted "
                        '        strSelectionFormula = strSelectionFormula & " and {journal_master.post_state} LIKE '" & Request.QueryString("poststate") & "'"
                        '    End If
                        'Else
                        '    If ViewState("poststate") = "P" Then
                        '        strReportTitle = " State: Posted "
                        '        strSelectionFormula = " {journal_master.post_state} LIKE '" & Request.QueryString("poststate") & "'"
                        '    ElseIf ViewState("poststate") = "U" Then
                        '        strReportTitle = "; State: UnPosted "
                        '        strSelectionFormula = " {journal_master.post_state} LIKE '" & Request.QueryString("poststate") & "'"
                        '    End If

                        'End If
                        strReportName = CType(Server.MapPath("~\Report\Journalreport.rpt"), String)
                        rptreportname = "Report - Journal"
                        Exit Select

                End Select

                If strReportName = "" Then
                    'Response.Redirect(CType(Session("BackPageName"), String), False)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
                    Exit Sub
                Else
                    ViewState.Add("RepCalledFrom", 0)
                    btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")

                    'BindReport(strReportName, CType(Session("SelectionFormula"), String), CType(Session("ReportTitle"), String))
                    BindReport(strReportName, strSelectionFormula, strReportTitle)
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


            Dim RepTbls As Tables = repDeocument.Database.Tables

            For Each RepTbl As Table In RepTbls
                Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
                RepTblLogonInfo.ConnectionInfo = ConnInfo
                RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
            Next
            Me.CRVReport.ReportSource = repDeocument

            pnames = repDeocument.DataDefinition.ParameterFields

            If Request.QueryString("Pageame") <> "Logdetails" And Request.QueryString("Pageame") <> "Profit Center Master" Then

                repDeocument.SummaryInfo.ReportTitle = strReportTitle
            End If
            If Request.QueryString("Pageame") <> "Logdetails" Then
                pname = pnames.Item("CompanyName")
                paramvalue.Value = rptcompanyname
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            End If
            If Request.QueryString("Pageame") <> "Logdetails" Then

                pname = pnames.Item("ReportName")
                paramvalue.Value = rptreportname
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

            End If
            pname = pnames.Item("rptFilter")
            paramvalue.Value = strReportTitle
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            If Request.QueryString("Pageame") = "Profit Center Master" Then

                pname = pnames.Item("ReportTitle_text")
                paramvalue.Value = strReportTitle
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            End If



            If Request.QueryString("Pageame") = "Logdetails" Then

                pname = pnames.Item("CompanyName")
                paramvalue.Value = ""
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                'pname = pnames.Item("rptFilter")
                'paramvalue.Value = ""
                'param = pname.CurrentValues
                'param.Add(paramvalue)
                'pname.ApplyCurrentValues(param)




                If Request.QueryString("user") IsNot Nothing Then
                    If Request.QueryString("user") <> "[Select]" Then



                        pname = pnames.Item("ReportName")

                        paramvalue.Value = "Log File for Specific User"
                        param = pname.CurrentValues
                        param.Add(paramvalue)
                        pname.ApplyCurrentValues(param)
                    Else

                        pname = pnames.Item("ReportName")
                        paramvalue.Value = rptreportname
                        param = pname.CurrentValues
                        param.Add(paramvalue)
                        pname.ApplyCurrentValues(param)


                    End If


                End If
            End If
            pname = pnames.Item("cmb")
            paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1050)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            If Request.QueryString("Pageame") = "BankDetails" Or Request.QueryString("Pageame") = "Account Group" Or Request.QueryString("Pageame") = "Accounts Master" Then

                pname = pnames.Item("divcode")
                paramvalue.Value = ViewState("divcode")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            End If

            If ViewState("Pageame") = "OpeningTrailBalance" Then
                pname = pnames.Item("@tran_id")
                paramvalue.Value = ViewState("TranID")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@tran_type")
                paramvalue.Value = ViewState("TranType")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@type")
                paramvalue.Value = ViewState("Type")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@orderby")
                paramvalue.Value = 0
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@div_code")
                paramvalue.Value = ViewState("divcode")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            End If
            If ViewState("Pageame") = "Journal" Or ViewState("Pageame" = "Promotion") Then
                pname = pnames.Item("addrLine1")
                paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1063)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("addrLine2")
                paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1064)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("addrLine3")
                paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1065)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("addrLine4")
                paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1066)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)


                pname = pnames.Item("addrLine5")
                paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1053)
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
        'If CType(Session("BackPageName"), String) = "" Then
        '    Response.Redirect("MainPage.aspx", False)
        '    Exit Sub
        'Else
        '    Session("ColReportParams") = Nothing
        '    Response.Redirect(CType(Session("BackPageName"), String), False)
        'End If

        If CType(ViewState("BackPageName"), String) = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
            Exit Sub
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        End If

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If ViewState("RepCalledFrom") <> 1 Then
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

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Session.Add("ReportSource", repDeocument)
        ViewState.Add("RepCalledFrom", 1)
        Dim strpop As String = ""
        strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
End Class

