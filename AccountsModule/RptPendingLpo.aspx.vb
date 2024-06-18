Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports ClosedXML.Excel
Imports System.IO
Imports System.Net
Imports System.Linq

Partial Class RptPendingLpo
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
    '    <System.Web.Script.Services.ScriptMethod()> _
    '<System.Web.Services.WebMethod()> _
    '    Public Shared Function GetCustomers(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
    '        Dim strSqlQry As String = ""
    '        Dim myDS As New DataSet
    '        Dim customers As New List(Of String)
    '        Try
    '            If prefixText = " " Then
    '                prefixText = ""
    '            End If

    '            strSqlQry = "select agentcode,agentname from agentmast where active=1 and divcode='" + contextKey + "' and agentname like '%" & prefixText & "%' order by agentname asc"
    '            Dim SqlConn As New SqlConnection
    '            Dim myDataAdapter As New SqlDataAdapter
    '            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
    '            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
    '            myDataAdapter.Fill(myDS)
    '            If myDS.Tables(0).Rows.Count > 0 Then
    '                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
    '                    customers.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("agentcode").ToString()))
    '                Next
    '            End If
    '            Return customers
    '        Catch ex As Exception
    '            Return customers
    '        End Try
    '    End Function

   

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetSuppliers(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppliers As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select partycode,partyname from partymast(nolock) where active=1 and partyname like '%" & prefixText & "%' order by partyname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    suppliers.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))
                Next
            End If
            Return suppliers
        Catch ex As Exception
            Return suppliers
        End Try
    End Function
#End Region

#Region "Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit"
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If IsPostBack = False Then
            Dim appid As String = CType(Request.QueryString("appid"), String)
            If Request.QueryString("appid") Is Nothing = False Then
                Select Case appid
                    Case 1
                        Me.MasterPageFile = "~/PriceListMaster.master"
                    Case 2
                        Me.MasterPageFile = "~/RoomBlock.master"
                    Case 3
                        Me.MasterPageFile = "~/ReservationMaster.master"
                    Case 4
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 5
                        Me.MasterPageFile = "~/UserAdminMaster.master"
                    Case 6
                        Me.MasterPageFile = "~/WebAdminMaster.master"
                    Case 7
                        Me.MasterPageFile = "~/TransferHistoryMaster.master"
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 16
                        Me.MasterPageFile = "~/AccountsMaster.master"   '' Added shahul MCP accounts
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
            Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
            Dim strappname As String = ""
            strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(displayName,'') as displayname from appmaster a inner join division_master d on a.displayname=d.accountsmodulename where a.appid='" & appid & "'")
            ViewState("Appname") = strappname
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")
            ViewState.Add("divcode", divid)
        End If
    End Sub
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
                                                       CType(strappname, String), "AccountsModule\rptDailySalesReport.aspx?appid=" & strappid, btnAddNew, btnLoadReport, _
                                                       btnprint, gvSearch:=gvSearchResult)
                txtDivcode.Text = ViewState("divcode")

                ddlServiceType.Items.Add(New ListItem("All", 0))
                ddlServiceType.Items.Add(New ListItem("Hotel", 1))
                ddlServiceType.Items.Add(New ListItem("Visa", 2))
                ddlServiceType.Items.Add(New ListItem("AirportMA", 3))
                ddlServiceType.Items.Add(New ListItem("Others", 4))
                ddlServiceType.Items.Add(New ListItem("Tours", 5))
                ddlServiceType.Items.Add(New ListItem("Transfers", 6))
                ddlServiceType.SelectedIndex = 0

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptDailySalesReport.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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


            If txtParty.Text.trim = "" Then txtPartyCode.text = ""
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
            objUtils.WritErrorLog("rptDailySalesReport.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected sub ExcelReport() "
    Public Sub ExcelReport()

        Dim fromdate, todate, type, partyCode, party, rptcompanyname, rptname, filter, rptfilter, filwidth, lastcol, decimalPoint, DecimalPoints, arrHeaders(), arrTotal(), currcode, currDecno As String
        Dim rownum As Integer

        Dim stotalval, stotaltaxval, stotalnontaxval, stotalvatval, totalval, totaltaxval, totalnontaxval, totalvatval As Decimal

        fromdate = txtFromDt.Text.Trim
        todate = txtToDt.Text.Trim
        partyCode = txtPartyCode.Text.Trim
        party = txtParty.Text.Trim
        type = ddlServiceType.SelectedItem.Text()
        Dim typevalue = ddlServiceType.SelectedValue
        ' typevalue = "3"
        filter = ""
        If partyCode <> "" Then
            filter = filter & "   Supplier: " & txtParty.Text.Trim
        End If
        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add("Pending LPO")
        Dim lastcolnum As Integer
        rownum = 5

        ws.Columns("A").Width = 13
        ws.Columns("B").Width = 11
        ws.Columns("C").Width = 11
        ws.Columns("D").Width = 35
        ws.Columns("E").Width = 5
        ws.Columns("F").Width = 5
        ws.Columns("G").Width = 14
        ws.Columns("H").Width = 14
        ws.Columns("I").Width = 15
        ws.Columns("J").Width = 14
        ws.Columns("K").Width = 15
        ws.Columns("L").Width = 14
        filwidth = 120


        rptname = "Pending LPO Report"
        rptfilter = "From Date : " & fromdate & "   To Date : " & todate & "   Service Type: " & type & "  " & filter

        Dim sqlConn As New SqlConnection
        Dim mySqlCmd As New SqlCommand
        Dim myDataAdapter As New SqlDataAdapter
        Dim ds As New DataSet
        sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
        mySqlCmd = New SqlCommand("sp_rpt_pendingPurchase", sqlConn)
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = txtDivcode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(fromdate), "yyyy/MM/dd")
        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(todate), "yyyy/MM/dd")
        mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = txtPartyCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.Int, 20)).Value = ddlServiceType.SelectedValue

        myDataAdapter.SelectCommand = mySqlCmd
        mySqlCmd.CommandTimeout = 0
        myDataAdapter.Fill(ds)
        Dim SalesRptdt As New DataTable
        SalesRptdt = ds.Tables(0)

        If txtDivcode.Text.Trim <> "" Then
            rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & txtDivcode.Text.Trim & "'"), String)
        Else
            rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
        End If

        If SalesRptdt.Rows.Count > 0 Then
            totalval = IIf(IsDBNull(SalesRptdt.Compute("SUM(costvalue)", "")), 0.0, SalesRptdt.Compute("SUM(costvalue)", ""))

            'for hotel and all add taxable,nontaxable,vat value
            If (typevalue = "0" Or typevalue = "1") Then
                lastcol = "J"
                totaltaxval = IIf(IsDBNull(SalesRptdt.Compute("SUM(taxablevalue)", "")), 0.0, SalesRptdt.Compute("SUM(taxablevalue)", ""))
                totalnontaxval = IIf(IsDBNull(SalesRptdt.Compute("SUM(nontaxablevalue)", "")), 0.0, SalesRptdt.Compute("SUM(nontaxablevalue)", ""))
                totalvatval = IIf(IsDBNull(SalesRptdt.Compute("SUM(vatvalue)", "")), 0.0, SalesRptdt.Compute("SUM(vatvalue)", ""))
            Else
                lastcol = "G"

            End If



            ws.Row(1).Height = 20
            ws.Row(1).Height = 20
            'Report Name Heading
            Dim company = ws.Range("A1:" & lastcol & "1").Merge()
            ws.Cell("A1").Value = rptcompanyname
            company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetBold().Font.FontSize = 15
            company.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0048C0"))
            company.Style.Font.FontColor = XLColor.Black
            company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            'Report Name Heading

            Dim company1 = ws.Range("A2:" & lastcol & "2").Merge()
            ws.Cell("A2").Value = rptname
            company1.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetBold().Font.FontSize = 14
            company1.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0080C0"))
            company1.Style.Font.FontColor = XLColor.Black
            company1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            company1.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            'Report filter Heading

            ws.Cell("A3").Value = rptfilter
            Dim report = ws.Range("A3:" & lastcol & "3").Merge()
            report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 12
            report.Style.Alignment.SetWrapText().Font.FontColor = XLColor.Black


            If rptfilter.Length > filwidth Then
                Dim rowheight = IIf(rptfilter.Length > filwidth And rptfilter.Length < (filwidth + filwidth), 32, 48)
                ws.Row(3).Height = rowheight
            End If


            ' currcode = SalesRptdt.AsEnumerable().Select(Function(s) s.Field(Of String)("currcode")).FirstOrDefault
            ' currDecno = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select isnull(nodigit,0) as nodigit from currmast where currcode= '" & currcode & "'"), Integer)
            decimalPoint = "N" & "2"



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



            'for hotel and all add taxable,nontaxable,vat value
            If (typevalue = "0" Or typevalue = "1") Then
                arrHeaders = {"LPO No", "LPO Date", "Resn.No", "Particulars", "Night", "Units", "Value", "Taxable Value", "Non-Taxable Value", "Vat Value"}
                arrTotal = {"Final Total", Decimal.Parse(totalval).ToString(decimalPoint), Decimal.Parse(totaltaxval).ToString(decimalPoint), Decimal.Parse(totalnontaxval).ToString(decimalPoint), Decimal.Parse(totalvatval).ToString(decimalPoint)}
                lastcolnum = 10
            Else
                arrHeaders = {"LPO No", "LPO Date", "Resn.No", "Particulars", "Night", "Units", "Value"}
                lastcolnum = 7
                arrTotal = {"Final Total", Decimal.Parse(totalval).ToString(decimalPoint)}

            End If


            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 10
            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)

            For i = 0 To arrHeaders.Length - 1
                ws.Cell(rownum, i + 1).Value = arrHeaders(i)
            Next

            Dim grpby As Object

            grpby = From grp In SalesRptdt.AsEnumerable() Group grp By g = New With {Key .code = grp.Field(Of String)("partycode"), Key .name = grp.Field(Of String)("partyname")} Into Group Order By g.name

            For Each key In grpby
                Dim k As Integer = 0
                For Each row In key.Group
                    If k = 0 Then
                        rownum += 1
                        Dim grpname As IXLRange = ws.Range("A" & rownum & ":" + lastcol & rownum).Merge()

                        grpname.Value = "Service Provider:" & row("partycode") & Space(10) & row("partyname")

                        '  grpname.Style.Fill.SetBackgroundColor(XLColor.LightGray)
                        grpname.Style.Font.SetBold().Font.FontSize = 10
                        grpname.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                        grpname.Style.Alignment.WrapText = True
                    End If

                    stotalval = stotalval + IIf(IsDBNull(row("costvalue")), 0.0, row("costvalue"))

                    If (typevalue = "0" Or typevalue = "1") Then
                        stotaltaxval = stotaltaxval + IIf(IsDBNull(row("taxablevalue")), 0.0, row("taxablevalue"))
                        stotalnontaxval = stotalnontaxval + IIf(IsDBNull(row("nontaxablevalue")), 0.0, row("nontaxablevalue"))
                        stotalvatval = stotalvatval + IIf(IsDBNull(row("vatvalue")), 0.0, row("vatvalue"))

                        arrHeaders = {row("invoiceno").ToString(), IIf(IsDBNull(row("invoicedate")), "", (row("invoicedate")).ToString()), row("requestid").ToString(), IIf(IsDBNull(row("services")), "", (row("services")).ToString()), IIf(IsDBNull(row("nights")), "", (row("nights")).ToString()), IIf(IsDBNull(row("units")), "", (row("units")).ToString()), IIf(IsDBNull(row("costvalue")), "", (row("costvalue")).ToString()), IIf(IsDBNull(row("taxablevalue")), "", (row("taxablevalue")).ToString()), IIf(IsDBNull(row("nontaxablevalue")), "", (row("nontaxablevalue")).ToString()), IIf(IsDBNull(row("vatvalue")), "", (row("vatvalue")).ToString())}
                    Else
                        arrHeaders = {row("invoiceno").ToString(), IIf(IsDBNull(row("invoicedate")), "", (row("invoicedate")).ToString()), row("requestid").ToString(), IIf(IsDBNull(row("services")), "", (row("services")).ToString()), IIf(IsDBNull(row("nights")), "", (row("nights")).ToString()), IIf(IsDBNull(row("units")), "", (row("units")).ToString()), IIf(IsDBNull(row("costvalue")), "", (row("costvalue")).ToString())}

                    End If
                    k += 1
                    rownum += 1

                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.FontSize = 9
                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True



                    For i = 0 To arrHeaders.Length - 1
                        If i >= 6 Then
                            ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrHeaders(i))
                            ws.Cell(rownum, i + 1).Style.NumberFormat.Format = DecimalPoints
                            ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ElseIf i = 4 Or i = 5 Then
                            ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                            ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        Else
                            ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                            ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        End If
                    Next
                Next

                If (typevalue = "0" Or typevalue = "1") Then
                    arrHeaders = {"S. Provider Total", Decimal.Parse(stotalval).ToString(decimalPoint), Decimal.Parse(stotaltaxval).ToString(decimalPoint), Decimal.Parse(stotalnontaxval).ToString(decimalPoint), Decimal.Parse(stotalvatval).ToString(decimalPoint)}
                Else
                    arrHeaders = {"S. Provider Total", Decimal.Parse(stotalval).ToString(decimalPoint)}
                End If


                rownum += 1
                ws.Range(rownum, 1, rownum, lastcolnum).Style.Font.SetBold().Font.FontSize = 10
                ws.Range(rownum, 1, rownum, lastcolnum).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(rownum, 1, rownum, lastcolnum).Style.Alignment.WrapText = True

                For i = 0 To arrHeaders.Length - 1
                    If i = 0 Then
                        ws.Range("A" & rownum & ":F" & rownum).Value = arrHeaders(i)
                        ws.Range("A" & rownum & ":F" & rownum).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    Else
                        ws.Cell(rownum, i + 6).Value = Decimal.Parse(arrHeaders(i))
                        ws.Cell(rownum, i + 6).Style.NumberFormat.Format = DecimalPoints
                        ws.Cell(rownum, i + 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    End If
                Next
                stotalnontaxval = 0.0
                stotaltaxval = 0.0
                stotalval = 0.0
                stotalvatval = 0.0

                k = 0
            Next


            rownum += 1

            ws.Range(rownum, 1, rownum, lastcolnum).Style.Font.SetBold().Font.FontSize = 10
            ws.Range(rownum, 1, rownum, lastcolnum).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(rownum, 1, rownum, lastcolnum).Style.Alignment.WrapText = True

            For i = 0 To arrTotal.Length - 1
                If i = 0 Then
                    ws.Range("A" & rownum & ":F" & rownum).Value = arrTotal(i)
                    ws.Range("A" & rownum & ":F" & rownum).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

                Else
                    ws.Cell(rownum, i + 6).Value = Decimal.Parse(arrTotal(i))
                    ws.Cell(rownum, i + 6).Style.NumberFormat.Format = DecimalPoints
                    ws.Cell(rownum, i + 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                End If
            Next

            ws.Cell((rownum + 4), 1).Value = "Printed Date : " & Now.ToString("dd/MM/yyyy")
            ws.Range((rownum + 4), 1, (rownum + 4), 2).Merge()

            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                wb.Dispose()
                Response.Clear()
                Response.Buffer = True
                Response.AddHeader("content-disposition", "attachment;filename=PendingLpo" & Now.ToString("ddMMyyyyHHmmss") & ".xlsx")
                Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Cookies.Add(New HttpCookie("DownloadDailySalesReport", "True"))
                Response.Flush()
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            End Using
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Records not found, Please redefine search criteria');", True)
        End If
    End Sub
#End Region

#Region "Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click"
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
       
       
        txtParty.Text = ""
        txtPartyCode.Text = ""
        txtFromDt.Text = Now.Date
        txtToDt.Text = Now.Date
    End Sub
#End Region

End Class
