Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.Globalization
Imports System.Linq
Imports ClosedXML.Excel
Imports System.Collections.Generic 'Ram 22082022

Public Class BalanceSheetPdf
    Inherits System.Web.UI.Page
    Dim objutils As New clsUtils
#Region "global declaration"
    Dim objclsUtilities As New clsUtils
    Dim normalfont As Font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK)
    Dim caption As Font = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.BLACK)
    Dim normalfontbold As Font = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLACK)
    Dim headerfont As Font = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.BLACK)
    Dim Companyname As Font = New Font(FontFactory.GetFont("Times New Roman", 14, Font.BOLD, BaseColor.WHITE))
    Dim ReportNamefont As Font = New Font(FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK))
    Dim Filter As Font = New Font(FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK))
    Dim bgColor As BaseColor = New BaseColor(204, 204, 204)
    Dim rptcompanyname, decno, currency, arrHeader(), arrHeaders(), grp, grp1, grp2, grp3, grp4, space, group2name, group3name As String
    Dim grpby2, grpby3, grpby4, grpHead As New DataTable
    Dim acctmajor() As System.Data.DataRow
    Dim decpt, flag As Integer
    Dim grpToatalname As String = ""
    Dim decimalPoint As String = ""
    Dim coadd1, coadd2, copobox, cotel, cofax As String
    Dim totalgrp, Liabilitiestot, EquityTot, ProfitTot, pdebit, pcredit, cdebit, ccredit, total, ftotal, subtotal, ptotal, finaltotal, subnametotal, tdebit, tcredit, fpdebit, fpcredit, fcdebit, fccredit, ftdebit, ftcredit As Decimal
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
    'Ram 22082022
#Region "Generate Report New"
    Public Sub GenerateReportNew(ByVal todate As String, divcode As String, reportsType As String, pagetype As String, level As String, rptreportname As String, reportfilter As String, newformat As Integer, ByRef bytes() As Byte, ByVal printMode As String, Optional ByVal fileName As String = "")
        Dim dtBalanceSheet As New DataTable
        Dim dtBalanceSheetDate As New DataTable
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
        sqlcmd = New SqlCommand("sp_Balance_newformat", conn1)
        sqlcmd.CommandType = CommandType.StoredProcedure
        sqlcmd.Parameters.AddWithValue("@date", todate)
        sqlcmd.Parameters.AddWithValue("@division", divcode)
        sqlAdp = New SqlDataAdapter(sqlcmd)
        sqlAdp.SelectCommand.CommandTimeout = 180
        sqlAdp.Fill(ds)

        If ds IsNot Nothing Then
            If ds.Tables.Count > 0 Then
                dtBalanceSheet = ds.Tables(0)
                dtBalanceSheetDate = ds.Tables(1)
            End If
        End If

        If String.Equals(reportsType, "excel") Then
            ExcelReportNew(dtBalanceSheet, dtBalanceSheetDate, reportfilter, currency, rptreportname, bytes)
        End If
    End Sub
#End Region
#Region "ExcelReportNew"

    Public Sub ExcelReportNew(ByVal dtBalanceSheet As DataTable, ByVal dtBalanceSheetDate As DataTable, ByVal reportfilter As String, ByVal currency As String, ByVal rptreportname As String, ByRef bytes() As Byte)
        Dim arrHeaders() As String
        Dim wb As New XLWorkbook
        Dim decimalPoint1 As String = ""
        'Sharfudeen 08-09-2022

        Dim positiveFormat As String = ""
        Dim negativeFormat As String = ""
        Dim zeroFormat As String = ""
        Dim numberFormat As String = ""

        Dim fullNumberFormat As String = ""

        Dim roundpositiveFormat As String = ""
        Dim roundnegativeFormat As String = ""
        Dim roundnumberFormat As String = ""
        'Sharfudeen 08-09-2022
        If decpt = 2 Then
            decimalPoint = "#,##0.00"
            decimalPoint1 = "(#,##0.00)"
            'sharfudeen 07/09/2022
            positiveFormat = "#,##0.00_)"
            negativeFormat = "(#,##0.00)"
            zeroFormat = "-_)"
            numberFormat = positiveFormat + ";" + negativeFormat + " ;@"

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

        Dim ws = wb.Worksheets.Add("Balance Sheet")
        ws.Columns.AdjustToContents()
        ws.Column("A").Width = 35
        ws.Column("B").Width = 20
        ws.Column("C").Width = 20
        Dim rownum As Integer = 6

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

        If dtBalanceSheetDate.Rows.Count > 0 Then
            ws.Cell("B5").Value = dtBalanceSheetDate.Rows(0)("Column1").ToString()
            ws.Cell("B5").Style.Font.SetBold().Font.FontSize = 10
            ws.Cell("B5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            ws.Cell("B5").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            ws.Cell("B6").Value = currency
            ws.Cell("B6").Style.Font.SetBold().Font.FontSize = 10
            ws.Cell("B6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            ws.Cell("B6").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            ws.Cell("C5").Value = dtBalanceSheetDate.Rows(0)("Column2").ToString()
            ws.Cell("C5").Style.Font.SetBold().Font.FontSize = 10
            ws.Cell("C5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            ws.Cell("C5").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            ws.Cell("C6").Value = currency
            ws.Cell("C6").Style.Font.SetBold().Font.FontSize = 10
            ws.Cell("C6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            ws.Cell("C6").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
        End If

        If dtBalanceSheet.Rows.Count > 0 Then
            Dim dtfilterLevel1 As New DataTable
            Dim dtfilterLevel2 As New DataTable
            Dim dtfilterLevel3 As New DataTable
            Dim dtfilterLevel4 As New DataTable
            Dim dtfilterLevel5 As New DataTable
            Dim CAlevel1sum As Decimal = 0
            Dim CAlevel2sum As Decimal = 0
            Dim PAlevel1sum As Decimal = 0
            Dim PAlevel2sum As Decimal = 0

            Dim distinctLevel1 As IEnumerable(Of Int32) = dtBalanceSheet.AsEnumerable().
                Select(Function(row) row.Field(Of Int32)("Level1")).Distinct()

            For Each rowlevel1 In distinctLevel1

                dtfilterLevel1 = dtBalanceSheet.Select("Level1=" + rowlevel1.ToString()).CopyToDataTable()

                ws.Cell(rownum + 1, 1).Value = dtfilterLevel1.Rows(0)("Level1Name").ToString()
                ws.Cell(rownum + 1, 1).Style.Font.SetBold().Font.FontSize = 14
                ws.Cell(rownum + 1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

                rownum += 1

                CAlevel1sum = Convert.ToDecimal(dtfilterLevel1.Compute("SUM(CurrentAmount)", String.Empty))
                PAlevel1sum = Convert.ToDecimal(dtfilterLevel1.Compute("SUM(PreviousYearAmount)", String.Empty))

                Dim distinctLevel2 As IEnumerable(Of Int32) = dtfilterLevel1.AsEnumerable().
                Select(Function(row) row.Field(Of Int32)("Level2")).Distinct()

                For Each rowlevel2 In distinctLevel2

                    dtfilterLevel2 = dtfilterLevel1.Select("Level2=" + rowlevel2.ToString()).CopyToDataTable()

                    ws.Cell(rownum + 1, 1).Value = dtfilterLevel2.Rows(0)("Level2Name").ToString()
                    ws.Cell(rownum + 1, 1).Style.Font.SetBold().Font.FontSize = 12
                    ws.Cell(rownum + 1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

                    rownum += 1

                    CAlevel2sum = Convert.ToDecimal(dtfilterLevel2.Compute("SUM(CurrentAmount)", String.Empty))
                    PAlevel2sum = Convert.ToDecimal(dtfilterLevel2.Compute("SUM(PreviousYearAmount)", String.Empty))

                    Dim distinctLevel3 As IEnumerable(Of Int32) = dtfilterLevel2.AsEnumerable().
                        Select(Function(row) row.Field(Of Int32)("Level3")).Distinct()

                    For Each rowlevel3 In distinctLevel3

                        dtfilterLevel3 = dtfilterLevel2.Select("Level3=" + rowlevel3.ToString()).CopyToDataTable()

                        ws.Cell(rownum + 1, 1).Value = dtfilterLevel3.Rows(0)("AccName").ToString()
                        ws.Cell(rownum + 1, 1).Style.Font.FontSize = 10
                        ws.Cell(rownum + 1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

                        ws.Cell(rownum + 1, 2).Value = Decimal.Parse(Convert.ToDecimal(dtfilterLevel3.Rows(0)("CurrentAmount")))
                        ws.Cell(rownum + 1, 2).Style.NumberFormat.Format = roundnumberFormat ' decimalPoint 'sharfudeen 08/09/2022
                        'ws.Cell(rownum + 1, 2).Style.NumberFormat.Format = decimalPoint
                        ws.Cell(rownum + 1, 2).Style.Font.FontSize = 10
                        ws.Cell(rownum + 1, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                        ws.Cell(rownum + 1, 3).Value = Decimal.Parse(Convert.ToDecimal(dtfilterLevel3.Rows(0)("PreviousYearAmount")))
                        ws.Cell(rownum + 1, 3).Style.NumberFormat.Format = roundnumberFormat ' decimalPoint 'sharfudeen 08/09/2022
                        'ws.Cell(rownum + 1, 3).Style.NumberFormat.Format = decimalPoint
                        ws.Cell(rownum + 1, 3).Style.Font.FontSize = 10
                        ws.Cell(rownum + 1, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                        rownum += 1

                    Next

                    ws.Cell(rownum + 1, 1).Value = "Total " & dtfilterLevel2.Rows(0)("Level2Name").ToString()
                    ws.Cell(rownum + 1, 1).Style.Font.SetBold().Font.FontSize = 11
                    ws.Cell(rownum + 1, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)

                    ws.Cell(rownum + 1, 2).Value = Decimal.Parse(Convert.ToDecimal(CAlevel2sum))
                    '
                    ws.Cell(rownum + 1, 2).Style.NumberFormat.Format = roundnumberFormat ' decimalPoint 'sharfudeen 08/09/2022

                    ws.Cell(rownum + 1, 2).Style.Border.TopBorder = XLBorderStyleValues.Medium
                    ws.Cell(rownum + 1, 2).Style.Border.BottomBorder = XLBorderStyleValues.Medium
                    ws.Cell(rownum + 1, 2).Style.Font.SetBold().Font.FontSize = 11
                    ws.Cell(rownum + 1, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                    ws.Cell(rownum + 1, 3).Value = Decimal.Parse(Convert.ToDecimal(PAlevel2sum))
                    ws.Cell(rownum + 1, 3).Style.NumberFormat.Format = roundnumberFormat ' decimalPoint 'sharfudeen 08/09/2022
                    'ws.Cell(rownum + 1, 3).Style.NumberFormat.Format = decimalPoint
                    ws.Cell(rownum + 1, 3).Style.Border.TopBorder = XLBorderStyleValues.Medium
                    ws.Cell(rownum + 1, 3).Style.Border.BottomBorder = XLBorderStyleValues.Medium
                    ws.Cell(rownum + 1, 3).Style.Font.SetBold().Font.FontSize = 11
                    ws.Cell(rownum + 1, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                    rownum += 1
                Next

                rownum += 1

                ws.Cell(rownum + 1, 1).Value = "Total " & dtfilterLevel1.Rows(0)("Level1Name").ToString()
                ws.Cell(rownum + 1, 1).Style.Font.SetBold().Font.FontSize = 12
                ws.Cell(rownum + 1, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)

                ws.Cell(rownum + 1, 2).Value = Decimal.Parse(Convert.ToDecimal(CAlevel1sum))
                'ws.Cell(rownum + 1, 2).Style.NumberFormat.Format = decimalPoint
                ws.Cell(rownum + 1, 2).Style.NumberFormat.Format = roundnumberFormat ' decimalPoint 'sharfudeen 08/09/2022
                ws.Cell(rownum + 1, 2).Style.Border.TopBorder = XLBorderStyleValues.Medium
                ws.Cell(rownum + 1, 2).Style.Border.BottomBorder = XLBorderStyleValues.Medium
                ws.Cell(rownum + 1, 2).Style.Font.SetBold().Font.FontSize = 12
                ws.Cell(rownum + 1, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                ws.Cell(rownum + 1, 3).Value = Decimal.Parse(Convert.ToDecimal(PAlevel1sum))
                ws.Cell(rownum + 1, 3).Style.NumberFormat.Format = roundnumberFormat ' decimalPoint 'sharfudeen 08/09/2022
                ws.Cell(rownum + 1, 3).Style.Border.TopBorder = XLBorderStyleValues.Medium
                ws.Cell(rownum + 1, 3).Style.Border.BottomBorder = XLBorderStyleValues.Medium
                ws.Cell(rownum + 1, 3).Style.Font.SetBold().Font.FontSize = 12
                ws.Cell(rownum + 1, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

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
    'Ram 22082022
#Region "GenerateReport"
    Public Sub GenerateReport(ByVal todate As String, divcode As String, reportsType As String, pagetype As String, level As String, rptreportname As String, reportfilter As String, newformat As Integer, ByRef bytes() As Byte, ByVal printMode As String, Optional ByVal fileName As String = "")

        Try

            Dim conn1 As New SqlConnection

            conn1 = clsDBConnect.dbConnectionnew("strDBConnection")

            Dim sql = "SELECT acctgroup.acctname, acctgroup2.acctname as acctname2, acctgroup3.acctname as acctname3, acctgroup4.acctname as acctname4, acctgroup5.acctname as acctname2, view_actgroup.acctname, view_actgroup.acctlevel, Balance.Amount as amount, Balance.Head, Balance.State state, Balance.GroupHeader, acctgroup2.acctcode as acctcode2, acctgroup3.acctcode as acctcode3, acctgroup4.acctcode as acctcode4, acctgroup5.acctcode as acctcode5, view_actgroup.acctcode, profit.Profit, profit.equity, profit.Liablities, acctgroup1.acctname as acctname1, acctgroup1.acctcode  as acctcode1 ,acctgroup.acctcode FROM   (((((((dbo.balance Balance INNER JOIN dbo.view_actgroup view_actgroup ON ((Balance.acctcode=view_actgroup.acctcode) AND (Balance.State=view_actgroup.acctlevel)) AND (Balance.div_code=view_actgroup.div_code)) LEFT OUTER JOIN dbo.acctgroup acctgroup2 ON (Balance.Level2=acctgroup2.parentid) AND (Balance.div_code=acctgroup2.div_code)) LEFT OUTER JOIN dbo.acctgroup acctgroup3 ON (Balance.Level3=acctgroup3.parentid) AND (Balance.div_code=acctgroup3.div_code)) LEFT OUTER JOIN dbo.acctgroup acctgroup4 ON (Balance.Level4=acctgroup4.parentid) AND (Balance.div_code=acctgroup4.div_code)) LEFT OUTER JOIN dbo.acctgroup acctgroup5 ON (Balance.Level5=acctgroup5.parentid) AND (Balance.div_code=acctgroup5.div_code)) INNER JOIN dbo.Profit profit ON (Balance.Head=profit.Rowno) AND (Balance.div_code=profit.div_code)) INNER JOIN dbo.acctgroup acctgroup1 ON (Balance.Level1=acctgroup1.parentid) AND (Balance.div_code=acctgroup1.div_code)) INNER JOIN dbo.acctgroup acctgroup ON (Balance.Level0=acctgroup.parentid) AND (Balance.div_code=acctgroup.div_code) ORDER BY acctgroup.acctname, Balance.Head, Balance.GroupHeader, acctgroup1.acctname, acctgroup2.acctname, acctgroup3.acctname, acctgroup4.acctname, acctgroup5.acctname"

            Dim custdetailsdt As New DataTable
            Dim sqlAdp As SqlDataAdapter

            sqlAdp = New SqlDataAdapter(sql, conn1)
            'Using dad As New SqlDataAdapter(sql, conn1)
            sqlAdp.SelectCommand.CommandTimeout = 180
            sqlAdp.Fill(custdetailsdt)

            'End Using




            If divcode <> "" Then
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
            Else
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
            End If
            decpt = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
            decno = "N" + decpt.ToString()
            currency = objutils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)


            If String.Equals(reportsType, "excel") Then
                ExcelReport(custdetailsdt, reportfilter, currency, rptreportname, bytes)
            Else
                Dim documentWidth As Single

                Dim document As New Document(PageSize.A4, 23.0F, 23.0F, 30.0F, 35.0F)
                documentWidth = 550.0F

                Using memoryStream As New System.IO.MemoryStream()
                    Dim writer As PdfWriter = PdfWriter.GetInstance(document, memoryStream)

                    Dim phrase As Phrase = Nothing
                    Dim cell As PdfPCell = Nothing
                    Dim cell1 As PdfPCell = Nothing
                    Dim titletable As PdfPTable = Nothing
                    Dim Rowtitlebg As BaseColor = New BaseColor(192, 192, 192)
                    Dim CompanybgColor As BaseColor = New BaseColor(0, 72, 192)
                    Dim ReportNamebgColor As BaseColor = New BaseColor(0, 128, 192)

                    If divcode <> "" Then
                        rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
                    Else
                        rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
                    End If
                    'titletable = New PdfPTable(2)
                    'titletable.TotalWidth = documentWidth
                    'titletable.LockedWidth = True
                    'titletable.SetWidths(New Single() {0.5F, 0.5F})
                    'titletable.Complete = False
                    'titletable.SplitRows = False
                    ''company name
                    'phrase = New Phrase()
                    'phrase.Add(New Chunk(rptcompanyname, Companyname))
                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    'cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    'cell.Colspan = 2
                    'cell.SetLeading(12, 0)
                    'cell.PaddingBottom = 4
                    'cell.BackgroundColor = CompanybgColor
                    'titletable.AddCell(cell)
                    'titletable.Complete = True
                    coadd1 = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coadd1 from Columbusmaster"), String)
                    coadd2 = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select coadd2 from Columbusmaster"), String)
                    copobox = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select copobox from Columbusmaster"), String)
                    cotel = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select cotel from Columbusmaster"), String)
                    cofax = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select cofax from Columbusmaster"), String)




                    'Header Table
                    titletable = New PdfPTable(2)
                    titletable.TotalWidth = documentWidth
                    titletable.LockedWidth = True
                    titletable.SetWidths(New Single() {0.71F, 0.29F})

                    titletable.Complete = False
                    titletable.SplitRows = False
                    titletable.SpacingBefore = 10.0F
                    titletable.WidthPercentage = 100
                    cell = ImageCell("~/Images/Logo.png", 80.0F, PdfPCell.ALIGN_LEFT)
                    titletable.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(rptcompanyname & Environment.NewLine, ReportNamefont))
                    phrase.Add(New Chunk(coadd2 & copobox & Environment.NewLine, normalfont))
                    ' phrase.Add(New Chunk( & Environment.NewLine & vbLf, normalfont))
                    phrase.Add(New Chunk(cotel & cofax, normalfont))


                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.PaddingBottom = 5.0F
                    cell.SetLeading(14, 0)
                    titletable.AddCell(cell)

                    Dim Reporttitle = New PdfPTable(2)
                    Reporttitle.TotalWidth = documentWidth
                    Reporttitle.LockedWidth = True
                    Reporttitle.SetWidths(New Single() {0.5F, 0.5F})
                    Reporttitle.Complete = False
                    Reporttitle.SplitRows = False
                    phrase = New Phrase()
                    phrase.Add(New Chunk(rptreportname, ReportNamefont))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_CENTER, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 4
                    Reporttitle.AddCell(cell)

                    phrase = New Phrase()
                    phrase.Add(New Chunk(reportfilter, Filter))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 5
                    cell.PaddingTop = 10
                    Reporttitle.AddCell(cell)
                    Reporttitle.SpacingBefore = 5
                    Reporttitle.SpacingAfter = 5
                    Reporttitle.Complete = True

                    'Dim tableTitle As PdfPTable = Nothing

                    'tableTitle = New PdfPTable(3)
                    '' arrHeaders = {"Code", "Name", "Total Amount (" + currency + ")"}
                    'arrHeaders = {"Code", "Name", "Total Amount (" + currency + ")"}
                    'tableTitle.SetWidths(New Single() {0.15F, 0.65, 0.2F})
                    'tableTitle.TotalWidth = documentWidth
                    'tableTitle.LockedWidth = True
                    'tableTitle.SplitRows = False

                    'tableTitle.SpacingBefore = 15

                    'For i = 0 To arrHeaders.Length - 1
                    '    phrase = New Phrase()
                    '    phrase.Add(New Chunk(arrHeaders(i), normalfontbold))
                    '    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)

                    '    cell.SetLeading(12, 0)
                    '    cell.PaddingBottom = 4.0F
                    '    cell.PaddingTop = 1.0F
                    '    tableTitle.AddCell(cell)
                    'Next
                    'tableTitle.Complete = True


                    Dim FooterTable = New PdfPTable(2)
                    FooterTable.TotalWidth = documentWidth
                    FooterTable.LockedWidth = True
                    FooterTable.SetWidths(New Single() {0.5F, 0.5F})
                    FooterTable.Complete = False
                    FooterTable.SplitRows = False
                    phrase = New Phrase()
                    phrase.Add(New Chunk("Printed Date: " & Date.Now.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)))
                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, False)
                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                    cell.Colspan = 2
                    cell.SetLeading(12, 0)
                    cell.PaddingBottom = 3
                    FooterTable.AddCell(cell)
                    FooterTable.SpacingBefore = 12.0F
                    FooterTable.Complete = True

                    writer.PageEvent = New ClsHeaderFooter(titletable, Reporttitle, FooterTable, Nothing, "printDate")
                    document.Open()


                    Dim tableData As PdfPTable = Nothing

                    tableData = New PdfPTable(3)
                    tableData.SetWidths(New Single() {0.15F, 0.65, 0.2F})
                    tableData.TotalWidth = documentWidth
                    tableData.LockedWidth = True
                    tableData.SplitRows = False
                    ' tableData.KeepTogether = True

                    group3name = ""
                    Dim k As Integer = 0
                    Dim Print As Boolean = False
                    Dim arrData() As String


                    If custdetailsdt.Rows.Count > 0 Then

                        'acctmajor = custdetailsdt.Select("group1name<>'" & Nothing & "'")
                        Dim GropName As String = ""
                        Dim sumlevel, sumlevel1 As Decimal
                        Dim group1 = From gpbyrow In custdetailsdt.AsEnumerable() Group gpbyrow By g = New With {Key .gpbyname = gpbyrow(21)} Into Group Order By g.gpbyname
                        For Each keys In group1

                            grpHead = keys.Group.CopyToDataTable
                            Dim groupHead = From gpbyrow In custdetailsdt.AsEnumerable() Group gpbyrow By g = New With {Key .gpHead = gpbyrow(8), Key .gpgrophead = gpbyrow(10)} Into Group

                            For Each keysh In groupHead

                                grpHead = keysh.Group.CopyToDataTable

                                If keysh.g.gpgrophead = 4 Then
                                    GropName = "Total Assets"

                                ElseIf keysh.g.gpgrophead = 5 Then
                                    GropName = "Total Liabilities "
                                ElseIf keysh.g.gpgrophead = 6 Then
                                    GropName = "Total Partner Equity "

                                End If

                                Dim grpbyacct1 = From gpbyrow In grpHead.AsEnumerable() Group gpbyrow By g = New With {Key .acctname1 = gpbyrow(20)} Into Group

                                For Each keyacc1 In grpbyacct1
                                    grpby2 = keysh.Group.CopyToDataTable

                                    normalfontbold = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK)

                                    Dim level0 = grpby2.Compute("SUM(amount)", "state>=1")

                                    If IsDBNull(level0) Then
                                        sumlevel = 0.0

                                    Else
                                        sumlevel = Convert.ToDecimal(level0).ToString(decno)

                                    End If
                                    ' Dim code = grpby2.Select("acctcode1")
                                    Dim code = grpby2.AsEnumerable().Select(Function(s) s(19)).FirstOrDefault()
                                    arrData = {keyacc1.g.acctname1, code.ToString(), sumlevel.ToString(decno)}

                                    For i = 0 To arrData.Length - 1
                                        phrase = New Phrase()
                                        phrase.Add(New Chunk(arrData(i), normalfontbold))
                                        If i = arrData.Length - 1 Then
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                        Else
                                            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                        End If
                                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                        ' cell.Colspan = 4
                                        cell.SetLeading(12, 0)
                                        cell.PaddingBottom = 4.0F
                                        cell.PaddingTop = 1.0F
                                        cell.BackgroundColor = bgColor
                                        cell.BorderWidthBottom = 0
                                        tableData.AddCell(cell)
                                    Next



                                    Dim group2 = From gpby2 In grpby2.AsEnumerable() Group gpby2 By g = New With {Key .gpby2name = gpby2(11)} Into Group
                                    For Each key2 In group2
                                        ' grp2 = key2.g.gpby2name
                                        grpby3 = key2.Group.CopyToDataTable

                                        Dim level1 = grpby3.Compute("SUM(amount)", "state>=2")

                                        If IsDBNull(level1) Then
                                            sumlevel = 0.0
                                            sumlevel1 = 0.0
                                        Else
                                            sumlevel = Convert.ToDecimal(level1)
                                            sumlevel1 = Convert.ToDecimal(level1)

                                        End If
                                        ' code = grpby2.Select("acctcode2")
                                        code = grpby3.AsEnumerable().Select(Function(s) s(1)).FirstOrDefault()
                                        normalfontbold = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)
                                        arrData = {key2.g.gpby2name, code.ToString(), sumlevel.ToString(decno)}
                                        For i = 0 To arrData.Length - 1
                                            phrase = New Phrase()
                                            phrase.Add(New Chunk(arrData(i), normalfontbold))
                                            If i = arrData.Length - 1 Then
                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                            Else
                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                            End If
                                            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                            ' cell.Colspan = 4
                                            cell.SetLeading(12, 0)
                                            cell.PaddingBottom = 4.0F
                                            cell.PaddingTop = 1.0F
                                            cell.BackgroundColor = bgColor
                                            cell.BorderWidthBottom = 0
                                            tableData.AddCell(cell)
                                        Next



                                        Dim group3 = From gpby3 In grpby3.AsEnumerable() Group gpby3 By g = New With {Key .gpby3name = gpby3(12)} Into Group
                                        For Each key3 In group3
                                            grpby3 = key3.Group.CopyToDataTable
                                            normalfontbold = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLACK)

                                            Dim level2 = grpby3.Compute("SUM(amount)", "state>=3")

                                            If IsDBNull(level2) Then
                                                sumlevel = 0.0
                                            Else
                                                sumlevel = Convert.ToDecimal(level2)

                                            End If
                                            code = grpby3.AsEnumerable().Select(Function(s) s(2)).FirstOrDefault()

                                            arrData = {key3.g.gpby3name, code.ToString(), sumlevel.ToString(decno)}
                                            For i = 0 To arrData.Length - 1
                                                phrase = New Phrase()
                                                phrase.Add(New Chunk(arrData(i), normalfontbold))
                                                If i = arrData.Length - 1 Then
                                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                                Else
                                                    cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                                End If
                                                cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                                ' cell.Colspan = 4
                                                cell.SetLeading(12, 0)
                                                cell.PaddingBottom = 4.0F
                                                cell.PaddingTop = 1.0F
                                                cell.BorderWidthBottom = 0
                                                tableData.AddCell(cell)
                                            Next

                                            Dim group4 = From gpby4 In grpby3.AsEnumerable() Group gpby4 By g = New With {Key .gpby4name = gpby4(13)} Into Group
                                            For Each key4 In group4
                                                grpby3 = key4.Group.CopyToDataTable

                                                Dim level3 = grpby3.Compute("SUM(amount)", "state>=4")

                                                If IsDBNull(level3) Then
                                                    sumlevel = 0.0
                                                Else
                                                    sumlevel = Convert.ToDecimal(level3)

                                                End If
                                                code = grpby3.AsEnumerable().Select(Function(s) s(3)).FirstOrDefault()

                                                arrData = {key4.g.gpby4name, code.ToString(), sumlevel.ToString(decno)}

                                                normalfontbold = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.BLACK)

                                                For i = 0 To arrData.Length - 1
                                                    phrase = New Phrase()
                                                    phrase.Add(New Chunk(arrData(i), normalfontbold))
                                                    If i = arrData.Length - 1 Then
                                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                                    Else
                                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                                    End If
                                                    cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                                    ' cell.Colspan = 4
                                                    cell.SetLeading(12, 0)
                                                    cell.PaddingBottom = 4.0F
                                                    cell.PaddingTop = 1.0F
                                                    cell.BorderWidthBottom = 0
                                                    tableData.AddCell(cell)
                                                Next
                                                Dim group5 = From gpby5 In grpby3.AsEnumerable() Group gpby5 By g = New With {Key .gpby5name = gpby5(14)} Into Group
                                                For Each key5 In group5
                                                    grpby3 = key5.Group.CopyToDataTable
                                                    Dim level4 = grpby3.Compute("SUM(amount)", "state>=4")

                                                    If IsDBNull(level4) Then
                                                        sumlevel = 0.0
                                                    Else
                                                        sumlevel = Convert.ToDecimal(level4)

                                                    End If
                                                    code = grpby3.AsEnumerable().Select(Function(s) s(4)).FirstOrDefault()
                                                    If code.ToString <> "" Then
                                                        arrData = {key5.g.gpby5name, code.ToString(), sumlevel.ToString(decno)}

                                                        normalfontbold = FontFactory.GetFont("Arial", 7, Font.NORMAL, BaseColor.BLACK)

                                                        For i = 0 To arrData.Length - 1
                                                            phrase = New Phrase()
                                                            phrase.Add(New Chunk(arrData(i), normalfontbold))
                                                            If i = arrData.Length - 1 Then
                                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                                            Else
                                                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                                            End If
                                                            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT
                                                            ' cell.Colspan = 4
                                                            cell.SetLeading(12, 0)
                                                            cell.PaddingBottom = 4.0F
                                                            cell.PaddingTop = 1.0F
                                                            cell.BorderWidthBottom = 0
                                                            tableData.AddCell(cell)
                                                        Next
                                                    End If
                                                Next
                                            Next
                                        Next
                                    Next
                                Next
                                normalfontbold = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLACK)

                                Dim bgColor1 As BaseColor = New BaseColor(179, 179, 179)

                                arrHeader = {GropName, sumlevel1.ToString(decno)}

                                For i = 0 To arrHeader.Length - 1
                                    phrase = New Phrase()
                                    phrase.Add(New Chunk(arrHeader(i), normalfontbold))
                                    If i = 0 Then
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                                        cell.Colspan = 2
                                    Else
                                        cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                    End If
                                    'cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                                    cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                                    cell.SetLeading(12, 0)
                                    cell.PaddingBottom = 4.0F
                                    cell.PaddingTop = 1.0F
                                    cell.PaddingLeft = 1.0F
                                    cell.BackgroundColor = bgColor1
                                    If i = 0 Then
                                        cell.BorderWidthRight = 0
                                    ElseIf i = arrHeader.Length - 1 Then
                                        cell.BorderWidthLeft = 0
                                    Else
                                        cell.BorderWidthRight = 0
                                        cell.BorderWidthLeft = 0
                                    End If
                                    tableData.AddCell(cell)
                                Next


                            Next
                        Next
                        normalfontbold = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLACK)
                        bgColor = New BaseColor(217, 217, 217)

                        ' Dim Profit As Decimal
                        Dim Profit = custdetailsdt.AsEnumerable().Select(Function(s) s(16)).FirstOrDefault()

                        If Not IsDBNull(Profit) Then
                            Profit = Convert.ToDecimal(Profit)
                        Else
                            Profit = 0.0
                        End If


                        arrHeader = {"NET PROFIT FOR THE PERIOD", IIf(Profit < 0, "(" & (Math.Abs(Decimal.Parse(Profit))).ToString(decno) & ")", Decimal.Parse(Profit).ToString(decno))}

                        For i = 0 To arrHeader.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrHeader(i), normalfontbold))
                            If i = 0 Then
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            Else

                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            End If

                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.PaddingLeft = 1.0F
                            cell.BackgroundColor = bgColor
                            If i = 0 Then
                                cell.BorderWidthRight = 0
                                cell.Colspan = 2
                            ElseIf i = arrHeader.Length - 1 Then
                                cell.BorderWidthLeft = 0
                            Else
                                cell.BorderWidthRight = 0
                                cell.BorderWidthLeft = 0
                            End If
                            tableData.AddCell(cell)
                        Next

                        Dim Equity = custdetailsdt.AsEnumerable().Select(Function(s) s(17)).FirstOrDefault()

                        If Not IsDBNull(Equity) Then
                            EquityTot = Convert.ToDecimal(Equity) + Profit
                        Else
                            EquityTot = Profit
                        End If

                        arrHeader = {"Total Equity", IIf(EquityTot < 0, "(" & Math.Abs(Decimal.Parse(EquityTot)).ToString(decno) & ")", Decimal.Parse(EquityTot).ToString(decno))}

                        For i = 0 To arrHeader.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrHeader(i), normalfontbold))
                            If i = 0 Then
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            Else

                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            End If

                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.PaddingLeft = 1.0F
                            cell.BackgroundColor = bgColor
                            If i = 0 Then
                                cell.BorderWidthRight = 0
                                cell.Colspan = 2
                            ElseIf i = arrHeader.Length - 1 Then
                                cell.BorderWidthLeft = 0
                            Else
                                cell.BorderWidthRight = 0
                                cell.BorderWidthLeft = 0
                            End If
                            tableData.AddCell(cell)
                        Next

                        Liabilitiestot = custdetailsdt.AsEnumerable().Select(Function(s) s(18)).FirstOrDefault()

                        If Not IsDBNull(Liabilitiestot) Then
                            Liabilitiestot = Convert.ToDecimal(Liabilitiestot) + EquityTot

                        Else
                            Liabilitiestot = EquityTot
                        End If
                        arrHeader = {"Total Liabilities + Total Equity", IIf(Liabilitiestot < 0, "(" & Math.Abs(Decimal.Parse(Liabilitiestot)).ToString(decno) & ")", Decimal.Parse(Liabilitiestot).ToString(decno))}

                        For i = 0 To arrHeader.Length - 1
                            phrase = New Phrase()
                            phrase.Add(New Chunk(arrHeader(i), normalfontbold))
                            If i = 0 Then
                                cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT, 1, True)
                            Else

                                cell = PhraseCell(phrase, PdfPCell.ALIGN_RIGHT, 1, True)
                            End If

                            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT
                            cell.SetLeading(12, 0)
                            cell.PaddingBottom = 4.0F
                            cell.PaddingTop = 1.0F
                            cell.PaddingLeft = 1.0F
                            cell.BackgroundColor = bgColor
                            If i = 0 Then
                                cell.BorderWidthRight = 0
                                cell.Colspan = 2
                            ElseIf i = arrHeader.Length - 1 Then
                                cell.BorderWidthLeft = 0
                            Else
                                cell.BorderWidthRight = 0
                                cell.BorderWidthLeft = 0
                            End If
                            tableData.AddCell(cell)
                        Next
                    End If
                    tableData.Complete = True
                    document.Add(tableData)
                    document.AddTitle(rptreportname)
                    document.Close()

                    If printMode = "download" Then
                        Dim pagingFont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)
                        Dim reader As New PdfReader(memoryStream.ToArray())
                        Using mStream As New MemoryStream()
                            Using stamper As New PdfStamper(reader, mStream)
                                Dim pages As Integer = reader.NumberOfPages
                                For i As Integer = 1 To pages
                                    ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, New Phrase("Page " + i.ToString() + " of " + pages.ToString(), pagingFont), IIf(documentWidth = 550.0F, documentWidth + 25, documentWidth + 35), 10.0F, 0)
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
#Region "ExcelReport"
    Public Sub ExcelReport(ByVal custdetailsdt As DataTable, ByVal reportfilter As String, ByVal currency As String, ByVal rptreportname As String, ByRef bytes() As Byte)
        Dim arrHeaders() As String
        Dim wb As New XLWorkbook
        Dim decimalPoint1
        If decpt = 2 Then
            decimalPoint = "#,##0.00"
            decimalPoint1 = "(#,##0.00)"
        ElseIf decpt = 3 Then
            decimalPoint = "#,##0.000"
            decimalPoint1 = "(#,##0.000)"
        ElseIf decpt = 4 Then
            decimalPoint = "#,##0.0000"
            decimalPoint1 = "(#,##0.0000)"
        End If

        Dim ws = wb.Worksheets.Add("Balance Sheet")
        ws.Columns.AdjustToContents()
        ws.Column("A").Width = 12
        ws.Column("B").Width = 35
        ws.Column("C").Width = 18
        Dim rownum As Integer = 6

        'Comapny Name Heading
        ws.Cell("A2").Value = rptcompanyname
        Dim company = ws.Range("A2:C2").Merge()
        company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
        company.Style.Font.SetBold().Font.FontSize = 15
        ' company.Style.Font.FontColor = XLColor.
        company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
        ws.Range("A2:C2").Merge()

        'Report Name Heading
        ws.Cell("A3").Value = rptreportname
        Dim report = ws.Range("A3:C3").Merge()
        report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
        report.Style.Font.SetBold().Font.FontSize = 14
        ' report.Style.Font.FontColor = XLColor.White
        report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

        Dim filter = ws.Range("A4:C4").Merge()
        filter.Style.Font.SetBold().Font.FontSize = 12
        filter.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontColor = XLColor.Black
        filter.Cell(1, 1).Value = reportfilter

        If custdetailsdt.Rows.Count > 0 Then
            Dim GropName As String = ""
            Dim sumlevel, sumlevel1 As Decimal
            Dim group1 = From gpbyrow In custdetailsdt.AsEnumerable() Group gpbyrow By g = New With {Key .gpbyname = gpbyrow(21)} Into Group Order By g.gpbyname
            For Each keys In group1

                grpHead = keys.Group.CopyToDataTable
                Dim groupHead = From gpbyrow In custdetailsdt.AsEnumerable() Group gpbyrow By g = New With {Key .gpHead = gpbyrow(8), Key .gpgrophead = gpbyrow(10)} Into Group

                For Each keysh In groupHead

                    grpHead = keysh.Group.CopyToDataTable

                    If keysh.g.gpgrophead = 4 Then
                        GropName = "Total Assets"

                    ElseIf keysh.g.gpgrophead = 5 Then
                        GropName = "Total Liabilities "
                    ElseIf keysh.g.gpgrophead = 6 Then
                        GropName = "Total Partner Equity "

                    End If

                    Dim grpbyacct1 = From gpbyrow In grpHead.AsEnumerable() Group gpbyrow By g = New With {Key .acctname1 = gpbyrow(20)} Into Group

                    For Each keyacc1 In grpbyacct1
                        grpby2 = keysh.Group.CopyToDataTable

                        normalfontbold = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK)

                        Dim level0 = grpby2.Compute("SUM(amount)", "state>=1")

                        If IsDBNull(level0) Then
                            sumlevel = 0.0

                        Else
                            sumlevel = Convert.ToDecimal(level0).ToString(decno)

                        End If
                        ' Dim code = grpby2.Select("acctcode1")
                        Dim code = grpby2.AsEnumerable().Select(Function(s) s(19)).FirstOrDefault()
                        arrHeaders = {keyacc1.g.acctname1, code.ToString(), sumlevel.ToString(decno)}

                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 12
                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#d9d9d9")).Alignment.WrapText = True
                        ws.Cell(rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)

                        For i = 0 To arrHeaders.Length - 1
                            If i = arrHeaders.Length - 1 Then
                                ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrHeaders(i))
                                ws.Cell(rownum, i + 1).Style.NumberFormat.Format = decimalPoint
                                ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                            Else
                                ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                                ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            End If

                        Next
                        Dim group2 = From gpby2 In grpby2.AsEnumerable() Group gpby2 By g = New With {Key .gpby2name = gpby2(11)} Into Group
                        For Each key2 In group2
                            ' grp2 = key2.g.gpby2name
                            grpby3 = key2.Group.CopyToDataTable

                            Dim level1 = grpby3.Compute("SUM(amount)", "state>=2")

                            If IsDBNull(level1) Then
                                sumlevel = 0.0
                                sumlevel1 = 0.0
                            Else
                                sumlevel = Convert.ToDecimal(level1)
                                sumlevel1 = Convert.ToDecimal(level1)

                            End If
                            ' code = grpby2.Select("acctcode2")
                            code = grpby3.AsEnumerable().Select(Function(s) s(1)).FirstOrDefault()
                            normalfontbold = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)
                            arrHeaders = {key2.g.gpby2name, code.ToString(), sumlevel.ToString(decno)}
                            rownum = rownum + 1
                            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 11
                            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#d9d9d9")).Alignment.WrapText = True
                            ws.Cell(rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            ' ws.Cell(rownum, 1).Value = grp
                            ' ws.Range("A" & rownum & ":D" & rownum).Merge()
                            For i = 0 To arrHeaders.Length - 1
                                If i = arrHeaders.Length - 1 Then
                                    ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrHeaders(i))
                                    ws.Cell(rownum, i + 1).Style.NumberFormat.Format = decimalPoint
                                    ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                                Else
                                    ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                                    ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                End If

                            Next


                            Dim group3 = From gpby3 In grpby3.AsEnumerable() Group gpby3 By g = New With {Key .gpby3name = gpby3(12)} Into Group
                            For Each key3 In group3
                                grpby3 = key3.Group.CopyToDataTable
                                normalfontbold = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLACK)

                                Dim level2 = grpby3.Compute("SUM(amount)", "state>=3")

                                If IsDBNull(level2) Then
                                    sumlevel = 0.0
                                Else
                                    sumlevel = Convert.ToDecimal(level2)

                                End If
                                code = grpby3.AsEnumerable().Select(Function(s) s(2)).FirstOrDefault()

                                arrHeaders = {key3.g.gpby3name, code.ToString(), sumlevel.ToString(decno)}

                                rownum = rownum + 1
                                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 10
                                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
                                ws.Cell(rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                ' ws.Cell(rownum, 1).Value = grp
                                ' ws.Range("A" & rownum & ":D" & rownum).Merge()
                                For i = 0 To arrHeaders.Length - 1
                                    If i = arrHeaders.Length - 1 Then
                                        ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrHeaders(i))
                                        ws.Cell(rownum, i + 1).Style.NumberFormat.Format = decimalPoint
                                        ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                                    Else
                                        ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                                        ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                    End If

                                Next
                                Dim group4 = From gpby4 In grpby3.AsEnumerable() Group gpby4 By g = New With {Key .gpby4name = gpby4(13)} Into Group
                                For Each key4 In group4
                                    grpby3 = key4.Group.CopyToDataTable

                                    Dim level3 = grpby3.Compute("SUM(amount)", "state>=4")

                                    If IsDBNull(level3) Then
                                        sumlevel = 0.0
                                    Else
                                        sumlevel = Convert.ToDecimal(level3)

                                    End If
                                    code = grpby3.AsEnumerable().Select(Function(s) s(3)).FirstOrDefault()

                                    arrHeaders = {key4.g.gpby4name, code.ToString(), sumlevel.ToString(decno)}

                                    rownum = rownum + 1
                                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 9
                                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
                                    ws.Cell(rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                    ' ws.Cell(rownum, 1).Value = grp
                                    ' ws.Range("A" & rownum & ":D" & rownum).Merge()
                                    For i = 0 To arrHeaders.Length - 1
                                        If i = arrHeaders.Length - 1 Then
                                            ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrHeaders(i))
                                            ws.Cell(rownum, i + 1).Style.NumberFormat.Format = decimalPoint
                                            ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                                        Else
                                            ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                                            ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                        End If

                                    Next

                                    Dim group5 = From gpby5 In grpby3.AsEnumerable() Group gpby5 By g = New With {Key .gpby5name = gpby5(14)} Into Group
                                    For Each key5 In group5
                                        grpby3 = key5.Group.CopyToDataTable
                                        Dim level4 = grpby3.Compute("SUM(amount)", "state>=4")

                                        If IsDBNull(level4) Then
                                            sumlevel = 0.0
                                        Else
                                            sumlevel = Convert.ToDecimal(level4)

                                        End If
                                        code = grpby3.AsEnumerable().Select(Function(s) s(4)).FirstOrDefault()

                                        arrHeaders = {key5.g.gpby5name, code.ToString(), sumlevel.ToString(decno)}

                                        rownum = rownum + 1
                                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.FontSize = 9
                                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
                                        ws.Cell(rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)

                                        For i = 0 To arrHeaders.Length - 1
                                            If i = arrHeaders.Length - 1 Then
                                                ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrHeaders(i))
                                                ws.Cell(rownum, i + 1).Style.NumberFormat.Format = decimalPoint
                                                ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                                            Else
                                                ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                                                ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                                            End If

                                        Next
                                    Next
                                Next
                            Next
                        Next
                    Next

                    arrHeader = {GropName, sumlevel1.ToString(decno)}
                    rownum = rownum + 1
                    ws.Range(rownum, 1, rownum, arrHeader.Length + 1).Style.Font.SetBold().Font.FontSize = 10
                    ws.Range(rownum, 1, rownum, arrHeader.Length + 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    ws.Range(rownum, 1, rownum, arrHeader.Length + 1).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#bfbfbf")).Alignment.WrapText = True
                    ws.Cell(rownum, 1).Style.Alignment.SetVertical(XLAlignmentHorizontalValues.Center)

                    For i = 0 To arrHeader.Length - 1
                        If i = arrHeader.Length - 1 Then
                            ws.Cell(rownum, i + 2).Value = Decimal.Parse(arrHeader(i))
                            ws.Cell(rownum, i + 2).Style.NumberFormat.Format = decimalPoint
                            ws.Cell(rownum, i + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                        Else
                            ws.Cell(rownum, i + 1).Value = arrHeader(i)
                            ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            ws.Range(rownum, i + 1, rownum, i + 2).Merge()
                        End If

                    Next
                Next
            Next
            normalfontbold = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLACK)
            bgColor = New BaseColor(217, 217, 217)

            Dim Profit = custdetailsdt.AsEnumerable().Select(Function(s) s(16)).FirstOrDefault()

            If Not IsDBNull(Profit) Then
                Profit = Convert.ToDecimal(Profit)
            Else
                Profit = 0.0
            End If


            arrHeader = {"NET PROFIT FOR THE PERIOD", Decimal.Parse(Profit).ToString(decno)}

            rownum = rownum + 1
            ws.Range(rownum, 1, rownum, arrHeader.Length + 1).Style.Font.SetBold().Font.FontSize = 10
            ws.Range(rownum, 1, rownum, arrHeader.Length + 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
            ws.Range(rownum, 1, rownum, arrHeader.Length + 1).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#b3b3b3")).Alignment.WrapText = True
            ws.Cell(rownum, 1).Style.Alignment.SetVertical(XLAlignmentHorizontalValues.Center)

            For i = 0 To arrHeader.Length - 1
                If i = arrHeader.Length - 1 Then

                    If arrHeader(i) < 0 Then
                        ws.Cell(rownum, 3).Value = Math.Abs(Decimal.Parse(arrHeader(i)))
                        ws.Cell(rownum, 3).Style.NumberFormat.Format = decimalPoint1
                        ws.Cell(rownum, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    Else
                        ws.Cell(rownum, 3).Value = Decimal.Parse(arrHeader(i))
                        ws.Cell(rownum, 3).Style.NumberFormat.Format = decimalPoint
                        ws.Cell(rownum, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    End If
                Else
                    ws.Cell(rownum, i + 1).Value = arrHeader(i)
                    ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    ws.Range(rownum, 1, rownum, 2).Merge()
                End If

            Next

            Dim Equity = custdetailsdt.AsEnumerable().Select(Function(s) s(17)).FirstOrDefault()

            If Not IsDBNull(Equity) Then
                EquityTot = Convert.ToDecimal(Equity) + Profit
            Else
                EquityTot = Profit
            End If
            arrHeader = {"Total Equity", Decimal.Parse(EquityTot).ToString(decno)}

            rownum = rownum + 1
            ws.Range(rownum, 1, rownum, arrHeader.Length + 1).Style.Font.SetBold().Font.FontSize = 10
            ws.Range(rownum, 1, rownum, arrHeader.Length + 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
            ws.Range(rownum, 1, rownum, arrHeader.Length + 1).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#b3b3b3")).Alignment.WrapText = True
            ws.Cell(rownum, 1).Style.Alignment.SetVertical(XLAlignmentHorizontalValues.Center)

            For i = 0 To arrHeader.Length - 1
                If i = arrHeader.Length - 1 Then
                    If arrHeader(i) < 0 Then
                        ws.Cell(rownum, 3).Value = Math.Abs(Decimal.Parse(arrHeader(i)))
                        ws.Cell(rownum, 3).Style.NumberFormat.Format = decimalPoint1
                        ws.Cell(rownum, i + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    Else
                        ws.Cell(rownum, i + 2).Value = Decimal.Parse(arrHeader(i))
                        ws.Cell(rownum, i + 2).Style.NumberFormat.Format = decimalPoint
                        ws.Cell(rownum, i + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    End If
                Else
                    ws.Cell(rownum, i + 1).Value = arrHeader(i)
                    ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    ws.Range(rownum, i + 1, rownum, i + 2).Merge()
                End If

            Next


            Dim liability = custdetailsdt.AsEnumerable().Select(Function(s) s(18)).FirstOrDefault()

            If Not IsDBNull(liability) Then
                Liabilitiestot = Convert.ToDecimal(liability) + EquityTot

            Else
                Liabilitiestot = EquityTot
            End If


            arrHeader = {"Total Liabilities + Total Equity", Decimal.Parse(Liabilitiestot).ToString(decno)}

            rownum = rownum + 1
            ws.Range(rownum, 1, rownum, arrHeader.Length + 1).Style.Font.SetBold().Font.FontSize = 10
            ws.Range(rownum, 1, rownum, arrHeader.Length + 1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
            ws.Range(rownum, 1, rownum, arrHeader.Length + 1).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#b3b3b3")).Alignment.WrapText = True
            ws.Cell(rownum, 1).Style.Alignment.SetVertical(XLAlignmentHorizontalValues.Center)

            For i = 0 To arrHeader.Length - 1
                If i = arrHeader.Length - 1 Then

                    If arrHeader(i) < 0 Then
                        ws.Cell(rownum, 3).Value = Math.Abs(Decimal.Parse(arrHeader(i)))
                        ws.Cell(rownum, 3).Style.NumberFormat.Format = decimalPoint1
                        ws.Cell(rownum, i + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                    Else
                        ws.Cell(rownum, i + 2).Value = Decimal.Parse(arrHeader(i))
                        ws.Cell(rownum, i + 2).Style.NumberFormat.Format = decimalPoint
                        ws.Cell(rownum, i + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    End If
                Else
                    ws.Cell(rownum, i + 1).Value = arrHeader(i)
                    ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    ws.Range(rownum, i + 1, rownum, i + 2).Merge()
                End If

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
    '#Region "ExcelReport"
    '    Public Sub ExcelReport(ByVal custdetailsdt As DataTable, ByVal reportfilter As String, ByVal currency As String, ByVal rptreportname As String, ByVal acctmajorsub As DataTable, ByRef bytes() As Byte)
    '        Dim arrHeaders() As String
    '        Dim wb As New XLWorkbook
    '        Dim ws = wb.Worksheets.Add("Balance Sheet")
    '        ws.Columns.AdjustToContents()
    '        ws.Columns("A:D").Width = 25
    '        Dim rownum As Integer = 6

    '        'Comapny Name Heading
    '        ws.Cell("A2").Value = rptcompanyname
    '        Dim company = ws.Range("A2:D2").Merge()
    '        company.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0048C0")).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
    '        company.Style.Font.FontSize = 15
    '        company.Style.Font.FontColor = XLColor.White
    '        company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
    '        company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
    '        ws.Range("A2:D2").Merge()

    '        'Report Name Heading
    '        ws.Cell("A3").Value = rptreportname
    '        Dim report = ws.Range("A3:D3").Merge()
    '        report.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0000C0")).Border.SetOutsideBorder(XLBorderStyleValues.Thin)
    '        report.Style.Font.FontSize = 14
    '        report.Style.Font.FontColor = XLColor.White
    '        report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
    '        report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

    '        Dim filter = ws.Range("A4:D4").Merge()
    '        filter.Style.Font.SetBold().Font.FontSize = 12
    '        filter.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontColor = XLColor.Black
    '        filter.Cell(1, 1).Value = reportfilter
    '        If custdetailsdt.Rows.Count > 0 Then

    '            arrHeaders = {"", "", "Amount (" + currency + ")", "Total Amount (" + currency + ")"}
    '            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 10
    '            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
    '            For i = 0 To arrHeaders.Length - 1
    '                If i = 2 Then
    '                    ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                Else
    '                    ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
    '                End If
    '                If i = 0 Then
    '                    ws.Cell(rownum, i + 1).Style.Border.SetRightBorder(XLBorderStyleValues.None)
    '                ElseIf i = arrHeaders.Length - 1 Then
    '                    ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None)
    '                Else
    '                    ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None)
    '                End If
    '                ws.Cell(rownum, i + 1).Value = arrHeaders(i)
    '            Next

    '            group2name = ""
    '            group3name = ""
    '            Dim k As Integer = 0
    '            Dim Print As Boolean = False
    '            custdetailsdt.DefaultView.Sort = "group1code ASC"
    '            custdetailsdt = custdetailsdt.DefaultView.ToTable

    '            acctmajor = custdetailsdt.Select("group1name<>'" & Nothing & "'")

    '            'custdetailsdt = a
    '            Dim group1 = From gpbyrow In acctmajor.AsEnumerable() Group gpbyrow By g = New With {Key .gpby1code = gpbyrow.Field(Of String)("group1code"), Key .gpby1name = gpbyrow.Field(Of String)("group1name")} Into Group Order By g.gpby1code
    '            For Each keys In group1
    '                grp = keys.g.gpby1code + " " + keys.g.gpby1name
    '                grp1 = keys.g.gpby1name
    '                rownum = rownum + 1
    '                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 10
    '                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
    '                ws.Cell(rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
    '                ws.Cell(rownum, 1).Value = grp
    '                ws.Range("A" & rownum & ":D" & rownum).Merge()
    '                ' ws.Range(rownum, 5).Style.Border.SetBottomBorder(XLBorderStyleValues.None)

    '                grpby2 = keys.Group.CopyToDataTable
    '                Dim group2 = From gpby2 In grpby2.AsEnumerable() Group gpby2 By g = New With {Key .gpby2code = gpby2.Field(Of String)("group2code"), Key .gpby2name = gpby2.Field(Of String)("group2name")} Into Group Order By g.gpby2code
    '                For Each key2 In group2
    '                    grp2 = key2.g.gpby2name
    '                    grpby3 = key2.Group.CopyToDataTable
    '                    Dim group3 = From gpby3 In grpby3.AsEnumerable() Group gpby3 By g = New With {Key .gpby3code = gpby3.Field(Of String)("group3code"), Key .gpby3name = gpby3.Field(Of String)("group3name")} Into Group Order By g.gpby3code
    '                    For Each key3 In group3
    '                        grp3 = key3.g.gpby3name
    '                        grpby4 = key3.Group.CopyToDataTable

    '                        Dim group4 = From gpby4 In grpby4.AsEnumerable() Group gpby4 By g = New With {Key .gpby4code = gpby4.Field(Of String)("group4code"), Key .gpby4name = gpby4.Field(Of String)("group4name")} Into Group Order By g.gpby4code
    '                        For Each key4 In group4
    '                            grp4 = key4.g.gpby4name
    '                            For Each row In key4.Group
    '                                grpby4 = key4.Group.CopyToDataTable
    '                                acctmajor = acctmajorsub.Select("acctcode='" & row("acc_code").ToString() & "' AND div_code='" & row("div_id").ToString() & "'")

    '                                If Not (TypeOf acctmajor(0)("acctsubname") Is DBNull) Then
    '                                    Print = True
    '                                    subnametotal = subnametotal + Decimal.Parse(row("closingbalance"))
    '                                    If Not group2name.Equals(grp2) Then
    '                                        If Not group3name.Equals(grp3) Then
    '                                            arrHeader = {acctmajor(0)("acctsubname").ToString(), grp2, grp3, grp4}
    '                                        Else
    '                                            arrHeader = {acctmajor(0)("acctsubname").ToString(), grp2, grp4}
    '                                        End If
    '                                    Else
    '                                        If Not group3name.Equals(grp3) Then
    '                                            arrHeader = {acctmajor(0)("acctsubname").ToString(), grp2, grp3, grp4}
    '                                        Else
    '                                            arrHeader = {acctmajor(0)("acctsubname").ToString(), grp4}
    '                                        End If
    '                                    End If
    '                                Else
    '                                    If Not group2name.Equals(grp2) Then
    '                                        If Not group3name.Equals(grp3) Then
    '                                            arrHeader = {grp2, grp3, grp4}
    '                                        Else
    '                                            arrHeader = {grp2, grp4}
    '                                        End If
    '                                    Else
    '                                        If Not group3name.Equals(grp3) Then
    '                                            arrHeader = {grp3, grp4}
    '                                        Else
    '                                            arrHeader = {grp4}
    '                                        End If
    '                                    End If
    '                                End If
    '                                group2name = grp2
    '                                group3name = grp3

    '                                If k = 0 Then
    '                                    For i = 0 To arrHeader.Length - 1
    '                                        space = space + "    "
    '                                        rownum = rownum + 1
    '                                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 10
    '                                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.None).Border.SetTopBorder(XLBorderStyleValues.None).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '                                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
    '                                        ws.Cell(rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
    '                                        ws.Cell(rownum, 1).Value = space & arrHeader(i)
    '                                        ws.Range("A" & rownum & ":D" & rownum).Merge()
    '                                    Next
    '                                    k = k + 1
    '                                End If
    '                                total = total + Decimal.Parse(row("closingbalance"))
    '                                arrHeader = {row("acct_name").ToString, Decimal.Parse(row("closingbalance")).ToString(decno), " "}
    '                                rownum = rownum + 1
    '                                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.FontSize = 10
    '                                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '                                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
    '                                For i = 0 To arrHeader.Length - 1
    '                                    If i = 0 Then
    '                                        ws.Cell(rownum, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
    '                                        ws.Range("A" & rownum & ":B" & rownum).Merge()
    '                                        ws.Cell(rownum, 2).Style.Border.SetRightBorder(XLBorderStyleValues.None)
    '                                        ws.Cell(rownum, 1).Value = space & arrHeader(i)
    '                                    ElseIf i = arrHeader.Length - 1 Then
    '                                        ws.Cell(rownum, i + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                                        ws.Cell(rownum, i + 2).Style.Border.SetLeftBorder(XLBorderStyleValues.None)
    '                                        ws.Cell(rownum, i + 2).Value = arrHeader(i)
    '                                    Else
    '                                        ws.Cell(rownum, i + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                                        ws.Cell(rownum, i + 2).Value = Decimal.Parse(arrHeader(i))
    '                                        ws.Cell(rownum, i + 2).Style.Border.SetLeftBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None)
    '                                        If decpt = 2 Then
    '                                            ws.Cell(rownum, i + 2).Style.NumberFormat.Format = "#,##0.00"
    '                                        ElseIf decpt = 3 Then
    '                                            ws.Cell(rownum, i + 2).Style.NumberFormat.Format = "#,##0.000"
    '                                        ElseIf decpt = 4 Then
    '                                            ws.Cell(rownum, i + 2).Style.NumberFormat.Format = "#,##0.0000"
    '                                        End If
    '                                    End If
    '                                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetTopBorder(XLBorderStyleValues.None)
    '                                Next
    '                            Next
    '                            space = ""
    '                            k = 0
    '                            arrHeader = {"", "Total " + grp4, "", Decimal.Parse(total).ToString(decno)}
    '                            rownum = rownum + 1
    '                            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 10
    '                            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '                            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
    '                            For i = 0 To arrHeader.Length - 1
    '                                ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                                If i = 0 Then
    '                                    ws.Cell(rownum, i + 1).Style.Border.SetRightBorder(XLBorderStyleValues.None)
    '                                    ws.Cell(rownum, i + 1).Value = arrHeader(i)
    '                                ElseIf i = arrHeader.Length - 1 Then
    '                                    ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None)
    '                                    ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrHeader(i))
    '                                    If decpt = 2 Then
    '                                        ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.00"
    '                                    ElseIf decpt = 3 Then
    '                                        ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.000"
    '                                    ElseIf decpt = 4 Then
    '                                        ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.0000"
    '                                    End If
    '                                Else
    '                                    ws.Cell(rownum, i + 1).Value = arrHeader(i)
    '                                    ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None)
    '                                End If
    '                                ws.Cell(rownum, i + 1).Style.Border.SetTopBorder(XLBorderStyleValues.None).Border.SetBottomBorder(XLBorderStyleValues.None)

    '                            Next
    '                            ptotal = ptotal + total
    '                            total = 0
    '                        Next
    '                        rownum = rownum + 1
    '                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 9
    '                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
    '                        arrHeader = {"", "Total " + grp3, "", Decimal.Parse(ptotal).ToString(decno)}
    '                        For i = 0 To arrHeader.Length - 1
    '                            ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                            If i = 0 Then
    '                                ws.Cell(rownum, i + 1).Style.Border.SetRightBorder(XLBorderStyleValues.None)
    '                                ws.Cell(rownum, i + 1).Value = arrHeader(i)
    '                            ElseIf i = arrHeader.Length - 1 Then
    '                                ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None)
    '                                ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrHeader(i))
    '                                If decpt = 2 Then
    '                                    ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.00"
    '                                ElseIf decpt = 3 Then
    '                                    ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.000"
    '                                ElseIf decpt = 4 Then
    '                                    ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.0000"
    '                                End If
    '                            Else
    '                                ws.Cell(rownum, i + 1).Value = arrHeader(i)
    '                                ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None)
    '                            End If
    '                            ws.Cell(rownum, i + 1).Style.Border.SetTopBorder(XLBorderStyleValues.None).Border.SetBottomBorder(XLBorderStyleValues.None)
    '                            'ws.Cell(rownum, i + 2).Value = arrHeader(i)
    '                        Next
    '                        subtotal = subtotal + ptotal
    '                        ptotal = 0
    '                    Next
    '                    rownum = rownum + 1
    '                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 9
    '                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
    '                    arrHeader = {"", "Total " + grp2, "", Decimal.Parse(subtotal).ToString(decno)}
    '                    For i = 0 To arrHeader.Length - 1
    '                        ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                        If i = 0 Then
    '                            ws.Cell(rownum, i + 1).Style.Border.SetRightBorder(XLBorderStyleValues.None)
    '                            ws.Cell(rownum, i + 1).Value = arrHeader(i)
    '                        ElseIf i = arrHeader.Length - 1 Then
    '                            ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None)
    '                            ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrHeader(i))
    '                            If decpt = 2 Then
    '                                ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.00"
    '                            ElseIf decpt = 3 Then
    '                                ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.000"
    '                            ElseIf decpt = 4 Then
    '                                ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.0000"
    '                            End If
    '                        Else
    '                            ws.Cell(rownum, i + 1).Value = arrHeader(i)
    '                            ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None)
    '                        End If
    '                        ws.Cell(rownum, i + 1).Style.Border.SetTopBorder(XLBorderStyleValues.None).Border.SetBottomBorder(XLBorderStyleValues.None)
    '                        'ws.Cell(rownum, i + 2).Value = arrHeader(i)

    '                    Next

    '                    ftotal = ftotal + subtotal
    '                    If Print Then
    '                        rownum = rownum + 1
    '                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 9
    '                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
    '                        arrHeader = {"", "Total " + acctmajor(0)("acctsubname").ToString(), "", Decimal.Parse(subnametotal).ToString(decno)}
    '                        For i = 0 To arrHeader.Length - 1
    '                            ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                            If i = 0 Then
    '                                ws.Cell(rownum, i + 1).Style.Border.SetRightBorder(XLBorderStyleValues.None)
    '                                ws.Cell(rownum, i + 1).Value = arrHeader(i)
    '                            ElseIf i = arrHeader.Length - 1 Then
    '                                ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None)
    '                                ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrHeader(i))
    '                                If decpt = 2 Then
    '                                    ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.00"
    '                                ElseIf decpt = 3 Then
    '                                    ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.000"
    '                                ElseIf decpt = 4 Then
    '                                    ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.0000"
    '                                End If
    '                            Else
    '                                ws.Cell(rownum, i + 1).Value = arrHeader(i)
    '                                ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None)
    '                            End If
    '                            'ws.Cell(rownum, i + 2).Value = arrHeader(i)
    '                        Next
    '                    End If
    '                    subtotal = 0
    '                    subnametotal = 0
    '                Next
    '                If Not Print Then
    '                    rownum = rownum + 1
    '                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 9
    '                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
    '                    arrHeader = {"", "Total " + acctmajor(0)("acctsubname").ToString(), "", Decimal.Parse(ftotal).ToString(decno)}
    '                    For i = 0 To arrHeader.Length - 1
    '                        ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                        If i = 0 Then
    '                            ws.Cell(rownum, i + 1).Style.Border.SetRightBorder(XLBorderStyleValues.None)
    '                            ws.Cell(rownum, i + 1).Value = arrHeader(i)
    '                        ElseIf i = arrHeader.Length - 1 Then
    '                            ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None)
    '                            ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrHeader(i))
    '                            If decpt = 2 Then
    '                                ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.00"
    '                            ElseIf decpt = 3 Then
    '                                ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.000"
    '                            ElseIf decpt = 4 Then
    '                                ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.0000"
    '                            End If
    '                        Else
    '                            ws.Cell(rownum, i + 1).Value = arrHeader(i)
    '                            ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None)
    '                        End If
    '                        ws.Cell(rownum, i + 1).Style.Border.SetTopBorder(XLBorderStyleValues.None)
    '                        'ws.Cell(rownum, i + 2).Value = arrHeader(i)

    '                    Next
    '                End If
    '                Print = False
    '                rownum = rownum + 1
    '                arrHeader = {"", "Total " + grp1, "", Decimal.Parse(ftotal).ToString(decno)}
    '                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 10
    '                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
    '                For i = 0 To arrHeader.Length - 1
    '                    ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                    If i = 0 Then
    '                        ws.Cell(rownum, i + 1).Style.Border.SetRightBorder(XLBorderStyleValues.None)
    '                        ws.Cell(rownum, i + 1).Value = arrHeader(i)
    '                    ElseIf i = arrHeader.Length - 1 Then
    '                        ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None)
    '                        ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrHeader(i))
    '                        If decpt = 2 Then
    '                            ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.00"
    '                        ElseIf decpt = 3 Then
    '                            ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.000"
    '                        ElseIf decpt = 4 Then
    '                            ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.0000"
    '                        End If
    '                    Else
    '                        ws.Cell(rownum, i + 1).Value = arrHeader(i)
    '                        ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None)
    '                    End If

    '                Next
    '                finaltotal = finaltotal + ftotal
    '                ftotal = 0
    '            Next
    '            rownum = rownum + 1
    '            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 10
    '            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
    '            arrHeader = {"", "Total LIABILITIES AND CAPITAL", "", Decimal.Parse(finaltotal).ToString(decno)}
    '            For i = 0 To arrHeader.Length - 1
    '                ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                If i = 0 Then
    '                    ws.Cell(rownum, i + 1).Style.Border.SetRightBorder(XLBorderStyleValues.None)
    '                    ws.Cell(rownum, i + 1).Value = arrHeader(i)
    '                ElseIf i = arrHeader.Length - 1 Then
    '                    ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None)
    '                    ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrHeader(i))
    '                    If decpt = 2 Then
    '                        ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.00"
    '                    ElseIf decpt = 3 Then
    '                        ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.000"
    '                    ElseIf decpt = 4 Then
    '                        ws.Cell(rownum, i + 1).Style.NumberFormat.Format = "#,##0.0000"
    '                    End If
    '                Else
    '                    ws.Cell(rownum, i + 1).Value = arrHeader(i)
    '                    ws.Cell(rownum, i + 1).Style.Border.SetLeftBorder(XLBorderStyleValues.None).Border.SetRightBorder(XLBorderStyleValues.None)
    '                End If

    '            Next

    '            ws.Cell((rownum + 2), 1).Value = "Printed Date : " & Now.ToString("dd/MM/yyyy")
    '            ws.Range((rownum + 2), 1, (rownum + 2), 3).Merge()
    '            Using wStream As New MemoryStream()
    '                wb.SaveAs(wStream)
    '                bytes = wStream.ToArray()
    '            End Using

    '        End If

    '    End Sub

    '#End Region




End Class
