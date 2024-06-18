'------------================--------------=======================------------------================
'   Module Name    :    rptComplimentReportSearch.aspx 
'   Developer Name :    Julie Jacob
'   Date           :    
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class rptComplimentReportSearch
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
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                'hidecontrols()

                txtFromDate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy")
                txtToDate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustomercode, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1 order by agentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustomername, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustomercodeto, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1 order by agentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustomernameto, "agentname", "agentcode", " select agentname,agentcode from agentmast where active=1 order by agentname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmarketcode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmarketname, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmarketcodeto, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlmarketnameto, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

                txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")


                rbmarketall.Attributes.Add("onclick", "rbevent(this,'" & rbmarketrange.ClientID & "','A','Market')")
                rbmarketrange.Attributes.Add("onclick", "rbevent(this,'" & rbmarketall.ClientID & "','R','Market')")
                rbCustall.Attributes.Add("onclick", "rbevent(this,'" & rbcustrange.ClientID & "','A','Customer')")
                rbcustrange.Attributes.Add("onclick", "rbevent(this,'" & rbCustall.ClientID & "','R','Customer')")
            Catch ex As Exception

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("rptComplimentReportSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

            End Try

        Else
            If rbmarketall.Checked Then
                'ddlmarketcode.Style("visibility") = "hidden"
                'ddlmarketcodeto.Style("visibility") = "hidden"
                'ddlmarketname.Style("visibility") = "hidden"
                'ddlmarketnameto.Style("visibility") = "hidden"
                ddlmarketcode.Disabled = True
                ddlmarketcodeto.Disabled = True
                ddlmarketname.Disabled = True
                ddlmarketnameto.Disabled = True
                ddlmarketcode.Value = "[Select]"
                ddlmarketcodeto.Value = "[Select]"
                ddlmarketname.Value = "[Select]"
                ddlmarketnameto.Value = "[Select]"
            End If
            If rbmarketrange.Checked Then
                'ddlmarketcode.Style("visibility") = "visible"
                'ddlmarketcodeto.Style("visibility") = "visible"
                'ddlmarketname.Style("visibility") = "visible"
                'ddlmarketnameto.Style("visibility") = "visible"
                ddlmarketcode.Disabled = False
                ddlmarketcodeto.Disabled = False
                ddlmarketname.Disabled = False
                ddlmarketnameto.Disabled = False
            End If

            If rbCustall.Checked Then
                'ddlcustomercode.Style("visibility") = "hidden"
                'ddlcustomercodeto.Style("visibility") = "hidden"
                'ddlcustomername.Style("visibility") = "hidden"
                'ddlcustomernameto.Style("visibility") = "hidden"
                ddlcustomercode.Disabled = True
                ddlcustomercodeto.Disabled = True
                ddlcustomername.Disabled = True
                ddlcustomernameto.Disabled = True
                ddlcustomercode.Value = "[Select]"
                ddlcustomercodeto.Value = "[Select]"
                ddlcustomername.Value = "[Select]"
                ddlcustomernameto.Value = "[Select]"
            End If
            If rbcustrange.Checked Then
                'ddlcustomercode.Style("visibility") = "visible"
                'ddlcustomercodeto.Style("visibility") = "visible"
                'ddlcustomername.Style("visibility") = "visible"
                'ddlcustomernameto.Style("visibility") = "visible"
                ddlcustomercode.Disabled = False
                ddlcustomercodeto.Disabled = False
                ddlcustomername.Disabled = False
                ddlcustomernameto.Disabled = False
            End If

        End If
        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            ddlcustomercode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlcustomername.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlcustomercodeto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlcustomernameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            ddlmarketcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlmarketname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlmarketcodeto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlmarketnameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        End If
    End Sub
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Response.Redirect("~\MainPage.aspx", False)
    End Sub
    Protected Sub btnLoadreport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadreport.Click
        Try
            If ValidatePage() = False Then
                Exit Sub
            End If
            Dim strReportTitle As String = ""

            Dim strfromdate As String = ""
            Dim strtodate As String = ""

            Dim strfromcustcode As String = ""
            Dim strtocustcode As String = ""
            Dim strfrommarketcode As String = ""
            Dim strtomarketcode As String = ""
            Dim strreporttype As String = ""

            strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")
            If Trim(txtToDate.Text) <> "" Then
                strtodate = Format(CType(txtToDate.Text, Date), "yyyy/MM/dd")
            Else
                strtodate = strfromdate
            End If


          
            strfromcustcode = IIf(UCase(ddlcustomercode.Items(ddlcustomercode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlcustomercode.Items(ddlcustomercode.SelectedIndex).Text, "")
            strtocustcode = IIf(UCase(ddlcustomercodeto.Items(ddlcustomercodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlcustomercodeto.Items(ddlcustomercodeto.SelectedIndex).Text, "")
            strfrommarketcode = IIf(UCase(ddlmarketcode.Items(ddlmarketcode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlmarketcode.Items(ddlmarketcode.SelectedIndex).Text, "")
            strtomarketcode = IIf(UCase(ddlmarketcodeto.Items(ddlmarketcodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlmarketcodeto.Items(ddlmarketcodeto.SelectedIndex).Text, "")
          
            If ddlrpttype.Value = 0 Then
                strreporttype = "0"
            Else
                strreporttype = "1"
            End If
            strReportTitle = "Complimentary Reports"
         
            Dim strpop As String = ""
            strpop = "window.open('rptComplimentReport.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&fromcust=" & strfromcustcode & "&tocust=" & strtocustcode & "&frommarket=" & strfrommarketcode & "&tomarket=" & strtomarketcode & "&type=" & strreporttype & "','RepComplimentary','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptComplimentReportSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub
    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click

    End Sub
    Protected Sub hidecontrols()
        ddlmarketcode.Style("visibility") = "hidden"
        ddlmarketname.Style("visibility") = "hidden"
        ddlmarketcodeto.Style("visibility") = "hidden"
        ddlmarketnameto.Style("visibility") = "hidden"

        ddlcustomercode.Style("visibility") = "hidden"
        ddlcustomername.Style("visibility") = "hidden"
        ddlcustomercodeto.Style("visibility") = "hidden"
        ddlcustomernameto.Style("visibility") = "hidden"

    End Sub

    Public Function ValidatePage() As Boolean
        Try
            If txtFromDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter from date');", True)
                SetFocus(txtFromDate)
                ValidatePage = False
                Exit Function
            End If
            If txtToDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter to date');", True)
                SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If

            If CType(objDateTime.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objDateTime.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                ValidatePage = False
                Exit Function
            End If
            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptComplimentReportSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Function
End Class
