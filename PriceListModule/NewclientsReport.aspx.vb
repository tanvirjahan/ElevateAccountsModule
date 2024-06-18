Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class NewclientsReport
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils

    Dim catcode As String, citycode As String, ctrycode As String, sectorcode As String
    Dim fromdate As String, todate As String, orderby As String
    Dim repfilter As String, reportoption As String, reporttitle As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState.Add("Pageame", Request.QueryString("Pageame"))
        ViewState.Add("BackPageName", Request.QueryString("BackPageName"))

        If Not Session("CompanyName") Is Nothing Then
            Me.Page.Title = CType(Session("CompanyName"), String)
        End If

        Try

            If Request.QueryString("fromdate") <> "" Then
                repfilter = "From Date:" & Format("U", CType(Request.QueryString("fromdate"), Date))
                ViewState.Add("fromdate", Request.QueryString("fromdate"))
            Else
                ViewState.Add("fromdate", String.Empty)
            End If
            If Request.QueryString("todate") <> "" Then
                repfilter = repfilter & ";To Date:" & Format("U", CType(Request.QueryString("todate"), Date))
                ViewState.Add("todate", Request.QueryString("todate"))
            Else
                ViewState.Add("todate", String.Empty)
            End If
            If Request.QueryString("catcode") <> "" Then
                repfilter = repfilter & ",Category Code:" & Request.QueryString("catcode")
                ViewState.Add("catcode", Request.QueryString("catcode"))
            Else
                ViewState.Add("catcode", String.Empty)
            End If
            If Request.QueryString("ctrycode") <> "" Then
                repfilter = repfilter & ",Country:" & Request.QueryString("ctrycode")
                ViewState.Add("ctrycode", Request.QueryString("ctrycode"))
            Else
                ViewState.Add("ctrycode", String.Empty)
            End If
            If Request.QueryString("citycode") <> "" Then
                repfilter = repfilter & ",City:" & Request.QueryString("citycode")
                ViewState.Add("citycode", Request.QueryString("citycode"))
            Else
                ViewState.Add("citycode", String.Empty)
            End If
            If Request.QueryString("sectorcode") <> "" Then
                repfilter = repfilter & ",Sector Code:" & Request.QueryString("sectorcode")
                ViewState.Add("sectorcode", Request.QueryString("sectorcode"))
            Else
                ViewState.Add("sectorcode", String.Empty)
            End If

            If Request.QueryString("orderby") <> "" Then
                'orderby = Request.QueryString("orderby")
                ViewState.Add("orderby", Request.QueryString("orderby"))
            Else
                ViewState.Add("orderby", String.Empty)
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
        rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
        rptreportname = "Report - New Clients"

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
            .ServerName = Session("dbServerName")        'ConfigurationManager.AppSettings("dbServerName")
            .DatabaseName = Session("dbDatabaseName")    'ConfigurationManager.AppSettings("dbDatabaseName")
            .UserID = Session("dbUserName")              'ConfigurationManager.AppSettings("dbUserName")
            .Password = Session("dbPassword")            'ConfigurationManager.AppSettings("dbPassword")
        End With

        'reportoption = Session("reportoption")
        rep.Load(Server.MapPath("~\Report\rptnewclients.rpt"))



        Me.CRVNewClients.ReportSource = rep
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
        'paramvalue.Value = Session("repfilter")
        paramvalue.Value = repfilter
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("@catcode")
        'paramvalue.Value = Session("catcode")
        paramvalue.Value = ViewState("catcode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@ctrycode")
        'paramvalue.Value = Session("ctrytype")
        paramvalue.Value = ViewState("ctrycode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@citycode")
        'paramvalue.Value = Session("citycode")
        paramvalue.Value = ViewState("citycode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@sectorcode")
        'paramvalue.Value = Session("sectorcode")
        paramvalue.Value = ViewState("sectorcode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@frmdate")
        'paramvalue.Value = Session("fromdate")
        paramvalue.Value = ViewState("fromdate")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@todate")
        'paramvalue.Value = Session("todate")
        paramvalue.Value = ViewState("todate")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("order")
        'paramvalue.Value = Session("orderby")
        paramvalue.Value = ViewState("orderby")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        Me.CRVNewClients.ReportSource = rep
        'If strSelectionFormula <> "" Then
        '    CRVSupplierPolicies.SelectionFormula = strSelectionFormula
        'End If
        Me.CRVNewClients.DataBind()
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If ViewState("RepCalledFrom") <> 1 Then
            rep.Close()
            rep.Dispose()
            ' Session("ColReportParams") = Nothing
        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Session("fromdate") = ""
        'Session("todate") = ""
        'Session("catcode") = ""
        'Session("citycode") = ""
        'Session("ctrycode") = ""
        'Session("sectorcode") = ""

        'Session("repfilter") = ""
        'Session("reportoption") = ""
        'Session("ReportTitle") = ""
        ''Response.Redirect("NewclientsSearch.aspx", False)
        'Response.Redirect("NewclientsSearch.aspx?catcode=" & catcode & "&citycode=" & citycode _
        '& "&ctrycode=" & ctrycode & "&sectorcode=" & sectorcode & "&fromdate=" & fromdate & "&todate=" & todate _
        '& "&orderby=" & orderby & "&repfilter=" & repfilter & "&reportoption=" & reportoption & "&reporttitle=" & reporttitle, False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Session.Add("ReportSource", rep)
        ViewState.Add("RepCalledFrom", 1)
        Dim strpop As String = ""
        'strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
End Class
