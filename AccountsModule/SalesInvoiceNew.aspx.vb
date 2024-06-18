Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.IO
Imports System.Collections.Generic
Imports System.Globalization


Partial Class SalesInvoiceNew
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim myDataAdapter As SqlDataAdapter
    Dim sqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region

#Region "Web Service Methods"
    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function GetRequestId(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim reqids As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select requestid from booking_header(nolock) where div_code='" & contextKey.Trim & "' and requestid like  '%" & prefixText & "%' order by requestid"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")         'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    reqids.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("requestid").ToString(), myDS.Tables(0).Rows(i)("requestid").ToString()))
                Next
            End If

            Return reqids
        Catch ex As Exception
            Return reqids
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function GetCustomers(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim agentnames As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select distinct ltrim(rtrim(agentname)) agentname from booking_header h(nolock) inner join agentmast a(nolock) on h.agentcode=a.agentcode where div_code='" & contextKey.Trim & "' and a.agentname like '%" & prefixText & "%' order by agentname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")         'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    agentnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("agentname").ToString()))
                Next
            End If

            Return agentnames
        Catch ex As Exception
            Return agentnames
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function GetAgentRef(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim agentref As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select distinct ltrim(rtrim(agentref)) as agentref from booking_header(nolock) where div_code='" & contextKey.Trim & "' and isnull(agentref,'')<>'' and agentref like '%" & prefixText & "%' order by agentref"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")         'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    agentref.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentref").ToString(), myDS.Tables(0).Rows(i)("agentref").ToString()))
                Next
            End If

            Return agentref
        Catch ex As Exception
            Return agentref
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function GetguestNames(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim gname As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select (case when isnull(g.title,'')<>'' then g.title else '' end + case when isnull(g.firstname,'')<>'' then case when isnull(g.title,'')<>'' then ' ' else '' end + " &
            "g.firstname else '' end + case when ISNULL(g.middlename,'')<>'' then ' ' + g.middlename else '' end + " &
            "case when isnull(g.lastname,'')<>'' then ' '+ g.lastname else '' end) as guestname  from booking_header h " &
            "inner join booking_guest g on h.requestid=g.requestid where div_code='" & contextKey.Trim & "' and g.guestlineno=1 and " &
            "(case when isnull(g.title,'')<>'' then g.title else '' end + case when isnull(g.firstname,'')<>'' then case when isnull(g.title,'')<>'' then ' ' else '' end + " &
            "g.firstname else '' end + case when ISNULL(g.middlename,'')<>'' then ' ' + g.middlename else '' end + " &
            "case when isnull(g.lastname,'')<>'' then ' '+ g.lastname else '' end) like '%" & prefixText & "%' order by guestname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")         'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    gname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("guestname").ToString(), myDS.Tables(0).Rows(i)("guestname").ToString()))
                Next
            End If

            Return gname
        Catch ex As Exception
            Return gname
        End Try
    End Function

    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function GetSuppliers(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppliername As New List(Of String)
        Try
            If prefixText = " " Then
                prefixText = ""
            End If
            strSqlQry = "select distinct partyname from view_common_booking_all where div_code='" & contextKey.Trim & "' and partyname like '%" & prefixText & "%' order by partyname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")         'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    suppliername.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partyname").ToString()))
                Next
            End If

            Return suppliername
        Catch ex As Exception
            Return suppliername
        End Try
    End Function

#End Region

#Region "Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit"
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Convert.ToString(Request.QueryString("State")) = "Amend" Then
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
        End If
    End Sub
#End Region

#Region "Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                If Convert.ToString(Request.QueryString("State")) = "View" Then
                    Session("SalesInvoiceState") = "View"
                    Page.Title = "View Sales Invoice"
                    lblHeading.Text = "View Sales Invoice"
                ElseIf Convert.ToString(Request.QueryString("State")) = "Amend" Then
                    Session("SalesInvoiceState") = "Amend"
                    Page.Title = "Pending To Invoice After Amendment"
                    lblHeading.Text = "Pending To Invoice After Amendment"
                    btnCancel.Visible = False
                ElseIf Convert.ToString(Request.QueryString("State")) = "New" Then
                    Session("SalesInvoiceState") = "New"
                    Page.Title = "New Sales Invoice"
                    lblHeading.Text = "New Sales Invoice"

                    divAdvanced.Visible = True
       
                Else
                    Session("SalesInvoiceState") = ""
                End If
                Dim strBookingType As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2036'")
                Dim arrBookingType As String() = strBookingType.Split(";")
                If arrBookingType.Length > 0 Then
                    For i = 0 To arrBookingType.GetUpperBound(0)
                        ddlBookingType.Items.Add(New ListItem(arrBookingType(i).Trim, arrBookingType(i).Trim))
                    Next
                Else
                    ddlBookingType.Items.Add(New ListItem("All", "All"))
                End If
                'Tanvir 27102023
                txtscheduledate.Visible = False
                txttime.Visible = False
                ImgBtnscheduledate.Visible = False
                btnsaveschedule.Visible = False
                'Tanvir 27102023
                ViewState.Add("divcode", Request.QueryString("divid"))
                txtDivCode.Text = ViewState("divcode")
                Dim baseCurrency As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='457'")
                gvReadyInvoice.Columns(11).HeaderText = "Sales Amount (" + baseCurrency + ")"
                gvPending.Columns(10).HeaderText = "Sales Amount (" + baseCurrency + ")"

                Dim checkDateFlag As String = Convert.ToString(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='5511'"))
                If String.IsNullOrWhiteSpace(checkDateFlag) Then
                    hdnChkDtFlag.Value = "N"
                Else
                    hdnChkDtFlag.Value = checkDateFlag
                End If
                If hdnChkDtFlag.Value = "Y" Then
                    lblchkFromDt.InnerText = "From Check In Date"
                    lblChkToDt.InnerText = "To Check In Date"
                Else
                    lblchkFromDt.InnerText = "From Check Out Date"
                    lblChkToDt.InnerText = "To Check Out Date"
                End If

                Dim decimalPlaces As Integer = Convert.ToInt32(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='509'"))
                Session.Add("decimalPlaces", decimalPlaces)

                Dim dtReceipt As New DataTable
                dtReceipt.Columns.Add(New DataColumn("selection", GetType(Boolean)))
                dtReceipt.Columns.Add(New DataColumn("requestId", GetType(String)))
                dtReceipt.Columns.Add(New DataColumn("voucherNo", GetType(String)))
                dtReceipt.Columns.Add(New DataColumn("voucherDate", GetType(String)))
                dtReceipt.Columns.Add(New DataColumn("amountReceived", GetType(Decimal)))
                dtReceipt.Columns.Add(New DataColumn("voucherLineno", GetType(Integer)))
                dtReceipt.Columns.Add(New DataColumn("voucherType", GetType(String)))
                Session("dtReceipt") = dtReceipt

                Dim dt As New DataTable
                dt.Columns.Add(New DataColumn("selection", GetType(Boolean)))
                dt.Columns.Add(New DataColumn("requestId", GetType(String)))
                dt.Columns.Add(New DataColumn("div_code", GetType(String)))
                dt.Columns.Add(New DataColumn("status", GetType(String)))
                dt.Columns.Add(New DataColumn("amended", GetType(String)))
                dt.Columns.Add(New DataColumn("arrivalDate", GetType(Date)))
                dt.Columns.Add(New DataColumn("departureDate", GetType(Date)))
                dt.Columns.Add(New DataColumn("agentName", GetType(String)))
                dt.Columns.Add(New DataColumn("agentref", GetType(String)))
                dt.Columns.Add(New DataColumn("guestName", GetType(String)))
                dt.Columns.Add(New DataColumn("Currency", GetType(String)))
                dt.Columns.Add(New DataColumn("amount", GetType(Decimal)))
                dt.Columns.Add(New DataColumn("salesAmount", GetType(Decimal)))
                dt.Columns.Add(New DataColumn("amountReceived", GetType(Decimal)))
                dt.Columns.Add(New DataColumn("receivedStatus", GetType(Integer)))
                dt.Columns.Add(New DataColumn("invoiceNo", GetType(String)))

                If Convert.ToString(Session("SalesInvoiceState")) = "View" Then
                    Dim invoiceNo As String
                    invoiceNo = CType(Request.QueryString("ID"), String)
                    FillEntry(invoiceNo)
                    gvPending.DataSource = dt
                    gvPending.DataBind()
                    DisableControl()
                ElseIf Convert.ToString(Session("SalesInvoiceState")) = "Amend" Then
                    gvReadyInvoice.Columns(3).HeaderText = "Amended Date"
                    gvReadyInvoice.DataSource = dt
                    gvReadyInvoice.DataBind()
                    gvPending.Columns(2).HeaderText = "Amended Date"
                    gvPending.DataSource = dt
                    gvPending.DataBind()
                ElseIf Convert.ToString(Session("SalesInvoiceState")) = "New" Then
                    gvReadyInvoice.DataSource = dt
                    gvReadyInvoice.DataBind()
                    gvPending.DataSource = dt
                    gvPending.DataBind()
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
                End If
                Dim firstday = New DateTime(Date.Now.Year, Date.Now.Month, 1)
                Dim lastday = firstday.AddMonths(1).AddDays(-1)
                txtChkFromDt.Text = firstday.ToString("dd/MM/yyyy")
                txtChkToDt.Text = lastday.ToString("dd/MM/yyyy")

                Session("bookingInvoice") = dt
                Session.Add("strsortexpressionReady", "DepartureDate")
                Session.Add("strsortdirectionReady", SortDirection.Ascending)

                Session.Add("strsortexpressionPending", "DepartureDate")
                Session.Add("strsortdirectionPending", SortDirection.Ascending)
                btnGenInvoice.Enabled = False
                btnGenInvoice.Attributes.Add("class", "disablebtn")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SalesInvoiceNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Protected Sub DisableControl()"
    Protected Sub DisableControl()
        txtChkFromDt.Enabled = False
        txtChkToDt.Enabled = False
        lnkBtnAdvanced.Enabled = False
        divAdvanced.Visible = False
        ImgAdvanced.Disabled = True
        btnDisplay.Enabled = False
        btnReset.Enabled = False
        gvReadyInvoice.Enabled = False
        gvPending.Enabled = False
        btnValidate.Enabled = False
        btnGenInvoice.Enabled = False
    End Sub
#End Region

#Region "Protected Sub FillEntry(ByVal invoiceNo As String)"
    Protected Sub FillEntry(ByVal invoiceNo As String)
        Try
            lblMsg.Visible = False
            Dim strSortBy As String = Convert.ToString(Session("strsortExpressionReady"))
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("sp_show_salesinvoice", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0
            mySqlCmd.Parameters.Add(New SqlParameter("@invoiceNo", SqlDbType.VarChar, 20)).Value = invoiceNo
            Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
                Using dt As New DataTable()
                    myDataAdapter.Fill(dt)
                    Dim dc As New DataColumn("Selection", GetType(Boolean))
                    dc.DefaultValue = False
                    dt.Columns.Add(dc)
                    gvReadyInvoice.DataSource = dt
                    gvReadyInvoice.DataBind()
                    If dt.Rows.Count = 0 Then
                        lblMsg.Visible = True
                    End If
                End Using
            End Using
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SalesInvoiceNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub lnkBtnAdvanced_Click(sender As Object, e As System.EventArgs) Handles lnkBtnAdvanced.Click"
    Protected Sub lnkBtnAdvanced_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBtnAdvanced.Click
        Try
            Dim imgName As String = ImgAdvanced.Src.ToString()
            If imgName.ToLower() = "~/images/rightArrow.png".ToLower() Then
                ImgAdvanced.Attributes("src") = "~/images/downArrow.png"
                divAdvanced.Visible = True
            Else
                ImgAdvanced.Attributes("src") = "~/images/rightArrow.png"
                divAdvanced.Visible = False
                txtBookingNo.Text = ""
                txtCustName.Text = ""
                txtSupName.Text = ""
                txtGuestName.Text = ""
                txtCustRef.Text = ""
                ddlBookingType.SelectedIndex = 0
                txtReqFromDt.Text = ""
                txtReqToDt.Text = ""
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SalesInvoiceNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Function BuildCondition() As String"
    Protected Function BuildCondition() As String
        Try
            Dim strWhereCond As String = ""
            If Convert.ToString(Session("SalesInvoiceState")) <> "Amend" Then
                strWhereCond = "isnull(invno,'')='' "
            End If
            If txtBookingNo.Text.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "RequestId like '%" & txtBookingNo.Text.Trim.ToUpper & "%'"
                Else
                    strWhereCond = strWhereCond & "and RequestId like '%" & txtBookingNo.Text.Trim.ToUpper & "%'"
                End If
            End If
            If txtCustName.Text.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "agentName like '%" & txtCustName.Text.Trim & "%'"
                Else
                    strWhereCond = strWhereCond & "and agentName like '%" & txtCustName.Text.Trim & "%'"
                End If
            End If
            If txtSupName.Text.Trim() <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "requestid in (select distinct requestid from view_common_booking_all where partyName like '%" & txtSupName.Text.Trim & "%')"
                Else
                    strWhereCond = strWhereCond & "and requestid in (select distinct requestid from view_common_booking_all where partyName like '%" & txtSupName.Text.Trim & "%')"
                End If
            End If
            If txtGuestName.Text.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "guestName like '%" & txtGuestName.Text.Trim & "%'"
                Else
                    strWhereCond = strWhereCond & "and guestName like '%" & txtGuestName.Text.Trim & "%'"
                End If
            End If
            If txtCustRef.Text.Trim <> "" Then
                If strWhereCond = "" Then
                    strWhereCond = "agentref like '%" & txtCustRef.Text.Trim & "%'"
                Else
                    strWhereCond = strWhereCond & "and agentref like '%" & txtCustRef.Text.Trim & "%'"
                End If
            End If
            If ddlBookingType.SelectedValue.Trim <> "All" Then
                If strWhereCond = "" Then
                    strWhereCond = "bookingengineratetype ='" & ddlBookingType.SelectedValue & "'"
                Else
                    strWhereCond = strWhereCond & "and bookingengineratetype ='" & ddlBookingType.SelectedValue & "'"
                End If
            End If
            If IsDate(txtReqFromDt.Text) And IsDate(txtReqToDt.Text) Then
                If strWhereCond = "" Then
                    strWhereCond = "requestDate between CONVERT(datetime, '" + txtReqFromDt.Text + "',103) and CONVERT(datetime, '" + txtReqToDt.Text + "',103)"
                Else
                    strWhereCond = strWhereCond & "and requestDate between CONVERT(datetime, '" + txtReqFromDt.Text + "',103) and CONVERT(datetime, '" + txtReqToDt.Text + "',103)"
                End If
            End If
            BuildCondition = strWhereCond
        Catch ex As Exception
            BuildCondition = ""
            Throw ex
        End Try
    End Function
#End Region

    Function validatedate() As Boolean
        Dim fromdate As DateTime = Convert.ToString(Convert.ToDateTime(txtChkFromDt.Text).ToString("yyyy/MM/dd"))
        Dim todate As DateTime = Convert.ToString(Convert.ToDateTime(txtChkToDt.Text).ToString("yyyy/MM/dd"))
        Dim differenceInDays As Integer = (todate - fromdate).Days
        validatedate = True
        If differenceInDays > 31 Then
            validatedate = False
            Exit Function
        End If


        Return validatedate
    End Function
#Region "Protected Sub btnDisplay_Click(sender As Object, e As System.EventArgs) Handles btnDisplay.Click"
    Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Try
       
            If Session("SalesInvoiceState") <> "Amend" And Session("SalesInvoiceState") <> "New" And Convert.ToString(ViewState("divcode")) <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Invalid page state' );", True)
                Exit Sub
            End If
            TabBookList.ActiveTab = panConfirm

            DisplayConfirmedRecords()
            DisplayPendingRecords()
            gvErrorList.DataSource = Nothing
            gvErrorList.DataBind()
            btnValidate.Enabled = True
            btnValidate.Attributes.Add("class", "btn")
            lblValidateMsg.Visible = False
            btnGenInvoice.Enabled = False
            btnGenInvoice.Attributes.Add("class", "disablebtn")
            Dim dtReceipt As DataTable = CType(Session("dtReceipt"), DataTable)
            dtReceipt.Clear()
        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + HttpUtility.JavaScriptStringEncode(ex.Message) & "' );", True)
            objUtils.WritErrorLog("SalesInvoiceNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub DisplayConfirmedRecords()"
    Protected Sub DisplayConfirmedRecords()
        Try
            lblMsg.Visible = False
            Dim strBindCondition As String = BuildCondition()
            Dim strSortBy As String = Convert.ToString(Session("strsortExpressionReady"))
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("booking_salesInvoice_search", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0 'Tanvir 27022023
            mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = Convert.ToString(ViewState("divcode"))
            mySqlCmd.Parameters.Add(New SqlParameter("@bindCond", SqlDbType.VarChar)).Value = strBindCondition
            mySqlCmd.Parameters.Add(New SqlParameter("@sortBy", SqlDbType.VarChar, 100)).Value = strSortBy
            mySqlCmd.Parameters.Add(New SqlParameter("@FromChkOutDt", SqlDbType.VarChar, 10)).Value = Convert.ToString(Convert.ToDateTime(txtChkFromDt.Text).ToString("yyyy/MM/dd"))
            mySqlCmd.Parameters.Add(New SqlParameter("@ToChkOutDt", SqlDbType.VarChar, 10)).Value = Convert.ToString(Convert.ToDateTime(txtChkToDt.Text).ToString("yyyy/MM/dd"))
            mySqlCmd.Parameters.Add(New SqlParameter("@SalesInvoiceState", SqlDbType.VarChar, 20)).Value = Session("SalesInvoiceState")
            mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar, 20)).Value = txtBookingNo.Text.Trim.ToUpper
            mySqlCmd.Parameters.Add(New SqlParameter("@dateFlag", SqlDbType.VarChar, 1)).Value = hdnChkDtFlag.Value.Trim

            Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
                Using dt As New DataTable()
                    myDataAdapter.Fill(dt)
                    Dim dc As New DataColumn("Selection", GetType(Boolean))
                    dc.DefaultValue = False
                    dt.Columns.Add(dc)
                    Dim dc1 As New DataColumn("InvoiceNo", GetType(String))
                    dc1.DefaultValue = ""
                    dt.Columns.Add(dc1)
                    gvReadyInvoice.DataSource = dt
                    gvReadyInvoice.DataBind()
                    If dt.Rows.Count = 0 Then
                        lblMsg.Visible = True
                    End If
                End Using
            End Using
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Protected Sub DisplayPendingRecords()"
    Protected Sub DisplayPendingRecords()
        Try
            lblPendingMsg.Visible = False
            Dim strBindCondition As String = BuildCondition()
            Dim strSortBy As String = Convert.ToString(Session("strsortExpressionPending"))
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("bookingPending_salesInvoice_search", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0
            mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = Convert.ToString(ViewState("divcode"))
            mySqlCmd.Parameters.Add(New SqlParameter("@bindCond", SqlDbType.VarChar)).Value = strBindCondition
            mySqlCmd.Parameters.Add(New SqlParameter("@sortBy", SqlDbType.VarChar, 100)).Value = strSortBy
            mySqlCmd.Parameters.Add(New SqlParameter("@FromChkOutDt", SqlDbType.VarChar, 10)).Value = Convert.ToString(Convert.ToDateTime(txtChkFromDt.Text).ToString("yyyy/MM/dd"))
            mySqlCmd.Parameters.Add(New SqlParameter("@ToChkOutDt", SqlDbType.VarChar, 10)).Value = Convert.ToString(Convert.ToDateTime(txtChkToDt.Text).ToString("yyyy/MM/dd"))
            mySqlCmd.Parameters.Add(New SqlParameter("@SalesInvoiceState", SqlDbType.VarChar, 20)).Value = Session("SalesInvoiceState")
            mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar, 20)).Value = txtBookingNo.Text.Trim.ToUpper
            mySqlCmd.Parameters.Add(New SqlParameter("@dateFlag", SqlDbType.VarChar, 1)).Value = hdnChkDtFlag.Value.Trim
            Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
                Using dt As New DataTable()
                    myDataAdapter.Fill(dt)
                    gvPending.DataSource = dt
                    gvPending.DataBind()
                    If dt.Rows.Count = 0 Then
                        lblPendingMsg.Visible = True
                    End If
                End Using
            End Using
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Protected Sub btnValidate_Click(sender As Object, e As System.EventArgs) Handles btnValidate.Click"
    Protected Sub btnValidate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnValidate.Click
        Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from  sealing_master")
        If gvReadyInvoice.Rows.Count > 0 Then
            Dim requestIds As String = ""
            Dim chkSelection As New CheckBox
            Dim lblRequestId As New Label
            Dim hdnArrivalDt As HiddenField
            Dim hdnDepartureDt As HiddenField

            For Each dr As GridViewRow In gvReadyInvoice.Rows
                chkSelection = CType(dr.FindControl("chkSelection"), CheckBox)
                If chkSelection.Checked = True Then
                    lblRequestId = CType(dr.FindControl("lblRequestId"), Label)
                    hdnArrivalDt = CType(dr.FindControl("hdnArrivalDt"), HiddenField)
                    hdnDepartureDt = CType(dr.FindControl("hdnDepartureDt"), HiddenField)
                    If requestIds = "" Then
                        requestIds = lblRequestId.Text.Trim
                    Else
                        requestIds = requestIds + "," + lblRequestId.Text.Trim
                    End If
                    'added param on 22/07/2021
                    If IsDate(sealdate) Then
                        If hdnChkDtFlag.Value = "Y" Then
                            If (CType(hdnArrivalDt.Value, Date) <= CType(sealdate, Date)) Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Booking(" + lblRequestId.Text + ") invoice Date Should be greater than seal date - " & CType(sealdate, Date).ToString("dd/MM/yyyy") & "');", True)
                                btnGenInvoice.Enabled = False
                                btnGenInvoice.Attributes.Add("class", "disablebtn")
                                Exit Sub
                            End If
                        Else
                            If (CType(hdnDepartureDt.Value, Date) <= CType(sealdate, Date)) Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Booking(" + lblRequestId.Text + ") invoice Date Should be greater than seal date - " & CType(sealdate, Date).ToString("dd/MM/yyyy") & "');", True)
                                btnGenInvoice.Enabled = False
                                btnGenInvoice.Attributes.Add("class", "disablebtn")
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            Next
            Dim dt As New DataTable
            If requestIds <> "" Then
                sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                mySqlCmd = New SqlCommand("sp_validate_booking_forinvoice", sqlConn)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.CommandTimeout = 0
                mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = Convert.ToString(ViewState("divcode"))
                mySqlCmd.Parameters.Add(New SqlParameter("@requestIds", SqlDbType.VarChar)).Value = requestIds
                mySqlCmd.Parameters.Add(New SqlParameter("@mode", SqlDbType.VarChar, 20)).Value = Convert.ToString(Session("SalesInvoiceState"))
                Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
                    myDataAdapter.Fill(dt)
                    gvErrorList.DataSource = dt
                    gvErrorList.DataBind()

                End Using
                clsDBConnect.dbCommandClose(mySqlCmd)
                clsDBConnect.dbConnectionClose(sqlConn)

                Dim findSeverity = (From n In dt.AsEnumerable Where n.Field(Of Integer)("severity") = 1 Select n)
                If findSeverity.Count > 0 Then
                    btnGenInvoice.Enabled = False
                    btnGenInvoice.Attributes.Add("class", "disablebtn")
                Else
                    Dim chkSelectAll As CheckBox = CType(gvReadyInvoice.HeaderRow.FindControl("chkSelectAll"), CheckBox)
                    chkSelectAll.Enabled = False
                    For Each gvr As GridViewRow In gvReadyInvoice.Rows
                        chkSelection = CType(gvr.FindControl("chkSelection"), CheckBox)
                        chkSelection.Enabled = False
                    Next
                    btnGenInvoice.Enabled = True
                    btnGenInvoice.Attributes.Add("class", "btn")
                    lblValidateMsg.Visible = True
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select bookings in ready to invoice');", True)
                Exit Sub
            End If
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('There is no bookings in ready to invoice');", True)
            Exit Sub
        End If
    End Sub
#End Region

#Region "Protected Sub btnReset_Click(sender As Object, e As System.EventArgs) Handles btnReset.Click"
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        'changed by Priyanka  29/08/2019
        ' Dim firstday = New DateTime(Date.Now.Year, Date.Now.Month, 1)
        'Dim lastday = firstday.AddMonths(1).AddDays(-1)
        'txtChkFromDt.Text = firstday.ToString("dd/MM/yyyy")
        'txtChkToDt.Text = lastday.ToString("dd/MM/yyyy")
        txtBookingNo.Text = ""
        txtCustName.Text = ""
        txtSupName.Text = ""
        txtGuestName.Text = ""
        txtCustRef.Text = ""
        ddlBookingType.SelectedIndex = 0
        txtReqFromDt.Text = ""
        txtReqToDt.Text = ""
        Dim dt As DataTable = CType(Session("bookingInvoice"), DataTable)
        gvReadyInvoice.DataSource = dt
        gvReadyInvoice.DataBind()
        gvPending.DataSource = dt
        gvPending.DataBind()
        TabBookList.ActiveTab = panConfirm
        gvErrorList.DataSource = Nothing
        gvErrorList.DataBind()
    End Sub
#End Region

#Region "Protected Sub gvReadyInvoice_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvReadyInvoice.RowCommand"
    Protected Sub gvReadyInvoice_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvReadyInvoice.RowCommand
        Try
            If e.CommandName = "Receipts" Then
                Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
                hdReadyIndex.Value = rowIndex
                Dim row As GridViewRow = gvReadyInvoice.Rows(rowIndex)
                Dim lblRequestId As Label = CType(row.FindControl("lblRequestId"), Label)
                hdRequestId.Value = lblRequestId.Text.Trim
                Dim dtReceipt As DataTable = CType(Session("dtReceipt"), DataTable)
                Dim filterRow = From n In dtReceipt.AsEnumerable Where n.Field(Of String)("requestId") = lblRequestId.Text.Trim Select n
                Dim filterDt As New DataTable
                If filterRow.Count > 0 Then
                    filterDt = filterRow.CopyToDataTable()
                End If
                sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                mySqlCmd = New SqlCommand("select requestId,voucherNo,convert(varchar(10),voucherDate,103) as voucherDate,amountReceived,voucherLineno,voucherType from view_futurebooking_receipts_detail where requestid=@requestid order by voucherno", sqlConn)
                mySqlCmd.CommandType = CommandType.Text
                mySqlCmd.CommandTimeout = 0
                mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = lblRequestId.Text.Trim
                Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
                    Using dt As New DataTable()
                        myDataAdapter.Fill(dt)
                        Dim dc As New DataColumn("selection", GetType(Boolean))
                        If filterDt.Rows.Count > 0 Then
                            dt.Columns.Add(dc)
                            Dim matchrow = From n In dt.AsEnumerable Join m In filterDt.AsEnumerable On n.Field(Of String)("voucherno") Equals m.Field(Of String)("voucherno") Select n
                            If matchrow.Count > 0 Then
                                For Each dr As DataRow In matchrow
                                    dr("selection") = True
                                Next
                                dt.AcceptChanges()
                            End If
                        Else
                            dc.DefaultValue = True
                            dt.Columns.Add(dc)
                        End If
                        gvShowReceipt.DataSource = dt
                        gvShowReceipt.DataBind()
                    End Using
                End Using
                clsDBConnect.dbCommandClose(mySqlCmd)
                clsDBConnect.dbConnectionClose(sqlConn)
                ModalExtraPopup.Show()
            ElseIf e.CommandName = "Proforma" Then
                Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = gvReadyInvoice.Rows(rowIndex)
                Dim lblRequestId As Label = CType(row.FindControl("lblRequestId"), Label)
                If lblRequestId.Text.Trim <> "" Then
                    Dim strpop As String = ""
                    strpop = "window.open('PrintReport.aspx?printId=bookingConfirmation&RequestId=" & lblRequestId.Text.Trim & "','ProformaPrint');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "proformaInvoice", strpop, True)
                End If
            ElseIf e.CommandName = "View" Then
                Dim lbtnView As LinkButton
                lbtnView = CType(gvReadyInvoice.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbtnView"), LinkButton)
                Dim invoiceno As String = lbtnView.Text.Trim
                If invoiceno <> "" Then
                    Dim strpop As String = ""
                    strpop = "window.open('PrintReport.aspx?printId=salesInvoice&InvoiceNo=" & invoiceno & "','SalesInvoicePrint');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "salesInvoice", strpop, True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SalesInvoiceNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub gvReadyInvoice_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReadyInvoice.RowDataBound"
    Protected Sub gvReadyInvoice_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReadyInvoice.RowDataBound
        e.Row.Cells(12).BorderWidth = "0"
        e.Row.Cells(13).BorderWidth = "0"
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim decimalPlaces As Integer = CType(Session("decimalPlaces"), Integer)
            'If e.Row.Cells(3).Text.Trim = "1" Then
            '    e.Row.Cells(3).Text = "Amended"
            'Else
            '    e.Row.Cells(3).Text = ""
            'End If
            Dim lblAmount As Label = CType(e.Row.FindControl("lblAmount"), Label)
            If IsNumeric(lblAmount.Text) Then
                lblAmount.Text = Math.Round(Convert.ToDecimal(lblAmount.Text), decimalPlaces)
            End If
            Dim lblSalesAmount As Label = CType(e.Row.FindControl("lblSalesAmount"), Label)
            If IsNumeric(lblSalesAmount.Text) Then
                lblSalesAmount.Text = Math.Round(Convert.ToDecimal(lblSalesAmount.Text), decimalPlaces)
            End If
            Dim lblAmountReceived As Label = CType(e.Row.FindControl("lblAmountReceived"), Label)
            If IsNumeric(lblAmountReceived.Text) Then
                lblAmountReceived.Text = Math.Round(Convert.ToDecimal(lblAmountReceived.Text), decimalPlaces)
            End If
            Dim lblReceivedStatus As Label = CType(e.Row.FindControl("lblReceivedStatus"), Label)
            Dim btnAmtReceived As Button = CType(e.Row.FindControl("btnAmtReceived"), Button)
            If lblReceivedStatus.Text.Trim = "1" Then
                btnAmtReceived.Visible = True
            Else
                btnAmtReceived.Visible = False
            End If

        End If
    End Sub
#End Region

#Region "Protected Sub gvPending_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPending.RowDataBound"
    Protected Sub gvPending_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPending.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim decimalPlaces As Integer = CType(Session("decimalPlaces"), Integer)
            If e.Row.Cells(2).Text.Trim = "1" Then
                e.Row.Cells(2).Text = "Amended"
            Else
                e.Row.Cells(2).Text = ""
            End If
            Dim lblAmount As Label = CType(e.Row.FindControl("lblAmount"), Label)
            If IsNumeric(lblAmount.Text) Then
                lblAmount.Text = Math.Round(Convert.ToDecimal(lblAmount.Text), decimalPlaces)
            End If
            Dim lblSalesAmount As Label = CType(e.Row.FindControl("lblSalesAmount"), Label)
            If IsNumeric(lblSalesAmount.Text) Then
                lblSalesAmount.Text = Math.Round(Convert.ToDecimal(lblSalesAmount.Text), decimalPlaces)
            End If
        End If
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region "Protected Sub gvReadyInvoice_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvReadyInvoice.Sorting"
    Protected Sub gvReadyInvoice_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvReadyInvoice.Sorting
        Session.Add("strsortexpressionReady", e.SortExpression)
        SortReadyGridColoumn()
    End Sub
#End Region

#Region "Public Sub SortReadyGridColoumn()"
    Public Sub SortReadyGridColoumn()
        Dim DataTable As DataTable
        DisplayConfirmedRecords()
        DataTable = gvReadyInvoice.DataSource
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirectionReady", objUtils.SwapSortDirection(Session("strsortdirectionReady")))
            dataView.Sort = Session("strsortexpressionReady") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirectionReady"))
            gvReadyInvoice.DataSource = dataView
            gvReadyInvoice.DataBind()
        End If
    End Sub
#End Region

#Region "Protected Sub gvPending_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvPending.Sorting"
    Protected Sub gvPending_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvPending.Sorting
        Session.Add("strsortExpressionPending", e.SortExpression)
        SortPendingGridColoumn()
    End Sub
#End Region

#Region "Public Sub SortPendingGridColoumn()"
    Public Sub SortPendingGridColoumn()
        Dim DataTable As DataTable
        DisplayPendingRecords()
        DataTable = gvPending.DataSource
        If IsDBNull(DataTable) = False Then
            Dim dataView As DataView = DataTable.DefaultView
            Session.Add("strsortdirectionPending", objUtils.SwapSortDirection(Session("strsortdirectionPending")))
            dataView.Sort = Session("strsortexpressionPending") & " " & objUtils.ConvertSortDirectionToSql(Session("strsortdirectionPending"))
            gvPending.DataSource = dataView
            gvPending.DataBind()
        End If
    End Sub
#End Region

#Region "Protected Sub btnGenInvoice_Click(sender As Object, e As System.EventArgs) Handles btnGenInvoice.Click"
    Protected Sub btnGenInvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenInvoice.Click
        Try
            'Tanvir 27102023
            'If validatedate() = False Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Date Range upto 15 Days' );", True)
            '    Exit Sub
            'End If
            'Tanvir 27102023
            btnGenInvoice.Enabled = False
            btnValidate.Enabled = False
            Dim dtReceipt As DataTable = CType(Session("dtReceipt"), DataTable)
            Dim chkSelection As New CheckBox
            Dim lblRequestId As New Label
            Dim lblAmountReceived As New Label
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            For Each dr As GridViewRow In gvReadyInvoice.Rows
                chkSelection = CType(dr.FindControl("chkSelection"), CheckBox)
                If chkSelection.Checked = True Then
                    lblRequestId = CType(dr.FindControl("lblRequestId"), Label)
                    lblAmountReceived = CType(dr.FindControl("lblAmountReceived"), Label)
                    If IsNumeric(lblAmountReceived.Text.Trim) Then
                        Dim filterRow = From n In dtReceipt.AsEnumerable Where n.Field(Of String)("requestId") = lblRequestId.Text.Trim Select n
                        If Not filterRow.Count > 0 Then
                            mySqlCmd = New SqlCommand("select requestId,voucherNo,convert(varchar(10),voucherDate,103) as voucherDate,amountReceived,voucherLineno,voucherType from view_futurebooking_receipts_detail where requestid=@requestid order by voucherno", sqlConn)
                            mySqlCmd.CommandType = CommandType.Text
                            mySqlCmd.CommandTimeout = 0
                            mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = lblRequestId.Text.Trim
                            Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
                                Using dt As New DataTable()
                                    myDataAdapter.Fill(dt)
                                    Dim dc As New DataColumn("selection", GetType(Boolean))
                                    dc.DefaultValue = True
                                    dt.Columns.Add(dc)
                                    Dim calculateAmtReceived As Decimal = dt.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("amountreceived"))
                                    If lblAmountReceived.Text.Trim <> calculateAmtReceived Then
                                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Amount Received is not match; Refresh list or select receipts' );", True)
                                        Exit Sub
                                    Else
                                        For Each mdr As DataRow In dt.Rows
                                            Dim rdr As DataRow = dtReceipt.NewRow
                                            rdr("selection") = mdr("selection")
                                            rdr("requestid") = mdr("requestId")
                                            rdr("voucherNo") = mdr("voucherNo")
                                            rdr("voucherDate") = mdr("voucherDate")
                                            rdr("amountReceived") = mdr("amountReceived")
                                            rdr("voucherLineno") = mdr("voucherLineno")
                                            rdr("voucherType") = mdr("voucherType")
                                            dtReceipt.Rows.Add(rdr)
                                        Next
                                        dtReceipt.AcceptChanges()
                                    End If
                                End Using
                            End Using
                            clsDBConnect.dbCommandClose(mySqlCmd)
                        Else
                            Dim calculateAmtReceived As Decimal = (From n In dtReceipt.AsEnumerable() Where n.Field(Of Boolean)("Selection") = True And n.Field(Of String)("requestid") = lblRequestId.Text.Trim Select n).Sum(Function(row) row.Field(Of Decimal)("amountreceived"))
                            If lblAmountReceived.Text.Trim <> calculateAmtReceived Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Amount Received and selected voucher value is not match; verify selected receipts' );", True)
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            Next
            clsDBConnect.dbConnectionClose(sqlConn)
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open

            Dim invRequestIds As String = ""
            For Each dr As GridViewRow In gvReadyInvoice.Rows
                chkSelection = CType(dr.FindControl("chkSelection"), CheckBox)
                If chkSelection.Checked = True Then
                    sqlTrans = sqlConn.BeginTransaction
                    lblRequestId = CType(dr.FindControl("lblRequestId"), Label)
                    If invRequestIds = "" Then
                        invRequestIds = "'" + lblRequestId.Text.Trim + "'"
                    Else
                        invRequestIds = invRequestIds + ",'" + lblRequestId.Text.Trim + "'"
                    End If
                    Dim xmlReceipt As String = ""
                    Dim filterRow = (From n In dtReceipt.AsEnumerable() Where n.Field(Of Boolean)("Selection") = True And n.Field(Of String)("requestid") = lblRequestId.Text.Trim Select n)
                    If filterRow.Count > 0 Then
                        Dim filterDt As DataTable
                        filterDt = filterRow.CopyToDataTable
                        filterDt.TableName = "Receipts"
                        xmlReceipt = ConvertDatatableToXML(filterDt)
                    End If
                    mySqlCmd = New SqlCommand("sp_getinvoice_data", sqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.CommandTimeout = 0
                    mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar)).Value = lblRequestId.Text.Trim
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 100)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@xmlReceipt", SqlDbType.Xml)).Value = xmlReceipt
                    mySqlCmd.Parameters.Add(New SqlParameter("@mode", SqlDbType.VarChar, 20)).Value = Convert.ToString(Session("SalesInvoiceState"))
                    mySqlCmd.ExecuteNonQuery()
                    clsDBConnect.dbCommandClose(mySqlCmd)
                    sqlTrans.Commit()
                    'sqlTrans = Nothing
                End If
            Next

            clsDBConnect.dbSqlTransation(sqlTrans)
            clsDBConnect.dbConnectionClose(sqlConn)
            sqlTrans = Nothing

            btnGenInvoice.Enabled = False
            btnGenInvoice.Attributes.Add("class", "disablebtn")
            btnValidate.Enabled = False
            btnValidate.Attributes.Add("class", "disablebtn")

            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            strSqlQry = "select distinct invoiceno,requestid from InvoiceHeader where requestid in (" + invRequestIds + ")"
            mySqlCmd = New SqlCommand(strSqlQry, sqlConn)
            mySqlCmd.CommandType = CommandType.Text
            mySqlCmd.CommandTimeout = 0
            myDataAdapter = New SqlDataAdapter(mySqlCmd)
            Dim InvidDt As New DataTable
            myDataAdapter.Fill(InvidDt)
            clsDBConnect.dbCommandClose(mySqlCmd)
            clsDBConnect.dbConnectionClose(sqlConn)

            Dim sdt As DataTable = CType(Session("bookingInvoice"), DataTable)
            Dim readyDt As DataTable = sdt.Clone()
            For Each gvr As GridViewRow In gvReadyInvoice.Rows
                Dim readyDr As DataRow = readyDt.NewRow
                readyDr("selection") = CType(gvr.FindControl("chkSelection"), CheckBox).Checked
                Dim requestid As String = CType(gvr.FindControl("lblRequestId"), Label).Text.Trim
                readyDr("requestId") = requestid
                readyDr("status") = gvr.Cells(2).Text.Trim
                Dim strAmended As String = gvr.Cells(3).Text.Trim.Replace("&nbsp;", "")
                readyDr("amended") = strAmended
                readyDr("arrivaldate") = CType(gvr.FindControl("hdnArrivalDt"), HiddenField).Value.Trim ' gvr.Cells(4).Text.Trim 'changed by mohamed on 30/08/2021
                readyDr("departureDate") = CType(gvr.FindControl("hdnDepartureDt"), HiddenField).Value.Trim ' gvr.Cells(5).Text.Trim 'changed by mohamed on 30/08/2021
                readyDr("agentName") = gvr.Cells(6).Text.Trim
                readyDr("agentref") = CType(gvr.FindControl("lblAgentRef"), Label).Text.Trim
                readyDr("guestName") = gvr.Cells(8).Text.Trim
                readyDr("currency") = gvr.Cells(9).Text.Trim
                readyDr("amount") = CType(gvr.FindControl("lblAmount"), Label).Text.Trim
                readyDr("salesAmount") = CType(gvr.FindControl("lblSalesAmount"), Label).Text.Trim
                Dim amtRec As Label = CType(gvr.FindControl("lblAmountReceived"), Label)
                If IsNumeric(amtRec.Text.Trim) Then
                    readyDr("amountReceived") = Convert.ToDecimal(amtRec.Text)
                End If
                readyDr("receivedStatus") = CType(gvr.FindControl("lblReceivedStatus"), Label).Text.Trim
                Dim invid As String = Convert.ToString((From n In InvidDt.AsEnumerable Where n.Field(Of String)("requestid") = requestid Select n.Field(Of String)("invoiceno")).FirstOrDefault)
                readyDr("invoiceno") = invid
                readyDt.Rows.Add(readyDr)
            Next
            gvReadyInvoice.DataSource = readyDt
            gvReadyInvoice.DataBind()
            Dim chkSelectAll As CheckBox = CType(gvReadyInvoice.HeaderRow.FindControl("chkSelectAll"), CheckBox)
            chkSelectAll.Enabled = False
            For Each gvr As GridViewRow In gvReadyInvoice.Rows
                chkSelection = CType(gvr.FindControl("chkSelection"), CheckBox)
                chkSelection.Enabled = False
            Next
            Dim msg As String = ""
            If Session("SalesInvoiceState") = "New" Then
                msg = "Created"
            ElseIf Session("SalesInvoiceState") = "Amend" Then
                msg = "Amended"
            End If
            msg = "alert('Sales invoice " + msg + " Successfully' );"




            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", msg, True)
        Catch ex As Exception
            If Not sqlConn Is Nothing Then
                If sqlConn.State = ConnectionState.Open Then
                    If Not sqlTrans Is Nothing Then
                        sqlTrans.Rollback()
                        clsDBConnect.dbSqlTransation(sqlTrans)
                    End If
                    clsDBConnect.dbConnectionClose(sqlConn)
                End If
            End If
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + Regex.Replace(ex.Message, "[^a-zA-Z0-9_@.-]", " ") & "' );", True)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + HttpUtility.JavaScriptStringEncode(ex.Message) & "' );", True)
            objUtils.WritErrorLog("SalesInvoiceNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            ModalPopupLoading.Hide()
        End Try
    End Sub
#End Region

#Region "Public Function ConvertDatatableToXML(ByVal dt As DataTable) As String"
    Public Function ConvertDatatableToXML(ByVal dt As DataTable) As String
        Dim mstr As New MemoryStream()
        dt.WriteXml(mstr, True)
        mstr.Seek(0, SeekOrigin.Begin)
        Dim sr As New StreamReader(mstr)
        Dim xmlstr As String
        xmlstr = sr.ReadToEnd()
        Return (xmlstr)
    End Function
#End Region

#Region "Protected Sub gvShowReceipt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvShowReceipt.RowDataBound"
    Protected Sub gvShowReceipt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvShowReceipt.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim decimalPlaces As Integer = CType(Session("decimalPlaces"), Integer)
            Dim lblAmountReceived As Label = CType(e.Row.FindControl("lblAmountReceived"), Label)
            If IsNumeric(lblAmountReceived.Text) Then
                lblAmountReceived.Text = Math.Round(Convert.ToDecimal(lblAmountReceived.Text), decimalPlaces)
            End If
        End If
    End Sub
#End Region

#Region "Protected Sub btnRecalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRecalculate.Click"
    Protected Sub btnRecalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRecalculate.Click
        Dim dtReceipt As DataTable = CType(Session("dtReceipt"), DataTable)
        Dim filterRow = From n In dtReceipt.AsEnumerable Where n.Field(Of String)("requestId") = hdRequestId.Value Select n
        If filterRow.Count > 0 Then
            For i = filterRow.Count - 1 To 0 Step -1
                filterRow(i).Delete()
            Next
            dtReceipt.AcceptChanges()
        End If
        Dim chkSelection As New CheckBox
        Dim lblRequestId As New Label
        Dim lblVoucherNo As New Label
        Dim lblVoucherDt As New Label
        Dim lblAmountReceived As New Label
        Dim lblVoucherLineno As New Label
        Dim lblVoucherType As New Label
        Dim AmtReceived As Decimal
        For Each gvr As GridViewRow In gvShowReceipt.Rows
            chkSelection = CType(gvr.FindControl("chkSelection"), CheckBox)
            If chkSelection.Checked = True Then
                lblRequestId = CType(gvr.FindControl("lblRequestId"), Label)
                lblVoucherNo = CType(gvr.FindControl("lblVoucherNo"), Label)
                lblVoucherDt = CType(gvr.FindControl("lblVoucherDt"), Label)
                lblAmountReceived = CType(gvr.FindControl("lblAmountReceived"), Label)
                lblVoucherLineno = CType(gvr.FindControl("lblVoucherLineno"), Label)
                lblVoucherType = CType(gvr.FindControl("lblVoucherType"), Label)
                Dim dr As DataRow = dtReceipt.NewRow
                dr("selection") = True
                dr("requestId") = lblRequestId.Text.Trim
                dr("voucherNo") = lblVoucherNo.Text.Trim
                dr("voucherDate") = lblVoucherDt.Text.Trim
                dr("amountReceived") = lblAmountReceived.Text.Trim
                dr("voucherLineno") = lblVoucherLineno.Text.Trim
                dr("voucherType") = lblVoucherType.Text.Trim
                dtReceipt.Rows.Add(dr)
                If IsNumeric(lblAmountReceived.Text.Trim) Then
                    AmtReceived = AmtReceived + Convert.ToDecimal(lblAmountReceived.Text.Trim)
                End If
            End If
        Next
        Session("dtReceipt") = dtReceipt
        Dim rowIndex As Integer = Convert.ToInt32(hdReadyIndex.Value)
        Dim gvrReady As GridViewRow = gvReadyInvoice.Rows(rowIndex)
        Dim readyRequestId As String = CType(gvrReady.FindControl("lblRequestId"), Label).Text.Trim
        If readyRequestId = hdRequestId.Value.Trim Then
            Dim readyAmountReceived As Label = CType(gvrReady.FindControl("lblAmountReceived"), Label)
            If AmtReceived > 0 Then
                readyAmountReceived.Text = AmtReceived
            Else
                readyAmountReceived.Text = ""
            End If
        End If
    End Sub
#End Region

    'Protected Sub btnSchedule_Click(sender As Object, e As System.EventArgs) Handles btnSchedule.Click
    '    'Tanvir 27102023
    '    txtscheduledate.Visible = True
    '    txttime.Visible = True
    '    ImgBtnscheduledate.Visible = True
    '    'Tanvir 27102023
    'End Sub

    Protected Sub chkschedule_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkschedule.CheckedChanged
        If chkschedule.Checked = True Then
            txtscheduledate.Visible = True
            txttime.Visible = True
            ImgBtnscheduledate.Visible = True
            btnsaveschedule.Visible = True
        Else
            txtscheduledate.Visible = False
            txttime.Visible = False
            ImgBtnscheduledate.Visible = False
            btnsaveschedule.Visible = False
        End If
    End Sub
    'Tanvir 27102023
    Function validatescheduledate() As Boolean
        Dim dateStr As String = Convert.ToDateTime(txtscheduledate.Text).ToString("yyyy/MM/dd")
        Dim timeStr As String = txttime.Text
        Dim combinedDateTimeStr As String = dateStr & " " & timeStr

        validatescheduledate = True
        Dim scheduleid As String
        scheduleid = (CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select scheduleid  From Invoicing_search_Scheduler (nolock) where processscheduledtime='" & combinedDateTimeStr & "' and processflag <> 2"), String))
        If scheduleid <> "" Then
            validatescheduledate = False
            Exit Function
        End If
        Return validatescheduledate
    End Function
    'Tanvir 27102023
    Protected Sub btnsaveschedule_Click(sender As Object, e As System.EventArgs) Handles btnsaveschedule.Click
        Try
            btnsaveschedule.Enabled = False
            If validatescheduledate() = False Then
                '  ModalExtraPopup.Show()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Date and Time is already Scheduled.')", True)
                Exit Sub

            End If

            Dim strSortBy As String = Convert.ToString(Session("strsortExpressionReady"))
            Dim strBindCondition As String = BuildCondition()
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("sp_add_Invoice_Scheduler", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandTimeout = 0 'Tanvir 27022023
            mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = Convert.ToString(ViewState("divcode"))
            mySqlCmd.Parameters.Add(New SqlParameter("@frmcheckindate", SqlDbType.DateTime)).Value = Convert.ToDateTime(txtChkFromDt.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@tocheckindate", SqlDbType.DateTime)).Value = Convert.ToDateTime(txtChkToDt.Text).ToString("yyyy/MM/dd")
            mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 200)).Value = txtBookingNo.Text.Trim.ToUpper
            mySqlCmd.Parameters.Add(New SqlParameter("@agentname", SqlDbType.VarChar, 200)).Value = txtCustName.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@partyname", SqlDbType.VarChar, 20)).Value = txtSupName.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@agentrefno", SqlDbType.VarChar, 20)).Value = txtCustRef.Text.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@bookingengineratetype", SqlDbType.VarChar, 20)).Value = ddlBookingType.SelectedValue
            mySqlCmd.Parameters.Add(New SqlParameter("@reqfrmdate", SqlDbType.DateTime)).Value = IIf(txtReqFromDt.Text <> "", Format((txtReqFromDt.Text), "yyyy/MM/dd"), DBNull.Value)
            mySqlCmd.Parameters.Add(New SqlParameter("@reqtodate", SqlDbType.DateTime)).Value = IIf(txtReqToDt.Text <> "", Format((txtReqToDt.Text), "yyyy/MM/dd"), DBNull.Value)
            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
            mySqlCmd.Parameters.Add(New SqlParameter("@bindCond", SqlDbType.VarChar)).Value = strBindCondition
            mySqlCmd.Parameters.Add(New SqlParameter("@sortBy", SqlDbType.VarChar, 100)).Value = strSortBy
            mySqlCmd.Parameters.Add(New SqlParameter("@SalesInvoiceState", SqlDbType.VarChar, 20)).Value = Session("SalesInvoiceState")
            mySqlCmd.Parameters.Add(New SqlParameter("@dateFlag", SqlDbType.VarChar, 1)).Value = hdnChkDtFlag.Value.Trim
            Dim dateStr As String = Convert.ToDateTime(txtscheduledate.Text).ToString("yyyy/MM/dd")
            Dim timeStr As String = txttime.Text
            Dim combinedDateTimeStr As String = dateStr & " " & timeStr
            Dim dateTimeValue As DateTime

            If DateTime.TryParseExact(combinedDateTimeStr, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, dateTimeValue) Then
                Dim formattedDateTime As String = dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss")
                mySqlCmd.Parameters.Add(New SqlParameter("@processscheduledtime", SqlDbType.DateTime)).Value = CType(Convert.ToDateTime((txtChkToDt.Text).ToString("yyyy/MM/dd") + " " + CType(txttime.Text, String)), String)
            Else
                mySqlCmd.Parameters.Add(New SqlParameter("@processscheduledtime", SqlDbType.DateTime)).Value = combinedDateTimeStr 'C onvert.ToDateTime(txtChkToDt.Text).ToString("yyyy/MM/dd")
            End If

            mySqlCmd.Parameters.Add(New SqlParameter("@processflag", SqlDbType.Int)).Value = 0
            mySqlCmd.ExecuteNonQuery()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Schedule has saved sucessfully !!');", True)
            btnsaveschedule.Enabled = True
        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + HttpUtility.JavaScriptStringEncode(ex.Message) & "' );", True)
            objUtils.WritErrorLog("SalesInvoiceNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            btnsaveschedule.Enabled = True
        End Try
    End Sub

    'Rosalin 27/10/2023
    Protected Sub btnViewSchedule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewSchedule.Click
        Dim strpop As String = ""
        strpop = "window.open('SalesInvoiceScheduler.aspx?State=New&divid=" & ViewState("divcode") & "' ,'SalesInvoice');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup1", strpop, True)
    End Sub
End Class
