Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Globalization

Public Class ClsJournalVoucherPdf
    Inherits System.Web.UI.Page
    Dim objutils As New clsUtils
#Region "global declaration"
    Dim objclsUtilities As New clsUtils
    Dim tback As BaseColor = New BaseColor(255, 188, 155)
    Dim normalfont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
    Dim footerdfont As Font = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)
    Dim companyTitle As Font = FontFactory.GetFont("Verdana", 12, Font.BOLD, BaseColor.BLACK)
    Dim reportname As Font = FontFactory.GetFont("Verdana", 14, Font.BOLD, BaseColor.BLACK)
    Dim normalfontbold As Font = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)
    Dim headerfont As Font = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK)
#End Region


#Region "GenerateReport"
    Public Sub GenerateReport(ByVal trantype As String, ByVal tranid As String, ByVal divcode As String, ByVal PrntSec As Integer, ByRef bytes() As Byte, ByVal printMode As String)

        Try
            Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 30.0F, 35.0F)
            'document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)
            Dim documentWidth As Single = 550.0F


            Dim Journal_master As New DataTable
            Dim titleDt As New DataTable
            Dim JournalDs As New DataSet
            Dim conn1 As New SqlConnection
            conn1 = clsDBConnect.dbConnectionnew("strDBConnection")
            Dim sqlcmd As New SqlCommand("sp_rpt_journal", conn1)
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = tranid
            sqlcmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
            sqlcmd.Parameters.Add(New SqlParameter("@trantype", SqlDbType.VarChar, 20)).Value = trantype
            Using dad As New SqlDataAdapter
                dad.SelectCommand = sqlcmd
                dad.Fill(JournalDs)
                Journal_master = JournalDs.Tables(0)
                titleDt = JournalDs.Tables(1)
            End Using
            clsDBConnect.dbCommandClose(sqlcmd)
            clsDBConnect.dbConnectionClose(conn1)
            Dim Journalmaster() As System.Data.DataRow
            Journalmaster = Journal_master.Select("tran_id='" & tranid & "'")


            Dim lsCtry As String = objutils.GetString(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=459")


            Dim curr As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
            Dim decnum As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), String)

            Dim decno As String = "N" + decnum

            Dim coadd1 As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coadd1 from Columbusmaster"), String)
            Dim coadd2 As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coadd2 from Columbusmaster"), String)
            Dim copobox As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select copobox from Columbusmaster"), String)
            Dim cotel As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select cotel from Columbusmaster"), String)
            Dim cofax As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select cofax from Columbusmaster"), String)
            Dim coemail As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coemail from Columbusmaster"), String)
            Dim coweb As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coweb from Columbusmaster"), String)
            Dim TRNNo As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select TRNNo from Columbusmaster"), String)

            Dim subreport_detail() As System.Data.DataRow
            If trantype <> "JVR" Then
                Dim subreport_details As New DataTable
                Dim conn As New SqlConnection
                conn = clsDBConnect.dbConnectionnew("strDBConnection")
                sqlcmd = New SqlCommand("sp_rpt_journal_adjustBill", conn)
                sqlcmd.CommandType = CommandType.StoredProcedure
                sqlcmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = tranid
                sqlcmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
                Using dad As New SqlDataAdapter
                    dad.SelectCommand = sqlcmd
                    dad.Fill(subreport_details)
                End Using
                clsDBConnect.dbCommandClose(sqlcmd)
                clsDBConnect.dbConnectionClose(conn)
                subreport_detail = subreport_details.Select("against_tran_id='" & tranid & "'")
            End If

            If Journalmaster.Length > 0 Then
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
                    tableheader.WidthPercentage = 100
                    If divcode = "01" Then
                        cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
                    Else
                        cell = ImageCell("~/Images/Logo1.png", 80.0F, PdfPCell.ALIGN_LEFT)
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
                    ' Christo. 03/01/19
                    If lsCtry.Trim.ToUpper = "OM" Then
                        ' For Oman Not Required
                    Else
                        phrase.Add(New Chunk("TRN   :" & Space(6) & TRNNo & Environment.NewLine, normalfont))
                    End If
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.PaddingBottom = 5.0F
                    cell.SetLeading(12, 0)
                    tableheader.AddCell(cell)



                    Dim tblTitle As PdfPTable = New PdfPTable(1)
                    tblTitle.SetWidths(New Single() {1.0F})
                    tblTitle.TotalWidth = documentWidth
                    tblTitle.LockedWidth = True
                    Dim strTitle As String = ""
                    If titleDt.Rows.Count > 0 Then
                        strTitle = titleDt.Rows(0)("title")
                    End If
                    phrase = New Phrase()
                    phrase.Add(New Chunk(strTitle, headerfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                    cell.PaddingBottom = 3.0F
                    tblTitle.AddCell(cell)
                    tblTitle.SpacingBefore = 15
                    tblTitle.SpacingAfter = 8

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
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 3
                    FooterTable.SpacingBefore = 5.0F
                    FooterTable.AddCell(cell)
                    FooterTable.Complete = True


                    'add common header and footer part to every page
                    writer.PageEvent = New ClsHeaderFooter(tableheader, tblTitle, FooterTable, Nothing, "Voucher")
                    document.Open()

                    'common params
                    Dim arrow3() As String = Nothing
                    Dim arrData2() As String = Nothing
                    Dim receipt() As String = Nothing
                    Dim sumtotal() As String = Nothing
                    Dim acctname As String = Nothing
                    Dim costcenter_code As String = Nothing
                    Dim rdebit As Decimal
                    Dim rcredit As Decimal
                    arrow3 = {"Account No.", "Account Name / Description ", "Debit(" + curr + ")", "Credit(" + curr + ")"}



                    Dim tablecommon1 As PdfPTable = New PdfPTable(1)
                    tablecommon1.SetWidths(New Single() {1.0F})
                    tablecommon1.TotalWidth = documentWidth
                    tablecommon1.LockedWidth = True



                    Dim tbl3 As PdfPTable = New PdfPTable(8)
                    tbl3.SetWidths(New Single() {0.11F, 0.11F, 0.11F, 0.11F, 0.11F, 0.15F, 0.15F, 0.15F})
                    tbl3.TotalWidth = documentWidth
                    tbl3.LockedWidth = True
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Voucher No." & Space(10), normalfontbold))
                    phrase.Add(New Chunk(Journalmaster(0)("tran_id").ToString() & Space(40), normalfont))
                    phrase.Add(New Chunk("Date" & Space(10), normalfontbold))
                    phrase.Add(New Chunk(Format(Convert.ToDateTime(Journalmaster(0)("journal_tran_date").ToString()), "dd-MM-yyyy") & Space(40), normalfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.PaddingBottom = 10.0F
                    cell.Colspan = 8
                    tbl3.AddCell(cell)


                    For i = 0 To 3
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrow3(i), normalfontbold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                        cell.BackgroundColor = New BaseColor(192, 192, 192)
                        cell.PaddingBottom = 3.0F
                        If i = 1 Then
                            cell.Colspan = 5
                        End If
                        tbl3.AddCell(cell)
                    Next

                    For i = 0 To Journalmaster.Length - 1
                        rdebit = rdebit + Decimal.Parse(Journalmaster(i)("basedebit"))
                        rcredit = rcredit + Decimal.Parse(Journalmaster(i)("basecredit"))
                        acctname = IIf(Not (TypeOf Journalmaster(i)("acctname") Is DBNull), Journalmaster(i)("acctname").ToString(), Journalmaster(i)("costcenter_name").ToString())
                        costcenter_code = IIf(Not (TypeOf Journalmaster(i)("costcenter_code") Is DBNull), Journalmaster(i)("costcenter_code").ToString(), Journalmaster(i)("controlacctcode").ToString())
                        'receipt = {Journalmaster(i)("journal_acc_code").ToString() + Environment.NewLine + Environment.NewLine + costcenter_code, Journalmaster(i)("des").ToString() + Environment.NewLine + Environment.NewLine + acctname + Environment.NewLine + Environment.NewLine + Journalmaster(i)("journal_narration").ToString(), IIf(Decimal.Parse(Journalmaster(i)("basedebit")) = 0.0, "-", Decimal.Parse(Journalmaster(i)("basedebit")).ToString(decno)), IIf(Decimal.Parse(Journalmaster(i)("basecredit")) = 0.0, "-", Decimal.Parse(Journalmaster(i)("basecredit")).ToString(decno))}
                        receipt = {Journalmaster(i)("journal_acc_code").ToString(), Journalmaster(i)("des").ToString() + Environment.NewLine + Environment.NewLine + Journalmaster(i)("journal_narration").ToString(), IIf(Decimal.Parse(Journalmaster(i)("basedebit")) = 0.0, "-", Decimal.Parse(Journalmaster(i)("basedebit")).ToString(decno)), IIf(Decimal.Parse(Journalmaster(i)("basecredit")) = 0.0, "-", Decimal.Parse(Journalmaster(i)("basecredit")).ToString(decno))}
                        For j = 0 To 3
                            phrase = New Phrase()
                            phrase.Add(New Chunk(receipt(j), normalfont))
                            If j = 2 Or j = 3 Then
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                            Else
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                            End If
                            cell.PaddingBottom = 3.0F
                            If j = 1 Then
                                cell.Colspan = 5
                            End If
                            tbl3.AddCell(cell)
                        Next

                        If Not subreport_detail Is Nothing Then
                            For s = 0 To subreport_detail.Length - 1
                                If subreport_detail(s)("tran_lineno").ToString().Equals(Journalmaster(i)("tran_lineno").ToString()) Then
                                    arrData2 = {"Adjusted Against Following Invoices-", " ", "Invoice Date", "Type", "Invoice No.", subreport_detail(0)("field2").ToString(), "Due Date", "Amount Adjusted", " "}
                                    For k = 0 To arrData2.Length - 1
                                        phrase = New Phrase()
                                        phrase.Add(New Chunk(arrData2(k), normalfontbold))
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                        cell.PaddingBottom = 4.0F
                                        cell.PaddingTop = 1.0F
                                        If k = 0 Then
                                            cell.BackgroundColor = BaseColor.GRAY
                                            cell.Colspan = 6
                                        ElseIf k = arrData2.Length - 1 Or k = 1 Then
                                            cell.Colspan = 2
                                            cell.BorderWidthTop = 0.0F
                                            cell.BorderWidthBottom = 0.0F
                                        Else
                                            cell.BackgroundColor = New BaseColor(192, 192, 192)
                                        End If
                                        tbl3.AddCell(cell)
                                    Next
                                    arrData2 = {subreport_detail(s)("tran_date"), subreport_detail(s)("tran_type"), subreport_detail(s)("tran_id"), subreport_detail(s)("open_field2"), subreport_detail(s)("open_due_date"), Decimal.Parse(subreport_detail(s)("open_credit")).ToString(decno), " "}
                                    For l = 0 To arrData2.Length - 1
                                        phrase = New Phrase()
                                        phrase.Add(New Chunk(arrData2(l), normalfont))
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                        cell.PaddingBottom = 4.0F
                                        cell.PaddingTop = 1.0F
                                        If l = arrData2.Length - 1 Then
                                            cell.Colspan = 2
                                            cell.BorderWidthTop = 0.0F
                                        End If
                                        tbl3.AddCell(cell)
                                    Next
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                    sumtotal = {"Total", rdebit.ToString(decno), rcredit.ToString(decno)}

                    For i = 0 To 2
                        phrase = New Phrase()
                        phrase.Add(New Chunk(sumtotal(i), normalfontbold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                        cell.BackgroundColor = tback
                        cell.PaddingBottom = 3.0F
                        If i = 0 Then
                            cell.Colspan = 6
                        Else
                            cell.Colspan = 1
                        End If
                        tbl3.AddCell(cell)
                    Next
                    tbl3.Complete = True
                    tbl3.SpacingBefore = 5.0F
                    tablecommon1.AddCell(tbl3)
                    tablecommon1.SpacingBefore = 5.0F
                    tablecommon1.Complete = True
                    document.Add(tablecommon1)

                    Dim tbl4 As PdfPTable = New PdfPTable(8)
                    tbl4.SetWidths(New Single() {0.11F, 0.11F, 0.11F, 0.11F, 0.11F, 0.15F, 0.15F, 0.15F})
                    tbl4.TotalWidth = documentWidth
                    tbl4.LockedWidth = True
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Prepared By:" & Space(7) & Space(12) & Space(12) & Space(12) & Space(12), normalfontbold))
                    ' Christo - 03/01/19 - for Oman - instead of checked by, Director of Finance
                    If lsCtry.Trim.ToUpper = "OM" Then
                        phrase.Add(New Chunk("Director of Finance:" & Space(7) & Space(12) & Space(12), normalfontbold))
                        phrase.Add(New Chunk("General Manager:" & Space(7) & Space(12), normalfontbold))
                    Else
                        phrase.Add(New Chunk("Checked By:" & Space(7) & Space(12) & Space(12) & Space(12) & Space(12), normalfontbold))
                        phrase.Add(New Chunk("Approved By:" & Space(7) & Space(12), normalfontbold))
                    End If
                    'phrase.Add(New Chunk("Received By:" & Space(7) & Space(12) & Space(12) & Environment.NewLine & vbLf, normalfontbold))
                    phrase.Add(New Chunk(Environment.NewLine & vbLf & Journalmaster(0)("UserName").ToString() & Space(12) & Space(12), normalfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.PaddingBottom = 3.0F
                    cell.PaddingTop = 20.0F
                    cell.Colspan = 8
                    tbl4.AddCell(cell)
                    tbl4.Complete = True
                    tbl4.SpacingBefore = 5.0F
                    document.Add(tbl4)

                    document.AddTitle("Journal Voucher")
                    document.Close()
                    If printMode = "download" Then
                        Dim pagingFont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)
                        Dim reader As New PdfReader(memoryStream.ToArray())
                        Using mStream As New MemoryStream()
                            Using stamper As New PdfStamper(reader, mStream)
                                Dim pages As Integer = reader.NumberOfPages
                                For i As Integer = 1 To pages
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 550.0F, 10.0F, 0)
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
End Class
