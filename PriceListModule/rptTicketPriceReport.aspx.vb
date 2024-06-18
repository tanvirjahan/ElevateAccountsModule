Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class rptTicketPriceReport
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim sptypecode As String, partycode As String, citycode As String, ctrycode As String, catcode As String
    Dim fromdate As String, todate As String, asondate As String, sellcode As String, plgrpcode As String
    Dim supagentcode As String, subseasoncode As String, flightcode As String, flightclasscode As String
    Dim repfilter As String, reportoption As String, reporttitle As String, approve As String



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

            If Request.QueryString("catcode") <> "" Then
                'catcode = Request.QueryString("catcode")
                ViewState.Add("catcode", Request.QueryString("catcode"))
            Else
                ViewState.Add("catcode", String.Empty)
            End If
            If Request.QueryString("sellcode") <> "" Then
                'sellcode = Request.QueryString("sellcode")
                ViewState.Add("sellcode", Request.QueryString("sellcode"))
            Else
                ViewState.Add("sellcode", String.Empty)
            End If

            If Request.QueryString("plgrpcode") <> "" Then
                'plgrpcode = Request.QueryString("plgrpcode")
                ViewState.Add("plgrpcode", Request.QueryString("plgrpcode"))
            Else
                ViewState.Add("plgrpcode", String.Empty)
            End If

            If Request.QueryString("supagentcode") <> "" Then
                'supagentcode = Request.QueryString("supagentcode")
                ViewState.Add("supagentcode", Request.QueryString("supagentcode"))
            Else
                ViewState.Add("supagentcode", String.Empty)
            End If
            If Request.QueryString("subseasoncode") <> "" Then
                'subseasoncode = Request.QueryString("subseasoncode")
                ViewState.Add("subseasoncode", Request.QueryString("subseasoncode"))
            Else
                ViewState.Add("subseasoncode", String.Empty)
            End If

            If Request.QueryString("flightcode") <> "" Then
                'flightcode = Request.QueryString("flightcode")
                ViewState.Add("flightcode", Request.QueryString("flightcode"))
            Else
                ViewState.Add("flightcode", String.Empty)
            End If

            If Request.QueryString("flightclasscode") <> "" Then
                'flightclasscode = Request.QueryString("flightclasscode")
                ViewState.Add("flightclasscode", Request.QueryString("flightclasscode"))
            Else
                ViewState.Add("flightclasscode", String.Empty)
            End If

            If Request.QueryString("approve") <> "" Then
                ViewState.Add("approve", Request.QueryString("approve"))
            Else
                ViewState.Add("approve", 2)
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
            'If Session("Rep") Is Nothing Then
            rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
            If ViewState("sellcode") = "" Then
                rptreportname = "Report - Ticketing Cost Price List"
            Else
                rptreportname = "Report - Ticketing Selling Price List"
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



            reportoption = ViewState("reportoption")
            rep.Load(Server.MapPath("~\Report\rptTicketPriceList.rpt"))

            Me.CRVTicketPricelist.ReportSource = rep
            Dim RepTbls As Tables = rep.Database.Tables

            For Each RepTbl As Table In RepTbls
                Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
                RepTblLogonInfo.ConnectionInfo = ConnInfo
                RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
            Next

            'rep.SummaryInfo.ReportTitle = strReportTitle
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
            paramvalue.Value = "Updated as on : " + ViewState("asondate")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@sptypecode")
            paramvalue.Value = ViewState("sptypecode")
            'paramvalue.Value = sptypecode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@supagentcode")
            paramvalue.Value = ViewState("supagentcode")
            'paramvalue.Value = supagentcode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@partycode")
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

            pname = pnames.Item("@sellcode")
            paramvalue.Value = ViewState("sellcode")
            'paramvalue.Value = sellcode
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

            pname = pnames.Item("@subseascode")
            paramvalue.Value = ViewState("subseasoncode")
            'paramvalue.Value = subseasoncode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@flightcode")
            paramvalue.Value = ViewState("flightcode")
            'paramvalue.Value = flightcode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@flightclscode")
            paramvalue.Value = ViewState("flightclasscode")
            'paramvalue.Value = flightclasscode
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@approve")
            paramvalue.Value = ViewState("approve")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)



            Me.CRVTicketPricelist.ReportSource = rep
            'If strSelectionFormula <> "" Then
            '    CRVSupplierPolicies.SelectionFormula = strSelectionFormula
            'End If
            Session.Add("ReportSource", rep)
            Me.CRVTicketPricelist.DataBind()


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
        '        Session("supagentcode") = ""
        '        Session("partycode") = ""
        '        Session("fromdate") = ""
        '        Session("todate") = ""
        '        Session("plgrpcode") = ""
        '        Session("citycode") = ""
        '        Session("ctrycode") = ""
        '        Session("catcode") = ""
        '        Session("sellcode") = ""
        '        Session("subseasoncode") = ""
        '        Session("flightcode") = ""
        '        Session("flightclasscode") = ""
        '        Session("repfilter") = ""
        '        Session("reportoption") = ""
        '        Session("asondate") = ""
        '        Session("ReportTitle") = ""
        '        'Response.Redirect("rptpricelistSearch.aspx", False)
        '        Response.Redirect("rptTicketPriceSearch.aspx?sptypecode=" & sptypecode & "&partycode=" & partycode _
        '& "&citycode=" & citycode & "&ctrycode=" & ctrycode & "&asondate=" & asondate & "&fromdate=" & fromdate & "&todate=" & todate _
        '& "&supagentcode=" & supagentcode & "&sellcode=" & sellcode & "&subseasoncode=" & subseasoncode _
        '& "&catcode=" & catcode & "&plgrpcode=" & plgrpcode & "&flightcode=" & flightcode & "&flightclasscode=" & flightclasscode & "&repfilter=" _
        '& repfilter & "&reportoption=" & reportoption & "&reporttitle=" & reporttitle, False)
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
        strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
End Class
