Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptPricelistReport
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim sptypecode As String, partycode As String, citycode As String, ctrycode As String
    Dim fromdate As String, todate As String, catcode As String, sellcode As String, plgrpcode As String, asondate As String
    Dim meal As String, selltype As String, except As String, roomtype As String, agentcode As String, promocountry As String
    Dim repfilter As String, reportoption As String, reporttitle As String, approve As String, showweb As String, rtptype As String


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
            If Request.QueryString("partycode") <> "" Then
                'partycode = Request.QueryString("partycode")
                ViewState.Add("partycode", Request.QueryString("partycode"))
            Else
                ViewState.Add("partycode", String.Empty)
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
            If Request.QueryString("plgrpcode") <> "" Then
                'plgrpcode = Request.QueryString("plgrpcode")
                ViewState.Add("plgrpcode", Request.QueryString("plgrpcode"))
            Else
                ViewState.Add("plgrpcode", String.Empty)
            End If
            If Request.QueryString("sellcode") <> "" Then
                'sellcode = Request.QueryString("sellcode")
                ViewState.Add("sellcode", Request.QueryString("sellcode"))
            Else
                ViewState.Add("sellcode", String.Empty)
            End If
            If Request.QueryString("citycode") <> "" Then
                'citycode = Request.QueryString("citycode")
                ViewState.Add("citycode", Request.QueryString("citycode"))
            Else
                ViewState.Add("citycode", String.Empty)
            End If
            If Request.QueryString("ctrycode") <> "" Then
                'ctrycode = Request.QueryString("ctrycode")
                ViewState.Add("ctrycode", Request.QueryString("ctrycode"))
            Else
                ViewState.Add("ctrycode", String.Empty)
            End If
            If Request.QueryString("catcode") <> "" Then
                'catcode = Request.QueryString("catcode")
                ViewState.Add("catcode", Request.QueryString("catcode"))
            Else
                ViewState.Add("catcode", String.Empty)
            End If
            If Request.QueryString("meal") <> "" Then
                'meal = Request.QueryString("meal")
                ViewState.Add("meal", Request.QueryString("meal"))
            Else
                ViewState.Add("meal", String.Empty)
            End If
            If Request.QueryString("selltype") <> "" Then
                'selltype = Request.QueryString("selltype")
                ViewState.Add("selltype", Request.QueryString("selltype"))
            Else
                ViewState.Add("selltype", Request.QueryString("meal"))
            End If
            If Request.QueryString("except") <> "" Then
                'except = Request.QueryString("except")
                ViewState.Add("except", Request.QueryString("except"))
            Else
                ViewState.Add("except", String.Empty)
            End If
            If Request.QueryString("roomtype") <> "" Then
                'roomtype = Request.QueryString("roomtype")
                ViewState.Add("roomtype", Request.QueryString("roomtype"))
            Else
                ViewState.Add("roomtype", String.Empty)
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

            If Request.QueryString("subscode") <> "" Then
                ViewState.Add("subseascode", Request.QueryString("subscode"))
            Else
                ViewState.Add("subseascode", String.Empty)
            End If
            If Request.QueryString("convtbase") <> "" Then
                ViewState.Add("convtobase", Request.QueryString("convtbase"))
            Else
                ViewState.Add("convtobase", 0)
            End If
            If Request.QueryString("crate") <> "" Then
                ViewState.Add("convrate", Request.QueryString("crate"))
            Else
                ViewState.Add("convrate", 0)
            End If

            If Request.QueryString("approve") <> "" Then
                ViewState.Add("approve", Request.QueryString("approve"))
            Else
                ViewState.Add("approve", 2)
            End If

            If Request.QueryString("showweb") <> "" Then
                ViewState.Add("showweb", Request.QueryString("showweb"))
            Else
                ViewState.Add("showweb", 2)
            End If

            If Request.QueryString("rpttype") <> "" Then
                ViewState.Add("rpttype", Request.QueryString("rpttype"))
            Else
                ViewState.Add("rpttype", 0)
            End If

            If Request.QueryString("sellcatcode") <> "" Then
                ViewState.Add("sellcatcode", Request.QueryString("sellcatcode"))
            Else
                ViewState.Add("sellcatcode", String.Empty)
            End If

            If Request.QueryString("pfilter") <> "" Then
                ViewState.Add("pfilter", Request.QueryString("pfilter"))
            Else
                ViewState.Add("pfilter", 0)
            End If

            If Request.QueryString("hotelcost") <> "" Then
                ViewState.Add("hotelcost", Request.QueryString("hotelcost"))
            Else
                ViewState.Add("hotelcost", 0)
            End If
            If Request.QueryString("agentcode") <> "" Then
                ViewState.Add("agentcode", Request.QueryString("agentcode"))
            Else
                ViewState.Add("agentcode", String.Empty)
            End If

            If Request.QueryString("promocountry") <> "" Then
                ViewState.Add("promocountry", Request.QueryString("promocountry"))
            Else
                ViewState.Add("promocountry", String.Empty)
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
        Try
            Dim strReportTitle As String = ""
            Dim reportoption As String = ""
            Dim sellname As String = ""
            Dim commyn As String = 0
            'If Session("Rep") Is Nothing Then
            rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
            If Trim(ViewState("sellcode")) = "" Then
                'rptreportname = "Report - Price List - Cost Price"
                If ViewState("hotelcost") = 0 Then
                    rptreportname = "NET PAYABLE"
                Else
                    rptreportname = "HOTEL COST"
                End If

            Else
                sellname = objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select sellname from sellmast where sellcode ='" + Trim(ViewState("sellcode")) + "'")
                commyn = objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select commyn from sellmast where sellcode ='" + Trim(ViewState("sellcode")) + "'")
                ' rptreportname = "Report - Price List -" + sellname
                rptreportname = sellname
            End If

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

            'reportoption = ViewState("reportoption")
            'rep.Load(Server.MapPath("~\Report\rptpricelist_Landscap.rpt"))

            If ViewState("approve") = "1" Then

                If ViewState("rpttype") = 1 Then
                    'rep.Load(Server.MapPath("~\Report\rptpricelist.rpt"))
                    'Else
                    rep.Load(Server.MapPath("~\Report\rptpricelist_Landscap.rpt"))
                End If

            ElseIf ViewState("approve") = "0" Then
                If ViewState("rpttype") = 1 Then
                    'rep.Load(Server.MapPath("~\Report\rptpricelistnotapproved.rpt"))
                    'Else
                    rep.Load(Server.MapPath("~\Report\rptpricelistnotapproved_Landscap.rpt"))
                End If

            Else
                'If ViewState("rpttype") = 0 Then
                'rep.Load(Server.MapPath("~\Report\rptpricelistAll.rpt"))
                'Else
                rep.Load(Server.MapPath("~\Report\rptpricelistAll_Landscap.rpt"))
                'End If

            End If

            Me.CRVPricelist.ReportSource = rep
            Dim RepTbls As Tables = rep.Database.Tables

            For Each RepTbl As Table In RepTbls
                Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
                RepTblLogonInfo.ConnectionInfo = ConnInfo
                RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
            Next

            rep.SummaryInfo.ReportTitle = strReportTitle

            pnames = rep.DataDefinition.ParameterFields


            'pname = pnames.Item("@sptypecode")
            'paramvalue.Value = sptypecode
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@ppartycode")
            'paramvalue.Value = partycode
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@frmdate")
            'paramvalue.Value = fromdate
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@todate")
            'paramvalue.Value = todate
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)


            'pname = pnames.Item("@plgrpcode")
            'paramvalue.Value = plgrpcode
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@sellcode")
            'paramvalue.Value = sellcode
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@mealprn")
            'paramvalue.Value = meal
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@citycode")
            'paramvalue.Value = citycode
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@ctrycode")
            'paramvalue.Value = ctrycode
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@catcode")
            'paramvalue.Value = catcode
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@subseascode")
            'paramvalue.Value = "" 'Session("subseascode")
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@except")
            'paramvalue.Value = except
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@rmtypcode")
            'paramvalue.Value = roomtype
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@selltype")
            'paramvalue.Value = selltype
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@promtype")
            'paramvalue.Value = "0"
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)


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

            pname = pnames.Item("marketname")
            paramvalue.Value = ViewState("plgrpcode")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            'pname = pnames.Item("agentname")
            'paramvalue.Value = ViewState("agentcode")
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            pname = pnames.Item("CtryName")
            paramvalue.Value = ViewState("ctrycode")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
            pname = pnames.Item("CityName")
            paramvalue.Value = ViewState("citycode")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


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


            If ViewState("partycode") <> "" Then
                pname = pnames.Item("partyname")
                paramvalue.Value = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select partyname from partymast where partycode='" + ViewState("partycode") + "'"), String)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            Else
                pname = pnames.Item("partyname")
                paramvalue.Value = ""
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

            End If

            pname = pnames.Item("repfilter")
            paramvalue.Value = "Updated as on : " + ViewState("asondate")
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

            pname = pnames.Item("@sellcode")
            paramvalue.Value = ViewState("sellcode")

            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@mealprn")
            paramvalue.Value = ViewState("meal")
            'paramvalue.Value = ""
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

            pname = pnames.Item("@catcode")
            paramvalue.Value = ViewState("catcode")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)





            pname = pnames.Item("@subseascode")
            paramvalue.Value = ViewState("subseascode") 'Session("subseascode")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@except")
            paramvalue.Value = ViewState("except")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@rmtypcode")
            paramvalue.Value = ViewState("roomtype")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@selltype")
            paramvalue.Value = ViewState("selltype")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@promtype")
            'paramvalue.Value = "0"
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            pname = pnames.Item("@convtobase")
            paramvalue.Value = ViewState("convtobase")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@convrate")
            paramvalue.Value = ViewState("convrate")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@approve")
            paramvalue.Value = ViewState("approve")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@showagent")
            paramvalue.Value = ViewState("showweb")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@commyn")
            paramvalue.Value = commyn
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@agcode")
            paramvalue.Value = ViewState("agentcode")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@rate")
            paramvalue.Value = 0
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@sellcatcode")  '17082011
            paramvalue.Value = ViewState("sellcatcode")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@pfilter")  '17082011
            paramvalue.Value = ViewState("pfilter")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@hotelcostprice")  '17082011
            paramvalue.Value = ViewState("hotelcost")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("@promocountry")
            paramvalue.Value = ViewState("promocountry")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            'pname = pnames.Item("@agentcode")
            'paramvalue.Value = ViewState("agentcode")
            'param = pname.CurrentValues
            'param.Add(paramvalue)
            'pname.ApplyCurrentValues(param)

            ''Added agentcode by Archana on 24/05/2015

            Me.CRVPricelist.ReportSource = rep
            'If strSelectionFormula <> "" Then
            '    CRVSupplierPolicies.SelectionFormula = strSelectionFormula
            'End If
            Session.Add("ReportSource", rep)
            Me.CRVPricelist.DataBind()
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If ViewState("RepCalledFrom") <> 1 Then
            rep.Close()
            rep.Dispose()
            ' Session("ColReportParams") = Nothing
        End If
    End Sub


    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        '        Session("sptypecode") = ""
        '        Session("ppartycode") = ""
        '        Session("fromdate") = ""
        '        Session("todate") = ""
        '        Session("plgrpcode") = ""
        '        Session("citycode") = ""
        '        Session("ctrycode") = ""
        '        Session("catcode") = ""
        '        Session("promtype") = ""
        '        Session("repfilter") = ""
        '        Session("reportoption") = ""
        '        Session("asondate") = ""
        '        Session("sellcode") = ""
        '        Session("meal") = ""
        '        Session("except") = ""
        '        Session("roomtype") = ""
        '        Session("selltype") = ""
        '        Session("ReportTitle") = ""
        '        'Response.Redirect("rptpricelistSearch.aspx", False)
        '        Response.Redirect("rptpricelistSearch.aspx?sptypecode=" & sptypecode & "&partycode=" & partycode _
        '& "&citycode=" & citycode & "&ctrycode=" & ctrycode & "&asondate=" & asondate & "&fromdate=" & fromdate & "&todate=" & todate _
        '& "&meal=" & meal & "&except=" & except & "&roomtype=" & roomtype _
        '& "&catcode=" & catcode & "&plgrpcode=" & plgrpcode & "&selltype=" & selltype & "&sellcode=" & sellcode & "&repfilter=" _
        '& repfilter & "&reportoption=" & reportoption & "&reporttitle=" & reporttitle, False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Session.Add("ReportSource", rep)
    '    Dim strpop As String = ""
    '    'strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
    '    strpop = "<script language=""javascript"">var win=window.open('RptPrintPage.aspx','printdoc','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", strpop, True)
    'End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Session.Add("ReportSource", rep)
        ViewState.Add("RepCalledFrom", 1)
        Dim strpop As String = ""
        'strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
End Class
