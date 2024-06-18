'------------================--------------=======================------------------================
'   Module Name    :    RptSupplierLedger.aspx
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
Imports System.Globalization
#End Region

Partial Class RptSupplierLedger
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDate As New clsDateTime
    Dim strappid As String = ""
    Dim strappname As String = ""
    Shared divcode As String = ""
    Shared supptype As String = ""
#End Region
    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function Getbankslist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim acctnames As New List(Of String)
        Dim divid As String = ""
        Try

            strSqlQry = "select acctname,acctcode from acctmast where controlyn='Y'and div_code='" & divcode & "' and acctname like  '" & Trim(prefixText) & "%' order by acctcode"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    acctnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString()))

                Next
            End If
            Return acctnames
        Catch ex As Exception
            Return acctnames
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function Getsuppliertypelist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppliertypenames As New List(Of String)
        Dim divid As String = ""
        Try

            strSqlQry = "select sptypecode,sptypename from sptypemast where active=1 and sptypename like  '" & Trim(prefixText) & "%'  order by sptypecode "

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    suppliertypenames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("sptypename").ToString(), myDS.Tables(0).Rows(i)("sptypecode").ToString()))

                Next
            End If
            Return suppliertypenames
        Catch ex As Exception
            Return suppliertypenames
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function Getsupplierlist(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppliernames As New List(Of String)
        Dim city, ctry, custagent, type As String
        ctry = ""
        city = ""

        custagent = ""
        type = ""

        Try

            If contextKey.ToLower = "true" Then
                contextKey = ""
            Else
                If contextKey <> "" Then
                    ctry = contextKey.Trim.Split("||")(0)

                    city = contextKey.Trim.Split("||")(2)

                    custagent = contextKey.Trim.Split("||")(4)

                    type = contextKey.Trim.Split("||")(6)
                End If
            End If

            If supptype = "S" Then
                strSqlQry = "select partyname,partycode from partymast where partyname like  '" & Trim(prefixText) & "%'  "
                If ctry <> "" Then
                    strSqlQry = strSqlQry & " and ctrycode='" & ctry & "'"
                End If
                If city <> "" Then
                    strSqlQry = strSqlQry & " and citycode='" & city & "'"
                End If
                If custagent <> "" Then
                    strSqlQry = strSqlQry & " and catcode='" & custagent & "'"
                End If

                If type <> "" Then
                    strSqlQry = strSqlQry & " and sptypecode='" & type & "'"
                End If
                strSqlQry = strSqlQry & "  order by partycode"

            ElseIf supptype = "A" Then
                strSqlQry = "select supagentcode as partycode, supagentname as partyname from supplier_agents where active=1 and supagentname like  '" & Trim(prefixText) & "%'  "

                If ctry <> "" Then
                    strSqlQry = strSqlQry & " and ctrycode='" & ctry & "'"
                End If
                If city <> "" Then
                    strSqlQry = strSqlQry & " and citycode='" & city & "'"
                End If
                If custagent <> "" Then
                    strSqlQry = strSqlQry & " and catcode='" & custagent & "'"
                End If

                If type <> "" Then
                    strSqlQry = strSqlQry & " and sptypecode='" & type & "'"
                End If
                strSqlQry = strSqlQry & "  order by supagentcode"
            End If







            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    suppliernames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))

                Next
            End If
            Return suppliernames
        Catch ex As Exception
            Return suppliernames
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getcategorylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim categorynames As New List(Of String)
        Dim divid As String = ""
        Try

            strSqlQry = "select catcode,catname from catmast where active=1 and  catname like  '" & Trim(prefixText) & "%'order by catcode "
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    categorynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("catname").ToString(), myDS.Tables(0).Rows(i)("catcode").ToString()))

                Next
            End If
            Return categorynames
        Catch ex As Exception
            Return categorynames
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
 <System.Web.Services.WebMethod()> _
    Public Shared Function Getcitylist(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim mrktname As New List(Of String)
        Try
            If contextKey = "True" Then
                contextKey = ""
            End If
            If contextKey = "" Then
                strSqlQry = "select cityname,citycode from citymast where cityname like  '" & Trim(prefixText) & "%' order by cityname"
            Else
                strSqlQry = "select cityname,citycode from citymast where ctrycode='" & contextKey & "' and cityname like  '" & Trim(prefixText) & "%' order by cityname"
            End If

            'strSqlQry = "select plgrpname,plgrpcode from plgrpmast where plgrpname like  '" & Trim(prefixText) & "%' order by plgrpname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    mrktname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("cityname").ToString(), myDS.Tables(0).Rows(i)("citycode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return mrktname
        Catch ex As Exception
            Return mrktname
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getctrylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim ctrynames As New List(Of String)
        Dim divid As String = ""
        Try
            strSqlQry = "select ctrycode , ctryname from ctrymast where active=1 and  ctryname like  '" & Trim(prefixText) & "%' order by ctrycode"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    ctrynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("ctryname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))

                Next
            End If
            Return ctrynames
        Catch ex As Exception
            Return ctrynames
        End Try

    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
            Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
            Dim appidnew As String = CType(Request.QueryString("appid"), String)
            strappid = appidnew

            If appidnew Is Nothing = False Then
                '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
                strappname = Session("AppName")
                '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            End If

            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else
                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                   CType(strappname, String), "AccountsModule\RptSupplierLedger.aspx?appid=" + strappid, btnadd, Button1, btnReport, gv_SearchResult)
            End If
            '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
            '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            ViewState.Add("divcode", divid)
            divcode = ViewState("divcode")
            supptype = "S"
            If IsPostBack = False Then
                txtconnection.Value = Session("dbconnectionName")
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                SetFocus(ddlsuppliertype)

                Dim strfromctrlcode1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select options_available from reservation_parameters where param_id=538")
                Dim strfromctrlname1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_value from reservation_parameters where param_id=538")

                Dim strtoctrlcode1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select options_available from reservation_parameters where param_id=539")
                Dim strtoctrlname1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_value from reservation_parameters where param_id=539")
                txtsuppliercode.Attributes.Add("readonly", "readonly")
                TxtBankCode.Attributes.Add("readonly", "readonly")

                txtcatcode.Attributes.Add("readonly", "readonly")
                txtcitycode.Attributes.Add("readonly", "readonly")
                txtctrycode.Attributes.Add("readonly", "readonly")


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrency, "currcode", "currcode", " select 'Party Currency' as currcode union select option_selected as currcode from reservation_parameters where param_id=457", False)

                '----------------------------- Default Dates
                txtFromDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                txtToDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                ' txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")

                '---------------------

                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format("U", CType(Request.QueryString("fromdate"), Date))
                End If
                If Request.QueryString("todate") <> "" Then
                    ' todate = Request.QueryString("todate")
                    txtToDate.Text = Format("U", CType(Request.QueryString("todate"), Date))
                End If


                If Request.QueryString("acctype") <> "" Then
                    ddlsuppliertype.SelectedValue = Request.QueryString("acctype")
                End If


                If Request.QueryString("fromcode") <> "" Then
                    'rbSuprange.Checked = True
                    'rbSupall.Checked = False
                    TxtBankName.Text = Request.QueryString("fromcode")
                    'ddlFromSupplierName.Value = Request.QueryString("fromcode")
                    'ddlFromSupplier.Value = ddlFromSupplierName.Items(ddlFromSupplierName.SelectedIndex).Text
                    'Else
                    '    rbSuprange.Checked = False
                    '    rbSupall.Checked = True
                End If
                If Request.QueryString("tocode") <> "" Then
                    'rbSuprange.Checked = True
                    'rbSupall.Checked = False
                    TxtBankName.Text = Request.QueryString("tocode")
                    'ddlToSupplierName.Value = Request.QueryString("tocode")
                    'ddlToSupplier.Value = ddlToSupplierName.Items(ddlToSupplierName.SelectedIndex).Text
                    ''Else
                    '    rbSuprange.Checked = False
                    '    rbSupall.Checked = True
                End If

                If Request.QueryString("fromcat") <> "" Then
                    'rbCatrange.Checked = True
                    'rbCatall.Checked = False
                    txtcatname.Text = Request.QueryString("fromcat")
                    'ddlFromCategoryName.Value = Request.QueryString("fromcat")
                    'ddlFromCategory.Value = ddlFromCategoryName.Items(ddlFromCategoryName.SelectedIndex).Text
                    ''Else
                    '    rbCatrange.Checked = False
                    '    rbCatall.Checked = True
                End If

                If Request.QueryString("tocat") <> "" Then
                    'rbCatrange.Checked = True
                    'rbCatall.Checked = False
                    txtcatname.Text = Request.QueryString("tocat")
                    'ddlToCategoryName.Value = Request.QueryString("tocat")
                    'ddlToCategory.Value = ddlToCategoryName.Items(ddlToCategoryName.SelectedIndex).Text
                    ''Else
                    '    rbCatrange.Checked = False
                    '    rbCatall.Checked = True
                End If

                If Request.QueryString(" fromctry") <> "" Then
                    'rbCtrrange.Checked = True
                    'rbCtrall.Checked = False
                    txtctryname.Text = Request.QueryString("fromctry")
                    'ddlFromCountryName.Value = Request.QueryString("fromctry")
                    'ddlFromCountry.Value = ddlFromCountryName.Items(ddlFromCountryName.SelectedIndex).Text
                    ''Else
                    '    rbCtrrange.Checked = False
                    '    rbCtrall.Checked = True
                End If

                If Request.QueryString(" toctry") <> "" Then
                    'rbCtrrange.Checked = True
                    'rbCtrall.Checked = False
                    txtctryname.Text = Request.QueryString("toctry")
                    'ddlToCountryName.Value = Request.QueryString("toctry")
                    'ddlToCountry.Value = ddlToCountryName.Items(ddlToCountryName.SelectedIndex).Text
                    ''Else
                    '    rbCtrrange.Checked = False
                End If



                If Request.QueryString(" fromcity") <> "" Then
                    ''rbCityrange.Checked = True
                    ''rbCityall.Checked = False
                    txtcityname.Text = Request.QueryString("fromcity")
                    ''ddlFromCityName.Value = Request.QueryString("fromcity")
                    ''ddlFromCity.Value = ddlFromCityName.Items(ddlFromCityName.SelectedIndex).Text

                    ''Else
                    '    rbCityrange.Checked = False
                End If

                If Request.QueryString(" tocity") <> "" Then
                    ''rbCityrange.Checked = True
                    ''rbCityall.Checked = False
                    txtcityname.Text = Request.QueryString("tocity")
                    ''ddlToCityName.Value = Request.QueryString("tocity")
                    ''ddlToCity.Value = ddlToCityName.Items(ddlToCityName.SelectedIndex).Text
                    ' ''Else
                    '    rbCityrange.Checked = False
                End If


                If Request.QueryString("fromglcode") <> "" Then
                    ''rbControlrange.Checked = True
                    ''rbControlall.Checked = False
                    txtsuppliername.Text = Request.QueryString("fromglcode")
                    ''ddlFromControlName.Value = Request.QueryString("fromglcode")
                    ''ddlFromControl.Value = ddlFromControlName.Items(ddlFromControlName.SelectedIndex).Text
                    'Else
                    '    rbControlrange.Checked = False

                End If

                If Request.QueryString("toglcode") <> "" Then
                    ''rbControlrange.Checked = True
                    ''rbControlall.Checked = False
                    txtsuppliername.Text = Request.QueryString("toglcode")
                    ''ddlToControlName.Value = Request.QueryString("toglcode")
                    ''ddlToControl.Value = ddlToControlName.Items(ddlToControlName.SelectedIndex).Text
                    'Else
                    '    rbControlrange.Checked = False

                End If


                If Request.QueryString("currtype") <> "" Then
                    ddlCurrency.SelectedIndex = IIf(Request.QueryString("currtype") = 1, 0, 1)
                End If
                If Request.QueryString("ledgertype") <> "" Then
                    ddlLedgerType.SelectedIndex = Request.QueryString("ledgertype")
                End If

                ''rbSupall.Attributes.Add("onclick", "rbevent(this,'" & rbSuprange.ClientID & "','A','Supplier')")
                ''rbSuprange.Attributes.Add("onclick", "rbevent(this,'" & rbSupall.ClientID & "','R','Supplier')")
                '' ''rbControlall.Attributes.Add("onclick", "rbevent(this,'" & rbControlrange.ClientID & "','A','Control')")
                ''rbControlrange.Attributes.Add("onclick", "rbevent(this,'" & rbControlall.ClientID & "','R','Control')")
                ''rbCatall.Attributes.Add("onclick", "rbevent(this,'" & rbCatrange.ClientID & "','A','Category')")
                ''rbCatrange.Attributes.Add("onclick", "rbevent(this,'" & rbCatall.ClientID & "','R','Category')")
                ''rbCityall.Attributes.Add("onclick", "rbevent(this,'" & rbCityrange.ClientID & "','A','City')")
                ''rbCityrange.Attributes.Add("onclick", "rbevent(this,'" & rbCityall.ClientID & "','R','City')")
                ''rbCtrall.Attributes.Add("onclick", "rbevent(this,'" & rbCtrrange.ClientID & "','A','Country')")
                ''rbCtrrange.Attributes.Add("onclick", "rbevent(this,'" & rbCtrall.ClientID & "','R','Country')")

                'ddlsuppliertype.Attributes.Add("onchange", "fillSup('" & ddlsuppliertype.ClientID & "')") '06/04/2017

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ''ddlsuppliertype.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlFromSupplier.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlFromSupplierName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlToSupplier.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlToSupplierName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    '' ''ddlFromControl.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlFromControlName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlToControl.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlToControlName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlFromCategory.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlFromCategoryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlToCategory.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlToCategoryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlFromCity.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlFromCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlToCity.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlToCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlFromCountry.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlFromCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlToCountry.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlToCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlCurrency.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlLedgerType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlPDC.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
            End If
            checkrb_status()
            ClientScript.GetPostBackEventReference(Me, String.Empty)
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepSupLedgerWindowPostBack") Then
                btnReport_Click(sender, e)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Public Sub checkrb_status()
        ''If rbSupall.Checked = True Then
        ''    ddlFromSupplier.Disabled = True
        ''    ddlFromSupplierName.Disabled = True
        ''    ddlToSupplier.Disabled = True
        ''    ddlToSupplierName.Disabled = True
        ''Else
        ''    ddlFromSupplier.Disabled = False
        ''    ddlFromSupplierName.Disabled = False
        ''    ddlToSupplier.Disabled = False
        ''    ddlToSupplierName.Disabled = False
        ''End If
        ''If rbControlall.Checked = True Then
        ''    ddlFromControl.Disabled = True
        ''    ddlFromControlName.Disabled = True
        ''    ddlToControl.Disabled = True
        ''    ddlToControlName.Disabled = True
        ''Else
        ''    ddlFromControl.Disabled = False
        ''    ddlFromControlName.Disabled = False
        ''    ddlToControl.Disabled = False
        ''    ddlToControlName.Disabled = False
        ''End If
        ''If rbCatall.Checked = True Then
        ''    ddlFromCategory.Disabled = True
        ''    ddlFromCategoryName.Disabled = True
        ''    ddlToCategory.Disabled = True
        ''    ddlToCategoryName.Disabled = True
        ''Else
        ''    ddlFromCategory.Disabled = False
        ''    ddlFromCategoryName.Disabled = False
        ''    ddlToCategory.Disabled = False
        ''    ddlToCategoryName.Disabled = False
        ''End If
        ''If rbCityall.Checked = True Then
        ''    ddlFromCity.Disabled = True
        ''    ddlFromCityName.Disabled = True
        ''    ddlToCity.Disabled = True
        ''    ddlToCityName.Disabled = True
        ''Else
        ''    ddlFromCity.Disabled = False
        ''    ddlFromCityName.Disabled = False
        ''    ddlToCity.Disabled = False
        ''    ddlToCityName.Disabled = False
        ''End If
        ''If rbCtrall.Checked = True Then
        ''    ddlFromCountry.Disabled = True
        ''    ddlFromCountryName.Disabled = True
        ''    ddlToCountry.Disabled = True
        ''    ddlToCountryName.Disabled = True
        ''Else
        ''    ddlFromCountry.Disabled = False
        ''    ddlFromCountryName.Disabled = False
        ''    ddlToCountry.Disabled = False
        ''    ddlToCountryName.Disabled = False
        ''End If
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Try

            If ValidatePage() = False Then
                Exit Sub
            End If

            'Session.Add("Pageame", "SupplierLedger")
            'Session.Add("BackPageName", "RptSupplierLedger.aspx")
            Dim strReportTitle As String = ""
            Dim strfromdate As String = ""
            Dim strtodate As String = ""
            Dim stracctype As String = ""

            Dim strcurrtype As String = ""
            Dim strfromcode As String = ""
            Dim strtocode As String = ""
            Dim strglcode As String = ""
            Dim strglcodeto As String = ""
            Dim strcatcode As String = ""
            Dim strcatcodeto As String = ""
            Dim strfromcity As String = ""
            Dim strtocity As String = ""
            Dim strfromctry As String = ""
            Dim strtoctry As String = ""
            Dim ledgertype As String = ""
            Dim pdcyesno As String = ""
            Dim custgroup_sp_type As String

            custgroup_sp_type = txtsuptypecode.Text


            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            stracctype = ddlsuppliertype.SelectedValue

            strfromcode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text, "") 'IIf(UCase(ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text, "")
            strtocode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text, "") ' IIf(UCase(ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text, "")

            strcatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "") 'IIf(UCase(ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text, "")
            strcatcodeto = IIf(txtcatcode.Text <> "", txtcatcode.Text, "") 'IIf(UCase(ddlToCategory.Items(ddlToCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCategory.Items(ddlToCategory.SelectedIndex).Text, "")


            strfromctry = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") 'IIf(UCase(ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text, "")
            strtoctry = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") 'IIf(UCase(ddlToCountry.Items(ddlToCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCountry.Items(ddlToCountry.SelectedIndex).Text, "")

            strfromcity = IIf(txtcitycode.Text <> "", txtcitycode.Text, "") ' IIf(UCase(ddlFromCity.Items(ddlFromCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCity.Items(ddlFromCity.SelectedIndex).Text, "")
            strtocity = IIf(txtcitycode.Text <> "", txtcitycode.Text, "") ''IIf(UCase(ddlToCity.Items(ddlToCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCity.Items(ddlToCity.SelectedIndex).Text, "")




            strglcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "")  'IIf(UCase(ddlFromControl.Items(ddlFromControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromControl.Items(ddlFromControl.SelectedIndex).Text, "")
            strglcodeto = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "") 'IIf(UCase(ddlToControl.Items(ddlToControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToControl.Items(ddlToControl.SelectedIndex).Text, "")
            'strcurrtype = ddlCurrency.SelectedIndex
            ledgertype = ddlLedgerType.Value
            pdcyesno = ddlPDC.Value


            Select Case ddlCurrency.SelectedIndex
                Case 0
                    strcurrtype = 1
                Case 1
                    strcurrtype = 0

                Case Else
                    strcurrtype = ddlCurrency.SelectedIndex

            End Select

            'Response.Redirect("rptsupp_custledgerReport.aspx?type=S&fromdate=" & strfromdate & "&todate=" & strtodate _
            '& "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcity=" & strfromcity & "&tocity=" & strtocity & "&actype=" & stracctype & "&fromcode=" & strfromcode & "&tocode=" & strtocode _
            '& "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto _
            '& "&currtype=" & strcurrtype & "&ledgertype=" & ledgertype & "&pdcyesno=" & pdcyesno, False)

            Dim strpop As String = ""
            'strpop = "window.open('rptsupp_custledgerReport.aspx?type=S&Pageame=SupplierLedger&BackPageName=RptSupplierLedger.aspx&fromdate=" & strfromdate & "&todate=" & strtodate _
            '& "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcity=" & strfromcity & "&tocity=" & strtocity & "&actype=" & stracctype & "&fromcode=" & strfromcode & "&tocode=" & strtocode _
            '& "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto _
            '& "&currtype=" & strcurrtype & "&ledgertype=" & ledgertype & "&pdcyesno=" & pdcyesno & "','RepSupLedger','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"

            strpop = "window.open('rptsupp_custledgerReport.aspx?type=S&Pageame=SupplierLedger&BackPageName=RptSupplierLedger.aspx&fromdate=" & strfromdate & "&todate=" & strtodate _
            & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcity=" & strfromcity & "&tocity=" & strtocity & "&actype=" & stracctype & "&fromcode=" & strfromcode & "&tocode=" & strtocode _
            & "&fromcat=" & strcatcode & "&tocat=" & strcatcode & "&fromglcode=" & strglcode & "&divid=" & divcode & "&toglcode=" & strglcodeto _
            & "&currtype=" & strcurrtype & "&ledgertype=" & ledgertype & "&custgroup_sp_type=" & custgroup_sp_type & "&pdcyesno=" & pdcyesno & "','RepSupLedger');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptsuppliertrialbalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try


    End Sub


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptSupplierLedger','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Dim frmdate As DateTime
        Dim MyCultureInfo As New CultureInfo("fr-Fr")
        Dim ds As DataSet

        Try
            If txtFromDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtFromDate.ClientID + "');", True)
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
            If CType(objDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objDate.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If

            frmdate = DateTime.Parse(txtFromDate.Text, MyCultureInfo, DateTimeStyles.None)
            ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=1103")
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)(0)) = False Then
                        If frmdate < ds.Tables(0).Rows(0)("option_selected") Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date cannot enter below the " & ds.Tables(0).Rows(0)("option_selected") & " ' );", True)
                            ValidatePage = False
                            Exit Function
                        End If
                    End If
                End If
            End If


            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function

    Protected Sub ddlsupptype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddlSupplierType.SelectedValue = "S " Then
            supptype = "S"
            txtsuppliercode.Text = ""
            txtsuppliername.Text = ""
        End If
        If ddlSupplierType.SelectedValue = "A" Then
            supptype = "A"
            txtsuppliercode.Text = ""
            txtsuppliername.Text = ""
        End If
    End Sub

    Protected Sub Button1_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try

            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(16) As SqlParameter


            If ValidatePage() = False Then
                Exit Sub
            End If

            'Session.Add("Pageame", "SupplierLedger")
            'Session.Add("BackPageName", "RptSupplierLedger.aspx")
            Dim strReportTitle As String = ""
            Dim strfromdate As String = ""
            Dim strtodate As String = ""
            Dim stracctype As String = ""

            Dim strcurrtype As String = ""
            Dim strfromcode As String = ""
            Dim strtocode As String = ""
            Dim strglcode As String = ""
            Dim strglcodeto As String = ""
            Dim strcatcode As String = ""
            Dim strcatcodeto As String = ""
            Dim strfromcity As String = ""
            Dim strtocity As String = ""
            Dim strfromctry As String = ""
            Dim strtoctry As String = ""
            Dim ledgertype As String = ""
            Dim pdcyesno As String = ""


            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            stracctype = ddlSupplierType.SelectedValue

            ''strfromcode = IIf(UCase(ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text, "")
            ''strtocode = IIf(UCase(ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text, "")

            ''strcatcode = IIf(UCase(ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text, "")
            ''strcatcodeto = IIf(UCase(ddlToCategory.Items(ddlToCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCategory.Items(ddlToCategory.SelectedIndex).Text, "")


            ''strfromctry = IIf(UCase(ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text, "")
            ''strtoctry = IIf(UCase(ddlToCountry.Items(ddlToCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCountry.Items(ddlToCountry.SelectedIndex).Text, "")

            ''strfromcity = IIf(UCase(ddlFromCity.Items(ddlFromCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCity.Items(ddlFromCity.SelectedIndex).Text, "")
            ''strtocity = IIf(UCase(ddlToCity.Items(ddlToCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCity.Items(ddlToCity.SelectedIndex).Text, "")




            strglcode = TxtBankCode.Text 'IIf(UCase(ddlFromControl.Items(ddlFromControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromControl.Items(ddlFromControl.SelectedIndex).Text, "")
            strglcodeto = TxtBankCode.Text ' IIf(UCase(ddlToControl.Items(ddlToControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToControl.Items(ddlToControl.SelectedIndex).Text, "")
            'strcurrtype = ddlCurrency.SelectedIndex
            ledgertype = ddlLedgerType.Value
            pdcyesno = IIf(ddlPDC.Value = "Yes", "1", "0")


            Select Case ddlCurrency.SelectedIndex
                Case 0
                    strcurrtype = 1
                Case 1
                    strcurrtype = 0

                Case Else
                    strcurrtype = ddlCurrency.SelectedIndex

            End Select



            parm(0) = New SqlParameter("@fromdate", strfromdate)
            parm(1) = New SqlParameter("@todate", strtodate)
            parm(2) = New SqlParameter("@type", stracctype)
            parm(3) = New SqlParameter("@currflg", strcurrtype)
            parm(4) = New SqlParameter("@fromacct", strfromcode)
            parm(5) = New SqlParameter("@toacct", strtocode)
            parm(6) = New SqlParameter("@fromcontrol", strglcode)
            parm(7) = New SqlParameter("@tocontrol", strglcodeto)
            parm(8) = New SqlParameter("@fromcat", strcatcode)
            parm(9) = New SqlParameter("@tocat", strcatcodeto)
            parm(10) = New SqlParameter("@fromcity", strfromcity)
            parm(11) = New SqlParameter("@tocity", strtocity)
            parm(12) = New SqlParameter("@fromctry", strfromctry)
            parm(13) = New SqlParameter("@toctry", strtoctry)
            parm(14) = New SqlParameter("@ledgertype", ledgertype)
            parm(15) = New SqlParameter("@pdcyesno", pdcyesno)


            For i = 0 To 15
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_party_ledger_xls", parms)

            If ds.Tables.Count > 0 Then

                If ds.Tables(0).Rows.Count > 0 Then
                    objUtils.ExportToExcel(ds, Response)
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Record found' );", True)
                End If

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptSupplierLedger.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Page.IsPostBack = False Then
            If Request.QueryString("appid") Is Nothing = False Then
                Dim appid As String = CType(Request.QueryString("appid"), String)
                Select Case appid
                    Case 1
                        Me.MasterPageFile = "~/PriceListMaster.master"
                    Case 2
                        Me.MasterPageFile = "~/RoomBlock.master"
                    Case 3
                        Me.MasterPageFile = "~/ReservationMaster.master"
                    Case 4
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 5
                        Me.MasterPageFile = "~/UserAdminMaster.master"
                    Case 6
                        Me.MasterPageFile = "~/WebAdminMaster.master"
                    Case 7
                        Me.MasterPageFile = "~/TransferHistoryMaster.master"
                    Case 14
                        Me.MasterPageFile = "~/AccountsMaster.master"
                    Case 16
                        Me.MasterPageFile = "~/AccountsMaster.master"   '' Added shahul MCP accounts
                    Case Else
                        Me.MasterPageFile = "~/SubPageMaster.master"
                End Select
            Else
                Me.MasterPageFile = "~/SubPageMaster.master"
            End If
        End If
    End Sub

    Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
        Try

            If ValidatePage() = False Then
                Exit Sub
            End If

            'Session.Add("Pageame", "SupplierLedger")
            'Session.Add("BackPageName", "RptSupplierLedger.aspx")
            Dim strReportTitle As String = ""
            Dim strfromdate As String = ""
            Dim strtodate As String = ""
            Dim stracctype As String = ""

            Dim strcurrtype As String = ""
            Dim strfromname As String = ""
            Dim strtoname As String = ""
            Dim strfromcode As String = ""
            Dim strtocode As String = ""
            Dim strglcode As String = ""
            Dim strglcodeto As String = ""
            Dim strcatcode As String = ""
            Dim strcatcodeto As String = ""
            Dim strfromcity As String = ""
            Dim strtocity As String = ""
            Dim strfromctry As String = ""
            Dim strtoctry As String = ""
            Dim ledgertype As String = ""
            Dim pdcyesno As String = ""
            Dim custgroup_sp_type As String
            Dim strglname As String = ""
            Dim strglnameto As String = ""
            custgroup_sp_type = txtsuptypecode.Text


            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            stracctype = ddlSupplierType.SelectedValue


            strfromcode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text, "") 'IIf(UCase(ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text, "")
            strtocode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text, "") ' IIf(UCase(ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text, "")

            strfromname = IIf(txtsuppliername.Text <> "", txtsuppliername.Text, "") 'IIf(UCase(ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text, "")
            ' strtoname = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text, "") ' IIf(UCase(ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text, "")


            strcatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "") 'IIf(UCase(ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text, "")
            strcatcodeto = IIf(txtcatcode.Text <> "", txtcatcode.Text, "") 'IIf(UCase(ddlToCategory.Items(ddlToCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCategory.Items(ddlToCategory.SelectedIndex).Text, "")



            strfromctry = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") 'IIf(UCase(ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text, "")
            strtoctry = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") 'IIf(UCase(ddlToCountry.Items(ddlToCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCountry.Items(ddlToCountry.SelectedIndex).Text, "")

            strfromcity = IIf(txtcitycode.Text <> "", txtcitycode.Text, "") ' IIf(UCase(ddlFromCity.Items(ddlFromCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCity.Items(ddlFromCity.SelectedIndex).Text, "")
            strtocity = IIf(txtcitycode.Text <> "", txtcitycode.Text, "") ''IIf(UCase(ddlToCity.Items(ddlToCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCity.Items(ddlToCity.SelectedIndex).Text, "")



            strglname = IIf(TxtBankName.Text <> "", TxtBankName.Text, "")
            strglcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "")  'IIf(UCase(ddlFromControl.Items(ddlFromControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromControl.Items(ddlFromControl.SelectedIndex).Text, "")
            strglcodeto = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "") 'IIf(UCase(ddlToControl.Items(ddlToControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToControl.Items(ddlToControl.SelectedIndex).Text, "")
            'strcurrtype = ddlCurrency.SelectedIndex
            ledgertype = ddlLedgerType.Value
            pdcyesno = ddlPDC.Value


            Select Case ddlCurrency.SelectedIndex
                Case 0
                    strcurrtype = 1
                Case 1
                    strcurrtype = 0

                Case Else
                    strcurrtype = ddlCurrency.SelectedIndex

            End Select

            'Response.Redirect("rptsupp_custledgerReport.aspx?type=S&fromdate=" & strfromdate & "&todate=" & strtodate _
            '& "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcity=" & strfromcity & "&tocity=" & strtocity & "&actype=" & stracctype & "&fromcode=" & strfromcode & "&tocode=" & strtocode _
            '& "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto _
            '& "&currtype=" & strcurrtype & "&ledgertype=" & ledgertype & "&pdcyesno=" & pdcyesno, False)

            Dim strpop As String = ""
            'strpop = "window.open('rptsupp_custledgerReport.aspx?type=S&Pageame=SupplierLedger&BackPageName=RptSupplierLedger.aspx&fromdate=" & strfromdate & "&todate=" & strtodate _
            '& "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcity=" & strfromcity & "&tocity=" & strtocity & "&actype=" & stracctype & "&fromcode=" & strfromcode & "&tocode=" & strtocode _
            '& "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto _
            '& "&currtype=" & strcurrtype & "&ledgertype=" & ledgertype & "&pdcyesno=" & pdcyesno & "','RepSupLedger','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
            strpop = "window.open('TransactionReports.aspx?type=S&Pageame=SupplierLedger&reportsType=pdf&BackPageName=RptSupplierLedger.aspx&printId=SupplierLedger&fromdate=" & strfromdate & "&todate=" & strtodate _
                                   & "&fromname=" & strfromname & "&glname=" & strglname & "&frommkname=" & txtcityname.Text & "&fromctryname=" & txtctryname.Text & "&fromcatname=" & txtcatname.Text & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcity=" & strfromcity & "&tocity=" & strtocity & "&actype=" & stracctype & "&fromcode=" & strfromcode & "&tocode=" & strtocode _
                                   & "&fromcat=" & strcatcode & "&tocat=" & strcatcode & "&fromglcode=" & strglcode & "&divid=" & divcode & "&toglcode=" & strglcodeto _
                                   & "&currtype=" & strcurrtype & "&ledgertype=" & ledgertype & "&custgroup_sp_type=" & custgroup_sp_type & "&pdcyesno=" & pdcyesno & "','RepSupLedger');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptsuppliertrialbalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try


    End Sub

    Protected Sub btnExlReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExlReport.Click
        Try

            If ValidatePage() = False Then
                Exit Sub
            End If

            'Session.Add("Pageame", "SupplierLedger")
            'Session.Add("BackPageName", "RptSupplierLedger.aspx")
            Dim strReportTitle As String = ""
            Dim strfromdate As String = ""
            Dim strtodate As String = ""
            Dim stracctype As String = ""

            Dim strcurrtype As String = ""
            Dim strfromname As String = ""
            Dim strtoname As String = ""
            Dim strfromcode As String = ""
            Dim strtocode As String = ""
            Dim strglcode As String = ""
            Dim strglcodeto As String = ""
            Dim strcatcode As String = ""
            Dim strcatcodeto As String = ""
            Dim strfromcity As String = ""
            Dim strtocity As String = ""
            Dim strfromctry As String = ""
            Dim strtoctry As String = ""
            Dim ledgertype As String = ""
            Dim pdcyesno As String = ""
            Dim custgroup_sp_type As String
            Dim strglname As String = ""
            Dim strglnameto As String = ""
            custgroup_sp_type = txtsuptypecode.Text


            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            stracctype = ddlSupplierType.SelectedValue


            strfromcode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text, "") 'IIf(UCase(ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text, "")
            strtocode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text, "") ' IIf(UCase(ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text, "")

            strfromname = IIf(txtsuppliername.Text <> "", txtsuppliername.Text, "") 'IIf(UCase(ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text, "")
            ' strtoname = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text, "") ' IIf(UCase(ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text, "")


            strcatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "") 'IIf(UCase(ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text, "")
            strcatcodeto = IIf(txtcatcode.Text <> "", txtcatcode.Text, "") 'IIf(UCase(ddlToCategory.Items(ddlToCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCategory.Items(ddlToCategory.SelectedIndex).Text, "")



            strfromctry = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") 'IIf(UCase(ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text, "")
            strtoctry = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") 'IIf(UCase(ddlToCountry.Items(ddlToCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCountry.Items(ddlToCountry.SelectedIndex).Text, "")

            strfromcity = IIf(txtcitycode.Text <> "", txtcitycode.Text, "") ' IIf(UCase(ddlFromCity.Items(ddlFromCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCity.Items(ddlFromCity.SelectedIndex).Text, "")
            strtocity = IIf(txtcitycode.Text <> "", txtcitycode.Text, "") ''IIf(UCase(ddlToCity.Items(ddlToCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCity.Items(ddlToCity.SelectedIndex).Text, "")



            strglname = IIf(TxtBankName.Text <> "", TxtBankName.Text, "")
            strglcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "")  'IIf(UCase(ddlFromControl.Items(ddlFromControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromControl.Items(ddlFromControl.SelectedIndex).Text, "")
            strglcodeto = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "") 'IIf(UCase(ddlToControl.Items(ddlToControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToControl.Items(ddlToControl.SelectedIndex).Text, "")
            'strcurrtype = ddlCurrency.SelectedIndex
            ledgertype = ddlLedgerType.Value
            pdcyesno = ddlPDC.Value



            Select Case ddlCurrency.SelectedIndex
                Case 0
                    strcurrtype = 1
                Case 1
                    strcurrtype = 0

                Case Else
                    strcurrtype = ddlCurrency.SelectedIndex

            End Select

            Dim strpop As String = ""
            strpop = "window.open('TransactionReports.aspx?type=S&Pageame=SupplierLedger&reportsType=excel&BackPageName=RptSupplierLedger.aspx&printId=SupplierLedger&fromdate=" & strfromdate & "&todate=" & strtodate _
             & "&fromname=" & strfromname & "&glname=" & strglname & "&frommkname=" & txtcityname.Text & "&fromctryname=" & txtctryname.Text & "&fromcatname=" & txtcatname.Text & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcity=" & strfromcity & "&tocity=" & strtocity & "&actype=" & stracctype & "&fromcode=" & strfromcode & "&tocode=" & strtocode _
             & "&fromcat=" & strcatcode & "&tocat=" & strcatcode & "&fromglcode=" & strglcode & "&divid=" & divcode & "&toglcode=" & strglcodeto _
             & "&currtype=" & strcurrtype & "&ledgertype=" & ledgertype & "&custgroup_sp_type=" & custgroup_sp_type & "&pdcyesno=" & pdcyesno & "','RepSupLedger');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptsuppliertrialbalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try


    End Sub

End Class
