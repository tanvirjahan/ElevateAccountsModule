#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
#End Region

Partial Class AccountsModule_PurchaseInvoices
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim ObjDateTime As New clsDateTime
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    'For accounts posting
    Dim caccounts As clssave = Nothing
    Dim cacc As clsAccounts = Nothing
    Dim ctran As clstran = Nothing
    Dim csubtran As clsSubTran = Nothing
    Dim mbasecurrency As String = ""
    'For accounts posting
#End Region
#Region "Enum parameter/GridCol"
    Enum parameter
        acctype = 0
        acccode = 1
        glcode = 2
        against_tran_id = 3
        against_tran_type = 4
        against_tran_lineno = 5
        acc_div_id = 6
        frmdate = 7
        todate = 8
        requestid = 9
    End Enum
    Enum GridCol
        SrNo = 0
        InvoiceNo = 1
        RequestId = 2
        Particulars = 3
        Value = 4
        PIYNTCol = 5
        SupplierInvoiceValueTCol = 6
        SupplierInvoiceValueKWDTCol = 7
        DiffAmountKWDTCol = 8
        NarrationTCol = 9
        PostDiffACTCol = 10
        acc_tran_type = 11
        acc_tran_lineno = 12
    End Enum
#End Region
#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
    Public Sub NumbersHtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
        'txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
#End Region
#Region "NumbersDecimalRound"
    Public Sub NumbersDecimalRound(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
    Public Sub NumbersDecimalRoundHtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
    End Sub
#End Region
#Region "NumbersInt"
    Public Sub NumbersIntHtml(ByVal txtbox As HtmlInputText)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber1(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Public Sub NumbersInt(ByVal txtbox As TextBox)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber1(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
    Public Sub NumbersDateInt(ByVal txtbox As TextBox)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber2(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region
#Region "TextLock"
    Public Sub TextLock(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region
#Region "TextLock"
    Public Sub TextLockhtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            ViewState.Add("PurchaseInvoiceState", Request.QueryString("State"))
            ViewState.Add("PurchaseInvoiceRefCode", Request.QueryString("RefCode"))

            If Page.IsPostBack = False Then
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                txtconnection.Value = Session("dbconnectionName")

                FillDDL("IsPostBackFalse")
                txtPInvoiceDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy")
                txtSInvoiceDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy")
                txtFromDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy")
                txtToDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy")
                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
                txtbasecurr.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 457)
                NumbersHtml(txtExchRate)
                Label1.Text = txtbasecurr.Value + " Supplier Invoice Total"
                Label2.Text = txtbasecurr.Value + " Diff Amount Total"


                Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select top 1  sealdate from  sealing_master ")
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("sealdate")) = False Then
                            txtpdate.Text = CType(ds.Tables(0).Rows(0)("sealdate"), String)
                        End If
                    Else
                        txtpdate.Text = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 508)
                    End If
                End If

                btnDisplay.Attributes.Add("onclick", "return DispalyValidate()")
                txtExchRate.Attributes.Add("onchange", "ChangeValuesOnExchRate()")
                'SelectAll,DeSelectAll
                'btnSelectAll.Attributes.Add("onclick", "SelectAll()")
                'btnDeSelectAll.Attributes.Add("onclick", "DeSelectAll()")
                txtxtrequestid.Attributes.Add("onchange", "changesupplier()")

                divRes.Style("HEIGHT") = "0px"

                If ViewState("PurchaseInvoiceState") = "New" Then
                    lblHeading.Text = "Add Purchase Invoice"
                    'btnSave.Text = "Save"
                    ' btnPrint.Visible = False
                    DisableControlOnDisplay("UnVisible")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save')==false)return false;")
                ElseIf ViewState("PurchaseInvoiceState") = "Edit" Then
                    ShowRecord(ViewState("PurchaseInvoiceRefCode"))
                    ShowRecordForGrid()
                    lblHeading.Text = "Edit Purchase Invoice"
                    'btnSave.Text = "Update"
                    'btnPrint.Visible = False
                    btnClear.Visible = False
                    btnDisplay.Visible = False
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update')==false)return false;")
                ElseIf ViewState("PurchaseInvoiceState") = "Delete" Then
                    ShowRecord(ViewState("PurchaseInvoiceRefCode"))
                    ShowRecordForGrid()
                    lblHeading.Text = "Delete Purchase Invoice"
                    ' btnSave.Text = "Delete"

                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete')==false)return false;")
                ElseIf ViewState("PurchaseInvoiceState") = "View" Then
                    ShowRecord(ViewState("PurchaseInvoiceRefCode"))
                    ShowRecordForGrid()
                    lblHeading.Text = "View Purchase Invoice"
                    ' DisableControl()
                End If
                CheckPostUnpostRight(CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType("Accounts Module", String), "AccountsModule\PurchaseInvoicesSearch.aspx")
                DisableControl()

                btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")
                ddlNarration.Attributes.Add("onchange", "javascript:FillCombotoText('" + CType(ddlNarration.ClientID, String) + "','" + CType(txtNarration.ClientID, String) + "')")
                Dim typ As Type
                typ = GetType(DropDownList)
                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSupplierCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSpplierName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlAccrualCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlAccuralName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlControlCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlControlName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlPostToCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlPostToName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlNarration.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                btnPrint.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to print?')==false)return false;")
                btnSave.Attributes.Add("onclick", "return validate_click()")
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoices.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack = True Then
                FillDDL("IsPostBackTrue")
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoices.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub DisableControl()
        If ViewState("PurchaseInvoiceState") = "New" Then
            btnSave.Text = "Save"
        ElseIf ViewState("PurchaseInvoiceState") = "Edit" Then
            btnSave.Text = "Update"
        ElseIf ViewState("PurchaseInvoiceState") = "View" Or ViewState("PurchaseInvoiceState") = "Delete" Then
            txtPInvoiceNo.Disabled = True
            txtPInvoiceDate.Enabled = False
            txtSInvoiceDate.Enabled = False
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
            ddlType.Disabled = True
            ddlSupplierCode.Disabled = True
            ddlSpplierName.Disabled = True
            ddlPostToCode.Disabled = True
            ddlPostToName.Disabled = True
            ddlAccrualCode.Disabled = True
            ddlAccuralName.Disabled = True
            ddlControlCode.Disabled = True
            ddlControlName.Disabled = True
            txtSInvoiceNo.Disabled = True
            txtCurrency.Disabled = True
            txtExchRate.Disabled = True
            ddlNarration.Disabled = True
            txtNarration.Disabled = True
            gvResult.Enabled = False
            btnDisplay.Visible = False
            btnClear.Visible = False
            btnSelectAll.Visible = False
            btnDeSelectAll.Visible = False
            chkPost.Visible = False
            If ViewState("PurchaseInvoiceState") = "Delete" Then
                btnSave.Text = "Delete"
            End If
        End If
        If ViewState("PurchaseInvoiceState") = "View" Then
            btnPrint.Visible = True
            btnSave.Visible = False
        Else
            btnSave.Visible = True
            btnPrint.Visible = False
        End If
    End Sub
    Private Sub FillDDL(ByVal strType As String)
        Try
            If strType = "IsPostBackFalse" Then
                If ddlType.Value = "S" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "Code", "des", "select   Code,des from view_account where type ='S'", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpplierName, "des", "Code", "select   Code,des from view_account where type ='S'", True)


                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToCode, "postaccount", "partyname", "select top 10   Code as postaccount ,des as partyname from view_account where type ='S' order by Code ", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToName, "partyname", "postaccount", "select top 10   des as partyname,Code as postaccount  from view_account where type ='S' order by des ", True)

                End If
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlControlCode, "acctcode", "acctname", "select distinct acctcode,acctname from acctmast where isnull(controlyn,'N')='Y' and cust_supp='S'", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlControlName, "acctname", "acctcode", "select distinct acctcode,acctname from acctmast where isnull(controlyn,'N')='Y' and cust_supp='S'", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccrualCode, "acctcode", "acctname", "select distinct acctcode,acctname from acctmast where isnull(controlyn,'N')='Y' and cust_supp='S'", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccuralName, "acctname", "acctcode", "select distinct acctcode,acctname from acctmast where isnull(controlyn,'N')='Y' and cust_supp='S'", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlNarration, "narration", "narration", "select distinct narration from narration where active=1", True)

            ElseIf strType = "IsPostBackTrue" Then
                If ddlType.Value <> "[Select]" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "Code", "des", "select Code,des from view_account where type ='" & ddlType.Value & "'", True, ddlSupplierCode.Value)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpplierName, "des", "Code", "select Code,des from view_account where type ='" & ddlType.Value & "' ", True, ddlSpplierName.Value)
                    If ddlSupplierCode.Value <> "[Select]" Then
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToCode, "postaccount", "partyname", "select   Code as postaccount ,des as partyname from view_account where type ='" & ddlType.Value & "' and code <> '" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'  order by Code ", True, txtPostCode.Value)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToName, "partyname", "postaccount", "select    des as partyname,Code as postaccount  from view_account where type ='" & ddlType.Value & "' and code <> '" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'   order by des ", True, txtPostName.Value)
                    Else
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToCode, "postaccount", "partyname", "select top 10   Code as postaccount ,des as partyname from view_account where  type ='" & ddlType.Value & "' order by Code ", True, txtPostCode.Value)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToName, "partyname", "postaccount", "select top 10   des as partyname,Code as postaccount  from view_account where  type ='" & ddlType.Value & "' order by des ", True, txtPostName.Value)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoices.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Function ValidateDisplay() As Boolean
        If txtFromDate.Text <> "" And txtToDate.Text <> "" Then
            'Change 12/11/2008 *****************************
            'If CType(ObjDateTime.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) <= ObjDateTime.ConvertDateromTextBoxToDatabase(txtFromDate.Text) Then
            If CType(ObjDateTime.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date) < ObjDateTime.ConvertDateromTextBoxToDatabase(txtFromDate.Text) Then
                'Change 12/11/2008 *****************************
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To Date should be greater than From Date.');", True)
                SetFocus(txtToDate)
                ValidateDisplay = False
                Exit Function
            End If
        Else
            If txtFromDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select From Date.');", True)
                SetFocus(txtFromDate)
                ValidateDisplay = False
                Exit Function
            End If
            If txtToDate.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select To Date.');", True)
                SetFocus(txtToDate)
                ValidateDisplay = False
                Exit Function
            End If
        End If
        ValidateDisplay = True
    End Function

    Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        FillGrid()
    End Sub

    Public Function doSearch() As DataSet
        'Change 12/11/2008 *****************************
        Dim divid As String
        divid = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=511")
        'Change 12/11/2008 *****************************
        '

        doSearch = Nothing
        Try
            Dim count As Integer = [Enum].GetValues(GetType(parameter)).Length


            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(count) As SqlParameter

            parm(parameter.acctype) = New SqlParameter("@acctype", CType(ddlType.Value.Trim, String))
            If Not (ddlSupplierCode.Value = "[Select]") Then
                parm(parameter.acccode) = New SqlParameter("@acccode", CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text, String))
            Else
                parm(parameter.acccode) = New SqlParameter("@acccode", String.Empty)
            End If

            '------------glcode temp fill from accrual code
            If Not (ddlAccrualCode.Value = "[Select]") Then
                parm(parameter.glcode) = New SqlParameter("@glcode", CType(ddlAccrualCode.Items(ddlAccrualCode.SelectedIndex).Text, String))
            Else
                parm(parameter.glcode) = New SqlParameter("@glcode", String.Empty)
            End If

            If txtPInvoiceNo.Value <> "" Then
                parm(parameter.against_tran_id) = New SqlParameter("@against_tran_id", txtPInvoiceNo.Value)
                parm(parameter.against_tran_type) = New SqlParameter("@against_tran_type", "PI")
                parm(parameter.against_tran_lineno) = New SqlParameter("@against_tran_lineno", 1)
            Else
                parm(parameter.against_tran_id) = New SqlParameter("@against_tran_id", String.Empty)
                parm(parameter.against_tran_type) = New SqlParameter("@against_tran_type", String.Empty)
                parm(parameter.against_tran_lineno) = New SqlParameter("@against_tran_lineno", 0)
            End If


            '----------------- against_tran_lineno temp pass 0

            '----------------- acc_div_id temp pass 1
            'Change 12/11/2008 *****************************
            'parm(parameter.acc_div_id) = New SqlParameter("@acc_div_id", 1)
            parm(parameter.acc_div_id) = New SqlParameter("@acc_div_id", divid)
            'Change 12/11/2008 *****************************


            If Not (txtFromDate.Text = "") Then
                parm(parameter.frmdate) = New SqlParameter("@frmdate", CType(ObjDateTime.ConvertDateromTextBoxToTextYearMonthDay(txtFromDate.Text), String))
            Else
                parm(parameter.frmdate) = New SqlParameter("@frmdate", "1900/01/01")
            End If
            If Not (txtToDate.Text = "") Then
                parm(parameter.todate) = New SqlParameter("@todate", CType(ObjDateTime.ConvertDateromTextBoxToTextYearMonthDay(txtToDate.Text), String))
            Else
                parm(parameter.todate) = New SqlParameter("@todate", "1900/01/01")
            End If

            If txtxtrequestid.Value <> "" Then
                parm(parameter.requestid) = New SqlParameter("@requestid", txtxtrequestid.Value.Trim)
            Else
                parm(parameter.requestid) = New SqlParameter("@requestid", String.Empty)
            End If


            For i = 0 To count - 1
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_providerdue", parms)
            Return ds
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoices.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function


    Private Sub EnableDisableControl(ByVal strType As String)
        If strType = "Enable" Then
            txtPInvoiceDate.Enabled = True
            txtSInvoiceDate.Enabled = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
            ddlType.Disabled = False
            ddlSupplierCode.Disabled = False
            ddlSpplierName.Disabled = False
            ddlPostToCode.Disabled = False
            ddlPostToName.Disabled = False
            ddlAccrualCode.Disabled = False
            ddlAccuralName.Disabled = False
            ddlControlCode.Disabled = False
            ddlControlName.Disabled = False
            'txtSInvoiceNo.Disabled = False
            'txtCurrency.Disabled = False
            'txtExchRate.Disabled = False
            'ddlNarration.Disabled = False
            'txtNarration.Disabled = False

            '----------- Fill page load data / clear previous data
            'txtPInvoiceDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy")
            'txtSInvoiceDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy")
            txtFromDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy")
            txtToDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy")
            '    FillDDL("IsPostBackFalse")

            ' gvResult.Visible = False
            lblMsg.Visible = False

            txtTotalDiffAmountKWD.Value = 0
            txtTotalSInvoiceValue.Value = 0
            txtTotalDiffAmountKWD.Visible = False
            txtTotalSInvoiceValue.Visible = False
            lblTotal.Visible = False
            txtNarration.Value = ""
            txtCurrency.Value = ""
            txtExchRate.Value = ""
            txtSInvoiceNo.Value = ""
            DisableControlOnDisplay("UnVisible")
        ElseIf strType = "Disable" Then

            'txtPInvoiceDate.Enabled = False
            'txtSInvoiceDate.Enabled = False
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
            ddlType.Disabled = True
            ddlSupplierCode.Disabled = True
            ddlSpplierName.Disabled = True
            ddlPostToCode.Disabled = True
            ddlPostToName.Disabled = True
            ddlAccrualCode.Disabled = True
            ddlAccuralName.Disabled = True
            ddlControlCode.Disabled = True
            ddlControlName.Disabled = True
            'txtSInvoiceNo.Disabled = True
            'txtCurrency.Disabled = True
            'txtExchRate.Disabled = True
            'ddlNarration.Disabled = True
            'txtNarration.Disabled = True

            txtTotalDiffAmountKWD.Visible = True
            txtTotalSInvoiceValue.Visible = True
            lblTotal.Visible = True
        End If

    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        EnableDisableControl("Enable")
        ddlType.Value = "S"
        ddlSupplierCode.Value = "[Select]"
        ddlSpplierName.Value = "[Select]"
        ddlAccrualCode.Value = "[Select]"
        ddlAccuralName.Value = "[Select]"
        ddlPostToCode.Value = "[Select]"
        ddlPostToName.Value = "[Select]"
        FillDDL("IsPostBackFalse")
    End Sub

    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal
        Rdamt = Math.Round(Val(Ramt), CType(txtdecimal.Value, Integer))
        Return Rdamt
    End Function

    Protected Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        Try
            If (e.Row.RowType = DataControlRowType.Header) Then
                e.Row.Cells(8).Text = "Supplier Invoice Value  " + "[" + txtbasecurr.Value + "]"
                e.Row.Cells(9).Text = "Diff Amount " + "[" + txtbasecurr.Value + "]"
            End If


            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim ddlPostAC As HtmlSelect
                Dim ddlPostdiffacName As HtmlSelect
                Dim txtNarr As HtmlInputText
                Dim chkSel As HtmlInputCheckBox
                Dim invoiceno As String
                Dim hdngroup As HiddenField

                Dim txtSInvVal, txtDiffAmount, txtSInvValKWD, txtDiffAmtKWD, txtSValue, txtexchrate As HtmlInputText
                ddlPostAC = CType(e.Row.FindControl("ddlPostDiffAC"), HtmlSelect)
                txtNarr = CType(e.Row.FindControl("txtNarration"), HtmlInputText)
                chkSel = CType(e.Row.FindControl("chkPIYN"), HtmlInputCheckBox)
                txtSInvVal = CType(e.Row.FindControl("txtSInvoiceValue"), HtmlInputText)
                txtSInvValKWD = CType(e.Row.FindControl("txtSInvoiceValueKWD"), HtmlInputText)
                txtDiffAmtKWD = CType(e.Row.FindControl("txtDiffAmountKWD"), HtmlInputText)
                txtSValue = CType(e.Row.FindControl("txtvalue"), HtmlInputText)
                txtDiffAmount = CType(e.Row.FindControl("txtDiffAmount"), HtmlInputText)
                txtexchrate = CType(e.Row.FindControl("txtConvRate"), HtmlInputText)
                ddlPostdiffacName = CType(e.Row.FindControl("ddlPostdiffacName"), HtmlSelect)
                hdngroup = CType(e.Row.FindControl("hdngroup"), HiddenField)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostAC, "acctcode", "acctname", "select acctcode,acctname from acctmast where controlyn='N' and bankyn='N'", True, ddlPostAC.Items(ddlPostAC.SelectedIndex).Text)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostdiffacName, "acctname", "acctcode", "select acctname,acctcode from acctmast where controlyn='N' and bankyn='N'", True, ddlPostAC.Value)

                'If ddlPostAC.Disabled = True Then
                '    ddlPostAC.Disabled = True
                'End If

                invoiceno = e.Row.Cells(1).Text

                NumbersDecimalRoundHtml(txtSInvVal)
                TextLockhtml(txtSInvValKWD)
                TextLockhtml(txtDiffAmtKWD)
                TextLockhtml(txtSValue)
                TextLockhtml(txtDiffAmount)


                'If ddlNarration.Value <> "[Select]" Then
                '    txtNarr.Value = ddlNarration.Value
                'ElseIf txtNarration.Value <> "" Then
                '    txtNarr.Value = txtNarration.Value
                'End If

                chkSel.Attributes.Add("OnClick", "javascript:FillSInvoiceValue('" + CType(chkSel.ClientID, String) + "','" + CType(txtSValue.ClientID, String) + "','" + CType(txtSInvVal.ClientID, String) + "','" + CType(txtSInvValKWD.ClientID, String) + "','" + CType(txtDiffAmtKWD.ClientID, String) + "','" + CType(ddlPostAC.ClientID, String) + "','" + CType(txtNarr.ClientID, String) + "','" + CType(txtDiffAmount.ClientID, String) + "','" + CType(txtexchrate.ClientID, String) + "','" + CType(ddlPostdiffacName.ClientID, String) + "')")
                txtSInvVal.Attributes.Add("Onblur", "javascript:OnchangeFillInvoiceValue('" + CType(chkSel.ClientID, String) + "','" + CType(txtSValue.ClientID, String) + "','" + CType(txtSInvVal.ClientID, String) + "','" + CType(txtSInvValKWD.ClientID, String) + "','" + CType(txtDiffAmtKWD.ClientID, String) + "','" + CType(ddlPostAC.ClientID, String) + "','" + CType(txtDiffAmount.ClientID, String) + "','" + CType(txtexchrate.ClientID, String) + "','" + CType(ddlPostdiffacName.ClientID, String) + "','" + CType(invoiceno, String) + "','" + CType(hdngroup.Value, String) + "')")
                ddlPostdiffacName.Attributes.Add("onchange", "Filldiffac('" + CType(ddlPostAC.ClientID, String) + "','" + CType(ddlPostdiffacName.ClientID, String) + "')")

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoices.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim sqlTrans As SqlTransaction
        Try

            Dim gvrow As GridViewRow
            Dim strdiv, particulars As String
            Dim invtype As Label
            Dim invlineno As Label

            Dim chkSel As HtmlInputCheckBox
            Dim txtSInvVal, txtNarr, txtDiffAmtKWD, txtDiffAmount, txtSValue, txtSInvValKWD, txtconvrate As HtmlInputText
            Dim ddlPostAC As HtmlSelect
            Dim acc_tranlinenno, acc_against_tranlinenno, acc_openlineno As Long
            Dim invvalue, invvaluebase, convrate As Double
            acc_tranlinenno = 1
            acc_openlineno = 100
            acc_against_tranlinenno = 500
            If Page.IsValid = True Then
                If ViewState("PurchaseInvoiceState") = "New" Or ViewState("PurchaseInvoiceState") = "Edit" Then
                    If ValidaPage() = False Then
                        Exit Sub
                    End If
                End If
                If ViewState("PurchaseInvoiceState") = "Edit" Or ViewState("PurchaseInvoiceState") = "Delete" Then
                    If validate_BillAgainst() = False Then
                        Exit Sub
                    End If
                End If

                If ViewState("PurchaseInvoiceState") = "New" Or ViewState("PurchaseInvoiceState") = "Edit" Or ViewState("PurchaseInvoiceState") = "Delete" Then
                    If Validateseal() = False Then
                        Exit Sub
                    End If
                End If


                'Change 12/11/2008 *****************************
                'strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511)
                strdiv = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
                'Change 12/11/2008 *****************************

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))         'connection open
                sqlTrans = SqlConn.BeginTransaction         'transaction start
                If chkPost.Checked = True Then
                    'For Accounts posting
                    initialclass(SqlConn, sqlTrans)
                    'For Accounts posting
                End If

                If (ViewState("PurchaseInvoiceState") = "New" Or ViewState("PurchaseInvoiceState") = "Edit") Then
                    If ViewState("PurchaseInvoiceState") = "New" Then
                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("PURCHASE", SqlConn, sqlTrans)
                        txtPInvoiceNo.Value = optionval.Trim
                        myCommand = New SqlCommand("sp_add_providerinv_header", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    ElseIf ViewState("PurchaseInvoiceState") = "Edit" Then
                        myCommand = New SqlCommand("sp_mod_providerinv_header", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    End If

                    myCommand.Parameters.Add(New SqlParameter("@div_id ", SqlDbType.VarChar, 10)).Value = CType(strdiv, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtPInvoiceNo.Value, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "PI"
                    myCommand.Parameters.Add(New SqlParameter("@tran_date", SqlDbType.DateTime)).Value = CType(ObjDateTime.ConvertDateromTextBoxToDatabase(txtPInvoiceDate.Text), Date)
                    myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = CType(ddlType.Value, String)
                    myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text, String)
                    If ddlPostToCode.Value <> "[Select]" Then
                        myCommand.Parameters.Add(New SqlParameter("@postaccount", SqlDbType.VarChar, 20)).Value = CType(ddlPostToCode.Items(ddlPostToCode.SelectedIndex).Text, String)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@postaccount", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If
                    If ddlAccrualCode.Value <> "[Select]" Then
                        myCommand.Parameters.Add(New SqlParameter("@accrualcode", SqlDbType.VarChar, 20)).Value = CType(ddlAccrualCode.Items(ddlAccrualCode.SelectedIndex).Text, String)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@accrualcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If

                    If ddlControlCode.Value <> "[Select]" Then
                        myCommand.Parameters.Add(New SqlParameter("@controlcode", SqlDbType.VarChar, 20)).Value = CType(ddlControlCode.Items(ddlControlCode.SelectedIndex).Text, String)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@controlcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If
                    myCommand.Parameters.Add(New SqlParameter("@supinvno", SqlDbType.VarChar, 20)).Value = CType(txtSInvoiceNo.Value, String)
                    myCommand.Parameters.Add(New SqlParameter("@supinvdate", SqlDbType.DateTime)).Value = CType(ObjDateTime.ConvertDateromTextBoxToDatabase(txtSInvoiceDate.Text), Date)
                    myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(txtCurrency.Value, String)
                    If txtExchRate.Value = "" Then
                        myCommand.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 18)).Value = System.DBNull.Value
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 18, 12)).Value = CType(Val(txtExchRate.Value), Decimal)
                    End If

                    myCommand.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 500)).Value = CType(txtNarration.Value, String)
                    myCommand.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = CType(ObjDateTime.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date)
                    myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDateTime.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date)
                    myCommand.Parameters.Add(New SqlParameter("@currencyamount", SqlDbType.Money)).Value = txtTotalSInvoiceValue.Value
                    myCommand.Parameters.Add(New SqlParameter("@baseamount", SqlDbType.Money)).Value = txtTotalSInvoiceValueKWD.Value
                    myCommand.Parameters.Add(New SqlParameter("@diffamount", SqlDbType.Money)).Value = txtTotalDiffAmountKWD.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_state", SqlDbType.Money)).Value = System.DBNull.Value
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If chkPost.Checked = True Then
                        myCommand.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "P"
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "U"
                    End If

                    myCommand.ExecuteNonQuery()

                    If chkPost.Checked = True Then
                        'For Accounts Posting
                        caccounts.clraccounts()
                        cacc.acc_tran_id = txtPInvoiceNo.Value
                        cacc.acc_tran_type = CType("PI", String)
                        cacc.acc_tran_date = Format(CType(txtPInvoiceDate.Text, Date), "yyyy/MM/dd")
                        cacc.acc_div_id = strdiv
                    End If

                    'Cr Posting To Control Account Code - now moved to grid each line posting 
                    'If chkPost.Checked = True Then
                    '    ctran = New clstran
                    '    ctran.acc_tran_id = cacc.acc_tran_id
                    '    ctran.acc_code = ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text
                    '    ctran.acc_type = ddlType.Value
                    '    ctran.acc_currency_id = txtCurrency.Value
                    '    ctran.acc_currency_rate = CType(txtExchRate.Value, Decimal)
                    '    ctran.acc_div_id = strdiv
                    '    ctran.acc_narration = txtNarration.Value
                    '    ctran.acc_tran_date = cacc.acc_tran_date
                    '    ctran.acc_tran_lineno = acc_tranlinenno
                    '    ctran.acc_tran_type = cacc.acc_tran_type
                    '    If ddlType.Value <> "G" Then
                    '        If ddlControlCode.Value <> "[Select]" Then
                    '            ctran.pacc_gl_code = CType(ddlControlCode.Items(ddlControlCode.SelectedIndex).Text, String)
                    '        Else
                    '            ctran.pacc_gl_code = ""
                    '        End If
                    '    Else
                    '        ctran.pacc_gl_code = ""
                    '    End If

                    '    ctran.acc_ref1 = ""
                    '    ctran.acc_ref2 = ""
                    '    ctran.acc_ref3 = ""
                    '    ctran.acc_ref4 = ""
                    '    cacc.addtran(ctran)

                    '    csubtran = New clsSubTran
                    '    csubtran.acc_against_tran_id = cacc.acc_tran_id
                    '    csubtran.acc_against_tran_lineno = acc_tranlinenno
                    '    csubtran.acc_against_tran_type = cacc.acc_tran_type
                    '    csubtran.acc_debit = DecRound(IIf(CType(txtTotalSInvoiceValue.Value, Decimal) < 0, Math.Abs(CType(txtTotalSInvoiceValue.Value, Decimal)), 0))
                    '    csubtran.acc_credit = DecRound(IIf(CType(CType(txtTotalSInvoiceValue.Value, Decimal), Decimal) > 0, CType(txtTotalSInvoiceValue.Value, Decimal), 0))
                    '    csubtran.acc_base_debit = DecRound(IIf(CType(txtTotalSInvoiceValueKWD.Value, Decimal) < 0, Math.Abs(CType(txtTotalSInvoiceValueKWD.Value, Decimal)), 0))
                    '    csubtran.acc_base_credit = DecRound(IIf(CType(CType(txtTotalSInvoiceValueKWD.Value, Decimal), Decimal) > 0, CType(txtTotalSInvoiceValueKWD.Value, Decimal), 0))

                    '    csubtran.acc_tran_date = cacc.acc_tran_date
                    '    csubtran.acc_due_date = cacc.acc_tran_date
                    '    csubtran.acc_field1 = CType(txtSInvoiceNo.Value, String)
                    '    csubtran.acc_field2 = CType(ObjDateTime.ConvertDateromTextBoxToDatabase(txtSInvoiceDate.Text), String)
                    '    csubtran.acc_field3 = CType(txtNarration.Value, String)
                    '    csubtran.acc_field4 = ""
                    '    csubtran.acc_field5 = ""
                    '    csubtran.acc_tran_id = cacc.acc_tran_id
                    '    csubtran.acc_tran_lineno = acc_tranlinenno
                    '    csubtran.acc_tran_type = cacc.acc_tran_type
                    '    csubtran.acc_narration = txtNarration.Value
                    '    csubtran.acc_type = ddlType.Value
                    '    csubtran.currate = CType(txtExchRate.Value, Decimal)
                    '    csubtran.costcentercode = ""
                    '    cacc.addsubtran(csubtran)
                    '    acc_tranlinenno = acc_tranlinenno + 1
                    'End If

                    If ViewState("PurchaseInvoiceState") = "Edit" Then
                        If chkPost.Checked = False Then
                            myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure
                            myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "providerinv_header"
                            myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtPInvoiceNo.Value
                            myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = "PI"
                            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                            myCommand.ExecuteNonQuery()
                        End If
                        myCommand = New SqlCommand("sp_del_open_detail_new", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtPInvoiceNo.Value, String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = "PI"
                        myCommand.ExecuteNonQuery()

                        myCommand = New SqlCommand("sp_del_providerinv_detail", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@div_id ", SqlDbType.VarChar, 10)).Value = CType(strdiv, String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtPInvoiceNo.Value, String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "PI"
                        myCommand.ExecuteNonQuery()
                    End If



                    For Each gvrow In gvResult.Rows
                        chkSel = CType(gvrow.FindControl("chkPIYN"), HtmlInputCheckBox)
                        If chkSel.Checked = True Then
                            myCommand = New SqlCommand("sp_add_providerinv_detail", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure

                            myCommand.Parameters.Add(New SqlParameter("@div_id ", SqlDbType.VarChar, 10)).Value = CType(strdiv, String)
                            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtPInvoiceNo.Value, String)
                            myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "PI"
                            myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int, 9)).Value = CType(gvrow.Cells(0).Text, Integer)
                            myCommand.Parameters.Add(New SqlParameter("@invno", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(1).Text, String)

                            invtype = gvrow.FindControl("lblAccTranType")
                            If invtype.Text <> "" Then
                                myCommand.Parameters.Add(New SqlParameter("@invtype", SqlDbType.VarChar, 20)).Value = CType(invtype.Text, String)
                            End If
                            invlineno = gvrow.FindControl("lblAccTranLineno")
                            If invlineno.Text <> "" Then
                                myCommand.Parameters.Add(New SqlParameter("@invlineno", SqlDbType.Int, 9)).Value = CType(invlineno.Text, Integer)
                            End If
                            If CType(gvrow.Cells(2).Text, String) = "" Or CType(gvrow.Cells(2).Text, String) = "&nbsp;" Then
                                myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = ""
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(2).Text, String)
                            End If
                            If CType(gvrow.Cells(3).Text, String) = "" Or CType(gvrow.Cells(3).Text, String) = "&nbsp;" Then
                                myCommand.Parameters.Add(New SqlParameter("@particulars", SqlDbType.VarChar, 500)).Value = ""
                                particulars = ""
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@particulars", SqlDbType.VarChar, 500)).Value = CType(gvrow.Cells(3).Text, String)
                                particulars = CType(gvrow.Cells(3).Text, String)
                            End If


                            ddlPostAC = CType(gvrow.FindControl("ddlPostDiffAC"), HtmlSelect)
                            If ddlPostAC.Value = "[Select]" Then
                                txtSValue = CType(gvrow.FindControl("txtSInvoiceValue"), HtmlInputText)
                            Else
                                txtSValue = CType(gvrow.FindControl("txtvalue"), HtmlInputText) 'this is original

                            End If


                            If txtSValue.Value = "" Or txtSValue.Value = "&nbsp;" Then
                                myCommand.Parameters.Add(New SqlParameter("@invvalue", SqlDbType.Money)).Value = "0.00"
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@invvalue", SqlDbType.Money)).Value = txtSValue.Value
                            End If

                            txtSInvVal = CType(gvrow.FindControl("txtSInvoiceValue"), HtmlInputText)
                            If txtSInvVal.Value = "" Or txtSInvVal.Value = "&nbsp;" Then
                                myCommand.Parameters.Add(New SqlParameter("@currencyamount", SqlDbType.Money)).Value = "0.00"
                                invvalue = 0
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@currencyamount", SqlDbType.Money)).Value = txtSInvVal.Value
                                invvalue = txtSInvVal.Value
                            End If

                            txtSInvValKWD = CType(gvrow.FindControl("txtSInvoiceValueKWD"), HtmlInputText)
                            If txtSInvValKWD.Value = "" Or txtSInvValKWD.Value = "&nbsp;" Then
                                myCommand.Parameters.Add(New SqlParameter("@baseamount", SqlDbType.Money)).Value = "0.00"
                                invvaluebase = 0
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@baseamount", SqlDbType.Money)).Value = txtSInvValKWD.Value
                                invvaluebase = txtSInvValKWD.Value
                            End If
                            txtDiffAmtKWD = CType(gvrow.FindControl("txtDiffAmountKWD"), HtmlInputText)
                            If txtDiffAmtKWD.Value = "" Or txtDiffAmtKWD.Value = "&nbsp;" Then
                                myCommand.Parameters.Add(New SqlParameter("@diffamount", SqlDbType.Money)).Value = "0.00"
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@diffamount", SqlDbType.Money)).Value = txtDiffAmtKWD.Value
                            End If

                            txtNarr = CType(gvrow.FindControl("txtNarration"), HtmlInputText)
                            If txtNarr.Value = "" Or txtNarr.Value = "&nbsp;" Then
                                myCommand.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 500)).Value = System.DBNull.Value
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 500)).Value = txtNarr.Value
                            End If
                            ddlPostAC = CType(gvrow.FindControl("ddlPostDiffAC"), HtmlSelect)
                            If ddlPostAC.Items(ddlPostAC.SelectedIndex).Text <> "[Select]" Then
                                myCommand.Parameters.Add(New SqlParameter("@diffpostac", SqlDbType.VarChar, 20)).Value = ddlPostAC.Items(ddlPostAC.SelectedIndex).Text
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@diffpostac", SqlDbType.VarChar, 20)).Value = System.DBNull.Value
                            End If
                            txtconvrate = CType(gvrow.FindControl("txtConvRate"), HtmlInputText)
                            If txtconvrate.Value <> "" Then
                                myCommand.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 18, 12)).Value = CType(Val(txtconvrate.Value), Decimal)
                                convrate = CType(Val(txtconvrate.Value), Decimal)
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 18, 12)).Value = 0
                                convrate = 0
                            End If

                            myCommand.ExecuteNonQuery()

                            acc_openlineno = CType(gvrow.Cells(0).Text, Integer) + 10000
                            acc_against_tranlinenno = CType(gvrow.Cells(0).Text, Integer) + 20000
                            If chkPost.Checked = True Then
                                ctran = New clstran
                                ctran.acc_tran_id = cacc.acc_tran_id
                                ctran.acc_code = ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text
                                ctran.acc_type = ddlType.Value
                                ctran.acc_currency_id = txtCurrency.Value
                                ctran.acc_currency_rate = convrate
                                ctran.acc_div_id = strdiv
                                ctran.acc_narration = txtNarr.Value
                                ctran.acc_tran_date = cacc.acc_tran_date
                                ctran.acc_tran_lineno = CType(gvrow.Cells(0).Text, Integer)
                                ctran.acc_tran_type = cacc.acc_tran_type
                                If ddlType.Value <> "G" Then
                                    If ddlControlCode.Value <> "[Select]" Then
                                        ctran.pacc_gl_code = CType(ddlControlCode.Items(ddlControlCode.SelectedIndex).Text, String)
                                    Else
                                        ctran.pacc_gl_code = ""
                                    End If
                                Else
                                    ctran.pacc_gl_code = ""
                                End If

                                ctran.acc_ref1 = ""
                                ctran.acc_ref2 = ""
                                ctran.acc_ref3 = ""
                                ctran.acc_ref4 = ""
                                cacc.addtran(ctran)

                                csubtran = New clsSubTran
                                csubtran.acc_against_tran_id = cacc.acc_tran_id
                                csubtran.acc_against_tran_lineno = CType(gvrow.Cells(0).Text, Integer)
                                csubtran.acc_against_tran_type = cacc.acc_tran_type
                                csubtran.acc_debit = DecRound(IIf(CType(invvalue, Decimal) < 0, Math.Abs(CType(invvalue, Decimal)), 0))
                                csubtran.acc_credit = DecRound(IIf(CType(CType(invvalue, Decimal), Decimal) > 0, CType(invvalue, Decimal), 0))
                                csubtran.acc_base_debit = DecRound(IIf(CType(invvaluebase, Decimal) < 0, Math.Abs(CType(invvaluebase, Decimal)), 0))
                                csubtran.acc_base_credit = DecRound(IIf(CType(CType(invvaluebase, Decimal), Decimal) > 0, CType(invvaluebase, Decimal), 0))

                                csubtran.acc_tran_date = cacc.acc_tran_date
                                csubtran.acc_due_date = cacc.acc_tran_date
                                csubtran.acc_field1 = CType(txtSInvoiceNo.Value, String)
                                csubtran.acc_field2 = CType(ObjDateTime.ConvertDateromTextBoxToDatabase(txtSInvoiceDate.Text), String)
                                csubtran.acc_field3 = CType(particulars, String)
                                csubtran.acc_field4 = ""
                                csubtran.acc_field5 = ""
                                csubtran.acc_tran_id = cacc.acc_tran_id
                                csubtran.acc_tran_lineno = CType(gvrow.Cells(0).Text, Integer)
                                csubtran.acc_tran_type = cacc.acc_tran_type
                                csubtran.acc_narration = txtNarration.Value
                                csubtran.acc_type = ddlType.Value
                                csubtran.currate = convrate
                                csubtran.costcentercode = ""
                                cacc.addsubtran(csubtran)
                                acc_tranlinenno = acc_tranlinenno + 1

                                'For accrual posting only ctran required, subtran will be taken care in accrual posting
                                ctran = New clstran
                                ctran.acc_tran_id = cacc.acc_tran_id
                                ctran.acc_code = ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text
                                ctran.acc_type = ddlType.Value
                                ctran.acc_currency_id = txtCurrency.Value
                                ctran.acc_currency_rate = convrate
                                ctran.acc_div_id = strdiv
                                ctran.acc_narration = txtNarr.Value
                                ctran.acc_tran_date = cacc.acc_tran_date
                                ctran.acc_tran_lineno = acc_openlineno
                                ctran.acc_tran_type = cacc.acc_tran_type
                                If ddlType.Value <> "G" Then
                                    If ddlAccrualCode.Value <> "[Select]" Then
                                        ctran.pacc_gl_code = CType(ddlAccrualCode.Items(ddlAccrualCode.SelectedIndex).Text, String)
                                    Else
                                        ctran.pacc_gl_code = ""
                                    End If
                                Else
                                    ctran.pacc_gl_code = ""
                                End If

                                ctran.acc_ref1 = ""
                                ctran.acc_ref2 = ""
                                ctran.acc_ref3 = ""
                                ctran.acc_ref4 = ""
                                cacc.addtran(ctran)

                            End If

                            'Save_Open_detail(CType(strdiv, String), CType(gvrow.Cells(1).Text, String), CType(invtype.Text, String), txtPInvoiceDate.Text, CType(invlineno.Text, Integer), txtExchRate.Value, txtSValue.Value, CType(gvrow.Cells(0).Text, Integer), SqlConn, sqlTrans)

                            Save_Open_detail(CType(strdiv, String), CType(gvrow.Cells(1).Text, String), CType(invtype.Text, String), txtPInvoiceDate.Text, CType(invlineno.Text, Integer), convrate, txtSValue.Value, acc_openlineno, SqlConn, sqlTrans)
                            'acc_openlineno = acc_openlineno + 1

                            If txtDiffAmtKWD.Value = "" Or txtDiffAmtKWD.Value = "&nbsp;" Then
                            Else
                                If Val(txtDiffAmtKWD.Value) <> 0 And ddlPostAC.Value <> "[Select]" Then
                                    txtDiffAmount = CType(gvrow.FindControl("txtDiffAmount"), HtmlInputText)
                                    If chkPost.Checked = True Then
                                        ctran = New clstran
                                        ctran.acc_tran_id = cacc.acc_tran_id
                                        ctran.acc_code = ddlPostAC.Items(ddlPostAC.SelectedIndex).Text    'ddlPostAC.Value
                                        ctran.acc_type = "G"
                                        ctran.acc_currency_id = mbasecurrency
                                        ctran.acc_currency_rate = 1
                                        ctran.acc_div_id = strdiv
                                        ctran.acc_narration = txtNarr.Value
                                        ctran.acc_tran_date = cacc.acc_tran_date
                                        ctran.acc_tran_lineno = acc_against_tranlinenno
                                        ctran.acc_tran_type = cacc.acc_tran_type
                                        ctran.pacc_gl_code = ""
                                        ctran.acc_ref1 = ""
                                        ctran.acc_ref2 = ""
                                        ctran.acc_ref3 = ""
                                        ctran.acc_ref4 = ""
                                        cacc.addtran(ctran)

                                        csubtran = New clsSubTran
                                        csubtran.acc_against_tran_id = cacc.acc_tran_id
                                        csubtran.acc_against_tran_lineno = acc_against_tranlinenno
                                        csubtran.acc_against_tran_type = cacc.acc_tran_type
                                        csubtran.acc_debit = DecRound(IIf(CType(txtDiffAmtKWD.Value, Decimal) < 0, Math.Abs(CType(txtDiffAmtKWD.Value, Decimal)), 0))
                                        csubtran.acc_credit = DecRound(IIf(CType(CType(txtDiffAmtKWD.Value, Decimal), Decimal) > 0, CType(txtDiffAmtKWD.Value, Decimal), 0))
                                        csubtran.acc_base_debit = DecRound(IIf(CType(txtDiffAmtKWD.Value, Decimal) < 0, Math.Abs(CType(txtDiffAmtKWD.Value, Decimal)), 0))
                                        csubtran.acc_base_credit = DecRound(IIf(CType(CType(txtDiffAmtKWD.Value, Decimal), Decimal) > 0, CType(txtDiffAmtKWD.Value, Decimal), 0))

                                        csubtran.acc_tran_date = cacc.acc_tran_date
                                        csubtran.acc_due_date = cacc.acc_tran_date
                                        csubtran.acc_field1 = ""
                                        csubtran.acc_field2 = ""
                                        csubtran.acc_field3 = ""
                                        csubtran.acc_field4 = ""
                                        csubtran.acc_field5 = ""
                                        csubtran.acc_tran_id = cacc.acc_tran_id
                                        csubtran.acc_tran_lineno = acc_against_tranlinenno ' acc_tranlinenno
                                        csubtran.acc_tran_type = cacc.acc_tran_type
                                        csubtran.acc_narration = txtNarr.Value
                                        csubtran.acc_type = "G"
                                        csubtran.currate = 1
                                        csubtran.costcentercode = ""
                                        cacc.addsubtran(csubtran)
                                        ' acc_tranlinenno = acc_tranlinenno + 1
                                        'acc_against_tranlinenno = acc_against_tranlinenno + 1
                                    End If
                                End If
                            End If

                        End If
                    Next

                    If chkPost.Checked = True Then
                        'For Accounts Posting
                        cacc.table_name = ""
                        caccounts.Addaccounts(cacc)
                        If caccounts.saveaccounts(Session("dbconnectionName"), SqlConn, sqlTrans, Me.Page) <> 0 Then
                            Err.Raise(vbObjectError + 100)
                        End If
                        'For Accounts Posting
                        lblPostmsg.Text = "Posted"
                        lblPostmsg.ForeColor = Drawing.Color.Red
                    Else
                        lblPostmsg.Text = "UnPosted"
                        lblPostmsg.ForeColor = Drawing.Color.Green
                    End If
                ElseIf ViewState("PurchaseInvoiceState") = "Delete" Then

                    myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "providerinv_header"
                    myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtPInvoiceNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = "PI"
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.ExecuteNonQuery()

                    myCommand = New SqlCommand("sp_del_open_detail_new", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtPInvoiceNo.Value, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = "PI"
                    myCommand.ExecuteNonQuery()

                    myCommand = New SqlCommand("sp_del_providerinv", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id ", SqlDbType.VarChar, 10)).Value = CType(strdiv, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtPInvoiceNo.Value, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "PI"
                    myCommand.ExecuteNonQuery()
                End If

                sqlTrans.Commit()                               'transaction commit
                clsDBConnect.dbSqlTransation(sqlTrans)          'close transaction  
                clsDBConnect.dbCommandClose(myCommand)          'close command
                clsDBConnect.dbConnectionClose(SqlConn)         'close connection
            End If

            'Response.Redirect("PurchaseInvoicesSearch.aspx", False)

            If ViewState("PurchaseInvoiceState") = "Delete" Then
                'Response.Redirect("PurchaseInvoicesSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('PurchaseInvoiceWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            Else
                btnPrint.Visible = True
                ViewState("PurchaseInvoiceState") = "View"
                DisableControl()
                'btnPrint_Click(sender, e)
            End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoices.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#Region "Public Sub Save_Open_detail(ByVal intReceiptLinNo As String, ByVal SqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)"
    Public Sub Save_Open_detail(ByVal strdiv As String, ByVal TranId As String, ByVal TranType As String, ByVal Trandate As String, ByVal TranLineNo As String, ByVal Exrate As Decimal, ByVal open_amt As Decimal, ByVal againstLineno As String, ByVal SqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)
        'strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511)
        myCommand = New SqlCommand("sp_add_open_detail_new", SqlConn, sqlTrans)
        myCommand.CommandType = CommandType.StoredProcedure
        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = TranId
        myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = TranType
        myCommand.Parameters.Add(New SqlParameter("@tran_date ", SqlDbType.DateTime)).Value = Format(CType(Trandate, Date), "yyyy/MM/dd")
        myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int)).Value = TranLineNo

        myCommand.Parameters.Add(New SqlParameter("@against_tran_id", SqlDbType.VarChar, 20)).Value = txtPInvoiceNo.Value
        myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int)).Value = againstLineno '1
        myCommand.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = "PI"
        myCommand.Parameters.Add(New SqlParameter("@against_tran_date ", SqlDbType.DateTime)).Value = Format(CType(txtPInvoiceDate.Text, Date), "yyyy/MM/dd")
        If txtPInvoiceDate.Text = "" Then
            myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = DBNull.Value
        Else
            myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = Format(CType(txtPInvoiceDate.Text, Date), "yyyy/MM/dd")
        End If
        'spersoncode = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"agentmast", "spersoncode", "agentcode", strAccCode)
        myCommand.Parameters.Add(New SqlParameter("@open_sales_code", SqlDbType.VarChar, 10)).Value = "" 'DBNull.Value
        myCommand.Parameters.Add(New SqlParameter("@open_debit", SqlDbType.Money)).Value = DecRound(IIf(CType(CType(open_amt, Decimal), Decimal) > 0, CType(open_amt, Decimal), 0))
        myCommand.Parameters.Add(New SqlParameter("@open_credit", SqlDbType.Money)).Value = DecRound(IIf(CType(open_amt, Decimal) < 0, Math.Abs(CType(open_amt, Decimal)), 0))
        myCommand.Parameters.Add(New SqlParameter("@open_field1", SqlDbType.VarChar, 100)).Value = ""
        myCommand.Parameters.Add(New SqlParameter("@open_field2", SqlDbType.VarChar, 100)).Value = ""
        myCommand.Parameters.Add(New SqlParameter("@open_field3", SqlDbType.VarChar, 100)).Value = ""
        myCommand.Parameters.Add(New SqlParameter("@open_field4", SqlDbType.VarChar, 100)).Value = ""
        myCommand.Parameters.Add(New SqlParameter("@open_field5", SqlDbType.VarChar, 100)).Value = ""
        myCommand.Parameters.Add(New SqlParameter("@open_mode", SqlDbType.Char, 1)).Value = "B"
        myCommand.Parameters.Add(New SqlParameter("@open_exchg_diff", SqlDbType.Money)).Value = 0
        myCommand.Parameters.Add(New SqlParameter("@dr_cr", SqlDbType.Char, 1)).Value = DBNull.Value
        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
        myCommand.Parameters.Add(New SqlParameter("@currency_rate", SqlDbType.Decimal, 18, 12)).Value = Val(Exrate)
        myCommand.Parameters.Add(New SqlParameter("@base_debit", SqlDbType.Money)).Value = DecRound(IIf(CType(CType(open_amt, Decimal), Decimal) > 0, DecRound(CType(open_amt, Decimal) * CType(Val(Exrate), Decimal)), 0))
        myCommand.Parameters.Add(New SqlParameter("@base_credit", SqlDbType.Money)).Value = DecRound(IIf(CType(open_amt, Decimal) < 0, DecRound(Math.Abs(CType(open_amt, Decimal)) * CType(Val(Exrate), Decimal)), 0))
        myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.Char, 1)).Value = ddlType.Value
        myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text
        myCommand.Parameters.Add(New SqlParameter("@acc_gl_code", SqlDbType.VarChar, 20)).Value = CType(ddlAccrualCode.Items(ddlAccrualCode.SelectedIndex).Text, String)
        myCommand.ExecuteNonQuery()
    End Sub
#End Region
    Private Function ValidaPage() As Boolean
        Dim gvrow As GridViewRow
        Dim chkSel As HtmlInputCheckBox
        Dim txtSInvVal As HtmlInputText
        Dim txtNarr As HtmlInputText
        Dim txtDiffAmtKWD As HtmlInputText
        Dim ddlPostAC As HtmlSelect
        Dim ddlPostdiffacName As HtmlSelect
        If txtTotalSInvoiceValue.Value = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select atleast one record.');", True)
            SetFocus(gvResult)
            ValidaPage = False
            Exit Function
        End If
        If ddlControlCode.Value = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select control account code.');", True)
            SetFocus(ddlControlCode)
            ValidaPage = False
            Exit Function
        End If

        'If txtExchRate.Value = "" Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Exchange rate can not be blank.');", True)
        '    SetFocus(txtExchRate)
        '    ValidaPage = False
        '    Exit Function
        'ElseIf txtExchRate.Value <= 0 Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Exchange rate must be greater than zero.');", True)
        '    SetFocus(txtExchRate)
        '    ValidaPage = False
        '    Exit Function
        'End If

        If txtNarration.Value = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select or enter narration.');", True)
            SetFocus(txtNarration)
            ValidaPage = False
            Exit Function
        End If

        For Each gvrow In gvResult.Rows
            chkSel = CType(gvrow.FindControl("chkPIYN"), HtmlInputCheckBox)
            txtSInvVal = CType(gvrow.FindControl("txtSInvoiceValue"), HtmlInputText)
            txtNarr = CType(gvrow.FindControl("txtNarration"), HtmlInputText)
            txtDiffAmtKWD = CType(gvrow.FindControl("txtDiffAmountKWD"), HtmlInputText)
            ddlPostAC = CType(gvrow.FindControl("ddlPostDiffAC"), HtmlSelect)
            ddlPostdiffacName = CType(gvrow.FindControl("ddlPostdiffacName"), HtmlSelect)
            If chkSel.Checked = True Then
                If txtSInvVal.Value = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Invoice value can not be zero.');", True)
                    SetFocus(txtSInvVal)
                    ValidaPage = False
                    Exit Function
                End If
                If txtNarr.Value = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter narration.');", True)
                    SetFocus(txtNarr)
                    ValidaPage = False
                    Exit Function
                End If
                If txtDiffAmtKWD.Value <> "" Then
                    If txtDiffAmtKWD.Value <> 0 Then
                        If ddlPostAC.Value = "[Select]" Then
                            ddlPostdiffacName.Disabled = False
                            '    ddlPostAC.Disabled = False
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select post diff account if need.');", True)
                            SetFocus(ddlPostAC)
                            'ValidaPage = False
                            'Exit Function
                        End If
                    End If
                End If
            End If
        Next
        ValidaPage = True
    End Function
    Public Function Validateseal() As Boolean
        Try

            Validateseal = True
            Dim invdate As DateTime
            Dim sealdate As DateTime
            Dim MyCultureInfo As New CultureInfo("fr-Fr")
            invdate = DateTime.Parse(txtPInvoiceDate.Text, MyCultureInfo, DateTimeStyles.None)
            sealdate = DateTime.Parse(txtpdate.Text, MyCultureInfo, DateTimeStyles.None)
            If invdate <= sealdate Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed in this period cannot make entry.Close the entry and make with another date')", True)
                Validateseal = False
            End If

        Catch ex As Exception
            Validateseal = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("requestforinvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
    Private Sub ShowRecord(ByVal TranId As String)
        Try
            Dim strSqlQry As String
            strSqlQry = "select * from providerinv_header where tran_id='" & TranId & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                  'connection open
            myCommand = New SqlCommand(strSqlQry, SqlConn)
            mySqlReader = myCommand.ExecuteReader
            If mySqlReader.HasRows = True Then
                If mySqlReader.Read Then
                    If IsDBNull(mySqlReader("post_state")) = False Then
                        If CType(mySqlReader("post_state"), String) = "P" Then
                            lblPostmsg.Text = "Posted"
                            lblPostmsg.ForeColor = Drawing.Color.Red
                            chkPost.Checked = True
                        Else
                            lblPostmsg.Text = "UnPosted"
                            lblPostmsg.ForeColor = Drawing.Color.Green
                        End If
                    Else
                        lblPostmsg.Text = "UnPosted"
                        lblPostmsg.ForeColor = Drawing.Color.Green
                    End If

                    If IsDBNull(mySqlReader("tran_id")) = False Then
                        txtPInvoiceNo.Value = mySqlReader("tran_id")
                    End If


                    'Format(CType(mySqlReader("cheque_date"), Date), "dd/MM/yyyy")
                    If IsDBNull(mySqlReader("tran_date")) = False Then
                        txtPInvoiceDate.Text = Format(CType(mySqlReader("tran_date"), Date), "dd/MM/yyyy")
                    End If
                    If IsDBNull(mySqlReader("acc_type")) = False Then
                        ddlType.Value = mySqlReader("acc_type")
                    End If

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "code", "des", "select code,des from view_account where type = '" & ddlType.Value & "' order by code", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpplierName, "des", "code", "select des,code from view_account where type = '" & ddlType.Value & "' order by des", True)


                    If IsDBNull(mySqlReader("acc_code")) = False Then
                        ddlSpplierName.Value = mySqlReader("acc_code")
                        ddlSupplierCode.Value = ddlSpplierName.Items(ddlSpplierName.SelectedIndex).Text
                    Else
                        ddlSpplierName.Value = "[Select]"
                        ddlSupplierCode.Value = "[Select]"
                    End If

                    If ddlSupplierCode.Value <> "[Select]" Then
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToCode, "postaccount", "partyname", "select   Code as postaccount ,des as partyname from view_account where type ='" & ddlType.Value & "' and code <> '" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'  order by Code ", True)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToName, "partyname", "postaccount", "select    des as partyname,Code as postaccount  from view_account where type ='" & ddlType.Value & "' and code <> '" & ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text & "'   order by des ", True)
                    Else
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToCode, "postaccount", "partyname", "select top 10   Code as postaccount ,des as partyname from view_account where type ='" & ddlType.Value & "' order by Code ", True)
                        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPostToName, "partyname", "postaccount", "select top 10   des as partyname,Code as postaccount  from view_account where type ='" & ddlType.Value & "' order by des ", True)
                    End If

                    If IsDBNull(mySqlReader("postaccount")) = False Then
                        ddlPostToName.Value = mySqlReader("postaccount")

                        ddlPostToCode.Value = ddlPostToName.Items(ddlPostToName.SelectedIndex).Text
                        txtPostCode.Value = mySqlReader("postaccount")
                        txtPostName.Value = ddlPostToName.Items(ddlPostToName.SelectedIndex).Text
                    Else
                        ddlPostToName.Value = "[Select]"
                        ddlPostToCode.Value = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("accrualcode")) = False Then
                        ddlAccuralName.Value = mySqlReader("accrualcode")
                        ddlAccrualCode.Value = ddlAccuralName.Items(ddlAccuralName.SelectedIndex).Text
                    Else
                        ddlAccuralName.Value = "[Select]"
                        ddlAccrualCode.Value = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("controlcode")) = False Then
                        ddlControlName.Value = mySqlReader("controlcode")
                        ddlControlCode.Value = ddlControlName.Items(ddlControlName.SelectedIndex).Text
                    Else
                        ddlControlName.Value = "[Select]"
                        ddlControlCode.Value = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("supinvno")) = False Then
                        txtSInvoiceNo.Value = mySqlReader("supinvno")
                    End If
                    If IsDBNull(mySqlReader("supinvdate")) = False Then
                        txtSInvoiceDate.Text = Format(CType(mySqlReader("supinvdate"), Date), "dd/MM/yyyy")
                    End If
                    If IsDBNull(mySqlReader("currcode")) = False Then
                        txtCurrency.Value = mySqlReader("currcode")
                    End If
                    If IsDBNull(mySqlReader("convrate")) = False Then
                        txtExchRate.Value = mySqlReader("convrate")
                    End If
                    If IsDBNull(mySqlReader("narration")) = False Then
                        ' ddlNarration.Value = mySqlReader("narration")
                        ' If ddlNarration.Value = "[Select]" Then
                        txtNarration.Value = mySqlReader("narration")
                        'End If
                    End If
                    If IsDBNull(mySqlReader("fromdate")) = False Then
                        txtFromDate.Text = Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy")
                    End If
                    If IsDBNull(mySqlReader("todate")) = False Then
                        txtToDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")
                    End If
                    'If IsDBNull(mySqlReader("currencyamount")) = False Then
                    '    txtTotalSInvoiceValue.Value = mySqlReader("currencyamount")
                    'End If
                    'If IsDBNull(mySqlReader("baseamount")) = False Then
                    '    txtTotalSInvoiceValueKWD.Value = mySqlReader("baseamount")
                    'End If
                    'If IsDBNull(mySqlReader("diffamount")) = False Then
                    '    txtTotalDiffAmountKWD.Value = mySqlReader("diffamount")
                    'End If

                    If Trim(txtbasecurr.Value) = Trim(txtCurrency.Value) Then
                        txtExchRate.Disabled = True
                    Else
                        txtExchRate.Disabled = False
                    End If

                End If

                'ShowRecordForGrid()


            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoices.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try

    End Sub

    Private Sub ShowRecordForGrid()
        Dim gvrow As GridViewRow
        Dim chkSel As HtmlInputCheckBox
        Dim txtvalue, txtSInvVal, txtNarr, txtDiffAmtKWD, txtDiffAmount, txtSInvValKWD As HtmlInputText
        Dim ddlPostAC As HtmlSelect
        Dim ddlPostdiffacName As HtmlSelect
        'Dim txtSValue As HtmlInputText
        Dim strCriteria As String = ""
        Dim invtype As Label
        Dim invlineno As Label

        'select currencyamount,baseamount,diffamount,narration,diffpostac from providerinv_detail
        'where invno='' and invtype='' and invlineno='' 

        FillGrid()
        For Each gvrow In gvResult.Rows
            invtype = gvrow.FindControl("lblAccTranType")
            invlineno = gvrow.FindControl("lblAccTranLineno")
            strCriteria = " tran_id='" & txtPInvoiceNo.Value & "' and invno='" & gvrow.Cells(1).Text & "' and invtype='" & invtype.Text & "' and invlineno='" & invlineno.Text & "'"

            'txtSValue = CType(gvrow.FindControl("txtvalue"), HtmlInputText)
            chkSel = CType(gvrow.FindControl("chkPIYN"), HtmlInputCheckBox)
            txtSInvVal = CType(gvrow.FindControl("txtSInvoiceValue"), HtmlInputText)
            txtSInvValKWD = CType(gvrow.FindControl("txtSInvoiceValueKWD"), HtmlInputText)
            txtDiffAmtKWD = CType(gvrow.FindControl("txtDiffAmountKWD"), HtmlInputText)
            txtNarr = CType(gvrow.FindControl("txtNarration"), HtmlInputText)
            ddlPostAC = CType(gvrow.FindControl("ddlPostDiffAC"), HtmlSelect)
            txtvalue = CType(gvrow.FindControl("txtvalue"), HtmlInputText)
            txtDiffAmount = CType(gvrow.FindControl("txtDiffAmount"), HtmlInputText)
            ddlPostdiffacName = CType(gvrow.FindControl("ddlPostdiffacName"), HtmlSelect)

            txtSInvVal.Value = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "providerinv_detail", "currencyamount", strCriteria)
            txtSInvValKWD.Value = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "providerinv_detail", "baseamount", strCriteria)
            txtDiffAmtKWD.Value = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "providerinv_detail", "diffamount", strCriteria)
            txtNarr.Value = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "providerinv_detail", "narration", strCriteria)
            ddlPostAC.Items(ddlPostAC.SelectedIndex).Text = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "providerinv_detail", "diffpostac", strCriteria)
            txtDiffAmount.Value = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "providerinv_detail", "isnull(invvalue,0)-isnull(currencyamount,0)", strCriteria)
            ddlPostdiffacName.Value = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "providerinv_detail", "diffpostac", strCriteria)

            '   ddlPostAC.Disabled = True
            ddlPostdiffacName.Disabled = True

            If Trim(txtDiffAmtKWD.Value) <> "" Then
                If Val(txtDiffAmtKWD.Value) <> 0 Then
                    '   ddlPostAC.Disabled = False
                    ddlPostdiffacName.Disabled = True
                End If
            End If
            'If txtSInvVal.Value <> "" And txtSInvValKWD.Value <> "" And txtDiffAmtKWD.Value <> "" And txtNarr.Value <> "" Then
            'If txtSInvVal.Value <> "" And txtSInvValKWD.Value <> "" And txtDiffAmtKWD.Value <> "" Then
            '    chkSel.Checked = True
            '    txtDiffAmount.Value = DecRound(DecRound(txtvalue.Value) - DecRound(txtSInvVal.Value))
            '    txtInvoiceTotal.Value = DecRound(DecRound(txtInvoiceTotal.Value) + DecRound(txtvalue.Value))

            '    txtTotalSInvoiceValue.Value = DecRound(DecRound(txtTotalSInvoiceValue.Value) + DecRound(txtSInvVal.Value))
            '    txtTotalSInvoiceValueKWD.Value = DecRound(DecRound(txtTotalSInvoiceValueKWD.Value) + DecRound(txtSInvValKWD.Value))

            '    txtTotalDiffAmountKWD.Value = DecRound(DecRound(txtTotalDiffAmountKWD.Value) + DecRound(txtDiffAmtKWD.Value))

            'End If

            If txtvalue.Value <> "" And txtSInvVal.Value <> "" Then
                chkSel.Checked = True
                'txtDiffAmount.Value = DecRound(DecRound(txtvalue.Value) - DecRound(txtSInvVal.Value))
                txtInvoiceTotal.Value = DecRound(DecRound(txtInvoiceTotal.Value) + DecRound(txtvalue.Value))
                txtTotalSInvoiceValue.Value = DecRound(DecRound(txtTotalSInvoiceValue.Value) + DecRound(txtSInvVal.Value))
                If txtSInvValKWD.Value <> "" Then
                    txtTotalSInvoiceValueKWD.Value = DecRound(DecRound(txtTotalSInvoiceValueKWD.Value) + DecRound(txtSInvValKWD.Value))
                End If
                If txtDiffAmtKWD.Value <> "" Then
                    txtTotalDiffAmountKWD.Value = DecRound(DecRound(txtTotalDiffAmountKWD.Value) + DecRound(txtDiffAmtKWD.Value))
                End If
            End If
        Next
    End Sub

    Private Sub FillGrid()
        Dim gvrow As GridViewRow
        Dim cntrow As Integer = 1

        Try
            If ValidateDisplay() = False Then
                Exit Sub
            End If
            gvResult.Visible = True
            Dim dsResult As New DataSet
            dsResult = doSearch()
            If dsResult.Tables.Count > 0 Then
                If dsResult.Tables(0).Rows.Count > 0 Then
                    gvResult.DataSource = dsResult.Tables(0)
                    gvResult.DataBind()
                    lblMsg.Text = ""
                    DisableControlOnDisplay("Visible")
                    EnableDisableControl("Disable")
                    gvResult.Visible = True
                Else
                    gvResult.Visible = False
                    lblMsg.Visible = True
                    lblMsg.Text = "Records not found."
                    DisableControlOnDisplay("UnVisible")
                End If
            Else
                gvResult.Visible = False
                lblMsg.Visible = True
                lblMsg.Text = "Records not found."
                DisableControlOnDisplay("UnVisible")
            End If
            txtRowCount.Value = gvResult.Rows.Count

            For Each gvrow In gvResult.Rows
                gvrow.Cells(0).Text = cntrow
                cntrow = cntrow + 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoices.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub DisableControlOnDisplay(ByVal strMode As String)
        If strMode = "Visible" Then
            divRes.Style("HEIGHT") = "300px"
            btnDeSelectAll.Visible = True
            btnSelectAll.Visible = True
            btnSave.Visible = True
            lblTotal.Visible = True
            txtTotalDiffAmountKWD.Visible = True
            txtTotalSInvoiceValue.Visible = True
        ElseIf strMode = "UnVisible" Then
            divRes.Style("HEIGHT") = "0px"
            btnDeSelectAll.Visible = False
            btnSelectAll.Visible = False
            btnSave.Visible = False
            lblTotal.Visible = False
            txtTotalDiffAmountKWD.Visible = False
            txtTotalSInvoiceValue.Visible = False
        End If
    End Sub
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        'Response.Redirect("PurchaseInvoicesSearch.aspx", False)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('PurchaseInvoiceWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'If MsgBox("Do you want to print", MsgBoxStyle.YesNo, "Doc Print") = MsgBoxResult.No Then
            '    Exit Sub
            'End If
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Do you want to print' );", True)
            ' Dim strReportTitle As String = ""
            'Dim strSelectionFormula As String = ""
            ' Session.Add("RefCode", CType(txtPInvoiceNo.Value.Trim, String))
            'Session.Add("Pageame", "PurchaseInvoiceDoc")
            'Session.Add("BackPageName", "~\AccountsModule\PurchaseInvoices.aspx")

            'strSelectionFormula = ""
            'If txtPInvoiceNo.Value.Trim <> "" Then
            '    If Trim(strSelectionFormula) = "" Then
            '        'strReportTitle = "Doc No : " & txtDocNo.Value.Trim
            '        strSelectionFormula = " {providerinv_header.tran_id}='" & txtPInvoiceNo.Value.Trim & "'"
            '    Else
            '        'strReportTitle = strReportTitle & "Doc No : " & txtDocNo.Value.Trim & "'"
            '        strSelectionFormula = strSelectionFormula & " AND {providerinv_header.tran_id}='" & txtPInvoiceNo.Value.Trim & "'"
            '    End If
            'Else
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Purchase Invoice No' );", True)
            '    Exit Sub
            'End If

            'If Trim(strSelectionFormula) = "" Then
            '    'Change 12/11/2008 *****************************
            '    'strSelectionFormula = " {providerinv_header.tran_type} = '" & txtTranType.Value & "' " & _
            '    '" and  {providerinv_header.div_id} = '" & CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
            '    strSelectionFormula = " {providerinv_header.tran_type} = '" & txtTranType.Value & "' " & _
            '    " and  {providerinv_header.div_id} = '" & CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
            '    'Change 12/11/2008 *****************************
            'Else
            '    'Change 12/11/2008 *****************************
            '    'strSelectionFormula = strSelectionFormula & " AND {providerinv_header.tran_type} = '" & txtTranType.Value & "'" & _
            '    '" and  {providerinv_header.div_id} = '" & CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
            '    strSelectionFormula = strSelectionFormula & " AND {providerinv_header.tran_type} = '" & txtTranType.Value & "'" & _
            '    " and  {providerinv_header.div_id} = '" & CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
            '    'Change 12/11/2008 *****************************
            'End If

            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)

            ''Session.Add("PrinDocTitle", "Purchase Invoice")

            'Dim ScriptStr As String
            'ScriptStr = "<script language=""javascript"">var win=window.open('../PriceListModule/PrintDocNew.aspx?Pageame=PurchaseInvoiceDoc&BackPageName=~\AccountsModule\PurchaseInvoices.aspx&PInvoiceNo=" & txtPInvoiceNo.Value & "&TranType=" & txtTranType.Value & "','printdoc','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)


            Response.Redirect("../PriceListModule/PrintDocNew.aspx?Pageame=PurchaseInvoiceDoc&BackPageName=~\AccountsModule\PurchaseInvoices.aspx&PInvoiceNo=" & txtPInvoiceNo.Value & "&TranType=" & txtTranType.Value, False)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoices.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=PurchaseInvoices','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Private Sub initialclass(ByVal con As SqlConnection, ByVal stran As SqlTransaction)
        caccounts = Nothing
        cacc = Nothing
        ctran = Nothing
        csubtran = Nothing
        caccounts = New clssave
        cacc = New clsAccounts
        cacc.clropencol()
        cacc.tran_mode = IIf(ViewState("PurchaseInvoiceState") = "New", 1, 2)
        mbasecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
        cacc.start()
    End Sub
    Private Function validate_BillAgainst() As Boolean
        Try
            validate_BillAgainst = True
            Dim Alflg As Integer
            Dim ErrMsg, strdiv As String
            'Change 12/11/2008 *****************************
            'strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511)
            strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
            'Change 12/11/2008 *****************************

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myCommand = New SqlCommand("sp_Check_AgainstBills", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtPInvoiceNo.Value
            myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = "PI"

            Dim param1 As SqlParameter
            Dim param2 As SqlParameter
            param1 = New SqlParameter
            param1.ParameterName = "@allowflg"
            param1.Direction = ParameterDirection.Output
            param1.DbType = DbType.Int16
            param1.Size = 9
            myCommand.Parameters.Add(param1)
            param2 = New SqlParameter
            param2.ParameterName = "@errmsg"
            param2.Direction = ParameterDirection.Output
            param2.DbType = DbType.String
            param2.Size = 200
            myCommand.Parameters.Add(param2)
            myDataAdapter = New SqlDataAdapter(myCommand)
            myCommand.ExecuteNonQuery()

            Alflg = param1.Value
            ErrMsg = param2.Value

            If Alflg = 1 And ErrMsg <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg & "');", True)
                validate_BillAgainst = False
                Exit Function
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PurchaseInvoices.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try
    End Function
    Private Sub CheckPostUnpostRight(ByVal UserName As String, ByVal UserPwd As String, ByVal AppName As String, ByVal PageName As String)
        Dim PostUnpostFlag As Boolean = False
        PostUnpostFlag = objUser.PostUnpostRight(Session("dbconnectionName"), UserName, UserPwd, AppName, PageName)
        If PostUnpostFlag = True Then
            chkPost.Visible = True
            lblPostmsg.Visible = True
        Else
            chkPost.Visible = False
            lblPostmsg.Visible = False
            If ViewState("PurchaseInvoiceState") = "Edit" Then
                If chkPost.Checked = True Then
                    ViewState.Add("PurchaseInvoiceState", "View")
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This transaction has been posted, you do not have rights to edit.' );", True)
                End If
            End If
        End If
    End Sub
End Class

'    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPostToCode, "postaccount", "partyname", "select distinct view_account.postaccount,partymast.partyname from view_account,partymast where view_account.postaccount=partymast.partycode and view_account.postaccount is not null AND view_account.TYPE='S'", True, ddlPostToCode.Value)
'    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPostToName, "partyname", "postaccount", "select distinct view_account.postaccount,partymast.partyname from view_account,partymast where view_account.postaccount=partymast.partycode and view_account.postaccount is not null AND view_account.TYPE='S'", True, ddlPostToName.Value)

'ElseIf ddlType.Value = "A" Then
'    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSupplierCode, "Code", "des", "select Code,des from view_account where type ='A'", True, ddlSupplierCode.Value)
'    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSpplierName, "des", "Code", "select Code,des from view_account where type ='A'", True, ddlSpplierName.Value)
'    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPostToCode, "postaccount", "supagentname", "select distinct view_account.postaccount,supplier_agents.supagentname from view_account,supplier_agents where view_account.postaccount=supplier_agents.supagentcode and view_account.postaccount is not null AND view_account.TYPE='A'", True, ddlPostToCode.Value)
'    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPostToName, "supagentname", "postaccount", "select distinct supplier_agents.supagentname,view_account.postaccount from view_account,supplier_agents where view_account.postaccount=supplier_agents.supagentcode and view_account.postaccount is not null AND view_account.TYPE='A'", True, ddlPostToName.Value)
'End If


'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPostToCode, "postaccount", "partyname", "select distinct view_account.postaccount,partymast.partyname from view_account,partymast where view_account.postaccount=partymast.partycode and view_account.postaccount is not null AND view_account.TYPE='S'", True)
'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlPostToName, "partyname", "postaccount", "select distinct view_account.postaccount,partymast.partyname from view_account,partymast where view_account.postaccount=partymast.partycode and view_account.postaccount is not null AND view_account.TYPE='S'", True)
