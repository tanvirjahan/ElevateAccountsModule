#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class AccountsModule_RptInvNoSuppandcostSearch
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
                    txtFromDate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyy")
                End If
                If txtToDate.Text = "" Then
                    txtToDate.Text = DateAdd(DateInterval.Month, 1, objDateTime.GetSystemDateOnly(Session("dbconnectionName")))
                End If

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplier, "partyname", "partycode", "select partyname,partycode from partymast where active=1 order by partyname", True)

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlSupplier.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

                '  txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")



            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("RptCostRegisterSearch.aspx ", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Exit?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "ChildWindowPostBack") Then
        End If
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Response.Redirect("~/MainPage.aspx")
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptCostRegisterSearch','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnLoadreport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadreport.Click

        If ValidatePage() = True Then
            Try

                Dim strfromdate, strtodate, strsuppcode, strreqid, strinvoiceno, strtype, poststate, suppType As String

                strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")

                strtodate = Format(CType(txtToDate.Text, Date), "yyyy/MM/dd")
                If ddlSupplier.Items(ddlSupplier.SelectedIndex).Text <> "[Select]" Then
                    strsuppcode = ddlSupplier.Value 'ddlSupplier.Items(ddlSupplier.SelectedIndex).Text
                Else
                    strsuppcode = ""
                End If

                Dim strpop As String = ""
                ' strpop = "window.open('../Reservation/reserv_invoice_Report.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&custcode=" & strcustcode & "&reqid=" & strreqid & "&invoiceno=" & strinvoiceno & "&type=" & strtype & " ','SalesRegisterSearch','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"

                Dim suppcode As String = CType(ddlSupplier.Items(ddlSupplier.SelectedIndex).Value, String)
                Dim invbooktype As String = cbobooktype.SelectedValue
                If suppcode = "[Select]" Then suppcode = ""
                strpop = "window.open('../Reservation/reserv_invoice_Report.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&suppcode=" & suppcode & "&invbooktype=" & invbooktype & "&type=2&repname=RptInvNoSuppandcostSearch','RptInvNoSuppandcostSearch','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

                'Response.Redirect("reserv_invoice_Report.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&custcode=" & strcustcode & "&reqid=" & strreqid & "&invoiceno=" & strinvoiceno & "&type=" & strtype, False)

                'End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("RptCostRegisterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If

            If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
                If CType(objDateTime.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objDateTime.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                    SetFocus(txtToDate)
                    ValidatePage = False
                    Exit Function
                End If
            End If

            'If ddlSupplier.Items(ddlSupplier.SelectedIndex).Text = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select supplier.');", True)
            '    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
            '    SetFocus(ddlSupplier)
            '    ValidatePage = False
            '    Exit Function
            'End If

            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptCostRegisterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click

        ddlSupplier.Value = "[Select]"

        txtFromDate.Text = ""
        txtToDate.Text = ""
        suppSearch.Value = ""

        If txtFromDate.Text = "" Then
            'txtFromDate.Text = Format(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"),"select fdate from toursmaster"), Date), "dd/MM/yyy")
            txtFromDate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyy")
        End If
        If txtToDate.Text = "" Then
            txtToDate.Text = DateAdd(DateInterval.Month, 1, objDateTime.GetSystemDateOnly(Session("dbconnectionName")))
        End If
    End Sub

End Class
