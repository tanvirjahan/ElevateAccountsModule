Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class VisitfollReport
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
            If Request.QueryString("smancode") <> "" Then
                'catcode = Request.QueryString("catcode")
                ViewState.Add("smancode", Request.QueryString("smancode"))
            Else
                ViewState.Add("smancode", String.Empty)
            End If
            If Request.QueryString("smanname") <> "" Then
                'catcode = Request.QueryString("catcode")
                ViewState.Add("smanname", Request.QueryString("smanname"))
            Else
                ViewState.Add("smanname", String.Empty)
            End If

            If Request.QueryString("ctrycode") <> "" Then
                ViewState.Add("ctrycode", Request.QueryString("ctrycode"))
            Else
                ViewState.Add("ctrycode", String.Empty)
            End If
            If Request.QueryString("ctryname") <> "" Then
                ViewState.Add("ctryname", Request.QueryString("ctryname"))
            Else
                ViewState.Add("ctryname", String.Empty)
            End If
            If Request.QueryString("citycode") <> "" Then
                ViewState.Add("citycode", Request.QueryString("citycode"))
            Else
                ViewState.Add("citycode", String.Empty)
            End If
            If Request.QueryString("cityname") <> "" Then
                ViewState.Add("cityname", Request.QueryString("cityname"))
            Else
                ViewState.Add("cityname", String.Empty)
            End If
            If Request.QueryString("customercode") <> "" Then
                ViewState.Add("customercode", Request.QueryString("customercode"))
            Else
                ViewState.Add("customercode", String.Empty)
            End If
            If Request.QueryString("customername") <> "" Then
                ViewState.Add("customername", Request.QueryString("customername"))
            Else
                ViewState.Add("customername", String.Empty)
            End If
            If Request.QueryString("fromdate") <> "" Then
                ViewState.Add("fromdate", Request.QueryString("fromdate"))
            Else
                ViewState.Add("fromdate", String.Empty)
            End If
            If Request.QueryString("todate") <> "" Then
                ViewState.Add("todate", Request.QueryString("todate"))
            Else
                ViewState.Add("todate", String.Empty)
            End If
            If Request.QueryString("reporttitle") <> "" Then
                ViewState.Add("reporttitle", Request.QueryString("reporttitle"))
            Else
                ViewState.Add("reporttitle", String.Empty)
            End If
            If Request.QueryString("repfilter") <> "" Then
                repfilter = Request.QueryString("repfilter")
            Else
                repfilter = ""
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
        rptreportname = "Report - Visit Follow Up"

        Dim pnames As ParameterFieldDefinitions
        Dim pname As ParameterFieldDefinition
        Dim param As New ParameterValues
        Dim paramvalue As New ParameterDiscreteValue
        Dim ConnInfo As New ConnectionInfo

        With ConnInfo
            .ServerName = Session("dbServerName")        'ConfigurationManager.AppSettings("dbServerName")
            .DatabaseName = Session("dbDatabaseName")    'ConfigurationManager.AppSettings("dbDatabaseName")
            .UserID = Session("dbUserName")              'ConfigurationManager.AppSettings("dbUserName")
            .Password = Session("dbPassword")            'ConfigurationManager.AppSettings("dbPassword")
        End With

        'reportoption = Session("reportoption")
        rep.Load(Server.MapPath("~\Report\rptvisitfollup.rpt"))



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


        pname = pnames.Item("@smancode")
        'paramvalue.Value = Session("catcode")
        paramvalue.Value = ViewState("smancode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        If ViewState("smanname") <> "" Then
            repfilter = repfilter + " ; " + " Sales Man : " + ViewState("smanname")
        End If


        pname = pnames.Item("@ctrycode")
        paramvalue.Value = ViewState("ctrycode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        If ViewState("ctryname") <> "" Then
            repfilter = repfilter + " ; " + "Country : " + ViewState("ctryname")
        End If

        pname = pnames.Item("@citycode")
        paramvalue.Value = ViewState("citycode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        If ViewState("cityname") <> "" Then
            repfilter = repfilter + " ; " + "City : " + ViewState("cityname")
        End If

        pname = pnames.Item("@customercode")
        paramvalue.Value = ViewState("customercode")
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        If ViewState("customername") <> "" Then
            repfilter = repfilter + " ; " + "Customers : " + ViewState("customername")
        End If


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

        pname = pnames.Item("repfilter")
        paramvalue.Value = repfilter
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        Me.CRVNewClients.ReportSource = rep
        'End If
        Me.CRVNewClients.DataBind()
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If ViewState("RepCalledFrom") <> 1 Then
            rep.Close()
            rep.Dispose()
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
