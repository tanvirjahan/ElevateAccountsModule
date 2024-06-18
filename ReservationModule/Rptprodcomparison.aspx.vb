
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports ClosedXML.Excel
Imports System.IO
Imports System.Net
Imports System.Linq

Partial Class Rptprodcomparison
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUser As New clsUser
    Dim objUtils As New clsUtils
    Dim mySqlConn As SqlConnection
    Dim mySqlCmd As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim strSqlQry As String
    Dim document As New XLWorkbook
    Dim myds As New DataSet
#End Region

#Region "Web Methods"
    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetHotelChain(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""

        Dim myDS As New DataSet
        Dim agentnames As New List(Of String)
        Try
            strSqlQry = "select hotelchainname,hotelchaincode from hotelchainmaster  where  active=1 and  hotelchainname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    agentnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("hotelchainname").ToString(), myDS.Tables(0).Rows(i)("hotelchaincode").ToString()))
                Next
            End If
            Return agentnames
        Catch ex As Exception
            Return agentnames
        End Try
    End Function
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
    Public Shared Function GetHotelGroups(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim hotelGroups As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If

            strSqlQry = "select hotelgroupcode,hotelgroupname from hotelgroup where    active=1 and hotelgroupname like '%" & prefixText & "%' order by hotelgroupname asc"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    hotelGroups.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("hotelgroupname").ToString(), myDS.Tables(0).Rows(i)("hotelgroupcode").ToString()))
                Next
            End If
            Return hotelGroups
        Catch ex As Exception
            Return hotelGroups
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
            strSqlQry = "select partycode,partyname from partymast where active=1 and partyname like '%" & prefixText & "%' order by partyname asc"
            'sptypecode='HOT' and 'changed by mohamed on 28/05/2019
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

    Private Sub ShowSelectedColumns(ByVal row As DataRow, ByVal ws As IXLWorksheet, ByVal rowcount As Integer, ByVal decno As Integer, ByVal decPlaces As String)
        If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Then
            ws.Cell(rowcount, 2).Value = row("partyname")
        ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
            ws.Cell(rowcount, 2).Value = row("agentname")
        ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 3 Then
            ws.Cell(rowcount, 2).Value = row("catname")
        ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 4 Then
            ws.Cell(rowcount, 2).Value = row("sourcectryname")
        ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 5 Then
            ws.Cell(rowcount, 2).Value = row("salesname")
        Else
            ws.Cell(rowcount, 2).Value = row("partyname")
        End If
        ws.Cell(rowcount, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)

        If ddlColumnns.SelectedValue = "All" Then

            '   noofcol = 14
            ws.Cell(rowcount, 3).Value = IIf(row("pax_old") <> "0", row("pax_old"), " ")
            ws.Cell(rowcount, 4).Value = IIf(row("units_old") <> "0", row("units_old"), "")
            ws.Cell(rowcount, 5).Value = IIf(row("roomnights_old") <> "0", row("roomnights_old"), " ")
            ws.Cell(rowcount, 6).Value = IIf(row("salevalue_old") <> "0", Math.Round(row("salevalue_old"), decno), " ")
            ws.Cell(rowcount, 7).Value = IIf(row("pax") <> "0", row("pax"), " ")
            ws.Cell(rowcount, 8).Value = IIf(row("units") <> "0", row("units"), " ")
            ws.Cell(rowcount, 9).Value = IIf(row("roomnights") <> "0", row("roomnights"), "")
            ws.Cell(rowcount, 10).Value = IIf(row("salevalue") <> "0", Math.Round(row("salevalue"), decno), "")
            ws.Cell(rowcount, 11).Value = FormatNumber(Math.Round(row("pax_variance"), decno), UseParensForNegativeNumbers:=TriState.True)
            ws.Cell(rowcount, 12).Value = FormatNumber(Math.Round(row("units_variance"), decno), UseParensForNegativeNumbers:=TriState.True)
            ws.Cell(rowcount, 13).Value = FormatNumber(Math.Round(row("roomnights_variance"), decno), UseParensForNegativeNumbers:=TriState.True)
            ws.Cell(rowcount, 14).Value = FormatNumber(Math.Round(row("salevalue_variance"), decno), UseParensForNegativeNumbers:=TriState.True)
            ws.Range("K" + rowcount.ToString + ":N" + rowcount.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
            ' .Font.SetBold(True).NumberFormat.Format = decPlaces
            ' '   ws.Range("K" + rowcount.ToString + ":N" + rowcount.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Font.SetBold(True).NumberFormat.Format = "0.00;[Red](0.00)"
        ElseIf ddlColumnns.SelectedValue = "RoomNights" Then
            ' noofcol = 5
            ws.Cell(rowcount, 3).Value = IIf(row("roomnights_old") <> "0", row("roomnights_old"), "")
            ws.Cell(rowcount, 4).Value = IIf(row("roomnights") <> "0", row("roomnights"), " ")
            ws.Cell(rowcount, 5).Value = FormatNumber(Math.Round(row("roomnights_variance"), decno), UseParensForNegativeNumbers:=TriState.True)
            ws.Range("E" + rowcount.ToString + ":E" + rowcount.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
        ElseIf ddlColumnns.SelectedValue = "Pax" Then
            ' noofcol = 5,
            ws.Cell(rowcount, 3).Value = IIf(row("Pax_old") <> "0", row("Pax_old"), "")
            ws.Cell(rowcount, 4).Value = IIf(row("Pax") <> "0", row("Pax"), "")
            ws.Cell(rowcount, 5).Value = FormatNumber(Math.Round(row("Pax_variance"), decno), UseParensForNegativeNumbers:=TriState.True)
            ws.Range("E" + rowcount.ToString + ":E" + rowcount.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
        ElseIf ddlColumnns.SelectedValue = "Units" Then
            ' noofcol = 5
            ws.Cell(rowcount, 3).Value = IIf(row("units_old") <> "0", row("units_old"), " ")
            ws.Cell(rowcount, 4).Value = IIf(row("units") <> "0", row("units"), "")
            ws.Cell(rowcount, 5).Value = FormatNumber(Math.Round(row("units_variance"), decno), UseParensForNegativeNumbers:=TriState.True)
            ws.Range("E" + rowcount.ToString + ":E" + rowcount.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
        ElseIf ddlColumnns.SelectedValue = "SaleValue" Then
            ' noofcol = 5
            ws.Cell(rowcount, 3).Value = IIf(row("salevalue_old") <> "0", Math.Round(row("salevalue_old"), decno), "")
            ws.Cell(rowcount, 4).Value = IIf(row("salevalue") <> "0", Math.Round(row("salevalue"), decno), " ")
            ws.Cell(rowcount, 5).Value = FormatNumber(Math.Round(row("salevalue_variance"), decno), UseParensForNegativeNumbers:=TriState.True)
            ws.Range("E" + rowcount.ToString + ":E" + rowcount.ToString).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
        End If







    End Sub


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
                txtToDt.Text = Now.Date.AddMonths(3)
                txtCmpFrmDt.Text = Now.Date.AddYears(-1)
                txtCmpToDt.Text = CType(txtCmpFrmDt.Text, Date).AddMonths(3)
                If AppId.Value Is Nothing = False Then
                    strappid = AppId.Value
                End If
                strappname = objUser.GetAppName(Session("dbconnectionName"), strappid)

                'objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                '                                       CType(strappname, String), "ReservationModule\RptInhouse.aspx?type=" + txtRptType.Text.Trim, btnAddNew, btnExportToExcel, _
                '                                       btnLoadReport, gvSearch:=gvSearchResult)
                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                       CType(strappname, String), "ReservationModule\Rptprodcomparison.aspx?appid=" + strappid.Trim, btnAddNew, btnExportToExcel, _
                                       btnExportToExcel, gvSearch:=gvSearchResult)
                ddlGuestName.Items.Add(New ListItem("Lead Guest Name", "0"))
                ddlGuestName.Items.Add(New ListItem("All Guest Names", "1"))
                ddlGuestName.SelectedIndex = 0
                ddlBookingStatus.Items.Add(New ListItem("All", "1"))
                ddlBookingStatus.Items.Add(New ListItem("Confirmed", "0"))

                ddlGroupBy.Items.Add(New ListItem("Agent-Provider", "0", False))
                ddlGroupBy.Items.Add(New ListItem("Hotel", "1"))
                ddlGroupBy.Items.Add(New ListItem("Agent", "2"))
                ddlGroupBy.Items.Add(New ListItem("Provider Category", "3", False))
                ddlGroupBy.Items.Add(New ListItem("Country", "4", False))
                ddlGroupBy.Items.Add(New ListItem("Sales Person", "5", False))
                ddlGroupBy.Items.Add(New ListItem("Country Group", "6"))
                ddlGroupBy.SelectedIndex = 0
                objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlbookingstatusdb, "bsname", "bscode", "select * from booking_confirmation_statusdropdown", False)

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

#Region "Protected Sub btnLoadReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadReport.Click"
    Protected Sub btnLoadReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadReport.Click
        Try
            'ExcelProdComparison()
            newExcelProdComparison()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Rptprodcomparision.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Protected sub ExcelProdComparison() "
    Public Sub ExcelProdComparison()

        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
        mySqlCmd = New SqlCommand("sp_rep_production_comparison", mySqlConn)
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.Parameters.Add(New SqlParameter("@datetype", SqlDbType.VarChar, 20)).Value = "Arrival" 'Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtFromDt.Text).ToString("yyyy/MM/dd") ' CType(IIf(TxtAgentCode.Text.Trim <> "", TxtAgentCode.Text.Trim, ""), String)
        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtToDt.Text).ToString("yyyy/MM/dd") 'CType(IIf(TxtSectorCode.Text.Trim <> "", TxtSectorCode.Text.Trim, ""), String)
        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate1", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtCmpFrmDt.Text).ToString("yyyy/MM/dd") ' CType(IIf(TxtCtryCode.Text.Trim <> "", TxtCtryCode.Text.Trim, ""), String)
        mySqlCmd.Parameters.Add(New SqlParameter("@todate1", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtCmpToDt.Text).ToString("yyyy/MM/dd") ' CType(IIf(TxtCityCode.Text.Trim <> "", TxtCityCode.Text.Trim, ""), String)
        mySqlCmd.Parameters.Add(New SqlParameter("@guesttype", SqlDbType.Int)).Value = Convert.ToInt32(ddlGuestName.SelectedValue)
        mySqlCmd.Parameters.Add(New SqlParameter("@bookingstatus", SqlDbType.Int)).Value = Convert.ToInt32(ddlBookingStatus.SelectedValue)
        mySqlCmd.Parameters.Add(New SqlParameter("@groupby", SqlDbType.Int)).Value = Convert.ToInt32(ddlGroupBy.SelectedValue)
        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroup", SqlDbType.VarChar, 20)).Value = txtCtryGroupCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@customergroup", SqlDbType.VarChar, 20)).Value = txtCustGroupCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@sourcectrycode", SqlDbType.VarChar, 20)).Value = txtSourceCtryCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@agentcatcode", SqlDbType.VarChar, 20)).Value = txtCustCategoryCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = txtCustCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@hotelcitycode", SqlDbType.VarChar, 20)).Value = txtHotelCityCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@hotelcatcode", SqlDbType.VarChar, 20)).Value = txtHotelCategoryCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@hotelcode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@requesttype", SqlDbType.VarChar, 20)).Value = ddlRequestType.SelectedValue
        mySqlCmd.Parameters.Add(New SqlParameter("@orderby", SqlDbType.Int)).Value = Convert.ToInt32(ddlOrder.SelectedValue)
        mySqlCmd.Parameters.Add(New SqlParameter("@invoicetype", SqlDbType.Int)).Value = Convert.ToInt32(ddlInvStatus.SelectedValue)
        If ChkExclComp.Checked = True Then
            mySqlCmd.Parameters.Add(New SqlParameter("@excludecompliment", SqlDbType.Int)).Value = 1
        Else
            mySqlCmd.Parameters.Add(New SqlParameter("@excludecompliment", SqlDbType.Int)).Value = 0
        End If
        myDataAdapter = New SqlDataAdapter(mySqlCmd)
        myDataAdapter.Fill(myds)
        Dim productiondt As DataTable = myds.Tables(0)

        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add("ProductionComparison")
        Dim rowcount As Integer = 1
        ws.Columns.AdjustToContents()
        ws.Column("B").Width = 30
        ws.Columns("A").Hide()
        Dim decno As Integer = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), Integer)
        Dim decPlaces As String
        If decno = 3 Then
            decPlaces = "#,##0.000;[Red](#,##0.000)"
        Else
            decPlaces = "#,##0.00;[Red](#,##0.00)"
        End If
        'Dim companyname = ws.Range("B" & rowcount & ":N" & rowcount).Merge()
        'companyname.Style.Font.SetBold().Font.FontSize = 14
        'companyname.Style.Font.FontColor = XLColor.Black
        'companyname.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        'companyname.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
        'companyname.Style.Fill.SetBackgroundColor(XLColor.LightGray)
        ws.Range("B" & rowcount & ":N" & rowcount).Merge()
        ws.Cell(1, 2).Value = CType(Session("CompanyName"), String)


        If ddlColumnns.SelectedValue = "All" Then
            ws.Range("B" & rowcount & ":N" & rowcount).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            ws.Range("B" & rowcount & ":N" & rowcount).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            ws.Range("B" & rowcount & ":N" & rowcount).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            ws.Range("B" & rowcount & ":N" & rowcount).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range("B" & rowcount & ":N" & rowcount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            ws.Range("B" & rowcount & ":N" & rowcount).Style.Font.SetBold()
            ws.Range("B" & rowcount & ":N" & rowcount).Style.Font.FontSize = 16
            ws.Range("B" & rowcount & ":N" & rowcount).Style.Fill.SetBackgroundColor(XLColor.LightGray)
        Else
            ws.Range("B" & rowcount & ":E" & rowcount).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            ws.Range("B" & rowcount & ":E" & rowcount).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            ws.Range("B" & rowcount & ":E" & rowcount).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            ws.Range("B" & rowcount & ":E" & rowcount).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range("B" & rowcount & ":E" & rowcount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            ws.Range("B" & rowcount & ":E" & rowcount).Style.Font.SetBold()
            ws.Range("B" & rowcount & ":E" & rowcount).Style.Font.FontSize = 16
            ws.Range("B" & rowcount & ":E" & rowcount).Style.Fill.SetBackgroundColor(XLColor.LightGray)
        End If

        rowcount = rowcount + 1


        ws.Range("B" & rowcount & ":N" & rowcount).Merge()

        ws.Cell(2, 2).Value = "Comparative Report"

        'repheading.Style.Font.SetBold().Font.FontSize = 14
        'repheading.Style.Font.FontColor = XLColor.Black
        'repheading.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        'repheading.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center


        If ddlColumnns.SelectedValue = "All" Then
            ws.Range("B" & rowcount & ":N" & rowcount).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            ws.Range("B" & rowcount & ":N" & rowcount).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            ws.Range("B" & rowcount & ":N" & rowcount).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            ws.Range("B" & rowcount & ":N" & rowcount).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range("B" & rowcount & ":N" & rowcount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            ws.Range("B" & rowcount & ":N" & rowcount).Style.Font.SetBold()
            ws.Range("B" & rowcount & ":N" & rowcount).Style.Font.FontSize = 14
            ' ws.Range("B" & rowcount & ":N" & rowcount).Style.Fill.SetBackgroundColor(XLColor.LightGray)
        Else
            ws.Range("B" & rowcount & ":E" & rowcount).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            ws.Range("B" & rowcount & ":E" & rowcount).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            ws.Range("B" & rowcount & ":E" & rowcount).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            ws.Range("B" & rowcount & ":E" & rowcount).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range("B" & rowcount & ":E" & rowcount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            ws.Range("B" & rowcount & ":E" & rowcount).Style.Font.SetBold()
            ws.Range("B" & rowcount & ":E" & rowcount).Style.Font.FontSize = 14
            '  ws.Range("B" & rowcount & ":E" & rowcount).Style.Fill.SetBackgroundColor(XLColor.LightGray)
        End If
        rowcount = rowcount + 1

        Dim repfilter As New StringBuilder
        repfilter.Append("Current Period :" & txtFromDt.Text & "-  " & txtToDt.Text & " Compare Period:" & txtCmpFrmDt.Text & "-  " & txtCmpToDt.Text & " ")


        ws.Cell("B" & rowcount).Value = repfilter
        If txtHotel.Text <> "" Then
            ws.Cell("B" & rowcount).Value = repfilter.Append(" Hotel: " & txtHotel.Text & "")
        End If
        If txtCtryGroup.Text <> "" Then
            ws.Cell("B" & rowcount).Value = repfilter.Append(" Country Group: " & txtCtryGroup.Text & "")
        End If
        If txtCustGroup.Text <> "" Then

            ws.Cell("B" & rowcount).Value = repfilter.Append(", Customer Group: " & txtCustGroup.Text & "")
        End If

        If txtSourceCtry.Text <> "" Then

            ws.Cell("B" & rowcount).Value = repfilter.Append(", Source Country: " & txtSourceCtry.Text & "")
        End If
        If txtCustCategory.Text <> "" Then
            ws.Cell("B" & rowcount).Value = repfilter.Append(",  Customer Category: " & txtCustCategory.Text & "")
        End If
        If txtCust.Text <> "" Then
            ws.Cell("B" & rowcount).Value = repfilter.Append(",  Customer: " & txtCust.Text & "")
        End If
        If txtHotelCity.Text <> "" Then
            ws.Cell("B" & rowcount).Value = repfilter.Append(",  Hotel City  :  " & txtHotelCity.Text & "")
        End If
        If txtHotelCategory.Text <> "" Then
            ws.Cell("B" & rowcount).Value = repfilter.Append(",  Hotel Category  :  " & txtHotelCategory.Text & "")
        End If
        Dim lsCurrencyCode As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457")
        ws.Cell("B" & rowcount).Value = repfilter.Append("   Currency: " & Convert.ToString(lsCurrencyCode) & "  ")

        Dim filter = ws.Range("B" & rowcount & ":N" & rowcount).Merge()

        filter.Style.Font.SetBold().Font.FontSize = 10
        filter.Style.Font.FontColor = XLColor.Black
        filter.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
        filter.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

        rowcount = rowcount + 1


        ws.Range("B" & rowcount & ":B" & rowcount + 1).Merge()
        ws.Range("B" & rowcount & ":B" & rowcount + 1).Style.Fill.SetBackgroundColor(XLColor.LightGray)
        If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Then
            ws.Cell("B" & rowcount).Value = "Service Provider"

        ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
            ws.Cell("B" & rowcount).Value = "Client"

        ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 3 Then
            ws.Cell("B" & rowcount).Value = "Provider Category"


        ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 4 Then


            ws.Cell("B" & rowcount).Value = "Country"

        ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 5 Then
            ws.Cell("B" & rowcount).Value = "SalesPerson"
        Else
            ws.Cell("B" & rowcount).Value = "Customer-Provider"
        End If

        Dim tabletitle

        If ddlColumnns.SelectedValue = "All" Then
            tabletitle = ws.Range(rowcount, 2, rowcount + 1, 14)

            ws.Cell("C" & rowcount).Value = txtCmpFrmDt.Text & "-" & txtCmpToDt.Text
            ws.Range("C" & rowcount & ":F" & rowcount).Merge()
            ws.Range("C" & rowcount & ":F" & rowcount).Style.Fill.SetBackgroundColor(XLColor.LightGray)
            ws.Cell("G" & rowcount).Value = txtFromDt.Text & "-" & txtToDt.Text
            ws.Range("G" & rowcount & ":J" & rowcount).Merge()
            ws.Range("G" & rowcount & ":J" & rowcount).Style.Fill.SetBackgroundColor(XLColor.LightGray)
            ws.Cell("K" & rowcount).Value = "Variance(%)"
            ws.Range("K" & rowcount & ":N" & rowcount).Merge()
            ws.Range("K" & rowcount & ":N" & rowcount).Style.Fill.SetBackgroundColor(XLColor.LightGray)
            rowcount = rowcount + 1
            ws.Cell("C" & rowcount).Value = "Total Pax"
            ws.Cell("D" & rowcount).Value = "No.of Rooms"
            ws.Cell("E" & rowcount).Value = "Total Room Nights"
            ws.Cell("F" & rowcount).Value = "Salevalue"
            ws.Cell("G" & rowcount).Value = "Total Pax"
            ws.Cell("H" & rowcount).Value = "No.of Rooms"
            ws.Cell("I" & rowcount).Value = "Total Room Nights"
            ws.Cell("J" & rowcount).Value = "Salevalue"
            ws.Cell("K" & rowcount).Value = "Pax (%)"
            ws.Cell("L" & rowcount).Value = " Room Nights(%)"
            ws.Cell("M" & rowcount).Value = "Total Room Nights(%)"
            ws.Cell("N" & rowcount).Value = "Sale Value(%)"
            ws.Column("C").Width = 10
            ws.Column("D").Width = 10
            ws.Column("E").Width = 10
            ws.Column("F").Width = 15
            ws.Column("G").Width = 10
            ws.Column("H").Width = 10
            ws.Column("I").Width = 10
            ws.Column("J").Width = 15
            ws.Column("K").Width = 10
            ws.Column("L").Width = 10
            ws.Column("M").Width = 15
            ws.Column("N").Width = 15
            ws.Range(4, 2, 4, 14).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#FFE5CC"))
            ws.Range(3, 2, 4, 14).Style.Font.FontSize = 11
        ElseIf ddlColumnns.SelectedValue = "RoomNights" Or ddlColumnns.SelectedValue = "Pax" Or ddlColumnns.SelectedValue = "Units" Or ddlColumnns.SelectedValue = "SaleValue" Then
            tabletitle = ws.Range(rowcount, 2, rowcount + 1, 5)
            ws.Cell("C" & rowcount).Value = txtCmpFrmDt.Text & "-" & txtCmpToDt.Text
            ws.Cell("D" & rowcount).Value = txtFromDt.Text & "-" & txtToDt.Text
            ws.Cell("E" & rowcount).Value = "Variance(%)"
            rowcount = rowcount + 1
            'ws.Range("C" & rowcount & ":F" & rowcount).Merge()
            If ddlColumnns.SelectedValue = "RoomNights" Then
                ws.Cell("C" & rowcount).Value = "Total Room Nights"
                ws.Cell("D" & rowcount).Value = "Total Room Nights"
                ws.Cell("E" & rowcount).Value = "Total Room Nights(%)"
            ElseIf ddlColumnns.SelectedValue = "Pax" Then
                ws.Cell("C" & rowcount).Value = "Total Pax"
                ws.Cell("D" & rowcount).Value = "Total Pax"
                ws.Cell("E" & rowcount).Value = "Total Pax(%)"
            ElseIf ddlColumnns.SelectedValue = "Units" Then
                ws.Cell("C" & rowcount).Value = "No.of Rooms"
                ws.Cell("D" & rowcount).Value = "No.of Rooms"
                ws.Cell("E" & rowcount).Value = "No.of Rooms(%)"
            ElseIf ddlColumnns.SelectedValue = "SaleValue" Then
                ws.Cell("C" & rowcount).Value = "Salevalue "
                ws.Cell("D" & rowcount).Value = "Salevalue "
                ws.Cell("E" & rowcount).Value = "Sale Value(%)"
            End If
            ws.Column("C").Width = 15
            ws.Column("D").Width = 15
            ws.Column("E").Width = 18

            ws.Range(4, 2, 4, 5).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#FFE5CC"))
            ws.Range(3, 2, 4, 5).Style.Font.FontSize = 11

        End If
        tabletitle.Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
        tabletitle.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        tabletitle.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
        Dim noofcol = 14

        If ddlColumnns.SelectedValue <> "All" Then
            ws.Range("B1:N1").Unmerge()
            ws.Range("B1:E1").Merge()
            ws.Range("B2:N2").Unmerge()
            ws.Range("B2:E2").Merge()
            noofcol = 5
        End If
        Dim count = productiondt.Rows.Count

        If count <> 0 Then

            rowcount = rowcount + 1

            Dim productiondtags = From productiondtag In productiondt.AsEnumerable() Group productiondtag By GroupName = New With {Key .GroupName = productiondtag.Field(Of String)("agentname")} Into Group
            Dim result = (From suppliers In productiondt.AsEnumerable
                                   Group suppliers By agentname = suppliers.Field(Of String)("agentname") Into g = Group
                                   Select New With {
                                       .agentname = agentname, .count = g.Count,
                                      .pax_old = g.Sum(Function(r) r.Field(Of Int32)("pax_old")), .units_old = g.Sum(Function(r) r.Field(Of Int32)("units_old")), .roomnights_old = g.Sum(Function(r) r.Field(Of Int32)("roomnights_old")),
                                  .salevalue_old = g.Sum(Function(r) r.Field(Of Decimal)("salevalue_old")), .pax = g.Sum(Function(r) r.Field(Of Int32)("pax")), .units = g.Sum(Function(r) r.Field(Of Int32)("pax")), .roomnights = g.Sum(Function(r) r.Field(Of Int32)("roomnights")),
                                        .salevalue = g.Sum(Function(r) r.Field(Of Decimal)("salevalue")), .pax_variance = g.Sum(Function(r) r.Field(Of Decimal)("pax_variance")), .units_variance = g.Sum(Function(r) r.Field(Of Decimal)("units_variance")), .roomnights_variance = g.Sum(Function(r) r.Field(Of Decimal)("roomnights_variance")),
                                        .salevalue_variance = g.Sum(Function(r) r.Field(Of Decimal)("salevalue_variance"))
                                  }).ToList()


            Dim tableData
            If Convert.ToInt32(ddlGroupBy.SelectedValue) <> 0 And ddlColumnns.SelectedValue = "All" Then
                tableData = ws.Range(rowcount, 2, rowcount + count - 1, noofcol)
            Else
                tableData = ws.Range(rowcount, 2, rowcount + count - 1, noofcol)
            End If
            tableData.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 10
            tableData.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
            Dim i As Integer = 0
            Dim totalsarray(), subtotalsarray() As Decimal
            Dim noofrows As Decimal = productiondt.Rows.Count


            If ddlColumnns.SelectedValue = "All" Then
                Dim salevalue_Variance As Decimal = 0
                If (productiondt.Compute("sum(salevalue_old)", String.Empty) <> 0) Then
                    salevalue_Variance = ((productiondt.Compute("sum(salevalue)", String.Empty) - productiondt.Compute("sum(salevalue_old)", String.Empty)) * 100) / productiondt.Compute("sum(salevalue_old)", String.Empty)
                Else
                    salevalue_Variance = 100
                End If

                totalsarray = {productiondt.Compute("Sum(pax_old)", String.Empty), productiondt.Compute("sum(units_old)", String.Empty), productiondt.Compute("sum(roomnights_old)", String.Empty), productiondt.Compute("Sum(salevalue_old)", String.Empty), productiondt.Compute("Sum(pax)", String.Empty), productiondt.Compute("sum(units)", String.Empty), productiondt.Compute("sum(roomnights)", String.Empty), productiondt.Compute("Sum(salevalue)", String.Empty), IIf((productiondt.Compute("sum(Pax_old)", String.Empty) <> 0), ((productiondt.Compute("sum(pax)", String.Empty) - productiondt.Compute("sum(pax_old)", String.Empty)) * 100) / productiondt.Compute("sum(Pax_old)", String.Empty), 100), IIf((productiondt.Compute("sum(units_old)", String.Empty) <> 0), ((productiondt.Compute("sum(units)", String.Empty) - productiondt.Compute("sum(units_old)", String.Empty)) * 100) / productiondt.Compute("sum(units_old)", String.Empty), 100), IIf((productiondt.Compute("sum(roomnights_old)", String.Empty) <> 0), ((productiondt.Compute("sum(roomnights)", String.Empty) - productiondt.Compute("sum(roomnights_old)", String.Empty)) * 100) / productiondt.Compute("sum(roomnights_old)", String.Empty), 100), FormatNumber(Math.Round(salevalue_Variance, decno), UseParensForNegativeNumbers:=TriState.True)}
            ElseIf ddlColumnns.SelectedValue = "RoomNights" Then

                totalsarray = {productiondt.Compute("sum(roomnights_old)", String.Empty), productiondt.Compute("sum(roomnights)", String.Empty), IIf((productiondt.Compute("sum(roomnights_old)", String.Empty) <> 0), ((productiondt.Compute("sum(roomnights)", String.Empty) - productiondt.Compute("sum(roomnights_old)", String.Empty)) * 100) / productiondt.Compute("sum(roomnights_old)", String.Empty), 100)}


            ElseIf ddlColumnns.SelectedValue = "Pax" Then
                totalsarray = {productiondt.Compute("sum(pax_old)", String.Empty), productiondt.Compute("sum(pax)", String.Empty), IIf((productiondt.Compute("sum(Pax_old)", String.Empty) <> 0), ((productiondt.Compute("sum(pax)", String.Empty) - productiondt.Compute("sum(pax_old)", String.Empty)) * 100) / productiondt.Compute("sum(Pax_old)", String.Empty), 100)}


            ElseIf ddlColumnns.SelectedValue = "Units" Then

                totalsarray = {productiondt.Compute("sum(units_old)", String.Empty), productiondt.Compute("sum(units)", String.Empty), IIf((productiondt.Compute("sum(units_old)", String.Empty) <> 0), ((productiondt.Compute("sum(units)", String.Empty) - productiondt.Compute("sum(units_old)", String.Empty)) * 100) / productiondt.Compute("sum(units_old)", String.Empty), 100)}

            ElseIf ddlColumnns.SelectedValue = "SaleValue" Then
                ' Dim salevalueold_total As Decimal = Convert.ToDecimal(productiondt.Compute("sum(salevalue_old)", String.Empty))
                Dim salevalue_variance As Decimal = 0
                Dim compare As Decimal = 0 '
                If productiondt.Compute("sum(salevalue_old)", String.Empty) <> 0 Then
                    salevalue_variance = ((productiondt.Compute("sum(salevalue)", String.Empty) - productiondt.Compute("sum(salevalue_old)", String.Empty)) * 100) / productiondt.Compute("sum(salevalue_old)", String.Empty)

                Else
                    salevalue_variance = 100
                End If
                totalsarray = {productiondt.Compute("sum(salevalue_old)", String.Empty), productiondt.Compute("sum(salevalue)", String.Empty), FormatNumber(Math.Round(salevalue_variance, decno), UseParensForNegativeNumbers:=TriState.True)}


            End If


            If Convert.ToInt32(ddlGroupBy.SelectedValue) = 0 Then


                For i = 0 To productiondtags.Count - 1

                    ws.Cell(rowcount, 2).Value = productiondtags(i).GroupName.GroupName
                    ws.Range(rowcount, 2, rowcount, noofcol).Merge()
                    ws.Range(rowcount, 2, rowcount, noofcol).Style.Fill.SetBackgroundColor(XLColor.LightGray)
                    ws.Range(rowcount, 2, rowcount, noofcol).Style.Font.Bold = True
                    ws.Range(rowcount, 2, rowcount, noofcol).Style.Font.FontSize = 11
                    rowcount = rowcount + 1

                    For Each row In productiondtags(i).Group

                        ShowSelectedColumns(row, ws, rowcount, decno, decPlaces)
                        tableData = ws.Range(rowcount, 2, rowcount + result(i).count, noofcol)
                        tableData.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
                        tableData.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True


                        rowcount = rowcount + 1

                    Next

                    '  ws.Range(rowcount, 2, rowcount, noofcol).Style.Fill.SetBackgroundColor(XLColor.LightSkyBlue)
                    ws.Range(rowcount, 2, rowcount, noofcol).Style.Font.Bold = True


                    If ddlColumnns.SelectedValue = "All" Then

                        Dim salevalue_variance As Decimal = 0
                        If ((result(i).salevalue_old) <> 0) Then
                            salevalue_variance = ((result(i).salevalue - result(i).salevalue_old) * 100) / result(i).salevalue_old
                        Else
                            salevalue_variance = 100
                        End If

                        subtotalsarray = {result(i).pax_old, result(i).units_old, result(i).roomnights_old, result(i).salevalue_old, result(i).pax, result(i).units, result(i).roomnights, FormatNumber(Math.Round(result(i).salevalue, decno), UseParensForNegativeNumbers:=TriState.True), IIf((result(i).pax_old <> 0), FormatNumber(Math.Round(((result(i).pax - result(i).pax_old) * 100) / result(i).pax_old, decno), UseParensForNegativeNumbers:=TriState.True), 100), IIf((result(i).units_old <> 0), ((result(i).units - result(i).units_old) * 100) / result(i).units_old, 100), IIf((result(i).roomnights_old <> 0), ((result(i).roomnights - result(i).roomnights_old) * 100) / result(i).roomnights_old, 100), FormatNumber(Math.Round(salevalue_variance, decno), UseParensForNegativeNumbers:=TriState.True)}


                    ElseIf ddlColumnns.SelectedValue = "RoomNights" Then
                        ' tableData = ws.Range(rowcount, 2, rowcount + result(i).count, noofcol)
                        subtotalsarray = {result(i).roomnights_old, result(i).roomnights, IIf((result(i).roomnights_old <> 0), ((result(i).roomnights - result(i).roomnights_old) * 100) / result(i).roomnights_old, 100)}

                    ElseIf ddlColumnns.SelectedValue = "Pax" Then
                        ' tableData = ws.Range(rowcount, 2, rowcount + result(i).count, noofcol)

                        subtotalsarray = {result(i).pax_old, result(i).pax, IIf((result(i).pax_old <> 0), ((result(i).pax - result(i).pax_old) * 100) / result(i).pax_old, 100)}

                    ElseIf ddlColumnns.SelectedValue = "Units" Then
                        '   tableData = ws.Range(rowcount, 2, rowcount + result(i).count, noofcol)

                        ' tableData = ws.Range(rowcount, 2, rowcount + result(i).count, noofcol)

                        subtotalsarray = {result(i).units_old, result(i).units, IIf((result(i).units_old <> 0), ((result(i).units - result(i).units_old) * 100) / result(i).units_old, 100)}

                    ElseIf ddlColumnns.SelectedValue = "SaleValue" Then
                        '    tableData = ws.Range(rowcount, 2, rowcount + result(i).count, noofcol)
                        Dim salevalue_variance As Decimal = 0
                        If ((result(i).salevalue_old) <> 0) Then
                            salevalue_variance = ((result(i).salevalue - result(i).salevalue_old) * 100) / result(i).salevalue_old
                        Else
                            salevalue_variance = 100
                        End If
                        subtotalsarray = {FormatNumber(Math.Round(result(i).salevalue_old, decno), UseParensForNegativeNumbers:=TriState.True), FormatNumber(Math.Round(result(i).salevalue, decno), UseParensForNegativeNumbers:=TriState.True), FormatNumber(Math.Round(salevalue_variance, decno), UseParensForNegativeNumbers:=TriState.True)}

                    End If
                    ' tableData.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
                    ' tableData.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True


                    Dim rowtext As String = " Sub Total"
                    Totalrow(subtotalsarray, ws, rowcount, noofcol, decno, rowtext)

                    rowcount = rowcount + 1



                Next


            Else
                For Each row In productiondt.Rows
                    ShowSelectedColumns(row, ws, rowcount, decno, decPlaces)
                    rowcount = rowcount + 1
                Next

            End If




            If productiondt.Rows.Count > 0 Then
                Totalrow(totalsarray, ws, rowcount, decno, noofcol)
            End If



        End If



        ' Dim bytes() As Byte
        Using MyMemoryStream As New MemoryStream()
            wb.SaveAs(MyMemoryStream)

            wb.Dispose()
            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment;filename=ProductionComparison.xlsx")
            Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"

            MyMemoryStream.WriteTo(Response.OutputStream)
            ' Response.Cookies.Add(New HttpCookie("Downloaded", "True"))
            Response.Flush()
            HttpContext.Current.ApplicationInstance.CompleteRequest()

        End Using

    End Sub
#End Region




    Private Sub Totalrow(ByVal totalsarray() As Decimal, ByVal ws As IXLWorksheet, ByVal rowcount As Integer, ByVal decno As Integer, ByVal noofcol As Integer, Optional ByVal rowtext As String = "Net Total")
        ws.Cell(rowcount, 2).Value = rowtext
        ws.Cell(rowcount, 2).Style.Font.Bold = True
        ws.Range(rowcount, 2, rowcount, noofcol).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
        ws.Range(rowcount, 2, rowcount, noofcol).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
        ws.Range(rowcount, 2, rowcount, noofcol).Style.Font.Bold = True
        If rowtext = "Net Total" Then
            ws.Range(rowcount, 2, rowcount, noofcol).Style.Font.FontSize = 12
            ws.Range(rowcount, 2, rowcount, noofcol).Style.Fill.BackgroundColor = XLColor.LightGray

        Else
            ws.Range(rowcount, 2, rowcount, noofcol).Style.Font.FontSize = 11
        End If
        ws.Range(rowcount, 2, rowcount, noofcol).Style.Alignment.WrapText = True

        For i As Integer = 0 To totalsarray.Length - 1
            ws.Cell(rowcount, i + 3).Value = FormatNumber(Math.Round(totalsarray(i), decno), UseParensForNegativeNumbers:=TriState.True)
            ws.Cell(rowcount, i + 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right
        Next

        ws.Range(rowcount, 2, rowcount, noofcol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

    End Sub

    Protected Sub txtFromDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        txtToDt.Text = CType(txtFromDt.Text, Date).AddMonths(3)
        txtCmpFrmDt.Text = CType(txtFromDt.Text, Date).AddYears(-1)
        txtCmpToDt.Text = CType(txtCmpFrmDt.Text, Date).AddMonths(3)
    End Sub

    Protected Sub txtCmpFrmDt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCmpFrmDt.TextChanged
        txtCmpToDt.Text = CType(txtCmpFrmDt.Text, Date).AddMonths(3)
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        ' newExcelProdComparison()
        txtFromDt.Text = Now.Date
        txtToDt.Text = Now.Date.AddMonths(3)
        txtCmpFrmDt.Text = Now.Date.AddYears(-1)
        txtCmpToDt.Text = CType(txtCmpFrmDt.Text, Date).AddMonths(3)
        ddlGuestName.SelectedIndex = 0
        ddlBookingStatus.SelectedIndex = 0
        txtHotel.Text = ""
        txtHotelCode.Text = ""
        txtCustGroup.Text = ""
        txtCustGroupCode.Text = ""
        txtCtryGroup.Text = ""
        txtCtryGroupCode.Text = ""
        txtSourceCtry.Text = ""
        txtSourceCtryCode.Text = ""
        txtCustCategory.Text = ""
        txtCustCategoryCode.Text = ""
        txtCust.Text = ""
        txtCustCode.Text = ""
        txtHotelCity.Text = ""
        txtHotelCityCode.Text = ""
        txtHotelCategory.Text = ""
        txtHotelCategoryCode.Text = ""
        ddlGroupBy.SelectedIndex = 0
        ddlColumnns.SelectedIndex = 0
        ddlRequestType.SelectedIndex = 0
        ddlOrder.SelectedIndex = 0
        ddlInvStatus.SelectedIndex = 0
        ChkExclComp.Checked = False
    End Sub

#Region "Protected Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit"
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Request.QueryString("appid") Is Nothing = False Then
            Dim appid As String = CType(Request.QueryString("appid"), String)
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
    End Sub
#End Region
    '#Region "Protected sub newExcelProdComparison() "
    '    Public Sub newExcelProdComparison()

    '        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
    '        mySqlCmd = New SqlCommand("sp_rep_production_comparison_new", mySqlConn)
    '        mySqlCmd.CommandType = CommandType.StoredProcedure
    '        mySqlCmd.Parameters.Add(New SqlParameter("@datetype", SqlDbType.VarChar, 20)).Value = "Arrival" ' ddldatetype.SelectedValue 'Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
    '        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtFromDt.Text).ToString("yyyy/MM/dd") ' CType(IIf(TxtAgentCode.Text.Trim <> "", TxtAgentCode.Text.Trim, ""), String)
    '        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtToDt.Text).ToString("yyyy/MM/dd") 'CType(IIf(TxtSectorCode.Text.Trim <> "", TxtSectorCode.Text.Trim, ""), String)
    '        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate1", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtCmpFrmDt.Text).ToString("yyyy/MM/dd") ' CType(IIf(TxtCtryCode.Text.Trim <> "", TxtCtryCode.Text.Trim, ""), String)
    '        mySqlCmd.Parameters.Add(New SqlParameter("@todate1", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtCmpToDt.Text).ToString("yyyy/MM/dd") ' CType(IIf(TxtCityCode.Text.Trim <> "", TxtCityCode.Text.Trim, ""), String)
    '        mySqlCmd.Parameters.Add(New SqlParameter("@guesttype", SqlDbType.Int)).Value = Convert.ToInt32(ddlGuestName.SelectedValue)
    '        mySqlCmd.Parameters.Add(New SqlParameter("@bookingstatus", SqlDbType.Int)).Value = Convert.ToInt32(ddlBookingStatus.SelectedValue)
    '        mySqlCmd.Parameters.Add(New SqlParameter("@groupby", SqlDbType.Int)).Value = Convert.ToInt32(ddlGroupBy.SelectedValue)
    '        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroup", SqlDbType.VarChar, 20)).Value = txtCtryGroupCode.Text.Trim
    '        mySqlCmd.Parameters.Add(New SqlParameter("@customergroup", SqlDbType.VarChar, 20)).Value = txtCustGroupCode.Text.Trim
    '        mySqlCmd.Parameters.Add(New SqlParameter("@sourcectrycode", SqlDbType.VarChar, 20)).Value = txtSourceCtryCode.Text.Trim
    '        mySqlCmd.Parameters.Add(New SqlParameter("@agentcatcode", SqlDbType.VarChar, 20)).Value = txtCustCategoryCode.Text.Trim
    '        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = txtCustCode.Text.Trim
    '        mySqlCmd.Parameters.Add(New SqlParameter("@hotelcitycode", SqlDbType.VarChar, 20)).Value = txtHotelCityCode.Text.Trim
    '        mySqlCmd.Parameters.Add(New SqlParameter("@hotelcatcode", SqlDbType.VarChar, 20)).Value = txtHotelCategoryCode.Text.Trim
    '        mySqlCmd.Parameters.Add(New SqlParameter("@hotelcode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
    '        mySqlCmd.Parameters.Add(New SqlParameter("@requesttype", SqlDbType.VarChar, 20)).Value = ddlRequestType.SelectedValue

    '        myDataAdapter = New SqlDataAdapter(mySqlCmd)
    '        myDataAdapter.Fill(myds)
    '        Dim productiondt As DataTable = myds.Tables(0)
    '        If productiondt.Rows.Count <> 0 Then
    '            Dim wb As New XLWorkbook
    '            Dim ws = wb.Worksheets.Add("ProductionComparison")
    '            Dim rowcount As Integer = 4

    '            If Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
    '                ws.Columns("A:B").Width = 16
    '                ws.Columns("C:D").Width = 30
    '                ws.Columns("E:F").Width = 9
    '                ws.Column("G").Width = 10
    '                ws.Columns("H:J").Width = 10
    '                ws.Columns("K:L").Width = 11
    '                ws.Column("M").Width = 13
    '                ws.Columns("N:O").Width = 9
    '                ws.Column("P").Width = 10
    '                ws.Columns("Q:S").Width = 10
    '                ws.Columns("T:U").Width = 12
    '                ws.Column("V").Width = 14
    '            Else
    '                ws.Columns("A:B").Width = 13
    '                ws.Columns("C").Width = 30
    '                ws.Columns("D:E").Width = 9
    '                ws.Column("F").Width = 10
    '                ws.Columns("G:I").Width = 10
    '                ws.Columns("J:K").Width = 11
    '                ws.Column("L").Width = 13
    '                ws.Columns("M:N").Width = 9
    '                ws.Column("O").Width = 10
    '                ws.Columns("P:R").Width = 10
    '                ws.Columns("S:T").Width = 12
    '                ws.Column("U").Width = 14
    '            End If

    '            Dim decno As Integer = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), Integer)
    '            Dim decPlaces As String
    '            If decno = 3 Then
    '                decPlaces = "#,##0.000"
    '            Else
    '                decPlaces = "#,##0.00"
    '            End If
    '            'company Name Heading
    '            Dim company = ws.Range("A1:V1").Merge()
    '            ws.Cell("A1").Value = CType(Session("CompanyName"), String)
    '            company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetBold().Font.FontSize = 15
    '            company.Style.Font.FontColor = XLColor.Black
    '            company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
    '            company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
    '            company.Style.Fill.SetBackgroundColor(XLColor.LightGray)


    '            'Report Name Heading
    '            Dim Report = ws.Range("A2:V2").Merge()
    '            ws.Cell("A2").Value = "Comparative Report  "
    '            Report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetBold().Font.FontSize = 14
    '            Report.Style.Font.FontColor = XLColor.Black
    '            Report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
    '            Report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
    '            Report.Style.Fill.SetBackgroundColor(XLColor.LightGray)

    '            Dim arrHeaders() As String

    '            Dim repfilter As New StringBuilder
    '            repfilter.Append("Current Period :" & txtFromDt.Text & " -  " & txtToDt.Text & "   Compare Period : " & txtCmpFrmDt.Text & " -  " & txtCmpToDt.Text & " ")

    '            If txtHotel.Text <> "" Then
    '                repfilter.Append(" Hotel : " & txtHotel.Text & "")
    '            End If
    '            If txtCtryGroup.Text <> "" Then
    '                repfilter.Append(" Country Group : " & txtCtryGroup.Text & "")
    '            End If
    '            If txtCustGroup.Text <> "" Then
    '                repfilter.Append(" , Customer Group : " & txtCustGroup.Text & "")
    '            End If

    '            If txtSourceCtry.Text <> "" Then
    '                repfilter.Append(" , Source Country : " & txtSourceCtry.Text & "")
    '            End If
    '            If txtCustCategory.Text <> "" Then
    '                repfilter.Append(",  Customer Category :  " & txtCustCategory.Text & "")
    '            End If
    '            If txtCust.Text <> "" Then
    '                repfilter.Append(",  Customer  :  " & txtCust.Text & "")
    '            End If
    '            If txtHotelCity.Text <> "" Then
    '                repfilter.Append(",  Hotel City  :  " & txtHotelCity.Text & "")
    '            End If
    '            If txtHotelCategory.Text <> "" Then
    '                repfilter.Append(",  Hotel Category  :  " & txtHotelCategory.Text & "")
    '            End If

    '            If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Then
    '                arrHeaders = {"Hotel", "Country Group", "Agents", "Pax-Adult", "Pax-Child", "Room Nights", "Selling", "Cost", "MarkUp ", "Mark Up Value", "Commission ", "Commission Value",
    '                              "Pax-Adult", "Pax-Child", "Room Nights", "Selling", "Cost", "MarkUp ", "Mark Up Value", "Commission ", "Commission Value"}
    '                repfilter.Append(" Group By : " & ddlGroupBy.SelectedItem.Text)
    '            ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
    '                arrHeaders = {"Agents", "Location", "Hotel", "Pax-Adult", "Pax-Child", "Room Nights", "Selling", "Cost", "MarkUp", "Mark Up Value", "Commission ", "Commission Value",
    '                             "Pax-Adult", "Pax-Child", "Room Nights", "Selling", "Cost", "MarkUp ", "Mark Up Value", "Commission ", "Commission Value"}
    '                repfilter.Append(" Group By : " & ddlGroupBy.SelectedItem.Text)
    '            ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
    '                arrHeaders = {"Country Group", "Country", "Agents", "Hotel", "Pax-Adult", "Pax-Child", "Room Nights", "Selling", "Cost", "MarkUp ", "Mark Up Value", "Commission ", "Commission Value",
    '                                 "Pax-Adult", "Pax-Child", "Room Nights", "Selling", "Cost", "MarkUp ", "Mark Up Value", "Commission ", "Commission Value"}
    '                repfilter.Append(" Group By : " & ddlGroupBy.SelectedItem.Text)
    '            End If



    '            Dim filter = ws.Range("A3:V3").Merge()
    '            ws.Cell("A3").Value = repfilter
    '            filter.Style.Font.SetBold().Font.FontSize = 12
    '            filter.Style.Font.FontColor = XLColor.Black
    '            filter.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
    '            filter.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

    '            Dim mfrmdt, mtodt, yfrmdt, dfrmdt, dtodt As String
    '            Dim pstart, pend, cstart, cend As Integer
    '            yfrmdt = Convert.ToDateTime(txtCmpFrmDt.Text).Year
    '            dfrmdt = Convert.ToDateTime(txtCmpFrmDt.Text).Day
    '            mfrmdt = Convert.ToDateTime(txtCmpFrmDt.Text).ToString("MMM")
    '            dtodt = Convert.ToDateTime(txtCmpToDt.Text).Day
    '            mtodt = Convert.ToDateTime(txtCmpToDt.Text).ToString("MMM")
    '            If Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
    '                pstart = 5
    '                pend = 13
    '                cstart = 14
    '                cend = 22
    '            Else
    '                pstart = 4
    '                pend = 12
    '                cstart = 13
    '                cend = 21
    '            End If

    '            Dim tabletitle = ws.Range(rowcount, pstart, rowcount, pend).Merge()
    '            ws.Cell(rowcount, pstart).Value = yfrmdt & "    " & dfrmdt & " " & mfrmdt & " - " & dtodt & " " & mtodt
    '            tabletitle.Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
    '            tabletitle.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
    '            tabletitle.Style.Font.FontColor = XLColor.Red
    '            tabletitle.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True

    '            yfrmdt = Convert.ToDateTime(txtFromDt.Text).Year
    '            dfrmdt = Convert.ToDateTime(txtFromDt.Text).Day
    '            mfrmdt = Convert.ToDateTime(txtFromDt.Text).ToString("MMM")
    '            dtodt = Convert.ToDateTime(txtToDt.Text).Day
    '            mtodt = Convert.ToDateTime(txtToDt.Text).ToString("MMM")


    '            tabletitle = ws.Range(rowcount, cstart, rowcount, cend).Merge()
    '            ws.Cell(rowcount, cstart).Value = yfrmdt & "    " & dfrmdt & " " & mfrmdt & " - " & dtodt & " " & mtodt
    '            tabletitle.Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
    '            tabletitle.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
    '            tabletitle.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True


    '            rowcount = rowcount + 1
    '            ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 9
    '            ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '            ws.Range(rowcount, 1, rowcount, pstart - 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
    '            ws.Range(rowcount, pstart, rowcount, cend).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '            For i = 0 To arrHeaders.Length - 1
    '                If i > pstart - 2 AndAlso i < pend Then
    '                    ws.Cell(rowcount, i + 1).Style.Font.FontColor = XLColor.Red
    '                End If
    '                ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
    '            Next
    '            Dim sumdata As DataTable
    '            Dim psale, csale As Integer
    '            psale = 7
    '            csale = 16
    '            Dim gp, gp1, adults, sale, cost, roomnights, sale_old, cost_old, ad_old, ch_old, rm_old As String
    '            If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Then
    '                gp = "partyname"
    '            ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
    '                gp = "agentname"
    '            ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
    '                gp = "countrygroupname"
    '                gp1 = "sourcectryname"
    '                psale = 8
    '                csale = 17
    '            End If
    '            If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Or Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
    '                Dim grpby = From grp In productiondt.AsEnumerable() Group grp By g = New With {Key .customer = grp.Field(Of String)(gp)} Into Group Order By g.customer
    '                '    ws.Columns("I,K,R,T").SetDataType(XLCellValues.Text)

    '                For Each keyData In grpby
    '                    sumdata = keyData.Group.CopyToDataTable()
    '                    sale_old = Math.Round(sumdata.Compute("Sum(salevalue_old)", String.Empty), decno)
    '                    ad_old = Math.Round(sumdata.Compute("Sum(adults_old)", String.Empty), decno)
    '                    ch_old = sumdata.Compute("Sum(child_old)", String.Empty)
    '                    rm_old = sumdata.Compute("Sum(roomnights_old)", String.Empty)
    '                    cost_old = sumdata.Compute("Sum(costvalue_old)", String.Empty)
    '                    sale = Math.Round(sumdata.Compute("Sum(salevalue)", String.Empty), decno)
    '                    adults = sumdata.Compute("Sum(adults)", String.Empty)
    '                    Dim ch = sumdata.AsEnumerable().Sum(Function(s) s.Field(Of Integer)("child"))
    '                    roomnights = sumdata.Compute("Sum(roomnights)", String.Empty)
    '                    cost = Math.Round(sumdata.Compute("Sum(costvalue)", String.Empty), decno)

    '                    arrHeaders = {keyData.g.customer, "", "", ad_old, ch_old, rm_old, sale_old, cost_old, Convert.ToString(Math.Round(sumdata.Compute("Sum(markup_old)", String.Empty), decno)) + "%", Math.Round(sumdata.Compute("Sum(markupvalue_old)", String.Empty), decno), Convert.ToString(Math.Round(sumdata.Compute("Sum(commission_old)", String.Empty), decno)) + "%", Math.Round(sumdata.Compute("Sum(commissionvalue_old)", String.Empty), decno),
    '                               adults, ch, roomnights, sale, cost, Convert.ToString(Math.Round(sumdata.Compute("Sum(markup)", String.Empty), decno)) + "%", Math.Round(sumdata.Compute("Sum(markupvalue)", String.Empty), decno), Convert.ToString(Math.Round(sumdata.Compute("Sum(commission)", String.Empty), decno)) + "%", Math.Round(sumdata.Compute("Sum(commissionvalue)", String.Empty), decno)}
    '                    rowcount = rowcount + 1
    '                    Dim tableData = ws.Range(rowcount, 1, rowcount, cend)
    '                    tableData.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
    '                    tableData.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '                    ws.Range(rowcount, 1, rowcount, pstart - 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
    '                    ws.Range(rowcount, pstart, rowcount, cend).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '                    ws.Range(rowcount, psale, rowcount, psale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                    ws.Range(rowcount, psale + 3, rowcount, psale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                    ws.Range(rowcount, psale + 5, rowcount, psale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                    ws.Range(rowcount, csale, rowcount, csale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                    ws.Range(rowcount, csale + 3, rowcount, csale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                    ws.Range(rowcount, csale + 5, rowcount, csale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                    For i = 0 To arrHeaders.Length - 1
    '                        If i > pstart - 2 AndAlso i < pend Then
    '                            If (i = psale - 1 Or i = psale Or i = psale + 2 Or i = psale + 4) And arrHeaders(i) <> "" Then
    '                                ws.Cell(rowcount, i + 1).Value = Convert.ToDecimal(Decimal.Parse(arrHeaders(i)))
    '                                ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = decPlaces
    '                            Else
    '                                ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
    '                            End If
    '                            ws.Cell(rowcount, i + 1).Style.Font.FontColor = XLColor.Red
    '                        ElseIf (i = csale - 1 Or i = csale Or i = csale + 2 Or i = csale + 4) And arrHeaders(i) <> "" Then
    '                            ws.Cell(rowcount, i + 1).Value = Convert.ToDecimal(Decimal.Parse(arrHeaders(i)))
    '                            ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = decPlaces
    '                        ElseIf i = 0 Then
    '                            ws.Range(rowcount, i + 1, rowcount, i + 3).Merge()
    '                            ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
    '                        Else
    '                            ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
    '                        End If
    '                        ws.Cell(rowcount, i + 1).Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.LightBlue)
    '                    Next

    '                    For Each row In keyData.Group
    '                        If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Then
    '                            arrHeaders = {"", IIf(IsDBNull(row("countrygroupname")), "", row("countrygroupname")), IIf(IsDBNull(row("agentname")), "", row("agentname")), IIf(row("adults_old") = 0, "", row("adults_old")), IIf(row("child_old") = 0, "", row("child_old")), IIf(row("roomnights_old") = 0, "", row("roomnights_old")), IIf(Math.Round(row("salevalue_old"), decno) = 0.0, "", Math.Round(row("salevalue_old"), decno)), IIf(Math.Round(row("costvalue_old"), decno) = 0.0, "", Math.Round(row("costvalue_old"), decno)),
    '                                          IIf(Math.Round(row("markup_old"), decno) = 0.0, "", Convert.ToString(Math.Round(row("markup_old"), decno)) + "%"), IIf(Math.Round(row("markupvalue_old"), decno) = 0.0, "", Math.Round(row("markupvalue_old"), decno)), IIf(Math.Round(row("commission_old"), decno) = 0.0, "", Convert.ToString(Math.Round(row("commission_old"), decno)) + "%"), IIf(Math.Round(row("commissionvalue_old"), decno) = 0.0, "", Math.Round(row("commissionvalue_old"), decno)),
    '                                          IIf(row("adults") = 0, "", row("adults")), IIf(row("child") = 0, "", row("child")), IIf(row("roomnights") = 0, "", row("roomnights")), IIf(Math.Round(row("salevalue"), decno) = 0.0, "", Math.Round(row("salevalue"), decno)), IIf(Math.Round(row("costvalue"), decno) = 0.0, "", Math.Round(row("costvalue"), decno)),
    '                                            IIf(Math.Round(row("markup"), decno) = 0.0, "", Convert.ToString(Math.Round(row("markup"), decno)) + "%"), IIf(Math.Round(row("markupvalue"), decno) = 0.0, "", Math.Round(row("markupvalue"), decno)), IIf(Math.Round(row("commission"), decno) = 0.0, "", Convert.ToString(Math.Round(row("commission"), decno)) + "%"), IIf(Math.Round(row("commissionvalue"), decno) = 0.0, "", Math.Round(row("commissionvalue"), decno))}
    '                        ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
    '                            arrHeaders = {"", IIf(IsDBNull(row("hotelcityname")), "", row("hotelcityname")), IIf(IsDBNull(row("partyname")), "", row("partyname")), IIf(row("adults_old") = 0, "", row("adults_old")), IIf(row("child_old") = 0, "", row("child_old")), IIf(row("roomnights_old") = 0, "", row("roomnights_old")), IIf(Math.Round(row("salevalue_old"), decno) = 0.0, "", Math.Round(row("salevalue_old"), decno)), IIf(Math.Round(row("costvalue_old"), decno) = 0.0, "", Math.Round(row("costvalue_old"), decno)),
    '                                          IIf(Math.Round(row("markup_old"), decno) = 0.0, "", Convert.ToString(Math.Round(row("markup_old"), decno)) + "%"), IIf(Math.Round(row("markupvalue_old"), decno) = 0.0, "", Math.Round(row("markupvalue_old"), decno)), IIf(Math.Round(row("commission_old"), decno) = 0.0, "", Convert.ToString(Math.Round(row("commission_old"), decno)) + "%"), IIf(Math.Round(row("commissionvalue_old"), decno) = 0.0, "", Math.Round(row("commissionvalue_old"), decno)),
    '                                          IIf(row("adults") = 0, "", row("adults")), IIf(row("child") = 0, "", row("child")), IIf(row("roomnights") = 0, "", row("roomnights")), IIf(Math.Round(row("salevalue"), decno) = 0.0, "", Math.Round(row("salevalue"), decno)), IIf(Math.Round(row("costvalue"), decno) = 0.0, "", Math.Round(row("costvalue"), decno)),
    '                                          IIf(Math.Round(row("markup"), decno) = 0.0, " ", Convert.ToString(Math.Round(row("markup"), decno)) + "%"), IIf(Math.Round(row("markupvalue"), decno) = 0.0, "", Math.Round(row("markupvalue"), decno)), IIf(Math.Round(row("commission"), decno) = 0.0, "", Convert.ToString(Math.Round(row("commission"), decno)) + "%"), IIf(Math.Round(row("commissionvalue"), decno) = 0.0, "", Math.Round(row("commissionvalue"), decno))}

    '                        End If
    '                        rowcount += 1
    '                        tableData = ws.Range(rowcount, 1, rowcount, cend)
    '                        tableData.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
    '                        tableData.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
    '                        ws.Range(rowcount, 1, rowcount, pstart - 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
    '                        ws.Range(rowcount, pstart, rowcount, cend).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '                        ws.Range(rowcount, psale, rowcount, psale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                        ws.Range(rowcount, psale + 3, rowcount, psale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                        ws.Range(rowcount, psale + 5, rowcount, psale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                        ws.Range(rowcount, csale, rowcount, csale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                        ws.Range(rowcount, csale + 3, rowcount, csale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                        ws.Range(rowcount, csale + 5, rowcount, csale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)

    '                        ws.Cell(rowcount, 9).SetDataType(XLCellValues.Number)
    '                        For i = 0 To arrHeaders.Length - 1
    '                            If i > pstart - 2 AndAlso i < pend Then
    '                                If (i = psale - 1 Or i = psale Or i = psale + 2 Or i = psale + 4) And arrHeaders(i) <> "" Then
    '                                    ws.Cell(rowcount, i + 1).Value = Convert.ToDecimal(Decimal.Parse(arrHeaders(i)))
    '                                    ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = decPlaces
    '                                Else
    '                                    ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
    '                                End If
    '                                ws.Cell(rowcount, i + 1).Style.Font.FontColor = XLColor.Red
    '                            ElseIf (i = csale - 1 Or i = csale Or i = csale + 2 Or i = csale + 4) And arrHeaders(i) <> "" Then
    '                                ws.Cell(rowcount, i + 1).Value = Convert.ToDecimal(Decimal.Parse(arrHeaders(i)))
    '                                ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = decPlaces
    '                            Else
    '                                ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
    '                            End If
    '                        Next
    '                    Next
    '                Next

    '            Else
    '                ' keyData.g.srctry
    '                Dim grpby = From grp In productiondt.AsEnumerable() Group grp By g = New With {Key .customer = grp.Field(Of String)(gp), .srctry = grp.Field(Of String)(gp1)} Into Group Order By g.customer
    '                For Each keyData In grpby
    '                    sumdata = keyData.Group.CopyToDataTable()
    '                    arrHeaders = {keyData.g.customer, keyData.g.srctry, "", "", IIf(IsDBNull(sumdata.Compute("Sum(adults_old)", String.Empty)), "0", sumdata.Compute("Sum(adults_old)", String.Empty)), IIf(IsDBNull(sumdata.Compute("Sum(child_old)", String.Empty)), "0", sumdata.Compute("Sum(child_old)", String.Empty)), IIf(IsDBNull(sumdata.Compute("Sum(roomnights_old)", String.Empty)), "0", sumdata.Compute("Sum(roomnights_old)", String.Empty)),
    '                                                     IIf(IsDBNull(sumdata.Compute("Sum(salevalue_old)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(salevalue_old)", String.Empty), decno)), IIf(IsDBNull(sumdata.Compute("Sum(costvalue_old)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(costvalue_old)", String.Empty), decno)),
    '                                  IIf(IsDBNull(sumdata.Compute("Sum(markup_old)", String.Empty)), "0", Convert.ToString(Math.Round(sumdata.Compute("Sum(markup_old)", String.Empty), decno)) + "%"), IIf(IsDBNull(sumdata.Compute("Sum(markupvalue_old)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(markupvalue_old)", String.Empty), decno)), IIf(IsDBNull(sumdata.Compute("Sum(commission_old)", String.Empty)), "0", Convert.ToString(Math.Round(sumdata.Compute("Sum(commission_old)", String.Empty), decno)) + "%"), IIf(IsDBNull(sumdata.Compute("Sum(commissionvalue_old)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(commissionvalue_old)", String.Empty), decno)),
    '                                                       IIf(IsDBNull(sumdata.Compute("Sum(adults)", String.Empty)), "0", sumdata.Compute("Sum(adults)", String.Empty)), sumdata.AsEnumerable().Sum(Function(s) s.Field(Of Integer)("child")), IIf(IsDBNull(sumdata.Compute("Sum(roomnights)", String.Empty)), "0", sumdata.Compute("Sum(roomnights)", String.Empty)), IIf(IsDBNull(sumdata.Compute("Sum(salevalue)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(salevalue)", String.Empty), decno)),
    '                                                   IIf(IsDBNull(sumdata.Compute("Sum(costvalue)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(costvalue)", String.Empty), decno)),
    '                                  IIf(IsDBNull(sumdata.Compute("Sum(markup)", String.Empty)), "0", Convert.ToString(Math.Round(sumdata.Compute("Sum(markup)", String.Empty), decno)) + "%"), IIf(IsDBNull(sumdata.Compute("Sum(markupvalue)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(markupvalue)", String.Empty), decno)), IIf(IsDBNull(sumdata.Compute("Sum(commission)", String.Empty)), "0", Convert.ToString(Math.Round(sumdata.Compute("Sum(commission)", String.Empty), decno)) + "%"), IIf(IsDBNull(sumdata.Compute("Sum(commissionvalue)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(commissionvalue)", String.Empty), decno))}
    '                    rowcount = rowcount + 1
    '                    Dim tableData = ws.Range(rowcount, 1, rowcount, 22)
    '                    tableData.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetBold().Font.FontSize = 9
    '                    tableData.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
    '                    ws.Range(rowcount, 1, rowcount, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
    '                    ws.Range(rowcount, 5, rowcount, 22).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '                    ws.Range(rowcount, psale, rowcount, psale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                    ws.Range(rowcount, psale + 3, rowcount, psale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                    ws.Range(rowcount, psale + 5, rowcount, psale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                    ws.Range(rowcount, csale, rowcount, csale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                    ws.Range(rowcount, csale + 3, rowcount, csale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                    ws.Range(rowcount, csale + 5, rowcount, csale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                    For i = 0 To arrHeaders.Length - 1
    '                        If i > pstart - 2 AndAlso i < pend Then
    '                            If (i = psale - 1 Or i = psale Or i = psale + 2 Or i = psale + 4) And arrHeaders(i) <> "" Then
    '                                ws.Cell(rowcount, i + 1).Value = Convert.ToDecimal(Decimal.Parse(arrHeaders(i)))
    '                                ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = decPlaces
    '                            Else
    '                                ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
    '                            End If
    '                            ws.Cell(rowcount, i + 1).Style.Font.FontColor = XLColor.Red
    '                        ElseIf (i = csale - 1 Or i = csale Or i = csale + 2 Or i = csale + 4) And arrHeaders(i) <> "" Then
    '                            ws.Cell(rowcount, i + 1).Value = Convert.ToDecimal(Decimal.Parse(arrHeaders(i)))
    '                            ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = decPlaces
    '                        Else
    '                            ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
    '                        End If
    '                    Next

    '                    Dim grpbyAgent = From grp In sumdata.AsEnumerable() Group grp By g = New With {Key .agent = grp.Field(Of String)("agentname")} Into Group Order By g.agent

    '                    For Each agent In grpbyAgent
    '                        rowcount += 1
    '                        arrHeaders = {"", "", agent.g.agent, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}
    '                        tableData = ws.Range(rowcount, 1, rowcount, 22)
    '                        tableData.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
    '                        tableData.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
    '                        ws.Range(rowcount, 1, rowcount, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
    '                        ws.Range(rowcount, 5, rowcount, 22).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '                        ws.Range(rowcount, psale, rowcount, psale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                        ws.Range(rowcount, psale + 3, rowcount, psale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                        ws.Range(rowcount, psale + 5, rowcount, psale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                        ws.Range(rowcount, csale, rowcount, csale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                        ws.Range(rowcount, csale + 3, rowcount, csale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                        ws.Range(rowcount, csale + 5, rowcount, csale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                        For i = 0 To arrHeaders.Length - 1
    '                            If i > pstart - 2 AndAlso i < pend Then
    '                                If (i = psale - 1 Or i = psale Or i = psale + 2 Or i = psale + 4) And arrHeaders(i) <> "" Then
    '                                    ws.Cell(rowcount, i + 1).Value = Convert.ToDecimal(Decimal.Parse(arrHeaders(i)))
    '                                    ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = decPlaces
    '                                Else
    '                                    ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
    '                                End If
    '                                ws.Cell(rowcount, i + 1).Style.Font.FontColor = XLColor.Red
    '                            ElseIf (i = csale - 1 Or i = csale Or i = csale + 2 Or i = csale + 4) And arrHeaders(i) <> "" Then
    '                                ws.Cell(rowcount, i + 1).Value = Convert.ToDecimal(Decimal.Parse(arrHeaders(i)))
    '                                ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = decPlaces
    '                            Else
    '                                ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
    '                            End If
    '                            ws.Cell(rowcount, i + 1).Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.LightBlue)
    '                        Next

    '                        For Each row In agent.Group
    '                            rowcount += 1
    '                            arrHeaders = {"", "", "", IIf(IsDBNull(row("partyname")), "", row("partyname")), IIf(row("adults_old") = 0, "", row("adults_old")), IIf(row("child_old") = 0, "", row("child_old")), IIf(row("roomnights_old") = 0, "", row("roomnights_old")), IIf(Math.Round(row("salevalue_old"), decno) = 0.0, "", Math.Round(row("salevalue_old"), decno)), IIf(Math.Round(row("costvalue_old"), decno) = 0.0, "", Math.Round(row("costvalue_old"), decno)),
    '                                          IIf(Math.Round(row("markup_old"), decno) = 0.0, "", Convert.ToString(Math.Round(row("markup_old"), decno)) + "%"), IIf(Math.Round(row("markupvalue_old"), decno) = 0.0, "", Math.Round(row("markupvalue_old"), decno)), IIf(Math.Round(row("commission_old"), decno) = 0.0, "", Convert.ToString(Math.Round(row("commission_old"), decno)) + "%"), IIf(Math.Round(row("commissionvalue_old"), decno) = 0.0, "", Math.Round(row("commissionvalue_old"), decno)),
    '                                          IIf(row("adults") = 0, "", row("adults")), IIf(row("child") = 0, "", row("child")), IIf(row("roomnights") = 0, "", row("roomnights")), IIf(Math.Round(row("salevalue"), decno) = 0.0, "", Math.Round(row("salevalue"), decno)), IIf(Math.Round(row("costvalue"), decno) = 0.0, "", Math.Round(row("costvalue"), decno)),
    '                                          IIf(Math.Round(row("markup"), decno) = 0.0, "", Convert.ToString(Math.Round(row("markup"), decno)) + "%"), IIf(Math.Round(row("markupvalue"), decno) = 0.0, "", Math.Round(row("markupvalue"), decno)), IIf(Math.Round(row("commission"), decno) = 0.0, "", Convert.ToString(Math.Round(row("commission"), decno)) + "%"), IIf(Math.Round(row("commissionvalue"), decno) = 0.0, "", Math.Round(row("commissionvalue"), decno))}
    '                            tableData = ws.Range(rowcount, 1, rowcount, 22)
    '                            tableData.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
    '                            tableData.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
    '                            ws.Range(rowcount, 1, rowcount, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
    '                            ws.Range(rowcount, 5, rowcount, 22).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '                            ws.Range(rowcount, psale, rowcount, psale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                            ws.Range(rowcount, psale + 3, rowcount, psale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                            ws.Range(rowcount, psale + 5, rowcount, psale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                            ws.Range(rowcount, csale, rowcount, csale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                            ws.Range(rowcount, csale + 3, rowcount, csale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                            ws.Range(rowcount, csale + 5, rowcount, csale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '                            For i = 0 To arrHeaders.Length - 1
    '                                If i > pstart - 2 AndAlso i < pend Then
    '                                    If (i = psale - 1 Or i = psale Or i = psale + 2 Or i = psale + 4) And arrHeaders(i) <> "" Then
    '                                        ws.Cell(rowcount, i + 1).Value = Convert.ToDecimal(Decimal.Parse(arrHeaders(i)))
    '                                        ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = decPlaces
    '                                    Else
    '                                        ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
    '                                    End If
    '                                    ws.Cell(rowcount, i + 1).Style.Font.FontColor = XLColor.Red
    '                                ElseIf (i = csale - 1 Or i = csale Or i = csale + 2 Or i = csale + 4) And arrHeaders(i) <> "" Then
    '                                    ws.Cell(rowcount, i + 1).Value = Convert.ToDecimal(Decimal.Parse(arrHeaders(i)))
    '                                    ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = decPlaces
    '                                Else
    '                                    ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
    '                                End If
    '                            Next
    '                        Next
    '                    Next
    '                Next
    '            End If
    '            rowcount += 1
    '            Dim Data = ws.Range(rowcount, 1, rowcount, cend)
    '            Data.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetBold().Fill.SetBackgroundColor(XLColor.LightGray).Font.FontSize = 9
    '            Data.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
    '            ws.Range(rowcount, 1, rowcount, pstart - 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '            ws.Range(rowcount, 1, rowcount, pstart - 1).Merge()
    '            ws.Range(rowcount, pstart, rowcount, cend).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
    '            ws.Range(rowcount, psale, rowcount, psale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '            ws.Range(rowcount, psale + 3, rowcount, psale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '            ws.Range(rowcount, psale + 5, rowcount, psale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '            ws.Range(rowcount, csale, rowcount, csale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '            ws.Range(rowcount, csale + 3, rowcount, csale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '            ws.Range(rowcount, csale + 5, rowcount, csale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
    '            If Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
    '                arrHeaders = {"Net Total", "", "", "", IIf(IsDBNull(productiondt.Compute("Sum(adults_old)", String.Empty)), "0", productiondt.Compute("Sum(adults_old)", String.Empty)), IIf(IsDBNull(productiondt.Compute("Sum(child_old)", String.Empty)), "0", productiondt.Compute("Sum(child_old)", String.Empty)), IIf(IsDBNull(productiondt.Compute("Sum(roomnights_old)", String.Empty)), "0", productiondt.Compute("Sum(roomnights_old)", String.Empty)),
    '                                                     IIf(IsDBNull(productiondt.Compute("Sum(salevalue_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(salevalue_old)", String.Empty), decno)), IIf(IsDBNull(productiondt.Compute("Sum(costvalue_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(costvalue_old)", String.Empty), decno)),
    '                             IIf(IsDBNull(productiondt.Compute("Sum(markup_old)", String.Empty)), "0", Convert.ToString(Math.Round(productiondt.Compute("Sum(markup_old)", String.Empty), decno)) + "%"), IIf(IsDBNull(productiondt.Compute("Sum(markupvalue_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(markupvalue_old)", String.Empty), decno)), IIf(IsDBNull(productiondt.Compute("Sum(commission_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(commission_old)", String.Empty), decno)), IIf(IsDBNull(productiondt.Compute("Sum(commissionvalue_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(commissionvalue_old)", String.Empty), decno)),
    '                                                       IIf(IsDBNull(productiondt.Compute("Sum(adults)", String.Empty)), "0", productiondt.Compute("Sum(adults)", String.Empty)), productiondt.AsEnumerable().Sum(Function(s) s.Field(Of Integer)("child")), IIf(IsDBNull(productiondt.Compute("Sum(roomnights)", String.Empty)), "0", productiondt.Compute("Sum(roomnights)", String.Empty)), IIf(IsDBNull(productiondt.Compute("Sum(salevalue)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(salevalue)", String.Empty), decno)),
    '                                                   IIf(IsDBNull(productiondt.Compute("Sum(costvalue)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(costvalue)", String.Empty), decno)),
    '                              IIf(IsDBNull(productiondt.Compute("Sum(markup)", String.Empty)), "0", Convert.ToString(Math.Round(productiondt.Compute("Sum(markup)", String.Empty), decno)) + "%"), IIf(IsDBNull(productiondt.Compute("Sum(markupvalue)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(markupvalue)", String.Empty), decno)), IIf(IsDBNull(productiondt.Compute("Sum(commission)", String.Empty)), "0", Convert.ToString(Math.Round(productiondt.Compute("Sum(commission)", String.Empty), decno)) + "%"), IIf(IsDBNull(productiondt.Compute("Sum(commissionvalue)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(commissionvalue)", String.Empty), decno))}

    '            Else
    '                arrHeaders = {"Net Total", "", "", IIf(IsDBNull(productiondt.Compute("Sum(adults_old)", String.Empty)), "0", productiondt.Compute("Sum(adults_old)", String.Empty)), IIf(IsDBNull(productiondt.Compute("Sum(child_old)", String.Empty)), "0", productiondt.Compute("Sum(child_old)", String.Empty)), IIf(IsDBNull(productiondt.Compute("Sum(roomnights_old)", String.Empty)), "0", productiondt.Compute("Sum(roomnights_old)", String.Empty)),
    '                                                     IIf(IsDBNull(productiondt.Compute("Sum(salevalue_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(salevalue_old)", String.Empty), decno)), IIf(IsDBNull(productiondt.Compute("Sum(costvalue_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(costvalue_old)", String.Empty), decno)),
    '                             IIf(IsDBNull(productiondt.Compute("Sum(markup_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(markup_old)", String.Empty), decno)), IIf(IsDBNull(productiondt.Compute("Sum(markupvalue_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(markupvalue_old)", String.Empty), decno)), IIf(IsDBNull(productiondt.Compute("Sum(commission_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(commission_old)", String.Empty), decno)), IIf(IsDBNull(productiondt.Compute("Sum(commissionvalue_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(commissionvalue_old)", String.Empty), decno)),
    '                                                       IIf(IsDBNull(productiondt.Compute("Sum(adults)", String.Empty)), "0", productiondt.Compute("Sum(adults)", String.Empty)), productiondt.AsEnumerable().Sum(Function(s) s.Field(Of Integer)("child")), IIf(IsDBNull(productiondt.Compute("Sum(roomnights)", String.Empty)), "0", productiondt.Compute("Sum(roomnights)", String.Empty)), IIf(IsDBNull(productiondt.Compute("Sum(salevalue)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(salevalue)", String.Empty), decno)),
    '                                                   IIf(IsDBNull(productiondt.Compute("Sum(costvalue)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(costvalue)", String.Empty), decno)),
    '                              IIf(IsDBNull(productiondt.Compute("Sum(markup)", String.Empty)), "0", Convert.ToString(Math.Round(productiondt.Compute("Sum(markup)", String.Empty), decno)) + "%"), IIf(IsDBNull(productiondt.Compute("Sum(markupvalue)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(markupvalue)", String.Empty), decno)), IIf(IsDBNull(productiondt.Compute("Sum(commission)", String.Empty)), "0", Convert.ToString(Math.Round(productiondt.Compute("Sum(commission)", String.Empty), decno)) + "%"), IIf(IsDBNull(productiondt.Compute("Sum(commissionvalue)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(commissionvalue)", String.Empty), decno))}
    '            End If

    '            For i = 0 To arrHeaders.Length - 1
    '                If i > pstart - 2 AndAlso i < pend Then
    '                    If (i = psale - 1 Or i = psale Or i = psale + 2 Or i = psale + 4) And arrHeaders(i) <> "" Then
    '                        ws.Cell(rowcount, i + 1).Value = Convert.ToDecimal(Decimal.Parse(arrHeaders(i)))
    '                        ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = decPlaces
    '                    Else
    '                        ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
    '                    End If
    '                    ws.Cell(rowcount, i + 1).Style.Font.FontColor = XLColor.Red
    '                ElseIf (i = csale - 1 Or i = csale Or i = csale + 2 Or i = csale + 4) And arrHeaders(i) <> "" Then
    '                    ws.Cell(rowcount, i + 1).Value = Convert.ToDecimal(Decimal.Parse(arrHeaders(i)))
    '                    ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = decPlaces
    '                Else
    '                    ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
    '                End If
    '            Next


    '            ws.Cell((rowcount + 4), 1).Value = "Printed Date : " & Now.ToString("dd/MM/yyyy")
    '            ws.Range((rowcount + 4), 1, (rowcount + 4), 3).Merge()
    '            Using MyMemoryStream As New MemoryStream()
    '                wb.SaveAs(MyMemoryStream)
    '                wb.Dispose()
    '                Response.Clear()
    '                Response.Buffer = True
    '                Response.AddHeader("content-disposition", "attachment;filename=Comparative" & Now.ToString("dd/MM/yyyy") & ".xlsx")
    '                Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
    '                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"

    '                MyMemoryStream.WriteTo(Response.OutputStream)
    '                Response.Flush()
    '                HttpContext.Current.ApplicationInstance.CompleteRequest()

    '            End Using
    '        Else
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Records not found, Please redefine search criteria');", True)
    '        End If
    '    End Sub
    '#End Region
#Region "Protected sub newExcelProdComparison() "
    Public Sub newExcelProdComparison()

        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
        mySqlCmd = New SqlCommand("sp_rep_production_comparison_new", mySqlConn)
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.Parameters.Add(New SqlParameter("@datetype", SqlDbType.VarChar, 20)).Value = "Arrival" ' ddldatetype.SelectedValue 'Convert.ToDateTime(txtFromDate.Text).ToString("yyyy/MM/dd")
        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtFromDt.Text).ToString("yyyy/MM/dd") ' CType(IIf(TxtAgentCode.Text.Trim <> "", TxtAgentCode.Text.Trim, ""), String)
        mySqlCmd.Parameters.Add(New SqlParameter("@todate", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtToDt.Text).ToString("yyyy/MM/dd") 'CType(IIf(TxtSectorCode.Text.Trim <> "", TxtSectorCode.Text.Trim, ""), String)
        mySqlCmd.Parameters.Add(New SqlParameter("@fromdate1", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtCmpFrmDt.Text).ToString("yyyy/MM/dd") ' CType(IIf(TxtCtryCode.Text.Trim <> "", TxtCtryCode.Text.Trim, ""), String)
        mySqlCmd.Parameters.Add(New SqlParameter("@todate1", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtCmpToDt.Text).ToString("yyyy/MM/dd") ' CType(IIf(TxtCityCode.Text.Trim <> "", TxtCityCode.Text.Trim, ""), String)
        mySqlCmd.Parameters.Add(New SqlParameter("@guesttype", SqlDbType.Int)).Value = Convert.ToInt32(ddlGuestName.SelectedValue)
        mySqlCmd.Parameters.Add(New SqlParameter("@bookingstatus", SqlDbType.Int)).Value = Convert.ToInt32(ddlBookingStatus.SelectedValue)
        mySqlCmd.Parameters.Add(New SqlParameter("@groupby", SqlDbType.Int)).Value = Convert.ToInt32(ddlGroupBy.SelectedValue)
        mySqlCmd.Parameters.Add(New SqlParameter("@countrygroup", SqlDbType.VarChar, 20)).Value = txtCtryGroupCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@customergroup", SqlDbType.VarChar, 20)).Value = txtCustGroupCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@sourcectrycode", SqlDbType.VarChar, 20)).Value = txtSourceCtryCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@agentcatcode", SqlDbType.VarChar, 20)).Value = txtCustCategoryCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = txtCustCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@hotelcitycode", SqlDbType.VarChar, 20)).Value = txtHotelCityCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@hotelcatcode", SqlDbType.VarChar, 20)).Value = txtHotelCategoryCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@hotelcode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@requesttype", SqlDbType.VarChar, 20)).Value = ddlRequestType.SelectedValue
        mySqlCmd.Parameters.Add(New SqlParameter("@hotelgroupcode", SqlDbType.VarChar, 20)).Value = txtHotelGroupCode.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@hotelchaincode", SqlDbType.VarChar, 20)).Value = Txt_HotelChain.Text.Trim
        mySqlCmd.Parameters.Add(New SqlParameter("@bookingstatusmanual", SqlDbType.VarChar, 100)).Value = ddlbookingstatusdb.SelectedValue
        mySqlCmd.Parameters.Add(New SqlParameter("@excludestaffroom", SqlDbType.Int)).Value = IIf(Chk_StafRoom.Checked = True, 1, 0) '*** Danny Staff Room 28/03/2019 

        mySqlCmd.CommandTimeout = 0
        myDataAdapter = New SqlDataAdapter(mySqlCmd)
        myDataAdapter.Fill(myds)
        Dim productiondt As DataTable = myds.Tables(0)
        If productiondt.Rows.Count <> 0 Then
            Dim wb As New XLWorkbook
            Dim ws = wb.Worksheets.Add("ProductionComparison")
            Dim rowcount As Integer = 4
            Dim colcount, filterCount As Integer
            If Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
                If ddlColumnns.SelectedValue = "All" Then


                    ws.Columns("A:B").Width = 16
                    ws.Columns("C:D").Width = 30
                    ws.Column("E").Width = 14
                    ws.Columns("F:G").Width = 9
                    ws.Column("H").Width = 14
                    ws.Columns("I:J").Width = 10
                    ws.Columns("K:L").Width = 11
                    ws.Columns("M:P").Width = 13

                    ws.Column("Q").Width = 9
                    ws.Column("R").Width = 10
                    ws.Column("T").Width = 14
                    ws.Columns("U:V").Width = 10
                    ws.Columns("W:AB").Width = 13
                    '  ws.Column("Z:AA").Width = 14
                    colcount = 28
                    filterCount = 220
                Else
                    ws.Columns("A:B").Width = 16
                    ws.Columns("C:D").Width = 30
                    ws.Columns("E:F").Width = 20
                    colcount = 6
                    filterCount = 60
                End If

            Else
                If ddlColumnns.SelectedValue = "All" Then
                    ws.Columns("A:B").Width = 13
                    ws.Columns("C").Width = 30
                    ws.Column("D").Width = 14
                    ws.Columns("E:F").Width = 9
                    ws.Column("G").Width = 14
                    ws.Columns("H:I").Width = 10
                    ws.Columns("J:K").Width = 11
                    ws.Columns("L:O").Width = 13


                    ws.Column("Q").Width = 10
                    ws.Column("S").Width = 14
                    ws.Columns("T:U").Width = 10
                    ws.Columns("V:AA").Width = 13

                    colcount = 27
                    filterCount = 220
                Else
                    ws.Columns("A:B").Width = 16
                    ws.Columns("C").Width = 30
                    ws.Columns("D:E").Width = 20
                    colcount = 5
                    filterCount = 50
                End If
            End If

            Dim decno As Integer = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=509"), Integer)
            Dim decPlaces As String
            If decno = 3 Then
                decPlaces = "#,##0.000"
            Else
                decPlaces = "#,##0.00"
            End If
            'company Name Heading
            Dim company = ws.Range(1, 1, 1, colcount).Merge()
            ws.Cell("A1").Value = CType(Session("CompanyName"), String)
            company.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetBold().Font.FontSize = 15
            company.Style.Font.FontColor = XLColor.Black
            company.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            company.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            company.Style.Fill.SetBackgroundColor(XLColor.LightGray)


            'Report Name Heading
            Dim Report = ws.Range(2, 1, 2, colcount).Merge()
            ws.Cell("A2").Value = "Comparative Report  "
            Report.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Font.SetBold().Font.FontSize = 14
            Report.Style.Font.FontColor = XLColor.Black
            Report.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            Report.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
            Report.Style.Fill.SetBackgroundColor(XLColor.LightGray)

            Dim arrHeaders() As String
            Dim netTotal() As String
            Dim repfilter As New StringBuilder
            repfilter.Append("Current Period :" & txtFromDt.Text & " -  " & txtToDt.Text & "   Compare Period : " & txtCmpFrmDt.Text & " -  " & txtCmpToDt.Text & " ")

            If txtHotel.Text <> "" Then
                repfilter.Append(" Hotel : " & txtHotel.Text & "")
            End If
            If txtCtryGroup.Text <> "" Then
                repfilter.Append(" Country Group : " & txtCtryGroup.Text & "")
            End If


            If txtSourceCtry.Text <> "" Then
                repfilter.Append(" , Source Country : " & txtSourceCtry.Text & "")
            End If

            If txtCust.Text <> "" Then
                repfilter.Append(",  Agent  :  " & txtCust.Text & "")
            End If
            If txtCustGroup.Text <> "" Then
                repfilter.Append(" , Agent Group : " & txtCustGroup.Text & "")
            End If
            If txtCustCategory.Text <> "" Then
                repfilter.Append(",  Agent Category :  " & txtCustCategory.Text & "")
            End If
            If txtHotelCity.Text <> "" Then
                repfilter.Append(",  Hotel City  :  " & txtHotelCity.Text & "")
            End If
            If txtHotelCategory.Text <> "" Then
                repfilter.Append(",  Hotel Category  :  " & txtHotelCategory.Text & "")
            End If

            Dim lsCurrencyCode As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457")
            repfilter.Append("   Currency: " & Convert.ToString(lsCurrencyCode) & "  ")


            Dim netcommissionpercent, netcommissionpercent_old, netmarkuppercent, netmarkuppercent_old As Decimal
            Dim netcostvalue, netmarkupvalue, netcommissionvalue, netcostvalue_old, netmarkupvalue_old, netcommissionvalue_old As Decimal
            netcostvalue_old = IIf(IsDBNull(productiondt.Compute("Sum(costvalue_old)", String.Empty)), "0", productiondt.Compute("Sum(costvalue_old)", String.Empty))
            netmarkupvalue_old = IIf(IsDBNull(productiondt.Compute("Sum(markupvalue_old)", String.Empty)), "0", productiondt.Compute("Sum(markupvalue_old)", String.Empty))
            netcommissionvalue_old = IIf(IsDBNull(productiondt.Compute("Sum(commissionvalue_old)", String.Empty)), "0", productiondt.Compute("Sum(commissionvalue_old)", String.Empty))


            netcostvalue = IIf(IsDBNull(productiondt.Compute("Sum(costvalue)", String.Empty)), "0", productiondt.Compute("Sum(costvalue)", String.Empty))
            netmarkupvalue = IIf(IsDBNull(productiondt.Compute("Sum(markupvalue)", String.Empty)), "0", productiondt.Compute("Sum(markupvalue)", String.Empty))
            netcommissionvalue = IIf(IsDBNull(productiondt.Compute("Sum(commissionvalue)", String.Empty)), "0", productiondt.Compute("Sum(commissionvalue)", String.Empty))
            Dim netnoofrooms, netnights, netnoofrooms_old, netnights_old As Integer
            netnoofrooms = productiondt.Compute("Sum(noofrooms)", String.Empty)
            netnights = productiondt.Compute("Sum(nights)", String.Empty)
            netnoofrooms_old = productiondt.Compute("Sum(noofrooms_old)", String.Empty)
            netnights_old = productiondt.Compute("Sum(nights_old)", String.Empty)

            If (netcostvalue_old = 0 Or netmarkupvalue_old = 0) Then

                netmarkuppercent_old = "0"
            Else

                netmarkuppercent_old = Convert.ToString(Math.Round(netmarkupvalue_old / netcostvalue_old * 100, decno))
            End If


            If (netcostvalue = 0 Or netmarkupvalue = 0) Then
                netmarkuppercent = "0"
            Else
                netmarkuppercent = Convert.ToString(Math.Round(netmarkupvalue / netcostvalue * 100, decno))
            End If
            ' IIf(IsDBNull(productiondt.Compute("Sum(commissionvalue)", String.Empty)), "0",
            If (netcommissionvalue = "0") Then
                netcommissionpercent = "0"
            Else
                netcommissionpercent = Convert.ToString(Math.Round(netcommissionvalue / (netcostvalue + netcommissionvalue) * 100, decno))

            End If


            If (netcommissionvalue_old = "0") Then
                netcommissionpercent_old = "0"
            Else
                netcommissionpercent_old = Convert.ToString(Math.Round(netcommissionvalue_old / (netcostvalue_old + netcommissionvalue_old) * 100, decno))

            End If

            If ddlColumnns.SelectedValue = "All" Then
                If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Then
                    arrHeaders = {"Hotel", "Country Group", "Agents", "Summary", "Pax-Adult", "Pax-Child", "No.Of Rooms/Units", "Nights", "Room Nights", "Selling", "Cost", "MarkUp ", "Mark Up Value", "Commission ", "Commission Value",
                                   "Summary", "Pax-Adult", "Pax-Child", "No.Of Rooms/Units", "Nights", "Room Nights", "Selling", "Cost", "MarkUp ", "Mark Up Value", "Commission ", "Commission Value"}
                ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
                    arrHeaders = {"Agents", "Location", "Hotel", "Summary", "Pax-Adult", "Pax-Child", "No.Of Rooms/Units", "Nights", "Room Nights", "Selling", "Cost", "MarkUp", "Mark Up Value", "Commission ", "Commission Value",
                                  "Summary", "Pax-Adult", "Pax-Child", "No.Of Rooms/Units", "Nights", "Room Nights", "Selling", "Cost", "MarkUp ", "Mark Up Value", "Commission ", "Commission Value"}
                ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
                    arrHeaders = {"Country Group", "Country", "Agents", "Hotel", "Summary", "Pax-Adult", "Pax-Child", "No.Of Rooms/Units", "Nights", "Room Nights", "Selling", "Cost", "MarkUp ", "Mark Up Value", "Commission ", "Commission Value",
                                      "Summary", "Pax-Adult", "Pax-Child", "No.Of Rooms/Units", "Nights", "Room Nights", "Selling", "Cost", "MarkUp ", "Mark Up Value", "Commission ", "Commission Value"}
                End If
                If Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
                    netTotal = {"Net Total", "", "", "", IIf(IsDBNull(productiondt.Compute("Sum(noofbookings_old)", String.Empty)), "0", productiondt.Compute("Sum(noofbookings_old)", String.Empty)), IIf(IsDBNull(productiondt.Compute("Sum(adults_old)", String.Empty)), "0", productiondt.Compute("Sum(adults_old)", String.Empty)), IIf(IsDBNull(productiondt.Compute("Sum(child_old)", String.Empty)), "0", productiondt.Compute("Sum(child_old)", String.Empty)), IIf(IsDBNull(netnoofrooms_old), "0", netnoofrooms_old), IIf(IsDBNull(netnights_old), "0", netnights_old), IIf(IsDBNull(productiondt.Compute("Sum(roomnights_old)", String.Empty)), "0", productiondt.Compute("Sum(roomnights_old)", String.Empty)),
                                                         IIf(IsDBNull(productiondt.Compute("Sum(salevalue_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(salevalue_old)", String.Empty), decno)), IIf(IsDBNull(productiondt.Compute("Sum(costvalue_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(costvalue_old)", String.Empty), decno)), netmarkuppercent,
                                IIf(IsDBNull(productiondt.Compute("Sum(markupvalue_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(markupvalue_old)", String.Empty), decno)), netcommissionpercent_old, IIf(IsDBNull(productiondt.Compute("Sum(commissionvalue_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(commissionvalue_old)", String.Empty), decno)),
                                                          IIf(IsDBNull(productiondt.Compute("Sum(noofbookings)", String.Empty)), "0", productiondt.Compute("Sum(noofbookings)", String.Empty)), IIf(IsDBNull(productiondt.Compute("Sum(adults)", String.Empty)), "0", productiondt.Compute("Sum(adults)", String.Empty)), productiondt.AsEnumerable().Sum(Function(s) s.Field(Of Integer)("child")), IIf(IsDBNull(netnoofrooms), "0", netnoofrooms), IIf(IsDBNull(netnights), "0", netnights), IIf(IsDBNull(productiondt.Compute("Sum(roomnights)", String.Empty)), "0", productiondt.Compute("Sum(roomnights)", String.Empty)), IIf(IsDBNull(productiondt.Compute("Sum(salevalue)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(salevalue)", String.Empty), decno)),
                                                       IIf(IsDBNull(productiondt.Compute("Sum(costvalue)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(costvalue)", String.Empty), decno)), netmarkuppercent,
                                  IIf(IsDBNull(productiondt.Compute("Sum(markupvalue)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(markupvalue)", String.Empty), decno)), netcommissionpercent, IIf(IsDBNull(productiondt.Compute("Sum(commissionvalue)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(commissionvalue)", String.Empty), decno))}

                Else
                    netTotal = {"Net Total", "", "", IIf(IsDBNull(productiondt.Compute("Sum(noofbookings_old)", String.Empty)), "0", productiondt.Compute("Sum(noofbookings_old)", String.Empty)), IIf(IsDBNull(productiondt.Compute("Sum(adults_old)", String.Empty)), "0", productiondt.Compute("Sum(adults_old)", String.Empty)), IIf(IsDBNull(productiondt.Compute("Sum(child_old)", String.Empty)), "0", productiondt.Compute("Sum(child_old)", String.Empty)), IIf(IsDBNull(netnoofrooms_old), "0", netnoofrooms_old), IIf(IsDBNull(netnights_old), "0", netnights_old), IIf(IsDBNull(productiondt.Compute("Sum(roomnights_old)", String.Empty)), "0", productiondt.Compute("Sum(roomnights_old)", String.Empty)),
                                                         IIf(IsDBNull(productiondt.Compute("Sum(salevalue_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(salevalue_old)", String.Empty), decno)), IIf(IsDBNull(productiondt.Compute("Sum(costvalue_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(costvalue_old)", String.Empty), decno)),
                                  netmarkuppercent_old, IIf(IsDBNull(productiondt.Compute("Sum(markupvalue_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(markupvalue_old)", String.Empty), decno)), netcommissionpercent_old, IIf(IsDBNull(productiondt.Compute("Sum(commissionvalue_old)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(commissionvalue_old)", String.Empty), decno)),
                                                         IIf(IsDBNull(productiondt.Compute("Sum(noofbookings)", String.Empty)), "0", productiondt.Compute("Sum(noofbookings)", String.Empty)), IIf(IsDBNull(productiondt.Compute("Sum(adults)", String.Empty)), "0", productiondt.Compute("Sum(adults)", String.Empty)), productiondt.AsEnumerable().Sum(Function(s) s.Field(Of Integer)("child")), IIf(IsDBNull(netnoofrooms), "0", netnoofrooms), IIf(IsDBNull(netnights), "0", netnights), IIf(IsDBNull(productiondt.Compute("Sum(roomnights)", String.Empty)), "0", productiondt.Compute("Sum(roomnights)", String.Empty)), IIf(IsDBNull(productiondt.Compute("Sum(salevalue)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(salevalue)", String.Empty), decno)),
                                                       IIf(IsDBNull(productiondt.Compute("Sum(costvalue)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(costvalue)", String.Empty), decno)),
                     netmarkuppercent, IIf(IsDBNull(productiondt.Compute("Sum(markupvalue)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(markupvalue)", String.Empty), decno)), netcommissionpercent, IIf(IsDBNull(productiondt.Compute("Sum(commissionvalue)", String.Empty)), "0", Math.Round(productiondt.Compute("Sum(commissionvalue)", String.Empty), decno))}
                End If
            ElseIf ddlColumnns.SelectedValue = "RoomNights" Then
                If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Then
                    arrHeaders = {"Hotel", "Country Group", "Agents", "Total Room Nights", "Total Room Nights"}
                ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
                    arrHeaders = {"Agents", "Location", "Hotel", "Total Room Nights", "Total Room Nights"}
                ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
                    arrHeaders = {"Country Group", "Country", "Agents", "Hotel", "Total Room Nights", "Total Room Nights"}
                End If
                If Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
                    netTotal = {"Net Total", "", "", "", IIf(IsDBNull(productiondt.Compute("Sum(roomnights_old)", String.Empty)), "0", productiondt.Compute("Sum(roomnights_old)", String.Empty)),
                                                              IIf(IsDBNull(productiondt.Compute("Sum(roomnights)", String.Empty)), "0", productiondt.Compute("Sum(roomnights)", String.Empty))}
                Else
                    netTotal = {"Net Total", "", "", IIf(IsDBNull(productiondt.Compute("Sum(roomnights_old)", String.Empty)), "0", productiondt.Compute("Sum(roomnights_old)", String.Empty)),
                                                              IIf(IsDBNull(productiondt.Compute("Sum(roomnights)", String.Empty)), "0", productiondt.Compute("Sum(roomnights)", String.Empty))}
                End If
            ElseIf ddlColumnns.SelectedValue = "Units" Then
                If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Then
                    arrHeaders = {"Hotel", "Country Group", "Agents", "No.of Rooms", "No.of Rooms"}
                ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
                    arrHeaders = {"Agents", "Location", "Hotel", "No.of Rooms", "No.of Rooms"}
                ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
                    arrHeaders = {"Country Group", "Country", "Agents", "Hotel", "No.of Rooms", "No.of Rooms"}
                End If
                If Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
                    netTotal = {"Net Total", "", "", "", IIf(IsDBNull(productiondt.Compute("Sum(units_old)", String.Empty)), "0", productiondt.Compute("Sum(units_old)", String.Empty)),
                                                              IIf(IsDBNull(productiondt.Compute("Sum(units)", String.Empty)), "0", productiondt.Compute("Sum(units)", String.Empty))}
                Else
                    netTotal = {"Net Total", "", "", IIf(IsDBNull(productiondt.Compute("Sum(units_old)", String.Empty)), "0", productiondt.Compute("Sum(units_old)", String.Empty)),
                                                              IIf(IsDBNull(productiondt.Compute("Sum(units)", String.Empty)), "0", productiondt.Compute("Sum(units)", String.Empty))}
                End If
            ElseIf ddlColumnns.SelectedValue = "Pax" Then
                If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Then
                    arrHeaders = {"Hotel", "Country Group", "Agents", "Total Pax", "Total Pax"}
                ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
                    arrHeaders = {"Agents", "Location", "Hotel", "Total Pax", "Total Pax"}
                ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
                    arrHeaders = {"Country Group", "Country", "Agents", "Hotel", "Total Pax", "Total Pax"}
                End If
                If Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
                    netTotal = {"Net Total", "", "", "", IIf(IsDBNull(productiondt.Compute("Sum(pax_old)", String.Empty)), "0", productiondt.Compute("Sum(pax_old)", String.Empty)),
                                                              IIf(IsDBNull(productiondt.Compute("Sum(pax)", String.Empty)), "0", productiondt.Compute("Sum(pax)", String.Empty))}
                Else
                    netTotal = {"Net Total", "", "", IIf(IsDBNull(productiondt.Compute("Sum(pax_old)", String.Empty)), "0", productiondt.Compute("Sum(pax_old)", String.Empty)),
                                                              IIf(IsDBNull(productiondt.Compute("Sum(pax)", String.Empty)), "0", productiondt.Compute("Sum(pax)", String.Empty))}
                End If
            ElseIf ddlColumnns.SelectedValue = "SaleValue" Then
                If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Then
                    arrHeaders = {"Hotel", "Country Group", "Agents", "Sale Value", "Sale Value"}
                ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
                    arrHeaders = {"Agents", "Location", "Hotel", "Sale Value", "Sale Value"}
                ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
                    arrHeaders = {"Country Group", "Country", "Agents", "Hotel", "Sale Value", "Sale Value"}
                End If
                If Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
                    netTotal = {"Net Total", "", "", "", IIf(IsDBNull(productiondt.Compute("Sum(salevalue_old)", String.Empty)), "0", productiondt.Compute("Sum(salevalue_old)", String.Empty)),
                                                              IIf(IsDBNull(productiondt.Compute("Sum(salevalue)", String.Empty)), "0", productiondt.Compute("Sum(salevalue)", String.Empty))}
                Else
                    netTotal = {"Net Total", "", "", IIf(IsDBNull(productiondt.Compute("Sum(salevalue_old)", String.Empty)), "0", productiondt.Compute("Sum(salevalue_old)", String.Empty)),
                                                              IIf(IsDBNull(productiondt.Compute("Sum(salevalue)", String.Empty)), "0", productiondt.Compute("Sum(salevalue)", String.Empty))}
                End If
            End If

            If repfilter.Length > filterCount Then
                If repfilter.Length <= (filterCount + filterCount) Then
                    ws.Row(3).Height = 20
                ElseIf repfilter.Length <= (filterCount + filterCount + filterCount) Then
                    ws.Row(3).Height = 40
                ElseIf repfilter.Length <= (filterCount + filterCount + filterCount + filterCount) Then
                    ws.Row(3).Height = 60
                End If
            End If

            repfilter.Append(" Group By : " & ddlGroupBy.SelectedItem.Text)
            Dim filter = ws.Range(3, 1, 3, colcount).Merge()
            ws.Cell("A3").Value = repfilter
            filter.Style.Font.SetBold().Alignment.SetWrapText().Font.FontSize = 12
            filter.Style.Font.FontColor = XLColor.Black
            filter.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
            filter.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center

            Dim mfrmdt, mtodt, yfrmdt, dfrmdt, dtodt As String
            Dim pstart, pend, cstart, cend As Integer
            yfrmdt = Convert.ToDateTime(txtCmpFrmDt.Text).Year
            dfrmdt = Convert.ToDateTime(txtCmpFrmDt.Text).Day
            mfrmdt = Convert.ToDateTime(txtCmpFrmDt.Text).ToString("MMM")
            dtodt = Convert.ToDateTime(txtCmpToDt.Text).Day
            mtodt = Convert.ToDateTime(txtCmpToDt.Text).ToString("MMM")
            Dim tabletitle, title1
            If ddlColumnns.SelectedValue = "All" Then
                If Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
                    pstart = 5
                    pend = 16
                    cstart = 17
                    cend = 28
                Else
                    pstart = 4
                    pend = 15
                    cstart = 16
                    cend = 27
                End If
                tabletitle = ws.Range(rowcount, pstart, rowcount, pend).Merge()
                title1 = ws.Range(rowcount, cstart, rowcount, cend).Merge()
            Else
                If Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
                    pstart = 5
                    pend = 6
                    cstart = 7
                    cend = 8
                Else
                    pstart = 4
                    pend = 5
                    cstart = 6
                    cend = 7
                End If
                tabletitle = ws.Range(rowcount, pstart, rowcount, pstart).Merge()
                title1 = ws.Range(rowcount, pend, rowcount, pend).Merge()
            End If



            tabletitle.Value = yfrmdt & "    " & dfrmdt & " " & mfrmdt & " - " & dtodt & " " & mtodt
            tabletitle.Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
            tabletitle.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
            tabletitle.Style.Font.FontColor = XLColor.Red
            tabletitle.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True

            yfrmdt = Convert.ToDateTime(txtFromDt.Text).Year
            dfrmdt = Convert.ToDateTime(txtFromDt.Text).Day
            mfrmdt = Convert.ToDateTime(txtFromDt.Text).ToString("MMM")
            dtodt = Convert.ToDateTime(txtToDt.Text).Day
            mtodt = Convert.ToDateTime(txtToDt.Text).ToString("MMM")

            title1.Value = yfrmdt & "    " & dfrmdt & " " & mfrmdt & " - " & dtodt & " " & mtodt
            title1.Style.Font.SetBold().Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
            title1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left
            title1.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True


            rowcount = rowcount + 1
            ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Font.SetBold().Font.FontSize = 9
            ws.Range(rowcount, 1, rowcount, arrHeaders.Length).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            ws.Range(rowcount, 1, rowcount, pstart - 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
            ws.Range(rowcount, pstart, rowcount, cend).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            If ddlColumnns.SelectedValue = "All" Then
                ws.Range(rowcount, pstart, rowcount, pend).Style.Font.FontColor = XLColor.Red
            Else
                ws.Range(rowcount, pstart, rowcount, pstart).Style.Font.FontColor = XLColor.Red
            End If
            For i = 0 To arrHeaders.Length - 1
                ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
            Next
            Dim sumdata As DataTable
            Dim psale, csale As Integer
            psale = 10
            csale = 22
            Dim gp, gp1, adults, nbookings, nbookings_old, sale, cost, roomnights, sale_old, cost_old, ad_old, ch_old, rm_old, nofrm_old, nts_old, noofrooms, nights As String
            If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Then
                gp = "partyname"
            ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
                gp = "agentname"
            ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 6 Then
                gp = "countrygroupname"
                gp1 = "sourcectryname"
                psale = 11
                csale = 23
            End If
            If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Or Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
                Dim grpby = From grp In productiondt.AsEnumerable() Group grp By g = New With {Key .customer = grp.Field(Of String)(gp)} Into Group Order By g.customer
                For Each keyData In grpby
                    sumdata = keyData.Group.CopyToDataTable()
                    sale_old = Math.Round(sumdata.Compute("Sum(salevalue_old)", String.Empty), decno)
                    ad_old = Math.Round(sumdata.Compute("Sum(adults_old)", String.Empty), decno)
                    ch_old = sumdata.Compute("Sum(child_old)", String.Empty)
                    nofrm_old = sumdata.Compute("Sum(noofrooms_old)", String.Empty)
                    nts_old = sumdata.Compute("Sum(nights_old)", String.Empty)
                    rm_old = sumdata.Compute("Sum(roomnights_old)", String.Empty)
                    cost_old = sumdata.Compute("Sum(costvalue_old)", String.Empty)
                    sale = Math.Round(sumdata.Compute("Sum(salevalue)", String.Empty), decno)
                    adults = sumdata.Compute("Sum(adults)", String.Empty)
                    Dim ch = sumdata.AsEnumerable().Sum(Function(s) s.Field(Of Integer)("child"))
                    noofrooms = sumdata.Compute("Sum(noofrooms)", String.Empty)
                    nights = sumdata.Compute("Sum(nights)", String.Empty)
                    roomnights = sumdata.Compute("Sum(roomnights)", String.Empty)
                    cost = Math.Round(sumdata.Compute("Sum(costvalue)", String.Empty), decno)
                    nbookings = sumdata.Compute("Sum(noofbookings)", String.Empty)
                    nbookings_old = sumdata.Compute("Sum(noofbookings_old)", String.Empty)
                    Dim commisionpercent_old, commissionpercent, markuppercent_old, markuppercent As Decimal
                    If (cost_old = 0.0 Or sumdata.Compute("Sum(markupvalue_old)", String.Empty) = 0.0) Then
                        commisionpercent_old = " 0"
                    Else
                        commisionpercent_old = Convert.ToString(Math.Round(sumdata.Compute("Sum(commissionvalue_old)", String.Empty) / (cost_old + sumdata.Compute("Sum(commissionvalue_old)", String.Empty)) * 100, 8))
                    End If

                    If (cost = 0.0 Or sumdata.Compute("Sum(commissionvalue)", String.Empty) = 0.0) Then
                        commissionpercent = "0"
                    Else
                        commissionpercent = Convert.ToString(Math.Round(sumdata.Compute("Sum(commissionvalue)", String.Empty) / (cost + sumdata.Compute("Sum(commissionvalue)", String.Empty)) * 100, 8))
                    End If


                    If (sumdata.Compute("Sum(markuppercent_old)", String.Empty) = 0.0) Then
                        markuppercent_old = "0"
                    Else
                        markuppercent_old = Convert.ToString(Math.Round(sumdata.Compute("Sum(markupvalue_old)", String.Empty) / cost_old * 100, 8))
                    End If

                    If (sumdata.Compute("Sum(markuppercent)", String.Empty) = 0.0) Then
                        markuppercent = "0"
                    Else
                        markuppercent = Convert.ToString(Math.Round(sumdata.Compute("Sum(markupvalue)", String.Empty) / cost * 100, 8))
                    End If
                    If ddlColumnns.SelectedValue = "All" Then
                        'arrHeaders = {keyData.g.customer, "", "", nbookings_old, ad_old, ch_old, rm_old, sale_old, cost_old, Math.Round(sumdata.Compute("Sum(markuppercent_old)", String.Empty), decno), Math.Round(sumdata.Compute("Sum(markupvalue_old)", String.Empty), decno), , Math.Round(sumdata.Compute("Sum(commissionvalue_old)", String.Empty), decno),
                        '    nbookings, adults, ch, roomnights, sale, cost, Convert.ToString(Math.Round(sumdata.Compute("Sum(markuppercent)", String.Empty), decno)), Math.Round(sumdata.Compute("Sum(markupvalue)", String.Empty), decno), commissionpercent, Math.Round(sumdata.Compute("Sum(commissionvalue)", String.Empty), decno)}
                        arrHeaders = {keyData.g.customer, "", "", nbookings_old, ad_old, ch_old, nofrm_old, nts_old, rm_old, sale_old, cost_old, markuppercent_old, Math.Round(sumdata.Compute("Sum(markupvalue_old)", String.Empty), decno), commisionpercent_old, Math.Round(sumdata.Compute("Sum(commissionvalue_old)", String.Empty), decno),
                    nbookings, adults, ch, noofrooms, nights, roomnights, sale, cost, markuppercent, Math.Round(sumdata.Compute("Sum(markupvalue)", String.Empty), decno), commissionpercent, Math.Round(sumdata.Compute("Sum(commissionvalue)", String.Empty), decno)}


                    ElseIf ddlColumnns.SelectedValue = "RoomNights" Then
                        arrHeaders = {keyData.g.customer, "", "", rm_old, roomnights}
                    ElseIf ddlColumnns.SelectedValue = "Units" Then
                        arrHeaders = {keyData.g.customer, "", "", sumdata.Compute("Sum(units_old)", String.Empty), sumdata.Compute("Sum(units)", String.Empty)}
                    ElseIf ddlColumnns.SelectedValue = "Pax" Then
                        arrHeaders = {keyData.g.customer, "", "", sumdata.Compute("Sum(pax_old)", String.Empty), sumdata.Compute("Sum(pax)", String.Empty)}
                    ElseIf ddlColumnns.SelectedValue = "SaleValue" Then
                        arrHeaders = {keyData.g.customer, "", "", sale_old, sale}
                    End If


                    rowcount = rowcount + 1
                    Dim tableData = ws.Range(rowcount, 1, rowcount, arrHeaders.Length)
                    tableData.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Fill.SetBackgroundColor(XLColor.LightBlue).Font.FontSize = 9
                    tableData.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
                    ws.Range(rowcount, 1, rowcount, pstart - 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    ws.Range(rowcount, pstart, rowcount, cend).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    ws.Range(rowcount, psale, rowcount, psale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ws.Range(rowcount, psale + 3, rowcount, psale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ws.Range(rowcount, psale + 5, rowcount, psale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ws.Range(rowcount, csale, rowcount, csale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ws.Range(rowcount, csale + 3, rowcount, csale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ws.Range(rowcount, csale + 5, rowcount, csale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ws.Range(rowcount, 1, rowcount, 3).Merge()
                    ShowData(ws, arrHeaders, decPlaces, pstart, pend, psale, csale, rowcount)


                    For Each row In keyData.Group

                        If ddlColumnns.SelectedValue = "All" Then
                            If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Then
                                arrHeaders = {"", IIf(IsDBNull(row("countrygroupname")), "", row("countrygroupname")), IIf(IsDBNull(row("agentname")), "", row("agentname")), IIf(row("noofbookings_old") = 0, "", row("noofbookings_old")), IIf(row("adults_old") = 0, "", row("adults_old")), IIf(row("child_old") = 0, "", row("child_old")), IIf(row("noofrooms_old") = 0, "", row("noofrooms_old")), IIf(row("nights_old") = 0, "", row("nights_old")), IIf(row("roomnights_old") = 0, "", row("roomnights_old")), IIf(Math.Round(row("salevalue_old"), decno) = 0.0, "", Math.Round(row("salevalue_old"), decno)), IIf(Math.Round(row("costvalue_old"), decno) = 0.0, "", Math.Round(row("costvalue_old"), decno)),
                                              IIf(Math.Round(row("markuppercent_old"), 8) = 0.0, "", Convert.ToString(Math.Round(row("markuppercent_old"), 8))), IIf(Math.Round(row("markupvalue_old"), decno) = 0.0, "", Math.Round(row("markupvalue_old"), decno)), IIf(Math.Round(row("commissionpercent_old"), 8) = 0.0, "", Convert.ToString(Math.Round(row("commissionpercent_old"), 8))), IIf(Math.Round(row("commissionvalue_old"), decno) = 0.0, "", Math.Round(row("commissionvalue_old"), decno)),
                                              IIf(row("noofbookings") = 0, "", row("noofbookings")), IIf(row("adults") = 0, "", row("adults")), IIf(row("child") = 0, "", row("child")), IIf(row("noofrooms") = 0, "", row("noofrooms")), IIf(row("nights") = 0, "", row("nights")), IIf(row("roomnights") = 0, "", row("roomnights")), IIf(Math.Round(row("salevalue"), decno) = 0.0, "", Math.Round(row("salevalue"), decno)), IIf(Math.Round(row("costvalue"), decno) = 0.0, "", Math.Round(row("costvalue"), decno)),
                                                IIf(Math.Round(row("markuppercent"), 8) = 0.0, "", Convert.ToString(Math.Round(row("markuppercent"), 8))), IIf(Math.Round(row("markupvalue"), decno) = 0.0, "", Math.Round(row("markupvalue"), decno)), IIf(Math.Round(row("commissionpercent"), 8) = 0.0, "", Convert.ToString(Math.Round(row("commissionpercent"), 8))), IIf(Math.Round(row("commissionvalue"), decno) = 0.0, "", Math.Round(row("commissionvalue"), decno))}


                            ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
                                arrHeaders = {"", IIf(IsDBNull(row("hotelcityname")), "", row("hotelcityname")), IIf(IsDBNull(row("partyname")), "", row("partyname")), IIf(row("noofbookings_old") = 0, "", row("noofbookings_old")), IIf(row("adults_old") = 0, "", row("adults_old")), IIf(row("child_old") = 0, "", row("child_old")), IIf(row("noofrooms_old") = 0, "", row("noofrooms_old")), IIf(row("nights_old") = 0, "", row("nights_old")), IIf(row("roomnights_old") = 0, "", row("roomnights_old")), IIf(Math.Round(row("salevalue_old"), decno) = 0.0, "", Math.Round(row("salevalue_old"), decno)), IIf(Math.Round(row("costvalue_old"), decno) = 0.0, "", Math.Round(row("costvalue_old"), decno)),
                                              IIf(Math.Round(row("markuppercent_old"), 8) = 0.0, "", Convert.ToString(Math.Round(row("markuppercent_old"), 8))), IIf(Math.Round(row("markupvalue_old"), decno) = 0.0, "", Math.Round(row("markupvalue_old"), decno)), IIf(Math.Round(row("commissionpercent_old"), 8) = 0.0, "", Convert.ToString(Math.Round(row("commissionpercent_old"), 8))), IIf(Math.Round(row("commissionvalue_old"), decno) = 0.0, "", Math.Round(row("commissionvalue_old"), decno)),
                                               IIf(row("noofbookings") = 0, "", row("noofbookings")), IIf(row("adults") = 0, "", row("adults")), IIf(row("child") = 0, "", row("child")), IIf(row("noofrooms") = 0, "", row("noofrooms")), IIf(row("nights") = 0, "", row("nights")), IIf(row("roomnights") = 0, "", row("roomnights")), IIf(Math.Round(row("salevalue"), decno) = 0.0, "", Math.Round(row("salevalue"), decno)), IIf(Math.Round(row("costvalue"), decno) = 0.0, "", Math.Round(row("costvalue"), decno)),
                                                IIf(Math.Round(row("markuppercent"), 8) = 0.0, "", Convert.ToString(Math.Round(row("markuppercent"), 8))), IIf(Math.Round(row("markupvalue"), decno) = 0.0, "", Math.Round(row("markupvalue"), decno)), IIf(Math.Round(row("commissionpercent"), 8) = 0.0, "", Convert.ToString(Math.Round(row("commissionpercent"), 8))), IIf(Math.Round(row("commissionvalue"), decno) = 0.0, "", Math.Round(row("commissionvalue"), decno))}

                            End If
                        ElseIf ddlColumnns.SelectedValue = "RoomNights" Then
                            If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Then
                                arrHeaders = {"", IIf(IsDBNull(row("countrygroupname")), "", row("countrygroupname")), IIf(IsDBNull(row("agentname")), "", row("agentname")), IIf(row("roomnights_old") = 0, "", row("roomnights_old")), IIf(row("roomnights") = 0, "", row("roomnights"))}
                            ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
                                arrHeaders = {"", IIf(IsDBNull(row("hotelcityname")), "", row("hotelcityname")), IIf(IsDBNull(row("partyname")), "", row("partyname")), IIf(row("roomnights_old") = 0, "", row("roomnights_old")), IIf(row("roomnights") = 0, "", row("roomnights"))}
                            End If

                        ElseIf ddlColumnns.SelectedValue = "Units" Then
                            If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Then
                                arrHeaders = {"", IIf(IsDBNull(row("countrygroupname")), "", row("countrygroupname")), IIf(IsDBNull(row("agentname")), "", row("agentname")), IIf(row("units_old") = 0, "", row("units_old")), IIf(row("units") = 0, "", row("units"))}
                            ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
                                arrHeaders = {"", IIf(IsDBNull(row("hotelcityname")), "", row("hotelcityname")), IIf(IsDBNull(row("partyname")), "", row("partyname")), IIf(row("units_old") = 0, "", row("units_old")), IIf(row("units") = 0, "", row("units"))}
                            End If
                        ElseIf ddlColumnns.SelectedValue = "Pax" Then
                            If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Then
                                arrHeaders = {"", IIf(IsDBNull(row("countrygroupname")), "", row("countrygroupname")), IIf(IsDBNull(row("agentname")), "", row("agentname")), IIf(row("pax_old") = 0, "", row("pax_old")), IIf(row("pax") = 0, "", row("pax"))}
                            ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
                                arrHeaders = {"", IIf(IsDBNull(row("hotelcityname")), "", row("hotelcityname")), IIf(IsDBNull(row("partyname")), "", row("partyname")), IIf(row("pax_old") = 0, "", row("pax_old")), IIf(row("pax") = 0, "", row("pax"))}
                            End If
                        ElseIf ddlColumnns.SelectedValue = "SaleValue" Then
                            If Convert.ToInt32(ddlGroupBy.SelectedValue) = 1 Then
                                arrHeaders = {"", IIf(IsDBNull(row("countrygroupname")), "", row("countrygroupname")), IIf(IsDBNull(row("agentname")), "", row("agentname")), IIf(Math.Round(row("salevalue_old"), decno) = 0.0, "", Math.Round(row("salevalue_old"), decno)), IIf(Math.Round(row("salevalue"), decno) = 0.0, "", Math.Round(row("salevalue"), decno))}
                            ElseIf Convert.ToInt32(ddlGroupBy.SelectedValue) = 2 Then
                                arrHeaders = {"", IIf(IsDBNull(row("hotelcityname")), "", row("hotelcityname")), IIf(IsDBNull(row("partyname")), "", row("partyname")), IIf(Math.Round(row("salevalue_old"), decno) = 0.0, "", Math.Round(row("salevalue_old"), decno)), IIf(Math.Round(row("salevalue"), decno) = 0.0, "", Math.Round(row("salevalue"), decno))}
                            End If
                        End If
                        rowcount += 1
                        tableData = ws.Range(rowcount, 1, rowcount, arrHeaders.Length)
                        tableData.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
                        tableData.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                        ws.Range(rowcount, 1, rowcount, pstart - 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        ws.Range(rowcount, pstart, rowcount, cend).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        ws.Range(rowcount, psale, rowcount, psale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ws.Range(rowcount, psale + 3, rowcount, psale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ws.Range(rowcount, psale + 5, rowcount, psale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ws.Range(rowcount, csale, rowcount, csale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ws.Range(rowcount, csale + 3, rowcount, csale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ws.Range(rowcount, csale + 5, rowcount, csale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ShowData(ws, arrHeaders, decPlaces, pstart, pend, psale, csale, rowcount)
                    Next
                Next
            Else
                ' keyData.g.srctry for group  by country group
                Dim grpby = From grp In productiondt.AsEnumerable() Group grp By g = New With {Key .customer = grp.Field(Of String)(gp), .srctry = grp.Field(Of String)(gp1)} Into Group Order By g.customer
                For Each keyData In grpby
                    sumdata = keyData.Group.CopyToDataTable()
                    Dim commisionpercent_old, commissionpercent, markuppercent_old, markuppercent, costvalue_old, costvalue As Decimal
                    costvalue_old = sumdata.Compute("Sum(costvalue_old)", String.Empty)
                    costvalue = sumdata.Compute("Sum(costvalue)", String.Empty)
                    If (costvalue_old = 0.0 Or sumdata.Compute("Sum(markupvalue_old)", String.Empty) = 0.0) Then
                        commisionpercent_old = " 0"
                    Else
                        commisionpercent_old = Convert.ToString(Math.Round(sumdata.Compute("Sum(commissionvalue_old)", String.Empty) / (costvalue_old + sumdata.Compute("Sum(commissionvalue_old)", String.Empty)) * 100, 8))
                    End If

                    If (costvalue = 0.0 Or sumdata.Compute("Sum(commissionvalue)", String.Empty) = 0.0) Then
                        commissionpercent = "0"
                    Else
                        commissionpercent = Convert.ToString(Math.Round(sumdata.Compute("Sum(commissionvalue)", String.Empty) / (costvalue + sumdata.Compute("Sum(commissionvalue)", String.Empty)) * 100, 8))
                    End If


                    If (sumdata.Compute("Sum(markuppercent_old)", String.Empty) = 0.0) Then
                        markuppercent_old = "0"
                    Else
                        markuppercent_old = Convert.ToString(Math.Round(sumdata.Compute("Sum(markupvalue_old)", String.Empty) / costvalue_old * 100, 8))
                    End If

                    If (sumdata.Compute("Sum(markuppercent)", String.Empty) = 0.0) Then
                        markuppercent = "0"
                    Else
                        markuppercent = Convert.ToString(Math.Round(sumdata.Compute("Sum(markupvalue)", String.Empty) / costvalue * 100, 8))
                    End If
                    nofrm_old = sumdata.Compute("Sum(noofrooms_old)", String.Empty)
                    nts_old = sumdata.Compute("Sum(nights_old)", String.Empty)
                    noofrooms = sumdata.Compute("Sum(noofrooms)", String.Empty)
                    nights = sumdata.Compute("Sum(nights)", String.Empty)
                    If ddlColumnns.SelectedValue = "All" Then
                        arrHeaders = {keyData.g.customer, keyData.g.srctry, "", "", IIf(IsDBNull(sumdata.Compute("Sum(noofbookings_old)", String.Empty)), "0", sumdata.Compute("Sum(noofbookings_old)", String.Empty)), IIf(IsDBNull(sumdata.Compute("Sum(adults_old)", String.Empty)), "0", sumdata.Compute("Sum(adults_old)", String.Empty)), IIf(IsDBNull(sumdata.Compute("Sum(child_old)", String.Empty)), "0", sumdata.Compute("Sum(child_old)", String.Empty)), IIf(IsDBNull(nofrm_old), "0", nofrm_old), IIf(IsDBNull(nts_old), "0", nts_old), IIf(IsDBNull(sumdata.Compute("Sum(roomnights_old)", String.Empty)), "0", sumdata.Compute("Sum(roomnights_old)", String.Empty)),
                                                       IIf(IsDBNull(sumdata.Compute("Sum(salevalue_old)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(salevalue_old)", String.Empty), decno)), IIf(IsDBNull(sumdata.Compute("Sum(costvalue_old)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(costvalue_old)", String.Empty), decno)),
                                    markuppercent_old, IIf(IsDBNull(sumdata.Compute("Sum(markupvalue_old)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(markupvalue_old)", String.Empty), decno)), commisionpercent_old, IIf(IsDBNull(sumdata.Compute("Sum(commissionvalue_old)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(commissionvalue_old)", String.Empty), decno)),
                                                         IIf(IsDBNull(sumdata.Compute("Sum(noofbookings)", String.Empty)), "0", sumdata.Compute("Sum(noofbookings)", String.Empty)), IIf(IsDBNull(sumdata.Compute("Sum(adults)", String.Empty)), "0", sumdata.Compute("Sum(adults)", String.Empty)), sumdata.AsEnumerable().Sum(Function(s) s.Field(Of Integer)("child")), IIf(IsDBNull(noofrooms), "0", noofrooms), IIf(IsDBNull(nights), "0", nights), IIf(IsDBNull(sumdata.Compute("Sum(roomnights)", String.Empty)), "0", sumdata.Compute("Sum(roomnights)", String.Empty)), IIf(IsDBNull(sumdata.Compute("Sum(salevalue)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(salevalue)", String.Empty), decno)),
                                                     IIf(IsDBNull(sumdata.Compute("Sum(costvalue)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(costvalue)", String.Empty), decno)),
                                    markuppercent, IIf(IsDBNull(sumdata.Compute("Sum(markupvalue)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(markupvalue)", String.Empty), decno)), commissionpercent, IIf(IsDBNull(sumdata.Compute("Sum(commissionvalue)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(commissionvalue)", String.Empty), decno))}
                    ElseIf ddlColumnns.SelectedValue = "RoomNights" Then
                        arrHeaders = {keyData.g.customer, keyData.g.srctry, "", "", IIf(IsDBNull(sumdata.Compute("Sum(roomnights_old)", String.Empty)), "0", sumdata.Compute("Sum(roomnights_old)", String.Empty)),
                                       IIf(IsDBNull(sumdata.Compute("Sum(roomnights)", String.Empty)), "0", sumdata.Compute("Sum(roomnights)", String.Empty))}
                    ElseIf ddlColumnns.SelectedValue = "Units" Then
                        arrHeaders = {keyData.g.customer, keyData.g.srctry, "", "", IIf(IsDBNull(sumdata.Compute("Sum(units_old)", String.Empty)), "0", sumdata.Compute("Sum(units_old)", String.Empty)),
                                                              IIf(IsDBNull(sumdata.Compute("Sum(units)", String.Empty)), "0", sumdata.Compute("Sum(units)", String.Empty))}
                    ElseIf ddlColumnns.SelectedValue = "Pax" Then
                        arrHeaders = {keyData.g.customer, keyData.g.srctry, "", "", IIf(IsDBNull(sumdata.Compute("Sum(pax_old)", String.Empty)), "0", sumdata.Compute("Sum(pax_old)", String.Empty)),
                                      IIf(IsDBNull(sumdata.Compute("Sum(pax)", String.Empty)), "0", sumdata.Compute("Sum(pax)", String.Empty))}
                    ElseIf ddlColumnns.SelectedValue = "SaleValue" Then
                        arrHeaders = {keyData.g.customer, keyData.g.srctry, "", "", IIf(IsDBNull(sumdata.Compute("Sum(salevalue_old)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(salevalue_old)", String.Empty), decno)),
                                        IIf(IsDBNull(sumdata.Compute("Sum(salevalue)", String.Empty)), "0", Math.Round(sumdata.Compute("Sum(salevalue)", String.Empty), decno))}
                    End If


                    rowcount = rowcount + 1
                    Dim tableData = ws.Range(rowcount, 1, rowcount, arrHeaders.Length)
                    tableData.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetBold().Font.FontSize = 9
                    tableData.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                    ws.Range(rowcount, 1, rowcount, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    ws.Range(rowcount, 5, rowcount, arrHeaders.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    ws.Range(rowcount, psale, rowcount, psale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ws.Range(rowcount, psale + 3, rowcount, psale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ws.Range(rowcount, psale + 5, rowcount, psale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ws.Range(rowcount, csale, rowcount, csale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ws.Range(rowcount, csale + 3, rowcount, csale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ws.Range(rowcount, csale + 5, rowcount, csale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                    ShowData(ws, arrHeaders, decPlaces, pstart, pend, psale, csale, rowcount)


                    Dim grpbyAgent = From grp In sumdata.AsEnumerable() Group grp By g = New With {Key .agent = grp.Field(Of String)("agentname")} Into Group Order By g.agent

                    For Each agent In grpbyAgent
                        rowcount += 1
                        If ddlColumnns.SelectedValue = "All" Then
                            arrHeaders = {"", "", agent.g.agent, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""}
                        ElseIf ddlColumnns.SelectedValue = "RoomNights" Then
                            arrHeaders = {"", "", agent.g.agent, "", "", ""}
                        ElseIf ddlColumnns.SelectedValue = "Units" Then
                            arrHeaders = {"", "", agent.g.agent, "", "", ""}
                        ElseIf ddlColumnns.SelectedValue = "Pax" Then
                            arrHeaders = {"", "", agent.g.agent, "", "", ""}
                        ElseIf ddlColumnns.SelectedValue = "SaleValue" Then
                            arrHeaders = {"", "", agent.g.agent, "", "", ""}
                        End If

                        tableData = ws.Range(rowcount, 1, rowcount, arrHeaders.Length)
                        tableData.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Fill.SetBackgroundColor(XLColor.LightBlue).Font.FontSize = 9
                        tableData.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                        ws.Range(rowcount, 1, rowcount, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        ws.Range(rowcount, 5, rowcount, arrHeaders.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                        ws.Range(rowcount, psale, rowcount, psale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ws.Range(rowcount, psale + 3, rowcount, psale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ws.Range(rowcount, psale + 5, rowcount, psale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ws.Range(rowcount, csale, rowcount, csale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ws.Range(rowcount, csale + 3, rowcount, csale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ws.Range(rowcount, csale + 5, rowcount, csale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                        ShowData(ws, arrHeaders, decPlaces, pstart, pend, psale, csale, rowcount)


                        For Each row In agent.Group
                            rowcount += 1
                            If ddlColumnns.SelectedValue = "All" Then
                                arrHeaders = {"", "", "", IIf(IsDBNull(row("partyname")), "", row("partyname")), IIf(row("noofbookings_old") = 0, "", row("noofbookings_old")), IIf(row("adults_old") = 0, "", row("adults_old")), IIf(row("child_old") = 0, "", row("child_old")), IIf(row("noofrooms_old") = 0, "", row("noofrooms_old")), IIf(row("nights_old") = 0, "", row("nights_old")), IIf(row("roomnights_old") = 0, "", row("roomnights_old")), IIf(Math.Round(row("salevalue_old"), decno) = 0.0, "", Math.Round(row("salevalue_old"), decno)), IIf(Math.Round(row("costvalue_old"), decno) = 0.0, "", Math.Round(row("costvalue_old"), decno)),
                                          IIf(Math.Round(row("markuppercent_old"), 8) = 0.0, "", Convert.ToString(Math.Round(row("markuppercent_old"), 8))), IIf(Math.Round(row("markupvalue_old"), decno) = 0.0, "", Math.Round(row("markupvalue_old"), decno)), IIf(Math.Round(row("commissionpercent_old"), decno) = 0.0, "", Convert.ToString(Math.Round(row("commissionpercent_old"), decno))), IIf(Math.Round(row("commissionvalue_old"), decno) = 0.0, "", Math.Round(row("commissionvalue_old"), decno)),
                                          IIf(row("noofbookings_old") = 0, "", row("noofbookings_old")), IIf(row("adults") = 0, "", row("adults")), IIf(row("child") = 0, "", row("child")), IIf(row("noofrooms") = 0, "", row("noofrooms")), IIf(row("nights") = 0, "", row("nights")), IIf(row("roomnights") = 0, "", row("roomnights")), IIf(Math.Round(row("salevalue"), decno) = 0.0, "", Math.Round(row("salevalue"), decno)), IIf(Math.Round(row("costvalue"), decno) = 0.0, "", Math.Round(row("costvalue"), decno)),
                                          IIf(Math.Round(row("markuppercent"), 8) = 0.0, "", Convert.ToString(Math.Round(row("markuppercent"), 8))), IIf(Math.Round(row("markupvalue"), decno) = 0.0, "", Math.Round(row("markupvalue"), decno)), IIf(Math.Round(row("commissionpercent"), decno) = 0.0, "", Convert.ToString(Math.Round(row("commissionpercent"), decno))), IIf(Math.Round(row("commissionvalue"), decno) = 0.0, "", Math.Round(row("commissionvalue"), decno))}
                            ElseIf ddlColumnns.SelectedValue = "RoomNights" Then
                                arrHeaders = {"", "", "", IIf(IsDBNull(row("partyname")), "", row("partyname")), IIf(row("roomnights_old") = 0, "", row("roomnights_old")), IIf(row("roomnights") = 0, "", row("roomnights"))}
                            ElseIf ddlColumnns.SelectedValue = "Units" Then
                                arrHeaders = {"", "", "", IIf(IsDBNull(row("partyname")), "", row("partyname")), IIf(row("units_old") = 0, "", row("units_old")), IIf(row("units") = 0, "", row("units"))}
                            ElseIf ddlColumnns.SelectedValue = "Pax" Then
                                arrHeaders = {"", "", "", IIf(IsDBNull(row("partyname")), "", row("partyname")), IIf(row("pax_old") = 0, "", row("pax_old")), IIf(row("pax") = 0, "", row("pax"))}
                            ElseIf ddlColumnns.SelectedValue = "SaleValue" Then
                                arrHeaders = {"", "", "", IIf(IsDBNull(row("partyname")), "", row("partyname")), IIf(Math.Round(row("salevalue_old"), decno) = 0.0, "", Math.Round(row("salevalue_old"), decno)), IIf(Math.Round(row("salevalue"), decno) = 0.0, "", Math.Round(row("salevalue"), decno))}
                            End If

                            tableData = ws.Range(rowcount, 1, rowcount, arrHeaders.Length)
                            tableData.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.FontSize = 9
                            tableData.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin).Alignment.WrapText = True
                            ws.Range(rowcount, 1, rowcount, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                            ws.Range(rowcount, 5, rowcount, arrHeaders.Length).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                            ws.Range(rowcount, psale, rowcount, psale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                            ws.Range(rowcount, psale + 3, rowcount, psale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                            ws.Range(rowcount, psale + 5, rowcount, psale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                            ws.Range(rowcount, csale, rowcount, csale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                            ws.Range(rowcount, csale + 3, rowcount, csale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                            ws.Range(rowcount, csale + 5, rowcount, csale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                            ShowData(ws, arrHeaders, decPlaces, pstart, pend, psale, csale, rowcount)

                        Next
                    Next
                Next
            End If
            rowcount += 1
            Dim Data = ws.Range(rowcount, 1, rowcount, netTotal.Length)
            Data.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetBold().Fill.SetBackgroundColor(XLColor.LightGray).Font.FontSize = 9
            Data.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetTopBorder(XLBorderStyleValues.Thin).Border.SetLeftBorder(XLBorderStyleValues.Thin).Border.SetRightBorder(XLBorderStyleValues.Thin)
            ws.Range(rowcount, 1, rowcount, pstart - 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
            ws.Range(rowcount, 1, rowcount, pstart - 1).Merge()
            ws.Range(rowcount, pstart, rowcount, cend).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            ws.Range(rowcount, psale, rowcount, psale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
            ws.Range(rowcount, psale + 3, rowcount, psale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
            ws.Range(rowcount, psale + 5, rowcount, psale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
            ws.Range(rowcount, csale, rowcount, csale + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
            ws.Range(rowcount, csale + 3, rowcount, csale + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
            ws.Range(rowcount, csale + 5, rowcount, csale + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
            ShowData(ws, netTotal, decPlaces, pstart, pend, psale, csale, rowcount)


            ws.Cell((rowcount + 4), 1).Value = "Printed Date : " & Now.ToString("dd/MM/yyyy")
            ws.Range((rowcount + 4), 1, (rowcount + 4), 3).Merge()
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                wb.Dispose()
                Response.Clear()
                Response.Buffer = True
                Response.AddHeader("content-disposition", "attachment;filename=Comparative" & Now.ToString("dd/MM/yyyy") & ".xlsx")
                Response.AddHeader("Content-Length", MyMemoryStream.Length.ToString())
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"

                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                HttpContext.Current.ApplicationInstance.CompleteRequest()

            End Using
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Records not found, Please redefine search criteria');", True)
        End If
    End Sub
#End Region

    Private Sub ShowData(ByRef ws As IXLWorksheet, ByVal arrHeaders() As String, ByVal decPlaces As String, ByVal pstart As Integer, ByVal pend As Integer, ByVal psale As Integer, ByVal csale As Integer, ByRef rowcount As Integer)
        If ddlColumnns.SelectedValue = "All" Then
            ws.Range(rowcount, pstart, rowcount, pend).Style.Font.FontColor = XLColor.Red
        Else
            ws.Range(rowcount, pstart, rowcount, pstart).Style.Font.FontColor = XLColor.Red
        End If
        For i = 0 To arrHeaders.Length - 1
            If ddlColumnns.SelectedValue = "All" Then
                '  ws.Columns("T").Style.NumberFormat.Format = 
                If (i = psale - 1 Or i = psale Or i = psale + 1 Or i = psale + 3 Or i = psale + 2 Or i = psale + 4 Or i = csale - 1 Or i = csale Or i = csale + 1 Or i = csale + 3 Or i = csale + 2 Or i = csale + 4) And arrHeaders(i) <> "" Then
                    ws.Cell(rowcount, i + 1).Value = Convert.ToDecimal(Decimal.Parse(arrHeaders(i)))
                    If i = psale + 1 Or i = psale + 3 Or i = csale + 1 Or i = csale + 3 Then
                        ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = "#,##0.00\%"
                        'ws.Cell(rowcount, i + 1).Style.NumberFormat.NumberFormatId = 10

                    Else
                        ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = decPlaces
                    End If
                Else
                    ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
                End If
            ElseIf ddlColumnns.SelectedValue = "SaleValue" Then
                If (i = pstart - 1 Or i = pstart) And arrHeaders(i) <> "" Then
                    ws.Cell(rowcount, i + 1).Value = Convert.ToDecimal(Decimal.Parse(arrHeaders(i)))
                    ws.Cell(rowcount, i + 1).Style.NumberFormat.Format = decPlaces
                Else
                    ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
                End If
            Else
                ws.Cell(rowcount, i + 1).Value = arrHeaders(i)
            End If
        Next
    End Sub

    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        'newExcelProdComparison()
        ExcelProdComparison()
    End Sub
End Class


