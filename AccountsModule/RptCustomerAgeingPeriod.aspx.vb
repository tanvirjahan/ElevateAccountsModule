#Region "Namespace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region
Partial Class RptCustomerAgeingPeriod
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDate As New clsDateTime
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack = False Then
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                If ddlType.SelectedIndex = 0 Then
                    lblHeading.Text = "Customer Ageing Period Summary"
                    ViewState.Add("reporttype", "CustomerAgeingperiodSummary")
                ElseIf ddlType.SelectedIndex = 1 Then
                    lblHeading.Text = "Customer Ageing Period Detail"
                    ViewState.Add("reporttype", "CustomerAgeingperioddetail")
                End If

                SetFocus(rbACall)

                'Customer 
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromAccount, "agentcode", "agentname", "select agentcode,agentname  from agentmast where active=1 order by agentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromAccountName, "agentname", "agentcode", "select agentname,agentcode  from agentmast where active=1 order by agentname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToAccount, "agentcode", "agentname", "select agentcode,agentname  from agentmast where active=1 order by agentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToAccountName, "agentname", "agentcode", "select agentname,agentcode  from agentmast where active=1 order by agentname", True)

                'Controll acc
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromControl, "acctcode", "acctname", "select acctcode,acctname from acctmast where controlyn='Y'and cust_supp='C' order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromControlName, "acctname", "acctcode", "select acctname,acctcode from acctmast where controlyn='Y'and cust_supp='C' order by acctname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToControl, "acctcode", "acctname", "select acctcode,acctname from acctmast where controlyn='Y'and cust_supp='C' order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToControlName, "acctname", "acctcode", "select acctname,acctcode from acctmast where controlyn='Y'and cust_supp='C' order by acctname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromCategory, "agentcatcode", "agentcatname", "select agentcatcode ,agentcatname from agentcatmast where active=1 order by agentcatcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromCategoryName, "agentcatname", "agentcatcode", "select agentcatname,agentcatcode from agentcatmast where active=1 order by agentcatname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToCategory, "agentcatcode", "agentcatname", "select agentcatcode,agentcatname from agentcatmast where active=1 order by agentcatcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToCategoryName, "agentcatname", "agentcatcode", "select agentcatname,agentcatcode from agentcatmast where active=1 order by agentcatname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromMarket, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToMarket, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToMarketName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromCountry, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1 order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromCountryName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1 order by ctryname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToCountry, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1 order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToCountryName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1 order by ctryname", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCurrency, "currcode", "currcode", "select option_selected as currcode from reservation_parameters where param_id=457 union select 'A/C Currency'", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrency, "currcode", "currcode", "select option_selected as currcode from reservation_parameters where param_id=457 union select 'A/C Currency'", False)

                '----------------------------- Default Dates
                txtFromDate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy")
                ddlCurrency.SelectedIndex = 1

                rbACall.Attributes.Add("onclick", "rbevent(this,'" & rbACrange.ClientID & "','A','Account')")
                rbACrange.Attributes.Add("onclick", "rbevent(this,'" & rbACall.ClientID & "','R','Account')")
                rbControlall.Attributes.Add("onclick", "rbevent(this,'" & rbControlrange.ClientID & "','A','Control')")
                rbControlrange.Attributes.Add("onclick", "rbevent(this,'" & rbControlall.ClientID & "','R','Control')")
                rbCatall.Attributes.Add("onclick", "rbevent(this,'" & rbCatrange.ClientID & "','A','Category')")
                rbCatrange.Attributes.Add("onclick", "rbevent(this,'" & rbCatall.ClientID & "','R','Category')")
                rbMarkall.Attributes.Add("onclick", "rbevent(this,'" & rbMarkrange.ClientID & "','A','Market')")
                rbMarkrange.Attributes.Add("onclick", "rbevent(this,'" & rbMarkall.ClientID & "','R','Market')")
                rbCtrall.Attributes.Add("onclick", "rbevent(this,'" & rbCtrrange.ClientID & "','A','Country')")
                rbCtrrange.Attributes.Add("onclick", "rbevent(this,'" & rbCtrall.ClientID & "','R','Country')")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlFromAccount.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlFromAccountName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlToAccount.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlToAccountName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlFromControl.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlFromControlName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlToControl.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlToControlName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

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
                    ddlReportGroup.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ' ddlReportLayout.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlReportOrder.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlIncludeZero.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ' ddlPartyBreak.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlAgeingType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                btnReport.Attributes.Add("onclick", "return FormValidation()")
            Else
                checkrb_status()
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
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
        If rbControlall.Checked = True Then
            ddlFromControl.Disabled = True
            ddlFromControlName.Disabled = True
            ddlToControl.Disabled = True
            ddlToControlName.Disabled = True
        Else
            ddlFromControl.Disabled = False
            ddlFromControlName.Disabled = False
            ddlToControl.Disabled = False
            ddlToControlName.Disabled = False
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
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptCustomerAgeingSummary','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim strReportTitle As String = ""
            Dim strfromdate As String = ""
            Dim strtodate As String = ""
            Dim strfomaccode As String = ""
            Dim strtoaccode As String = ""
            Dim strfromctrlcode As String = ""
            Dim strtoctrlcode As String = ""
            Dim strfromccatcode As String = ""
            Dim strtoccatcode As String = ""
            Dim strfromcitycode As String = ""
            Dim strtocitycode As String = ""
            Dim strfromctrycode As String = ""
            Dim strtoctrycode As String = ""
            Dim strremarks As String = ""


            Dim strgroupby As Integer = 0
            Dim strperiod1 As Integer = 0
            Dim strperiod2 As Integer = 0

            Dim strtype As String = "C"
            Dim strdatetype As Integer = 0 '0 as on date, 1 from -to date
            Dim stragingtype As Integer = 0
            Dim strpdcyesno As Integer = 0
            Dim strincludezero As Integer = 0 ' 0 no, 1 yes
            Dim strcurr As Integer = 0 ' 0 Ac Currency, 1 base currency
            Dim strsumdet As Integer = 0 '0-Summary,1-Detail
            Dim strreportorder As String = "" ' 1 - Code, 2 Name

            Dim strreporttype As String = ""
            Dim strrepfilter As String = ""

            If ddlType.SelectedIndex = 0 Then
                strsumdet = 0
            Else
                strsumdet = 1
            End If

            strtodate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")


            strcurr = IIf(ddlCurrency.Value = "A/C Currency", 0, 1)
            stragingtype = ddlAgeingType.SelectedIndex
            strincludezero = ddlIncludeZero.SelectedIndex

            strfomaccode = IIf(UCase(ddlFromAccount.Items(ddlFromAccount.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromAccount.Items(ddlFromAccount.SelectedIndex).Text, "")
            strtoaccode = IIf(UCase(ddlToAccount.Items(ddlToAccount.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToAccount.Items(ddlToAccount.SelectedIndex).Text, "")

            strfromctrlcode = IIf(UCase(ddlFromControl.Items(ddlFromControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromControl.Items(ddlFromControl.SelectedIndex).Text, "")
            strtoctrlcode = IIf(UCase(ddlToControl.Items(ddlToControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToControl.Items(ddlToControl.SelectedIndex).Text, "")

            strfromccatcode = IIf(UCase(ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text, "")
            strtoccatcode = IIf(UCase(ddlToCategory.Items(ddlToCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCategory.Items(ddlToCategory.SelectedIndex).Text, "")

            strfromcitycode = IIf(UCase(ddlFromMarket.Items(ddlFromMarket.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromMarket.Items(ddlFromMarket.SelectedIndex).Text, "")
            strtocitycode = IIf(UCase(ddlToMarket.Items(ddlToMarket.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToMarket.Items(ddlToMarket.SelectedIndex).Text, "")

            strfromctrycode = IIf(UCase(ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text, "")
            strtoctrycode = IIf(UCase(ddlToCountry.Items(ddlToCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCountry.Items(ddlToCountry.SelectedIndex).Text, "")

            strpdcyesno = 0
            strincludezero = IIf(ddlIncludeZero.SelectedIndex = 0, 1, 0)
            strgroupby = Val(ddlReportGroup.SelectedIndex)
            strreportorder = Trim(ddlReportOrder.SelectedIndex)

            If Period1.Value = 6 Then
                strperiod1 = 150
            Else
                strperiod1 = Period1.Items(Period1.SelectedIndex).Text
            End If

            If period2.Value = 5 Then
                strperiod2 = 150
            Else
                strperiod2 = period2.Items(period2.SelectedIndex).Text
            End If

            strreporttype = strsumdet

            If ViewState("reporttype") = "CustomerAgeingperiodSummary" Then
                Dim strpop As String = ""
                strpop = "window.open('rptAgeingperiod.aspx?Pageame=rptAgeingperiod&BackPageName=rptAgeingperiod.aspx&todate=" & strtodate _
                & "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                & "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno & "&curr=" & strcurr _
                & "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                & "&Period1=" & strperiod1 & "&Period2=" & strperiod2 _
                & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "','RepCustAgeingSum','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf ViewState("reporttype") = "CustomerAgeingperioddetail" Then
                Dim strpop As String = ""
                strpop = "window.open('rptAgeingperiod.aspx?Pageame=rptAgeingperiod&BackPageName=rptAgeingperiod.aspx&todate=" & strtodate _
                & "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                & "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno & "&curr=" & strcurr _
                & "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                & "&Period1=" & strperiod1 & "&Period1=" & strperiod2 _
                & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "','RepCustAgeingDet','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptSupplierAgeingSummary.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("~/MainPage.aspx")
    End Sub

    Protected Sub Button1_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(14) As SqlParameter

            Dim strReportTitle As String = ""
            Dim strfromdate As String = ""
            Dim strtodate As String = ""
            Dim strfomaccode As String = ""
            Dim strtoaccode As String = ""
            Dim strfromctrlcode As String = ""
            Dim strtoctrlcode As String = ""
            Dim strfromccatcode As String = ""
            Dim strtoccatcode As String = ""
            Dim strfromcitycode As String = ""
            Dim strtocitycode As String = ""
            Dim strfromctrycode As String = ""
            Dim strtoctrycode As String = ""
            Dim strremarks As String = ""


            Dim strgroupby As Integer = 0

            Dim strtype As String = "C"
            Dim strdatetype As Integer = 0 '0 as on date, 1 from -to date
            Dim stragingtype As Integer = 0
            Dim strpdcyesno As Integer = 0
            Dim strincludezero As Integer = 0 ' 0 no, 1 yes
            Dim strcurr As Integer = 0 ' 0 Ac Currency, 1 base currency
            Dim strsumdet As Integer = 0 '0-Summary,1-Detail
            Dim strreportorder As String = "" ' 1 - Code, 2 Name

            Dim strreporttype As String = "SupplierAgeingSummary"
            Dim strrepfilter As String = ""
            If ViewState("reporttype") = "SupplierAgeingSummary" Or ViewState("reporttype") = "CustomerAgeingSummary" Then
                strsumdet = 0
            ElseIf ViewState("reporttype") = "SupplierAgeingDetail" Or ViewState("reporttype") = "CustomerAgeingDetail" Then
                strsumdet = 1
            End If
            strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")
            If ViewState("reporttype") = "CustomerAgeingDetail" Or ViewState("reporttype") = "CustomerAgeingSummary" Then
                strtype = "C"
            End If
            strcurr = IIf(ddlCurrency.Value = "A/C Currency", 0, 1)
            stragingtype = ddlAgeingType.SelectedIndex
            strincludezero = ddlIncludeZero.SelectedIndex

            strfomaccode = IIf(UCase(ddlFromAccount.Items(ddlFromAccount.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromAccount.Items(ddlFromAccount.SelectedIndex).Text, "")
            strtoaccode = IIf(UCase(ddlToAccount.Items(ddlToAccount.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToAccount.Items(ddlToAccount.SelectedIndex).Text, "")

            strfromctrlcode = IIf(UCase(ddlFromControl.Items(ddlFromControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromControl.Items(ddlFromControl.SelectedIndex).Text, "")
            strtoctrlcode = IIf(UCase(ddlToControl.Items(ddlToControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToControl.Items(ddlToControl.SelectedIndex).Text, "")

            strfromccatcode = IIf(UCase(ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text, "")
            strtoccatcode = IIf(UCase(ddlToCategory.Items(ddlToCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCategory.Items(ddlToCategory.SelectedIndex).Text, "")

            strfromcitycode = IIf(UCase(ddlFromMarket.Items(ddlFromMarket.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromMarket.Items(ddlFromMarket.SelectedIndex).Text, "")
            strtocitycode = IIf(UCase(ddlToMarket.Items(ddlToMarket.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToMarket.Items(ddlToMarket.SelectedIndex).Text, "")

            strfromctrycode = IIf(UCase(ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text, "")
            strtoctrycode = IIf(UCase(ddlToCountry.Items(ddlToCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCountry.Items(ddlToCountry.SelectedIndex).Text, "")

            strpdcyesno = 0
            strincludezero = IIf(ddlIncludeZero.SelectedIndex = 0, 1, 0)
            strgroupby = Val(ddlReportGroup.SelectedIndex)
            strreportorder = Trim(ddlReportOrder.SelectedIndex)

            parm(0) = New SqlParameter("@todate", strfromdate)
            parm(1) = New SqlParameter("@type", strtype)
            parm(2) = New SqlParameter("@currflg", strcurr)
            parm(3) = New SqlParameter("@fromacct", strfomaccode)
            parm(4) = New SqlParameter("@toacct", strtoaccode)
            parm(5) = New SqlParameter("@fromcontrol", strfromctrlcode)
            parm(6) = New SqlParameter("@tocontrol", strtoctrlcode)
            parm(7) = New SqlParameter("@fromcat", strfromccatcode)
            parm(8) = New SqlParameter("@tocat", strtoccatcode)
            parm(9) = New SqlParameter("@fromcity", strfromcitycode)
            parm(10) = New SqlParameter("@tocity", strtocitycode)
            parm(11) = New SqlParameter("@fromctry", strfromctrycode)
            parm(12) = New SqlParameter("@toctry", strtoctrycode)
            parm(13) = New SqlParameter("@agingtype", stragingtype)
            parm(14) = New SqlParameter("@summdet", strsumdet)

            For i = 0 To 14
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_statement_partyaging_xls", parms)

            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    objUtils.ExportToExcel(ds, Response)
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Record found' );", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptsuppliertrialbalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

End Class

