'------------================--------------=======================------------------================

'
'------------================--------------=======================------------------================
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
Partial Class ExcursionModule_RptPrintPage

    Inherits System.Web.UI.Page
    Dim rep As New ReportDocument
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            rep = New ReportDocument()
            If Session("ReportSource") Is Nothing = False Then
                rep = CType(Session("ReportSource"), ReportDocument)
            End If

            Dim oStream As MemoryStream

            Response.Clear()
            Response.Buffer = True


            oStream = CType(rep.ExportToStream(ExportFormatType.PortableDocFormat), MemoryStream)
            Response.ContentType = "application/pdf"


            rep.Close()
            rep.Dispose()

            Try
                Response.BinaryWrite(oStream.ToArray())
                Response.End()
            Catch ex As Exception

            Finally
                oStream.Flush()
                oStream.Close()
                oStream.Dispose()
            End Try

            'Response.Buffer = False
            'Response.ClearContent()
            'Response.ClearHeaders()
            'rep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, False, "Report")

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
       
    End Sub

End Class


