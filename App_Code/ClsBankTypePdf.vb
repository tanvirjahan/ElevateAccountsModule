Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Collections.Generic
Imports System.IO
Imports ClosedXML.Excel

Public Class ClsBankTypePdf
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
    Dim rptcompanyname, rptreportname, decno, currency As String
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
    Public Sub GenerateReport(ByVal reportsType As String, ByVal printId As String, ByRef bytes() As Byte, ByVal printMode As String, Optional ByVal fileName As String = "")

        Try
            Dim rptType As Integer
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            Dim sqlString As String
            Dim custdetailsdt As New DataTable
            Dim divcode
            Dim arrHeaders() As String

            'If Bank Type then set 0 for Other Bank Master Report set 1. 
            rptType = IIf(printId = "Bank Type", 0, 1)
            If printId = "Bank Type" Then
                sqlString = "SELECT bank_master_type.bank_master_type_code, bank_master_type.bank_master_type_des FROM  dbo.bank_master_type bank_master_type"
                rptreportname = "Report - Bank Master Type"
                arrHeaders = {"Bank Master Type Code", "Description"}
            Else
                sqlString = " SELECT customer_bank_master.other_bank_master_code, customer_bank_master.other_bank_master_des  FROM  dbo.customer_bank_master customer_bank_master"
                rptreportname = "Report -Other Bank Master"
                arrHeaders = {"Other Bank Master Code ", "Other Bank Master Name"}
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

            If reportsType = "excel" Then
                ExcelReport(custdetailsdt, rptType, arrHeaders, bytes)
            Else

                Dim documentWidth As Single
                '  Dim document As New Document(PageSize.A4, -10.0F, 10.0F, 30.0F, 35.0F)
                Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 20.0F, 35.0F)
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
                    cell.PaddingTop = 5
                    titletable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(rptreportname, ReportNamefont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.BorderWidthTop = 0
                    cell.PaddingBottom = 18
                    cell.PaddingLeft = 5
                    cell.SetLeading(12, 0)
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
                    tableTitle = New PdfPTable(2)
                    tableTitle.TotalWidth = 400.0F
                    tableTitle.HorizontalAlignment = Element.ALIGN_LEFT

                    tableTitle.LockedWidth = True
                    tableTitle.SplitRows = False
                    'tableTitle.KeepTogether = True
                    tableTitle.SpacingBefore = 10
                    tableTitle.SetWidths(New Single() {0.3F, 0.7F})

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
                        Dim tableData = New PdfPTable(2)
                        tableData.TotalWidth = 400.0F
                        tableData.HorizontalAlignment = Element.ALIGN_LEFT
                        tableData.LockedWidth = True
                        tableData.SplitRows = False
                        tableData.SetWidths(New Single() {0.3F, 0.7F})
                        tableData.SpacingBefore = 0

                        For Each row In custdetailsdt.Rows

                            If rptType = 0 Then
                                arrHeaders = {row("bank_master_type_code"), row("bank_master_type_des")}
                            Else
                                arrHeaders = {row("other_bank_master_code"), row("other_bank_master_des")}
                            End If

                            For i = 0 To arrHeaders.Length - 1
                                phrase = New Phrase()
                                phrase.Add(New Chunk(arrHeaders(i), normalfont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.SetLeading(12, 0)
                                cell.PaddingBottom = 2.0F
                                '  cell.PaddingTop = 1.0F
                                tableData.AddCell(cell)
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
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 710.0F, 10.0F, 0)
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

#Region "ExcelReport"

    Public Sub ExcelReport(ByVal custdetailsdt As DataTable, ByVal rptType As Integer, ByVal arrHeaders() As String, ByRef bytes() As Byte)

        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add(rptreportname)
        Dim rowCount As Integer = 2
        ws.Columns("A:B").Width = 30

        'company name
        ws.Range(rowCount, 1, rowCount + 2, 2).Style.Font.SetBold().Font.FontSize = 15
        ws.Cell(rowCount, 1).Value = rptcompanyname
        ws.Range(rowCount, 1, rowCount, 2).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin)

        rowCount = rowCount + 1
        ws.Cell(rowCount, 1).Value = rptreportname
        ws.Range(rowCount, 1, rowCount, 2).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin)

        rowCount = rowCount + 2
        ws.Range(rowCount, 2, rowCount, 2).Style.Font.SetBold().Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Font.FontSize = 9
        ws.Range(rowCount, 2, rowCount, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

        For i = 0 To arrHeaders.Length - 1
            ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
        Next

        Dim code As Integer
        Dim description, rowCode, rowName As String
        If custdetailsdt.Rows.Count > 0 Then

            For Each row In custdetailsdt.Rows

                rowCount = rowCount + 1
                ws.Range(rowCount, 1, rowCount, 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Font.FontSize = 9
                ws.Range(rowCount, 1, rowCount, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)


                If rptType = 0 Then
                    arrHeaders = {row("bank_master_type_code"), row("bank_master_type_des")}
                Else
                    arrHeaders = {row("other_bank_master_code"), row("other_bank_master_des")}
                End If

                For i = 0 To arrHeaders.Length - 1
                    ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                Next

            Next


        End If

        ws.Cell((rowCount + 2), 1).Value = "Printed Date:" & Now.ToString("dd/MM/yyyy")
        ws.Range((rowCount + 2), 1, (rowCount + 2), 3).Merge()
        Using wStream As New MemoryStream()
            wb.SaveAs(wStream)
            bytes = wStream.ToArray()
        End Using

    End Sub

#End Region

End Class
