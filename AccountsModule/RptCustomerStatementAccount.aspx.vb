''------------================--------------=======================------------------================
''   Module Name    :    RptCustomerStatementAccount.aspx
''   Developer Name :    Nilesh Sawant
''   Date           :    29 SEP 2008
''   
''
''------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Globalization
#End Region

Partial Class RptCustomerStatementAccount
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim SqlCmd As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim dpFDate As EclipseWebSolutions.DatePicker.DatePicker
    Dim dpTDate As EclipseWebSolutions.DatePicker.DatePicker
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim ObjDate As New clsDateTime
    Dim sqlTrans As SqlTransaction
    Dim strappid As String = ""
    Dim strappname As String = ""

#End Region

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

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            ddlAgeing.Focus()

            Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
            Dim AppId As String = CType(Request.QueryString("appid"), String)

            If AppId Is Nothing = False Then
                strappid = AppId
            End If

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
                                                   CType(strappname, String), "AccountsModule\RptCustomerStatementAccount.aspx?appid=" + strappid, btnadd, Button1, btnPdfReport, gv_SearchResult)
            End If

            '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
            '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            ViewState.Add("divcode", divid)
            Session("divcode") = ViewState("divcode")
            If Page.IsPostBack = False Then
                'hidecontrols()
                If Request.QueryString("ageing_tran_type") Is Nothing = False Then
                    If Request.QueryString("ageing_tran_type") = "customerageing1" Then
                        lblHeading.Text = "Customer Pending Invoices"
                        ddlIncludeZero.Visible = False
                        lblIncludeZero.Visible = False
                        ddlwithmovmt.Visible = False
                        lblReportType.Visible = False
                    End If
                    If Request.QueryString("ageing_tran_type") = "customerageing2" Then
                        lblHeading.Text = "Customer Pending Invoices - II"
                        ddlIncludeZero.Disabled = True
                        ddlwithmovmt.Visible = False
                        lblReportType.Visible = False
                    End If
                End If
                ImgBtnToDate.Visible = False
                lbltodate.Visible = False


                Dim c As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=457")
                ddlCurrencyType.Items.Clear()
                ddlCurrencyType.Items.Add("A/C Currency")
                ddlCurrencyType.Items.Add(c)
                txtaccode.Attributes.Add("readonly", "readonly")
                txtcountrycode.Attributes.Add("readonly", "readonly")
                txtmarketcode.Attributes.Add("readonly", "readonly")
                txtcategorycode.Attributes.Add("readonly", "readonly")
                txtcontrolcode.Attributes.Add("readonly", "readonly")
                txtmarketcode.Attributes.Add("readonly", "readonly")


                '--------------------------------------------------------------------------------------
                '--------------------------------------------------------------------------------------
                'ddlAgeing.SelectedIndex = 1


                'txtFromDate.Text = ObjDate.GetSystemDateTime(Session("dbconnectionName")).Day & "/" & ObjDate.GetSystemDateTime(Session("dbconnectionName")).Month & "/" & ObjDate.GetSystemDateTime(Session("dbconnectionName")).Year
                'txtToDate.Text = ObjDate.GetSystemDateTime(Session("dbconnectionName")).Day & "/" & ObjDate.GetSystemDateTime(Session("dbconnectionName")).Month & "/" & ObjDate.GetSystemDateTime(Session("dbconnectionName")).Year
                txtFromDate.Text = Format(CType(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy")
                txtToDate.Text = Format(CType(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy")
                txtagDate.Text = Format(CType(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyyy")

                rdbtnAsOnDate.Checked = True

                ' txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")

                txtToDate.Visible = False

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

                If Request.QueryString("agdate") <> "" Then
                    txtagDate.Text = Format("U", CType(Request.QueryString("agdate"), Date))
                End If

                If Request.QueryString("fromaccode") <> "" Then

                    txtacname.Text = Request.QueryString("fromaccode")
                    txtaccode.Text = Request.QueryString("fromaccode")
                End If

                If Request.QueryString("remarks") <> "" Then
                    txtRemark.Value = Request.QueryString("remarks")
                End If
                If Request.QueryString("agingtype") <> "" Then
                    ddlAgeing.SelectedIndex = Request.QueryString("agingtype")
                End If
                If Request.QueryString("includezero") <> "" Then
                    ddlIncludeZero.SelectedIndex = Request.QueryString("includezero")
                    '  IIf(Request.QueryString("includezero") = 0, ddlIncludeZero.SelectedIndex = 0, ddlIncludeZero.SelectedIndex = 1)
                End If

                '--------------------------------------------------------------------------------------
            Else

            End If
            checkrb_status()

            Dim typ As Type
            typ = GetType(DropDownList)
            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("RptCustomerStatementAccount.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub rdbtnFromToDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lblasdate.Text = "From Date"
        txtToDate.Visible = True
        txtToDate.Enabled = True
        ImgBtnToDate.Visible = True
        lbltodate.Visible = True
        txtToDate.Text = ObjDate.GetSystemDateTime(Session("dbconnectionName")).Day & "/" & ObjDate.GetSystemDateTime(Session("dbconnectionName")).Month & "/" & ObjDate.GetSystemDateTime(Session("dbconnectionName")).Year
    End Sub

    Protected Sub rdbtnAsOnDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lblasdate.Text = "As On Date"
        txtToDate.Enabled = False
        txtToDate.Visible = False
        ImgBtnToDate.Visible = False
        lbltodate.Visible = False
    End Sub
    Public Sub checkrb_status()
        If rdbtnAsOnDate.Checked = True Then
            txtToDate.Visible = False
            ImgBtnToDate.Visible = False
        Else
            txtToDate.Visible = True
            ImgBtnToDate.Visible = True
        End If

    End Sub


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
    Public Shared Function Getactype(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim agent As New List(Of String)
        Dim city, ctry, custgroup, custagent As String
        ctry = ""
        city = ""
        custgroup = ""
        custagent = ""
        Dim div As String
        Try
            div = HttpContext.Current.Session("divcode")
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
            strSqlQry = "select agentname,agentcode from agentmast  where divcode='" & div & "' and  agentname like  '" & Trim(prefixText) & "%'  "
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
    Public Shared Function Getcontrolname(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim controlname As New List(Of String)
        Dim div As String
        Try
            div = HttpContext.Current.Session("divcode")

            strSqlQry = "select acctname,acctcode from acctmast where controlyn='Y' and cust_supp = 'C' and div_code='" & div & "' and acctname like  '" & Trim(prefixText) & "%' order by acctname"
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

            strSqlQry = "select ctryname,ctrycode from ctrymast where ctryname like  '" & Trim(prefixText) & "%' order by ctryname"
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

    Public Function validation() As Boolean
        Dim frmdate As DateTime
        Dim MyCultureInfo As New CultureInfo("fr-Fr")
        Dim ds As DataSet


        frmdate = DateTime.Parse(txtFromDate.Text, MyCultureInfo, DateTimeStyles.None)

        validation = True

        If txtFromDate.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date field can not be blank.');", True)
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtFromDate.ClientID + "');", True)
            SetFocus(txtFromDate)
            validation = False
            Exit Function
        End If
        If txtToDate.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date field can not be blank.');", True)
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
            SetFocus(txtToDate)
            validation = False
            Exit Function
        End If
        If rdbtnAsOnDate.Checked = False Then
            If CType(ObjDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(ObjDate.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                SetFocus(txtToDate)
                validation = False
                Exit Function
            End If
        End If





        ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=1103")
        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                If IsDBNull(ds.Tables(0).Rows(0)(0)) = False Then
                    If frmdate < ds.Tables(0).Rows(0)("option_selected") Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date cannot enter below the " & ds.Tables(0).Rows(0)("option_selected") & " ' );", True)
                        validation = False
                        Exit Function
                    End If
                End If
            End If
        End If

    End Function

    Protected Sub btnLoadReprt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadReprt.Click
        Try
            'Session.Add("Pageame", "RptCustomerStatementAccount")
            'Session.Add("BackPageName", "RptCustomerStatementAccount.aspx")


            Dim strReportTitle As String = ""

            Dim strfromdate As String = ""
            Dim strtodate As String = ""
            Dim stragDate As String = ""

            Dim strfomaccode As String = ""
            Dim strtoaccode As String = ""
            Dim strfromctrlcode As String = ""
            Dim strtoctrlcode As String = ""
            Dim strfromccatcode As String = ""
            Dim strtoccatcode As String = ""
            Dim strfrommarketcode As String = ""
            Dim strtomarketcode As String = ""
            Dim strfromctrycode As String = ""
            Dim strtoctrycode As String = ""
            Dim strremarks As String = ""

            Dim strtype As String = "C"
            Dim strdatetype As Integer = 0 '0 as on date, 1 from -to date
            Dim stragingtype As Integer = 0
            Dim stragingreport As Integer = 0 '0 -Customer Statement, 1 - Customer Ageing Report-I , 2- Customer Ageing Report-II
            Dim strpdcyesno As Integer = 0
            Dim strincludezero As Integer = 0 ' 0 no, 1 yes
            Dim strcurr As Integer = 0 ' 0 Ac Currency, 1 base currency

            Dim strreporttype As String
            Dim strrepfilter As String = ""

            Dim strrpttype As Integer = 0

            Dim strincludeproforma As Integer = 0

            strreporttype = Session("CustomerStatement")

            If validation() = False Then
                Exit Sub
            End If


            strdatetype = IIf(rdbtnAsOnDate.Checked = True, 0, 1)
            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strincludeproforma = IIf(ddlproforma.Value = "Yes", 1, 0)
            If rdbtnAsOnDate.Checked = True Then
                txtToDate.Text = txtFromDate.Text
            End If

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



            strtype = "C"
            strcurr = Val(ddlCurrencyType.SelectedIndex)


            stragingtype = Val(ddlAgeing.SelectedIndex)
            strpdcyesno = 0
            strincludezero = ddlIncludeZero.SelectedIndex

            strremarks = Trim(txtRemark.Value)

            strrpttype = ddlwithmovmt.SelectedIndex
            If Request.QueryString("ageing_tran_type") Is Nothing = False Then

                strReportTitle = "Customer Ageing Report"
                If Request.QueryString("ageing_tran_type") = "customerageing1" Then
                    stragingreport = 1
                Else
                    stragingreport = 2
                End If

            Else
                strReportTitle = "Customer Statement"
            End If

            Dim strpop As String = ""
            'strpop = "window.open('rptStatement.aspx?Pageame=RptCustomerStatementAccount&BackPageName=RptCustomerStatementAccount.aspx&fromdate=" & strfromdate & "&todate=" & strtodate _
            '& "&datetype=" & strdatetype & "&type=" & strtype & "&curr=" & strcurr & "&fromaccode=" & strfomaccode _
            '& "&toaccode=" & strtoaccode & "&fromctrlcode=" & strfromctrlcode & "&toctrlcode=" & strtoctrlcode _
            '& "&fromccatcode=" & strfromccatcode & "&toccatcode=" & strtoccatcode & "&frommarketcode=" & strfrommarketcode _
            '& "&tomarketcode=" & strtomarketcode & "&fromctrycode=" & strfromctrycode & "&toctrycode=" & strtoctrycode _
            '& "&agingtype=" & stragingtype & "&pdcyesno=" & strpdcyesno & "&strincludeproforma=" & strincludeproforma _
            '& "&includezero=" & strincludezero & "&rpttype=" & strrpttype & "&remarks=" & strremarks & "&reporttype=" & strreporttype _
            '& "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&ageingreporttyp=" & stragingreport & "&agdate=" & stragDate & "','RepCustStAc','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"

            strpop = "window.open('rptStatement.aspx?Pageame=RptCustomerStatementAccount&BackPageName=RptCustomerStatementAccount.aspx&fromdate=" & strfromdate & "&todate=" & strtodate _
            & "&datetype=" & strdatetype & "&type=" & strtype & "&curr=" & strcurr & "&fromaccode=" & txtaccode.Text _
            & "&toaccode=" & txtaccode.Text & "&fromctrlcode=" & txtcontrolcode.Text & "&toctrlcode=" & txtcontrolcode.Text & "&divid=" & ViewState("divcode") _
            & "&fromccatcode=" & txtcategorycode.Text & "&custgroup_sp_type=" & Txtcustgroupcode.Text & "&toccatcode=" & txtcategorycode.Text & "&frommarketcode=" & txtmarketcode.Text _
            & "&tomarketcode=" & txtmarketcode.Text & "&fromctrycode=" & txtcountrycode.Text & "&toctrycode=" & txtcountrycode.Text _
            & "&agingtype=" & stragingtype & "&pdcyesno=" & strpdcyesno & "&strincludeproforma=" & strincludeproforma _
            & "&includezero=" & strincludezero & "&rpttype=" & strrpttype & "&remarks=" & strremarks & "&reporttype=" & strreporttype _
            & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&ageingreporttyp=" & stragingreport & "&agdate=" & stragDate & "','RepCustStAc');"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptCustomerStatementAccount.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

    'Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
    '    Try

    '        'Session.Add("Pageame", "RptCustomerStatementAccount")
    '        'Session.Add("BackPageName", "RptCustomerStatementAccount.aspx")
    '        Dim strReportTitle As String = ""

    '        Dim strfromdate As String = ""
    '        Dim strtodate As String = ""
    '        Dim stragDate As String = ""

    '        Dim strfomaccode As String = ""
    '        Dim strtoaccode As String = ""
    '        Dim strfromctrlcode As String = ""
    '        Dim strtoctrlcode As String = ""
    '        Dim strfromccatcode As String = ""
    '        Dim strtoccatcode As String = ""
    '        Dim strfrommarketcode As String = ""
    '        Dim strtomarketcode As String = ""
    '        Dim strfromctrycode As String = ""
    '        Dim strtoctrycode As String = ""
    '        Dim strremarks As String = ""

    '        Dim strtype As String = "C"
    '        Dim strdatetype As Integer = 0 '0 as on date, 1 from -to date
    '        Dim stragingtype As Integer = 0
    '        Dim stragingreport As Integer = 0 '0 -Customer Statement, 1 - Customer Ageing Report-I , 2- Customer Ageing Report-II
    '        Dim strpdcyesno As Integer = 0
    '        Dim strincludezero As Integer = 0 ' 0 no, 1 yes
    '        Dim strcurr As Integer = 0 ' 0 Ac Currency, 1 base currency

    '        Dim strreporttype As String
    '        Dim strrepfilter As String = ""

    '        Dim strrpttype As Integer = 0

    '        Dim strincludeproforma As Integer = 0

    '        strreporttype = Session("CustomerStatement")

    '        If validation() = False Then
    '            Exit Sub
    '        End If


    '        strdatetype = IIf(rdbtnAsOnDate.Checked = True, 0, 1)
    '        strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
    '        strincludeproforma = IIf(ddlproforma.Value = "Yes", 1, 0)
    '        If rdbtnAsOnDate.Checked = True Then
    '            txtToDate.Text = txtFromDate.Text
    '        End If

    '        If Trim(txtToDate.Text) <> "" Then
    '            strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
    '        Else
    '            strtodate = strfromdate
    '        End If

    '        'stragDate = Mid(Format(CType(txtagDate.Text, Date), "yyyy/MM/dd"), 1, 10)
    '        If Trim(txtToDate.Text) <> "" Then
    '            stragDate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
    '        Else
    '            stragDate = strfromdate
    '        End If



    '        strtype = "C"
    '        strcurr = Val(ddlCurrencyType.SelectedIndex)


    '        stragingtype = Val(ddlAgeing.SelectedIndex)
    '        strpdcyesno = 0
    '        strincludezero = ddlIncludeZero.SelectedIndex

    '        strremarks = Trim(txtRemark.Value)

    '        strrpttype = ddlwithmovmt.SelectedIndex
    '        If Request.QueryString("ageing_tran_type") Is Nothing = False Then

    '            strReportTitle = "Customer Ageing Report"
    '            If Request.QueryString("ageing_tran_type") = "customerageing1" Then
    '                stragingreport = 1
    '            Else
    '                stragingreport = 2
    '            End If

    '        Else
    '            strReportTitle = "Customer Statement"
    '        End If

    '        Dim strpop As String = ""
    '        strpop = "window.open('TransactionReports.aspx?printId=CustomerStatement&fromdate=" & strfromdate & "&todate=" & strtodate _
    '        & "&datetype=" & strdatetype & "&type=" & strtype & "&curr=" & strcurr & "&fromaccode=" & txtaccode.Text _
    '        & "&toaccode=" & txtaccode.Text & "&fromctrlcode=" & txtcontrolcode.Text & "&toctrlcode=" & txtcontrolcode.Text & "&divid=" & ViewState("divcode") _
    '        & "&fromccatcode=" & txtcategorycode.Text & "&custgroup_sp_type=" & Txtcustgroupcode.Text & "&toccatcode=" & txtcategorycode.Text & "&frommarketcode=" & txtmarketcode.Text _
    '        & "&tomarketcode=" & txtmarketcode.Text & "&fromctrycode=" & txtcountrycode.Text & "&toctrycode=" & txtcountrycode.Text _
    '        & "&agingtype=" & stragingtype & "&pdcyesno=" & strpdcyesno & "&strincludeproforma=" & strincludeproforma _
    '        & "&includezero=" & strincludezero & "&rpttype=" & strrpttype & "&remarks=" & strremarks & "&reporttype=" & strreporttype _
    '        & "&repfilter=" & strrepfilter & "&reporttitle=" & strReportTitle & "&ageingreporttyp=" & stragingreport & "&agdate=" & stragDate & "','RepCustStAc');"

    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    '    Catch ex As Exception
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
    '        objUtils.WritErrorLog("RptCustomerStatementAccountNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

    '    End Try

    'End Sub
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptCustomerStatementAccount','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub Button1_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try

            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(18) As SqlParameter

            Dim strReportTitle As String = ""

            Dim strfromdate As String = ""
            Dim strtodate As String = ""
            Dim strasdate As String = ""

            Dim strfomaccode As String = ""
            Dim strtoaccode As String = ""
            Dim strfromctrlcode As String = ""
            Dim strtoctrlcode As String = ""
            Dim strfromccatcode As String = ""
            Dim strtoccatcode As String = ""
            Dim strfrommarketcode As String = ""
            Dim strtomarketcode As String = ""
            Dim strfromctrycode As String = ""
            Dim strtoctrycode As String = ""
            Dim strremarks As String = ""

            Dim strtype As String = "C"
            Dim strdatetype As Integer = 0 '0 as on date, 1 from -to date
            Dim stragingtype As Integer = 0
            Dim strpdcyesno As Integer = 0
            Dim strincludezero As Integer = 0 ' 0 no, 1 yes
            Dim strcurr As Integer = 0 ' 0 Ac Currency, 1 base currency

            Dim strreporttype As String
            Dim strrepfilter As String = ""

            Dim strrpttype As Integer = 0 ' 0-Summary,1-Detail 2-Pending Invoice

            strreporttype = Session("CustomerStatement")
            If validation() = False Then
                Exit Sub
            End If

            strdatetype = IIf(rdbtnAsOnDate.Checked = True, 0, 1)
            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            If Trim(txtToDate.Text) <> "" Then
                strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            Else
                strtodate = strfromdate
            End If


            strtype = "C"
            strcurr = Val(ddlCurrencyType.SelectedIndex)



            strtoaccode = IIf(txtaccode.Text <> "", txtaccode.Text, "")

            strfomaccode = IIf(txtaccode.Text <> "", txtaccode.Text, "")
            strtoctrlcode = IIf(txtcontrolcode.Text <> "", txtcontrolcode.Text, "")
            strfromctrlcode = IIf(txtcontrolcode.Text <> "", txtcontrolcode.Text, "")
            strtoccatcode = IIf(txtcategorycode.Text <> "", txtcategorycode.Text, "")
            strfromccatcode = IIf(txtcategorycode.Text <> "", txtcategorycode.Text, "")
            strfrommarketcode = IIf(txtmarketcode.Text <> "", txtmarketcode.Text, "")
            strtomarketcode = IIf(txtmarketcode.Text <> "", txtmarketcode.Text, "")
            strtoctrycode = IIf(txtcountrycode.Text <> "", txtcountrycode.Text, "")
            strfromctrycode = IIf(txtcountrycode.Text <> "", txtcountrycode.Text, "")


            stragingtype = Val(ddlAgeing.SelectedIndex)
            strpdcyesno = 0


            strremarks = Trim(txtRemark.Value)

            If Request.QueryString("ageing_tran_type") Is Nothing = False Then
                'If Request.QueryString("ageing_tran_type") = "customerageing1" Then
                strincludezero = 1
                strrpttype = 2
                'Else
                'strincludezero = 1
                'strrpttype = 2
                'End If
            Else
                strincludezero = ddlIncludeZero.SelectedIndex
                strrpttype = ddlwithmovmt.SelectedIndex
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
            parm(11) = New SqlParameter("@fromcity", strfrommarketcode)
            parm(12) = New SqlParameter("@tocity", strfrommarketcode)
            parm(13) = New SqlParameter("@fromctry", strfromctrycode)
            parm(14) = New SqlParameter("@toctry", strtoctrycode)
            parm(15) = New SqlParameter("@agingtype", stragingtype)
            parm(16) = New SqlParameter("@pdcyesno", strpdcyesno)
            parm(17) = New SqlParameter("@includezero", strincludezero)
            parm(18) = New SqlParameter("@rpttype", strrpttype)


            For i = 0 To 18
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_statement_party_xls", parms)

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
    Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
        Try

            'Session.Add("Pageame", "RptCustomerStatementAccount")
            'Session.Add("BackPageName", "RptCustomerStatementAccount.aspx")
            Dim strReportTitle As String = ""

            Dim strfromdate As String = ""
            Dim strtodate As String = ""
            Dim stragDate As String = ""

            Dim strfomaccode As String = ""
            Dim strtoaccode As String = ""
            Dim strfromctrlcode As String = ""
            Dim strtoctrlcode As String = ""
            Dim strfromccatcode As String = ""
            Dim strtoccatcode As String = ""
            Dim strfrommarketcode As String = ""
            Dim strtomarketcode As String = ""
            Dim strfromctrycode As String = ""
            Dim strtoctrycode As String = ""
            Dim strremarks As String = ""
            Dim reportfilter As String = ""
            Dim strtype As String = "C"
            Dim strdatetype As Integer = 0 '0 as on date, 1 from -to date
            Dim stragingtype As Integer = 0
            Dim stragingreport As Integer = 0 '0 -Customer Statement, 1 - Customer Ageing Report-I , 2- Customer Ageing Report-II
            Dim strpdcyesno As Integer = 0
            Dim strincludezero As Integer = 0 ' 0 no, 1 yes
            Dim strcurr As Integer = 0 ' 0 Ac Currency, 1 base currency

            Dim strreporttype As String
            Dim strrepfilter As String = ""

            Dim strrpttype As Integer = 0

            Dim strincludeproforma As Integer = 0

            strreporttype = Session("CustomerStatement")

            If validation() = False Then
                Exit Sub
            End If


            strdatetype = IIf(rdbtnAsOnDate.Checked = True, 0, 1)
            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strincludeproforma = IIf(ddlproforma.Value = "Yes", 1, 0)
            If rdbtnAsOnDate.Checked = True Then
                txtToDate.Text = txtFromDate.Text
            End If

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

            If Trim(txtFromDate.Text) <> "" And Trim(txtToDate.Text) <> "" Then
                reportfilter = "From Date :" & Space(2) & Convert.ToDateTime(strfromdate).ToString("dd/MM/yyyy") & Space(2) & "To" & Space(2) & Convert.ToDateTime(strtodate).ToString("dd/MM/yyyy")
            End If
            If Trim(txtacname.Text) <> "" Then
                reportfilter = reportfilter & Space(2) & "Customer Name  :" & Space(2) & txtacname.Text & Space(2)
            End If

            If Trim(txtmarketname.Text) <> "" Then

                reportfilter = reportfilter & Space(2) & ":Market :" & Space(2) & txtmarketname.Text & Space(2)
            End If

            If Trim(txtcountryname.Text) <> "" Then
                reportfilter = reportfilter & Space(2) & "Country : " & Space(2) & txtcountryname.Text & Space(2)
            End If
            If Trim(txtcategoryname.Text) <> "" Then
                reportfilter = reportfilter & Space(2) & "Category :   " & Space(2) & txtcategoryname.Text & Space(2)
            End If
            If Trim(txtcontrolname.Text) <> "" Then
                reportfilter = reportfilter & Space(2) & "Control Account :  " & Space(2) & txtcontrolname.Text & Space(2)
            End If

            strtype = "C"
            strcurr = Val(ddlCurrencyType.SelectedIndex)


            stragingtype = Val(ddlAgeing.SelectedIndex)
            strpdcyesno = 0
            strincludezero = ddlIncludeZero.SelectedIndex

            strremarks = Trim(txtRemark.Value)

            strrpttype = ddlwithmovmt.SelectedIndex
            If Request.QueryString("ageing_tran_type") Is Nothing = False Then

                strReportTitle = "Customer Ageing Report"
                If Request.QueryString("ageing_tran_type") = "customerageing1" Then
                    stragingreport = 1
                Else
                    stragingreport = 2
                End If

            Else
                strReportTitle = "Customer Statement"
            End If

            Dim strpop As String = ""
            strpop = "window.open('TransactionReports.aspx?printId=CustomerStatement&reportsType=pdf&fromdate=" & strfromdate & "&todate=" & strtodate _
            & "&datetype=" & strdatetype & "&type=" & strtype & "&curr=" & strcurr & "&fromaccode=" & txtaccode.Text _
            & "&toaccode=" & txtaccode.Text & "&fromctrlcode=" & txtcontrolcode.Text & "&toctrlcode=" & txtcontrolcode.Text & "&divid=" & ViewState("divcode") _
            & "&fromccatcode=" & txtcategorycode.Text & "&custgroup_sp_type=" & Txtcustgroupcode.Text & "&toccatcode=" & txtcategorycode.Text & "&frommarketcode=" & txtmarketcode.Text _
            & "&tomarketcode=" & txtmarketcode.Text & "&fromctrycode=" & txtcountrycode.Text & "&toctrycode=" & txtcountrycode.Text _
            & "&agingtype=" & stragingtype & "&pdcyesno=" & strpdcyesno & "&strincludeproforma=" & strincludeproforma _
            & "&includezero=" & strincludezero & "&rpttype=" & strrpttype & "&remarks=" & strremarks & "&reporttype=" & strreporttype _
            & "&repfilter=" & reportfilter & "&reporttitle=" & strReportTitle & "&ageingreporttyp=" & stragingreport & "&agdate=" & stragDate & "','RepCustStAc');"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptCustomerStatementAccountNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

    Protected Sub btnExcelReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelReport.Click
        Try

            'Session.Add("Pageame", "RptCustomerStatementAccount")
            'Session.Add("BackPageName", "RptCustomerStatementAccount.aspx")
            Dim strReportTitle As String = ""

            Dim strfromdate As String = ""
            Dim strtodate As String = ""
            Dim stragDate As String = ""

            Dim strfomaccode As String = ""
            Dim strtoaccode As String = ""
            Dim strfromctrlcode As String = ""
            Dim strtoctrlcode As String = ""
            Dim strfromccatcode As String = ""
            Dim strtoccatcode As String = ""
            Dim strfrommarketcode As String = ""
            Dim strtomarketcode As String = ""
            Dim strfromctrycode As String = ""
            Dim strtoctrycode As String = ""
            Dim strremarks As String = ""
            Dim reportfilter As String = ""
            Dim strtype As String = "C"
            Dim strdatetype As Integer = 0 '0 as on date, 1 from -to date
            Dim stragingtype As Integer = 0
            Dim stragingreport As Integer = 0 '0 -Customer Statement, 1 - Customer Ageing Report-I , 2- Customer Ageing Report-II
            Dim strpdcyesno As Integer = 0
            Dim strincludezero As Integer = 0 ' 0 no, 1 yes
            Dim strcurr As Integer = 0 ' 0 Ac Currency, 1 base currency

            Dim strreporttype As String
            Dim strrepfilter As String = ""

            Dim strrpttype As Integer = 0

            Dim strincludeproforma As Integer = 0

            strreporttype = Session("CustomerStatement")

            If validation() = False Then
                Exit Sub
            End If


            strdatetype = IIf(rdbtnAsOnDate.Checked = True, 0, 1)
            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strincludeproforma = IIf(ddlproforma.Value = "Yes", 1, 0)
            If rdbtnAsOnDate.Checked = True Then
                txtToDate.Text = txtFromDate.Text
            End If

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

            If Trim(txtFromDate.Text) <> "" And Trim(txtToDate.Text) <> "" Then
                reportfilter = "From" & Space(2) & Convert.ToDateTime(strfromdate).ToString("dd/MM/yyyy") & Space(2) & "To" & Space(2) & Convert.ToDateTime(strtodate).ToString("dd/MM/yyyy")
            End If
            If Trim(txtacname.Text) <> "" Then
                ' reportfilter = reportfilter & Space(2) & "Customer Name From" & Space(2) & txtacname.Text & Space(2) & "To" & Space(2) & txtacname.Text
                reportfilter = reportfilter & Space(2) & "Customer Name :" & Space(2) & txtacname.Text & Space(2)
            End If

            If Trim(txtmarketname.Text) <> "" Then

                ' reportfilter = reportfilter & Space(2) & ":Market From" & Space(2) & txtmarketname.Text & Space(2) & "To" & Space(2) & txtmarketname.Text
                reportfilter = reportfilter & Space(2) & ":Market   :" & Space(2) & txtmarketname.Text & Space(2) & "To" & Space(2)
            End If

            If Trim(txtcountryname.Text) <> "" Then
                '  reportfilter = reportfilter & Space(2) & "Country From" & Space(2) & txtcountryname.Text & Space(2) & "To" & Space(2) & txtcountryname.Text
                reportfilter = reportfilter & Space(2) & "Country   :" & Space(2) & txtcountryname.Text & Space(2)
            End If
            If Trim(txtcategoryname.Text) <> "" Then
                '  reportfilter = reportfilter & Space(2) & "Category  From" & Space(2) & txtcategoryname.Text & Space(2) & "To" & Space(2) & txtcategoryname.Text
                reportfilter = reportfilter & Space(2) & "Category    :" & Space(2) & txtcategoryname.Text & Space(2)
            End If
            If Trim(txtcontrolname.Text) <> "" Then
                ' reportfilter = reportfilter & Space(2) & "Control Account  : " & Space(2) & txtcontrolname.Text & Space(2) & "To" & Space(2) & txtcontrolname.Text
                reportfilter = reportfilter & Space(2) & "Control Account  : " & Space(2) & txtcontrolname.Text & Space(2)
            End If


            strtype = "C"
            strcurr = Val(ddlCurrencyType.SelectedIndex)


            stragingtype = Val(ddlAgeing.SelectedIndex)
            strpdcyesno = 0
            strincludezero = ddlIncludeZero.SelectedIndex

            strremarks = Trim(txtRemark.Value)

            strrpttype = ddlwithmovmt.SelectedIndex
            If Request.QueryString("ageing_tran_type") Is Nothing = False Then

                strReportTitle = "Customer Ageing Report"
                If Request.QueryString("ageing_tran_type") = "customerageing1" Then
                    stragingreport = 1
                Else
                    stragingreport = 2
                End If

            Else
                strReportTitle = "Customer Statement"
            End If

            Dim strpop As String = ""
            strpop = "window.open('TransactionReports.aspx?printId=CustomerStatement&reportsType=excel&fromdate=" & strfromdate & "&todate=" & strtodate _
            & "&datetype=" & strdatetype & "&type=" & strtype & "&curr=" & strcurr & "&fromaccode=" & txtaccode.Text _
            & "&toaccode=" & txtaccode.Text & "&fromctrlcode=" & txtcontrolcode.Text & "&toctrlcode=" & txtcontrolcode.Text & "&divid=" & ViewState("divcode") _
            & "&fromccatcode=" & txtcategorycode.Text & "&custgroup_sp_type=" & Txtcustgroupcode.Text & "&toccatcode=" & txtcategorycode.Text & "&frommarketcode=" & txtmarketcode.Text _
            & "&tomarketcode=" & txtmarketcode.Text & "&fromctrycode=" & txtcountrycode.Text & "&toctrycode=" & txtcountrycode.Text _
            & "&agingtype=" & stragingtype & "&pdcyesno=" & strpdcyesno & "&strincludeproforma=" & strincludeproforma _
            & "&includezero=" & strincludezero & "&rpttype=" & strrpttype & "&remarks=" & strremarks & "&reporttype=" & strreporttype _
            & "&repfilter=" & reportfilter & "&reporttitle=" & strReportTitle & "&ageingreporttyp=" & stragingreport & "&agdate=" & stragDate & "','RepCustStAc');"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptCustomerStatementAccountNew.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub



End Class