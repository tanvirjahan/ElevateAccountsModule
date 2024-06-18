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
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Session("GlobalUserName") Is Nothing Then
            Try
                Dim printId = Request.QueryString("printId")
                Dim fileName As String = ""
                Dim bytes As Byte()
                If printId = "salesInvoice" Then
                    Dim SI As clsInvoicePdf = New clsInvoicePdf()
                    Dim invoiceNo As String = Request.QueryString("InvoiceNo")
                    Dim ds As New DataSet
                    bytes = {}
                    SI.InvoicePrint(invoiceNo, bytes, ds, "download")
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
                ElseIf printId = "ProformaVat" Then
                    Dim bc As clsBookingConfirmationPdf = New clsBookingConfirmationPdf()
                    Dim requestid = Request.QueryString("RequestId")
                    Dim ManualInvNo = Request.QueryString("ManualInvNo")
                    Dim ds As New DataSet
                    bytes = {}
                    Dim chkCumulative As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select bookingengineratetype from booking_header H inner join agentmast A on H.agentcode=A.agentcode and H.requestid='" + requestid + "'")
                    If String.IsNullOrEmpty(chkCumulative) Then chkCumulative = ""
                    bc.GenerateReportProformaVat(requestid, ManualInvNo, bytes, ds, "download")

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
                ElseIf printId = "bookingConfirmation" Then
                    Dim bc As clsBookingConfirmationPdf = New clsBookingConfirmationPdf()
                    Dim requestid = Request.QueryString("RequestId")
                    Dim ds As New DataSet
                    bytes = {}
                    Dim chkCumulative As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select bookingengineratetype from booking_header H inner join agentmast A on H.agentcode=A.agentcode and H.requestid='" + requestid + "'")
                    If String.IsNullOrEmpty(chkCumulative) Then chkCumulative = ""

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

                ElseIf printId = "bookingVoucher" Then
                    Dim bc As clsBookingConfirmationPdf = New clsBookingConfirmationPdf()
                    Dim requestid = Request.QueryString("RequestId")
                    Dim ds As New DataSet
                    bytes = {}
                    Dim chkCumulative As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select bookingengineratetype from booking_header H inner join agentmast A on H.agentcode=A.agentcode and H.requestid='" + requestid + "'")
                    If String.IsNullOrEmpty(chkCumulative) Then chkCumulative = ""
                    ' If chkCumulative.Trim() = "CUMULATIVE" Then
                    bc.GenerateVoucherReport(requestid, bytes, ds, "download")
                    'Else
                    '    bc.GenerateReport(requestid, bytes, ds, "download")
                    'End If
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
                ElseIf printId = "Itinerary" Then
                    Dim Itinerary As clsItineraryPdf = New clsItineraryPdf()
                    Dim requestid = Request.QueryString("RequestId")
                    Dim rlineNo = Request.QueryString("rlineNo")
                    bytes = {}
                    Dim objResParam As New ReservationParameters
                    Itinerary.GenerateItineraryReport(requestid, bytes, "download")
                    fileName = "Itinerary@" + requestid + "@" + DateTime.Now.ToString("ddMMyyyy@HHmmss") + ".pdf"
                    Response.Clear()
                    Response.AddHeader("Content-Disposition", "inline; filename=" + fileName)
                    Response.AddHeader("Content-Length", bytes.Length.ToString())
                    Response.ContentType = "application/pdf"
                    Response.Buffer = True
                    Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    Response.BinaryWrite(bytes)
                    Response.Flush()
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                ElseIf printId = "hotelVoucher" Then
                    Dim hVoucher As clsHotelVoucherPdf = New clsHotelVoucherPdf()
                    Dim requestid = Request.QueryString("RequestId")
                    Dim rlineNo As Integer = Request.QueryString("rlineNo")
                    bytes = {}
                    hVoucher.GenerateHotelVoucher(requestid, rlineNo, bytes, "download")
                    fileName = "HotelVoucher@" + requestid + "@" + DateTime.Now.ToString("ddMMyyyy@HHmmss") + ".pdf"
                    Response.Clear()
                    Response.AddHeader("Content-Disposition", "inline; filename=" + fileName)
                    Response.AddHeader("Content-Length", bytes.Length.ToString())
                    Response.ContentType = "application/pdf"
                    Response.Buffer = True
                    Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    Response.BinaryWrite(bytes)
                    Response.Flush()
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                ElseIf printId = "ExcursionTicket" Then

                    Dim requestid = Request.QueryString("RequestId")
                    Dim rlineNo As Integer = Request.QueryString("RlineNo")

                    Dim strExc As String = ""
                    strExc = "select file_attachment from New_booking_tours t(nolock),NewBooking_ServiceAllocation(nolock) s where t.requestid=s.requestid and t.service_id = s.service_id and isnull(s.file_attachment,'')<>'' and t.requestid='" & requestid & "' and elineno='" & rlineNo & "'"
                    Dim dt As DataTable
                    Dim objclsUtilities As New clsUtils
                    dt = objclsUtilities.GetDataFromDataTable(strExc)
                    If dt.Rows.Count > 0 Then

                        For i As Integer = 0 To dt.Rows.Count - 1
                            '    Dim strFilePath As String = "D:\Abin\New\RG001425.png" ' & dt.Rows(i)("file_attachment").ToString '
                            Dim strFilePath As String = ConfigurationManager.AppSettings("ExcursionTicketPath").ToString & dt.Rows(i)("file_attachment").ToString '
                            Response.ContentType = ContentType
                            Response.AppendHeader("Content-Disposition", ("attachment; filename=" + Path.GetFileName(strFilePath)))
                            Response.WriteFile(strFilePath)
                            Response.End()
                        Next


                    End If

                    'Dim hVoucher As clsHotelVoucherPdf = New clsHotelVoucherPdf()
                    'Dim requestid = Request.QueryString("RequestId")
                    'Dim rlineNo As Integer = Request.QueryString("rlineNo")
                    'bytes = {}
                    'hVoucher.GenerateHotelVoucher(requestid, rlineNo, bytes, "download")
                    'fileName = "HotelVoucher@" + requestid + "@" + DateTime.Now.ToString("ddMMyyyy@HHmmss") + ".pdf"
                    'Response.Clear()
                    'Response.AddHeader("Content-Disposition", "inline; filename=" + fileName)
                    'Response.AddHeader("Content-Length", bytes.Length.ToString())
                    'Response.ContentType = "application/pdf"
                    'Response.Buffer = True
                    'Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    'Response.BinaryWrite(bytes)
                    'Response.Flush()
                    'HttpContext.Current.ApplicationInstance.CompleteRequest()
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
