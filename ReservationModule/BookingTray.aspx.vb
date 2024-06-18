Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Linq
Imports ClosedXML.Excel
Imports System.IO
Imports System.Web.Services

Partial Class BookingTray
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
    Dim document As New XLWorkbook
#End Region

#Region "Web Methods"

    <System.Web.Script.Services.ScriptMethod()> _
 <System.Web.Services.WebMethod()> _
    Public Shared Function GetDestinationList(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Hotelnames As New List(Of String)
        Try

            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select destcode,destname,desttype from view_destination_search(nolock) where destname like  '%" & prefixText & "%' "
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")   'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Hotelnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("destname").ToString(), myDS.Tables(0).Rows(i)("destcode").ToString() + "|" + myDS.Tables(0).Rows(i)("desttype").ToString()))
                Next
            End If

            Return Hotelnames
        Catch ex As Exception
            Return Hotelnames
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function GetHotelName(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Hotelnames As New List(Of String)
        Try

            Dim strDest As String = ""
            Dim strStar As String = ""
            Dim strPropType As String = ""
            If prefixText = " " Then
                prefixText = ""
            End If
            If contextKey <> "" Then
                Dim strContext As String()
                strContext = contextKey.Trim.Split("||")
                For i As Integer = 0 To strContext.Length - 1
                    If strContext(i).Contains("DC:") Then
                        strDest = strContext(i).Replace("DC:", "")
                    End If

                Next


            End If

            Dim str As String = contextKey
            strSqlQry = "select v.partycode,v.partyname from sectormaster(nolock) s,catmast(nolock) c,view_approved_hotels_new v where v.sectorcode=s.sectorcode  and v.catcode=c.catcode   and v.partyname like  '%" & prefixText & "%' "
            If strDest.Trim <> "" Then
                strSqlQry = strSqlQry & " and (v.citycode = '" & strDest.Trim & "' or s.sectorcode = '" & strDest.Trim & "')  "
            End If
            strSqlQry = strSqlQry & " order by v.partyname  "

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Hotelnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))
                Next
            End If

            Return Hotelnames
        Catch ex As Exception
            Return Hotelnames
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function GetCountry(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Hotelnames As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select a.ctrycode,c.ctryname from agentmast_countries(nolock) a,ctrymast(nolock) c where a.ctrycode=c.ctrycode and a.agentcode= '" & contextKey.Trim & "' and ctryname like  '" & prefixText & "%'  order by ctryname"

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Hotelnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("ctryname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))
                Next

            End If

            Return Hotelnames
        Catch ex As Exception
            Return Hotelnames
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function GetCountryDetails(ByVal CustCode As String) As String

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Hotelnames As New List(Of String)
        Try

            strSqlQry = "select a.ctrycode,c.ctryname from agentmast_countries(nolock) a,ctrymast c where a.ctrycode=c.ctrycode and a.agentcode= '" & CustCode.Trim & "'  order by ctryname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS, "Countries")

            Return myDS.GetXml()
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetCustomers(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Hotelnames As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If

            'If HttpContext.Current.Session("sAgentCompany") = "924065660726315" Then 'AgentsOnlineCommon
            If contextKey = "All" Then
                strSqlQry = "select agentcode,agentname from agentmast a(nolock) inner join division_master d on a.divcode=d.division_master_code " &
                "where active=1 and agentname like '" & prefixText & "%' order by agentname"
            Else
                strSqlQry = "select agentcode,agentname from agentmast(nolock) where active=1 and divcode='" & contextKey & "' and agentname like '" & prefixText & "%' order by agentname"
            End If
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")    'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Hotelnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("agentcode").ToString()))
                Next

            End If

            Return Hotelnames
        Catch ex As Exception
            Return Hotelnames
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetRODetails(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim UserName As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select UserName,usercode from UserMaster(nolock) where active=1 and  UserName like  '" & prefixText & "%'  order by UserName "

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    UserName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("UserName").ToString(), myDS.Tables(0).Rows(i)("usercode").ToString()))
                Next

            End If

            Return UserName
        Catch ex As Exception
            Return UserName
        End Try

    End Function

#End Region

#Region "Enum GridCol"
    Enum GridCol
        slNo = 0
        bookingNo = 1
        bookingDate = 2
        arrivalDate = 3
        customerName = 4
        customerRef = 5
        guestName = 6
        bookingStatus = 7
        salesAmount = 8
        lastModifiedDate = 9
        lastModifiedBy = 10
        invoiceNo = 11
        print = 12
    End Enum
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
                Dim AppId As String = CType(Request.QueryString("appid"), String)

                If AppId Is Nothing = False Then
                    strappid = AppId
                End If
                strappname = objUser.GetAppName(Session("dbconnectionName"), strappid)

                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "ReservationModule\BookingTray.aspx?appid=" + strappid, btnAddNew, btnLoadReport, _
                                                       btnPrint, gvSearch:=gvSearchResult, PrintColumnNo:=GridCol.print)
                'If gvSearchResult.Columns(12).Visible Then
                '    gvSearchResult.Columns(13).Visible = True
                'Else
                '    gvSearchResult.Columns(13).Visible = False
                'End If
                btnExportToExcel.Visible = False

                ddlTravelDate.SelectedIndex = 0
                txtTravelFromDate.Text = ""
                dvTravelFromDate.Visible = False
                txtTravelToDate.Text = ""
                dvTravelToDate.Visible = False

                ddlBookingDate.SelectedIndex = 0
                txtBookingFromDate.Text = ""
                dvBookingFromDate.Visible = False
                txtBookingToDate.Text = ""
                dvBookingToDate.Visible = False

                Dim baseCurrency As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='457'")
                gvSearchResult.Columns(11).HeaderText = "Sales Amount (" + baseCurrency + ")"
                Session.Add("baseCurrency", baseCurrency)

                Dim decimalPlaces As Integer = Convert.ToInt32(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='509'"))
                Session.Add("decimalPlaces", decimalPlaces)

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

                RowsPerPageCUS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                Session.Add("strsortExpression", "requestId")
                Session.Add("strsortdirection", SortDirection.Descending)

                Dim dt As New DataTable
                dt.Columns.Add(New DataColumn("sequenceNo", GetType(Integer)))
                dt.Columns.Add(New DataColumn("requestId", GetType(String)))
                dt.Columns.Add(New DataColumn("requestDate", GetType(DateAndTime)))
                dt.Columns.Add(New DataColumn("checkin", GetType(DateAndTime)))
                dt.Columns.Add(New DataColumn("agentname", GetType(String)))
                dt.Columns.Add(New DataColumn("agentref", GetType(String)))
                dt.Columns.Add(New DataColumn("guestname", GetType(String)))
                dt.Columns.Add(New DataColumn("bookingstatus", GetType(String)))
                dt.Columns.Add(New DataColumn("salevaluebase", GetType(Decimal)))
                dt.Columns.Add(New DataColumn("createddate", GetType(DateAndTime)))
                dt.Columns.Add(New DataColumn("createdby", GetType(String)))
                dt.Columns.Add(New DataColumn("lastmodified", GetType(DateAndTime)))
                dt.Columns.Add(New DataColumn("modifiedby", GetType(String)))
                dt.Columns.Add(New DataColumn("invoiceno", GetType(String)))
                Session("bookingSearchResult") = dt
                gvSearchResult.DataSource = dt
                gvSearchResult.DataBind()

                gvSearchResult.Columns(12).Visible = False

                dvTransferType.Visible = False
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BookingTray.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Function Validation() As Boolean"
    Protected Function Validation() As Boolean
        If ddlTravelDate.SelectedValue = "Specific date" Or ddlTravelDate.SelectedValue = "Check In or Check Out" Or ddlTravelDate.SelectedValue = "Checkin date" Or ddlTravelDate.SelectedValue = "Checkout date" Then
            If Not IsDate(txtTravelFromDate.Text.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Check travel from date');", True)
                Validation = False
                Exit Function
            End If

            If Not IsDate(txtTravelToDate.Text.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Check travel to date');", True)
                Validation = False
                Exit Function
            End If
        End If
        If ddlBookingDate.SelectedValue = "Specific date" Then
            If Not IsDate(txtBookingFromDate.Text.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Check booking from date');", True)
                Validation = False
                Exit Function
            End If

            If Not IsDate(txtBookingToDate.Text.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Check booking to date');", True)
                Validation = False
                Exit Function
            End If
        End If
        Validation = True
    End Function
#End Region

#Region "Private Sub FillGridNew()"
    Private Sub FillGridNew()
        Try






            If Validation() = False Then Exit Sub
            Dim divcode As String = ""
            If ddlDivision.SelectedIndex = 0 Then
                For i = 1 To ddlDivision.Items.Count - 1
                    If divcode = "" Then
                        divcode = "'" + ddlDivision.Items(i).Value + "'"
                    Else
                        divcode = divcode + ",'" + ddlDivision.Items(i).Value + "'"
                    End If
                Next
            Else
                divcode = "'" + ddlDivision.SelectedValue + "'"
            End If
            Dim companyCode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select stuff((select ',' + convert(varchar(30),randomnumber) " &
            "from agentmast_whitelabel a,agentmast am where a.agentcode=am.agentcode and a.owncompany=1 and " &
            "am.divcode in (" + divcode + ") for xml path ('')),1,1,'') as companycode")

            Dim pagevaluecus = RowsPerPageCUS.SelectedValue
            lblMsg.Visible = False
            If gvSearchResult.PageIndex < 0 Then gvSearchResult.PageIndex = 0
            Dim myDS As New DataSet
            If Not Session("sDsBooking") Is Nothing Then

                myDS = Session("sDsBooking")
                If myDS.Tables(0).Rows.Count > 0 Then
                    gvSearchResult.DataSource = myDS.Tables(0)
                    gvSearchResult.PageSize = pagevaluecus
                    gvSearchResult.DataBind()
                    ModalPopupLoading.Hide()
                Else
                    ModalPopupLoading.Hide()
                    gvSearchResult.PageIndex = 0
                    gvSearchResult.DataBind()
                    lblMsg.Visible = True
                    lblMsg.Text = "Records not found, Please redefine search criteria."
                End If
            Else





                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myCommand = New SqlCommand("sp_booking_search", SqlConn)
                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.CommandTimeout = 0
                myCommand.Parameters.Add(New SqlParameter("@logintype", SqlDbType.VarChar, 20)).Value = "RO"
                myCommand.Parameters.Add(New SqlParameter("@webusername", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                myCommand.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = txtCustomerCode.Text.Trim
                myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = txtBookingRef.Text.Trim
                myCommand.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar, 50)).Value = ddlServiceType.SelectedValue
                Dim DestinationCodeAndType As String = txtDestinationCode.Text
                Dim strDest As String() = txtDestinationCode.Text.Split("|")
                If strDest.Length = 2 Then
                    myCommand.Parameters.Add(New SqlParameter("@destinationcode", SqlDbType.VarChar, 50)).Value = strDest(0)
                    myCommand.Parameters.Add(New SqlParameter("@destinationtype", SqlDbType.VarChar, 50)).Value = strDest(1)
                Else
                    myCommand.Parameters.Add(New SqlParameter("@destinationcode", SqlDbType.VarChar, 50)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@destinationtype", SqlDbType.VarChar, 50)).Value = ""
                End If
                myCommand.Parameters.Add(New SqlParameter("@agentref", SqlDbType.VarChar, 50)).Value = txtAgentRef.Text.Trim
                myCommand.Parameters.Add(New SqlParameter("@guestfirstname", SqlDbType.VarChar, 200)).Value = txtGuestFirstName.Text.Trim
                myCommand.Parameters.Add(New SqlParameter("@guestlastname", SqlDbType.VarChar, 200)).Value = txtGuestSecondName.Text.Trim
                myCommand.Parameters.Add(New SqlParameter("@traveldatetype", SqlDbType.VarChar, 50)).Value = ddlTravelDate.SelectedValue
                If (ddlTravelDate.SelectedValue = "Check In or Check Out" _
                    Or ddlTravelDate.SelectedValue = "Checkin date" _
                    Or ddlTravelDate.SelectedValue = "Checkout date") Then
                    myCommand.Parameters.Add(New SqlParameter("@traveldatefrom", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtTravelFromDate.Text.Trim).ToString("yyyy/MM/dd")
                    myCommand.Parameters.Add(New SqlParameter("@traveldateto", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtTravelToDate.Text.Trim).ToString("yyyy/MM/dd")
                Else
                    myCommand.Parameters.Add(New SqlParameter("@traveldatefrom", SqlDbType.VarChar, 20)).Value = txtTravelFromDate.Text.Trim
                    myCommand.Parameters.Add(New SqlParameter("@traveldateto", SqlDbType.VarChar, 20)).Value = txtTravelToDate.Text.Trim
                End If
                myCommand.Parameters.Add(New SqlParameter("@bookingdatetype", SqlDbType.VarChar, 50)).Value = ddlBookingDate.SelectedValue
                If ddlBookingDate.SelectedValue = "Specific date" Then
                    myCommand.Parameters.Add(New SqlParameter("@bookingdatefrom", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtBookingFromDate.Text.Trim).ToString("yyyy/MM/dd")
                    myCommand.Parameters.Add(New SqlParameter("@bookingdateto", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtBookingToDate.Text.Trim).ToString("yyyy/MM/dd")
                Else
                    myCommand.Parameters.Add(New SqlParameter("@bookingdatefrom", SqlDbType.VarChar, 20)).Value = txtBookingFromDate.Text.Trim
                    myCommand.Parameters.Add(New SqlParameter("@bookingdateto", SqlDbType.VarChar, 20)).Value = txtBookingToDate.Text.Trim
                End If
                myCommand.Parameters.Add(New SqlParameter("@bookingstatus", SqlDbType.VarChar, 50)).Value = ddlBookingStatus.SelectedValue
                myCommand.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
                myCommand.Parameters.Add(New SqlParameter("@hotelconfno", SqlDbType.VarChar, 50)).Value = txtHotelConfNo.Text.Trim
                myCommand.Parameters.Add(New SqlParameter("@searchagentcode", SqlDbType.VarChar, 20)).Value = txtCustomerCode.Text.Trim
                myCommand.Parameters.Add(New SqlParameter("@sourcectrycode", SqlDbType.VarChar, 20)).Value = txtCountryCode.Text.Trim
                myCommand.Parameters.Add(New SqlParameter("@usercode", SqlDbType.VarChar, 20)).Value = txtROCode.Text.Trim
                myCommand.Parameters.Add(New SqlParameter("@companycode", SqlDbType.VarChar, 200)).Value = companyCode     '"675558760549078"
                myCommand.Parameters.Add(New SqlParameter("@subusercode", SqlDbType.VarChar, 20)).Value = ""
                myCommand.Parameters.Add(New SqlParameter("@transfertype", SqlDbType.VarChar, 20)).Value = ddlTransferType.SelectedValue
                myDataAdapter = New SqlDataAdapter(myCommand)
                myDataAdapter.Fill(myDS)
                Session("sDsBooking") = myDS
                If myDS.Tables(0).Rows.Count > 0 Then
                    gvSearchResult.DataSource = myDS.Tables(0)
                    gvSearchResult.PageSize = pagevaluecus
                    gvSearchResult.DataBind()
                    ModalPopupLoading.Hide()
                Else
                    ModalPopupLoading.Hide()
                    gvSearchResult.PageIndex = 0
                    gvSearchResult.DataBind()
                    lblMsg.Visible = True
                    lblMsg.Text = "Records not found, Please redefine search criteria."
                End If
                clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                clsDBConnect.dbConnectionClose(SqlConn)
            End If
        Catch ex As Exception
            ModalPopupLoading.Hide()
            If Not SqlConn Is Nothing Then
                If SqlConn.State = ConnectionState.Open Then
                    clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                    clsDBConnect.dbConnectionClose(SqlConn)
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BookingTray.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub ddlTravelDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTravelDate.SelectedIndexChanged"
    Protected Sub ddlTravelDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTravelDate.SelectedIndexChanged
        If (ddlTravelDate.SelectedItem.Text.ToUpper = "Check in or Check Out".ToUpper() Or ddlTravelDate.SelectedItem.Text.ToUpper = "Checkin Date".ToUpper() Or ddlTravelDate.SelectedItem.Text.ToUpper() = "Checkout Date".ToUpper()) Then
            dvTravelFromDate.Visible = True
            dvTravelToDate.Visible = True
        Else
            txtTravelFromDate.Text = ""
            dvTravelFromDate.Visible = False
            txtTravelToDate.Text = ""
            dvTravelToDate.Visible = False
        End If
    End Sub
#End Region

#Region "Protected Sub ddlBookingDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBookingDate.SelectedIndexChanged"
    Protected Sub ddlBookingDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBookingDate.SelectedIndexChanged
        If ddlBookingDate.SelectedItem.Text.ToUpper = "Specific date".ToUpper() Then
            dvBookingFromDate.Visible = True
            dvBookingToDate.Visible = True
        Else
            txtBookingFromDate.Text = ""
            dvBookingFromDate.Visible = False
            txtBookingToDate.Text = ""
            dvBookingToDate.Visible = False
        End If
    End Sub
#End Region

#Region "Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Session("sDsBooking") = Nothing

        FillGridNew()
    End Sub
#End Region

#Region "Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click"
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        txtBookingRef.Text = ""
        ddlServiceType.SelectedIndex = 0
        txtDestinationName.Text = ""
        txtDestinationCode.Text = ""
        txtAgentRef.Text = ""
        txtGuestFirstName.Text = ""
        txtGuestSecondName.Text = ""
        ddlTravelDate.SelectedIndex = 0
        dvTravelFromDate.Visible = False
        dvTravelToDate.Visible = False
        txtTravelFromDate.Text = ""
        txtTravelToDate.Text = ""
        ddlBookingDate.SelectedIndex = 0
        dvBookingFromDate.Visible = False
        dvBookingToDate.Visible = False
        txtBookingFromDate.Text = ""
        txtBookingToDate.Text = ""
        ddlBookingStatus.SelectedIndex = 0
        txtHotelName.Text = ""
        txtHotelCode.Text = ""
        txtHotelConfNo.Text = ""
        ddlDivision.SelectedIndex = 0
        txtCustomer.Text = ""
        txtCustomerCode.Text = ""
        txtCountry.Text = ""
        txtCountryCode.Text = ""
        txtRO.Text = ""
        txtROCode.Text = ""
        Dim dt As DataTable = CType(Session("bookingSearchResult"), DataTable)
        dt.Clear()
        gvSearchResult.DataSource = dt
        gvSearchResult.DataBind()
        lblMsg.Visible = False
    End Sub
#End Region

#Region "Protected Sub gvSearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSearchResult.PageIndexChanging"
    Protected Sub gvSearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSearchResult.PageIndexChanging
        gvSearchResult.PageIndex = e.NewPageIndex
        FillGridNew()
    End Sub
#End Region

#Region "Protected Sub gvSearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSearchResult.Sorting"
    Protected Sub gvSearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSearchResult.Sorting
        Session.Add("strsortexpression", e.SortExpression)
        SortGridColumn()
    End Sub
#End Region

#Region "Public Sub SortGridColumn()"
    Public Sub SortGridColumn()
        Dim DataTable As DataTable
        FillGridNew()
        DataTable = gvSearchResult.DataSource
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirection", objUtils.SwapSortDirection(Session("strsortdirection")))
            dataView.Sort = Session("strsortexpression") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirection"))
            gvSearchResult.DataSource = dataView
            gvSearchResult.DataBind()
        End If
    End Sub
#End Region

#Region "Protected Sub RowsPerPageCUS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged"
    Protected Sub RowsPerPageCUS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowsPerPageCUS.SelectedIndexChanged
        FillGridNew()
    End Sub
#End Region

#Region "Protected Sub gvSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSearchResult.RowCommand"
    Protected Sub gvSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSearchResult.RowCommand
        Try
            Dim strpop As String = ""
            If e.CommandName = "ProformaPrint" Then
                Dim lblRequestId As Label
                Dim lblInvoiceNo As Label
                lblInvoiceNo = gvSearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblInvoiceNo")
                lblRequestId = gvSearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblRequestId")
                strpop = "window.open('PrintReport.aspx?printId=bookingConfirmation&RequestId=" & lblRequestId.Text.Trim & "&ManualInvNo= " & lblInvoiceNo.Text & "','ProformaPrint');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "proformaInvoice", strpop, True)


                'Dim chkCumulative As String = objUtils.ExecuteQueryReturnStringValue("select bookingengineratetype from booking_header H inner join agentmast A on H.agentcode=A.agentcode and H.requestid='" + lblRequestId.Text.Trim + "'")
                'If String.IsNullOrEmpty(chkCumulative) Then chkCumulative = ""
                'If chkCumulative.Trim() = "CUMULATIVE" Then

                '  End If
            ElseIf e.CommandName = "ItineraryPrint" Then
                Dim lblRequestId As Label
                lblRequestId = gvSearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblRequestId")

                strpop = "window.open('PrintReport.aspx?printId=Itinerary&RequestId=" & lblRequestId.Text.Trim & "');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popupItinerary", strpop, True)
                'Dim ConfirmStatus As String = objUtils.ExecuteQueryReturnStringValue("select dbo.fn_booking_confirmstatus('" & lblRequestId.Text.Trim & "') as ConfirmStatus")
                'If ConfirmStatus = "1" Then
                '    Dim hvDt As DataTable = objUtils.GetDataFromDataTable("select rlineno from booking_hotel_detail where requestid ='" + lblRequestId.Text.Trim + "' group by rlineno")
                '    If hvDt.Rows.Count > 0 Then
                '        Dim rlineNumber As Integer = 0
                '        For Each hvDr As DataRow In hvDt.Rows
                '            rlineNumber = Convert.ToInt32(hvDr("rlineno"))
                '            strpop = "window.open('PrintReport.aspx?printId=hotelVoucher&RequestId=" & lblRequestId.Text.Trim & "&rlineNo=" & rlineNumber.ToString() & "');"
                '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup" + rlineNumber.ToString(), strpop, True)
                '        Next
                '    End If
                'End If
            ElseIf e.CommandName = "VoucherPrint" Then
                Dim lblRequestId As Label
                Dim lblInvoiceNo As Label
                lblInvoiceNo = gvSearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblInvoiceNo")
                lblRequestId = gvSearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblRequestId")
                ' strpop = "window.open('PrintReport.aspx?printId=ProformaVat&RequestId=" & lblRequestId.Text.Trim & "&ManualInvNo= " & lblInvoiceNo.Text & "','ProformaPrint');"
                strpop = "window.open('PrintReport.aspx?printId=bookingVoucher&RequestId=" & lblRequestId.Text.Trim & "&ManualInvNo= " & lblInvoiceNo.Text & "','ProformaPrint');"
                '     strpop = "window.open('../AccountsModule/PrintReport.aspx?printId=bookingConfirmation&RequestId=" & CType(lblRequestId.Text.Trim, String) & "','ProformaPrint');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "proformaInvoice", strpop, True)
            ElseIf e.CommandName = "InvoicePrint" Then
                Dim lblInvoiceNo As Label
                lblInvoiceNo = gvSearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblInvoiceNo")
                If lblInvoiceNo.Text.Trim <> "" Then
                    strpop = "window.open('PrintReport.aspx?printId=salesInvoice&InvoiceNo=" & CType(lblInvoiceNo.Text.Trim, String) & "','SalesInvoicePrint');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "salesInvoice", strpop, True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BookingTray.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub gvSearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSearchResult.RowDataBound"
    Protected Sub gvSearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSearchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim decimalPlaces As Integer = CType(Session("decimalPlaces"), Integer)
            Dim lblSaleValue As Label = CType(e.Row.FindControl("lblSaleValue"), Label)
            If IsNumeric(lblSaleValue.Text) Then
                lblSaleValue.Text = Math.Round(Convert.ToDecimal(lblSaleValue.Text), decimalPlaces)
            End If
            Dim lblInvoiceNo As Label
            lblInvoiceNo = CType(e.Row.FindControl("lblInvoiceNo"), Label)
            If lblInvoiceNo.Text.Trim = "" Then
                Dim lbtnPrint As LinkButton
                lbtnPrint = CType(e.Row.FindControl("lbtnPrint"), LinkButton)
                lbtnPrint.Visible = False
            End If
            Dim lblBookingStatus As Label
            lblBookingStatus = CType(e.Row.FindControl("lblBookingStatus"), Label)
            If lblBookingStatus.Text.Trim = "Cancelled" Then
                Dim lbtnProforma As LinkButton
                lbtnProforma = CType(e.Row.FindControl("lbtnProforma"), LinkButton)
                lbtnProforma.Visible = False
            End If
            Dim lbtnSequenceNo As LinkButton
            lbtnSequenceNo = CType(e.Row.FindControl("lbtnSequenceNo"), LinkButton)
            Dim trClass As HtmlTableRow = CType(e.Row.FindControl("trClass"), HtmlTableRow)
            If lbtnSequenceNo.CommandArgument = "Show" Then
                trClass.Visible = False
            Else
                trClass.Visible = True
            End If
            Dim baseCurrency As String = CType(Session("baseCurrency"), String)
            Dim gvHotelDetail As GridView = CType(e.Row.FindControl("gvHotelDetail"), GridView)
            gvHotelDetail.Columns(8).HeaderText = "Sale Nontaxable (" + baseCurrency + ")"
            gvHotelDetail.Columns(9).HeaderText = "Sale Taxable (" + baseCurrency + ")"
            gvHotelDetail.Columns(10).HeaderText = "VAT Payable (" + baseCurrency + ")"
            gvHotelDetail.Columns(11).HeaderText = "Total Sale (" + baseCurrency + ")"
            gvHotelDetail.Columns(12).HeaderText = "Cost Nontaxable (" + baseCurrency + ")"
            gvHotelDetail.Columns(13).HeaderText = "Cost Taxable (" + baseCurrency + ")"
            gvHotelDetail.Columns(14).HeaderText = "VAT Input (" + baseCurrency + ")"
            gvHotelDetail.Columns(15).HeaderText = "Total Cost (" + baseCurrency + ")"
            gvHotelDetail.Columns(16).HeaderText = "Gross Profit (" + baseCurrency + ")"
            gvHotelDetail.Columns(12).Visible = False
        End If
    End Sub
#End Region

#Region "Protected Sub lbtnSequenceNo_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub lbtnSequenceNo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim lbtnSequenceNo As LinkButton = CType(sender, LinkButton)
            Dim gvr As GridViewRow = lbtnSequenceNo.NamingContainer
            Dim panHotelDetail As Panel
            Dim gvHotelDetail As GridView
            Dim lblRequestId As Label
            Dim lblMsgHotelDetail As Label
            Dim trClass As HtmlTableRow = CType(gvr.FindControl("trClass"), HtmlTableRow)
            panHotelDetail = CType(gvr.FindControl("panHotelDetail"), Panel)
            gvHotelDetail = CType(gvr.FindControl("gvHotelDetail"), GridView)
            lblMsgHotelDetail = CType(gvr.FindControl("lblMsgHotelDetail"), Label)
            If lbtnSequenceNo.CommandArgument = "Show" Then
                lblRequestId = CType(gvr.FindControl("lblRequestId"), Label)
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
                myCommand = New SqlCommand("sp_getbookingtray_detail", SqlConn)
                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.CommandTimeout = 0
                myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = lblRequestId.Text.Trim
                myDataAdapter = New SqlDataAdapter(myCommand)
                Using dt As New DataTable
                    myDataAdapter.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        gvHotelDetail.DataSource = dt
                        gvHotelDetail.DataBind()

                        Dim lblTotalSaleNontaxValue As Label
                        Dim lblTotalSaleValue As Label
                        Dim lblTotalVatPayable As Label
                        Dim lblTotalSaleValueBase As Label
                        Dim lblTotalCostNontaxable As Label
                        Dim lblTotalCost As Label
                        Dim lblTotalVatInputBase As Label
                        Dim lblTotalCostValueBase As Label
                        Dim lblTotalGrossProfit As Label

                        lblTotalSaleNontaxValue = CType(gvHotelDetail.FooterRow.FindControl("lblTotalSaleNontaxValue"), Label)
                        lblTotalSaleValue = CType(gvHotelDetail.FooterRow.FindControl("lblTotalSaleValue"), Label)
                        lblTotalVatPayable = CType(gvHotelDetail.FooterRow.FindControl("lblTotalVatPayable"), Label)
                        lblTotalSaleValueBase = CType(gvHotelDetail.FooterRow.FindControl("lblTotalSaleValueBase"), Label)
                        lblTotalCostNontaxable = CType(gvHotelDetail.FooterRow.FindControl("lblTotalCostNontaxable"), Label)
                        lblTotalCost = CType(gvHotelDetail.FooterRow.FindControl("lblTotalCost"), Label)
                        lblTotalVatInputBase = CType(gvHotelDetail.FooterRow.FindControl("lblTotalVatInputBase"), Label)
                        lblTotalCostValueBase = CType(gvHotelDetail.FooterRow.FindControl("lblTotalCostValueBase"), Label)
                        lblTotalGrossProfit = CType(gvHotelDetail.FooterRow.FindControl("lblTotalGrossProfit"), Label)

                        Dim TotalSaleNontaxValue As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("salenontaxablebase"))
                        Dim TotalSaleValue As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("saletaxablebase"))
                        Dim TotalVatPayable As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("vatpayablebase"))
                        Dim TotalSaleValueBase As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("salevaluebase"))
                        Dim TotalCostNontaxable As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("costnontaxablebase"))
                        Dim TotalCost As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("costtaxablebase"))
                        Dim TotalVatInputBase As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("vatinputbase"))
                        Dim TotalCostValueBase As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("costvaluebase"))
                        Dim TotalGrossProfit As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("grossprofit"))

                        Dim decimalPlaces As Integer = CType(Session("decimalPlaces"), Integer)
                        For i = 0 To 6
                            gvHotelDetail.FooterRow.Cells(i).CssClass = "fbcolor"
                        Next
                        For i = 17 To 19
                            gvHotelDetail.FooterRow.Cells(i).CssClass = "fbcolor"
                        Next
                        lblTotalSaleNontaxValue.Text = Math.Round(TotalSaleNontaxValue, decimalPlaces)
                        lblTotalSaleValue.Text = Math.Round(TotalSaleValue, decimalPlaces)
                        lblTotalVatPayable.Text = Math.Round(TotalVatPayable, decimalPlaces)
                        lblTotalSaleValueBase.Text = Math.Round(TotalSaleValueBase, decimalPlaces)
                        lblTotalCostNontaxable.Text = Math.Round(TotalCostNontaxable, decimalPlaces)
                        lblTotalCost.Text = Math.Round(TotalCost, decimalPlaces)
                        lblTotalVatInputBase.Text = Math.Round(TotalVatInputBase, decimalPlaces)
                        lblTotalCostValueBase.Text = Math.Round(TotalCostValueBase, decimalPlaces)
                        lblTotalGrossProfit.Text = Math.Round(TotalGrossProfit, decimalPlaces)
                        lblMsgHotelDetail.Visible = False
                    Else
                        gvHotelDetail.DataSource = dt
                        gvHotelDetail.DataBind()
                        lblMsgHotelDetail.Visible = True
                        lblMsgHotelDetail.Text = "Records not found, Please check booking details."
                    End If
                End Using
                clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                clsDBConnect.dbConnectionClose(SqlConn)
                trClass.Visible = True
                panHotelDetail.Visible = True
                lbtnSequenceNo.CommandArgument = "Hide"
            Else
                lbtnSequenceNo.CommandArgument = "Show"
                trClass.Visible = False
                panHotelDetail.Visible = False
                gvHotelDetail.DataSource = Nothing
                gvHotelDetail.DataBind()
                lblMsgHotelDetail.Visible = False
            End If
        Catch ex As Exception
            If Not SqlConn Is Nothing Then
                clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                clsDBConnect.dbConnectionClose(SqlConn)
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BookingTray.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub gvHotelDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)"
    Protected Sub gvHotelDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblSaleNontaxValue As Label
            Dim lblSalevalue As Label
            Dim lblVatPayable As Label
            Dim lblSaleValueBase As Label
            Dim lblCostNontaxable As Label
            Dim lblCost As Label
            Dim lblVatInputBase As Label
            Dim lblCostValueBase As Label
            Dim lblGrossProfit As Label
            Dim lblComplimentaryCust As Label
            Dim lblComplimentarySupp As Label
            Dim lblToDate As Label

            lblSaleNontaxValue = CType(e.Row.FindControl("lblSaleNontaxValue"), Label)
            lblSalevalue = CType(e.Row.FindControl("lblSalevalue"), Label)
            lblVatPayable = CType(e.Row.FindControl("lblVatPayable"), Label)
            lblSaleValueBase = CType(e.Row.FindControl("lblSaleValueBase"), Label)
            lblCostNontaxable = CType(e.Row.FindControl("lblCostNontaxable"), Label)
            lblCost = CType(e.Row.FindControl("lblCost"), Label)
            lblVatInputBase = CType(e.Row.FindControl("lblVatInputBase"), Label)
            lblCostValueBase = CType(e.Row.FindControl("lblCostValueBase"), Label)
            lblGrossProfit = CType(e.Row.FindControl("lblGrossProfit"), Label)
            lblComplimentaryCust = CType(e.Row.FindControl("lblComplimentaryCust"), Label)
            lblComplimentarySupp = CType(e.Row.FindControl("lblComplimentarySupp"), Label)
            lblToDate = CType(e.Row.FindControl("lblToDate"), Label)

            Dim lbConfirm As LinkButton = CType(e.Row.FindControl("lbConfirm"), LinkButton)
            Dim txtConfirmNo As TextBox = CType(e.Row.FindControl("txtConfirmNo"), TextBox)
            Dim txtConfirmTimeLimit As TextBox = CType(e.Row.FindControl("txtConfirmTimeLimit"), TextBox)
            Dim btnConfirmSave As Button = CType(e.Row.FindControl("btnConfirmSave"), Button)
            Dim btnConfirmCancel As Button = CType(e.Row.FindControl("btnConfirmCancel"), Button)
            Dim ImgBtnConfirmTimeLimit As ImageButton = CType(e.Row.FindControl("ImgBtnConfirmTimeLimit"), ImageButton)
            Dim Str As String = "'" + txtConfirmNo.ClientID + "'" + ",'" + txtConfirmTimeLimit.ClientID + "'"

            ' btnConfirmSave.Attributes.Add("OnClientClick", "javascript:fnConfirmSave('" + txtConfirmNo.ClientID + "','" + txtConfirmTimeLimit.ClientID + "')")
            '  btnConfirmSave.Attributes.Add("OnClientClick", "javascript:fnConfirmSave()")
            '            txtsaleprice.Attributes.Add("onChange", "javascript:CalculateRoomTotalPrice('" + lblSaleTotal.ClientID + "','" + _gvPricebreakup.ClientID + "'
            Dim lblLineStatus As Label = CType(e.Row.FindControl("lblLineStatus"), Label)

            If lblLineStatus.Text = "Confirmed" Or lblLineStatus.Text = "Cancelled" Then
                lbConfirm.Visible = False
                txtConfirmNo.Visible = False
                txtConfirmTimeLimit.Visible = False
                btnConfirmSave.Visible = False
                btnConfirmCancel.Visible = False
                ImgBtnConfirmTimeLimit.Visible = False
            Else
                lbConfirm.Visible = True
                txtConfirmNo.Visible = False
                txtConfirmTimeLimit.Visible = False
                btnConfirmSave.Visible = False
                btnConfirmCancel.Visible = False
                ImgBtnConfirmTimeLimit.Visible = False
            End If
            'Dim dvFlight As HtmlGenericControl = CType(e.Row.FindControl("dvFlight"), HtmlGenericControl)
            'Dim txtFlightCode As TextBox = CType(e.Row.FindControl("txtFlightCode"), TextBox)
            'Dim txtFlightTime As TextBox = CType(e.Row.FindControl("txtFlightTime"), TextBox)
            Dim lbUpdateFlight As LinkButton = CType(e.Row.FindControl("lbUpdateFlight"), LinkButton)
            Dim lblServiceType As Label = CType(e.Row.FindControl("lblServiceType"), Label)
            Dim lbUpdatePickupTime As LinkButton = CType(e.Row.FindControl("lbUpdatePickupTime"), LinkButton)
            Dim lblAirportbordercode As Label = CType(e.Row.FindControl("lblAirportbordercode"), Label)
            ' dvFlight.Visible = False
            If lblAirportbordercode.Text.Trim <> "" Or lblAirportbordercode.Text <> "" Then
                lbUpdateFlight.Text = "Change"
            Else
                lbUpdateFlight.Text = "Add"
            End If

            If lblServiceType.Text = "Hotel" Or lblServiceType.Text.Trim = "Tours" Or lblServiceType.Text = "Tours" Or lblServiceType.Text = "Visa" Or lblServiceType.Text = "Others" Or lblServiceType.Text.Contains("INTERHOTEL") Then
                ' dvFlight.Visible = False
                lbUpdateFlight.Visible = False
            End If

            'If (lblServiceType.Text.Contains("Tours") Or lblServiceType.Text.Contains("Transfer")) And lblLineStatus.Text <> "Cancelled" Then
            '    lbUpdatePickupTime.Visible = True
            'Else
            '    lbUpdatePickupTime.Visible = False
            'End If

            Dim decimalPlaces As Integer = CType(Session("decimalPlaces"), Integer)
            If IsNumeric(lblSaleNontaxValue.Text) Then
                lblSaleNontaxValue.Text = Math.Round(Convert.ToDecimal(lblSaleNontaxValue.Text), decimalPlaces)
            End If
            If IsNumeric(lblSalevalue.Text) Then
                lblSalevalue.Text = Math.Round(Convert.ToDecimal(lblSalevalue.Text), decimalPlaces)
            End If
            If IsNumeric(lblVatPayable.Text) Then
                lblVatPayable.Text = Math.Round(Convert.ToDecimal(lblVatPayable.Text), decimalPlaces)
            End If
            If IsNumeric(lblSaleValueBase.Text) Then
                lblSaleValueBase.Text = Math.Round(Convert.ToDecimal(lblSaleValueBase.Text), decimalPlaces)
            End If
            If IsNumeric(lblCostNontaxable.Text) Then
                lblCostNontaxable.Text = Math.Round(Convert.ToDecimal(lblCostNontaxable.Text), decimalPlaces)
            End If
            If IsNumeric(lblCost.Text) Then
                lblCost.Text = Math.Round(Convert.ToDecimal(lblCost.Text), decimalPlaces)
            End If
            If IsNumeric(lblVatInputBase.Text) Then
                lblVatInputBase.Text = Math.Round(Convert.ToDecimal(lblVatInputBase.Text), decimalPlaces)
            End If
            If IsNumeric(lblCostValueBase.Text) Then
                lblCostValueBase.Text = Math.Round(Convert.ToDecimal(lblCostValueBase.Text), decimalPlaces)
            End If
            If IsNumeric(lblGrossProfit.Text) Then
                lblGrossProfit.Text = Math.Round(Convert.ToDecimal(lblGrossProfit.Text), decimalPlaces)
            End If
            If IsNumeric(lblComplimentaryCust.Text) Then
                If Convert.ToInt32(lblComplimentaryCust.Text) = 0 Then lblComplimentaryCust.Text = "No" Else lblComplimentaryCust.Text = "Yes"
            End If
            If IsNumeric(lblComplimentarySupp.Text) Then
                If Convert.ToInt32(lblComplimentarySupp.Text) = 0 Then lblComplimentarySupp.Text = "No" Else lblComplimentarySupp.Text = "Yes"
            End If
            If lblToDate.Text = "01/01/1900" Then
                lblToDate.Text = ""
            End If


            Dim txtRequestId As TextBox = CType(e.Row.FindControl("txtRequestId"), TextBox)
            Dim txtRlineNo As TextBox = CType(e.Row.FindControl("txtRlineNo"), TextBox)
            '  Dim lblServiceType As Label = CType(e.Row.FindControl("lblServiceType"), Label)
            Dim lbExcusrionTicket As LinkButton = CType(e.Row.FindControl("lbExcusrionTicket"), LinkButton)
            If lblServiceType.Text.Trim = "Tours" Then
                Dim strExc As String = "select count(file_attachment)cnt from New_booking_tours t(nolock) , NewBooking_ServiceAllocation (nolock) s where t.requestid=s.requestid and t.service_id = s.service_id and isnull(s.file_attachment,'')<>'' and t.requestid='" + txtRequestId.Text + "' and elineno='" + txtRlineNo.Text + "'"

                Dim strExcCount As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strExc)
                If Val(strExcCount) > 0 Then
                    lbExcusrionTicket.Visible = True
                Else
                    lbExcusrionTicket.Visible = False
                End If
            Else
                lbExcusrionTicket.Visible = False
            End If

        End If
    End Sub
#End Region

    Protected Sub btnLoadReport_Click(sender As Object, e As System.EventArgs) Handles btnLoadReport.Click
        Try
            If Validation() = False Then Exit Sub
            Dim divcode As String = ""
            If ddlDivision.SelectedIndex = 0 Then
                For i = 1 To ddlDivision.Items.Count - 1
                    If divcode = "" Then
                        divcode = "'" + ddlDivision.Items(i).Value + "'"
                    Else
                        divcode = divcode + ",'" + ddlDivision.Items(i).Value + "'"
                    End If
                Next
            Else
                divcode = "'" + ddlDivision.SelectedValue + "'"
            End If
            Dim companyCode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select stuff((select ',' + convert(varchar(30),randomnumber) " &
            "from agentmast_whitelabel a,agentmast am where a.agentcode=am.agentcode and a.owncompany=1 and " &
            "am.divcode in (" + divcode + ") for xml path ('')),1,1,'') as companycode")
            Dim myDS As New DataSet
            Dim pagevaluecus = RowsPerPageCUS.SelectedValue
            lblMsg.Visible = False
            If gvSearchResult.PageIndex < 0 Then gvSearchResult.PageIndex = 0
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myCommand = New SqlCommand("sp_booking_search", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 0
            myCommand.Parameters.Add(New SqlParameter("@logintype", SqlDbType.VarChar, 20)).Value = "RO"
            myCommand.Parameters.Add(New SqlParameter("@webusername", SqlDbType.VarChar, 20)).Value = ""
            'CType(Session("GlobalUserName"), String)
            myCommand.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = txtCustomerCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = txtBookingRef.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar, 50)).Value = ddlServiceType.SelectedValue
            Dim DestinationCodeAndType As String = txtDestinationCode.Text
            Dim strDest As String() = txtDestinationCode.Text.Split("|")
            If strDest.Length = 2 Then
                myCommand.Parameters.Add(New SqlParameter("@destinationcode", SqlDbType.VarChar, 50)).Value = strDest(0)
                myCommand.Parameters.Add(New SqlParameter("@destinationtype", SqlDbType.VarChar, 50)).Value = strDest(1)
            Else
                myCommand.Parameters.Add(New SqlParameter("@destinationcode", SqlDbType.VarChar, 50)).Value = ""
                myCommand.Parameters.Add(New SqlParameter("@destinationtype", SqlDbType.VarChar, 50)).Value = ""
            End If
            myCommand.Parameters.Add(New SqlParameter("@agentref", SqlDbType.VarChar, 50)).Value = txtAgentRef.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@guestfirstname", SqlDbType.VarChar, 200)).Value = txtGuestFirstName.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@guestlastname", SqlDbType.VarChar, 200)).Value = txtGuestSecondName.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@traveldatetype", SqlDbType.VarChar, 50)).Value = ddlTravelDate.SelectedValue
            If (ddlTravelDate.SelectedValue = "Check In or Check Out" _
                Or ddlTravelDate.SelectedValue = "Checkin date" _
                Or ddlTravelDate.SelectedValue = "Checkout date") Then
                myCommand.Parameters.Add(New SqlParameter("@traveldatefrom", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtTravelFromDate.Text.Trim).ToString("yyyy/MM/dd")
                myCommand.Parameters.Add(New SqlParameter("@traveldateto", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtTravelToDate.Text.Trim).ToString("yyyy/MM/dd")
            Else
                myCommand.Parameters.Add(New SqlParameter("@traveldatefrom", SqlDbType.VarChar, 20)).Value = txtTravelFromDate.Text.Trim
                myCommand.Parameters.Add(New SqlParameter("@traveldateto", SqlDbType.VarChar, 20)).Value = txtTravelToDate.Text.Trim
            End If
            myCommand.Parameters.Add(New SqlParameter("@bookingdatetype", SqlDbType.VarChar, 50)).Value = ddlBookingDate.SelectedValue
            If ddlBookingDate.SelectedValue = "Specific date" Then
                myCommand.Parameters.Add(New SqlParameter("@bookingdatefrom", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtBookingFromDate.Text.Trim).ToString("yyyy/MM/dd")
                myCommand.Parameters.Add(New SqlParameter("@bookingdateto", SqlDbType.VarChar, 20)).Value = Convert.ToDateTime(txtBookingToDate.Text.Trim).ToString("yyyy/MM/dd")
            Else
                myCommand.Parameters.Add(New SqlParameter("@bookingdatefrom", SqlDbType.VarChar, 20)).Value = txtBookingFromDate.Text.Trim
                myCommand.Parameters.Add(New SqlParameter("@bookingdateto", SqlDbType.VarChar, 20)).Value = txtBookingToDate.Text.Trim
            End If
            myCommand.Parameters.Add(New SqlParameter("@bookingstatus", SqlDbType.VarChar, 50)).Value = ddlBookingStatus.SelectedValue
            myCommand.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = txtHotelCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@hotelconfno", SqlDbType.VarChar, 50)).Value = txtHotelConfNo.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@searchagentcode", SqlDbType.VarChar, 20)).Value = txtCustomerCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@sourcectrycode", SqlDbType.VarChar, 20)).Value = txtCountryCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@usercode", SqlDbType.VarChar, 20)).Value = txtROCode.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@companycode", SqlDbType.VarChar, 200)).Value = companyCode     '"675558760549078"
            myCommand.Parameters.Add(New SqlParameter("@subusercode", SqlDbType.VarChar, 20)).Value = ""
            myDataAdapter = New SqlDataAdapter(myCommand)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                'gvSearchResult.DataSource = myDS.Tables(0)
                'gvSearchResult.PageSize = pagevaluecus
                'gvSearchResult.DataBind()
                'ModalPopupLoading.Hide()


                Dim trow As Integer = 3
                Dim wb As New XLWorkbook

                Dim FolderPath As String = "..\ExcelTemplates\"
                Dim FileName As String = "BookingList.xlsx"
                Dim FilePath As String = Server.MapPath(FolderPath + FileName)
                Dim RandomCls As New Random()
                Dim RandomNo As String = RandomCls.Next(100000, 9999999).ToString

                Dim FileNameNew As String = "BookingList_" & Now.Year & Now.Month & Now.Day & Now.Hour & Now.Minute & Now.Second & Now.Millisecond & ".xlsx"
                document = New XLWorkbook
                Dim ws As IXLWorksheet = wb.Worksheets.Add("BookingListSheet")
                ws.Style.Font.FontName = "Times New Roman"



                Dim header As IXLRange
                header = ws.Range("A2:O2")
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
                headertext.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
                headertext.Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
                Dim range1 As IXLRange
                range1 = ws.Range("e2:k2")
                range1.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                range1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                range1.Merge()
                range1.Value = CType(Session("CompanyName"), String)
                range1.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
                range1.Style.Border.SetTopBorder(XLBorderStyleValues.Thin)




                Dim header2 As IXLRange
                header2 = ws.Range("A3:O3")
                header2.Style.Font.SetBold()
                header2.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#FFE5CC"))
                header2.Merge()
                header2.Style.Font.FontSize = 14
                Dim headertext2 As Object
                headertext2 = ws.Ranges("e3:h3")
                headertext2.Style.Font.SetBold()
                headertext2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                headertext2.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                Dim range2 As IXLRange
                range2 = ws.Range("e3:h3")
                Dim multiline As Object
                multiline = String.Concat("Booking List")
                range2.Cell(1, 1).Value = multiline
                range2.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                range2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                range2.Merge()

                ws.Range(3, 1, 3, 17).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
                ws.Range(3, 1, 3, 17).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
                ws.Range(3, 15, 3, 17).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(4, 15, 4, 17).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(2, 15, 2, 17).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                Dim repfilter As New StringBuilder

                ws.Cell(4, 1).Value = repfilter

                'If TxtSectorName.Text <> "" Then
                '    ws.Cell(4, 1).Value = repfilter.Append("Country Group: " & TxtSectorName.Text & "")
                'End If
                'If TxtCtryName.Text <> "" Then
                '    ws.Cell(4, 1).Value = repfilter.Append(" , Airport : " & TxtCtryName.Text & "")
                'End If
                'If TxtAgentName.Text <> "" Then

                '    ws.Cell(4, 1).Value = repfilter.Append(" , Agent : " & TxtAgentName.Text & "")
                'End If
                'If txtHotelName.Text <> "" Then

                '    ws.Cell(4, 1).Value = repfilter.Append(" , Hotel : " & txtHotelName.Text & "")
                'End If
                'If TxtCityName.Text <> "" Then

                '    ws.Cell(4, 1).Value = repfilter.Append(" , Hotel City: " & TxtCityName.Text & "")
                'End If
                'If TxtFlightName.Text <> "" Then
                '    ws.Cell(4, 1).Value = repfilter.Append(",  Flight :  " & TxtFlightName.Text & "")
                'End If
                'ws.Cell(4, 1).Value = repfilter.Append(" , Group By:  " & GroupBy.SelectedItem.Text & "  , Division:  " & ddldivisions.SelectedItem.Text & "")
                'ws.Range(4, 1, 4, 17).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)

                trow = 5

                ws.Column("A").Width = 10
                ws.Column("B").Width = 15
                ws.Column("C").Width = 20
                ws.Column("D").Width = 50
                ws.Column("E").Width = 20
                ws.Column("F").Width = 20
                ws.Column("G").Width = 20
                ws.Column("H").Width = 50
                ws.Column("I").Width = 30
                ws.Column("J").Width = 40
                ws.Column("K").Width = 20
                ws.Column("L").Width = 30
                ws.Column("M").Width = 20
                ws.Column("N").Width = 20
                ws.Column("O").Width = 30
                ws.Column("P").Width = 30
                ws.Column("Q").Width = 11
                'ws.Column("P").Width = 25
                'ws.Column("Q").Width = 15
                'ws.Column("R").Width = 7
                'ws.Column("S").Width = 15
                'ws.Column("t").Width = 25


                Dim title = ws.Range(trow, 1, trow, 23)

                Dim bookingtitle = ws.Range(trow, 1, trow, 23).Style.Font.SetBold()
                ws.Range(trow, 1, trow, 17).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
                ws.Range(trow, 1, trow, 17).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
                bookingtitle.Alignment.WrapText = True
                ws.Cell(trow, 1).Value = "Sequence No"
                ws.Cell(trow, 2).Value = "Booking No"
                ws.Cell(trow, 3).Value = "Booking Date"
                ws.Cell(trow, 4).Value = "Hotel/Service Name"
                ws.Cell(trow, 5).Value = "CheckIn Date"
                ws.Cell(trow, 6).Value = "CheckOut Date"
                ws.Cell(trow, 7).Value = "Service Date"
                ws.Cell(trow, 8).Value = "Customer Name"
                ws.Cell(trow, 9).Value = "Customer Ref."
                ws.Cell(trow, 10).Value = "Guest Name"
                ws.Cell(trow, 11).Value = "Booking Status"
                ws.Cell(trow, 12).Value = "Sales Amount"

                ws.Cell(trow, 13).Value = "Collected Value"
                ws.Cell(trow, 14).Value = "Balance Amount"

                ws.Cell(trow, 15).Value = "Last Modified Date"
                ws.Cell(trow, 16).Value = "Last Modified By"
                ws.Cell(trow, 17).Value = "Invoice No."
                'ws.Cell(trow, 16).Value = "Agent Name"
                'ws.Cell(trow, 17).Value = "Agency Ref."
                'ws.Cell(trow, 18).Value = "Nights"
                'ws.Cell(trow, 19).Value = "Conf. No."
                'ws.Cell(trow, 20).Value = "Remarks"
                'ws.Cell(trow, 21).Value = "Room Details"
                ws.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                ws.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                ws.Range(trow, 1, trow, 17).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
                ws.Range(trow, 1, trow, 17).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                ws.Range(trow, 1, trow, 17).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)



                trow = trow + 1
                If myDS.Tables(0).Rows.Count >= 0 Then
                    For Each bookingdetail In myDS.Tables(0).Rows
                        title = ws.Range(++trow, 1, trow, 17)
                        title.Style.Alignment.WrapText = True
                        ws.Cell(trow, 1).Value = bookingdetail("SequenceNo")
                        ws.Cell(trow, 2).Value = bookingdetail("requestid")
                        ws.Cell(trow, 3).Value = bookingdetail("requestdate")
                        ws.Cell(trow, 4).Value = bookingdetail("servicename")
                        ws.Cell(trow, 5).Value = bookingdetail("checkindate")
                        ws.Cell(trow, 6).Value = bookingdetail("checkoutdate")
                        ws.Cell(trow, 7).Value = bookingdetail("servicedate")
                        ws.Cell(trow, 8).Value = bookingdetail("agentname")
                        ws.Cell(trow, 9).Value = bookingdetail("agentref")
                        ws.Cell(trow, 10).Value = bookingdetail("guestname")
                        ws.Cell(trow, 11).Value = bookingdetail("bookingstatus")
                        ws.Cell(trow, 12).Value = bookingdetail("salevaluebase")

                        ws.Cell(trow, 13).Value = bookingdetail("collectedamt")
                        ws.Cell(trow, 14).Value = bookingdetail("balanceamt")

                        ws.Cell(trow, 15).Value = bookingdetail("lastmodified")
                        ws.Cell(trow, 16).Value = bookingdetail("modifiedby")
                        ws.Cell(trow, 17).Value = (bookingdetail("invoiceno"))
                        'ws.Cell(trow, 16).Value = bookingdetail("agentname")
                        'ws.Cell(trow, 17).Value = bookingdetail("agentref")
                        'ws.Cell(trow, 18).Value = bookingdetail("nights")
                        'ws.Cell(trow, 19).Value = bookingdetail("confno")
                        'ws.Cell(trow, 20).Value = bookingdetail("remarks")
                        'ws.Column("U").Width = 30
                        'ws.Cell(trow, 21).Value = bookingdetail("roomdetails")
                        ws.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                        ws.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                        ws.Range(trow, 1, trow, 17).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
                        ws.Range(trow, 1, trow, 17).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                        ws.Range(trow, 1, trow, 17).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)

                        trow = trow + 1
                    Next

                End If

                ModalPopupLoading.Hide()
                Try
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
                Catch ex As Exception

                End Try






            Else
                ModalPopupLoading.Hide()
                gvSearchResult.PageIndex = 0
                gvSearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)
        Catch ex As Exception
            ModalPopupLoading.Hide()
            If Not SqlConn Is Nothing Then
                If SqlConn.State = ConnectionState.Open Then
                    clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                    clsDBConnect.dbConnectionClose(SqlConn)
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BookingTray.aspx::LoadReport", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnConfirmSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim mySqlConn As SqlConnection
        Dim sqlTrans As SqlTransaction
        Dim mySqlCmd As SqlCommand
        Try
            Dim btnConfirmSave As Button = CType(sender, Button)
            Dim gvr As GridViewRow = btnConfirmSave.NamingContainer
            Dim lblLineStatus As Label = CType(gvr.FindControl("lblLineStatus"), Label)
            Dim txtConfirmNo As TextBox = CType(gvr.FindControl("txtConfirmNo"), TextBox)
            Dim txtConfirmTimeLimit As TextBox = CType(gvr.FindControl("txtConfirmTimeLimit"), TextBox)
            If txtConfirmNo.Text = "" Or txtConfirmTimeLimit.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter confirmation number or time limt' );", True)
                Exit Sub
            End If
            Dim lblConfNo As Label = CType(gvr.FindControl("lblConfNo"), Label)
            Dim lblServiceType As Label = CType(gvr.FindControl("lblServiceType"), Label)
            Dim txtRequestId As TextBox = CType(gvr.FindControl("txtRequestId"), TextBox)
            Dim txtRlineNo As TextBox = CType(gvr.FindControl("txtRlineNo"), TextBox)
            Dim txtRoomno As TextBox = CType(gvr.FindControl("txtRoomno"), TextBox)
            Dim btnConfirmCancel As Button = CType(gvr.FindControl("btnConfirmCancel"), Button)
            Dim ImgBtnConfirmTimeLimit As ImageButton = CType(gvr.FindControl("ImgBtnConfirmTimeLimit"), ImageButton)
            Dim iRoomNo As Integer = 0
            If lblServiceType.Text = "Hotel" Then
                iRoomNo = txtRoomno.Text
            End If


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

            'Inserting Into Logs
            mySqlCmd = New SqlCommand("sp_UpdateServiceConfirmation", mySqlConn, sqlTrans)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(txtRequestId.Text.Trim, String)
            mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.VarChar, 20)).Value = CType(txtRlineNo.Text.Trim, String)
            mySqlCmd.Parameters.Add(New SqlParameter("@roomno", SqlDbType.Int)).Value = iRoomNo
            mySqlCmd.Parameters.Add(New SqlParameter("@confno", SqlDbType.VarChar, 20)).Value = CType(txtConfirmNo.Text.Trim, String)
            mySqlCmd.Parameters.Add(New SqlParameter("@canceltimelimit", SqlDbType.VarChar, 20)).Value = Format(CType(txtConfirmTimeLimit.Text, Date), "yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar, 20)).Value = CType(lblServiceType.Text.Trim, String)
            mySqlCmd.Parameters.Add(New SqlParameter("@loggeduser", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
            mySqlCmd.ExecuteNonQuery()


            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)

            lblLineStatus.Text = "Confirmed"
            lblConfNo.Text = txtConfirmNo.Text
            txtConfirmNo.Visible = False
            txtConfirmTimeLimit.Visible = False
            btnConfirmSave.Visible = False
            btnConfirmCancel.Visible = False
            ImgBtnConfirmTimeLimit.Visible = False

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BookingTray.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

        'Dim btnConfirmSave As Button = CType(gvr.FindControl("btnConfirmSave"), Button)
        'Dim lbConfirm As LinkButton = CType(gvr.FindControl("lbConfirm"), LinkButton)
        'Dim ImgBtnConfirmTimeLimit As ImageButton = CType(gvr.FindControl("ImgBtnConfirmTimeLimit"), ImageButton)
    End Sub

    Protected Sub btnConfirmCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btnConfirmCancel As Button = CType(sender, Button)
        Dim gvr As GridViewRow = btnConfirmCancel.NamingContainer
        Dim lblLineStatus As Label = CType(gvr.FindControl("lblLineStatus"), Label)
        Dim txtConfirmNo As TextBox = CType(gvr.FindControl("txtConfirmNo"), TextBox)
        Dim txtConfirmTimeLimit As TextBox = CType(gvr.FindControl("txtConfirmTimeLimit"), TextBox)
        Dim btnConfirmSave As Button = CType(gvr.FindControl("btnConfirmSave"), Button)
        Dim lbConfirm As LinkButton = CType(gvr.FindControl("lbConfirm"), LinkButton)
        Dim ImgBtnConfirmTimeLimit As ImageButton = CType(gvr.FindControl("ImgBtnConfirmTimeLimit"), ImageButton)
        If lblLineStatus.Text = "Confirmed" Then
            lbConfirm.Visible = False
        Else
            lbConfirm.Visible = True
        End If

        txtConfirmNo.Visible = False
        txtConfirmTimeLimit.Visible = False
        btnConfirmSave.Visible = False
        btnConfirmCancel.Visible = False
        ImgBtnConfirmTimeLimit.Visible = False

    End Sub


    Protected Sub lbConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim lbConfirm As LinkButton = CType(sender, LinkButton)
        Dim gvr As GridViewRow = lbConfirm.NamingContainer
        Dim lblLineStatus As Label = CType(gvr.FindControl("lblLineStatus"), Label)
        Dim txtConfirmNo As TextBox = CType(gvr.FindControl("txtConfirmNo"), TextBox)
        Dim txtConfirmTimeLimit As TextBox = CType(gvr.FindControl("txtConfirmTimeLimit"), TextBox)
        Dim btnConfirmSave As Button = CType(gvr.FindControl("btnConfirmSave"), Button)
        Dim btnConfirmCancel As Button = CType(gvr.FindControl("btnConfirmCancel"), Button)
        Dim ImgBtnConfirmTimeLimit As ImageButton = CType(gvr.FindControl("ImgBtnConfirmTimeLimit"), ImageButton)
        If lblLineStatus.Text = "Confirmed" Then
            lbConfirm.Visible = False
            txtConfirmNo.Visible = False
            txtConfirmTimeLimit.Visible = False
            btnConfirmSave.Visible = False
            btnConfirmCancel.Visible = False
            ImgBtnConfirmTimeLimit.Visible = False
        Else
            lbConfirm.Visible = False
            txtConfirmNo.Visible = True
            txtConfirmTimeLimit.Visible = True
            btnConfirmSave.Visible = True
            btnConfirmCancel.Visible = True
            ImgBtnConfirmTimeLimit.Visible = True
        End If
    End Sub

    Protected Sub lbUpdateGuest_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        'Dim modalSelectGuest As AjaxControlToolkit.ModalPopupExtender
        'modalSelectGuest = CType(FindControl("modalSelectGuest"), AjaxControlToolkit.ModalPopupExtender)

        Dim lbUpdateGuest As LinkButton = CType(sender, LinkButton)
        Dim gvr As GridViewRow = lbUpdateGuest.NamingContainer
        Dim lblRequestId As Label = CType(gvr.FindControl("lblRequestId"), Label)
        Dim strQuery As String = "select * from booking_guest where requestid='" & lblRequestId.Text & "'"
        Dim dt As DataTable

        Dim objclsUtilities As New clsUtils
        dt = objclsUtilities.GetDataFromDataTable(strQuery)
        gvGuestDetails.DataSource = dt
        gvGuestDetails.DataBind()
        modalSelectGuest.Show()

    End Sub




    Protected Sub gvGuestDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGuestDetails.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblGuestType As Label = CType(e.Row.FindControl("lblGuestType"), Label)
            Dim lbltitle As Label = CType(e.Row.FindControl("lbltitle"), Label)
            Dim ddlTittle As DropDownList = CType(e.Row.FindControl("ddlGuestTittle"), DropDownList)

            If lblGuestType.Text = "Adult" Then
                ddlTittle.Items.Add(New System.Web.UI.WebControls.ListItem("--", "0"))
                ddlTittle.Items.Add(New System.Web.UI.WebControls.ListItem("Mr", "Mr"))
                ddlTittle.Items.Add(New System.Web.UI.WebControls.ListItem("Mrs", "Mrs"))
                ddlTittle.Items.Add(New System.Web.UI.WebControls.ListItem("Ms", "Ms"))
            Else
                ddlTittle.Items.Add(New System.Web.UI.WebControls.ListItem("--", "0"))
                ddlTittle.Items.Add(New System.Web.UI.WebControls.ListItem("Child Male", "Child Male"))
                ddlTittle.Items.Add(New System.Web.UI.WebControls.ListItem("Child Female", "Child Female"))
            End If
            ddlTittle.SelectedValue = lbltitle.Text
        End If

    End Sub


    Protected Sub btnGuestSave_Click(sender As Object, e As System.EventArgs) Handles btnGuestSave.Click
        For Each gvrow As GridViewRow In gvGuestDetails.Rows
            Dim lblRequestId As Label = CType(gvrow.FindControl("lblRequestId"), Label)
            Dim lblRlineno As Label = CType(gvrow.FindControl("lblRlineno"), Label)
            Dim lblroomNo As Label = CType(gvrow.FindControl("lblroomNo"), Label)
            Dim lblGuestLineNo As Label = CType(gvrow.FindControl("lblGuestLineNo"), Label)

            Dim ddlGuestTittle As DropDownList = CType(gvrow.FindControl("ddlGuestTittle"), DropDownList)
            Dim txtFirstName As TextBox = CType(gvrow.FindControl("txtFirstName"), TextBox)
            Dim txtMiddeleName As TextBox = CType(gvrow.FindControl("txtMiddeleName"), TextBox)
            Dim txtlastname As TextBox = CType(gvrow.FindControl("txtlastname"), TextBox)
            Dim txtHotelRoomNo As TextBox = CType(gvrow.FindControl("txtHotelRoomNo"), TextBox)
            Dim txtMobileNo As TextBox = CType(gvrow.FindControl("txtMobileNo"), TextBox)

            If ddlGuestTittle.SelectedValue = "0" Or txtFirstName.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter tittle or first name.' );", True)
                modalSelectGuest.Show()
                Exit Sub
            End If

            Dim mySqlConn As SqlConnection
            Dim sqlTrans As SqlTransaction
            Dim mySqlCmd As SqlCommand
            Try



                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                'Inserting Into Logs
                mySqlCmd = New SqlCommand("sp_UpdateGuestDetails", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(lblRequestId.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.VarChar, 20)).Value = CType(lblRlineno.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@roomno", SqlDbType.Int)).Value = lblroomNo.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@guestlineno", SqlDbType.Int)).Value = lblGuestLineNo.Text

                mySqlCmd.Parameters.Add(New SqlParameter("@Title", SqlDbType.VarChar, 20)).Value = ddlGuestTittle.SelectedValue
                mySqlCmd.Parameters.Add(New SqlParameter("@firstname", SqlDbType.VarChar, 200)).Value = txtFirstName.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@middlename", SqlDbType.VarChar, 200)).Value = txtMiddeleName.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@lastname", SqlDbType.VarChar, 200)).Value = txtlastname.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@hotelroomno", SqlDbType.VarChar, 20)).Value = txtHotelRoomNo.Text
                mySqlCmd.Parameters.Add(New SqlParameter("@updateduser", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.Parameters.Add(New SqlParameter("@mobileno", SqlDbType.VarChar, 20)).Value = txtMobileNo.Text
                mySqlCmd.ExecuteNonQuery()


                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)


            Catch ex As Exception
                If mySqlConn.State = ConnectionState.Open Then
                    sqlTrans.Rollback()
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("BookingTray.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        Next

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Hotel Room Number Updated Succesfully.' );", True)


    End Sub


    Protected Sub lbViewFlight_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dvBookingRemarks.Visible = False
        dvflightDetailsPopup.Visible = True
        lblViewDetailsPopupHeading.Text = "Flight Details"
        Dim lbViewFlight As LinkButton = CType(sender, LinkButton)
        Dim gvr As GridViewRow = lbViewFlight.NamingContainer
        Dim lblRequestId As Label = CType(gvr.FindControl("lblRequestId"), Label)
        Dim strQuery As String = "select g.Requestid,g.RLineno,g.GuestLineNo, bg.title+ ' '+ bg.firstname +' '+bg.middlename + ' '+ bg.lastname GuestName,convert(varchar(10),arrdate,103)ArrivalDate,arrflightcode ArrivalFlightCode,arrflighttime, (select airportbordername from airportbordersmaster a where a.airportbordercode=g.arrairportbordercode)ArrivalAirport,convert(varchar(10),depdate,103)depdate,depflightcode,depflighttime, (select airportbordername from airportbordersmaster a where a.airportbordercode=g.depairportbordercode)DepartureAirport from booking_guest_flights(nolock) g left outer join booking_guest(nolock) bg on g.requestid=bg.requestid and g.rlineno=bg.rlineno and g.guestlineno=bg.guestlineno where g.requestid='" & lblRequestId.Text & "'" & " order by g.rlineno,g.guestlineno"
        Dim dt As DataTable

        Dim objclsUtilities As New clsUtils
        dt = objclsUtilities.GetDataFromDataTable(strQuery)
        gvFlightDetails.DataSource = dt
        If dt.Rows.Count > 0 Then
            gvFlightDetails.DataBind()
            ModalFlightDetails.Show()
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Flight details is not available.' );", True)

        End If


    End Sub


    
    Protected Sub lbViewBookingRemarks_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblViewDetailsPopupHeading.Text = "Booking Remarks"
        dvBookingRemarks.Visible = True
        dvflightDetailsPopup.Visible = False
        Dim lbViewFlight As LinkButton = CType(sender, LinkButton)
        Dim gvr As GridViewRow = lbViewFlight.NamingContainer
        Dim lblRequestId As Label = CType(gvr.FindControl("lblRequestId"), Label)
        Dim strQuery As String = "select * from view_booking_remarks_all where requestid='" & lblRequestId.Text & "' order by ServiceDate"
        Dim dt As DataTable

        Dim objclsUtilities As New clsUtils
        dt = objclsUtilities.GetDataFromDataTable(strQuery)

        If dt.Rows.Count > 0 Then
            gvBookingRemarks.DataSource = dt
            gvBookingRemarks.DataBind()
            ModalFlightDetails.Show()
        Else
            gvBookingRemarks.DataBind()
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Booking remarks is not available.' );", True)

        End If


    End Sub
    '

    Protected Sub ddlServiceType_TextChanged(sender As Object, e As System.EventArgs) Handles ddlServiceType.TextChanged
        If ddlServiceType.SelectedValue = "Transfers" Or ddlServiceType.SelectedValue = "Airport Services" Then
            dvTransferType.Visible = True
        Else
            dvTransferType.Visible = False
        End If
    End Sub


    Protected Sub gvBookingRemarks_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBookingRemarks.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblPartyremarks As Label = CType(e.Row.FindControl("lblPartyremarks"), Label)
            Dim lblAgentremarks As Label = CType(e.Row.FindControl("lblAgentremarks"), Label)
            Dim lblArrivalremarks As Label = CType(e.Row.FindControl("lblArrivalremarks"), Label)
            Dim lblDepartureRemarks As Label = CType(e.Row.FindControl("lblDepartureRemarks"), Label)

            Dim txtPartyremarks As TextBox = CType(e.Row.FindControl("txtPartyremarks"), TextBox)
            Dim txtAgentremarks As TextBox = CType(e.Row.FindControl("txtAgentremarks"), TextBox)
            Dim txtArrivalremarks As TextBox = CType(e.Row.FindControl("txtArrivalremarks"), TextBox)
            Dim txtDepartureRemarks As TextBox = CType(e.Row.FindControl("txtDepartureRemarks"), TextBox)

            Dim btnRemarksEdit As Button = CType(e.Row.FindControl("btnRemarksEdit"), Button)
            Dim btnRemarksUpdate As Button = CType(e.Row.FindControl("btnRemarksUpdate"), Button)
            Dim btnRemarksCancel As Button = CType(e.Row.FindControl("btnRemarksCancel"), Button)

            txtPartyremarks.Visible = False
            txtAgentremarks.Visible = False
            txtArrivalremarks.Visible = False
            txtDepartureRemarks.Visible = False

            btnRemarksUpdate.Visible = False
            btnRemarksCancel.Visible = False



        End If
    End Sub

    Protected Sub btnRemarksEdit_Click(sender As Object, e As System.EventArgs)
        Dim btnRemarksEdit As Button = CType(sender, Button)
        Dim gvr As GridViewRow = btnRemarksEdit.NamingContainer


        Dim lblPartyremarks As Label = CType(gvr.FindControl("lblPartyremarks"), Label)
        Dim lblAgentremarks As Label = CType(gvr.FindControl("lblAgentremarks"), Label)
        Dim lblArrivalremarks As Label = CType(gvr.FindControl("lblArrivalremarks"), Label)
        Dim lblDepartureRemarks As Label = CType(gvr.FindControl("lblDepartureRemarks"), Label)

        Dim txtPartyremarks As TextBox = CType(gvr.FindControl("txtPartyremarks"), TextBox)
        Dim txtAgentremarks As TextBox = CType(gvr.FindControl("txtAgentremarks"), TextBox)
        Dim txtArrivalremarks As TextBox = CType(gvr.FindControl("txtArrivalremarks"), TextBox)
        Dim txtDepartureRemarks As TextBox = CType(gvr.FindControl("txtDepartureRemarks"), TextBox)


        Dim btnRemarksUpdate As Button = CType(gvr.FindControl("btnRemarksUpdate"), Button)
        Dim btnRemarksCancel As Button = CType(gvr.FindControl("btnRemarksCancel"), Button)

        txtPartyremarks.Visible = True
        txtAgentremarks.Visible = True
        txtArrivalremarks.Visible = True
        txtDepartureRemarks.Visible = True

        lblPartyremarks.Visible = False
        lblAgentremarks.Visible = False
        lblArrivalremarks.Visible = False
        lblDepartureRemarks.Visible = False

        btnRemarksEdit.Visible = False
        btnRemarksUpdate.Visible = True
        btnRemarksCancel.Visible = True

        Dim lblServiceType As Label = CType(gvr.FindControl("lblServiceType"), Label)
        If lblServiceType.Text = "Visa" Then
            txtPartyremarks.Visible = False
            txtAgentremarks.Visible = False

            txtDepartureRemarks.Visible = False

        End If
        ModalFlightDetails.Show()
    End Sub

    Protected Sub btnRemarksUpdate_Click(sender As Object, e As System.EventArgs)

        Dim btnRemarksUpdate As Button = CType(sender, Button)
        Dim gvr As GridViewRow = btnRemarksUpdate.NamingContainer


        Dim lblPartyremarks As Label = CType(gvr.FindControl("lblPartyremarks"), Label)
        Dim lblAgentremarks As Label = CType(gvr.FindControl("lblAgentremarks"), Label)
        Dim lblArrivalremarks As Label = CType(gvr.FindControl("lblArrivalremarks"), Label)
        Dim lblDepartureRemarks As Label = CType(gvr.FindControl("lblDepartureRemarks"), Label)

        Dim txtPartyremarks As TextBox = CType(gvr.FindControl("txtPartyremarks"), TextBox)
        Dim txtAgentremarks As TextBox = CType(gvr.FindControl("txtAgentremarks"), TextBox)
        Dim txtArrivalremarks As TextBox = CType(gvr.FindControl("txtArrivalremarks"), TextBox)
        Dim txtDepartureRemarks As TextBox = CType(gvr.FindControl("txtDepartureRemarks"), TextBox)


        Dim btnRemarksEdit As Button = CType(gvr.FindControl("btnRemarksEdit"), Button)
        Dim btnRemarksCancel As Button = CType(gvr.FindControl("btnRemarksCancel"), Button)

        txtPartyremarks.Visible = False
        txtAgentremarks.Visible = False
        txtArrivalremarks.Visible = False
        txtDepartureRemarks.Visible = False

        lblPartyremarks.Visible = True
        lblAgentremarks.Visible = True
        lblArrivalremarks.Visible = True
        lblDepartureRemarks.Visible = True

        btnRemarksEdit.Visible = True
        btnRemarksUpdate.Visible = False
        btnRemarksCancel.Visible = False


        Dim lblRequestId As Label = CType(gvr.FindControl("lblRequestId"), Label)
        Dim lblServiceType As Label = CType(gvr.FindControl("lblServiceType"), Label)
        Dim lblRLineno As Label = CType(gvr.FindControl("lblRLineno"), Label)


        Dim sqlTrans As SqlTransaction

        Try
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            sqlTrans = SqlConn.BeginTransaction
            myCommand = New SqlCommand("sp_UpdateBookingRemarks", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = lblRequestId.Text
            myCommand.Parameters.Add(New SqlParameter("@ServiceType", SqlDbType.VarChar, 100)).Value = Convert.ToString(lblServiceType.Text.Trim)
            myCommand.Parameters.Add(New SqlParameter("@RLineno", SqlDbType.Int)).Value = lblRLineno.Text
            myCommand.Parameters.Add(New SqlParameter("@Partyremarks", SqlDbType.VarChar, 2000)).Value = txtPartyremarks.Text
            myCommand.Parameters.Add(New SqlParameter("@Agentremarks", SqlDbType.VarChar, 2000)).Value = txtAgentremarks.Text
            myCommand.Parameters.Add(New SqlParameter("@Arrivalremarks", SqlDbType.VarChar, 2000)).Value = txtArrivalremarks.Text
            myCommand.Parameters.Add(New SqlParameter("@DepartureRemarks", SqlDbType.VarChar, 2000)).Value = txtDepartureRemarks.Text
            myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
            myCommand.ExecuteNonQuery()
            sqlTrans.Commit()
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbConnectionClose(SqlConn)






            lblPartyremarks.Text = txtPartyremarks.Text
            lblAgentremarks.Text = txtAgentremarks.Text
            lblArrivalremarks.Text = txtArrivalremarks.Text
            lblDepartureRemarks.Text = txtDepartureRemarks.Text

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Updated Succesfully. " & "' );", True)
        Catch ex As Exception
            If Not SqlConn Is Nothing Then
                If SqlConn.State = ConnectionState.Open Then
                    If Not sqlTrans Is Nothing Then
                        sqlTrans.Rollback()
                        sqlTrans.Dispose()
                    End If
                    clsDBConnect.dbCommandClose(myCommand)
                    clsDBConnect.dbConnectionClose(SqlConn)
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + Regex.Replace(ex.Message, "[^a-zA-Z0-9_@.-]", " ") & "' );", True)
            objUtils.WritErrorLog("BookingTray.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

        ModalFlightDetails.Show()


    End Sub

    Protected Sub btnRemarksCancel_Click(sender As Object, e As System.EventArgs)

        Dim btnRemarksCancel As Button = CType(sender, Button)
        Dim gvr As GridViewRow = btnRemarksCancel.NamingContainer


        Dim lblPartyremarks As Label = CType(gvr.FindControl("lblPartyremarks"), Label)
        Dim lblAgentremarks As Label = CType(gvr.FindControl("lblAgentremarks"), Label)
        Dim lblArrivalremarks As Label = CType(gvr.FindControl("lblArrivalremarks"), Label)
        Dim lblDepartureRemarks As Label = CType(gvr.FindControl("lblDepartureRemarks"), Label)

        Dim txtPartyremarks As TextBox = CType(gvr.FindControl("txtPartyremarks"), TextBox)
        Dim txtAgentremarks As TextBox = CType(gvr.FindControl("txtAgentremarks"), TextBox)
        Dim txtArrivalremarks As TextBox = CType(gvr.FindControl("txtArrivalremarks"), TextBox)
        Dim txtDepartureRemarks As TextBox = CType(gvr.FindControl("txtDepartureRemarks"), TextBox)


        Dim btnRemarksEdit As Button = CType(gvr.FindControl("btnRemarksEdit"), Button)
        Dim btnRemarksUpdate As Button = CType(gvr.FindControl("btnRemarksUpdate"), Button)

        txtPartyremarks.Visible = False
        txtAgentremarks.Visible = False
        txtArrivalremarks.Visible = False
        txtDepartureRemarks.Visible = False

        lblPartyremarks.Visible = True
        lblPartyremarks.Visible = True
        lblPartyremarks.Visible = True
        lblPartyremarks.Visible = True

        btnRemarksEdit.Visible = True
        btnRemarksUpdate.Visible = False
        btnRemarksCancel.Visible = False
        ModalFlightDetails.Show()
    End Sub

    Protected Sub lbExcusrionTicket_Click(sender As Object, e As System.EventArgs)

        Dim lbtnSequenceNo As LinkButton = CType(sender, LinkButton)
        Dim gvr As GridViewRow = lbtnSequenceNo.NamingContainer

        Dim txtRequestId As TextBox = CType(gvr.FindControl("txtRequestId"), TextBox)
        Dim txtRlineNo As TextBox = CType(gvr.FindControl("txtRlineNo"), TextBox)

        Dim strpop As String = "window.open('PrintReport.aspx?RequestId=" & txtRequestId.Text.Trim & "&RlineNo=" & txtRlineNo.Text.Trim & "&printId=ExcursionTicket');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


    End Sub

    Protected Sub lbUpdateFlight_Click(sender As Object, e As System.EventArgs)

        Dim lbtnSequenceNo As LinkButton = CType(sender, LinkButton)
        Dim gvr As GridViewRow = lbtnSequenceNo.NamingContainer

        Session("slbUpdateFlight") = lbtnSequenceNo

        Dim txtRequestId As TextBox = CType(gvr.FindControl("txtRequestId"), TextBox)
        Dim txtRlineNo As TextBox = CType(gvr.FindControl("txtRlineNo"), TextBox)

        Dim lblTflightNo As Label = CType(gvr.FindControl("lblTflightNo"), Label)
        Dim lblTflighttime As Label = CType(gvr.FindControl("lblTflighttime"), Label)
        Dim lblAirportbordercode As Label = CType(gvr.FindControl("lblAirportbordercode"), Label)
        Dim txtTranflightCode As TextBox = CType(gvr.FindControl("txtTranflightCode"), TextBox)
        Dim lblServiceType As Label = CType(gvr.FindControl("lblServiceType"), Label)
        lblFlightRequestId.Text = txtRequestId.Text
        lblFlightRlineNo.Text = txtRlineNo.Text
        lblAirportbordercodePopup.Text = lblAirportbordercode.Text
        txtFlightCode.Text = lblTflightNo.Text
        txtFlightTime.Text = lblTflighttime.Text
        txtTranflightCodePopup.Text = txtTranflightCode.Text
        lblFlightServiceType.Text = lblServiceType.Text
        mpFlightUpdate.Show()
        '  lblAirportbordercode lblServiceType
        'Dim dvFlight As HtmlGenericControl = CType(gvr.FindControl("dvFlight"), HtmlGenericControl)
        'Dim txtFlightCode As TextBox = CType(gvr.FindControl("txtFlightCode"), TextBox)
        'Dim txtFlightTime As TextBox = CType(gvr.FindControl("txtFlightTime"), TextBox)
        'Dim lbUpdateFlight As LinkButton = CType(gvr.FindControl("lbUpdateFlight"), LinkButton)

        'Dim lblFlight As Label = CType(gvr.FindControl("lblFlight"), Label)
        'dvFlight.Visible = True
        'lbUpdateFlight.Visible = False
        'lblFlight.Visible = False

    End Sub

    Protected Sub lbUpdatePickupTime_Click(sender As Object, e As System.EventArgs)

        Dim lbtnSequenceNo As LinkButton = CType(sender, LinkButton)
        Dim gvr As GridViewRow = lbtnSequenceNo.NamingContainer

        Dim txtRequestId As TextBox = CType(gvr.FindControl("txtRequestId"), TextBox)
        Dim txtRlineNo As TextBox = CType(gvr.FindControl("txtRlineNo"), TextBox)
        Dim lblParticulars As Label = CType(gvr.FindControl("lblParticulars"), Label)
        Dim lblServiceType As Label = CType(gvr.FindControl("lblServiceType"), Label)

        lblRequestidPopup.Text = txtRequestId.Text
        lbltlinenoPopup.Text = txtRlineNo.Text


        'Dim dvTourPickup As HtmlGenericControl = CType(gvr.FindControl("dvTourPickup"), HtmlGenericControl)
        'Dim dvTransferPickup As HtmlGenericControl = CType(gvr.FindControl("dvTransferPickup"), HtmlGenericControl)

        If lblServiceType.Text.Contains("Tours") Then
            lblServiceTypePopup.Text = "TOUR"
            dvTourPickup.Visible = True
            dvTransferPickup.Visible = False

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myCommand = New SqlCommand("sp_get_excursion_pickuptime", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 0
            myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = txtRequestId.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@elineno", SqlDbType.VarChar, 20)).Value = txtRlineNo.Text.Trim
            Dim myDS As New DataSet
            myDataAdapter = New SqlDataAdapter(myCommand)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                ModalPopupExtenderPickupTime.Show()
                gvExcursionPickupTime.DataSource = myDS.Tables(0)
                gvExcursionPickupTime.DataBind()

            Else
                ModalPopupExtenderPickupTime.Hide()
                gvExcursionPickupTime.DataBind()


            End If
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)
        End If

        If lblServiceType.Text.Contains("Transfer") Or lblServiceType.Text.Contains("Airport") Then
            dvTourPickup.Visible = False
            dvTransferPickup.Visible = True
            Dim strTransferType As String = ""
            Dim strServiceType As String = ""
            If lblServiceType.Text.Contains("Transfer") Then
                strServiceType = "TRANSFER"
            End If
            If lblServiceType.Text.Contains("Airport") Then
                strServiceType = "AIRPORT"
            End If

            If lblServiceType.Text.Contains("ARRIVAL") Then
                strTransferType = "ARRIVAL"
            End If
            If lblServiceType.Text.Contains("DEPARTURE") Then
                strTransferType = "DEPARTURE"
            End If
            If lblServiceType.Text.Contains("TRANSIT") Then
                strTransferType = "TRANSIT"
            End If
            If lblServiceType.Text.Contains("INTERHOTEL") Then
                strTransferType = "INTERHOTEL"
            End If
            lbltransfertypePopup.Text = strTransferType
            lblServiceTypePopup.Text = strServiceType
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myCommand = New SqlCommand("sp_get_transfers_Airport_Pickuptime", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 0
            myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = txtRequestId.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@tlineno", SqlDbType.VarChar, 20)).Value = txtRlineNo.Text.Trim
            myCommand.Parameters.Add(New SqlParameter("@transfertype", SqlDbType.VarChar, 20)).Value = strTransferType.Trim
            myCommand.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar, 20)).Value = strServiceType.Trim
            Dim myDt As New DataTable
            myDataAdapter = New SqlDataAdapter(myCommand)
            myDataAdapter.Fill(myDt)
            If myDt.Rows.Count > 0 Then
                txtTransferPickupTime.Text = myDt.Rows(0)("pickuptime").ToString
                ModalPopupExtenderPickupTime.Show()
            Else
                ModalPopupExtenderPickupTime.Hide()
            End If

            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)
        End If


    End Sub


    Protected Sub btnFlightSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)


        'Dim btnFlightSave As Button = CType(sender, Button)
        'Dim gvr As GridViewRow = btnFlightSave.NamingContainer
        'Dim txtRequestId As TextBox = CType(gvr.FindControl("txtRequestId"), TextBox)
        'Dim txtRlineNo As TextBox = CType(gvr.FindControl("txtRlineNo"), TextBox)
        'Dim lblServiceType As Label = CType(gvr.FindControl("lblServiceType"), Label)
        'Dim lblFlight As Label = CType(gvr.FindControl("lblFlight"), Label)

        'Dim txtTranflightCode As TextBox = CType(gvr.FindControl("txtTranflightCode"), TextBox)
        'Dim txtFlightTime As TextBox = CType(gvr.FindControl("txtFlightTime"), TextBox)
        'Dim txtFlightCode As TextBox = CType(gvr.FindControl("txtFlightCode"), TextBox)

        If txtTranflightCodePopup.Text = "" Or txtFlightTime.Text = "" Or txtFlightCode.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Please select flight code or time. " & "' );", True)
            Exit Sub
        End If


        Dim mySqlConn As SqlConnection
        Dim sqlTrans As SqlTransaction
        Dim mySqlCmd As SqlCommand

        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
        sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

        'Inserting Into Logs
        mySqlCmd = New SqlCommand("sp_UpdateFlightDetils", mySqlConn, sqlTrans)
        mySqlCmd.CommandType = CommandType.StoredProcedure
        mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(lblFlightRequestId.Text.Trim, String)
        mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.VarChar, 20)).Value = CType(lblFlightRlineNo.Text.Trim, String)
        mySqlCmd.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar, 20)).Value = CType(lblFlightServiceType.Text.Trim, String)
        mySqlCmd.Parameters.Add(New SqlParameter("@flightcode", SqlDbType.VarChar, 20)).Value = CType(txtFlightCode.Text.Trim, String)
        mySqlCmd.Parameters.Add(New SqlParameter("@flight_tranid", SqlDbType.VarChar, 20)).Value = CType(txtTranflightCodePopup.Text.Trim, String)
        mySqlCmd.Parameters.Add(New SqlParameter("@flighttime", SqlDbType.VarChar, 20)).Value = CType(txtFlightTime.Text.Trim, String)
        mySqlCmd.ExecuteNonQuery()


        sqlTrans.Commit()    'SQl Tarn Commit
        clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
        clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
        clsDBConnect.dbConnectionClose(mySqlConn)



        Dim slbUpdateFlight As LinkButton
        slbUpdateFlight = Session("slbUpdateFlight")
        Dim gvr As GridViewRow = slbUpdateFlight.NamingContainer
        Dim gv As GridView = CType((gvr.Parent.Parent), GridView)
        'Dim gvHeaderRow As GridViewRow = CType(gv.NamingContainer, GridViewRow)
        'Dim lbtnSequenceNo As LinkButton = CType(gvHeaderRow.FindControl("lbtnSequenceNo"), LinkButton)
        'Dim gvHotelDetail As GridView = CType(gvHeaderRow.FindControl("gvHotelDetail"), GridView)

        'lbtnSequenceNo.CommandArgument = "Show"
        'lbtnSequenceNo_Click(sender, e)
        ''If lbtnSequenceNo.CommandArgument = "Show" Then

        ''End If

        'Dim lblFlight As Label = CType(gvr.FindControl("lblFlight"), Label)
        'lblFlight.Text = txtFlightCode.Text.Trim & " " & txtFlightTime.Text.Trim
        'Dim txtTranflightCode As TextBox = CType(gvr.FindControl("txtTranflightCode"), TextBox)
        'txtTranflightCode.Text = txtTranflightCodePopup.Text
        'Dim lblTflightNo As Label = CType(gvr.FindControl("lblTflightNo"), Label)
        'Dim lblTflighttime As Label = CType(gvr.FindControl("lblTflighttime"), Label)
        'lblTflightNo.Text = txtFlightCode.Text.Trim
        'lblTflighttime.Text = txtFlightTime.Text.Trim


        Dim lblFlight As Label = CType(gv.Rows(gvr.RowIndex).FindControl("lblFlight"), Label)
        lblFlight.Text = txtFlightCode.Text.Trim & " " & txtFlightTime.Text.Trim
        Dim txtTranflightCode As TextBox = CType(gv.Rows(gvr.RowIndex).FindControl("txtTranflightCode"), TextBox)
        txtTranflightCode.Text = txtTranflightCodePopup.Text
        Dim lblTflightNo As Label = CType(gv.Rows(gvr.RowIndex).FindControl("lblTflightNo"), Label)
        Dim lblTflighttime As Label = CType(gv.Rows(gvr.RowIndex).FindControl("lblTflighttime"), Label)
        lblTflightNo.Text = txtFlightCode.Text.Trim
        lblTflighttime.Text = txtFlightTime.Text.Trim

               
    End Sub
    Protected Sub btnFlightCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        'Dim btnFlightCancel As Button = CType(sender, Button)
        'Dim gvr As GridViewRow = btnFlightCancel.NamingContainer
        ''Dim lblLineStatus As Label = CType(gvr.FindControl("lblLineStatus"), Label)

        'Dim dvFlight As HtmlGenericControl = CType(gvr.FindControl("dvFlight"), HtmlGenericControl)
        'Dim txtFlightCode As TextBox = CType(gvr.FindControl("txtFlightCode"), TextBox)
        'Dim txtFlightTime As TextBox = CType(gvr.FindControl("txtFlightTime"), TextBox)
        'Dim lbUpdateFlight As LinkButton = CType(gvr.FindControl("lbUpdateFlight"), LinkButton)
        'Dim lblServiceType As Label = CType(gvr.FindControl("lblServiceType"), Label)
        'Dim lblFlight As Label = CType(gvr.FindControl("lblFlight"), Label)
        'dvFlight.Visible = False
        'lbUpdateFlight.Visible = True
        'lblFlight.Visible = True

    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function Getflight(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Arrivalflight As New List(Of String)
        Try

            If prefixText = " " Then
                prefixText = ""
            End If
            Dim str As String() = contextKey.Split("|")
            Dim strServiceType As String = ""
            Dim strAirportborderCode As String = ""
            If str.Length = 2 Then
                Dim strTemp As String = str(0)
                If strTemp.Contains("ARRIVAL") Then
                    strServiceType = "ARRIVAL"
                ElseIf strTemp.Contains("DEPARTURE") Then
                    strServiceType = "DEPARTURE"
                Else
                    strServiceType = "ARRIVAL"
                End If


                strAirportborderCode = str(1)
            End If

            If strServiceType = "DEPARTURE" Then
                strSqlQry = "select distinct flight_tranid,flightcode from view_flightmast_departure where  airportbordercode='" & strAirportborderCode & "' and flightcode like  '" & prefixText & "%' "
            Else
                strSqlQry = "select  distinct flight_tranid,flightcode from view_flightmast_arrival where  airportbordercode='" & strAirportborderCode & "' and flightcode like  '" & prefixText & "%'  "
            End If


            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    'Arrivalflight.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("flightcode").ToString(), myDS.Tables(0).Rows(i)("flight_tranid").ToString() & "||" & myDS.Tables(0).Rows(i)("destintime").ToString()))
                    Arrivalflight.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("flightcode").ToString(), myDS.Tables(0).Rows(i)("flight_tranid").ToString()))
                Next
    
            End If

            Return Arrivalflight
        Catch ex As Exception
            Return Arrivalflight
        End Try

    End Function
    <WebMethod()> _
    Public Shared Function GetAirportAndTimeDetails(ByVal flightcode As String) As String
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet

        Try
            Dim strFlightCode As String
            'Dim strDayName As String
            'If flightcode <> "" Then
            '    Dim strFlightCodes As String() = flightcode.Split("|")
            '    strFlightCode = strFlightCodes(0)
            '    strDayName = strFlightCodes(1)
            'End If

            'strSqlQry = "select distinct destintime from view_flightmast_arrival where flight_tranid='" & strFlightCode & "' and fldayofweek='" & strDayName & "'"
            strSqlQry = "select top 1  destintime from view_flightmast where flight_tranid='" & flightcode & "' "

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS, "Customers")

            Return myDS.GetXml()
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Protected Sub btnUpdatePickTime_Click(sender As Object, e As System.EventArgs) Handles btnUpdatePickTime.Click
        Try


            Dim strServiceType As String = ""
            If lblServiceTypePopup.Text = "TOUR" Then
                If gvExcursionPickupTime.Rows.Count > 0 Then
                    Dim strBuffer As New StringBuilder
                    strBuffer.Append("<DocumentElement>")


                    For Each gvrow As GridViewRow In gvExcursionPickupTime.Rows
                        Dim txtPickupTime As TextBox = CType(gvrow.FindControl("txtPickupTime"), TextBox)
                        Dim txtReturnPickupTime As TextBox = CType(gvrow.FindControl("txtReturnPickupTime"), TextBox)
                        If txtPickupTime.Text.Trim <> "" Or txtReturnPickupTime.Text.Trim <> "" Then
                            Dim lblRequestId As Label = CType(gvrow.FindControl("lblRequestId"), Label)
                            Dim lblRlineno As Label = CType(gvrow.FindControl("lblRlineno"), Label)
                            Dim lblExcTypeCode As Label = CType(gvrow.FindControl("lblExcTypeCode"), Label)
                            Dim lblServiceDate As Label = CType(gvrow.FindControl("lblServiceDate"), Label)

                            strBuffer.Append("<Table>")
                            strBuffer.Append("<RequestId>" & lblRequestId.Text.Trim & "</RequestId>")
                            strBuffer.Append("<elineno>" & lblRlineno.Text.Trim & "</elineno>")
                            strBuffer.Append("<ExcTypeCode>" & lblExcTypeCode.Text.Trim & "</ExcTypeCode>")
                            strBuffer.Append("<ServiceDate>" & lblServiceDate.Text.Trim & "</ServiceDate>")
                            strBuffer.Append("<PickupTime>" & txtPickupTime.Text.Trim & "</PickupTime>")
                            strBuffer.Append("<ReturnPickupTime>" & txtReturnPickupTime.Text.Trim & "</ReturnPickupTime>")
                            strBuffer.Append("</Table>")

                        End If
                    Next
                    strBuffer.Append("</DocumentElement>")
                    Dim mySqlConn As SqlConnection
                    Dim sqlTrans As SqlTransaction
                    Dim mySqlCmd As SqlCommand

                    Try
                        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                        sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                        'Inserting Into Logs
                        mySqlCmd = New SqlCommand("sp_UpdateExcursionPickupTime", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@XMLExcursionPickupTime", SqlDbType.Xml)).Value = CType(strBuffer.ToString, String)
                        mySqlCmd.ExecuteNonQuery()
                        sqlTrans.Commit()    'SQl Tarn Commit
                        clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                        clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                        clsDBConnect.dbConnectionClose(mySqlConn)
                    Catch ex As Exception
                        If mySqlConn.State = ConnectionState.Open Then
                            sqlTrans.Rollback()
                        End If
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                        objUtils.WritErrorLog("BookingTray.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
                    End Try
 
                End If
            End If
            If lblServiceTypePopup.Text = "TRANSFER" Or lblServiceTypePopup.Text = "AIRPORT" Then

                If txtTransferPickupTime.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter pickup time.' );", True)
                    Exit Sub
                End If

                Dim mySqlConn As SqlConnection
                Dim sqlTrans As SqlTransaction
                Dim mySqlCmd As SqlCommand
                Try
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    'Inserting Into Logs
                    mySqlCmd = New SqlCommand("sp_update_transfers_Airport_Pickuptime", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = lblRequestidPopup.Text.Trim
                    mySqlCmd.Parameters.Add(New SqlParameter("@tlineno", SqlDbType.VarChar, 20)).Value = lbltlinenoPopup.Text.Trim
                    mySqlCmd.Parameters.Add(New SqlParameter("@transfertype", SqlDbType.VarChar, 20)).Value = lbltransfertypePopup.Text.Trim
                    mySqlCmd.Parameters.Add(New SqlParameter("@servicetype", SqlDbType.VarChar, 20)).Value = lblServiceTypePopup.Text.Trim
                    mySqlCmd.Parameters.Add(New SqlParameter("@pickuptime", SqlDbType.VarChar, 20)).Value = txtTransferPickupTime.Text.Trim
                    mySqlCmd.ExecuteNonQuery()
                    sqlTrans.Commit()    'SQl Tarn Commit
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)
                Catch ex As Exception
                    If mySqlConn.State = ConnectionState.Open Then
                        sqlTrans.Rollback()
                    End If
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                    objUtils.WritErrorLog("BookingTray.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
                End Try

            End If

            
        Catch ex As Exception
           
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BookingTray.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub lbGRNote_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim lbGRNote As LinkButton = CType(sender, LinkButton)
        Dim gvr As GridViewRow = lbGRNote.NamingContainer
        Dim lblRequestId As Label = CType(gvr.FindControl("lblRequestId"), Label)
        lblGRNoteRequestId.Text = lblRequestId.Text
        Dim strQuery As String = "select * from tblGRNotes where requestid='" & lblRequestId.Text & "'"
        Dim dt As DataTable

        Dim objclsUtilities As New clsUtils
        dt = objclsUtilities.GetDataFromDataTable(strQuery)

        If dt.Rows.Count > 0 Then
            txtGRNote.Text = dt.Rows(0)("Note").ToString
        Else
            txtGRNote.Text = ""
        End If
        mpGRNote.Show()
    End Sub

    Protected Sub btnGRNoteSave_Click(sender As Object, e As System.EventArgs) Handles btnGRNoteSave.Click
        Dim mySqlConn As SqlConnection
        Dim sqlTrans As SqlTransaction
        Dim mySqlCmd As SqlCommand
        Try


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

            'Inserting Into Logs
            mySqlCmd = New SqlCommand("sp_SaveGRNote", mySqlConn, sqlTrans)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(lblGRNoteRequestId.Text.Trim, String)
            mySqlCmd.Parameters.Add(New SqlParameter("@Note", SqlDbType.VarChar, 20)).Value = CType(txtGRNote.Text.Trim, String)
            mySqlCmd.Parameters.Add(New SqlParameter("@UpdatedUser", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName").Trim, String)
            mySqlCmd.ExecuteNonQuery()

            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("BookingTray.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
End Class
