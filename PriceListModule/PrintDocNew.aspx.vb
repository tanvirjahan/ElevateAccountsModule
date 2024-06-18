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

Partial Class PriceListModule_PrintDocNew
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim repDeocument As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim strReportName As String = ""
    Dim objutils As New clsUtils
    Dim strReportTitle As String = ""
    Dim strSelectionFormula As String = ""
#End Region


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Not Session("CompanyName") Is Nothing Then
            Me.Page.Title = CType(Session("CompanyName"), String)
        End If

        rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
        If Request.QueryString("BackPageName") <> "" Then
            ViewState("BackPageName") = Request.QueryString("BackPageName")
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
            Else
                Select Case CType(ViewState("Pageame"), String)
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
                            " and  {providerinv_header.div_id} = '" & CType(objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511), String) & "'"
                        Else
                            strSelectionFormula = strSelectionFormula & " AND {providerinv_header.tran_type} = '" & Request.QueryString("TranType") & "'" & _
                            " and  {providerinv_header.div_id} = '" & CType(objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511), String) & "'"
                        End If

                        rptreportname = "Purchase Invoice"
                        strReportName = CType(Server.MapPath("~\Report\rptPurchaseInvoice.rpt"), String)
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
                            " and  {supplierinvoice_header.div_id} = '" & CType(objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511), String) & "'"
                        Else
                            strSelectionFormula = strSelectionFormula & " AND {supplierinvoice_header.tran_type} = '" & Request.QueryString("TranType") & "'" & _
                            " and  {supplierinvoice_header.div_id} = '" & CType(objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511), String) & "'"
                        End If

                        rptreportname = "Supplier Statement Matching"
                        strReportName = CType(Server.MapPath("~\Report\rptupdatesupplier.rpt"), String)
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
                    Case "ExchangediffDoc"
                        If Request.QueryString("TranId") <> "" Then
                            If Trim(strSelectionFormula) = "" Then
                                strSelectionFormula = "  {exchange_master.tran_id}='" & Request.QueryString("TranId") & "'"
                            Else
                                strSelectionFormula = strSelectionFormula & " AND  {exchange_master.tran_id}='" & Request.QueryString("Tranid") & "'"
                            End If
                        End If

                        If Trim(strSelectionFormula) = "" Then
                            strSelectionFormula = " {exchange_master.tran_type} = '" & Request.QueryString("TranType") & "' " & _
                            " and  {exchange_master.journal_div_id} = '" & CType(objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511), String) & "'"
                        Else
                            strSelectionFormula = strSelectionFormula & " and {exchange_master.tran_type}= '" & Request.QueryString("TranType") & "'" & _
                            " and  {exchange_master.journal_div_id} = '" & CType(objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511), String) & "' "
                        End If
                        Session("ColReportParams") = Nothing
                        rptreportname = "Exchange Voucher"
                        strReportName = CType(Server.MapPath("~\Report\rptExchange.rpt"), String)
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
                            " and  {trdpurchase_master.div_id} = '" & CType(objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511), String) & "'"
                        Else
                            strSelectionFormula = strSelectionFormula & " AND {trdpurchase_master.tran_type} = '" & Request.QueryString("TranType") & "'" & _
                            " and  {trdpurchase_master.div_id} = '" & CType(objutils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511), String) & "'"
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
                    Case "PackageCostDoc"
                        Session("ColReportParams") = Nothing
                        rptreportname = "Package"
                        strReportName = CType(Server.MapPath("~\Report\rptPackage_Main.rpt"), String)
                        Exit Select
                    Case "PackageSellingDoc"
                        Session("ColReportParams") = Nothing
                        rptreportname = "Package"
                        strReportName = CType(Server.MapPath("~\Report\rptPackage_MainReport.rpt"), String)
                        Exit Select
                    Case "GroupQuotationMainDoc"
                        Session("ColReportParams") = Nothing
                        rptreportname = "Group Quotation"
                        strReportName = CType(Server.MapPath("~\Report\rptGroupQuotation_Main.rpt"), String)
                        Exit Select

                    Case "PrepaidAllotDoc"
                        If Request.QueryString("PrepaidAllotId") <> "" Then
                            strSelectionFormula = "{preallotment_header.preallotid} = '" & Request.QueryString("PrepaidAllotId") & "'"
                        End If

                        Session("ColReportParams") = Nothing
                        rptreportname = "Payment Requisition"
                        strReportName = CType(Server.MapPath("~\Report\rptprepaidallotment.rpt"), String)
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
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
                Else
                    BindReport(strReportName, CType(strSelectionFormula, String), CType(strReportTitle, String))

                End If

            End If
        End If
    End Sub
    Private Sub BindReport(ByVal ReportName As String, ByVal strSelectionFormula As String, ByVal strReportTitle As String)
        Try
            Dim pnames As ParameterFieldDefinitions
            Dim pname As ParameterFieldDefinition
            Dim param As New ParameterValues
            Dim paramvalue As New ParameterDiscreteValue

            Dim ConnInfo As New ConnectionInfo
            With ConnInfo
                .ServerName = Session("dbServerName")        'ConfigurationManager.AppSettings("dbServerName")
                .DatabaseName = Session("dbDatabaseName")    'ConfigurationManager.AppSettings("dbDatabaseName")
                .UserID = Session("dbUserName")              'ConfigurationManager.AppSettings("dbUserName")
                .Password = Session("dbPassword")            'ConfigurationManager.AppSettings("dbPassword")
            End With

            repDeocument.Load(ReportName)

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

            If rptreportname = "Online Payment Receipt" Or rptreportname = "Online Payment Details" Then
                pname = pnames.Item("noofdecimals")
                paramvalue.Value = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), String)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            End If

            pname = pnames.Item("CompanyName")
            paramvalue.Value = rptcompanyname
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            'pname = pnames.Item("ReportName")
            'paramvalue.Value = rptreportname
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            Dim ctry As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=459"), String)

            pname = pnames.Item("ReportName")
            paramvalue.Value = rptreportname + IIf(ctry = "UAE", "", "  -  OMAN")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

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
            If CType(ViewState("Pageame"), String) = "PackageCostDoc" Or CType(ViewState("Pageame"), String) = "PackageSellingDoc" Then
                pname = pnames.Item("@pkgid")
                paramvalue.Value = CType(Request.QueryString("CostPkgId"), String)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            End If

            If CType(ViewState("Pageame"), String) = "PackageSellingDoc" Then
                pname = pnames.Item("@pkgrowid")
                paramvalue.Value = CType(Request.QueryString("Costpkgrowid"), String)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            End If
            If CType(ViewState("Pageame"), String) = "GroupQuotationMainDoc" Then
                pname = pnames.Item("@groupquoteid")
                paramvalue.Value = CType(Request.QueryString("GroupQuotationId"), String)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("BaseCurr")
                paramvalue.Value = CType(ViewState("BaseCurr"), String)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            End If
            If CType(ViewState("Pageame"), String) = "PackageSellingDoc" Then
                pname = pnames.Item("@BaseCurr")
                paramvalue.Value = CType(ViewState("BaseCurr"), String)
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


            repDeocument.RecordSelectionFormula = strSelectionFormula
            Response.Buffer = False
            Response.ClearContent()
            Response.ClearHeaders()
            repDeocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, False, "Report")

        Catch ex As Exception

        Finally
            repDeocument.Dispose()
        End Try

    End Sub
End Class
