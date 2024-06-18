Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq
Imports System.Globalization
Imports ClosedXML.Excel



Public Class clsSerialityControl
    Inherits System.Web.UI.Page

#Region "global declaration"
    Dim objutils As New clsUtils
    Dim normalfont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
    Dim caption As Font = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.BLACK)
    Dim normalfontbold As Font = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)
    Dim headerfont As Font = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.BLACK)
    Dim Companyname As Font = New Font(FontFactory.GetFont("Times New Roman", 14, Font.BOLD, BaseColor.WHITE))
    Dim ReportNamefont As Font = New Font(FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.WHITE))
    Dim rptcompanyname, rptreportname, fromname, rptfilter, currname, decno As String
    Dim documentWidth As Single = 550.0F
    Dim debit, credit, totaldebit, totalcredit As Decimal
    Dim phrase As Phrase = Nothing
    Dim cell As PdfPCell = Nothing
    Dim totalbg As BaseColor = New BaseColor(223, 223, 223)
#End Region

#Region "GenerateReport"
    Public Sub GenerateReport(ByVal reportsType As String, ByVal fromVal As String, ByVal toVal As String, ByVal divcode As String, ByRef bytes() As Byte, ByVal printMode As String)

        Try
            If divcode <> "" Then
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
            Else
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
            End If
            rptreportname = "Seriality Control Report"
            rptfilter = "Booking From" & Space(1) & Convert.ToString(fromVal) & Space(1) & " To" & Space(1) & Convert.ToString(toVal)

            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("sp_seriality", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 20)).Value = divcode
            mySqlCmd.Parameters.Add(New SqlParameter("@fromrequestid", SqlDbType.VarChar, 20)).Value = fromVal
            mySqlCmd.Parameters.Add(New SqlParameter("@torequestid", SqlDbType.VarChar, 20)).Value = toVal
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim serialDt As New DataTable
            serialDt = ds.Tables(0)

            Dim arrHeaders() As String = Nothing
            arrHeaders = {"Booking No", "Date", "Agency", "Arrival Date", "Invoice No", "LPO No"}

            If reportsType = "excel" Then
                ExcelReport(serialDt, arrHeaders, bytes)
            Else
                Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 20.0F, 35.0F)
                Dim documentWidth As Single = 550.0F

                Using memoryStream As New System.IO.MemoryStream()
                    Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)

                    Dim titletable As PdfPTable = Nothing

                    Dim CompanybgColor As BaseColor = New BaseColor(0, 72, 192)
                    Dim ReportNamebgColor As BaseColor = New BaseColor(0, 128, 192)

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
                    cell.PaddingBottom = 4
                    cell.BackgroundColor = CompanybgColor
                    titletable.AddCell(cell)

                    Dim Reporttitle = New PdfPTable(1)
                    Reporttitle.TotalWidth = documentWidth
                    Reporttitle.LockedWidth = True
                    Reporttitle.SetWidths(New Single() {1.0F})
                    Reporttitle.Complete = False
                    Reporttitle.SplitRows = False
                    phrase = New Phrase()
                    phrase.Add(New Chunk(rptreportname, ReportNamefont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.PaddingBottom = 4
                    cell.SetLeading(12, 0)
                    cell.BackgroundColor = ReportNamebgColor
                    Reporttitle.SpacingBefore = 5
                    Reporttitle.SpacingAfter = 5
                    Reporttitle.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(rptfilter, headerfont))
                    cell = New PdfPCell(phrase)
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 5
                    cell.PaddingTop = 10
                    Reporttitle.AddCell(cell)

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
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 3
                    FooterTable.SpacingBefore = 12.0F
                    FooterTable.AddCell(cell)
                    FooterTable.Complete = True

                    writer.PageEvent = New ClsHeaderFooter(titletable, Reporttitle, FooterTable, Nothing, "printDate")

                    document.Open()

                    If serialDt.Rows.Count > 0 Then
                        Dim tableTitle As PdfPTable = Nothing
                        tableTitle = New PdfPTable(6)
                        tableTitle.SetWidths(New Single() {0.15F, 0.11F, 0.33F, 0.11F, 0.15F, 0.15F})
                        tableTitle.TotalWidth = documentWidth
                        tableTitle.HeaderRows = 1
                        tableTitle.LockedWidth = True
                        tableTitle.SplitRows = False
                        tableTitle.Complete = False
                        For i = 0 To arrHeaders.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.SetLeading(12, 0)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            tableTitle.AddCell(cell)
                        Next
                        For Each serialDr In serialDt.Rows

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(serialDr("requestid")), normalfont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            tableTitle.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToDateTime(serialDr("requestdate")).ToString("dd/MM/yyyy"), normalfont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            tableTitle.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(serialDr("agentname")), normalfont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            tableTitle.AddCell(cell)

                            Dim arrivalDate As String
                            If IsDate(serialDr("checkin")) Then
                                arrivalDate = Convert.ToDateTime(serialDr("checkin")).ToString("dd/MM/yyyy")
                            Else
                                arrivalDate = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrivalDate, normalfont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            tableTitle.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(serialDr("invoiceno")), normalfont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            tableTitle.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(serialDr("purchaseinvoiceno")), normalfont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            tableTitle.AddCell(cell)

                        Next
                        tableTitle.Complete = True
                        document.Add(tableTitle)

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
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), documentWidth, 10.0F, 0)
                                Next
                            End Using
                            bytes = mStream.ToArray()
                        End Using
                    End If
                End Using
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

#Region "ExcelReport"
    Public Sub ExcelReport(ByVal serialDt As DataTable, ByVal arrHeaders() As String, ByRef bytes() As Byte)
        Try

            Dim col As Integer
            Dim companyCol, reportCol, filtercol As String
            Dim wbook As New XLWorkbook
            Dim ws = wbook.Worksheets.Add("SerialityControl")

            'Page Margins
            ws.PageSetup.Margins.Top = 0.75
            ws.PageSetup.Margins.Bottom = 0.75
            ws.PageSetup.Margins.Left = 0.25
            ws.PageSetup.Margins.Right = 0.25
            ws.PageSetup.Margins.Header = 0.3
            ws.PageSetup.Margins.Footer = 0.3

            ' Header and Footer
            ws.PageSetup.Footer.Right.AddText("Page" & Space(2), XLHFOccurrence.AllPages)
            ws.PageSetup.Footer.Right.AddText(XLHFPredefinedText.PageNumber, XLHFOccurrence.AllPages)
            ws.PageSetup.Footer.Right.AddText(Space(2) & "of" & Space(2), XLHFOccurrence.AllPages)
            ws.PageSetup.Footer.Right.AddText(XLHFPredefinedText.NumberOfPages, XLHFOccurrence.AllPages)
            ws.PageSetup.Footer.Left.AddText(XLHFPredefinedText.Date, XLHFOccurrence.AllPages)
            ws.PageSetup.PaperSize = XLPaperSize.A4Paper
            ws.PageSetup.SetRowsToRepeatAtTop(1, 3)


            Dim rowcount As Integer = 4
            ws.Columns.AdjustToContents()
            ws.Column("A").Width = 17
            ws.Column("B").Width = 14
            ws.Column("C").Width = 44
            ws.Columns("D").Width = 14
            ws.Columns("E").Width = 17
            ws.Columns("F").Width = 17

            companyCol = "A2:F2"
            reportCol = "A3:F3"
            filtercol = "A4:F4"

            'Comapny Name Heading
            ws.Cell("A2").Value = rptcompanyname
            Dim company = ws.Range(companyCol).Merge()
            company.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromHtml("#0048C0"))
            company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
            company.Style.Font.FontSize = 15
            company.Style.Font.FontColor = XLColor.White
            company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            'Report Name Heading
            ws.Cell("A3").Value = rptreportname
            Dim report = ws.Range(reportCol).Merge()
            report.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromHtml("#0080C0"))
            report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
            report.Style.Font.FontSize = 14
            report.Style.Font.FontColor = XLColor.White
            report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center


            'filter Name Heading
            ws.Cell("A4").Value = rptfilter
            Dim filter = ws.Range(filtercol).Merge()
            filter.Style.Font.SetBold()
            filter.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
            filter.Style.Font.FontSize = 12
            filter.Style.Font.FontColor = XLColor.Black
            filter.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
            filter.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            If serialDt.Rows.Count > 0 Then
                rowcount = rowcount + 2
                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Alignment.WrapText = True
                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.Bold = True
                For i = 0 To arrHeaders.Length - 1
                    ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
                    ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                Next
                
                For Each serialDr As DataRow In serialDt.Rows
                    rowcount = rowcount + 1
                    ws.Cell(rowcount, 1).Value = "'" + Convert.ToString(serialDr("requestid"))
                    ws.Cell(rowcount, 2).Value = Convert.ToDateTime(serialDr("requestdate")).ToString("dd/MM/yyyy")
                    ws.Cell(rowcount, 3).Value = Convert.ToString(serialDr("agentName"))
                    Dim arrivalDate As String
                    If IsDate(serialDr("checkin")) Then
                        arrivalDate = Convert.ToDateTime(serialDr("checkin")).ToString("dd/MM/yyyy")
                    Else
                        arrivalDate = ""
                    End If
                    ws.Cell(rowcount, 4).Value = arrivalDate
                    ws.Cell(rowcount, 5).Value = Convert.ToString(serialDr("invoiceno"))
                    ws.Cell(rowcount, 6).Value = Convert.ToString(serialDr("purchaseinvoiceno"))

                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Alignment.WrapText = True
                Next
            End If

            ws.Cell((rowcount + 2), 1).Value = "Printed Date:" & Now.ToString("dd/MM/yyyy")
            ws.Range((rowcount + 2), 1, (rowcount + 2), 3).Merge()
            Using wStream As New MemoryStream()
                wbook.SaveAs(wStream)
                bytes = wStream.ToArray()
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

End Class
