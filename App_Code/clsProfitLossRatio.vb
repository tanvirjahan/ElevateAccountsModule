Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Globalization
Imports System.Collections.Generic
Imports System.Linq
Imports ClosedXML.Excel


Public Class clsProfitLossRatio
    Inherits Web.UI.Page

#Region "global declaration"
    Dim objutils As New clsUtils

    Dim Level1font As Font = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK)
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
    Dim documentWidth As Single = 770.0F
    Dim CompanybgColor As BaseColor = New BaseColor(0, 72, 192)
    Dim ReportNamebgColor As BaseColor = New BaseColor(0, 128, 192)
    Dim CompanybgColor1 As BaseColor = New BaseColor(225, 225, 225)
    Dim ReportNamebgColor1 As BaseColor = New BaseColor(225, 225, 225)
    Dim View_pf As New DataTable
    Dim dtnettotal As New DataTable
    Dim acctgroupTable As New DataTable
    Dim view_actgroup As New DataTable
    Dim month1, month2, month3 As Integer
    Dim costofsales, costofsales1, costofsales2, costofsales3, costofsales4, costofsales5, costofsales6, income, income1, income2, income3, income4, income5, income6, expanse, expanse1, expanse2, expanse3, expanse4, expanse5, expanse6 As Decimal
    Dim amountperct, amountperct1, amountperct2, amountperct3, amountperct4, amountperct5, totPercent As Decimal
    Dim costofsalesperct, costofsalesperct1, costofsalesperct2, costofsalesperct3, costofsalesperct4, costofsalesperct5, costofsalesperct6, totPercentCost As Decimal
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
        cell.BorderColor = basecolor.WHITE
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 0.0F
        cell.PaddingTop = 0.0F
        Return cell
    End Function
#End Region

#Region "GenerateReportMonthRatiowise"

    Public Sub GenerateReportMonthRatiowise(ByVal reportsType As String, ByVal fromdate As String, ByVal todate As String, ByVal divcode As String, ByVal rpttype As String, ByVal type As String, ByVal closing As String, ByVal Ratio As String, ByVal strrpttype1 As String, ByRef bytes() As Byte, ByVal printMode As String, Optional ByVal fileName As String = "")
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
            mySqlCmd = New SqlCommand("sp_rep_profitloss_withratio", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0 'Tanvir 04052023

            mySqlCmd.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 20)).Value = divcode
            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@exchrate", SqlDbType.VarChar, 20)).Value = "3.6500000000"
            mySqlCmd.Parameters.Add(New SqlParameter("@level", SqlDbType.VarChar, 1)).Value = "D"
            mySqlCmd.Parameters.Add(New SqlParameter("@datetype", SqlDbType.VarChar, 1)).Value = "A"
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim custdetailsdt As New DataTable
            Dim view As DataView = ds.Tables(0).DefaultView
            view.RowFilter = " acc_code <> '00' "

            View_pf = view.ToTable()



            Dim netview As DataView = ds.Tables(0).DefaultView
            netview.RowFilter = " acc_code = '00' "


            dtnettotal = netview.ToTable()


            sqlquery = "SELECT acctgroup.acctname, acctgroup.div_code, acctgroup.parentid FROM  acctgroup where div_code='" & divcode & "'"

            Using ds1 As New SqlDataAdapter(sqlquery, sqlConn)
                ds1.Fill(acctgroupTable)
            End Using

            sqlquery = "SELECT * FROM  view_actgroup"

            Using ds2 As New SqlDataAdapter(sqlquery, sqlConn)
                ds2.Fill(view_actgroup)
            End Using

            Dim currentdate As Date = Convert.ToDateTime(fromdate)
            'Dim monthname1, monthname2, monthname3, monthname4
            'month1 = View_pf.Rows(0).Item("month1name")

            'month2 = View_pf.Rows(0).Item("month2name") '(currentdate.AddMonths(1)).Month
            'month3 = View_pf.Rows(0).Item("month3name") '(currentdate.AddMonths(2)).Month
            'month4 = View_pf.Rows(0).Item("month4name") '"YTD UPTO " + MonthName(month3)
            ''Dim month5, month6 As String
            'month5 = View_pf.Rows(0).Item("month5name") '(currentdate.AddMonths(2)).Month
            'month6 = View_pf.Rows(0).Item("month6name") '"YTD UPTO " + MonthName(month3)
            Dim noofmonths As Integer
            Dim todatedt As Date = Convert.ToDateTime(todate)
            noofmonths = DateDiff(DateInterval.Month, currentdate, todatedt) + 1
            If reportsType = "excel" Then
                ExcelReportMonthRatioWise(bytes, noofmonths)
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

                    arrHeaders = {"", View_pf.Rows(0).Item("month1name"), View_pf.Rows(0).Item("month2name"), View_pf.Rows(0).Item("month3name"), View_pf.Rows(0).Item("month4name"), View_pf.Rows(0).Item("month5name"), View_pf.Rows(0).Item("month6name")}
                    For i = 0 To arrHeaders.Length - 1
                        phrase = New Phrase()
                        phrase.Add(New Chunk(arrHeaders(i), Level3font))
                        cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                        cell.SetLeading(12, 0)
                        cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                        cell.PaddingBottom = 4.0F
                        cell.PaddingTop = 2.0F
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
                        Dim amount, amount1, amount2, amount3, amount4, amount5, amount20, amount21, amount22, amount23, amount24, amount25, amount26 As Decimal
                        Dim acct1name, acct2name As String


                        Dim dataView As New DataView(View_pf)
                        dataView.Sort = "AccName ASC"
                        gpbyName = dataView.ToTable()



                        'Group by Level0 and Head
                        Dim groups = From custledger In gpbyName.AsEnumerable() Group custledger By g = New With {Key .level0 = custledger.Field(Of Integer)("level0"), Key .div_code = custledger.Field(Of String)("div_id"), Key .Head = custledger.Field(Of Integer)("Head")} Into Group Order By g.Head

                        For Each groupby In groups

                            acctLevel = groupby.Group.CopyToDataTable
                            'Group by GroupHead
                            Dim gpbygroupHeader = From custledger In acctLevel.AsEnumerable() Group custledger By g = New With {Key .groupHeader = custledger.Field(Of Integer)("GroupHeader"), Key .div_code = custledger.Field(Of String)("div_id")} Into Group Order By g.div_code


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

#Region "ExcelReportMonthRatioWise"
    Public Sub ExcelReportMonthRatioWise(ByRef bytes() As Byte, ByRef noofmonths As Integer)



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
        ws.Column("A").Width = 40
        ws.Columns("B,D,F,H,J,L,N").Width = 17
        ws.Columns("C,E,G,I,K,M,O").Width = 10


        Dim n As Integer
        If noofmonths = 1 Then
            arrHeaders = {"", "Total", "%", "'" + View_pf.Rows(0).Item("month1name"), "%"}
            n = 5
        ElseIf noofmonths = 2 Then
            arrHeaders = {"", "Total", "%", "'" + View_pf.Rows(0).Item("month1name"), "%", "'" + View_pf.Rows(0).Item("month2name"), "%"}
            n = 7
        ElseIf noofmonths = 3 Then
            arrHeaders = {"", "Total", "%", "'" + View_pf.Rows(0).Item("month1name"), "%", "'" + View_pf.Rows(0).Item("month2name"), "%", "'" + View_pf.Rows(0).Item("month3name"), "%"}
            n = 9
        ElseIf noofmonths = 4 Then

            arrHeaders = {"", "Total", "%", "'" + View_pf.Rows(0).Item("month1name"), "%", "'" + View_pf.Rows(0).Item("month2name"), "%", "'" + View_pf.Rows(0).Item("month3name"), "%", "'" + View_pf.Rows(0).Item("month4name"), "%"}
            n = 11
        ElseIf noofmonths = 5 Then
            arrHeaders = {"", "Total", "%", "'" + View_pf.Rows(0).Item("month1name"), "%", "'" + View_pf.Rows(0).Item("month2name"), "%", "'" + View_pf.Rows(0).Item("month3name"), "%", "'" + View_pf.Rows(0).Item("month4name"), "%", "'" + View_pf.Rows(0).Item("month5name"), "%"}
            n = 13
        ElseIf noofmonths >= 6 Then
            arrHeaders = {"", "Total", "%", "'" + View_pf.Rows(0).Item("month1name"), "%", "'" + View_pf.Rows(0).Item("month2name"), "%", "'" + View_pf.Rows(0).Item("month3name"), "%", "'" + View_pf.Rows(0).Item("month4name"), "%", "'" + View_pf.Rows(0).Item("month5name"), "%", "'" + View_pf.Rows(0).Item("month6name"), "%"}
            n = 15
        End If
        'Comapny Name Heading
        ws.Cell("A2").Value = rptcompanyname
        Dim company = ws.Range(2, 1, 2, n).Merge()
        company.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromHtml("#0048C0"))
        company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
        company.Style.Font.FontSize = 15
        company.Style.Font.FontColor = XLColor.White
        company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center


        'Report Name Heading
        ws.Cell("A3").Value = rptreportname
      
        

        Dim report = ws.Range(3, 1, 3, n).Merge()
        report.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromHtml("#0080C0"))
        report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
        report.Style.Font.FontSize = 14
        report.Style.Font.FontColor = XLColor.White
        report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
        'report.Style.Alignment.Vertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)



        ''Report Filterdt
        'Dim filter = ws.Range("B6:I6")
        'filter.Style.Font.SetBold().Font.FontSize = 14
        'filter.Style.Font.FontColor = XLColor.Black
        'filter.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
        'filter.Cell(1, 1).Value = reportfilter


        'arrHeaders = {"", MonthName(month1), MonthName(month2), MonthName(month3), month4}

        ' arrHeaders = {"", month1, month2, month3, month4}
        

        ws.Range(5, 1, 5, n).Style.Font.SetBold.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Border.SetTopBorder(XLBorderStyleValues.Thin).Font.FontSize = 12
        ws.Range(5, 1, 5, n).Style.Alignment.WrapText = True
        ws.Range(5, 1, 5, n).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
        For i = 0 To arrHeaders.Length - 1
            ws.Cell(5, i + 1).Value = arrHeaders(i)
        Next

        If View_pf.Rows.Count > 0 Then
            Dim acctLevel, gacctLevel0, gacctLevel1, gacctLevel2, gacctLevel3, gacctLevel4, gpbyName As New DataTable
            '  Dim acctgroup1 As New DataTable
            Dim totAmt, amount, amount1, amount2, amount3, amount4, amount5, totAmtCost, amount20, amount21, amount22, amount23, amount24, amount25 As Decimal
            Dim acct1name, acct2name As String


            Dim dataView As New DataView(View_pf)
            dataView.Sort = "mainlevel ASC, grouplevel ASC, Acc_name ASC"
            gpbyName = dataView.ToTable()

            'Group by Level0 and Head
            Dim groups = From custledger In gpbyName.AsEnumerable() Group custledger By g = New With {Key .level0 = custledger.Field(Of Integer)("level0"), Key .div_id = custledger.Field(Of String)("div_id"), Key .Head = custledger.Field(Of Integer)("Head")} Into Group Order By g.Head

            For Each groupby In groups

                acctLevel = groupby.Group.CopyToDataTable
                'Group by GroupHead
                Dim gpbygroupHeader = From custledger In acctLevel.AsEnumerable() Group custledger By g = New With {Key .groupHeader = custledger.Field(Of Integer)("GroupHeader"), Key .div_id = custledger.Field(Of String)("div_id")} Into Group Order By g.groupHeader

                For Each gpbyHeader In gpbygroupHeader

                    'Level1
                    gacctLevel0 = gpbyHeader.Group.CopyToDataTable

                    ' Get the Sum of Income
                    If noofmonths >= 1 Then
                        Dim Getincome = gacctLevel0.Compute("SUM(month1)", "GroupHeader='1' AND State>=1") 'Amount 
                        If IsDBNull(Getincome) Then
                            income = income + 0.0
                        Else
                            income = income + Convert.ToDecimal(Getincome)

                        End If

                        Dim Getexpanse = gacctLevel0.Compute("SUM(month1)", "GroupHeader='2' AND State>=1") 'Amount
                        If IsDBNull(Getexpanse) Then
                            expanse = expanse + 0.0
                        Else
                            expanse = expanse + Convert.ToDecimal(Getexpanse)

                        End If

                        Dim Getcostofsales = gacctLevel0.Compute("SUM(month1)", "GroupHeader='3' AND State>=1")
                        If IsDBNull(Getcostofsales) Then
                            costofsales = costofsales + 0.0
                        Else
                            costofsales = costofsales + Convert.ToDecimal(Getcostofsales)

                        End If


                        Dim Getcostofsalesper = gacctLevel0.Compute("SUM(month1perc)", "GroupHeader='3' AND State>=1")
                        If IsDBNull(Getcostofsalesper) Then
                            costofsalesperct = costofsalesperct + 0.0
                        Else
                            costofsalesperct = costofsalesperct + Convert.ToDecimal(Getcostofsales)

                        End If
                    End If
                    If noofmonths >= 2 Then
                        Dim Getincome1 = gacctLevel0.Compute("SUM(month2)", "GroupHeader='1' AND State>=1") 'Amount1
                        If IsDBNull(Getincome1) Then
                            income1 = income1 + 0.0
                        Else
                            income1 = income1 + Convert.ToDecimal(Getincome1)

                        End If

                        Dim Getexpanse1 = gacctLevel0.Compute("SUM(month2)", "GroupHeader='2' AND State>=1") 'Amount1
                        If IsDBNull(Getexpanse1) Then
                            expanse1 = expanse1 + 0.0
                        Else
                            expanse1 = expanse1 + Convert.ToDecimal(Getexpanse1)

                        End If


                        Dim Getcostofsales1 = gacctLevel0.Compute("SUM(month2)", "GroupHeader='3' AND State>=1")
                        If IsDBNull(Getcostofsales1) Then
                            costofsales1 = costofsales1 + 0.0
                        Else
                            costofsales1 = costofsales1 + Convert.ToDecimal(Getcostofsales1)

                        End If

                        Dim Getcostofsalesper1 = gacctLevel0.Compute("SUM(month2perc)", "GroupHeader='3' AND State>=1")
                        If IsDBNull(Getcostofsalesper1) Then
                            costofsalesperct1 = costofsalesperct1 + 0.0
                        Else
                            costofsalesperct1 = costofsalesperct1 + Convert.ToDecimal(Getcostofsalesper1)

                        End If
                    End If

                    If noofmonths >= 3 Then
                        Dim Getincome2 = gacctLevel0.Compute("SUM(month3)", "GroupHeader='1' AND State>=1") 'Amount2
                        If IsDBNull(Getincome2) Then
                            income2 = income2 + 0.0
                        Else
                            income2 = income2 + Convert.ToDecimal(Getincome2)

                        End If


                        Dim Getexpanse2 = gacctLevel0.Compute("SUM(month3)", "GroupHeader='2' AND State>=1") 'Amount2
                        If IsDBNull(Getexpanse2) Then
                            expanse2 = expanse2 + 0.0
                        Else
                            expanse2 = expanse2 + Convert.ToDecimal(Getexpanse2)

                        End If



                        Dim Getcostofsales2 = gacctLevel0.Compute("SUM(month3)", "GroupHeader='3' AND State>=1")
                        If IsDBNull(Getcostofsales2) Then
                            costofsales2 = costofsales2 + 0.0
                        Else
                            costofsales2 = costofsales2 + Convert.ToDecimal(Getcostofsales2)

                        End If

                        Dim Getcostofsalesper2 = gacctLevel0.Compute("SUM(month3perc)", "GroupHeader='3' AND State>=1")
                        If IsDBNull(Getcostofsalesper2) Then
                            costofsalesperct2 = costofsalesperct2 + 0.0
                        Else
                            costofsalesperct2 = costofsalesperct2 + Convert.ToDecimal(Getcostofsalesper2)

                        End If

                    End If
                    If noofmonths >= 4 Then
                        Dim Getincome3 = gacctLevel0.Compute("SUM(month4)", "GroupHeader='1' AND State>=1") 'Amount3
                        If IsDBNull(Getincome3) Then
                            income3 = income3 + 0.0
                        Else
                            income3 = income3 + Convert.ToDecimal(Getincome3)
                        End If

                        Dim Getexpanse3 = gacctLevel0.Compute("SUM(month4)", "GroupHeader='2' AND State>=1") 'Amount3
                        If IsDBNull(Getexpanse3) Then
                            expanse3 = expanse3 + 0.0
                        Else
                            expanse3 = expanse3 + Convert.ToDecimal(Getexpanse3)
                        End If
                        Dim Getcostofsales3 = gacctLevel0.Compute("SUM(month4)", "GroupHeader='3' AND State>=1")
                        If IsDBNull(Getcostofsales3) Then
                            costofsales3 = costofsales3 + 0.0
                        Else
                            costofsales3 = costofsales3 + Convert.ToDecimal(Getcostofsales3)

                        End If


                        Dim Getcostofsalesper3 = gacctLevel0.Compute("SUM(month4perc)", "GroupHeader='3' AND State>=1")
                        If IsDBNull(Getcostofsalesper3) Then
                            costofsalesperct3 = costofsalesperct3 + 0.0
                        Else
                            costofsalesperct3 = costofsalesperct3 + Convert.ToDecimal(Getcostofsalesper3)

                        End If
                    End If
                    If noofmonths >= 5 Then
                        Dim Getincome4 = gacctLevel0.Compute("SUM(month5)", "GroupHeader='1' AND State>=1") 'Amount3
                        If IsDBNull(Getincome4) Then
                            income4 = income4 + 0.0
                        Else
                            income4 = income4 + Convert.ToDecimal(Getincome4)
                        End If


                        Dim Getexpanse4 = gacctLevel0.Compute("SUM(month5)", "GroupHeader='2' AND State>=1") 'Amount2
                        If IsDBNull(Getexpanse4) Then
                            expanse4 = expanse4 + 0.0
                        Else
                            expanse4 = expanse4 + Convert.ToDecimal(Getexpanse4)

                        End If


                        Dim Getcostofsales4 = gacctLevel0.Compute("SUM(month5)", "GroupHeader='3' AND State>=1")
                        If IsDBNull(Getcostofsales4) Then
                            costofsales4 = costofsales4 + 0.0
                        Else
                            costofsales4 = costofsales4 + Convert.ToDecimal(Getcostofsales4)

                        End If

                        Dim Getcostofsalesper4 = gacctLevel0.Compute("SUM(month5perc)", "GroupHeader='3' AND State>=1")
                        If IsDBNull(Getcostofsalesper4) Then
                            costofsalesperct4 = costofsalesperct4 + 0.0
                        Else
                            costofsalesperct4 = costofsalesperct4 + Convert.ToDecimal(Getcostofsalesper4)

                        End If
                    End If
                    If noofmonths >= 6 Then
                        Dim Getincome5 = gacctLevel0.Compute("SUM(month6)", "GroupHeader='1' AND State>=1") 'Amount3
                        If IsDBNull(Getincome5) Then
                            income5 = income5 + 0.0
                        Else
                            income5 = income5 + Convert.ToDecimal(Getincome5)
                        End If

                        Dim Getexpanse5 = gacctLevel0.Compute("SUM(month6)", "GroupHeader='2' AND State>=1") 'Amount3
                        If IsDBNull(Getexpanse5) Then
                            expanse5 = expanse5 + 0.0
                        Else
                            expanse5 = expanse5 + Convert.ToDecimal(Getexpanse5)
                        End If


                        Dim Getcostofsales5 = gacctLevel0.Compute("SUM(month6)", "GroupHeader='3' AND State>=1")
                        If IsDBNull(Getcostofsales5) Then
                            costofsales5 = costofsales5 + 0.0
                        Else
                            costofsales5 = costofsales5 + Convert.ToDecimal(Getcostofsales5)

                        End If

                        Dim Getcostofsalesper5 = gacctLevel0.Compute("SUM(month6)", "GroupHeader='3' AND State>=1")
                        If IsDBNull(Getcostofsalesper5) Then
                            costofsalesperct5 = costofsalesperct5 + 0.0
                        Else
                            costofsalesperct5 = costofsalesperct5 + Convert.ToDecimal(Getcostofsalesper5)

                        End If
                    End If
                    'Get The Sum of Expanses   


                    'Get the Sum of CostOfSales












                    'COst of sales percent










                    'level 1 Group By
                    Dim gpbyLevel1 = From custledger In gacctLevel0.AsEnumerable() Group custledger By g = New With {Key .level1 = custledger.Field(Of Integer)("level1"), Key .div_id = custledger.Field(Of String)("div_id")} Into Group Order By g.div_id

                    For Each gpby1 In gpbyLevel1
                        Dim tmplevel As String = gpby1.g.level1
                        Dim tmpdivid As String = gpby1.g.div_id
                        Dim acctgroup1 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname1 = a.Field(Of String)("acctname"), .div_code1 = a.Field(Of String)("div_code"), .parentid1 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid1 = tmplevel And a.div_code1 = tmpdivid).OrderBy(Function(o) o.parentid1)
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
                        amountperct = 0
                        amountperct1 = 0
                        amountperct2 = 0
                        amountperct3 = 0
                        amountperct4 = 0
                        amountperct5 = 0
                        acct1name = ""
                        If acctgroup1.Count > 0 Then
                            For Each acct1row In acctgroup1
                                acct1name = acct1row.acctname1
                            Next
                        End If

                        For Each row In gpby1.Group
                            If row("state") >= 1 Then
                                If noofmonths >= 1 Then
                                    If Not (TypeOf row("month1") Is DBNull) Then
                                        amount = amount + Decimal.Parse(row("month1"))
                                    Else
                                        amount = amount + 0.0
                                    End If
                                    'For Monthpercent
                                    If Not (TypeOf row("month1perc") Is DBNull) Then
                                        amountperct = amountperct + Decimal.Parse(row("month1perc"))
                                    Else
                                        amountperct = amountperct + 0.0
                                    End If

                                End If
                                If noofmonths >= 2 Then
                                    If Not (TypeOf row("month2") Is DBNull) Then
                                        amount1 = amount1 + Decimal.Parse(row("month2"))
                                    Else
                                        amount1 = amount1 + 0.0
                                    End If
                                    'For Monthpercent

                                    If Not (TypeOf row("month2perc") Is DBNull) Then
                                        amountperct1 = amountperct1 + Decimal.Parse(row("month2perc"))
                                    Else
                                        amountperct1 = amountperct1 + 0.0
                                    End If


                                End If
                                If noofmonths >= 3 Then
                                    If Not (TypeOf row("month3") Is DBNull) Then
                                        amount2 = amount2 + Decimal.Parse(row("month3"))
                                    Else
                                        amount2 = amount2 + 0.0
                                    End If
                                    'For Monthpercent
                                    If Not (TypeOf row("month3perc") Is DBNull) Then
                                        amountperct2 = amountperct2 + Decimal.Parse(row("month3perc"))
                                    Else
                                        amountperct2 = amountperct2 + 0.0
                                    End If


                                End If
                                If noofmonths >= 4 Then
                                    If Not (TypeOf row("month4") Is DBNull) Then
                                        amount3 = amount3 + Decimal.Parse(row("month4"))
                                    Else
                                        amount3 = amount3 + 0.0
                                    End If
                                    'For Monthpercent
                                    If Not (TypeOf row("month4perc") Is DBNull) Then
                                        amountperct3 = amountperct3 + Decimal.Parse(row("month4perc"))
                                    Else
                                        amountperct3 = amountperct3 + 0.0
                                    End If


                                End If
                                If noofmonths >= 5 Then
                                    If Not (TypeOf row("month5") Is DBNull) Then
                                        amount4 = amount4 + Decimal.Parse(row("month5"))
                                    Else
                                        amount4 = amount4 + 0.0
                                    End If
                                    'For Monthpercent
                                    If Not (TypeOf row("month5perc") Is DBNull) Then
                                        amountperct4 = amountperct4 + Decimal.Parse(row("month5perc"))
                                    Else
                                        amountperct4 = amountperct4 + 0.0
                                    End If
                                End If
                                If noofmonths >= 6 Then
                                    If Not (TypeOf row("month6") Is DBNull) Then
                                        amount5 = amount5 + Decimal.Parse(row("month6"))
                                    Else
                                        amount5 = amount5 + 0.0
                                    End If
                                    'For Monthpercent

                                    If Not (TypeOf row("month6perc") Is DBNull) Then
                                        amountperct5 = amountperct5 + Decimal.Parse(row("month6perc"))
                                    Else
                                        amountperct5 = amountperct5 + 0.0
                                    End If

                                End If

                            End If
                        Next
                        If noofmonths = 1 Then
                            totAmt = Val(amount)
                            totPercent = Val(amountperct)
                            arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno)}
                        ElseIf noofmonths = 2 Then
                            totAmt = Val(amount) + Val(amount1)
                            totPercent = Val(amountperct) + Val(amountperct1)
                            arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno)}

                        ElseIf noofmonths = 3 Then
                            totAmt = Val(amount) + Val(amount1) + Val(amount2)
                            totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2)
                            arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno)}
                        ElseIf noofmonths = 4 Then


                            totAmt = Val(amount) + Val(amount1) + Val(amount2) + Val(amount3)
                            totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2) + Val(amountperct3)
                            arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno), amount3.ToString(decno), amountperct3.ToString(decno)}
                        ElseIf noofmonths = 5 Then
                            totAmt = Val(amount) + Val(amount1) + Val(amount2) + Val(amount3) + Val(amount4)
                            totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2) + Val(amountperct3) + Val(amountperct4)
                            arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno), amount3.ToString(decno), amountperct3.ToString(decno), amount4.ToString(decno), amountperct4.ToString(decno)}

                        ElseIf noofmonths >= 6 Then

                            totAmt = Val(amount) + Val(amount1) + Val(amount2) + Val(amount3) + Val(amount4) + Val(amount5)
                            totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2) + Val(amountperct3) + Val(amountperct4) + Val(amountperct5)
                            arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno), amount3.ToString(decno), amountperct3.ToString(decno), amount4.ToString(decno), amountperct4.ToString(decno), amount5.ToString(decno), amountperct5.ToString(decno)}

                        End If

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
                        Dim acctLevel2 = From row1 In gacctLevel1.AsEnumerable() Group row1 By g1 = New With {Key .level2 = row1.Field(Of Integer)("level2"), Key .divcode2 = row1.Field(Of String)("div_id")} Into Group Order By g1.divcode2

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
                            amountperct = 0
                            amountperct1 = 0
                            amountperct2 = 0
                            amountperct3 = 0
                            amountperct4 = 0
                            amountperct5 = 0
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
                                    If noofmonths >= 1 Then
                                        If Not (TypeOf row("month1") Is DBNull) Then
                                            amount = amount + Decimal.Parse(row("month1"))
                                        Else
                                            amount = amount + 0.0
                                        End If
                                        'For Monthpercent

                                        If Not (TypeOf row("month1perc") Is DBNull) Then
                                            amountperct = amountperct + Decimal.Parse(row("month1perc"))
                                        Else
                                            amountperct = amountperct + 0.0
                                        End If


                                    End If
                                    If noofmonths >= 2 Then
                                        If Not (TypeOf row("month2") Is DBNull) Then
                                            amount1 = amount1 + Decimal.Parse(row("month2"))
                                        Else
                                            amount1 = amount1 + 0.0
                                        End If

                                        'For Monthpercent
                                        If Not (TypeOf row("month2perc") Is DBNull) Then
                                            amountperct1 = amountperct1 + Decimal.Parse(row("month2perc"))
                                        Else
                                            amountperct1 = amountperct1 + 0.0
                                        End If

                                    End If
                                    If noofmonths >= 3 Then
                                        If Not (TypeOf row("month3") Is DBNull) Then
                                            amount2 = amount2 + Decimal.Parse(row("month3"))
                                        Else
                                            amount2 = amount2 + 0.0
                                        End If

                                        'For Monthpercent
                                        If Not (TypeOf row("month3perc") Is DBNull) Then
                                            amountperct2 = amountperct2 + Decimal.Parse(row("month3perc"))
                                        Else
                                            amountperct2 = amountperct2 + 0.0
                                        End If
                                    End If

                                    If noofmonths >= 4 Then
                                        If Not (TypeOf row("month4") Is DBNull) Then
                                            amount3 = amount3 + Decimal.Parse(row("month4"))
                                        Else
                                            amount3 = amount3 + 0.0
                                        End If
                                        'For Monthpercent

                                        If Not (TypeOf row("month4perc") Is DBNull) Then
                                            amountperct3 = amountperct3 + Decimal.Parse(row("month4perc"))
                                        Else
                                            amountperct3 = amountperct3 + 0.0
                                        End If
                                    End If

                                    If noofmonths >= 5 Then
                                        If Not (TypeOf row("month5") Is DBNull) Then
                                            amount4 = amount4 + Decimal.Parse(row("month5"))
                                        Else
                                            amount4 = amount4 + 0.0
                                        End If
                                        'For Monthpercent
                                        If Not (TypeOf row("month5perc") Is DBNull) Then
                                            amountperct4 = amountperct4 + Decimal.Parse(row("month5perc"))
                                        Else
                                            amountperct4 = amountperct4 + 0.0
                                        End If

                                    End If
                                    If noofmonths >= 6 Then
                                        If Not (TypeOf row("month6") Is DBNull) Then
                                            amount5 = amount5 + Decimal.Parse(row("month6"))
                                        Else
                                            amount5 = amount5 + 0.0
                                        End If

                                        'For Monthpercent
                                        If Not (TypeOf row("month6perc") Is DBNull) Then
                                            amountperct5 = amountperct5 + Decimal.Parse(row("month6perc"))
                                        Else
                                            amountperct5 = amountperct5 + 0.0
                                        End If


                                    End If

                                End If

                            Next

                            If noofmonths = 1 Then
                                totAmt = Val(amount)
                                totPercent = Val(amountperct)
                                arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno)}
                            ElseIf noofmonths = 2 Then
                                totAmt = Val(amount) + Val(amount1)
                                totPercent = Val(amountperct) + Val(amountperct1)
                                arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno)}
                            ElseIf noofmonths = 3 Then
                                totAmt = Val(amount) + Val(amount1) + Val(amount2)
                                totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2)
                                arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno)}
                            ElseIf noofmonths = 4 Then
                                totAmt = Val(amount) + Val(amount1) + Val(amount2) + Val(amount3) + Val(amount4)
                                totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2) + Val(amountperct3) + Val(amountperct4)
                                arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno), amount3.ToString(decno), amountperct3.ToString(decno), amount4.ToString(decno), amountperct4.ToString(decno)}
                            ElseIf noofmonths = 5 Then


                                totAmt = Val(amount) + Val(amount1) + Val(amount2) + Val(amount3)
                                totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2) + Val(amountperct3)
                                arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno), amount3.ToString(decno), amountperct3.ToString(decno)}
                            ElseIf noofmonths >= 6 Then
                                totAmt = Val(amount) + Val(amount1) + Val(amount2) + Val(amount3) + Val(amount4) + Val(amount5)
                                totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2) + Val(amountperct3) + Val(amountperct4) + Val(amountperct5)
                                arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno), amount3.ToString(decno), amountperct3.ToString(decno), amount4.ToString(decno), amountperct4.ToString(decno), amount5.ToString(decno), amountperct5.ToString(decno)}

                            End If
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
                            ' Level3
                            Dim acctLevel3 = From row1 In gacctLevel2.AsEnumerable() Group row1 By g1 = New With {Key .level3 = row1.Field(Of Integer)("level3"), Key .divcode3 = row1.Field(Of String)("div_id")} Into Group Order By g1.divcode3
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
                                amountperct = 0
                                amountperct1 = 0
                                amountperct2 = 0
                                amountperct3 = 0
                                amountperct4 = 0
                                amountperct5 = 0
                                acct1name = ""
                                If acctgroup3.Count > 0 Then

                                    For Each acct3row In acctgroup3
                                        acct1name = acct3row.acctname3
                                    Next

                                End If
                                For Each row In gpbylevel3.Group
                                    If row("state") >= 1 Then    '3 
                                        If noofmonths >= 1 Then
                                            If Not (TypeOf row("month1") Is DBNull) Then
                                                amount = amount + Decimal.Parse(row("month1"))
                                            Else
                                                amount = amount + 0.0
                                            End If

                                            If Not (TypeOf row("month1perc") Is DBNull) Then
                                                amountperct = amountperct + Decimal.Parse(row("month1perc"))
                                            Else
                                                amountperct = amountperct + 0.0
                                            End If
                                        End If
                                        If noofmonths >= 2 Then
                                            If Not (TypeOf row("month2") Is DBNull) Then
                                                amount1 = amount1 + Decimal.Parse(row("month2"))
                                            Else
                                                amount1 = amount1 + 0.0
                                            End If

                                            If Not (TypeOf row("month2perc") Is DBNull) Then
                                                amountperct1 = amountperct1 + Decimal.Parse(row("month2perc"))
                                            Else
                                                amountperct1 = amountperct1 + 0.0
                                            End If
                                        End If
                                        If noofmonths >= 3 Then
                                            If Not (TypeOf row("month3") Is DBNull) Then
                                                amount2 = amount2 + Decimal.Parse(row("month3"))
                                            Else
                                                amount2 = amount2 + 0.0
                                            End If


                                            If Not (TypeOf row("month3perc") Is DBNull) Then
                                                amountperct2 = amountperct2 + Decimal.Parse(row("month3perc"))
                                            Else
                                                amountperct2 = amountperct2 + 0.0
                                            End If

                                        End If
                                        If noofmonths >= 4 Then
                                            If Not (TypeOf row("month4") Is DBNull) Then
                                                amount3 = amount3 + Decimal.Parse(row("month4"))
                                            Else
                                                amount3 = amount3 + 0.0
                                            End If

                                            If Not (TypeOf row("month4perc") Is DBNull) Then
                                                amountperct3 = amountperct3 + Decimal.Parse(row("month4perc"))
                                            Else
                                                amountperct3 = amountperct3 + 0.0
                                            End If
                                        End If
                                        If noofmonths >= 5 Then
                                            If Not (TypeOf row("month5") Is DBNull) Then
                                                amount4 = amount4 + Decimal.Parse(row("month5"))
                                            Else
                                                amount4 = amount4 + 0.0
                                            End If
                                            If Not (TypeOf row("month5perc") Is DBNull) Then
                                                amountperct4 = amountperct4 + Decimal.Parse(row("month5perc"))
                                            Else
                                                amountperct4 = amountperct4 + 0.0
                                            End If

                                        End If
                                        If noofmonths >= 6 Then
                                            If Not (TypeOf row("month6") Is DBNull) Then
                                                amount5 = amount5 + Decimal.Parse(row("month6"))
                                            Else
                                                amount5 = amount5 + 0.0
                                            End If

                                            If Not (TypeOf row("month6perc") Is DBNull) Then
                                                amountperct5 = amountperct5 + Decimal.Parse(row("month6perc"))
                                            Else
                                                amountperct5 = amountperct5 + 0.0
                                            End If

                                        End If
                                        'For Monthpercent








                                    End If

                                Next

                                If noofmonths = 1 Then
                                    totAmt = Val(amount)
                                    totPercent = Val(amountperct)
                                    arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno)}
                                ElseIf noofmonths = 2 Then
                                    totAmt = Val(amount) + Val(amount1)
                                    totPercent = Val(amountperct) + Val(amountperct1)
                                    arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno)}

                                ElseIf noofmonths = 3 Then

                                    totAmt = Val(amount) + Val(amount1) + Val(amount2)
                                    totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2)
                                    arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno)}

                                ElseIf noofmonths = 4 Then
                                    totAmt = Val(amount) + Val(amount1) + Val(amount2) + Val(amount3)
                                    totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2) + Val(amountperct3)
                                    arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno), amount3.ToString(decno), amountperct3.ToString(decno)}
                                ElseIf noofmonths = 5 Then
                                    totAmt = Val(amount) + Val(amount1) + Val(amount2) + Val(amount3) + Val(amount4)
                                    totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2) + Val(amountperct3) + Val(amountperct4)
                                    arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno), amount3.ToString(decno), amountperct3.ToString(decno), amount4.ToString(decno), amountperct4.ToString(decno)}
                                ElseIf noofmonths >= 6 Then
                                    totAmt = Val(amount) + Val(amount1) + Val(amount2) + Val(amount3) + Val(amount4) + Val(amount5)
                                    totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2) + Val(amountperct3) + Val(amountperct4) + Val(amountperct5)
                                    arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno), amount3.ToString(decno), amountperct3.ToString(decno), amount4.ToString(decno), amountperct4.ToString(decno), amount5.ToString(decno), amountperct5.ToString(decno)}
                                End If

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
                                Dim acctLevel4 = From row1 In gacctLevel3.AsEnumerable() Group row1 By g1 = New With {Key .level4 = row1.Field(Of Integer)("level4"), Key .divcode4 = row1.Field(Of String)("div_id")} Into Group Order By g1.divcode4

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
                                    amountperct = 0
                                    amountperct1 = 0
                                    amountperct2 = 0
                                    amountperct3 = 0
                                    amountperct4 = 0
                                    amountperct5 = 0
                                    acct1name = ""
                                    If acctgroup4.Count > 0 Then

                                        For Each acct4row In acctgroup4
                                            acct1name = acct4row.acctname4
                                        Next
                                    End If
                                    For Each row In gpbylevel4.Group
                                        If row("state") >= 1 Then   '4
                                            If noofmonths >= 1 Then
                                                If Not (TypeOf row("month1") Is DBNull) Then
                                                    amount = amount + Decimal.Parse(row("month1"))
                                                Else
                                                    amount = amount + 0.0
                                                End If

                                                'For Monthpercent
                                                If Not (TypeOf row("month1perc") Is DBNull) Then
                                                    amountperct = amountperct + Decimal.Parse(row("month1perc"))
                                                Else
                                                    amountperct = amountperct + 0.0
                                                End If

                                            End If
                                            If noofmonths >= 2 Then
                                                If Not (TypeOf row("month2") Is DBNull) Then
                                                    amount1 = amount1 + Decimal.Parse(row("month2"))
                                                Else
                                                    amount1 = amount1 + 0.0
                                                End If

                                                If Not (TypeOf row("month2perc") Is DBNull) Then
                                                    amountperct1 = amountperct1 + Decimal.Parse(row("month2perc"))
                                                Else
                                                    amountperct1 = amountperct1 + 0.0
                                                End If


                                            End If
                                            If noofmonths >= 3 Then
                                                If Not (TypeOf row("month3") Is DBNull) Then
                                                    amount2 = amount2 + Decimal.Parse(row("month3"))
                                                Else
                                                    amount2 = amount2 + 0.0
                                                End If

                                                If Not (TypeOf row("month3perc") Is DBNull) Then
                                                    amountperct2 = amountperct2 + Decimal.Parse(row("month3perc"))
                                                Else
                                                    amountperct2 = amountperct2 + 0.0
                                                End If


                                            End If
                                            If noofmonths >= 4 Then
                                                If Not (TypeOf row("month4") Is DBNull) Then
                                                    amount3 = amount3 + Decimal.Parse(row("month4"))
                                                Else
                                                    amount3 = amount3 + 0.0
                                                End If

                                                If Not (TypeOf row("month4perc") Is DBNull) Then
                                                    amountperct3 = amountperct3 + Decimal.Parse(row("month4perc"))
                                                Else
                                                    amountperct3 = amountperct3 + 0.0
                                                End If


                                            End If
                                            If noofmonths >= 5 Then
                                                If Not (TypeOf row("month5") Is DBNull) Then
                                                    amount4 = amount4 + Decimal.Parse(row("month5"))
                                                Else
                                                    amount4 = amount4 + 0.0
                                                End If

                                                If Not (TypeOf row("month5perc") Is DBNull) Then
                                                    amountperct4 = amountperct4 + Decimal.Parse(row("month5perc"))
                                                Else
                                                    amountperct4 = amountperct4 + 0.0
                                                End If


                                            End If
                                            If noofmonths >= 6 Then
                                                If Not (TypeOf row("month6") Is DBNull) Then
                                                    amount5 = amount5 + Decimal.Parse(row("month6"))
                                                Else
                                                    amount5 = amount5 + 0.0
                                                End If


                                                If Not (TypeOf row("month6perc") Is DBNull) Then
                                                    amountperct5 = amountperct5 + Decimal.Parse(row("month6perc"))
                                                Else
                                                    amountperct5 = amountperct5 + 0.0
                                                End If

                                            End If








                                        End If

                                    Next

                                    If noofmonths = 1 Then
                                        totAmt = Val(amount)
                                        totPercent = Val(amountperct)
                                        arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno)}

                                    ElseIf noofmonths = 2 Then
                                        totAmt = Val(amount) + Val(amount1)
                                        totPercent = Val(amountperct) + Val(amountperct1)
                                        arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno)}
                                    ElseIf noofmonths = 3 Then
                                        totAmt = Val(amount) + Val(amount1) + Val(amount2)
                                        totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2)
                                        arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno)}
                                    ElseIf noofmonths = 4 Then

                                        totAmt = Val(amount) + Val(amount1) + Val(amount2) + Val(amount3)
                                        totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2) + Val(amountperct3)
                                        arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno), amount3.ToString(decno), amountperct3.ToString(decno)}


                                    ElseIf noofmonths = 5 Then
                                        totAmt = Val(amount) + Val(amount1) + Val(amount2) + Val(amount3) + Val(amount4)
                                        totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2) + Val(amountperct3) + Val(amountperct4)
                                        arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno), amount3.ToString(decno), amountperct3.ToString(decno), amount4.ToString(decno), amountperct4.ToString(decno)}
                                    ElseIf noofmonths >= 6 Then

                                        totAmt = Val(amount) + Val(amount1) + Val(amount2) + Val(amount3) + Val(amount4) + Val(amount5)
                                        totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2) + Val(amountperct3) + Val(amountperct4) + Val(amountperct5)
                                        arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno), amount3.ToString(decno), amountperct3.ToString(decno), amount4.ToString(decno), amountperct4.ToString(decno), amount5.ToString(decno), amountperct5.ToString(decno)}
                                    End If

                                    rowCount = rowCount + 1
                                    ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
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

                                    Dim acctLevel5 = From row1 In gacctLevel4.AsEnumerable() Where row1.Field(Of Integer)("level5") <> 0 Group row1 By g1 = New With {Key .level5 = row1.Field(Of Integer)("level5"), Key .divcode5 = row1.Field(Of String)("div_id")} Into Group Order By g1.divcode5
                                    If acctLevel5.Count > 0 Then
                                        ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Font.Bold = True
                                    End If
                                    For Each gpbylevel5 In acctLevel5
                                        Dim acctgroup5 = acctgroupTable.AsEnumerable().Select(Function(a) New With {.acctname5 = a.Field(Of String)("acctname"), .div_code5 = a.Field(Of String)("div_code"), .parentid5 = a.Field(Of Integer)("parentid")}).Where(Function(a) a.parentid5 = gpbylevel5.g1.level5 And a.div_code5 = gpbylevel5.g1.divcode5).OrderBy(Function(o) o.acctname5)
                                        'gacctLevel5 = gpbylevel5.Group.CopyToDataTable
                                        amount = 0
                                        amount1 = 0
                                        amount2 = 0
                                        amount3 = 0
                                        amount4 = 0
                                        amount5 = 0
                                        amountperct = 0
                                        amountperct1 = 0
                                        amountperct2 = 0
                                        amountperct3 = 0
                                        amountperct4 = 0
                                        amountperct5 = 0
                                        acct1name = ""
                                        If acctgroup5.Count > 0 Then

                                            For Each acct5row In acctgroup5
                                                acct1name = acct5row.acctname5
                                            Next
                                        End If
                                        For Each row In gpbylevel5.Group
                                            If row("state") >= 1 Then   '5
                                                If noofmonths >= 1 Then
                                                    If Not (TypeOf row("month1") Is DBNull) Then
                                                        amount = amount + Decimal.Parse(row("month1"))
                                                    Else
                                                        amount = amount + 0.0
                                                    End If

                                                    If Not (TypeOf row("month1perc") Is DBNull) Then
                                                        amountperct = amountperct + Decimal.Parse(row("month1perc"))
                                                    Else
                                                        amountperct = amountperct + 0.0
                                                    End If

                                                End If
                                                If noofmonths >= 2 Then
                                                    If Not (TypeOf row("month2") Is DBNull) Then
                                                        amount1 = amount1 + Decimal.Parse(row("month2"))
                                                    Else
                                                        amount1 = amount1 + 0.0
                                                    End If

                                                    If Not (TypeOf row("month2perc") Is DBNull) Then
                                                        amountperct1 = amountperct1 + Decimal.Parse(row("month2perc"))
                                                    Else
                                                        amountperct1 = amountperct1 + 0.0
                                                    End If


                                                End If
                                                If noofmonths >= 3 Then
                                                    If Not (TypeOf row("month3") Is DBNull) Then
                                                        amount2 = amount2 + Decimal.Parse(row("month3"))
                                                    Else
                                                        amount2 = amount2 + 0.0
                                                    End If
                                                    If Not (TypeOf row("month3perc") Is DBNull) Then
                                                        amountperct2 = amountperct2 + Decimal.Parse(row("month3perc"))
                                                    Else
                                                        amountperct2 = amountperct2 + 0.0
                                                    End If

                                                End If
                                                If noofmonths >= 4 Then
                                                    If Not (TypeOf row("month4") Is DBNull) Then
                                                        amount3 = amount3 + Decimal.Parse(row("month4"))
                                                    Else
                                                        amount3 = amount3 + 0.0
                                                    End If

                                                    If Not (TypeOf row("month4perc") Is DBNull) Then
                                                        amountperct3 = amountperct3 + Decimal.Parse(row("month4perc"))
                                                    Else
                                                        amountperct3 = amountperct3 + 0.0
                                                    End If


                                                End If
                                                If noofmonths >= 5 Then
                                                    If Not (TypeOf row("month5") Is DBNull) Then
                                                        amount4 = amount4 + Decimal.Parse(row("month5"))
                                                    Else
                                                        amount4 = amount4 + 0.0
                                                    End If

                                                    If Not (TypeOf row("month5perc") Is DBNull) Then
                                                        amountperct4 = amountperct4 + Decimal.Parse(row("month5perc"))
                                                    Else
                                                        amountperct4 = amountperct4 + 0.0
                                                    End If


                                                End If

                                                If noofmonths >= 6 Then
                                                    If Not (TypeOf row("month6") Is DBNull) Then
                                                        amount5 = amount5 + Decimal.Parse(row("month6"))
                                                    Else
                                                        amount5 = amount5 + 0.0
                                                    End If

                                                    If Not (TypeOf row("month6perc") Is DBNull) Then
                                                        amountperct5 = amountperct5 + Decimal.Parse(row("month6perc"))
                                                    Else
                                                        amountperct5 = amountperct5 + 0.0
                                                    End If


                                                End If
                                            End If



                                            'For Monthpercent

                                        Next

                                        If noofmonths = 1 Then
                                            totAmt = Val(amount)
                                            totPercent = Val(amountperct)
                                            arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno)}
                                        ElseIf noofmonths = 2 Then
                                            totAmt = Val(amount) + Val(amount1)
                                            totPercent = Val(amountperct) + Val(amountperct1)
                                            arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno)}

                                        ElseIf noofmonths = 3 Then
                                            totAmt = Val(amount) + Val(amount1) + Val(amount2)
                                            totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2)
                                            arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno)}

                                        ElseIf noofmonths = 4 Then
                                            totAmt = Val(amount) + Val(amount1) + Val(amount2) + Val(amount3)
                                            totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2) + Val(amountperct3)
                                            arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno), amount3.ToString(decno), amountperct3.ToString(decno)}

                                        ElseIf noofmonths = 5 Then
                                            totAmt = Val(amount) + Val(amount1) + Val(amount2) + Val(amount3) + Val(amount4)
                                            totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2) + Val(amountperct3) + Val(amountperct4)
                                            arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno), amount3.ToString(decno), amountperct3.ToString(decno), amount4.ToString(decno), amountperct4.ToString(decno)}
                                        ElseIf noofmonths >= 6 Then
                                            totAmt = Val(amount) + Val(amount1) + Val(amount2) + Val(amount3) + Val(amount4) + Val(amount5)
                                            totPercent = Val(amountperct) + Val(amountperct1) + Val(amountperct2) + Val(amountperct3) + Val(amountperct4) + Val(amountperct5)
                                            arrHeaders = {acct1name, totAmt.ToString(decno), totPercent.ToString(decno), amount.ToString(decno), amountperct.ToString(decno), amount1.ToString(decno), amountperct1.ToString(decno), amount2.ToString(decno), amountperct2.ToString(decno), amount3.ToString(decno), amountperct3.ToString(decno), amount4.ToString(decno), amountperct4.ToString(decno), amount5.ToString(decno), amountperct5.ToString(decno)}
                                        End If


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
                            If noofmonths = 1 Then
                                totAmtCost = Val(amount20)
                                totPercentCost = Val(costofsalesperct)
                                arrHeaders = {acct2name, totAmtCost.ToString(decno), totPercentCost.ToString(decno), amount20.ToString(decno), costofsalesperct.ToString(decno)}

                            ElseIf noofmonths = 2 Then
                                totAmtCost = Val(amount20) + Val(amount21)
                                totPercentCost = Val(costofsalesperct) + Val(costofsalesperct1)
                                arrHeaders = {acct2name, totAmtCost.ToString(decno), totPercentCost.ToString(decno), amount20.ToString(decno), costofsalesperct.ToString(decno), amount21.ToString(decno), costofsalesperct1.ToString(decno)}

                            ElseIf noofmonths = 3 Then
                                totAmtCost = Val(amount20) + Val(amount21) + Val(amount22)
                                totPercentCost = Val(costofsalesperct) + Val(costofsalesperct1) + Val(costofsalesperct2)
                                arrHeaders = {acct2name, totAmtCost.ToString(decno), totPercentCost.ToString(decno), amount20.ToString(decno), costofsalesperct.ToString(decno), amount21.ToString(decno), costofsalesperct1.ToString(decno), amount22.ToString(decno), costofsalesperct2.ToString(decno)}


                            ElseIf noofmonths = 4 Then
                                totAmtCost = Val(amount20) + Val(amount21) + Val(amount22) + Val(amount23)
                                totPercentCost = Val(costofsalesperct) + Val(costofsalesperct1) + Val(costofsalesperct2) + Val(costofsalesperct3)
                                arrHeaders = {acct2name, totAmtCost.ToString(decno), totPercentCost.ToString(decno), amount20.ToString(decno), costofsalesperct.ToString(decno), amount21.ToString(decno), costofsalesperct1.ToString(decno), amount22.ToString(decno), costofsalesperct2.ToString(decno), amount23.ToString(decno), costofsalesperct3.ToString(decno)}

                            ElseIf noofmonths = 5 Then
                                totAmtCost = Val(amount20) + Val(amount21) + Val(amount22) + Val(amount23) + Val(amount24)
                                totPercentCost = Val(costofsalesperct) + Val(costofsalesperct1) + Val(costofsalesperct2) + Val(costofsalesperct3) + Val(costofsalesperct4)
                                arrHeaders = {acct2name, totAmtCost.ToString(decno), totPercentCost.ToString(decno), amount20.ToString(decno), costofsalesperct.ToString(decno), amount21.ToString(decno), costofsalesperct1.ToString(decno), amount22.ToString(decno), costofsalesperct2.ToString(decno), amount23.ToString(decno), costofsalesperct3.ToString(decno), amount24.ToString(decno), costofsalesperct4.ToString(decno)}
                            ElseIf noofmonths >= 6 Then
                                totAmtCost = Val(amount20) + Val(amount21) + Val(amount22) + Val(amount23) + Val(amount24) + Val(amount25)
                                totPercentCost = Val(costofsalesperct) + Val(costofsalesperct1) + Val(costofsalesperct2) + Val(costofsalesperct3) + Val(costofsalesperct4) + Val(costofsalesperct5)
                                arrHeaders = {acct2name, totAmtCost.ToString(decno), totPercentCost.ToString(decno), amount20.ToString(decno), costofsalesperct.ToString(decno), amount21.ToString(decno), costofsalesperct1.ToString(decno), amount22.ToString(decno), costofsalesperct2.ToString(decno), amount23.ToString(decno), costofsalesperct3.ToString(decno), amount24.ToString(decno), costofsalesperct4.ToString(decno), amount25.ToString(decno), costofsalesperct5.ToString(decno)}
                            End If

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
                If groupby.g.Head = 1 Then
                    Dim gfooter As String = "GROSS PROFIT"
                    Dim totgProfit As Decimal
                    Dim totgProfitPerc As Decimal
                    Dim g As Integer = 0
                    If dtnettotal.Rows.Count <> 0 Then
                        rowCount = rowCount + 2
                        Dim grow As DataRow = dtnettotal.Rows(0)
                        If noofmonths = 1 Then
                            totgProfit = Val(grow.Item("month1"))
                            totgProfitPerc = Val(grow.Item("month1perc"))
                            arrHeaders = {gfooter, totgProfit.ToString(decno), totgProfitPerc.ToString(decno), grow.Item("month1"), grow.Item("month1perc")}

                        ElseIf noofmonths = 2 Then
                            totgProfit = Val(grow.Item("month1")) + Val(grow.Item("month2"))
                            totgProfitPerc = Val(grow.Item("month1perc")) + grow.Item("month2perc")
                            arrHeaders = {gfooter, totgProfit.ToString(decno), totgProfitPerc.ToString(decno), grow.Item("month1"), grow.Item("month1perc"), grow.Item("month2"), grow.Item("month2perc")}


                        ElseIf noofmonths = 3 Then

                            totgProfit = Val(grow.Item("month1")) + Val(grow.Item("month2")) + Val(grow.Item("month3"))
                            totgProfitPerc = Val(grow.Item("month1perc")) + grow.Item("month2perc") + grow.Item("month3perc")
                            arrHeaders = {gfooter, totgProfit.ToString(decno), totgProfitPerc.ToString(decno), grow.Item("month1"), grow.Item("month1perc"), grow.Item("month2"), grow.Item("month2perc"), grow.Item("month3"), grow.Item("month3perc")}

                        ElseIf noofmonths = 4 Then
                            totgProfit = Val(grow.Item("month1")) + Val(grow.Item("month2")) + Val(grow.Item("month3")) + Val(grow.Item("month4"))
                            totgProfitPerc = Val(grow.Item("month1perc")) + grow.Item("month2perc") + grow.Item("month3perc") + grow.Item("month4perc")
                            arrHeaders = {gfooter, totgProfit.ToString(decno), totgProfitPerc.ToString(decno), grow.Item("month1"), grow.Item("month1perc"), grow.Item("month2"), grow.Item("month2perc"), grow.Item("month3"), grow.Item("month3perc"), grow.Item("month4"), grow.Item("month4perc")}




                        ElseIf noofmonths = 5 Then
                            totgProfit = Val(grow.Item("month1")) + Val(grow.Item("month2")) + Val(grow.Item("month3")) + Val(grow.Item("month4")) + Val(grow.Item("month5"))
                            totgProfitPerc = Val(grow.Item("month1perc")) + grow.Item("month2perc") + grow.Item("month3perc") + grow.Item("month4perc") + grow.Item("month5perc")
                            arrHeaders = {gfooter, totgProfit.ToString(decno), totgProfitPerc.ToString(decno), grow.Item("month1"), grow.Item("month1perc"), grow.Item("month2"), grow.Item("month2perc"), grow.Item("month3"), grow.Item("month3perc"), grow.Item("month4"), grow.Item("month4perc"), grow.Item("month5"), grow.Item("month5perc")}


                        ElseIf noofmonths >= 6 Then
                            totgProfit = Val(grow.Item("month1")) + Val(grow.Item("month2")) + Val(grow.Item("month3")) + Val(grow.Item("month4")) + Val(grow.Item("month5")) + Val(grow.Item("month6"))
                            totgProfitPerc = Val(grow.Item("month1perc")) + grow.Item("month2perc") + grow.Item("month3perc") + grow.Item("month4perc") + grow.Item("month5perc") + grow.Item("month6perc")
                            arrHeaders = {gfooter, totgProfit.ToString(decno), totgProfitPerc.ToString(decno), grow.Item("month1"), grow.Item("month1perc"), grow.Item("month2"), grow.Item("month2perc"), grow.Item("month3"), grow.Item("month3perc"), grow.Item("month4"), grow.Item("month4perc"), grow.Item("month5"), grow.Item("month5perc"), grow.Item("month6"), grow.Item("month6perc")}

                        End If

                        ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
                        ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                        ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Alignment.WrapText = True

                        For j = 0 To arrHeaders.Length - 1
                            If j = 0 Then
                                ws.Cell(rowCount, j + 1).Value = arrHeaders(j)
                                ws.Cell(rowCount, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            Else
                                ws.Cell(rowCount, j + 1).Value = Decimal.Parse(arrHeaders(j))
                                ws.Cell(rowCount, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).NumberFormat.Format = DecimalPoint
                            End If
                        Next
                        rowCount = rowCount + 1

                    End If
                End If
            Next
            Dim footer As String = "Net PROFIT"
            If Convert.ToString(ws.Cell(rowCount - 1, 1).Value).Trim <> "GROSS PROFIT" Then
                rowCount = rowCount + 2
            End If
            Dim totProfit As Decimal
            Dim totProfitPerc As Decimal
            If dtnettotal.Rows.Count <> 0 Then

                Dim grow As DataRow = dtnettotal.Rows(1)
                If noofmonths = 1 Then
                    totProfit = Val(grow.Item("month1"))
                    totProfitPerc = Val(grow.Item("month1perc"))
                    arrHeaders = {footer, totProfit.ToString(decno), totProfitPerc.ToString(decno), grow.Item("month1"), grow.Item("month1perc")}
                ElseIf noofmonths = 2 Then
                    totProfit = Val(grow.Item("month1")) + Val(grow.Item("month2"))
                    totProfitPerc = Val(grow.Item("month1perc")) + grow.Item("month2perc")
                    arrHeaders = {footer, totProfit.ToString(decno), totProfitPerc.ToString(decno), grow.Item("month1"), grow.Item("month1perc"), grow.Item("month2"), grow.Item("month2perc")}
                ElseIf noofmonths = 3 Then
                    totProfit = Val(grow.Item("month1")) + Val(grow.Item("month2")) + Val(grow.Item("month3"))
                    totProfitPerc = Val(grow.Item("month1perc")) + grow.Item("month2perc") + grow.Item("month3perc")
                    arrHeaders = {footer, totProfit.ToString(decno), totProfitPerc.ToString(decno), grow.Item("month1"), grow.Item("month1perc"), grow.Item("month2"), grow.Item("month2perc"), grow.Item("month3"), grow.Item("month3perc")}
                ElseIf noofmonths = 4 Then
                    totProfit = Val(grow.Item("month1")) + Val(grow.Item("month2")) + Val(grow.Item("month3")) + Val(grow.Item("month4"))
                    totProfitPerc = Val(grow.Item("month1perc")) + grow.Item("month2perc") + grow.Item("month3perc") + grow.Item("month4perc")
                    arrHeaders = {footer, totProfit.ToString(decno), totProfitPerc.ToString(decno), grow.Item("month1"), grow.Item("month1perc"), grow.Item("month2"), grow.Item("month2perc"), grow.Item("month3"), grow.Item("month3perc"), grow.Item("month4"), grow.Item("month4perc")}

                ElseIf noofmonths = 5 Then
                    totProfit = Val(grow.Item("month1")) + Val(grow.Item("month2")) + Val(grow.Item("month3")) + Val(grow.Item("month4")) + Val(grow.Item("month5"))
                    totProfitPerc = Val(grow.Item("month1perc")) + grow.Item("month2perc") + grow.Item("month3perc") + grow.Item("month4perc") + grow.Item("month5perc")
                    arrHeaders = {footer, totProfit.ToString(decno), totProfitPerc.ToString(decno), grow.Item("month1"), grow.Item("month1perc"), grow.Item("month2"), grow.Item("month2perc"), grow.Item("month3"), grow.Item("month3perc"), grow.Item("month4"), grow.Item("month4perc"), grow.Item("month5"), grow.Item("month5perc")}

                ElseIf noofmonths >= 6 Then
                    totProfit = Val(grow.Item("month1")) + Val(grow.Item("month2")) + Val(grow.Item("month3")) + Val(grow.Item("month4")) + Val(grow.Item("month5")) + Val(grow.Item("month6"))
                    totProfitPerc = Val(grow.Item("month1perc")) + grow.Item("month2perc") + grow.Item("month3perc") + grow.Item("month4perc") + grow.Item("month5perc") + grow.Item("month6perc")
                    arrHeaders = {footer, totProfit.ToString(decno), totProfitPerc.ToString(decno), grow.Item("month1"), grow.Item("month1perc"), grow.Item("month2"), grow.Item("month2perc"), grow.Item("month3"), grow.Item("month3perc"), grow.Item("month4"), grow.Item("month4perc"), grow.Item("month5"), grow.Item("month5perc"), grow.Item("month6"), grow.Item("month6perc")}
                End If
                '  arrHeaders = {"NET PROFIT", (income - expanse - costofsales).ToString(decno), (income1 - expanse1 - costofsales1).ToString(decno), (income2 - expanse2 - costofsales2).ToString(decno), (income3 - expanse3 - costofsales3).ToString(decno), (income4 - expanse4 - costofsales4).ToString(decno), (income5 - expanse5 - costofsales5).ToString(decno)}

                ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
                ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(rowCount, 1, rowCount, arrHeaders.Length).Style.Alignment.WrapText = True

                For j = 0 To arrHeaders.Length - 1
                    'Phrase = New Phrase()
                    'Phrase.Add(New Chunk(arrHeaders(i), Level1font))
                    If j = 0 Then
                        ws.Cell(rowCount, j + 1).Value = arrHeaders(j)
                        ws.Cell(rowCount, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    Else
                        ws.Cell(rowCount, j + 1).Value = Decimal.Parse(arrHeaders(j))
                        ws.Cell(rowCount, j + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).NumberFormat.Format = DecimalPoint
                    End If
                Next
            End If
        End If

        ws.Cell((rowCount + 2), 1).Value = "Printed Date:" & Now.ToString("dd/MM/yyyy")
        ws.Range((rowCount + 2), 1, (rowCount + 2), 3).Merge()
        Using wStream As New MemoryStream()
            wb.SaveAs(wStream)
            bytes = wStream.ToArray()
        End Using
    End Sub
#End Region

End Class
