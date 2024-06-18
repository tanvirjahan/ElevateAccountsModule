Imports System
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Globalization
Imports System.Diagnostics

Imports ClosedXML.Excel


Imports System.Collections.Generic

Imports System.Net
Imports System.Linq
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class TransactionReports
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim reportname As String
    Dim bytes As Byte()
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
    Dim documentWidth As Single = 770.0F

    
#End Region

#Region "Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Session("GlobalUserName") Is Nothing Then
            Try
                Dim printId = Request.QueryString("printId")
                Dim reportsType = Request.QueryString("reportsType")
                Dim fileName As String = ""
                If printId = "CustomerLedger" Or printId = "SupplierLedger" Or printId = "GLLedger" Then
                    Dim report As New CustomerLedgerPdf
                    Dim actype = Trim(Request.QueryString("actype"))
                    Dim fromdate = Trim(Request.QueryString("fromdate"))
                    Dim todate = Trim(Request.QueryString("todate"))
                    Dim fromcode = Trim(Request.QueryString("fromcode"))
                    Dim tocode = Trim(Request.QueryString("tocode"))
                    Dim pdcyesno = Trim(Request.QueryString("pdcyesno"))
                    Dim divcode = Trim(Request.QueryString("divid"))
                    Dim ledgertype, led As String
                    Dim fromname As String = String.Empty
                    Dim rptfromname As String = String.Empty
                    If actype = "G" Then
                        ledgertype = Trim(Request.QueryString("reptype"))
                        fromname = Trim(Request.QueryString("fromname"))
                        rptfromname = ",Control Account :"
                    Else
                        ledgertype = Trim(Request.QueryString("ledgertype"))
                        pdcyesno = IIf(pdcyesno = "Yes", 1, 0)
                    End If

                    Dim glname As String = String.Empty
                    Dim custgroup_sp_type = Trim(Request.QueryString("custgroup_sp_type"))
                    Dim type = Request.QueryString("type")
                    Dim fromctry = Trim(Request.QueryString("fromctry"))
                    Dim toctry = Trim(Request.QueryString("toctry"))
                    Dim frommarketcode, tomarketcode, frommkname, ctryname, catname, reportfilter As String
                    frommarketcode = String.Empty
                    tomarketcode = String.Empty
                    frommkname = String.Empty
                    ctryname = String.Empty
                    catname = String.Empty
                    reportfilter = String.Empty
                    led = IIf(type = "S" Or type = "C", IIf(ledgertype = "0", "Summary", "Detailed"), IIf(ledgertype = "1", "Summary", "Detailed"))
                    reportname = led + " Report - " & IIf(actype = "C", "Customer ", IIf(actype = "S", "Supplier ", IIf(actype = "A", "Supplier Agent ", "General "))) & "Ledger"
                    fromname = Trim(Request.QueryString("fromname"))
                    frommkname = Trim(Request.QueryString("frommkname"))
                    ctryname = Trim(Request.QueryString("fromctryname"))
                    catname = Trim(Request.QueryString("fromcatname"))
                    If actype = "C" Then
                        frommarketcode = Trim(Request.QueryString("frommarketcode"))
                        tomarketcode = Trim(Request.QueryString("tomarketcode"))
                        rptfromname = ", Customer Name :"
                        glname = Trim(Request.QueryString("fromglname"))
                    ElseIf actype = "S" Then
                        frommarketcode = Trim(Request.QueryString("fromcity"))
                        tomarketcode = Trim(Request.QueryString("tocity"))
                        rptfromname = ",Supplier Name  :"
                        glname = Trim(Request.QueryString("glname"))
                    ElseIf actype = "A" Then
                        frommarketcode = Trim(Request.QueryString("fromcity"))
                        tomarketcode = Trim(Request.QueryString("tocity"))
                        rptfromname = ",Supplier Agent Name  :"
                        glname = Trim(Request.QueryString("glname"))
                    End If

                    Dim fromcat = Trim(Request.QueryString("fromcat"))
                    Dim tocat = Trim(Request.QueryString("tocat"))
                    Dim fromglcode = Trim(Request.QueryString("fromglcode"))
                    Dim toglcode = Trim(Request.QueryString("toglcode"))
                    Dim currtype = Trim(Request.QueryString("currtype"))
                    Dim ageing = Trim(Request.QueryString("ageing"))
                    Dim ststement = Trim(Request.QueryString("ststement"))
                    Dim web = 0
                    Dim summdet = 0
                    Dim ds As New DataSet
                    bytes = {}

                    If Not (String.IsNullOrEmpty(fromdate)) And Not (String.IsNullOrEmpty(todate)) Then
                        reportfilter = "From Date:" & Space(2) & Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy") & Space(2) & "To :" & Space(2) & Convert.ToDateTime(todate).ToString("dd/MM/yyyy")
                    End If
                    If printId = "GLLedger" Then
                        If Not (String.IsNullOrEmpty(fromname)) Then
                            reportfilter = reportfilter & Space(2) & rptfromname & Space(2) & fromname & Space(2)
                        End If
                    Else
                        If Not (String.IsNullOrEmpty(fromname)) Then
                            'reportfilter = reportfilter & Space(2) & rptfromname & Space(2) & fromname & Space(2) & "To" & Space(2) & fromname reportfilter = reportfilter & Space(2) & rptfromname & Space(2) & fromname & Space(2) & "To" & Space(2) & fromname
                            reportfilter = reportfilter & Space(2) & rptfromname & Space(2) & fromname & Space(2)
                        End If

                    End If

                    If Not (String.IsNullOrEmpty(frommkname)) Then

                        ' reportfilter = reportfilter & Space(2) & IIf(type = "C", ":Market From  ", "Supplier City From") & Space(2) & frommkname & Space(2) & "To" & Space(2) & frommkname
                        reportfilter = reportfilter & Space(2) & IIf(type = "C", ",Market   ", " Supplier City ") & Space(2) & frommkname & Space(2)
                    End If

                    If Not (String.IsNullOrEmpty(ctryname)) Then
                        ' reportfilter = reportfilter & Space(2) & "Supplier Country From :" & Space(2) & ctryname & Space(2) & "To" & Space(2) & ctryname
                        reportfilter = reportfilter & Space(2) & ", Supplier Country  :" & Space(2) & ctryname & Space(2)
                    End If
                    If Not (String.IsNullOrEmpty(catname)) Then
                        ' reportfilter = reportfilter & Space(2) & "Category  From :" & Space(2) & catname & Space(2) & "To" & Space(2) & catname
                        reportfilter = reportfilter & Space(2) & ", Category   :" & Space(2) & catname & Space(2)
                    End If



                    If Not (String.IsNullOrEmpty(glname)) Then
                        '   reportfilter = reportfilter & Space(2) & "Control Account Name From" & Space(2) & glname & Space(2) & "To" & Space(2) & glname
                        reportfilter = reportfilter & Space(2) & ", Control Account : " & Space(2) & glname & Space(2)
                    End If


                    report.CreatePdf(fromdate, todate, reportfilter, reportname, reportsType, actype, type, fromctry, toctry, frommarketcode, tomarketcode, fromcode, tocode, fromcat, tocat, fromglcode, toglcode,
                     pdcyesno, currtype, ledgertype, divcode, ageing, ststement, custgroup_sp_type, bytes, ds, "download")

                ElseIf printId = "CustomerStatement" Or printId = "SupplierStatement" Then
                    Dim report As New CustomerStatement_Report

                    Dim custgroup_sp_type = Trim(Request.QueryString("custgroup_sp_type"))
                    Dim fromdate = Trim(Request.QueryString("fromdate"))
                    Dim todate = Trim(Request.QueryString("todate"))
                    Dim datetype = Request.QueryString("datetype")
                    Dim type = Request.QueryString("type")
                    Dim curr = Trim(Request.QueryString("curr"))
                    Dim fromacct = Trim(Request.QueryString("fromaccode"))
                    Dim toacct = Trim(Request.QueryString("toaccode"))
                    Dim fromcontrol = Trim(Request.QueryString("fromctrlcode"))
                    Dim tocontrol = Trim(Request.QueryString("toctrlcode"))
                    Dim divcode = Trim(Request.QueryString("divid"))
                    Dim fromcat = Trim(Request.QueryString("fromccatcode"))
                    Dim tocat = Trim(Request.QueryString("toccatcode"))
                    Dim fromcity = Trim(Request.QueryString("frommarketcode"))
                    Dim tocity = Trim(Request.QueryString("tomarketcode"))
                    Dim fromctry = Trim(Request.QueryString("fromctrycode"))
                    Dim toctry = Trim(Request.QueryString("toctrycode"))
                    Dim agingtype = Trim(Request.QueryString("agingtype"))
                    Dim pdcyesno = Trim(Request.QueryString("pdcyesno"))
                    Dim includezero = Trim(Request.QueryString("includezero"))
                    Dim remarks = Trim(Request.QueryString("remarks"))
                    Dim rpttype = Trim(Request.QueryString("rpttype"))
                    Dim reporttype = Trim(Request.QueryString("reporttype"))
                    Dim repfilter = Trim(Request.QueryString("repfilter"))
                    Dim reporttitle = Trim(Request.QueryString("reporttitle"))
                    Dim agdate = Trim(Request.QueryString("agdate"))
                    Dim inclproforma = Trim(Request.QueryString("strincludeproforma"))
                    Dim ageingreporttyp = Trim(Request.QueryString("ageingreporttyp"))
                    Dim supptype = Trim(Request.QueryString("supptype"))

                    Dim web = 0
                    Dim summdet = 0
                    Dim ds As New DataSet
                    reportname = printId
                    bytes = {}
                    report.CreatePdf(reportsType, repfilter, datetype, fromdate, todate, type, curr, fromacct, toacct, fromcontrol, tocontrol, fromcat, tocat, fromcity, tocity, fromctry, toctry, agingtype, pdcyesno, includezero, summdet, web, divcode, custgroup_sp_type, inclproforma, bytes, ds, rpttype, "download")

                ElseIf printId = "CustomerTrial" Or printId = "SupplierTrial" Then
                    Dim report As New ClsCustomerTrialPdf
                    Dim custgroup_sp_type = Trim(Request.QueryString("custgroup_sp_type"))
                    Dim fromdate = Trim(Request.QueryString("fromdate"))
                    Dim todate = Trim(Request.QueryString("todate"))
                    Dim type = Trim(Request.QueryString("type"))
                    Dim fromctry = Trim(Request.QueryString("fromctry"))
                    Dim toctry = Trim(Request.QueryString("toctry"))
                    Dim movflg = Request.QueryString("movflg")
                    Dim fromcode = Trim(Request.QueryString("fromcode"))
                    Dim tocode = Trim(Request.QueryString("tocode"))
                    Dim accttype = Request.QueryString("acctype")
                    Dim frommarketcode, frommarketname, tomarketcode, fromcatname, fromsptype, fromname, fromglname, fromctryname, tosptype As String
                    frommarketcode = String.Empty
                    tomarketcode = String.Empty
                    fromsptype = String.Empty
                    tosptype = String.Empty
                    Dim fromcat = Trim(Request.QueryString("fromcat"))
                    Dim tocat = Trim(Request.QueryString("tocat"))
                    Dim fromglcode = Trim(Request.QueryString("fromglcode"))
                    Dim toglcode = Trim(Request.QueryString("toglcode"))
                    Dim curr = Trim(Request.QueryString("currtype"))
                    Dim orderby = Trim(Request.QueryString("orderby"))
                    Dim includezero = Trim(Request.QueryString("includezero"))
                    Dim gpby = Trim(Request.QueryString("gpby"))
                    Dim withcredit = Trim(Request.QueryString("withcredit"))
                    Dim divcode = Trim(Request.QueryString("divid"))
                    Dim trialtype = Trim(Request.QueryString("trialtype"))
                    ' movflg = 2
                    Dim ds As New DataSet
                    bytes = {}
                    Dim reportfilter As String = String.Empty
                    If type = "C" Then
                        reportname = "Customer"
                        frommarketname = Trim(Request.QueryString("frommarketname"))
                        frommarketcode = Trim(Request.QueryString("frommarketcode"))
                        'tomarketcode = Trim(Request.QueryString("tomarketcode"))
                    Else
                        reportname = "Supplier"
                        frommarketname = Trim(Request.QueryString("fromcityname"))
                        frommarketcode = Trim(Request.QueryString("fromcity"))
                        'tomarketcode = Trim(Request.QueryString("tocity"))
                        fromsptype = Trim(Request.QueryString("fromsptype"))
                        '  tosptype = Trim(Request.QueryString("tosptype"))
                    End If
                    fromcatname = Trim(Request.QueryString("fromcatname"))
                    fromctryname = Trim(Request.QueryString("fromctryname"))
                    fromglname = Trim(Request.QueryString("fromglname"))
                    fromname = Trim(Request.QueryString("fromname"))

                    If Not (String.IsNullOrEmpty(fromname)) Then
                        '  reportfilter = reportfilter & Space(2) & IIf(type = "C", reportname, IIf(accttype = "S", reportname, reportname & Space(2) & "Agent")) & "Name From" & Space(2) & fromname & Space(2) & "To" & Space(2) & fromname
                        reportfilter = reportfilter & Space(2) & IIf(type = "C", reportname, IIf(accttype = "S", reportname, reportname & Space(2) & " , Agent")) & " Name  :" & Space(2) & fromname & Space(2)
                    End If
                    'If type = "S" And Not (String.IsNullOrEmpty(fromsptype)) And Not (String.IsNullOrEmpty(tosptype)) Then
                    '    reportFilter = reportFilter & Space(2) & "Supplier City From" & Space(2) & frommarketcode & Space(2) & "To" & Space(2) & tomarketcode
                    'End If

                    If Not (String.IsNullOrEmpty(frommarketname)) Then
                        ' reportfilter = reportfilter & Space(2) & IIf(type = "C", "Market  From", "Supplier City From") & Space(2) & frommarketname & Space(2) & "To" & Space(2) & frommarketname
                        reportfilter = reportfilter & Space(2) & IIf(type = "C", " , Market    :", ",Supplier City   :") & Space(2) & frommarketname & Space(2)
                    End If

                    If Not (String.IsNullOrEmpty(fromctryname)) Then
                        '  reportfilter = reportfilter & Space(2) & "Supplier Country From" & Space(2) & fromctryname & Space(2) & "To" & Space(2) & fromctryname
                        reportfilter = reportfilter & Space(2) & " ,Supplier Country   :" & Space(2) & fromctryname & Space(2)
                    End If
                    If Not (String.IsNullOrEmpty(fromcatname)) Then
                        ' reportfilter = reportfilter & Space(2) & "Category Code From" & Space(2) & fromcatname & Space(2) & "To" & Space(2) & fromcatname
                        reportfilter = reportfilter & Space(2) & " ,Category Code   :" & Space(2) & fromcatname & Space(2)
                    End If
                    If Not (String.IsNullOrEmpty(fromglname)) Then
                        'reportfilter = reportfilter & Space(2) & "Control Account Name From" & Space(2) & fromglname & Space(2) & "To" & Space(2) & fromglname
                        reportfilter = reportfilter & Space(2) & " ,Control Account  :  " & Space(2) & fromglname & Space(2)
                    End If

                    If type = "C" Then
                        'reportname = "Customer"
                        'frommarketcode = Trim(Request.QueryString("frommarketcode"))
                        'tomarketcode = Trim(Request.QueryString("tomarketcode"))
                        If movflg = 0 Then
                            report.CustTrialBal_WithMovement(reportsType, reportfilter, fromdate, todate, fromctry, toctry, movflg, fromcode, tocode, frommarketcode, tomarketcode, fromglcode, toglcode,
                                       orderby, curr, includezero, gpby, withcredit, divcode, trialtype, "C", fromcat, tocat, custgroup_sp_type, reportname, accttype, bytes, ds, "download")
                        ElseIf movflg = 1 Then
                            report.CustTrialBal_WithMovement(reportsType, reportfilter, fromdate, todate, fromctry, toctry, movflg, fromcode, tocode, frommarketcode, tomarketcode, fromglcode, toglcode,
                                       orderby, curr, includezero, gpby, withcredit, divcode, trialtype, "C", fromcat, tocat, custgroup_sp_type, reportname, accttype, bytes, ds, "download")
                        ElseIf movflg = 2 Then
                            report.CustTrialBal(reportsType, fromdate, todate, fromctry, toctry, movflg, fromcode, tocode, frommarketcode, tomarketcode, fromglcode, toglcode,
                                       orderby, curr, includezero, gpby, withcredit, divcode, trialtype, "C", fromcat, tocat, custgroup_sp_type, bytes, ds, "download")
                        End If
                    ElseIf type = "S" Then
                        'reportname = "Supplier"
                        'frommarketcode = Trim(Request.QueryString("fromcity"))
                        'tomarketcode = Trim(Request.QueryString("tocitySupplierTrial
                        'tosptype = Trim(Request.QueryString("tosptype"))
                        '  lscusotmer()
                        If movflg = 0 Then
                            report.CustTrialBal_WithMovement(reportsType, reportfilter, fromdate, todate, fromctry, toctry, movflg, fromcode, tocode, frommarketcode, tomarketcode, fromglcode, toglcode,
                                       orderby, curr, includezero, gpby, withcredit, divcode, trialtype, "S", fromcat, tocat, custgroup_sp_type, reportname, accttype, bytes, ds, "download", fromsptype, tosptype)

                        ElseIf movflg = 1 Then
                            report.CustTrialBal_WithMovement(reportsType, reportfilter, fromdate, todate, fromctry, toctry, movflg, fromcode, tocode, frommarketcode, tomarketcode, fromglcode, toglcode,
                                       orderby, curr, includezero, gpby, withcredit, divcode, trialtype, "S", fromcat, tocat, custgroup_sp_type, reportname, accttype, bytes, ds, "download", fromsptype, tosptype)
                        End If
                    End If
                    reportname = reportname & "Trial Balance"
                ElseIf printId = "CustomerAgeing" Then
                    Dim bc As New ClsCustomerAgeingPdf
                    Dim obj As New CustomerAgeingDetailPdf
                    Dim requestid = Request.QueryString("RequestId")
                    Dim tran_type = Request.QueryString("tran_type")
                    Dim datetype = Trim(Request.QueryString("datetype"))
                    Dim divcode = Trim(Request.QueryString("divid"))
                    Dim custgroup_sp_type = Trim(Request.QueryString("custgroup_sp_type"))
                    Dim fromdate = Trim(Request.QueryString("fromdate"))
                    Dim todate = Trim(Request.QueryString("todate"))

                    Dim Type = Trim(Request.QueryString("type"))
                    Dim currflg = Trim(Request.QueryString("curr"))
                    Dim fromacct = Trim(Request.QueryString("fromaccode"))
                    Dim toacct = Trim(Request.QueryString("toaccode"))
                    Dim fromcontrol = Trim(Request.QueryString("fromctrlcode"))
                    Dim tocontrol = Trim(Request.QueryString("toctrlcode"))
                    Dim fromcat = Trim(Request.QueryString("fromccatcode"))
                    Dim tocat = Trim(Request.QueryString("toccatcode"))
                    Dim fromcity = Trim(Request.QueryString("fromcitycode"))
                    Dim tocity = Trim(Request.QueryString("tocitycode"))
                    Dim fromctry = Trim(Request.QueryString("fromctrycode"))
                    Dim toctry = Trim(Request.QueryString("toctrycode"))
                    Dim agingtype = Trim(Request.QueryString("agingtype"))
                    Dim pdcyesno = Trim(Request.QueryString("pdcyesno"))
                    Dim includezero = Trim(Request.QueryString("includezero"))
                    Dim remarks = Trim(Request.QueryString("remarks"))
                    Dim rpttype = Trim(Request.QueryString("rpttype"))
                    Dim groupby = Trim(Request.QueryString("groupby"))
                    Dim sumdet = Trim(Request.QueryString("sumdet"))
                    Dim reporttype = Trim(Request.QueryString("reporttype"))
                    Dim repfilter = Trim(Request.QueryString("repfilter"))
                    Dim reporttitle = Trim(Request.QueryString("reporttitle"))
                    Dim orderby = Trim(Request.QueryString("orderby"))
                    Dim custtype = IIf(Type = "C", Trim(Request.QueryString("custtype")), Trim(Request.QueryString("supptype")))
                    Dim rptgrp = Trim(Request.QueryString("rptgroup"))
                    Dim rptodr = Trim(Request.QueryString("rptOrder"))
                    Dim frommarket = Trim(Request.QueryString("frommarketcode"))
                    Dim tomarket = Trim(Request.QueryString("tomarketcode"))
                    Dim agdate = Trim(Request.QueryString("agdate"))
                    Dim inclproforma = Trim(Request.QueryString("strincludeproforma"))
                    'End If
                    Dim fromname = Trim(Request.QueryString("fromname"))
                    Dim frommkname = Trim(Request.QueryString("frommkname"))
                    Dim ctryname = Trim(Request.QueryString("fromctryname"))
                    Dim catname = Trim(Request.QueryString("fromcatname"))
                    Dim glname = Trim(Request.QueryString("glname"))
                    Dim currcodefilter = Trim(Request.QueryString("currcode"))
                    Dim reportfilter As String = String.Empty
                    Dim rptfromname As String = String.Empty
                    If reporttype = "SupplierAgeingSummary" Then
                        If Type = "C" Then
                            reportname = "CUSTOMER AGEING SUMMARY"
                            rptfromname = "Customer Name From "
                        ElseIf Type = "S" Then
                            reportname = "SUPPLIER AGEING SUMMARY"
                            rptfromname = "Supplier Name From "
                        Else
                            reportname = "SUPPLIER AGENT AGEING SUMMARY"
                            rptfromname = "Supplier Agent Name From "
                        End If
                    Else
                        If Type = "C" Then
                            reportname = "CUSTOMER AGEING DETAIL"
                            rptfromname = "Customer Name From "
                        ElseIf Type = "S" Then
                            reportname = "SUPPLIER AGEING DETAIL"
                            rptfromname = "Supplier Name From "
                        Else
                            reportname = "Supplier Agent Ageing Detail"
                            rptfromname = "Supplier Agent Name From "
                        End If
                    End If


                    'If Not (String.IsNullOrEmpty(fromdate)) And Not (String.IsNullOrEmpty(todate)) Then
                    '    reportfilter = "From" & Space(2) & Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy") & Space(2) & "To" & Space(2) & Convert.ToDateTime(todate).ToString("dd/MM/yyyy")
                    'End If
                    If Not (String.IsNullOrEmpty(fromname)) Then
                        ' reportfilter = reportfilter & Space(2) & rptfromname & Space(2) & fromname & Space(2) & "To" & Space(2) & fromname
                        reportfilter = reportfilter & Space(2) & rptfromname & ":" & Space(2) & fromname & Space(2)
                    End If

                    If Not (String.IsNullOrEmpty(frommkname)) Then

                        reportfilter = reportfilter & Space(2) & IIf(Type = "C", ":Market From", "Supplier City From") & Space(2) & frommkname & Space(2) & "To" & Space(2) & frommkname
                    End If

                    If Not (String.IsNullOrEmpty(ctryname)) Then
                        ' reportfilter = reportfilter & Space(2) & "Supplier Country From" & Space(2) & ctryname & Space(2) & "To" & Space(2) & ctryname
                        reportfilter = reportfilter & Space(2) & "Supplier Country From :" & Space(2) & ctryname & Space(2)
                    End If
                    If Not (String.IsNullOrEmpty(catname)) Then
                        'reportfilter = reportfilter & Space(2) & "Category  From" & Space(2) & catname & Space(2) & "To" & Space(2) & catname
                        reportfilter = reportfilter & Space(2) & "Category  From :" & Space(2) & catname & Space(2)
                    End If
                    If Not (String.IsNullOrEmpty(glname)) Then
                        ' reportfilter = reportfilter & Space(2) & "Control Account Name From" & Space(2) & glname & Space(2) & "To" & Space(2) & glname
                        reportfilter = reportfilter & Space(2) & "Control Account Name From :" & Space(2) & glname & Space(2)
                    End If

                    Dim web = 0
                    Dim summdet = 0
                    Dim ds As New DataSet
                    bytes = {}

                    If sumdet = 0 Then
                        bc.GenerateReport(todate, Type, reportfilter, currflg, reportname, reportsType, fromacct, toacct, fromcontrol, tocontrol, fromcat, tocat, fromcity, tocity, fromctry, toctry, agingtype, summdet, web, custtype, divcode, custgroup_sp_type, inclproforma, currcodefilter, bytes, ds, "download", orderby, groupby)
                        fileName = reportname + "@" + DateTime.Now.ToString("ddMMyyyy@HHmmss") + ".pdf"
                    Else
                        obj.GenerateReport(todate, Type, reportfilter, currflg, reportname, reportsType, fromacct, toacct, fromcontrol, tocontrol, fromcat, tocat, fromcity, tocity, fromctry, toctry, agingtype, sumdet, web, custtype, divcode, custgroup_sp_type, inclproforma, bytes, ds, "download", orderby, groupby)
                        fileName = reportname + "@" + DateTime.Now.ToString("ddMMyyyy@HHmmss") + ".pdf"
                    End If
                ElseIf printId = "ReceiptVoucher" Then
                    Dim report As New clsBankReceiptVoucherPdf

                    Dim objCashVoucher As New ClsCashVoucherPdf
                    Dim trantype = Request.QueryString("TranType")
                    Dim tranid = Request.QueryString("Tranid")
                    Dim divcode = Trim(Request.QueryString("divid"))
                    Dim CashBankType = Trim(Request.QueryString("CashBankType"))
                    Dim PrntSec = Trim(Request.QueryString("PrntSec"))
                    Dim PrntCliCurr = Trim(Request.QueryString("PrntCliCurr"))
                    Dim PrinDocTitle = Trim(Request.QueryString("PrinDocTitle"))
                    bytes = {}
                    If trantype = "CRV" Or trantype = "CV" Then
                        If trantype = "CRV" Then
                            reportname = "Cash Receipt Voucher"
                            report.GenerateReport(trantype, tranid, divcode, CashBankType, PrntSec, PrntCliCurr, PrinDocTitle, bytes, "download")
                        ElseIf trantype = "CV" Then
                            reportname = "Contra Voucher"
                            report.GenerateReportCV(trantype, tranid, divcode, CashBankType, PrntSec, PrntCliCurr, PrinDocTitle, bytes, "download")
                        End If
                    ElseIf trantype = "CPV" Or trantype = "BPV" Then
                        If trantype = "CPV" Then
                            reportname = "Cash Payment Voucher"
                        Else
                            reportname = "Bank Payment Voucher"
                        End If
                        objCashVoucher.GenerateReport(trantype, tranid, divcode, CashBankType, PrntSec, PrinDocTitle, bytes, "download")

                    ElseIf trantype = "RV" Then

                        reportname = "Receipt Voucher"
                        report.GenerateReportRV(trantype, tranid, divcode, CashBankType, PrntSec, PrntCliCurr, PrinDocTitle, bytes, "download")
                    End If
                ElseIf printId = "JournalDoc" Then
                    Dim objjournalVoucher As New ClsJournalVoucherPdf
                    Dim trantype = Request.QueryString("TranType")
                    Dim tranid = Request.QueryString("Tranid")
                    Dim divcode = Trim(Request.QueryString("divid"))
                    Dim PrntSec = Trim(Request.QueryString("PrntSec"))
                    bytes = {}
                    reportname = "Journal Voucher"
                    objjournalVoucher.GenerateReport(trantype, tranid, divcode, PrntSec, bytes, "download")
                ElseIf printId = "GlTrialBal" Then
                    Dim objclsGLTrialBalPdf As New clsGLTrialBalPdf
                    Dim fromdate = Request.QueryString("fromdate")
                    Dim todate = Request.QueryString("todate")
                    Dim divcode = Trim(Request.QueryString("divid"))
                    Dim withmov = Trim(Request.QueryString("withmov"))
                    Dim closing = Request.QueryString("closing")
                    Dim level = Request.QueryString("level")
                    Dim acccodefrom = Trim(Request.QueryString("acccodefrom"))
                    Dim fromname = Trim(Request.QueryString("fromname"))
                    Dim acccodeto = Trim(Request.QueryString("acccodeto"))
                    Dim rptype = Trim(Request.QueryString("rptype"))
                    Dim rptvalue = Trim(Request.QueryString("rptval"))
                    bytes = {}
                    Dim rptname1 As String
                    If withmov = "0" Then
                        reportname = "GL Trial Balanace - Transactions"
                        rptname1 = "Transactions"
                    Else
                        reportname = "GL Trial Balanace - Balances"
                        rptname1 = "Balances"
                    End If

                    objclsGLTrialBalPdf.GenerateReport(fromdate, todate, fromname, divcode, reportname, withmov, closing, level, acccodefrom, acccodeto, rptype, rptvalue, reportsType, bytes, "download")
                    reportname = printId & rptname1
                    'Tanvir 20062022
                ElseIf printId = "VATAccrued" Then
                    Dim objclsGLTrialBalPdf As New clsGLTrialBalPdf
                    Dim fromdate = Request.QueryString("fromdate")
                    Dim divcode = Trim(Request.QueryString("divid"))
                    bytes = {}
                    Dim rptname1 As String
                    reportname = "VAT ACCRUED BREAKUP"
                    objclsGLTrialBalPdf.GenerateVatAccReport(fromdate, reportname, divcode, "excel", bytes, "download")
                    'Tanvir 20062022
                ElseIf printId = "CashBankBook" Then
                    Dim objclsCashBankBookPdf As New ClsCashBankBookPdf
                    Dim fromdate = Request.QueryString("fromdate")
                    Dim todate = Request.QueryString("todate")
                    Dim divcode = Trim(Request.QueryString("divid"))
                    Dim frmcode = Trim(Request.QueryString("frmcode"))
                    Dim frmbankname = Trim(Request.QueryString("frmname"))
                    Dim pagetype = Trim(Request.QueryString("pagetype"))
                    Dim type = Request.QueryString("type")
                    Dim currflag = Request.QueryString("currflg")
                    Dim inclpagebrk = Request.QueryString("inclpagebr")
                    Dim cashbanktype = Request.QueryString("cashbanktype")

                    bytes = {}

                    reportname = "CashBankBook"
                    objclsCashBankBookPdf.GenerateReport(reportsType, fromdate, todate, divcode, pagetype, type, currflag, frmcode, frmbankname, inclpagebrk, cashbanktype, bytes, "download")
                ElseIf printId = "ProfitLoss" Then
                    Dim objClsProfitLossPdf As New ClsProfitLossPdf
                    Dim fromdate = Request.QueryString("frmdate")
                    Dim todate = Request.QueryString("todate")
                    Dim divcode = Trim(Request.QueryString("divid"))
                    Dim type = Request.QueryString("type")

                    Dim strrpttype1 = Trim(Request.QueryString("strrpttype1"))
                    Dim rpttype = Trim(Request.QueryString("rptype"))
                    Dim RptRatio = Trim(Request.QueryString("chkRatio"))
                    Dim closing = Request.QueryString("closing")
                    bytes = {}
                    'If withmov = "0" Then
                    reportname = printId
                    'Else
                    '    reportname = "GL Trial Balanace - Balances"
                    'End If
                    If rpttype = "1" And RptRatio = "1" Then
                        reportname = "ProfitLossRatio"
                        Dim objClsprofitlossRatio As clsProfitLossRatio = New clsProfitLossRatio()
                        objClsprofitlossRatio.GenerateReportMonthRatiowise(reportsType, fromdate, todate, divcode, rpttype, type, closing, RptRatio, strrpttype1, bytes, "download")
                    ElseIf rpttype = "1" Then
                        objClsProfitLossPdf.GenerateReportMonthwise(reportsType, fromdate, todate, divcode, rpttype, type, closing, strrpttype1, bytes, "download")
                    Else
                        objClsProfitLossPdf.GenerateReport(reportsType, fromdate, todate, divcode, rpttype, type, closing, strrpttype1, bytes, "download")
                    End If
                ElseIf printId = "MatchOutStanding" Then
                    Dim clsmatchoutstand As New clsMatchOutStandPdf
                    Dim trantype = Request.QueryString("TranType")
                    Dim tranid = Request.QueryString("Tranid")
                    'Dim divcode = Trim(Request.QueryString("divid"))
                    Dim divcode = Session("div_code")
                    Dim curr = Trim(Request.QueryString("Curr"))
                    bytes = {}
                    reportname = "Match OutStanding"
                    clsmatchoutstand.GenerateReport(trantype, tranid, divcode, curr, bytes, "download")

                ElseIf printId = "salesinvoivefreeform" Then

                    Dim objclsInvoiceFree As New ClsInvoiceFreeFormPdf

                    Dim trantype = Request.QueryString("TranType")
                    Dim tranid = Request.QueryString("Tranid")
                    Dim divcode = Trim(Request.QueryString("divid"))
                    Dim PrntSec = Trim(Request.QueryString("PrntSec"))
                    Dim TRNNo = Trim(Request.QueryString("TRNNo"))
                    bytes = {}
                    If trantype = "IN" Then
                        reportname = "SaleInvoiceFreeForm"

                    ElseIf trantype = "PI" Then
                        reportname = "PurchaseInvoiceFreeForm"
                    ElseIf trantype = "PIManual" Then
                        reportname = "PurchaseInvoiceFreeFormManual"
                    ElseIf trantype = "PE" Then
                        reportname = "PurchaseExpenses"
                    End If

                    objclsInvoiceFree.GenerateReport(trantype, tranid, divcode, PrntSec, TRNNo, bytes, "download")

                ElseIf printId = "OpeningTrailBalance" Then

                    Dim obj As New clsOpeningTrialBalPdf

                    Dim trantype = Request.QueryString("TranType")
                    Dim tranid = Request.QueryString("TranID")
                    Dim divcode = Trim(Request.QueryString("divid"))
                    Dim type = Trim(Request.QueryString("Type"))
                    bytes = {}
                    reportname = "Report - Opening Trial Balance"
                    obj.GenerateReport(reportsType, trantype, divcode, tranid, type, reportname, bytes, "download")

                ElseIf printId = "Bank Type" Or printId = "OtherBankMaster" Then
                    Dim objClsBankTypePdf As New ClsBankTypePdf
                    bytes = {}
                    reportname = printId
                    objClsBankTypePdf.GenerateReport(reportsType, printId, bytes, "download")
                ElseIf printId = "BankDetails" Then
                    Dim objClsBankDetailsPdf As New ClsBankDetailsPdf
                    Dim divcode = Trim(Request.QueryString("divid"))
                    bytes = {}
                    reportname = printId
                    objClsBankDetailsPdf.GenerateReport(reportsType, divcode, bytes, "download")
                ElseIf printId = "BalanceSheet" Then

                    Dim obj As New BalanceSheetPdf

                    Dim pagetype = Request.QueryString("pagetype")
                    Dim level = Request.QueryString("level")
                    Dim divcode = Trim(Request.QueryString("divid"))
                    Dim newformat = Trim(Request.QueryString("newformat"))
                    Dim todate = Trim(Request.QueryString("todate"))
                    bytes = {}
                    reportname = "Balance Sheet"
                    Dim reportfilter As String = "As On Date " + Format(CDate(todate), "dd/MM/yyyy")
                    obj.GenerateReport(todate, divcode, reportsType, pagetype, level, reportname, reportfilter, newformat, bytes, "download")
                    'Ram 22/08/2022
                ElseIf printId = "BalanceSheetNew" Then

                    Dim obj As New BalanceSheetPdf

                    Dim pagetype = Request.QueryString("pagetype")
                    Dim level = Request.QueryString("level")
                    Dim divcode = Trim(Request.QueryString("divid"))
                    Dim newformat = Trim(Request.QueryString("newformat"))
                    Dim todate = Trim(Request.QueryString("todate"))
                    bytes = {}
                    reportname = "Balance Sheet"
                    Dim reportfilter As String = "As On Date " + Format(CDate(todate), "dd/MM/yyyy")
                    obj.GenerateReportNew(todate, divcode, reportsType, pagetype, level, reportname, reportfilter, newformat, bytes, "download")
                ElseIf printId = "ProfitLossNew" Then

                    Dim obj As New ClsProfitLossPdf

                    Dim pagetype = Request.QueryString("pagetype")
                    Dim level = Request.QueryString("level")
                    Dim divcode = Trim(Request.QueryString("divid"))
                    Dim newformat = Trim(Request.QueryString("newformat"))
                    Dim todate = Trim(Request.QueryString("todate"))
                    bytes = {}
                    reportname = "PROFIT & LOSS STATEMENT"
                    Dim reportfilter As String = "YEAR ENDED " + Format(CDate(todate), "dd MMM yyyy")
                    obj.GenerateReportPLNew(todate, divcode, reportsType, pagetype, level, reportname, reportfilter, newformat, bytes, "download")
                    'Ram 22/08/2022
                ElseIf printId = "AirportMaPriceList" Then
                    Dim objclsAirportMaExl As New clsAirportMaExl
                    ' Dim divcode = Trim(Request.QueryString("divid"))
                    bytes = {}
                    reportname = printId
                    Dim fromdate = Request.QueryString("fromdate")
                    Dim todate = Request.QueryString("todate")
                    Dim decno = Request.QueryString("decno")
                    Dim curr = Request.QueryString("curr")
                    Dim remark = Request.QueryString("remark")
                    Dim ctrygrpcode = Request.QueryString("ctrygrpcode")
                    Dim srcctrycode = Request.QueryString("srcctrycode")
                    Dim airport = Request.QueryString("airport")
                    Dim servicetype = Request.QueryString("servicetype")
                    Dim RateType = Request.QueryString("RateType")
                    Dim PartyCode = Request.QueryString("PartyCode")
                    objclsAirportMaExl.ExcelReport(bytes, decno, fromdate, todate, curr, remark, ctrygrpcode, airport, srcctrycode, servicetype, RateType, PartyCode)


                ElseIf printId = "Currency Conversion Rates" Then

                    Dim objClsCurrencyRatePdf As New ClsCurrencyRatePdf
                    Dim fromcurr = Trim(Request.QueryString("FromCurr"))
                    Dim tocurr = Trim(Request.QueryString("ToCurr"))
                    fromcurr = IIf(fromcurr = "[Select]", "", fromcurr)
                    tocurr = IIf(tocurr = "[Select]", "", tocurr)
                    bytes = {}
                    reportname = printId
                    objClsCurrencyRatePdf.GenerateReport(fromcurr, tocurr, bytes, "download")
                ElseIf printId = "CustomerGroup" Then
                    Dim objClsCustomerGroupPdf As New ClsCustomerGroupPdf
                    ' Dim divcode = Trim(Request.QueryString("divid"))
                    bytes = {}
                    reportname = printId
                    objClsCustomerGroupPdf.GenerateReport(bytes, "download")

                ElseIf printId = "emaillog" Then
                    Dim fs As FileStream
                    Dim downnfilepath = Trim(Request.QueryString("filepath").Replace("*", "\"))
                    Dim downlfilename = Trim(Request.QueryString("filename"))

                    Dim file As System.IO.FileInfo = New System.IO.FileInfo(downnfilepath)
                    fs = New FileStream(downnfilepath, FileMode.Open, FileAccess.Read)
                    If file.Exists Then

                        Dim br As BinaryReader = New BinaryReader(fs)
                        Dim bytes As Byte() = br.ReadBytes(Convert.ToInt32(fs.Length))
                        br.Close()
                        fs.Close()

                        'Write the file to Reponse
                        Response.Buffer = True
                        Response.Charset = ""
                        Response.Cache.SetCacheability(HttpCacheability.NoCache)
                        Response.ContentType = ContentType
                        Response.AddHeader("content-disposition", "attachment;filename=" & downlfilename)
                        Response.BinaryWrite(bytes)
                        Response.Flush()
                        Response.End()

                    Else
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('File not Found');", True)
                    End If
                    Exit Sub
                ElseIf printId = "suppConsolidateInvoice" Then
                    Dim objcls As clsSuppConsolidateInvoice = New clsSuppConsolidateInvoice()
                    Dim acctType As String = Trim(Request.QueryString("acctType"))
                    Dim acctCode As String = Trim(Request.QueryString("acctCode"))
                    Dim TranType As String = Trim(Request.QueryString("TranType"))
                    Dim divid As String = Trim(Request.QueryString("divid"))
                    Dim fromDate As String = Trim(Request.QueryString("fromDate"))
                    Dim toDate As String = Trim(Request.QueryString("toDate"))
                    bytes = {}
                    objcls.GenerateReport(acctType, acctCode, TranType, divid, fromDate, toDate, bytes, "download")
                    reportsType = "PDF"
                    reportname = printId
                ElseIf printId = "CashBankBalance" Then
                    Dim objclsCashBankBalancePdf As New ClsCashBankBalancePdf
                    Dim fromdate = Request.QueryString("fromdate")
                    Dim divcode = Trim(Request.QueryString("divid"))
                    reportsType = Request.QueryString("reportsType")
                    bytes = {}
                    reportname = printId
                    objclsCashBankBalancePdf.GenerateReport(reportsType, fromdate, divcode, bytes, "download")
                ElseIf printId = "SerialityControl" Then
                    Dim objclsSerialityControl As New clsSerialityControl
                    Dim fromval = Request.QueryString("fromVal")
                    Dim toval = Request.QueryString("toVal")
                    Dim divcode = Trim(Request.QueryString("divid"))
                    reportsType = Request.QueryString("reportsType")
                    bytes = {}
                    reportname = printId
                    objclsSerialityControl.GenerateReport(reportsType, fromval, toval, divcode, bytes, "download")
                ElseIf printId = "HotelGroups" Then
                    Dim rptcompanyname As String = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
                    Dim type As String = Request.QueryString("suptype")
                    Dim heading() As String
                    If type = "OTH" Or type = "TRFS" Or type = "VISA" Then
                        reportname = "Report - " + type + " Supplier Details"
                        heading = {"Supplier Code", "Supplier Name", "Address", "TRN No"}
                    ElseIf type = "HOT" Then
                        reportname = "Report - Hotel Group"
                        heading = {"Hotel Group Code", "Hotel Group Name", "Active", "TRN No"}
                    ElseIf type = "EXC" Then
                        reportname = "Report - Excursion Supplier Group"
                        heading = {"Supplier Code", "Supplier Group", "Active", "TRN No"}
                    End If
                    ExportToExcelParty(heading, type, reportname, rptcompanyname, bytes)
                End If



                    If reportsType = "excel" Then
                        Response.AddHeader("Content-Disposition", "inline; filename=" + reportname + "@" + DateTime.Now.ToString("ddMMyyyy@HHmmss") + ".xlsx")
                        Response.AddHeader("Content-Length", bytes.Length.ToString())
                        Response.ContentType = "application/xls"
                    Else
                        Response.AddHeader("Content-Disposition", "inline; filename=" + reportname + "@" + DateTime.Now.ToString("ddMMyyyy@HHmmss") + ".pdf")
                        Response.AddHeader("Content-Length", bytes.Length.ToString())
                        Response.ContentType = "application/pdf"

                    End If

                    Response.Buffer = True
                    Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    Response.BinaryWrite(bytes)
                    Response.Flush()
                    HttpContext.Current.ApplicationInstance.CompleteRequest()



            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("TransactionReports.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            Response.Redirect("Login.aspx", True)
        End If
    End Sub

    Public Sub ExcelReport(ByRef bytes() As Byte, ByVal decno As String, ByVal fromdate As Date, ByVal todate As Date, ByVal curr As String, ByVal remark As String,
                        ByVal ctrygrp As String, ByVal airport As String, ByVal srcctry As String, ByVal servicetype As String, ByVal RateType As String,
                        ByVal Partycode As String, ByVal agentcode As String)
        Try


            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet

            Dim wb As New XLWorkbook
            Dim ws = wb.Worksheets.Add("AirportMAPriceListReport")
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            '  mySqlCmd = New SqlCommand("sp_rep_airportmapricelist", sqlConn)
            mySqlCmd = New SqlCommand("sp_rep_airportmapricelist", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = Format((fromdate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = Format((todate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@countrygroup", SqlDbType.VarChar, 10)).Value = ctrygrp
            mySqlCmd.Parameters.Add(New SqlParameter("@sourcectrycode", SqlDbType.VarChar, 10)).Value = srcctry
            mySqlCmd.Parameters.Add(New SqlParameter("@airportbordercode", SqlDbType.VarChar)).Value = airport
            mySqlCmd.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar)).Value = servicetype.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar)).Value = Partycode.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@Ratetype", SqlDbType.VarChar)).Value = RateType.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar)).Value = agentcode
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim Airportmadt, dtcurrremarks As New DataTable
            Airportmadt = ds.Tables(0)
            Dim cc = Airportmadt.Columns.Count - 9
            Dim arrHeaders(cc) As String

            Dim decimalPoint1 = "", decimalpoint As String = ""
            If decno = 2 Then
                decimalpoint = "#,##0.00"
                decimalPoint1 = "(#,##0.00)"
            ElseIf decno = 3 Then
                decimalpoint = "#,##0.000"
                decimalPoint1 = "(#,##0.000)"
            ElseIf decno = 4 Then
                decimalpoint = "#,##0.0000"
                decimalPoint1 = "(#,##0.0000)"
            End If


            ws.Columns.AdjustToContents()
            Dim colcount = Airportmadt.Columns.Count
            ws.Column("A").Width = 50
            ws.Columns(2, cc).Width = 20
            Dim rownum As Integer = 6

            'Comapny Name Heading
            ws.Cell("A1").Value = CType(Session("CompanyName"), String)
            Dim company = ws.Range(1, 1, 1, cc).Merge()
            company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
            company.Style.Font.SetBold().Font.FontSize = 14
            company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            Dim fromtodate As String = IIf(String.Compare((fromdate).ToString("yyyy"), (todate).ToString("yyyy")) = 0, (fromdate).ToString("yyyy"), (fromdate).ToString("yyyy") & "/" & (todate).ToString("yy"))

            ws.Cell("A2").Value = "Tariff " & fromtodate
            Dim report = ws.Range(2, 1, 2, cc).Merge()
            report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
            report.Style.Font.SetBold().Font.FontSize = 14
            report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            ws.Cell("A3").Value = "EXTRA SERVICES"
            Dim report1 = ws.Range(3, 1, 3, cc).Merge()
            report1.Style.Fill.BackgroundColor = XLColor.Navy
            report1.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
            report1.Style.Font.FontColor = XLColor.White
            report1.Style.Font.SetBold().Font.FontSize = 13
            report1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            report1.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            If RateType.Trim.ToLower = "salevalue" Then
                ws.Cell("A4").Value = "MEET AND ASSIST SERVICES SALE PRICELIST"
            ElseIf RateType.Trim.ToLower = "costvalue" Then
                ws.Cell("A4").Value = "MEET AND ASSIST SERVICES COST PRICELIST"
            End If
            Dim report2 = ws.Range(4, 1, 4, cc).Merge()
            report2.Style.Font.FontColor = XLColor.Red
            report2.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
            report2.Style.Font.SetBold().Font.FontSize = 13
            report2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            report2.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            Dim filter = ws.Range(5, 1, 5, cc).Merge()
            filter.Style.Font.SetBold().Font.FontSize = 12
            filter.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            filter.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            filter.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontColor = XLColor.Black
            filter.Cell(1, 1).Value = "Rates valid from " & fromdate.ToString("dd") & Space(1) & fromdate.ToString("MMM") & Space(1) & fromdate.ToString("yyyy") & " - " & todate.ToString("dd") & Space(1) & todate.ToString("MMM") & Space(1) & todate.ToString("yyyy")


            Dim grpbyairport = From grpbyair In Airportmadt.AsEnumerable() Group grpbyair By g = New With {Key .airportname = grpbyair.Field(Of String)("airportbordername")} Into Group Order By g.airportname




            For Each AirportData In grpbyairport

                Dim Tbldata1 As DataTable = AirportData.Group.CopyToDataTable
                Dim Grpbyservicetype = From grpbyser In Tbldata1.AsEnumerable() Group grpbyser By g = New With {Key .servicetype = grpbyser.Field(Of String)("servicetype"), Key .opcode = grpbyser.Field(Of String)("oplistcode")} Into Group Order By g.servicetype
                Dim c As Integer = 0
                For Each grpserviceData In Grpbyservicetype

                    ''ws.Cell(rownum, 1).Value = CType(dtHead.Rows(rows)("frmdatec"), Date).ToString("dd-MMM-yyyy") + " To " + CType(dtHead.Rows(rows)("todatec"), Date).ToString("dd-MMM-yyyy")
                    'ws.Cell(rownum, 1).Value = "From : " & Convert.ToString(AirportData.g.airportname)
                    'Dim DateTitle11 = ws.Range(rownum, 1, rownum, arrHeaders.Length - 1).Merge()
                    'DateTitle11.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentHorizontalValues.Center).Alignment.WrapText = True
                    'DateTitle11.Style.Font.SetBold().Font.SetFontColor(XLColor.White).Font.FontSize = 13
                    'DateTitle11.Style.Fill.BackgroundColor = XLColor.FromArgb(41, 45, 134)
                    'DateTitle11.Style.Alignment.WrapText = True

                    'rownum = rownum + 1

                    Dim arrCount As Integer = 0
                    c += 1
                    If c = 2 Then
                        rownum += 1
                        ws.Range(rownum, 1, rownum, arrHeaders.Length - 1).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                        ws.Cell(rownum, 1).Value = ""
                    End If
                    Dim dtHead As DataTable



                    dtHead = objUtils.GetDataFromDataTable("EXEC SP_SelectAirportMADates '" + Format((fromdate), "yyyy/MM/dd") + "','" + Format((todate), "yyyy/MM/dd") + "','" + grpserviceData.g.opcode + "','" + RateType.Trim + "'")

                    For rows = 0 To dtHead.Rows.Count - 1
                        If rows = 0 Then
                            ws.Cell(rownum, 1).Value = CType(dtHead.Rows(rows)("applicableto") + " IN " + dtHead.Rows(rows)("currcode"), String)
                            'phrase.Add(New Chunk(CType(dtHead.Rows(rows)("frmdatecToPrint"), Date).ToString("dd-MMM-yyyy") + " To " + CType(dtHead.Rows(rows)("todatecToPrint"), Date).ToString("dd-MMM-yyyy"), ReportNamefont))
                            Dim DateTitle22 = ws.Range(rownum, 1, rownum, arrHeaders.Length - 1).Merge()
                            DateTitle22.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentHorizontalValues.Center).Alignment.WrapText = True
                            DateTitle22.Style.Font.SetBold().Font.SetFontColor(XLColor.White).Font.FontSize = 13
                            DateTitle22.Style.Fill.BackgroundColor = XLColor.FromArgb(41, 45, 134)
                            DateTitle22.Style.Alignment.WrapText = True

                            rownum = rownum + 1
                        End If
                        'ws.Cell(rownum, 1).Value = CType(dtHead.Rows(rows)("frmdatec"), Date).ToString("dd-MMM-yyyy") + " To " + CType(dtHead.Rows(rows)("todatec"), Date).ToString("dd-MMM-yyyy")
                        ws.Cell(rownum, 1).Value = CType(dtHead.Rows(rows)("frmdatecToPrint"), Date).ToString("dd-MMM-yyyy") + " To " + CType(dtHead.Rows(rows)("todatecToPrint"), Date).ToString("dd-MMM-yyyy")
                        'phrase.Add(New Chunk(CType(dtHead.Rows(rows)("frmdatecToPrint"), Date).ToString("dd-MMM-yyyy") + " To " + CType(dtHead.Rows(rows)("todatecToPrint"), Date).ToString("dd-MMM-yyyy"), ReportNamefont))
                        Dim DateTitle = ws.Range(rownum, 1, rownum, arrHeaders.Length - 1).Merge()
                        DateTitle.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentHorizontalValues.Center).Alignment.WrapText = True
                        DateTitle.Style.Font.SetBold().Font.SetFontColor(XLColor.White).Font.FontSize = 13
                        DateTitle.Style.Fill.BackgroundColor = XLColor.FromArgb(41, 45, 134)
                        DateTitle.Style.Alignment.WrapText = True

                        rownum = rownum + 1
                    Next

                    arrHeaders(0) = grpserviceData.g.servicetype & " Service in " & AirportData.g.airportname
                    For i = 10 To colcount - 1
                        arrCount = arrCount + 1
                        arrHeaders(arrCount) = Airportmadt.Columns(i).ColumnName
                    Next
                    rownum += 1
                    ws.Range(rownum, 1, rownum, arrCount + 1).Style.Font.SetBold().Font.FontSize = 9
                    ws.Range(rownum, 1, rownum, arrCount + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 179, 102)
                    ws.Range(rownum, 1, rownum, arrCount + 1).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.WrapText = True
                    ws.Range(rownum, 1, rownum, arrCount + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)

                    For i = 0 To arrHeaders.Length - 2
                        ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                    Next
                    Dim arrTotal(cc - 1) As Decimal
                    Dim count As Integer = -1
                    Dim remarks(grpserviceData.Group.Count) As String
                    Dim adultsalevalue, childsalevalue As Decimal
                    Dim total() As String
                    arrCount = 0
                    For Each row In grpserviceData.Group

                        arrHeaders(0) = row("servicetypename").ToString()
                        arrCount = 0
                        For i = 10 To colcount - 1
                            arrCount = arrCount + 1
                            If Not IsDBNull(row(i)) Then
                                arrHeaders(arrCount) = row(i)
                                arrTotal(arrCount - 1) = arrTotal(arrCount - 1) + row(i)
                            Else
                                arrHeaders(arrCount) = ""
                            End If
                        Next

                        rownum += 1
                        ws.Range(rownum, 1, rownum, arrCount + 1).Style.Font.FontSize = 9
                        ws.Range(rownum, 1, rownum, arrCount + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 217, 179)
                        ws.Range(rownum, 1, rownum, arrCount + 1).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.WrapText = True
                        ws.Range(rownum, 2, rownum, arrCount + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        ws.Range(rownum, 1, rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)


                        ws.Cell(rownum, 1).Value = arrHeaders(0)
                        For i = 1 To arrHeaders.Length - 2
                            If arrHeaders(i) <> "" Then
                                ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                                'ws.Cell(rownum, i + 1).Style.NumberFormat.Format = decimalpoint
                            Else
                                ws.Cell(rownum, i + 1).Value = ""
                            End If
                            ws.Cell(rownum, i + 1).Style.NumberFormat.Format = decimalpoint
                        Next
                        If row("remarks").ToString.Trim.Length > 0 Then
                            rownum += 1
                            ws.Range(rownum, 1, rownum, arrCount + 1).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.WrapText = True
                            ws.Range(rownum, 2, rownum, arrCount + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                            ws.Range(rownum, 1, rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            ws.Range(rownum, 1, rownum, arrCount + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 255)
                            ws.Range(rownum, 1, rownum, arrCount + 1).Merge()
                            ws.Range(rownum, 1, rownum, arrCount + 1).Value = row("remarks")
                        End If
                    Next
                    '   total = {"Total", "", IIf(childsalevalue = 0.0, "", childsalevalue), "", IIf(adultsalevalue = 0.0, "", adultsalevalue)}

                    rownum += 1
                    'ws.Range(rownum, 1, rownum, arrTotal.Length).Style.Font.SetBold().Font.FontSize = 10
                    'ws.Range(rownum, 1, rownum, arrTotal.Length).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 217, 179)
                    'ws.Range(rownum, 1, rownum, arrTotal.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.WrapText = True
                    'ws.Range(rownum, 2, rownum, arrTotal.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    'ws.Range(rownum, 1, rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                    'ws.Cell(rownum, 1).Value = "Total"

                    'For i = 0 To arrTotal.Length - 2
                    '    'If arrTotal(i) <> 0.0 Then
                    '    ws.Cell(rownum, i + 2).Value = arrTotal(i)

                    '    ws.Cell(rownum, i + 2).Style.NumberFormat.Format = decimalpoint
                    '    'Else
                    '    '    ws.Cell(rownum, i + 1).Value = arrTotal(i)
                    '    'End If
                    'Next
                    'adultsalevalue = 0
                    'childsalevalue = 0
                    If remark <> Nothing AndAlso remark <> "" Then
                        count = count + 1
                        remarks(count) = "* " & remark
                    End If
                    If remarks.Length > 1 Then
                        ws.Range(rownum, 1, rownum + count + 1, arrCount + 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                        For i = 0 To count
                            rownum += 1
                            Dim d1 As Integer = remarks(i).Length / ((arrCount + 1) * 11)
                            If d1 = 1 Then
                                ws.Row(rownum).Height = 6
                            ElseIf d1 = 2 Then
                                ws.Row(rownum).Height = 12
                            ElseIf d1 = 3 Then
                                ws.Row(rownum).Height = 18
                            ElseIf d1 = 4 Then
                                ws.Row(rownum).Height = 24
                            ElseIf d1 = 5 Then
                                ws.Row(rownum).Height = 30
                            End If
                            ws.Range(rownum, 1, rownum, arrCount + 1).Merge()
                            ws.Range(rownum, 1, rownum, arrCount + 1).Style.Font.FontSize = 9
                            ws.Cell(rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            ws.Cell(rownum, 1).Value = remarks(i)
                        Next
                    End If
                Next
                rownum += 1
                ws.Range(rownum, 1, rownum, arrHeaders.Length).Merge()
                ws.Cell(rownum, 1).Value = ""
                rownum += 1
                ws.Range(rownum, 1, rownum, arrHeaders.Length).Merge()
                ws.Cell(rownum, 1).Value = ""
            Next



            ws.Cell((rownum + 2), 1).Value = "Printed Date : " & Now.ToString("dd/MM/yyyy")
            ws.Range((rownum + 2), 1, (rownum + 2), 4).Merge()
            Using wStream As New MemoryStream()
                wb.SaveAs(wStream)
                bytes = wStream.ToArray()
            End Using
        Catch ex As Exception

        End Try
    End Sub

    Public Sub ExportToExcel(ByVal heading() As String, ByVal type As String, ByVal reportname As String, ByVal rptcompanyname As String, ByRef bytes() As Byte)
        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim sptypecode As String
            Dim SuppAddress As String
            If type = "OTH" Then
                sptypecode = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1501'"), String)
                strSqlQry = "SELECT partymast.partycode, partymast.partyname,partymast.add1,partymast.add2,partymast.add3,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1 FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode   where partymast.sptypecode= '" + sptypecode + "'  ORDER BY partymast.partyname ASC"
            ElseIf type = "TRFS" Then
                sptypecode = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='564'"), String)
                strSqlQry = "SELECT partymast.partycode, partymast.partyname,partymast.add1,partymast.add2,partymast.add3,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1 FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode   where partymast.sptypecode= '" + sptypecode + "'  ORDER BY partymast.partyname ASC"
            ElseIf type = "VISA" Then
                sptypecode = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1032'"), String)
                strSqlQry = "SELECT partymast.partycode, partymast.partyname,partymast.add1,partymast.add2,partymast.add3,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1 FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode   where partymast.sptypecode= '" + sptypecode + "'  ORDER BY partymast.partyname ASC"
            ElseIf type = "HOT" Then
                strSqlQry = "SELECT partymast.partycode, partymast.partyname, partymast.active,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_hotelgroup(partymast.partycode),'') hotelgroup,partymast.tel1, partymast.contact1 ,isnull(A.UserName,'') AddUser  ,CONVERT(VARCHAR,partymast.adddate,22) adddate,isnull(B.UserName,'') ModUser,CONVERT(VARCHAR,partymast.moddate,22) moddate FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode left outer JOIN hotelchainmaster on hotelchainmaster.hotelchaincode =partymast.hotelchaincode left outer JOIN hotelstatus on hotelstatus.hotelstatuscode =partymast.hotelstatuscode left JOIN UserMaster A ON A.UserCode =partymast.adduser 	left JOIN UserMaster B ON B.UserCode =partymast.moduser  where partymast.sptypecode='HOT'  ORDER BY partymast.partyname ASC"
            ElseIf type = "EXC" Then
                sptypecode = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1033'"), String)
                strSqlQry = "SELECT partymast.partycode, partymast.partyname, partymast.active,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1 FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode   where partymast.sptypecode= '" + sptypecode + "'  ORDER BY partymast.partyname ASC"
            End If
            'CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1501'"), String)
            Dim ds As New DataSet

            Dim wb As New XLWorkbook
            Dim ws = wb.Worksheets.Add("HotelReport")
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, sqlConn)
            myDataAdapter.Fill(ds)
            Dim hotelgroups As New DataTable
            hotelgroups = ds.Tables(0)
            ws.Columns.AdjustToContents()

            ws.Column("A").Width = 50
            ws.Column("B").Width = 20

            Dim rownum As Integer = 4



            'Comapny Name Heading
            ws.Cell("A1").Value = rptcompanyname
            Dim company = ws.Range(1, 1, 1, 2).Merge()
            company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
            company.Style.Font.SetBold().Font.FontSize = 14
            company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
            company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            ws.Cell("A2").Value = reportname
            Dim rptreportname = ws.Range(2, 1, 2, 2).Merge()
            rptreportname.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
            rptreportname.Style.Font.SetBold().Font.FontSize = 12
            rptreportname.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
            rptreportname.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            ws.Range(rownum, 1, rownum, 2).Style.Font.SetBold().Font.FontSize = 10
            ws.Range(rownum, 1, rownum, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.WrapText = True
            ws.Range(rownum, 1, rownum, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
            ws.Cell(rownum, 1).Value = heading(0)
            ws.Cell(rownum, 2).Value = heading(1)


            rownum += 1
            ws.Range(rownum, 1, rownum + hotelgroups.Rows.Count - 1, 2).Style.Font.FontSize = 9
            ws.Range(rownum, 1, rownum + hotelgroups.Rows.Count - 1, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.WrapText = True
            ws.Range(rownum, 1, rownum + hotelgroups.Rows.Count - 1, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
            For i = 0 To hotelgroups.Rows.Count - 1
                ws.Cell(rownum, 1).Value = hotelgroups.Rows(i)("partyname")

                If type = "OTH" Or type = "TRFS" Or type = "VISA" Then
                    'SuppAddress
                    If Not IsDBNull(hotelgroups.Rows(i)("add1")) AndAlso hotelgroups.Rows(i)("add1") <> "" Then
                        SuppAddress = hotelgroups.Rows(i)("add1")
                        If Not IsDBNull(hotelgroups.Rows(i)("add2")) AndAlso hotelgroups.Rows(i)("add2") <> "" Then
                            SuppAddress = SuppAddress & " - " & hotelgroups.Rows(i)("add2")
                        End If
                        If Not IsDBNull(hotelgroups.Rows(i)("add3")) AndAlso hotelgroups.Rows(i)("add3") <> "" Then
                            SuppAddress = SuppAddress & " - " & hotelgroups.Rows(i)("add3")
                        End If

                    ElseIf Not IsDBNull(hotelgroups.Rows(i)("add2")) AndAlso hotelgroups.Rows(i)("add2") <> "" Then
                        SuppAddress = hotelgroups.Rows(i)("add2")
                        If Not IsDBNull(hotelgroups.Rows(i)("add1")) AndAlso hotelgroups.Rows(i)("add1") <> "" Then
                            SuppAddress = SuppAddress & " - " & hotelgroups.Rows(i)("add1")
                        End If
                        If Not IsDBNull(hotelgroups.Rows(i)("add3")) AndAlso hotelgroups.Rows(i)("add3") <> "" Then
                            SuppAddress = SuppAddress & " - " & hotelgroups.Rows(i)("add3")
                        End If
                    ElseIf Not IsDBNull(hotelgroups.Rows(i)("add3")) AndAlso hotelgroups.Rows(i)("add3") <> "" Then
                        SuppAddress = hotelgroups.Rows(i)("add3")
                        If Not IsDBNull(hotelgroups.Rows(i)("add2")) AndAlso hotelgroups.Rows(i)("add2") <> "" Then
                            SuppAddress = SuppAddress & " - " & hotelgroups.Rows(i)("add2")
                        End If
                        If Not IsDBNull(hotelgroups.Rows(i)("add1")) AndAlso hotelgroups.Rows(i)("add1") <> "" Then
                            SuppAddress = SuppAddress & " - " & hotelgroups.Rows(i)("add1")
                        End If
                    Else
                        SuppAddress = "-"
                    End If
                    ws.Cell(rownum, 2).Value = SuppAddress
                    'ws.Cell(rownum, 2).Value = hotelgroups.Rows(i)("add1") & " - " & hotelgroups.Rows(i)("add2") & " - " & hotelgroups.Rows(i)("add3")
                ElseIf type = "HOT" Then
                    ws.Cell(rownum, 2).Value = IIf(hotelgroups.Rows(i)("active") = 1, "Active", "InActive")
                ElseIf type = "EXC" Then
                    ws.Cell(rownum, 2).Value = IIf(hotelgroups.Rows(i)("active") = 1, "Yes", "No")
                End If
                rownum += 1
            Next
            ws.Cell((rownum + 2), 1).Value = "Printed Date : " & Now.ToString("dd/MM/yyyy")
            ws.Range((rownum + 2), 1, (rownum + 2), 4).Merge()
            Using wStream As New MemoryStream()
                wb.SaveAs(wStream)
                bytes = wStream.ToArray()
            End Using
        Catch ex As Exception

        End Try

    End Sub


    Public Sub ExportToExcelParty(ByVal heading() As String, ByVal type As String, ByVal reportname As String, ByVal rptcompanyname As String, ByRef bytes() As Byte)
        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim sptypecode As String
            Dim SuppAddress As String
            Dim sheetName As String = "Suppliers"
            If type = "OTH" Then
                sptypecode = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1501'"), String)
                strSqlQry = "SELECT '''' + partymast.partycode partycode, partymast.partyname,partymast.add1,partymast.add2,partymast.add3,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1,'''' + isnull(partymast.TRNNo,'') TRNNo FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode   where partymast.sptypecode= '" + sptypecode + "'  ORDER BY partymast.partyname ASC"
                sheetName = "Others"
            ElseIf type = "TRFS" Then
                sptypecode = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='564'"), String)
                strSqlQry = "SELECT '''' +  partymast.partycode partycode, partymast.partyname,partymast.add1,partymast.add2,partymast.add3,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1,'''' + isnull(partymast.TRNNo,'') TRNNo FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode   where partymast.sptypecode= '" + sptypecode + "'  ORDER BY partymast.partyname ASC"
                sheetName = "Transfers"
            ElseIf type = "VISA" Then
                sptypecode = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1032'"), String)
                strSqlQry = "SELECT '''' +  partymast.partycode partycode, partymast.partyname,partymast.add1,partymast.add2,partymast.add3,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1,'''' + isnull(partymast.TRNNo,'') TRNNo FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode   where partymast.sptypecode= '" + sptypecode + "'  ORDER BY partymast.partyname ASC"
                sheetName = "Visa"
            ElseIf type = "HOT" Then
                strSqlQry = "SELECT  '''' +  partymast.partycode partycode, partymast.partyname, partymast.active,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_hotelgroup(partymast.partycode),'') hotelgroup,partymast.tel1, partymast.contact1 ,isnull(A.UserName,'') AddUser  ,CONVERT(VARCHAR,partymast.adddate,22) adddate,isnull(B.UserName,'') ModUser,CONVERT(VARCHAR,partymast.moddate,22) moddate,'''' + isnull(partymast.TRNNo,'') TRNNo FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode left outer JOIN hotelchainmaster on hotelchainmaster.hotelchaincode =partymast.hotelchaincode left outer JOIN hotelstatus on hotelstatus.hotelstatuscode =partymast.hotelstatuscode left JOIN UserMaster A ON A.UserCode =partymast.adduser 	left JOIN UserMaster B ON B.UserCode =partymast.moduser  where partymast.sptypecode='HOT'  ORDER BY partymast.partyname ASC"
                sheetName = "Hotel"
            ElseIf type = "EXC" Then
                sptypecode = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1033'"), String)
                strSqlQry = "SELECT '''' +  partymast.partycode partycode, partymast.partyname, partymast.active,catmast.catname,citymast.cityname,sectormaster.sectorname,isnull(dbo.fn_get_suppliergroups(partymast.partycode),'') suppliergroup,partymast.tel1, partymast.contact1,'''' + isnull(partymast.TRNNo,'') TRNNo FROM partymast INNER JOIN  citymast ON partymast.citycode = citymast.citycode INNER JOIN  catmast ON partymast.catcode = catmast.catcode INNER JOIN  ctrymast ON partymast.ctrycode = ctrymast.ctrycode left  JOIN  sectormaster ON partymast.sectorcode = sectormaster.sectorcode   where partymast.sptypecode= '" + sptypecode + "'  ORDER BY partymast.partyname ASC"
                sheetName = "Excursion"
            End If
            'CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1501'"), String)
            Dim ds As New DataSet

            Dim wb As New XLWorkbook
            Dim ws = wb.Worksheets.Add(sheetName)
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, sqlConn)
            myDataAdapter.Fill(ds)
            Dim hotelgroups As New DataTable
            hotelgroups = ds.Tables(0)
            ws.Columns.AdjustToContents()

            ws.Column("A").Width = 20
            ws.Column("B").Width = 50
            ws.Column("C").Width = 30
            ws.Column("D").Width = 30
            Dim rownum As Integer = 4



            'Comapny Name Heading
            ws.Cell("A1").Value = rptcompanyname
            Dim company = ws.Range(1, 1, 1, 4).Merge()
            company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
            company.Style.Font.SetBold().Font.FontSize = 14
            company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
            company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            ws.Cell("A2").Value = reportname
            Dim rptreportname = ws.Range(2, 1, 2, 4).Merge()
            rptreportname.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
            rptreportname.Style.Font.SetBold().Font.FontSize = 12
            rptreportname.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
            rptreportname.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            ws.Range(rownum, 1, rownum, 4).Style.Font.SetBold().Font.FontSize = 10
            ws.Range(rownum, 1, rownum, 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.WrapText = True
            ws.Range(rownum, 1, rownum, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
            For i = 0 To heading.Length - 1
                ws.Cell(rownum, i + 1).Value = heading(i)

            Next
            rownum += 1
            ws.Range(rownum, 1, rownum + hotelgroups.Rows.Count - 1, 4).Style.Font.FontSize = 9
            ws.Range(rownum, 1, rownum + hotelgroups.Rows.Count - 1, 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.WrapText = True
            ws.Range(rownum, 1, rownum + hotelgroups.Rows.Count - 1, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
            For i = 0 To hotelgroups.Rows.Count - 1
                ws.Cell(rownum, 1).Value = hotelgroups.Rows(i)("partycode")
                ws.Cell(rownum, 2).Value = hotelgroups.Rows(i)("partyname")
                If type = "OTH" Or type = "TRFS" Or type = "VISA" Then
                    'SuppAddress
                    If Not IsDBNull(hotelgroups.Rows(i)("add1")) AndAlso hotelgroups.Rows(i)("add1") <> "" Then
                        SuppAddress = hotelgroups.Rows(i)("add1")
                        If Not IsDBNull(hotelgroups.Rows(i)("add2")) AndAlso hotelgroups.Rows(i)("add2") <> "" Then
                            SuppAddress = SuppAddress & " - " & hotelgroups.Rows(i)("add2")
                        End If
                        If Not IsDBNull(hotelgroups.Rows(i)("add3")) AndAlso hotelgroups.Rows(i)("add3") <> "" Then
                            SuppAddress = SuppAddress & " - " & hotelgroups.Rows(i)("add3")
                        End If

                    ElseIf Not IsDBNull(hotelgroups.Rows(i)("add2")) AndAlso hotelgroups.Rows(i)("add2") <> "" Then
                        SuppAddress = hotelgroups.Rows(i)("add2")
                        If Not IsDBNull(hotelgroups.Rows(i)("add1")) AndAlso hotelgroups.Rows(i)("add1") <> "" Then
                            SuppAddress = SuppAddress & " - " & hotelgroups.Rows(i)("add1")
                        End If
                        If Not IsDBNull(hotelgroups.Rows(i)("add3")) AndAlso hotelgroups.Rows(i)("add3") <> "" Then
                            SuppAddress = SuppAddress & " - " & hotelgroups.Rows(i)("add3")
                        End If
                    ElseIf Not IsDBNull(hotelgroups.Rows(i)("add3")) AndAlso hotelgroups.Rows(i)("add3") <> "" Then
                        SuppAddress = hotelgroups.Rows(i)("add3")
                        If Not IsDBNull(hotelgroups.Rows(i)("add2")) AndAlso hotelgroups.Rows(i)("add2") <> "" Then
                            SuppAddress = SuppAddress & " - " & hotelgroups.Rows(i)("add2")
                        End If
                        If Not IsDBNull(hotelgroups.Rows(i)("add1")) AndAlso hotelgroups.Rows(i)("add1") <> "" Then
                            SuppAddress = SuppAddress & " - " & hotelgroups.Rows(i)("add1")
                        End If
                    Else
                        SuppAddress = "-"
                    End If
                    ws.Cell(rownum, 3).Value = SuppAddress
                    'ws.Cell(rownum, 2).Value = hotelgroups.Rows(i)("add1") & " - " & hotelgroups.Rows(i)("add2") & " - " & hotelgroups.Rows(i)("add3")
                ElseIf type = "HOT" Then
                    ws.Cell(rownum, 3).Value = IIf(hotelgroups.Rows(i)("active") = 1, "Active", "InActive")
                ElseIf type = "EXC" Then
                    ws.Cell(rownum, 3).Value = IIf(hotelgroups.Rows(i)("active") = 1, "Yes", "No")
                End If
                ws.Cell(rownum, 4).Value = hotelgroups.Rows(i)("TRNNo")
                rownum += 1
            Next
            ws.Cell((rownum + 2), 1).Value = "Printed Date : " & Now.ToString("dd/MM/yyyy")
            ws.Range((rownum + 2), 1, (rownum + 2), 4).Merge()
            Using wStream As New MemoryStream()
                wb.SaveAs(wStream)
                bytes = wStream.ToArray()
            End Using
        Catch ex As Exception

        End Try

    End Sub


#End Region

End Class
