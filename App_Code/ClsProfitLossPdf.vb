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

Public Class ClsProfitLossPdf
    Inherits System.Web.UI.Page
    Dim objutils As New clsUtils
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

    Dim rptcompanyname, rptreportname, decno, fromname, sqlquery, rptfilter, currname, month13, addrLine, addrLine1, addrLine2, addrLine3, addrLine4, addrLine5 As String
    Dim documentWidth As Single = 550.0F
    Dim CompanybgColor As BaseColor = New BaseColor(0, 72, 192)
    Dim ReportNamebgColor As BaseColor = New BaseColor(0, 128, 192)
    Dim CompanybgColor1 As BaseColor = New BaseColor(225, 225, 225)
    Dim ReportNamebgColor1 As BaseColor = New BaseColor(225, 225, 225)
    Dim View_pf As New DataTable
    Dim acctgroupTable As New DataTable
    Dim view_actgroup As New DataTable
    Dim month1, month2, month3, month4, month5, month6, month7, month8, month9, month10, month11, month12 As Integer
    Dim costofsales, costofsales1, costofsales2, costofsales3, costofsales4, costofsales5, costofsales6, costofsales7, costofsales8, costofsales9, costofsales10, costofsales11, costofsales12, income, income1, income2, income3, income4, income5, income6, income7, income8, income9, income10, income11, income12, expanse, expanse1, expanse2, expanse3, expanse4, expanse5, expanse6, expanse7, expanse8, expanse9, expanse10, expanse11, expanse12 As Decimal
    'Ram 22082022
    Dim currency As String, decimalPoint As String = ""
    Dim decpt As Integer
    'Ram 22082022
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

    '"ColumbusRPTS"."dbo"."sp_rep_profitloss_new_5";1 '2017/01/01', '2017/03/23', '02', 0

#Region "Private Shared Function ImageCell(path As String, scale As Single, align As Integer) As PdfPCell"
    Private Shared Function ImageCell(ByVal path As String, ByVal scale As Single, ByVal align As Integer) As PdfPCell
        Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path))
        image.ScalePercent(scale)
        Dim cell As New PdfPCell(image)
        cell.BorderColor = basecolor.WHITE
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 0.0F
        cell.PaddingTop = 0.0F
        Return cell
    End Function
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
            mySqlCmd.CommandTimeout = 0 'Tanvir 04052023
            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")

            mySqlCmd.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 20)).Value = divcode
            mySqlCmd.Parameters.Add(New SqlParameter("@closing", SqlDbType.Int)).Value = closing

            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim custdetailsdt As New DataTable
            custdetailsdt = ds.Tables(0)

            If reportsType = "excel" Then
                GenerateReportExcel(divcode, custdetailsdt, bytes)
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

    '#Region "GenerateReport"
    '    Public Sub GenerateReport(ByVal reportsType As String, ByVal fromdate As String, ByVal todate As String, ByVal divcode As String, ByVal rpttype As String, ByVal type As String, ByVal closing As String, ByVal strrpttype1 As String, ByRef bytes() As Byte, ByVal printMode As String, Optional ByVal fileName As String = "")

    '        Try
    '            If divcode <> "" Then
    '                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
    '            Else
    '                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
    '            End If

    '            rptreportname = " Report- Income Statement "


    '            rptreportname = rptreportname & Environment.NewLine & vbLf & "From" & Space(2) & fromdate & "  To" & Space(2) & todate
    '            rptfilter = "From" & Space(2) & fromdate & "  To" & Space(2) & todate
    '            decno = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
    '            decno = "N" & decno


    '            Dim sqlConn As New SqlConnection
    '            Dim mySqlCmd As New SqlCommand
    '            Dim myDataAdapter As New SqlDataAdapter
    '            Dim ds As New DataSet
    '            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
    '            mySqlCmd = New SqlCommand("sp_rep_profitloss_new_5", sqlConn)
    '            mySqlCmd.CommandType = CommandType.StoredProcedure
    '            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")
    '            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")

    '            mySqlCmd.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 20)).Value = divcode
    '            mySqlCmd.Parameters.Add(New SqlParameter("@closing", SqlDbType.Int)).Value = closing

    '            myDataAdapter.SelectCommand = mySqlCmd
    '            myDataAdapter.Fill(ds)
    '            Dim custdetailsdt As New DataTable
    '            custdetailsdt = ds.Tables(0)

    '            If reportsType = "excel" Then
    '                GenerateReportExcel(divcode, custdetailsdt, bytes)
    '            Else
    '                Dim line As Paragraph
    '                line = New Paragraph(New Chunk(New iTextSharp.text.pdf.draw.LineSeparator(1.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)))
    '                Dim document As New Document(PageSize.A4, -10.0F, 10.0F, 30.0F, 35.0F)
    '                Using memoryStream As New System.IO.MemoryStream()
    '                    Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)

    '                    Dim phrase As Phrase = Nothing
    '                    Dim cell As PdfPCell = Nothing
    '                    'Dim titletable As PdfPTable = Nothing
    '                    Dim debit, credit, totaldedit, totalcredit As Decimal

    '                    Dim arrHeaders() As String

    '                    Dim logo As PdfPTable = New PdfPTable(1)
    '                    logo.TotalWidth = documentWidth
    '                    logo.LockedWidth = True
    '                    logo.SetWidths(New Single() {1.0F})
    '                    logo.Complete = False
    '                    logo.SplitRows = False
    '                    'company name
    '                    If divcode = "01" Then
    '                        cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
    '                    Else
    '                        cell = ImageCell("~/Images/Logo1.png", 80.0F, PdfPCell.ALIGN_LEFT)
    '                    End If
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
    '                    cell.Colspan = 2
    '                    cell.SetLeading(12, 0)
    '                    cell.PaddingBottom = 4
    '                    logo.AddCell(cell)

    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk(rptcompanyname, Companyname))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER
    '                    logo.AddCell(cell)

    '                    Dim tblTitle As PdfPTable = New PdfPTable(1)
    '                    tblTitle.SetWidths(New Single() {1.0F})
    '                    tblTitle.TotalWidth = documentWidth
    '                    tblTitle.LockedWidth = True
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk(rptreportname, ReportNamefont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                    cell.PaddingBottom = 3.0F
    '                    tblTitle.AddCell(cell)
    '                    tblTitle.SpacingBefore = 7

    '                    Dim titletable = New PdfPTable(1)
    '                    titletable.TotalWidth = documentWidth
    '                    titletable.LockedWidth = True
    '                    titletable.SetWidths(New Single() {1.0F})

    '                    titletable.SplitRows = False
    '                    titletable.WidthPercentage = 100
    '                    If divcode = "01" Then
    '                        cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
    '                    Else
    '                        cell = ImageCell("~/Images/Logo1.png", 80.0F, PdfPCell.ALIGN_LEFT)
    '                    End If
    '                    titletable.AddCell(cell)
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk(rptcompanyname, Companyname))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                    cell.SetLeading(12, 0)
    '                    cell.PaddingBottom = 4
    '                    titletable.AddCell(cell)
    '                    titletable.Complete = True


    '                    Dim Reporttitle = New PdfPTable(1)
    '                    Reporttitle.TotalWidth = documentWidth
    '                    Reporttitle.LockedWidth = True
    '                    Reporttitle.SetWidths(New Single() {1.0F})
    '                    Reporttitle.Complete = False
    '                    '  Reporttitle.SplitRows = False
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk(rptreportname, ReportNamefont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, True)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                    cell.PaddingBottom = 4
    '                    cell.SetLeading(12, 0)
    '                    'cell.BackgroundColor = ReportNamebgColor1
    '                    Reporttitle.SpacingBefore = 5
    '                    Reporttitle.SpacingAfter = 0
    '                    Reporttitle.AddCell(cell)
    '                    Reporttitle.Complete = True

    '                    Dim FooterTable = New PdfPTable(1)

    '                    FooterTable.TotalWidth = documentWidth
    '                    FooterTable.LockedWidth = True
    '                    FooterTable.SetWidths(New Single() {1.0F})
    '                    FooterTable.Complete = False
    '                    FooterTable.SplitRows = False

    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk(addrLine3 + addrLine5, normalfont))
    '                    cell = New PdfPCell(phrase)
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                    cell.Colspan = 2
    '                    cell.SetLeading(10, 0)
    '                    cell.PaddingBottom = 1
    '                    FooterTable.AddCell(cell)

    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk(addrLine1, normalfont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                    cell.Colspan = 2
    '                    cell.SetLeading(10, 0)
    '                    cell.PaddingBottom = 1
    '                    FooterTable.AddCell(cell)

    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk(addrLine2 + "  " + addrLine4, normalfont))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                    cell.Colspan = 2
    '                    cell.SetLeading(10, 0)
    '                    cell.PaddingBottom = 1
    '                    FooterTable.AddCell(cell)
    '                    ' DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk("Printed Date: " & Date.Now.ToString("yyyy-MM-dd HH:mm:ss"), normalfont))
    '                    cell = New PdfPCell(phrase)
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP
    '                    cell.Colspan = 2
    '                    cell.SetLeading(12, 0)
    '                    cell.PaddingBottom = 3
    '                    FooterTable.AddCell(cell)
    '                    FooterTable.Complete = True


    '                    Dim MainTitle = New PdfPTable(1)
    '                    MainTitle.TotalWidth = documentWidth
    '                    MainTitle.LockedWidth = True

    '                    MainTitle.Complete = False
    '                    MainTitle.SplitRows = False
    '                    MainTitle.SetWidths(New Single() {1.0F})



    '                    Dim tableTitle = New PdfPTable(3)
    '                    tableTitle.TotalWidth = documentWidth
    '                    tableTitle.LockedWidth = True

    '                    tableTitle.Complete = False
    '                    tableTitle.SplitRows = False
    '                    tableTitle.SetWidths(New Single() {0.6F, 0.2F, 0.2F})
    '                    cell.AddElement(line)
    '                    cell.Colspan = 3
    '                    tableTitle.AddCell(cell)
    '                    'tableTitle.SplitRows = False
    '                    If custdetailsdt.Rows.Count > 0 Then
    '                        Dim currcode = custdetailsdt.AsEnumerable().Select(Function(x) New With {Key .currcode = x.Field(Of String)("currcode")}).FirstOrDefault
    '                        arrHeaders = {"", "Amount(" & currcode.currcode.ToString() & ")", "Total Amount(" & currcode.currcode.ToString() & ")"}
    '                        For i = 0 To arrHeaders.Length - 1
    '                            phrase = New Phrase()
    '                            phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
    '                            cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
    '                            cell.Colspan = 1
    '                            cell.SetLeading(12, 0)
    '                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                            cell.PaddingBottom = 4.0F
    '                            cell.PaddingTop = 5.0F
    '                            tableTitle.AddCell(cell)
    '                        Next
    '                        tableTitle.Complete = True
    '                        writer.PageEvent = New ClsHeaderFooter(logo, tblTitle, FooterTable, tableTitle)
    '                    Else
    '                        writer.PageEvent = New ClsHeaderFooter(Nothing, Nothing, FooterTable, Nothing)
    '                    End If
    '                    document.Open()


    '                    'document.Add(titletable)
    '                    'document.Add(Reporttitle)

    '                    'document.Add(tableTitle)

    '                    If custdetailsdt.Rows.Count > 0 Then
    '                        Dim tableData As PdfPTable = New PdfPTable(3)
    '                        tableData.TotalWidth = documentWidth
    '                        tableData.LockedWidth = True
    '                        tableData.SplitRows = False
    '                        '  tableData.KeepTogether = True
    '                        tableData.SpacingBefore = 5
    '                        tableData.SpacingAfter = 0
    '                        tableData.SetWidths(New Single() {0.6F, 0.2F, 0.2F})

    '                        Dim group1name, gpbyname, gpbycode, expenseTitle, groupname, totalTitle As String
    '                        Dim grouporder As Integer
    '                        Dim totalClosingBal, Totalincome, TotalExpense, TotalCost, finalTotalclosingBal As Decimal

    '                        Dim groups = From gpbyrow In custdetailsdt.AsEnumerable() Group gpbyrow By g = New With {Key .grouporder = gpbyrow.Field(Of Integer)("grouporder"), Key .group1name = gpbyrow.Field(Of String)("group1name")} Into Group Order By g.grouporder

    '                        For Each gpbyKey In groups
    '                            grouporder = gpbyKey.g.grouporder
    '                            groupname = gpbyKey.g.group1name
    '                            arrHeaders = {IIf(String.Equals(groupname, "Income"), "Revenue", groupname), "", ""}

    '                            For i = 0 To arrHeaders.Length - 1
    '                                phrase = New Phrase()
    '                                phrase.Add(New Chunk(arrHeaders(i), normalfontbold))

    '                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                cell.SetLeading(12, 0)
    '                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                If i = 0 Then
    '                                    cell.PaddingLeft = 3.0F
    '                                End If
    '                                cell.PaddingBottom = 4.0F
    '                                cell.PaddingTop = 1.0F
    '                                tableData.AddCell(cell)
    '                            Next

    '                            If grouporder = 4 Then
    '                                Dim gpby3 = gpbyKey.Group.CopyToDataTable

    '                                If grouporder = 4 Then
    '                                    gpbycode = "group4code"
    '                                    gpbyname = "group4name"
    '                                Else
    '                                    gpbycode = "group3code"
    '                                    gpbyname = "group2name"
    '                                End If

    '                                Dim groups3 = From gpbyrow In gpby3.AsEnumerable() Group gpbyrow By g = New With {Key .gpbycode = gpbyrow.Field(Of String)(gpbycode), Key .gpbyname = gpbyrow.Field(Of String)(gpbyname)} Into Group Order By g.gpbycode
    '                                For Each groupby3 In groups3

    '                                    arrHeaders = {groupby3.g.gpbyname, "", ""}
    '                                    For i = 0 To arrHeaders.Length - 1
    '                                        phrase = New Phrase()
    '                                        phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
    '                                        If i = 0 Then
    '                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                            cell.PaddingLeft = 12.0F
    '                                        Else
    '                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
    '                                        End If

    '                                        cell.SetLeading(12, 0)
    '                                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                        cell.PaddingBottom = 4.0F
    '                                        cell.PaddingTop = 1.0F
    '                                        tableData.AddCell(cell)
    '                                    Next

    '                                    For Each row In groupby3.Group
    '                                        arrHeaders = {row("acct_name"), Decimal.Parse(row("closingbalance")).ToString(decno), ""}
    '                                        For i = 0 To arrHeaders.Length - 1
    '                                            phrase = New Phrase()
    '                                            phrase.Add(New Chunk(arrHeaders(i), normalfont))
    '                                            If i = 0 Then
    '                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                                cell.PaddingLeft = 24.0F
    '                                            Else
    '                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                                            End If

    '                                            cell.SetLeading(12, 0)
    '                                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                            cell.PaddingBottom = 4.0F
    '                                            cell.PaddingTop = 1.0F
    '                                            tableData.AddCell(cell)
    '                                        Next

    '                                        If String.Equals(row("group2name"), "Expenditure") And String.Equals(row("group4name"), "Direct Costs ") Then
    '                                            expenseTitle = "Total Direct Expenses"
    '                                        Else
    '                                            If row("group4name").ToString().Contains("Expanses") Then
    '                                                expenseTitle = row("group4name").ToString()
    '                                            Else
    '                                                expenseTitle = row("group4name").ToString() & Space(2) & "Expanses"
    '                                            End If
    '                                        End If
    '                                        totalClosingBal = totalClosingBal + Decimal.Parse(row("closingbalance"))
    '                                        finalTotalclosingBal = finalTotalclosingBal + Decimal.Parse(row("closingbalance"))
    '                                    Next

    '                                    arrHeaders = {expenseTitle, "", totalClosingBal.ToString(decno)}
    '                                    totalClosingBal = 0
    '                                    For i = 0 To arrHeaders.Length - 1
    '                                        phrase = New Phrase()
    '                                        phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
    '                                        If i = 0 Then
    '                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                            cell.PaddingLeft = 3.0F
    '                                        Else
    '                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                                        End If
    '                                        cell.SetLeading(12, 0)
    '                                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                        cell.PaddingBottom = 4.0F
    '                                        cell.PaddingTop = 1.0F
    '                                        tableData.AddCell(cell)
    '                                    Next
    '                                Next

    '                            Else
    '                                For Each row In gpbyKey.Group
    '                                    arrHeaders = {row("acct_name"), Decimal.Parse(row("closingbalance")).ToString(decno), ""}

    '                                    'added mohamed and param on 17/11/2020
    '                                    If row("grouporder").ToString().Trim() = "4" Then
    '                                        TotalExpense = TotalExpense + Decimal.Parse(row("closingbalance"))
    '                                    ElseIf row("grouporder").ToString().Trim() = "2" Then
    '                                        TotalCost = TotalCost + Decimal.Parse(row("closingbalance"))
    '                                    Else
    '                                        Totalincome = Totalincome + Decimal.Parse(row("closingbalance"))
    '                                    End If
    '                                    'Totalincome = Totalincome + Decimal.Parse(row("closingbalance"))

    '                                    For i = 0 To arrHeaders.Length - 1
    '                                        phrase = New Phrase()
    '                                        phrase.Add(New Chunk(arrHeaders(i), normalfont))
    '                                        If i = 0 Then
    '                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                            cell.PaddingLeft = 24.0F
    '                                        Else
    '                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                                        End If

    '                                        cell.SetLeading(12, 0)
    '                                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                        cell.PaddingBottom = 4.0F
    '                                        cell.PaddingTop = 1.0F
    '                                        tableData.AddCell(cell)
    '                                    Next

    '                                Next
    '                                arrHeaders = {"Gross Revenue", "", ""}
    '                                For i = 0 To arrHeaders.Length - 1
    '                                    phrase = New Phrase()
    '                                    phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
    '                                    If i = 0 Then
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                        '  cell.PaddingLeft = 24.0F
    '                                    Else
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                                    End If

    '                                    cell.SetLeading(12, 0)
    '                                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                    cell.PaddingBottom = 4.0F
    '                                    cell.PaddingTop = 1.0F
    '                                    tableData.AddCell(cell)
    '                                Next

    '                                arrHeaders = {"Less : Sales Commission Discount", "", ""}
    '                                For i = 0 To arrHeaders.Length - 1
    '                                    phrase = New Phrase()
    '                                    phrase.Add(New Chunk(arrHeaders(i), normalfont))
    '                                    If i = 0 Then
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                        '  cell.PaddingLeft = 24.0F
    '                                    Else
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                                    End If

    '                                    cell.SetLeading(12, 0)
    '                                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                    cell.PaddingBottom = 4.0F
    '                                    cell.PaddingTop = 1.0F
    '                                    tableData.AddCell(cell)
    '                                Next

    '                                'added mohamed and param on 17/11/2020
    '                                Dim lPrintTotal As Double
    '                                lPrintTotal = Totalincome

    '                                If grouporder = 1 Then
    '                                    totalTitle = "Net Revenue"
    '                                    lPrintTotal = Totalincome
    '                                ElseIf grouporder = 2 Then
    '                                    totalTitle = "Total Cost of Sales"
    '                                    lPrintTotal = TotalCost
    '                                ElseIf grouporder = 4 Then
    '                                    totalTitle = "Total Expense"
    '                                    lPrintTotal = TotalExpense
    '                                Else
    '                                    totalTitle = "Toatl"
    '                                End If

    '                                arrHeaders = {totalTitle, "", lPrintTotal.ToString(decno)}
    '                                For i = 0 To arrHeaders.Length - 1
    '                                    phrase = New Phrase()
    '                                    phrase.Add(New Chunk(arrHeaders(i), normalfont))
    '                                    If i = 0 Then
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                        '  cell.PaddingLeft = 24.0F
    '                                    Else
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                                    End If

    '                                    cell.SetLeading(12, 0)
    '                                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                    cell.PaddingBottom = 4.0F
    '                                    cell.PaddingTop = 1.0F
    '                                    tableData.AddCell(cell)
    '                                Next
    '                            End If

    '                        Next

    '                        'added mohamed and param on 17/11/2020
    '                        If Totalincome - TotalCost - TotalExpense >= 0 Then
    '                            arrHeaders = {"Net Income(profit)", "", (Totalincome - TotalCost - TotalExpense - finalTotalclosingBal).ToString(decno)}
    '                        Else
    '                            arrHeaders = {"Net Income(Loss)", "", (Totalincome - TotalCost - TotalExpense - finalTotalclosingBal).ToString(decno)}
    '                        End If

    '                        ''arrHeaders = {"Net Income(Loss)", "", (Totalincome - finalTotalclosingBal).ToString(decno)}
    '                        For i = 0 To arrHeaders.Length - 1
    '                            phrase = New Phrase()
    '                            phrase.Add(New Chunk(arrHeaders(i), Level3font))
    '                            If i = 0 Then
    '                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                cell.BorderWidthRight = 0

    '                            ElseIf i = arrHeaders.Length - 1 Then
    '                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
    '                                cell.BorderWidthLeft = 0
    '                            Else
    '                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
    '                                cell.BorderWidthLeft = 0
    '                                cell.BorderWidthRight = 0
    '                            End If

    '                            cell.SetLeading(12, 0)
    '                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                            cell.PaddingBottom = 4.0F
    '                            cell.PaddingTop = 1.0F
    '                            tableData.AddCell(cell)
    '                        Next

    '                        document.Add(tableData)
    '                    End If
    '                    document.AddTitle(rptreportname)
    '                    document.Close()
    '                    If printMode = "download" Then
    '                        Dim pagingFont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)
    '                        Dim reader As New PdfReader(memoryStream.ToArray())
    '                        Using mStream As New MemoryStream()
    '                            Using stamper As New PdfStamper(reader, mStream)
    '                                Dim pages As Integer = reader.NumberOfPages
    '                                For i As Integer = 1 To pages
    '                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), documentWidth, 10.0F, 0)
    '                                Next
    '                            End Using
    '                            bytes = mStream.ToArray()
    '                        End Using
    '                    End If
    '                End Using
    '            End If
    '        Catch ex As Exception
    '            Throw ex
    '        End Try
    '    End Sub
    '#End Region

#Region "GenerateReportExcel"

    Public Sub GenerateReportExcel(ByVal divcode As String, ByVal custdetailsdt As DataTable, ByRef bytes() As Byte)
        Dim arrHeaders() As String
        Dim rowCount As Integer = 7
        Dim imagePath, DecimalPoint As String
        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add("ProfitLoss")

        '  ws.
        If divcode = "01" Then
            imagePath = "~/Images/Logo.png"
        Else
            imagePath = "~/Images/Logo.png"
        End If




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
        ws.Range(2, 1, 4, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
        'Comapny Name Heading
        ws.Cell("A2").Value = rptcompanyname
        Dim company = ws.Range("A2:C2").Merge()
        company.Style.Font.SetBold().Font.FontSize = 15
        company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
        ws.Column("A").Width = 50
        ws.Columns("B:C").Width = 25

        'Report Name Heading
        ws.Cell("A3").Value = rptreportname
        Dim report = ws.Range("A3:C3").Merge()
        report.Style.Font.SetBold().Font.FontSize = 14
        report.Style.Font.FontColor = XLColor.Black
        report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

        ws.Cell("A4").Value = rptfilter
        Dim filter = ws.Range("A4:C4").Merge()
        filter.Style.Font.SetBold().Font.FontSize = 14
        filter.Style.Font.FontColor = XLColor.Black
        filter.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        filter.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

        ws.Range("A6:C6").Style.Border.SetTopBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
        If custdetailsdt.Rows.Count > 0 Then
            Dim tabletitle = ws.Range(rowCount, 1, rowCount, 3)
            tabletitle.Style.Font.SetBold().Alignment.Vertical = XLAlignmentVerticalValues.Center
            tabletitle.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            'tabletitle.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            tabletitle.Style.Alignment.WrapText = True
            Dim currcode = custdetailsdt.AsEnumerable().Select(Function(x) New With {Key .currcode = x.Field(Of String)("currcode")}).FirstOrDefault
            arrHeaders = {"", "Amount(" & currcode.currcode.ToString() & ")", "Total Amount(" & currcode.currcode.ToString() & ")"}

            For i = 0 To arrHeaders.Length - 1
                ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
            Next

            Dim group1name, gpbyname, gpbycode, expenseTitle, groupname, totalTitle As String
            Dim grouporder As Integer
            Dim totalClosingBal, Totalincome, TotalExpense, TotalCost, finalTotalclosingBal As Decimal

            Dim groups = From gpbyrow In custdetailsdt.AsEnumerable() Group gpbyrow By g = New With {Key .grouporder = gpbyrow.Field(Of Integer)("grouporder"), Key .group1name = gpbyrow.Field(Of String)("group1name")} Into Group Order By g.grouporder

            For Each gpbyKey In groups
                grouporder = gpbyKey.g.grouporder
                groupname = gpbyKey.g.group1name
                arrHeaders = {IIf(String.Equals(groupname, "Income"), Space(3) & "Revenue", Space(3) & groupname), "", ""}
                rowCount = rowCount + 1
                ws.Range(rowCount, 1, rowCount, 3).Style.Font.SetBold().Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
                ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.WrapText = True
                For i = 0 To arrHeaders.Length - 1
                    ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
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

                        arrHeaders = {Space(6) & groupby3.g.gpbyname, "", ""}
                        rowCount = rowCount + 1
                        ws.Range(rowCount, 1, rowCount, 3).Style.Font.SetBold().Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Font.FontSize = 9
                        ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                        ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.WrapText = True
                        For i = 0 To arrHeaders.Length - 1
                            ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                            ws.Cell(rowCount, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                        Next

                        For Each row In groupby3.Group
                            arrHeaders = {Space(12) & row("acct_name"), Decimal.Parse(row("closingbalance")).ToString(decno), ""}
                            rowCount = rowCount + 1
                            ws.Range(rowCount, 1, rowCount, 3).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Font.FontSize = 9
                            ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                            ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.WrapText = True
                            For i = 0 To arrHeaders.Length - 1
                                If i = 0 Or i = 2 Then
                                    ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                                    ws.Cell(rowCount, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                                Else
                                    ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                                    ws.Cell(rowCount, i + 1).Style.NumberFormat.Format = DecimalPoint
                                    ws.Cell(rowCount, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

                                End If

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

                        arrHeaders = {Space(3) & expenseTitle, "", totalClosingBal.ToString(decno)}
                        totalClosingBal = 0
                        rowCount = rowCount + 1
                        ws.Range(rowCount, 1, rowCount, 3).Style.Font.SetBold().Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Font.FontSize = 9
                        ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                        ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.WrapText = True
                        For i = 0 To arrHeaders.Length - 1
                            If i = 0 Or i = 1 Then
                                ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                                ws.Cell(rowCount, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                            Else
                                ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                                ws.Cell(rowCount, i + 1).Style.NumberFormat.Format = DecimalPoint
                                ws.Cell(rowCount, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

                            End If

                        Next
                    Next

                Else
                    For Each row In gpbyKey.Group
                        arrHeaders = {Space(12) & row("acct_name"), Decimal.Parse(row("closingbalance")).ToString(decno), ""}

                        'added mohamed and param
                        If row("grouporder").ToString().Trim() = "4" Then
                            TotalExpense = TotalExpense + Decimal.Parse(row("closingbalance"))
                        ElseIf row("grouporder").ToString().Trim() = "2" Then
                            TotalCost = TotalCost + Decimal.Parse(row("closingbalance"))
                        Else
                            Totalincome = Totalincome + Decimal.Parse(row("closingbalance"))
                        End If
                        'Totalincome = Totalincome + Decimal.Parse(row("closingbalance"))

                        rowCount = rowCount + 1
                        ws.Range(rowCount, 1, rowCount, 3).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Font.FontSize = 9
                        ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                        ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.WrapText = True
                        For i = 0 To arrHeaders.Length - 1
                            If i = 0 Then
                                ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                                ws.Cell(rowCount, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                            Else
                                If i = 1 Then
                                    ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                                    ws.Cell(rowCount, i + 1).Style.NumberFormat.Format = DecimalPoint
                                Else
                                    ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                                End If

                                ws.Cell(rowCount, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

                            End If

                        Next
                    Next
                    arrHeaders = {"Gross Revenue", "", ""}
                    rowCount = rowCount + 1
                    ws.Range(rowCount, 1, rowCount, 3).Style.Font.SetBold().Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Font.FontSize = 9
                    ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                    ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.WrapText = True
                    For i = 0 To arrHeaders.Length - 1
                        If i = 0 Then

                            ws.Cell(rowCount, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                        Else
                            ' ws.Cell(rowCount, i + 2).Value = Decimal.Parse(arrHeaders(i))
                            ws.Cell(rowCount, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

                        End If
                        ws.Cell(rowCount, i + 1).Value = arrHeaders(i)

                    Next

                    arrHeaders = {"Less : Sales Commission Discount", "", ""}
                    rowCount = rowCount + 1
                    ws.Range(rowCount, 1, rowCount, 3).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Font.FontSize = 9
                    ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                    ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.WrapText = True
                    For i = 0 To arrHeaders.Length - 1
                        If i = 0 Then

                            ws.Cell(rowCount, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                        Else
                            ' ws.Cell(rowCount, i + 2).Value = Decimal.Parse(arrHeaders(i))
                            ws.Cell(rowCount, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

                        End If
                        ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
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
                    rowCount = rowCount + 1
                    ws.Range(rowCount, 1, rowCount, 3).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Font.FontSize = 9
                    ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
                    ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.WrapText = True
                    For i = 0 To arrHeaders.Length - 1
                        If i = 0 Or i = 1 Then
                            ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                            ws.Cell(rowCount, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                        Else
                            ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                            ws.Cell(rowCount, i + 1).Style.NumberFormat.Format = DecimalPoint
                            ws.Cell(rowCount, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

                        End If

                    Next
                End If

            Next

            'added mohamed and param on 17/11/2020
            If Totalincome - TotalCost - TotalExpense >= 0 Then
                arrHeaders = {"Net Income(profit)", "", (Totalincome - TotalCost - TotalExpense - finalTotalclosingBal).ToString(decno)}
            Else
                arrHeaders = {"Net Income(Loss)", "", (Totalincome - TotalCost - TotalExpense - finalTotalclosingBal).ToString(decno)}
            End If
            'arrHeaders = {"Net Income(Loss)", "", (Totalincome - finalTotalclosingBal).ToString(decno)}
            rowCount = rowCount + 1
            ws.Range(rowCount, 1, rowCount, 3).Style.Font.SetBold().Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
            ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            ws.Range(rowCount, 1, rowCount, 3).Style.Alignment.WrapText = True
            For i = 0 To arrHeaders.Length - 1
                If i = 0 Or i = 1 Then
                    ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                    ws.Cell(rowCount, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
                Else
                    If (Totalincome - finalTotalclosingBal) < 0 Then
                        ws.Cell(rowCount, i + 1).Value = arrHeaders(i)
                    Else
                        ws.Cell(rowCount, i + 1).Value = Decimal.Parse(arrHeaders(i))
                        ws.Cell(rowCount, i + 1).Style.NumberFormat.Format = DecimalPoint
                    End If
                    ws.Cell(rowCount, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

                End If

            Next
        End If

        ws.Cell((rowCount + 2), 1).Value = "Printed Date:" & Now.ToString("dd/MM/yyyy")
        ws.Range((rowCount + 2), 1, (rowCount + 2), 3).Merge()

        '  var(Image = ws.AddPicture(ImageLocation))
        '  .MoveTo(ws.Cell("B3").Address)
        '.Scale(0.5); // optional: resize picture
        Using wStream As New MemoryStream()
            wb.SaveAs(wStream)
            bytes = wStream.ToArray()
        End Using

    End Sub
#End Region

#Region "GenerateReportMonthWise"

    'Public Sub GenerateReportMonthwise(ByVal reportsType As String, ByVal fromdate As String, ByVal todate As String, ByVal divcode As String, ByVal rpttype As String, ByVal type As String, ByVal closing As String, ByVal strrpttype1 As String, ByRef bytes() As Byte, ByVal printMode As String, Optional ByVal fileName As String = "")
    '    Dim sqlConn As New SqlConnection

    '    Try
    '        If divcode <> "" Then
    '            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
    '        Else
    '            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
    '        End If

    '        rptreportname = " Report- Income Statement"
    '        decno = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
    '        decno = "N" & decno
    '        sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")

    '        Dim mySqlCmd As New SqlCommand
    '        Dim myDataAdapter As New SqlDataAdapter
    '        Dim ds As New DataSet
    '        mySqlCmd = New SqlCommand("sp_pf", sqlConn)
    '        mySqlCmd.CommandType = CommandType.StoredProcedure
    '        mySqlCmd.Parameters.Add(New SqlParameter("@date", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")
    '        mySqlCmd.Parameters.Add(New SqlParameter("@range", SqlDbType.VarChar, 20)).Value = 3
    '        mySqlCmd.Parameters.Add(New SqlParameter("@division", SqlDbType.VarChar, 20)).Value = divcode

    '        myDataAdapter.SelectCommand = mySqlCmd
    '        myDataAdapter.Fill(ds)
    '        Dim custdetailsdt As New DataTable
    '        View_pf = ds.Tables(0)
    '        sqlquery = "SELECT acctgroup.acctname, acctgroup.div_code, acctgroup.parentid FROM  acctgroup where div_code='" & divcode & "'"

    '        Using ds1 As New SqlDataAdapter(sqlquery, sqlConn)
    '            ds1.Fill(acctgroupTable)
    '        End Using

    '        sqlquery = "SELECT * FROM  view_actgroup"

    '        Using ds2 As New SqlDataAdapter(sqlquery, sqlConn)
    '            ds2.Fill(view_actgroup)
    '        End Using

    '        Dim currentdate As Date = Convert.ToDateTime(fromdate)
    '        month1 = currentdate.Month

    '        month2 = (currentdate.AddMonths(1)).Month
    '        month3 = (currentdate.AddMonths(2)).Month
    '        month4 = "YTD UPTO " + MonthName(month3)


    '        If reportsType = "excel" Then
    '            ExcelReportMonthWise(bytes)
    '        Else
    '            Dim document As New Document(PageSize.A4, -10.0F, 10.0F, 30.0F, 35.0F)
    '            Using memoryStream As New System.IO.MemoryStream()
    '                Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)
    '                Dim phrase As Phrase = Nothing
    '                Dim cell As PdfPCell = Nothing
    '                Dim cell1 As PdfPCell = Nothing
    '                Dim titletable As PdfPTable = Nothing
    '                Dim Rowtitlebg As BaseColor = New BaseColor(192, 192, 192)
    '                Dim CompanybgColor As BaseColor = New BaseColor(0, 72, 192)
    '                Dim ReportNamebgColor As BaseColor = New BaseColor(0, 128, 192)

    '                titletable = New PdfPTable(1)
    '                titletable.TotalWidth = documentWidth
    '                titletable.LockedWidth = True
    '                titletable.SetWidths(New Single() {1.0F})

    '                titletable.Complete = False
    '                titletable.SplitRows = False
    '                'company name
    '                phrase = New Phrase()
    '                phrase.Add(New Chunk(rptcompanyname, Companyname1))
    '                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
    '                cell.VerticalAlignment = PdfPCell.ALIGN_TOP
    '                cell.Colspan = 2
    '                cell.SetLeading(12, 0)
    '                cell.PaddingBottom = 4
    '                cell.BackgroundColor = CompanybgColor
    '                titletable.AddCell(cell)

    '                Dim Reporttitle = New PdfPTable(1)
    '                Reporttitle.TotalWidth = documentWidth
    '                Reporttitle.LockedWidth = True
    '                Reporttitle.SetWidths(New Single() {1.0F})
    '                Reporttitle.Complete = False
    '                Reporttitle.SplitRows = False
    '                phrase = New Phrase()
    '                phrase.Add(New Chunk(rptreportname, ReportNamefont1))
    '                cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
    '                cell.VerticalAlignment = PdfPCell.ALIGN_TOP
    '                cell.PaddingBottom = 4
    '                cell.SetLeading(12, 0)
    '                cell.BackgroundColor = ReportNamebgColor
    '                Reporttitle.SpacingBefore = 5
    '                Reporttitle.SpacingAfter = 0
    '                Reporttitle.AddCell(cell)
    '                Reporttitle.Complete = True

    '                addrLine1 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1063)
    '                addrLine2 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1064)
    '                addrLine3 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1065)
    '                addrLine4 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1066)
    '                addrLine5 = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1053)





    '                Dim FooterTable = New PdfPTable(1)
    '                FooterTable.TotalWidth = documentWidth
    '                FooterTable.LockedWidth = True
    '                FooterTable.SetWidths(New Single() {1.0F})
    '                FooterTable.Complete = False
    '                FooterTable.SplitRows = False
    '                phrase = New Phrase()
    '                phrase.Add(New Chunk("Printed Date:" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss"), normalfont))
    '                cell = New PdfPCell(phrase)
    '                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
    '                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
    '                cell.SetLeading(12, 0)
    '                cell.PaddingBottom = 3
    '                FooterTable.SpacingBefore = 5.0F
    '                FooterTable.AddCell(cell)
    '                FooterTable.Complete = True

    '                Dim arrHeaders() As String
    '                Dim tableTitle = New PdfPTable(5)
    '                tableTitle.TotalWidth = documentWidth
    '                tableTitle.LockedWidth = True

    '                tableTitle.Complete = False
    '                tableTitle.SplitRows = False
    '                tableTitle.Complete = False
    '                tableTitle.SplitRows = False
    '                tableTitle.SetWidths(New Single() {0.32F, 0.17F, 0.17F, 0.17F, 0.17F})

    '                arrHeaders = {"", MonthName(month1), MonthName(month2), MonthName(month3), month4}
    '                For i = 0 To arrHeaders.Length - 1
    '                    phrase = New Phrase()
    '                    phrase.Add(New Chunk(arrHeaders(i), Level3font))
    '                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
    '                    cell.SetLeading(12, 0)
    '                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                    cell.PaddingBottom = 4.0F
    '                    cell.PaddingTop = 2.0F
    '                    tableTitle.AddCell(cell)
    '                Next

    '                tableTitle.SpacingBefore = 5
    '                tableTitle.SpacingAfter = 5

    '                writer.PageEvent = New ClsHeaderFooter(titletable, Reporttitle, FooterTable, tableTitle, "printDate")

    '                document.Open()
    '                If View_pf.Rows.Count > 0 Then

    '                    Dim tableData = New PdfPTable(5)
    '                    tableData.TotalWidth = documentWidth
    '                    tableData.LockedWidth = True

    '                    tableData.Complete = False
    '                    tableData.SplitRows = False
    '                    tableData.Complete = False
    '                    tableData.SplitRows = False
    '                    tableData.SetWidths(New Single() {0.32F, 0.17F, 0.17F, 0.17F, 0.17F})

    '                    Dim acctLevel, gacctLevel0, gacctLevel1, gacctLevel2, gacctLevel3, gacctLevel4, gpbyName As New DataTable
    '                    '  Dim acctgroup1 As New DataTable
    '                    Dim amount, amount1, amount2, amount3, amount20, amount21, amount22, amount23 As Decimal
    '                    Dim acct1name, acct2name As String


    '                    Dim dataView As New DataView(View_pf)
    '                    dataView.Sort = "AccName ASC"
    '                    gpbyName = dataView.ToTable()



    '                    'Group by Level0 and Head
    '                    Dim groups = From custledger In gpbyName.AsEnumerable() Group custledger By g = New With {Key .level0 = custledger.Field(Of Integer)("level0"), Key .div_code = custledger.Field(Of String)("div_code"), Key .Head = custledger.Field(Of Integer)("Head")} Into Group Order By g.Head

    '                    For Each groupby In groups

    '                        acctLevel = groupby.Group.CopyToDataTable
    '                        'Group by GroupHead
    '                        Dim gpbygroupHeader = From custledger In acctLevel.AsEnumerable() Group custledger By g = New With {Key .groupHeader = custledger.Field(Of Integer)("GroupHeader"), Key .div_code = custledger.Field(Of String)("div_code")} Into Group Order By g.div_code


    '                        For Each gpbyHeader In gpbygroupHeader

    '                            'Level1
    '                            gacctLevel0 = gpbyHeader.Group.CopyToDataTable

    '                            ' Get the Sum of Income
    '                            Dim Getincome = gacctLevel0.Compute("SUM(Amount)", "GroupHeader='1' AND State>=1")
    '                            If IsDBNull(Getincome) Then
    '                                income = income + 0.0
    '                            Else
    '                                income = income + Convert.ToDecimal(Getincome)

    '                            End If

    '                            Dim Getincome1 = gacctLevel0.Compute("SUM(Amount1)", "GroupHeader='1' AND State>=1")
    '                            If IsDBNull(Getincome1) Then
    '                                income1 = income1 + 0.0
    '                            Else
    '                                income1 = income1 + Convert.ToDecimal(Getincome1)

    '                            End If

    '                            Dim Getincome2 = gacctLevel0.Compute("SUM(Amount2)", "GroupHeader='1' AND State>=1")
    '                            If IsDBNull(Getincome2) Then
    '                                income2 = income2 + 0.0
    '                            Else
    '                                income2 = income2 + Convert.ToDecimal(Getincome2)

    '                            End If

    '                            Dim Getincome3 = gacctLevel0.Compute("SUM(Amount3)", "GroupHeader='1' AND State>=1")
    '                            If IsDBNull(Getincome3) Then
    '                                income3 = income3 + 0.0
    '                            Else
    '                                income3 = income3 + Convert.ToDecimal(Getincome3)
    '                            End If


    '                            'Get The Sum of Expanses
    '                            Dim Getexpanse = gacctLevel0.Compute("SUM(Amount)", "GroupHeader='2' AND State>=1")
    '                            If IsDBNull(Getexpanse) Then
    '                                expanse = expanse + 0.0
    '                            Else
    '                                expanse = expanse + Convert.ToDecimal(Getexpanse)

    '                            End If

    '                            Dim Getexpanse1 = gacctLevel0.Compute("SUM(Amount1)", "GroupHeader='2' AND State>=1")
    '                            If IsDBNull(Getexpanse1) Then
    '                                expanse1 = expanse1 + 0.0
    '                            Else
    '                                expanse1 = expanse1 + Convert.ToDecimal(Getexpanse1)

    '                            End If

    '                            Dim Getexpanse2 = gacctLevel0.Compute("SUM(Amount2)", "GroupHeader='2' AND State>=1")
    '                            If IsDBNull(Getexpanse2) Then
    '                                expanse2 = expanse2 + 0.0
    '                            Else
    '                                expanse2 = expanse2 + Convert.ToDecimal(Getexpanse2)

    '                            End If

    '                            Dim Getexpanse3 = gacctLevel0.Compute("SUM(Amount3)", "GroupHeader='2' AND State>=1")
    '                            If IsDBNull(Getexpanse3) Then
    '                                expanse3 = expanse3 + 0.0
    '                            Else
    '                                expanse3 = expanse3 + Convert.ToDecimal(Getexpanse3)
    '                            End If


    '                            'Get the Sum of CostOfSales


    '                            Dim Getcostofsales = gacctLevel0.Compute("SUM(Amount)", "GroupHeader='3' AND State>=1")
    '                            If IsDBNull(Getcostofsales) Then
    '                                costofsales = costofsales + 0.0
    '                            Else
    '                                costofsales = costofsales + Convert.ToDecimal(Getcostofsales)

    '                            End If

    '                            Dim Getcostofsales1 = gacctLevel0.Compute("SUM(Amount1)", "GroupHeader='3' AND State>=1")
    '                            If IsDBNull(Getcostofsales1) Then
    '                                costofsales1 = costofsales1 + 0.0
    '                            Else
    '                                costofsales1 = costofsales1 + Convert.ToDecimal(Getcostofsales1)

    '                            End If

    '                            Dim Getcostofsales2 = gacctLevel0.Compute("SUM(Amount2)", "GroupHeader='3' AND State>=1")
    '                            If IsDBNull(Getcostofsales2) Then
    '                                costofsales2 = costofsales2 + 0.0
    '                            Else
    '                                costofsales2 = costofsales2 + Convert.ToDecimal(Getcostofsales2)

    '                            End If

    '                            Dim Getcostofsales3 = gacctLevel0.Compute("SUM(Amount3)", "GroupHeader='3' AND State>=1")
    '                            If IsDBNull(Getcostofsales3) Then
    '                                costofsales3 = costofsales3 + 0.0
    '                            Else
    '                                costofsales3 = costofsales3 + Convert.ToDecimal(Getcostofsales3)

    '                            End If

    '                            'level 1 Group By
    '                            Dim gpbyLevel1 = From custledger In gacctLevel0.AsEnumerable() Group custledger By g = New With {Key .level1 = custledger.Field(Of Integer)("level1"), Key .div_code = custledger.Field(Of String)("div_code")} Into Group Order By g.div_code

    '                            For Each gpby1 In gpbyLevel1
    '                                Dim acctgroup1 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname1 = a.Field(Of String)("acctname"), .div_code1 = a.Field(Of String)("div_code"), .parentid1 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid1 = gpby1.g.level1 And a.div_code1 = gpby1.g.div_code).OrderBy(Function(o) o.parentid1)
    '                                gacctLevel1 = gpby1.Group.CopyToDataTable



    '                                'Dim dataView1 As New DataView(gacctLevel1)
    '                                'dataView1.Sort = "AccName ASC"
    '                                'gacctLevel1 = dataView.ToTable()
    '                                amount = 0
    '                                amount1 = 0
    '                                amount2 = 0
    '                                amount3 = 0
    '                                acct1name = ""

    '                                If acctgroup1.Count > 0 Then
    '                                    For Each acct1row In acctgroup1
    '                                        acct1name = acct1row.acctname1
    '                                    Next
    '                                End If

    '                                For Each row In gpby1.Group
    '                                    If row("state") >= 1 Then
    '                                        If Not (TypeOf row("amount") Is DBNull) Then
    '                                            amount = amount + Decimal.Parse(row("amount"))
    '                                        Else
    '                                            amount = amount + 0.0
    '                                        End If

    '                                        If Not (TypeOf row("amount1") Is DBNull) Then
    '                                            amount1 = amount1 + Decimal.Parse(row("amount1"))
    '                                        Else
    '                                            amount1 = amount1 + 0.0
    '                                        End If

    '                                        If Not (TypeOf row("amount2") Is DBNull) Then
    '                                            amount2 = amount2 + Decimal.Parse(row("amount2"))
    '                                        Else
    '                                            amount2 = amount2 + 0.0
    '                                        End If
    '                                        If Not (TypeOf row("amount3") Is DBNull) Then
    '                                            amount3 = amount3 + Decimal.Parse(row("amount3"))
    '                                        Else
    '                                            amount3 = amount3 + 0.0
    '                                        End If

    '                                    End If
    '                                Next

    '                                arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
    '                                For i = 0 To arrHeaders.Length - 1
    '                                    phrase = New Phrase()
    '                                    phrase.Add(New Chunk(arrHeaders(i), Level1font))
    '                                    If i = 0 Then
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                    Else
    '                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                                    End If

    '                                    cell.SetLeading(12, 0)
    '                                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                    cell.PaddingBottom = 4.0F
    '                                    cell.PaddingTop = 1.0F
    '                                    tableData.AddCell(cell)
    '                                Next

    '                                ' Level(2)
    '                                Dim acctLevel2 = From row1 In gacctLevel1.AsEnumerable() Group row1 By g1 = New With {Key .level2 = row1.Field(Of Integer)("level2"), Key .divcode2 = row1.Field(Of String)("div_code")} Into Group Order By g1.divcode2

    '                                For Each gpbylevel2 In acctLevel2
    '                                    ' Dim acctgroup2 = acctgroupTable.Select("parentid='" & gpbylevel2.g1.level2 & "'")
    '                                    Dim acctgroup2 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname2 = a.Field(Of String)("acctname"), .div_code2 = a.Field(Of String)("div_code"), .parentid2 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid2 = gpbylevel2.g1.level2 And a.div_code2 = gpbylevel2.g1.divcode2)
    '                                    gacctLevel2 = gpbylevel2.Group.CopyToDataTable
    '                                    amount = 0
    '                                    amount1 = 0
    '                                    amount2 = 0
    '                                    amount3 = 0
    '                                    acct1name = ""
    '                                    acct2name = ""
    '                                    If acctgroup2.Count > 0 Then

    '                                        For Each acct2row In acctgroup2
    '                                            acct1name = acct2row.acctname2
    '                                            acct2name = acct2row.acctname2
    '                                        Next
    '                                    End If
    '                                    For Each row In gpbylevel2.Group
    '                                        If row("state") >= 1 Then
    '                                            If Not (TypeOf row("amount") Is DBNull) Then
    '                                                amount = amount + Decimal.Parse(row("amount"))
    '                                            Else
    '                                                amount = amount + 0.0
    '                                            End If

    '                                            If Not (TypeOf row("amount1") Is DBNull) Then
    '                                                amount1 = amount1 + Decimal.Parse(row("amount1"))
    '                                            Else
    '                                                amount1 = amount1 + 0.0
    '                                            End If

    '                                            If Not (TypeOf row("amount2") Is DBNull) Then
    '                                                amount2 = amount2 + Decimal.Parse(row("amount2"))
    '                                            Else
    '                                                amount2 = amount2 + 0.0
    '                                            End If
    '                                            If Not (TypeOf row("amount3") Is DBNull) Then
    '                                                amount3 = amount3 + Decimal.Parse(row("amount3"))
    '                                            Else
    '                                                amount3 = amount3 + 0.0
    '                                            End If

    '                                        End If

    '                                    Next

    '                                    arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
    '                                    For i = 0 To arrHeaders.Length - 1
    '                                        phrase = New Phrase()
    '                                        phrase.Add(New Chunk(arrHeaders(i), Level2font))
    '                                        If i = 0 Then
    '                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                        Else
    '                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                                        End If
    '                                        cell.SetLeading(12, 0)
    '                                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                        cell.PaddingBottom = 4.0F
    '                                        cell.PaddingTop = 1.0F
    '                                        tableData.AddCell(cell)
    '                                    Next

    '                                    amount20 = amount
    '                                    amount21 = amount1
    '                                    amount22 = amount2
    '                                    amount23 = amount3

    '                                    ' Level3
    '                                    Dim acctLevel3 = From row1 In gacctLevel2.AsEnumerable() Group row1 By g1 = New With {Key .level3 = row1.Field(Of Integer)("level3"), Key .divcode3 = row1.Field(Of String)("div_code")} Into Group Order By g1.divcode3
    '                                    For Each gpbylevel3 In acctLevel3
    '                                        Dim acctgroup3 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname3 = a.Field(Of String)("acctname"), .div_code3 = a.Field(Of String)("div_code"), .parentid3 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid3 = gpbylevel3.g1.level3 And a.div_code3 = gpbylevel3.g1.divcode3).OrderBy(Function(o) o.acctname3)
    '                                        gacctLevel3 = gpbylevel3.Group.CopyToDataTable

    '                                        amount = 0
    '                                        amount1 = 0
    '                                        amount2 = 0
    '                                        amount3 = 0
    '                                        acct1name = ""
    '                                        If acctgroup3.Count > 0 Then
    '                                            For Each acct3row In acctgroup3
    '                                                acct1name = acct3row.acctname3
    '                                            Next
    '                                        End If
    '                                        For Each row In gpbylevel3.Group
    '                                            If row("state") >= 3 Then
    '                                                If Not (TypeOf row("amount") Is DBNull) Then
    '                                                    amount = amount + Decimal.Parse(row("amount"))
    '                                                Else
    '                                                    amount = amount + 0.0
    '                                                End If

    '                                                If Not (TypeOf row("amount1") Is DBNull) Then
    '                                                    amount1 = amount1 + Decimal.Parse(row("amount1"))
    '                                                Else
    '                                                    amount1 = amount1 + 0.0
    '                                                End If

    '                                                If Not (TypeOf row("amount2") Is DBNull) Then
    '                                                    amount2 = amount2 + Decimal.Parse(row("amount2"))
    '                                                Else
    '                                                    amount2 = amount2 + 0.0
    '                                                End If
    '                                                If Not (TypeOf row("amount3") Is DBNull) Then
    '                                                    amount3 = amount3 + Decimal.Parse(row("amount3"))
    '                                                Else
    '                                                    amount3 = amount3 + 0.0
    '                                                End If

    '                                            End If

    '                                        Next

    '                                        arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
    '                                        For i = 0 To arrHeaders.Length - 1
    '                                            phrase = New Phrase()
    '                                            phrase.Add(New Chunk(arrHeaders(i), Level3font))
    '                                            If i = 0 Then
    '                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                            Else
    '                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                                            End If
    '                                            cell.SetLeading(12, 0)
    '                                            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                            cell.PaddingBottom = 4.0F
    '                                            cell.PaddingTop = 1.0F
    '                                            tableData.AddCell(cell)
    '                                        Next


    '                                        'level4
    '                                        Dim acctLevel4 = From row1 In gacctLevel3.AsEnumerable() Group row1 By g1 = New With {Key .level4 = row1.Field(Of Integer)("level4"), Key .divcode4 = row1.Field(Of String)("div_code")} Into Group Order By g1.divcode4

    '                                        For Each gpbylevel4 In acctLevel4
    '                                            Dim acctgroup4 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname4 = a.Field(Of String)("acctname"), .div_code4 = a.Field(Of String)("div_code"), .parentid4 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid4 = gpbylevel4.g1.level4 And a.div_code4 = gpbylevel4.g1.divcode4).OrderBy(Function(o) o.acctname4)
    '                                            gacctLevel4 = gpbylevel4.Group.CopyToDataTable

    '                                            If acctgroup4.Count > 0 Then
    '                                                amount = 0
    '                                                amount1 = 0
    '                                                amount2 = 0
    '                                                amount3 = 0
    '                                                acct1name = ""
    '                                                For Each acct4row In acctgroup4
    '                                                    acct1name = acct4row.acctname4
    '                                                Next
    '                                            Else
    '                                                acct1name = ""
    '                                            End If
    '                                            For Each row In gpbylevel4.Group
    '                                                If row("state") >= 4 Then
    '                                                    If Not (TypeOf row("amount") Is DBNull) Then
    '                                                        amount = amount + Decimal.Parse(row("amount"))
    '                                                    Else
    '                                                        amount = amount + 0.0
    '                                                    End If

    '                                                    If Not (TypeOf row("amount1") Is DBNull) Then
    '                                                        amount1 = amount1 + Decimal.Parse(row("amount1"))
    '                                                    Else
    '                                                        amount1 = amount1 + 0.0
    '                                                    End If

    '                                                    If Not (TypeOf row("amount2") Is DBNull) Then
    '                                                        amount2 = amount2 + Decimal.Parse(row("amount2"))
    '                                                    Else
    '                                                        amount2 = amount2 + 0.0
    '                                                    End If
    '                                                    If Not (TypeOf row("amount3") Is DBNull) Then
    '                                                        amount3 = amount3 + Decimal.Parse(row("amount3"))
    '                                                    Else
    '                                                        amount3 = amount3 + 0.0
    '                                                    End If

    '                                                End If

    '                                            Next

    '                                            arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
    '                                            For i = 0 To arrHeaders.Length - 1
    '                                                phrase = New Phrase()
    '                                                phrase.Add(New Chunk(arrHeaders(i), Level4font))
    '                                                If i = 0 Then
    '                                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                                Else
    '                                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                                                End If
    '                                                cell.SetLeading(12, 0)
    '                                                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                                cell.PaddingBottom = 4.0F
    '                                                cell.PaddingTop = 1.0F
    '                                                tableData.AddCell(cell)
    '                                            Next

    '                                            'level5

    '                                            Dim acctLevel5 = From row1 In gacctLevel4.AsEnumerable() Group row1 By g1 = New With {Key .level5 = row1.Field(Of Integer)("level5"), Key .divcode5 = row1.Field(Of String)("div_code")} Into Group Order By g1.divcode5

    '                                            For Each gpbylevel5 In acctLevel5
    '                                                Dim acctgroup5 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname5 = a.Field(Of String)("acctname"), .div_code5 = a.Field(Of String)("div_code"), .parentid5 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid5 = gpbylevel5.g1.level5 And a.div_code5 = gpbylevel5.g1.divcode5).OrderBy(Function(o) o.acctname5)
    '                                                'gacctLevel5 = gpbylevel5.Group.CopyToDataTable
    '                                                If acctgroup5.Count > 0 Then
    '                                                    amount = 0
    '                                                    amount1 = 0
    '                                                    amount2 = 0
    '                                                    amount3 = 0
    '                                                    acct1name = ""
    '                                                    For Each acct5row In acctgroup5
    '                                                        acct1name = acct5row.acctname5
    '                                                    Next
    '                                                Else
    '                                                    acct1name = ""
    '                                                End If

    '                                                For Each row In gpbylevel5.Group
    '                                                    If row("state") >= 5 Then
    '                                                        If Not (TypeOf row("amount") Is DBNull) Then
    '                                                            amount = amount + Decimal.Parse(row("amount"))
    '                                                        Else
    '                                                            amount = amount + 0.0
    '                                                        End If

    '                                                        If Not (TypeOf row("amount1") Is DBNull) Then
    '                                                            amount1 = amount1 + Decimal.Parse(row("amount1"))
    '                                                        Else
    '                                                            amount1 = amount1 + 0.0
    '                                                        End If

    '                                                        If Not (TypeOf row("amount2") Is DBNull) Then
    '                                                            amount2 = amount2 + Decimal.Parse(row("amount2"))
    '                                                        Else
    '                                                            amount2 = amount2 + 0.0
    '                                                        End If
    '                                                        If Not (TypeOf row("amount3") Is DBNull) Then
    '                                                            amount3 = amount3 + Decimal.Parse(row("amount3"))
    '                                                        Else
    '                                                            amount3 = amount3 + 0.0
    '                                                        End If

    '                                                    End If

    '                                                Next

    '                                                arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
    '                                                For i = 0 To arrHeaders.Length - 1
    '                                                    phrase = New Phrase()
    '                                                    phrase.Add(New Chunk(arrHeaders(i), Level5font))
    '                                                    If i = 0 Then
    '                                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                                    Else
    '                                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                                                    End If
    '                                                    cell.SetLeading(12, 0)
    '                                                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                                    cell.PaddingBottom = 4.0F
    '                                                    cell.PaddingTop = 1.0F
    '                                                    tableData.AddCell(cell)
    '                                                Next
    '                                                'Else
    '                                                '    arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
    '                                                '    For i = 0 To arrHeaders.Length - 1
    '                                                '        phrase = New Phrase()
    '                                                '        phrase.Add(New Chunk(arrHeaders(i), Level5font))
    '                                                '        If i = 0 Then
    '                                                '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                                '        Else
    '                                                '            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                                                '        End If
    '                                                '        cell.SetLeading(12, 0)
    '                                                '        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                                '        cell.PaddingBottom = 4.0F
    '                                                '        cell.PaddingTop = 1.0F
    '                                                '        tableData.AddCell(cell)
    '                                                '    Next
    '                                                'End If
    '                                            Next
    '                                            'End level 5

    '                                            'Else
    '                                            '    arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
    '                                            '    For i = 0 To arrHeaders.Length - 1
    '                                            '        phrase = New Phrase()
    '                                            '        phrase.Add(New Chunk(arrHeaders(i), Level4font))
    '                                            '        If i = 0 Then
    '                                            '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                            '        Else
    '                                            '            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                                            '        End If
    '                                            '        cell.SetLeading(12, 0)
    '                                            '        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                            '        cell.PaddingBottom = 4.0F
    '                                            '        cell.PaddingTop = 1.0F
    '                                            '        tableData.AddCell(cell)
    '                                            '    Next

    '                                            'End If
    '                                        Next
    '                                        'End Level 4


    '                                        'Else

    '                                        '    arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
    '                                        '    For i = 0 To arrHeaders.Length - 1
    '                                        '        phrase = New Phrase()
    '                                        '        phrase.Add(New Chunk(arrHeaders(i), Level3font))
    '                                        '        If i = 0 Then
    '                                        '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                        '        Else
    '                                        '            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                                        '        End If
    '                                        '        cell.SetLeading(12, 0)
    '                                        '        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                        '        cell.PaddingBottom = 4.0F
    '                                        '        cell.PaddingTop = 1.0F
    '                                        '        tableData.AddCell(cell)
    '                                        '    Next

    '                                        'End If
    '                                    Next
    '                                    'end level 3

    '                                    'Else

    '                                    '    arrHeaders = {"", amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno)}
    '                                    '    For i = 0 To arrHeaders.Length - 1
    '                                    '        phrase = New Phrase()
    '                                    '        phrase.Add(New Chunk(arrHeaders(i), Level2font))
    '                                    '        If i = 0 Then
    '                                    '            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                    '        Else
    '                                    '            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                                    '        End If
    '                                    '        cell.SetLeading(12, 0)
    '                                    '        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                    '        cell.PaddingBottom = 4.0F
    '                                    '        cell.PaddingTop = 1.0F
    '                                    '        tableData.AddCell(cell)
    '                                    '    Next

    '                                    'End If


    '                                    If String.Equals(acct2name, "Income") Then
    '                                        acct2name = "Total Revenue"
    '                                    ElseIf String.Equals(acct2name, "Cost Of Sales") Then
    '                                        acct2name = "Total Cost Of Sales"
    '                                    ElseIf String.Equals(acct2name, "Expenses") Then
    '                                        acct2name = "Other Direct Costs"

    '                                    Else
    '                                        acct2name = ""
    '                                    End If

    '                                    arrHeaders = {acct2name, amount20.ToString(decno), amount21.ToString(decno), amount22.ToString(decno), amount23.ToString(decno)}
    '                                    For i = 0 To arrHeaders.Length - 1
    '                                        phrase = New Phrase()
    '                                        phrase.Add(New Chunk(arrHeaders(i), Level4font))
    '                                        If i = 0 Then
    '                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                                        Else
    '                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                                        End If
    '                                        cell.SetLeading(12, 0)
    '                                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                                        cell.PaddingBottom = 4.0F
    '                                        cell.PaddingTop = 1.0F
    '                                        tableData.AddCell(cell)
    '                                    Next
    '                                Next

    '                                'End level 2
    '                            Next
    '                            'End level 1
    '                        Next

    '                    Next

    '                    arrHeaders = {"NET PROFIT", (income - expanse - costofsales).ToString(decno), (income1 - expanse1 - costofsales1).ToString(decno), (income2 - expanse2 - costofsales2).ToString(decno), (income3 - expanse3 - costofsales3).ToString(decno)}
    '                    For i = 0 To arrHeaders.Length - 1
    '                        phrase = New Phrase()
    '                        phrase.Add(New Chunk(arrHeaders(i), Level3font))
    '                        If i = 0 Then
    '                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
    '                        Else
    '                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

    '                        End If

    '                        cell.SetLeading(12, 0)
    '                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
    '                        cell.PaddingBottom = 4.0F
    '                        cell.PaddingTop = 1.0F
    '                        tableData.AddCell(cell)
    '                    Next

    '                    document.Add(tableData)
    '                End If
    '                document.AddTitle(rptreportname)
    '                document.Close()
    '                If printMode = "download" Then
    '                    Dim pagingFont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)
    '                    Dim reader As New PdfReader(memoryStream.ToArray())
    '                    Using mStream As New MemoryStream()
    '                        Using stamper As New PdfStamper(reader, mStream)
    '                            Dim pages As Integer = reader.NumberOfPages
    '                            For i As Integer = 1 To pages
    '                                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), documentWidth, 10.0F, 0)
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
            Dim stprocname As String
            Dim range As Integer
            'Tanvir 12012023
            If reportsType = "excel" Then
                stprocname = "sp_pf_yearwise"
                range = 12
            Else
                stprocname = "sp_pf"
                range = 3
            End If
            mySqlCmd = New SqlCommand(stprocname, sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0 'Tanvir 04052023
            mySqlCmd.Parameters.Add(New SqlParameter("@date", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@range", SqlDbType.VarChar, 20)).Value = range
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
            month4 = (currentdate.AddMonths(3)).Month
            month5 = (currentdate.AddMonths(4)).Month
            month6 = (currentdate.AddMonths(5)).Month
            month7 = (currentdate.AddMonths(6)).Month
            month8 = (currentdate.AddMonths(7)).Month
            month9 = (currentdate.AddMonths(8)).Month
            month10 = (currentdate.AddMonths(9)).Month
            month11 = (currentdate.AddMonths(10)).Month
            month12 = (currentdate.AddMonths(11)).Month
            month13 = "YTD UPTO " + MonthName(month12)


            If reportsType = "excel" Then
                ExcelReportYearMonthWise(bytes) 'Tanvir 11012024
                'ExcelReportMonthWise(bytes)
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
                        Dim groups = From custledger In gpbyName.AsEnumerable() Group custledger By g = New With {Key .level0 = custledger.Field(Of Integer)("level0"), Key .div_code = custledger.Field(Of String)("div_code"), Key .Head = custledger.Field(Of Integer)("Head"), Key .GroupHeader = custledger.Field(Of Integer)("GroupHeader")} Into Group Order By g.Head, g.GroupHeader

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
                                Dim Getexpanse = gacctLevel0.Compute("SUM(Amount)", "GroupHeader='3' AND State>=1")
                                If IsDBNull(Getexpanse) Then
                                    expanse = expanse + 0.0
                                Else
                                    expanse = expanse + Convert.ToDecimal(Getexpanse)

                                End If

                                Dim Getexpanse1 = gacctLevel0.Compute("SUM(Amount1)", "GroupHeader='3' AND State>=1")
                                If IsDBNull(Getexpanse1) Then
                                    expanse1 = expanse1 + 0.0
                                Else
                                    expanse1 = expanse1 + Convert.ToDecimal(Getexpanse1)

                                End If

                                Dim Getexpanse2 = gacctLevel0.Compute("SUM(Amount2)", "GroupHeader='3' AND State>=1")
                                If IsDBNull(Getexpanse2) Then
                                    expanse2 = expanse2 + 0.0
                                Else
                                    expanse2 = expanse2 + Convert.ToDecimal(Getexpanse2)

                                End If

                                Dim Getexpanse3 = gacctLevel0.Compute("SUM(Amount3)", "GroupHeader='3' AND State>=1")
                                If IsDBNull(Getexpanse3) Then
                                    expanse3 = expanse3 + 0.0
                                Else
                                    expanse3 = expanse3 + Convert.ToDecimal(Getexpanse3)
                                End If


                                'Get the Sum of CostOfSales


                                Dim Getcostofsales = gacctLevel0.Compute("SUM(Amount)", "GroupHeader='2' AND State>=1")
                                If IsDBNull(Getcostofsales) Then
                                    costofsales = costofsales + 0.0
                                Else
                                    costofsales = costofsales + Convert.ToDecimal(Getcostofsales)

                                End If

                                Dim Getcostofsales1 = gacctLevel0.Compute("SUM(Amount1)", "GroupHeader='2' AND State>=1")
                                If IsDBNull(Getcostofsales1) Then
                                    costofsales1 = costofsales1 + 0.0
                                Else
                                    costofsales1 = costofsales1 + Convert.ToDecimal(Getcostofsales1)

                                End If

                                Dim Getcostofsales2 = gacctLevel0.Compute("SUM(Amount2)", "GroupHeader='2' AND State>=1")
                                If IsDBNull(Getcostofsales2) Then
                                    costofsales2 = costofsales2 + 0.0
                                Else
                                    costofsales2 = costofsales2 + Convert.ToDecimal(Getcostofsales2)

                                End If

                                Dim Getcostofsales3 = gacctLevel0.Compute("SUM(Amount3)", "GroupHeader='2' AND State>=1")
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


                                        If String.Equals(acct2name, "Income") Or String.Equals(acct2name, "Direct Revenues") Then
                                            acct2name = "Total Revenue"
                                        ElseIf String.Equals(acct2name, "Cost Of Sales") Or String.Equals(acct2name, "Direct Cost") Then
                                            acct2name = "Total Cost Of Sales"
                                        ElseIf String.Equals(acct2name, "Expenses") Or String.Equals(acct2name, "Indirect Cost") Then
                                            acct2name = "Other Costs"
                                        Else
                                            acct2name = acct2name ' "" 'changed by mohamed on 26/09/2021
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

                            If groupby.g.GroupHeader = 2 Then
                                arrHeaders = {"GROSS PROFIT", (income - costofsales).ToString(decno), (income1 - costofsales1).ToString(decno), (income2 - costofsales2).ToString(decno), (income3 - costofsales3).ToString(decno)}
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
#End Region

    Public Sub ExcelReportYearMonthWise(ByRef bytes() As Byte)

        Dim arrHeaders() As String
        Dim rowCount As Integer = 5
        Dim imagePath, DecimalPoint As String
        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add("ProfitandLoss")

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
        ws.Column("A").Width = 30
        ws.Columns("B:N").Width = 20
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


        arrHeaders = {"", MonthName(month1), MonthName(month2), MonthName(month3), MonthName(month4), MonthName(month5), MonthName(month6), MonthName(month7), MonthName(month8), MonthName(month9), MonthName(month10), MonthName(month11), MonthName(month12), month13}
        ws.Range(5, 1, 5, 13).Style.Font.SetBold.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetTopBorder(XLBorderStyleValues.Thin).Font.FontSize = 12
        ws.Range(5, 1, 5, 13).Style.Alignment.WrapText = True
        For i = 0 To arrHeaders.Length - 1
            ws.Cell(5, i + 1).Value = arrHeaders(i)
        Next

        If View_pf.Rows.Count > 0 Then
            Dim acctLevel, gacctLevel0, gacctLevel1, gacctLevel2, gacctLevel3, gacctLevel4, gpbyName As New DataTable
            '  Dim acctgroup1 As New DataTable
            Dim amount, amount1, amount2, amount3, amount4, amount5, amount6, amount20, amount21, amount22, amount23, amount24, amount25, amount26 As Decimal
            Dim amount7, amount8, amount9, amount10, amount11, amount12, amount27, amount28, amount29, amount210, amount211, amount212 As Decimal
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

                    Dim Getincome4 = gacctLevel0.Compute("SUM(Amount4)", "GroupHeader='1' AND State>=1")
                    If IsDBNull(Getincome4) Then
                        income4 = income4 + 0.0
                    Else
                        income4 = income4 + Convert.ToDecimal(Getincome4)
                    End If



                    Dim Getincome5 = gacctLevel0.Compute("SUM(Amount5)", "GroupHeader='1' AND State>=1")
                    If IsDBNull(Getincome5) Then
                        income5 = income5 + 0.0
                    Else
                        income5 = income5 + Convert.ToDecimal(Getincome5)
                    End If

                    Dim Getincome6 = gacctLevel0.Compute("SUM(Amount6)", "GroupHeader='1' AND State>=1")
                    If IsDBNull(Getincome6) Then
                        income6 = income6 + 0.0
                    Else
                        income6 = income6 + Convert.ToDecimal(Getincome6)
                    End If


                    Dim Getincome7 = gacctLevel0.Compute("SUM(Amount7)", "GroupHeader='1' AND State>=1")
                    If IsDBNull(Getincome7) Then
                        income7 = income7 + 0.0
                    Else
                        income7 = income7 + Convert.ToDecimal(Getincome7)
                    End If


                    Dim Getincome8 = gacctLevel0.Compute("SUM(Amount8)", "GroupHeader='1' AND State>=1")
                    If IsDBNull(Getincome8) Then
                        income8 = income8 + 0.0
                    Else
                        income8 = income8 + Convert.ToDecimal(Getincome8)
                    End If


                    Dim Getincome9 = gacctLevel0.Compute("SUM(Amount9)", "GroupHeader='1' AND State>=1")
                    If IsDBNull(Getincome9) Then
                        income9 = income9 + 0.0
                    Else
                        income9 = income9 + Convert.ToDecimal(Getincome9)
                    End If


                    Dim Getincome10 = gacctLevel0.Compute("SUM(Amount10)", "GroupHeader='1' AND State>=1")
                    If IsDBNull(Getincome10) Then
                        income10 = income10 + 0.0
                    Else
                        income10 = income10 + Convert.ToDecimal(Getincome10)
                    End If


                    Dim Getincome11 = gacctLevel0.Compute("SUM(Amount11)", "GroupHeader='1' AND State>=1")
                    If IsDBNull(Getincome11) Then
                        income11 = income11 + 0.0
                    Else
                        income11 = income11 + Convert.ToDecimal(Getincome11)
                    End If

                    Dim Getincome12 = gacctLevel0.Compute("SUM(Amount12)", "GroupHeader='1' AND State>=1")
                    If IsDBNull(Getincome12) Then
                        income12 = income12 + 0.0
                    Else
                        income12 = income12 + Convert.ToDecimal(Getincome12)
                    End If




                    'Get The Sum of Expanses
                    Dim Getexpanse = gacctLevel0.Compute("SUM(Amount)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse) Then
                        expanse = expanse + 0.0
                    Else
                        expanse = expanse + Convert.ToDecimal(Getexpanse)

                    End If

                    Dim Getexpanse1 = gacctLevel0.Compute("SUM(Amount1)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse1) Then
                        expanse1 = expanse1 + 0.0
                    Else
                        expanse1 = expanse1 + Convert.ToDecimal(Getexpanse1)

                    End If

                    Dim Getexpanse2 = gacctLevel0.Compute("SUM(Amount2)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse2) Then
                        expanse2 = expanse2 + 0.0
                    Else
                        expanse2 = expanse2 + Convert.ToDecimal(Getexpanse2)

                    End If

                    Dim Getexpanse3 = gacctLevel0.Compute("SUM(Amount3)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse3) Then
                        expanse3 = expanse3 + 0.0
                    Else
                        expanse3 = expanse3 + Convert.ToDecimal(Getexpanse3)
                    End If

                    Dim Getexpanse4 = gacctLevel0.Compute("SUM(Amount4)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse4) Then
                        expanse4 = expanse4 + 0.0
                    Else
                        expanse4 = expanse4 + Convert.ToDecimal(Getexpanse4)
                    End If

                    Dim Getexpanse5 = gacctLevel0.Compute("SUM(Amount5)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse5) Then
                        expanse5 = expanse5 + 0.0
                    Else
                        expanse5 = expanse5 + Convert.ToDecimal(Getexpanse5)
                    End If

                    Dim Getexpanse6 = gacctLevel0.Compute("SUM(Amount6)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse6) Then
                        expanse6 = expanse6 + 0.0
                    Else
                        expanse6 = expanse6 + Convert.ToDecimal(Getexpanse6)
                    End If





                    Dim Getexpanse7 = gacctLevel0.Compute("SUM(Amount7)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse7) Then
                        expanse7 = expanse7 + 0.0
                    Else
                        expanse7 = expanse7 + Convert.ToDecimal(Getexpanse7)
                    End If

                    Dim Getexpanse8 = gacctLevel0.Compute("SUM(Amount8)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse8) Then
                        expanse8 = expanse8 + 0.0
                    Else
                        expanse8 = expanse8 + Convert.ToDecimal(Getexpanse8)
                    End If

                    Dim Getexpanse9 = gacctLevel0.Compute("SUM(Amount9)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse9) Then
                        expanse9 = expanse9 + 0.0
                    Else
                        expanse9 = expanse9 + Convert.ToDecimal(Getexpanse9)
                    End If

                    Dim Getexpanse10 = gacctLevel0.Compute("SUM(Amount10)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse10) Then
                        expanse10 = expanse10 + 0.0
                    Else
                        expanse10 = expanse10 + Convert.ToDecimal(Getexpanse10)
                    End If


                    Dim Getexpanse11 = gacctLevel0.Compute("SUM(Amount11)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse11) Then
                        expanse11 = expanse11 + 0.0
                    Else
                        expanse11 = expanse11 + Convert.ToDecimal(Getexpanse11)
                    End If

                    Dim Getexpanse12 = gacctLevel0.Compute("SUM(Amount12)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse12) Then
                        expanse12 = expanse12 + 0.0
                    Else
                        expanse12 = expanse12 + Convert.ToDecimal(Getexpanse12)
                    End If


                    'Get the Sum of CostOfSales


                    Dim Getcostofsales = gacctLevel0.Compute("SUM(Amount)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getcostofsales) Then
                        costofsales = costofsales + 0.0
                    Else
                        costofsales = costofsales + Convert.ToDecimal(Getcostofsales)

                    End If

                    Dim Getcostofsales1 = gacctLevel0.Compute("SUM(Amount1)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getcostofsales1) Then
                        costofsales1 = costofsales1 + 0.0
                    Else
                        costofsales1 = costofsales1 + Convert.ToDecimal(Getcostofsales1)

                    End If

                    Dim Getcostofsales2 = gacctLevel0.Compute("SUM(Amount2)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getcostofsales2) Then
                        costofsales2 = costofsales2 + 0.0
                    Else
                        costofsales2 = costofsales2 + Convert.ToDecimal(Getcostofsales2)

                    End If

                    Dim Getcostofsales3 = gacctLevel0.Compute("SUM(Amount3)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getcostofsales3) Then
                        costofsales3 = costofsales3 + 0.0
                    Else
                        costofsales3 = costofsales3 + Convert.ToDecimal(Getcostofsales3)

                    End If



                    Dim Getcostofsales4 = gacctLevel0.Compute("SUM(Amount4)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getcostofsales4) Then
                        costofsales4 = costofsales4 + 0.0
                    Else
                        costofsales4 = costofsales4 + Convert.ToDecimal(Getcostofsales4)

                    End If

                    Dim Getcostofsales5 = gacctLevel0.Compute("SUM(Amount5)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getcostofsales5) Then
                        costofsales5 = costofsales5 + 0.0
                    Else
                        costofsales5 = costofsales5 + Convert.ToDecimal(Getcostofsales5)

                    End If
                    Dim Getcostofsales6 = gacctLevel0.Compute("SUM(Amount6)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getcostofsales6) Then
                        costofsales6 = costofsales6 + 0.0
                    Else
                        costofsales6 = costofsales6 + Convert.ToDecimal(Getcostofsales6)

                    End If


                    Dim Getcostofsales7 = gacctLevel0.Compute("SUM(Amount7)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getcostofsales7) Then
                        costofsales7 = costofsales7 + 0.0
                    Else
                        costofsales7 = costofsales7 + Convert.ToDecimal(Getcostofsales7)

                    End If

                    Dim Getcostofsales8 = gacctLevel0.Compute("SUM(Amount8)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getcostofsales8) Then
                        costofsales8 = costofsales8 + 0.0
                    Else
                        costofsales8 = costofsales8 + Convert.ToDecimal(Getcostofsales8)

                    End If


                    Dim Getcostofsales9 = gacctLevel0.Compute("SUM(Amount9)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getcostofsales9) Then
                        costofsales9 = costofsales9 + 0.0
                    Else
                        costofsales9 = costofsales9 + Convert.ToDecimal(Getcostofsales9)

                    End If


                    Dim Getcostofsales10 = gacctLevel0.Compute("SUM(Amount10)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getcostofsales10) Then
                        costofsales10 = costofsales10 + 0.0
                    Else
                        costofsales10 = costofsales10 + Convert.ToDecimal(Getcostofsales10)

                    End If

                    Dim Getcostofsales11 = gacctLevel0.Compute("SUM(Amount11)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getcostofsales11) Then
                        costofsales11 = costofsales11 + 0.0
                    Else
                        costofsales11 = costofsales11 + Convert.ToDecimal(Getcostofsales11)

                    End If

                    Dim Getcostofsales12 = gacctLevel0.Compute("SUM(Amount12)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getcostofsales12) Then
                        costofsales12 = costofsales12 + 0.0
                    Else
                        costofsales12 = costofsales12 + Convert.ToDecimal(Getcostofsales12)

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
                        amount4 = 0
                        amount5 = 0
                        amount6 = 0
                        amount7 = 0
                        amount8 = 0
                        amount9 = 0
                        amount10 = 0
                        amount11 = 0
                        amount12 = 0

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

                                If Not (TypeOf row("amount4") Is DBNull) Then
                                    amount4 = amount4 + Decimal.Parse(row("amount4"))
                                Else
                                    amount4 = amount4 + 0.0
                                End If
                                If Not (TypeOf row("amount5") Is DBNull) Then
                                    amount5 = amount5 + Decimal.Parse(row("amount5"))
                                Else
                                    amount5 = amount5 + 0.0
                                End If
                                If Not (TypeOf row("amount6") Is DBNull) Then
                                    amount6 = amount6 + Decimal.Parse(row("amount6"))
                                Else
                                    amount6 = amount6 + 0.0
                                End If
                                If Not (TypeOf row("amount7") Is DBNull) Then
                                    amount7 = amount7 + Decimal.Parse(row("amount7"))
                                Else
                                    amount7 = amount7 + 0.0
                                End If
                                If Not (TypeOf row("amount8") Is DBNull) Then
                                    amount8 = amount8 + Decimal.Parse(row("amount8"))
                                Else
                                    amount8 = amount8 + 0.0
                                End If
                                If Not (TypeOf row("amount9") Is DBNull) Then
                                    amount9 = amount9 + Decimal.Parse(row("amount9"))
                                Else
                                    amount9 = amount9 + 0.0
                                End If
                                If Not (TypeOf row("amount10") Is DBNull) Then
                                    amount10 = amount10 + Decimal.Parse(row("amount10"))
                                Else
                                    amount10 = amount10 + 0.0
                                End If
                                If Not (TypeOf row("amount11") Is DBNull) Then
                                    amount11 = amount11 + Decimal.Parse(row("amount11"))
                                Else
                                    amount11 = amount11 + 0.0
                                End If
                                If Not (TypeOf row("amount12") Is DBNull) Then
                                    amount12 = amount12 + Decimal.Parse(row("amount12"))
                                Else
                                    amount12 = amount12 + 0.0
                                End If
                            End If
                        Next

                        arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno), amount4.ToString(decno), amount5.ToString(decno), amount6.ToString(decno), amount7.ToString(decno), amount8.ToString(decno), amount9.ToString(decno), amount10.ToString(decno), amount11.ToString(decno), amount12.ToString(decno)}
                        rowCount = rowCount + 1
                        ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 12
                        ' ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
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
                            amount4 = 0
                            amount5 = 0
                            amount6 = 0
                            amount7 = 0
                            amount8 = 0
                            amount9 = 0
                            amount10 = 0
                            amount11 = 0
                            amount12 = 0
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

                                    If Not (TypeOf row("amount4") Is DBNull) Then
                                        amount4 = amount4 + Decimal.Parse(row("amount4"))
                                    Else
                                        amount4 = amount4 + 0.0
                                    End If
                                    If Not (TypeOf row("amount5") Is DBNull) Then
                                        amount5 = amount5 + Decimal.Parse(row("amount5"))
                                    Else
                                        amount5 = amount5 + 0.0
                                    End If
                                    If Not (TypeOf row("amount6") Is DBNull) Then
                                        amount6 = amount6 + Decimal.Parse(row("amount6"))
                                    Else
                                        amount6 = amount6 + 0.0
                                    End If


                                    If Not (TypeOf row("amount7") Is DBNull) Then
                                        amount7 = amount7 + Decimal.Parse(row("amount7"))
                                    Else
                                        amount7 = amount7 + 0.0
                                    End If

                                    If Not (TypeOf row("amount8") Is DBNull) Then
                                        amount8 = amount8 + Decimal.Parse(row("amount8"))
                                    Else
                                        amount8 = amount8 + 0.0
                                    End If


                                    If Not (TypeOf row("amount9") Is DBNull) Then
                                        amount9 = amount9 + Decimal.Parse(row("amount9"))
                                    Else
                                        amount9 = amount9 + 0.0
                                    End If

                                    If Not (TypeOf row("amount10") Is DBNull) Then
                                        amount10 = amount10 + Decimal.Parse(row("amount10"))
                                    Else
                                        amount10 = amount10 + 0.0
                                    End If

                                    If Not (TypeOf row("amount11") Is DBNull) Then
                                        amount11 = amount11 + Decimal.Parse(row("amount11"))
                                    Else
                                        amount11 = amount11 + 0.0
                                    End If

                                    If Not (TypeOf row("amount12") Is DBNull) Then
                                        amount12 = amount12 + Decimal.Parse(row("amount12"))
                                    Else
                                        amount12 = amount12 + 0.0
                                    End If

                                End If

                            Next

                            arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno), amount4.ToString(decno), amount5.ToString(decno), amount6.ToString(decno), amount7.ToString(decno), amount8.ToString(decno), amount9.ToString(decno), amount10.ToString(decno), amount11.ToString(decno), amount12.ToString(decno)}
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
                            amount24 = amount4
                            amount25 = amount5
                            amount26 = amount6
                            amount27 = amount7
                            amount28 = amount8
                            amount29 = amount9
                            amount210 = amount10
                            amount211 = amount11
                            amount212 = amount12

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
                                amount4 = 0
                                amount5 = 0
                                amount6 = 0
                                amount7 = 0
                                amount8 = 0
                                amount9 = 0
                                amount10 = 0
                                amount11 = 0
                                amount12 = 0

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

                                        If Not (TypeOf row("amount4") Is DBNull) Then
                                            amount4 = amount4 + Decimal.Parse(row("amount4"))
                                        Else
                                            amount4 = amount4 + 0.0
                                        End If
                                        If Not (TypeOf row("amount5") Is DBNull) Then
                                            amount5 = amount5 + Decimal.Parse(row("amount5"))
                                        Else
                                            amount5 = amount5 + 0.0
                                        End If
                                        If Not (TypeOf row("amount6") Is DBNull) Then
                                            amount6 = amount6 + Decimal.Parse(row("amount6"))
                                        Else
                                            amount6 = amount6 + 0.0
                                        End If

                                        If Not (TypeOf row("amount7") Is DBNull) Then
                                            amount7 = amount7 + Decimal.Parse(row("amount7"))
                                        Else
                                            amount7 = amount7 + 0.0
                                        End If

                                        If Not (TypeOf row("amount8") Is DBNull) Then
                                            amount8 = amount8 + Decimal.Parse(row("amount8"))
                                        Else
                                            amount8 = amount8 + 0.0
                                        End If

                                        If Not (TypeOf row("amount9") Is DBNull) Then
                                            amount9 = amount9 + Decimal.Parse(row("amount9"))
                                        Else
                                            amount9 = amount9 + 0.0
                                        End If


                                        If Not (TypeOf row("amount10") Is DBNull) Then
                                            amount10 = amount10 + Decimal.Parse(row("amount10"))
                                        Else
                                            amount10 = amount10 + 0.0
                                        End If

                                        If Not (TypeOf row("amount11") Is DBNull) Then
                                            amount11 = amount11 + Decimal.Parse(row("amount11"))
                                        Else
                                            amount11 = amount11 + 0.0
                                        End If


                                        If Not (TypeOf row("amount12") Is DBNull) Then
                                            amount12 = amount12 + Decimal.Parse(row("amount12"))
                                        Else
                                            amount12 = amount12 + 0.0
                                        End If


                                    End If

                                Next

                                arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno), amount4.ToString(decno), amount5.ToString(decno), amount6.ToString(decno), amount7.ToString(decno), amount8.ToString(decno), amount9.ToString(decno), amount10.ToString(decno), amount11.ToString(decno), amount12.ToString(decno)}
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
                                    amount4 = 0
                                    amount5 = 0
                                    amount6 = 0
                                    amount7 = 0
                                    amount8 = 0
                                    amount9 = 0
                                    amount10 = 0
                                    amount11 = 0
                                    amount12 = 0

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

                                            If Not (TypeOf row("amount4") Is DBNull) Then
                                                amount4 = amount4 + Decimal.Parse(row("amount4"))
                                            Else
                                                amount4 = amount4 + 0.0
                                            End If
                                            If Not (TypeOf row("amount5") Is DBNull) Then
                                                amount5 = amount5 + Decimal.Parse(row("amount5"))
                                            Else
                                                amount5 = amount5 + 0.0
                                            End If
                                            If Not (TypeOf row("amount6") Is DBNull) Then
                                                amount6 = amount6 + Decimal.Parse(row("amount6"))
                                            Else
                                                amount6 = amount6 + 0.0
                                            End If

                                            If Not (TypeOf row("amount7") Is DBNull) Then
                                                amount7 = amount7 + Decimal.Parse(row("amount7"))
                                            Else
                                                amount7 = amount7 + 0.0
                                            End If

                                            If Not (TypeOf row("amount8") Is DBNull) Then
                                                amount8 = amount8 + Decimal.Parse(row("amount8"))
                                            Else
                                                amount8 = amount8 + 0.0
                                            End If

                                            If Not (TypeOf row("amount9") Is DBNull) Then
                                                amount9 = amount9 + Decimal.Parse(row("amount9"))
                                            Else
                                                amount9 = amount9 + 0.0
                                            End If

                                            If Not (TypeOf row("amount10") Is DBNull) Then
                                                amount10 = amount10 + Decimal.Parse(row("amount10"))
                                            Else
                                                amount10 = amount10 + 0.0
                                            End If

                                            If Not (TypeOf row("amount11") Is DBNull) Then
                                                amount11 = amount11 + Decimal.Parse(row("amount11"))
                                            Else
                                                amount11 = amount11 + 0.0
                                            End If

                                            If Not (TypeOf row("amount12") Is DBNull) Then
                                                amount12 = amount12 + Decimal.Parse(row("amount12"))
                                            Else
                                                amount12 = amount12 + 0.0
                                            End If

                                        End If

                                    Next

                                    arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno), amount4.ToString(decno), amount5.ToString(decno), amount6.ToString(decno), amount7.ToString(decno), amount8.ToString(decno), amount9.ToString(decno), amount10.ToString(decno), amount11.ToString(decno), amount12.ToString(decno)}
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
                                        amount4 = 0
                                        amount5 = 0
                                        amount6 = 0
                                        amount7 = 0
                                        amount8 = 0
                                        amount9 = 0
                                        amount10 = 0
                                        amount11 = 0
                                        amount12 = 0

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
                                                If Not (TypeOf row("amount4") Is DBNull) Then
                                                    amount4 = amount4 + Decimal.Parse(row("amount4"))
                                                Else
                                                    amount4 = amount4 + 0.0
                                                End If
                                                If Not (TypeOf row("amount5") Is DBNull) Then
                                                    amount5 = amount5 + Decimal.Parse(row("amount5"))
                                                Else
                                                    amount5 = amount5 + 0.0
                                                End If
                                                If Not (TypeOf row("amount6") Is DBNull) Then
                                                    amount6 = amount6 + Decimal.Parse(row("amount6"))
                                                Else
                                                    amount6 = amount6 + 0.0
                                                End If

                                                If Not (TypeOf row("amount7") Is DBNull) Then
                                                    amount7 = amount7 + Decimal.Parse(row("amount7"))
                                                Else
                                                    amount7 = amount7 + 0.0
                                                End If

                                                If Not (TypeOf row("amount8") Is DBNull) Then
                                                    amount8 = amount8 + Decimal.Parse(row("amount8"))
                                                Else
                                                    amount8 = amount8 + 0.0
                                                End If


                                                If Not (TypeOf row("amount9") Is DBNull) Then
                                                    amount9 = amount9 + Decimal.Parse(row("amount9"))
                                                Else
                                                    amount9 = amount9 + 0.0
                                                End If

                                                If Not (TypeOf row("amount10") Is DBNull) Then
                                                    amount10 = amount10 + Decimal.Parse(row("amount10"))
                                                Else
                                                    amount10 = amount10 + 0.0
                                                End If

                                                If Not (TypeOf row("amount11") Is DBNull) Then
                                                    amount11 = amount11 + Decimal.Parse(row("amount11"))
                                                Else
                                                    amount11 = amount11 + 0.0
                                                End If
                                                If Not (TypeOf row("amount12") Is DBNull) Then
                                                    amount12 = amount12 + Decimal.Parse(row("amount12"))
                                                Else
                                                    amount12 = amount12 + 0.0
                                                End If


                                            End If

                                        Next

                                        arrHeaders = {acct1name, amount.ToString(decno), amount1.ToString(decno), amount2.ToString(decno), amount3.ToString(decno), amount4.ToString(decno), amount5.ToString(decno), amount6.ToString(decno), amount7.ToString(decno), amount8.ToString(decno), amount9.ToString(decno), amount10.ToString(decno), amount11.ToString(decno), amount12.ToString(decno)}
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


                            arrHeaders = {acct2name, amount20.ToString(decno), amount21.ToString(decno), amount22.ToString(decno), amount23.ToString(decno), amount24.ToString(decno), amount25.ToString(decno), amount26.ToString(decno), amount27.ToString(decno), amount28.ToString(decno), amount29.ToString(decno), amount210.ToString(decno), amount211.ToString(decno), amount212.ToString(decno)}
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


            arrHeaders = {"NET PROFIT", (income - expanse - costofsales).ToString(decno), (income1 - expanse1 - costofsales1).ToString(decno), (income2 - expanse2 - costofsales2).ToString(decno), (income3 - expanse3 - costofsales3).ToString(decno), (income4 - expanse4 - costofsales4).ToString(decno), (income5 - expanse5 - costofsales5).ToString(decno), (income6 - expanse6 - costofsales6).ToString(decno), (income7 - expanse7 - costofsales7).ToString(decno), (income8 - expanse8 - costofsales8).ToString(decno), (income9 - expanse9 - costofsales9).ToString(decno), (income10 - expanse10 - costofsales10).ToString(decno), (income11 - expanse11 - costofsales11).ToString(decno), (income12 - expanse12 - costofsales12).ToString(decno)}
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
                    Dim Getexpanse = gacctLevel0.Compute("SUM(Amount)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse) Then
                        expanse = expanse + 0.0
                    Else
                        expanse = expanse + Convert.ToDecimal(Getexpanse)

                    End If

                    Dim Getexpanse1 = gacctLevel0.Compute("SUM(Amount1)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse1) Then
                        expanse1 = expanse1 + 0.0
                    Else
                        expanse1 = expanse1 + Convert.ToDecimal(Getexpanse1)

                    End If

                    Dim Getexpanse2 = gacctLevel0.Compute("SUM(Amount2)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse2) Then
                        expanse2 = expanse2 + 0.0
                    Else
                        expanse2 = expanse2 + Convert.ToDecimal(Getexpanse2)

                    End If

                    Dim Getexpanse3 = gacctLevel0.Compute("SUM(Amount3)", "GroupHeader='3' AND State>=1")
                    If IsDBNull(Getexpanse3) Then
                        expanse3 = expanse3 + 0.0
                    Else
                        expanse3 = expanse3 + Convert.ToDecimal(Getexpanse3)
                    End If


                    'Get the Sum of CostOfSales


                    Dim Getcostofsales = gacctLevel0.Compute("SUM(Amount)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getcostofsales) Then
                        costofsales = costofsales + 0.0
                    Else
                        costofsales = costofsales + Convert.ToDecimal(Getcostofsales)

                    End If

                    Dim Getcostofsales1 = gacctLevel0.Compute("SUM(Amount1)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getcostofsales1) Then
                        costofsales1 = costofsales1 + 0.0
                    Else
                        costofsales1 = costofsales1 + Convert.ToDecimal(Getcostofsales1)

                    End If

                    Dim Getcostofsales2 = gacctLevel0.Compute("SUM(Amount2)", "GroupHeader='2' AND State>=1")
                    If IsDBNull(Getcostofsales2) Then
                        costofsales2 = costofsales2 + 0.0
                    Else
                        costofsales2 = costofsales2 + Convert.ToDecimal(Getcostofsales2)

                    End If

                    Dim Getcostofsales3 = gacctLevel0.Compute("SUM(Amount3)", "GroupHeader='2' AND State>=1")
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
    'Ram 22082022
#Region "Generate Report New"
    Public Sub GenerateReportPLNew(ByVal todate As String, divcode As String, reportsType As String, pagetype As String, level As String, rptreportname As String, reportfilter As String, newformat As Integer, ByRef bytes() As Byte, ByVal printMode As String, Optional ByVal fileName As String = "")
        Dim dtPLSheet As New DataTable
        Dim dtPLSheetDate As New DataTable
        Dim ds As New DataSet

        If divcode <> "" Then
            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
        Else
            rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
        End If

        currency = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)

        decpt = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
        decno = "N" + decpt.ToString()

        Dim conn1 As New SqlConnection
        Dim sqlcmd As New SqlCommand
        Dim sqlAdp As SqlDataAdapter
        conn1 = clsDBConnect.dbConnectionnew("strDBConnection")
        sqlcmd = New SqlCommand("sp_profitloss_newformat", conn1)
        sqlcmd.CommandType = CommandType.StoredProcedure
        sqlcmd.Parameters.AddWithValue("@date", todate)
        sqlcmd.Parameters.AddWithValue("@division", divcode)
        sqlAdp = New SqlDataAdapter(sqlcmd)
        sqlAdp.SelectCommand.CommandTimeout = 180
        sqlAdp.Fill(ds)

        If ds IsNot Nothing Then
            If ds.Tables.Count > 0 Then
                dtPLSheet = ds.Tables(0)
                dtPLSheetDate = ds.Tables(1)
            End If
        End If

        If String.Equals(reportsType, "excel") Then
            ExcelReportPLNew(dtPLSheet, dtPLSheetDate, reportfilter, currency, rptreportname, bytes)
        End If
    End Sub
#End Region

#Region "ExcelReportPLNew"

    Public Sub ExcelReportPLNew(ByVal dtPLSheet As DataTable, ByVal dtPLSheetDate As DataTable, ByVal reportfilter As String, ByVal currency As String, ByVal rptreportname As String, ByRef bytes() As Byte)
        Dim arrHeaders() As String
        Dim wb As New XLWorkbook
        Dim decimalPoint1 As String = ""
        'sharfudeen 09-09-2022
        Dim positiveFormat As String = ""
        Dim negativeFormat As String = ""
        Dim zeroFormat As String = ""
        Dim numberFormat As String = ""

        Dim fullNumberFormat As String = ""

        Dim roundpositiveFormat As String = ""
        Dim roundnegativeFormat As String = ""
        Dim roundnumberFormat As String = ""
        'sharfudeen 09-09-2022
        If decpt = 2 Then
            decimalPoint = "#,##0.00"
            decimalPoint1 = "(#,##0.00)"
            'sharfudeen 07/09/2022
            positiveFormat = "#,##0.00_)"
            negativeFormat = "(#,##0.00)"
            zeroFormat = "-_)"
            numberFormat = positiveFormat + ";" + negativeFormat + ";@"


            fullNumberFormat = positiveFormat + ";" + negativeFormat + ";" + zeroFormat

            roundpositiveFormat = "#,##0_)"
            roundnegativeFormat = "(#,##0)"
            roundnumberFormat = roundpositiveFormat + ";" + roundnegativeFormat + ";@"
            'sharfudeen 07/09/2022

        ElseIf decpt = 3 Then
            decimalPoint = "#,##0.000"
            decimalPoint1 = "(#,##0.000)"

            'sharfudeen 07/09/2022
            positiveFormat = "#,##0.000_)"
            negativeFormat = "(#,##0.000)"
            zeroFormat = "-_)"
            numberFormat = positiveFormat + ";" + negativeFormat
            fullNumberFormat = positiveFormat + ";" + negativeFormat + ";" + zeroFormat

            'sharfudeen 07/09/2022
        ElseIf decpt = 4 Then
            decimalPoint = "#,##0.0000"
            decimalPoint1 = "(#,##0.0000)"
            'sharfudeen 07/09/2022
            positiveFormat = "#,##0.0000_)"
            negativeFormat = "(#,##0.0000)"
            zeroFormat = "-_)"
            numberFormat = positiveFormat + ";" + negativeFormat
            fullNumberFormat = positiveFormat + ";" + negativeFormat + ";" + zeroFormat
            'sharfudeen 07/09/2022

        End If

        Dim ws = wb.Worksheets.Add("P&L")
        ws.Columns.AdjustToContents()
        ws.Column("A").Width = 35
        ws.Column("B").Width = 20
        ws.Column("C").Width = 20
        Dim rownum As Integer = 7

        'Comapny Name Heading
        ws.Cell("A1").Value = rptcompanyname
        Dim company = ws.Range("A1:C1").Merge()
        company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
        company.Style.Font.SetBold().Font.FontSize = 15
        ' company.Style.Font.FontColor = XLColor.
        company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
        ws.Range("A1:C1").Merge()

        'Report Name Heading
        ws.Cell("A2").Value = rptreportname
        Dim report = ws.Range("A2:C2").Merge()
        report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
        report.Style.Font.SetBold().Font.FontSize = 14
        ' report.Style.Font.FontColor = XLColor.White
        report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
        ws.Range("A2:C2").Merge()

        Dim filter = ws.Range("A3:C3").Merge()
        filter.Style.Font.SetBold().Font.FontSize = 12
        filter.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontColor = XLColor.Black
        filter.Cell(1, 1).Value = reportfilter

        If dtPLSheetDate.Rows.Count > 0 Then

            ws.Cell("B5").Value = "PERIOD ENDING"
            Dim report1 = ws.Range("B5:C5").Merge()
            report1.Style.Font.SetBold().Font.FontSize = 10
            report1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            report1.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            ws.Range("B5:C5").Merge()

            ws.Cell("B6").Value = dtPLSheetDate.Rows(0)("Column1").ToString()
            ws.Cell("B6").Style.Font.SetBold().Font.FontSize = 10
            ws.Cell("B6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            ws.Cell("B6").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            ws.Cell("B7").Value = currency
            ws.Cell("B7").Style.Font.SetBold().Font.FontSize = 10
            ws.Cell("B7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            ws.Cell("B7").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            ws.Cell("C6").Value = dtPLSheetDate.Rows(0)("Column2").ToString()
            ws.Cell("C6").Style.Font.SetBold().Font.FontSize = 10
            ws.Cell("C6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            ws.Cell("C6").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            ws.Cell("C7").Value = currency
            ws.Cell("C7").Style.Font.SetBold().Font.FontSize = 10
            ws.Cell("C7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            ws.Cell("C7").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
        End If

        If dtPLSheet.Rows.Count > 0 Then
            Dim dtfilterLevel1 As New DataTable
            Dim dtfilterLevel2 As New DataTable
            Dim dtfilterLevel3 As New DataTable
            Dim dtfilterLevel4 As New DataTable
            Dim dtfilterLevel5 As New DataTable
            Dim CAlevel1sum As Decimal = 0
            Dim CAlevel2sum As Decimal = 0
            Dim PAlevel1sum As Decimal = 0
            Dim PAlevel2sum As Decimal = 0

            Dim distinctLevel1 As IEnumerable(Of Int32) = dtPLSheet.AsEnumerable().
                Select(Function(row) row.Field(Of Int32)("GroupHeader")).Distinct()

            For Each rowlevel1 In distinctLevel1

                dtfilterLevel1 = dtPLSheet.Select("GroupHeader=" + rowlevel1.ToString()).CopyToDataTable()

                For i = 0 To dtfilterLevel1.Rows.Count - 1

                    ws.Cell(rownum + 1, 1).Value = dtfilterLevel1.Rows(i)("AccName").ToString()
                    ws.Cell(rownum + 1, 1).Style.Font.FontSize = 10
                    ws.Cell(rownum + 1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

                    ws.Cell(rownum + 1, 2).Value = Decimal.Parse(Convert.ToDecimal(dtfilterLevel1.Rows(i)("CurrentAmount")))
                    ws.Cell(rownum + 1, 2).Style.NumberFormat.Format = roundnumberFormat ' decimalPoint 'sharfudeen 07/09/2022
                    'ws.Cell(rownum + 1, 2).Style.NumberFormat.Format = decimalPoint
                    ws.Cell(rownum + 1, 2).Style.Font.FontSize = 10
                    ws.Cell(rownum + 1, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                    ws.Cell(rownum + 1, 3).Value = Decimal.Parse(Convert.ToDecimal(dtfilterLevel1.Rows(i)("PreviousYearAmount")))
                    ws.Cell(rownum + 1, 3).Style.NumberFormat.Format = roundnumberFormat ' decimalPoint 'sharfudeen 07/09/2022
                    'ws.Cell(rownum + 1, 3).Style.NumberFormat.Format = decimalPoint
                    ws.Cell(rownum + 1, 3).Style.Font.FontSize = 10
                    ws.Cell(rownum + 1, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                    rownum += 1
                Next

                rownum += 1

            Next
        End If

        ws.Cell((rownum + 2), 1).Value = "Printed Date : " & Now.ToString("dd/MM/yyyy")
        ws.Range((rownum + 2), 1, (rownum + 2), 3).Merge()

        Using wStream As New MemoryStream()
            wb.SaveAs(wStream)
            bytes = wStream.ToArray()
        End Using

    End Sub

#End Region

End Class
