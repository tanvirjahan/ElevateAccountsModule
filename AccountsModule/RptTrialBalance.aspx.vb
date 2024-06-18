Imports System.Data
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Data.SqlClient
Partial Class AccountsModule_RptTrialBalance
    Inherits System.Web.UI.Page
    Dim month, lastday As String
    Dim max As Integer

    Private Shared divcode As String

    Dim objUtils As New clsUtils
    Dim objDate As New clsDateTime
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim strappid As String = ""
    Dim strappname As String = ""
    Dim day As Date
    Dim objUser As New clsUser

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        Dim appidnew As String = CType(Request.QueryString("appid"), String)
        strappid = appidnew

        If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
            Response.Redirect("~/Login.aspx", False)
            Exit Sub
        Else
            objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                               CType(strappname, String), "AccountsModule\RptTrialBalance.aspx?appid=" + strappid, btnadd, Button1, btnloadreport, gv_SearchResult)
        End If


        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
        '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
        strappname = Session("AppName")
        '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        ViewState.Add("divcode", divid)
        divcode = ViewState("divcode")
        TxtBankCode.Attributes.Add("readonly", "readonly")
        If Page.IsPostBack = False Then
            If CType(Session("GlobalUserName"), String) = "" Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            End If
            Try
                month = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=521"), String)
                lastday = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=529"), String)
                max = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=507"), String)

                Dim k As ListItem
                For i As Integer = 1 To max
                    k = New ListItem(i, i)
                    ddlselect.Items.Add(k)
                Next
                ddlselect.SelectedValue = max
                '----------------------------- Default Dates
                txtFromDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                day = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                txttoDate.Text = Format(objDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")

                txtFromDate.Attributes.Add("onchange", "showclosing()")
                txttoDate.Attributes.Add("onchange", "showclosing()")

                ''    rptall.Checked = True

                ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlacccodefrom, "acctcode", "acctname", "select acctcode,acctname from acctgroup where  childid<>0 and  accttype<>2 order by acctcode", True)
                ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlaccnamefrom, "acctname", "acctcode", "select acctname,acctcode from acctgroup where  childid<>0  and  accttype<>2 order by acctname", True)
                ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlacccodeto, "acctcode", "acctname", "select acctcode,acctname from acctgroup where  childid<>0  and  accttype<>2 order by acctcode", True)
                ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlaccnameto, "acctname", "acctcode", "select acctname,acctcode from acctgroup where  childid<>0  and  accttype<>2  order by acctname", True)



                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format("U", CType(Request.QueryString("fromdate"), Date))
                End If
                If Request.QueryString("todate") <> "" Then

                    txttoDate.Text = Format("U", CType(Request.QueryString("todate"), Date))
                End If
                If Request.QueryString("closing") <> "" Then
                    ddlclosing.SelectedIndex = Request.QueryString("closing")
                End If

                If Request.QueryString("level") <> "" Then
                    ddlselect.SelectedIndex = CInt(Request.QueryString("level")) - 1
                End If


                If Request.QueryString("withmov") <> "" Then
                    ddlwithmovmt.SelectedIndex = Request.QueryString("withmov")
                End If

                If day.Month = month And day.Day = lastday Then
                    Label3.Style("display") = "block"
                    ddlclosing.Style("display") = "block"
                Else
                    Label3.Style("display") = "none"
                    ddlclosing.Style("display") = "none"
                End If

                'txtFromDate.Attributes.Add("OnChange", "return fromtext('" + month + "')")
                'txttoDate.Attributes.Add("OnChange", "return totext('" + month + "')")
                ' txtFromDate.Attributes.Add("OnChange", "javascript: fromtext('" + CType(month, String) + "', '" + CType(lastday, String) + "')")
                ' txttoDate.Attributes.Add("OnChange", "javascript: totext('" + CType(month, String) + "', '" + CType(lastday, String) + "')")

                ' ''rptall.Attributes.Add("onclick", "rbevent(this,'" & rptrange.ClientID & "','A','account')")
                ' ''rptrange.Attributes.Add("onclick", "rbevent(this,'" & rptall.ClientID & "','R','account')")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ' ''ddlacccodefrom.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ' ''ddlaccnamefrom.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ' ''ddlacccodeto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ' ''ddlaccnameto.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

                ClientScript.GetPostBackEventReference(Me, String.Empty)
                If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepTBWindowPostBack") Then
                    btnloadreport_Click(sender, e)
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("RptTrialBalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            month = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=521"), String)
            lastday = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=529"), String)
            If ddlwithmovmt.SelectedIndex = 1 Then
                day = Format(CType(txtFromDate.Text, Date), "dd/MM/yyyy")
                Label2.Text = "AsOnDate"
                Label1.Style("display") = "none"
                txttoDate.Style("display") = "none"
                ImgBtntoDt.Style("display") = "none"
            Else
                day = Format(CType(txttoDate.Text, Date), "dd/MM/yyyy")
                Label2.Text = "FromDate"
                Label1.Style("display") = ""
                txttoDate.Style("display") = ""
                ImgBtntoDt.Style("display") = ""
            End If
            If day.Month = month And day.Day = lastday Then
                Label3.Style("display") = "block"
                ddlclosing.Style("display") = "block"
            Else
                Label3.Style("display") = "none"
                ddlclosing.Style("display") = "none"
            End If
        End If

        checkrb_status1()

    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function Getbankslist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim acctnames As New List(Of String)
        Dim divid As String = ""
        Try

            strSqlQry = "select acctcode,acctname from acctgroup where  acctlevel=4   and div_code='" & divcode & "' and acctname like  '" & Trim(prefixText) & "%' order by acctcode"
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
    Public Sub checkrb_status1()
        ' ''If rptall.Checked = True Then
        ' ''    ddlacccodefrom.Disabled = True
        ' ''    ddlaccnamefrom.Disabled = True
        ' ''    ddlacccodeto.Disabled = True
        ' ''    ddlaccnameto.Disabled = True
        ' ''Else
        ' ''    ddlacccodefrom.Disabled = False
        ' ''    ddlaccnamefrom.Disabled = False
        ' ''    ddlacccodeto.Disabled = False
        ' ''    ddlaccnameto.Disabled = False
        ' ''End If
    End Sub
    Protected Sub txttoDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txttoDate.TextChanged
        If Label2.Text <> "AsOnDate" Then
            If day.Month = month And day.Day = lastday Then
                Label3.Style("display") = "block"
                ddlclosing.Style("display") = "block"
            Else
                Label3.Style("display") = "none"
                ddlclosing.Style("display") = "none"
            End If
        Else
            Exit Sub
        End If

    End Sub

    Protected Sub fromtext(ByVal sender As Object, ByVal e As System.EventArgs)
        If Label2.Text = "AsOnDate" Then
            If day.Month = month And day.Day = lastday Then
                Label3.Style("display") = "block"
                ddlclosing.Style("display") = "block"
            Else
                Label3.Style("display") = "none"
                ddlclosing.Style("display") = "none"
            End If
        Else
            Exit Sub
        End If

    End Sub


    Protected Sub btnloadreport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnloadreport.Click
        Try
            'If ValidatePage() = True Then
            If ValidatePage() = False Then
                Exit Sub
            End If

            'Session.Add("Pageame", "GLtrialbalReport")
            'Session.Add("BackPageName", "RptTrialBalance.aspx")

            Dim strfromdate, strtodate, strclosing, strlevel, strwithmovmt As String
            Dim stracccodefrom As String = ""
            Dim stracccodeto As String = ""
            Dim rptvalue As Integer
            Dim rptype As Integer = 0

            strclosing = ddlclosing.SelectedIndex.ToString


            strlevel = ddlselect.SelectedValue.ToString
            strwithmovmt = ddlwithmovmt.SelectedIndex.ToString
            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = IIf(strwithmovmt = 0, Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10), strfromdate)

            rptype = IIf(chkTree.Checked, 1, 0)

            ' ''stracccodefrom = IIf(UCase(ddlacccodefrom.Items(ddlacccodefrom.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlacccodefrom.Items(ddlacccodefrom.SelectedIndex).Text, "")
            ' ''stracccodeto = IIf(UCase(ddlacccodeto.Items(ddlacccodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlacccodeto.Items(ddlacccodeto.SelectedIndex).Text, "")
            ' ''rptvalue = IIf(rptall.Checked, 0, 1)



            'If chkTree.Checked = True Then

            '    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            '    sqlTrans = mySqlConn.BeginTransaction

            '    mySqlCmd = New SqlCommand
            '    mySqlCmd.Connection = mySqlConn
            '    mySqlCmd.Transaction = sqlTrans

            '    mySqlCmd.CommandType = CommandType.StoredProcedure
            '    mySqlCmd.CommandText = "sp_gltbtree"
            '    mySqlCmd.CommandTimeout = 0
            '    Dim parms As New List(Of SqlParameter)
            '    Dim parm(11) As SqlParameter
            '    parm(0) = New SqlParameter("@frmdate", CType(strfromdate, String))
            '    parm(1) = New SqlParameter("@todate", CType(strtodate, String))
            '    parm(2) = New SqlParameter("@movflg", CType(strwithmovmt, Integer))
            '    parm(3) = New SqlParameter("@frmdiv", "")
            '    parm(4) = New SqlParameter("@todiv", "")
            '    parm(5) = New SqlParameter("@ptype", "G")
            '    parm(6) = New SqlParameter("@frmac", CType(stracccodefrom, String))
            '    parm(7) = New SqlParameter("@toac", CType(stracccodeto, String))
            '    parm(8) = New SqlParameter("@tbord", 0)
            '    parm(9) = New SqlParameter("@aclevel", CType(strlevel, Integer))
            '    parm(10) = New SqlParameter("@closing", CType(strclosing, Integer))

            '    For i = 0 To 10
            '        mySqlCmd.Parameters.Add(parm(i))
            '    Next
            '    mySqlCmd.ExecuteNonQuery()

            'End If

            'Response.Redirect("rptgltrialbalReport.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&closing=" & strclosing & "&level=" & strlevel & "&withmov=" & strwithmovmt, False)
            Dim strpop As String = ""
            'strpop = "window.open('rptgltrialbalReport.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&closing=" & strclosing & "&level=" & strlevel & "&withmov=" & strwithmovmt & "&acccodefrom=" & stracccodefrom & "&acccodeto=" & stracccodeto & "&rptype=" & rptype & "&rptval=" & rptvalue & "','RepTB','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
            strpop = "window.open('rptgltrialbalReport.aspx?fromdate=" & strfromdate & "&todate=" & strtodate & "&closing=" & strclosing & "&level=" & strlevel & "&divid=" & ViewState("divcode") & "&withmov=" & strwithmovmt & "&acccodefrom=" & stracccodefrom & "&acccodeto=" & stracccodeto & "&rptype=" & rptype & "&rptval=" & rptvalue & "','RepTB' );"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptTrialBalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try


    End Sub

    Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReport.Click
        Try
            If ValidatePage() = False Then
                Exit Sub
            End If

            Dim strfromdate, strtodate, strclosing, strlevel, strwithmovmt As String
            Dim stracccodefrom As String = ""
            Dim stracccodeto As String = ""
            Dim rptvalue As Integer
            Dim rptype As Integer = 0

            strclosing = ddlclosing.SelectedIndex.ToString


            strlevel = ddlselect.SelectedValue.ToString
            strwithmovmt = ddlwithmovmt.SelectedIndex.ToString
            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = IIf(strwithmovmt = 0, Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10), strfromdate)

            rptype = IIf(chkTree.Checked, 1, 0)
            Dim strpop As String = ""
            strpop = "window.open('TransactionReports.aspx?printId=GlTrialBal&fromdate=" & strfromdate & "&todate=" & strtodate & "&closing=" & strclosing & "&level=" & strlevel & "&divid=" & ViewState("divcode") & "&withmov=" & strwithmovmt & "&acccodefrom=" & stracccodefrom & "&acccodeto=" & stracccodeto & "&rptype=" & rptype & "&rptval=" & rptvalue & "','RepTB' );"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptTrialBalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try


    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptTrialBalance','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Public Function ValidatePage() As Boolean
        'Dim frmdate, todate As Date
        Dim objDateTime As New clsDateTime
        Dim strfromcode, strtocode As Integer

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
            If ddlwithmovmt.SelectedIndex <> 1 Then
                If CType(objDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objDate.ConvertDateromTextBoxToDatabase(txttoDate.Text), Date) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                    SetFocus(txttoDate)
                    ValidatePage = False
                    Exit Function
                End If
            End If
            ''If rptrange.Checked = True Then
            ''    strfromcode = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select acctlevel from acctgroup where acctcode='" + ddlacccodefrom.Items(ddlacccodefrom.SelectedIndex).Text + "'"), String)
            ''    strtocode = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select acctlevel from acctgroup where acctcode='" + ddlacccodeto.Items(ddlacccodeto.SelectedIndex).Text + "'"), String)

            ''    If strfromcode <> strtocode Then
            ''        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Accountcode from and To level sould be same');", True)
            ''        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlacccodefrom.ClientID + "');", True)
            ''        SetFocus(ddlacccodefrom)
            ''        ValidatePage = False
            ''        Exit Function
            ''    End If
            ''End If


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

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim parms As New List(Of SqlParameter)
        Dim i As Integer
        Dim parm(8) As SqlParameter
        If ValidatePage() = False Then
            Exit Sub
        End If
        Dim strfromdate, strtodate, strclosing, strlevel, strwithmovmt As String
        Dim stracccodefrom As String = ""
        Dim stracccodeto As String = ""
        Dim rptvalue As Integer
        'Dim DA As SqlDataAdapter
        'Dim con As SqlConnection

        strclosing = ddlclosing.SelectedIndex.ToString


        strlevel = ddlselect.SelectedValue.ToString
        strwithmovmt = ddlwithmovmt.SelectedIndex.ToString
        strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
        strtodate = IIf(strwithmovmt = 0, Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10), strfromdate)



        'stracccodefrom = IIf(UCase(ddlacccodefrom.Items(ddlacccodefrom.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlacccodefrom.Items(ddlacccodefrom.SelectedIndex).Text, "")
        'stracccodeto = IIf(UCase(ddlacccodeto.Items(ddlacccodeto.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlacccodeto.Items(ddlacccodeto.SelectedIndex).Text, "")
        'rptvalue = IIf(rptall.Checked, 0, 1)

        strclosing = ddlclosing.SelectedIndex.ToString

        parm(0) = New SqlParameter("@frmdate", strfromdate)
        parm(1) = New SqlParameter("@todate", strtodate)
        parm(2) = New SqlParameter("@div_code", divcode)
        parm(3) = New SqlParameter("@closing", strclosing)
        parm(4) = New SqlParameter("@rptval", rptvalue)
        parm(5) = New SqlParameter("@acccodefrom", stracccodefrom)
        parm(6) = New SqlParameter("@acccodeto", stracccodeto)
        parm(7) = New SqlParameter("@rpttype", strwithmovmt)
        parm(8) = New SqlParameter("@level", strlevel)

        For i = 0 To 8
            parms.Add(parm(i))
        Next

        Dim ds As New DataSet
        'ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"),"sp_get_request_search", parms)
        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_rep_trialbalance_xls", parms)
        If ds.Tables.Count > 0 Then

            If ds.Tables(0).Rows.Count > 0 Then
                objUtils.ExportToExcel(ds, Response)
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Record found' );", True)
            End If

        End If
    End Sub

    Protected Sub btnclose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclose.Click
        month = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=521"), String)
        lastday = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=529"), String)
        If ddlwithmovmt.SelectedIndex = 1 Then
            day = Format(CType(txtFromDate.Text, Date), "dd/MM/yyyy")
            Label2.Text = "AsOnDate"
            Label1.Style("display") = "none"
            txttoDate.Style("display") = "none"
            ImgBtntoDt.Style("display") = "none"
        Else
            day = Format(CType(txttoDate.Text, Date), "dd/MM/yyyy")
            Label2.Text = "FromDate"
            Label1.Style("display") = ""
            txttoDate.Style("display") = ""
            ImgBtntoDt.Style("display") = ""
        End If
        If day.Month = month And day.Day = lastday Then
            Label3.Style("display") = "block"
            ddlclosing.Style("display") = "block"
        Else
            Label3.Style("display") = "none"
            ddlclosing.Style("display") = "none"
        End If

    End Sub

    Protected Sub ddlwithmovmt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlwithmovmt.SelectedIndexChanged
        If ddlwithmovmt.SelectedIndex = 1 Then
            txttoDate.Text = txtFromDate.Text

        End If
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
    Protected Sub btnExcelReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelReport.Click
        Try
            If ValidatePage() = False Then
                Exit Sub
            End If

            Dim strfromdate, strtodate, strclosing, strlevel, strwithmovmt As String
            Dim stracccodefrom As String = ""
            Dim stracccodeto As String = ""
            Dim rptvalue As Integer
            Dim rptype As Integer = 0

            strclosing = ddlclosing.SelectedIndex.ToString


            strlevel = ddlselect.SelectedValue.ToString
            strwithmovmt = ddlwithmovmt.SelectedIndex.ToString
            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = IIf(strwithmovmt = 0, Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10), strfromdate)

            rptype = IIf(chkTree.Checked, 1, 0)
            Dim strpop As String = ""
            strpop = "window.open('TransactionReports.aspx?printId=GlTrialBal&reportsType=excel&fromdate=" & strfromdate & "&todate=" & strtodate & "&closing=" & strclosing & "&level=" & strlevel & "&divid=" & ViewState("divcode") & "&withmov=" & strwithmovmt & "&fromname=" & TxtBankName.Text & "&acccodefrom=" & stracccodefrom & "&acccodeto=" & stracccodeto & "&rptype=" & rptype & "&rptval=" & rptvalue & "','RepTB' );"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptTrialBalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try


    End Sub

End Class
