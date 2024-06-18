Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports ClosedXML.Excel
Imports System.IO
Imports System.Net
Imports System.Linq
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class clsAirportMaExl
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
    Dim normalfont As Font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK)
    Dim headfont As Font = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLACK)
    Dim normalfont1 As Font = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK)
    Dim Companyname As Font = New Font(FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK))
    Dim ReportNamefont As Font = New Font(FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.WHITE))
    Dim Servicefont As Font = New Font(FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.RED))

    Dim documentWidth As Single = 770.0F
    Dim phrase As Phrase = Nothing
    Dim cell As PdfPCell = Nothing
    Dim titleColor As BaseColor = New BaseColor(41, 45, 134)
    Dim Airportbg As BaseColor = New BaseColor(255, 179, 102)
    Dim Databg As BaseColor = New BaseColor(255, 217, 179)

#End Region

#Region "Protected sub ExcelReport()"
    Public Sub ExcelReport(ByRef bytes() As Byte, ByVal decno As String, ByVal fromdate As Date, ByVal todate As Date, ByVal curr As String, ByVal remark As String,
                           ByVal ctrygrp As String, ByVal airport As String, ByVal srcctry As String, ByVal servicetype As String, ByVal RateType As String, ByVal Partycode As String)
        Dim sqlConn As New SqlConnection
        Dim mySqlCmd As New SqlCommand
        Dim myDataAdapter As New SqlDataAdapter
        Dim ds As New DataSet

        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add("AirportMAPriceListReport")
        sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
        mySqlCmd = New SqlCommand("sp_rep_airportmapricelist", sqlConn)
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = Format((fromdate), "yyyy/MM/dd")
        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = Format((todate), "yyyy/MM/dd")
        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroup", SqlDbType.VarChar, 10)).Value = ctrygrp
        mySqlCmd.Parameters.Add(New SqlParameter("@sourcectrycode", SqlDbType.VarChar, 10)).Value = srcctry
        mySqlCmd.Parameters.Add(New SqlParameter("@airportbordercode", SqlDbType.VarChar)).Value = airport
        mySqlCmd.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar)).Value = servicetype.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar)).Value = Partycode.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@Ratetype", SqlDbType.VarChar)).Value = RateType.Trim
        myDataAdapter.SelectCommand = mySqlCmd
        myDataAdapter.Fill(ds)
        Dim Airportmadt, dtcurrremarks As New DataTable
        Airportmadt = ds.Tables(0)
        Dim cc = Airportmadt.Columns.Count - 7
        Dim arrHeaders(cc) As String

        Dim decimalPoint1, decimalpoint As String
        If decno = 2 Then
            decimalpoint = "#,##0.00"
            decimalPoint1 = "(#,##0.00)"
        ElseIf decno = 3 Then
            decimalpoint = "#,##0.000"
            decimalPoint1 = "(#,##0.000)"
        ElseIf decno = 4 Then
            decimalpoint = "#,##0.0000"
            decimalPoint1 = "(#,##0.0000)"
        End If


        ws.Columns.AdjustToContents()
        Dim colcount = Airportmadt.Columns.Count
        ws.Column("A").Width = 50
        ws.Columns(2, cc).Width = 20
        Dim rownum As Integer = 6

        'Comapny Name Heading
        ws.Cell("A1").Value = CType(Session("CompanyName"), String)
        Dim company = ws.Range(1, 1, 1, cc).Merge()
        company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
        company.Style.Font.SetBold().Font.FontSize = 14
        company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

        ws.Cell("A2").Value = "Tariff " & (fromdate).ToString("yyyy") & "/" & (todate).ToString("yy")
        Dim report = ws.Range(2, 1, 2, cc).Merge()
        report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
        report.Style.Font.SetBold().Font.FontSize = 14
        report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

        ws.Cell("A3").Value = "EXTRA SERVICES"
        Dim report1 = ws.Range(3, 1, 3, cc).Merge()
        report1.Style.Fill.BackgroundColor = XLColor.Navy
        report1.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
        report1.Style.Font.FontColor = XLColor.White
        report1.Style.Font.SetBold().Font.FontSize = 13
        report1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        report1.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
        If RateType.Trim.ToLower = "salevalue" Then
            ws.Cell("A4").Value = "MEET AND ASSIST SERVICES SALE PRICELIST"
        ElseIf RateType.Trim.ToLower = "costvalue" Then
            ws.Cell("A4").Value = "MEET AND ASSIST SERVICES COST PRICELIST"
        End If
        Dim report2 = ws.Range(4, 1, 4, cc).Merge()
        report2.Style.Font.FontColor = XLColor.Red
        report2.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
        report2.Style.Font.SetBold().Font.FontSize = 13
        report2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        report2.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

        Dim filter = ws.Range(5, 1, 5, cc).Merge()
        filter.Style.Font.SetBold().Font.FontSize = 12
        filter.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        filter.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
        filter.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontColor = XLColor.Black
        filter.Cell(1, 1).Value = "Rates valid from " & fromdate.ToString("dd") & Space(1) & fromdate.ToString("MMM") & Space(1) & fromdate.ToString("yyyy") & " - " & todate.ToString("dd") & Space(1) & todate.ToString("MMM") & Space(1) & todate.ToString("yyyy")


        Dim grpbyairport = From grpbyair In Airportmadt.AsEnumerable() Group grpbyair By g = New With {Key .airportname = grpbyair.Field(Of String)("airportbordername")} Into Group Order By g.airportname

        For Each AirportData In grpbyairport

            Dim Tbldata1 As DataTable = AirportData.Group.CopyToDataTable
            Dim Grpbyservicetype = From grpbyser In Tbldata1.AsEnumerable() Group grpbyser By g = New With {Key .servicetype = grpbyser.Field(Of String)("servicetype")} Into Group Order By g.servicetype
            Dim c As Integer = 0
            For Each grpserviceData In Grpbyservicetype
                Dim arrCount As Integer = 0
                c += 1
                If c = 2 Then
                    rownum += 1
                    ws.Range(rownum, 1, rownum, arrHeaders.Length - 1).Merge().Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                    ws.Cell(rownum, 1).Value = ""
                End If

                arrHeaders(0) = grpserviceData.g.servicetype & " Service in " & AirportData.g.airportname
                For i = 8 To colcount - 1
                    arrCount = arrCount + 1
                    arrHeaders(arrCount) = Airportmadt.Columns(i).ColumnName
                Next
                rownum += 1
                ws.Range(rownum, 1, rownum, arrCount + 1).Style.Font.SetBold().Font.FontSize = 9
                ws.Range(rownum, 1, rownum, arrCount + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 179, 102)
                ws.Range(rownum, 1, rownum, arrCount + 1).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.WrapText = True
                ws.Range(rownum, 1, rownum, arrCount + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)

                For i = 0 To arrHeaders.Length - 2
                    ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                Next

                Dim count As Integer = -1
                Dim remarks(grpserviceData.Group.Count) As String
                Dim adultsalevalue, childsalevalue As Decimal
                Dim total() As String
                arrCount = 0
                For Each row In grpserviceData.Group

                    arrHeaders(0) = row("servicetypename").ToString()
                    For i = 8 To colcount - 1
                        arrCount = arrCount + 1
                        If Not IsDBNull(row(i)) Then
                            arrHeaders(arrCount) = row(i)
                        Else
                            arrHeaders(arrCount) = ""
                        End If
                    Next
                    If Not IsDBNull(row(9)) Then
                        childsalevalue = childsalevalue + row(9)
                    End If
                    If Not IsDBNull(row(11)) Then
                        adultsalevalue = adultsalevalue + row(11)
                    End If
                    rownum += 1
                    ws.Range(rownum, 1, rownum, arrCount + 1).Style.Font.FontSize = 9
                    ws.Range(rownum, 1, rownum, arrCount + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 217, 179)
                    ws.Range(rownum, 1, rownum, arrCount + 1).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.WrapText = True
                    ws.Range(rownum, 2, rownum, arrCount + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    ws.Range(rownum, 1, rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)

                    For i = 0 To arrHeaders.Length - 2
                        If (i = arrCount Or i = arrCount - 2) AndAlso arrHeaders(i) <> "" Then
                            ws.Cell(rownum, i + 1).Value = Convert.ToDecimal(Decimal.Parse(arrHeaders(i)))
                            ws.Cell(rownum, i + 1).Style.NumberFormat.Format = decimalpoint
                        Else
                            ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                        End If
                    Next

                    If Not IsDBNull(row("remarks")) AndAlso row("remarks") <> "" Then
                        count = count + 1
                        remarks(count) = "* " & row("remarks")
                    End If
                    arrCount = 0
                Next
                total = {"Total", "", IIf(childsalevalue = 0.0, "", childsalevalue), "", IIf(adultsalevalue = 0.0, "", adultsalevalue)}
                rownum += 1
                ws.Range(rownum, 1, rownum, 5).Style.Font.SetBold().Font.FontSize = 10
                ws.Range(rownum, 1, rownum, 5).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 217, 179)
                ws.Range(rownum, 1, rownum, 5).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.WrapText = True
                ws.Range(rownum, 2, rownum, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                ws.Range(rownum, 1, rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                For i = 0 To total.Length - 1
                    If (i = 2 Or i = 4) AndAlso total(i) <> "" Then
                        ws.Cell(rownum, i + 1).Value = Convert.ToDecimal(Decimal.Parse(total(i)))
                        ws.Cell(rownum, i + 1).Style.NumberFormat.Format = decimalpoint
                    Else
                        ws.Cell(rownum, i + 1).Value = total(i)
                    End If
                Next
                adultsalevalue = 0
                childsalevalue = 0
                If remark <> Nothing AndAlso remark <> "" Then
                    count = count + 1
                    remarks(count) = "* " & remark
                End If
                If remarks.Length > 1 Then
                    ws.Range(rownum, 1, rownum + count + 1, colcount - 7).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Alignment.SetVertical(XLAlignmentVerticalValues.Top)
                    For i = 0 To count
                        rownum += 1
                        Dim d1 As Integer = remarks(i).Length / ((colcount - 7) * 11)
                        If d1 = 1 Then
                            ws.Row(rownum).Height = 6
                        ElseIf d1 = 2 Then
                            ws.Row(rownum).Height = 12
                        ElseIf d1 = 3 Then
                            ws.Row(rownum).Height = 18
                        ElseIf d1 = 4 Then
                            ws.Row(rownum).Height = 24
                        ElseIf d1 = 5 Then
                            ws.Row(rownum).Height = 30
                        End If
                        ws.Range(rownum, 1, rownum, colcount - 7).Merge()
                        ws.Range(rownum, 1, rownum, colcount - 7).Style.Font.FontSize = 9
                        ws.Cell(rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        ws.Cell(rownum, 1).Value = remarks(i)
                    Next
                End If
            Next
            rownum += 1
            ws.Range(rownum, 1, rownum, arrHeaders.Length).Merge()
            ws.Cell(rownum, 1).Value = ""
            rownum += 1
            ws.Range(rownum, 1, rownum, arrHeaders.Length).Merge()
            ws.Cell(rownum, 1).Value = ""
        Next



        ws.Cell((rownum + 2), 1).Value = "Printed Date : " & Now.ToString("dd/MM/yyyy")
        ws.Range((rownum + 2), 1, (rownum + 2), 4).Merge()
        Using wStream As New MemoryStream()
            wb.SaveAs(wStream)
            bytes = wStream.ToArray()
        End Using

    End Sub
#End Region


End Class
