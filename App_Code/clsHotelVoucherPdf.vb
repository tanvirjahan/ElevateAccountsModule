Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class clsHotelVoucherPdf

#Region "Global Variable"
    Dim objclsUtilities As New clsUtils
    Dim NormalFont As Font = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK)
    Dim NormalFontBold As Font = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK)
    Dim NormalFontBoldRed As Font = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.RED)
    Dim TitleFontBoldUnderLine As Font = FontFactory.GetFont("Arial", 12, Font.BOLD Or Font.UNDERLINE, BaseColor.BLACK)
    Dim titleColor As BaseColor = New BaseColor(214, 214, 214)
#End Region

#Region "Public Sub GenerateHotelVoucher(ByVal requestID As String, ByVal rLineNo As Integer, ByRef bytes() As Byte, ByVal printMode As String, Optional ByVal fileName As String = "")"
    Public Sub GenerateHotelVoucher(ByVal requestID As String, ByVal rLineNo As Integer, ByRef bytes() As Byte, ByVal printMode As String, Optional ByVal fileName As String = "")
        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("sp_Hotel_Voucher_Print", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@RequestID", SqlDbType.VarChar, 20)).Value = requestID
            mySqlCmd.Parameters.Add(New SqlParameter("@rLineNo", SqlDbType.Int)).Value = rLineNo
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim headerDt As DataTable = ds.Tables(0)
            Dim hotelDt As DataTable = ds.Tables(1)
            Dim CheckInOutDt As DataTable = ds.Tables(2)
            Dim ContactDt As DataTable = ds.Tables(3)
            clsDBConnect.dbConnectionClose(sqlConn)
            If headerDt.Rows.Count > 0 Then
                Dim document As New Document(PageSize.A4, 0.0F, 0.0F, 20.0F, 35.0F)
                Dim documentWidth As Single = 550.0F
                Dim TitleFont As Font = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)
                Dim TitleFontBold As Font = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)
                Dim TitleFontBig As Font = FontFactory.GetFont("Arial", 16, Font.NORMAL, BaseColor.BLACK)
                Dim TitleFontBigBold As Font = FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)
                Dim remainingPageSpace As Single
                Using memoryStream As New System.IO.MemoryStream()
                    Dim writer As PdfWriter
                    If printMode = "download" Then
                        writer = PdfWriter.GetInstance(document, memoryStream)
                    Else
                        Dim path As String = System.Web.HttpContext.Current.Server.MapPath("~\SavedReports\") + fileName
                        writer = PdfWriter.GetInstance(document, New FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                    End If
                    Dim phrase As Phrase = Nothing
                    Dim cell As PdfPCell = Nothing
                    Dim tblHeader As PdfPTable = Nothing
                    document.Open()
                    'Header Table
                    Dim headerDr As DataRow = headerDt.Rows(0)
                    tblHeader = New PdfPTable(2)
                    tblHeader.TotalWidth = documentWidth
                    tblHeader.LockedWidth = True
                    tblHeader.SetWidths(New Single() {0.3F, 0.7F})
                    tblHeader.Complete = False
                    tblHeader.SplitRows = False
                    'Company Name 
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("division_master_des")), TitleFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.PaddingBottom = 6.0F
                    tblHeader.AddCell(cell)
                    'Company Logo
                    If (headerDr("div_code") = "01") Then
                        cell = ImageCell("~/images/logo.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                    Else
                        cell = ImageCell("~/images/logo.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                    End If
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.BorderColor = BaseColor.BLACK
                    cell.PaddingTop = 3.0F
                    cell.PaddingBottom = 8.0F
                    tblHeader.AddCell(cell)
                    'Company Address
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("address1")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Tel : " & Convert.ToString(headerDr("tel")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("E-mail : " & Convert.ToString(headerDr("email")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Website : " & Convert.ToString(headerDr("website")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingBottom = 8.0F
                    tblHeader.AddCell(cell)

                    tblHeader.Complete = True
                    document.Add(tblHeader)
                    writer.PageEvent = New clsHotelVoucherPageEvents(printMode)

                    '----Booking Details
                    If hotelDt.Rows.Count > 0 Then
                        Dim tblBooking As PdfPTable = New PdfPTable(2)
                        tblBooking.TotalWidth = documentWidth
                        tblBooking.LockedWidth = True
                        tblBooking.SetWidths(New Single() {0.4F, 0.6F})
                        tblBooking.Complete = False
                        tblBooking.SplitRows = False
                        For Each hotelDr As DataRow In hotelDt.Rows
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("ReferenceAgent")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.PaddingLeft = 5.0F
                            tblBooking.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("ReferenceNumber")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.PaddingLeft = 5.0F
                            tblBooking.AddCell(cell)
                            For col As Integer = 2 To hotelDt.Columns.Count - 1
                                phrase = New Phrase()
                                phrase.Add(New Chunk(Convert.ToString(hotelDt.Columns(col).ColumnName), NormalFont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                cell.PaddingTop = 2.0F
                                cell.PaddingBottom = 5.0F
                                cell.PaddingLeft = 5.0F
                                tblBooking.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk(Convert.ToString(hotelDr(col)), NormalFont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                cell.PaddingTop = 2.0F
                                cell.PaddingBottom = 5.0F
                                cell.PaddingLeft = 5.0F
                                tblBooking.AddCell(cell)
                            Next
                        Next
                        tblBooking.Complete = True
                        tblBooking.SpacingBefore = 15
                        document.Add(tblBooking)
                    End If

                    Dim tblBill As PdfPTable = New PdfPTable(1)
                    tblBill.TotalWidth = documentWidth
                    tblBill.LockedWidth = True
                    tblBill.SetWidths(New Single() {1.0F})
                    tblBill.Complete = False
                    tblBill.SplitRows = False

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Please bill the above to " & Convert.ToString(headerDr("division_master_des")) & " and rest all extras to the guests directly", NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingTop = 2.0F
                    cell.PaddingBottom = 5.0F
                    tblBill.AddCell(cell)

                    tblBill.Complete = True
                    tblBill.SpacingBefore = 7
                    document.Add(tblBill)

                    '-----Check In /Check Out Note
                    If CheckInOutDt.Rows.Count > 0 Then
                        Dim tblCheckInOut As PdfPTable = New PdfPTable(2)
                        tblCheckInOut.TotalWidth = documentWidth
                        tblCheckInOut.LockedWidth = True
                        tblCheckInOut.SetWidths(New Single() {0.05F, 0.95F})
                        tblCheckInOut.Complete = False
                        tblCheckInOut.SplitRows = False

                        phrase = New Phrase()
                        phrase.Add(New Chunk("CHECK IN & CHECK OUT NOTE" & vbCrLf, TitleFontBoldUnderLine))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.SetLeading(15, 0)
                        cell.PaddingLeft = 10.0F
                        cell.PaddingBottom = 5.0F
                        cell.Colspan = 2
                        tblCheckInOut.AddCell(cell)

                        For Each CheckInOutDr As DataRow In CheckInOutDt.Rows
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Chr(149), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                            cell.SetLeading(15, 0)
                            cell.PaddingBottom = 5.0F
                            tblCheckInOut.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(CheckInOutDr("policyText")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 5.0F
                            tblCheckInOut.AddCell(cell)
                        Next

                        tblCheckInOut.Complete = True
                        tblCheckInOut.SpacingBefore = 15
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 72 Then document.NewPage()
                        document.Add(tblCheckInOut)
                    End If

                    '-----Emergency Contact details
                    If ContactDt.Rows.Count > 0 Then
                        Dim tblContact As PdfPTable = New PdfPTable(3)
                        tblContact.TotalWidth = documentWidth
                        tblContact.LockedWidth = True
                        tblContact.SetWidths(New Single() {0.4F, 0.1, 0.5F})
                        tblContact.Complete = False
                        tblContact.SplitRows = False

                        phrase = New Phrase()
                        phrase.Add(New Chunk("EMERGENCY CONTACT" & vbCrLf, TitleFontBoldUnderLine))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.SetLeading(15, 0)
                        cell.PaddingLeft = 10.0F
                        cell.PaddingBottom = 5.0F
                        cell.Colspan = 3
                        tblContact.AddCell(cell)

                        For Each ContactDr As DataRow In ContactDt.Rows
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(ContactDr("ContactPerson")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                            cell.SetLeading(15, 0)
                            cell.PaddingLeft = 10.0F
                            cell.PaddingBottom = 5.0F
                            tblContact.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(":", NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 5.0F
                            tblContact.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(ContactDr("ContactNo")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 5.0F
                            tblContact.AddCell(cell)
                        Next
                        tblContact.Complete = True
                        tblContact.SpacingBefore = 15
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 72 Then document.NewPage()
                        document.Add(tblContact)
                    End If

                    document.AddTitle("Hotel Voucher-" & requestID)
                    document.Close()
                    If printMode = "download" Then
                        Dim pagingFont As Font = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.GRAY)
                        Dim reader As New PdfReader(memoryStream.ToArray())
                        Using mStream As New MemoryStream()
                            Using stamper As New PdfStamper(reader, mStream)
                                Dim pages As Integer = reader.NumberOfPages
                                For i As Integer = 1 To pages
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 300.0F, 20.0F, 0)
                                Next
                            End Using
                            bytes = mStream.ToArray()
                        End Using
                    End If
                End Using
            Else
                Throw New Exception("There is no rows in header table")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Private Shared Function PhraseCell(phrase As Phrase, align As Integer, Cols As Integer, celBorder As Boolean, Optional celBottomBorder As String = ""None"") As PdfPCell"
    Private Shared Function PhraseCell(ByVal phrase As Phrase, ByVal align As Integer, ByVal Cols As Integer, ByVal celBorder As Boolean, Optional ByVal celBottomBorder As String = "None") As PdfPCell
        Dim cell As New PdfPCell(phrase)
        If Cols > 1 Then cell.Colspan = Cols
        If celBorder Then
            If celBottomBorder <> "None" Then
                If celBottomBorder = "No" Then
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER
                    cell.BorderColor = BaseColor.BLACK
                Else
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER
                    cell.BorderColor = BaseColor.BLACK
                End If
            Else
                cell.BorderColor = BaseColor.BLACK
            End If
        Else
            cell.Border = Rectangle.NO_BORDER
        End If
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 0.5F
        cell.PaddingTop = 0.0F
        Return cell
    End Function
#End Region

#Region "Private Shared Function ImageCell(path As String, scale As Single, align As Integer) As PdfPCell"
    Private Shared Function ImageCell(ByVal path As String, ByVal scale As Single, ByVal align As Integer) As PdfPCell
        Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path))
        image.ScaleAbsolute(scale, scale)
        Dim cell As New PdfPCell(image)
        cell.BorderColor = BaseColor.WHITE
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 0.0F
        cell.PaddingTop = 0.0F
        Return cell
    End Function
#End Region

End Class
