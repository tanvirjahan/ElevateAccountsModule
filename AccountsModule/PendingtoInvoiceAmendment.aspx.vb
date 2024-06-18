
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Partial Class PendingtoInvoiceAmendment
    Inherits System.Web.UI.Page


#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objdatetime As New clsDateTime
    Dim objEmail As New clsEmail
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction

#End Region

#Region "Enum parameter"
    Enum parameter
        requestid = 0
        agentref = 1
        usercode = 2
        fromrequestdate = 3
        torequestdate = 4
        fromcheckindate = 5
        tocheckindate = 6
        fromcheckoutdate = 7
        tocheckoutdate = 8
        agentcode = 9
        partycode = 10
        supagentcode = 11
        guestfname = 12
        guestlname = 13
        filter = 14
        subusercode = 15
        searchtype = 16
        orderby = 17
    End Enum
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                txtconnection.Value = Session("dbconnectionName")
                imgicon.Style("visibility") = "hidden"
                lblAPending.Visible = False
                lblAReady.Visible = False
                grdInvError.Visible = False
                lblCancelH.Visible = False

                FillAllDDL()
                txtfrmCheckin.Text = Now.Date
                txttoCheckin.Text = Now.Date

            Else
                'If chkinv.Text = "Yes" Then
                '    btngenerateinvoice_Click(sender, e)
                'End If




            End If
            btnDisplay.Attributes.Add("onclick", "return FormValidation()")
            btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Exit?')==false)return false;")
            btngenerateinvoice.Attributes.Add("onclick", "return invoicevalidate()")

        Catch ex As Exception

        End Try
    End Sub

    Private Sub FillAllDDL()
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
    End Sub


    Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Try
            Dim dsResult As New DataSet
            lblMessage.Visible = False
            grdInvError.Visible = False
            dsResult = doSearch()
            If dsResult.Tables.Count > 0 Then
                If dsResult.Tables(0).Rows.Count > 0 Then
                    lblAReady.Visible = True
                    divRes.Style("HEIGHT") = "300px"
                    grdAmendmentReady.DataSource = dsResult.Tables(0)
                    grdAmendmentReady.Visible = True

                    Btnselectall.Visible = True
                    btnunselect.Visible = True
                    btngenerateinvoice.Visible = True
                    grdAmendmentReady.DataBind()
                    lblMessage.Text = ""
                Else
                    lblAReady.Visible = False
                    divRes.Style("HEIGHT") = "0px"
                    grdAmendmentReady.Visible = False
                    Btnselectall.Visible = False
                    btnunselect.Visible = False
                    btngenerateinvoice.Visible = False

                    lblMessage.Visible = True
                    lblMessage.Text = "No Records Found"
                    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('First reconfirm the booking.');", True)
                End If

                If dsResult.Tables(1).Rows.Count > 0 Then
                    lblAPending.Visible = True
                    divRes1.Style("HEIGHT") = "300px"
                    grdAmendemntPending.DataSource = dsResult.Tables(1)
                    grdAmendemntPending.Visible = True
                    grdAmendemntPending.DataBind()

                Else
                    lblAPending.Visible = False
                    divRes1.Style("HEIGHT") = "0px"
                    grdAmendemntPending.Visible = False
                    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('First reconfirm the booking.');", True)
                End If

                If dsResult.Tables(2).Rows.Count > 0 Then
                    lblCancelH.Visible = True
                    pnlCanc.Style("HEIGHT") = "300px"
                    grdCancelInv.DataSource = dsResult.Tables(2)
                    grdCancelInv.Visible = True

                    btnSelectAllCancel.Visible = True
                    btnUnSelectAllCancel.Visible = True
                    btnRemoveInvoices.Visible = True
                    grdCancelInv.DataBind()
                    lblCancl.Visible = False
                Else
                    lblCancelH.Visible = False
                    pnlCanc.Style("HEIGHT") = "0px"
                    grdCancelInv.Visible = False
                    btnSelectAllCancel.Visible = False
                    btnUnSelectAllCancel.Visible = False
                    btnRemoveInvoices.Visible = False

                    lblCancl.Visible = False
                    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('First reconfirm the booking.');", True)
                End If

            Else
                divRes.Style("HEIGHT") = "0px"
                pnlCanc.Style("HEIGHT") = "0px"
                grdAmendmentReady.Visible = False
                grdAmendemntPending.Visible = False
                grdInvError.Visible = False
                Btnselectall.Visible = False
                btnunselect.Visible = False
                btngenerateinvoice.Visible = False
                btnSelectAllCancel.Visible = False
                btnUnSelectAllCancel.Visible = False
                btnRemoveInvoices.Visible = False
                lblMessage.Visible = True
                lblMessage.Text = "No Records Found"
                lblCancl.Visible = False
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('First reconfirm the booking.');", True)
            End If
            imgicon.Style("visibility") = "hidden"
        Catch ex As Exception
            objUtils.WritErrorLog("ReservationInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ReservationPIWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        'Response.Redirect("ReservationInvoiceSearch.aspx", False)
    End Sub

    Protected Sub lnkpreviewinvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim objbtn As LinkButton = CType(sender, LinkButton)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex
        Dim requestid As Label
        requestid = CType(grdAmendmentReady.Rows(row.RowIndex).FindControl("lblReqid"), Label)
        If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_chk_invoice('" & requestid.Text & "')") = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Not Ready for Invoicing, Some lines not confirmed or Reconfirmed!! ');", True)

        Else

            Dim grpexists As Integer = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select groups  from reservation_headernew where requestid ='" & requestid.Text & "'")
            If grpexists = 1 Then
                'Dim sqlstring = "select * from reservation_group_FITconfirmation where requestid='" & lblReqId.Text & "'"
                'Dim dt As DataSet = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), sqlstring)
                'If dt.Tables(0).Rows.Count > 0 Then

            Else
                grpexists = 0
                ' End If
            End If


            Dim strpop As String = ""
            'strpop = "window.open('InvoicePrint.aspx?State=Print&InvoiceNo=" + CType(lblINo.Text.Trim, String) + "&RequestId=" + CType(lblReqId.Text.Trim, String) + "','Reservation','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptConfirmaton.aspx?reqid=" + CType(requestid.Text, String) + "&typ=Invoice&Grp=" + grpexists.ToString + "','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,resizable=yes,status=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            'Dim strpop As String = ""
            'strpop = "window.open('rptConfirmaton.aspx?reqid=" + requestid.Text + "&typ=Invoice','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,resizable=yes,status=yes');"
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        End If


        'End If
    End Sub
    Public Function doSearch() As DataSet
        doSearch = Nothing
        Try
            Dim count As Integer = [Enum].GetValues(GetType(parameter)).Length


            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(count) As SqlParameter



            If Not (txtRequestId.Value = "") Then
                parm(parameter.requestid) = New SqlParameter("@requestid", CType(txtRequestId.Value.Trim, String))
            Else
                parm(parameter.requestid) = New SqlParameter("@requestid", String.Empty)
            End If
            If Not (txtCustRef.Value = "") Then
                parm(parameter.agentref) = New SqlParameter("@agentref", CType(txtCustRef.Value.Trim, String))
            Else
                parm(parameter.agentref) = New SqlParameter("@agentref", String.Empty)
            End If

            If Not (ddlUserName.Value = "[Select]") Then
                parm(parameter.usercode) = New SqlParameter("@usercode", CType(ddlUserName.Value.Trim, String))
            Else
                parm(parameter.usercode) = New SqlParameter("@usercode", String.Empty)
            End If
            If Not (txtfrmrequestdate.Text = "") Then
                parm(parameter.fromrequestdate) = New SqlParameter("@fromrequestdate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(txtfrmrequestdate.Text), String))
            Else
                parm(parameter.fromrequestdate) = New SqlParameter("@fromrequestdate", "1900/01/01")
            End If
            If Not (txttorequestdate.Text = "") Then
                parm(parameter.torequestdate) = New SqlParameter("@torequestdate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(txttorequestdate.Text), String))
            Else
                parm(parameter.torequestdate) = New SqlParameter("@torequestdate", "1900/01/01")
            End If
            If Not (txtfrmCheckin.Text = "") Then
                parm(parameter.fromcheckindate) = New SqlParameter("@fromcheckindate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(txtfrmCheckin.Text), String))
            Else
                parm(parameter.fromcheckindate) = New SqlParameter("@fromcheckindate", "1900/01/01")
            End If
            If Not (txttoCheckin.Text = "") Then
                parm(parameter.tocheckindate) = New SqlParameter("@tocheckindate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(txttoCheckin.Text), String))
            Else
                parm(parameter.tocheckindate) = New SqlParameter("@tocheckindate", "1900/01/01")
            End If

            If Not (txtfrmCheckout.Text = "") Then
                parm(parameter.fromcheckoutdate) = New SqlParameter("@fromcheckoutdate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(txtfrmCheckout.Text), String))
            Else
                parm(parameter.fromcheckoutdate) = New SqlParameter("@fromcheckoutdate", "1900/01/01")
            End If
            If Not (txttoCheckout.Text = "") Then
                parm(parameter.tocheckoutdate) = New SqlParameter("@tocheckoutdate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(txttoCheckout.Text), String))
            Else
                parm(parameter.tocheckoutdate) = New SqlParameter("@tocheckoutdate", "1900/01/01")
            End If

            If Not (ddlCustomerName.Value = "[Select]") Then
                parm(parameter.agentcode) = New SqlParameter("@agentcode ", CType(ddlCustomerName.Value, String))
            Else
                parm(parameter.agentcode) = New SqlParameter("@agentcode ", String.Empty)
            End If
            If CType(ddlSupplierName.Value, String) <> "[Select]" Then
                parm(parameter.partycode) = New SqlParameter("@partycode ", CType(ddlSupplierName.Value.Trim, String))

            Else
                parm(parameter.partycode) = New SqlParameter("@partycode ", String.Empty)
            End If

            If CType(ddlSupplerAgentName.Value, String) <> "[Select]" Then
                parm(parameter.supagentcode) = New SqlParameter("@supagentcode ", CType(ddlSupplerAgentName.Value.Trim, String))
            Else
                parm(parameter.supagentcode) = New SqlParameter("@supagentcode ", String.Empty)
            End If
            If Not (txtFirstName.Value = "") Then
                parm(parameter.guestfname) = New SqlParameter("@guestfname", CType(txtFirstName.Value.Trim, String))
            Else
                parm(parameter.guestfname) = New SqlParameter("@guestfname", String.Empty)
            End If
            If Not (txtLastName.Value = "") Then
                parm(parameter.guestlname) = New SqlParameter("@guestlname", CType(txtLastName.Value.Trim, String))
            Else
                parm(parameter.guestlname) = New SqlParameter("@guestlname", String.Empty)
            End If

            parm(parameter.filter) = New SqlParameter("@filter", 5)

            If Not CType(ddlSubUserName.Value, String) = "[Select]" Then
                parm(parameter.subusercode) = New SqlParameter("@subusercode ", CType(ddlSubUserName.Value.Trim, String))
            Else
                parm(parameter.subusercode) = New SqlParameter("@subusercode ", String.Empty)
            End If
            parm(parameter.searchtype) = New SqlParameter("@searchtype", 0)

            parm(parameter.orderby) = New SqlParameter("@orderby", 0)

            For i = 0 To count - 1
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            'ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"),"sp_get_request_search", parms)
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_request_search_forinvoice_amend", parms)
            Return ds
        Catch ex As Exception
            objUtils.WritErrorLog("PendingtoInvoiceAmendment.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

    Protected Sub grdAmendmentReady_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdAmendmentReady.RowCommand
        Try
            Dim lblReqID As Label
            If e.CommandName = "Invoice" Then
                lblReqID = grdAmendmentReady.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblReqid")

                'Session.Add("RequestId", CType(lblReqID.Text.Trim, String))

                'Change 12/11/2008 ******************************
                'Session.Add("State", "New")
                'Change 12/11/2008 ******************************
                'Response.Redirect("RequestForInvoicing.aspx", False)

                Dim strpop As String = ""
                strpop = "window.open('RequestForInvoicing.aspx?State=New&appid=" + ViewState("AppId") + "&RequestId=" + CType(lblReqID.Text.Trim, String) + "','Reservation','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ReservationInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btndummy_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        validategrid()
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ReservationInvoice','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Private Sub validategrid()

        Dim chksel As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        Dim custname As String = ""
        Dim i As Integer = 0
        Dim sender As Object = New Object()

        ' Dim chkflag As Boolean = False
        For Each gvRow1 In grdAmendmentReady.Rows
            chksel = gvRow1.FindControl("chkSel")
            Dim requestid As Label, invid As Label, lblStatus As Label
            requestid = CType(gvRow1.FindControl("lblReqid"), Label)
            invid = CType(gvRow1.FindControl("lblinvno"), Label)
            lblStatus = CType(gvRow1.FindControl("lblStatus"), Label)

            custname = grdAmendmentReady.DataKeys(i).Values("customer").ToString

            If chksel.Checked = True Then
                chksel.Checked = False
                If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_chk_invoice('" & requestid.Text & "')") = 0 Then
                    btngenerateinvoice.Style("visibility") = "visible"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "javascript:hiddenvalchng();", True)
                    Exit Sub
                Else
                    Dim ds As New DataSet
                    Dim sqlstr As String
                    sqlstr = "execute sp_validate_reservation_invoice '" & CType(requestid.Text, String) & "' "
                    ds = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), sqlstr)
                    If ds IsNot Nothing Then
                        If ds.Tables(0).Rows.Count > 0 Then
                            ''display in grid
                            grdInvError.Visible = True
                            grdInvError.DataSource = ds.Tables(0)
                            grdInvError.DataBind()

                            btngenerateinvoice.Style("visibility") = "visible"

                            Exit Sub
                        Else
                            If lblStatus.Text.Trim <> "Cancelled" Then
                                If save(requestid.Text, custname, invid.Text) = False Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Save Unsuccessful !! ');", True)
                                    Exit Sub
                                End If
                            Else
                                If saveCancellation(requestid.Text, custname, invid.Text) = False Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Save Unsuccessful !! ');", True)
                                    Exit Sub
                                End If
                            End If

                        End If
                    End If
                End If

            End If
            i = i + 1
        Next

        '  Dim sender As Object = New Object()
        btnDisplay_Click(sender, New System.EventArgs())
        'Dim strscript As String = ""
        'strscript = "window.opener.__doPostBack('ReservationPIWindowPostBack', '');window.opener.focus();window.close();"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

        ''Dim strpop As String = ""
        ''strpop = "window.open('ReservationInvoiceSearch.aspx','ReservationWindowPostBack','width=1000,height=620 left=10,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        ''ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


    End Sub

    Private Function save(ByVal requestid As String, ByVal custname As String, ByVal invoiceno As String) As Boolean
        Dim mySqlCmd As SqlCommand
        save = True
        'no errors, save with procedure
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

            mySqlCmd = New SqlCommand
            mySqlCmd.Connection = mySqlConn
            mySqlCmd.Transaction = sqlTrans

            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandText = "sp_autopost_reservation_invoice"
            Dim parms As New List(Of SqlParameter)
            Dim parm(3) As SqlParameter
            parm(0) = New SqlParameter("@requestid", CType(requestid, String))
            parm(1) = New SqlParameter("@userlogged", CType(Session("GlobalUserName"), String))
            parm(2) = New SqlParameter("@invoiceno", invoiceno)
            'parm(2).Direction = ParameterDirection.Output
            For i = 0 To 2
                mySqlCmd.Parameters.Add(parm(i))
            Next
            mySqlCmd.ExecuteNonQuery()

            'Dim strInvoiceNo As String = ""
            'strInvoiceNo = parm(2).Value.ToString()
            'temp  saving
            'mySqlCmd = New SqlCommand
            'mySqlCmd.Connection = mySqlConn
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandText = "sp_add_salesinvoice"
            Dim parms2 As New List(Of SqlParameter)
            Dim parm2(3) As SqlParameter

            parm2(0) = New SqlParameter("@requestid", CType(requestid, String))
            parm2(1) = New SqlParameter("@adduser", CType(Session("GlobalUserName"), String))
            parm2(2) = New SqlParameter("@invoiceno", CType(invoiceno, String))
            mySqlCmd.Parameters.Clear()
            For i = 0 To 2
                mySqlCmd.Parameters.Add(parm2(i))
            Next
            mySqlCmd.ExecuteNonQuery()
            'connection close


            sqlTrans.Commit()    'SQl Tarn Commit
            'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            'clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            ''clsDBConnect.dbCommandClose(mySqlCmd2)               'sql command disposed
            'clsDBConnect.dbConnectionClose(mySqlConn)

            btngenerateinvoice.Style("visibility") = "visible"

            If invoiceno <> "" Then
                'objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "")
                ' when saved , gets invoice no , send mail to accounts dept. 
                If SendMailInvoiceNo(invoiceno, custname) = True Then
                    'after send mail redirect to printpage.
                End If
                'Dim strpop As String = ""
                'strpop = "window.open('ReservationPrintPage.aspx','Reservation','width=1000,height=620 left=10,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            End If

        Catch ex As Exception
            save = False

            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & "');", True)
        Finally
            sqlTrans.Dispose()
            mySqlConn.Close()
            mySqlConn.Dispose()
        End Try




    End Function

    Public Function SendMailInvoiceNo(ByVal prm_strInvoiceNo As String, ByVal custname As String) As Boolean
        SendMailInvoiceNo = True

        Dim strMessage As String = ""
        Dim strSubject As String = ""
        Try

            strSubject = "Amended Invoice No:" + CType(prm_strInvoiceNo, String) + " Generated."

            strMessage = "Amended Invoice No:" + CType(prm_strInvoiceNo, String) + " generated for Agent Name : " & custname & " <br /><br /><br />"

            strMessage += "<br />&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp"
            strMessage += "<br />Regards<br /><br />" + CType(Session("GlobalUserName"), String)


            Dim strfrommail = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='563'")
            Dim strTomail = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='568'")

            If objEmail.SendEmail(strfrommail, strTomail, strSubject, strMessage) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "2", "alert('Mail Sent Sucessfully to " + strTomail + "');", True)
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "3", "alert('Failed to Send the mail to " + strTomail + "');", True)
                SendMailInvoiceNo = False
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ReservationSummary.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally

        End Try

    End Function

    Protected Sub btngenerateinvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btngenerateinvoice.Click

        Dim chksel As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        Dim chkflag As Boolean = False
        For Each gvRow1 In grdAmendmentReady.Rows
            chksel = gvRow1.FindControl("chkSel")
            If chksel.Checked = True Then
                chkflag = True
            End If
        Next

        If chkflag = False Then
            btngenerateinvoice.Style("visibility") = "visible"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please  Select atleast one Row!! ');", True)
            Exit Sub
        Else
            validategrid()
        End If
    End Sub

    Protected Sub Btnselectall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnselectall.Click
        Dim chksel As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        For Each gvRow1 In grdAmendmentReady.Rows
            chksel = gvRow1.FindControl("chkSel")
            chksel.Checked = True
        Next
    End Sub

    Protected Sub btnunselect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnunselect.Click
        Dim chksel As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        For Each gvRow1 In grdAmendmentReady.Rows
            chksel = gvRow1.FindControl("chkSel")
            chksel.Checked = False
        Next
    End Sub

    Protected Sub btnSelectAllCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectAllCancel.Click
        Dim chksel As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        For Each gvRow1 In grdCancelInv.Rows
            chksel = gvRow1.FindControl("chkSel")
            chksel.Checked = True
        Next
    End Sub

    Protected Sub btnUnSelectAllCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnSelectAllCancel.Click
        Dim chksel As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        For Each gvRow1 In grdCancelInv.Rows
            chksel = gvRow1.FindControl("chkSel")
            chksel.Checked = False
        Next
    End Sub

    'Private Sub validategridCancellation()

    '    Dim chksel As HtmlInputCheckBox
    '    Dim gvRow1 As GridViewRow
    '    Dim custname As String = ""
    '    Dim canceltype As String = ""
    '    Dim i As Integer = 0
    '    ' Dim chkflag As Boolean = False
    '    For Each gvRow1 In grdCancelInv.Rows
    '        chksel = gvRow1.FindControl("chkSel")
    '        Dim requestid As Label, invid As Label
    '        requestid = CType(gvRow1.FindControl("lblReqid"), Label)
    '        invid = CType(gvRow1.FindControl("lblinvno"), Label)

    '        custname = grdCancelInv.DataKeys(i).Values("customer").ToString
    '        canceltype = grdCancelInv.DataKeys(i).Values("CancelType").ToString
    '        If chksel.Checked = True Then
    '            chksel.Checked = False
    '            If canceltype = "Charged" Then
    '                Session("reqId") = requestid.Text
    '                Session("custnm") = custname
    '                Session("invno") = invid.Text
    '                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "javascript:chargedcanceltype();", True)
    '                Exit Sub
    '            Else

    '                If saveCancellation(requestid.Text, custname, invid.Text) = False Then
    '                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Save Unsuccessful !! ');", True)
    '                    Exit Sub
    '                End If


    '            End If

    '        End If
    '        i = i + 1
    '    Next

    '    Dim sender As Object = New Object()
    '    btnDisplay_Click(sender, New System.EventArgs())
    '    'Dim strscript As String = ""
    '    'strscript = "window.opener.__doPostBack('ReservationPIWindowPostBack', '');window.opener.focus();window.close();"
    '    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

    '    ''Dim strpop As String = ""
    '    ''strpop = "window.open('ReservationInvoiceSearch.aspx','ReservationWindowPostBack','width=1000,height=620 left=10,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
    '    ''ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


    'End Sub

    Private Function validategridCancellation()
        validategridCancellation = True
        Dim chksel As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        Dim custname As String = ""
        Dim canceltype As String = ""
        Dim i As Integer = 0
        Dim chkflag As Boolean = False
        ' Dim chkflag As Boolean = False
        For Each gvRow1 In grdCancelInv.Rows
            chksel = gvRow1.FindControl("chkSel")
            Dim requestid As Label, invid As Label
            requestid = CType(gvRow1.FindControl("lblReqid"), Label)
            invid = CType(gvRow1.FindControl("lblinvno"), Label)

            custname = grdCancelInv.DataKeys(i).Values("customer").ToString
            canceltype = grdCancelInv.DataKeys(i).Values("CancelType").ToString
            If chksel.Checked = True Then
                chkflag = True
                'chksel.Checked = False
                If canceltype = "Charged" Then
                    'Session("reqId") = requestid.Text
                    'Session("custnm") = custname
                    'Session("invno") = invid.Text
                    validategridCancellation = False

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "javascript:chargedcanceltype();", True)
                    Exit Function
                Else


                End If

            End If


            i = i + 1
        Next
        If chkflag = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please  Select atleast one Row!! ');", True)
            Exit Function
            validategridCancellation = False
        End If




    End Function

    Private Function SaveCancel() As Boolean
        SaveCancel = True
        Dim chksel As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        Dim custname As String = ""
        Dim canceltype As String = ""
        Dim i As Integer = 0
        ' Dim chkflag As Boolean = False
        For Each gvRow1 In grdCancelInv.Rows
            chksel = gvRow1.FindControl("chkSel")
            Dim requestid As Label, invid As Label

            requestid = CType(gvRow1.FindControl("lblReqid"), Label)
            invid = CType(gvRow1.FindControl("lblinvno"), Label)
            custname = grdCancelInv.DataKeys(i).Values("customer").ToString
            canceltype = grdCancelInv.DataKeys(i).Values("CancelType").ToString

            If chksel.Checked = True Then
                chksel.Checked = False
                If saveCancellation(requestid.Text, custname, invid.Text) = False Then
                    SaveCancel = False
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Save Unsuccessful !! ');", True)
                    Exit Function
                End If

            End If
            i = i + 1
        Next

        Dim sender As Object = New Object()
        btnDisplay_Click(sender, New System.EventArgs())

    End Function



    Private Function saveCancellation(ByVal requestid As String, ByVal custname As String, ByVal invoiceno As String) As Boolean
        Dim mySqlCmd As SqlCommand
        saveCancellation = True
        'no errors, save with procedure
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

            mySqlCmd = New SqlCommand
            mySqlCmd.Connection = mySqlConn
            mySqlCmd.Transaction = sqlTrans

            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandText = "sp_reservation_invoice_cancellation"
            Dim parms As New List(Of SqlParameter)
            Dim parm(3) As SqlParameter
            parm(0) = New SqlParameter("@requestid", CType(requestid, String))
            parm(1) = New SqlParameter("@userlogged", CType(Session("GlobalUserName"), String))
            parm(2) = New SqlParameter("@invoiceno", invoiceno)
            'parm(2).Direction = ParameterDirection.Output
            For i = 0 To 2
                mySqlCmd.Parameters.Add(parm(i))
            Next
            mySqlCmd.ExecuteNonQuery()



            sqlTrans.Commit()    'SQl Tarn Commit

            'If invoiceno <> "" Then
            '    'objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "")
            '    ' when saved , gets invoice no , send mail to accounts dept. 
            '    If SendMailInvoiceNo(invoiceno, custname) = True Then
            '        'after send mail redirect to printpage.
            '    End If
            'End If

        Catch ex As Exception
            saveCancellation = False

            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & "');", True)
        Finally
            sqlTrans.Dispose()
            mySqlConn.Close()
            mySqlConn.Dispose()
        End Try




    End Function

    Protected Sub btnRemoveInvoices_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveInvoices.Click

        If validategridCancellation() = False Then

            Exit Sub
        Else
            SaveCancel()
        End If
    End Sub

    Protected Sub btndmyRemvInv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndmyRemvInv.Click

        SaveCancel()
        ''If Session("reqId").ToString <> "" And Session("invno") <> "" Then

        'If saveCancellation(Session("reqId").ToString, Session("custnm").ToString, Session("invno").ToString) = False Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Save Unsuccessful !! ');", True)
        '    Exit Sub
        'Else
        '    Session("reqId") = ""
        '    Session("custnm") = ""
        '    Session("invno") = ""
        '    validategridCancellation()
        'End If


        ''End If


    End Sub
End Class
