'------------================--------------=======================------------------================
'   Module Name    :    RptSupplierStatementOfAccount.aspx
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

Partial Class RptSupplierStatementOfAccount
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDate As New clsDateTime
    Dim strappid As String = ""
    Dim strappname As String = ""
    Private Shared divcode As String = ""
    Private Shared suptype As String = ""
#End Region
    <System.Web.Script.Services.ScriptMethod()> _
       <System.Web.Services.WebMethod()> _
    Public Shared Function Getbankslist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim acctnames As New List(Of String)
        Dim divid As String = ""
        Try


            strSqlQry = "select acctname,acctcode from acctmast where controlyn='Y'   and div_code='" & divcode & "' and acctname like  '" & Trim(prefixText) & "%' order by acctcode"
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
        type = ""
        custagent = ""

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

            If suptype = "S" Then
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

            ElseIf suptype = "A" Then
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
                                               CType(strappname, String), "AccountsModule\RptSupplierStatementOfAccount.aspx?appid=" + strappid, btnadd, Button1, btnReport, gv_SearchResult)
        End If

        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
        '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<    
        ViewState.Add("divcode", divid)
        divcode = ViewState("divcode")
        suptype = "S"
        ' Session.Add("Suptype", "S")
        txtpartycode.Attributes.Add("readonly", "readonly")
        TxtBankCode.Attributes.Add("readonly", "readonly")
        TxtBankCode.Attributes.Add("readonly", "readonly")
        txtcatcode.Attributes.Add("readonly", "readonly")
        txtcitycode.Attributes.Add("readonly", "readonly")
        txtctrycode.Attributes.Add("readonly", "readonly")
        Try
            If IsPostBack = False Then

                txtconnection.Value = Session("dbconnectionName")
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If


                If Request.QueryString("ageing_tran_type") Is Nothing = False Then
                    If Request.QueryString("ageing_tran_type") = "supplierageing1" Then
                        lblHeading.Text = "Supplier Pending Invoices"
                        ddlIncludeZero.Visible = False
                        lblIncludeZero.Visible = False
                        ddlSupType.Enabled = False
                        lblsupsupagnt.Text = "Enter Supplier"
                    End If
                    If Request.QueryString("ageing_tran_type") = "supplierageing2" Then
                        lblHeading.Text = "Supplier Pending Invoices - II"
                        ddlIncludeZero.Disabled = True
                        ddlSupType.Enabled = False
                        lblsupsupagnt.Text = "Enter Supplier"
                    End If
                End If
                'ddlAgeing.SelectedIndex = 1

                Dim strfromctrlcode1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select options_available from reservation_parameters where param_id=538")
                Dim strfromctrlname1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_value from reservation_parameters where param_id=538")

                Dim strtoctrlcode1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select options_available from reservation_parameters where param_id=539")
                Dim strtoctrlname1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_value from reservation_parameters where param_id=539")

                SetFocus(ddlSupType)
                '  hidecontrols()
                txtToDate.Visible = False
                ImgBtnRevDate.Visible = False
                lbltodate.Visible = False
                txtFromDate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy")
                txtToDate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy")

                txtagDate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy")
                '  txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")

                Dim c As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
                ddlCurrencyType.Items.Clear()
                ddlCurrencyType.Items.Add("A/C Currency")
                ddlCurrencyType.Items.Add(c)
                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format("U", CType(Request.QueryString("fromdate"), Date))
                End If
                If Request.QueryString("todate") <> "" Then
                    txtToDate.Text = Format("U", CType(Request.QueryString("todate"), Date))
                End If

                IIf(Request.QueryString("datetype") = 0, rdbtnAsOnDate.Checked = True, rdbtnFromToDate.Checked = True)
                If Request.QueryString("curr") <> "" Then
                    ddlCurrencyType.SelectedIndex = Request.QueryString("curr")
                End If


                If Request.QueryString("remarks") <> "" Then
                    txtRemark.Value = Request.QueryString("remarks")
                End If
                If Request.QueryString("agingtype") <> "" Then
                    ddlAgeing.SelectedIndex = Request.QueryString("agingtype")
                End If
                If Request.QueryString("includezero") <> "" Then
                    ddlIncludeZero.SelectedIndex = Request.QueryString("includezero")
                    IIf(Request.QueryString("includezero") = 0, ddlIncludeZero.SelectedIndex = 0, ddlIncludeZero.SelectedIndex = 1)
                End If
                If Request.QueryString("reporttype") <> "" Then
                    ddlLedgerType.Value = Request.QueryString("reporttype")
                End If
                If Request.QueryString("type") <> "" Then
                    ddlSupType.SelectedValue = Request.QueryString("type")
                End If

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlSupType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
            End If
            checkrb_status()

            ClientScript.GetPostBackEventReference(Me, String.Empty)
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepSupStmntWindowPostBack") Then
                btnReport_Click(sender, e)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Public Sub checkrb_status()
        'If rbSupall.Checked = True Then
        '    ddlFromSupplier.Disabled = True
        '    ddlFromSupplierName.Disabled = True
        '    ddlToSupplier.Disabled = True
        '    ddlToSupplierName.Disabled = True
        'Else
        '    ddlFromSupplier.Disabled = False
        '    ddlFromSupplierName.Disabled = False
        '    ddlToSupplier.Disabled = False
        '    ddlToSupplierName.Disabled = False
        'End If
        'If rbControlall.Checked = True Then
        '    ddlFromControl.Disabled = True
        '    ddlFromControlName.Disabled = True
        '    ddlToControl.Disabled = True
        '    ddlToControlName.Disabled = True
        'Else
        '    ddlFromControl.Disabled = False
        '    ddlFromControlName.Disabled = False
        '    ddlToControl.Disabled = False
        '    ddlToControlName.Disabled = False
        'End If
        'If rbCatall.Checked = True Then
        '    ddlFromCategory.Disabled = True
        '    ddlFromCategoryName.Disabled = True
        '    ddlToCategory.Disabled = True
        '    ddlToCategoryName.Disabled = True
        'Else
        '    ddlFromCategory.Disabled = False
        '    ddlFromCategoryName.Disabled = False
        '    ddlToCategory.Disabled = False
        '    ddlToCategoryName.Disabled = False
        'End If
        'If rbCityall.Checked = True Then
        '    ddlFromCity.Disabled = True
        '    ddlFromCityName.Disabled = True
        '    ddlToCity.Disabled = True
        '    ddlToCityName.Disabled = True
        'Else
        '    ddlFromCity.Disabled = False
        '    ddlFromCityName.Disabled = False
        '    ddlToCity.Disabled = False
        '    ddlToCityName.Disabled = False
        'End If
        'If rbCtrall.Checked = True Then
        '    ddlFromCountry.Disabled = True
        '    ddlFromCountryName.Disabled = True
        '    ddlToCountry.Disabled = True
        '    ddlToCountryName.Disabled = True
        'Else
        '    ddlFromCountry.Disabled = False
        '    ddlFromCountryName.Disabled = False
        '    ddlToCountry.Disabled = False
        '    ddlToCountryName.Disabled = False
        'End If
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Try

            If ValidatePage() = False Then
                Exit Sub
            End If

            'Session.Add("Pageame", "RptCustomerStatementAccount")
            'Session.Add("BackPageName", "RptCustomerStatementAccount.aspx")

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

            Dim strtype As String = "C"
            Dim strdatetype As Integer = 0 '0 as on date, 1 from -to date
            Dim stragingtype As Integer = 0
            Dim stragingreport As Integer = 0 '0-Supplier Statement ,1-Supplier Ageing Report-I, 2- Supplier Ageing Report-II
            Dim strpdcyesno As Integer = 0
            Dim strincludezero As Integer = 0 ' 0 no, 1 yes
            Dim strcurr As Integer = 0 ' 0 Ac Currency, 1 base currency
            Dim strreportorder As String = ""
            Dim strreporttype As String = ""
            Dim strrepfilter As String = ""
            Dim stragDate As String = ""
            Dim strincludeproforma As Integer = 0

            If rdbtnAsOnDate.Checked = True Then
                txtToDate.Text = txtFromDate.Text
            End If

            strdatetype = IIf(rdbtnAsOnDate.Checked = True, 0, 1)
            strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")  'Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            If Trim(txtToDate.Text) <> "" Then
                strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            Else
                strtodate = strfromdate
            End If
            'stragDate = Mid(Format(CType(txtagDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            If Trim(txtToDate.Text) <> "" Then
                stragDate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            Else
                stragDate = strfromdate
            End If


            strtype = IIf(UCase(ddlSupType.Items(ddlSupType.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSupType.Items(ddlSupType.SelectedIndex).Value, "")
            strcurr = Val(ddlCurrencyType.SelectedIndex)
            strfomaccode = IIf(txtpartycode.Text <> "", txtpartycode.Text, "")
            strtoaccode = IIf(txtpartycode.Text <> "", txtpartycode.Text, "")
            strfromctrlcode = IIf(TxtBankCode.Text <> " ", TxtBankCode.Text, " ")
            strtoctrlcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "")
            strfromccatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "")
            strtoccatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "")
            strfromcitycode = IIf(txtcitycode.Text <> "", txtcitycode.Text, "")
            strtocitycode = IIf(txtcitycode.Text <> "", txtcitycode.Text, "")
            strfromctrycode = IIf(txtctrycode.Text <> "", txtctrycode.Text, "")
            strtoctrycode = IIf(txtctrycode.Text <> "", txtctrycode.Text, "")

            stragingtype = Val(ddlAgeing.SelectedIndex)
            strpdcyesno = 0

            strincludeproforma = IIf(ddlproforma.Value = "Yes", 1, 0)
            ''strincludezero = IIf(ddlIncludeZero.SelectedIndex = 0, 0, 1)
            strincludezero = ddlIncludeZero.SelectedIndex

            strremarks = Trim(txtRemark.Value)
            strtype = IIf(UCase(ddlSupType.Items(ddlSupType.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSupType.Items(ddlSupType.SelectedIndex).Value, "")

            Dim custgroup_sp_type As String

            custgroup_sp_type = txtsuptypecode.Text

            strreporttype = Trim(ddlLedgerType.SelectedIndex)
            If Request.QueryString("ageing_tran_type") Is Nothing = False Then
                strtype = "S"
                strReportTitle = "Supplier Ageing Report"
                If Request.QueryString("ageing_tran_type") = "supplierageing1" Then
                    stragingreport = 1
                Else
                    stragingreport = 2
                End If
            Else
                If strtype = "S" Then
                    strReportTitle = "Supplier Statement"
                Else
                    strReportTitle = "Supplier Agent Statement"
                End If
            End If

            ''Response.Redirect("rptStatement.aspx?Pageame=RptSupplierStatementAccount&BackPageName=RptSupplierStatementOfAccount.aspx&fromdate=" & strfromdate & "&todate=" & strtodate _
            ''& "&datetype=" & strdatetype & "&type=" & strtype & "&curr=" & strcurr & "&fromaccode=" & strfomaccode _
            ''& "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
            ''& "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
            ''& "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
            ''& "&agingtype=" & stragingtype & "&pdcyesno=" & strpdcyesno _
            ''& "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype _
            ''& "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle, False)

            Dim strpop As String = ""
            '' strpop = "window.open('rptStatement.aspx?Pageame=RptSupplierStatementAccount&BackPageName=RptSupplierStatementOfAccount.aspx&fromdate=" & strfromdate & "&todate=" & strtodate _
            ''& "&datetype=" & strdatetype & "&type=" & strtype & "&curr=" & strcurr & "&fromaccode=" & strfomaccode _
            ''& "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
            ''& "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
            ''& "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
            ''& "&agingtype=" & stragingtype & "&pdcyesno=" & strpdcyesno _
            ''& "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype _
            ''    & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&ageingreporttyp=" & stragingreport & "&agdate=" & stragDate & "','RepSupStmnt','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"

            strpop = "window.open('rptStatement.aspx?Pageame=RptSupplierStatementAccount&BackPageName=RptSupplierStatementOfAccount.aspx&fromdate=" & strfromdate & "&todate=" & strtodate _
           & "&datetype=" & strdatetype & "&type=" & strtype & "&curr=" & strcurr & "&fromaccode=" & strfomaccode _
           & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
           & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
           & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode & "&strincludeproforma=" & strincludeproforma _
           & "&divid=" & divcode & "&custgroup_sp_type=" & custgroup_sp_type & "&agingtype=" & stragingtype & "&pdcyesno=" & strpdcyesno _
           & "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype _
               & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&ageingreporttyp=" & stragingreport & "&agdate=" & stragDate & "','RepSupStmnt');"




            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptSupplierStatementAccount.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub



    Protected Sub rdbtnAsOnDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lblasdate.Text = "As On Date"
        txtToDate.Enabled = False
        txtToDate.Visible = False
        ImgBtnRevDate.Visible = False
        lbltodate.Visible = False
    End Sub
    Protected Sub rdbtnFromToDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lblasdate.Text = "From Date"
        txtToDate.Visible = True
        txtToDate.Enabled = True
        ImgBtnRevDate.Visible = True
        lbltodate.Visible = True
        txtToDate.Text = objDate.GetSystemDateTime(Session("dbconnectionName")).Day & "/" & objDate.GetSystemDateTime(Session("dbconnectionName")).Month & "/" & objDate.GetSystemDateTime(Session("dbconnectionName")).Year
    End Sub
    Protected Sub hidecontrols()


    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptSupplierStatementofAccount','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
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
            If rdbtnFromToDate.Checked = True Then
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
            'Rosalin 21/11/2023 supplier should be selected.
            If txtpartyname.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Supplier field can not be blank.');", True)
                SetFocus(txtpartyname)
                ValidatePage = False
                Exit Function
            End If

            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Function

    Protected Sub ddlSupType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSupType.SelectedIndexChanged
        If ddlSupType.SelectedValue = "S" Then
            ' Session.Add("Suptype", "S")
            suptype = "S"
            txtpartycode.Text = ""
            txtpartyname.Text = ""
        End If
        If ddlSupType.SelectedValue = "A" Then
            suptype = "A"
            txtpartycode.Text = ""
            txtpartyname.Text = ""
            ' Session.Add("Suptype", "A")
        End If
    End Sub

    Protected Sub Button1_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(20) As SqlParameter
            If ValidatePage() = False Then
                Exit Sub
            End If

            'Session.Add("Pageame", "RptCustomerStatementAccount")
            'Session.Add("BackPageName", "RptCustomerStatementAccount.aspx")

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

            Dim strtype As String
            Dim strdatetype As Integer = 0 '0 as on date, 1 from -to date
            Dim stragingtype As Integer = 0
            Dim strpdcyesno As Integer = 0
            Dim strincludezero As Integer = 0 ' 0 no, 1 yes
            Dim strcurr As Integer = 0 ' 0 Ac Currency, 1 base currency
            Dim strreportorder As String = ""
            Dim strreporttype As String = ""
            Dim strrepfilter As String = ""
            Dim strrpttype As Integer = 0 ' 0-Summary,1-Detail 2-Pending Invoice

            strdatetype = IIf(rdbtnAsOnDate.Checked = True, 0, 1)
            strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")  'Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            If Trim(txtToDate.Text) <> "" Then
                strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            Else
                strtodate = strfromdate
            End If
            strtype = IIf(UCase(ddlSupType.Items(ddlSupType.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSupType.Items(ddlSupType.SelectedIndex).Value, "")
            strcurr = Val(ddlCurrencyType.SelectedIndex)
            strfomaccode = IIf(txtpartycode.Text <> "", txtpartycode.Text, "") ' IIf(UCase(ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text, "")
            strtoaccode = IIf(txtpartycode.Text <> "", txtpartycode.Text, "") 'IIf(UCase(ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text, "")
            strfromctrlcode = IIf(TxtBankCode.Text <> " ", TxtBankCode.Text, " ") 'IIf(UCase(ddlFromControl.Items(ddlFromControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromControl.Items(ddlFromControl.SelectedIndex).Text, "")
            strtoctrlcode = IIf(TxtBankCode.Text <> " ", TxtBankCode.Text, " ") 'IIf(UCase(ddlToControl.Items(ddlToControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToControl.Items(ddlToControl.SelectedIndex).Text, "")
            strfromccatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "") 'IIf(UCase(ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text, "")
            strtoccatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "") 'IIf(UCase(ddlToCategory.Items(ddlToCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCategory.Items(ddlToCategory.SelectedIndex).Text, "")
            strfromcitycode = IIf(txtcitycode.Text <> "", txtcitycode.Text, "") ' IIf(UCase(ddlFromCity.Items(ddlFromCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCity.Items(ddlFromCity.SelectedIndex).Text, "")
            strtocitycode = IIf(txtcitycode.Text <> "", txtcitycode.Text, "") ' IIf(UCase(ddlToCity.Items(ddlToCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCity.Items(ddlToCity.SelectedIndex).Text, "")
            strfromctrycode = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") ' IIf(UCase(ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text, "")
            strtoctrycode = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") 'IIf(UCase(ddlToCountry.Items(ddlToCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCountry.Items(ddlToCountry.SelectedIndex).Text, "")

            stragingtype = Val(ddlAgeing.SelectedIndex)
            strpdcyesno = 0
            strincludezero = IIf(ddlIncludeZero.SelectedIndex = 0, 0, 1)

            strremarks = Trim(txtRemark.Value)
            strtype = IIf(UCase(ddlSupType.Items(ddlSupType.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSupType.Items(ddlSupType.SelectedIndex).Value, "")
            Dim strweb As String = "0"
            strreporttype = Trim(ddlLedgerType.SelectedIndex)
            If Request.QueryString("ageing_tran_type") Is Nothing = False Then
                'If Request.QueryString("ageing_tran_type") = "supplierageing1" Then
                strrpttype = 2
                'Else
                'strrpttype = 0
                'End If
            Else
                strrpttype = 0
            End If

            parm(0) = New SqlParameter("@datetype", strdatetype)
            parm(1) = New SqlParameter("@fromdate", strfromdate)
            parm(2) = New SqlParameter("@todate", strtodate)
            parm(3) = New SqlParameter("@type", strtype)
            parm(4) = New SqlParameter("@currflg", strcurr)
            parm(5) = New SqlParameter("@fromacct", strfomaccode)
            parm(6) = New SqlParameter("@toacct", strtoaccode)
            parm(7) = New SqlParameter("@fromcontrol", strfromctrlcode)
            parm(8) = New SqlParameter("@tocontrol", strtoctrlcode)
            parm(9) = New SqlParameter("@fromcat", strfromccatcode)
            parm(10) = New SqlParameter("@tocat", strtoccatcode)
            parm(11) = New SqlParameter("@fromcity", strfromcitycode)
            parm(12) = New SqlParameter("@tocity", strtocitycode)
            parm(13) = New SqlParameter("@fromctry", strfromctrycode)
            parm(14) = New SqlParameter("@toctry", strtoctrycode)
            parm(15) = New SqlParameter("@agingtype", stragingtype)
            parm(16) = New SqlParameter("@pdcyesno", strpdcyesno)
            parm(17) = New SqlParameter("@includezero", strincludezero)
            parm(18) = New SqlParameter("@rpttype", strrpttype)
            parm(19) = New SqlParameter("@web", strweb)
            parm(20) = New SqlParameter("@divcode", divcode)


            For i = 0 To 20
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_statement_party_xls", parms)

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

            If ValidatePage() = False Then
                Exit Sub
            End If

            'Session.Add("Pageame", "RptCustomerStatementAccount")
            'Session.Add("BackPageName", "RptCustomerStatementAccount.aspx")

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

            Dim strtype As String = "C"
            Dim strdatetype As Integer = 0 '0 as on date, 1 from -to date
            Dim stragingtype As Integer = 0
            Dim stragingreport As Integer = 0 '0-Supplier Statement ,1-Supplier Ageing Report-I, 2- Supplier Ageing Report-II
            Dim strpdcyesno As Integer = 0
            Dim strincludezero As Integer = 0 ' 0 no, 1 yes
            Dim strcurr As Integer = 0 ' 0 Ac Currency, 1 base currency
            Dim strreportorder As String = ""
            Dim strreporttype As String = ""
            Dim strrepfilter As String = ""
            Dim stragDate As String = ""
            Dim strincludeproforma As Integer = 0

            If rdbtnAsOnDate.Checked = True Then
                txtToDate.Text = txtFromDate.Text
            End If

            strdatetype = IIf(rdbtnAsOnDate.Checked = True, 0, 1)
            strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")  'Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            If Trim(txtToDate.Text) <> "" Then
                strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            Else
                strtodate = strfromdate
            End If
            'stragDate = Mid(Format(CType(txtagDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            If Trim(txtToDate.Text) <> "" Then
                stragDate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            Else
                stragDate = strfromdate
            End If
            Dim reportfilter As String = ""
            If Trim(txtFromDate.Text) <> "" And Trim(txtToDate.Text) <> "" Then
                reportfilter = "From Date:" & Space(2) & Convert.ToDateTime(strfromdate).ToString("dd/MM/yyyy") & Space(2) & "To :" & Space(2) & Convert.ToDateTime(strtodate).ToString("dd/MM/yyyy")
            End If
            If Trim(txtsuptypename.Text) <> "" Then
                '  reportfilter = reportfilter & Space(2) & "Supplier Type From" & Space(2) & txtsuptypename.Text & Space(2) & "To" & Space(2) & txtsuptypename.Text
                reportfilter = reportfilter & Space(2) & "Supplier Type From:" & Space(2) & txtsuptypename.Text & Space(2)
            End If
            If Trim(txtpartyname.Text) <> "" Then
                ' reportfilter = reportfilter & Space(2) & "Supplier Name From" & Space(2) & txtpartyname.Text & Space(2) & "To" & Space(2) & txtsuptypename.Text
                reportfilter = reportfilter & Space(2) & "Supplier Name From :" & Space(2) & txtpartyname.Text & Space(2)
            End If
            If Trim(txtcityname.Text) <> "" Then

                ' reportfilter = reportfilter & Space(2) & ":City From" & Space(2) & txtcityname.Text & Space(2) & "To" & Space(2) & txtcityname.Text
                reportfilter = reportfilter & Space(2) & ":City From :" & Space(2) & txtcityname.Text & Space(2)
            End If

            If Trim(txtctryname.Text) <> "" Then
                '  reportfilter = reportfilter & Space(2) & "Country From" & Space(2) & txtctryname.Text & Space(2) & "To" & Space(2) & txtctryname.Text
                reportfilter = reportfilter & Space(2) & "Country From :" & Space(2) & txtctryname.Text & Space(2)
            End If
            If Trim(txtcatname.Text) <> "" Then
                '  reportfilter = reportfilter & Space(2) & "Category  From" & Space(2) & txtcatname.Text & Space(2) & "To" & Space(2) & txtcatname.Text
                reportfilter = reportfilter & Space(2) & "Category  From :" & Space(2) & txtcatname.Text & Space(2)
            End If
            If Trim(TxtBankName.Text) <> "" Then
                '  reportfilter = reportfilter & Space(2) & "Control Account From " & Space(2) & TxtBankName.Text & Space(2) & "To" & Space(2) & TxtBankName.Text
                reportfilter = reportfilter & Space(2) & "Control Account From  :" & Space(2) & TxtBankName.Text & Space(2)
            End If



            strtype = IIf(UCase(ddlSupType.Items(ddlSupType.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSupType.Items(ddlSupType.SelectedIndex).Value, "")
            strcurr = Val(ddlCurrencyType.SelectedIndex)
            strfomaccode = IIf(txtpartycode.Text <> "", txtpartycode.Text, "")
            strtoaccode = IIf(txtpartycode.Text <> "", txtpartycode.Text, "")
            strfromctrlcode = IIf(TxtBankCode.Text <> " ", TxtBankCode.Text, " ")
            strtoctrlcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "")
            strfromccatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "")
            strtoccatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "")
            strfromcitycode = IIf(txtcitycode.Text <> "", txtcitycode.Text, "")
            strtocitycode = IIf(txtcitycode.Text <> "", txtcitycode.Text, "")
            strfromctrycode = IIf(txtctrycode.Text <> "", txtctrycode.Text, "")
            strtoctrycode = IIf(txtctrycode.Text <> "", txtctrycode.Text, "")

            stragingtype = Val(ddlAgeing.SelectedIndex)
            strpdcyesno = 0

            strincludeproforma = IIf(ddlproforma.Value = "Yes", 1, 0)
            ''strincludezero = IIf(ddlIncludeZero.SelectedIndex = 0, 0, 1)
            strincludezero = ddlIncludeZero.SelectedIndex

            strremarks = Trim(txtRemark.Value)
            strtype = IIf(UCase(ddlSupType.Items(ddlSupType.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSupType.Items(ddlSupType.SelectedIndex).Value, "")

            Dim custgroup_sp_type As String

            custgroup_sp_type = txtsuptypecode.Text

            strreporttype = Trim(ddlLedgerType.SelectedIndex)
            If Request.QueryString("ageing_tran_type") Is Nothing = False Then
                strtype = "S"
                strReportTitle = "Supplier Ageing Report"
                If Request.QueryString("ageing_tran_type") = "supplierageing1" Then
                    stragingreport = 1
                Else
                    stragingreport = 2
                End If
            Else
                If strtype = "S" Then
                    strReportTitle = "Supplier Statement"
                Else
                    strReportTitle = "Supplier Agent Statement"
                End If
            End If

            ''Response.Redirect("rptStatement.aspx?Pageame=RptSupplierStatementAccount&BackPageName=RptSupplierStatementOfAccount.aspx&fromdate=" & strfromdate & "&todate=" & strtodate _
            ''& "&datetype=" & strdatetype & "&type=" & strtype & "&curr=" & strcurr & "&fromaccode=" & strfomaccode _
            ''& "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
            ''& "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
            ''& "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
            ''& "&agingtype=" & stragingtype & "&pdcyesno=" & strpdcyesno _
            ''& "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype _
            ''& "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle, False)

            Dim strpop As String = ""
            '' strpop = "window.open('rptStatement.aspx?Pageame=RptSupplierStatementAccount&BackPageName=RptSupplierStatementOfAccount.aspx&fromdate=" & strfromdate & "&todate=" & strtodate _
            ''& "&datetype=" & strdatetype & "&type=" & strtype & "&curr=" & strcurr & "&fromaccode=" & strfomaccode _
            ''& "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
            ''& "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
            ''& "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
            ''& "&agingtype=" & stragingtype & "&pdcyesno=" & strpdcyesno _
            ''& "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype _
            ''    & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&ageingreporttyp=" & stragingreport & "&agdate=" & stragDate & "','RepSupStmnt','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"

            strpop = "window.open('TransactionReports.aspx?Pageame=RptSupplierStatementAccount&BackPageName=RptSupplierStatementOfAccount.aspx&printId=SupplierStatement&reportsType=pdf&fromdate=" & strfromdate & "&todate=" & strtodate _
           & "&datetype=" & strdatetype & "&type=" & strtype & "&curr=" & strcurr & "&fromaccode=" & strfomaccode _
           & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
           & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
           & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode & "&strincludeproforma=" & strincludeproforma _
           & "&divid=" & divcode & "&custgroup_sp_type=" & custgroup_sp_type & "&agingtype=" & stragingtype & "&pdcyesno=" & strpdcyesno _
           & "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype _
               & "&repfilter=" & reportfilter & "&reporttitle=" & strReportTitle & "&ageingreporttyp=" & stragingreport & "&agdate=" & stragDate & "','RepSupStmnt');"




            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptSupplierStatementAccount.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

    Protected Sub btnExcelReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelReport.Click
        Try

            If ValidatePage() = False Then
                Exit Sub
            End If

            'Session.Add("Pageame", "RptCustomerStatementAccount")
            'Session.Add("BackPageName", "RptCustomerStatementAccount.aspx")

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

            Dim strtype As String = "C"
            Dim strdatetype As Integer = 0 '0 as on date, 1 from -to date
            Dim stragingtype As Integer = 0
            Dim stragingreport As Integer = 0 '0-Supplier Statement ,1-Supplier Ageing Report-I, 2- Supplier Ageing Report-II
            Dim strpdcyesno As Integer = 0
            Dim strincludezero As Integer = 0 ' 0 no, 1 yes
            Dim strcurr As Integer = 0 ' 0 Ac Currency, 1 base currency
            Dim strreportorder As String = ""
            Dim strreporttype As String = ""
            Dim strrepfilter As String = ""
            Dim stragDate As String = ""
            Dim strincludeproforma As Integer = 0

            If rdbtnAsOnDate.Checked = True Then
                txtToDate.Text = txtFromDate.Text
            End If

            strdatetype = IIf(rdbtnAsOnDate.Checked = True, 0, 1)
            strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")  'Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            If Trim(txtToDate.Text) <> "" Then
                strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            Else
                strtodate = strfromdate
            End If
            'stragDate = Mid(Format(CType(txtagDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            If Trim(txtToDate.Text) <> "" Then
                stragDate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            Else
                stragDate = strfromdate
            End If


            strtype = IIf(UCase(ddlSupType.Items(ddlSupType.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSupType.Items(ddlSupType.SelectedIndex).Value, "")
            strcurr = Val(ddlCurrencyType.SelectedIndex)
            strfomaccode = IIf(txtpartycode.Text <> "", txtpartycode.Text, "")
            strtoaccode = IIf(txtpartycode.Text <> "", txtpartycode.Text, "")
            strfromctrlcode = IIf(TxtBankCode.Text <> " ", TxtBankCode.Text, " ")
            strtoctrlcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "")
            strfromccatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "")
            strtoccatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "")
            strfromcitycode = IIf(txtcitycode.Text <> "", txtcitycode.Text, "")
            strtocitycode = IIf(txtcitycode.Text <> "", txtcitycode.Text, "")
            strfromctrycode = IIf(txtctrycode.Text <> "", txtctrycode.Text, "")
            strtoctrycode = IIf(txtctrycode.Text <> "", txtctrycode.Text, "")

            stragingtype = Val(ddlAgeing.SelectedIndex)
            strpdcyesno = 0

            strincludeproforma = IIf(ddlproforma.Value = "Yes", 1, 0)
            ''strincludezero = IIf(ddlIncludeZero.SelectedIndex = 0, 0, 1)
            strincludezero = ddlIncludeZero.SelectedIndex

            strremarks = Trim(txtRemark.Value)
            strtype = IIf(UCase(ddlSupType.Items(ddlSupType.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSupType.Items(ddlSupType.SelectedIndex).Value, "")

            Dim custgroup_sp_type As String

            custgroup_sp_type = txtsuptypecode.Text

            strreporttype = Trim(ddlLedgerType.SelectedIndex)
            If Request.QueryString("ageing_tran_type") Is Nothing = False Then
                strtype = "S"
                strReportTitle = "Supplier Ageing Report"
                If Request.QueryString("ageing_tran_type") = "supplierageing1" Then
                    stragingreport = 1
                Else
                    stragingreport = 2
                End If
            Else
                If strtype = "S" Then
                    strReportTitle = "Supplier Statement"
                Else
                    strReportTitle = "Supplier Agent Statement"
                End If
            End If
            Dim reportfilter As String = ""
            If Trim(txtFromDate.Text) <> "" And Trim(txtToDate.Text) <> "" Then
                reportfilter = "From Date :" & Space(2) & Convert.ToDateTime(strfromdate).ToString("dd/MM/yyyy") & Space(2) & "To :" & Space(2) & Convert.ToDateTime(strtodate).ToString("dd/MM/yyyy")
            End If
            If Trim(txtsuptypename.Text) <> "" Then
                '  reportfilter = reportfilter & Space(2) & "Supplier Name From" & Space(2) & txtsuptypename.Text & Space(2) & "To" & Space(2) & txtsuptypename.Text
                reportfilter = reportfilter & Space(2) & "Supplier Type  Name From :" & Space(2) & txtsuptypename.Text & Space(2)
            End If
            If Trim(txtpartyname.Text) <> "" Then
                '  reportfilter = reportfilter & Space(2) & "Supplier Name From" & Space(2) & txtsuptypename.Text & Space(2) & "To" & Space(2) & txtsuptypename.Text
                reportfilter = reportfilter & Space(2) & "Supplier Name From :" & Space(2) & txtpartyname.Text & Space(2)
            End If

            If Trim(txtcityname.Text) <> "" Then

                '     reportfilter = reportfilter & Space(2) & ":City From" & Space(2) & txtcityname.Text & Space(2) & "To" & Space(2) & txtcityname.Text
                reportfilter = reportfilter & Space(2) & "City From :" & Space(2) & txtcityname.Text & Space(2)
            End If

            If Trim(txtctryname.Text) <> "" Then
                ' reportfilter = reportfilter & Space(2) & "Country From" & Space(2) & txtctryname.Text & Space(2) & "To" & Space(2) & txtctryname.Text
                reportfilter = reportfilter & Space(2) & "Country From :" & Space(2) & txtctryname.Text & Space(2)
            End If
            If Trim(txtcatname.Text) <> "" Then
                ' reportfilter = reportfilter & Space(2) & "Category  From" & Space(2) & txtcatname.Text & Space(2) & "To" & Space(2) & txtcatname.Text
                reportfilter = reportfilter & Space(2) & "Category  From :" & Space(2) & txtcatname.Text & Space(2)
            End If
            If Trim(TxtBankName.Text) <> "" Then
                'reportfilter = reportfilter & Space(2) & "Control Account From " & Space(2) & TxtBankName.Text & Space(2) & "To" & Space(2) & TxtBankName.Text
                reportfilter = reportfilter & Space(2) & "Control Account From: " & Space(2) & TxtBankName.Text & Space(2)
            End If
            ''Response.Redirect("rptStatement.aspx?Pageame=RptSupplierStatementAccount&BackPageName=RptSupplierStatementOfAccount.aspx&fromdate=" & strfromdate & "&todate=" & strtodate _
            ''& "&datetype=" & strdatetype & "&type=" & strtype & "&curr=" & strcurr & "&fromaccode=" & strfomaccode _
            ''& "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
            ''& "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
            ''& "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
            ''& "&agingtype=" & stragingtype & "&pdcyesno=" & strpdcyesno _
            ''& "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype _
            ''& "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle, False)

            Dim strpop As String = ""
            '' strpop = "window.open('rptStatement.aspx?Pageame=RptSupplierStatementAccount&BackPageName=RptSupplierStatementOfAccount.aspx&fromdate=" & strfromdate & "&todate=" & strtodate _
            ''& "&datetype=" & strdatetype & "&type=" & strtype & "&curr=" & strcurr & "&fromaccode=" & strfomaccode _
            ''& "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
            ''& "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
            ''& "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
            ''& "&agingtype=" & stragingtype & "&pdcyesno=" & strpdcyesno _
            ''& "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype _
            ''    & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&ageingreporttyp=" & stragingreport & "&agdate=" & stragDate & "','RepSupStmnt','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"

            strpop = "window.open('TransactionReports.aspx?Pageame=RptSupplierStatementAccount&BackPageName=RptSupplierStatementOfAccount.aspx&printId=SupplierStatement&reportsType=excel&fromdate=" & strfromdate & "&todate=" & strtodate _
           & "&datetype=" & strdatetype & "&type=" & strtype & "&curr=" & strcurr & "&fromaccode=" & strfomaccode _
           & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
           & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
           & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode & "&strincludeproforma=" & strincludeproforma _
           & "&divid=" & divcode & "&custgroup_sp_type=" & custgroup_sp_type & "&agingtype=" & stragingtype & "&pdcyesno=" & strpdcyesno _
           & "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype _
               & "&repfilter=" & reportfilter & "&reporttitle=" & strReportTitle & "&ageingreporttyp=" & stragingreport & "&agdate=" & stragDate & "','RepSupStmnt');"




            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptSupplierStatementAccount.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

End Class
