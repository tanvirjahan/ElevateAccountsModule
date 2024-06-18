Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
Imports System.IO
Imports System.IO.File
Partial Class WebAdminModule_rptAgentloginInformationReport
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Dim rptcompanyname As String
    Dim rptreportname As String
    Dim objutils As New clsUtils
    Dim fromdate As String
    Dim supcode As String, supcodeto As String
    Dim remarks As String


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState.Add("Pageame", Request.QueryString("Pageame"))
        ViewState.Add("BackPageName", Request.QueryString("BackPageName"))
        Try
            If Request.QueryString("fromdate") <> "" Then
                'fromdate = Request.QueryString("fromdate")
                ViewState.Add("fromdate", Request.QueryString("fromdate"))
            Else
                ViewState.Add("fromdate", String.Empty)
            End If
            If Request.QueryString("todate") <> "" Then
                'fromdate = Request.QueryString("fromdate")
                ViewState.Add("todate", Request.QueryString("todate"))
            Else
                ViewState.Add("todate", String.Empty)
            End If
            If Request.QueryString("custcode") <> "" Then
                'supcode = Request.QueryString("supcode")
                ViewState.Add("custcode", Request.QueryString("custcode"))
            Else
                ViewState.Add("custcode", String.Empty)
            End If
            If Request.QueryString("subuser") <> "" Then
                ViewState.Add("subuser", Request.QueryString("subuser"))
            Else
                ViewState.Add("subuser", String.Empty)
            End If
            If Request.QueryString("CtryCode") <> "" Then
                'supcode = Request.QueryString("supcode")
                ViewState.Add("CtryCode", Request.QueryString("CtryCode"))
            Else
                ViewState.Add("CtryCode", String.Empty)
            End If
            If Request.QueryString("CityCode") <> "" Then
                ViewState.Add("CityCode", Request.QueryString("CityCode"))
            Else
                ViewState.Add("CityCode", String.Empty)
            End If

            If Request.QueryString("rpttype") <> "" Then
                'supcode = Request.QueryString("supcode")
                ViewState.Add("rpttype", Request.QueryString("rpttype"))
            Else
                ViewState.Add("rpttype", String.Empty)
            End If

            If Request.QueryString("rptrepfilter") <> "" Then
                'supcode = Request.QueryString("supcode")
                ViewState.Add("rptrepfilter", Request.QueryString("rptrepfilter"))
            Else
                ViewState.Add("rptrepfilter", String.Empty)
            End If

            btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")

            '' BindReport1()
            BindReport()
        Catch ex As Exception
            objutils.WritErrorLog("rptRoomimgListReport.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Private Sub BindReport()
        Dim strReportTitle As String = ""
        Dim reportoption As String = ""

        rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster"), String)
        Dim pnames As ParameterFieldDefinitions
        Dim pname As ParameterFieldDefinition
        Dim param As New ParameterValues
        Dim paramvalue As New ParameterDiscreteValue
        Dim ConnInfo As New ConnectionInfo

        With ConnInfo
            .ServerName = Session("dbServerName")
            .DatabaseName = Session("dbDatabaseName")
            .UserID = Session("dbUserName")
            .Password = Session("dbPassword")
        End With


        rptreportname = "Agent Login Information Report"
        rep.Load(Server.MapPath("~\Report\rptagentlogininformation.rpt"))

        Me.CRVReport.ReportSource = rep
        Dim RepTbls As Tables = rep.Database.Tables

        For Each RepTbl As Table In RepTbls
            Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            RepTblLogonInfo.ConnectionInfo = ConnInfo
            RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        Next

        rep.SummaryInfo.ReportTitle = rptreportname

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
        paramvalue.Value = Trim(ViewState("rptrepfilter"))
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@agentcode")
        paramvalue.Value = Trim(ViewState("custcode"))
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@ctrycode")
        paramvalue.Value = Trim(ViewState("CtryCode"))
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@citycode")
        paramvalue.Value = Trim(ViewState("CityCode"))
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@loginfrmdate")
        paramvalue.Value = Trim(ViewState("fromdate"))
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@logintodate")
        paramvalue.Value = Trim(ViewState("todate"))
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        pname = pnames.Item("@subusercode")
        paramvalue.Value = Trim(ViewState("subuser"))
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        pname = pnames.Item("@list")
        paramvalue.Value = Trim(ViewState("rpttype"))
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)


        Session.Add("ReportSource", rep)
        Me.CRVReport.ReportSource = rep
        'If strSelectionFormula <> "" Then
        '    CRVSupplierPolicies.SelectionFormula = strSelectionFormula
        'End If
        Me.CRVReport.DataBind()
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If Page.IsPostBack = True Then
            If ViewState("RepCalledFrom") <> 1 Then
                rep.Close()
                rep.Dispose()
                GC.Collect()
            End If
        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Session.Add("ReportSource", rep)
        ViewState.Add("RepCalledFrom", 1)
        Dim strpop As String = ""
        strpop = "window.open('../PriceListModule/RptPrintPage.aspx','RptRoomingList','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
End Class
