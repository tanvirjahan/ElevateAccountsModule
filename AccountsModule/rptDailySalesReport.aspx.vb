Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports ClosedXML.Excel
Imports System.IO
Imports System.Net
Imports System.Linq

Partial Class rptDailySalesReport
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
    Public Shared Function GetCustomers(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim customers As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If

            strSqlQry = "select agentcode,agentname from agentmast where active=1 and divcode='" + contextKey + "' and agentname like '%" & prefixText & "%' order by agentname asc"
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

                ddlGrpBy.Items.Add(New ListItem("Agent", 0))
                ddlGrpBy.Items.Add(New ListItem("User", 1))
                ddlGrpBy.Items.Add(New ListItem("Supplier", 2))
                ddlGrpBy.SelectedIndex = 0

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
            If txtCust.Text.Trim = "" Then txtCustCode.Text = ""
            If txtUser.Text.Trim = "" Then txtUserCode.Text = ""
            If txtParty.Text.Trim = "" Then txtPartyCode.Text = ""
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

        Dim fromdate, todate, rpttype, customer, type, amt, user, partyCode, party, rptcompanyname, rptname, custname, filter, rptfilter, filwidth, cols, decimalPoint, DecimalPoints, arrHeaders(), currcode, currDecno As String
        Dim rownum, datetype, reqtype, reporttype, bpax, fbpax As Integer
        Dim bamt, fbamt As Decimal

        fromdate = txtFromDt.Text.Trim
        todate = txtToDt.Text.Trim
        rpttype = "Detailed"
        customer = txtCustCode.Text.Trim
        custname = txtCust.Text.Trim
        user = txtUserCode.Text.Trim
        partyCode = txtPartyCode.Text.Trim
        party = txtParty.Text.Trim
        type = ddldatetype.SelectedItem.Text

        filter = ""
        If customer <> "" Then
            filter = "   Customer: " & custname
        End If
        If user <> "" Then
            filter = filter & "   User: " & txtUser.Text.Trim
        End If
        If partyCode <> "" Then
            filter = filter & "   Supplier: " & txtParty.Text.Trim
        End If
        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add("Daily Sales Report")
        rownum = 5

        ws.Columns("A").Width = 10
        ws.Columns("B").Width = 15
        ws.Columns("C").Width = 30
        ws.Columns("D").Width = 10
        ws.Columns("E").Width = 30
        ws.Columns("F").Width = 14
        ws.Columns("G").Width = 14
        ws.Columns("H").Width = 14
        ws.Columns("I").Width = 14
        datetype = ddldatetype.SelectedItem.Value
        reqtype = 0
        filwidth = 120
        cols = "H"
        reporttype = 1
        rptname = "Daily Sales Report"
        rptfilter = "From Date : " & fromdate & "   To Date : " & todate & "   Date Type: " & type & "  " & filter

        Dim sqlConn As New SqlConnection
        Dim mySqlCmd As New SqlCommand
        Dim myDataAdapter As New SqlDataAdapter
        Dim ds As New DataSet
        Dim myds As New DataSet
        sqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
        mySqlCmd = New SqlCommand("sp_rpt_dailysalesReport", sqlConn)
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(fromdate), "yyyy-MM-dd")
        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(todate), "yyyy-MM-dd")
        mySqlCmd.Parameters.Add(New SqlParameter("@fromstaff", SqlDbType.VarChar, 20)).Value = user
        mySqlCmd.Parameters.Add(New SqlParameter("@agentfrm", SqlDbType.VarChar, 20)).Value = customer
        mySqlCmd.Parameters.Add(New SqlParameter("@datetype", SqlDbType.Int)).Value = datetype
        mySqlCmd.Parameters.Add(New SqlParameter("@requesttype", SqlDbType.Int)).Value = reqtype
        mySqlCmd.Parameters.Add(New SqlParameter("@reporttype", SqlDbType.Int)).Value = reporttype
        mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = txtDivcode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@partyfrm", SqlDbType.VarChar, 20)).Value = txtPartyCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@groupby", SqlDbType.Int)).Value = ddlGrpBy.SelectedValue.Trim
        myDataAdapter.SelectCommand = mySqlCmd
        myDataAdapter.Fill(ds)
        Dim SalesRptdt As New DataTable
        SalesRptdt = ds.Tables(0)

        If txtDivcode.Text.Trim <> "" Then
            rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code='" & txtDivcode.Text.Trim & "'"), String)
        Else
            rptcompanyname = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select conm from columbusmaster "), String)
        End If

        If SalesRptdt.Rows.Count > 0 Then
            'Report Name Heading
            Dim company = ws.Range("A1:" & cols & "1").Merge()
            ws.Cell("A1").Value = rptcompanyname
            company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetBold().Font.FontSize = 15
            company.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0048C0"))
            company.Style.Font.FontColor = XLColor.Black
            company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            'Report Name Heading
            Dim company1 = ws.Range("A2:" & cols & "2").Merge()
            ws.Cell("A2").Value = rptname
            company1.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetBold().Font.FontSize = 14
            company1.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#0080C0"))
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


            currcode = SalesRptdt.AsEnumerable().Select(Function(s) s.Field(Of String)("currcode")).FirstOrDefault
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

            arrHeaders = {"Sl.No.", "Request ID", IIf(ddlGrpBy.SelectedValue = 0, "User Name", "Agent Name"), "Pax", IIf(ddlGrpBy.SelectedValue = 2, "User Name", "Hotel"), "Arrival Date", "Value in AED", "Status"}

            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#87CEEB")).Font.SetBold().Font.FontSize = 10
            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True
            ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            For i = 0 To arrHeaders.Length - 1
                ws.Cell(rownum, i + 1).Value = arrHeaders(i)
            Next

            Dim grpby As Object
            If ddlGrpBy.SelectedValue = 0 Then
                grpby = From grp In SalesRptdt.AsEnumerable() Group grp By g = New With {Key .code = grp.Field(Of String)("agentcode"), Key .name = grp.Field(Of String)("agentname")} Into Group Order By g.name
            ElseIf ddlGrpBy.SelectedValue = 1 Then
                grpby = From grp In SalesRptdt.AsEnumerable() Group grp By g = New With {Key .code = grp.Field(Of String)("usercode"), Key .name = grp.Field(Of String)("username")} Into Group Order By g.name
            Else
                grpby = From grp In SalesRptdt.AsEnumerable() Group grp By g = New With {Key .code = grp.Field(Of String)("partycode"), Key .name = grp.Field(Of String)("services")} Into Group Order By g.name
            End If
            Dim othservcount As Integer = 0
            Dim othservamount = 0
            Dim totconfirmamt, totreqamt, totcancelamt, totamendedamt As Decimal
            Dim totconfirmpax, totreqpax, totcancelpax, totamendedpax As Integer
            For Each key In grpby
                Dim k As Integer = 0
                For Each row In key.Group
                    If k = 0 Then
                        rownum += 1
                        Dim grpname As IXLRange = ws.Range("A" & rownum & ":H" & rownum).Merge()
                        If ddlGrpBy.SelectedValue = 0 Then
                            grpname.Value = row("agentname")
                        ElseIf ddlGrpBy.SelectedValue = 1 Then
                            grpname.Value = row("username")
                        Else
                            grpname.Value = row("services")
                        End If
                        grpname.Style.Fill.SetBackgroundColor(XLColor.LightGray)
                        grpname.Style.Font.SetBold().Font.FontSize = 10
                        grpname.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                        grpname.Style.Alignment.WrapText = True
                    End If

                    'Dim pax = IIf(row("confirmpax") > 0, row("confirmpax").ToString(), IIf(row("reqpax") > 0, row("reqpax").ToString(), IIf(row("cancelpax") > 0, row("cancelpax").ToString(), IIf(row("Amendedpax") > 0, row("Amendedpax").ToString(), 0))))
                    'Dim valbase = IIf(row("confirmpax") > 0, Decimal.Parse(row("confirmamt")).ToString(decimalPoint), IIf(row("reqpax") > 0, Decimal.Parse(row("reqamt")).ToString(decimalPoint), IIf(row("cancelpax") > 0, Decimal.Parse(row("cancelamt")).ToString(decimalPoint), IIf(row("Amendedpax") > 0, Decimal.Parse(row("Amendedamt")).ToString(decimalPoint), "0.00"))))
                    'Dim status = IIf(row("confirmpax") > 0, "Confirmed", IIf(row("reqpax") > 0, "Request", IIf(row("cancelpax") > 0, "Cancelled", IIf(row("Amendedpax") > 0, "Amended", ""))))

                    'Dim pax = IIf(row("confirmpax") > 0, row("confirmpax").ToString(), IIf(row("reqpax") > 0, row("reqpax").ToString(), IIf(row("cancelpax") > 0, row("cancelpax").ToString(), 0)))
                    Dim valbase = IIf(row("status").ToString().ToLower() = "confirmed", Decimal.Parse(row("confirmamt")).ToString(decimalPoint), IIf(row("status").ToString().ToLower() = "requested", Decimal.Parse(row("reqamt")).ToString(decimalPoint), IIf(row("status").ToString().ToLower() = "cancelled", Decimal.Parse(row("cancelamt")).ToString(decimalPoint), "0.00")))
                    Dim status = IIf(row("status").ToString().ToLower() = "confirmed", "Confirmed", IIf(row("status").ToString().ToLower = "requested", "Request", IIf(row("status").ToString().ToLower() = "cancelled", "Cancelled", "")))
                    Dim pax = Convert.ToInt32(row("confirmpax")) + Convert.ToInt32(row("reqpax")) + Convert.ToInt32(row("cancelpax"))
                    If row("services") = "OTHER SERVICE PROVIDER" Then
                        othservcount += pax
                        othservamount += valbase
                    End If
                    If row("status").ToString().ToLower() = "confirmed" Then
                        totconfirmamt += Decimal.Parse(row("confirmamt"))
                        totconfirmpax += Decimal.Parse(row("confirmpax"))
                    End If
                    If row("status").ToString().ToLower() = "requested" Then
                        totreqamt += Decimal.Parse(row("reqamt"))
                        totreqpax += Decimal.Parse(row("reqpax"))
                    End If
                    If row("status").ToString().ToLower() = "cancelled" Then
                        totcancelamt += Decimal.Parse(row("cancelamt"))
                        totcancelpax += Decimal.Parse(row("cancelpax"))
                        'ElseIf row("Amendedpax") > 0 Then
                        '    totamendedamt += Decimal.Parse(row("Amendedamt"))
                        '    totamendedpax += Decimal.Parse(row("Amendedpax"))
                    End If
                    fbpax = fbpax + pax
                    fbamt = fbamt + valbase
                    arrHeaders = {k + 1, row("requestid").ToString(), IIf(ddlGrpBy.SelectedValue = 0, row("username").ToString(), row("agentname").ToString()), pax, IIf(ddlGrpBy.SelectedValue = 2, row("username").ToString(), row("services").ToString()), row("arrivaldate").ToString(), valbase, status}
                    k += 1

                    rownum += 1
                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Font.FontSize = 9
                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    ws.Range(rownum, 1, rownum, arrHeaders.Length).Style.Alignment.WrapText = True

                    bpax = bpax + pax
                    bamt = bamt + valbase

                    For i = 0 To arrHeaders.Length - 1
                        If i = 1 Or i = 3 Then
                            ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                            ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        ElseIf i = 6 Then
                            ws.Cell(rownum, i + 1).Value = Decimal.Parse(arrHeaders(i))
                            ws.Cell(rownum, i + 1).Style.NumberFormat.Format = DecimalPoints
                            ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        Else
                            ws.Cell(rownum, i + 1).Value = arrHeaders(i)
                            ws.Cell(rownum, i + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        End If
                    Next
                Next

                arrHeaders = {"SubTotal", bpax.ToString(), Decimal.Parse(bamt).ToString(decimalPoint)}

                rownum += 1
                ws.Range(rownum, 1, rownum, 8).Style.Font.SetBold().Font.FontSize = 10
                ws.Range(rownum, 1, rownum, 8).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(rownum, 1, rownum, 8).Style.Alignment.WrapText = True
                For i = 0 To arrHeaders.Length - 1
                    If i = 0 Then
                        ws.Range("A" & rownum & ":C" & rownum).Value = arrHeaders(i)
                        ws.Range("A" & rownum & ":C" & rownum).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ElseIf i = 1 Then
                        ws.Cell(rownum, i + 3).Value = arrHeaders(i)
                        ws.Cell(rownum, i + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    Else
                        ws.Cell(rownum, i + 5).Value = Decimal.Parse(arrHeaders(i))
                        ws.Cell(rownum, i + 5).Style.NumberFormat.Format = DecimalPoints
                        ws.Cell(rownum, i + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    End If
                Next
                'fbpax = fbpax + bpax
                'fbamt = fbamt + bamt
                bpax = 0
                bamt = 0.0
                k = 0
            Next
            arrHeaders = {"Pax - With Hotel Bookings", fbpax - othservcount, Decimal.Parse(fbamt - othservamount).ToString(decimalPoint)}
            rownum += 1
            ws.Range(rownum, 1, rownum, 8).Style.Font.SetBold().Font.FontSize = 10
            ws.Range(rownum, 1, rownum, 8).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(rownum, 1, rownum, 8).Style.Fill.SetBackgroundColor(XLColor.LightGray).Alignment.WrapText = True
            For i = 0 To arrHeaders.Length - 1
                If i = 0 Then
                    ws.Range("A" & rownum & ":C" & rownum).Value = arrHeaders(i)
                    ws.Range("A" & rownum & ":C" & rownum).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                ElseIf i = 1 Then
                    ws.Cell(rownum, i + 3).Value = arrHeaders(i)
                    ws.Cell(rownum, i + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                Else
                    ws.Cell(rownum, i + 5).Value = Decimal.Parse(arrHeaders(i))
                    ws.Cell(rownum, i + 5).Style.NumberFormat.Format = DecimalPoints
                    ws.Cell(rownum, i + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                End If
            Next
            arrHeaders = {"Pax - With Only Other Services", othservcount.ToString(), Decimal.Parse(othservamount).ToString(decimalPoint)}
            rownum += 1
            ws.Range(rownum, 1, rownum, 8).Style.Font.SetBold().Font.FontSize = 10
            ws.Range(rownum, 1, rownum, 8).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(rownum, 1, rownum, 8).Style.Fill.SetBackgroundColor(XLColor.LightGray).Alignment.WrapText = True
            For i = 0 To arrHeaders.Length - 1
                If i = 0 Then
                    ws.Range("A" & rownum & ":C" & rownum).Value = arrHeaders(i)
                    ws.Range("A" & rownum & ":C" & rownum).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                ElseIf i = 1 Then
                    ws.Cell(rownum, i + 3).Value = arrHeaders(i)
                    ws.Cell(rownum, i + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                Else
                    ws.Cell(rownum, i + 5).Value = Decimal.Parse(arrHeaders(i))
                    ws.Cell(rownum, i + 5).Style.NumberFormat.Format = DecimalPoints
                    ws.Cell(rownum, i + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                End If
            Next


            arrHeaders = {"Final Total", fbpax.ToString(), Decimal.Parse(fbamt).ToString(decimalPoint)}
            rownum += 1
            ws.Range(rownum, 1, rownum, 8).Style.Font.SetBold().Font.FontSize = 10
            ws.Range(rownum, 1, rownum, 8).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(rownum, 1, rownum, 8).Style.Fill.SetBackgroundColor(XLColor.LightGray).Alignment.WrapText = True
            For i = 0 To arrHeaders.Length - 1
                If i = 0 Then
                    ws.Range("A" & rownum & ":C" & rownum).Value = arrHeaders(i)
                    ws.Range("A" & rownum & ":C" & rownum).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                ElseIf i = 1 Then
                    ws.Cell(rownum, i + 3).Value = arrHeaders(i)
                    ws.Cell(rownum, i + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                Else
                    ws.Cell(rownum, i + 5).Value = Decimal.Parse(arrHeaders(i))
                    ws.Cell(rownum, i + 5).Style.NumberFormat.Format = DecimalPoints
                    ws.Cell(rownum, i + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                End If
            Next
            arrHeaders = {"Confirmed", totconfirmpax.ToString(), Decimal.Parse(totconfirmamt).ToString(decimalPoint),
                        "Cancelled", totcancelpax.ToString(), Decimal.Parse(totcancelamt).ToString(decimalPoint),
                        "Request", totreqpax.ToString(), Decimal.Parse(totreqamt).ToString(decimalPoint),
                        "Final Total", (totconfirmpax + totcancelpax + totreqpax).ToString(), Decimal.Parse(totconfirmamt + totcancelamt + totreqamt).ToString(decimalPoint)
                        }

            '"Amended", totamendedpax.ToString(), Decimal.Parse(totamendedamt).ToString(decimalPoint),
            '"Final Total", (totconfirmpax + totcancelpax + totamendedpax + totreqpax).ToString(), Decimal.Parse(totconfirmamt + totcancelamt + totamendedamt + totreqamt).ToString(decimalPoint)
            rownum += 2
            ws.Range(rownum, 1, rownum + 3, 3).Style.Font.SetBold().Font.FontSize = 10
            ws.Range(rownum, 1, rownum + 3, 3).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(rownum, 1, rownum + 3, 3).Style.Fill.SetBackgroundColor(XLColor.LightGray).Alignment.WrapText = True
            Dim colindex As Integer = 0

            For i = 0 To arrHeaders.Length - 1
                colindex += 1
                If colindex = 1 Then
                    ws.Cell(rownum, 1).Value = arrHeaders(i)
                    ws.Cell(rownum, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                ElseIf colindex = 2 Then
                    ws.Cell(rownum, 2).Value = arrHeaders(i)
                    ws.Cell(rownum, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                ElseIf colindex = 3 Then
                    ws.Cell(rownum, 3).Value = arrHeaders(i)
                    ws.Cell(rownum, 3).Style.NumberFormat.Format = DecimalPoints
                    ws.Cell(rownum, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    rownum += 1
                    colindex = 0
                End If
            Next
            mySqlCmd = New SqlCommand("sp_rpt_dailysalesReportnew_monthwise", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(fromdate), "yyyy-MM-dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 20)).Value = Format(Convert.ToDateTime(todate), "yyyy-MM-dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@fromstaff", SqlDbType.VarChar, 20)).Value = user
            mySqlCmd.Parameters.Add(New SqlParameter("@agentfrm", SqlDbType.VarChar, 20)).Value = customer
            mySqlCmd.Parameters.Add(New SqlParameter("@datetype", SqlDbType.Int)).Value = datetype
            mySqlCmd.Parameters.Add(New SqlParameter("@requesttype", SqlDbType.Int)).Value = reqtype
            mySqlCmd.Parameters.Add(New SqlParameter("@reporttype", SqlDbType.Int)).Value = reporttype
            mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = txtDivcode.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@partyfrm", SqlDbType.VarChar, 20)).Value = txtPartyCode.Text.Trim
            myDataAdapter.SelectCommand = mySqlCmd
            myDataAdapter.Fill(myds)
            Dim monthdt As New DataTable
            monthdt = myds.Tables(0)
            Dim cnos, nos, anos, rnos As Integer
            Dim mname As String = monthdt.Rows(0)("month1name")

            nos = monthdt.Rows(0)("month1nos") + monthdt.Rows(0)("month2nos") + monthdt.Rows(0)("month3nos") + monthdt.Rows(0)("month4nos") + monthdt.Rows(0)("month5nos") + monthdt.Rows(0)("month6nos") + monthdt.Rows(0)("month7nos")
            cnos = monthdt.Rows(0)("month1cnos") + monthdt.Rows(0)("month2cnos") + monthdt.Rows(0)("month3cnos") + monthdt.Rows(0)("month4cnos") + monthdt.Rows(0)("month5cnos") + monthdt.Rows(0)("month6cnos") + monthdt.Rows(0)("month7cnos")
            anos = monthdt.Rows(0)("month1anos") + monthdt.Rows(0)("month2anos") + monthdt.Rows(0)("month3anos") + monthdt.Rows(0)("month4anos") + monthdt.Rows(0)("month5anos") + monthdt.Rows(0)("month6anos") + monthdt.Rows(0)("month7anos")
            rnos = monthdt.Rows(0)("month1rnos") + monthdt.Rows(0)("month2rnos") + monthdt.Rows(0)("month3rnos") + monthdt.Rows(0)("month4rnos") + monthdt.Rows(0)("month5rnos") + monthdt.Rows(0)("month6rnos") + monthdt.Rows(0)("month7rnos")
            arrHeaders = {"", mname, monthdt.Rows(0)("month2name"), monthdt.Rows(0)("month3name").ToString(), monthdt.Rows(0)("month4name").ToString(), monthdt.Rows(0)("month5name").ToString(), monthdt.Rows(0)("month6name").ToString(), monthdt.Rows(0)("month7name").ToString(), "Total",
                            "Confirmed", monthdt.Rows(0)("month1nos").ToString(), monthdt.Rows(0)("month2nos").ToString(), monthdt.Rows(0)("month3nos").ToString(), monthdt.Rows(0)("month4nos").ToString(), monthdt.Rows(0)("month5nos").ToString(), monthdt.Rows(0)("month6nos").ToString(), monthdt.Rows(0)("month7nos").ToString(), nos.ToString(),
                            "Cancelled", monthdt.Rows(0)("month1cnos").ToString(), monthdt.Rows(0)("month2cnos").ToString(), monthdt.Rows(0)("month3cnos").ToString(), monthdt.Rows(0)("month4cnos").ToString(), monthdt.Rows(0)("month5cnos").ToString(), monthdt.Rows(0)("month6cnos").ToString(), monthdt.Rows(0)("month7cnos").ToString(), cnos.ToString(),
                            "Request", monthdt.Rows(0)("month1rnos").ToString(), monthdt.Rows(0)("month2rnos").ToString(), monthdt.Rows(0)("month3rnos").ToString(), monthdt.Rows(0)("month4rnos").ToString(), monthdt.Rows(0)("month5rnos").ToString(), monthdt.Rows(0)("month6rnos").ToString(), monthdt.Rows(0)("month7rnos").ToString(), rnos.ToString()
                         }
            '"Amended", monthdt.Rows(0)("month1anos").ToString(), monthdt.Rows(0)("month2anos").ToString(), monthdt.Rows(0)("month3anos").ToString(), monthdt.Rows(0)("month4anos").ToString(), monthdt.Rows(0)("month5anos").ToString(), monthdt.Rows(0)("month6anos").ToString(), monthdt.Rows(0)("month7anos").ToString(), anos.ToString(),
            rownum += 2
            ws.Range(rownum, 1, rownum + 3, 9).Style.Font.SetBold().Font.FontSize = 10
            ws.Range(rownum, 1, rownum + 3, 9).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(rownum, 1, rownum + 3, 9).Style.Fill.SetBackgroundColor(XLColor.LightGray).Alignment.WrapText = True
            colindex = 0
            ws.Range(rownum, 2, rownum, 8).Style.NumberFormat.Format = "MMM-yy"






            For i = 0 To arrHeaders.Length - 1
                colindex += 1
                If colindex = 9 Then
                    ws.Cell(rownum, colindex).Value = arrHeaders(i)
                    ws.Cell(rownum, colindex).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    rownum += 1
                    colindex = 0
                Else
                    ws.Cell(rownum, colindex).Value = arrHeaders(i)
                    ws.Cell(rownum, colindex).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                End If
            Next

            ws.Cell((rownum + 4), 1).Value = "Printed Date : " & Now.ToString("dd/MM/yyyy")
            ws.Range((rownum + 4), 1, (rownum + 4), 2).Merge()

            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                wb.Dispose()
                Response.Clear()
                Response.Buffer = True
                Response.AddHeader("content-disposition", "attachment;filename=DailySalesReport" & Now.ToString("ddMMyyyyHHmmss") & ".xlsx")
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
        txtCust.Text = ""
        txtCustCode.Text = ""
        txtUser.Text = ""
        txtUserCode.Text = ""
        txtParty.Text = ""
        txtPartyCode.Text = ""
        txtFromDt.Text = Now.Date
        txtToDt.Text = Now.Date
    End Sub
#End Region

End Class
