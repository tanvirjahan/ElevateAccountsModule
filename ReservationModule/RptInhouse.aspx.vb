Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports ClosedXML.Excel
Imports System.IO
Imports System.Net
Imports System.Linq

Partial Class RptInhouse
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
    Public Shared Function GetCountryGroup(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim countryGroup As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select countrygroupcode,countrygroupname from countrygroup(nolock) where active=1 and countrygroupname like '%" & prefixText & "%' " &
            "order by countrygroupname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    countryGroup.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("countrygroupname").ToString(), myDS.Tables(0).Rows(i)("countrygroupcode").ToString()))
                Next
            End If
            Return countryGroup
        Catch ex As Exception
            Return countryGroup
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
 <System.Web.Services.WebMethod()> _
    Public Shared Function GetCustomerGroup(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim customerGroup As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select customergroupcode,customergroupname from Customergroup where active=1 and customergroupname like '%" & prefixText & "%' " &
            "order by customergroupname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    customerGroup.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("customergroupname").ToString(), myDS.Tables(0).Rows(i)("customergroupcode").ToString()))
                Next
            End If
            Return customerGroup
        Catch ex As Exception
            Return customerGroup
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
 <System.Web.Services.WebMethod()> _
    Public Shared Function GetSourceCountry(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim sourceCtry As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select ctrycode,ctryname from ctrymast where active=1 and ctryname like '%" & prefixText & "%' order by ctryname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    sourceCtry.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("ctryname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))
                Next
            End If
            Return sourceCtry
        Catch ex As Exception
            Return sourceCtry
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetCustomerCategory(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim custCategory As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select agentcatcode,agentcatname from agentcatmast where active=1 and agentcatname like '%" & prefixText & "%' order by agentcatname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    custCategory.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentcatname").ToString(), myDS.Tables(0).Rows(i)("agentcatcode").ToString()))
                Next
            End If
            Return custCategory
        Catch ex As Exception
            Return custCategory
        End Try
    End Function

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
    Public Shared Function GetHotelCity(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim hotelCity As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select citycode,cityname from citymast c(nolock) inner join reservation_parameters r(nolock) on ctrycode= option_selected " &
            "where active=1 and param_id=459 and cityname like '%" & prefixText & "%' order by cityname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    hotelCity.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("cityname").ToString(), myDS.Tables(0).Rows(i)("citycode").ToString()))
                Next
            End If
            Return hotelCity
        Catch ex As Exception
            Return hotelCity
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetHotelCategory(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim hotelCategory As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select catcode,catname from catmast where sptypecode='HOT' and active=1 and catname like '%" & prefixText & "%' order by catname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    hotelCategory.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("catname").ToString(), myDS.Tables(0).Rows(i)("catcode").ToString()))
                Next
            End If
            Return hotelCategory
        Catch ex As Exception
            Return hotelCategory
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetHotels(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim hotels As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select partycode,partyname from partymast where sptypecode='HOT' and active=1 and partyname like '%" & prefixText & "%' order by partyname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    hotels.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))
                Next
            End If
            Return hotels
        Catch ex As Exception
            Return hotels
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
                If txtRptType.Text.Trim = "Inhouse" Then
                    lblHeading.Text = "Inhouse Report"
                    Me.Page.Title = "Inhouse Report"
                ElseIf txtRptType.Text.Trim = "E" Then
                    '  lblHeading.Text = "Expected Reservations Report"
                    ' Me.Page.Title = "Expected Reservations Report"
                    lblHeading.Text = "Production Report"
                    Me.Page.Title = "Production Report"

                    lbldatetype.Visible = True
                    ddldatetype.Visible = True
                    frmdate.Text = "From Date"
                    lbltodate.Text = "To Date"

                End If

                If AppId.Value Is Nothing = False Then
                    strappid = AppId.Value
                End If
                strappname = objUser.GetAppName(Session("dbconnectionName"), strappid)

                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "ReservationModule\RptInhouse.aspx?type=" + txtRptType.Text.Trim, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gvSearch:=gvSearchResult)

                ddlGuestName.Items.Add(New ListItem("Lead Guest Name", "0"))
                ddlGuestName.Items.Add(New ListItem("All Guest Names", "1"))
                ddlGuestName.SelectedIndex = 0

                ddlBookingStatus.Items.Add(New ListItem("All", "1"))
                ddlBookingStatus.Items.Add(New ListItem("Confirmed", "0"))
                ddlBookingStatus.SelectedIndex = 1

                ddlGroupBy.Items.Add(New ListItem("Customer", "0"))
                ddlGroupBy.Items.Add(New ListItem("Hotel", "1"))
                ddlGroupBy.SelectedIndex = 1
                'Added by abin on 20181229
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myCommand = New SqlCommand("select division_master_code as divcode, division_master_des as divname from division_master", SqlConn)
                myCommand.CommandType = CommandType.Text
                myReader = myCommand.ExecuteReader
                If myReader.HasRows Then
                    ddlDivision.DataTextField = "divname"
                    ddlDivision.DataValueField = "divcode"
                    ddlDivision.DataSource = myReader
                    ddlDivision.DataBind()
                End If
                Dim li As ListItem = New ListItem("All", "All")
                ddlDivision.Items.Insert(0, li)
                myReader.Close()
                clsDBConnect.dbCommandClose(myCommand)
                clsDBConnect.dbConnectionClose(SqlConn)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptInhouse.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            If txtCtryGroup.Text.Trim = "" Then txtCtryGroupCode.Text = ""
            If txtCustGroup.Text.Trim = "" Then txtCustGroupCode.Text = ""
            If txtSourceCtry.Text.Trim = "" Then txtSourceCtryCode.Text = ""
            If txtCustCategory.Text.Trim = "" Then txtCustCategoryCode.Text = ""
            If txtCust.Text.Trim = "" Then txtCustCode.Text = ""
            If txtHotelCity.Text.Trim = "" Then txtHotelCityCode.Text = ""
            If txtHotelCategory.Text.Trim = "" Then txtHotelCategoryCode.Text = ""
            If txtHotel.Text.Trim = "" Then txtHotelCode.Text = ""
            Validation = True
        Catch ex As Exception
            Validation = False
            Throw ex
        End Try
    End Function
#End Region

#Region "Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click"
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            If txtRptType.Text.Trim = "Inhouse" Then
                InhouseReport()
            ElseIf txtRptType.Text = "E" Then
                ExpReservationReport()
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptInhouse.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Protected sub ExpReservationReport() "


    Protected Sub ExpReservationReport()
        Try
            If Validation() = False Then Exit Sub
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myCommand = New SqlCommand("sp_rep_expected_reservations", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 0
            myCommand.Parameters.Add(New SqlParameter("@datetype", SqlDbType.VarChar, 20)).Value = ddldatetype.SelectedValue
            If IsDate(txtFromDt.Text) Then
                myCommand.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDt.Text).ToString("yyyy/MM/dd")
            Else
                myCommand.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = DBNull.Value
            End If
            If IsDate(txtToDt.Text) Then
                myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDt.Text).ToString("yyyy/MM/dd")
            Else
                myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = DBNull.Value
            End If

            myCommand.Parameters.Add(New SqlParameter("@guesttype", SqlDbType.Int)).Value = Convert.ToInt32(ddlGuestName.SelectedValue)
            myCommand.Parameters.Add(New SqlParameter("@bookingstatus", SqlDbType.Int)).Value = Convert.ToInt32(ddlBookingStatus.SelectedValue)
            myCommand.Parameters.Add(New SqlParameter("@groupby", SqlDbType.Int)).Value = Convert.ToInt32(ddlGroupBy.SelectedValue)
            myCommand.Parameters.Add(New SqlParameter("@countrygroup", SqlDbType.VarChar, 20)).Value = txtCtryGroupCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@customergroup", SqlDbType.VarChar, 20)).Value = txtCustGroupCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@sourcectrycode", SqlDbType.VarChar, 20)).Value = txtSourceCtryCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@agentcatcode", SqlDbType.VarChar, 20)).Value = txtCustCategoryCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = txtCustCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@hotelcitycode", SqlDbType.VarChar, 20)).Value = txtHotelCityCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@hotelcatcode", SqlDbType.VarChar, 20)).Value = txtHotelCategoryCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@hotelcode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = ddlDivision.SelectedValue.Trim
   
            myDataAdapter = New SqlDataAdapter(myCommand)
            Dim myds As New DataSet
            myDataAdapter.Fill(myds)


            Dim bookingdt As DataTable = myds.Tables(0)
 

            Dim FileNameNew As String = "ExpectedReservations_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
            Dim wb As New XLWorkbook
            Dim ws As IXLWorksheet = wb.Worksheets.Add("Expected Reservation")
            Dim trow As Integer

            Dim header As IXLRange
            header = ws.Range("A2:Q2")
            header.Style.Font.SetBold()
            header.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#e2efda"))
            header.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            header.Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            header.Merge()
            header.Style.Font.FontSize = 16
            Dim headertext As Object
            headertext = ws.Ranges("e2:h2")
            headertext.Style.Font.SetBold()

            headertext.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            headertext.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
            headertext.Style.Font.SetBold()
            headertext.Style.Font.FontSize = 16
            'header.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
            headertext.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            headertext.Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            Dim range1 As IXLRange
            range1 = ws.Range("f2:I2")
            range1.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
            range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            range1.Merge()

            range1.Value = CType(Session("CompanyName"), String)
            range1.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            range1.Style.Border.SetTopBorder(XLBorderStyleValues.Thin)

            Dim header2 As IXLRange
            header2 = ws.Range("A3:R3")
            header2.Style.Font.SetBold()
            header2.Style.Fill.SetBackgroundColor(XLColor.FromArgb(192, 192, 192))
            header2.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            header2.Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            header2.Merge()
            header2.Style.Font.FontSize = 16
            Dim headertext2 As Object
            headertext2 = ws.Ranges("e3:h3")
            headertext2.Style.Font.SetBold()

            headertext2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            headertext2.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
            headertext2.Style.Font.SetBold()
            headertext2.Style.Font.FontSize = 16
            'header.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
            headertext2.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            headertext2.Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            Dim range2 As IXLRange
            range2 = ws.Range("f3:I3")
            range2.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
            range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            range2.Merge()
            range2.Value = "Expected Reservations"
            range2.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            range2.Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            'range1.Style.Font.SetBold()
            'range1.Style.Fill.SetBackgroundColor(XLColor.FromArgb(192, 192, 192))
            'range1.Style.Font.FontSize = 14
            Dim repfilter As New StringBuilder
            repfilter.Append(" From Date: " & txtFromDt.Text & " - To Date: " & txtToDt.Text & ",Report Type- " & ddldatetype.SelectedItem.Text & " ")
            ws.Cell(4, 1).Value = repfilter
            If txtCustGroup.Text <> "" Then
                ws.Cell(4, 1).Value = repfilter.Append(" , Customer Group: " & txtCustGroup.Text & "")
            End If
            If txtCustCategory.Text <> "" Then
                ws.Cell(4, 1).Value = repfilter.Append(" , Customer Category: " & txtCustCategory.Text & "")
            End If
            If txtSourceCtry.Text <> "" Then

                ws.Cell(4, 1).Value = repfilter.Append(" ,Source Country: " & txtSourceCtry.Text & "")
            End If
            If txtHotelCity.Text <> "" Then

                ws.Cell(4, 1).Value = repfilter.Append(", Hotel City: " & txtHotelCity.Text & "")
            End If
            If txtHotelCategory.Text <> "" Then

                ws.Cell(4, 1).Value = repfilter.Append(" , Hotel Category: " & txtHotelCategory.Text & "")
            End If
            If txtHotel.Text <> "" Then

                ws.Cell(4, 1).Value = repfilter.Append(" , Hotel :  " & txtHotel.Text & "")

            End If

            ws.Range(4, 1, 4, 18).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)

            trow = 5

            ws.Column("A").Width = 5
            ws.Column("B").Width = 11
            ws.Column("C").Width = 10
            ws.Column("D").Width = 9.5
            ws.Column("E").Width = 25
            ws.Column("F").Width = 35
            ws.Column("G").Width = 20
            ws.Column("H").Width = 10
            ws.Column("I").Width = 11
            ws.Column("J").Width = 10
            ws.Column("K").Width = 6
            ws.Column("L").Width = 6
            ws.Column("M").Width = 6
            ws.Column("n").Width = 9
            ws.Column("O").Width = 8
            ws.Column("P").Width = 9
            ws.Column("Q").Width = 12.5
            ws.Column("R").Width = 25  ' created by 

            Dim title = ws.Range(trow, 1, trow, 18)
            Dim bookingtitle = ws.Range(trow, 1, trow, 23).Style.Font.SetBold()
            ws.Range(trow, 1, trow, 18).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            ws.Range(trow, 1, trow, 18).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            bookingtitle.Alignment.WrapText = True
            ws.Cell(trow, 1).Value = "S.No."
            ws.Cell(trow, 2).Value = "Booking No."
            ws.Cell(trow, 3).Value = "Booking Date"
            ws.Cell(trow, 4).Value = " Status"
            ws.Cell(trow, 5).Value = "Guest Name"
            ws.Cell(trow, 6).Value = "Services"
            ws.Cell(trow, 7).Value = "Agent"
            ws.Cell(trow, 8).Value = "Checkin"
            ws.Cell(trow, 9).Value = "CheckOut"
            ws.Cell(trow, 10).Value = "Request Type"
            ws.Cell(trow, 11).Value = "Adult"
            ws.Cell(trow, 12).Value = "Child"
            ws.Cell(trow, 13).Value = "Total Pax"
            ws.Cell(trow, 14).Value = "Currency"
            ws.Cell(trow, 15).Value = "Sale in Currency"
            ws.Cell(trow, 16).Value = "Sale Value"
            ws.Cell(trow, 17).Value = "Sale Excluded VAT"
            ws.Cell(trow, 18).Value = "Created By"
            ws.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
            ws.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
            ws.Range(trow, 1, trow, 18).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            ws.Range(trow, 1, trow, 18).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(trow, 1, trow, 18).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            'Dim Hotelnameid = (From n In hotelDt.AsEnumerable() Group n By HotelNameRowid = New With {Key .Hotelname = n.Item(2), Key .Rowid = n.Item(0)} Into g1 = Group Select New With {.Hotelname = HotelNameRowid}).ToList()

            Dim filterview As DataView

            trow = trow + 1
            If bookingdt.Rows.Count >= 0 Then
                Dim filterpartynames = (From n In bookingdt.AsEnumerable() Group n By groupname = New With {Key .groupname = n.Item(1)} Into g1 = Group Select groupname)

                For i = 0 To filterpartynames.Count - 1

                    Dim filterpartyname As String = Convert.ToString(filterpartynames(i).groupname)
                    filterview = New DataView(bookingdt)
                    filterview.RowFilter = "groupname= '" & filterpartyname & "' "
                    Dim filterdt As DataTable = filterview.ToTable


                    ws.Range(trow, 1, trow, 17).Merge()
                    ws.Cell(trow, 1).Value = "Supplier:" & filterpartyname & ""
                    ws.Range(trow, 1, trow, 17).Style.Fill.BackgroundColor = XLColor.FromArgb(192, 192, 200)
                    ws.Range(trow, 1, trow, 17).Style.Font.SetBold(True)
                    ws.Range(trow, 1, trow, 17).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
                    ws.Range(trow, 1, trow, 17).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                    ws.Range(trow, 1, trow, 17).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)

                    trow = trow + 1

                    For Each bookingdetail In filterdt.Rows

                        title = ws.Range(++trow, 1, trow, 17)
                        title.Style.Alignment.WrapText = True
                        ws.Cell(trow, 1).Value = bookingdetail("sno")
                        ws.Cell(trow, 2).Value = bookingdetail("requestid")
                        ws.Cell(trow, 3).Value = Format(bookingdetail("requestdate"), "dd/MM/yyyy")
                        ws.Cell(trow, 4).Value = bookingdetail("bookingstatus")
                        ws.Cell(trow, 5).Value = bookingdetail("guestnames")
                        ws.Cell(trow, 6).Value = bookingdetail("servicedetails")
                        ws.Cell(trow, 7).Value = bookingdetail("agentname")
                        ws.Cell(trow, 8).Value = bookingdetail("checkin")
                        If ((Format(bookingdetail("checkout"), "dd/MM/yyyy")) = "01/01/1900") Then

                            ws.Cell(trow, 9).Value = "    -   "
                        Else
                            ws.Cell(trow, 9).Value = bookingdetail("checkout")

                        End If

                        ws.Cell(trow, 10).Value = bookingdetail("requesttype")
                        ws.Cell(trow, 11).Value = bookingdetail("adults")
                        ws.Cell(trow, 12).Value = bookingdetail("child")
                        ws.Cell(trow, 13).Value = bookingdetail("totalpax")
                        ws.Cell(trow, 14).Value = bookingdetail("currcode")
                        ws.Cell(trow, 15).Value = bookingdetail("salecurrency")
                        ws.Cell(trow, 16).Value = bookingdetail("salevalue")
                        ws.Cell(trow, 17).Value = bookingdetail("salevalueexclvat")
                        ws.Cell(trow, 18).Value = bookingdetail("createdby")
                        ws.Range(trow, 1, trow, 18).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
                        ws.Range(trow, 1, trow, 18).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                        ws.Range(trow, 1, trow, 18).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
                        trow = trow + 1

                    Next
                    'trow = trow + 1
                    ws.Range(trow, 1, trow, 18).Style.Font.SetBold(True)
                    ws.Range(trow, 1, trow, 13).Merge()
                    ws.Cell(trow, 1).Value = "Supplier:  " & filterpartyname & ""
                    ws.Range(trow, 14, trow, 15).Merge()
                    ws.Cell(trow, 14).Value = " Sub total: "
                    ws.Cell(trow, 16).Value = filterdt.Compute("Sum(salevalue)", filterview.RowFilter)
                    ws.Cell(trow, 17).Value = filterdt.Compute("Sum(salevalueexclvat)", filterview.RowFilter)
                    trow = trow + 1

                Next



                Using MyMemoryStream As New MemoryStream()
                    wb.SaveAs(MyMemoryStream)
                    wb.Dispose()
                    Response.Clear()
                    Response.Buffer = True
                    Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
                    Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"

                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
                    Response.Flush()
                    HttpContext.Current.ApplicationInstance.CompleteRequest()

                End Using
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Records not found, Please redefine search criteria');", True)
            End If
        Catch ex As Exception
            If Not SqlConn Is Nothing Then
                clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                clsDBConnect.dbConnectionClose(SqlConn)
            End If
            Throw ex
        End Try
    End Sub
#End Region
#Region "Protected Sub InhouseReport()"
    Protected Sub InhouseReport()
        Try
            If Validation() = False Then Exit Sub
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myCommand = New SqlCommand("sp_rep_inhouse", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 0
            If IsDate(txtFromDt.Text) Then
                myCommand.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtFromDt.Text).ToString("yyyy/MM/dd")
            Else
                myCommand.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 10)).Value = DBNull.Value
            End If
            If IsDate(txtToDt.Text) Then
                myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = Convert.ToDateTime(txtToDt.Text).ToString("yyyy/MM/dd")
            Else
                myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 10)).Value = DBNull.Value
            End If
            myCommand.Parameters.Add(New SqlParameter("@guesttype", SqlDbType.Int)).Value = Convert.ToInt32(ddlGuestName.SelectedValue)
            myCommand.Parameters.Add(New SqlParameter("@bookingstatus", SqlDbType.Int)).Value = Convert.ToInt32(ddlBookingStatus.SelectedValue)
            myCommand.Parameters.Add(New SqlParameter("@groupby", SqlDbType.Int)).Value = Convert.ToInt32(ddlGroupBy.SelectedValue)
            myCommand.Parameters.Add(New SqlParameter("@countrygroup", SqlDbType.VarChar, 20)).Value = txtCtryGroupCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@customergroup", SqlDbType.VarChar, 20)).Value = txtCustGroupCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@sourcectrycode", SqlDbType.VarChar, 20)).Value = txtSourceCtryCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@agentcatcode", SqlDbType.VarChar, 20)).Value = txtCustCategoryCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = txtCustCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@hotelcitycode", SqlDbType.VarChar, 20)).Value = txtHotelCityCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@hotelcatcode", SqlDbType.VarChar, 20)).Value = txtHotelCategoryCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@hotelcode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 20)).Value = ddlDivision.SelectedValue.Trim
            myDataAdapter = New SqlDataAdapter(myCommand)
            Dim myDs As New DataSet()
            myDataAdapter.Fill(myDs)
            Dim resultDt As DataTable = myDs.Tables(0)
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)

            If resultDt.Rows.Count > 0 Then
                Dim excelLastRow As Integer
                Dim runningRow As Integer
                Dim wbook As XLWorkbook = New XLWorkbook
                Dim FolderPath As String = "..\ExcelTemplates\"
                Dim companyname As String = CType(Session("CompanyName"), String)
                Dim FileName As String
                If companyname <> "Elevate Tourism L.L.C." Then
                    FileName = "Inhouse00.xlsx"
                Else
                    FileName = "Inhouse01.xlsx"

                End If


                Dim FilePath As String = Server.MapPath(FolderPath + FileName)
                wbook = New XLWorkbook(FilePath)
                Dim wsInhouse As IXLWorksheet = wbook.Worksheet("Inhouse Report")
                'Dim wsInhouse As IXLWorksheet = wbook.Worksheets.Add("Inhouse Report")
                wsInhouse.Style.Font.SetFontName("Trebuchet MS")
             
                excelLastRow = 2
                wsInhouse.Cell(excelLastRow, 4).Value = companyname
                wsInhouse.Cell(excelLastRow, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                wsInhouse.Cell(excelLastRow, 4).Style.Font.SetBold(True).Font.SetFontSize(14)

                excelLastRow = 4
                wsInhouse.Cell(excelLastRow, 4).Value = "Inhouse Report"
                wsInhouse.Cell(excelLastRow, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                wsInhouse.Cell(excelLastRow, 4).Style.Font.SetBold(True).Font.SetFontSize(12)

                excelLastRow = 6
                wsInhouse.Cell(excelLastRow, 4).Value = "From Date :" + Convert.ToDateTime(txtFromDt.Text).ToString("dd/MM/yyyy") + " - To Date :" + Convert.ToDateTime(txtToDt.Text).ToString("dd/MM/yyyy") + " - Guest Type : " + ddlGuestName.SelectedItem.Text + "; Status : " + ddlBookingStatus.SelectedItem.Text
                wsInhouse.Cell(excelLastRow, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                wsInhouse.Cell(excelLastRow, 4).Style.Font.SetBold(True).Font.SetFontSize(10)

                excelLastRow = 9
                runningRow = excelLastRow

                Dim TblTitle() As String = {"S.No.", "Request Id", "Guest Name", "Services", "Room No", "Arr.Date", "Dep.Date", "Hotel Conf No", "Status", "Company", "Services Booked", "Adult", "Child", "Total Pax"}
                For i = 0 To TblTitle.GetUpperBound(0)
                    wsInhouse.Cell(runningRow, i + 1).Value = TblTitle(i)
                Next
                Dim rngTitle As IXLRange
                rngTitle = wsInhouse.Range("A" + Convert.ToString(excelLastRow) + ":N" + Convert.ToString(runningRow))
                rngTitle.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Font.SetFontName("Trebuchet MS").Font.SetFontSize(10)
                rngTitle.Style.Font.SetBold(True)
                rngTitle.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin)
                excelLastRow = excelLastRow + 1
                runningRow = runningRow + 1
                Dim groupDt As New DataTable
                Dim groupDr = (From n In resultDt.AsEnumerable() Group By groupby = n.Field(Of String)("groupby") Into groupHotel = Group Select groupby)
                If groupDr.Count > 0 Then
                    wsInhouse.Column(3).Width = 20  'guestnames
                    wsInhouse.Column(4).Width = 40   'roomdetails
                    wsInhouse.Column(8).Width = 15   'hotelconfno
                    wsInhouse.Column(10).Width = 20   'agentname
                    wsInhouse.Column(11).Width = 50   'bookedservices
                    For i = 0 To groupDr.Count - 1
                        Dim filterId As String = Convert.ToString(groupDr(i))
                        Dim filterName As String = (From n In resultDt.AsEnumerable() Where n.Field(Of String)("groupby") = filterId Select n.Field(Of String)("groupname")).Distinct.FirstOrDefault()
                        runningRow = runningRow + 1
                        Dim rngSubTitle As IXLRange
                        rngSubTitle = wsInhouse.Range("A" + Convert.ToString(runningRow) + ":N" + Convert.ToString(runningRow))
                        rngSubTitle.Merge().Style.Font.SetBold(True)
                        rngSubTitle.Style.Fill.SetBackgroundColor(XLColor.LightGray)
                        wsInhouse.Cell(runningRow, 1).Value = filterName
                        Dim filterDt As DataTable = (From n In resultDt.AsEnumerable() Where n.Field(Of String)("groupby") = filterId Select n).CopyToDataTable()
                        For Each filterDr As DataRow In filterDt.Rows
                            runningRow = runningRow + 1
                            wsInhouse.Cell(runningRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                            wsInhouse.Cell(runningRow, 1).Value = filterDr("sno")
                            wsInhouse.Cell(runningRow, 2).Value = filterDr("requestId")

                            wsInhouse.Cell(runningRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(True)
                            wsInhouse.Cell(runningRow, 3).Value = filterDr("guestnames")

                            wsInhouse.Cell(runningRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(True)
                            wsInhouse.Cell(runningRow, 4).Value = filterDr("roomdetails") + vbCrLf + filterDr("rateplandetail")

                            wsInhouse.Cell(runningRow, 5).Value = filterDr("roomno")

                            wsInhouse.Cell(runningRow, 6).Style.NumberFormat.Format = "dd/MM/yyyy"
                            wsInhouse.Cell(runningRow, 6).DataType = XLCellValues.Text
                            wsInhouse.Cell(runningRow, 6).Value = filterDr("arrdate")

                            wsInhouse.Cell(runningRow, 7).Style.NumberFormat.Format = "dd/MM/yyyy"
                            wsInhouse.Cell(runningRow, 7).DataType = XLCellValues.Text
                            wsInhouse.Cell(runningRow, 7).Value = filterDr("depdate")

                            wsInhouse.Cell(runningRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(True)
                            wsInhouse.Cell(runningRow, 8).Value = filterDr("hotelconfno")

                            wsInhouse.Cell(runningRow, 9).Value = filterDr("bookingstatus")

                            wsInhouse.Cell(runningRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(True)
                            wsInhouse.Cell(runningRow, 10).Value = filterDr("agentname")

                            wsInhouse.Cell(runningRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(True)
                            wsInhouse.Cell(runningRow, 11).Value = filterDr("bookedservices")

                            wsInhouse.Cell(runningRow, 12).Value = filterDr("adults")
                            wsInhouse.Cell(runningRow, 13).Value = filterDr("child")
                            wsInhouse.Cell(runningRow, 14).Value = filterDr("totalpax")
                        Next
                    Next

                    wsInhouse.Columns("1:2,5:7,9,12:14").AdjustToContents()
                    wsInhouse.Columns("2,5:7,9,12:14").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)

                    Dim tblRange As IXLRange
                    tblRange = wsInhouse.Range("A" + Convert.ToString(excelLastRow) + ":N" + Convert.ToString(runningRow))
                    tblRange.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Trebuchet MS").Font.SetFontSize(10)
                    tblRange.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin)
                End If
                Dim FileNameNew As String = "InhousePrint_" & Now.Year & Now.Month.ToString("00") & Now.Day.ToString("00") & Now.Hour & Now.Minute & Now.Second & ".xlsx"
                Using MyMemoryStream As New MemoryStream()
                    wbook.SaveAs(MyMemoryStream)
                    wbook.Dispose()
                    Response.Clear()
                    Response.Buffer = True
                    Response.AddHeader("content-disposition", "attachment;filename=" + FileNameNew)
                    Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
                    Response.Flush()
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                End Using
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Records not found, Please redefine search criteria');", True)
            End If
        Catch ex As Exception
            If Not SqlConn Is Nothing Then
                clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                clsDBConnect.dbConnectionClose(SqlConn)
            End If
            Throw ex
        End Try
    End Sub
#End Region

End Class
