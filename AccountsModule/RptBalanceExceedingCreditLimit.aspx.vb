
#Region "Namespace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class RptBalanceExceedingCreditLimit 
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
                SetFocus(txttoDate)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustfrm, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1  order by agentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustfrmname, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcustto, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1  order by agentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcusttoname, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname", True)

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



                '----------------------------- Default Dates
                txttoDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")

                If Request.QueryString("todate") <> "" Then
                    txttoDate.Text = Format("U", CType(Request.QueryString("todate"), Date))
                End If

                'If Request.QueryString("fromcode") <> "" Then
                '    rbncustrange.Checked = True
                'Else
                '    rbncustrange.Checked = False
                'End If
                'If Request.QueryString("tocode") <> "" Then
                '    rbncustrange.Checked = True
                'Else
                '    rbncustrange.Checked = False
                'End If

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

                If Request.QueryString("orderby") <> "" Then
                    ddlrptord.SelectedIndex = Request.QueryString("orderby")
                End If

                If Request.QueryString("gpby") <> "" Then
                    ddlgpby.SelectedIndex = Request.QueryString("gpby")
                End If

                rbCatall.Attributes.Add("onclick", "rbevent(this,'" & rbCatrange.ClientID & "','A','Category')")
                rbCatrange.Attributes.Add("onclick", "rbevent(this,'" & rbCatall.ClientID & "','R','Category')")
                rbMarkall.Attributes.Add("onclick", "rbevent(this,'" & rbMarkrange.ClientID & "','A','Market')")
                rbMarkrange.Attributes.Add("onclick", "rbevent(this,'" & rbMarkall.ClientID & "','R','Market')")
                rbCtrall.Attributes.Add("onclick", "rbevent(this,'" & rbCtrrange.ClientID & "','A','Country')")
                rbCtrrange.Attributes.Add("onclick", "rbevent(this,'" & rbCtrall.ClientID & "','R','Country')")
                rbncustall.Attributes.Add("onclick", "rbevent(this,'" & rbncustrange.ClientID & "','A','Country')")
                rbncustrange.Attributes.Add("onclick", "rbevent(this,'" & rbncustall.ClientID & "','R','customer')")




                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

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
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RptBalanceExceedingCreditLimitWindowPostBack") Then

        End If

    End Sub

    Public Sub checkrb_status()
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

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            If ValidatePage() = False Then
                Exit Sub
            End If

            ViewState.Add("Pageame", "BalanceExceedingCreditLimitReport")
            ViewState.Add("BackPageName", "RptBalanceExceedingCreditLimit.aspx")



            Dim strReportTitle As String = ""
            Dim strtodate As String = ""

            Dim strfromcode As String = ""
            Dim strtocode As String = ""
            Dim strmarketcode As String = ""
            Dim strmarketcodeto As String = ""
            Dim strcatcode As String = ""
            Dim strcatcodeto As String = ""
            Dim strorderby As String = ""
            Dim strgpby As String = ""
            Dim strfromctry As String = ""
            Dim strtoctry As String = ""

            strtodate = Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strfromcode = IIf(UCase(ddlcustfrm.Items(ddlcustfrm.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlcustfrm.Items(ddlcustfrm.SelectedIndex).Text, "")
            strtocode = IIf(UCase(ddlcustto.Items(ddlcustto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlcustto.Items(ddlcustto.SelectedIndex).Text, "")

            strmarketcode = IIf(UCase(ddlFromMarket.Items(ddlFromMarket.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromMarket.Items(ddlFromMarket.SelectedIndex).Text, "")
            strmarketcodeto = IIf(UCase(ddlToMarket.Items(ddlToMarket.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToMarket.Items(ddlToMarket.SelectedIndex).Text, "")

            strcatcode = IIf(UCase(ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text, "")
            strcatcodeto = IIf(UCase(ddlToCategory.Items(ddlToCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCategory.Items(ddlToCategory.SelectedIndex).Text, "")

          
            strfromctry = IIf(UCase(ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text, "")
            strtoctry = IIf(UCase(ddlToCountry.Items(ddlToCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCountry.Items(ddlToCountry.SelectedIndex).Text, "")

            strorderby = ddlrptord.SelectedIndex
            strgpby = ddlgpby.SelectedIndex
            Dim strpop As String = ""
            strpop = "window.open('RptBalanceExceedingCreditLimitReport.aspx?type=C&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&orderby=" & strorderby & "&gpby=" & strgpby & "','BalanceExceedingCreditLimitReport','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptcustomertrialbalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptCustomerBalanceExc','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Public Function ValidatePage() As Boolean
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
End Class
