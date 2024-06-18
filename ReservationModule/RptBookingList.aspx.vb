
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports ClosedXML.Excel
Imports System.IO
Imports System.Net
Imports System.Linq


Partial Class RptBookingList
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
#End Region

#Region "Web Methods"
    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetCustomers(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim customers As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select agentcode,agentname from agentmast where active=1 and agentname like '%" & prefixText & "%' order by agentname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    customers.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("agentcode").ToString()))
                Next
            End If
            Return customers
        Catch ex As Exception
            Return customers
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetUser(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim customers As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select UserCode,UserName from UserMaster where active = 1 and  UserName like '%" & prefixText & "%' order by UserName asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    customers.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("UserName").ToString(), myDS.Tables(0).Rows(i)("UserCode").ToString()))
                Next
            End If
            Return customers
        Catch ex As Exception
            Return customers
        End Try
    End Function
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then

                If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                Dim strappid As String = ""
                Dim strappname As String = ""

                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                ' Dim lbltitle As Label = CType(Master.FindControl("Title"), Label)
                Dim type As String = Convert.ToString(Request.QueryString("type"))
                If type <> "" Then
                    txtRptType.Text = type.Trim
                End If

                txtFromDt.Text = Now.Date
                txtToDt.Text = Now.Date
                If AppId.Value Is Nothing = False Then
                    strappid = AppId.Value
                End If
                strappname = objUser.GetAppName(Session("dbconnectionName"), strappid)

                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "ReservationModule\RptBookingList.aspx", btnAddNew, btnExportToExcel, _
                                                       btnprint, gvSearch:=gvSearchResult)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptBookingList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Function Validation() As Boolean"
    Protected Function Validation() As Boolean
        Try
            If (Not IsDate(txtFromDt.Text) And IsDate(txtToDt.Text)) Or (IsDate(txtFromDt.Text) And Not IsDate(txtToDt.Text)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Verify From Date and To Date' );", True)
                Validation = False
                Exit Function
            End If
            If txtCust.Text.Trim = "" Then txtCustCode.Text = ""
            If txtUser.Text.Trim = "" Then txtUserCode.Text = ""
            Validation = True
        Catch ex As Exception
            Validation = False
            Throw ex
        End Try
    End Function
#End Region

#Region "Protected Sub btnLoadReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadReport.Click"
    Protected Sub btnLoadReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadReport.Click
        Try
            ExcelReport()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptBookingList.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected sub ExcelReport() "
    Public Sub ExcelReport()

        Dim fromdate, todate, rpttype, customer, type, amt, user, rptcompanyname, rptname, custname, filter, rptfilter, filwidth, cols, decimalPoint, DecimalPoints, arrHeaders(), currcode, currDecno As String
        Dim rownum, datetype, reqtype, reporttype, bpax, fcfpax, fcnpax, cnpax, cfpax, fbpax As Integer
        Dim bamt, cfamt, cnamt, fbamt, fcfamt, fcnamt As Decimal

        fromdate = txtFromDt.Text.Trim
        todate = txtToDt.Text.Trim
        rpttype = ddlsummdet.SelectedItem.Text
        customer = txtCustCode.Text.Trim
        custname = txtCust.Text.Trim
        user = txtUserCode.Text.Trim
        type = ddldatetype.SelectedItem.Text

        filter = IIf(user <> "" AndAlso customer <> "", "   Customer: " & custname & "   User: " & txtUser.Text.Trim, IIf(user <> "", "   User: " & txtUser.Text.Trim, IIf(customer <> "", "   Customer: " & custname, "")))
        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add("Booking List")
        rownum = 5
        If rpttype.Equals("Summary") Then
            datetype = 0
            reqtype = ddldatetype.SelectedItem.Value
            reporttype = 0
            cols = "D"
            filwidth = 80
            rptfilter = "From Date : " & fromdate & "   To Date : " & todate & "   Request Type: " & type & "  " & filter
            rptname = "Summary Report - Booking List"
            ws.Columns("A:C").Width = 20
            ws.Columns("D").Width = 25
        Else
            ws.Columns("A").Width = 18
            ws.Columns("B").Width = 30
            ws.Columns("C").Width = 12
            ws.Columns("D").Width = 7
            ws.Columns("E").Width = 14
            ws.Columns("F").Width = 7
            ws.Columns("G").Width = 14
            ws.Columns("H").Width = 7
            ws.Columns("I").Width = 14
            datetype = ddldatetype.SelectedItem.Value
            reqtype = 0
            filwidth = 120
            cols = "I"
            reporttype = 1
            rptname = "Detailed Report - Booking List"
            rptfilter = "From Date : " & fromdate & "   To Date : " & todate & "   Date Type: " & type & "  " & filter
        End If

        Dim sqlConn As New SqlConnection
        Dim mySqlCmd As New SqlCommand
        Dim myDataAdapter As New SqlDataAdapter
        Dim ds As New DataSet
        sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
        mySqlCmd = New SqlCommand("sp_booking_listnew", sqlConn)
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(fromdate), "yyyy-MM-dd")
        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(todate), "yyyy-MM-dd")
        mySqlCmd.Parameters.Add(New SqlParameter("@fromstaff", SqlDbType.VarChar, 20)).Value = user
        mySqlCmd.Parameters.Add(New SqlParameter("@agentfrm", SqlDbType.VarChar, 20)).Value = customer
        mySqlCmd.Parameters.Add(New SqlParameter("@datetype", SqlDbType.Int)).Value = datetype
        mySqlCmd.Parameters.Add(New SqlParameter("@requesttype", SqlDbType.Int)).Value = reqtype
        mySqlCmd.Parameters.Add(New SqlParameter("@reporttype", SqlDbType.Int)).Value = reporttype
        myDataAdapter.SelectCommand = mySqlCmd
        myDataAdapter.Fill(ds)
        Dim custdetailsdt As New DataTable
        custdetailsdt = ds.Tables(0)
        rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)

        If custdetailsdt.Rows.Count > 0 Then
            'Report Name Heading
            Dim company = ws.Range("A1:" & cols & "1").Merge()
            ws.Cell("A1").Value = rptcompanyname
            company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetBold().Font.FontSize = 15
            company.Style.Font.FontColor = XLColor.Black
            company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            'Report Name Heading
            Dim company1 = ws.Range("A2:" & cols & "2").Merge()
            ws.Cell("A2").Value = rptname
            company1.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetBold().Font.FontSize = 14
            company1.Style.Font.FontColor = XLColor.Black
            company1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            company1.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            'Report filter Heading
            ws.Cell("A3").Value = rptfilter
            Dim report = ws.Range("A3:" & cols & "3").Merge()
            report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 12
            report.Style.Alignment.SetWrapText().Font.FontColor = XLColor.Black


            If rptfilter.Length > filwidth Then
                Dim rowheight = IIf(rptfilter.Length > filwidth And rptfilter.Length < (filwidth + filwidth), 32, 48)
                ws.Row(3).Height = rowheight
            End If

            If rpttype.Equals("Summary") Then
                arrHeaders = {"Salesperson Name", "No. Of Bookings Added", "No. Of Bookings Modified", "No. Of Bookings Cancelled"}
                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Fill.SetBackgroundColor(XLColor.LightGray).Font.SetBold().Font.FontSize = 10
                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                'ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                For i = 0 To arrHeaders.Length - 1
                    If i = 0 Then
                        ws.Cell(rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    Else
                        ws.Range(rownum, 2, rownum, arrHeaders.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    End If
                    ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                Next
                For Each row In custdetailsdt.Rows
                    'fill array with related sp data
                    arrHeaders = {row("UserName").ToString(), row("addedbooking").ToString(), row("modifiedbooking").ToString(), row("cancelledbooking").ToString()}
                    bpax = bpax + row("addedbooking")
                    cfpax = cfpax + row("modifiedbooking")
                    cnpax = cnpax + row("cancelledbooking")
                    rownum += 1
                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.FontSize = 9
                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
                    For i = 0 To arrHeaders.Length - 1
                        If i = 0 Then
                            ws.Cell(rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                        Else
                            ws.Range(rownum, 2, rownum, arrHeaders.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                            If Not arrHeaders(i).Equals("0") Then
                                ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                            Else
                                ws.Cell(rownum, i + 1).Value = ""
                            End If
                        End If

                    Next
                Next
                arrHeaders = {"Total", bpax, cfpax, cnpax}
                rownum += 1
                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 10
                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
                For i = 0 To arrHeaders.Length - 1
                    If i = 0 Then
                        ws.Cell(rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ws.Cell(rownum, 1).Style.Fill.SetBackgroundColor(XLColor.LightGray)
                        ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                    Else
                        ws.Range(rownum, 2, rownum, arrHeaders.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        If Not arrHeaders(i).Equals("0") Then
                            ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                        Else
                            ws.Cell(rownum, i + 1).Value = ""
                        End If
                    End If
                Next
            ElseIf rpttype.Equals("Detailed") Then
                currcode = custdetailsdt.AsEnumerable().Select(Function(s) s.Field(Of String)("currcode")).FirstOrDefault
                currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currcode & "'"), Integer)
                decimalPoint = "N" & currDecno.ToString()
                amt = "Amount(" & currcode & ")"
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


                arrHeaders = {"Agent Name", "Services", "Service Type", "Bookings", "Confirmations", " Cancellations"}
                ws.Range(rownum, 1, rownum, arrHeaders.Length + 3).Style.Fill.SetBackgroundColor(XLColor.LightGray).Font.SetBold().Font.FontSize = 10
                ws.Range(rownum, 1, rownum, arrHeaders.Length + 3).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(rownum, 1, rownum, arrHeaders.Length + 3).Style.Alignment.WrapText = True
                ws.Range(rownum, 4, rownum, arrHeaders.Length + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                For i = 0 To arrHeaders.Length - 1
                    If i < 3 Then
                        ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                        ws.Cell(rownum, i + 1).Style.Border.SetBottomBorder(XLBorderStyleValues.None)
                        ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    ElseIf i = 3 Then
                        ws.Range("D" & rownum & ":" & "E" & rownum).Merge().Value = arrHeaders(i)
                    ElseIf i = 4 Then
                        ws.Range("F" & rownum & ":" & "G" & rownum).Merge().Value = arrHeaders(i)
                    ElseIf i = 5 Then
                        ws.Range("H" & rownum & ":" & "I" & rownum).Merge().Value = arrHeaders(i)
                    End If
                Next
                arrHeaders = {"", "", "", "Pax", amt, "Pax", amt, "Pax", amt}
                rownum += 1
                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Fill.SetBackgroundColor(XLColor.LightGray).Font.SetBold().Font.FontSize = 10
                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
                ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                For i = 0 To arrHeaders.Length - 1
                    If i < 3 Then
                        ws.Cell(rownum, i + 1).Style.Border.SetTopBorder(XLBorderStyleValues.None)
                    End If
                    ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                Next

                Dim grpby = From grp In custdetailsdt.AsEnumerable() Group grp By g = New With {Key .customer = grp.Field(Of String)("agentcode")} Into Group Order By g.customer
                For Each key In grpby
                    Dim k As Integer = 0
                    For Each row In key.Group
                        If k = 0 Then
                            arrHeaders = {row("agentname").ToString(), row("services").ToString(), row("servicetype").ToString(), IIf(row("reqpax") > 0, row("reqpax").ToString(), ""), Decimal.Parse(row("reqamt")).ToString(decimalPoint),
                                    IIf(row("confirmpax") > 0, row("confirmpax").ToString(), ""), Decimal.Parse(row("confirmamt")).ToString(decimalPoint), IIf(row("cancelpax") > 0, row("cancelpax").ToString(), ""), Decimal.Parse(row("cancelamt")).ToString(decimalPoint)}
                            k += 1
                        Else
                            arrHeaders = {"", row("services").ToString(), row("servicetype").ToString(), IIf(row("reqpax") > 0, row("reqpax").ToString(), ""), Decimal.Parse(row("reqamt")).ToString(decimalPoint),
                                     IIf(row("confirmpax") > 0, row("confirmpax").ToString(), ""), Decimal.Parse(row("confirmamt")).ToString(decimalPoint), IIf(row("cancelpax") > 0, row("cancelpax").ToString(), ""), Decimal.Parse(row("cancelamt")).ToString(decimalPoint)}
                        End If

                        rownum += 1
                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.FontSize = 9
                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                        ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
                        bpax = bpax + row("reqpax")
                        cfpax = cfpax + row("confirmpax")
                        cnpax = cnpax + row("cancelpax")
                        bamt = bamt + Decimal.Parse(row("reqamt"))
                        cfamt = cfamt + Decimal.Parse(row("confirmamt"))
                        cnamt = cnamt + Decimal.Parse(row("cancelamt"))
                        For i = 0 To arrHeaders.Length - 1
                            If i < 3 Then
                                ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                                ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            ElseIf i = 3 Or i = 5 Or i = 7 Then
                                ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                                ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                            Else
                                ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrHeaders(i))
                                ws.Cell(rownum, i + 1).Style.NumberFormat.Format = DecimalPoints
                                ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                            End If
                        Next
                    Next
                    '  arrHeaders = {"SubTotal", IIf(bpax > 0, bpax.ToString(), ""), Decimal.Parse(bamt).ToString(decimalPoint), IIf(cfpax > 0, cfpax.ToString(), ""), Decimal.Parse(cfamt).ToString(decimalPoint), IIf(cnpax > 0, cnpax.ToString(), ""), Decimal.Parse(cnamt).ToString(decimalPoint)}
                    arrHeaders = {"SubTotal", bpax.ToString(), Decimal.Parse(bamt).ToString(decimalPoint), cfpax.ToString(), Decimal.Parse(cfamt).ToString(decimalPoint), cnpax.ToString(), Decimal.Parse(cnamt).ToString(decimalPoint)}

                    rownum += 1
                    ws.Range(rownum, 1, rownum, arrHeaders.Length + 2).Style.Font.SetBold().Font.FontSize = 10
                    ws.Range(rownum, 1, rownum, arrHeaders.Length + 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    ws.Range(rownum, 1, rownum, arrHeaders.Length + 2).Style.Alignment.WrapText = True
                    For i = 0 To arrHeaders.Length - 1
                        If i = 0 Then
                            ws.Range("A" & rownum & ":C" & rownum).Value = arrHeaders(i)
                            ws.Range("A" & rownum & ":C" & rownum).Merge().Style.Fill.SetBackgroundColor(XLColor.LightGray).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ElseIf i = 1 Or i = 3 Or i = 5 Then
                            ws.Cell(rownum, i + 3).Value = arrHeaders(i)
                            ws.Cell(rownum, i + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        Else
                            ws.Cell(rownum, i + 3).Value = Decimal.Parse(arrHeaders(i))
                            ws.Cell(rownum, i + 3).Style.NumberFormat.Format = DecimalPoints
                            ws.Cell(rownum, i + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        End If
                    Next
                    fbpax = fbpax + bpax
                    fcfpax = fcfpax + cfpax
                    fcnpax = fcnpax + cnpax
                    fbamt = fbamt + bamt
                    fcfamt = fcfamt + cfamt
                    fcnamt = fcnamt + cnamt
                    bpax = 0
                    cfpax = 0
                    cnpax = 0
                    bamt = 0
                    cfamt = 0
                    cnamt = 0
                Next
                '  arrHeaders = {"Final Total", IIf(fbpax > 0, fbpax.ToString(), ""), Decimal.Parse(fbamt).ToString(decimalPoint), IIf(fcfpax > 0, fcfpax.ToString(), ""), Decimal.Parse(fcfamt).ToString(decimalPoint), IIf(fcnpax > 0, fcnpax.ToString(), ""), Decimal.Parse(fcnamt).ToString(decimalPoint)}
                arrHeaders = {"Final Total", fbpax.ToString(), Decimal.Parse(fbamt).ToString(decimalPoint), fcfpax.ToString(), Decimal.Parse(fcfamt).ToString(decimalPoint), fcnpax.ToString(), Decimal.Parse(fcnamt).ToString(decimalPoint)}
                rownum += 1
                ws.Range(rownum, 1, rownum, arrHeaders.Length + 2).Style.Font.SetBold().Font.FontSize = 10
                ws.Range(rownum, 1, rownum, arrHeaders.Length + 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(rownum, 1, rownum, arrHeaders.Length + 2).Style.Alignment.WrapText = True
                For i = 0 To arrHeaders.Length - 1
                    If i = 0 Then
                        ws.Range("A" & rownum & ":C" & rownum).Value = arrHeaders(i)
                        ws.Range("A" & rownum & ":C" & rownum).Merge().Style.Fill.SetBackgroundColor(XLColor.LightGray).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ElseIf i = 1 Or i = 3 Or i = 5 Then
                        ws.Cell(rownum, i + 3).Value = arrHeaders(i)
                        ws.Cell(rownum, i + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    Else
                        ws.Cell(rownum, i + 3).Value = Decimal.Parse(arrHeaders(i))
                        ws.Cell(rownum, i + 3).Style.NumberFormat.Format = DecimalPoints
                        ws.Cell(rownum, i + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    End If
                Next
            End If


            ws.Cell((rownum + 4), 1).Value = "Printed Date : " & Now.ToString("dd/MM/yyyy")
            ws.Range((rownum + 4), 1, (rownum + 4), 2).Merge()

            ' Dim bytes() As Byte
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                wb.Dispose()
                Response.Clear()
                Response.Buffer = True
                Response.AddHeader("content-disposition", "attachment;filename=BookingList" & Now.ToString("dd/MM/yyyy") & ".xlsx")

                Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"

                MyMemoryStream.WriteTo(Response.OutputStream)
                ' Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
                Response.Flush()
                HttpContext.Current.ApplicationInstance.CompleteRequest()

            End Using
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Records not found, Please redefine search criteria');", True)
        End If
    End Sub
#End Region


    Protected Sub custtype(ByVal sender As Object, ByVal e As EventArgs)
        Dim message As String = ddlsummdet.SelectedItem.Text
        If message.Equals("Summary") Then
            lbldatetype.InnerText = "Request Type"
            ddldatetype.Items.Clear()
            ddldatetype.Items.Add(New ListItem("All", "0"))
            ddldatetype.Items.Add(New ListItem("Hotel", "1"))
            ddldatetype.Items.Add(New ListItem("Transfers", "2"))
            ddldatetype.Items.Add(New ListItem("Tours", "3"))
            ddldatetype.Items.Add(New ListItem("AirportMA", "4"))
            ddldatetype.Items.Add(New ListItem("Others", "5"))
            ddldatetype.Items.Add(New ListItem("Visa", "6"))
        Else
            lbldatetype.InnerText = "Date Type"
            ddldatetype.Items.Clear()
            ddldatetype.Items.Add(New ListItem("Request Date", "0"))
            ddldatetype.Items.Add(New ListItem("Arrival Date", "1"))
            ddldatetype.Items.Add(New ListItem("User Date", "2"))
        End If
    End Sub

#Region "Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click"
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        txtCust.Text = ""
        txtCustCode.Text = ""
        txtUser.Text = ""
        txtUserCode.Text = ""
        txtFromDt.Text = Now.Date
        txtToDt.Text = Now.Date
        lbldatetype.InnerText = "Date Type"
        ddldatetype.Items.Clear()
        ddldatetype.Items.Add(New ListItem("Request Date", "0"))
        ddldatetype.Items.Add(New ListItem("Arrival Date", "1"))
        ddldatetype.Items.Add(New ListItem("User Date", "2"))
        ddlsummdet.Items.Clear()
        ddlsummdet.Items.Add(New ListItem("Detailed", "0"))
        ddlsummdet.Items.Add(New ListItem("Summary", "1"))
    End Sub
#End Region

End Class



