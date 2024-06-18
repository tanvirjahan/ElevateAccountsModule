Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq
Imports System.Globalization
Imports ClosedXML.Excel

'GenerateReport(fromdate, todate, divcode, pagetype, type, currflag, bytes, "download")
Public Class ClsCashBankBookPdf

    Inherits System.Web.UI.Page
    Dim objutils As New clsUtils
#Region "global declaration"
    Dim objclsUtilities As New clsUtils
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
    ' Dim documentWidth As Single = 770.0F
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
    Public Sub GenerateReport(ByVal reportsType As String, ByVal fromdate As String, ByVal todate As String, ByVal divcode As String, ByVal pagetype As String, ByVal type As String, ByVal currflag As String, ByVal frmcode As String, ByVal frmbankname As String, ByVal inclpagebrk As Integer, ByVal cashbanktype As Integer, ByRef bytes() As Byte, ByVal printMode As String, Optional ByVal fileName As String = "")

        Try
            If divcode <> "" Then
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
            Else
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
            End If
            If type = "0" Then
                rptreportname = "Cash/Bank book --Summary"
            Else
                rptreportname = "Cash/Bank book --Detailed"
            End If

            rptreportname = rptreportname & Space(2) & "From" & Space(2) & Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy") & Space(2) & "To" & Space(2) & Convert.ToDateTime(todate).ToString("dd/MM/yyyy")
            'If Not (String.IsNullOrEmpty(frmcode)) Then
            '    fromname = frmbankname ' objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where bankyn='Y' and acctcode='" & frmcode & "' ")
            '    rptfilter = "For Bank " + fromname
            'End If
            'If currflag = "0" Then

            '    currname = Trim(CType(objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select currcode from acctmast where bankyn='Y'  and acctcode='" + frmcode + "'"), String))
            'Else
            '    currname = Trim(CType(objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String))

            'End If
            decno = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
            decno = "N" + decno
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet


            'Dim pdebit, pcredit, cdebit, ccredit, tdebit, tcredit As Decimal
            'Dim fpdebit, fpcredit, fcdebit, fccredit, ftdebit, ftcredit As Decimal
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("SP_BANKBOOK_new", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 20)).Value = divcode
            mySqlCmd.Parameters.Add(New SqlParameter("@bankcode", SqlDbType.VarChar, 20)).Value = frmcode
            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@type", SqlDbType.Int)).Value = type
            mySqlCmd.Parameters.Add(New SqlParameter("@currflg", SqlDbType.Int)).Value = currflag
            If cashbanktype = 1 Then
                mySqlCmd.Parameters.Add(New SqlParameter("@cashbanktype", SqlDbType.Int)).Value = 2
            Else
                mySqlCmd.Parameters.Add(New SqlParameter("@cashbanktype", SqlDbType.Int)).Value = 1
            End If



            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim custdetailsdt As New DataTable
            custdetailsdt = ds.Tables(0)

            Dim dataView As New DataView(custdetailsdt)
            dataView.Sort = "trandate ASC"
            custdetailsdt = dataView.ToTable()

            Dim tableTitle As PdfPTable = Nothing
            Dim arrHeaders() As String = Nothing
            If type = "0" Then
                tableTitle = New PdfPTable(7)
                'arrHeaders = {"DATE", "DOC. NO", "Description", "CHEQUE NO.", "RECEIPTS" & Environment.NewLine & "(" & currname & ")", "PAYMENTS" & Environment.NewLine & "(" & currname & ")", "BALANCE" & Environment.NewLine & "(" & currname & ")"}
                tableTitle.SetWidths(New Single() {0.1F, 0.12F, 0.24F, 0.13F, 0.13F, 0.13F, 0.15F})


            ElseIf type = "1" Then
                tableTitle = New PdfPTable(8)
                'arrHeaders = {"DATE", "DOC. NO", "NAME", "CHEQUE NO.", "RECEIPTS" & Environment.NewLine & "(" & currname & ")", "PAYMENTS" & Environment.NewLine & "(" & currname & ")", "BALANCE" & Environment.NewLine & "(" & currname & ")", "Description"}
                tableTitle.SetWidths(New Single() {0.09F, 0.11F, 0.19F, 0.1F, 0.13F, 0.12F, 0.12F, 0.14F})
            End If
            ' decno = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
            If reportsType = "excel" Then
                ExcelReport(type, custdetailsdt, arrHeaders, currflag, bytes, inclpagebrk)
            Else

                Dim document As New Document(PageSize.A4, -10.0F, 10.0F, 30.0F, 35.0F)
                If type = "1" Then
                    documentWidth = 770.0F
                    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)
                End If

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


                    'If inclpagebrk = 1 Then
                    '    tableTitle.TotalWidth = documentWidth
                    '    tableTitle.LockedWidth = True
                    '    tableTitle.SplitRows = False
                    '    tableTitle.KeepTogether = True

                    '    For i = 0 To arrHeaders.Length - 1
                    '        phrase = New Phrase()
                    '        phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
                    '        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    '        cell.SetLeading(12, 0)
                    '        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    '        cell.PaddingBottom = 4.0F
                    '        cell.PaddingTop = 1.0F
                    '        tableTitle.AddCell(cell)
                    '    Next
                    '    tableTitle.SpacingBefore = 20
                    '    tableTitle.SpacingAfter = 0
                    'End If
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

                    'writer.PageEvent = New ClsHeaderFooter(titletable, Reporttitle, FooterTable, Nothing)
                    writer.PageEvent = New ClsHeaderFooter(titletable, Reporttitle, FooterTable, Nothing, "printDate")

                    document.Open()
                    'custdetailsdt.DefaultView.Sort = "acctcode ASC"

                    If custdetailsdt.Rows.Count > 0 Then
                        If type = "1" Then

                            cashBankdetail(custdetailsdt, currflag, document, inclpagebrk, arrHeaders)
                        Else
                            Dim tableTotal As PdfPTable = Nothing
                            Dim arrTotal() As String = Nothing
                            Dim groups = From CashBankBook In custdetailsdt.AsEnumerable() Group CashBankBook By g = New With {Key .accnames = CashBankBook.Field(Of String)("accname")} Into Group Order By g.accnames

                            ' Dim groups = From CashBankBook In custdetailsdt.AsEnumerable() Group CashBankBook By g = New With {Key .trandate = CashBankBook.Field(Of String)("accname"), Key .tranid = CashBankBook.Field(Of String)("tranid")} Into Group Order By g.trandate
                            Dim groupnames = From CashBankBook In custdetailsdt.AsEnumerable() Group CashBankBook By GroupName = New With {Key .GroupName = CashBankBook.Field(Of String)("accname")} Into Group
                            Dim rowcount As Integer = 0
                            For Each gpbyrow In groups
                                'totalcredit = 0
                                'totaldebit = 0

                                '  For Each row In custdetailsdt.Rows

                                If currflag = "0" Then

                                    currname = Trim(CType(objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select currcode from acctmast where bankyn='Y'  and acctcode='" + gpbyrow.Group(0)("acccode") + "'"), String))
                                Else
                                    currname = Trim(CType(objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String))

                                End If

                                arrHeaders = {"DATE", "DOC. NO", "Description", "CHEQUE NO.", "RECEIPTS" & Environment.NewLine & "(" & currname & ")", "PAYMENTS" & Environment.NewLine & "(" & currname & ")", "BALANCE" & Environment.NewLine & "(" & currname & ")"}

                                Dim tableData As PdfPTable = Nothing
                                Dim tableheader As PdfPTable = Nothing
                                Dim arrdata() As String = Nothing
                                Dim tdate As String = Nothing




                                tableheader = New PdfPTable(7)
                                ' arrHeaders = {"DATE", "DOC. NO", "Description", "CHEQUE NO.", "RECEIPTS" & Environment.NewLine & vbLf & "(" & currname & ")", "PAYMENTS" & Environment.NewLine & vbLf & "(" & currname & ")", "BALANCE" & Environment.NewLine & vbLf & "(" & currname & ")"}
                                tableheader.SetWidths(New Single() {0.1F, 0.12F, 0.24F, 0.13F, 0.13F, 0.13F, 0.15F})





                                tableheader.TotalWidth = documentWidth
                                tableheader.LockedWidth = True
                                tableheader.SplitRows = False
                                tableheader.KeepTogether = True
                                phrase = New Phrase()
                                phrase.Add(New Chunk(gpbyrow.g.accnames.ToUpper(), normalfontbold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 7, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 10.0F
                                tableheader.AddCell(cell)


                                For i = 0 To arrHeaders.Length - 1
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                    cell.SetLeading(12, 0)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                    cell.PaddingBottom = 4.0F
                                    cell.PaddingTop = 1.0F
                                    tableheader.AddCell(cell)
                                Next
                                document.Add(tableheader)
                                tableheader.SpacingBefore = 10
                                'tableheader.SpacingAfter = 0
                                For Each row In gpbyrow.Group

                                    tdate = Format(Convert.ToDateTime(row("trandate").ToString()), "dd/MM/yyyy")
                                    debit = IIf(currflag = 1, Decimal.Parse(row("debit_base")), Decimal.Parse(row("debit")))
                                    credit = IIf(currflag = 1, Decimal.Parse(row("credit_base")), Decimal.Parse(row("credit")))
                                    totaldebit = totaldebit + debit
                                    totalcredit = totalcredit + credit
                                    tableData = New PdfPTable(7)

                                    arrdata = {tdate, row("tranid"), row("narration"), row("chequeno"), debit.ToString(decno), credit.ToString(decno), IIf(totaldebit - totalcredit >= 0, (Math.Abs(totaldebit - totalcredit)).ToString(decno) & " DR", (Math.Abs(totaldebit - totalcredit)).ToString(decno) & " CR")}
                                    tableData.SetWidths(New Single() {0.1F, 0.12F, 0.24F, 0.13F, 0.13F, 0.13F, 0.15F})

                                    tableTotal = New PdfPTable(7)
                                    tableTotal.SetWidths(New Single() {0.1F, 0.12F, 0.24F, 0.13F, 0.13F, 0.13F, 0.15F})


                                    tableData.TotalWidth = documentWidth
                                    tableData.LockedWidth = True
                                    tableData.SplitRows = False
                                    tableData.KeepTogether = True


                                    For i = 0 To arrdata.Length - 1
                                        phrase = New Phrase()
                                        phrase.Add(New Chunk(arrdata(i), normalfont))
                                        If i < 4 Then
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                        Else
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                        End If

                                        cell.SetLeading(12, 0)
                                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                        cell.PaddingBottom = 4.0F
                                        cell.PaddingTop = 1.0F
                                        tableData.AddCell(cell)
                                    Next
                                    tableData.SpacingBefore = 0
                                    tableData.SpacingAfter = 0
                                    document.Add(tableData)



                               

                                Next

                                tableTotal.TotalWidth = documentWidth
                                tableTotal.LockedWidth = True
                                tableTotal.SplitRows = False
                                tableTotal.KeepTogether = True
                                tableTotal.SpacingBefore = 0
                                tableTotal.SpacingAfter = 20
                                arrTotal = {"", "", "", "Final Total", totaldebit.ToString(decno), totalcredit.ToString(decno), ""}
                                For i = 0 To arrTotal.Length - 1
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(arrTotal(i), normalfontbold))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                    cell.SetLeading(12, 0)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                    cell.PaddingBottom = 4.0F
                                    cell.PaddingTop = 1.0F
                                    tableTotal.AddCell(cell)
                                Next
                                document.Add(tableTotal)
                                '  tableTotal.SpacingAfter = 50
                                totaldebit = 0
                                totalcredit = 0

                                If inclpagebrk = 1 Then
                                    If rowcount <> groups.Count - 1 Then
                                             document.NewPage()
                                    End If
                                End If
                                rowcount += 1
                            Next

                            ''If inclpagebrk = 0 Then

                            'tableTotal.TotalWidth = documentWidth
                            'tableTotal.LockedWidth = True
                            'tableTotal.SplitRows = False
                            'tableTotal.KeepTogether = True
                            'tableTotal.SpacingBefore = 0
                            'tableTotal.SpacingAfter = 0
                            'arrTotal = {"", "", "", "Final Total", totaldebit.ToString(decno), totalcredit.ToString(decno), ""}
                            'For i = 0 To arrTotal.Length - 1
                            '    phrase = New Phrase()
                            '    phrase.Add(New Chunk(arrTotal(i), normalfontbold))
                            '    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            '    cell.SetLeading(12, 0)
                            '    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                            '    cell.PaddingBottom = 4.0F
                            '    cell.PaddingTop = 1.0F
                            '    tableTotal.AddCell(cell)
                            'Next

                            'document.Add(tableTotal)
                            ''End If
                        End If
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

    Public Sub cashBankdetail(ByVal custdetailsdt As DataTable, ByVal currflag As String, ByRef document As Document, ByVal inclpagebrk As Integer, ByVal arrHeaders() As String)
        Dim tableTotal As PdfPTable = Nothing
        Dim arrTotal() As String = Nothing
        Dim Phrase

        Dim dataView As New DataView(custdetailsdt)
        dataView.Sort = "trandatec ASC"
        custdetailsdt = dataView.ToTable()

        ' Dim cust = custdetailsdt.AsEnumerable().OrderBy(Function(o) o.Field(Of Date)("trandate"))

        Dim groups = From CashBankBook In custdetailsdt.AsEnumerable() Group CashBankBook By g = New With {Key .accname = CashBankBook.Field(Of String)("accname")} Into Group
        '  Dim groups = From CashBankBook In cust.AsEnumerable() Group CashBankBook By g = New With {Key .tranid = CashBankBook.Field(Of String)("tranid")} Into Group Order By g.tranid

        Dim rptTotalDebit, rpttoatlCredit As Decimal
        Dim rowcount As Integer = 0
        For Each gpbyrow In groups


            If currflag = "0" Then

                currname = Trim(CType(objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select currcode from acctmast where bankyn='Y'  and acctcode='" + gpbyrow.Group(0)("acccode") + "'"), String))
            Else
                currname = Trim(CType(objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String))

            End If

            arrHeaders = {"DATE", "DOC. NO", "NAME", "CHEQUE NO.", "RECEIPTS" & Environment.NewLine & "(" & currname & ")", "PAYMENTS" & Environment.NewLine & "(" & currname & ")", "BALANCE" & Environment.NewLine & "(" & currname & ")", "Description"}
           

            totalcredit = 0
            totaldebit = 0
            Dim tableData As PdfPTable = Nothing
            Dim tableheader As PdfPTable = Nothing
            Dim arrdata() As String = Nothing
            Dim tdate As String = Nothing



            'Dim tblTitle As PdfPTable = New PdfPTable(1)
            'tblTitle.SetWidths(New Single() {1.0F})
            'tblTitle.TotalWidth = documentWidth
            'tblTitle.LockedWidth = True
            'Phrase = New Phrase()

            'rptreportname = gpbyrow.g.accname
            'Phrase.Add(New Chunk(rptreportname, headerfont))
            'cell = PhraseCell(Phrase, PdfPCell.ALIGN_LEFT, 1, False)
            'cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
            'cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            'cell.PaddingBottom = 3.0F

            'tblTitle.AddCell(cell)
            'tblTitle.SpacingBefore = 10
            'tblTitle.SpacingAfter = 12
            'document.Add(tblTitle)

            tableheader = New PdfPTable(8)
            ' arrHeaders = {"DATE", "DOC. NO", "Description", "CHEQUE NO.", "RECEIPTS" & Environment.NewLine & vbLf & "(" & currname & ")", "PAYMENTS" & Environment.NewLine & vbLf & "(" & currname & ")", "BALANCE" & Environment.NewLine & vbLf & "(" & currname & ")"}
            tableheader.SetWidths(New Single() {0.09F, 0.11F, 0.19F, 0.1F, 0.13F, 0.12F, 0.12F, 0.14F})



            tableheader.TotalWidth = documentWidth
            tableheader.LockedWidth = True
            tableheader.SplitRows = False
            tableheader.KeepTogether = True
            tableheader.SpacingBefore = 20
            'Dim tblTitle As PdfPTable = New PdfPTable(1)
            'tblTitle.SetWidths(New Single() {1.0F})
            'tblTitle.TotalWidth = documentWidth
            'tblTitle.LockedWidth = True
            Phrase = New Phrase()

            'rptreportname = gpbyrow.g.accname
            Phrase.Add(New Chunk(gpbyrow.g.accname.ToUpper(), normalfontbold))
            cell = PhraseCell(Phrase, PdfPCell.ALIGN_LEFT, 8, False)
            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
            cell.PaddingBottom = 10.0F

            tableheader.AddCell(cell)


            For i = 0 To arrHeaders.Length - 1
                Phrase = New Phrase()
                Phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
                cell = PhraseCell(Phrase, PdfPCell.ALIGN_CENTER, 1, True)
                cell.SetLeading(12, 0)
                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                cell.PaddingBottom = 4.0F
                cell.PaddingTop = 1.0F
                tableheader.AddCell(cell)
            Next
            document.Add(tableheader)
            tableheader.SpacingBefore = 60
            'tableheader.SpacingAfter = 0
            For Each row In gpbyrow.Group




                tdate = Format(Convert.ToDateTime(row("trandate").ToString()), "dd/MM/yyyy")
                debit = IIf(currflag = 1, Decimal.Parse(row("debit_base")), Decimal.Parse(row("debit")))
                credit = IIf(currflag = 1, Decimal.Parse(row("credit_base")), Decimal.Parse(row("credit")))
                totaldebit = totaldebit + debit
                totalcredit = totalcredit + credit

                rptTotalDebit = rptTotalDebit + debit
                rpttoatlCredit = rpttoatlCredit + credit

                tableData = New PdfPTable(8)
                Dim acctname As String = ""
                If row("receipt_received_from") <> "" Then
                    acctname = ", " + row("receipt_received_from")
                End If
                acctname = row("accname") + acctname
                arrdata = {tdate, row("tranid"), acctname, row("chequeno"), debit.ToString(decno), credit.ToString(decno), IIf(totaldebit - totalcredit >= 0, (Math.Abs(totaldebit - totalcredit)).ToString(decno) & " DR", (Math.Abs(totaldebit - totalcredit)).ToString(decno) & " CR"), row("narration")}

                tableData.SetWidths(New Single() {0.09F, 0.11F, 0.19F, 0.1F, 0.13F, 0.12F, 0.12F, 0.14F})
                tableData.TotalWidth = documentWidth
                tableData.LockedWidth = True
                tableData.SplitRows = False
                tableData.KeepTogether = True
                For i = 0 To arrdata.Length - 1
                    Phrase = New Phrase()
                    Phrase.Add(New Chunk(arrdata(i), normalfont))
                    If i < 4 Or i = 7 Then
                        cell = PhraseCell(Phrase, PdfPCell.ALIGN_LEFT, 1, True)
                    Else
                        cell = PhraseCell(Phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                    End If

                    cell.SetLeading(12, 0)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.PaddingBottom = 4.0F
                    cell.PaddingTop = 1.0F
                    tableData.AddCell(cell)
                Next
                tableData.SpacingBefore = 0
                tableData.SpacingAfter = 0
                document.Add(tableData)
            Next

            tableTotal = New PdfPTable(8)
            tableTotal.SetWidths(New Single() {0.09F, 0.11F, 0.19F, 0.1F, 0.13F, 0.12F, 0.12F, 0.14F})
            tableTotal.TotalWidth = documentWidth
            tableTotal.LockedWidth = True
            tableTotal.SplitRows = False
            tableTotal.KeepTogether = True
            tableTotal.SpacingBefore = 0
            tableTotal.SpacingAfter = 0
            arrTotal = {"", "", "", "Total", totaldebit.ToString(decno), totalcredit.ToString(decno), " ", ""}
            For i = 0 To arrTotal.Length - 1
                Phrase = New Phrase()
                Phrase.Add(New Chunk(arrTotal(i), normalfontbold))
                cell = PhraseCell(Phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                cell.SetLeading(12, 0)
                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                cell.PaddingBottom = 4.0F
                cell.PaddingTop = 1.0F
                tableTotal.AddCell(cell)

                document.Add(tableTotal)

            Next

            If inclpagebrk = 1 Then
                If rowcount <> groups.Count - 1 Then
                    '  If groups.cou Then
                    '  If custdetailsdt.Rows.IndexOf(row) <> custdetailsdt.Rows.Count - 1 Then
                    document.NewPage()
                End If
            End If
            rowcount += 1
        Next
        If inclpagebrk = 0 Then
            tableTotal = New PdfPTable(8)
            tableTotal.SetWidths(New Single() {0.09F, 0.11F, 0.19F, 0.1F, 0.13F, 0.12F, 0.12F, 0.14F})
            tableTotal.TotalWidth = documentWidth
            tableTotal.LockedWidth = True
            tableTotal.SplitRows = False
            tableTotal.KeepTogether = True
            tableTotal.SpacingBefore = 0
            tableTotal.SpacingAfter = 0
            arrTotal = {"", "", "", "REPORT TOTAL", rptTotalDebit.ToString(decno), rpttoatlCredit.ToString(decno), "", ""}
            For i = 0 To arrTotal.Length - 1
                Phrase = New Phrase()
                Phrase.Add(New Chunk(arrTotal(i), normalfontbold))
                cell = PhraseCell(Phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                cell.SetLeading(12, 0)
                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                If i = 4 Or i = 5 Then
                    cell.BackgroundColor = totalbg
                End If
                cell.PaddingBottom = 4.0F
                cell.PaddingTop = 1.0F
                tableTotal.AddCell(cell)
            Next
            document.Add(tableTotal)
        End If
    End Sub

#Region "ExcelReport"

    Public Sub ExcelReport(ByVal type As String, ByVal custdetailsdt As DataTable, ByVal arrHeading() As String, ByVal currflag As String, ByRef bytes() As Byte, ByVal pagebreak As Integer)
        Dim col As Integer
        Dim DecimalPoint, companyCol, reportCol, filtercol As String
        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add("CashBankBook")



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
        ws.PageSetup.PageOrientation = XLPageOrientation.Landscape
        ws.PageSetup.PaperSize = XLPaperSize.A4Paper
        ws.PageSetup.SetRowsToRepeatAtTop(1, 3)



        Dim rowcount As Integer = 3
        ws.Columns.AdjustToContents()
        ws.Column("A").Width = 14
        ws.Column("B").Width = 16
        ws.Column("C").Width = 22
        ws.Columns("D:G").Width = 14
        ws.Column("H").Width = 22

        If decno = "N1" Then
            DecimalPoint = "##,##,##,##0.0"
        ElseIf decno = "N2" Then
            DecimalPoint = "##,##,##,##0.00"
        ElseIf decno = "N3" Then
            DecimalPoint = "##,##,##,##0.000"
        ElseIf decno = "N4" Then
            DecimalPoint = "##,##,##,##0.0000"
        Else
            DecimalPoint = "##,##,##,##0.00"

        End If
        If type = "0" Then
            companyCol = "A2:G2"
            reportCol = "A3:G3"
            filtercol = "A4:G4"
        Else
            companyCol = "A2:H2"
            reportCol = "A3:H3"
            filtercol = "A4:H4"
        End If
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

        'Report Filter Heading
        'ws.Cell("A4").Value = rptfilter
        'Dim filter = ws.Range(filtercol).Merge()
        'filter.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
        'filter.Style.Font.SetBold().Font.FontSize = 14


        Dim arrHeaders() As String

        If custdetailsdt.Rows.Count > 0 Then
            If type = "1" Then
                cashBankdetailExcel(custdetailsdt, currflag, rowcount, DecimalPoint, ws, arrHeading, pagebreak)
            Else

                Dim groups = From CashBankBook In custdetailsdt.AsEnumerable() Group CashBankBook By g = New With {Key .accnames = CashBankBook.Field(Of String)("accname")} Into Group Order By g.accnames

                ' Dim groups = From CashBankBook In custdetailsdt.AsEnumerable() Group CashBankBook By g = New With {Key .trandate = CashBankBook.Field(Of String)("accname"), Key .tranid = CashBankBook.Field(Of String)("tranid")} Into Group Order By g.trandate
                Dim groupnames = From CashBankBook In custdetailsdt.AsEnumerable() Group CashBankBook By GroupName = New With {Key .GroupName = CashBankBook.Field(Of String)("accname")} Into Group


                Dim arrTotal() As String = Nothing
                For Each gpbyrow In groups

                    If currflag = "0" Then

                        currname = Trim(CType(objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select currcode from acctmast where bankyn='Y'  and acctcode='" + gpbyrow.Group(0)("acccode") + "'"), String))
                    Else
                        currname = Trim(CType(objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String))

                    End If

                    arrHeading = {"DATE", "DOC. NO", "Description", "CHEQUE NO.", "RECEIPTS" & Environment.NewLine & "(" & currname & ")", "PAYMENTS" & Environment.NewLine & "(" & currname & ")", "BALANCE" & Environment.NewLine & "(" & currname & ")"}

                    rowcount = rowcount + 2
                    ws.Range(rowcount, 1, rowcount, arrHeading.Length).Merge()
                    ws.Range(rowcount, 1, rowcount, arrHeading.Length).Value = gpbyrow.g.accnames.ToUpper()
                    'ws.Cells(rowcount, 1).Value = gpbyrow.g.accnames
                    rowcount = rowcount + 2
                    ws.Range(rowcount, 1, rowcount, arrHeading.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
                    ws.Range(rowcount, 1, rowcount, arrHeading.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    ws.Range(rowcount, 1, rowcount, arrHeading.Length).Style.Alignment.WrapText = True
                    ws.Range(rowcount, 1, rowcount, arrHeading.Length).Style.Font.Bold = True
                    For i = 0 To arrHeading.Length - 1
                        ws.Cell(rowcount, i + 1).Value = arrHeading(i)
                        ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    Next

                    For Each row In gpbyrow.Group


                        '  For Each row In custdetailsdt.Rows
                        Dim tdate As String = Nothing
                        '   If type = "0" Then
                        tdate = Format(Convert.ToDateTime(row("trandate").ToString()), "dd/MM/yyyy")
                        debit = IIf(currflag = 1, Decimal.Parse(row("debit_base")), Decimal.Parse(row("debit")))
                        credit = IIf(currflag = 1, Decimal.Parse(row("credit_base")), Decimal.Parse(row("credit")))
                        totaldebit = totaldebit + debit
                        totalcredit = totalcredit + credit
                        arrHeaders = {tdate, row("tranid"), row("narration"), row("chequeno"), debit.ToString(decno), credit.ToString(decno), IIf(totaldebit - totalcredit >= 0, Math.Abs((totaldebit - totalcredit)).ToString(decno) & " DR", Math.Abs((totaldebit - totalcredit)).ToString(decno) & " CR")}

                        rowcount = rowcount + 1
                        ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
                        ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                        ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Alignment.WrapText = True
                        For i = 0 To arrHeaders.Length - 1

                            If i < 4 Then
                                'If i = 3 Then
                                '    ws.Range(rowcount, i + 2).Style.Border.SetRightBorder(XLBorderStyleValues.None)
                                'End If
                                ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
                                ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            ElseIf i = 4 Or i = 5 Then
                                'If i = 4 Then
                                '    ws.Cell(rowcount, i + 2).Style.Border.SetRightBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.None)
                                'End If
                                ws.Cell(rowcount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                                ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                                ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoint
                            Else
                                ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
                                ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                            End If
                        Next



                    Next
                    arrHeaders = {"", "", "", "Final Total", totaldebit.ToString(decno), totalcredit.ToString(decno), ""}
                    rowcount = rowcount + 1
                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    ' ws.AddPcture()
                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Alignment.WrapText = True
                    For i = 0 To arrHeaders.Length - 1
                        If i = 4 Or i = 5 Then
                            ws.Cell(rowcount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                            ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoint
                        Else
                            ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
                        End If
                        ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    Next
                    totalcredit = 0
                    totaldebit = 0
                    If pagebreak = 1 Then
                        If rowcount <> groups.Count - 1 Then
                            ws.PageSetup.AddHorizontalPageBreak(rowcount)
                            ws.PageSetup.AddVerticalPageBreak(7)
                        End If
                    End If
                Next

            End If
        End If
        ws.Cell((rowcount + 2), 1).Value = "Printed Date:" & Now.ToString("dd/MM/yyyy")
        ws.Range((rowcount + 2), 1, (rowcount + 2), 3).Merge()
        Using wStream As New MemoryStream()
            wb.SaveAs(wStream)
            bytes = wStream.ToArray()
        End Using

    End Sub
#End Region


    'Public Sub cashBankdetailExcel(ByVal custdetailsdt As DataTable, ByVal currflag As String, ByRef rowCount As Integer, ByVal DecimalPoint As String, ByRef ws As IXLWorksheet)


    '    Dim arrTotal() As String = Nothing
    '    Dim Phrase

    '    Dim dataView As New DataView(custdetailsdt)
    '    dataView.Sort = "trandatec ASC"
    '    custdetailsdt = dataView.ToTable()

    '    ' Dim groups = From CashBankBook In custdetailsdt.AsEnumerable() Group CashBankBook By g = New With {Key .tranid = CashBankBook.Field(Of String)("tranid")} Into Group Order By g.tranid

    '    Dim rptTotalDebit, rpttoatlCredit As Decimal
    '    'For Each gpbyrow In groups
    '    '    totalcredit = 0
    '    '    totaldebit = 0
    '    For Each row In custdetailsdt.Rows

    '        Dim arrdata() As String = Nothing
    '        Dim tdate As String = Nothing
    '        tdate = Format(Convert.ToDateTime(row("trandate").ToString()), "dd/MM/yyyy")
    '        debit = IIf(currflag = 1, Decimal.Parse(row("debit_base")), Decimal.Parse(row("debit")))
    '        credit = IIf(currflag = 1, Decimal.Parse(row("credit_base")), Decimal.Parse(row("credit")))
    '        totaldebit = totaldebit + debit
    '        totalcredit = totalcredit + credit

    '        rptTotalDebit = rptTotalDebit + debit
    '        rpttoatlCredit = rpttoatlCredit + credit


    '        arrdata = {tdate, row("tranid"), row("accname"), row("chequeno"), debit.ToString(decno), credit.ToString(decno), "", row("narration")}
    '        rowCount = rowCount + 1
    '        ws.Range(rowCount, 1, rowCount, arrdata.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
    '        ws.Range(rowCount, 1, rowCount, arrdata.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '        ws.Range(rowCount, 1, rowCount, arrdata.Length).Style.Alignment.WrapText = True
    '        For i = 0 To arrdata.Length - 1
    '            Phrase = New Phrase()
    '            Phrase.Add(New Chunk(arrdata(i), normalfont))
    '            If i = 4 Or i = 5 Then
    '                ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrdata(i))
    '                ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                ws.Cell(rowCount, i + 1).Style.NumberFormat.Format = DecimalPoint

    '            Else
    '                ws.Cell(rowCount, i + 1).Value = arrdata(i)
    '                ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)

    '            End If
    '        Next
    '    Next


    '    'arrTotal = {"", "", "", "", totaldebit.ToString(decno), totalcredit.ToString(decno), IIf(totaldebit - totalcredit >= 0, Math.Abs((totaldebit - totalcredit)).ToString(decno) & "DR", Math.Abs((totaldebit - totalcredit)).ToString(decno) & "CR"), ""}
    '    'rowCount = rowCount + 1
    '    'ws.Range(rowCount, 1, rowCount, arrTotal.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
    '    'ws.Range(rowCount, 1, rowCount, arrTotal.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '    'ws.Range(rowCount, 1, rowCount, arrTotal.Length).Style.Alignment.WrapText = True
    '    'For i = 0 To arrTotal.Length - 1
    '    '    If i = 4 Or i = 5 Then
    '    '        ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrTotal(i))
    '    '        ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '    '        ws.Cell(rowCount, i + 1).Style.NumberFormat.Format = DecimalPoint

    '    '    Else
    '    '        ws.Cell(rowCount, i + 1).Value = arrTotal(i)
    '    '        ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

    '    '    End If
    '    'Next

    '    ' Next

    '    arrTotal = {"", "", "", "REPORT TOTAL", rptTotalDebit.ToString(decno), rpttoatlCredit.ToString(decno), "", ""}
    '    rowCount = rowCount + 1
    '    ws.Range(rowCount, 1, rowCount, arrTotal.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
    '    ws.Range(rowCount, 1, rowCount, arrTotal.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '    ws.Range(rowCount, 1, rowCount, arrTotal.Length).Style.Alignment.WrapText = True
    '    For i = 0 To arrTotal.Length - 1

    '        If i = 4 Or i = 5 Then
    '            ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrTotal(i))
    '            ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Fill.SetBackgroundColor(XLColor.LightGray)
    '            ws.Cell(rowCount, i + 1).Style.NumberFormat.Format = DecimalPoint

    '        Else
    '            ws.Cell(rowCount, i + 1).Value = arrTotal(i)
    '            ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

    '        End If
    '    Next

    'End Sub




    Public Sub cashBankdetailExcel(ByVal custdetailsdt As DataTable, ByVal currflag As String, ByRef rowCount As Integer, ByVal DecimalPoint As String, ByRef ws As IXLWorksheet, ByVal arrHeading() As String, ByVal pagebreak As Integer)


        Dim arrTotal() As String = Nothing
        Dim Phrase

        Dim dataView As New DataView(custdetailsdt)
        dataView.Sort = "trandatec ASC"
        custdetailsdt = dataView.ToTable()

        ' Dim groups = From CashBankBook In custdetailsdt.AsEnumerable() Group CashBankBook By g = New With {Key .tranid = CashBankBook.Field(Of String)("tranid")} Into Group Order By g.tranid

        Dim groups = From CashBankBook In custdetailsdt.AsEnumerable() Group CashBankBook By g = New With {Key .accname = CashBankBook.Field(Of String)("accname")} Into Group

        Dim rptTotalDebit, rpttoatlCredit As Decimal
        For Each gpbyrow In groups

            If currflag = "0" Then

                currname = Trim(CType(objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select currcode from acctmast where bankyn='Y'  and acctcode='" + gpbyrow.Group(0)("acccode") + "'"), String))
            Else
                currname = Trim(CType(objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String))

            End If

            arrHeading = {"DATE", "DOC. NO", "NAME", "CHEQUE NO.", "RECEIPTS" & Environment.NewLine & "(" & currname & ")", "PAYMENTS" & Environment.NewLine & "(" & currname & ")", "BALANCE" & Environment.NewLine & "(" & currname & ")", "Description"}


            totalcredit = 0
            totaldebit = 0
            rowCount = rowCount + 2
            ws.Range(rowCount, 1, rowCount, arrHeading.Length).Merge()
            ws.Range(rowCount, 1, rowCount, arrHeading.Length).Value = gpbyrow.g.accname.ToUpper()
            ws.Range(rowCount, 1, rowCount, arrHeading.Length).Style.Font.Bold = True
            'ws.Cells(rowcount, 1).Value = gpbyrow.g.accnames
            rowCount = rowCount + 2
            ws.Range(rowCount, 1, rowCount, arrHeading.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
            ws.Range(rowCount, 1, rowCount, arrHeading.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(rowCount, 1, rowCount, arrHeading.Length).Style.Alignment.WrapText = True
            For i = 0 To arrHeading.Length - 1
                ws.Cell(rowCount, i + 1).Value = arrHeading(i)
                ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            Next

            For Each row In gpbyrow.Group

                Dim arrdata() As String = Nothing
                Dim tdate As String = Nothing
                tdate = Format(Convert.ToDateTime(row("trandate").ToString()), "dd/MM/yyyy")
                debit = IIf(currflag = 1, Decimal.Parse(row("debit_base")), Decimal.Parse(row("debit")))
                credit = IIf(currflag = 1, Decimal.Parse(row("credit_base")), Decimal.Parse(row("credit")))
                totaldebit = totaldebit + debit
                totalcredit = totalcredit + credit

                rptTotalDebit = rptTotalDebit + debit
                rpttoatlCredit = rpttoatlCredit + credit
                Dim acctname As String = ""
                If row("receipt_received_from") <> "" Then
                    acctname = ", " + row("receipt_received_from")
                End If
                acctname = row("accname") + acctname

                arrdata = {tdate, row("tranid"), acctname, row("chequeno"), debit.ToString(decno), credit.ToString(decno), IIf(totaldebit - totalcredit >= 0, Math.Abs((totaldebit - totalcredit)).ToString(decno) & " DR", Math.Abs((totaldebit - totalcredit)).ToString(decno) & " CR"), row("narration")}
                rowCount = rowCount + 1
                ws.Range(rowCount, 1, rowCount, arrdata.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
                ws.Range(rowCount, 1, rowCount, arrdata.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(rowCount, 1, rowCount, arrdata.Length).Style.Alignment.WrapText = True
                For i = 0 To arrdata.Length - 1
                    Phrase = New Phrase()
                    Phrase.Add(New Chunk(arrdata(i), normalfont))
                    If i = 4 Or i = 5 Then
                        ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrdata(i))
                        ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ws.Cell(rowCount, i + 1).Style.NumberFormat.Format = DecimalPoint

                    Else
                        ws.Cell(rowCount, i + 1).Value = arrdata(i)
                        ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)

                    End If
                Next
            Next

            ws.Columns("G").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

            arrTotal = {"", "", "", "", totaldebit.ToString(decno), totalcredit.ToString(decno), " ", ""}
            rowCount = rowCount + 1
            ws.Range(rowCount, 1, rowCount, arrTotal.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
            ws.Range(rowCount, 1, rowCount, arrTotal.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(rowCount, 1, rowCount, arrTotal.Length).Style.Alignment.WrapText = True
            For i = 0 To arrTotal.Length - 1
                If i = 4 Or i = 5 Then
                    ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrTotal(i))
                    ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ws.Cell(rowCount, i + 1).Style.NumberFormat.Format = DecimalPoint

                Else
                    ws.Cell(rowCount, i + 1).Value = arrTotal(i)
                    ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                End If
            Next
            If pagebreak = 1 Then
                ws.PageSetup.AddHorizontalPageBreak(rowCount)
                ws.PageSetup.AddVerticalPageBreak(8)
            End If
        Next
        If pagebreak = 0 Then
            arrTotal = {"", "", "", "REPORT TOTAL", rptTotalDebit.ToString(decno), rpttoatlCredit.ToString(decno), "", ""}
            rowCount = rowCount + 1
            ws.Range(rowCount, 1, rowCount, arrTotal.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
            ws.Range(rowCount, 1, rowCount, arrTotal.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(rowCount, 1, rowCount, arrTotal.Length).Style.Alignment.WrapText = True
            For i = 0 To arrTotal.Length - 1

                If i = 4 Or i = 5 Then
                    ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrTotal(i))
                    ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Fill.SetBackgroundColor(XLColor.LightGray)
                    ws.Cell(rowCount, i + 1).Style.NumberFormat.Format = DecimalPoint

                Else
                    ws.Cell(rowCount, i + 1).Value = arrTotal(i)
                    ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                End If
            Next
        End If
    End Sub
End Class