Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Globalization
Imports System.Linq
Imports ClosedXML.Excel


Public Class clsOpeningTrialBalPdf
    Inherits System.Web.UI.Page
    Dim objutils As New clsUtils
#Region "global declaration"
    Dim objclsUtilities As New clsUtils
    Dim normalfont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
    Dim caption As Font = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.BLACK)
    Dim normalfontbold As Font = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLACK)
    Dim headerfont As Font = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.BLACK)
    Dim Companyname As Font = New Font(FontFactory.GetFont("Times New Roman", 14, Font.BOLD, BaseColor.BLACK))
    Dim ReportNamefont As Font = New Font(FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK))
    Dim rptcompanyname, decno, currency As String
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
    Public Sub GenerateReport(ByVal reportsType As String, ByVal trantype As String, ByVal divcode As String, ByVal tranid As String, ByVal type As String, ByVal rptreportname As String, ByRef bytes() As Byte, ByVal printMode As String, Optional ByVal fileName As String = "")

        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet

            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")

            mySqlCmd = New SqlCommand("sp_rep_TrialBal", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = tranid
            mySqlCmd.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 20)).Value = trantype
            mySqlCmd.Parameters.Add(New SqlParameter("@type", SqlDbType.VarChar, 20)).Value = type
            mySqlCmd.Parameters.Add(New SqlParameter("@orderby", SqlDbType.Int)).Value = 0
            mySqlCmd.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 20)).Value = divcode
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim custdetailsdt As New DataTable
            custdetailsdt = ds.Tables(0)


            mySqlCmd = New SqlCommand("sp_rep_TrialBalPartyBalceAmt", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = tranid
            mySqlCmd.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 20)).Value = trantype
            mySqlCmd.Parameters.Add(New SqlParameter("@type", SqlDbType.VarChar, 20)).Value = type
            mySqlCmd.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 20)).Value = divcode
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim partyBalanceAmt As New DataTable
            partyBalanceAmt = ds.Tables(0)


            If divcode <> "" Then
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
            Else
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
            End If
            decno = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
            decno = "N" + decno

            currency = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)

            If reportsType = "excel" Then
                ExcelReport(custdetailsdt, partyBalanceAmt, rptreportname, bytes)
            Else

                Dim documentWidth As Single
                Dim document As New Document(PageSize.A4, -10.0F, 10.0F, 30.0F, 35.0F)

                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)
                documentWidth = 770.0F

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
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.SetLeading(12, 0)
                    cell.BorderWidthBottom = 0
                    cell.PaddingBottom = 10
                    cell.PaddingLeft = 5
                    cell.PaddingTop = 5
                    titletable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(rptreportname, ReportNamefont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.BorderWidthTop = 0
                    cell.PaddingBottom = 18
                    cell.PaddingLeft = 5
                    cell.SetLeading(12, 0)
                    titletable.AddCell(cell)
                    titletable.Complete = True


                    Dim tableTitle As PdfPTable = Nothing
                    Dim arrHeaders() As String

                    tableTitle = New PdfPTable(11)
                    tableTitle.TotalWidth = documentWidth
                    tableTitle.LockedWidth = True
                    tableTitle.SplitRows = False
                    'tableTitle.KeepTogether = True
                    '  tableTitle.SpacingBefore = 0
                    tableTitle.SetWidths(New Single() {0.07F, 0.15F, 0.1F, 0.14F, 0.12F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F})
                    arrHeaders = {"A/C Code", "A/C Name", "Cost Center Code", "Cost Center Name", " Narration", "Debit", "Credit", "Currency", "Conv.Rate", "Base Debit", "Base Credit"}

                    For i = 0 To arrHeaders.Length - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrHeaders(i), normalfontbold))

                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER

                        If i = 0 Then
                            cell.BorderWidthRight = 0
                        ElseIf i = arrHeaders.Length - 1 Then
                            cell.BorderWidthLeft = 0
                        Else
                            cell.BorderWidthRight = 0
                            cell.BorderWidthLeft = 0
                        End If
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

                    Dim debit, credit, basedebit, basecredit, totalbasedebit, totalbasecreedit, currrate As Decimal
                    Dim code, acctname, costcode, costname, narration, curr As String
                    If custdetailsdt.Rows.Count > 0 Then


                        Dim tableData = New PdfPTable(11)
                        tableData.TotalWidth = documentWidth
                        tableData.LockedWidth = True
                        tableData.SplitRows = False
                        tableData.SetWidths(New Single() {0.07F, 0.15F, 0.1F, 0.14F, 0.12F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F})
                        tableData.SpacingBefore = 0
                        For Each row In custdetailsdt.Rows

                            If Not (TypeOf row("debit") Is DBNull) Then
                                debit = Decimal.Parse(row("debit"))
                            Else
                                debit = 0.0
                            End If


                            If Not (TypeOf row("credit") Is DBNull) Then
                                credit = Decimal.Parse(row("credit"))
                            Else
                                credit = 0.0
                            End If


                            If Not (TypeOf row("basedebit") Is DBNull) Then
                                basedebit = Decimal.Parse(row("basedebit"))
                            Else
                                basedebit = 0.0
                            End If


                            If Not (TypeOf row("basecredit") Is DBNull) Then
                                basecredit = Decimal.Parse(row("basecredit"))
                            Else
                                basecredit = 0.0
                            End If

                            If Not (TypeOf row("code") Is DBNull) Then
                                code = (row("code"))
                            Else
                                code = ""
                            End If

                            If Not (TypeOf row("acctname") Is DBNull) Then
                                acctname = (row("acctname"))
                            Else
                                acctname = ""
                            End If
                            If Not (TypeOf row("costcenter_code") Is DBNull) Then
                                costcode = (row("costcenter_code"))
                            Else
                                costcode = ""
                            End If

                            If Not (TypeOf row("costcenter_name") Is DBNull) Then
                                costname = (row("costcenter_name"))
                            Else
                                costname = ""
                            End If
                            If Not (TypeOf row("narration") Is DBNull) Then
                                narration = (row("narration"))
                            Else
                                narration = ""
                            End If
                            If Not (TypeOf row("currency") Is DBNull) Then
                                curr = (row("currency"))
                            Else
                                curr = ""
                            End If

                            If Not (TypeOf row("Currency_rate") Is DBNull) Then
                                currrate = Decimal.Parse(row("Currency_rate"))
                            Else
                                currrate = 0.0
                            End If



                            totalbasecreedit = totalbasecreedit + basecredit
                            totalbasedebit = totalbasedebit + debit

                            arrHeaders = {code, acctname, costcode, costname, narration, debit.ToString(decno), credit.ToString(decno), curr, currrate.ToString(decno), basedebit.ToString(decno), basecredit.ToString(decno)}
                            For i = 0 To arrHeaders.Length - 1
                                phrase = New Phrase()
                                phrase.Add(New Chunk(arrHeaders(i), normalfont))

                                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER

                                If i = 0 Then
                                    cell.BorderWidthRight = 0
                                ElseIf i = arrHeaders.Length - 1 Then
                                    cell.BorderWidthLeft = 0
                                Else
                                    cell.BorderWidthRight = 0
                                    cell.BorderWidthLeft = 0
                                End If
                                cell.SetLeading(12, 0)
                                cell.PaddingBottom = 8.0F
                                cell.PaddingTop = 1.0F
                                tableData.AddCell(cell)
                            Next

                        Next
                        Dim partyBalance As Decimal
                        If partyBalanceAmt.Rows.Count > 0 Then
                            For Each row In partyBalanceAmt.Rows

                                If Not (TypeOf row("partybalance") Is DBNull) Then
                                    partyBalance = Decimal.Parse(row("partybalance"))
                                Else
                                    partyBalance = 0.0
                                End If

                            Next


                        End If
                        Dim tableTotal As PdfPTable = Nothing
                        tableTotal = New PdfPTable(8)
                        ' tableTotal.SetWidths(New Single() {0.07F, 0.15F, 0.1F, 0.14F, 0.12F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F, 0.07F})

                        tableTotal.TotalWidth = documentWidth
                        tableTotal.LockedWidth = True
                        tableTotal.SplitRows = False
                        '  tableTotal.KeepTogether = True
                        tableTotal.SpacingBefore = 0

                        tableTotal.SetWidths(New Single() {0.18F, 0.14F, 0.2F, 0.13F, 0.05F, 0.14F, 0.08F, 0.08F})
                        arrHeaders = {"Total Net Amount", (totalbasecreedit - totalbasedebit).ToString(decno), "Total Party Balance", partyBalance.ToString("N2"), IIf((totalbasecreedit - totalbasedebit) > 0, "Cr", "Dr"), "Total Base Amount", totalbasedebit.ToString(decno), totalbasecreedit.ToString(decno)}
                        For i = 0 To arrHeaders.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrHeaders(i), normalfontbold))

                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER

                            If i = 0 Then
                                cell.BorderWidthRight = 0
                            ElseIf i = arrHeaders.Length - 1 Then
                                cell.BorderWidthLeft = 0
                            Else
                                cell.BorderWidthRight = 0
                                cell.BorderWidthLeft = 0
                            End If
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            tableTotal.AddCell(cell)
                        Next

                        document.Add(tableData)
                        document.Add(tableTotal)
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

#Region "Excel Report"
    Public Sub ExcelReport(ByVal custdetailsdt As DataTable, ByVal partyBalanceAmt As DataTable, ByVal rptreportname As String, ByRef bytes() As Byte)
        Dim wb As New XLWorkbook

        Dim arrHeaders() As String
        Dim DecimalPoints As String

        Dim ws = wb.Worksheets.Add(rptreportname)
        Dim rowCount As Integer = 2
        ws.Column("B").Width = 30
        ws.Columns("E:I").Width = 13
        ws.Columns("I:K").Width = 13
        ws.Column("H").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
        ws.Columns("C:D").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
        ws.Column("A").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
        'company name
        ws.Range(rowCount, 1, rowCount + 2, 11).Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin)

        ws.Cell(rowCount, 1).Value = rptcompanyname
        ws.Range(rowCount, 1, rowCount, 11).Merge().Style.Font.FontSize = 15
        ' ws.Range(rowCount + 1, 1, rowCount + 1, 11).Merge()

        rowCount = rowCount + 1
        ws.Cell(rowCount, 1).Value = rptreportname
        ws.Range(rowCount, 1, rowCount, 11).Merge().Style.Font.FontSize = 11

        ws.Range(rowCount + 1, 1, rowCount + 1, 11).Merge()

        arrHeaders = {"A/C Code", "A/C Name", "Cost Center Code", "Cost Center Name", " Narration", "Debit", "Credit", "Currency", "Conv.Rate", "Base Debit", "Base Credit"}
        rowCount = rowCount + 2
        ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Font.SetBold().Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 9
        ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.WrapText = True

        For i = 0 To arrHeaders.Length - 1
            ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
        Next

        Dim debit, credit, basedebit, basecredit, totalbasedebit, totalbasecreedit, currrate As Decimal
        Dim code, acctname, costcode, costname, narration, curr As String
        If custdetailsdt.Rows.Count > 0 Then

            For Each row In custdetailsdt.Rows

                If Not (TypeOf row("debit") Is DBNull) Then
                    debit = Decimal.Parse(row("debit"))
                Else
                    debit = 0.0
                End If


                If Not (TypeOf row("credit") Is DBNull) Then
                    credit = Decimal.Parse(row("credit"))
                Else
                    credit = 0.0
                End If


                If Not (TypeOf row("basedebit") Is DBNull) Then
                    basedebit = Decimal.Parse(row("basedebit"))
                Else
                    basedebit = 0.0
                End If


                If Not (TypeOf row("basecredit") Is DBNull) Then
                    basecredit = Decimal.Parse(row("basecredit"))
                Else
                    basecredit = 0.0
                End If

                If Not (TypeOf row("code") Is DBNull) Then
                    code = (row("code"))
                Else
                    code = ""
                End If

                If Not (TypeOf row("acctname") Is DBNull) Then
                    acctname = (row("acctname"))
                Else
                    acctname = ""
                End If
                If Not (TypeOf row("costcenter_code") Is DBNull) Then
                    costcode = (row("costcenter_code"))
                Else
                    costcode = ""
                End If

                If Not (TypeOf row("costcenter_name") Is DBNull) Then
                    costname = (row("costcenter_name"))
                Else
                    costname = ""
                End If
                If Not (TypeOf row("narration") Is DBNull) Then
                    narration = (row("narration"))
                Else
                    narration = ""
                End If
                If Not (TypeOf row("currency") Is DBNull) Then
                    curr = (row("currency"))
                Else
                    curr = ""
                End If

                If Not (TypeOf row("Currency_rate") Is DBNull) Then
                    currrate = Decimal.Parse(row("Currency_rate"))
                Else
                    currrate = 0.0
                End If

                If decno = "N1" Then
                    DecimalPoints = "##,##,##,##0.0"
                ElseIf decno = "N2" Then
                    DecimalPoints = "##,##,##,##0.00"
                ElseIf decno = "N3" Then
                    DecimalPoints = "##,##,##,##0.000"
                ElseIf decno = "N4" Then
                    DecimalPoints = "##,##,##,##0.0000"
                Else
                    DecimalPoints = "##,##,##,##0.00"

                End If

                totalbasecreedit = totalbasecreedit + basecredit
                totalbasedebit = totalbasedebit + debit

                arrHeaders = {code, acctname, costcode, costname, narration, debit.ToString(decno), credit.ToString(decno), curr, currrate.ToString(decno), basedebit.ToString(decno), basecredit.ToString(decno)}
                rowCount = rowCount + 1
                ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 8
                ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.WrapText = True

                For i = 0 To arrHeaders.Length - 1

                    If i = 5 Or i = 6 Or (i > 7 And i <= arrHeaders.Length - 1) Then

                        ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                        ws.Cell(rowCount, i + 1).Style.NumberFormat.Format = DecimalPoints
                    Else
                        ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                    End If

                Next

            Next
            Dim partyBalance As Decimal
            If partyBalanceAmt.Rows.Count > 0 Then
                For Each row In partyBalanceAmt.Rows

                    If Not (TypeOf row("partybalance") Is DBNull) Then
                        partyBalance = Decimal.Parse(row("partybalance"))
                    Else
                        partyBalance = 0.0
                    End If

                Next


            End If
            arrHeaders = {"Total Net Amount", (totalbasecreedit - totalbasedebit).ToString(decno), "", "Total Party Balance", partyBalance.ToString("N2"), IIf((totalbasecreedit - totalbasedebit) > 0, "Cr", "Dr"), "", "Total " & Space(3) & "Base ", "Amount", totalbasedebit.ToString(decno), totalbasecreedit.ToString(decno)}
            rowCount = rowCount + 1
            ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Font.SetBold().Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 8
            ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.WrapText = True

            For i = 0 To arrHeaders.Length - 1
                If i = 1 Or i = 4 Or i = 9 Or i = 10 Then
                    ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                    ws.Cell(rowCount, i + 1).Style.NumberFormat.Format = DecimalPoints
                ElseIf i = 5 Then
                    ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                    ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                Else

                    ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                End If

            Next

        End If
        ws.Cell((rowCount + 1), 1).Value = "Printed Date:" & Now.ToString("dd/MM/yyyy")
        ws.Range((rowCount + 1), 1, (rowCount + 1), 4).Merge()
        Using wStream As New MemoryStream()
            wb.SaveAs(wStream)
            bytes = wStream.ToArray()
        End Using



    End Sub
#End Region
End Class
