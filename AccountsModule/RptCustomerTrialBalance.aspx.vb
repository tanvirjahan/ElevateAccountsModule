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
Imports System.Globalization
#End Region

Partial Class RptCustomerTrialBalance
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
            ViewState.Add("RptCustomerTrialBalanceTranType", Request.QueryString("tran_type"))
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
                                                   CType(strappname, String), "AccountsModule\RptCustomerTrialBalance.aspx?tran_type=" + tran_type + "&appid=" + strappid, btnadd, Button1, btnReport, gv_SearchResult)
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
                If ViewState("RptCustomerTrialBalanceTranType") = "WOCR" Then
                    lblHeading.Text = "Customer Trial Balance"
                ElseIf ViewState("RptCustomerTrialBalanceTranType") = "WTCR" Then
                    lblHeading.Text = "Customers With Credit Balance"
                End If



                SetFocus(txtFromDate)

                txtcountrycode.Attributes.Add("readonly", "readonly")
                txtcustomercode.Attributes.Add("readonly", "readonly")
                txtcategorycode.Attributes.Add("readonly", "readonly")
                txtcontrolcode.Attributes.Add("readonly", "readonly")
                txtmarketcode.Attributes.Add("readonly", "readonly")


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrency, "currcode", "currcode", "select option_selected as currcode from reservation_parameters where param_id=457 union select 'A/C Currency'", False)


                '----------------------------- Default Dates
                txtFromDate.Text = Format(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                txttoDate.Text = Format(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")

                ' txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")


                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format("U", CType(Request.QueryString("fromdate"), Date))
                End If
                If Request.QueryString("todate") <> "" Then
                    txttoDate.Text = Format("U", CType(Request.QueryString("todate"), Date))
                End If
                If Request.QueryString("movflg") <> "" Then
                    ddlwithmovmt.SelectedIndex = Request.QueryString("movflg")
                End If



                If Request.QueryString("currtype") <> "" Then
                    ddlCurrency.SelectedIndex = IIf(Request.QueryString("currtype") = 1, 0, 1)
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




                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")


                    ddlCurrency.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")


                End If

            Else
                'checkrb_status()
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RptCustomerTrialBalanceWindowPostBack") Then

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
            ViewState.Add("BackPageName", "RptCustomerTrialBalance.aspx")

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
            Dim custgroup_sp_type As String

            custgroup_sp_type = Txtcustgroupcode.Text


            strmovflag = ddlwithmovmt.SelectedValue


            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            strtodate = IIf(strmovflag = 0, Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10), strfromdate)

            strcurrtype = IIf(ddlCurrency.Value = "A/C Currency", 1, 0)
            strorderby = ddlrptord.SelectedIndex
            strincludezero = ddlinclzero.SelectedIndex
            strgpby = ddlgpby.SelectedIndex
            strtrialtype = "TB"

            If ViewState("RptCustomerTrialBalanceTranType") = "WOCR" Then
                withCredit = 0
            ElseIf ViewState("RptCustomerTrialBalanceTranType") = "WTCR" Then
                withCredit = 1
            End If

            'Response.Redirect("rptsupptrialbalReport.aspx?type=C&fromdate=" & strfromdate & "&todate=" & strtodate _
            '& "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&movflg=" & strmovflag & "&fromcode=" & strfromcode & "&tocode =" & strtocode _
            '& "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode _
            '& "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto _
            '& "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit, False)

            Dim strpop As String = ""
            'strpop = "window.open('rptsupptrialbalReport.aspx?type=C&frmdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&movflg=" & strmovflag & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & "','RepRVPVReg','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            'strpop = "window.open('rptsupptrialbalReport.aspx?type=C&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&movflg=" & strmovflag & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & " &trialtype=" & strtrialtype & " ','RepCustBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('rptsupptrialbalReport.aspx?type=C&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & txtcountrycode.Text & "&toctry=" & txtcountrycode.Text & "&movflg=" & strmovflag & "&fromcode=" & txtcustomercode.Text & "&tocode=" & txtcustomercode.Text & "&frommarkcode=" & txtmarketcode.Text & "&tomarkcode=" & txtmarketcode.Text & "&fromcat=" & txtcategorycode.Text & "&tocat=" & txtcategorycode.Text & "&fromglcode=" & txtcontrolcode.Text & "&toglcode=" & txtcontrolcode.Text & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & " &divid=" & ViewState("divcode") & " &trialtype=" & strtrialtype & "&custgroup_sp_type=" & custgroup_sp_type & " ','RepCustBalance');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptcustomertrialbalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

    'Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
    '    Try
    '        'If ValidatePage() = True Then
    '        If ValidatePage() = False Then
    '            Exit Sub
    '        End If

    '        ViewState.Add("Pageame", "CustomertrialbalReport")
    '        ViewState.Add("BackPageName", "RptCustomerTrialBalance.aspx")

    '        Dim strReportTitle As String = ""

    '        Dim strfromdate As String = ""
    '        Dim strtodate As String = ""
    '        Dim strmovflag As String = ""

    '        Dim strfromcode As String = ""
    '        Dim strtocode As String = ""
    '        Dim strmarketcode As String = ""
    '        Dim strmarketcodeto As String = ""
    '        Dim strcatcode As String = ""
    '        Dim strcatcodeto As String = ""
    '        Dim strglcode As String = ""
    '        Dim strglcodeto As String = ""
    '        Dim strcurrtype As String = ""
    '        Dim strorderby As String = ""
    '        Dim strincludezero As String = ""
    '        Dim strgpby As String = ""

    '        Dim strfromctry As String = ""
    '        Dim strtoctry As String = ""
    '        Dim withCredit As String = ""
    '        Dim category As String = ""
    '        Dim strtrialtype As String = ""
    '        Dim custgroup_sp_type As String

    '        custgroup_sp_type = Txtcustgroupcode.Text


    '        strmovflag = ddlwithmovmt.SelectedValue


    '        strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)

    '        strtodate = IIf(strmovflag = 0, Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10), strfromdate)

    '        strcurrtype = IIf(ddlCurrency.Value = "A/C Currency", 1, 0)
    '        strorderby = ddlrptord.SelectedIndex
    '        strincludezero = ddlinclzero.SelectedIndex
    '        strgpby = ddlgpby.SelectedIndex
    '        strtrialtype = "TB"

    '        If ViewState("RptCustomerTrialBalanceTranType") = "WOCR" Then
    '            withCredit = 0
    '        ElseIf ViewState("RptCustomerTrialBalanceTranType") = "WTCR" Then
    '            withCredit = 1
    '        End If

    '        'Response.Redirect("rptsupptrialbalReport.aspx?type=C&fromdate=" & strfromdate & "&todate=" & strtodate _
    '        '& "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&movflg=" & strmovflag & "&fromcode=" & strfromcode & "&tocode =" & strtocode _
    '        '& "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode _
    '        '& "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto _
    '        '& "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit, False)

    '        Dim strpop As String = ""
    '        'strpop = "window.open('rptsupptrialbalReport.aspx?type=C&frmdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&movflg=" & strmovflag & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & "','RepRVPVReg','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
    '        'strpop = "window.open('rptsupptrialbalReport.aspx?type=C&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&movflg=" & strmovflag & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & " &trialtype=" & strtrialtype & " ','RepCustBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
    '        strpop = "window.open('TransactionReports.aspx?printId=CustomerTrial&type=C&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & txtcountrycode.Text & "&toctry=" & txtcountrycode.Text & "&movflg=" & strmovflag & "&fromcode=" & txtcustomercode.Text & "&tocode=" & txtcustomercode.Text & "&frommarkcode=" & txtmarketcode.Text & "&tomarkcode=" & txtmarketcode.Text & "&fromcat=" & txtcategorycode.Text & "&tocat=" & txtcategorycode.Text & "&fromglcode=" & txtcontrolcode.Text & "&toglcode=" & txtcontrolcode.Text & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & " &divid=" & ViewState("divcode") & " &trialtype=" & strtrialtype & "&custgroup_sp_type=" & custgroup_sp_type & " ','RepCustBalance');"
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        objUtils.WritErrorLog("rptcustomertrialbalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

    '    End Try

    'End Sub

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
        Dim frmdate As DateTime
        Dim MyCultureInfo As New CultureInfo("fr-Fr")
        Dim ds As DataSet

        Try
            If txtFromDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtFromDate.ClientID + "');", True)
                'SetFocus(txtFromDate)
                ValidatePage = False
                Exit Function
            End If
            If txtToDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                'SetFocus(txtToDate)
                ValidatePage = False
                Exit Function
            End If
            If CType(objDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objDate.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                'SetFocus(txtToDate)
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
    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function Getcontrolname(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim controlname As New List(Of String)
        Try

            strSqlQry = "select acctname,acctcode from acctmast where controlyn='Y' and div_code='" & divcode & "' and acctname like  '" & Trim(prefixText) & "%'"
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

            strSqlQry = "select customergroupname,customergroupcode from Customergroup  where  customergroupname like  '" & Trim(prefixText) & "%' order by customergroupname"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    agent.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("customergroupname").ToString(), myDS.Tables(0).Rows(i)("customergroupcode").ToString()))
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

            If contextKey = "true" Then
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

    Protected Sub Button1_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
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

            strmovflag = ddlwithmovmt.SelectedValue

            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            strtodate = IIf(strmovflag = 0, Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10), strfromdate)
            strtocode = IIf(txtcustomercode.Text <> "", txtcustomercode.Text, "")
            strfromcode = IIf(txtcustomercode.Text <> "", txtcustomercode.Text, "")
            strglcodeto = IIf(txtcontrolcode.Text <> "", txtcontrolcode.Text, "")
            strglcode = IIf(txtcontrolcode.Text <> "", txtcontrolcode.Text, "")
            strcatcodeto = IIf(txtcategorycode.Text <> "", txtcategorycode.Text, "")
            strcatcode = IIf(txtcategorycode.Text <> "", txtcategorycode.Text, "")
            strmarketcode = IIf(txtmarketcode.Text <> "", txtmarketcode.Text, "")
            strmarketcodeto = IIf(txtmarketcode.Text <> "", txtmarketcode.Text, "")
            strfromctry = IIf(txtcountrycode.Text <> "", txtcountrycode.Text, "")
            strtoctry = IIf(txtcountrycode.Text <> "", txtcountrycode.Text, "")


            strcurrtype = IIf(ddlCurrency.Value = "A/C Currency", 1, 0)
            strorderby = ddlrptord.SelectedIndex
            strincludezero = ddlinclzero.SelectedIndex
            strgpby = ddlgpby.SelectedIndex


            If ViewState("RptCustomerTrialBalanceTranType") = "WOCR" Then
                withCredit = 0
            ElseIf ViewState("RptCustomerTrialBalanceTranType") = "WTCR" Then
                withCredit = 1
            End If


            parm(0) = New SqlParameter("@fromdate", strfromdate)
            parm(1) = New SqlParameter("@todate", strtodate)
            parm(2) = New SqlParameter("@movflg", strmovflag)
            parm(3) = New SqlParameter("@fromcode", strfromcode)
            parm(4) = New SqlParameter("@tocode", strtocode)
            parm(5) = New SqlParameter("@frommarkcode", strmarketcode)
            parm(6) = New SqlParameter("@tomarkcode", strmarketcodeto)
            parm(7) = New SqlParameter("@fromctry", strfromctry)
            parm(8) = New SqlParameter("@toctry", strtoctry)
            parm(9) = New SqlParameter("@fromcat", strcatcode)
            parm(10) = New SqlParameter("@tocat", strcatcodeto)
            parm(11) = New SqlParameter("@fromglcode", strglcode)
            parm(12) = New SqlParameter("@toglcode", strglcodeto)
            parm(13) = New SqlParameter("@currtype", strcurrtype)
            parm(14) = New SqlParameter("@orderby", strorderby)
            parm(15) = New SqlParameter("@includezero", strincludezero)
            parm(16) = New SqlParameter("@withCredit", withCredit)


            For i = 0 To 16
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_customer_trialbal_xls", parms)

            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    objUtils.ExportToExcel(ds, Response)
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Record found' );", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptcustomertrialbalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

    Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
        Try
            'If ValidatePage() = True Then
            If ValidatePage() = False Then
                Exit Sub
            End If

            ViewState.Add("Pageame", "CustomertrialbalReport")
            ViewState.Add("BackPageName", "RptCustomerTrialBalance.aspx")

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
            Dim custgroup_sp_type As String

            custgroup_sp_type = Txtcustgroupcode.Text


            strmovflag = ddlwithmovmt.SelectedValue


            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            strtodate = IIf(strmovflag = 0, Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10), strfromdate)

            strcurrtype = IIf(ddlCurrency.Value = "A/C Currency", 1, 0)
            strorderby = ddlrptord.SelectedIndex
            strincludezero = ddlinclzero.SelectedIndex
            strgpby = ddlgpby.SelectedIndex
            strtrialtype = "TB"

            If ViewState("RptCustomerTrialBalanceTranType") = "WOCR" Then
                withCredit = 0
            ElseIf ViewState("RptCustomerTrialBalanceTranType") = "WTCR" Then
                withCredit = 1
            End If

            'Response.Redirect("rptsupptrialbalReport.aspx?type=C&fromdate=" & strfromdate & "&todate=" & strtodate _
            '& "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&movflg=" & strmovflag & "&fromcode=" & strfromcode & "&tocode =" & strtocode _
            '& "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode _
            '& "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto _
            '& "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit, False)

            Dim strpop As String = ""
            'strpop = "window.open('rptsupptrialbalReport.aspx?type=C&frmdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&movflg=" & strmovflag & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & "','RepRVPVReg','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            'strpop = "window.open('rptsupptrialbalReport.aspx?type=C&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&movflg=" & strmovflag & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & " &trialtype=" & strtrialtype & " ','RepCustBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('TransactionReports.aspx?printId=CustomerTrial&reportsType=pdf&type=C&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromctryname=" & txtcountryname.Text & "&fromctry=" & txtcountrycode.Text & "&toctry=" & txtcountrycode.Text & "&movflg=" & strmovflag & "&fromcode=" & txtcustomercode.Text & "&fromname=" & txtcustomername.Text & "&tocode=" & txtcustomercode.Text & "&frommarkcode=" & txtmarketcode.Text & "&frommarkname=" & txtmarketname.Text & "&tomarkcode=" & txtmarketcode.Text & "&fromcatname=" & txtcategoryname.Text & "&fromcat=" & txtcategorycode.Text & "&tocat=" & txtcategorycode.Text & "&fromglname=" & txtcontrolname.Text & "&fromglcode=" & txtcontrolcode.Text & "&toglcode=" & txtcontrolcode.Text & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & " &divid=" & ViewState("divcode") & " &trialtype=" & strtrialtype & "&custgroup_sp_type=" & custgroup_sp_type & " ','RepCustBalance');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptcustomertrialbalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

    Protected Sub btnExcelReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelReport.Click
        Try
            'If ValidatePage() = True Then
            If ValidatePage() = False Then
                Exit Sub
            End If

            ViewState.Add("Pageame", "CustomertrialbalReport")
            ViewState.Add("BackPageName", "RptCustomerTrialBalance.aspx")

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
            Dim custgroup_sp_type As String

            custgroup_sp_type = Txtcustgroupcode.Text


            strmovflag = ddlwithmovmt.SelectedValue


            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            strtodate = IIf(strmovflag = 0, Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10), strfromdate)

            strcurrtype = IIf(ddlCurrency.Value = "A/C Currency", 1, 0)
            strorderby = ddlrptord.SelectedIndex
            strincludezero = ddlinclzero.SelectedIndex
            strgpby = ddlgpby.SelectedIndex
            strtrialtype = "TB"

            If ViewState("RptCustomerTrialBalanceTranType") = "WOCR" Then
                withCredit = 0
            ElseIf ViewState("RptCustomerTrialBalanceTranType") = "WTCR" Then
                withCredit = 1
            End If

            'Response.Redirect("rptsupptrialbalReport.aspx?type=C&fromdate=" & strfromdate & "&todate=" & strtodate _
            '& "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&movflg=" & strmovflag & "&fromcode=" & strfromcode & "&tocode =" & strtocode _
            '& "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode _
            '& "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto _
            '& "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit, False)

            Dim strpop As String = ""
            'strpop = "window.open('rptsupptrialbalReport.aspx?type=C&frmdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&movflg=" & strmovflag & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & "','RepRVPVReg','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            'strpop = "window.open('rptsupptrialbalReport.aspx?type=C&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&movflg=" & strmovflag & "&fromcode=" & strfromcode & "&tocode=" & strtocode & "&frommarkcode=" & strmarketcode & "&tomarkcode=" & strmarketcodeto & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & " &trialtype=" & strtrialtype & " ','RepCustBalance','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            strpop = "window.open('TransactionReports.aspx?printId=CustomerTrial&reportsType=excel&type=C&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromctryname=" & txtcountryname.Text & "&fromctry=" & txtcountrycode.Text & "&toctry=" & txtcountrycode.Text & "&movflg=" & strmovflag & "&fromcode=" & txtcustomercode.Text & "&fromname=" & txtcustomername.Text & "&tocode=" & txtcustomercode.Text & "&frommarkcode=" & txtmarketcode.Text & "&frommarkname=" & txtmarketname.Text & "&tomarkcode=" & txtmarketcode.Text & "&fromcatname=" & txtcategoryname.Text & "&fromcat=" & txtcategorycode.Text & "&tocat=" & txtcategorycode.Text & "&fromglname=" & txtcontrolname.Text & "&fromglcode=" & txtcontrolcode.Text & "&toglcode=" & txtcontrolcode.Text & "&currtype=" & strcurrtype & "&orderby=" & strorderby & "&includezero=" & strincludezero & "&gpby=" & strgpby & "&withCredit=" & withCredit & " &divid=" & ViewState("divcode") & " &trialtype=" & strtrialtype & "&custgroup_sp_type=" & custgroup_sp_type & " ','RepCustBalance');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptcustomertrialbalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

End Class
