'------------================--------------=======================------------------================
'   Module Name    :    RptReceiptsRegister.aspx
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


Partial Class RptReceiptsRegister
    Inherits System.Web.UI.Page

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim strSqlQry As String
    Dim strWhereCond As String
    Private Shared divcode As String
    Dim SqlConn As SqlConnection
    Dim SqlCmd As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim dpFDate As EclipseWebSolutions.DatePicker.DatePicker
    Dim dpTDate As EclipseWebSolutions.DatePicker.DatePicker
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim ObjDate As New clsDateTime
    Dim objDateTime As New clsDateTime
    Dim sqlTrans As SqlTransaction
    Dim strappid As String = ""
    Dim strappname As String = ""
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ViewState.Add("RptReceiptsRegisterRVPVTranType", Request.QueryString("tran_type"))

        Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
        Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
        Dim appidnew As String = CType(Request.QueryString("appid"), String)
        strappid = appidnew
        Dim tran_type As String = CType(Request.QueryString("tran_type"), String)

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
                                               CType(strappname, String), "AccountsModule\RptReceiptsRegister.aspx?tran_type=" + tran_type + "&appid=" + strappid, btnadd, Button1, btnLoadReprt, gv_SearchResult)
        End If

        '*** Danny 04/04/2018>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select division_master_code from division_master where accountsmodulename='" & Session("DAppName") & "'")
        '*** Danny 04/04/2018<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        ViewState.Add("divcode", divid)
        divcode = ViewState("divcode")
        txtbankcode.Attributes.Add("readonly", "readonly")
        TxtcontaccCode.Attributes.Add("readonly", "readonly")
        Try
            If Page.IsPostBack = False Then

                txtconnection.Value = Session("dbconnectionName")

                If CType(ViewState("RptReceiptsRegisterRVPVTranType"), String) = "RV" Then
                    lblHeading.Text = "Report Receipt Register"
                ElseIf CType(ViewState("RptReceiptsRegisterRVPVTranType"), String) = "PV" Then
                    lblHeading.Text = "Report Payment Register"
                    Label12.Visible = True
                    DDLchoice.Visible = True
                End If


                txtFromDate.Text = ObjDate.GetSystemDateTime(Session("dbconnectionName")).Day & "/" & ObjDate.GetSystemDateTime(Session("dbconnectionName")).Month & "/" & ObjDate.GetSystemDateTime(Session("dbconnectionName")).Year
                txtTodate.Text = ObjDate.GetSystemDateTime(Session("dbconnectionName")).Day & "/" & ObjDate.GetSystemDateTime(Session("dbconnectionName")).Month & "/" & ObjDate.GetSystemDateTime(Session("dbconnectionName")).Year


                txtFromDate.Text = ""
                txtTodate.Text = ""
                txtFromRecvAmt.Text = ""
                txtToRecvAmt.Text = ""
                If txtFromDate.Text = "" Then
                    ' txtFromDate.Text = Format(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"),"select fdate from toursmaster"), Date), "dd/MM/yyy")
                    txtFromDate.Text = Format(CType(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyy")
                End If
                If txtTodate.Text = "" Then
                    txtTodate.Text = DateAdd(DateInterval.Month, 1, objDateTime.GetSystemDateOnly(Session("dbconnectionName")))
                End If
                ' ''strSqlQry = "select acctcode,acctname from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
                ' ''" and  bankyn='Y' order by acctcode "
                ' ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccCode, "acctcode", "acctname", strSqlQry, True)

                ' ''strSqlQry = "select acctname, acctcode from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
                ''" and  bankyn='Y'  order by acctname"
                ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccName, "acctname", "acctcode", strSqlQry, True)


                ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlBankName, "other_bank_master_des", "other_bank_master_code", "select other_bank_master_des,other_bank_master_code from customer_bank_master where active=1 ", True)
                ''objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlBankCode, "other_bank_master_code", "other_bank_master_des", "select other_bank_master_des,other_bank_master_code from customer_bank_master where active=1 ", True)

                ''ddlType.Attributes.Add("onchange", "javascript:FillBankCashDet('" + CType(ddlType.ClientID, String) + "','" + CType(ddlAccCode.ClientID, String) + "','" + CType(ddlAccName.ClientID, String) + "')")
                ''ddlAccCode.Attributes.Add("onchange", "javascript:FillCode('" + CType(ddlAccCode.ClientID, String) + "','" + CType(ddlAccName.ClientID, String) + "')")

                ''ddlAccCode.Attributes.Add("onchange", "javascript:FillCode('" + CType(ddlAccCode.ClientID, String) + "','" + CType(ddlAccName.ClientID, String) + "')")
                ''ddlAccName.Attributes.Add("onchange", "javascript:FillName('" + CType(ddlAccCode.ClientID, String) + "','" + CType(ddlAccName.ClientID, String) + "')")

                ''ddlBankCode.Attributes.Add("onchange", "javascript:FillCode('" + CType(ddlBankCode.ClientID, String) + "','" + CType(ddlBankName.ClientID, String) + "')")
                ''ddlBankName.Attributes.Add("onchange", "javascript:FillName('" + CType(ddlBankCode.ClientID, String) + "','" + CType(ddlBankName.ClientID, String) + "')")
                'txtFromDate.Attributes.Add("onchange", "javascript:ChangeDate();")
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ' ''ddlAccCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ' ''ddlAccName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ' ''ddlBankName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ' ''ddlBankCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlStatus.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("RptReceiptsRegister.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        ClientScript.GetPostBackEventReference(Me, String.Empty)
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "RptReceiptsRegisterWindowPostBack") Then

        End If

    End Sub
#End Region
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If CType(ViewState("RptReceiptsRegisterRVPVTranType"), String) = "RV" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptReceiptsRegister','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        ElseIf CType(ViewState("RptReceiptsRegisterRVPVTranType"), String) = "PV" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RptPaymentsRegister','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        End If
    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
           <System.Web.Services.WebMethod()> _
    Public Shared Function Getcontacclist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim acctnames As New List(Of String)
        Dim divid As String = ""
        Try


            strSqlQry = "select acctcode,acctname from acctmast ,bank_master_type where acctmast.div_code='" & divcode & "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
                " and  bankyn='Y'  and acctname like  '" & Trim(prefixText) & "%' order by acctcode"
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
    Public Shared Function Getbankslist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim banknames As New List(Of String)
        Dim divid As String = ""
        Try


            strSqlQry = "select other_bank_master_des,other_bank_master_code from customer_bank_master where active=1 and  other_bank_master_des like  '" & Trim(prefixText) & "%' order by other_bank_master_code"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    banknames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("other_bank_master_des").ToString(), myDS.Tables(0).Rows(i)("other_bank_master_code").ToString()))

                Next
            End If
            Return banknames
        Catch ex As Exception
            Return banknames
        End Try

    End Function
    Protected Function validaterpt() As Boolean
        Dim frmdate As DateTime
        Dim MyCultureInfo As New CultureInfo("fr-Fr")
        Dim ds As DataSet

        If txtFromDate.Text = "" Or txtTodate.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Fill the From and To date');", True)
            Return False
        End If

        If CType(ObjDate.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date) > CType(ObjDate.ConvertDateromTextBoxToDatabase(txtTodate.Text), Date) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should not less than from date.');", True)
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + txtToDate.ClientID + "');", True)
            SetFocus(txtTodate)

            Return False
        End If








        frmdate = DateTime.Parse(txtFromDate.Text, MyCultureInfo, DateTimeStyles.None)
        ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select option_selected  from reservation_parameters where param_id=1103")
        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                If IsDBNull(ds.Tables(0).Rows(0)(0)) = False Then
                    If frmdate < ds.Tables(0).Rows(0)("option_selected") Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From date cannot enter below the " & ds.Tables(0).Rows(0)("option_selected") & " ' );", True)
                        validaterpt = False
                        Exit Function
                    End If
                End If
            End If
        End If

        Return True
    End Function
    Protected Sub btnLoadReprt_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Pageame As String = ""
        If validaterpt() Then
            If CType(ViewState("RptReceiptsRegisterRVPVTranType"), String) = "RV" Then
                ViewState.Add("Pageame", "RecieptsReport")
                Pageame = "ReceiptsReport"
            ElseIf CType(ViewState("RptReceiptsRegisterRVPVTranType"), String) = "PV" Then
                ViewState.Add("Pageame", "PaymentsReport")
                Pageame = "PaymentsReport"
            End If

            ViewState.Add("Pageame", "GLtrialbalReport")
            ViewState.Add("BackPageName", "rptRV_PVreport.aspx")
            Try


                Dim strfromdate, strreporttype, strtodate, strtrantype, strtranid, strbanktype, strbankcode, strbank, poststate As String
                'strclosing = ddlclosing.SelectedIndex.ToString

                strfromdate = Mid(Format(CType(txtFromDate.Text, Date), "yyyy/MM/dd"), 1, 10)
                strtodate = Mid(Format(CType(txtTodate.Text, Date), "yyyy/MM/dd"), 1, 10)
                strreporttype = ddlrpttype.SelectedIndex()

                strtrantype = CType(ViewState("RptReceiptsRegisterRVPVTranType"), String)

                If CType(ViewState("RptReceiptsRegisterRVPVTranType"), String) = "PV" Then
                    Select Case DDLchoice.Value
                        Case 0
                            strtrantype = "CPV"
                        Case 1
                            strtrantype = "BPV"
                        Case 2
                            strtrantype = "DEP"
                    End Select
                End If

                strtranid = txtTranId.Text
                strbanktype = IIf(ddlType.Value = "[Select]", "", ddlType.Value)
                strbankcode = IIf(TxtcontaccCode.Text <> "", TxtcontaccCode.Text.Trim, "") '  IIf(ddlAccCode.Items(ddlAccCode.SelectedIndex).Text = "[Select]", "", ddlAccCode.Items(ddlAccCode.SelectedIndex).Text)
                strbank = IIf(txtbankcode.Text <> "", txtbankcode.Text.Trim, "") ' IIf(ddlBankCode.Items(ddlBankCode.SelectedIndex).Text = "[Select]", "", ddlBankCode.Items(ddlBankCode.SelectedIndex).Text)
                poststate = IIf(ddlStatus.Value = "[Select]", "", ddlStatus.Value)
                'Response.Redirect("rptRV_PVreport.aspx?frmdate=" & strfromdate & "&todate=" & strtodate & "&trantype=" & strtrantype & "&tranid=" & strtranid & "&C_Btype=" & strbanktype & "&accfrm=" & strbankcode & "&bank=" & strbank & "&type=" & strreporttype, False)
                Dim strpop As String = ""
                'strpop = "window.open('rptRV_PVreport.aspx?BackPageName=rptRV_PVreport.aspx&Pageame=" & Pageame & "&frmdate=" & strfromdate & "&todate=" & strtodate & "&trantype=" & strtrantype & "&tranid=" & strtranid & "&C_Btype=" & strbanktype & "&accfrm=" & strbankcode & "&bank=" & strbank & "&type=" & strreporttype & "&poststate=" & poststate & "','RepRVPVReg','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
                strpop = "window.open('rptRV_PVreport.aspx?BackPageName=rptRV_PVreport.aspx&Pageame=" & Pageame & "&frmdate=" & strfromdate & "&todate=" & strtodate & "&divid=" & divcode & "&trantype=" & strtrantype & "&tranid=" & strtranid & "&C_Btype=" & strbanktype & "&accfrm=" & strbankcode & "&bank=" & strbank & "&type=" & strreporttype & "&poststate=" & poststate & "','RepRVPVReg');"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("RptReceiptsRegister.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

            End Try
        End If
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        txtTranId.Text = ""
        ddlType.Value = "[Select]"
        ddlStatus.Value = "[Select]"
        ' ''ddlAccCode.Value = "[Select]"
        ' ''ddlAccName.Value = "[Select]"
        ' ''ddlBankCode.Value = "[Select]"
        ' ''ddlBankName.Value = "[Select]"
        txtFromDate.Text = ""
        txtTodate.Text = ""
        txtFromRecvAmt.Text = ""
        txtToRecvAmt.Text = ""
        If txtFromDate.Text = "" Then
            'txtFromDate.Text = Format(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"),"select fdate from toursmaster"), Date), "dd/MM/yyy")
            txtFromDate.Text = Format(CType(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), Date), "dd/MM/yyy")
        End If
        If txtTodate.Text = "" Then
            txtTodate.Text = DateAdd(DateInterval.Month, 1, objDateTime.GetSystemDateOnly(Session("dbconnectionName")))
        End If
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("~/MainPage.aspx")
    End Sub
End Class
