Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Configuration
Imports System.Threading
Imports System.Globalization
Imports System.Diagnostics
Imports ClosedXML.Excel

Public Class clsBookingServicerVoucher
    Dim objclsUtilities As New clsUtils

    '*** Danny 25/09/2018
#Region "Private Shared Function PhraseCell_SR(phrase As Phrase, align As Integer, Cols As Integer, celBorder As Boolean, Optional celBottomBorder As String = ""None"") As PdfPCell"
    Private Shared Function PhraseCell_SR(ByVal phrase As Phrase, ByVal align As Integer, ByVal Cols As Integer, ByVal celBorder As Boolean, Optional ByVal celBottomBorder As String = "None") As PdfPCell
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
    Private Shared Function ImageCell_SR(ByVal path As String, ByVal scale As Single, ByVal align As Integer) As PdfPCell
        Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path))

        Dim lWidth As Double, lHeight As Double
        If image.Height > scale Then
            lHeight = scale
            lWidth = image.Width / image.Height * lHeight
        ElseIf image.Height <= 0 Or image.Width <= 0 Then
            lHeight = scale
            lWidth = scale
        Else
            lHeight = image.Height
            lWidth = image.Width / image.Height * lHeight
        End If
        'image.ScaleAbsolute(scale, scale) 'changed by mohamed on 19/12/2018
        image.ScaleAbsolute(lWidth, lHeight)

        Dim cell As New PdfPCell(image)
        cell.BorderColor = BaseColor.WHITE
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 0.0F
        cell.PaddingTop = 0.0F
        Return cell
    End Function
#End Region
#Region "Global Variable"
    Dim SmallFont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
    Dim NormalFont_SR As Font = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK)
    Dim NormalFontBold_SR As Font = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK)
    Dim NormalFontBoldRed_SR As Font = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.RED)
    Dim NormalFontBoldTax_SR As Font = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)
    Dim titleColor_SR As BaseColor = New BaseColor(214, 214, 214)
#End Region
#Region "Protected Sub GuestList(ByRef tblGuest As PdfPTable, ByVal guestDt As DataTable)"
    Protected Sub GuestList_SR(ByRef tblGuest As PdfPTable, ByVal guestDt As DataTable)
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        phrase = New Phrase()
        tblGuest.SetWidths(New Single() {0.64F, 0.18F, 0.18F})
        Dim guestHeader() As String = {"Name of the Guest(s)", "Child D-O-B", "Arrival"}
        For i = 0 To 2
            phrase = New Phrase()
            phrase.Add(New Chunk(guestHeader(i), NormalFontBold_SR))
            cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 4.0F
            cell.PaddingTop = 1.0F
            cell.BackgroundColor = titleColor_SR
            tblGuest.AddCell(cell)
        Next
        For Each guestDr As DataRow In guestDt.Rows
            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(guestDr(0)), NormalFont_SR))
            cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.SetLeading(12, 0)
            cell.PaddingBottom = 3.0F
            tblGuest.AddCell(cell)

            Dim age As String = IIf(guestDr(1) = 0.0, "", Convert.ToString(Math.Round(guestDr(1))))
            phrase = New Phrase()
            phrase.Add(New Chunk(age, NormalFont_SR))
            cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            tblGuest.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(guestDr(2)), NormalFont_SR))
            cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            tblGuest.AddCell(cell)
        Next
        phrase = New Phrase()
        phrase.Add(New Chunk("Total No. of Pax = " & Convert.ToString(guestDt.Rows.Count), NormalFontBold_SR))
        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
        cell.Colspan = 3
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.PaddingLeft = 5.0F
        cell.PaddingBottom = 3.0F
        tblGuest.AddCell(cell)
    End Sub
#End Region
#Region "Protected Sub SpecialEvents(ByVal splEventDt As DataTable, ByVal documentWidth As Single, ByVal CurrCode As String, ByRef tblSplEvent As PdfPTable)"
    Protected Sub SpecialEvents_SR(ByVal splEventDt As DataTable, ByVal documentWidth As Single, ByVal CurrCode As String, ByRef tblSplEvent As PdfPTable)
        Dim phrase As New Phrase
        Dim cell As New PdfPCell
        Dim splEventTitleColor As BaseColor = New BaseColor(203, 235, 249)   '-255, 219, 212
        Dim arrSplEvent() As String = {"Special Events", "Date of Event", "Units/ Pax", "Type of Units/Pax", "Rate per Units/Pax", "Charges " & CurrCode}
        tblSplEvent.TotalWidth = documentWidth
        tblSplEvent.LockedWidth = True
        tblSplEvent.SetWidths(New Single() {0.42F, 0.12F, 0.09F, 0.12F, 0.12F, 0.13F})
        tblSplEvent.Complete = False
        tblSplEvent.HeaderRows = 1
        tblSplEvent.SplitRows = False
        For i = 0 To 5
            phrase = New Phrase()
            phrase.Add(New Chunk(arrSplEvent(i), NormalFontBold_SR))
            cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 4.0F
            cell.PaddingTop = 1.0F
            cell.BackgroundColor = splEventTitleColor
            tblSplEvent.AddCell(cell)
        Next
        For Each splEventDr As DataRow In splEventDt.Rows
            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(splEventDr("splEventName")), NormalFont_SR))
            cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.SetLeading(12, 0)
            cell.PaddingLeft = 3.0F
            cell.PaddingBottom = 4.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(splEventDr("splEventDate")), NormalFont_SR))
            cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(splEventDr("noOfPax")), NormalFont_SR))
            cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(splEventDr("paxType")), NormalFont_SR))
            cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(splEventDr("paxRate")), NormalFont_SR))
            cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            cell.PaddingRight = 4.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(splEventDr("splEventValue")), NormalFont_SR))
            cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            cell.PaddingRight = 4.0F
            tblSplEvent.AddCell(cell)
        Next
        tblSplEvent.Complete = True
    End Sub
#End Region
#Region "Protected Sub AppendOtherServices(ByRef tblOthServ As PdfPTable, ByVal inputDt As DataTable)"
    Protected Sub AppendOtherServices_SR(ByRef tblOthServ As PdfPTable, ByVal inputDt As DataTable)
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        For Each inputDr As DataRow In inputDt.Rows
            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(inputDr("ServiceName")), NormalFont_SR))
            If Convert.ToString(inputDr("OthServType")).Contains("Tour~~") Then
                phrase.Add(New Chunk(vbCr + Convert.ToString(inputDr("OthServType").ToString.Substring(inputDr("OthServType").ToString.IndexOf("~~") + 2)), SmallFont))
            End If
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True, "Yes")
            Else
                cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
            End If
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.SetLeading(12, 0)
            cell.PaddingBottom = 3.0F
            tblOthServ.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(inputDr("ServiceDate")), NormalFont_SR))
            cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell.Rowspan = 2
            End If
            tblOthServ.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(inputDr("Unit")), NormalFont_SR))
            cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell.Rowspan = 2
            End If
            tblOthServ.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(inputDr("UnitPrice")), NormalFont_SR))
            cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            cell.PaddingRight = 4.0F
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell.Rowspan = 2
            End If
            tblOthServ.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(inputDr("UnitSaleValue")), NormalFont_SR))
            cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            cell.PaddingRight = 4.0F
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell.Rowspan = 2
            End If
            tblOthServ.AddCell(cell)

            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                phrase = New Phrase()
                phrase.Add(New Chunk(Convert.ToString(inputDr("pickupdropoff")), NormalFont_SR))
                If Convert.ToInt32(inputDr("sic")) <> 1 Then
                    phrase.Add(New Chunk(" (" & inputDr("adults").ToString() & " Adults", NormalFont_SR))
                    If String.IsNullOrEmpty(Convert.ToString(inputDr("child")).Trim()) Or Convert.ToString(inputDr("child")).Trim() = "0" Then
                        phrase.Add(New Chunk(")", NormalFont_SR))
                    Else
                        phrase.Add(New Chunk(", " & inputDr("child").ToString() & " Child)", NormalFont_SR))
                    End If
                End If
                cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True, "No")
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                cell.SetLeading(12, 0)
                cell.PaddingBottom = 3.0F
                tblOthServ.AddCell(cell)
            End If
        Next
    End Sub
#End Region
#Region "Protected Sub BankDetails(ByRef tblBank As PdfPTable, ByVal BankDt As DataTable)"
    Protected Sub BankDetails_SR(ByRef tblBank As PdfPTable, ByVal BankDt As DataTable)
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        phrase = New Phrase()
        tblBank.SetWidths(New Single() {0.27F, 0.73F})
        phrase = New Phrase()
        phrase.Add(New Chunk("BENEFICIARY BANK DETAILS", NormalFontBold_SR))
        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.PaddingTop = 2.0F
        cell.PaddingBottom = 5.0F
        cell.Colspan = 2
        cell.BackgroundColor = titleColor_SR
        tblBank.AddCell(cell)

        Dim bankDr As DataRow = BankDt.Rows(0)
        Dim beneficiaryDetails() As String = {"BENEFICIARY NAME", Convert.ToString(bankDr("beneficiaryName")), "BENEFICIARY ADDRESS", Convert.ToString(bankDr("beneficiaryAddress")), "BANK NAME & ADDRESS", Convert.ToString(bankDr("bankName")) & ", " & Convert.ToString(bankDr(3)), _
         "ACCOUNT NUMBER", Convert.ToString(bankDr("accountNumber")), "IBAN NUMBER", Convert.ToString(bankDr("ibanNumber")), "SWIFT CODE", Convert.ToString(bankDr("swiftCode"))}
        For i = 0 To 11
            If i Mod 2 = 0 Then
                phrase = New Phrase()
                phrase.Add(New Chunk(beneficiaryDetails(i), NormalFontBold_SR))
                cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                cell.SetLeading(12, 0)
                cell.PaddingTop = 1
                cell.PaddingLeft = 3
                cell.PaddingRight = 3
                cell.PaddingBottom = 3
                tblBank.AddCell(cell)
            Else
                phrase = New Phrase()
                If i = 1 Then
                    phrase.Add(New Chunk(beneficiaryDetails(i), NormalFontBoldRed_SR))
                Else
                    phrase.Add(New Chunk(beneficiaryDetails(i), NormalFontBold_SR))
                End If
                cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                cell.SetLeading(12, 0)
                cell.PaddingTop = 1
                cell.PaddingLeft = 3
                cell.PaddingRight = 3
                cell.PaddingBottom = 3
                tblBank.AddCell(cell)
            End If
        Next

        phrase = New Phrase()
        phrase.Add(New Chunk("Note : It is mandatory to mention the IBAN number for Bank Payment Transfer", NormalFontBold_SR))
        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.SetLeading(12, 0)
        cell.PaddingTop = 2.0F
        cell.PaddingBottom = 5.0F
        cell.Colspan = 2
        tblBank.AddCell(cell)
    End Sub
#End Region

    Public Function ReadPdfFile(ByVal fileName As String) As String
        Try
            'Dim text As New StringBuilder

            'If File.Exists(fileName) Then
            '    Dim pdfReader As New PdfReader(fileName)
            '    For Page As Integer = 1 To pdfReader.NumberOfPages
            '        Dim strategy As ITextExtractionStrategy = New SimpleTextExtractionStrategy()
            '        Dim currentText As String = PdfTextExtractor.GetTextFromPage(pdfReader, Page, strategy)
            '        currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.[Default], Encoding.UTF8, Encoding.[Default].GetBytes(currentText)))
            '        text.Append(currentText)
            '    Next
            '    pdfReader.Close()
            'End If

            'Return text.ToString()


            Dim fileReader As String
            fileReader = My.Computer.FileSystem.ReadAllText(fileName)
            Return fileReader
        Catch ex As Exception
            Return ""
        End Try


    End Function

    Public Function GenerateServiceReport_SR(ByVal requestID As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, ByVal objResParam As ReservationParameters, Optional ByVal fileName As String = "", Optional ByVal aEmergencyNoId As String = "1", Optional ByVal sUnified As String = "0") As String
        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")

            '*** Danny Unified Booking 21/01/2019>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dim strUnifiedID As String = ""
            Dim strMainRequestId As String = ""
            Dim strInvoiceSource As String = ""
            If sUnified = "1" Then
                Dim dsUnifies As DataSet
                Dim strUnifiedRequestID As String
                Dim dtUnifiedHeader As DataTable = objclsUtilities.GetDataFromDataTable("sp_get_linked_unifiedbooking '" + requestID + "'")
                If dtUnifiedHeader.Rows.Count > 0 Then
                    strUnifiedID = dtUnifiedHeader.Rows(0)("unified_requestid").ToString
                    strMainRequestId = dtUnifiedHeader.Rows(0)("mainrequestid").ToString
                    strInvoiceSource = dtUnifiedHeader.Rows(0)("invoicesource").ToString
                    Dim dtUnifiedDeatais As DataTable = dtUnifiedHeader.DefaultView.ToTable 'objclsUtilities.GetDataFromDataTable("select * from unifiedbooking_detail where unified_requestid ='" + strUnifiedID + "' order by case when requestid='" & strMainRequestId & "' then 1 else 2 end, ulineno")

                    For Each rows In dtUnifiedDeatais.Rows
                        Dim sUnifiedDb As String = rows("dbname").ToString + ".dbo.sp_booking_confirmation_print"
                        strUnifiedRequestID = rows("requestid").ToString
                        If objResParam.LoginType = "Agent" And objResParam.WhiteLabel = "1" Then
                            mySqlCmd = New SqlCommand("sp_booking_confirmation_print_whitelabel", sqlConn)
                        Else
                            'mySqlCmd = New SqlCommand("sp_booking_confirmation_print", sqlConn)
                            mySqlCmd = New SqlCommand(sUnifiedDb, sqlConn)
                        End If
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@RequestID", SqlDbType.VarChar, 20)).Value = strUnifiedRequestID
                        myDataAdapter.SelectCommand = mySqlCmd
                        myDataAdapter.Fill(ds)
                    Next

                End If
                If ds Is Nothing Then
                    Return ""

                Else
                    ds.Tables(14).DefaultView.Sort = "orderdate,orderno"
                    ds1 = ds
                End If


            Else
                If objResParam.LoginType = "Agent" And objResParam.WhiteLabel = "1" Then
                    mySqlCmd = New SqlCommand("sp_booking_confirmation_print_whitelabel", sqlConn)
                Else
                    mySqlCmd = New SqlCommand("sp_booking_confirmation_print", sqlConn)

                End If
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@RequestID", SqlDbType.VarChar, 20)).Value = requestID
                myDataAdapter.SelectCommand = mySqlCmd
                myDataAdapter.Fill(ds)
                ds1 = ds
            End If
            '*** Danny Unified Booking 21/01/2019<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            ''If objResParam.LoginType = "Agent" And objResParam.WhiteLabel = "1" Then
            ''    mySqlCmd = New SqlCommand("sp_booking_confirmation_print_whitelabel", sqlConn)
            ''Else
            ''    mySqlCmd = New SqlCommand("sp_booking_confirmation_print", sqlConn)
            ''End If
            ''mySqlCmd.CommandType = CommandType.StoredProcedure
            ''mySqlCmd.Parameters.Add(New SqlParameter("@RequestID", SqlDbType.VarChar, 20)).Value = requestID
            ''myDataAdapter.SelectCommand = mySqlCmd
            ''myDataAdapter.Fill(ds)
            ''ds1 = ds




            clsDBConnect.dbConnectionClose(sqlConn)


            Dim parm(0) As SqlParameter
            Dim ds_SR As New DataSet
            If sUnified = "1" Then
                parm(0) = New SqlParameter("@RequestID", CType(strMainRequestId, String))
                ds_SR = objclsUtilities.GetDataSet(strInvoiceSource & ".dbo.SP_SelectServiceDetails", parm)
            Else
                parm(0) = New SqlParameter("@RequestID", CType(requestID, String))
                ds_SR = objclsUtilities.GetDataSet("SP_SelectServiceDetails", parm)
            End If




            If ds Is Nothing Or ds_SR Is Nothing _
                    Or ds.Tables(0) Is Nothing _
                    Or ds.Tables(1) Is Nothing _
                    Or ds.Tables(3) Is Nothing _
                    Or ds.Tables(4) Is Nothing _
                    Or ds.Tables(8) Is Nothing Then

                fileName = ""
                Return fileName
            End If
            If ds.Tables.Count = 0 _
                Or ds_SR.Tables.Count = 0 _
                Or ds.Tables(0).Rows.Count = 0 _
                Or ds.Tables(8).Rows.Count = 0 _
                Or ds_SR.Tables(0).Rows.Count = 0 Then

                fileName = ""
                Return fileName
            End If


            '*** Check parameter for Need to attach.  Danny
            '*** Only Confirmed Booking need to create SR. Danny
            Dim strStatus As String = "CF"
            If ds_SR.Tables(0).Rows(0)("Attach").ToString() <> "Y" Or ds_SR.Tables(0).Rows(0)("ConfirmStatus").ToString() <> "1" Then
                '*** Danny 28/11/2018 Following Removed on Client request John
                'fileName = ""
                'Return fileName
                strStatus = ""
            End If

            If ds.Tables(0).Rows.Count > 0 Then

                Dim document As New Document(PageSize.A4, 40.0F, 20.0F, 10.0F, 20.0F)
                Dim documentWidth As Single = 480.0F
                Dim H1 As Font = FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)
                Dim Caption1 As Font = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)
                Dim Caption2 As Font = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)
                Dim V1 As Font = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK)
                Dim F1 As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)

                Dim TitleFont As Font = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)
                Dim TitleFontBold As Font = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)
                Dim TitleFontBig As Font = FontFactory.GetFont("Arial", 16, Font.NORMAL, BaseColor.BLACK)
                Dim TitleFontBigBold As Font = FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)
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
                    Dim remainingPageSpace As Single
                    document.Open()
                    'Header Table
                    'Dim headerDr As DataRow = headerDt.Rows(0)

                    '*** Image
                    Dim dtblLogo As PdfPTable = New PdfPTable(4)
                    dtblLogo.SetWidths(New Single() {0.2F, 0.3F, 0.2F, 0.3F})
                    dtblLogo.TotalWidth = documentWidth
                    dtblLogo.LockedWidth = True

                    'changed by mohamed on 06/12/2018 as to show logo from table
                    Dim lsAgentCode As String = ds.Tables(0).Rows(0)("agentcode") 'changed by mohamed on 06/12/2018
                    Dim lbShowAgentLogo As Boolean = False
                    Dim dtAgentMast As DataTable = objclsUtilities.GetDataFromDataTable("select top 1 showAgentLogoInVoucher,voucherlogo from agentmast (nolock) where agentcode ='" + lsAgentCode + "'")
                    If dtAgentMast.Rows.Count > 0 Then
                        If IsDBNull(dtAgentMast.Rows(0)("showAgentLogoInVoucher")) = False Then
                            lbShowAgentLogo = IIf(dtAgentMast.Rows(0)("showAgentLogoInVoucher") = 1, True, False)
                        End If
                    End If
                    Dim lsLogoFileName As String = "", lsLogoVirtualFileName As String = ""

                    If objResParam.LoginType = "Agent" And objResParam.WhiteLabel = "1" Then
                        Dim logoName As String = objclsUtilities.ExecuteQueryReturnStringValue("select logofilename from agentmast_whitelabel where agentcode ='" + objResParam.AgentCode.Trim() + "'")
                        cell = ImageCell_SR("~/Logos/" + logoName, 60.0F, PdfPCell.ALIGN_CENTER)
                    ElseIf lbShowAgentLogo = True Then 'changed by mohamed on 06/12/2018 as to show logo from table
                        Dim lFileInfo As FileInfo
                        Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory
                        lsLogoFileName = (t & IIf(Right(t, 1) = "\", "", "\") & "SavedReports\" & lsAgentCode & "voucherlogo.bmp")
                        lsLogoVirtualFileName = "~\SavedReports\" & lsAgentCode & "voucherlogo.bmp"
                        If File.Exists(lsLogoFileName) Then 'changed by mohamed on 06/12/2018 changed the ways of deletion
                            lFileInfo = New FileInfo(lsLogoFileName)
                            If lFileInfo.IsReadOnly = True Then
                                lFileInfo.IsReadOnly = False
                            End If
                            File.Delete(lsLogoFileName)
                        End If

                        File.WriteAllBytes(lsLogoFileName, dtAgentMast.Rows(0)("voucherlogo"))
                        cell = ImageCell_SR(lsLogoVirtualFileName, 60.0F, PdfPCell.ALIGN_CENTER)
                    Else
                        If (ds.Tables(0).Rows(0)("div_code") = "01") Then
                            cell = ImageCell_SR("~/img/Logo1.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                        Else
                            cell = ImageCell_SR("~/img/Logo2.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                        End If
                    End If
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT 'ALIGN_LEFT 'changed by mohamed on 19/12/2018
                    cell.PaddingTop = 50
                    cell.Colspan = 4 'changed by mohamed on 19/12/2018
                    dtblLogo.AddCell(cell)


                    '*** Heading
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString("SERVICE VOUCHER"), H1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_BOTTOM
                    cell.Colspan = 4 '2 'changed by mohamed on 19/12/2018
                    cell.PaddingTop = 0 '50 'changed by mohamed on 19/12/2018
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Gap
                    phrase = New Phrase()
                    phrase.Add(New Chunk(" ", V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 4
                    'cell.PaddingTop = 50
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 1
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Booking Ref: " & vbLf, Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 2
                    phrase = New Phrase()
                    If sUnified = "1" Then '*** Danny Unified Booking 21/01/2019
                        phrase.Add(New Chunk(Convert.ToString(strUnifiedID) & vbLf, V1))
                    Else
                        phrase.Add(New Chunk(Convert.ToString(requestID) & vbLf, V1))
                    End If
                    'phrase.Add(New Chunk(Convert.ToString(requestID) & vbLf, V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 3
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Arrival: " + vbLf, Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 4
                    phrase = New Phrase()
                    If ds_SR.Tables(0).Rows(0)("minCheckin").ToString.Trim.Length > 0 Then
                        phrase.Add(New Chunk(Convert.ToString(Format(CType(ds_SR.Tables(0).Rows(0)("minCheckin"), Date), "dd MMM yy")) & vbLf, V1))
                    End If

                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Booking Name: " + vbLf, Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 2
                    phrase = New Phrase()
                    phrase.Add(New Chunk(ds.Tables(8).Rows(0)("guestname").ToString() + vbLf, V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 3
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Consultant: " & vbLf, Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 4
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(ds_SR.Tables(0).Rows(0)("Consultant")) & vbLf, V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 3
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Booking Old Ref: ", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 3
                    phrase = New Phrase()
                    phrase.Add(New Chunk(ds.Tables(0).Rows(0)("columbusref").ToString(), V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 3
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Issued: ", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 4
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(Format(CType(ds.Tables(0).Rows(0)("requestdate"), Date), "dd MMM yy")) & vbLf, V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 5
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Booking Status: ", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 5
                    strStatus = ds.Tables(0).Rows(0)("confirmationstatustext") 'changed by mohamed on 01/01/2019
                    phrase = New Phrase()
                    phrase.Add(New Chunk(strStatus, V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 6
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Agent Ref: ", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 6
                    phrase = New Phrase()
                    phrase.Add(New Chunk(ds.Tables(0).Rows(0)("agentref").ToString(), V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Gap
                    phrase = New Phrase()
                    phrase.Add(New Chunk(" ", V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 4
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    dtblLogo.AddCell(cell)

                    '*** Guests
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Guests: ", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)


                    phrase = New Phrase()
                    phrase.Add(New Chunk("Agent: ", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)


                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(ds.Tables(0).Rows(0)("agentName")), V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    dtblLogo.AddCell(cell)


                    phrase = New Phrase()
                    Dim liPrintGuest As Integer
                    Dim strAdults As Integer = 0
                    '*** Danny Unified Booking 21/01/2019
                    Dim view As DataView = New DataView(ds.Tables(15))
                    Dim distinctValues As DataTable = view.ToTable(True, "GuestName")

                    For SRi As Integer = 0 To distinctValues.Rows.Count - 1
                        liPrintGuest = 0
                        If Len(ds.Tables(15).Rows(SRi)("GuestName").ToString().Trim) <= 6 Then
                            liPrintGuest = 1
                        ElseIf ds.Tables(15).Rows(SRi)("GuestName").ToString().Trim.ToUpper().Contains("ADULT") Then
                        ElseIf ds.Tables(15).Rows(SRi)("GuestName").ToString().Trim.ToUpper().Substring(6).Contains("CHILD") Then '.Substring(6) added / changed by mohamed on 18/12/2018
                            'ElseIf ds.Tables(8).Rows(SRi)("guestname").ToString().Trim.ToUpper().Contains("ADULT") Then
                        Else
                            'strAdults = ds.Tables(8).Rows(SRi)("guestname").ToString() + vbLf
                            ' phrase.Add(New Chunk(ds.Tables(15).Rows(SRi)("GuestName").ToString() + vbLf, V1))
                            'Else
                            liPrintGuest = 1
                            '    strAdults = strAdults + 1
                        End If
                        If liPrintGuest = 1 Then
                            phrase.Add(New Chunk(ds.Tables(15).Rows(SRi)("GuestName").ToString() + vbLf, V1))
                        End If
                    Next
                    'If strAdults > 0 Then
                    '    phrase.Add(New Chunk(Convert.ToString(strAdults.ToString) + " More Adults, V1))
                    'End If
                    'phrase.Add(New Chunk(ds.Tables(8).Rows(SRi)("guestname").ToString() + vbLf, V1))
                    'phrase.Add(New Chunk(strAdults, V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 4
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    dtblLogo.AddCell(cell)

                    '*** Gap
                    phrase = New Phrase()
                    phrase.Add(New Chunk(" ", V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 4
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    dtblLogo.AddCell(cell)

                    document.Add(dtblLogo)



                    '*** Table Head
                    Dim dtblTable As PdfPTable = New PdfPTable(4)
                    dtblTable.TotalWidth = documentWidth
                    dtblTable.LockedWidth = True
                    dtblTable.SetWidths(New Single() {0.2F, 0.2F, 0.3F, 0.3F})

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Supplier", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY
                    cell.PaddingBottom = 4
                    dtblTable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Date", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY
                    cell.PaddingBottom = 4
                    dtblTable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Service", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 4
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY
                    dtblTable.AddCell(cell)


                    phrase = New Phrase()
                    phrase.Add(New Chunk("Details", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 4
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY
                    dtblTable.AddCell(cell)

                    Dim lsSupplierName, lssuppliertel, lsserviceFromDate, lsServiceToDate, lsservicename, lsservicedetail, lsservicetype As String
                    Dim dtRow As DataRowView, lbIsItHotel As Boolean
                    '*** Hotel Table Details=============================================
                    'For SR_Trani As Integer = 0 To ds.Tables(14).Rows.Count - 1
                    For Each DataRowView In ds.Tables(14).DefaultView

                        'dtRow = ds.Tables(14).Rows(SR_Trani)
                        dtRow = DataRowView
                        lsSupplierName = dtRow("suppliername").ToString '.Replace(Asc(10), vbLf)'changed by mohamed on 30/12/2018 as it is working fine without replacing
                        lssuppliertel = dtRow("suppliertel").ToString
                        lsserviceFromDate = dtRow("checkin")
                        lsServiceToDate = dtRow("checkout")
                        lsservicename = dtRow("servicename").ToString '.Replace(Asc(10), vbLf)'changed by mohamed on 30/12/2018 as it is working fine without replacing
                        lsservicedetail = dtRow("servicedetail").ToString '.Replace(Asc(10), vbLf)'changed by mohamed on 30/12/2018 as it is working fine without replacing
                        lsservicetype = dtRow("servicetype").ToString '.Replace(Asc(10), vbLf)'changed by mohamed on 30/12/2018 as it is working fine without replacing

                        If lsservicetype.Trim.ToUpper = "HOTEL" Then
                            lbIsItHotel = True
                        Else
                            lbIsItHotel = False
                        End If

                        phrase = New Phrase()
                        phrase.Add(New Chunk(lsSupplierName + vbLf, Caption2))
                        phrase.Add(New Chunk(lssuppliertel + vbLf, F1))
                        If dtRow("servicetype").ToString.Trim = "Tour" Then
                            If dtRow("suppliertel").ToString.Trim.Length > 0 Then
                                phrase.Add(New Chunk("Pick-Up:" + dtRow("suppliertel").ToString + vbLf, F1))

                            End If
                        End If
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)

                        phrase = New Phrase()
                        Dim TRDate As DateTime, lsDateAddStr As String
                        TRDate = CType(lsserviceFromDate, Date)
                        lsDateAddStr = IIf(lbIsItHotel, "Check-in: ", IIf(lsServiceToDate.Trim <> "", "Start: ", "")) + Format(TRDate, "dd MMM yy")
                        phrase.Add(New Chunk(lsDateAddStr & vbLf, F1))

                        Dim myCulture As System.Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture
                        Dim dayOfWeek As DayOfWeek = myCulture.Calendar.GetDayOfWeek(CType(lsserviceFromDate, Date))
                        Dim dayName As String = myCulture.DateTimeFormat.GetDayName(dayOfWeek)
                        If lsServiceToDate = "" Then
                            phrase.Add(New Chunk(Convert.ToString(dayName) & vbLf, F1))
                        End If

                        If lsServiceToDate.Trim <> "" Then
                            TRDate = CType(lsServiceToDate, Date)
                            lsDateAddStr = IIf(lbIsItHotel, "Check-out: ", "End: ") + Format(TRDate, "dd MMM yy")
                            phrase.Add(New Chunk(lsDateAddStr & vbLf, F1))
                            dayOfWeek = myCulture.Calendar.GetDayOfWeek(CType(lsServiceToDate, Date))
                            dayName = myCulture.DateTimeFormat.GetDayName(dayOfWeek)
                            If lsServiceToDate = "" Then
                                phrase.Add(New Chunk(Convert.ToString(dayName) & vbLf, F1))
                            End If
                        End If

                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk(lsservicename + vbLf, F1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)


                        phrase = New Phrase()
                        phrase.Add(New Chunk(lsservicedetail + vbLf, F1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)
                    Next

                    'changed by mohamed on 18/12/2018
                    'If ds.Tables(14).Rows.Count = 0 Then
                    '    phrase = New Phrase()
                    '    phrase.Add(New Chunk("NO SERVICE TO SHOW" + vbLf, F1))
                    '    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    '    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    '    cell.Colspan = 4
                    '    'cell.PaddingTop = 50
                    '    'cell.SetLeading(12, 0)
                    '    cell.PaddingBottom = 4
                    '    dtblTable.AddCell(cell)
                    'End If
                    '*** Gap
                    Dim strEmergencyNo As String()
                    If aEmergencyNoId = "2" Then 'changed by mohamed on 30/12/2018
                        strEmergencyNo = ds_SR.Tables(0).Rows(0)("24hoursEmergencyMobileNo2").ToString().Split("|")
                    ElseIf aEmergencyNoId = "3" Then
                        strEmergencyNo = ds_SR.Tables(0).Rows(0)("24hoursEmergencyMobileNo3").ToString().Split("|")
                    ElseIf aEmergencyNoId = "4" Then
                        strEmergencyNo = ds_SR.Tables(0).Rows(0)("24hoursEmergencyMobileNo4").ToString().Split("|")
                    ElseIf aEmergencyNoId = "5" Then
                        strEmergencyNo = ds_SR.Tables(0).Rows(0)("24hoursEmergencyMobileNo5").ToString().Split("|")
                    Else
                        strEmergencyNo = ds_SR.Tables(0).Rows(0)("24hoursEmergencyMobileNo").ToString().Split("|")
                    End If
                    'strEmergencyNo = ds_SR.Tables(0).Rows(0)("24hoursEmergencyMobileNo").ToString().Split("|")
                    phrase = New Phrase()
                    For Each lin In strEmergencyNo
                        phrase.Add(New Chunk(lin.ToString() + vbLf, Caption1))
                    Next

                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Colspan = 4
                    'cell.PaddingTop = 50
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    dtblTable.AddCell(cell)

                    document.Add(dtblTable)

                    Dim dtblFooter As PdfPTable = New PdfPTable(1)
                    dtblFooter.TotalWidth = documentWidth
                    dtblFooter.LockedWidth = True
                    dtblFooter.SetWidths(New Single() {1.0F})

                    '*** Footer File name and content Reading
                    '*** Reading Columbus Folder path
                    Dim strColumbusPath As String = System.Web.HttpContext.Current.Server.MapPath("")
                    strColumbusPath = System.IO.Path.GetDirectoryName(strColumbusPath)
                    'strColumbusPath = strColumbusPath + "\" + objclsUtilities.ExecuteQueryReturnStringValue("select option_selected from reservation_parameters where param_id=8")

                    Dim s As String = strColumbusPath + "\ExcelTemplates\" + ds_SR.Tables(0).Rows(0)("FooterFilename").ToString()
                    s = ReadPdfFile(s)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(s, F1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_JUSTIFIED, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    'cell.Colspan = 4
                    'cell.PaddingTop = 50
                    'cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    dtblFooter.AddCell(cell)


                    document.Add(dtblFooter)


                    'writer.PageEvent = New clsBookingConfirmPageEvents(tblInv, printMode)

                    'document.AddTitle(Convert.ToString(ds.Tables(0).Rows(0)("printHeader")) & "-" & requestID)
                    document.AddTitle("SERVICE VOUCHER -" & requestID)
                    document.Close()

                    If printMode = "download" Then
                        Dim pagingFont As Font = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.GRAY)
                        Dim reader As New PdfReader(memoryStream.ToArray())
                        Using mStream As New MemoryStream()
                            Using stamper As New PdfStamper(reader, mStream)
                                Dim pages As Integer = reader.NumberOfPages
                                For i As Integer = 1 To pages
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 300.0F, 20.0F, 0)
                                Next
                            End Using
                            bytes = mStream.ToArray()
                        End Using
                    End If
                End Using
                Return fileName
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function


    Public Function GenerateServiceReport_SR_OLD(ByVal requestID As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, ByVal objResParam As ReservationParameters, Optional ByVal fileName As String = "") As String
        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            If objResParam.LoginType = "Agent" And objResParam.WhiteLabel = "1" Then
                mySqlCmd = New SqlCommand("sp_booking_confirmation_print_whitelabel", sqlConn)
            Else
                mySqlCmd = New SqlCommand("sp_booking_confirmation_print", sqlConn)
            End If
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@RequestID", SqlDbType.VarChar, 20)).Value = requestID
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            ds1 = ds
            clsDBConnect.dbConnectionClose(sqlConn)


            Dim parm(0) As SqlParameter
            parm(0) = New SqlParameter("@RequestID", CType(requestID, String))

            Dim ds_SR As New DataSet
            ds_SR = objclsUtilities.GetDataSet("SP_SelectServiceDetails", parm)

            'Dim myDataAdapter1 As New SqlDataAdapter

            'Dim mySqlCmd1 As New SqlCommand
            'Dim sqlConn1 As New SqlConnection
            'sqlConn1 = clsDBConnect.dbConnectionnew("strDBConnection")
            'mySqlCmd1 = New SqlCommand("SP_SelectServiceDetails", sqlConn1)

            'mySqlCmd1.CommandType = CommandType.StoredProcedure
            'mySqlCmd1.Parameters.Add(New SqlParameter("@RequestID", SqlDbType.VarChar, 20)).Value = requestID
            'myDataAdapter1.SelectCommand = mySqlCmd1
            'myDataAdapter1.Fill(ds_SR)

            'clsDBConnect.dbConnectionClose(sqlConn1)


            If ds Is Nothing Or ds_SR Is Nothing _
                    Or ds.Tables(0) Is Nothing _
                    Or ds.Tables(1) Is Nothing _
                    Or ds.Tables(3) Is Nothing _
                    Or ds.Tables(4) Is Nothing _
                    Or ds.Tables(8) Is Nothing Then

                fileName = ""
                Return fileName
            End If
            If ds.Tables.Count = 0 _
                Or ds_SR.Tables.Count = 0 _
                Or ds.Tables(0).Rows.Count = 0 _
                Or ds.Tables(8).Rows.Count = 0 _
                Or ds_SR.Tables(0).Rows.Count = 0 Then

                fileName = ""
                Return fileName
            End If


            '*** Check parameter for Need to attach.  Danny
            '*** Only Confirmed Booking need to create SR. Danny
            Dim strStatus As String = "CF"
            If ds_SR.Tables(0).Rows(0)("Attach").ToString() <> "Y" Or ds_SR.Tables(0).Rows(0)("ConfirmStatus").ToString() <> "1" Then
                '*** Danny 28/11/2018 Following Removed on Client request John
                'fileName = ""
                'Return fileName
                strStatus = ""
            End If

            If ds.Tables(0).Rows.Count > 0 Then
                Dim document As New Document(PageSize.A4, 40.0F, 20.0F, 10.0F, 10.0F)
                Dim documentWidth As Single = 480.0F
                Dim H1 As Font = FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)
                Dim Caption1 As Font = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)
                Dim Caption2 As Font = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)
                Dim V1 As Font = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK)
                Dim F1 As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)

                Dim TitleFont As Font = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK)
                Dim TitleFontBold As Font = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)
                Dim TitleFontBig As Font = FontFactory.GetFont("Arial", 16, Font.NORMAL, BaseColor.BLACK)
                Dim TitleFontBigBold As Font = FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK)
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
                    Dim remainingPageSpace As Single
                    document.Open()
                    'Header Table
                    'Dim headerDr As DataRow = headerDt.Rows(0)

                    '*** Image
                    Dim dtblLogo As PdfPTable = New PdfPTable(4)
                    dtblLogo.SetWidths(New Single() {0.2F, 0.3F, 0.2F, 0.3F})
                    dtblLogo.TotalWidth = documentWidth
                    dtblLogo.LockedWidth = True
                    If objResParam.LoginType = "Agent" And objResParam.WhiteLabel = "1" Then
                        Dim logoName As String = objclsUtilities.ExecuteQueryReturnStringValue("select logofilename from agentmast_whitelabel where agentcode ='" + objResParam.AgentCode.Trim() + "'")
                        cell = ImageCell_SR("~/Logos/" + logoName, 60.0F, PdfPCell.ALIGN_CENTER)
                    Else
                        If (ds.Tables(0).Rows(0)("div_code") = "01") Then
                            cell = ImageCell_SR("~/img/Logo1.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                        Else
                            cell = ImageCell_SR("~/img/Logo2.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                        End If
                    End If
                    cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                    cell.PaddingTop = 50
                    cell.Colspan = 2
                    dtblLogo.AddCell(cell)

                    '*** Heading
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString("SERVICE VOUCHER"), H1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_BOTTOM
                    cell.Colspan = 2
                    cell.PaddingTop = 50
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Gap
                    phrase = New Phrase()
                    phrase.Add(New Chunk(" ", V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 4
                    'cell.PaddingTop = 50
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 1
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Booking Ref: " & vbLf, Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 2
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(requestID) & vbLf, V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 3
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Arrival: " + vbLf, Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 4
                    phrase = New Phrase()
                    If ds_SR.Tables(0).Rows(0)("minCheckin").ToString.Trim.Length > 0 Then
                        phrase.Add(New Chunk(Convert.ToString(Format(CType(ds_SR.Tables(0).Rows(0)("minCheckin"), Date), "dd MMM yy")) & vbLf, V1))
                    End If

                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Booking Name: " + vbLf, Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 2
                    phrase = New Phrase()
                    phrase.Add(New Chunk(ds.Tables(8).Rows(0)("guestname").ToString() + vbLf, V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 3
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Consultant: " & vbLf, Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 4
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(ds_SR.Tables(0).Rows(0)("Consultant")) & vbLf, V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 3
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Booking Old Ref: ", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 3
                    phrase = New Phrase()
                    phrase.Add(New Chunk(ds.Tables(0).Rows(0)("columbusref").ToString(), V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 3
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Issued: ", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 4
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(Format(CType(ds.Tables(0).Rows(0)("requestdate"), Date), "dd MMM yy")) & vbLf, V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 5
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Booking Status: ", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 5
                    phrase = New Phrase()
                    phrase.Add(New Chunk(strStatus, V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 6
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Agent Ref: ", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Booking Details 6
                    phrase = New Phrase()
                    phrase.Add(New Chunk(ds.Tables(0).Rows(0)("agentref").ToString(), V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)

                    '*** Gap
                    phrase = New Phrase()
                    phrase.Add(New Chunk(" ", V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 4
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    dtblLogo.AddCell(cell)

                    '*** Guests
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Guests: ", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)


                    phrase = New Phrase()
                    phrase.Add(New Chunk("Agent: ", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    dtblLogo.AddCell(cell)


                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(ds.Tables(0).Rows(0)("agentName")), V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    dtblLogo.AddCell(cell)


                    phrase = New Phrase()

                    Dim strAdults As Integer = 0
                    For SRi As Integer = 0 To ds.Tables(8).Rows.Count - 1
                        If ds.Tables(8).Rows(SRi)("guestname").ToString().Trim.ToUpper().Contains("ADULT") Then
                        ElseIf ds.Tables(8).Rows(SRi)("guestname").ToString().Trim.ToUpper().Substring(6).Contains("CHILD") Then '.Substring(6) added / changed by mohamed on 18/12/2018
                            'ElseIf ds.Tables(8).Rows(SRi)("guestname").ToString().Trim.ToUpper().Contains("ADULT") Then
                        Else
                            'strAdults = ds.Tables(8).Rows(SRi)("guestname").ToString() + vbLf
                            phrase.Add(New Chunk(ds.Tables(8).Rows(SRi)("guestname").ToString() + vbLf, V1))
                            'Else

                            '    strAdults = strAdults + 1
                        End If

                    Next
                    'If strAdults > 0 Then
                    '    phrase.Add(New Chunk(Convert.ToString(strAdults.ToString) + " More Adults, V1))
                    'End If
                    'phrase.Add(New Chunk(ds.Tables(8).Rows(SRi)("guestname").ToString() + vbLf, V1))
                    'phrase.Add(New Chunk(strAdults, V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 4
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    dtblLogo.AddCell(cell)

                    '*** Gap
                    phrase = New Phrase()
                    phrase.Add(New Chunk(" ", V1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 4
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    dtblLogo.AddCell(cell)

                    document.Add(dtblLogo)



                    '*** Table Head
                    Dim dtblTable As PdfPTable = New PdfPTable(4)
                    dtblTable.TotalWidth = documentWidth
                    dtblTable.LockedWidth = True
                    dtblTable.SetWidths(New Single() {0.2F, 0.2F, 0.3F, 0.3F})

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Supplier", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY
                    cell.PaddingBottom = 4
                    dtblTable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Date", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY
                    cell.PaddingBottom = 4
                    dtblTable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Service", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 4
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY
                    dtblTable.AddCell(cell)


                    phrase = New Phrase()
                    phrase.Add(New Chunk("Details", Caption1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 4
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY
                    dtblTable.AddCell(cell)

                    '*** Hotel Table Details=============================================
                    For SR_Trani As Integer = 0 To ds.Tables(1).Rows.Count - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(ds.Tables(0).Rows(0)("division_master_des").ToString() + vbLf, Caption2))
                        phrase.Add(New Chunk(ds.Tables(0).Rows(0)("tel").ToString() + vbLf, F1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)

                        phrase = New Phrase()
                        Dim TRDate As DateTime
                        TRDate = CType(ds.Tables(1).Rows(SR_Trani)("checkin"), Date)

                        phrase.Add(New Chunk(Convert.ToString("CheckIn: " + Format(TRDate, "dd MMM yy")) & vbLf, F1))

                        Dim myCulture As System.Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture
                        Dim dayOfWeek As DayOfWeek = myCulture.Calendar.GetDayOfWeek(CType(ds.Tables(1).Rows(SR_Trani)("checkin"), Date))
                        Dim dayName As String = myCulture.DateTimeFormat.GetDayName(dayOfWeek)
                        phrase.Add(New Chunk(Convert.ToString(dayName) & vbLf, V1))


                        TRDate = CType(ds.Tables(1).Rows(SR_Trani)("checkout"), Date)
                        phrase.Add(New Chunk(Convert.ToString("CheckOut: " + Format(TRDate, "dd MMM yy")) & vbLf, F1))
                        dayOfWeek = myCulture.Calendar.GetDayOfWeek(CType(ds.Tables(1).Rows(SR_Trani)("checkout"), Date))
                        dayName = myCulture.DateTimeFormat.GetDayName(dayOfWeek)
                        phrase.Add(New Chunk(Convert.ToString(dayName) & vbLf, V1))

                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk("Hotel Booking" + vbLf, F1))
                        phrase.Add(New Chunk(ds.Tables(1).Rows(SR_Trani)("partyname").ToString() + vbLf, F1))
                        phrase.Add(New Chunk(ds.Tables(1).Rows(SR_Trani)("roomdetail").ToString() + vbLf, F1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)

                        phrase = New Phrase()

                        If ds.Tables(1).Rows(SR_Trani)("CustomerRemark").ToString().Trim.Length > 0 Then
                            phrase.Add(New Chunk("Customer Remark: " + ds.Tables(1).Rows(SR_Trani)("CustomerRemark").ToString() + vbLf, F1))
                        End If
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)
                    Next

                    '*** Service Table Details=============================================
                    For SR_Trani As Integer = 0 To ds.Tables(3).Rows.Count - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(ds.Tables(0).Rows(0)("division_master_des").ToString() + vbLf, Caption2))
                        phrase.Add(New Chunk(ds.Tables(0).Rows(0)("tel").ToString() + vbLf, F1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)




                        phrase = New Phrase()
                        Dim TRDate As DateTime
                        TRDate = CType(ds.Tables(3).Rows(SR_Trani)("transferdate"), Date)
                        phrase.Add(New Chunk(Convert.ToString(Format(TRDate, "dd MMM yy")) & vbLf, F1))

                        Dim myCulture As System.Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture
                        Dim dayOfWeek As DayOfWeek = myCulture.Calendar.GetDayOfWeek(CType(ds.Tables(3).Rows(SR_Trani)("transferdate"), Date))
                        ' dayOfWeek.ToString() would return "Sunday" but it's an enum value,
                        ' the correct dayname can be retrieved via DateTimeFormat.
                        ' Following returns "Sonntag" for me since i'm in germany '
                        Dim dayName As String = myCulture.DateTimeFormat.GetDayName(dayOfWeek)

                        phrase.Add(New Chunk(Convert.ToString(dayName) & vbLf, V1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk(ds.Tables(3).Rows(SR_Trani)("transfername").ToString() + vbLf, F1))
                        phrase.Add(New Chunk(ds.Tables(3).Rows(SR_Trani)("pickupdropoff").ToString() + vbLf, F1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)

                        phrase = New Phrase()

                        If ds.Tables(3).Rows(SR_Trani)("CustomerRemark").ToString().Trim.Length > 0 Then
                            phrase.Add(New Chunk("Customer Remark: " + ds.Tables(3).Rows(SR_Trani)("CustomerRemark").ToString() + vbLf, F1))
                        End If
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)
                    Next

                    For SR_Trani As Integer = 0 To ds.Tables(4).Rows.Count - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(ds.Tables(0).Rows(0)("division_master_des").ToString() + vbLf, Caption2))
                        phrase.Add(New Chunk(ds.Tables(0).Rows(0)("tel").ToString() + vbLf, F1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)




                        phrase = New Phrase()
                        Dim TRDate As DateTime
                        TRDate = CType(ds.Tables(4).Rows(SR_Trani)("airportmadate"), Date)
                        phrase.Add(New Chunk(Convert.ToString(Format(TRDate, "dd MMM yy")) & vbLf, F1))

                        Dim myCulture As System.Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture
                        Dim dayOfWeek As DayOfWeek = myCulture.Calendar.GetDayOfWeek(CType(ds.Tables(4).Rows(SR_Trani)("airportmadate"), Date))
                        ' dayOfWeek.ToString() would return "Sunday" but it's an enum value,
                        ' the correct dayname can be retrieved via DateTimeFormat.
                        ' Following returns "Sonntag" for me since i'm in germany '
                        Dim dayName As String = myCulture.DateTimeFormat.GetDayName(dayOfWeek)

                        phrase.Add(New Chunk(Convert.ToString(dayName) & vbLf, V1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk(ds.Tables(4).Rows(SR_Trani)("airportmaname").ToString() + vbLf, F1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)

                        phrase = New Phrase()

                        If ds.Tables(4).Rows(SR_Trani)("CustomerRemark").ToString().Trim.Length > 0 Then
                            phrase.Add(New Chunk("Customer Remark: " + ds.Tables(4).Rows(SR_Trani)("CustomerRemark").ToString() + vbLf, F1))
                        End If
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)
                    Next


                    For SR_Trani As Integer = 0 To ds.Tables(5).Rows.Count - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(ds.Tables(0).Rows(0)("division_master_des").ToString() + vbLf, Caption2))
                        phrase.Add(New Chunk(ds.Tables(0).Rows(0)("tel").ToString() + vbLf, F1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)




                        phrase = New Phrase()
                        Dim TRDate As DateTime
                        TRDate = CType(ds.Tables(5).Rows(SR_Trani)("visadate"), Date)
                        phrase.Add(New Chunk(Convert.ToString(Format(TRDate, "dd MMM yy")) & vbLf, F1))

                        Dim myCulture As System.Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture
                        Dim dayOfWeek As DayOfWeek = myCulture.Calendar.GetDayOfWeek(CType(ds.Tables(5).Rows(SR_Trani)("visadate"), Date))
                        ' dayOfWeek.ToString() would return "Sunday" but it's an enum value,
                        ' the correct dayname can be retrieved via DateTimeFormat.
                        ' Following returns "Sonntag" for me since i'm in germany '
                        Dim dayName As String = myCulture.DateTimeFormat.GetDayName(dayOfWeek)

                        phrase.Add(New Chunk(Convert.ToString(dayName) & vbLf, V1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk(ds.Tables(5).Rows(SR_Trani)("visaname").ToString() + vbLf, F1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)

                        phrase = New Phrase()

                        If ds.Tables(5).Rows(SR_Trani)("CustomerRemark").ToString().Trim.Length > 0 Then
                            phrase.Add(New Chunk("Customer Remark: " + ds.Tables(5).Rows(SR_Trani)("CustomerRemark").ToString() + vbLf, F1))
                        End If
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)
                    Next

                    For SR_Trani As Integer = 0 To ds.Tables(6).Rows.Count - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(ds.Tables(0).Rows(0)("division_master_des").ToString() + vbLf, Caption2))
                        phrase.Add(New Chunk(ds.Tables(0).Rows(0)("tel").ToString() + vbLf, F1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)




                        phrase = New Phrase()
                        Dim TRDate As DateTime
                        TRDate = CType(ds.Tables(6).Rows(SR_Trani)("tourdate"), Date)
                        phrase.Add(New Chunk(Convert.ToString(Format(TRDate, "dd MMM yy")) & vbLf, F1))

                        Dim myCulture As System.Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture
                        Dim dayOfWeek As DayOfWeek = myCulture.Calendar.GetDayOfWeek(CType(ds.Tables(6).Rows(SR_Trani)("tourdate"), Date))
                        ' dayOfWeek.ToString() would return "Sunday" but it's an enum value,
                        ' the correct dayname can be retrieved via DateTimeFormat.
                        ' Following returns "Sonntag" for me since i'm in germany '
                        Dim dayName As String = myCulture.DateTimeFormat.GetDayName(dayOfWeek)

                        phrase.Add(New Chunk(Convert.ToString(dayName) & vbLf, V1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk(ds.Tables(6).Rows(SR_Trani)("tourname").ToString() + vbLf, F1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)

                        phrase = New Phrase()

                        If ds.Tables(6).Rows(SR_Trani)("CustomerRemark").ToString().Trim.Length > 0 Then
                            phrase.Add(New Chunk("Customer Remark: " + ds.Tables(6).Rows(SR_Trani)("CustomerRemark").ToString() + vbLf, F1))
                        End If
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)
                    Next

                    For SR_Trani As Integer = 0 To ds.Tables(7).Rows.Count - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(ds.Tables(0).Rows(0)("division_master_des").ToString() + vbLf, Caption2))
                        phrase.Add(New Chunk(ds.Tables(0).Rows(0)("tel").ToString() + vbLf, F1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)




                        phrase = New Phrase()
                        Dim TRDate As DateTime
                        TRDate = CType(ds.Tables(7).Rows(SR_Trani)("othdate"), Date)
                        phrase.Add(New Chunk(Convert.ToString(Format(TRDate, "dd MMM yy")) & vbLf, F1))

                        Dim myCulture As System.Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture
                        Dim dayOfWeek As DayOfWeek = myCulture.Calendar.GetDayOfWeek(CType(ds.Tables(7).Rows(SR_Trani)("othdate"), Date))
                        ' dayOfWeek.ToString() would return "Sunday" but it's an enum value,
                        ' the correct dayname can be retrieved via DateTimeFormat.
                        ' Following returns "Sonntag" for me since i'm in germany '
                        Dim dayName As String = myCulture.DateTimeFormat.GetDayName(dayOfWeek)

                        phrase.Add(New Chunk(Convert.ToString(dayName) & vbLf, V1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk(ds.Tables(7).Rows(SR_Trani)("othername").ToString() + vbLf, F1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)

                        phrase = New Phrase()

                        If ds.Tables(7).Rows(SR_Trani)("CustomerRemark").ToString().Trim.Length > 0 Then
                            phrase.Add(New Chunk("Customer Remark: " + ds.Tables(7).Rows(SR_Trani)("CustomerRemark").ToString() + vbLf, F1))
                        End If
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        'cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)
                    Next
                    If ds.Tables(3).Rows.Count = 0 And _
                       ds.Tables(4).Rows.Count = 0 And _
                       ds.Tables(5).Rows.Count = 0 And _
                       ds.Tables(6).Rows.Count = 0 And _
                       ds.Tables(7).Rows.Count = 0 Then

                        phrase = New Phrase()
                        phrase.Add(New Chunk("NO SERVICE TO SHOW" + vbLf, F1))
                        cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        cell.Colspan = 4
                        'cell.PaddingTop = 50
                        'cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4
                        dtblTable.AddCell(cell)
                    End If
                    '*** Gap
                    Dim strEmergencyNo As String()

                    strEmergencyNo = ds_SR.Tables(0).Rows(0)("24hoursEmergencyMobileNo").ToString().Split("|")
                    phrase = New Phrase()
                    For Each lin In strEmergencyNo
                        phrase.Add(New Chunk(lin.ToString() + vbLf, Caption1))
                    Next

                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Colspan = 4
                    'cell.PaddingTop = 50
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    dtblTable.AddCell(cell)

                    document.Add(dtblTable)

                    Dim dtblFooter As PdfPTable = New PdfPTable(1)
                    dtblFooter.TotalWidth = documentWidth
                    dtblFooter.LockedWidth = True
                    dtblFooter.SetWidths(New Single() {1.0F})

                    '*** Footer File name and content Reading
                    '*** Reading Columbus Folder path
                    Dim strColumbusPath As String = System.Web.HttpContext.Current.Server.MapPath("")
                    strColumbusPath = System.IO.Path.GetDirectoryName(strColumbusPath)
                    strColumbusPath = strColumbusPath + "\" + objclsUtilities.ExecuteQueryReturnStringValue("select option_selected from reservation_parameters where param_id=8")

                    Dim s As String = strColumbusPath + "\ExcelTemplates\" + ds_SR.Tables(0).Rows(0)("FooterFilename").ToString()
                    s = ReadPdfFile(s)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(s, F1))
                    cell = PhraseCell_SR(phrase, PdfPCell.ALIGN_JUSTIFIED, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    'cell.Colspan = 4
                    'cell.PaddingTop = 50
                    'cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    dtblFooter.AddCell(cell)


                    document.Add(dtblFooter)


                    'writer.PageEvent = New clsBookingConfirmPageEvents(tblInv, printMode)

                    'document.AddTitle(Convert.ToString(ds.Tables(0).Rows(0)("printHeader")) & "-" & requestID)
                    document.AddTitle("SERVICE VOUCHER -" & requestID)
                    document.Close()

                    If printMode = "download" Then
                        Dim pagingFont As Font = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.GRAY)
                        Dim reader As New PdfReader(memoryStream.ToArray())
                        Using mStream As New MemoryStream()
                            Using stamper As New PdfStamper(reader, mStream)
                                Dim pages As Integer = reader.NumberOfPages
                                For i As Integer = 1 To pages
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 300.0F, 20.0F, 0)
                                Next
                            End Using
                            bytes = mStream.ToArray()
                        End Using
                    End If
                End Using
                Return fileName
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function


End Class
