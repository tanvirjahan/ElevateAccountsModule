Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

Partial Class FreeformInvoiceSearch
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
        Invoiceno = 0
        status = 1
        invoicedate = 2
        requestid = 3
        customercode = 4
        customername = 5
        customerref = 6
        currncy = 7
        salamount = 8
        kwdamount = 9
        datecreated = 10
        usercreated = 11
        datemodified = 12
        usermodified = 13
        edit = 14
        view = 15
        print = 16
        delete = 17
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
        strpop = "window.open('Freeform_Invoice.aspx?State=New&appid=" + strappid + "','Accounts','width=1000,height=620 left=10,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        'Response.Redirect("FreeformInvoice.aspx", False)
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
                    strappname = AppName.Value
                End If
                SetFocus(txtInvoiceNo)
                If CType(Session("GlobalUserName"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType(strappname, String), "AccountsModule\FreeformInvoiceSearch.aspx", btnAddNew, BtnExportToExcel, _
                     btnPrint, gvResult, GridCol.edit, GridCol.delete, GridCol.view)

                End If
                'gvResult.Columns(17).Visible = False

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

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlCustomer.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

                txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("FreeformInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ReservationFreeWindowPostBack") Then
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
                    parm(parameter.fromdate) = New SqlParameter("@fromdate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(frmdate), String))

                    ' parm(parameter.fromdate) = New SqlParameter("@fromdate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(txtFromDate.Text), String))
                End If
            Else
                parm(parameter.fromdate) = New SqlParameter("@fromdate", "1900/01/01")
            End If

            If Not (Session("changeyear") Is Nothing) Then

                If Not (txtToDate.Text = "") Then

                    todate = CDate(Session("changeyear") + "/" + Month(CType(txtToDate.Text, Date)).ToString + "/" + Day(CType(txtToDate.Text, Date)).ToString)
                    parm(parameter.todate) = New SqlParameter("@todate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(todate), String))

                    ' parm(parameter.todate) = New SqlParameter("@todate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(txtToDate.Text), String))
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
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_freeform_invoice_search", parms)
            Return ds
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FreeformInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("FreeformInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

        'Record list will be according to the Changing the year  
        If Session("changeyear") <> Year(CType(txtFromDate.Text, Date)).ToString Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
            Exit Sub
        End If

        If Session("changeyear") <> Year(CType(txtToDate.Text, Date)).ToString Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
            Exit Sub
        End If

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
            If e.CommandName = "RowEdit" Then
                Dim strpop As String = ""

                If Validateseal(lblINo.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)

                    Return
                End If

                If ValidateRefund(lblReqId.Text) Then
                    strpop = "window.open('FreeForm_Invoice.aspx?State=Edit&appid=" + strappid + "&InvoiceNo=" + CType(lblINo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "','Reservation','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                Else
                    strpop = "alert('You can not edit the Invoice, There is a refund request for this request.')"
                End If

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If
            If e.CommandName = "RowView" Then
                If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(amended,0) from  reservation_invoice_header where invoiceno='" + lblINo.Text + "'") = "1" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "alert('This Invoice is amended,Plese edit Invoice and Save.')", True)
                    Return
                Else
                    Dim strpop As String = ""
                    strpop = "window.open('FreeForm_Invoice.aspx?State=View&appid=" + strappid + "&InvoiceNo=" + CType(lblINo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "','Accounts','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                    'Response.Redirect("RequestForInvoicing.aspx", False)
                End If
            End If
            If e.CommandName = "RowPrint" Then
                Dim strpop As String = ""
                strpop = "window.open('rptFreeInvoice.aspx?State=Print&InvoiceNo=" + CType(lblINo.Text.Trim, String) + "','Accounts','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

                'Response.Redirect("InvoicePrint.aspx", False)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FreeformInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("FreeformInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            objUtils.WritErrorLog("FreeformInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                strpop = "window.open('Free_invoice_Report.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&custcode=" & strcustcode & "&reqid=" & strreqid & "&invoiceno=" & strinvoiceno & "&type=" & strtype & "&poststate=" & poststate & " ','repInvoiceSearch','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)





                'Response.Redirect("reserv_invoice_Report.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&custcode=" & strcustcode & "&reqid=" & strreqid & "&invoiceno=" & strinvoiceno & "&type=" & strtype, False)

                'End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("FreeformInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=FreeformInvoiceSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
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
            objUtils.WritErrorLog("FreeformInvoiceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
