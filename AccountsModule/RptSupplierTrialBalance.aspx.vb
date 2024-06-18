'------------================--------------=======================------------------================
'   Module Name    :    RptSupplierTrialBalance.aspx
'   Developer Name :    Sandeep Indulkar
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
Partial Class RptSupplierTrialBalance
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
    Dim strappid As String = ""
    Dim strappname As String = ""
    Shared supptype As String = ""

    Shared divcode As String = ""
#End Region
    <System.Web.Script.Services.ScriptMethod()> _
      <System.Web.Services.WebMethod()> _
    Public Shared Function Getbankslist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim acctnames As New List(Of String)
        Dim divid As String = ""
        Try

            'If Not HttpContext.Current.Session("divcode") Is Nothing Then
            '    divid = Convert.ToString(HttpContext.Current.Session("divcode").ToString())
            'End If


            strSqlQry = "select acctcode,acctname from acctmast where controlyn='Y' and cust_supp='S'  and div_code='" & divcode & "' and acctname like  '" & Trim(prefixText) & "%' order by acctcode"
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
    Public Shared Function Getsupplierlist(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppliernames As New List(Of String)
        Dim city, ctry, custagent, type As String
        ctry = ""
        city = ""

        custagent = ""
        Type = ""
        Try

            If contextKey.ToLower = "true" Then
                contextKey = ""
            Else
                If contextKey <> "" Then
                    ctry = contextKey.Trim.Split("||")(0)

                    city = contextKey.Trim.Split("||")(2)

                    custagent = contextKey.Trim.Split("||")(4)
                    Type = contextKey.Trim.Split("||")(6)
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

                If Type <> "" Then
                    strSqlQry = strSqlQry & " and sptypecode='" & Type & "'"
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

                If Type <> "" Then
                    strSqlQry = strSqlQry & " and sptypecode='" & Type & "'"
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
        ViewState.Add("RptSupplierTrialBalanceTranType", Request.QueryString("tran_type"))

        Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        Dim appidnew As String = CType(Request.QueryString("appid"), String)
        Dim tran_type As String = CType(Request.QueryString("tran_type"), String)

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
                                               CType(strappname, String), "AccountsModule\RptSupplierTrialBalance.aspx?tran_type=" + tran_type + "&appid=" + strappid, btnadd, Button1, btnLoadreport, gv_SearchResult)
        End If

        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
        '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        ViewState.Add("divcode", divid)
        supptype = "S"
        divcode = ViewState("divcode")
        txtsuptypecode.Attributes.Add("readonly", "readonly")
        txtsuppliercode.Attributes.Add("readonly", "readonly")
        TxtBankCode.Attributes.Add("readonly", "readonly")
        txtcatcode.Attributes.Add("readonly", "readonly")
        txtcitycode.Attributes.Add("readonly", "readonly")
        txtctrycode.Attributes.Add("readonly", "readonly")
        If Page.IsPostBack = False Then
            Try
                txtconnection.Value = Session("dbconnectionName")
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                If ViewState("RptSupplierTrialBalanceTranType") = "WODR" Then
                    lblHeading.Text = "Supplier Trial Balance"
                ElseIf ViewState("RptSupplierTrialBalanceTranType") = "WTDR" Then
                    lblHeading.Text = "Supplier With Debit Balance"
                End If

                Dim strfromctrlcode1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select options_available from reservation_parameters where param_id=538")
                Dim strfromctrlname1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_value from reservation_parameters where param_id=538")

                Dim strtoctrlcode1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select options_available from reservation_parameters where param_id=539")
                Dim strtoctrlname1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_value from reservation_parameters where param_id=539")

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcurrency, "currcode", "currcode", "select option_selected as currcode from reservation_parameters where param_id=457 union select 'A/C Currency'", False)



                '----------------------------- Default Dates
                txtFromDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                txttoDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")

                ' txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")


                ' --



                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format("U", CType(Request.QueryString("fromdate"), Date))
                End If
                If Request.QueryString("todate") <> "" Then
                    ' todate = Request.QueryString("todate")
                    txttoDate.Text = Format("U", CType(Request.QueryString("todate"), Date))
                End If
                If Request.QueryString("movflg") <> "" Then
                    ddlwithmovmt.SelectedIndex = Request.QueryString("movflg")

                End If


                If Request.QueryString("acctype") <> "" Then
                    ddlSupType.SelectedValue = Request.QueryString("acctype")
                End If

                'ddlSupType.Attributes.Add("onchange", "fillSup('" & ddlSupType.ClientID & "')")

                'If Request.QueryString("fromcode") <> "" Then
                '    rbSuprange.Checked = True
                '    ddlSuppliername.Value = Request.QueryString("fromcode")
                '    ddlSuppliercode.Value = ddlSuppliername.Items(ddlSuppliername.SelectedIndex).Text

                'Else
                '    rbSuprange.Checked = False
                'End If
                'If Request.QueryString("tocode") <> "" Then
                '    rbSuprange.Checked = True
                '    ddlSuppliernameto.Value = Request.QueryString("tocode")
                '    ddlSuppliercodeto.Value = ddlSuppliernameto.Items(ddlSuppliernameto.SelectedIndex).Text
                'Else
                '    rbSuprange.Checked = False
                'End If
                If Request.QueryString("fromsptype") <> "" Then

                    ''    Rbsuptyperange.Checked = True
                    ''    ddlsuppliertypenamefrm.Value = Request.QueryString("fromsptype")
                    ''    ddlsuppliertypecodefrm.Value = ddlsuppliertypenamefrm.Items(ddlsuppliertypenamefrm.SelectedIndex).Text
                    ''Else
                    ''Rbsuptyperange.Checked = False
                End If
                If Request.QueryString("fromtosptype") <> "" Then
                    ''Rbsuptyperange.Checked = True
                    ''ddlsuppliertypenameto.Value = Request.QueryString("fromtosptype")
                    ''ddlsuppliertypecodeto.Value = ddlsuppliertypenameto.Items(ddlsuppliertypenameto.SelectedIndex).Text


                Else
                    ''  Rbsuptyperange.Checked = False


                End If
                If Request.QueryString(" fromcat") <> "" Then
                    ''rbCatrange.Checked = True
                    ''ddlCategoryname.Value = Request.QueryString("fromcat")
                    ''ddlCategorycode.Value = ddlCategoryname.Items(ddlCategoryname.SelectedIndex).Text

                Else
                    '' rbCatrange.Checked = False
                End If

                If Request.QueryString(" tocat") <> "" Then
                    ' ' rbCatrange.Checked = True
                    ''ddlCategorynameto.Value = Request.QueryString("tocat")
                    ''ddlCategorycodeto.Value = ddlCategorynameto.Items(ddlCategorynameto.SelectedIndex).Text
                Else
                    '  '   rbCatrange.Checked = False
                End If



                If Request.QueryString(" fromctry") <> "" Then
                    ''rbcountryrange.Checked = True
                    ''ddlCountryName.Value = Request.QueryString("fromctry")
                    ''ddlCountrycode.Value = ddlCountryName.Items(ddlCountryName.SelectedIndex).Text

                Else
                    'rbcountryrange.Checked = False
                End If

                If Request.QueryString(" toctry") <> "" Then
                    ''rbcountryrange.Checked = True
                    ''ddlCountrynameto.Value = Request.QueryString("toctry")
                    ''ddlCountrycodeto.Value = ddlCountrynameto.Items(ddlCountrynameto.SelectedIndex).Text
                Else
                    '' rbcountryrange.Checked = False
                End If



                If Request.QueryString(" fromcity") <> "" Then
                    ''rbCityrange.Checked = True
                    ''ddlCityname.Value = Request.QueryString("fromcity")
                    ''ddlCitycode.Value = ddlCityname.Items(ddlCityname.SelectedIndex).Text

                Else
                    '' rbCityrange.Checked = False
                End If

                If Request.QueryString(" tocity") <> "" Then
                    ''rbCityrange.Checked = True
                    ''ddlCitynameto.Value = Request.QueryString("tocity")
                    ''ddlCitycodeto.Value = ddlCitynameto.Items(ddlCitynameto.SelectedIndex).Text
                Else
                    ''rbCityrange.Checked = False
                End If




                If Request.QueryString("fromglcode") <> "" Then

                    ''rbtnctrlacctrng.Checked = True
                    ''ddlctrlnamefrm.Value = Request.QueryString("fromglcode")
                    ''ddlctrlcodefrm.Value = ddlctrlnamefrm.Items(ddlctrlnamefrm.SelectedIndex).Text
                Else
                    ''   rbtnctrlacctrng.Checked = False

                End If

                If Request.QueryString("toglcode") <> "" Then
                    ''rbtnctrlacctrng.Checked = True
                    ''ddlctrnameto.Value = Request.QueryString("toglcode")
                    ''ddlctrlcodeto.Value = ddlctrnameto.Items(ddlctrnameto.SelectedIndex).Text
                Else
                    ''   rbtnctrlacctrng.Checked = False

                End If




                If Request.QueryString("currtype") <> "" Then
                    ddlcurrency.SelectedIndex = IIf(Request.QueryString("currtype") = 1, 0, 1)
                End If
                If Request.QueryString("orderby") <> "" Then
                    ddlrptord.SelectedIndex = Request.QueryString("orderby")
                End If
                If Request.QueryString("includezero") <> "" Then
                    ddlinclzero.SelectedIndex = Request.QueryString("includezero")

                End If
                If Request.QueryString("gpby") <> "" Then
                    ddlgpby.SelectedIndex = Request.QueryString("gpby")
                End If


                ''rbCatall.Attributes.Add("onclick", "rbevent(this,'" & rbCatrange.ClientID & "','A','Category')")
                ''rbCatrange.Attributes.Add("onclick", "rbevent(this,'" & rbCatall.ClientID & "','R','Category')")
                ''rbCityall.Attributes.Add("onclick", "rbevent(this,'" & rbCityrange.ClientID & "','A','City')")
                ''rbCityrange.Attributes.Add("onclick", "rbevent(this,'" & rbCityall.ClientID & "','R','City')")
                ''rbSupall.Attributes.Add("onclick", "rbevent(this,'" & rbSuprange.ClientID & "','A','Supplier')")
                ''rbSuprange.Attributes.Add("onclick", "rbevent(this,'" & rbSupall.ClientID & "','R','Supplier')")
                ''rbcountryall.Attributes.Add("onclick", "rbevent(this,'" & rbcountryrange.ClientID & "','A','Country')")
                ''rbcountryrange.Attributes.Add("onclick", "rbevent(this,'" & rbcountryall.ClientID & "','R','Country')")
                ''Rbsuptypeall.Attributes.Add("onclick", "rbevent(this,'" & Rbsuptyperange.ClientID & "','A','Suppliertype')")
                ''Rbsuptyperange.Attributes.Add("onclick", "rbevent(this,'" & Rbsuptypeall.ClientID & "','R','Suppliertype')")
                '' ''rbtnctrlacctall.Attributes.Add("onclick", "rbevent(this,'" & rbtnctrlacctrng.ClientID & "','A','ctrlacct')")
                ''rbtnctrlacctrng.Attributes.Add("onclick", "rbevent(this,'" & rbtnctrlacctall.ClientID & "','R','ctrlacct')")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ''ddlCategorycode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlCategoryname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlCategorycodeto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlCategorynameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ''ddlCountrycode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlCountrycodeto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlCountrynameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ''ddlCitycode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlCityname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlCitycodeto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlCitynameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ''ddlSuppliercode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlSuppliername.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlSuppliercodeto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlSuppliernameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ''ddlsuppliertypecodefrm.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlsuppliertypenamefrm.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlsuppliertypenamefrm.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlsuppliertypenameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


                    ''ddlctrlcodefrm.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlctrlcodeto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlctrlnamefrm.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ''ddlctrnameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If






            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog(" ", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        Else
            checkrb_status()

        End If
        btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Exit?')==false)return false;")
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RptSupplierTrialBalanceWindowPostBack") Then

        End If

    End Sub

    Public Sub checkrb_status()



        ''If rbCatall.Checked = True Then
        ''    ddlCategorycode.Disabled = True
        ''    ddlCategoryname.Disabled = True
        ''    ddlCategorycodeto.Disabled = True
        ''    ddlCategorynameto.Disabled = True
        ''Else
        ''    ddlCategorycode.Disabled = False
        ''    ddlCategoryname.Disabled = False
        ''    ddlCategorycodeto.Disabled = False
        ''    ddlCategorynameto.Disabled = False
        ''End If
        ''If rbCityall.Checked = True Then



        ''    ddlCitycode.Disabled = True
        ''    ddlCityname.Disabled = True
        ''    ddlCitycodeto.Disabled = True
        ''    ddlCitynameto.Disabled = True
        ''Else
        ''    ddlCitycode.Disabled = False
        ''    ddlCityname.Disabled = False
        ''    ddlCitycodeto.Disabled = False
        ''    ddlCitynameto.Disabled = False
        ''End If
        ''If rbSupall.Checked = True Then


        ''    ddlSuppliercode.Disabled = True
        ''    ddlSuppliername.Disabled = True
        ''    ddlSuppliercodeto.Disabled = True
        ''    ddlSuppliernameto.Disabled = True
        ''Else
        ''    ddlSuppliercode.Disabled = False
        ''    ddlSuppliername.Disabled = False
        ''    ddlSuppliercodeto.Disabled = False
        ''    ddlSuppliernameto.Disabled = False
        ''End If
        ''  If rbcountryall.Checked = True Then



        ''    ddlCountrycode.Disabled = True
        ''    ddlCountryName.Disabled = True
        ''    ddlCountrycodeto.Disabled = True
        ''    ddlCountrynameto.Disabled = True


        ''Else

        ''    ddlCountrycode.Disabled = False
        ''    ddlCountryName.Disabled = False
        ''    ddlCountrycodeto.Disabled = False
        ''    ddlCountrynameto.Disabled = False

        ''End If




        ''If Rbsuptypeall.Checked = True Then


        ''    ddlsuppliertypecodefrm.Disabled = True
        ''    ddlsuppliertypenamefrm.Disabled = True
        ''    ddlsuppliertypecodeto.Disabled = True
        ''    ddlsuppliertypenameto.Disabled = True
        ''Else
        ''    ddlsuppliertypecodefrm.Disabled = False
        ''    ddlsuppliertypenamefrm.Disabled = False
        ''    ddlsuppliertypecodeto.Disabled = False
        ''    ddlsuppliertypenameto.Disabled = False
        ''End If


        ''If rbtnctrlacctall.Checked = True Then

        ''    ddlctrlcodefrm.Disabled = True
        ''    ddlctrlcodeto.Disabled = True
        ''    ddlctrlnamefrm.Disabled = True
        ''    ddlctrnameto.Disabled = True
        ''Else
        ''    ddlctrlcodefrm.Disabled = False
        ''    ddlctrlcodeto.Disabled = False
        ''    ddlctrlnamefrm.Disabled = False
        ''    ddlctrnameto.Disabled = False
        ''End If


    End Sub


    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Response.Redirect("~/MainPage.aspx")
    End Sub

    Protected Sub btnLoadreport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadreport.Click
        Try
            'If ValidatePage() = True Then

            If ValidatePage() = False Then
                Exit Sub
            End If
            ViewState.Add("Pageame", "SuppliertrialbalReport")
            ViewState.Add("BackPageName", "RptSupplierTrialBalance.aspx")



            Dim strReportTitle As String = ""

            Dim strfromdate As String = ""
            Dim strtodate As String = ""
            Dim strmovflag As String = ""
            Dim stracctype As String = ""
            Dim strfromcode As String = ""
            Dim strtocode As String = ""
            Dim strsuptypecode As String = ""
            Dim strsuptypecodeto As String = ""
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
            Dim strfromcity As String = ""
            Dim strtocity As String = ""
            Dim withCredit As String = ""
            Dim strtrialtype As String = ""

            strmovflag = ddlwithmovmt.SelectedValue


            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            strtodate = IIf(strmovflag = 0, Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10), strfromdate)

            stracctype = ddlSupType.SelectedValue

            strfromcode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text.Trim, "") 'IIf(UCase(ddlSuppliercode.Items(ddlSuppliercode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSuppliercode.Items(ddlSuppliercode.SelectedIndex).Text, "")
            strtocode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text.Trim, "") ' IIf(UCase(ddlSuppliercodeto.Items(ddlSuppliercodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSuppliercodeto.Items(ddlSuppliercodeto.SelectedIndex).Text, "")

            strsuptypecode = IIf(txtsuptypecode.Text <> "", txtsuptypecode.Text.Trim, "") ' IIf(UCase(ddlsuppliertypecodefrm.Items(ddlsuppliertypecodefrm.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlsuppliertypecodefrm.Items(ddlsuppliertypecodefrm.SelectedIndex).Text, "")
            strsuptypecodeto = IIf(txtsuptypecode.Text <> "", txtsuptypecode.Text.Trim, "") 'IIf(UCase(ddlsuppliertypecodeto.Items(ddlsuppliertypecodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlsuppliertypecodeto.Items(ddlsuppliertypecodeto.SelectedIndex).Text, "")
            strcatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text.Trim, "") 'IIf(UCase(ddlCategorycode.Items(ddlCategorycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCategorycode.Items(ddlCategorycode.SelectedIndex).Text, "")
            strcatcodeto = IIf(txtcatcode.Text <> "", txtcatcode.Text.Trim, "") ' IIf(UCase(ddlCategorycodeto.Items(ddlCategorycodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCategorycodeto.Items(ddlCategorycodeto.SelectedIndex).Text, "")


            strfromctry = IIf(txtctrycode.Text <> "", txtctrycode.Text.Trim, "") 'IIf(UCase(ddlCountrycode.Items(ddlCountrycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCountrycode.Items(ddlCountrycode.SelectedIndex).Text, "")
            strtoctry = IIf(txtctrycode.Text <> "", txtctrycode.Text.Trim, "") 'IIf(UCase(ddlCountrycodeto.Items(ddlCountrycodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCountrycodeto.Items(ddlCountrycodeto.SelectedIndex).Text, "")

            strfromcity = IIf(txtcitycode.Text <> "", txtcitycode.Text.Trim, "") 'IIf(UCase(ddlCitycode.Items(ddlCitycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCitycode.Items(ddlCitycode.SelectedIndex).Text, "")
            strtocity = IIf(txtcitycode.Text <> "", txtcitycode.Text.Trim, "") ' IIf(UCase(ddlCitycodeto.Items(ddlCitycodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCitycodeto.Items(ddlCitycodeto.SelectedIndex).Text, "")

            Dim custgroup_sp_type As String

            custgroup_sp_type = txtsuptypecode.Text



            strglcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text.Trim, "") 'IIf(UCase(ddlctrlcodefrm.Items(ddlctrlcodefrm.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlctrlcodefrm.Items(ddlctrlcodefrm.SelectedIndex).Text, "")
            strglcodeto = IIf(TxtBankCode.Text <> "", TxtBankCode.Text.Trim, "") ' IIf(UCase(ddlctrlcodeto.Items(ddlctrlcodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlctrlcodeto.Items(ddlctrlcodeto.SelectedIndex).Text, "")
            strcurrtype = IIf(ddlcurrency.SelectedIndex = 0, 1, 0)
            strorderby = ddlrptord.SelectedIndex
            strincludezero = ddlinclzero.SelectedIndex
            strgpby = ddlgpby.SelectedIndex
            strtrialtype = "TB"

            If ViewState("RptSupplierTrialBalanceTranType") = "WODR" Then
                withCredit = 0
            ElseIf ViewState("RptSupplierTrialBalanceTranType") = "WTDR" Then
                withCredit = 1
            End If


            'Response.Redirect("rptsupptrialbalReport.aspx?type=S&fromdate=" & strfromdate & "&todate=" & strtodate _
            '& "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcity=" & strfromcity & "&tocity=" & strtocity & "&movflg=" & strmovflag & "&acctype=" & stracctype & "&fromcode=" & strfromcode & "&tocode =" & strtocode _
            '& "&fromsptype=" & strsuptypecode & "&tosptype=" & strsuptypecodeto & "&fromcat=" & strcatcode _
            '& "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto _
            '& "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby, False)


            Dim strpop As String = ""
            'strpop = "window.open('rptsupptrialbalReport.aspx?type=C&frmdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&movflg=" & strmovflag & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & "','RepRVPVReg','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ' strpop = "window.open('rptsupptrialbalReport.aspx?type=S&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcity=" & strfromcity & "&tocity=" & strtocity & "&movflg=" & strmovflag & "&acctype=" & stracctype & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&fromsptype=" & strsuptypecode & "&tosptype=" & strsuptypecodeto & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & " &trialtype=" & strtrialtype & "','RepSuppBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptsupptrialbalReport.aspx?type=S&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcity=" & strfromcity & "&tocity=" & strtocity & "&movflg=" & strmovflag & "&acctype=" & stracctype & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&fromsptype=" & strsuptypecode & "&fromtosptype=" & strsuptypecode & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & " &trialtype=" & strtrialtype & " &divid=" & divcode & "&custgroup_sp_type=" & custgroup_sp_type & "','RepSuppBalance');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptsuppliertrialbalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try


    End Sub


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ViewState("RptSupplierTrialBalanceTranType") = "WODR" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptSupplierTrialBalance','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        ElseIf ViewState("RptSupplierTrialBalanceTranType") = "WTDR" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptSupplierwithDebitBalance','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        End If
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
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtFromDate.ClientID + "');", True)
                SetFocus(txtFromDate)
                ValidatePage = False
                Exit Function
            End If
            If txtToDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date field can not be blank.');", True)
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                SetFocus(txttoDate)
                ValidatePage = False
                Exit Function
            End If
            If ddlwithmovmt.SelectedItem.Text <> "Balance" Then
                If CType(objDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objDate.ConvertDateromTextBoxToDatabase(txttoDate.Text), Date) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                    SetFocus(txttoDate)
                    ValidatePage = False
                    Exit Function
                End If
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

    Protected Sub ddlSupType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddlSupType.SelectedValue = "S" Then
            supptype = "S"
            txtsuppliercode.Text = ""
            txtsuppliername.Text = ""
        End If
        If ddlSupType.SelectedValue = "A" Then
            supptype = "A"
            txtsuppliercode.Text = ""
            txtsuppliername.Text = ""
        End If
    End Sub

    Protected Sub Button1_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try

            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(20) As SqlParameter

            If ValidatePage() = False Then
                Exit Sub
            End If
            ViewState.Add("Pageame", "SuppliertrialbalReport")
            ViewState.Add("BackPageName", "RptSupplierTrialBalance.aspx")



            Dim strReportTitle As String = ""

            Dim strfromdate As String = ""
            Dim strtodate As String = ""
            Dim strmovflag As String = ""
            Dim stracctype As String = ""
            Dim strfromcode As String = ""
            Dim strtocode As String = ""
            Dim strsuptypecode As String = ""
            Dim strsuptypecodeto As String = ""
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
            Dim strfromcity As String = ""
            Dim strtocity As String = ""
            Dim withCredit As String = ""

            strmovflag = ddlwithmovmt.SelectedValue


            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            strtodate = IIf(strmovflag = 0, Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10), strfromdate)

            stracctype = ddlSupType.SelectedValue

            strfromcode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text, "") 'IIf(UCase(ddlSuppliercode.Items(ddlSuppliercode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSuppliercode.Items(ddlSuppliercode.SelectedIndex).Text, "")
            strtocode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text, "") ' IIf(UCase(ddlSuppliercodeto.Items(ddlSuppliercodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSuppliercodeto.Items(ddlSuppliercodeto.SelectedIndex).Text, "")

            strsuptypecode = IIf(txtsuptypecode.Text <> "", txtsuppliercode.Text, "") ' IIf(UCase(ddlsuppliertypecodefrm.Items(ddlsuppliertypecodefrm.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlsuppliertypecodefrm.Items(ddlsuppliertypecodefrm.SelectedIndex).Text, "")
            strsuptypecodeto = IIf(txtsuptypecode.Text <> "", txtsuppliercode.Text, "") ' IIf(UCase(ddlsuppliertypecodeto.Items(ddlsuppliertypecodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlsuppliertypecodeto.Items(ddlsuppliertypecodeto.SelectedIndex).Text, "")
            strcatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "") 'IIf(UCase(ddlCategorycode.Items(ddlCategorycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCategorycode.Items(ddlCategorycode.SelectedIndex).Text, "")
            strcatcodeto = IIf(txtcatcode.Text <> "", txtcatcode.Text, "") 'IIf(UCase(ddlCategorycodeto.Items(ddlCategorycodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCategorycodeto.Items(ddlCategorycodeto.SelectedIndex).Text, "")


            strfromctry = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") 'IIf(UCase(ddlCountrycode.Items(ddlCountrycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCountrycode.Items(ddlCountrycode.SelectedIndex).Text, "")
            strtoctry = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") 'IIf(UCase(ddlCountrycodeto.Items(ddlCountrycodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCountrycodeto.Items(ddlCountrycodeto.SelectedIndex).Text, "")

            strfromcity = IIf(txtcitycode.Text <> "", txtcitycode.Text, "") ' IIf(UCase(ddlCitycode.Items(ddlCitycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCitycode.Items(ddlCitycode.SelectedIndex).Text, "")
            strtocity = IIf(txtcitycode.Text <> "", txtcitycode.Text, "") 'IIf(UCase(ddlCitycodeto.Items(ddlCitycodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCitycodeto.Items(ddlCitycodeto.SelectedIndex).Text, "")

            strglcode = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") ' IIf(UCase(ddlctrlcodefrm.Items(ddlctrlcodefrm.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlctrlcodefrm.Items(ddlctrlcodefrm.SelectedIndex).Text, "")
            strglcodeto = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") ' IIf(UCase(ddlctrlcodeto.Items(ddlctrlcodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlctrlcodeto.Items(ddlctrlcodeto.SelectedIndex).Text, "")
            strcurrtype = IIf(ddlcurrency.SelectedIndex = 0, 1, 0)
            strorderby = ddlrptord.SelectedIndex
            strincludezero = ddlinclzero.SelectedIndex
            strgpby = ddlgpby.SelectedIndex

            If ViewState("RptSupplierTrialBalanceTranType") = "WODR" Then
                withCredit = 0
            ElseIf ViewState("RptSupplierTrialBalanceTranType") = "WTDR" Then
                withCredit = 1
            End If


            parm(0) = New SqlParameter("@fromdate", strfromdate)
            parm(1) = New SqlParameter("@todate", strtodate)
            parm(2) = New SqlParameter("@movflg", strmovflag)
            parm(3) = New SqlParameter("@acctype", stracctype)
            parm(4) = New SqlParameter("@fromcode", strfromcode)
            parm(5) = New SqlParameter("@tocode", strtocode)
            parm(6) = New SqlParameter("@fromsptype", strsuptypecode)
            parm(7) = New SqlParameter("@tosptype", strsuptypecodeto)
            parm(8) = New SqlParameter("@fromctry", strfromctry)
            parm(9) = New SqlParameter("@toctry", strtoctry)
            parm(10) = New SqlParameter("@fromcity", strfromcity)
            parm(11) = New SqlParameter("@tocity", strtocity)
            parm(12) = New SqlParameter("@fromcat", strcatcode)
            parm(13) = New SqlParameter("@tocat", strcatcodeto)
            parm(14) = New SqlParameter("@fromglcode", strglcode)
            parm(15) = New SqlParameter("@toglcode", strglcodeto)
            parm(16) = New SqlParameter("@currtype", strcurrtype)
            parm(17) = New SqlParameter("@orderby", strorderby)
            parm(18) = New SqlParameter("@includezero", strincludezero)
            parm(19) = New SqlParameter("@withCredit", withCredit)
            parm(20) = New SqlParameter("@divcode", divcode)


            For i = 0 To 20
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_supplier_trialbal_xls", parms)

            If ds.Tables.Count > 0 Then

                If ds.Tables(0).Rows.Count > 0 Then
                    objUtils.ExportToExcel(ds, Response)
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Record found' );", True)
                End If

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptsuppliertrialbalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub
    Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
        Try
            'If ValidatePage() = True Then

            If ValidatePage() = False Then
                Exit Sub
            End If
            ViewState.Add("Pageame", "SuppliertrialbalReport")
            ViewState.Add("BackPageName", "RptSupplierTrialBalance.aspx")



            Dim strReportTitle As String = ""

            Dim strfromdate As String = ""
            Dim strtodate As String = ""
            Dim strmovflag As String = ""
            Dim stracctype As String = ""
            Dim strfromcode As String = ""
            Dim strtocode As String = ""
            Dim strsuptypecode As String = ""
            Dim strsuptypecodeto As String = ""
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
            Dim strfromcity As String = ""
            Dim strtocity As String = ""
            Dim withCredit As String = ""
            Dim strtrialtype As String = ""

            strmovflag = ddlwithmovmt.SelectedValue


            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            strtodate = IIf(strmovflag = 0, Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10), strfromdate)

            stracctype = ddlSupType.SelectedValue

            strfromcode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text.Trim, "") 'IIf(UCase(ddlSuppliercode.Items(ddlSuppliercode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSuppliercode.Items(ddlSuppliercode.SelectedIndex).Text, "")
            strtocode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text.Trim, "") ' IIf(UCase(ddlSuppliercodeto.Items(ddlSuppliercodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSuppliercodeto.Items(ddlSuppliercodeto.SelectedIndex).Text, "")

            strsuptypecode = IIf(txtsuptypecode.Text <> "", txtsuptypecode.Text.Trim, "") ' IIf(UCase(ddlsuppliertypecodefrm.Items(ddlsuppliertypecodefrm.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlsuppliertypecodefrm.Items(ddlsuppliertypecodefrm.SelectedIndex).Text, "")
            strsuptypecodeto = IIf(txtsuptypecode.Text <> "", txtsuptypecode.Text.Trim, "") 'IIf(UCase(ddlsuppliertypecodeto.Items(ddlsuppliertypecodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlsuppliertypecodeto.Items(ddlsuppliertypecodeto.SelectedIndex).Text, "")
            strcatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text.Trim, "") 'IIf(UCase(ddlCategorycode.Items(ddlCategorycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCategorycode.Items(ddlCategorycode.SelectedIndex).Text, "")
            strcatcodeto = IIf(txtcatcode.Text <> "", txtcatcode.Text.Trim, "") ' IIf(UCase(ddlCategorycodeto.Items(ddlCategorycodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCategorycodeto.Items(ddlCategorycodeto.SelectedIndex).Text, "")


            strfromctry = IIf(txtctrycode.Text <> "", txtctrycode.Text.Trim, "") 'IIf(UCase(ddlCountrycode.Items(ddlCountrycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCountrycode.Items(ddlCountrycode.SelectedIndex).Text, "")
            strtoctry = IIf(txtctrycode.Text <> "", txtctrycode.Text.Trim, "") 'IIf(UCase(ddlCountrycodeto.Items(ddlCountrycodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCountrycodeto.Items(ddlCountrycodeto.SelectedIndex).Text, "")

            strfromcity = IIf(txtcitycode.Text <> "", txtcitycode.Text.Trim, "") 'IIf(UCase(ddlCitycode.Items(ddlCitycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCitycode.Items(ddlCitycode.SelectedIndex).Text, "")
            strtocity = IIf(txtcitycode.Text <> "", txtcitycode.Text.Trim, "") ' IIf(UCase(ddlCitycodeto.Items(ddlCitycodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCitycodeto.Items(ddlCitycodeto.SelectedIndex).Text, "")

            Dim custgroup_sp_type As String

            custgroup_sp_type = txtsuptypecode.Text



            strglcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text.Trim, "") 'IIf(UCase(ddlctrlcodefrm.Items(ddlctrlcodefrm.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlctrlcodefrm.Items(ddlctrlcodefrm.SelectedIndex).Text, "")
            strglcodeto = IIf(TxtBankCode.Text <> "", TxtBankCode.Text.Trim, "") ' IIf(UCase(ddlctrlcodeto.Items(ddlctrlcodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlctrlcodeto.Items(ddlctrlcodeto.SelectedIndex).Text, "")
            strcurrtype = IIf(ddlcurrency.SelectedIndex = 0, 1, 0)
            strorderby = ddlrptord.SelectedIndex
            strincludezero = ddlinclzero.SelectedIndex
            strgpby = ddlgpby.SelectedIndex
            strtrialtype = "TB"

            If ViewState("RptSupplierTrialBalanceTranType") = "WODR" Then
                withCredit = 0
            ElseIf ViewState("RptSupplierTrialBalanceTranType") = "WTDR" Then
                withCredit = 1
            End If


            'Response.Redirect("rptsupptrialbalReport.aspx?type=S&fromdate=" & strfromdate & "&todate=" & strtodate _
            '& "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcity=" & strfromcity & "&tocity=" & strtocity & "&movflg=" & strmovflag & "&acctype=" & stracctype & "&fromcode=" & strfromcode & "&tocode =" & strtocode _
            '& "&fromsptype=" & strsuptypecode & "&tosptype=" & strsuptypecodeto & "&fromcat=" & strcatcode _
            '& "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto _
            '& "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby, False)


            Dim strpop As String = ""
            'strpop = "window.open('rptsupptrialbalReport.aspx?type=C&frmdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&movflg=" & strmovflag & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & "','RepRVPVReg','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ' strpop = "window.open('rptsupptrialbalReport.aspx?type=S&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcity=" & strfromcity & "&tocity=" & strtocity & "&movflg=" & strmovflag & "&acctype=" & stracctype & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&fromsptype=" & strsuptypecode & "&tosptype=" & strsuptypecodeto & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & " &trialtype=" & strtrialtype & "','RepSuppBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('TransactionReports.aspx?type=S&printid=SupplierTrial&reportsType=pdf&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromctryname=" & txtctryname.Text & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcityname=" & txtcityname.Text & "&fromcity=" & strfromcity & "&tocity=" & strtocity & "&movflg=" & strmovflag & "&acctype=" & stracctype & "&fromname=" & txtsuppliername.Text & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&fromsptype=" & strsuptypecode & "&fromtosptype=" & strsuptypecode & "&fromcatname=" & txtcatname.Text & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglname=" & TxtBankName.Text & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & " &trialtype=" & strtrialtype & " &divid=" & divcode & "&custgroup_sp_type=" & custgroup_sp_type & "','RepSuppBalance');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptsuppliertrialbalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try


    End Sub

    Protected Sub btnExcelReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelReport.Click
        Try
            'If ValidatePage() = True Then

            If ValidatePage() = False Then
                Exit Sub
            End If
            ViewState.Add("Pageame", "SuppliertrialbalReport")
            ViewState.Add("BackPageName", "RptSupplierTrialBalance.aspx")



            Dim strReportTitle As String = ""

            Dim strfromdate As String = ""
            Dim strtodate As String = ""
            Dim strmovflag As String = ""
            Dim stracctype As String = ""
            Dim strfromcode As String = ""
            Dim strtocode As String = ""
            Dim strsuptypecode As String = ""
            Dim strsuptypecodeto As String = ""
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
            Dim strfromcity As String = ""
            Dim strtocity As String = ""
            Dim withCredit As String = ""
            Dim strtrialtype As String = ""

            strmovflag = ddlwithmovmt.SelectedValue


            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            strtodate = IIf(strmovflag = 0, Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10), strfromdate)

            stracctype = ddlSupType.SelectedValue

            strfromcode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text.Trim, "") 'IIf(UCase(ddlSuppliercode.Items(ddlSuppliercode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSuppliercode.Items(ddlSuppliercode.SelectedIndex).Text, "")
            strtocode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text.Trim, "") ' IIf(UCase(ddlSuppliercodeto.Items(ddlSuppliercodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSuppliercodeto.Items(ddlSuppliercodeto.SelectedIndex).Text, "")

            strsuptypecode = IIf(txtsuptypecode.Text <> "", txtsuptypecode.Text.Trim, "") ' IIf(UCase(ddlsuppliertypecodefrm.Items(ddlsuppliertypecodefrm.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlsuppliertypecodefrm.Items(ddlsuppliertypecodefrm.SelectedIndex).Text, "")
            strsuptypecodeto = IIf(txtsuptypecode.Text <> "", txtsuptypecode.Text.Trim, "") 'IIf(UCase(ddlsuppliertypecodeto.Items(ddlsuppliertypecodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlsuppliertypecodeto.Items(ddlsuppliertypecodeto.SelectedIndex).Text, "")
            strcatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text.Trim, "") 'IIf(UCase(ddlCategorycode.Items(ddlCategorycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCategorycode.Items(ddlCategorycode.SelectedIndex).Text, "")
            strcatcodeto = IIf(txtcatcode.Text <> "", txtcatcode.Text.Trim, "") ' IIf(UCase(ddlCategorycodeto.Items(ddlCategorycodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCategorycodeto.Items(ddlCategorycodeto.SelectedIndex).Text, "")


            strfromctry = IIf(txtctrycode.Text <> "", txtctrycode.Text.Trim, "") 'IIf(UCase(ddlCountrycode.Items(ddlCountrycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCountrycode.Items(ddlCountrycode.SelectedIndex).Text, "")
            strtoctry = IIf(txtctrycode.Text <> "", txtctrycode.Text.Trim, "") 'IIf(UCase(ddlCountrycodeto.Items(ddlCountrycodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCountrycodeto.Items(ddlCountrycodeto.SelectedIndex).Text, "")

            strfromcity = IIf(txtcitycode.Text <> "", txtcitycode.Text.Trim, "") 'IIf(UCase(ddlCitycode.Items(ddlCitycode.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCitycode.Items(ddlCitycode.SelectedIndex).Text, "")
            strtocity = IIf(txtcitycode.Text <> "", txtcitycode.Text.Trim, "") ' IIf(UCase(ddlCitycodeto.Items(ddlCitycodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlCitycodeto.Items(ddlCitycodeto.SelectedIndex).Text, "")

            Dim custgroup_sp_type As String

            custgroup_sp_type = txtsuptypecode.Text



            strglcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text.Trim, "") 'IIf(UCase(ddlctrlcodefrm.Items(ddlctrlcodefrm.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlctrlcodefrm.Items(ddlctrlcodefrm.SelectedIndex).Text, "")
            strglcodeto = IIf(TxtBankCode.Text <> "", TxtBankCode.Text.Trim, "") ' IIf(UCase(ddlctrlcodeto.Items(ddlctrlcodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlctrlcodeto.Items(ddlctrlcodeto.SelectedIndex).Text, "")
            strcurrtype = IIf(ddlcurrency.SelectedIndex = 0, 1, 0)
            strorderby = ddlrptord.SelectedIndex
            strincludezero = ddlinclzero.SelectedIndex
            strgpby = ddlgpby.SelectedIndex
            strtrialtype = "TB"

            If ViewState("RptSupplierTrialBalanceTranType") = "WODR" Then
                withCredit = 0
            ElseIf ViewState("RptSupplierTrialBalanceTranType") = "WTDR" Then
                withCredit = 1
            End If


            'Response.Redirect("rptsupptrialbalReport.aspx?type=S&fromdate=" & strfromdate & "&todate=" & strtodate _
            '& "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcity=" & strfromcity & "&tocity=" & strtocity & "&movflg=" & strmovflag & "&acctype=" & stracctype & "&fromcode=" & strfromcode & "&tocode =" & strtocode _
            '& "&fromsptype=" & strsuptypecode & "&tosptype=" & strsuptypecodeto & "&fromcat=" & strcatcode _
            '& "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto _
            '& "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby, False)


            Dim strpop As String = ""
            'strpop = "window.open('rptsupptrialbalReport.aspx?type=C&frmdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&movflg=" & strmovflag & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & "','RepRVPVReg','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ' strpop = "window.open('rptsupptrialbalReport.aspx?type=S&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcity=" & strfromcity & "&tocity=" & strtocity & "&movflg=" & strmovflag & "&acctype=" & stracctype & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&fromsptype=" & strsuptypecode & "&tosptype=" & strsuptypecodeto & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & " &trialtype=" & strtrialtype & "','RepSuppBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('TransactionReports.aspx?type=S&printid=SupplierTrial&reportsType=excel&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromctryname=" & txtctryname.Text & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&fromcityname=" & txtcityname.Text & "&fromcity=" & strfromcity & "&tocity=" & strtocity & "&movflg=" & strmovflag & "&acctype=" & stracctype & "&fromname=" & txtsuppliername.Text & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&fromsptype=" & strsuptypecode & "&fromtosptype=" & strsuptypecode & "&fromcatname=" & txtcatname.Text & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglname=" & TxtBankName.Text & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & " &trialtype=" & strtrialtype & " &divid=" & divcode & "&custgroup_sp_type=" & custgroup_sp_type & "','RepSuppBalance');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)


            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptsuppliertrialbalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try


    End Sub
End Class
