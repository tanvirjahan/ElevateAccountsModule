Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Globalization
Imports System.Collections.Generic
Imports System.Linq
'Imports System.Drawing
Imports ClosedXML.Excel
Partial Class Test
    Inherits System.Web.UI.Page
    Dim objutils As New clsUtils

    '#Region "global declaration"
    '    Dim objclsUtilities As New clsUtils

    '    Dim Level1font As Font = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.WHITE)

    '    Dim Level2font As Font = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK)
    '    Dim Level3font As Font = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)
    '    Dim Level4font As Font = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLACK)
    '    Dim Level5font As Font = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)

    '    Dim normalfont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
    '    Dim caption As Font = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.BLACK)
    '    Dim normalfontbold As Font = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)
    '    Dim headerfont As Font = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.BLACK)
    '    Dim Companyname As Font = New Font(FontFactory.GetFont("Times New Roman", 14, Font.BOLD, BaseColor.BLACK))
    '    Dim ReportNamefont As Font = New Font(FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK))
    '    Dim Companyname1 As Font = New Font(FontFactory.GetFont("Times New Roman", 14, Font.BOLD, BaseColor.WHITE))
    '    Dim ReportNamefont1 As Font = New Font(FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.WHITE))

    '    Dim rptcompanyname, rptreportname, decno, fromname, sqlquery, rptfilter, currname, month4, addrLine, addrLine1, addrLine2, addrLine3, addrLine4, addrLine5 As String
    '    Dim documentWidth As Single = 550.0F
    '    Dim CompanybgColor As BaseColor = New BaseColor(0, 72, 192)
    '    Dim ReportNamebgColor As BaseColor = New BaseColor(0, 128, 192)
    '    Dim CompanybgColor1 As BaseColor = New BaseColor(225, 225, 225)
    '    Dim ReportNamebgColor1 As BaseColor = New BaseColor(225, 225, 225)
    '    Dim View_pf As New DataTable
    '    Dim acctgroupTable As New DataTable
    '    Dim view_actgroup As New DataTable
    '    Dim month1, month2, month3 As Integer
    '    Dim costofsales, costofsales1, costofsales2, costofsales3, income, income1, income2, income3, expanse, expanse1, expanse2, expanse3 As Decimal
    '#End Region

    '#Region "global declaration"
    '    Dim objclsUtilities As New clsUtils
    '    Dim Header As BaseColor = New BaseColor(179, 217, 255)
    '    Dim tback As BaseColor = New BaseColor(255, 188, 155)
    '    Dim normalfont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
    '    Dim normalfontgray As Font = FontFactory.GetFont("Arial", 7, Font.NORMAL, BaseColor.DARK_GRAY)
    '    Dim footerdfont As Font = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)
    '    Dim companyTitle As Font = FontFactory.GetFont("Verdana", 12, Font.BOLD, BaseColor.BLACK)
    '    Dim reportname As Font = FontFactory.GetFont("Verdana", 14, Font.BOLD, BaseColor.BLACK)
    '    Dim normalfontbold As Font = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)
    '    Dim headerfont As Font = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)
    '#End Region

    '#Region "Global Variable"
    '    Dim objUtils As New clsUtils
    '    Dim NormalFont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)
    '    Dim datafont As Font = FontFactory.GetFont("Arial", 7, Font.NORMAL, BaseColor.BLACK)
    '    Dim datafontBold As Font = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.BLACK)
    '    Dim HeadingFont As Font = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.BLACK)
    '    Dim basecolor As BaseColor = New BaseColor(211, 211, 211)
    '    Dim BalFont As Font = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK)
    '    Dim TitleFont As Font = FontFactory.GetFont("Times New Roman", 12, Font.BOLD, BaseColor.BLACK)
    '    Dim NormalFontBold As Font = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK)
    '    Dim custSupType, contact1, DecimalPoint, DecimalPoint1 As String
    '    Dim acrlimit, cdays, finalcredit, finaldebit, totalbalances As Decimal
    '    Dim documentWidth As Single = 770.0F
    '    Dim currency As String
    '    Dim Month, reportfilter As String
    '    Dim agebalance, age9, age1, age2, age3, age4, age5, age6, age1bal As Decimal
    '    Dim currDecno As Integer
    '    Dim phrase As Phrase = Nothing
    '    Dim cell As PdfPCell = Nothing
    '    Dim rptcompanyname As String
    '#End Region

#Region "global declaration"
    Dim objclsUtilities As New clsUtils

    Dim Level1font As Font = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.WHITE)
    Dim Level2font As Font = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK)
    Dim Level3font As Font = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)
    Dim Level4font As Font = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLACK)
    Dim Level5font As Font = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)

    Dim normalfont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
    Dim caption As Font = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.BLACK)
    Dim normalfontbold As Font = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK)
    Dim headerfont As Font = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.BLACK)
    Dim Companyname As Font = New Font(FontFactory.GetFont("Times New Roman", 14, Font.BOLD, BaseColor.BLACK))
    Dim ReportNamefont As Font = New Font(FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK))
    Dim Companyname1 As Font = New Font(FontFactory.GetFont("Times New Roman", 14, Font.BOLD, BaseColor.WHITE))
    Dim ReportNamefont1 As Font = New Font(FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.WHITE))

    Dim rptcompanyname, rptreportname, decno, fromname, sqlquery, rptfilter, currname, month4, addrLine, addrLine1, addrLine2, addrLine3, addrLine4, addrLine5 As String
    Dim documentWidth As Single = 550.0F
    Dim CompanybgColor As BaseColor = New BaseColor(0, 72, 192)
    Dim ReportNamebgColor As BaseColor = New BaseColor(0, 128, 192)
    
    Dim View_pf As New DataTable
    Dim acctgroupTable As New DataTable
    Dim view_actgroup As New DataTable
    Dim month1, month2, month3 As Integer
    Dim costofsales, costofsales1, costofsales2, costofsales3, income, income1, income2, income3, expanse, expanse1, expanse2, expanse3 As Decimal
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("dbconnectionName") = "strDBConnection"
        Dim bytes As Byte()
        bytes = {}
        Try
            'GenerateReportMonthwise("pdf", "01/03/2021", "13/04/2021", "01", "1", "Profit", "0", "0", bytes, "download")
            'GenerateReportRV("RV", "000003", "01", "B", 0, 0, "Receipt Voucher", bytes, "download")
            GenerateReport("pdf", "01/03/2021", "13/04/2021", "01", "0", "Profit", "0", "0", bytes, "download")

            Dim ds As New DataSet
            'CreatePdf("pdf", "From Date:  15/04/2021  To :  15/04/2021", "0", "2021/04/15", "2021/04/15", "S", "0", "", "", "", "", "", "", "", "", "", "", "0", "0", "1", 0, 0, "01", "", "0", bytes, ds, "", "download")
            Response.AddHeader("Content-Disposition", "inline; filename=" + "ProfitLoss" + "@" + DateTime.Now.ToString("ddMMyyyy@HHmmss") + ".pdf")
            Response.AddHeader("Content-Length", bytes.Length.ToString())
            Response.ContentType = "application/pdf"



            Response.Buffer = True
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.BinaryWrite(bytes)
            Response.Flush()
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        Catch ex As Exception

        End Try


    End Sub
#End Region

#Region "GenerateReport"
    Public Sub GenerateReport(ByVal reportsType As String, ByVal fromdate As String, ByVal todate As String, ByVal divcode As String, ByVal rpttype As String, ByVal type As String, ByVal closing As String, ByVal strrpttype1 As String, ByRef bytes() As Byte, ByVal printMode As String, Optional ByVal fileName As String = "")

        Try
            If divcode <> "" Then
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
            Else
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
            End If

            rptreportname = " Report- Income Statement "


            rptreportname = rptreportname & Environment.NewLine & vbLf & "From" & Space(2) & fromdate & "  To" & Space(2) & todate
            rptfilter = "From" & Space(2) & fromdate & "  To" & Space(2) & todate
            decno = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
            decno = "N" & decno


            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("sp_rep_profitloss_new_5", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")

            mySqlCmd.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 20)).Value = divcode
            mySqlCmd.Parameters.Add(New SqlParameter("@closing", SqlDbType.Int)).Value = closing

            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim custdetailsdt As New DataTable
            custdetailsdt = ds.Tables(0)

            If reportsType = "excel" Then
                'GenerateReportExcel(divcode, custdetailsdt, bytes)
            Else
                Dim line As Paragraph
                line = New Paragraph(New Chunk(New iTextSharp.text.pdf.draw.LineSeparator(1.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)))
                Dim document As New Document(PageSize.A4, -10.0F, 10.0F, 30.0F, 35.0F)
                Using memoryStream As New System.IO.MemoryStream()
                    Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)

                    Dim phrase As Phrase = Nothing
                    Dim cell As PdfPCell = Nothing
                    'Dim titletable As PdfPTable = Nothing
                    Dim debit, credit, totaldedit, totalcredit As Decimal

                    Dim arrHeaders() As String

                    Dim logo As PdfPTable = New PdfPTable(1)
                    logo.TotalWidth = documentWidth
                    logo.LockedWidth = True
                    logo.SetWidths(New Single() {1.0F})
                    logo.Complete = False
                    logo.SplitRows = False
                    'company name
                    If divcode = "01" Then
                        cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
                    Else
                        cell = ImageCell("~/Images/Logo1.png", 80.0F, PdfPCell.ALIGN_LEFT)
                    End If
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4

                    logo.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(rptcompanyname, Companyname))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
                    logo.AddCell(cell)

                    Dim tblTitle As PdfPTable = New PdfPTable(1)
                    tblTitle.SetWidths(New Single() {1.0F})
                    tblTitle.TotalWidth = documentWidth
                    tblTitle.LockedWidth = True
                    phrase = New Phrase()
                    phrase.Add(New Chunk(rptreportname, ReportNamefont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.PaddingBottom = 3.0F
                    tblTitle.AddCell(cell)
                    tblTitle.SpacingBefore = 7

                    Dim titletable = New PdfPTable(1)
                    titletable.TotalWidth = documentWidth
                    titletable.LockedWidth = True
                    titletable.SetWidths(New Single() {1.0F})

                    titletable.SplitRows = False
                    titletable.WidthPercentage = 100
                    If divcode = "01" Then
                        cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
                    Else
                        cell = ImageCell("~/Images/Logo1.png", 80.0F, PdfPCell.ALIGN_LEFT)
                    End If
                    titletable.AddCell(cell)
                    phrase = New Phrase()
                    phrase.Add(New Chunk(rptcompanyname, Companyname))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    cell.BackgroundColor = CompanybgColor
                    titletable.AddCell(cell)
                    titletable.Complete = True


                    Dim Reporttitle = New PdfPTable(1)
                    Reporttitle.TotalWidth = documentWidth
                    Reporttitle.LockedWidth = True
                    Reporttitle.SetWidths(New Single() {1.0F})
                    Reporttitle.Complete = False
                    '  Reporttitle.SplitRows = False
                    phrase = New Phrase()
                    phrase.Add(New Chunk(rptreportname, ReportNamefont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.PaddingBottom = 4
                    cell.SetLeading(12, 0)
                    cell.BackgroundColor = ReportNamebgColor
                    Reporttitle.SpacingBefore = 5
                    Reporttitle.SpacingAfter = 0
                    Reporttitle.AddCell(cell)
                    Reporttitle.Complete = True

                    Dim FooterTable = New PdfPTable(1)

                    FooterTable.TotalWidth = documentWidth
                    FooterTable.LockedWidth = True
                    FooterTable.SetWidths(New Single() {1.0F})
                    FooterTable.Complete = False
                    FooterTable.SplitRows = False

                    phrase = New Phrase()
                    phrase.Add(New Chunk(addrLine3 + addrLine5, normalfont))
                    cell = New PdfPCell(phrase)
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Colspan = 2
                    cell.SetLeading(10, 0)
                    cell.PaddingBottom = 1
                    'cell.BorderWidth = 1
                    FooterTable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(addrLine1, normalfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Colspan = 2
                    cell.SetLeading(10, 0)
                    cell.PaddingBottom = 1
                    FooterTable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(addrLine2 + "  " + addrLine4, normalfont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Colspan = 2
                    cell.SetLeading(10, 0)
                    cell.PaddingBottom = 1
                    FooterTable.AddCell(cell)
                    ' DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Printed Date: " & Date.Now.ToString("yyyy-MM-dd HH:mm:ss"), normalfont))
                    cell = New PdfPCell(phrase)
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 3
                    FooterTable.AddCell(cell)
                    FooterTable.Complete = True


                    Dim MainTitle = New PdfPTable(1)
                    MainTitle.TotalWidth = documentWidth
                    MainTitle.LockedWidth = True

                    MainTitle.Complete = False
                    MainTitle.SplitRows = False
                    MainTitle.SetWidths(New Single() {1.0F})



                    Dim tableTitle = New PdfPTable(3)
                    tableTitle.TotalWidth = documentWidth
                    tableTitle.LockedWidth = True

                    tableTitle.Complete = False
                    tableTitle.SplitRows = False
                    tableTitle.SetWidths(New Single() {0.6F, 0.2F, 0.2F})
                    cell.AddElement(line)
                    cell.Colspan = 3
                    tableTitle.AddCell(cell)
                    'tableTitle.SplitRows = False
                    If custdetailsdt.Rows.Count > 0 Then
                        Dim currcode = custdetailsdt.AsEnumerable().Select(Function(x) New With {Key .currcode = x.Field(Of String)("currcode")}).FirstOrDefault
                        arrHeaders = {"", "Amount(" & currcode.currcode.ToString() & ")", "Total Amount(" & currcode.currcode.ToString() & ")"}
                        For i = 0 To arrHeaders.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                            cell.Colspan = 1
                            cell.SetLeading(12, 0)
                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 5.0F
                            cell.BorderWidthBottom = 1
                            tableTitle.AddCell(cell)
                        Next
                        tableTitle.Complete = True
                        writer.PageEvent = New ClsHeaderFooter(logo, tblTitle, FooterTable, tableTitle)
                    Else
                        writer.PageEvent = New ClsHeaderFooter(Nothing, Nothing, FooterTable, Nothing)
                    End If
                    document.Open()


                    'document.Add(titletable)
                    'document.Add(Reporttitle)

                    'document.Add(tableTitle)

                    If custdetailsdt.Rows.Count > 0 Then
                        Dim tableData As PdfPTable = New PdfPTable(3)
                        tableData.TotalWidth = documentWidth
                        tableData.LockedWidth = True
                        tableData.SplitRows = False
                        '  tableData.KeepTogether = True
                        tableData.SpacingBefore = 5
                        tableData.SpacingAfter = 0
                        tableData.SetWidths(New Single() {0.6F, 0.2F, 0.2F})

                        Dim group1name, gpbyname, gpbycode, expenseTitle, groupname, totalTitle As String
                        Dim grouporder As Integer
                        Dim totalClosingBal, Totalincome, TotalExpense, TotalCost, finalTotalclosingBal As Decimal

                        Dim groups = From gpbyrow In custdetailsdt.AsEnumerable() Group gpbyrow By g = New With {Key .grouporder = gpbyrow.Field(Of Integer)("grouporder"), Key .group1name = gpbyrow.Field(Of String)("group1name")} Into Group Order By g.grouporder

                        For Each gpbyKey In groups
                            grouporder = gpbyKey.g.grouporder
                            groupname = gpbyKey.g.group1name
                            arrHeaders = {IIf(String.Equals(groupname, "Income"), "Revenue", groupname), "", ""}

                            For i = 0 To arrHeaders.Length - 1
                                phrase = New Phrase()
                                phrase.Add(New Chunk(arrHeaders(i), Level1font))

                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                cell.SetLeading(12, 0)
                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                If i = 0 Then
                                    cell.PaddingLeft = 3.0F
                                End If
                                cell.PaddingBottom = 4.0F
                                cell.PaddingTop = 1.0F

                                cell.BorderWidth = 0
                                cell.BorderWidthLeft = 0.5
                                cell.BorderWidthRight = 0.5
                                cell.BorderWidthTop = 0.5
                                cell.BackgroundColor = BaseColor.DARK_GRAY
                                tableData.AddCell(cell)
                            Next

                            If grouporder = 4 Then
                                Dim gpby3 = gpbyKey.Group.CopyToDataTable

                                If grouporder = 4 Then
                                    gpbycode = "group4code"
                                    gpbyname = "group4name"
                                Else
                                    gpbycode = "group3code"
                                    gpbyname = "group2name"
                                End If

                                Dim groups3 = From gpbyrow In gpby3.AsEnumerable() Group gpbyrow By g = New With {Key .gpbycode = gpbyrow.Field(Of String)(gpbycode), Key .gpbyname = gpbyrow.Field(Of String)(gpbyname)} Into Group Order By g.gpbycode
                                For Each groupby3 In groups3

                                    arrHeaders = {groupby3.g.gpbyname, "", ""}
                                    For i = 0 To arrHeaders.Length - 1
                                        phrase = New Phrase()
                                        phrase.Add(New Chunk(arrHeaders(i), Level2font))
                                        If i = 0 Then
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                            cell.PaddingLeft = 12.0F
                                        Else
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                        End If

                                        cell.SetLeading(12, 0)
                                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                        cell.PaddingBottom = 4.0F
                                        cell.PaddingTop = 1.0F
                                        cell.BorderWidth = 0
                                        cell.BorderWidthLeft = 0.5
                                        cell.BorderWidthRight = 0.5
                                        cell.BackgroundColor = BaseColor.GRAY
                                        tableData.AddCell(cell)
                                    Next

                                    For Each row In groupby3.Group
                                        arrHeaders = {row("acct_name"), Decimal.Parse(row("closingbalance")).ToString(decno), ""}
                                        For i = 0 To arrHeaders.Length - 1
                                            phrase = New Phrase()
                                            phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
                                            If i = 0 Then
                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                                cell.PaddingLeft = 24.0F
                                            Else
                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                            End If

                                            cell.SetLeading(12, 0)
                                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                            cell.PaddingBottom = 4.0F
                                            cell.PaddingTop = 1.0F
                                            cell.BorderWidth = 0
                                            cell.BorderWidthLeft = 0.5
                                            cell.BorderWidthRight = 0.5
                                            cell.BackgroundColor = BaseColor.LIGHT_GRAY
                                            tableData.AddCell(cell)
                                        Next

                                        If String.Equals(row("group2name"), "Expenditure") And String.Equals(row("group4name"), "Direct Costs ") Then
                                            expenseTitle = "Total Direct Expenses"
                                        Else
                                            If row("group4name").ToString().Contains("Expanses") Then
                                                expenseTitle = row("group4name").ToString()
                                            Else
                                                expenseTitle = row("group4name").ToString() & Space(2) & "Expanses"
                                            End If
                                        End If
                                        totalClosingBal = totalClosingBal + Decimal.Parse(row("closingbalance"))
                                        finalTotalclosingBal = finalTotalclosingBal + Decimal.Parse(row("closingbalance"))
                                    Next

                                    arrHeaders = {expenseTitle, "", totalClosingBal.ToString(decno)}
                                    totalClosingBal = 0
                                    For i = 0 To arrHeaders.Length - 1
                                        phrase = New Phrase()
                                        phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
                                        If i = 0 Then
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                            cell.PaddingLeft = 3.0F
                                        Else
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                        End If
                                        cell.SetLeading(12, 0)
                                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                        cell.PaddingBottom = 4.0F
                                        cell.PaddingTop = 1.0F
                                        cell.BorderWidth = 0
                                        cell.BorderWidthLeft = 0.5
                                        cell.BorderWidthRight = 0.5
                                        cell.BackgroundColor = New BaseColor(220, 220, 220)
                                        tableData.AddCell(cell)
                                    Next
                                Next

                            Else
                                For Each row In gpbyKey.Group
                                    arrHeaders = {row("acct_name"), Decimal.Parse(row("closingbalance")).ToString(decno), ""}

                                    'added mohamed and param on 17/11/2020
                                    If row("grouporder").ToString().Trim() = "4" Then
                                        TotalExpense = TotalExpense + Decimal.Parse(row("closingbalance"))
                                    ElseIf row("grouporder").ToString().Trim() = "2" Then
                                        TotalCost = TotalCost + Decimal.Parse(row("closingbalance"))
                                    Else
                                        Totalincome = Totalincome + Decimal.Parse(row("closingbalance"))
                                    End If
                                    'Totalincome = Totalincome + Decimal.Parse(row("closingbalance"))

                                    For i = 0 To arrHeaders.Length - 1
                                        phrase = New Phrase()
                                        phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
                                        If i = 0 Then
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                            cell.PaddingLeft = 24.0F
                                        Else
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                        End If

                                        cell.SetLeading(12, 0)
                                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                        cell.PaddingBottom = 4.0F
                                        cell.PaddingTop = 1.0F
                                        cell.BorderWidth = 0
                                        cell.BorderWidthLeft = 0.5
                                        cell.BorderWidthRight = 0.5
                                        cell.BackgroundColor = BaseColor.LIGHT_GRAY
                                        tableData.AddCell(cell)
                                    Next

                                Next
                                arrHeaders = {"Gross Revenue", "", ""}
                                For i = 0 To arrHeaders.Length - 1
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
                                    If i = 0 Then
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                        '  cell.PaddingLeft = 24.0F
                                    Else
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                    End If

                                    cell.SetLeading(12, 0)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                    cell.PaddingBottom = 4.0F
                                    cell.PaddingTop = 1.0F
                                    cell.BorderWidth = 0
                                    cell.BorderWidthLeft = 0.5
                                    cell.BorderWidthRight = 0.5
                                    cell.BackgroundColor = New BaseColor(220, 220, 220)
                                    tableData.AddCell(cell)
                                Next

                                arrHeaders = {"Less : Sales Commission Discount", "", ""}
                                For i = 0 To arrHeaders.Length - 1
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(arrHeaders(i), normalfont))
                                    If i = 0 Then
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                        '  cell.PaddingLeft = 24.0F
                                    Else
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                    End If

                                    cell.SetLeading(12, 0)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                    cell.PaddingBottom = 4.0F
                                    cell.PaddingTop = 1.0F
                                    cell.BorderWidth = 0
                                    cell.BorderWidthLeft = 0.5
                                    cell.BorderWidthRight = 0.5

                                    tableData.AddCell(cell)
                                Next

                                'added mohamed and param on 17/11/2020
                                Dim lPrintTotal As Double
                                lPrintTotal = Totalincome

                                If grouporder = 1 Then
                                    totalTitle = "Net Revenue"
                                    lPrintTotal = Totalincome
                                ElseIf grouporder = 2 Then
                                    totalTitle = "Total Cost of Sales"
                                    lPrintTotal = TotalCost
                                ElseIf grouporder = 4 Then
                                    totalTitle = "Total Expense"
                                    lPrintTotal = TotalExpense
                                Else
                                    totalTitle = "Toatl"
                                End If

                                arrHeaders = {totalTitle, "", lPrintTotal.ToString(decno)}
                                For i = 0 To arrHeaders.Length - 1
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(arrHeaders(i), normalfont))
                                    If i = 0 Then
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                        '  cell.PaddingLeft = 24.0F
                                    Else
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                    End If

                                    cell.SetLeading(12, 0)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                    cell.PaddingBottom = 4.0F
                                    cell.PaddingTop = 1.0F
                                    cell.BorderWidth = 0
                                    cell.BorderWidthLeft = 0.5
                                    cell.BorderWidthRight = 0.5

                                    tableData.AddCell(cell)
                                Next
                            End If

                        Next

                        'added mohamed and param on 17/11/2020
                        If Totalincome - TotalCost - TotalExpense >= 0 Then
                            arrHeaders = {"Net Income(profit)", "", (Totalincome - TotalCost - TotalExpense - finalTotalclosingBal).ToString(decno)}
                        Else
                            arrHeaders = {"Net Income(Loss)", "", (Totalincome - TotalCost - TotalExpense - finalTotalclosingBal).ToString(decno)}
                        End If

                        ''arrHeaders = {"Net Income(Loss)", "", (Totalincome - finalTotalclosingBal).ToString(decno)}
                        For i = 0 To arrHeaders.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrHeaders(i), Level3font))
                            If i = 0 Then
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                cell.BorderWidthRight = 0

                            ElseIf i = arrHeaders.Length - 1 Then
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                cell.BorderWidthLeft = 0
                            Else
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                cell.BorderWidthLeft = 0
                                cell.BorderWidthRight = 0
                            End If

                            cell.SetLeading(12, 0)
                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BorderWidth = 0
                            cell.BorderWidthBottom = 1
                            cell.BorderWidthTop = 1

                            tableData.AddCell(cell)
                        Next

                        document.Add(tableData)
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
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

    Public Sub GenerateReportMonthwise(ByVal reportsType As String, ByVal fromdate As String, ByVal todate As String, ByVal divcode As String, ByVal rpttype As String, ByVal type As String, ByVal closing As String, ByVal strrpttype1 As String, ByRef bytes() As Byte, ByVal printMode As String, Optional ByVal fileName As String = "")
        Dim sqlConn As New SqlConnection

        Try
            If divcode <> "" Then
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
            Else
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
            End If

            rptreportname = " Report- Income Statement"
            decno = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
            decno = "N" & decno
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")

            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            mySqlCmd = New SqlCommand("sp_pf", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@date", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@range", SqlDbType.VarChar, 20)).Value = 3
            mySqlCmd.Parameters.Add(New SqlParameter("@division", SqlDbType.VarChar, 20)).Value = divcode

            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim custdetailsdt As New DataTable
            View_pf = ds.Tables(0)
            sqlquery = "SELECT acctgroup.acctname, acctgroup.div_code, acctgroup.parentid FROM  acctgroup where div_code='" & divcode & "'"

            Using ds1 As New SqlDataAdapter(sqlquery, sqlConn)
                ds1.Fill(acctgroupTable)
            End Using

            sqlquery = "SELECT * FROM  view_actgroup"

            Using ds2 As New SqlDataAdapter(sqlquery, sqlConn)
                ds2.Fill(view_actgroup)
            End Using

            Dim currentdate As Date = Convert.ToDateTime(fromdate)
            month1 = currentdate.Month

            month2 = (currentdate.AddMonths(1)).Month
            month3 = (currentdate.AddMonths(2)).Month
            month4 = "YTD UPTO " + MonthName(month3)


            If reportsType = "excel" Then
                ExcelReportMonthWise(bytes)
            Else
                Dim document As New Document(PageSize.A4, -10.0F, 10.0F, 30.0F, 35.0F)
                Using memoryStream As New System.IO.MemoryStream()
                    Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)
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
                    phrase.Add(New Chunk(rptcompanyname, Companyname1))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    cell.BackgroundColor = CompanybgColor
                    titletable.AddCell(cell)

                    Dim Reporttitle = New PdfPTable(1)
                    Reporttitle.TotalWidth = documentWidth
                    Reporttitle.LockedWidth = True
                    Reporttitle.SetWidths(New Single() {1.0F})
                    Reporttitle.Complete = False
                    Reporttitle.SplitRows = False
                    phrase = New Phrase()
                    phrase.Add(New Chunk(rptreportname, ReportNamefont1))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
                    cell.PaddingBottom = 4
                    cell.SetLeading(12, 0)
                    cell.BackgroundColor = ReportNamebgColor
                    Reporttitle.SpacingBefore = 5
                    Reporttitle.SpacingAfter = 0
                    Reporttitle.AddCell(cell)
                    Reporttitle.Complete = True

                    addrLine1 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1063)
                    addrLine2 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1064)
                    addrLine3 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1065)
                    addrLine4 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1066)
                    addrLine5 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1053)





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

                    Dim arrHeaders() As String
                    Dim tableTitle = New PdfPTable(5)
                    tableTitle.TotalWidth = documentWidth
                    tableTitle.LockedWidth = True

                    tableTitle.Complete = False
                    tableTitle.SplitRows = False
                    tableTitle.Complete = False
                    tableTitle.SplitRows = False
                    tableTitle.SetWidths(New Single() {0.32F, 0.17F, 0.17F, 0.17F, 0.17F})

                    arrHeaders = {"", MonthName(month1), MonthName(month2), MonthName(month3), month4}
                    For i = 0 To arrHeaders.Length - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrHeaders(i), Level3font))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                        cell.SetLeading(12, 0)
                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 2.0F
                        cell.BorderWidthTop = 1
                        cell.BorderWidthBottom = 1
                        tableTitle.AddCell(cell)
                    Next

                    tableTitle.SpacingBefore = 5
                    tableTitle.SpacingAfter = 5

                    writer.PageEvent = New ClsHeaderFooter(titletable, Reporttitle, FooterTable, tableTitle, "printDate")

                    document.Open()
                    If View_pf.Rows.Count > 0 Then

                        Dim tableData = New PdfPTable(5)
                        tableData.TotalWidth = documentWidth
                        tableData.LockedWidth = True

                        tableData.Complete = False
                        tableData.SplitRows = False
                        tableData.Complete = False
                        tableData.SplitRows = False
                        tableData.SetWidths(New Single() {0.32F, 0.17F, 0.17F, 0.17F, 0.17F})

                        Dim acctLevel, gacctLevel0, gacctLevel1, gacctLevel2, gacctLevel3, gacctLevel4, gpbyName As New DataTable
                        '  Dim acctgroup1 As New DataTable
                        Dim amount, amount1, amount2, amount3, amount20, amount21, amount22, amount23 As Decimal
                        Dim acct1name, acct2name As String


                        Dim dataView As New DataView(View_pf)
                        dataView.Sort = "AccName ASC"
                        gpbyName = dataView.ToTable()



                        'Group by Level0 and Head
                        Dim groups = From custledger In gpbyName.AsEnumerable() Group custledger By g = New With {Key .level0 = custledger.Field(Of Integer)("level0"), Key .div_code = custledger.Field(Of String)("div_code"), Key .Head = custledger.Field(Of Integer)("Head")} Into Group Order By g.Head

                        For Each groupby In groups

                            acctLevel = groupby.Group.CopyToDataTable
                            'Group by GroupHead
                            Dim gpbygroupHeader = From custledger In acctLevel.AsEnumerable() Group custledger By g = New With {Key .groupHeader = custledger.Field(Of Integer)("GroupHeader"), Key .div_code = custledger.Field(Of String)("div_code")} Into Group Order By g.div_code


                            For Each gpbyHeader In gpbygroupHeader

                                'Level1
                                gacctLevel0 = gpbyHeader.Group.CopyToDataTable

                                ' Get the Sum of Income
                                Dim Getincome = gacctLevel0.Compute("SUM(Amount)", "GroupHeader='1' AND State>=1")
                                If IsDBNull(Getincome) Then
                                    income = income + 0.0
                                Else
                                    income = income + Convert.ToDecimal(Getincome)

                                End If

                                Dim Getincome1 = gacctLevel0.Compute("SUM(Amount1)", "GroupHeader='1' AND State>=1")
                                If IsDBNull(Getincome1) Then
                                    income1 = income1 + 0.0
                                Else
                                    income1 = income1 + Convert.ToDecimal(Getincome1)

                                End If

                                Dim Getincome2 = gacctLevel0.Compute("SUM(Amount2)", "GroupHeader='1' AND State>=1")
                                If IsDBNull(Getincome2) Then
                                    income2 = income2 + 0.0
                                Else
                                    income2 = income2 + Convert.ToDecimal(Getincome2)

                                End If

                                Dim Getincome3 = gacctLevel0.Compute("SUM(Amount3)", "GroupHeader='1' AND State>=1")
                                If IsDBNull(Getincome3) Then
                                    income3 = income3 + 0.0
                                Else
                                    income3 = income3 + Convert.ToDecimal(Getincome3)
                                End If


                                'Get The Sum of Expanses
                                Dim Getexpanse = gacctLevel0.Compute("SUM(Amount)", "GroupHeader='2' AND State>=1")
                                If IsDBNull(Getexpanse) Then
                                    expanse = expanse + 0.0
                                Else
                                    expanse = expanse + Convert.ToDecimal(Getexpanse)

                                End If

                                Dim Getexpanse1 = gacctLevel0.Compute("SUM(Amount1)", "GroupHeader='2' AND State>=1")
                                If IsDBNull(Getexpanse1) Then
                                    expanse1 = expanse1 + 0.0
                                Else
                                    expanse1 = expanse1 + Convert.ToDecimal(Getexpanse1)

                                End If

                                Dim Getexpanse2 = gacctLevel0.Compute("SUM(Amount2)", "GroupHeader='2' AND State>=1")
                                If IsDBNull(Getexpanse2) Then
                                    expanse2 = expanse2 + 0.0
                                Else
                                    expanse2 = expanse2 + Convert.ToDecimal(Getexpanse2)

                                End If

                                Dim Getexpanse3 = gacctLevel0.Compute("SUM(Amount3)", "GroupHeader='2' AND State>=1")
                                If IsDBNull(Getexpanse3) Then
                                    expanse3 = expanse3 + 0.0
                                Else
                                    expanse3 = expanse3 + Convert.ToDecimal(Getexpanse3)
                                End If


                                'Get the Sum of CostOfSales


                                Dim Getcostofsales = gacctLevel0.Compute("SUM(Amount)", "GroupHeader='3' AND State>=1")
                                If IsDBNull(Getcostofsales) Then
                                    costofsales = costofsales + 0.0
                                Else
                                    costofsales = costofsales + Convert.ToDecimal(Getcostofsales)

                                End If

                                Dim Getcostofsales1 = gacctLevel0.Compute("SUM(Amount1)", "GroupHeader='3' AND State>=1")
                                If IsDBNull(Getcostofsales1) Then
                                    costofsales1 = costofsales1 + 0.0
                                Else
                                    costofsales1 = costofsales1 + Convert.ToDecimal(Getcostofsales1)

                                End If

                                Dim Getcostofsales2 = gacctLevel0.Compute("SUM(Amount2)", "GroupHeader='3' AND State>=1")
                                If IsDBNull(Getcostofsales2) Then
                                    costofsales2 = costofsales2 + 0.0
                                Else
                                    costofsales2 = costofsales2 + Convert.ToDecimal(Getcostofsales2)

                                End If

                                Dim Getcostofsales3 = gacctLevel0.Compute("SUM(Amount3)", "GroupHeader='3' AND State>=1")
                                If IsDBNull(Getcostofsales3) Then
                                    costofsales3 = costofsales3 + 0.0
                                Else
                                    costofsales3 = costofsales3 + Convert.ToDecimal(Getcostofsales3)

                                End If

                                'level 1 Group By
                                Dim gpbyLevel1 = From custledger In gacctLevel0.AsEnumerable() Group custledger By g = New With {Key .level1 = custledger.Field(Of Integer)("level1"), Key .div_code = custledger.Field(Of String)("div_code")} Into Group Order By g.div_code

                                For Each gpby1 In gpbyLevel1
                                    Dim acctgroup1 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname1 = a.Field(Of String)("acctname"), .div_code1 = a.Field(Of String)("div_code"), .parentid1 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid1 = gpby1.g.level1 And a.div_code1 = gpby1.g.div_code).OrderBy(Function(o) o.parentid1)
                                    gacctLevel1 = gpby1.Group.CopyToDataTable



                                    'Dim dataView1 As New DataView(gacctLevel1)
                                    'dataView1.Sort = "AccName ASC"
                                    'gacctLevel1 = dataView.ToTable()
                                    amount = 0
                                    amount1 = 0
                                    amount2 = 0
                                    amount3 = 0
                                    acct1name = ""

                                    If acctgroup1.Count > 0 Then
                                        For Each acct1row In acctgroup1
                                            acct1name = acct1row.acctname1
                                        Next
                                    End If

                                    For Each row In gpby1.Group
                                        If row("state") >= 1 Then
                                            If Not (TypeOf row("amount") Is DBNull) Then
                                                amount = amount + Decimal.Parse(row("amount"))
                                            Else
                                                amount = amount + 0.0
                                            End If

                                            If Not (TypeOf row("amount1") Is DBNull) Then
                                                amount1 = amount1 + Decimal.Parse(row("amount1"))
                                            Else
                                                amount1 = amount1 + 0.0
                                            End If

                                            If Not (TypeOf row("amount2") Is DBNull) Then
                                                amount2 = amount2 + Decimal.Parse(row("amount2"))
                                            Else
                                                amount2 = amount2 + 0.0
                                            End If
                                            If Not (TypeOf row("amount3") Is DBNull) Then
                                                amount3 = amount3 + Decimal.Parse(row("amount3"))
                                            Else
                                                amount3 = amount3 + 0.0
                                            End If

                                        End If
                                    Next

                                    arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
                                    For i = 0 To arrHeaders.Length - 1
                                        phrase = New Phrase()
                                        phrase.Add(New Chunk(arrHeaders(i), Level1font))
                                        If i = 0 Then
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                        Else
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                        End If

                                        cell.SetLeading(12, 0)
                                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                        cell.PaddingBottom = 4.0F
                                        cell.PaddingTop = 1.0F
                                        'cell.BorderWidthTop = 1
                                        cell.BorderWidthBottom = 0
                                        cell.BackgroundColor = BaseColor.DARK_GRAY

                                        tableData.AddCell(cell)
                                    Next

                                    ' Level(2)
                                    Dim acctLevel2 = From row1 In gacctLevel1.AsEnumerable() Group row1 By g1 = New With {Key .level2 = row1.Field(Of Integer)("level2"), Key .divcode2 = row1.Field(Of String)("div_code")} Into Group Order By g1.divcode2

                                    For Each gpbylevel2 In acctLevel2
                                        ' Dim acctgroup2 = acctgroupTable.Select("parentid='" & gpbylevel2.g1.level2 & "'")
                                        Dim acctgroup2 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname2 = a.Field(Of String)("acctname"), .div_code2 = a.Field(Of String)("div_code"), .parentid2 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid2 = gpbylevel2.g1.level2 And a.div_code2 = gpbylevel2.g1.divcode2)
                                        gacctLevel2 = gpbylevel2.Group.CopyToDataTable
                                        amount = 0
                                        amount1 = 0
                                        amount2 = 0
                                        amount3 = 0
                                        acct1name = ""
                                        acct2name = ""
                                        If acctgroup2.Count > 0 Then

                                            For Each acct2row In acctgroup2
                                                acct1name = acct2row.acctname2
                                                acct2name = acct2row.acctname2
                                            Next
                                        End If
                                        For Each row In gpbylevel2.Group
                                            If row("state") >= 1 Then
                                                If Not (TypeOf row("amount") Is DBNull) Then
                                                    amount = amount + Decimal.Parse(row("amount"))
                                                Else
                                                    amount = amount + 0.0
                                                End If

                                                If Not (TypeOf row("amount1") Is DBNull) Then
                                                    amount1 = amount1 + Decimal.Parse(row("amount1"))
                                                Else
                                                    amount1 = amount1 + 0.0
                                                End If

                                                If Not (TypeOf row("amount2") Is DBNull) Then
                                                    amount2 = amount2 + Decimal.Parse(row("amount2"))
                                                Else
                                                    amount2 = amount2 + 0.0
                                                End If
                                                If Not (TypeOf row("amount3") Is DBNull) Then
                                                    amount3 = amount3 + Decimal.Parse(row("amount3"))
                                                Else
                                                    amount3 = amount3 + 0.0
                                                End If

                                            End If

                                        Next

                                        arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
                                        For i = 0 To arrHeaders.Length - 1
                                            phrase = New Phrase()
                                            phrase.Add(New Chunk(arrHeaders(i), Level2font))
                                            If i = 0 Then
                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                            Else
                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                            End If
                                            cell.SetLeading(12, 0)
                                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                            cell.PaddingBottom = 4.0F
                                            cell.PaddingTop = 1.0F
                                            cell.Border = 0
                                            cell.BorderWidthLeft = 0.5
                                            cell.BorderWidthRight = 0.5
                                            cell.BackgroundColor = BaseColor.GRAY
                                            'cell.BorderWidthTop = 0
                                            'cell.BorderWidthBottom = 0
                                            tableData.AddCell(cell)
                                        Next

                                        amount20 = amount
                                        amount21 = amount1
                                        amount22 = amount2
                                        amount23 = amount3

                                        ' Level3
                                        Dim acctLevel3 = From row1 In gacctLevel2.AsEnumerable() Group row1 By g1 = New With {Key .level3 = row1.Field(Of Integer)("level3"), Key .divcode3 = row1.Field(Of String)("div_code")} Into Group Order By g1.divcode3
                                        For Each gpbylevel3 In acctLevel3
                                            Dim acctgroup3 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname3 = a.Field(Of String)("acctname"), .div_code3 = a.Field(Of String)("div_code"), .parentid3 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid3 = gpbylevel3.g1.level3 And a.div_code3 = gpbylevel3.g1.divcode3).OrderBy(Function(o) o.acctname3)
                                            gacctLevel3 = gpbylevel3.Group.CopyToDataTable

                                            amount = 0
                                            amount1 = 0
                                            amount2 = 0
                                            amount3 = 0
                                            acct1name = ""
                                            If acctgroup3.Count > 0 Then
                                                For Each acct3row In acctgroup3
                                                    acct1name = acct3row.acctname3
                                                Next
                                            End If
                                            For Each row In gpbylevel3.Group
                                                If row("state") >= 3 Then
                                                    If Not (TypeOf row("amount") Is DBNull) Then
                                                        amount = amount + Decimal.Parse(row("amount"))
                                                    Else
                                                        amount = amount + 0.0
                                                    End If

                                                    If Not (TypeOf row("amount1") Is DBNull) Then
                                                        amount1 = amount1 + Decimal.Parse(row("amount1"))
                                                    Else
                                                        amount1 = amount1 + 0.0
                                                    End If

                                                    If Not (TypeOf row("amount2") Is DBNull) Then
                                                        amount2 = amount2 + Decimal.Parse(row("amount2"))
                                                    Else
                                                        amount2 = amount2 + 0.0
                                                    End If
                                                    If Not (TypeOf row("amount3") Is DBNull) Then
                                                        amount3 = amount3 + Decimal.Parse(row("amount3"))
                                                    Else
                                                        amount3 = amount3 + 0.0
                                                    End If

                                                End If

                                            Next

                                            arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
                                            For i = 0 To arrHeaders.Length - 1
                                                phrase = New Phrase()
                                                phrase.Add(New Chunk(arrHeaders(i), Level3font))
                                                If i = 0 Then
                                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                                Else
                                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                                End If
                                                cell.SetLeading(12, 0)
                                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                                cell.PaddingBottom = 4.0F
                                                cell.PaddingTop = 1.0F
                                                cell.Border = 0
                                                cell.BorderWidthLeft = 0.5
                                                cell.BorderWidthRight = 0.5
                                                cell.BackgroundColor = BaseColor.LIGHT_GRAY
                                                tableData.AddCell(cell)
                                            Next


                                            'level4
                                            Dim acctLevel4 = From row1 In gacctLevel3.AsEnumerable() Group row1 By g1 = New With {Key .level4 = row1.Field(Of Integer)("level4"), Key .divcode4 = row1.Field(Of String)("div_code")} Into Group Order By g1.divcode4

                                            For Each gpbylevel4 In acctLevel4
                                                Dim acctgroup4 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname4 = a.Field(Of String)("acctname"), .div_code4 = a.Field(Of String)("div_code"), .parentid4 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid4 = gpbylevel4.g1.level4 And a.div_code4 = gpbylevel4.g1.divcode4).OrderBy(Function(o) o.acctname4)
                                                gacctLevel4 = gpbylevel4.Group.CopyToDataTable

                                                If acctgroup4.Count > 0 Then
                                                    amount = 0
                                                    amount1 = 0
                                                    amount2 = 0
                                                    amount3 = 0
                                                    acct1name = ""
                                                    For Each acct4row In acctgroup4
                                                        acct1name = acct4row.acctname4
                                                    Next
                                                Else
                                                    acct1name = ""
                                                End If
                                                For Each row In gpbylevel4.Group
                                                    If row("state") >= 4 Then
                                                        If Not (TypeOf row("amount") Is DBNull) Then
                                                            amount = amount + Decimal.Parse(row("amount"))
                                                        Else
                                                            amount = amount + 0.0
                                                        End If

                                                        If Not (TypeOf row("amount1") Is DBNull) Then
                                                            amount1 = amount1 + Decimal.Parse(row("amount1"))
                                                        Else
                                                            amount1 = amount1 + 0.0
                                                        End If

                                                        If Not (TypeOf row("amount2") Is DBNull) Then
                                                            amount2 = amount2 + Decimal.Parse(row("amount2"))
                                                        Else
                                                            amount2 = amount2 + 0.0
                                                        End If
                                                        If Not (TypeOf row("amount3") Is DBNull) Then
                                                            amount3 = amount3 + Decimal.Parse(row("amount3"))
                                                        Else
                                                            amount3 = amount3 + 0.0
                                                        End If

                                                    End If

                                                Next

                                                arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
                                                For i = 0 To arrHeaders.Length - 1
                                                    phrase = New Phrase()
                                                    phrase.Add(New Chunk(arrHeaders(i), Level4font))
                                                    If i = 0 Then
                                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                                    Else
                                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                                    End If
                                                    cell.SetLeading(12, 0)
                                                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                                    cell.PaddingBottom = 4.0F
                                                    cell.PaddingTop = 1.0F
                                                    cell.Border = 0
                                                    cell.BorderWidthLeft = 0.5
                                                    cell.BorderWidthRight = 0.5
                                                    cell.BackgroundColor = New BaseColor(220, 220, 220)


                                                    tableData.AddCell(cell)
                                                Next

                                                'level5

                                                Dim acctLevel5 = From row1 In gacctLevel4.AsEnumerable() Group row1 By g1 = New With {Key .level5 = row1.Field(Of Integer)("level5"), Key .divcode5 = row1.Field(Of String)("div_code")} Into Group Order By g1.divcode5

                                                For Each gpbylevel5 In acctLevel5
                                                    Dim acctgroup5 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname5 = a.Field(Of String)("acctname"), .div_code5 = a.Field(Of String)("div_code"), .parentid5 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid5 = gpbylevel5.g1.level5 And a.div_code5 = gpbylevel5.g1.divcode5).OrderBy(Function(o) o.acctname5)
                                                    'gacctLevel5 = gpbylevel5.Group.CopyToDataTable
                                                    If acctgroup5.Count > 0 Then
                                                        amount = 0
                                                        amount1 = 0
                                                        amount2 = 0
                                                        amount3 = 0
                                                        acct1name = ""
                                                        For Each acct5row In acctgroup5
                                                            acct1name = acct5row.acctname5
                                                        Next
                                                    Else
                                                        acct1name = ""
                                                    End If

                                                    For Each row In gpbylevel5.Group
                                                        If row("state") >= 5 Then
                                                            If Not (TypeOf row("amount") Is DBNull) Then
                                                                amount = amount + Decimal.Parse(row("amount"))
                                                            Else
                                                                amount = amount + 0.0
                                                            End If

                                                            If Not (TypeOf row("amount1") Is DBNull) Then
                                                                amount1 = amount1 + Decimal.Parse(row("amount1"))
                                                            Else
                                                                amount1 = amount1 + 0.0
                                                            End If

                                                            If Not (TypeOf row("amount2") Is DBNull) Then
                                                                amount2 = amount2 + Decimal.Parse(row("amount2"))
                                                            Else
                                                                amount2 = amount2 + 0.0
                                                            End If
                                                            If Not (TypeOf row("amount3") Is DBNull) Then
                                                                amount3 = amount3 + Decimal.Parse(row("amount3"))
                                                            Else
                                                                amount3 = amount3 + 0.0
                                                            End If

                                                        End If

                                                    Next

                                                    arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
                                                    For i = 0 To arrHeaders.Length - 1
                                                        phrase = New Phrase()
                                                        phrase.Add(New Chunk(arrHeaders(i), Level5font))
                                                        If i = 0 Then
                                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                                        Else
                                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                                        End If
                                                        cell.SetLeading(12, 0)
                                                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                                        cell.PaddingBottom = 4.0F
                                                        cell.PaddingTop = 1.0F
                                                        cell.Border = 0
                                                        cell.BorderWidthLeft = 0.5
                                                        cell.BorderWidthRight = 0.5
                                                        tableData.AddCell(cell)
                                                    Next
                                                    'Else
                                                    '    arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
                                                    '    For i = 0 To arrHeaders.Length - 1
                                                    '        phrase = New Phrase()
                                                    '        phrase.Add(New Chunk(arrHeaders(i), Level5font))
                                                    '        If i = 0 Then
                                                    '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                                    '        Else
                                                    '            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                                    '        End If
                                                    '        cell.SetLeading(12, 0)
                                                    '        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                                    '        cell.PaddingBottom = 4.0F
                                                    '        cell.PaddingTop = 1.0F
                                                    '        tableData.AddCell(cell)
                                                    '    Next
                                                    'End If
                                                Next
                                                'End level 5

                                                'Else
                                                '    arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
                                                '    For i = 0 To arrHeaders.Length - 1
                                                '        phrase = New Phrase()
                                                '        phrase.Add(New Chunk(arrHeaders(i), Level4font))
                                                '        If i = 0 Then
                                                '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                                '        Else
                                                '            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                                '        End If
                                                '        cell.SetLeading(12, 0)
                                                '        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                                '        cell.PaddingBottom = 4.0F
                                                '        cell.PaddingTop = 1.0F
                                                '        tableData.AddCell(cell)
                                                '    Next

                                                'End If
                                            Next
                                            'End Level 4


                                            'Else

                                            '    arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
                                            '    For i = 0 To arrHeaders.Length - 1
                                            '        phrase = New Phrase()
                                            '        phrase.Add(New Chunk(arrHeaders(i), Level3font))
                                            '        If i = 0 Then
                                            '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                            '        Else
                                            '            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                            '        End If
                                            '        cell.SetLeading(12, 0)
                                            '        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                            '        cell.PaddingBottom = 4.0F
                                            '        cell.PaddingTop = 1.0F
                                            '        tableData.AddCell(cell)
                                            '    Next

                                            'End If
                                        Next
                                        'end level 3

                                        'Else

                                        '    arrHeaders = {"", amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
                                        '    For i = 0 To arrHeaders.Length - 1
                                        '        phrase = New Phrase()
                                        '        phrase.Add(New Chunk(arrHeaders(i), Level2font))
                                        '        If i = 0 Then
                                        '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                        '        Else
                                        '            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                        '        End If
                                        '        cell.SetLeading(12, 0)
                                        '        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                        '        cell.PaddingBottom = 4.0F
                                        '        cell.PaddingTop = 1.0F
                                        '        tableData.AddCell(cell)
                                        '    Next

                                        'End If


                                        If String.Equals(acct2name, "Income") Then
                                            acct2name = "Total Revenue"
                                        ElseIf String.Equals(acct2name, "Cost Of Sales") Then
                                            acct2name = "Total Cost Of Sales"
                                        ElseIf String.Equals(acct2name, "Expenses") Then
                                            acct2name = "Other Direct Costs"

                                        Else
                                            acct2name = ""
                                        End If

                                        arrHeaders = {acct2name, amount20.ToString(decno), amount21.ToString(decno), amount22.ToString(decno), amount23.ToString(decno)}
                                        For i = 0 To arrHeaders.Length - 1
                                            phrase = New Phrase()
                                            phrase.Add(New Chunk(arrHeaders(i), Level4font))
                                            If i = 0 Then
                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                            Else
                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                                            End If
                                            cell.SetLeading(12, 0)
                                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                                            cell.PaddingBottom = 4.0F
                                            cell.PaddingTop = 1.0F
                                            cell.Border = 0


                                            cell.BorderWidthLeft = 0.5
                                            cell.BorderWidthRight = 0.5
                                            tableData.AddCell(cell)
                                        Next
                                    Next

                                    'End level 2
                                Next
                                'End level 1
                            Next

                        Next

                        arrHeaders = {"NET PROFIT", (income - expanse - costofsales).ToString(decno), (income1 - expanse1 - costofsales1).ToString(decno), (income2 - expanse2 - costofsales2).ToString(decno), (income3 - expanse3 - costofsales3).ToString(decno)}
                        For i = 0 To arrHeaders.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrHeaders(i), Level3font))
                            If i = 0 Then
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            Else
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                            End If

                            cell.SetLeading(12, 0)
                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.BorderWidth = 0
                            cell.BorderWidthTop = 1
                            cell.BorderWidthBottom = 1


                            tableData.AddCell(cell)
                        Next

                        document.Add(tableData)
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
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    'Public Sub SupplierStament(ByVal custdetailsdt As DataTable, ByVal dt As DataTable, ByVal agents As DataTable, ByVal detailsdt As DataTable, ByVal fromdate As String, ByVal todate As String, ByVal datetype As String, ByVal agingtype As String, ByVal Type As String, ByRef pdfdoc As Document, ByVal currflg As Integer)

    '    Dim phrase As Phrase = Nothing
    '    Dim cell As PdfPCell = Nothing
    '    Dim currDecno As Integer

    '    custdetailsdt.DefaultView.Sort = "acc_code ASC"
    '    custdetailsdt = custdetailsdt.DefaultView.ToTable
    '    'If detailsdt.Rows.Count > 0 Then
    '    '    age1 = Convert.ToDecimal(detailsdt.Compute("Sum(age1)", ""))
    '    '    age2 = Convert.ToDecimal(detailsdt.Compute("Sum(age2)", ""))
    '    '    age3 = Convert.ToDecimal(detailsdt.Compute("Sum(age3)", ""))
    '    '    age4 = Convert.ToDecimal(detailsdt.Compute("Sum(age4)", ""))
    '    '    age5 = Convert.ToDecimal(detailsdt.Compute("Sum(age5)", ""))
    '    '    age6 = Convert.ToDecimal(detailsdt.Compute("Sum(age6)", ""))
    '    '    age1bal = Convert.ToDecimal(detailsdt.Compute("Sum(balance)", ""))
    '    'End If
    '    For Each Customer_Statement In custdetailsdt.Rows

    '        Dim decCurreccy As String
    '        '07/01/2019

    '        currency = Customer_Statement("currcode").ToString()
    '        If currflg = 0 Then
    '            decCurreccy = currency
    '        Else
    '            decCurreccy = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
    '        End If
    '        currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & decCurreccy & "'"), Integer)

    '        'currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & decCurreccy & "'"), Integer)


    '        'Commented by Tanvir currency was showing usd 06/1/2019
    '        'If currflg = 0 Then
    '        '    currency = "USD"
    '        'Else
    '        '    Dim c As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
    '        '    currency = c

    '        'End If
    '        'currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currency & "'"), Integer)

    '        'Dim row() As DataRow = custdetailsdt.Select("trantype<>'PR'")

    '        Dim trantypep = Customer_Statement("trantype").ToString()

    '        ' If Not (String.Equals(trantypep, "PR")) Then
    '        Dim acccode As String = Customer_Statement("acc_code").ToString()
    '        Dim accname As String = Customer_Statement("accname").ToString()
    '        Dim crlimit As String = Customer_Statement("crlimit").ToString()
    '        Dim totaldebit, totalcredit, debits, credits As Decimal
    '        'Dim debit, credit, fdebit, fcredit, fbalance, mdebit, mcredit, mbalance, cumbal, totalbalance As Decimal
    '        Dim k, r As Integer
    '        Dim dr() As System.Data.DataRow
    '        Dim dr1() As System.Data.DataRow
    '        Dim agentdet() As System.Data.DataRow
    '        dr1 = dt.Select("acc_code='" & acccode & "'")

    '        Dim conn As New SqlConnection
    '        Dim SqlCmd As New SqlCommand
    '        Dim DataAdapter As New SqlDataAdapter
    '        Dim ds2 As New DataSet
    '        conn = clsDBConnect.dbConnectionnew("strDBConnection")
    '        SqlCmd = New SqlCommand("select Hotel_Account_Name, Hotel_Account_Number, Hotel_Account_Banck_Name, " +
    '             " Hotel_Account_Branch_Name,Hotel_Account_SWIFT, Hotel_Account_IBAN from partymast where partycode = '" & acccode & "'", conn)
    '        SqlCmd.CommandType = CommandType.Text

    '        SqlCmd.CommandTimeout = 0
    '        DataAdapter.SelectCommand = SqlCmd
    '        DataAdapter.Fill(ds2)
    '        Dim bankdetails As New DataTable
    '        bankdetails = ds2.Tables(0)

    '        Dim supplierAccount As New SupplierAccount
    '        Dim bankDetail As DataRow = bankdetails.Rows(0)

    '        If Not IsDBNull(bankDetail("Hotel_Account_Name")) Then
    '            supplierAccount.AccName = bankDetail("Hotel_Account_Name").ToString()
    '        End If

    '        If Not IsDBNull(bankDetail("Hotel_Account_Number")) Then
    '            supplierAccount.AccNo = bankDetail("Hotel_Account_Number").ToString()
    '        End If

    '        If Not IsDBNull(bankDetail("Hotel_Account_Banck_Name")) Then
    '            supplierAccount.BankName = bankDetail("Hotel_Account_Banck_Name").ToString()
    '        End If

    '        If Not IsDBNull(bankDetail("Hotel_Account_Branch_Name")) Then
    '            supplierAccount.BranchName = bankDetail("Hotel_Account_Branch_Name").ToString()
    '        End If

    '        If Not IsDBNull(bankDetail("Hotel_Account_IBAN")) Then
    '            supplierAccount.Iban = bankDetail("Hotel_Account_IBAN").ToString()
    '        End If

    '        If Not IsDBNull(bankDetail("Hotel_Account_SWIFT")) Then
    '            supplierAccount.SwiftCode = bankDetail("Hotel_Account_SWIFT").ToString()
    '        End If



    '        If dr1.Length = 0 Then
    '            dt.Rows.Add(acccode)
    '            dr = custdetailsdt.Select("acc_code='" & acccode & "'")
    '            agentdet = agents.Select("agentcode='" & acccode & "'")
    '            Dim dr3() As System.Data.DataRow
    '            '  dr3 = detailsdt.Select("acc_code='" & acccode & "'")

    '            Dim overdue, overbal, overcr As Decimal

    '            Dim gpbyMonth As DataTable
    '            Dim gpby As DataTable
    '            Dim arrow3() As String = Nothing
    '            Dim tdate As String = Nothing
    '            Dim arr3index As Integer
    '            Dim cumBalance As Decimal
    '            Dim mon As String = Format(Convert.ToDateTime(dr(0)("trandate").ToString()), "MM")

    '            If dr.Length > 0 Then
    '                '  Dim groups1 = From custledger In dr.AsEnumerable() Group custledger By g = New With {Key .acccode = custledger.Field(Of String)("acc_code"), Key .gl_code = custledger.Field(Of String)("acc_gl_code"), Key .accname = custledger.Field(Of String)("accname")} Into Group Order By g.acccode
    '                Dim groups1 = From gpbyrow In dr.AsEnumerable() Group gpbyrow By g = New With {Key .acccode = gpbyrow.Field(Of String)("acc_code"), Key .gl_code = gpbyrow.Field(Of String)("acc_gl_code"), Key .accname = gpbyrow.Field(Of String)("accname")} Into Group Order By g.acccode

    '                For Each gpdata1 In groups1
    '                    gpbyMonth = gpdata1.Group.CopyToDataTable()

    '                    Dim dr4 = detailsdt.AsEnumerable().Where(Function(s) s.Field(Of String)("acc_code") = gpdata1.g.acccode And s.Field(Of String)("acc_gl_code") = gpdata1.g.gl_code)
    '                    Dim debit_t As Object = gpbyMonth.Compute("SUM(debit)", "trantype<>'OB'")
    '                    Dim sumdebit As Decimal
    '                    If Not debit_t.Equals(DBNull.Value) Then
    '                        sumdebit = Convert.ToDecimal(gpbyMonth.Compute("SUM(debit)", "trantype<>'OB'"))
    '                    Else
    '                        sumdebit = 0
    '                    End If
    '                    Dim credit_t As Object = gpbyMonth.Compute("SUM(credit)", "trantype<>'OB'")
    '                    Dim sumCredit As Decimal
    '                    If Not credit_t.Equals(DBNull.Value) Then
    '                        sumCredit = Convert.ToDecimal(gpbyMonth.Compute("SUM(credit)", "trantype<>'OB'"))
    '                    Else
    '                        sumCredit = 0
    '                    End If
    '                    acccode = gpbyMonth(0)("acc_code")
    '                    agebalance = sumCredit - sumdebit

    '                    agentdet = agents.Select("agentcode='" & acccode & "'")

    '                    If agentdet.Length > 0 Then
    '                        'If agentdet(0)("crlimit") Then
    '                        '    overbal = agebalance
    '                        '    overcr = Decimal.Parse(agentdet(0)("crlimit"))
    '                        '    overdue = Decimal.Subtract(overcr, overbal)
    '                        '    contact1 = agentdet(0)("contact1").ToString()
    '                        '    acrlimit = Decimal.Parse(agentdet(0)("crlimit"))
    '                        '    cdays = Decimal.Parse(agentdet(0)("crdays"))
    '                        '    'Else
    '                        '    '    overdue = Decimal.Parse(dr3(0)("balance"))
    '                        'End If
    '                    Else
    '                        overbal = agebalance
    '                        overcr = 0
    '                        overdue = Decimal.Subtract(overcr, overbal)
    '                        contact1 = String.Empty
    '                        crlimit = 0
    '                        cdays = 0
    '                    End If
    '                    totalcredit = 0
    '                    totaldebit = 0
    '                    totalbalances = 0
    '                    Dim trantypes As String
    '                    Dim logo As PdfPTable = Nothing
    '                    logo = New PdfPTable(1)
    '                    logo.TotalWidth = documentWidth
    '                    logo.LockedWidth = True
    '                    logo.SetWidths(New Single() {1.0F})
    '                    logo.Complete = False
    '                    logo.SplitRows = False
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk("Printed Date : " + DateTime.Now.ToString("dd/MM/yyyy"), NormalFont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
    '                    logo.AddCell(cell)
    '                    'company name
    '                    cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
    '                    cell.Colspan = 2
    '                    cell.SetLeading(12, 0)
    '                    cell.PaddingBottom = 4
    '                    logo.AddCell(cell)
    '                    pdfdoc.Add(logo)

    '                    Dim tblTitle As PdfPTable = New PdfPTable(1)
    '                    tblTitle.SetWidths(New Single() {1.0F})
    '                    tblTitle.TotalWidth = documentWidth
    '                    tblTitle.LockedWidth = True
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk(custSupType & Environment.NewLine & vbLf & reportfilter, TitleFont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                    cell.PaddingBottom = 3.0F
    '                    tblTitle.AddCell(cell)
    '                    tblTitle.SpacingBefore = 7
    '                    pdfdoc.Add(tblTitle)
    '                    Dim tblcommon As PdfPTable = New PdfPTable(2)
    '                    tblcommon.SetWidths(New Single() {0.5F, 0.5F})
    '                    tblcommon.TotalWidth = documentWidth
    '                    tblcommon.LockedWidth = True
    '                    Dim tbl As PdfPTable = New PdfPTable(1)
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk("TO" + "           " + ":" + Space(12) + Customer_Statement("acc_code").ToString() + " " + Customer_Statement("accname").ToString() + Environment.NewLine, NormalFont))
    '                    phrase.Add(New Chunk("DATE" + "       " + ":" + Space(12) + Format(Convert.ToDateTime(todate), "dd/MM/yyyy") + Environment.NewLine, NormalFont))
    '                    phrase.Add(New Chunk("ATTN" + "       " + ":" + Space(12) + contact1 + Environment.NewLine, NormalFont))
    '                    phrase.Add(New Chunk("TEL" & "          " & ":" & Space(12) & Customer_Statement("tel1").ToString() & "                  ", NormalFont))
    '                    phrase.Add(New Chunk("FAX" & Space(11) & ":" & Space(12) & Customer_Statement("fax").ToString() & Environment.NewLine, NormalFont))
    '                    phrase.Add(New Chunk("           " & Space(18) & Customer_Statement("tel2").ToString() & Environment.NewLine, NormalFont))


    '                    phrase.Add(New Chunk("ACCOUNT NAME" + "   " + ":" + Space(6) + supplierAccount.AccName + Environment.NewLine, NormalFont))
    '                    phrase.Add(New Chunk("ACCOUNT NO" + "        " + ":" + Space(6) + supplierAccount.AccNo & Space(12), NormalFont))
    '                    phrase.Add(New Chunk("BANK NAME" + "   " + ":" + Space(6) + supplierAccount.BankName + ", " + supplierAccount.BranchName + Environment.NewLine, NormalFont))
    '                    phrase.Add(New Chunk("SWIFT CODE" & "          " & ":" & Space(6) & supplierAccount.SwiftCode & Space(12), NormalFont))
    '                    phrase.Add(New Chunk("IBAN" & "   " & ":" & Space(12) & supplierAccount.Iban & Environment.NewLine, NormalFont))
    '                    phrase.Add(New Chunk("           " & Space(18) & Customer_Statement("tel2").ToString() & "                  ", NormalFont))

    '                    'phrase.Add(New Chunk("ACCOUNT NAME" + "           " + ":" + Space(12) + supplierAccount.AccName + Environment.NewLine & vbLf, NormalFont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                    cell.PaddingBottom = 3.0F
    '                    tbl.AddCell(cell)
    '                    tblcommon.AddCell(tbl)



    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk("Credit Limit" & "      " & ":" & Space(5) & acrlimit.ToString() + Environment.NewLine & vbLf, NormalFont))
    '                    phrase.Add(New Chunk("Credit Days" & "      " & ":" & Space(5) & cdays.ToString() + Environment.NewLine & vbLf, NormalFont))
    '                    phrase.Add(New Chunk("OverDue" & "          " & ":" & Space(5) & overdue.ToString("N" + currDecno.ToString) + Environment.NewLine & vbLf, NormalFont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                    cell.SetLeading(12, 0)
    '                    cell.PaddingLeft = 250.0F
    '                    tblcommon.AddCell(cell)
    '                    tblcommon.SpacingBefore = 7
    '                    pdfdoc.Add(tblcommon)


    '                    Dim desc As PdfPTable = New PdfPTable(2)
    '                    desc.SetWidths(New Single() {0.82F, 0.18F})
    '                    desc.TotalWidth = documentWidth
    '                    desc.LockedWidth = True
    '                    phrase = New Phrase()
    '                    If datetype <> 0 Then
    '                        phrase.Add(New Chunk("Please Find the up to date statement of Account between " + Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy") + " and " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy"), BalFont))
    '                    Else
    '                        phrase.Add(New Chunk("Please Find the up to date statement of Account as on " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy"), BalFont))
    '                    End If
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
    '                    cell.PaddingBottom = 3.0F
    '                    desc.AddCell(cell)
    '                    phrase = New Phrase()
    '                    Dim curr As PdfPTable = New PdfPTable(1)
    '                    phrase.Add(New Chunk("Currency" + "         " + ":" + Space(7) + currency, NormalFont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
    '                    cell.PaddingBottom = 3.0F
    '                    curr.AddCell(cell)
    '                    desc.AddCell(curr)
    '                    desc.SpacingBefore = 7
    '                    pdfdoc.Add(desc)


    '                    Dim balance As PdfPTable = New PdfPTable(1)
    '                    balance.SetWidths(New Single() {1.0F})
    '                    balance.TotalWidth = documentWidth
    '                    balance.LockedWidth = True
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk("We would appreciate if you could settle the balance due at the earliest", BalFont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
    '                    cell.PaddingBottom = 3.0F
    '                    balance.AddCell(cell)
    '                    balance.SpacingBefore = 7
    '                    pdfdoc.Add(balance)

    '                    Dim maintbl As PdfPTable = New PdfPTable(10)
    '                    Dim arrow2() As String

    '                    ' maintbl = New PdfPTable(10)
    '                    maintbl.SetWidths(New Single() {0.075F, 0.045F, 0.09F, 0.07F, 0.08F, 0.34F, 0.1F, 0.1F, 0.1F, 0.1F})
    '                    maintbl.TotalWidth = documentWidth
    '                    maintbl.LockedWidth = True
    '                    arrow2 = {"TRAN DATE", "TYPE", "Doc No", "Booking ID.", "SUP INV NO/TICKET NO",
    '                                              "Description", "DEBIT", "CREDIT", "BALANCE", "CUMBAL"}

    '                    For i = 0 To arrow2.Length - 1
    '                        phrase = New Phrase()
    '                        phrase.Add(New Chunk(arrow2(i), HeadingFont))
    '                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
    '                        cell.SetLeading(12, 0)
    '                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
    '                        cell.PaddingBottom = 4.0F
    '                        cell.PaddingTop = 1.0F
    '                        maintbl.AddCell(cell)
    '                    Next

    '                    maintbl.SpacingBefore = 7
    '                    pdfdoc.Add(maintbl)

    '                    Dim group1 As IEnumerable(Of IGrouping(Of Integer, DataRow)) = gpbyMonth.AsEnumerable().GroupBy(Function(g) g.Field(Of Date)("trandate").Month)

    '                    finalcredit = 0
    '                    finaldebit = 0
    '                    For Each gpdata In group1
    '                        Dim perticulars As String

    '                        'gpdata.Select 
    '                        gpby = gpdata.CopyToDataTable()


    '                        For Each row As DataRow In gpdata
    '                            Dim data As PdfPTable = New PdfPTable(10)
    '                            data.SetWidths(New Single() {0.075F, 0.045F, 0.09F, 0.07F, 0.08F, 0.34F, 0.1F, 0.1F, 0.1F, 0.1F})
    '                            data.TotalWidth = documentWidth
    '                            data.LockedWidth = True
    '                            trantypes = row("trantype").ToString()
    '                            If Not String.IsNullOrEmpty(trantypes) Then
    '                                finalcredit = Decimal.Parse(row("credit")) + finalcredit
    '                                finaldebit = Decimal.Parse(row("debit")) + finaldebit
    '                                debits = IIf(trantypes = "OB", 0, Decimal.Parse(row("debit")))
    '                                credits = IIf(trantypes = "OB", 0, Decimal.Parse(row("credit")))
    '                                totalcredit = totalcredit + credits
    '                                totaldebit = totaldebit + debits
    '                                totalbalances = totalbalances + (credits - debits)
    '                                cumBalance = cumBalance + (credits - debits)

    '                                Dim docno As String = IIf(row("incinvoiceno") = "", row("tranid").ToString(), row("incinvoiceno").ToString())
    '                                tdate = Format(Convert.ToDateTime(row("trandate").ToString()), "dd/MM/yyyy")
    '                                If row("particulars").ToString().Contains(row("incinvdesc").ToString()) Then
    '                                    perticulars = row("particulars").ToString()
    '                                Else
    '                                    perticulars = row("particulars").ToString() & Environment.NewLine & vbLf & row("incinvdesc").ToString()
    '                                End If
    '                                arrow3 = {tdate, row("trantype").ToString(), docno, row("fileno").ToString(), row("reconfno").ToString(), perticulars, IIf(debits = 0, "-", debits.ToString("N" + currDecno.ToString)), IIf(credits = 0, "-", credits.ToString("N" + currDecno.ToString)), (credits - debits).ToString("N" + currDecno.ToString()), IIf(cumBalance <= 0, "(" & Decimal.Parse((Math.Abs(cumBalance))).ToString("N" + currDecno.ToString) & ")", cumBalance.ToString("N" + currDecno.ToString))}
    '                                For k = 0 To arrow3.Length - 1
    '                                    phrase = New Phrase()
    '                                    phrase.Add(New Chunk(arrow3(k), datafont))

    '                                    If k = 0 Or k < 5 Then
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                    ElseIf k = 5 Then
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                    Else
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
    '                                    End If
    '                                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                                    cell.SetLeading(12, 0)
    '                                    cell.PaddingBottom = 4.0F
    '                                    cell.PaddingTop = 1.0F
    '                                    data.AddCell(cell)
    '                                Next
    '                                data.SpacingBefore = 0
    '                                pdfdoc.Add(data)
    '                            End If
    '                        Next

    '                        Dim mtotal As PdfPTable = New PdfPTable(6)
    '                        Dim monthtotal() As String
    '                        mtotal.SetWidths(New Single() {0.55F, 0.15F, 0.1F, 0.1F, 0.1F, 0.1F})
    '                        mtotal.TotalWidth = documentWidth
    '                        mtotal.LockedWidth = True
    '                        monthtotal = {"", "MONTH TOTAL", totaldebit.ToString("N" + currDecno.ToString), totalcredit.ToString("N" + currDecno.ToString), totalbalances.ToString("N" + currDecno.ToString), ""}
    '                        For k = 0 To monthtotal.Length - 1
    '                            phrase = New Phrase()
    '                            phrase.Add(New Chunk(monthtotal(k), datafont))
    '                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
    '                            cell.SetLeading(12, 0)
    '                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                            If k <> 0 Then
    '                                cell.BackgroundColor = basecolor
    '                            End If
    '                            cell.PaddingBottom = 4.0F
    '                            cell.PaddingTop = 1.0F
    '                            mtotal.AddCell(cell)
    '                        Next
    '                        mtotal.SpacingBefore = 0
    '                        pdfdoc.Add(mtotal)

    '                    Next

    '                    Dim Final As PdfPTable = New PdfPTable(5)
    '                    Final.SetWidths(New Single() {0.55F, 0.15F, 0.1F, 0.1F, 0.2F})
    '                    Final.TotalWidth = documentWidth
    '                    Final.LockedWidth = True
    '                    Dim finalTotal() As String = {"", "Final Total", finaldebit.ToString("N" + currDecno.ToString), finalcredit.ToString("N" + currDecno.ToString), ""}
    '                    For k = 0 To finalTotal.Length - 1
    '                        phrase = New Phrase()
    '                        phrase.Add(New Chunk(finalTotal(k), datafontBold))
    '                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
    '                        cell.SetLeading(12, 0)
    '                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                        cell.PaddingBottom = 4.0F
    '                        cell.PaddingTop = 1.0F
    '                        Final.AddCell(cell)
    '                    Next
    '                    Final.SpacingBefore = 7
    '                    pdfdoc.Add(Final)

    '                    Dim one As String, two As String, three As String, four As String, five As String, six As String
    '                    If agingtype = 0 Then
    '                        If Month = "01" Then
    '                            one = "JAN"
    '                            two = "DEC"
    '                            three = "NOV"
    '                            four = "OCT"
    '                            five = "SEP"
    '                            six = "<=AUG"
    '                        ElseIf Month = "02" Then
    '                            one = "FEB"
    '                            two = "JAN"
    '                            three = "DEC"
    '                            four = "NOV"
    '                            five = "OCT"
    '                            six = "<=SEP"
    '                        ElseIf Month = "03" Then
    '                            one = "MAR"
    '                            two = "FEB"
    '                            three = "JAN"
    '                            four = "DEC"
    '                            five = "NOV"
    '                            six = "<=OCT"
    '                        ElseIf Month = "04" Then
    '                            one = "APR"
    '                            two = "MAR"
    '                            three = "FEB"
    '                            four = "JAN"
    '                            five = "DEC"
    '                            six = "<=NOV"
    '                        ElseIf Month = "05" Then
    '                            one = "MAY"
    '                            two = "APR"
    '                            three = "MAR"
    '                            four = "FEB"
    '                            five = "JAN"
    '                            six = "<=DEC"
    '                        ElseIf Month = "06" Then
    '                            one = "JUN"
    '                            two = "MAY"
    '                            three = "APR"
    '                            four = "MAR"
    '                            five = "FEB"
    '                            six = "<=JAN"
    '                        ElseIf Month = "07" Then
    '                            one = "JUL"
    '                            two = "JUN"
    '                            three = "MAY"
    '                            four = "APR"
    '                            five = "MAR"
    '                            six = "<=FEB"
    '                        ElseIf Month = "08" Then
    '                            one = "AUG"
    '                            two = "JUL"
    '                            three = "JUN"
    '                            four = "MAY"
    '                            five = "APR"
    '                            six = "<=MAR"
    '                        ElseIf Month = "09" Then
    '                            one = "SEP"
    '                            two = "AUG"
    '                            three = "JUL"
    '                            four = "JUN"
    '                            five = "MAY"
    '                            six = "<=APR"
    '                        ElseIf Month = "10" Then
    '                            one = "OCT"
    '                            two = "SEP"
    '                            three = "AUG"
    '                            four = "JUL"
    '                            five = "JUN"
    '                            six = "<=MAY"
    '                        ElseIf Month = "11" Then
    '                            one = "NOV"
    '                            two = "OCT"
    '                            three = "SEP"
    '                            four = "AUG"
    '                            five = "JUL"
    '                            six = "<=JUN"
    '                        ElseIf Month = "12" Then
    '                            one = "DEC"
    '                            two = "NOV"
    '                            three = "OCT"
    '                            four = "SEP"
    '                            five = "AUG"
    '                            six = "<=JUL"
    '                        End If
    '                    Else
    '                        one = "<=0-30"
    '                        two = "<=31-60"
    '                        three = "<=61-90"
    '                        four = "<=91-120"
    '                        five = "<=121-150"
    '                        six = "Over 150"
    '                    End If
    '                    Dim tbldetail As PdfPTable = New PdfPTable(1)
    '                    tbldetail.SetWidths(New Single() {1.0F})
    '                    tbldetail.TotalWidth = documentWidth
    '                    tbldetail.LockedWidth = True
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk("Ageing Analysis Of Balance", NormalFontBold))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                    cell.PaddingBottom = 3.0F
    '                    tbldetail.AddCell(cell)
    '                    tbldetail.SpacingBefore = 30
    '                    pdfdoc.Add(tbldetail)

    '                    Dim detailtbl As PdfPTable = New PdfPTable(8)
    '                    detailtbl.SetWidths(New Single() {0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F})
    '                    detailtbl.TotalWidth = documentWidth
    '                    detailtbl.LockedWidth = True
    '                    Dim arrow4() As String = {"BALANCE", "<0", one, two, three, four, five, six}
    '                    For i = 0 To 7
    '                        phrase = New Phrase()
    '                        phrase.Add(New Chunk(arrow4(i), HeadingFont))
    '                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                        cell.SetLeading(12, 0)
    '                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                        cell.PaddingBottom = 4.0F
    '                        cell.PaddingTop = 1.0F
    '                        detailtbl.AddCell(cell)
    '                    Next
    '                    detailtbl.SpacingBefore = 0
    '                    pdfdoc.Add(detailtbl)

    '                    Dim arr7() As Decimal
    '                    ' arr7 = {age1bal, age9, age1, age2, age3, age4, age5, age6}
    '                    arr7 = {dr4(0)("balance"), dr4(0)("age9"), dr4(0)("age1"), dr4(0)("age2"), dr4(0)("age3"), dr4(0)("age4"),
    '                                     dr4(0)("age5"), dr4(0)("age6")}

    '                    Dim tbldata As PdfPTable = New PdfPTable(8)
    '                    tbldata.SetWidths(New Single() {0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F})
    '                    tbldata.TotalWidth = documentWidth
    '                    tbldata.LockedWidth = True
    '                    For i = 0 To 7
    '                        phrase = New Phrase()
    '                        phrase.Add(New Chunk(arr7(i).ToString("N" + currDecno.ToString), datafont))
    '                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                        cell.SetLeading(12, 0)
    '                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                        cell.PaddingBottom = 4.0F
    '                        cell.PaddingTop = 1.0F
    '                        tbldata.AddCell(cell)
    '                    Next
    '                    tbldata.SpacingBefore = 0
    '                    pdfdoc.Add(tbldata)
    '                    finalcredit = 0
    '                    finaldebit = 0
    '                    cumBalance = 0
    '                    agebalance = 0
    '                    ' pdfdoc.NewPage()
    '                    'age9 = 0
    '                    'age1 = 0
    '                    'age2 = 0
    '                    'age3 = 0
    '                    'age4 = 0
    '                    'age5 = 0
    '                    'age6 = 0 
    '                    pdfdoc.NewPage()
    '                Next

    '            End If
    '        End If
    '        ' End If

    '    Next

    'End Sub

    'Public Sub CreatePdf(ByVal reportsType As String, ByVal rptfilter As String, ByVal datetype As Integer, ByVal fromdate As String, ByVal todate As String, ByVal Type As String, ByVal currflg As Integer, ByVal fromacct As String, ByVal toacct As String, ByVal fromcontrol As String, ByVal tocontrol As String,
    '                     ByVal fromcat As String, ByVal tocat As String, ByVal fromcity As String, ByVal tocity As String, ByVal fromctry As String, ByVal toctry As String,
    '                                                   ByVal agingtype As Integer, ByVal pdcyesno As Integer, ByVal includezero As Integer, ByVal summdet As Integer, ByVal web As Integer, ByVal divcode As String, ByVal custgroup_sp_type As String, ByVal inclproforma As Integer, ByRef bytes() As Byte, ByRef ds1 As DataSet, ByVal rpttype As String, ByVal printMode As String, Optional ByVal fileName As String = "")
    '    Try

    '        If divcode <> "" Then
    '            rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
    '        Else
    '            rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
    '        End If
    '        Month = Format(Convert.ToDateTime(todate), "MM")
    '        Dim sqlConn As New SqlConnection
    '        Dim mySqlCmd As New SqlCommand
    '        Dim myDataAdapter As New SqlDataAdapter
    '        Dim ds As New DataSet
    '        custSupType = IIf(Type = "C", "CUSTOMER STATEMENT", IIf(Type = "S", "SUPPLIER STATEMENT", "SUPPLIER AGENT STATEMENT"))

    '        sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
    '        mySqlCmd = New SqlCommand("sp_statement_party", sqlConn)
    '        mySqlCmd.CommandType = CommandType.StoredProcedure
    '        mySqlCmd.Parameters.Add(New SqlParameter("@datetype", SqlDbType.Int)).Value = datetype
    '        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")
    '        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")
    '        mySqlCmd.Parameters.Add(New SqlParameter("@type", SqlDbType.Char)).Value = Type
    '        mySqlCmd.Parameters.Add(New SqlParameter("@currflg", SqlDbType.Int)).Value = currflg
    '        mySqlCmd.Parameters.Add(New SqlParameter("@fromacct", SqlDbType.VarChar, 20)).Value = fromacct
    '        mySqlCmd.Parameters.Add(New SqlParameter("@toacct", SqlDbType.VarChar, 20)).Value = toacct
    '        mySqlCmd.Parameters.Add(New SqlParameter("@fromcontrol", SqlDbType.VarChar, 20)).Value = fromcontrol
    '        mySqlCmd.Parameters.Add(New SqlParameter("@tocontrol", SqlDbType.VarChar, 20)).Value = tocontrol
    '        mySqlCmd.Parameters.Add(New SqlParameter("@fromcat", SqlDbType.VarChar, 20)).Value = fromcat
    '        mySqlCmd.Parameters.Add(New SqlParameter("@tocat", SqlDbType.VarChar, 20)).Value = tocat
    '        mySqlCmd.Parameters.Add(New SqlParameter("@fromcity", SqlDbType.VarChar, 20)).Value = fromcity
    '        mySqlCmd.Parameters.Add(New SqlParameter("@tocity", SqlDbType.VarChar, 20)).Value = tocity
    '        mySqlCmd.Parameters.Add(New SqlParameter("@fromctry", SqlDbType.VarChar, 20)).Value = fromctry
    '        mySqlCmd.Parameters.Add(New SqlParameter("@toctry", SqlDbType.VarChar, 20)).Value = toctry
    '        mySqlCmd.Parameters.Add(New SqlParameter("@agingtype", SqlDbType.Int)).Value = agingtype
    '        mySqlCmd.Parameters.Add(New SqlParameter("@pdcyesno", SqlDbType.Int)).Value = pdcyesno
    '        mySqlCmd.Parameters.Add(New SqlParameter("@includezero", SqlDbType.Int)).Value = includezero
    '        mySqlCmd.Parameters.Add(New SqlParameter("@web", SqlDbType.Int)).Value = web
    '        mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
    '        mySqlCmd.Parameters.Add(New SqlParameter("@custgroup_sp_type", SqlDbType.VarChar, 20)).Value = custgroup_sp_type
    '        mySqlCmd.Parameters.Add(New SqlParameter("@inclproforma", SqlDbType.Int)).Value = inclproforma
    '        mySqlCmd.CommandTimeout = 0
    '        myDataAdapter.SelectCommand = mySqlCmd
    '        myDataAdapter.Fill(ds)
    '        Dim custdetailsdt As New DataTable
    '        custdetailsdt = ds.Tables(0)

    '        Dim conn As New SqlConnection
    '        Dim SqlCmd As New SqlCommand
    '        Dim DataAdapter As New SqlDataAdapter
    '        Dim ds2 As New DataSet
    '        conn = clsDBConnect.dbConnectionnew("strDBConnection")
    '        SqlCmd = New SqlCommand("sp_statement_partyaging", conn)
    '        SqlCmd.CommandType = CommandType.StoredProcedure
    '        SqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")
    '        SqlCmd.Parameters.Add(New SqlParameter("@type", SqlDbType.Char)).Value = Type
    '        SqlCmd.Parameters.Add(New SqlParameter("@currflg", SqlDbType.Int)).Value = currflg
    '        SqlCmd.Parameters.Add(New SqlParameter("@fromacct", SqlDbType.VarChar, 20)).Value = fromacct
    '        SqlCmd.Parameters.Add(New SqlParameter("@toacct", SqlDbType.VarChar, 20)).Value = toacct
    '        SqlCmd.Parameters.Add(New SqlParameter("@fromcontrol", SqlDbType.VarChar, 20)).Value = fromcontrol
    '        SqlCmd.Parameters.Add(New SqlParameter("@tocontrol", SqlDbType.VarChar, 20)).Value = tocontrol
    '        SqlCmd.Parameters.Add(New SqlParameter("@fromcat", SqlDbType.VarChar, 20)).Value = fromcat
    '        SqlCmd.Parameters.Add(New SqlParameter("@tocat", SqlDbType.VarChar, 20)).Value = tocat
    '        SqlCmd.Parameters.Add(New SqlParameter("@fromcity", SqlDbType.VarChar, 20)).Value = fromcity
    '        SqlCmd.Parameters.Add(New SqlParameter("@tocity", SqlDbType.VarChar, 20)).Value = tocity
    '        SqlCmd.Parameters.Add(New SqlParameter("@fromctry", SqlDbType.VarChar, 20)).Value = fromctry
    '        SqlCmd.Parameters.Add(New SqlParameter("@toctry", SqlDbType.VarChar, 20)).Value = toctry
    '        SqlCmd.Parameters.Add(New SqlParameter("@agingtype", SqlDbType.Int)).Value = agingtype
    '        SqlCmd.Parameters.Add(New SqlParameter("@summdet", SqlDbType.Int)).Value = summdet
    '        SqlCmd.Parameters.Add(New SqlParameter("@web", SqlDbType.Int)).Value = web
    '        SqlCmd.Parameters.Add(New SqlParameter("@agasondate", SqlDbType.DateTime)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")
    '        SqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
    '        SqlCmd.Parameters.Add(New SqlParameter("@custgroup_sp_type", SqlDbType.VarChar, 20)).Value = custgroup_sp_type
    '        SqlCmd.Parameters.Add(New SqlParameter("@inclproforma", SqlDbType.Int)).Value = inclproforma
    '        SqlCmd.CommandTimeout = 0
    '        DataAdapter.SelectCommand = SqlCmd
    '        DataAdapter.Fill(ds2)
    '        Dim detailsdt As New DataTable
    '        detailsdt = ds2.Tables(0)

    '        Dim dt = New DataTable()
    '        Dim acc = New DataColumn("acc_code", GetType(String))
    '        dt.Columns.Add(acc)


    '        Dim agents As New DataTable
    '        Dim conn1 As New SqlConnection
    '        conn1 = clsDBConnect.dbConnectionnew("strDBConnection")
    '        Dim strSql As String = "SELECT * FROM dbo.agentmast"
    '        Using dad As New SqlDataAdapter(strSql, conn1)
    '            dad.Fill(agents)
    '        End Using



    '        'If currflg = 0 Then
    '        '    currency = "USD"
    '        'Else
    '        '    Dim c As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
    '        '    currency = c
    '        'End If
    '        'currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currency & "'"), Integer)

    '        reportfilter = rptfilter
    '        If reportsType = "excel" Then

    '            ExcelReport(custdetailsdt, dt, agents, detailsdt, fromdate, todate, datetype, agingtype, Type, currflg, bytes)

    '        Else
    '            FontFactory.RegisterDirectories()
    '            Dim pdfdoc As New Document(PageSize.A4, 23.0F, 23.0F, 30.0F, 35.0F)
    '            pdfdoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)

    '            Using memoryStream As New System.IO.MemoryStream()
    '                Dim writer As PdfWriter = PdfWriter.GetInstance(pdfdoc, memoryStream)
    '                pdfdoc.Open()
    '                If custdetailsdt.Rows.Count > 0 Then
    '                    If Type = "S" Then
    '                        SupplierStament(custdetailsdt, dt, agents, detailsdt, fromdate, todate, datetype, agingtype, Type, pdfdoc, currflg)

    '                    Else
    '                        custdetailsdt.DefaultView.Sort = "acc_code ASC"
    '                        custdetailsdt = custdetailsdt.DefaultView.ToTable

    '                        For Each Customer_Statement In custdetailsdt.Rows
    '                            Dim decCurreccy As String
    '                            '07/01/2019

    '                            currency = Customer_Statement("currcode").ToString()
    '                            If currflg = 0 Then
    '                                decCurreccy = currency
    '                            Else
    '                                decCurreccy = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
    '                            End If
    '                            currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & decCurreccy & "'"), Integer)



    '                            ''added param 20/11/2018
    '                            'If currflg = 0 Then
    '                            '    currency = Customer_Statement("currcode").ToString()
    '                            '    currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currency & "'"), Integer)
    '                            'End If
    '                            Dim acccode As String = Customer_Statement("acc_code").ToString()
    '                            Dim accname As String = Customer_Statement("accname").ToString()
    '                            Dim crlimit As String = Customer_Statement("crlimit").ToString()
    '                            Dim debit, credit, fdebit, fcredit, fbalance, mdebit, mcredit, mbalance, cumbal, totalbalance As Decimal
    '                            Dim k As Integer
    '                            Dim dr() As System.Data.DataRow
    '                            Dim dr1() As System.Data.DataRow
    '                            Dim agentdet() As System.Data.DataRow
    '                            dr1 = dt.Select("acc_code='" & acccode & "'")
    '                            If dr1.Length = 0 Then
    '                                dt.Rows.Add(acccode)
    '                                dr = custdetailsdt.Select("acc_code='" & acccode & "'")
    '                                agentdet = agents.Select("agentcode='" & acccode & "'")
    '                                Dim dr3() As System.Data.DataRow
    '                                dr3 = detailsdt.Select("acc_code='" & acccode & "'")
    '                                'If dr3.Length = 0 Then
    '                                '    Dim x As Integer = 1
    '                                'End If
    '                                Dim logo As PdfPTable = Nothing
    '                                logo = New PdfPTable(1)
    '                                logo.TotalWidth = documentWidth
    '                                logo.LockedWidth = True
    '                                logo.SetWidths(New Single() {1.0F})
    '                                logo.Complete = False
    '                                logo.SplitRows = False
    '                                phrase = New Phrase()
    '                                phrase.Add(New Chunk("Printed Date : " + DateTime.Now.ToString("dd/MM/yyyy"), NormalFont))
    '                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
    '                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                                cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
    '                                logo.AddCell(cell)
    '                                'company name
    '                                If divcode = "02" Then
    '                                    cell = ImageCell("~/Images/logo.jpg", 80.0F, PdfPCell.ALIGN_LEFT)
    '                                Else
    '                                    cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
    '                                End If
    '                                cell.VerticalAlignment = PdfPCell.ALIGN_TOP
    '                                cell.Colspan = 2
    '                                cell.SetLeading(12, 0)
    '                                cell.PaddingBottom = 4
    '                                logo.AddCell(cell)
    '                                pdfdoc.Add(logo)

    '                                Dim tblTitle As PdfPTable = New PdfPTable(1)
    '                                tblTitle.SetWidths(New Single() {1.0F})
    '                                tblTitle.TotalWidth = documentWidth
    '                                tblTitle.LockedWidth = True
    '                                phrase = New Phrase()
    '                                phrase.Add(New Chunk(custSupType & Environment.NewLine & vbLf & reportfilter, TitleFont))
    '                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                                cell.PaddingBottom = 3.0F
    '                                tblTitle.AddCell(cell)
    '                                tblTitle.SpacingBefore = 7
    '                                pdfdoc.Add(tblTitle)
    '                                Dim overdue As Decimal
    '                                Dim l = agentdet.Length

    '                                If agentdet.Length > 0 Then
    '                                    If IsDBNull(agentdet(0)("crlimit")) Then agentdet(0)("crlimit") = 0
    '                                    If agentdet(0)("crlimit") Then
    '                                        Dim overbal As Decimal
    '                                        If dr3.Length > 0 Then
    '                                            overbal = Decimal.Parse(dr3(0)("balance"))
    '                                        Else
    '                                            overbal = 0
    '                                        End If
    '                                        Dim overcr As Decimal = Decimal.Parse(agentdet(0)("crlimit"))
    '                                        overdue = Decimal.Subtract(overbal, overcr)
    '                                        contact1 = agentdet(0)("contact1").ToString()
    '                                        acrlimit = Decimal.Parse(agentdet(0)("crlimit"))
    '                                        cdays = Decimal.Parse(agentdet(0)("crdays"))
    '                                    Else
    '                                        If dr3.Length > 0 Then
    '                                            overdue = Decimal.Parse(dr3(0)("balance"))
    '                                        Else
    '                                            overdue = 0
    '                                        End If
    '                                    End If
    '                                Else
    '                                    Dim overbal As Decimal = Decimal.Parse(dr3(0)("balance"))
    '                                    Dim overcr As Decimal = 0
    '                                    overdue = Decimal.Subtract(overbal, overcr)
    '                                    contact1 = String.Empty
    '                                    crlimit = 0
    '                                    cdays = 0
    '                                End If


    '                                Dim tblcommon As PdfPTable = New PdfPTable(2)
    '                                tblcommon.SetWidths(New Single() {0.5F, 0.5F})
    '                                tblcommon.TotalWidth = documentWidth
    '                                tblcommon.LockedWidth = True
    '                                Dim tbl As PdfPTable = New PdfPTable(1)
    '                                phrase = New Phrase()
    '                                phrase.Add(New Chunk("TO" + "           " + ":" + Space(12) + Customer_Statement("acc_code").ToString() + " " + Customer_Statement("accname").ToString() + Environment.NewLine & vbLf, NormalFont))
    '                                phrase.Add(New Chunk("DATE" + "       " + ":" + Space(12) + Convert.ToDateTime(todate).ToString("dd/MM/yyyy") + Environment.NewLine & vbLf, NormalFont))
    '                                phrase.Add(New Chunk("ATTN" + "       " + ":" + Space(12) + contact1 + Environment.NewLine & vbLf, NormalFont))
    '                                phrase.Add(New Chunk("TEL" & "          " & ":" & Space(12) & Customer_Statement("tel1").ToString() & "                  ", NormalFont))
    '                                phrase.Add(New Chunk("FAX" & Space(11) & ":" & Space(12) & Customer_Statement("fax").ToString() & Environment.NewLine, NormalFont))
    '                                phrase.Add(New Chunk("           " & Space(18) & Customer_Statement("tel2").ToString() & "                  ", NormalFont))
    '                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                                cell.PaddingBottom = 3.0F
    '                                tbl.AddCell(cell)
    '                                tblcommon.AddCell(tbl)

    '                                ''''''''''''''''''''''''''''''''''''''''''''''''''

    '                                'Dim tbl1 As PdfPTable = New PdfPTable(1)
    '                                'phrase = New Phrase()
    '                                'phrase.Add(New Chunk("TO" + "           " + ":" + Space(12) + Customer_Statement("acc_code").ToString() + " " + Customer_Statement("accname").ToString() + Environment.NewLine & vbLf, NormalFont))
    '                                'phrase.Add(New Chunk("DATE" + "       " + ":" + Space(12) + Convert.ToDateTime(todate).ToString("dd/MM/yyyy") + Environment.NewLine & vbLf, NormalFont))
    '                                'phrase.Add(New Chunk("ATTN" + "       " + ":" + Space(12) + contact1 + Environment.NewLine & vbLf, NormalFont))
    '                                'phrase.Add(New Chunk("TEL" & "          " & ":" & Space(12) & Customer_Statement("tel1").ToString() & "                  ", NormalFont))
    '                                'phrase.Add(New Chunk("FAX" & Space(11) & ":" & Space(12) & Customer_Statement("fax").ToString() & Environment.NewLine, NormalFont))
    '                                'phrase.Add(New Chunk("           " & Space(18) & Customer_Statement("tel2").ToString() & "                  ", NormalFont))
    '                                'cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                                'cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                                'cell.PaddingBottom = 3.0F
    '                                'tbl1.AddCell(cell)
    '                                'tblcommon.AddCell(tbl1)

    '                                ''''''''''''''''''''''''''''''''''''''''''''''''''

    '                                phrase = New Phrase()
    '                                phrase.Add(New Chunk("Credit Limit" & "      " & ":" & Space(5) & acrlimit.ToString() + Environment.NewLine & vbLf, NormalFont))
    '                                phrase.Add(New Chunk("Credit Days" & "      " & ":" & Space(5) & cdays.ToString() + Environment.NewLine & vbLf, NormalFont))
    '                                Dim overdueAmt As String = IIf(overdue <= 0, Decimal.Parse((Math.Abs(overdue))).ToString("N" + currDecno.ToString) + IIf(overdue = 0, "", " Cr"), overdue.ToString("N" + currDecno.ToString) + " Dr")
    '                                phrase.Add(New Chunk("OverDue" & "          " & ":" & Space(5) & overdueAmt + Environment.NewLine & vbLf, NormalFont))
    '                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                                cell.SetLeading(12, 0)
    '                                cell.PaddingLeft = 235.0F
    '                                tblcommon.AddCell(cell)
    '                                tblcommon.SpacingBefore = 7
    '                                pdfdoc.Add(tblcommon)


    '                                Dim desc As PdfPTable = New PdfPTable(2)
    '                                desc.SetWidths(New Single() {0.82F, 0.18F})
    '                                desc.TotalWidth = documentWidth
    '                                desc.LockedWidth = True
    '                                phrase = New Phrase()
    '                                If datetype <> 0 Then
    '                                    phrase.Add(New Chunk("Please Find the up to date statement of Account between " + Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy") + " and " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy"), BalFont))
    '                                Else
    '                                    phrase.Add(New Chunk("Please Find the up to date statement of Account as on " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy"), BalFont))
    '                                End If
    '                                cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 1, False)
    '                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
    '                                cell.PaddingBottom = 3.0F
    '                                desc.AddCell(cell)
    '                                phrase = New Phrase()
    '                                Dim curr As PdfPTable = New PdfPTable(1)
    '                                phrase.Add(New Chunk("Currency" + "         " + ":" + Space(7) + decCurreccy, NormalFont))
    '                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
    '                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                                cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
    '                                cell.PaddingBottom = 3.0F
    '                                cell.PaddingRight = 15.0F
    '                                curr.AddCell(cell)
    '                                desc.AddCell(curr)
    '                                desc.SpacingBefore = 7
    '                                pdfdoc.Add(desc)


    '                                Dim balance As PdfPTable = New PdfPTable(1)
    '                                balance.SetWidths(New Single() {1.0F})
    '                                balance.TotalWidth = documentWidth
    '                                balance.LockedWidth = True
    '                                phrase = New Phrase()
    '                                phrase.Add(New Chunk("We would appreciate if you could settle the balance due at the earliest", BalFont))
    '                                cell = PhraseCell(phrase, PdfPCell.ALIGN_MIDDLE, 1, False)
    '                                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
    '                                cell.PaddingBottom = 3.0F
    '                                balance.AddCell(cell)
    '                                balance.SpacingBefore = 7
    '                                pdfdoc.Add(balance)

    '                                Dim maintbl As PdfPTable = New PdfPTable(9)
    '                                Dim arrow2() As String

    '                                maintbl.SetWidths(New Single() {0.075F, 0.045F, 0.1F, 0.14F, 0.22F, 0.12F, 0.1F, 0.1F, 0.1F})
    '                                maintbl.TotalWidth = documentWidth
    '                                maintbl.LockedWidth = True
    '                                arrow2 = {"TRAN DATE", "TYPE", "INV.NO", "AGENT REF.", "GUEST/SERVICE DETAILS",
    '                                                          "SPERSON", "DEBIT", "CREDIT", "NET BALANCE"}

    '                                For i = 0 To arrow2.Length - 1
    '                                    phrase = New Phrase()
    '                                    phrase.Add(New Chunk(arrow2(i), HeadingFont))
    '                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
    '                                    cell.SetLeading(12, 0)
    '                                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
    '                                    cell.PaddingBottom = 4.0F
    '                                    cell.PaddingTop = 1.0F
    '                                    maintbl.AddCell(cell)
    '                                Next
    '                                maintbl.SpacingBefore = 7
    '                                pdfdoc.Add(maintbl)
    '                                Dim gpbyMonth As DataTable
    '                                Dim arrow3() As String = Nothing
    '                                Dim tdate As String = Nothing
    '                                Dim arr3index As Integer
    '                                Dim cumBalance As Decimal
    '                                Dim totaldebit, totalcredit, debits, credits As Decimal
    '                                finalcredit = 0
    '                                finaldebit = 0

    '                                If dr.Length > 0 Then
    '                                    gpbyMonth = dr.CopyToDataTable()
    '                                End If
    '                                Dim group As IEnumerable(Of IGrouping(Of Integer, DataRow)) = gpbyMonth.AsEnumerable().GroupBy(Function(g) g.Field(Of Date)("trandate").Month)

    '                                cumBalance = 0
    '                                For Each gpdata In group
    '                                    Dim perticulars As String

    '                                    totalcredit = 0
    '                                    totaldebit = 0
    '                                    totalbalances = 0
    '                                    Dim trantypes As String

    '                                    For Each row As DataRow In gpdata

    '                                        Dim data As PdfPTable = New PdfPTable(9)
    '                                        data.SetWidths(New Single() {0.075F, 0.045F, 0.1F, 0.14F, 0.22F, 0.12F, 0.1F, 0.1F, 0.1F})

    '                                        data.TotalWidth = documentWidth
    '                                        data.LockedWidth = True
    '                                        trantypes = row("trantype").ToString()

    '                                        finalcredit = Decimal.Parse(row("credit")) + finalcredit
    '                                        finaldebit = Decimal.Parse(row("debit")) + finaldebit
    '                                        debits = IIf(trantypes = "OB", 0, Decimal.Parse(row("debit")))
    '                                        credits = IIf(trantypes = "OB", 0, Decimal.Parse(row("credit")))
    '                                        totalcredit = totalcredit + credits
    '                                        totaldebit = totaldebit + debits
    '                                        totalbalances = totalbalances + (debits - credits)
    '                                        cumBalance = cumBalance + (debits - credits)
    '                                        Dim docno As String = IIf(row("incinvoiceno") = "", row("tranid").ToString(), row("incinvoiceno").ToString())
    '                                        tdate = Format(Convert.ToDateTime(row("trandate").ToString()), "dd/MM/yyyy")
    '                                        'row("arrdate").ToString(), row("depdate").ToString(),
    '                                        arrow3 = {tdate, row("trantype").ToString(), row("tranid").ToString(), row("reconfno").ToString(), row("particulars").ToString(),
    '                                       row("sperson").ToString(), IIf(debits = 0, "-", debits.ToString("N" + currDecno.ToString)), IIf(credits = 0, "-", credits.ToString("N" + currDecno.ToString)),
    '                                                  IIf(cumBalance <= 0, Decimal.Parse((Math.Abs(cumBalance))).ToString("N" + currDecno.ToString) + IIf(cumBalance = 0, "", " Cr"), cumBalance.ToString("N" + currDecno.ToString) + " Dr")}
    '                                        For k = 0 To arrow3.Length - 1

    '                                            phrase = New Phrase()
    '                                            phrase.Add(New Chunk(arrow3(k), datafont))
    '                                            If k = 0 Or k < 6 Then
    '                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                            Else
    '                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
    '                                            End If


    '                                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                                            cell.SetLeading(12, 0)
    '                                            cell.PaddingBottom = 4.0F
    '                                            cell.PaddingTop = 1.0F
    '                                            data.AddCell(cell)
    '                                        Next
    '                                        data.SpacingBefore = 0
    '                                        pdfdoc.Add(data)
    '                                        ' End If
    '                                    Next

    '                                    Dim mtotal As PdfPTable = New PdfPTable(5)
    '                                    Dim monthtotal() As String
    '                                    mtotal.SetWidths(New Single() {0.58F, 0.12F, 0.1F, 0.1F, 0.1F})
    '                                    mtotal.TotalWidth = documentWidth
    '                                    mtotal.LockedWidth = True
    '                                    monthtotal = {"", "MONTH TOTAL", totaldebit.ToString("N" + currDecno.ToString), totalcredit.ToString("N" + currDecno.ToString),
    '                                                  IIf(totalbalances <= 0, Decimal.Parse((Math.Abs(totalbalances))).ToString("N" + currDecno.ToString) + IIf(totalbalances = 0, "", " Cr"), totalbalances.ToString("N" + currDecno.ToString) + " Dr")}
    '                                    For k = 0 To monthtotal.Length - 1
    '                                        phrase = New Phrase()
    '                                        phrase.Add(New Chunk(monthtotal(k), datafont))
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
    '                                        cell.SetLeading(12, 0)
    '                                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                                        If k <> 0 Then
    '                                            cell.BackgroundColor = basecolor
    '                                        End If
    '                                        cell.PaddingBottom = 4.0F
    '                                        cell.PaddingTop = 1.0F
    '                                        mtotal.AddCell(cell)
    '                                    Next
    '                                    mtotal.SpacingBefore = 0
    '                                    pdfdoc.Add(mtotal)

    '                                Next

    '                                Dim Final As PdfPTable = New PdfPTable(5)
    '                                Final.SetWidths(New Single() {0.58F, 0.12F, 0.1F, 0.1F, 0.1F})
    '                                Final.TotalWidth = documentWidth
    '                                Final.LockedWidth = True
    '                                Dim finalTotal() As String = {"", "Final Total", finaldebit.ToString("N" + currDecno.ToString), finalcredit.ToString("N" + currDecno.ToString), ""}
    '                                For k = 0 To finalTotal.Length - 1
    '                                    phrase = New Phrase()
    '                                    phrase.Add(New Chunk(finalTotal(k), datafontBold))
    '                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, False)
    '                                    cell.SetLeading(12, 0)
    '                                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                                    cell.PaddingBottom = 4.0F
    '                                    cell.PaddingTop = 1.0F
    '                                    Final.AddCell(cell)
    '                                Next
    '                                Final.SpacingBefore = 7
    '                                pdfdoc.Add(Final)

    '                                Dim one As String, two As String, three As String, four As String, five As String, six As String
    '                                If dr3.Length > 0 Then
    '                                    If agingtype = 0 Then
    '                                        If Month = "01" Then
    '                                            one = "JAN"
    '                                            two = "DEC"
    '                                            three = "NOV"
    '                                            four = "OCT"
    '                                            five = "SEP"
    '                                            six = "AUG"
    '                                        ElseIf Month = "02" Then
    '                                            one = "FEB"
    '                                            two = "JAN"
    '                                            three = "DEC"
    '                                            four = "NOV"
    '                                            five = "OCT"
    '                                            six = "SEP"
    '                                        ElseIf Month = "03" Then
    '                                            one = "MAR"
    '                                            two = "FEB"
    '                                            three = "JAN"
    '                                            four = "DEC"
    '                                            five = "NOV"
    '                                            six = "OCT"
    '                                        ElseIf Month = "04" Then
    '                                            one = "APR"
    '                                            two = "MAR"
    '                                            three = "FEB"
    '                                            four = "JAN"
    '                                            five = "DEC"
    '                                            six = "NOV"
    '                                        ElseIf Month = "05" Then
    '                                            one = "MAY"
    '                                            two = "APR"
    '                                            three = "MAR"
    '                                            four = "FEB"
    '                                            five = "JAN"
    '                                            six = "DEC"
    '                                        ElseIf Month = "06" Then
    '                                            one = "JUN"
    '                                            two = "MAY"
    '                                            three = "APR"
    '                                            four = "MAR"
    '                                            five = "FEB"
    '                                            six = "JAN"
    '                                        ElseIf Month = "07" Then
    '                                            one = "JUL"
    '                                            two = "JUN"
    '                                            three = "MAY"
    '                                            four = "APR"
    '                                            five = "MAR"
    '                                            six = "FEB"
    '                                        ElseIf Month = "08" Then
    '                                            one = "AUG"
    '                                            two = "JUL"
    '                                            three = "JUN"
    '                                            four = "MAY"
    '                                            five = "APR"
    '                                            six = "MAR"
    '                                        ElseIf Month = "09" Then
    '                                            one = "SEP"
    '                                            two = "AUG"
    '                                            three = "JUL"
    '                                            four = "JUN"
    '                                            five = "MAY"
    '                                            six = "APR"
    '                                        ElseIf Month = "10" Then
    '                                            one = "OCT"
    '                                            two = "SEP"
    '                                            three = "AUG"
    '                                            four = "JUL"
    '                                            five = "JUN"
    '                                            six = "MAY"
    '                                        ElseIf Month = "11" Then
    '                                            one = "NOV"
    '                                            two = "OCT"
    '                                            three = "SEP"
    '                                            four = "AUG"
    '                                            five = "JUL"
    '                                            six = "JUN"
    '                                        ElseIf Month = "12" Then
    '                                            one = "DEC"
    '                                            two = "NOV"
    '                                            three = "OCT"
    '                                            four = "SEP"
    '                                            five = "AUG"
    '                                            six = "JUL"
    '                                        End If
    '                                    Else
    '                                        one = "<=0-30"
    '                                        two = "<=31-60"
    '                                        three = "<=61-90"
    '                                        four = "<=91-120"
    '                                        five = "<=121-150"
    '                                        six = "Over 150"
    '                                    End If
    '                                    Dim tbldetail As PdfPTable = New PdfPTable(1)
    '                                    tbldetail.SetWidths(New Single() {1.0F})
    '                                    tbldetail.TotalWidth = documentWidth
    '                                    tbldetail.LockedWidth = True
    '                                    phrase = New Phrase()
    '                                    phrase.Add(New Chunk("Aging Analysis Of Balance", NormalFontBold))
    '                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                                    cell.PaddingBottom = 3.0F
    '                                    tbldetail.AddCell(cell)
    '                                    tbldetail.SpacingBefore = 30
    '                                    pdfdoc.Add(tbldetail)

    '                                    Dim detailtbl As PdfPTable = New PdfPTable(8)
    '                                    detailtbl.SetWidths(New Single() {0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F})
    '                                    detailtbl.TotalWidth = documentWidth
    '                                    detailtbl.LockedWidth = True
    '                                    Dim arrow4() As String = {"BALANCE", "<0", one, two, three, four, five, six}
    '                                    For i = 0 To 7
    '                                        phrase = New Phrase()
    '                                        phrase.Add(New Chunk(arrow4(i), HeadingFont))
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
    '                                        cell.SetLeading(12, 0)
    '                                        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
    '                                        cell.PaddingBottom = 4.0F
    '                                        cell.PaddingTop = 1.0F
    '                                        detailtbl.AddCell(cell)
    '                                    Next
    '                                    detailtbl.SpacingBefore = 0
    '                                    pdfdoc.Add(detailtbl)



    '                                    Dim arr7() As Decimal
    '                                    arr7 = {dr3(0)("balance"), dr3(0)("age9"), dr3(0)("age1"), dr3(0)("age2"), dr3(0)("age3"), dr3(0)("age4"),
    '                                              dr3(0)("age5"), dr3(0)("age6")}

    '                                    Dim tbldata As PdfPTable = New PdfPTable(8)
    '                                    tbldata.SetWidths(New Single() {0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F, 0.125F})
    '                                    tbldata.TotalWidth = documentWidth
    '                                    tbldata.LockedWidth = True
    '                                    For i = 0 To 7
    '                                        phrase = New Phrase()
    '                                        If i = 0 Then
    '                                            Dim bal As String = IIf(arr7(i) <= 0, Decimal.Parse((Math.Abs(arr7(i)))).ToString("N" + currDecno.ToString) + IIf(arr7(i) = 0, "", " Cr"), arr7(i).ToString("N" + currDecno.ToString) + " Dr")
    '                                            phrase.Add(New Chunk(bal, datafont))
    '                                        Else
    '                                            phrase.Add(New Chunk(arr7(i).ToString("N" + currDecno.ToString), datafont))
    '                                        End If
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
    '                                        cell.SetLeading(12, 0)
    '                                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
    '                                        cell.PaddingBottom = 4.0F
    '                                        cell.PaddingTop = 1.0F
    '                                        tbldata.AddCell(cell)
    '                                    Next
    '                                    tbldata.SpacingBefore = 0
    '                                    pdfdoc.Add(tbldata)
    '                                End If
    '                                fdebit = 0.0
    '                                fcredit = 0.0
    '                                fbalance = 0
    '                                mdebit = 0
    '                                mcredit = 0
    '                                mbalance = 0
    '                                cumbal = 0
    '                                pdfdoc.NewPage()
    '                            End If
    '                        Next
    '                    End If
    '                Else
    '                    Dim logo As PdfPTable = Nothing
    '                    Dim Phrase = New Phrase()
    '                    Dim cell As PdfPCell = Nothing
    '                    logo = New PdfPTable(1)
    '                    logo.TotalWidth = documentWidth
    '                    logo.LockedWidth = True
    '                    logo.SetWidths(New Single() {1.0F})
    '                    logo.Complete = False
    '                    logo.SplitRows = False
    '                    Phrase = New Phrase()
    '                    Phrase.Add(New Chunk("Printed Date : " + DateTime.Now.ToString("dd/MM/yyyy"), NormalFont))
    '                    cell = PhraseCell(Phrase, PdfPCell.ALIGN_RIGHT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
    '                    logo.AddCell(cell)
    '                    'company name
    '                    cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
    '                    cell.Colspan = 2
    '                    cell.SetLeading(12, 0)
    '                    cell.PaddingBottom = 4
    '                    logo.AddCell(cell)
    '                    pdfdoc.Add(logo)

    '                    Dim tblTitle As PdfPTable = New PdfPTable(1)
    '                    tblTitle.SetWidths(New Single() {1.0F})
    '                    tblTitle.TotalWidth = documentWidth
    '                    tblTitle.LockedWidth = True
    '                    Phrase = New Phrase()
    '                    Phrase.Add(New Chunk(custSupType & Environment.NewLine & vbLf & reportfilter, TitleFont))
    '                    cell = PhraseCell(Phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                    cell.PaddingBottom = 3.0F
    '                    tblTitle.AddCell(cell)
    '                    tblTitle.SpacingBefore = 7
    '                    pdfdoc.Add(tblTitle)


    '                    Dim tblcommon As PdfPTable = New PdfPTable(2)
    '                    tblcommon.SetWidths(New Single() {0.5F, 0.5F})
    '                    tblcommon.TotalWidth = documentWidth
    '                    tblcommon.LockedWidth = True
    '                    Dim tbl As PdfPTable = New PdfPTable(1)
    '                    Phrase = New Phrase()
    '                    Phrase.Add(New Chunk("TO" + "           " + ":" + Space(12) + " " + Environment.NewLine & vbLf, NormalFont))
    '                    Phrase.Add(New Chunk("DATE" + "       " + ":" + Space(12) + todate + Environment.NewLine & vbLf, NormalFont))
    '                    Phrase.Add(New Chunk("ATTN" + "       " + ":" + Space(12) + contact1 + Environment.NewLine & vbLf, NormalFont))
    '                    Phrase.Add(New Chunk("TEL" & "          " & ":" & Space(12) & " ", NormalFont))
    '                    Phrase.Add(New Chunk("FAX" & Space(11) & ":" & "" & Environment.NewLine, NormalFont))
    '                    Phrase.Add(New Chunk("          " & Space(16) & "                  ", NormalFont))
    '                    cell = PhraseCell(Phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                    cell.PaddingBottom = 3.0F
    '                    tbl.AddCell(cell)
    '                    tblcommon.AddCell(tbl)
    '                    Phrase = New Phrase()
    '                    Phrase.Add(New Chunk("Credit Limit" & "      " & ":" & Space(5) & "" + Environment.NewLine & vbLf, NormalFont))
    '                    Phrase.Add(New Chunk("Credit Days" & "      " & ":" & Space(5) & "" + Environment.NewLine & vbLf, NormalFont))
    '                    Phrase.Add(New Chunk("OverDue" & "          " & ":" & Space(5) & "" + Environment.NewLine & vbLf, NormalFont))
    '                    cell = PhraseCell(Phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                    cell.SetLeading(12, 0)
    '                    cell.PaddingLeft = 250.0F
    '                    tblcommon.AddCell(cell)
    '                    tblcommon.SpacingBefore = 7
    '                    pdfdoc.Add(tblcommon)

    '                End If
    '                pdfdoc.AddTitle(custSupType)
    '                pdfdoc.Close()

    '                If printMode = "download" Then
    '                    Dim pagingFont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, basecolor.BLACK)
    '                    Dim reader As New PdfReader(memoryStream.ToArray())
    '                    Using mStream As New System.IO.MemoryStream()
    '                        Using stamper As New PdfStamper(reader, mStream)
    '                            Dim pages As Integer = reader.NumberOfPages
    '                            For i As Integer = 1 To pages
    '                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), 805.0F, 10.0F, 0)
    '                            Next
    '                        End Using
    '                        bytes = mStream.ToArray()
    '                    End Using
    '                End If
    '            End Using
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub
    'Public Sub SupplierStamentExcel(ByVal custdetailsdt As DataTable, ByVal dt As DataTable, ByVal agents As DataTable, ByVal detailsdt As DataTable, ByVal fromdate As String, ByVal todate As String, ByVal datetype As String, ByVal agingtype As String, ByVal Type As String, ByRef ws As IXLWorksheet, ByVal currflg As Integer, ByRef rowCount As Integer)
    '    Try
    '        Dim phrase As Phrase = Nothing
    '        Dim cell As PdfPCell = Nothing

    '        'If detailsdt.Rows.Count > 0 Then
    '        '    age1 = Convert.ToDecimal(detailsdt.Compute("Sum(age1)", ""))
    '        '    age2 = Convert.ToDecimal(detailsdt.Compute("Sum(age2)", ""))
    '        '    age3 = Convert.ToDecimal(detailsdt.Compute("Sum(age3)", ""))
    '        '    age4 = Convert.ToDecimal(detailsdt.Compute("Sum(age4)", ""))
    '        '    age5 = Convert.ToDecimal(detailsdt.Compute("Sum(age5)", ""))
    '        '    age6 = Convert.ToDecimal(detailsdt.Compute("Sum(age6)", ""))
    '        '    age1bal = Convert.ToDecimal(detailsdt.Compute("Sum(balance)", ""))
    '        'End If
    '        rowCount = 1

    '        '  ws.Cell("A2").Value = rptcompanyname
    '        Dim companyname = ws.Range("A" & rowCount & ":J" & rowCount).Merge()
    '        companyname.Style.Font.SetBold().Font.FontSize = 15
    '        companyname.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
    '        companyname.Merge()
    '        companyname.Style.Fill.BackgroundColor = XLColor.LightGray
    '        ws.Cell("A" & rowCount).Value = rptcompanyname

    '        rowCount = rowCount + 1
    '        ws.Cell("A" & rowCount).Value = custSupType

    '        Dim company = ws.Range("A" & rowCount & ":D" & rowCount).Merge()
    '        company.Style.Font.SetBold().Font.FontSize = 15

    '        rowCount = rowCount + 1
    '        ws.Cell("A" & rowCount).Value = reportfilter

    '        Dim filter = ws.Range("A" & rowCount & ":J" & rowCount).Merge()
    '        filter.Style.Alignment.SetWrapText().Font.SetBold().Font.FontSize = 13


    '        rowCount = rowCount + 2
    '        For Each Customer_Statement In custdetailsdt.Rows
    '            Dim decCurreccy As String
    '            '07/01/2019

    '            currency = Customer_Statement("currcode").ToString()
    '            If currflg = 0 Then
    '                decCurreccy = currency
    '            Else
    '                decCurreccy = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
    '            End If
    '            currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & decCurreccy & "'"), Integer)

    '            If currDecno = 1 Then
    '                DecimalPoint = "###,##,##,##0.0"
    '                DecimalPoint1 = "(###,##,##,##0.0)"
    '            ElseIf currDecno = 2 Then
    '                DecimalPoint = "###,##,##,##0.00"
    '                DecimalPoint1 = "(###,##,##,##0.00)"
    '            ElseIf currDecno = 3 Then
    '                DecimalPoint = "###,##,##,##0.000"
    '                DecimalPoint1 = "(###,##,##,##0.000)"
    '            ElseIf currDecno = 4 Then
    '                DecimalPoint = "###,##,##,##0.0000"
    '                DecimalPoint1 = "(###,##,##,##0.00000)"
    '            Else
    '                DecimalPoint = "###,##,##,##0.00"
    '                DecimalPoint1 = "(###,##,##,##0.00)"
    '            End If


    '            'If currflg = 0 Then
    '            '    currency = Customer_Statement("currcode").ToString()
    '            '    currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currency & "'"), Integer)
    '            'End If
    '            ' Commented by Tanvir 6/01/2019  currency was showing usd
    '            '    If currflg = 0 Then
    '            '        currency = "USD"
    '            '    Else
    '            '        Dim c As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
    '            '        currency = c

    '            '    End If
    '            ' currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currency & "'"), Integer)
    '            'Dim row() As DataRow = custdetailsdt.Select("trantype<>'PR'")
    '            Dim trantypep = Customer_Statement("trantype").ToString()
    '            If Not (String.Equals(trantypep, "PR")) Then
    '                Dim acccode As String = Customer_Statement("acc_code").ToString()
    '                Dim accname As String = Customer_Statement("accname").ToString()
    '                Dim crlimit As String = Customer_Statement("crlimit").ToString()
    '                Dim totaldebit, totalcredit, debits, credits As Decimal
    '                'Dim debit, credit, fdebit, fcredit, fbalance, mdebit, mcredit, mbalance, cumbal, totalbalance As Decimal
    '                Dim k, r As Integer
    '                Dim dr() As System.Data.DataRow
    '                Dim dr1() As System.Data.DataRow
    '                Dim agentdet() As System.Data.DataRow
    '                dr1 = dt.Select("acc_code='" & acccode & "'")
    '                If dr1.Length = 0 Then
    '                    dt.Rows.Add(acccode)
    '                    dr = custdetailsdt.Select("acc_code='" & acccode & "'")
    '                    agentdet = agents.Select("agentcode='" & acccode & "'")



    '                    Dim overdue, overbal, overcr As Decimal

    '                    Dim gpby As DataTable
    '                    ws.Column("A").Width = 12
    '                    ws.Column("B").Width = 10
    '                    ws.Column("C").Width = 13
    '                    ws.Column("E").Width = 10
    '                    ws.Column("D").Width = 12
    '                    ws.Column("F").Width = 42

    '                    ws.Columns("G:K").Width = 12

    '                    Dim gpbyMonth As DataTable
    '                    Dim arrow3() As String = Nothing
    '                    Dim tdate As String = Nothing
    '                    Dim arr3index As Integer
    '                    Dim cumBalance As Decimal
    '                    Dim mon As String = Format(Convert.ToDateTime(dr(0)("trandate").ToString()), "MM")
    '                    If dr.Length > 0 Then

    '                        Dim groups1 = From gpbyrow In dr.AsEnumerable() Group gpbyrow By g = New With {Key .acccode = gpbyrow.Field(Of String)("acc_code"), Key .gl_code = gpbyrow.Field(Of String)("acc_gl_code"), Key .accname = gpbyrow.Field(Of String)("accname")} Into Group Order By g.acccode

    '                        For Each gpdata1 In groups1

    '                            Dim dr4 = detailsdt.AsEnumerable().Where(Function(s) s.Field(Of String)("acc_code") = gpdata1.g.acccode And s.Field(Of String)("acc_gl_code") = gpdata1.g.gl_code)
    '                            gpbyMonth = gpdata1.Group.CopyToDataTable()

    '                            Dim sumdebit = Convert.ToDecimal(gpbyMonth.Compute("SUM(debit)", "trantype<>'OB'"))
    '                            Dim sumCredit = Convert.ToDecimal(gpbyMonth.Compute("SUM(credit)", "trantype<>'OB'"))
    '                            acccode = gpbyMonth(0)("acc_code")
    '                            agebalance = sumCredit - sumdebit

    '                            agentdet = agents.Select("agentcode='" & acccode & "'")

    '                            'If agentdet.Length > 0 Then
    '                            '    If Not (TypeOf agentdet(0)("crlimit") Is DBNull) Then
    '                            '        overbal = agebalance
    '                            '        overcr = Decimal.Parse(agentdet(0)("crlimit"))
    '                            '        overdue = Decimal.Subtract(overcr, overbal)
    '                            '        contact1 = agentdet(0)("contact1").ToString()
    '                            '        acrlimit = Decimal.Parse(agentdet(0)("crlimit"))
    '                            '        cdays = Decimal.Parse(agentdet(0)("crdays"))

    '                            '    End If
    '                            'Else
    '                            '    overbal = agebalance
    '                            '    overcr = 0
    '                            '    overdue = Decimal.Subtract(overcr, overbal)
    '                            '    contact1 = String.Empty
    '                            '    crlimit = 0
    '                            '    cdays = 0
    '                            'End If

    '                            totalcredit = 0
    '                            totaldebit = 0
    '                            totalbalances = 0
    '                            Dim trantypes As String



    '                            Dim rowheight As Integer
    '                            If reportfilter.Length > 140 Then
    '                                rowheight = IIf(reportfilter.Length > 140 And reportfilter.Length < 240, 32, IIf(reportfilter.Length > 240 And reportfilter.Length < 340, 48, 64))
    '                                ws.Row(rowCount).Height = rowheight
    '                            End If

    '                            rowCount = rowCount + 1
    '                            ws.Range(rowCount + 4, 1, rowCount + 4, 6).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)

    '                            'ws.Range(rowCount, 1, rowCount, 1).Value = "TO"
    '                            'ws.Range(rowCount, 1, rowCount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin)
    '                            'ws.Range(rowCount, 2, rowCount, 2).Value = ":" & Space(3) & Customer_Statement("acc_code").ToString() + " " + Customer_Statement("accname").ToString()
    '                            'ws.Range(rowCount, 2, rowCount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin)

    '                            'ws.Cell(rowCount, 9).Value = "Credit Limit"
    '                            'ws.Cell(rowCount, 10).Value = ":" & Space(3) & acrlimit.ToString()

    '                            'rowCount = rowCount + 1
    '                            'ws.Range(rowCount, 1, rowCount, 1).Value = "DATE"
    '                            'ws.Range(rowCount, 1, rowCount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin)
    '                            'ws.Range(rowCount, 2, rowCount, 2).Value = ":" & Space(3) & Convert.ToDateTime(todate).ToString("dd/MM/yyyy")
    '                            'ws.Range(rowCount, 2, rowCount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin)

    '                            'ws.Cell(rowCount, 9).Value = "Credit Days"
    '                            'ws.Cell(rowCount, 10).Value = ":" & Space(3) & cdays.ToString()


    '                            'rowCount = rowCount + 1
    '                            'ws.Range(rowCount, 1, rowCount, 1).Value = "ATTN."
    '                            'ws.Range(rowCount, 1, rowCount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin)
    '                            'ws.Range(rowCount, 2, rowCount, 2).Value = ":" & Space(3) & contact1
    '                            'ws.Range(rowCount, 2, rowCount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
    '                            'ws.Cell(rowCount, 9).Value = "OverDue"
    '                            'ws.Cell(rowCount, 10).Value = ":" & Space(3) & overdue.ToString("N" + currDecno.ToString)

    '                            'rowCount = rowCount + 1
    '                            'ws.Range(rowCount, 1, rowCount, 1).Value = "TEL"
    '                            'ws.Range(rowCount, 1, rowCount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin)
    '                            'ws.Range(rowCount, 2, rowCount, 2).Value = ":" & Space(3) & Customer_Statement("tel1").ToString() & Space(10) & "FAX:" & Space(3) & Customer_Statement("fax").ToString()
    '                            'ws.Range(rowCount, 2, rowCount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin)


    '                            rowCount = rowCount + 1
    '                            ws.Range(rowCount, 1, rowCount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin)

    '                            ws.Range(rowCount, 2, rowCount, 2).Value = Space(3) & Customer_Statement("tel2").ToString()
    '                            ws.Range(rowCount, 2, rowCount, 6).Merge()

    '                            rowCount = rowCount + 1
    '                            If datetype <> 0 Then
    '                                ws.Cell(rowCount, 1).Value = "Please Find the up to date statement of Account between " + Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy") + " and " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy")

    '                            Else
    '                                ws.Cell(rowCount, 1).Value = "Please Find the up to date statement of Account as on " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy")

    '                            End If
    '                            ws.Range(rowCount, 1, rowCount, 6).Merge()

    '                            ws.Cell(rowCount, 9).Value = "Currency" & Space(3) & ":" & Space(4) + currency
    '                            'ws.Cell(rowCount, 11)
    '                            ws.Range(rowCount, 9, rowCount, 10).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)

    '                            rowCount = rowCount + 1
    '                            ws.Cell(rowCount, 1).Value = "We would appreciate if you could settle the balance due at the earliest"
    '                            ws.Range(rowCount, 1, rowCount, 6).Merge()

    '                            Dim arrow2() As String
    '                            arrow2 = {"TRAN DATE", "TYPE", "Doc No", "Booking ID.", "SUP INV NO/TICKET NO",
    '                                                      "Description", "DEBIT", "CREDIT", "BALANCE", "CUMBAL"}

    '                            rowCount = rowCount + 2
    '                            ws.Range(rowCount, 1, rowCount, 10).Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
    '                            ws.Range(rowCount, 1, rowCount, 10).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
    '                            For i = 0 To arrow2.Length - 1
    '                                ws.Cell(rowCount, i + 1).Value = arrow2(i)

    '                            Next


    '                            Dim group1 As IEnumerable(Of IGrouping(Of Integer, DataRow)) = gpbyMonth.AsEnumerable().GroupBy(Function(g) g.Field(Of Date)("trandate").Month)
    '                            finalcredit = 0
    '                            finaldebit = 0

    '                            For Each gpdata In group1
    '                                Dim perticulars As String

    '                                gpby = gpdata.CopyToDataTable()


    '                                For Each row As DataRow In gpdata
    '                                    Dim data As PdfPTable = New PdfPTable(10)
    '                                    data.SetWidths(New Single() {0.075F, 0.045F, 0.07F, 0.07F, 0.08F, 0.36F, 0.1F, 0.1F, 0.1F, 0.1F})
    '                                    data.TotalWidth = documentWidth
    '                                    data.LockedWidth = True
    '                                    trantypes = row("trantype").ToString()
    '                                    If Not String.IsNullOrEmpty(trantypes) Then
    '                                        finalcredit = Decimal.Parse(row("credit")) + finalcredit
    '                                        finaldebit = Decimal.Parse(row("debit")) + finaldebit
    '                                        debits = IIf(trantypes = "OB", 0, Decimal.Parse(row("debit")))
    '                                        credits = IIf(trantypes = "OB", 0, Decimal.Parse(row("credit")))
    '                                        totalcredit = totalcredit + credits
    '                                        totaldebit = totaldebit + debits
    '                                        totalbalances = totalbalances + (credits - debits)
    '                                        cumBalance = cumBalance + (credits - debits)
    '                                        Dim docno As String = IIf(row("incinvoiceno") = "", row("tranid").ToString(), row("incinvoiceno").ToString())
    '                                        tdate = Format(Convert.ToDateTime(row("trandate").ToString()), "dd/MM/yyyy")
    '                                        If row("particulars").ToString().Contains(row("incinvdesc").ToString()) Then
    '                                            perticulars = row("particulars").ToString()
    '                                        Else
    '                                            perticulars = row("particulars").ToString() & Environment.NewLine & vbLf & row("incinvdesc").ToString()
    '                                        End If
    '                                        arrow3 = {tdate, row("trantype").ToString(), docno, row("fileno").ToString(), row("reconfno").ToString(), perticulars, IIf(debits = 0, "-", debits.ToString("N" + currDecno.ToString)), IIf(credits = 0, "-", Math.Round(credits, currDecno)), (credits - debits).ToString("N" + currDecno.ToString), IIf(cumBalance <= 0, Decimal.Parse((Math.Abs(cumBalance))).ToString("N" + currDecno.ToString()), cumBalance.ToString("N" + currDecno.ToString))}
    '                                        rowCount = rowCount + 1
    '                                        ws.Range(rowCount, 1, rowCount, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
    '                                        ws.Range(rowCount, 1, rowCount, 10).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)

    '                                        '  ws.Row(rowCount).AdjustToContents()
    '                                        For k = 0 To arrow3.Length - 1
    '                                            If k = 5 Then
    '                                                ws.Cell(rowCount, k + 1).Value = arrow3(k)
    '                                                ws.Cell(rowCount, k + 1).Style.Alignment.SetWrapText()
    '                                                ' ws.Range(rowCount, k + 1, rowCount, k + 2).Merge()
    '                                                ws.Cell(rowCount, k + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center)
    '                                            ElseIf k > 5 Then
    '                                                ws.Cell(rowCount, k + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                                                If k = 6 Then
    '                                                    If debits = 0 Then
    '                                                        ws.Cell(rowCount, k + 1).Value = "-"
    '                                                    Else
    '                                                        ws.Cell(rowCount, k + 1).Value = Decimal.Parse(arrow3(k))
    '                                                        ws.Cell(rowCount, k + 1).Style.Alignment.SetWrapText().NumberFormat.Format = DecimalPoint
    '                                                    End If
    '                                                ElseIf k = 7 Then
    '                                                    If credits = 0 Then
    '                                                        ws.Cell(rowCount, k + 1).Value = "-"
    '                                                    Else
    '                                                        ws.Cell(rowCount, k + 1).Value = Decimal.Parse(arrow3(k))
    '                                                        ws.Cell(rowCount, k + 1).Style.Alignment.SetWrapText().NumberFormat.Format = DecimalPoint
    '                                                    End If
    '                                                ElseIf k = arrow3.Length - 1 Then

    '                                                    If cumBalance < 0 Then
    '                                                        ws.Cell(rowCount, k + 1).Value = Decimal.Parse(arrow3(k))
    '                                                        ws.Cell(rowCount, k + 1).Style.Alignment.SetWrapText().NumberFormat.Format = DecimalPoint1
    '                                                    ElseIf cumBalance = 0 Then
    '                                                        ws.Cell(rowCount, k + 1).Value = "-"
    '                                                    Else
    '                                                        ws.Cell(rowCount, k + 1).Value = Decimal.Parse(arrow3(k))
    '                                                        ws.Cell(rowCount, k + 1).Style.Alignment.SetWrapText().NumberFormat.Format = DecimalPoint
    '                                                    End If

    '                                                Else

    '                                                    ws.Cell(rowCount, k + 1).Value = Decimal.Parse(arrow3(k))
    '                                                    ws.Cell(rowCount, k + 1).Style.Alignment.SetWrapText().NumberFormat.Format = DecimalPoint
    '                                                End If

    '                                            Else
    '                                                ws.Cell(rowCount, k + 1).Value = arrow3(k)
    '                                                ws.Cell(rowCount, k + 1).Style.Alignment.SetWrapText().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '                                            End If
    '                                        Next

    '                                    End If
    '                                Next


    '                                Dim monthtotal() As String

    '                                monthtotal = {"", "MONTH TOTAL", totaldebit.ToString("N" + currDecno.ToString), totalcredit.ToString("N" + currDecno.ToString), totalbalances.ToString("N" + currDecno.ToString), ""}
    '                                rowCount = rowCount + 1
    '                                ws.Range(rowCount, 1, rowCount, 10).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 7
    '                                ws.Range(rowCount, 1, rowCount, 10).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
    '                                ws.Range(rowCount, 6, rowCount, 10).Style.Fill.SetBackgroundColor(XLColor.LightGray)
    '                                For k = 0 To monthtotal.Length - 1
    '                                    If k = 0 Then
    '                                        ws.Range(rowCount, 1, rowCount, 5).Merge()
    '                                        ws.Range(rowCount, 1, rowCount, 5).Style.Border.SetBottomBorder(XLBorderStyleValues.None)
    '                                        ' ws.Range(rowCount, 2, rowCount, 7).Style.Border.SetTopBorder(XLBorderStyleValues.None)
    '                                    ElseIf k = 1 Or String.IsNullOrEmpty(monthtotal(k)) Then
    '                                        ws.Cell(rowCount, k + 5).Value = monthtotal(k)
    '                                        ws.Cell(rowCount, k + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                                    Else
    '                                        ws.Cell(rowCount, k + 5).Value = Decimal.Parse(monthtotal(k))
    '                                        ws.Cell(rowCount, k + 5).Style.NumberFormat.Format = DecimalPoint
    '                                        ws.Cell(rowCount, k + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                                    End If
    '                                Next
    '                            Next


    '                            Dim finalTotal() As String = {"", "Final Total", finaldebit.ToString("N" + currDecno.ToString), finalcredit.ToString("N" + currDecno.ToString), ""}

    '                            rowCount = rowCount + 1
    '                            ws.Range(rowCount, 1, rowCount, 10).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 7
    '                            ws.Range(rowCount, 1, rowCount, 10).Style.Alignment.WrapText = True
    '                            'ws.Range(rowCount, 2, rowCount, 12).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)

    '                            For k = 0 To finalTotal.Length - 1
    '                                If k = 0 Then
    '                                    ws.Range(rowCount, 1, rowCount, 5).Merge()
    '                                    ws.Range(rowCount, 1, rowCount, 5).Style.Border.SetBottomBorder(XLBorderStyleValues.None)

    '                                ElseIf k = 1 Or String.IsNullOrEmpty(finalTotal(k)) Then
    '                                    ws.Cell(rowCount, k + 5).Value = finalTotal(k)
    '                                    ws.Cell(rowCount, k + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                                Else
    '                                    ws.Cell(rowCount, k + 5).Value = Decimal.Parse(finalTotal(k))
    '                                    ws.Cell(rowCount, k + 5).Style.NumberFormat.Format = DecimalPoint
    '                                    ws.Cell(rowCount, k + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                                End If
    '                            Next


    '                            Dim one As String, two As String, three As String, four As String, five As String, six As String
    '                            If agingtype = 0 Then
    '                                If Month = "01" Then
    '                                    one = "JAN"
    '                                    two = "DEC"
    '                                    three = "NOV"
    '                                    four = "OCT"
    '                                    five = "SEP"
    '                                    six = "<=AUG"
    '                                ElseIf Month = "02" Then
    '                                    one = "FEB"
    '                                    two = "JAN"
    '                                    three = "DEC"
    '                                    four = "NOV"
    '                                    five = "OCT"
    '                                    six = "<=SEP"
    '                                ElseIf Month = "03" Then
    '                                    one = "MAR"
    '                                    two = "FEB"
    '                                    three = "JAN"
    '                                    four = "DEC"
    '                                    five = "NOV"
    '                                    six = "<=OCT"
    '                                ElseIf Month = "04" Then
    '                                    one = "APR"
    '                                    two = "MAR"
    '                                    three = "FEB"
    '                                    four = "JAN"
    '                                    five = "DEC"
    '                                    six = "<=NOV"
    '                                ElseIf Month = "05" Then
    '                                    one = "MAY"
    '                                    two = "APR"
    '                                    three = "MAR"
    '                                    four = "FEB"
    '                                    five = "JAN"
    '                                    six = "<=DEC"
    '                                ElseIf Month = "06" Then
    '                                    one = "JUN"
    '                                    two = "MAY"
    '                                    three = "APR"
    '                                    four = "MAR"
    '                                    five = "FEB"
    '                                    six = "<=JAN"
    '                                ElseIf Month = "07" Then
    '                                    one = "JUL"
    '                                    two = "JUN"
    '                                    three = "MAY"
    '                                    four = "APR"
    '                                    five = "MAR"
    '                                    six = "<=FEB"
    '                                ElseIf Month = "08" Then
    '                                    one = "AUG"
    '                                    two = "JUL"
    '                                    three = "JUN"
    '                                    four = "MAY"
    '                                    five = "APR"
    '                                    six = "<=MAR"
    '                                ElseIf Month = "09" Then
    '                                    one = "SEP"
    '                                    two = "AUG"
    '                                    three = "JUL"
    '                                    four = "JUN"
    '                                    five = "MAY"
    '                                    six = "<=APR"
    '                                ElseIf Month = "10" Then
    '                                    one = "OCT"
    '                                    two = "SEP"
    '                                    three = "AUG"
    '                                    four = "JUL"
    '                                    five = "JUN"
    '                                    six = "<=MAY"
    '                                ElseIf Month = "11" Then
    '                                    one = "NOV"
    '                                    two = "OCT"
    '                                    three = "SEP"
    '                                    four = "AUG"
    '                                    five = "JUL"
    '                                    six = "<=JUN"
    '                                ElseIf Month = "12" Then
    '                                    one = "DEC"
    '                                    two = "NOV"
    '                                    three = "OCT"
    '                                    four = "SEP"
    '                                    five = "AUG"
    '                                    six = "<=JUL"
    '                                End If
    '                            Else
    '                                one = "<=0-30"
    '                                two = "<=31-60"
    '                                three = "<=61-90"
    '                                four = "<=91-120"
    '                                five = "<=121-150"
    '                                six = "Over 150"
    '                            End If

    '                            rowCount = rowCount + 2

    '                            ws.Cell(rowCount, 1).Value = "Ageing Analysis Of Balance"
    '                            ws.Range(rowCount, 1, rowCount, 6).Merge().Style.Font.SetBold().Font.FontSize = 10
    '                            'phrase.Add(New Chunk("Ageing Analysis Of Balance", NormalFontBold))



    '                            rowCount = rowCount + 1

    '                            Dim arrow4() As String = {"BALANCE", "<0", one, two, three, four, five, six}
    '                            ws.Range(rowCount, 1, rowCount, 8).Style.Font.SetBold.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Bottom).Font.FontSize = 8
    '                            ws.Range(rowCount, 1, rowCount, 8).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '                            For i = 0 To 7
    '                                ws.Cell(rowCount, i + 1).Value = arrow4(i)
    '                            Next

    '                            rowCount = rowCount + 1
    '                            Dim arr7() As Decimal
    '                            '  arr7 = {age1bal, age9, age1, age2, age3, age4, age5, age6}
    '                            arr7 = {dr4(0)("balance"), dr4(0)("age9"), dr4(0)("age1"), dr4(0)("age2"), dr4(0)("age3"), dr4(0)("age4"),
    '                                             dr4(0)("age5"), dr4(0)("age6")}

    '                            ws.Range(rowCount, 1, rowCount, arr7.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Bottom).Font.FontSize = 8
    '                            ws.Range(rowCount, 1, rowCount, arr7.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)

    '                            For i = 0 To 7
    '                                ws.Cell(rowCount, i + 1).Value = arr7(i)
    '                                ws.Cell(rowCount, i + 1).Style.NumberFormat.Format = DecimalPoint
    '                            Next
    '                            ' ws.Columns("A:M").AdjustToContents()
    '                            'ws.Rows().AdjustToContents()
    '                            rowCount = rowCount + 2
    '                            finalcredit = 0
    '                            finaldebit = 0
    '                            cumBalance = 0
    '                            agebalance = 0
    '                        Next
    '                    End If
    '                End If
    '            End If
    '        Next
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub
    'Private Sub ExcelReport(ByVal custdetailsdt As DataTable, ByVal dt As DataTable, ByVal agents As DataTable, ByVal detailsdt As DataTable, ByVal fromdate As String, ByVal todate As String, ByVal datetype As String, ByVal agingtype As String, ByVal Type As String, ByVal currflg As Integer, ByRef bytes() As Byte)
    '    Try

    '        Dim pdfdoc As New Document(PageSize.A4, 23.0F, 23.0F, 30.0F, 35.0F)
    '        Dim wb As New XLWorkbook
    '        Dim ws = wb.Worksheets.Add("Statement")
    '        Dim rowcount As Integer = 5

    '        If custdetailsdt.Rows.Count > 0 Then

    '            custdetailsdt.DefaultView.Sort = "acc_code ASC"
    '            custdetailsdt = custdetailsdt.DefaultView.ToTable

    '            If Type = "S" Then
    '                SupplierStamentExcel(custdetailsdt, dt, agents, detailsdt, fromdate, todate, datetype, agingtype, Type, ws, currflg, rowcount)
    '            Else
    '                ws.Column("A:D").Width = 15
    '                ws.Column("E").Width = 28
    '                ws.Columns("L:I").Width = 15

    '                ws.Cell("A" & rowcount).Value = custSupType

    '                Dim company = ws.Range("A" & rowcount & ":E" & rowcount).Merge()
    '                company.Style.Font.SetBold().Font.FontSize = 15

    '                rowcount = rowcount + 1
    '                ws.Cell("A" & rowcount).Value = reportfilter

    '                Dim filter = ws.Range("A" & rowcount & ":I" & rowcount).Merge()
    '                filter.Style.Font.SetBold().Font.FontSize = 13

    '                Dim rowheight As Integer

    '                If reportfilter.Length > 140 Then
    '                    rowheight = IIf(reportfilter.Length > 140 And reportfilter.Length < 240, 32, IIf(reportfilter.Length > 240 And reportfilter.Length < 340, 48, 64))
    '                    ws.Row(rowcount).Height = rowheight
    '                End If

    '                For Each Customer_Statement In custdetailsdt.Rows
    '                    'added param 20/11/2018
    '                    'If currflg = 0 Then
    '                    '    currency = "USD"

    '                    'Else
    '                    '    Dim c As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
    '                    '    currency = c

    '                    'End If
    '                    'currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currency & "'"), Integer)
    '                    'If currflg = 0 Then
    '                    '    currency = Customer_Statement("currcode").ToString()
    '                    '    currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currency & "'"), Integer)
    '                    'End If
    '                    Dim decCurreccy As String
    '                    '07/01/2019

    '                    currency = Customer_Statement("currcode").ToString()
    '                    If currflg = 0 Then
    '                        decCurreccy = currency
    '                    Else
    '                        decCurreccy = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
    '                    End If
    '                    currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & decCurreccy & "'"), Integer)



    '                    If currDecno = 1 Then
    '                        DecimalPoint = "###,##,##,##0.0"
    '                        DecimalPoint1 = "(###,##,##,##0.0)"
    '                    ElseIf currDecno = 2 Then
    '                        DecimalPoint = "###,##,##,##0.00"
    '                        DecimalPoint1 = "(###,##,##,##0.00)"
    '                    ElseIf currDecno = 3 Then
    '                        DecimalPoint = "###,##,##,##0.000"
    '                        DecimalPoint1 = "(###,##,##,##0.000)"
    '                    ElseIf currDecno = 4 Then
    '                        DecimalPoint = "###,##,##,##0.0000"
    '                        DecimalPoint1 = "(###,##,##,##0.00000)"
    '                    Else
    '                        DecimalPoint = "###,##,##,##0.00"
    '                        DecimalPoint1 = "(###,##,##,##0.00)"
    '                    End If

    '                    Dim acccode As String = Customer_Statement("acc_code").ToString()
    '                    Dim accname As String = Customer_Statement("accname").ToString()
    '                    Dim crlimit As String = Customer_Statement("crlimit").ToString()
    '                    Dim debit, credit, fdebit, fcredit, fbalance, mdebit, mcredit, mbalance, cumbal, totalbalance As Decimal
    '                    Dim k As Integer
    '                    Dim dr() As System.Data.DataRow
    '                    Dim dr1() As System.Data.DataRow
    '                    Dim agentdet() As System.Data.DataRow
    '                    dr1 = dt.Select("acc_code='" & acccode & "'")


    '                    If dr1.Length = 0 Then
    '                        dt.Rows.Add(acccode)
    '                        dr = custdetailsdt.Select("acc_code='" & acccode & "'")
    '                        agentdet = agents.Select("agentcode='" & acccode & "'")
    '                        Dim dr3() As System.Data.DataRow
    '                        dr3 = detailsdt.Select("acc_code='" & acccode & "'")
    '                        Dim logo As PdfPTable = Nothing
    '                        logo = New PdfPTable(1)
    '                        logo.TotalWidth = documentWidth
    '                        logo.LockedWidth = True
    '                        logo.SetWidths(New Single() {1.0F})
    '                        logo.Complete = False
    '                        logo.SplitRows = False
    '                        'Phrase = New Phrase()
    '                        'Phrase.Add(New Chunk("Printed Date : " + DateTime.Now.ToString("dd/MM/yyyy"), NormalFont))
    '                        'cell = PhraseCell(Phrase, PdfPCell.ALIGN_RIGHT, 1, False)
    '                        'cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                        'cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT
    '                        'logo.AddCell(cell)
    '                        ''company name
    '                        'cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
    '                        'cell.VerticalAlignment = PdfPCell.ALIGN_TOP
    '                        'cell.Colspan = 2
    '                        'cell.SetLeading(12, 0)
    '                        'cell.PaddingBottom = 4
    '                        'logo.AddCell(cell)
    '                        'pdfdoc.Add(logo)

    '                        'Comapny Name Heading

    '                        Dim overdue As Decimal
    '                        Dim l = agentdet.Length
    '                        If agentdet.Length > 0 Then
    '                            If IsDBNull(agentdet(0)("crlimit")) Then agentdet(0)("crlimit") = 0
    '                            If agentdet(0)("crlimit") Then
    '                                Dim overbal As Decimal = Decimal.Parse(dr3(0)("balance"))
    '                                Dim overcr As Decimal = Decimal.Parse(agentdet(0)("crlimit"))
    '                                overdue = Decimal.Subtract(overbal, overcr)
    '                                contact1 = agentdet(0)("contact1").ToString()
    '                                acrlimit = Decimal.Parse(agentdet(0)("crlimit"))
    '                                cdays = Decimal.Parse(agentdet(0)("crdays"))
    '                            Else
    '                                overdue = Decimal.Parse(dr3(0)("balance"))
    '                            End If
    '                        Else
    '                            Dim overbal As Decimal = Decimal.Parse(dr3(0)("balance"))
    '                            Dim overcr As Decimal = 0
    '                            overdue = Decimal.Subtract(overbal, overcr)
    '                            contact1 = String.Empty
    '                            crlimit = 0
    '                            cdays = 0
    '                        End If

    '                        'phrase = New Phrase()
    '                        'phrase.Add(New Chunk("TO" + "           " + ":" + Space(12) + Customer_Statement("acc_code").ToString() + " " + Customer_Statement("accname").ToString() + Environment.NewLine & vbLf, NormalFont))
    '                        'phrase.Add(New Chunk("DATE" + "       " + ":" + Space(12) + Convert.ToDateTime(todate).ToString("dd/MM/yyyy") + Environment.NewLine & vbLf, NormalFont))
    '                        'phrase.Add(New Chunk("ATTN" + "       " + ":" + Space(12) + contact1 + Environment.NewLine & vbLf, NormalFont))
    '                        'phrase.Add(New Chunk("TEL" & "          " & ":" & Space(12) & Customer_Statement("tel1").ToString() & "                  ", NormalFont))
    '                        'phrase.Add(New Chunk("FAX" & Space(11) & ":" & Space(12) & Customer_Statement("fax").ToString() & Environment.NewLine, NormalFont))
    '                        'phrase.Add(New Chunk("          " & Space(16) & Customer_Statement("tel2").ToString() & "                  ", NormalFont))

    '                        rowcount = rowcount + 1
    '                        ws.Range(rowcount + 4, 1, rowcount + 4, 6).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Font.FontSize = 11

    '                        ws.Range(rowcount, 1, rowcount, 2).Value = "TO"
    '                        ws.Range(rowcount, 1, rowcount, 1).Style.Font.SetBold().Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Font.FontSize = 11
    '                        ws.Range(rowcount, 2, rowcount, 2).Value = ":" & Space(3) & Customer_Statement("acc_code").ToString() + " " + Customer_Statement("accname").ToString()
    '                        ws.Range(rowcount, 2, rowcount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Font.FontSize = 11

    '                        ws.Cell(rowcount, 8).Value = "Credit Limit"
    '                        ws.Cell(rowcount, 9).Value = ":" & Space(3) & acrlimit.ToString()

    '                        rowcount = rowcount + 1
    '                        ws.Range(rowcount, 1, rowcount, 1).Value = "DATE"
    '                        ws.Range(rowcount, 1, rowcount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin)
    '                        ws.Range(rowcount, 2, rowcount, 2).Value = ":" & Space(3) & Convert.ToDateTime(todate).ToString("dd/MM/yyyy")
    '                        ws.Range(rowcount, 2, rowcount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin)

    '                        ws.Cell(rowcount, 8).Value = "Credit Days"
    '                        ws.Cell(rowcount, 9).Value = ":" & Space(3) & cdays.ToString()


    '                        rowcount = rowcount + 1
    '                        ws.Range(rowcount, 1, rowcount, 1).Value = "ATTN."
    '                        ws.Range(rowcount, 1, rowcount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin)
    '                        ws.Range(rowcount, 2, rowcount, 2).Value = ":" & Space(3) & contact1
    '                        ws.Range(rowcount, 2, rowcount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin)

    '                        Dim overDueAmt As String = IIf(overdue <= 0, Decimal.Parse((Math.Abs(overdue))).ToString("N" + currDecno.ToString) + IIf(overdue = 0, "", " Cr"), overdue.ToString("N" + currDecno.ToString) + " Dr")
    '                        ws.Cell(rowcount, 8).Value = "OverDue"
    '                        ws.Cell(rowcount, 9).Value = ":" & Space(3) & overDueAmt

    '                        rowcount = rowcount + 1
    '                        ws.Range(rowcount, 1, rowcount, 1).Value = "TEL"
    '                        ws.Range(rowcount, 1, rowcount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin)
    '                        ws.Range(rowcount, 2, rowcount, 2).Value = ":" & Space(3) & Customer_Statement("tel1").ToString() & Space(15) & "FAX:" & Space(3) & Customer_Statement("fax").ToString()
    '                        ws.Range(rowcount, 2, rowcount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin)


    '                        rowcount = rowcount + 1
    '                        ws.Range(rowcount, 1, rowcount, 1).Style.Font.SetBold().Border.SetLeftBorder(XLBorderStyleValues.Thin)

    '                        ws.Range(rowcount, 2, rowcount, 2).Value = Space(3) & Customer_Statement("tel2").ToString()

    '                        ws.Range(rowcount, 2, rowcount, 6).Merge().Style.Border.SetRightBorder(XLBorderStyleValues.Thin)

    '                        rowcount = rowcount + 1
    '                        If datetype <> 0 Then
    '                            ws.Cell(rowcount, 1).Value = "Please Find the up to date statement of Account between " + Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy") + " and " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy")

    '                        Else
    '                            ws.Cell(rowcount, 1).Value = "Please Find the up to date statement of Account as on " + Convert.ToDateTime(todate).ToString("dd/MM/yyyy")

    '                        End If
    '                        ws.Range(rowcount, 1, rowcount, 7).Merge().Style.Alignment.WrapText = True

    '                        ws.Cell(rowcount, 8).Value = "Currency" & Space(3) & ":" & Space(4) + currency
    '                        'ws.Cell(rowCount, 11)
    '                        ws.Range(rowcount, 8, rowcount, 9).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)

    '                        rowcount = rowcount + 1
    '                        ws.Cell(rowcount, 1).Value = "We would appreciate if you could settle the balance due at the earliest"
    '                        ws.Range(rowcount, 1, rowcount, 6).Merge()



    '                        Dim arrow2() As String



    '                        arrow2 = {"TRAN DATE", "TYPE", "INV.NO", "AGENT REF.", "GUEST/SERVICE DETAILS",
    '                                                  "SPERSON", "DEBIT", "CREDIT", "NET BALANCE"}

    '                        rowcount = rowcount + 2
    '                        ws.Range(rowcount, 1, rowcount, arrow2.Length).Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
    '                        ws.Range(rowcount, 1, rowcount, arrow2.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True

    '                        For i = 0 To arrow2.Length - 1
    '                            ws.Cell(rowcount, i + 1).Value = arrow2(i)
    '                        Next

    '                        Dim gpbyMonth As DataTable
    '                        Dim arrow3() As String = Nothing
    '                        Dim tdate As String = Nothing
    '                        Dim arr3index As Integer
    '                        Dim cumBalance As Decimal
    '                        Dim totaldebit, totalcredit, debits, credits As Decimal
    '                        finalcredit = 0
    '                        finaldebit = 0

    '                        'Dim groups = From gpbymonth In dr.AsEnumerable() Group custledger By g = New With {Key .acccode = custledger.Field(Of String)("acc_code"), Key .gl_code = custledger.Field(Of String)("acc_gl_code"), Key .accname = custledger.Field(Of String)("accname")} Into Group Order By g.acccode
    '                        If dr.Length > 0 Then
    '                            gpbyMonth = dr.CopyToDataTable()
    '                        End If
    '                        Dim group As IEnumerable(Of IGrouping(Of Integer, DataRow)) = gpbyMonth.AsEnumerable().GroupBy(Function(g) g.Field(Of Date)("trandate").Month)
    '                        ' Dim groups As IEnumerable(Of IGrouping(Of String, DataRow)) = dscurrecncy.AsEnumerable.GroupBy(Function(g) g.Field(Of String)(groupby)).OrderBy(Function(o) o.Key)
    '                        cumBalance = 0
    '                        For Each gpdata In group
    '                            Dim perticulars As String

    '                            totalcredit = 0
    '                            totaldebit = 0
    '                            totalbalances = 0
    '                            Dim trantypes As String

    '                            For Each row As DataRow In gpdata


    '                                trantypes = row("trantype").ToString()
    '                                '  If Not String.Equals(trantypes, "PR") Then
    '                                finalcredit = Decimal.Parse(row("credit")) + finalcredit
    '                                finaldebit = Decimal.Parse(row("debit")) + finaldebit
    '                                debits = IIf(trantypes = "OB", 0, Decimal.Parse(row("debit")))
    '                                credits = IIf(trantypes = "OB", 0, Decimal.Parse(row("credit")))
    '                                totalcredit = totalcredit + credits
    '                                totaldebit = totaldebit + debits
    '                                totalbalances = totalbalances + (debits - credits)
    '                                cumBalance = cumBalance + (debits - credits)
    '                                Dim docno As String = IIf(row("incinvoiceno") = "", row("tranid").ToString(), row("incinvoiceno").ToString())
    '                                tdate = Format(Convert.ToDateTime(row("trandate").ToString()), "dd/MM/yyyy")
    '                                arrow3 = {tdate, row("trantype").ToString(), row("tranid").ToString(), row("reconfno").ToString(), row("particulars").ToString(),
    '                                row("sperson").ToString(), IIf(debits = 0, "-", debits.ToString("N" + currDecno.ToString)), IIf(credits = 0, "-", credits.ToString("N" + currDecno.ToString)),
    '                                          IIf(cumBalance <= 0, Decimal.Parse((Math.Abs(cumBalance))).ToString("N" + currDecno.ToString) + IIf(cumBalance = 0, "", " Cr"), cumBalance.ToString("N" + currDecno.ToString) + " Dr")}
    '                                rowcount = rowcount + 1
    '                                ws.Range(rowcount, 1, rowcount, arrow3.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
    '                                ws.Range(rowcount, 1, rowcount, arrow3.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True

    '                                For k = 0 To arrow3.Length - 1


    '                                    If k = 0 Or k < 6 Then
    '                                        ws.Cell(rowcount, k + 1).Value = arrow3(k)
    '                                        ws.Cell(rowcount, k + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
    '                                    Else
    '                                        ws.Cell(rowcount, k + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                                        If k = 6 Then
    '                                            If debits = 0 Then
    '                                                ws.Cell(rowcount, k + 1).Value = "-"
    '                                            Else
    '                                                ws.Cell(rowcount, k + 1).Value = Decimal.Parse(arrow3(k))
    '                                                ws.Cell(rowcount, k + 1).Style.NumberFormat.Format = DecimalPoint
    '                                            End If

    '                                        ElseIf k = 7 Then
    '                                            If credits = 0 Then
    '                                                ws.Cell(rowcount, k + 1).Value = "-"
    '                                            Else
    '                                                ws.Cell(rowcount, k + 1).Value = Decimal.Parse(arrow3(k))
    '                                                ws.Cell(rowcount, k + 1).Style.NumberFormat.Format = DecimalPoint
    '                                            End If


    '                                        ElseIf k = 8 Then
    '                                            If cumBalance = 0 Then
    '                                                ws.Cell(rowcount, k + 1).Value = "-"
    '                                            ElseIf cumBalance < 0 Then
    '                                                ws.Cell(rowcount, k + 1).Value = arrow3(k)  'Decimal.Parse(arrow3(k))
    '                                                'ws.Cell(rowcount, k + 1).Style.NumberFormat.Format = DecimalPoint1
    '                                            Else
    '                                                ws.Cell(rowcount, k + 1).Value = arrow3(k) 'Decimal.Parse(arrow3(k))
    '                                                'ws.Cell(rowcount, k + 1).Style.NumberFormat.Format = DecimalPoint
    '                                            End If
    '                                        Else
    '                                            ws.Cell(rowcount, k + 1).Value = Decimal.Parse(arrow3(k))
    '                                            ws.Cell(rowcount, k + 1).Style.NumberFormat.Format = DecimalPoint

    '                                        End If
    '                                    End If
    '                                Next

    '                            Next


    '                            Dim monthtotal() As String

    '                            monthtotal = {"", "MONTH TOTAL", totaldebit.ToString("N" + currDecno.ToString), totalcredit.ToString("N" + currDecno.ToString),
    '                                           IIf(totalbalances <= 0, Decimal.Parse((Math.Abs(totalbalances))).ToString("N" + currDecno.ToString) + IIf(totalbalances = 0, "", " Cr"), totalbalances.ToString("N" + currDecno.ToString) + " Dr")}
    '                            rowcount = rowcount + 1
    '                            ws.Range(rowcount, 1, rowcount, 9).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
    '                            ws.Range(rowcount, 1, rowcount, 9).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
    '                            ws.Range(rowcount, 7, rowcount, 9).Style.Fill.SetBackgroundColor(XLColor.LightGray)

    '                            For k = 0 To monthtotal.Length - 1
    '                                If k = 0 Then
    '                                    ws.Cell(rowcount, k + 1).Value = monthtotal(k)
    '                                    ws.Range(rowcount, 1, rowcount, 5).Merge()
    '                                Else
    '                                    If k = 1 Then
    '                                        ws.Cell(rowcount, k + 5).Value = monthtotal(k)
    '                                        'ws.Range(rowcount, k + 5, rowcount, k + 7).Merge()                                        
    '                                    ElseIf k = monthtotal.Length - 1 Then
    '                                        ws.Cell(rowcount, k + 5).Value = monthtotal(k)
    '                                        ws.Cell(rowcount, k + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                                    ElseIf monthtotal(k) = 0 And k <> monthtotal.Length - 1 Then
    '                                        ws.Cell(rowcount, k + 5).Value = "-"
    '                                        ws.Cell(rowcount, k + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                                        'ElseIf k = monthtotal.Length - 1 Then
    '                                        '    ws.Cell(rowcount, 14).Value = " "
    '                                    Else
    '                                        ws.Cell(rowcount, k + 5).Value = Decimal.Parse(monthtotal(k))
    '                                        ws.Cell(rowcount, k + 5).Style.NumberFormat.Format = DecimalPoint
    '                                    End If

    '                                End If
    '                            Next


    '                        Next

    '                        'Dim Final As PdfPTable = New PdfPTable(5)
    '                        'Final.SetWidths(New Single() {0.58F, 0.14F, 0.07F, 0.07F, 0.14F})
    '                        'Final.TotalWidth = documentWidth
    '                        'Final.LockedWidth = True
    '                        Dim finalTotal() As String = {"", "Final Total", finaldebit.ToString("N" + currDecno.ToString), finalcredit.ToString("N" + currDecno.ToString)}
    '                        rowcount = rowcount + 1
    '                        ws.Range(rowcount, 1, rowcount, 9).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
    '                        ' ws.Range(rowcount, 2, rowcount, 14).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
    '                        ' ws.Range(rowcount, 9, rowcount, 14).Style.Fill.SetBackgroundColor(XLColor.LightGray)

    '                        For k = 0 To finalTotal.Length - 1
    '                            If k = 0 Then
    '                                ws.Cell(rowcount, k + 1).Value = finalTotal(k)
    '                                ws.Range(rowcount, 1, rowcount, 5).Merge()
    '                            Else
    '                                If k = 1 Then
    '                                    ws.Cell(rowcount, k + 5).Value = finalTotal(k)
    '                                    'ws.Range(rowcount, k + 6, rowcount, k + 7).Merge()

    '                                ElseIf String.IsNullOrEmpty(finalTotal(k)) Then
    '                                    ws.Cell(rowcount, k + 5).Value = ""
    '                                Else
    '                                    ws.Cell(rowcount, k + 5).Value = Decimal.Parse(finalTotal(k))
    '                                    ws.Cell(rowcount, k + 5).Style.NumberFormat.Format = DecimalPoint
    '                                End If

    '                            End If
    '                        Next
    '                        'Final.SpacingBefore = 7
    '                        'pdfdoc.Add(Final)

    '                        Dim one As String, two As String, three As String, four As String, five As String, six As String
    '                        If agingtype = 0 Then
    '                            If Month = "01" Then
    '                                one = "JAN"
    '                                two = "DEC"
    '                                three = "NOV"
    '                                four = "OCT"
    '                                five = "SEP"
    '                                six = "AUG"
    '                            ElseIf Month = "02" Then
    '                                one = "FEB"
    '                                two = "JAN"
    '                                three = "DEC"
    '                                four = "NOV"
    '                                five = "OCT"
    '                                six = "SEP"
    '                            ElseIf Month = "03" Then
    '                                one = "MAR"
    '                                two = "FEB"
    '                                three = "JAN"
    '                                four = "DEC"
    '                                five = "NOV"
    '                                six = "OCT"
    '                            ElseIf Month = "04" Then
    '                                one = "APR"
    '                                two = "MAR"
    '                                three = "FEB"
    '                                four = "JAN"
    '                                five = "DEC"
    '                                six = "NOV"
    '                            ElseIf Month = "05" Then
    '                                one = "MAY"
    '                                two = "APR"
    '                                three = "MAR"
    '                                four = "FEB"
    '                                five = "JAN"
    '                                six = "DEC"
    '                            ElseIf Month = "06" Then
    '                                one = "JUN"
    '                                two = "MAY"
    '                                three = "APR"
    '                                four = "MAR"
    '                                five = "FEB"
    '                                six = "JAN"
    '                            ElseIf Month = "07" Then
    '                                one = "JUL"
    '                                two = "JUN"
    '                                three = "MAY"
    '                                four = "APR"
    '                                five = "MAR"
    '                                six = "FEB"
    '                            ElseIf Month = "08" Then
    '                                one = "AUG"
    '                                two = "JUL"
    '                                three = "JUN"
    '                                four = "MAY"
    '                                five = "APR"
    '                                six = "MAR"
    '                            ElseIf Month = "09" Then
    '                                one = "SEP"
    '                                two = "AUG"
    '                                three = "JUL"
    '                                four = "JUN"
    '                                five = "MAY"
    '                                six = "APR"
    '                            ElseIf Month = "10" Then
    '                                one = "OCT"
    '                                two = "SEP"
    '                                three = "AUG"
    '                                four = "JUL"
    '                                five = "JUN"
    '                                six = "MAY"
    '                            ElseIf Month = "11" Then
    '                                one = "NOV"
    '                                two = "OCT"
    '                                three = "SEP"
    '                                four = "AUG"
    '                                five = "JUL"
    '                                six = "JUN"
    '                            ElseIf Month = "12" Then
    '                                one = "DEC"
    '                                two = "NOV"
    '                                three = "OCT"
    '                                four = "SEP"
    '                                five = "AUG"
    '                                six = "JUL"
    '                            End If
    '                        Else
    '                            one = "<=0-30"
    '                            two = "<=31-60"
    '                            three = "<=61-90"
    '                            four = "<=91-120"
    '                            five = "<=121-150"
    '                            six = "Over 150"
    '                        End If

    '                        rowcount = rowcount + 2

    '                        ws.Cell(rowcount, 1).Value = "Ageing Analysis Of Balance"
    '                        ws.Range(rowcount, 1, rowcount, 6).Merge().Style.Font.SetBold().Font.FontSize = 10
    '                        'phrase.Add(New Chunk("Ageing Analysis Of Balance", NormalFontBold))

    '                        rowcount = rowcount + 1
    '                        Dim arrow4() As String = {"BALANCE", "<0", one, two, three, four, five, six}
    '                        ws.Range(rowcount, 1, rowcount, 8).Style.Font.SetBold.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Bottom).Font.FontSize = 9
    '                        ws.Range(rowcount, 1, rowcount, 8).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '                        For i = 0 To 7
    '                            ws.Cell(rowcount, i + 1).Value = arrow4(i)
    '                        Next

    '                        Dim arr7() As Decimal
    '                        arr7 = {dr3(0)("balance"), dr3(0)("age9"), dr3(0)("age1"), dr3(0)("age2"), dr3(0)("age3"), dr3(0)("age4"),
    '                                  dr3(0)("age5"), dr3(0)("age6")}
    '                        rowcount = rowcount + 1
    '                        ws.Range(rowcount, 1, rowcount, arr7.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Bottom).Font.FontSize = 9
    '                        ws.Range(rowcount, 1, rowcount, arr7.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)

    '                        For i = 0 To 7
    '                            If i = 0 Then
    '                                Dim bal As String = IIf(arr7(i) <= 0, Decimal.Parse((Math.Abs(arr7(i)))).ToString("N" + currDecno.ToString) + IIf(arr7(i) = 0, "", " Cr"), arr7(i).ToString("N" + currDecno.ToString) + " Dr")
    '                                ws.Cell(rowcount, i + 1).Value = bal
    '                                ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                            Else
    '                                ws.Cell(rowcount, i + 1).Value = arr7(i)
    '                                ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = DecimalPoint
    '                            End If
    '                        Next
    '                        rowcount = rowcount + 3
    '                        fdebit = 0.0
    '                        fcredit = 0.0
    '                        fbalance = 0
    '                        mdebit = 0
    '                        mcredit = 0
    '                        mbalance = 0
    '                        cumbal = 0

    '                    End If
    '                Next
    '            End If

    '        Else
    '            ws.Cell("A" & rowcount).Value = custSupType

    '            Dim company = ws.Range("A" & rowcount & ":D" & rowcount).Merge()
    '            company.Style.Font.SetBold().Font.FontSize = 15

    '            rowcount = rowcount + 1
    '            ws.Cell("A" & rowcount).Value = reportfilter

    '            Dim filter = ws.Range("A" & rowcount & ":M" & rowcount).Merge()
    '            filter.Style.Font.SetBold().Alignment.SetWrapText().Font.FontSize = 13

    '            Dim rowheight As Integer
    '            If reportfilter.Length > 140 Then
    '                rowheight = IIf(reportfilter.Length > 140 And reportfilter.Length < 240, 32, IIf(reportfilter.Length > 240 And reportfilter.Length < 340, 48, 64))
    '                ws.Row(rowcount).Height = rowheight
    '            End If


    '        End If

    '        ws.Cell((rowcount + 2), 1).Value = "Printed Date:" & Now.ToString("dd/MM/yyyy")
    '        ws.Range((rowcount + 2), 1, (rowcount + 2), 4).Merge()
    '        Using wStream As New MemoryStream()
    '            wb.SaveAs(wStream)
    '            bytes = wStream.ToArray()
    '        End Using
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub

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

    'Public Sub GenerateReportRV(ByVal trantype As String, ByVal tranid As String, ByVal divcode As String, ByVal CashBankType As String, ByVal PrntSec As Integer, ByVal PrntCliCurr As Integer, ByVal PrinDocTitle As String, ByRef bytes() As Byte, ByVal printMode As String)

    '    Try
    '        Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 30.0F, 35.0F)
    '        Dim documentWidth As Single
    '        documentWidth = 550.0F
    '        Dim rptreportname As String = Nothing
    '        Dim receipt_master As New DataTable
    '        Dim conn1 As New SqlConnection
    '        conn1 = clsDBConnect.dbConnectionnew("strDBConnection")
    '        Dim strSql As String
    '        Dim sqlcmd As New SqlCommand("sp_rpt_receipt", conn1)
    '        sqlcmd.CommandType = CommandType.StoredProcedure
    '        sqlcmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = tranid
    '        sqlcmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
    '        sqlcmd.Parameters.Add(New SqlParameter("@prtClientcurr", SqlDbType.Int)).Value = PrntCliCurr
    '        Using dad As New SqlDataAdapter
    '            dad.SelectCommand = sqlcmd
    '            dad.Fill(receipt_master)
    '        End Using
    '        clsDBConnect.dbCommandClose(sqlcmd)
    '        clsDBConnect.dbConnectionClose(conn1)
    '        Dim receiptmaster() As System.Data.DataRow
    '        receiptmaster = receipt_master.Select("tran_id='" & tranid & "'")

    '        'If receiptmaster.Length = 0 Then
    '        '    strSql = "SELECT receipt_master_new.tran_id, receipt_master_new.receipt_date,  receipt_master_new.receipt_currency_id , receipt_master_new.basecredit, " & _
    '        '    "receipt_master_new.receipt_received_from, receipt_detail.basedebit As rddebit, receipt_detail.basecredit As rdcredit,receipt_detail.receipt_currency_id as receipt_currency_id_clicurr ," & _
    '        '    "receipt_detail.receipt_debit as receipt_debit ,receipt_detail.receipt_credit as receipt_credit , receipt_master_new.receipt_cashbank_type, receipt_master_new.receipt_cheque_number, " & _
    '        '    "view_account.des, receipt_master_new.receipt_narration, receipt_detail.receipt_acc_code, receipt_detail.receipt_acc_type, receipt_detail.costcenter_code, " & _
    '        '    "receipt_detail.receipt_narration As narration, customer_bank_master.other_bank_master_des, receipt_master_new.receipt_cashbank_code, view_account_1.des As dest, " & _
    '        '    "receipt_master_new.basedebit, acctmast.acctname, '' costcenter_name, view_account.controlacctcode, UserMaster.UserName, receipt_detail.tran_id, " & _
    '        '    "receipt_detail.div_id, receipt_master_new.receipt_mrv, receipt_detail.tran_lineno FROM   ((((((dbo.receipt_detail receipt_detail INNER JOIN dbo.receipt_master_new receipt_master_new " & _
    '        '    "ON ((receipt_detail.tran_type=receipt_master_new.tran_type) AND (receipt_detail.tran_id=receipt_master_new.tran_id)) AND (receipt_detail.div_id=receipt_master_new.div_id)) " & _
    '        '    "INNER JOIN dbo.view_account view_account ON ((receipt_detail.receipt_acc_code=view_account.code) AND (receipt_detail.receipt_acc_type=view_account.type)) " & _
    '        '    "AND (receipt_detail.div_id=view_account.div_code))) LEFT OUTER JOIN dbo.customer_bank_master customer_bank_master ON " & _
    '        '    "receipt_master_new.receipt_customer_bank=customer_bank_master.other_bank_master_code) INNER JOIN dbo.view_account view_account_1 ON " & _
    '        '    "(receipt_master_new.receipt_cashbank_code=view_account_1.code) AND (receipt_master_new.receipt_div_id=view_account_1.div_code)) " & _
    '        '    "LEFT OUTER JOIN dbo.UserMaster UserMaster ON receipt_master_new.adduser=UserMaster.UserCode) LEFT OUTER JOIN dbo.acctmast acctmast ON " & _
    '        '    "(view_account.controlacctcode=acctmast.acctcode) AND (view_account.div_code=acctmast.div_code) where receipt_master_new.tran_id ='" + tranid + "' " & _
    '        '    "and receipt_master_new.receipt_div_id='" + divcode + "' ORDER BY receipt_master_new.tran_id, receipt_detail.tran_lineno"

    '        '    'LEFT OUTER JOIN dbo.costcenter_master costcenter_master ON receipt_detail.costcenter_code=costcenter_master.costcenter_code

    '        '    Using dad As New SqlDataAdapter(strSql, conn1)
    '        '        dad.Fill(receipt_master)
    '        '    End Using


    '        '    receiptmaster = receipt_master.Select("tran_id='" & tranid & "'")
    '        'End If
    '        Dim voccurr, curr As String

    '        curr = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)

    '        If PrntCliCurr = 1 Then
    '            'voccurr = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select top 1 isnull(receipt_currency_id,'') from receipt_Detail  where tran_id='" & tranid & "' and div_id='" & divcode & "' and receipt_acc_type<>'G' "), String)
    '            If receiptmaster.Length > 0 Then
    '                voccurr = receiptmaster(0)("receipt_currency_id")
    '            End If
    '        Else
    '            voccurr = curr
    '        End If
    '        If voccurr = "" Then
    '            voccurr = curr
    '        End If

    '        Dim lsCtry As String = objutils.GetString(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=459")

    '        Dim decnum As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), String)
    '        Dim decno As String = "N" + decnum
    '        Dim coadd1 As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coadd1 from Columbusmaster where div_code='" & divcode & "'"), String)
    '        Dim coadd2 As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coadd2 from Columbusmaster where div_code='" & divcode & "'"), String)
    '        Dim copobox As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select copobox from Columbusmaster where div_code='" & divcode & "'"), String)
    '        Dim cotel As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select cotel from Columbusmaster where div_code='" & divcode & "'"), String)
    '        Dim cofax As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select cofax from Columbusmaster where div_code='" & divcode & "'"), String)
    '        Dim coemail As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coemail from Columbusmaster where div_code='" & divcode & "'"), String)
    '        Dim coweb As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coweb from Columbusmaster where div_code='" & divcode & "'"), String)
    '        Dim TRNNo As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select TRNNo from Columbusmaster where div_code='" & divcode & "'"), String)

    '        Dim settle_details As New DataTable
    '        Dim conn As New SqlConnection
    '        conn = clsDBConnect.dbConnectionnew("strDBConnection")
    '        sqlcmd = New SqlCommand("sp_rpt_receipt_settle", conn)
    '        sqlcmd.CommandType = CommandType.StoredProcedure
    '        sqlcmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = tranid
    '        sqlcmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = divcode
    '        Using dad As New SqlDataAdapter
    '            dad.SelectCommand = sqlcmd
    '            dad.Fill(settle_details)
    '        End Using
    '        clsDBConnect.dbCommandClose(sqlcmd)
    '        clsDBConnect.dbConnectionClose(conn)
    '        Dim settle_detail() As System.Data.DataRow
    '        settle_detail = settle_details.Select("against_tran_id='" & tranid & "'")

    '        Dim decimalword As String = Nothing
    '        Dim decimalInword As String = Nothing
    '        Dim fractionword As String = Nothing
    '        'Dim rdcredit As Decimal = 0
    '        Dim fraction As Decimal
    '        'Dim fraction As Decimal = Math.Round(Decimal.Parse(receiptmaster(0)("basecredit")), 2) - Math.Truncate(Math.Round(Decimal.Parse(receiptmaster(0)("basecredit")), 3))
    '        Dim recAmt As Decimal
    '        Dim convRate As Decimal  'Modified param 02/02/2020
    '        If receiptmaster.Length <> 0 Then
    '            If PrntCliCurr = 1 Then
    '                convRate = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select receipt_currency_rate from receipt_master_new where tran_id='" & tranid & "' and receipt_div_id='" & divcode & "'"), String)
    '                recAmt = Math.Round(receiptmaster(0)("receipt_amount") / convRate, Convert.ToInt32(decnum))
    '                decimalword = Math.Truncate(Decimal.Parse(recAmt)).ToString()
    '                decimalInword = AmountInWords(decimalword)
    '                fraction = Math.Round(Decimal.Parse(recAmt), Convert.ToInt32(decnum), MidpointRounding.AwayFromZero) Mod 1
    '            Else
    '                'Dim TotalBaseCredit As Decimal = receipt_master.AsEnumerable().Where(Function(row) row.Field(Of String)("receipt_acc_type") <> "G").Sum(Function(row) row.Field(Of Decimal)("rdcredit"))
    '                'Dim TotalBaseDebit As Decimal = receipt_master.AsEnumerable().Where(Function(row) row.Field(Of String)("receipt_acc_type") <> "G").Sum(Function(row) row.Field(Of Decimal)("rddebit"))
    '                'rdcredit = TotalBaseCredit - TotalBaseDebit  

    '                decimalword = Math.Truncate(Decimal.Parse(receiptmaster(0)("receipt_amount"))).ToString()
    '                decimalInword = AmountInWords(decimalword)
    '                fraction = Math.Round(Decimal.Parse(receiptmaster(0)("receipt_amount")), Convert.ToInt32(decnum), MidpointRounding.AwayFromZero) Mod 1
    '                convRate = 1
    '            End If
    '        End If
    '        'Modified Param 15/11/2018

    '        If fraction > 0 Then
    '            Dim arrFraction As String() = fraction.ToString.Split(".")
    '            If arrFraction.Length = 2 Then
    '                fraction = arrFraction(1)
    '            Else
    '                fraction = 0
    '            End If
    '        End If
    '        If fraction > 0 Then
    '            Dim fractionStr As String = fraction.ToString()
    '            While fractionStr.Length < decnum
    '                fractionStr = fractionStr + "0"
    '            End While
    '            Dim coin As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select currcoin from currmast where currcode= '" & voccurr.Trim & "'"), String)
    '            fractionword = "AND " & coin & "  " & AmountInWords(fractionStr) + "  " + "ONLY"
    '            'If decnum.Equals("2") Then
    '            '    fractionword = (fraction.ToString()).Split(New Char() {"."})(1) & "/100" + " " + "FILS ONLY"
    '            'ElseIf decnum.Equals("3") AndAlso trantype.Equals("RV") Then
    '            '    fractionword = (fraction.ToString()).Split(New Char() {"."})(1) & "/1000" + " " + "BAIZA ONLY"
    '            'ElseIf decnum.Equals("3") AndAlso trantype.Equals("CRV") Then
    '            '    fractionword = (fraction.ToString()).Split(New Char() {"."})(1) & "/1000" + " " + "FILS ONLY"
    '            'End If
    '        Else
    '            decimalInword = UCase(decimalInword) + "  " + "ONLY"
    '            fractionword = ""
    '        End If

    '        Using memoryStream As New System.IO.MemoryStream()
    '            Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)

    '            Dim phrase As Phrase = Nothing
    '            Dim cell As PdfPCell = Nothing
    '            Dim tableheader As PdfPTable = Nothing

    '            'Header Table

    '            tableheader = New PdfPTable(2)
    '            tableheader.TotalWidth = documentWidth
    '            tableheader.LockedWidth = True
    '            tableheader.SetWidths(New Single() {0.75F, 0.25F})

    '            tableheader.Complete = False
    '            tableheader.SplitRows = False
    '            tableheader.SpacingBefore = 10.0F
    '            tableheader.WidthPercentage = 100
    '            'company name
    '            If divcode = "02" Then
    '                cell = ImageCell("~/Images/logo.jpg", 80.0F, PdfPCell.ALIGN_LEFT)
    '            Else
    '                cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
    '            End If
    '            tableheader.AddCell(cell)

    '            phrase = New Phrase()
    '            phrase.Add(New Chunk(coadd1 & Environment.NewLine, normalfontgray))
    '            phrase.Add(New Chunk(coadd2 & Environment.NewLine, normalfontgray))
    '            phrase.Add(New Chunk("PO Box" & Space(3) & ":" & Space(6) & copobox & Environment.NewLine, normalfontgray))
    '            'phrase.Add(New Chunk("Email :" & Space(6) & coemail & Environment.NewLine, normalfont))
    '            phrase.Add(New Chunk("Tel" & Space(10) & ":" & Space(6) & cotel & Environment.NewLine, normalfontgray))
    '            'phrase.Add(New Chunk("Fax    :" & Space(6) & cofax & Environment.NewLine, normalfont))
    '            phrase.Add(New Chunk("Web" & Space(8) & ":" & Space(6) & coweb & Environment.NewLine, normalfontgray))
    '            'changed by Christo on 02/01/2019
    '            'Dim lsCtry As String = objutils.GetString(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=459")
    '            'If lsCtry.Trim.ToUpper = "OM" Then
    '            '    ' TRN not required for Oman 
    '            'Else
    '            phrase.Add(New Chunk("TRN" & Space(8) & ":" & Space(6) & TRNNo & Environment.NewLine, normalfontgray))
    '            'End If
    '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '            cell.PaddingBottom = 5.0F
    '            cell.SetLeading(12, 0)
    '            tableheader.AddCell(cell)


    '            Dim tblTitle As PdfPTable = New PdfPTable(1)
    '            tblTitle.SetWidths(New Single() {1.0F})
    '            tblTitle.TotalWidth = documentWidth
    '            tblTitle.LockedWidth = True
    '            phrase = New Phrase()
    '            If trantype.Equals("RV") Then
    '                rptreportname = "Receipt Voucher"
    '                phrase.Add(New Chunk(rptreportname, headerfont))
    '                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
    '                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
    '                cell.PaddingBottom = 3.0F

    '            End If

    '            tblTitle.AddCell(cell)
    '            tblTitle.SpacingBefore = 8
    '            tblTitle.SpacingAfter = 10

    '            Dim FooterTable = New PdfPTable(1)
    '            FooterTable.TotalWidth = documentWidth
    '            FooterTable.LockedWidth = True
    '            FooterTable.SetWidths(New Single() {1.0F})
    '            FooterTable.Complete = False
    '            FooterTable.SplitRows = False
    '            phrase = New Phrase()
    '            phrase.Add(New Chunk("Printed Date:" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss"), normalfont))
    '            cell = New PdfPCell(phrase)
    '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '            cell.Colspan = 2
    '            cell.SetLeading(12, 0)
    '            cell.PaddingBottom = 3
    '            FooterTable.SpacingBefore = 12.0F
    '            FooterTable.AddCell(cell)
    '            FooterTable.Complete = True


    '            'add common header and footer part to every page
    '            writer.PageEvent = New ClsHeaderFooter(tableheader, tblTitle, FooterTable, Nothing, "Voucher")
    '            document.Open()

    '            Dim collType As String

    '            Dim cheque As Boolean = False
    '            Dim ccredit As Boolean = False
    '            Dim cash As Boolean = False
    '            Dim banktransfer As Boolean = False

    '            Dim cheque_number As String = receiptmaster(0)("receipt_cheque_number").ToString
    '            Dim bank_type As String = receiptmaster(0)("receipt_cashbank_type").ToString
    '            Dim bank_transfer As String = receiptmaster(0)("other_bank_master_des").ToString

    '            If bank_type.Equals("B") AndAlso (cheque_number.ToUpper().IndexOf("CC") <> 1 AndAlso cheque_number.ToUpper().IndexOf("CASH") <> 1) Then
    '                cheque = True
    '                collType = "Cheque"

    '            End If
    '            If bank_type.Equals("B") AndAlso (cheque_number.ToUpper().IndexOf("CC") = 1) Then
    '                ccredit = True
    '                collType = "C.Card"
    '            End If
    '            If bank_type.Equals("C") Then
    '                cash = True
    '                collType = "Cash"
    '            End If

    '            If UCase(bank_transfer) = "BANK TRANSFER" Then
    '                banktransfer = True
    '                cheque = False
    '                cash = False
    '                ccredit = False
    '                collType = "Bank Transfer"
    '            End If

    '            Dim tblcommon As PdfPTable = New PdfPTable(1)
    '            tblcommon.SetWidths(New Single() {1.0F})
    '            tblcommon.TotalWidth = documentWidth
    '            tblcommon.LockedWidth = True
    '            'Dim tbl As PdfPTable = New PdfPTable(7)
    '            'tbl.SetWidths(New Single() {0.15F, 0.05F, 0.15F, 0.05F, 0.15F, 0.05F, 0.4F})
    '            Dim tbl As PdfPTable = New PdfPTable(9)
    '            tbl.SetWidths(New Single() {0.3F, 0.05F, 0.15F, 0.05F, 0.15F, 0.05F, 0.3F, 0.05F, 0.3F})
    '            If receiptmaster.Length <> 0 Then
    '                phrase = New Phrase()
    '                phrase.Add(New Chunk("Voucher No." & Space(10), normalfontbold))
    '                phrase.Add(New Chunk(receiptmaster(0)("tran_id").ToString() & Space(25), normalfontbold))
    '                phrase.Add(New Chunk("Date" & Space(10), normalfontbold))
    '                phrase.Add(New Chunk(Format(Convert.ToDateTime(receiptmaster(0)("receipt_date").ToString()), "dd-MM-yyyy") & Space(25), normalfont))

    '                If PrntCliCurr = 1 Then
    '                    phrase.Add(New Chunk("Amount(" & voccurr & ")" & Space(10), normalfontbold))
    '                    phrase.Add(New Chunk(Decimal.Parse(recAmt).ToString(decno) & Environment.NewLine & vbLf & vbLf, normalfont))
    '                Else
    '                    phrase.Add(New Chunk("Amount(" & voccurr & ")" & Space(10), normalfontbold))
    '                    phrase.Add(New Chunk(Decimal.Parse(receiptmaster(0)("receipt_Amount")).ToString(decno) & Environment.NewLine & vbLf & vbLf, normalfont))
    '                End If
    '                ' If trantype.Equals("RV") Then
    '                phrase.Add(New Chunk("M.RV No." & Space(15), normalfontbold))
    '                phrase.Add(New Chunk(receiptmaster(0)("receipt_mrv").ToString() & Environment.NewLine & vbLf & vbLf, normalfont))
    '                phrase.Add(New Chunk("Received From" & Space(5), normalfontbold))
    '                phrase.Add(New Chunk(receiptmaster(0)("receipt_received_from").ToString() & vbLf, normalfont))
    '                phrase.Add(New Chunk("     " & Space(20) & "------------------------------------------------------------------------------------------------------------------" & Environment.NewLine & vbLf))
    '                'Else
    '                '    phrase.Add(New Chunk("Paid For" & Space(16), normalfontbold))
    '                '    phrase.Add(New Chunk(receiptmaster(0)("receipt_received_from").ToString() & vbLf & vbLf & vbLf, normalfont))
    '                '  End If
    '            End If
    '            phrase.Add(New Chunk("Amount" & Space(17), normalfontbold))
    '            phrase.Add(New Chunk(voccurr & "  ", normalfont))
    '            phrase.Add(New Chunk(decimalInword.ToUpper() & Space(1) & fractionword.ToUpper() & Environment.NewLine & vbLf & vbLf, normalfont))

    '            'If trantype.Equals("CRV") Then
    '            ' phrase.Add(New Chunk("Received By" & Space(10) & "----------------------------------------------------" & Space(10) & "Signature" & Space(10) & "----------------------------------------------------" & vbLf & vbLf, normalfontbold))
    '            ' End If
    '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '            cell.PaddingBottom = 3.0F
    '            cell.PaddingTop = 10.0F
    '            'cell.Colspan = 7
    '            cell.Colspan = 9
    '            tbl.AddCell(cell)
    '            '  If trantype.Equals("RV") Then
    '            phrase = New Phrase()
    '            phrase.Add(New Chunk("Collection", normalfontbold))
    '            phrase.Add(New Chunk("Type", normalfontbold))
    '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '            cell.PaddingBottom = 3.0F
    '            'cell.Width = 30.0F
    '            cell.Colspan = 1
    '            tbl.AddCell(cell)



    '            cell = IIf(cash, ImageCell("~/Images/checked.png", 35.0F, PdfPCell.ALIGN_LEFT), ImageCell("~/Images/uncheck.png", 35.0F, PdfPCell.ALIGN_LEFT))
    '            cell.Colspan = 1
    '            cell.PaddingTop = 3
    '            cell.PaddingLeft = 3
    '            tbl.AddCell(cell)
    '            phrase = New Phrase()
    '            phrase.Add(New Chunk("Cash", normalfontbold))
    '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '            cell.PaddingBottom = 3.0F
    '            cell.Colspan = 1
    '            tbl.AddCell(cell)

    '            cell = IIf(ccredit, ImageCell("~/Images/checked.png", 35.0F, PdfPCell.ALIGN_LEFT), ImageCell("~/Images/uncheck.png", 35.0F, PdfPCell.ALIGN_LEFT))
    '            cell.PaddingTop = 3
    '            cell.Colspan = 1
    '            tbl.AddCell(cell)
    '            phrase = New Phrase()
    '            phrase.Add(New Chunk("C.Card", normalfontbold))
    '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '            cell.PaddingBottom = 3.0F
    '            cell.Colspan = 1
    '            tbl.AddCell(cell)

    '            cell = IIf(cheque, ImageCell("~/Images/checked.png", 35.0F, PdfPCell.ALIGN_LEFT), ImageCell("~/Images/uncheck.png", 35.0F, PdfPCell.ALIGN_LEFT))
    '            cell.Colspan = 1
    '            cell.PaddingTop = 2
    '            tbl.AddCell(cell)
    '            phrase = New Phrase()
    '            phrase.Add(New Chunk("Cheque" & Environment.NewLine & vbLf & vbLf, normalfontbold))
    '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '            cell.PaddingBottom = 3.0F
    '            cell.Colspan = 1
    '            tbl.AddCell(cell)

    '            '''' Bank Transfer - christo - 22/12/18
    '            cell = IIf(banktransfer, ImageCell("~/Images/checked.png", 35.0F, PdfPCell.ALIGN_LEFT), ImageCell("~/Images/uncheck.png", 35.0F, PdfPCell.ALIGN_LEFT))
    '            cell.Colspan = 1
    '            cell.PaddingTop = 2
    '            tbl.AddCell(cell)
    '            phrase = New Phrase()
    '            phrase.Add(New Chunk("Bank Transfer" & Environment.NewLine & vbLf & vbLf, normalfontbold))
    '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '            cell.PaddingBottom = 3.0F
    '            cell.Colspan = 1
    '            tbl.AddCell(cell)
    '            If receiptmaster.Length <> 0 Then
    '                '''''''''''''''''''''''''''''''''''''''
    '                phrase = New Phrase()
    '                phrase.Add(New Chunk("Cheque No." & Space(11), normalfontbold))
    '                phrase.Add(New Chunk(receiptmaster(0)("receipt_cheque_number").ToString() & Space(50), normalfont))
    '                phrase.Add(New Chunk("Bank/Branch" & Space(9), normalfontbold))
    '                phrase.Add(New Chunk(receiptmaster(0)("other_bank_master_des").ToString() & vbLf, normalfont))
    '                phrase.Add(New Chunk("          " & Space(15) & "---------------------------" & Space(25) & "         " & "------------------------------" & Environment.NewLine & vbLf))
    '                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                cell.PaddingBottom = 2.0F
    '                'cell.Colspan = 7
    '                cell.Colspan = 9

    '                tbl.AddCell(cell)
    '            End If
    '            phrase = New Phrase()
    '            phrase.Add(New Chunk("Description", normalfontbold))
    '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '            cell.PaddingBottom = 3.0F
    '            'cell.Colspan = 1
    '            cell.Colspan = 2
    '            cell.Border = Rectangle.NO_BORDER
    '            tbl.AddCell(cell)
    '            phrase = New Phrase()
    '            phrase.Add(New Chunk(receiptmaster(0)("receipt_narration").ToString() & vbLf & vbLf & vbLf & vbLf & vbLf & vbLf, normalfont))
    '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '            cell.PaddingBottom = 3.0F
    '            cell.Border = Rectangle.NO_BORDER
    '            'cell.Colspan = 6
    '            cell.Colspan = 7

    '            tbl.AddCell(cell)
    '            phrase = New Phrase()
    '            phrase.Add(New Chunk("Prepared By:" & Space(10) & Space(10) & Space(10), normalfontbold))
    '            'phrase.Add(New Chunk("Checked By:" & Space(15) & Space(20) & Space(20), normalfontbold))
    '            'changed by Christo on 02/01/2019
    '            'If lsCtry.Trim.ToUpper = "OM" Then
    '            '    phrase.Add(New Chunk("Director of Finance:" & Space(15) & Space(15), normalfontbold))
    '            '    phrase.Add(New Chunk("General Manager:" & Space(5) & Space(5) & Environment.NewLine & vbLf, normalfontbold))
    '            'Else
    '            phrase.Add(New Chunk("Checked By:" & Space(10) & Space(10) & Space(10), normalfontbold))
    '            phrase.Add(New Chunk("Approved By:" & Space(10) & Space(10) & Environment.NewLine & vbLf, normalfontbold))
    '            'End If

    '            phrase.Add(New Chunk(receiptmaster(0)("UserName").ToString() & Space(20) & Space(20), normalfont))
    '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '            cell.PaddingBottom = 3.0F
    '            cell.PaddingTop = 100.0F
    '            'cell.Colspan = 7
    '            cell.Colspan = 9

    '            tbl.AddCell(cell)

    '            If trantype.Equals("RV") Then
    '                tblcommon.AddCell(tbl)
    '                tblcommon.SpacingBefore = 0
    '                tblcommon.Complete = True
    '                document.Add(tblcommon)
    '            End If

    '            'common params
    '            Dim arrow3() As String = Nothing
    '            Dim receipt() As String = Nothing
    '            Dim sumtotal() As String = Nothing
    '            Dim acctname As String = Nothing
    '            Dim costcenter_code As String = Nothing
    '            Dim rdebit As Decimal = rdebit + Decimal.Parse(receiptmaster(0)("basecredit"))
    '            Dim rcredit As Decimal = rcredit + Decimal.Parse(receiptmaster(0)("basedebit"))
    '            Dim debit As String = IIf(Decimal.Parse(receiptmaster(0)("basedebit")) = 0.0, "-", Decimal.Parse(receiptmaster(0)("basedebit")).ToString(decno))
    '            Dim credit As String = IIf(Decimal.Parse(receiptmaster(0)("basecredit")) = 0.0, "-", Decimal.Parse(receiptmaster(0)("basecredit")).ToString(decno))

    '            '   arrow3 = {"Account No.", "Account Name / Description ", "Debit(" + curr + ")", "Credit(" + curr + ")", receiptmaster(0)("receipt_cashbank_code").ToString(), receiptmaster(0)("dest").ToString() + Environment.NewLine + Environment.NewLine + receiptmaster(0)("receipt_narration").ToString(), credit, debit}


    '            arrow3 = {"S.No.", "Account Code.", "Account Head", "Narration", "Amount(" & voccurr & ")"}
    '            If trantype.Equals("CRV") Then
    '                cashReceiptVoucher(document, decno, receiptmaster, settle_detail, tbl, arrow3, rdebit, rcredit, normalfont, normalfontbold, documentWidth)
    '            ElseIf PrntSec = 1 AndAlso (trantype.Equals("RV") Or trantype.Equals("CV")) Then


    '                document.NewPage()

    '                Dim tbl3 As PdfPTable = New PdfPTable(5)

    '                tbl3.SetWidths(New Single() {0.07, 0.18F, 0.2F, 0.4F, 0.15F})

    '                'Dim tblhead As PdfPTable = New PdfPTable(2)

    '                'tblhead.SetWidths(New Single() {0.5F, 0.5F})
    '                'tblhead.TotalWidth = documentWidth
    '                'tblhead.SplitRows = False
    '                'tblhead.Complete = False
    '                'tblhead.LockedWidth = True

    '                tbl3.TotalWidth = documentWidth
    '                tbl3.SplitRows = False
    '                tbl3.Complete = False
    '                tbl3.LockedWidth = True

    '                phrase = New Phrase()
    '                phrase.Add(New Chunk("RV No." & Space(10), normalfontbold))
    '                phrase.Add(New Chunk(receiptmaster(0)("tran_id").ToString() & Space(60), normalfont))


    '                phrase.Add(New Chunk(Space(60) & "Date" & Space(10), normalfontbold))
    '                phrase.Add(New Chunk(Format(Convert.ToDateTime(receiptmaster(0)("receipt_date").ToString()), "dd-MM-yyyy") & Environment.NewLine & vbLf, normalfont))

    '                phrase.Add(New Chunk("Received From" & Space(10), normalfontbold))
    '                phrase.Add(New Chunk(receiptmaster(0)("receipt_received_from").ToString() & Environment.NewLine & vbLf, normalfont))




    '                phrase.Add(New Chunk(collType & Space(10), normalfontbold))

    '                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                cell.PaddingTop = 3.0F
    '                cell.PaddingBottom = 3.0F
    '                cell.Colspan = 5
    '                cell.Border = Rectangle.TOP_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.BOTTOM_BORDER
    '                tbl3.AddCell(cell)


    '                phrase = New Phrase()
    '                phrase.Add(New Chunk(" " & vbLf, footerdfont))
    '                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                cell.PaddingBottom = 2.0F
    '                cell.Colspan = 5
    '                tbl3.AddCell(cell)

    '                If settle_detail.Length > 0 Then
    '                    ' Sub Table-Adjust Bill Detail
    '                    Dim tbl4 As PdfPTable = New PdfPTable(8)
    '                    tbl4.SetWidths(New Single() {0.11F, 0.1F, 0.11F, 0.15F, 0.12F, 0.11F, 0.15F, 0.15F})
    '                    tbl4.TotalWidth = documentWidth

    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk("Settlement Details" & vbLf & vbLf, footerdfont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                    cell.PaddingBottom = 3.0F
    '                    cell.Colspan = 8
    '                    cell.Border = Rectangle.BOTTOM_BORDER Or Rectangle.LEFT_BORDER Or Rectangle.RIGHT_BORDER Or Rectangle.TOP_BORDER
    '                    tbl4.AddCell(cell)

    '                    Dim arrData2() As String
    '                    arrData2 = {"Date", "Voucher Type", "Voucher No.", settle_detail(0)("field2").ToString(), settle_detail(0)("field3").ToString(), "Due Date", "Amount Adjusted"}

    '                    For i = 0 To arrData2.Length - 1
    '                        phrase = New Phrase()
    '                        phrase.Add(New Chunk(arrData2(i), normalfontbold))
    '                        If i = 6 Then
    '                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
    '                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                            cell.Colspan = 2
    '                        Else
    '                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
    '                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                        End If
    '                        cell.PaddingBottom = 4.0F
    '                        cell.PaddingTop = 1.0F
    '                        cell.BackgroundColor = New BaseColor(215, 215, 215)
    '                        If i <= 5 Then
    '                            cell.BorderWidthBottom = 0
    '                        End If
    '                        tbl4.AddCell(cell)
    '                    Next

    '                    arrData2 = {"", "", "", "", "", "", settle_detail(0)("receipt_currency_id").ToString(), curr}
    '                    Dim saleDecno As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select nodigit from currmast where currcode= '" & settle_detail(0)("receipt_currency_id").ToString() & "'"), String)
    '                    For i = 0 To arrData2.Length - 1
    '                        phrase = New Phrase()
    '                        phrase.Add(New Chunk(arrData2(i), normalfontbold))
    '                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
    '                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                        cell.PaddingBottom = 4.0F
    '                        cell.Colspan = 1
    '                        cell.PaddingTop = 1.0F
    '                        cell.BackgroundColor = New BaseColor(215, 215, 215)
    '                        If i <= 5 Then
    '                            cell.BorderWidthTop = 0
    '                        End If
    '                        tbl4.AddCell(cell)
    '                    Next
    '                    Dim totalodebit As Decimal
    '                    Dim totalbdebit As Decimal
    '                    'Table Data
    '                    For i = 0 To settle_detail.Length - 1
    '                        arrData2 = {settle_detail(i)("tran_date"), settle_detail(i)("tran_type"), settle_detail(i)("tran_id"), settle_detail(i)("open_field2"), settle_detail(i)("open_field3"), settle_detail(i)("open_due_date"), Decimal.Parse(settle_detail(i)("open_credit")).ToString("N" + saleDecno), Decimal.Parse(settle_detail(i)("base_credit")).ToString(decno)}
    '                        totalodebit = totalodebit + Decimal.Parse(settle_detail(i)("open_credit"))
    '                        totalbdebit = totalbdebit + Decimal.Parse(settle_detail(i)("base_credit"))
    '                        For j = 0 To arrData2.Length - 1
    '                            phrase = New Phrase()
    '                            phrase.Add(New Chunk(arrData2(j), normalfont))
    '                            If j > 5 Then
    '                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
    '                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT

    '                            Else
    '                                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
    '                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                            End If
    '                            cell.PaddingBottom = 4.0F
    '                            cell.PaddingTop = 1.0F
    '                            tbl4.AddCell(cell)
    '                        Next
    '                    Next

    '                    arrData2 = {"", "", "", "", "", "Total", totalodebit.ToString("N" + saleDecno), totalbdebit.ToString(decno)}

    '                    For i = 0 To arrData2.Length - 1
    '                        phrase = New Phrase()
    '                        phrase.Add(New Chunk(arrData2(i), normalfontbold))

    '                        If i > 5 Then
    '                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
    '                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT

    '                        Else
    '                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
    '                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                        End If
    '                        cell.PaddingBottom = 4.0F
    '                        cell.PaddingTop = 1.0F
    '                        tbl4.AddCell(cell)
    '                    Next
    '                    ' document.Add(tbl4)
    '                End If


    '                For i = 0 To arrow3.Length - 1
    '                    phrase = New Phrase()

    '                    phrase.Add(New Chunk(arrow3(i), normalfontbold))

    '                    If i = 4 Then
    '                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
    '                        cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                    Else
    '                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                    End If


    '                    '  cell.BackgroundColor = Header
    '                    cell.PaddingBottom = 3.0F


    '                    tbl3.AddCell(cell)
    '                Next
    '                Dim sumamt As Decimal
    '                Dim count As Integer = 0
    '                For i = 0 To receiptmaster.Length - 1
    '                    Dim excludeRow As String = ""
    '                    If PrntCliCurr = 1 And receiptmaster(i)("receipt_acc_type") = "G" Then
    '                        excludeRow = Convert.ToString(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(acctCode,'') from AcctAutomatCalculate where div_code='" & divcode & "' and acctcode='" & receiptmaster(i)("receipt_acc_code") & "'"))
    '                    End If
    '                    If (PrntCliCurr = 1 And excludeRow = "") Or PrntCliCurr = 0 Then
    '                        count += 1
    '                        acctname = IIf(Not (TypeOf receiptmaster(i)("acctname") Is DBNull), receiptmaster(i)("acctname").ToString(), receiptmaster(i)("costcenter_name").ToString())
    '                        costcenter_code = IIf(Not (TypeOf receiptmaster(i)("costcenter_code") Is DBNull), receiptmaster(i)("costcenter_code").ToString(), receiptmaster(i)("controlacctcode").ToString())
    '                        'If PrntCliCurr = 1 Then
    '                        '    receipt = {receiptmaster(i)("receipt_acc_code").ToString() + Environment.NewLine + Environment.NewLine + costcenter_code, receiptmaster(i)("des").ToString() + Environment.NewLine + Environment.NewLine + acctname + Environment.NewLine + Environment.NewLine + receiptmaster(i)("narration").ToString(), IIf(Decimal.Parse(receiptmaster(i)("receipt_debit")) = 0.0, "-", Decimal.Parse(receiptmaster(i)("receipt_debit")).ToString(decno)), IIf(Decimal.Parse(receiptmaster(i)("receipt_credit")) = 0.0, "-", Decimal.Parse(receiptmaster(i)("receipt_credit")).ToString(decno))}
    '                        '    rdebit = rdebit + Decimal.Parse(receiptmaster(i)("receipt_debit"))
    '                        '    rcredit = rcredit + Decimal.Parse(receiptmaster(i)("receipt_credit"))

    '                        'Else
    '                        Dim acctNo As String = ""
    '                        Dim acctDesc As String = ""
    '                        Dim narration As String = ""
    '                        Dim amt As Decimal


    '                        'If (receiptmaster(i)("receipt_acc_type") = "G") Then
    '                        '    acctNo = receiptmaster(i)("receipt_acc_code").ToString()
    '                        '    acctDesc = receiptmaster(i)("des").ToString()
    '                        '    narration = receiptmaster(i)("narration").ToString()
    '                        'Else
    '                        '    acctNo = receiptmaster(i)("receipt_acc_code").ToString() + Environment.NewLine + Environment.NewLine + costcenter_code
    '                        '    acctDesc = receiptmaster(i)("des").ToString() + Environment.NewLine + Environment.NewLine + acctname
    '                        '    narration = receiptmaster(i)("narration").ToString()
    '                        'End If

    '                        acctNo = receiptmaster(i)("receipt_acc_code").ToString()
    '                        acctDesc = receiptmaster(i)("des").ToString()
    '                        narration = receiptmaster(i)("narration").ToString()

    '                        amt = Decimal.Parse(receiptmaster(i)("rdcredit")) - Decimal.Parse(receiptmaster(i)("rddebit"))
    '                        If PrntCliCurr = 1 Then
    '                            amt = Math.Round(amt / convRate, Convert.ToInt32(decnum))
    '                        End If
    '                        'receipt = {acctNo, acctDesc, IIf(Decimal.Parse(receiptmaster(i)("rddebit")) = 0.0, "-", Decimal.Parse(receiptmaster(i)("rddebit")).ToString(decno)), IIf(Decimal.Parse(receiptmaster(i)("rdcredit")) = 0.0, "-", Decimal.Parse(receiptmaster(i)("rdcredit")).ToString(decno))}
    '                        receipt = {count.ToString(), acctNo, acctDesc, narration, amt.ToString(decno)}

    '                        sumamt = sumamt + amt


    '                        ' End If
    '                        For j = 0 To receipt.Length - 1
    '                            phrase = New Phrase()
    '                            phrase.Add(New Chunk(receipt(j), normalfont))

    '                            If j = 4 Then
    '                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
    '                                cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
    '                            Else
    '                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                            End If
    '                            cell.PaddingBottom = 3.0F

    '                            tbl3.AddCell(cell)
    '                        Next
    '                    End If
    '                Next

    '                Dim amtinword As String
    '                Dim intergerpart As Integer
    '                Dim fractionpart As Decimal
    '                Dim strArr() As String
    '                'intergerpart = Int(Math.Abs(sumamt))
    '                'fractionpart = Math.Abs(sumamt) - intergerpart
    '                '  strArr = sumamt.ToString().Split(".")

    '                'fractionpart = Decimal.Parse(strArr(1))
    '                'amtinword = AmountInWords(intergerpart) & " " & AmountInWords(fractionpart)
    '                Dim Receiptamt As Decimal = Math.Abs(sumamt)
    '                Dim decWord As Decimal = Math.Truncate(Decimal.Parse(Receiptamt)).ToString()
    '                decimalInword = ""
    '                decimalInword = AmountInWords(decWord)
    '                fraction = 0
    '                fraction = Math.Round(Decimal.Parse(Receiptamt), Convert.ToInt32(decnum), MidpointRounding.AwayFromZero) Mod 1
    '                If fraction > 0 Then
    '                    Dim arrFraction As String() = fraction.ToString.Split(".")
    '                    If arrFraction.Length = 2 Then
    '                        fraction = arrFraction(1)
    '                    Else
    '                        fraction = 0
    '                    End If
    '                End If
    '                fractionword = ""
    '                If fraction > 0 Then
    '                    Dim fractionStr As String = fraction.ToString()
    '                    While fractionStr.Length < decnum
    '                        fractionStr = fractionStr + "0"
    '                    End While
    '                    Dim currcoin As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select currcoin from currmast where currcode= '" & voccurr.Trim & "'"), String)
    '                    fractionword = "And " & currcoin & " " & AmountInWords(fractionStr) + " " + "Only"
    '                Else
    '                    decimalInword = decimalInword + "  " + "Only"
    '                    fractionword = ""
    '                End If

    '                sumtotal = {"The sum of" & " " & voccurr & " " & decimalInword & " " & fractionword, sumamt.ToString(decno)}

    '                For i = 0 To 1
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk(sumtotal(i), normalfontbold))
    '                    If i = 0 Then
    '                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                        cell.Colspan = 4
    '                    Else
    '                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
    '                        cell.Colspan = 1
    '                    End If

    '                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT

    '                    cell.PaddingBottom = 5.0F

    '                    tbl3.AddCell(cell)
    '                Next

    '                phrase = New Phrase()

    '                'Modified by Priyanka 23/12/2019
    '                'If lsCtry.Trim.ToUpper = "OM" Then
    '                '    phrase.Add(New Chunk("Director of Finance:" & Space(15) & Space(15) & Space(10), normalfontbold))
    '                '    phrase.Add(New Chunk("General Manager:" & Space(15) & Space(5), normalfontbold))
    '                'Else
    '                '    phrase.Add(New Chunk("Prepared By:" & Space(10) & Space(10) & Space(10), normalfontbold))
    '                '    phrase.Add(New Chunk("Checked By:" & Space(10) & Space(10) & Space(10), normalfontbold))
    '                'End If

    '                phrase.Add(New Chunk("Cashier" & Space(60) & Space(60), footerdfont))
    '                phrase.Add(New Chunk("Accountant", footerdfont))
    '                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                cell.PaddingBottom = 3.0F
    '                cell.PaddingTop = 30.0F
    '                cell.Colspan = 5
    '                tbl3.AddCell(cell)
    '                tbl3.SpacingBefore = 0
    '                tbl3.Complete = True
    '                'tablecommon1.AddCell(tbl3)
    '                'tablecommon1.SpacingBefore = 0
    '                'tablecommon1.Complete = True
    '                'document.Add(tablecommon1)
    '                document.Add(tbl3)
    '            End If
    '            document.AddTitle(rptreportname)
    '            document.Close()
    '            If printMode = "download" Then
    '                Dim pagingFont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)
    '                Dim reader As New PdfReader(memoryStream.ToArray())
    '                Using mStream As New MemoryStream()
    '                    Using stamper As New PdfStamper(reader, mStream)
    '                        Dim pages As Integer = reader.NumberOfPages
    '                        For i As Integer = 1 To pages
    '                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), documentWidth, 10.0F, 0)
    '                        Next
    '                    End Using
    '                    bytes = mStream.ToArray()
    '                End Using
    '            End If
    '        End Using

    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Sub

   
    Public Sub ExcelReportMonthWise(ByRef bytes() As Byte)

        Dim arrHeaders() As String
        Dim rowCount As Integer = 5
        Dim imagePath, DecimalPoint As String
        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add("GlTrailBal")

        If decno = "N1" Then
            DecimalPoint = "##,##,##,##0.0"
        ElseIf decno = "N2" Then
            DecimalPoint = "##,##,##,##0.00"
        ElseIf decno = "N3" Then
            DecimalPoint = "##,##,##,##0.000"
        ElseIf decno = "N4" Then
            DecimalPoint = "##,##,##,##0.0000"
        Else
            DecimalPoint = "##,##,##,##0.00"

        End If
        ws.Column("A").Width = 50
        ws.Columns("B:E").Width = 20
        'Comapny Name Heading
        ws.Cell("A2").Value = rptcompanyname
        Dim company = ws.Range("A2:E2").Merge()
        company.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromHtml("#0048C0"))
        company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
        company.Style.Font.FontSize = 15
        company.Style.Font.FontColor = XLColor.White
        company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center


        'Report Name Heading
        ws.Cell("A3").Value = rptreportname
        Dim report = ws.Range("A3:E3").Merge()
        report.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromHtml("#0080C0"))
        report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
        report.Style.Font.FontSize = 14
        report.Style.Font.FontColor = XLColor.White
        report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
        'report.Style.Alignment.Vertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)



        ''Report Filter
        'Dim filter = ws.Range("B6:I6")
        'filter.Style.Font.SetBold().Font.FontSize = 14
        'filter.Style.Font.FontColor = XLColor.Black
        'filter.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
        'filter.Cell(1, 1).Value = reportfilter


        arrHeaders = {"", MonthName(month1), MonthName(month2), MonthName(month3), month4}
        ws.Range(5, 1, 5, 5).Style.Font.SetBold.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetTopBorder(XLBorderStyleValues.Thin).Font.FontSize = 12
        ws.Range(5, 1, 5, 5).Style.Alignment.WrapText = True
        For i = 0 To arrHeaders.Length - 1
            ws.Cell(5, i + 1).Value = arrHeaders(i)
        Next

        If View_pf.Rows.Count > 0 Then
            Dim acctLevel, gacctLevel0, gacctLevel1, gacctLevel2, gacctLevel3, gacctLevel4, gpbyName As New DataTable
            '  Dim acctgroup1 As New DataTable
            Dim amount, amount1, amount2, amount3, amount20, amount21, amount22, amount23 As Decimal
            Dim acct1name, acct2name As String


            Dim dataView As New DataView(View_pf)
            dataView.Sort = "AccName ASC"
            gpbyName = dataView.ToTable()

            'Group by Level0 and Head
            Dim groups = From custledger In gpbyName.AsEnumerable() Group custledger By g = New With {Key .level0 = custledger.Field(Of Integer)("level0"), Key .div_code = custledger.Field(Of String)("div_code"), Key .Head = custledger.Field(Of Integer)("Head")} Into Group Order By g.Head

            For Each groupby In groups

                acctLevel = groupby.Group.CopyToDataTable
                'Group by GroupHead
                Dim gpbygroupHeader = From custledger In acctLevel.AsEnumerable() Group custledger By g = New With {Key .groupHeader = custledger.Field(Of Integer)("GroupHeader"), Key .div_code = custledger.Field(Of String)("div_code")} Into Group Order By g.div_code


                For Each gpbyHeader In gpbygroupHeader

                    'Level1
                    gacctLevel0 = gpbyHeader.Group.CopyToDataTable

                    ' Get the Sum of Income
                    Dim Getincome = gacctLevel0.Compute("SUM(Amount)", "GroupHeader='1' AND State>=1")
                    If IsDBNull(Getincome) Then
                        income = income + 0.0
                    Else
                        income = income + Convert.ToDecimal(Getincome)

                    End If

                    Dim Getincome1 = gacctLevel0.Compute("SUM(Amount1)", "GroupHeader='1' AND State>=1")
                    If IsDBNull(Getincome1) Then
                        income1 = income1 + 0.0
                    Else
                        income1 = income1 + Convert.ToDecimal(Getincome1)

                    End If

                    Dim Getincome2 = gacctLevel0.Compute("SUM(Amount2)", "GroupHeader='1' AND State>=1")
                    If IsDBNull(Getincome2) Then
                        income2 = income2 + 0.0
                    Else
                        income2 = income2 + Convert.ToDecimal(Getincome2)

                    End If

                    Dim Getincome3 = gacctLevel0.Compute("SUM(Amount3)", "GroupHeader='1' AND State>=1")
                    If IsDBNull(Getincome3) Then
                        income3 = income3 + 0.0
                    Else
                        income3 = income3 + Convert.ToDecimal(Getincome3)
                    End If


                    'Get The Sum of Expanses
                    Dim Getexpanse = gacctLevel0.Compute("SUM(Amount)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getexpanse) Then
                        expanse = expanse + 0.0
                    Else
                        expanse = expanse + Convert.ToDecimal(Getexpanse)

                    End If

                    Dim Getexpanse1 = gacctLevel0.Compute("SUM(Amount1)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getexpanse1) Then
                        expanse1 = expanse1 + 0.0
                    Else
                        expanse1 = expanse1 + Convert.ToDecimal(Getexpanse1)

                    End If

                    Dim Getexpanse2 = gacctLevel0.Compute("SUM(Amount2)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getexpanse2) Then
                        expanse2 = expanse2 + 0.0
                    Else
                        expanse2 = expanse2 + Convert.ToDecimal(Getexpanse2)

                    End If

                    Dim Getexpanse3 = gacctLevel0.Compute("SUM(Amount3)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getexpanse3) Then
                        expanse3 = expanse3 + 0.0
                    Else
                        expanse3 = expanse3 + Convert.ToDecimal(Getexpanse3)
                    End If


                    'Get the Sum of CostOfSales


                    Dim Getcostofsales = gacctLevel0.Compute("SUM(Amount)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getcostofsales) Then
                        costofsales = costofsales + 0.0
                    Else
                        costofsales = costofsales + Convert.ToDecimal(Getcostofsales)

                    End If

                    Dim Getcostofsales1 = gacctLevel0.Compute("SUM(Amount1)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getcostofsales1) Then
                        costofsales1 = costofsales1 + 0.0
                    Else
                        costofsales1 = costofsales1 + Convert.ToDecimal(Getcostofsales1)

                    End If

                    Dim Getcostofsales2 = gacctLevel0.Compute("SUM(Amount2)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getcostofsales2) Then
                        costofsales2 = costofsales2 + 0.0
                    Else
                        costofsales2 = costofsales2 + Convert.ToDecimal(Getcostofsales2)

                    End If

                    Dim Getcostofsales3 = gacctLevel0.Compute("SUM(Amount3)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getcostofsales3) Then
                        costofsales3 = costofsales3 + 0.0
                    Else
                        costofsales3 = costofsales3 + Convert.ToDecimal(Getcostofsales3)

                    End If

                    'level 1 Group By
                    Dim gpbyLevel1 = From custledger In gacctLevel0.AsEnumerable() Group custledger By g = New With {Key .level1 = custledger.Field(Of Integer)("level1"), Key .div_code = custledger.Field(Of String)("div_code")} Into Group Order By g.div_code

                    For Each gpby1 In gpbyLevel1
                        Dim acctgroup1 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname1 = a.Field(Of String)("acctname"), .div_code1 = a.Field(Of String)("div_code"), .parentid1 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid1 = gpby1.g.level1 And a.div_code1 = gpby1.g.div_code).OrderBy(Function(o) o.parentid1)
                        gacctLevel1 = gpby1.Group.CopyToDataTable

                        'Dim dataView1 As New DataView(gacctLevel1)
                        'dataView1.Sort = "AccName ASC"
                        'gacctLevel1 = dataView.ToTable()
                        amount = 0
                        amount1 = 0
                        amount2 = 0
                        amount3 = 0
                        acct1name = ""
                        If acctgroup1.Count > 0 Then
                            For Each acct1row In acctgroup1
                                acct1name = acct1row.acctname1
                            Next
                        End If

                        For Each row In gpby1.Group
                            If row("state") >= 1 Then
                                If Not (TypeOf row("amount") Is DBNull) Then
                                    amount = amount + Decimal.Parse(row("amount"))
                                Else
                                    amount = amount + 0.0
                                End If

                                If Not (TypeOf row("amount1") Is DBNull) Then
                                    amount1 = amount1 + Decimal.Parse(row("amount1"))
                                Else
                                    amount1 = amount1 + 0.0
                                End If

                                If Not (TypeOf row("amount2") Is DBNull) Then
                                    amount2 = amount2 + Decimal.Parse(row("amount2"))
                                Else
                                    amount2 = amount2 + 0.0
                                End If
                                If Not (TypeOf row("amount3") Is DBNull) Then
                                    amount3 = amount3 + Decimal.Parse(row("amount3"))
                                Else
                                    amount3 = amount3 + 0.0
                                End If

                            End If
                        Next

                        arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
                        rowCount = rowCount + 1
                        ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 12
                        ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                        ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Alignment.WrapText = True
                        For i = 0 To arrHeaders.Length - 1
                            'Phrase = New Phrase()
                            'Phrase.Add(New Chunk(arrHeaders(i), Level1font))
                            If i = 0 Then
                                ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                                ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            Else
                                ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                                ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).NumberFormat.Format = DecimalPoint
                            End If
                        Next
                        ' End If
                        ' Level(2)
                        Dim acctLevel2 = From row1 In gacctLevel1.AsEnumerable() Group row1 By g1 = New With {Key .level2 = row1.Field(Of Integer)("level2"), Key .divcode2 = row1.Field(Of String)("div_code")} Into Group Order By g1.divcode2

                        For Each gpbylevel2 In acctLevel2
                            ' Dim acctgroup2() As DataRow = acctgroupTable.Select("parentid='" & gpbylevel2.g1.level2 & "'")
                            Dim acctgroup2 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname2 = a.Field(Of String)("acctname"), .div_code2 = a.Field(Of String)("div_code"), .parentid2 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid2 = gpbylevel2.g1.level2 And a.div_code2 = gpbylevel2.g1.divcode2)
                            gacctLevel2 = gpbylevel2.Group.CopyToDataTable

                            amount = 0
                            amount1 = 0
                            amount2 = 0
                            amount3 = 0
                            acct1name = ""
                            acct2name = ""
                            If acctgroup2.Count > 0 Then
                                For Each acct2row In acctgroup2
                                    acct1name = acct2row.acctname2
                                    acct2name = acct2row.acctname2
                                Next
                            End If
                            For Each row In gpbylevel2.Group
                                If row("state") >= 1 Then
                                    If Not (TypeOf row("amount") Is DBNull) Then
                                        amount = amount + Decimal.Parse(row("amount"))
                                    Else
                                        amount = amount + 0.0
                                    End If

                                    If Not (TypeOf row("amount1") Is DBNull) Then
                                        amount1 = amount1 + Decimal.Parse(row("amount1"))
                                    Else
                                        amount1 = amount1 + 0.0
                                    End If

                                    If Not (TypeOf row("amount2") Is DBNull) Then
                                        amount2 = amount2 + Decimal.Parse(row("amount2"))
                                    Else
                                        amount2 = amount2 + 0.0
                                    End If
                                    If Not (TypeOf row("amount3") Is DBNull) Then
                                        amount3 = amount3 + Decimal.Parse(row("amount3"))
                                    Else
                                        amount3 = amount3 + 0.0
                                    End If

                                End If

                            Next

                            arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
                            rowCount = rowCount + 1
                            ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 11
                            ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                            ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Alignment.WrapText = True
                            For i = 0 To arrHeaders.Length - 1
                                'Phrase = New Phrase()
                                'Phrase.Add(New Chunk(arrHeaders(i), Level1font))
                                If i = 0 Then
                                    ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                                    ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                Else
                                    ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                                    ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).NumberFormat.Format = DecimalPoint
                                End If
                            Next

                            amount20 = amount
                            amount21 = amount1
                            amount22 = amount2
                            amount23 = amount3

                            ' Level3
                            Dim acctLevel3 = From row1 In gacctLevel2.AsEnumerable() Group row1 By g1 = New With {Key .level3 = row1.Field(Of Integer)("level3"), Key .divcode3 = row1.Field(Of String)("div_code")} Into Group Order By g1.divcode3
                            For Each gpbylevel3 In acctLevel3
                                Dim acctgroup3 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname3 = a.Field(Of String)("acctname"), .div_code3 = a.Field(Of String)("div_code"), .parentid3 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid3 = gpbylevel3.g1.level3 And a.div_code3 = gpbylevel3.g1.divcode3).OrderBy(Function(o) o.acctname3)
                                gacctLevel3 = gpbylevel3.Group.CopyToDataTable

                                'Dim dataView3 As New DataView(gacctLevel3)
                                'dataView3.Sort = "AccName ASC"
                                'gacctLevel3 = dataView.ToTable()
                                amount = 0
                                amount1 = 0
                                amount2 = 0
                                amount3 = 0
                                acct1name = ""
                                If acctgroup3.Count > 0 Then

                                    For Each acct3row In acctgroup3
                                        acct1name = acct3row.acctname3
                                    Next

                                End If
                                For Each row In gpbylevel3.Group
                                    If row("state") >= 3 Then
                                        If Not (TypeOf row("amount") Is DBNull) Then
                                            amount = amount + Decimal.Parse(row("amount"))
                                        Else
                                            amount = amount + 0.0
                                        End If

                                        If Not (TypeOf row("amount1") Is DBNull) Then
                                            amount1 = amount1 + Decimal.Parse(row("amount1"))
                                        Else
                                            amount1 = amount1 + 0.0
                                        End If

                                        If Not (TypeOf row("amount2") Is DBNull) Then
                                            amount2 = amount2 + Decimal.Parse(row("amount2"))
                                        Else
                                            amount2 = amount2 + 0.0
                                        End If
                                        If Not (TypeOf row("amount3") Is DBNull) Then
                                            amount3 = amount3 + Decimal.Parse(row("amount3"))
                                        Else
                                            amount3 = amount3 + 0.0
                                        End If

                                    End If

                                Next

                                arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
                                rowCount = rowCount + 1
                                ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
                                ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                                ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Alignment.WrapText = True
                                For i = 0 To arrHeaders.Length - 1
                                    'Phrase = New Phrase()
                                    'Phrase.Add(New Chunk(arrHeaders(i), Level1font))
                                    If i = 0 Then
                                        ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                                        ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                    Else
                                        ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                                        ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).NumberFormat.Format = DecimalPoint
                                    End If
                                Next


                                'level4
                                Dim acctLevel4 = From row1 In gacctLevel3.AsEnumerable() Group row1 By g1 = New With {Key .level4 = row1.Field(Of Integer)("level4"), Key .divcode4 = row1.Field(Of String)("div_code")} Into Group Order By g1.divcode4

                                For Each gpbylevel4 In acctLevel4
                                    Dim acctgroup4 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname4 = a.Field(Of String)("acctname"), .div_code4 = a.Field(Of String)("div_code"), .parentid4 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid4 = gpbylevel4.g1.level4 And a.div_code4 = gpbylevel4.g1.divcode4).OrderBy(Function(o) o.acctname4)
                                    gacctLevel4 = gpbylevel4.Group.CopyToDataTable

                                    'Dim dataView4 As New DataView(gacctLevel4)
                                    'dataView4.Sort = "AccName ASC"
                                    'gacctLevel4 = dataView.ToTable()
                                    amount = 0
                                    amount1 = 0
                                    amount2 = 0
                                    amount3 = 0
                                    acct1name = ""
                                    If acctgroup4.Count > 0 Then

                                        For Each acct4row In acctgroup4
                                            acct1name = acct4row.acctname4
                                        Next
                                    End If
                                    For Each row In gpbylevel4.Group
                                        If row("state") >= 4 Then
                                            If Not (TypeOf row("amount") Is DBNull) Then
                                                amount = amount + Decimal.Parse(row("amount"))
                                            Else
                                                amount = amount + 0.0
                                            End If

                                            If Not (TypeOf row("amount1") Is DBNull) Then
                                                amount1 = amount1 + Decimal.Parse(row("amount1"))
                                            Else
                                                amount1 = amount1 + 0.0
                                            End If

                                            If Not (TypeOf row("amount2") Is DBNull) Then
                                                amount2 = amount2 + Decimal.Parse(row("amount2"))
                                            Else
                                                amount2 = amount2 + 0.0
                                            End If
                                            If Not (TypeOf row("amount3") Is DBNull) Then
                                                amount3 = amount3 + Decimal.Parse(row("amount3"))
                                            Else
                                                amount3 = amount3 + 0.0
                                            End If

                                        End If

                                    Next

                                    arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
                                    rowCount = rowCount + 1
                                    ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
                                    ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                                    ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Alignment.WrapText = True
                                    For i = 0 To arrHeaders.Length - 1
                                        'Phrase = New Phrase()
                                        'Phrase.Add(New Chunk(arrHeaders(i), Level1font))
                                        If i = 0 Then
                                            ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                                            ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                        Else
                                            ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                                            ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).NumberFormat.Format = DecimalPoint
                                        End If
                                    Next
                                    'level5

                                    Dim acctLevel5 = From row1 In gacctLevel4.AsEnumerable() Group row1 By g1 = New With {Key .level5 = row1.Field(Of Integer)("level5"), Key .divcode5 = row1.Field(Of String)("div_code")} Into Group Order By g1.divcode5

                                    For Each gpbylevel5 In acctLevel5
                                        Dim acctgroup5 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname5 = a.Field(Of String)("acctname"), .div_code5 = a.Field(Of String)("div_code"), .parentid5 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid5 = gpbylevel5.g1.level5 And a.div_code5 = gpbylevel5.g1.divcode5).OrderBy(Function(o) o.acctname5)
                                        'gacctLevel5 = gpbylevel5.Group.CopyToDataTable
                                        amount = 0
                                        amount1 = 0
                                        amount2 = 0
                                        amount3 = 0
                                        acct1name = ""
                                        If acctgroup5.Count > 0 Then

                                            For Each acct5row In acctgroup5
                                                acct1name = acct5row.acctname5
                                            Next
                                        End If
                                        For Each row In gpbylevel5.Group
                                            If row("state") >= 5 Then
                                                If Not (TypeOf row("amount") Is DBNull) Then
                                                    amount = amount + Decimal.Parse(row("amount"))
                                                Else
                                                    amount = amount + 0.0
                                                End If

                                                If Not (TypeOf row("amount1") Is DBNull) Then
                                                    amount1 = amount1 + Decimal.Parse(row("amount1"))
                                                Else
                                                    amount1 = amount1 + 0.0
                                                End If

                                                If Not (TypeOf row("amount2") Is DBNull) Then
                                                    amount2 = amount2 + Decimal.Parse(row("amount2"))
                                                Else
                                                    amount2 = amount2 + 0.0
                                                End If
                                                If Not (TypeOf row("amount3") Is DBNull) Then
                                                    amount3 = amount3 + Decimal.Parse(row("amount3"))
                                                Else
                                                    amount3 = amount3 + 0.0
                                                End If

                                            End If

                                        Next

                                        arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
                                        rowCount = rowCount + 1
                                        ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 8
                                        ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                                        ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Alignment.WrapText = True
                                        For i = 0 To arrHeaders.Length - 1
                                            'Phrase = New Phrase()
                                            'Phrase.Add(New Chunk(arrHeaders(i), Level1font))
                                            If i = 0 Then
                                                ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                                                ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                            Else
                                                ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                                                ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).NumberFormat.Format = DecimalPoint
                                            End If
                                        Next
                                    Next
                                    'End level 5
                                Next
                                'End Level 4
                            Next
                            'end level 3

                            If String.Equals(acct2name, "Income") Then
                                acct2name = "Total Revenue"
                            ElseIf String.Equals(acct2name, "Cost Of Sales") Then
                                acct2name = "Total Cost Of Sales"
                            ElseIf String.Equals(acct2name, "Expenses") Then
                                acct2name = "Other Direct Costs"
                            End If


                            arrHeaders = {acct2name, amount20.ToString(decno), amount21.ToString(decno), amount22.ToString(decno), amount23.ToString(decno)}
                            rowCount = rowCount + 1
                            ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 12
                            ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                            ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Alignment.WrapText = True
                            For i = 0 To arrHeaders.Length - 1
                                'Phrase = New Phrase()
                                'Phrase.Add(New Chunk(arrHeaders(i), Level1font))
                                If i = 0 Then
                                    ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                                    ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                Else
                                    ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                                    ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).NumberFormat.Format = DecimalPoint
                                End If
                            Next
                        Next

                        'End level 2
                    Next
                    'End level 1
                Next

            Next


            arrHeaders = {"NET PROFIT", (income - expanse - costofsales).ToString(decno), (income1 - expanse1 - costofsales1).ToString(decno), (income2 - expanse2 - costofsales2).ToString(decno), (income3 - expanse3 - costofsales3).ToString(decno)}
            rowCount = rowCount + 1
            ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
            ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thick).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Alignment.WrapText = True
            For i = 0 To arrHeaders.Length - 1
                'Phrase = New Phrase()
                'Phrase.Add(New Chunk(arrHeaders(i), Level1font))
                If i = 0 Then
                    ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                    ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                Else
                    ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                    ws.Cell(rowCount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).NumberFormat.Format = DecimalPoint
                End If
            Next

        End If

        ws.Cell((rowCount + 2), 1).Value = "Printed Date:" & Now.ToString("dd/MM/yyyy")
        ws.Range((rowCount + 2), 1, (rowCount + 2), 3).Merge()
        Using wStream As New MemoryStream()
            wb.SaveAs(wStream)
            bytes = wStream.ToArray()
        End Using
    End Sub


End Class

#Region "Class SupplierAccount"
Public Class SupplierAccount
    Private _acccode As String
    Private _accname As String
    Private _accno As String
    Private _bankname As String
    Private _branchname As String
    Private _swiftcode As String
    Private _iban As String
    Public Property AccCode() As String
        Get
            Return _acccode
        End Get
        Set(ByVal value As String)
            _acccode = value
        End Set
    End Property
    Public Property AccName() As String
        Get
            Return _accname
        End Get
        Set(ByVal value As String)
            _accname = value
        End Set
    End Property

    Public Property AccNo() As String
        Get
            Return _accno
        End Get
        Set(ByVal value As String)
            _accno = value
        End Set
    End Property


    Public Property BankName() As String
        Get
            Return _bankname
        End Get
        Set(ByVal value As String)
            _bankname = value
        End Set
    End Property

    Public Property BranchName() As String
        Get
            Return _branchname
        End Get
        Set(ByVal value As String)
            _branchname = value
        End Set
    End Property

    Public Property SwiftCode() As String
        Get
            Return _swiftcode
        End Get
        Set(ByVal value As String)
            _swiftcode = value
        End Set
    End Property

    Public Property Iban() As String
        Get
            Return _iban
        End Get
        Set(ByVal value As String)
            _iban = value
        End Set
    End Property
End Class
#End Region

