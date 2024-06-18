Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptStatement
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim datetype As Integer, curr As Integer, agingtype As Integer, sumdet As Integer, custtype As Integer, supptype As Integer
    Dim pdcyesno As Integer, includezero As Integer, groupby As Integer
    Dim fromdate As String, todate As String, type As String, fromacct As String, toacct As String, inclproforma As String, agdate As String
    Dim fromctrl As String, toctrl As String, fromcat As String, tocat As String, custgroup_sp_type As String
    Dim fromcity As String, tocity As String, fromctry As String, toctry As String, remarks As String
    Dim reporttype As String, repfilter As String, reporttitle As String, orderby As String
    Dim rptgrp As String
    Dim rptodr As String
    Dim rptcus As String 'Added by Archana
    Dim frommarket As String, tomarket As String
    Dim divcode As String
    Dim rpttype As Integer
    Dim objDate As New clsDateTime
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If

            If Request.QueryString("tran_type") <> "" Then
                ViewState.Add("TranType", Request.QueryString("tran_type"))
            End If
            If Request.QueryString("Pageame") <> "" Then
                ViewState.Add("Pageame", Request.QueryString("Pageame"))
            End If
            If Request.QueryString("BackPageName") <> "" Then
                ViewState.Add("BackPageName", Request.QueryString("BackPageName"))
            End If

            If Request.QueryString("datetype") <> "" Then
                datetype = Trim(Request.QueryString("datetype"))
            End If

            If Request.QueryString("divid") <> "" Then
                divcode = Trim(Request.QueryString("divid"))
            End If
            If Request.QueryString("custgroup_sp_type") <> "" Then
                custgroup_sp_type = Trim(Request.QueryString("custgroup_sp_type"))
            End If
            If Request.QueryString("fromdate") <> "" Then
                fromdate = Trim(Request.QueryString("fromdate"))
            End If
            If Request.QueryString("todate") <> "" Then
                todate = Trim(Request.QueryString("todate"))
            End If
            If Request.QueryString("type") <> "" Then
                type = Trim(Request.QueryString("type"))
            End If
            If Request.QueryString("curr") <> "" Then
                curr = Trim(Request.QueryString("curr"))
            End If
            If Request.QueryString("fromaccode") <> "" Then
                fromacct = Trim(Request.QueryString("fromaccode"))
            End If
            If Request.QueryString("toaccode") <> "" Then
                toacct = Trim(Request.QueryString("toaccode"))
            End If
            If Request.QueryString("fromctrlcode") <> "" Then
                fromctrl = Trim(Request.QueryString("fromctrlcode"))
            End If
            If Request.QueryString("toctrlcode") <> "" Then
                toctrl = Trim(Request.QueryString("toctrlcode"))
            End If

            If Request.QueryString("fromccatcode") <> "" Then
                fromcat = Trim(Request.QueryString("fromccatcode"))
            End If
            If Request.QueryString("toccatcode") <> "" Then
                tocat = Trim(Request.QueryString("toccatcode"))
            End If
            If Request.QueryString("fromcitycode") <> "" Then
                fromcity = Trim(Request.QueryString("fromcitycode"))
            End If
            If Request.QueryString("tocitycode") <> "" Then
                tocity = Trim(Request.QueryString("tocitycode"))
            End If
            If Request.QueryString("fromctrycode") <> "" Then
                fromctry = Trim(Request.QueryString("fromctrycode"))
            End If
            If Request.QueryString("toctrycode") <> "" Then
                toctry = Trim(Request.QueryString("toctrycode"))
            End If
            If Request.QueryString("agingtype") <> "" Then
                agingtype = Trim(Request.QueryString("agingtype"))
            End If
            If Request.QueryString("pdcyesno") <> "" Then
                pdcyesno = Trim(Request.QueryString("pdcyesno"))
            End If
            If Request.QueryString("includezero") <> "" Then
                includezero = Trim(Request.QueryString("includezero"))
            End If
            If Request.QueryString("remarks") <> "" Then
                remarks = Trim(Request.QueryString("remarks"))
            End If

            If Request.QueryString("rpttype") <> "" Then
                rpttype = Trim(Request.QueryString("rpttype"))
            End If


            If Request.QueryString("groupby") <> "" Then
                groupby = Trim(Request.QueryString("groupby"))
            End If
            If Request.QueryString("sumdet") <> "" Then
                sumdet = Trim(Request.QueryString("sumdet"))
            End If
            If Request.QueryString("reporttype") <> "" Then
                reporttype = Trim(Request.QueryString("reporttype"))
            End If
            If Request.QueryString("repfilter") <> "" Then
                repfilter = Trim(Request.QueryString("repfilter"))
            End If
            If Request.QueryString("reporttitle") <> "" Then
                reporttitle = Trim(Request.QueryString("reporttitle"))
            End If
            If Request.QueryString("orderby") <> "" Then
                orderby = Trim(Request.QueryString("orderby"))
            End If

            If Request.QueryString("custtype") <> "" Then
                custtype = Trim(Request.QueryString("custtype"))
            End If
            'Added custtype by Archana on 02-04-2015

            If Request.QueryString("rptgroup") <> "" Then
                rptgrp = Trim(Request.QueryString("rptgroup"))
            End If
            If Request.QueryString("rptOrder") <> "" Then
                rptodr = Trim(Request.QueryString("rptOrder"))
            End If

            If Request.QueryString("custtype") <> "" Then
                rptcus = Trim(Request.QueryString("custtype"))
            End If



            If Request.QueryString("frommarketcode") <> "" Then
                frommarket = Trim(Request.QueryString("frommarketcode"))
            End If
            If Request.QueryString("tomarketcode") <> "" Then
                tomarket = Trim(Request.QueryString("tomarketcode"))
            End If

            If Request.QueryString("agdate") <> "" Then
                agdate = Trim(Request.QueryString("agdate"))
            End If

            If Request.QueryString("strincludeproforma") <> "" Then
                inclproforma = Trim(Request.QueryString("strincludeproforma"))
            End If

            If Request.QueryString("supptype") <> "" Then
                supptype = Trim(Request.QueryString("supptype"))
            End If


            ViewState.Add("RepCalledFrom", 0)
            btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")
            BindReport()
        Catch ex As Exception
            '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("RptCustomerStatementAccount.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub
    '#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'BindReport()
        Catch ex As Exception

            '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objutils.WritErrorLog("RptCustomerStatementAccount.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))


        End Try
    End Sub
    '#End Region
    'End Sub
    Private Sub BindReport()
        Dim strReportTitle As String = ""
        Dim reportoption As String = ""
        'If Session("Rep") Is Nothing Then
        '  rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)

        If divcode <> "" Then
            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
        Else
            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
        End If
        'rptreportname = "Arrival Report"

        Dim pnames As ParameterFieldDefinitions
        Dim pname As ParameterFieldDefinition
        Dim param As New ParameterValues
        Dim paramvalue As New ParameterDiscreteValue
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
        Dim proformaflg As String = "N"
        If Request.QueryString("ageingreporttyp") = "0" Then
            If reporttype = "SupplierAgeingSummary" Or reporttype = "SupplierAgeingDetail" Then
                If reporttype = "SupplierAgeingSummary" Then
                    If type = "C" Then
                        rptreportname = "CUSTOMER AGEING SUMMARY"
                    ElseIf type = "S" Then
                        rptreportname = "SUPPLIER AGEING SUMMARY"
                    Else
                        rptreportname = "SUPPLIER AGENT AGEING SUMMARY"
                    End If
                    rep.Load(Server.MapPath("~\Report\rptPartyAging.rpt"))
                    proformaflg = "Y"
                Else
                    If type = "C" Then
                        rptreportname = "CUSTOMER AGEING DETAIL"
                    ElseIf type = "S" Then
                        rptreportname = "SUPPLIER AGEING DETAIL"
                    Else
                        rptreportname = "Supplier Agent Ageing Detail"
                    End If
                    rep.Load(Server.MapPath("~\Report\rptPartyAging_Detail.rpt"))
                    proformaflg = "Y"
                End If
            Else
                If type = "C" Then
                    rptreportname = "CUSTOMER STATEMENT"
                    If rpttype = 0 Then
                        'rep.Load(Server.MapPath("..\Report\rptCustomerStatement.rpt"))
                        rep.Load(Server.MapPath("..\Report\rptCustomerStatementnew.rpt"))
                    Else
                        'rep.Load(Server.MapPath("..\Report\rptCustomerStatement_detail.rpt"))
                        rep.Load(Server.MapPath("..\Report\rptCustomerStatement_detailnew.rpt"))

                    End If
                    proformaflg = "Y"
                ElseIf type = "S" Then
                    rptreportname = "SUPPLIER STATEMENT"
                    'rep.Load(Server.MapPath("..\Report\rptSupplierStatement.rpt"))
                    rep.Load(Server.MapPath("..\Report\rptSupplierStatementnew.rpt"))
                    'proformaflg = "N"
                ElseIf type = "A" Then
                    rptreportname = "Supplier Agent Statement"
                    'rep.Load(Server.MapPath("..\Report\rptPartyStatement.rpt"))
                    'proformaflg = "Y"
                    rep.Load(Server.MapPath("..\Report\rptSupplierStatementnew.rpt"))
                    'proformaflg = "N"
                End If

            End If
        ElseIf Request.QueryString("ageingreporttyp") = "1" Then
            If type = "C" Then
                rptreportname = "CUSTOMER PENDING INVOICES" 'CUSTOMER AGEING REPORT
                rep.Load(Server.MapPath("..\Report\rptCustomerStatement_Ageing1.rpt"))
                proformaflg = "Y"
            ElseIf type = "S" Then
                rptreportname = "SUPPLIER PENDING INVOICES" 'SUPPLIER AGEING REPORT
                rep.Load(Server.MapPath("..\Report\rptSupplierStatement_Ageing1.rpt"))
                proformaflg = "N"
            End If
        ElseIf Request.QueryString("ageingreporttyp") = "2" Then
            If type = "C" Then
                rptreportname = "CUSTOMER PENDING INVOICES"
                rep.Load(Server.MapPath("..\Report\rptCustomerStatement_Ageing2.rpt"))
                proformaflg = "Y"
            ElseIf type = "S" Then
                rptreportname = "SUPPLIER PENDING INVOICES"
                rep.Load(Server.MapPath("..\Report\rptSupplierStatement_Ageing2.rpt"))
                proformaflg = "N"
            End If
        End If

        Me.CRVReport.ReportSource = rep
        Dim RepTbls As Tables = rep.Database.Tables

        For Each RepTbl As Table In RepTbls
            Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            RepTblLogonInfo.ConnectionInfo = ConnInfo
            RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        Next

        rep.SummaryInfo.ReportTitle = reporttitle

        pnames = rep.DataDefinition.ParameterFields

        'pname = pnames.Item("CompanyName")
        'paramvalue.Value = rptcompanyname
        'param = pname.CurrentValues
        'param.Add(paramvalue)
        'pname.ApplyCurrentValues(param)



        If rptreportname <> "Supplier AgentStatement" Then
            pname = pnames.Item("cmb")
            paramvalue.Value = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1050"), String)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
        Else
            pname = pnames.Item("CompanyName")
            paramvalue.Value = rptcompanyname
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        End If

        pname = pnames.Item("ReportName")
        paramvalue.Value = rptreportname
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)



        If Request.QueryString("ageingreporttyp") = "1" Or Request.QueryString("ageingreporttyp") = "2" Then
            If type = "C" Then
                pname = pnames.Item("decno")
                paramvalue.Value = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), String)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            End If
        End If
        If rptreportname = "CUSTOMER STATEMENT" Or rptreportname = "SUPPLIER STATEMENT" Or rptreportname = "Supplier Agent Statement" Then
            pname = pnames.Item("decno")
            paramvalue.Value = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), String)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
        End If

        If reporttype = "SupplierAgeingSummary" Or reporttype = "SupplierAgeingDetail" Then
            pname = pnames.Item("@todate")
            paramvalue.Value = Trim(fromdate)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@type")
            paramvalue.Value = Trim(type)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@currflg")
            paramvalue.Value = Val(curr)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@fromacct")
            paramvalue.Value = Trim(fromacct)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@toacct")
            paramvalue.Value = Trim(toacct)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@fromcontrol")
            paramvalue.Value = Trim(fromctrl)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@tocontrol")
            paramvalue.Value = Trim(toctrl)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@fromcat")
            paramvalue.Value = Trim(fromcat)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@tocat")
            paramvalue.Value = Trim(tocat)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@fromcity")
            paramvalue.Value = Trim(fromcity)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@tocity")
            paramvalue.Value = Trim(tocity)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@fromctry")
            paramvalue.Value = Trim(fromctry)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@toctry")
            paramvalue.Value = Trim(toctry)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@agingtype")
            paramvalue.Value = Val(agingtype)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@summdet")
            paramvalue.Value = Val(sumdet)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("groupby")
            paramvalue.Value = Val(groupby)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("orderby")
            paramvalue.Value = Trim(orderby)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@web")
            paramvalue.Value = 0
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("currdate")
            paramvalue.Value = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "yyyy/MM/dd")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@divcode")
            paramvalue.Value = divcode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@custgroup_sp_type")
            paramvalue.Value = Trim(custgroup_sp_type)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("@inclproforma")
            paramvalue.Value = IIf(IsNothing(inclproforma), "0", inclproforma)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        Else ' Statement
            pname = pnames.Item("@datetype")
            paramvalue.Value = Val(datetype)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@fromdate")
            paramvalue.Value = Trim(fromdate)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@todate")
            paramvalue.Value = Trim(todate)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@type")
            paramvalue.Value = Trim(type)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@currflg")
            paramvalue.Value = Val(curr)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@fromacct")
            paramvalue.Value = Trim(fromacct)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@toacct")
            paramvalue.Value = Trim(toacct)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@fromcontrol")
            paramvalue.Value = Trim(fromctrl)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@tocontrol")
            paramvalue.Value = Trim(toctrl)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@fromcat")
            paramvalue.Value = Trim(fromcat)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@tocat")
            paramvalue.Value = Trim(tocat)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            ''If customer statement then will pass the Market in the city 
            If Trim(type) = "C" Then
                pname = pnames.Item("@fromcity")
                paramvalue.Value = Trim(frommarket)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@tocity")
                paramvalue.Value = Trim(tomarket)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            Else
                pname = pnames.Item("@fromcity")
                paramvalue.Value = Trim(fromcity)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("@tocity")
                paramvalue.Value = Trim(tocity)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            End If

            pname = pnames.Item("@fromctry")
            paramvalue.Value = Trim(fromctry)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@toctry")
            paramvalue.Value = Trim(toctry)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@agingtype")
            paramvalue.Value = Val(agingtype)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@pdcyesno")
            paramvalue.Value = Val(pdcyesno)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@includezero")
            paramvalue.Value = Val(includezero)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@web")
            paramvalue.Value = 0
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@divcode")
            paramvalue.Value = Trim(divcode) 'Trim(CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=511"), String))
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@custgroup_sp_type")
            paramvalue.Value = Trim(custgroup_sp_type)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@inclproforma")
            paramvalue.Value = IIf(IsNothing(inclproforma), "0", inclproforma)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        End If
        'If Request.QueryString("ageingreporttyp") = "1" Or Request.QueryString("ageingreporttyp") = "2" Then
        '    pname = pnames.Item("@includezero")
        '    paramvalue.Value = 1
        '    param = pname.CurrentValues
        '    param.Add(paramvalue)
        '    pname.ApplyCurrentValues(param)
        'Else
        '    pname = pnames.Item("@includezero")
        '    paramvalue.Value = Val(includezero)
        '    param = pname.CurrentValues
        '    param.Add(paramvalue)
        '    pname.ApplyCurrentValues(param)
        'End If


        If type = "C" And (rptreportname = "CUSTOMER AGEING SUMMARY" Or rptreportname = "CUSTOMER AGEING DETAIL") Then

            pname = pnames.Item("@custtype")
            paramvalue.Value = Val(custtype)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        Else
            If type = "S" And (rptreportname = "SUPPLIER AGEING SUMMARY" Or rptreportname = "SUPPLIER AGEING DETAIL") Then
                pname = pnames.Item("@custtype")
                paramvalue.Value = Val(supptype)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

            End If
        End If


        'If rptreportname = "Supplier Agent Statement" Then

        '    pname = pnames.Item("remarks")
        '    paramvalue.Value = Trim(remarks)
        '    param = pname.CurrentValues
        '    param.Add(paramvalue)
        '    pname.ApplyCurrentValues(param)

        'End If
        If rptreportname = "CUSTOMER STATEMENT" Or rptreportname = "SUPPLIER STATEMENT" Or rptreportname = "Supplier Agent Statement" Then
            ' for Aging sub report'''''''
            pname = pnames.Item("@todate1")
            paramvalue.Value = Trim(todate)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@summdet")
            paramvalue.Value = 0
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@agasondate")
            paramvalue.Value = Trim(agdate)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@inclproforma")
            paramvalue.Value = IIf(IsNothing(inclproforma), "0", inclproforma)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        End If

        'pname = pnames.Item("@custtype")
        'paramvalue.Value = 0
        'param = pname.CurrentValues
        'param.Add(paramvalue)
        'pname.ApplyCurrentValues(param)
        'Added by Archana

        'pname = pnames.Item("@divcode")
        'paramvalue.Value = divcode
        'param = pname.CurrentValues
        'param.Add(paramvalue)
        'pname.ApplyCurrentValues(param)

        'pname = pnames.Item("@web")
        'paramvalue.Value = 0
        'param = pname.CurrentValues
        'param.Add(paramvalue)
        'pname.ApplyCurrentValues(param)



        If proformaflg = "Y" And Trim(type) = "C" Then

            'pname = pnames.Item("@tocheckindate")
            'paramvalue.Value = "2020/12/31"
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)



            pname = pnames.Item("inclproforma")
            paramvalue.Value = IIf(IsNothing(inclproforma), "0", inclproforma)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        End If


        If proformaflg = "Y" And Trim(type) = "A" Then

            pname = pnames.Item("inclproforma")
            paramvalue.Value = IIf(IsNothing(inclproforma), "0", inclproforma)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        End If
        'If rptreportname = "SUPPLIER STATEMENT" Then



        pname = pnames.Item("addrLine1")
        paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1063)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("addrLine2")
        'paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1064)

        If type = "S" And fromacct Is Nothing = False Then
            paramvalue.Value = objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(atel1,'') from partymast(nolock) where partycode='" & fromacct & "'")
        ElseIf type = "C" And fromacct Is Nothing = False Then
            paramvalue.Value = objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(atel1,'') from agentmast(nolock) where agentcode='" & fromacct & "' and divcode='" & divcode & "'")
        ElseIf type = "A" And fromacct Is Nothing = False Then
            paramvalue.Value = objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(atel1,'') from supplier_agents(nolock) where supagentcode='" & fromacct & "'")
        Else
            paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1064)
        End If
        If paramvalue.Value = Nothing Then
            paramvalue.Value = ""
        End If
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("addrLine3")
        paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1065)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("addrLine4")
        ' paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1066)
        If type = "S" And fromacct Is Nothing = False Then
            paramvalue.Value = objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(afax1,'') from partymast(nolock) where partycode='" & fromacct & "'")
        ElseIf type = "C" And fromacct Is Nothing = False Then
            paramvalue.Value = objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(afax1,'') from agentmast(nolock) where agentcode='" & fromacct & "' and divcode='" & divcode & "'")
        ElseIf type = "A" And fromacct Is Nothing = False Then
            paramvalue.Value = objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(afax1,'') from supplier_agents(nolock) where supagentcode='" & fromacct & "'")
        Else
            paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1064)
        End If
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("addrLine5")
        paramvalue.Value = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1053)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)
        'End If

        'Me.CRVReport.ReportSource = rep
        'If strSelectionFormula <> "" Then
        '    CRVSupplierPolicies.SelectionFormula = strSelectionFormula
        'End If
        Session.Add("ReportSource", rep)
        Me.CRVReport.DataBind()
        'Response.Buffer = False
        'Response.ClearContent()
        'Response.ClearHeaders()
        'rep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, False, "Report")




        'Added If condition for C and S by Archana on 04/04/2015





    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If ViewState("RepCalledFrom") <> 1 Then
            rep.Close()
            rep.Dispose()
            ' Session("ColReportParams") = Nothing
        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click

        'If ViewState("TranType") = "" Then
        '    Response.Redirect(ViewState("BackPageName") & "?fromdate=" & fromdate & "&todate=" & todate _
        '    & "&datetype=" & datetype & "&type=" & type & "&curr=" & curr _
        '    & "&fromaccode=" & fromacct & "&toaccode=" & toacct & "&fromctrlcode=" & fromctrl _
        '    & "&toctrlcode=" & toctrl & "&fromccatcode=" & fromcat _
        '    & "&toccatcode=" & tocat & "&fromcitycode=" & fromcity & "&tocitycode=" & tocity _
        '    & "&fromctrycode=" & fromctry & "&toctrycode=" & toctry & "&agingtype=" & agingtype _
        '    & "&pdcyesno=" & pdcyesno & "&includezero=" & includezero & "&frommarketcode=" & frommarket & "&tomarketcode=" & tomarket _
        '    & "&remarks=" & remarks & "&groupby=" & reporttype & "&groupby=" & reporttitle _
        '    & "&sumdet=" & sumdet & "&reporttype=" & reporttype & "&orderby=" & orderby & "&rptgroup=" & rptgrp & "&rptorder=" & rptodr, False)
        'Else
        '    Response.Redirect(ViewState("BackPageName") & "?tran_type=" & ViewState("TranType") & "&fromdate=" & fromdate & "&todate=" & todate _
        '   & "&datetype=" & datetype & "&type=" & type & "&curr=" & curr _
        '   & "&fromaccode=" & fromacct & "&toaccode=" & toacct & "&fromctrlcode=" & fromctrl _
        '   & "&toctrlcode=" & toctrl & "&fromccatcode=" & fromcat _
        '   & "&toccatcode=" & tocat & "&fromcitycode=" & fromcity & "&tocitycode=" & tocity _
        '   & "&fromctrycode=" & fromctry & "&toctrycode=" & toctry & "&agingtype=" & agingtype _
        '   & "&pdcyesno=" & pdcyesno & "&includezero=" & includezero & "&frommarketcode=" & frommarket & "&tomarketcode=" & tomarket _
        '   & "&remarks=" & remarks & "&groupby=" & reporttype & "&groupby=" & reporttitle _
        '   & "&sumdet=" & sumdet & "&reporttype=" & reporttype & "&orderby=" & orderby & "&rptgroup=" & rptgrp & "&rptorder=" & rptodr, False)
        '        End If

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)

    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Session.Add("ReportSource", rep)
        ViewState.Add("RepCalledFrom", 1)
        Dim strpop As String = ""
        strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
End Class
