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
    Dim meal As String, selltype As String, except As String, roomtype As String
    Dim repfilter As String, reportoption As String, reporttitle As String, approve As String, showweb As String, rtptype As String


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState.Add("Pageame", Request.QueryString("Pageame"))
        ViewState.Add("BackPageName", Request.QueryString("BackPageName"))

        If Not Session("CompanyName") Is Nothing Then
            Me.Page.Title = CType(Session("CompanyName"), String)
        End If

        Try
            If Request.QueryString("excursions") <> "" Then
                'sptypecode = Request.QueryString("sptypecode")
                ViewState.Add("excursions", Trim(Session("mulselno")))
            Else
                ViewState.Add("excursions", String.Empty)
            End If

            If Request.QueryString("mysellcode") <> "" Then
                'sptypecode = Request.QueryString("sptypecode")
                ViewState.Add("sellcode", Request.QueryString("sellcode"))
            Else
                ViewState.Add("sellcode", String.Empty)
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


            reportoption = ViewState("reportoption")

            Select Request.QueryString("reporttype")
                Case 0
                    Select Case Request.QueryString("chkstatus")
                        Case 0
                            If Request.QueryString("trfreq") = 1 Then
                                rep.Load(Server.MapPath("~\Report\ExcReglst_det_trfreq.rpt"))
                            Else
                                rep.Load(Server.MapPath("~\Report\ExcReglst_detnew.rpt"))
                            End If

                        Case 1
                            rep.Load(Server.MapPath("~\Report\ExcReglst_detnew.rpt"))
                        Case 2
                            rep.Load(Server.MapPath("~\Report\excreglst_detcostnew.rpt"))
                        Case 4
                            rep.Load(Server.MapPath("~\Report\excreglst_detcommcostnew.rpt"))
                    End Select
                Case 1
                    rep.Load(Server.MapPath("~\Report\excreglst_detnormal_worate.rpt"))
                Case 2
                    rep.Load(Server.MapPath("~\Report\excreglst_detcommcost_unlink.rpt"))
                Case 3
                    rep.Load(Server.MapPath("~\Report\Excsafari_detnew.rpt"))
            End Select



            Me.CRVPricelist.ReportSource = rep
            Dim RepTbls As Tables = rep.Database.Tables

            For Each RepTbl As Table In RepTbls
                Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
                RepTblLogonInfo.ConnectionInfo = ConnInfo
                RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
            Next

            rep.SummaryInfo.ReportTitle = strReportTitle

            pnames = rep.DataDefinition.ParameterFields


            pname = pnames.Item("Conm")
            paramvalue.Value = rptcompanyname
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("rephead")
            paramvalue.Value = ViewState("reporttitle")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("repfilter")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = ViewState("repfilter")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pexcfrm")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("excproviderfrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pexcto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("excproviderto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("photfrm")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("hotelcodefrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("photto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("hotelcodefrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pddtfrm")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Format(CType(Request.QueryString("reqfrom"), Date), "yyyy-MM-dd")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pddtto")
            paramvalue.Value = Format(CType(Request.QueryString("reqto"), Date), "yyyy-MM-dd")
            'paramvalue.Value = fromdate
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pspfrm")
            paramvalue.Value = Request.QueryString("spfrom")
            'paramvalue.Value = todate
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pspto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("spto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("dateyn")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("dateyn")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("ppayfrn")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("paytermsfrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("ppayto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("paytermsto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("ptourdtfrm")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Format(CType(Request.QueryString("tourfrom"), Date), "yyyy-MM-dd")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("ptourdtto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Format(CType(Request.QueryString("tourto"), Date), "yyyy-MM-dd") '"date(" & Mid(DTPfrom1.value, 7, 4) & "," & Mid(DTPfrom1.value, 4, 2) & "," & Mid(DTPfrom1.value, 1, 2) & ")" 'Format(CType(Request.QueryString("tourto"), Date), "yyyy/MM/dd")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("date")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("date")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("gmode")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("strgroupby")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("mbasecurr")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = ""
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pmktfrm")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("marketcodefrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pmktto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("marketcodeto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcntfrm")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("clientcodefrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcntto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("clientcodeto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("opt")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = IIf(Request.QueryString("opt") <> "", 1, 0)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("excursion")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = IIf(Request.QueryString("opt") <> "", Trim(Session("mulselno")), ViewState("excursions"))
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("prepaid")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = 0
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcltfrm")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("collectfrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcltto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("collectto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pexcpfrm")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("excproviderfrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pexcpto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("excproviderto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("lang")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("language")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcityfrm")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("citycodefrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("pcityto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("citycodeto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("excgroupfrom")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("excgpcodefrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("excgroupto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("excgpcodeto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("partyfrom")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("spfrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("partyto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("spto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("mainfrom")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("maingpcodefrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("mainto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("maingpcodeto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("dmc")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("dmc")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("driverfrom")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("driverfrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("driverto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("driverto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("officefrom")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("salesfrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("officeto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("salesto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("spersonfrom")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("sapcodefrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("spersonto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("sapcodeto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)



            pname = pnames.Item("operatorfrom")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("operatorcodefrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)



            pname = pnames.Item("operatorto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("operatorcodeto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("tourguidefrom")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("tourguidefrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)



            pname = pnames.Item("tourguideto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("tourguideto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("othsellcodefrom")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("selltypecodefrom")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)



            pname = pnames.Item("othsellcodeto")
            'paramvalue.Value = Session("repfilter")
            paramvalue.Value = Request.QueryString("selltypecodeto")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)



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
        strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
End Class
