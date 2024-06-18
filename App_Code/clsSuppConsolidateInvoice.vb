Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO
Imports System.Linq
Imports System.Globalization

Public Class clsSuppConsolidateInvoice

#Region "global declaration"
    Dim objutils As New clsUtils
    Dim count, rowcount As Integer
    Dim Reportname, strSql, InvoiceType, decimalPoint As String
    Dim normalfontbold As Font = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLACK)
    Dim normalfont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)
    Dim bankfontbold As Font = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)
    Dim bankfont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
    Dim underline As Font = FontFactory.GetFont("Arial", 9, Font.UNDERLINE, BaseColor.BLACK)
    Dim invoicefont As Font = FontFactory.GetFont("Arial", 15, Font.BOLD, BaseColor.BLACK)
    Dim line As Paragraph
    Dim cheaderfontBold As Font = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)
    Dim cheaderfont As Font = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK)


    Dim totaldebit, totalcredit, amount As Decimal
    Dim userName, currency, docno, fractionword, decimalword, decimalInword, agentcode As String
    Dim totalnonamt, totalvatper, totaltaxamt, totalvatamt, totalamt As Decimal

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
    Public Sub GenerateReport(ByVal acctType As String, ByVal acctCode As String, ByVal trantype As String, ByVal divcode As String, ByVal fromdate As String, ByVal todate As String, ByRef bytes() As Byte, ByVal printMode As String)

        Try
            Dim document As New Document(PageSize.A4, 20.0F, 20.0F, 20.0F, 25.0F)
            Dim documentWidth As Single = 550.0F
            line = New Paragraph(New Chunk(New iTextSharp.text.pdf.draw.LineSeparator(1.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)))
            InvoiceType = "TAX INVOICE"
            Dim decno As String
            Dim Invoice As New DataTable
            Dim conn1 As New SqlConnection
            conn1 = clsDBConnect.dbConnectionnew("strDBConnection")
            Dim sqlcmd As New SqlCommand("sp_rpt_freeforminvoice_print_consolidated", conn1)
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = acctType
            sqlcmd.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = acctCode
            sqlcmd.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = trantype
            sqlcmd.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 10)).Value = divcode
            sqlcmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = fromdate
            sqlcmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = todate
            Using ds As New SqlDataAdapter
                ds.SelectCommand = sqlcmd
                ds.Fill(Invoice)
            End Using
            clsDBConnect.dbCommandClose(sqlcmd)
            clsDBConnect.dbConnectionClose(conn1)

            Using memoryStream As New System.IO.MemoryStream()
                Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)

                Dim phrase As Phrase = Nothing
                Dim cell As PdfPCell = Nothing

                Dim lsCtry As String = objutils.GetString("strDBConnection", "select option_selected from reservation_parameters where param_id=459")

                Dim coadd1 As String = CType(objutils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select coadd1 from Columbusmaster"), String)
                Dim coadd2 As String = CType(objutils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select coadd2 from Columbusmaster"), String)
                Dim copobox As String = CType(objutils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select copobox from Columbusmaster"), String)
                Dim cotel As String = CType(objutils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select cotel from Columbusmaster"), String)
                Dim cofax As String = CType(objutils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select cofax from Columbusmaster"), String)
                Dim coemail As String = CType(objutils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select coemail from Columbusmaster"), String)
                Dim coweb As String = CType(objutils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select coweb from Columbusmaster"), String)
                Dim TRN As String = CType(objutils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select TRNNo from Columbusmaster where div_code='" + divcode + "'"), String)

                'Dim acctype As String = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select acc_type  from freeforminvoice_master where tran_id='" + tranid + "' and tran_type='" + trantype + "' and div_code='" + divcode + "'"), String)

                Dim tableheader As PdfPTable = Nothing
                'Header Table
                tableheader = New PdfPTable(2)
                tableheader.TotalWidth = documentWidth
                tableheader.LockedWidth = True
                tableheader.SetWidths(New Single() {0.7F, 0.3F})

                tableheader.Complete = False
                tableheader.SplitRows = False
                tableheader.WidthPercentage = 100
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
                phrase.Add(New Chunk("TRN   :" & Space(6) & TRN & Environment.NewLine, normalfont))

                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 5.0F
                cell.SetLeading(12, 0)
                tableheader.AddCell(cell)
                tableheader.Complete = True

                'Add common header and footer part to every page

                writer.PageEvent = New ClsHeaderFooter(tableheader, Nothing, Nothing, Nothing, "ConsolidateInvoice", Nothing)
                document.Open()

                If Invoice.Rows.Count > 0 Then

                    Dim suppcodes = (From n In Invoice.AsEnumerable Group By supcode = n.Field(Of String)("supcode"), des = n.Field(Of String)("des") Into grp = Group Select New With {.supcode = supcode, .des = des}).ToList()
                    If suppcodes.Count > 0 Then
                        Dim orderSup = (From n In suppcodes Order By n.des Select n)
                        For k = 0 To orderSup.Count - 1
                            Dim partycode = orderSup(k).supcode
                            Dim filterDt As DataTable
                            filterDt = (From n In Invoice.AsEnumerable() Where n.Field(Of String)("supcode") = partycode Select n Order By n.Field(Of Date)("invoice_date") Ascending).CopyToDataTable()
                            agentcode = partycode
                            count = 0
                            totalnonamt = 0
                            totaltaxamt = 0
                            totalvatamt = 0
                            totalamt = 0
                            If k <> 0 Then
                                document.NewPage()
                                document.PageCount = 1
                            End If
                            Dim tblService As PdfPTable = New PdfPTable(7)
                            decno = CType(objutils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select nodigit from currmast where currcode= '" & filterDt.Rows(0)("currcode") & "'"), String)
                            decimalPoint = "N" + decno
                            For Each row In filterDt.Rows
                                Dim arrservice() As String
                                If count = 0 Then
                                    Dim tblInvoice As PdfPTable = New PdfPTable(1)
                                    tblInvoice.SetWidths(New Single() {1.0F})
                                    tblInvoice.TotalWidth = 220.0F
                                    tblInvoice.LockedWidth = True
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(InvoiceType, invoicefont))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                                    cell.SetLeading(12, 0)
                                    cell.PaddingTop = 3.0F
                                    cell.PaddingBottom = 5.0F
                                    tblInvoice.AddCell(cell)

                                    tblInvoice.WriteSelectedRows(0, -1, 180, 685, writer.DirectContent)

                                    Dim tblInvoice2 As PdfPTable = New PdfPTable(2)
                                    tblInvoice2.SetWidths(New Single() {0.4F, 0.6F})
                                    tblInvoice2.TotalWidth = 140.0F
                                    tblInvoice2.LockedWidth = True

                                    phrase = New Phrase()
                                    phrase.Add(New Chunk("Invoice No", normalfont))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                                    cell.SetLeading(12, 0)
                                    cell.PaddingTop = 3.0F
                                    cell.PaddingLeft = 3.0F
                                    cell.PaddingBottom = 3.0F
                                    tblInvoice2.AddCell(cell)

                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(row("tran_id").ToString(), normalfontbold))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                                    cell.SetLeading(12, 0)
                                    cell.PaddingTop = 3.0F
                                    cell.PaddingLeft = 3.0F
                                    cell.PaddingBottom = 3.0F
                                    tblInvoice2.AddCell(cell)

                                    phrase = New Phrase()
                                    phrase.Add(New Chunk("Date", normalfont))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                                    cell.SetLeading(12, 0)
                                    cell.PaddingTop = 3.0F
                                    cell.PaddingLeft = 3.0F
                                    cell.PaddingBottom = 3.0F
                                    tblInvoice2.AddCell(cell)

                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(Format(Convert.ToDateTime(row("invoice_date").ToString()), "dd-MM-yyyy"), normalfontbold))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                                    cell.SetLeading(12, 0)
                                    cell.PaddingTop = 3.0F
                                    cell.PaddingLeft = 3.0F
                                    cell.PaddingBottom = 3.0F
                                    tblInvoice2.AddCell(cell)

                                    tblInvoice2.WriteSelectedRows(0, -1, 425, 685, writer.DirectContent)

                                    Dim tblCompany As PdfPTable = New PdfPTable(1)
                                    tblCompany.SetWidths(New Single() {1.0F})
                                    tblCompany.TotalWidth = documentWidth
                                    tblCompany.LockedWidth = True

                                    phrase = New Phrase()
                                    phrase.Add(New Chunk("Ms/Company:" & Space(5), normalfontbold))
                                    phrase.Add(New Chunk(row(3).ToString(), cheaderfont))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 1, True)
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE

                                    cell.SetLeading(12, 0)
                                    cell.PaddingBottom = 5.0F
                                    cell.PaddingLeft = 3.0F
                                    cell.PaddingTop = 5.0F
                                    cell.FixedHeight = 30.0F
                                    tblCompany.AddCell(cell)

                                    Dim contact As String = IIf((TypeOf row("contact") Is DBNull), "", row("contact"))
                                    Dim tel As String = IIf((TypeOf row("tel") Is DBNull), "", row("tel"))
                                    Dim fax As String = IIf((TypeOf row("fax") Is DBNull), "", row("fax"))
                                    Dim addr As String
                                    If Not (String.IsNullOrEmpty(contact) Or String.IsNullOrEmpty(tel)) Then
                                        addr = "Contact:" & Space(2) & contact & Space(2) & tel & Space(2) & fax
                                    Else
                                        addr = ""
                                    End If

                                    phrase = New Phrase()
                                    phrase.Add(New Chunk("Address:" & Space(5), normalfontbold))
                                    phrase.Add(New Chunk(addr, cheaderfont))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 1, True)
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE
                                    cell.PaddingBottom = 5.0F
                                    cell.PaddingLeft = 3.0F
                                    cell.PaddingTop = 5.0F
                                    cell.FixedHeight = 30.0F
                                    cell.SetLeading(12, 0)
                                    tblCompany.AddCell(cell)

                                    phrase = New Phrase()
                                    phrase.Add(New Chunk("TRN No.:" & Space(5), normalfontbold))
                                    phrase.Add(New Chunk(Convert.ToString(row("sup_TRNNo")), cheaderfont))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 1, True)
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE
                                    cell.PaddingBottom = 5.0F
                                    cell.PaddingLeft = 3.0F
                                    cell.PaddingTop = 5.0F
                                    cell.SetLeading(12, 0)
                                    tblCompany.AddCell(cell)

                                    tblCompany.SpacingBefore = 90.0F
                                    document.Add(tblCompany)

                                    tblService.SetWidths(New Single() {0.11F, 0.34F, 0.12F, 0.07F, 0.12F, 0.12F, 0.12F})
                                    tblService.TotalWidth = documentWidth
                                    tblService.Complete = False
                                    tblService.LockedWidth = True

                                    arrservice = {"Date", "Service / Description", "Non Taxable Amount(" & row("currcode") & ")", "VAT%", "Taxable Amount(" & row("currcode") & ")", "VAT Amount(" & row("currcode") & ")", "Total(" & row("currcode") & ")"}

                                    For i = 0 To arrservice.Length - 1
                                        phrase = New Phrase()
                                        phrase.Add(New Chunk(arrservice(i), normalfontbold))
                                        If i = 0 Or i = 2 Then
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                                        Else
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                                        End If
                                        cell.SetLeading(12, 0)
                                        cell.PaddingLeft = 3.0F
                                        cell.PaddingBottom = 3.0F
                                        tblService.AddCell(cell)

                                    Next
                                End If

                                arrservice = {Convert.ToDateTime(row("invoice_date")).ToString("dd/MM/yyyy"), row("bookingNo") + ", " + row("referenceNo") + ", " + row("particulars") + vbCrLf + row("acctname"), IIf(Decimal.Parse(row("nontaxamt")) >= 0, Decimal.Parse(row("nontaxamt")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(row("nontaxamt"))).ToString(decimalPoint) & ")"), row("vatperc") & "%", IIf(Decimal.Parse(row("taxamt")) >= 0, Decimal.Parse(row("taxamt")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(row("taxamt"))).ToString(decimalPoint) & ")"), IIf(Decimal.Parse(row("vatamt")) >= 0, Decimal.Parse(row("vatamt")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(row("vatamt"))).ToString(decimalPoint) & ")"), IIf(Decimal.Parse(row("amount")) >= 0, Decimal.Parse(row("amount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(row("amount"))).ToString(decimalPoint) & ")")}

                                For i = 0 To arrservice.Length - 1
                                    phrase = New Phrase()
                                    If i = 0 Or i = 1 Then
                                        phrase.Add(New Chunk(arrservice(i), cheaderfont))
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                        cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                                    ElseIf i = 3 Then
                                        phrase.Add(New Chunk(arrservice(i), normalfont))
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                        cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                                    ElseIf arrservice.Length - 1 Then
                                        phrase.Add(New Chunk(arrservice(i), normalfont))
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                        cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT

                                    Else
                                        phrase.Add(New Chunk(arrservice(i), normalfont))
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                        cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                                    End If
                                    cell.BorderWidthBottom = 0
                                    cell.BorderWidthTop = 0
                                    cell.PaddingLeft = 3.0F
                                    cell.SetLeading(12, 0)
                                    cell.PaddingBottom = 3.0F
                                    tblService.AddCell(cell)
                                Next
                                currency = row("currcode")
                                totalnonamt = totalnonamt + Decimal.Parse(row("nontaxamt"))
                                totaltaxamt = totaltaxamt + Decimal.Parse(row("taxamt"))
                                totalvatamt = totalvatamt + Decimal.Parse(row("vatamt"))
                                totalamt = totalamt + Decimal.Parse(row("amount"))
                                count = count + 1
                            Next
                            fractionword = ""
                            Dim fraction As Decimal = Math.Round(totalamt, Convert.ToInt32(decno), MidpointRounding.AwayFromZero) Mod 1
                            If fraction > 0 Then
                                Dim arrFraction As String() = fraction.ToString.Split(".")
                                If arrFraction.Length = 2 Then
                                    fraction = arrFraction(1)
                                Else
                                    fraction = 0
                                End If
                            End If
                            decimalword = Math.Truncate(totalamt).ToString()
                            decimalInword = AmountInWords(decimalword)
                            If fraction > 0 Then
                                Dim coin As String = CType(objutils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select currcoin from currmast where currcode= '" & currency.Trim & "'"), String)
                                fractionword = "AND " & coin & "  " & AmountInWords(fraction.ToString())
                            End If

                            Dim arrTotal() As String = {currency & Space(2) & decimalInword.ToUpper() & Space(2) & fractionword.ToUpper() & " ONLY", IIf(totalnonamt >= 0, totalnonamt.ToString(decimalPoint), "(" & Math.Abs(totalnonamt).ToString(decimalPoint) & ")"), "", IIf(totaltaxamt >= 0, totaltaxamt.ToString(decimalPoint), "(" & Math.Abs(totaltaxamt).ToString(decimalPoint) & ")"), IIf(totalvatamt >= 0, totalvatamt.ToString(decimalPoint), "(" & Math.Abs(totalvatamt).ToString(decimalPoint) & ")"), IIf(totalamt >= 0, totalamt.ToString(decimalPoint), "(" & Math.Abs(totalamt).ToString(decimalPoint) & ")")}

                            For i = 0 To arrTotal.Length - 1
                                phrase = New Phrase()
                                If i = 0 Then
                                    phrase.Add(New Chunk(arrTotal(i), normalfontbold))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                                    cell.Colspan = 2
                                ElseIf i = 2 Then
                                    phrase.Add(New Chunk(arrTotal(i), normalfontbold))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                                Else
                                    phrase.Add(New Chunk(arrTotal(i), normalfontbold))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                                End If

                                cell.SetLeading(12, 0)
                                cell.PaddingLeft = 3.0F

                                cell.PaddingBottom = 3.0F
                                tblService.AddCell(cell)
                            Next
                            tblService.SpacingBefore = 10.0F
                            tblService.SpacingAfter = 5
                            tblService.Complete = True
                            document.Add(tblService)

                            'BANK detail Table

                            Dim sqlConn As New SqlConnection
                            Dim mySqlCmd As New SqlCommand
                            Dim myDataAdapter As New SqlDataAdapter
                            Dim ds As New DataSet
                            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
                            mySqlCmd = New SqlCommand("sp_agentmast_bank", sqlConn)
                            mySqlCmd.CommandType = CommandType.StoredProcedure


                            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 10)).Value = agentcode


                            myDataAdapter.SelectCommand = mySqlCmd
                            myDataAdapter.Fill(ds)
                            Dim BankDetail As New DataTable

                            BankDetail = ds.Tables(0)

                            'If BankDetail.Rows.Count > 0 Then
                            ' Don't show for Purchase related entries - Christo. A - 03/01/19
                            If (BankDetail.Rows.Count > 0) Then      ' And acctype = "C")
                                For Each row In BankDetail.Rows

                                    Dim tblBankDetail As PdfPTable = New PdfPTable(2)
                                    tblBankDetail.SetWidths(New Single() {0.15F, 0.85})
                                    tblBankDetail.TotalWidth = 540.0F
                                    tblBankDetail.LockedWidth = True

                                    Dim tblMain As PdfPTable = New PdfPTable(1)
                                    tblMain.SetWidths(New Single() {1.0F})
                                    tblMain.TotalWidth = 540.0F
                                    tblMain.LockedWidth = True

                                    phrase = New Phrase()
                                    phrase.Add(New Chunk("PLEASE NOTE OUR BANK DETAILS:", underline))
                                    phrase.Add(New Chunk(Space(40) & "***all the bank transfer charges have to be done by agent" & Environment.NewLine & vbLf, bankfontbold))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_TOP
                                    cell.PaddingTop = 2.0F
                                    cell.PaddingBottom = 3.0F
                                    cell.PaddingLeft = 3.0F
                                    cell.SetLeading(12, 0)
                                    cell.Colspan = 2
                                    tblBankDetail.AddCell(cell)
                                    arrTotal = {"Account Name", ":" & Space(2) & row("accountname"), "Account Number", ":" & Space(2) & row("accountnumber"), "Bank Name ", ":" & Space(2) & row("bankname"), "Branch Name", ":" & Space(2) & row("branchname"), "SWIFT Code", ":" & Space(2) & row("swiftcode"), "IBAN Number", ":" & Space(2) & row("IBAnnumber")}
                                    For i = 0 To arrTotal.Length - 1

                                        phrase = New Phrase()
                                        phrase.Add(New Chunk(arrTotal(i), bankfont))
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                        cell.HorizontalAlignment = PdfPCell.ALIGN_TOP
                                        cell.PaddingTop = 2.0F
                                        cell.PaddingBottom = 3.0F
                                        cell.PaddingLeft = 3.0F
                                        cell.SetLeading(12, 0)
                                        tblBankDetail.AddCell(cell)
                                    Next

                                    tblMain.AddCell(tblBankDetail)
                                    document.Add(tblMain)
                                Next
                            End If
                            
                            Dim tbl = New PdfPTable(4)
                            tbl.TotalWidth = documentWidth
                            tbl.LockedWidth = True
                            tbl.SetWidths(New Single() {0.25F, 0.25F, 0.25F, 0.25F})
                            If lsCtry.Trim.ToUpper = "OM" Then
                                Dim arrOM() As String = {"Prepared By", " Director of Finance", " General Manager", " Received By"}

                                For i = 0 To arrOM.Length - 1
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(arrOM(i), cheaderfont))

                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                    cell.SetLeading(12, 0)
                                    cell.PaddingBottom = 2.0F
                                    tbl.AddCell(cell)
                                Next
                            Else
                                Dim arr() As String = {"Prepared By", " Checked By", " Approved By", " Received By"}

                                For i = 0 To arr.Length - 1
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(arr(i), cheaderfont))

                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                    cell.SetLeading(12, 0)
                                    cell.PaddingBottom = 2.0F
                                    tbl.AddCell(cell)
                                Next

                            End If

                            tbl.SpacingBefore = 20.0F
                            document.Add(tbl)
                            'Horizontal line
                            document.Add(line)
                        Next
                    End If
                End If
                document.AddTitle(InvoiceType)
                document.Close()

                bytes = memoryStream.ToArray()

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
