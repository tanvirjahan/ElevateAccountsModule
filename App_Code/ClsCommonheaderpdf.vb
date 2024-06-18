Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq

Public Class clscommonheaderpdf : Implements IPdfPageEvent
    Dim _xPosition As Single = 300.0F
    Dim headertbl As PdfPTable
    Dim tableheader As PdfPTable
    Dim header As String
    Dim headerfont As Font = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.BLACK)

    Dim normalfont As Font = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.BLACK)
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
    Private Shared Function ImageCell(ByVal path As String, ByVal scalewidth As Single, ByVal scaleheight As Single, ByVal align As Integer) As PdfPCell
        Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance("D:\\Columbuscommon\\images\\logo1.png")
        image.ScaleAbsolute(scalewidth, scaleheight)
        Dim cell As New PdfPCell(image)
        cell.BorderColor = BaseColor.WHITE
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 0.0F
        cell.PaddingTop = 0.0F
        Return cell
    End Function
    'Dim watermark As String
    'Dim titleInvDt As PdfPTable
    'Dim strPrintMode As String
    'Dim _xPosition As Single = 300.0F
    Public Sub New(ByVal tableheader As String)
        Header = tableheader
        'strPrintMode = printMode
    End Sub

    Public Sub OnOpenDocument(writer As PdfWriter, document As Document) Implements IPdfPageEvent.OnOpenDocument

    End Sub
    Public Sub OnCloseDocument(writer As PdfWriter, document As Document) Implements IPdfPageEvent.OnCloseDocument

    End Sub

    Public Sub OnStartPage(writer As PdfWriter, document As Document) Implements IPdfPageEvent.OnStartPage
        Try
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)
            Dim documentWidth As Single = 770.0F
            Dim phrase As Phrase = Nothing
            Dim cell As PdfPCell = Nothing
            If writer.PageNumber > 0 Then
                tableheader = New PdfPTable(1)
                tableheader.TotalWidth = documentWidth
                tableheader.LockedWidth = True

                tableheader.SetWidths(New Single() {1.0F}) '    
                tableheader.Complete = False
                tableheader.SplitRows = False
                tableheader.SpacingBefore = 30.0F
                tableheader.WidthPercentage = 100
                tableheader.SpacingAfter = 10.0F
                cell = ImageCell("~/Images/Logo.bmp", 150.0F, 80.0F, PdfPCell.ALIGN_LEFT)
                tableheader.AddCell(cell)


                Phrase = New Phrase()
                phrase.Add(New Chunk(header.ToString(), headerfont))
                cell = PhraseCell(Phrase, PdfPCell.ALIGN_CENTER, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                cell.PaddingBottom = 4.0F
                cell.PaddingTop = 1.0F
                tableheader.AddCell(cell)
                document.Add(tableheader)
            End If
        Catch docEx As DocumentException
            Throw docEx
        End Try
    End Sub
    Public Sub OnEndPage(writer As PdfWriter, document As Document) Implements IPdfPageEvent.OnEndPage
        Try
            If writer.PageNumber > 0 Then
                '    Dim phrase As Phrase = Nothing
                '    Dim cell As PdfPCell = Nothing
                '    Dim tablefooter As PdfPTable = New PdfPTable(1)
                '    phrase = New Phrase()
                '    phrase.Add(New Chunk(" M-1 / Deira House Building", headerfont))
                '    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                '    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                '    cell.PaddingBottom = 4.0F
                '    cell.PaddingTop = 1.0F
                '    tablefooter.AddCell(cell)

                '    phrase = New Phrase()
                '    phrase.Add(New Chunk("Tel (Office): +971 (4) 3888222 Tel (Office): +971 (4) 3888222", normalfont))
                '    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                '    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                '    cell.PaddingBottom = 4.0F
                '    cell.PaddingTop = 1.0F
                '    tablefooter.AddCell(cell)

                '    phrase = New Phrase()
                '    phrase.Add(New Chunk("5th floor, shaik zayed road,dubai U.A.E", headerfont))
                '    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                '    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                '    cell.PaddingBottom = 4.0F
                '    cell.PaddingTop = 1.0F
                '    tablefooter.AddCell(cell)
                '    document.Add(tablefooter)
            End If
        Catch docEx As DocumentException
            Throw docEx
        End Try


        'Try
        ' document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)
        ' Dim documentWidth As Single = 770.0F
        '  If writer.PageNumber > 1 Then

        '   document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)
        ' Dim phrase As Phrase = Nothing
        ' Dim cell As PdfPCell = Nothing
        '  Dim table As PdfPTable = Nothing
        ' Dim documentWidth As Single = 700.0F
        '  document.Add(tablefooterl)
        '  End If
        '

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

