'------------================--------------=======================------------------================
'   Module Name    :    RptGeneralLedger.aspx
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

Partial Class RptGeneralLedger
    Inherits System.Web.UI.Page
    Dim objUtils As New clsUtils

#Region "Global Declaration"
    Dim objUser As New clsUser
    Dim objDate As New clsDateTime
    Dim strappid As String = ""
    Dim strappname As String = ""
    Shared divcode As String = ""
#End Region
    <System.Web.Script.Services.ScriptMethod()> _
                <System.Web.Services.WebMethod()> _
    Public Shared Function Getbankslist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim banknames As New List(Of String)
        Dim divid As String = ""
        Try


            strSqlQry = "select * from acctmast where  div_code='" & divcode & "'   and  acctname like  '%" & Trim(prefixText) & "%' order by acctcode"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    banknames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString()))

                Next
            End If
            Return banknames
        Catch ex As Exception
            Return banknames
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
                                                   CType(strappname, String), "AccountsModule\RptGeneralLedger.aspx?appid=" + strappid, btnadd, Button1, btnReport, gv_SearchResult)
            End If

            '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
            '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            ViewState.Add("divcode", divid)
            divcode = ViewState("divcode")
            TxtBankCode.Attributes.Add("readonly", "readonly")
            If IsPostBack = False Then
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                SetFocus(txtFromDate)

                ' ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromAccount, "acctcode", "acctname", "select * from acctmast where div_code='" & ViewState("divcode") & "' order by acctcode ", True)
                ' ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlFromAccountName, "acctname", "acctcode", "select * from acctmast where div_code='" & ViewState("divcode") & "' order by acctname", True)
                ' ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToAccount, "acctcode", "acctname", "select * from acctmast where div_code='" & ViewState("divcode") & "' order by acctcode", True)
                ' ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlToAccountName, "acctname", "acctcode", "select * from acctmast where div_code='" & ViewState("divcode") & "' order by acctname", True)

                '----------------------------- Default Dates
                txtFromDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                txtToDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")


                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format("U", CType(Request.QueryString("fromdate"), Date))
                End If
                If Request.QueryString("todate") <> "" Then

                    txtToDate.Text = Format("U", CType(Request.QueryString("todate"), Date))
                End If
                If Request.QueryString("frommac") <> "" Then

                    ' ''rbACrange.Checked = True

                    ' ''ddlFromAccountName.Value = Request.QueryString("frommac")
                    ' ''ddlFromAccount.Value = ddlFromAccountName.Items(ddlFromAccountName.SelectedIndex).Text

                Else
                    ''' rbACrange.Checked = False

                End If


                If Request.QueryString("toac") <> "" Then
                    ''rbACrange.Checked = True

                    ''ddlToAccountName.Value = Request.QueryString("frommac")
                    ''ddlToAccount.Value = ddlToAccountName.Items(ddlToAccountName.SelectedIndex).Text


                Else
                    '''  rbACrange.Checked = False

                End If


                If Request.QueryString("reptype") <> "" Then
                    ddlLedgerType.SelectedIndex = Request.QueryString("closing")
                End If
                If Request.QueryString("pdcyesno") <> "" Then
                    ddlPDC.SelectedIndex = Request.QueryString("closing")
                End If




                '  txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")
                ''rbACall.Attributes.Add("onclick", "rbevent(this,'" & rbACrange.ClientID & "','A','Account')")
                ''rbACrange.Attributes.Add("onclick", "rbevent(this,'" & rbACall.ClientID & "','R','Account')")



            Else
                checkrb_status()
            End If

            ClientScript.GetPostBackEventReference(Me, String.Empty)
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepGLWindowPostBack") Then
                btnReport_Click(sender, e)
            End If
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                ' ''ddlFromAccount.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ' ''ddlFromAccountName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ' ''ddlToAccount.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ' ''ddlToAccountName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ' ''ddlLedgerType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlPDC.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub

    Public Sub checkrb_status()
        ' ''If rbACall.Checked = True Then
        ' ''    ddlFromAccount.Disabled = True
        ' ''    ddlFromAccountName.Disabled = True
        ' ''    ddlToAccount.Disabled = True
        ' ''    ddlToAccountName.Disabled = True
        ' ''Else
        ' ''    ddlFromAccount.Disabled = False
        ' ''    ddlFromAccountName.Disabled = False
        ' ''    ddlToAccount.Disabled = False
        ' ''    ddlToAccountName.Disabled = False
        ' ''End If
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Try
            'If ValidatePage() = True Then
            If ValidatePage() = False Then
                Exit Sub
            End If

            'Session.Add("Pageame", "GLtrialbalReport")
            'Session.Add("BackPageName", "RptGeneralLedger.aspx")

            Dim strfromdate, strtodate, strfrmac, strtoac, strreptype, strpdctype As String
            strreptype = ddlLedgerType.SelectedIndex.ToString
            strpdctype = ddlPDC.SelectedIndex.ToString

            strfrmac = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "")  '''IIf(UCase(ddlFromAccount.Items(ddlFromAccount.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromAccount.Items(ddlFromAccount.SelectedIndex).Text, "")

            strtoac = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "") '''IIf(UCase(ddlToAccount.Items(ddlToAccount.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToAccount.Items(ddlToAccount.SelectedIndex).Text, "")
            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            'Response.Redirect("rptsupp_custledgerReport.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&fromcode=" & strfrmac & "&tocode=" & strtoac & "&reptype=" & strreptype & "&pdcyesno=" & strpdctype & "&actype=G", False)

            Dim strpop As String = ""
            'strpop = "window.open('rptsupp_custledgerReport.aspx?Pageame=GLtrialbalReport&BackPageName=RptGeneralLedger.aspx&actype=G&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromcode=" & strfrmac & "&tocode=" & strtoac & "&reptype=" & strreptype & "&pdcyesno=" & strpdctype & "','RepGL','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
            strpop = "window.open('rptsupp_custledgerReport.aspx?Pageame=GLtrialbalReport&BackPageName=RptGeneralLedger.aspx&actype=G&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromcode=" & strfrmac & "&tocode=" & strtoac & "&divid=" & ViewState("divcode") & "&reptype=" & strreptype & "&pdcyesno=" & strpdctype & "','RepGL' );"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptGeneralLedger.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

    Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
        Try
            'If ValidatePage() = True Then
            If ValidatePage() = False Then
                Exit Sub
            End If

            'Session.Add("Pageame", "GLtrialbalReport")
            'Session.Add("BackPageName", "RptGeneralLedger.aspx")

            Dim strfromdate, strtodate, strfrmac, strtoac, strreptype, strpdctype As String
            strreptype = ddlLedgerType.SelectedIndex.ToString
            strpdctype = ddlPDC.SelectedIndex.ToString

            strfrmac = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "")  '''IIf(UCase(ddlFromAccount.Items(ddlFromAccount.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromAccount.Items(ddlFromAccount.SelectedIndex).Text, "")

            strtoac = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "") '''IIf(UCase(ddlToAccount.Items(ddlToAccount.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToAccount.Items(ddlToAccount.SelectedIndex).Text, "")
            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            'Response.Redirect("rptsupp_custledgerReport.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&fromcode=" & strfrmac & "&tocode=" & strtoac & "&reptype=" & strreptype & "&pdcyesno=" & strpdctype & "&actype=G", False)

            Dim strpop As String = ""
            'strpop = "window.open('rptsupp_custledgerReport.aspx?Pageame=GLtrialbalReport&BackPageName=RptGeneralLedger.aspx&actype=G&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromcode=" & strfrmac & "&tocode=" & strtoac & "&reptype=" & strreptype & "&pdcyesno=" & strpdctype & "','RepGL','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
            strpop = "window.open('TransactionReports.aspx?printId=GLLedger&reportsType=pdf&actype=G&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromcode=" & strfrmac & "&tocode=" & strtoac & "&fromname=" & TxtBankName.Text & "&divid=" & ViewState("divcode") & "&reptype=" & strreptype & "&pdcyesno=" & strpdctype & "','RepGL' );"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptGeneralLedger.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptGeneralLedger','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Dim frmdate As DateTime
        Dim frmdate1 As DateTime
        Dim opdate As DateTime
        Dim opparam As String
        Dim ds As DataSet

        Dim MyCultureInfo As New CultureInfo("fr-Fr")
        frmdate = DateTime.Parse(txtFromDate.Text, MyCultureInfo, DateTimeStyles.None)
        opparam = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 508)
        opdate = DateTime.Parse(opparam, MyCultureInfo, DateTimeStyles.None)


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

            'If frmdate <= opdate Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date should not less than opening date.');", True)
            '    ValidatePage = False
            '    Exit Function
            'End If

            frmdate1 = DateTime.Parse(txtFromDate.Text, MyCultureInfo, DateTimeStyles.None)
            ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=1103")
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    If IsDBNull(ds.Tables(0).Rows(0)(0)) = False Then
                        If frmdate1 < ds.Tables(0).Rows(0)("option_selected") Then
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

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)


        Dim parms As New List(Of SqlParameter)
        Dim i As Integer
        Dim parm(7) As SqlParameter


        If ValidatePage() = False Then
            Exit Sub
        End If

        'Session.Add("Pageame", "GLtrialbalReport")
        'Session.Add("BackPageName", "RptGeneralLedger.aspx")

        Dim strfromdate, strtodate, strfrmac, strtoac, strreptype, strpdctype As String
        strreptype = ddlLedgerType.SelectedIndex.ToString
        strpdctype = ddlPDC.SelectedIndex.ToString

        strfrmac = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "") '''IIf(UCase(ddlFromAccount.Items(ddlFromAccount.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromAccount.Items(ddlFromAccount.SelectedIndex).Text, "")

        strtoac = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "") ''' IIf(UCase(ddlToAccount.Items(ddlToAccount.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToAccount.Items(ddlToAccount.SelectedIndex).Text, "")
        strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
        strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)

        'Response.Redirect("rptsupp_custledgerReport.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&fromcode=" & strfrmac & "&tocode=" & strtoac & "&reptype=" & strreptype & "&pdcyesno=" & strpdctype & "&actype=G", False)

        parm(0) = New SqlParameter("@frmdate", strfromdate)
        parm(1) = New SqlParameter("@todate", strtodate)
        parm(2) = New SqlParameter("@div_code", divcode)
        parm(3) = New SqlParameter("@actype", "G")
        parm(4) = New SqlParameter("@frmac", strfrmac)
        parm(5) = New SqlParameter("@toac", strtoac)
        parm(6) = New SqlParameter("@reptype", strreptype)
        parm(7) = New SqlParameter("@pdctype", strpdctype)


        For i = 0 To 7
            parms.Add(parm(i))
        Next

        Dim ds As New DataSet
        'ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"),"sp_get_request_search", parms)
        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_rep_general_ledger_xls", parms)

        If ds IsNot Nothing Then
            If ds.Tables(0).Rows.Count > 0 Then
                objUtils.ExportToExcel(ds, Response)
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Record found' );", True)
            End If
        End If


    End Sub
    Protected Sub btnExlReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExlReport.Click
        Try
            'If ValidatePage() = True Then
            If ValidatePage() = False Then
                Exit Sub
            End If

            'Session.Add("Pageame", "GLtrialbalReport")
            'Session.Add("BackPageName", "RptGeneralLedger.aspx")

            Dim strfromdate, strtodate, strfrmac, strtoac, strreptype, strpdctype As String
            strreptype = ddlLedgerType.SelectedIndex.ToString
            strpdctype = ddlPDC.SelectedIndex.ToString

            strfrmac = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "")  '''IIf(UCase(ddlFromAccount.Items(ddlFromAccount.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlFromAccount.Items(ddlFromAccount.SelectedIndex).Text, "")

            strtoac = IIf(TxtBankCode.Text <> "", TxtBankCode.Text, "") '''IIf(UCase(ddlToAccount.Items(ddlToAccount.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlToAccount.Items(ddlToAccount.SelectedIndex).Text, "")
            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)

            'Response.Redirect("rptsupp_custledgerReport.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&fromcode=" & strfrmac & "&tocode=" & strtoac & "&reptype=" & strreptype & "&pdcyesno=" & strpdctype & "&actype=G", False)

            Dim strpop As String = ""
            'strpop = "window.open('rptsupp_custledgerReport.aspx?Pageame=GLtrialbalReport&BackPageName=RptGeneralLedger.aspx&actype=G&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromcode=" & strfrmac & "&tocode=" & strtoac & "&reptype=" & strreptype & "&pdcyesno=" & strpdctype & "','RepGL','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
            strpop = "window.open('TransactionReports.aspx?printId=GLLedger&reportsType=excel&actype=G&fromdate=" & strfromdate & "&todate=" & strtodate & "&fromname=" & TxtBankName.Text & "&fromcode=" & strfrmac & "&tocode=" & strtoac & "&divid=" & ViewState("divcode") & "&reptype=" & strreptype & "&pdcyesno=" & strpdctype & "','RepGL' );"

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptGeneralLedger.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

End Class
