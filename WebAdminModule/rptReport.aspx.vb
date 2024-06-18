Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

Partial Class rptReport
    Inherits System.Web.UI.Page
    Dim repDeocument As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
        Dim strReportName As String = ""
        If CType(Session("BackPageName"), String) = "" Then
            Response.Redirect("~/Login.aspx", False)
            Exit Sub
        Else
            If CType(Session("Pageame"), String) = "" Then
                Response.Redirect(CType(Session("BackPageName"), String), False)
                Exit Sub
            Else
                Select Case CType(Session("Pageame"), String)
                    Case "PreferredSupplier"
                        strReportName = CType(Server.MapPath("~\Report\rptPreferredSuppliers.rpt"), String)
                        rptreportname = "Report - Preferred Suppliers"
                        Exit Select
                    Case "ApproveCustomersforWeb"
                        strReportName = CType(Server.MapPath("~\Report\rptApproveCustomersforWeb.rpt"), String)
                        rptreportname = "Report - Approve Customers for Web"
                        Exit Select
                    Case "LockAgentsforWeb"
                        strReportName = CType(Server.MapPath("~\Report\rptLockAgentsforWeb.rpt"), String)
                        rptreportname = "Report - Lock Agents for Web"
                        Exit Select

                End Select
                If strReportName = "" Then
                    Response.Redirect(CType(Session("BackPageName"), String), False)
                    Exit Sub
                Else
                    BindReport(strReportName, CType(Session("SelectionFormula"), String), CType(Session("ReportTitle"), String))
                End If

            End If
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub BindReport(ByVal ReportName As String, ByVal strSelectionFormula As String, ByVal strReportTitle As String)

        Dim pnames As ParameterFieldDefinitions
        Dim pname As ParameterFieldDefinition
        Dim param As New ParameterValues
        Dim paramvalue As New ParameterDiscreteValue

        'If Session("Rep") Is Nothing Then
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
        repDeocument.Load(ReportName)

        Me.CRVReport.ReportSource = repDeocument
        Dim RepTbls As Tables = repDeocument.Database.Tables

        For Each RepTbl As Table In RepTbls
            Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            RepTblLogonInfo.ConnectionInfo = ConnInfo
            RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        Next

        repDeocument.SummaryInfo.ReportTitle = strReportTitle
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

        If Not Session("ColReportParams") Is Nothing Then
            Dim p As Integer
            Dim colreport As New Collection
            colreport = Session("ColReportParams")
            Dim creport As New clsReportParam
            For p = 1 To colreport.Count
                creport = colreport.Item(p)
                pname = pnames.Item(creport.rep_parametername)
                paramvalue.Value = creport.rep_parametervalue
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

            Next

        End If
        Me.CRVReport.ReportSource = repDeocument
        If strSelectionFormula <> "" Then
            CRVReport.SelectionFormula = strSelectionFormula
        End If
        Session.Add("ReportSource", repDeocument)
        Me.CRVReport.DataBind()
        CRVReport.HasCrystalLogo = False
        ' CRVReport.HasToggleGroupTreeButton = False
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If CType(Session("BackPageName"), String) = "" Then
            Response.Redirect("MainPage.aspx", False)
            Exit Sub
        Else
            Session("ColReportParams") = Nothing
            Response.Redirect(CType(Session("BackPageName"), String), False)
        End If



    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If Page.IsPostBack = True Then
            repDeocument.Close()
            repDeocument.Dispose()
            ' Session("ColReportParams") = Nothing
        End If
    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Session.Add("ReportSource", repDeocument)
    '    Dim strpop As String = ""
    '    'strpop = "window.open('../PriceListModule/RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
    '    strpop = "<script language=""javascript"">var win=window.open('../PriceListModule/RptPrintPage.aspx','printdoc','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
    '    'ScriptStr = "<script language=""javascript"">var win=window.open('PrintDoc.aspx','mywindow2','toolbar=0,width=250,height=150,top=100,left=200,scrollbars=yes,resizable=no,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, False)
    'End Sub
End Class

