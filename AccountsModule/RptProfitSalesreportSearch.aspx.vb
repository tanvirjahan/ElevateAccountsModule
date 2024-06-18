'------------================--------------=======================------------------================
'   Module Name    :    RptSalesreportSearch.aspx 
'   Developer Name :    Sandeep Indulkar
'   Date           :    
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
#End Region


Partial Class RptProfitSalesreportSearch
    Inherits System.Web.UI.Page


#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDate As New clsDateTime
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                txtconnection.Value = Session("dbconnectionName")
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                If txtFromDate.Text = "" Then
                    'txtFromDate.Text = Format(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"),"select fdate from toursmaster"), Date), "dd/MM/yyy")
                    txtFromDate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyy")
                End If
                If txtToDate.Text = "" Then
                    txtToDate.Text = DateAdd(DateInterval.Month, 1, objDateTime.GetSystemDateOnly(Session("dbconnectionName")))
                End If

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryCD, "ctrycode", "ctryname", "select distinct ctrycode,ctryname from ctrymast where active=1 order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryNM, "ctryname", "ctrycode", "select distinct ctrycode,ctryname from ctrymast where active=1 order by ctryname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltoCountryCD, "ctrycode", "ctryname", "select distinct ctrycode,ctryname from ctrymast where active=1 order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltoCountryNM, "ctryname", "ctrycode", "select distinct ctrycode,ctryname from ctrymast where active=1 order by ctryname", True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomer, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname", True)

                rdbtnCountryAll.Attributes.Add("onclick", "AllRange('" & rdbtnCountryAll.ClientID & "','A','Cntry')")
                rdbtnCountryRange.Attributes.Add("onclick", "AllRange('" & rdbtnCountryRange.ClientID & "','R','Cntry')")

                If Request.QueryString("fromctrycode") <> "" Then
                    rdbtnCountryRange.Checked = True
                    rdbtnCountryAll.Checked = False
                    ddlCountryNM.Value = Request.QueryString("fromctrycode")
                    ddlCountryCD.Value = ddlCountryNM.Items(ddlCountryNM.SelectedIndex).Text
                End If
                If Request.QueryString("toctrycode") <> "" Then
                    rdbtnCountryRange.Checked = True
                    rdbtnCountryAll.Checked = False
                    ddltoCountryNM.Value = Request.QueryString("toctrycode")
                    ddltoCountryCD.Value = ddltoCountryNM.Items(ddltoCountryNM.SelectedIndex).Text

                    txtToCountryName.Value = ddltoCountryNM.Items(ddltoCountryNM.SelectedIndex).Text
                    txtTocountyCode.Value = ddltoCountryCD.Items(ddltoCountryCD.SelectedIndex).Text
                End If

                Dim typ As Type
                typ = GetType(DropDownList)
                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlCustomer.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCountryCD.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCountryNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddltoCountryCD.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddltoCountryNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

                'Dim c As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
                'ddlCurrencyType.Items.Clear()
                'ddlCurrencyType.Items.Add("A/C Currency")
                'ddlCurrencyType.Items.Add(c)



                ' txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")



            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("RptSalesreportSearch.aspx ", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltoCountryCD, "ctrycode", "ctryname", "select distinct ctrycode,ctryname from ctrymast where active=1 order by ctrycode", True, txtToCountryName.Value)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddltoCountryNM, "ctryname", "ctrycode", "select distinct ctrycode,ctryname from ctrymast where active=1 order by ctryname", True, txtTocountyCode.Value)
        End If

        btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Exit?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ChildWindowPostBack") Then
        End If
        checkrb_status()

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Response.Redirect("~/MainPage.aspx")
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptSalesreportSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnLoadreport_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        If ValidatePage() = True Then
            Try

                Dim strfromdate, strtodate, strcustcode, strreqid, strinvoiceno, strtype, poststate As String
                Dim strtoctrycode, strfromctrycode As String
                Dim currtype As Integer, strrpttype As Integer

                strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")

                strtodate = Format(CType(txtToDate.Text, Date), "yyyy/MM/dd")
                If ddlCustomer.Items(ddlCustomer.SelectedIndex).Text <> "[Select]" Then
                    strcustcode = ddlCustomer.Value 'ddlCustomer.Items(ddlCustomer.SelectedIndex).Text
                Else
                    strcustcode = ""
                End If

                strfromctrycode = IIf(UCase(ddlCountryCD.Items(ddlCountryCD.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCountryCD.Items(ddlCountryCD.SelectedIndex).Text, "")
                strtoctrycode = IIf(UCase(ddltoCountryCD.Items(ddltoCountryCD.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddltoCountryCD.Items(ddltoCountryCD.SelectedIndex).Text, "")

                ''strreqid = txtRequestId.Value
                strinvoiceno = txtInvoiceNo.Value

                '' strtype = ddlrpttype.SelectedValue

                If ddlStatus.Value <> "[Select]" Then
                    poststate = ddlStatus.Value
                Else
                    poststate = ""
                End If

                strrpttype = ddlrpttype.SelectedValue

                'currtype = ddlCurrencyType.SelectedIndex

                Dim strpop As String = ""
                ' strpop = "window.open('../Reservation/reserv_invoice_Report.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&custcode=" & strcustcode & "&reqid=" & strreqid & "&invoiceno=" & strinvoiceno & "&type=" & strtype & " ','SalesRegisterSearch','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('../AccountsModule/reserv_profitwise_Reportsales.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&custcode=" & strcustcode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode & "&invoiceno=" & strinvoiceno & "&type=" & strtype & "&poststate=" & poststate & "&rpttype=" & strrpttype & "&repname=salesreport  ','SalesreportSearch','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)



                'Response.Redirect("reserv_invoice_Report.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&custcode=" & strcustcode & "&reqid=" & strreqid & "&invoiceno=" & strinvoiceno & "&type=" & strtype, False)

                'End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("RptSalesreportSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Try
            If txtFromDate.Text = "" Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtFromDate.ClientID + "');", True)
                SetFocus(txtFromDate)
                ValidatePage = False
                Exit Function
            End If


            If txtToDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date field can not be blank.');", True)
                ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If

            If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                If CType(objDateTime.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objDateTime.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                    ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                    SetFocus(txtToDate)
                    ValidatePage = False
                    Exit Function
                End If
            End If



            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptSalesreportSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtInvoiceNo.Value = ""
        ''txtRequestId.Value = ""
        ddlCustomer.Value = "[Select]"
        ddlStatus.Value = "[Select]"

        ''txtCustRef.Value = ""
        txtFromDate.Text = ""
        txtToDate.Text = ""
        'ddlCurrencyType.Value = "[Select]"
        If txtFromDate.Text = "" Then
            'txtFromDate.Text = Format(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"),"select fdate from toursmaster"), Date), "dd/MM/yyy")
            txtFromDate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyy")
        End If
        If txtToDate.Text = "" Then
            txtToDate.Text = DateAdd(DateInterval.Month, 1, objDateTime.GetSystemDateOnly(Session("dbconnectionName")))
        End If
    End Sub

    Public Sub checkrb_status()
        If rdbtnCountryAll.Checked = True Then
            ddlCountryCD.Disabled = True
            ddlCountryNM.Disabled = True
            ddltoCountryCD.Disabled = True
            ddltoCountryNM.Disabled = True
        Else
            ddlCountryCD.Disabled = False
            ddlCountryNM.Disabled = False
            ddltoCountryCD.Disabled = False
            ddltoCountryNM.Disabled = False
        End If
    End Sub

    Protected Sub hidecontrols()
        ddlCountryCD.Style("visibility") = "hidden"
        ddlCountryNM.Style("visibility") = "hidden"
        ddltoCountryCD.Style("visibility") = "hidden"
        ddltoCountryNM.Style("visibility") = "hidden"

        lblctryfrom.Style("visibility") = "hidden"
        lblctryto.Style("visibility") = "hidden"

    End Sub

   
End Class
