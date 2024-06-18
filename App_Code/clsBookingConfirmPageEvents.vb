Imports Microsoft.VisualBasic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Public Class clsBookingConfirmPageEvents : Implements IPdfPageEvent
    Dim watermark As String
    Dim titleInvDt As PdfPTable
    Dim strPrintMode As String
    Dim _xPosition As Single = 300.0F
    Public Sub New(ByVal invTitleDt As PdfPTable, ByVal printMode As String)
        titleInvDt = invTitleDt
        strPrintMode = printMode
    End Sub

    Public Sub OnOpenDocument(writer As PdfWriter, document As Document) Implements IPdfPageEvent.OnOpenDocument

    End Sub
    Public Sub OnCloseDocument(writer As PdfWriter, document As Document) Implements IPdfPageEvent.OnCloseDocument

    End Sub
    Public Sub OnStartPage(writer As PdfWriter, document As Document) Implements IPdfPageEvent.OnStartPage
        Try
            If writer.PageNumber > 1 Then
                titleInvDt.SpacingAfter = 7
                document.Add(titleInvDt)
            End If
        Catch docEx As DocumentException
            Throw docEx
        End Try
    End Sub
    Protected Sub OnEndPage(writer As PdfWriter, document As Document) Implements IPdfPageEvent.OnEndPage
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
    Public Sub OnParagraph(writer As PdfWriter, document As Document, paragraphPosition As Single) Implements IPdfPageEvent.OnParagraph

    End Sub
    Public Sub OnParagraphEnd(writer As PdfWriter, document As Document, paragraphPosition As Single) Implements IPdfPageEvent.OnParagraphEnd

    End Sub
    Public Sub OnChapter(writer As PdfWriter, document As Document, paragraphPosition As Single, title As Paragraph) Implements IPdfPageEvent.OnChapter

    End Sub
    Public Sub OnChapterEnd(writer As PdfWriter, document As Document, paragraphPosition As Single) Implements IPdfPageEvent.OnChapterEnd

    End Sub
    Public Sub OnSection(writer As PdfWriter, document As Document, paragraphPosition As Single, depth As Integer, title As Paragraph) Implements IPdfPageEvent.OnSection

    End Sub
    Public Sub OnSectionEnd(writer As PdfWriter, document As Document, paragraphPosition As Single) Implements IPdfPageEvent.OnSectionEnd

    End Sub
    Public Sub OnGenericTag(writer As PdfWriter, document As Document, rect As Rectangle, text As String) Implements IPdfPageEvent.OnGenericTag

    End Sub
End Class
