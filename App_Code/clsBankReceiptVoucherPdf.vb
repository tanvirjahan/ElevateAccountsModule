Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Globalization
Imports System.Linq

Public Class clsBankReceiptVoucherPdf
    Inherits System.Web.UI.Page
    Dim objutils As New clsUtils
#Region "global declaration"
    Dim objclsUtilities As New clsUtils
    Dim Header As BaseColor = New BaseColor(179, 217, 255)
    Dim tback As BaseColor = New BaseColor(255, 188, 155)
    Dim normalfont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
    Dim normalfontgray As Font = FontFactory.GetFont("Arial", 7, Font.NORMAL, BaseColor.DARK_GRAY)
    Dim footerdfont As Font = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)
    Dim companyTitle As Font = FontFactory.GetFont("Verdana", 12, Font.BOLD, BaseColor.BLACK)
    Dim reportname As Font = FontFactory.GetFont("Verdana", 14, Font.BOLD, BaseColor.BLACK)
    Dim normalfontbold As Font = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)
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


    'modified by priyanka 23/12/2019---to change receipt voucher to old format.
#Region "GenerateReport"
    Public Sub GenerateReport(ByVal trantype As String, ByVal tranid As String, ByVal divcode As String, ByVal CashBankType As String, ByVal PrntSec As Integer, ByVal PrntCliCurr As Integer, ByVal PrinDocTitle As String, ByRef bytes() As Byte, ByVal printMode As String)

        Try
            Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 30.0F, 35.0F)
            Dim documentWidth As Single
            documentWidth = 550.0F
            Dim rptreportname As String = Nothing
            Dim receipt_master As New DataTable
            Dim conn1 As New SqlConnection
            conn1 = clsDBConnect.dbConnectionnew("strDBConnection")
            Dim strSql As String
            Dim sqlcmd As New SqlCommand("sp_rpt_receipt", conn1)
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = tranid
            sqlcmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
            sqlcmd.Parameters.Add(New SqlParameter("@prtClientcurr", SqlDbType.Int)).Value = PrntCliCurr
            sqlcmd.Parameters.Add(New SqlParameter("@trantype", SqlDbType.VarChar, 20)).Value = trantype
            Using dad As New SqlDataAdapter
                dad.SelectCommand = sqlcmd
                dad.Fill(receipt_master)
            End Using
            clsDBConnect.dbCommandClose(sqlcmd)
            clsDBConnect.dbConnectionClose(conn1)
            Dim receiptmaster() As System.Data.DataRow
            receiptmaster = receipt_master.Select("tran_id='" & tranid & "'")

            'If receiptmaster.Length = 0 Then
            '    strSql = "SELECT receipt_master_new.tran_id, receipt_master_new.receipt_date,  receipt_master_new.receipt_currency_id , receipt_master_new.basecredit, " & _
            '    "receipt_master_new.receipt_received_from, receipt_detail.basedebit As rddebit, receipt_detail.basecredit As rdcredit,receipt_detail.receipt_currency_id as receipt_currency_id_clicurr ," & _
            '    "receipt_detail.receipt_debit as receipt_debit ,receipt_detail.receipt_credit as receipt_credit , receipt_master_new.receipt_cashbank_type, receipt_master_new.receipt_cheque_number, " & _
            '    "view_account.des, receipt_master_new.receipt_narration, receipt_detail.receipt_acc_code, receipt_detail.receipt_acc_type, receipt_detail.costcenter_code, " & _
            '    "receipt_detail.receipt_narration As narration, customer_bank_master.other_bank_master_des, receipt_master_new.receipt_cashbank_code, view_account_1.des As dest, " & _
            '    "receipt_master_new.basedebit, acctmast.acctname, '' costcenter_name, view_account.controlacctcode, UserMaster.UserName, receipt_detail.tran_id, " & _
            '    "receipt_detail.div_id, receipt_master_new.receipt_mrv, receipt_detail.tran_lineno FROM   ((((((dbo.receipt_detail receipt_detail INNER JOIN dbo.receipt_master_new receipt_master_new " & _
            '    "ON ((receipt_detail.tran_type=receipt_master_new.tran_type) AND (receipt_detail.tran_id=receipt_master_new.tran_id)) AND (receipt_detail.div_id=receipt_master_new.div_id)) " & _
            '    "INNER JOIN dbo.view_account view_account ON ((receipt_detail.receipt_acc_code=view_account.code) AND (receipt_detail.receipt_acc_type=view_account.type)) " & _
            '    "AND (receipt_detail.div_id=view_account.div_code))) LEFT OUTER JOIN dbo.customer_bank_master customer_bank_master ON " & _
            '    "receipt_master_new.receipt_customer_bank=customer_bank_master.other_bank_master_code) INNER JOIN dbo.view_account view_account_1 ON " & _
            '    "(receipt_master_new.receipt_cashbank_code=view_account_1.code) AND (receipt_master_new.receipt_div_id=view_account_1.div_code)) " & _
            '    "LEFT OUTER JOIN dbo.UserMaster UserMaster ON receipt_master_new.adduser=UserMaster.UserCode) LEFT OUTER JOIN dbo.acctmast acctmast ON " & _
            '    "(view_account.controlacctcode=acctmast.acctcode) AND (view_account.div_code=acctmast.div_code) where receipt_master_new.tran_id ='" + tranid + "' " & _
            '    "and receipt_master_new.receipt_div_id='" + divcode + "' ORDER BY receipt_master_new.tran_id, receipt_detail.tran_lineno"

            '    'LEFT OUTER JOIN dbo.costcenter_master costcenter_master ON receipt_detail.costcenter_code=costcenter_master.costcenter_code

            '    Using dad As New SqlDataAdapter(strSql, conn1)
            '        dad.Fill(receipt_master)
            '    End Using


            '    receiptmaster = receipt_master.Select("tran_id='" & tranid & "'")
            'End If
            Dim voccurr, curr As String
            curr = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)

            If PrntCliCurr = 1 Then
                voccurr = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select top 1 receipt_currency_id from receipt_Detail  where tran_id='" & tranid & "' and receipt_acc_type<>'G' "), String)
            End If
            If voccurr = "" Then
                voccurr = curr
            End If

            Dim lsCtry As String = objutils.GetString(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=459")

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

            Dim settle_details As New DataTable
            Dim conn As New SqlConnection
            conn = clsDBConnect.dbConnectionnew("strDBConnection")
            sqlcmd = New SqlCommand("sp_rpt_receipt_settle", conn)
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = tranid
            sqlcmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
            Using dad As New SqlDataAdapter
                dad.SelectCommand = sqlcmd
                dad.Fill(settle_details)
            End Using
            clsDBConnect.dbCommandClose(sqlcmd)
            clsDBConnect.dbConnectionClose(conn)
            Dim settle_detail() As System.Data.DataRow
            settle_detail = settle_details.Select("against_tran_id='" & tranid & "'")

            Dim decimalword As String = Nothing
            Dim decimalInword As String = Nothing
            Dim fractionword As String = Nothing
            'Dim rdcredit As Decimal = 0
            Dim fraction As Decimal
            'Dim fraction As Decimal = Math.Round(Decimal.Parse(receiptmaster(0)("basecredit")), 2) - Math.Truncate(Math.Round(Decimal.Parse(receiptmaster(0)("basecredit")), 3))
            If receiptmaster.Length <> 0 Then
                If PrntCliCurr = 1 Then
                    decimalword = Math.Truncate(Decimal.Parse(receiptmaster(0)("receipt_amount"))).ToString()
                    decimalInword = AmountInWords(decimalword)
                    fraction = Math.Round(Decimal.Parse(receiptmaster(0)("receipt_amount")), Convert.ToInt32(decnum), MidpointRounding.AwayFromZero) Mod 1

                Else
                    'Dim TotalBaseCredit As Decimal = receipt_master.AsEnumerable().Where(Function(row) row.Field(Of String)("receipt_acc_type") <> "G").Sum(Function(row) row.Field(Of Decimal)("rdcredit"))
                    'Dim TotalBaseDebit As Decimal = receipt_master.AsEnumerable().Where(Function(row) row.Field(Of String)("receipt_acc_type") <> "G").Sum(Function(row) row.Field(Of Decimal)("rddebit"))
                    'rdcredit = TotalBaseCredit - TotalBaseDebit
                    decimalword = Math.Truncate(Decimal.Parse(receiptmaster(0)("receipt_amount"))).ToString()
                    decimalInword = AmountInWords(decimalword)
                    fraction = Math.Round(Decimal.Parse(receiptmaster(0)("receipt_amount")), Convert.ToInt32(decnum), MidpointRounding.AwayFromZero) Mod 1

                End If
            End If
            'Modified Param 15/11/2018

            If fraction > 0 Then
                Dim arrFraction As String() = fraction.ToString.Split(".")
                If arrFraction.Length = 2 Then
                    fraction = arrFraction(1)
                Else
                    fraction = 0
                End If
            End If
            If fraction > 0 Then
                Dim coin As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select currcoin from currmast where currcode= '" & voccurr.Trim & "'"), String)
                fractionword = "AND " & coin & "  " & AmountInWords(fraction.ToString()) + "  " + "ONLY"
                'If decnum.Equals("2") Then
                '    fractionword = (fraction.ToString()).Split(New Char() {"."})(1) & "/100" + " " + "FILS ONLY"
                'ElseIf decnum.Equals("3") AndAlso trantype.Equals("RV") Then
                '    fractionword = (fraction.ToString()).Split(New Char() {"."})(1) & "/1000" + " " + "BAIZA ONLY"
                'ElseIf decnum.Equals("3") AndAlso trantype.Equals("CRV") Then
                '    fractionword = (fraction.ToString()).Split(New Char() {"."})(1) & "/1000" + " " + "FILS ONLY"
                'End If
            Else
                decimalInword = UCase(decimalInword) + "  " + "ONLY"
                fractionword = ""
            End If

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
                'changed by Christo on 02/01/2019
                'Dim lsCtry As String = objutils.GetString(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=459")
                If lsCtry.Trim.ToUpper = "OM" Then
                    ' TRN not required for Oman 
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
                phrase = New Phrase()
                If trantype.Equals("RV") Then
                    rptreportname = "Receipt Voucher"
                    phrase.Add(New Chunk(rptreportname, headerfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                    cell.PaddingBottom = 3.0F
                ElseIf trantype.Equals("CV") Then
                    rptreportname = "Contra Voucher"
                    phrase.Add(New Chunk(rptreportname, headerfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                    cell.PaddingBottom = 3.0F
                Else
                    rptreportname = "Cash Receipt Voucher"
                    phrase.Add(New Chunk(rptreportname, reportname))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                    cell.PaddingBottom = 3.0F
                End If
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

                Dim cheque As Boolean = False
                Dim ccredit As Boolean = False
                Dim cash As Boolean = False
                Dim banktransfer As Boolean = False

                Dim cheque_number As String = receiptmaster(0)("receipt_cheque_number").ToString
                Dim bank_type As String = receiptmaster(0)("receipt_cashbank_type").ToString
                Dim bank_transfer As String = receiptmaster(0)("other_bank_master_des").ToString

                If bank_type.Equals("B") AndAlso (cheque_number.ToUpper().IndexOf("CC") <> 1 AndAlso cheque_number.ToUpper().IndexOf("CASH") <> 1) Then
                    cheque = True
                End If
                If bank_type.Equals("B") AndAlso (cheque_number.ToUpper().IndexOf("CC") = 1) Then
                    ccredit = True
                End If
                If bank_type.Equals("C") Then
                    cash = True
                End If

                If UCase(bank_transfer) = "BANK TRANSFER" Then
                    banktransfer = True
                    cheque = False
                    cash = False
                    ccredit = False
                End If

                Dim tblcommon As PdfPTable = New PdfPTable(1)
                tblcommon.SetWidths(New Single() {1.0F})
                tblcommon.TotalWidth = documentWidth
                tblcommon.LockedWidth = True
                'Dim tbl As PdfPTable = New PdfPTable(7)
                'tbl.SetWidths(New Single() {0.15F, 0.05F, 0.15F, 0.05F, 0.15F, 0.05F, 0.4F})
                Dim tbl As PdfPTable = New PdfPTable(9)
                tbl.SetWidths(New Single() {0.3F, 0.05F, 0.15F, 0.05F, 0.15F, 0.05F, 0.3F, 0.05F, 0.3F})
                If receiptmaster.Length <> 0 Then
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Voucher No." & Space(10), normalfontbold))
                    phrase.Add(New Chunk(receiptmaster(0)("tran_id").ToString() & Space(25), normalfont))
                    phrase.Add(New Chunk("Date" & Space(10), normalfontbold))
                    phrase.Add(New Chunk(Format(Convert.ToDateTime(receiptmaster(0)("receipt_date").ToString()), "dd-MM-yyyy") & Space(25), normalfont))

                    If PrntCliCurr = 1 Then
                        phrase.Add(New Chunk("Amount(" & voccurr & ")" & Space(10), normalfontbold))
                        phrase.Add(New Chunk(Decimal.Parse(receiptmaster(0)("receipt_Amount")).ToString(decno) & Environment.NewLine & vbLf & vbLf, normalfont))
                    Else
                        phrase.Add(New Chunk("Amount(" & voccurr & ")" & Space(10), normalfontbold))
                        phrase.Add(New Chunk(Decimal.Parse(receiptmaster(0)("receipt_Amount")).ToString(decno) & Environment.NewLine & vbLf & vbLf, normalfont))
                    End If
                    ' If trantype.Equals("RV") Then
                    phrase.Add(New Chunk("M.RV No." & Space(15), normalfontbold))
                    phrase.Add(New Chunk(receiptmaster(0)("receipt_mrv").ToString() & Environment.NewLine & vbLf & vbLf, normalfont))
                    phrase.Add(New Chunk("Received From" & Space(5), normalfontbold))
                    phrase.Add(New Chunk(receiptmaster(0)("receipt_received_from").ToString() & vbLf, normalfont))
                    phrase.Add(New Chunk("     " & Space(20) & "------------------------------------------------------------------------------------------------------------------" & Environment.NewLine & vbLf))
                    'Else
                    '    phrase.Add(New Chunk("Paid For" & Space(16), normalfontbold))
                    '    phrase.Add(New Chunk(receiptmaster(0)("receipt_received_from").ToString() & vbLf & vbLf & vbLf, normalfont))
                    '  End If
                End If
                phrase.Add(New Chunk("Amount" & Space(17), normalfontbold))
                phrase.Add(New Chunk(voccurr & "  ", normalfont))
                phrase.Add(New Chunk(decimalInword.ToUpper() & Space(1) & fractionword.ToUpper() & Environment.NewLine & vbLf & vbLf, normalfont))

                'If trantype.Equals("CRV") Then
                ' phrase.Add(New Chunk("Received By" & Space(10) & "----------------------------------------------------" & Space(10) & "Signature" & Space(10) & "----------------------------------------------------" & vbLf & vbLf, normalfontbold))
                ' End If
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                cell.PaddingTop = 10.0F
                'cell.Colspan = 7
                cell.Colspan = 9
                tbl.AddCell(cell)
                '  If trantype.Equals("RV") Then
                phrase = New Phrase()
                phrase.Add(New Chunk("Collection", normalfontbold))
                phrase.Add(New Chunk("Type", normalfontbold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                'cell.Width = 30.0F
                cell.Colspan = 1
                tbl.AddCell(cell)



                cell = IIf(cash, ImageCell("~/Images/checked.png", 35.0F, PdfPCell.ALIGN_LEFT), ImageCell("~/Images/uncheck.png", 35.0F, PdfPCell.ALIGN_LEFT))
                cell.Colspan = 1
                cell.PaddingTop = 3
                cell.PaddingLeft = 3
                tbl.AddCell(cell)
                phrase = New Phrase()
                phrase.Add(New Chunk("Cash", normalfontbold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                cell.Colspan = 1
                tbl.AddCell(cell)

                cell = IIf(ccredit, ImageCell("~/Images/checked.png", 35.0F, PdfPCell.ALIGN_LEFT), ImageCell("~/Images/uncheck.png", 35.0F, PdfPCell.ALIGN_LEFT))
                cell.PaddingTop = 3
                cell.Colspan = 1
                tbl.AddCell(cell)
                phrase = New Phrase()
                phrase.Add(New Chunk("C.Card", normalfontbold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                cell.Colspan = 1
                tbl.AddCell(cell)

                cell = IIf(cheque, ImageCell("~/Images/checked.png", 35.0F, PdfPCell.ALIGN_LEFT), ImageCell("~/Images/uncheck.png", 35.0F, PdfPCell.ALIGN_LEFT))
                cell.Colspan = 1
                cell.PaddingTop = 2
                tbl.AddCell(cell)
                phrase = New Phrase()
                phrase.Add(New Chunk("Cheque" & Environment.NewLine & vbLf & vbLf, normalfontbold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                cell.Colspan = 1
                tbl.AddCell(cell)

                '''' Bank Transfer - christo - 22/12/18
                cell = IIf(banktransfer, ImageCell("~/Images/checked.png", 35.0F, PdfPCell.ALIGN_LEFT), ImageCell("~/Images/uncheck.png", 35.0F, PdfPCell.ALIGN_LEFT))
                cell.Colspan = 1
                cell.PaddingTop = 2
                tbl.AddCell(cell)
                phrase = New Phrase()
                phrase.Add(New Chunk("Bank Transfer" & Environment.NewLine & vbLf & vbLf, normalfontbold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                cell.Colspan = 1
                tbl.AddCell(cell)
                If receiptmaster.Length <> 0 Then
                    '''''''''''''''''''''''''''''''''''''''
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Cheque No." & Space(11), normalfontbold))
                    phrase.Add(New Chunk(receiptmaster(0)("receipt_cheque_number").ToString() & Space(50), normalfont))
                    phrase.Add(New Chunk("Bank/Branch" & Space(9), normalfontbold))
                    phrase.Add(New Chunk(receiptmaster(0)("other_bank_master_des").ToString() & vbLf, normalfont))
                    phrase.Add(New Chunk("          " & Space(15) & "---------------------------" & Space(25) & "         " & "------------------------------" & Environment.NewLine & vbLf))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.PaddingBottom = 2.0F
                    'cell.Colspan = 7
                    cell.Colspan = 9

                    tbl.AddCell(cell)
                End If
                phrase = New Phrase()
                phrase.Add(New Chunk("Description", normalfontbold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                'cell.Colspan = 1
                cell.Colspan = 2
                cell.Border = Rectangle.NO_BORDER
                tbl.AddCell(cell)
                phrase = New Phrase()
                phrase.Add(New Chunk(receiptmaster(0)("receipt_narration").ToString() & vbLf & vbLf & vbLf & vbLf & vbLf & vbLf, normalfont))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                cell.Border = Rectangle.NO_BORDER
                'cell.Colspan = 6
                cell.Colspan = 7

                tbl.AddCell(cell)
                phrase = New Phrase()
                phrase.Add(New Chunk("Prepared By:" & Space(10) & Space(10) & Space(10), normalfontbold))
                'phrase.Add(New Chunk("Checked By:" & Space(15) & Space(20) & Space(20), normalfontbold))
                'changed by Christo on 02/01/2019
                If lsCtry.Trim.ToUpper = "OM" Then
                    phrase.Add(New Chunk("Director of Finance:" & Space(15) & Space(15), normalfontbold))
                    phrase.Add(New Chunk("General Manager:" & Space(5) & Space(5) & Environment.NewLine & vbLf, normalfontbold))
                Else
                    phrase.Add(New Chunk("Checked By:" & Space(10) & Space(10) & Space(10), normalfontbold))
                    phrase.Add(New Chunk("Approved By:" & Space(10) & Space(10) & Environment.NewLine & vbLf, normalfontbold))
                End If

                phrase.Add(New Chunk(receiptmaster(0)("UserName").ToString() & Space(20) & Space(20), normalfont))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                cell.PaddingTop = 100.0F
                'cell.Colspan = 7
                cell.Colspan = 9

                tbl.AddCell(cell)

                If trantype.Equals("RV") Or trantype.Equals("CV") Then
                    tblcommon.AddCell(tbl)
                    tblcommon.SpacingBefore = 0
                    tblcommon.Complete = True
                    document.Add(tblcommon)
                End If

                'common params
                Dim arrow3() As String = Nothing
                Dim receipt() As String = Nothing
                Dim sumtotal() As String = Nothing
                Dim acctname As String = Nothing
                Dim costcenter_code As String = Nothing
                Dim rdebit As Decimal = rdebit + Decimal.Parse(receiptmaster(0)("basecredit"))
                Dim rcredit As Decimal = rcredit + Decimal.Parse(receiptmaster(0)("basedebit"))
                Dim debit As String = IIf(Decimal.Parse(receiptmaster(0)("basedebit")) = 0.0, "-", Decimal.Parse(receiptmaster(0)("basedebit")).ToString(decno))
                Dim credit As String = IIf(Decimal.Parse(receiptmaster(0)("basecredit")) = 0.0, "-", Decimal.Parse(receiptmaster(0)("basecredit")).ToString(decno))

                arrow3 = {"Account No.", "Account Name / Description ", "Debit(" + curr + ")", "Credit(" + curr + ")", receiptmaster(0)("receipt_cashbank_code").ToString(), receiptmaster(0)("dest").ToString() + Environment.NewLine + Environment.NewLine + receiptmaster(0)("receipt_narration").ToString(), credit, debit}
                If trantype.Equals("CRV") Then
                    cashReceiptVoucher(document, decno, receiptmaster, settle_detail, tbl, arrow3, rdebit, rcredit, normalfont, normalfontbold, documentWidth)
                ElseIf PrntSec = 1 AndAlso (trantype.Equals("RV") Or trantype.Equals("CV")) Then

                    document.NewPage()
                    'Dim tablecommon1 As PdfPTable = New PdfPTable(1)
                    'tablecommon1.SetWidths(New Single() {1.0F})
                    'tablecommon1.TotalWidth = documentWidth
                    'tablecommon1.SplitRows = True
                    'tablecommon1.Complete = False
                    'tablecommon1.LockedWidth = True

                    'Dim tbl2 As PdfPTable = New PdfPTable(8)
                    'tbl2.SetWidths(New Single() {0.11F, 0.1F, 0.11F, 0.2F, 0.15F, 0.11F, 0.11F, 0.11F})
                    'tbl2.TotalWidth = documentWidth
                    'tbl2.LockedWidth = True
                    ''tbl2.KeepTogether = True

                    'phrase = New Phrase()
                    'phrase.Add(New Chunk("Voucher No." & Space(10), normalfontbold))
                    'phrase.Add(New Chunk(receiptmaster(0)("tran_id").ToString() & Space(40), normalfont))
                    'phrase.Add(New Chunk("Date" & Space(10), normalfontbold))
                    'phrase.Add(New Chunk(Format(Convert.ToDateTime(receiptmaster(0)("receipt_date").ToString()), "dd-MM-yyyy") & Space(40), normalfont))
                    'phrase.Add(New Chunk("Amount(" & curr & ")" & Space(10), normalfontbold))
                    'phrase.Add(New Chunk(Decimal.Parse(receiptmaster(0)("basecredit")).ToString(decno) & Environment.NewLine & vbLf, normalfont))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    'cell.PaddingBottom = 3.0F
                    'cell.Colspan = 8
                    'tbl2.AddCell(cell)
                    'tablecommon1.AddCell(tbl2)
                    Dim tbl3 As PdfPTable = New PdfPTable(8)
                    tbl3.SetWidths(New Single() {0.11F, 0.1F, 0.11F, 0.15F, 0.12F, 0.11F, 0.15F, 0.15F})
                    tbl3.TotalWidth = documentWidth
                    tbl3.SplitRows = False
                    tbl3.Complete = False
                    tbl3.LockedWidth = True
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Voucher No." & Space(10), normalfontbold))
                    phrase.Add(New Chunk(receiptmaster(0)("tran_id").ToString() & Space(25), normalfont))
                    phrase.Add(New Chunk("Date" & Space(10), normalfontbold))
                    phrase.Add(New Chunk(Format(Convert.ToDateTime(receiptmaster(0)("receipt_date").ToString()), "dd-MM-yyyy") & Space(25), normalfont))
                    phrase.Add(New Chunk("Amount(" & curr & ")" & Space(10), normalfontbold))
                    'If PrntCliCurr = 1 Then
                    '    phrase.Add(New Chunk(Decimal.Parse(receiptmaster(0)("receipt_credit")).ToString(decno) & Environment.NewLine & vbLf, normalfont))
                    'Else
                    phrase.Add(New Chunk(Decimal.Parse(receiptmaster(0)("basecredit")).ToString(decno) & Environment.NewLine & vbLf, normalfont))

                    '   End If


                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.PaddingTop = 3.0F
                    cell.PaddingBottom = 3.0F
                    cell.Colspan = 8
                    cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER
                    tbl3.AddCell(cell)


                    phrase = New Phrase()
                    phrase.Add(New Chunk("Settlement Details" & vbLf & vbLf, footerdfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.PaddingBottom = 3.0F
                    cell.Colspan = 8
                    cell.Border = Rectangle.BOTTOM_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER
                    tbl3.AddCell(cell)

                    If settle_detail.Length > 0 Then
                        ' Sub Table-Adjust Bill Detail
                        Dim arrData2() As String
                        arrData2 = {"Date", "Voucher Type", "Voucher No.", settle_detail(0)("field2").ToString(), settle_detail(0)("field3").ToString(), "Due Date", "Amount Adjusted"}

                        For i = 0 To arrData2.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrData2(i), normalfontbold))
                            If i = 6 Then
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.Colspan = 2
                            Else
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                            End If
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = New BaseColor(215, 215, 215)
                            If i <= 5 Then
                                cell.BorderWidthBottom = 0
                            End If
                            tbl3.AddCell(cell)
                        Next

                        arrData2 = {"", "", "", "", "", "", settle_detail(0)("receipt_currency_id").ToString(), curr}
                        Dim saleDecno As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select nodigit from currmast where currcode= '" & settle_detail(0)("receipt_currency_id").ToString() & "'"), String)
                        For i = 0 To arrData2.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrData2(i), normalfontbold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                            cell.PaddingBottom = 4.0F
                            cell.Colspan = 1
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = New BaseColor(215, 215, 215)
                            If i <= 5 Then
                                cell.BorderWidthTop = 0
                            End If
                            tbl3.AddCell(cell)
                        Next
                        Dim totalodebit As Decimal
                        Dim totalbdebit As Decimal
                        'Table Data
                        For i = 0 To settle_detail.Length - 1
                            arrData2 = {settle_detail(i)("tran_date"), settle_detail(i)("tran_type"), settle_detail(i)("tran_id"), settle_detail(i)("open_field2"), settle_detail(i)("open_field3"), settle_detail(i)("open_due_date"), Decimal.Parse(settle_detail(i)("open_credit")).ToString("N" + saleDecno), Decimal.Parse(settle_detail(i)("base_credit")).ToString(decno)}
                            totalodebit = totalodebit + Decimal.Parse(settle_detail(i)("open_credit"))
                            totalbdebit = totalbdebit + Decimal.Parse(settle_detail(i)("base_credit"))
                            For j = 0 To arrData2.Length - 1
                                phrase = New Phrase()
                                phrase.Add(New Chunk(arrData2(j), normalfont))
                                If j > 5 Then
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT

                                Else
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                End If
                                cell.PaddingBottom = 4.0F
                                cell.PaddingTop = 1.0F
                                tbl3.AddCell(cell)
                            Next
                        Next

                        arrData2 = {"", "", "", "", "", "Total", totalodebit.ToString("N" + saleDecno), totalbdebit.ToString(decno)}

                        For i = 0 To arrData2.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrData2(i), normalfontbold))

                            If i > 5 Then
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT

                            Else
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                            End If
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            tbl3.AddCell(cell)
                        Next
                    End If

                    phrase = New Phrase()
                    phrase.Add(New Chunk(" " & vbLf & vbLf, footerdfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.PaddingBottom = 3.0F
                    cell.Colspan = 8
                    tbl3.AddCell(cell)

                    For i = 0 To 7
                        phrase = New Phrase()
                        If i <= 3 Then
                            phrase.Add(New Chunk(arrow3(i), normalfontbold))
                        Else
                            phrase.Add(New Chunk(arrow3(i), normalfont))
                        End If
                        If i = 7 Or i = 6 Then
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                        ElseIf i < 4 Then
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                        Else
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        End If
                        cell.BackgroundColor = Header
                        cell.PaddingBottom = 3.0F

                        If i = 1 Or i = 5 Then
                            cell.Colspan = 5
                        Else
                            cell.Colspan = 1
                        End If
                        tbl3.AddCell(cell)
                    Next


                    For i = 0 To receiptmaster.Length - 1

                        acctname = IIf(Not (TypeOf receiptmaster(i)("acctname") Is DBNull), receiptmaster(i)("acctname").ToString(), receiptmaster(i)("costcenter_name").ToString())
                        costcenter_code = IIf(Not (TypeOf receiptmaster(i)("costcenter_code") Is DBNull), receiptmaster(i)("costcenter_code").ToString(), receiptmaster(i)("controlacctcode").ToString())
                        'If PrntCliCurr = 1 Then
                        '    receipt = {receiptmaster(i)("receipt_acc_code").ToString() + Environment.NewLine + Environment.NewLine + costcenter_code, receiptmaster(i)("des").ToString() + Environment.NewLine + Environment.NewLine + acctname + Environment.NewLine + Environment.NewLine + receiptmaster(i)("narration").ToString(), IIf(Decimal.Parse(receiptmaster(i)("receipt_debit")) = 0.0, "-", Decimal.Parse(receiptmaster(i)("receipt_debit")).ToString(decno)), IIf(Decimal.Parse(receiptmaster(i)("receipt_credit")) = 0.0, "-", Decimal.Parse(receiptmaster(i)("receipt_credit")).ToString(decno))}
                        '    rdebit = rdebit + Decimal.Parse(receiptmaster(i)("receipt_debit"))
                        '    rcredit = rcredit + Decimal.Parse(receiptmaster(i)("receipt_credit"))

                        'Else
                        Dim acctNo As String = ""
                        Dim acctDesc As String = ""
                        If (receiptmaster(i)("receipt_acc_type") = "G") Then
                            acctNo = receiptmaster(i)("receipt_acc_code").ToString()
                            acctDesc = receiptmaster(i)("des").ToString() + Environment.NewLine + Environment.NewLine + receiptmaster(i)("narration").ToString()
                        Else
                            acctNo = receiptmaster(i)("receipt_acc_code").ToString() + Environment.NewLine + Environment.NewLine + costcenter_code
                            acctDesc = receiptmaster(i)("des").ToString() + Environment.NewLine + Environment.NewLine + acctname + Environment.NewLine + Environment.NewLine + receiptmaster(i)("narration").ToString()
                        End If
                        receipt = {acctNo, acctDesc, IIf(Decimal.Parse(receiptmaster(i)("rddebit")) = 0.0, "-", Decimal.Parse(receiptmaster(i)("rddebit")).ToString(decno)), IIf(Decimal.Parse(receiptmaster(i)("rdcredit")) = 0.0, "-", Decimal.Parse(receiptmaster(i)("rdcredit")).ToString(decno))}
                        rdebit = rdebit + Decimal.Parse(receiptmaster(i)("rddebit"))
                        rcredit = rcredit + Decimal.Parse(receiptmaster(i)("rdcredit"))

                        ' End If
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
                            Else
                                cell.Colspan = 1
                            End If
                            tbl3.AddCell(cell)
                        Next
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

                    phrase = New Phrase()

                    If lsCtry.Trim.ToUpper = "OM" Then
                        phrase.Add(New Chunk("Director of Finance:" & Space(15) & Space(15) & Space(10), normalfontbold))
                        phrase.Add(New Chunk("General Manager:" & Space(15) & Space(5), normalfontbold))
                    Else
                        phrase.Add(New Chunk("Prepared By:" & Space(10) & Space(10) & Space(10), normalfontbold))
                        phrase.Add(New Chunk("Checked By:" & Space(10) & Space(10) & Space(10), normalfontbold))
                    End If

                    phrase.Add(New Chunk("Approved By:" & Space(10) & Space(10) & Environment.NewLine & vbLf, normalfontbold))
                    phrase.Add(New Chunk(receiptmaster(0)("UserName").ToString() & Space(20) & Space(20), normalfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.PaddingBottom = 3.0F
                    cell.PaddingTop = 50.0F
                    cell.Colspan = 8
                    tbl3.AddCell(cell)
                    tbl3.SpacingBefore = 0
                    tbl3.Complete = True
                    'tablecommon1.AddCell(tbl3)
                    'tablecommon1.SpacingBefore = 0
                    'tablecommon1.Complete = True
                    'document.Add(tablecommon1)
                    document.Add(tbl3)
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

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
#End Region

    Public Sub GenerateReportRV(ByVal trantype As String, ByVal tranid As String, ByVal divcode As String, ByVal CashBankType As String, ByVal PrntSec As Integer, ByVal PrntCliCurr As Integer, ByVal PrinDocTitle As String, ByRef bytes() As Byte, ByVal printMode As String)

        Try
            Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 30.0F, 35.0F)
            Dim documentWidth As Single
            documentWidth = 550.0F
            Dim rptreportname As String = Nothing
            Dim receipt_master As New DataTable
            Dim conn1 As New SqlConnection
            conn1 = clsDBConnect.dbConnectionnew("strDBConnection")
            Dim strSql As String
            Dim sqlcmd As New SqlCommand("sp_rpt_receipt", conn1)
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = tranid
            sqlcmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
            sqlcmd.Parameters.Add(New SqlParameter("@prtClientcurr", SqlDbType.Int)).Value = PrntCliCurr
            sqlcmd.Parameters.Add(New SqlParameter("@trantype", SqlDbType.VarChar, 20)).Value = trantype
            Using dad As New SqlDataAdapter
                dad.SelectCommand = sqlcmd
                dad.Fill(receipt_master)
            End Using
            clsDBConnect.dbCommandClose(sqlcmd)
            clsDBConnect.dbConnectionClose(conn1)
            Dim receiptmaster() As System.Data.DataRow
            receiptmaster = receipt_master.Select("tran_id='" & tranid & "'")

            'If receiptmaster.Length = 0 Then
            '    strSql = "SELECT receipt_master_new.tran_id, receipt_master_new.receipt_date,  receipt_master_new.receipt_currency_id , receipt_master_new.basecredit, " & _
            '    "receipt_master_new.receipt_received_from, receipt_detail.basedebit As rddebit, receipt_detail.basecredit As rdcredit,receipt_detail.receipt_currency_id as receipt_currency_id_clicurr ," & _
            '    "receipt_detail.receipt_debit as receipt_debit ,receipt_detail.receipt_credit as receipt_credit , receipt_master_new.receipt_cashbank_type, receipt_master_new.receipt_cheque_number, " & _
            '    "view_account.des, receipt_master_new.receipt_narration, receipt_detail.receipt_acc_code, receipt_detail.receipt_acc_type, receipt_detail.costcenter_code, " & _
            '    "receipt_detail.receipt_narration As narration, customer_bank_master.other_bank_master_des, receipt_master_new.receipt_cashbank_code, view_account_1.des As dest, " & _
            '    "receipt_master_new.basedebit, acctmast.acctname, '' costcenter_name, view_account.controlacctcode, UserMaster.UserName, receipt_detail.tran_id, " & _
            '    "receipt_detail.div_id, receipt_master_new.receipt_mrv, receipt_detail.tran_lineno FROM   ((((((dbo.receipt_detail receipt_detail INNER JOIN dbo.receipt_master_new receipt_master_new " & _
            '    "ON ((receipt_detail.tran_type=receipt_master_new.tran_type) AND (receipt_detail.tran_id=receipt_master_new.tran_id)) AND (receipt_detail.div_id=receipt_master_new.div_id)) " & _
            '    "INNER JOIN dbo.view_account view_account ON ((receipt_detail.receipt_acc_code=view_account.code) AND (receipt_detail.receipt_acc_type=view_account.type)) " & _
            '    "AND (receipt_detail.div_id=view_account.div_code))) LEFT OUTER JOIN dbo.customer_bank_master customer_bank_master ON " & _
            '    "receipt_master_new.receipt_customer_bank=customer_bank_master.other_bank_master_code) INNER JOIN dbo.view_account view_account_1 ON " & _
            '    "(receipt_master_new.receipt_cashbank_code=view_account_1.code) AND (receipt_master_new.receipt_div_id=view_account_1.div_code)) " & _
            '    "LEFT OUTER JOIN dbo.UserMaster UserMaster ON receipt_master_new.adduser=UserMaster.UserCode) LEFT OUTER JOIN dbo.acctmast acctmast ON " & _
            '    "(view_account.controlacctcode=acctmast.acctcode) AND (view_account.div_code=acctmast.div_code) where receipt_master_new.tran_id ='" + tranid + "' " & _
            '    "and receipt_master_new.receipt_div_id='" + divcode + "' ORDER BY receipt_master_new.tran_id, receipt_detail.tran_lineno"

            '    'LEFT OUTER JOIN dbo.costcenter_master costcenter_master ON receipt_detail.costcenter_code=costcenter_master.costcenter_code

            '    Using dad As New SqlDataAdapter(strSql, conn1)
            '        dad.Fill(receipt_master)
            '    End Using


            '    receiptmaster = receipt_master.Select("tran_id='" & tranid & "'")
            'End If
            Dim voccurr, curr As String

            curr = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)

            If PrntCliCurr = 1 Then
                'voccurr = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select top 1 isnull(receipt_currency_id,'') from receipt_Detail  where tran_id='" & tranid & "' and div_id='" & divcode & "' and receipt_acc_type<>'G' "), String)
                If receiptmaster.Length > 0 Then
                    voccurr = receiptmaster(0)("receipt_currency_id")
                End If
            Else
                voccurr = curr
            End If
            If voccurr = "" Then
                voccurr = curr
            End If

            Dim lsCtry As String = objutils.GetString(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=459")

            Dim decnum As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), String)
            Dim decno As String = "N" + decnum
            Dim coadd1 As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coadd1 from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim coadd2 As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coadd2 from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim copobox As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select copobox from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim cotel As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select cotel from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim cofax As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select cofax from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim coemail As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coemail from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim coweb As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coweb from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim TRNNo As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select TRNNo from Columbusmaster where div_code='" & divcode & "'"), String)

            Dim settle_details As New DataTable
            Dim conn As New SqlConnection
            conn = clsDBConnect.dbConnectionnew("strDBConnection")
            sqlcmd = New SqlCommand("sp_rpt_receipt_settle", conn)
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = tranid
            sqlcmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
            Using dad As New SqlDataAdapter
                dad.SelectCommand = sqlcmd
                dad.Fill(settle_details)
            End Using
            clsDBConnect.dbCommandClose(sqlcmd)
            clsDBConnect.dbConnectionClose(conn)
            Dim settle_detail() As System.Data.DataRow
            settle_detail = settle_details.Select("against_tran_id='" & tranid & "'")

            Dim str As String = ""
            Dim decimalword As String = Nothing
            Dim decimalInword As String = Nothing
            Dim fractionword As String = Nothing
            'Dim rdcredit As Decimal = 0
            Dim fraction As Decimal
            'Dim fraction As Decimal = Math.Round(Decimal.Parse(receiptmaster(0)("basecredit")), 2) - Math.Truncate(Math.Round(Decimal.Parse(receiptmaster(0)("basecredit")), 3))
            Dim recAmt As Decimal
            Dim convRate As Decimal  'Modified param 02/02/2020
            If receiptmaster.Length <> 0 Then
                If PrntCliCurr = 1 Then
                    'Sharfudeen 20/12/2023
                    '  convRate = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select receipt_currency_rate from receipt_master_new where tran_id='" & tranid & "' and tran_type='" & trantype & "' and receipt_div_id='" & divcode & "'"), String)
                    Str = "select d.receipt_currency_rate from receipt_master_new m,receipt_detail d where " & _
                   " m.tran_id = d.tran_id And m.tran_type = d.tran_type And m.div_id = d.div_id " & _
                    "and m.tran_id='" & tranid & "' and m.tran_type='" & trantype & "' and m.receipt_div_id='" & divcode & "' " & _
                    " and receipt_acc_type in ('C','S') "

                    convRate = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), Str), String)

                    recAmt = Math.Round(receiptmaster(0)("receipt_amount") / convRate, Convert.ToInt32(decnum))
                    decimalword = Math.Truncate(Decimal.Parse(recAmt)).ToString()
                    decimalInword = AmountInWords(decimalword)
                    fraction = Math.Round(Decimal.Parse(recAmt), Convert.ToInt32(decnum), MidpointRounding.AwayFromZero) Mod 1
                Else
                    'Dim TotalBaseCredit As Decimal = receipt_master.AsEnumerable().Where(Function(row) row.Field(Of String)("receipt_acc_type") <> "G").Sum(Function(row) row.Field(Of Decimal)("rdcredit"))
                    'Dim TotalBaseDebit As Decimal = receipt_master.AsEnumerable().Where(Function(row) row.Field(Of String)("receipt_acc_type") <> "G").Sum(Function(row) row.Field(Of Decimal)("rddebit"))
                    'rdcredit = TotalBaseCredit - TotalBaseDebit  

                    decimalword = Math.Truncate(Decimal.Parse(receiptmaster(0)("receipt_amount"))).ToString()
                    decimalInword = AmountInWords(decimalword)
                    fraction = Math.Round(Decimal.Parse(receiptmaster(0)("receipt_amount")), Convert.ToInt32(decnum), MidpointRounding.AwayFromZero) Mod 1
                    convRate = 1
                End If
            End If
            'Modified Param 15/11/2018

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
                While fractionStr.Length < decnum
                    fractionStr = fractionStr + "0"
                End While
                Dim coin As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select currcoin from currmast where currcode= '" & voccurr.Trim & "'"), String)
                fractionword = "AND " & coin & "  " & AmountInWords(fractionStr) + "  " + "ONLY"
                'If decnum.Equals("2") Then
                '    fractionword = (fraction.ToString()).Split(New Char() {"."})(1) & "/100" + " " + "FILS ONLY"
                'ElseIf decnum.Equals("3") AndAlso trantype.Equals("RV") Then
                '    fractionword = (fraction.ToString()).Split(New Char() {"."})(1) & "/1000" + " " + "BAIZA ONLY"
                'ElseIf decnum.Equals("3") AndAlso trantype.Equals("CRV") Then
                '    fractionword = (fraction.ToString()).Split(New Char() {"."})(1) & "/1000" + " " + "FILS ONLY"
                'End If
            Else
                decimalInword = UCase(decimalInword) + "  " + "ONLY"
                fractionword = ""
            End If

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
                phrase.Add(New Chunk(coadd1 & Environment.NewLine, normalfontgray))
                phrase.Add(New Chunk(coadd2 & Environment.NewLine, normalfontgray))
                phrase.Add(New Chunk("PO Box" & Space(3) & ":" & Space(6) & copobox & Environment.NewLine, normalfontgray))
                'phrase.Add(New Chunk("Email :" & Space(6) & coemail & Environment.NewLine, normalfont))
                phrase.Add(New Chunk("Tel" & Space(10) & ":" & Space(6) & cotel & Environment.NewLine, normalfontgray))
                'phrase.Add(New Chunk("Fax    :" & Space(6) & cofax & Environment.NewLine, normalfont))
                phrase.Add(New Chunk("Web" & Space(8) & ":" & Space(6) & coweb & Environment.NewLine, normalfontgray))
                'changed by Christo on 02/01/2019
                'Dim lsCtry As String = objutils.GetString(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=459")
                'If lsCtry.Trim.ToUpper = "OM" Then
                '    ' TRN not required for Oman 
                'Else
                phrase.Add(New Chunk("TRN" & Space(8) & ":" & Space(6) & TRNNo & Environment.NewLine, normalfontgray))
                'End If
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
                If trantype.Equals("RV") Then
                    rptreportname = "Receipt Voucher"
                    phrase.Add(New Chunk(rptreportname, headerfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                    cell.PaddingBottom = 3.0F

                End If

                tblTitle.AddCell(cell)
                tblTitle.SpacingBefore = 8
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
                cell.PaddingBottom = 3
                FooterTable.SpacingBefore = 12.0F
                FooterTable.AddCell(cell)
                FooterTable.Complete = True


                'add common header and footer part to every page
                writer.PageEvent = New ClsHeaderFooter(tableheader, tblTitle, FooterTable, Nothing, "Voucher")
                document.Open()

                Dim collType As String

                Dim cheque As Boolean = False
                Dim ccredit As Boolean = False
                Dim cash As Boolean = False
                Dim banktransfer As Boolean = False

                Dim cheque_number As String = receiptmaster(0)("receipt_cheque_number").ToString
                Dim bank_type As String = receiptmaster(0)("receipt_cashbank_type").ToString
                Dim bank_transfer As String = receiptmaster(0)("other_bank_master_des").ToString

                If bank_type.Equals("B") AndAlso (cheque_number.ToUpper().IndexOf("CC") <> 1 AndAlso cheque_number.ToUpper().IndexOf("CASH") <> 1) Then
                    cheque = True
                    collType = "Cheque"

                End If
                If bank_type.Equals("B") AndAlso (cheque_number.ToUpper().IndexOf("CC") = 1) Then
                    ccredit = True
                    collType = "C.Card"
                End If
                If bank_type.Equals("C") Then
                    cash = True
                    collType = "Cash"
                End If

                If UCase(bank_transfer) = "BANK TRANSFER" Then
                    banktransfer = True
                    cheque = False
                    cash = False
                    ccredit = False
                    collType = "Bank Transfer"
                End If

                Dim tblcommon As PdfPTable = New PdfPTable(1)
                tblcommon.SetWidths(New Single() {1.0F})
                tblcommon.TotalWidth = documentWidth
                tblcommon.LockedWidth = True
                'Dim tbl As PdfPTable = New PdfPTable(7)
                'tbl.SetWidths(New Single() {0.15F, 0.05F, 0.15F, 0.05F, 0.15F, 0.05F, 0.4F})
                Dim tbl As PdfPTable = New PdfPTable(9)
                tbl.SetWidths(New Single() {0.3F, 0.05F, 0.15F, 0.05F, 0.15F, 0.05F, 0.3F, 0.05F, 0.3F})
                If receiptmaster.Length <> 0 Then
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Voucher No." & Space(10), normalfontbold))
                    phrase.Add(New Chunk(receiptmaster(0)("tran_id").ToString() & Space(25), normalfontbold))
                    phrase.Add(New Chunk("Date" & Space(10), normalfontbold))
                    phrase.Add(New Chunk(Format(Convert.ToDateTime(receiptmaster(0)("receipt_date").ToString()), "dd-MM-yyyy") & Space(25), normalfont))

                    If PrntCliCurr = 1 Then
                        phrase.Add(New Chunk("Amount(" & voccurr & ")" & Space(10), normalfontbold))
                        phrase.Add(New Chunk(Decimal.Parse(recAmt).ToString(decno) & Environment.NewLine & vbLf & vbLf, normalfont))
                    Else
                        phrase.Add(New Chunk("Amount(" & voccurr & ")" & Space(10), normalfontbold))
                        phrase.Add(New Chunk(Decimal.Parse(receiptmaster(0)("receipt_Amount")).ToString(decno) & Environment.NewLine & vbLf & vbLf, normalfont))
                    End If
                    ' If trantype.Equals("RV") Then
                    phrase.Add(New Chunk("M.RV No." & Space(15), normalfontbold))
                    phrase.Add(New Chunk(receiptmaster(0)("receipt_mrv").ToString() & Environment.NewLine & vbLf & vbLf, normalfont))
                    phrase.Add(New Chunk("Received From" & Space(5), normalfontbold))
                    phrase.Add(New Chunk(receiptmaster(0)("receipt_received_from").ToString() & vbLf, normalfont))
                    phrase.Add(New Chunk("     " & Space(20) & "------------------------------------------------------------------------------------------------------------------" & Environment.NewLine & vbLf))
                    'Else
                    '    phrase.Add(New Chunk("Paid For" & Space(16), normalfontbold))
                    '    phrase.Add(New Chunk(receiptmaster(0)("receipt_received_from").ToString() & vbLf & vbLf & vbLf, normalfont))
                    '  End If
                End If
                phrase.Add(New Chunk("Amount" & Space(17), normalfontbold))
                phrase.Add(New Chunk(voccurr & "  ", normalfont))
                phrase.Add(New Chunk(decimalInword.ToUpper() & Space(1) & fractionword.ToUpper() & Environment.NewLine & vbLf & vbLf, normalfont))

                'If trantype.Equals("CRV") Then
                ' phrase.Add(New Chunk("Received By" & Space(10) & "----------------------------------------------------" & Space(10) & "Signature" & Space(10) & "----------------------------------------------------" & vbLf & vbLf, normalfontbold))
                ' End If
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                cell.PaddingTop = 10.0F
                'cell.Colspan = 7
                cell.Colspan = 9
                tbl.AddCell(cell)
                '  If trantype.Equals("RV") Then
                phrase = New Phrase()
                phrase.Add(New Chunk("Collection", normalfontbold))
                phrase.Add(New Chunk("Type", normalfontbold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                'cell.Width = 30.0F
                cell.Colspan = 1
                tbl.AddCell(cell)



                cell = IIf(cash, ImageCell("~/Images/checked.png", 35.0F, PdfPCell.ALIGN_LEFT), ImageCell("~/Images/uncheck.png", 35.0F, PdfPCell.ALIGN_LEFT))
                cell.Colspan = 1
                cell.PaddingTop = 3
                cell.PaddingLeft = 3
                tbl.AddCell(cell)
                phrase = New Phrase()
                phrase.Add(New Chunk("Cash", normalfontbold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                cell.Colspan = 1
                tbl.AddCell(cell)

                cell = IIf(ccredit, ImageCell("~/Images/checked.png", 35.0F, PdfPCell.ALIGN_LEFT), ImageCell("~/Images/uncheck.png", 35.0F, PdfPCell.ALIGN_LEFT))
                cell.PaddingTop = 3
                cell.Colspan = 1
                tbl.AddCell(cell)
                phrase = New Phrase()
                phrase.Add(New Chunk("C.Card", normalfontbold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                cell.Colspan = 1
                tbl.AddCell(cell)

                cell = IIf(cheque, ImageCell("~/Images/checked.png", 35.0F, PdfPCell.ALIGN_LEFT), ImageCell("~/Images/uncheck.png", 35.0F, PdfPCell.ALIGN_LEFT))
                cell.Colspan = 1
                cell.PaddingTop = 2
                tbl.AddCell(cell)
                phrase = New Phrase()
                phrase.Add(New Chunk("Cheque" & Environment.NewLine & vbLf & vbLf, normalfontbold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                cell.Colspan = 1
                tbl.AddCell(cell)

                '''' Bank Transfer - christo - 22/12/18
                cell = IIf(banktransfer, ImageCell("~/Images/checked.png", 35.0F, PdfPCell.ALIGN_LEFT), ImageCell("~/Images/uncheck.png", 35.0F, PdfPCell.ALIGN_LEFT))
                cell.Colspan = 1
                cell.PaddingTop = 2
                tbl.AddCell(cell)
                phrase = New Phrase()
                phrase.Add(New Chunk("Bank Transfer" & Environment.NewLine & vbLf & vbLf, normalfontbold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                cell.Colspan = 1
                tbl.AddCell(cell)
                If receiptmaster.Length <> 0 Then
                    '''''''''''''''''''''''''''''''''''''''
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Cheque No." & Space(11), normalfontbold))
                    phrase.Add(New Chunk(receiptmaster(0)("receipt_cheque_number").ToString() & Space(50), normalfont))
                    phrase.Add(New Chunk("Bank/Branch" & Space(9), normalfontbold))
                    phrase.Add(New Chunk(receiptmaster(0)("other_bank_master_des").ToString() & vbLf, normalfont))
                    phrase.Add(New Chunk("          " & Space(15) & "---------------------------" & Space(25) & "         " & "------------------------------" & Environment.NewLine & vbLf))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.PaddingBottom = 2.0F
                    'cell.Colspan = 7
                    cell.Colspan = 9

                    tbl.AddCell(cell)
                End If
                phrase = New Phrase()
                phrase.Add(New Chunk("Description", normalfontbold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                'cell.Colspan = 1
                cell.Colspan = 2
                cell.Border = Rectangle.NO_BORDER
                tbl.AddCell(cell)
                phrase = New Phrase()
                phrase.Add(New Chunk(receiptmaster(0)("receipt_narration").ToString() & vbLf & vbLf & vbLf & vbLf & vbLf & vbLf, normalfont))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                cell.Border = Rectangle.NO_BORDER
                'cell.Colspan = 6
                cell.Colspan = 7

                tbl.AddCell(cell)
                phrase = New Phrase()
                phrase.Add(New Chunk("Prepared By:" & Space(10) & Space(10) & Space(10), normalfontbold))
                'phrase.Add(New Chunk("Checked By:" & Space(15) & Space(20) & Space(20), normalfontbold))
                'changed by Christo on 02/01/2019
                'If lsCtry.Trim.ToUpper = "OM" Then
                '    phrase.Add(New Chunk("Director of Finance:" & Space(15) & Space(15), normalfontbold))
                '    phrase.Add(New Chunk("General Manager:" & Space(5) & Space(5) & Environment.NewLine & vbLf, normalfontbold))
                'Else
                phrase.Add(New Chunk("Checked By:" & Space(10) & Space(10) & Space(10), normalfontbold))
                phrase.Add(New Chunk("Approved By:" & Space(10) & Space(10) & Environment.NewLine & vbLf, normalfontbold))
                'End If

                phrase.Add(New Chunk(receiptmaster(0)("UserName").ToString() & Space(20) & Space(20), normalfont))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                cell.PaddingTop = 100.0F
                'cell.Colspan = 7
                cell.Colspan = 9

                tbl.AddCell(cell)

                If trantype.Equals("RV") Then
                    tblcommon.AddCell(tbl)
                    tblcommon.SpacingBefore = 0
                    tblcommon.Complete = True
                    document.Add(tblcommon)
                End If

                'common params
                Dim arrow3() As String = Nothing
                Dim receipt() As String = Nothing
                Dim sumtotal() As String = Nothing
                Dim acctname As String = Nothing
                Dim costcenter_code As String = Nothing
                Dim rdebit As Decimal = rdebit + Decimal.Parse(receiptmaster(0)("basecredit"))
                Dim rcredit As Decimal = rcredit + Decimal.Parse(receiptmaster(0)("basedebit"))
                Dim debit As String = IIf(Decimal.Parse(receiptmaster(0)("basedebit")) = 0.0, "-", Decimal.Parse(receiptmaster(0)("basedebit")).ToString(decno))
                Dim credit As String = IIf(Decimal.Parse(receiptmaster(0)("basecredit")) = 0.0, "-", Decimal.Parse(receiptmaster(0)("basecredit")).ToString(decno))

                '   arrow3 = {"Account No.", "Account Name / Description ", "Debit(" + curr + ")", "Credit(" + curr + ")", receiptmaster(0)("receipt_cashbank_code").ToString(), receiptmaster(0)("dest").ToString() + Environment.NewLine + Environment.NewLine + receiptmaster(0)("receipt_narration").ToString(), credit, debit}


                arrow3 = {"S.No.", "Account Code.", "Account Head", "Narration", "Amount(" & voccurr & ")"}
                If trantype.Equals("CRV") Then
                    cashReceiptVoucher(document, decno, receiptmaster, settle_detail, tbl, arrow3, rdebit, rcredit, normalfont, normalfontbold, documentWidth)
                ElseIf PrntSec = 1 AndAlso (trantype.Equals("RV") Or trantype.Equals("CV")) Then


                    document.NewPage()

                    Dim tbl3 As PdfPTable = New PdfPTable(5)

                    tbl3.SetWidths(New Single() {0.07, 0.18F, 0.2F, 0.4F, 0.15F})

                    'Dim tblhead As PdfPTable = New PdfPTable(2)

                    'tblhead.SetWidths(New Single() {0.5F, 0.5F})
                    'tblhead.TotalWidth = documentWidth
                    'tblhead.SplitRows = False
                    'tblhead.Complete = False
                    'tblhead.LockedWidth = True

                    tbl3.TotalWidth = documentWidth
                    tbl3.SplitRows = False
                    tbl3.Complete = False
                    tbl3.LockedWidth = True

                    phrase = New Phrase()
                    phrase.Add(New Chunk("RV No." & Space(10), normalfontbold))
                    phrase.Add(New Chunk(receiptmaster(0)("tran_id").ToString() & Space(60), normalfont))


                    phrase.Add(New Chunk(Space(60) & "Date" & Space(10), normalfontbold))
                    phrase.Add(New Chunk(Format(Convert.ToDateTime(receiptmaster(0)("receipt_date").ToString()), "dd-MM-yyyy") & Environment.NewLine & vbLf, normalfont))

                    phrase.Add(New Chunk("Received From" & Space(10), normalfontbold))
                    phrase.Add(New Chunk(receiptmaster(0)("receipt_received_from").ToString() & Environment.NewLine & vbLf, normalfont))




                    phrase.Add(New Chunk(collType & Space(10), normalfontbold))

                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.PaddingTop = 3.0F
                    cell.PaddingBottom = 3.0F
                    cell.Colspan = 5
                    cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                    tbl3.AddCell(cell)


                    phrase = New Phrase()
                    phrase.Add(New Chunk(" " & vbLf, footerdfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.PaddingBottom = 2.0F
                    cell.Colspan = 5
                    tbl3.AddCell(cell)

                    If settle_detail.Length > 0 Then
                        ' Sub Table-Adjust Bill Detail
                        Dim tbl4 As PdfPTable = New PdfPTable(8)
                        tbl4.SetWidths(New Single() {0.11F, 0.1F, 0.11F, 0.15F, 0.12F, 0.11F, 0.15F, 0.15F})
                        tbl4.TotalWidth = documentWidth

                        phrase = New Phrase()
                        phrase.Add(New Chunk("Settlement Details" & vbLf & vbLf, footerdfont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        cell.PaddingBottom = 3.0F
                        cell.Colspan = 8
                        cell.Border = Rectangle.BOTTOM_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER
                        tbl4.AddCell(cell)

                        Dim arrData2() As String
                        arrData2 = {"Date", "Voucher Type", "Voucher No.", settle_detail(0)("field2").ToString(), settle_detail(0)("field3").ToString(), "Due Date", "Amount Adjusted"}

                        For i = 0 To arrData2.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrData2(i), normalfontbold))
                            If i = 6 Then
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                cell.Colspan = 2
                            Else
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                            End If
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = New BaseColor(215, 215, 215)
                            If i <= 5 Then
                                cell.BorderWidthBottom = 0
                            End If
                            tbl4.AddCell(cell)
                        Next

                        arrData2 = {"", "", "", "", "", "", settle_detail(0)("receipt_currency_id").ToString(), curr}
                        Dim saleDecno As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select nodigit from currmast where currcode= '" & settle_detail(0)("receipt_currency_id").ToString() & "'"), String)
                        For i = 0 To arrData2.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrData2(i), normalfontbold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                            cell.PaddingBottom = 4.0F
                            cell.Colspan = 1
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = New BaseColor(215, 215, 215)
                            If i <= 5 Then
                                cell.BorderWidthTop = 0
                            End If
                            tbl4.AddCell(cell)
                        Next
                        Dim totalodebit As Decimal
                        Dim totalbdebit As Decimal
                        'Table Data
                        For i = 0 To settle_detail.Length - 1
                            arrData2 = {settle_detail(i)("tran_date"), settle_detail(i)("tran_type"), settle_detail(i)("tran_id"), settle_detail(i)("open_field2"), settle_detail(i)("open_field3"), settle_detail(i)("open_due_date"), Decimal.Parse(settle_detail(i)("open_credit")).ToString("N" + saleDecno), Decimal.Parse(settle_detail(i)("base_credit")).ToString(decno)}
                            totalodebit = totalodebit + Decimal.Parse(settle_detail(i)("open_credit"))
                            totalbdebit = totalbdebit + Decimal.Parse(settle_detail(i)("base_credit"))
                            For j = 0 To arrData2.Length - 1
                                phrase = New Phrase()
                                phrase.Add(New Chunk(arrData2(j), normalfont))
                                If j > 5 Then
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT

                                Else
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                End If
                                cell.PaddingBottom = 4.0F
                                cell.PaddingTop = 1.0F
                                tbl4.AddCell(cell)
                            Next
                        Next

                        arrData2 = {"", "", "", "", "", "Total", totalodebit.ToString("N" + saleDecno), totalbdebit.ToString(decno)}

                        For i = 0 To arrData2.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrData2(i), normalfontbold))

                            If i > 5 Then
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT

                            Else
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                            End If
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            tbl4.AddCell(cell)
                        Next
                        document.Add(tbl4)
                    End If


                    For i = 0 To arrow3.Length - 1
                        phrase = New Phrase()

                        phrase.Add(New Chunk(arrow3(i), normalfontbold))

                        If i = 4 Then
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                        Else
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        End If


                        '  cell.BackgroundColor = Header
                        cell.PaddingBottom = 3.0F


                        tbl3.AddCell(cell)
                    Next
                    Dim sumamt As Decimal
                    Dim count As Integer = 0
                    For i = 0 To receiptmaster.Length - 1
                        Dim excludeRow As String = ""
                        If PrntCliCurr = 1 And receiptmaster(i)("receipt_acc_type") = "G" Then
                            excludeRow = Convert.ToString(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(acctCode,'') from AcctAutomatCalculate where div_code='" & divcode & "' and acctcode='" & receiptmaster(i)("receipt_acc_code") & "'"))
                        End If
                        If (PrntCliCurr = 1 And excludeRow = "") Or PrntCliCurr = 0 Then
                            count += 1
                            acctname = IIf(Not (TypeOf receiptmaster(i)("acctname") Is DBNull), receiptmaster(i)("acctname").ToString(), receiptmaster(i)("costcenter_name").ToString())
                            costcenter_code = IIf(Not (TypeOf receiptmaster(i)("costcenter_code") Is DBNull), receiptmaster(i)("costcenter_code").ToString(), receiptmaster(i)("controlacctcode").ToString())
                            'If PrntCliCurr = 1 Then
                            '    receipt = {receiptmaster(i)("receipt_acc_code").ToString() + Environment.NewLine + Environment.NewLine + costcenter_code, receiptmaster(i)("des").ToString() + Environment.NewLine + Environment.NewLine + acctname + Environment.NewLine + Environment.NewLine + receiptmaster(i)("narration").ToString(), IIf(Decimal.Parse(receiptmaster(i)("receipt_debit")) = 0.0, "-", Decimal.Parse(receiptmaster(i)("receipt_debit")).ToString(decno)), IIf(Decimal.Parse(receiptmaster(i)("receipt_credit")) = 0.0, "-", Decimal.Parse(receiptmaster(i)("receipt_credit")).ToString(decno))}
                            '    rdebit = rdebit + Decimal.Parse(receiptmaster(i)("receipt_debit"))
                            '    rcredit = rcredit + Decimal.Parse(receiptmaster(i)("receipt_credit"))

                            'Else
                            Dim acctNo As String = ""
                            Dim acctDesc As String = ""
                            Dim narration As String = ""
                            Dim amt As Decimal


                            'If (receiptmaster(i)("receipt_acc_type") = "G") Then
                            '    acctNo = receiptmaster(i)("receipt_acc_code").ToString()
                            '    acctDesc = receiptmaster(i)("des").ToString()
                            '    narration = receiptmaster(i)("narration").ToString()
                            'Else
                            '    acctNo = receiptmaster(i)("receipt_acc_code").ToString() + Environment.NewLine + Environment.NewLine + costcenter_code
                            '    acctDesc = receiptmaster(i)("des").ToString() + Environment.NewLine + Environment.NewLine + acctname
                            '    narration = receiptmaster(i)("narration").ToString()
                            'End If

                            acctNo = receiptmaster(i)("receipt_acc_code").ToString()
                            acctDesc = receiptmaster(i)("des").ToString()
                            narration = receiptmaster(i)("narration").ToString()

                            amt = Decimal.Parse(receiptmaster(i)("rdcredit")) - Decimal.Parse(receiptmaster(i)("rddebit"))
                            If PrntCliCurr = 1 Then
                                amt = Math.Round(amt / convRate, Convert.ToInt32(decnum))
                            End If
                            'receipt = {acctNo, acctDesc, IIf(Decimal.Parse(receiptmaster(i)("rddebit")) = 0.0, "-", Decimal.Parse(receiptmaster(i)("rddebit")).ToString(decno)), IIf(Decimal.Parse(receiptmaster(i)("rdcredit")) = 0.0, "-", Decimal.Parse(receiptmaster(i)("rdcredit")).ToString(decno))}
                            receipt = {count.ToString(), acctNo, acctDesc, narration, amt.ToString(decno)}

                            sumamt = sumamt + amt


                            ' End If
                            For j = 0 To receipt.Length - 1
                                phrase = New Phrase()
                                phrase.Add(New Chunk(receipt(j), normalfont))

                                If j = 4 Then
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                Else
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                End If
                                cell.PaddingBottom = 3.0F

                                tbl3.AddCell(cell)
                            Next
                        End If
                    Next

                    Dim amtinword As String
                    Dim intergerpart As Integer
                    Dim fractionpart As Decimal
                    Dim strArr() As String
                    'intergerpart = Int(Math.Abs(sumamt))
                    'fractionpart = Math.Abs(sumamt) - intergerpart
                    '  strArr = sumamt.ToString().Split(".")

                    'fractionpart = Decimal.Parse(strArr(1))
                    'amtinword = AmountInWords(intergerpart) & " " & AmountInWords(fractionpart)
                    Dim Receiptamt As Decimal = Math.Abs(sumamt)
                    Dim decWord As Decimal = Math.Truncate(Decimal.Parse(Receiptamt)).ToString()
                    decimalInword = ""
                    decimalInword = AmountInWords(decWord)
                    fraction = 0
                    fraction = Math.Round(Decimal.Parse(Receiptamt), Convert.ToInt32(decnum), MidpointRounding.AwayFromZero) Mod 1
                    If fraction > 0 Then
                        Dim arrFraction As String() = fraction.ToString.Split(".")
                        If arrFraction.Length = 2 Then
                            fraction = arrFraction(1)
                        Else
                            fraction = 0
                        End If
                    End If
                    fractionword = ""
                    If fraction > 0 Then
                        Dim fractionStr As String = fraction.ToString()
                        While fractionStr.Length < decnum
                            fractionStr = fractionStr + "0"
                        End While
                        Dim currcoin As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select currcoin from currmast where currcode= '" & voccurr.Trim & "'"), String)
                        fractionword = "And " & currcoin & " " & AmountInWords(fractionStr) + " " + "Only"
                    Else
                        decimalInword = decimalInword + "  " + "Only"
                        fractionword = ""
                    End If

                    sumtotal = {"The sum of" & " " & voccurr & " " & decimalInword & " " & fractionword, sumamt.ToString(decno)}

                    For i = 0 To 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(sumtotal(i), normalfontbold))
                        If i = 0 Then
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.Colspan = 4
                        Else
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.Colspan = 1
                        End If

                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT

                        cell.PaddingBottom = 5.0F

                        tbl3.AddCell(cell)
                    Next

                    phrase = New Phrase()

                    'Modified by Priyanka 23/12/2019
                    'If lsCtry.Trim.ToUpper = "OM" Then
                    '    phrase.Add(New Chunk("Director of Finance:" & Space(15) & Space(15) & Space(10), normalfontbold))
                    '    phrase.Add(New Chunk("General Manager:" & Space(15) & Space(5), normalfontbold))
                    'Else
                    '    phrase.Add(New Chunk("Prepared By:" & Space(10) & Space(10) & Space(10), normalfontbold))
                    '    phrase.Add(New Chunk("Checked By:" & Space(10) & Space(10) & Space(10), normalfontbold))
                    'End If

                    phrase.Add(New Chunk("Cashier" & Space(60) & Space(60), footerdfont))
                    phrase.Add(New Chunk("Accountant", footerdfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.PaddingBottom = 3.0F
                    cell.PaddingTop = 30.0F
                    cell.Colspan = 5
                    tbl3.AddCell(cell)
                    tbl3.SpacingBefore = 0
                    tbl3.Complete = True
                    'tablecommon1.AddCell(tbl3)
                    'tablecommon1.SpacingBefore = 0
                    'tablecommon1.Complete = True
                    'document.Add(tablecommon1)
                    document.Add(tbl3)
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

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#Region "GenerateReport CV"
    Public Sub GenerateReportCV(ByVal trantype As String, ByVal tranid As String, ByVal divcode As String, ByVal CashBankType As String, ByVal PrntSec As Integer, ByVal PrntCliCurr As Integer, ByVal PrinDocTitle As String, ByRef bytes() As Byte, ByVal printMode As String)

        Try
            Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 30.0F, 35.0F)
            Dim documentWidth As Single
            documentWidth = 550.0F
            Dim rptreportname As String = Nothing
            Dim receipt_master As New DataTable
            Dim conn1 As New SqlConnection
            conn1 = clsDBConnect.dbConnectionnew("strDBConnection")
            Dim strSql As String
            Dim sqlcmd As New SqlCommand("sp_rpt_receipt", conn1)
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = tranid
            sqlcmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
            sqlcmd.Parameters.Add(New SqlParameter("@prtClientcurr", SqlDbType.Int)).Value = PrntCliCurr
            sqlcmd.Parameters.Add(New SqlParameter("@trantype", SqlDbType.VarChar, 20)).Value = trantype
            Using dad As New SqlDataAdapter
                dad.SelectCommand = sqlcmd
                dad.Fill(receipt_master)
            End Using
            clsDBConnect.dbCommandClose(sqlcmd)
            clsDBConnect.dbConnectionClose(conn1)
            Dim receiptmaster() As System.Data.DataRow
            receiptmaster = receipt_master.Select("tran_id='" & tranid & "'")


            Dim voccurr, curr As String
            Dim convRate As Decimal
            curr = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)

            If receiptmaster.Length > 0 Then
                voccurr = receiptmaster(0)("receipt_currency_id")
                convRate = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select receipt_currency_rate from receipt_master_new where tran_id='" & tranid & "' and receipt_div_id='" & divcode & "'"), String)
            End If
            If voccurr = "" Then
                voccurr = curr
                convRate = 1
            End If

            Dim decnum As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), String)
            Dim decno As String = "N" + decnum
            Dim coadd1 As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coadd1 from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim coadd2 As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coadd2 from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim copobox As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select copobox from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim cotel As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select cotel from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim cofax As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select cofax from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim coemail As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coemail from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim coweb As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coweb from Columbusmaster where div_code='" & divcode & "'"), String)
            Dim TRNNo As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select TRNNo from Columbusmaster where div_code='" & divcode & "'"), String)

            Dim settle_details As New DataTable
            Dim conn As New SqlConnection
            conn = clsDBConnect.dbConnectionnew("strDBConnection")
            sqlcmd = New SqlCommand("sp_rpt_receipt_settle", conn)
            sqlcmd.CommandType = CommandType.StoredProcedure
            sqlcmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = tranid
            sqlcmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
            Using dad As New SqlDataAdapter
                dad.SelectCommand = sqlcmd
                dad.Fill(settle_details)
            End Using
            clsDBConnect.dbCommandClose(sqlcmd)
            clsDBConnect.dbConnectionClose(conn)
            Dim settle_detail() As System.Data.DataRow
            settle_detail = settle_details.Select("against_tran_id='" & tranid & "'")

            Dim decimalword As String = Nothing
            Dim decimalInword As String = Nothing
            Dim fractionword As String = Nothing
            Dim fraction As Decimal

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
                phrase.Add(New Chunk(copobox & Environment.NewLine, normalfont))
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
                rptreportname = "Contra Voucher"
                phrase.Add(New Chunk(rptreportname, headerfont))
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

                'common params
                Dim arrow3() As String = Nothing
                Dim receipt() As String = Nothing
                Dim sumtotal() As String = Nothing
                Dim acctname As String = Nothing
                Dim costcenter_code As String = Nothing
                Dim rdebit As Decimal = rdebit + Decimal.Parse(receiptmaster(0)("basecredit"))
                Dim rcredit As Decimal = rcredit + Decimal.Parse(receiptmaster(0)("basedebit"))
                Dim debit As String = IIf(Decimal.Parse(receiptmaster(0)("basedebit")) = 0.0, "-", Decimal.Parse(receiptmaster(0)("basedebit")).ToString(decno))
                Dim credit As String = IIf(Decimal.Parse(receiptmaster(0)("basecredit")) = 0.0, "-", Decimal.Parse(receiptmaster(0)("basecredit")).ToString(decno))

                arrow3 = {"Account No.", "Account Name", "Description", "Amount(" + voccurr + ")"}

                Dim tbl3 As PdfPTable = New PdfPTable(4)
                tbl3.SetWidths(New Single() {0.15F, 0.26F, 0.43F, 0.16F})
                tbl3.TotalWidth = documentWidth
                tbl3.SplitRows = False
                tbl3.Complete = False
                tbl3.LockedWidth = True
                phrase = New Phrase()
                phrase.Add(New Chunk("Voucher No." & Space(10), normalfontbold))
                phrase.Add(New Chunk(receiptmaster(0)("tran_id").ToString() & Space(25), normalfont))
                phrase.Add(New Chunk("Date" & Space(10), normalfontbold))
                phrase.Add(New Chunk(Format(Convert.ToDateTime(receiptmaster(0)("receipt_date").ToString()), "dd-MM-yyyy") & Space(25), normalfont))
                phrase.Add(New Chunk("Amount(" & voccurr & ")" & Space(10), normalfontbold))
                Dim amt As Decimal = Math.Round(Decimal.Parse(receiptmaster(0)("basecredit")) / convRate, Convert.ToInt32(decnum))
                phrase.Add(New Chunk(amt.ToString(decno) & Environment.NewLine & vbLf, normalfont))

                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingTop = 3.0F
                cell.PaddingBottom = 3.0F
                cell.Colspan = 4
                cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                tbl3.AddCell(cell)

                'phrase = New Phrase()
                'phrase.Add(New Chunk("Settlement Details" & vbLf & vbLf, footerdfont))
                'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                'cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                'cell.PaddingBottom = 3.0F
                'cell.Colspan = 8
                'cell.Border = Rectangle.BOTTOM_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER
                'tbl3.AddCell(cell)

                'If settle_detail.Length > 0 Then
                '    ' Sub Table-Adjust Bill Detail
                '    Dim arrData2() As String
                '    arrData2 = {"Date", "Voucher Type", "Voucher No.", settle_detail(0)("field2").ToString(), settle_detail(0)("field3").ToString(), "Due Date", "Amount Adjusted"}

                '    For i = 0 To arrData2.Length - 1
                '        phrase = New Phrase()
                '        phrase.Add(New Chunk(arrData2(i), normalfontbold))
                '        If i = 6 Then
                '            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                '            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                '            cell.Colspan = 2
                '        Else
                '            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                '            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                '        End If
                '        cell.PaddingBottom = 4.0F
                '        cell.PaddingTop = 1.0F
                '        cell.BackgroundColor = New BaseColor(215, 215, 215)
                '        If i <= 5 Then
                '            cell.BorderWidthBottom = 0
                '        End If
                '        tbl3.AddCell(cell)
                '    Next

                '    arrData2 = {"", "", "", "", "", "", settle_detail(0)("receipt_currency_id").ToString(), curr}
                '    Dim saleDecno As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select nodigit from currmast where currcode= '" & settle_detail(0)("receipt_currency_id").ToString() & "'"), String)
                '    For i = 0 To arrData2.Length - 1
                '        phrase = New Phrase()
                '        phrase.Add(New Chunk(arrData2(i), normalfontbold))
                '        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                '        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                '        cell.PaddingBottom = 4.0F
                '        cell.Colspan = 1
                '        cell.PaddingTop = 1.0F
                '        cell.BackgroundColor = New BaseColor(215, 215, 215)
                '        If i <= 5 Then
                '            cell.BorderWidthTop = 0
                '        End If
                '        tbl3.AddCell(cell)
                '    Next
                '    Dim totalodebit As Decimal
                '    Dim totalbdebit As Decimal
                '    'Table Data
                '    For i = 0 To settle_detail.Length - 1
                '        arrData2 = {settle_detail(i)("tran_date"), settle_detail(i)("tran_type"), settle_detail(i)("tran_id"), settle_detail(i)("open_field2"), settle_detail(i)("open_field3"), settle_detail(i)("open_due_date"), Decimal.Parse(settle_detail(i)("open_credit")).ToString("N" + saleDecno), Decimal.Parse(settle_detail(i)("base_credit")).ToString(decno)}
                '        totalodebit = totalodebit + Decimal.Parse(settle_detail(i)("open_credit"))
                '        totalbdebit = totalbdebit + Decimal.Parse(settle_detail(i)("base_credit"))
                '        For j = 0 To arrData2.Length - 1
                '            phrase = New Phrase()
                '            phrase.Add(New Chunk(arrData2(j), normalfont))
                '            If j > 5 Then
                '                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                '                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT

                '            Else
                '                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                '                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                '            End If
                '            cell.PaddingBottom = 4.0F
                '            cell.PaddingTop = 1.0F
                '            tbl3.AddCell(cell)
                '        Next
                '    Next

                '    arrData2 = {"", "", "", "", "", "Total", totalodebit.ToString("N" + saleDecno), totalbdebit.ToString(decno)}

                '    For i = 0 To arrData2.Length - 1
                '        phrase = New Phrase()
                '        phrase.Add(New Chunk(arrData2(i), normalfontbold))

                '        If i > 5 Then
                '            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                '            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT

                '        Else
                '            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                '            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                '        End If
                '        cell.PaddingBottom = 4.0F
                '        cell.PaddingTop = 1.0F
                '        tbl3.AddCell(cell)
                '    Next
                'End If

                phrase = New Phrase()
                phrase.Add(New Chunk(" " & vbLf & vbLf, footerdfont))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 3.0F
                cell.Colspan = 4
                tbl3.AddCell(cell)

                For i = 0 To 3
                    phrase = New Phrase()
                    phrase.Add(New Chunk(arrow3(i), normalfontbold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.BackgroundColor = Header
                    cell.PaddingBottom = 3.0F
                    tbl3.AddCell(cell)
                Next

                Dim totalAmt As Decimal = 0
                For i = 0 To receiptmaster.Length - 1

                    acctname = IIf(Not (TypeOf receiptmaster(i)("acctname") Is DBNull), receiptmaster(i)("acctname").ToString(), receiptmaster(i)("costcenter_name").ToString())
                    costcenter_code = IIf(Not (TypeOf receiptmaster(i)("costcenter_code") Is DBNull), receiptmaster(i)("costcenter_code").ToString(), receiptmaster(i)("controlacctcode").ToString())

                    Dim acctNo As String = ""
                    Dim acctDesc As String = ""
                    Dim narration As String = ""
                    If (receiptmaster(i)("receipt_acc_type") = "G") Then
                        acctNo = receiptmaster(i)("receipt_acc_code").ToString()
                        acctDesc = receiptmaster(i)("des").ToString()
                        narration = receiptmaster(i)("narration").ToString()
                    Else
                        acctNo = receiptmaster(i)("receipt_acc_code").ToString()
                        acctDesc = receiptmaster(i)("des").ToString() + Environment.NewLine + Environment.NewLine + acctname
                        narration = receiptmaster(i)("narration").ToString()
                    End If
                    Dim cvAmt As Decimal = Math.Round((Decimal.Parse(receiptmaster(i)("rdcredit")) - Decimal.Parse(receiptmaster(i)("rddebit"))) / convRate, Convert.ToInt32(decnum))
                    totalAmt = totalAmt + cvAmt
                    receipt = {acctNo, acctDesc, narration, cvAmt.ToString(decno)}


                    For j = 0 To 3
                        phrase = New Phrase()
                        phrase.Add(New Chunk(receipt(j), normalfont))
                        If j = 3 Then
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                        Else
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        End If
                        cell.PaddingBottom = 3.0F
                        tbl3.AddCell(cell)
                    Next
                Next

                Dim VoucherAmt As Decimal = Math.Abs(totalAmt)
                Dim decWord As Decimal = Math.Truncate(Decimal.Parse(VoucherAmt)).ToString()
                decimalInword = ""
                decimalInword = AmountInWords(decWord)
                fraction = 0
                fraction = Math.Round(Decimal.Parse(VoucherAmt), Convert.ToInt32(decnum), MidpointRounding.AwayFromZero) Mod 1
                If fraction > 0 Then
                    Dim arrFraction As String() = fraction.ToString.Split(".")
                    If arrFraction.Length = 2 Then
                        fraction = arrFraction(1)
                    Else
                        fraction = 0
                    End If
                End If
                fractionword = ""
                If fraction > 0 Then
                    Dim fractionStr As String = fraction.ToString()
                    While fractionStr.Length < decnum
                        fractionStr = fractionStr + "0"
                    End While
                    Dim currcoin As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select currcoin from currmast where currcode= '" & voccurr.Trim & "'"), String)
                    fractionword = "And " & currcoin & " " & AmountInWords(fractionStr) + " " + "Only"
                Else
                    decimalInword = decimalInword + "  " + "Only"
                    fractionword = ""
                End If

                sumtotal = {"The sum of" & " " & voccurr & " " & decimalInword & " " & fractionword, totalAmt.ToString(decno)}

                For i = 0 To 1
                    phrase = New Phrase()
                    phrase.Add(New Chunk(sumtotal(i), normalfontbold))
                    If i = 0 Then
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    Else
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    End If
                    'cell.BackgroundColor = tback
                    cell.PaddingBottom = 3.0F
                    If i = 0 Then
                        cell.Colspan = 3
                    Else
                        cell.Colspan = 1
                    End If
                    tbl3.AddCell(cell)
                Next

                phrase = New Phrase()
                phrase.Add(New Chunk("Accountant:" & Environment.NewLine & vbLf, normalfontbold))
                phrase.Add(New Chunk(receiptmaster(0)("UserName").ToString(), normalfont))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                cell.PaddingBottom = 3.0F
                cell.PaddingTop = 25.0F
                cell.Colspan = 2
                cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                tbl3.AddCell(cell)

                phrase = New Phrase()
                phrase.Add(New Chunk("Approved By:", normalfontbold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                cell.PaddingBottom = 3.0F
                cell.PaddingTop = 25.0F
                cell.Colspan = 2
                cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                tbl3.AddCell(cell)
                tbl3.SpacingBefore = 0
                tbl3.Complete = True
                document.Add(tbl3)

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

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
#End Region


#Region "Private Shared Function cashReceiptVoucher(phrase As Phrase, align As Integer, Cols As Integer, celBorder As Boolean, Optional celBottomBorder As String = ""None"") As PdfPCell"
    Private Shared Function cashReceiptVoucher(ByRef document As Document, ByVal decno As String, ByVal receiptmaster() As DataRow, ByVal settle_detail() As DataRow, ByVal tbl As PdfPTable, ByVal arrow3() As String, ByVal rdebit As Decimal, ByVal rcredit As Decimal,
                                               ByVal normalfont As Font, ByVal normalfontbold As Font, ByVal width As Single)
        Dim subReport() As String = {"Payment Against Following Invoices -", "Invoice Date", "Invoice Type", "Invoice No.", "Due Date", "Amount Adjusted"}
        Dim arrData2() As String = Nothing
        Dim receipt() As String = Nothing
        Dim sumTotal() As String = Nothing
        Dim totalbdebit As Decimal
        Dim acctname As String = Nothing
        Dim costcenter_code As String = Nothing
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        If settle_detail.Length > 0 Then
            Dim subHeader As PdfPTable = New PdfPTable(5)
            subHeader.SetWidths(New Single() {0.2F, 0.15F, 0.2F, 0.15F, 0.3F})
            subHeader.TotalWidth = width
            subHeader.LockedWidth = True
            For i = 0 To subReport.Length - 1
                phrase = New Phrase()
                phrase.Add(New Chunk(subReport(i), normalfontbold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                If i = 0 Then
                    cell.BackgroundColor = New BaseColor(225, 196, 255)
                    cell.Colspan = 5
                End If
                cell.PaddingBottom = 3.0F
                subHeader.AddCell(cell)
            Next
            For i = 0 To settle_detail.Length - 1
                arrData2 = {settle_detail(i)("tran_date"), settle_detail(i)("tran_type"), settle_detail(i)("tran_id"), settle_detail(i)("open_due_date"), Decimal.Parse(settle_detail(i)("base_credit")).ToString(decno)}
                totalbdebit = totalbdebit + Decimal.Parse(settle_detail(i)("base_credit"))
                For j = 0 To arrData2.Length - 1
                    phrase = New Phrase()
                    phrase.Add(New Chunk(arrData2(j), normalfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.PaddingBottom = 4.0F
                    cell.PaddingTop = 1.0F
                    subHeader.AddCell(cell)
                Next
            Next

            arrData2 = {"Total", totalbdebit.ToString(decno)}

            For i = 0 To arrData2.Length - 1
                phrase = New Phrase()
                phrase.Add(New Chunk(arrData2(i), normalfontbold))

                If i = 0 Then
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Colspan = 4
                Else
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                End If
                cell.PaddingBottom = 4.0F
                cell.PaddingTop = 1.0F
                subHeader.AddCell(cell)
            Next

            subHeader.Complete = True
            subHeader.SpacingBefore = 15
            document.Add(subHeader)
            document.NewPage()
        End If
        Dim tablecommon1 As PdfPTable = New PdfPTable(1)
        tablecommon1.SetWidths(New Single() {1.0F})
        tablecommon1.TotalWidth = width
        tablecommon1.LockedWidth = True
        tablecommon1.AddCell(tbl)
        tablecommon1.Complete = True
        tablecommon1.SpacingBefore = 15.0F
        tablecommon1.SpacingAfter = 15.0F
        document.Add(tablecommon1)

        'Dim dataHeader As PdfPTable = New PdfPTable(4)
        'dataHeader.SetWidths(New Single() {0.15F, 0.55F, 0.15F, 0.15F})
        'dataHeader.TotalWidth = width
        'dataHeader.LockedWidth = True
        'For i = 0 To 3
        '    phrase = New Phrase()
        '    phrase.Add(New Chunk(arrow3(i), normalfontbold))
        '    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
        '    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
        '    cell.PaddingBottom = 4.0F
        '    cell.PaddingTop = 1.0F
        '    dataHeader.AddCell(cell)
        'Next

        'For i = 0 To receiptmaster.Length - 1
        '    rdebit = rdebit + Decimal.Parse(receiptmaster(i)("rddebit"))
        '    rcredit = rcredit + Decimal.Parse(receiptmaster(i)("rdcredit"))
        '    acctname = IIf(Not (TypeOf receiptmaster(i)("acctname") Is DBNull), receiptmaster(i)("acctname").ToString(), receiptmaster(i)("costcenter_name").ToString())
        '    costcenter_code = IIf(Not (TypeOf receiptmaster(i)("costcenter_code") Is DBNull), receiptmaster(i)("costcenter_code").ToString(), receiptmaster(i)("controlacctcode").ToString())
        '    receipt = {receiptmaster(i)("receipt_acc_code").ToString() + Environment.NewLine + Environment.NewLine + costcenter_code, receiptmaster(i)("des").ToString() + Environment.NewLine + Environment.NewLine + acctname + Environment.NewLine + Environment.NewLine + receiptmaster(i)("narration").ToString(), IIf(Decimal.Parse(receiptmaster(i)("rddebit")) = 0.0, "-", Decimal.Parse(receiptmaster(i)("rddebit")).ToString(decno)), IIf(Decimal.Parse(receiptmaster(i)("rdcredit")) = 0.0, "-", Decimal.Parse(receiptmaster(i)("rdcredit")).ToString(decno))}
        '    For j = 0 To 3
        '        phrase = New Phrase()
        '        phrase.Add(New Chunk(receipt(j), normalfont))
        '        If j = 2 Or j = 3 Then
        '            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
        '            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
        '        Else
        '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
        '            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
        '        End If
        '        cell.PaddingBottom = 3.0F
        '        dataHeader.AddCell(cell)
        '    Next
        'Next
        'For i = 4 To 7
        '    phrase = New Phrase()
        '    phrase.Add(New Chunk(arrow3(i), normalfont))
        '    If i = 6 Or i = 7 Then
        '        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
        '        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
        '    Else
        '        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
        '        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
        '    End If
        '    cell.PaddingBottom = 4.0F
        '    cell.PaddingTop = 1.0F
        '    dataHeader.AddCell(cell)
        'Next
        'sumTotal = {"Total", rdebit.ToString(decno), rcredit.ToString(decno)}
        'For i = 0 To 2
        '    phrase = New Phrase()
        '    phrase.Add(New Chunk(sumTotal(i), normalfontbold))
        '    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
        '    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
        '    cell.PaddingBottom = 4.0F
        '    cell.PaddingTop = 1.0F
        '    cell.BackgroundColor = New BaseColor(255, 188, 155)
        '    If i = 0 Then
        '        cell.Colspan = 2
        '    End If
        '    dataHeader.AddCell(cell)
        'Next
        'dataHeader.Complete = True
        'dataHeader.SpacingBefore = 15
        'document.Add(dataHeader)
        Dim footer As PdfPTable = New PdfPTable(1)
        footer.SetWidths(New Single() {1.0F})
        footer.TotalWidth = width
        footer.LockedWidth = True
        phrase = New Phrase()
        phrase.Add(New Chunk("Prepared By:" & Space(7) & Space(12) & Space(12), normalfontbold))
        phrase.Add(New Chunk("Accounts:" & Space(7) & Space(12) & Space(12), normalfontbold))
        phrase.Add(New Chunk("Checked By:" & Space(7) & Space(12) & Space(12), normalfontbold))
        phrase.Add(New Chunk("Approved By:" & Space(7) & Space(12) & Environment.NewLine & vbLf, normalfontbold))

        phrase.Add(New Chunk(receiptmaster(0)("UserName").ToString() & Space(12) & Space(12), normalfont))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
        cell.PaddingBottom = 3.0F
        cell.PaddingTop = 20.0F
        ' footer.AddCell(cell)
        ' document.Add(footer)
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
