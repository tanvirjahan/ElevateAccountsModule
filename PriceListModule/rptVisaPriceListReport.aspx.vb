Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptVisaPriceListReport
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim strReportTitle As String = ""
    Dim reportoption As String = ""
    Dim othergroupcode As String
    Dim sptypecode As String, partycode As String, citycode As String, ctrycode As String
    Dim fromdate As String, todate As String, catcode As String, sellcode As String, plgrpcode As String, asondate As String
    Dim meal As String, selltype As String, except As String, roomtype As String, approve As String
    Dim repfilter As String, reporttitle As String



    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState.Add("Pageame", Request.QueryString("Pageame"))
        ViewState.Add("BackPageName", Request.QueryString("BackPageName"))

        If Not Session("CompanyName") Is Nothing Then
            Me.Page.Title = CType(Session("CompanyName"), String)
        End If

        Try
            If Request.QueryString("othergroupcode") <> "" Then
                'othergroupcode = Request.QueryString("othergroupcode")
                ViewState.Add("othergroupcode", Request.QueryString("othergroupcode"))
            Else
                ViewState.Add("othergroupcode", String.Empty)
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

            If Request.QueryString("sptypecode") <> "" Then
                'sptypecode = Request.QueryString("sptypecode")
                ViewState.Add("sptypecode", Request.QueryString("sptypecode"))
            Else
                ViewState.Add("sptypecode", String.Empty)
            End If
            If Request.QueryString("partycode") <> "" Then
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
            If Request.QueryString("catcode") <> "" Then
                'catcode = Request.QueryString("catcode")
                ViewState.Add("catcode", Request.QueryString("catcode"))
            Else
                ViewState.Add("catcode", String.Empty)
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
            If Request.QueryString("othcat") <> "" Then
                'reporttitle = Request.QueryString("reporttitle")
                ViewState.Add("othcat", Server.UrlDecode(Request.QueryString("othcat")))
            Else
                ViewState.Add("othcat", String.Empty)
            End If

            If Request.QueryString("approve") <> "" Then
                ViewState.Add("approve", Request.QueryString("approve"))
            Else
                ViewState.Add("approve", 2)
            End If

            If Request.QueryString("rtptype") <> "" Then
                ViewState.Add("rtptype", Request.QueryString("rtptype"))
            Else
                ViewState.Add("rtptype", String.Empty)
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

        'Try
        '    BindReport()
        'Catch ex As Exception
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        'End Try
    End Sub
    '#End Region
    'End Sub
    Private Sub BindReport()
        'If Session("Rep") Is Nothing Then
        rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)


        Dim pnames As ParameterFieldDefinitions
        Dim pname As ParameterFieldDefinition
        Dim param As New ParameterValues
        Dim paramvalue As New ParameterDiscreteValue
        Dim ConnInfo As New ConnectionInfo
        Dim strtitle As String = ""
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

        rep.Load(Server.MapPath("~\Report\rptpricelistreport.rpt"))
        strtitle = "Visa Price List"
        rptreportname = "Report -" + strtitle + " Price List - Sales Sheet"
        Me.CRVPricelist.ReportSource = rep
        Dim RepTbls As Tables = rep.Database.Tables

        For Each RepTbl As Table In RepTbls
            Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            RepTblLogonInfo.ConnectionInfo = ConnInfo
            RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        Next

        rep.SummaryInfo.ReportTitle = strReportTitle

        pnames = rep.DataDefinition.ParameterFields


        'If Session("reportoption") = "" Then
        If ViewState("rtptype") = "Sales" Then
            Dim othcat As String = CType(ViewState("othcat"), String)
            Dim strothcat() As String
            strothcat = othcat.Split("$")


            pname = pnames.Item("@grpcode")
            paramvalue.Value = ViewState("othergroupcode")
            'paramvalue.Value = othergroupcode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@frmdate")
            paramvalue.Value = ViewState("fromdate")
            'paramvalue.Value = fromdate
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@todate")
            paramvalue.Value = ViewState("todate")
            'paramvalue.Value = todate
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@plgrpcode")
            paramvalue.Value = ViewState("plgrpcode")
            'paramvalue.Value = plgrpcode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@sellcode")
            paramvalue.Value = ViewState("sellcode")
            'paramvalue.Value = sellcode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("@othcatcode1")
            If strothcat.Length > 0 Then
                paramvalue.Value = strothcat(0)
            Else
                paramvalue.Value = ""
            End If
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@othcatcode2")
            If strothcat.Length > 1 Then
                paramvalue.Value = strothcat(1)
            Else
                paramvalue.Value = ""
            End If
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@othcatcode3")
            If strothcat.Length > 2 Then
                paramvalue.Value = strothcat(2)
            Else
                paramvalue.Value = ""
            End If
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@othcatcode4")
            If strothcat.Length > 3 Then
                paramvalue.Value = strothcat(3)
            Else
                paramvalue.Value = ""
            End If
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@othcatcode5")
            If strothcat.Length > 4 Then
                paramvalue.Value = strothcat(4)
            Else
                paramvalue.Value = ""
            End If
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@othcatcode6")
            If strothcat.Length > 5 Then
                paramvalue.Value = strothcat(5)
            Else
                paramvalue.Value = ""
            End If
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@othcatcode7")
            If strothcat.Length > 6 Then
                paramvalue.Value = strothcat(6)
            Else
                paramvalue.Value = ""
            End If
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@othcatcode8")
            If strothcat.Length > 7 Then
                paramvalue.Value = strothcat(7)
            Else
                paramvalue.Value = ""
            End If
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@othcatcode9")
            If strothcat.Length > 8 Then
                paramvalue.Value = strothcat(8)
            Else
                paramvalue.Value = ""
            End If
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@othcatcode10")
            If strothcat.Length > 9 Then
                paramvalue.Value = strothcat(9)
            Else
                paramvalue.Value = ""
            End If
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("@othcatcode11")
            If strothcat.Length > 10 Then
                paramvalue.Value = strothcat(10)
            Else
                paramvalue.Value = ""
            End If
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@othcatcode12")
            If strothcat.Length > 11 Then
                paramvalue.Value = strothcat(11)
            Else
                paramvalue.Value = ""
            End If
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("@approve")
            paramvalue.Value = ViewState("approve")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


        Else
            pname = pnames.Item("@grpcode")
            paramvalue.Value = ViewState("othergroupcode")
            'paramvalue.Value = othergroupcode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@sptypecode")
            paramvalue.Value = ViewState("sptypecode")
            'paramvalue.Value = sptypecode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@ppartycode")
            paramvalue.Value = ViewState("partycode")
            'paramvalue.Value = partycode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@frmdate")
            paramvalue.Value = ViewState("fromdate")
            'paramvalue.Value = fromdate
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@todate")
            paramvalue.Value = ViewState("todate")
            'paramvalue.Value = todate
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@plgrpcode")
            paramvalue.Value = ViewState("plgrpcode")
            'paramvalue.Value = plgrpcode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@citycode")
            paramvalue.Value = ViewState("citycode")
            'paramvalue.Value = citycode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@ctrycode")
            paramvalue.Value = ViewState("ctrycode")
            'paramvalue.Value = ctrycode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@catcode")
            paramvalue.Value = ViewState("catcode")
            'paramvalue.Value = catcode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@approve")
            paramvalue.Value = ViewState("approve")
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


        pname = pnames.Item("repfilter")
        paramvalue.Value = "" '"Updated as on : " + Session("asondate")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        Me.CRVPricelist.ReportSource = rep
        'If strSelectionFormula <> "" Then
        '    CRVSupplierPolicies.SelectionFormula = strSelectionFormula
        'End If
        Session.Add("ReportSource", rep)
        Me.CRVPricelist.DataBind()
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If ViewState("RepCalledFrom") <> 1 Then
            rep.Close()
            rep.Dispose()
            ' Session("ColReportParams") = Nothing
        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click

        'Session("grpcode") = ""
        'Session("fromdate") = ""
        'Session("todate") = ""
        'Session("plgrpcode") = ""
        'Session("sellcode") = ""

        'Session("sptypecode") = ""
        'Session("ppartycode") = ""
        'Session("citycode") = ""
        'Session("ctrycode") = ""
        'Session("catcode") = ""

        'Session("repfilter") = ""
        'Session("reportoption") = ""

        'Session("ReportTitle") = ""
        'If reportoption = "" Then
        '    'Response.Redirect("rptOtherServicePriceList.aspx", False)
        '    Response.Redirect("rptOtherServicePriceList.aspx?othergroupcode=" & othergroupcode & "&fromdate=" & fromdate _
        '    & "&todate=" & todate & "&plgrpcode=" & plgrpcode & "&sellcode=" & sellcode & "&repfilter=" _
        '    & repfilter & "&reportoption=" & reportoption & "&reporttitle=" & reporttitle, False)
        'Else
        '    'Response.Redirect("rptOtherServiceCost.aspx", False)
        '    Response.Redirect("rptOtherServiceCost.aspx?othergroupcode=" & othergroupcode & "&fromdate=" & fromdate _
        '    & "&todate=" & todate & "&plgrpcode=" & plgrpcode & "&catcode=" & catcode & "&sptypecode=" & sptypecode _
        '    & "&partycode=" & partycode & "&ctrycode=" & ctrycode & "&citycode=" & citycode & "&repfilter=" _
        '    & repfilter & "&reportoption=" & reportoption & "&reporttitle=" & reporttitle, False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
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
        'strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
End Class
