Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

Partial Class PrintdocNew
    Inherits System.Web.UI.Page
    Dim repDeocument As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim strSelectionFormula As String = ""
    Dim strReportTitle As String = ""
    Dim strReportName As String = ""

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not Session("CompanyName") Is Nothing Then
            Me.Page.Title = CType(Session("CompanyName"), String)
        End If

        '  rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
        If Request.QueryString("BackPageName") <> "" Then
            ViewState("BackPageName") = Request.QueryString("BackPageName")
        End If
        If Request.QueryString("divid") <> "" Then
            ViewState("divcode") = Request.QueryString("divid")
        End If

        If ViewState("divcode") <> "" Then
            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & ViewState("divcode") & "'"), String)
        Else
            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
        End If

        If Request.QueryString("BaseCurr") Is Nothing = False Then
            ViewState("BaseCurr") = Request.QueryString("BaseCurr")
        Else
            ViewState("BaseCurr") = ""
        End If
        If Request.QueryString("Pageame") <> "" Then
            ViewState("Pageame") = Request.QueryString("Pageame")
        End If
        If CType(ViewState("BackPageName"), String) = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)

        Else
            If CType(ViewState("Pageame"), String) = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
                Exit Sub
            Else
                Select Case CType(ViewState("Pageame"), String)
                    Case "PreferredSupplier"
                        'If Request.QueryString("SupTypeCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & "; Supplier Type: " & Request.QueryString("SupTypeName")
                        '        strSelectionFormula = strSelectionFormula & " and {partymast.sptypecode} = '" & Request.QueryString("SupTypeCode") & "'"
                        '    Else
                        '        strReportTitle = "Supplier Type: " & Request.QueryString("SupTypeName")
                        '        strSelectionFormula = "{partymast.sptypecode} = '" & Request.QueryString("SupTypeCode") & "'"
                        '    End If
                        'End If
                        'If Request.QueryString("CatCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & "; Category: " & Request.QueryString("CatName")
                        '        strSelectionFormula = strSelectionFormula & " and {partymast.catcode} = '" & Request.QueryString("CatCode") & "'"
                        '    Else
                        '        strReportTitle = "Category: " & Request.QueryString("CatName")
                        '        strSelectionFormula = "{partymast.catcode} = '" & Request.QueryString("CatCode") & "'"
                        '    End If
                        'End If
                        'If Request.QueryString("CtryCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & "; Country: " & Request.QueryString("CtryName")
                        '        strSelectionFormula = strSelectionFormula & " and {ctrymast.ctrycode} = '" & Request.QueryString("CtryCode") & "'"
                        '    Else
                        '        strReportTitle = "Country: " & Request.QueryString("CtryName")
                        '        strSelectionFormula = "{ctrymast.ctrycode} = '" & Request.QueryString("CtryCode") & "'"
                        '    End If
                        'End If
                        'If Request.QueryString("SectCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & "; Sector: " & Request.QueryString("SectName")
                        '        strSelectionFormula = strSelectionFormula & " and {sectormaster.sectorcode} = '" & Request.QueryString("SectCode") & "'"
                        '    Else
                        '        strReportTitle = "Sector: " & Request.QueryString("SectName")
                        '        strSelectionFormula = "{sectormaster.sectorcode} = '" & Request.QueryString("SectCode") & "'"
                        '    End If
                        'End If
                        'If Request.QueryString("CityCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & "; City: " & Request.QueryString("CityName")
                        '        strSelectionFormula = strSelectionFormula & " and {partymast.citycode} = '" & Request.QueryString("CityCode") & "'"
                        '    Else
                        '        strReportTitle = "City: " & Request.QueryString("CityName")
                        '        strSelectionFormula = "{partymast.citycode} = '" & Request.QueryString("CityCode") & "'"
                        '    End If
                        'End If

                        'If strSelectionFormula <> "" Then
                        '    strSelectionFormula = strSelectionFormula & " and {partymast.preferred}=1 and {partymast.active}=1"

                        'Else
                        '    strSelectionFormula = "{partymast.preferred}=1 and {partymast.active}=1"

                        'End If
                        strReportName = CType(Server.MapPath("~\Report\rptPreferredSuppliers.rpt"), String)
                        rptreportname = "Report - Preferred Suppliers"
                        Exit Select
                    Case "ApproveCustomersforWeb"
                        'If Request.QueryString("SellTypeCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & "; Selling Type Code : " & Request.QueryString("SellTypeCode")
                        '        strSelectionFormula = strSelectionFormula & " and {agentmast.sellcode} = '" & Request.QueryString("SellTypeCode") & "'"
                        '    Else
                        '        strReportTitle = "Selling Type  Code: " & Request.QueryString("SellTypeCode")
                        '        strSelectionFormula = "{agentmast.sellcode} = '" & Request.QueryString("SellTypeCode") & "'"
                        '    End If
                        'End If
                        'If Request.QueryString("CatCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & "; Category Code : " & Request.QueryString("CatCode")
                        '        strSelectionFormula = strSelectionFormula & " and {agentmast.catcode} = '" & Request.QueryString("CatCode") & "'"
                        '    Else
                        '        strReportTitle = "Category Code: " & Request.QueryString("CatCode")
                        '        strSelectionFormula = "{agentmast.catcode} = '" & Request.QueryString("CatCode") & "'"
                        '    End If
                        'End If
                        'If Request.QueryString("CtryCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & "; Country Code : " & Request.QueryString("CtryCode")
                        '        strSelectionFormula = strSelectionFormula & " and {agentmast.ctrycode} = '" & Request.QueryString("CtryCode") & "'"
                        '    Else
                        '        strReportTitle = "Country Code: " & Request.QueryString("CtryCode")
                        '        strSelectionFormula = "{agentmast.ctrycode} = '" & Request.QueryString("CtryCode") & "'"
                        '    End If
                        'End If
                        'If Request.QueryString("MktCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Market Code : " & Request.QueryString("MktCode")
                        '        strSelectionFormula = strSelectionFormula & " and {agentmast.plgrpcode} = '" & Request.QueryString("MktCode") & "'"
                        '    Else
                        '        strReportTitle = "Market Code: " & Request.QueryString("MktCode")
                        '        strSelectionFormula = "{agentmast.plgrpcode} = '" & Request.QueryString("MktCode") & "'"
                        '    End If
                        'End If

                        'If Request.QueryString("CityCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & "; City Code: " & Request.QueryString("CityCode")
                        '        strSelectionFormula = strSelectionFormula & " and {agentmast.citycode} = '" & Request.QueryString("CityCode") & "'"
                        '    Else
                        '        strReportTitle = "City Code: " & Request.QueryString("CityCode")
                        '        strSelectionFormula = "{agentmast.citycode} = '" & Request.QueryString("CityCode") & "'"
                        '    End If
                        'End If

                        strReportName = CType(Server.MapPath("~\Report\rptApproveCustomersforWeb.rpt"), String)
                        rptreportname = "Report - Approve Customers for Web"
                        Exit Select
                    Case "LockAgentsforWeb"
                        'If Request.QueryString("SellTypeCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & "; Selling Type Code : " & Request.QueryString("SellTypeCode")
                        '        strSelectionFormula = strSelectionFormula & " and {agentmast.sellcode} = '" & Request.QueryString("SellTypeCode") & "'"
                        '    Else
                        '        strReportTitle = "Selling Type  Code: " & Request.QueryString("SellTypeCode")
                        '        strSelectionFormula = "{agentmast.sellcode} = '" & Request.QueryString("SellTypeCode") & "'"
                        '    End If
                        'End If
                        'If Request.QueryString("CatCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & "; Category Code : " & Request.QueryString("CatCode")
                        '        strSelectionFormula = strSelectionFormula & " and {agentmast.catcode} = '" & Request.QueryString("CatCode") & "'"
                        '    Else
                        '        strReportTitle = "Category Code: " & Request.QueryString("CatCode")
                        '        strSelectionFormula = "{agentmast.catcode} = '" & Request.QueryString("CatCode") & "'"
                        '    End If
                        'End If
                        'If Request.QueryString("CtryCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & "; Country Code : " & Request.QueryString("CtryCode")
                        '        strSelectionFormula = strSelectionFormula & " and {agentmast.ctrycode} = '" & Request.QueryString("CtryCode") & "'"
                        '    Else
                        '        strReportTitle = "Country Code: " & Request.QueryString("CtryCode")
                        '        strSelectionFormula = "{agentmast.ctrycode} = '" & Request.QueryString("CtryCode") & "'"
                        '    End If
                        'End If
                        'If Request.QueryString("MktCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & " ; Market Code : " & Request.QueryString("MktCode")
                        '        strSelectionFormula = strSelectionFormula & " and {agentmast.plgrpcode} = '" & Request.QueryString("MktCode") & "'"
                        '    Else
                        '        strReportTitle = "Market Code: " & Request.QueryString("MktCode")
                        '        strSelectionFormula = "{agentmast.plgrpcode} = '" & Request.QueryString("MktCode") & "'"
                        '    End If
                        'End If

                        'If Request.QueryString("CityCode") <> "[Select]" Then
                        '    If strSelectionFormula <> "" Then
                        '        strReportTitle = strReportTitle & "; City Code: " & Request.QueryString("CityCode")
                        '        strSelectionFormula = strSelectionFormula & " and {agentmast.citycode} = '" & Request.QueryString("CityCode") & "'"
                        '    Else
                        '        strReportTitle = "City Code: " & Request.QueryString("CityCode")
                        '        strSelectionFormula = "{agentmast.citycode} = '" & Request.QueryString("CityCode") & "'"
                        '    End If
                        'End If

                        strReportName = CType(Server.MapPath("~\Report\rptLockAgentsforWeb.rpt"), String)
                        rptreportname = "Report - Lock Agents for Web"
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
                            " and  {journal_master.journal_div_id} = '" & ViewState("divcode") & "'"
                        Else
                            strSelectionFormula = strSelectionFormula & " and {journal_master.tran_type}= '" & Request.QueryString("TranType") & "'" & _
                            " and  {journal_master.journal_div_id} = '" & ViewState("divcode") & "'"
                        End If
                        Session("ColReportParams") = Nothing
                        rptreportname = "Journal Voucher"
                        strReportName = CType(Server.MapPath("~\Report\rptJournal.rpt"), String)
                        Exit Select

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
                            " and  {receipt_master_new.div_id} = '" & ViewState("divcode") & "'"
                        Else
                            strSelectionFormula = strSelectionFormula & " AND {receipt_master_new.tran_type}= '" & Request.QueryString("TranType") & "'" & _
                            " and {receipt_master_new.div_id}= '" & ViewState("divcode") & "'"
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
                            " and  {matchos_master.div_id} = '" & ViewState("divcode") & "'"
                        Else
                            strSelectionFormula = strSelectionFormula & " AND {matchos_master.tran_type} = '" & Request.QueryString("TranType") & "'" & _
                            " and  {matchos_master.div_id} ='" & ViewState("divcode") & "'"
                        End If

                        rptreportname = "Match Outstanding"
                        If Request.QueryString("Curr") <> "" Then
                            strReportName = CType(Server.MapPath("~\Report\rptMatchOutstandclient.rpt"), String)
                        Else
                            strReportName = CType(Server.MapPath("~\Report\rptMatchOutstand.rpt"), String)
                        End If

                        Exit Select
                    Case "UpdatesupplierinvoiceDoc"
                        If Request.QueryString("PInvoiceNo") <> "" Then
                            If Trim(strSelectionFormula) = "" Then
                                strSelectionFormula = " {supplierinvoice_header.tran_id}='" & Request.QueryString("PInvoiceNo") & "'"
                            Else
                                strSelectionFormula = strSelectionFormula & " AND {supplierinvoice_header.tran_id}='" & Request.QueryString("PInvoiceNo") & "'"
                            End If
                        End If

                        If Trim(strSelectionFormula) = "" Then
                            strSelectionFormula = " {supplierinvoice_header.tran_type} = '" & Request.QueryString("TranType") & "' " & _
                            " and  {supplierinvoice_header.div_id} ='" & ViewState("divcode") & "'"
                        Else
                            strSelectionFormula = strSelectionFormula & " AND {supplierinvoice_header.tran_type} = '" & Request.QueryString("TranType") & "'" & _
                            " and  {supplierinvoice_header.div_id} = '" & ViewState("divcode") & "'"
                        End If

                        rptreportname = "Supplier Statement Matching"
                        strReportName = CType(Server.MapPath("~\Report\rptupdatesupplier.rpt"), String)
                        Exit Select
                    Case "PurchaseInvoiceDoc"
                        If Request.QueryString("PInvoiceNo") <> "" Then
                            If Trim(strSelectionFormula) = "" Then
                                strSelectionFormula = " {providerinv_header.tran_id}='" & Request.QueryString("PInvoiceNo") & "'"
                            Else
                                strSelectionFormula = strSelectionFormula & " AND {providerinv_header.tran_id}='" & Request.QueryString("PInvoiceNo") & "'"
                            End If
                        End If

                        If Trim(strSelectionFormula) = "" Then
                            strSelectionFormula = " {providerinv_header.tran_type} = '" & Request.QueryString("TranType") & "' " & _
                            " and  {providerinv_header.div_id} = '" & ViewState("divcode") & "'"
                        Else
                            strSelectionFormula = strSelectionFormula & " AND {providerinv_header.tran_type} = '" & Request.QueryString("TranType") & "'" & _
                            " and  {providerinv_header.div_id} = '" & ViewState("divcode") & "'"
                        End If

                        rptreportname = "Purchase Invoice"
                        strReportName = CType(Server.MapPath("~\Report\rptPurchaseInvoice.rpt"), String)
                        Exit Select
                    Case "PaymentReceipt"
                        If Request.QueryString("PaymentID") <> "" Then
                            strSelectionFormula = "{Deduction_Master.PaymentID} = '" & Request.QueryString("PaymentID") & "'"
                        End If

                        Session("ColReportParams") = Nothing
                        rptreportname = "Online Payment Details"
                        strReportName = CType(Server.MapPath("~\Report\rptreceipt_payment.rpt"), String)
                        Exit Select
                    Case "webapproval"


                        If Request.QueryString("marketcode") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Market Code : " & Request.QueryString("marketcode")
                                strSelectionFormula = strSelectionFormula & " and {sp_webapproval_list;1.plgrpcode}='" & Request.QueryString("marketcode") & "'"
                            Else
                                strReportTitle = "Market Code : " & Request.QueryString("marketcode")
                                strSelectionFormula = "{sp_webapproval_list;1.plgrpcode} ='" & Request.QueryString("marketcode") & "'"
                            End If
                        End If

                        If Request.QueryString("sellcode") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Sell Code : " & Request.QueryString("sellcode")
                                strSelectionFormula = strSelectionFormula & " and {sp_webapproval_list;1.sellcode} = '" & Request.QueryString("sellcode") & "'"
                            Else
                                strReportTitle = "Agent code : " & Request.QueryString("sellcode")
                                strSelectionFormula = "{sp_webapproval_list;1.sellcode} = '" & Request.QueryString("custcode") & "'"
                            End If
                        End If

                        If Request.QueryString("custcode") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Agent code : " & Request.QueryString("custcode")
                                strSelectionFormula = strSelectionFormula & " and {sp_webapproval_list;1.agentcode} LIKE '" & Request.QueryString("custcode") & "*'"
                            Else
                                strReportTitle = "Agent code : " & Request.QueryString("custcode")
                                strSelectionFormula = "{sp_webapproval_list;1.agentcode} LIKE '" & Request.QueryString("custcode") & "*'"
                            End If
                        End If

                        If Request.QueryString("CustName") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Agent Name : " & Request.QueryString("CustName")
                                strSelectionFormula = strSelectionFormula & " and {sp_webapproval_list;1.agentname} LIKE '" & Request.QueryString("CustName") & "*'"
                            Else
                                strReportTitle = "Agent Name : " & Request.QueryString("CustName")
                                strSelectionFormula = "{sp_webapproval_list;1.agentname} LIKE '" & Request.QueryString("CustName") & "*'"
                            End If
                        End If



                        If Request.QueryString("status") <> "" Then
                            If strSelectionFormula <> "" Then
                                If Request.QueryString("status") = "A" Then
                                    strReportTitle = strReportTitle & " ; Status : " & "Approved"
                                    strSelectionFormula = strSelectionFormula & " and {sp_webapproval_list;1.webapprove} = 1"
                                Else
                                    If Request.QueryString("status") = "U" Then
                                        strReportTitle = strReportTitle & "  Status : " & "Un Approved"
                                        strSelectionFormula = strSelectionFormula & " and {sp_webapproval_list;1.webapprove} = 0 "
                                    End If
                                End If
                            Else
                                If Request.QueryString("status") = "A" Then
                                    strReportTitle = strReportTitle & " ; Status : " & "Approved"
                                    strSelectionFormula = strSelectionFormula & "  {sp_webapproval_list;1.webapprove} = 1"
                                Else
                                    If Request.QueryString("status") = "U" Then
                                        strReportTitle = strReportTitle & "  Status : " & "Un Approved"
                                        strSelectionFormula = strSelectionFormula & "  {sp_webapproval_list;1.webapprove} = 0 "
                                    End If
                                End If
                            End If

                        End If



                        If Trim(Request.QueryString("CtryCode")) <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Country : " & CType(Request.QueryString("CtryCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sp_webapproval_list;1.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                            Else
                                strReportTitle = "Country : " & CType(Request.QueryString("CtryCode"), String)
                                strSelectionFormula = "{sp_webapproval_list;1.ctrycode} = '" & CType(Request.QueryString("CtryCode"), String) & "'"
                            End If
                        End If
                        If Request.QueryString("CityCode") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; City : " & CType(Request.QueryString("CityCode"), String)
                                strSelectionFormula = strSelectionFormula & " and {sp_webapproval_list;1.citycode} = '" & CType(Request.QueryString("CityCode"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & "  City : " & CType(Request.QueryString("CityCode"), String)
                                strSelectionFormula = "{sp_webapproval_list;1.citycode} = '" & CType(Request.QueryString("CityCode"), String) & "'"
                            End If
                        End If

                        If Request.QueryString("category") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; Category : " & CType(Request.QueryString("category"), String)
                                strSelectionFormula = strSelectionFormula & " and {sp_webapproval_list;1.catcode} = '" & CType(Request.QueryString("category"), String) & "'"
                            Else
                                strReportTitle = strReportTitle & "  Category : " & CType(Request.QueryString("category"), String)
                                strSelectionFormula = "{sp_webapproval_list;1.catcode} = '" & CType(Request.QueryString("category"), String) & "'"
                            End If
                        End If


                        If Request.QueryString("fromdate") <> "" And Request.QueryString("todate") <> "" Then
                            If strSelectionFormula <> "" Then
                                strReportTitle = strReportTitle & " ; From Date: " & Format(CType(Request.QueryString("fromdate"), Date), "dd/MM/yyyy").ToString
                                strReportTitle = strReportTitle & " ; To Date: " & Format(CType(Request.QueryString("todate"), Date), "dd/MM/yyyy").ToString
                                strSelectionFormula = strSelectionFormula & " AND (({sp_webapproval_list;1.appdate} IN Date('" & Format(CType(Request.QueryString("fromdate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("todate"), Date), "yyyy/MM/dd 00:00:00 ") & "'))) "
                            Else
                                strReportTitle = strReportTitle & "  From Date: " & Format(CType(Request.QueryString("fromdate"), Date), "dd/MM/yyyy").ToString
                                strReportTitle = strReportTitle & "  To Date: " & Format(CType(Request.QueryString("todate"), Date), "dd/MM/yyyy").ToString
                                strSelectionFormula = strSelectionFormula & "  (({sp_webapproval_list;1.appdate} IN Date('" & Format(CType(Request.QueryString("fromdate"), Date), "yyyy/MM/dd 00:00:00 ") & "') "
                                strSelectionFormula = strSelectionFormula & " TO Date('" & Format(CType(Request.QueryString("todate"), Date), "yyyy/MM/dd 00:00:00 ") & "'))) "

                            End If
                        End If

                        strReportName = CType(Server.MapPath("~\Report\rptWebapproval_list.rpt"), String)
                        rptreportname = "Report - Client Web Approval"
                        Exit Select

                    Case "salesinvoicefreeform"
                        rptreportname = "salesinvoicefreeform"
                        strReportName = CType(Server.MapPath("~\Report\salesinvoicefreeform.rpt"), String)
                        Exit Select

                    Case "PaymentReceipt"
                        If Request.QueryString("PaymentID") <> "" Then
                            strSelectionFormula = "{Deduction_Master.PaymentID} = '" & Request.QueryString("PaymentID") & "'"
                        End If

                        Session("ColReportParams") = Nothing
                        rptreportname = "Online Payment Details"
                        strReportName = CType(Server.MapPath("~\Report\rptreceipt_payment.rpt"), String)
                        Exit Select

                End Select
                If strReportName = "" Then
                    'Response.Redirect(CType(Session("BackPageName"), String), False)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
                    Exit Sub
                Else
                    ViewState.Add("RepCalledFrom", 0)
                    btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")
                    BindReport(strReportName, strSelectionFormula, strReportTitle)
                End If

            End If
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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


        If rptreportname = "Receipt Voucher" Or rptreportname = "Cash Payment Voucher" Or rptreportname = "Bank Payment Voucher" Or rptreportname = "Journal Voucher" Then
            pname = pnames.Item("cmb")
            paramvalue.Value = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1050"), String)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("prntsec")
            paramvalue.Value = Request.QueryString("PrntSec")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
        End If

        If CType(ViewState("Pageame"), String) = "ReceiptDoc" Then
            pname = pnames.Item("decno")
            paramvalue.Value = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), String)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        End If
        If CType(ViewState("Pageame"), String) = "ReceiptDoc" Or CType(ViewState("Pageame"), String) = "JournalDoc" Or CType(ViewState("Pageame"), String) = "ExchangediffDoc" Or CType(ViewState("Pageame"), String) = "PurchaseInvoiceDoc" Or CType(ViewState("Pageame"), String) = "PrepaidAllotDoc" Or CType(ViewState("Pageame"), String) = "PackageCostDoc" Or CType(ViewState("Pageame"), String) = "GroupQuotationMainDoc" Or CType(ViewState("Pageame"), String) = "PackageSellingDoc" Then
            pname = pnames.Item("currency")
            paramvalue.Value = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
        End If

        If rptreportname = "salesinvoicefreeform" Then
            pname = pnames.Item("tran_type")
            paramvalue.Value = Request.QueryString("TranType")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("div_code")
            paramvalue.Value = Request.QueryString("divid")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("tran_id")
            paramvalue.Value = Request.QueryString("TranId")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("basecurr")
            paramvalue.Value = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        End If
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
        ' CRVReport.HasToggleGroupTreeButton = False
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'If CType(Session("BackPageName"), String) = "" Then
        '    Response.Redirect("MainPage.aspx", False)
        '    Exit Sub
        'Else
        '    Session("ColReportParams") = Nothing
        '    Response.Redirect(CType(Session("BackPageName"), String), False)
        'End If
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)

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

    Public Sub New()

    End Sub
End Class

