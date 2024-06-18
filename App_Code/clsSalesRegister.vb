Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports ClosedXML.Excel

Public Class clsSalesRegister
    Inherits System.Web.UI.Page
#Region "global declaration"
    Dim objutils As New clsUtils
    Dim rptcompanyname, rptreportname, fromname, rptfilter, currname, decno As String
#End Region


    '#Region "Public Sub GenerateReport(ByVal reportsType As String, ByVal strfromDate As String, ByVal strToDate As String, ByVal divcode As String, ByVal Agent As String, ByVal currType As Integer, ByRef bytes() As Byte, ByVal printMode As String)"
    '    Public Sub GenerateReport(ByVal reportsType As String, ByVal strfromDate As String, ByVal strToDate As String, ByVal divcode As String, ByVal Agent As String, ByVal currType As Integer, ByRef bytes() As Byte, ByVal printMode As String)
    '        Try
    '            If divcode <> "" Then
    '                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
    '            Else
    '                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
    '            End If
    '            rptreportname = "Report - Sales Register" & Space(1) & "From" & Space(1) & Convert.ToDateTime(strfromDate).ToString("dd/MM/yyyy") & Space(1) & "To" & Space(1) & Convert.ToDateTime(strToDate).ToString("dd/MM/yyyy")

    '            Dim sqlConn As New SqlConnection
    '            Dim mySqlCmd As New SqlCommand
    '            Dim myDataAdapter As New SqlDataAdapter
    '            Dim ds As New DataSet
    '            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
    '            mySqlCmd = New SqlCommand("sp_rpt_sales_register", sqlConn)
    '            mySqlCmd.CommandType = CommandType.StoredProcedure
    '            mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 10)).Value = divcode
    '            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(strfromDate).ToString("yyyy/MM/dd")
    '            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(strToDate).ToString("yyyy/MM/dd")
    '            mySqlCmd.Parameters.Add(New SqlParameter("@currency", SqlDbType.Int)).Value = currType
    '            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = Agent
    '            myDataAdapter.SelectCommand = mySqlCmd
    '            myDataAdapter.Fill(ds)
    '            Dim salesRegDt As New DataTable
    '            salesRegDt = ds.Tables(0)

    '            rptfilter = ""
    '            If (Agent <> "") Then
    '                If salesRegDt.Rows.Count > 0 Then
    '                    rptfilter = "Agent : " & salesRegDt(0)("agentName")
    '                Else
    '                    Dim agentName As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select agentName from agentmast(nolock) where agentcode='" & Agent & "'"), String)
    '                    rptfilter = "Agent : " & agentName
    '                End If
    '            End If
    '            rptfilter = rptfilter & Space(4) & "Currency :" & Space(1) & IIf(currType = 0, "Party Currency", "Base Currency")

    '            Dim arrHeaders() As String = Nothing
    '            arrHeaders = {"Invoice No", "Date", "Request ID", "Agency", "Adult", "Child", "Total", "Arrival Date", "Depart Date", "Nights", "Room", "Visa", "Others", "Total Sale"}

    '            If reportsType = "excel" Then
    '                ExcelReport(salesRegDt, arrHeaders, bytes)
    '            Else

    '            End If
    '        Catch ex As Exception
    '            Throw ex
    '        End Try
    '    End Sub
    '#End Region

#Region "Public Sub GenerateReport(ByVal reportsType As String, ByVal strfromDate As String, ByVal strToDate As String, ByVal divcode As String, ByVal Agent As String, ByVal currType As Integer,ByVal Sector As String, ByVal Ctry As String, ByVal detailsummary As String, ByVal reportOrder As String,ByRef bytes() As Byte, ByVal printMode As String)"
    Public Sub GenerateReport(ByVal reportsType As String, ByVal strfromDate As String, ByVal strToDate As String, ByVal divcode As String, ByVal Agent As String, ByVal currType As Integer, ByVal Sector As String, ByVal Ctry As String, ByVal detailsummary As String, ByVal reportOrder As String, ByRef bytes() As Byte, ByVal printMode As String)
        Try
            If divcode <> "" Then
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & divcode & "'"), String)
            Else
                rptcompanyname = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
            End If
            rptreportname = "Report - Sales Register" & Space(1) & "From" & Space(1) & Convert.ToDateTime(strfromDate).ToString("dd/MM/yyyy") & Space(1) & "To" & Space(1) & Convert.ToDateTime(strToDate).ToString("dd/MM/yyyy")

            Dim sqlConn As New SqlConnection
            Dim mySqlCmd As New SqlCommand
            Dim myDataAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            mySqlCmd = New SqlCommand("sp_rpt_sales_register", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 10)).Value = divcode
            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(strfromDate).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(strToDate).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@currency", SqlDbType.Int)).Value = currType
            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = Agent
            mySqlCmd.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = Sector
            mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = Ctry
            mySqlCmd.Parameters.Add(New SqlParameter("@reporttype", SqlDbType.Int)).Value = detailsummary
            mySqlCmd.Parameters.Add(New SqlParameter("@reportorder", SqlDbType.Int)).Value = reportOrder

            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(ds)
            Dim salesRegDt As New DataTable
            salesRegDt = ds.Tables(0)

            rptfilter = ""
            If (Agent <> "") Then
                If salesRegDt.Rows.Count > 0 Then
                    rptfilter = "Agent : " & salesRegDt(0)("agentName")
                Else
                    Dim agentName As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select agentName from agentmast(nolock) where agentcode='" & Agent & "'"), String)
                    rptfilter = "Agent : " & agentName
                End If
            End If
            If (Sector <> "") Then
                If salesRegDt.Rows.Count > 0 Then
                    rptfilter = IIf(rptfilter <> "", Space(4), "") + rptfilter + "Sector : " & salesRegDt(0)("sectorname")
                Else
                    Dim sectorname As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select sectorname from agent_sectormaster(nolock) where sectorcode='" & Sector & "'"), String)
                    rptfilter = IIf(rptfilter <> "", Space(4), "") + rptfilter + "Sector : " & sectorname
                End If
            End If
            If (Ctry <> "") Then
                If salesRegDt.Rows.Count > 0 Then
                    rptfilter = IIf(rptfilter <> "", Space(4), "") + rptfilter + "Country : " & salesRegDt(0)("ctryname")
                Else
                    Dim ctryName As String = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select ctryname from ctrymast(nolock) where ctrycode='" & Ctry & "'"), String)
                    rptfilter = IIf(rptfilter <> "", Space(4), "") + rptfilter + "Country : " & ctryName
                End If
            End If


            rptfilter = rptfilter & Space(4) & "Currency :" & Space(1) & IIf(currType = 0, "Party Currency", "Base Currency")

            Dim arrHeaders() As String = Nothing
            If detailsummary = 0 Then

                arrHeaders = {"Invoice No", "Date", "Request ID", "Agency", "Sector", "Adult", "Child", "Total", "Arrival Date", "Depart Date", "Nights", "Guest Night Adult", "Guest Night Child", "Total Guest Night", "Room", "Visa", "Others", "Total Sale"}
                If reportsType = "excel" Then
                    ExcelReport(salesRegDt, arrHeaders, bytes)
                End If
            Else
                If reportsType = "excel" Then
                    SummaryExcelReport(salesRegDt, bytes, strfromDate, strToDate)
                End If
            End If


           
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Protected Sub ExcelReport(ByVal salesRegDt As DataTable, ByVal arrHeaders() As String, ByRef bytes() As Byte)"
    Protected Sub ExcelReport(ByVal salesRegDt As DataTable, ByVal arrHeaders() As String, ByRef bytes() As Byte)
        Try

            Dim col As Integer
            Dim companyCol, reportCol, filtercol As String
            Dim wbook As New XLWorkbook
            Dim ws = wbook.Worksheets.Add("SalesRegister")

            'Page Margins
            ws.PageSetup.Margins.Top = 0.75
            ws.PageSetup.Margins.Bottom = 0.75
            ws.PageSetup.Margins.Left = 0.25
            ws.PageSetup.Margins.Right = 0.25
            ws.PageSetup.Margins.Header = 0.3
            ws.PageSetup.Margins.Footer = 0.3

            ' Header and Footer
            ws.PageSetup.Footer.Right.AddText("Page" & Space(2), XLHFOccurrence.AllPages)
            ws.PageSetup.Footer.Right.AddText(XLHFPredefinedText.PageNumber, XLHFOccurrence.AllPages)
            ws.PageSetup.Footer.Right.AddText(Space(2) & "of" & Space(2), XLHFOccurrence.AllPages)
            ws.PageSetup.Footer.Right.AddText(XLHFPredefinedText.NumberOfPages, XLHFOccurrence.AllPages)
            ws.PageSetup.Footer.Left.AddText(XLHFPredefinedText.Date, XLHFOccurrence.AllPages)
            ws.PageSetup.PaperSize = XLPaperSize.A4Paper
            ws.PageSetup.PageOrientation = XLPageOrientation.Landscape
            ws.PageSetup.SetRowsToRepeatAtTop(1, 3)


            Dim rowcount As Integer = 4
            ws.Columns.AdjustToContents()
            ws.Column("A").Width = 14
            ws.Column("B").Width = 12
            ws.Column("C").Width = 14
            ws.Columns("D").Width = 25
            ws.Columns("E").Width = 20
            ws.Columns("F").Width = 8
            ws.Columns("G").Width = 8
            ws.Columns("H").Width = 8
            ws.Columns("I").Width = 12
            ws.Columns("J").Width = 12
            ws.Columns("K").Width = 8
            ws.Columns("L").Width = 12

            ws.Columns("M").Width = 12
            ws.Columns("N").Width = 12
            ws.Columns("O").Width = 12

            ws.Columns("P").Width = 12
            ws.Columns("Q").Width = 12
            ws.Columns("R").Width = 12

            companyCol = "A2:R2"
            reportCol = "A3:R3"
            filtercol = "A4:R4"

            'Comapny Name Heading
            ws.Cell("A2").Value = rptcompanyname
            Dim company = ws.Range(companyCol).Merge()
            company.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromHtml("#0048C0"))
            company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
            company.Style.Font.FontSize = 15
            company.Style.Font.FontColor = XLColor.Black
            company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            'Report Name Heading
            ws.Cell("A3").Value = rptreportname
            Dim report = ws.Range(reportCol).Merge()
            report.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromHtml("#0080C0"))
            report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
            report.Style.Font.FontSize = 14
            report.Style.Font.FontColor = XLColor.Black
            report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center


            'filter Name Heading
            ws.Cell("A4").Value = rptfilter
            Dim filter = ws.Range(filtercol).Merge()
            filter.Style.Font.SetBold()
            filter.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
            filter.Style.Font.FontSize = 12
            filter.Style.Font.FontColor = XLColor.Black
            filter.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
            filter.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            Dim currDecno As Integer = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= 'AED'"), Integer)
            Dim decimalPoint As String = "N" & currDecno.ToString()
            Dim decimalPoints As String
            If decimalPoint = "N1" Then
                decimalPoints = "##,##,##,##0.0"
            ElseIf decimalPoint = "N2" Then
                decimalPoints = "##,##,##,##0.00"
            ElseIf decimalPoint = "N3" Then
                decimalPoints = "##,##,##,##0.000"

            ElseIf decimalPoint = "N4" Then
                decimalPoints = "##,##,##,##0.0000"
            Else
                decimalPoints = "##,##,##,##0.00"
            End If

            If salesRegDt.Rows.Count > 0 Then
                rowcount = rowcount + 2
                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Fill.SetBackgroundColor(XLColor.LightGray)
                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Alignment.WrapText = True
                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.Bold = True
                For i = 0 To arrHeaders.Length - 1
                    ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
                    ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                Next
                Dim totalGuest As Integer
                Dim adult As Integer
                Dim child As Integer

                Dim night As Integer
                Dim ntsAdult As Integer
                Dim ntschild As Integer
                Dim totalngts As Integer
                Dim totntsAdult As Integer
                Dim totntschild As Integer
                Dim finaltotalngts As Integer

                Dim totAdults As Integer = 0
                Dim totChild As Integer = 0
                Dim totGuest As Integer = 0
                Dim totNights As Integer = 0
                Dim totRoomVal As Decimal = 0
                Dim totVisaVal As Decimal = 0
                Dim totOtherVal As Decimal = 0
                Dim totGrand As Decimal = 0
                For Each salesRegDr As DataRow In salesRegDt.Rows
                    rowcount = rowcount + 1

                    ws.Cell(rowcount, 1).Value = Convert.ToString(salesRegDr("invoiceno"))
                    ws.Cell(rowcount, 2).Value = "'" + Convert.ToDateTime(salesRegDr("invoicedate")).ToString("dd/MM/yyyy")
                    ws.Cell(rowcount, 3).Value = Convert.ToString(salesRegDr("requestid"))
                    ws.Cell(rowcount, 4).Value = Convert.ToString(salesRegDr("agentName"))
                    ws.Cell(rowcount, 5).Value = Convert.ToString(salesRegDr("sectorName"))
                    ws.Cell(rowcount, 6).Value = Convert.ToString(salesRegDr("adult"))
                    ws.Cell(rowcount, 7).Value = Convert.ToString(salesRegDr("child"))
                    totalGuest = 0
                    If IsNumeric(salesRegDr("adult")) Then
                        totalGuest = Convert.ToInt32(salesRegDr("adult"))
                        totAdults = totAdults + Convert.ToInt32(salesRegDr("adult"))
                    End If
                    If IsNumeric(salesRegDr("child")) Then
                        totalGuest = totalGuest + Convert.ToInt32(salesRegDr("child"))
                        totChild = totChild + Convert.ToInt32(salesRegDr("child"))
                    End If
                    ws.Cell(rowcount, 8).Value = totalGuest.ToString
                    Dim arrivalDate As String
                    If IsDate(salesRegDr("arrivaldate")) Then
                        arrivalDate = Convert.ToDateTime(salesRegDr("arrivaldate")).ToString("dd/MM/yyyy")
                    Else
                        arrivalDate = ""
                    End If
                    totGuest = totGuest + totalGuest
                    ws.Cell(rowcount, 9).Value = arrivalDate
                    Dim departureDate As String
                    If IsDate(salesRegDr("departuredate")) Then
                        departureDate = Convert.ToDateTime(salesRegDr("departureDate")).ToString("dd/MM/yyyy")
                    Else
                        departureDate = ""
                    End If
                    ws.Cell(rowcount, 10).Value = departureDate
                    ws.Cell(rowcount, 11).Value = Convert.ToString(salesRegDr("nights"))
                    'Added by Priyanka on 21/01/2020
                    night = Convert.ToInt32(salesRegDr("nights"))
                    adult = Convert.ToString(salesRegDr("adult"))
                    child = Convert.ToString(salesRegDr("child"))

                    ntsAdult = adult * night
                    ntschild = child * night
                    totalngts = ntsAdult + ntschild
                    totntsAdult = totntsAdult + ntsAdult
                    totntschild = totntschild + ntschild
                    finaltotalngts = finaltotalngts + totalngts

                    ws.Cell(rowcount, 12).Value = Convert.ToString(ntsAdult)
                    ws.Cell(rowcount, 13).Value = Convert.ToString(ntschild)
                    ws.Cell(rowcount, 14).Value = Convert.ToString(totalngts)
                    ws.Cell(rowcount, 15).Value = Convert.ToString(salesRegDr("room_sales"))
                    ws.Cell(rowcount, 15).Style.NumberFormat.Format = decimalPoints
                    ws.Cell(rowcount, 16).Value = Convert.ToString(salesRegDr("visa_sales"))
                    ws.Cell(rowcount, 16).Style.NumberFormat.Format = decimalPoints
                    ws.Cell(rowcount, 17).Value = Convert.ToString(salesRegDr("other_sales"))
                    ws.Cell(rowcount, 17).Style.NumberFormat.Format = decimalPoints
                    ws.Cell(rowcount, 18).Value = Convert.ToString(salesRegDr("total_sales"))
                    ws.Cell(rowcount, 18).Style.NumberFormat.Format = decimalPoints


                    If IsNumeric(salesRegDr("nights")) Then
                        totNights = totNights + Convert.ToInt32(salesRegDr("nights"))
                    End If
                    If IsNumeric(salesRegDr("room_sales")) Then
                        totRoomVal = totRoomVal + Convert.ToDecimal(salesRegDr("room_sales"))
                    End If
                    If IsNumeric(salesRegDr("visa_sales")) Then
                        totVisaVal = totVisaVal + Convert.ToDecimal(salesRegDr("visa_sales"))
                    End If
                    If IsNumeric(salesRegDr("other_sales")) Then
                        totOtherVal = totOtherVal + Convert.ToDecimal(salesRegDr("other_sales"))
                    End If
                    If IsNumeric(salesRegDr("total_sales")) Then
                        totGrand = totGrand + Convert.ToDecimal(salesRegDr("total_sales"))
                    End If
                    ws.Range(rowcount, 1, rowcount, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    ws.Range(rowcount, 6, rowcount, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    ws.Range(rowcount, 9, rowcount, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    ws.Range(rowcount, 11, rowcount, 14).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    ws.Range(rowcount, 15, rowcount, 18).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Alignment.WrapText = True
                Next
                arrHeaders = {"Total", totAdults.ToString(), totChild.ToString(), totGuest.ToString(), totNights.ToString(), totntsAdult.ToString(), totntschild.ToString(), finaltotalngts.ToString(), Convert.ToDecimal(totRoomVal).ToString(decno), Convert.ToDecimal(totVisaVal).ToString(decno), Convert.ToDecimal(totOtherVal).ToString(decno), Convert.ToDecimal(totGrand).ToString(decno)}
                rowcount += 1
                ws.Range(rowcount, 1, rowcount, 18).Style.Font.SetBold().Font.FontSize = 10
                ws.Range(rowcount, 1, rowcount, 18).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(rowcount, 1, rowcount, 18).Style.Fill.SetBackgroundColor(XLColor.LightGray).Alignment.WrapText = True
                For i = 0 To arrHeaders.Length - 1
                    If i = 0 Then
                        ws.Range("A" & rowcount & ":E" & rowcount).Value = arrHeaders(i)
                        ws.Range("A" & rowcount & ":E" & rowcount).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ElseIf i >= 1 And i <= 3 Then
                        ws.Cell(rowcount, i + 5).Value = arrHeaders(i)
                        ws.Cell(rowcount, i + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    ElseIf i = 4 Or i = 5 Or i = 6 Or i = 7 Then
                        ws.Cell(rowcount, i + 7).Value = arrHeaders(i)
                        ws.Cell(rowcount, i + 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    ElseIf i >= 8 Then
                        ws.Cell(rowcount, i + 7).Value = arrHeaders(i)
                        ws.Cell(rowcount, i + 7).Style.NumberFormat.Format = decimalPoints
                        ws.Cell(rowcount, i + 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    End If
                Next

            End If

            ws.Cell((rowcount + 2), 1).Value = "Printed Date:" & Now.ToString("dd/MM/yyyy")
            ws.Range((rowcount + 2), 1, (rowcount + 2), 3).Merge()
            Using wStream As New MemoryStream()
                wbook.SaveAs(wStream)
                bytes = wStream.ToArray()
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Protected Sub SummaryExcelReport(ByVal salesRegDt As DataTable, ByVal arrHeaders() As String, ByRef bytes() As Byte)"
    Protected Sub SummaryExcelReport(ByVal salesRegDt As DataTable, ByRef bytes() As Byte, ByVal strfromdate As String, ByVal strTodate As String)
        Try

            Dim col As Integer

            Dim companyCol, reportCol, filtercol As String
            Dim wbook As New XLWorkbook
            Dim ws = wbook.Worksheets.Add("SalesRegisterSummary")

            'Page Margins
            ws.PageSetup.Margins.Top = 0.75
            ws.PageSetup.Margins.Bottom = 0.75
            ws.PageSetup.Margins.Left = 0.25
            ws.PageSetup.Margins.Right = 0.25
            ws.PageSetup.Margins.Header = 0.3
            ws.PageSetup.Margins.Footer = 0.3

            ' Header and Footer
            ws.PageSetup.Footer.Right.AddText("Page" & Space(2), XLHFOccurrence.AllPages)
            ws.PageSetup.Footer.Right.AddText(XLHFPredefinedText.PageNumber, XLHFOccurrence.AllPages)
            ws.PageSetup.Footer.Right.AddText(Space(2) & "of" & Space(2), XLHFOccurrence.AllPages)
            ws.PageSetup.Footer.Right.AddText(XLHFPredefinedText.NumberOfPages, XLHFOccurrence.AllPages)
            ws.PageSetup.Footer.Left.AddText(XLHFPredefinedText.Date, XLHFOccurrence.AllPages)
            ws.PageSetup.PaperSize = XLPaperSize.A4Paper
            ws.PageSetup.PageOrientation = XLPageOrientation.Landscape
            ws.PageSetup.SetRowsToRepeatAtTop(1, 3)


            Dim rowcount As Integer = 4
            ws.Columns.AdjustToContents()
            ws.Column("A").Width = 30
            ws.Column("B").Width = 25
            ws.Column("C").Width = 6
            ws.Column("d").Width = 6
            ws.Column("e").Width = 6
            ws.Column("f").Width = 6
            ws.Column("g").Width = 6
            ws.Column("h").Width = 6
            ws.Column("i").Width = 6
            ws.Column("j").Width = 6
            ws.Column("k").Width = 6
            ws.Column("l").Width = 6
            ws.Column("m").Width = 6
            ws.Column("n").Width = 6
            ws.Column("o").Width = 6
            ws.Column("p").Width = 6
            ws.Column("q").Width = 6
            ws.Column("r").Width = 6
            ws.Column("s").Width = 6
            ws.Column("t").Width = 6
            ws.Column("u").Width = 6
            ws.Column("v").Width = 6
            ws.Column("w").Width = 6
            ws.Column("x").Width = 6
            ws.Column("y").Width = 6
            ws.Column("z").Width = 6


            

            Dim startmonth = Convert.ToDateTime(strfromdate).Month
            Dim endmonth = Convert.ToDateTime(strTodate).Month
            Dim startYear As Integer = Convert.ToDateTime(strfromdate).Year
            Dim endYear As Integer = Convert.ToDateTime(strTodate).Year



            'companyCol = "A2:Q2"
            'reportCol = "A3:Q3"
            'filtercol = "A4:Q4"
            Dim calMonth As Integer = ((endYear - startYear) * 12) + endmonth - startmonth
            If calMonth + 1 > 12 Then
                Throw New ApplicationException("Date range should be less than equal to 12 months")
            End If
            Dim lastcol = (((calMonth) + 1) * 2) + 4
            'Dim lastcol = (((endmonth - startmonth) + 1) * 2) + 4
            Dim arrHeaders(lastcol) As String
            'Comapny Name Heading
            ws.Cell("A2").Value = rptcompanyname
            Dim company = ws.Range(2, 1, 2, lastcol).Merge()
            company.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromHtml("#0048C0"))
            company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
            company.Style.Font.FontSize = 15
            company.Style.Font.FontColor = XLColor.Black
            company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            'Report Name Heading
            ws.Cell("A3").Value = rptreportname
            Dim report = ws.Range(3, 1, 3, lastcol).Merge()
            report.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromHtml("#0080C0"))
            report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
            report.Style.Font.FontSize = 14
            report.Style.Font.FontColor = XLColor.Black
            report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center


            'filter Name Heading
            ws.Cell("A4").Value = rptfilter
            Dim filter = ws.Range(4, 1, 4, lastcol).Merge()
            filter.Style.Font.SetBold()
            filter.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
            filter.Style.Font.FontSize = 12
            filter.Style.Font.FontColor = XLColor.Black
            filter.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
            filter.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            

            Dim currDecno As Integer = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= 'AED'"), Integer)
            Dim decimalPoint As String = "N" & currDecno.ToString()
            Dim decimalPoints As String
            If decimalPoint = "N1" Then
                decimalPoints = "##,##,##,##0.0"
            ElseIf decimalPoint = "N2" Then
                decimalPoints = "##,##,##,##0.00"
            ElseIf decimalPoint = "N3" Then
                decimalPoints = "##,##,##,##0.000"

            ElseIf decimalPoint = "N4" Then
                decimalPoints = "##,##,##,##0.0000"
            Else
                decimalPoints = "##,##,##,##0.00"
            End If

            If salesRegDt.Rows.Count > 0 Then
                rowcount = rowcount + 2
                Dim colcount As Integer = 2
                Dim colname As String
                Dim monthcount As Integer = 1
                For m = 1 To calMonth + 1 'startmonth To endmonth
                    colname = "month" & monthcount & "name"
                    monthcount = monthcount + 1
                    Dim datecol = Convert.ToDateTime(salesRegDt.Rows.Item(0).Item(colname))
                    'Dim year As String = datecol.Year
                    Dim year As String = datecol.ToString("yy")
                    Dim month As String = datecol.ToString("MMM")

                    arrHeaders(colcount) = month & "-" & year

                    'arrHeaders(colcount) = Space(1) & month & "-" & year & Space(1) & "."
                    colcount = colcount + 1
                Next
                arrHeaders(0) = "Agency"
                arrHeaders(1) = "Sector"
                arrHeaders(colcount) = "Total"

                ws.Range(rowcount, 1, rowcount, lastcol).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
                ws.Range(rowcount, 1, rowcount, lastcol).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(rowcount, 1, rowcount, lastcol).Style.Fill.SetBackgroundColor(XLColor.LightGray)
                '   ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Alignment.WrapText = True
                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.Bold = True

                For i = 0 To (calMonth) + 3
                    If i = 0 Or i = 1 Then
                        ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
                        ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    ElseIf i = 2 Then
                        ws.Range(rowcount, i + 1, rowcount, i + 2).SetValue(Of String)(Convert.ToString(arrHeaders(i)))
                        ' ws.Range(rowcount, i + 1, rowcount, i + 2).Value = arrHeaders(i)
                        ws.Range(rowcount, i + 1, rowcount, i + 2).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    Else
                        ws.Range(rowcount, i + (i - 1), rowcount, i + i).SetValue(Of String)(Convert.ToString(arrHeaders(i)))
                        'ws.Range(rowcount, i + (i - 1), rowcount, i + i).Value = arrHeaders(i)
                        ws.Range(rowcount, i + (i - 1), rowcount, i + i).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)

                    End If
                Next
                rowcount = rowcount + 1

                ws.Range(rowcount, 3, rowcount, lastcol).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.FontSize = 10
                ws.Range(rowcount, 3, rowcount, lastcol).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(rowcount, 3, rowcount, lastcol).Style.Fill.SetBackgroundColor(XLColor.LightGray)
                '   ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Alignment.WrapText = True
                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.Bold = True

                For c = 3 To lastcol
                    If c Mod 2 <> 0 Then
                        ws.Cell(rowcount, c).Value = "Adult"
                    Else
                        ws.Cell(rowcount, c).Value = "Child"
                    End If
                Next

                Dim fianltotalAdult() As Integer
                Dim finaltotalchild() As Integer
                Dim totalAdults As Integer = 0
                Dim totalChild As Integer = 0
                Dim arrTotal(lastcol) As String

               

                For Each salesRegDr As DataRow In salesRegDt.Rows
                    rowcount = rowcount + 1
                    colcount = 3

                    ws.Range(rowcount, 1, rowcount, lastcol).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
                    ws.Range(rowcount, 1, rowcount, lastcol).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    ws.Range(rowcount, 1, rowcount, lastcol).Style.Alignment.WrapText = True


                    ws.Cell(rowcount, 1).Value = Convert.ToString(salesRegDr("agentname"))
                    ws.Cell(rowcount, 2).Value = Convert.ToString(salesRegDr("sectorname"))
                    totalAdults = 0
                    totalChild = 0
                    monthcount = 1
                    For m = 0 To calMonth

                        colname = "month" & monthcount & "Adult"
                        ws.Cell(rowcount, colcount).Value = Convert.ToString(salesRegDr(colname))
                        Dim total = Convert.ToInt32(arrTotal(colcount - 1)) + Convert.ToInt32(salesRegDr(colname))
                        arrTotal(colcount - 1) = total
                        totalAdults = totalAdults + Convert.ToInt32(salesRegDr(colname))
                        colcount = colcount + 1
                        colname = "month" & monthcount & "Child"

                        ws.Cell(rowcount, colcount).Value = Convert.ToString(salesRegDr(colname))
                        total = Convert.ToInt32(arrTotal(colcount - 1)) + Convert.ToInt32(salesRegDr(colname))
                        arrTotal(colcount - 1) = total
                        colcount = colcount + 1
                        monthcount = monthcount + 1
                        totalChild = totalChild + Convert.ToInt32(salesRegDr(colname))
                    Next
                    ws.Cell(rowcount, colcount).Value = totalAdults
                    arrTotal(colcount - 1) = Convert.ToInt32(arrTotal(colcount - 1)) + totalAdults
                    ws.Cell(rowcount, colcount + 1).Value = totalChild
                    arrTotal(colcount) = Convert.ToInt32(arrTotal(colcount)) + totalChild

                Next
                arrTotal(0) = "Total"
                arrTotal(1) = "Total"
                rowcount += 1

                ws.Range(rowcount, 1, rowcount, lastcol).Style.Font.SetBold().Font.FontSize = 10
                ws.Range(rowcount, 1, rowcount, lastcol).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(rowcount, 1, rowcount, lastcol).Style.Fill.SetBackgroundColor(XLColor.LightGray).Alignment.WrapText = True

                For i = 0 To arrTotal.Length - 1
                    If i = 0 Or i = 1 Then
                        ws.Range(rowcount, 1, rowcount, 2).Value = arrTotal(i)
                        ws.Range(rowcount, 1, rowcount, 2).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                    ElseIf i > 1 Then
                        ws.Cell(rowcount, i + 1).Value = arrTotal(i)
                        ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                    End If
                Next

            End If

            ws.Cell((rowcount + 2), 1).Value = "Printed Date:" & Now.ToString("dd/MM/yyyy")
            ws.Range((rowcount + 2), 1, (rowcount + 2), 3).Merge()
            Using wStream As New MemoryStream()
                wbook.SaveAs(wStream)
                bytes = wStream.ToArray()
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

    '#Region "Protected Sub ExcelReport(ByVal salesRegDt As DataTable, ByVal arrHeaders() As String, ByRef bytes() As Byte)"
    '    Protected Sub ExcelReport(ByVal salesRegDt As DataTable, ByVal arrHeaders() As String, ByRef bytes() As Byte)
    '        Try

    '            Dim col As Integer
    '            Dim companyCol, reportCol, filtercol As String
    '            Dim wbook As New XLWorkbook
    '            Dim ws = wbook.Worksheets.Add("SalesRegister")

    '            'Page Margins
    '            ws.PageSetup.Margins.Top = 0.75
    '            ws.PageSetup.Margins.Bottom = 0.75
    '            ws.PageSetup.Margins.Left = 0.25
    '            ws.PageSetup.Margins.Right = 0.25
    '            ws.PageSetup.Margins.Header = 0.3
    '            ws.PageSetup.Margins.Footer = 0.3

    '            ' Header and Footer
    '            ws.PageSetup.Footer.Right.AddText("Page" & Space(2), XLHFOccurrence.AllPages)
    '            ws.PageSetup.Footer.Right.AddText(XLHFPredefinedText.PageNumber, XLHFOccurrence.AllPages)
    '            ws.PageSetup.Footer.Right.AddText(Space(2) & "of" & Space(2), XLHFOccurrence.AllPages)
    '            ws.PageSetup.Footer.Right.AddText(XLHFPredefinedText.NumberOfPages, XLHFOccurrence.AllPages)
    '            ws.PageSetup.Footer.Left.AddText(XLHFPredefinedText.Date, XLHFOccurrence.AllPages)
    '            ws.PageSetup.PaperSize = XLPaperSize.A4Paper
    '            ws.PageSetup.PageOrientation = XLPageOrientation.Landscape
    '            ws.PageSetup.SetRowsToRepeatAtTop(1, 3)


    '            Dim rowcount As Integer = 4
    '            ws.Columns.AdjustToContents()
    '            ws.Column("A").Width = 14
    '            ws.Column("B").Width = 12
    '            ws.Column("C").Width = 14
    '            ws.Columns("D").Width = 25
    '            ws.Columns("E").Width = 8
    '            ws.Columns("F").Width = 8
    '            ws.Columns("G").Width = 8
    '            ws.Columns("H").Width = 12
    '            ws.Columns("I").Width = 12
    '            ws.Columns("J").Width = 8
    '            ws.Columns("K").Width = 12
    '            ws.Columns("L").Width = 12
    '            ws.Columns("M").Width = 12
    '            ws.Columns("N").Width = 12

    '            companyCol = "A2:N2"
    '            reportCol = "A3:N3"
    '            filtercol = "A4:N4"

    '            'Comapny Name Heading
    '            ws.Cell("A2").Value = rptcompanyname
    '            Dim company = ws.Range(companyCol).Merge()
    '            company.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromHtml("#0048C0"))
    '            company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
    '            company.Style.Font.FontSize = 15
    '            company.Style.Font.FontColor = XLColor.Black
    '            company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
    '            company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

    '            'Report Name Heading
    '            ws.Cell("A3").Value = rptreportname
    '            Dim report = ws.Range(reportCol).Merge()
    '            report.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromHtml("#0080C0"))
    '            report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
    '            report.Style.Font.FontSize = 14
    '            report.Style.Font.FontColor = XLColor.Black
    '            report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
    '            report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center


    '            'filter Name Heading
    '            ws.Cell("A4").Value = rptfilter
    '            Dim filter = ws.Range(filtercol).Merge()
    '            filter.Style.Font.SetBold()
    '            filter.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.FontSize = 10
    '            filter.Style.Font.FontSize = 12
    '            filter.Style.Font.FontColor = XLColor.Black
    '            filter.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
    '            filter.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

    '            Dim currDecno As Integer = CType(objutils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= 'AED'"), Integer)
    '            Dim decimalPoint As String = "N" & currDecno.ToString()
    '            Dim decimalPoints As String
    '            If decimalPoint = "N1" Then
    '                decimalPoints = "##,##,##,##0.0"
    '            ElseIf decimalPoint = "N2" Then
    '                decimalPoints = "##,##,##,##0.00"
    '            ElseIf decimalPoint = "N3" Then
    '                decimalPoints = "##,##,##,##0.000"

    '            ElseIf decimalPoint = "N4" Then
    '                decimalPoints = "##,##,##,##0.0000"
    '            Else
    '                decimalPoints = "##,##,##,##0.00"
    '            End If

    '            If salesRegDt.Rows.Count > 0 Then
    '                rowcount = rowcount + 2
    '                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
    '                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Fill.SetBackgroundColor(XLColor.LightGray)
    '                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Alignment.WrapText = True
    '                ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.Bold = True
    '                For i = 0 To arrHeaders.Length - 1
    '                    ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
    '                    ws.Cell(rowcount, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '                Next
    '                Dim totalGuest As Integer
    '                Dim totAdults As Integer = 0
    '                Dim totChild As Integer = 0
    '                Dim totGuest As Integer = 0
    '                Dim totNights As Integer = 0
    '                Dim totRoomVal As Decimal = 0
    '                Dim totVisaVal As Decimal = 0
    '                Dim totOtherVal As Decimal = 0
    '                Dim totGrand As Decimal = 0
    '                For Each salesRegDr As DataRow In salesRegDt.Rows
    '                    rowcount = rowcount + 1

    '                    ws.Cell(rowcount, 1).Value = Convert.ToString(salesRegDr("invoiceno"))
    '                    ws.Cell(rowcount, 2).Value = "'" + Convert.ToDateTime(salesRegDr("invoicedate")).ToString("dd/MM/yyyy")
    '                    ws.Cell(rowcount, 3).Value = Convert.ToString(salesRegDr("requestid"))
    '                    ws.Cell(rowcount, 4).Value = Convert.ToString(salesRegDr("agentName"))
    '                    ws.Cell(rowcount, 5).Value = Convert.ToString(salesRegDr("adult"))
    '                    ws.Cell(rowcount, 6).Value = Convert.ToString(salesRegDr("child"))
    '                    totalGuest = 0
    '                    If IsNumeric(salesRegDr("adult")) Then
    '                        totalGuest = Convert.ToInt32(salesRegDr("adult"))
    '                        totAdults = totAdults + Convert.ToInt32(salesRegDr("adult"))
    '                    End If
    '                    If IsNumeric(salesRegDr("child")) Then
    '                        totalGuest = totalGuest + Convert.ToInt32(salesRegDr("child"))
    '                        totChild = totChild + Convert.ToInt32(salesRegDr("child"))
    '                    End If
    '                    ws.Cell(rowcount, 7).Value = totalGuest.ToString
    '                    Dim arrivalDate As String
    '                    If IsDate(salesRegDr("arrivaldate")) Then
    '                        arrivalDate = Convert.ToDateTime(salesRegDr("arrivaldate")).ToString("dd/MM/yyyy")
    '                    Else
    '                        arrivalDate = ""
    '                    End If
    '                    totGuest = totGuest + totalGuest
    '                    ws.Cell(rowcount, 8).Value = arrivalDate
    '                    Dim departureDate As String
    '                    If IsDate(salesRegDr("departuredate")) Then
    '                        departureDate = Convert.ToDateTime(salesRegDr("departureDate")).ToString("dd/MM/yyyy")
    '                    Else
    '                        departureDate = ""
    '                    End If
    '                    ws.Cell(rowcount, 9).Value = departureDate
    '                    ws.Cell(rowcount, 10).Value = Convert.ToString(salesRegDr("nights"))
    '                    ws.Cell(rowcount, 11).Value = Convert.ToString(salesRegDr("room_sales"))
    '                    ws.Cell(rowcount, 11).Style.NumberFormat.Format = decimalPoints
    '                    ws.Cell(rowcount, 12).Value = Convert.ToString(salesRegDr("visa_sales"))
    '                    ws.Cell(rowcount, 12).Style.NumberFormat.Format = decimalPoints
    '                    ws.Cell(rowcount, 13).Value = Convert.ToString(salesRegDr("other_sales"))
    '                    ws.Cell(rowcount, 13).Style.NumberFormat.Format = decimalPoints
    '                    ws.Cell(rowcount, 14).Value = Convert.ToString(salesRegDr("total_sales"))
    '                    ws.Cell(rowcount, 14).Style.NumberFormat.Format = decimalPoints


    '                    If IsNumeric(salesRegDr("nights")) Then
    '                        totNights = totNights + Convert.ToInt32(salesRegDr("nights"))
    '                    End If
    '                    If IsNumeric(salesRegDr("room_sales")) Then
    '                        totRoomVal = totRoomVal + Convert.ToDecimal(salesRegDr("room_sales"))
    '                    End If
    '                    If IsNumeric(salesRegDr("visa_sales")) Then
    '                        totVisaVal = totVisaVal + Convert.ToDecimal(salesRegDr("visa_sales"))
    '                    End If
    '                    If IsNumeric(salesRegDr("other_sales")) Then
    '                        totOtherVal = totOtherVal + Convert.ToDecimal(salesRegDr("other_sales"))
    '                    End If
    '                    If IsNumeric(salesRegDr("total_sales")) Then
    '                        totGrand = totGrand + Convert.ToDecimal(salesRegDr("total_sales"))
    '                    End If
    '                    ws.Range(rowcount, 1, rowcount, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
    '                    ws.Range(rowcount, 5, rowcount, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '                    ws.Range(rowcount, 8, rowcount, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
    '                    ws.Range(rowcount, 10, rowcount, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '                    ws.Range(rowcount, 11, rowcount, 14).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
    '                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '                    ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Alignment.WrapText = True
    '                Next
    '                arrHeaders = {"Total", totAdults.ToString(), totChild.ToString(), totGuest.ToString(), totNights.ToString(), Convert.ToDecimal(totRoomVal).ToString(decno), Convert.ToDecimal(totVisaVal).ToString(decno), Convert.ToDecimal(totOtherVal).ToString(decno), Convert.ToDecimal(totGrand).ToString(decno)}
    '                rowcount += 1
    '                ws.Range(rowcount, 1, rowcount, 14).Style.Font.SetBold().Font.FontSize = 10
    '                ws.Range(rowcount, 1, rowcount, 14).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '                ws.Range(rowcount, 1, rowcount, 14).Style.Fill.SetBackgroundColor(XLColor.LightGray).Alignment.WrapText = True
    '                For i = 0 To arrHeaders.Length - 1
    '                    If i = 0 Then
    '                        ws.Range("A" & rowcount & ":D" & rowcount).Value = arrHeaders(i)
    '                        ws.Range("A" & rowcount & ":D" & rowcount).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                    ElseIf i >= 1 And i <= 3 Then
    '                        ws.Cell(rowcount, i + 4).Value = arrHeaders(i)
    '                        ws.Cell(rowcount, i + 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '                    ElseIf i = 4 Then
    '                        ws.Cell(rowcount, i + 6).Value = arrHeaders(i)
    '                        ws.Cell(rowcount, i + 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '                    ElseIf i >= 5 Then
    '                        ws.Cell(rowcount, i + 6).Value = arrHeaders(i)
    '                        ws.Cell(rowcount, i + 6).Style.NumberFormat.Format = decimalPoints
    '                        ws.Cell(rowcount, i + 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                    End If
    '                Next

    '            End If

    '            ws.Cell((rowcount + 2), 1).Value = "Printed Date:" & Now.ToString("dd/MM/yyyy")
    '            ws.Range((rowcount + 2), 1, (rowcount + 2), 3).Merge()
    '            Using wStream As New MemoryStream()
    '                wbook.SaveAs(wStream)
    '                bytes = wStream.ToArray()
    '            End Using
    '        Catch ex As Exception
    '            Throw ex
    '        End Try
    '    End Sub
    '#End Region

End Class
