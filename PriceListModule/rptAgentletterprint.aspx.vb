Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
#End Region

Partial Class rptAgentletterprint
    Inherits System.Web.UI.Page
    Dim repDeocument As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim skip As String, sptypecode As String, partycodef As String, partycodet As String
    Dim citycodef As String, citycodet As String, asondate As String, plgrpcodef As String, plgrpcodet As String
    Dim repfilter As String, reportoption As String, reporttitle As String, rpttype As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState.Add("Pageame", Request.QueryString("Pageame"))
        ViewState.Add("BackPageName", Request.QueryString("BackPageName"))

        If Not Session("CompanyName") Is Nothing Then
            Me.Page.Title = CType(Session("CompanyName"), String)
        End If

        Try
            If Request.QueryString("lettercode") <> "" Then
                'sptypecode = Request.QueryString("sptypecode")
                ViewState.Add("lettercode", Request.QueryString("lettercode"))
            Else
                ViewState.Add("lettercode", String.Empty)
            End If
            If Request.QueryString("agentcode") <> "" Then
                ViewState.Add("agentcode", Request.QueryString("agentcode"))
            Else
                ViewState.Add("agentcode", String.Empty)
            End If
            If Request.QueryString("sno") <> "" Then
                ViewState.Add("sno", Request.QueryString("sno"))
            Else
                ViewState.Add("sno", String.Empty)
            End If

            If Request.QueryString("repfilter") <> "" Then
                'partycodef = Request.QueryString("partycodef")
                ViewState.Add("repfilter", Request.QueryString("repfilter"))
            Else
                ViewState.Add("repfilter", String.Empty)
            End If

            If Request.QueryString("rpttype") <> "" Then
                rpttype = Request.QueryString("rpttype")
            End If

            ViewState.Add("RepCalledFrom", 0)
            BindReport(ViewState("lettercode"))
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
    Private Sub BindReport(ByVal strSelectionFormula As String)
        Try
            Dim strReportTitle As String = ""
            Dim reportoption As String = ""
            'If Session("Rep") Is Nothing Then
            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
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

            If rpttype = "0" Then
                repDeocument.Load(Server.MapPath("~\Report\rptletterprint.rpt"))
            Else
                repDeocument.Load(Server.MapPath("~\Report\rptagentletterprint.rpt"))
            End If

            Me.CRVPLprint.ReportSource = repDeocument
            Dim RepTbls As Tables = repDeocument.Database.Tables

            For Each RepTbl As Table In RepTbls
                Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
                RepTblLogonInfo.ConnectionInfo = ConnInfo
                RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
            Next

            'rep.SummaryInfo.ReportTitle = ViewState("reporttitle")

            pnames = repDeocument.DataDefinition.ParameterFields

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
            paramvalue.Value = " Letter Option " + Trim(ViewState("lettercode"))
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("lettercode")
            paramvalue.Value = ViewState("lettercode")
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            If rpttype = "1" Then
                pname = pnames.Item("agentcode")
                paramvalue.Value = ViewState("agentcode")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

                pname = pnames.Item("sno")
                paramvalue.Value = ViewState("sno")
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

            End If

            strSelectionFormula = ViewState("lettercode")

            Me.CRVPLprint.ReportSource = repDeocument
            Response.Buffer = False
            Response.ClearContent()
            Response.ClearHeaders()
            repDeocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, False, "Report")

        Catch ex As Exception

        Finally
            repDeocument.Dispose()
            GC.Collect()
        End Try

    End Sub


End Class
