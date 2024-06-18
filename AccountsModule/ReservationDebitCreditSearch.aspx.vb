'------------================--------------=======================------------------================
'   Module Name    :    ReservationDebitCreditSearch.aspx
'   Developer Name :    Jaffer
'   Date           :    02 Feb 2009
'   
'
'------------================--------------=======================------------------================
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class AccountsModule_ReservationDebitCreditSearch
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
        BankMasterTypeTCol = 0
        BankMasterType = 1
        Description = 2
        CashBank = 3
        DateCreated = 4
        UserCreated = 5
        DateModified = 6
        UserModified = 7
        Edit = 8
        View = 9
        Delete = 10
    End Enum
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                txtconnection.Value = Session("dbconnectionName")

                SetFocus(txtCreditNoteNo)
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
                If CType(Session("GlobalUserName"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType(strappname, String), "AccountsModule\ReservationDebitCreditSearch.aspx", btnAddNew, BtnExportToExcel, _
                                                       btnPrint, gvResult, GridCol.Edit, GridCol.Delete, GridCol.View)
                End If


                'If txtFromDate.Text = "" Then
                '    txtFromDate.Text = Format(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select fdate from toursmaster"), Date), "dd/MM/yyy")
                'End If
                'If txtToDate.Text = "" Then
                '    txtToDate.Text = DateAdd(DateInterval.Month, 1, objdatetime.GetSystemDateOnly(Session("dbconnectionName")))
                'End If

                'Record list will be according to the Changing the year  
                If Not (Session("changeyear") Is Nothing) Then
                    frmdate = CDate(Session("changeyear") + "/01" + "/01")

                    If Session("changeyear") = Year(Now).ToString Then
                        todate = CDate(Session("changeyear") + "/" + Month(Now).ToString + "/" + Day(Now).ToString)
                    Else
                        todate = CDate(Session("changeyear") + "/" + "12" + "/" + "31")
                    End If

                    txtFromDate.Text = Format(CType(frmdate, Date), "dd/MM/yyy")
                    txtToDate.Text = Format(CType(todate, Date), "dd/MM/yyy")

                Else
                    txtFromDate.Text = ""
                    txtToDate.Text = ""
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
                objUtils.WritErrorLog("ReservationDebitCreditSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnClear.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to clear search criteria?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ReservationWindowPostBack") Then
            FillGrid()
        End If
    End Sub
    Public Function doSearch() As DataSet
        doSearch = Nothing
        Try
            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(12) As SqlParameter
            If Not (txtCreditNoteNo.Value = "") Then
                parm(0) = New SqlParameter("@creditnoteno", CType(txtCreditNoteNo.Value.Trim, String))
            Else
                parm(0) = New SqlParameter("@creditnoteno", String.Empty)
            End If
            If Not (txtRequestId.Value = "") Then
                parm(1) = New SqlParameter("@requestid", CType(txtRequestId.Value.Trim, String))
            Else
                parm(1) = New SqlParameter("@requestid", String.Empty)
            End If
            If Not (txtInvoiceNo.Value = "") Then
                parm(2) = New SqlParameter("@invoiceno", CType(txtInvoiceNo.Value.Trim, String))
            Else
                parm(2) = New SqlParameter("@invoiceno", String.Empty)
            End If
            If Not (ddlCustomer.Value = "[Select]") Then
                parm(3) = New SqlParameter("@agentcode", CType(ddlCustomer.Value.Trim, String))
            Else
                parm(3) = New SqlParameter("@agentcode", String.Empty)
            End If
            If Not (txtCustRef.Value = "") Then
                parm(4) = New SqlParameter("@agentref", CType(txtCustRef.Value.Trim, String))
            Else
                parm(4) = New SqlParameter("@agentref", String.Empty)
            End If
            If Not (txtFromDate.Text = "") Then
                parm(5) = New SqlParameter("@fromdate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(txtFromDate.Text), String))
            Else
                parm(5) = New SqlParameter("@fromdate", "1900/01/01")
            End If
            If Not (txtToDate.Text = "") Then
                parm(6) = New SqlParameter("@todate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(txtToDate.Text), String))
            Else
                parm(6) = New SqlParameter("@todate", "1900/01/01")
            End If
            If Not (txtFromAmount.Value = "") Then
                parm(7) = New SqlParameter("@fromamount ", CType(txtFromAmount.Value, String))
            Else
                parm(7) = New SqlParameter("@fromamount ", String.Empty)
            End If
            If Not (txtToAmount.Value = "") Then
                parm(8) = New SqlParameter("@toamount ", CType(txtToAmount.Value, String))
            Else
                parm(8) = New SqlParameter("@toamount ", String.Empty)
            End If
            If Not (ddlStatus.Value = "[Select]") Then
                parm(9) = New SqlParameter("@post_state", CType(ddlStatus.Value.Trim, String))
            Else
                parm(9) = New SqlParameter("@post_state", String.Empty)
            End If
            parm(10) = New SqlParameter("@orderby ", CType(ddlOrderBy.Value.Trim, Integer))
            If Not (txtSearchExport.Value = "") Then
                parm(11) = New SqlParameter("@searchExport", CType(txtSearchExport.Value.Trim, String))
            Else
                parm(11) = New SqlParameter("@searchExport", 0)
            End If

            For i = 0 To 11
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_creditnote_search", parms)
            Return ds

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReservationDebitCreditSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                    BtnExportToExcel.Visible = True
                Else
                    gvResult.Visible = False
                    lblMsg.Visible = True
                    lblMsg.Text = "No Records Found"
                    BtnExportToExcel.Visible = False
                End If
            Else
                gvResult.Visible = False
                lblMsg.Visible = True
                lblMsg.Text = "No Records Found"
                BtnExportToExcel.Visible = False
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReservationDebitCreditSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub gvSearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvResult.PageIndexChanging
        gvResult.PageIndex = e.NewPageIndex
        FillGrid()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'Record list will be according to the Changing the year  
        If Session("changeyear") <> Year(CType(txtFromDate.Text, Date)).ToString Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
            Exit Sub
        End If

        If Session("changeyear") <> Year(CType(txtToDate.Text, Date)).ToString Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot list for the Different year Change year '  );", True)
            Exit Sub
        End If



        txtSearchExport.Value = 0
        FillGrid()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtCreditNoteNo.Value = ""
        txtRequestId.Value = ""
        txtInvoiceNo.Value = ""
        ddlCustomer.Value = "[Select]"
        ddlStatus.Value = "[Select]"
        txtCustRef.Value = ""
        'txtFromDate.Text = ""
        ' txtToDate.Text = ""
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

            Dim lblCreditNoteNo As Label
            Dim lblReqId As Label
            lblCreditNoteNo = gvResult.Rows(CType(e.CommandArgument.ToString, Integer)).FindControl("lblCreditNoteNo")
            lblReqId = gvResult.Rows(CType(e.CommandArgument.ToString, Integer)).FindControl("lblRequestId")
            If e.CommandName = "RowEdit" Then

                If Validateseal(lblCreditNoteNo.Text) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed cannot Edit/Delete...')", True)
                    Return
                End If
                Dim strpop As String = ""
                strpop = "window.open('RefundCreditNote.aspx?State=Edit&CreditNoteNo=" + CType(lblCreditNoteNo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "','Reservation','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

                'Response.Redirect("RefundCreditNote.aspx", False)
            End If
            If e.CommandName = "RowView" Then
                Dim strpop As String = ""
                strpop = "window.open('RefundCreditNote.aspx?State=View&CreditNoteNo=" + CType(lblCreditNoteNo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "','Reservation','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                'Response.Redirect("RefundCreditNote.aspx", False)
            End If
            If e.CommandName = "RowPrint" Then
                Dim strpop As String = ""
                strpop = "window.open('CreditNotePrint.aspx?State=Print&CreditNoteNo=" + CType(lblCreditNoteNo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "','Reservation','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

                'Response.Redirect("InvoicePrint.aspx", False)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReservationDebitCreditSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If ValidatePage() = True Then
            Try
                Dim strfromdate, strtodate, strcustcode, strreqid, strinvoiceno, strtype, poststate, strcreditnoteno As String
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
                strcreditnoteno = txtCreditNoteNo.Value
                If ddlStatus.Value <> "[Select]" Then
                    poststate = ddlStatus.Value
                Else
                    poststate = ""
                End If
                Dim strpop As String = ""
                ' strpop = "window.open('reserv_invoice_Report.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&custcode=" & strcustcode & "&reqid=" & strreqid & "&invoiceno=" & strinvoiceno & "&type=" & strtype & " ','repInvoiceSearch','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('CreditNote_Report.aspx?creditnoteno=" & strcreditnoteno & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&custcode=" & strcustcode & "&reqid=" & strreqid & "&invoiceno=" & strinvoiceno & "&type=" & strtype & "&poststate=" & poststate & " ','repInvoiceSearch','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
                'Response.Redirect("reserv_invoice_Report.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&custcode=" & strcustcode & "&reqid=" & strreqid & "&invoiceno=" & strinvoiceno & "&type=" & strtype, False)
                'End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ReservationDebitCreditSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ReservationInvoiceSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Public Function ValidatePage() As Boolean
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
            objUtils.WritErrorLog("ReservationDebitCreditSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
    Public Function Validateseal(ByVal tranid) As Boolean
        Try
            Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select * from  reservation_creditnote_header where creditnoteno='" + tranid + "' ")
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

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Dim strpop As String = ""
        strpop = "window.open('RefundCreditNote.aspx?State=New','Accounts','width=1000,height=620 left=10,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        'Response.Redirect("RefundCreditNote.aspx", False)
    End Sub
End Class
