Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.ReportSource
Imports System.IO


Partial Class ReservationInvoiceSearch
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objdatetime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim objEmail As New clsEmail
#End Region
#Region "Enum parameter"
    Enum parameter
        invoiceno = 0
        requestid = 1
        agentcode = 2
        agentref = 3
        fromdate = 4
        todate = 5
        fromamount = 6
        toamount = 7
        post_state = 8
        orderby = 9
        searchExport = 10
    End Enum
#End Region
#Region "Enum GridCol"
    Enum GridCol

        chkselect = 0
        Invoiceno = 1
        status = 2
        invoicedate = 3
        requestid = 4
        customercode = 5
        customername = 6
        customerref = 7
        currncy = 8
        salamount = 9
        kwdamount = 10
        datecreated = 11
        usercreated = 12
        datemodified = 13
        usermodified = 14
        edit = 15
        view = 16
        print = 17
        email = 19

    End Enum
#End Region
    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
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
        Dim strpop As String = ""
        'strpop = "window.open('ReservationInvoicenew.aspx?State=New&appid=" + strappid + "','Reservation','width=1000,height=620 left=10,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        strpop = "window.open('ReservationInvoicenew.aspx?State=New&appid=" + strappid + "','Reservation');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        'Response.Redirect("ReservationInvoice.aspx", False)
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Page.IsPostBack = False Then
            If Request.QueryString("appid") Is Nothing = False Then
                Dim appid As String = CType(Request.QueryString("appid"), String)
                ViewState.Add("Appid", appid)
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
                    Case 14, 16
                        Me.MasterPageFile = "~/AccountsMaster.master"   'changed by mohamed on 27/08/2018
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                txtconnection.Value = Session("dbconnectionName")
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""

                Dim frmdate As String = ""
                Dim todate As String = ""


                If AppId Is Nothing = False Then
                    strappid = AppId.Value
                End If
                If AppName Is Nothing = False Then
                    '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
                    strappname = Session("AppName")
                    '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                End If
                SetFocus(txtInvoiceNo)
                If CType(Session("GlobalUserName"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType(strappname, String), "AccountsModule\ReservationInvoiceSearch.aspx?appid=" + strappid, btnAddNew, BtnExportToExcel, _
                     btnPrint, gvResult, GridCol.edit, 18, GridCol.view, 0, 0, 0, 0, 0)
                    'GridCol.email
                End If
                gvResult.Columns(15).Visible = False

                'Record list will be according to the Changing the year  
                If Not (Session("changeyear") Is Nothing) Then
                    frmdate = CDate(Session("changeyear") + "/01" + "/01")

                    If Session("changeyear") = Year(Now).ToString Then
                        todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
                    Else
                        todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
                    End If

                    If txtFromDate.Text = "" Then
                        txtFromDate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
                    End If

                    If txtToDate.Text = "" Then
                        txtToDate.Text = Format(CType(todate, Date), "dd/MM/yyy")
                    End If
                Else
                    If txtFromDate.Text = "" Then
                        txtFromDate.Text = Format(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select fdate from toursmaster"), Date), "dd/MM/yyy")
                    End If

                    If txtToDate.Text = "" Then
                        txtToDate.Text = DateAdd(DateInterval.Month, 1, objdatetime.GetSystemDateOnly(Session("dbconnectionName")))
                    End If
                End If

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomer, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname", True)

                FillGrid()
                gvResult.Columns(0).Visible = True

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlCustomer.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

                txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ReservationInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        'BtnReports.Attributes.Add("onclick", "return windows();")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ReservationWindowPostBack") Then
            FillGrid()
        End If
    End Sub
    Public Function doSearch() As DataSet
        doSearch = Nothing
        Try
            Dim count As Integer = [Enum].GetValues(GetType(parameter)).Length


            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(count) As SqlParameter

            Dim frmdate As String = ""
            Dim todate As String = ""


            If Not (txtInvoiceNo.Value = "") Then
                parm(parameter.invoiceno) = New SqlParameter("@invoiceno", CType(txtInvoiceNo.Value.Trim, String))
            Else
                parm(parameter.invoiceno) = New SqlParameter("@invoiceno", String.Empty)
            End If

            If Not (txtRequestId.Value = "") Then
                parm(parameter.requestid) = New SqlParameter("@requestid", CType(txtRequestId.Value.Trim, String))
            Else
                parm(parameter.requestid) = New SqlParameter("@requestid", String.Empty)
            End If

            If Not (ddlCustomer.Value = "[Select]") Then
                parm(parameter.agentcode) = New SqlParameter("@agentcode", CType(ddlCustomer.Value.Trim, String))
            Else
                parm(parameter.agentcode) = New SqlParameter("@agentcode", String.Empty)
            End If
            If Not (txtCustRef.Value = "") Then
                parm(parameter.agentref) = New SqlParameter("@agentref", CType(txtCustRef.Value.Trim, String))
            Else
                parm(parameter.agentref) = New SqlParameter("@agentref", String.Empty)
            End If




            'Record list will be according to the Changing the year  
            If Not (Session("changeyear") Is Nothing) Then
                If Not (txtFromDate.Text = "") Then

                    frmdate = CDate(Session("changeyear") + "/" + Month(CType(txtFromDate.Text, Date)).ToString + "/" + Day(CType(txtFromDate.Text, Date)).ToString)
                    '  parm(parameter.fromdate) = New SqlParameter("@fromdate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(frmdate), String))

                    parm(parameter.fromdate) = New SqlParameter("@fromdate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(txtFromDate.Text), String))
                End If
            Else
                parm(parameter.fromdate) = New SqlParameter("@fromdate", "1900/01/01")
            End If

            If Not (Session("changeyear") Is Nothing) Then

                If Not (txtToDate.Text = "") Then
                    
                    todate = CDate(Session("changeyear") + "/" + Month(CType(txtToDate.Text, Date)).ToString + "/" + Day(CType(txtToDate.Text, Date)).ToString)
                    'parm(parameter.todate) = New SqlParameter("@todate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(todate), String))
                    '''Commented as per rajeev required 23/12/15
                    parm(parameter.todate) = New SqlParameter("@todate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(txtToDate.Text), String))
                End If
            Else
                parm(parameter.todate) = New SqlParameter("@todate", "1900/01/01")
            End If


            If Not (txtFromAmount.Value = "") Then
                parm(parameter.fromamount) = New SqlParameter("@fromamount ", CType(txtFromAmount.Value, String))
            Else
                parm(parameter.fromamount) = New SqlParameter("@fromamount ", String.Empty)
            End If
            If Not (txtToAmount.Value = "") Then
                parm(parameter.toamount) = New SqlParameter("@toamount ", CType(txtToAmount.Value, String))
            Else
                parm(parameter.toamount) = New SqlParameter("@toamount ", String.Empty)
            End If

            If Not (ddlStatus.Value = "[Select]") Then
                parm(parameter.post_state) = New SqlParameter("@post_state", CType(ddlStatus.Value.Trim, String))
            Else
                parm(parameter.post_state) = New SqlParameter("@post_state", String.Empty)
            End If

            parm(parameter.orderby) = New SqlParameter("@orderby ", 0)


            If Not (txtSearchExport.Value = "") Then
                parm(parameter.searchExport) = New SqlParameter("@searchExport", CType(txtSearchExport.Value.Trim, String))
            Else
                parm(parameter.searchExport) = New SqlParameter("@searchExport", 0)
            End If


            ' parm(parameter.orderby) = New SqlParameter("@orderby", CType(ddlOrderBy.Value.Trim, Integer))

            For i = 0 To count - 1
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_invoice_search", parms)
            Return ds
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReservationInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

    Private Sub FillGrid()
        Try
            Dim dsResult As New DataSet
            lblMsg.Visible = False
            dsResult = doSearch()
            gvResult.Visible = True


            If gvResult.PageIndex < 0 Then
                gvResult.PageIndex = 0
            End If

            If dsResult.Tables.Count > 0 Then
                If dsResult.Tables(0).Rows.Count > 0 Then

                    gvResult.DataSource = dsResult.Tables(0)
                    gvResult.DataBind()
                    lblMsg.Text = ""
                Else
                    gvResult.Visible = False
                    lblMsg.Visible = True
                    lblMsg.Text = "No Records Found"
                End If
            Else
                gvResult.Visible = False
                lblMsg.Visible = True
                lblMsg.Text = "No Records Found"
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReservationInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub gvSearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvResult.PageIndexChanging
        gvResult.PageIndex = e.NewPageIndex
        FillGrid()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        txtSearchExport.Value = 0
        Dim frmdate As String = ""
        Dim todate As String = ""

        If txtFromDate.Text = "" Or txtToDate.Text = "" Then
            'Record list will be according to the Changing the year  
            If Not (Session("changeyear") Is Nothing) Then
                frmdate = CDate(Session("changeyear") + "/01" + "/01")

                If Session("changeyear") = Year(Now).ToString Then
                    todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
                Else
                    todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
                End If

                If txtFromDate.Text = "" Then
                    txtFromDate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
                End If

                If txtToDate.Text = "" Then
                    txtToDate.Text = Format(CType(todate, Date), "dd/MM/yyy")
                End If
            Else
                If txtFromDate.Text = "" Then
                    txtFromDate.Text = Format(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select fdate from toursmaster"), Date), "dd/MM/yyy")
                End If

                If txtToDate.Text = "" Then
                    txtToDate.Text = DateAdd(DateInterval.Month, 1, objdatetime.GetSystemDateOnly(Session("dbconnectionName")))
                End If
            End If
        End If

        '''Commented as per rajeev required 23/12/15
        'Record list will be according to the Changing the year  
        'If Session("changeyear") <> Year(CType(txtFromDate.Text, Date)).ToString Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
        '    Exit Sub
        'End If

        'If Session("changeyear") <> Year(CType(txtToDate.Text, Date)).ToString Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
        '    Exit Sub
        'End If

        FillGrid()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtInvoiceNo.Value = ""
        txtRequestId.Value = ""
        ddlCustomer.Value = "[Select]"
        ddlStatus.Value = "[Select]"
        txtCustRef.Value = ""
        txtFromDate.Text = ""
        txtToDate.Text = ""
        txtFromAmount.Value = ""
        txtToAmount.Value = ""
        ddlOrderBy.SelectedIndex = 0
        FillGrid()
    End Sub

    Protected Sub rbsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        pnlAdvSearch.Visible = False
    End Sub

    Protected Sub rbnadsearch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        pnlAdvSearch.Visible = True
    End Sub

    Protected Sub gvResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvResult.RowCommand
        Try
            If e.CommandName = "Page" Then Exit Sub
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
            Dim lblINo As Label
            Dim lblReqId As Label
            lblINo = gvResult.Rows(CType(e.CommandArgument.ToString, Integer)).FindControl("lblInvoiceNo")
            lblReqId = gvResult.Rows(CType(e.CommandArgument.ToString, Integer)).FindControl("lblRequestId")
            Dim customer As String
            customer = gvResult.Rows(CType(e.CommandArgument.ToString, Integer)).Cells(4).Text + " - " + gvResult.Rows(CType(e.CommandArgument.ToString, Integer)).Cells(5).Text


            If e.CommandName = "RowEdit" Then
                Dim strpop As String = ""

                If Validateseal(lblINo.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)

                    Return
                End If


                If ValidateRefund(lblReqId.Text) Then
                    'strpop = "window.open('RequestForInvoicing.aspx?State=Edit&appid=" + strappid + "&InvoiceNo=" + CType(lblINo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "','Reservation','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('RequestForInvoicing.aspx?State=Edit&appid=" + strappid + "&InvoiceNo=" + CType(lblINo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "','Reservation');"
                Else
                    strpop = "alert('You can not edit the Invoice, There is a refund request for this request.')"

                End If

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

                'Response.Redirect("RequestForInvoicing.aspx", False)
            End If
            If e.CommandName = "RowView" Then
                If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select post_state  from  reservation_invoice_header where invoiceno='" + lblINo.Text + "'") = "D" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "alert('This Invoice has been deleted.')", True)
                    Return
                ElseIf objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(invoiced,0) from  reservation_invoice_amendments(nolock) where invoiceno='" + lblINo.Text + "'") = "0" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "alert('This Invoice is amended,Plese edit Invoice and Save.')", True)
                    Return
                Else
                    Dim strpop As String = ""
                    'strpop = "window.open('RequestForInvoicing.aspx?State=View&appid=" + strappid + "&InvoiceNo=" + CType(lblINo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "','Reservation','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('RequestForInvoicing.aspx?State=View&appid=" + strappid + "&InvoiceNo=" + CType(lblINo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "','Reservation');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                    'Response.Redirect("RequestForInvoicing.aspx", False)
                End If
            End If
            If e.CommandName = "RowPrint" Then


                Dim grpexists As Integer = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select groups  from reservation_headernew where requestid ='" & lblReqId.Text & "'")
                If grpexists = 1 Then
                    'Dim sqlstring = "select * from reservation_group_FITconfirmation where requestid='" & lblReqId.Text & "'"
                    'Dim dt As DataSet = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), sqlstring)
                    'If dt.Tables(0).Rows.Count > 0 Then

                Else
                    grpexists = 0
                    ' End If
                End If



                If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select post_state  from  reservation_invoice_header where invoiceno='" + lblINo.Text + "'") = "D" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "alert('This Invoice has been deleted.')", True)
                    Return

                Else

                    Dim strpop As String = ""
                    'strpop = "window.open('InvoicePrint.aspx?State=Print&InvoiceNo=" + CType(lblINo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "','Reservation','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    'strpop = "window.open('rptConfirmaton.aspx?reqid=" + CType(lblReqId.Text.Trim, String) + "&typ=Invoice&Grp=" + grpexists.ToString + "','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,resizable=yes,status=yes');"
                    strpop = "window.open('rptConfirmaton.aspx?reqid=" + CType(lblReqId.Text.Trim, String) + "&typ=Invoice&Grp=" + grpexists.ToString + "','PopUpGuestDetails');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                End If
                'Response.Redirect("InvoicePrint.aspx", False)
            ElseIf e.CommandName = "RowPrintpl" Then
                If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select post_state  from  reservation_invoice_header where invoiceno='" + lblINo.Text + "'") = "D" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "alert('This Invoice has been deleted.')", True)
                    Return

                Else
                    Dim strpop As String = ""
                    'strpop = "window.open('rptConfirmaton.aspx?reqid=" + CType(lblReqId.Text.Trim, String) + "&typ=printpl','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"
                    strpop = "window.open('rptConfirmaton.aspx?reqid=" + CType(lblReqId.Text.Trim, String) + "&typ=printpl','PopUpGuestDetails');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                End If
            End If
                'RowEmail

            If e.CommandName = "RowEmail" Then
                If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select post_state  from  reservation_invoice_header where invoiceno='" + lblINo.Text + "' ") = "D" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "alert('This Invoice has been deleted.')", True)
                    Return

                Else
                    Dim strCustCode As String = gvResult.Rows(CType(e.CommandArgument.ToString, Integer)).Cells(4).Text.ToString

                    EmailInvoice(CType(lblReqId.Text.Trim, String), CType(lblINo.Text.Trim, String), strCustCode)

                End If

            End If



            If e.CommandName = "RowAuthorizeAmendpl" Then

                'Dim strCustCode As String = gvResult.Rows(CType(e.CommandArgument.ToString, Integer)).Cells(4).Text.ToString

                'EmailInvoice(CType(lblReqId.Text.Trim, String), CType(lblINo.Text.Trim, String), strCustCode)

                If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select post_state  from  reservation_invoice_header where invoiceno='" + lblINo.Text + "' and dbo.fn_chk_Activecancelled (requestid)=0 ") = "D" Then
                    'added fnchk - needs to chk 
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "alert('This Invoice has been deleted.')", True)
                    Return

                Else
                    Dim strpop As String = ""
                    'strpop = "window.open('RequestForInvoicing.aspx?State=AuthorizeAmend&appid=" + strappid + "&InvoiceNo=" + CType(lblINo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "','Reservation','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('RequestForInvoicing.aspx?State=AuthorizeAmend&appid=" + strappid + "&InvoiceNo=" + CType(lblINo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "','Reservation');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                End If

            End If

            If e.CommandName = "RowAuthorizecancelpl" Then

                'Dim strCustCode As String = gvResult.Rows(CType(e.CommandArgument.ToString, Integer)).Cells(4).Text.ToString

                'EmailInvoice(CType(lblReqId.Text.Trim, String), CType(lblINo.Text.Trim, String), strCustCode)

                If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select post_state  from  reservation_invoice_header where invoiceno='" + lblINo.Text + "' and dbo.fn_chk_Activecancelled (requestid)=0 ") = "D" Then
                    'added fnchk - needs to chk 
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "alert('This Invoice has been deleted.')", True)
                    Return

                Else
                    Dim strpop As String = ""
                    'strpop = "window.open('RequestForInvoicing.aspx?State=Authorizecancel&appid=" + strappid + "&InvoiceNo=" + CType(lblINo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "','Reservation','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('RequestForInvoicing.aspx?State=Authorizecancel&appid=" + strappid + "&InvoiceNo=" + CType(lblINo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "','Reservation');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                End If

            End If

                If e.CommandName = "RowviewAmendpl" Then
                If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select post_state  from  reservation_invoice_header where invoiceno='" + lblINo.Text + "'  ") = "D" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "alert('This Invoice has been deleted.')", True)
                    Return

                Else

                    Dim strpop As String = ""
                    ' strpop = "window.open('Reservation_Amend_Authorization Log.aspx?appid=" + strappid + "&InvoiceNo=" + CType(lblINo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "&customer=" + CType(customer.Trim, String) + "','Reservation','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    strpop = "window.open('Reservation_Amend_Authorization Log.aspx?appid=" + strappid + "&InvoiceNo=" + CType(lblINo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "&customer=" + CType(customer.Trim, String) + "','Reservation');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                End If
                End If

            If e.CommandName = "Rowprintjournal" Then
                If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select post_state  from  reservation_invoice_header where invoiceno='" + lblINo.Text + "'") = "D" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "alert('This Invoice has been deleted.')", True)
                    Return

                Else

                    Dim strpop As String = ""
                    'strpop = "window.open('InvoicePrint.aspx?State=Print&InvoiceNo=" + CType(lblINo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "','Reservation','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    'strpop = "window.open('rptInvoice.aspx?reqid=" + CType(lblReqId.Text.Trim, String) + "&Type=PrintJournal&invoiceno=" + CType(lblINo.Text.Trim, String) + "','PopUpPrintJournal','width=1010,height=650 left=0,top=0 scrollbars=yes,resizable=yes,status=yes');"
                    strpop = "window.open('rptInvoice.aspx?reqid=" + CType(lblReqId.Text.Trim, String) + "&Type=PrintJournal&invoiceno=" + CType(lblINo.Text.Trim, String) + "','PopUpPrintJournal');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReservationInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Private Sub EmailInvoice(ByVal strRequestId As String, ByVal strInvNo As String, ByVal strCustomerCode As String)

        Dim ds As DataSet
        Dim strEmail As String = ""
        Dim strContactPerson As String = ""

        If strCustomerCode <> "" Then
            ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select aemail ,acontact1 from agentmast where agentcode='" & strCustomerCode & "'")
            If ds IsNot Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("aemail")) = False Then
                        strEmail = ds.Tables(0).Rows(0)("aemail").ToString

                    End If
                    If IsDBNull(ds.Tables(0).Rows(0)("acontact1")) = False Then
                        strContactPerson = ds.Tables(0).Rows(0)("acontact1").ToString

                    End If

                End If
            End If
        End If
        ''**testing***

        '***************************
        If ((strEmail.Length = 0) Or (strContactPerson.Length = 0)) = False Then
            If SendMailInvoiceReport(strRequestId, strInvNo, strContactPerson, strEmail) = True Then

            End If
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Email Id or Contact Person Entered for corresponding Agent !!' );", True)

        End If
    End Sub

    Public Function SendMailInvoiceReport(ByVal requestid As String, ByVal strInvNo As String, ByVal strContactPerson As String, ByVal strToMail As String) As Boolean
        SendMailInvoiceReport = True
        Dim rnd As Random = New Random
        Dim strFilename As String = ""
        Dim strFullpath As String = ""
        Dim strMessage As String = ""
        Dim strSubject As String = ""
        Try

            strFilename = rnd.Next.ToString()
            strFullpath = Server.MapPath(".")
            strFullpath += "\\SavedReports\" + strFilename + ".pdf"

            Dim rep As New ReportDocument
            Dim pnames As ParameterFieldDefinitions
            Dim pname As ParameterFieldDefinition
            Dim param As New ParameterValues
            Dim paramvalue As New ParameterDiscreteValue
            Dim ConnInfo As New ConnectionInfo
            With ConnInfo
                '.ServerName = ConfigurationManager.AppSettings("dbServerName")
                '.DatabaseName = ConfigurationManager.AppSettings("dbDatabaseName")
                '.UserID = ConfigurationManager.AppSettings("dbUserName")
                '.Password = ConfigurationManager.AppSettings("dbPassword")
                .ServerName = Session("dbServerName")
                .DatabaseName = Session("dbDatabaseName")
                .UserID = Session("dbUserName")
                .Password = Session("dbPassword")
            End With


            Dim grpexists As Integer = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select groups  from reservation_headernew where requestid ='" & requestid & "'")
            If grpexists = 1 Then
                rep.Load(Server.MapPath("~\Report\rptsalesinvoice_groups.rpt"))
            Else

                rep.Load(Server.MapPath("~\Report\rptsalesinvoice_2.rpt"))
            End If

            Dim RepTbls As Tables = rep.Database.Tables

            For Each RepTbl As Table In RepTbls
                Dim RepTblLogonInfo As TableLogOnInfo = RepTbl.LogOnInfo
                RepTblLogonInfo.ConnectionInfo = ConnInfo
                RepTbl.ApplyLogOnInfo(RepTblLogonInfo)
            Next

            pnames = rep.DataDefinition.ParameterFields
            If grpexists = 1 Then
                pname = pnames.Item("@priceoption")
                paramvalue.Value = 0
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)


                pname = pnames.Item("imgpath")
                paramvalue.Value = ""
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

            End If

          
            pname = pnames.Item("@requestid")
            paramvalue.Value = requestid
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("@proforma")
            paramvalue.Value = 0
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("addrLine1")
            paramvalue.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1063)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)
            If grpexists <> 1 Then
                pname = pnames.Item("addrLine2")
                paramvalue.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1064)
                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)

            End If
           
            pname = pnames.Item("addrLine3")
            paramvalue.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1065)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            pname = pnames.Item("addrLine4")
            paramvalue.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1066)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)


            pname = pnames.Item("addrLine5")
            paramvalue.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1053)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)

            If grpexists <> 1 Then


                pname = pnames.Item("signatureexistflag")
                Dim picflag As Boolean = False
                Dim path As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select u.usersign from reservation_headernew h(nolock) left outer join  usermaster as  u(nolock) on h.usercode=u.usercode where  h.requestid='" & requestid & "'")
                If path <> "" Then
                    Dim t As String = System.AppDomain.CurrentDomain.BaseDirectory()

                    path = t & "useradminmodule\UploadImage\" & path
                    If File.Exists(path) Then
                        picflag = True
                    Else
                        picflag = False


                    End If
                End If

                'If picflag = False Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('User Signature File not found, Please save Signature  using UserAdmin.');", True)

                'End If

                paramvalue.Value = IIf(picflag, path, "0")

                param = pname.CurrentValues
                param.Add(paramvalue)
                pname.ApplyCurrentValues(param)
            End If



            pname = pnames.Item("cmb")
            paramvalue.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 1050)
            param = pname.CurrentValues
            param.Add(paramvalue)
            pname.ApplyCurrentValues(param)






            Response.Buffer = False
            Response.ClearContent()
            Response.ClearHeaders()
            rep.ExportToDisk(ExportFormatType.PortableDocFormat, strFullpath)
            rep.Dispose()

            strSubject = "Invoice.no:" + CType(strInvNo, String)
            strMessage = "Dear " + strContactPerson + "&nbsp;<br /><br />&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;"
            strMessage += "Please find the attached Invoice [Invoice No : " + CType(strInvNo, String) + "]<br />&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp"
            strMessage += "<br />Regards<br /><br />" + CType(Session("GlobalUserName"), String) + "<br />" + CType(Session("Desg"), String) + "<br />" + CType(Session("ComapnyName"), String)


            Dim strfrommail = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='563'")

            If objEmail.SendEmail(strfrommail, strToMail, strSubject, strMessage, strFullpath) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "2", "alert('Mail Sent Sucessfully to " + strToMail + "');", True)
                SaveToInvoiceMailLog(CType(strInvNo, String), "INO", strfrommail, strToMail)


            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "3", "alert('Failed to Send the mail to " + strToMail + "');", True)
                SendMailInvoiceReport = False

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReservationPrintPage.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            Dim thefile As FileInfo = New FileInfo(strFullpath)
            If thefile.Exists Then
                File.Delete(strFullpath)
            End If
        End Try

    End Function


    Private Sub SaveToInvoiceMailLog(ByVal prm_strInvNo As String, ByVal prm_strInvType As String, ByVal prm_strEmailSentBy As String,
                                 ByVal prm_strEmailSentID As String)
        Dim mySqlConn As SqlConnection
        Dim sqlTrans As SqlTransaction
        Dim mySqlCmd As SqlCommand
        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        Try
            sqlTrans = mySqlConn.BeginTransaction()

            mySqlCmd = New SqlCommand
            mySqlCmd.CommandText = "sp_add_invoice_email_log"
            mySqlCmd.Connection = mySqlConn
            mySqlCmd.Transaction = sqlTrans
            mySqlCmd.CommandType = CommandType.StoredProcedure
            Dim parms As New List(Of SqlParameter)
            Dim parm(4) As SqlParameter
            parm(0) = New SqlParameter("@invoiceno", CType(prm_strInvNo, String))
            parm(1) = New SqlParameter("@invoicetype", CType(prm_strInvType, Integer))
            parm(2) = New SqlParameter("@emailsentby", CType(prm_strEmailSentBy, Integer))
            parm(3) = New SqlParameter("@emailsentid", CType(prm_strEmailSentID, Integer))
            '@emailsentid            

            For p = 0 To 3
                mySqlCmd.Parameters.Add(parm(p))
            Next
            mySqlCmd.ExecuteNonQuery()

            sqlTrans.Commit()

        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" + ex.Message + "');", True)

        Finally
            sqlTrans.Dispose()
            mySqlConn.Close()
            mySqlConn.Dispose()

        End Try
    End Sub


    Public Function Validateseal(ByVal invno) As Boolean
        Try
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  reservation_invoice_header where invoiceno='" + invno + "' ")
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)("tran_state")) = False Then
                        If ds.Tables(0).Rows(0)("tran_state") = "S" Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Validateseal = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReservationInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

    Public Function ValidateRefund(ByVal reqid) As Boolean
        Try
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  refund_request_header where requestid='" + reqid + "'")
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return True
            End If
        Catch ex As Exception
            ValidateRefund = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReservationInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
     Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        If ValidatePage() = True Then
            Try

                Dim strfromdate, strtodate, strcustcode, strreqid, strinvoiceno, strtype, poststate As String

                strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")

                strtodate = Format(CType(txtToDate.Text, Date), "yyyy/MM/dd")
                If ddlCustomer.Items(ddlCustomer.SelectedIndex).Text <> "[Select]" Then
                    strcustcode = ddlCustomer.Value 'ddlCustomer.Items(ddlCustomer.SelectedIndex).Text
                Else
                    strcustcode = ""
                End If

                strreqid = txtRequestId.Value
                strinvoiceno = txtInvoiceNo.Value

                strtype = ddlrpttype.SelectedValue
                If ddlStatus.Value <> "[Select]" Then
                    poststate = ddlStatus.Value
                Else
                    poststate = ""
                End If
                Dim strpop As String = ""
                ' strpop = "window.open('reserv_invoice_Report.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&custcode=" & strcustcode & "&reqid=" & strreqid & "&invoiceno=" & strinvoiceno & "&type=" & strtype & " ','repInvoiceSearch','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('reserv_invoice_Report.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&custcode=" & strcustcode & "&reqid=" & strreqid & "&invoiceno=" & strinvoiceno & "&type=" & strtype & "&poststate=" & poststate & " ','repInvoiceSearch','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)





                'Response.Redirect("reserv_invoice_Report.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&custcode=" & strcustcode & "&reqid=" & strreqid & "&invoiceno=" & strinvoiceno & "&type=" & strtype, False)

                'End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ReservationInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub

    Protected Sub BtnReports_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnReports.Click

        If ValidatePage() = True Then
            Try
                Dim checkedrow As Integer = 0
                Dim reqIdList As String = String.Empty
                Dim count As Integer = 0
                For Each row As GridViewRow In gvResult.Rows

                    Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("chkSelect"), CheckBox)

                    If chkRow.Checked = True Then
                        Dim lblinvoiceno As String = TryCast(row.Cells(1).FindControl("lblInvoiceNo"), Label).Text
                        Dim lblrequestid As String = TryCast(row.Cells(4).FindControl("lblRequestId"), Label).Text
                      

                        If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select post_state  from  reservation_invoice_header where invoiceno='" + lblinvoiceno + "'") = "D" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "alert('This Invoice has been deleted.')", True)
                            Return
                        Else
                            Dim strpop As String = ""

                            ' strpop = "window.open('rptConfirmaton.aspx?reqid=" + CType(lblrequestid, String) + "&typ=Invoice&Grp=" + grpexists.ToString + "','PopUpGuestDetails'," & randomnumber & ",'width=1010,height=650 left=0,top=0 scrollbars=yes,resizable=yes,status=yes');"
                            'strpop = "window.open('rptConfirmaton.aspx?reqid=" + CType(lblrequestid, String) + "&typ=Invoice&Grp=" + grpexists.ToString + "','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,status=yes');"                            
                            checkedrow = checkedrow + 1
                            If count = 0 Then
                                reqIdList = lblrequestid
                                count = 1
                            Else
                                reqIdList &= "|" & lblrequestid
                            End If
                        End If
                    End If
                Next                
                ScriptManager.RegisterStartupScript(Page, GetType(Page), "popup", "<script type='text/javascript'>openwindow('" & reqIdList & "'," & checkedrow & ");</script>", False)
               
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ReservationInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub
    'Added BtnReports_Click by Archana on 10/5/2015 for multiple reports when clicking on report button

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ReservationInvoiceSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Try
            If txtFromDate.Text = "" Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtFromDate.ClientID + "');", True)
                'SetFocus(txtFromDate)
                ValidatePage = False
                Exit Function
            End If


            If txtToDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                'SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If

            If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                If CType(objDateTime.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objDateTime.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                    'SetFocus(txtToDate)
                    ValidatePage = False
                    Exit Function
                End If
            End If



            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReservationInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
    Protected Sub BtnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportToExcel.Click
        Try
            If ValidatePage() = True Then
                txtSearchExport.Value = 1
                Dim dsResult As New DataSet
                dsResult = doSearch()
                objUtils.ExportToExcel(dsResult, Response)
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound

        If (e.Row.RowType = DataControlRowType.Header) Then
            Dim basecurr As String
            basecurr = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 457)
            e.Row.Cells(9).Text = e.Row.Cells(9).Text + "[" + basecurr + "]"
        End If
    End Sub
End Class
