Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Linq

Public Class clsBookingConfirmationPdf

#Region "Global Variable"
    Dim objclsUtilities As New clsUtils
    Dim NormalFont As Font = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK)
    Dim NormalFontBold As Font = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK)
    Dim NormalFontBoldRed As Font = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.RED)
    Dim NormalFontBoldTax As Font = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)
    Dim titleColor As BaseColor = New BaseColor(214, 214, 214)


    Dim NormalFontVAT As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)
    Dim NormalFontBoldVAT As Font = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLACK)
    Dim NormalFontBoldRedVAT As Font = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.RED)

    Dim SmallFont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)


#End Region

#Region "Public Sub GenerateReport(ByVal requestID As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "")"
    Public Sub GenerateReportOld(ByVal requestID As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "")
        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            Dim whitelabel As String = objclsUtilities.ExecuteQueryReturnStringValuenew("strDBConnection", "select isnull(w.agentcode,'') as agentcode from booking_header h left join agentmast_whitelabel w on h.agentcode=w.agentcode where h.requestid='" + requestID + "'")
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            If whitelabel <> "" Then
                mySqlCmd = New SqlCommand("sp_booking_confirmation_print_whitelabel", sqlConn)
            Else
                mySqlCmd = New SqlCommand("sp_booking_confirmation_print", sqlConn)
            End If
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@RequestID", SqlDbType.VarChar, 20)).Value = requestID
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            ds1 = ds
            Dim headerDt As DataTable = ds.Tables(0)
            Dim hotelDt As DataTable = ds.Tables(1)
            Dim tariffDt As DataTable = ds.Tables(2)
            Dim othServDt As DataTable = ds.Tables(3)
            Dim airportDt As DataTable = ds.Tables(4)
            Dim visaDt As DataTable = ds.Tables(5)
            Dim tourDt As DataTable = ds.Tables(6)
            Dim OtherDt As DataTable = ds.Tables(7)
            Dim guestDt As DataTable = ds.Tables(8)
            Dim contactDt As DataTable = ds.Tables(9)
            Dim BankDt As DataTable = ds.Tables(10)
            Dim SplEventDt As DataTable = ds.Tables(11)
            clsDBConnect.dbConnectionClose(sqlConn)
            If headerDt.Rows.Count > 0 Then
                Dim document As New Document(PageSize.A4, 0.0F, 0.0F, 20.0F, 35.0F)
                Dim documentWidth As Single = 550.0F
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
                    Dim headerDr As DataRow = headerDt.Rows(0)
                    table = New PdfPTable(2)
                    table.TotalWidth = documentWidth
                    table.LockedWidth = True
                    table.SetWidths(New Single() {0.5F, 0.5F})
                    table.Complete = False
                    table.SplitRows = False
                    Dim tblLogo As PdfPTable = New PdfPTable(2)
                    tblLogo.SetWidths(New Single() {0.27F, 0.73F})
                    'Company Name 
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("division_master_des")), TitleFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    tblLogo.AddCell(cell)
                    'Company Logo
                    'If objResParam.LoginType = "Agent" And objResParam.WhiteLabel = "1" Then
                    '    Dim logoName As String = objclsUtilities.ExecuteQueryReturnStringValuenew("select logofilename from agentmast_whitelabel where agentcode ='" + objResParam.AgentCode.Trim() + "'")
                    '    cell = ImageCell("~/Logos/" + logoName, 60.0F, PdfPCell.ALIGN_CENTER)
                    'Else
                    If (headerDr("div_code") = "01") Then
                        cell = ImageCell("~/Images/Logo.png", 60.0F, PdfPCell.ALIGN_CENTER)
                    ElseIf (headerDr("div_code") = "02") Then
                        cell = ImageCell("~/Images/logo.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                    Else
                        cell = ImageCell("~/Images/Logo.png", 60.0F, PdfPCell.ALIGN_CENTER)
                    End If
                    'End If
                    tblLogo.AddCell(cell)
                    'Company Address
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("address1")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Fax : " & Convert.ToString(headerDr("fax")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Tel : " & Convert.ToString(headerDr("tel")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("E-mail : " & Convert.ToString(headerDr("email")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Website : " & Convert.ToString(headerDr("website")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    tblLogo.AddCell(cell)
                    table.AddCell(tblLogo)

                    Dim tblClient As PdfPTable = New PdfPTable(2)
                    tblClient.SetWidths(New Single() {0.5F, 0.5F})
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentName")), TitleFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                    cell.SetLeading(11, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentAddress")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Tel : " & Convert.ToString(headerDr("agentTel")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Fax : " & Convert.ToString(headerDr("agentfax")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Email : " & Convert.ToString(headerDr("agentEmail")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Attn. : " & Convert.ToString(headerDr("agentContact")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    table.AddCell(tblClient)
                    table.Complete = True
                    document.Add(table)

                    Dim tblTitle As PdfPTable = New PdfPTable(1)
                    tblTitle.SetWidths(New Single() {1.0F})
                    tblTitle.TotalWidth = documentWidth
                    tblTitle.LockedWidth = True
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("printHeader")), TitleFontBigBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3.0F
                    cell.BackgroundColor = titleColor
                    tblTitle.AddCell(cell)
                    tblTitle.SpacingBefore = 7
                    document.Add(tblTitle)

                    Dim tblInv As PdfPTable = New PdfPTable(6)
                    tblInv.SetWidths(New Single() {0.12F, 0.14F, 0.12F, 0.14F, 0.12F, 0.25F})
                    tblInv.TotalWidth = documentWidth
                    tblInv.LockedWidth = True
                    tblInv.SplitRows = False
                    Dim arrTitle() As String = {"Invoice No : ", headerDr("requestID").ToString(), "Dated : ", headerDr("requestDate"), "Your Ref : ", headerDr("agentRef")}
                    For i = 0 To 5
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrTitle(i), NormalFontBold))
                        cell = New PdfPCell(phrase)
                        If i = 0 Then
                            cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER Or Rectangle.TOP_BORDER
                        ElseIf i = 5 Then
                            cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER Or Rectangle.TOP_BORDER
                        Else
                            cell.Border = Rectangle.BOTTOM_BORDER Or Rectangle.TOP_BORDER
                        End If
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        If i Mod 2 = 0 Then
                            cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                        Else
                            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                        End If
                        cell.Padding = 3
                        tblInv.AddCell(cell)
                    Next
                    document.Add(tblInv)
                    writer.PageEvent = New clsBookingConfirmPageEvents(tblInv, printMode)

                    If hotelDt.Rows.Count > 0 Then
                        Dim arrServ() As String = {"Hotel Services", "Chk. in", "Chk. Out", "Charges " & Convert.ToString(headerDr("currCode"))}
                        Dim tblServ As PdfPTable = New PdfPTable(4)
                        tblServ.TotalWidth = documentWidth
                        tblServ.LockedWidth = True
                        tblServ.SetWidths(New Single() {0.63F, 0.12F, 0.12F, 0.13F})
                        tblServ.Complete = False
                        tblServ.HeaderRows = 1
                        tblServ.SplitRows = False
                        For i = 0 To 3
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrServ(i), NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = titleColor
                            tblServ.AddCell(cell)
                        Next
                        For Each hotelDr As DataRow In hotelDt.Rows
                            Dim tblTariff As PdfPTable = New PdfPTable(2)
                            tblTariff.SetWidths(New Single() {0.05F, 0.95F})
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("partyName")) & vbLf, NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            cell.Colspan = 2
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            phrase.Add(New Chunk("", NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                            cell.PaddingBottom = 3.0F
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("noofrooms")) & "  " & Convert.ToString(hotelDr("RoomDetail")) & vbLf, NormalFont))
                            If Convert.ToString(hotelDr("occupancy")) <> "" Then
                                phrase.Add(New Chunk("[ " & Convert.ToString(hotelDr("occupancy")) & " ]", NormalFont))
                            End If

                            Dim rLineNo As Integer = Convert.ToInt32(hotelDr("rLineNo"))
                            Dim roomNo As Integer = Convert.ToInt32(hotelDr("roomNo"))
                            Dim tariffFilter = (From n In tariffDt.AsEnumerable() Where n.Field(Of Int32)("rLineNo") = rLineNo And n.Field(Of Int32)("roomNo") = roomNo Select n Order By Convert.ToDateTime(n.Field(Of String)("fromDate")) Ascending).ToList()
                            Dim filterTariffDt As New DataTable
                            If (tariffFilter.Count > 0) Then filterTariffDt = tariffFilter.CopyToDataTable()
                            If filterTariffDt.Rows.Count > 0 Then
                                For Each ratesDr As DataRow In filterTariffDt.Rows
                                    phrase.Add(New Chunk(vbLf + "From " + Convert.ToString(ratesDr("fromDate")) & " " & Convert.ToString(ratesDr("nights")) & " Nts * " & Convert.ToString(ratesDr("salePrice")) & " * " & Convert.ToString(hotelDr("noofrooms")) & " Units = ", NormalFont))
                                    phrase.Add(New Chunk(Convert.ToString(ratesDr("saleValue")) & " " & Convert.ToString(headerDr("currCode")), NormalFontBold))
                                    If ratesDr("bookingCode") <> "" Then
                                        phrase.Add(New Chunk(vbCrLf & "( " + Convert.ToString(ratesDr("bookingCode")) + " )", NormalFontBold))
                                    End If
                                Next
                            End If
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            If Convert.ToString(hotelDr("hotelConfNo")) <> "" Then
                                phrase.Add(New Chunk("Hotel Conf No : " & Convert.ToString(hotelDr("hotelConfNo")), NormalFontBold))
                            End If

                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            cell.Colspan = 2
                            tblTariff.AddCell(cell)
                            tblServ.AddCell(tblTariff)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("checkIn")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            tblServ.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("checkOut")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 2.0F
                            tblServ.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("salevalue")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 2.0F
                            cell.PaddingRight = 4.0F
                            tblServ.AddCell(cell)

                            If SplEventDt.Rows.Count > 0 Then
                                Dim partyCode As String = hotelDr("partyCode").ToString()
                                Dim index As Integer = hotelDt.Rows.IndexOf(hotelDr)
                                Dim i As Integer = 0
                                Dim lastIndex As Integer = index
                                Dim filterRows = (From n In SplEventDt.AsEnumerable() Where n.Field(Of String)("PartyCode") = partyCode)
                                If filterRows.Count > 0 Then
                                    Dim filterHotelRows = (From n In hotelDt.AsEnumerable() Where n.Field(Of String)("PartyCode") = partyCode)
                                    While i < filterHotelRows.Count
                                        If hotelDt.Rows.IndexOf(filterHotelRows(i)) > index Then
                                            lastIndex = hotelDt.Rows.IndexOf(filterHotelRows(i))
                                            Exit While
                                        End If
                                        i = i + 1
                                    End While
                                End If
                                If index = lastIndex Then
                                    Dim filterSplEvt As New DataTable
                                    If (filterRows.Count > 0) Then filterSplEvt = filterRows.CopyToDataTable()
                                    If filterSplEvt.Rows.Count > 0 Then
                                        Dim tblSplEvent As New PdfPTable(6)
                                        Dim currCode As String = Convert.ToString(headerDr("currCode"))
                                        SpecialEvents(filterSplEvt, documentWidth, currCode, tblSplEvent)
                                        cell = New PdfPCell(tblSplEvent)
                                        cell.Colspan = 4
                                        cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                                        tblServ.AddCell(cell)
                                    End If
                                End If
                            End If
                        Next
                        tblServ.Complete = True
                        tblServ.SpacingBefore = 7
                        document.Add(tblServ)
                    End If

                    If othServDt.Rows.Count > 0 Or airportDt.Rows.Count > 0 Or visaDt.Rows.Count > 0 Or tourDt.Rows.Count > 0 Or OtherDt.Rows.Count > 0 Then
                        Dim OthServ() As String = {"Other Services", "Date of Service", "Units/ Pax", "Rate per Units/Pax", "Charges " & Convert.ToString(headerDr("currCode"))}
                        Dim tblOthServ As PdfPTable = New PdfPTable(5)
                        tblOthServ.TotalWidth = documentWidth
                        tblOthServ.LockedWidth = True
                        tblOthServ.SetWidths(New Single() {0.57F, 0.12F, 0.09F, 0.11F, 0.13F})
                        tblOthServ.SplitRows = False
                        tblOthServ.Complete = False
                        tblOthServ.HeaderRows = 1
                        For i = 0 To 4
                            phrase = New Phrase()
                            phrase.Add(New Chunk(OthServ(i), NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = titleColor
                            tblOthServ.AddCell(cell)
                        Next

                        Dim MergeDt As DataTable = New DataTable()
                        Dim OthServType As DataColumn = New DataColumn("OthServType", GetType(String))
                        Dim ServiceName As DataColumn = New DataColumn("ServiceName", GetType(String))
                        Dim ServiceDate As DataColumn = New DataColumn("ServiceDate", GetType(String))
                        Dim Unit As DataColumn = New DataColumn("Unit", GetType(String))
                        Dim UnitPrice As DataColumn = New DataColumn("UnitPrice", GetType(Decimal))
                        Dim UnitSaleValue As DataColumn = New DataColumn("UnitSaleValue", GetType(Decimal))
                        Dim Adults As DataColumn = New DataColumn("Adults", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim Child As DataColumn = New DataColumn("Child", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim Senior As DataColumn = New DataColumn("Senior", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim PickUpDropOff As DataColumn = New DataColumn("PickUpDropOff", GetType(String)) With {.DefaultValue = DBNull.Value}
                        Dim Sic As DataColumn = New DataColumn("Sic", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        MergeDt.Columns.Add(OthServType)
                        MergeDt.Columns.Add(ServiceName)
                        MergeDt.Columns.Add(ServiceDate)
                        MergeDt.Columns.Add(Unit)
                        MergeDt.Columns.Add(UnitPrice)
                        MergeDt.Columns.Add(UnitSaleValue)
                        MergeDt.Columns.Add(Adults)
                        MergeDt.Columns.Add(Child)
                        MergeDt.Columns.Add(Senior)
                        MergeDt.Columns.Add(PickUpDropOff)
                        MergeDt.Columns.Add(Sic)
                        For Each othServDr As DataRow In othServDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Transfer"
                            MergeDr("ServiceName") = othServDr("transferName")
                            MergeDr("ServiceDate") = othServDr("transferDate")
                            MergeDr("Unit") = othServDr("units")
                            MergeDr("UnitPrice") = othServDr("unitPrice")
                            MergeDr("UnitSaleValue") = othServDr("unitSaleValue")
                            MergeDr("Adults") = othServDr("Adults")
                            MergeDr("Child") = othServDr("Child")
                            MergeDr("PickUpDropOff") = othServDr("PickUpDropOff")
                            MergeDr("Sic") = othServDr("Sic")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each VisaDr As DataRow In visaDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Visa"
                            MergeDr("ServiceName") = VisaDr("visaName")
                            MergeDr("ServiceDate") = VisaDr("VisaDate")
                            MergeDr("Unit") = VisaDr("noOfvisas")
                            MergeDr("UnitPrice") = VisaDr("visaPrice")
                            MergeDr("UnitSaleValue") = VisaDr("visaValue")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each airportDr As DataRow In airportDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Airport"
                            MergeDr("ServiceName") = airportDr("airportmaname")
                            MergeDr("ServiceDate") = airportDr("airportmadate")
                            MergeDr("Unit") = airportDr("units")
                            MergeDr("UnitPrice") = airportDr("unitPrice")
                            MergeDr("UnitSaleValue") = airportDr("unitSaleValue")
                            MergeDr("Adults") = airportDr("Adults")
                            MergeDr("Child") = airportDr("Child")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each tourDr As DataRow In tourDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Tour"
                            MergeDr("ServiceName") = tourDr("tourname")
                            MergeDr("ServiceDate") = tourDr("tourdate")
                            MergeDr("Unit") = tourDr("units")
                            MergeDr("UnitPrice") = tourDr("unitPrice")
                            MergeDr("UnitSaleValue") = tourDr("unitSaleValue")
                            MergeDr("Adults") = tourDr("Adults")
                            MergeDr("Child") = tourDr("Child")
                            MergeDr("Senior") = tourDr("Senior")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each otherDr As DataRow In OtherDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Other"
                            MergeDr("ServiceName") = otherDr("othername")
                            MergeDr("ServiceDate") = otherDr("othdate")
                            MergeDr("Unit") = otherDr("units")
                            MergeDr("UnitPrice") = otherDr("unitPrice")
                            MergeDr("UnitSaleValue") = otherDr("unitSaleValue")
                            MergeDr("Adults") = otherDr("Adults")
                            MergeDr("Child") = otherDr("Child")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        If MergeDt.Rows.Count > 0 Then
                            Dim MergeOrderDt As DataTable = (From n In MergeDt.AsEnumerable() Select n Order By Convert.ToDateTime(n.Field(Of String)("ServiceDate")) Ascending).CopyToDataTable()
                            AppendOtherServices(tblOthServ, MergeOrderDt)
                        End If
                        tblOthServ.Complete = True
                        tblOthServ.SpacingBefore = 7
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 72 Then document.NewPage()
                        document.Add(tblOthServ)
                    End If

                    Dim tblTotal As PdfPTable = New PdfPTable(2)
                    tblTotal.TotalWidth = documentWidth
                    tblTotal.LockedWidth = True
                    tblTotal.SetWidths(New Single() {0.8F, 0.2F})
                    tblTotal.KeepTogether = True

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Total Amount (" + Convert.ToString(headerDr("currcode")) + ")", NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 10.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("saleCurrency")), NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 4.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Total Amount (" + Convert.ToString(headerDr("baseCurrcode")) + ")", NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 10.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("saleValue")), NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 4.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    tblTotal.SpacingBefore = 7
                    document.Add(tblTotal)

                    '------- Tax Note ----------------
                    Dim tblTax As PdfPTable = New PdfPTable(2)
                    tblTax.TotalWidth = documentWidth
                    tblTax.LockedWidth = True
                    tblTax.SetWidths(New Single() {0.03, 0.97F})
                    tblTax.KeepTogether = True
                    tblTax.Complete = False
                    tblTax.SplitRows = False

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Chr(149), NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER
                    cell.PaddingLeft = 7.0F
                    cell.PaddingBottom = 3.0F
                    cell.PaddingTop = 3.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("ABOVE RATES ARE INCLUSIVE OF ALL TAXES INCLUDING VAT", NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER
                    cell.PaddingLeft = 2.0F
                    cell.PaddingBottom = 3.0F
                    cell.PaddingTop = 3.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Chr(149), NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingLeft = 7.0F
                    cell.PaddingBottom = 5.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("ABOVE RATES DOES NOT INCLUDE TOURISM DIRHAM FEE WHICH IS TO BE PAID BY THE GUEST DIRECTLY AT THE HOTEL", NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingLeft = 2.0F
                    cell.PaddingBottom = 5.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    tblTax.Complete = True
                    tblTax.SpacingBefore = 7
                    document.Add(tblTax)

                    If guestDt.Rows.Count > 0 Then
                        Dim tblGuest As PdfPTable = New PdfPTable(3)
                        tblGuest.TotalWidth = documentWidth
                        tblGuest.LockedWidth = True
                        tblGuest.Complete = False
                        tblGuest.SplitRows = False
                        tblGuest.HeaderRows = 1
                        GuestList(tblGuest, guestDt)
                        tblGuest.Complete = True
                        tblGuest.SpacingBefore = 7
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 48 Then document.NewPage()
                        document.Add(tblGuest)
                    End If

                    Dim tblFooter As New PdfPTable(1)
                    tblFooter.TotalWidth = documentWidth
                    tblFooter.LockedWidth = True
                    tblFooter.Complete = False
                    tblFooter.SetWidths({1.0F})
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Thanks and Best Regards", NormalFont))
                    cell = New PdfPCell(phrase)
                    cell.BorderWidth = 0.7F
                    cell.Border = Rectangle.NO_BORDER
                    cell.PaddingTop = 6.0F
                    cell.PaddingLeft = 15.0F
                    tblFooter.AddCell(cell)

                    If contactDt.Rows.Count > 0 Then
                        Dim contractDr As DataRow = contactDt.Rows(0)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(contractDr("salesPerson")) + "<" + Convert.ToString(contractDr("salesemail")) + ">" + vbCrLf + "DESTINATION SPECIALIST", NormalFont))
                        cell = New PdfPCell(phrase)
                        cell.BorderWidth = 0.7F
                        cell.BorderColor = BaseColor.WHITE
                        cell.SetLeading(15, 0)
                        cell.PaddingTop = 35.0F
                        tblFooter.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk("Mobile No - " + Convert.ToString(contractDr("salesmobile")), NormalFont))
                        cell = New PdfPCell(phrase)
                        cell.BorderWidth = 0.7F
                        cell.BorderColor = BaseColor.WHITE
                        cell.SetLeading(15, 0)
                        cell.PaddingTop = 14.0F
                        tblFooter.AddCell(cell)
                    End If
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Any Discrepancy on the above Invoice to be revert back within 72 hours from the date of Confirmation or else treated as final", NormalFont))
                    cell = New PdfPCell(phrase)
                    cell.BorderWidth = 0.7F
                    cell.BorderColor = BaseColor.WHITE
                    cell.SetLeading(15, 0)
                    cell.PaddingTop = 3.0F
                    tblFooter.AddCell(cell)
                    tblFooter.Complete = True
                    document.Add(tblFooter)

                    If BankDt.Rows.Count > 0 Then
                        Dim tblBank As PdfPTable = New PdfPTable(2)
                        tblBank.TotalWidth = documentWidth
                        tblBank.LockedWidth = True
                        tblBank.Complete = False
                        tblBank.SplitRows = False
                        BankDetails(tblBank, BankDt)
                        tblBank.Complete = True
                        tblBank.SpacingBefore = 7
                        tblBank.KeepTogether = True
                        document.Add(tblBank)
                    End If
                    document.AddTitle(Convert.ToString(headerDr("printHeader")) & "-" & requestID)
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
            Else
                Throw New Exception("There is no rows in header table")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub GenerateReport(ByVal requestID As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "")
        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            Dim whitelabel As String = objclsUtilities.ExecuteQueryReturnStringValuenew("strDBConnection", "select isnull(w.agentcode,'') as agentcode from booking_header h left join agentmast_whitelabel w on h.agentcode=w.agentcode where h.requestid='" + requestID + "'")

            If whitelabel <> "" Then
                mySqlCmd = New SqlCommand("sp_booking_confirmation_print_whitelabel", sqlConn)
            Else
                mySqlCmd = New SqlCommand("sp_booking_confirmation_print", sqlConn)
            End If
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@RequestID", SqlDbType.VarChar, 20)).Value = requestID
            mySqlCmd.CommandTimeout = 0
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            ds1 = ds
            Dim headerDt As DataTable = ds.Tables(0)
            Dim hotelDt As DataTable = ds.Tables(1)
            Dim tariffDt As DataTable = ds.Tables(2)
            Dim othServDt As DataTable = ds.Tables(3)
            Dim airportDt As DataTable = ds.Tables(4)
            Dim visaDt As DataTable = ds.Tables(5)
            Dim tourDt As DataTable = ds.Tables(6)
            Dim OtherDt As DataTable = ds.Tables(7)
            Dim guestDt As DataTable = ds.Tables(8)
            Dim contactDt As DataTable = ds.Tables(9)
            Dim objUtils As New clsUtils 'changed by nv on 27/02/2021
            'Dim BankDt As DataTable = objUtils.GetDataFromDataTable("execute sp_getbank_detail_foragent '" & requestID & "'") 'ds.Tables(10) 'ds.Tables(10) 'changed by mohamed on 26/11/2020
            Dim BankDt As DataTable = ds.Tables(10)
            Dim SplEventDt As DataTable = ds.Tables(11)
            clsDBConnect.dbConnectionClose(sqlConn)



            Dim Showguestroomwise As String = objclsUtilities.ExecuteQueryReturnStringValuenew("strDBConnection", "select option_selected from reservation_parameters where param_id=5304") '' Added shahul 25/07/18

            If headerDt.Rows.Count > 0 Then
                Dim document As New Document(PageSize.A4, 0.0F, 0.0F, 20.0F, 35.0F)
                Dim documentWidth As Single = 550.0F
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
                    Dim headerDr As DataRow = headerDt.Rows(0)
                    table = New PdfPTable(2)
                    table.TotalWidth = documentWidth
                    table.LockedWidth = True
                    table.SetWidths(New Single() {0.5F, 0.5F})
                    table.Complete = False
                    table.SplitRows = False
                    Dim tblLogo As PdfPTable = New PdfPTable(2)
                    tblLogo.SetWidths(New Single() {0.27F, 0.73F})
                    'Company Name 
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("division_master_des")), TitleFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    tblLogo.AddCell(cell)
                    'Company Logo
                    If whitelabel <> "" Then
                        Dim logoName As String = objclsUtilities.ExecuteQueryReturnStringValuenew("strDBConnection", "select logofilename from agentmast_whitelabel where agentcode in (select top 1 agentcode from booking_header(nolock) where requestid='" + requestID.Trim() + "') ")
                        cell = ImageCell("~/Logos/" + logoName, 60.0F, PdfPCell.ALIGN_CENTER)
                    Else
                        'If (headerDr("div_code") = "01") Then
                        '    cell = ImageCell("~/img/RoyalPark.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                        'Else
                        '    cell = ImageCell("~/img/Royalgulf.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                        'End If

                        If (headerDr("div_code") = "01") Then
                            cell = ImageCell("~/Images/Logo.png", 60.0F, PdfPCell.ALIGN_CENTER)
                        ElseIf (headerDr("div_code") = "02") Then
                            cell = ImageCell("~/Images/logo.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                        Else
                            cell = ImageCell("~/Images/Logo.png", 60.0F, PdfPCell.ALIGN_CENTER)
                        End If

                    End If
                    tblLogo.AddCell(cell)
                    'Company Address
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("address1")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Fax : " & Convert.ToString(headerDr("fax")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Tel : " & Convert.ToString(headerDr("tel")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("E-mail : " & Convert.ToString(headerDr("email")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Website : " & Convert.ToString(headerDr("website")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    tblLogo.AddCell(cell)
                    table.AddCell(tblLogo)

                    Dim tblClient As PdfPTable = New PdfPTable(2)
                    tblClient.SetWidths(New Single() {0.5F, 0.5F})
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentName")), TitleFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                    cell.SetLeading(11, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentAddress")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Tel : " & Convert.ToString(headerDr("agentTel")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Fax : " & Convert.ToString(headerDr("agentfax")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Email : " & Convert.ToString(headerDr("agentEmail")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    'Added by abin on 20181021 -- start
                    phrase = New Phrase()
                    phrase.Add(New Chunk("TRN NO : " & Convert.ToString(headerDr("TrnNo")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    'Added by abin on 20181021 -- End
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Attn. : " & Convert.ToString(headerDr("agentContact")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    table.AddCell(tblClient)
                    table.Complete = True
                    document.Add(table)

                    Dim tblTitle As PdfPTable = New PdfPTable(1)
                    tblTitle.SetWidths(New Single() {1.0F})
                    tblTitle.TotalWidth = documentWidth
                    tblTitle.LockedWidth = True
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("printHeader")), TitleFontBigBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3.0F
                    cell.BackgroundColor = titleColor
                    tblTitle.AddCell(cell)
                    tblTitle.SpacingBefore = 7
                    document.Add(tblTitle)

                    Dim tblInv As PdfPTable = New PdfPTable(6)
                    tblInv.SetWidths(New Single() {0.12F, 0.14F, 0.12F, 0.14F, 0.12F, 0.25F})
                    tblInv.TotalWidth = documentWidth
                    tblInv.LockedWidth = True
                    tblInv.SplitRows = False
                    Dim arrTitle() As String = {"Invoice No : ", headerDr("requestID").ToString(), "Dated : ", headerDr("requestDate"), "Your Ref : ", headerDr("agentRef")}
                    For i = 0 To 5
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrTitle(i), NormalFontBold))
                        cell = New PdfPCell(phrase)
                        If i = 0 Then
                            cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER Or Rectangle.TOP_BORDER
                        ElseIf i = 5 Then
                            cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER Or Rectangle.TOP_BORDER
                        Else
                            cell.Border = Rectangle.BOTTOM_BORDER Or Rectangle.TOP_BORDER
                        End If
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        If i Mod 2 = 0 Then
                            cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                        Else
                            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                        End If
                        cell.Padding = 3
                        tblInv.AddCell(cell)
                    Next
                    document.Add(tblInv)
                    writer.PageEvent = New clsBookingConfirmPageEvents(tblInv, printMode)

                    If hotelDt.Rows.Count > 0 Then
                        Dim arrServ() As String = {"Hotel Services", "Chk. in", "Chk. Out", "Charges " & Convert.ToString(headerDr("currCode"))}
                        Dim tblServ As PdfPTable = New PdfPTable(4)
                        tblServ.TotalWidth = documentWidth
                        tblServ.LockedWidth = True
                        tblServ.SetWidths(New Single() {0.63F, 0.12F, 0.12F, 0.13F})
                        tblServ.Complete = False
                        tblServ.HeaderRows = 1
                        tblServ.SplitRows = False
                        For i = 0 To 3
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrServ(i), NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = titleColor
                            tblServ.AddCell(cell)
                        Next
                        For Each hotelDr As DataRow In hotelDt.Rows
                            Dim tblTariff As PdfPTable = New PdfPTable(2)
                            tblTariff.SetWidths(New Single() {0.05F, 0.95F})
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("partyName")) & vbLf, NormalFontBold))
                            phrase.Add(New Chunk(Convert.ToString(ServiceRemarksParce(hotelDr)) & vbLf, SmallFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            cell.Colspan = 2
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            phrase.Add(New Chunk("", NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                            cell.PaddingBottom = 3.0F
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("noofrooms")) & "  " & Convert.ToString(hotelDr("RoomDetail")) & vbLf, NormalFont))
                            If Convert.ToString(hotelDr("occupancy")) <> "" Then
                                phrase.Add(New Chunk("[ " & Convert.ToString(hotelDr("occupancy")) & " ]", NormalFont))
                            End If

                            Dim rLineNo As Integer = Convert.ToInt32(hotelDr("rLineNo"))
                            Dim roomNo As Integer = Convert.ToInt32(hotelDr("roomNo"))
                            Dim tariffFilter = (From n In tariffDt.AsEnumerable() Where n.Field(Of Int32)("rLineNo") = rLineNo And n.Field(Of Int32)("roomNo") = roomNo Select n Order By Convert.ToDateTime(n.Field(Of String)("fromDate")) Ascending).ToList()
                            Dim filterTariffDt As New DataTable
                            If (tariffFilter.Count > 0) Then filterTariffDt = tariffFilter.CopyToDataTable()
                            If filterTariffDt.Rows.Count > 0 Then
                                For Each ratesDr As DataRow In filterTariffDt.Rows
                                    phrase.Add(New Chunk(vbLf + "From " + Convert.ToString(ratesDr("fromDate")) & " " & Convert.ToString(ratesDr("nights")) & " Nts * " & Convert.ToString(ratesDr("salePrice")) & " * " & Convert.ToString(hotelDr("noofrooms")) & " Units = ", NormalFont))
                                    phrase.Add(New Chunk(Convert.ToString(ratesDr("saleValue")) & " " & Convert.ToString(headerDr("currCode")), NormalFontBold))
                                    If ratesDr("bookingCode") <> "" Then
                                        phrase.Add(New Chunk(vbCrLf & "( " + Convert.ToString(ratesDr("bookingCode")) + " )", NormalFontBold))
                                    End If
                                Next
                            End If
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            If Convert.ToString(hotelDr("hotelConfNo")) <> "" Then
                                phrase.Add(New Chunk("Hotel Conf No : " & Convert.ToString(hotelDr("hotelConfNo")), NormalFontBold))
                            End If

                            ' ''' Added shahul 25/07/2018
                            'Dim guestFilter = (From n In guestDt.AsEnumerable() Where n.Field(Of Int32)("rLineNo") = rLineNo And n.Field(Of Int32)("roomNo") = roomNo).ToList()
                            'Dim guestFilterDt As New DataTable
                            'If (guestFilter.Count > 0) Then guestFilterDt = guestFilter.CopyToDataTable()
                            'If guestFilterDt.Rows.Count > 0 Then
                            '    phrase.Add(New Chunk("Guest Names : ", NormalFont))
                            '    For Each guestDr As DataRow In guestFilterDt.Rows
                            '        If guestDr("Guestname") <> "" Then
                            '            phrase.Add(New Chunk(vbCrLf & Convert.ToString(guestDr("Guestname")), NormalFont))
                            '        End If
                            '    Next
                            'End If
                            If Showguestroomwise = "1" Then
                                Dim guestFilter = (From n In guestDt.AsEnumerable() Where n.Field(Of Int32)("rLineNo") = rLineNo).ToList() 'And n.Field(Of Int32)("roomNo") = roomNo
                                Dim guestFilterDt As New DataTable
                                If (guestFilter.Count > 0) Then guestFilterDt = guestFilter.CopyToDataTable()
                                If guestFilterDt.Rows.Count > 0 Then
                                    Dim tblGuestnew As PdfPTable = New PdfPTable(2)
                                    ' tblGuestnew.TotalWidth = documentWidth
                                    tblGuestnew.SetWidths(New Single() {0.7F, 0.3F})
                                    'tblGuestnew.LockedWidth = True
                                    tblGuestnew.Complete = False
                                    tblGuestnew.SplitRows = False
                                    'tblGuestnew.HeaderRows = 1
                                    GuestListnew(tblGuestnew, guestFilterDt)
                                    tblGuestnew.Complete = True

                                    cell = New PdfPCell(tblGuestnew)
                                    cell.Colspan = 2
                                    tblTariff.AddCell(cell)

                                End If
                            End If



                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            cell.Colspan = 2
                            tblTariff.AddCell(cell)
                            tblServ.AddCell(tblTariff)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("checkIn")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            tblServ.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("checkOut")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 2.0F
                            tblServ.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("salevalue")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 2.0F
                            cell.PaddingRight = 4.0F
                            tblServ.AddCell(cell)

                            If SplEventDt.Rows.Count > 0 Then
                                Dim partyCode As String = hotelDr("partyCode").ToString()
                                Dim index As Integer = hotelDt.Rows.IndexOf(hotelDr)
                                Dim i As Integer = 0
                                Dim lastIndex As Integer = index
                                Dim filterRows = (From n In SplEventDt.AsEnumerable() Where n.Field(Of String)("PartyCode") = partyCode)
                                If filterRows.Count > 0 Then
                                    Dim filterHotelRows = (From n In hotelDt.AsEnumerable() Where n.Field(Of String)("PartyCode") = partyCode)
                                    While i < filterHotelRows.Count
                                        If hotelDt.Rows.IndexOf(filterHotelRows(i)) > index Then
                                            lastIndex = hotelDt.Rows.IndexOf(filterHotelRows(i))
                                            Exit While
                                        End If
                                        i = i + 1
                                    End While
                                End If
                                If index = lastIndex Then
                                    Dim filterSplEvt As New DataTable
                                    If (filterRows.Count > 0) Then filterSplEvt = filterRows.CopyToDataTable()
                                    If filterSplEvt.Rows.Count > 0 Then
                                        Dim tblSplEvent As New PdfPTable(6)
                                        Dim currCode As String = Convert.ToString(headerDr("currCode"))
                                        SpecialEvents(filterSplEvt, documentWidth, currCode, tblSplEvent)
                                        cell = New PdfPCell(tblSplEvent)
                                        cell.Colspan = 4
                                        cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                                        tblServ.AddCell(cell)
                                    End If
                                End If
                            End If
                        Next
                        tblServ.Complete = True
                        tblServ.SpacingBefore = 7
                        document.Add(tblServ)
                    End If

                    If othServDt.Rows.Count > 0 Or airportDt.Rows.Count > 0 Or visaDt.Rows.Count > 0 Or tourDt.Rows.Count > 0 Or OtherDt.Rows.Count > 0 Then
                        Dim OthServ() As String = {"Other Services", "Date of Service", "Units/ Pax", "Rate per Units/Pax", "Charges " & Convert.ToString(headerDr("currCode"))}
                        Dim tblOthServ As PdfPTable = New PdfPTable(5)
                        tblOthServ.TotalWidth = documentWidth
                        tblOthServ.LockedWidth = True
                        tblOthServ.SetWidths(New Single() {0.57F, 0.12F, 0.09F, 0.11F, 0.13F})
                        tblOthServ.SplitRows = False
                        tblOthServ.Complete = False
                        tblOthServ.HeaderRows = 1
                        For i = 0 To 4
                            phrase = New Phrase()
                            phrase.Add(New Chunk(OthServ(i), NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = titleColor
                            tblOthServ.AddCell(cell)
                        Next

                        Dim MergeDt As DataTable = New DataTable()
                        Dim OthServType As DataColumn = New DataColumn("OthServType", GetType(String))
                        Dim ServiceRemark As DataColumn = New DataColumn("ServiceRemark", GetType(String))
                        Dim ServiceName As DataColumn = New DataColumn("ServiceName", GetType(String))
                        Dim ServiceDate As DataColumn = New DataColumn("ServiceDate", GetType(String))
                        Dim Unit As DataColumn = New DataColumn("Unit", GetType(String))
                        Dim UnitPrice As DataColumn = New DataColumn("UnitPrice", GetType(Decimal))
                        Dim UnitSaleValue As DataColumn = New DataColumn("UnitSaleValue", GetType(Decimal))
                        Dim Adults As DataColumn = New DataColumn("Adults", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim Child As DataColumn = New DataColumn("Child", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim Senior As DataColumn = New DataColumn("Senior", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim PickUpDropOff As DataColumn = New DataColumn("PickUpDropOff", GetType(String)) With {.DefaultValue = DBNull.Value}
                        Dim Sic As DataColumn = New DataColumn("Sic", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        MergeDt.Columns.Add(OthServType)
                        MergeDt.Columns.Add(ServiceName)
                        MergeDt.Columns.Add(ServiceRemark)
                        MergeDt.Columns.Add(ServiceDate)
                        MergeDt.Columns.Add(Unit)
                        MergeDt.Columns.Add(UnitPrice)
                        MergeDt.Columns.Add(UnitSaleValue)
                        MergeDt.Columns.Add(Adults)
                        MergeDt.Columns.Add(Child)
                        MergeDt.Columns.Add(Senior)
                        MergeDt.Columns.Add(PickUpDropOff)
                        MergeDt.Columns.Add(Sic)
                        Dim strRemarks As String = ""
                        For Each othServDr As DataRow In othServDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Transfer"
                            MergeDr("ServiceName") = othServDr("transferName")
                            MergeDr("ServiceRemark") = ServiceRemarksParce(othServDr)
                            MergeDr("ServiceDate") = othServDr("transferDate")
                            MergeDr("Unit") = othServDr("units")
                            MergeDr("UnitPrice") = othServDr("unitPrice")
                            MergeDr("UnitSaleValue") = othServDr("unitSaleValue")
                            MergeDr("Adults") = othServDr("Adults")
                            MergeDr("Child") = othServDr("Child")
                            MergeDr("PickUpDropOff") = othServDr("PickUpDropOff")
                            MergeDr("Sic") = othServDr("Sic")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each VisaDr As DataRow In visaDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Visa"
                            MergeDr("ServiceName") = VisaDr("visaName")
                            MergeDr("ServiceRemark") = ServiceRemarksParce(VisaDr)
                            MergeDr("ServiceDate") = VisaDr("VisaDate")
                            MergeDr("Unit") = VisaDr("noOfvisas")
                            MergeDr("UnitPrice") = VisaDr("visaPrice")
                            MergeDr("UnitSaleValue") = VisaDr("visaValue")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each airportDr As DataRow In airportDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Airport"
                            MergeDr("ServiceName") = airportDr("airportmaname")
                            MergeDr("ServiceRemark") = ServiceRemarksParce(airportDr)
                            MergeDr("ServiceDate") = airportDr("airportmadate")
                            MergeDr("Unit") = airportDr("units")
                            MergeDr("UnitPrice") = airportDr("unitPrice")
                            MergeDr("UnitSaleValue") = airportDr("unitSaleValue")
                            MergeDr("Adults") = airportDr("Adults")
                            MergeDr("Child") = airportDr("Child")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each tourDr As DataRow In tourDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Tour"
                            MergeDr("ServiceName") = tourDr("tourname")
                            MergeDr("ServiceRemark") = ServiceRemarksParce(tourDr)
                            MergeDr("ServiceDate") = tourDr("tourdate")
                            MergeDr("Unit") = tourDr("units")
                            MergeDr("UnitPrice") = tourDr("unitPrice")
                            MergeDr("UnitSaleValue") = tourDr("unitSaleValue")
                            MergeDr("Adults") = tourDr("Adults")
                            MergeDr("Child") = tourDr("Child")
                            MergeDr("Senior") = tourDr("Senior")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each otherDr As DataRow In OtherDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Other"


                            MergeDr("ServiceRemark") = ServiceRemarksParce(otherDr)
                            MergeDr("ServiceName") = otherDr("othername")
                            MergeDr("ServiceDate") = otherDr("othdate")
                            MergeDr("Unit") = otherDr("units")
                            MergeDr("UnitPrice") = otherDr("unitPrice")
                            MergeDr("UnitSaleValue") = otherDr("unitSaleValue")
                            MergeDr("Adults") = otherDr("Adults")
                            MergeDr("Child") = otherDr("Child")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        If MergeDt.Rows.Count > 0 Then
                            Dim MergeOrderDt As DataTable = (From n In MergeDt.AsEnumerable() Select n Order By Convert.ToDateTime(n.Field(Of String)("ServiceDate")) Ascending).CopyToDataTable()
                            AppendOtherServices(tblOthServ, MergeOrderDt)
                        End If
                        tblOthServ.Complete = True
                        tblOthServ.SpacingBefore = 7
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 72 Then document.NewPage()
                        document.Add(tblOthServ)
                    End If

                    Dim tblTotal As PdfPTable = New PdfPTable(2)
                    tblTotal.TotalWidth = documentWidth
                    tblTotal.LockedWidth = True
                    tblTotal.SetWidths(New Single() {0.8F, 0.2F})
                    tblTotal.KeepTogether = True

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Total Amount (" + Convert.ToString(headerDr("currcode")) + ")", NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 10.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("saleCurrency")), NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 4.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Total Amount (" + Convert.ToString(headerDr("baseCurrcode")) + ")", NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 10.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("saleValue")), NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 4.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    tblTotal.SpacingBefore = 7
                    document.Add(tblTotal)

                    '------- Tax Note ----------------
                    Dim tblTax As PdfPTable = New PdfPTable(2)
                    tblTax.TotalWidth = documentWidth
                    tblTax.LockedWidth = True
                    tblTax.SetWidths(New Single() {0.03, 0.97F})
                    tblTax.KeepTogether = True
                    tblTax.Complete = False
                    tblTax.SplitRows = False

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Chr(149), NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER
                    cell.PaddingLeft = 7.0F
                    cell.PaddingBottom = 3.0F
                    cell.PaddingTop = 3.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("ABOVE RATES ARE INCLUSIVE OF ALL TAXES INCLUDING VAT", NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER
                    cell.PaddingLeft = 2.0F
                    cell.PaddingBottom = 3.0F
                    cell.PaddingTop = 3.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Chr(149), NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingLeft = 7.0F
                    cell.PaddingBottom = 5.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("ABOVE RATES DOES NOT INCLUDE TOURISM DIRHAM FEE WHICH IS TO BE PAID BY THE GUEST DIRECTLY AT THE HOTEL", NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingLeft = 2.0F
                    cell.PaddingBottom = 5.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    tblTax.Complete = True
                    tblTax.SpacingBefore = 7
                    document.Add(tblTax)

                    If guestDt.Rows.Count > 0 Then
                        Dim tblGuest As PdfPTable = New PdfPTable(3)
                        tblGuest.TotalWidth = documentWidth
                        tblGuest.LockedWidth = True
                        tblGuest.Complete = False
                        tblGuest.SplitRows = False
                        tblGuest.HeaderRows = 1
                        GuestList(tblGuest, guestDt)
                        tblGuest.Complete = True
                        tblGuest.SpacingBefore = 7
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 48 Then document.NewPage()
                        document.Add(tblGuest)
                    End If




                    Dim tblFooter As New PdfPTable(1)
                    tblFooter.TotalWidth = documentWidth
                    tblFooter.LockedWidth = True
                    tblFooter.Complete = False
                    tblFooter.SetWidths({1.0F})
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Thanks and Best Regards", NormalFont))
                    cell = New PdfPCell(phrase)
                    cell.BorderWidth = 0.7F
                    cell.Border = Rectangle.NO_BORDER
                    cell.PaddingTop = 6.0F
                    cell.PaddingLeft = 15.0F
                    tblFooter.AddCell(cell)

                    If contactDt.Rows.Count > 0 Then
                        Dim contractDr As DataRow = contactDt.Rows(0)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(contractDr("salesPerson")) + "<" + Convert.ToString(contractDr("salesemail")) + ">" + vbCrLf + "DESTINATION SPECIALIST", NormalFont))
                        cell = New PdfPCell(phrase)
                        cell.BorderWidth = 0.7F
                        cell.BorderColor = BaseColor.WHITE
                        cell.SetLeading(15, 0)
                        cell.PaddingTop = 35.0F
                        tblFooter.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk("Mobile No - " + Convert.ToString(contractDr("salesmobile")), NormalFont))
                        cell = New PdfPCell(phrase)
                        cell.BorderWidth = 0.7F
                        cell.BorderColor = BaseColor.WHITE
                        cell.SetLeading(15, 0)
                        cell.PaddingTop = 14.0F
                        tblFooter.AddCell(cell)
                    End If
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Any Discrepancy on the above Invoice to be revert back within 72 hours from the date of Confirmation or else treated as final", NormalFont))
                    cell = New PdfPCell(phrase)
                    cell.BorderWidth = 0.7F
                    cell.BorderColor = BaseColor.WHITE
                    cell.SetLeading(15, 0)
                    cell.PaddingTop = 3.0F
                    tblFooter.AddCell(cell)
                    tblFooter.Complete = True
                    document.Add(tblFooter)

                    'nv 27/02/2021 uncommented below portion and commented image portion
                    If BankDt.Rows.Count > 0 Then
                        Dim tblBank As PdfPTable = New PdfPTable(2)
                        tblBank.TotalWidth = documentWidth
                        tblBank.LockedWidth = True
                        tblBank.Complete = False
                        tblBank.SplitRows = False
                        BankDetails(tblBank, BankDt)
                        tblBank.Complete = True
                        tblBank.SpacingBefore = 7
                        tblBank.KeepTogether = True
                        document.Add(tblBank)
                    End If

                    'Dim strBankImage As String = "RP_RAKBank.jpg"
                    'strBankImage = Convert.ToString(headerDr("BankImage"))
                    'If strBankImage <> "" Then
                    '    Dim tblBank As PdfPTable = New PdfPTable(1)
                    '    ' tblLogo.SetWidths(New Single() {0.27F, 0.73F})
                    '    'Company Name 
                    '    phrase = New Phrase()
                    '    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    '    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    '    cell.PaddingBottom = 4
                    '    tblBank.AddCell(cell)
                    '    cell = ImageCell("~/Images/" & strBankImage, 500.0F, PdfPCell.ALIGN_CENTER)
                    '    tblBank.AddCell(cell)
                    '    document.Add(tblBank)
                    'End If


                    'If (headerDr("div_code") = "01") Then
                    '    Dim tblBank As PdfPTable = New PdfPTable(1)
                    '    tblBank.TotalWidth = documentWidth
                    '    tblBank.LockedWidth = True
                    '    Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/Images/RPT_Bank1.png"))
                    '    image.ScalePercent(100, 100)
                    '    cell = New PdfPCell(image, True)
                    '    cell.Border = Rectangle.NO_BORDER
                    '    tblBank.AddCell(cell)
                    '    Dim image1 As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/Images/RPT_Bank2.png"))
                    '    image1.ScalePercent(100, 100)
                    '    cell = New PdfPCell(image1, True)
                    '    cell.Border = Rectangle.NO_BORDER
                    '    tblBank.AddCell(cell)
                    '    document.Add(tblBank)
                    'Else

                    '    Dim tblBank As PdfPTable = New PdfPTable(1)
                    '    ' tblLogo.SetWidths(New Single() {0.27F, 0.73F})
                    '    'Company Name 
                    '    phrase = New Phrase()
                    '    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    '    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    '    cell.PaddingBottom = 4
                    '    tblBank.AddCell(cell)
                    '    cell = ImageCell("~/Images/RGT-Bank.png", 500.0F, PdfPCell.ALIGN_CENTER)
                    '    tblBank.AddCell(cell)
                    '    document.Add(tblBank)
                    'End If


                    document.AddTitle(Convert.ToString(headerDr("printHeader")) & "-" & requestID)
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
            Else
                Throw New Exception("There is no rows in header table")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region


#Region "Protected Sub GuestListnew(ByRef tblGuest As PdfPTable, ByVal guestDt As DataTable)"
    Protected Sub GuestListnew(ByRef tblGuest As PdfPTable, ByVal guestDt As DataTable)
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        phrase = New Phrase()
        'tblGuest.SetWidths(New Single() {0.45F, 0.55F})
        Dim guestHeader() As String = {"Name of the Guest(s)", "Child D-O-B"}
        For i = 0 To 1
            phrase = New Phrase()
            phrase.Add(New Chunk(guestHeader(i), NormalFontBold))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 4.0F
            cell.PaddingTop = 1.0F
            cell.BackgroundColor = titleColor
            tblGuest.AddCell(cell)
        Next
        For Each guestDr As DataRow In guestDt.Rows
            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(guestDr(0)), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.SetLeading(12, 0)
            cell.PaddingBottom = 3.0F
            tblGuest.AddCell(cell)

            Dim age As String = IIf(guestDr(1) = 0.0, "", Convert.ToString(Math.Round(guestDr(1))))
            phrase = New Phrase()
            phrase.Add(New Chunk(age, NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            tblGuest.AddCell(cell)

            'phrase = New Phrase()
            'phrase.Add(New Chunk(Convert.ToString(guestDr(2)), NormalFont))
            'cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            'cell.PaddingBottom = 3.0F
            'tblGuest.AddCell(cell)
        Next
        'phrase = New Phrase()
        'phrase.Add(New Chunk("Total No. of Pax = " & Convert.ToString(guestDt.Rows.Count), NormalFontBold))
        'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
        'cell.Colspan = 2
        'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        'cell.PaddingLeft = 5.0F
        'cell.PaddingBottom = 3.0F
        'tblGuest.AddCell(cell)
    End Sub
#End Region

#Region "Protected Sub AppendOtherServices(ByRef tblOthServ As PdfPTable, ByVal inputDt As DataTable)"
    Protected Sub AppendOtherServices(ByRef tblOthServ As PdfPTable, ByVal inputDt As DataTable)
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        For Each inputDr As DataRow In inputDt.Rows
            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(inputDr("ServiceName")), NormalFont))
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True, "Yes")
            Else
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
            End If
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.SetLeading(12, 0)
            cell.PaddingBottom = 3.0F
            tblOthServ.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(inputDr("ServiceDate")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell.Rowspan = 2
            End If
            tblOthServ.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(inputDr("Unit")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell.Rowspan = 2
            End If
            tblOthServ.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(inputDr("UnitPrice")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            cell.PaddingRight = 4.0F
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell.Rowspan = 2
            End If
            tblOthServ.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(inputDr("UnitSaleValue")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            cell.PaddingRight = 4.0F
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell.Rowspan = 2
            End If
            tblOthServ.AddCell(cell)

            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                phrase = New Phrase()
                phrase.Add(New Chunk(Convert.ToString(inputDr("pickupdropoff")), NormalFont))
                If Convert.ToInt32(inputDr("sic")) <> 1 Then
                    phrase.Add(New Chunk(" (" & inputDr("adults").ToString() & " Adults", NormalFont))
                    If String.IsNullOrEmpty(Convert.ToString(inputDr("child")).Trim()) Or Convert.ToString(inputDr("child")).Trim() = "0" Then
                        phrase.Add(New Chunk(")", NormalFont))
                    Else
                        phrase.Add(New Chunk(", " & inputDr("child").ToString() & " Child)", NormalFont))
                    End If
                End If
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True, "No")
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                cell.SetLeading(12, 0)
                cell.PaddingBottom = 3.0F
                tblOthServ.AddCell(cell)
            End If
        Next
    End Sub
#End Region

#Region "Protected Sub SpecialEvents(ByVal splEventDt As DataTable, ByVal documentWidth As Single, ByVal CurrCode As String, ByRef tblSplEvent As PdfPTable)"
    Protected Sub SpecialEvents(ByVal splEventDt As DataTable, ByVal documentWidth As Single, ByVal CurrCode As String, ByRef tblSplEvent As PdfPTable)
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
            phrase.Add(New Chunk(arrSplEvent(i), NormalFontBold))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 4.0F
            cell.PaddingTop = 1.0F
            cell.BackgroundColor = splEventTitleColor
            tblSplEvent.AddCell(cell)
        Next
        For Each splEventDr As DataRow In splEventDt.Rows
            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(splEventDr("splEventName")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.SetLeading(12, 0)
            cell.PaddingLeft = 3.0F
            cell.PaddingBottom = 4.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(splEventDr("splEventDate")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(splEventDr("noOfPax")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(splEventDr("paxType")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(splEventDr("paxRate")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            cell.PaddingRight = 4.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(splEventDr("splEventValue")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            cell.PaddingRight = 4.0F
            tblSplEvent.AddCell(cell)
        Next
        tblSplEvent.Complete = True
    End Sub
#End Region

#Region "Protected Sub GuestList(ByRef tblGuest As PdfPTable, ByVal guestDt As DataTable)"
    Protected Sub GuestList(ByRef tblGuest As PdfPTable, ByVal guestDt As DataTable)
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        phrase = New Phrase()
        tblGuest.SetWidths(New Single() {0.64F, 0.18F, 0.18F})
        Dim guestHeader() As String = {"Name of the Guest(s)", "Child D-O-B", "Arrival"}
        For i = 0 To 2
            phrase = New Phrase()
            phrase.Add(New Chunk(guestHeader(i), NormalFontBold))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 4.0F
            cell.PaddingTop = 1.0F
            cell.BackgroundColor = titleColor
            tblGuest.AddCell(cell)
        Next
        For Each guestDr As DataRow In guestDt.Rows
            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(guestDr(0)), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.SetLeading(12, 0)
            cell.PaddingBottom = 3.0F
            tblGuest.AddCell(cell)

            Dim age As String = IIf(guestDr(1) = 0.0, "", Convert.ToString(Math.Round(guestDr(1))))
            phrase = New Phrase()
            phrase.Add(New Chunk(age, NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            tblGuest.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(guestDr(2)), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            tblGuest.AddCell(cell)
        Next
        phrase = New Phrase()
        phrase.Add(New Chunk("Total No. of Pax = " & Convert.ToString(guestDt.Rows.Count), NormalFontBold))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
        cell.Colspan = 3
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.PaddingLeft = 5.0F
        cell.PaddingBottom = 3.0F
        tblGuest.AddCell(cell)
    End Sub
#End Region

    Protected Sub GuestFlightList(ByRef tblGuest As PdfPTable, ByVal guestDt As DataTable)
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        phrase = New Phrase()
        tblGuest.SetWidths(New Single() {0.24F, 0.19F, 0.22F, 0.2F, 0.2F, 0.2F, 0.2F})
        Dim guestHeader() As String = {"Name of the Guest(s)", "Arrival Date", "Airport", "Flight", "Departure Date", "Airport", "Flight"}
        For i = 0 To 6
            phrase = New Phrase()
            phrase.Add(New Chunk(guestHeader(i), NormalFontBold))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 4.0F
            cell.PaddingTop = 1.0F
            cell.BackgroundColor = titleColor
            tblGuest.AddCell(cell)
        Next
        For Each guestDr As DataRow In guestDt.Rows
            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(guestDr("GuestName")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.SetLeading(12, 0)
            cell.PaddingBottom = 3.0F
            tblGuest.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(guestDr("arrdate")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            tblGuest.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(guestDr("arairportbordername")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            tblGuest.AddCell(cell)


            Dim strArrFlight As String = ""
            If guestDr("arrflighttime").ToString = "" Then
                strArrFlight = guestDr("arrflightcode").ToString
            Else
                strArrFlight = guestDr("arrflightcode").ToString + " @" + guestDr("arrflighttime").ToString
            End If
            Dim strDepFlight As String = ""
            If guestDr("depflighttime").ToString = "" Then
                strDepFlight = guestDr("depflightcode").ToString
            Else
                strDepFlight = guestDr("depflightcode").ToString + " @" + guestDr("depflighttime").ToString
            End If

            phrase = New Phrase()
            phrase.Add(New Chunk(strArrFlight, NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            tblGuest.AddCell(cell)



            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(guestDr("depdate")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            tblGuest.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(guestDr("depairportbordername")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            tblGuest.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(strDepFlight, NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            tblGuest.AddCell(cell)



        Next
        phrase = New Phrase()
        phrase.Add(New Chunk("Total No. of Pax = " & Convert.ToString(guestDt.Rows.Count), NormalFontBold))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
        cell.Colspan = 3
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.PaddingLeft = 5.0F
        cell.PaddingBottom = 3.0F
        tblGuest.AddCell(cell)
    End Sub

#Region "Protected Sub BankDetails(ByRef tblBank As PdfPTable, ByVal BankDt As DataTable)"
    Protected Sub BankDetails(ByRef tblBank As PdfPTable, ByVal BankDt As DataTable)
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        phrase = New Phrase()
        tblBank.SetWidths(New Single() {0.27F, 0.73F})
        phrase = New Phrase()
        phrase.Add(New Chunk("BENEFICIARY BANK DETAILS", NormalFontBold))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.PaddingTop = 2.0F
        cell.PaddingBottom = 5.0F
        cell.Colspan = 2
        cell.BackgroundColor = titleColor
        tblBank.AddCell(cell)

        Dim bankDr As DataRow = BankDt.Rows(0)
        Dim beneficiaryDetails() As String = {"BENEFICIARY NAME", Convert.ToString(bankDr("beneficiaryName")), "BENEFICIARY ADDRESS", Convert.ToString(bankDr("beneficiaryAddress")), "BANK NAME & ADDRESS", Convert.ToString(bankDr("bankName")) & ", " & Convert.ToString(bankDr(3)), _
         "ACCOUNT NUMBER", Convert.ToString(bankDr("accountNumber")), "IBAN NUMBER", Convert.ToString(bankDr("ibanNumber")), "SWIFT CODE", Convert.ToString(bankDr("swiftCode"))}
        For i = 0 To 11
            If i Mod 2 = 0 Then
                phrase = New Phrase()
                phrase.Add(New Chunk(beneficiaryDetails(i), NormalFontBold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
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
                    phrase.Add(New Chunk(beneficiaryDetails(i), NormalFontBoldRed))
                Else
                    phrase.Add(New Chunk(beneficiaryDetails(i), NormalFontBold))
                End If
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
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
        phrase.Add(New Chunk("Note : It is mandatory to mention the IBAN number for Bank Payment Transfer", NormalFontBold))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.SetLeading(12, 0)
        cell.PaddingTop = 2.0F
        cell.PaddingBottom = 5.0F
        cell.Colspan = 2
        tblBank.AddCell(cell)
    End Sub
#End Region

#Region "Private Shared Sub DrawLine(writer As PdfWriter, x1 As Single, y1 As Single, x2 As Single, y2 As Single, color As BaseColor)"
    Private Shared Sub DrawLine(ByVal writer As PdfWriter, ByVal x1 As Single, ByVal y1 As Single, ByVal x2 As Single, ByVal y2 As Single, ByVal color As BaseColor)
        Dim contentByte As PdfContentByte = writer.DirectContent
        contentByte.SetColorStroke(color)
        contentByte.MoveTo(x1, y1)
        contentByte.LineTo(x2, y2)
        contentByte.Stroke()
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
        image.ScaleAbsolute(scale, scale)
        Dim cell As New PdfPCell(image)
        cell.BorderColor = BaseColor.WHITE
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 0.0F
        cell.PaddingTop = 0.0F
        Return cell
    End Function
#End Region

#Region "Public Sub GenerateCumulativeReport(ByVal requestID As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "")"
    Public Sub GenerateCumulativeReportOld(ByVal requestID As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "")
        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("sp_booking_confirmation_print_Cumulative", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@RequestID", SqlDbType.VarChar, 20)).Value = requestID
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            ds1 = ds
            Dim headerDt As DataTable = ds.Tables(0)
            Dim hotelDt As DataTable = ds.Tables(1)
            Dim tariffDt As DataTable = ds.Tables(2)
            Dim othServDt As DataTable = ds.Tables(3)
            Dim airportDt As DataTable = ds.Tables(4)
            Dim visaDt As DataTable = ds.Tables(5)
            Dim tourDt As DataTable = ds.Tables(6)
            Dim OtherDt As DataTable = ds.Tables(7)
            Dim guestDt As DataTable = ds.Tables(8)
            Dim contactDt As DataTable = ds.Tables(9)
            Dim BankDt As DataTable = ds.Tables(10)
            Dim SplEventDt As DataTable = ds.Tables(11)
            clsDBConnect.dbConnectionClose(sqlConn)
            If headerDt.Rows.Count > 0 Then
                Dim document As New Document(PageSize.A4, 0.0F, 0.0F, 20.0F, 35.0F)
                Dim documentWidth As Single = 550.0F
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
                    Dim headerDr As DataRow = headerDt.Rows(0)
                    table = New PdfPTable(2)
                    table.TotalWidth = documentWidth
                    table.LockedWidth = True
                    table.SetWidths(New Single() {0.5F, 0.5F})
                    table.Complete = False
                    table.SplitRows = False
                    Dim tblLogo As PdfPTable = New PdfPTable(2)
                    tblLogo.SetWidths(New Single() {0.27F, 0.73F})
                    'Company Name 
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("division_master_des")), TitleFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    tblLogo.AddCell(cell)
                    'Company Logo
                    'If (headerDr("div_code") = "01") Then
                    '    cell = ImageCell("~/Images/Logo11LetterHead.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                    'Else
                    '    cell = ImageCell("~/Images/Logo21LetterHead.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                    'End If
                    If (headerDr("div_code") = "01") Then
                        cell = ImageCell("~/Images/Logo.png", 60.0F, PdfPCell.ALIGN_CENTER)
                    ElseIf (headerDr("div_code") = "02") Then
                        cell = ImageCell("~/Images/Logo.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                    Else
                        cell = ImageCell("~/Images/Logo.png", 60.0F, PdfPCell.ALIGN_CENTER)
                    End If
                    tblLogo.AddCell(cell)
                    'Company Address
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("address1")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Fax : " & Convert.ToString(headerDr("fax")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Tel : " & Convert.ToString(headerDr("tel")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("E-mail : " & Convert.ToString(headerDr("email")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Website : " & Convert.ToString(headerDr("website")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    tblLogo.AddCell(cell)
                    table.AddCell(tblLogo)

                    Dim tblClient As PdfPTable = New PdfPTable(2)
                    tblClient.SetWidths(New Single() {0.5F, 0.5F})
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentName")), TitleFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                    cell.SetLeading(11, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentAddress")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Tel : " & Convert.ToString(headerDr("agentTel")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Fax : " & Convert.ToString(headerDr("agentfax")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Email : " & Convert.ToString(headerDr("agentEmail")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Attn. : " & Convert.ToString(headerDr("agentContact")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    table.AddCell(tblClient)
                    table.Complete = True
                    document.Add(table)

                    Dim tblTitle As PdfPTable = New PdfPTable(1)
                    tblTitle.SetWidths(New Single() {1.0F})
                    tblTitle.TotalWidth = documentWidth
                    tblTitle.LockedWidth = True
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("printHeader")), TitleFontBigBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3.0F
                    cell.BackgroundColor = titleColor
                    tblTitle.AddCell(cell)
                    tblTitle.SpacingBefore = 7
                    document.Add(tblTitle)

                    Dim tblInv As PdfPTable = New PdfPTable(6)
                    tblInv.SetWidths(New Single() {0.12F, 0.14F, 0.12F, 0.14F, 0.12F, 0.25F})
                    tblInv.TotalWidth = documentWidth
                    tblInv.LockedWidth = True
                    tblInv.SplitRows = False
                    Dim arrTitle() As String = {"Invoice No : ", headerDr("requestID").ToString(), "Dated : ", headerDr("requestDate"), "Your Ref : ", headerDr("agentRef")}
                    For i = 0 To 5
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrTitle(i), NormalFontBold))
                        cell = New PdfPCell(phrase)
                        If i = 0 Then
                            cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER Or Rectangle.TOP_BORDER
                        ElseIf i = 5 Then
                            cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER Or Rectangle.TOP_BORDER
                        Else
                            cell.Border = Rectangle.BOTTOM_BORDER Or Rectangle.TOP_BORDER
                        End If
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        If i Mod 2 = 0 Then
                            cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                        Else
                            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                        End If
                        cell.Padding = 3
                        tblInv.AddCell(cell)
                    Next
                    document.Add(tblInv)
                    writer.PageEvent = New clsBookingConfirmPageEvents(tblInv, printMode)

                    If hotelDt.Rows.Count > 0 Then
                        Dim arrServ() As String = {"Hotel Services", "Chk. in", "Chk. Out"}
                        Dim tblServ As PdfPTable = New PdfPTable(3)
                        tblServ.TotalWidth = documentWidth
                        tblServ.LockedWidth = True
                        tblServ.SetWidths(New Single() {0.74F, 0.13F, 0.13F})
                        tblServ.Complete = False
                        tblServ.HeaderRows = 1
                        tblServ.SplitRows = False
                        For i = 0 To 2
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrServ(i), NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = titleColor
                            tblServ.AddCell(cell)
                        Next
                        For Each hotelDr As DataRow In hotelDt.Rows
                            Dim tblTariff As PdfPTable = New PdfPTable(2)
                            tblTariff.SetWidths(New Single() {0.05F, 0.95F})
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("partyName")) & vbLf, NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            cell.Colspan = 2
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            phrase.Add(New Chunk("", NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                            cell.PaddingBottom = 3.0F
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("RoomDetail")) & vbLf, NormalFont))

                            If Convert.ToString(hotelDr("occupancy")) <> "" Then
                                phrase.Add(New Chunk("[ " & Convert.ToString(hotelDr("occupancy")) & " ]", NormalFont))
                            End If

                            Dim rLineNo As Integer = Convert.ToInt32(hotelDr("rLineNo"))
                            Dim roomNo As Integer = Convert.ToInt32(hotelDr("roomNo"))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            If Convert.ToString(hotelDr("hotelConfNo")) <> "" Then

                                phrase.Add(New Chunk("Hotel Conf No : " & Convert.ToString(hotelDr("hotelConfNo")), NormalFontBold))
                            End If



                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            cell.Colspan = 2
                            tblTariff.AddCell(cell)
                            tblServ.AddCell(tblTariff)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("checkIn")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            tblServ.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("checkOut")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 2.0F
                            tblServ.AddCell(cell)

                            If SplEventDt.Rows.Count > 0 Then
                                Dim partyCode As String = hotelDr("partyCode").ToString()
                                Dim index As Integer = hotelDt.Rows.IndexOf(hotelDr)
                                Dim i As Integer = 0
                                Dim lastIndex As Integer = index
                                Dim filterRows = (From n In SplEventDt.AsEnumerable() Where n.Field(Of String)("PartyCode") = partyCode)
                                If filterRows.Count > 0 Then
                                    Dim filterHotelRows = (From n In hotelDt.AsEnumerable() Where n.Field(Of String)("PartyCode") = partyCode)
                                    While i < filterHotelRows.Count
                                        If hotelDt.Rows.IndexOf(filterHotelRows(i)) > index Then
                                            lastIndex = hotelDt.Rows.IndexOf(filterHotelRows(i))
                                            Exit While
                                        End If
                                        i = i + 1
                                    End While
                                End If
                                If index = lastIndex Then
                                    Dim filterSplEvt As New DataTable
                                    If (filterRows.Count > 0) Then filterSplEvt = filterRows.CopyToDataTable()
                                    If filterSplEvt.Rows.Count > 0 Then
                                        Dim tblSplEvent As New PdfPTable(4)
                                        Dim currCode As String = Convert.ToString(headerDr("currCode"))
                                        CumulativeSpecialEvents(filterSplEvt, documentWidth, tblSplEvent)
                                        cell = New PdfPCell(tblSplEvent)
                                        cell.Colspan = 3
                                        cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                                        tblServ.AddCell(cell)
                                    End If
                                End If
                            End If
                        Next
                        tblServ.Complete = True
                        tblServ.SpacingBefore = 7
                        document.Add(tblServ)
                    End If

                    If othServDt.Rows.Count > 0 Or airportDt.Rows.Count > 0 Or visaDt.Rows.Count > 0 Or tourDt.Rows.Count > 0 Or OtherDt.Rows.Count > 0 Then
                        Dim OthServ() As String = {"Other Services", "Units/ Pax", "Date of Service"}
                        Dim tblOthServ As PdfPTable = New PdfPTable(3)
                        tblOthServ.TotalWidth = documentWidth
                        tblOthServ.LockedWidth = True
                        tblOthServ.SetWidths(New Single() {0.74F, 0.13F, 0.13F})
                        tblOthServ.SplitRows = False
                        tblOthServ.Complete = False
                        tblOthServ.HeaderRows = 1
                        For i = 0 To 2
                            phrase = New Phrase()
                            phrase.Add(New Chunk(OthServ(i), NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = titleColor
                            tblOthServ.AddCell(cell)
                        Next

                        Dim MergeDt As DataTable = New DataTable()
                        Dim OthServType As DataColumn = New DataColumn("OthServType", GetType(String))
                        Dim ServiceName As DataColumn = New DataColumn("ServiceName", GetType(String))
                        Dim Unit As DataColumn = New DataColumn("Unit", GetType(String))
                        Dim ServiceDate As DataColumn = New DataColumn("ServiceDate", GetType(String))
                        Dim Adults As DataColumn = New DataColumn("Adults", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim Child As DataColumn = New DataColumn("Child", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim Senior As DataColumn = New DataColumn("Senior", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim PickUpDropOff As DataColumn = New DataColumn("PickUpDropOff", GetType(String)) With {.DefaultValue = DBNull.Value}
                        Dim Sic As DataColumn = New DataColumn("Sic", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        MergeDt.Columns.Add(OthServType)
                        MergeDt.Columns.Add(ServiceName)
                        MergeDt.Columns.Add(Unit)
                        MergeDt.Columns.Add(ServiceDate)
                        MergeDt.Columns.Add(Adults)
                        MergeDt.Columns.Add(Child)
                        MergeDt.Columns.Add(Senior)
                        MergeDt.Columns.Add(PickUpDropOff)
                        MergeDt.Columns.Add(Sic)
                        For Each othServDr As DataRow In othServDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Transfer"
                            MergeDr("ServiceName") = othServDr("transferName")
                            MergeDr("Unit") = othServDr("units")
                            MergeDr("ServiceDate") = othServDr("transferDate")
                            MergeDr("Adults") = othServDr("Adults")
                            MergeDr("Child") = othServDr("Child")
                            MergeDr("PickUpDropOff") = othServDr("PickUpDropOff")
                            MergeDr("Sic") = othServDr("Sic")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each VisaDr As DataRow In visaDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Visa"
                            MergeDr("ServiceName") = VisaDr("visaName")
                            MergeDr("Unit") = VisaDr("noOfvisas")
                            MergeDr("ServiceDate") = VisaDr("VisaDate")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each airportDr As DataRow In airportDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Airport"
                            MergeDr("ServiceName") = airportDr("airportmaname")
                            MergeDr("Unit") = airportDr("units")
                            MergeDr("ServiceDate") = airportDr("airportmadate")
                            MergeDr("Adults") = airportDr("Adults")
                            MergeDr("Child") = airportDr("Child")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each tourDr As DataRow In tourDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Tour"
                            MergeDr("ServiceName") = tourDr("tourname")
                            MergeDr("Unit") = tourDr("units")
                            MergeDr("ServiceDate") = tourDr("tourdate")
                            MergeDr("Adults") = tourDr("Adults")
                            MergeDr("Child") = tourDr("Child")
                            MergeDr("Senior") = tourDr("Senior")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each otherDr As DataRow In OtherDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Other"
                            MergeDr("ServiceName") = otherDr("othername")
                            MergeDr("Unit") = otherDr("units")
                            MergeDr("ServiceDate") = otherDr("othdate")
                            MergeDr("Adults") = otherDr("Adults")
                            MergeDr("Child") = otherDr("Child")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        If MergeDt.Rows.Count > 0 Then
                            Dim MergeOrderDt As DataTable = (From n In MergeDt.AsEnumerable() Select n Order By Convert.ToDateTime(n.Field(Of String)("ServiceDate")) Ascending).CopyToDataTable()
                            CumulativeOtherService(tblOthServ, MergeOrderDt)
                        End If
                        tblOthServ.Complete = True
                        tblOthServ.SpacingBefore = 7
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 72 Then document.NewPage()
                        document.Add(tblOthServ)
                    End If

                    Dim tblTotal As PdfPTable = New PdfPTable(2)
                    tblTotal.TotalWidth = documentWidth
                    tblTotal.LockedWidth = True
                    tblTotal.SetWidths(New Single() {0.8F, 0.2F})
                    tblTotal.KeepTogether = True

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Total Amount (" + Convert.ToString(headerDr("currcode")) + ")", NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 10.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("saleCurrency")), NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 4.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    If Convert.ToDecimal(headerDr("DiscountMarkup")) > 0 Then
                        phrase = New Phrase()
                        phrase.Add(New Chunk("Discount (" + Convert.ToString(headerDr("currcode")) + ")", NormalFontBold))
                        cell = New PdfPCell(phrase)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                        cell.Border = Rectangle.LEFT_BORDER
                        cell.PaddingTop = 3.0F
                        cell.PaddingRight = 10.0F
                        cell.PaddingBottom = 3.0F
                        tblTotal.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(headerDr("DiscountMarkup")), NormalFontBold))
                        cell = New PdfPCell(phrase)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                        cell.Border = Rectangle.RIGHT_BORDER
                        cell.PaddingTop = 3.0F
                        cell.PaddingRight = 4.0F
                        cell.PaddingBottom = 3.0F
                        tblTotal.AddCell(cell)
                    End If

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Net Amount (" + Convert.ToString(headerDr("currcode")) + ")", NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.LEFT_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 10.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("netSaleCurrency")), NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.RIGHT_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 4.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Total Amount (" + Convert.ToString(headerDr("baseCurrcode")) + ")", NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 10.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("SaleValue")), NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 4.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    tblTotal.SpacingBefore = 7
                    document.Add(tblTotal)

                    '------- Tax Note ----------------
                    Dim tblTax As PdfPTable = New PdfPTable(2)
                    tblTax.TotalWidth = documentWidth
                    tblTax.LockedWidth = True
                    tblTax.SetWidths(New Single() {0.03, 0.97F})
                    tblTax.KeepTogether = True
                    tblTax.Complete = False
                    tblTax.SplitRows = False

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Chr(149), NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER
                    cell.PaddingLeft = 7.0F
                    cell.PaddingBottom = 3.0F
                    cell.PaddingTop = 3.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("ABOVE RATES ARE INCLUSIVE OF ALL TAXES INCLUDING VAT", NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER
                    cell.PaddingLeft = 2.0F
                    cell.PaddingBottom = 3.0F
                    cell.PaddingTop = 3.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Chr(149), NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingLeft = 7.0F
                    cell.PaddingBottom = 5.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("ABOVE RATES DOES NOT INCLUDE TOURISM DIRHAM FEE WHICH IS TO BE PAID BY THE GUEST DIRECTLY AT THE HOTEL", NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingLeft = 2.0F
                    cell.PaddingBottom = 5.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    tblTax.Complete = True
                    tblTax.SpacingBefore = 7
                    document.Add(tblTax)

                    If guestDt.Rows.Count > 0 Then
                        Dim tblGuest As PdfPTable = New PdfPTable(3)
                        tblGuest.TotalWidth = documentWidth
                        tblGuest.LockedWidth = True
                        tblGuest.Complete = False
                        tblGuest.SplitRows = False
                        tblGuest.HeaderRows = 1
                        GuestList(tblGuest, guestDt)
                        tblGuest.Complete = True
                        tblGuest.SpacingBefore = 7
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 48 Then document.NewPage()
                        document.Add(tblGuest)
                    End If

                    Dim tblFooter As New PdfPTable(1)
                    tblFooter.TotalWidth = documentWidth
                    tblFooter.LockedWidth = True
                    tblFooter.Complete = False
                    tblFooter.SetWidths({1.0F})
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Thanks and Best Regards", NormalFont))
                    cell = New PdfPCell(phrase)
                    cell.BorderWidth = 0.7F
                    cell.Border = Rectangle.NO_BORDER
                    cell.PaddingTop = 6.0F
                    cell.PaddingLeft = 15.0F
                    tblFooter.AddCell(cell)

                    If contactDt.Rows.Count > 0 Then
                        Dim contractDr As DataRow = contactDt.Rows(0)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(contractDr("salesPerson")) + "<" + Convert.ToString(contractDr("salesemail")) + ">" + vbCrLf + "DESTINATION SPECIALIST", NormalFont))
                        cell = New PdfPCell(phrase)
                        cell.BorderWidth = 0.7F
                        cell.BorderColor = BaseColor.WHITE
                        cell.SetLeading(15, 0)
                        cell.PaddingTop = 35.0F
                        tblFooter.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk("Mobile No - " + Convert.ToString(contractDr("salesmobile")), NormalFont))
                        cell = New PdfPCell(phrase)
                        cell.BorderWidth = 0.7F
                        cell.BorderColor = BaseColor.WHITE
                        cell.SetLeading(15, 0)
                        cell.PaddingTop = 14.0F
                        tblFooter.AddCell(cell)
                    End If
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Any Discrepancy on the above Invoice to be revert back within 72 hours from the date of Confirmation or else treated as final", NormalFont))
                    cell = New PdfPCell(phrase)
                    cell.BorderWidth = 0.7F
                    cell.BorderColor = BaseColor.WHITE
                    cell.SetLeading(15, 0)
                    cell.PaddingTop = 3.0F
                    tblFooter.AddCell(cell)
                    tblFooter.Complete = True
                    document.Add(tblFooter)

                    If BankDt.Rows.Count > 0 Then
                        Dim tblBank As PdfPTable = New PdfPTable(2)
                        tblBank.TotalWidth = documentWidth
                        tblBank.LockedWidth = True
                        tblBank.Complete = False
                        tblBank.SplitRows = False
                        BankDetails(tblBank, BankDt)
                        tblBank.Complete = True
                        tblBank.SpacingBefore = 7
                        tblBank.KeepTogether = True
                        document.Add(tblBank)
                    End If
                    document.AddTitle(Convert.ToString(headerDr("printHeader")) & "-" & requestID)
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
            Else
                Throw New Exception("There is no rows in header table")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub GenerateCumulativeReport(ByVal requestID As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "")
        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("sp_booking_confirmation_print_Cumulative", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@RequestID", SqlDbType.VarChar, 20)).Value = requestID
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            ds1 = ds
            Dim headerDt As DataTable = ds.Tables(0)
            Dim hotelDt As DataTable = ds.Tables(1)
            Dim tariffDt As DataTable = ds.Tables(2)
            Dim othServDt As DataTable = ds.Tables(3)
            Dim airportDt As DataTable = ds.Tables(4)
            Dim visaDt As DataTable = ds.Tables(5)
            Dim tourDt As DataTable = ds.Tables(6)
            Dim OtherDt As DataTable = ds.Tables(7)
            Dim guestDt As DataTable = ds.Tables(8)
            Dim contactDt As DataTable = ds.Tables(9)
            Dim BankDt As DataTable = ds.Tables(10)
            Dim SplEventDt As DataTable = ds.Tables(11)
            clsDBConnect.dbConnectionClose(sqlConn)
            If headerDt.Rows.Count > 0 Then
                Dim document As New Document(PageSize.A4, 0.0F, 0.0F, 20.0F, 35.0F)
                Dim documentWidth As Single = 550.0F
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
                    Dim headerDr As DataRow = headerDt.Rows(0)
                    table = New PdfPTable(2)
                    table.TotalWidth = documentWidth
                    table.LockedWidth = True
                    table.SetWidths(New Single() {0.5F, 0.5F})
                    table.Complete = False
                    table.SplitRows = False
                    Dim tblLogo As PdfPTable = New PdfPTable(2)
                    tblLogo.SetWidths(New Single() {0.27F, 0.73F})
                    'Company Name 
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("division_master_des")), TitleFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    tblLogo.AddCell(cell)
                    'Company Logo
                    'If (headerDr("div_code") = "01") Then
                    '    cell = ImageCell("~/img/RoyalPark.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                    'Else
                    '    cell = ImageCell("~/img/Royalgulf.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                    'End If

                    If (headerDr("div_code") = "01") Then
                        cell = ImageCell("~/Images/Logo.png", 60.0F, PdfPCell.ALIGN_CENTER)
                    ElseIf (headerDr("div_code") = "02") Then
                        cell = ImageCell("~/Images/logo.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                    Else
                        cell = ImageCell("~/Images/Logo.png", 60.0F, PdfPCell.ALIGN_CENTER)
                    End If

                    tblLogo.AddCell(cell)
                    'Company Address
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("address1")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Fax : " & Convert.ToString(headerDr("fax")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Tel : " & Convert.ToString(headerDr("tel")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("E-mail : " & Convert.ToString(headerDr("email")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Website : " & Convert.ToString(headerDr("website")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    tblLogo.AddCell(cell)
                    table.AddCell(tblLogo)

                    Dim tblClient As PdfPTable = New PdfPTable(2)
                    tblClient.SetWidths(New Single() {0.5F, 0.5F})
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentName")), TitleFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                    cell.SetLeading(11, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentAddress")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Tel : " & Convert.ToString(headerDr("agentTel")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Fax : " & Convert.ToString(headerDr("agentfax")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Email : " & Convert.ToString(headerDr("agentEmail")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)


                    'Added by abin on 20181021 -- start
                    phrase = New Phrase()
                    phrase.Add(New Chunk("TRN NO : " & Convert.ToString(headerDr("TrnNo")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    'Added by abin on 20181021 -- End


                    phrase = New Phrase()
                    phrase.Add(New Chunk("Attn. : " & Convert.ToString(headerDr("agentContact")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    table.AddCell(tblClient)
                    table.Complete = True
                    document.Add(table)

                    Dim tblTitle As PdfPTable = New PdfPTable(1)
                    tblTitle.SetWidths(New Single() {1.0F})
                    tblTitle.TotalWidth = documentWidth
                    tblTitle.LockedWidth = True
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("printHeader")), TitleFontBigBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3.0F
                    cell.BackgroundColor = titleColor
                    tblTitle.AddCell(cell)
                    tblTitle.SpacingBefore = 7
                    document.Add(tblTitle)

                    Dim tblInv As PdfPTable = New PdfPTable(6)
                    tblInv.SetWidths(New Single() {0.12F, 0.14F, 0.12F, 0.14F, 0.12F, 0.25F})
                    tblInv.TotalWidth = documentWidth
                    tblInv.LockedWidth = True
                    tblInv.SplitRows = False
                    Dim arrTitle() As String = {"Invoice No : ", headerDr("requestID").ToString(), "Dated : ", headerDr("requestDate"), "Your Ref : ", headerDr("agentRef")}
                    For i = 0 To 5
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrTitle(i), NormalFontBold))
                        cell = New PdfPCell(phrase)
                        If i = 0 Then
                            cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER Or Rectangle.TOP_BORDER
                        ElseIf i = 5 Then
                            cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER Or Rectangle.TOP_BORDER
                        Else
                            cell.Border = Rectangle.BOTTOM_BORDER Or Rectangle.TOP_BORDER
                        End If
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        If i Mod 2 = 0 Then
                            cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                        Else
                            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                        End If
                        cell.Padding = 3
                        tblInv.AddCell(cell)
                    Next
                    document.Add(tblInv)
                    writer.PageEvent = New clsBookingConfirmPageEvents(tblInv, printMode)

                    If hotelDt.Rows.Count > 0 Then
                        Dim arrServ() As String = {"Hotel Services", "Chk. in", "Chk. Out"}
                        Dim tblServ As PdfPTable = New PdfPTable(3)
                        tblServ.TotalWidth = documentWidth
                        tblServ.LockedWidth = True
                        tblServ.SetWidths(New Single() {0.74F, 0.13F, 0.13F})
                        tblServ.Complete = False
                        tblServ.HeaderRows = 1
                        tblServ.SplitRows = False
                        For i = 0 To 2
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrServ(i), NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = titleColor
                            tblServ.AddCell(cell)
                        Next
                        For Each hotelDr As DataRow In hotelDt.Rows
                            Dim tblTariff As PdfPTable = New PdfPTable(2)
                            tblTariff.SetWidths(New Single() {0.05F, 0.95F})
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("partyName")) & vbLf, NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            cell.Colspan = 2
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            phrase.Add(New Chunk("", NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                            cell.PaddingBottom = 3.0F
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("RoomDetail")) & vbLf, NormalFont))

                            If Convert.ToString(hotelDr("occupancy")) <> "" Then
                                phrase.Add(New Chunk("[ " & Convert.ToString(hotelDr("occupancy")) & " ]", NormalFont))
                            End If

                            Dim rLineNo As Integer = Convert.ToInt32(hotelDr("rLineNo"))
                            Dim roomNo As Integer = Convert.ToInt32(hotelDr("roomNo"))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            If Convert.ToString(hotelDr("hotelConfNo")) <> "" Then

                                phrase.Add(New Chunk("Hotel Conf No : " & Convert.ToString(hotelDr("hotelConfNo")), NormalFontBold))
                            End If



                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            cell.Colspan = 2
                            tblTariff.AddCell(cell)
                            tblServ.AddCell(tblTariff)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("checkIn")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            tblServ.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("checkOut")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 2.0F
                            tblServ.AddCell(cell)

                            If SplEventDt.Rows.Count > 0 Then
                                Dim partyCode As String = hotelDr("partyCode").ToString()
                                Dim index As Integer = hotelDt.Rows.IndexOf(hotelDr)
                                Dim i As Integer = 0
                                Dim lastIndex As Integer = index
                                Dim filterRows = (From n In SplEventDt.AsEnumerable() Where n.Field(Of String)("PartyCode") = partyCode)
                                If filterRows.Count > 0 Then
                                    Dim filterHotelRows = (From n In hotelDt.AsEnumerable() Where n.Field(Of String)("PartyCode") = partyCode)
                                    While i < filterHotelRows.Count
                                        If hotelDt.Rows.IndexOf(filterHotelRows(i)) > index Then
                                            lastIndex = hotelDt.Rows.IndexOf(filterHotelRows(i))
                                            Exit While
                                        End If
                                        i = i + 1
                                    End While
                                End If
                                If index = lastIndex Then
                                    Dim filterSplEvt As New DataTable
                                    If (filterRows.Count > 0) Then filterSplEvt = filterRows.CopyToDataTable()
                                    If filterSplEvt.Rows.Count > 0 Then
                                        Dim tblSplEvent As New PdfPTable(4)
                                        Dim currCode As String = Convert.ToString(headerDr("currCode"))
                                        CumulativeSpecialEvents(filterSplEvt, documentWidth, tblSplEvent)
                                        cell = New PdfPCell(tblSplEvent)
                                        cell.Colspan = 3
                                        cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                                        tblServ.AddCell(cell)
                                    End If
                                End If
                            End If
                        Next
                        tblServ.Complete = True
                        tblServ.SpacingBefore = 7
                        document.Add(tblServ)
                    End If

                    If othServDt.Rows.Count > 0 Or airportDt.Rows.Count > 0 Or visaDt.Rows.Count > 0 Or tourDt.Rows.Count > 0 Or OtherDt.Rows.Count > 0 Then
                        Dim OthServ() As String = {"Other Services", "Units/ Pax", "Date of Service"}
                        Dim tblOthServ As PdfPTable = New PdfPTable(3)
                        tblOthServ.TotalWidth = documentWidth
                        tblOthServ.LockedWidth = True
                        tblOthServ.SetWidths(New Single() {0.74F, 0.13F, 0.13F})
                        tblOthServ.SplitRows = False
                        tblOthServ.Complete = False
                        tblOthServ.HeaderRows = 1
                        For i = 0 To 2
                            phrase = New Phrase()
                            phrase.Add(New Chunk(OthServ(i), NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = titleColor
                            tblOthServ.AddCell(cell)
                        Next

                        Dim MergeDt As DataTable = New DataTable()
                        Dim OthServType As DataColumn = New DataColumn("OthServType", GetType(String))
                        Dim ServiceName As DataColumn = New DataColumn("ServiceName", GetType(String))
                        Dim Unit As DataColumn = New DataColumn("Unit", GetType(String))
                        Dim ServiceDate As DataColumn = New DataColumn("ServiceDate", GetType(String))
                        Dim Adults As DataColumn = New DataColumn("Adults", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim Child As DataColumn = New DataColumn("Child", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim Senior As DataColumn = New DataColumn("Senior", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim PickUpDropOff As DataColumn = New DataColumn("PickUpDropOff", GetType(String)) With {.DefaultValue = DBNull.Value}
                        Dim Sic As DataColumn = New DataColumn("Sic", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        MergeDt.Columns.Add(OthServType)
                        MergeDt.Columns.Add(ServiceName)
                        MergeDt.Columns.Add(Unit)
                        MergeDt.Columns.Add(ServiceDate)
                        MergeDt.Columns.Add(Adults)
                        MergeDt.Columns.Add(Child)
                        MergeDt.Columns.Add(Senior)
                        MergeDt.Columns.Add(PickUpDropOff)
                        MergeDt.Columns.Add(Sic)
                        For Each othServDr As DataRow In othServDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Transfer"
                            MergeDr("ServiceName") = othServDr("transferName")
                            MergeDr("Unit") = othServDr("units")
                            MergeDr("ServiceDate") = othServDr("transferDate")
                            MergeDr("Adults") = othServDr("Adults")
                            MergeDr("Child") = othServDr("Child")
                            MergeDr("PickUpDropOff") = othServDr("PickUpDropOff")
                            MergeDr("Sic") = othServDr("Sic")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each VisaDr As DataRow In visaDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Visa"
                            MergeDr("ServiceName") = VisaDr("visaName")
                            MergeDr("Unit") = VisaDr("noOfvisas")
                            MergeDr("ServiceDate") = VisaDr("VisaDate")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each airportDr As DataRow In airportDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Airport"
                            MergeDr("ServiceName") = airportDr("airportmaname")
                            MergeDr("Unit") = airportDr("units")
                            MergeDr("ServiceDate") = airportDr("airportmadate")
                            MergeDr("Adults") = airportDr("Adults")
                            MergeDr("Child") = airportDr("Child")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each tourDr As DataRow In tourDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Tour"
                            MergeDr("ServiceName") = tourDr("tourname")
                            MergeDr("Unit") = tourDr("units")
                            MergeDr("ServiceDate") = tourDr("tourdate")
                            MergeDr("Adults") = tourDr("Adults")
                            MergeDr("Child") = tourDr("Child")
                            MergeDr("Senior") = tourDr("Senior")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each otherDr As DataRow In OtherDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Other"
                            MergeDr("ServiceName") = otherDr("othername")
                            MergeDr("Unit") = otherDr("units")
                            MergeDr("ServiceDate") = otherDr("othdate")
                            MergeDr("Adults") = otherDr("Adults")
                            MergeDr("Child") = otherDr("Child")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        If MergeDt.Rows.Count > 0 Then
                            Dim MergeOrderDt As DataTable = (From n In MergeDt.AsEnumerable() Select n Order By Convert.ToDateTime(n.Field(Of String)("ServiceDate")) Ascending).CopyToDataTable()
                            CumulativeOtherService(tblOthServ, MergeOrderDt)
                        End If
                        tblOthServ.Complete = True
                        tblOthServ.SpacingBefore = 7
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 72 Then document.NewPage()
                        document.Add(tblOthServ)
                    End If

                    Dim tblTotal As PdfPTable = New PdfPTable(2)
                    tblTotal.TotalWidth = documentWidth
                    tblTotal.LockedWidth = True
                    tblTotal.SetWidths(New Single() {0.8F, 0.2F})
                    tblTotal.KeepTogether = True

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Total Amount (" + Convert.ToString(headerDr("currcode")) + ")", NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 10.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("saleCurrency")), NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 4.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    If Convert.ToDecimal(headerDr("DiscountMarkup")) > 0 Then
                        phrase = New Phrase()
                        phrase.Add(New Chunk("Discount (" + Convert.ToString(headerDr("currcode")) + ")", NormalFontBold))
                        cell = New PdfPCell(phrase)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                        cell.Border = Rectangle.LEFT_BORDER
                        cell.PaddingTop = 3.0F
                        cell.PaddingRight = 10.0F
                        cell.PaddingBottom = 3.0F
                        tblTotal.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(headerDr("DiscountMarkup")), NormalFontBold))
                        cell = New PdfPCell(phrase)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                        cell.Border = Rectangle.RIGHT_BORDER
                        cell.PaddingTop = 3.0F
                        cell.PaddingRight = 4.0F
                        cell.PaddingBottom = 3.0F
                        tblTotal.AddCell(cell)
                    End If

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Net Amount (" + Convert.ToString(headerDr("currcode")) + ")", NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.LEFT_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 10.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("netSaleCurrency")), NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.RIGHT_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 4.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Total Amount (" + Convert.ToString(headerDr("baseCurrcode")) + ")", NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 10.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("SaleValue")), NormalFontBold))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 4.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    tblTotal.SpacingBefore = 7
                    document.Add(tblTotal)

                    '------- Tax Note ----------------
                    Dim tblTax As PdfPTable = New PdfPTable(2)
                    tblTax.TotalWidth = documentWidth
                    tblTax.LockedWidth = True
                    tblTax.SetWidths(New Single() {0.03, 0.97F})
                    tblTax.KeepTogether = True
                    tblTax.Complete = False
                    tblTax.SplitRows = False

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Chr(149), NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER
                    cell.PaddingLeft = 7.0F
                    cell.PaddingBottom = 3.0F
                    cell.PaddingTop = 3.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("ABOVE RATES ARE INCLUSIVE OF ALL TAXES INCLUDING VAT", NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER
                    cell.PaddingLeft = 2.0F
                    cell.PaddingBottom = 3.0F
                    cell.PaddingTop = 3.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Chr(149), NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingLeft = 7.0F
                    cell.PaddingBottom = 5.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("ABOVE RATES DOES NOT INCLUDE TOURISM DIRHAM FEE WHICH IS TO BE PAID BY THE GUEST DIRECTLY AT THE HOTEL", NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingLeft = 2.0F
                    cell.PaddingBottom = 5.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    tblTax.Complete = True
                    tblTax.SpacingBefore = 7
                    document.Add(tblTax)

                    If guestDt.Rows.Count > 0 Then
                        Dim tblGuest As PdfPTable = New PdfPTable(3)
                        tblGuest.TotalWidth = documentWidth
                        tblGuest.LockedWidth = True
                        tblGuest.Complete = False
                        tblGuest.SplitRows = False
                        tblGuest.HeaderRows = 1
                        GuestList(tblGuest, guestDt)
                        tblGuest.Complete = True
                        tblGuest.SpacingBefore = 7
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 48 Then document.NewPage()
                        document.Add(tblGuest)
                    End If

                    Dim tblFooter As New PdfPTable(1)
                    tblFooter.TotalWidth = documentWidth
                    tblFooter.LockedWidth = True
                    tblFooter.Complete = False
                    tblFooter.SetWidths({1.0F})
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Thanks and Best Regards", NormalFont))
                    cell = New PdfPCell(phrase)
                    cell.BorderWidth = 0.7F
                    cell.Border = Rectangle.NO_BORDER
                    cell.PaddingTop = 6.0F
                    cell.PaddingLeft = 15.0F
                    tblFooter.AddCell(cell)

                    If contactDt.Rows.Count > 0 Then
                        Dim contractDr As DataRow = contactDt.Rows(0)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(contractDr("salesPerson")) + "<" + Convert.ToString(contractDr("salesemail")) + ">" + vbCrLf + "DESTINATION SPECIALIST", NormalFont))
                        cell = New PdfPCell(phrase)
                        cell.BorderWidth = 0.7F
                        cell.BorderColor = BaseColor.WHITE
                        cell.SetLeading(15, 0)
                        cell.PaddingTop = 35.0F
                        tblFooter.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk("Mobile No - " + Convert.ToString(contractDr("salesmobile")), NormalFont))
                        cell = New PdfPCell(phrase)
                        cell.BorderWidth = 0.7F
                        cell.BorderColor = BaseColor.WHITE
                        cell.SetLeading(15, 0)
                        cell.PaddingTop = 14.0F
                        tblFooter.AddCell(cell)
                    End If
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Any Discrepancy on the above Invoice to be revert back within 72 hours from the date of Confirmation or else treated as final", NormalFont))
                    cell = New PdfPCell(phrase)
                    cell.BorderWidth = 0.7F
                    cell.BorderColor = BaseColor.WHITE
                    cell.SetLeading(15, 0)
                    cell.PaddingTop = 3.0F
                    tblFooter.AddCell(cell)
                    tblFooter.Complete = True
                    document.Add(tblFooter)

                    'If BankDt.Rows.Count > 0 Then
                    '    Dim tblBank As PdfPTable = New PdfPTable(2)
                    '    tblBank.TotalWidth = documentWidth
                    '    tblBank.LockedWidth = True
                    '    tblBank.Complete = False
                    '    tblBank.SplitRows = False
                    '    BankDetails(tblBank, BankDt)
                    '    tblBank.Complete = True
                    '    tblBank.SpacingBefore = 7
                    '    tblBank.KeepTogether = True
                    '    document.Add(tblBank)
                    'End If

                    Dim strBankImage As String = "RP_RAKBank.jpg"
                    strBankImage = Convert.ToString(headerDr("BankImage"))
                    If strBankImage <> "" Then
                        Dim tblBank As PdfPTable = New PdfPTable(1)
                        ' tblLogo.SetWidths(New Single() {0.27F, 0.73F})
                        'Company Name 
                        phrase = New Phrase()
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                        cell.PaddingBottom = 4
                        tblBank.AddCell(cell)
                        cell = ImageCell("~/Images/" & strBankImage, 500.0F, PdfPCell.ALIGN_CENTER)
                        tblBank.AddCell(cell)
                        document.Add(tblBank)
                    End If

                    'If (headerDr("div_code") = "01") Then
                    '    Dim tblBank As PdfPTable = New PdfPTable(1)
                    '    tblBank.TotalWidth = documentWidth
                    '    tblBank.LockedWidth = True
                    '    Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/Images/RPT_Bank1.png"))
                    '    image.ScalePercent(100, 100)
                    '    cell = New PdfPCell(image, True)
                    '    cell.Border = Rectangle.NO_BORDER
                    '    tblBank.AddCell(cell)
                    '    Dim image1 As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/Images/RPT_Bank2.png"))
                    '    image1.ScalePercent(100, 100)
                    '    cell = New PdfPCell(image1, True)
                    '    cell.Border = Rectangle.NO_BORDER
                    '    tblBank.AddCell(cell)
                    '    document.Add(tblBank)
                    'Else

                    '    Dim tblBank As PdfPTable = New PdfPTable(1)
                    '    ' tblLogo.SetWidths(New Single() {0.27F, 0.73F})
                    '    'Company Name 
                    '    phrase = New Phrase()
                    '    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    '    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    '    cell.PaddingBottom = 4
                    '    tblBank.AddCell(cell)
                    '    cell = ImageCell("~/Images/RGT-Bank.png", 500.0F, PdfPCell.ALIGN_CENTER)
                    '    tblBank.AddCell(cell)
                    '    document.Add(tblBank)
                    'End If

                    document.AddTitle(Convert.ToString(headerDr("printHeader")) & "-" & requestID)
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
            Else
                Throw New Exception("There is no rows in header table")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub GenerateVoucherReport(ByVal requestID As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "")
        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("sp_booking_confirmation_print", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@RequestID", SqlDbType.VarChar, 20)).Value = requestID
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            ds1 = ds
            Dim headerDt As DataTable = ds.Tables(0)
            Dim hotelDt As DataTable = ds.Tables(1)
            Dim tariffDt As DataTable = ds.Tables(2)
            Dim othServDt As DataTable = ds.Tables(3)
            Dim airportDt As DataTable = ds.Tables(4)
            Dim visaDt As DataTable = ds.Tables(5)
            Dim tourDt As DataTable = ds.Tables(6)
            Dim OtherDt As DataTable = ds.Tables(7)
            Dim guestDt As DataTable = ds.Tables(8)
            Dim contactDt As DataTable = ds.Tables(9)
            Dim BankDt As DataTable = ds.Tables(10)
            Dim SplEventDt As DataTable = ds.Tables(11)
            Dim GuestAndFlight As DataTable = ds.Tables(12)
            Dim EmContactDt As DataTable = ds.Tables(14)
            clsDBConnect.dbConnectionClose(sqlConn)
            If headerDt.Rows.Count > 0 Then
                Dim document As New Document(PageSize.A4, 0.0F, 0.0F, 20.0F, 35.0F)
                Dim documentWidth As Single = 550.0F
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
                    Dim headerDr As DataRow = headerDt.Rows(0)
                    table = New PdfPTable(1)
                    table.TotalWidth = documentWidth
                    table.LockedWidth = True
                    table.SetWidths(New Single() {0.9F})
                    table.Complete = False
                    table.SplitRows = False
                    Dim tblLogo As PdfPTable = New PdfPTable(2)
                    tblLogo.SetWidths(New Single() {0.27F, 0.73F})
                    'Company Name 
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("division_master_des")), TitleFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    tblLogo.AddCell(cell)
                    'Company Logo
                    'If (headerDr("div_code") = "01") Then
                    '    cell = ImageCell("~/img/RoyalPark.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                    'Else
                    '    cell = ImageCell("~/img/Royalgulf.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                    'End If

                    If (headerDr("div_code") = "01") Then
                        cell = ImageCell("~/Images/Logo.png", 60.0F, PdfPCell.ALIGN_CENTER)
                    ElseIf (headerDr("div_code") = "02") Then
                        cell = ImageCell("~/Images/logo.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                    Else
                        cell = ImageCell("~/Images/Logo.png", 60.0F, PdfPCell.ALIGN_CENTER)
                    End If

                    tblLogo.AddCell(cell)
                    'Company Address
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("address1")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Fax : " & Convert.ToString(headerDr("fax")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Tel : " & Convert.ToString(headerDr("tel")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("E-mail : " & Convert.ToString(headerDr("email")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Website : " & Convert.ToString(headerDr("website")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    tblLogo.AddCell(cell)
                    table.AddCell(tblLogo)

                    'Dim tblClient As PdfPTable = New PdfPTable(2)
                    'tblClient.SetWidths(New Single() {0.5F, 0.5F})
                    'phrase = New Phrase()
                    'phrase.Add(New Chunk(Convert.ToString(headerDr("agentName")), TitleFontBold))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                    'cell.SetLeading(11, 0)
                    'tblClient.AddCell(cell)
                    'phrase = New Phrase()
                    'phrase.Add(New Chunk(Convert.ToString(headerDr("agentAddress")), NormalFont))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(12, 0)
                    'tblClient.AddCell(cell)
                    'phrase = New Phrase()
                    'phrase.Add(New Chunk("Tel : " & Convert.ToString(headerDr("agentTel")), NormalFont))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(12, 0)
                    'tblClient.AddCell(cell)
                    'phrase = New Phrase()
                    'phrase.Add(New Chunk("Fax : " & Convert.ToString(headerDr("agentfax")), NormalFont))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(12, 0)
                    'tblClient.AddCell(cell)
                    'phrase = New Phrase()
                    'phrase.Add(New Chunk("Email : " & Convert.ToString(headerDr("agentEmail")), NormalFont))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(12, 0)
                    'tblClient.AddCell(cell)


                    ''Added by abin on 20181021 -- start
                    'phrase = New Phrase()
                    'phrase.Add(New Chunk("TRN NO : " & Convert.ToString(headerDr("TrnNo")), NormalFont))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.SetLeading(12, 0)
                    'tblClient.AddCell(cell)
                    ''Added by abin on 20181021 -- End


                    'phrase = New Phrase()
                    'phrase.Add(New Chunk("Attn. : " & Convert.ToString(headerDr("agentContact")), NormalFont))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    'cell.SetLeading(12, 0)
                    'tblClient.AddCell(cell)
                    'table.AddCell(tblClient)
                    table.Complete = True
                    document.Add(table)

                    Dim tblTitle As PdfPTable = New PdfPTable(1)
                    tblTitle.SetWidths(New Single() {1.0F})
                    tblTitle.TotalWidth = documentWidth
                    tblTitle.LockedWidth = True
                    phrase = New Phrase()
                    '  phrase.Add(New Chunk(Convert.ToString(headerDr("printHeader")), TitleFontBigBold))
                    phrase.Add(New Chunk("Voucher", TitleFontBigBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.PaddingBottom = 3.0F
                    cell.BackgroundColor = titleColor
                    tblTitle.AddCell(cell)
                    tblTitle.SpacingBefore = 7
                    document.Add(tblTitle)

                    Dim tblInv As PdfPTable = New PdfPTable(6)
                    tblInv.SetWidths(New Single() {0.12F, 0.14F, 0.12F, 0.14F, 0.12F, 0.25F})
                    tblInv.TotalWidth = documentWidth
                    tblInv.LockedWidth = True
                    tblInv.SplitRows = False
                    Dim arrTitle() As String = {"Invoice No : ", headerDr("requestID").ToString(), "Dated : ", headerDr("requestDate"), "Your Ref : ", headerDr("agentRef")}
                    For i = 0 To 5
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrTitle(i), NormalFontBold))
                        cell = New PdfPCell(phrase)
                        If i = 0 Then
                            cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER Or Rectangle.TOP_BORDER
                        ElseIf i = 5 Then
                            cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER Or Rectangle.TOP_BORDER
                        Else
                            cell.Border = Rectangle.BOTTOM_BORDER Or Rectangle.TOP_BORDER
                        End If
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        If i Mod 2 = 0 Then
                            cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                        Else
                            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                        End If
                        cell.Padding = 3
                        tblInv.AddCell(cell)
                    Next
                    document.Add(tblInv)
                    writer.PageEvent = New clsBookingConfirmPageEvents(tblInv, printMode)

                    If hotelDt.Rows.Count > 0 Then
                        Dim arrServ() As String = {"Hotel Services", "Chk. in", "Chk. Out"}
                        Dim tblServ As PdfPTable = New PdfPTable(3)
                        tblServ.TotalWidth = documentWidth
                        tblServ.LockedWidth = True
                        tblServ.SetWidths(New Single() {0.74F, 0.13F, 0.13F})
                        tblServ.Complete = False
                        tblServ.HeaderRows = 1
                        tblServ.SplitRows = False
                        For i = 0 To 2
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrServ(i), NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = titleColor
                            tblServ.AddCell(cell)
                        Next
                        For Each hotelDr As DataRow In hotelDt.Rows
                            Dim tblTariff As PdfPTable = New PdfPTable(2)
                            tblTariff.SetWidths(New Single() {0.05F, 0.95F})
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("partyName")) & vbLf, NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            cell.Colspan = 2
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            phrase.Add(New Chunk("", NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                            cell.PaddingBottom = 3.0F
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            Dim strroomNo As String = Convert.ToString(hotelDr("noofrooms"))
                            ' phrase.Add(New Chunk(Convert.ToString(hotelDr("RoomDetail") & " - " & strroomNo & " Room") & vbLf, NormalFont))
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("RoomDetail") & " - " & strroomNo & " Room") & vbLf, NormalFont))
                            If Convert.ToString(hotelDr("occupancy")) <> "" Then
                                phrase.Add(New Chunk("[ " & Convert.ToString(hotelDr("occupancy")) & " ]", NormalFont))
                            End If

                            Dim rLineNo As Integer = Convert.ToInt32(hotelDr("rLineNo"))

                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            If Convert.ToString(hotelDr("hotelConfNo")) <> "" Then

                                phrase.Add(New Chunk("Hotel Conf No : " & Convert.ToString(hotelDr("hotelConfNo")), NormalFontBold))
                            End If



                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            cell.Colspan = 2
                            tblTariff.AddCell(cell)
                            tblServ.AddCell(tblTariff)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("checkIn")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 3.0F
                            tblServ.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("checkOut")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 2.0F
                            tblServ.AddCell(cell)

                            If SplEventDt.Rows.Count > 0 Then
                                Dim partyCode As String = hotelDr("partyCode").ToString()
                                Dim index As Integer = hotelDt.Rows.IndexOf(hotelDr)
                                Dim i As Integer = 0
                                Dim lastIndex As Integer = index
                                Dim filterRows = (From n In SplEventDt.AsEnumerable() Where n.Field(Of String)("PartyCode") = partyCode)
                                If filterRows.Count > 0 Then
                                    Dim filterHotelRows = (From n In hotelDt.AsEnumerable() Where n.Field(Of String)("PartyCode") = partyCode)
                                    While i < filterHotelRows.Count
                                        If hotelDt.Rows.IndexOf(filterHotelRows(i)) > index Then
                                            lastIndex = hotelDt.Rows.IndexOf(filterHotelRows(i))
                                            Exit While
                                        End If
                                        i = i + 1
                                    End While
                                End If
                                If index = lastIndex Then
                                    Dim filterSplEvt As New DataTable
                                    If (filterRows.Count > 0) Then filterSplEvt = filterRows.CopyToDataTable()
                                    If filterSplEvt.Rows.Count > 0 Then
                                        Dim tblSplEvent As New PdfPTable(4)
                                        Dim currCode As String = Convert.ToString(headerDr("currCode"))
                                        CumulativeSpecialEvents(filterSplEvt, documentWidth, tblSplEvent)
                                        cell = New PdfPCell(tblSplEvent)
                                        cell.Colspan = 3
                                        cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                                        tblServ.AddCell(cell)
                                    End If
                                End If
                            End If
                        Next
                        tblServ.Complete = True
                        tblServ.SpacingBefore = 7
                        document.Add(tblServ)
                    End If

                    If othServDt.Rows.Count > 0 Or airportDt.Rows.Count > 0 Or visaDt.Rows.Count > 0 Or tourDt.Rows.Count > 0 Or OtherDt.Rows.Count > 0 Then
                        Dim OthServ() As String = {"Other Services", "Units/ Pax", "Date of Service"}
                        Dim tblOthServ As PdfPTable = New PdfPTable(3)
                        tblOthServ.TotalWidth = documentWidth
                        tblOthServ.LockedWidth = True
                        tblOthServ.SetWidths(New Single() {0.74F, 0.13F, 0.13F})
                        tblOthServ.SplitRows = False
                        tblOthServ.Complete = False
                        tblOthServ.HeaderRows = 1
                        For i = 0 To 2
                            phrase = New Phrase()
                            phrase.Add(New Chunk(OthServ(i), NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = titleColor
                            tblOthServ.AddCell(cell)
                        Next

                        Dim MergeDt As DataTable = New DataTable()
                        Dim OthServType As DataColumn = New DataColumn("OthServType", GetType(String))
                        Dim ServiceName As DataColumn = New DataColumn("ServiceName", GetType(String))
                        Dim Unit As DataColumn = New DataColumn("Unit", GetType(String))
                        Dim ServiceDate As DataColumn = New DataColumn("ServiceDate", GetType(String))
                        Dim Adults As DataColumn = New DataColumn("Adults", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim Child As DataColumn = New DataColumn("Child", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim Senior As DataColumn = New DataColumn("Senior", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim PickUpDropOff As DataColumn = New DataColumn("PickUpDropOff", GetType(String)) With {.DefaultValue = DBNull.Value}
                        Dim Sic As DataColumn = New DataColumn("Sic", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        MergeDt.Columns.Add(OthServType)
                        MergeDt.Columns.Add(ServiceName)
                        MergeDt.Columns.Add(Unit)
                        MergeDt.Columns.Add(ServiceDate)
                        MergeDt.Columns.Add(Adults)
                        MergeDt.Columns.Add(Child)
                        MergeDt.Columns.Add(Senior)
                        MergeDt.Columns.Add(PickUpDropOff)
                        MergeDt.Columns.Add(Sic)
                        For Each othServDr As DataRow In othServDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Transfer"
                            MergeDr("ServiceName") = othServDr("transferName")
                            MergeDr("Unit") = othServDr("units")
                            MergeDr("ServiceDate") = othServDr("transferDate")
                            MergeDr("Adults") = othServDr("Adults")
                            MergeDr("Child") = othServDr("Child")
                            MergeDr("PickUpDropOff") = othServDr("PickUpDropOff")
                            MergeDr("Sic") = othServDr("Sic")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each VisaDr As DataRow In visaDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Visa"
                            MergeDr("ServiceName") = VisaDr("visaName")
                            MergeDr("Unit") = VisaDr("noOfvisas")
                            MergeDr("ServiceDate") = VisaDr("VisaDate")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each airportDr As DataRow In airportDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Airport"
                            MergeDr("ServiceName") = airportDr("airportmaname")
                            MergeDr("Unit") = airportDr("units")
                            MergeDr("ServiceDate") = airportDr("airportmadate")
                            MergeDr("Adults") = airportDr("Adults")
                            MergeDr("Child") = airportDr("Child")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each tourDr As DataRow In tourDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Tour"
                            MergeDr("ServiceName") = tourDr("tourname")
                            MergeDr("Unit") = tourDr("units")
                            MergeDr("ServiceDate") = tourDr("tourdate")
                            MergeDr("Adults") = tourDr("Adults")
                            MergeDr("Child") = tourDr("Child")
                            MergeDr("Senior") = tourDr("Senior")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each otherDr As DataRow In OtherDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Other"
                            MergeDr("ServiceName") = otherDr("othername")
                            MergeDr("Unit") = otherDr("units")
                            MergeDr("ServiceDate") = otherDr("othdate")
                            MergeDr("Adults") = otherDr("Adults")
                            MergeDr("Child") = otherDr("Child")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        If MergeDt.Rows.Count > 0 Then
                            Dim MergeOrderDt As DataTable = (From n In MergeDt.AsEnumerable() Select n Order By Convert.ToDateTime(n.Field(Of String)("ServiceDate")) Ascending).CopyToDataTable()
                            CumulativeOtherService(tblOthServ, MergeOrderDt)
                        End If
                        tblOthServ.Complete = True
                        tblOthServ.SpacingBefore = 7
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 72 Then document.NewPage()
                        document.Add(tblOthServ)
                    End If

                    'Dim tblTotal As PdfPTable = New PdfPTable(2)
                    'tblTotal.TotalWidth = documentWidth
                    'tblTotal.LockedWidth = True
                    'tblTotal.SetWidths(New Single() {0.8F, 0.2F})
                    'tblTotal.KeepTogether = True

                    'phrase = New Phrase()
                    'phrase.Add(New Chunk("Total Amount (" + Convert.ToString(headerDr("currcode")) + ")", NormalFontBold))
                    'cell = New PdfPCell(phrase)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    'cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER
                    'cell.PaddingTop = 3.0F
                    'cell.PaddingRight = 10.0F
                    'cell.PaddingBottom = 3.0F
                    'tblTotal.AddCell(cell)

                    'phrase = New Phrase()
                    'phrase.Add(New Chunk(Convert.ToString(headerDr("saleCurrency")), NormalFontBold))
                    'cell = New PdfPCell(phrase)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    'cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER
                    'cell.PaddingTop = 3.0F
                    'cell.PaddingRight = 4.0F
                    'cell.PaddingBottom = 3.0F
                    'tblTotal.AddCell(cell)

                    'If Convert.ToDecimal(headerDr("DiscountMarkup")) > 0 Then
                    '    phrase = New Phrase()
                    '    phrase.Add(New Chunk("Discount (" + Convert.ToString(headerDr("currcode")) + ")", NormalFontBold))
                    '    cell = New PdfPCell(phrase)
                    '    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    '    cell.Border = Rectangle.LEFT_BORDER
                    '    cell.PaddingTop = 3.0F
                    '    cell.PaddingRight = 10.0F
                    '    cell.PaddingBottom = 3.0F
                    '    tblTotal.AddCell(cell)

                    '    phrase = New Phrase()
                    '    phrase.Add(New Chunk(Convert.ToString(headerDr("DiscountMarkup")), NormalFontBold))
                    '    cell = New PdfPCell(phrase)
                    '    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    '    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    '    cell.Border = Rectangle.RIGHT_BORDER
                    '    cell.PaddingTop = 3.0F
                    '    cell.PaddingRight = 4.0F
                    '    cell.PaddingBottom = 3.0F
                    '    tblTotal.AddCell(cell)
                    'End If

                    'phrase = New Phrase()
                    'phrase.Add(New Chunk("Net Amount (" + Convert.ToString(headerDr("currcode")) + ")", NormalFontBold))
                    'cell = New PdfPCell(phrase)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    'cell.Border = Rectangle.LEFT_BORDER
                    'cell.PaddingTop = 3.0F
                    'cell.PaddingRight = 10.0F
                    'cell.PaddingBottom = 3.0F
                    'tblTotal.AddCell(cell)

                    'phrase = New Phrase()
                    'phrase.Add(New Chunk(Convert.ToString(headerDr("netSaleCurrency")), NormalFontBold))
                    'cell = New PdfPCell(phrase)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    'cell.Border = Rectangle.RIGHT_BORDER
                    'cell.PaddingTop = 3.0F
                    'cell.PaddingRight = 4.0F
                    'cell.PaddingBottom = 3.0F
                    'tblTotal.AddCell(cell)

                    'phrase = New Phrase()
                    'phrase.Add(New Chunk("Total Amount (" + Convert.ToString(headerDr("baseCurrcode")) + ")", NormalFontBold))
                    'cell = New PdfPCell(phrase)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    'cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                    'cell.PaddingTop = 3.0F
                    'cell.PaddingRight = 10.0F
                    'cell.PaddingBottom = 3.0F
                    'tblTotal.AddCell(cell)

                    'phrase = New Phrase()
                    'phrase.Add(New Chunk(Convert.ToString(headerDr("SaleValue")), NormalFontBold))
                    'cell = New PdfPCell(phrase)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    'cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                    'cell.PaddingTop = 3.0F
                    'cell.PaddingRight = 4.0F
                    'cell.PaddingBottom = 3.0F
                    'tblTotal.AddCell(cell)

                    'tblTotal.SpacingBefore = 7
                    'document.Add(tblTotal)

                    ''------- Tax Note ----------------
                    'Dim tblTax As PdfPTable = New PdfPTable(2)
                    'tblTax.TotalWidth = documentWidth
                    'tblTax.LockedWidth = True
                    'tblTax.SetWidths(New Single() {0.03, 0.97F})
                    'tblTax.KeepTogether = True
                    'tblTax.Complete = False
                    'tblTax.SplitRows = False

                    'phrase = New Phrase()
                    'phrase.Add(New Chunk(Chr(149), NormalFontBoldTax))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    'cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER
                    'cell.PaddingLeft = 7.0F
                    'cell.PaddingBottom = 3.0F
                    'cell.PaddingTop = 3.0F
                    'cell.BackgroundColor = BaseColor.YELLOW
                    'tblTax.AddCell(cell)

                    'phrase = New Phrase()
                    'phrase.Add(New Chunk("ABOVE RATES ARE INCLUSIVE OF ALL TAXES INCLUDING VAT", NormalFontBoldTax))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER
                    'cell.PaddingLeft = 2.0F
                    'cell.PaddingBottom = 3.0F
                    'cell.PaddingTop = 3.0F
                    'cell.BackgroundColor = BaseColor.YELLOW
                    'tblTax.AddCell(cell)

                    'phrase = New Phrase()
                    'phrase.Add(New Chunk(Chr(149), NormalFontBoldTax))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    'cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                    'cell.PaddingLeft = 7.0F
                    'cell.PaddingBottom = 5.0F
                    'cell.BackgroundColor = BaseColor.YELLOW
                    'tblTax.AddCell(cell)

                    'phrase = New Phrase()
                    'phrase.Add(New Chunk("ABOVE RATES DOES NOT INCLUDE TOURISM DIRHAM FEE WHICH IS TO BE PAID BY THE GUEST DIRECTLY AT THE HOTEL", NormalFontBoldTax))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    'cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                    'cell.PaddingLeft = 2.0F
                    'cell.PaddingBottom = 5.0F
                    'cell.BackgroundColor = BaseColor.YELLOW
                    'tblTax.AddCell(cell)

                    'tblTax.Complete = True
                    'tblTax.SpacingBefore = 7
                    'document.Add(tblTax)

                    If GuestAndFlight.Rows.Count > 0 Then
                        Dim tblGuest As PdfPTable = New PdfPTable(7)
                        tblGuest.TotalWidth = documentWidth
                        tblGuest.LockedWidth = True
                        tblGuest.Complete = False
                        tblGuest.SplitRows = False
                        tblGuest.HeaderRows = 1
                        GuestFlightList(tblGuest, GuestAndFlight)
                        tblGuest.Complete = True
                        tblGuest.SpacingBefore = 7
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 48 Then document.NewPage()
                        document.Add(tblGuest)
                    End If

                    Dim tblFooter As New PdfPTable(1)
                    tblFooter.TotalWidth = documentWidth
                    tblFooter.LockedWidth = True
                    tblFooter.Complete = False
                    tblFooter.SetWidths({1.0F})
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Thanks and Best Regards,", NormalFont))
                    cell = New PdfPCell(phrase)
                    cell.BorderWidth = 0.7F
                    cell.Border = Rectangle.NO_BORDER
                    cell.PaddingTop = 20.0F
                    'cell.PaddingLeft = 15.0F
                    tblFooter.AddCell(cell)

                    If contactDt.Rows.Count > 0 Then
                        Dim contractDr As DataRow = contactDt.Rows(0)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(contractDr("salesPerson")) + "<" + Convert.ToString(contractDr("salesemail")) + ">" + vbCrLf + vbCrLf + "DESTINATION SPECIALIST", NormalFont))
                        cell = New PdfPCell(phrase)
                        cell.BorderWidth = 0.7F
                        cell.BorderColor = BaseColor.WHITE
                        cell.SetLeading(15, 0)
                        cell.PaddingTop = 5.0F
                        tblFooter.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk("Mobile No - " + Convert.ToString(contractDr("salesmobile")), NormalFont))
                        cell = New PdfPCell(phrase)
                        cell.BorderWidth = 0.7F
                        cell.BorderColor = BaseColor.WHITE
                        cell.SetLeading(15, 0)
                        cell.PaddingTop = 5.0F
                        tblFooter.AddCell(cell)
                    End If
                    phrase = New Phrase()
                    '  phrase.Add(New Chunk("Any Discrepancy on the above Invoice to be revert back within 72 hours from the date of Confirmation or else treated as final", NormalFont))
                    phrase.Add(New Chunk("Kindly note Early Check-In/Late Check-Out is subject to availability and at the discretion of the hotel. This is a computer generated voucher hence does not require any signature. ", NormalFont))
                    cell = New PdfPCell(phrase)
                    cell.BorderWidth = 0.7F
                    cell.BorderColor = BaseColor.WHITE
                    cell.SetLeading(15, 0)
                    cell.PaddingTop = 3.0F
                    tblFooter.AddCell(cell)
                    tblFooter.Complete = True
                    document.Add(tblFooter)


                    If EmContactDt.Rows.Count > 0 Then
                        Dim tblContact As PdfPTable = New PdfPTable(3)
                        tblContact.TotalWidth = documentWidth
                        tblContact.LockedWidth = True
                        tblContact.SetWidths(New Single() {0.4F, 0.1, 0.5F})
                        tblContact.Complete = False
                        tblContact.SplitRows = False
                        Dim TitleFontBoldUnderLine As Font = FontFactory.GetFont("Arial", 12, Font.BOLD Or Font.UNDERLINE, BaseColor.BLACK)
                        phrase = New Phrase()
                        phrase.Add(New Chunk("EMERGENCY CONTACT" & vbCrLf, TitleFontBoldUnderLine))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.SetLeading(15, 0)
                        cell.PaddingLeft = 10.0F
                        cell.PaddingBottom = 5.0F
                        cell.Colspan = 3
                        tblContact.AddCell(cell)

                        For Each ContactDr As DataRow In EmContactDt.Rows
                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(ContactDr("ContactPerson")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                            cell.SetLeading(15, 0)
                            cell.PaddingLeft = 10.0F
                            cell.PaddingBottom = 5.0F
                            tblContact.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(":", NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 5.0F
                            tblContact.AddCell(cell)

                            phrase = New Phrase()
                            phrase.Add(New Chunk(Convert.ToString(ContactDr("ContactNo")), NormalFont))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 5.0F
                            tblContact.AddCell(cell)
                        Next
                        tblContact.Complete = True
                        tblContact.SpacingBefore = 15
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 72 Then document.NewPage()
                        document.Add(tblContact)
                    End If
                    'If BankDt.Rows.Count > 0 Then
                    '    Dim tblBank As PdfPTable = New PdfPTable(2)
                    '    tblBank.TotalWidth = documentWidth
                    '    tblBank.LockedWidth = True
                    '    tblBank.Complete = False
                    '    tblBank.SplitRows = False
                    '    BankDetails(tblBank, BankDt)
                    '    tblBank.Complete = True
                    '    tblBank.SpacingBefore = 7
                    '    tblBank.KeepTogether = True
                    '    document.Add(tblBank)
                    'End If
                    document.AddTitle(Convert.ToString(headerDr("printHeader")) & "-" & requestID)
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
            Else
                Throw New Exception("There is no rows in header table")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Protected Sub CumulativeSpecialEvents(ByVal splEventDt As DataTable, ByVal documentWidth As Single, ByRef tblSplEvent As PdfPTable)"
    Protected Sub CumulativeSpecialEvents(ByVal splEventDt As DataTable, ByVal documentWidth As Single, ByRef tblSplEvent As PdfPTable)
        Dim phrase As New Phrase
        Dim cell As New PdfPCell
        Dim splEventTitleColor As BaseColor = New BaseColor(203, 235, 249)   '-255, 219, 212
        Dim arrSplEvent() As String = {"Special Events", "Units/ Pax", "Type of Units/Pax", "Date of Event"}
        tblSplEvent.TotalWidth = documentWidth
        tblSplEvent.LockedWidth = True
        tblSplEvent.SetWidths(New Single() {0.61F, 0.13F, 0.13F, 0.13F})
        tblSplEvent.Complete = False
        tblSplEvent.HeaderRows = 1
        tblSplEvent.SplitRows = False
        For i = 0 To 3
            phrase = New Phrase()
            phrase.Add(New Chunk(arrSplEvent(i), NormalFontBold))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 4.0F
            cell.PaddingTop = 1.0F
            cell.BackgroundColor = splEventTitleColor
            tblSplEvent.AddCell(cell)
        Next
        For Each splEventDr As DataRow In splEventDt.Rows
            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(splEventDr("splEventName")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.SetLeading(12, 0)
            cell.PaddingLeft = 3.0F
            cell.PaddingBottom = 4.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(splEventDr("noOfPax")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(splEventDr("paxType")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(splEventDr("splEventDate")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            tblSplEvent.AddCell(cell)
        Next
        tblSplEvent.Complete = True
    End Sub
#End Region

#Region "Protected Sub CumulativeOtherService(ByRef tblOthServ As PdfPTable, ByVal inputDt As DataTable)"
    Protected Sub CumulativeOtherService(ByRef tblOthServ As PdfPTable, ByVal inputDt As DataTable)
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        For Each inputDr As DataRow In inputDt.Rows
            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(inputDr("ServiceName")), NormalFont))
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True, "Yes")
            Else
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
            End If
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.SetLeading(12, 0)
            cell.PaddingBottom = 3.0F
            tblOthServ.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(inputDr("Unit")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell.Rowspan = 2
            End If
            tblOthServ.AddCell(cell)

            phrase = New Phrase()
            phrase.Add(New Chunk(Convert.ToString(inputDr("ServiceDate")), NormalFont))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell.Rowspan = 2
            End If
            tblOthServ.AddCell(cell)

            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                phrase = New Phrase()
                phrase.Add(New Chunk(Convert.ToString(inputDr("pickupdropoff")), NormalFont))
                If Convert.ToInt32(inputDr("sic")) <> 1 Then
                    phrase.Add(New Chunk(" (" & inputDr("adults").ToString() & " Adults", NormalFont))
                    If String.IsNullOrEmpty(Convert.ToString(inputDr("child")).Trim()) Or Convert.ToString(inputDr("child")).Trim() = "0" Then
                        phrase.Add(New Chunk(")", NormalFont))
                    Else
                        phrase.Add(New Chunk(", " & inputDr("child").ToString() & " Child)", NormalFont))
                    End If
                End If
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True, "No")
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                cell.SetLeading(12, 0)
                cell.PaddingBottom = 3.0F
                tblOthServ.AddCell(cell)
            End If
        Next
    End Sub
#End Region

#Region "Public Sub GenerateReportProformaVat(ByVal requestID As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "")"
    Public Sub GenerateReportProformaVat(ByVal requestID As String, ByVal ManualInvNo As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "")
        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            Dim decimalplaces As Integer = objclsUtilities.ExecuteQueryReturnStringValuenew("strDBConnection", "(select option_selected from reservation_parameters where param_id=509)")

            mySqlCmd = New SqlCommand("sp_booking_confirmation_print_vat", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@RequestID", SqlDbType.VarChar, 20)).Value = requestID
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            ds1 = ds
            Dim headerDt As DataTable = ds.Tables(0)
            Dim hotelDt As DataTable = ds.Tables(1)
            Dim tariffDt As DataTable = ds.Tables(2)
            Dim othServDt As DataTable = ds.Tables(3)
            Dim airportDt As DataTable = ds.Tables(4)
            Dim visaDt As DataTable = ds.Tables(5)
            Dim tourDt As DataTable = ds.Tables(6)
            Dim OtherDt As DataTable = ds.Tables(7)
            Dim guestDt As DataTable = ds.Tables(8)
            Dim contactDt As DataTable = ds.Tables(9)
            Dim BankDt As DataTable = ds.Tables(10)
            Dim SplEventDt As DataTable = ds.Tables(11)
            clsDBConnect.dbConnectionClose(sqlConn)
            If headerDt.Rows.Count > 0 Then
                Dim document As New Document(PageSize.A4, 0.0F, 0.0F, 20.0F, 35.0F)
                Dim documentWidth As Single = 550.0F
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

                    Dim headerDr As DataRow = headerDt.Rows(0)
                    table = New PdfPTable(2)
                    table.TotalWidth = documentWidth
                    table.LockedWidth = True
                    table.SetWidths(New Single() {0.5F, 0.5F})
                    table.Complete = False
                    table.SplitRows = False
                    Dim tblLogo As PdfPTable = New PdfPTable(2)
                    tblLogo.SetWidths(New Single() {0.27F, 0.73F})
                    'Company Name 
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("division_master_des")), TitleFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    tblLogo.AddCell(cell)
                    'Company Logo
                    'If objResParam.LoginType = "Agent" And objResParam.WhiteLabel = "1" Then
                    '    Dim logoName As String = objclsUtilities.ExecuteQueryReturnStringValuenew("strDBConnection" "select logofilename from agentmast_whitelabel where agentcode ='" + objResParam.AgentCode.Trim() + "'")
                    '    cell = ImageCell("~/Logos/" + logoName, 60.0F, PdfPCell.ALIGN_CENTER)
                    'Else
                    If (headerDr("div_code") = "01") Then
                        cell = ImageCell("~/Images/Logo.png", 60.0F, PdfPCell.ALIGN_CENTER)
                    ElseIf (headerDr("div_code") = "02") Then
                        cell = ImageCell("~/Images/logo.jpg", 60.0F, PdfPCell.ALIGN_CENTER)
                    Else
                        cell = ImageCell("~/Images/Logo.png", 60.0F, PdfPCell.ALIGN_CENTER)
                    End If
                    'End If
                    tblLogo.AddCell(cell)
                    'Company Address
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("address1")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Fax : " & Convert.ToString(headerDr("fax")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Tel : " & Convert.ToString(headerDr("tel")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("E-mail : " & Convert.ToString(headerDr("email")) & vbLf, NormalFont))
                    phrase.Add(New Chunk("Website : " & Convert.ToString(headerDr("website")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    tblLogo.AddCell(cell)
                    table.AddCell(tblLogo)

                    Dim tblClient As PdfPTable = New PdfPTable(2)
                    tblClient.SetWidths(New Single() {0.5F, 0.5F})
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentName")), TitleFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                    cell.SetLeading(11, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(Convert.ToString(headerDr("agentAddress")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Tel : " & Convert.ToString(headerDr("agentTel")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Fax : " & Convert.ToString(headerDr("agentfax")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Email : " & Convert.ToString(headerDr("agentEmail")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("TRNNo : " & Convert.ToString(headerDr("agentTrnno")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Attn. : " & Convert.ToString(headerDr("agentContact")), NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 2, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.SetLeading(12, 0)
                    tblClient.AddCell(cell)
                    table.AddCell(tblClient)
                    table.Complete = True
                    document.Add(table)
                    Dim tblTitle As PdfPTable = New PdfPTable(1)
                    tblTitle.SetWidths(New Single() {1.0F})
                    tblTitle.TotalWidth = documentWidth
                    tblTitle.LockedWidth = True
                    If ManualInvNo <> "" Then
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString("TAX INVOICE"), TitleFontBigBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.BackgroundColor = titleColor
                        tblTitle.AddCell(cell)
                        tblTitle.SpacingBefore = 7
                        document.Add(tblTitle)
                    Else
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(headerDr("printHeader")), TitleFontBigBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        cell.PaddingBottom = 3.0F
                        cell.BackgroundColor = titleColor
                        tblTitle.AddCell(cell)
                        tblTitle.SpacingBefore = 7
                        document.Add(tblTitle)
                    End If


                    Dim tblInvmain As PdfPTable = New PdfPTable(1)
                    tblInvmain.SetWidths(New Single() {0.1F})
                    tblInvmain.TotalWidth = documentWidth
                    tblInvmain.LockedWidth = True
                    tblInvmain.SplitRows = False

                    Dim tblInv As PdfPTable = New PdfPTable(6)
                    tblInv.SetWidths(New Single() {0.15F, 0.14F, 0.12F, 0.14F, 0.12F, 0.22F})
                    tblInv.TotalWidth = documentWidth
                    tblInv.LockedWidth = True
                    tblInv.SplitRows = False
                    Dim arrTitle() As String
                    If ManualInvNo <> "" Then
                        'arrTitle = {"Invoice No : ", ManualInvNo, "Dated : ", headerDr("requestDate"), "Your Ref : ", headerDr("agentRef"), "Request No : ", headerDr("requestID").ToString()}
                        arrTitle = {"Invoice No : ", ManualInvNo, "Dated : ", headerDr("requestDate"), "Your Ref : ", headerDr("agentRef")}
                    Else
                        arrTitle = {"Invoice No : ", "0", "Dated : ", headerDr("requestDate"), "Your Ref : ", headerDr("agentRef")}
                    End If
                    For i = 0 To 5
                        phrase = New Phrase()
                        'phrase.Add(New Chunk(arrTitle(i), NormalFontBold))
                        phrase.Add(New Chunk(arrTitle(i), NormalFontBoldVAT))
                        cell = New PdfPCell(phrase)
                        If i = 0 Then
                            cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER
                        ElseIf i = 5 Then
                            cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER
                        Else
                            cell.Border = Rectangle.TOP_BORDER
                        End If
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        If i Mod 2 = 0 Then
                            cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                        Else
                            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                        End If
                        cell.Padding = 3
                        tblInv.AddCell(cell)
                    Next

                    tblInvmain.AddCell(tblInv)


                    ' ''If ManualInvNo <> "" Then
                    Dim tblManualInv As PdfPTable = New PdfPTable(2)
                    tblManualInv.SetWidths(New Single() {0.15F, 0.75F})
                    tblManualInv.TotalWidth = documentWidth
                    tblManualInv.LockedWidth = True
                    tblManualInv.SplitRows = False
                    Dim arrTitleManu() As String
                    arrTitleManu = {"Request No : ", headerDr("requestID").ToString()}

                    For i = 0 To 1
                        phrase = New Phrase()
                        'phrase.Add(New Chunk(arrTitleManu(i), NormalFontBold))
                        phrase.Add(New Chunk(arrTitleManu(i), NormalFontBoldVAT))
                        cell = New PdfPCell(phrase)
                        If i = 0 Then
                            cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER
                        Else
                            cell.Border = Rectangle.TOP_BORDER Or Rectangle.RIGHT_BORDER
                        End If
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                        If i Mod 2 = 0 Then
                            cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                        Else
                            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                        End If
                        cell.Padding = 3
                        tblManualInv.AddCell(cell)
                    Next
                    tblInvmain.AddCell(tblManualInv)
                    document.Add(tblInvmain)
                    'document.Add(tblManualInv)
                    'End If

                    writer.PageEvent = New clsBookingConfirmPageEvents(tblInvmain, printMode)

                    If hotelDt.Rows.Count > 0 Then
                        Dim arrServ() As String = {"Hotel Services", "Charges " & Convert.ToString(headerDr("currCode")), "Taxable Amount " & Convert.ToString(headerDr("currCode")), "Non Taxable Amount " & Convert.ToString(headerDr("currCode")), "VAT Amount " & Convert.ToString(headerDr("currCode")), "Total Amount " & Convert.ToString(headerDr("currCode"))}
                        Dim tblServ As PdfPTable = New PdfPTable(6)
                        tblServ.TotalWidth = documentWidth
                        tblServ.LockedWidth = True
                        tblServ.SetWidths(New Single() {0.55F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F})
                        tblServ.Complete = False
                        tblServ.HeaderRows = 1
                        tblServ.SplitRows = False
                        For i = 0 To 5
                            phrase = New Phrase()
                            'phrase.Add(New Chunk(arrServ(i), NormalFontBold))
                            phrase.Add(New Chunk(arrServ(i), NormalFontBoldVAT))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = titleColor
                            tblServ.AddCell(cell)
                        Next
                        For Each hotelDr As DataRow In hotelDt.Rows
                            Dim tblTariff As PdfPTable = New PdfPTable(2)
                            tblTariff.SetWidths(New Single() {0.05F, 0.95F})
                            phrase = New Phrase()
                            'phrase.Add(New Chunk(Convert.ToString(hotelDr("partyName")) & vbLf, NormalFontBold))
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("partyName")) & vbLf, NormalFontBoldVAT))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            cell.Colspan = 2
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            'phrase.Add(New Chunk("", NormalFont))
                            phrase.Add(New Chunk("", NormalFontVAT))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                            cell.PaddingBottom = 3.0F
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            'phrase.Add(New Chunk(Convert.ToString(hotelDr("noofrooms")) & "  " & Convert.ToString(hotelDr("RoomDetail")) & vbLf, NormalFont))
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("noofrooms")) & "  " & Convert.ToString(hotelDr("RoomDetail")) & vbLf, NormalFontVAT))
                            If Convert.ToString(hotelDr("occupancy")) <> "" Then
                                'phrase.Add(New Chunk("[ " & Convert.ToString(hotelDr("occupancy")) & " ]", NormalFont))
                                phrase.Add(New Chunk("[ " & Convert.ToString(hotelDr("occupancy")) & " ]", NormalFontVAT))
                            End If

                            Dim rLineNo As Integer = Convert.ToInt32(hotelDr("rLineNo"))
                            Dim roomNo As Integer = Convert.ToInt32(hotelDr("roomNo"))
                            Dim tariffFilter = (From n In tariffDt.AsEnumerable() Where n.Field(Of Int32)("rLineNo") = rLineNo And n.Field(Of Int32)("roomNo") = roomNo Select n Order By Convert.ToDateTime(n.Field(Of String)("fromDate")) Ascending).ToList()
                            Dim filterTariffDt As New DataTable
                            If (tariffFilter.Count > 0) Then filterTariffDt = tariffFilter.CopyToDataTable()
                            If filterTariffDt.Rows.Count > 0 Then
                                For Each ratesDr As DataRow In filterTariffDt.Rows
                                    'phrase.Add(New Chunk(vbLf + "From " + Convert.ToString(ratesDr("fromDate")) & " " & Convert.ToString(ratesDr("nights")) & " Nts * " & Convert.ToString(ratesDr("salePrice")) & " * " & Convert.ToString(hotelDr("noofrooms")) & " Units = ", NormalFont))
                                    phrase.Add(New Chunk(vbLf + "From " + Convert.ToString(ratesDr("fromDate")) & " " & Convert.ToString(ratesDr("nights")) & " Nts * " & Convert.ToString(ratesDr("salePrice")) & " * " & Convert.ToString(hotelDr("noofrooms")) & " Units = ", NormalFontVAT))
                                    'phrase.Add(New Chunk(Convert.ToString(ratesDr("saleValue")) & " " & Convert.ToString(headerDr("currCode")), NormalFontBold))
                                    phrase.Add(New Chunk(Convert.ToString(ratesDr("saleValue")) & " " & Convert.ToString(headerDr("currCode")), NormalFontBoldVAT))
                                    If ratesDr("bookingCode") <> "" Then
                                        'phrase.Add(New Chunk(vbCrLf & "( " + Convert.ToString(ratesDr("bookingCode")) + " )", NormalFontBold))
                                        phrase.Add(New Chunk(vbCrLf & "( " + Convert.ToString(ratesDr("bookingCode")) + " )", NormalFontBoldVAT))
                                    End If
                                Next
                            End If
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            tblTariff.AddCell(cell)
                            phrase = New Phrase()
                            If Convert.ToString(hotelDr("hotelConfNo")) <> "" Then
                                'phrase.Add(New Chunk("Hotel Conf No : " & Convert.ToString(hotelDr("hotelConfNo")), NormalFontBold))
                                phrase.Add(New Chunk("Hotel Conf No : " & Convert.ToString(hotelDr("hotelConfNo")), NormalFontBoldVAT))
                            End If
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            cell.Colspan = 2
                            tblTariff.AddCell(cell)

                            phrase = New Phrase()
                            'If Convert.ToString(hotelDr("hotelConfNo")) <> "" Then
                            'phrase.Add(New Chunk("Check in : " & Convert.ToString(hotelDr("checkIn")) & "  Check Out : " & Convert.ToString(hotelDr("checkOut")), NormalFontBold))
                            phrase.Add(New Chunk("Check in : " & Convert.ToString(hotelDr("checkIn")) & "  Check Out : " & Convert.ToString(hotelDr("checkOut")), NormalFontBoldVAT))
                            'End If
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 3.0F
                            cell.Colspan = 2
                            tblTariff.AddCell(cell)


                            tblServ.AddCell(tblTariff)


                            phrase = New Phrase()
                            'phrase.Add(New Chunk(Convert.ToString(hotelDr("salevalue")), NormalFont))
                            phrase.Add(New Chunk(Convert.ToString(hotelDr("salevalue")), NormalFontVAT))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 2.0F
                            cell.PaddingRight = 4.0F
                            tblServ.AddCell(cell)

                            phrase = New Phrase()
                            'phrase.Add(New Chunk(Convert.ToString(Math.Round(hotelDr("prices_saletaxablevalue"), decimalplaces, MidpointRounding.AwayFromZero)), NormalFont))
                            phrase.Add(New Chunk(Convert.ToString(Math.Round(hotelDr("prices_saletaxablevalue"), decimalplaces, MidpointRounding.AwayFromZero)), NormalFontVAT))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 2.0F
                            cell.PaddingRight = 4.0F
                            tblServ.AddCell(cell)


                            phrase = New Phrase()
                            ''phrase.Add(New Chunk(Convert.ToString(Format(hotelDr("prices_salenontaxablevalue"), "0.00")), NormalFont))
                            'phrase.Add(New Chunk(Convert.ToString(Math.Round(hotelDr("prices_salenontaxablevalue"), decimalplaces)), NormalFont))
                            phrase.Add(New Chunk(Convert.ToString(Math.Round(hotelDr("prices_salenontaxablevalue"), decimalplaces)), NormalFontVAT))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 2.0F
                            cell.PaddingRight = 4.0F
                            tblServ.AddCell(cell)
                            phrase = New Phrase()
                            'phrase.Add(New Chunk(Convert.ToString(Math.Round(hotelDr("prices_salevatvalue"), decimalplaces)), NormalFont))
                            phrase.Add(New Chunk(Convert.ToString(Math.Round(hotelDr("prices_salevatvalue"), decimalplaces)), NormalFontVAT))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 2.0F
                            cell.PaddingRight = 4.0F
                            tblServ.AddCell(cell)

                            phrase = New Phrase()
                            Dim total = hotelDr("prices_saletaxablevalue") + hotelDr("prices_salenontaxablevalue") + hotelDr("prices_salevatvalue")
                            'phrase.Add(New Chunk(Convert.ToString(Math.Round(total, decimalplaces)), NormalFont))
                            phrase.Add(New Chunk(Convert.ToString(Math.Round(total, decimalplaces)), NormalFontVAT))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 2.0F
                            cell.PaddingRight = 4.0F
                            tblServ.AddCell(cell)
                            If SplEventDt.Rows.Count > 0 Then
                                Dim partyCode As String = hotelDr("partyCode").ToString()
                                Dim index As Integer = hotelDt.Rows.IndexOf(hotelDr)
                                Dim i As Integer = 0
                                Dim lastIndex As Integer = index
                                Dim filterRows = (From n In SplEventDt.AsEnumerable() Where n.Field(Of String)("PartyCode") = partyCode)
                                If filterRows.Count > 0 Then
                                    Dim filterHotelRows = (From n In hotelDt.AsEnumerable() Where n.Field(Of String)("PartyCode") = partyCode)
                                    While i < filterHotelRows.Count
                                        If hotelDt.Rows.IndexOf(filterHotelRows(i)) > index Then
                                            lastIndex = hotelDt.Rows.IndexOf(filterHotelRows(i))
                                            Exit While
                                        End If
                                        i = i + 1
                                    End While
                                End If
                                If index = lastIndex Then
                                    Dim filterSplEvt As New DataTable
                                    If (filterRows.Count > 0) Then filterSplEvt = filterRows.CopyToDataTable()
                                    If filterSplEvt.Rows.Count > 0 Then
                                        Dim tblSplEvent As New PdfPTable(6)
                                        Dim currCode As String = Convert.ToString(headerDr("currCode"))

                                        SpecialEventsVat(filterSplEvt, documentWidth, currCode, tblSplEvent, decimalplaces, hotelDt.Rows.Count)
                                        cell = New PdfPCell(tblSplEvent)
                                        cell.Colspan = 6
                                        cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                                        tblServ.AddCell(cell)
                                    End If
                                End If
                            End If
                        Next
                        tblServ.Complete = True
                        tblServ.SpacingBefore = 7
                        document.Add(tblServ)
                    End If
                    Dim totalothvat


                    If othServDt.Rows.Count > 0 Or airportDt.Rows.Count > 0 Or visaDt.Rows.Count > 0 Or tourDt.Rows.Count > 0 Or OtherDt.Rows.Count > 0 Then

                        Dim OthServ() As String
                        If hotelDt.Rows.Count > 0 Then
                            OthServ = {"Other Services", " ", " ", " ", " ", " "}
                        Else
                            OthServ = {"Other Services", "Charges " & Convert.ToString(headerDr("currCode")), "Taxable Amount" & Convert.ToString(headerDr("currCode")), "Non Taxable Amount" & Convert.ToString(headerDr("currCode")), "VAT Amount" & Convert.ToString(headerDr("currCode")), "Total Amount" & Convert.ToString(headerDr("currCode"))}

                        End If
                        Dim tblOthServ As PdfPTable = New PdfPTable(6)
                        tblOthServ.TotalWidth = documentWidth
                        tblOthServ.LockedWidth = True
                        tblOthServ.SetWidths(New Single() {0.55F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F})
                        tblOthServ.SplitRows = False
                        tblOthServ.Complete = False
                        tblOthServ.HeaderRows = 1
                        For i = 0 To 5
                            phrase = New Phrase()
                            'phrase.Add(New Chunk(OthServ(i), NormalFontBold))
                            phrase.Add(New Chunk(OthServ(i), NormalFontBoldVAT))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = titleColor
                            tblOthServ.AddCell(cell)
                        Next

                        Dim MergeDt As DataTable = New DataTable()
                        Dim OthServType As DataColumn = New DataColumn("OthServType", GetType(String))
                        Dim ServiceName As DataColumn = New DataColumn("ServiceName", GetType(String))
                        Dim ServiceDate As DataColumn = New DataColumn("ServiceDate", GetType(String))
                        Dim Unit As DataColumn = New DataColumn("Unit", GetType(String))
                        Dim UnitPrice As DataColumn = New DataColumn("UnitPrice", GetType(Decimal))
                        Dim UnitSaleValue As DataColumn = New DataColumn("UnitSaleValue", GetType(Decimal))
                        Dim UnitSaleTaxableValue As DataColumn = New DataColumn("UnitSaleTaxableValue", GetType(Decimal))
                        Dim UnitSaleNonTaxableValue As DataColumn = New DataColumn("UnitSaleNonTaxableValue", GetType(Decimal))
                        Dim UnitSaleVatValue As DataColumn = New DataColumn("UnitSaleVatValue", GetType(Decimal))
                        Dim Adults As DataColumn = New DataColumn("Adults", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim Child As DataColumn = New DataColumn("Child", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim Senior As DataColumn = New DataColumn("Senior", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        Dim PickUpDropOff As DataColumn = New DataColumn("PickUpDropOff", GetType(String)) With {.DefaultValue = DBNull.Value}
                        Dim Sic As DataColumn = New DataColumn("Sic", GetType(Integer)) With {.DefaultValue = DBNull.Value}
                        MergeDt.Columns.Add(OthServType)
                        MergeDt.Columns.Add(ServiceName)
                        MergeDt.Columns.Add(ServiceDate)
                        MergeDt.Columns.Add(Unit)
                        MergeDt.Columns.Add(UnitPrice)
                        MergeDt.Columns.Add(UnitSaleValue)
                        MergeDt.Columns.Add(UnitSaleTaxableValue)
                        MergeDt.Columns.Add(UnitSaleNonTaxableValue)
                        MergeDt.Columns.Add(UnitSaleVatValue)
                        MergeDt.Columns.Add(Adults)
                        MergeDt.Columns.Add(Child)
                        MergeDt.Columns.Add(Senior)
                        MergeDt.Columns.Add(PickUpDropOff)
                        MergeDt.Columns.Add(Sic)
                        For Each othServDr As DataRow In othServDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Transfer"
                            MergeDr("ServiceName") = othServDr("transferName")
                            MergeDr("ServiceDate") = othServDr("transferDate")
                            MergeDr("Unit") = othServDr("units")
                            MergeDr("UnitPrice") = othServDr("unitPrice")
                            MergeDr("UnitSaleValue") = othServDr("unitSaleValue")
                            MergeDr("UnitSaleTaxableValue") = othServDr("trfsaletaxablevalue")
                            MergeDr("UnitSaleNonTaxableValue") = 0
                            MergeDr("UnitSaleVatValue") = othServDr("trfsalevatvalue")
                            MergeDr("Adults") = othServDr("Adults")
                            MergeDr("Child") = othServDr("Child")
                            MergeDr("PickUpDropOff") = othServDr("PickUpDropOff")
                            MergeDr("Sic") = othServDr("Sic")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each VisaDr As DataRow In visaDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Visa"
                            MergeDr("ServiceName") = VisaDr("visaName")
                            MergeDr("ServiceDate") = VisaDr("VisaDate")
                            MergeDr("Unit") = VisaDr("noOfvisas")
                            MergeDr("UnitPrice") = VisaDr("visaPrice")
                            MergeDr("UnitSaleValue") = VisaDr("visaValue")
                            MergeDr("UnitSaleTaxableValue") = VisaDr("visataxablevalue")
                            MergeDr("UnitSaleNonTaxableValue") = VisaDr("visanontaxablevalue")
                            MergeDr("UnitSaleVatValue") = VisaDr("visavatvalue")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each airportDr As DataRow In airportDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Airport"
                            MergeDr("ServiceName") = airportDr("airportmaname")
                            MergeDr("ServiceDate") = airportDr("airportmadate")
                            MergeDr("Unit") = airportDr("units")
                            MergeDr("UnitPrice") = airportDr("unitPrice")
                            MergeDr("UnitSaleValue") = airportDr("unitSaleValue")
                            MergeDr("UnitSaleTaxableValue") = airportDr("airportmataxablevalue")
                            MergeDr("UnitSaleNonTaxableValue") = 0
                            MergeDr("UnitSaleVatValue") = airportDr("airportmavatvalue")
                            MergeDr("Adults") = airportDr("Adults")
                            MergeDr("Child") = airportDr("Child")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each tourDr As DataRow In tourDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Tour"
                            MergeDr("ServiceName") = tourDr("tourname")
                            MergeDr("ServiceDate") = tourDr("tourdate")
                            MergeDr("Unit") = tourDr("units")
                            MergeDr("UnitPrice") = tourDr("unitPrice")
                            MergeDr("UnitSaleValue") = tourDr("unitSaleValue")
                            MergeDr("UnitSaleTaxableValue") = tourDr("tourstaxablevalue")
                            MergeDr("UnitSaleNonTaxableValue") = 0
                            MergeDr("UnitSaleVatValue") = tourDr("toursvatvalue")
                            MergeDr("Adults") = tourDr("Adults")
                            MergeDr("Child") = tourDr("Child")
                            MergeDr("Senior") = tourDr("Senior")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        For Each otherDr As DataRow In OtherDt.Rows
                            Dim MergeDr As DataRow = MergeDt.NewRow
                            MergeDr("OthServType") = "Other"
                            MergeDr("ServiceName") = otherDr("othername")
                            MergeDr("ServiceDate") = otherDr("othdate")
                            MergeDr("Unit") = otherDr("units")
                            MergeDr("UnitPrice") = otherDr("unitPrice")
                            MergeDr("UnitSaleValue") = otherDr("unitSaleValue")
                            MergeDr("UnitSaleTaxableValue") = otherDr("othertaxablevalue")
                            MergeDr("UnitSaleNonTaxableValue") = 0
                            MergeDr("UnitSaleVatValue") = otherDr("othervatvalue")
                            MergeDr("Adults") = otherDr("Adults")
                            MergeDr("Child") = otherDr("Child")
                            MergeDt.Rows.Add(MergeDr)
                        Next
                        If MergeDt.Rows.Count > 0 Then
                            Dim MergeOrderDt As DataTable = (From n In MergeDt.AsEnumerable() Select n Order By Convert.ToDateTime(n.Field(Of String)("ServiceDate")) Ascending).CopyToDataTable()
                            AppendOtherServicesVat(tblOthServ, MergeOrderDt, decimalplaces)
                        End If
                        tblOthServ.Complete = True
                        tblOthServ.SpacingBefore = 3.0F
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 72 Then document.NewPage()
                        document.Add(tblOthServ)

                        totalothvat = MergeDt.Compute("sum(UnitSaleVatValue)", String.Empty)
                    End If

                    Dim tblTotal As PdfPTable = New PdfPTable(2)
                    tblTotal.TotalWidth = documentWidth
                    tblTotal.LockedWidth = True
                    tblTotal.SetWidths(New Single() {0.8F, 0.2F})
                    tblTotal.KeepTogether = True

                    phrase = New Phrase()
                    'phrase.Add(New Chunk("Total VAT Amount (" + Convert.ToString(headerDr("currcode")) + ")", NormalFontBold))
                    phrase.Add(New Chunk("Total VAT Amount (" + Convert.ToString(headerDr("currcode")) + ")", NormalFontBoldVAT))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER
                    cell.PaddingTop = 7.0F
                    cell.PaddingRight = 10.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    Dim totalvat
                    If hotelDt.Rows.Count > 0 And SplEventDt.Rows.Count > 0 Then
                        totalvat = hotelDt.Compute("sum(prices_salevatvalue)", String.Empty) + SplEventDt.Compute("sum(spleventsalevatvalue)", String.Empty) + totalothvat

                    ElseIf hotelDt.Rows.Count > 0 Then
                        totalvat = hotelDt.Compute("sum(prices_salevatvalue)", String.Empty) + totalothvat
                    Else

                        totalvat = totalothvat
                    End If


                    'phrase.Add(New Chunk(Convert.ToString(Format(totalvat, "0.00")), NormalFontBold))
                    phrase.Add(New Chunk(Convert.ToString(Format(totalvat, "0.00")), NormalFontBoldVAT))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 4.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)


                    phrase = New Phrase()
                    'phrase.Add(New Chunk("Total Amount (" + Convert.ToString(headerDr("currcode")) + ")", NormalFontBold))
                    phrase.Add(New Chunk("Total Amount (" + Convert.ToString(headerDr("currcode")) + ")", NormalFontBoldVAT))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.LEFT_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 10.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    'phrase.Add(New Chunk(Convert.ToString(headerDr("saleCurrency")), NormalFontBold))
                    phrase.Add(New Chunk(Convert.ToString(headerDr("saleCurrency")), NormalFontBoldVAT))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
                    cell.Border = Rectangle.RIGHT_BORDER
                    cell.PaddingTop = 3.0F
                    cell.PaddingRight = 4.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)





                    Dim totGrandAmount As Decimal
                    If Not IsDBNull(headerDr("saleCurrency")) Then
                        totGrandAmount = Math.Round(headerDr("saleCurrency"), decimalplaces)
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
                        totalWord = totalWord + " ONLY "
                    End If





                    phrase = New Phrase()
                    'phrase.Add(New Chunk("Total Amount (in Words)", NormalFontBold))
                    phrase.Add(New Chunk("Total Amount (in Words)", NormalFontBoldVAT))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.LEFT_BORDER
                    cell.PaddingTop = 3.0F
                    cell.Colspan = "2"
                    cell.PaddingRight = 4.0F
                    cell.PaddingLeft = 3.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    phrase = New Phrase()
                    'phrase.Add(New Chunk(" (" + Convert.ToString(headerDr("currcode")) + ") " & Convert.ToString(totalWord), NormalFont))
                    phrase.Add(New Chunk(" (" + Convert.ToString(headerDr("currcode")) + ") " & Convert.ToString(totalWord), NormalFontVAT))
                    cell = New PdfPCell(phrase)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER Or Rectangle.LEFT_BORDER
                    cell.PaddingTop = 3.0F
                    cell.Colspan = "2"
                    cell.PaddingRight = 4.0F
                    cell.PaddingBottom = 3.0F
                    tblTotal.AddCell(cell)

                    tblTotal.SpacingBefore = 7
                    document.Add(tblTotal)

                    '------- Tax Note ----------------
                    Dim tblTax As PdfPTable = New PdfPTable(2)
                    tblTax.TotalWidth = documentWidth
                    tblTax.LockedWidth = True
                    tblTax.SetWidths(New Single() {0.03, 0.97F})
                    tblTax.KeepTogether = True
                    tblTax.Complete = False
                    tblTax.SplitRows = False

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Chr(149), NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.TOP_BORDER
                    cell.PaddingLeft = 7.0F
                    cell.PaddingBottom = 3.0F
                    cell.PaddingTop = 3.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("ABOVE RATES ARE INCLUSIVE OF ALL TAXES INCLUDING VAT", NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER
                    cell.PaddingLeft = 2.0F
                    cell.PaddingBottom = 3.0F
                    cell.PaddingTop = 3.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(Chr(149), NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Border = Rectangle.LEFT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingLeft = 7.0F
                    cell.PaddingBottom = 5.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("ABOVE RATES DOES NOT INCLUDE TOURISM DIRHAM FEE WHICH IS TO BE PAID BY THE GUEST DIRECTLY AT THE HOTEL", NormalFontBoldTax))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.Border = Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                    cell.PaddingLeft = 2.0F
                    cell.PaddingBottom = 5.0F
                    cell.BackgroundColor = BaseColor.YELLOW
                    tblTax.AddCell(cell)

                    tblTax.Complete = True
                    tblTax.SpacingBefore = 7
                    document.Add(tblTax)

                    If guestDt.Rows.Count > 0 Then
                        Dim tblGuest As PdfPTable = New PdfPTable(3)
                        tblGuest.TotalWidth = documentWidth
                        tblGuest.LockedWidth = True
                        tblGuest.Complete = False
                        tblGuest.SplitRows = False
                        tblGuest.HeaderRows = 1
                        GuestList(tblGuest, guestDt)
                        tblGuest.Complete = True
                        tblGuest.SpacingBefore = 7
                        remainingPageSpace = writer.GetVerticalPosition(False) - document.BottomMargin
                        If remainingPageSpace < 48 Then document.NewPage()
                        document.Add(tblGuest)
                    End If



                    Dim tblFooter As New PdfPTable(1)
                    tblFooter.TotalWidth = documentWidth
                    tblFooter.LockedWidth = True
                    tblFooter.Complete = False
                    tblFooter.SetWidths({1.0F})
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Thanks and Best Regards", NormalFont))
                    cell = New PdfPCell(phrase)
                    cell.BorderWidth = 0.7F
                    cell.Border = Rectangle.NO_BORDER
                    cell.PaddingTop = 6.0F
                    cell.PaddingLeft = 15.0F
                    tblFooter.AddCell(cell)

                    If contactDt.Rows.Count > 0 Then
                        Dim contractDr As DataRow = contactDt.Rows(0)
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Convert.ToString(contractDr("salesPerson")) + "<" + Convert.ToString(contractDr("salesemail")) + ">" + vbCrLf + "DESTINATION SPECIALIST", NormalFont))
                        cell = New PdfPCell(phrase)
                        cell.BorderWidth = 0.7F
                        cell.BorderColor = BaseColor.WHITE
                        cell.SetLeading(15, 0)
                        cell.PaddingTop = 35.0F
                        tblFooter.AddCell(cell)

                        phrase = New Phrase()
                        phrase.Add(New Chunk("Mobile No - " + Convert.ToString(contractDr("salesmobile")), NormalFont))
                        cell = New PdfPCell(phrase)
                        cell.BorderWidth = 0.7F
                        cell.BorderColor = BaseColor.WHITE
                        cell.SetLeading(15, 0)
                        cell.PaddingTop = 14.0F
                        tblFooter.AddCell(cell)
                    End If
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Any Discrepancy on the above Invoice to be revert back within 72 hours from the date of Confirmation or else treated as final", NormalFont))
                    cell = New PdfPCell(phrase)
                    cell.BorderWidth = 0.7F
                    cell.BorderColor = BaseColor.WHITE
                    cell.SetLeading(15, 0)
                    cell.PaddingTop = 3.0F
                    tblFooter.AddCell(cell)
                    tblFooter.Complete = True
                    document.Add(tblFooter)

                    If BankDt.Rows.Count > 0 Then
                        Dim tblBank As PdfPTable = New PdfPTable(2)
                        tblBank.TotalWidth = documentWidth
                        tblBank.LockedWidth = True
                        tblBank.Complete = False
                        tblBank.SplitRows = False
                        BankDetailsvat(tblBank, BankDt, headerDt.Rows(0).Item("division_master_des"))
                        tblBank.Complete = True
                        tblBank.SpacingBefore = 7
                        tblBank.KeepTogether = True
                        document.Add(tblBank)
                    End If

                    document.AddTitle(Convert.ToString(headerDr("printHeader")) & "-" & requestID)
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
            Else
                Throw New Exception("There is no rows in header table")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
#Region "Protected Sub BankDetailsvat(ByRef tblBank As PdfPTable, ByVal BankDt As DataTable,ByVal companyname as string)"
    Protected Sub BankDetailsvat(ByRef tblBank As PdfPTable, ByVal BankDt As DataTable, ByVal companyname As String)
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        phrase = New Phrase()
        tblBank.SetWidths(New Single() {0.27F, 0.73F})
        phrase = New Phrase()
        phrase.Add(New Chunk("BENEFICIARY BANK DETAILS", NormalFontBold))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.PaddingTop = 2.0F
        cell.PaddingBottom = 5.0F
        cell.Colspan = 2
        cell.BackgroundColor = titleColor
        tblBank.AddCell(cell)

        'Dim bankDr As DataRow = BankDt.Rows(0)

        For Each bankDr As DataRow In BankDt.Rows

            Dim beneficiaryDetails() As String = {"BENEFICIARY NAME", companyname, "BENEFICIARY ADDRESS", Convert.ToString(bankDr("beneficiaryAddress")), "BANK NAME & ADDRESS", Convert.ToString(bankDr("bankName")) & ", " & Convert.ToString(bankDr(3)), _
             "ACCOUNT NUMBER", Convert.ToString(bankDr("accountNumber")), "IBAN NUMBER", Convert.ToString(bankDr("ibanNumber")), "SWIFT CODE", Convert.ToString(bankDr("swiftCode"))}
            For i = 0 To 11
                If i Mod 2 = 0 Then
                    phrase = New Phrase()
                    phrase.Add(New Chunk(beneficiaryDetails(i), NormalFontBold))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
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
                        phrase.Add(New Chunk(beneficiaryDetails(i), NormalFontBoldRed))
                    Else
                        phrase.Add(New Chunk(beneficiaryDetails(i), NormalFontBold))
                    End If
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                    cell.SetLeading(12, 0)
                    cell.PaddingTop = 1
                    cell.PaddingLeft = 3
                    cell.PaddingRight = 3
                    cell.PaddingBottom = 3
                    tblBank.AddCell(cell)
                End If
            Next

            If BankDt.Rows.Count > 1 Then
                phrase = New Phrase()
                phrase.Add(New Chunk(" ", NormalFontBold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                cell.SetLeading(12, 0)
                cell.PaddingTop = 2.0F
                cell.PaddingBottom = 5.0F
                cell.Colspan = 2
                tblBank.AddCell(cell)
            End If
        Next

        phrase = New Phrase()
        phrase.Add(New Chunk("Note : It is mandatory to mention the IBAN number for Bank Payment Transfer", NormalFontBold))
        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        cell.SetLeading(12, 0)
        cell.PaddingTop = 2.0F
        cell.PaddingBottom = 5.0F
        cell.Colspan = 2
        tblBank.AddCell(cell)
    End Sub
#End Region
#Region "Protected Sub SpecialEventsVat(ByVal splEventDt As DataTable, ByVal documentWidth As Single, ByVal CurrCode As String, ByRef tblSplEvent As PdfPTable)"
    Protected Sub SpecialEventsVat(ByVal splEventDt As DataTable, ByVal documentWidth As Single, ByVal CurrCode As String, ByRef tblSplEvent As PdfPTable, ByVal decimalplaces As Integer, ByVal hoteldtrowcount As Integer)
        Dim phrase As New Phrase
        Dim cell As New PdfPCell
        'Dim splEventTitleColor As BaseColor = New BaseColor(203, 235, 249)   '-255, 219, 212
        Dim splEventTitleColor As BaseColor = New BaseColor(214, 214, 214)
        Dim arrSplEvent() As String

        If hoteldtrowcount > 0 Then

            arrSplEvent = {"Special Events", " ", " ", " ", " ", " "}

        Else
            arrSplEvent = {"Special Events", "Charges " & CurrCode, "Taxable Amount " & CurrCode, "Non Taxable Amount " & CurrCode, "VAT Amount " & CurrCode, "Total Amount " & CurrCode}

        End If
        tblSplEvent.TotalWidth = documentWidth
        tblSplEvent.LockedWidth = True
        tblSplEvent.SetWidths(New Single() {0.55F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F})
        tblSplEvent.Complete = False
        tblSplEvent.HeaderRows = 1
        tblSplEvent.SplitRows = False
        For i = 0 To 5
            phrase = New Phrase()
            'phrase.Add(New Chunk(arrSplEvent(i), NormalFontBold))
            phrase.Add(New Chunk(arrSplEvent(i), NormalFontBoldVAT))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 4.0F
            cell.PaddingTop = 3.0F
            cell.BackgroundColor = splEventTitleColor
            tblSplEvent.AddCell(cell)
        Next
        For Each splEventDr As DataRow In splEventDt.Rows
            phrase = New Phrase()
            'phrase.Add(New Chunk(Convert.ToString(splEventDr("splEventName")) & vbLf, NormalFont))
            phrase.Add(New Chunk(Convert.ToString(splEventDr("splEventName")) & vbLf, NormalFontVAT))
            'phrase.Add(New Chunk("Event Date : " & Convert.ToString(splEventDr("splEventDate")) & "Units/Pax : " & Convert.ToString(splEventDr("noOfPax")) & vbLf, NormalFontBold))
            phrase.Add(New Chunk("Event Date : " & Convert.ToString(splEventDr("splEventDate")) & "Units/Pax : " & Convert.ToString(splEventDr("noOfPax")) & vbLf, NormalFontBoldVAT))
            'phrase.Add(New Chunk("Types of Units/Pax : " & Convert.ToString(splEventDr("paxType")) & vbLf, NormalFontBold))
            phrase.Add(New Chunk("Types of Units/Pax : " & Convert.ToString(splEventDr("paxType")) & vbLf, NormalFontBoldVAT))
            'phrase.Add(New Chunk("Rate Per Units/Pax : " & Convert.ToString(splEventDr("paxRate")) & vbLf, NormalFontBold))
            phrase.Add(New Chunk("Rate Per Units/Pax : " & Convert.ToString(splEventDr("paxRate")) & vbLf, NormalFontBoldVAT))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.SetLeading(12, 0)
            cell.PaddingLeft = 3.0F
            cell.PaddingBottom = 4.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            'phrase.Add(New Chunk(Convert.ToString(Math.Round(splEventDr("splEventValue"), decimalplaces)), NormalFont))
            phrase.Add(New Chunk(Convert.ToString(Math.Round(splEventDr("splEventValue"), decimalplaces)), NormalFontVAT))
            'cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            'phrase.Add(New Chunk(Convert.ToString(Math.Round(splEventDr("spleventsaletaxablevalue"), decimalplaces)), NormalFont))
            phrase.Add(New Chunk(Convert.ToString(Math.Round(splEventDr("spleventsaletaxablevalue"), decimalplaces)), NormalFontVAT))
            'cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            'phrase.Add(New Chunk(Convert.ToString(Math.Round(splEventDr("spleventsalenontaxablevalue"), decimalplaces)), NormalFont))
            phrase.Add(New Chunk(Convert.ToString(Math.Round(splEventDr("spleventsalenontaxablevalue"), decimalplaces)), NormalFontVAT))
            'cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            'phrase.Add(New Chunk(Convert.ToString(Math.Round(splEventDr("spleventsalevatvalue"), decimalplaces)), NormalFont))
            phrase.Add(New Chunk(Convert.ToString(Math.Round(splEventDr("spleventsalevatvalue"), decimalplaces)), NormalFontVAT))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            cell.PaddingRight = 4.0F
            tblSplEvent.AddCell(cell)

            phrase = New Phrase()
            Dim total = splEventDr("spleventsaletaxablevalue") + splEventDr("spleventsalenontaxablevalue") + splEventDr("spleventsalevatvalue")
            'phrase.Add(New Chunk(Convert.ToString(Math.Round(total, decimalplaces)), NormalFont))
            phrase.Add(New Chunk(Convert.ToString(Math.Round(total, decimalplaces)), NormalFontVAT))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 2.0F
            cell.PaddingRight = 4.0F
            tblSplEvent.AddCell(cell)
        Next
        tblSplEvent.Complete = True
    End Sub
#End Region

#Region "Protected Sub AppendOtherServicesVat(ByRef tblOthServ As PdfPTable, ByVal inputDt As DataTable)"
    Protected Sub AppendOtherServicesVat(ByRef tblOthServ As PdfPTable, ByVal inputDt As DataTable, ByVal decimalplaces As Integer)
        Dim phrase As Phrase = Nothing
        Dim cell As PdfPCell = Nothing
        For Each inputDr As DataRow In inputDt.Rows
            phrase = New Phrase()
            'phrase.Add(New Chunk(Convert.ToString(inputDr("ServiceName")) & vbLf, NormalFont))
            phrase.Add(New Chunk(Convert.ToString(inputDr("ServiceName")) & vbLf, NormalFontVAT))
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True, "Yes")
            Else
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                'phrase.Add(New Chunk(vbLf + "Service Date : " & Convert.ToString(inputDr("ServiceDate")) & "  Units/Pax :" & Convert.ToString(inputDr("Unit")), NormalFontBold))
                phrase.Add(New Chunk(vbLf + "Service Date : " & Convert.ToString(inputDr("ServiceDate")) & "  Units/Pax :" & Convert.ToString(inputDr("Unit")), NormalFontBoldVAT))
                'phrase.Add(New Chunk(vbCrLf + "Rate Per Units/Pax : " & Convert.ToString(inputDr("UnitPrice")) & vbLf, NormalFontBold))
                phrase.Add(New Chunk(vbCrLf + "Rate Per Units/Pax : " & Convert.ToString(inputDr("UnitPrice")) & vbLf, NormalFontBoldVAT))

            End If
            cell.PaddingLeft = 3.0F
            cell.PaddingTop = 3.0F
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.SetLeading(12, 0)
            cell.PaddingBottom = 3.0F
            tblOthServ.AddCell(cell)


            phrase = New Phrase()
            'phrase.Add(New Chunk(Convert.ToString(inputDr("UnitSaleValue")), NormalFont))
            phrase.Add(New Chunk(Convert.ToString(inputDr("UnitSaleValue")), NormalFontVAT))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell.Rowspan = 2
            End If
            tblOthServ.AddCell(cell)

            phrase = New Phrase()
            'phrase.Add(New Chunk(Convert.ToString(Math.Round(inputDr("UnitSaleTaxableValue"), decimalplaces)), NormalFont))
            phrase.Add(New Chunk(Convert.ToString(Math.Round(inputDr("UnitSaleTaxableValue"), decimalplaces)), NormalFontVAT))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            cell.PaddingRight = 4.0F
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell.Rowspan = 2
            End If
            tblOthServ.AddCell(cell)

            phrase = New Phrase()
            'phrase.Add(New Chunk(Convert.ToString(Math.Round(inputDr("UnitSaleNonTaxableValue"), decimalplaces)), NormalFont))
            phrase.Add(New Chunk(Convert.ToString(Math.Round(inputDr("UnitSaleNonTaxableValue"), decimalplaces)), NormalFontVAT))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            cell.PaddingRight = 4.0F
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell.Rowspan = 2
            End If
            tblOthServ.AddCell(cell)

            phrase = New Phrase()
            'phrase.Add(New Chunk(Convert.ToString(Math.Round(inputDr("UnitSaleVatValue"), decimalplaces)), NormalFont))
            phrase.Add(New Chunk(Convert.ToString(Math.Round(inputDr("UnitSaleVatValue"), decimalplaces)), NormalFontVAT))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            cell.PaddingRight = 4.0F
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell.Rowspan = 2
            End If

            tblOthServ.AddCell(cell)
            phrase = New Phrase()
            Dim total
            If Convert.ToString(inputDr("OthServType")) = "Visa" Then
                total = inputDr("UnitSaleTaxableValue") + inputDr("UnitSaleNonTaxableValue") + inputDr("UnitSaleVatValue")
            Else

                total = inputDr("UnitSaleTaxableValue") + inputDr("UnitSaleVatValue")
            End If
            'phrase.Add(New Chunk(Convert.ToString(Math.Round(total, decimalplaces)), NormalFont))
            phrase.Add(New Chunk(Convert.ToString(Math.Round(total, decimalplaces)), NormalFontVAT))
            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
            cell.PaddingBottom = 3.0F
            cell.PaddingRight = 4.0F
            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                cell.Rowspan = 2
            End If
            tblOthServ.AddCell(cell)

            If Convert.ToString(inputDr("OthServType")) = "Transfer" Then
                phrase = New Phrase()
                'phrase.Add(New Chunk(Convert.ToString(inputDr("pickupdropoff")), NormalFont))
                phrase.Add(New Chunk(Convert.ToString(inputDr("pickupdropoff")), NormalFontVAT))
                If Convert.ToInt32(inputDr("sic")) <> 1 Then
                    'phrase.Add(New Chunk(" (" & inputDr("adults").ToString() & " Adults", NormalFont))
                    phrase.Add(New Chunk(" (" & inputDr("adults").ToString() & " Adults", NormalFontVAT))
                    If String.IsNullOrEmpty(Convert.ToString(inputDr("child")).Trim()) Or Convert.ToString(inputDr("child")).Trim() = "0" Then
                        'phrase.Add(New Chunk(")", NormalFont))
                        phrase.Add(New Chunk(")", NormalFontVAT))

                    Else
                        'phrase.Add(New Chunk(", " & inputDr("child").ToString() & " Child)", NormalFont))
                        phrase.Add(New Chunk(", " & inputDr("child").ToString() & " Child)", NormalFontVAT))
                    End If
                End If
                'phrase.Add(New Chunk(vbLf + "Service Date : " & Convert.ToString(inputDr("ServiceDate")) & "  Units/Pax :" & Convert.ToString(inputDr("Unit")), NormalFontBold))
                phrase.Add(New Chunk(vbLf + "Service Date : " & Convert.ToString(inputDr("ServiceDate")) & "  Units/Pax :" & Convert.ToString(inputDr("Unit")), NormalFontBoldVAT))
                'phrase.Add(New Chunk(vbLf + "Rate Per Units/Pax  :" & Convert.ToString(inputDr("UnitPrice")) & vbLf, NormalFontBold))
                phrase.Add(New Chunk(vbLf + "Rate Per Units/Pax  :" & Convert.ToString(inputDr("UnitPrice")) & vbLf, NormalFontBoldVAT))
                cell.PaddingTop = 3.0F
                cell.PaddingLeft = 3.0F
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True, "No")
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                cell.Border = Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
                cell.SetLeading(12, 0)
                cell.PaddingBottom = 3.0F
                tblOthServ.AddCell(cell)
            End If




        Next
    End Sub
#End Region
    Protected Function ServiceRemarksParce(ByVal otherDr As DataRow) As String
        Dim strRemarks As String = ""
        Try
            If otherDr("CustomerRemark").ToString.Trim.Length > 0 Then
                If (strRemarks.Trim.Length > 0) Then
                    strRemarks = strRemarks + vbCr
                End If
                strRemarks = strRemarks + "    Customer Remark:" + otherDr("CustomerRemark")
            End If

        Catch ex As Exception

        End Try

        Return strRemarks
    End Function
End Class
