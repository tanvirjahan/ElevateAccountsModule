#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Collections.Generic
Imports System.IO
Imports System.Web
Imports System.Net.Mail
Imports System.Web.HttpServerUtility
#End Region
Partial Class AccountsModule_RptviewProfitnLoss
    Inherits System.Web.UI.Page





#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objdatetime As New clsDateTime
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        hdnPrivilage.Value = IsEditPrivilege()
        If Page.IsPostBack = False Then
            Try
                txtconnection.Value = Session("dbconnectionName")
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                If AppId Is Nothing = False Then
                    strappid = AppId.Value
                End If
                If AppName Is Nothing = False Then
                    strappname = AppName.Value
                End If
                If CType(Session("GlobalUserName"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                Else
                  

                End If

                Divmain.Visible = True
                btnExportToExcel.Visible = False
                ddlOrderBy.SelectedIndex = 8
                LoadControls()
                FillGrid()
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ReservationSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ChildWindowPostBack") Then
            If gvSearchRes.Rows.Count > 0 Then

                Dim ds As DataSet = DoExactSearch()
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        lblMsg.Visible = False
                        gvSearchRes.DataSource = ds.Tables(0)
                        gvSearchRes.DataBind()
                        gvSearchRes.Visible = True
                    Else
                        gvSearchRes.Visible = False
                        lblMsg.Visible = True
                    End If
                End If
            End If
        End If

        Dim typ As Type
        typ = GetType(DropDownList)


        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
            ddlSupplierCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSupplierName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        End If
    End Sub
    Protected Sub gvSearchRes_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)


    End Sub


  

#Region "Public Function IsPrivilege() As Boolean"
    Public Function IsEditPrivilege() As Integer
        Try
            Dim strSql As String
            Dim usrCode As String
            usrCode = CType(Session("GlobalUserName"), String)
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            strSql = "select appid from group_privilege_Detail where privilegeid='12' and appid='3' and "
            strSql += "groupid=(SELECT groupid FROM UserMaster WHERE UserCode='" + usrCode + "')"
            mySqlCmd = New SqlCommand(strSql, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                Return 1
            Else
                Return 0
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Reservation.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close 
        End Try
    End Function
#End Region


    Protected Sub rbnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        pnlSearch.Visible = True

        'Label1.Visible = True
        'Label10.Visible = True
        'Label11.Visible = True
        ''Label12.Visible = True
        ''Label13.Visible = True
        ''Label14.Visible = True
        'Label15.Visible = True
        'Label16.Visible = True
        'Label17.Visible = True
        ''Label18.Visible = True
        ''Label19.Visible = True
        'Label2.Visible = True
        'Label3.Visible = True
        'Label4.Visible = True
        'Label5.Visible = True
        'Label6.Visible = True
        'Label7.Visible = True
        'Label8.Visible = True
        'Label9.Visible = True
        'txtCustRef.Visible = True
        'txtGFname.Visible = True
        'txtGLname.Visible = True
        'ddlCustomerCode.Visible = True
        'ddlCustomerName.Visible = True
        ''ddlFilterBy.Visible = True
        ''ddlOrderBy.Visible = True
        'ddlSupplerAgentName.Visible = True
        'ddlSupplierAgentCode.Visible = True
        'ddlSupplierCode.Visible = True
        'ddlSupplierName.Visible = True
        'ddlUserCode.Visible = True
        'ddlUserName.Visible = True
        'ddlSubUserCode.Visible = True
        'ddlSubUserName.Visible = True
        'dpFromCheckindate.Visible = True
        'dpFromCheckOut.Visible = True
        'dpFromReqDate.Visible = True
        'dpToCheckindate.Visible = True
        'dpTocheckOut.Visible = True
        'dpToReqDate.Visible = True
    End Sub

    Protected Sub rbsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        pnlSearch.Visible = False


        'Label1.Visible = False
        'Label10.Visible = False
        'Label11.Visible = False
        ''Label12.Visible = False
        ''Label13.Visible = False
        ''Label14.Visible = False
        'Label15.Visible = False
        'Label16.Visible = False
        'Label17.Visible = False
        ''Label18.Visible = False
        ''Label19.Visible = False
        'Label2.Visible = False
        'Label3.Visible = False
        'Label4.Visible = False
        'Label5.Visible = False
        'Label6.Visible = False
        'Label7.Visible = False
        'Label8.Visible = False
        'Label9.Visible = False
        'txtCustRef.Visible = False
        'txtGFname.Visible = False
        'txtGLname.Visible = False
        'ddlCustomerCode.Visible = False
        'ddlCustomerName.Visible = False
        ''ddlFilterBy.Visible = False
        ''ddlOrderBy.Visible = False
        'ddlSupplerAgentName.Visible = False
        'ddlSupplierAgentCode.Visible = False
        'ddlSupplierCode.Visible = False
        'ddlSupplierName.Visible = False
        'ddlUserCode.Visible = False
        'ddlUserName.Visible = False
        'ddlSubUserCode.Visible = False
        'ddlSubUserName.Visible = False
        'dpFromCheckindate.Visible = False
        'dpFromCheckOut.Visible = False
        'dpFromReqDate.Visible = False
        'dpToCheckindate.Visible = False
        'dpTocheckOut.Visible = False
        'dpToReqDate.Visible = False
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gvSearchRes.PageIndex = 0
        Dim ds As DataSet = DoExactSearch()
        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                lblMsg.Visible = False
                gvSearchRes.DataSource = ds.Tables(0)
                gvSearchRes.DataBind()
                gvSearchRes.Visible = True
            Else
                gvSearchRes.Visible = False
                lblMsg.Visible = True
            End If
        Else
            gvSearchRes.Visible = False
            lblMsg.Visible = True
        End If

    End Sub

#Region "Public Function LoadControls()"
    Public Sub LoadControls()
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUserCode, "usercode", "username", "select usercode,username  from UserMaster where active=1 order by usercode", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlUserName, "username", "usercode", "select usercode,username  from UserMaster where active=1 order by username", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomerCode, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1 order by agentcode", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomerName, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname ", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "partycode", "partyname", "select partyname,partycode from partymast where active ='1' order by partycode", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierName, "partyname", "partycode", "select partyname,partycode from partymast where active ='1' order by partyname", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierAgentCode, "supagentcode", "supagentname", "select supagentcode,supagentname from supplier_agents where active ='1' order by supagentcode", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplerAgentName, "supagentname", "supagentcode", "select supagentname,supagentcode from supplier_agents where active ='1' order by supagentname", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubUserCode, "AGENT_SUB_CODE", "Sub_User_Name", "select AGENT_SUB_CODE,Sub_User_Name from agents_subusers order by AGENT_SUB_CODE", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSubUserName, "Sub_User_Name", "AGENT_SUB_CODE", "select Sub_User_Name,AGENT_SUB_CODE from agents_subusers order by Sub_User_Name", True)
        Dim typ As Type
        typ = GetType(DropDownList)
        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
            ddlCustomerCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCustomerName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSupplierCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSupplierName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSupplierAgentCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSupplerAgentName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSubUserCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlSubUserName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlUserCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlUserName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        End If

        txtCustRef.Value = ""
        txtGFname.Value = ""
        txtGLname.Value = ""
        txtReqId.Value = ""
        txtsno.Value = ""
        dpFromCheckindate.txtDate.Text = ""
        dpToCheckindate.txtDate.Text = ""
        dpFromCheckOut.txtDate.Text = ""
        dpFromReqDate.txtDate.Text = ""
        dpTocheckOut.txtDate.Text = ""
        dpToReqDate.txtDate.Text = ""
        txtFromDate.Text = ""
        rdbLike.Checked = True
    End Sub
#End Region

#Region "Public Function DoExactSearch()"
    Public Function DoExactSearch() As DataSet
        Dim parms As New List(Of SqlParameter)
        Dim i As Integer
        Dim parm(23) As SqlParameter

        If Not (txtReqId.Value.Trim().ToString() = "") Then
            parm(0) = New SqlParameter("@requestid", CType(txtReqId.Value.Trim().ToString(), String))
        Else
            parm(0) = New SqlParameter("@requestid", String.Empty)
        End If

        If Not (txtCustRef.Value.Trim().ToString() = "") Then
            parm(1) = New SqlParameter("@agentref", CType(txtCustRef.Value.Trim().ToString(), String))
        Else
            parm(1) = New SqlParameter("@agentref", String.Empty)
        End If
        If Not (ddlUserName.Value = "" Or ddlUserName.Value = "[Select]") Then
            parm(2) = New SqlParameter("@usercode", CType(ddlUserName.Value, String))
        Else
            parm(2) = New SqlParameter("@usercode", String.Empty)
        End If
        If Not (dpFromReqDate.txtDate.Text = "") Then
            parm(3) = New SqlParameter("@fromrequestdate", objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(dpFromReqDate.txtDate.Text, String)))
        Else
            parm(3) = New SqlParameter("@fromrequestdate", "1900/01/01")
        End If
        If Not (dpToReqDate.txtDate.Text = "") Then
            parm(4) = New SqlParameter("@torequestdate", objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(dpToReqDate.txtDate.Text, String)))
        Else
            parm(4) = New SqlParameter("@torequestdate", "1900/01/01")
        End If
        If Not (txtFromDate.Text.Trim = "") Then
            parm(5) = New SqlParameter("@fromcheckindate", objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(txtFromDate.Text.Trim, String)))
        Else
            parm(5) = New SqlParameter("@fromcheckindate", "1900/01/01")
        End If
        If Not (txtFromDate.Text.Trim = "") Then
            parm(6) = New SqlParameter("@tocheckindate", objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(txtToDate.Text.Trim, String)))
        Else
            parm(6) = New SqlParameter("@tocheckindate", "1900/01/01")
        End If
        If Not (dpFromCheckOut.txtDate.Text = "") Then
            parm(7) = New SqlParameter("@fromcheckoutdate", objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(dpFromCheckOut.txtDate.Text, String)))
        Else
            parm(7) = New SqlParameter("@fromcheckoutdate", "1900/01/01")
        End If
        If Not (dpTocheckOut.txtDate.Text = "") Then
            parm(8) = New SqlParameter("@tocheckoutdate", objdatetime.ConvertDateromTextBoxToTextYearMonthDay(CType(dpTocheckOut.txtDate.Text, String)))
        Else
            parm(8) = New SqlParameter("@tocheckoutdate", "1900/01/01")
        End If
        If Not (ddlCustomerName.Value = "" Or ddlCustomerName.Value = "[Select]") Then
            parm(9) = New SqlParameter("@agentcode", CType(ddlCustomerName.Value, String))
        Else
            parm(9) = New SqlParameter("@agentcode", String.Empty)
        End If
        If Not (ddlSupplierName.Value = "" Or ddlSupplierName.Value = "[Select]") Then
            parm(10) = New SqlParameter("@partycode", CType(ddlSupplierName.Value, String))
        Else
            parm(10) = New SqlParameter("@partycode", String.Empty)
        End If
        If Not (ddlSupplerAgentName.Value = "" Or ddlSupplerAgentName.Value = "[Select]") Then
            parm(11) = New SqlParameter("@supagentcode", CType(ddlSupplerAgentName.Value, String))
        Else
            parm(11) = New SqlParameter("@supagentcode", String.Empty)
        End If
        If Not (txtGFname.Value = "") Then
            parm(12) = New SqlParameter("@guestfname", CType(txtGFname.Value, String))
        Else
            parm(12) = New SqlParameter("@guestfname", String.Empty)
        End If

        If Not (txtGLname.Value = "") Then
            parm(13) = New SqlParameter("@guestlname", CType(txtGLname.Value, String))
        Else
            parm(13) = New SqlParameter("@guestlname", String.Empty)
        End If
        'If ddlFilterBy.Value = "All" Then
        '    parm(14) = New SqlParameter("@filter", "0")
        'Else
        '    If ddlFilterBy.Value = "Confirmed" Then
        '        parm(14) = New SqlParameter("@filter", "1")
        '    Else
        '        If ddlFilterBy.Value = "Pending Confirmation" Then
        '            parm(14) = New SqlParameter("@filter", "2")
        '        Else
        '            If ddlFilterBy.Value = "Cancelled" Then
        '                parm(14) = New SqlParameter("@filter", "3")
        '            Else
        '                parm(14) = New SqlParameter("@filter", "0")
        '            End If
        '        End If
        '    End If
        'End If
        parm(14) = New SqlParameter("@filter", ddlFilterBy.Value)

        If Not (ddlSubUserName.Value = "" Or ddlSubUserName.Value = "[Select]") Then
            parm(15) = New SqlParameter("@subusercode", CType(ddlSubUserName.Value, String))
        Else
            parm(15) = New SqlParameter("@subusercode", String.Empty)
        End If
        'If rblistSearch.Items(0).Selected = True Then
        '    parm(16) = New SqlParameter("@searchtype", "0")
        'Else
        '    parm(16) = New SqlParameter("@searchtype", "1")
        'End If
        If rdbExact.Checked = True Then
            parm(16) = New SqlParameter("@searchtype", "0")
        ElseIf rdbLike.Checked = True Then
            parm(16) = New SqlParameter("@searchtype", "1")
        End If
        If ddlOrderBy.Value = "Sno" Then
            parm(17) = New SqlParameter("@orderby", "8")
        ElseIf ddlOrderBy.Value = "Fileno Desc" Then
            parm(17) = New SqlParameter("@orderby", "0")
        Else
            If ddlOrderBy.Value = "Fileno Asc" Then
                parm(17) = New SqlParameter("@orderby", "1")
            Else
                If ddlOrderBy.Value = "Request Date" Then
                    parm(17) = New SqlParameter("@orderby", "2")
                Else
                    If ddlOrderBy.Value = "Check in Date" Then
                        parm(17) = New SqlParameter("@orderby", "3")
                    Else
                        If ddlOrderBy.Value = "Check out Date" Then
                            parm(17) = New SqlParameter("@orderby", "4")
                        Else
                            If ddlOrderBy.Value = "Customer" Then
                                parm(17) = New SqlParameter("@orderby", "5")
                            Else
                                If ddlOrderBy.Value = "Supplier" Then
                                    parm(17) = New SqlParameter("@orderby", "6")
                                Else
                                    parm(17) = New SqlParameter("@orderby", "7")
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
        If Not (txtsno.Value.Trim().ToString() = "") Then
            parm(18) = New SqlParameter("@sno", CType(txtsno.Value.Trim().ToString(), String))
        Else
            parm(18) = New SqlParameter("@sno", String.Empty)
        End If

        parm(19) = New SqlParameter("@deptuser", CType(Session("GlobalUserName"), String))

        If rbnallbooking.Checked = True Then
            parm(20) = New SqlParameter("@bookingstatus", 0)
        ElseIf rbnbackbooking.Checked = True Then
            parm(20) = New SqlParameter("@bookingstatus", 1)
        ElseIf rbnonlinebooking.Checked = True Then
            parm(20) = New SqlParameter("@bookingstatus", 2)
        End If
        parm(21) = New SqlParameter("@bkgStatusActiveCancelled", ddlBookingStatus.Value)
        parm(22) = New SqlParameter("@invStatus", ddlInvStatus.Value)
        For i = 0 To 22
            parms.Add(parm(i))
        Next
        Dim ds As DataSet = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_request_search_new", parms)
        Return ds


    End Function
#End Region
    Private Sub FillGrid()
        Dim ds As DataSet = DoExactSearch()
        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                lblMsg.Visible = False
                gvSearchRes.DataSource = ds.Tables(0)
                gvSearchRes.DataBind()
                gvSearchRes.Visible = True
            Else
                gvSearchRes.Visible = False
                lblMsg.Visible = True
            End If
        End If
    End Sub
#Region "Public Function FormatDate()"
    Public Function FormatDate(ByVal obj As Object) As String
        If Not (obj Is Nothing) Then
            If (obj.ToString() = "") = False Then
                Return CType(obj.ToString(), Date).ToShortDateString()
            End If
        Else
            Return ""
        End If
    End Function
#End Region

    Protected Sub gvSearchRes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSearchRes.PageIndexChanging
        gvSearchRes.PageIndex = e.NewPageIndex
        Dim ds As DataSet = DoExactSearch()

        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                lblMsg.Visible = False
                gvSearchRes.DataSource = ds.Tables(0)
                gvSearchRes.DataBind()
                gvSearchRes.Visible = True
            Else
                gvSearchRes.Visible = False
                lblMsg.Visible = True
            End If
        End If
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        LoadControls()
        FillGrid()
    End Sub

    Protected Sub gvSearchRes_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSearchRes.RowCommand
        Try
            If e.CommandName = "Page" Then
                Exit Sub
            End If
            Dim i As Integer = 0
        If e.CommandName = "Printpl" Then
                Dim rowindex As Integer = CInt(e.CommandArgument)
                Dim row As GridViewRow = gvSearchRes.Rows(rowindex)
                Dim lblReqid As Label = DirectCast(row.FindControl("lblReqid"), Label)
                Dim strpop As String = ""
                strpop = "window.open('../Reservation/rptConfirmaton.aspx?reqid=" + lblReqid.Text + "&typ=printpl','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,resizable=yes,status=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReservationSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

   

#Region "Public Function IsPrivilege() As Boolean"
    Public Function IsInvoiced(ByVal Reqid) As Integer
        Try
            Dim strSql As String
            Dim usrCode As String
            usrCode = CType(Session("GlobalUserName"), String)
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            strSql = "select 't' from reservation_headernew where requestid='" + Reqid + "' and isnull(invoiced,0)=1  and isnull(invno,'')<>''"

            mySqlCmd = New SqlCommand(strSql, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                Return 1
            Else
                Return 0
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Reservation.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close 
        End Try
    End Function
#End Region

    Protected Sub gvSearchRes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSearchRes.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim strpost As String = ""
            Dim strseal As String = ""
            Dim lblReqid As Label = CType(e.Row.FindControl("lblReqid"), Label)
            Dim hdnPosted As HiddenField = CType(e.Row.FindControl("hdnPosted"), HiddenField)
            Dim hdnInvoiced As HiddenField = CType(e.Row.FindControl("hdnInvoiced"), HiddenField)
            Dim hdnseal As HiddenField = CType(e.Row.FindControl("hdnsealed"), HiddenField)
            hdnInvoiced.Value = IsInvoiced(lblReqid.Text)
            Dim usrCode As String = CType(Session("GlobalUserName"), String)

            Dim lnkInvoice As LinkButton = CType(e.Row.FindControl("lnkInvoice"), LinkButton)

            strpost = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(post_state,'')  from reservation_invoice_header(nolock) where  requestid='" & lblReqid.Text & "'")
            strseal = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(tran_state,'')  from reservation_invoice_header(nolock) where  requestid='" & lblReqid.Text & "'")

            If strpost = "P" Then
                hdnPosted.Value = 1
            Else
                hdnPosted.Value = 0
            End If

            If strseal = "S" Then
                hdnseal.Value = 1
            Else
                hdnseal.Value = 0
            End If


            Dim lnkPrintpl As LinkButton = CType(e.Row.FindControl("lnkView"), LinkButton)
            ' Dim lnkplMail As LinkButton = CType(e.Row.FindControl("lnkplMail"), LinkButton)

            'Dim lnkTourExpSheet As LinkButton = CType(e.Row.FindControl("lnkTourExpSheet"), LinkButton)

            Dim performa As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_chk_proforma('" & lblReqid.Text & "')")
            If performa <> 1 Then


                lnkPrintpl.Enabled = False
                '  lnkplMail.Enabled = False
            End If
            Dim cancelled As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_chk_Activecancelled('" & lblReqid.Text & "')")


            If cancelled <> 1 Then
                lnkPrintpl.Enabled = False
            End If
            '  Dim amendno As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(max(amendno),0) from  reservation_invoice_amendments  where amended=0 and requestid='" & lblReqid.Text & "' and amenduser='" & CType(Session("GlobalUserName"), String) & "'")

            ' If amendno = 0 Then
            'lnkEdit.Attributes.Add("onclick", "return ValidateAmendment('" + hdnPosted.ClientID + "','" + hdnInvoiced.ClientID + "','" + hdnseal.ClientID + "','" + lblReqid.Text + "','" + CType(Session("GlobalUserName"), String) + "','" + txtconnection.Value + "');")


            'End If


            'If amendno > 0 Then
            '    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "javascript:hiddenvalchng();", True)
            '    '    Exit Sub
            '    lnkEdit.Attributes.Add("onclick", "return hiddenvalchng();")

            'End If

            'If hdnInvoiced.Value = 1 Then
            '    lnkInvoice.Enabled = False
            'Else
            '    lnkInvoice.Enabled = True
            'End If

        End If
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ReservationSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

End Class
