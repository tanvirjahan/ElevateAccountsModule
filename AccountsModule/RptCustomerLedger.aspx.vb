'------------================--------------=======================------------------================
'   Module Name    :    RptCustomerLedger.aspx
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

Partial Class RptCustomerLedger
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDate As New clsDateTime
    Dim strappid As String = ""
    Dim strappname As String = ""
    Shared divcode As String = ""
#End Region



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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try



            Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
            Dim AppId As String = CType(Request.QueryString("appid"), String)
            strappid = AppId
            If AppId Is Nothing = False Then
                '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
                strappname = Session("AppName")
                '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            End If
            If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            Else
                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                   CType(strappname, String), "AccountsModule\RptCustomerLedger.aspx?appid=" + strappid, btnadd, Button1, btnPdfReport, gv_SearchResult)
            End If


            txtagDate.Focus()



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

                SetFocus(txtFromDate)
                If Request.QueryString("custstatement") <> "" Then
                    If Request.QueryString("custstatement") = 1 Then
                        lblHeading.Text = "Customer Ledger  Statement "


                        lblageing.Visible = True

                        ddlAgeing.Visible = True
                        ddlPDC.Visible = False
                        ddlLedgerType.Visible = False
                        lblpdcdet.Visible = False
                        lblledgertype.Visible = False
                    Else
                        lblageing.Visible = False

                        ddlAgeing.Visible = False
                        ddlPDC.Visible = True
                        ddlLedgerType.Visible = True
                        lblpdcdet.Visible = True
                        lblledgertype.Visible = True
                    End If
                End If

                txtcountrycode.Attributes.Add("readonly", "readonly")
                txtcustomercode.Attributes.Add("readonly", "readonly")
                txtcategorycode.Attributes.Add("readonly", "readonly")
                txtcontrolcode.Attributes.Add("readonly", "readonly")
                txtmarketcode.Attributes.Add("readonly", "readonly")



                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrency, "currcode", "currcode", " select 'Party Currency'as currcode union select option_selected as currcode from reservation_parameters where param_id=457", False)

                '----------------------------- Default Dates
                txtFromDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                txtToDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                txtagDate.Text = Format(CType(objDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy")

                ' txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")


                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format("U", CType(Request.QueryString("fromdate"), Date))
                End If
                If Request.QueryString("todate") <> "" Then
                    ' todate = Request.QueryString("todate")
                    txtToDate.Text = Format("U", CType(Request.QueryString("todate"), Date))
                End If



                If Request.QueryString("currtype") <> "" Then
                    ddlCurrency.SelectedIndex = IIf(Request.QueryString("currtype") = 1, 0, 1)
                End If
                If Request.QueryString("ledgertype") <> "" Then
                    ddlLedgerType.SelectedIndex = Request.QueryString("ledgertype")
                End If


                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlCurrency.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlLedgerType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlPDC.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
            End If
            ClientScript.GetPostBackEventReference(Me, String.Empty)
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepCustLedgerWindowPostBack") Then
                btnReport_Click(sender, e)
            End If
            'checkrb_status()
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub



    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Try
            'If ValidatePage() = True Then

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
            Dim ageing As String = 0
            Dim custgroup_sp_type As String

            custgroup_sp_type = Txtcustgroupcode.Text

            Dim ststement As String = "0"


            If Request.QueryString("custstatement") <> "" Then
                If Request.QueryString("custstatement") = 1 Then
                    ststement = "1"
                End If
            End If




            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            stracctype = "C"

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
            If Request.QueryString("custstatement") = 1 Then
                lblHeading.Text = "Customer Ledger  Statement "


                ageing = ddlAgeing.SelectedIndex


            Else
                ageing = 0

            End If

            'Response.Redirect("rptsupp_custledgerReport.aspx?type=C&fromdate=" & strfromdate & "&todate=" & strtodate _
            '& "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&frommarketcode=" & strfromcity & "&tomarketcode=" & strtocity & "&actype=" & stracctype & "&fromcode=" & strfromcode & "&tocode=" & strtocode _
            '& "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto _
            '& "&currtype=" & strcurrtype & "&ledgertype=" & ledgertype & "&pdcyesno=" & pdcyesno, False)
            Dim strpop As String = ""
            'strpop = "window.open('rptsupp_custledgerReport.aspx?Pageame=SupplierLedger&BackPageName=RptSupplierLedger.aspx&type=C&fromdate=" & strfromdate & "&todate=" & strtodate _
            '           & "&fromctry=" & strfromctry & "&toctry=" & strtoctry & "&frommarketcode=" & strfromcity & "&tomarketcode=" & strtocity & "&actype=" & stracctype & "&fromcode=" & strfromcode & "&tocode=" & strtocode _
            '           & "&fromcat=" & strcatcode & "&tocat=" & strcatcodeto & "&fromglcode=" & strglcode & "&toglcode=" & strglcodeto _
            '           & "&currtype=" & strcurrtype & "&ledgertype=" & ledgertype & "&pdcyesno=" & pdcyesno & "&ststement=" & ststement & "&ageing=" & ageing & "','RepCustLedger','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"

            strpop = "window.open('rptsupp_custledgerReport.aspx?Pageame=SupplierLedger&BackPageName=RptSupplierLedger.aspx&type=C&fromdate=" & strfromdate & "&todate=" & strtodate _
                       & "&fromctry=" & txtcountrycode.Text & "&toctry=" & txtcountrycode.Text & "&frommarketcode=" & txtmarketcode.Text & "&tomarketcode=" & txtmarketcode.Text & "&actype=" & stracctype & "&fromcode=" & txtcustomercode.Text & "&tocode=" & txtcustomercode.Text _
                       & "&fromcat=" & txtcategorycode.Text & "&tocat=" & txtcategorycode.Text & "&fromglcode=" & txtcontrolcode.Text & "&toglcode=" & txtcontrolcode.Text & "&divid=" & ViewState("divcode") _
                       & "&currtype=" & strcurrtype & "&ledgertype=" & ledgertype & "&custgroup_sp_type=" & custgroup_sp_type & "&pdcyesno=" & pdcyesno & "&ststement=" & ststement & "&ageing=" & ageing & "','RepCustLedger');"



            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptCustomerLedger.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try



    End Sub

    'Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
    '    Try


    '        If ValidatePage() = False Then
    '            Exit Sub
    '        End If
    '        'Session.Add("Pageame", "SupplierLedger")
    '        'Session.Add("BackPageName", "RptSupplierLedger.aspx")

    '        Dim strReportTitle As String = ""
    '        Dim strfromdate As String = ""
    '        Dim strtodate As String = ""
    '        Dim stracctype As String = ""

    '        Dim strcurrtype As String = ""
    '        Dim strfromcode As String = ""
    '        Dim strtocode As String = ""
    '        Dim strglcode As String = ""
    '        Dim strglcodeto As String = ""
    '        Dim strcatcode As String = ""
    '        Dim strcatcodeto As String = ""
    '        Dim strfromcity As String = ""
    '        Dim strtocity As String = ""
    '        Dim strfromctry As String = ""
    '        Dim strtoctry As String = ""
    '        Dim ledgertype As String = ""
    '        Dim pdcyesno As String = ""
    '        Dim ageing As String = 0
    '        Dim custgroup_sp_type As String

    '        custgroup_sp_type = Txtcustgroupcode.Text

    '        Dim ststement As String = "0"


    '        If Request.QueryString("custstatement") <> "" Then
    '            If Request.QueryString("custstatement") = 1 Then
    '                ststement = "1"
    '            End If
    '        End If

    '        strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
    '        strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
    '        stracctype = "C"

    '        ledgertype = ddlLedgerType.Value
    '        pdcyesno = ddlPDC.Value

    '        Select Case ddlCurrency.SelectedIndex
    '            Case 0
    '                strcurrtype = 1
    '            Case 1
    '                strcurrtype = 0

    '            Case Else
    '                strcurrtype = ddlCurrency.SelectedIndex

    '        End Select
    '        If Request.QueryString("custstatement") = 1 Then
    '            lblHeading.Text = "Customer Ledger  Statement "


    '            ageing = ddlAgeing.SelectedIndex


    '        Else
    '            ageing = 0

    '        End If
    '        Dim strpop As String = ""
    '        strpop = "window.open('TransactionReports.aspx?printId=CustomerLedger&type=C&fromdate=" & strfromdate & "&todate=" & strtodate _
    '                   & "&fromctry=" & txtcountrycode.Text & "&toctry=" & txtcountrycode.Text & "&frommarketcode=" & txtmarketcode.Text & "&tomarketcode=" & txtmarketcode.Text & "&actype=" & stracctype & "&fromcode=" & txtcustomercode.Text & "&tocode=" & txtcustomercode.Text _
    '                   & "&fromcat=" & txtcategorycode.Text & "&tocat=" & txtcategorycode.Text & "&fromglcode=" & txtcontrolcode.Text & "&toglcode=" & txtcontrolcode.Text & "&divid=" & ViewState("divcode") _
    '                   & "&currtype=" & strcurrtype & "&ledgertype=" & ledgertype & "&custgroup_sp_type=" & custgroup_sp_type & "&pdcyesno=" & pdcyesno & "&ststement=" & ststement & "&ageing=" & ageing & "','RepCustLedger');"

    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    '        'End If

    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        objUtils.WritErrorLog("rptCustomerLedger.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

    '    End Try



    'End Sub



    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptCustomerLedger','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
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

    Protected Sub Button1_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
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
            stracctype = "C"



            strtocode = IIf(txtcustomercode.Text <> "", txtcustomercode.Text, "")
            strfromcode = IIf(txtcustomercode.Text <> "", txtcustomercode.Text, "")
            strglcodeto = IIf(txtcontrolcode.Text <> "", txtcontrolcode.Text, "")
            strglcode = IIf(txtcontrolcode.Text <> "", txtcontrolcode.Text, "")
            strcatcodeto = IIf(txtcategorycode.Text <> "", txtcategorycode.Text, "")
            strcatcode = IIf(txtcategorycode.Text <> "", txtcategorycode.Text, "")
            strtocity = IIf(txtmarketcode.Text <> "", txtmarketcode.Text, "")
            strfromcity = IIf(txtmarketcode.Text <> "", txtmarketcode.Text, "")
            strfromctry = IIf(txtcountrycode.Text <> "", txtcountrycode.Text, "")
            strtoctry = IIf(txtcountrycode.Text <> "", txtcountrycode.Text, "")


            Select Case ddlCurrency.SelectedIndex
                Case 0
                    strcurrtype = 1
                Case 1
                    strcurrtype = 0

                Case Else
                    strcurrtype = ddlCurrency.SelectedIndex

            End Select

            ledgertype = ddlLedgerType.Value
            pdcyesno = IIf(ddlPDC.Value = "Yes", "1", "0")


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
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"),
                                          "sp_party_ledger_xls", parms)

            If ds.Tables.Count > 0 Then

                If ds.Tables(0).Rows.Count > 0 Then
                    objUtils.ExportToExcel(ds, Response)
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Record found' );", True)
                End If

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptCustomerLedger.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub


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
    Public Shared Function Getcity(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

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

            strSqlQry = "select ctryname,ctrycode from ctrymast where ctryname like  '" & Trim(prefixText) & "%' order by ctryname "
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

            strSqlQry = "select agentcatname,agentcatcode from agentcatmast where agentcatname like  '" & Trim(prefixText) & "%' order by agentcatname"
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
    Public Shared Function Getcontrolname(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim controlname As New List(Of String)

        Try



            strSqlQry = "select acctname,acctcode from acctmast where  controlyn='Y'and div_code='" & divcode & "' and acctname like  '" & Trim(prefixText) & "%' order by acctname"
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
            Dim ageing As String = 0
            Dim custgroup_sp_type As String

            custgroup_sp_type = Txtcustgroupcode.Text

            Dim ststement As String = "0"


            If Request.QueryString("custstatement") <> "" Then
                If Request.QueryString("custstatement") = 1 Then
                    ststement = "1"
                End If
            End If

            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            stracctype = "C"

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
            If Request.QueryString("custstatement") = 1 Then
                lblHeading.Text = "Customer Ledger  Statement "


                ageing = ddlAgeing.SelectedIndex


            Else
                ageing = 0

            End If
            Dim strpop As String = ""
            strpop = "window.open('TransactionReports.aspx?printId=CustomerLedger&reportsType=pdf&type=C&fromdate=" & strfromdate & "&todate=" & strtodate _
                     & "&fromname=" & txtcustomername.Text & "&fromglname=" & txtcontrolname.Text & "&fromctryname=" & txtcountryname.Text & "&frommkname=" & txtmarketname.Text & "&fromcatname=" & txtcategoryname.Text & "&fromctry=" & txtcountrycode.Text & "&toctry=" & txtcountrycode.Text & "&frommarketcode=" & txtmarketcode.Text & "&tomarketcode=" & txtmarketcode.Text & "&actype=" & stracctype & "&fromcode=" & txtcustomercode.Text & "&tocode=" & txtcustomercode.Text _
                       & "&fromcat=" & txtcategorycode.Text & "&tocat=" & txtcategorycode.Text & "&fromglcode=" & txtcontrolcode.Text & "&toglcode=" & txtcontrolcode.Text & "&divid=" & ViewState("divcode") _
                       & "&currtype=" & strcurrtype & "&ledgertype=" & ledgertype & "&custgroup_sp_type=" & custgroup_sp_type & "&pdcyesno=" & pdcyesno & "&ststement=" & ststement & "&ageing=" & ageing & "','RepCustLedger');"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptCustomerLedger.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

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
            Dim ageing As String = 0
            Dim custgroup_sp_type As String

            custgroup_sp_type = Txtcustgroupcode.Text

            Dim ststement As String = "0"


            If Request.QueryString("custstatement") <> "" Then
                If Request.QueryString("custstatement") = 1 Then
                    ststement = "1"
                End If
            End If

            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            stracctype = "C"

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
            If Request.QueryString("custstatement") = 1 Then
                lblHeading.Text = "Customer Ledger  Statement "


                ageing = ddlAgeing.SelectedIndex


            Else
                ageing = 0

            End If
            Dim strpop As String = ""
            strpop = "window.open('TransactionReports.aspx?printId=CustomerLedger&reportsType=excel&type=C&fromdate=" & strfromdate & "&todate=" & strtodate _
                     & "&fromname=" & txtcustomername.Text & "&fromglname=" & txtcontrolname.Text & "&fromctryname=" & txtcountryname.Text & "&frommkname=" & txtmarketname.Text & "&fromcatname=" & txtcategoryname.Text & "&fromctry=" & txtcountrycode.Text & "&toctry=" & txtcountrycode.Text & "&frommarketcode=" & txtmarketcode.Text & "&tomarketcode=" & txtmarketcode.Text & "&actype=" & stracctype & "&fromcode=" & txtcustomercode.Text & "&tocode=" & txtcustomercode.Text _
                       & "&fromcat=" & txtcategorycode.Text & "&tocat=" & txtcategorycode.Text & "&fromglcode=" & txtcontrolcode.Text & "&toglcode=" & txtcontrolcode.Text & "&divid=" & ViewState("divcode") _
                       & "&currtype=" & strcurrtype & "&ledgertype=" & ledgertype & "&custgroup_sp_type=" & custgroup_sp_type & "&pdcyesno=" & pdcyesno & "&ststement=" & ststement & "&ageing=" & ageing & "','RepCustLedger');"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("rptCustomerLedger.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try



    End Sub

End Class
