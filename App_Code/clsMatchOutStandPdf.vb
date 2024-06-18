Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Globalization
Public Class clsMatchOutStandPdf
    Inherits System.Web.UI.Page
    Dim objutils As New clsUtils

#Region "global declaration"
    Dim objclsUtilities As New clsUtils
    Dim normalfont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
    Dim reportname As Font = FontFactory.GetFont("Verdana", 14, Font.BOLD, BaseColor.BLACK)
    Dim normalfontbold As Font = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)
    Dim title As Font = FontFactory.GetFont("Arial", 10, Font.BOLDITALIC, BaseColor.BLACK)
    Dim headerfont As Font = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)
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
    Public Sub GenerateReport(ByVal trantype As String, ByVal tranid As String, ByVal divcode As String, ByVal currency As String, ByRef bytes() As Byte, ByVal printMode As String)
        Try
            Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 30.0F, 35.0F)
            Dim documentWidth As Single
            documentWidth = 550.0F

            Dim matchos_master As New DataTable
            Dim conn1 As New SqlConnection
            conn1 = clsDBConnect.dbConnectionnew("strDBConnection")
            Dim strSql As String = "SELECT DISTINCT matchos_master.tran_id, matchos_master.matchos_date, matchos_master.acc_code, matchos_master.currcode, open_detail.tran_id As otran_id, open_detail.tran_date As otran_date, open_detail.tran_type As otran_type, open_detail.base_debit, open_detail.base_credit, view_account.des, matchos_master.tran_type, matchos_master.tran_date, matchos_master.narration, matchos_master.currency_rate, matchos_master.acc_type, matchos_master.gl_code, open_detail.open_due_date, open_detail.open_debit, open_detail.open_credit, matchos_master.amount, matchos_master.div_id FROM   (dbo.open_detail open_detail INNER JOIN dbo.matchos_master matchos_master ON (open_detail.against_tran_id=matchos_master.tran_id) AND (open_detail.div_id=matchos_master.div_id)) LEFT OUTER JOIN dbo.view_account view_account ON (matchos_master.acc_code=view_account.code) AND (matchos_master.acc_type=view_account.type) ORDER BY matchos_master.tran_id"
            Using matchos As New SqlDataAdapter(strSql, conn1)
                matchos.Fill(matchos_master)
            End Using
            Dim matchosmaster() As System.Data.DataRow
            matchosmaster = matchos_master.Select("tran_id='" & tranid & "'")

            Dim basecurr As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)

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

            Using memoryStream As New System.IO.MemoryStream()
                Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)

                Dim phrase As Phrase = Nothing
                Dim cell As PdfPCell = Nothing
                Dim tableheader As PdfPTable = Nothing

                'Header Table

                tableheader = New PdfPTable(2)
                tableheader.TotalWidth = documentWidth
                tableheader.LockedWidth = True
                tableheader.SetWidths(New Single() {0.7F, 0.3F})

                tableheader.Complete = False
                tableheader.SplitRows = False
                tableheader.SpacingBefore = 10.0F
                tableheader.WidthPercentage = 100
                cell = ImageCell("~/Images/logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
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


                Dim tblTitle As PdfPTable = New PdfPTable(1)
                tblTitle.SetWidths(New Single() {1.0F})
                tblTitle.TotalWidth = documentWidth
                tblTitle.LockedWidth = True
                phrase = New Phrase()
                phrase.Add(New Chunk("Match OutStanding", headerfont))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                cell.PaddingBottom = 3.0F
                tblTitle.AddCell(cell)
                tblTitle.SpacingBefore = 10
                tblTitle.SpacingAfter = 12

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
                cell.PaddingBottom = 3
                FooterTable.SpacingBefore = 12.0F
                FooterTable.AddCell(cell)
                FooterTable.Complete = True


                'add common header and footer part to every page
                writer.PageEvent = New ClsHeaderFooter(tableheader, tblTitle, FooterTable, Nothing, "Voucher")
                document.Open()


                Dim tblcommon As PdfPTable = New PdfPTable(1)
                tblcommon.SetWidths(New Single() {1.0F})
                tblcommon.TotalWidth = documentWidth
                tblcommon.LockedWidth = True
                Dim tbl As PdfPTable = New PdfPTable(6)
                tbl.SetWidths(New Single() {0.15F, 0.2F, 0.15F, 0.2F, 0.15F, 0.15F})
                phrase = New Phrase()
                phrase.Add(New Chunk("Document No" & Space(5), normalfontbold))
                phrase.Add(New Chunk(matchosmaster(0)("tran_id").ToString() & Space(25), normalfont))
                phrase.Add(New Chunk("Type" & Space(20), normalfontbold))
                phrase.Add(New Chunk(matchosmaster(0)("tran_type").ToString() & Space(25) & Environment.NewLine & vbLf, normalfont))
                phrase.Add(New Chunk("Entry Date" & Space(10), normalfontbold))
                phrase.Add(New Chunk(Format(Convert.ToDateTime(matchosmaster(0)("matchos_date").ToString()), "dd-MM-yyyy") & Space(28), normalfont))
                phrase.Add(New Chunk("Trans Date" & Space(10), normalfontbold))
                phrase.Add(New Chunk(Format(Convert.ToDateTime(matchosmaster(0)("tran_date").ToString()), "dd-MM-yyyy") & Space(25) & Environment.NewLine & vbLf, normalfont))
                phrase.Add(New Chunk("Narration" & Space(12), normalfontbold))
                phrase.Add(New Chunk(matchosmaster(0)("narration").ToString() & Space(25) & Environment.NewLine & vbLf, normalfont))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                cell.PaddingTop = 10.0F
                cell.Colspan = 6
                tbl.AddCell(cell)

                Dim arrow3() As String = Nothing
                Dim matchos_data() As String = Nothing
                Dim sumtotal() As String = Nothing
                Dim debit As Decimal
                Dim credit As Decimal
                If currency.Equals("") Then
                    arrow3 = {"Type", "Account Code / Name", "GL Code", "Currency", "Amount", "Base Amount"}
                    matchos_data = {matchosmaster(0)("acc_type").ToString(), matchosmaster(0)("acc_code").ToString + Environment.NewLine & vbLf + matchosmaster(0)("des").ToString(),
                                   matchosmaster(0)("gl_code").ToString(), matchosmaster(0)("currcode").ToString() + "   " + Decimal.Parse(matchosmaster(0)("currency_rate")).ToString(decno), Decimal.Parse(matchosmaster(0)("amount")).ToString(decno),
                                    IIf(Decimal.Parse(matchosmaster(0)("base_debit")) = 0.0, Decimal.Parse(matchos_master.Compute("Sum (base_credit)", "tran_id='" & tranid & "'")).ToString(decno), Decimal.Parse(matchos_master.Compute("Sum (base_debit)", "tran_id='" & tranid & "'")).ToString(decno))}

                Else
                    arrow3 = {"Type", "Account Code / Name", "GL Code", "Currency", "Amount"}
                    matchos_data = {matchosmaster(0)("acc_type").ToString(), matchosmaster(0)("acc_code").ToString + Environment.NewLine & vbLf + matchosmaster(0)("des").ToString(),
                                   matchosmaster(0)("gl_code").ToString(), matchosmaster(0)("currcode").ToString() + "   " + Decimal.Parse(matchosmaster(0)("currency_rate")).ToString(decno), Decimal.Parse(matchosmaster(0)("amount")).ToString(decno)}

                End If

                For i = 0 To arrow3.Length - 1
                    phrase = New Phrase()
                    phrase.Add(New Chunk(arrow3(i), normalfontbold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.PaddingBottom = 3.0F
                    If i = 0 Then
                        cell.BorderWidthRight = 0
                    ElseIf i = arrow3.Length - 1 Then
                        cell.BorderWidthLeft = 0
                    Else
                        cell.BorderWidthLeft = 0
                        cell.BorderWidthRight = 0
                    End If
                    If Not currency.Equals("") Then
                        If i = 3 Then
                            cell.Colspan = 2
                        End If
                    End If
                    cell.BackgroundColor = New BaseColor(192, 192, 192)
                    tbl.AddCell(cell)
                Next

                For i = 0 To matchos_data.Length - 1
                    phrase = New Phrase()
                    phrase.Add(New Chunk(matchos_data(i), normalfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.PaddingBottom = 3.0F
                    If i = 0 Then
                        cell.BorderWidthRight = 0
                    ElseIf i = arrow3.Length - 1 Then
                        cell.BorderWidthLeft = 0
                    Else
                        cell.BorderWidthLeft = 0
                        cell.BorderWidthRight = 0
                    End If
                    If Not currency.Equals("") Then
                        If i = 3 Then
                            cell.Colspan = 2
                        End If
                    End If
                    tbl.AddCell(cell)
                Next
                arrow3 = {"BILL DETAILS", "Bill No", "Type", "Bill Date", "Due Date", "Debit", "Credit"}
                For i = 0 To arrow3.Length - 1
                    phrase = New Phrase()
                    If i = 0 Then
                        phrase.Add(New Chunk(arrow3(i), title))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    ElseIf i = 6 Or i = 5 Then
                        phrase.Add(New Chunk(arrow3(i), normalfontbold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                    Else
                        phrase.Add(New Chunk(arrow3(i), normalfontbold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    End If
                    cell.PaddingBottom = 3.0F
                    If i = 0 Then
                        cell.Colspan = 6
                    ElseIf i = 1 Then
                        cell.BorderWidthRight = 0
                        cell.BackgroundColor = New BaseColor(192, 192, 192)
                    ElseIf i = arrow3.Length - 1 Then
                        cell.BorderWidthLeft = 0
                        cell.BackgroundColor = New BaseColor(192, 192, 192)
                    Else
                        cell.BorderWidthLeft = 0
                        cell.BorderWidthRight = 0
                        cell.BackgroundColor = New BaseColor(192, 192, 192)
                    End If
                    tbl.AddCell(cell)
                Next
                For i = 0 To matchosmaster.Length - 1
                    If currency.Equals("") Then
                        debit = debit + Decimal.Parse(matchosmaster(i)("base_debit"))
                        credit = credit + Decimal.Parse(matchosmaster(i)("base_credit"))
                        matchos_data = {matchosmaster(i)("otran_id").ToString(), matchosmaster(i)("otran_type").ToString,
                                   Format(Convert.ToDateTime(matchosmaster(i)("otran_date").ToString()), "dd-MM-yyyy"), Format(Convert.ToDateTime(matchosmaster(i)("open_due_date").ToString()), "dd-MM-yyyy"), Decimal.Parse(matchosmaster(i)("base_debit")).ToString(decno), Decimal.Parse(matchosmaster(i)("base_credit")).ToString(decno)}
                    Else
                        debit = debit + Decimal.Parse(matchosmaster(i)("open_debit"))
                        credit = credit + Decimal.Parse(matchosmaster(i)("open_credit"))
                        matchos_data = {matchosmaster(i)("otran_id").ToString(), matchosmaster(i)("otran_type").ToString,
                                   Format(Convert.ToDateTime(matchosmaster(i)("otran_date").ToString()), "dd-MM-yyyy"), Format(Convert.ToDateTime(matchosmaster(i)("open_due_date").ToString()), "dd-MM-yyyy"), Decimal.Parse(matchosmaster(i)("open_debit")).ToString(decno), Decimal.Parse(matchosmaster(i)("open_credit")).ToString(decno)}
                    End If
                    For j = 0 To matchos_data.Length - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(matchos_data(j), normalfont))
                        If j = 5 Or j = 4 Then
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                        Else
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                        End If
                        cell.PaddingBottom = 3.0F
                        If j = 0 Then
                            cell.BorderWidthRight = 0
                        ElseIf j = arrow3.Length - 1 Then
                            cell.BorderWidthLeft = 0
                        Else
                            cell.BorderWidthLeft = 0
                            cell.BorderWidthRight = 0
                        End If
                        tbl.AddCell(cell)
                    Next
                Next
                sumtotal = {"Total", debit.ToString(decno), credit.ToString(decno)}
                For i = 0 To 2
                    phrase = New Phrase()
                    phrase.Add(New Chunk(sumtotal(i), normalfontbold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.PaddingBottom = 5.0F
                    cell.PaddingTop = 5.0F
                    If i = 0 Then
                        cell.BorderWidthRight = 0
                        cell.Colspan = 4
                    ElseIf i = 2 Then
                        cell.BorderWidthLeft = 0
                        cell.Colspan = 1
                    Else
                        cell.Colspan = 1
                        cell.BorderWidthLeft = 0
                        cell.BorderWidthRight = 0
                    End If
                    tbl.AddCell(cell)
                Next
                tbl.SpacingBefore = 5
                tbl.Complete = True
                tblcommon.AddCell(tbl)
                tblcommon.SpacingBefore = 15
                tblcommon.Complete = True
                document.Add(tblcommon)

                Dim footer As PdfPTable = New PdfPTable(1)
                footer.SetWidths(New Single() {1.0F})
                footer.TotalWidth = documentWidth
                footer.LockedWidth = True
                phrase = New Phrase()
                phrase.Add(New Chunk("Prepared By" & Space(140), normalfontbold))
                phrase.Add(New Chunk("Approved By" & Space(20) & Environment.NewLine & vbLf, normalfontbold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                footer.AddCell(cell)
                footer.Complete = True
                footer.SpacingBefore = 20
                document.Add(footer)
                document.AddTitle("Match OutStanding")
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

End Class
