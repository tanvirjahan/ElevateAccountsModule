
Imports System
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Globalization
Imports System.Diagnostics

Partial Class PrintReport
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
#End Region

#Region "Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("GlobalUserName") Is Nothing Then
            Try
                Dim printId = Request.QueryString("printId")
                Dim fileName As String = ""
                Dim bytes As Byte()
                If printId = "salesInvoice" Then
                    Dim SI As clsInvoicePdf = New clsInvoicePdf()
                    Dim invoiceNo As String = Request.QueryString("InvoiceNo")
                    Dim formatType As String = Convert.ToString(Request.QueryString("FormatType"))
                    If formatType = Nothing Then formatType = ""
                    Dim ds As New DataSet
                    bytes = {}
                    SI.InvoicePrint(invoiceNo, bytes, ds, "download", formatType:=formatType)
                    fileName = "Invoice@" + invoiceNo + ".pdf"
                    Response.Clear()
                    Response.AddHeader("Content-Disposition", "inline; filename=" + fileName)
                    Response.AddHeader("Content-Length", bytes.Length.ToString())
                    Response.ContentType = "application/pdf"
                    Response.Buffer = True
                    Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    Response.BinaryWrite(bytes)
                    Response.Flush()
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                ElseIf printId = "InvoiceVoucher" Then
                    Dim JI As clsJournalPdf = New clsJournalPdf()
                    Dim invoiceNo As String = Request.QueryString("InvoiceNo")
                    Dim ds As New DataSet
                    bytes = {}
                    JI.JournalPrint(invoiceNo, bytes, ds, "download")
                    fileName = "InvoiceVoucher@" + invoiceNo + ".pdf"
                    Response.Clear()
                    Response.AddHeader("Content-Disposition", "inline; filename=" + fileName)
                    Response.AddHeader("Content-Length", bytes.Length.ToString())
                    Response.ContentType = "application/pdf"
                    Response.Buffer = True
                    Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    Response.BinaryWrite(bytes)
                    Response.Flush()
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                ElseIf printId = "PurchaseInvoiceVoucher" Then
                    Dim JI As clsJournalPdf = New clsJournalPdf()
                    Dim invoiceNo As String = Request.QueryString("InvoiceNo")
                    Dim divcode As String = Request.QueryString("divcode")
                    Dim ds As New DataSet
                    bytes = {}
                    JI.PurchaseJournalPrint(invoiceNo, divcode, bytes, ds, "download")
                    fileName = "InvoiceVoucher@" + invoiceNo + ".pdf"
                    Response.Clear()
                    Response.AddHeader("Content-Disposition", "inline; filename=" + fileName)
                    Response.AddHeader("Content-Length", bytes.Length.ToString())
                    Response.ContentType = "application/pdf"
                    Response.Buffer = True
                    Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    Response.BinaryWrite(bytes)
                    Response.Flush()
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                ElseIf printId = "InvoiceVoucher" Then
                    Dim JI As clsJournalPdf = New clsJournalPdf()
                    Dim invoiceNo As String = Request.QueryString("InvoiceNo")
                    Dim ds As New DataSet
                    bytes = {}
                    JI.JournalPrint(invoiceNo, bytes, ds, "download")
                    fileName = "InvoiceVoucher@" + invoiceNo + ".pdf"
                    Response.Clear()
                    Response.AddHeader("Content-Disposition", "inline; filename=" + fileName)
                    Response.AddHeader("Content-Length", bytes.Length.ToString())
                    Response.ContentType = "application/pdf"
                    Response.Buffer = True
                    Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    Response.BinaryWrite(bytes)
                    Response.Flush()
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                ElseIf printId = "bookingConfirmation" Then
                    Dim bc As clsBookingConfirmationPdf = New clsBookingConfirmationPdf()
                    Dim requestid = Request.QueryString("RequestId")
                    Dim ds As New DataSet
                    bytes = {}
                    Dim chkCumulative As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select bookingengineratetype from booking_header H inner join agentmast A on H.agentcode=A.agentcode and H.requestid='" + requestid + "'")
                    If String.IsNullOrEmpty(chkCumulative) Then chkCumulative = ""
                    'If chkCumulative.Trim() = "CUMULATIVE" And Not requestid.Contains("RGV") And Not requestid.Contains("RPV") And Not requestid.Contains("RGT1.0") And Not requestid.Contains("RPT1.0") And Not requestid.Contains("RG1.0") And Not requestid.Contains("RP1.0") Then
                    '    bc.GenerateCumulativeReport(requestid, bytes, ds, "download")
                    'Else
                    '    bc.GenerateReport(requestid, bytes, ds, "download")
                    'End If

                    Dim strQueryGroup As String = "select isnull(isGroup,0)isGroup from booking_header(nolock) where requestid='" & requestid & "'"
                    Dim strIsGroup As String = objUtils.ExecuteQueryReturnStringValue(strQueryGroup)
                    If strIsGroup = "1" Then
                        bc.GenerateCumulativeReport(requestid, bytes, ds, "download")
                    Else
                        Dim strIsPackageQuery As String = "select count(*)cnt from booking_profit_summary(nolock) where requestid='" & requestid & "'"
                        Dim strIsPackage As String = objUtils.ExecuteQueryReturnStringValue(strIsPackageQuery)
                        If chkCumulative.Trim() = "CUMULATIVE" And Val(strIsPackage) > 0 Then
                            bc.GenerateCumulativeReport(requestid, bytes, ds, "download")
                        Else
                            bc.GenerateReport(requestid, bytes, ds, "download")
                        End If
                    End If
                    fileName = requestid + "@" + DateTime.Now.ToString("ddMMyyyy@HHmmss") + ".pdf"
                    Response.Clear()
                    Response.AddHeader("Content-Disposition", "inline; filename=" + fileName)
                    Response.AddHeader("Content-Length", bytes.Length.ToString())
                    Response.ContentType = "application/pdf"
                    Response.Buffer = True
                    Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    Response.BinaryWrite(bytes)
                    Response.Flush()
                    HttpContext.Current.ApplicationInstance.CompleteRequest()

                ElseIf printId = "PurchaseInvoice" Then
                    Dim SI As clsInvoicePdf = New clsInvoicePdf()
                    Dim invoiceNo As String = Request.QueryString("InvoiceNo")
                    Dim divcode As String = Request.QueryString("divcode")
                    Dim ds As New DataSet
                    bytes = {}
                    SI.PurchaseInvoicePrint(invoiceNo, divcode, bytes, ds, "download")
                    fileName = "Invoice@" + invoiceNo + ".pdf"
                    Response.Clear()
                    Response.AddHeader("Content-Disposition", "inline; filename=" + fileName)
                    Response.AddHeader("Content-Length", bytes.Length.ToString())
                    Response.ContentType = "application/pdf"
                    Response.Buffer = True
                    Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    Response.BinaryWrite(bytes)
                    Response.Flush()
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("PrintReport.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            Response.Redirect("Login.aspx", True)
        End If
    End Sub
#End Region

End Class
