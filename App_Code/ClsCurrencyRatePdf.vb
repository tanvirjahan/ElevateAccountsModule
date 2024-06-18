Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Public Class ClsCurrencyRatePdf
    Inherits System.Web.UI.Page
    Dim objutils As New clsUtils
#Region "global declaration"
    Dim objclsUtilities As New clsUtils
    Dim normalfont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
    Dim caption As Font = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.BLACK)
    Dim normalfontbold As Font = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)
    Dim headerfont As Font = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.BLACK)
    Dim Companyname As Font = New Font(FontFactory.GetFont("Times New Roman", 14, Font.BOLD, BaseColor.BLACK))
    Dim ReportNamefont As Font = New Font(FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK))

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


#Region "GenerateReport"
    Public Sub GenerateReport(ByVal fromcurr As String, ByVal tocurr As String, ByRef bytes() As Byte, ByVal printMode As String, Optional ByVal fileName As String = "")

        Try
            Dim rptType As Integer
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            Dim rptcompanyname, rptreportname As String, decno, currency, sqlString As String
            Dim custdetailsdt As New DataTable
            Dim divcode
            Dim arrHeaders() As String


            rptreportname = "Report -Currency Conversion Rates"

            If Not (String.IsNullOrEmpty(fromcurr)) And Not (String.IsNullOrEmpty(tocurr)) And Not (fromcurr.Trim = "[Select]") And Not (tocurr.Trim = "[Select]") Then
                sqlString = "SELECT currrates.currcode, currrates.tocurr, currrates.convrate FROM dbo.currrates currrates where currrates.currcode='" & fromcurr & "' And currrates.tocurr='" & tocurr & "'ORDER BY currrates.currcode"
                rptreportname = rptreportname & Environment.NewLine & "From Currency" & Space(2) & fromcurr & Space(2) & "To Currency" & Space(2) & tocurr
            ElseIf Not (String.IsNullOrEmpty(fromcurr)) And Not (fromcurr.Trim = "[Select]") Then
                sqlString = "SELECT currrates.currcode, currrates.tocurr, currrates.convrate FROM dbo.currrates currrates where currrates.currcode='" & fromcurr & "' ORDER BY currrates.currcode"
                rptreportname = rptreportname & Environment.NewLine & "From Currency" & Space(2) & fromcurr
            ElseIf Not (String.IsNullOrEmpty(tocurr)) And Not (tocurr.Trim = "[Select]") Then
                sqlString = "SELECT currrates.currcode, currrates.tocurr, currrates.convrate FROM dbo.currrates currrates where currrates.tocurr='" & tocurr & "' ORDER BY currrates.currcode"
                rptreportname = rptreportname & Environment.NewLine & "From Currency" & Space(2) & fromcurr

            Else
                sqlString = "SELECT currrates.currcode, currrates.tocurr, currrates.convrate FROM dbo.currrates currrates ORDER BY currrates.currcode"
            End If
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")

            Using ds1 As New SqlDataAdapter(sqlString, sqlConn)
                ds1.Fill(custdetailsdt)
            End Using


            If divcode <> "" Then
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
            Else
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
            End If


            decno = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
            decno = "N" + decno

            currency = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)

            Dim documentWidth As Single
            '  Dim document As New Document(PageSize.A4, -10.0F, 10.0F, 30.0F, 35.0F)
            Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 20.0F, 40.0F)
            ' document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)
            documentWidth = 550.0F

            Using memoryStream As New System.IO.MemoryStream()
                Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)

                Dim phrase As Phrase = Nothing
                Dim cell As PdfPCell = Nothing

                Dim titletable As PdfPTable = Nothing

                titletable = New PdfPTable(1)
                titletable.TotalWidth = documentWidth
                titletable.LockedWidth = True
                titletable.SetWidths(New Single() {1.0F})

                titletable.Complete = False
                titletable.SplitRows = False
                'company name
                phrase = New Phrase()
                phrase.Add(New Chunk(rptcompanyname, Companyname))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                cell.SetLeading(12, 0)
                cell.BorderWidthBottom = 0
                cell.PaddingBottom = 10
                cell.PaddingLeft = 5
                cell.PaddingTop = 0
                titletable.AddCell(cell)

                phrase = New Phrase()
                phrase.Add(New Chunk(rptreportname, ReportNamefont))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.BorderWidthTop = 0
                cell.PaddingBottom = 18
                cell.PaddingLeft = 5
                cell.SetLeading(12, 1)
                titletable.AddCell(cell)
                titletable.Complete = True


                'Dim Reporttitle = New PdfPTable(1)
                'Reporttitle.TotalWidth = documentWidth
                'Reporttitle.LockedWidth = True
                'Reporttitle.SetWidths(New Single() {1.0F})
                'Reporttitle.Complete = False
                'Reporttitle.SplitRows = False
                'phrase = New Phrase()
                'phrase.Add(New Chunk(rptreportname, ReportNamefont))
                'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                'cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                'cell.PaddingBottom = 4
                'cell.Colspan = 2
                'cell.SetLeading(12, 0)
                'Reporttitle.AddCell(cell)
                'Reporttitle.SpacingBefore = 5
                'Reporttitle.SpacingAfter = 5
                'Reporttitle.Complete = True

                Dim tableTitle As PdfPTable = Nothing
                tableTitle = New PdfPTable(3)
                tableTitle.TotalWidth = 300.0F
                tableTitle.HorizontalAlignment = Element.ALIGN_LEFT

                tableTitle.LockedWidth = True
                tableTitle.SplitRows = False
                'tableTitle.KeepTogether = True
                tableTitle.SpacingBefore = 10
                tableTitle.SetWidths(New Single() {0.5F, 0.25F, 0.25F})
                arrHeaders = {"From Currency", "Conversion", "To Currency"}

                For i = 0 To arrHeaders.Length - 1
                    phrase = New Phrase()
                    phrase.Add(New Chunk(arrHeaders(i), normalfontbold))

                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4.0F
                    cell.PaddingTop = 1.0F
                    tableTitle.AddCell(cell)
                Next
                tableTitle.Complete = True

                Dim FooterTable = New PdfPTable(1)
                FooterTable.TotalWidth = documentWidth
                FooterTable.LockedWidth = True
                FooterTable.SetWidths(New Single() {1.0F})
                FooterTable.Complete = False
                FooterTable.SplitRows = False
                phrase = New Phrase()
                phrase.Add(New Chunk("Printed Date: " & Date.Now.ToString("dd/MM/yyyy"), normalfont))
                cell = New PdfPCell(phrase)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT

                cell.SetLeading(12, 0)
                cell.PaddingBottom = 3
                FooterTable.SpacingBefore = 12.0F
                FooterTable.AddCell(cell)
                FooterTable.Complete = True

                writer.PageEvent = New ClsHeaderFooter(titletable, Nothing, FooterTable, tableTitle)
                document.Open()

                Dim code As Integer
                Dim description, rowCode, rowName As String
                If custdetailsdt.Rows.Count > 0 Then
                    Dim tableData = New PdfPTable(3)
                    tableData.TotalWidth = 300.0F
                    tableData.HorizontalAlignment = Element.ALIGN_LEFT
                    tableData.LockedWidth = True
                    tableData.SplitRows = False
                    tableData.SetWidths(New Single() {0.5F, 0.25F, 0.25F})
                    tableData.SpacingBefore = 0

                    Dim groups As IEnumerable(Of IGrouping(Of String, DataRow)) = custdetailsdt.AsEnumerable.GroupBy(Function(g) g.Field(Of String)("currcode"))

                    For Each grpby In groups

                        arrHeaders = {grpby.Key}
                        For i = 0 To arrHeaders.Length - 1

                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 8.0F
                            cell.Colspan = 3
                            tableData.AddCell(cell)
                        Next
                        For Each row As DataRow In grpby
                            arrHeaders = {"1" & Space(2) & row("currcode") & Space(2) & "=", Decimal.Parse(row("convrate")).ToString("N6"), row("tocurr")}
                            For i = 0 To arrHeaders.Length - 1
                                phrase = New Phrase()
                                phrase.Add(New Chunk(arrHeaders(i), normalfont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.SetLeading(12, 0)
                                cell.PaddingBottom = 4.0F
                                '  cell.PaddingTop = 1.0F
                                tableData.AddCell(cell)
                            Next
                        Next
                    Next
                    document.Add(tableData)

                End If

                document.AddTitle(rptreportname)
                document.Close()

                If printMode = "download" Then
                    Dim pagingFont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)
                    Dim reader As New PdfReader(memoryStream.ToArray())
                    Using mStream As New MemoryStream()
                        Using stamper As New PdfStamper(reader, mStream)
                            Dim pages As Integer = reader.NumberOfPages
                            For i As Integer = 1 To pages
                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 530.0F, 10.0F, 0)
                            Next
                        End Using
                        bytes = mStream.ToArray()
                    End Using
                End If
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

End Class
