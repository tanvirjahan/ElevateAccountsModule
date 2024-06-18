Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq

Public Class clsInvoicePdf

#Region "Global Variable"
    Dim objUtils As clsUtils
    Dim fontName As String = "Arial"
    Dim fontSize As Integer = 10
    Dim NormalFont As Font = FontFactory.GetFont(fontName, fontSize, Font.NORMAL, BaseColor.BLACK)
    Dim NormalFontBold As Font = FontFactory.GetFont(fontName, fontSize, Font.BOLD, BaseColor.BLACK)
    Dim serviceFont As Font = FontFactory.GetFont(fontName, 9, Font.NORMAL, BaseColor.BLACK)
    Dim serviceFontBold As Font = FontFactory.GetFont(fontName, 9, Font.BOLD, BaseColor.BLACK)
    Dim titleColor As BaseColor = New BaseColor(214, 214, 214)
#End Region
    Public Sub PurchaseInvoicePrint(ByVal invoiceNo As String, ByVal divcode As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "")
        Try
            Dim decPlaces As Integer '= bjUtils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select option_selected from reservation_parameters where param_id='509'"))
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As SqlCommand
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("select option_selected from reservation_parameters where param_id='509'", sqlConn)
            mySqlCmd.CommandType = CommandType.Text
            decPlaces = Convert.ToInt32(mySqlCmd.ExecuteScalar())
            mySqlCmd.Dispose()
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            mySqlCmd = New SqlCommand("sp_purchaseinvoice_print", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@invoiceNo", SqlDbType.VarChar, 20)).Value = invoiceNo
            mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            ds1 = ds
            Dim headerDt As DataTable = ds.Tables(0)
            Dim serviceDt As DataTable = ds.Tables(1)
            Dim totalDt As DataTable = ds.Tables(2)
            Dim guestDt As DataTable = ds.Tables(3)

            clsDBConnect.dbConnectionClose(sqlConn)
            Dim remainingPageSpace As Single
            If headerDt.Rows.Count > 0 Then
                Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 20.0F, 35.0F)
                Dim documentWidth As Single = 550.0F
                'Dim NormalFontRed As Font = FontFactory.GetFont(fontName, 11, Font.NORMAL, BaseColor.RED)
                'Dim NormalFontBoldRed As Font = FontFactory.GetFont(fontName, 11, Font.BOLD, BaseColor.RED)
                Dim TitleFont As Font = FontFactory.GetFont(fontName, 12, Font.NORMAL, BaseColor.BLACK)
                Dim TitleFontBold As Font = FontFactory.GetFont(fontName, 14, Font.BOLD, BaseColor.BLACK)
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
                    table.SetWidths(New Single() {0.65F, 0.35F})
                    table.Complete = False
                    table.SplitRows = False
                    Dim tblLogo As PdfPTable = New PdfPTable(3)
                    tblLogo.SetWidths(New Single() {0.01F, 0.29F, 0.7F})
                    cell = New PdfPCell()
                    cell.Border = Rectangle.NO_BORDER
                    cell.Rowspan = 2
                    tblLogo.AddCell(cell)
                    'Company Logo
                    If (headerDr("divcode") = "01") Then
                        ' cell = ImageCell("~/images/Logo.png", 100.0F, 70.0F, PdfPCell.ALIGN_LEFT)
                        cell = ImageCell("~/images/logo.jpg", 70.0F, PdfPCell.ALIGN_LEFT)
                    Else
                        ' cell = ImageCell("~/images/Logo.png", 100.0F, 57.0F, PdfPCell.ALIGN_LEFT)
                        cell = ImageCell("~/images/logo.jpg", 57.0F, PdfPCell.ALIGN_LEFT)
                    End If
                    cell.Rowspan = 2
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
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
                    'cell = New PdfPCell()
                    'cell.Border = Rectangle.NO_BORDER
                    'cell.colspan = 2
                    'tblLogo.AddCell(cell)
                    cell = New PdfPCell(tblLogo)
                    cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER Or Rectangle.LEFT_BORDER
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    table.AddCell(cell)

                    Dim tblAddress As PdfPTable = New PdfPTable(3)
                    tblAddress.SetWidths(New Single() {0.25F, 0.05F, 0.7F})

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Tel", NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("tel")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Fax", NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("fax")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("E-mail", NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("email")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Website", NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("website")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("TRN", NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("TRN")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    cell = New PdfPCell(tblAddress)
                    cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
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

                    Dim tblClient As PdfPTable = New PdfPTable(2)
                    tblClient.SetWidths(New Single() {0.65F, 0.35F})
                    tblClient.TotalWidth = documentWidth
                    tblClient.LockedWidth = True
                    tblClient.Complete = False
                    tblClient.SplitRows = False

                    Dim tblTo As PdfPTable = New PdfPTable(2)
                    tblTo.SetWidths(New Single() {0.1F, 0.9F})

                    phrase = New Phrase()
                    phrase.Add(New Chunk("To", NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3.0F
                    tblTo.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentcode")) & "    " & Convert.ToString(headerDr("agentName")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3.0F
                    tblTo.AddCell(cell)
                    cell = New PdfPCell()
                    cell.Border = Rectangle.NO_BORDER
                    tblTo.AddCell(cell)
                    Dim agentAddress As String = Convert.ToString(headerDr("agentadd1"))
                    If Convert.ToString(headerDr("agentadd2")) <> "" Then agentAddress = agentAddress + ", " + Convert.ToString(headerDr("agentadd2"))
                    If Convert.ToString(headerDr("agentadd3")) <> "" Then agentAddress = agentAddress + ", " + Convert.ToString(headerDr("agentadd3"))
                    phrase = New Phrase()
                    phrase.Add(New Chunk(agentAddress, NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3.0F
                    tblTo.AddCell(cell)
                    cell = New PdfPCell()
                    cell.Border = Rectangle.NO_BORDER
                    tblTo.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("TRN : " & Convert.ToString(headerDr("agenttrnno")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3.0F
                    tblTo.AddCell(cell)
                    cell = New PdfPCell(tblTo)
                    cell.Border = Rectangle.NO_BORDER
                    tblClient.AddCell(cell)

                    Dim tblInvoice As PdfPTable = New PdfPTable(2)
                    tblInvoice.SetWidths(New Single() {0.3F, 0.7F})

                    phrase = New Phrase()
                    phrase.Add(New Chunk("PI No.", NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("InvoiceNo")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Date", NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("InvoiceDate")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Ref. No.", NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentRef")), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3.0F
                    tblInvoice.AddCell(cell)
                    cell = New PdfPCell(tblInvoice)
                    cell.Border = Rectangle.NO_BORDER
                    tblClient.AddCell(cell)
                    tblClient.AddCell(cell)
                    tblClient.SpacingBefore = 7
                    tblClient.Complete = True
                    document.Add(tblClient)

                    'Dim tblService As PdfPTable = New PdfPTable(13)
                    'tblService.SetWidths(New Single() {0.04F, 0.09F, 0.05F, 0.09F, 0.09F, 0.035F, 0.185F, 0.08F, 0.08F, 0.045F, 0.07F, 0.07F, 0.075F})
                    Dim tblService As PdfPTable = New PdfPTable(12)
                    tblService.SetWidths(New Single() {0.04F, 0.095F, 0.05F, 0.09F, 0.09F, 0.035F, 0.2F, 0.09F, 0.09F, 0.045F, 0.085F, 0.09F})
                    tblService.TotalWidth = documentWidth
                    tblService.LockedWidth = True
                    tblService.HeaderRows = 1
                    tblService.Complete = False
                    tblService.SplitRows = False
                    'Dim arrService() As String = {"S.N.", "Resn.No.", "Units", "In", "Out", "Nts", "Particulars", "Taxable Amount", "Non Taxable Amount", "VAT %", "Vat Amount", "Commission", "Grand Total"}
                    Dim arrService() As String = {"S.N.", "Resn.No.", "Units", "In", "Out", "Nts", "Particulars", "Taxable Amount", "Non Taxable Amount", "VAT %", "Vat Amount", "Grand Total"}

                    For i = 0 To arrService.GetUpperBound(0)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrService(i), serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 1.0F
                        tblService.AddCell(cell)
                    Next
                    Dim slno As Integer = 0
                    For Each dr As DataRow In serviceDt.Rows
                        slno = slno + 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(slno), serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        tblService.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(dr("requestId")), serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        tblService.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(dr("units")), serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        tblService.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(dr("checkin")), serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        tblService.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(dr("checkout")), serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        tblService.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(dr("nights")), serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        tblService.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(dr("particulars")), serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        tblService.AddCell(cell)
                        'Dim rates As String
                        'If Not IsDBNull(dr("rate")) Then
                        '    rates = Convert.ToString(Math.Round(dr("rate"), decPlaces))
                        'Else
                        '    rates = ""
                        'End If
                        'phrase = New Phrase()
                        'phrase.Add(New Chunk(rates, serviceFont))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.PaddingBottom = 3.0F
                        'tblService.AddCell(cell)
                        Dim amt As String
                        If Not IsDBNull(dr("taxableAmountbase")) Then
                            amt = Convert.ToString(Math.Round(dr("taxableAmountbase"), decPlaces))
                        Else
                            amt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(amt, serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        tblService.AddCell(cell)
                        Dim taxableAmt As String
                        If Not IsDBNull(dr("nontaxableamountbase")) Then
                            taxableAmt = Convert.ToString(Math.Round(dr("nontaxableamountbase"), decPlaces))
                        Else
                            taxableAmt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(taxableAmt, serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        tblService.AddCell(cell)
                        Dim vatPerc As String
                        If Not IsDBNull(dr("vatperc")) Then
                            vatPerc = Convert.ToString(Math.Round(dr("vatperc"), 2))
                        Else
                            vatPerc = ""
                        End If

                        phrase = New Phrase()
                        phrase.Add(New Chunk(vatPerc, serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        tblService.AddCell(cell)

                        Dim vatAmt As String
                        If Not IsDBNull(dr("vatAmountbase")) Then
                            vatAmt = Convert.ToString(Math.Round(dr("vatAmountbase"), decPlaces))
                        Else
                            vatAmt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(vatAmt, serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        tblService.AddCell(cell)
                        Dim cmsion As String
                        Dim grandAmt As String
                        Dim grandTotal As Decimal
                        If Not IsDBNull(dr("grandAmountbase")) Then
                            grandAmt = Convert.ToString(Math.Round(dr("grandAmountbase"), decPlaces))
                        Else
                            grandAmt = ""
                        End If
                        If Not IsDBNull(dr("Commission")) Then
                            cmsion = Convert.ToString(Math.Round(dr("Commission"), 2))
                            If Not IsDBNull(dr("grandAmountbase")) Then
                                grandTotal = Math.Round(dr("Commission"), 2) + Math.Round(dr("grandAmountbase"), decPlaces)
                                grandAmt = grandTotal.ToString()
                            End If
                        Else
                            cmsion = ""
                        End If

                        'phrase = New Phrase()
                        'phrase.Add(New Chunk(cmsion, serviceFont))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.PaddingBottom = 3.0F
                        'tblService.AddCell(cell)


                        phrase = New Phrase()
                        phrase.Add(New Chunk(grandAmt, serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        tblService.AddCell(cell)
                    Next

                    tblService.Complete = True
                    tblService.SpacingBefore = 7
                    remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                    If remainingPageSpace < 72 Then document.NewPage()
                    document.Add(tblService)

                    If totalDt.Rows.Count > 0 Then
                        Dim totalDr As DataRow = totalDt.Rows(0)
                        'Dim tblTotal As PdfPTable = New PdfPTable(7)
                        'tblTotal.SetWidths(New Single() {0.58F, 0.08F, 0.05F, 0.045F, 0.085F, 0.08F, 0.08F})
                        Dim tblTotal As PdfPTable = New PdfPTable(6)
                        tblTotal.SetWidths(New Single() {0.6F, 0.09F, 0.09F, 0.045F, 0.085F, 0.09F})
                        tblTotal.TotalWidth = documentWidth
                        tblTotal.LockedWidth = True
                        tblTotal.Complete = False
                        tblTotal.SplitRows = False
                        tblTotal.KeepTogether = True
                        phrase = New Phrase()
                        phrase.Add(New Chunk("Total(" & Convert.ToString(headerDr("salecurrcode")) & ")", serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                        cell.PaddingBottom = 6.0F
                        cell.PaddingTop = 6.0F
                        tblTotal.AddCell(cell)
                        Dim TotalAmt As String
                        If Not IsDBNull(totalDr("TotaltaxableAmountbase")) Then
                            TotalAmt = Convert.ToString(Math.Round(totalDr("TotaltaxableAmountbase"), decPlaces))
                        Else
                            TotalAmt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(TotalAmt, serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                        cell.PaddingBottom = 6.0F
                        cell.PaddingTop = 6.0F
                        tblTotal.AddCell(cell)


                        Dim TotalTaxableAmt As String
                        If Not IsDBNull(totalDr("Totalnontaxableamountbase")) Then
                            TotalTaxableAmt = Convert.ToString(Math.Round(totalDr("Totalnontaxableamountbase"), decPlaces))
                        Else
                            TotalTaxableAmt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(TotalTaxableAmt, serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                        cell.PaddingBottom = 6.0F
                        cell.PaddingTop = 6.0F
                        tblTotal.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk("", serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                        cell.PaddingBottom = 6.0F
                        cell.PaddingTop = 6.0F
                        tblTotal.AddCell(cell)
                        Dim TotalVatAmt As String
                        If Not IsDBNull(totalDr("totalvatAmountbase")) Then
                            TotalVatAmt = Convert.ToString(Math.Round(totalDr("totalvatAmountbase"), decPlaces))
                        Else
                            TotalVatAmt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(TotalVatAmt, serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                        cell.PaddingBottom = 6.0F
                        cell.PaddingTop = 6.0F
                        tblTotal.AddCell(cell)
                        Dim TotalCommissionAmt As String
                        If Not IsDBNull(totalDr("Commission")) Then
                            TotalCommissionAmt = Convert.ToString(Math.Round(totalDr("Commission"), decPlaces))
                        Else
                            TotalCommissionAmt = ""
                        End If
                        'phrase = New Phrase()
                        'phrase.Add(New Chunk(TotalCommissionAmt, serviceFontBold))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.PaddingBottom = 6.0F
                        'cell.PaddingTop = 6.0F
                        'cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                        'tblTotal.AddCell(cell)
                        Dim TotalgrandAmt As String
                        Dim Totalgrand As Decimal
                        If Not IsDBNull(totalDr("TotalgrandAmountbase")) Then
                            If Not IsDBNull(totalDr("Commission")) Then
                                Totalgrand = Math.Round(totalDr("totalgrandAmountbase"), decPlaces) + Math.Round(totalDr("Commission"), decPlaces)
                            Else
                                Totalgrand = Math.Round(totalDr("totalgrandAmountbase"), decPlaces)
                            End If
                            TotalgrandAmt = Convert.ToString(Totalgrand)
                        Else
                            TotalgrandAmt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(TotalgrandAmt, serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 6.0F
                        cell.PaddingTop = 6.0F
                        cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                        tblTotal.AddCell(cell)
                        'Dim convrate As String
                        'If Not IsDBNull(headerDr("saleconvrate")) Then
                        '    convrate = Convert.ToString(Math.Round(headerDr("saleconvrate"), 3))
                        'Else
                        '    convrate = ""
                        'End If
                        'phrase = New Phrase()
                        'phrase.Add(New Chunk("Total(" & Convert.ToString(headerDr("basecurrcode")) & ")@" & convrate, serviceFontBold))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.Border = Rectangle.BOTTOM_BORDER Or Rectangle.LEFT_BORDER
                        'cell.PaddingBottom = 6.0F
                        'tblTotal.AddCell(cell)
                        'Dim TotalBaseAmt As String
                        'If Not IsDBNull(totalDr("TotalBaseAmount")) Then
                        '    TotalBaseAmt = Convert.ToString(Math.Round(totalDr("TotalBaseAmount"), decPlaces))
                        'Else
                        '    TotalBaseAmt = ""
                        'End If
                        'phrase = New Phrase()
                        'phrase.Add(New Chunk(TotalBaseAmt, serviceFontBold))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.Border = Rectangle.BOTTOM_BORDER
                        'cell.PaddingBottom = 6.0F
                        'tblTotal.AddCell(cell)
                        'phrase = New Phrase()
                        'phrase.Add(New Chunk("", serviceFontBold))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.Border = Rectangle.BOTTOM_BORDER
                        'cell.PaddingBottom = 6.0F
                        'tblTotal.AddCell(cell)
                        'Dim TotalBaseTaxableAmt As String
                        'If Not IsDBNull(totalDr("TotalBaseTaxableAmount")) Then
                        '    TotalBaseTaxableAmt = Convert.ToString(Math.Round(totalDr("TotalBaseTaxableAmount"), decPlaces))
                        'Else
                        '    TotalBaseTaxableAmt = ""
                        'End If
                        'phrase = New Phrase()
                        'phrase.Add(New Chunk(TotalBaseTaxableAmt, serviceFontBold))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.Border = Rectangle.BOTTOM_BORDER
                        'cell.PaddingBottom = 6.0F
                        'tblTotal.AddCell(cell)
                        'Dim TotalBaseVatAmt As String
                        'If Not IsDBNull(totalDr("TotalBaseVatAmount")) Then
                        '    TotalBaseVatAmt = Convert.ToString(Math.Round(totalDr("TotalBaseVatAmount"), decPlaces))
                        'Else
                        '    TotalBaseVatAmt = ""
                        'End If
                        'phrase = New Phrase()
                        'phrase.Add(New Chunk(TotalBaseVatAmt, serviceFontBold))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.Border = Rectangle.BOTTOM_BORDER
                        'cell.PaddingBottom = 6.0F
                        'tblTotal.AddCell(cell)
                        'Dim TotalBaseGrandAmt As String
                        'If Not IsDBNull(totalDr("TotalBaseGrandAmount")) Then
                        '    TotalBaseGrandAmt = Convert.ToString(Math.Round(totalDr("TotalBaseGrandAmount"), decPlaces))
                        'Else
                        '    TotalBaseGrandAmt = ""
                        'End If
                        'phrase = New Phrase()
                        'phrase.Add(New Chunk(TotalBaseGrandAmt, serviceFontBold))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.Border = Rectangle.BOTTOM_BORDER Or Rectangle.RIGHT_BORDER
                        'cell.PaddingBottom = 6.0F
                        'tblTotal.AddCell(cell)
                        tblTotal.Complete = True
                        tblTotal.SpacingBefore = 3
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 72 Then document.NewPage()
                        document.Add(tblTotal)

                        Dim totGrandAmount As Decimal
                        If Not IsDBNull(totalDr("TotalgrandAmountbase")) Then
                            totGrandAmount = Math.Round(totalDr("totalgrandAmountbase"), decPlaces)
                            If Not IsDBNull(totalDr("Commission")) Then
                                totGrandAmount = totGrandAmount + Math.Round(totalDr("Commission"), decPlaces)
                            End If
                        Else
                            totGrandAmount = 0.0
                        End If

                        Dim str As String
                        If totGrandAmount Mod 1 > 0.0 Then
                            str = Convert.ToString(totGrandAmount)
                        Else
                            str = Convert.ToString(Math.Round(totGrandAmount))
                        End If
                        sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
                        mySqlCmd = New SqlCommand("select dbo.towords('" & str & "','','" & headerDr("salecurrcoin") & "')", sqlConn)
                        mySqlCmd.CommandType = CommandType.Text
                        Dim totalWord As String = Convert.ToString(mySqlCmd.ExecuteScalar())
                        mySqlCmd.Dispose()
                        If totalWord <> "" Then
                            totalWord = totalWord.Remove(0, 1)
                        End If
                        Dim BaseGrandAmt As Decimal
                        'If Not IsDBNull(totalDr("TotalBaseGrandAmount")) Then
                        '    BaseGrandAmt = Math.Round(totalDr("TotalBaseGrandAmount"), decPlaces)
                        'Else
                        '    BaseGrandAmt = 0.0
                        'End If
                        'Dim strBase As String
                        'If BaseGrandAmt Mod 1 > 0.0 Then
                        '    strBase = Convert.ToString(BaseGrandAmt)
                        'Else
                        '    strBase = Convert.ToString(Math.Round(BaseGrandAmt))
                        'End If

                        'mySqlCmd = New SqlCommand("select dbo.towords('" & strBase & "','','" & headerDr("basecurrcoin") & "')", sqlConn)
                        'mySqlCmd.CommandType = CommandType.Text
                        'Dim totalBaseWord As String = Convert.ToString(mySqlCmd.ExecuteScalar())
                        'mySqlCmd.Dispose()
                        'clsDBConnect.dbConnectionClose(sqlConn)
                        'If totalBaseWord <> "" Then
                        '    totalBaseWord = totalBaseWord.Remove(0, 1)
                        'End If
                        Dim tblWord As PdfPTable = New PdfPTable(1)
                        tblWord.SetWidths(New Single() {1.0F})
                        tblWord.TotalWidth = documentWidth
                        tblWord.LockedWidth = True
                        tblWord.Complete = False
                        tblWord.SplitRows = False
                        tblWord.KeepTogether = True
                        phrase = New Phrase()
                        phrase.Add(New Chunk("Amount(" & Convert.ToString(headerDr("salecurrcode")) & ")", NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER
                        cell.PaddingBottom = 3.0F
                        cell.PaddingTop = 6.0F
                        tblWord.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk(totalWord & " ONLY ", NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                        cell.PaddingBottom = 6.0F
                        cell.PaddingTop = 3.0F
                        cell.PaddingLeft = 15.0F
                        tblWord.AddCell(cell)

                        'phrase = New Phrase()
                        'phrase.Add(New Chunk("Amount(" & Convert.ToString(headerDr("basecurrcode")) & ")", NormalFontBold))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER
                        'cell.PaddingBottom = 3.0F
                        'tblWord.AddCell(cell)

                        'phrase = New Phrase()
                        'phrase.Add(New Chunk(totalBaseWord, NormalFontBold))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                        'cell.PaddingBottom = 6.0F
                        'cell.PaddingTop = 3.0F
                        'cell.PaddingLeft = 15.0F
                        'tblWord.AddCell(cell)

                        tblWord.Complete = True
                        tblWord.SpacingBefore = 7
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 72 Then document.NewPage()
                        document.Add(tblWord)
                    End If

                    Dim para As New Paragraph
                    para.Add(New Chunk("Remarks", NormalFontBold))
                    para.SpacingBefore = 7
                    para.Alignment =
                    document.Add(para)

                    If guestDt.Rows.Count > 0 Then
                        Dim tblGuest As PdfPTable = New PdfPTable(3)
                        tblGuest.SetWidths(New Single() {0.15F, 0.4F, 0.45})
                        tblGuest.TotalWidth = documentWidth
                        tblGuest.LockedWidth = True
                        tblGuest.Complete = False
                        tblGuest.SplitRows = False
                        tblGuest.HeaderRows = 1
                        Dim arrGuest() As String = {"Request ID", "Guest Name"}
                        For i = 0 To 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrGuest(i), NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            tblGuest.AddCell(cell)
                        Next
                        phrase = New Phrase()
                        phrase.Add(New Chunk("", NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 1.0F
                        tblGuest.AddCell(cell)

                        For Each guestDr As DataRow In guestDt.Rows
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(guestDr("requestId")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            tblGuest.AddCell(cell)
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(guestDr("guestName")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            tblGuest.AddCell(cell)
                            phrase = New Phrase()
                            phrase.Add(New Chunk("", NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            tblGuest.AddCell(cell)
                        Next
                        tblGuest.Complete = True
                        tblGuest.SpacingBefore = 7
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 72 Then document.NewPage()
                        document.Add(tblGuest)
                    End If
                    document.AddTitle(Convert.ToString(headerDr("printHeader")) & " - " & invoiceNo.Replace("/", "-"))
                    document.Close()

                    If printMode = "download" Then
                        Dim pagingFont As Font = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.GRAY)
                        Dim reader As New PdfReader(memoryStream.ToArray())
                        Using mStream As New MemoryStream()
                            Using stamper As New PdfStamper(reader, mStream)
                                Dim pages As Integer = reader.NumberOfPages
                                For i As Integer = 1 To pages
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, New Phrase("Date : " + Date.Today.ToString("dd/MM/yyyy") + "   " + DateTime.Now.ToString("HH:mm"), pagingFont), 50.0F, 20.0F, 0)
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 540.0F, 20.0F, 0)
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

    Public Sub InvoicePrint(ByVal invoiceNo As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "", Optional ByVal formatType As String = "")
        Try
            Dim decPlaces As Integer '= bjUtils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select option_selected from reservation_parameters where param_id='509'"))
            Dim HeaderAddressDt As New DataTable
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As SqlCommand
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("select option_selected from reservation_parameters where param_id='509'", sqlConn)
            mySqlCmd.CommandType = CommandType.Text
            decPlaces = Convert.ToInt32(mySqlCmd.ExecuteScalar())
            mySqlCmd.Dispose()
            mySqlCmd = New SqlCommand("select option_selected,option_value from reservation_parameters where param_id='5514'", sqlConn)
            mySqlCmd.CommandType = CommandType.Text
            Dim mysqlAdapter As New SqlDataAdapter()
            mysqlAdapter.SelectCommand = mySqlCmd
            mysqlAdapter.Fill(HeaderAddressDt)
            mySqlCmd.Dispose()

            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            mySqlCmd = New SqlCommand("sp_invoice_print", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@invoiceNo", SqlDbType.VarChar, 20)).Value = invoiceNo
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            ds1 = ds
            Dim headerDt As DataTable = ds.Tables(0)
            Dim serviceDt As DataTable = ds.Tables(1)
            Dim totalDt As DataTable = ds.Tables(2)
            Dim guestDt As DataTable = ds.Tables(3)
            Dim bankDt As DataTable = ds.Tables(4)

            clsDBConnect.dbConnectionClose(sqlConn)
            Dim remainingPageSpace As Single
            Dim decno As String
            Dim decimalPoint As String
            If headerDt.Rows.Count > 0 Then
                Dim sqlQry As String = "select nodigit from currmast where currcode= '" & headerDt.Rows(0)("salecurrcode") & "'"
                sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
                mySqlCmd = New SqlCommand(sqlQry, sqlConn)
                mySqlCmd.CommandType = CommandType.Text
                decno = Convert.ToInt32(mySqlCmd.ExecuteScalar())
                mySqlCmd.Dispose()
                clsDBConnect.dbConnectionClose(sqlConn)
                decimalPoint = "N" + decno

                Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 20.0F, 35.0F)
                Dim documentWidth As Single = 550.0F
                'Dim NormalFontRed As Font = FontFactory.GetFont(fontName, 11, Font.NORMAL, BaseColor.RED)
                'Dim NormalFontBoldRed As Font = FontFactory.GetFont(fontName, 11, Font.BOLD, BaseColor.RED)
                Dim TitleFont As Font = FontFactory.GetFont(fontName, 12, Font.NORMAL, BaseColor.BLACK)
                Dim TitleFontBold As Font = FontFactory.GetFont(fontName, 14, Font.BOLD, BaseColor.BLACK)
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
                    table.SetWidths(New Single() {0.64F, 0.36F})
                    table.Complete = False
                    table.SplitRows = False
                    Dim tblLogo As PdfPTable = New PdfPTable(3)
                    tblLogo.SetWidths(New Single() {0.01F, 0.29F, 0.7F})
                    cell = New PdfPCell()
                    cell.Border = Rectangle.NO_BORDER
                    cell.Rowspan = 2
                    tblLogo.AddCell(cell)
                    'Company Logo
                    If (headerDr("divcode") = "01") Then
                        cell = ImageCell("~/images/logo.jpg", 63.0F, PdfPCell.ALIGN_LEFT)
                    Else
                        cell = ImageCell("~/images/logo.jpg", 57.0F, PdfPCell.ALIGN_LEFT)
                    End If
                    cell.Rowspan = 2
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
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
                    Dim HeaderAddress1 As String = ""
                    Dim HeaderAddress2 As String = ""
                    If HeaderAddressDt.Rows.Count > 0 Then
                        HeaderAddress1 = Convert.ToString(HeaderAddressDt.Rows(0)("option_selected"))
                        HeaderAddress2 = Convert.ToString(HeaderAddressDt.Rows(0)("option_value"))
                    End If
                    phrase = New Phrase()
                    phrase.Add(New Chunk(HeaderAddress1 & vbCrLf & HeaderAddress2 & vbLf, NormalFont))   'Convert.ToString(headerDr("address1")
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 3.0F
                    tblLogo.AddCell(cell)
                    'cell = New PdfPCell()
                    'cell.Border = Rectangle.NO_BORDER
                    'cell.colspan = 2
                    'tblLogo.AddCell(cell)
                    cell = New PdfPCell(tblLogo)
                    cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER Or Rectangle.LEFT_BORDER
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    table.AddCell(cell)

                    Dim tblAddress As PdfPTable = New PdfPTable(3)
                    tblAddress.SetWidths(New Single() {0.45F, 0.05F, 0.5F})

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Tel", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("tel")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("P.O. Box", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("fax")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)

                    'phrase = New Phrase()
                    'phrase.Add(New Chunk("E-mail", serviceFontBold))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(11, 0)
                    'cell.PaddingBottom = 3
                    'tblAddress.AddCell(cell)
                    'phrase = New Phrase()
                    'phrase.Add(New Chunk(":", serviceFontBold))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.PaddingBottom = 3
                    'tblAddress.AddCell(cell)
                    'phrase = New Phrase()
                    'phrase.Add(New Chunk("", serviceFontBold))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(11, 0)
                    'cell.PaddingBottom = 3
                    'tblAddress.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Website", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("website")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Tax Registration No", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("trn")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    cell = New PdfPCell(tblAddress)
                    cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
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

                    Dim tblClient As PdfPTable = New PdfPTable(2)
                    tblClient.SetWidths(New Single() {0.66F, 0.34F})
                    tblClient.TotalWidth = documentWidth
                    tblClient.LockedWidth = True
                    tblClient.Complete = False
                    tblClient.SplitRows = False

                    Dim tblTo As PdfPTable = New PdfPTable(2)
                    tblTo.SetWidths(New Single() {0.05F, 0.95F})

                    phrase = New Phrase()
                    phrase.Add(New Chunk("To", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.PaddingBottom = 5.0F
                    tblTo.AddCell(cell)

                    Dim tblGuest As PdfPTable = New PdfPTable(3)
                    tblGuest.SetWidths(New Single() {0.27F, 0.03F, 0.7F})
                    tblGuest.Complete = False
                    tblGuest.SplitRows = False

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Client Name", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentName")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    Dim agentAddress As String = Convert.ToString(headerDr("agentadd1"))
                    If Convert.ToString(headerDr("agentadd2")) <> "" Then agentAddress = agentAddress + ", " + Convert.ToString(headerDr("agentadd2"))
                    If Convert.ToString(headerDr("agentadd3")) <> "" Then agentAddress = agentAddress + ", " + Convert.ToString(headerDr("agentadd3"))
                    phrase = New Phrase()
                    phrase.Add(New Chunk(agentAddress, serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Tel", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentTel")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Tax Registration No", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agenttrnno")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)

                    If guestDt.Rows.Count > 0 Then
                        phrase = New Phrase()
                        phrase.Add(New Chunk("Guest Name", serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        tblGuest.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk(":", serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 5.0F
                        tblGuest.AddCell(cell)

                        Dim gName As String = ""
                        For Each guestDr As DataRow In guestDt.Rows
                            If gName = "" Then
                                gName = Convert.ToString(guestDr("guestName"))
                            Else
                                gName = gName + vbCrLf + Convert.ToString(guestDr("guestName"))
                            End If
                        Next
                        phrase = New Phrase()
                        phrase.Add(New Chunk(gName, serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        tblGuest.AddCell(cell)
                        tblGuest.Complete = True
                    End If
                    cell = New PdfPCell(tblGuest)
                    cell.Border = Rectangle.NO_BORDER
                    cell.PaddingBottom = 5.0F
                    tblTo.AddCell(cell)

                    cell = New PdfPCell(tblTo)
                    cell.Border = Rectangle.NO_BORDER
                    tblClient.AddCell(cell)

                    Dim tblInvoice As PdfPTable = New PdfPTable(3)
                    tblInvoice.SetWidths(New Single() {0.4F, 0.03, 0.57F})

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Invoice No.", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("InvoiceNo")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Date", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("InvoiceDate")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Juniper Ref. No.", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("requestid")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Client Ref. No.", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentRef")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    cell = New PdfPCell(tblInvoice)
                    cell.Border = Rectangle.NO_BORDER
                    tblClient.AddCell(cell)
                    tblClient.AddCell(cell)
                    tblClient.SpacingBefore = 7
                    tblClient.Complete = True
                    document.Add(tblClient)

                    Dim serviceRowMinimumHeight As Single = 30.0F
                    Dim serviceTotalMinimumHeight As Single = 25.0F
                    Dim tblService As PdfPTable
                    If formatType = "F2" Then
                        tblService = New PdfPTable(8)
                        tblService.SetWidths(New Single() {0.05F, 0.1F, 0.1F, 0.07F, 0.32F, 0.12F, 0.12F, 0.12F})
                    Else
                        tblService = New PdfPTable(11)
                        tblService.SetWidths(New Single() {0.05F, 0.1F, 0.1F, 0.06F, 0.19F, 0.1F, 0.045F, 0.1F, 0.09F, 0.085F, 0.09F})
                    End If
                    tblService.TotalWidth = documentWidth
                    tblService.LockedWidth = True
                    tblService.HeaderRows = 1
                    tblService.Complete = False
                    tblService.SplitRows = False
                    Dim arrService() As String
                    If formatType = "F2" Then
                        arrService = {"Units", "Check In", "Check Out", "No. Of Nights", "Description", "Sales Before Tax", "VAT Amount", "Total Amount"}
                    Else
                        arrService = {"Units", "Check In", "Check Out", "No. Of Nights", "Description", "Sales Before Tax", "VAT %", "Nontaxable Amount", "Taxable Amount", "VAT Amount", "Total Amount"}
                    End If

                    For i = 0 To arrService.GetUpperBound(0)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrService(i), serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 1.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)
                    Next
                    Dim slno As Integer = 0
                    For Each dr As DataRow In serviceDt.Rows
                        slno = slno + 1
                        'phrase = New Phrase()
                        'phrase.Add(New Chunk(Convert.ToString(slno), serviceFont))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.PaddingBottom = 3.0F
                        'tblService.AddCell(cell)
                        'phrase = New Phrase()
                        'phrase.Add(New Chunk(Convert.ToString(dr("requestId")), serviceFont))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.PaddingBottom = 3.0F
                        'tblService.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(dr("units")), serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(dr("checkin")), serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(dr("checkout")), serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(dr("nights")), serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)
                        Dim servDescription As String = Convert.ToString(dr("particulars"))
                        If servDescription = "HANDLING FEES" Then
                            servDescription = "Handling Fees"
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(servDescription, serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)
                        Dim amt As String
                        If Not IsDBNull(dr("amount")) Then
                            amt = IIf(Decimal.Parse(dr("amount")) >= 0, Decimal.Parse(dr("amount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(dr("amount"))).ToString(decimalPoint) & ")")
                        Else
                            amt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(amt, serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)

                        If formatType = "" Then
                            Dim vatPerc As String
                            If Not IsDBNull(dr("vatperc")) Then
                                vatPerc = Convert.ToString(Math.Round(dr("vatperc"), 2))
                            Else
                                vatPerc = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(vatPerc, serviceFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            cell.MinimumHeight = serviceRowMinimumHeight
                            tblService.AddCell(cell)

                            Dim nonTax As String
                            If Not IsDBNull(dr("nontaxableAmount")) Then
                                nonTax = IIf(Decimal.Parse(dr("nontaxableAmount")) >= 0, Decimal.Parse(dr("nontaxableAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(dr("nontaxableAmount"))).ToString(decimalPoint) & ")")
                            Else
                                nonTax = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(nonTax, serviceFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            cell.MinimumHeight = serviceRowMinimumHeight
                            tblService.AddCell(cell)

                            Dim taxableAmt As String
                            If Not IsDBNull(dr("taxableAmount")) Then
                                taxableAmt = IIf(Decimal.Parse(dr("taxableAmount")) >= 0, Decimal.Parse(dr("taxableAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(dr("taxableAmount"))).ToString(decimalPoint) & ")")
                            Else
                                taxableAmt = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(taxableAmt, serviceFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            cell.MinimumHeight = serviceRowMinimumHeight
                            tblService.AddCell(cell)
                        End If
                        Dim vatAmt As String
                        If Not IsDBNull(dr("vatAmount")) Then
                            vatAmt = IIf(Decimal.Parse(dr("vatAmount")) >= 0, Decimal.Parse(dr("vatAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(dr("vatAmount"))).ToString(decimalPoint) & ")")
                        Else
                            vatAmt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(vatAmt, serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)
                        Dim grandAmt As String
                        If Not IsDBNull(dr("grandAmount")) Then
                            grandAmt = IIf(Decimal.Parse(dr("grandAmount")) >= 0, Decimal.Parse(dr("grandAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(dr("grandAmount"))).ToString(decimalPoint) & ")")
                        Else
                            grandAmt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(grandAmt, serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)
                    Next

                    tblService.Complete = True
                    tblService.SpacingBefore = 7
                    remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                    If remainingPageSpace < 72 Then document.NewPage()
                    document.Add(tblService)

                    If totalDt.Rows.Count > 0 Then
                        Dim totalDr As DataRow = totalDt.Rows(0)

                        Dim tblTotal As PdfPTable
                        If formatType = "F2" Then
                            tblTotal = New PdfPTable(5)
                            tblTotal.SetWidths(New Single() {0.47F, 0.17F, 0.12F, 0.12F, 0.12F})
                        Else
                            tblTotal = New PdfPTable(8)
                            tblTotal.SetWidths(New Single() {0.32F, 0.17F, 0.1F, 0.045F, 0.1F, 0.09F, 0.085F, 0.09F})
                        End If
                        tblTotal.TotalWidth = documentWidth
                        tblTotal.LockedWidth = True
                        tblTotal.Complete = False
                        tblTotal.SplitRows = False
                        tblTotal.KeepTogether = True

                        Dim lineFlag As Boolean = False
                        If Convert.ToString(headerDr("salecurrcode")) = Convert.ToString(headerDr("basecurrcode")) Then lineFlag = True

                        cell = New PdfPCell()
                        If lineFlag = True Then
                            cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                            cell.PaddingBottom = 6.0F
                        Else
                            cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER
                            cell.PaddingBottom = 3.0F
                        End If
                        cell.PaddingTop = 6.0F
                        cell.MinimumHeight = serviceTotalMinimumHeight
                        tblTotal.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk("Total(" & Convert.ToString(headerDr("salecurrcode")) & ")", serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        If lineFlag = True Then
                            cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                            cell.PaddingBottom = 6.0F
                        Else
                            cell.Border = Rectangle.TOP_BORDER
                            cell.PaddingBottom = 3.0F
                        End If
                        cell.PaddingTop = 6.0F
                        cell.MinimumHeight = serviceTotalMinimumHeight
                        tblTotal.AddCell(cell)
                        Dim TotalAmt As String
                        If Not IsDBNull(totalDr("TotalAmount")) Then
                            TotalAmt = IIf(Decimal.Parse(totalDr("Totalamount")) >= 0, Decimal.Parse(totalDr("Totalamount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("Totalamount"))).ToString(decimalPoint) & ")")
                        Else
                            TotalAmt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(TotalAmt, serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        If lineFlag = True Then
                            cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                            cell.PaddingBottom = 6.0F
                        Else
                            cell.Border = Rectangle.TOP_BORDER
                            cell.PaddingBottom = 3.0F
                        End If
                        cell.PaddingTop = 6.0F
                        cell.MinimumHeight = serviceTotalMinimumHeight
                        tblTotal.AddCell(cell)

                        If formatType = "" Then
                            phrase = New Phrase()
                            phrase.Add(New Chunk("", serviceFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            If lineFlag = True Then
                                cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                                cell.PaddingBottom = 6.0F
                            Else
                                cell.Border = Rectangle.TOP_BORDER
                                cell.PaddingBottom = 3.0F
                            End If

                            cell.PaddingTop = 6.0F
                            cell.MinimumHeight = serviceTotalMinimumHeight
                            tblTotal.AddCell(cell)

                            Dim TotalNontaxableAmount As String
                            If Not IsDBNull(totalDr("TotalNontaxableAmount")) Then
                                TotalNontaxableAmount = IIf(Decimal.Parse(totalDr("TotalNontaxableAmount")) >= 0, Decimal.Parse(totalDr("TotalNontaxableAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("TotalNontaxableAmount"))).ToString(decimalPoint) & ")")
                            Else
                                TotalNontaxableAmount = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(TotalNontaxableAmount, serviceFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            If lineFlag = True Then
                                cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                                cell.PaddingBottom = 6.0F
                            Else
                                cell.Border = Rectangle.TOP_BORDER
                                cell.PaddingBottom = 3.0F
                            End If
                            cell.PaddingTop = 6.0F
                            cell.MinimumHeight = serviceTotalMinimumHeight
                            tblTotal.AddCell(cell)

                            Dim TotalTaxableAmt As String
                            If Not IsDBNull(totalDr("TotalTaxableAmount")) Then
                                TotalTaxableAmt = IIf(Decimal.Parse(totalDr("TotalTaxableAmount")) >= 0, Decimal.Parse(totalDr("TotalTaxableAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("TotalTaxableAmount"))).ToString(decimalPoint) & ")")
                            Else
                                TotalTaxableAmt = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(TotalTaxableAmt, serviceFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            If lineFlag = True Then
                                cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                                cell.PaddingBottom = 6.0F
                            Else
                                cell.Border = Rectangle.TOP_BORDER
                                cell.PaddingBottom = 3.0F
                            End If
                            cell.PaddingTop = 6.0F
                            cell.MinimumHeight = serviceTotalMinimumHeight
                            tblTotal.AddCell(cell)
                        End If

                        Dim TotalVatAmt As String
                        If Not IsDBNull(totalDr("totalvatAmount")) Then
                            TotalVatAmt = IIf(Decimal.Parse(totalDr("totalvatAmount")) >= 0, Decimal.Parse(totalDr("totalvatAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("totalvatAmount"))).ToString(decimalPoint) & ")")
                        Else
                            TotalVatAmt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(TotalVatAmt, serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        If lineFlag = True Then
                            cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                            cell.PaddingBottom = 6.0F
                        Else
                            cell.Border = Rectangle.TOP_BORDER
                            cell.PaddingBottom = 3.0F
                        End If
                        cell.PaddingTop = 6.0F
                        cell.MinimumHeight = serviceTotalMinimumHeight
                        tblTotal.AddCell(cell)
                        Dim TotalgrandAmt As String
                        If Not IsDBNull(totalDr("TotalgrandAmount")) Then
                            TotalgrandAmt = IIf(Decimal.Parse(totalDr("totalgrandAmount")) >= 0, Decimal.Parse(totalDr("totalgrandAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("totalgrandAmount"))).ToString(decimalPoint) & ")")
                        Else
                            TotalgrandAmt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(TotalgrandAmt, serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 6.0F
                        If lineFlag = True Then
                            cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                            cell.PaddingBottom = 6.0F
                        Else
                            cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER
                            cell.PaddingBottom = 3.0F
                        End If
                        cell.MinimumHeight = serviceTotalMinimumHeight
                        tblTotal.AddCell(cell)

                        If Convert.ToString(headerDr("salecurrcode")) <> Convert.ToString(headerDr("basecurrcode")) Then
                            cell = New PdfPCell()
                            cell.Border = Rectangle.BOTTOM_BORDER Or Rectangle.LEFT_BORDER
                            cell.PaddingBottom = 6.0F
                            cell.MinimumHeight = serviceTotalMinimumHeight
                            tblTotal.AddCell(cell)

                            Dim convrate As String
                            If Not IsDBNull(headerDr("saleconvrate")) Then
                                convrate = Convert.ToString(Math.Round(headerDr("saleconvrate"), 4))
                            Else
                                convrate = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk("Total(" & Convert.ToString(headerDr("basecurrcode")) & ")@" & convrate, serviceFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.Border = Rectangle.BOTTOM_BORDER
                            cell.PaddingBottom = 6.0F
                            cell.MinimumHeight = serviceTotalMinimumHeight
                            tblTotal.AddCell(cell)
                            Dim TotalBaseAmt As String
                            If Not IsDBNull(totalDr("TotalBaseAmount")) Then
                                TotalBaseAmt = IIf(Decimal.Parse(totalDr("TotalBaseAmount")) >= 0, Decimal.Parse(totalDr("TotalBaseAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("TotalBaseAmount"))).ToString(decimalPoint) & ")")
                            Else
                                TotalBaseAmt = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(TotalBaseAmt, serviceFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.Border = Rectangle.BOTTOM_BORDER
                            cell.PaddingBottom = 6.0F
                            cell.MinimumHeight = serviceTotalMinimumHeight
                            tblTotal.AddCell(cell)

                            If formatType = "" Then
                                phrase = New Phrase()
                                phrase.Add(New Chunk("", serviceFontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                cell.Border = Rectangle.BOTTOM_BORDER
                                cell.PaddingBottom = 6.0F
                                cell.MinimumHeight = serviceTotalMinimumHeight
                                tblTotal.AddCell(cell)

                                Dim totalBaseNontaxableAmount As String
                                If Not IsDBNull(totalDr("TotalBaseTaxableAmount")) Then
                                    totalBaseNontaxableAmount = IIf(Decimal.Parse(totalDr("totalBaseNontaxableAmount")) >= 0, Decimal.Parse(totalDr("totalBaseNontaxableAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("totalBaseNontaxableAmount"))).ToString(decimalPoint) & ")")
                                Else
                                    totalBaseNontaxableAmount = ""
                                End If
                                phrase = New Phrase()
                                phrase.Add(New Chunk(totalBaseNontaxableAmount, serviceFontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                cell.Border = Rectangle.BOTTOM_BORDER
                                cell.PaddingBottom = 6.0F
                                cell.MinimumHeight = serviceTotalMinimumHeight
                                tblTotal.AddCell(cell)

                                Dim TotalBaseTaxableAmt As String
                                If Not IsDBNull(totalDr("TotalBaseTaxableAmount")) Then
                                    TotalBaseTaxableAmt = IIf(Decimal.Parse(totalDr("TotalBaseTaxableAmount")) >= 0, Decimal.Parse(totalDr("TotalBaseTaxableAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("TotalBaseTaxableAmount"))).ToString(decimalPoint) & ")")
                                Else
                                    TotalBaseTaxableAmt = ""
                                End If
                                phrase = New Phrase()
                                phrase.Add(New Chunk(TotalBaseTaxableAmt, serviceFontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                cell.Border = Rectangle.BOTTOM_BORDER
                                cell.PaddingBottom = 6.0F
                                cell.MinimumHeight = serviceTotalMinimumHeight
                                tblTotal.AddCell(cell)
                            End If

                            Dim TotalBaseVatAmt As String
                            If Not IsDBNull(totalDr("TotalBaseVatAmount")) Then
                                TotalBaseVatAmt = IIf(Decimal.Parse(totalDr("TotalBaseVatAmount")) >= 0, Decimal.Parse(totalDr("TotalBaseVatAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("TotalBaseVatAmount"))).ToString(decimalPoint) & ")")
                            Else
                                TotalBaseVatAmt = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(TotalBaseVatAmt, serviceFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.Border = Rectangle.BOTTOM_BORDER
                            cell.PaddingBottom = 6.0F
                            cell.MinimumHeight = serviceTotalMinimumHeight
                            tblTotal.AddCell(cell)
                            Dim TotalBaseGrandAmt As String
                            If Not IsDBNull(totalDr("TotalBaseGrandAmount")) Then
                                TotalBaseGrandAmt = IIf(Decimal.Parse(totalDr("TotalBaseGrandAmount")) >= 0, Decimal.Parse(totalDr("TotalBaseGrandAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("TotalBaseGrandAmount"))).ToString(decimalPoint) & ")")
                            Else
                                TotalBaseGrandAmt = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(TotalBaseGrandAmt, serviceFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.Border = Rectangle.BOTTOM_BORDER Or Rectangle.RIGHT_BORDER
                            cell.PaddingBottom = 6.0F
                            cell.MinimumHeight = serviceTotalMinimumHeight
                            tblTotal.AddCell(cell)
                        End If

                        tblTotal.Complete = True
                        tblTotal.SpacingBefore = 3
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 72 Then document.NewPage()
                        document.Add(tblTotal)

                        Dim totGrandAmount As Decimal
                        If Not IsDBNull(totalDr("TotalgrandAmount")) Then
                            totGrandAmount = Math.Round(totalDr("totalgrandAmount"), decPlaces)
                        Else
                            totGrandAmount = 0.0
                        End If

                        Dim str As String
                        If totGrandAmount Mod 1 > 0.0 Then
                            str = Convert.ToString(totGrandAmount)
                        Else
                            str = Convert.ToString(Math.Round(totGrandAmount))
                        End If
                        sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
                        mySqlCmd = New SqlCommand("select dbo.towords('" & str & "','','" & headerDr("salecurrcoin") & "')", sqlConn)
                        mySqlCmd.CommandType = CommandType.Text
                        Dim totalWord As String = Convert.ToString(mySqlCmd.ExecuteScalar())
                        mySqlCmd.Dispose()
                        If totalWord <> "" Then
                            totalWord = totalWord.Remove(0, 1)
                        End If
                        Dim BaseGrandAmt As Decimal
                        If Not IsDBNull(totalDr("TotalBaseGrandAmount")) Then
                            BaseGrandAmt = Math.Round(totalDr("TotalBaseGrandAmount"), decPlaces)
                        Else
                            BaseGrandAmt = 0.0
                        End If
                        Dim strBase As String
                        If BaseGrandAmt Mod 1 > 0.0 Then
                            strBase = Convert.ToString(BaseGrandAmt)
                        Else
                            strBase = Convert.ToString(Math.Round(BaseGrandAmt))
                        End If

                        mySqlCmd = New SqlCommand("select dbo.towords('" & strBase & "','','" & headerDr("basecurrcoin") & "')", sqlConn)
                        mySqlCmd.CommandType = CommandType.Text
                        Dim totalBaseWord As String = Convert.ToString(mySqlCmd.ExecuteScalar())
                        mySqlCmd.Dispose()
                        clsDBConnect.dbConnectionClose(sqlConn)
                        If totalBaseWord <> "" Then
                            totalBaseWord = totalBaseWord.Remove(0, 1)
                        End If
                        Dim tblWord As PdfPTable = New PdfPTable(1)
                        tblWord.SetWidths(New Single() {1.0F})
                        tblWord.TotalWidth = documentWidth
                        tblWord.LockedWidth = True
                        tblWord.Complete = False
                        tblWord.SplitRows = False
                        tblWord.KeepTogether = True
                        'phrase = New Phrase()
                        'phrase.Add(New Chunk("Amount(" & Convert.ToString(headerDr("salecurrcode")) & ")", NormalFontBold))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER
                        'cell.PaddingBottom = 3.0F
                        'cell.PaddingTop = 6.0F
                        'tblWord.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk("Amount(" & Convert.ToString(headerDr("salecurrcode")) & ") " & Trim(totalWord) & " ONLY", serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER 'Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER
                        cell.PaddingBottom = 6.0F
                        cell.PaddingTop = 3.0F
                        cell.PaddingLeft = 15.0F
                        tblWord.AddCell(cell)

                        'phrase = New Phrase()
                        'phrase.Add(New Chunk("Amount(" & Convert.ToString(headerDr("basecurrcode")) & ")", NormalFontBold))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER
                        'cell.PaddingBottom = 3.0F
                        'tblWord.AddCell(cell)

                        'phrase = New Phrase()
                        'phrase.Add(New Chunk(Trim(totalBaseWord) & " ONLY", NormalFontBold))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                        'cell.PaddingBottom = 6.0F
                        'cell.PaddingTop = 3.0F
                        'cell.PaddingLeft = 15.0F
                        'tblWord.AddCell(cell)

                        tblWord.Complete = True
                        tblWord.SpacingBefore = 7
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 72 Then document.NewPage()
                        document.Add(tblWord)
                    End If

                    Dim tblFinal As PdfPTable = New PdfPTable(1)
                    tblFinal.SetWidths(New Single() {1.0F})
                    tblFinal.TotalWidth = documentWidth
                    tblFinal.LockedWidth = True
                    tblFinal.Complete = False
                    tblFinal.SplitRows = False
                    tblFinal.KeepTogether = True

                    'Payment Bank Details
                    If bankDt.Rows.Count > 0 Then
                        Dim bankDr As DataRow = bankDt.Rows(0)
                        Dim tblBank As PdfPTable = New PdfPTable(3)
                        tblBank.SetWidths(New Single() {0.2F, 0.05, 0.75F})
                        tblBank.TotalWidth = documentWidth
                        tblBank.LockedWidth = True
                        tblBank.Complete = False
                        tblBank.SplitRows = False
                        tblBank.KeepTogether = True
                        phrase = New Phrase()
                        phrase.Add(New Chunk(" In case the selected payment type was a bank transfer, please perform the payment to the following account.", serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER
                        cell.PaddingBottom = 6.0F
                        cell.PaddingTop = 6.0F
                        cell.Colspan = 3
                        tblBank.AddCell(cell)

                        Dim arrBank() As String = {"Bank", Convert.ToString(bankDr("dispbankname")), "Bank Number", Convert.ToString(bankDr("accountnumber")),
                                                   "IBAN Code", Convert.ToString(bankDr("ibannumber")), "SWIFT Code", Convert.ToString(bankDr("swiftcode")),
                                                   "Bank Address", Convert.ToString(bankDr("branchname"))}

                        For i = 0 To arrBank.GetUpperBound(0)
                            If i Mod 2 = 0 Then
                                phrase = New Phrase()
                                phrase.Add(New Chunk(" " + arrBank(i), serviceFontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                If i + 1 = arrBank.GetUpperBound(0) Then
                                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                                Else
                                    cell.Border = Rectangle.LEFT_BORDER
                                End If
                                cell.PaddingBottom = 6.0F
                                'cell.PaddingTop = 6.0F
                                tblBank.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk(":", serviceFontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                If i + 1 = arrBank.GetUpperBound(0) Then
                                    cell.Border = Rectangle.BOTTOM_BORDER
                                End If
                                cell.PaddingBottom = 6.0F
                                'cell.PaddingTop = 6.0F
                                tblBank.AddCell(cell)
                            Else
                                phrase = New Phrase()
                                phrase.Add(New Chunk(arrBank(i), serviceFontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                If i = arrBank.GetUpperBound(0) Then
                                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                                Else
                                    cell.Border = Rectangle.RIGHT_BORDER
                                End If
                                cell.PaddingBottom = 6.0F
                                'cell.PaddingTop = 6.0F
                                tblBank.AddCell(cell)
                            End If
                        Next
                        tblBank.Complete = True

                        cell = New PdfPCell(tblBank)
                        cell.Border = Rectangle.NO_BORDER
                        tblFinal.AddCell(cell)

                    End If

                    cell = New PdfPCell()
                    cell.Border = Rectangle.NO_BORDER
                    cell.FixedHeight = 20.0F
                    tblFinal.AddCell(cell)

                    'Approval
                    Dim tblApprove = New PdfPTable(3)
                    tblApprove.TotalWidth = documentWidth
                    tblApprove.LockedWidth = True
                    tblApprove.SetWidths(New Single() {0.34F, 0.34F, 0.32F})
                    tblApprove.Complete = False
                    tblApprove.SplitRows = False
                    tblApprove.KeepTogether = True
                    Dim arr() As String = {"Prepared By", " Approved By", "Company Stamp"}
                    For i = 0 To arr.Length - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arr(i), serviceFontBold))

                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        cell.SetLeading(12, 0)
                        cell.PaddingBottom = 2.0F
                        tblApprove.AddCell(cell)
                    Next
                    cell = New PdfPCell()
                    cell.Colspan = 2
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.NO_BORDER
                    cell.PaddingTop = 5.0F
                    tblApprove.AddCell(cell)
                    cell = ImageCell("~/images/CompanyStamp.png", 70.0F, PdfPCell.ALIGN_LEFT)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingTop = 5.0F
                    tblApprove.AddCell(cell)
                    tblApprove.Complete = True

                    cell = New PdfPCell(tblApprove)
                    cell.Border = Rectangle.NO_BORDER
                    tblFinal.AddCell(cell)

                    tblFinal.SpacingBefore = 7.0F
                    remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                    If remainingPageSpace < 72 Then document.NewPage()
                    document.Add(tblFinal)

                    document.AddTitle(Convert.ToString(headerDr("printHeader")) & " - " & invoiceNo.Replace("/", "-"))
                    document.Close()

                    If printMode = "download" Then
                        Dim pagingFont As Font = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.GRAY)
                        Dim reader As New PdfReader(memoryStream.ToArray())
                        Using mStream As New MemoryStream()
                            Using stamper As New PdfStamper(reader, mStream)
                                Dim pages As Integer = reader.NumberOfPages
                                For i As Integer = 1 To pages
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, New Phrase("Date : " + Date.Today.ToString("dd/MM/yyyy") + "   " + DateTime.Now.ToString("HH:mm"), pagingFont), 50.0F, 20.0F, 0)
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 540.0F, 20.0F, 0)
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

    ' roslain 06/09/2023 CR point - 4 download all invoices into folder
    Public Sub InvoicePrint_Download(ByVal invoiceNo As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "", Optional ByVal formatType As String = "")
        Try
            Dim decPlaces As Integer '= bjUtils.ExecuteQueryReturnSingleValuenew("strDBConnection", "select option_selected from reservation_parameters where param_id='509'"))
            Dim HeaderAddressDt As New DataTable
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As SqlCommand
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("select option_selected from reservation_parameters where param_id='509'", sqlConn)
            mySqlCmd.CommandType = CommandType.Text
            decPlaces = Convert.ToInt32(mySqlCmd.ExecuteScalar())
            mySqlCmd.Dispose()
            mySqlCmd = New SqlCommand("select option_selected,option_value from reservation_parameters where param_id='5514'", sqlConn)
            mySqlCmd.CommandType = CommandType.Text
            Dim mysqlAdapter As New SqlDataAdapter()
            mysqlAdapter.SelectCommand = mySqlCmd
            mysqlAdapter.Fill(HeaderAddressDt)
            mySqlCmd.Dispose()

            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            mySqlCmd = New SqlCommand("sp_invoice_print", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@invoiceNo", SqlDbType.VarChar, 20)).Value = invoiceNo
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            ds1 = ds
            Dim headerDt As DataTable = ds.Tables(0)
            Dim serviceDt As DataTable = ds.Tables(1)
            Dim totalDt As DataTable = ds.Tables(2)
            Dim guestDt As DataTable = ds.Tables(3)
            Dim bankDt As DataTable = ds.Tables(4)

            clsDBConnect.dbConnectionClose(sqlConn)
            Dim remainingPageSpace As Single
            Dim decno As String
            Dim decimalPoint As String
            If headerDt.Rows.Count > 0 Then
                Dim sqlQry As String = "select nodigit from currmast where currcode= '" & headerDt.Rows(0)("salecurrcode") & "'"
                sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
                mySqlCmd = New SqlCommand(sqlQry, sqlConn)
                mySqlCmd.CommandType = CommandType.Text
                decno = Convert.ToInt32(mySqlCmd.ExecuteScalar())
                mySqlCmd.Dispose()
                clsDBConnect.dbConnectionClose(sqlConn)
                decimalPoint = "N" + decno

                Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 20.0F, 35.0F)
                Dim documentWidth As Single = 550.0F
                'Dim NormalFontRed As Font = FontFactory.GetFont(fontName, 11, Font.NORMAL, BaseColor.RED)
                'Dim NormalFontBoldRed As Font = FontFactory.GetFont(fontName, 11, Font.BOLD, BaseColor.RED)
                Dim TitleFont As Font = FontFactory.GetFont(fontName, 12, Font.NORMAL, BaseColor.BLACK)
                Dim TitleFontBold As Font = FontFactory.GetFont(fontName, 14, Font.BOLD, BaseColor.BLACK)
                Using memoryStream As New System.IO.MemoryStream()
                    Dim writer As PdfWriter
                    If printMode = "download" Then
                        writer = PdfWriter.GetInstance(document, memoryStream)
                    Else
                        ' Dim path As String = System.Web.HttpContext.Current.Server.MapPath("~\SavedReports\") + fileName
                        'Dim path As String = System.Web.HttpContext.Current.Server.MapPath("~\SavedReports\") + fileName
                        writer = PdfWriter.GetInstance(document, New FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
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
                    table.SetWidths(New Single() {0.64F, 0.36F})
                    table.Complete = False
                    table.SplitRows = False
                    Dim tblLogo As PdfPTable = New PdfPTable(3)
                    tblLogo.SetWidths(New Single() {0.01F, 0.29F, 0.7F})
                    cell = New PdfPCell()
                    cell.Border = Rectangle.NO_BORDER
                    cell.Rowspan = 2
                    tblLogo.AddCell(cell)
                    'Company Logo
                    If (headerDr("divcode") = "01") Then
                        cell = ImageCell("~/images/logo.jpg", 63.0F, PdfPCell.ALIGN_LEFT)
                    Else
                        cell = ImageCell("~/images/logo.jpg", 57.0F, PdfPCell.ALIGN_LEFT)
                    End If
                    cell.Rowspan = 2
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
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
                    Dim HeaderAddress1 As String = ""
                    Dim HeaderAddress2 As String = ""
                    If HeaderAddressDt.Rows.Count > 0 Then
                        HeaderAddress1 = Convert.ToString(HeaderAddressDt.Rows(0)("option_selected"))
                        HeaderAddress2 = Convert.ToString(HeaderAddressDt.Rows(0)("option_value"))
                    End If
                    phrase = New Phrase()
                    phrase.Add(New Chunk(HeaderAddress1 & vbCrLf & HeaderAddress2 & vbLf, NormalFont))   'Convert.ToString(headerDr("address1")
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 3.0F
                    tblLogo.AddCell(cell)
                    'cell = New PdfPCell()
                    'cell.Border = Rectangle.NO_BORDER
                    'cell.colspan = 2
                    'tblLogo.AddCell(cell)
                    cell = New PdfPCell(tblLogo)
                    cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER Or Rectangle.LEFT_BORDER
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    table.AddCell(cell)

                    Dim tblAddress As PdfPTable = New PdfPTable(3)
                    tblAddress.SetWidths(New Single() {0.45F, 0.05F, 0.5F})

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Tel", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("tel")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("P.O. Box", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("fax")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)

                    'phrase = New Phrase()
                    'phrase.Add(New Chunk("E-mail", serviceFontBold))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(11, 0)
                    'cell.PaddingBottom = 3
                    'tblAddress.AddCell(cell)
                    'phrase = New Phrase()
                    'phrase.Add(New Chunk(":", serviceFontBold))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.PaddingBottom = 3
                    'tblAddress.AddCell(cell)
                    'phrase = New Phrase()
                    'phrase.Add(New Chunk("", serviceFontBold))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(11, 0)
                    'cell.PaddingBottom = 3
                    'tblAddress.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Website", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("website")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Tax Registration No", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("trn")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(11, 0)
                    cell.PaddingBottom = 3
                    tblAddress.AddCell(cell)
                    cell = New PdfPCell(tblAddress)
                    cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
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

                    Dim tblClient As PdfPTable = New PdfPTable(2)
                    tblClient.SetWidths(New Single() {0.66F, 0.34F})
                    tblClient.TotalWidth = documentWidth
                    tblClient.LockedWidth = True
                    tblClient.Complete = False
                    tblClient.SplitRows = False

                    Dim tblTo As PdfPTable = New PdfPTable(2)
                    tblTo.SetWidths(New Single() {0.05F, 0.95F})

                    phrase = New Phrase()
                    phrase.Add(New Chunk("To", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.PaddingBottom = 5.0F
                    tblTo.AddCell(cell)

                    Dim tblGuest As PdfPTable = New PdfPTable(3)
                    tblGuest.SetWidths(New Single() {0.27F, 0.03F, 0.7F})
                    tblGuest.Complete = False
                    tblGuest.SplitRows = False

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Client Name", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentName")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    Dim agentAddress As String = Convert.ToString(headerDr("agentadd1"))
                    If Convert.ToString(headerDr("agentadd2")) <> "" Then agentAddress = agentAddress + ", " + Convert.ToString(headerDr("agentadd2"))
                    If Convert.ToString(headerDr("agentadd3")) <> "" Then agentAddress = agentAddress + ", " + Convert.ToString(headerDr("agentadd3"))
                    phrase = New Phrase()
                    phrase.Add(New Chunk(agentAddress, serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Tel", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentTel")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Tax Registration No", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agenttrnno")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblGuest.AddCell(cell)

                    If guestDt.Rows.Count > 0 Then
                        phrase = New Phrase()
                        phrase.Add(New Chunk("Guest Name", serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        tblGuest.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk(":", serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 5.0F
                        tblGuest.AddCell(cell)

                        Dim gName As String = ""
                        For Each guestDr As DataRow In guestDt.Rows
                            If gName = "" Then
                                gName = Convert.ToString(guestDr("guestName"))
                            Else
                                gName = gName + vbCrLf + Convert.ToString(guestDr("guestName"))
                            End If
                        Next
                        phrase = New Phrase()
                        phrase.Add(New Chunk(gName, serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        tblGuest.AddCell(cell)
                        tblGuest.Complete = True
                    End If
                    cell = New PdfPCell(tblGuest)
                    cell.Border = Rectangle.NO_BORDER
                    cell.PaddingBottom = 5.0F
                    tblTo.AddCell(cell)

                    cell = New PdfPCell(tblTo)
                    cell.Border = Rectangle.NO_BORDER
                    tblClient.AddCell(cell)

                    Dim tblInvoice As PdfPTable = New PdfPTable(3)
                    tblInvoice.SetWidths(New Single() {0.4F, 0.03, 0.57F})

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Invoice No.", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("InvoiceNo")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Date", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("InvoiceDate")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Juniper Ref. No.", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("requestid")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Client Ref. No.", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(":", serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentRef")), serviceFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 5.0F
                    tblInvoice.AddCell(cell)
                    cell = New PdfPCell(tblInvoice)
                    cell.Border = Rectangle.NO_BORDER
                    tblClient.AddCell(cell)
                    tblClient.AddCell(cell)
                    tblClient.SpacingBefore = 7
                    tblClient.Complete = True
                    document.Add(tblClient)

                    Dim serviceRowMinimumHeight As Single = 30.0F
                    Dim serviceTotalMinimumHeight As Single = 25.0F
                    Dim tblService As PdfPTable
                    If formatType = "F2" Then
                        tblService = New PdfPTable(8)
                        tblService.SetWidths(New Single() {0.05F, 0.1F, 0.1F, 0.07F, 0.32F, 0.12F, 0.12F, 0.12F})
                    Else
                        tblService = New PdfPTable(11)
                        tblService.SetWidths(New Single() {0.05F, 0.1F, 0.1F, 0.06F, 0.19F, 0.1F, 0.045F, 0.1F, 0.09F, 0.085F, 0.09F})
                    End If
                    tblService.TotalWidth = documentWidth
                    tblService.LockedWidth = True
                    tblService.HeaderRows = 1
                    tblService.Complete = False
                    tblService.SplitRows = False
                    Dim arrService() As String
                    If formatType = "F2" Then
                        arrService = {"Units", "Check In", "Check Out", "No. Of Nights", "Description", "Sales Before Tax", "VAT Amount", "Total Amount"}
                    Else
                        arrService = {"Units", "Check In", "Check Out", "No. Of Nights", "Description", "Sales Before Tax", "VAT %", "Nontaxable Amount", "Taxable Amount", "VAT Amount", "Total Amount"}
                    End If

                    For i = 0 To arrService.GetUpperBound(0)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrService(i), serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 1.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)
                    Next
                    Dim slno As Integer = 0
                    For Each dr As DataRow In serviceDt.Rows
                        slno = slno + 1
                        'phrase = New Phrase()
                        'phrase.Add(New Chunk(Convert.ToString(slno), serviceFont))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.PaddingBottom = 3.0F
                        'tblService.AddCell(cell)
                        'phrase = New Phrase()
                        'phrase.Add(New Chunk(Convert.ToString(dr("requestId")), serviceFont))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.PaddingBottom = 3.0F
                        'tblService.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(dr("units")), serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(dr("checkin")), serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(dr("checkout")), serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(dr("nights")), serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)
                        Dim servDescription As String = Convert.ToString(dr("particulars"))
                        If servDescription = "HANDLING FEES" Then
                            servDescription = "Handling Fees"
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(servDescription, serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)
                        Dim amt As String
                        If Not IsDBNull(dr("amount")) Then
                            amt = IIf(Decimal.Parse(dr("amount")) >= 0, Decimal.Parse(dr("amount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(dr("amount"))).ToString(decimalPoint) & ")")
                        Else
                            amt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(amt, serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)

                        If formatType = "" Then
                            Dim vatPerc As String
                            If Not IsDBNull(dr("vatperc")) Then
                                vatPerc = Convert.ToString(Math.Round(dr("vatperc"), 2))
                            Else
                                vatPerc = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(vatPerc, serviceFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            cell.MinimumHeight = serviceRowMinimumHeight
                            tblService.AddCell(cell)

                            Dim nonTax As String
                            If Not IsDBNull(dr("nontaxableAmount")) Then
                                nonTax = IIf(Decimal.Parse(dr("nontaxableAmount")) >= 0, Decimal.Parse(dr("nontaxableAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(dr("nontaxableAmount"))).ToString(decimalPoint) & ")")
                            Else
                                nonTax = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(nonTax, serviceFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            cell.MinimumHeight = serviceRowMinimumHeight
                            tblService.AddCell(cell)

                            Dim taxableAmt As String
                            If Not IsDBNull(dr("taxableAmount")) Then
                                taxableAmt = IIf(Decimal.Parse(dr("taxableAmount")) >= 0, Decimal.Parse(dr("taxableAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(dr("taxableAmount"))).ToString(decimalPoint) & ")")
                            Else
                                taxableAmt = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(taxableAmt, serviceFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            cell.MinimumHeight = serviceRowMinimumHeight
                            tblService.AddCell(cell)
                        End If
                        Dim vatAmt As String
                        If Not IsDBNull(dr("vatAmount")) Then
                            vatAmt = IIf(Decimal.Parse(dr("vatAmount")) >= 0, Decimal.Parse(dr("vatAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(dr("vatAmount"))).ToString(decimalPoint) & ")")
                        Else
                            vatAmt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(vatAmt, serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)
                        Dim grandAmt As String
                        If Not IsDBNull(dr("grandAmount")) Then
                            grandAmt = IIf(Decimal.Parse(dr("grandAmount")) >= 0, Decimal.Parse(dr("grandAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(dr("grandAmount"))).ToString(decimalPoint) & ")")
                        Else
                            grandAmt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(grandAmt, serviceFont))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.MinimumHeight = serviceRowMinimumHeight
                        tblService.AddCell(cell)
                    Next

                    tblService.Complete = True
                    tblService.SpacingBefore = 7
                    remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                    If remainingPageSpace < 72 Then document.NewPage()
                    document.Add(tblService)

                    If totalDt.Rows.Count > 0 Then
                        Dim totalDr As DataRow = totalDt.Rows(0)

                        Dim tblTotal As PdfPTable
                        If formatType = "F2" Then
                            tblTotal = New PdfPTable(5)
                            tblTotal.SetWidths(New Single() {0.47F, 0.17F, 0.12F, 0.12F, 0.12F})
                        Else
                            tblTotal = New PdfPTable(8)
                            tblTotal.SetWidths(New Single() {0.32F, 0.17F, 0.1F, 0.045F, 0.1F, 0.09F, 0.085F, 0.09F})
                        End If
                        tblTotal.TotalWidth = documentWidth
                        tblTotal.LockedWidth = True
                        tblTotal.Complete = False
                        tblTotal.SplitRows = False
                        tblTotal.KeepTogether = True

                        Dim lineFlag As Boolean = False
                        If Convert.ToString(headerDr("salecurrcode")) = Convert.ToString(headerDr("basecurrcode")) Then lineFlag = True

                        cell = New PdfPCell()
                        If lineFlag = True Then
                            cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                            cell.PaddingBottom = 6.0F
                        Else
                            cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER
                            cell.PaddingBottom = 3.0F
                        End If
                        cell.PaddingTop = 6.0F
                        cell.MinimumHeight = serviceTotalMinimumHeight
                        tblTotal.AddCell(cell)
                        phrase = New Phrase()
                        phrase.Add(New Chunk("Total(" & Convert.ToString(headerDr("salecurrcode")) & ")", serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        If lineFlag = True Then
                            cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                            cell.PaddingBottom = 6.0F
                        Else
                            cell.Border = Rectangle.TOP_BORDER
                            cell.PaddingBottom = 3.0F
                        End If
                        cell.PaddingTop = 6.0F
                        cell.MinimumHeight = serviceTotalMinimumHeight
                        tblTotal.AddCell(cell)
                        Dim TotalAmt As String
                        If Not IsDBNull(totalDr("TotalAmount")) Then
                            TotalAmt = IIf(Decimal.Parse(totalDr("Totalamount")) >= 0, Decimal.Parse(totalDr("Totalamount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("Totalamount"))).ToString(decimalPoint) & ")")
                        Else
                            TotalAmt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(TotalAmt, serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        If lineFlag = True Then
                            cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                            cell.PaddingBottom = 6.0F
                        Else
                            cell.Border = Rectangle.TOP_BORDER
                            cell.PaddingBottom = 3.0F
                        End If
                        cell.PaddingTop = 6.0F
                        cell.MinimumHeight = serviceTotalMinimumHeight
                        tblTotal.AddCell(cell)

                        If formatType = "" Then
                            phrase = New Phrase()
                            phrase.Add(New Chunk("", serviceFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            If lineFlag = True Then
                                cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                                cell.PaddingBottom = 6.0F
                            Else
                                cell.Border = Rectangle.TOP_BORDER
                                cell.PaddingBottom = 3.0F
                            End If

                            cell.PaddingTop = 6.0F
                            cell.MinimumHeight = serviceTotalMinimumHeight
                            tblTotal.AddCell(cell)

                            Dim TotalNontaxableAmount As String
                            If Not IsDBNull(totalDr("TotalNontaxableAmount")) Then
                                TotalNontaxableAmount = IIf(Decimal.Parse(totalDr("TotalNontaxableAmount")) >= 0, Decimal.Parse(totalDr("TotalNontaxableAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("TotalNontaxableAmount"))).ToString(decimalPoint) & ")")
                            Else
                                TotalNontaxableAmount = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(TotalNontaxableAmount, serviceFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            If lineFlag = True Then
                                cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                                cell.PaddingBottom = 6.0F
                            Else
                                cell.Border = Rectangle.TOP_BORDER
                                cell.PaddingBottom = 3.0F
                            End If
                            cell.PaddingTop = 6.0F
                            cell.MinimumHeight = serviceTotalMinimumHeight
                            tblTotal.AddCell(cell)

                            Dim TotalTaxableAmt As String
                            If Not IsDBNull(totalDr("TotalTaxableAmount")) Then
                                TotalTaxableAmt = IIf(Decimal.Parse(totalDr("TotalTaxableAmount")) >= 0, Decimal.Parse(totalDr("TotalTaxableAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("TotalTaxableAmount"))).ToString(decimalPoint) & ")")
                            Else
                                TotalTaxableAmt = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(TotalTaxableAmt, serviceFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            If lineFlag = True Then
                                cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                                cell.PaddingBottom = 6.0F
                            Else
                                cell.Border = Rectangle.TOP_BORDER
                                cell.PaddingBottom = 3.0F
                            End If
                            cell.PaddingTop = 6.0F
                            cell.MinimumHeight = serviceTotalMinimumHeight
                            tblTotal.AddCell(cell)
                        End If

                        Dim TotalVatAmt As String
                        If Not IsDBNull(totalDr("totalvatAmount")) Then
                            TotalVatAmt = IIf(Decimal.Parse(totalDr("totalvatAmount")) >= 0, Decimal.Parse(totalDr("totalvatAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("totalvatAmount"))).ToString(decimalPoint) & ")")
                        Else
                            TotalVatAmt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(TotalVatAmt, serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        If lineFlag = True Then
                            cell.Border = Rectangle.TOP_BORDER Or Rectangle.BOTTOM_BORDER
                            cell.PaddingBottom = 6.0F
                        Else
                            cell.Border = Rectangle.TOP_BORDER
                            cell.PaddingBottom = 3.0F
                        End If
                        cell.PaddingTop = 6.0F
                        cell.MinimumHeight = serviceTotalMinimumHeight
                        tblTotal.AddCell(cell)
                        Dim TotalgrandAmt As String
                        If Not IsDBNull(totalDr("TotalgrandAmount")) Then
                            TotalgrandAmt = IIf(Decimal.Parse(totalDr("totalgrandAmount")) >= 0, Decimal.Parse(totalDr("totalgrandAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("totalgrandAmount"))).ToString(decimalPoint) & ")")
                        Else
                            TotalgrandAmt = ""
                        End If
                        phrase = New Phrase()
                        phrase.Add(New Chunk(TotalgrandAmt, serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingTop = 6.0F
                        If lineFlag = True Then
                            cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                            cell.PaddingBottom = 6.0F
                        Else
                            cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER
                            cell.PaddingBottom = 3.0F
                        End If
                        cell.MinimumHeight = serviceTotalMinimumHeight
                        tblTotal.AddCell(cell)

                        If Convert.ToString(headerDr("salecurrcode")) <> Convert.ToString(headerDr("basecurrcode")) Then
                            cell = New PdfPCell()
                            cell.Border = Rectangle.BOTTOM_BORDER Or Rectangle.LEFT_BORDER
                            cell.PaddingBottom = 6.0F
                            cell.MinimumHeight = serviceTotalMinimumHeight
                            tblTotal.AddCell(cell)

                            Dim convrate As String
                            If Not IsDBNull(headerDr("saleconvrate")) Then
                                convrate = Convert.ToString(Math.Round(headerDr("saleconvrate"), 4))
                            Else
                                convrate = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk("Total(" & Convert.ToString(headerDr("basecurrcode")) & ")@" & convrate, serviceFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.Border = Rectangle.BOTTOM_BORDER
                            cell.PaddingBottom = 6.0F
                            cell.MinimumHeight = serviceTotalMinimumHeight
                            tblTotal.AddCell(cell)
                            Dim TotalBaseAmt As String
                            If Not IsDBNull(totalDr("TotalBaseAmount")) Then
                                TotalBaseAmt = IIf(Decimal.Parse(totalDr("TotalBaseAmount")) >= 0, Decimal.Parse(totalDr("TotalBaseAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("TotalBaseAmount"))).ToString(decimalPoint) & ")")
                            Else
                                TotalBaseAmt = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(TotalBaseAmt, serviceFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.Border = Rectangle.BOTTOM_BORDER
                            cell.PaddingBottom = 6.0F
                            cell.MinimumHeight = serviceTotalMinimumHeight
                            tblTotal.AddCell(cell)

                            If formatType = "" Then
                                phrase = New Phrase()
                                phrase.Add(New Chunk("", serviceFontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                cell.Border = Rectangle.BOTTOM_BORDER
                                cell.PaddingBottom = 6.0F
                                cell.MinimumHeight = serviceTotalMinimumHeight
                                tblTotal.AddCell(cell)

                                Dim totalBaseNontaxableAmount As String
                                If Not IsDBNull(totalDr("TotalBaseTaxableAmount")) Then
                                    totalBaseNontaxableAmount = IIf(Decimal.Parse(totalDr("totalBaseNontaxableAmount")) >= 0, Decimal.Parse(totalDr("totalBaseNontaxableAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("totalBaseNontaxableAmount"))).ToString(decimalPoint) & ")")
                                Else
                                    totalBaseNontaxableAmount = ""
                                End If
                                phrase = New Phrase()
                                phrase.Add(New Chunk(totalBaseNontaxableAmount, serviceFontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                cell.Border = Rectangle.BOTTOM_BORDER
                                cell.PaddingBottom = 6.0F
                                cell.MinimumHeight = serviceTotalMinimumHeight
                                tblTotal.AddCell(cell)

                                Dim TotalBaseTaxableAmt As String
                                If Not IsDBNull(totalDr("TotalBaseTaxableAmount")) Then
                                    TotalBaseTaxableAmt = IIf(Decimal.Parse(totalDr("TotalBaseTaxableAmount")) >= 0, Decimal.Parse(totalDr("TotalBaseTaxableAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("TotalBaseTaxableAmount"))).ToString(decimalPoint) & ")")
                                Else
                                    TotalBaseTaxableAmt = ""
                                End If
                                phrase = New Phrase()
                                phrase.Add(New Chunk(TotalBaseTaxableAmt, serviceFontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                cell.Border = Rectangle.BOTTOM_BORDER
                                cell.PaddingBottom = 6.0F
                                cell.MinimumHeight = serviceTotalMinimumHeight
                                tblTotal.AddCell(cell)
                            End If

                            Dim TotalBaseVatAmt As String
                            If Not IsDBNull(totalDr("TotalBaseVatAmount")) Then
                                TotalBaseVatAmt = IIf(Decimal.Parse(totalDr("TotalBaseVatAmount")) >= 0, Decimal.Parse(totalDr("TotalBaseVatAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("TotalBaseVatAmount"))).ToString(decimalPoint) & ")")
                            Else
                                TotalBaseVatAmt = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(TotalBaseVatAmt, serviceFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.Border = Rectangle.BOTTOM_BORDER
                            cell.PaddingBottom = 6.0F
                            cell.MinimumHeight = serviceTotalMinimumHeight
                            tblTotal.AddCell(cell)
                            Dim TotalBaseGrandAmt As String
                            If Not IsDBNull(totalDr("TotalBaseGrandAmount")) Then
                                TotalBaseGrandAmt = IIf(Decimal.Parse(totalDr("TotalBaseGrandAmount")) >= 0, Decimal.Parse(totalDr("TotalBaseGrandAmount")).ToString(decimalPoint), "(" & Math.Abs(Decimal.Parse(totalDr("TotalBaseGrandAmount"))).ToString(decimalPoint) & ")")
                            Else
                                TotalBaseGrandAmt = ""
                            End If
                            phrase = New Phrase()
                            phrase.Add(New Chunk(TotalBaseGrandAmt, serviceFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.Border = Rectangle.BOTTOM_BORDER Or Rectangle.RIGHT_BORDER
                            cell.PaddingBottom = 6.0F
                            cell.MinimumHeight = serviceTotalMinimumHeight
                            tblTotal.AddCell(cell)
                        End If

                        tblTotal.Complete = True
                        tblTotal.SpacingBefore = 3
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 72 Then document.NewPage()
                        document.Add(tblTotal)

                        Dim totGrandAmount As Decimal
                        If Not IsDBNull(totalDr("TotalgrandAmount")) Then
                            totGrandAmount = Math.Round(totalDr("totalgrandAmount"), decPlaces)
                        Else
                            totGrandAmount = 0.0
                        End If

                        Dim str As String
                        If totGrandAmount Mod 1 > 0.0 Then
                            str = Convert.ToString(totGrandAmount)
                        Else
                            str = Convert.ToString(Math.Round(totGrandAmount))
                        End If
                        sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
                        mySqlCmd = New SqlCommand("select dbo.towords('" & str & "','','" & headerDr("salecurrcoin") & "')", sqlConn)
                        mySqlCmd.CommandType = CommandType.Text
                        Dim totalWord As String = Convert.ToString(mySqlCmd.ExecuteScalar())
                        mySqlCmd.Dispose()
                        If totalWord <> "" Then
                            totalWord = totalWord.Remove(0, 1)
                        End If
                        Dim BaseGrandAmt As Decimal
                        If Not IsDBNull(totalDr("TotalBaseGrandAmount")) Then
                            BaseGrandAmt = Math.Round(totalDr("TotalBaseGrandAmount"), decPlaces)
                        Else
                            BaseGrandAmt = 0.0
                        End If
                        Dim strBase As String
                        If BaseGrandAmt Mod 1 > 0.0 Then
                            strBase = Convert.ToString(BaseGrandAmt)
                        Else
                            strBase = Convert.ToString(Math.Round(BaseGrandAmt))
                        End If

                        mySqlCmd = New SqlCommand("select dbo.towords('" & strBase & "','','" & headerDr("basecurrcoin") & "')", sqlConn)
                        mySqlCmd.CommandType = CommandType.Text
                        Dim totalBaseWord As String = Convert.ToString(mySqlCmd.ExecuteScalar())
                        mySqlCmd.Dispose()
                        clsDBConnect.dbConnectionClose(sqlConn)
                        If totalBaseWord <> "" Then
                            totalBaseWord = totalBaseWord.Remove(0, 1)
                        End If
                        Dim tblWord As PdfPTable = New PdfPTable(1)
                        tblWord.SetWidths(New Single() {1.0F})
                        tblWord.TotalWidth = documentWidth
                        tblWord.LockedWidth = True
                        tblWord.Complete = False
                        tblWord.SplitRows = False
                        tblWord.KeepTogether = True
                        'phrase = New Phrase()
                        'phrase.Add(New Chunk("Amount(" & Convert.ToString(headerDr("salecurrcode")) & ")", NormalFontBold))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER
                        'cell.PaddingBottom = 3.0F
                        'cell.PaddingTop = 6.0F
                        'tblWord.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk("Amount(" & Convert.ToString(headerDr("salecurrcode")) & ") " & Trim(totalWord) & " ONLY", serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER 'Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER
                        cell.PaddingBottom = 6.0F
                        cell.PaddingTop = 3.0F
                        cell.PaddingLeft = 15.0F
                        tblWord.AddCell(cell)

                        'phrase = New Phrase()
                        'phrase.Add(New Chunk("Amount(" & Convert.ToString(headerDr("basecurrcode")) & ")", NormalFontBold))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER
                        'cell.PaddingBottom = 3.0F
                        'tblWord.AddCell(cell)

                        'phrase = New Phrase()
                        'phrase.Add(New Chunk(Trim(totalBaseWord) & " ONLY", NormalFontBold))
                        'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        'cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                        'cell.PaddingBottom = 6.0F
                        'cell.PaddingTop = 3.0F
                        'cell.PaddingLeft = 15.0F
                        'tblWord.AddCell(cell)

                        tblWord.Complete = True
                        tblWord.SpacingBefore = 7
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 72 Then document.NewPage()
                        document.Add(tblWord)
                    End If

                    Dim tblFinal As PdfPTable = New PdfPTable(1)
                    tblFinal.SetWidths(New Single() {1.0F})
                    tblFinal.TotalWidth = documentWidth
                    tblFinal.LockedWidth = True
                    tblFinal.Complete = False
                    tblFinal.SplitRows = False
                    tblFinal.KeepTogether = True

                    'Payment Bank Details
                    If bankDt.Rows.Count > 0 Then
                        Dim bankDr As DataRow = bankDt.Rows(0)
                        Dim tblBank As PdfPTable = New PdfPTable(3)
                        tblBank.SetWidths(New Single() {0.2F, 0.05, 0.75F})
                        tblBank.TotalWidth = documentWidth
                        tblBank.LockedWidth = True
                        tblBank.Complete = False
                        tblBank.SplitRows = False
                        tblBank.KeepTogether = True
                        phrase = New Phrase()
                        phrase.Add(New Chunk(" In case the selected payment type was a bank transfer, please perform the payment to the following account.", serviceFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER
                        cell.PaddingBottom = 6.0F
                        cell.PaddingTop = 6.0F
                        cell.Colspan = 3
                        tblBank.AddCell(cell)

                        Dim arrBank() As String = {"Bank", Convert.ToString(bankDr("dispbankname")), "Bank Number", Convert.ToString(bankDr("accountnumber")),
                                                   "IBAN Code", Convert.ToString(bankDr("ibannumber")), "SWIFT Code", Convert.ToString(bankDr("swiftcode")),
                                                   "Bank Address", Convert.ToString(bankDr("branchname"))}

                        For i = 0 To arrBank.GetUpperBound(0)
                            If i Mod 2 = 0 Then
                                phrase = New Phrase()
                                phrase.Add(New Chunk(" " + arrBank(i), serviceFontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                If i + 1 = arrBank.GetUpperBound(0) Then
                                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                                Else
                                    cell.Border = Rectangle.LEFT_BORDER
                                End If
                                cell.PaddingBottom = 6.0F
                                'cell.PaddingTop = 6.0F
                                tblBank.AddCell(cell)

                                phrase = New Phrase()
                                phrase.Add(New Chunk(":", serviceFontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                If i + 1 = arrBank.GetUpperBound(0) Then
                                    cell.Border = Rectangle.BOTTOM_BORDER
                                End If
                                cell.PaddingBottom = 6.0F
                                'cell.PaddingTop = 6.0F
                                tblBank.AddCell(cell)
                            Else
                                phrase = New Phrase()
                                phrase.Add(New Chunk(arrBank(i), serviceFontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                If i = arrBank.GetUpperBound(0) Then
                                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                                Else
                                    cell.Border = Rectangle.RIGHT_BORDER
                                End If
                                cell.PaddingBottom = 6.0F
                                'cell.PaddingTop = 6.0F
                                tblBank.AddCell(cell)
                            End If
                        Next
                        tblBank.Complete = True

                        cell = New PdfPCell(tblBank)
                        cell.Border = Rectangle.NO_BORDER
                        tblFinal.AddCell(cell)

                    End If

                    cell = New PdfPCell()
                    cell.Border = Rectangle.NO_BORDER
                    cell.FixedHeight = 20.0F
                    tblFinal.AddCell(cell)

                    'Approval
                    Dim tblApprove = New PdfPTable(3)
                    tblApprove.TotalWidth = documentWidth
                    tblApprove.LockedWidth = True
                    tblApprove.SetWidths(New Single() {0.34F, 0.34F, 0.32F})
                    tblApprove.Complete = False
                    tblApprove.SplitRows = False
                    tblApprove.KeepTogether = True
                    Dim arr() As String = {"Prepared By", " Approved By", "Company Stamp"}
                    For i = 0 To arr.Length - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arr(i), serviceFontBold))

                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        cell.SetLeading(12, 0)
                        cell.PaddingBottom = 2.0F
                        tblApprove.AddCell(cell)
                    Next
                    cell = New PdfPCell()
                    cell.Colspan = 2
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.NO_BORDER
                    cell.PaddingTop = 5.0F
                    tblApprove.AddCell(cell)
                    cell = ImageCell("~/images/CompanyStamp.png", 70.0F, PdfPCell.ALIGN_LEFT)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingTop = 5.0F
                    tblApprove.AddCell(cell)
                    tblApprove.Complete = True

                    cell = New PdfPCell(tblApprove)
                    cell.Border = Rectangle.NO_BORDER
                    tblFinal.AddCell(cell)

                    tblFinal.SpacingBefore = 7.0F
                    remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                    If remainingPageSpace < 72 Then document.NewPage()
                    document.Add(tblFinal)

                    document.AddTitle(Convert.ToString(headerDr("printHeader")) & " - " & invoiceNo.Replace("/", "-"))
                    document.Close()

                    If printMode = "download" Then
                        Dim pagingFont As Font = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.GRAY)
                        Dim reader As New PdfReader(memoryStream.ToArray())
                        Using mStream As New MemoryStream()
                            Using stamper As New PdfStamper(reader, mStream)
                                Dim pages As Integer = reader.NumberOfPages
                                For i As Integer = 1 To pages
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, New Phrase("Date : " + Date.Today.ToString("dd/MM/yyyy") + "   " + DateTime.Now.ToString("HH:mm"), pagingFont), 50.0F, 20.0F, 0)
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 540.0F, 20.0F, 0)
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
