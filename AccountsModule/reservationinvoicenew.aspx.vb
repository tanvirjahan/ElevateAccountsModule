
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region
Partial Class reservationinvoicenew
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
        bookingtype = 18
    End Enum
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ViewState.Add("ResState", Request.QueryString("State"))
            ViewState.Add("ResInvoiceNo", Request.QueryString("InvoiceNo"))
            ViewState.Add("ResRequestId", Request.QueryString("RequestId"))
            ViewState.Add("AppId", Request.QueryString("appid"))


            If Page.IsPostBack = False Then
                txtconnection.Value = Session("dbconnectionName")
                imgicon.Style("visibility") = "hidden"


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

            parm(parameter.bookingtype) = New SqlParameter("@BookingType", CType(ddlBookingType.Value.Trim, String))

            For i = 0 To count - 1
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            'ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"),"sp_get_request_search", parms)
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_request_search_forinvoice_new", parms)
            ViewState("DataSetSource") = ds
            Return ds
        Catch ex As Exception
            objUtils.WritErrorLog("ReservationInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

    Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Try            
            ViewState("DataSetSource") = Nothing
            Call BindData()            
        Catch ex As Exception
            objUtils.WritErrorLog("ReservationInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub BindData()

        Dim dsResult As New DataSet
        lblMessage.Visible = False
        If ViewState("DataSetSource") Is Nothing Then
            dsResult = doSearch()
        Else
            dsResult = ViewState("DataSetSource")
        End If

        If dsResult.Tables.Count > 0 Then
            If dsResult.Tables(0).Rows.Count > 0 Then
                divRes.Style("HEIGHT") = "500px"
                gvSearchResult.DataSource = dsResult.Tables(0)
                gvSearchResult.Visible = True
                Btnselectall.Visible = True
                btnunselect.Visible = True
                btngenerateinvoice.Visible = True
                gvSearchResult.DataBind()
                lblMessage.Text = ""
            Else
                divRes.Style("HEIGHT") = "0px"
                gvSearchResult.Visible = False
                Btnselectall.Visible = False
                btnunselect.Visible = False
                btngenerateinvoice.Visible = False

                lblMessage.Visible = True
                lblMessage.Text = "No Records Found"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('First reconfirm the booking.');", True)
            End If
        Else
            divRes.Style("HEIGHT") = "0px"
            gvSearchResult.Visible = False
            Btnselectall.Visible = False
            btnunselect.Visible = False
            btngenerateinvoice.Visible = False
            lblMessage.Visible = True
            lblMessage.Text = "No Records Found"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('First reconfirm the booking.');", True)
        End If
        imgicon.Style("visibility") = "hidden"


    End Sub


    Protected Sub gvSearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSearchResult.PageIndexChanging
        gvSearchResult.PageIndex = e.NewPageIndex
        Call BindData()
    End Sub

    'Private Function CheckReconfirmation() As Boolean
    '    Dim strQry As String
    '    Dim strResult As String
    '    strQry = "select distinct d.requestid,d.reconfirmed  from reservation_headernew h,reservation_detailnew d where d.requestid='" & txtRequestId.Value.Trim & "' and h.agentcode='" & ddlCustomerCode.Items(ddlCustomerCode.SelectedIndex).Text & "'"
    '    strResult = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"),strQry)
    '    If IsDBNull(strResult) = False Then
    '        If strResult = "0" Then
    '            CheckReconfirmation = True
    '        Else
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('First reconfirm the booking.');", True)
    '            CheckReconfirmation = False
    '        End If
    '    Else
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('First reconfirm the booking.');", True)
    '        CheckReconfirmation = False
    '    End If

    '    CheckReconfirmation = True
    'End Function

    Protected Sub gvSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSearchResult.RowCommand
        Try
            Dim lblReqID As Label
            If e.CommandName = "Invoice" Then
                lblReqID = gvSearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("lblReqid")

                'Session.Add("RequestId", CType(lblReqID.Text.Trim, String))

                'Change 12/11/2008 ******************************
                'Session.Add("State", "New")
                'Change 12/11/2008 ******************************
                'Response.Redirect("RequestForInvoicing.aspx", False)

                Dim grpexists As Integer = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select groups  from reservation_headernew where requestid ='" & lblReqID.Text & "'")
                If grpexists = 1 Then
                    'Dim sqlstring = "select * from reservation_group_FITconfirmation where requestid='" & lblReqID.Text & "'"
                    'Dim dt As DataSet = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), sqlstring)
                    'If dt.Tables(0).Rows.Count > 0 Then

                Else
                    grpexists = 0
                    ' End If
                End If





                Dim strpop As String = ""
                strpop = "window.open('RequestForInvoicing.aspx?State=New&appid=" + ViewState("AppId") + "&RequestId=" + CType(lblReqID.Text.Trim, String) + "&Grp=" + grpexists.ToString + "','Reservation','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

            End If
        Catch ex As Exception
            objUtils.WritErrorLog("ReservationInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ReservationWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        'Response.Redirect("ReservationInvoiceSearch.aspx", False)
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ReservationInvoice','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub lnkpreviewinvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim objbtn As LinkButton = CType(sender, LinkButton)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex
        Dim requestid As Label
        requestid = CType(gvSearchResult.Rows(row.RowIndex).FindControl("lblReqid"), Label)
        If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_chk_invoice('" & requestid.Text & "')") = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Not Ready for Invoicing, Some lines not confirmed or Reconfirmed!! ');", True)

        Else

            Dim grpexists As Integer = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select groups  from reservation_headernew where requestid ='" & requestid.Text & "'")
            If grpexists = 1 Then
                'Dim sqlstring = "select * from reservation_group_FITconfirmation where requestid='" & requestid.Text & "'"
                'Dim dt As DataSet = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), sqlstring)
                'If dt.Tables(0).Rows.Count > 0 Then

            Else
                grpexists = 0
                'End If
            End If





            Dim strpop As String = ""

            strpop = "window.open('rptConfirmaton.aspx?reqid=" + requestid.Text + "&typ=Proforma&Grp=" + grpexists.ToString + "','PopUpGuestDetails','width=1010,height=650 left=0,top=0 scrollbars=yes,resizable=yes,status=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        End If


        'End If
    End Sub

    Protected Sub Btnselectall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnselectall.Click
        Dim chksel As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        For Each gvRow1 In gvSearchResult.Rows
            chksel = gvRow1.FindControl("chkSel")
            chksel.Checked = True
        Next
    End Sub

    Protected Sub btnunselect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnunselect.Click
        Dim chksel As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        For Each gvRow1 In gvSearchResult.Rows
            chksel = gvRow1.FindControl("chkSel")
            chksel.Checked = False
        Next
    End Sub


    Protected Sub gvSearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSearchResult.RowDataBound

        'If (e.Row.RowType = DataControlRowType.DataRow) Then
        '    Dim previnvoice As LinkButton
        '    Dim requestid As Label
        '    requestid = CType(e.Row.FindControl("lblReqid"), Label)
        '    previnvoice = CType(e.Row.FindControl("lnkpreviewinvoice"), LinkButton)
        '    previnvoice.Attributes.Add("OnClick", "return chkInvoicefn('" & requestid.Text & "');")


        'End If
    End Sub

    Protected Sub btngenerateinvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btngenerateinvoice.Click
        'chkinv.Text = "No"

        'If CType(hdnchkinvoice.Text, Integer) < 3 Then
        '    chkinv.Text = "Yes"

        '    hdnchkinvoice.Text = CType(hdnchkinvoice.Text, Integer) + 1


        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "<script language =""javascript"" type=""text/javascript"" > alert('hi');document.getElementById('" + btngenerateinvoice.ClientID + "').Click();document.forms[0].submit();</script>", False)
        '    'document.getElementById('" + hdnchkinvoice.ClientID + "').Onchange();
        '    ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", " javascript:hiddenvalchng();", False)


        '    'btngenerateinvoice_Click(sender, e)

        '    ' test(sender, e)

        'End If

        '   btngenerateinvoice.Visible = False

        Dim chksel As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        Dim chkflag As Boolean = False
        For Each gvRow1 In gvSearchResult.Rows
            chksel = gvRow1.FindControl("chkSel")
            If chksel.Checked = True Then
                chkflag = True
            End If
        Next

        If chkflag = False Then
            '     btngenerateinvoice.Visible = True
            btngenerateinvoice.Style("visibility") = "visible"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please  Select atleast one Row!! ');", True)
            Exit Sub
        Else
            validategrid()
            btnDisplay_Click(sender, e)
        End If

    End Sub


    Private Sub validategrid()

        Dim chksel As HtmlInputCheckBox
        Dim gvRow1 As GridViewRow
        Dim custname As String = ""
        Dim i As Integer = 0
        Dim ds As New DataSet
        Dim sqlstr As String

        ' Dim chkflag As Boolean = False
        For Each gvRow1 In gvSearchResult.Rows
            chksel = gvRow1.FindControl("chkSel")
            Dim requestid As Label
            requestid = CType(gvRow1.FindControl("lblReqid"), Label)

            custname = gvSearchResult.DataKeys(i).Values("customer").ToString

            If chksel.Checked = True Then
                chksel.Checked = False
                If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_chk_invoice('" & requestid.Text & "')") = 0 Then
                    btngenerateinvoice.Style("visibility") = "visible"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "javascript:hiddenvalchng();", True)
                    Exit Sub
                Else

                    If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_chk_booking('" & requestid.Text & "')") = "H" Then




                        sqlstr = "execute sp_validate_reservation_invoice '" & CType(requestid.Text, String) & "' "
                        ds = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), sqlstr)
                        If ds IsNot Nothing Then
                            If ds.Tables(0).Rows.Count > 0 Then
                                'display in grid
                                grdInvError.Visible = True
                                grdInvError.DataSource = ds.Tables(0)
                                grdInvError.DataBind()

                                btngenerateinvoice.Style("visibility") = "visible"

                                ' Exit Sub
                                GoTo v
                            Else
                                If save(requestid.Text, custname) = False Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Save Unsuccessful !! ');", True)
                                    Exit Sub
                                End If
                            End If
                        End If
                    ElseIf objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_chk_booking('" & requestid.Text & "')") = "T" Then
                        '''''Transfer only

                        sqlstr = "execute sp_validate_reservation_invoice_transfers '" & CType(requestid.Text, String) & "' "
                        ds = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), sqlstr)
                        If ds IsNot Nothing Then
                            If ds.Tables(0).Rows.Count > 0 Then
                                'display in grid
                                grdInvError.Visible = True
                                grdInvError.DataSource = ds.Tables(0)
                                grdInvError.DataBind()

                                btngenerateinvoice.Style("visibility") = "visible"

                                'Exit Sub
                                GoTo v
                            Else
                                If saveTransfer(requestid.Text, custname) = False Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Save Unsuccessful !! ');", True)
                                    Exit Sub
                                End If
                            End If
                        End If
                    ElseIf objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.fn_chk_booking('" & requestid.Text & "')") = "E" Then
                        '''' Excursion
                        sqlstr = "execute sp_validate_reservation_invoice_excursions '" & CType(requestid.Text, String) & "' "
                        ds = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), sqlstr)
                        If ds IsNot Nothing Then
                            If ds.Tables(0).Rows.Count > 0 Then
                                'display in grid
                                grdInvError.Visible = True
                                grdInvError.DataSource = ds.Tables(0)
                                grdInvError.DataBind()

                                btngenerateinvoice.Style("visibility") = "visible"

                                'Exit Sub
                                GoTo v
                                'this is commented by Elsitta on 210415 because it is not taking the full for loop. 
                            Else
                                If saveexcursion(requestid.Text, custname) = False Then
                                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Save Unsuccessful !! ');", True)
                                    'Exit Sub
                                    GoTo v
                                End If
                            End If
                        End If
                    End If

                End If

            End If
            i = i + 1
        Next




        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ReservationWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)



v:
     

        '------------------------------------------------------
        Dim oldrequestid As String
        If grdInvError.Rows.Count <> 0 Then
            oldrequestid = IIf(grdInvError.Rows(0).Cells(2).Text <> "", grdInvError.Rows(0).Cells(2).Text, "")
            Dim count As Integer = grdInvError.Rows.Count
            For i = 1 To grdInvError.Rows.Count - 1



                If (grdInvError.Rows(i).Cells(2).Text = oldrequestid) Then
                    grdInvError.Rows(i).Cells(2).Text = String.Empty
                Else
                    oldrequestid = grdInvError.Rows(i).Cells(2).Text
                End If




            Next
        End If









        '------------------------------------------------------











        'Dim strpop As String = ""
        'strpop = "window.open('ReservationInvoiceSearch.aspx','ReservationWindowPostBack','width=1000,height=620 left=10,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


    End Sub

    Private Function save(ByVal requestid As String, ByVal custname As String) As Boolean
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
            parm(2) = New SqlParameter("@invoiceno", SqlDbType.VarChar, 20)
            parm(2).Direction = ParameterDirection.Output
            parm(2).Value = ""
            For i = 0 To 2
                mySqlCmd.Parameters.Add(parm(i))
            Next
            mySqlCmd.ExecuteNonQuery()

            Dim strInvoiceNo As String = ""
            strInvoiceNo = parm(2).Value.ToString()
            'temp  saving
            'mySqlCmd = New SqlCommand
            'mySqlCmd.Connection = mySqlConn
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandText = "sp_add_salesinvoice"
            Dim parms2 As New List(Of SqlParameter)
            Dim parm2(3) As SqlParameter

            parm2(0) = New SqlParameter("@requestid", CType(requestid, String))
            parm2(1) = New SqlParameter("@adduser", CType(Session("GlobalUserName"), String))
            parm2(2) = New SqlParameter("@invoiceno", CType(strInvoiceNo, String))
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

            ''mail sending to accounts dept not requires 16122014 
            'If strInvoiceNo <> "" Then
            '    'objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "")
            '    ' when saved , gets invoice no , send mail to accounts dept. 
            '    If SendMailInvoiceNo(strInvoiceNo, custname) = True Then
            '        'after send mail redirect to printpage.
            '    End If
            '    'Dim strpop As String = ""
            '    'strpop = "window.open('ReservationPrintPage.aspx','Reservation','width=1000,height=620 left=10,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            '    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            'End If

        Catch ex As Exception
            save = False
            imgicon.Style("visibility") = "hidden"
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & "');", True)
        Finally
            sqlTrans.Dispose()
            mySqlConn.Close()
            mySqlConn.Dispose()
        End Try
        imgicon.Style("visibility") = "hidden"

        btngenerateinvoice.Style("visibility") = "visible"

    End Function
    Private Function saveTransfer(ByVal requestid As String, ByVal custname As String) As Boolean
        Dim mySqlCmd As SqlCommand
        saveTransfer = True
        'no errors, save with procedure
        Try

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

            mySqlCmd = New SqlCommand
            mySqlCmd.Connection = mySqlConn
            mySqlCmd.Transaction = sqlTrans

            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandText = "sp_autopost_reservation_invoice_transfers"
            Dim parms As New List(Of SqlParameter)
            Dim parm(3) As SqlParameter
            parm(0) = New SqlParameter("@requestid", CType(requestid, String))
            parm(1) = New SqlParameter("@userlogged", CType(Session("GlobalUserName"), String))
            parm(2) = New SqlParameter("@invoiceno", SqlDbType.VarChar, 20)
            parm(2).Direction = ParameterDirection.Output
            parm(2).Value = ""
            For i = 0 To 2
                mySqlCmd.Parameters.Add(parm(i))
            Next
            mySqlCmd.ExecuteNonQuery()

            Dim strInvoiceNo As String = ""
            strInvoiceNo = parm(2).Value.ToString()
            'temp  saving
            'mySqlCmd = New SqlCommand
            'mySqlCmd.Connection = mySqlConn
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandText = "sp_add_salesinvoice"
            Dim parms2 As New List(Of SqlParameter)
            Dim parm2(3) As SqlParameter

            parm2(0) = New SqlParameter("@requestid", CType(requestid, String))
            parm2(1) = New SqlParameter("@adduser", CType(Session("GlobalUserName"), String))
            parm2(2) = New SqlParameter("@invoiceno", CType(strInvoiceNo, String))
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

            ''mail sending to accounts dept not requires 16122014 
            'If strInvoiceNo <> "" Then
            '    'objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "")
            '    ' when saved , gets invoice no , send mail to accounts dept. 
            '    If SendMailInvoiceNo(strInvoiceNo, custname) = True Then
            '        'after send mail redirect to printpage.
            '    End If
            '    'Dim strpop As String = ""
            '    'strpop = "window.open('ReservationPrintPage.aspx','Reservation','width=1000,height=620 left=10,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            '    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            'End If

        Catch ex As Exception
            saveTransfer = False
            imgicon.Style("visibility") = "hidden"
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & "');", True)
        Finally
            sqlTrans.Dispose()
            mySqlConn.Close()
            mySqlConn.Dispose()
        End Try
        imgicon.Style("visibility") = "hidden"

        btngenerateinvoice.Style("visibility") = "visible"

    End Function
    Private Function saveexcursion(ByVal requestid As String, ByVal custname As String) As Boolean
        Dim mySqlCmd As SqlCommand
        saveexcursion = True
        'no errors, save with procedure
        Try

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

            mySqlCmd = New SqlCommand
            mySqlCmd.Connection = mySqlConn
            mySqlCmd.Transaction = sqlTrans

            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandText = "sp_autopost_reservation_invoice_excursions"
            Dim parms As New List(Of SqlParameter)
            Dim parm(3) As SqlParameter
            parm(0) = New SqlParameter("@requestid", CType(requestid, String))
            parm(1) = New SqlParameter("@userlogged", CType(Session("GlobalUserName"), String))
            parm(2) = New SqlParameter("@invoiceno", SqlDbType.VarChar, 20)
            parm(2).Direction = ParameterDirection.Output
            parm(2).Value = ""
            For i = 0 To 2
                mySqlCmd.Parameters.Add(parm(i))
            Next
            mySqlCmd.ExecuteNonQuery()

            Dim strInvoiceNo As String = ""
            strInvoiceNo = parm(2).Value.ToString()
            'temp  saving
            'mySqlCmd = New SqlCommand
            'mySqlCmd.Connection = mySqlConn
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.CommandText = "sp_add_salesinvoice"
            Dim parms2 As New List(Of SqlParameter)
            Dim parm2(3) As SqlParameter

            parm2(0) = New SqlParameter("@requestid", CType(requestid, String))
            parm2(1) = New SqlParameter("@adduser", CType(Session("GlobalUserName"), String))
            parm2(2) = New SqlParameter("@invoiceno", CType(strInvoiceNo, String))
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

            ''mail sending to accounts dept not requires 16122014 
            'If strInvoiceNo <> "" Then
            '    'objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "")
            '    ' when saved , gets invoice no , send mail to accounts dept. 
            '    If SendMailInvoiceNo(strInvoiceNo, custname) = True Then
            '        'after send mail redirect to printpage.
            '    End If
            '    'Dim strpop As String = ""
            '    'strpop = "window.open('ReservationPrintPage.aspx','Reservation','width=1000,height=620 left=10,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            '    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            'End If

        Catch ex As Exception
            saveexcursion = False
            imgicon.Style("visibility") = "hidden"
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & "');", True)
        Finally
            sqlTrans.Dispose()
            mySqlConn.Close()
            mySqlConn.Dispose()
        End Try
        imgicon.Style("visibility") = "hidden"

        btngenerateinvoice.Style("visibility") = "visible"

    End Function
    Public Function SendMailInvoiceNo(ByVal prm_strInvoiceNo As String, ByVal custname As String) As Boolean
        SendMailInvoiceNo = True

        Dim strMessage As String = ""
        Dim strSubject As String = ""

        Try

            strSubject = "Invoice No:" + CType(prm_strInvoiceNo, String) + " Generated."

            strMessage = "Invoice No:" + CType(prm_strInvoiceNo, String) + " generated for Agent Name : " & custname & " <br /><br /><br />"

            strMessage += "<br />&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp"
            strMessage += "<br />Regards<br /><br />" + CType(Session("GlobalUserName"), String)


            Dim strfrommail = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='563'")
            Dim strTomail = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='568'")
            Dim strcc = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1060'")

            If objEmail.SendEmailBCC(strfrommail, strTomail, strcc, strSubject, strMessage, "") Then
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

    'Private Sub test(ByVal sender As Object, ByVal e As System.EventArgs)
    '    btngenerateinvoice_Click(sender, e)
    'End Sub

    Protected Sub btndummy_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        validategrid()
    End Sub

End Class
