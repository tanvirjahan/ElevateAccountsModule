
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
#End Region

Partial Class ReservationInvoice
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objdatetime As New clsDateTime
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
            ViewState.Add("ResState", Request.QueryString("State"))
            ViewState.Add("ResInvoiceNo", Request.QueryString("InvoiceNo"))
            ViewState.Add("ResRequestId", Request.QueryString("RequestId"))
            ViewState.Add("AppId", Request.QueryString("appid"))

            If Page.IsPostBack = False Then
                txtconnection.Value = Session("dbconnectionName")
                imgicon.Style("visibility") = "hidden"
                FillAllDDL()
            End If
            btnDisplay.Attributes.Add("onclick", "return FormValidation()")
            btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Exit?')==false)return false;")

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
            If Not (dpFromReqDate.txtDate.Text = "") Then
                parm(parameter.fromrequestdate) = New SqlParameter("@fromrequestdate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(dpFromReqDate.txtDate.Text), String))
            Else
                parm(parameter.fromrequestdate) = New SqlParameter("@fromrequestdate", "1900/01/01")
            End If
            If Not (dpFromReqDate.txtDate.Text = "") Then
                parm(parameter.torequestdate) = New SqlParameter("@torequestdate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(dpFromReqDate.txtDate.Text), String))
            Else
                parm(parameter.torequestdate) = New SqlParameter("@torequestdate", "1900/01/01")
            End If
            If Not (dpFromCheckindate.txtDate.Text = "") Then
                parm(parameter.fromcheckindate) = New SqlParameter("@fromcheckindate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(dpFromCheckindate.txtDate.Text), String))
            Else
                parm(parameter.fromcheckindate) = New SqlParameter("@fromcheckindate", "1900/01/01")
            End If
            If Not (dpToCheckindate.txtDate.Text = "") Then
                parm(parameter.tocheckindate) = New SqlParameter("@tocheckindate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(dpToCheckindate.txtDate.Text), String))
            Else
                parm(parameter.tocheckindate) = New SqlParameter("@tocheckindate", "1900/01/01")
            End If

            If Not (dpFromCheckOut.txtDate.Text = "") Then
                parm(parameter.fromcheckoutdate) = New SqlParameter("@fromcheckoutdate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(dpFromCheckOut.txtDate.Text), String))
            Else
                parm(parameter.fromcheckoutdate) = New SqlParameter("@fromcheckoutdate", "1900/01/01")
            End If
            If Not (dpTocheckOut.txtDate.Text = "") Then
                parm(parameter.tocheckoutdate) = New SqlParameter("@tocheckoutdate", CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(dpTocheckOut.txtDate.Text), String))
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
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_request_search_forinvoice", parms)
            Return ds
        Catch ex As Exception
            objUtils.WritErrorLog("ReservationInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

    Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Try
            'If CheckReconfirmation() = False Then
            '    Exit Sub
            'End If

            Dim dsResult As New DataSet
            lblMessage.Visible = False
            dsResult = doSearch()
            If dsResult.Tables.Count > 0 Then
                If dsResult.Tables(0).Rows.Count > 0 Then
                    divRes.Style("HEIGHT") = "500px"
                    gvSearchResult.DataSource = dsResult.Tables(0)
                    gvSearchResult.Visible = True
                    gvSearchResult.DataBind()
                    lblMessage.Text = ""
                Else
                    divRes.Style("HEIGHT") = "0px"
                    gvSearchResult.Visible = False
                    lblMessage.Visible = True
                    lblMessage.Text = "No Records Found"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('First reconfirm the booking.');", True)
                End If
            Else
                divRes.Style("HEIGHT") = "0px"
                gvSearchResult.Visible = False
                lblMessage.Visible = True
                lblMessage.Text = "No Records Found"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('First reconfirm the booking.');", True)
            End If
            imgicon.Style("visibility") = "hidden"
        Catch ex As Exception
            objUtils.WritErrorLog("ReservationInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
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

                Dim strpop As String = ""
                strpop = "window.open('RequestForInvoicing.aspx?State=New&appid=" + ViewState("AppId") + "&RequestId=" + CType(lblReqID.Text.Trim, String) + "','Reservation','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
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
End Class
