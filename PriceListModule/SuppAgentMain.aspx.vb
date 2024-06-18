Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Collections.Generic
Partial Class SuppAgentMain
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection
    Dim objUser As New clsUser
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim CurrCount As Integer
        Dim ctryCount As Integer
        Dim catCount As Integer

        Dim secCount As Integer
        Dim cityCount As Integer

        Dim CurrlngCount As Int16
        Dim CtrylngCount As Int16
        Dim seclngCount As Int16
        Dim catlngCount As Int16
        Dim citylngCount As Int16


        Dim strappname As String = ""
        Dim strTempUserFunctionalRight As String()
        Dim strsecTempUserFunctionalRight As String()
        Dim strctryTempUserFunctionalRight As String()
        Dim strcityTempUserFunctionalRight As String()
        Dim strcatTempUserFunctionalRight As String()
       

        Dim strcityRights As String
        Dim strctryRights As String
        Dim strsecRights As String
        Dim strcatRights As String

        Dim strCurrRights As String

        ViewState.Add("appid", Request.QueryString("appid"))
        Dim strappid As String = ViewState("appid")

        Dim groupid As String = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))

        Dim menuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\CurrenciesSearch.aspx?appid=" + strappid, strappid)
        Dim functionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, menuid)

        Dim ctrymenuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\CountriesSearch.aspx?appid=" + strappid, strappid)
        Dim ctryfunctionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, ctrymenuid)


        Dim catmenuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\SuppliercategoriesSearch.aspx?appid=" + strappid, strappid)
        Dim catfunctionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, catmenuid)

        Dim citymenuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\CitiesSearch.aspx?appid=" + strappid, strappid)
        Dim cityfunctionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, citymenuid)


        Dim secmenuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\SectorSearch.aspx?appid=" + strappid, strappid)
        Dim secfunctionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, secmenuid)

      
        If functionalrights <> "" Then
            strTempUserFunctionalRight = functionalrights.Split(";")
            For CurrlngCount = 0 To strTempUserFunctionalRight.Length - 1
                strCurrRights = strTempUserFunctionalRight.GetValue(CurrlngCount)

                If strCurrRights = "01" Then
                    CurrCount = 1
                End If
            Next
            If CurrCount = 1 Then
                btnCurrency.Visible = True
            Else
                btnCurrency.Visible = False
            End If
        End If

        If ctryfunctionalrights <> "" Then
            strctryTempUserFunctionalRight = ctryfunctionalrights.Split(";")
            For CtrylngCount = 0 To strctryTempUserFunctionalRight.Length - 1
                strctryRights = strctryTempUserFunctionalRight.GetValue(CtrylngCount)

                If strctryRights = "01" Then
                    ctryCount = 1
                End If
            Next
            If ctryCount = 1 Then
                btnCountry.Visible = True
            Else
                btnCountry.Visible = False
            End If
        End If
        If cityfunctionalrights <> "" Then
            strcityTempUserFunctionalRight = cityfunctionalrights.Split(";")
            For citylngCount = 0 To strcityTempUserFunctionalRight.Length - 1
                strcityRights = strcityTempUserFunctionalRight.GetValue(citylngCount)

                If strcityRights = "01" Then
                    cityCount = 1
                End If
            Next
            If cityCount = 1 Then
                btnCity.Visible = True
            Else
                btnCity.Visible = False
            End If
        End If
        If secfunctionalrights <> "" Then
            strsecTempUserFunctionalRight = secfunctionalrights.Split(";")
            For seclngCount = 0 To strsecTempUserFunctionalRight.Length - 1
                strsecRights = strsecTempUserFunctionalRight.GetValue(seclngCount)

                If strsecRights = "01" Then
                    secCount = 1
                End If
            Next
            If secCount = 1 Then
                btnSector.Visible = True
            Else
                btnSector.Visible = False
            End If
        End If

        If catfunctionalrights <> "" Then
            strcatTempUserFunctionalRight = catfunctionalrights.Split(";")
            For catlngCount = 0 To strcatTempUserFunctionalRight.Length - 1
                strcatRights = strcatTempUserFunctionalRight.GetValue(catlngCount)

                If strcatRights = "01" Then
                    catCount = 1
                End If
            Next
            If catCount = 1 Then
                btnCategory.Visible = True
            Else
                btnCategory.Visible = False
            End If
        End If


        Dim RefCode As String


        If IsPostBack = False Then
            'If Not Session("CompanyName") Is Nothing Then
            '    Me.Page.Title = CType(Session("CompanyName"), String)
            'End If

            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\SupplieragentsSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String),
                                                                 CType(Request.QueryString("appid"), Integer))
            '  ViewState.Add("CitiesValue", Request.QueryString("Value"))
            txtconnection.Value = Session("dbconnectionName")


            PanelMain.Visible = True
            charcters(txtSuppCode)
            charcters(txtSuppName)



            ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlType, "sptypecode", "sptypename", "select sptypecode, sptypename from sptypemast where active=1 order by sptypecode", True)
            ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlTName, "sptypename", "sptypecode", "select sptypename,sptypecode from sptypemast where active=1 order by sptypename", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 order by catcode", True)
            '  objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 order by catname", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSellingCode, "scatcode", "scatname", "select scatcode,scatname from sellcatmast where active=1 order by scatcode", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSellingName, "scatname", "scatcode", "select scatname,scatcode from sellcatmast where active=1 order by scatname", True)
            '  objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrCode, "currcode", "currname", "select currcode,currname from currmast where active=1  order by currcode", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrName, "currname", "currcode", "select currname,currcode from currmast where active=1  order by currname", True)
            ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlContCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1  order by ctrycode", True)
            ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcontName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1  order by ctryname", True)
            ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1  order by citycode", True)
            ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True)
            ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorCode, "sectorcode", "sectorname", "select sectorcode,sectorname from sectormaster where active=1  order by sectorcode", True)
            ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorName, "sectorname", "sectorcode", "select sectorname,sectorcode from sectormaster where active=1  order by sectorname", True)

            ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), cmbctrlcode, "acctcode", "acctname", "select acctcode,acctname from acctmast where upper(controlyn)='Y' and cust_supp='S'order by acctcode", True)
            ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), cmbctrlname, "acctname", "acctcode", "select  acctname,acctcode from acctmast where upper(controlyn)='Y'  and cust_supp='S'order by acctname", True)

            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), cmbaccrualcode, "acctcode", "acctname", "select acctcode,acctname from acctmast where upper(controlyn)='Y' and cust_supp='S'order by acctcode", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), cmbaccrualname, "acctname", "acctcode", "select  acctname,acctcode from acctmast where upper(controlyn)='Y'  and cust_supp='S'order by acctname", True)

            If CType(Session("SupagentsState"), String) = "New" Then
                SetFocus(txtSuppCode)
                lblHeading.Text = "Add New Supplier Agents" + " - " + PanelMain.GroupingText
                Page.Title = Page.Title + " " + "New Supplier Agents Master" + " - " + PanelMain.GroupingText
                btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")
            ElseIf CType(Session("SupagentsState"), String) = "Edit" Then

                btnSave.Text = "Update"

                RefCode = CType(Session("SupagentsRefCode"), String)
                ShowRecord(RefCode)
                txtSuppCode.Disabled = True
                txtSuppName.Disabled = True
                SetFocus(ddlType)
                lblHeading.Text = "Edit Supplier Agents" + " - " + PanelMain.GroupingText
                Page.Title = Page.Title + " " + "Edit Supplier Agents Master" + " - " + PanelMain.GroupingText
                DisableControl()

                btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save supplier agent?')==false)return false;")
            ElseIf CType(Session("SupagentsState"), String) = "View" Then

                RefCode = CType(Session("SupagentsRefCode"), String)
                ShowRecord(RefCode)
                txtSuppCode.Disabled = True
                txtSuppName.Disabled = True
                DisableControl()
                lblHeading.Text = "View Supplier Agents" + " - " + PanelMain.GroupingText
                Page.Title = Page.Title + " " + "View Supplier Agents Master" + " - " + PanelMain.GroupingText
                btnSave.Visible = False
                btnCancel.Text = "Return to Search"
                btnCancel.Focus()

            ElseIf CType(Session("SupagentsState"), String) = "Delete" Then

                RefCode = CType(Session("SupagentsRefCode"), String)
                ShowRecord(RefCode)
                txtSuppCode.Disabled = True
                txtSuppName.Disabled = True
                lblHeading.Text = "Delete Supplier Agents" + " - " + PanelMain.GroupingText
                Page.Title = Page.Title + " " + "Delete Supplier Agents Master" + " - " + PanelMain.GroupingText
                btnSave.Text = "Delete"
                btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete supplier agent?')==false)return false;")
                DisableControl()
            End If





            btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                'TextCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'TxtCategoryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'txtCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'TxtTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '  ddlSellingCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'ddlSellingName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'txtcountrycode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'txtcountryname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'txtsectorcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'txtsectorname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'TextCurrencyCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'TxtCurrencyName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'txtcitycode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'txtcityname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                'txtcontrolacccode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'txtcontrolaccname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'cmbaccrualcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'cmbaccrualname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            End If

            'Else

            '    Try


            '        If ddlType.Value <> "[Select]" Then
            '            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where sptypecode='" & TxtTypeName.Text & "'and  active=1 order by catcode", True, ddlCCode.Value)
            '            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where sptypecode='" & TxtTypeName.Text & "' and  active=1 order by catname", True, ddlCatName.Value)
            '            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSellingCode, "scatcode", "scatname", "select scatcode,scatname from sellcatmast where  sptypecode='" & ddlTName.Value & "' and active=1 order by scatcode", True, ddlSellingCode.Value)
            '            ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSellingName, "scatname", "scatcode", "select scatname,scatcode from sellcatmast where  sptypecode='" & ddlTName.Value & "' and active=1 order by scatname", True, ddlSellingName.Value)

            '        Else
            '            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 order by catcode", True)
            '            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 order by catname", True)
            '            ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSellingCode, "scatcode", "scatname", "select scatcode,scatname from sellcatmast where active=1 order by scatcode", True)
            '            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSellingName, "scatname", "scatcode", "select scatname,scatcode from sellcatmast where active=1 order by scatname", True)

            '        End If

            '        If ddlContCode.Value <> "[Select]" Then
            '            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where ctrycode='" & ddlcontName.Value & "' and active=1  order by citycode", True, ddlCityCode.Value)
            '            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where  ctrycode='" & ddlcontName.Value & "' andactive=1  order by cityname", True, ddlCityName.Value)

            '        Else
            '            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1  order by citycode", True)
            '            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1  order by cityname", True)
            '        End If

            '        If ddlCityCode.Value <> "[Select]" Then
            '            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorCode, "sectorcode", "sectorname", "select sectorcode,sectorname from sectormaster where citycode='" & ddlCityName.Value & "' and active=1  order by sectorcode", True, ddlSectorCode.Value)
            '            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorName, "sectorname", "sectorcode", "select sectorname,sectorcode from sectormaster where  citycode='" & ddlCityName.Value & "' and active=1  order by sectorname", True, ddlSectorName.Value)
            '        Else
            '            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorCode, "sectorcode", "sectorname", "select sectorcode,sectorname from sectormaster where active=1  order by sectorcode", True)
            '            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorName, "sectorname", "sectorcode", "select sectorname,sectorcode from sectormaster where active=1  order by sectorname", True)
            '        End If


            '    Catch ex As Exception
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            '    End Try
        End If
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "SupcatsfromWindowPostBack") Then
            If Session("addcategory") = "new" Then
                If Session("SupCategoryName") IsNot Nothing Then
                    If Session("SupCategoryCode") IsNot Nothing Then
                        Dim categoryname As String = Session("SupCategoryName")
                        TextCode.Text = Session("SupCategoryCode")
                        TxtCategoryName.Text = categoryname
                        Session.Remove("addcategory")
                        Session.Remove("SupCategoryName")
                        Session.Remove("SupCategoryCode")
                    End If
                End If
                If Session("CurrName") IsNot Nothing Then
                    If Session("CurrCode") IsNot Nothing Then
                        Dim currencyname As String = Session("CurrName")
                        TextCurrencyCode.Text = Session("CurrCode")
                        TxtCurrencyName.Text = currencyname
                        Session.Remove("addcategory")
                        Session.Remove("CurrName")
                        Session.Remove("CurrCode")
                    End If
                End If
                If Session("CitiesName") IsNot Nothing Then
                    If Session("CitiesCode") IsNot Nothing Then
                        Dim cityname As String = Session("CitiesName")
                        txtcitycode.Text = Session("CitiesCode")
                        txtcityname.Text = cityname
                        Session.Remove("addcategory")
                        Session.Remove("CitiesName")
                        Session.Remove("CitiesCode")
                    End If
                End If

                If Session("SectorName") IsNot Nothing Then
                    If Session("SectorCode") IsNot Nothing Then
                        Dim sectorname As String = Session("SectorName")
                        txtsectorname.Text = sectorname
                        txtsectorcode.Text = Session("SectorCode")
                        Session.Remove("addcategory")
                        Session.Remove("SectorName")
                        Session.Remove("SectorCode")
                    End If
                End If



            End If
        End If
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CtryWindowPostBack") Then
            If Session("addcategory") = "new" Then
                If Session("CountryName") IsNot Nothing Then
                    If Session("CountryCode") IsNot Nothing Then
                        Dim countryname As String = Session("CountryName")
                        txtcountrycode.Text = Session("CountryCode")
                        txtcountryname.Text = countryname
                        Session.Remove("addcategory")
                        Session.Remove("CountryName")
                        Session.Remove("CountryCode")
                    End If
                End If
            End If
        End If

        Session.Add("submenuuser", "SupplierAgentsSearch.aspx")
        Page.Title = "SupplierAgents Entry"
    End Sub
#End Region
#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('SupagentsWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#End Region
#Region " Private Function ValidateMainDetails() As Boolean"
    Private Function ValidateMainDetails() As Boolean
        ' Dim lbl As Label
        'Dim str As String
        Try
            If txtSuppCode.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Code field can not be blank.');", True)
                'ScriptManagerProxy1.FindControl("txtSuppCode").Focus()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtSuppCode.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If txtSuppName.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name field can not be blank.');", True)
                ' ScriptManagerProxy1.FindControl("txtSuppName").Focus()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtSuppName.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If TxtTypeName.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Supplier Type Name.');", True)
                'SetFocus(ddlSector)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + TxtTypeName.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If TxtCategoryName.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Caregory.');", True)
                'ScriptManagerProxy1.FindControl("ddlTypeCode").Focus()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + TxtCategoryName.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If TxtCurrencyName.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Currency.');", True)
                '    ScriptManagerProxy1.FindControl("ddlCategory").Focus()
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + TxtCurrencyName.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If txtcountryname.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Country.');", True)
                'SetFocus(ddlSelling)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtcountryname.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If txtcityname.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select City.');", True)
                'SetFocus(ddlCurrency)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtcityname.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If txtsectorname.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select sector.');", True)
                'SetFocus(ddlCountry)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtsectorname.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If
            If txtcontrolaccname.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select control A/C.');", True)
                'SetFocus(ddlCity)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtcontrolaccname.ClientID + "');", True)
                ValidateMainDetails = False
                Exit Function
            End If

            'If cmbctrlcode.Value = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Control A/C Code.');", True)
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + cmbctrlcode.ClientID + "');", True)
            '    Exit Function
            'End If

            'If cmbaccrualcode.Value = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Accrual A/C Code.');", True)
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + cmbaccrualcode.ClientID + "');", True)
            '    Exit Function
            'End If

            ValidateMainDetails = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function
#End Region
#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If Session("SupagentsState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "view_account", "code", CType(txtSuppCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This  code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "view_account", "des", txtSuppName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This  name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf Session("SupagentsState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "view_account", "code", "des", txtSuppName.Value.Trim, CType(txtSuppCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This  name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                If Session("SupagentsState") = "New" Or Session("SupagentsState") = "Edit" Then
                    If ValidateMainDetails() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    If Session("SupagentsState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_supagents", mySqlConn, sqlTrans)
                    ElseIf Session("SupagentsState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_supagents", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(txtSuppCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@supagentname", SqlDbType.VarChar, 200)).Value = CType(txtSuppName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@catcode", SqlDbType.VarChar, 20)).Value = CType(TextCode.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@scatcode", SqlDbType.VarChar, 20)).Value = "" ' CType((ddlSellingCode.Items(ddlSellingCode.SelectedIndex).Text), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType(txtcountrycode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = CType(txtcitycode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(TextCurrencyCode.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = CType(txtsectorcode.Text, String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If txtcontrolacccode.Text <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@controlacctcode", SqlDbType.VarChar, 20)).Value = CType(txtcontrolacccode.Text.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@controlacctcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If
                    'If cmbaccrualcode.Value <> "[Select]" Then
                    ' mySqlCmd.Parameters.Add(New SqlParameter("@accrualacctcode", SqlDbType.VarChar, 20)).Value = CType(cmbaccrualcode.Items(cmbaccrualcode.SelectedIndex).Text.Trim, String)
                    'Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@accrualacctcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    'End If
                    mySqlCmd.ExecuteNonQuery()
                ElseIf Session("SupagentsState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("sp_del_supagents", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@supagentcode", SqlDbType.VarChar, 20)).Value = CType(txtSuppCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("delete from supagents_multiemail where supagentcode='" & txtSuppCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                '  Session.Add("SessionFirstCheck", "Edit")
                If Session("SupagentsState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                    Session.Add("SupagentsState", "Edit")
                    Session.Add("SupagentsRefCode", CType(txtSuppCode.Value.Trim, String))
                    txtCode.Text = txtSuppCode.Value
                ElseIf Session("SupagentsState") = "Edit" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                    Session.Add("SupagentsState", "Edit")
                    txtCode.Text = txtSuppCode.Value
                End If
                If Session("SupagentsState") = "Delete" Then
                    'Response.Redirect("SupplierAgentsSearch.aspx", False)
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('SupagentsWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                End If
                Session("type") = ""
                Session("SuppAgentMain_ctryname_for_filter") = ""
                Session("SuppAgentMain_cityname_for_filter") = ""

                ' Response.Redirect("SupplierAgents.aspx", False)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

#End Region
#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        If CType(Session("SupagentsState"), String) = "View" Then
            Me.txtSuppCode.Disabled = True
            Me.txtSuppName.Disabled = True

            TxtCategoryName.Enabled = False

            TextCode.Enabled = False




            TxtCurrencyName.Enabled = False

            TextCurrencyCode.Enabled = False

            txtcountryname.Enabled = False
            txtcountrycode.Enabled = False

            txtcityname.Enabled = False
            txtcitycode.Enabled = False



            txtsectorname.Enabled = False
            txtsectorcode.Enabled = False

            txtcontrolaccname.Enabled = False
            txtcontrolacccode.Enabled = False


            TxtTypeName.Enabled = False
            txtCode.Enabled = False






            ddlCCode.Disabled = True
            ddlCatName.Disabled = True
            ' ddlSellingCode.Disabled = True
            'ddlSellingName.Disabled = True
            ddlCurrCode.Disabled = True
            ddlCurrName.Disabled = True
            ddlContCode.Disabled = True
            ddlcontName.Disabled = True
            ddlCityCode.Disabled = True
            ddlCityName.Disabled = True
            ddlSectorCode.Disabled = True
            ddlSectorName.Disabled = True
            chkActive.Disabled = True
            cmbctrlcode.Disabled = True
            cmbctrlname.Disabled = True
            'cmbaccrualcode.Disabled = True
            'cmbaccrualname.Disabled = True

            '------------------------------------------------------
        ElseIf CType(Session("SupagentsState"), String) = "Edit" Then
            If checkForAccountExisting() = False Then
                ddlCurrCode.Disabled = True
                ddlCurrName.Disabled = True
            End If
        End If
    End Sub
#End Region
#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from supplier_agents Where supagentcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("supagentcode")) = False Then
                        Me.txtSuppCode.Value = mySqlReader("supagentcode")
                        Me.txtCode.Text = txtSuppCode.Value
                    End If
                    If IsDBNull(mySqlReader("supagentname")) = False Then
                        Me.txtSuppName.Value = mySqlReader("supagentname")
                    End If

                    If IsDBNull(mySqlReader("sptypecode")) = False Then
                        txtCode.Text = mySqlReader("sptypecode")
                        TxtTypeName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", mySqlReader("sptypecode"))
                    End If

                    ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "catcode", "catname", "select catcode,catname from catmast where active=1 and sptypecode='" & mySqlReader("sptypecode") & "'  order by catcode", True)
                    ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCatName, "catname", "catcode", "select catname,catcode from catmast where active=1 and sptypecode='" & mySqlReader("sptypecode") & "' order by catname", True)

                    ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSellingCode, "scatcode", "scatname", "select scatcode,scatname from sellcatmast where active=1  and sptypecode='" & mySqlReader("sptypecode") & "' order by scatcode", True)
                    ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSellingName, "scatname", "scatcode", "select scatname,scatcode from sellcatmast where active=1  and sptypecode='" & mySqlReader("sptypecode") & "' order by scatname", True)
                    If IsDBNull(mySqlReader("catcode")) = False Then
                        TextCode.Text = mySqlReader("catcode")
                        TxtCategoryName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "catmast", "catname", "catcode", mySqlReader("catcode"))

                    End If





                    If IsDBNull(mySqlReader("scatcode")) = False Then
                        ' ddlSellingName.Value = mySqlReader("scatcode")
                        'ddlSellingCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sellcatmast", "scatname", "scatcode", mySqlReader("scatcode"))
                    End If
                    If IsDBNull(mySqlReader("currcode")) = False Then
                        TextCurrencyCode.Text = mySqlReader("currcode")
                        TxtCurrencyName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", mySqlReader("currcode"))
                    End If

                    If IsDBNull(mySqlReader("ctrycode")) = False Then
                        txtcountrycode.Text = mySqlReader("ctrycode")
                        txtcountryname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", mySqlReader("ctrycode"))
                    End If

                    'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname from citymast where active=1 and ctrycode='" & mySqlReader("ctrycode") & "' order by citycode", True)
                    'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode from citymast where active=1 and ctrycode='" & mySqlReader("ctrycode") & "'  order by cityname", True)

                    'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorCode, "sectorcode", "sectorname", "select sectorcode,sectorname from sectormaster where active=1 and ctrycode='" & mySqlReader("ctrycode") & "' order by sectorcode", True)
                    'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorName, "sectorname", "sectorcode", "select sectorname,sectorcode from sectormaster where active=1 and ctrycode='" & mySqlReader("ctrycode") & "'  order by sectorname", True)



                    If IsDBNull(mySqlReader("citycode")) = False Then
                        txtcitycode.Text = mySqlReader("citycode")
                        txtcityname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "citymast", "cityname", "citycode", mySqlReader("citycode"))
                    End If


                    ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorCode, "sectorcode", "sectorname", "select sectorcode,sectorname from sectormaster where active=1 and ctrycode='" & mySqlReader("ctrycode") & "' and citycode= '" & mySqlReader("citycode") & "' order by sectorcode", True)
                    ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorName, "sectorname", "sectorcode", "select sectorname,sectorcode from sectormaster where active=1 and ctrycode='" & mySqlReader("ctrycode") & "' and  citycode='" & mySqlReader("citycode") & "'  order by sectorname", True)

                    If IsDBNull(mySqlReader("sectorcode")) = False Then
                        txtsectorcode.Text = mySqlReader("sectorcode")
                        txtsectorname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sectormaster", "sectorname", "sectorcode", mySqlReader("sectorcode"))
                    End If

                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If
                    If IsDBNull(mySqlReader("controlacctcode")) = False Then
                        txtcontrolacccode.Text = mySqlReader("controlacctcode")
                        txtcontrolaccname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("controlacctcode"))
                    Else
                        txtcontrolaccname.Text = ""
                        txtcontrolacccode.Text = ""
                    End If
                    'If IsDBNull(mySqlReader("accrualacctcode")) = False Then
                    '    cmbaccrualname.Value = mySqlReader("accrualacctcode")
                    '    cmbaccrualcode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("accrualacctcode"))
                    'Else
                    'cmbctrlname.Value = "[Select]"
                    'cmbctrlcode.Value = "[Select]"
                    'End If

                End If
               
                Session("type") = txtCode.Text
                Session("SuppAgentMain_ctryname_for_filter") = txtcountrycode.Text
                Session("SuppAgentMain_cityname_for_filter") = txtcitycode.Text



            End If


            mySqlCmd.Dispose()
            mySqlReader.Close()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierAgents.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try


       



    End Sub
#End Region
#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region
#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region
#Region "telepphone"
    Public Sub telepphone(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkTelephoneNumber(event)")
    End Sub
#End Region
#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "blocksale_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a BlockFullOfSales, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cancel_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a CancellationPolicy, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "child_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a ChildPolicy, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "compulsory_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a CompulsoryRemarks, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cplisthnew", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a PriceList, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "earlypromotion_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a EarliBirdPromotion, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "minnights_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a MinimumNights, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplist_costh", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a OtherServiceCostPriceList, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "promotion_header", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a Promotions, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sellsph", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a SellingFormulaForSupplier, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sparty_policy", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a GeneralPolicy, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "spleventplisth", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a  SpecialEvents/Extras, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "tktplisthnew", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a  TicketingpriceList, cannot delete this SupplierAgent');", True)
            checkForDeletion = False
            Exit Function
            ''check the accounts
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "receipt_detail", "receipt_acc_code", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a accounts transaction, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "journal_detail", "journal_acc_code", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a accounts transaction, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "openparty_master", "open_code", CType(txtSuppCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for a accounts Opening balance transaction, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function

 ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "edit_contracts", "supagentcode", CType(txtSuppCode.Value.Trim, String)) = True Then
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierAgent is already used for edit_Contracts, cannot delete this SupplierAgent');", True)
        checkForDeletion = False
        Exit Function

        End If
        checkForDeletion = True
    End Function
#End Region
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SuppAgentMain','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Private Function checkForAccountExisting() As Boolean
        'GetDBFieldValueExist
        Dim strValue As String = ""
        If Session("SupagentsState") = "Edit" Then
            strValue = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select top 1 't' from acc_tran where acc_type='A' and acc_code='" & Trim(txtSuppCode.Value) & "'")
            If strValue <> "" Then
                checkForAccountExisting = False
                Exit Function
            End If
            strValue = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select top 1 't' from view_unpost_acccode where acc_type='A' and acc_code='" & Trim(txtSuppCode.Value) & "'")
            If strValue <> "" Then
                checkForAccountExisting = False
                Exit Function
            End If
        End If

        checkForAccountExisting = True
    End Function
    Protected Sub txtcontrolaccnameName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtcontrolaccname.TextChanged
        Session("SuppAgentMain_acctname_for_filter") = txtcountrycode.Text()
    End Sub
    Protected Sub TxtCityName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtcityname.TextChanged
        Session("SuppAgentMain_cityname_for_filter") = txtcitycode.Text
    End Sub
    Protected Sub TxtSectorName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtsectorname.TextChanged
        Session("SuppAgentMain_sectorname_for_filter") = txtcountrycode.Text()
    End Sub
    Protected Sub TxtCountryName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtcountryname.TextChanged
        Session("SuppAgentMain_ctryname_for_filter") = txtcountrycode.Text
    End Sub
    Protected Sub TxtCurrencyName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtCurrencyName.TextChanged
        Session("SuppAgentMain_currname_for_filter") = TextCurrencyCode.Text()
    End Sub
    Protected Sub TxtTypeName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtTypeName.TextChanged
        Session("type") = txtCode.Text
    End Sub
    Protected Sub TxtCategoryName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtCategoryName.TextChanged
        Session("SuppAgentMain_catname_for_filter") = TextCode.Text()
    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
          <System.Web.Services.WebMethod()> _
    Public Shared Function GetTypeName(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim TypeName As New List(Of String)
        Try
            strSqlQry = "select sptypename,sptypecode from sptypemast where sptypename like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    TypeName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("sptypename").ToString(), myDS.Tables(0).Rows(i)("sptypecode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return TypeName
        Catch ex As Exception
            Return TypeName
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
      <System.Web.Services.WebMethod()> _
    Public Shared Function GetCategoryName(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim sptyp As String = HttpContext.Current.Session("type")
        Dim myDS As New DataSet
        Dim TypeName As New List(Of String)
        Try
            strSqlQry = "select catname,catcode from catmast where active=1"
            If Trim(sptyp) <> "" Then
                strSqlQry = strSqlQry + " and sptypecode='" & Trim(sptyp) & "'"
            End If
            strSqlQry = strSqlQry + " and catname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    TypeName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("catname").ToString(), myDS.Tables(0).Rows(i)("catcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return TypeName
        Catch ex As Exception
            Return TypeName
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
      <System.Web.Services.WebMethod()> _
    Public Shared Function Getcountrylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim CurrencyName As New List(Of String)
        Try
            strSqlQry = "select ctryname,ctrycode from ctrymast where ctryname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    CurrencyName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("ctryname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return CurrencyName
        Catch ex As Exception
            Return CurrencyName
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
      <System.Web.Services.WebMethod()> _
    Public Shared Function GetCurrencyName(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim CurrencyName As New List(Of String)
        Try
            strSqlQry = "select currname,currcode from currmast where currname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    CurrencyName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("currname").ToString(), myDS.Tables(0).Rows(i)("currcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return CurrencyName
        Catch ex As Exception
            Return CurrencyName
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function Getsectorlist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim ctry, city As String
        Dim CurrencyName As New List(Of String)
        Try
            If HttpContext.Current.Session("SuppAgentMain_ctryname_for_filter") IsNot Nothing Then
                ctry = HttpContext.Current.Session("SuppAgentMain_ctryname_for_filter")
            End If
            If HttpContext.Current.Session("SuppAgentMain_cityname_for_filter") IsNot Nothing Then
                city = HttpContext.Current.Session("SuppAgentMain_cityname_for_filter")
            End If
            strSqlQry = "select sectorname,sectorcode from sectormaster  where  active=1"
            If Trim(ctry) <> "" Then
                strSqlQry = strSqlQry + " and ctrycode='" & Trim(ctry) & "'"
            End If
            If Trim(city) <> "" Then
                strSqlQry = strSqlQry + " and citycode='" & Trim(city) & "'"
            End If
            strSqlQry = strSqlQry + " and sectorname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    CurrencyName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("sectorname").ToString(), myDS.Tables(0).Rows(i)("sectorcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return CurrencyName
        Catch ex As Exception
            Return CurrencyName
        End Try

    End Function
    Protected Sub btnCategory_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strpop As String = ""
        strpop = "window.open('SupplierCategories.aspx?State=New&Value=Addfrom','Supcats');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    Protected Sub btnCountry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCountry.Click
        Dim strpop As String = ""
        strpop = "window.open('Countries.aspx?State=New&Value=Addfrom','Country');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    Protected Sub btnCurrency_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrency.Click
        Dim strpop As String = ""
        strpop = "window.open('Currencies.aspx?State=New&Value=Addfrom','Currency');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    Protected Sub btnCity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCity.Click
        Dim strpop As String = ""
        strpop = "window.open('Cities.aspx?State=New&Value=Addfrom','City');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    Protected Sub btnSector_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSector.Click
        Dim strpop As String = ""
        strpop = "window.open('Sector.aspx?State=New&Value=Addfrom','Sector');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function Getcitylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim ctry As String = ""
        Dim CurrencyName As New List(Of String)
        Try
            If HttpContext.Current.Session("SuppAgentMain_ctryname_for_filter") IsNot Nothing Then
                ctry = HttpContext.Current.Session("SuppAgentMain_ctryname_for_filter")
            End If
            strSqlQry = "select cityname,citycode from citymast where active=1"
            If Trim(ctry) <> "" Then
                strSqlQry = strSqlQry + " and ctrycode='" & Trim(ctry) & "'"
            End If
            strSqlQry = strSqlQry + " and cityname like  '" & Trim(prefixText) & "%'"

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    CurrencyName.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("cityname").ToString(), myDS.Tables(0).Rows(i)("citycode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return CurrencyName
        Catch ex As Exception
            Return CurrencyName
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
        <System.Web.Services.WebMethod()> _
    Public Shared Function Getcontrolacclist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Controlaccnames As New List(Of String)
        Try
            strSqlQry = "select acctname,acctcode from acctmast where upper(controlyn)='Y'  and cust_supp='S'  and  acctname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Controlaccnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Controlaccnames
        Catch ex As Exception
            Return Controlaccnames
        End Try
    End Function
End Class
'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlTypeCode, "sptypecode", "select distinct sptypecode from sptypemast where active=1 order by sptypecode", True)
'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCurrency, "currcode", "select distinct currcode from currmast where active=1  order by currcode", True)
'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCountry, "ctrycode", "select distinct ctrycode from ctrymast where active=1  order by ctrycode", True)
' objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlTypeCode, "sptypecode", "select * from sptypemast where active=1 order by sptypecode", True)



'If IsDBNull(mySqlReader("sptypecode")) = False Then
'    ddlTypeCode.SelectedValue = mySqlReader("sptypecode")
'    txtTypeName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sptypemast", "sptypename", "sptypecode", ddlTypeCode.SelectedValue)


'End If
''objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlCategory, "catcode", "catname", "select * from catmast where active=1  and sptypecode='" & ddlTypeCode.SelectedItem.Text & "' order by catcode", True)
''objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlSelling, "scatcode", "scatname", "select * from sellcatmast where active=1  and sptypecode='" & ddlTypeCode.SelectedItem.Text & "' order by scatcode", True)
'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCategory, "catcode", "select distinct catcode from catmast where active=1  and sptypecode='" & ddlTypeCode.SelectedValue & "' order by catcode", True)
'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSelling, "scatcode", "select distinct scatcode from sellcatmast where active=1  and sptypecode='" & ddlTypeCode.SelectedValue & "' order by scatcode", True)

''---------- Main Details    -------------------------
'If IsDBNull(mySqlReader("catcode")) = False Then

'    ddlCategory.SelectedValue = mySqlReader("catcode")
'    txtCategoryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"catmast", "catname", "catcode", ddlCategory.SelectedValue)
'End If
'If IsDBNull(mySqlReader("scatcode")) = False Then
'    ddlSelling.SelectedValue = mySqlReader("scatcode")
'    txtSellingName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sellcatmast", "scatname", "scatcode", ddlSelling.SelectedValue)
'End If
'If IsDBNull(mySqlReader("currcode")) = False Then
'    ddlCurrency.SelectedValue = mySqlReader("currcode")
'    txtCurrencyName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"currmast", "currname", "currcode", ddlCurrency.SelectedValue)
'End If
'If IsDBNull(mySqlReader("ctrycode")) = False Then
'    ddlCountry.SelectedValue = mySqlReader("ctrycode")
'    txtCountryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", ddlCountry.SelectedValue)
'End If
'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCity, "citycode", "select distinct citycode from citymast where active=1  and ctrycode='" & ddlCountry.SelectedValue & "' order by citycode", True)
'If IsDBNull(mySqlReader("citycode")) = False Then
'    ddlCity.SelectedValue = mySqlReader("citycode")
'    txtCityName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"citymast", "cityname", "citycode", ddlCity.SelectedItem.Text)
'End If
'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSector, "sectorcode", "select distinct sectorcode from sectormaster where active=1  and citycode='" & ddlCity.SelectedValue & "' order by sectorcode", True)
'If IsDBNull(mySqlReader("sectorcode")) = False Then
'    ddlSector.SelectedValue = mySqlReader("sectorcode")
'    txtSectorName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sectormaster", "sectorname", "sectorcode", ddlSector.SelectedValue)
'End If



'#Region "Protected Sub ddlTypeCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
'    Protected Sub ddlTypeCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
'        txtTypeName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sptypemast", "sptypename", "sptypecode", ddlTypeCode.SelectedValue)
'        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCategory, "catcode", "select * from catmast where active=1  and sptypecode='" & ddlTypeCode.SelectedValue & "' order by catcode", True)
'        txtCategoryName.Value = ""
'        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSelling, "scatcode", "select * from sellcatmast where active=1  and sptypecode='" & ddlTypeCode.SelectedValue & "' order by scatcode", True)
'        txtSellingName.Value = ""
'    End Sub
'#End Region

'#Region "Protected Sub ddlCity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
'    Protected Sub ddlCity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
'        txtCityName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"citymast", "cityname", "citycode", ddlCity.SelectedItem.Text)
'        objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSector, "sectorcode", "select * from sectormaster where active=1  and citycode='" & ddlCity.SelectedValue & "' order by sectorcode", True)
'        txtSectorName.Value = ""
'    End Sub
'#End Region


'Protected Sub ddlSector_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSector.SelectedIndexChanged
'    txtSectorName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sectormaster", "sectorname", "sectorcode", ddlSector.SelectedValue)
'End Sub


'Protected Sub ddlCategory_SelectedIndexChanged2(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCategory.SelectedIndexChanged
'    If ddlCategory.SelectedValue = "[Select]" Then
'        txtCategoryName.Value = ""
'    Else
'        txtCategoryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"catmast", "catname", "catcode", ddlCategory.SelectedValue)
'    End If
'End Sub

'Protected Sub ddlSelling_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSelling.SelectedIndexChanged
'    txtSellingName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sellcatmast", "scatname", "scatcode", ddlSelling.SelectedValue)
'End Sub
'Protected Sub ddlCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrency.SelectedIndexChanged
'    txtCurrencyName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"currmast", "currname", "currcode", ddlCurrency.SelectedValue)
'End Sub

'Protected Sub ddlCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
'    txtCountryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", ddlCountry.SelectedValue)
'    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCity, "citycode", "select distinct citycode from citymast where active=1  and ctrycode='" & ddlCountry.SelectedValue & "' order by citycode", True)
'    objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSector, "sectorcode", "select distinct sectorcode from sectormaster where active=1  and citycode='" & ddlCity.SelectedValue & "' order by sectorcode", True)
'    txtCityName.Value = ""
'    txtSectorName.Value = ""
'End Sub