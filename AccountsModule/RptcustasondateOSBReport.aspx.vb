Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class RptcustasondateOSBReport
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim fromdate As String, todate As String, movflg As String
    Dim accttype, gpby, frommarkcode, tomarkcode, fromctry, toctry, fromcity, tocity, fromcode, tocode, fromsptype, fromtosptype, fromcat, tocat, fromglcode, toglcode, currtype, orderby, includezero, withCredit As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If

            If Request.QueryString("todate") <> "" Then
                todate = Trim(Request.QueryString("todate"))
            End If


            If Request.QueryString("frommarkcode") <> "" Then
                frommarkcode = Trim(Request.QueryString("frommarkcode"))
            End If
            If Request.QueryString("tomarkcode") <> "" Then
                tomarkcode = Trim(Request.QueryString("tomarkcode"))
            End If

            If Request.QueryString("fromcode") <> "" Then
                fromcode = Trim(Request.QueryString("fromcode"))
            End If
            If Request.QueryString("tocode") <> "" Then
                tocode = Trim(Request.QueryString("tocode"))
            End If

            If Request.QueryString("fromcat") <> "" Then
                fromcat = Trim(Request.QueryString("fromcat"))
            End If

            If Request.QueryString("tocat") <> "" Then
                tocat = Trim(Request.QueryString("tocat"))
            End If
            If Request.QueryString("fromglcode") <> "" Then
                fromglcode = Trim(Request.QueryString("fromglcode"))
            End If
            If Request.QueryString("toglcode") <> "" Then
                toglcode = Trim(Request.QueryString("toglcode"))
            End If
            If Request.QueryString("currtype") <> "" Then
                currtype = Trim(Request.QueryString("currtype"))
            End If
            If Request.QueryString("orderby") <> "" Then
                orderby = Trim(Request.QueryString("orderby"))
            End If
            If Request.QueryString("includezero") <> "" Then
                includezero = Trim(Request.QueryString("includezero"))
            End If
            If Request.QueryString("gpby") <> "" Then
                gpby = Trim(Request.QueryString("gpby"))
            End If
            If Request.QueryString("withCredit") <> "" Then
                withCredit = Trim(Request.QueryString("withCredit"))
            End If


            ViewState.Add("RepCalledFrom", 0)
            btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")
            BindReport()
            btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")
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

    Private Sub BindReport()
        Dim strReportTitle As String = ""
        Dim reportoption As String = ""
        rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
        'rptreportname = "Arrival Report"

        Dim pnames As ParameterFieldDefinitions
        Dim pname As ParameterFieldDefinition
        Dim param As New ParameterValues
        Dim paramvalue As New ParameterDiscreteValue
        Dim ConnInfo As New ConnectionInfo
        Dim reportfilter As String
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

        rep.Load(Server.MapPath("..\Report\rptcustOutstanding_percentage.rpt"))

        Me.CRVReport.ReportSource = rep
        Dim RepTbls As Tables = rep.Database.Tables

        For Each RepTbl As Table In RepTbls
            Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            RepTblLogonInfo.ConnectionInfo = ConnInfo
            RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        Next

        rptreportname = "Customer Outstanding Report - As On date"
        rep.SummaryInfo.ReportTitle = rptreportname

        pnames = rep.DataDefinition.ParameterFields

        reportfilter = ""

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

        pname = pnames.Item("gpby")
        paramvalue.Value = gpby
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("@todate")
        paramvalue.Value = Trim(todate)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        reportfilter = "As on Date " + Format(CType(todate, Date), "dd/MM/yyyy") + " "


        pname = pnames.Item("@fromcode")
        paramvalue.Value = Trim(fromcode)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@tocode")
        paramvalue.Value = Trim(tocode)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        reportfilter = IIf((fromcode <> "" And tocode <> ""), " Customer Code from " + fromcode + " to " + tocode, "")

        pname = pnames.Item("@frommarkcode")
        paramvalue.Value = Trim(frommarkcode)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@tomarkcode")
        paramvalue.Value = Trim(tomarkcode)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        reportfilter = reportfilter + IIf((frommarkcode <> "" And tomarkcode <> ""), " Market from " + frommarkcode + " to " + tomarkcode, "")

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
        reportfilter = reportfilter + IIf((fromctry <> "" And toctry <> ""), " Country  from " + fromctry + " to " + toctry, "")

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

        reportfilter = reportfilter + IIf((fromcat <> "" And tocat <> ""), " Category code from " + fromcat + " to " + tocat, "")


        pname = pnames.Item("@fromglcode")
        paramvalue.Value = Trim(fromglcode)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@toglcode")
        paramvalue.Value = Trim(toglcode)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)
        reportfilter = reportfilter + IIf((fromglcode <> "" And toglcode <> ""), " Control account code from " + fromglcode + " to " + toglcode, "")

        pname = pnames.Item("@currtype")
        paramvalue.Value = Trim(currtype)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@orderby")
        paramvalue.Value = Trim(orderby)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@includezero")
        paramvalue.Value = Trim(includezero)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)
        ' If Request.QueryString("type") = "C" Then

        pname = pnames.Item("@withCredit")
        paramvalue.Value = Trim(withCredit)
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)
        'End If

        pname = pnames.Item("reportfilter")
        paramvalue.Value = reportfilter
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        Session.Add("ReportSource", rep)
        '  Me.CRVReport.ReportSource = rep
        Me.CRVReport.DataBind()
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'If Page.IsPostBack = True Then
        '    rep.Close()
        '    rep.Dispose()
        'End If
        If ViewState("RepCalledFrom") <> 1 Then
            rep.Close()
            rep.Dispose()
            ' Session("ColReportParams") = Nothing
        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
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
