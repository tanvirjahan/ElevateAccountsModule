Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.IO
Imports System.Collections.Generic

Partial Class ImportBookingRecalculate
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

                'Dim appid As String = CType(Request.QueryString("appid"), String)

                'Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                'Dim strappname As String = ""
                'strappname = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(displayName,'') as displayname from appmaster a inner join division_master d on a.displayname=d.accountsmodulename where a.appid='" & appid & "'")
                'ViewState("Appname") = strappname
                'Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & ViewState("Appname") & "'")
                'ViewState.Add("divcode", divid)


                'If Convert.ToString(Request.QueryString("State")) = "View" Then
                '    Session("SalesInvoiceState") = "View"
                '    Page.Title = "View Sales Invoice"
                '    lblHeading.Text = "View Sales Invoice"
                'ElseIf Convert.ToString(Request.QueryString("State")) = "Amend" Then
                Session("SalesInvoiceState") = "Recalculate"
                Page.Title = "Import Booking Element Recalculate"
                lblHeading.Text = "Import Booking Element Recalculate"
                'btnCancel.Visible = False
                'ElseIf Convert.ToString(Request.QueryString("State")) = "New" Then
                '    Session("SalesInvoiceState") = "New"
                '    Page.Title = "New Sales Invoice"
                '    lblHeading.Text = "New Sales Invoice"

                '    divAdvanced.Visible = True

                'Else
                '    Session("SalesInvoiceState") = ""
                'End If
                Dim strBookingType As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='2036'")
                Dim arrBookingType As String() = strBookingType.Split(";")
                If arrBookingType.Length > 0 Then
                    For i = 0 To arrBookingType.GetUpperBound(0)
                        ddlBookingType.Items.Add(New ListItem(arrBookingType(i).Trim, arrBookingType(i).Trim))
                    Next
                Else
                    ddlBookingType.Items.Add(New ListItem("All", "All"))
                End If

                ViewState.Add("divcode", Request.QueryString("divid"))
                txtDivCode.Text = ViewState("divcode")
                Dim baseCurrency As String = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='457'")
                gvReadyInvoice.Columns(11).HeaderText = "Sales Amount (" + baseCurrency + ")"
                'gvPending.Columns(10).HeaderText = "Sales Amount (" + baseCurrency + ")"

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

                'If Convert.ToString(Session("SalesInvoiceState")) = "View" Then
                '    Dim invoiceNo As String
                '    invoiceNo = CType(Request.QueryString("ID"), String)
                '    FillEntry(invoiceNo)
                '    gvPending.DataSource = dt
                '    gvPending.DataBind()
                '    DisableControl()
                'ElseIf Convert.ToString(Session("SalesInvoiceState")) = "Amend" Then
                '    gvReadyInvoice.Columns(3).HeaderText = "Amended Date"
                '    gvReadyInvoice.DataSource = dt
                '    gvReadyInvoice.DataBind()
                '    gvPending.Columns(2).HeaderText = "Amended Date"
                '    gvPending.DataSource = dt
                '    gvPending.DataBind()
                'ElseIf Convert.ToString(Session("SalesInvoiceState")) = "New" Then
                '    gvReadyInvoice.DataSource = dt
                '    gvReadyInvoice.DataBind()
                '    gvPending.DataSource = dt
                '    gvPending.DataBind()
                'Else
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
                'End If

                If Convert.ToString(Session("SalesInvoiceState")) = "Recalculate" Then
                    gvReadyInvoice.DataSource = dt
                    gvReadyInvoice.DataBind()
                    'gvPending.DataSource = dt
                    'gvPending.DataBind()
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
                End If

                lblpurchaseinvoice.Visible = False

                Dim firstday = New DateTime(Date.Now.Year, Date.Now.Month, 1)
                Dim lastday = firstday.AddMonths(1).AddDays(-1)
                txtChkFromDt.Text = firstday.ToString("dd/MM/yyyy")
                txtChkToDt.Text = lastday.ToString("dd/MM/yyyy")

                Session("bookingInvoice") = dt
                Session.Add("strsortexpressionReady", "DepartureDate")
                Session.Add("strsortdirectionReady", SortDirection.Ascending)

                Session.Add("strsortexpressionPending", "DepartureDate")
                Session.Add("strsortdirectionPending", SortDirection.Ascending)
                'btnGenInvoice.Enabled = False
                'btnGenInvoice.Attributes.Add("class", "disablebtn")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ImportBookingRecalculate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        'gvPending.Enabled = False
        'btnValidate.Enabled = False
        'btnGenInvoice.Enabled = False
        btnRecalculate.Enabled = False
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
            objUtils.WritErrorLog("ImportBookingRecalculate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("ImportBookingRecalculate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Function BuildCondition() As String"
    Protected Function BuildCondition() As String
        Try
            Dim strWhereCond As String = ""
            'If Convert.ToString(Session("SalesInvoiceState")) <> "Amend" Then
            '    strWhereCond = "isnull(invno,'')='' "
            'End If
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

#Region "Protected Sub btnDisplay_Click(sender As Object, e As System.EventArgs) Handles btnDisplay.Click"
    Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Try
            If Session("SalesInvoiceState") <> "Recalculate" And Convert.ToString(ViewState("divcode")) <> "" Then 'Session("SalesInvoiceState") <> "Amend" And Session("SalesInvoiceState") <> "New"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Invalid page state' );", True)
                Exit Sub
            End If
            TabBookList.ActiveTab = panConfirm
            DisplayConfirmedRecords()
            'DisplayPendingRecords()
            'gvErrorList.DataSource = Nothing
            'gvErrorList.DataBind()
            'btnValidate.Enabled = True
            'btnValidate.Attributes.Add("class", "btn")
            'lblValidateMsg.Visible = False
            'btnGenInvoice.Enabled = False
            'btnGenInvoice.Attributes.Add("class", "disablebtn
            btnRecalculate.Enabled = True
            btnRecalculate.Attributes.Add("class", "btn")
            Dim dtReceipt As DataTable = CType(Session("dtReceipt"), DataTable)
            dtReceipt.Clear()
        Catch ex As Exception
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + HttpUtility.JavaScriptStringEncode(ex.Message) & "' );", True)
            objUtils.WritErrorLog("ImportBookingRecalculate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub DisplayConfirmedRecords()"
    Protected Sub DisplayConfirmedRecords()
        Try
            lblMsg.Visible = False
            Dim myDS As New DataSet
            Dim strBindCondition As String = BuildCondition()
            Dim strSortBy As String = Convert.ToString(Session("strsortExpressionReady"))
            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("booking_recalculate_search", sqlConn)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@divCode", SqlDbType.VarChar, 20)).Value = Convert.ToString(ViewState("divcode"))
            mySqlCmd.Parameters.Add(New SqlParameter("@bindCond", SqlDbType.VarChar)).Value = strBindCondition
            mySqlCmd.Parameters.Add(New SqlParameter("@sortBy", SqlDbType.VarChar, 100)).Value = strSortBy
            mySqlCmd.Parameters.Add(New SqlParameter("@FromChkOutDt", SqlDbType.VarChar, 10)).Value = Convert.ToString(Convert.ToDateTime(txtChkFromDt.Text).ToString("yyyy/MM/dd"))
            mySqlCmd.Parameters.Add(New SqlParameter("@ToChkOutDt", SqlDbType.VarChar, 10)).Value = Convert.ToString(Convert.ToDateTime(txtChkToDt.Text).ToString("yyyy/MM/dd"))
            mySqlCmd.Parameters.Add(New SqlParameter("@SalesInvoiceState", SqlDbType.VarChar, 20)).Value = Session("SalesInvoiceState")
            mySqlCmd.Parameters.Add(New SqlParameter("@requestId", SqlDbType.VarChar, 20)).Value = txtBookingNo.Text.Trim.ToUpper

            'sharfudeen 20/09/2022
            'Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
            '    Using dt As New DataTable()
            '        myDataAdapter.Fill(dt)
            '        Dim dc As New DataColumn("Selection", GetType(Boolean))
            '        dc.DefaultValue = False
            '        dt.Columns.Add(dc)
            '        'Dim dc1 As New DataColumn("InvoiceNo", GetType(String))
            '        'dc1.DefaultValue = ""
            '        'dt.Columns.Add(dc1)
            '        gvReadyInvoice.DataSource = dt
            '        gvReadyInvoice.DataBind()
            '        If dt.Rows.Count = 0 Then
            '            lblMsg.Visible = True
            '        End If
            '    End Using
            'End Using

            Using myDataAdapter As New SqlDataAdapter(mySqlCmd)
                Using dt As New DataTable()
                    myDataAdapter.Fill(myDS)
                    Dim dc As New DataColumn("Selection", GetType(Boolean))
                    dc.DefaultValue = False
                    myDS.Tables(0).Columns.Add(dc)
                    'Dim dc1 As New DataColumn("InvoiceNo", GetType(String))
                    'dc1.DefaultValue = ""
                    'dt.Columns.Add(dc1)
                    gvReadyInvoice.DataSource = myDS.Tables(0)
                    gvReadyInvoice.DataBind()
                    If myDS.Tables(0).Rows.Count = 0 Then
                        lblMsg.Visible = True
                    End If
                    If myDS.Tables(1).Rows.Count > 0 Then
                        gvPurchaseInvoice.DataSource = myDS.Tables(1)
                        gvPurchaseInvoice.DataBind()
                        lblpurchaseinvoice.Visible = True
                    End If
                    If myDS.Tables(1).Rows.Count = 0 Then
                        lblPMsg.Visible = True
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
        'gvPending.DataSource = dt
        'gvPending.DataBind()
        TabBookList.ActiveTab = panConfirm
        'gvErrorList.DataSource = Nothing
        'gvErrorList.DataBind()
    End Sub
#End Region

#Region "Protected Sub gvPurchaseInvoice_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPurchaseInvoice.RowCommand"
    Protected Sub gvPurchaseInvoice_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPurchaseInvoice.RowCommand
        Try
            If e.CommandName = "View" Then
                Dim lbtnView As LinkButton
                lbtnView = CType(gvPurchaseInvoice.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lbtnView"), LinkButton)
                Dim invoiceno As String = lbtnView.Text.Trim
                If invoiceno <> "" Then
                    Dim strpop As String = ""
                    strpop = "window.open('PrintReport.aspx?printId=salesInvoice&InvoiceNo=" & invoiceno & "','SalesInvoicePrint');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "salesInvoice", strpop, True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ImportBookingRecalculate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

#Region "Protected Sub gvReadyInvoice_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvReadyInvoice.RowCommand"
    Protected Sub gvReadyInvoice_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvReadyInvoice.RowCommand
        Try
            If e.CommandName = "View" Then
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
            objUtils.WritErrorLog("ImportBookingRecalculate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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



    '#Region "Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click"
    '    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    '    End Sub
    '#End Region

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

#Region "Protected Sub btnRecalculate_Click(sender As Object, e As System.EventArgs) Handles btnRecalculate.Click"
    Protected Sub btnRecalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRecalculate.Click
        Try
            'btnGenInvoice.Enabled = False
            'btnValidate.Enabled = False
            'btnRecalculate.Enabled = False
            Dim dtReceipt As DataTable = CType(Session("dtReceipt"), DataTable)
            Dim chkSelection As New CheckBox
            Dim lblRequestId As New Label
            Dim lblAmountReceived As New Label

            Dim sealdate As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sealdate from  sealing_master")
            Dim hdnArrivalDt As HiddenField
            Dim hdnDepartureDt As HiddenField
            Dim strRequestid As String = ""

            For Each dr As GridViewRow In gvReadyInvoice.Rows
                chkSelection = CType(dr.FindControl("chkSelection"), CheckBox)
                If chkSelection.Checked = True Then
                    lblRequestId = CType(dr.FindControl("lblRequestId"), Label)
                    If objUtils.EntryExists(Session("dbconnectionName"), "viewPurchasedetails", "requestid", " requestid='" & lblRequestId.Text & "' ") Then
                        strRequestid = strRequestid + "," + lblRequestId.Text
                        chkSelection.Checked = False
                    End If
                End If
            Next
            If strRequestid <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Purchase invoice already created for this bookings : " + strRequestid + "' );", True)
                Exit Sub
            End If

            sqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open

            Dim invRequestIds As String = ""
            For Each dr As GridViewRow In gvReadyInvoice.Rows
                chkSelection = CType(dr.FindControl("chkSelection"), CheckBox)
                If chkSelection.Checked = True Then
                    sqlTrans = sqlConn.BeginTransaction
                    lblRequestId = CType(dr.FindControl("lblRequestId"), Label)
                    hdnArrivalDt = CType(dr.FindControl("hdnArrivalDt"), HiddenField)
                    hdnDepartureDt = CType(dr.FindControl("hdnDepartureDt"), HiddenField)

                    'added param on 22/07/2021
                    If IsDate(sealdate) Then
                        Dim invoicedate As String = ""
                        invoicedate = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select invoicedate from InvoiceHeader(nolock) where requestid='" + lblRequestId.Text + "'")
                        If IsDate(invoicedate) Then
                            If (CType(invoicedate, Date) <= CType(sealdate, Date)) Then
                                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Booking(" + lblRequestId.Text + ") invoice Date with in sealed date - " & CType(sealdate, Date).ToString("dd/MM/yyyy") & "');", True)
                                Exit Sub
                            End If
                        Else
                            If hdnChkDtFlag.Value = "Y" Then
                                If (CType(hdnArrivalDt.Value, Date) <= CType(sealdate, Date)) Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Booking(" + lblRequestId.Text + ") Dates with in sealed date - " & CType(sealdate, Date).ToString("dd/MM/yyyy") & "');", True)
                                    Exit Sub
                                End If
                            Else
                                If (CType(hdnDepartureDt.Value, Date) <= CType(sealdate, Date)) Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Booking(" + lblRequestId.Text + ") Dates with in sealed date - " & CType(sealdate, Date).ToString("dd/MM/yyyy") & "');", True)
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If

                    If invRequestIds = "" Then
                        invRequestIds = "'" + lblRequestId.Text.Trim + "'"
                    Else
                        invRequestIds = invRequestIds + ",'" + lblRequestId.Text.Trim + "'"
                    End If

                    mySqlCmd = New SqlCommand("sp_recalculate_importBooking", sqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@bookingCode", SqlDbType.VarChar, 20)).Value = lblRequestId.Text.Trim
                    mySqlCmd.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 100)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()
                    clsDBConnect.dbCommandClose(mySqlCmd)
                    sqlTrans.Commit()
                    'sqlTrans = Nothing
                End If
            Next

            clsDBConnect.dbSqlTransation(sqlTrans)
            clsDBConnect.dbConnectionClose(sqlConn)
            sqlTrans = Nothing

            'btnGenInvoice.Enabled = False
            'btnGenInvoice.Attributes.Add("class", "disablebtn")
            'btnValidate.Enabled = False
            'btnValidate.Attributes.Add("class", "disablebtn")
            'btnRecalculate.Enabled = False
            'btnRecalculate.Attributes.Add("class", "disablebtn")

            
            Dim msg As String = ""
            msg = "alert('Booking Value including VAT are recalculated successfully' );"
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
            objUtils.WritErrorLog("ImportBookingRecalculate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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


End Class
