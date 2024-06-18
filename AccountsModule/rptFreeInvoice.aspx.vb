'------------================--------------=======================------------------================
'   Module Name    :    rptInvoice.aspx
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
Partial Class Free_rptInvoice
    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    'Dim rptcompanyname As String
    'Dim rptreportname As String
    Dim objutils As New clsUtils
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            Dim InvoiceNo As String = ""
            'requestid = CType(Session("RequestId"), String)
            If Request.QueryString("InvoiceNo") Is Nothing = False Then
                InvoiceNo = CType(Request.QueryString("InvoiceNo"), String)
            End If
            BindReport(InvoiceNo)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub BindReport(ByVal InvoiceNo As String)
        Try
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

            rep.Load(Server.MapPath("~\Report\rptFreeInvoicenew.rpt"))

            Me.CRVInvoice.ReportSource = rep
            Dim RepTbls As Tables = rep.Database.Tables

            For Each RepTbl As Table In RepTbls
                Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
                RepTblLogonInfo.ConnectionInfo = ConnInfo
                RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
            Next

            rep.SummaryInfo.ReportTitle = strReportTitle
            pnames = rep.DataDefinition.ParameterFields

            pname = pnames.Item("@requestid")
            paramvalue.Value = InvoiceNo
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("CompanyName")
            paramvalue.Value = CType(Session("CompanyName"), String)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            Me.CRVInvoice.ReportSource = rep
            Me.CRVInvoice.DataBind()
            'Response.Buffer = False
            'Response.ClearContent()
            'Response.ClearHeaders()
            'rep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, False, "Report")
        Catch ex As Exception

        Finally
            rep.Dispose()
        End Try
    End Sub
End Class
