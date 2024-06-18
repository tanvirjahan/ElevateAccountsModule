Imports Microsoft.VisualBasic
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient

Public Class ClsCashVoucherPdf
    Inherits System.Web.UI.Page
    Dim objutils As New clsUtils
#Region "global declaration"
    Dim objclsUtilities As New clsUtils
    Dim count, rowcount, pageorintation As Integer
    Dim Reportname, strSql, decimalPoint As String
    Dim normalfont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
    Dim caption As Font = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.BLACK)
    Dim normalfontbold As Font = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)
    Dim headerfont As Font = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK)
    Dim cheaderfontBold As Font = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK)
    Dim cheaderfont As Font = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK)
    Dim Titlebg As BaseColor = New BaseColor(143, 143, 143)
    Dim subTitlebg As BaseColor = New BaseColor(234, 234, 234)
    Dim totaldebit, totalcredit, amount As Decimal
    Dim userName, currency, decno, fractionword, decimalword, decimalInword, Baiza, receipt_date, basedebit As String
    Dim convrate As Decimal
    Dim documentWidth As Single = 550.0F
    Dim totalbg As BaseColor = New BaseColor(255, 188, 155)

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
    Public Sub GenerateReport(ByVal trantype As String, ByVal tranid As String, ByVal divcode As String, ByVal CashBankType As String, ByVal PrntSec As Integer, ByVal PrinDocTitle As String, ByRef bytes() As Byte, ByVal printMode As String)

        Try
            Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 30.0F, 35.0F)
            
            Dim cashVoucher As New DataTable
            Dim conn1 As New SqlConnection
            conn1 = clsDBConnect.dbConnectionnew("strDBConnection")

            Dim lsCtry As String = CType(objutils.GetString(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=459"), String)

            Dim coadd1 As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coadd1 from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim coadd2 As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coadd2 from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim copobox As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select copobox from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim cotel As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select cotel from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim cofax As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select cofax from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim coemail As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coemail from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim coweb As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coweb from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim TRNNo As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select TRNNo from Columbusmaster where div_code='" & divcode & "'"), String)

            Dim sqlcmd As New SqlCommand("sp_rpt_payment", conn1)
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = tranid
            sqlcmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
            sqlcmd.Parameters.Add(New SqlParameter("@trantype", SqlDbType.VarChar, 20)).Value = trantype
            Using dad As New SqlDataAdapter
                dad.SelectCommand = sqlcmd
                dad.Fill(cashVoucher)
            End Using
            clsDBConnect.dbCommandClose(sqlcmd)
            clsDBConnect.dbConnectionClose(conn1)

            Using memoryStream As New System.IO.MemoryStream()
                Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)

                Dim phrase As Phrase = Nothing
                Dim cell As PdfPCell = Nothing
                Dim tableheader As PdfPTable = Nothing

                'Header Table
                tableheader = New PdfPTable(2)
                tableheader.TotalWidth = documentWidth
                tableheader.LockedWidth = True
                tableheader.SetWidths(New Single() {0.75F, 0.25F})

                tableheader.Complete = False
                tableheader.SplitRows = False
                tableheader.SpacingBefore = 10.0F
                tableheader.WidthPercentage = 100
                'company name
                If divcode = "02" Then
                    cell = ImageCell("~/Images/logo.jpg", 80.0F, PdfPCell.ALIGN_LEFT)
                Else
                    cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
                End If
                tableheader.AddCell(cell)

                phrase = New Phrase()
                phrase.Add(New Chunk(coadd1 & Environment.NewLine, normalfont))
                phrase.Add(New Chunk(coadd2 & Environment.NewLine, normalfont))
                phrase.Add(New Chunk(copobox & Environment.NewLine & vbLf, normalfont))
                phrase.Add(New Chunk("Email :" & Space(6) & coemail & Environment.NewLine, normalfont))
                phrase.Add(New Chunk("Tel     :" & Space(6) & cotel & Environment.NewLine, normalfont))
                phrase.Add(New Chunk("Fax    :" & Space(6) & cofax & Environment.NewLine, normalfont))
                phrase.Add(New Chunk("Web   :" & Space(6) & coweb & Environment.NewLine, normalfont))
                phrase.Add(New Chunk("TRN   :" & Space(6) & TRNNo & Environment.NewLine, normalfont))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 5.0F
                cell.SetLeading(12, 0)
                tableheader.AddCell(cell)

                Reportname = "Payment Voucher" 'PrinDocTitle
                Dim tblTitle As PdfPTable = New PdfPTable(1)
                tblTitle.SetWidths(New Single() {1.0F})
                tblTitle.TotalWidth = documentWidth
                tblTitle.LockedWidth = True
                phrase = New Phrase()
                phrase.Add(New Chunk(Reportname, headerfont))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                cell.PaddingBottom = 2.0F
                tblTitle.AddCell(cell)
                tblTitle.SpacingBefore = 10
                tblTitle.SpacingAfter = 10

                Dim FooterTable = New PdfPTable(1)
                FooterTable.TotalWidth = documentWidth
                FooterTable.LockedWidth = True
                FooterTable.SetWidths(New Single() {1.0F})
                FooterTable.Complete = False
                FooterTable.SplitRows = False
                phrase = New Phrase()
                phrase.Add(New Chunk("Printed Date:" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss"), normalfont))
                cell = New PdfPCell(phrase)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.Colspan = 2
                cell.SetLeading(12, 0)
                cell.PaddingBottom = 2
                FooterTable.AddCell(cell)
                FooterTable.Complete = True

                cell = Nothing
                'Add common header and footer part to every page

                writer.PageEvent = New ClsHeaderFooter(tableheader, tblTitle, FooterTable, Nothing, "Voucher")

                If cashVoucher.Rows.Count > 0 Then
                    rowcount = cashVoucher.Rows.Count
                    document.Open()
                    'currency = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
                    Dim paymentDs As DataSet = CType(objutils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select receipt_currency_id,receipt_currency_rate from receipt_master_new where tran_id='" & tranid & "' and tran_type='" & trantype & "' and div_id='" & divcode & "'"), DataSet)
                    Dim paymentDt As DataTable = paymentDs.Tables(0)
                    If paymentDt.Rows.Count > 0 Then
                        currency = paymentDt.Rows(0)("receipt_currency_id")
                        convrate = paymentDt.Rows(0)("receipt_currency_rate")
                    Else
                        currency = ""
                        convrate = 0
                    End If
                    decno = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), String)
                    decimalPoint = "N" & decno
                    Dim voucherAmt As Decimal

                    Dim tblData As PdfPTable = New PdfPTable(5)
                    For Each row In cashVoucher.Rows
                        If count = 0 Then
                            userName = row("UserName")
                            Dim tblcommon As PdfPTable
                            If trantype = "CPV" Then
                                tblcommon = New PdfPTable(4)
                                tblcommon.SetWidths(New Single() {0.15F, 0.5F, 0.15F, 0.2F})
                                tblcommon.TotalWidth = documentWidth
                                tblcommon.LockedWidth = True

                                phrase = New Phrase()
                                phrase.Add(New Chunk("Voucher No.", cheaderfontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                tblcommon.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk(row("tran_id").ToString(), cheaderfont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                tblcommon.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk("Date", cheaderfontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                tblcommon.AddCell(cell)

                                phrase = New Phrase()
                                receipt_date = (Format(Convert.ToDateTime(row("receipt_date").ToString()), "dd/MM/yyyy"))
                                phrase.Add(New Chunk(receipt_date, cheaderfont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                tblcommon.AddCell(cell)

                                'phrase.Add(New Chunk("Amount(" & currency & ")" & Space(5), cheaderfontBold))
                                'basedebit = Decimal.Parse(row(13)).ToString(decimalPoint)
                                'phrase.Add(New Chunk(basedebit & Environment.NewLine & vbLf, cheaderfont))

                                'phrase.Add(New Chunk("M.RV No." & Space(13), cheaderfontBold))
                                'phrase.Add(New Chunk(row("receipt_mrv").ToString() & Environment.NewLine & vbLf, cheaderfont))

                                phrase = New Phrase()
                                phrase.Add(New Chunk("Paid To", cheaderfontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                tblcommon.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk(row("receipt_received_from").ToString(), cheaderfont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 7.0F
                                cell.PaddingTop = 1.0F
                                cell.Colspan = 3
                                tblcommon.AddCell(cell)


                                phrase = New Phrase()
                                phrase.Add(New Chunk("Cash", cheaderfontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                tblcommon.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk(row(12).ToString(), cheaderfont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 7.0F
                                cell.PaddingTop = 1.0F
                                cell.Colspan = 3
                                tblcommon.AddCell(cell)

                                'phrase.Add(New Chunk("Amount" & Space(13), cheaderfontBold))

                                'Dim fraction As Decimal = Math.Round(Decimal.Parse(row(13)), 2) - Math.Truncate(Math.Round(Decimal.Parse(row(13)), 3))
                                voucherAmt = Math.Round(row(13) / convrate, Convert.ToInt32(decno))
                                decimalword = Math.Truncate(Decimal.Parse(voucherAmt)).ToString()
                                decimalInword = AmountInWords(decimalword)

                                'Modified Param 15/11/2018
                                Dim fraction As Decimal = Math.Round(Decimal.Parse(voucherAmt), Convert.ToInt32(decno), MidpointRounding.AwayFromZero) Mod 1
                                If fraction > 0 Then
                                    Dim arrFraction As String() = fraction.ToString.Split(".")
                                    If arrFraction.Length = 2 Then
                                        fraction = arrFraction(1)
                                    Else
                                        fraction = 0
                                    End If
                                End If
                                If fraction > 0 Then
                                    Dim fractionStr As String = fraction.ToString()
                                    While fractionStr.Length < decno
                                        fractionStr = fractionStr + "0"
                                    End While
                                    Dim coin As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select currcoin from currmast where currcode= '" & currency.Trim & "'"), String)
                                    fractionword = "AND " & coin & " " & AmountInWords(fractionStr) + " " + "ONLY"
                                    'If decno = 3 Then
                                    '    fractionword = (fraction.ToString()).Split(New Char() {"."})(1) & "/1000" + " " + Baiza
                                    'Else
                                    '    fractionword = (fraction.ToString()).Split(New Char() {"."})(1) & "/100" + " " + "FILS ONLY"
                                    'End If
                                Else
                                    decimalInword = decimalInword + " " + "ONLY"
                                    fractionword = ""
                                End If
                                'phrase.Add(New Chunk(currency & Space(2) & decimalInword.ToUpper() & Space(2) & fractionword.ToUpper() & Environment.NewLine & vbLf, cheaderfont))

                            ElseIf trantype = "BPV" Then

                                tblcommon = New PdfPTable(6)
                                tblcommon.SetWidths(New Single() {0.14F, 0.24F, 0.14F, 0.17F, 0.14F, 0.17F})
                                tblcommon.TotalWidth = documentWidth
                                tblcommon.LockedWidth = True

                                phrase = New Phrase()
                                phrase.Add(New Chunk("Payment Voucher No.", cheaderfontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                tblcommon.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk(row("tran_id").ToString(), cheaderfont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                cell.Colspan = 3
                                tblcommon.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk("Date", cheaderfontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                tblcommon.AddCell(cell)

                                phrase = New Phrase()
                                receipt_date = (Format(Convert.ToDateTime(row("receipt_date").ToString()), "dd/MM/yyyy"))
                                phrase.Add(New Chunk(receipt_date, cheaderfont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                tblcommon.AddCell(cell)

                                'phrase.Add(New Chunk("Amount(" & currency & ")" & Space(5), cheaderfontBold))
                                'basedebit = Decimal.Parse(row(13)).ToString(decimalPoint)
                                'phrase.Add(New Chunk(basedebit & Environment.NewLine & vbLf, cheaderfont))

                                phrase = New Phrase()
                                phrase.Add(New Chunk("Paid To", cheaderfontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                tblcommon.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk(row("receipt_received_from").ToString(), cheaderfont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                cell.Colspan = 5
                                tblcommon.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk("Bank", cheaderfontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                tblcommon.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk(row(12).ToString(), cheaderfont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                tblcommon.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk("Cheque No.", cheaderfontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                tblcommon.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk(row("receipt_cheque_number").ToString(), cheaderfont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                tblcommon.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk("Cheque Date", cheaderfontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                tblcommon.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk((Format(Convert.ToDateTime(row("cheque_date").ToString()), "dd/MM/yyyy")), cheaderfont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 10.0F
                                cell.PaddingTop = 1.0F
                                tblcommon.AddCell(cell)
                                
                                'phrase.Add(New Chunk("Amount" & Space(13), cheaderfontBold))
                                'Dim fraction As Decimal = Math.Round(Decimal.Parse(row(13)), 2) - Math.Truncate(Math.Round(Decimal.Parse(row(13)), 3))
                                voucherAmt = Math.Round(row(13) / convrate, Convert.ToInt32(decno))
                                decimalword = Math.Truncate(Decimal.Parse(voucherAmt)).ToString()
                                decimalInword = AmountInWords(decimalword)
                                'Modified Param 15/11/2018
                                Dim fraction As Decimal = Math.Round(Decimal.Parse(voucherAmt), Convert.ToInt32(decno), MidpointRounding.AwayFromZero) Mod 1
                                If fraction > 0 Then
                                    Dim arrFraction As String() = fraction.ToString.Split(".")
                                    If arrFraction.Length = 2 Then
                                        fraction = arrFraction(1)
                                    Else
                                        fraction = 0
                                    End If
                                End If
                                If fraction > 0 Then
                                    Dim fractionStr As String = fraction.ToString()
                                    While fractionStr.Length < decno
                                        fractionStr = fractionStr + "0"
                                    End While
                                    Dim coin As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select currcoin from currmast where currcode= '" & currency.Trim & "'"), String)
                                    fractionword = "AND " & coin & " " & AmountInWords(fractionStr) + " " + "ONLY"
                                    'If decno = 3 Then
                                    '    fractionword = (fraction.ToString()).Split(New Char() {"."})(1) & "/1000" + " " + Baiza
                                    'Else
                                    '    fractionword = (fraction.ToString()).Split(New Char() {"."})(1) & "/100" + " " + "FILS ONLY"
                                    'End If

                                Else
                                    decimalInword = decimalInword + " " + "ONLY"
                                    fractionword = ""
                                End If
                                'phrase.Add(New Chunk(currency & Space(2) & decimalInword.ToUpper() & Space(2) & fractionword.ToUpper() & Environment.NewLine & vbLf, cheaderfont))

                                'phrase.Add(New Chunk("Cheque No." & Space(6), cheaderfontBold))
                                'phrase.Add(New Chunk(row("receipt_cheque_number").ToString() & Space(20), cheaderfont))
                                'phrase.Add(New Chunk("Bank/Branch" & Space(5), cheaderfontBold))
                                'phrase.Add(New Chunk(row(12).ToString() & Environment.NewLine & vbLf, cheaderfont))
                                'phrase.Add(New Chunk("Cheque Date" & Space(5), cheaderfontBold))
                                'phrase.Add(New Chunk((Format(Convert.ToDateTime(row("cheque_date").ToString()), "dd-MM-yyyy")), cheaderfont))

                            End If

                            tblcommon.SpacingBefore = 12
                            document.Add(tblcommon)

                            cell = Nothing
                            Dim tblHeader As PdfPTable = New PdfPTable(5)
                            tblHeader.SetWidths(New Single() {0.1F, 0.15F, 0.3F, 0.25F, 0.2F})
                            tblHeader.TotalWidth = documentWidth
                            tblHeader.LockedWidth = True
                            tableheader.DefaultCell.BorderWidth = 30

                            phrase = New Phrase()

                            Dim arrHeaders() As String = {"Sl.No.", "Account Code", "Account Name", "Narration", "Amount(" & currency & ")"}
                           
                            For i = 0 To arrHeaders.Length - 1

                                phrase = New Phrase()
                                phrase.Add(New Chunk(arrHeaders(i), cheaderfontBold))

                                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                cell.SetLeading(12, 0)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.PaddingBottom = 4.0F
                                cell.PaddingTop = 1.0F
                                tblHeader.AddCell(cell)
                            Next
                            tblHeader.SpacingBefore = 15
                            document.Add(tblHeader)
                            ' count = count + 1
                        End If
                        ' Data of table
                        tblData.SetWidths(New Single() {0.1F, 0.15F, 0.3F, 0.25F, 0.2F})
                        tblData.TotalWidth = documentWidth
                        tblData.LockedWidth = True
                        'If count = 0 Then
                        '    Dim arrData1() As String = {row("receipt_cashbank_code"), row(12) & Environment.NewLine & vbLf & row(6), IIf(row(2) = 0.0, "-", Decimal.Parse(row(2)).ToString(decimalPoint)), IIf(row(13) = 0.0, "-", Decimal.Parse(row(13)).ToString(decimalPoint))}

                        '    For i = 0 To arrData1.Length - 1

                        '        phrase = New Phrase()
                        '        phrase.Add(New Chunk(arrData1(i), normalfont))

                        '        If i > 1 Then
                        '            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        '            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                        '        Else
                        '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        '            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        '        End If

                        '        cell.SetLeading(12, 0)

                        '        cell.PaddingBottom = 4.0F
                        '        cell.PaddingTop = 1.0F
                        '        tblData.AddCell(cell)
                        '    Next

                        '    totaldebit = totaldebit + row(13)
                        '    totalcredit = totalcredit + row(2)
                        'End If
                        count = count + 1
                        Dim tmpAmt As Decimal = Math.Round((Val(row(3)) - Val(row(4))) / convrate, Convert.ToInt32(decno))
                        Dim arrData() As String = {row("tran_lineno"), row("receipt_acc_code"), row(5), row(10), IIf(tmpAmt = 0.0, "-", Decimal.Parse(tmpAmt).ToString(decimalPoint))}
                        For i = 0 To arrData.Length - 1

                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrData(i), normalfont))

                            If i > 3 Then
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                            Else
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                            End If

                            cell.SetLeading(12, 0)

                            cell.PaddingBottom = 3.0F
                            cell.PaddingTop = 1.0F
                            tblData.AddCell(cell)

                        Next
                        totaldebit = totaldebit + tmpAmt
                        'totalcredit = totalcredit + row(3)

                        'If count = rowcount Then
                        '    Dim arrData1() As String = {row("receipt_cashbank_code"), row(12) & Environment.NewLine & vbLf & row(6), IIf(row(2) = 0.0, "-", Decimal.Parse(row(2)).ToString(decimalPoint)), IIf(row(13) = 0.0, "-", Decimal.Parse(row(13)).ToString(decimalPoint))}

                        '    For i = 0 To arrData1.Length - 1

                        '        phrase = New Phrase()
                        '        phrase.Add(New Chunk(arrData1(i), normalfont))

                        '        If i > 1 Then
                        '            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        '            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                        '        Else
                        '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        '            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        '        End If

                        '        cell.SetLeading(12, 0)

                        '        cell.PaddingBottom = 4.0F
                        '        cell.PaddingTop = 1.0F
                        '        tblData.AddCell(cell)
                        '    Next

                        '    totaldebit = totaldebit + row(13)
                        '    totalcredit = totalcredit + row(2)
                        'End If

                    Next
                    'Total of basedebit and base credit
                    cell = Nothing
                    phrase = New Phrase()
                    Dim arrTotal() As String = {"The sum of " & currency & Space(2) & decimalInword.ToUpper() & Space(2) & fractionword.ToUpper(), totaldebit.ToString(decimalPoint)}
                    For i = 0 To arrTotal.Length - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrTotal(i), normalfontbold))

                        If i = 0 Then

                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.Colspan = 4
                        Else
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE

                        End If
                        'cell.BackgroundColor = totalbg
                        cell.PaddingBottom = 9.0F
                        cell.PaddingTop = 4.0F
                        tblData.AddCell(cell)

                    Next
                    document.Add(tblData)

                    cell = Nothing
                    Dim tbl = New PdfPTable(3)
                    tbl.TotalWidth = documentWidth
                    tbl.LockedWidth = True
                    tbl.SetWidths(New Single() {0.33F, 0.33F, 0.34F})
                    ' Christo. A - 03/01/19
                    'If lsCtry.Trim.ToUpper = "OM" Then
                    '    Dim arrOM() As String = {"Prepared By:", " Director of Finance:", " General Manager:", " Received By:"}
                    '    For i = 0 To arrOM.Length - 1
                    '        phrase = New Phrase()
                    '        phrase.Add(New Chunk(arrOM(i), cheaderfontBold))

                    '        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    '        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    '        cell.SetLeading(12, 0)
                    '        cell.PaddingBottom = 4.0F
                    '        cell.PaddingTop = 1.0F
                    '        tbl.AddCell(cell)
                    '    Next
                    'Else
                    

                    Dim arr() As String = {"Prepared By:", "Authorised By:", " Received By:"}
                    For i = 0 To arr.Length - 1
                        phrase = New Phrase()

                        phrase.Add(New Chunk(arr(i), cheaderfontBold))

                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 27.0F
                        If i = 0 Then
                            cell.Border = Rectangle.LEFT_BORDER
                        ElseIf i = 2 Then
                            cell.Border = Rectangle.RIGHT_BORDER
                        End If
                        tbl.AddCell(cell)
                    Next
                    'End If


                    For i = 0 To arr.Length - 1
                        phrase = New Phrase()
                        If i = 0 Then
                            phrase.Add(New Chunk(userName, cheaderfont))
                        Else
                            phrase.Add(New Chunk(" ", cheaderfont))
                        End If
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        cell.SetLeading(12, 0)
                        cell.PaddingBottom = 8.0F
                        cell.PaddingTop = 1.0F
                        If i = 0 Then
                            cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                        ElseIf i = 2 Then
                            cell.Border = Rectangle.BOTTOM_BORDER Or Rectangle.RIGHT_BORDER
                        Else
                            cell.Border = Rectangle.BOTTOM_BORDER
                        End If
                        cell.BorderColor = BaseColor.BLACK
                        tbl.AddCell(cell)
                    Next
                    document.Add(tbl)

                    ' Sub Table-Adjust Bill Detail
                    Dim tospacing, bottomspacing As Decimal
                    Dim cellheight As Integer = 5
                    Dim Billdetail As New DataTable
                    Dim conn As New SqlConnection
                    conn = clsDBConnect.dbConnectionnew("strDBConnection")
                    'sqlcmd = New SqlCommand("sp_rpt_payment_billDetails", conn)
                    sqlcmd = New SqlCommand("sp_rpt_payment_billDetails", conn)
                    sqlcmd.CommandType = CommandType.StoredProcedure
                    sqlcmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = tranid
                    sqlcmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
                    sqlcmd.Parameters.Add(New SqlParameter("@trantype", SqlDbType.VarChar, 20)).Value = trantype
                    Using ds As New SqlDataAdapter
                        ds.SelectCommand = sqlcmd
                        ds.Fill(Billdetail)
                    End Using
                    clsDBConnect.dbCommandClose(sqlcmd)
                    clsDBConnect.dbConnectionClose(conn)
                    If Billdetail.Rows.Count > 0 Then
                        Dim arrData2() As String
                        count = 0
                        Dim tblmainBill As PdfPTable = New PdfPTable(1)
                        tblmainBill.SetWidths(New Single() {1.0F})
                        tblmainBill.TotalWidth = documentWidth
                        tblmainBill.LockedWidth = True
                        '  tblmainBill.DefaultCell.Border = Rectangle.NO_BORDER
                        'tblmainBill.DefaultCell.PaddingBottom = 2.0F
                        Dim tblBill As PdfPTable = New PdfPTable(8)
                        tblBill.SetWidths(New Single() {0.11F, 0.08F, 0.13F, 0.2F, 0.13F, 0.13F, 0.11F, 0.11F})
                        tblBill.TotalWidth = documentWidth
                        tblBill.LockedWidth = True
                        Dim openDecno As String
                        If count = 0 Then
                            If trantype = "BPV" Then
                                document.NewPage()
                                phrase = New Phrase()
                                'phrase.Add(New Chunk("Voucher No." & Space(8), cheaderfontBold))
                                'phrase.Add(New Chunk(tranid & Space(40), cheaderfont))
                                'phrase.Add(New Chunk("Date" & Space(10), cheaderfontBold))
                                'phrase.Add(New Chunk(receipt_date & Space(40), cheaderfont))
                                'phrase.Add(New Chunk("Amount(" & currency & ")" & Space(15), cheaderfontBold))
                                'phrase.Add(New Chunk(basedebit & Environment.NewLine & vbLf, cheaderfont))

                                phrase.Add(New Chunk("Voucher No." & Space(5), cheaderfontBold))
                                phrase.Add(New Chunk(tranid & Space(20), cheaderfont))
                                phrase.Add(New Chunk("Date" & Space(5), cheaderfontBold))
                                phrase.Add(New Chunk(receipt_date & Space(25), cheaderfont))
                                phrase.Add(New Chunk("Amount(" & currency & ")" & Space(5), cheaderfontBold))
                                phrase.Add(New Chunk(basedebit & Environment.NewLine & vbLf, cheaderfont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                cell.PaddingTop = 10.0F
                                cell.PaddingBottom = 6.0F
                                tblmainBill.AddCell(cell)
                                cellheight = 20
                            End If

                            phrase = New Phrase()
                            phrase.Add(New Chunk(""))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.Colspan = 8
                            cell.FixedHeight = cellheight
                            cell.PaddingBottom = bottomspacing
                            tblBill.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk("Payment Against Follwing Invoices-" & Space(15), cheaderfontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                            cell.SetLeading(12, 0)
                            cell.Colspan = 8
                            cell.PaddingBottom = 4.0F
                            cell.BackgroundColor = Titlebg
                            cell.PaddingTop = 1.0F
                            tblBill.AddCell(cell)

                            arrData2 = {"Date", "Inv. Type", "Invoice No", "Guest Name", "Ref. No.", "Due Date", "Amount Adjusted"}

                            For i = 0 To arrData2.Length - 1
                                phrase = New Phrase()
                                phrase.Add(New Chunk(arrData2(i), cheaderfontBold))

                                If i = 6 Then
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                    cell.Colspan = 2
                                ElseIf i <= 5 Then
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                    cell.BorderWidthBottom = 0
                                End If
                                cell.PaddingBottom = 4.0F
                                cell.PaddingTop = 1.0F
                                cell.BackgroundColor = subTitlebg
                                tblBill.AddCell(cell)
                            Next

                            Dim receiptcurrency As String = Billdetail.Rows.Item(0).Item("receipt_currency_id")
                            openDecno = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select nodigit from currmast where currcode= '" & receiptcurrency & "'"), String)
                            arrData2 = {"", "", "", "", "", "", receiptcurrency, currency}

                            For i = 0 To arrData2.Length - 1
                                phrase = New Phrase()
                                phrase.Add(New Chunk(arrData2(i), cheaderfontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                If i <= 5 Then
                                    cell.BorderWidthTop = 0
                                End If
                                cell.PaddingBottom = 4.0F
                                cell.Colspan = 1
                                cell.BackgroundColor = subTitlebg
                                cell.PaddingTop = 1.0F
                                tblBill.AddCell(cell)
                            Next

                        End If


                        Dim totalodebit As Decimal
                        Dim totalbdebit As Decimal
                        'Table Data
                        For Each row In Billdetail.Rows
                            arrData2 = {Format(Convert.ToDateTime(row("tran_date").ToString()), "dd-MM-yyyy"), row("tran_type"), row("tran_id"), row("open_field2"), row("open_field3"), Format(Convert.ToDateTime(row("open_due_date").ToString), "dd-MM-yyyy"), Decimal.Parse(row("open_debit")).ToString("N" + openDecno), Decimal.Parse(row("base_debit")).ToString(decimalPoint)}

                            For i = 0 To arrData2.Length - 1
                                phrase = New Phrase()
                                phrase.Add(New Chunk(arrData2(i), normalfont))

                                If i > 5 Then
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT

                                Else
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                End If
                                cell.PaddingBottom = 4.0F
                                cell.PaddingTop = 1.0F
                                tblBill.AddCell(cell)
                            Next
                            totalodebit = totalodebit + arrData2(6)
                            totalbdebit = totalbdebit + arrData2(7)
                        Next

                        arrData2 = {"", "", "", "", "", "Total", totalodebit.ToString("N" + openDecno), totalbdebit.ToString(decimalPoint)}

                        For i = 0 To arrData2.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrData2(i), normalfontbold))

                            If i > 5 Then
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT

                            Else
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                            End If
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            tblBill.AddCell(cell)
                        Next
                        If trantype = "BPV" Then

                            If lsCtry.Trim.ToUpper = "OM" Then
                                Dim arr1OM() As String = {"Prepared By:", " Director of Finance:", " General Manager:", " Received By:"}
                                For i = 0 To arr1OM.Length - 1
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(arr1OM(i), cheaderfontBold))

                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                    cell.SetLeading(12, 0)
                                    cell.PaddingBottom = 10.0F
                                    cell.PaddingTop = 30.0F
                                    cell.Colspan = 2
                                    tblBill.AddCell(cell)
                                Next
                            Else
                                Dim arr1() As String = {"Prepared By:", " Checked By:", " Approved By:", " Received By:"}
                                For i = 0 To arr1.Length - 1
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(arr1(i), cheaderfontBold))

                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                    cell.SetLeading(12, 0)
                                    cell.PaddingBottom = 10.0F
                                    cell.PaddingTop = 30.0F
                                    cell.Colspan = 2
                                    tblBill.AddCell(cell)
                                Next

                            End If



                        End If
                        tblmainBill.AddCell(tblBill)
                        tblmainBill.SpacingBefore = 10
                        document.Add(tblmainBill)
                    End If
                    document.AddTitle(Reportname)
                    document.Close()
                End If

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

        Catch ex As Exception
            Throw ex

        End Try
    End Sub
#End Region
#Region "Private Shared Function ImageCell(path As String, scale As Single, align As Integer) As PdfPCell"
    Private Shared Function ImageCell(ByVal path As String, ByVal scale As Single, ByVal align As Integer) As PdfPCell
        Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path))
        image.ScalePercent(scale)
        Dim cell As New PdfPCell(image)
        cell.BorderColor = BaseColor.WHITE
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 0.0F
        cell.PaddingTop = 0.0F
        Return cell
    End Function
#End Region

#Region "Convert Numeric To Word"

    Public Function AmountInWords(ByVal nAmount As String, Optional ByVal wAmount _
                 As String = vbNullString, Optional ByVal nSet As Object = Nothing) As String
        'Let's make sure entered value is numeric
        If Not IsNumeric(nAmount) Then Return "Please enter numeric values only."

        Dim tempDecValue As String = String.Empty : If InStr(nAmount, ".") Then _
            tempDecValue = nAmount.Substring(nAmount.IndexOf("."))
        nAmount = Replace(nAmount, tempDecValue, String.Empty)

        Try
            Dim intAmount As Long = nAmount
            If intAmount > 0 Then
                nSet = IIf((intAmount.ToString.Trim.Length / 3) _
                 > (CLng(intAmount.ToString.Trim.Length / 3)), _
                  CLng(intAmount.ToString.Trim.Length / 3) + 1, _
                   CLng(intAmount.ToString.Trim.Length / 3))
                Dim eAmount As Long = Microsoft.VisualBasic.Left(intAmount.ToString.Trim, _
                  (intAmount.ToString.Trim.Length - ((nSet - 1) * 3)))
                Dim multiplier As Long = 10 ^ (((nSet - 1) * 3))

                Dim Ones() As String = _
                {"", "One", "Two", "Three", _
                  "Four", "Five", _
                  "Six", "Seven", "Eight", "Nine"}
                Dim Teens() As String = {"", _
                "Eleven", "Twelve", "Thirteen", _
                  "Fourteen", "Fifteen", _
                  "Sixteen", "Seventeen", "Eighteen", "Nineteen"}
                Dim Tens() As String = {"", "Ten", _
                "Twenty", "Thirty", _
                  "Forty", "Fifty", "Sixty", _
                  "Seventy", "Eighty", "Ninety"}
                Dim HMBT() As String = {"", "", _
                "Thousand", "Million", _
                  "Billion", "Trillion", _
                  "Quadrillion", "Quintillion"}

                intAmount = eAmount

                Dim nHundred As Integer = intAmount \ 100 : intAmount = intAmount Mod 100
                Dim nTen As Integer = intAmount \ 10 : intAmount = intAmount Mod 10
                Dim nOne As Integer = intAmount \ 1

                If nHundred > 0 Then wAmount = wAmount & _
                Ones(nHundred) & " Hundred " 'This is for hundreds                
                If nTen > 0 Then 'This is for tens and teens
                    If nTen = 1 And nOne > 0 Then 'This is for teens 
                        wAmount = wAmount & Teens(nOne) & " "
                    Else 'This is for tens, 10 to 90
                        wAmount = wAmount & Tens(nTen) & IIf(nOne > 0, "-", " ")
                        If nOne > 0 Then wAmount = wAmount & Ones(nOne) & " "
                    End If
                Else 'This is for ones, 1 to 9
                    If nOne > 0 Then wAmount = wAmount & Ones(nOne) & " "
                End If
                wAmount = wAmount & HMBT(nSet) & " "
                wAmount = AmountInWords(CStr(CLng(nAmount) - _
                  (eAmount * multiplier)).Trim & tempDecValue, wAmount, nSet - 1)
            Else
                If Val(nAmount) = 0 Then nAmount = nAmount & _
                tempDecValue : tempDecValue = String.Empty
                If (Math.Round(Val(nAmount), 2) * 100) > 0 Then wAmount = _
                  Trim(AmountInWords(CStr(Math.Round(Val(nAmount), 2) * 100), _
                  wAmount.Trim & "", 1)) & " "
            End If
        Catch ex As Exception

        End Try

        'Trap null values
        If IsNothing(wAmount) = True Then wAmount = String.Empty Else wAmount = _
          IIf(InStr(wAmount.Trim.ToLower, ""), _
          wAmount.Trim, wAmount.Trim & "")

        'Display the result
        Return wAmount
    End Function
#End Region
End Class
