Imports Microsoft.VisualBasic
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class clsJournalPdfPageEvents : Implements IPdfPageEvent
    Dim titleJournal As PdfPTable
    Dim strPrintMode As String
    Dim _xPosition As Single = 300.0F
    Public Sub New(ByVal TitleDt As PdfPTable, ByVal printMode As String)
        titleJournal = TitleDt
        strPrintMode = printMode
    End Sub
    Public Sub OnChapter(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document, ByVal paragraphPosition As Single, ByVal title As iTextSharp.text.Paragraph) Implements iTextSharp.text.pdf.IPdfPageEvent.OnChapter

    End Sub

    Public Sub OnChapterEnd(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document, ByVal paragraphPosition As Single) Implements iTextSharp.text.pdf.IPdfPageEvent.OnChapterEnd

    End Sub

    Public Sub OnCloseDocument(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document) Implements iTextSharp.text.pdf.IPdfPageEvent.OnCloseDocument

    End Sub

    Public Sub OnEndPage(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document) Implements iTextSharp.text.pdf.IPdfPageEvent.OnEndPage
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

    Public Sub OnGenericTag(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document, ByVal rect As iTextSharp.text.Rectangle, ByVal text As String) Implements iTextSharp.text.pdf.IPdfPageEvent.OnGenericTag

    End Sub

    Public Sub OnOpenDocument(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document) Implements iTextSharp.text.pdf.IPdfPageEvent.OnOpenDocument

    End Sub

    Public Sub OnParagraph(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document, ByVal paragraphPosition As Single) Implements iTextSharp.text.pdf.IPdfPageEvent.OnParagraph

    End Sub

    Public Sub OnParagraphEnd(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document, ByVal paragraphPosition As Single) Implements iTextSharp.text.pdf.IPdfPageEvent.OnParagraphEnd

    End Sub

    Public Sub OnSection(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document, ByVal paragraphPosition As Single, ByVal depth As Integer, ByVal title As iTextSharp.text.Paragraph) Implements iTextSharp.text.pdf.IPdfPageEvent.OnSection

    End Sub

    Public Sub OnSectionEnd(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document, ByVal paragraphPosition As Single) Implements iTextSharp.text.pdf.IPdfPageEvent.OnSectionEnd

    End Sub

    Public Sub OnStartPage(ByVal writer As iTextSharp.text.pdf.PdfWriter, ByVal document As iTextSharp.text.Document) Implements iTextSharp.text.pdf.IPdfPageEvent.OnStartPage
        Try
            If writer.PageNumber > 1 Then
                titleJournal.SpacingAfter = 12
                document.Add(titleJournal)
            End If
        Catch docEx As DocumentException
            Throw docEx
        End Try
    End Sub
End Class
