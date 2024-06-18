Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Collections.Generic
Imports System.Linq
Imports ClosedXML.Excel
Imports System.IO
Public Class ClsCustomerTrialPdf
    Inherits System.Web.UI.Page
#Region "Global Parameters"
    Dim objutils As New clsUtils
    Dim rptcompanyname, IsgroupBy, reportFilter, colheader1, colheader2, head, heading, GrandTotalv, rpttype As String
    Dim custdetailsdtgpby As New DataTable
    Dim dscurrecncy() As System.Data.DataRow
    Dim led, rptreportname, DecimalPoints, report, spName, addrLine1, addrLine2, addrLine3, addrLine4, addrLine5, currtype, cmb, decimalPoint, currcode As String
    Dim NormalFont As Font = New Font(FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK))
    Dim NormalFontBold As Font = New Font(FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK))
    Dim HeadFontBold As Font = New Font(FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK))
    Dim rowcount As Integer = 2
    Dim HeadFont As Font = New Font(FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK))
    Dim TableHeadFontBold As Font = New Font(FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK))
    Dim Footerfont As Font = FontFactory.GetFont("Arial", 7, Font.NORMAL, BaseColor.BLACK)
    Dim CompanyFont As Font = New Font(FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.WHITE))
    Dim ReportNameFont As Font = New Font(FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.WHITE))
    Dim Rowtitlebg As BaseColor = New BaseColor(192, 192, 192)
    Dim titleColor As BaseColor = New BaseColor(0, 72, 92)
    Dim grandtotalbg As BaseColor = New BaseColor(162, 162, 162)
    Dim groupbg As BaseColor = New BaseColor(255, 204, 255)

    Dim socredit, sodebit, sccredit, scdebit, stcredit, stdebit, toDebit, tcdebit, ttdebit As Decimal
    '  Dim socreditu, sodebitu, sccreditu, scdebitu, stcreditu, stdebitu As Decimal
    Dim Totalodebit, Totalocredit, Totalcdebit, Totalccredit, Totaltdebit As Decimal
    Dim Totaltcredit, Totalodebitu, Totalocreditu, Totalcdebitu, Totalccreditu, Totaltdebitu, Totaltcreditu As Double

    Dim documentWidth As Single = 770.0F
    Dim phrase As Phrase = Nothing
    Dim cell As PdfPCell = Nothing
    Dim cell1 As PdfPCell = Nothing
    Dim k As Integer = 0
    Dim l As Integer = 0
    Dim sp As Integer = 138
#End Region


    Public Sub CustTrialBal_WithMovement(ByVal reportsType As String, ByVal rptfilter As String, ByVal fromdate As String, ByVal todate As String, ByVal fromctry As String, ByVal toctry As String, ByVal movflg As String, ByVal fromcode As String, ByVal tocode As String, ByVal frommarketcode As String, ByVal tomarketcode As String, ByVal fromglcode As String, ByVal toglcode As String,
                                    ByVal orderby As String, ByVal curr As String, ByVal includezero As String, ByVal gpby As String, ByVal withcredit As String, ByVal divcode As String, ByVal trialtype As String, ByVal type As String, ByVal fromcat As String, ByVal tocat As String, ByVal custgroup_sp_type As String, ByVal reporttype As String, ByVal accttype As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fromsptype As String = "", Optional ByVal tosptype As String = "", Optional ByVal fileName As String = "")

        IsgroupBy = gpby

        rpttype = reportsType
        If divcode <> "" Then
            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
        Else
            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
        End If
        Dim decno = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), String)
        ' Dim decno As Integer
        ' Dim currency As String = String.Empty
        'If curr <> 0 Then
        '    currency = "USD"

        'Else
        '    Dim c As String = objutils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
        '    currency = c
        'End If
        'decno = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currency & "'"), Integer)

        decimalPoint = "N" & decno.ToString()

        addrLine1 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1063)
        addrLine2 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1064)
        addrLine3 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1065)
        addrLine4 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1066)
        addrLine5 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1053)

        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add(type & "TrailBal")

        rptreportname = IIf(Trim(withcredit) = 1, IIf(type = "C", reporttype & "s With Credit Balance",
                   IIf(accttype = "S", reporttype & " With Debit Balance",
                      reporttype & " Agent With Debit Balance")), IIf(type = "S",
                 IIf(accttype = "S", reporttype & " Trial Balance", reporttype & " Agent Trial Balance"),
                 reporttype & " Trial Balance"))

        spName = IIf(type = "S", "sp_supplier_trialbal", "sp_customer_trialbal")

        Try
            FontFactory.RegisterDirectories()
            Dim pdfdoc As Document = New Document(PageSize.A4, -10.0F, 10.0F, 30.0F, 41.0F)


            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand(spName, sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure


            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@movflg", SqlDbType.Int)).Value = Convert.ToInt32(movflg)
            mySqlCmd.Parameters.Add(New SqlParameter("@fromcode", SqlDbType.VarChar, 20)).Value = fromcode
            mySqlCmd.Parameters.Add(New SqlParameter("@tocode", SqlDbType.VarChar, 20)).Value = tocode

            If type = "C" Then
                mySqlCmd.Parameters.Add(New SqlParameter("@frommarkcode", SqlDbType.VarChar, 20)).Value = frommarketcode
                mySqlCmd.Parameters.Add(New SqlParameter("@tomarkcode", SqlDbType.VarChar, 20)).Value = tomarketcode
                colheader1 = "Customer Code"
                colheader2 = "Customer Name"
                pdfdoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)
            End If
            mySqlCmd.Parameters.Add(New SqlParameter("@fromctry", SqlDbType.VarChar, 20)).Value = fromctry
            mySqlCmd.Parameters.Add(New SqlParameter("@toctry", SqlDbType.VarChar, 20)).Value = toctry
            mySqlCmd.Parameters.Add(New SqlParameter("@fromcat", SqlDbType.VarChar, 20)).Value = fromcat
            mySqlCmd.Parameters.Add(New SqlParameter("@tocat", SqlDbType.VarChar, 20)).Value = tocat
            mySqlCmd.Parameters.Add(New SqlParameter("@fromglcode", SqlDbType.VarChar, 20)).Value = fromglcode
            mySqlCmd.Parameters.Add(New SqlParameter("@toglcode", SqlDbType.VarChar, 20)).Value = toglcode

            mySqlCmd.Parameters.Add(New SqlParameter("@currtype", SqlDbType.Int)).Value = Convert.ToInt32(curr)
            mySqlCmd.Parameters.Add(New SqlParameter("@orderby", SqlDbType.Int)).Value = Convert.ToInt32(orderby)
            mySqlCmd.Parameters.Add(New SqlParameter("@includezero", SqlDbType.Int)).Value = includezero

            mySqlCmd.Parameters.Add(New SqlParameter("@withcredit", SqlDbType.Int)).Value = Convert.ToInt32(withcredit)
            mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
            mySqlCmd.Parameters.Add(New SqlParameter("@custgroup_sp_type", SqlDbType.VarChar, 20)).Value = custgroup_sp_type

            If type = "S" Then
                mySqlCmd.Parameters.Add(New SqlParameter("@acctype", SqlDbType.VarChar, 1)).Value = accttype
                mySqlCmd.Parameters.Add(New SqlParameter("@fromsptype", SqlDbType.VarChar, 20)).Value = fromsptype
                mySqlCmd.Parameters.Add(New SqlParameter("@tosptype", SqlDbType.VarChar, 20)).Value = tosptype
                mySqlCmd.Parameters.Add(New SqlParameter("@fromcity", SqlDbType.VarChar, 20)).Value = frommarketcode
                mySqlCmd.Parameters.Add(New SqlParameter("@tocity", SqlDbType.VarChar, 20)).Value = tomarketcode
                colheader1 = "Supplier Code"
                colheader2 = "Supplier Name"

                If movflg = "0" Then
                    If type = "S" Then
                        documentWidth = 550.0F
                        sp = 35
                    Else
                        pdfdoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)
                    End If
                Else
                    documentWidth = 550.0F
                    sp = 35
                End If

            End If

            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim custdetailsdt As New DataTable
            custdetailsdt = ds.Tables(0)

            Dim tableTitle As PdfPTable = Nothing
            Dim arrHeaders() As String
            Dim colspanvalue As Integer

            '  currtype = IIf(curr <> "0", "(In Party Currency)", "(In Base Currency)")
            currcode = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)

            If curr = 1 Then
                curr = "(In A/C Currency)"
                currtype = custdetailsdt.AsEnumerable().Select(Function(s) s.Field(Of String)("currcode")).FirstOrDefault
            Else
                curr = "(In Base Currency)"
                currtype = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
            End If

            If movflg = "0" Then
                tableTitle = New PdfPTable(9)
                arrHeaders = {colheader1, colheader2, "Currency", "Opening", "Current", "Closing"}
                tableTitle.SetWidths(New Single() {0.14F, 0.21F, 0.11F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F})
                head = "Report- " & rptreportname & " - Transaction : From  " &
                    Format(Convert.ToDateTime(fromdate), "dd/MM/yyyy") & "  To" & " " &
                    Format(Convert.ToDateTime(todate), "dd/MM/yyyy") & " " & curr
            ElseIf movflg = "1" Then
                head = "Report- " & rptreportname & " - Balances As of : From  " &
                    Format(Convert.ToDateTime(fromdate), "dd/MM/yyyy") & " " & curr
                If type = "S" Then
                    tableTitle = New PdfPTable(5)
                    arrHeaders = {colheader1, colheader2, "Currency", "Debit", "Credit"}
                    tableTitle.SetWidths(New Single() {0.16F, 0.34F, 0.14, 0.18F, 0.18F})
                ElseIf type = "C" Then
                    tableTitle = New PdfPTable(10)
                    arrHeaders = {colheader1, colheader2, "Telephone", "Fax", "Crlimit", "Credit Type", "Curr.", "Debit", "Credit", "Remarks"}
                    tableTitle.SetWidths(New Single() {0.08F, 0.16F, 0.12F, 0.1F, 0.09F, 0.08F, 0.07F, 0.09F, 0.11F, 0.12F})

                End If
            End If

            'If Not (String.IsNullOrEmpty(fromcode)) And Not (String.IsNullOrEmpty(tocode)) Then
            '    reportFilter = reportFilter & Space(2) & IIf(type = "C", "Customer Name From", "Supplier Name From") & Space(2) & fromcode & Space(2) & "To" & Space(2) & tocode
            'End If
            'If type = "S" And Not (String.IsNullOrEmpty(fromsptype)) And Not (String.IsNullOrEmpty(tosptype)) Then
            '    reportFilter = reportFilter & Space(2) & "Supplier City From" & Space(2) & frommarketcode & Space(2) & "To" & Space(2) & tomarketcode
            'End If

            'If Not (String.IsNullOrEmpty(frommarketcode)) And Not (String.IsNullOrEmpty(tomarketcode)) Then
            '    reportFilter = reportFilter & Space(2) & IIf(type = "C", "Market  From", "Supplier City From") & Space(2) & frommarketcode & Space(2) & "To" & Space(2) & tomarketcode
            'End If

            'If Not (String.IsNullOrEmpty(fromctry)) And Not (String.IsNullOrEmpty(toctry)) Then
            '    reportFilter = reportFilter & Space(2) & "Supplier Country From" & Space(2) & fromctry & Space(2) & "To" & Space(2) & toctry
            'End If
            'If Not (String.IsNullOrEmpty(fromcat)) And Not (String.IsNullOrEmpty(tocat)) Then
            '    reportFilter = reportFilter & Space(2) & "Category Code From" & Space(2) & fromcat & Space(2) & "To" & Space(2) & tocat
            'End If
            'If Not (String.IsNullOrEmpty(fromglcode)) And Not (String.IsNullOrEmpty(toglcode)) Then
            '    reportFilter = reportFilter & Space(2) & "Control Account Code From " & Space(2) & fromglcode & Space(2) & "To" & Space(2) & toglcode
            'End If
            reportFilter = rptfilter

            If reportsType = "excel" Then
                heading = head
                CustTrialBal_WithMovementExcel(custdetailsdt, type, curr, bytes, movflg, arrHeaders)
            Else
                Using memoryStream As New System.IO.MemoryStream()
                    Dim writer As PdfWriter = PdfWriter.GetInstance(pdfdoc, memoryStream)
                    Dim titletable As PdfPTable = Nothing

                    Dim CompanybgColor As BaseColor = New BaseColor(0, 72, 192)
                    Dim ReportNamebgColor As BaseColor = New BaseColor(0, 128, 192)

                    titletable = New PdfPTable(1)
                    titletable.TotalWidth = documentWidth
                    titletable.LockedWidth = True
                    titletable.SetWidths(New Single() {1.0F})

                    titletable.Complete = False
                    titletable.SplitRows = False
                    'company name
                    phrase = New Phrase()
                    phrase.Add(New Chunk(rptcompanyname, CompanyFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    cell.BackgroundColor = CompanybgColor
                    titletable.AddCell(cell)
                    ' pdfdoc.Add(titletable)
                    'Report name

                    Dim Reporttitle = New PdfPTable(1)
                    Reporttitle.TotalWidth = documentWidth
                    Reporttitle.LockedWidth = True
                    Reporttitle.SetWidths(New Single() {1.0F})
                    Reporttitle.Complete = False
                    Reporttitle.SplitRows = False
                    phrase = New Phrase()
                    phrase.Add(New Chunk(rptreportname, ReportNameFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.PaddingBottom = 4
                    cell.SetLeading(12, 0)
                    cell.BackgroundColor = ReportNamebgColor
                    '  cell.PaddingBottom = 
                    Reporttitle.SpacingBefore = 5
                    Reporttitle.SpacingAfter = 5
                    Reporttitle.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(head, HeadFontBold))
                    cell = New PdfPCell(phrase)
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    '  cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 5
                    Reporttitle.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(reportFilter, HeadFontBold))
                    cell = New PdfPCell(phrase)
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 5
                    Reporttitle.AddCell(cell)

                    Dim FooterTable = New PdfPTable(1)
                    FooterTable.TotalWidth = documentWidth
                    FooterTable.LockedWidth = True
                    FooterTable.SetWidths(New Single() {1.0F})
                    FooterTable.Complete = False
                    FooterTable.SplitRows = False

                    phrase = New Phrase()
                    phrase.Add(New Chunk(addrLine3 + addrLine5, Footerfont))
                    cell = New PdfPCell(phrase)
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Colspan = 2
                    cell.SetLeading(10, 0)
                    ' cell.PaddingBottom = 1
                    FooterTable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(addrLine1, Footerfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Colspan = 2
                    cell.SetLeading(10, 0)
                    ' cell.PaddingBottom = 1
                    FooterTable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(addrLine2 + "  " + addrLine4, Footerfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Colspan = 2
                    cell.SetLeading(10, 0)
                    ' cell.PaddingBottom = 1
                    FooterTable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk("Printed Date:" & Date.Now.ToString("dd/MM/yyyy HH:mm:ss"), Footerfont))
                    cell = New PdfPCell(phrase)
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Colspan = 2
                    cell.SetLeading(10, 0)
                    cell.PaddingBottom = 3.0F
                    FooterTable.AddCell(cell)
                    FooterTable.Complete = True
                    tableTitle.TotalWidth = documentWidth
                    tableTitle.LockedWidth = True
                    tableTitle.SplitRows = False
                    '  tableTitle.KeepTogether = True
                    tableTitle.SpacingBefore = 20
                    tableTitle.SpacingAfter = 0

                    For i = 0 To arrHeaders.Length - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrHeaders(i), TableHeadFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.SetLeading(12, 0)
                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 1.0F

                        If movflg = "0" Then
                            If i <= 2 Then
                                cell.BorderWidthBottom = 0
                            Else
                                cell.Colspan = 2
                            End If
                        End If

                        tableTitle.AddCell(cell)

                    Next

                    If movflg = "0" Then
                        Dim arrHeader() As String = {"", "", "", "Debit", "Credit", "Debit", "Credit", "Debit", "Credit"}
                        For i = 0 To arrHeader.Length - 1

                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrHeader(i), NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            If i <= 2 Then
                                cell.BorderWidthTop = 0
                            End If
                            cell.SetLeading(12, 0)
                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F

                            tableTitle.AddCell(cell)
                        Next
                    End If


                    writer.PageEvent = New ClsHeaderFooter(titletable, Reporttitle, FooterTable, tableTitle)

                    pdfdoc.Open()
                    If custdetailsdt.Rows.Count > 0 Then
                        If movflg = "1" Then

                            CustTrialBal_WithioutMovement1(custdetailsdt, type, curr, pdfdoc, movflg)
                        ElseIf movflg = "0" Then
                            Dim co = custdetailsdt.Rows.Count

                            Dim grpby = From custledger In custdetailsdt.AsEnumerable() Group custledger By g = New With {Key .currcode = custledger.Field(Of String)("currcode")} Into Group Order By g.currcode

                            Dim currarr(grpby.Count) As String

                            For Each row In grpby
                                Dim count = 0
                                Dim c As String = (row.g.currcode).ToString()

                                currarr(count) = c
                                count = count + 1
                            Next

                            For j = 0 To currarr.Length - 2

                                dscurrecncy = custdetailsdt.Select("currcode='" & currarr(j) & "'")
                                If dscurrecncy.Length > 0 Then
                                    GrandTotalv = "Total"
                                    If currarr.Length > 2 Then
                                        GrandTotalv = "Grand Total"
                                    End If
                                    custdetailsdtgpby = dscurrecncy.CopyToDataTable()
                                    Dim tableData = New PdfPTable(9)
                                    tableData.SetWidths(New Single() {0.14F, 0.21F, 0.11F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F})
                                    tableData.TotalWidth = documentWidth
                                    tableData.LockedWidth = True
                                    tableData.Complete = False
                                    tableData.SplitRows = False
                                    tableData.SpacingBefore = 0.0F

                                    SubTotal(type, custdetailsdtgpby, tableData, movflg, ws)

                                    Totalodebit = Convert.ToDouble(custdetailsdt.Compute("SUM(odebit)", "currcode ='" & currarr(j) & "'"))
                                    Totalocredit = Convert.ToDouble(custdetailsdt.Compute("SUM(ocredit)", "currcode ='" & currarr(j) & "'"))

                                    Totalcdebit = Convert.ToDouble(custdetailsdt.Compute("SUM(cdebit)", "currcode ='" & currarr(j) & "'"))
                                    Totalccredit = Convert.ToDouble(custdetailsdt.Compute("SUM(ccredit)", "currcode='" & currarr(j) & "'"))


                                    Totaltdebit = Convert.ToDouble(custdetailsdt.Compute("SUM(tdebit)", "currcode ='" & currarr(j) & "'"))
                                    Totaltcredit = Convert.ToDouble(custdetailsdt.Compute("SUM(tcredit)", "currcode ='" & currarr(j) & "'"))


                                    pdfdoc.Add(tableData)
                                    If curr <> "0" Then
                                        Dim TotalTable = New PdfPTable(9)
                                        TotalTable.SetWidths(New Single() {0.14F, 0.21F, 0.11F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F})
                                        TotalTable.TotalWidth = documentWidth
                                        TotalTable.LockedWidth = True
                                        TotalTable.Complete = False
                                        TotalTable.SplitRows = False
                                        TotalTable.SpacingBefore = 0.0F

                                        Dim Total() As String = {"", currarr(j), "Total", Totalodebit.ToString(decimalPoint), Totalocredit.ToString(decimalPoint), Totalcdebit.ToString(decimalPoint), Totalccredit.ToString(decimalPoint), Totaltdebit.ToString(decimalPoint), Totaltcredit.ToString(decimalPoint)}
                                        For i = 0 To Total.Length - 1
                                            phrase = New Phrase()
                                            phrase.Add(New Chunk(Total(i), NormalFontBold))
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                            cell.SetLeading(12, 0)
                                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                            cell.PaddingBottom = 4.0F
                                            cell.PaddingTop = 1.0F
                                            cell.BackgroundColor = Rowtitlebg
                                            TotalTable.AddCell(cell)
                                        Next

                                        pdfdoc.Add(TotalTable)
                                    End If

                                End If
                            Next
                            'dscurrecncy = custdetailsdt.Select("currcode='USD'")
                            'If dscurrecncy.Length > 0 Then
                            '    custdetailsdtgpby = dscurrecncy.CopyToDataTable()
                            '    GrandTotalv = "Grand Total"
                            '    Dim tableData1 = New PdfPTable(9)
                            '    tableData1.SetWidths(New Single() {0.14F, 0.21F, 0.11F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F})
                            '    tableData1.TotalWidth = documentWidth
                            '    tableData1.LockedWidth = True
                            '    tableData1.Complete = False
                            '    tableData1.SplitRows = False
                            '    tableData1.SpacingBefore = 0

                            '    SubTotal(type, custdetailsdtgpby, tableData1, movflg, ws)
                            '    pdfdoc.Add(tableData1)

                            '    Totalodebitu = (Convert.ToDouble(custdetailsdt.Compute("SUM(odebit)", "currcode ='USD'")))
                            '    Totalocreditu = Convert.ToDouble(custdetailsdt.Compute("SUM(ocredit)", "currcode ='USD'"))

                            '    Totalcdebitu = Convert.ToDouble(custdetailsdt.Compute("SUM(cdebit)", "currcode ='USD'"))
                            '    Totalccreditu = Convert.ToDouble(custdetailsdt.Compute("SUM(ccredit)", "currcode ='USD'"))


                            '    Totaltdebitu = Convert.ToDouble(custdetailsdt.Compute("SUM(tdebit)", "currcode ='USD'"))
                            '    Totaltcreditu = Convert.ToDouble(custdetailsdt.Compute("SUM(tcredit)", "currcode ='USD'"))

                            '    Dim TotalTable1 = New PdfPTable(9)
                            '    TotalTable1.SetWidths(New Single() {0.14F, 0.21F, 0.11F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F})
                            '    TotalTable1.TotalWidth = documentWidth
                            '    TotalTable1.LockedWidth = True
                            '    TotalTable1.SplitRows = False
                            '    TotalTable1.SpacingBefore = 0.0F

                            '    Dim Total1() As String = {"", "USD", "Total", Totalodebitu.ToString(decimalPoint), Totalocreditu.ToString(decimalPoint), Totalcdebitu.ToString(decimalPoint), Totalccreditu.ToString(decimalPoint), Totaltdebitu.ToString(decimalPoint), Totaltcreditu.ToString(decimalPoint)}
                            '    For i = 0 To Total1.Length - 1
                            '        phrase = New Phrase()
                            '        phrase.Add(New Chunk(Total1(i), NormalFontBold))
                            '        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            '        cell.SetLeading(12, 0)
                            '        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                            '        cell.PaddingBottom = 4.0F
                            '        cell.PaddingTop = 1.0F
                            '        cell.BackgroundColor = Rowtitlebg
                            '        TotalTable1.AddCell(cell)
                            '    Next
                            '    pdfdoc.Add(TotalTable1)

                            'End If
                            Dim GrandTotal = New PdfPTable(9)
                            GrandTotal.SetWidths(New Single() {0.14F, 0.21F, 0.11F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F})
                            GrandTotal.TotalWidth = documentWidth
                            GrandTotal.LockedWidth = True
                            GrandTotal.Complete = False
                            GrandTotal.SplitRows = False
                            GrandTotal.SpacingBefore = 4
                            GrandTotal.DefaultCell.Border = Rectangle.NO_BORDER


                            Dim Total2() As String = {"", " ", GrandTotalv, (Totalodebit + Totalodebitu).ToString(decimalPoint), (Totalocredit + Totalocreditu).ToString(decimalPoint), (Totalcdebit + Totalcdebitu).ToString(decimalPoint), (Totalccredit + Totalccreditu).ToString(decimalPoint), (Totaltdebit + Totaltdebitu).ToString(decimalPoint), (Totaltcredit + Totaltcreditu).ToString(decimalPoint)}
                            For i = 0 To Total2.Length - 1
                                phrase = New Phrase()
                                phrase.Add(New Chunk(Total2(i), NormalFontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                cell.SetLeading(12, 0)
                                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                If i = 0 Then
                                    cell.BorderWidthRight = 0
                                ElseIf i = Total2.Length - 1 Then
                                    cell.BorderWidthLeft = 0
                                Else
                                    cell.BorderWidthRight = 0
                                    cell.BorderWidthLeft = 0
                                End If
                                cell.PaddingBottom = 4.0F
                                cell.PaddingTop = 1.0F
                                cell.FixedHeight = 20
                                cell.BackgroundColor = grandtotalbg
                                GrandTotal.AddCell(cell)
                            Next

                            pdfdoc.Add(GrandTotal)

                            'Dim NetTotal = New PdfPTable(9)
                            Dim NetTotal = New PdfPTable(4)
                            ' NetTotal.SetWidths(New Single() {0.14F, 0.21F, 0.11F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F})
                            NetTotal.SetWidths(New Single() {0.47F, 0.18F, 0.18F, 0.18F})
                            NetTotal.TotalWidth = documentWidth
                            NetTotal.LockedWidth = True
                            NetTotal.Complete = False
                            NetTotal.SplitRows = False
                            NetTotal.SpacingBefore = 4
                            NetTotal.DefaultCell.Border = Rectangle.NO_BORDER

                            If IsgroupBy = 0 Then
                                toDebit = IIf(Totalodebitu - Totalocreditu = 0.0, Totalodebit - Totalocredit, Totalodebitu - Totalocreditu)
                            Else
                                toDebit = sodebit - socredit
                            End If
                            Dim Total3() As String
                            If type = "C" Then
                                If movflg = "0" Then
                                    '    Total3 = {"", " ", "Net Final Total", toDebit.ToString(decimalPoint), "",
                                    '              ((Totalcdebit + Totalcdebitu) - (Totalccredit + Totalccreditu)).ToString(decimalPoint),
                                    '             "", "", ((Totaltdebit + Totaltdebitu) - (Totaltcredit + Totaltcreditu)).ToString(decimalPoint)}

                                    Total3 = {"Net Final Total", toDebit.ToString(decimalPoint),
                                              ((Totalcdebit + Totalcdebitu) - (Totalccredit + Totalccreditu)).ToString(decimalPoint),
                                              ((Totaltdebit + Totaltdebitu) - (Totaltcredit + Totaltcreditu)).ToString(decimalPoint)}

                                Else
                                    '    Total3 = {"", " ", "Net Final Total", toDebit.ToString(decimalPoint), "",
                                    '              ((Totalcdebit + Totalcdebitu) - (Totalccredit + Totalccreditu)).ToString(decimalPoint), "",
                                    '              ((Totaltdebit + Totaltdebitu) - (Totaltcredit + Totaltcreditu)).ToString(decimalPoint), ""}
                                    Total3 = {"Net Final Total", toDebit.ToString(decimalPoint),
                                            ((Totalcdebit + Totalcdebitu) - (Totalccredit + Totalccreditu)).ToString(decimalPoint),
                                               ((Totaltdebit + Totaltdebitu) - (Totaltcredit + Totaltcreditu)).ToString(decimalPoint)}
                                End If
                            ElseIf type = "S" Then
                                If movflg = "0" Then
                                    'Total3 = {"", " ", "Net Final Total", "",
                                    '          (Totalocredit + Totalocreditu - (Totalodebit + Totalodebitu)).ToString(decimalPoint), "",
                                    '          ((Totalccredit + Totalccreditu) - (Totalcdebit + Totalcdebitu)).ToString(decimalPoint), "",
                                    '          ((Totaltcredit + Totaltcreditu) - (Totaltdebit + Totaltdebitu)).ToString(decimalPoint)}
                                    Total3 = {"Net Final Total",
                                         (Totalocredit + Totalocreditu - (Totalodebit + Totalodebitu)).ToString(decimalPoint),
                                         ((Totalccredit + Totalccreditu) - (Totalcdebit + Totalcdebitu)).ToString(decimalPoint),
                                         ((Totaltcredit + Totaltcreditu) - (Totaltdebit + Totaltdebitu)).ToString(decimalPoint)}

                                Else
                                    '    Total3 = {"", " ", "Net Final Total",
                                    '              (Totalocredit + Totalocreditu - (Totalodebit + Totalodebitu)).ToString(decimalPoint), "",
                                    '              ((Totalccredit + Totalccreditu) - (Totalcdebit + Totalcdebitu)).ToString(decimalPoint), "",
                                    '              ((Totaltcredit + Totaltcreditu) - (Totaltdebit + Totaltdebitu)).ToString(decimalPoint), ""}
                                    Total3 = {"Net Final Total",
                                          (Totalocredit + Totalocreditu - (Totalodebit + Totalodebitu)).ToString(decimalPoint),
                                          ((Totalccredit + Totalccreditu) - (Totalcdebit + Totalcdebitu)).ToString(decimalPoint),
                                          ((Totaltcredit + Totaltcreditu) - (Totaltdebit + Totaltdebitu)).ToString(decimalPoint)}

                                End If
                            End If

                            For i = 0 To Total3.Length - 1
                                phrase = New Phrase()
                                phrase.Add(New Chunk(Total3(i), NormalFontBold))
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                'If i = 2 Then
                                '    cell.Colspan = 3
                                '    'ElseIf i = 4 Then
                                '    '    cell.Colspan = 2
                                '    '    i = 4
                                '    'ElseIf i = 5 Then
                                '    '    cell.Colspan = 2
                                '    '    i = 6
                                'End If
                                cell.SetLeading(12, 0)
                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                cell.PaddingBottom = 4.0F
                                cell.PaddingTop = 1.0F
                                If i = 0 Then
                                    cell.BorderWidthRight = 0
                                ElseIf i = Total3.Length - 1 Then
                                    cell.BorderWidthLeft = 0
                                Else
                                    cell.BorderWidthRight = 0
                                    cell.BorderWidthLeft = 0
                                End If

                                NetTotal.AddCell(cell)
                            Next

                            pdfdoc.Add(NetTotal)
                        End If
                    End If
                    pdfdoc.Close()

                    If printMode = "download" Then
                        Dim pagingFont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)
                        Dim reader As New PdfReader(memoryStream.ToArray())
                        Using mStream As New System.IO.MemoryStream()
                            Using stamper As New PdfStamper(reader, mStream)
                                Dim pages As Integer = reader.NumberOfPages
                                For i As Integer = 1 To pages
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), documentWidth, 8.0F, 0)
                                    ' ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 775.0F, 10.0F, 0)

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

    Private Shared Function PhraseCell(ByVal phrase As Phrase, ByVal align As Integer, ByVal Cols As Integer, ByVal celBorder As Boolean, Optional ByVal celBottomBorder As String = "None") As PdfPCell
        Dim cell As PdfPCell = New PdfPCell(phrase)
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

    Public Sub SubTotal(ByVal type As String, ByVal dscurrecncy As DataTable, ByRef table As PdfPTable, ByVal movflg As String, ByRef ws As IXLWorksheet)
        ' Dim groupbyrows As New DataTable

        Dim groupby, gptype As String

        If rpttype = "pdf" Then
            table.SetWidths(New Single() {0.14F, 0.21F, 0.11F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F})
            table.TotalWidth = documentWidth
            table.LockedWidth = True
            table.Complete = False
            table.SplitRows = False
            table.SpacingBefore = 0
        End If

        Dim arrData1() As String

        If IsgroupBy = 0 Then
            Dim arrData() As String
            ' Without Group By
            For Each row In dscurrecncy.Rows
                If rpttype = "excel" Then
                    arrData = {(row("acc_code") & Space(1) & ".").ToString(), row("acc_name"), row("currcode"), Decimal.Parse(row("odebit")).ToString(decimalPoint), Decimal.Parse(row("ocredit")).ToString(decimalPoint), Decimal.Parse(row("cdebit")).ToString(decimalPoint), Decimal.Parse(row("ccredit")).ToString(decimalPoint), Decimal.Parse(row("tdebit")).ToString(decimalPoint), Decimal.Parse(row("tcredit")).ToString(decimalPoint)}
                Else
                    arrData = {row("acc_code").ToString(), row("acc_name"), row("currcode"), Decimal.Parse(row("odebit")).ToString(decimalPoint), Decimal.Parse(row("ocredit")).ToString(decimalPoint), Decimal.Parse(row("cdebit")).ToString(decimalPoint), Decimal.Parse(row("ccredit")).ToString(decimalPoint), Decimal.Parse(row("tdebit")).ToString(decimalPoint), Decimal.Parse(row("tcredit")).ToString(decimalPoint)}
                End If

                rowcount = rowcount + 1
                ws.Range(rowcount, 1, rowcount, arrData.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Font.FontSize = 9
                ws.Range(rowcount, 1, rowcount, arrData.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.WrapText = True
                ws.Range(rowcount, 1, rowcount, arrData.Length).Style.Alignment.WrapText = True

                'Dim arrData() As String = {dscurrecncy(j)("acc_code"), (dscurrecncy(j)("acc_name")), (Decimal.Parse(dscurrecncy(j)("currcode"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("odebit"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("ocredit"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("cdebit"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("ccredit"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("tdebit"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("tcredit"))).ToString(decimalPoint)}
                For i = 0 To arrData.Length - 1

                    If rpttype = "pdf" Then
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrData(i), NormalFont))

                        If i > 2 Then
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                        ElseIf i = 2 Then
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                        Else
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        End If

                        cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 1.0F
                        table.AddCell(cell)

                    Else
                        If i > 2 Then
                            ws.Cell(rowcount, i + 1).Value = Decimal.Parse(arrData(i))
                            ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).NumberFormat.Format = DecimalPoints
                        ElseIf i = 2 Then
                            ws.Cell(rowcount, i + 1).Value = arrData(i)
                            ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        ElseIf i = 0 Then
                            ws.Cell(rowcount, i + 1).Value = arrData(i)
                            ws.Cell(rowcount, i + 1).SetDataType(XLCellValues.Text)
                        Else
                            ws.Cell(rowcount, i + 1).Value = arrData(i)
                            ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        End If
                    End If

                Next
            Next



        Else

            'with Group by
            If IsgroupBy = 1 Then
                If type = "S" Then
                    groupby = "sptypename"
                    gptype = "SP Type Name : "
                ElseIf type = "C" Then
                    groupby = "Marketname"
                    gptype = "Market Name : "
                End If

            ElseIf IsgroupBy = 2 Then
                groupby = "catname"
                gptype = "Category Name : "
            ElseIf IsgroupBy = 3 Then
                groupby = "glname"
                gptype = "Control A/C Name : "
            End If

            Dim groups As IEnumerable(Of IGrouping(Of String, DataRow)) = dscurrecncy.AsEnumerable.GroupBy(Function(g) g.Field(Of String)(groupby)).OrderBy(Function(o) o.Key)

            For Each ls In groups
                'ls.Key
                socredit = 0.0
                sodebit = 0.0
                sccredit = 0.0
                scdebit = 0.0
                stcredit = 0.0
                stdebit = 0.0
                phrase = New Phrase()

                rowcount = rowcount + 1
                ws.Range(rowcount, 1, rowcount, 9).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Font.SetBold().Font.FontSize = 9
                ws.Range(rowcount, 1, rowcount, 9).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Fill.SetBackgroundColor(XLColor.LightPink).Alignment.WrapText = True
                ws.Range(rowcount, 1, rowcount, 9).Style.Fill.BackgroundColor = XLColor.FromHtml("#ffccff")
                ws.Range(rowcount, 1, rowcount, 9).Style.Alignment.WrapText = True
                If type = "C" Or movflg = "1" Then
                    If rpttype = "pdf" Then
                        phrase.Add(New Chunk(gptype & ls.Key, NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.SetLeading(12, 0)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 1.0F
                        cell.BackgroundColor = groupbg
                        cell.Colspan = 9
                        table.AddCell(cell)
                    Else
                        ws.Cell(rowcount, 1).Value = gptype & ls.Key
                    End If

                Else
                    If rpttype = "pdf" Then
                        phrase.Add(New Chunk(ls.Key, NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                        cell.SetLeading(12, 0)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 1.0F
                        cell.BackgroundColor = groupbg
                        cell.Colspan = 9
                        table.AddCell(cell)
                    Else
                        ws.Cell(rowcount, 1).Value = gptype & ls.Key
                    End If
                End If

                Dim arrData() As String

                For Each row As DataRow In ls

                    'Dim c = row.Field(Of String)("acc_code")
                    If rpttype = "excel" Then
                        arrData = {(row("acc_code") & Space(1) & ".").ToString(), row("acc_name"), row("currcode"), Decimal.Parse(row("odebit")).ToString(decimalPoint), Decimal.Parse(row("ocredit")).ToString(decimalPoint), Decimal.Parse(row("cdebit")).ToString(decimalPoint), Decimal.Parse(row("ccredit")).ToString(decimalPoint), Decimal.Parse(row("tdebit")).ToString(decimalPoint), Decimal.Parse(row("tcredit")).ToString(decimalPoint)}
                    Else
                        arrData = {row("acc_code"), row("acc_name"), row("currcode"), Decimal.Parse(row("odebit")).ToString(decimalPoint), Decimal.Parse(row("ocredit")).ToString(decimalPoint), Decimal.Parse(row("cdebit")).ToString(decimalPoint), Decimal.Parse(row("ccredit")).ToString(decimalPoint), Decimal.Parse(row("tdebit")).ToString(decimalPoint), Decimal.Parse(row("tcredit")).ToString(decimalPoint)}
                    End If

                    rowcount = rowcount + 1
                    ws.Range(rowcount, 1, rowcount, arrData.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
                    ws.Range(rowcount, 1, rowcount, arrData.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                    ws.Range(rowcount, 1, rowcount, arrData.Length).Style.Alignment.WrapText = True
                    'Dim arrData() As String = {dscurrecncy(j)("acc_code"), (dscurrecncy(j)("acc_name")), (Decimal.Parse(dscurrecncy(j)("currcode"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("odebit"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("ocredit"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("cdebit"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("ccredit"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("tdebit"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("tcredit"))).ToString(decimalPoint)}
                    For i = 0 To arrData.Length - 1
                        If rpttype = "pdf" Then
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrData(i), NormalFont))

                            If i > 2 Then
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT

                            Else
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                            End If

                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            table.AddCell(cell)
                        Else
                            If i > 2 Then
                                ws.Cell(rowcount, i + 1).Value = Decimal.Parse(arrData(i))
                                ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).NumberFormat.Format = DecimalPoints
                            ElseIf i = 2 Then
                                ws.Cell(rowcount, i + 1).Value = arrData(i)
                                ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                            ElseIf i = 0 Then
                                ws.Cell(rowcount, i + 1).Value = arrData(i)
                                ws.Cell(rowcount, i + 1).SetDataType(XLCellValues.Text)
                            Else
                                ws.Cell(rowcount, i + 1).Value = arrData(i)
                                ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            End If

                        End If
                    Next

                    '  If currtype = 1 Then
                    sodebit = sodebit + arrData(3)
                    socredit = socredit + arrData(4)
                    scdebit = scdebit + arrData(5)
                    sccredit = sccredit + arrData(6)
                    stdebit = stdebit + arrData(7)
                    stcredit = stcredit + arrData(8)

                Next

                arrData1 = {" ", "", "Sub Total", sodebit.ToString(decimalPoint), socredit.ToString(decimalPoint), scdebit.ToString(decimalPoint), sccredit.ToString(decimalPoint), stdebit.ToString(decimalPoint), stcredit.ToString(decimalPoint)}
                rowcount = rowcount + 1

                ws.Range(rowcount, 1, rowcount, arrData1.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold().Font.FontSize = 9
                ws.Range(rowcount, 1, rowcount, arrData1.Length).Style.Fill.SetBackgroundColor(XLColor.LightGray).Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True

                For i = 0 To arrData1.Length - 1
                    If rpttype = "pdf" Then
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrData1(i), NormalFontBold))
                        cell1 = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell1.SetLeading(12, 0)
                        cell1.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                        cell1.PaddingBottom = 4.0F
                        cell1.PaddingTop = 1.0F
                        cell1.BackgroundColor = Rowtitlebg
                        table.AddCell(cell1)
                    Else
                        If i < 3 Then
                            ws.Cell(rowcount, i + 1).Value = arrData1(i)
                        Else
                            ws.Cell(rowcount, i + 1).Value = Decimal.Parse(arrData1(i))
                            ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints
                        End If

                    End If

                Next

            Next
        End If
    End Sub

#Region "Excel CustTrialBal_WithMovement"
    Public Sub CustTrialBal_WithMovementExcel(ByVal custdetailsdt As DataTable, ByVal type As String, ByVal curr As String, ByRef bytes() As Byte, ByVal movflg As String, ByVal arrHeaders() As String)
        Dim colcount As Integer
        'Dim pdfdoc As Document = New Document(PageSize.A4, -10.0F, 10.0F, 30.0F, 35.0F)
        If decimalPoint = "N1" Then
            DecimalPoints = "##,##,##,##0.0"
        ElseIf decimalPoint = "N2" Then
            DecimalPoints = "##,##,##,##0.00"
        ElseIf decimalPoint = "N3" Then
            DecimalPoints = "##,##,##,##0.000"
        ElseIf decimalPoint = "N4" Then
            DecimalPoints = "##,##,##,##0.0000"
        Else
            DecimalPoints = "##,##,##,##0.00"

        End If
        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add("TraialBal")

        colcount = IIf(movflg = "0", arrHeaders.Length + 3, arrHeaders.Length)

        ws.Columns.AdjustToContents()
        ws.Column("B").Width = 30
        ws.Columns("D:I").Width = 13
        ws.Column("A").Width = 13
        If type = "C" And movflg = "1" Then
            ws.Column("C").Width = 13
            ws.Column("J").Width = 16

        ElseIf type = "S" And movflg = "1" Then
            ws.Column("B").Width = 35
            ws.Columns("D:I").Width = 15
            ws.Column("A").Width = 15
        End If

        'Comapny Name Heading
        ws.Cell("A2").Value = rptcompanyname
        Dim company = ws.Range(rowcount, 1, rowcount, colcount).Merge()
        company.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromHtml("#0048C0"))
        company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
        company.Style.Font.FontSize = 15
        company.Style.Font.FontColor = XLColor.White
        company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

        rowcount = rowcount + 1
        'Report Name Heading
        ws.Cell("A3").Value = rptreportname
        Dim report = ws.Range(rowcount, 1, rowcount, colcount).Merge()
        report.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromHtml("#0080C0"))
        report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
        report.Style.Font.FontSize = 14
        report.Style.Font.FontColor = XLColor.White
        report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
        'report.Style.Alignment.Vertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
        rowcount = rowcount + 1

        ws.Cell(rowcount, 1).Value = heading
        ws.Cell(rowcount, 1).Style.Alignment.WrapText = True
        Dim head = ws.Range(rowcount, 1, rowcount, colcount).Merge()
        head.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
        head.Style.Font.SetBold().Font.FontSize = 12
        head.Style.Font.FontColor = XLColor.Black
        head.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
        Dim rowheight As Integer

        If Not String.IsNullOrEmpty(reportFilter) Then
            rowcount = rowcount + 1

            If reportFilter.Length > 110 And type <> "S" And movflg <> "1" Then
                rowheight = IIf(reportFilter.Length > 110 And reportFilter.Length < 210, 32, IIf(reportFilter.Length > 210 And reportFilter.Length < 310, 48, 64))
                ws.Row(rowcount).Height = rowheight
            Else
                If reportFilter.Length > 70 Then
                    rowheight = IIf(reportFilter.Length > 70 And reportFilter.Length < 140, 32, IIf(reportFilter.Length > 140 And reportFilter.Length < 210, 48, 64))
                    ws.Row(rowcount).Height = rowheight
                End If

            End If

            ws.Cell(rowcount, 1).Value = reportFilter
            Dim filter = ws.Range(rowcount, 1, rowcount, colcount).Merge()
            filter.Style.Font.FontColor = XLColor.Black
            filter.Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Font.FontSize = 12
            filter.Style.Alignment.WrapText = True
        End If

        rowcount = rowcount + 2
        ws.Range(rowcount, 1, rowcount + 1, colcount).Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
        ws.Range(rowcount, 1, rowcount + 1, colcount).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
        ws.Range(rowcount, 1, rowcount + 1, colcount).Style.Alignment.WrapText = True

        For i = 0 To arrHeaders.Length - 1
            If movflg = "0" Then
                If i < 3 Then
                    ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
                    ws.Range(rowcount, i + 1, rowcount + 1, i + 1).Merge()
                ElseIf i = 3 Then
                    ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
                    ws.Range(rowcount, i + 1, rowcount, i + 2).Merge()
                ElseIf i = 4 Then

                    ws.Cell(rowcount, 6).Value = arrHeaders(i)
                    ws.Range(rowcount, 6, rowcount, 7).Merge()
                ElseIf i = 5 Then
                    ws.Cell(rowcount, 8).Value = arrHeaders(i)
                    ws.Range(rowcount, 8, rowcount, 9).Merge()
                End If
            Else
                ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
            End If
        Next


        If movflg = "0" Then
            Dim arr() As String = {"Debit", "Credit"}
            Dim col As Integer = 4
            rowcount = rowcount + 1
            ws.Cell(rowcount, col).Value = arr(0)
            ws.Cell(rowcount, col + 1).Value = arr(1)

            ws.Cell(rowcount, col + 2).Value = arr(0)
            ws.Cell(rowcount, col + 3).Value = arr(1)

            ws.Cell(rowcount, col + 4).Value = arr(0)
            ws.Cell(rowcount, col + 5).Value = arr(1)
        End If

        If custdetailsdt.Rows.Count > 0 Then
            If movflg = "1" Then

                WithioutMovementExcel(custdetailsdt, type, curr, ws, movflg)

            ElseIf movflg = "0" Then

                Dim co = custdetailsdt.Rows.Count

                Dim grpby = From custledger In custdetailsdt.AsEnumerable() Group custledger By g = New With {Key .currcode = custledger.Field(Of String)("currcode")} Into Group Order By g.currcode

                Dim currarr(grpby.Count) As String

                For Each row In grpby
                    Dim count = 0
                    Dim c As String = (row.g.currcode).ToString()

                    currarr(count) = c
                    count = count + 1
                Next

                For j = 0 To currarr.Length - 2

                    dscurrecncy = custdetailsdt.Select("currcode='" & currarr(j) & "'")
                    If dscurrecncy.Length > 0 Then
                        GrandTotalv = "Total"
                        If currarr.Length > 2 Then
                            GrandTotalv = "Grand Total"
                        End If

                        custdetailsdtgpby = dscurrecncy.CopyToDataTable()
                        Dim tableData = New PdfPTable(9)
                        tableData.SetWidths(New Single() {0.14F, 0.21F, 0.11F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F})
                        tableData.TotalWidth = documentWidth
                        tableData.LockedWidth = True
                        tableData.Complete = False
                        tableData.SplitRows = False
                        tableData.SpacingBefore = 0.0F

                        SubTotal(type, custdetailsdtgpby, tableData, movflg, ws)

                        Totalodebit = Convert.ToDouble(custdetailsdt.Compute("SUM(odebit)", "currcode ='" & currarr(j) & "'"))
                        Totalocredit = Convert.ToDouble(custdetailsdt.Compute("SUM(ocredit)", "currcode ='" & currarr(j) & "'"))

                        Totalcdebit = Convert.ToDouble(custdetailsdt.Compute("SUM(cdebit)", "currcode ='" & currarr(j) & "'"))
                        Totalccredit = Convert.ToDouble(custdetailsdt.Compute("SUM(ccredit)", "currcode='" & currarr(j) & "'"))


                        Totaltdebit = Convert.ToDouble(custdetailsdt.Compute("SUM(tdebit)", "currcode ='" & currarr(j) & "'"))
                        Totaltcredit = Convert.ToDouble(custdetailsdt.Compute("SUM(tcredit)", "currcode ='" & currarr(j) & "'"))


                        If curr <> "0" Then

                            Dim Total() As String = {"", currarr(j), "Total", Totalodebit.ToString(decimalPoint), Totalocredit.ToString(decimalPoint), Totalcdebit.ToString(decimalPoint), Totalccredit.ToString(decimalPoint), Totaltdebit.ToString(decimalPoint), Totaltcredit.ToString(decimalPoint)}
                            rowcount = rowcount + 1

                            ws.Range(rowcount, 1, rowcount, Total.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetBold().Font.FontSize = 8
                            ws.Range(rowcount, 1, rowcount, Total.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                            ws.Range(rowcount, 1, rowcount, Total.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Fill.BackgroundColor = XLColor.LightGray
                            For i = 0 To Total.Length - 1
                                If i < 3 Then
                                    ws.Cell(rowcount, i + 1).Value = Total(i)
                                Else
                                    ws.Cell(rowcount, i + 1).Value = Decimal.Parse(Total(i))
                                    ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints
                                End If

                            Next

                        End If

                    End If
                Next

                'dscurrecncy = custdetailsdt.Select("currcode='USD'")
                'If dscurrecncy.Length > 0 Then
                '    custdetailsdtgpby = dscurrecncy.CopyToDataTable()
                '    GrandTotalv = "Grand Total"
                '    Dim tableData1 = New PdfPTable(9)
                '    'tableData1.SetWidths(New Single() {0.14F, 0.21F, 0.11F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F})
                '    'tableData1.TotalWidth = documentWidth
                '    'tableData1.LockedWidth = True
                '    'tableData1.Complete = False
                '    'tableData1.SplitRows = False
                '    'tableData1.SpacingBefore = 0

                '    SubTotal(type, custdetailsdtgpby, tableData1, movflg, ws)
                '    ' pdfdoc.Add(tableData1)

                '    Totalodebitu = (Convert.ToDouble(custdetailsdt.Compute("SUM(odebit)", "currcode ='USD'")))
                '    Totalocreditu = Convert.ToDouble(custdetailsdt.Compute("SUM(ocredit)", "currcode ='USD'"))

                '    Totalcdebitu = Convert.ToDouble(custdetailsdt.Compute("SUM(cdebit)", "currcode ='USD'"))
                '    Totalccreditu = Convert.ToDouble(custdetailsdt.Compute("SUM(ccredit)", "currcode ='USD'"))


                '    Totaltdebitu = Convert.ToDouble(custdetailsdt.Compute("SUM(tdebit)", "currcode ='USD'"))
                '    Totaltcreditu = Convert.ToDouble(custdetailsdt.Compute("SUM(tcredit)", "currcode ='USD'"))

                '    Dim TotalTable1 = New PdfPTable(9)
                '    TotalTable1.SetWidths(New Single() {0.14F, 0.21F, 0.11F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F})
                '    TotalTable1.TotalWidth = documentWidth
                '    TotalTable1.LockedWidth = True
                '    TotalTable1.SplitRows = False
                '    TotalTable1.SpacingBefore = 0.0F

                '    Dim Total1() As String = {"", "USD", "Total", Totalodebitu.ToString(decimalPoint), Totalocreditu.ToString(decimalPoint), Totalcdebitu.ToString(decimalPoint), Totalccreditu.ToString(decimalPoint), Totaltdebitu.ToString(decimalPoint), Totaltcreditu.ToString(decimalPoint)}
                '    rowcount = rowcount + 1
                '    ws.Range(rowcount, 1, rowcount, Total1.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetBold().Font.FontSize = 8
                '    ws.Range(rowcount, 1, rowcount, Total1.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                '    ws.Range(rowcount, 1, rowcount, Total1.Length).Style.Fill.BackgroundColor = XLColor.LightGray


                '    For i = 0 To Total1.Length - 1
                '        If i < 3 Then

                '            ws.Cell(rowcount, i + 1).Value = Total1(i)
                '        Else
                '            ws.Cell(rowcount, i + 1).Value = Decimal.Parse(Total1(i))
                '            ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints
                '        End If
                '    Next


                'End If
                Dim GrandTotal = New PdfPTable(9)
                GrandTotal.SetWidths(New Single() {0.14F, 0.21F, 0.11F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F})
                GrandTotal.TotalWidth = documentWidth
                GrandTotal.LockedWidth = True
                GrandTotal.Complete = False
                GrandTotal.SplitRows = False
                GrandTotal.SpacingBefore = 4
                GrandTotal.DefaultCell.Border = Rectangle.NO_BORDER


                Dim Total2() As String = {"", " ", GrandTotalv, (Totalodebit + Totalodebitu).ToString(decimalPoint), (Totalocredit + Totalocreditu).ToString(decimalPoint), (Totalcdebit + Totalcdebitu).ToString(decimalPoint), (Totalccredit + Totalccreditu).ToString(decimalPoint), (Totaltdebit + Totaltdebitu).ToString(decimalPoint), (Totaltcredit + Totaltcreditu).ToString(decimalPoint)}
                rowcount = rowcount + 1
                ws.Range(rowcount, 1, rowcount, Total2.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetBold().Font.FontSize = 8
                ws.Range(rowcount, 1, rowcount, Total2.Length).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                ws.Range(rowcount, 1, rowcount, Total2.Length).Style.Fill.BackgroundColor = XLColor.LightGray
                ws.Range(rowcount, 1, rowcount, Total2.Length).Style.Alignment.WrapText = True

                For i = 0 To Total2.Length - 1

                    If i < 3 Then
                        ws.Cell(rowcount, i + 1).Value = Total2(i)
                    Else
                        ws.Cell(rowcount, i + 1).Value = Decimal.Parse(Total2(i))
                        ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints
                    End If
                Next



                Dim NetTotal = New PdfPTable(9)
                NetTotal.SetWidths(New Single() {0.14F, 0.21F, 0.11F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F, 0.09F})
                NetTotal.TotalWidth = documentWidth
                NetTotal.LockedWidth = True
                NetTotal.Complete = False
                NetTotal.SplitRows = False
                NetTotal.SpacingBefore = 4
                NetTotal.DefaultCell.Border = Rectangle.NO_BORDER

                If IsgroupBy = 0 Then
                    toDebit = IIf(Totalodebitu - Totalocreditu = 0.0, Totalodebit - Totalocredit, Totalodebitu - Totalocreditu)
                Else
                    toDebit = sodebit - socredit
                End If
                Dim Total3() As String
                If type = "C" Then
                    If movflg = "0" Then
                        Total3 = {"", " ", "Net Final Total", toDebit.ToString(decimalPoint), "",
                                  ((Totalcdebit + Totalcdebitu) - (Totalccredit + Totalccreditu)).ToString(decimalPoint),
                                 "", "", ((Totaltdebit + Totaltdebitu) - (Totaltcredit + Totaltcreditu)).ToString(decimalPoint)}
                    Else
                        Total3 = {"", " ", "Net Final Total", toDebit.ToString(decimalPoint), "",
                                  ((Totalcdebit + Totalcdebitu) - (Totalccredit + Totalccreditu)).ToString(decimalPoint), "",
                                  ((Totaltdebit + Totaltdebitu) - (Totaltcredit + Totaltcreditu)).ToString(decimalPoint), ""}
                    End If
                ElseIf type = "S" Then
                    If movflg = "0" Then
                        Total3 = {"", " ", "Net Final Total", "",
                                  (Totalocredit + Totalocreditu - (Totalodebit + Totalodebitu)).ToString(decimalPoint), "",
                                  ((Totalccredit + Totalccreditu) - (Totalcdebit + Totalcdebitu)).ToString(decimalPoint), "",
                                  ((Totaltcredit + Totaltcreditu) - (Totaltdebit + Totaltdebitu)).ToString(decimalPoint)}
                    Else
                        Total3 = {"", " ", "Net Final Total",
                                  (Totalocredit + Totalocreditu - (Totalodebit + Totalodebitu)).ToString(decimalPoint), "",
                                  ((Totalccredit + Totalccreditu) - (Totalcdebit + Totalcdebitu)).ToString(decimalPoint), "",
                                  ((Totaltcredit + Totaltcreditu) - (Totaltdebit + Totaltdebitu)).ToString(decimalPoint), ""}
                    End If
                End If

                rowcount = rowcount + 1
                ws.Range(rowcount, 1, rowcount, Total2.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetBold().Font.FontSize = 8
                ws.Range(rowcount, 1, rowcount, Total2.Length).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                ws.Range(rowcount, 1, rowcount, Total2.Length).Style.Fill.BackgroundColor = XLColor.Gray
                ws.Range(rowcount, 1, rowcount, Total2.Length).Style.Alignment.WrapText = True

                For i = 0 To Total3.Length - 1
                    If i < 3 Then
                        ws.Cell(rowcount, i + 1).Value = Total3(i)
                    ElseIf String.IsNullOrEmpty(Total3(i)) Then
                        ws.Cell(rowcount, i + 1).Value = Total3(i)
                    Else
                        ws.Cell(rowcount, i + 1).Value = Decimal.Parse(Total3(i))
                        ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints
                    End If

                Next


            End If
        End If

        rowcount = rowcount + 2

        ws.Cell(rowcount, 1).Value = addrLine3 + " " + addrLine5
        ws.Range(rowcount, 1, rowcount, 5).Merge()
        rowcount = rowcount + 1
        ws.Cell(rowcount, 1).Value = addrLine1 + Space(3) + addrLine2 + "  " + addrLine4
        ws.Range(rowcount, 1, rowcount, 5).Merge()


        ws.Cell((rowcount + 1), 1).Value = "Printed Date:" & Now.ToString("dd/MM/yyyy")
        ws.Range((rowcount + 1), 1, (rowcount + 1), 5).Merge()
        Using wStream As New MemoryStream()
            wb.SaveAs(wStream)
            bytes = wStream.ToArray()
        End Using


    End Sub
#End Region

    Public Sub CustTrialBal_WithioutMovement1(ByVal custdetailsdt As DataTable, ByVal type As String, ByVal curr As String, ByRef pdfdoc As Document, ByVal movflg As String)
        Dim custdetailsdt1 As DataTable
        Dim co = custdetailsdt.Rows.Count

        Dim grpby = From custledger In custdetailsdt.AsEnumerable() Group custledger By g = New With {Key .currcode = custledger.Field(Of String)("currcode")} Into Group Order By g.currcode

        Dim currarr(grpby.Count) As String

        For Each row In grpby
            Dim count = 0
            Dim c As String = (row.g.currcode).ToString()

            currarr(count) = c
            count = count + 1
        Next
        'For Each rows In grpby
        '    custdetailsdt1 = rows.Group.CopyToDataTable

        For j = 0 To currarr.Length - 2
            ' Dim currs = currarr(j)
            ' dscurrecncy = custdetailsdt.Select("currcode='OMR'")
            dscurrecncy = custdetailsdt.Select("currcode='" & currarr(j) & "'")
            If dscurrecncy.Length > 0 Then
                custdetailsdtgpby = dscurrecncy.CopyToDataTable()


                GrandTotalv = "Total"
                If currarr.Length > 2 Then
                    GrandTotalv = "Grand Total"

                    Totaltdebitu = Convert.ToDouble(custdetailsdt.Compute("SUM(tdebit)", "currcode ='" & currarr(currarr.Length - 1) & "'"))
                    Totaltcreditu = Convert.ToDouble(custdetailsdt.Compute("SUM(tcredit)", "currcode ='" & currarr(currarr.Length - 1) & "'"))

                End If
                Dim tableData = New PdfPTable(10)
                Dim TotalTable = New PdfPTable(10)
                Dim Total() As String

                Totaltdebit = Convert.ToDouble(custdetailsdt.Compute("SUM(tdebit)", "currcode ='" & currarr(j) & "'"))
                Totaltcredit = Convert.ToDouble(custdetailsdt.Compute("SUM(tcredit)", "currcode ='" & currarr(j) & "'"))
                If type = "C" Then
                    tableData.SetWidths(New Single() {0.08F, 0.16F, 0.12F, 0.1F, 0.09F, 0.08F, 0.07F, 0.09F, 0.11F, 0.12F})
                    Total = {"", "", "", "", currarr(j) & "Total", "", "", Totaltdebit.ToString(decimalPoint), Totaltcredit.ToString(decimalPoint), ""}
                    TotalTable.SetWidths(New Single() {0.08F, 0.16F, 0.12F, 0.1F, 0.09F, 0.08F, 0.07F, 0.09F, 0.11F, 0.12F})
                ElseIf type = "S" Then
                    tableData = New PdfPTable(5)
                    tableData.SetWidths(New Single() {0.16F, 0.34F, 0.14, 0.18F, 0.18F})

                    TotalTable = New PdfPTable(5)
                    TotalTable.SetWidths(New Single() {0.16F, 0.34F, 0.14, 0.18F, 0.18F})
                    Total = {"", "", currarr(j) & "Total", Totaltdebit.ToString(decimalPoint), Totaltcredit.ToString(decimalPoint)}
                End If


                tableData.TotalWidth = documentWidth
                tableData.LockedWidth = True
                tableData.Complete = False
                tableData.SplitRows = False
                tableData.SpacingBefore = 0.0F

                SubTotalWitout(type, custdetailsdtgpby, tableData, movflg)
                pdfdoc.Add(tableData)
                If curr <> "0" Then

                    TotalTable.TotalWidth = documentWidth
                    TotalTable.LockedWidth = True
                    TotalTable.Complete = False
                    TotalTable.SplitRows = False
                    TotalTable.SpacingBefore = 0.0F

                    For i = 0 To Total.Length - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Total(i), NormalFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.SetLeading(12, 0)
                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 1.0F
                        cell.BackgroundColor = Rowtitlebg
                        TotalTable.AddCell(cell)
                    Next
                    pdfdoc.Add(TotalTable)
                End If
            End If

        Next
        '  Next
        Dim tableData1 = New PdfPTable(10)
        Dim TotalTable1 = New PdfPTable(10)
        Dim GrandTotal = New PdfPTable(10)
        Dim NetTotal = New PdfPTable(10)
        Dim Total1() As String
        Dim Total3() As String
        Dim Total2() As String

        'dscurrecncy = custdetailsdt.Select("currcode='USD'")
        'If dscurrecncy.Length > 0 Then
        '    custdetailsdtgpby = dscurrecncy.CopyToDataTable()
        '    GrandTotalv = "Grand Total"
        '    Totaltdebitu = Convert.ToDouble(custdetailsdt.Compute("SUM(tdebit)", "currcode ='USD'"))
        '    Totaltcreditu = Convert.ToDouble(custdetailsdt.Compute("SUM(tcredit)", "currcode ='USD'"))


        '    If type = "C" Then
        '        tableData1.SetWidths(New Single() {0.08F, 0.16F, 0.12F, 0.1F, 0.09F, 0.08F, 0.07F, 0.09F, 0.11F, 0.12F})
        '        Total1 = {"", "", "", "", "USD Total", "", "", Totaltdebitu.ToString(decimalPoint), Totaltcreditu.ToString(decimalPoint), ""}

        '        TotalTable1.SetWidths(New Single() {0.08F, 0.16F, 0.12F, 0.1F, 0.09F, 0.08F, 0.07F, 0.09F, 0.11F, 0.12F})

        '    ElseIf type = "S" Then
        '        tableData1 = New PdfPTable(5)
        '        tableData1.SetWidths(New Single() {0.16F, 0.34F, 0.14, 0.18F, 0.18F})


        '        TotalTable1 = New PdfPTable(5)
        '        TotalTable1.SetWidths(New Single() {0.16F, 0.34F, 0.14, 0.18F, 0.18F})
        '        Total1 = {"", "", "USD Total", Totaltdebitu.ToString(decimalPoint), Totaltcreditu.ToString(decimalPoint)}
        '    End If

        '    tableData1.TotalWidth = documentWidth
        '    tableData1.LockedWidth = True
        '    tableData1.Complete = False
        '    tableData1.SplitRows = False
        '    tableData1.SpacingBefore = 0

        '    SubTotalWitout(type, custdetailsdtgpby, tableData1, movflg)
        '    pdfdoc.Add(tableData1)

        '    TotalTable1.TotalWidth = documentWidth
        '    TotalTable1.LockedWidth = True
        '    TotalTable1.SplitRows = False
        '    TotalTable1.SpacingBefore = 0.0F
        '    For i = 0 To Total1.Length - 1
        '        phrase = New Phrase()
        '        phrase.Add(New Chunk(Total1(i), NormalFontBold))
        '        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
        '        cell.SetLeading(12, 0)
        '        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
        '        cell.PaddingBottom = 4.0F
        '        cell.PaddingTop = 1.0F
        '        cell.BackgroundColor = Rowtitlebg
        '        TotalTable1.AddCell(cell)
        '    Next
        '    pdfdoc.Add(TotalTable1)

        'End If

        If type = "C" Then
            GrandTotal.SetWidths(New Single() {0.08F, 0.16F, 0.12F, 0.1F, 0.09F, 0.08F, 0.07F, 0.09F, 0.11F, 0.12F})
            Total2 = {"", " ", "", "", GrandTotalv, "", (Totaltdebit + Totaltdebitu).ToString(decimalPoint), (Totaltcredit + Totaltcreditu).ToString(decimalPoint), ""}
            NetTotal.SetWidths(New Single() {0.08F, 0.16F, 0.12F, 0.1F, 0.09F, 0.08F, 0.07F, 0.09F, 0.11F, 0.12F})
            If movflg = "0" Then
                Total3 = {"", " ", "", "", "Net Final Total", "", "", "",
                          ((Totaltdebit + Totaltdebitu) - (Totaltcredit + Totaltcreditu)).ToString(decimalPoint)}
            Else
                Total3 = {"", " ", "", "", "Net Final Total", "", "",
                          ((Totaltdebit + Totaltdebitu) - (Totaltcredit + Totaltcreditu)).ToString(decimalPoint), ""}
            End If
            GrandTotal.TotalWidth = documentWidth
            GrandTotal.LockedWidth = True
            GrandTotal.Complete = False
            GrandTotal.SplitRows = False
            GrandTotal.SpacingBefore = 4
            GrandTotal.DefaultCell.Border = Rectangle.NO_BORDER

            For i = 0 To Total2.Length - 1
                phrase = New Phrase()
                phrase.Add(New Chunk(Total2(i), TableHeadFontBold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                cell.SetLeading(12, 0)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                If i = 0 Then
                    cell.BorderWidthRight = 0

                ElseIf i = 4 Then
                    cell.Colspan = 2
                    cell.BorderWidthRight = 0
                    cell.BorderWidthLeft = 0

                ElseIf i = Total2.Length - 1 Then
                    cell.BorderWidthLeft = 0
                Else
                    cell.BorderWidthRight = 0
                    cell.BorderWidthLeft = 0
                End If
                cell.PaddingBottom = 4.0F
                cell.PaddingTop = 1.0F
                cell.FixedHeight = 20
                cell.BackgroundColor = grandtotalbg
                GrandTotal.AddCell(cell)
            Next

            pdfdoc.Add(GrandTotal)

            NetTotal.TotalWidth = documentWidth
            NetTotal.LockedWidth = True
            NetTotal.Complete = False
            NetTotal.SplitRows = False
            NetTotal.SpacingBefore = 4
            NetTotal.DefaultCell.Border = Rectangle.NO_BORDER
            For i = 0 To Total3.Length - 1
                phrase = New Phrase()
                phrase.Add(New Chunk(Total3(i), TableHeadFontBold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                cell.SetLeading(12, 0)
                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                cell.PaddingBottom = 4.0F
                cell.PaddingTop = 1.0F
                If i = 0 Then
                    cell.BorderWidthRight = 0
                ElseIf i = 4 Then
                    cell.Colspan = 2
                    cell.BorderWidthRight = 0
                    cell.BorderWidthLeft = 0
                ElseIf i = Total3.Length - 1 Then
                    cell.BorderWidthLeft = 0
                Else
                    cell.BorderWidthRight = 0
                    cell.BorderWidthLeft = 0
                End If

                NetTotal.AddCell(cell)
            Next
            pdfdoc.Add(NetTotal)

        ElseIf type = "S" Then
            Total2 = {"", " ", GrandTotalv, (Totaltdebit + Totaltdebitu).ToString(decimalPoint), (Totaltcredit + Totaltcreditu).ToString(decimalPoint)}
            GrandTotal = New PdfPTable(5)
            GrandTotal.SetWidths(New Single() {0.16F, 0.34F, 0.14, 0.18F, 0.18F})

            NetTotal = New PdfPTable(5)
            NetTotal.SetWidths(New Single() {0.16F, 0.34F, 0.14, 0.18F, 0.18F})
            Total3 = {"", "", "Net Final Total", "", ((Totaltcredit + Totaltcreditu) - (Totaltdebit + Totaltdebitu)).ToString(decimalPoint)}

            GrandTotal.TotalWidth = documentWidth
            GrandTotal.LockedWidth = True
            GrandTotal.Complete = False
            GrandTotal.SplitRows = False
            GrandTotal.SpacingBefore = 4
            GrandTotal.DefaultCell.Border = Rectangle.NO_BORDER

            For i = 0 To Total2.Length - 1
                phrase = New Phrase()
                phrase.Add(New Chunk(Total2(i), TableHeadFontBold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                cell.SetLeading(12, 0)
                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                If i = 0 Then
                    cell.BorderWidthRight = 0
                ElseIf i = Total2.Length - 1 Then
                    cell.BorderWidthLeft = 0
                Else
                    cell.BorderWidthRight = 0
                    cell.BorderWidthLeft = 0
                End If
                cell.PaddingBottom = 4.0F
                cell.PaddingTop = 1.0F
                cell.FixedHeight = 20
                cell.BackgroundColor = grandtotalbg
                GrandTotal.AddCell(cell)
            Next

            pdfdoc.Add(GrandTotal)

            NetTotal.TotalWidth = documentWidth
            NetTotal.LockedWidth = True
            NetTotal.Complete = False
            NetTotal.SplitRows = False
            NetTotal.SpacingBefore = 4
            NetTotal.DefaultCell.Border = Rectangle.NO_BORDER
            For i = 0 To Total3.Length - 1
                phrase = New Phrase()
                phrase.Add(New Chunk(Total3(i), TableHeadFontBold))
                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                cell.SetLeading(12, 0)
                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                cell.PaddingBottom = 4.0F
                cell.PaddingTop = 1.0F
                If i = 0 Then
                    cell.BorderWidthRight = 0
                ElseIf i = Total3.Length - 1 Then
                    cell.BorderWidthLeft = 0
                Else
                    cell.BorderWidthRight = 0
                    cell.BorderWidthLeft = 0
                End If
                NetTotal.AddCell(cell)
            Next
            pdfdoc.Add(NetTotal)
        End If


    End Sub


#Region "Excel WithMovement"

    Public Sub WithioutMovementExcel(ByVal custdetailsdt As DataTable, ByVal type As String, ByVal curr As String, ByRef ws As IXLWorksheet, ByVal movflg As String)
        Dim col As Integer
        Dim Total() As String
        Dim co = custdetailsdt.Rows.Count

        Dim grpby = From custledger In custdetailsdt.AsEnumerable() Group custledger By g = New With {Key .currcode = custledger.Field(Of String)("currcode")} Into Group Order By g.currcode

        Dim currarr(grpby.Count) As String

        For Each row In grpby
            Dim count = 0
            Dim c As String = (row.g.currcode).ToString()

            currarr(count) = c
            count = count + 1
        Next
        'For Each rows In grpby
        '    custdetailsdt1 = rows.Group.CopyToDataTable

        For j = 0 To currarr.Length - 2
            ' Dim currs = currarr(j)
            ' dscurrecncy = custdetailsdt.Select("currcode='OMR'")
            dscurrecncy = custdetailsdt.Select("currcode='" & currarr(j) & "'")

            If dscurrecncy.Length > 0 Then
                custdetailsdtgpby = dscurrecncy.CopyToDataTable()
                GrandTotalv = "Total"
                If currarr.Length > 2 Then
                    GrandTotalv = "Grand Total"

                    Totaltdebitu = Convert.ToDouble(custdetailsdt.Compute("SUM(tdebit)", "currcode ='" & currarr(currarr.Length - 1) & "'"))
                    Totaltcreditu = Convert.ToDouble(custdetailsdt.Compute("SUM(tcredit)", "currcode ='" & currarr(currarr.Length - 1) & "'"))

                End If


                Totaltdebit = Convert.ToDouble(custdetailsdt.Compute("SUM(tdebit)", "currcode ='" & currarr(j) & "'"))
                Totaltcredit = Convert.ToDouble(custdetailsdt.Compute("SUM(tcredit)", "currcode ='" & currarr(j) & "'"))

                If type = "C" Then
                    Total = {"", "", "", "", currarr(j) & " Total", "", "", Totaltdebit.ToString(decimalPoint), Totaltcredit.ToString(decimalPoint), ""}

                    col = 4
                ElseIf type = "S" Then


                    Total = {"", "", currarr(j) & "Total", Totaltdebit.ToString(decimalPoint), Totaltcredit.ToString(decimalPoint)}
                    col = 2
                End If

                SubTotalWitoutExcel(type, custdetailsdtgpby, ws, movflg, Total.Length + 1)

                If curr <> "0" Then

                    rowcount = rowcount + 1

                    ws.Range(rowcount, 1, rowcount, Total.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold().Font.FontSize = 8
                    ws.Range(rowcount, 1, rowcount, Total.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                    ws.Range(rowcount, 1, rowcount, Total.Length).Style.Fill.BackgroundColor = XLColor.LightGray
                    '  ws.Range(rowcount, 1, rowcount, Total.Length).Style.Font.Bold = False

                    For i = 0 To Total.Length - 1
                        If i = col Then
                            ws.Cell(rowcount, i + 1).Value = Total(i)
                        ElseIf String.IsNullOrEmpty(Total(i)) Then
                            ws.Cell(rowcount, i + 1).Value = Total(i)
                        Else
                            ws.Cell(rowcount, i + 1).Value = Decimal.Parse(Total(i))
                            ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints

                        End If

                    Next

                End If


            End If
        Next
        Dim Total1() As String
        Dim Total3() As String
        Dim Total2() As String

        'dscurrecncy = custdetailsdt.Select("currcode='USD'")
        'If dscurrecncy.Length > 0 Then
        '    custdetailsdtgpby = dscurrecncy.CopyToDataTable()
        '    GrandTotalv = "Grand Total"
        '    Totaltdebitu = Convert.ToDouble(custdetailsdt.Compute("SUM(tdebit)", "currcode ='USD'"))
        '    Totaltcreditu = Convert.ToDouble(custdetailsdt.Compute("SUM(tcredit)", "currcode ='USD'"))


        '    If type = "C" Then
        '        col = 4
        '        Total1 = {"", "", "", "", "USD Total", "", "", Totaltdebitu.ToString(decimalPoint), Totaltcreditu.ToString(decimalPoint), ""}
        '    ElseIf type = "S" Then
        '        col = 2
        '        Total1 = {"", "", "USD Total", Totaltdebitu.ToString(decimalPoint), Totaltcreditu.ToString(decimalPoint)}
        '    End If



        '    SubTotalWitoutExcel(type, custdetailsdtgpby, ws, movflg, Total1.Length)
        '    rowcount = rowcount + 1

        '    ws.Range(rowcount, 1, rowcount, Total1.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold().Font.FontSize = 8
        '    ws.Range(rowcount, 1, rowcount, Total1.Length).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
        '    ws.Range(rowcount, 1, rowcount, Total1.Length).Style.Fill.BackgroundColor = XLColor.LightGray

        '    For i = 0 To Total1.Length - 1
        '        If i = col Then
        '            ws.Cell(rowcount, i + 1).Value = Total1(i)
        '        ElseIf String.IsNullOrEmpty(Total1(i)) Then
        '            ws.Cell(rowcount, i + 1).Value = Total1(i)
        '        Else
        '            ws.Cell(rowcount, i + 1).Value = Decimal.Parse(Total1(i))
        '            ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints

        '        End If
        '    Next


        'End If

        If type = "C" Then

            Total2 = {"", " ", "", "", GrandTotalv, "", (Totaltdebit + Totaltdebitu).ToString(decimalPoint), (Totaltcredit + Totaltcreditu).ToString(decimalPoint), ""}

            If movflg = "0" Then
                Total3 = {"", " ", "", "", "Net Final Total", "", "", "",
                          ((Totaltdebit + Totaltdebitu) - (Totaltcredit + Totaltcreditu)).ToString(decimalPoint)}
            Else
                Total3 = {"", " ", "", "", "Net Final Total", "", "",
                          ((Totaltdebit + Totaltdebitu) - (Totaltcredit + Totaltcreditu)).ToString(decimalPoint), ""}


            End If

            rowcount = rowcount + 1

            ws.Range(rowcount, 1, rowcount, Total.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold().Font.FontSize = 10
            ws.Range(rowcount, 1, rowcount, Total.Length).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
            ws.Range(rowcount, 1, rowcount, Total.Length).Style.Fill.BackgroundColor = XLColor.LightGray

            For i = 0 To Total2.Length - 1
                If i = 4 Then
                    ws.Cell(rowcount, i + 1).Value = Total2(i)
                    ws.Range(rowcount, i + 1, rowcount, i + 1).Merge()
                ElseIf i < 4 Then
                    ws.Cell(rowcount, i + 1).Value = Total2(i)
                Else
                    If String.IsNullOrEmpty(Total2(i)) Then
                        ws.Cell(rowcount, i + 1).Value = Total2(i)
                    Else
                        ws.Cell(rowcount, i + 1).Value = Decimal.Parse(Total2(i))
                        ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints
                    End If
                End If

            Next
            rowcount = rowcount + 1

            ws.Range(rowcount, 1, rowcount, Total.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold().Font.FontSize = 10
            ws.Range(rowcount, 1, rowcount, Total.Length).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
            ws.Range(rowcount, 1, rowcount, Total.Length).Style.Fill.BackgroundColor = XLColor.Gray

            For i = 0 To Total3.Length - 1
                If i = 4 Then
                    ws.Cell(rowcount, i + 0).Value = Total3(i)
                    ws.Range(rowcount, i + 0, rowcount, i + 1).Merge()
                ElseIf i < 4 Then
                    ws.Cell(rowcount, i + 1).Value = Total3(i)
                Else
                    If String.IsNullOrEmpty(Total3(i)) Then
                        ws.Cell(rowcount, i + 1).Value = Total3(i)
                    Else
                        ws.Cell(rowcount, i + 1).Value = Decimal.Parse(Total3(i))
                        ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints
                    End If
                End If


            Next


        ElseIf type = "S" Then
            Total2 = {"", " ", GrandTotalv, (Totaltdebit + Totaltdebitu).ToString(decimalPoint), (Totaltcredit + Totaltcreditu).ToString(decimalPoint)}

            Total3 = {"", "", "Net Final Total", "", ((Totaltcredit + Totaltcreditu) - (Totaltdebit + Totaltdebitu)).ToString(decimalPoint)}


            rowcount = rowcount + 1

            ws.Range(rowcount, 1, rowcount, Total2.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold().Font.FontSize = 10
            ws.Range(rowcount, 1, rowcount, Total2.Length).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
            ws.Range(rowcount, 1, rowcount, Total2.Length).Style.Fill.BackgroundColor = XLColor.LightGray
            ws.Range(rowcount, 1, rowcount, Total2.Length).Style.Alignment.WrapText = True
            For i = 0 To Total2.Length - 1


                If i = 2 Then
                    ws.Cell(rowcount, i + 1).Value = Total2(i)
                    ws.Range(rowcount, i + 1, rowcount, i + 0).Merge()
                ElseIf i < 2 Then
                    ws.Cell(rowcount, i + 1).Value = Total2(i)
                Else
                    If String.IsNullOrEmpty(Total2(i)) Then
                        ws.Cell(rowcount, i + 1).Value = Total2(i)
                    Else
                        ws.Cell(rowcount, i + 1).Value = Decimal.Parse(Total2(i))
                        ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints
                    End If
                End If

            Next

            rowcount = rowcount + 1

            ws.Range(rowcount, 1, rowcount, Total2.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold().Font.FontSize = 10
            ws.Range(rowcount, 1, rowcount, Total2.Length).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
            ws.Range(rowcount, 1, rowcount, Total2.Length).Style.Fill.BackgroundColor = XLColor.Gray
            ws.Range(rowcount, 1, rowcount, Total2.Length).Style.Alignment.WrapText = True

            For i = 0 To Total3.Length - 1
                If i = 2 Then
                    ws.Cell(rowcount, i + 0).Value = Total3(i)
                    ws.Range(rowcount, i + 0, rowcount, i + 1).Merge()
                ElseIf i < 2 Then
                    ws.Cell(rowcount, i + 1).Value = Total3(i)
                Else
                    If String.IsNullOrEmpty(Total3(i)) Then
                        ws.Cell(rowcount, i + 1).Value = Total3(i)
                    Else
                        ws.Cell(rowcount, i + 1).Value = Decimal.Parse(Total3(i))
                        ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints
                    End If
                End If

                ' NetTotal.AddCell(cell)
            Next
            ' pdfdoc.Add(NetTotal)
        End If


    End Sub


#End Region

    Public Sub SubTotalWitout(ByVal type As String, ByVal dscurrecncy As DataTable, ByRef table As PdfPTable, ByVal movflg As String)
        ' Dim groupbyrows As New DataTable

        Dim groupby, gptype As String

        Dim arrindex1, arrindex2 As Integer
        Dim arrData1() As String
        Dim arrData() As String
        Dim crlimit, credit, debit As Decimal
        If IsgroupBy = 0 Then
            ' Without Group By
            For Each row In dscurrecncy.Rows
                If type = "C" Then
                    crlimit = IIf((TypeOf row("crlimit") Is DBNull), 0.0, row("crlimit"))
                    arrData = {row("acc_code"), row("acc_name"), IIf((TypeOf row("tel") Is DBNull), "", row("tel")), IIf((TypeOf row("fax") Is DBNull), "", row("fax")), IIf(crlimit = 0.0, "-", crlimit.ToString(decimalPoint)), IIf((TypeOf row("catcode") Is DBNull), "", row("catcode")), IIf((TypeOf row("currcode") Is DBNull), "", row("currcode")), IIf((TypeOf row("tdebit") Is DBNull), "", Decimal.Parse(row("tdebit")).ToString(decimalPoint)), IIf((TypeOf row("tcredit") Is DBNull), "", Decimal.Parse(row("tcredit")).ToString(decimalPoint)), IIf((TypeOf row("remarks") Is DBNull), "", row("remarks"))}
                    arrindex1 = 6
                    arrindex2 = 9
                    table.SetWidths(New Single() {0.08F, 0.16F, 0.12F, 0.1F, 0.09F, 0.08F, 0.07F, 0.09F, 0.11F, 0.12F})
                ElseIf type = "S" Then
                    table.SetWidths(New Single() {0.16F, 0.34F, 0.14, 0.18F, 0.18F})
                    arrData = {row("acc_code"), row("acc_name"), IIf((TypeOf row("currcode") Is DBNull), "", row("currcode")), IIf((TypeOf row("tdebit") Is DBNull), "", Decimal.Parse(row("tdebit")).ToString(decimalPoint)), IIf((TypeOf row("tcredit") Is DBNull), "", Decimal.Parse(row("tcredit")).ToString(decimalPoint))}
                    arrindex1 = 2
                    arrindex2 = 5
                End If
                table.TotalWidth = documentWidth
                table.LockedWidth = True
                table.Complete = False
                table.SplitRows = False
                table.SpacingBefore = 0
                For i = 0 To arrData.Length - 1
                    phrase = New Phrase()
                    phrase.Add(New Chunk(arrData(i), NormalFont))

                    If i > arrindex1 And i < arrindex2 Then
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                    ElseIf i = 2 Then
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    Else
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    End If
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4.0F
                    cell.PaddingTop = 1.0F
                    table.AddCell(cell)
                Next
            Next
        Else

            'with Group by
            If IsgroupBy = 1 Then
                If type = "C" Then
                    groupby = "Marketname"
                    gptype = "Market Name : "
                ElseIf type = "S" Then
                    gptype = "SP Type Name:"
                    groupby = "sptypename"
                End If

            ElseIf IsgroupBy = 2 Then
                groupby = "catname"
                gptype = "Category Name : "
            ElseIf IsgroupBy = 3 Then
                groupby = "glname"
                gptype = "Control A/C Name : "
            End If

            Dim groups As IEnumerable(Of IGrouping(Of String, DataRow)) = dscurrecncy.AsEnumerable.GroupBy(Function(g) g.Field(Of String)(groupby)).OrderBy(Function(o) o.Key)

            For Each ls In groups
                stcredit = 0.0
                stdebit = 0.0
                phrase = New Phrase()
                If type = "C" Or movflg = "1" Then
                    phrase.Add(New Chunk(gptype & ls.Key, NormalFontBold))
                Else
                    phrase.Add(New Chunk(ls.Key, NormalFontBold))
                End If
                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                cell.SetLeading(12, 0)
                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                cell.PaddingBottom = 4.0F
                cell.PaddingTop = 1.0F
                cell.BackgroundColor = groupbg
                cell.Colspan = 10
                table.AddCell(cell)

                For Each row As DataRow In ls

                    credit = IIf((TypeOf row("tcredit") Is DBNull), "", Decimal.Parse(row("tcredit")))
                    debit = IIf((TypeOf row("tdebit") Is DBNull), 0.0, Decimal.Parse(row("tdebit")))
                    stdebit = stdebit + debit
                    stcredit = stcredit + credit
                    If type = "C" Then
                        crlimit = IIf((TypeOf row("crlimit") Is DBNull), 0.0, row("crlimit"))
                        arrData = {row("acc_code"), row("acc_name"), IIf((TypeOf row("tel") Is DBNull), "", row("tel")), IIf((TypeOf row("fax") Is DBNull), "", row("fax")), IIf(crlimit = 0.0, "-", crlimit.ToString(decimalPoint)), IIf((TypeOf row("catcode") Is DBNull), "", row("catcode")), IIf((TypeOf row("currcode") Is DBNull), "", row("currcode")), debit.ToString(decimalPoint), credit.ToString(decimalPoint), IIf((TypeOf row("remarks") Is DBNull), "", row("remarks"))}
                        arrindex1 = 6
                        arrindex2 = 9
                        table.SetWidths(New Single() {0.08F, 0.16F, 0.12F, 0.1F, 0.09F, 0.08F, 0.07F, 0.09F, 0.11F, 0.12F})
                        arrData1 = {" ", "", "", "", "", "Sub Total", " ", stdebit.ToString(decimalPoint), stcredit.ToString(decimalPoint), ""}
                    ElseIf type = "S" Then
                        table.SetWidths(New Single() {0.16F, 0.34F, 0.14, 0.18F, 0.18F})
                        arrData = {row("acc_code"), row("acc_name"), IIf((TypeOf row("currcode") Is DBNull), "", row("currcode")), debit.ToString(decimalPoint), credit.ToString(decimalPoint)}
                        arrindex1 = 2
                        arrindex2 = 5
                        arrData1 = {" ", "", "Sub Total", stdebit.ToString(decimalPoint), stcredit.ToString(decimalPoint)}
                    End If
                    table.TotalWidth = documentWidth
                    table.LockedWidth = True
                    table.Complete = False
                    table.SplitRows = False
                    table.SpacingBefore = 0
                    For i = 0 To arrData.Length - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrData(i), NormalFont))

                        If i > arrindex1 And i < arrindex2 Then
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT

                        Else
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        End If

                        cell.SetLeading(12, 0)
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 1.0F
                        table.AddCell(cell)
                    Next

                Next
                For i = 0 To arrData1.Length - 1
                    phrase = New Phrase()
                    phrase.Add(New Chunk(arrData1(i), NormalFontBold))
                    cell1 = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                    cell1.SetLeading(12, 0)
                    cell1.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                    cell1.PaddingBottom = 4.0F
                    cell1.PaddingTop = 1.0F
                    cell1.BackgroundColor = Rowtitlebg
                    table.AddCell(cell1)
                Next

            Next
        End If
    End Sub

    Public Sub SubTotalWitoutExcel(ByVal type As String, ByVal dscurrecncy As DataTable, ByRef ws As IXLWorksheet, ByVal movflg As String, ByVal arrlength As Integer)
        ' Dim groupbyrows As New DataTable

        Dim groupby, gptype As String

        Dim arrindex1, arrindex2 As Integer
        Dim arrData1() As String
        Dim arrData() As String
        Dim crlimit, credit, debit As Decimal
        If IsgroupBy = 0 Then
            ' Without Group By

            For Each row In dscurrecncy.Rows

                rowcount = rowcount + 1
                ws.Range(rowcount, 1, rowcount, arrlength - 1).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Font.FontSize = 9
                ws.Range(rowcount, 1, rowcount, arrlength - 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.WrapText = True
                ws.Range(rowcount, 1, rowcount, arrlength - 1).Style.Alignment.WrapText = True
                If type = "C" Then
                    crlimit = IIf((TypeOf row("crlimit") Is DBNull), 0.0, row("crlimit"))
                    arrData = {(row("acc_code") & Space(1) & ".").ToString(), row("acc_name"), IIf((TypeOf row("tel") Is DBNull), "", row("tel")), IIf((TypeOf row("fax") Is DBNull), "", row("fax")), IIf(crlimit = 0.0, "-", crlimit.ToString(decimalPoint)), IIf((TypeOf row("catcode") Is DBNull), "", row("catcode")), IIf((TypeOf row("currcode") Is DBNull), "", row("currcode")), IIf((TypeOf row("tdebit") Is DBNull), "", Decimal.Parse(row("tdebit")).ToString(decimalPoint)), IIf((TypeOf row("tcredit") Is DBNull), "", Decimal.Parse(row("tcredit")).ToString(decimalPoint)), IIf((TypeOf row("remarks") Is DBNull), "", row("remarks"))}


                    For i = 0 To arrData.Length - 1
                        ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        If i = 4 Then
                            ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                            If crlimit = 0 Then
                                ws.Cell(rowcount, i + 1).Value = arrData(i)
                                ' ws.Cell(rowcount, i + 1).SetDataType(XLCellValues.Text)
                            Else
                                ws.Cell(rowcount, i + 1).Value = Decimal.Parse(arrData(i))
                                ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints
                            End If
                        ElseIf i = 0 Then
                            ws.Cell(rowcount, i + 1).Value = arrData(i)
                            'ws.Cell(rowcount, i + 1).Style.NumberFormat.NumberFormatId = 1
                        ElseIf i = 7 Or i = 8 Then
                            ws.Cell(rowcount, i + 1).Value = Decimal.Parse(arrData(i))
                            ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints
                            ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        Else
                            If i = 6 Or i = 5 Then
                                ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                            End If
                            ws.Cell(rowcount, i + 1).Value = arrData(i)
                        End If

                    Next

                ElseIf type = "S" Then

                    arrData = {(row("acc_code") & Space(1) & ".").ToString(), row("acc_name"), IIf((TypeOf row("currcode") Is DBNull), "", row("currcode")), IIf((TypeOf row("tdebit") Is DBNull), "", Decimal.Parse(row("tdebit")).ToString(decimalPoint)), IIf((TypeOf row("tcredit") Is DBNull), "", Decimal.Parse(row("tcredit")).ToString(decimalPoint))}
                    arrindex1 = 2
                    arrindex2 = 5

                    ws.Range(rowcount, 1, rowcount, arrData.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
                    ws.Range(rowcount, 1, rowcount, arrData.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                    ws.Range(rowcount, 1, rowcount, arrData.Length).Style.Alignment.WrapText = True

                    For i = 0 To arrData.Length - 1
                        ws.Cell(rowcount, i + 1).Style.Font.Bold = False
                        If i = 3 Or i = 4 Then

                            ws.Cell(rowcount, i + 1).Value = Decimal.Parse(arrData(i))
                            ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints
                            ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ElseIf i = 0 Then
                            ws.Cell(rowcount, i + 1).Value = arrData(i)
                            ws.Cell(rowcount, i + 1).SetDataType(XLCellValues.Text)
                            ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        ElseIf i = 2 Then
                            ws.Cell(rowcount, i + 1).Value = arrData(i)
                            ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)

                        Else

                            ws.Cell(rowcount, i + 1).Value = arrData(i)
                            ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        End If

                    Next
                End If

            Next
        Else

            'with Group by
            If IsgroupBy = 1 Then
                If type = "C" Then
                    groupby = "Marketname"
                    gptype = "Market Name : "
                ElseIf type = "S" Then
                    gptype = "SP Type Name:"
                    groupby = "sptypename"
                End If

            ElseIf IsgroupBy = 2 Then
                groupby = "catname"
                gptype = "Category Name : "
            ElseIf IsgroupBy = 3 Then
                groupby = "glname"
                gptype = "Control A/C Name : "
            End If

            Dim groups As IEnumerable(Of IGrouping(Of String, DataRow)) = dscurrecncy.AsEnumerable.GroupBy(Function(g) g.Field(Of String)(groupby)).OrderBy(Function(o) o.Key)

            For Each ls In groups
                stcredit = 0.0
                stdebit = 0.0
                phrase = New Phrase()

                rowcount = rowcount + 1
                ws.Range(rowcount, 1, rowcount, arrlength - 1).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Font.SetBold().Font.FontSize = 9
                ws.Range(rowcount, 1, rowcount, arrlength - 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                ws.Range(rowcount, 1, rowcount, arrlength - 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#ffccff")
                ws.Range(rowcount, 1, rowcount, arrlength - 1).Style.Alignment.WrapText = True
                If type = "C" Or movflg = "1" Then
                    'phrase.Add(New Chunk(gptype & ls.Key, NormalFontBold))
                    ws.Cell(rowcount, 1).Value = gptype & ls.Key

                Else
                    ws.Cell(rowcount, 1).Value = ls.Key
                    'phrase.Add(New Chunk(ls.Key, NormalFontBold))
                End If


                For Each row As DataRow In ls

                    credit = IIf((TypeOf row("tcredit") Is DBNull), "", Decimal.Parse(row("tcredit")))
                    debit = IIf((TypeOf row("tdebit") Is DBNull), 0.0, Decimal.Parse(row("tdebit")))
                    stdebit = stdebit + debit
                    stcredit = stcredit + credit

                    rowcount = rowcount + 1

                    If type = "C" Then
                        crlimit = IIf((TypeOf row("crlimit") Is DBNull), 0.0, row("crlimit"))
                        arrData = {(Space(1) & row("acc_code") & Space(1)).ToString(), row("acc_name"), IIf((TypeOf row("tel") Is DBNull), "", row("tel")), IIf((TypeOf row("fax") Is DBNull), "", row("fax")), IIf(crlimit = 0.0, "-", crlimit.ToString(decimalPoint)), IIf((TypeOf row("catcode") Is DBNull), "", row("catcode")), IIf((TypeOf row("currcode") Is DBNull), "", row("currcode")), debit.ToString(decimalPoint), credit.ToString(decimalPoint), IIf((TypeOf row("remarks") Is DBNull), "", row("remarks"))}

                        ws.Range(rowcount, 1, rowcount, arrData.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
                        ws.Range(rowcount, 1, rowcount, arrData.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                        ws.Range(rowcount, 1, rowcount, arrData.Length).Style.Alignment.WrapText = True
                        arrData1 = {"", "", "", "", "", "Sub Total", "", stdebit.ToString(decimalPoint), stcredit.ToString(decimalPoint), ""}
                        arrindex1 = 5
                        For i = 0 To arrData.Length - 1
                            ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            If i = 4 Then
                                ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                If crlimit = 0 Then
                                    ws.Cell(rowcount, i + 1).Value = arrData(i)
                                    ws.Cell(rowcount, i + 1).SetDataType(XLCellValues.Text)
                                Else
                                    ws.Cell(rowcount, i + 1).Value = Decimal.Parse(arrData(i))
                                    ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints
                                End If
                            ElseIf i = 0 Then
                                ws.Cell(rowcount, i + 1).Value = Decimal.Parse(arrData(i))
                                ws.Cell(rowcount, i + 1).Style.NumberFormat.NumberFormatId = 1

                            ElseIf i = 7 Or i = 8 Then
                                ws.Cell(rowcount, i + 1).Value = Decimal.Parse(arrData(i))
                                ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints
                                ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                            Else
                                If i = 6 Then
                                    ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                End If
                                ws.Cell(rowcount, i + 1).Value = arrData(i)
                            End If

                        Next
                    ElseIf type = "S" Then

                        arrData = {(row("acc_code") & Space(1) & ".").ToString(), row("acc_name"), IIf((TypeOf row("currcode") Is DBNull), "", row("currcode")), debit.ToString(decimalPoint), credit.ToString(decimalPoint)}

                        ws.Range(rowcount, 1, rowcount, arrData.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
                        ws.Range(rowcount, 1, rowcount, arrData.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                        ws.Range(rowcount, 1, rowcount, arrData.Length).Style.Alignment.WrapText = True
                        For i = 0 To arrData.Length - 1
                            ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)

                            If i = 3 Or i = 4 Then
                                ws.Cell(rowcount, i + 1).Value = Decimal.Parse(arrData(i))
                                ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints
                                ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                            ElseIf i = 2 Then
                                ws.Cell(rowcount, i + 1).Value = arrData(i)
                                ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                            ElseIf i = 0 Then
                                ws.Cell(rowcount, i + 1).Value = arrData(i)
                                ws.Cell(rowcount, i + 1).SetDataType(XLCellValues.Text)
                            Else
                                ws.Cell(rowcount, i + 1).Value = arrData(i)
                                '  ws.Cell(rowcount, i + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            End If

                        Next

                        arrData1 = {"", "", "Sub Total", stdebit.ToString(decimalPoint), stcredit.ToString(decimalPoint)}
                        arrindex1 = 2
                    End If


                Next

                rowcount = rowcount + 1
                ws.Range(rowcount, 1, rowcount, arrData1.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetBold().Font.FontSize = 9
                ws.Range(rowcount, 1, rowcount, arrData1.Length).Style.Fill.SetBackgroundColor(XLColor.LightGray).Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                ws.Range(rowcount, 1, rowcount, arrData1.Length).Style.Alignment.WrapText = True
                For i = 0 To arrData1.Length - 1
                    If i = arrindex1 Then
                        ws.Cell(rowcount, i + 1).Value = arrData1(i)

                    ElseIf String.IsNullOrEmpty(arrData1(i)) Then
                        ws.Cell(rowcount, i + 1).Value = arrData1(i)
                    Else
                        ws.Cell(rowcount, i + 1).Value = Decimal.Parse(arrData1(i))
                        ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoints
                    End If
                Next

            Next
        End If
    End Sub


    Public Sub CustTrialBal(ByVal reportsType As String, ByVal fromdate As String, ByVal todate As String, ByVal fromctry As String, ByVal toctry As String, ByVal movflg As String, ByVal fromcode As String, ByVal tocode As String, ByVal frommarketcode As String, ByVal tomarketcode As String, ByVal fromglcode As String, ByVal toglcode As String,
                                       ByVal orderby As String, ByVal curr As String, ByVal includezero As String, ByVal gpby As String, ByVal withcredit As String, ByVal divcode As String, ByVal trialtype As String, ByVal type As String, ByVal fromcat As String, ByVal tocat As String, ByVal custgroup_sp_type As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "")

        IsgroupBy = gpby

        If divcode <> "" Then
            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
        Else
            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
        End If

        addrLine1 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1063)
        addrLine2 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1064)
        addrLine3 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1065)
        addrLine4 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1066)
        addrLine5 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1053)

        If type = "C" Then
            If trialtype = "TB" Then
                rptreportname = IIf(Trim(withcredit) = 1, "Customers With Credit Balance", "Customer Trial Balance")
            End If

        End If

        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            Dim dscurrecncy() As System.Data.DataRow
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("sp_customer_trialbal", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure


            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@movflg", SqlDbType.Int)).Value = Convert.ToInt32(movflg)
            mySqlCmd.Parameters.Add(New SqlParameter("@fromcode", SqlDbType.VarChar, 20)).Value = fromcode
            mySqlCmd.Parameters.Add(New SqlParameter("@tocode", SqlDbType.VarChar, 20)).Value = tocode
            mySqlCmd.Parameters.Add(New SqlParameter("@frommarkcode", SqlDbType.VarChar, 20)).Value = frommarketcode
            mySqlCmd.Parameters.Add(New SqlParameter("@tomarkcode", SqlDbType.VarChar, 20)).Value = tomarketcode
            mySqlCmd.Parameters.Add(New SqlParameter("@fromctry", SqlDbType.VarChar, 20)).Value = fromctry
            mySqlCmd.Parameters.Add(New SqlParameter("@toctry", SqlDbType.VarChar, 20)).Value = toctry
            mySqlCmd.Parameters.Add(New SqlParameter("@fromcat", SqlDbType.VarChar, 20)).Value = fromcat
            mySqlCmd.Parameters.Add(New SqlParameter("@tocat", SqlDbType.VarChar, 20)).Value = tocat

            mySqlCmd.Parameters.Add(New SqlParameter("@fromglcode", SqlDbType.VarChar, 20)).Value = fromglcode
            mySqlCmd.Parameters.Add(New SqlParameter("@toglcode", SqlDbType.VarChar, 20)).Value = toglcode

            mySqlCmd.Parameters.Add(New SqlParameter("@currtype", SqlDbType.Int)).Value = Convert.ToInt32(curr)
            mySqlCmd.Parameters.Add(New SqlParameter("@orderby", SqlDbType.Int)).Value = Convert.ToInt32(orderby)
            mySqlCmd.Parameters.Add(New SqlParameter("@includezero", SqlDbType.Int)).Value = includezero

            mySqlCmd.Parameters.Add(New SqlParameter("@withcredit", SqlDbType.Int)).Value = Convert.ToInt32(withcredit)
            mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
            mySqlCmd.Parameters.Add(New SqlParameter("@custgroup_sp_type", SqlDbType.VarChar, 20)).Value = custgroup_sp_type


            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim custdetailsdt As New DataTable
            Dim custdetailsdtgpby As New DataTable
            custdetailsdt = ds.Tables(0)
            If custdetailsdt.Rows.Count > 0 Then
                FontFactory.RegisterDirectories()
                Dim pdfdoc As Document = New Document(PageSize.A4, -10.0F, 10.0F, 30.0F, 35.0F)
                pdfdoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)

                Using memoryStream As New System.IO.MemoryStream()
                    Dim writer As PdfWriter = PdfWriter.GetInstance(pdfdoc, memoryStream)
                    Dim titletable As PdfPTable = Nothing

                    Dim CompanybgColor As BaseColor = New BaseColor(0, 72, 192)
                    Dim ReportNamebgColor As BaseColor = New BaseColor(0, 128, 192)

                    titletable = New PdfPTable(1)
                    titletable.TotalWidth = documentWidth
                    titletable.LockedWidth = True
                    titletable.SetWidths(New Single() {1.0F})

                    titletable.Complete = False
                    titletable.SplitRows = False
                    'company name
                    phrase = New Phrase()
                    phrase.Add(New Chunk(rptcompanyname, CompanyFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    cell.BackgroundColor = CompanybgColor
                    titletable.AddCell(cell)
                    ' pdfdoc.Add(titletable)
                    'Report name

                    Dim Reporttitle = New PdfPTable(1)
                    Reporttitle.TotalWidth = documentWidth
                    Reporttitle.LockedWidth = True
                    Reporttitle.SetWidths(New Single() {1.0F})
                    Reporttitle.Complete = False
                    Reporttitle.SplitRows = False
                    phrase = New Phrase()
                    phrase.Add(New Chunk(rptreportname, ReportNameFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.PaddingBottom = 4
                    cell.SetLeading(12, 0)
                    cell.BackgroundColor = ReportNamebgColor
                    '  cell.PaddingBottom = 

                    Reporttitle.SpacingBefore = 5
                    Reporttitle.SpacingAfter = 5
                    Reporttitle.AddCell(cell)
                    currtype = IIf(curr <> "0", "(In Party Currency)", "(In Base Currency)")
                    head = IIf(withcredit = "0" Or True, "Report- Customer Trial Balance -Transaction : From  " & Format(Convert.ToDateTime(fromdate), "dd-MM-yyyy") & "  To" & " " & Format(Convert.ToDateTime(todate), "dd-MM-yyyy") & " " & currtype, "Report- Customers With Credit Balance -Transaction : From" & fromdate & "To" & " " & todate & " " & currtype)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(head, HeadFontBold))
                    cell = New PdfPCell(phrase)
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 3
                    Reporttitle.AddCell(cell)

                    Dim FooterTable = New PdfPTable(1)

                    FooterTable.TotalWidth = documentWidth
                    FooterTable.LockedWidth = True
                    FooterTable.SetWidths(New Single() {1.0F})
                    FooterTable.Complete = False
                    FooterTable.SplitRows = False

                    phrase = New Phrase()
                    phrase.Add(New Chunk(addrLine3 + " " + addrLine5, NormalFont))
                    cell = New PdfPCell(phrase)
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1
                    FooterTable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(addrLine1 + Space(138) + addrLine2 + "  " + addrLine4, NormalFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 1

                    FooterTable.AddCell(cell)
                    ' DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Printed Date:" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss"), NormalFont))
                    cell = New PdfPCell(phrase)
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 3
                    FooterTable.AddCell(cell)

                    FooterTable.Complete = True
                    Dim tableTitle = New PdfPTable(5)
                    Dim arrHeaders() As String = {"Customer Code", "Customer Name", "Currency", "Balance", "Comments"}

                    For i = 0 To arrHeaders.Length - 1
                        tableTitle.TotalWidth = documentWidth
                        'tableTitle.HorizontalAlignment = Element.ALIGN_LEFT
                        tableTitle.LockedWidth = True
                        tableTitle.SetWidths(New Single() {0.12F, 0.3F, 0.1F, 0.18F, 0.3F})
                        tableTitle.SplitRows = False
                        tableTitle.KeepTogether = True
                        tableTitle.SpacingBefore = 20
                        tableTitle.SpacingAfter = 0
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrHeaders(i), TableHeadFontBold))

                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                        cell.SetLeading(12, 0)
                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 1.0F
                        ' cell.Colspan = 6

                        'If i <= 2 Then
                        '    cell.BorderWidthBottom = 0
                        'Else
                        '    cell.Colspan = 2
                        'End If
                        tableTitle.AddCell(cell)
                        'tableTitle.HeaderRows = 1
                    Next

                    writer.PageEvent = New ClsHeaderFooter(titletable, Reporttitle, FooterTable, tableTitle)

                    pdfdoc.Open()
                    Dim tableData = New PdfPTable(5)
                    tableData.SetWidths(New Single() {0.12F, 0.3F, 0.1F, 0.18F, 0.3F})
                    tableData.TotalWidth = documentWidth
                    tableData.LockedWidth = True
                    tableData.Complete = False
                    tableData.SplitRows = False
                    tableData.SpacingBefore = 0.0F

                    Totaltdebit = Convert.ToDouble(custdetailsdt.Compute("SUM(tdebit)", ""))
                    Totaltcredit = Convert.ToDouble(custdetailsdt.Compute("SUM(tcredit)", ""))

                    'without Group By
                    If IsgroupBy = 0 Then
                        For Each row In custdetailsdt.Rows
                            Dim arrData() As String = {row("acc_code"), row("acc_name"), IIf((TypeOf row("currcode") Is DBNull), "", row("currcode")), Decimal.Parse(row("tdebit") - row("tcredit")), IIf((TypeOf row("comment") Is DBNull), "", row("comment"))}
                            For i = 0 To arrData.Length - 1
                                phrase = New Phrase()
                                phrase.Add(New Chunk(arrData(i), NormalFont))

                                If i = 3 Then
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT

                                Else
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                End If

                                cell.SetLeading(12, 0)
                                cell.PaddingBottom = 4.0F
                                cell.PaddingTop = 1.0F
                                tableData.AddCell(cell)
                            Next
                        Next

                    Else
                        Dim groupby, gptype As String
                        'with Group by
                        If IsgroupBy = 1 Then
                            groupby = "Marketname"
                            gptype = "Market Name : "
                        ElseIf IsgroupBy = 2 Then
                            groupby = "catname"
                            gptype = "Category Name : "
                        ElseIf IsgroupBy = 3 Then
                            groupby = "glname"
                            gptype = "Control A/C Name : "
                        End If

                        Dim groups As IEnumerable(Of IGrouping(Of String, DataRow)) = custdetailsdt.AsEnumerable.GroupBy(Function(g) g.Field(Of String)(groupby)).OrderBy(Function(o) o.Key)

                        For Each ls In groups

                            stcredit = 0.0
                            stdebit = 0.0
                            phrase = New Phrase()
                            phrase.Add(New Chunk(gptype & ls.Key, NormalFontBold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                            cell.SetLeading(12, 0)
                            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BackgroundColor = groupbg
                            cell.Colspan = 10
                            tableData.AddCell(cell)

                            For Each row As DataRow In ls

                                'Dim c = row.Field(Of String)("acc_code")
                                Dim arrData() As String = {row("acc_code"), row("acc_name"), IIf((TypeOf row("currcode") Is DBNull), "", row("currcode")), Decimal.Parse(row("tdebit") - row("tcredit")), IIf((TypeOf row("comment") Is DBNull), "", row("comment"))}

                                'Dim arrData() As String = {dscurrecncy(j)("acc_code"), (dscurrecncy(j)("acc_name")), (Decimal.Parse(dscurrecncy(j)("currcode"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("odebit"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("ocredit"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("cdebit"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("ccredit"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("tdebit"))).ToString(decimalPoint), (Decimal.Parse(dscurrecncy(j)("tcredit"))).ToString(decimalPoint)}
                                For i = 0 To arrData.Length - 1
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(arrData(i), NormalFont))

                                    If i = 3 Then
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT

                                    Else
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                    End If

                                    cell.SetLeading(12, 0)
                                    cell.PaddingBottom = 4.0F
                                    cell.PaddingTop = 1.0F
                                    tableData.AddCell(cell)
                                Next
                                stdebit = stdebit + arrData(3)
                                ' stcredit = stcredit + arrData(8)

                            Next

                            Dim arrData1() As String = {"", "Sub Total", "", "$" & (stdebit.ToString(decimalPoint)), ""}

                            For i = 0 To arrData1.Length - 1
                                phrase = New Phrase()
                                phrase.Add(New Chunk(arrData1(i), NormalFontBold))
                                cell1 = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                cell1.SetLeading(12, 0)
                                cell1.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                cell1.PaddingBottom = 4.0F
                                cell1.PaddingTop = 1.0F
                                cell1.BackgroundColor = Rowtitlebg
                                tableData.AddCell(cell1)
                            Next

                        Next

                    End If
                    pdfdoc.Add(tableData)
                    Dim GrandTotal = New PdfPTable(3)
                    GrandTotal.SetWidths(New Single() {0.45F, 0.25, 0.3F})
                    GrandTotal.TotalWidth = documentWidth
                    GrandTotal.LockedWidth = True
                    GrandTotal.Complete = False
                    GrandTotal.SplitRows = False
                    GrandTotal.SpacingBefore = 4
                    GrandTotal.DefaultCell.Border = Rectangle.NO_BORDER


                    Dim Total2() As String = {"Grand Total", (Totaltdebit - Totaltcredit).ToString(decimalPoint), ""}
                    For i = 0 To Total2.Length - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Total2(i), TableHeadFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.SetLeading(12, 0)
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                        If i = 0 Then
                            cell.BorderWidthRight = 0
                        ElseIf i = Total2.Length - 1 Then
                            cell.BorderWidthLeft = 0
                        Else
                            cell.BorderWidthRight = 0
                            cell.BorderWidthLeft = 0
                        End If
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 1.0F
                        cell.FixedHeight = 20
                        cell.BackgroundColor = grandtotalbg
                        GrandTotal.AddCell(cell)
                    Next

                    pdfdoc.Add(GrandTotal)

                    Dim NetTotal = New PdfPTable(3)
                    NetTotal.SetWidths(New Single() {0.45F, 0.25, 0.3F})
                    NetTotal.TotalWidth = documentWidth
                    NetTotal.LockedWidth = True
                    NetTotal.Complete = False
                    NetTotal.SplitRows = False
                    NetTotal.SpacingBefore = 4
                    NetTotal.DefaultCell.Border = Rectangle.NO_BORDER

                    Dim Total3() As String = {"Net Final Total", (Totaltdebit - Totaltcredit).ToString(decimalPoint), ""}
                    For i = 0 To Total3.Length - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(Total3(i), TableHeadFontBold))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                        cell.SetLeading(12, 0)
                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 1.0F
                        If i = 0 Then
                            cell.BorderWidthRight = 0
                        ElseIf i = Total3.Length - 1 Then
                            cell.BorderWidthLeft = 0
                        Else
                            cell.BorderWidthRight = 0
                            cell.BorderWidthLeft = 0
                        End If

                        NetTotal.AddCell(cell)
                    Next

                    pdfdoc.Add(NetTotal)

                    pdfdoc.AddTitle(rptreportname)
                    pdfdoc.Close()

                    If printMode = "download" Then
                        Dim pagingFont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)
                        Dim reader As New PdfReader(memoryStream.ToArray())
                        Using mStream As New System.IO.MemoryStream()
                            Using stamper As New PdfStamper(reader, mStream)
                                Dim pages As Integer = reader.NumberOfPages
                                For i As Integer = 1 To pages
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 750.0F, 5.0F, 0)
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

End Class
