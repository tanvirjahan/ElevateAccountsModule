Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource

Partial Class PriceListModule_test
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Dim repDeocument As New ReportDocument
            repDeocument = CType(Session("doc"), ReportDocument)
            Response.Buffer = False
            Response.ClearContent()
            Response.ClearHeaders()
            repDeocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, False, "Report")
            Session.Add("doc", "")
        End If
    End Sub
End Class
