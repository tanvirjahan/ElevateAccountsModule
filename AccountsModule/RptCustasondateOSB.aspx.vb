'------------================--------------=======================------------------================
'   Module Name    :    RptCustomerTrialBalance.aspx
'   Developer Name :    Govardhan
'   Date           :    
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region

Partial Class RptCustasondateOSB
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDate As New clsDateTime
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ViewState.Add("RptCustomerTrialBalanceTranType", "WOCR")
            If IsPostBack = False Then
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                If ViewState("RptCustomerTrialBalanceTranType") = "WOCR" Then
                    lblHeading.Text = "Customer Outstanding and Collection"
                End If

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustfrm, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1  order by agentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustfrmname, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustto, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1  order by agentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcusttoname, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname", True)


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromAccount, "acctcode", "acctname", "select * from acctmast  where controlyn='Y' and cust_supp='C' order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromAccountName, "acctname", "acctcode", "select * from acctmast where controlyn='Y' and cust_supp='C' order by acctname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToAccount, "acctcode", "acctname", "select * from acctmast where controlyn='Y' and cust_supp='C' order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToAccountName, "acctname", "acctcode", "select * from acctmast where controlyn='Y' and cust_supp='C' order by acctname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromCategory, "agentcatcode", "agentcatname", "select * from agentcatmast where active=1 order by agentcatcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromCategoryName, "agentcatname", "agentcatcode", "select * from agentcatmast where active=1 order by agentcatname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToCategory, "agentcatcode", "agentcatname", "select * from agentcatmast where active=1 order by agentcatcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToCategoryName, "agentcatname", "agentcatcode", "select * from agentcatmast where active=1 order by agentcatname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromMarket, "plgrpcode", "plgrpname", "select * from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromMarketName, "plgrpname", "plgrpcode", "select * from plgrpmast where active=1 order by plgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToMarket, "plgrpcode", "plgrpname", "select * from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToMarketName, "plgrpname", "plgrpcode", "select * from plgrpmast where active=1 order by plgrpname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromCountry, "ctrycode", "ctryname", "select * from ctrymast where active=1 order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromCountryName, "ctryname", "ctrycode", "select * from ctrymast where active=1 order by ctryname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToCountry, "ctrycode", "ctryname", "select * from ctrymast where active=1 order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToCountryName, "ctryname", "ctrycode", "select * from ctrymast where active=1 order by ctryname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrency, "currcode", "currcode", "select option_selected as currcode from reservation_parameters where param_id=457 union select 'A/C Currency'", False)


                '----------------------------- Default Dates
                txttoDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")

                If Request.QueryString("todate") <> "" Then
                    txttoDate.Text = Format("U", CType(Request.QueryString("todate"), Date))
                End If

                If Request.QueryString("fromcode") <> "" Then
                    rbncustrange.Checked = True
                    ddlFromAccountName.Value = Request.QueryString("fromcode")
                    ddlFromAccount.Value = ddlFromAccountName.Items(ddlFromAccountName.SelectedIndex).Text

                Else
                    rbncustrange.Checked = False
                End If
                If Request.QueryString("tocode") <> "" Then
                    rbncustrange.Checked = True
                    ddlToAccountName.Value = Request.QueryString("tocode")
                    ddlToAccount.Value = ddlToAccountName.Items(ddlToAccountName.SelectedIndex).Text
                Else
                    rbncustrange.Checked = False
                End If

                If Request.QueryString("fromMarkcode") <> "" Then

                    rbMarkrange.Checked = True
                    ddlFromMarketName.Value = Request.QueryString("fromMarkcode")
                    ddlFromMarket.Value = ddlFromMarketName.Items(ddlFromMarketName.SelectedIndex).Text
                Else
                    rbMarkrange.Checked = False
                End If
                If Request.QueryString("tomarkcode") <> "" Then
                    rbMarkrange.Checked = True
                    ddlToMarketName.Value = Request.QueryString("tomarkcode")
                    ddlToMarket.Value = ddlToMarketName.Items(ddlToMarketName.SelectedIndex).Text
                Else
                    rbMarkrange.Checked = False
                End If
                If Request.QueryString(" fromcat") <> "" Then
                    rbCatrange.Checked = True
                    ddlFromCategoryName.Value = Request.QueryString("fromcat")
                    ddlFromCategory.Value = ddlFromCategoryName.Items(ddlFromCategoryName.SelectedIndex).Text
                Else
                    rbCatrange.Checked = False
                End If

                If Request.QueryString(" tocat") <> "" Then
                    rbCatrange.Checked = True
                    ddlToCategoryName.Value = Request.QueryString("tocat")
                    ddlToCategory.Value = ddlToCategoryName.Items(ddlToCategoryName.SelectedIndex).Text
                Else
                    rbCatrange.Checked = False
                End If

                If Request.QueryString(" fromctry") <> "" Then
                    rbCtrrange.Checked = True
                    ddlFromCountryName.Value = Request.QueryString("fromctry")
                    ddlFromCountry.Value = ddlFromCountryName.Items(ddlFromCountryName.SelectedIndex).Text

                Else
                    rbCtrrange.Checked = False
                End If

                If Request.QueryString(" toctry") <> "" Then
                    rbCtrrange.Checked = True
                    ddlToCountryName.Value = Request.QueryString("toctry")
                    ddlToCountry.Value = ddlToCountryName.Items(ddlToCountryName.SelectedIndex).Text
                Else
                    rbCtrrange.Checked = False
                End If
                If Request.QueryString("fromglcode") <> "" Then

                    rbCtrrange.Checked = True
                    ddlFromAccountName.Value = Request.QueryString("fromglcode")
                    ddlFromAccount.Value = ddlFromAccountName.Items(ddlFromAccountName.SelectedIndex).Text
                Else
                    rbCtrrange.Checked = False
                End If

                If Request.QueryString("toglcode") <> "" Then
                    rbCtrrange.Checked = True
                    ddlToAccountName.Value = Request.QueryString("toglcode")
                    ddlToAccount.Value = ddlToAccountName.Items(ddlToAccountName.SelectedIndex).Text
                Else
                    rbCtrrange.Checked = False

                End If

                ddlCurrency.SelectedIndex = 1

                If Request.QueryString("orderby") <> "" Then
                    ddlrptord.SelectedIndex = Request.QueryString("orderby")
                End If
                If Request.QueryString("includezero") <> "" Then
                    ddlinclzero.SelectedIndex = Request.QueryString("includezero")

                End If
                If Request.QueryString("gpby") <> "" Then
                    ddlgpby.SelectedIndex = Request.QueryString("gpby")
                End If


                ' --
                rbACall.Attributes.Add("onclick", "rbevent(this,'" & rbACrange.ClientID & "','A','Account')")
                rbACrange.Attributes.Add("onclick", "rbevent(this,'" & rbACall.ClientID & "','R','Account')")
                rbCatall.Attributes.Add("onclick", "rbevent(this,'" & rbCatrange.ClientID & "','A','Category')")
                rbCatrange.Attributes.Add("onclick", "rbevent(this,'" & rbCatall.ClientID & "','R','Category')")
                rbMarkall.Attributes.Add("onclick", "rbevent(this,'" & rbMarkrange.ClientID & "','A','Market')")
                rbMarkrange.Attributes.Add("onclick", "rbevent(this,'" & rbMarkall.ClientID & "','R','Market')")
                rbCtrall.Attributes.Add("onclick", "rbevent(this,'" & rbCtrrange.ClientID & "','A','Country')")
                rbCtrrange.Attributes.Add("onclick", "rbevent(this,'" & rbCtrall.ClientID & "','R','Country')")
                rbncustall.Attributes.Add("onclick", "rbevent(this,'" & rbncustrange.ClientID & "','A','customer')")
                rbncustrange.Attributes.Add("onclick", "rbevent(this,'" & rbncustall.ClientID & "','R','customer')")


                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlFromAccount.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlFromAccountName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlToAccount.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlToAccountName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlFromCategory.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlFromCategoryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlToCategory.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlToCategoryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlFromMarket.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlFromMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlToMarket.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlToMarketName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlFromCountry.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlFromCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlToCountry.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlToCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCurrency.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlcustfrm.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlcustto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlcustfrmname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlcusttoname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

            Else
                checkrb_status()
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RptCustomerTrialBalanceWindowPostBack") Then

        End If

    End Sub

    Public Sub checkrb_status()
        If rbACall.Checked = True Then
            ddlFromAccount.Disabled = True
            ddlFromAccountName.Disabled = True
            ddlToAccount.Disabled = True
            ddlToAccountName.Disabled = True
        Else
            ddlFromAccount.Disabled = False
            ddlFromAccountName.Disabled = False
            ddlToAccount.Disabled = False
            ddlToAccountName.Disabled = False
        End If
        If rbCatall.Checked = True Then
            ddlFromCategory.Disabled = True
            ddlFromCategoryName.Disabled = True
            ddlToCategory.Disabled = True
            ddlToCategoryName.Disabled = True
        Else
            ddlFromCategory.Disabled = False
            ddlFromCategoryName.Disabled = False
            ddlToCategory.Disabled = False
            ddlToCategoryName.Disabled = False
        End If
        If rbMarkall.Checked = True Then
            ddlFromMarket.Disabled = True
            ddlFromMarketName.Disabled = True
            ddlToMarket.Disabled = True
            ddlToMarketName.Disabled = True
        Else
            ddlFromMarket.Disabled = False
            ddlFromMarketName.Disabled = False
            ddlToMarket.Disabled = False
            ddlToMarketName.Disabled = False
        End If
        If rbCtrall.Checked = True Then
            ddlFromCountry.Disabled = True
            ddlFromCountryName.Disabled = True
            ddlToCountry.Disabled = True
            ddlToCountryName.Disabled = True
        Else
            ddlFromCountry.Disabled = False
            ddlFromCountryName.Disabled = False
            ddlToCountry.Disabled = False
            ddlToCountryName.Disabled = False
        End If




        If rbncustall.Checked = True Then
            ddlcustfrm.Disabled = True
            ddlcustto.Disabled = True
            ddlcustfrmname.Disabled = True
            ddlcusttoname.Disabled = True
        Else
            ddlcustfrm.Disabled = False
            ddlcustto.Disabled = False
            ddlcustfrmname.Disabled = False
            ddlcusttoname.Disabled = False
        End If
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("~/MainPage.aspx")
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Try
            'If ValidatePage() = True Then
            If ValidatePage() = False Then
                Exit Sub
            End If

            ViewState.Add("Pageame", "CustomertrialbalReport")
            ViewState.Add("BackPageName", "RptCustasondateOSB.aspx")

            Dim strReportTitle As String = ""

            Dim strfromdate As String = ""
            Dim strtodate As String = ""
            Dim strmovflag As String = ""

            Dim strfromcode As String = ""
            Dim strtocode As String = ""
            Dim strmarketcode As String = ""
            Dim strmarketcodeto As String = ""
            Dim strcatcode As String = ""
            Dim strcatcodeto As String = ""
            Dim strglcode As String = ""
            Dim strglcodeto As String = ""
            Dim strcurrtype As String = ""
            Dim strorderby As String = ""
            Dim strincludezero As String = ""
            Dim strgpby As String = ""

            Dim strfromctry As String = ""
            Dim strtoctry As String = ""
            Dim withCredit As String = ""
            Dim category As String = ""
            Dim strtrialtype As String = ""

            strtodate = Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            strfromcode = IIf(UCase(ddlcustfrm.Items(ddlcustfrm.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlcustfrm.Items(ddlcustfrm.SelectedIndex).Text, "")
            strtocode = IIf(UCase(ddlcustto.Items(ddlcustto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlcustto.Items(ddlcustto.SelectedIndex).Text, "")

            strmarketcode = IIf(UCase(ddlFromMarket.Items(ddlFromMarket.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromMarket.Items(ddlFromMarket.SelectedIndex).Text, "")
            strmarketcodeto = IIf(UCase(ddlToMarket.Items(ddlToMarket.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToMarket.Items(ddlToMarket.SelectedIndex).Text, "")

            strcatcode = IIf(UCase(ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text, "")
            strcatcodeto = IIf(UCase(ddlToCategory.Items(ddlToCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCategory.Items(ddlToCategory.SelectedIndex).Text, "")


            strfromctry = IIf(UCase(ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text, "")
            strtoctry = IIf(UCase(ddlToCountry.Items(ddlToCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCountry.Items(ddlToCountry.SelectedIndex).Text, "")

            strglcode = IIf(UCase(ddlFromAccount.Items(ddlFromAccount.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromAccount.Items(ddlFromAccount.SelectedIndex).Text, "")
            strglcodeto = IIf(UCase(ddlToAccount.Items(ddlToAccount.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToAccount.Items(ddlToAccount.SelectedIndex).Text, "")
            strcurrtype = IIf(ddlCurrency.Value = "A/C Currency", 1, 0)
            strorderby = ddlrptord.SelectedIndex
            strincludezero = ddlinclzero.SelectedIndex
            strgpby = ddlgpby.SelectedIndex
            withCredit = 0

            Dim strpop As String = ""
            strpop = "window.open('RptcustasondateOSBReport.aspx?type=C&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & " &trialtype=" & strtrialtype & " ','RepCustBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptcustasondateOSBReport.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ViewState("RptCustomerTrialBalanceTranType") = "WOCR" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptCustomerTrialBalance','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        ElseIf ViewState("RptCustomerTrialBalanceTranType") = "WTCR" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptCustomerwithCRBalance','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        End If
    End Sub
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Try
            If txttoDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txttoDate.ClientID + "');", True)
                'SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If


            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function

    Protected Sub Button1_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try

            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(16) As SqlParameter

            Dim strReportTitle As String = ""

            Dim strfromdate As String = ""
            Dim strtodate As String = ""
            Dim strmovflag As String = ""

            Dim strfromcode As String = ""
            Dim strtocode As String = ""
            Dim strmarketcode As String = ""
            Dim strmarketcodeto As String = ""
            Dim strcatcode As String = ""
            Dim strcatcodeto As String = ""
            Dim strglcode As String = ""
            Dim strglcodeto As String = ""
            Dim strcurrtype As String = ""
            Dim strorderby As String = ""
            Dim strincludezero As String = ""
            Dim strgpby As String = ""

            Dim strfromctry As String = ""
            Dim strtoctry As String = ""
            Dim withCredit As String = ""
            Dim category As String = ""


            strtodate = Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            strfromcode = IIf(UCase(ddlcustfrm.Items(ddlcustfrm.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlcustfrm.Items(ddlcustfrm.SelectedIndex).Text, "")
            strtocode = IIf(UCase(ddlcustto.Items(ddlcustto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlcustto.Items(ddlcustto.SelectedIndex).Text, "")

            strmarketcode = IIf(UCase(ddlFromMarket.Items(ddlFromMarket.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromMarket.Items(ddlFromMarket.SelectedIndex).Text, "")
            strmarketcodeto = IIf(UCase(ddlToMarket.Items(ddlToMarket.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToMarket.Items(ddlToMarket.SelectedIndex).Text, "")

            strcatcode = IIf(UCase(ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text, "")
            strcatcodeto = IIf(UCase(ddlToCategory.Items(ddlToCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCategory.Items(ddlToCategory.SelectedIndex).Text, "")


            strfromctry = IIf(UCase(ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text, "")
            strtoctry = IIf(UCase(ddlToCountry.Items(ddlToCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCountry.Items(ddlToCountry.SelectedIndex).Text, "")

            strglcode = IIf(UCase(ddlFromAccount.Items(ddlFromAccount.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromAccount.Items(ddlFromAccount.SelectedIndex).Text, "")
            strglcodeto = IIf(UCase(ddlToAccount.Items(ddlToAccount.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToAccount.Items(ddlToAccount.SelectedIndex).Text, "")
            strcurrtype = IIf(ddlCurrency.Value = "A/C Currency", 1, 0)
            strorderby = ddlrptord.SelectedIndex
            strincludezero = ddlinclzero.SelectedIndex
            strgpby = ddlgpby.SelectedIndex


            If ViewState("RptCustomerTrialBalanceTranType") = "WOCR" Then
                withCredit = 0
            ElseIf ViewState("RptCustomerTrialBalanceTranType") = "WTCR" Then
                withCredit = 1
            End If


            parm(0) = New SqlParameter("@todate", strtodate)
            parm(1) = New SqlParameter("@fromcode", strfromcode)
            parm(2) = New SqlParameter("@tocode", strtocode)
            parm(3) = New SqlParameter("@frommarkcode", strmarketcode)
            parm(4) = New SqlParameter("@tomarkcode", strmarketcodeto)
            parm(5) = New SqlParameter("@fromctry", strfromctry)
            parm(6) = New SqlParameter("@toctry", strtoctry)
            parm(7) = New SqlParameter("@fromcat", strcatcode)
            parm(8) = New SqlParameter("@tocat", strcatcodeto)
            parm(9) = New SqlParameter("@fromglcode", strglcode)
            parm(10) = New SqlParameter("@toglcode", strglcodeto)
            parm(11) = New SqlParameter("@currtype", strcurrtype)
            parm(12) = New SqlParameter("@orderby", strorderby)
            parm(13) = New SqlParameter("@includezero", strincludezero)
            parm(14) = New SqlParameter("@withCredit", withCredit)


            For i = 0 To 14
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_customer_osmonthwise_xls", parms)

            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    objUtils.ExportToExcel(ds, Response)
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Record found' );", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptCustasondateOSB.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub
End Class
