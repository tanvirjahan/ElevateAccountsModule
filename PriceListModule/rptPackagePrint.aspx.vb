'------------================--------------=======================------------------================
'   Module Name    :    rptPackagePrint.aspx
'   Developer Name :    Jaffer
'   Date           :    21 Oct 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class PriceListModule_rptPackagePrint
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    'Dim rptcompanyname As String
    'Dim rptreportname As String
    Dim objutils As New clsUtils
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            Dim pkgid As String = ""
            'requestid = CType(Session("RequestId"), String)
            If Request.QueryString("pkgid") Is Nothing = False Then
                pkgid = CType(Request.QueryString("pkgid"), String)
            End If
            BindReport(pkgid)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub BindReport(ByVal pkgid As String)
        Dim strReportTitle As String = "Reservation Allotment Details"
        Dim reportoption As String = ""

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

        rep.Load(Server.MapPath("~\Report\rptPackage_MainReport.rpt"))

        Me.CRVPackage.ReportSource = rep
        Dim RepTbls As Tables = rep.Database.Tables

        For Each RepTbl As Table In RepTbls
            Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
            RepTblLogonInfo.ConnectionInfo = ConnInfo
            RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
        Next

        rep.SummaryInfo.ReportTitle = strReportTitle
        pnames = rep.DataDefinition.ParameterFields

        pname = pnames.Item("@pkgid")
        paramvalue.Value = pkgid
        param = pname.CurrentValues
        param.Add(paramvalue)
        pname.ApplyCurrentValues(param)

        ViewState.Add("RepCalledFrom", 0)
        btnBack.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to close report?')==false)return false;")



        Me.CRVPackage.ReportSource = rep
        Me.CRVPackage.DataBind()
       
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("CalculateSellingPriceSearch.aspx", False)
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If ViewState("RepCalledFrom") <> 1 Then
            rep.Close()
            rep.Dispose()
            ' Session("ColReportParams") = Nothing
        End If
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Session.Add("ReportSource", rep)
        ViewState.Add("RepCalledFrom", 1)
        Dim strpop As String = ""
        strpop = "window.open('RptPrintPage.aspx','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
End Class
