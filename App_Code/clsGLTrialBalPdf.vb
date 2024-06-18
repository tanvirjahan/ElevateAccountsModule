Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Globalization
Imports ClosedXML.Excel

Public Class clsGLTrialBalPdf
    Inherits System.Web.UI.Page
    Dim objutils As New clsUtils
#Region "global declaration"
    Dim objclsUtilities As New clsUtils
    Dim normalfont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
    Dim caption As Font = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.BLACK)
    Dim normalfontbold As Font = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLACK)
    Dim normalfontboldlevel2 As Font = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)
    Dim normalfontboldlevel3 As Font = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.BLACK)
    Dim normalfontlevel4 As Font = FontFactory.GetFont("Arial", 6, Font.BOLD, BaseColor.BLACK)
    Dim normalfontlevel5 As Font = FontFactory.GetFont("Arial", 7, Font.NORMAL, BaseColor.BLACK)
    Dim headerfont As Font = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.BLACK)
    Dim Companyname As Font = New Font(FontFactory.GetFont("Times New Roman", 14, Font.BOLD, BaseColor.WHITE))
    Dim ReportNamefont As Font = New Font(FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.WHITE))
    Dim reportfilter As String, rptcompanyname, rptreportname As String, decno As String
    Dim pdebit, pcredit, cdebit, ccredit, tdebit, tcredit, fpdebit, fpcredit, fcdebit, fccredit, ftdebit, ftcredit As Decimal

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
    Public Sub GenerateReport(ByVal fromdate As String, ByVal todate As String, ByVal fromname As String, ByVal divcode As String, ByVal reportname As String, ByVal withmov As String, ByVal closing As String, ByVal level As String, ByVal acccodefrom As String, ByVal acccodeto As String, ByVal rptype As Integer,
                         ByVal rptvalue As Integer, ByVal reportType As String, ByRef bytes() As Byte, ByVal printMode As String, Optional ByVal fileName As String = "")

        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet

            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("sp_gltbtree1", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@movflg", SqlDbType.Int)).Value = IIf(Val(withmov) = 0, 1, 0)
            mySqlCmd.Parameters.Add(New SqlParameter("@frmdiv", SqlDbType.VarChar, 20)).Value = divcode
            mySqlCmd.Parameters.Add(New SqlParameter("@todiv", SqlDbType.VarChar, 20)).Value = divcode
            mySqlCmd.Parameters.Add(New SqlParameter("@currflg", SqlDbType.Int)).Value = 1
            mySqlCmd.Parameters.Add(New SqlParameter("@ptype", SqlDbType.VarChar, 20)).Value = "G"
            mySqlCmd.Parameters.Add(New SqlParameter("@frmac", SqlDbType.VarChar, 20)).Value = Trim(acccodefrom)
            mySqlCmd.Parameters.Add(New SqlParameter("@toac", SqlDbType.VarChar, 20)).Value = Trim(acccodeto)
            mySqlCmd.Parameters.Add(New SqlParameter("@tbord", SqlDbType.Int)).Value = 1
            mySqlCmd.Parameters.Add(New SqlParameter("@aclevel", SqlDbType.Int)).Value = Trim(level)
            mySqlCmd.Parameters.Add(New SqlParameter("@closing", SqlDbType.Int)).Value = Val(closing)
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim custdetailsdt As New DataTable
            custdetailsdt = ds.Tables(0)

            If divcode <> "" Then
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
            Else
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
            End If
            decno = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
            decno = "N" + decno
            If withmov = "0" Then
                reportfilter = "From " + Format(CDate(fromdate), "dd/MM/yyyy") + "  To " + Format(CDate(todate), "dd/MM/yyyy")
                rptreportname = "GL Trial Balance -- Transactions"
            Else
                reportfilter = "As on " + Format(CDate(fromdate), "dd/MM/yyyy")
                rptreportname = "GL Trial Balance -- Balances"
            End If

            If Not String.IsNullOrEmpty(fromname) Then
                reportfilter = reportfilter & Space(2) & "Control Account From" & Space(2) & fromname & Space(2) & "To" & Space(2) & fromname
            End If

            If String.Equals(reportType, "excel") Then
                ExcelReport(custdetailsdt, withmov, bytes)

            Else

                Dim documentWidth As Single
                '    Dim document As New Document(PageSize.A4, -10.0F, 10.0F, 30.0F, 38.0F)
                Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 30.0F, 35.0F)
                If withmov = "0" Then
                    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)
                    documentWidth = 770.0F
                Else
                    documentWidth = 550.0F
                End If
                Using memoryStream As New System.IO.MemoryStream()
                    Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)

                    Dim phrase As Phrase = Nothing
                    Dim cell As PdfPCell = Nothing
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
                    phrase.Add(New Chunk(reportfilter, headerfont))
                    cell = New PdfPCell(phrase)
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 5
                    cell.PaddingTop = 10
                    Reporttitle.AddCell(cell)

                    Dim tableTitle As PdfPTable = Nothing
                    Dim arrHeaders() As String
                    If withmov = "0" Then
                        tableTitle = New PdfPTable(8)
                        arrHeaders = {"A/C Code", "Account Name", "Opening", "For The Period", "Closing"}
                        tableTitle.SetWidths(New Single() {0.1F, 0.24F, 0.11F, 0.11F, 0.11F, 0.11F, 0.11F, 0.11F})
                    ElseIf withmov = "1" Then
                        tableTitle = New PdfPTable(4)
                        arrHeaders = {"Account Code", "Account Name", "Debit", "Credit"}
                        tableTitle.SetWidths(New Single() {0.15F, 0.55F, 0.15F, 0.15F})
                    End If
                    tableTitle.TotalWidth = documentWidth
                    tableTitle.LockedWidth = True
                    tableTitle.SplitRows = False
                    tableTitle.KeepTogether = True
                    tableTitle.SpacingBefore = 10
                    tableTitle.SpacingAfter = 0

                    For i = 0 To arrHeaders.Length - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.SetLeading(12, 0)
                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 1.0F

                        If withmov = "0" Then
                            If i <= 1 Then
                                cell.BorderWidthBottom = 0
                            Else
                                cell.Colspan = 2
                            End If
                        End If
                        tableTitle.AddCell(cell)
                    Next

                    If withmov = "0" Then
                        Dim arrHeader() As String = {"", "", "Debit", "Credit", "Debit", "Credit", "Debit", "Credit"}
                        For i = 0 To arrHeader.Length - 1

                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrHeader(i), normalfontbold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            If i <= 1 Then
                                cell.BorderWidthTop = 0
                            End If
                            cell.SetLeading(12, 0)
                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F

                            tableTitle.AddCell(cell)
                        Next
                    End If
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
                    cell.SetLeading(2, 0)
                    cell.PaddingBottom = 20.0F
                    FooterTable.SpacingBefore = 2.0F
                    FooterTable.SpacingAfter = 2.0F
                    FooterTable.AddCell(cell)
                    FooterTable.Complete = True

                    'writer.PageEvent = New ClsHeaderFooter(titletable, Reporttitle, FooterTable, tableTitle)

                    writer.PageEvent = New ClsHeaderFooter(titletable, Reporttitle, FooterTable, tableTitle, "printDate")
                    document.Open()
                    fpdebit = 0
                    fpcredit = 0
                    fcdebit = 0
                    fccredit = 0
                    ftdebit = 0
                    ftcredit = 0
                    pdebit = 0
                    pcredit = 0
                    cdebit = 0
                    ccredit = 0
                    tdebit = 0
                    tcredit = 0
                    custdetailsdt.DefaultView.Sort = "acctcode ASC"
                    custdetailsdt = custdetailsdt.DefaultView.ToTable


                    If custdetailsdt.Rows.Count > 0 Then

                        For Each row In custdetailsdt.Rows
                            Dim tableData As PdfPTable
                            tdebit = IIf(Decimal.Parse(row("tdebit")) >= Decimal.Parse(row("tcredit")), Decimal.Subtract(Decimal.Parse(row("tdebit")), Decimal.Parse(row("tcredit"))), 0)
                            tcredit = IIf(Decimal.Parse(row("tcredit")) > Decimal.Parse(row("tdebit")), Decimal.Subtract(Decimal.Parse(row("tcredit")), Decimal.Parse(row("tdebit"))), 0)

                            If withmov = "0" Then
                                tableData = New PdfPTable(8)
                                pdebit = IIf(Decimal.Parse(row("pdebit")) >= Decimal.Parse(row("pcredit")), Decimal.Subtract(Decimal.Parse(row("pdebit")), Decimal.Parse(row("pcredit"))), 0)
                                pcredit = IIf(Decimal.Parse(row("pcredit")) > Decimal.Parse(row("pdebit")), Decimal.Subtract(Decimal.Parse(row("pcredit")), Decimal.Parse(row("pdebit"))), 0)
                                cdebit = IIf(Decimal.Parse(row("cdebit")) >= Decimal.Parse(row("ccredit")), Decimal.Subtract(Decimal.Parse(row("cdebit")), Decimal.Parse(row("ccredit"))), 0)
                                ccredit = IIf(Decimal.Parse(row("ccredit")) > Decimal.Parse(row("cdebit")), Decimal.Subtract(Decimal.Parse(row("ccredit")), Decimal.Parse(row("cdebit"))), 0)
                                arrHeaders = {row("acctcode").ToString(), row("acctname").ToString(), pdebit.ToString(decno), pcredit.ToString(decno), cdebit.ToString(decno), ccredit.ToString(decno), tdebit.ToString(decno), tcredit.ToString(decno)}
                                tableData.SetWidths(New Single() {0.1F, 0.24F, 0.11F, 0.11F, 0.11F, 0.11F, 0.11F, 0.11F})
                                If row("state") = "1" Then
                                    fpdebit = fpdebit + pcredit
                                    fpcredit = fpcredit + pcredit
                                    fcdebit = fcdebit + cdebit
                                    fccredit = fccredit + ccredit
                                    ftdebit = ftdebit + tdebit
                                    ftcredit = ftcredit + tcredit
                                End If

                            ElseIf withmov = "1" Then
                                tableData = New PdfPTable(4)
                                arrHeaders = {row("acctcode").ToString(), row("acctname").ToString(), tdebit.ToString(decno), tcredit.ToString(decno)}
                                tableData.SetWidths(New Single() {0.15F, 0.55F, 0.15F, 0.15F})
                                If row("state") = "1" Then
                                    ftdebit = ftdebit + tdebit
                                    ftcredit = ftcredit + tcredit
                                End If
                            End If
                            tableData.TotalWidth = documentWidth
                            tableData.LockedWidth = True
                            tableData.Complete = False
                            tableData.SplitRows = False
                            tableData.SpacingBefore = 0.0F
                            For i = 0 To arrHeaders.Length - 1
                                phrase = New Phrase()
                                Dim fontselect As Font
                                If row("state") = "1" Then
                                    fontselect = normalfontbold
                                ElseIf row("state") = "2" Then
                                    fontselect = normalfontboldlevel2
                                ElseIf row("state") = "3" Then
                                    fontselect = normalfontboldlevel3

                                ElseIf row("state") = "4" Then
                                    fontselect = normalfontlevel4
                                Else
                                    fontselect = normalfontlevel5
                                End If
                                phrase.Add(New Chunk(arrHeaders(i), fontselect))
                                If i > 1 Then
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                Else
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                End If
                                cell.SetLeading(12, 0)
                                cell.PaddingBottom = 4.0F
                                cell.PaddingTop = 1.0F
                                tableData.AddCell(cell)
                            Next
                            tableData.Complete = True
                            document.Add(tableData)
                        Next

                        Dim sumTotal As PdfPTable
                        If withmov = "0" Then
                            sumTotal = New PdfPTable(8)
                            arrHeaders = {"Final Total", fpdebit.ToString(decno), fpcredit.ToString(decno), fcdebit.ToString(decno), fccredit.ToString(decno), ftdebit.ToString(decno), ftcredit.ToString(decno)}
                            sumTotal.SetWidths(New Single() {0.14F, 0.2F, 0.11F, 0.11F, 0.11F, 0.11F, 0.11F, 0.11F})
                        Else
                            sumTotal = New PdfPTable(4)
                            arrHeaders = {"Final Total", ftdebit.ToString(decno), ftcredit.ToString(decno)}
                            sumTotal.SetWidths(New Single() {0.15F, 0.55F, 0.15F, 0.15F})
                        End If
                        sumTotal.TotalWidth = documentWidth
                        sumTotal.LockedWidth = True
                        sumTotal.Complete = False
                        sumTotal.SplitRows = False
                        sumTotal.SpacingBefore = 0.0F
                        For i = 0 To arrHeaders.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
                            If i > 0 Then
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                            Else
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                            End If
                            If i = 0 Then
                                cell.Colspan = 2
                            End If
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            sumTotal.AddCell(cell)
                        Next
                        sumTotal.Complete = True
                        document.Add(sumTotal)
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
                                    'ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), documentWidth, 10.0F, 0)
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), IIf(documentWidth = 550.0F, documentWidth + 25, documentWidth + 35), 20.0F, 0)
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
    'Tanvir 20062022
    Public Sub GenerateVatAccReport(ByVal fromdate As String, ByVal reportname As String, ByVal divcode As String, ByVal reportType As String,
                        ByRef bytes() As Byte, ByVal printMode As String, Optional ByVal fileName As String = "")

        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet

            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("sp_get_vataccrued_breakup", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@asondate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")

            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim custdetailsdt As New DataTable
            custdetailsdt = ds.Tables(0)

            If divcode <> "" Then
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
            Else
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
            End If
            decno = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
            decno = "N" + decno

            If String.Equals(reportType, "excel") Then
                ExcelVatAccrReport(custdetailsdt, Format(Convert.ToDateTime(fromdate), "dd/MM/yyyy"), reportname, bytes)

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#Region "ExcelReport"
    Public Sub ExcelVatAccrReport(ByVal custdetailsdt As DataTable, ByVal fromdate As String, ByVal reportname As String, ByRef bytes() As Byte)
        fpdebit = 0
        fpcredit = 0
        fcdebit = 0
        fccredit = 0
        ftdebit = 0
        ftcredit = 0
        pdebit = 0
        pcredit = 0
        cdebit = 0
        ccredit = 0
        tdebit = 0
        tcredit = 0
        ' custdetailsdt.DefaultView.Sort = "Bookingcode ASC"
        custdetailsdt = custdetailsdt.DefaultView.ToTable

        Dim deciamlPoint As String

        If decno = "N1" Then
            deciamlPoint = "###,##,##,##0.0"

        ElseIf decno = "N2" Then
            deciamlPoint = "###,##,##,##0.00"
        ElseIf decno = "N3" Then
            deciamlPoint = "###,##,##,##0.000"
        ElseIf decno = "N4" Then
            deciamlPoint = "###,##,##,##0.0000"
        End If

        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add("VatAccruedBreakup")
        Dim rowcount As Integer = 2
        ws.Columns.AdjustToContents()

        Dim colCount As Integer
        ws.Column("B").Width = 8
        colCount = 8
        'Comapny Name Heading
        ws.Cell(rowcount, 1).Value = rptcompanyname
        Dim company = ws.Range(rowcount, 1, rowcount, colCount).Merge()
        company.Style.Font.SetBold().Border.SetOutsideBorder(XLBorderStyleValues.Thin).Fill.SetBackgroundColor(XLColor.FromHtml("#0048C0"))
        company.Style.Font.FontSize = 15
        company.Style.Font.FontColor = XLColor.White
        company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

        rowcount = rowcount + 1
        'Report Name Heading
        ws.Cell("A3").Value = reportname
        Dim report = ws.Range(rowcount, 1, rowcount, colCount).Merge()
        report.Style.Font.SetBold().Border.SetOutsideBorder(XLBorderStyleValues.Thin).Fill.SetBackgroundColor(XLColor.FromHtml("#0080C0"))
        report.Style.Font.FontSize = 14
        report.Style.Font.FontColor = XLColor.White
        report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
        'report.Style.Alignment.Vertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)

        Dim arrHeaders() As String

        'Report Filter
        rowcount = rowcount + 1
        ws.Cell("A4").Value = "As On Date:" + fromdate
        Dim filter = ws.Range(rowcount, 1, rowcount, colCount).Merge()
        filter.Style.Font.SetBold().Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 12
        filter.Style.Font.FontColor = XLColor.Black
        filter.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
        filter.Style.Alignment.WrapText = True
        Dim rowheight As Integer
      
        rowcount = 7
        Dim tabletitle = ws.Range(6, 1, 7, 8)
        tabletitle.Style.Font.SetBold().Alignment.Vertical = XLAlignmentVerticalValues.Center
        tabletitle.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        tabletitle.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
        ws.Cell("A6").Value = "Booking Code"

        ws.Cell("B6").Value = "Service Type"
        ws.Columns("A:B").Width = 15
        ws.Cell("C6").Value = "Supplier Name"
        ws.Columns("c").Width = 40
        ws.Cell("D6").Value = "Arrival Date"

        ws.Cell("E6").Value = "Departure Date"


        ws.Cell("F6").Value = "Debit"


        ws.Cell("G6").Value = "Credit"

        ws.Cell("H6").Value = "Balance"

        ws.Columns("D:H").Width = 15
        

        ws.Column(1).CellsUsed().SetDataType(XLCellValues.Text)


        If custdetailsdt.Rows.Count > 0 Then
            For Each row In custdetailsdt.Rows
               

                arrHeaders = {(row("Booking Code") & Space(1) & " ").ToString(), row("Service Type").ToString(), row("Supplier Name").ToString(), row("Arrival Date").ToString(), row("Departure Date").ToString(), row("Debit").ToString(), row("Credit").ToString(), row("Balance").ToString()}
            
                rowcount = rowcount + 1
               
                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.WrapText = True
                  For i = 0 To arrHeaders.Length - 1

                    ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
                Next


                ws.Range(rowcount, 6, rowcount, arrHeaders.Length).Style.NumberFormat.Format = deciamlPoint


            Next
            arrHeaders = {"Final Total", "", "", "", "", custdetailsdt.Compute("Sum(Debit)", "").ToString(), custdetailsdt.Compute("Sum(Credit)", "").ToString(), custdetailsdt.Compute("Sum(Balance)", "").ToString()}
            rowcount = rowcount + 1
            ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.SetBold().Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)

            For i = 0 To arrHeaders.Length - 1
                ws.Cell(rowcount, i + 1).Value = arrHeaders(i)

            Next




            ws.Cell((rowcount + 2), 1).Value = "Printed Date:" & Now.ToString("dd/MM/yyyy")
            ws.Range((rowcount + 2), 1, (rowcount + 2), 3).Merge()
            Using wStream As New MemoryStream()
                wb.SaveAs(wStream)
                bytes = wStream.ToArray()
            End Using
        End If
    End Sub
#End Region
    'Tanvir 20062022
#Region "ExcelReport"
    Public Sub ExcelReport(ByVal custdetailsdt As DataTable, ByVal withmov As String, ByRef bytes() As Byte)
        fpdebit = 0
        fpcredit = 0
        fcdebit = 0
        fccredit = 0
        ftdebit = 0
        ftcredit = 0
        pdebit = 0
        pcredit = 0
        cdebit = 0
        ccredit = 0
        tdebit = 0
        tcredit = 0
        custdetailsdt.DefaultView.Sort = "acctcode ASC"
        custdetailsdt = custdetailsdt.DefaultView.ToTable

        Dim deciamlPoint As String

        If decno = "N1" Then
            deciamlPoint = "###,##,##,##0.0"

        ElseIf decno = "N2" Then
            deciamlPoint = "###,##,##,##0.00"
        ElseIf decno = "N3" Then
            deciamlPoint = "###,##,##,##0.000"
        ElseIf decno = "N4" Then
            deciamlPoint = "###,##,##,##0.0000"
        End If

        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add("GlTrailBal")
        Dim rowcount As Integer = 2
        ws.Columns.AdjustToContents()

        Dim colCount As Integer
        ws.Column("B").Width = 15
        If withmov = "0" Then
            colCount = 8
            ws.Column("B").Width = 25
            ws.Columns("C:H").Width = 13
        Else
            colCount = 4
            ws.Column("B").Width = 33
            ws.Columns("C:D").Width = 20

        End If
        'Comapny Name Heading
        ws.Cell(rowcount, 1).Value = rptcompanyname
        Dim company = ws.Range(rowcount, 1, rowcount, colCount).Merge()
        company.Style.Font.SetBold().Border.SetOutsideBorder(XLBorderStyleValues.Thin).Fill.SetBackgroundColor(XLColor.FromHtml("#0048C0"))
        company.Style.Font.FontSize = 15
        company.Style.Font.FontColor = XLColor.White
        company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

        rowcount = rowcount + 1
        'Report Name Heading
        ws.Cell("A3").Value = rptreportname
        Dim report = ws.Range(rowcount, 1, rowcount, colCount).Merge()
        report.Style.Font.SetBold().Border.SetOutsideBorder(XLBorderStyleValues.Thin).Fill.SetBackgroundColor(XLColor.FromHtml("#0080C0"))
        report.Style.Font.FontSize = 14
        report.Style.Font.FontColor = XLColor.White
        report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
        'report.Style.Alignment.Vertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)

        Dim arrHeaders() As String

        'Report Filter
        rowcount = rowcount + 1
        ws.Cell("A4").Value = reportfilter
        Dim filter = ws.Range(rowcount, 1, rowcount, colCount).Merge()
        filter.Style.Font.SetBold().Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 12
        filter.Style.Font.FontColor = XLColor.Black
        filter.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
        filter.Style.Alignment.WrapText = True
        Dim rowheight As Integer
        If withmov = "0" Then
            If reportfilter.Length > 80 Then
                rowheight = IIf(reportfilter.Length > 80 And reportfilter.Length < 160, 32, IIf(reportfilter.Length > 160 And reportfilter.Length < 300, 48, 64))
                ws.Row(rowcount).Height = rowheight
            End If
        Else
            If reportfilter.Length > 70 Then
                rowheight = IIf(reportfilter.Length > 70 And reportfilter.Length < 140, 32, IIf(reportfilter.Length > 140 And reportfilter.Length < 300, 48, 64))
                ws.Row(rowcount).Height = rowheight
            End If
        End If


        'Table Heading
        If withmov = "0" Then
            rowcount = 7
            Dim tabletitle = ws.Range(6, 1, 7, 8)
            tabletitle.Style.Font.SetBold().Alignment.Vertical = XLAlignmentVerticalValues.Center
            tabletitle.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            tabletitle.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
            ws.Cell("A6").Value = "A/C Code"
            ws.Range("A6:A7").Merge()
            ws.Cell("B6").Value = "Account Name"
            ws.Range("B6:B7").Merge()
            ws.Cell("C6").Value = "Opening"
            ws.Range("C6:D6").Merge()
            ws.Cell("E6").Value = "For the Period"
            ws.Range("E6:F6").Merge()
            ws.Cell("G6").Value = "Closing"
            ws.Range("G6:H6").Merge()
            ws.Cell("C7").Value = "Debit"
            ws.Cell("D7").Value = "Credit"
            ws.Cell("E7").Value = "Debit"
            ws.Cell("F7").Value = "Credit"
            ws.Cell("G7").Value = "Debit"
            ws.Cell("H7").Value = "Credit"
        Else
            rowcount = 6
            Dim tabletitle = ws.Range(6, 1, 6, 4)
            tabletitle.Style.Font.SetBold().Alignment.Vertical = XLAlignmentVerticalValues.Center
            tabletitle.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            tabletitle.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
            ws.Cell("A6").Value = "Account Code"
            ws.Cell("B6").Value = "Account Name"
            ws.Cell("C6").Value = "Debit"
            ws.Cell("D6").Value = "Credit"
        End If

        '   ws.Column(1).CellsUsed().SetDataType(XLCellValues.Text)


        If custdetailsdt.Rows.Count > 0 Then
            For Each row In custdetailsdt.Rows
                tdebit = IIf(Decimal.Parse(row("tdebit")) >= Decimal.Parse(row("tcredit")), Decimal.Subtract(Decimal.Parse(row("tdebit")), Decimal.Parse(row("tcredit"))), 0)
                tcredit = IIf(Decimal.Parse(row("tcredit")) > Decimal.Parse(row("tdebit")), Decimal.Subtract(Decimal.Parse(row("tcredit")), Decimal.Parse(row("tdebit"))), 0)

                If withmov = "0" Then

                    pdebit = IIf(Decimal.Parse(row("pdebit")) >= Decimal.Parse(row("pcredit")), Decimal.Subtract(Decimal.Parse(row("pdebit")), Decimal.Parse(row("pcredit"))), 0)
                    pcredit = IIf(Decimal.Parse(row("pcredit")) > Decimal.Parse(row("pdebit")), Decimal.Subtract(Decimal.Parse(row("pcredit")), Decimal.Parse(row("pdebit"))), 0)
                    cdebit = IIf(Decimal.Parse(row("cdebit")) >= Decimal.Parse(row("ccredit")), Decimal.Subtract(Decimal.Parse(row("cdebit")), Decimal.Parse(row("ccredit"))), 0)
                    ccredit = IIf(Decimal.Parse(row("ccredit")) > Decimal.Parse(row("cdebit")), Decimal.Subtract(Decimal.Parse(row("ccredit")), Decimal.Parse(row("cdebit"))), 0)
                    arrHeaders = {(row("acctcode") & Space(1) & " ").ToString(), row("acctname").ToString(), pdebit.ToString(decno), pcredit.ToString(decno), cdebit.ToString(decno), ccredit.ToString(decno), tdebit.ToString(decno), tcredit.ToString(decno)}
                    'arrHeaders = {(row("acctcode")).ToString(), row("acctname").ToString(), pdebit.ToString(decno), pcredit.ToString(decno), cdebit.ToString(decno), ccredit.ToString(decno), tdebit.ToString(decno), tcredit.ToString(decno)}
                    If row("state") = "1" Then
                        fpdebit = fpdebit + pcredit
                        fpcredit = fpcredit + pcredit
                        fcdebit = fcdebit + cdebit
                        fccredit = fccredit + ccredit
                        ftdebit = ftdebit + tdebit
                        ftcredit = ftcredit + tcredit
                    End If

                ElseIf withmov = "1" Then

                    arrHeaders = {(row("acctcode") & Space(1) & " ").ToString(), row("acctname").ToString(), tdebit.ToString(decno), tcredit.ToString(decno)}
                    If row("state") = "1" Then
                        ftdebit = ftdebit + tdebit
                        ftcredit = ftcredit + tcredit
                    End If
                End If
                ' Dim dataStyle
                rowcount = rowcount + 1
                If row("state") = "1" Then
                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 12
                    'fontselect = normalfontbold
                ElseIf row("state") = "2" Then
                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 11
                ElseIf row("state") = "3" Then
                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 10

                ElseIf row("state") = "4" Then
                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 9
                Else
                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.FontSize = 10
                End If
                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.WrapText = True
                ' dataStyle.Style
                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Alignment.WrapText = True

                For i = 0 To arrHeaders.Length - 1

                    If i > 1 Then
                        ws.Cell(rowcount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                        ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    Else

                        ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        ws.Cell(rowcount, i + 1).DataType = XLCellValues.Text
                        If i = 0 Then
                            ws.Cell(rowcount, i + 1).Value = "'" + arrHeaders(i)
                        Else
                            ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
                        End If

                    End If

                Next


                ws.Range(rowcount, 3, rowcount, arrHeaders.Length).Style.NumberFormat.Format = deciamlPoint


            Next

            If withmov = "0" Then
                arrHeaders = {"Final Total", "", fpdebit.ToString(decno), fpcredit.ToString(decno), fcdebit.ToString(decno), fccredit.ToString(decno), ftdebit.ToString(decno), ftcredit.ToString(decno)}

            Else

                arrHeaders = {"Final Total", "", ftdebit.ToString(decno), ftcredit.ToString(decno)}

            End If

            rowcount = rowcount + 1
            ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.SetBold().Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            For i = 0 To arrHeaders.Length - 1
                If i > 1 Then
                    ws.Cell(rowcount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                    ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).NumberFormat.Format = deciamlPoint
                Else
                    ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
                    ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                End If


            Next

            ws.Cell((rowcount + 2), 1).Value = "Printed Date:" & Now.ToString("dd/MM/yyyy")
            ws.Range((rowcount + 2), 1, (rowcount + 2), 3).Merge()
            Using wStream As New MemoryStream()
                wb.SaveAs(wStream)
                bytes = wStream.ToArray()
            End Using
        End If
    End Sub

#End Region
End Class
