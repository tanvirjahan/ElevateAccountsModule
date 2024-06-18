#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
#End Region

Partial Class AccountsModule_UpdatesupplierInvoices
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
    Dim ds As DataSet
    Dim dsdata As DataSet
    'For accounts posting
#End Region
#Region "Enum parameter/GridCol"
    Enum parameter
        fromdate = 0
        todate = 1
        type = 2
        fromacct = 3
        fromcontrol = 4
    End Enum
    Enum GridCol
        SrNo = 0
        tranDate = 1
        tranid = 2
        type = 3
        requestid = 4
        invoiceno = 5
        remarks = 6
        amount = 7
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
            ViewState.Add("UpdatesupplierInvoiceState", Request.QueryString("State"))
            ViewState.Add("UpdatesupplierInvoiceRefCode", Request.QueryString("RefCode"))

            If Page.IsPostBack = False Then
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                txtconnection.Value = Session("dbconnectionName")

                FillDDL("IsPostBackFalse")
                txtPInvoiceDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy")
                txtFromDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy")
                txtToDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy")
                '         txtbasecurr.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 457)
                NumbersHtml(txtExchRate)

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

                txtxtrequestid.Attributes.Add("onchange", "changesupplier()")
                txtcprice.Attributes.Add("onchange", "calculatevalue()")
                '             divRes.Style("HEIGHT") = "0px"

                If ViewState("UpdatesupplierInvoiceState") = "New" Then
                    lblHeading.Text = "Add Updatesupplier Invoice"
                    'btnSave.Text = "Save"
                    'btnPrint.Visible = False
                    btnPrint.Visible = False
                    btnSave.Visible = False

                    DisableControlOnDisplay("UnVisible")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save')==false)return false;")
                ElseIf ViewState("UpdatesupplierInvoiceState") = "Edit" Then
                    ShowRecord(ViewState("UpdatesupplierInvoiceRefCode"))
                    ShowRecordForGrid(ViewState("UpdatesupplierInvoiceRefCode"))
                    lblHeading.Text = "Edit UpdatesupplierInvoice"
                    btnPrint.Visible = False
                    btnClear.Visible = False
                    btnDisplay.Visible = False
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update')==false)return false;")
                ElseIf ViewState("UpdatesupplierInvoiceState") = "Delete" Then
                    ShowRecord(ViewState("UpdatesupplierInvoiceRefCode"))
                    ShowRecordForGrid(ViewState("UpdatesupplierInvoiceRefCode"))
                    lblHeading.Text = "Delete Updatesupplier Invoice"

                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete')==false)return false;")
                ElseIf ViewState("UpdatesupplierInvoiceState") = "View" Then
                    ShowRecord(ViewState("UpdatesupplierInvoiceRefCode"))
                    ShowRecordForGrid(ViewState("UpdatesupplierInvoiceRefCode"))
                    lblHeading.Text = "View Updatesupplier Invoice"
                    ' DisableControl()
                End If
                DisableControl()

                btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")
                Dim typ As Type
                typ = GetType(DropDownList)
                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSupplierCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSpplierName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlControlCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlControlName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                btnPrint.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to print?')==false)return false;")
                btnSave.Attributes.Add("onclick", "return validate_click()")
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdatesupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack = True Then
                FillDDL("IsPostBackTrue")
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdatesupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Private Sub DisableControl()
        If ViewState("UpdatesupplierInvoiceState") = "New" Then
            btnSave.Text = "Save"
        ElseIf ViewState("UpdatesupplierInvoiceState") = "Edit" Then
            btnSave.Text = "Update"
        ElseIf ViewState("UpdatesupplierInvoiceState") = "View" Or ViewState("UpdatesupplierInvoiceState") = "Delete" Then
            txtPInvoiceNo.Disabled = True
            txtPInvoiceDate.Enabled = False
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
            ddlType.Disabled = True
            ddlSupplierCode.Disabled = True
            ddlSpplierName.Disabled = True
            ddlControlCode.Disabled = True
            ddlControlName.Disabled = True
            txtCurrency.Disabled = True
            txtExchRate.Disabled = True
            txtremarks.Enabled = False
            gvResult.Enabled = False
            btnDisplay.Visible = False
            btnClear.Visible = False
            If ViewState("UpdatesupplierInvoiceState") = "Delete" Then
                btnSave.Text = "Delete"
            End If
        End If
        If ViewState("UpdatesupplierInvoiceState") = "View" Then
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
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "Code", "des", "select   Code,des from view_account where type ='S' order by Code", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpplierName, "des", "Code", "select   Code,des from view_account where type ='S' order by des", True)



                End If
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlControlCode, "acctcode", "acctname", "select distinct acctcode,acctname from acctmast where isnull(controlyn,'N')='Y' and cust_supp='S'", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlControlName, "acctname", "acctcode", "select distinct acctcode,acctname from acctmast where isnull(controlyn,'N')='Y' and cust_supp='S'", True)

            ElseIf strType = "IsPostBackTrue" Then
                If ddlType.Value <> "[Select]" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierCode, "Code", "des", "select Code,des from view_account where type ='" & ddlType.Value & "' order by Code", True, ddlSupplierCode.Value)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSpplierName, "des", "Code", "select Code,des from view_account where type ='" & ddlType.Value & "' order by des", True, ddlSpplierName.Value)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdatesupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        doSearch = Nothing
        Try
            Dim count As Integer = [Enum].GetValues(GetType(parameter)).Length
            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(count) As SqlParameter


            If Not (txtFromDate.Text = "") Then
                parm(parameter.fromdate) = New SqlParameter("@fromdate", CType(ObjDateTime.ConvertDateromTextBoxToTextYearMonthDay(txtFromDate.Text), String))
            Else
                parm(parameter.fromdate) = New SqlParameter("@fromdate", "1900/01/01")
            End If
            If Not (txtToDate.Text = "") Then
                parm(parameter.todate) = New SqlParameter("@todate", CType(ObjDateTime.ConvertDateromTextBoxToTextYearMonthDay(txtToDate.Text), String))
            Else
                parm(parameter.todate) = New SqlParameter("@todate", "1900/01/01")
            End If

            parm(parameter.type) = New SqlParameter("@type", CType(ddlType.Value.Trim, String))

            If Not (ddlSupplierCode.Value = "[Select]") Then
                parm(parameter.fromacct) = New SqlParameter("@fromacct", CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text, String))
            Else
                parm(parameter.fromacct) = New SqlParameter("@fromacct", String.Empty)
            End If

            If Not (ddlControlCode.Value = "[Select]") Then
                parm(parameter.fromcontrol) = New SqlParameter("@fromcontrol", CType(ddlControlCode.Items(ddlControlCode.SelectedIndex).Text, String))
            Else
                parm(parameter.fromcontrol) = New SqlParameter("@fromcontrol", String.Empty)
            End If


            For i = 0 To count - 1
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_party_invoice", parms)
            Return ds
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdatesupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function


    Private Sub EnableDisableControl(ByVal strType As String)
        If strType = "Enable" Then
            txtPInvoiceDate.Enabled = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
            ddlType.Disabled = False
            ddlSupplierCode.Disabled = False
            ddlSpplierName.Disabled = False
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

            txtremarks.Text = ""
            txtCurrency.Value = ""
            txtExchRate.Value = ""
            DisableControlOnDisplay("UnVisible")
        ElseIf strType = "Disable" Then

            'txtPInvoiceDate.Enabled = False
            'txtSInvoiceDate.Enabled = False
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
            ddlType.Disabled = True
            ddlSupplierCode.Disabled = True
            ddlSpplierName.Disabled = True
            ddlControlCode.Disabled = True
            ddlControlName.Disabled = True

        End If

    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        EnableDisableControl("Enable")
        ddlType.Value = "S"
        ddlSupplierCode.Value = "[Select]"
        ddlSpplierName.Value = "[Select]"
        FillDDL("IsPostBackFalse")
    End Sub

    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal
        Rdamt = 2
        Return Rdamt
    End Function

    Protected Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        Try
            'If (e.Row.RowType = DataControlRowType.Header) Then
            '    e.Row.Cells(8).Text = "Supplier Invoice Value  " + "[" + txtbasecurr.Value + "]"
            '    e.Row.Cells(9).Text = "Diff Amount " + "[" + txtbasecurr.Value + "]"
            'End If
            Dim btnres As Button
            Dim invoiceno As String


            If (e.Row.RowType = DataControlRowType.DataRow) Then
                btnres = CType(e.Row.FindControl("btnreservation"), Button)
                Dim chkselect As CheckBox = CType(e.Row.FindControl("chkSelect"), CheckBox)
                invoiceno = e.Row.Cells(2).Text


              

                'If ChkAdjusted(invoiceno) = False Then
                '    btnres.Enabled = False
                'End If

                btnres.Attributes.Add("onclick", "javascript:btnclick( '" + btnres.ClientID + "','" + chkselect.ClientID + "')")

                If ViewState("UpdatesupplierInvoiceState") <> "New" Then
                    chkselect.Checked = True
                    chkselect.Enabled = False
                End If

                '    Dim txtNarr As HtmlInputText
                '    Dim chkSel As HtmlInputCheckBox
                '    Dim invoiceno As String
                '    Dim hdngroup As HiddenField

                '    Dim txtSInvVal, txtDiffAmount, txtSInvValKWD, txtDiffAmtKWD, txtSValue, txtexchrate As HtmlInputText
                '    txtNarr = CType(e.Row.FindControl("txtNarration"), HtmlInputText)
                '    chkSel = CType(e.Row.FindControl("chkPIYN"), HtmlInputCheckBox)
                '    txtSInvVal = CType(e.Row.FindControl("txtSInvoiceValue"), HtmlInputText)
                '    txtSInvValKWD = CType(e.Row.FindControl("txtSInvoiceValueKWD"), HtmlInputText)
                '    txtDiffAmtKWD = CType(e.Row.FindControl("txtDiffAmountKWD"), HtmlInputText)
                '    txtSValue = CType(e.Row.FindControl("txtvalue"), HtmlInputText)
                '    txtDiffAmount = CType(e.Row.FindControl("txtDiffAmount"), HtmlInputText)
                '    txtexchrate = CType(e.Row.FindControl("txtConvRate"), HtmlInputText)
                '    hdngroup = CType(e.Row.FindControl("hdngroup"), HiddenField)



                '    invoiceno = e.Row.Cells(1).Text

                '    NumbersDecimalRoundHtml(txtSInvVal)
                '    TextLockhtml(txtSInvValKWD)
                '    TextLockhtml(txtDiffAmtKWD)
                '    TextLockhtml(txtSValue)
                '    TextLockhtml(txtDiffAmount)

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdatesupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim sqlTrans As SqlTransaction
        Try

            Dim gvrow As GridViewRow
            Dim strdiv, particulars As String
            Dim invtype As Label
            Dim invlineno As Label

            Dim txtSInvVal, txtNarr, txtDiffAmtKWD, txtDiffAmount, txtSValue, txtSInvValKWD
            Dim ddlPostAC As HtmlSelect
            Dim acc_tranlinenno, acc_against_tranlinenno, acc_openlineno As Long
            Dim invvalue, invvaluebase, convrate As Double
            acc_tranlinenno = 1
            acc_openlineno = 100
            acc_against_tranlinenno = 500
            If Page.IsValid = True Then
                If ViewState("UpdatesupplierInvoiceState") = "New" Or ViewState("UpdatesupplierInvoiceState") = "Edit" Then
                    If ValidaPage() = False Then
                        Exit Sub
                    End If
                End If




                If ViewState("UpdatesupplierInvoiceState") = "New" Or ViewState("UpdatesupplierInvoiceState") = "Edit" Or ViewState("UpdatesupplierInvoiceState") = "Delete" Then
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


                If (ViewState("UpdatesupplierInvoiceState") = "New" Or ViewState("UpdatesupplierInvoiceState") = "Edit") Then
                    If ViewState("UpdatesupplierInvoiceState") = "New" Then
                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("SUPINVOICE", SqlConn, sqlTrans)
                        txtPInvoiceNo.Value = optionval.Trim
                        myCommand = New SqlCommand("sp_add_supplierinvoice_header", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    ElseIf ViewState("UpdatesupplierInvoiceState") = "Edit" Then
                        myCommand = New SqlCommand("sp_mod_supplierinvoice_header", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    End If

                    myCommand.Parameters.Add(New SqlParameter("@div_id ", SqlDbType.VarChar, 10)).Value = CType(strdiv, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtPInvoiceNo.Value, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "SIN"
                    myCommand.Parameters.Add(New SqlParameter("@tran_date", SqlDbType.DateTime)).Value = CType(ObjDateTime.ConvertDateromTextBoxToDatabase(txtPInvoiceDate.Text), Date)
                    myCommand.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(ddlSupplierCode.Items(ddlSupplierCode.SelectedIndex).Text, String)
                    If ddlControlCode.Value <> "[Select]" Then
                        myCommand.Parameters.Add(New SqlParameter("@glcode", SqlDbType.VarChar, 20)).Value = CType(ddlControlCode.Items(ddlControlCode.SelectedIndex).Text, String)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@glcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If
                    myCommand.Parameters.Add(New SqlParameter("@acctype", SqlDbType.VarChar, 1)).Value = CType(ddlType.Value, String)

                    myCommand.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = CType(ObjDateTime.ConvertDateromTextBoxToDatabase(txtFromDate.Text), Date)
                    myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = CType(ObjDateTime.ConvertDateromTextBoxToDatabase(txtToDate.Text), Date)
                    myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(txtCurrency.Value, String)
                    If Val(txtconvrate.Value) = 0 Then
                        myCommand.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 18)).Value = System.DBNull.Value
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 18, 12)).Value = CType(Val(txtconvrate.Value), Decimal)
                    End If
                    myCommand.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 500)).Value = CType(txtremarks.Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = ObjDateTime.GetSystemDateTime(Session("dbconnectionName"))
                    myCommand.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()



                    If ViewState("UpdatesupplierInvoiceState") = "Edit" Then
                        myCommand = New SqlCommand("sp_del_supplierinvoice_detail", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@div_id ", SqlDbType.VarChar, 10)).Value = CType(strdiv, String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtPInvoiceNo.Value, String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "SIN"
                        myCommand.ExecuteNonQuery()
                    End If



                    For Each gvrow In gvResult.Rows
                        myCommand = New SqlCommand("sp_add_supplierinvoice_detail", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        Dim lblAccTranLineno As Label = gvrow.FindControl("lblAccTranLineno")
                        Dim txtinvno As HtmlInputText = gvrow.FindControl("txtinvno")
                        Dim hdnrlinneo As HiddenField = gvrow.FindControl("hdnrlineno")
                        Dim hdnvalue As HiddenField = gvrow.FindControl("hdnvalue")
                        Dim hdnamount As HiddenField = gvrow.FindControl("hdnamount")
                        Dim chkSelect As CheckBox = gvrow.FindControl("chkSelect")

                        Dim txtDescription As TextBox = gvrow.FindControl("txtDescription")

                        If chkSelect.Checked = True Then
                            myCommand.Parameters.Add(New SqlParameter("@sno", SqlDbType.Int, 9)).Value = CType(gvrow.Cells(0).Text, Integer)
                            myCommand.Parameters.Add(New SqlParameter("@div_id ", SqlDbType.VarChar, 10)).Value = CType(strdiv, String)
                            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtPInvoiceNo.Value, String)
                            myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "SIN"
                            myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int, 9)).Value = CType(lblAccTranLineno.Text, Integer)
                            myCommand.Parameters.Add(New SqlParameter("@acc_tran_id", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(2).Text, String)
                            myCommand.Parameters.Add(New SqlParameter("@acc_tran_type", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(4).Text, String)
                            myCommand.Parameters.Add(New SqlParameter("@acc_tran_date", SqlDbType.DateTime)).Value = CType(ObjDateTime.ConvertDateromTextBoxToDatabase(gvrow.Cells(1).Text), Date)
                            myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(5).Text, String)
                            myCommand.Parameters.Add(New SqlParameter("@confno", SqlDbType.VarChar, 20)).Value = ""
                            myCommand.Parameters.Add(New SqlParameter("@supplierinvoiceno", SqlDbType.VarChar, 20)).Value = CType(txtinvno.Value, String)

                            'myCommand.Parameters.Add(New SqlParameter("@description", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(7).Text, String)
                            myCommand.Parameters.Add(New SqlParameter("@description", SqlDbType.VarChar, 500)).Value = CType(txtDescription.Text, String)

                            myCommand.Parameters.Add(New SqlParameter("@creditamount", SqlDbType.Decimal)).Value = CType(gvrow.Cells(8).Text, String)
                            If Val(hdnamount.Value) <> 0 Then
                                myCommand.Parameters.Add(New SqlParameter("@amendedcramount", SqlDbType.Decimal)).Value = CType(hdnamount.Value, Decimal)
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@amendedcramount", SqlDbType.Decimal)).Value = 0
                            End If

                            myCommand.ExecuteNonQuery()

                            If Val(hdnrlinneo.Value) <> 0 Then
                                ' Updating the log
                                myCommand = New SqlCommand("sp_reservation_savelog_new_rmtyp", SqlConn, sqlTrans)
                                myCommand.CommandType = CommandType.StoredProcedure
                                myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(5).Text, String)
                                myCommand.ExecuteNonQuery()


                                'Update the reservation
                                ''''''''time being commented for Test mode


                                myCommand = New SqlCommand("sp_revalue_suppliercost", SqlConn, sqlTrans)
                                myCommand.CommandType = CommandType.StoredProcedure
                                myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(5).Text, String)
                                myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtPInvoiceNo.Value, String)
                                myCommand.Parameters.Add(New SqlParameter("@restype", SqlDbType.VarChar, 10)).Value = CType(htnsptype.Value, String)
                                myCommand.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int, 9)).Value = CType(hdnrlinneo.Value, Integer)
                                myCommand.Parameters.Add(New SqlParameter("@acc_linneo", SqlDbType.Int, 9)).Value = CType(lblAccTranLineno.Text, Integer)
                                If gvrow.Cells(8).Text <> "" Then
                                    myCommand.Parameters.Add(New SqlParameter("@cprice", SqlDbType.Decimal)).Value = CType(gvrow.Cells(8).Text, Decimal)
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@cprice", SqlDbType.Decimal)).Value = 0
                                End If

                                If Val(hdnvalue.Value) <> 0 Then
                                    myCommand.Parameters.Add(New SqlParameter("@cvalue", SqlDbType.Decimal)).Value = CType(hdnvalue.Value, Decimal)
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@cvalue", SqlDbType.Decimal)).Value = 0
                                End If

                                myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = CType(hdndocno.Value, String)
                                myCommand.ExecuteNonQuery()

                                ''''''''''''''''''


                                'Regenerate Generate Invoice
                                If validategrid(gvrow.Cells(5).Text, gvrow.Cells(2).Text, SqlConn, sqlTrans) = False Then
                                    If SqlConn.State = ConnectionState.Open Then
                                        sqlTrans.Rollback()
                                    End If
                                    Exit Sub
                                End If

                                If txtinvno.Value <> "" Then
                                    myCommand = New SqlCommand("sp_update_supinvoice_accsubtran", SqlConn, sqlTrans)
                                    myCommand.CommandType = CommandType.StoredProcedure
                                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(strdiv, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acc_tran_id", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(2).Text, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acc_tran_type", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(4).Text, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acc_linneo", SqlDbType.Int, 9)).Value = CType(lblAccTranLineno.Text, Integer)
                                    myCommand.Parameters.Add(New SqlParameter("@supplierinvoiceno", SqlDbType.VarChar, 20)).Value = CType(txtinvno.Value, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acctype", SqlDbType.VarChar, 10)).Value = CType(ddlType.Value, String)
                                    myCommand.ExecuteNonQuery()
                                End If


                            Else
                                If txtinvno.Value <> "" Then
                                    myCommand = New SqlCommand("sp_update_supinvoice_accsubtran", SqlConn, sqlTrans)
                                    myCommand.CommandType = CommandType.StoredProcedure
                                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(strdiv, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acc_tran_id", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(2).Text, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acc_tran_type", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(4).Text, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acc_linneo", SqlDbType.Int, 9)).Value = CType(lblAccTranLineno.Text, Integer)
                                    myCommand.Parameters.Add(New SqlParameter("@supplierinvoiceno", SqlDbType.VarChar, 20)).Value = CType(txtinvno.Value, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acctype", SqlDbType.VarChar, 10)).Value = CType(ddlType.Value, String)
                                    myCommand.ExecuteNonQuery()
                                End If

                            End If

                        End If
                    Next

                    ' Updating the invoiceno no only

                    For Each gvrow In gvResult.Rows
                        Dim lblAccTranLineno As Label = gvrow.FindControl("lblAccTranLineno")
                        Dim txtinvno As HtmlInputText = gvrow.FindControl("txtinvno")
                        Dim hdnrlinneo As HiddenField = gvrow.FindControl("hdnrlineno")
                        Dim hdnvalue As HiddenField = gvrow.FindControl("hdnvalue")
                        Dim hdnamount As HiddenField = gvrow.FindControl("hdnamount")
                        Dim chkSelect As CheckBox = gvrow.FindControl("chkSelect")

                        If chkSelect.Checked = True Then
                            If Val(hdnrlinneo.Value) <> 0 Then
                                If txtinvno.Value <> "" Then
                                    myCommand = New SqlCommand("sp_update_supinvoice_accsubtran", SqlConn, sqlTrans)
                                    myCommand.CommandType = CommandType.StoredProcedure
                                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(strdiv, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acc_tran_id", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(2).Text, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acc_tran_type", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(4).Text, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acc_linneo", SqlDbType.Int, 9)).Value = CType(lblAccTranLineno.Text, Integer)
                                    myCommand.Parameters.Add(New SqlParameter("@supplierinvoiceno", SqlDbType.VarChar, 20)).Value = CType(txtinvno.Value, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acctype", SqlDbType.VarChar, 10)).Value = CType(ddlType.Value, String)
                                    myCommand.ExecuteNonQuery()
                                End If
                            Else
                                If txtinvno.Value <> "" Then
                                    myCommand = New SqlCommand("sp_update_supinvoice_accsubtran", SqlConn, sqlTrans)
                                    myCommand.CommandType = CommandType.StoredProcedure
                                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(strdiv, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acc_tran_id", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(2).Text, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acc_tran_type", SqlDbType.VarChar, 20)).Value = CType(gvrow.Cells(4).Text, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acc_linneo", SqlDbType.Int, 9)).Value = CType(lblAccTranLineno.Text, Integer)
                                    myCommand.Parameters.Add(New SqlParameter("@supplierinvoiceno", SqlDbType.VarChar, 20)).Value = CType(txtinvno.Value, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acctype", SqlDbType.VarChar, 10)).Value = CType(ddlType.Value, String)
                                    myCommand.ExecuteNonQuery()
                                End If

                            End If
                        End If
                    Next


                ElseIf ViewState("UpdatesupplierInvoiceState") = "Delete" Then

                    myCommand = New SqlCommand("sp_del_supplierinvoice", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id ", SqlDbType.VarChar, 10)).Value = CType(strdiv, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtPInvoiceNo.Value, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "SIN"
                    myCommand.ExecuteNonQuery()
                End If

                sqlTrans.Commit()                               'transaction commit
                clsDBConnect.dbSqlTransation(sqlTrans)          'close transaction  
                clsDBConnect.dbCommandClose(myCommand)          'close command
                clsDBConnect.dbConnectionClose(SqlConn)         'close connection
            End If


                 Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('PurchaseInvoiceWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

            'btnPrint.Visible = True
            '    ViewState("UpdatesupplierState") = "View"
            '    DisableControl()


        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdatesupplierInvoices.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Public Function validategrid(ByVal requestid As String, ByVal invocieno As String, ByVal SqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)

        Dim i As Integer = 0
        Dim ds As New DataSet
        Dim sqlstr As String
        Try
            validategrid = True

            sqlstr = "execute sp_validate_reservation_invoice_supplier '" & CType(requestid, String) & "' "
            ds = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), sqlstr)
            If ds IsNot Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    'display in grid
                    grdInvError.Visible = True
                    grdInvError.DataSource = ds.Tables(0)
                    grdInvError.DataBind()
                    validategrid = False
                    Exit Function
                Else
                    If invoicesave(requestid, invocieno, SqlConn, sqlTrans) = False Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Save Unsuccessful !! ');", True)
                        validategrid = False
                        Exit Function
                    End If
                End If

            End If
        Catch ex As Exception
            validategrid = False
        End Try
    End Function

    Private Function invoicesave(ByVal requestid As String, ByVal invoiceno As String, ByVal SqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction) As Boolean
        Dim mySqlCmd As SqlCommand

        invoicesave = True
        'no errors, save with procedure
        Try

            myCommand = New SqlCommand("sp_autopost_reservation_invoice", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure

            'myCommand.CommandType = CommandType.StoredProcedure
            'myCommand.CommandText = "sp_autopost_reservation_invoice"

            Dim parms As New List(Of SqlParameter)
            Dim parm(3) As SqlParameter
            parm(0) = New SqlParameter("@requestid", CType(requestid, String))
            parm(1) = New SqlParameter("@userlogged", CType(Session("GlobalUserName"), String))
            parm(2) = New SqlParameter("@invoiceno", invoiceno)

            For i = 0 To 2
                myCommand.Parameters.Add(parm(i))
            Next
            myCommand.ExecuteNonQuery()

            'temp  saving

            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandText = "sp_add_salesinvoice"
            Dim parms2 As New List(Of SqlParameter)
            Dim parm2(3) As SqlParameter

            parm2(0) = New SqlParameter("@requestid", CType(requestid, String))
            parm2(1) = New SqlParameter("@adduser", CType(Session("GlobalUserName"), String))
            parm2(2) = New SqlParameter("@invoiceno", CType(invoiceno, String))
            myCommand.Parameters.Clear()
            For i = 0 To 2
                myCommand.Parameters.Add(parm2(i))
            Next
            myCommand.ExecuteNonQuery()


        Catch ex As Exception
            invoicesave = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message & "');", True)

        End Try
    End Function
    Private Function ValidaPage() As Boolean
        Dim gvrow As GridViewRow
        Dim txtSInvVal As HtmlInputText
        Dim txtNarr As HtmlInputText
        Dim txtDiffAmtKWD As HtmlInputText
        Dim flag As Integer = 0

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

        If txtremarks.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select or enter remarks.');", True)
            SetFocus(txtremarks)
            ValidaPage = False
            Exit Function
        End If

        If gvResult.Rows.Count = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Detail cannot be blank');", True)
            ValidaPage = False
            Exit Function
        End If


        For Each gvrow In gvResult.Rows
            Dim chkSelect As CheckBox = gvrow.FindControl("chkSelect")

            If chkSelect.Checked = True Then
                flag = 1
            End If
            
        Next
        If flag = 0 Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select a detail line');", True)
            ValidaPage = False
            Exit Function
        End If

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
            strSqlQry = "select div_id,tran_id,tran_type,tran_date,partycode,glcode,fromdate,todate,currcode,convrate,remarks,adddate,adduser,moddate,moduser,acctype acc_type from supplierinvoice_header where tran_id='" & TranId & "'"
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                  'connection open
            myCommand = New SqlCommand(strSqlQry, SqlConn)
            mySqlReader = myCommand.ExecuteReader
            If mySqlReader.HasRows = True Then
                If mySqlReader.Read Then

                    If IsDBNull(mySqlReader("tran_id")) = False Then
                        txtPInvoiceNo.Value = mySqlReader("tran_id")
                        hdndocno.Value = mySqlReader("tran_id")
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


                    If IsDBNull(mySqlReader("partycode")) = False Then
                        ddlSpplierName.Value = mySqlReader("partycode")
                        ddlSupplierCode.Value = ddlSpplierName.Items(ddlSpplierName.SelectedIndex).Text
                        htnsptype.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sptypecode from partymast where partycode='" + mySqlReader("partycode") + "'")
                    Else
                        ddlSpplierName.Value = "[Select]"
                        ddlSupplierCode.Value = "[Select]"
                    End If

                    If IsDBNull(mySqlReader("glcode")) = False Then
                        ddlControlName.Value = mySqlReader("glcode")
                        ddlControlCode.Value = ddlControlName.Items(ddlControlName.SelectedIndex).Text
                    Else
                        ddlControlName.Value = "[Select]"
                        ddlControlCode.Value = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("currcode")) = False Then
                        txtCurrency.Value = mySqlReader("currcode")
                    End If
                    If IsDBNull(mySqlReader("convrate")) = False Then
                        txtExchRate.Value = mySqlReader("convrate")
                    End If
                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtremarks.Text = mySqlReader("remarks")
                        'End If
                    End If
                    If IsDBNull(mySqlReader("fromdate")) = False Then
                        txtFromDate.Text = Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy")
                    End If
                    If IsDBNull(mySqlReader("todate")) = False Then
                        txtToDate.Text = Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy")
                    End If
                    If IsDBNull(mySqlReader("convrate")) = False Then
                        txtconvrate.Value = mySqlReader("convrate")
                    End If



                End If

                'ShowRecordForGrid()


            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdatesupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try

    End Sub

    Private Sub ShowRecordForGrid(ByVal TranId As String)
        Dim gvrow As GridViewRow
        Dim strsql As String
        Dim ds As New DataSet

        'strsql = "select sno,div_id,tran_id,acc_tran_type trantype,tran_lineno acc_tran_lineno,acc_tran_id tranid,acc_tran_type,convert(varchar(10),acc_tran_date,103) trandate,requestid fileno,confno," & _
        '          " supplierinvoiceno reconfno,description particulars,creditamount credit,amendedcramount,'' restype,'" + ddlType.Value + "' acc_type,'" + ddlSpplierName.Value + "' acc_code,'" + ddlControlName.Value + "'  acc_gl_code " & _
        '          " ,'" + ddlSupplierCode.Value + "' accname ,'' arrdate,'' depdate,1 mode,0 debit,supplierinvoiceno otherref  from supplierinvoice_detail where tran_id='" & TranId & "' order by sno"


        ''''Added shahul 20/03/16
        strsql = "select supplierinvoice_detail.sno,div_id,tran_id,acc_tran_type trantype,tran_lineno acc_tran_lineno,acc_tran_id tranid,acc_tran_type,convert(varchar(10),acc_tran_date,103) trandate,supplierinvoice_detail.requestid fileno,confno," & _
                " supplierinvoiceno reconfno,description particulars,creditamount credit,amendedcramount,'' restype,'" + ddlType.Value + "' acc_type,'" + ddlSpplierName.Value + "' acc_code,'" + ddlControlName.Value + "'  acc_gl_code " & _
                " ,'" + ddlSupplierCode.Value + "' accname ,'' arrdate,'' depdate,1 mode,0 debit,supplierinvoiceno otherref,a.agentname  from supplierinvoice_detail   left join reservation_invoice_header h on  h.requestid =supplierinvoice_detail.requestid  left join agentmast a on h.agentcode =a.agentcode  " & _
                " where tran_id='" & TranId & "' order by supplierinvoice_detail.sno"


        ds = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strsql)

        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                gvResult.DataSource = ds.Tables(0)
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




        'For Each gvrow In gvResult.Rows
        '    invtype = gvrow.FindControl("lblAccTranType")
        '    invlineno = gvrow.FindControl("lblAccTranLineno")
        '    strCriteria = " tran_id='" & txtPInvoiceNo.Value & "' and invno='" & gvrow.Cells(1).Text & "' and invtype='" & invtype.Text & "' and invlineno='" & invlineno.Text & "'"

        '    'txtSValue = CType(gvrow.FindControl("txtvalue"), HtmlInputText)
        '    chkSel = CType(gvrow.FindControl("chkPIYN"), HtmlInputCheckBox)
        '    txtSInvVal = CType(gvrow.FindControl("txtSInvoiceValue"), HtmlInputText)
        '    txtSInvValKWD = CType(gvrow.FindControl("txtSInvoiceValueKWD"), HtmlInputText)
        '    txtDiffAmtKWD = CType(gvrow.FindControl("txtDiffAmountKWD"), HtmlInputText)
        '    txtNarr = CType(gvrow.FindControl("txtNarration"), HtmlInputText)
        '    txtvalue = CType(gvrow.FindControl("txtvalue"), HtmlInputText)
        '    txtDiffAmount = CType(gvrow.FindControl("txtDiffAmount"), HtmlInputText)

        '    txtSInvVal.Value = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "providerinv_detail", "currencyamount", strCriteria)
        '    txtSInvValKWD.Value = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "providerinv_detail", "baseamount", strCriteria)
        '    txtDiffAmtKWD.Value = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "providerinv_detail", "diffamount", strCriteria)
        '    txtNarr.Value = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "providerinv_detail", "narration", strCriteria)
        '    txtDiffAmount.Value = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "providerinv_detail", "isnull(invvalue,0)-isnull(currencyamount,0)", strCriteria)

        'Next
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
                    'btnPrint.Visible = True
                    btnSave.Visible = True
                    btnDisplay.Enabled = False
                    hdndocno.Value = CType((objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_new_sin_temp")).Tables(0).Rows(0)(0), String)


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
            '    txtRowCount.Value = gvResult.Rows.Count

            For Each gvrow In gvResult.Rows
                gvrow.Cells(0).Text = cntrow
                cntrow = cntrow + 1
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdatesupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub DisableControlOnDisplay(ByVal strMode As String)
        If strMode = "Visible" Then
            '   divRes.Style("HEIGHT") = "300px"
            btnSave.Visible = True
        ElseIf strMode = "UnVisible" Then
            '   divRes.Style("HEIGHT") = "0px"
            btnSave.Visible = False
        End If
    End Sub
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('PurchaseInvoiceWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Response.Redirect("../PriceListModule/PrintDocNew.aspx?Pageame=UpdatesupplierinvoiceDoc&BackPageName=~\AccountsModule\UpdatesupplierInvoice.aspx&PInvoiceNo=" & txtPInvoiceNo.Value & "&TranType=SIN", False)


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdatesupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=UpdatesupplierInvoice','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Private Sub initialclass(ByVal con As SqlConnection, ByVal stran As SqlTransaction)
        caccounts = Nothing
        cacc = Nothing
        ctran = Nothing
        csubtran = Nothing
        caccounts = New clssave
        cacc = New clsAccounts
        cacc.clropencol()
        cacc.tran_mode = IIf(ViewState("UpdatesupplierInvoiceState") = "New", 1, 2)
        mbasecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
        cacc.start()
    End Sub
    Protected Sub btnreservation_Click(ByVal sender As Object, ByVal e As System.EventArgs)


        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex

        Dim strpop As String = ""
        Dim parms As New List(Of SqlParameter)
        Dim i As Integer
        Dim parm(6) As SqlParameter
        Dim ds As DataSet

        Dim requestid As String
        Dim invoiceno As String
        Dim partycode As String
        Dim acc_tran_lineno As String
        Dim trantype As String
        Dim acctype As String
        Dim ghdnrlineno As HiddenField
        Dim sqlTrans As SqlTransaction
        Dim chkselect As CheckBox

        requestid = gvResult.DataKeys(rowid).Values("fileno").ToString()
        invoiceno = gvResult.DataKeys(rowid).Values("tranid").ToString()
        partycode = gvResult.DataKeys(rowid).Values("acc_code").ToString()
        acc_tran_lineno = gvResult.DataKeys(rowid).Values("acc_tran_lineno").ToString()
        trantype = gvResult.DataKeys(rowid).Values("trantype").ToString()
        acctype = gvResult.DataKeys(rowid).Values("acc_type").ToString()
        ghdnrlineno = gvResult.Rows(rowid).FindControl("hdnrlineno")
        chkselect = gvResult.Rows(rowid).FindControl("chkselect")

        'To avoid adjusted validation Message this chkadjusted function has been commented By Riswan 18/03/2015
        Dim strmsg As String = "" 'ChkAdjusted(invoiceno)
        If strmsg <> "" Then
            ' btnres.Enabled = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strmsg & "');", True)

            Exit Sub
        End If

        If chkselect.Checked = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select the line' );", True)
            Exit Sub
        End If

        If requestid <> "" Then
            parm(0) = New SqlParameter("@requestid", CType(requestid, String))
        Else
            parm(0) = New SqlParameter("@requestid", String.Empty)
        End If
        If invoiceno <> "" Then
            parm(1) = New SqlParameter("@invoiceno", CType(invoiceno, String))
        Else
            parm(1) = New SqlParameter("@invoiceno", String.Empty)
        End If
        If partycode <> "" Then
            parm(2) = New SqlParameter("@partycode", CType(partycode, String))
        Else
            parm(2) = New SqlParameter("@partycode", String.Empty)
        End If
        If acc_tran_lineno <> "" Then
            parm(3) = New SqlParameter("@acc_tran_lineno", CType(acc_tran_lineno, String))
        Else
            parm(3) = New SqlParameter("@acc_tran_lineno", String.Empty)

        End If
        If trantype <> "" Then
            parm(4) = New SqlParameter("@trantype", CType(trantype, String))
        Else
            parm(4) = New SqlParameter("@trantype", String.Empty)

        End If
        If acctype <> "" Then
            parm(5) = New SqlParameter("@acctype", CType(acctype, String))
        Else
            parm(5) = New SqlParameter("@acctype", String.Empty)

        End If

        For i = 0 To 5
            parms.Add(parm(i))
        Next



        If htnsptype.Value = "HOT" Then
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_hotel_cost_detail", parms)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then

                    Try
                        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))         'connection open
                        sqlTrans = SqlConn.BeginTransaction         'transaction start

                        myCommand = New SqlCommand("sp_del_reservation_supplier_invoicetemp", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(ds.Tables(0).Rows(0)("requestid"), String)
                        myCommand.Parameters.Add(New SqlParameter("@rlinneo", SqlDbType.Int, 9)).Value = CType(ds.Tables(0).Rows(0)("rlineno"), Integer)
                        myCommand.ExecuteNonQuery()

                        sqlTrans.Commit()                               'transaction commit
                        clsDBConnect.dbSqlTransation(sqlTrans)          'close transaction  
                        clsDBConnect.dbCommandClose(myCommand)          'close command
                        clsDBConnect.dbConnectionClose(SqlConn)         'close connection

                    Catch ex As Exception
                        If SqlConn.State = ConnectionState.Open Then
                            sqlTrans.Rollback()
                        End If
                    End Try


                    dsdata = GetData(ds.Tables(0).Rows(0)("requestid"), ds.Tables(0).Rows(0)("rlineno"), ds.Tables(0).Rows(0)("supagentcode"), ds.Tables(0).Rows(0)("partycode"), ghdnrlineno.Value)
                    If dsdata.Tables.Count > 0 Then

                        hdnrequestid.Value = requestid
                        hdnrlineno.Value = ds.Tables(0).Rows(0)("rlineno")
                        hdnrowid1.Value = rowid
                        hdnpartycode.Value = ds.Tables(0).Rows(0)("partycode")
                        hdnsupagentcode1.Value = ds.Tables(0).Rows(0)("supagentcode")
                        ghdnrlineno1.Value = ghdnrlineno.Value

                        ModalPopuphoteldetail.Show()
                        gvRoomDetails.DataSource = dsdata.Tables(0)
                        gvRoomDetails.DataBind()
                    End If
                End If
            End If
        Else
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_transfer_cost_detail", parms)
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    Dim hdnvalue As HiddenField
                    hdnvalue = gvResult.Rows(rowid).FindControl("hdnvalue")
                    Dim cprice As Decimal
                    If Val(hdnvalue.Value) <> 0 Then
                        cprice = CType(gvResult.Rows(rowid).Cells(8).Text, Decimal)
                    Else
                        cprice = 0
                    End If

                    dsdata = GetTransferData(ds.Tables(0).Rows(0)("requestid"), ds.Tables(0).Rows(0)("rlineno"), ds.Tables(0).Rows(0)("supagentcode"), ds.Tables(0).Rows(0)("partycode"), cprice)
                    If dsdata.Tables.Count > 0 Then
                        If dsdata.Tables(0).Rows.Count > 0 Then
                            ModalPopuphoteltransfer.Show()
                            showtransfer(rowid, ds.Tables(0).Rows(0)("rlineno"))
                        End If
                    End If
                End If
            End If
        End If


        'If ds.Tables(0).Rows.Count > 0 Then
        '    strpop = "window.open('Reservation_costamend.aspx?requestid=" & ds.Tables(0).Rows(0)("requestid") & "&rlineno=" & ds.Tables(0).Rows(0)("rlineno") & "&supagentcode=" & ds.Tables(0).Rows(0)("supagentcode") & _
        '         "&partycode=" & ds.Tables(0).Rows(0)("partycode") & "&datein=" & ds.Tables(0).Rows(0)("datein") & "&dateout=" & ds.Tables(0).Rows(0)("dateout") & "&rmtypcode=" & ds.Tables(0).Rows(0)("rmtypcode") & _
        '         "&mealcode=" & ds.Tables(0).Rows(0)("mealcode") & "&norooms=" & ds.Tables(0).Rows(0)("norooms") & "&hotellineno=" & ds.Tables(0).Rows(0)("hotellineno") & " ','Reservationfreeform','width=900,height=600 left=0,top=0 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "UpdatesupplierInvoices", strpop, True)
        'Else
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No record found')", True)
        'End If
    End Sub

#Region "Public Function GetData()"
    Public Function GetData(ByVal requestid As String, ByVal rlineno As String, ByVal supagentcode As String, ByVal partycode As String, ByVal grlinneo As String) As DataSet
        Dim parms As New List(Of SqlParameter)
        Dim i As Integer
        Dim parm(5) As SqlParameter

        If requestid <> "" Then
            parm(0) = New SqlParameter("@requestid", CType(requestid, String))
        Else
            parm(0) = New SqlParameter("@requestid", String.Empty)
        End If
        If rlineno <> "" Then
            parm(1) = New SqlParameter("@rlineno", CType(rlineno, String))
        Else
            parm(1) = New SqlParameter("@rlineno", String.Empty)
        End If
        If partycode <> "" Then
            parm(2) = New SqlParameter("@partycode", CType(partycode, String))
        Else
            parm(2) = New SqlParameter("@partycode", String.Empty)
        End If
        If supagentcode <> "" Then
            parm(3) = New SqlParameter("@supagentcode", CType(supagentcode, String))
        Else
            parm(3) = New SqlParameter("@supagentcode", String.Empty)

        End If
        If CType(grlinneo, String) <> "" Then
            parm(4) = New SqlParameter("@glinneo", grlinneo)
        Else
            parm(4) = New SqlParameter("@glinneo", String.Empty)

        End If

        For i = 0 To 4
            parms.Add(parm(i))
        Next
        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_amendcost_price", parms)
        Return ds
    End Function
#End Region

#Region "Public Function GetTransferData()"
    Public Function GetTransferData(ByVal requestid As String, ByVal rlineno As String, ByVal supagentcode As String, ByVal partycode As String, ByVal cprice As Decimal) As DataSet
        Dim parms As New List(Of SqlParameter)
        Dim i As Integer
        Dim parm(5) As SqlParameter

        If requestid <> "" Then
            parm(0) = New SqlParameter("@requestid", CType(requestid, String))
        Else
            parm(0) = New SqlParameter("@requestid", String.Empty)
        End If
        If rlineno <> "" Then
            parm(1) = New SqlParameter("@rlineno", CType(rlineno, String))
        Else
            parm(1) = New SqlParameter("@rlineno", String.Empty)
        End If
        If partycode <> "" Then
            parm(2) = New SqlParameter("@partycode", CType(partycode, String))
        Else
            parm(2) = New SqlParameter("@partycode", String.Empty)
        End If
        If supagentcode <> "" Then
            parm(3) = New SqlParameter("@supagentcode", CType(supagentcode, String))
        Else
            parm(3) = New SqlParameter("@supagentcode", String.Empty)
        End If

        If cprice <> 0 Then
            parm(4) = New SqlParameter("@cprice", CType(cprice, Decimal))
        Else
            parm(4) = New SqlParameter("@cprice", 0)
        End If

        For i = 0 To 4
            parms.Add(parm(i))
        Next
        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_amendcost_transferprice", parms)
        Return ds
    End Function
#End Region

#Region "Public Function RecalculateData()"
    Public Function RecalculateData(ByVal requestid As String, ByVal rlineno As String, ByVal supagentcode As String, ByVal partycode As String, ByVal grlinneo As String, ByVal cprice As Decimal, ByVal slineno As Integer) As DataSet
        Dim parms As New List(Of SqlParameter)
        Dim i As Integer
        Dim parm(7) As SqlParameter

        If requestid <> "" Then
            parm(0) = New SqlParameter("@requestid", CType(requestid, String))
        Else
            parm(0) = New SqlParameter("@requestid", String.Empty)
        End If
        If rlineno <> "" Then
            parm(1) = New SqlParameter("@rlineno", CType(rlineno, String))
        Else
            parm(1) = New SqlParameter("@rlineno", String.Empty)
        End If
        If partycode <> "" Then
            parm(2) = New SqlParameter("@partycode", CType(partycode, String))
        Else
            parm(2) = New SqlParameter("@partycode", String.Empty)
        End If
        If supagentcode <> "" Then
            parm(3) = New SqlParameter("@supagentcode", CType(supagentcode, String))
        Else
            parm(3) = New SqlParameter("@supagentcode", String.Empty)

        End If
        If CType(grlinneo, String) <> "" Then
            parm(4) = New SqlParameter("@glinneo", grlinneo)
        Else
            parm(4) = New SqlParameter("@glinneo", String.Empty)

        End If

        If CType(cprice, Decimal) <> 0 Then
            parm(5) = New SqlParameter("@price", cprice)
        Else
            parm(5) = New SqlParameter("@price", String.Empty)
        End If

        If CType(slineno, Integer) <> 0 Then
            parm(6) = New SqlParameter("@slineno", slineno)
        Else
            parm(6) = New SqlParameter("@slineno", String.Empty)
        End If

        For i = 0 To 6
            parms.Add(parm(i))
        Next
        ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_recalculatecost_price", parms)
        Return ds
    End Function
#End Region

#Region "Public Function showtransfer(rowid as integer,rlinneo as integer)"
    Public Function showtransfer(ByVal rowid As Integer, ByVal rlineno As Integer)
        If dsdata.Tables.Count > 0 Then
            If IsDBNull(dsdata.Tables(0).Rows(0)("tdate")) = False Then
                txtDate.Text = dsdata.Tables(0).Rows(0)("tdate")
            End If
            If IsDBNull(dsdata.Tables(0).Rows(0)("othtypname")) = False Then
                txtroutes.Value = dsdata.Tables(0).Rows(0)("othtypname")
            End If
            If IsDBNull(dsdata.Tables(0).Rows(0)("othcatname")) = False Then
                txtvehicle.Value = dsdata.Tables(0).Rows(0)("othcatname")
            End If
            If IsDBNull(dsdata.Tables(0).Rows(0)("units")) = False Then
                txtUnits.Value = dsdata.Tables(0).Rows(0)("units")
            End If
            If IsDBNull(dsdata.Tables(0).Rows(0)("price")) = False Then
                txtprice.Value = dsdata.Tables(0).Rows(0)("price")
                lblsprice.Text = dsdata.Tables(0).Rows(0)("scurrcode")
            End If
            If IsDBNull(dsdata.Tables(0).Rows(0)("Value")) = False Then
                txtValue.Value = dsdata.Tables(0).Rows(0)("Value")
                lblsvalue.Text = dsdata.Tables(0).Rows(0)("scurrcode")
            End If
            If IsDBNull(dsdata.Tables(0).Rows(0)("cprice")) = False Then
                txtcprice.Value = dsdata.Tables(0).Rows(0)("cprice")
                lblcprice.Text = IIf(IsDBNull(dsdata.Tables(0).Rows(0)("currcode")) = True, 1, dsdata.Tables(0).Rows(0)("currcode"))
            End If
            If IsDBNull(dsdata.Tables(0).Rows(0)("costvalue")) = False Then
                txtcValue.Value = dsdata.Tables(0).Rows(0)("costvalue")
                lblcvalue.Text = IIf(IsDBNull(dsdata.Tables(0).Rows(0)("currcode")) = True, 1, dsdata.Tables(0).Rows(0)("currcode"))
            End If

            hdnolineno.Value = rlineno
            hdnrowid.Value = rowid

            End If

    End Function
#End Region

    Protected Sub gvRoomDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRoomDetails.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim lblroomtype As Label
            Dim lblrtypeName As Label
            Dim lblMCode As Label
            Dim lblCat As Label
            Dim lblsval As Label
            Dim lblcvalue As Label
            Dim lblNoroom As Label

            lblsval = CType(e.Row.FindControl("lblsval"), Label)
            lblcvalue = CType(e.Row.FindControl("lblcvalue"), Label)
            lblroomtype = CType(e.Row.FindControl("lblroomtype"), Label)
            lblrtypeName = CType(e.Row.FindControl("lblrmtypname"), Label)
            lblMCode = CType(e.Row.FindControl("lblMCode"), Label)
            lblCat = CType(e.Row.FindControl("lblCat"), Label)
            lblNoroom = CType(e.Row.FindControl("lblNoroom"), Label)

            Dim gv As GridView
            gv = CType(e.Row.FindControl("gvPrice"), GridView)
            If dsdata.Tables.Count > 1 Then
                dsdata.Tables(1).DefaultView.RowFilter = "rmtypcode='" + lblroomtype.Text + "'" + " and mealcode='" + lblMCode.Text + "'" + " and rmcatcode='" + lblCat.Text + "'"
                gv.DataSource = dsdata.Tables(1).DefaultView.ToTable()
                gv.DataBind()
            End If
            Dim gvrow As GridViewRow
            For Each gvrow In gv.Rows
                Dim txtcprice As TextBox = CType(gvrow.FindControl("txtcostpricenight"), TextBox)
            Next
        End If
    End Sub

    Protected Sub gvPrice_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim txtcostpricenight As TextBox
            Dim lblnights As Label
            Dim lblCostvalue As Label
            Dim btncostclick As Button
            txtcostpricenight = e.Row.FindControl("txtcostpricenight")
            lblnights = e.Row.FindControl("lblnights")
            lblCostvalue = e.Row.FindControl("lblCostvalue")

            btncostclick = e.Row.FindControl("btncostclick")

            txtcostpricenight.Attributes.Add("onchange", "javascript:Changecost('" + CType(txtcostpricenight.ClientID, String) + "','" + CType(lblCostvalue.ClientID, String) + "','" + CType(lblnights.ClientID, String) + "','" + CType(btncostclick.ClientID, String) + "')")
            btncostclick.Attributes.Add("onclick", "Javascript:btnclick('" + CType(btncostclick.ClientID, String) + "')")
        End If
    End Sub

    Protected Sub btncostclick_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ModalPopuphoteldetail.Show()

        Dim objbtn As Button = CType(sender, Button)
        Dim rowid As Integer = 0
        Dim row As GridViewRow
        row = CType(objbtn.NamingContainer, GridViewRow)
        rowid = row.RowIndex
        Dim gvrow As GridViewRow
        Dim gvchild As GridViewRow
        Dim costtotal As Decimal
        Dim lblcostvalue1 As Label
        Dim lblnoroom As Label
        Dim i As Integer
        Dim ds1 As DataSet
        costtotal = 0

        'lblcostvalue1 = gvRoomDetails.Rows(rowid).FindControl("lblCostvalue")
        'lblnoroom = gvRoomDetails.Rows(rowid).FindControl("lblnoroom")
        'gvchild = gvRoomDetails.Rows(rowid).FindControl("gvPrice")

        'If Not gvchild Is Nothing Then
        '    For i = 0 To gvchild.Rows.Count - 1
        '    Next
        'End If

        'costtotal = 0
        'lblcostvalue1 = gvRoomDetails.Rows(0).FindControl("lblCostvalue")
        'lblnoroom = gvRoomDetails.Rows(0).FindControl("lblnoroom")
        Try

            btnCostSave_Click()

            For Each gvrow In gvRoomDetails.Rows
                Dim gvchildview As GridView = CType(gvrow.FindControl("gvPrice"), GridView)
                For Each gvchild In gvchildview.Rows

                    Dim txtcostpricenight As TextBox = gvchild.FindControl("txtcostpricenight")
                    Dim lblslineno As Label = gvchild.FindControl("lblslineno")

                    dsdata = RecalculateData(hdnrequestid.Value, hdnrlineno.Value, hdnsupagentcode1.Value, hdnpartycode.Value, ghdnrlineno1.Value, Val(txtcostpricenight.Text), lblslineno.Text)
                    If dsdata.Tables.Count > 0 Then
                        gvRoomDetails.DataSource = dsdata.Tables(0)
                        gvRoomDetails.DataBind()
                    End If


                    'Dim lblCostvalue As Label = gvchild.FindControl("lblCostvalue")
                    'Dim txtcostpricenight As TextBox = gvchild.FindControl("txtcostpricenight")
                    'Dim lblnights As Label = gvchild.FindControl("lblnights")
                    'lblCostvalue.Text = CType(Val(txtcostpricenight.Text) * Val(lblnights.Text) * Val(lblnoroom.Text), Double).ToString
                    'costtotal = costtotal + (Val(txtcostpricenight.Text) * Val(lblnights.Text) * Val(lblnoroom.Text))
                Next
            Next

        Catch ex As Exception

        End Try

        'lblcostvalue1.Text = CType(costtotal, Double).ToString
    End Sub

    Protected Sub btnMoreClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMoreClose.Click
        gvRoomDetails.DataSource = Nothing
        gvRoomDetails.DataBind()
        '   grdguestbind(1)

        ModalPopuphoteldetail.Hide()
    End Sub

    Protected Sub btntransfersave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btntransfersave.Click
        Dim gvrow As GridViewRow
        Dim hdnvalue As HiddenField
        Dim hdnrlineno As HiddenField
        Dim hdnamount As HiddenField
        For Each gvrow In gvResult.Rows
            hdnvalue = gvrow.FindControl("hdnvalue")
            hdnrlineno = gvrow.FindControl("hdnrlineno")
            hdnamount = gvrow.FindControl("hdnamount")
            If gvrow.RowIndex = hdnrowid.Value Then
                gvrow.Cells(8).Text = CType(txtcprice.Value, Double).ToString
                hdnvalue.Value = txtcValue.Value
                hdnrlineno.Value = hdnolineno.Value
                '  hdnamount.Value = CType(txtcprice.Value, Double).ToString
            End If
        Next
    End Sub

    Protected Sub btnCostSave_Click()
        Dim sqlTrans As SqlTransaction
        Try
            If validate_hotels() = False Then
                Exit Sub
            End If

            Dim gvrow As GridViewRow
            Dim gvrow1 As GridViewRow
            Dim gvchild As GridView
            Dim txtcostpricenight As TextBox
            Dim cvalue As Decimal
            Dim lblcostvalue As Label
            Dim lblroomtype As Label
            Dim lblMCode As Label
            Dim lblCat As Label
            Dim lblCostvalue1 As Label
            Dim lblslineno As Label
            cvalue = 0

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))         'connection open
            sqlTrans = SqlConn.BeginTransaction         'transaction start

            myCommand = New SqlCommand("sp_del_reservation_supplier_invoicetemp", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(hdnrequestid.Value, String)
            myCommand.Parameters.Add(New SqlParameter("@rlinneo", SqlDbType.Int, 9)).Value = CType(hdnrlineno.Value, Integer)
            myCommand.ExecuteNonQuery()

            For Each gvrow In gvRoomDetails.Rows
                gvchild = CType(gvrow.FindControl("gvprice"), GridView)
                lblroomtype = gvrow.FindControl("lblroomtype")
                lblMCode = gvrow.FindControl("lblMCode")
                lblCat = gvrow.FindControl("lblCat")

                lblcostvalue = gvrow.FindControl("lblcostvalue")
                cvalue = lblcostvalue.Text
                For Each gvrow1 In gvchild.Rows
                    txtcostpricenight = gvrow1.FindControl("txtcostpricenight")
                    lblCostvalue1 = gvrow1.FindControl("lblCostvalue")
                    lblslineno = gvrow1.FindControl("lblslineno")

                    myCommand = New SqlCommand("sp_reservation_supplier_invoicetemp", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@invoiceno", SqlDbType.VarChar, 20)).Value = CType(hdndocno.Value, String)
                    myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(hdnrequestid.Value, String)
                    myCommand.Parameters.Add(New SqlParameter("@rlinneo", SqlDbType.Int, 9)).Value = CType(hdnrlineno.Value, Integer)
                    myCommand.Parameters.Add(New SqlParameter("@slineno", SqlDbType.Int, 9)).Value = CType(lblslineno.Text, Integer)
                    myCommand.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(lblroomtype.Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(lblCat.Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(lblMCode.Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@cprice", SqlDbType.Decimal)).Value = CType(txtcostpricenight.Text, Decimal)
                    myCommand.Parameters.Add(New SqlParameter("@cvalue", SqlDbType.Decimal)).Value = CType(lblCostvalue1.Text, Decimal)
                    myCommand.ExecuteNonQuery()
                Next
            Next

            sqlTrans.Commit()                               'transaction commit
            clsDBConnect.dbSqlTransation(sqlTrans)          'close transaction  
            clsDBConnect.dbCommandClose(myCommand)          'close command
            clsDBConnect.dbConnectionClose(SqlConn)         'close connection
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdatesupplierInvoices.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub


    Protected Sub btnMoreSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMoreSave.Click
        Dim sqlTrans As SqlTransaction
        Try
            If validate_hotels() = False Then
                Exit Sub
            End If

            Dim gvrow As GridViewRow
            Dim gvrow1 As GridViewRow
            Dim gvchild As GridView
            Dim txtcostpricenight As TextBox
            Dim cvalue As Decimal
            Dim lblcostvalue As Label
            Dim lblroomtype As Label
            Dim lblMCode As Label
            Dim lblCat As Label
            Dim lblCostvalue1 As Label
            Dim lblslineno As Label
            cvalue = 0



            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))         'connection open
            sqlTrans = SqlConn.BeginTransaction         'transaction start

            myCommand = New SqlCommand("sp_del_reservation_supplier_invoicetemp", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(hdnrequestid.Value, String)
            myCommand.Parameters.Add(New SqlParameter("@rlinneo", SqlDbType.Int, 9)).Value = CType(hdnrlineno.Value, Integer)
            myCommand.ExecuteNonQuery()

            myCommand = New SqlCommand("sp_del_reservation_supplier_matchinvoicetemp", SqlConn, sqlTrans)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(hdnrequestid.Value, String)
            myCommand.Parameters.Add(New SqlParameter("@rlinneo", SqlDbType.Int, 9)).Value = CType(hdnrlineno.Value, Integer)
            myCommand.ExecuteNonQuery()

            For Each gvrow In gvRoomDetails.Rows
                gvchild = CType(gvrow.FindControl("gvprice"), GridView)
                lblroomtype = gvrow.FindControl("lblroomtype")
                lblMCode = gvrow.FindControl("lblMCode")
                lblCat = gvrow.FindControl("lblCat")

                lblcostvalue = gvrow.FindControl("lblcostvalue")

                For Each gvrow1 In gvchild.Rows
                    txtcostpricenight = gvrow1.FindControl("txtcostpricenight")
                    lblCostvalue1 = gvrow1.FindControl("lblCostvalue")
                    lblslineno = gvrow1.FindControl("lblslineno")
                    cvalue = cvalue + Val(lblCostvalue1.Text)

                    myCommand = New SqlCommand("sp_reservation_supplier_matchinvoicetemp", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@invoiceno", SqlDbType.VarChar, 20)).Value = CType(hdndocno.Value, String)
                    myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(hdnrequestid.Value, String)
                    myCommand.Parameters.Add(New SqlParameter("@rlinneo", SqlDbType.Int, 9)).Value = CType(hdnrlineno.Value, Integer)
                    myCommand.Parameters.Add(New SqlParameter("@slineno", SqlDbType.Int, 9)).Value = CType(lblslineno.Text, Integer)
                    myCommand.Parameters.Add(New SqlParameter("@rmtypcode", SqlDbType.VarChar, 20)).Value = CType(lblroomtype.Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(lblCat.Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(lblMCode.Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@cprice", SqlDbType.Decimal)).Value = CType(txtcostpricenight.Text, Decimal)
                    myCommand.Parameters.Add(New SqlParameter("@cvalue", SqlDbType.Decimal)).Value = CType(lblCostvalue1.Text, Decimal)
                    myCommand.ExecuteNonQuery()


                Next
            Next

            sqlTrans.Commit()                               'transaction commit
            clsDBConnect.dbSqlTransation(sqlTrans)          'close transaction  
            clsDBConnect.dbCommandClose(myCommand)          'close command
            clsDBConnect.dbConnectionClose(SqlConn)         'close connection

            Dim hdnvalue As HiddenField
            Dim hdnrlineno1 As HiddenField
            Dim hdncprice As HiddenField
            For Each gvrow In gvResult.Rows
                hdnvalue = gvrow.FindControl("hdnvalue")
                hdnrlineno1 = gvrow.FindControl("hdnrlineno")
                hdncprice = gvrow.FindControl("hdncprice")
                If gvrow.RowIndex = hdnrowid1.Value Then
                    gvrow.Cells(8).Text = CType(cvalue, Double).ToString
                    hdncprice.Value = CType(cvalue, Double).ToString
                    'hdnvalue.Value = txtcValue.Value
                    hdnrlineno1.Value = hdnrlineno.Value
                End If
            Next

        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdatesupplierInvoices.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

    Private Function validate_hotels() As Boolean
        Try
            validate_hotels = True
            Dim gvrow As GridViewRow
            Dim gvrow1 As GridViewRow
            Dim gvprice_chld As GridView
            Dim txtcostpricenight As TextBox
            For Each gvrow In gvRoomDetails.Rows
                gvprice_chld = CType(gvrow.FindControl("gvprice"), GridView)

                For Each gvrow1 In gvprice_chld.Rows
                    txtcostpricenight = gvrow1.FindControl("txtcostpricenight")
                    If txtcostpricenight.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cost cannot be blank' );", True)
                        validate_hotels = False
                        Exit For
                    End If
                Next
            Next

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("UpdatesupplierInvoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

    Private Function ChkAdjusted(ByVal invoiceno As String) As String
        ChkAdjusted = True
        myCommand = New SqlCommand
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        myCommand.Connection = SqlConn

        Dim parms As New List(Of SqlParameter)
        Dim parm(3) As SqlParameter
        myCommand.CommandType = CommandType.StoredProcedure

        myCommand.CommandText = "sp_check_adjusted"
        parm(0) = New SqlParameter("@acc_tran_id", CType(invoiceno, String))
        parm(1) = New SqlParameter("@acc_tran_type", "IN")
        parm(2) = New SqlParameter("@errmessage", SqlDbType.VarChar, 1000)
        parm(2).Direction = ParameterDirection.Output
        parm(2).Value = ""
        myCommand.Parameters.Clear()
        For i = 0 To 2
            myCommand.Parameters.Add(parm(i))
        Next
        myCommand.ExecuteNonQuery()

        Dim strError As String = ""
        strError = parm(2).Value.ToString()
        If strError = "" Then

            ChkAdjusted = ""
        Else
            strError = strError.Remove(strError.Length - 1, 1)
            ChkAdjusted = strError
            'ChkAdjusted = False
        End If
    End Function
End Class
