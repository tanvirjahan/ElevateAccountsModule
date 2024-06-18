Imports Microsoft.VisualBasic
Imports iTextSharp.text.pdf
Imports iTextSharp.text

Public Class Party_ledger : Implements IPdfPageEvent

    Dim rptcompanyname As String
    Dim rptreportname, addrLine1, addrLine2, addrLine3, addrLine4, addrLine5 As String
    Dim phrase As Phrase = Nothing
    Dim cell As PdfPCell = Nothing
    Dim documentWidth As Single = 770.0F
    '  Dim Reporttitle = New PdfPTable(1)
    Dim Reporttitle As PdfPTable = Nothing
    Dim titletable As PdfPTable = Nothing

    Dim FooterTable As PdfPTable = Nothing
    Public Sub New(ByVal companyname As PdfPTable, ByVal report As PdfPTable, ByVal footer As PdfPTable)
        titletable = companyname
        Reporttitle = report
        FooterTable = footer

    End Sub

    Public Sub OnOpenDocument(ByVal writer As PdfWriter, ByVal document As Document) Implements IPdfPageEvent.OnOpenDocument

    End Sub
    Public Sub OnCloseDocument(ByVal writer As PdfWriter, ByVal document As Document) Implements IPdfPageEvent.OnCloseDocument

    End Sub

    Public Sub OnStartPage(ByVal writer As PdfWriter, ByVal document As Document) Implements IPdfPageEvent.OnStartPage
        Try
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)

            If writer.PageNumber >= 0 Then
                document.Add(titletable)
                document.Add(Reporttitle)
                FooterTable.WriteSelectedRows(0, -1, 30, 40, writer.DirectContent)
                '  document.Add(FooterTable)
            End If
        Catch docEx As DocumentException
            Throw docEx
        End Try
    End Sub
    Public Sub OnEndPage(ByVal writer As PdfWriter, ByVal document As Document) Implements IPdfPageEvent.OnEndPage
        Try


            'End If
        Catch docEx As DocumentException
            Throw docEx
        End Try




    End Sub

    Private Shared Function PhraseCell(ByVal phrase As Phrase, ByVal align As Integer, ByVal Cols As Integer, ByVal celBorder As Boolean, Optional ByVal celBottomBorder As String = "None") As PdfPCell
        Dim cell As PdfPCell = New PdfPCell(phrase)
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

    Public Sub OnParagraph(ByVal writer As PdfWriter, ByVal document As Document, ByVal paragraphPosition As Single) Implements IPdfPageEvent.OnParagraph

    End Sub
    Public Sub OnParagraphEnd(ByVal writer As PdfWriter, ByVal document As Document, ByVal paragraphPosition As Single) Implements IPdfPageEvent.OnParagraphEnd

    End Sub
    Public Sub OnChapter(ByVal writer As PdfWriter, ByVal document As Document, ByVal paragraphPosition As Single, ByVal title As Paragraph) Implements IPdfPageEvent.OnChapter

    End Sub
    Public Sub OnChapterEnd(ByVal writer As PdfWriter, ByVal document As Document, ByVal paragraphPosition As Single) Implements IPdfPageEvent.OnChapterEnd

    End Sub
    Public Sub OnSection(ByVal writer As PdfWriter, ByVal document As Document, ByVal paragraphPosition As Single, ByVal depth As Integer, ByVal title As Paragraph) Implements IPdfPageEvent.OnSection

    End Sub
    Public Sub OnSectionEnd(ByVal writer As PdfWriter, ByVal document As Document, ByVal paragraphPosition As Single) Implements IPdfPageEvent.OnSectionEnd

    End Sub
    Public Sub OnGenericTag(ByVal writer As PdfWriter, ByVal document As Document, ByVal rect As Rectangle, ByVal text As String) Implements IPdfPageEvent.OnGenericTag

    End Sub



    'Private acc_type As String
    'Public Property _acc_type() As String
    '    Get
    '        Return acc_type
    '    End Get
    '    Set(ByVal value As String)
    '        acc_type = value
    '    End Set
    'End Property

    'Private acc_code As String
    'Public Property _acc_code() As String
    '    Get
    '        Return acc_code
    '    End Get
    '    Set(ByVal value As String)
    '        acc_code = value
    '    End Set
    'End Property

    'Private acc_gl_code As String
    'Public Property _acc_gl_code() As String
    '    Get
    '        Return acc_gl_code
    '    End Get
    '    Set(ByVal value As String)
    '        acc_gl_code = value
    '    End Set
    'End Property



    'Private acc_name As String
    'Public Property _acc_name() As String
    '    Get
    '        Return acc_name
    '    End Get
    '    Set(ByVal value As String)
    '        acc_name = value
    '    End Set
    'End Property


    'Private currcode As String
    'Public Property _currcode() As String
    '    Get
    '        Return currcode
    '    End Get
    '    Set(ByVal value As String)
    '        currcode = value
    '    End Set
    'End Property

    'Private tranid As String
    'Public Property _tranid() As String
    '    Get
    '        Return tranid
    '    End Get
    '    Set(ByVal value As String)
    '        tranid = value
    '    End Set
    'End Property

    'Private trantype As String
    'Public Property _trantype() As String
    '    Get
    '        Return trantype
    '    End Get
    '    Set(ByVal value As String)
    '        trantype = value
    '    End Set
    'End Property


    'Private trandate As Date


    'Public Property _trandate() As Date
    '    Get
    '        Return trandate
    '    End Get
    '    Set(ByVal value As Date)
    '        trandate = value
    '    End Set
    'End Property



    'Private refno As String
    'Public Property _refno() As String
    '    Get
    '        Return refno
    '    End Get
    '    Set(ByVal value As String)
    '        refno = value
    '    End Set
    'End Property



    'Private narration As String
    'Public Property _narration() As String
    '    Get
    '        Return narration
    '    End Get
    '    Set(ByVal value As String)
    '        narration = value
    '    End Set
    'End Property


    'Private mode As Integer
    'Public Property _mode() As Integer
    '    Get
    '        Return mode
    '    End Get
    '    Set(ByVal value As Integer)
    '        mode = value
    '    End Set
    'End Property

    'Private debit As Decimal
    'Public Property _debit() As Decimal
    '    Get
    '        Return debit
    '    End Get
    '    Set(ByVal value As Decimal)
    '        debit = value
    '    End Set
    'End Property

    'Private credit As Decimal
    'Public Property _credit() As Decimal
    '    Get
    '        Return credit
    '    End Get
    '    Set(ByVal value As Decimal)
    '        credit = value
    '    End Set
    'End Property


    'Private AdjustedBillRef As String
    'Public Property _AdjustedBillRef() As String
    '    Get
    '        Return AdjustedBillRef
    '    End Get
    '    Set(ByVal value As String)
    '        AdjustedBillRef = value
    '    End Set
    'End Property

    'Private div_id As String
    'Public Property _div_id() As String
    '    Get
    '        Return div_id
    '    End Get
    '    Set(ByVal value As String)
    '        div_id = value
    '    End Set
    'End Property

    'Private acctname As String
    'Public Property _acctname() As String
    '    Get
    '        Return acctname
    '    End Get
    '    Set(ByVal value As String)
    '        acctname = value
    '    End Set
    'End Property

    'Private acctname As String
    'Public Property _acctname() As String
    '    Get
    '        Return acctname
    '    End Get
    '    Set(ByVal value As String)
    '        acctname = value
    '    End Set
    'End Property

    'Private companyname As String
    'Public Property _companyname() As String
    '    Get
    '        Return companyname
    '    End Get
    '    Set(ByVal value As String)
    '        companyname = value
    '    End Set
    'End Property

    'Private reportname As String
    'Public Property _reportname() As String
    '    Get
    '        Return reportname
    '    End Get
    '    Set(ByVal value As String)
    '        reportname = value
    '    End Set
    'End Property

    'Private addrLine1 As String
    'Public Property _addrLine1() As String
    '    Get
    '        Return addrLine1
    '    End Get
    '    Set(ByVal value As String)
    '        addrLine1 = value
    '    End Set
    'End Property

    'Private addrLine2 As String
    'Public Property _addrLine2() As String
    '    Get
    '        Return addrLine2
    '    End Get
    '    Set(ByVal value As String)
    '        addrLine2 = value
    '    End Set
    'End Property
    'Private addrLine3 As String
    'Public Property _addrLine3() As String
    '    Get
    '        Return addrLine3
    '    End Get
    '    Set(ByVal value As String)
    '        addrLine3 = value
    '    End Set
    'End Property
    'Private addrLine4 As String
    'Public Property _addrLine4() As String
    '    Get
    '        Return addrLine4
    '    End Get
    '    Set(ByVal value As String)
    '        addrLine4 = value
    '    End Set
    'End Property
    'Private addrLine5 As String
    'Public Property _addrLine5() As String
    '    Get
    '        Return addrLine5
    '    End Get
    '    Set(ByVal value As String)
    '        addrLine5 = value
    '    End Set
    'End Property

End Class
