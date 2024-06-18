Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptSupplierpoliciesReport
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim sptypecode As String, partycode As String, citycode As String, ctrycode As String
    Dim fromdate As String, todate As String, catcode As String, scatcode As String, plgrpcode As String, asondate As String
    Dim promtype As String, repfilter As String, reportoption As String, reporttitle As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState.Add("Pageame", Request.QueryString("Pageame"))
        ViewState.Add("BackPageName", Request.QueryString("BackPageName"))

        If Not Session("CompanyName") Is Nothing Then
            Me.Page.Title = CType(Session("CompanyName"), String)
        End If

        'If CType(ViewState("BackPageName"), String) = "" Then
        '    'Response.Redirect("Login.aspx", False)
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        'Else
        '    If CType(ViewState("Pageame"), String) = "" Then
        '        'Response.Redirect(CType(Session("BackPageName"), String), False)
        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        '        Exit Sub
        '    Else
        '        Select Case CType(ViewState("Pageame"), String)
        Try
            If Request.QueryString("sptypecode") <> "" Then
                'sptypecode = Request.QueryString("sptypecode")
                ViewState.Add("sptypecode", Request.QueryString("sptypecode"))
            Else
                ViewState.Add("sptypecode", String.Empty)
            End If
            If Request.QueryString("partycode") <> "" Then
                'partycode = Request.QueryString("partycode")
                ViewState.Add("partycode", Request.QueryString("partycode"))
            Else
                ViewState.Add("partycode", String.Empty)
            End If
            If Request.QueryString("ctrycode") <> "" Then
                'ctrycode = Request.QueryString("ctrycode")
                ViewState.Add("ctrycode", Request.QueryString("ctrycode"))
            Else
                ViewState.Add("ctrycode", String.Empty)
            End If
            If Request.QueryString("citycode") <> "" Then
                'citycode = Request.QueryString("citycode")
                ViewState.Add("citycode", Request.QueryString("citycode"))
            Else
                ViewState.Add("citycode", String.Empty)
            End If
            If Request.QueryString("fromdate") <> "" Then
                'fromdate = Request.QueryString("fromdate")
                ViewState.Add("fromdate", Request.QueryString("fromdate"))
            Else
                ViewState.Add("fromdate", String.Empty)
            End If
            If Request.QueryString("todate") <> "" Then
                'todate = Request.QueryString("todate")
                ViewState.Add("todate", Request.QueryString("todate"))
            Else
                ViewState.Add("todate", String.Empty)
            End If
            If Request.QueryString("asondate") <> "" Then
                'asondate = Request.QueryString("asondate")
                ViewState.Add("asondate", Request.QueryString("asondate"))
            Else
                ViewState.Add("asondate", String.Empty)
            End If
            If Request.QueryString("promtype") <> "" Then
                'promtype = Request.QueryString("promtype")
                ViewState.Add("promtype", Request.QueryString("promtype"))
            Else
                ViewState.Add("promtype", String.Empty)
            End If

            If Request.QueryString("catcode") <> "" Then
                'catcode = Request.QueryString("catcode")
                ViewState.Add("catcode", Request.QueryString("catcode"))
            Else
                ViewState.Add("catcode", String.Empty)
            End If
            If Request.QueryString("scatcode") <> "" Then
                'scatcode = Request.QueryString("scatcode")
                ViewState.Add("scatcode", Request.QueryString("scatcode"))
            Else
                ViewState.Add("scatcode", String.Empty)
            End If

            If Request.QueryString("plgrpcode") <> "" Then
                'plgrpcode = Request.QueryString("plgrpcode")
                ViewState.Add("plgrpcode", Request.QueryString("plgrpcode"))
            Else
                ViewState.Add("plgrpcode", String.Empty)
            End If

            If Request.QueryString("approve") <> "" Then
                'promtype = Request.QueryString("promtype")
                ViewState.Add("approve", Request.QueryString("approve"))
            Else
                ViewState.Add("approve", String.Empty)
            End If

            If Request.QueryString("roomtype") <> "" Then
                'promtype = Request.QueryString("promtype")
                ViewState.Add("roomtype", Request.QueryString("roomtype"))
            Else
                ViewState.Add("roomtype", String.Empty)
            End If

            If Request.QueryString("partyfilter") <> "" Then
                'promtype = Request.QueryString("promtype")
                ViewState.Add("partyfilter", Request.QueryString("partyfilter"))
            Else
                ViewState.Add("partyfilter", String.Empty)
            End If

            If Request.QueryString("repfilter") <> "" Then
                'repfilter = Request.QueryString("repfilter")
                ViewState.Add("repfilter", Request.QueryString("repfilter"))
            Else
                ViewState.Add("repfilter", String.Empty)
            End If
            If Request.QueryString("reportoption") <> "" Then
                'reportoption = Request.QueryString("reportoption")
                ViewState.Add("reportoption", Request.QueryString("reportoption"))
            Else
                ViewState.Add("reportoption", String.Empty)
            End If
            If Request.QueryString("reporttitle") <> "" Then
                'reporttitle = Request.QueryString("reporttitle")
                ViewState.Add("reporttitle", Request.QueryString("reporttitle"))
            Else
                ViewState.Add("reporttitle", String.Empty)
            End If
            'If strReportName = "" Then
            '    'Response.Redirect(CType(Session("BackPageName"), String), False)
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
            '    Exit Sub
            'Else
            ViewState.Add("RepCalledFrom", 0)
            btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")
            BindReport()
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try

    End Sub
    '#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub
    '#End Region
    'End Sub
    Private Sub BindReport()
        Dim strReportTitle As String = ""

        'If Session("Rep") Is Nothing Then
        rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
        rptreportname = "Report - Supplier Policies"

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
        If ViewState("approve") = "1" Then


            rep.Load(Server.MapPath("~\Report\rptSupplierpoliciesAllnew.rpt"))
          

        ElseIf ViewState("approve") = "0" Then
            rep.Load(Server.MapPath("~\Report\rptSupplierpoliciesNotApprovednew.rpt"))

        Else
           
            rep.Load(Server.MapPath("~\Report\rptSupplierpoliciesnewAllReport.rpt"))

        End If




       

        'If ViewState("reportoption") = "1" Then
        '    rep.Load(Server.MapPath("~\Report\rptSupplierpoliciesAllnew.rpt"))
        'ElseIf ViewState("reportoption") = "2" Then
        '    rep.Load(Server.MapPath("~\Report\rptSupplierpolicies_cancellation.rpt"))
        'ElseIf ViewState("reportoption") = "3" Then
        '    rep.Load(Server.MapPath("~\Report\rptSupplierpolicies_childpolicy.rpt"))
        'ElseIf ViewState("reportoption") = "4" Or ViewState("reportoption") = "5" Or ViewState("reportoption") = "10" Then ' Promotion, Early Bird
        '    rep.Load(Server.MapPath("~\Report\rptSupplierpolicies_Promotion.rpt"))
        'ElseIf ViewState("reportoption") = "6" Then ' Compulsory Remarks
        '    rep.Load(Server.MapPath("~\Report\rptSupplierpolicies_compulsory.rpt"))
        'ElseIf ViewState("reportoption") = "7" Then ' General Policy
        '    rep.Load(Server.MapPath("~\Report\rptSupplierpolicies_general.rpt"))
        'ElseIf ViewState("reportoption") = "8" Then ' Block sales
        '    rep.Load(Server.MapPath("~\Report\rptSupplierpolicies_blocksale.rpt"))
        'ElseIf ViewState("reportoption") = "9" Then ' Minimum Nights
        '    rep.Load(Server.MapPath("~\Report\rptSupplierpolicies_minnights.rpt"))
        'ElseIf ViewState("reportoption") = "11" Then ' Special Events
        '    rep.Load(Server.MapPath("~\Report\rptSupplierpolicies_splevents.rpt"))
        'End If

        Me.CRVSupplierPolicies.ReportSource = rep
        Dim RepTbls As Tables = rep.Database.Tables

        For Each RepTbl As Table In RepTbls
            Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            RepTblLogonInfo.ConnectionInfo = ConnInfo
            RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        Next
        rep.SummaryInfo.ReportTitle = strReportTitle

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

        pname = pnames.Item("repfilter")
        paramvalue.Value = ViewState("repfilter")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@sptypecode")
        paramvalue.Value = ViewState("sptypecode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@ppartycode")
        paramvalue.Value = ViewState("partycode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("@frmdate")
        paramvalue.Value = ViewState("fromdate")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@todate")
        paramvalue.Value = ViewState("todate")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@plgrpcode")
        paramvalue.Value = ViewState("plgrpcode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("@citycode")
        paramvalue.Value = ViewState("citycode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@ctrycode")
        paramvalue.Value = ViewState("ctrycode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        If ViewState("catcode") <> "" Then
            pname = pnames.Item("catname")
            paramvalue.Value = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select catname from catmast where catcode='" + ViewState("catcode") + "'"), String)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
        Else
            pname = pnames.Item("catname")
            paramvalue.Value = ""
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        End If

        'pname = pnames.Item("catname")
        'paramvalue.Value = ViewState("catcode")
        'param = pname.CurrentValues
        'param.Add(paramvalue)
        'pname.ApplyCurrentValues(param)

        pname = pnames.Item("@catcode")
        paramvalue.Value = ViewState("catcode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@scatcode")
        paramvalue.Value = ViewState("scatcode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@rmtypcode")
        paramvalue.Value = ViewState("roomtype")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@approve")
        paramvalue.Value = ViewState("approve")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        
        pname = pnames.Item("reportoption")
        paramvalue.Value = ViewState("reportoption")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@pfilter")
        paramvalue.Value = ViewState("partyfilter")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        'If ViewState("reportoption") = "1" Or ViewState("reportoption") = "4" Or ViewState("reportoption") = "5" Or ViewState("reportoption") = "10" Then  ' All Policies, promotion, Earlybird
        '    pname = pnames.Item("@promtype")
        '    paramvalue.Value = ViewState("promtype")
        '    param = pname.CurrentValues
        '    param.Add(paramvalue)
        '    pname.ApplyCurrentValues(param)
        'End If

        Me.CRVSupplierPolicies.ReportSource = rep
        Session.Add("ReportSource", rep)
        Me.CRVSupplierPolicies.DataBind()
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If ViewState("RepCalledFrom") <> 1 Then
            rep.Close()
            rep.Dispose()
            ' Session("ColReportParams") = Nothing
        End If
    End Sub

    
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click

        'Response.Redirect("rptSupplierpoliciesSearch.aspx?sptypecode=" & sptypecode & "&partycode=" & partycode _
        '& "&citycode=" & citycode & "&ctrycode=" & ctrycode & "&asondate=" & asondate & "&fromdate=" & fromdate & "&todate=" & todate _
        '& "&catcode=" & catcode & "&plgrpcode=" & plgrpcode & "&promtype=" & promtype & "&scatcode=" & scatcode & "&repfilter=" _
        '& repfilter & "&reportoption=" & reportoption & "&reporttitle=" & reporttitle, False)

        'If CType(ViewState("BackPageName"), String) = "" Then
        '    'Response.Redirect("MainPage.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        '    Exit Sub
        'Else
        '    'Session("ColReportParams") = Nothing
        '    'Response.Redirect(CType(Session("BackPageName"), String), False)
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        'End If
    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Session.Add("ReportSource", rep)
    '    Dim strpop As String = ""
    '    strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    'End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Session.Add("ReportSource", rep)
        ViewState.Add("RepCalledFrom", 1)
        Dim strpop As String = ""
        ' strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
End Class
