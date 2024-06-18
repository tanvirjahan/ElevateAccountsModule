Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptRGprint
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim skip As String, sptypecode As String, partycodef As String, partycodet As String
    Dim citycodef As String, citycodet As String, asondate As String, plgrpcodef As String, plgrpcodet As String
    Dim repfilter As String, reportoption As String, reporttitle As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState.Add("Pageame", Request.QueryString("Pageame"))
        ViewState.Add("BackPageName", Request.QueryString("BackPageName"))

        If Not Session("CompanyName") Is Nothing Then
            Me.Page.Title = CType(Session("CompanyName"), String)
        End If

        Try
            If Request.QueryString("regno") <> "" Then
                'sptypecode = Request.QueryString("sptypecode")
                ViewState.Add("regno", Request.QueryString("regno"))
            Else
                ViewState.Add("regno", String.Empty)
            End If
            If Request.QueryString("repfilter") <> "" Then
                'partycodef = Request.QueryString("partycodef")
                ViewState.Add("repfilter", Request.QueryString("repfilter"))
            Else
                ViewState.Add("repfilter", String.Empty)
            End If

            ViewState.Add("RepCalledFrom", 0)
            '  btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")
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
        '  rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
        rptreportname = "Registration Form"

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



        'If Session("skip") = "Y" Then

        ' rep.Load(Server.MapPath("~\Agentaonline\agentReport\rptregistration_report.rpt"))

        rep.Load(Server.MapPath("../AgentsOnline/AgentReport/Newagentregistrationform.rpt"))
        'rep.Load(Server.MapPath("~\Report\Newagentregistrationform.rpt"))


        Me.CRVPLprint.ReportSource = rep
        Dim RepTbls As Tables = rep.Database.Tables

        For Each RepTbl As Table In RepTbls
            Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            RepTblLogonInfo.ConnectionInfo = ConnInfo
            RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        Next

        'rep.SummaryInfo.ReportTitle = ViewState("reporttitle")

        pnames = rep.DataDefinition.ParameterFields

        'pname = pnames.Item("CompanyName")
        'paramvalue.Value = rptcompanyname
        'param = pname.CurrentValues
        'param.Add(paramvalue)
        'pname.ApplyCurrentValues(param)

        'pname = pnames.Item("ReportName")
        'paramvalue.Value = rptreportname
        'param = pname.CurrentValues
        'param.Add(paramvalue)
        'pname.ApplyCurrentValues(param)


        'pname = pnames.Item("repfilter")
        ''paramvalue.Value = "Updated as on : " + Session("reportoption")
        'paramvalue.Value = " Registration no : " + Trim(ViewState("regno"))
        'param = pname.CurrentValues
        'param.Add(paramvalue)
        'pname.ApplyCurrentValues(param)

        pname = pnames.Item("regno")
        'paramvalue.Value = Session("skip")
        paramvalue.Value = ViewState("regno")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)



        Me.CRVPLprint.ReportSource = rep
        'If strSelectionFormula <> "" Then
        '    CRVSupplierPolicies.SelectionFormula = strSelectionFormula
        'End If
        'Session.Add("ReportSource", rep)
        'Me.CRVPLprint.DataBind()

        Response.Buffer = False
        Response.ClearContent()
        Response.ClearHeaders()
        rep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, False, "Report")
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If ViewState("RepCalledFrom") <> 1 Then
            rep.Close()
            rep.Dispose()
            ' Session("ColReportParams") = Nothing
        End If
    End Sub




    'Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click

    '    'Session("skip") = ""
    '    'Session("sptypecode") = ""
    '    'Session("partycodef") = ""
    '    'Session("partycodet") = ""
    '    'Session("plgrpcodef") = ""
    '    'Session("plgrpcodet") = ""
    '    'Session("citycodef") = ""
    '    'Session("citycodet") = ""
    '    'Session("asondate") = ""

    '    'Session("repfilter") = ""
    '    'Session("reportoption") = ""
    '    'Session("ReportTitle") = ""
    '    ''        Response.Redirect("rptPriceExpirySearch.aspx", False)
    '    'Response.Redirect("rptPriceExpirySearch.aspx?skip=" & skip & "&sptypecode=" & sptypecode _
    '    '& "&partycodef=" & partycodef & "&partycodet=" & partycodet & "&plgrpcodef=" & plgrpcodef & "&plgrpcodet=" & plgrpcodet _
    '    '& "&citycodef=" & citycodef & "&citycodet=" & citycodet & "&asondate=" & asondate _
    '    '& "&repfilter=" & repfilter & "&reportoption=" & reportoption & "&reporttitle=" & reporttitle, False)
    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    'End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Session.Add("ReportSource", rep)
    '    Dim strpop As String = ""
    '    strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    'End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Session.Add("ReportSource", rep)
    '    ViewState.Add("RepCalledFrom", 1)
    '    Dim strpop As String = ""
    '    strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    'End Sub
End Class
