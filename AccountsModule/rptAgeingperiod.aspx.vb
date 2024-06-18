Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptAgeingperiod
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim datetype As Integer, curr As Integer, agingtype As Integer, sumdet As Integer
    Dim pdcyesno As Integer, includezero As Integer, groupby As Integer
    Dim fromdate As String, todate As String, type As String, fromacct As String, toacct As String
    Dim fromctrl As String, toctrl As String, fromcat As String, tocat As String
    Dim fromcity As String, tocity As String, fromctry As String, toctry As String, remarks As String
    Dim period1 As Integer, period2 As Integer
    Dim reporttype As String, repfilter As String, reporttitle As String, orderby As String
    Dim rptgrp As String
    Dim rptodr As String
    Dim frommarket As String, tomarket As String
    Dim rpttype As Integer
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

            If Request.QueryString("period1") <> "" Then
                period1 = Trim(Request.QueryString("period1"))
            End If

            If Request.QueryString("period2") <> "" Then
                period2 = Trim(Request.QueryString("period2"))
            End If

            If Request.QueryString("agingtype") <> "" Then
                agingtype = Trim(Request.QueryString("agingtype"))
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
            If Request.QueryString("rptgroup") <> "" Then
                rptgrp = Trim(Request.QueryString("rptgroup"))
            End If
            If Request.QueryString("rptOrder") <> "" Then
                rptodr = Trim(Request.QueryString("rptOrder"))
            End If

            If Request.QueryString("frommarketcode") <> "" Then
                frommarket = Trim(Request.QueryString("frommarketcode"))
            End If
            If Request.QueryString("tomarketcode") <> "" Then
                tomarket = Trim(Request.QueryString("tomarketcode"))
            End If


            ViewState.Add("RepCalledFrom", 0)
            btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")
            BindReport()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
    '#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'BindReport()
        Catch ex As Exception

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
    '#End Region
    'End Sub
    Private Sub BindReport()
        Dim strReportTitle As String = ""
        Dim reportoption As String = ""
        'If Session("Rep") Is Nothing Then
        rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
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

        rptreportname = "Customer Ageing Period"
        If reporttype = 0 Then
            rep.Load(Server.MapPath("..\Report\ageing_party_split.rpt"))
        Else
            rep.Load(Server.MapPath("..\Report\ageing_party_split_detail.rpt"))
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
        paramvalue.Value = reporttype
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@web")
        paramvalue.Value = 0
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@col1")
        paramvalue.Value = period1
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@col2")
        paramvalue.Value = period2
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("groupby")
        paramvalue.Value = groupby
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("orderby")
        paramvalue.Value = orderby
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        'Me.CRVReport.ReportSource = rep
        'If strSelectionFormula <> "" Then
        '    CRVSupplierPolicies.SelectionFormula = strSelectionFormula
        'End If
        Session.Add("ReportSource", rep)
        Me.CRVReport.DataBind()
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
