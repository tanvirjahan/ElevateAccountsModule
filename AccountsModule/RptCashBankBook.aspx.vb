'------------================--------------=======================------------------================
'   Module Name    :    RptCashBankBook.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    29 SEP 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Collections.Generic
#End Region


Partial Class RptCashBankBook
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
    Private Shared divcode As String
    Dim strappname As String = ""
#End Region

    <System.Web.Script.Services.ScriptMethod()> _
             <System.Web.Services.WebMethod()> _
    Public Shared Function Getbankslist(ByVal prefixText As String, ByVal contextKey As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim banknames As New List(Of String)
        Dim divid As String = ""
        Try

            If contextKey = "C" Then
                strSqlQry = "select acctcode,acctname from acctmast where bankyn='Y' and div_code='" & divcode & "'   and  acctname like  '" & Trim(prefixText) & "%' and   bank_master_type_code  =2 order by acctcode"
            ElseIf contextKey = "B" Then
                strSqlQry = "select acctcode,acctname from acctmast where bankyn='Y' and div_code='" & divcode & "'   and  acctname like  '" & Trim(prefixText) & "%' and  bank_master_type_code  =1  order by acctcode"
                'Else
                '    strSqlQry = "select acctcode,acctname from acctmast where bankyn='Y' and div_code='" & divcode & "'   and  acctname like  '" & Trim(prefixText) & "%'   order by acctcode"
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
                    banknames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString()))

                Next
            End If
            Return banknames
        Catch ex As Exception
            Return banknames
        End Try

    End Function
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Page.IsPostBack = False Then
                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim appidnew As String = CType(Request.QueryString("appid"), String)
                strappid = appidnew
                If appidnew Is Nothing = False Then
                    '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>            
                    strappname = Session("AppName")
                    '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                End If
                '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
                '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                ViewState.Add("divcode", divid)
                divcode = ViewState("divcode")

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                Else
                    objUser.CheckUserRight(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), _
                                                       CType(strappname, String), "AccountsModule\RptCashBankBook.aspx?appid=" + strappid, btnadd, Button1, btnLoadReprt, gv_SearchResult)
                End If

                ddlCashBankType.SelectedIndex = 0

                ' ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccCD, "acctcode", "acctname", "select acctcode,acctname from acctmast where bankyn='Y' and div_code='" & ViewState("divcode") & "'   order by acctcode", True)
                ' ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccNM, "acctname", "acctcode", "select acctcode,acctname from acctmast where bankyn='Y' and div_code='" & ViewState("divcode") & "'  order by acctname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcurrency, "currcode", "currcode", "select 'Bank Currency' as currcode union select option_selected  from reservation_parameters where param_id=457 ", True)
                btnLoadReprt.Attributes.Add("onclick", "return FormValidation()")
                txtFromDate.Text = Format(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                txtToDate.Text = Format(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), "dd/MM/yyyy")
                'txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")

                If Request.QueryString("fromdate") <> "" Then
                    txtFromDate.Text = Format("U", CType(Request.QueryString("fromdate"), Date)) 'Format(Request.QueryString("fromdate"), "dd/MM/yyyy")
                End If

                If Request.QueryString("todate") <> "" Then
                    txtToDate.Text = Format("U", CType(Request.QueryString("todate"), Date)) 'Format(Request.QueryString("todate"), "dd/MM/yyyy")
                End If

                If Request.QueryString("frmcode") <> "" Then
                    txtbankcode.Text = Request.QueryString("frmcode") ' ''ddlAccNM.Value = Request.QueryString("frmcode")
                    ' ''ddlAccCD.Value = ddlAccNM.Items(ddlAccNM.SelectedIndex).Text
                End If

                If Request.QueryString("Type") <> "" Then
                    ddlType.SelectedIndex = Request.QueryString("Type ")
                End If


                If Request.QueryString("currflg") <> "" Then
                    ddlcurrency.SelectedIndex = Request.QueryString("currflg")
                Else

                    ddlcurrency.SelectedIndex = 1
                End If


                Dim typ As Type
                typ = GetType(DropDownList)
                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ' ''ddlAccCD.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ' ''ddlAccNM.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                ' ''SetFocus(ddlAccCD)
            End If
            ClientScript.GetPostBackEventReference(Me, String.Empty)
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RepCBbookWindowPostBack") Then
                btnLoadReprt_Click(sender, e)
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("RptCashBankBook.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub btnLoadReprt_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'If ValidatePage() = True Then
            If ValidatePage() = False Then
                Exit Sub
            End If
            'Session.Add("Pageame", "RptCashBankBook")
            'Session.Add("BackPageName", "RptCashBankBook.aspx")

            Dim strfromdate, strfromname, strtodate, strcode, strcurrflg, strtype As String
            strcode = IIf(txtbankcode.Text <> "", txtbankcode.Text, "") 'IIf(UCase(ddlAccCD.Items(ddlAccCD.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlAccCD.Items(ddlAccCD.SelectedIndex).Text, "")
            strfromname = IIf(txtbankcode.Text <> "", txtbankcode.Text, "") ' IIf(UCase(ddlAccNM.Items(ddlAccNM.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlAccNM.Items(ddlAccNM.SelectedIndex).Text, "")

            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strcurrflg = IIf(ddlcurrency.SelectedIndex = 1, 0, 1)
            strtype = ddlType.SelectedIndex

            'Response.Redirect("rptgltrialbalReport.aspx?pagetype=C&fromname=" & strfromname & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&frmcode=" & strcode & "&type=" & strtype & "&currflg=" & strcurrflg, False)

            Dim strpop As String = ""
            'strpop = "window.open('rptgltrialbalReport.aspx?pagetype=C&fromname=" & strfromname & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&frmcode=" & strcode & "&type=" & strtype & "&currflg=" & strcurrflg & "','RepCBbook','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
            'strpop = "window.open('rptgltrialbalReport.aspx?pagetype=C&fromdate=" & strfromdate & "&todate=" & strtodate & "&frmcode=" & strcode & "&type=" & strtype & "&currflg=" & strcurrflg & "','RepCBbook','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
            strpop = "window.open('rptgltrialbalReport.aspx?pagetype=C&fromdate=" & strfromdate & "&todate=" & strtodate & "&frmcode=" & strcode & "&type=" & strtype & "&divid=" & ViewState("divcode") & "&currflg=" & strcurrflg & "','RepCBbook');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptCashBankBook.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

    Protected Sub btnPdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'If ValidatePage() = True Then
            If ValidatePage() = False Then
                Exit Sub
            End If
            'Session.Add("Pageame", "RptCashBankBook")
            'Session.Add("BackPageName", "RptCashBankBook.aspx")

            Dim strfromdate, strfromname, strtodate, strcode, strcurrflg, strtype As String
            strcode = IIf(txtbankcode.Text <> "", txtbankcode.Text, "") 'IIf(UCase(ddlAccCD.Items(ddlAccCD.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlAccCD.Items(ddlAccCD.SelectedIndex).Text, "")
            strfromname = IIf(txtbankname.Text <> "", txtbankname.Text, "") ' IIf(UCase(ddlAccNM.Items(ddlAccNM.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlAccNM.Items(ddlAccNM.SelectedIndex).Text, "")

            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strcurrflg = IIf(ddlcurrency.SelectedIndex = 1, 0, 1)
            strtype = ddlType.SelectedIndex
            Dim inclpagebr As Integer
            If chkinclpagebr.Checked = True Then
                inclpagebr = 1
            Else
                inclpagebr = 0
            End If
            'Response.Redirect("rptgltrialbalReport.aspx?pagetype=C&fromname=" & strfromname & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&frmcode=" & strcode & "&type=" & strtype & "&currflg=" & strcurrflg, False)

            Dim strpop As String = ""
            'strpop = "window.open('rptgltrialbalReport.aspx?pagetype=C&fromname=" & strfromname & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&frmcode=" & strcode & "&type=" & strtype & "&currflg=" & strcurrflg & "','RepCBbook','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
            Dim cashbanktype As Integer
            If ddlCashBankType.SelectedIndex = 0 Then
                cashbanktype = 1
            ElseIf ddlCashBankType.SelectedIndex = 1 Then
                cashbanktype = 2
            End If
            'strpop = "window.open('rptgltrialbalReport.aspx?pagetype=C&fromdate=" & strfromdate & "&todate=" & strtodate & "&frmcode=" & strcode & "&type=" & strtype & "&currflg=" & strcurrflg & "','RepCBbook','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
            strpop = "window.open('TransactionReports.aspx?pagetype=C&printId=CashBankBook&fromdate=" & strfromdate & "&todate=" & strtodate & "&frmcode=" & strcode & "&frmname=" & strfromname & "&type=" & strtype & "&divid=" & ViewState("divcode") & "&inclpagebr=" & inclpagebr & "&currflg=" & strcurrflg & "&cashbanktype= " & cashbanktype & "','RepCBbook');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptCashBankBook.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptCashBankBook','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
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


            ''If txtbankcode.Text = "" Then 'ddlAccCD.Value = "[Select]" Then
            ''    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please choose  cash/Bank  code.');", True)
            ''    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + ddlAccCD.ClientID + "');", True)

            ''    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Code field can not be blank.');", True)
            ''    SetFocus(txtbankcode.Text)
            ''    'SetFocus(txtToDate)
            ''    ValidatePage = False
            ''    Exit Function
            ''End If


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
    Protected Sub btnExcelReport_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'If ValidatePage() = True Then
            If ValidatePage() = False Then
                Exit Sub
            End If
            'Session.Add("Pageame", "RptCashBankBook")
            'Session.Add("BackPageName", "RptCashBankBook.aspx")

            Dim strfromdate, strfromname, strtodate, strcode, strcurrflg, strtype As String
            strcode = IIf(txtbankcode.Text <> "", txtbankcode.Text, "") 'IIf(UCase(ddlAccCD.Items(ddlAccCD.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlAccCD.Items(ddlAccCD.SelectedIndex).Text, "")
            strfromname = IIf(txtbankcode.Text <> "", txtbankcode.Text, "") ' IIf(UCase(ddlAccNM.Items(ddlAccNM.SelectedIndex).Text) <> Trim(UCase("[Select]")), ddlAccNM.Items(ddlAccNM.SelectedIndex).Text, "")

            strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strtodate = Mid(Format(CType(txtToDate.Text, Date), "yyyy/MM/dd"), 1, 10)
            strcurrflg = IIf(ddlcurrency.SelectedIndex = 1, 0, 1)
            strtype = ddlType.SelectedIndex

            'Response.Redirect("rptgltrialbalReport.aspx?pagetype=C&fromname=" & strfromname & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&frmcode=" & strcode & "&type=" & strtype & "&currflg=" & strcurrflg, False)


            'strpop = "window.open('rptgltrialbalReport.aspx?pagetype=C&fromname=" & strfromname & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&frmcode=" & strcode & "&type=" & strtype & "&currflg=" & strcurrflg & "','RepCBbook','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
            Dim cashbanktype As Integer
            If ddlCashBankType.SelectedIndex = 0 Then
                cashbanktype = 1
            ElseIf ddlCashBankType.SelectedIndex = 1 Then
                cashbanktype = 2
            End If
            Dim strpop As String = ""
            Dim inclpagebr As Integer
            If chkinclpagebr.Checked = True Then
                inclpagebr = 1
            Else
                inclpagebr = 0
            End If
            'strpop = "window.open('rptgltrialbalReport.aspx?pagetype=C&fromname=" & strfromname & "&fromdate=" & strfromdate & "&todate=" & strtodate & "&frmcode=" & strcode & "&type=" & strtype & "&currflg=" & strcurrflg & "','RepCBbook','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
            'strpop = "window.open('rptgltrialbalReport.aspx?pagetype=C&fromdate=" & strfromdate & "&todate=" & strtodate & "&frmcode=" & strcode & "&type=" & strtype & "&currflg=" & strcurrflg & "','RepCBbook','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes' );"
            strpop = "window.open('TransactionReports.aspx?pagetype=C&printId=CashBankBook&reportsType=excel&fromdate=" & strfromdate & "&todate=" & strtodate & "&frmcode=" & strcode & "&frmname=" & strfromname & "&type=" & strtype & "&divid=" & ViewState("divcode") & "&inclpagebr=" & inclpagebr & "&currflg=" & strcurrflg & "&cashbanktype= " & cashbanktype & "','RepCBbook');"


            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            'End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RptCashBankBook.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub
End Class
