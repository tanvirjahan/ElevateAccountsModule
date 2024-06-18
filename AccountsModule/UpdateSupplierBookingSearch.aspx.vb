Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic


Partial Class UpdateSupplierBookingSearch
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
    Dim sqlTrans As SqlTransaction
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
            strSqlQry = "select agentcode,agentname from agentmast(nolock) where active=1 and divcode='" & contextKey & "' and agentname like '" & prefixText & "%' order by agentname"

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
        requestid = 0
        requestDate = 1
        agentname = 2
        agentref = 3
        hotelref = 4
        hotelname = 5
        guestname = 6
        addDate = 7
        addUser = 8
        modDate = 9
        modUser = 10
        edit = 11
        print = 12
    End Enum
#End Region

#Region "Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim appid As String = CType(Request.QueryString("appid"), String)

        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        Dim strappname As String = ""
        strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(displayName,'') as displayname from appmaster a inner join division_master d on a.displayname=d.accountsmodulename where a.appid='" & appid & "'")
        ViewState("Appname") = strappname
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")
        ViewState.Add("divcode", divid)
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
                Dim AppId As String = CType(Request.QueryString("appid"), String)

                If AppId Is Nothing = False Then
                    strappid = AppId
                End If
                If AppName Is Nothing = False Then
                    strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(displayName,'') as displayname from appmaster a inner join division_master d on a.displayname=d.accountsmodulename where a.appid='" & AppId & "'")
                    If strappname = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Accounts display name does not match with accounts module name in division master' );", True)
                        Exit Sub
                    End If
                End If

                txtDivcode.Value = ViewState("divcode")
                Page.ClientScript.RegisterHiddenField("divCode", ViewState("divcode"))

                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\UpdateSupplierBookingSearch.aspx?appid=" + strappid, btnAddNew, btnExportToExcel, _
                                                       btnPrint, gvSearch:=gvSearchResult, EditColumnNo:=GridCol.edit, PrintColumnNo:=GridCol.print)

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

                RowsPerPageCUS.SelectedValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2006'")
                Session.Add("strsortExpression", "requestId")
                Session.Add("strsortdirection", SortDirection.Descending)

                Dim dt As New DataTable
                dt.Columns.Add(New DataColumn("requestId", GetType(String)))
                dt.Columns.Add(New DataColumn("requestDate", GetType(DateAndTime)))
                dt.Columns.Add(New DataColumn("agentname", GetType(String)))
                dt.Columns.Add(New DataColumn("agentref", GetType(String)))
                dt.Columns.Add(New DataColumn("hotelconfno", GetType(String)))
                dt.Columns.Add(New DataColumn("servicename", GetType(String)))
                dt.Columns.Add(New DataColumn("guestname", GetType(String)))
                dt.Columns.Add(New DataColumn("createddate", GetType(DateAndTime)))
                dt.Columns.Add(New DataColumn("createdby", GetType(String)))
                dt.Columns.Add(New DataColumn("lastmodified", GetType(DateAndTime)))
                dt.Columns.Add(New DataColumn("modifiedby", GetType(String)))
                Session("bookingSearchResult") = dt
                gvSearchResult.DataSource = dt
                gvSearchResult.DataBind()
            Else
                Page.ClientScript.RegisterHiddenField("divCode", ViewState("divcode"))
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierBookingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Function Validation() As Boolean"
    Protected Function Validation() As Boolean
        If ddlTravelDate.SelectedValue = "Specific date" Then
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
            Dim companyCode As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select randomnumber from agentmast_whitelabel a,agentmast am " &
            "where a.agentcode=am.agentcode and a.owncompany=1 and am.divcode='" & ViewState("divcode") & "'")
            Dim myDS As New DataSet
            Dim pagevaluecus = RowsPerPageCUS.SelectedValue
            lblMsg.Visible = False
            If gvSearchResult.PageIndex < 0 Then gvSearchResult.PageIndex = 0
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myCommand = New SqlCommand("sp_booking_search", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
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
            If ddlTravelDate.SelectedValue = "Specific date" Then
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
            myCommand.Parameters.Add(New SqlParameter("@companycode", SqlDbType.VarChar, 20)).Value = companyCode     '"675558760549078"
            myCommand.Parameters.Add(New SqlParameter("@subusercode", SqlDbType.VarChar, 20)).Value = ""
            myDataAdapter = New SqlDataAdapter(myCommand)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                gvSearchResult.DataSource = myDS.Tables(0)
                gvSearchResult.PageSize = pagevaluecus
                gvSearchResult.DataBind()
            Else
                gvSearchResult.PageIndex = 0
                gvSearchResult.DataBind()
                lblMsg.Visible = True
                lblMsg.Text = "Records not found, Please redefine search criteria."
            End If
            clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            clsDBConnect.dbConnectionClose(SqlConn)
        Catch ex As Exception
            If Not SqlConn Is Nothing Then
                If SqlConn.State = ConnectionState.Open Then
                    clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
                    clsDBConnect.dbConnectionClose(SqlConn)
                End If
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierBookingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub ddlTravelDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTravelDate.SelectedIndexChanged"
    Protected Sub ddlTravelDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTravelDate.SelectedIndexChanged
        If ddlTravelDate.SelectedItem.Text.ToUpper = "Specific date".ToUpper() Then
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
            Dim lblRequestId As Label
            lblRequestId = gvSearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblRequestId")
            Dim strpop As String = ""
            If e.CommandName = "UpdateSupplier" Then
                strpop = "window.open('UpdateSupplierBooking.aspx?State=Edit&divid=" & ViewState("divcode") & "&ID=" & CType(lblRequestId.Text.Trim, String) & "','UpdateSupplier');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
            ElseIf e.CommandName = "Print" Then
                strpop = "window.open('PrintReport.aspx?printId=bookingConfirmation&RequestId=" & CType(lblRequestId.Text.Trim, String) & "','ProformaPrint');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "proformaInvoice", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdateSupplierBookingSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete"
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Dim lbltitle As Label = CType(Me.Master.FindControl("title"), Label)
        Dim strTitle As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_des from division_master where division_master_code ='" & ViewState("divcode") & "'")
        lbltitle.Text = strTitle
        Me.Page.Title = strTitle
    End Sub
#End Region

End Class
