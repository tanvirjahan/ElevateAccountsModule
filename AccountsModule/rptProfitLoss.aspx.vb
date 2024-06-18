Imports System.Data
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Globalization
Partial Class AccountsModule_rptProfitLoss
    Inherits System.Web.UI.Page
    Dim objUtils As New clsUtils
    Dim ObjDate As New clsDateTime
    Dim month, lastday As String
    Dim max As Integer
    Dim day As Date
    Dim strappid As String = ""
    Dim strappname As String = ""
    Private Shared divcode As String = ""
    Dim objUser As New clsUser
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        Dim appidnew As String = CType(Request.QueryString("appid"), String)
        Dim plType As String = CType(Request.QueryString("plType"), String)
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
            If String.IsNullOrEmpty(plType) Then plType = ""
            If plType.ToUpper = "PLRatio".ToUpper Then
                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                               CType(strappname, String), "AccountsModule\RptProfitLoss.aspx?appid=" + strappid + "&plType=" + plType, btnadd, btnExcelReprt, btnPdfReprt, gv_SearchResult)
                btnPdfReprt.Visible = False
                ChkRatio.Checked = True
                chkmonth.Checked = True
                chkmonth.Attributes.Add("style", "display:none")
                lblMonth.Attributes.Add("style", "display:none")
                spanHeading.InnerHtml = "Income Statement With Ratio"
            Else
                objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                               CType(strappname, String), "AccountsModule\RptProfitLoss.aspx?appid=" + strappid, btnadd, Button1, btnLoadReprt, gv_SearchResult)
                btnPdfReprt.Visible = True
                ChkRatio.Checked = False
                spanHeading.InnerHtml = "Income Statement"
            End If

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

            Dim max As Integer

            max = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=507"), String)


            Dim k As ListItem
            For i As Integer = 1 To max
                k = New ListItem(i, i)
                ddlselect.Items.Add(k)
            Next
            ddlselect.SelectedValue = max
            txtFromDate.Text = Format(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
            txttoDate.Text = Format(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
            '   txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")

            txttoDate.Attributes.Add("onchange", "showclosing()")

            If Request.QueryString("frmdate") <> "" Then

                txtFromDate.Text = Format("U", CType(Request.QueryString("frmdate"), Date)) 'Format(CDate(Request.QueryString("frmdate")), "dd/MM/yyyy")
            End If
            If Request.QueryString("todate") <> "" Then

                txttoDate.Text = Format("U", CType(Request.QueryString("todate"), Date)) 'Format(CDate(Request.QueryString("todate")), "dd/MM/yyyy")
            End If

            If day.Month = month And day.Day = lastday Then
                Label3.Style("display") = "block"
                ddlclosing.Style("display") = "block"
            Else
                Label3.Style("display") = "none"
                ddlclosing.Style("display") = "none"
            End If

            ddlselect.Visible = False
            Button1.Visible = False
            ddlrpttype.Visible = False
            btnHelp.Visible = False
            lblreporttype.Visible = False
            lbllevel.Visible = False
            If Request.QueryString("level") <> "" Then
                ddlselect.SelectedValue = Trim(Request.QueryString("level"))
            End If

        End If

        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepPLWindowPostBack") Then
            btnLoadReprt_Click(sender, e)
        End If
    End Sub

    'Protected Function val() As Boolean
    '    If CDate(txtFromDate.Text).Year <> CDate(ObjDate.GetSystemDateOnly(Session("dbconnectionName"))).Year Then
    '         ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter From Date in the current financial year');", True)

    '        Return False
    '    End If

    '    If CDate(txtFromDate.Text) > CDate(txttoDate.Text) Then
    '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To Date should be greater than from Date');", True)
    '        Return False
    '    End If
    '    Return True

    'End Function


    Protected Sub btnLoadReprt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadReprt.Click
        If ValidatePage() = False Then
            Exit Sub
        End If
        Dim strclosing As String
        Dim strrpttype As Integer = 0
        Dim rptype As Integer = 0
        strrpttype = ddlrpttype.SelectedIndex
        strclosing = ddlclosing.SelectedIndex.ToString
        rptype = IIf(chkmonth.Checked, 1, 0)

        'Response.Redirect("rptRV_PVreport.aspx?frmdate=" & txtFromDate.Text & "&todate=" & txttoDate.Text & "&type=Profit&level=" & ddlselect.SelectedValue, False)
        Dim strpop As String = ""
        'strpop = "window.open('rptRV_PVreport.aspx?frmdate=" & txtFromDate.Text & "&todate=" & txttoDate.Text & "&type=Profit&level=" & ddlselect.SelectedValue & "&strrpttype1=" & strrpttype & "&rptype=" & rptype & "&closing=" & strclosing & "','RepPL','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
        strpop = "window.open('rptRV_PVreport.aspx?frmdate=" & txtFromDate.Text & "&todate=" & txttoDate.Text & "&type=Profit&level=" & ddlselect.SelectedValue & "&divid=" & divcode & "&strrpttype1=" & strrpttype & "&rptype=" & rptype & "&closing=" & strclosing & "','RepPL');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub


    Protected Sub btnPdfReprt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdfReprt.Click
        If ValidatePage() = False Then
            Exit Sub
        End If
        Dim strclosing As String
        Dim strrpttype As Integer = 0
        Dim rptype As Integer = 0
        strrpttype = ddlrpttype.SelectedIndex
        strclosing = ddlclosing.SelectedIndex.ToString
        rptype = IIf(chkmonth.Checked, 1, 0)

        'Response.Redirect("rptRV_PVreport.aspx?frmdate=" & txtFromDate.Text & "&todate=" & txttoDate.Text & "&type=Profit&level=" & ddlselect.SelectedValue, False)
        Dim strpop As String = ""
        'strpop = "window.open('rptRV_PVreport.aspx?frmdate=" & txtFromDate.Text & "&todate=" & txttoDate.Text & "&type=Profit&level=" & ddlselect.SelectedValue & "&strrpttype1=" & strrpttype & "&rptype=" & rptype & "&closing=" & strclosing & "','RepPL','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
        strpop = "window.open('TransactionReports.aspx?printid=ProfitLoss&reportsType=pdf&frmdate=" & txtFromDate.Text & "&todate=" & txttoDate.Text & "&type=Profit&level=" & ddlselect.SelectedValue & "&divid=" & divcode & "&strrpttype1=" & strrpttype & "&rptype=" & rptype & "&closing=" & strclosing & "','RepPL');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptProfitandLoss','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
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
            If CType(objDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(objDate.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
                SetFocus(txttoDate)
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

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(3) As SqlParameter

            If ValidatePage() = False Then
                Exit Sub
            End If

            Dim strrpttype As Integer = 0
            Dim strlevel, strrptvalue As Integer
            Dim strfromdate, strtodate As String

            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = Mid(Format(CType(txttoDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strrpttype = ddlrpttype.SelectedIndex
            strlevel = ddlselect.SelectedValue
            strrptvalue = ddlrpttype.SelectedIndex

            parm(0) = New SqlParameter("@fromdate", strfromdate)
            parm(1) = New SqlParameter("@todate", strtodate)
            parm(2) = New SqlParameter("@div_code", divcode)
            parm(3) = New SqlParameter("@level", strlevel)

            For i = 0 To 3
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            If strrptvalue = 0 Then
                ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_rep_profitloss_xls", parms)
            Else
                ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_rep_profitlossratio_xls", parms)
            End If

            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    objUtils.ExportToExcel(ds, Response)
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Record found' );", True)
            End If

        Catch ex As Exception
        End Try

    End Sub

    Protected Sub btnclose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclose.Click
        month = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=521"), String)
        lastday = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected From reservation_parameters where param_id=529"), String)
        day = Format(CType(txttoDate.Text, Date), "dd/MM/yyyy")
        If day.Month = month And day.Day = lastday Then
            Label3.Style("display") = "block"
            ddlclosing.Style("display") = "block"
        Else
            Label3.Style("display") = "none"
            ddlclosing.Style("display") = "none"
        End If
    End Sub

    Protected Sub btnExcelReprt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelReprt.Click
        If ValidatePage() = False Then
            Exit Sub
        End If
        Dim strclosing As String
        Dim strrpttype As Integer = 0
        Dim strratio As Integer = 0
        Dim rptype As Integer = 0
        Dim rptratio As Integer = 0
        strrpttype = ddlrpttype.SelectedIndex
        strclosing = ddlclosing.SelectedIndex.ToString
        rptype = IIf(chkmonth.Checked, 1, 0)
        rptratio = IIf(ChkRatio.Checked, 1, 0)

        'Response.Redirect("rptRV_PVreport.aspx?frmdate=" & txtFromDate.Text & "&todate=" & txttoDate.Text & "&type=Profit&level=" & ddlselect.SelectedValue, False)
        Dim strpop As String = ""
        'strpop = "window.open('rptRV_PVreport.aspx?frmdate=" & txtFromDate.Text & "&todate=" & txttoDate.Text & "&type=Profit&level=" & ddlselect.SelectedValue & "&strrpttype1=" & strrpttype & "&rptype=" & rptype & "&closing=" & strclosing & "','RepPL','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
        strpop = "window.open('TransactionReports.aspx?printid=ProfitLoss&reportsType=excel&frmdate=" & txtFromDate.Text & "&todate=" & txttoDate.Text & "&type=Profit&level=" & ddlselect.SelectedValue & "&divid=" & divcode & "&strrpttype1=" & strrpttype & "&rptype=" & rptype & "&closing=" & strclosing & "&chkRatio=" & rptratio & "','RepPL');"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub
     
End Class
