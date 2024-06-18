Imports Microsoft.VisualBasic
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient

Public Class ClsInvoiceFreeFormPdf
    Inherits System.Web.UI.Page
    Dim objutils As New clsUtils


#Region "global declaration"
    Dim objclsUtilities As New clsUtils
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
    Public Sub GenerateReport(ByVal trantype As String, ByVal tranid As String, ByVal divcode As String, ByVal PrntSec As String, ByVal TRNNo As String, ByRef bytes() As Byte, ByVal printMode As String)

        Try
            Dim document As New Document(PageSize.A4, 20.0F, 20.0F, 20.0F, 20.0F)
            '  document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)
            Dim documentWidth As Single = 550.0F
            ' tranid = "INM-000269"
            line = New Paragraph(New Chunk(New iTextSharp.text.pdf.draw.LineSeparator(1.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)))
            Dim decno As String
            'Dim decno = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), String)
            'decimalPoint = "N" + decno

            Dim Invoice As New DataTable
            Dim conn1 As New SqlConnection
            conn1 = clsDBConnect.dbConnectionnew("strDBConnection")

            If trantype = "IN" Then
                InvoiceType = "TAX INVOICE"
            ElseIf trantype = "PI" Then
                InvoiceType = "PURCHASE INVOICE"
            ElseIf trantype = "PIManual" Then
                InvoiceType = "PURCHASE INVOICE"
            ElseIf trantype = "PE" Then
                InvoiceType = "PURCHASE VOUCHER"
            ElseIf trantype = "DN" Then
                InvoiceType = "TAX DEBIT NOTE" 'Tanvir  06/07/2022 "DEBIT NOTE"
            ElseIf trantype = "CN" Then
                InvoiceType = "TAX CREDIT NOTE" 'Tanvir  06/07/2022
            End If

            ' christo -03/01/19 -- add account name & change the join
            Dim sqlcmd As New SqlCommand("sp_rpt_freeforminvoice_print", conn1)
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = tranid
            sqlcmd.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = trantype
            sqlcmd.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 10)).Value = divcode
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

                Dim lsCtry As String = objutils.GetString(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=459")

                Dim conm As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from Columbusmaster where div_code='" + divcode + "'"), String)
                Dim coadd1 As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coadd1 from Columbusmaster"), String)
                Dim coadd2 As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coadd2 from Columbusmaster"), String)
                Dim copobox As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select copobox from Columbusmaster"), String)
                Dim cotel As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select cotel from Columbusmaster"), String)
                Dim cofax As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select cofax from Columbusmaster"), String)
                Dim coemail As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coemail from Columbusmaster"), String)
                Dim coweb As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coweb from Columbusmaster"), String)
                Dim TRN As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select TRNNo from Columbusmaster where div_code='" + divcode + "'"), String)

                Dim acctype As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select acc_type  from freeforminvoice_master where tran_id='" + tranid + "' and tran_type='" + trantype + "' and div_code='" + divcode + "'"), String)
                If trantype = "PIManual" Then
                    acctype = "S"
                End If

                Dim tableheader As PdfPTable = Nothing
                Dim tblService As PdfPTable = New PdfPTable(6)
                'Header Table
                tableheader = New PdfPTable(2)
                tableheader.TotalWidth = documentWidth
                tableheader.LockedWidth = True
                tableheader.SetWidths(New Single() {0.65F, 0.35F})

                tableheader.Complete = False
                tableheader.SplitRows = False
                tableheader.WidthPercentage = 100
                If divcode = "01" Then
                    cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
                Else
                    cell = ImageCell("~/Images/Logo1.png", 80.0F, PdfPCell.ALIGN_LEFT)
                End If
                tableheader.AddCell(cell)

                phrase = New Phrase()
                phrase.Add(New Chunk(conm & Environment.NewLine, normalfont))
                phrase.Add(New Chunk(coadd1 & Environment.NewLine, normalfont))
                phrase.Add(New Chunk(coadd2 & Environment.NewLine, normalfont))
                phrase.Add(New Chunk("PO Box :" & Space(6) & copobox & Environment.NewLine & vbLf, normalfont))
                'phrase.Add(New Chunk("Email :" & Space(6) & coemail & Environment.NewLine, normalfont))
                phrase.Add(New Chunk("Tel     :" & Space(6) & cotel & Environment.NewLine, normalfont))
                'phrase.Add(New Chunk("Fax    :" & Space(6) & cofax & Environment.NewLine, normalfont))
                phrase.Add(New Chunk("Web   :" & Space(6) & coweb & Environment.NewLine, normalfont))
                'phrase.Add(New Chunk("TRN   :" & Space(6) & TRNNo & Environment.NewLine, normalfont))
                ' Christo. 02/01/19
                If lsCtry.Trim.ToUpper = "OM" Then
                    ' TRN Not Required for Oman
                Else
                    phrase.Add(New Chunk("TRN   :" & Space(6) & TRN & Environment.NewLine, normalfont))
                End If

                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 5.0F
                cell.SetLeading(12, 0)
                tableheader.AddCell(cell)

                Dim FooterTable = New PdfPTable(1)
                FooterTable.TotalWidth = documentWidth
                FooterTable.LockedWidth = True
                FooterTable.SetWidths(New Single() {1.0F})
                FooterTable.Complete = False
                FooterTable.SplitRows = False
                cell.AddElement(line)
                FooterTable.AddCell(cell)
                phrase = New Phrase()
                phrase.Add(New Chunk("Printed Date:" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss"), normalfont))
                cell = New PdfPCell(phrase)
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.SetLeading(12, 0)
                cell.PaddingBottom = 2
                FooterTable.AddCell(cell)


                FooterTable.Complete = True


                'Add common header and footer part to every page

                writer.PageEvent = New ClsHeaderFooter(tableheader, Nothing, Nothing, Nothing, "Voucher", FooterTable)
                document.Open()
                'If trantype = "IN" Then
                '    InvoiceType = "TAX INVOICE"
                'End If
                If Invoice.Rows.Count > 0 Then
                    For Each row In Invoice.Rows
                        agentcode = row("supcode").ToString()
                        Dim arrservice() As String
                        If count = 0 Then

                            If trantype = "IN" Then
                                If row("InvoiceType") = "Commercial Invoice" Then
                                    InvoiceType = "COMMERCIAL INVOICE"
                                Else
                                    InvoiceType = "TAX INVOICE"
                                End If
                            End If

                            Dim tblInvoice As PdfPTable = New PdfPTable(1)
                            tblInvoice.SetWidths(New Single() {1.0F})
                            tblInvoice.TotalWidth = 220.0F
                            tblInvoice.LockedWidth = True
                            phrase = New Phrase()
                            phrase.Add(New Chunk(InvoiceType, invoicefont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                            cell.PaddingTop = 3.0F
                            cell.PaddingBottom = 5.0F
                            ' cell.PaddingTop = 5.0F
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

                            Dim tblCompany As PdfPTable = New PdfPTable(2)
                            tblCompany.SetWidths(New Single() {0.12F, 0.88F})
                            tblCompany.TotalWidth = documentWidth
                            tblCompany.LockedWidth = True

                            phrase = New Phrase()
                            phrase.Add(New Chunk("Ms/Company:", normalfontbold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 5.0F
                            cell.PaddingLeft = 3.0F
                            cell.PaddingTop = 5.0F
                            cell.FixedHeight = 30.0F
                            cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                            tblCompany.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(row(3).ToString(), cheaderfont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 5.0F
                            cell.PaddingLeft = 3.0F
                            cell.PaddingTop = 5.0F
                            cell.FixedHeight = 30.0F
                            cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                            tblCompany.AddCell(cell)

                            If (trantype = "IN") Then
                                phrase = New Phrase()
                                phrase.Add(New Chunk("Guest Name:", normalfontbold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                cell.PaddingBottom = 5.0F
                                cell.PaddingLeft = 3.0F
                                cell.PaddingTop = 5.0F
                                cell.FixedHeight = 30.0F
                                cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                                tblCompany.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk(row("GuestName").ToString(), cheaderfont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                cell.PaddingBottom = 5.0F
                                cell.PaddingLeft = 3.0F
                                cell.PaddingTop = 5.0F
                                cell.FixedHeight = 30.0F
                                cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                                tblCompany.AddCell(cell)
                            End If

                            Dim address As String = ""
                            Dim contact As String = ""
                            Dim tel As String = ""
                            Dim fax As String = ""
                            Dim strqry As String
                            If row("acc_type") = "C" Then
                                strqry = "select add1,add2,add3,tel1 as tel,contact1 as contact, fax from agentmast(nolock) where agentcode='" + row("supcode") + "' and divcode='" + row("div_code") + "'"
                            Else
                                strqry = "select add1,add2,add3,tel1 as tel,contact1 as contact, fax from partymast(nolock) where partycode='" + row("supcode") + "'"
                            End If
                            Dim dt As DataTable = objutils.GetDataFromDataTable(strqry)
                            If dt.Rows.Count > 0 Then
                                Dim dr As DataRow = dt.Rows(0)
                                If Not IsDBNull(dr("add1")) Then
                                    address = Convert.ToString(dr("add1")).Trim()
                                End If
                                If Not IsDBNull(dr("add2")) Then
                                    If address = "" Then
                                        address = Convert.ToString(dr("add2")).Trim()
                                    Else
                                        address = address + IIf(Convert.ToString(dr("add2")).Trim() = "", "", ", " + Convert.ToString(dr("add2")).Trim())
                                    End If
                                End If
                                If Not IsDBNull(dr("add3")) Then
                                    If address = "" Then
                                        address = Convert.ToString(dr("add3")).Trim()
                                    Else
                                        address = address + IIf(Convert.ToString(dr("add3")).Trim() = "", "", ", " + Convert.ToString(dr("add3")).Trim())
                                    End If
                                End If

                                contact = IIf((TypeOf dr("contact") Is DBNull), "", dr("contact"))
                                tel = IIf((TypeOf dr("tel") Is DBNull), "", dr("tel"))
                                fax = IIf((TypeOf dr("fax") Is DBNull), "", dr("fax"))
                            Else
                                contact = IIf((TypeOf row("contact") Is DBNull), "", row("contact"))
                                tel = IIf((TypeOf row("tel") Is DBNull), "", row("tel"))
                                fax = IIf((TypeOf row("fax") Is DBNull), "", row("fax"))
                            End If

                            Dim addr As String
                            'If Not (String.IsNullOrEmpty(contact) Or String.IsNullOrEmpty(tel) Or String.IsNullOrEmpty(fax)) Then                            
                            If Not (String.IsNullOrEmpty(contact) Or String.IsNullOrEmpty(tel)) Then
                                addr = "Contact:" & Space(2) & contact & Space(2) & tel & Space(2) & fax
                            Else
                                addr = ""
                            End If
                            If address <> "" Then
                                addr = address + vbCr + addr
                            End If

                            phrase = New Phrase()
                            phrase.Add(New Chunk("Address:", normalfontbold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                            cell.PaddingBottom = 5.0F
                            cell.PaddingLeft = 3.0F
                            cell.PaddingTop = 5.0F
                            cell.MinimumHeight = 30.0F
                            cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                            tblCompany.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(addr, cheaderfont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                            cell.PaddingBottom = 5.0F
                            cell.PaddingLeft = 3.0F
                            cell.PaddingTop = 5.0F
                            cell.MinimumHeight = 30.0F
                            cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                            tblCompany.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk("TRN No.:", normalfontbold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 5.0F
                            cell.PaddingLeft = 3.0F
                            cell.PaddingTop = 5.0F
                            cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                            tblCompany.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(TRNNo, cheaderfont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 5.0F
                            cell.PaddingLeft = 3.0F
                            cell.PaddingTop = 5.0F
                            cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                            tblCompany.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk("Ref No.:" & Space(5), normalfontbold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 5.0F
                            cell.PaddingLeft = 3.0F
                            cell.PaddingTop = 5.0F
                            cell.PaddingLeft = 3.0F
                            cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                            tblCompany.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(row("referenceno").ToString(), cheaderfont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 5.0F
                            cell.PaddingLeft = 3.0F
                            cell.PaddingTop = 5.0F
                            cell.PaddingLeft = 3.0F
                            cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                            tblCompany.AddCell(cell)

                            tblCompany.SpacingBefore = 90.0F
                            document.Add(tblCompany)


                            tblService.SetWidths(New Single() {0.45F, 0.12F, 0.07F, 0.12F, 0.12F, 0.12F})
                            tblService.TotalWidth = documentWidth
                            tblService.LockedWidth = True

                            arrservice = {"SERVICE/DESCRIPTION", "Non Taxable Amount(" & row("currcode") & ")", "VAT%", "Taxable Amount(" & row("currcode") & ")", "VAT Amount(" & row("currcode") & ")", "Total(" & row("currcode") & ")"}

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
                                '  cell.FixedHeight = 10
                                cell.PaddingBottom = 3.0F
                                tblService.AddCell(cell)
                                count = count + 1
                            Next
                        End If
                        decno = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select nodigit from currmast where currcode= '" & row("currcode") & "'"), String)
                        decimalPoint = "N" + decno

                        'arrservice = {row("particulars"), IIf(Decimal.Parse(row("nontaxamt")) >= 0, Decimal.Parse(row("nontaxamt")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(row("nontaxamt"))).ToString(decimalPoint) & ")"), row("vatperc") & "%", IIf(Decimal.Parse(row("taxamt")) >= 0, Decimal.Parse(row("taxamt")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(row("taxamt"))).ToString(decimalPoint) & ")"), IIf(Decimal.Parse(row("vatamt")) >= 0, Decimal.Parse(row("vatamt")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(row("vatamt"))).ToString(decimalPoint) & ")"), IIf(Decimal.Parse(row("amount")) >= 0, Decimal.Parse(row("amount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(row("amount"))).ToString(decimalPoint) & ")")}
                        ' add accountname - christo - 03/01/19
                        '+ vbCrLf + row("acctname")
                        Dim vatpercent As String
                        If trantype = "IN" And row("invoiceType") = "Commercial Invoice" Then
                            If Val(row("vatperc")) = 0 Then
                                vatpercent = ""
                            Else
                                vatpercent = row("vatperc") & "%"
                            End If
                        Else
                            vatpercent = row("vatperc") & "%"
                        End If

                        arrservice = {row("particulars"), IIf(Decimal.Parse(row("nontaxamt")) >= 0, Decimal.Parse(row("nontaxamt")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(row("nontaxamt"))).ToString(decimalPoint) & ")"), vatpercent, IIf(Decimal.Parse(row("taxamt")) >= 0, Decimal.Parse(row("taxamt")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(row("taxamt"))).ToString(decimalPoint) & ")"), IIf(Decimal.Parse(row("vatamt")) >= 0, Decimal.Parse(row("vatamt")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(row("vatamt"))).ToString(decimalPoint) & ")"), IIf(Decimal.Parse(row("amount")) >= 0, Decimal.Parse(row("amount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(row("amount"))).ToString(decimalPoint) & ")")}

                        For i = 0 To arrservice.Length - 1
                            phrase = New Phrase()
                            If i = 0 Then
                                phrase.Add(New Chunk(arrservice(i), cheaderfont))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                            ElseIf i = 2 Then
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
                        ' totalvatper = totalvatper + Decimal.Parse(row("vatperc"))
                        totaltaxamt = totaltaxamt + Decimal.Parse(row("taxamt"))
                        totalvatamt = totalvatamt + Decimal.Parse(row("vatamt"))
                        totalamt = totalamt + Decimal.Parse(row("amount"))
                    Next
                    fractionword = ""
                    'modified param 13/11/2018
                    'Dim fraction As Decimal = Math.Round(totalamt, 2) - Math.Truncate(Math.Round(totalamt, 2))
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
                        Dim coin As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select currcoin from currmast where currcode= '" & currency.Trim & "'"), String)
                        fractionword = "AND " & coin & "  " & AmountInWords(fraction.ToString())
                    End If
                    ' phrase.Add(New Chunk(currency & Space(2) & decimalInword.ToUpper() & Space(3) & fractionword.ToUpper() & "ONLY", cheaderfont))


                    Dim arrTotal() As String = {currency & Space(2) & decimalInword.ToUpper() & Space(2) & fractionword.ToUpper() & " ONLY", IIf(totalnonamt >= 0, totalnonamt.ToString(decimalPoint), "(" & Math.Abs(totalnonamt).ToString(decimalPoint) & ")"), "", IIf(totaltaxamt >= 0, totaltaxamt.ToString(decimalPoint), "(" & Math.Abs(totaltaxamt).ToString(decimalPoint) & ")"), IIf(totalvatamt >= 0, totalvatamt.ToString(decimalPoint), "(" & Math.Abs(totalvatamt).ToString(decimalPoint) & ")"), IIf(totalamt >= 0, totalamt.ToString(decimalPoint), "(" & Math.Abs(totalamt).ToString(decimalPoint) & ")")}

                    For i = 0 To arrTotal.Length - 1
                        phrase = New Phrase()
                        If i = 0 Then
                            phrase.Add(New Chunk(arrTotal(i), normalfontbold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
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
                    document.Add(tblService)

                    'BANK detail Table

                    Dim sqlConn As New SqlConnection
                    Dim mySqlCmd As New SqlCommand
                    Dim myDataAdapter As New SqlDataAdapter
                    Dim ds As New DataSet
                    sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
                    mySqlCmd = New SqlCommand("sp_agentmast_bank", sqlConn)
                    mySqlCmd.CommandType = CommandType.StoredProcedure


                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = agentcode
                    mySqlCmd.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = acctype

                    myDataAdapter.SelectCommand = mySqlCmd
                    myDataAdapter.Fill(ds)
                    Dim BankDetail As New DataTable

                    BankDetail = ds.Tables(0)

                    'If BankDetail.Rows.Count > 0 Then
                    ' Don't show for Purchase related entries - Christo. A - 03/01/19
                    If (BankDetail.Rows.Count > 0 And (acctype = "C" Or (trantype = "IN" And acctype = "S"))) Then
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
                            phrase.Add(New Chunk(Space(40) & "***all the bank transfer charges have to be borne by customer" & Environment.NewLine & vbLf, bankfontbold))
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

                    Dim tbl = New PdfPTable(3)
                    tbl.TotalWidth = documentWidth
                    tbl.LockedWidth = True
                    tbl.SetWidths(New Single() {0.33F, 0.33F, 0.34F})
                    If lsCtry.Trim.ToUpper = "OM" Then


                        'Sharfudeen 20/12/2023
                        ' Dim arrOM() As String = {"Prepared By", " Checked By", " Approved By"}   ', " Received By"
                        Dim arrOM() As String = {"Prepared By", " Approved By", " Company Stamp"}   ', " Received By"

                        '  Dim arrOM() As String = {"Prepared By", " Checked By", " Approved By"}   ', " Received By"


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
                        'Sharfudeen 20/12/2023
                        ' Dim arr() As String = {"Prepared By", " Checked By", " Approved By"}  ', " Received By"
                        Dim arr() As String = {"Prepared By", " Approved By", " Company Stamp"}  ', " Received By"

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

                    'Sharfudeen 20/12/2023 start
                    cell = New PdfPCell()
                    cell.Colspan = 2
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.NO_BORDER
                    cell.PaddingTop = 5.0F
                    tbl.AddCell(cell)
                    cell = ImageCell("~/images/CompanyStamp.png", 70.0F, PdfPCell.ALIGN_LEFT)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingTop = 5.0F
                    tbl.AddCell(cell)
                    tbl.Complete = True
                    'Sharfudeen 20/12/2023 end


                    tbl.SpacingBefore = 20.0F
                    document.Add(tbl)
                    'Horizontal line
                    document.Add(line)
                End If
                document.AddTitle(InvoiceType)
                document.Close()


                If printMode = "download" Then
                    Dim pagingFont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)
                    Dim reader As New PdfReader(memoryStream.ToArray())
                    Using mStream As New MemoryStream()
                        Using stamper As New PdfStamper(reader, mStream)
                            Dim pages As Integer = reader.NumberOfPages
                            For i As Integer = 1 To pages

                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 560.0F, 10.0F, 0)
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
