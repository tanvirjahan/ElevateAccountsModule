Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptPriceExpiryReport
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
            If Request.QueryString("sptypecode") <> "" Then
                'sptypecode = Request.QueryString("sptypecode")
                ViewState.Add("sptypecode", Request.QueryString("sptypecode"))
            Else
                ViewState.Add("sptypecode", String.Empty)
            End If
            If Request.QueryString("partycodef") <> "" Then
                'partycodef = Request.QueryString("partycodef")
                ViewState.Add("partycodef", Request.QueryString("partycodef"))
            Else
                ViewState.Add("partycodef", String.Empty)
            End If
            If Request.QueryString("plgrpcodef") <> "" Then
                'plgrpcodef = Request.QueryString("plgrpcodef")
                ViewState.Add("plgrpcodef", Request.QueryString("plgrpcodef"))
            Else
                ViewState.Add("plgrpcodef", String.Empty)
            End If
            If Request.QueryString("ctrycode") <> "" Then
                'citycodef = Request.QueryString("citycodef")
                ViewState.Add("ctrycode", Request.QueryString("ctrycode"))
            Else
                ViewState.Add("ctrycode", String.Empty)
            End If
            If Request.QueryString("citycodef") <> "" Then
                'citycodef = Request.QueryString("citycodef")
                ViewState.Add("citycodef", Request.QueryString("citycodef"))
            Else
                ViewState.Add("citycodef", String.Empty)
            End If
            If Request.QueryString("catcode") <> "" Then
                'citycodef = Request.QueryString("citycodef")
                ViewState.Add("catcode", Request.QueryString("catcode"))
            Else
                ViewState.Add("catcode", String.Empty)
            End If
            If Request.QueryString("sellcatcode") <> "" Then
                'citycodef = Request.QueryString("citycodef")
                ViewState.Add("sellcatcode", Request.QueryString("sellcatcode"))
            Else
                ViewState.Add("sellcatcode", String.Empty)
            End If
            If Request.QueryString("asondate") <> "" Then
                'asondate = Request.QueryString("asondate")
                ViewState.Add("asondate", Request.QueryString("asondate"))
            Else
                ViewState.Add("asondate", String.Empty)
            End If
            If Request.QueryString("approve") <> "" Then
                'repfilter = Request.QueryString("repfilter")
                ViewState.Add("approve", Request.QueryString("approve"))
            Else
                ViewState.Add("approve", String.Empty)
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
            If Request.QueryString("skip") <> "" Then
                'reporttitle = Request.QueryString("reporttitle")
                ViewState.Add("skip", Request.QueryString("skip"))
            Else
                ViewState.Add("skip", String.Empty)
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
        rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
        rptreportname = "Report - Price Expiry"

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
        If ViewState("skip") = "Y" Then
            rep.Load(Server.MapPath("~\Report\rptPriceExpiry_RTMPnew.rpt"))
        Else
            rep.Load(Server.MapPath("~\Report\rptPriceExpirynew.rpt"))
        End If


        Me.CRVPriceExpiry.ReportSource = rep
        Dim RepTbls As Tables = rep.Database.Tables

        For Each RepTbl As Table In RepTbls
            Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            RepTblLogonInfo.ConnectionInfo = ConnInfo
            RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        Next

        rep.SummaryInfo.ReportTitle = ViewState("reporttitle")

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
        'paramvalue.Value = "Updated as on : " + Session("reportoption")
        paramvalue.Value = "Updated as on : " + ViewState("reportoption")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@skiprmtypml")
        'paramvalue.Value = Session("skip")
        paramvalue.Value = ViewState("skip")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@sptypecode")
        'paramvalue.Value = Session("sptypecode")
        paramvalue.Value = ViewState("sptypecode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@plgrpcodeF")
        'paramvalue.Value = Session("plgrpcodef")
        paramvalue.Value = ViewState("plgrpcodef")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        'pname = pnames.Item("@plgrpcodeT")
        ''paramvalue.Value = Session("plgrpcodet")
        'paramvalue.Value = ViewState("plgrpcodet")
        'param = pname.CurrentValues
        'param.Add(paramvalue)
        'pname.ApplyCurrentValues(param)

        pname = pnames.Item("@ctrycode")
        'paramvalue.Value = Session("plgrpcodef")
        paramvalue.Value = ViewState("ctrycode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@citycodeF")
        'paramvalue.Value = Session("citycodef")
        paramvalue.Value = ViewState("citycodef")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        'pname = pnames.Item("@citycodeT")
        ''paramvalue.Value = Session("citycodet")
        'paramvalue.Value = ViewState("citycodet")
        'param = pname.CurrentValues
        'param.Add(paramvalue)
        'pname.ApplyCurrentValues(param)

        pname = pnames.Item("@catcode")
        paramvalue.Value = ViewState("catcode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@scatcode")
        paramvalue.Value = ViewState("sellcatcode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@partycodeF")
        'paramvalue.Value = Session("partycodef")
        paramvalue.Value = ViewState("partycodef")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        'pname = pnames.Item("@partycodeT")
        ''paramvalue.Value = Session("partycodet")
        'paramvalue.Value = ViewState("partycodet")
        'param = pname.CurrentValues
        'param.Add(paramvalue)
        'pname.ApplyCurrentValues(param)


        pname = pnames.Item("@lastdate")
        'paramvalue.Value = Session("asondate")
        paramvalue.Value = ViewState("asondate")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@approve")
        'paramvalue.Value = Session("asondate")
        paramvalue.Value = ViewState("approve")
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

        If ViewState("sellcatcode") <> "" Then
            pname = pnames.Item("scatname")
            paramvalue.Value = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select scatname from sellcatmast where scatcode='" + ViewState("sellcatcode") + "'"), String)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
        Else
            pname = pnames.Item("scatname")
            paramvalue.Value = ""
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
        End If

        If ViewState("partycodef") <> "" Then
            pname = pnames.Item("supplier")
            paramvalue.Value = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" + ViewState("partycodef") + "'"), String)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
        Else
            pname = pnames.Item("supplier")
            paramvalue.Value = ""
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

        End If

        Me.CRVPriceExpiry.ReportSource = rep
        'If strSelectionFormula <> "" Then
        '    CRVSupplierPolicies.SelectionFormula = strSelectionFormula
        'End If
        Session.Add("ReportSource", rep)
        Me.CRVPriceExpiry.DataBind()
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If ViewState("RepCalledFrom") <> 1 Then
            rep.Close()
            rep.Dispose()
            ' Session("ColReportParams") = Nothing
        End If
    End Sub




    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click

        'Session("skip") = ""
        'Session("sptypecode") = ""
        'Session("partycodef") = ""
        'Session("partycodet") = ""
        'Session("plgrpcodef") = ""
        'Session("plgrpcodet") = ""
        'Session("citycodef") = ""
        'Session("citycodet") = ""
        'Session("asondate") = ""

        'Session("repfilter") = ""
        'Session("reportoption") = ""
        'Session("ReportTitle") = ""
        ''        Response.Redirect("rptPriceExpirySearch.aspx", False)
        'Response.Redirect("rptPriceExpirySearch.aspx?skip=" & skip & "&sptypecode=" & sptypecode _
        '& "&partycodef=" & partycodef & "&partycodet=" & partycodet & "&plgrpcodef=" & plgrpcodef & "&plgrpcodet=" & plgrpcodet _
        '& "&citycodef=" & citycodef & "&citycodet=" & citycodet & "&asondate=" & asondate _
        '& "&repfilter=" & repfilter & "&reportoption=" & reportoption & "&reporttitle=" & reporttitle, False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
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
        'strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails';"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
End Class
