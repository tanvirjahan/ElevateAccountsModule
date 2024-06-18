Imports Microsoft.VisualBasic
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class clsHotelVoucherPageEvents : Implements IPdfPageEvent
    Dim strPrintMode As String
    Dim _xPosition As Single = 300.0F
    Public Sub New(ByVal printMode As String)
        strPrintMode = printMode
    End Sub

    Public Sub OnChapter(writer As iTextSharp.text.pdf.PdfWriter, document As iTextSharp.text.Document, paragraphPosition As Single, title As iTextSharp.text.Paragraph) Implements iTextSharp.text.pdf.IPdfPageEvent.OnChapter

    End Sub

    Public Sub OnChapterEnd(writer As iTextSharp.text.pdf.PdfWriter, document As iTextSharp.text.Document, paragraphPosition As Single) Implements iTextSharp.text.pdf.IPdfPageEvent.OnChapterEnd

    End Sub

    Public Sub OnCloseDocument(writer As iTextSharp.text.pdf.PdfWriter, document As iTextSharp.text.Document) Implements iTextSharp.text.pdf.IPdfPageEvent.OnCloseDocument

    End Sub

    Public Sub OnEndPage(writer As iTextSharp.text.pdf.PdfWriter, document As iTextSharp.text.Document) Implements iTextSharp.text.pdf.IPdfPageEvent.OnEndPage
        If strPrintMode = "SaveServer" Then
            Dim cb As PdfContentByte = writer.DirectContentUnder
            Dim baseFont As BaseFont = baseFont.CreateFont(baseFont.HELVETICA, baseFont.WINANSI, baseFont.EMBEDDED)
            cb.BeginText()
            cb.SetColorFill(BaseColor.GRAY)
            cb.SetFontAndSize(baseFont, 12)
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Page " + Str(writer.PageNumber), _xPosition, 20, 0)
            cb.EndText()
        End If
    End Sub

    Public Sub OnGenericTag(writer As iTextSharp.text.pdf.PdfWriter, document As iTextSharp.text.Document, rect As iTextSharp.text.Rectangle, text As String) Implements iTextSharp.text.pdf.IPdfPageEvent.OnGenericTag

    End Sub

    Public Sub OnOpenDocument(writer As iTextSharp.text.pdf.PdfWriter, document As iTextSharp.text.Document) Implements iTextSharp.text.pdf.IPdfPageEvent.OnOpenDocument

    End Sub

    Public Sub OnParagraph(writer As iTextSharp.text.pdf.PdfWriter, document As iTextSharp.text.Document, paragraphPosition As Single) Implements iTextSharp.text.pdf.IPdfPageEvent.OnParagraph

    End Sub

    Public Sub OnParagraphEnd(writer As iTextSharp.text.pdf.PdfWriter, document As iTextSharp.text.Document, paragraphPosition As Single) Implements iTextSharp.text.pdf.IPdfPageEvent.OnParagraphEnd

    End Sub

    Public Sub OnSection(writer As iTextSharp.text.pdf.PdfWriter, document As iTextSharp.text.Document, paragraphPosition As Single, depth As Integer, title As iTextSharp.text.Paragraph) Implements iTextSharp.text.pdf.IPdfPageEvent.OnSection

    End Sub

    Public Sub OnSectionEnd(writer As iTextSharp.text.pdf.PdfWriter, document As iTextSharp.text.Document, paragraphPosition As Single) Implements iTextSharp.text.pdf.IPdfPageEvent.OnSectionEnd

    End Sub

    Public Sub OnStartPage(writer As iTextSharp.text.pdf.PdfWriter, document As iTextSharp.text.Document) Implements iTextSharp.text.pdf.IPdfPageEvent.OnStartPage

    End Sub
End Class
