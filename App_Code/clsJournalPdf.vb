Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq

Public Class clsJournalPdf

#Region "Global Variable"
    Dim objUtils As clsUtils
    Dim fontName As String = "Arial"
    Dim fontSize As Integer = 10
    Dim NormalFont As Font = FontFactory.GetFont(fontName, FontSize, Font.NORMAL, BaseColor.BLACK)
    Dim NormalFontBold As Font = FontFactory.GetFont(fontName, fontSize, Font.BOLD, BaseColor.BLACK)
    Dim SubTitleFontBold As Font = FontFactory.GetFont(fontName, 11, Font.BOLD, BaseColor.BLACK)
    Dim titleColor As BaseColor = New BaseColor(214, 214, 214)
    Dim TabletitleBColor As BaseColor = New BaseColor(225, 225, 225)
    Dim TableFooterBColor As BaseColor = New BaseColor(238, 238, 238)
#End Region

#Region "Public Sub JournalPrint(ByVal invoiceNo As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "")"
    Public Sub JournalPrint(ByVal invoiceNo As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "")
        Try
            Dim decPlaces As Integer
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As SqlCommand
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("select option_selected from reservation_parameters where param_id='509'", sqlConn)
            mySqlCmd.CommandType = CommandType.Text
            decPlaces = Convert.ToInt32(mySqlCmd.ExecuteScalar())
            mySqlCmd.Dispose()
            Dim convDecPlaces As Integer
            mySqlCmd = New SqlCommand("select option_selected from reservation_parameters where param_id='2038'", sqlConn)
            mySqlCmd.CommandType = CommandType.Text
            convDecPlaces = Convert.ToInt32(mySqlCmd.ExecuteScalar())
            mySqlCmd.Dispose()
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            mySqlCmd = New SqlCommand("sp_journal_print", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@invoiceNo", SqlDbType.VarChar, 20)).Value = invoiceNo
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            ds1 = ds
            Dim headerDt As DataTable = ds.Tables(0)
            Dim customerDt As DataTable = ds.Tables(1)
            Dim supplierDt As DataTable = ds.Tables(2)
            Dim adjReceiptDt As DataTable = ds.Tables(3)
            clsDBConnect.dbConnectionClose(sqlConn)
            Dim remainingPageSpace As Single
            If headerDt.Rows.Count > 0 Then
                Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 20.0F, 35.0F)
                Dim documentWidth As Single = 550.0F
                Dim TitleFont As Font = FontFactory.GetFont(fontName, 12, Font.NORMAL, BaseColor.BLACK)
                Dim TitleFontBold As Font = FontFactory.GetFont(fontName, 13, Font.BOLD, BaseColor.BLACK)
                Using memoryStream As New System.IO.MemoryStream()
                    Dim writer As PdfWriter
                    If printMode = "download" Then
                        writer = PdfWriter.GetInstance(document, memoryStream)
                    Else
                        Dim path As String = System.Web.HttpContext.Current.Server.MapPath("~\SavedReports\") + fileName
                        writer = PdfWriter.GetInstance(document, New FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                    End If
                    Dim phrase As Phrase = Nothing
                    Dim cell As PdfPCell = Nothing
                    Dim table As PdfPTable = Nothing
                    document.Open()
                    'Header Table
                    Dim headerDr As DataRow = headerDt.Rows(0)
                    table = New PdfPTable(2)
                    table.TotalWidth = documentWidth
                    table.LockedWidth = True
                    table.SetWidths(New Single() {0.75F, 0.25F})
                    table.Complete = False
                    table.SplitRows = False
                    Dim tblLogo As PdfPTable = New PdfPTable(2)
                    tblLogo.SetWidths(New Single() {0.3F, 0.7F})
                    'Company Logo
                    If (headerDr("divcode") = "01") Then
                        cell = ImageCell("~/images/logo.jpg", 70.0F, 70.0F, PdfPCell.ALIGN_LEFT)
                    Else
                        cell = ImageCell("~/images/logo.jpg", 68.0F, 65.0F, PdfPCell.ALIGN_LEFT)
                    End If
                    cell.Rowspan = 2
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.NO_BORDER
                    tblLogo.AddCell(cell)

                    'Company Name 
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("division_master_des")), TitleFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    tblLogo.AddCell(cell)

                    'Company Address
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("address1")) & vbLf, NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 3.0F
                    tblLogo.AddCell(cell)
                    cell = New PdfPCell(tblLogo)
                    cell.Border = Rectangle.NO_BORDER
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    table.AddCell(cell)
                    cell = New PdfPCell()
                    cell.Border = Rectangle.NO_BORDER
                    table.AddCell(cell)
                    table.Complete = True
                    document.Add(table)

                    Dim tblTitle As PdfPTable = New PdfPTable(1)
                    tblTitle.SetWidths(New Single() {1.0F})
                    tblTitle.TotalWidth = documentWidth
                    tblTitle.LockedWidth = True
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("printHeader")), TitleFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER Or Rectangle.LEFT_BORDER
                    cell.BackgroundColor = titleColor
                    cell.PaddingBottom = 3.0F
                    tblTitle.AddCell(cell)
                    tblTitle.SpacingBefore = 7
                    document.Add(tblTitle)

                    'PDF Title
                    Dim tblInvoice As PdfPTable = New PdfPTable(2)
                    tblInvoice.SetWidths(New Single() {0.5F, 0.5F})
                    tblInvoice.TotalWidth = documentWidth
                    tblInvoice.LockedWidth = True

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Invoice No. : " & Convert.ToString(headerDr("invoiceNo")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.NO_BORDER
                    cell.PaddingBottom = 3.0F
                    cell.PaddingLeft = 10.0F
                    tblInvoice.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Invoice Date : " & Convert.ToString(headerDr("invoiceDate")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.NO_BORDER
                    cell.PaddingBottom = 3.0F
                    cell.PaddingRight = 15.0F
                    tblInvoice.AddCell(cell)
                    tblInvoice.SpacingBefore = 7
                    document.Add(tblInvoice)

                    'Page wise Header
                    Dim tblpageHead As PdfPTable = New PdfPTable(2)
                    tblpageHead.SetWidths(New Single() {0.5F, 0.5F})
                    tblpageHead.TotalWidth = documentWidth
                    tblpageHead.LockedWidth = True

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Invoice No. : " & Convert.ToString(headerDr("invoiceNo")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingTop = 1.0F
                    cell.PaddingBottom = 4.0F
                    cell.PaddingLeft = 10.0F
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                    tblpageHead.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Invoice Date : " & Convert.ToString(headerDr("invoiceDate")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingTop = 1.0F
                    cell.PaddingBottom = 4.0F
                    cell.PaddingRight = 15.0F
                    cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                    tblpageHead.AddCell(cell)
                    tblpageHead.SpacingBefore = 3
                    writer.PageEvent = New clsJournalPdfPageEvents(tblpageHead, printMode)

                    'Customer posting
                    If customerDt.Rows.Count > 0 Then
                        Dim arrCustomer() As String = {"Account Name", "Currency", "Currency Rate", "Debit", "Credit", "Debit In " & Convert.ToString(headerDr("baseCurrCode")), "Credit In " & Convert.ToString(headerDr("baseCurrCode"))}
                        Dim tblCust As PdfPTable = New PdfPTable(7)
                        tblCust.SetWidths(New Single() {0.4F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F})
                        tblCust.TotalWidth = documentWidth
                        tblCust.LockedWidth = True
                        tblCust.HeaderRows = 2
                        tblCust.Complete = False
                        tblCust.SplitRows = False

                        phrase = New Phrase()
                        phrase.Add(New Chunk("Customer Posting", SubTitleFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 5.0F
                        cell.PaddingLeft = 0.0F
                        cell.Colspan = 7
                        tblCust.AddCell(cell)
                        For i = 0 To arrCustomer.GetUpperBound(0)
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(arrCustomer(i)), NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.BackgroundColor = TabletitleBColor
                            tblCust.AddCell(cell)
                        Next
                        For Each custDr As DataRow In customerDt.Rows
                            Dim AcctName As String = ""
                            If Convert.ToString(custDr("accountName")) <> "" Then
                                AcctName = Convert.ToString(custDr("accountName")) & "   " & "[ " & Convert.ToString(custDr("accountcode")) & " ]" & vbCrLf & vbCrLf
                            End If
                            Dim ctrlAcctName As String = ""
                            If custDr("controlaccountName") <> "" Then
                                ctrlAcctName = Convert.ToString(custDr("controlaccountName")) & "   " & "[ " & Convert.ToString(custDr("controlaccountcode")) & " ]"
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(AcctName, NormalFont))
                            phrase.Add(New Chunk(ctrlAcctName, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            tblCust.AddCell(cell)
                            
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(custDr("currcode")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.Rowspan = 2
                            tblCust.AddCell(cell)
                            Dim currencyRate As String = ""
                            If IsNumeric(custDr("acc_currency_rate")) Then
                                currencyRate = Convert.ToString(Math.Round(custDr("acc_currency_rate"), convDecPlaces))
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(currencyRate, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.Rowspan = 2
                            tblCust.AddCell(cell)
                            Dim debitval As String = ""
                            If IsNumeric(custDr("acc_debit")) Then
                                If custDr("acc_debit") > 0 Then
                                    debitval = Convert.ToString(Math.Round(custDr("acc_debit"), decPlaces))
                                End If
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(debitval, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.PaddingRight = 5.0F
                            cell.Rowspan = 2
                            tblCust.AddCell(cell)
                            Dim creditval As String = ""
                            If IsNumeric(custDr("acc_credit")) Then
                                If custDr("acc_credit") > 0 Then
                                    creditval = Convert.ToString(Math.Round(custDr("acc_credit"), decPlaces))
                                End If
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(creditval, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.PaddingRight = 5.0F
                            cell.Rowspan = 2
                            tblCust.AddCell(cell)
                            Dim debitvalBase As String = ""
                            If IsNumeric(custDr("acc_base_debit")) Then
                                If custDr("acc_base_debit") > 0 Then
                                    debitvalBase = Convert.ToString(Math.Round(custDr("acc_base_debit"), decPlaces))
                                End If
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(debitvalBase, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.PaddingRight = 5.0F
                            cell.Rowspan = 2
                            tblCust.AddCell(cell)
                            Dim creditvalBase As String = ""
                            If IsNumeric(custDr("acc_base_credit")) Then
                                If custDr("acc_base_credit") > 0 Then
                                    creditvalBase = Convert.ToString(Math.Round(custDr("acc_base_credit"), decPlaces))
                                End If
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(creditvalBase, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.PaddingRight = 5.0F
                            cell.Rowspan = 2
                            tblCust.AddCell(cell)
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(custDr("acc_narration")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            tblCust.AddCell(cell)
                        Next
                        Dim custStartRow = tblCust.Size - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk("Total", NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 7.0F
                        cell.Colspan = 3
                        cell.BackgroundColor = TableFooterBColor
                        tblCust.AddCell(cell)
                        Dim sumdebitval As String = Convert.ToString(Math.Round(customerDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("acc_debit")), decPlaces))
                        phrase = New Phrase()
                        phrase.Add(New Chunk(sumdebitval, NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 5.0F
                        cell.BackgroundColor = TableFooterBColor
                        tblCust.AddCell(cell)
                        Dim sumcreditval As String = Convert.ToString(Math.Round(customerDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("acc_credit")), decPlaces))
                        phrase = New Phrase()
                        phrase.Add(New Chunk(sumcreditval, NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 5.0F
                        cell.BackgroundColor = TableFooterBColor
                        tblCust.AddCell(cell)
                        Dim sumdebitvalBase As String = Convert.ToString(Math.Round(customerDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("acc_base_debit")), decPlaces))
                        phrase = New Phrase()
                        phrase.Add(New Chunk(sumdebitvalBase, NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 5.0F
                        cell.BackgroundColor = TableFooterBColor
                        tblCust.AddCell(cell)
                        Dim sumcreditvalBase As String = Convert.ToString(Math.Round(customerDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("acc_base_credit")), decPlaces))
                        phrase = New Phrase()
                        phrase.Add(New Chunk(sumcreditvalBase, NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 5.0F
                        cell.BackgroundColor = TableFooterBColor
                        tblCust.AddCell(cell)
                        tblCust.Complete = True
                        tblCust.KeepRowsTogether(custStartRow, tblCust.Size)
                        tblCust.SpacingBefore = 10
                        document.Add(tblCust)
                    End If

                    'Provisional Supplier Posting
                    If supplierDt.Rows.Count > 0 Then
                        Dim arrSupplier() As String = {"Account Name", "Currency", "Currency Rate", "Debit", "Credit", "Debit In " & Convert.ToString(headerDr("baseCurrCode")), "Credit In " & Convert.ToString(headerDr("baseCurrCode"))}
                        Dim tblSupplier As PdfPTable = New PdfPTable(7)
                        tblSupplier.SetWidths(New Single() {0.4F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F})
                        tblSupplier.TotalWidth = documentWidth
                        tblSupplier.LockedWidth = True
                        tblSupplier.HeaderRows = 2
                        tblSupplier.Complete = False
                        tblSupplier.SplitRows = False

                        phrase = New Phrase()
                        phrase.Add(New Chunk("Provisional Supplier Posting", SubTitleFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 5.0F
                        cell.PaddingLeft = 0.0F
                        cell.Colspan = 7
                        tblSupplier.AddCell(cell)
                        For i = 0 To arrSupplier.GetUpperBound(0)
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(arrSupplier(i)), NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.BackgroundColor = TabletitleBColor
                            tblSupplier.AddCell(cell)
                        Next
                        tblSupplier.KeepRowsTogether(0, 2)
                        For Each supplierDr As DataRow In supplierDt.Rows
                            Dim AcctName As String = ""
                            If Convert.ToString(supplierDr("accountName")) <> "" Then
                                AcctName = Convert.ToString(supplierDr("accountName")) & "   " & "[ " & Convert.ToString(supplierDr("accountcode")) & " ]" & vbCrLf & vbCrLf
                            End If
                            Dim ctrlAcctName As String = ""
                            If supplierDr("controlaccountName") <> "" Then
                                ctrlAcctName = Convert.ToString(supplierDr("controlaccountName")) & "   " & "[ " & Convert.ToString(supplierDr("controlaccountcode")) & " ]"
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(AcctName, NormalFont))
                            phrase.Add(New Chunk(ctrlAcctName, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            tblSupplier.AddCell(cell)
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(supplierDr("currcode")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.Rowspan = 2
                            tblSupplier.AddCell(cell)
                            Dim currencyRate As String = ""
                            If IsNumeric(supplierDr("acc_currency_rate")) Then
                                currencyRate = Convert.ToString(Math.Round(supplierDr("acc_currency_rate"), convDecPlaces))
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(currencyRate, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.Rowspan = 2
                            tblSupplier.AddCell(cell)
                            Dim debitval As String = ""
                            If IsNumeric(supplierDr("acc_debit")) Then
                                If supplierDr("acc_debit") > 0 Then
                                    debitval = Convert.ToString(Math.Round(supplierDr("acc_debit"), decPlaces))
                                End If
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(debitval, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.PaddingRight = 5.0F
                            cell.Rowspan = 2
                            tblSupplier.AddCell(cell)
                            Dim creditval As String = ""
                            If IsNumeric(supplierDr("acc_credit")) Then
                                If supplierDr("acc_credit") > 0 Then
                                    creditval = Convert.ToString(Math.Round(supplierDr("acc_credit"), decPlaces))
                                End If
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(creditval, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.PaddingRight = 5.0F
                            cell.Rowspan = 2
                            tblSupplier.AddCell(cell)
                            Dim debitvalBase As String = ""
                            If IsNumeric(supplierDr("acc_base_debit")) Then
                                If supplierDr("acc_base_debit") > 0 Then
                                    debitvalBase = Convert.ToString(Math.Round(supplierDr("acc_base_debit"), decPlaces))
                                End If
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(debitvalBase, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.PaddingRight = 5.0F
                            cell.Rowspan = 2
                            tblSupplier.AddCell(cell)
                            Dim creditvalBase As String = ""
                            If IsNumeric(supplierDr("acc_base_credit")) Then
                                If supplierDr("acc_base_credit") > 0 Then
                                    creditvalBase = Convert.ToString(Math.Round(supplierDr("acc_base_credit"), decPlaces))
                                End If
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(creditvalBase, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.PaddingRight = 5.0F
                            cell.Rowspan = 2
                            tblSupplier.AddCell(cell)
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(supplierDr("acc_narration")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            tblSupplier.AddCell(cell)
                        Next
                        Dim startRow As Integer = tblSupplier.Size - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk("Total", NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 7.0F
                        cell.Colspan = 3
                        cell.BackgroundColor = TableFooterBColor
                        tblSupplier.AddCell(cell)
                        Dim sumdebitval As String = Convert.ToString(Math.Round(supplierDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("acc_debit")), decPlaces))
                        phrase = New Phrase()
                        phrase.Add(New Chunk(sumdebitval, NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 5.0F
                        cell.BackgroundColor = TableFooterBColor
                        tblSupplier.AddCell(cell)
                        Dim sumcreditval As String = Convert.ToString(Math.Round(supplierDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("acc_credit")), decPlaces))
                        phrase = New Phrase()
                        phrase.Add(New Chunk(sumcreditval, NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 5.0F
                        cell.BackgroundColor = TableFooterBColor
                        tblSupplier.AddCell(cell)
                        Dim sumdebitvalBase As String = Convert.ToString(Math.Round(supplierDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("acc_base_debit")), decPlaces))
                        phrase = New Phrase()
                        phrase.Add(New Chunk(sumdebitvalBase, NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 5.0F
                        cell.BackgroundColor = TableFooterBColor
                        tblSupplier.AddCell(cell)
                        Dim sumcreditvalBase As String = Convert.ToString(Math.Round(supplierDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("acc_base_credit")), decPlaces))
                        phrase = New Phrase()
                        phrase.Add(New Chunk(sumcreditvalBase, NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 5.0F
                        cell.BackgroundColor = TableFooterBColor
                        tblSupplier.AddCell(cell)
                        tblSupplier.Complete = True
                        tblSupplier.KeepRowsTogether(startRow, tblSupplier.Size)
                        tblSupplier.SpacingBefore = 14
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 100 Then document.NewPage()
                        document.Add(tblSupplier)
                    End If

                    'Receipts Adjustment
                    If adjReceiptDt.Rows.Count > 0 Then
                        Dim arrAdjReceipt() As String = {"Booking No.", "Voucher No.", "Voucher Date", "Amount Adjusted"}
                        Dim tblAdjReceipt As PdfPTable = New PdfPTable(4)
                        tblAdjReceipt.SetWidths(New Single() {0.2F, 0.35F, 0.2F, 0.25F})
                        tblAdjReceipt.TotalWidth = documentWidth
                        tblAdjReceipt.LockedWidth = True
                        tblAdjReceipt.HeaderRows = 2
                        tblAdjReceipt.Complete = False
                        tblAdjReceipt.SplitRows = False

                        phrase = New Phrase()
                        phrase.Add(New Chunk("Receipts Adjusted", SubTitleFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 5.0F
                        cell.PaddingLeft = 0.0F
                        cell.Colspan = 4
                        tblAdjReceipt.AddCell(cell)
                        For i = 0 To arrAdjReceipt.GetUpperBound(0)
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(arrAdjReceipt(i)), NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.BackgroundColor = TabletitleBColor
                            tblAdjReceipt.AddCell(cell)
                        Next
                        tblAdjReceipt.KeepRowsTogether(0, 2)
                        For Each adjReceiptDr As DataRow In adjReceiptDt.Rows
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(adjReceiptDr("requestId")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            tblAdjReceipt.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(adjReceiptDr("voucherNo")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            tblAdjReceipt.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(adjReceiptDr("voucherDate")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            tblAdjReceipt.AddCell(cell)

                            Dim Adjustval As String = ""
                            If IsNumeric(adjReceiptDr("amountreceived")) Then
                                If adjReceiptDr("amountreceived") > 0 Then
                                    Adjustval = Convert.ToString(Math.Round(adjReceiptDr("amountreceived"), decPlaces))
                                End If
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Adjustval, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.PaddingRight = 5.0F
                            tblAdjReceipt.AddCell(cell)
                        Next
                        Dim startRow As Integer = tblAdjReceipt.Size - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk("Total", NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 7.0F
                        cell.Colspan = 3
                        cell.BackgroundColor = TableFooterBColor
                        tblAdjReceipt.AddCell(cell)
                        Dim sumAdjval As String = Convert.ToString(Math.Round(adjReceiptDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("amountreceived")), decPlaces))
                        phrase = New Phrase()
                        phrase.Add(New Chunk(sumAdjval, NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 5.0F
                        cell.BackgroundColor = TableFooterBColor
                        tblAdjReceipt.AddCell(cell)
                        tblAdjReceipt.Complete = True
                        tblAdjReceipt.KeepRowsTogether(startRow, tblAdjReceipt.Size)
                        tblAdjReceipt.SpacingBefore = 14
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 100 Then document.NewPage()
                        document.Add(tblAdjReceipt)
                    End If

                    'Approval
                    Dim arrApprove() As String = {"Prepared By :", "Checked By :", "Approved By :", "Received By :"}
                    Dim tblApprove As PdfPTable = New PdfPTable(8)
                    tblApprove.SetWidths(New Single() {0.15F, 0.1F, 0.15F, 0.1F, 0.15F, 0.1F, 0.15F, 0.1F})
                    tblApprove.TotalWidth = documentWidth
                    tblApprove.LockedWidth = True
                    For i = 0 To arrApprove.GetUpperBound(0)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(arrApprove(i)), NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.Border = Rectangle.NO_BORDER
                        cell.PaddingBottom = 3.0F
                        tblApprove.AddCell(cell)

                        cell = New PdfPCell()
                        cell.Border = Rectangle.NO_BORDER
                        cell.PaddingBottom = 3.0F
                        tblApprove.AddCell(cell)
                    Next
                    tblApprove.SpacingBefore = 30
                    document.Add(tblApprove)

                    document.AddTitle(Convert.ToString(headerDr("printHeader")) & " - " & invoiceNo.Replace("/", "-"))
                    document.Close()

                    If printMode = "download" Then
                        Dim pagingFont As Font = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.GRAY)
                        Dim reader As New PdfReader(memoryStream.ToArray())
                        Using mStream As New MemoryStream()
                            Using stamper As New PdfStamper(reader, mStream)
                                Dim pages As Integer = reader.NumberOfPages
                                For i As Integer = 1 To pages
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, New Phrase("Printed Date : " + Date.Today.ToString("dd/MM/yyyy") + "   " + DateTime.Now.ToString("HH:mm"), pagingFont), 50.0F, 20.0F, 0)
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, New Phrase("Page No : Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 540.0F, 20.0F, 0)
                                Next
                            End Using
                            bytes = mStream.ToArray()
                        End Using
                    End If
                End Using
            Else
                Throw New Exception("There is no row in header table for this invoice no")
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

#Region "Private Shared Function ImageCell(ByVal path As String, ByVal scaleX As Single, ByVal scaleY As Single, ByVal align As Integer) As PdfPCell"
    Private Shared Function ImageCell(ByVal path As String, ByVal scaleX As Single, ByVal scaleY As Single, ByVal align As Integer) As PdfPCell
        Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path))
        image.ScaleAbsolute(scaleX, scaleY)

        Dim cell As New PdfPCell(image)
        cell.BorderColor = BaseColor.WHITE
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 0.0F
        cell.PaddingTop = 0.0F
        Return cell
    End Function
#End Region
#Region "Public Sub PurchaseJournalPrint(ByVal invoiceNo As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "")"
    Public Sub PurchaseJournalPrint(ByVal invoiceNo As String, ByVal divcode As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "")
        Try
            Dim decPlaces As Integer
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As SqlCommand
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("select option_selected from reservation_parameters where param_id='509'", sqlConn)
            mySqlCmd.CommandType = CommandType.Text
            decPlaces = Convert.ToInt32(mySqlCmd.ExecuteScalar())
            mySqlCmd.Dispose()
            Dim convDecPlaces As Integer
            mySqlCmd = New SqlCommand("select option_selected from reservation_parameters where param_id='2038'", sqlConn)
            mySqlCmd.CommandType = CommandType.Text
            convDecPlaces = Convert.ToInt32(mySqlCmd.ExecuteScalar())
            mySqlCmd.Dispose()
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            mySqlCmd = New SqlCommand("sp_purchasejournal_print", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@invoiceNo", SqlDbType.VarChar, 20)).Value = invoiceNo
            mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            ds1 = ds
            Dim headerDt As DataTable = ds.Tables(0)
            ' Dim customerDt As DataTable = ds.Tables(1)
            Dim supplierDt As DataTable = ds.Tables(1)
            'Dim adjReceiptDt As DataTable = ds.Tables(3)
            clsDBConnect.dbConnectionClose(sqlConn)
            Dim remainingPageSpace As Single
            If headerDt.Rows.Count > 0 Then
                Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 20.0F, 35.0F)
                Dim documentWidth As Single = 550.0F
                Dim TitleFont As Font = FontFactory.GetFont(fontName, 12, Font.NORMAL, BaseColor.BLACK)
                Dim TitleFontBold As Font = FontFactory.GetFont(fontName, 13, Font.BOLD, BaseColor.BLACK)
                Using memoryStream As New System.IO.MemoryStream()
                    Dim writer As PdfWriter
                    If printMode = "download" Then
                        writer = PdfWriter.GetInstance(document, memoryStream)
                    Else
                        Dim path As String = System.Web.HttpContext.Current.Server.MapPath("~\SavedReports\") + fileName
                        writer = PdfWriter.GetInstance(document, New FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                    End If
                    Dim phrase As Phrase = Nothing
                    Dim cell As PdfPCell = Nothing
                    Dim table As PdfPTable = Nothing
                    document.Open()
                    'Header Table
                    Dim headerDr As DataRow = headerDt.Rows(0)
                    table = New PdfPTable(2)
                    table.TotalWidth = documentWidth
                    table.LockedWidth = True
                    table.SetWidths(New Single() {0.75F, 0.25F})
                    table.Complete = False
                    table.SplitRows = False
                    Dim tblLogo As PdfPTable = New PdfPTable(2)
                    tblLogo.SetWidths(New Single() {0.3F, 0.7F})
                    'Company Logo
                    If (headerDr("divcode") = "01") Then
                        cell = ImageCell("~/images/logo.png", 100.0F, 70.0F, PdfPCell.ALIGN_LEFT)
                    Else
                        cell = ImageCell("~/images/logo.png", 57.0F, 57.0F, PdfPCell.ALIGN_LEFT)
                    End If
                    cell.Rowspan = 2
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.NO_BORDER
                    tblLogo.AddCell(cell)

                    'Company Name 
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("division_master_des")), TitleFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    tblLogo.AddCell(cell)

                    'Company Address
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("address1")) & vbLf, NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 3.0F
                    tblLogo.AddCell(cell)
                    cell = New PdfPCell(tblLogo)
                    cell.Border = Rectangle.NO_BORDER
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    table.AddCell(cell)
                    cell = New PdfPCell()
                    cell.Border = Rectangle.NO_BORDER
                    table.AddCell(cell)
                    table.Complete = True
                    document.Add(table)

                    Dim tblTitle As PdfPTable = New PdfPTable(1)
                    tblTitle.SetWidths(New Single() {1.0F})
                    tblTitle.TotalWidth = documentWidth
                    tblTitle.LockedWidth = True
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("printHeader")), TitleFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER Or Rectangle.LEFT_BORDER
                    cell.BackgroundColor = titleColor
                    cell.PaddingBottom = 3.0F
                    tblTitle.AddCell(cell)
                    tblTitle.SpacingBefore = 7
                    document.Add(tblTitle)

                    'PDF Title
                    Dim tblInvoice As PdfPTable = New PdfPTable(2)
                    tblInvoice.SetWidths(New Single() {0.5F, 0.5F})
                    tblInvoice.TotalWidth = documentWidth
                    tblInvoice.LockedWidth = True

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Invoice No. : " & Convert.ToString(headerDr("invoiceNo")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.NO_BORDER
                    cell.PaddingBottom = 3.0F
                    cell.PaddingLeft = 10.0F
                    tblInvoice.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Invoice Date : " & Convert.ToString(headerDr("invoiceDate")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.NO_BORDER
                    cell.PaddingBottom = 3.0F
                    cell.PaddingRight = 15.0F
                    tblInvoice.AddCell(cell)
                    tblInvoice.SpacingBefore = 7
                    document.Add(tblInvoice)

                    'Page wise Header
                    Dim tblpageHead As PdfPTable = New PdfPTable(2)
                    tblpageHead.SetWidths(New Single() {0.5F, 0.5F})
                    tblpageHead.TotalWidth = documentWidth
                    tblpageHead.LockedWidth = True

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Invoice No. : " & Convert.ToString(headerDr("invoiceNo")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingTop = 1.0F
                    cell.PaddingBottom = 4.0F
                    cell.PaddingLeft = 10.0F
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                    tblpageHead.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Invoice Date : " & Convert.ToString(headerDr("invoiceDate")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingTop = 1.0F
                    cell.PaddingBottom = 4.0F
                    cell.PaddingRight = 15.0F
                    cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                    tblpageHead.AddCell(cell)
                    tblpageHead.SpacingBefore = 3
                    writer.PageEvent = New clsJournalPdfPageEvents(tblpageHead, printMode)

                    'Customer posting
                    'If customerDt.Rows.Count > 0 Then
                    '    Dim arrCustomer() As String = {"Account Name", "Currency", "Currency Rate", "Debit", "Credit", "Debit In " & Convert.ToString(headerDr("baseCurrCode")), "Credit In " & Convert.ToString(headerDr("baseCurrCode"))}
                    '    Dim tblCust As PdfPTable = New PdfPTable(7)
                    '    tblCust.SetWidths(New Single() {0.4F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F})
                    '    tblCust.TotalWidth = documentWidth
                    '    tblCust.LockedWidth = True
                    '    tblCust.HeaderRows = 2
                    '    tblCust.Complete = False
                    '    tblCust.SplitRows = False

                    '    phrase = New Phrase()
                    '    phrase.Add(New Chunk("Customer Posting", SubTitleFontBold))
                    '    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    '    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '    cell.PaddingBottom = 5.0F
                    '    cell.PaddingLeft = 0.0F
                    '    cell.Colspan = 7
                    '    tblCust.AddCell(cell)
                    '    For i = 0 To arrCustomer.GetUpperBound(0)
                    '        phrase = New Phrase()
                    '        phrase.Add(New Chunk(Convert.ToString(arrCustomer(i)), NormalFontBold))
                    '        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    '        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '        cell.PaddingTop = 2.0F
                    '        cell.PaddingBottom = 5.0F
                    '        cell.BackgroundColor = TabletitleBColor
                    '        tblCust.AddCell(cell)
                    '    Next
                    '    For Each custDr As DataRow In customerDt.Rows
                    '        Dim AcctName As String = ""
                    '        If Convert.ToString(custDr("accountName")) <> "" Then
                    '            AcctName = Convert.ToString(custDr("accountName")) & "   " & "[ " & Convert.ToString(custDr("accountcode")) & " ]" & vbCrLf & vbCrLf
                    '        End If
                    '        Dim ctrlAcctName As String = ""
                    '        If custDr("controlaccountName") <> "" Then
                    '            ctrlAcctName = Convert.ToString(custDr("controlaccountName")) & "   " & "[ " & Convert.ToString(custDr("controlaccountcode")) & " ]"
                    '        End If
                    '        phrase = New Phrase()
                    '        phrase.Add(New Chunk(AcctName, NormalFont))
                    '        phrase.Add(New Chunk(ctrlAcctName, NormalFont))
                    '        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                    '        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '        cell.PaddingTop = 2.0F
                    '        cell.PaddingBottom = 5.0F
                    '        tblCust.AddCell(cell)

                    '        phrase = New Phrase()
                    '        phrase.Add(New Chunk(Convert.ToString(custDr("currcode")), NormalFont))
                    '        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    '        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '        cell.PaddingTop = 2.0F
                    '        cell.PaddingBottom = 5.0F
                    '        cell.Rowspan = 2
                    '        tblCust.AddCell(cell)
                    '        Dim currencyRate As String = ""
                    '        If IsNumeric(custDr("acc_currency_rate")) Then
                    '            currencyRate = Convert.ToString(Math.Round(custDr("acc_currency_rate"), convDecPlaces))
                    '        End If
                    '        phrase = New Phrase()
                    '        phrase.Add(New Chunk(currencyRate, NormalFont))
                    '        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                    '        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '        cell.PaddingTop = 2.0F
                    '        cell.PaddingBottom = 5.0F
                    '        cell.Rowspan = 2
                    '        tblCust.AddCell(cell)
                    '        Dim debitval As String = ""
                    '        If IsNumeric(custDr("acc_debit")) Then
                    '            If custDr("acc_debit") > 0 Then
                    '                debitval = Convert.ToString(Math.Round(custDr("acc_debit"), decPlaces))
                    '            End If
                    '        End If
                    '        phrase = New Phrase()
                    '        phrase.Add(New Chunk(debitval, NormalFont))
                    '        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                    '        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '        cell.PaddingTop = 2.0F
                    '        cell.PaddingBottom = 5.0F
                    '        cell.PaddingRight = 5.0F
                    '        cell.Rowspan = 2
                    '        tblCust.AddCell(cell)
                    '        Dim creditval As String = ""
                    '        If IsNumeric(custDr("acc_credit")) Then
                    '            If custDr("acc_credit") > 0 Then
                    '                creditval = Convert.ToString(Math.Round(custDr("acc_credit"), decPlaces))
                    '            End If
                    '        End If
                    '        phrase = New Phrase()
                    '        phrase.Add(New Chunk(creditval, NormalFont))
                    '        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                    '        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '        cell.PaddingTop = 2.0F
                    '        cell.PaddingBottom = 5.0F
                    '        cell.PaddingRight = 5.0F
                    '        cell.Rowspan = 2
                    '        tblCust.AddCell(cell)
                    '        Dim debitvalBase As String = ""
                    '        If IsNumeric(custDr("acc_base_debit")) Then
                    '            If custDr("acc_base_debit") > 0 Then
                    '                debitvalBase = Convert.ToString(Math.Round(custDr("acc_base_debit"), decPlaces))
                    '            End If
                    '        End If
                    '        phrase = New Phrase()
                    '        phrase.Add(New Chunk(debitvalBase, NormalFont))
                    '        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                    '        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '        cell.PaddingTop = 2.0F
                    '        cell.PaddingBottom = 5.0F
                    '        cell.PaddingRight = 5.0F
                    '        cell.Rowspan = 2
                    '        tblCust.AddCell(cell)
                    '        Dim creditvalBase As String = ""
                    '        If IsNumeric(custDr("acc_base_credit")) Then
                    '            If custDr("acc_base_credit") > 0 Then
                    '                creditvalBase = Convert.ToString(Math.Round(custDr("acc_base_credit"), decPlaces))
                    '            End If
                    '        End If
                    '        phrase = New Phrase()
                    '        phrase.Add(New Chunk(creditvalBase, NormalFont))
                    '        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                    '        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '        cell.PaddingTop = 2.0F
                    '        cell.PaddingBottom = 5.0F
                    '        cell.PaddingRight = 5.0F
                    '        cell.Rowspan = 2
                    '        tblCust.AddCell(cell)
                    '        phrase = New Phrase()
                    '        phrase.Add(New Chunk(Convert.ToString(custDr("acc_narration")), NormalFont))
                    '        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                    '        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '        cell.PaddingTop = 2.0F
                    '        cell.PaddingBottom = 5.0F
                    '        tblCust.AddCell(cell)
                    '    Next
                    '    Dim custStartRow = tblCust.Size - 1
                    '    phrase = New Phrase()
                    '    phrase.Add(New Chunk("Total", NormalFontBold))
                    '    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                    '    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '    cell.PaddingTop = 2.0F
                    '    cell.PaddingBottom = 5.0F
                    '    cell.PaddingRight = 7.0F
                    '    cell.Colspan = 3
                    '    cell.BackgroundColor = TableFooterBColor
                    '    tblCust.AddCell(cell)
                    '    Dim sumdebitval As String = Convert.ToString(Math.Round(customerDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("acc_debit")), decPlaces))
                    '    phrase = New Phrase()
                    '    phrase.Add(New Chunk(sumdebitval, NormalFontBold))
                    '    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                    '    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '    cell.PaddingTop = 2.0F
                    '    cell.PaddingBottom = 5.0F
                    '    cell.PaddingRight = 5.0F
                    '    cell.BackgroundColor = TableFooterBColor
                    '    tblCust.AddCell(cell)
                    '    Dim sumcreditval As String = Convert.ToString(Math.Round(customerDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("acc_credit")), decPlaces))
                    '    phrase = New Phrase()
                    '    phrase.Add(New Chunk(sumcreditval, NormalFontBold))
                    '    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                    '    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '    cell.PaddingTop = 2.0F
                    '    cell.PaddingBottom = 5.0F
                    '    cell.PaddingRight = 5.0F
                    '    cell.BackgroundColor = TableFooterBColor
                    '    tblCust.AddCell(cell)
                    '    Dim sumdebitvalBase As String = Convert.ToString(Math.Round(customerDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("acc_base_debit")), decPlaces))
                    '    phrase = New Phrase()
                    '    phrase.Add(New Chunk(sumdebitvalBase, NormalFontBold))
                    '    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                    '    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '    cell.PaddingTop = 2.0F
                    '    cell.PaddingBottom = 5.0F
                    '    cell.PaddingRight = 5.0F
                    '    cell.BackgroundColor = TableFooterBColor
                    '    tblCust.AddCell(cell)
                    '    Dim sumcreditvalBase As String = Convert.ToString(Math.Round(customerDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("acc_base_credit")), decPlaces))
                    '    phrase = New Phrase()
                    '    phrase.Add(New Chunk(sumcreditvalBase, NormalFontBold))
                    '    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                    '    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '    cell.PaddingTop = 2.0F
                    '    cell.PaddingBottom = 5.0F
                    '    cell.PaddingRight = 5.0F
                    '    cell.BackgroundColor = TableFooterBColor
                    '    tblCust.AddCell(cell)
                    '    tblCust.Complete = True
                    '    tblCust.KeepRowsTogether(custStartRow, tblCust.Size)
                    '    tblCust.SpacingBefore = 10
                    '    document.Add(tblCust)
                    'End If

                    'Provisional Supplier Posting
                    If supplierDt.Rows.Count > 0 Then
                        Dim arrSupplier() As String = {"Account Name", "Currency", "Currency Rate", "Debit", "Credit", "Debit In " & Convert.ToString(headerDr("baseCurrCode")), "Credit In " & Convert.ToString(headerDr("baseCurrCode"))}
                        Dim tblSupplier As PdfPTable = New PdfPTable(7)
                        tblSupplier.SetWidths(New Single() {0.4F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F})
                        tblSupplier.TotalWidth = documentWidth
                        tblSupplier.LockedWidth = True
                        tblSupplier.HeaderRows = 2
                        tblSupplier.Complete = False
                        tblSupplier.SplitRows = False

                        phrase = New Phrase()
                        phrase.Add(New Chunk("Provisional Supplier Posting", SubTitleFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 5.0F
                        cell.PaddingLeft = 0.0F
                        cell.Colspan = 7
                        tblSupplier.AddCell(cell)
                        For i = 0 To arrSupplier.GetUpperBound(0)
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(arrSupplier(i)), NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.BackgroundColor = TabletitleBColor
                            tblSupplier.AddCell(cell)
                        Next
                        tblSupplier.KeepRowsTogether(0, 2)
                        For Each supplierDr As DataRow In supplierDt.Rows
                            Dim AcctName As String = ""
                            If Convert.ToString(supplierDr("accountName")) <> "" Then
                                AcctName = Convert.ToString(supplierDr("accountName")) & "   " & "[ " & Convert.ToString(supplierDr("accountcode")) & " ]" & vbCrLf & vbCrLf
                            End If
                            Dim ctrlAcctName As String = ""
                            If supplierDr("controlaccountName") <> "" Then
                                ctrlAcctName = Convert.ToString(supplierDr("controlaccountName")) & "   " & "[ " & Convert.ToString(supplierDr("controlaccountcode")) & " ]"
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(AcctName, NormalFont))
                            phrase.Add(New Chunk(ctrlAcctName, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            tblSupplier.AddCell(cell)
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(supplierDr("currcode")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.Rowspan = 2
                            tblSupplier.AddCell(cell)
                            Dim currencyRate As String = ""
                            If IsNumeric(supplierDr("acc_currency_rate")) Then
                                currencyRate = Convert.ToString(Math.Round(supplierDr("acc_currency_rate"), convDecPlaces))
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(currencyRate, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.Rowspan = 2
                            tblSupplier.AddCell(cell)
                            Dim debitval As String = ""
                            If IsNumeric(supplierDr("acc_debit")) Then
                                If supplierDr("acc_debit") > 0 Then
                                    debitval = Convert.ToString(Math.Round(supplierDr("acc_debit"), decPlaces))
                                End If
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(debitval, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.PaddingRight = 5.0F
                            cell.Rowspan = 2
                            tblSupplier.AddCell(cell)
                            Dim creditval As String = ""
                            If IsNumeric(supplierDr("acc_credit")) Then
                                If supplierDr("acc_credit") > 0 Then
                                    creditval = Convert.ToString(Math.Round(supplierDr("acc_credit"), decPlaces))
                                End If
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(creditval, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.PaddingRight = 5.0F
                            cell.Rowspan = 2
                            tblSupplier.AddCell(cell)
                            Dim debitvalBase As String = ""
                            If IsNumeric(supplierDr("acc_base_debit")) Then
                                If supplierDr("acc_base_debit") > 0 Then
                                    debitvalBase = Convert.ToString(Math.Round(supplierDr("acc_base_debit"), decPlaces))
                                End If
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(debitvalBase, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.PaddingRight = 5.0F
                            cell.Rowspan = 2
                            tblSupplier.AddCell(cell)
                            Dim creditvalBase As String = ""
                            If IsNumeric(supplierDr("acc_base_credit")) Then
                                If supplierDr("acc_base_credit") > 0 Then
                                    creditvalBase = Convert.ToString(Math.Round(supplierDr("acc_base_credit"), decPlaces))
                                End If
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(creditvalBase, NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            cell.PaddingRight = 5.0F
                            cell.Rowspan = 2
                            tblSupplier.AddCell(cell)
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(supplierDr("acc_narration")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingTop = 2.0F
                            cell.PaddingBottom = 5.0F
                            tblSupplier.AddCell(cell)
                        Next
                        Dim startRow As Integer = tblSupplier.Size - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk("Total", NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 7.0F
                        cell.Colspan = 3
                        cell.BackgroundColor = TableFooterBColor
                        tblSupplier.AddCell(cell)
                        Dim sumdebitval As String = Convert.ToString(Math.Round(supplierDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("acc_debit")), decPlaces))
                        phrase = New Phrase()
                        phrase.Add(New Chunk(sumdebitval, NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 5.0F
                        cell.BackgroundColor = TableFooterBColor
                        tblSupplier.AddCell(cell)
                        Dim sumcreditval As String = Convert.ToString(Math.Round(supplierDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("acc_credit")), decPlaces))
                        phrase = New Phrase()
                        phrase.Add(New Chunk(sumcreditval, NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 5.0F
                        cell.BackgroundColor = TableFooterBColor
                        tblSupplier.AddCell(cell)
                        Dim sumdebitvalBase As String = Convert.ToString(Math.Round(supplierDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("acc_base_debit")), decPlaces))
                        phrase = New Phrase()
                        phrase.Add(New Chunk(sumdebitvalBase, NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 5.0F
                        cell.BackgroundColor = TableFooterBColor
                        tblSupplier.AddCell(cell)
                        Dim sumcreditvalBase As String = Convert.ToString(Math.Round(supplierDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("acc_base_credit")), decPlaces))
                        phrase = New Phrase()
                        phrase.Add(New Chunk(sumcreditvalBase, NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 2.0F
                        cell.PaddingBottom = 5.0F
                        cell.PaddingRight = 5.0F
                        cell.BackgroundColor = TableFooterBColor
                        tblSupplier.AddCell(cell)
                        tblSupplier.Complete = True
                        tblSupplier.KeepRowsTogether(startRow, tblSupplier.Size)
                        tblSupplier.SpacingBefore = 14
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 100 Then document.NewPage()
                        document.Add(tblSupplier)
                    End If

                    'Receipts Adjustment
                    'If adjReceiptDt.Rows.Count > 0 Then
                    '    Dim arrAdjReceipt() As String = {"Booking No.", "Voucher No.", "Voucher Date", "Amount Adjusted"}
                    '    Dim tblAdjReceipt As PdfPTable = New PdfPTable(4)
                    '    tblAdjReceipt.SetWidths(New Single() {0.2F, 0.35F, 0.2F, 0.25F})
                    '    tblAdjReceipt.TotalWidth = documentWidth
                    '    tblAdjReceipt.LockedWidth = True
                    '    tblAdjReceipt.HeaderRows = 2
                    '    tblAdjReceipt.Complete = False
                    '    tblAdjReceipt.SplitRows = False

                    '    phrase = New Phrase()
                    '    phrase.Add(New Chunk("Receipts Adjusted", SubTitleFontBold))
                    '    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    '    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '    cell.PaddingBottom = 5.0F
                    '    cell.PaddingLeft = 0.0F
                    '    cell.Colspan = 4
                    '    tblAdjReceipt.AddCell(cell)
                    '    For i = 0 To arrAdjReceipt.GetUpperBound(0)
                    '        phrase = New Phrase()
                    '        phrase.Add(New Chunk(Convert.ToString(arrAdjReceipt(i)), NormalFontBold))
                    '        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    '        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '        cell.PaddingTop = 2.0F
                    '        cell.PaddingBottom = 5.0F
                    '        cell.BackgroundColor = TabletitleBColor
                    '        tblAdjReceipt.AddCell(cell)
                    '    Next
                    '    tblAdjReceipt.KeepRowsTogether(0, 2)
                    '    For Each adjReceiptDr As DataRow In adjReceiptDt.Rows
                    '        phrase = New Phrase()
                    '        phrase.Add(New Chunk(Convert.ToString(adjReceiptDr("requestId")), NormalFont))
                    '        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                    '        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '        cell.PaddingTop = 2.0F
                    '        cell.PaddingBottom = 5.0F
                    '        tblAdjReceipt.AddCell(cell)

                    '        phrase = New Phrase()
                    '        phrase.Add(New Chunk(Convert.ToString(adjReceiptDr("voucherNo")), NormalFont))
                    '        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                    '        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '        cell.PaddingTop = 2.0F
                    '        cell.PaddingBottom = 5.0F
                    '        tblAdjReceipt.AddCell(cell)

                    '        phrase = New Phrase()
                    '        phrase.Add(New Chunk(Convert.ToString(adjReceiptDr("voucherDate")), NormalFont))
                    '        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                    '        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '        cell.PaddingTop = 2.0F
                    '        cell.PaddingBottom = 5.0F
                    '        tblAdjReceipt.AddCell(cell)

                    '        Dim Adjustval As String = ""
                    '        If IsNumeric(adjReceiptDr("amountreceived")) Then
                    '            If adjReceiptDr("amountreceived") > 0 Then
                    '                Adjustval = Convert.ToString(Math.Round(adjReceiptDr("amountreceived"), decPlaces))
                    '            End If
                    '        End If
                    '        phrase = New Phrase()
                    '        phrase.Add(New Chunk(Adjustval, NormalFont))
                    '        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                    '        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '        cell.PaddingTop = 2.0F
                    '        cell.PaddingBottom = 5.0F
                    '        cell.PaddingRight = 5.0F
                    '        tblAdjReceipt.AddCell(cell)
                    '    Next
                    '    Dim startRow As Integer = tblAdjReceipt.Size - 1
                    '    phrase = New Phrase()
                    '    phrase.Add(New Chunk("Total", NormalFontBold))
                    '    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                    '    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '    cell.PaddingTop = 2.0F
                    '    cell.PaddingBottom = 5.0F
                    '    cell.PaddingRight = 7.0F
                    '    cell.Colspan = 3
                    '    cell.BackgroundColor = TableFooterBColor
                    '    tblAdjReceipt.AddCell(cell)
                    '    Dim sumAdjval As String = Convert.ToString(Math.Round(adjReceiptDt.AsEnumerable().Sum(Function(n) n.Field(Of Decimal)("amountreceived")), decPlaces))
                    '    phrase = New Phrase()
                    '    phrase.Add(New Chunk(sumAdjval, NormalFontBold))
                    '    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                    '    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '    cell.PaddingTop = 2.0F
                    '    cell.PaddingBottom = 5.0F
                    '    cell.PaddingRight = 5.0F
                    '    cell.BackgroundColor = TableFooterBColor
                    '    tblAdjReceipt.AddCell(cell)
                    '    tblAdjReceipt.Complete = True
                    '    tblAdjReceipt.KeepRowsTogether(startRow, tblAdjReceipt.Size)
                    '    tblAdjReceipt.SpacingBefore = 14
                    '    remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                    '    If remainingPageSpace < 100 Then document.NewPage()
                    '    document.Add(tblAdjReceipt)
                    'End If

                    'Approval
                    Dim arrApprove() As String = {"Prepared By :", "Checked By :", "Approved By :", "Received By :"}
                    Dim tblApprove As PdfPTable = New PdfPTable(8)
                    tblApprove.SetWidths(New Single() {0.15F, 0.1F, 0.15F, 0.1F, 0.15F, 0.1F, 0.15F, 0.1F})
                    tblApprove.TotalWidth = documentWidth
                    tblApprove.LockedWidth = True
                    For i = 0 To arrApprove.GetUpperBound(0)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(arrApprove(i)), NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.Border = Rectangle.NO_BORDER
                        cell.PaddingBottom = 3.0F
                        tblApprove.AddCell(cell)

                        cell = New PdfPCell()
                        cell.Border = Rectangle.NO_BORDER
                        cell.PaddingBottom = 3.0F
                        tblApprove.AddCell(cell)
                    Next
                    tblApprove.SpacingBefore = 30
                    document.Add(tblApprove)

                    document.AddTitle(Convert.ToString(headerDr("printHeader")) & " - " & invoiceNo.Replace("/", "-"))
                    document.Close()

                    If printMode = "download" Then
                        Dim pagingFont As Font = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.GRAY)
                        Dim reader As New PdfReader(memoryStream.ToArray())
                        Using mStream As New MemoryStream()
                            Using stamper As New PdfStamper(reader, mStream)
                                Dim pages As Integer = reader.NumberOfPages
                                For i As Integer = 1 To pages
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, New Phrase("Printed Date : " + Date.Today.ToString("dd/MM/yyyy") + "   " + DateTime.Now.ToString("HH:mm"), pagingFont), 50.0F, 20.0F, 0)
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, New Phrase("Page No : Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 540.0F, 20.0F, 0)
                                Next
                            End Using
                            bytes = mStream.ToArray()
                        End Using
                    End If
                End Using
            Else
                Throw New Exception("There is no row in header table for this invoice no")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
End Class
