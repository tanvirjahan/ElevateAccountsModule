Imports Microsoft.VisualBasic
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic
Imports ClosedXML.Excel
Imports System.IO

Public Class CustomerLedgerPdf
    Inherits System.Web.UI.Page

#Region "global declaration"
    Dim objutils As New clsUtils
    Dim led, rptcompanyname, reportfilter, acccode, glcode, accname, rptreportname, spName, report, arrData(), arrData1(), arrHeader(), balance, addrLine1, addrLine2, addrLine3, addrLine4, addrLine5, cmb As String
    Dim count, pdcyes, count2, rcount, newp, currDecno As Integer
    Dim totaldebit, debit, credit, totalcredit, ftotaldebit, ftotalcredit, fbal As Decimal
    Dim FilterFont As Font = New Font(FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK))
    Dim NormalFont As Font = New Font(FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK))
    Dim FooterFont As Font = New Font(FontFactory.GetFont("Arial", 7, Font.NORMAL, BaseColor.BLACK))
    Dim NormalFontBold As Font = New Font(FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK))
    Dim CompanyFont As Font = New Font(FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.WHITE))
    Dim ReportNameFont As Font = New Font(FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.WHITE))
    Dim titleColor As BaseColor = New BaseColor(0, 72, 92)
    'Dim pdfdoc As Document = New Document(PageSize.A4, 23.0F, 23.0F, 30.0F, 35.0F)
    Dim pdfdoc As Document = New Document(PageSize.A4, 23.0F, 23.0F, 30.0F, 41.0F)
    Dim documentWidth As Single
#End Region


    Public Sub CreatePdf(ByVal fromdate As String, ByVal todate As String, ByVal rptfiter As String, ByVal reportname As String, ByVal reportsType As String, ByVal actype As String, ByVal type As String, ByVal fromctry As String, ByVal toctry As String, ByVal frommarketcode As String, ByVal tomarketcode As String, ByVal fromcode As String, ByVal tocode As String, ByVal fromcat As String, ByVal tocat As String, ByVal fromglcode As String, ByVal toglcode As String,
                                     ByVal pdcyesno As String, ByVal currtype As String, ByVal ledgertype As String, ByVal divcode As String, ByVal ageing As String, ByVal ststement As String, ByVal custgroup_sp_type As String, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal printMode As String, Optional ByVal fileName As String = "")

        Dim tableData1 = New PdfPTable(8)
        Dim tableTitle1 = New PdfPTable(8)
        Dim tableData = New PdfPTable(8)
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

        pdcyes = Integer.Parse(pdcyesno)
        spName = IIf(actype = "G", "sp_rep_general_ledger", "sp_party_ledger")
        Try
            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet

            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand(spName, sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0 'Tanvir 04052023
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")
            If actype = "G" Then
                mySqlCmd.Parameters.Add(New SqlParameter("@frmdate", SqlDbType.VarChar, 10)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")
                mySqlCmd.Parameters.Add(New SqlParameter("@reptype", SqlDbType.Int)).Value = Integer.Parse(ledgertype)
                mySqlCmd.Parameters.Add(New SqlParameter("@pdctype", SqlDbType.Int)).Value = pdcyes
                mySqlCmd.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 20)).Value = divcode
                mySqlCmd.Parameters.Add(New SqlParameter("@actype", SqlDbType.Char)).Value = actype
                mySqlCmd.Parameters.Add(New SqlParameter("@frmac", SqlDbType.VarChar, 20)).Value = fromcode
                mySqlCmd.Parameters.Add(New SqlParameter("@toac", SqlDbType.VarChar, 20)).Value = tocode
            Else
                mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")
                mySqlCmd.Parameters.Add(New SqlParameter("@type", SqlDbType.Char)).Value = actype
                mySqlCmd.Parameters.Add(New SqlParameter("@currflg", SqlDbType.Int)).Value = currtype
                mySqlCmd.Parameters.Add(New SqlParameter("@fromacct", SqlDbType.VarChar, 20)).Value = fromcode
                mySqlCmd.Parameters.Add(New SqlParameter("@toacct", SqlDbType.VarChar, 20)).Value = tocode
                mySqlCmd.Parameters.Add(New SqlParameter("@fromcontrol", SqlDbType.VarChar, 20)).Value = fromglcode
                mySqlCmd.Parameters.Add(New SqlParameter("@tocontrol", SqlDbType.VarChar, 20)).Value = toglcode
                mySqlCmd.Parameters.Add(New SqlParameter("@fromcat", SqlDbType.VarChar, 20)).Value = fromcat
                mySqlCmd.Parameters.Add(New SqlParameter("@tocat", SqlDbType.VarChar, 20)).Value = tocat
                mySqlCmd.Parameters.Add(New SqlParameter("@fromcity", SqlDbType.VarChar, 20)).Value = frommarketcode
                mySqlCmd.Parameters.Add(New SqlParameter("@tocity", SqlDbType.VarChar, 20)).Value = tomarketcode
                mySqlCmd.Parameters.Add(New SqlParameter("@fromctry", SqlDbType.VarChar, 20)).Value = fromctry
                mySqlCmd.Parameters.Add(New SqlParameter("@toctry", SqlDbType.VarChar, 20)).Value = toctry
                mySqlCmd.Parameters.Add(New SqlParameter("@ledgertype", SqlDbType.Int)).Value = ledgertype
                mySqlCmd.Parameters.Add(New SqlParameter("@pdcyesno", SqlDbType.Int)).Value = pdcyes
                mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
                mySqlCmd.Parameters.Add(New SqlParameter("@custgroup_sp_type", SqlDbType.VarChar, 20)).Value = custgroup_sp_type

            End If

            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim custdetailsdt As New DataTable
            custdetailsdt = ds.Tables(0)
            rptreportname = reportname
            reportfilter = rptfiter
            'If Not (String.IsNullOrEmpty(fromdate)) And Not (String.IsNullOrEmpty(todate)) Then
            '    reportfilter = "From" & Space(2) & Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy") & Space(2) & "To" & Space(2) & Convert.ToDateTime(todate).ToString("dd/MM/yyyy")
            'End If
            'If Not (String.IsNullOrEmpty(fromcode)) And Not (String.IsNullOrEmpty(tocode)) Then
            '    reportfilter = reportfilter & Space(2) & IIf(type = "C", "Customer Name From", "Supplier Name From") & Space(2) & fromname & Space(2) & "To" & Space(2) & fromname
            'End If

            'If Not (String.IsNullOrEmpty(frommarketcode)) And Not (String.IsNullOrEmpty(tomarketcode)) Then

            '    reportfilter = reportfilter & Space(2) & IIf(type = "C", ":Market From", "Supplier City From") & Space(2) & frommarketcode & Space(2) & "To" & Space(2) & tomarketcode
            'End If

            'If Not (String.IsNullOrEmpty(fromctry)) And Not (String.IsNullOrEmpty(toctry)) Then
            '    reportfilter = reportfilter & Space(2) & "Supplier Country From" & Space(2) & fromctry & Space(2) & "To" & Space(2) & toctry
            'End If
            'If Not (String.IsNullOrEmpty(fromcat)) And Not (String.IsNullOrEmpty(tocat)) Then
            '    reportfilter = reportfilter & Space(2) & "Category  From" & Space(2) & fromcat & Space(2) & "To" & Space(2) & tocat
            'End If
            'If Not (String.IsNullOrEmpty(fromglcode)) And Not (String.IsNullOrEmpty(toglcode)) Then
            '    reportfilter = reportfilter & Space(2) & "Control Account From " & Space(2) & glname & Space(2) & "To" & Space(2) & glname
            'End If

            'If custdetailsdt.Rows.Count > 0 Then

            'End If

            If String.Equals(reportsType, "excel") Then
                ExcelReport(custdetailsdt, actype, type, rptreportname, ledgertype, bytes)

            Else
                If type = "C" Then
                    pdfdoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)
                    documentWidth = 770.0F
                ElseIf actype = "G" Then
                    If ledgertype = "0" Then
                        pdfdoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)
                        documentWidth = 770.0F
                    Else
                        documentWidth = 550.0F
                    End If
                Else
                    documentWidth = 550.0F
                End If


                Using memoryStream As New System.IO.MemoryStream()
                    Dim writer As PdfWriter = PdfWriter.GetInstance(pdfdoc, memoryStream)
                    Dim phrase As Phrase = Nothing
                    Dim cell As PdfPCell = Nothing
                    Dim cell1 As PdfPCell = Nothing
                    Dim titletable As PdfPTable = Nothing
                    Dim Rowtitlebg As BaseColor = New BaseColor(192, 192, 192)
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
                    cell.PaddingBottom = 4
                    cell.SetLeading(12, 0)
                    cell.BackgroundColor = ReportNamebgColor
                    Reporttitle.SpacingBefore = 5
                    Reporttitle.SpacingAfter = 5
                    Reporttitle.AddCell(cell)

                    Dim FilterTable = New PdfPTable(1)
                    ' Add Report Filter
                    FilterTable.TotalWidth = documentWidth
                    FilterTable.LockedWidth = True
                    FilterTable.SetWidths(New Single() {1.0F})
                    FilterTable.Complete = False
                    FilterTable.SplitRows = False
                    phrase = New Phrase()
                    phrase.Add(New Chunk(reportfilter, FilterFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    '  cell.Colspan = 2
                    cell.PaddingTop = 4
                    cell.PaddingBottom = 4
                    cell.SetLeading(12, 0)
                    FilterTable.SpacingAfter = 7
                    FilterTable.AddCell(cell)

                    Dim FooterTable = New PdfPTable(1)

                    FooterTable.TotalWidth = documentWidth
                    'FooterTable.PaddingTop = 10.0F
                    FooterTable.SpacingAfter = 5.0F
                    FooterTable.LockedWidth = True
                    FooterTable.SetWidths(New Single() {1.0F})
                    FooterTable.Complete = False
                    FooterTable.SplitRows = False

                    phrase = New Phrase()
                    phrase.Add(New Chunk(addrLine3 + addrLine5, FooterFont))
                    cell = New PdfPCell(phrase)
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Colspan = 2
                    cell.SetLeading(10, 0)
                    cell.PaddingBottom = 1
                    FooterTable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(addrLine1, FooterFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    '  cell.Colspan = 2
                    cell.SetLeading(10, 0)
                    ' cell.PaddingBottom = 1
                    FooterTable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(addrLine2 + "  " + addrLine4, FooterFont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Colspan = 2
                    cell.SetLeading(10, 0)
                    cell.PaddingBottom = 1
                    FooterTable.AddCell(cell)
                    '  DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Printed Date: " & Date.Now.ToString("dd/MM/yyyy HH:mm:ss"), FooterFont))
                    cell = New PdfPCell(phrase)
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 3
                    FooterTable.AddCell(cell)

                    FooterTable.Complete = True

                    writer.PageEvent = New ClsHeaderFooter(titletable, Reporttitle, FooterTable, FilterTable)
                    ' writer.PageEvent = New ClsHeaderFooter(titletable, Reporttitle, FooterTable, FilterTable, "printDate")
                    'pdfdoc.Add(Reporttitle)

                    pdfdoc.Open()
                    If custdetailsdt.Rows.Count > 0 Then

                        Dim MainTable = New PdfPTable(1)
                        MainTable.TotalWidth = documentWidth
                        MainTable.LockedWidth = True
                        MainTable.PaddingTop = 2.0F
                        MainTable.SpacingAfter = 10.0F
                        MainTable.SetWidths(New Single() {1.0F})
                        MainTable.SplitRows = False
                        Dim groups = Nothing
                        If actype = "G" Then
                            groups = From custledger In custdetailsdt.AsEnumerable() Group custledger By g = New With {Key .acccode = custledger.Field(Of String)("acc_code"), Key .accname = custledger.Field(Of String)("acctname")} Into Group Order By g.acccode
                        Else
                            groups = From custledger In custdetailsdt.AsEnumerable() Group custledger By g = New With {Key .acccode = custledger.Field(Of String)("acc_code"), Key .gl_code = custledger.Field(Of String)("acc_gl_code"), Key .accname = custledger.Field(Of String)("accname")} Into Group Order By g.acccode
                        End If


                        For Each grow In groups
                            totalcredit = 0
                            totaldebit = 0
                            rcount = 0
                            Dim acccode = grow.g.acccode
                            If type = "C" Or type = "S" Then
                                glcode = grow.g.gl_code
                            End If
                            Dim accname = grow.g.accname

                            'Dim acctname = (From cust In custdetailsdt.Rows Where cust("acc_code") = acccode And cust("acc_gl_code") = glcode And cust("accname") = accname)

                            For Each party_ledger In grow.Group
                                debit = Decimal.Parse(IIf(IsDBNull(party_ledger("debit")), "0", party_ledger("debit")))
                                credit = Decimal.Parse(party_ledger("credit"))
                                totaldebit = totaldebit + debit
                                totalcredit = totalcredit + credit
                                'currDecno = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & party_ledger("currcode") & "'"), Integer) Tanvir 05052023
                                currDecno = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='509'"), Integer) 'Tanvir 05052023

                                'balance = IIf(party_ledger("debit") = 0.0, Decimal.Parse(party_ledger("credit")).ToString("N2") & "CR", Decimal.Parse(party_ledger("debit")).ToString("N2") & "DR")
                                If type = "C" Or actype = "G" Then
                                    balance = IIf(totaldebit - totalcredit > 0, (totaldebit - totalcredit).ToString("N" + currDecno.ToString) & " DR", ((totaldebit - totalcredit) * -1).ToString("N" + currDecno.ToString) & " CR")
                                    'fbal = fbal + IIf(totaldebit - totalcredit > 0, (totaldebit - totalcredit), ((totaldebit - totalcredit) * -1))
                                    fbal = fbal + (debit - credit)
                                Else
                                    balance = IIf(totalcredit - totaldebit > 0, (totalcredit - totaldebit).ToString("N" + currDecno.ToString) & " CR", ((totalcredit - totaldebit) * -1).ToString("N" + currDecno.ToString) & " DR")
                                End If



                                If ledgertype = "1" AndAlso Not actype = "G" Then
                                    tableTitle1 = New PdfPTable(9)
                                    tableTitle1.SetWidths(New Single() {0.1F, 0.08F, 0.05F, 0.08F, 0.17F, 0.13F, 0.13F, 0.13F, 0.13F})
                                    arrHeader = {"Date", "Doc. NO ", "Type", "Other Ref", "Description", "AdjustedBillRef", "Debit(" & party_ledger("currcode") & ")", "Credit(" + party_ledger("currcode") + ")", "Balance(" + party_ledger("currcode") + ")"}
                                    tableData1 = New PdfPTable(9)
                                    tableData = New PdfPTable(9)
                                    count = 6
                                    tableData1.SetWidths(New Single() {0.1F, 0.08F, 0.05F, 0.08F, 0.17F, 0.13F, 0.13F, 0.13F, 0.13F})
                                    arrData = {party_ledger("trandate"), party_ledger("tranid"), party_ledger("trantype"), party_ledger("refno"), party_ledger("narration"), party_ledger("AdjustedBillRef"), IIf(debit = 0.0, "-", debit.ToString("N" + currDecno.ToString)), IIf(credit = 0.0, "-", credit.ToString("N" + currDecno.ToString)), balance}
                                    count2 = 5
                                    arrData1 = {"", "", "", "", "", "Total", IIf(totaldebit = 0.0, "-", totaldebit.ToString("N" + currDecno.ToString)), IIf(totalcredit = 0.0, "-", totalcredit.ToString("N" + currDecno.ToString)), ""}
                                    tableData.SetWidths(New Single() {0.1F, 0.08F, 0.05F, 0.08F, 0.17F, 0.13F, 0.13F, 0.13F, 0.13F})
                                Else
                                    tableTitle1.SetWidths(New Single() {0.09F, 0.12F, 0.05F, 0.09F, 0.26F, 0.13F, 0.13F, 0.13F})
                                    arrHeader = {"Date", "Doc No. ", "Type", "Other Ref", "Description", "Debit(" & party_ledger("currcode") & ")", "Credit(" + party_ledger("currcode") + ")", "Balance(" + party_ledger("currcode") + ")"}
                                    count = 5
                                    tableData1.SetWidths(New Single() {0.09F, 0.12F, 0.05F, 0.09F, 0.26F, 0.13F, 0.13F, 0.13F})
                                    arrData = {party_ledger("trandate"), party_ledger("tranid"), party_ledger("trantype"), party_ledger("refno"), party_ledger("narration"), IIf(debit = 0.0, "-", debit.ToString("N" + currDecno.ToString)), IIf(credit = 0.0, "-", credit.ToString("N" + currDecno.ToString)), balance}
                                    count2 = 4
                                    arrData1 = {"", "", "", "", IIf(actype = "G", "Total", "SubTotal"), IIf(totaldebit = 0.0, "-", totaldebit.ToString("N" + currDecno.ToString)), IIf(totalcredit = 0.0, "-", totalcredit.ToString("N" + currDecno.ToString)), ""}
                                    tableData.SetWidths(New Single() {0.09F, 0.12F, 0.05F, 0.09F, 0.26F, 0.13F, 0.13F, 0.13F})

                                End If

                                If rcount = 0 Then
                                    rcount = rcount + 1
                                    Dim tableTitle = New PdfPTable(4)
                                    Dim arrHeaders() As String
                                    If actype = "G" Then
                                        tableTitle = New PdfPTable(2)
                                        arrHeaders = {acccode, accname}
                                        tableTitle.SetWidths(New Single() {0.15F, 0.85F})
                                    Else
                                        arrHeaders = {acccode, accname, glcode, party_ledger("acctname")}
                                        tableTitle.SetWidths(New Single() {0.15F, 0.35F, 0.16F, 0.35F})
                                    End If
                                    tableTitle.TotalWidth = documentWidth
                                    tableTitle.LockedWidth = True
                                    tableTitle.Complete = False
                                    tableTitle.SplitRows = False
                                    tableTitle.KeepTogether = True
                                    For i = 0 To arrHeaders.Length - 1
                                        phrase = New Phrase()
                                        phrase.Add(New Chunk(arrHeaders(i), NormalFontBold))
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                        cell.SetLeading(12, 0)
                                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                        cell.BackgroundColor = Rowtitlebg
                                        cell.PaddingBottom = 4.0F
                                        cell.PaddingTop = 1.0F
                                        If i = 0 Then
                                            cell.BorderWidthRight = 0
                                        ElseIf i = arrHeaders.Length - 1 Then
                                            cell.BorderWidthLeft = 0
                                        Else
                                            cell.BorderWidthRight = 0
                                            cell.BorderWidthLeft = 0
                                        End If
                                        tableTitle.AddCell(cell)
                                    Next

                                    pdfdoc.Add(tableTitle)
                                    tableTitle1.TotalWidth = documentWidth
                                    tableTitle1.LockedWidth = True
                                    tableTitle1.Complete = False
                                    tableTitle1.SplitRows = False
                                    tableTitle1.KeepTogether = True
                                    For i = 0 To arrHeader.Length - 1
                                        phrase = New Phrase()
                                        phrase.Add(New Chunk(arrHeader(i), NormalFontBold))
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                        cell.SetLeading(12, 0)
                                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
                                        cell.BackgroundColor = Rowtitlebg
                                        cell.PaddingBottom = 4.0F
                                        cell.PaddingTop = 1.0F
                                        tableTitle1.SpacingBefore = 0
                                        tableTitle1.AddCell(cell)
                                    Next
                                    pdfdoc.Add(tableTitle1)
                                End If

                                tableData.TotalWidth = documentWidth
                                tableData.LockedWidth = True
                                tableData.Complete = False
                                tableData.SplitRows = False
                                tableData.KeepTogether = True
                                For i = 0 To arrData.Length - 1
                                    phrase = New Phrase()
                              
                                        phrase.Add(New Chunk(arrData(i), NormalFont))



                                    If i >= count Then

                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                    Else
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                        cell.BorderWidthBottom = 0
                                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                    End If

                                    cell.SetLeading(12, 0)
                                    cell.PaddingBottom = 4.0F
                                    tableData.SpacingBefore = 0
                                    tableData.AddCell(cell)
                                Next
                                pdfdoc.Add(tableData)
                            Next

                            'Subtotal of Client 
                            tableData1.TotalWidth = documentWidth
                            tableData1.LockedWidth = True
                            tableData1.Complete = False
                            tableData1.SplitRows = False
                            For i = 0 To arrData1.Length - 1
                                If i <= count2 Then

                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(arrData1(i), NormalFontBold))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                    cell.BorderWidthTop = 0
                                    cell.VerticalAlignment = PdfPCell.ALIGN_BOTTOM

                                Else
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(arrData1(i), NormalFontBold))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                End If
                                cell.SetLeading(12, 0)
                                cell.PaddingBottom = 4.0F
                                tableData1.SpacingBefore = 0
                                tableData1.AddCell(cell)
                            Next
                            pdfdoc.Add(tableData1)
                            ftotaldebit = ftotaldebit + totaldebit
                            ftotalcredit = ftotalcredit + totalcredit
                        Next
                        If actype = "G" Then
                            arrData1 = {"", "", "", "", "Final Total", IIf(ftotaldebit = 0.0, "-", ftotaldebit.ToString("N" + currDecno.ToString())), IIf(ftotalcredit = 0.0, "-", ftotalcredit.ToString("N" + currDecno.ToString())), fbal.ToString("N" + currDecno.ToString())}
                            tableData1.TotalWidth = documentWidth
                            tableData1.LockedWidth = True
                            tableData1.Complete = False
                            tableData1.SplitRows = False
                            For i = 0 To arrData1.Length - 1
                                If i <= count2 Then

                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(arrData1(i), NormalFontBold))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                    cell.BorderWidthTop = 0
                                    cell.VerticalAlignment = PdfPCell.ALIGN_BOTTOM

                                Else
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(arrData1(i), NormalFontBold))
                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                End If
                                cell.SetLeading(12, 0)
                                cell.PaddingBottom = 4.0F
                                cell.BorderWidthTop = 0
                                cell.BorderWidthBottom = 0
                                cell.BorderWidthLeft = 0
                                cell.BorderWidthRight = 0
                                tableData1.AddCell(cell)
                            Next
                            tableData1.Complete = True
                            tableData1.SpacingBefore = 10
                            pdfdoc.Add(tableData1)
                        End If

                    End If
                    pdfdoc.AddTitle(rptreportname)
                    pdfdoc.Close()

                    If printMode = "download" Then
                        Dim pagingFont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)
                        Dim reader As New PdfReader(memoryStream.ToArray())
                        Using mStream As New System.IO.MemoryStream()
                            Using stamper As New PdfStamper(reader, mStream)
                                Dim pages As Integer = reader.NumberOfPages
                                For i As Integer = 1 To pages
                                    '       ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), documentWidth, 5.0F, 0)
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), IIf(documentWidth = 550.0F, documentWidth + 25, documentWidth + 35), 11.0F, 0)
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
#Region "ExcelReport"
    Public Sub ExcelReport(ByVal custdetailsdt As DataTable, ByVal actype As String, ByVal type As String, ByVal rptreportname As String, ByVal ledgertype As String, ByRef bytes() As Byte)


        Dim arrHeaders() As String
        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add("Ledger")
        ws.Columns.AdjustToContents()
        Dim rownum As Integer = 6

        Dim company, report, filter
        ws.Columns("A:D").Width = 11
        ws.Columns("B").Width = 15
        ws.Columns("E").Width = 30
        ws.Columns("F:I").Width = 13

        If ledgertype = "1" AndAlso Not actype = "G" Then
            company = ws.Range("A2:I2").Merge()
            report = ws.Range("A3:I3").Merge()
            filter = ws.Range("A4:I4").Merge()
        Else
            report = ws.Range("A3:H3").Merge()
            filter = ws.Range("A4:H4").Merge()
            company = ws.Range("A2:H2").Merge()
        End If

        'Comapny Name Heading
        ws.Cell("A2").Value = rptcompanyname
        company.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0048C0")) 'Tanvir 05052023
        company.Style.Font.FontSize = 15
        company.Style.Font.FontColor = XLColor.White
        company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
        company.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True

        'Report Name Heading
        ws.Cell("A3").Value = rptreportname
        report.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0000C0"))  'Tanvir 05052023
        report.Style.Font.FontSize = 14
        report.Style.Font.FontColor = XLColor.White
        report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
        report.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True

        filter.Style.Font.SetBold().Font.FontSize = 12
        filter.Style.Font.FontColor = XLColor.Black
        filter.Cell(1, 1).Value = reportfilter
        ' Dim le = reportfilter.Length
        ' ws.Row(4).AdjustToContents()
        Dim rowheight As Integer

        If reportfilter.Length > 100 Then
            rowheight = IIf(reportfilter.Length > 100 And reportfilter.Length < 200, 32, IIf(reportfilter.Length > 200 And reportfilter.Length < 300, 48, 64))
            ws.Row(4).Height = rowheight
        End If

        filter.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
        If custdetailsdt.Rows.Count > 0 Then


            Dim groups = Nothing
            If actype = "G" Then
                groups = From custledger In custdetailsdt.AsEnumerable() Group custledger By g = New With {Key .acccode = custledger.Field(Of String)("acc_code"), Key .accname = custledger.Field(Of String)("acctname")} Into Group Order By g.acccode
            Else
                groups = From custledger In custdetailsdt.AsEnumerable() Group custledger By g = New With {Key .acccode = custledger.Field(Of String)("acc_code"), Key .gl_code = custledger.Field(Of String)("acc_gl_code"), Key .accname = custledger.Field(Of String)("accname")} Into Group Order By g.acccode
            End If

            For Each grow In groups
                totalcredit = 0
                totaldebit = 0
                rcount = 0
                Dim acccode = grow.g.acccode
                If type = "C" Or type = "S" Then
                    glcode = grow.g.gl_code
                End If
                Dim accname = grow.g.accname

                For Each party_ledger In grow.Group
                    debit = Decimal.Parse(party_ledger("debit"))
                    credit = Decimal.Parse(party_ledger("credit"))
                    totaldebit = totaldebit + debit
                    totalcredit = totalcredit + credit
                    '  currDecno = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & party_ledger("currcode") & "'"), Integer) 'Tanvir 04052023

                    currDecno = Convert.ToInt32(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='509'")) 'Tanvir 05052023

                    If type = "C" Or actype = "G" Then
                        balance = IIf(totaldebit - totalcredit > 0, (totaldebit - totalcredit).ToString("N" + currDecno.ToString) & " DR", ((totaldebit - totalcredit) * -1).ToString("N" + currDecno.ToString) & " CR")
                        fbal = fbal + (debit - credit)
                    Else
                        balance = IIf(totalcredit - totaldebit > 0, (totalcredit - totaldebit).ToString("N" + currDecno.ToString) & " CR", ((totalcredit - totaldebit) * -1).ToString("N" + currDecno.ToString) & " DR")
                    End If

                    If ledgertype = "1" AndAlso Not actype = "G" Then
                        arrHeader = {"Date", "Doc. NO ", "Type", "Other Ref", "Description", "AdjustedBillRef", "Debit(" & party_ledger("currcode") & ")", "Credit(" + party_ledger("currcode") + ")", "Balance(" + party_ledger("currcode") + ")"}
                        count = 6
                        arrData = {party_ledger("trandate"), party_ledger("tranid"), party_ledger("trantype"), party_ledger("refno"), party_ledger("narration"), party_ledger("AdjustedBillRef"), IIf(debit = 0.0, "-", debit.ToString("N" + currDecno.ToString)), IIf(credit = 0.0, "-", credit.ToString("N" + currDecno.ToString)), balance}
                        count2 = 5
                        arrData1 = {"", "", "", "", "", "Total", IIf(totaldebit = 0.0, "-", totaldebit.ToString("N" + currDecno.ToString)), IIf(totalcredit = 0.0, "-", totalcredit.ToString("N" + currDecno.ToString)), ""}
                    Else
                        arrHeader = {"Date", "Doc No. ", "Type", "Other Ref", "Description", "Debit(" & party_ledger("currcode") & ")", "Credit(" + party_ledger("currcode") + ")", "Balance(" + party_ledger("currcode") + ")"}
                        count = 5
                        arrData = {party_ledger("trandate"), party_ledger("tranid"), party_ledger("trantype"), party_ledger("refno"), party_ledger("narration"), IIf(debit = 0.0, "-", debit.ToString("N" + currDecno.ToString)), IIf(credit = 0.0, "-", credit.ToString("N" + currDecno.ToString)), balance}
                        count2 = 4
                        arrData1 = {"", "", "", "", IIf(actype = "G", "Total", "SubTotal"), IIf(totaldebit = 0.0, "-", totaldebit.ToString("N" + currDecno.ToString)), IIf(totalcredit = 0.0, "-", totalcredit.ToString("N" + currDecno.ToString)), ""}
                    End If


                    If rcount = 0 Then
                        rcount = rcount + 1
                        If actype = "G" Then
                            arrHeaders = {(acccode & Space(1) & ".").ToString(), accname}
                        Else
                            arrHeaders = {(acccode & Space(1) & ".").ToString(), accname, glcode, party_ledger("acctname")}
                        End If
                        ws.Range(rownum, 1, rownum, arrHeader.Length).Style.Font.SetBold().Font.FontSize = 10
                        '   ws.Range(rownum, 1, rownum, arrHeader.Length).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin) Tanvir 04052023
                        ' ws.Range(rownum, 1, rownum, 8).Style.Fill.SetBackgroundColor(XLColor.LightGray) Tanvir 04052023
                        ws.Range(rownum, 1, rownum, arrHeader.Length).Style.Alignment.WrapText = True

                        For i = 0 To arrHeaders.Length - 1
                            ws.Range(rownum, 1, rownum, arrHeader.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            If i = arrHeaders.Length - 1 AndAlso actype = "G" Then
                                ws.Range("B" & rownum & ":H" & rownum).Merge()
                                ws.Range("B" & rownum & ":H" & rownum).Value = arrHeaders(i)
                                '  ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None)
                            ElseIf i = 0 Then
                                ws.Cell(rownum, 1).Value = arrHeaders(i)
                                '  ws.Cell(rownum, 1).Style.NumberFormat.SetNumberFormatId((int)XLPredefinedFormat.Number.Integer);


                            Else
                                If i = 1 Then
                                    ws.Range("B" & rownum & ":D" & rownum).Merge()
                                    ws.Range("B" & rownum & ":D" & rownum).Value = arrHeaders(i)
                                    ' ws.Range("B" & rownum & ":D" & rownum).Style.Border.SetLeftBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None)
                                ElseIf i = arrHeaders.Length - 1 Then
                                    If ledgertype = "1" AndAlso Not actype = "G" Then
                                        ws.Range("F" & rownum & ":I" & rownum).Merge()
                                        ws.Range("F" & rownum & ":I" & rownum).Value = arrHeaders(i)
                                        ' ws.Range("F" & rownum & ":I" & rownum).Style.Border.SetLeftBorder(XLBorderStyleValues.None)
                                    Else
                                        ws.Range("G" & rownum & ":H" & rownum).Merge()
                                        ws.Range("G" & rownum & ":H" & rownum).Value = arrHeaders(i)
                                        ' ws.Range("G" & rownum & ":H" & rownum).Style.Border.SetLeftBorder(XLBorderStyleValues.None)
                                    End If
                                Else
                                    ws.Cell(rownum, 5).Value = arrHeaders(i)
                                    ' ws.Cell(rownum, 5).Style.Border.SetLeftBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None)
                                End If
                            End If
                        Next
                        rownum = rownum + 1
                        ws.Range(rownum, 1, rownum, arrHeader.Length).Style.Font.SetBold().Font.FontSize = 10
                        '    ws.Range(rownum, 1, rownum, arrHeader.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin) Tanvir 04052023
                        ws.Range(rownum, 1, rownum, arrHeader.Length).Style.Alignment.WrapText = True
                        For i = 0 To arrHeader.Length - 1
                            ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            ws.Cell(rownum, i + 1).Value = arrHeader(i)
                            '  ws.Cell(rownum, i + 1).Style.Fill.SetBackgroundColor(XLColor.LightGray)  Tanvir 04052023
                        Next
                        rownum = rownum + 1
                    End If
                    ws.Range(rownum, 1, rownum, arrData.Length).Style.Font.FontSize = 9
                    'ws.Range(rownum, 1, rownum, arrHeader.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin) Tanvir 04052023
                    ws.Range(rownum, 1, rownum, arrData.Length).Style.Alignment.WrapText = True
                    For i = 0 To arrData.Length - 1
                        If i >= count Then
                            ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        Else
                            ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            ws.Cell(rownum, i + 1).Style.Border.SetBottomBorder(XLBorderStyleValues.None)
                        End If

                        If i = count Or i = count + 1 Then
                            If Not arrData(i).Equals("-") Then
                                ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrData(i))
                            Else
                                ws.Cell(rownum, i + 1).Value = arrData(i)
                            End If
                            If currDecno = 2 Then
                                ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.00"
                            ElseIf currDecno = 3 Then
                                ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.000"
                            ElseIf currDecno = 4 Then
                                ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.0000"
                            End If
                        Else
                            ws.Cell(rownum, i + 1).Value = arrData(i)
                        End If
                    Next
                    rownum = rownum + 1
                Next
                ws.Range(rownum, 1, rownum, arrData1.Length).Style.Font.SetBold().Font.FontSize = 10
                '   ws.Range(rownum, 1, rownum, arrHeader.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin) Tanvir 04052023
                ws.Range(rownum, 1, rownum, arrData1.Length).Style.Alignment.WrapText = True

                For i = 0 To arrData1.Length - 1
                    ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    If i <= count2 Then
                        ws.Cell(rownum, i + 1).Style.Border.SetTopBorder(XLBorderStyleValues.None)
                    End If
                    If i = count Or i = count + 1 Then
                        If Not arrData(i).Equals("-") Then
                            ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrData1(i))
                            If currDecno = 2 Then
                                ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.00"
                            ElseIf currDecno = 3 Then
                                ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.000"
                            ElseIf currDecno = 4 Then
                                ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.0000"
                            End If
                        Else
                            ws.Cell(rownum, i + 1).Value = arrData1(i)
                        End If

                    Else
                        ws.Cell(rownum, i + 1).Value = arrData1(i)
                    End If
                Next
                rownum = rownum + 1
                ftotaldebit = ftotaldebit + totaldebit
                ftotalcredit = ftotalcredit + totalcredit
            Next
            'Tanvir 04052023
            If actype = "G" Then
                rownum = rownum + 1
                ws.Range(rownum, 1, rownum, arrData1.Length).Style.Font.SetBold().Font.FontSize = 10
                ' ws.Range(rownum, 1, rownum, arrData1.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin) Tanvir 04052023
                ws.Range(rownum, 1, rownum, arrData1.Length).Style.Alignment.WrapText = True
                arrData1 = {"", "", "", "", "Final Total", IIf(ftotaldebit = 0.0, "-", ftotaldebit.ToString("N" + currDecno.ToString())), IIf(ftotalcredit = 0.0, "-", ftotalcredit.ToString("N" + currDecno.ToString())), fbal.ToString("N" + currDecno.ToString())}
                For i = 0 To arrData1.Length - 1
                    ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ws.Range(rownum, 2, rownum, arrData1.Length + 1).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None)
                    If i > 4 Then
                        If Not arrData1(i).Equals("-") Then
                            ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrData1(i))
                        Else
                            ws.Cell(rownum, i + 1).Value = arrData1(i)
                        End If
                        If currDecno = 2 Then
                            ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.00"
                        ElseIf currDecno = 3 Then
                            ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.000"
                        ElseIf currDecno = 4 Then
                            ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.0000"
                        End If
                    Else
                        ws.Cell(rownum, i + 1).Value = arrData1(i)
                    End If
                Next
            End If
            'Tanvir 04052023

        End If
        ws.Cell((rownum + 2), 1).Value = addrLine3 & addrLine5
        ws.Range((rownum + 2), 1, (rownum + 2), 5).Merge()
        ws.Cell((rownum + 3), 1).Value = addrLine1
        ws.Range((rownum + 3), 1, (rownum + 3), 5).Merge()
        ws.Cell((rownum + 4), 1).Value = addrLine2 & "  " & addrLine4
        ws.Range((rownum + 4), 1, (rownum + 4), 5).Merge()
        ws.Cell((rownum + 5), 1).Value = "Printed Date : " & Now.ToString("dd/MM/yyyy")
        ws.Range((rownum + 5), 1, (rownum + 5), 5).Merge()
        Using wStream As New MemoryStream()
            wb.SaveAs(wStream)
            bytes = wStream.ToArray()
        End Using
    End Sub

#End Region
End Class
