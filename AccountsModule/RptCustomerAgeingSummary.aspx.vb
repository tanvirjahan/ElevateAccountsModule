#Region "Namespace"
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

#End Region
Partial Class RptCustomerAgeingSummary
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDate As New clsDateTime
    Dim strappid As String = ""
    Dim strappname As String = ""
    Private Shared divcode As String = ""
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

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
                                                   CType(strappname, String), "AccountsModule\RptCustomerAgeingSummary.aspx?tran_type=" + tran_type + "&appid=" + strappid, btnadd, Button1, btnReport, gv_SearchResult)
            End If


            '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
            '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            ViewState.Add("divcode", divid)
            divcode = ViewState("divcode")

            If IsPostBack = False Then
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                If Request.QueryString("tran_type") = "customersummary" Then
                    lblHeading.Text = "Customer Ageing Summary"
                    ViewState.Add("reporttype", "CustomerAgeingSummary")
                ElseIf Request.QueryString("tran_type") = "customerageing1" Then
                    lblHeading.Text = "Customer Ageing Report "
                    ViewState.Add("reporttype", "CustomerAgeing1")
                ElseIf Request.QueryString("tran_type") = "customerageing2" Then
                    lblHeading.Text = "Customer Ageing Report - II"
                    ViewState.Add("reporttype", "CustomerAgeing2")
                ElseIf Request.QueryString("tran_type") = "customerdetail" Then
                    lblHeading.Text = "Customer Ageing Detail"
                    ViewState.Add("reporttype", "CustomerAgeingDetail")
                End If

                txtcountrycode.Attributes.Add("readonly", "readonly")
                txtcustomercode.Attributes.Add("readonly", "readonly")
                txtcategorycode.Attributes.Add("readonly", "readonly")
                txtcontrolcode.Attributes.Add("readonly", "readonly")
                txtmarketcode.Attributes.Add("readonly", "readonly")
                'ddlAgeingType.SelectedIndex = 1

                txtFromDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrency, "currcode", "currcode", " select 'Party Currency'as currcode union select option_selected as currcode from reservation_parameters where param_id=457", False)

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


                End If
                'btnReport.Attributes.Add("onclick", "return FormValidation()")
            Else
                'checkrb_status()
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptCustomerAgeingSummary','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    'Protected Sub btnpdfformat_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles pdfformat.Click
    '    Try
    '        Dim strReportTitle As String = ""
    '        Dim strfromdate As String = ""
    '        Dim strtodate As String = ""
    '        Dim strfomaccode As String = ""
    '        Dim strtoaccode As String = ""
    '        Dim strfromctrlcode As String = ""
    '        Dim strtoctrlcode As String = ""
    '        Dim strfromccatcode As String = ""
    '        Dim strtoccatcode As String = ""
    '        Dim strfromcitycode As String = ""
    '        Dim strtocitycode As String = ""
    '        Dim strfromctrycode As String = ""
    '        Dim strtoctrycode As String = ""
    '        Dim strremarks As String = ""

    '        Dim custgroup_sp_type As String
    '        custgroup_sp_type = Txtcustgroupcode.Text

    '        Dim strgroupby As Integer = 0

    '        Dim strtype As String = "C"
    '        Dim strdatetype As Integer = 0 '0 as on date, 1 from -to date
    '        Dim stragingtype As Integer = 0
    '        Dim stragingreport As Integer = 0
    '        Dim strpdcyesno As Integer = 0
    '        Dim strincludezero As Integer = 0 ' 0 no, 1 yes
    '        Dim strcurr As Integer = 0 ' 0 Ac Currency, 1 base currency
    '        Dim strsumdet As Integer = 0 '0-Summary,1-Detail
    '        Dim strreportorder As String = "" ' 1 - Code, 2 Name
    '        Dim strcustomertype As String = "" '1 - Cash, 2 - Credit
    '        Dim strincludeproforma As Integer = 0 ' 0 no, 1 yes

    '        Dim strreporttype As String = ""
    '        Dim strrepfilter As String = ""
    '        If ViewState("reporttype") = "SupplierAgeingSummary" Or ViewState("reporttype") = "CustomerAgeingSummary" Then
    '            strreporttype = "SupplierAgeingSummary"
    '            strsumdet = 0
    '        ElseIf ViewState("reporttype") = "SupplierAgeingDetail" Or ViewState("reporttype") = "CustomerAgeingDetail" Then
    '            strreporttype = "SupplierAgeingDetail"
    '            strsumdet = 1
    '        End If


    '        strfromdate = Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd")

    '        If ViewState("reporttype") = "CustomerAgeingDetail" Or ViewState("reporttype") = "CustomerAgeingSummary" Then
    '            strtype = "C"
    '        End If
    '        'strcurr = IIf(ddlCurrency.Value = "A/C Currency", 0, 1)
    '        strcurr = IIf(ddlCurrency.SelectedIndex = 0, 1, 0)
    '        stragingtype = ddlAgeingType.SelectedIndex
    '        strincludezero = ddlIncludeZero.SelectedIndex


    '        strpdcyesno = 0
    '        strincludezero = IIf(ddlIncludeZero.SelectedIndex = 0, 1, 0)
    '        strgroupby = Val(ddlReportGroup.SelectedIndex)
    '        strreportorder = Trim(ddlReportOrder.SelectedIndex)
    '        strcustomertype = Trim(ddlCustomerType.SelectedIndex)

    '        strincludeproforma = IIf(ddlproforma.Value = "Yes", 1, 0)

    '        Dim clscustomeragingobj As New ClsCustomerAgeingPdf

    '        clscustomeragingobj.tran_type = Request.QueryString("tran_type")
    '        Dim strpop As String = ""
    '        '  strpop = "window.open('PrintReport.aspx?printId=CustomerAgeing','RepCustAgeingDet' );"


    '        strpop = "window.open('TransactionReports.aspx?printId=CustomerAgeing&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strfromdate _
    '        & "&fromaccode=" & txtcustomercode.Text & "&toaccode=" & txtcustomercode.Text & "&fromctrlcode=" & txtcontrolcode.Text & "&toctrlcode=" & txtcontrolcode.Text _
    '        & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & txtmarketcode.Text _
    '        & "&tocitycode=" & txtmarketcode.Text & "&fromctrycode=" & txtcountrycode.Text & "&toctrycode=" & txtcountrycode.Text _
    '        & "&agingtype=" & stragingtype & "&custgroup_sp_type=" & custgroup_sp_type & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno & "&curr=" & strcurr _
    '        & "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&divid=" & ViewState("divcode") & "&type=" & strtype & "&orderby=" & strreportorder _
    '        & "&custtype=" & strcustomertype & "&strincludeproforma=" & strincludeproforma _
    '        & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&custype=" & ddlCustomerType.Value & "&ageingreporttyp=" & stragingreport & "','RepCustAgeingDet' );"

    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    '        'End If
    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        objUtils.WritErrorLog("RptSupplierAgeingSummary.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
    '    End Try
    'End Sub


    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
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

            Dim custgroup_sp_type As String
            custgroup_sp_type = Txtcustgroupcode.Text

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
            Dim strcustomertype As String = "" '1 - Cash, 2 - Credit
            Dim strincludeproforma As Integer = 0 ' 0 no, 1 yes

            Dim strreporttype As String = ""
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
            End If
            'strcurr = IIf(ddlCurrency.Value = "A/C Currency", 0, 1)
            strcurr = IIf(ddlCurrency.SelectedIndex = 0, 1, 0)
            stragingtype = ddlAgeingType.SelectedIndex
            strincludezero = ddlIncludeZero.SelectedIndex


            strpdcyesno = 0
            strincludezero = IIf(ddlIncludeZero.SelectedIndex = 0, 1, 0)
            strgroupby = Val(ddlReportGroup.SelectedIndex)
            strreportorder = Trim(ddlReportOrder.SelectedIndex)
            strcustomertype = Trim(ddlCustomerType.SelectedIndex)

            strincludeproforma = IIf(ddlproforma.Value = "Yes", 1, 0)

            Dim strpop As String = ""

            strpop = "window.open('rptStatement.aspx?Pageame=RptSupplierAgingSummary&BackPageName=RptSupplierAgeingSummary.aspx&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strtodate _
      & "&fromaccode=" & txtcustomercode.Text & "&toaccode=" & txtcustomercode.Text & "&fromctrlcode=" & txtcontrolcode.Text & "&toctrlcode=" & txtcontrolcode.Text _
      & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & txtmarketcode.Text _
      & "&tocitycode=" & txtmarketcode.Text & "&fromctrycode=" & txtcountrycode.Text & "&toctrycode=" & txtcountrycode.Text _
      & "&agingtype=" & stragingtype & "&custgroup_sp_type=" & custgroup_sp_type & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno & "&curr=" & strcurr _
      & "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&divid=" & ViewState("divcode") & "&type=" & strtype & "&orderby=" & strreportorder _
      & "&custtype=" & strcustomertype & "&strincludeproforma=" & strincludeproforma _
      & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&custype=" & ddlCustomerType.Value & "&ageingreporttyp=" & stragingreport & "','RepCustAgeingDet' );"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptSupplierAgeingSummary.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("~/MainPage.aspx")
    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function Getcontrolname(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim controlname As New List(Of String)
        Try

            strSqlQry = "select acctname,acctcode from acctmast where  controlyn='Y' and div_code='" & divcode & "' and acctname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    controlname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return controlname
        Catch ex As Exception
            Return controlname
        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function Getcustgroup(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim agent As New List(Of String)
        Try

            strSqlQry = "select customergroupname,customergroupcode from Customergroup  where  customergroupname like  '" & Trim(prefixText) & "%' order by customeraccodename"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    agent.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("customergroupname").ToString(), myDS.Tables(0).Rows(i)("customeraccodecode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return agent
        Catch ex As Exception
            Return agent
        End Try

    End Function


    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function Getcustomer(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim agent As New List(Of String)
        Dim city, ctry, custgroup, custagent As String
        ctry = ""
        city = ""
        custgroup = ""
        custagent = ""

        Try

            If contextKey = "True" Then
                contextKey = ""
            Else
                If contextKey <> "" Then
                    ctry = contextKey.Trim.Split("||")(0)

                    city = contextKey.Trim.Split("||")(2)
                    custgroup = contextKey.Trim.Split("||")(4)
                    custagent = contextKey.Trim.Split("||")(6)
                End If
            End If
            strSqlQry = "select agentname,agentcode from agentmast  where divcode='" & divcode & "' and  agentname like  '" & Trim(prefixText) & "%'  "
            If ctry <> "" Then
                strSqlQry = strSqlQry & " and ctrycode='" & ctry & "'"
            End If
            If city <> "" Then
                strSqlQry = strSqlQry & " and citycode='" & city & "'"
            End If
            If custagent <> "" Then
                strSqlQry = strSqlQry & " and catcode='" & custagent & "'"
            End If
            If custgroup <> "" Then
                strSqlQry = strSqlQry & "   and agentcode in (select distinct agentcode from Customergroup_detail where customergroupcode='" & custgroup & "') "

            End If



            strSqlQry = strSqlQry & "  order by agentname"


            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    agent.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("agentcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return agent
        Catch ex As Exception
            Return agent
        End Try

    End Function




    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getmarket(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

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
    Public Shared Function Getcountry(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim ctryname As New List(Of String)
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

                    ctryname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("ctryname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return ctryname
        Catch ex As Exception
            Return ctryname
        End Try
    End Function


    <System.Web.Script.Services.ScriptMethod()> _
 <System.Web.Services.WebMethod()> _
    Public Shared Function Getcategory(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim categoryname As New List(Of String)
        Try

            strSqlQry = "select agentcatname,agentcatcode from agentcatmast where agentcatname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    categoryname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentcatname").ToString(), myDS.Tables(0).Rows(i)("agentcatcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return categoryname
        Catch ex As Exception
            Return categoryname
        End Try

    End Function


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
            Dim strcustomertype As String = "" '1-Cash,2-Credit

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

            strfomaccode = IIf(txtcustomercode.Text <> "", txtcustomercode.Text, "")
            strfomaccode = IIf(txtcustomercode.Text <> "", txtcustomercode.Text, "")
            strfromctrlcode = IIf(txtcontrolcode.Text <> "", txtcontrolcode.Text, "")
            strfromctrlcode = IIf(txtcontrolcode.Text <> "", txtcontrolcode.Text, "")
            strtoccatcode = IIf(txtcategorycode.Text <> "", txtcategorycode.Text, "")
            strtoccatcode = IIf(txtcategorycode.Text <> "", txtcategorycode.Text, "")
            strtocitycode = IIf(txtmarketcode.Text <> "", txtmarketcode.Text, "")
            strtocitycode = IIf(txtmarketcode.Text <> "", txtmarketcode.Text, "")
            strfromctrycode = IIf(txtcountrycode.Text <> "", txtcountrycode.Text, "")
            strtoctrycode = IIf(txtcountrycode.Text <> "", txtcountrycode.Text, "")




            strpdcyesno = 0
            strincludezero = IIf(ddlIncludeZero.SelectedIndex = 0, 1, 0)
            strgroupby = Val(ddlReportGroup.SelectedIndex)
            strreportorder = Trim(ddlReportOrder.SelectedIndex)
            strcustomertype = Trim(ddlCustomerType.SelectedIndex) 'Added strcustomertype by Archana on 02/04/2015

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
    Protected Sub btnpdfformat_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles pdfformat.Click
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

            Dim custgroup_sp_type As String
            custgroup_sp_type = Txtcustgroupcode.Text

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
            Dim strcustomertype As String = "" '1 - Cash, 2 - Credit
            Dim strincludeproforma As Integer = 0 ' 0 no, 1 yes

            Dim strreporttype As String = ""
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
            End If
            'strcurr = IIf(ddlCurrency.Value = "A/C Currency", 0, 1)
            strcurr = IIf(ddlCurrency.SelectedIndex = 0, 1, 0)
            stragingtype = ddlAgeingType.SelectedIndex
            strincludezero = ddlIncludeZero.SelectedIndex


            strpdcyesno = 0
            strincludezero = IIf(ddlIncludeZero.SelectedIndex = 0, 1, 0)
            strgroupby = Val(ddlReportGroup.SelectedIndex)
            strreportorder = Trim(ddlReportOrder.SelectedIndex)
            strcustomertype = Trim(ddlCustomerType.SelectedIndex)

            strincludeproforma = IIf(ddlproforma.Value = "Yes", 1, 0)

            Dim clscustomeragingobj As New ClsCustomerAgeingPdf

            clscustomeragingobj.tran_type = Request.QueryString("tran_type")
            Dim strpop As String = ""
            '  strpop = "window.open('PrintReport.aspx?printId=CustomerAgeing','RepCustAgeingDet' );"


            strpop = "window.open('TransactionReports.aspx?printId=CustomerAgeing&reportsType=pdf&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strfromdate _
            & "&fromaccode=" & txtcustomercode.Text & "&toaccode=" & txtcustomercode.Text & "&fromctrlcode=" & txtcontrolcode.Text & "&toctrlcode=" & txtcontrolcode.Text _
            & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & txtmarketcode.Text _
             & "&fromname=" & txtcustomername.Text & "&frommkname=" & txtmarketname.Text & "&fromctryname=" & txtcountryname.Text & "&fromcatname=" & txtcategoryname.Text & "&glname=" & txtcontrolname.Text _
            & "&tocitycode=" & txtmarketcode.Text & "&fromctrycode=" & txtcountrycode.Text & "&toctrycode=" & txtcountrycode.Text _
            & "&agingtype=" & stragingtype & "&custgroup_sp_type=" & custgroup_sp_type & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno & "&curr=" & strcurr _
            & "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&divid=" & ViewState("divcode") & "&type=" & strtype & "&orderby=" & strreportorder _
            & "&custtype=" & strcustomertype & "&strincludeproforma=" & strincludeproforma _
            & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&custype=" & ddlCustomerType.Value & "&ageingreporttyp=" & stragingreport & "','RepCustAgeingDet' );"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptSupplierAgeingSummary.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnExlReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExlReport.Click
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

            Dim custgroup_sp_type As String
            custgroup_sp_type = Txtcustgroupcode.Text

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
            Dim strcustomertype As String = "" '1 - Cash, 2 - Credit
            Dim strincludeproforma As Integer = 0 ' 0 no, 1 yes

            Dim strreporttype As String = ""
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
            End If
            'strcurr = IIf(ddlCurrency.Value = "A/C Currency", 0, 1)
            strcurr = IIf(ddlCurrency.SelectedIndex = 0, 1, 0)
            stragingtype = ddlAgeingType.SelectedIndex
            strincludezero = ddlIncludeZero.SelectedIndex


            strpdcyesno = 0
            strincludezero = IIf(ddlIncludeZero.SelectedIndex = 0, 1, 0)
            strgroupby = Val(ddlReportGroup.SelectedIndex)
            strreportorder = Trim(ddlReportOrder.SelectedIndex)
            strcustomertype = Trim(ddlCustomerType.SelectedIndex)

            strincludeproforma = IIf(ddlproforma.Value = "Yes", 1, 0)

            Dim clscustomeragingobj As New ClsCustomerAgeingPdf

            clscustomeragingobj.tran_type = Request.QueryString("tran_type")
            Dim strpop As String = ""
            '  strpop = "window.open('PrintReport.aspx?printId=CustomerAgeing','RepCustAgeingDet' );"


            strpop = "window.open('TransactionReports.aspx?printId=CustomerAgeing&reportsType=excel&tran_type=" & Request.QueryString("tran_type") & "&fromdate=" & strfromdate & "&todate=" & strfromdate _
            & "&fromaccode=" & txtcustomercode.Text & "&toaccode=" & txtcustomercode.Text & "&fromctrlcode=" & txtcontrolcode.Text & "&toctrlcode=" & txtcontrolcode.Text _
            & "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&fromcitycode=" & txtmarketcode.Text _
             & "&fromname=" & txtcustomername.Text & "&frommkname=" & txtmarketname.Text & "&fromctryname=" & txtcountryname.Text & "&fromcatname=" & txtcategoryname.Text & "&glname=" & txtcontrolname.Text _
            & "&tocitycode=" & txtmarketcode.Text & "&fromctrycode=" & txtcountrycode.Text & "&toctrycode=" & txtcountrycode.Text _
            & "&agingtype=" & stragingtype & "&custgroup_sp_type=" & custgroup_sp_type & "&groupby=" & strgroupby & "&sumdet=" & strsumdet & "&pdcyesno=" & strpdcyesno & "&curr=" & strcurr _
            & "&includezero=" & strincludezero & "&remarks=" & strremarks & "&reporttype=" & strreporttype & "&divid=" & ViewState("divcode") & "&type=" & strtype & "&orderby=" & strreportorder _
            & "&custtype=" & strcustomertype & "&strincludeproforma=" & strincludeproforma _
            & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&rptgroup=" & ddlReportGroup.Value & "&rptOrder=" & ddlReportOrder.Value & "&custype=" & ddlCustomerType.Value & "&ageingreporttyp=" & stragingreport & "','RepCustAgeingDet' );"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptSupplierAgeingSummary.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

End Class



