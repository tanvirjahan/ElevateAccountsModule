'------------================--------------=======================------------------================
'   Module Name    :    RptSupplierAgeingSummary.aspx
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

Partial Class RptSupplierAgeingSummary
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
          
            txtconnection.Value = Session("dbconnectionName")
            If Request.QueryString("tran_type") = "suppliersummary" Then
                lblHeading.Text = "Supplier Ageing Summary"
                ViewState.Add("reporttype", "SupplierAgeingSummary")

            ElseIf Request.QueryString("tran_type") = "supplierdetail" Then
                lblHeading.Text = "Supplier Ageing Detail"
                ViewState.Add("reporttype", "SupplierAgeingDetail")

            ElseIf Request.QueryString("tran_type") = "customersummary" Then
                lblHeading.Text = "Customer Ageing Summary"
                ViewState.Add("reporttype", "CustomerAgeingSummary")
                lblsupplier.Visible = False
                ddlSupType.Visible = False
                Panel3.Visible = False
            ElseIf Request.QueryString("tran_type") = "customerdetail" Then
                lblHeading.Text = "Customer Ageing Detail"
                ViewState.Add("reporttype", "CustomerAgeingDetail")
                lblsupplier.Visible = False
                ddlSupType.Visible = False
                Panel3.Visible = False
            End If
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
                                                   CType(strappname, String), "AccountsModule\RptSupplierAgeingSummary.aspx?tran_type=" + tran_type + "&appid=" + strappid, btnadd, Button1, btnReport, gv_SearchResult)
            End If
            '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
            '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            ViewState.Add("divcode", divid)
            divcode = ViewState("divcode")
            supptype = "S"
            txtsuppliercode.Attributes.Add("readonly", "readonly")
            TxtBankCode.Attributes.Add("readonly", "readonly")
            TxtBankCode.Attributes.Add("readonly", "readonly")
            txtcatcode.Attributes.Add("readonly", "readonly")
            txtcitycode.Attributes.Add("readonly", "readonly")
            txtctrycode.Attributes.Add("readonly", "readonly")

            If ddlReportGroup.SelectedIndex = 0 Then
                ddlReportGroup.SelectedIndex = 1
            End If
            If ddlReportOrder.SelectedIndex = 0 Then
                ddlReportOrder.SelectedIndex = 1
            End If


            If IsPostBack = False Then
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If


                'If Request.QueryString("type").ToString = "S" Then
                '    lblHeading.Text = "Supplier Ageing Summary"
                'ElseIf Request.QueryString("type").ToString = "D" Then
                '    lblHeading.Text = "Supplier Ageing Detail"
                'End If
                ' hidecontrols()

                Dim strfromctrlcode1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select options_available from reservation_parameters where param_id=538")
                Dim strfromctrlname1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_value from reservation_parameters where param_id=538")

                Dim strtoctrlcode1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select options_available from reservation_parameters where param_id=539")
                Dim strtoctrlname1 As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_value from reservation_parameters where param_id=539")

                ' ddlAgeingType.SelectedIndex = 1
                SetFocus(ddlSupType)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrency, "currcode", "currcode", "select option_selected as currcode from reservation_parameters where param_id=457 union select 'A/C Currency'", False)

                '----------------------------- Default Dates
                txtFromDate.Text = Format(objDate.GetSystemDateTime(Session("dbconnectionName")), "dd/MM/yyyy") 'objDate.ConvertDateFromDatabaseToTextBoxFormat(ObjDate.GetSystemDateOnly(Session("dbconnectionName")))


                '   ddlSupType.Attributes.Add("onchange", "fillSup('" & ddlSupType.ClientID & "')")

                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format("U", CType(Request.QueryString("fromdate"), Date))
                End If

                If Request.QueryString("curr") <> "" Then
                    ddlCurrency.SelectedIndex = Request.QueryString("curr")
                End If

                If Request.QueryString("fromaccode") <> "" Then
                    txtsuppliername.Text = Request.QueryString("fromaccode")
                    ''    rbSuprange.Checked = True
                    ''    rbSupall.Checked = False
                    ''    ddlFromSupplierName.Value = Request.QueryString("fromaccode")
                    ''    ddlFromSupplier.Value = ddlFromSupplierName.Items(ddlFromSupplierName.SelectedIndex).Text
                End If
                If Request.QueryString("toaccode") <> "" Then
                    ''    rbSuprange.Checked = True
                    txtsuppliername.Text = Request.QueryString("toaccode")
                    ''    rbSupall.Checked = False
                    ''    ddlToSupplierName.Value = Request.QueryString("toaccode")
                    ''    ddlToSupplier.Value = ddlToSupplierName.Items(ddlToSupplierName.SelectedIndex).Text
                End If
                If Request.QueryString("fromctrlcode") <> "" Then
                    ''    rbControlrange.Checked = True
                    TxtBankName.Text = Request.QueryString("fromctrlcode")
                    ''    rbControlall.Checked = False
                    ''    ddlFromControlName.Value = Request.QueryString("fromctrlcode")
                    ''    ddlFromControl.Value = ddlFromControlName.Items(ddlFromControlName.SelectedIndex).Text
                End If
                If Request.QueryString("toctrlcode") <> "" Then
                    ''rbControlrange.Checked = True
                    ''rbControlall.Checked = False
                    TxtBankName.Text = Request.QueryString("toctrlcode")
                    ''ddlToControlName.Value = Request.QueryString("toctrlcode")
                    ''ddlToControl.Value = ddlToControlName.Items(ddlToControlName.SelectedIndex).Text
                End If
                If Request.QueryString("fromccatcode") <> "" Then
                    ''rbCatrange.Checked = True
                    txtcatname.Text = Request.QueryString("fromccatcode")
                    ''rbCatall.Checked = False
                    ''ddlFromCategoryName.Value = Request.QueryString("fromccatcode")
                    ''ddlFromCategory.Value = ddlFromCategoryName.Items(ddlFromCategoryName.SelectedIndex).Text
                End If
                If Request.QueryString("toccatcode") <> "" Then
                    ''rbCatrange.Checked = True
                    ''rbCatall.Checked = False
                    txtcatname.Text = Request.QueryString("toccatcode")
                    ''rbCatall.Checked = False
                    ''ddlToCategoryName.Value = Request.QueryString("toccatcode")
                    ''ddlToCategory.Value = ddlToCategoryName.Items(ddlToCategoryName.SelectedIndex).Text
                End If

                If Request.QueryString("fromcitycode") <> "" Then
                    ''rbCityrange.Checked = True
                    ''rbCityall.Checked = False
                    txtcityname.Text = Request.QueryString("fromcitycode")
                    ''ddlFromCityName.Value = Request.QueryString("fromcitycode")
                    ''ddlFromCity.Value = ddlFromCityName.Items(ddlFromCityName.SelectedIndex).Text
                End If

                If Request.QueryString("tocitycode") <> "" Then
                    ''rbCityrange.Checked = True
                    ''rbCityall.Checked = False
                    txtcityname.Text = Request.QueryString("tocitycode")
                    ''ddlToCityName.Value = Request.QueryString("tocitycode")
                    ''ddlToCity.Value = ddlToCityName.Items(ddlToCityName.SelectedIndex).Text
                End If

                If Request.QueryString("fromctrycode") <> "" Then
                    ''rbCtrrange.Checked = True
                    ''rbCtrall.Checked = False
                    txtctryname.Text = Request.QueryString("fromctrycode")
                    ''ddlFromCountryName.Value = Request.QueryString("fromctrycode")
                    ''ddlFromCountry.Value = ddlFromCountryName.Items(ddlFromCountryName.SelectedIndex).Text
                End If
                If Request.QueryString("toctrycode") <> "" Then
                    ''rbCtrrange.Checked = True
                    ''rbCtrall.Checked = False
                    ''ddlToCountryName.Value = Request.QueryString("toctrycode")
                    ''ddlToCountry.Value = ddlToCountryName.Items(ddlToCountryName.SelectedIndex).Text
                End If
                If Request.QueryString("agingtype") <> "" Then
                    ddlAgeingType.SelectedIndex = Request.QueryString("agingtype")
                End If

                If Request.QueryString("includezero") <> "" Then
                    ddlIncludeZero.SelectedIndex = Request.QueryString("includezero")
                End If
                If Request.QueryString("rptgroup") <> "" Then
                    ddlReportGroup.Value = Request.QueryString("rptgroup")
                End If
                If Request.QueryString("rptorder") <> "" Then
                    ddlReportOrder.Value = Request.QueryString("rptorder")
                End If


                If Request.QueryString("type") <> "" Then
                    ddlSupType.SelectedValue = Request.QueryString("type")
                End If
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlSupType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                    ddlReportGroup.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlReportOrder.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlIncludeZero.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlAgeingType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                btnReport.Attributes.Add("onclick", "return FormValidation()")
            End If
            checkrb_status()

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Public Sub checkrb_status()

    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
       <System.Web.Services.WebMethod()> _
    Public Shared Function GetCurrencylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Currencynames As New List(Of String)
        Try
            strSqlQry = "select currcode,currname from currmast where active=1 and  currname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Currencynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("currname").ToString(), myDS.Tables(0).Rows(i)("currcode").ToString()))

                Next
            End If
            Return Currencynames
        Catch ex As Exception
            Return Currencynames
        End Try

    End Function
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Dim frmdate As DateTime
        Dim MyCultureInfo As New CultureInfo("fr-Fr")
        Dim ds As DataSet

        Try

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
    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Try
            'Session.Add("Pageame", "RptSupplierAgingSummary")
            'Session.Add("BackPageName", "RptSupplierAgingSummary.aspx")
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
            Dim stragingreport As Integer = 0
            Dim strpdcyesno As Integer = 0
            Dim strincludezero As Integer = 0 ' 0 no, 1 yes
            Dim strcurr As Integer = 0 ' 0 Ac Currency, 1 base currency
            Dim strsumdet As Integer = 0 '0-Summary,1-Detail
            Dim strreportorder As String = "" ' 1 - Code, 2 Name

            Dim strsuppliertype As String = "" '1 - Cash, 2 - Credit
            Dim strincludeproforma As Integer = 0 ' 0 No, 1 Yes

            Dim strreporttype As String = "SupplierAgeingSummary"
            Dim strrepfilter As String = ""
            Dim custgroup_sp_type As String

            custgroup_sp_type = txtsuptypecode.Text
            strtodate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")
            If ValidatePage() = False Then
                Exit Sub
            End If

            If ViewState("reporttype") = "SupplierAgeingSummary" Or ViewState("reporttype") = "CustomerAgeingSummary" Then
                strreporttype = "SupplierAgeingSummary"
                strsumdet = 0
            ElseIf ViewState("reporttype") = "SupplierAgeingDetail" Or ViewState("reporttype") = "CustomerAgeingDetail" Then
                strreporttype = "SupplierAgeingDetail"
                strsumdet = 1
            End If
            strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")
            If ViewState("reporttype") = "CustomerAgeingDetail" Or ViewState("reporttype") = "CustomerAgeingSummary" Then
                strtype = "C"
            Else
                strtype = IIf(UCase(ddlSupType.Items(ddlSupType.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSupType.Items(ddlSupType.SelectedIndex).Value, "")
            End If
            strcurr = IIf(ddlCurrency.Value = "A/C Currency", 0, 1)
            stragingtype = ddlAgeingType.SelectedIndex
            strincludezero = ddlIncludeZero.SelectedIndex
            strfomaccode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text, "") ' IIf(UCase(ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text, "")
            strtoaccode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text, "") 'IIf(UCase(ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text, "")
            strfromctrlcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "") '(UCase(ddlFromControl.Items(ddlFromControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromControl.Items(ddlFromControl.SelectedIndex).Text, "")
            strtoctrlcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "") 'IIf(UCase(ddlToControl.Items(ddlToControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToControl.Items(ddlToControl.SelectedIndex).Text, "")
            strfromccatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "") 'IIf(UCase(ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text, "")
            strtoccatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "") 'IIf(UCase(ddlToCategory.Items(ddlToCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCategory.Items(ddlToCategory.SelectedIndex).Text, "")
            strfromcitycode = IIf(txtcitycode.Text <> "", txtcitycode.Text, "") ' IIf(UCase(ddlFromCity.Items(ddlFromCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCity.Items(ddlFromCity.SelectedIndex).Text, "")
            strtocitycode = IIf(txtcitycode.Text <> "", txtcitycode.Text, "") 'IIf(UCase(ddlToCity.Items(ddlToCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCity.Items(ddlToCity.SelectedIndex).Text, "")
            strfromctrycode = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") 'IIf(UCase(ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text, "")
            strtoctrycode = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") 'IIf(UCase(ddlToCountry.Items(ddlToCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCountry.Items(ddlToCountry.SelectedIndex).Text, "")
            strpdcyesno = 0
            strincludezero = IIf(ddlIncludeZero.SelectedIndex = 0, 1, 0)
            strgroupby = Val(ddlReportGroup.SelectedIndex)
            strreportorder = Trim(ddlReportOrder.SelectedIndex)

            strsuppliertype = Trim(ddlSupplierType.SelectedIndex)
            strincludeproforma = IIf(ddlproforma.SelectedIndex = 0, 1, 0)


            'Response.Redirect("rptStatement.aspx?Pageame=RptSupplierAgingSummary&BackPageName=RptSupplierAgeingSummary.aspx&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
            '& "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
            '& "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
            '& "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
            '& "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
            '& "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
            '& "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value, False)
            If ViewState("reporttype") = "CustomerAgeingSummary" Then
                Dim strpop As String = ""
                'strpop = "window.open('rptStatement.aspx?Pageame=RptSupplierAgingSummary&BackPageName=RptSupplierAgeingSummary.aspx&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                '& "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                '& "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                '& "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                '& "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
                '& "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                '& "&supptype=" & strsuppliertype _
                '& "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&ageingreporttyp=" & stragingreport & "','RepCustAgeingSum','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"


                strpop = "window.open('rptStatement.aspx?Pageame=RptSupplierAgingSummary&BackPageName=RptSupplierAgeingSummary.aspx&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                & "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                & "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
                & "&includezero=" & strincludezero & "&custgroup_sp_type=" & custgroup_sp_type & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                & "&supptype=" & strsuppliertype & "&strincludeproforma=" & strincludeproforma _
                & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&ageingreporttyp=" & stragingreport & "','RepCustAgeingSum');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf ViewState("reporttype") = "CustomerAgeingDetail" Then
                Dim strpop As String = ""
                'strpop = "window.open('rptStatement.aspx?Pageame=RptSupplierAgingSummary&BackPageName=RptSupplierAgeingSummary.aspx&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                '& "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                '& "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                '& "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                '& "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
                '& "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                '& "&supptype=" & strsuppliertype _
                '& "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&ageingreporttyp=" & stragingreport & "','RepCustAgeingDet','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"


                strpop = "window.open('rptStatement.aspx?Pageame=RptSupplierAgingSummary&BackPageName=RptSupplierAgeingSummary.aspx&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                & "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                & "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
                & "&includezero=" & strincludezero & "&custgroup_sp_type=" & custgroup_sp_type & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                & "&supptype=" & strsuppliertype & "&strincludeproforma=" & strincludeproforma _
                & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&ageingreporttyp=" & stragingreport & "','RepCustAgeingDet');"



                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf ViewState("reporttype") = "SupplierAgeingSummary" Then
                Dim strpop As String = ""
                'strpop = "window.open('rptStatement.aspx?Pageame=RptSupplierAgingSummary&BackPageName=RptSupplierAgeingSummary.aspx&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                '& "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                '& "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                '& "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                '& "&curr=" & strcurr & "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
                '& "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                '& "&supptype=" & strsuppliertype _
                '& "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&ageingreporttyp=" & stragingreport & "','RepSupAgeingSum','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"

                strpop = "window.open('rptStatement.aspx?Pageame=RptSupplierAgingSummary&BackPageName=RptSupplierAgeingSummary.aspx&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                & "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                & "&curr=" & strcurr & "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
                & "&includezero=" & strincludezero & "&custgroup_sp_type=" & custgroup_sp_type & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                & "&supptype=" & strsuppliertype & "&strincludeproforma=" & strincludeproforma _
                & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&ageingreporttyp=" & stragingreport & "&divid=" & divcode & " ','RepSupAgeingSum');"










                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf ViewState("reporttype") = "SupplierAgeingDetail" Then

                Dim strpop As String = ""
                'strpop = "window.open('rptStatement.aspx?Pageame=RptSupplierAgingSummary&BackPageName=RptSupplierAgeingSummary.aspx&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                '& "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                '& "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                '& "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                '& "&curr=" & strcurr & "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
                '& "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                '& "&supptype=" & strsuppliertype _
                '& "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&ageingreporttyp=" & stragingreport & "','RepSupAgeingDet','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"

                strpop = "window.open('rptStatement.aspx?Pageame=RptSupplierAgingSummary&BackPageName=RptSupplierAgeingSummary.aspx&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                & "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                & "&curr=" & strcurr & "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
                & "&includezero=" & strincludezero & "&custgroup_sp_type=" & custgroup_sp_type & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                & "&supptype=" & strsuppliertype & "&strincludeproforma=" & strincludeproforma _
                & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&ageingreporttyp=" & stragingreport & "&divid=" & divcode & "','RepSupAgeingDet');"






                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptSupplierAgeingSummary.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub



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
    Public Shared Function Getbankslist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim acctnames As New List(Of String)
        Dim divid As String = ""
        Try

            strSqlQry = "select acctname,acctcode from acctmast where controlyn='Y'  and div_code='" & divcode & "' and acctname like  '" & Trim(prefixText) & "%' order by acctcode"
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
    Protected Sub hidecontrols()
        'ddlFromSupplier.Style("visibility") = "hidden"
        'ddlFromSupplierName.Style("visibility") = "hidden"
        'ddlToSupplier.Style("visibility") = "hidden"
        'ddlToSupplierName.Style("visibility") = "hidden"

        ''ddlFromControl.Style("visibility") = "hidden"
        ''ddlFromControlName.Style("visibility") = "hidden"
        ''ddlToControl.Style("visibility") = "hidden"
        ''ddlToControlName.Style("visibility") = "hidden"

        ''ddlFromCity.Style("visibility") = "hidden"
        ''ddlFromCityName.Style("visibility") = "hidden"
        ''ddlToCity.Style("visibility") = "hidden"
        ''ddlToCityName.Style("visibility") = "hidden"

        ''ddlFromCountry.Style("visibility") = "hidden"
        ''ddlFromCountryName.Style("visibility") = "hidden"
        ''ddlToCountry.Style("visibility") = "hidden"
        ''ddlToCountryName.Style("visibility") = "hidden"

        ''ddlFromCategory.Style("visibility") = "hidden"
        ''ddlFromCategoryName.Style("visibility") = "hidden"
        ''ddlToCategory.Style("visibility") = "hidden"
        ''ddlToCategoryName.Style("visibility") = "hidden"

        ' ''lblacctfrom.Style("visibility") = "hidden"
        ' ''lblacctto.Style("visibility") = "hidden"
        ''lblcontrolfrom.Style("visibility") = "hidden"
        ''lblcontrolto.Style("visibility") = "hidden"
        ''lblmarketfrom.Style("visibility") = "hidden"
        ''lblmarketto.Style("visibility") = "hidden"
        ''lblcatfrom.Style("visibility") = "hidden"
        ''lblcatto.Style("visibility") = "hidden"
        ''lblctryfrom.Style("visibility") = "hidden"
        ''lblctryto.Style("visibility") = "hidden"

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptSupplierAgeingSummary','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ddlSupType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddlSupType.SelectedValue = "S" Then
            ' Session.Add("Suptype", "S")
            supptype = "S"
            txtsuppliercode.Text = ""
            txtsuppliername.Text = ""
        End If
        If ddlSupType.SelectedValue = "A" Then
            ' Session.Add("Suptype", "A")
            supptype = "A"
            txtsuppliercode.Text = ""
            txtsuppliername.Text = ""
        End If
    End Sub

    'Protected Sub ddlCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '     If ddlCurrency.Value = "A/C Currency" Then
    '        TxtCurrName.Visible = True
    '        lblcurrency.Visible = True

    '    Else
    '        TxtCurrName.Visible = False
    '        lblcurrency.Visible = False
    '    End If
    'End Sub

    Protected Sub Button1_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try

            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(15) As SqlParameter

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
                strreporttype = "SupplierAgeingSummary"
                strsumdet = 0
            ElseIf ViewState("reporttype") = "SupplierAgeingDetail" Or ViewState("reporttype") = "CustomerAgeingDetail" Then
                strreporttype = "SupplierAgeingDetail"
                strsumdet = 1
            End If
            strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")
            If ViewState("reporttype") = "CustomerAgeingDetail" Or ViewState("reporttype") = "CustomerAgeingSummary" Then
                strtype = "C"
            Else
                strtype = IIf(UCase(ddlSupType.Items(ddlSupType.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSupType.Items(ddlSupType.SelectedIndex).Value, "")
            End If
            strcurr = IIf(ddlCurrency.Value = "A/C Currency", 0, 1)
            stragingtype = ddlAgeingType.SelectedIndex
            strincludezero = ddlIncludeZero.SelectedIndex

            strfomaccode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text.Trim, "") 'IIf(UCase(ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text, "")
            strtoaccode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text.Trim, "") ' IIf(UCase(ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text, "")
            strfromctrlcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text.Trim, "") ' IIf(UCase(ddlFromControl.Items(ddlFromControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromControl.Items(ddlFromControl.SelectedIndex).Text, "")
            strtoctrlcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text.Trim, "") 'IIf(UCase(ddlToControl.Items(ddlToControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToControl.Items(ddlToControl.SelectedIndex).Text, "")
            strfromccatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text.Trim, "") ' IIf(UCase(ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text, "")
            strtoccatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text.Trim, "") 'IIf(UCase(ddlToCategory.Items(ddlToCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCategory.Items(ddlToCategory.SelectedIndex).Text, "")
            strfromcitycode = IIf(txtcitycode.Text <> "", txtcitycode.Text.Trim, "") ' IIf(UCase(ddlFromCity.Items(ddlFromCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCity.Items(ddlFromCity.SelectedIndex).Text, "")
            strtocitycode = IIf(txtcitycode.Text <> "", txtcitycode.Text.Trim, "") ' IIf(UCase(ddlToCity.Items(ddlToCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCity.Items(ddlToCity.SelectedIndex).Text, "")
            strfromctrycode = IIf(txtctrycode.Text <> "", txtctrycode.Text.Trim, "") ' IIf(UCase(ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text, "")
            strtoctrycode = IIf(txtctrycode.Text <> "", txtctrycode.Text.Trim, "") 'IIf(UCase(ddlToCountry.Items(ddlToCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCountry.Items(ddlToCountry.SelectedIndex).Text, "")
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
            parm(15) = New SqlParameter("@divcode", divcode)

            For i = 0 To 15
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_statement_partyaging_xls", parms)

            If ds.Tables.Count > 0 Then

                If ds.Tables(0).Rows.Count > 0 Then
                    objUtils.ExportToExcel(ds, Response)
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Record found' );", True)
                End If

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptSupplierAgeingSummary.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub
    Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
        Try
            'Session.Add("Pageame", "RptSupplierAgingSummary")
            'Session.Add("BackPageName", "RptSupplierAgingSummary.aspx")
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
            Dim stragingreport As Integer = 0
            Dim strpdcyesno As Integer = 0
            Dim strincludezero As Integer = 0 ' 0 no, 1 yes
            Dim strcurr As Integer = 0 ' 0 Ac Currency, 1 base currency
            Dim strsumdet As Integer = 0 '0-Summary,1-Detail
            Dim strreportorder As String = "" ' 1 - Code, 2 Name

            Dim strsuppliertype As String = "" '1 - Cash, 2 - Credit
            Dim strincludeproforma As Integer = 0 ' 0 No, 1 Yes

            Dim strreporttype As String = "SupplierAgeingSummary"
            Dim strrepfilter As String = ""
            Dim custgroup_sp_type As String

            custgroup_sp_type = txtsuptypecode.Text
            strtodate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")
            If ValidatePage() = False Then
                Exit Sub
            End If

            If ViewState("reporttype") = "SupplierAgeingSummary" Or ViewState("reporttype") = "CustomerAgeingSummary" Then
                strreporttype = "SupplierAgeingSummary"
                strsumdet = 0
            ElseIf ViewState("reporttype") = "SupplierAgeingDetail" Or ViewState("reporttype") = "CustomerAgeingDetail" Then
                strreporttype = "SupplierAgeingDetail"
                strsumdet = 1
            End If
            strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")
            If ViewState("reporttype") = "CustomerAgeingDetail" Or ViewState("reporttype") = "CustomerAgeingSummary" Then
                strtype = "C"
            Else
                strtype = IIf(UCase(ddlSupType.Items(ddlSupType.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSupType.Items(ddlSupType.SelectedIndex).Value, "")
            End If
            strcurr = IIf(ddlCurrency.Value = "A/C Currency", 0, 1)
            stragingtype = ddlAgeingType.SelectedIndex
            strincludezero = ddlIncludeZero.SelectedIndex
            strfomaccode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text, "") ' IIf(UCase(ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text, "")
            strtoaccode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text, "") 'IIf(UCase(ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text, "")
            strfromctrlcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "") '(UCase(ddlFromControl.Items(ddlFromControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromControl.Items(ddlFromControl.SelectedIndex).Text, "")
            strtoctrlcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "") 'IIf(UCase(ddlToControl.Items(ddlToControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToControl.Items(ddlToControl.SelectedIndex).Text, "")
            strfromccatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "") 'IIf(UCase(ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text, "")
            strtoccatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "") 'IIf(UCase(ddlToCategory.Items(ddlToCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCategory.Items(ddlToCategory.SelectedIndex).Text, "")
            strfromcitycode = IIf(txtcitycode.Text <> "", txtcitycode.Text, "") ' IIf(UCase(ddlFromCity.Items(ddlFromCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCity.Items(ddlFromCity.SelectedIndex).Text, "")
            strtocitycode = IIf(txtcitycode.Text <> "", txtcitycode.Text, "") 'IIf(UCase(ddlToCity.Items(ddlToCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCity.Items(ddlToCity.SelectedIndex).Text, "")
            strfromctrycode = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") 'IIf(UCase(ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text, "")
            strtoctrycode = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") 'IIf(UCase(ddlToCountry.Items(ddlToCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCountry.Items(ddlToCountry.SelectedIndex).Text, "")
            strpdcyesno = 0
            strincludezero = IIf(ddlIncludeZero.SelectedIndex = 0, 1, 0)
            strgroupby = Val(ddlReportGroup.SelectedIndex)
            strreportorder = Trim(ddlReportOrder.SelectedIndex)

            strsuppliertype = Trim(ddlSupplierType.SelectedIndex)
            strincludeproforma = IIf(ddlproforma.SelectedIndex = 0, 1, 0)
            Dim strcurrcode As String
            If strcurr = 0 Then
                strcurrcode = TxtCurrCode.Text
            Else
                strcurrcode = ""
            End If
            If ViewState("reporttype") = "CustomerAgeingSummary" Then
                Dim strpop As String = ""

                strpop = "window.open('TransactionReports.aspx?printId=CustomerAgeing&reportsType=pdf&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                & "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                & "&fromname=" & txtsuppliername.Text & "&frommkname=" & txtcityname.Text & "&fromctryname=" & txtctryname.Text & "&fromcatname=" & txtcatname.Text & "&glname=" & TxtBankName.Text _
                & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                & "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
                & "&includezero=" & strincludezero & "&custgroup_sp_type=" & custgroup_sp_type & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                & "&supptype=" & strsuppliertype & "&strincludeproforma=" & strincludeproforma _
                & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&ageingreporttyp=" & stragingreport & "','RepCustAgeingSum');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf ViewState("reporttype") = "CustomerAgeingDetail" Then
                Dim strpop As String = ""
                strpop = "window.open('TransactionReports.aspx?printId=CustomerAgeing&reportsType=pdf&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                & "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                 & "&fromname=" & txtsuppliername.Text & "&frommkname=" & txtcityname.Text & "&fromctryname=" & txtctryname.Text & "&fromcatname=" & txtcatname.Text & "&glname=" & TxtBankName.Text _
                & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                & "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
                & "&includezero=" & strincludezero & "&custgroup_sp_type=" & custgroup_sp_type & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                & "&supptype=" & strsuppliertype & "&strincludeproforma=" & strincludeproforma _
                & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&ageingreporttyp=" & stragingreport & "','RepCustAgeingDet');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf ViewState("reporttype") = "SupplierAgeingSummary" Then
                Dim strpop As String = ""
                strpop = "window.open('TransactionReports.aspx?printId=CustomerAgeing&reportsType=pdf&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                & "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                 & "&fromname=" & txtsuppliername.Text & "&frommkname=" & txtcityname.Text & "&fromctryname=" & txtctryname.Text & "&fromcatname=" & txtcatname.Text & "&glname=" & TxtBankName.Text _
                & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                & "&curr=" & strcurr & "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
                & "&includezero=" & strincludezero & "&custgroup_sp_type=" & custgroup_sp_type & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                & "&supptype=" & strsuppliertype & "&strincludeproforma=" & strincludeproforma _
                & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&ageingreporttyp=" & stragingreport & "&divid=" & divcode & "&currcode=" & strcurrcode & " ','RepSupAgeingSum');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf ViewState("reporttype") = "SupplierAgeingDetail" Then

                Dim strpop As String = ""
                strpop = "window.open('TransactionReports.aspx?printId=CustomerAgeing&reportsType=pdf&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                & "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                 & "&fromname=" & txtsuppliername.Text & "&frommkname=" & txtcityname.Text & "&fromctryname=" & txtctryname.Text & "&fromcatname=" & txtcatname.Text & "&glname=" & TxtBankName.Text _
                & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                & "&curr=" & strcurr & "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
                & "&includezero=" & strincludezero & "&custgroup_sp_type=" & custgroup_sp_type & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                & "&supptype=" & strsuppliertype & "&strincludeproforma=" & strincludeproforma _
                & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&ageingreporttyp=" & stragingreport & "&divid=" & divcode & "','RepSupAgeingDet');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptSupplierAgeingSummary.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

    Protected Sub btnExlReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExlReport.Click
        Try
            'Session.Add("Pageame", "RptSupplierAgingSummary")
            'Session.Add("BackPageName", "RptSupplierAgingSummary.aspx")
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
            Dim stragingreport As Integer = 0
            Dim strpdcyesno As Integer = 0
            Dim strincludezero As Integer = 0 ' 0 no, 1 yes
            Dim strcurr As Integer = 0 ' 0 Ac Currency, 1 base currency
            Dim strsumdet As Integer = 0 '0-Summary,1-Detail
            Dim strreportorder As String = "" ' 1 - Code, 2 Name

            Dim strsuppliertype As String = "" '1 - Cash, 2 - Credit
            Dim strincludeproforma As Integer = 0 ' 0 No, 1 Yes

            Dim strreporttype As String = "SupplierAgeingSummary"
            Dim strrepfilter As String = ""
            Dim custgroup_sp_type As String

            custgroup_sp_type = txtsuptypecode.Text
            strtodate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")
            If ValidatePage() = False Then
                Exit Sub
            End If

            If ViewState("reporttype") = "SupplierAgeingSummary" Or ViewState("reporttype") = "CustomerAgeingSummary" Then
                strreporttype = "SupplierAgeingSummary"
                strsumdet = 0
            ElseIf ViewState("reporttype") = "SupplierAgeingDetail" Or ViewState("reporttype") = "CustomerAgeingDetail" Then
                strreporttype = "SupplierAgeingDetail"
                strsumdet = 1
            End If
            strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")
            If ViewState("reporttype") = "CustomerAgeingDetail" Or ViewState("reporttype") = "CustomerAgeingSummary" Then
                strtype = "C"
            Else
                strtype = IIf(UCase(ddlSupType.Items(ddlSupType.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlSupType.Items(ddlSupType.SelectedIndex).Value, "")
            End If
            strcurr = IIf(ddlCurrency.Value = "A/C Currency", 0, 1)
            stragingtype = ddlAgeingType.SelectedIndex
            strincludezero = ddlIncludeZero.SelectedIndex
            strfomaccode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text, "") ' IIf(UCase(ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromSupplier.Items(ddlFromSupplier.SelectedIndex).Text, "")
            strtoaccode = IIf(txtsuppliercode.Text <> "", txtsuppliercode.Text, "") 'IIf(UCase(ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToSupplier.Items(ddlToSupplier.SelectedIndex).Text, "")
            strfromctrlcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "") '(UCase(ddlFromControl.Items(ddlFromControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromControl.Items(ddlFromControl.SelectedIndex).Text, "")
            strtoctrlcode = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "") 'IIf(UCase(ddlToControl.Items(ddlToControl.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToControl.Items(ddlToControl.SelectedIndex).Text, "")
            strfromccatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "") 'IIf(UCase(ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCategory.Items(ddlFromCategory.SelectedIndex).Text, "")
            strtoccatcode = IIf(txtcatcode.Text <> "", txtcatcode.Text, "") 'IIf(UCase(ddlToCategory.Items(ddlToCategory.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCategory.Items(ddlToCategory.SelectedIndex).Text, "")
            strfromcitycode = IIf(txtcitycode.Text <> "", txtcitycode.Text, "") ' IIf(UCase(ddlFromCity.Items(ddlFromCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCity.Items(ddlFromCity.SelectedIndex).Text, "")
            strtocitycode = IIf(txtcitycode.Text <> "", txtcitycode.Text, "") 'IIf(UCase(ddlToCity.Items(ddlToCity.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCity.Items(ddlToCity.SelectedIndex).Text, "")
            strfromctrycode = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") 'IIf(UCase(ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromCountry.Items(ddlFromCountry.SelectedIndex).Text, "")
            strtoctrycode = IIf(txtctrycode.Text <> "", txtctrycode.Text, "") 'IIf(UCase(ddlToCountry.Items(ddlToCountry.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToCountry.Items(ddlToCountry.SelectedIndex).Text, "")
            strpdcyesno = 0
            strincludezero = IIf(ddlIncludeZero.SelectedIndex = 0, 1, 0)
            strgroupby = Val(ddlReportGroup.SelectedIndex)
            strreportorder = Trim(ddlReportOrder.SelectedIndex)

            strsuppliertype = Trim(ddlSupplierType.SelectedIndex)
            strincludeproforma = IIf(ddlproforma.SelectedIndex = 0, 1, 0)
            If ViewState("reporttype") = "CustomerAgeingSummary" Then
                Dim strpop As String = ""

                strpop = "window.open('TransactionReports.aspx?printId=CustomerAgeing&reportsType=excel&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                & "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                 & "&fromname=" & txtsuppliername.Text & "&frommkname=" & txtcityname.Text & "&fromctryname=" & txtctryname.Text & "&fromcatname=" & txtcatname.Text & "&glname=" & TxtBankName.Text _
                & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                & "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
                & "&includezero=" & strincludezero & "&custgroup_sp_type=" & custgroup_sp_type & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                & "&supptype=" & strsuppliertype & "&strincludeproforma=" & strincludeproforma _
                & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&ageingreporttyp=" & stragingreport & "','RepCustAgeingSum');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf ViewState("reporttype") = "CustomerAgeingDetail" Then
                Dim strpop As String = ""
                strpop = "window.open('TransactionReports.aspx?printId=CustomerAgeing&reportsType=excel&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                & "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                 & "&fromname=" & txtsuppliername.Text & "&frommkname=" & txtcityname.Text & "&fromctryname=" & txtctryname.Text & "&fromcatname=" & txtcatname.Text & "&glname=" & TxtBankName.Text _
                & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                & "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
                & "&includezero=" & strincludezero & "&custgroup_sp_type=" & custgroup_sp_type & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                & "&supptype=" & strsuppliertype & "&strincludeproforma=" & strincludeproforma _
                & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&ageingreporttyp=" & stragingreport & "','RepCustAgeingDet');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf ViewState("reporttype") = "SupplierAgeingSummary" Then
                Dim strpop As String = ""
                strpop = "window.open('TransactionReports.aspx?printId=CustomerAgeing&reportsType=excel&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                & "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                 & "&fromname=" & txtsuppliername.Text & "&frommkname=" & txtcityname.Text & "&fromctryname=" & txtctryname.Text & "&fromcatname=" & txtcatname.Text & "&glname=" & TxtBankName.Text _
                & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                & "&curr=" & strcurr & "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
                & "&includezero=" & strincludezero & "&custgroup_sp_type=" & custgroup_sp_type & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                & "&supptype=" & strsuppliertype & "&strincludeproforma=" & strincludeproforma _
                & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&ageingreporttyp=" & stragingreport & "&divid=" & divcode & " ','RepSupAgeingSum');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            ElseIf ViewState("reporttype") = "SupplierAgeingDetail" Then

                Dim strpop As String = ""
                strpop = "window.open('TransactionReports.aspx?printId=CustomerAgeing&reportsType=excel&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
                & "&fromaccode=" & strfomaccode & "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
                 & "&fromname=" & txtsuppliername.Text & "&frommkname=" & txtcityname.Text & "&fromctryname=" & txtctryname.Text & "&fromcatname=" & txtcatname.Text & "&glname=" & TxtBankName.Text _
                & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & strfromcitycode _
                & "&tocitycode=" & strtocitycode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
                & "&curr=" & strcurr & "&agingtype=" & stragingtype & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno _
                & "&includezero=" & strincludezero & "&custgroup_sp_type=" & custgroup_sp_type & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&type=" & strtype & "&orderby=" & strreportorder _
                & "&supptype=" & strsuppliertype & "&strincludeproforma=" & strincludeproforma _
                & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&ageingreporttyp=" & stragingreport & "&divid=" & divcode & "','RepSupAgeingDet');"

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptSupplierAgeingSummary.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try
    End Sub

End Class
