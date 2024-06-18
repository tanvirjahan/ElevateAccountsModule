'------------================--------------=======================------------------================
'   Module Name    :    Freeform_Invoice.aspx
'   Developer Name :    sharfudeen
'   Date           :    
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
#End Region
Partial Class Freeform_invoice
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objdatetime As New clsDateTime
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim ObjDate As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlConn As SqlConnection
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
    Dim gvRow As GridViewRow
    Dim chckDeletion As CheckBox
    'For accounts posting
    Dim caccounts As clssave = Nothing
    Dim cacc As clsAccounts = Nothing
    Dim ctran As clstran = Nothing
    Dim csubtran As clsSubTran = Nothing
    Dim mbasecurrency As String = ""
    Dim dt As DataTable
    'For accounts posting
#End Region
#Region "Enum GridCol"
    Enum GridCol
        RequestType = 0
        SNo = 1
        AccoutType = 2
        AccountcodeTCol = 3
        AccountNameTCol = 4
        ControlAccountCode = 5
        CurrencyCode = 6
        ExchRate = 7
        Debit = 8
        Credit = 9
        KWDDebit = 10
        KWDCredit = 11
        Narration = 12
        SupplierName = 13
        CheckIn = 14
        CheckOut = 15
        ReconfNo = 16
        AccLineno = 17
        rlineno = 18
        slineno = 19
        Accountcode = 20
        AccountName = 21
        Actualcode = 22
    End Enum
#End Region

#Region "Enum EnumHeader"
    Enum EnumHeader
        invoiceno = 0
        invoicedate = 1
        requestid = 2
        requestdate = 3
        agentcode = 4
        agentname = 5
        currcode = 6
        convrate = 7
        arrdate = 8
        depdate = 9
        amount = 10
        agentrefno = 11
        guestname = 12
        narration = 13
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
    Private Sub initialclass(ByVal con As SqlConnection, ByVal stran As SqlTransaction)
        caccounts = Nothing
        cacc = Nothing
        ctran = Nothing
        csubtran = Nothing
        caccounts = New clssave
        cacc = New clsAccounts
        cacc.clropencol()
        cacc.tran_mode = IIf(ViewState("FreeFormState") = "New", 1, 2)
        mbasecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
        cacc.start()

    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        ViewState.Add("FreeFormState", Request.QueryString("State"))
        ViewState.Add("FreeFormRefCode", Request.QueryString("InvoiceNo"))

        If IsPostBack = False Then
            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If

            If CType(Session("GlobalUserName"), String) = "" Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            End If
            Try

                txtconnection.Value = Session("dbconnectionName")
                hdnSS.Value = 0

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomer, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1 order by agentcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomerName, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname ", True)

                strSqlQry = "select  narration,narration from narration where active=1"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlNarration, "narration", "narration", strSqlQry, True)

                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
                txtbasecurr.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 457)

                lblITot_DB.Text = "Total " + txtbasecurr.Value + " Debit"
                lblItot_Cr.Text = "Total " + txtbasecurr.Value + " Debit"
                'lblCTot_DB.Text = "Total " + txtbasecurr.Value + " Debit"
                'lblCtot_Cr.Text = "Total " + txtbasecurr.Value + " Debit"

                Dim lablstr As String
                lablstr = ""

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

                div_income.Style("height") = 0
                div_cost.Style("height") = 0

                lablstr = "Free Form Invoice"
                If ViewState("FreeFormState") = "New" Then
                    SetFocus(txtDate)
                    txtDate.Text = Format("dd/MM/yyyy", CType(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), Date))

                    dpFromCheckindate.txtDate.Text = Format("dd/MM/yyyy", CType(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), Date))
                    dpFromCheckOut.txtDate.Text = Format("dd/MM/yyyy", CType(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), Date))

                    dpFromReqDate.txtDate.Text = Format("dd/MM/yyyy", CType(ObjDate.GetSystemDateOnly(Session("dbconnectionName")), Date))

                    lblHeading.Text = "Add New " & lablstr
                    btnSave.Text = "Save"
                    '    FillGrids()
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    btnfillincome.Attributes.Add("onclick", "return FormValidation('IN')")
                ElseIf ViewState("FreeFormState") = "Copy" Then
                    SetFocus(txtDate)
                    lblHeading.Text = "Copy " & lablstr
                    btnSave.Text = "Save"
                    btnfillincome.Attributes.Add("onclick", "return FormValidation('IN')")
                    '  FillGrids()
                    ShowRecord(CType(ViewState("FreeFormRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("FreeFormState") = "Edit" Then
                    txtDocNo.Text = Request.QueryString("InvoiceNo")
                    SetFocus(txtDate)
                    lblHeading.Text = "Edit " & lablstr
                    btnSave.Text = "Update"
                    btnfillincome.Attributes.Add("onclick", "return FormValidation('Edit')")
                    ' FillGrids()
                    ShowRecord(CType(ViewState("FreeFormRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('IN')")
                ElseIf ViewState("FreeFormState") = "View" Then
                    txtDocNo.Text = Request.QueryString("InvoiceNo")
                    SetFocus(btnCancel)
                    lblHeading.Text = "View " & lablstr
                    btnSave.Visible = False
                    ' FillGrids()
                    ShowRecord(CType(ViewState("FreeFormRefCode"), String))
                ElseIf ViewState("FreeFormState") = "Delete" Then
                    txtDocNo.Text = Request.QueryString("InvoiceNo")
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete " & lablstr
                    btnSave.Text = "Delete"
                    ' FillGrids()
                    ShowRecord(CType(ViewState("FreeFormRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                ElseIf ViewState("FreeFormState") = "Cancel" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Cancel " & lablstr
                    btnSave.Text = "Cancel"
                    '   FillGrids()
                    ShowRecord(CType(ViewState("FreeFormRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Cancel')")

                ElseIf ViewState("FreeFormState") = "undoCancel" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Undo Cancel " & lablstr
                    btnSave.Text = "Undo"
                    ShowRecord(CType(ViewState("FreeFormRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('UndoCancel')")


                End If
                CheckPostUnpostRight(CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType("Accounts Module", String), "AccountsModule\FreeformInvoiceSearch.aspx")
                DisableControl()


                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")
                txtConversion.Attributes.Add("onchange", "javascript:chnagerate()")
                ddlNarration.Attributes.Add("onchange", "javascript:FillCombotoText('" + CType(ddlNarration.ClientID, String) + "','" + CType(txtNarration.ClientID, String) + "')")
                NumbersHtml(txtConversion)
                NumbersDecimalRoundHtml(txtSaleValue)

                Dim typ As Type
                typ = GetType(DropDownList)


                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                ddlCustomer.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlCustomerName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlNarration.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                btnPrint.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to print?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("Freeform_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            If IsPostBack = True Then
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomerName, "agentname", "agentcode", "select agentname,agentcode from agentmast where active=1 order by agentname ", True, txtcustcode.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustomer, "agentcode", "agentname", "select agentcode,agentname from agentmast where active=1 order by agentcode", True, ddlCustomerName.Items(ddlCustomerName.SelectedIndex).Text)

                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
                txtConversion.Disabled = False
                If Trim(txtbasecurr.Value) = Trim(txtCurrency.Value) Then
                    txtConversion.Disabled = True
                End If


                'If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "AdjBillWindowPostBack") Then
                '    If DecRound(Session("AmountAdjusted")) <> 0 And DecRound(Session("BaseAmountAdjusted")) <> 0 Then
                '        txtConversion.Value = CType(Session("BaseAmountAdjusted"), Decimal) / CType(Session("AmountAdjusted"), Decimal)
                '        txtConversion.Value = Math.Round(CType(txtConversion.Value, Decimal), 8)
                '    End If
                'End If
                Dim lblsno1 As Label
                Dim ddlACode As HtmlSelect
                Dim ddlAName As HtmlSelect
                Dim ddlAccType As HtmlSelect
                Dim txtcurrcode As HtmlInputText
                Dim txtExchRate As HtmlInputText
                Dim txtamount As HtmlInputText
                Dim txtbaseamount As HtmlInputText
                Dim ddlservices As HtmlSelect
                Dim code As String
                Dim name As String

                Dim hdnAcctCode As HiddenField
                Dim hdnAcctName As HiddenField

                For Each gvRow In gv_purchase_detail.Rows
                    lblsno1 = gvRow.FindControl("lblsno")
                    ddlAccType = gvRow.FindControl("ddlAccType")
                    ddlACode = gvRow.FindControl("ddlAcctCode")
                    ddlAName = gvRow.FindControl("ddlAcctName")
                    txtcurrcode = gvRow.FindControl("txtcurrcode")
                    txtExchRate = gvRow.FindControl("txtExchRate")
                    txtamount = gvRow.FindControl("txtamount")
                    txtbaseamount = gvRow.FindControl("txtbaseamount")
                    ddlservices = gvRow.FindControl("ddlservices")

                    hdnAcctCode = gvRow.FindControl("hdnAcctCode")
                    hdnAcctName = gvRow.FindControl("hdnAcctName")


                    code = hdnAcctCode.Value
                    name = hdnAcctName.Value

                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlACode, "code", "des", "select code,des from  view_account where type='" & ddlAccType.Value & "' and controlyn='N' and type<>'C' and type<>'G' order by code", True, name)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAName, "des", "code", "select des,code from  view_account where type='" & ddlAccType.Value & "' and controlyn='N' and type<>'C' and type<>'G' order by des", True, code)

                Next


            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Freeform_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub



    '#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    '    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
    '        Dim dt As DataTable
    '        Dim dr As DataRow
    '        Dim i As Integer
    '        dt = New DataTable
    '        dt.Columns.Add(New DataColumn("SrNo", GetType(Integer)))
    '        For i = 1 To lngcount
    '            dr = dt.NewRow()
    '            dr(0) = i
    '            dt.Rows.Add(dr)
    '        Next
    '        'return a DataView to the DataTable
    '        CreateDataSource = New DataView(dt)
    '        'End If
    '    End Function
    '#End Region



#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)

        Try
            Dim lineno As String = ""
            Dim acccode As String = ""
            Dim dsResult As New DataSet
            Dim invoiceamt As Double = 0
            Dim baseinvoiceamt As Double = 0
            Dim dr As DataRow
            lblMessageIP.Visible = False
            dsResult = doSearch(RefCode)
            If dsResult.Tables.Count > 0 Then
                If dsResult.Tables(0).Rows.Count > 0 Then
                    txtDate.Text = Format(CType(dsResult.Tables(0).Rows(0)("invoicedate").ToString, Date), "dd/MM/yyyy")
                    txtRequestNo.Text = CType(dsResult.Tables(0).Rows(0)("requestid"), String)
                    dpFromReqDate.txtDate.Text = Format(CType(dsResult.Tables(0).Rows(0)("requestdate").ToString, Date), "dd/MM/yyyy")
                    ddlCustomerName.Value = CType(dsResult.Tables(0).Rows(0)("agentcode"), String)
                    ddlCustomer.Items(ddlCustomer.SelectedIndex).Text = ddlCustomerName.Value
                    txtcustcode.Value = CType(dsResult.Tables(0).Rows(0)("agentcode"), String)
                    txtCurrency.Value = CType(dsResult.Tables(0).Rows(0)("currcode"), String)
                    txtConversion.Value = CType(dsResult.Tables(0).Rows(0)("convrate"), String)
                    dpFromCheckindate.txtDate.Text = Format(CType(dsResult.Tables(0).Rows(0)("arrivaldate").ToString, Date), "dd/MM/yyyy")
                    dpFromCheckOut.txtDate.Text = Format(CType(dsResult.Tables(0).Rows(0)("departuredate").ToString, Date), "dd/MM/yyyy")
                    txtSaleValue.Value = CType(dsResult.Tables(0).Rows(0)("salevalue"), String)
                    txtReferenceNo.Text = CType(dsResult.Tables(0).Rows(0)("agentref"), String)
                    txtGuestName.Text = CType(dsResult.Tables(0).Rows(0)("guestname"), String)
                    txtNarration.Value = CType(dsResult.Tables(0).Rows(0)("narration"), String)

                End If
                If dsResult.Tables(1).Rows.Count > 0 Then
                    gv_IncomePosting.DataSource = dsResult.Tables(1)
                    gv_IncomePosting.Visible = True
                    gv_IncomePosting.DataBind()
                    div_income.Style("height") = 200
                    txtgridrowsip.Value = gv_IncomePosting.Rows.Count
                    lblMessageIP.Text = ""
                    Session.Add("income", dsResult.Tables(1))

                    'If IsDBNull(dsResult.Tables(1).Rows(0)("acc_lineno")) = False Then
                    '    lineno = CType(dsResult.Tables(1).Rows(0)("acc_lineno"), String)
                    'End If
                    'If IsDBNull(dsResult.Tables(1).Rows(0)("controlcode")) = False Then
                    '    acccode = CType(dsResult.Tables(1).Rows(0)("controlcode"), String)
                    'End If
                    'If IsDBNull(dsResult.Tables(1).Rows(0)("debit")) = False Then
                    '    invoiceamt = CType(dsResult.Tables(1).Rows(0)("debit"), Double)
                    'End If
                    'If IsDBNull(dsResult.Tables(1).Rows(0)("basedebit")) = False Then
                    '    baseinvoiceamt = CType(dsResult.Tables(1).Rows(0)("basedebit"), Double)
                    'End If

                Else
                    gv_IncomePosting.Visible = False
                    lblMessageIP.Visible = True
                    lblMessageIP.Text = "No Records Found"
                End If
                If dsResult.Tables(2).Rows.Count > 0 Then
                    gv_CostPosting.DataSource = dsResult.Tables(2)
                    gv_CostPosting.Visible = True
                    gv_CostPosting.DataBind()
                    div_cost.Style("height") = 200
                    txtgridrowscp.Value = gv_CostPosting.Rows.Count
                    Session.Add("cost", dsResult.Tables(2))

                Else
                    gv_CostPosting.Visible = False
                End If

                If dsResult.Tables(3).Rows.Count > 0 Then
                    gv_purchase_detail.DataSource = dsResult.Tables(3)
                    gv_purchase_detail.Visible = True
                    gv_purchase_detail.DataBind()
                    Session.Add("purchase", dsResult.Tables(3))
                Else
                    gv_purchase_detail.Visible = False
                End If
            Else
                If dsResult.Tables(1).Rows.Count <= 0 Then
                    gv_IncomePosting.Visible = False
                    lblMessageIP.Visible = True
                    lblMessageIP.Text = "No Records Found"
                    gv_CostPosting.Visible = False
                    gv_purchase_detail.Visible = False
                End If
            End If

            'hndStatus.Value = 0

            chkPost.Checked = False
            If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(post_state,'')  from invoice_header(nolock) where invoiceno='" & txtDocNo.Text & "' ") = "P" Then
                chkPost.Checked = True
                lblPostmsg.Text = "Posted"
                lblPostmsg.ForeColor = Drawing.Color.Red
                chkPost.Checked = True
            Else
                lblPostmsg.Text = "UnPosted"
                lblPostmsg.ForeColor = Drawing.Color.Green
            End If

            Dim basecurr As String = ""


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FreeForm_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region


    Public Function Validateseal() As Boolean
        Try

            Validateseal = True
            Dim invdate As DateTime
            Dim sealdate As DateTime
            Dim MyCultureInfo As New CultureInfo("fr-Fr")
            invdate = DateTime.Parse(txtDate.Text, MyCultureInfo, DateTimeStyles.None)
            sealdate = DateTime.Parse(txtpdate.Text, MyCultureInfo, DateTimeStyles.None)
            If invdate <= sealdate Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed in this period cannot make entry.Close the entry and make with another date')", True)
                Validateseal = False
            End If

        Catch ex As Exception
            Validateseal = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FreeForm_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("FreeFormState") = "New" Then
            btnPrint.Visible = False
        ElseIf ViewState("FreeFormState") = "Copy" Then
            btnPrint.Visible = False
        ElseIf ViewState("FreeFormState") = "Edit" Then
            btnAdd.Visible = True
            btnDelLine.Visible = True
            btnAdd_det.Visible = True
            btnDelLine_det.Visible = True

            btnPrint.Visible = False
        ElseIf ViewState("FreeFormState") = "Delete" Or ViewState("FreeFormState") = "View" Or ViewState("FreeFormState") = "Cancel" Or ViewState("FreeFormState") = "undoCancel" Then
            txtDocNo.Enabled = False
            txtDate.Enabled = False
            txtReferenceNo.Enabled = False
            txtNarration.Disabled = False
            txtCurrency.Disabled = True
            txtConversion.Disabled = True
            'ddlType.Disabled = True
            ddlCustomer.Disabled = True
            ddlCustomerName.Disabled = True
            ImgBtnFrmDt.Enabled = False
            gv_IncomePosting.Enabled = False
            gv_CostPosting.Enabled = False
            gv_purchase_detail.Enabled = False

            '     DisableGrid()
            btnPrint.Visible = False
            btnSave.Visible = False
            chkPost.Visible = False
        End If
        If ViewState("FreeFormState") = "View" Then
            btnPrint.Visible = True
        ElseIf ViewState("FreeFormState") = "Delete" Then
            btnSave.Visible = True
        ElseIf ViewState("FreeFormState") = "Cancel" Or ViewState("FreeFormState") = "undoCancel" Then
            btnPrint.Visible = False
            btnSave.Visible = True
        End If
    End Sub

#End Region


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim gvRow As GridViewRow
        Dim txtERate As HtmlInputText
        Dim txtdbt As HtmlInputText
        Dim txtcrt As HtmlInputText
        Dim txtkwddbt As HtmlInputText
        Dim txtkwdcrt As HtmlInputText
        Dim lblSupName As Label
        Dim lblChkIn As Label
        Dim lblChkOut As Label
        Dim lblRCNo As Label
        Dim lblALineno As Label
        Dim lblRlNo As Label
        Dim lblSLno As Label

        Dim ddlACode As HtmlSelect
        Dim lblActualCode As Label

        Dim strdiv, strcostcentercode, strglcode As String
        Dim acc_tranlinenno, acc_against_tranlinenno As Long
        Dim docyear As String
        acc_tranlinenno = 0
        acc_against_tranlinenno = 0
        Try
            If Page.IsValid = True Then
                If validatePage() = False Then
                    Exit Sub
                End If
                If ViewState("FreeFormState") = "Edit" Or ViewState("FreeFormState") = "Delete" Then
                    If validate_BillAgainst() = False Then
                        Exit Sub
                    End If
                End If

                'If ViewState("ResState") = "New" Or ViewState("ResState") = "Edit" Or ViewState("ResState") = "Delete" Then
                '    If Validateseal() = False Then
                '        Exit Sub
                '    End If
                'End If


                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
                strcostcentercode = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 510)
                strglcode = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select controlacctcode from agentmast where agentcode='" & ddlCustomer.Items(ddlCustomer.SelectedIndex).Text & "'")
                If chkPost.Checked = True Then
                    'For Accounts posting
                    initialclass(mySqlConn, sqlTrans)
                    'For Accounts posting
                End If
                If (ViewState("FreeFormState") = "New" Or ViewState("FreeFormState") = "Copy") And txtDocNo.Text = "" Then
                    'docyear = Trim(Str(Year(dpInvoiceDate.txtDate.Text)))
                    'If CType(docyear, Integer) < 2010 Then 'check if before the 2010 because from 2010 startt new no. for new year
                    '    txtDocNo.Value = objUtils.GetAutoDocNo("INVOICE", mySqlConn, sqlTrans)
                    'Else
                    '    txtInvoiceNo.Value = objUtils.GetAutoDocNoyear("INVOICE", mySqlConn, sqlTrans, docyear)
                    'End If
                    txtDocNo.Text = objUtils.GetAutoDocNo("FINVOICE", mySqlConn, sqlTrans)

                    mySqlCmd = New SqlCommand("sp_add_invoice_header", mySqlConn, sqlTrans)
                ElseIf ViewState("FreeFormState") = "Edit" Then
                    mySqlCmd = New SqlCommand("sp_mod_invoice_header", mySqlConn, sqlTrans)
                Else
                    Return
                End If
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@invoiceno", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@invoicetype", SqlDbType.VarChar, 10)).Value = CType("IN", String)
                mySqlCmd.Parameters.Add(New SqlParameter("@invoicedate", SqlDbType.DateTime)).Value = Format(CType(txtDate.Text, Date), "yyyy/MM/dd")
                mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(txtRequestNo.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(ddlCustomer.Items(ddlCustomer.SelectedIndex).Text, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@agentref", SqlDbType.VarChar, 20)).Value = CType(txtReferenceNo.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                If dpFromCheckindate.txtDate.Text <> "" Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@datein", SqlDbType.DateTime)).Value = CType(objdatetime.ConvertDateromTextBoxToDatabase(dpFromCheckindate.txtDate.Text), Date)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@datein", SqlDbType.DateTime)).Value = DBNull.Value
                End If

                If dpFromCheckOut.txtDate.Text <> "" Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@dateout", SqlDbType.DateTime)).Value = CType(objdatetime.ConvertDateromTextBoxToDatabase(dpFromCheckOut.txtDate.Text), Date)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@dateout", SqlDbType.DateTime)).Value = DBNull.Value
                End If

                If txtGuestName.Text <> "" Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@GuestName", SqlDbType.VarChar, 200)).Value = CType(txtGuestName.Text, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@GuestName", SqlDbType.VarChar, 200)).Value = String.Empty
                End If

                If txtNarration.Value <> "" Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 200)).Value = CType(txtNarration.Value, String)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 200)).Value = String.Empty
                End If

                If dpFromReqDate.txtDate.Text <> "" Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@requestdate", SqlDbType.DateTime)).Value = CType(objdatetime.ConvertDateromTextBoxToDatabase(dpFromReqDate.txtDate.Text), Date)
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@requestdate", SqlDbType.DateTime)).Value = DBNull.Value
                End If



                If chkPost.Checked = True Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "P"
                Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "U"
                End If
                mySqlCmd.ExecuteNonQuery()

                If chkPost.Checked = True Then
                    'For Accounts Posting
                    caccounts.clraccounts()
                    cacc.acc_tran_id = txtDocNo.Text
                    cacc.acc_tran_type = CType("IN", String)
                    cacc.acc_tran_date = Format(CType(txtDate.Text, Date), "yyyy/MM/dd")
                    cacc.acc_div_id = strdiv
                End If

                If ViewState("FreeFormState") = "Edit" Then
                    If chkPost.Checked = False Then
                        mySqlCmd = New SqlCommand("sp_delvoucher", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "reservation_invoice_header"
                        mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType("IN", String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                        mySqlCmd.ExecuteNonQuery()
                    End If
                    mySqlCmd = New SqlCommand("sp_del_invoice_detail", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@invoiceno", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(txtRequestNo.Text.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_purchase_detail", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@invoiceno", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text, String)
                    mySqlCmd.ExecuteNonQuery()

                End If
                '--------------------------------------sp_add_reservation_invoice_detail for grid 1
                Dim hdnAccType As HiddenField
                Dim hdnConAccCode As HiddenField
                Dim lblCurCode As Label
                Dim hdnCurrCode As HiddenField
                Dim lblrequestlineno As Label
                Dim lblrequesttype As Label
                Dim txtnarration1 As HtmlInputText

                For Each gvRow In gv_IncomePosting.Rows
                    hdnAccType = gvRow.FindControl("hdnAccType")
                    hdnConAccCode = gvRow.FindControl("hdnConAccCode")
                    lblCurCode = gvRow.FindControl("lblCurrCode")
                    hdnCurrCode = gvRow.FindControl("hdnCurrCode")
                    lblrequestlineno = gvRow.FindControl("lblrequestlineno")
                    lblrequesttype = gvRow.FindControl("lblrequesttype")
                    txtnarration1 = gvRow.FindControl("txtnarration")

                    mySqlCmd = New SqlCommand("sp_add_invoice_detail", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@invoiceno", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@invoicetype", SqlDbType.VarChar, 10)).Value = CType("IN", String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@invoicedate", SqlDbType.DateTime)).Value = Format(CType(txtDate.Text, Date), "yyyy/MM/dd")
                    mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(txtRequestNo.Text.Trim, String)

                    lblALineno = gvRow.FindControl("lblAccLineno")
                    mySqlCmd.Parameters.Add(New SqlParameter("@acc_lineno", SqlDbType.Int, 9)).Value = CType(lblALineno.Text, Integer)

                    mySqlCmd.Parameters.Add(New SqlParameter("@requesttype", SqlDbType.VarChar, 1)).Value = CType(lblrequesttype.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@requestlineno", SqlDbType.Int, 9)).Value = CType(lblrequestlineno.Text, Integer)
                    mySqlCmd.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.Char, 1)).Value = CType(hdnAccType.Value, Char)
                    ddlACode = gvRow.FindControl("ddlAcctCode")
                    mySqlCmd.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(ddlACode.Items(ddlACode.SelectedIndex).Text, String)
                    lblActualCode = gvRow.FindControl("lblActualCode")
                    If lblActualCode.Text = "" = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@actualcode", SqlDbType.VarChar, 20)).Value = CType(lblActualCode.Text, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@actualcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If

                    If hdnAccType.Value <> "G" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@controlcode", SqlDbType.VarChar, 20)).Value = CType(hdnConAccCode.Value, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@controlcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(hdnCurrCode.Value, String)
                    txtERate = gvRow.FindControl("txtExchRate")

                    mySqlCmd.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 12, 8)).Value = CType(txtERate.Value, Decimal)


                    txtdbt = gvRow.FindControl("txtDebit")
                    mySqlCmd.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = CType(txtdbt.Value, Decimal)

                    txtkwddbt = gvRow.FindControl("txtKWDDebit")
                    mySqlCmd.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = CType(txtkwddbt.Value, Decimal)

                    txtcrt = gvRow.FindControl("txtCredit")
                    mySqlCmd.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = CType(txtcrt.Value, Decimal)

                    txtkwdcrt = gvRow.FindControl("txtKWDCredit")
                    mySqlCmd.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = CType(txtkwdcrt.Value, Decimal)

                    mySqlCmd.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 500)).Value = CType(txtnarration1.Value, String)

                    lblSupName = gvRow.FindControl("lblSupplierName")
                    mySqlCmd.Parameters.Add(New SqlParameter("@partyname", SqlDbType.VarChar, 100)).Value = CType(lblSupName.Text, String)

                    lblChkIn = gvRow.FindControl("lblCheckIn")
                    mySqlCmd.Parameters.Add(New SqlParameter("@datein", SqlDbType.VarChar, 10)).Value = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(lblChkIn.Text), String)

                    lblChkOut = gvRow.FindControl("lblCheckOut")
                    mySqlCmd.Parameters.Add(New SqlParameter("@dateout", SqlDbType.VarChar, 10)).Value = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(lblChkOut.Text), String)

                    lblRCNo = gvRow.FindControl("lblReConfNo")
                    mySqlCmd.Parameters.Add(New SqlParameter("@reconfno", SqlDbType.VarChar, 100)).Value = CType(lblRCNo.Text, String)

                    lblRlNo = gvRow.FindControl("lblRlineNo")
                    If lblRlNo.Text <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int, 9)).Value = CType(lblRlNo.Text, Integer)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int, 9)).Value = 0
                    End If

                    lblSLno = gvRow.FindControl("lblSLineno")
                    If lblSLno.Text <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@slineno", SqlDbType.Int, 9)).Value = CType(lblSLno.Text, Integer)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@slineno", SqlDbType.Int, 9)).Value = 0
                    End If
                    mySqlCmd.ExecuteNonQuery()

                    acc_tranlinenno = CType(lblALineno.Text, Integer)
                    If chkPost.Checked = True Then
                        'Posting for the Grid Accounts
                        ctran = New clstran
                        ctran.acc_tran_id = cacc.acc_tran_id
                        ctran.acc_code = CType(ddlACode.Items(ddlACode.SelectedIndex).Text, String)
                        ctran.acc_type = CType(hdnAccType.Value, Char)
                        ctran.acc_currency_id = CType(hdnCurrCode.Value, String)
                        ctran.acc_currency_rate = CType(txtERate.Value, Decimal)
                        ctran.acc_div_id = strdiv
                        ctran.acc_narration = CType(txtnarration1.Value, String)
                        ctran.acc_tran_date = cacc.acc_tran_date
                        ctran.acc_tran_lineno = acc_tranlinenno 'CType(lblRlNo.Text, Integer)
                        ctran.acc_tran_type = cacc.acc_tran_type
                        If CType(hdnAccType.Value, Char) <> "G" Then
                            If CType(hdnConAccCode.Value, String) <> "[Select]" Then
                                ctran.pacc_gl_code = CType(hdnConAccCode.Value, String)
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
                        csubtran.acc_against_tran_lineno = acc_tranlinenno
                        csubtran.acc_against_tran_type = cacc.acc_tran_type
                        csubtran.acc_debit = DecRound(CType(txtdbt.Value, Decimal))
                        csubtran.acc_credit = DecRound(CType(txtcrt.Value, Decimal))
                        csubtran.acc_base_debit = DecRound(CType(txtkwddbt.Value, Decimal))
                        csubtran.acc_base_credit = DecRound(CType(txtkwdcrt.Value, Decimal))
                        csubtran.acc_tran_date = cacc.acc_tran_date
                        csubtran.acc_due_date = cacc.acc_tran_date
                        If ctran.acc_type = "G" Then
                            csubtran.acc_field1 = ""
                            csubtran.acc_field2 = ""
                            csubtran.acc_field3 = ""
                            csubtran.acc_field4 = ""
                            csubtran.acc_field5 = ""
                        Else
                            csubtran.acc_field1 = CType(txtRequestNo.Text.Trim, String)
                            csubtran.acc_field2 = CType(lblRCNo.Text, String)
                            csubtran.acc_field3 = CType(txtnarration1.Value, String)
                            csubtran.acc_field4 = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(lblChkIn.Text), String)
                            csubtran.acc_field5 = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(lblChkOut.Text), String)
                        End If
                        csubtran.acc_tran_id = cacc.acc_tran_id
                        csubtran.acc_tran_lineno = acc_tranlinenno 'CType(lblRlNo.Text, Integer)
                        csubtran.acc_tran_type = cacc.acc_tran_type
                        csubtran.acc_narration = CType(txtnarration1.Value, String)
                        csubtran.acc_type = CType(hdnAccType.Value, Char)
                        csubtran.currate = CType(txtERate.Value, Decimal)
                        csubtran.costcentercode = IIf(ctran.acc_type = "G", strcostcentercode, "")
                        cacc.addsubtran(csubtran)
                        acc_tranlinenno = acc_tranlinenno + 1
                    End If
                Next
                '--------------------------------------sp_add_reservation_invoice_detail for grid 2

                acc_against_tranlinenno = acc_tranlinenno
                Dim hdnAccType1 As HiddenField
                Dim hdnConAccCode1 As HiddenField
                Dim lblCurCode1 As Label
                Dim hdnCurrCode1 As HiddenField

                For Each gvRow In gv_CostPosting.Rows

                    hdnAccType1 = gvRow.FindControl("hdnAccType")
                    hdnConAccCode1 = gvRow.FindControl("hdnConAccCode")
                    lblCurCode1 = gvRow.FindControl("lblCurrCode")
                    hdnCurrCode1 = gvRow.FindControl("hdnCurrCode")
                    lblrequestlineno = gvRow.FindControl("lblrequestlineno")
                    lblrequesttype = gvRow.FindControl("lblrequesttype")
                    txtnarration1 = gvRow.FindControl("txtnarration")

                    mySqlCmd = New SqlCommand("sp_add_invoice_detail", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@invoiceno", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@invoicetype", SqlDbType.VarChar, 10)).Value = CType("IN", String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@invoicedate", SqlDbType.DateTime)).Value = CType(objdatetime.ConvertDateromTextBoxToDatabase(txtDate.Text), Date)
                    mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(txtRequestNo.Text.Trim, String)

                    lblALineno = gvRow.FindControl("lblAccLineno")
                    mySqlCmd.Parameters.Add(New SqlParameter("@acc_lineno", SqlDbType.Int, 9)).Value = CType(lblALineno.Text, Integer)

                    mySqlCmd.Parameters.Add(New SqlParameter("@requesttype", SqlDbType.VarChar, 1)).Value = CType(lblrequesttype.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@requestlineno", SqlDbType.Int, 9)).Value = CType(CType(lblrequestlineno.Text, Integer), Integer)
                    ''   mySqlCmd.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.Char, 1)).Value = CType(gvRow.Cells(GridCol.AccoutType).Text, Char)
                    ''new change 
                    mySqlCmd.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.Char, 1)).Value = CType(hdnAccType1.Value, Char)

                    ddlACode = gvRow.FindControl("ddlAcctCode")
                    mySqlCmd.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(ddlACode.Items(ddlACode.SelectedIndex).Text, String)

                    lblActualCode = gvRow.FindControl("lblActualCode")
                    If lblActualCode.Text = "" = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@actualcode", SqlDbType.VarChar, 20)).Value = CType(lblActualCode.Text, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@actualcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If


                    If CType(gvRow.Cells(GridCol.AccoutType).Text, Char) <> "G" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@controlcode", SqlDbType.VarChar, 20)).Value = CType(hdnConAccCode1.Value, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@controlcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If


                    mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(hdnCurrCode1.Value, String)

                    txtERate = gvRow.FindControl("txtExchRate")
                    mySqlCmd.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 12, 8)).Value = CType(txtERate.Value, Decimal)

                    txtdbt = gvRow.FindControl("txtDebit")
                    mySqlCmd.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = CType(txtdbt.Value, Decimal)

                    txtkwddbt = gvRow.FindControl("txtKWDDebit")
                    mySqlCmd.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = CType(txtkwddbt.Value, Decimal)

                    txtcrt = gvRow.FindControl("txtCredit")
                    mySqlCmd.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = CType(txtcrt.Value, Decimal)

                    txtkwdcrt = gvRow.FindControl("txtKWDCredit")
                    mySqlCmd.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = CType(txtkwdcrt.Value, Decimal)

                    mySqlCmd.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 500)).Value = CType(txtnarration1.Value, String)

                    lblSupName = gvRow.FindControl("lblSupplierName")
                    mySqlCmd.Parameters.Add(New SqlParameter("@partyname", SqlDbType.VarChar, 100)).Value = CType(lblSupName.Text, String)

                    lblChkIn = gvRow.FindControl("lblCheckIn")
                    mySqlCmd.Parameters.Add(New SqlParameter("@datein", SqlDbType.VarChar, 10)).Value = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(lblChkIn.Text), String)

                    lblChkOut = gvRow.FindControl("lblCheckOut")
                    mySqlCmd.Parameters.Add(New SqlParameter("@dateout", SqlDbType.VarChar, 10)).Value = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(lblChkOut.Text), String)

                    lblRCNo = gvRow.FindControl("lblReConfNo")
                    mySqlCmd.Parameters.Add(New SqlParameter("@reconfno", SqlDbType.VarChar, 100)).Value = CType(lblRCNo.Text, String)

                    lblRlNo = gvRow.FindControl("lblRlineNo")
                    If Val(lblRlNo.Text) <> 0 Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int, 9)).Value = CType(lblRlNo.Text, Integer)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int, 9)).Value = 0
                    End If

                    lblSLno = gvRow.FindControl("lblSLineno")
                    If Val(lblSLno.Text) <> 0 Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@slineno", SqlDbType.Int, 9)).Value = CType(lblSLno.Text, Integer)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@slineno", SqlDbType.Int, 9)).Value = 0
                    End If

                    mySqlCmd.ExecuteNonQuery()

                    acc_tranlinenno = CType(lblALineno.Text, Integer)
                    If chkPost.Checked = True Then
                        'Posting for the Grid Accounts
                        ctran = New clstran
                        ctran.acc_tran_id = cacc.acc_tran_id
                        ctran.acc_code = CType(ddlACode.Items(ddlACode.SelectedIndex).Text, String)
                        ''ctran.acc_type = CType(gvRow.Cells(GridCol.AccoutType).Text, Char)
                        ''ctran.acc_currency_id = CType(gvRow.Cells(GridCol.CurrencyCode).Text, String)
                        ''New changes
                        ctran.acc_type = CType(hdnAccType1.Value, Char)
                        ctran.acc_currency_id = CType(hdnCurrCode1.Value, String)
                        ctran.acc_currency_rate = CType(txtERate.Value, Decimal)
                        ctran.acc_div_id = strdiv
                        ctran.acc_narration = CType(txtnarration1.Value, String)
                        ctran.acc_tran_date = cacc.acc_tran_date
                        ctran.acc_tran_lineno = acc_tranlinenno ' CType(lblRlNo.Text, Integer)
                        ctran.acc_tran_type = cacc.acc_tran_type
                        If CType(hdnAccType1.Value, Char) <> "G" Then
                            If CType(hdnConAccCode1.Value, String) <> "[Select]" Then
                                ctran.pacc_gl_code = CType(hdnConAccCode1.Value, String)
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

                        'If CType(gvRow.Cells(GridCol.AccoutType).Text, Char) = "G" Then
                        csubtran = New clsSubTran
                        csubtran.acc_against_tran_id = cacc.acc_tran_id
                        csubtran.acc_against_tran_lineno = acc_tranlinenno 'CType(lblRlNo.Text, Integer)
                        csubtran.acc_against_tran_type = cacc.acc_tran_type
                        csubtran.acc_debit = DecRound(CType(txtdbt.Value, Decimal))
                        csubtran.acc_credit = DecRound(CType(txtcrt.Value, Decimal))
                        csubtran.acc_base_debit = DecRound(CType(txtkwddbt.Value, Decimal))
                        csubtran.acc_base_credit = DecRound(CType(txtkwdcrt.Value, Decimal))

                        csubtran.acc_tran_date = cacc.acc_tran_date
                        csubtran.acc_due_date = cacc.acc_tran_date
                        If ctran.acc_type = "G" Then
                            csubtran.acc_field1 = ""
                            csubtran.acc_field2 = ""
                            csubtran.acc_field3 = ""
                            csubtran.acc_field4 = ""
                            csubtran.acc_field5 = ""
                        Else
                            csubtran.acc_field1 = CType(txtRequestNo.Text.Trim, String)
                            csubtran.acc_field2 = CType(lblRCNo.Text, String)
                            csubtran.acc_field3 = CType(txtnarration1.Value, String)
                            csubtran.acc_field4 = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(lblChkIn.Text), String)
                            csubtran.acc_field5 = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(lblChkOut.Text), String)
                        End If
                        csubtran.acc_tran_id = cacc.acc_tran_id
                        csubtran.acc_tran_lineno = acc_tranlinenno ' CType(lblRlNo.Text, Integer)
                        csubtran.acc_tran_type = cacc.acc_tran_type
                        csubtran.acc_narration = CType(txtnarration1.Value, String)
                        ''csubtran.acc_type = CType(gvRow.Cells(GridCol.AccoutType).Text, Char)
                        csubtran.acc_type = CType(hdnAccType1.Value, Char)
                        csubtran.currate = CType(txtERate.Value, Decimal)
                        csubtran.costcentercode = IIf(ctran.acc_type = "G", strcostcentercode, "")
                        cacc.addsubtran(csubtran)
                        acc_tranlinenno = acc_tranlinenno + 1
                    End If
                Next

                Dim strnarration As String
                strnarration = txtRequestNo.Text + ":" + "Customer Ref : " + txtReferenceNo.Text + dpFromCheckindate.txtDate.Text + ":" + dpFromCheckOut.txtDate.Text + ":" + txtGuestName.Text + ":" + txtNarration.Value

                Dim lblsno1 As Label
                Dim ddlACode1 As HtmlSelect
                Dim ddlAName1 As HtmlSelect
                Dim ddlAccType1 As HtmlSelect
                Dim txtcurrcode1 As HtmlInputText
                Dim txtExchRate1 As HtmlInputText
                Dim txtamount1 As HtmlInputText
                Dim txtbaseamount1 As HtmlInputText
                Dim ddlservices1 As HtmlSelect
                For Each gvRow In gv_purchase_detail.Rows
                    lblsno1 = gvRow.FindControl("lblsno")
                    ddlAccType1 = gvRow.FindControl("ddlAccType")
                    ddlACode1 = gvRow.FindControl("ddlAcctCode")
                    ddlAName1 = gvRow.FindControl("ddlAcctName")
                    txtcurrcode1 = gvRow.FindControl("txtcurrcode")
                    txtExchRate1 = gvRow.FindControl("txtExchRate")
                    txtamount1 = gvRow.FindControl("txtamount")
                    txtbaseamount1 = gvRow.FindControl("txtbaseamount")
                    ddlservices1 = gvRow.FindControl("ddlservices")

                    myCommand = New SqlCommand("sp_add_purchase_detail", mySqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text, String)
                    myCommand.Parameters.Add(New SqlParameter("@sno", SqlDbType.Int, 9)).Value = CType(lblsno1.Text, Integer)
                    myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 10)).Value = CType(ddlAccType1.Value, String)
                    If ddlACode1.Value <> "[Select]" Then
                        myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(ddlACode1.Items(ddlACode1.SelectedIndex).Text, String)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = String.Empty
                    End If
                    If txtcurrcode1.Value <> "" Then
                        myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 10)).Value = CType(txtcurrcode1.Value, String)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 10)).Value = ""
                    End If
                    If Val(txtExchRate1.Value) <> 0 Then
                        myCommand.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 12, 8)).Value = CType(txtExchRate1.Value, Decimal)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 12, 8)).Value = CType(txtExchRate1.Value, Decimal)
                    End If
                    myCommand.Parameters.Add(New SqlParameter("@amount", SqlDbType.Money)).Value = CType(txtamount1.Value, Decimal)
                    myCommand.Parameters.Add(New SqlParameter("@baseamount", SqlDbType.Money)).Value = CType(txtbaseamount1.Value, Decimal)
                    myCommand.Parameters.Add(New SqlParameter("@datein", SqlDbType.VarChar, 10)).Value = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(dpFromCheckindate.txtDate.Text), String)
                    myCommand.Parameters.Add(New SqlParameter("@dateout", SqlDbType.VarChar, 10)).Value = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(dpFromCheckOut.txtDate.Text), String)
                    myCommand.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 500)).Value = strnarration.Trim
                    myCommand.Parameters.Add(New SqlParameter("@requesttype", SqlDbType.Char, 1)).Value = ddlservices1.Value

                    myCommand.ExecuteNonQuery()
                Next


                mySqlCmd = New SqlCommand("sp_del_purchase_detail_temp", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = CType(txtpurchaseid.Value, String)
                mySqlCmd.ExecuteNonQuery()



                If chkPost.Checked = True Then
                    'For Accounts Posting
                    cacc.table_name = ""
                    caccounts.Addaccounts(cacc)

                    'If txtAmtAdv.Value <> "" And Val(txtAmtAdv.Value) <> 0 Then
                    '    cacc1.table_name = ""
                    '    caccounts.Addaccounts(cacc1)
                    'End If

                    If caccounts.saveaccounts(Session("dbconnectionName"), mySqlConn, sqlTrans, Me.Page) <> 0 Then
                        Err.Raise(vbObjectError + 100)
                    End If
                    'For Accounts Posting
                    lblPostmsg.Text = "Posted"
                    lblPostmsg.ForeColor = Drawing.Color.Red
                Else
                    lblPostmsg.Text = "UnPosted"
                    lblPostmsg.ForeColor = Drawing.Color.Green
                End If

                sqlTrans.Commit()                                           'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)                      ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)                       'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)                   'connection close
                'Change 12/11/2008 ****************************
                'Response.Redirect("ReservationInvoice.aspx", False)
                'Response.Redirect("InvoicePrint.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('ReservationFreeWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                'Change 12/11/2008 ****************************
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)                      ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)                       'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)                   'connection close
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FreeForm_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Private Function validate_BillAgainst() As Boolean
        Try

            validate_BillAgainst = True
            Dim myDataAdapter As SqlDataAdapter
            Dim Alflg As Integer
            Dim ErrMsg, strdiv As String
            strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myCommand = New SqlCommand("sp_Check_AgainstBills", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Text
            myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType("IN", String)

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
            objUtils.WritErrorLog("FreeForm_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try
    End Function

    Private Function validatePage_purchase() As Boolean
        Try
            Dim ddlAccType As HtmlSelect
            Dim lblsno1 As Label
            Dim ddlACode As HtmlSelect
            Dim ddlAName As HtmlSelect
            Dim txtcurrcode As HtmlInputText
            Dim txtamount As HtmlInputText
            Dim txtbaseamount As HtmlInputText
            Dim ddlservices As HtmlSelect
            Dim txtExchRate As HtmlInputText
            Dim dfalg As Boolean = True

            For Each gvRow In gv_purchase_detail.Rows
                lblsno1 = gvRow.FindControl("lblsno")
                ddlAccType = gvRow.FindControl("ddlAccType")
                ddlACode = gvRow.FindControl("ddlAcctCode")
                ddlAName = gvRow.FindControl("ddlAcctName")
                txtcurrcode = gvRow.FindControl("txtcurrcode")
                txtExchRate = gvRow.FindControl("txtExchRate")
                txtamount = gvRow.FindControl("txtamount")
                txtbaseamount = gvRow.FindControl("txtbaseamount")
                ddlservices = gvRow.FindControl("ddlservices")

                If ddlAccType.Value.Trim <> "[Select]" Or txtExchRate.Value.Trim <> "" Then
                    dfalg = False
                    If ddlAccType.Value = "[Select]" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select purchase account type.');", True)
                        validatePage_purchase = False
                        Exit Function
                    End If
                    If txtExchRate.Value = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select purchase currancy code.');", True)
                        validatePage_purchase = False
                        Exit Function
                    End If
                    If Val(txtExchRate.Value) <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid purchase exachange rate.');", True)
                        validatePage_purchase = False
                        Exit Function
                    End If

                    If txtcurrcode.Value = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select purchase currancy code.');", True)
                        validatePage_purchase = False
                        Exit Function
                    End If


                    If Val(txtamount.Value) <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter purchase  amount.');", True)
                        validatePage_purchase = False
                        Exit Function
                    End If
                    If Val(txtbaseamount.Value) <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Purchase base amount cannot be 0.');", True)
                        validatePage_purchase = False
                        Exit Function
                    End If

                End If
            Next

            validatePage_purchase = True

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("requestforinvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
    Private Function validatePage() As Boolean
        Try
            validatePage = True

            If ddlCustomer.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select customer account code.');", True)
                validatePage = False
                Exit Function
            End If

            If ddlCustomerName.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select customer name code.');", True)
                validatePage = False
                Exit Function
            End If

            If txtConversion.Value = "" Or Val(txtConversion.Value) = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Conversion cannot be 0.');", True)
                validatePage = False
                Exit Function
            End If

            If Val(txtSaleValue.Value) = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Sale value cannot be 0.');", True)
                validatePage = False
                Exit Function
            End If

            If txtCurrency.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Currency cannot be 0.');", True)
                validatePage = False
                Exit Function
            End If

            If txtGuestName.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Guest name cannot be blank');", True)
                validatePage = False
                Exit Function
            End If

            If txtRequestNo.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Requestid cannot be blank');", True)
                validatePage = False
                Exit Function
            End If

            If txtReferenceNo.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Reference no cannot be blank');", True)
                validatePage = False
                Exit Function
            End If


            If Val(txtPL.Value) < 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check the entry profit is below 0');", True)
                validatePage = False
                Exit Function
            End If

            If Val(txtPL.Value) = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please check the entry profit is  0');", True)
                validatePage = False
                Exit Function
            End If

            If ViewState("FreeFormState") = "New" Then
                If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "invoice_header", "requestid", CType(txtRequestNo.Text.Trim, String)) = True Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Request no. already exists');", True)
                    validatePage = False
                    Exit Function
                End If
            End If

            Dim baseamt As Decimal
            Dim dfalg As Boolean = True
            Dim lblrequesttype As Label
            Dim lblrequestlineno As Label
            Dim ddlAccType As HtmlSelect
            Dim ddlAcctCode As HtmlSelect
            Dim ddlConAccCode As HtmlSelect
            Dim lblCurrCode As Label
            Dim txtExchRate, txtCredit, txtBaseCredit, txtBaseDebit, txtDebit As HtmlInputText
            Dim lblAccLineno As Label

            For Each gvRow In gv_IncomePosting.Rows
                lblrequesttype = gvRow.FindControl("lblrequesttype")
                lblrequestlineno = gvRow.FindControl("lblrequestlineno")
                lblAccLineno = gvRow.FindControl("lblAccLineno")
                ddlAccType = gvRow.FindControl("ddlAccType")
                ddlAcctCode = gvRow.FindControl("ddlAcctCode")
                ddlConAccCode = gvRow.FindControl("ddlConAccCode")
                lblCurrCode = gvRow.FindControl("lblCurrCode")
                txtExchRate = gvRow.FindControl("txtExchRate")
                txtDebit = gvRow.FindControl("txtDebit")
                txtCredit = gvRow.FindControl("txtCredit")
                txtBaseDebit = gvRow.FindControl("txtKWDDebit")
                txtBaseCredit = gvRow.FindControl("txtKWDCredit")

                If ddlAccType.Value.Trim <> "[Select]" Or txtExchRate.Value.Trim <> "" Or ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text <> "[Select]" Then
                    dfalg = False
                    If ddlAccType.Value = "[Select]" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select income account type.');", True)
                        SetFocus(ddlAccType)
                        validatePage = False
                        Exit Function
                    End If
                    If ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text = "[Select]" And ddlAccType.Value <> "G" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select income account code.');", True)
                        validatePage = False
                        Exit Function
                    End If

                    If lblCurrCode.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select income currency code.');", True)
                        validatePage = False
                        Exit Function
                    End If

                    If txtExchRate.Value = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select income exachange code.');", True)
                        validatePage = False
                        Exit Function
                    End If
                    If Val(txtExchRate.Value) <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid income exachange rate.');", True)
                        SetFocus(ddlAccType)
                        validatePage = False
                        Exit Function
                    End If
                    If Val(txtDebit.Value) <= 0 And Val(txtCredit.Value) <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter income debit or credit amount.');", True)
                        SetFocus(txtDebit)
                        validatePage = False
                        Exit Function
                    End If
                    If Val(txtBaseDebit.Value) = 0 And Val(txtBaseCredit.Value) = 0 Then
                        Dim strMsg As String = ""
                        Dim basecurr As String = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
                        strMsg = "Both " & basecurr & " Debit amount and " & basecurr & " Credit amount can not be zero."
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "');", True)
                        validatePage = False
                        Exit Function
                    End If


                    If ddlAccType.Value <> "G" Then
                        If ddlConAccCode.Value = "[Select]" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Control account code.');", True)
                            validatePage = False
                            Exit Function
                        End If
                        If Val(txtBaseDebit.Value) <> 0 Then
                            baseamt = DecRound(CType(txtBaseDebit.Value, Decimal))
                        Else
                            baseamt = DecRound(CType(txtBaseCredit.Value, Decimal))
                        End If
                    End If
                End If
            Next

            If Val(txtTotalKWDDebit.Value) <> Val(txtTotalKWDCredit.Value) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Income total of debit and credit in base amount should be equal.');", True)
                validatePage = False
                Exit Function
            End If

            For Each gvRow In gv_CostPosting.Rows
                lblrequesttype = gvRow.FindControl("lblrequesttype")
                lblrequestlineno = gvRow.FindControl("lblrequestlineno")
                lblAccLineno = gvRow.FindControl("lblAccLineno")
                ddlAccType = gvRow.FindControl("ddlAccType")
                ddlAcctCode = gvRow.FindControl("ddlAcctCode")
                ddlConAccCode = gvRow.FindControl("ddlConAccCode")
                lblCurrCode = gvRow.FindControl("lblCurrCode")
                txtExchRate = gvRow.FindControl("txtExchRate")
                txtDebit = gvRow.FindControl("txtDebit")
                txtCredit = gvRow.FindControl("txtCredit")
                txtBaseDebit = gvRow.FindControl("txtKWDDebit")
                txtBaseCredit = gvRow.FindControl("txtKWDCredit")

                If ddlAccType.Value.Trim <> "[Select]" Or txtExchRate.Value.Trim <> "" Or ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text <> "[Select]" Then
                    dfalg = False
                    If ddlAccType.Value = "[Select]" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select cost account type.');", True)
                        validatePage = False
                        Exit Function
                    End If
                    If ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text = "[Select]" And ddlAccType.Value <> "G" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select cost account code.');", True)
                        validatePage = False
                        Exit Function
                    End If

                    If lblCurrCode.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Currancy code.');", True)
                        validatePage = False
                        Exit Function
                    End If


                    If txtExchRate.Value = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select cost exachange code.');", True)
                        validatePage = False
                        Exit Function
                    End If
                    If Val(txtExchRate.Value) <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid cost exachange rate.');", True)
                        validatePage = False
                        Exit Function
                    End If
                    If Val(txtDebit.Value) <= 0 And Val(txtCredit.Value) <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter cost debit or credit amount.');", True)
                        validatePage = False
                        Exit Function
                    End If
                    If Val(txtBaseDebit.Value) = 0 And Val(txtBaseCredit.Value) = 0 Then
                        Dim strMsg As String = ""
                        Dim basecurr As String = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
                        strMsg = "Both " & basecurr & " Debit amount and " & basecurr & " Credit amount can not be zero."
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "');", True)
                        validatePage = False
                        Exit Function
                    End If


                    If ddlAccType.Value <> "G" Then
                        If ddlConAccCode.Value = "[Select]" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Control account code.');", True)
                            validatePage = False
                            Exit Function
                        End If
                        If Val(txtBaseDebit.Value) <> 0 Then
                            baseamt = DecRound(CType(txtBaseDebit.Value, Decimal))
                        Else
                            baseamt = DecRound(CType(txtBaseCredit.Value, Decimal))
                        End If
                    End If
                End If

            Next

            If Val(txtTotalKWDDebitCP.Value) <> Val(txtTotalKWDCreditCP.Value) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cost total of debit and credit in base amount should be equal.');", True)
                validatePage = False
                Exit Function
            End If

            Dim lblsno1 As Label
            Dim ddlACode As HtmlSelect
            Dim ddlAName As HtmlSelect
            Dim txtcurrcode As HtmlInputText
            Dim txtamount As HtmlInputText
            Dim txtbaseamount As HtmlInputText
            Dim ddlservices As HtmlSelect

            For Each gvRow In gv_purchase_detail.Rows
                lblsno1 = gvRow.FindControl("lblsno")
                ddlAccType = gvRow.FindControl("ddlAccType")
                ddlACode = gvRow.FindControl("ddlAcctCode")
                ddlAName = gvRow.FindControl("ddlAcctName")
                txtcurrcode = gvRow.FindControl("txtcurrcode")
                txtExchRate = gvRow.FindControl("txtExchRate")
                txtamount = gvRow.FindControl("txtamount")
                txtbaseamount = gvRow.FindControl("txtbaseamount")
                ddlservices = gvRow.FindControl("ddlservices")

                If ddlAccType.Value.Trim <> "[Select]" Or txtExchRate.Value.Trim <> "" Then
                    dfalg = False
                    If ddlAccType.Value = "[Select]" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select purchase account type.');", True)
                        validatePage = False
                        Exit Function
                    End If
                    If txtExchRate.Value = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select purchase currancy code.');", True)
                        validatePage = False
                        Exit Function
                    End If
                    If Val(txtExchRate.Value) <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid purchase exachange rate.');", True)
                        validatePage = False
                        Exit Function
                    End If

                    If txtcurrcode.Value = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select purchase currancy code.');", True)
                        validatePage = False
                        Exit Function
                    End If


                    If Val(txtamount.Value) <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter purchase  amount.');", True)
                        validatePage = False
                        Exit Function
                    End If
                    If Val(txtbaseamount.Value) <= 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Purchase base amount cannot be 0.');", True)
                        validatePage = False
                        Exit Function
                    End If

                End If
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("requestforinvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If ViewState("FreeFormState") = "New" Or ViewState("FreeFormState") = "Copy" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "trdpurchase_master", "tran_id", CType(txtDocNo.Text.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This record is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Session.Remove("Collection" & ":" & txtAdjcolno.Value)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ReservationFreeWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try

            Dim strpop As String = ""
            strpop = "window.open('rptFreeInvoice.aspx?State=Print&InvoiceNo=" + CType(txtDocNo.Text.Trim, String) + "','Accounts','width=1000,height=620 left=20,top=20 status=yes,toolbar=no,menubar=no,resizable=yes,scrollbars=yes');"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FreeForm_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal
        Rdamt = Math.Round(Val(Ramt), CType(txtdecimal.Value, Integer))
        Return Rdamt
    End Function




    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=DebitNote','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Private Sub CheckPostUnpostRight(ByVal UserName As String, ByVal UserPwd As String, ByVal AppName As String, ByVal PageName As String)
        Dim PostUnpostFlag As Boolean = False
        PostUnpostFlag = objUser.PostUnpostRight(Session("dbconnectionName"), UserName, UserPwd, AppName, PageName)
        If PostUnpostFlag = True Then
            chkPost.Visible = True
            lblPostmsg.Visible = True
        Else
            chkPost.Visible = False
            lblPostmsg.Visible = False
            If ViewState("FreeFormState") = "Edit" Then
                If chkPost.Checked = True Then
                    ViewState.Add("FreeFormState", "View")
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This transaction has been posted, you do not have rights to edit.' );", True)
                End If
            End If
        End If
    End Sub

    Protected Sub gv_IncomePosting_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_IncomePosting.RowDataBound
        Dim lblCurrCode As Label
        Dim lblrequesttype As Label
        Dim lblrequestlineno As Label
        Dim txtExchRate As HtmlInputText
        Dim txtDebit As HtmlInputText
        Dim txtKWDDebit As HtmlInputText
        Dim hdnCurrCode As HiddenField

        Dim ddlACode As HtmlSelect
        Dim ddlAName As HtmlSelect
        Dim ddlAccType As HtmlSelect

        Dim hdnAccType As HiddenField

        Dim lblCtrlAccCode As Label
        Dim ddlConAccCode As HtmlSelect
        Dim ddlConAccName As HtmlSelect
        Dim hdnConAccCode As HiddenField

        Dim txtERate As HtmlInputText

        Dim txtdbt As HtmlInputText
        Dim txtcrt As HtmlInputText
        Dim lblACode As Label
        Dim lblAName As Label
        Dim lblChkIn As Label
        Dim lblChkOut As Label

        Dim txtkwddbt As HtmlInputText
        Dim txtkwdcrt As HtmlInputText
        Dim txtnarration1 As HtmlInputText
        Dim lblAcctype As Label
        'Dim lblrequestlineno As label
        Dim actype As String = ""
        Dim sqlstr1 As String = ""
        Dim sqlstr2 As String = ""

        Try
            If (e.Row.RowType = DataControlRowType.Header) Then
                e.Row.Cells(10).Text = "Debit" + "[" + txtbasecurr.Value + "]"
                e.Row.Cells(11).Text = "Credit" + "[" + txtbasecurr.Value + "]"
            End If

            If (e.Row.RowType = DataControlRowType.DataRow) Then
                lblrequesttype = e.Row.FindControl("lblrequesttype")
                lblrequestlineno = e.Row.FindControl("lblrequestlineno")
                lblCurrCode = e.Row.FindControl("lblCurrCode")
                lblChkIn = e.Row.FindControl("lblCheckIn")
                lblChkOut = e.Row.FindControl("lblCheckOut")


                txtExchRate = e.Row.FindControl("txtExchRate")
                txtDebit = e.Row.FindControl("txtDebit")
                txtKWDDebit = e.Row.FindControl("txtKWDDebit")
                hdnCurrCode = e.Row.FindControl("hdnCurrCode")

                ddlACode = e.Row.FindControl("ddlAcctCode")
                ddlAName = e.Row.FindControl("ddlAcctName")
                ddlAccType = e.Row.FindControl("ddlAccType")
                hdnAccType = e.Row.FindControl("hdnAccType")

                lblCtrlAccCode = e.Row.FindControl("lblCtrlAccCode")
                ddlConAccCode = e.Row.FindControl("ddlConAccCode")
                ddlConAccName = e.Row.FindControl("ddlConAccName")
                hdnConAccCode = e.Row.FindControl("hdnConAccCode")

                txtERate = e.Row.FindControl("txtExchRate")

                lblACode = e.Row.FindControl("lblAcctCode")
                lblAName = e.Row.FindControl("lblAcctName")

                lblAcctype = e.Row.FindControl("lblAcctype")

                txtkwddbt = e.Row.FindControl("txtKWDDebit")
                txtkwdcrt = e.Row.FindControl("txtKWDCredit")

                txtdbt = e.Row.FindControl("txtDebit")
                txtcrt = e.Row.FindControl("txtCredit")
                txtnarration1 = e.Row.FindControl("txtnarration")

                If txtTotalKWDCredit.Value = "" Then
                    txtTotalKWDCredit.Value = 0
                End If
                If txtTotalKWDDebit.Value = "" Then
                    txtTotalKWDDebit.Value = 0
                End If

                If txtkwddbt.Value = "" Or txtkwddbt.Value = "&nbsp;" Then
                    txtkwddbt.Value = 0
                End If
                If txtkwdcrt.Value = "" Or txtkwdcrt.Value = "&nbsp;" Then
                    txtkwdcrt.Value = 0
                End If

                txtTotalKWDCredit.Value = Math.Round(CType(txtTotalKWDCredit.Value, Decimal) + CType(txtkwdcrt.Value, Decimal), 3)
                txtTotalKWDDebit.Value = Math.Round(CType(txtTotalKWDDebit.Value, Decimal) + CType(txtkwddbt.Value, Decimal), 3)

                txtERate.Disabled = False

                If txtnarration1.Value = "" Then
                    txtnarration1.Value = txtRequestNo.Text + ":" + "Customer Ref : " + txtReferenceNo.Text + dpFromCheckindate.txtDate.Text + ":" + dpFromCheckOut.txtDate.Text + ":" + txtGuestName.Text + ":" + txtNarration.Value
                End If

                lblChkIn.Text = Format(CType(dpFromCheckindate.txtDate.Text, Date), "yyyy/MM/dd")
                lblChkOut.Text = Format(CType(dpFromCheckOut.txtDate.Text, Date), "yyyy/MM/dd")


                '  e.Row.Cells(12).Width = 25

                If CType(lblAcctype.Text, String) = "G" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlACode, "code", "des", "select code,des from  view_account where type='G' and controlyn='N' order by code", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAName, "des", "code", "select des,code from  view_account where type='G' and controlyn='N' order by des", True)
                    ddlACode.Disabled = False
                    ddlAName.Disabled = False
                    txtERate.Disabled = True
                    ddlAccType.Disabled = True

                Else
                    actype = CType(lblAcctype.Text, String)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlACode, "code", "des", "select code,des from  view_account where type='" & actype & "' and controlyn='N' order by code", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAName, "des", "code", "select des,code from  view_account where type='" & actype & "' and controlyn='N' order by des", True)
                    ddlACode.Disabled = True
                    ddlAName.Disabled = True
                    ddlAccType.Disabled = True

                    If txtbasecurr.Value = CType(e.Row.Cells(GridCol.CurrencyCode).Text, String) Then
                        txtERate.Disabled = True
                    End If
                End If

                If lblrequestlineno.Text = "0" Then
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlACode, "code", "des", "select code,des from  view_account where controlyn='N' order by code")
                    '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAName, "des", "code", "select des,code from  view_account where controlyn='N' order by des")
                End If

                'ddlACode.Value = CType(lblAName.Text, String)
                'ddlAName.Value = CType(lblACode.Text, String)
                Dim a As ListItem
                ddlAName.Value = CType(lblACode.Text, String)
                For Each a In ddlACode.Items
                    If a.Text = CType(lblACode.Text, String) Then
                        ddlACode.Value = a.Value
                        Exit For
                    End If
                Next

                'If Session("change") <> "Yes" Then
                '    ddlAccType.Disabled = True
                'End If

                If ddlAccType Is Nothing = False Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccType, "acc_type_des", "acc_type_name", "select acc_type_des,acc_type_name from  acc_type_master where acc_type_mode<>'G' order by acc_type_name", True)
                    ddlAccType.Value = lblAcctype.Text
                End If
                If lblrequestlineno.Text = "0" Then
                    ddlConAccCode.Style("visibility") = "visible"
                    ddlConAccName.Style("visibility") = "visible"

                    lblCtrlAccCode.Style("visibility") = "hidden"

                    sqlstr1 = GetSql1(lblAcctype.Text, lblACode.Text)
                    sqlstr2 = GetSql2(lblAcctype.Text, lblACode.Text)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True)
                    ddlConAccName.Value = CType(lblCtrlAccCode.Text, String)
                    For Each a In ddlConAccCode.Items
                        If a.Text = CType(lblCtrlAccCode.Text, String) Then
                            ddlConAccCode.Value = a.Value
                            Exit For
                        End If
                    Next
                    ddlConAccCode.Disabled = True
                    ddlConAccName.Disabled = True


                    ddlAccType.Style("visibility") = "visible"
                    lblAcctype.Style("visibility") = "hidden"

                    ddlAccType.Attributes.Add("onchange", "javascript:LoadAcc('" + CType(hdnAccType.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "')")
                    ddlAccType.Value = lblAcctype.Text
                    'If lblAcctype.Text = "G" Then
                    '    ddlACode.Disabled = True
                    '    ddlAName.Disabled = True
                    'Else
                    '    ddlACode.Disabled = False
                    '    ddlAName.Disabled = False
                    'End If
                Else
                    lblCtrlAccCode.Style("visibility") = "visible"
                    ddlConAccCode.Style("visibility") = "hidden"
                    ddlConAccName.Style("visibility") = "hidden"

                    ddlAccType.Style("visibility") = "hidden"
                    lblAcctype.Style("visibility") = "visible"
                End If

                ddlConAccCode.Attributes.Add("onchange", "javascript:FillCode('" + CType(hdnConAccCode.ClientID, String) + "','" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "')")
                ddlConAccName.Attributes.Add("onchange", "javascript:FillName('" + CType(hdnConAccCode.ClientID, String) + "','" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "')")

                'ddlACode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"view_account", "des", "code", CType(lblACode.Text, String)) 'e.Row.Cells(3).Text
                'ddlAName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"view_account", "code", "des", CType(lblAName.Text, String))  'e.Row.Cells(3).Text

                ddlACode.Attributes.Add("onchange", "javascript:FillAccCode('" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "','" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(lblCurrCode.ClientID, String) + "','" + CType(txtExchRate.ClientID, String) + "','" + CType(txtDebit.ClientID, String) + "','" + CType(txtKWDDebit.ClientID, String) + "','" + CType(hdnCurrCode.ClientID, String) + "','" + CType(hdnConAccCode.ClientID, String) + "','" + e.Row.RowIndex.ToString() + "')")
                ddlAName.Attributes.Add("onchange", "javascript:FillAccName('" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "','" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(lblCurrCode.ClientID, String) + "','" + CType(txtExchRate.ClientID, String) + "','" + CType(txtDebit.ClientID, String) + "','" + CType(txtKWDDebit.ClientID, String) + "','" + CType(hdnCurrCode.ClientID, String) + "','" + CType(hdnConAccCode.ClientID, String) + "','" + e.Row.RowIndex.ToString() + "')")

                txtERate.Attributes.Add("onchange", "javascript:GetKWDDrCr('" + CType(txtdbt.ClientID, String) + "','" + CType(txtcrt.ClientID, String) + "','" + CType(txtkwddbt.ClientID, String) + "','" + CType(txtkwdcrt.ClientID, String) + "','" + CType(txtERate.ClientID, String) + "','" + CType("IP", String) + "')")
                txtdbt.Attributes.Add("onchange", "javascript:GetKWDDrCr('" + CType(txtdbt.ClientID, String) + "','" + CType(txtcrt.ClientID, String) + "','" + CType(txtkwddbt.ClientID, String) + "','" + CType(txtkwdcrt.ClientID, String) + "','" + CType(txtERate.ClientID, String) + "','" + CType("IP", String) + "')")
                txtcrt.Attributes.Add("onchange", "javascript:GetKWDDrCr('" + CType(txtdbt.ClientID, String) + "','" + CType(txtcrt.ClientID, String) + "','" + CType(txtkwddbt.ClientID, String) + "','" + CType(txtkwdcrt.ClientID, String) + "','" + CType(txtERate.ClientID, String) + "','" + CType("IP", String) + "')")
                NumbersHtml(txtERate)
                NumbersHtml(txtdbt)
                NumbersHtml(txtcrt)
                'NumbersDecimalRoundHtml(txtdbt)
                'NumbersDecimalRoundHtml(txtcrt)

                Dim typ As Type
                typ = GetType(DropDownList)
                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                End If
                ddlACode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlAName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FreeForm_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Sub

    Public Function GetSql1(ByVal typ As String, ByVal accode As String) As String
        Dim strsql1 As String = ""

        strsql1 = "select distinct view_account.controlacctcode, acctmast.acctname from acctmast ,view_account where "
        strsql1 += "view_account.controlacctcode= acctmast.acctcode Union all select distinct partymast.controlacctcode  , "
        strsql1 += "acctmast.acctname  from acctmast ,partymast where   partymast.controlacctcode= acctmast.acctcode   "
        strsql1 += "Union all  select distinct supplier_agents.controlacctcode, acctmast.acctname  from acctmast ,supplier_agents where   supplier_agents.controlacctcode= acctmast.acctcode "
        Return strsql1
    End Function
    Public Function GetSql2(ByVal typ As String, ByVal accode As String) As String
        Dim strsql2 As String = ""
        strsql2 = "select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where  "
        strsql2 += " view_account.controlacctcode = acctmast.acctcode Union all select distinct acctmast.acctname ,partymast.controlacctcode "
        strsql2 += " from acctmast ,partymast where partymast.controlacctcode = acctmast.acctcode"
        strsql2 += " Union all select distinct acctmast.acctname ,supplier_agents.controlacctcode from acctmast ,supplier_agents where  supplier_agents.controlacctcode = acctmast.acctcode"
        Return strsql2
    End Function

    Private Sub FillAllData()
        Try
            Dim lineno As String = ""
            Dim acccode As String = ""
            Dim dsResult As New DataSet
            Dim invoiceamt As Double = 0
            Dim baseinvoiceamt As Double = 0
            Dim dr As DataRow
            lblMessageIP.Visible = False
            '    dsResult = doSearch()
            If dsResult.Tables.Count > 0 Then
                If dsResult.Tables(0).Rows.Count > 0 Then

                End If
                If dsResult.Tables(1).Rows.Count > 0 Then
                    gv_IncomePosting.DataSource = dsResult.Tables(1)
                    gv_IncomePosting.Visible = True
                    gv_IncomePosting.DataBind()
                    'txtgridrowsip.Value = gv_IncomePosting.Rows.Count
                    lblMessageIP.Text = ""
                    If IsDBNull(dsResult.Tables(1).Rows(0)("acc_lineno")) = False Then
                        lineno = CType(dsResult.Tables(1).Rows(0)("acc_lineno"), String)
                    End If
                    If IsDBNull(dsResult.Tables(1).Rows(0)("controlcode")) = False Then
                        acccode = CType(dsResult.Tables(1).Rows(0)("controlcode"), String)
                    End If
                    'If IsDBNull(dsResult.Tables(1).Rows(0)("currcode")) = False Then
                    '    lblAdvAmt.Text = CType(dsResult.Tables(1).Rows(0)("currcode"), String) + " Adv. Amount"
                    '    lblBalAdj.Text = CType(dsResult.Tables(1).Rows(0)("currcode"), String) + " Balance to Adj"
                    'End If
                    If IsDBNull(dsResult.Tables(1).Rows(0)("debit")) = False Then
                        invoiceamt = CType(dsResult.Tables(1).Rows(0)("debit"), Double)
                    End If
                    If IsDBNull(dsResult.Tables(1).Rows(0)("basedebit")) = False Then
                        baseinvoiceamt = CType(dsResult.Tables(1).Rows(0)("basedebit"), Double)
                    End If
                Else
                    gv_IncomePosting.Visible = False
                    lblMessageIP.Visible = True
                    lblMessageIP.Text = "No Records Found"
                End If

            Else
                If dsResult.Tables(1).Rows.Count <= 0 Then
                    gv_IncomePosting.Visible = False
                    lblMessageIP.Visible = True
                    lblMessageIP.Text = "No Records Found"

                End If
            End If
            Dim adjamt As Double = 0
            Dim baseadjamt As Double = 0

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FreeForm_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Public Function doSearch(ByVal RefCode As String) As DataSet
        doSearch = Nothing
        Try
            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(1) As SqlParameter

            If Not (RefCode = "") Then
                parm(0) = New SqlParameter("@docno", CType(RefCode, String))
            Else
                parm(0) = New SqlParameter("@docno", String.Empty)
            End If
            parms.Add(parm(0))


            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_Freeform_invoicing", parms)
            Return ds
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FreeForm_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

    Protected Sub btnfillincome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfillincome.Click
        Try
            If Validatedate() = False Then
                Exit Sub
            End If

            Session.Add("income", Nothing)
            'If Session("income") Is Nothing Then
            '    CreateTable()
            'End If

            Dim dsResult As New DataSet
            dsResult = FillSalesgrid()
            If dsResult.Tables.Count > 0 Then
                If dsResult.Tables(0).Rows.Count > 0 Then
                    gv_IncomePosting.DataSource = dsResult.Tables(0)
                    gv_IncomePosting.DataBind()
                    div_income.Style("height") = 200
                    txtgridrowsip.Value = gv_IncomePosting.Rows.Count
                    txtmaxacclineno.Value = dsResult.Tables(1).Rows(0)("acc_lineno")
                    Session.Add("income", dsResult.Tables(0))
                    btnAdd.Visible = True
                    btnDelLine.Visible = True
                    If gv_purchase_detail.Rows.Count = 0 Then
                        fillgrddetail(gv_purchase_detail, False, 1)
                        btnAdd_det.Visible = True
                        btnDelLine_det.Visible = True
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FreeForm_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        End Try

    End Sub

    Public Function FillSalesgrid() As DataSet
        FillSalesgrid = Nothing
        Try

            Dim ds As DataSet = New DataSet()
            Dim parms As New List(Of SqlParameter)
            Dim parm(4) As SqlParameter

            parm(0) = New SqlParameter("@acctype", "C")
            If CType(ddlCustomer.Items(ddlCustomer.SelectedIndex).Text, String) <> "[Select]" Then
                parm(1) = New SqlParameter("@acccode", CType(ddlCustomer.Items(ddlCustomer.SelectedIndex).Text, String))
            Else
                parm(1) = New SqlParameter("@acccode", String.Empty)
            End If
            If Val(txtConversion.Value) <> 0 Then
                parm(2) = New SqlParameter("@convrate", CType(txtConversion.Value, Decimal))
            Else
                parm(2) = New SqlParameter("@convrate", 0)
            End If
            If Val(txtSaleValue.Value) <> 0 Then
                parm(3) = New SqlParameter("@amount", CType(txtSaleValue.Value, String))
            Else
                parm(3) = New SqlParameter("@amount", 0)
            End If
            parm(4) = New SqlParameter("@servicecategory", String.Empty)

            parms.Add(parm(0))
            parms.Add(parm(1))
            parms.Add(parm(2))
            parms.Add(parm(3))
            parms.Add(parm(4))

            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_fill_freeforminvoice_income", parms)
            Return ds
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FreeForm_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

    Public Function FillCostgrid() As DataSet
        FillCostgrid = Nothing
        Try

            Dim ds As DataSet = New DataSet()
            Dim parms As New List(Of SqlParameter)
            Dim parm(0) As SqlParameter

            If txtpurchaseid.Value <> "" Then
                parm(0) = New SqlParameter("@docno", CType(txtpurchaseid.Value, String))
            Else
                parm(0) = New SqlParameter("@docno", String.Empty)
            End If
            parms.Add(parm(0))

            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_fill_freeforminvoice_cost", parms)
            Return ds
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FreeForm_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
    Public Function Validatedate() As Boolean
        Try

            Validatedate = True
            Dim fromdate As DateTime
            Dim todate As DateTime
            Dim MyCultureInfo As New CultureInfo("fr-Fr")
            fromdate = DateTime.Parse(dpFromCheckindate.txtDate.Text, MyCultureInfo, DateTimeStyles.None)
            todate = DateTime.Parse(dpFromCheckOut.txtDate.Text, MyCultureInfo, DateTimeStyles.None)
            If todate <= fromdate Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Departure Date should be greater than Arrival Date')", True)
                Validatedate = False
            End If

        Catch ex As Exception
            Validatedate = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FreeForm_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function



#Region "Public Function SaveGridincome()"
    Public Function SaveGridincome()
        If gv_IncomePosting.Rows.Count > 0 Then
            Dim gvrow As GridViewRow
            Dim dr As DataRow
            dt = CType(Session("income"), DataTable)
            Dim lblrequesttype As Label
            Dim lblrequestlineno As Label
            Dim ddlAccType As HtmlSelect
            Dim ddlAcctCode As HtmlSelect
            Dim ddlConAccCode As HtmlSelect
            Dim lblCurrCode As Label
            Dim txtExchRate, txtCredit, txtBaseCredit, txtBaseDebit, txtDebit As HtmlInputText
            Dim lblAccLineno As Label

            For Each gvrow In gv_IncomePosting.Rows
                lblrequesttype = gvrow.FindControl("lblrequesttype")
                lblrequestlineno = gvrow.FindControl("lblrequestlineno")
                lblAccLineno = gvrow.FindControl("lblAccLineno")
                ddlAccType = gvrow.FindControl("ddlAccType")
                ddlAcctCode = gvrow.FindControl("ddlAcctCode")
                ddlConAccCode = gvrow.FindControl("ddlConAccCode")
                lblCurrCode = gvrow.FindControl("lblCurrCode")
                txtExchRate = gvrow.FindControl("txtExchRate")
                txtDebit = gvrow.FindControl("txtDebit")
                txtCredit = gvrow.FindControl("txtCredit")
                txtBaseDebit = gvrow.FindControl("txtKWDDebit")
                txtBaseCredit = gvrow.FindControl("txtKWDCredit")
                For Each dr In dt.Rows
                    If dr("acc_lineno") = lblAccLineno.Text Then
                        dr("requesttype") = lblrequesttype.Text
                        dr("requestlineno") = lblrequestlineno.Text
                        dr("acc_lineno") = lblAccLineno.Text
                        dr("acc_type") = ddlAccType.Value
                        dr("acc_code") = ddlAcctCode.Items(ddlAcctCode.SelectedIndex).Text
                        dr("controlcode") = ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text
                        dr("currcode") = lblCurrCode.Text
                        dr("convrate") = txtExchRate.Value
                        dr("debit") = txtDebit.Value
                        dr("credit") = txtCredit.Value
                        dr("basedebit") = txtBaseDebit.Value
                        dr("basecredit") = txtBaseCredit.Value
                    End If
                Next
                dt.AcceptChanges()
            Next
            Session.Add("income", dt)
        End If
        SaveGridincome = ""
    End Function
#End Region

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        SaveGridincome()

        Dim i As Integer = 0
        Dim lblAccLineno As Label
        Dim AccLineno As String = "600"
        Dim maxtlineno As Integer = 600
        For Each gvRow In gv_IncomePosting.Rows
            lblAccLineno = CType(gvRow.FindControl("lblAccLineno"), Label)
            If lblAccLineno Is Nothing = False Then
                If lblAccLineno.Text <> "" Then
                    AccLineno = lblAccLineno.Text
                    If CType(lblAccLineno.Text, Integer) > maxtlineno Then
                        maxtlineno = CType(lblAccLineno.Text, Integer)
                    End If
                End If
            End If
        Next
        dt = CType(Session("income"), DataTable)

        Dim dr As DataRow
        Dim drnew As DataRow = dt.NewRow()

        drnew("requesttype") = "C"
        drnew("requestlineno") = 0
        drnew("acc_lineno") = maxtlineno + 1
        drnew("acc_type") = "G"
        drnew("acc_code") = "[Select]"
        drnew("controlcode") = "[Select]"
        drnew("currcode") = txtbasecurr.Value
        drnew("convrate") = "1.000000000000"
        drnew("debit") = 0
        drnew("credit") = 0
        drnew("basedebit") = 0
        drnew("basecredit") = 0
        dt.Rows.Add(drnew)
        dt.AcceptChanges()
        Session.Add("income", dt)

        txtmaxacclineno.Value = maxtlineno + 1
        txtTotalKWDCredit.Value = 0
        txtTotalKWDDebit.Value = 0

        BindData()
    End Sub

#Region "Public Function BindData()"
    Public Function BindData()
        If Session("income") Is Nothing = False Then
            dt = CType(Session("income"), DataTable)
            If dt Is Nothing = False Then

                gv_IncomePosting.DataSource = dt
                gv_IncomePosting.DataBind()
                gv_IncomePosting.Visible = True
                div_income.Style("height") = 200
                txtgridrowsip.Value = gv_IncomePosting.Rows.Count
                btnAdd.Visible = True
                btnDelLine.Visible = True

            End If
        Else
            FillSalesgrid()
        End If
        BindData = ""
    End Function
#End Region



    Protected Sub btnDelLine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelLine.Click
        SaveGridincome()
        dt = CType(Session("income"), DataTable)
        If dt.Rows.Count > 1 Then
            Dim gvrow As GridViewRow
            Dim chkSel As CheckBox
            Dim i As Integer = 0
            For Each gvrow In gv_IncomePosting.Rows
                chkSel = CType(gvrow.FindControl("chckDeletion"), CheckBox)
                If chkSel.Checked Then
                    i += 1
                End If
            Next
            If i < 1 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one row');", True)
                Return
            End If
            If i > 1 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select only one row');", True)
                Return
            End If
            Dim lblAccLineno As Label
            Dim AccLineno As String = ""
            For Each gvrow In gv_IncomePosting.Rows
                chkSel = CType(gvrow.FindControl("chckDeletion"), CheckBox)
                lblAccLineno = CType(gvrow.FindControl("lblAccLineno"), Label)
                If chkSel.Checked = True Then
                    AccLineno = lblAccLineno.Text
                    If lblAccLineno.Text <> "" Then
                        AccLineno = lblAccLineno.Text
                    End If
                End If
            Next
            dt = CType(Session("income"), DataTable)
            Dim dr As DataRow
            Dim drnew As DataRow = dt.NewRow()
            For Each dr In dt.Rows
                If dr("acc_lineno") = AccLineno Then
                    dt.Rows.Remove(dr)
                    Exit For
                End If
            Next
            dt.AcceptChanges()
            Session.Add("income", dt)
            BindData()
        Else
            gv_IncomePosting.Visible = False
            btnAdd.Visible = False
            btnDelLine.Visible = False
        End If
    End Sub

#Region "Public Sub fillgrddetail(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillgrddetail(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 1
        Else
            lngcnt = count
        End If

        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
    End Sub
#End Region

#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable

        dt.Columns.Add(New DataColumn("requesttype", GetType(String)))
        dt.Columns.Add(New DataColumn("SrNo", GetType(Integer)))
        dt.Columns.Add(New DataColumn("acc_type", GetType(String)))
        dt.Columns.Add(New DataColumn("acc_code", GetType(String)))
        dt.Columns.Add(New DataColumn("accname", GetType(String)))
        dt.Columns.Add(New DataColumn("currcode", GetType(String)))
        dt.Columns.Add(New DataColumn("convrate", GetType(Double)))
        dt.Columns.Add(New DataColumn("amount", GetType(Double)))
        dt.Columns.Add(New DataColumn("baseamount", GetType(Double)))

        Dim drnew As DataRow = dt.NewRow()

        drnew("requesttype") = "H"
        drnew("SrNo") = 1
        drnew("acc_type") = "S"
        drnew("acc_code") = "[Select]"
        drnew("accname") = "[Select]"
        drnew("currcode") = ""
        drnew("convrate") = 0
        drnew("amount") = 0
        drnew("baseamount") = 0
        dt.Rows.Add(drnew)
        dt.AcceptChanges()

        'For i = 1 To lngcount
        '    dr = dt.NewRow()
        '    dr(0) = i
        '    dt.Rows.Add(dr)
        'Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        Session.Add("purchase", dt)
        'End If
    End Function
#End Region

    Protected Sub gv_purchase_detail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_purchase_detail.RowDataBound
        Dim ddlACode As HtmlSelect
        Dim ddlAName As HtmlSelect
        Dim ddlAccType As HtmlSelect
        Dim txtcurrcode As HtmlInputText
        Dim txtExchRate As HtmlInputText
        Dim txtamount As HtmlInputText
        Dim txtbaseamount As HtmlInputText
        Dim hdnAccType As HiddenField
        Dim lblAcctype As Label
        Dim lblACode As Label
        Dim lblAName As Label
        Dim actype As String = ""

        Dim hdnAcctCode As HiddenField
        Dim hdnAcctName As HiddenField

        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then

                txtcurrcode = e.Row.FindControl("txtcurrcode")
                ddlAccType = e.Row.FindControl("ddlAccType")
                hdnAccType = e.Row.FindControl("hdnAccType")
                ddlACode = e.Row.FindControl("ddlAcctCode")
                ddlAName = e.Row.FindControl("ddlAcctName")
                txtExchRate = e.Row.FindControl("txtExchRate")
                txtamount = e.Row.FindControl("txtamount")
                txtbaseamount = e.Row.FindControl("txtbaseamount")
                lblACode = e.Row.FindControl("lblAcctCode")
                lblAName = e.Row.FindControl("lblAcctName")
                lblAcctype = e.Row.FindControl("lblAcctype")

                hdnAcctCode = e.Row.FindControl("hdnAcctCode")
                hdnAcctName = e.Row.FindControl("hdnAcctName")


                If ddlAccType Is Nothing = False Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccType, "acc_type_des", "acc_type_name", "select acc_type_des,acc_type_name from  acc_type_master where acc_type_mode<>'G' and acc_type_name<>'C' and acc_type_name<>'G' order by acc_type_name", True)
                End If
                ddlAccType.Value = hdnAccType.Value


                'If CType(lblAcctype.Text, String) = "" Then
                '    ddlAccType.Value = "A"
                '    lblAcctype.Text = ddlAccType.Value
                'Else
                '    ddlAccType.Value = lblAcctype.Text
                'End If

                actype = CType(lblAcctype.Text, String)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlACode, "code", "des", "select code,des from  view_account where type='" & actype & "' and controlyn='N' and type<>'C' and type<>'G' order by code", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAName, "des", "code", "select des,code from  view_account where type='" & actype & "' and controlyn='N' and type<>'C' and type<>'G' order by des", True)

                'Dim a As ListItem
                'ddlAName.Value = CType(lblACode.Text, String)
                'For Each a In ddlACode.Items
                '    If a.Text = CType(lblACode.Text, String) Then
                '        ddlACode.Value = a.Value
                '        Exit For
                '    End If
                'Next

                ddlACode.Items(ddlACode.SelectedIndex).Text = lblACode.Text
                ddlAName.Value = lblACode.Text

                hdnAcctCode.Value = lblACode.Text
                hdnAcctName.Value = ddlAName.Items(ddlAName.SelectedIndex).Text

                'lblACode = e.Row.FindControl("lblAcctCode")
                'lblAName = e.Row.FindControl("lblAcctName")

                'Dim typ As Type
                'typ = GetType(DropDownList)
                'If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                '    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                'End If

                'ddlACode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'ddlAName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                ddlAccType.Attributes.Add("onchange", "javascript:LoadAcc('" + CType(hdnAccType.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "')")
                ddlACode.Attributes.Add("onchange", "javascript:FillPDetails_code('" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(txtcurrcode.ClientID, String) + "','" + CType(txtExchRate.ClientID, String) + "','" + CType(hdnAcctCode.ClientID, String) + "','" + CType(hdnAcctName.ClientID, String) + "')")
                ddlAName.Attributes.Add("onchange", "javascript:FillPDetails_name('" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(txtcurrcode.ClientID, String) + "','" + CType(txtExchRate.ClientID, String) + "','" + CType(hdnAcctCode.ClientID, String) + "','" + CType(hdnAcctName.ClientID, String) + "')")

                txtExchRate.Attributes.Add("onchange", "javascript:calculatebaseamount('" + CType(txtamount.ClientID, String) + "','" + CType(txtbaseamount.ClientID, String) + "','" + CType(txtExchRate.ClientID, String) + "')")
                txtamount.Attributes.Add("onchange", "javascript:calculatebaseamount('" + CType(txtamount.ClientID, String) + "','" + CType(txtbaseamount.ClientID, String) + "','" + CType(txtExchRate.ClientID, String) + "')")
                NumbersHtml(txtExchRate)
                NumbersHtml(txtbaseamount)
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FreeForm_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#Region "Public Function Savepurchase_det()"
    Public Function Savepurchase_det()
        If gv_purchase_detail.Rows.Count > 0 Then

            Dim dr As DataRow
            dt = CType(Session("purchase"), DataTable)
            Dim lblsno1 As Label
            Dim ddlACode As HtmlSelect
            Dim ddlAName As HtmlSelect
            Dim ddlAccType As HtmlSelect
            Dim txtcurrcode As HtmlInputText
            Dim txtExchRate As HtmlInputText
            Dim txtamount As HtmlInputText
            Dim txtbaseamount As HtmlInputText
            Dim ddlservices As HtmlSelect

            For Each gvrow In gv_purchase_detail.Rows
                lblsno1 = gvRow.FindControl("lblsno")
                ddlAccType = gvRow.FindControl("ddlAccType")
                ddlACode = gvRow.FindControl("ddlAcctCode")
                ddlAName = gvRow.FindControl("ddlAcctName")
                txtcurrcode = gvRow.FindControl("txtcurrcode")
                txtExchRate = gvRow.FindControl("txtExchRate")
                txtamount = gvRow.FindControl("txtamount")
                txtbaseamount = gvRow.FindControl("txtbaseamount")
                ddlservices = gvRow.FindControl("ddlservices")

                For Each dr In dt.Rows
                    If dr("SrNo") = lblsno1.Text Then
                        dr("requesttype") = ddlservices.Value
                        dr("SrNo") = lblsno1.Text
                        dr("acc_type") = ddlAccType.Value
                        dr("acc_code") = ddlACode.Items(ddlACode.SelectedIndex).Text
                        '   dr("accname") = ddlAName.Items(ddlAName.SelectedIndex).Text
                        dr("currcode") = txtcurrcode.Value
                        dr("convrate") = txtExchRate.Value
                        dr("amount") = txtamount.Value
                        dr("baseamount") = txtbaseamount.Value
                    End If
                Next
                dt.AcceptChanges()
            Next
            Session.Add("purchase", dt)
        End If
        Savepurchase_det = ""
    End Function
#End Region

    Protected Sub btnAdd_det_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd_det.Click

        Savepurchase_det()

        Dim i As Integer = 0
        Dim lblsno As Label
        Dim sno As String = "1"
        Dim maxtlineno As Integer = 1
        For Each gvRow In gv_purchase_detail.Rows
            lblsno = CType(gvRow.FindControl("lblsno"), Label)
            If lblsno Is Nothing = False Then
                If lblsno.Text <> "" Then
                    sno = lblsno.Text
                    If CType(lblsno.Text, Integer) > maxtlineno Then
                        maxtlineno = CType(lblsno.Text, Integer)
                    End If
                End If
            End If
        Next
        dt = CType(Session("purchase"), DataTable)

        Dim drnew As DataRow = dt.NewRow()
        drnew("requesttype") = "H"
        drnew("SrNo") = maxtlineno + 1
        drnew("acc_type") = "[Select]"
        drnew("acc_code") = "[Select]"
        '  drnew("accname") = "[Select]"
        drnew("currcode") = ""
        drnew("convrate") = 0
        drnew("amount") = 0
        drnew("baseamount") = 0

        dt.Rows.Add(drnew)
        dt.AcceptChanges()
        Session.Add("purchase", dt)

        If Session("purchase") Is Nothing = False Then
            dt = CType(Session("purchase"), DataTable)
            If dt Is Nothing = False Then
                gv_purchase_detail.DataSource = dt
                gv_purchase_detail.DataBind()
                gv_purchase_detail.Visible = True
                btnAdd_det.Visible = True
                btnDelLine_det.Visible = True
            End If
        End If
    End Sub

    Protected Sub btnDelLine_det_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelLine_det.Click
        Savepurchase_det()
        dt = CType(Session("purchase"), DataTable)
        If dt.Rows.Count > 1 Then
            Dim gvrow As GridViewRow
            Dim chkSel As CheckBox
            Dim i As Integer = 0
            For Each gvrow In gv_purchase_detail.Rows
                chkSel = CType(gvrow.FindControl("chckDeletion"), CheckBox)
                If chkSel.Checked Then
                    i += 1
                End If
            Next
            If i < 1 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select one row');", True)
                Return
            End If
            If i > 1 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select only one row');", True)
                Return
            End If
            Dim lblsno As Label
            Dim SrNo As String = ""
            For Each gvrow In gv_purchase_detail.Rows
                chkSel = CType(gvrow.FindControl("chckDeletion"), CheckBox)
                lblsno = CType(gvrow.FindControl("lblsno"), Label)
                If chkSel.Checked = True Then
                    SrNo = lblsno.Text
                    If lblsno.Text <> "" Then
                        SrNo = lblsno.Text
                    End If
                End If
            Next
            dt = CType(Session("purchase"), DataTable)
            Dim dr As DataRow
            Dim drnew As DataRow = dt.NewRow()
            For Each dr In dt.Rows
                If dr("SrNo") = SrNo Then
                    dt.Rows.Remove(dr)
                    Exit For
                End If
            Next
            dt.AcceptChanges()
            Session.Add("purchase", dt)
            BindData_purchase()
            hdnSS.Value = 1
        Else
            gv_purchase_detail.Visible = False
            btnDelLine.Visible = False
            btnAdd_det.Visible = False
        End If
    End Sub


#Region "Public Function BindData_purchase()"
    Public Function BindData_purchase()
        If Session("purchase") Is Nothing = False Then
            dt = CType(Session("purchase"), DataTable)
            If dt Is Nothing = False Then

                gv_purchase_detail.DataSource = dt
                gv_purchase_detail.DataBind()
                gv_purchase_detail.Visible = True
                '     div_income.Style("height") = 200
                '     txtgridrowsip.Value = gv_purchase_detail.Rows.Count
                btnDelLine.Visible = True
                btnDelLine.Visible = True

            End If
        Else
            '     FillSalesgrid()
        End If
        BindData_purchase = ""
    End Function
#End Region
    Protected Sub btnfilldetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfilldetail.Click
        Try

            If validatePage_purchase() = False Then
                Exit Sub
            End If


            Dim strnarration As String
            strnarration = txtRequestNo.Text + ":" + "Customer Ref : " + txtReferenceNo.Text + dpFromCheckindate.txtDate.Text + ":" + dpFromCheckOut.txtDate.Text + ":" + txtGuestName.Text + ":" + txtNarration.Value

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start


            If ViewState("FreeFormState") = "New" Or ViewState("FreeFormState") = "Copy" Or ViewState("FreeFormState") = "Edit" Then
                Dim optionval As String
                If Trim(txtpurchaseid.Value) = "" Then
                    optionval = objUtils.GetAutoDocNo("PINVTMP", SqlConn, sqlTrans)
                    txtpurchaseid.Value = optionval.Trim
                End If

                myCommand = New SqlCommand("sp_addDel_purchase_detail_temp", SqlConn, sqlTrans)
                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = CType(txtpurchaseid.Value, String)
                myCommand.ExecuteNonQuery()

                txtTotalKWDCreditCP.Value = 0
                txtTotalKWDDebitCP.Value = 0
                txtPL.Value = 0

                Dim lblsno1 As Label
                Dim ddlACode As HtmlSelect
                Dim ddlAName As HtmlSelect
                Dim ddlAccType As HtmlSelect
                Dim txtcurrcode As HtmlInputText
                Dim txtExchRate As HtmlInputText
                Dim txtamount As HtmlInputText
                Dim txtbaseamount As HtmlInputText
                Dim ddlservices As HtmlSelect
                For Each gvRow In gv_purchase_detail.Rows
                    lblsno1 = gvRow.FindControl("lblsno")
                    ddlAccType = gvRow.FindControl("ddlAccType")
                    ddlACode = gvRow.FindControl("ddlAcctCode")
                    ddlAName = gvRow.FindControl("ddlAcctName")
                    txtcurrcode = gvRow.FindControl("txtcurrcode")
                    txtExchRate = gvRow.FindControl("txtExchRate")
                    txtamount = gvRow.FindControl("txtamount")
                    txtbaseamount = gvRow.FindControl("txtbaseamount")
                    ddlservices = gvRow.FindControl("ddlservices")
                    If ddlACode.Value <> "[Select]" And ddlAccType.Value <> "[Select]" Then

                        myCommand = New SqlCommand("sp_add_purchase_detail_temp", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = CType(txtpurchaseid.Value, String)
                        myCommand.Parameters.Add(New SqlParameter("@sno", SqlDbType.Int, 9)).Value = CType(lblsno1.Text, Integer)
                        myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 10)).Value = CType(ddlAccType.Value, String)
                        If ddlACode.Value <> "[Select]" Then
                            myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(ddlACode.Items(ddlACode.SelectedIndex).Text, String)
                        Else
                            myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = String.Empty
                        End If
                        If txtcurrcode.Value <> "" Then
                            myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 10)).Value = CType(txtcurrcode.Value, String)
                        Else
                            myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 10)).Value = ""
                        End If
                        If Val(txtExchRate.Value) <> 0 Then
                            myCommand.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 12, 8)).Value = CType(txtExchRate.Value, Decimal)
                        Else
                            myCommand.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal, 12, 8)).Value = CType(txtExchRate.Value, Decimal)
                        End If
                        myCommand.Parameters.Add(New SqlParameter("@amount", SqlDbType.Money)).Value = CType(txtamount.Value, Decimal)
                        myCommand.Parameters.Add(New SqlParameter("@baseamount", SqlDbType.Money)).Value = CType(txtbaseamount.Value, Decimal)
                        myCommand.Parameters.Add(New SqlParameter("@datein", SqlDbType.VarChar, 10)).Value = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(dpFromCheckindate.txtDate.Text), String)
                        myCommand.Parameters.Add(New SqlParameter("@dateout", SqlDbType.VarChar, 10)).Value = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(dpFromCheckOut.txtDate.Text), String)
                        myCommand.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 500)).Value = strnarration.Trim
                        myCommand.Parameters.Add(New SqlParameter("@requesttype", SqlDbType.Char, 1)).Value = ddlservices.Value

                        myCommand.ExecuteNonQuery()

                    End If
                Next
            End If

            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close

            Fillcost()

        Catch ex As Exception
            sqlTrans.Rollback()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("FreeForm_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#Region "Public Function Fillcost()"
    Public Function Fillcost()


        If gv_purchase_detail.Rows.Count > 0 Then

            Try
                'If Validatedate() = False Then
                '    Exit Function
                'End If

                Session.Add("cost", Nothing)
                'If Session("income") Is Nothing Then
                '    CreateTable()
                'End If

                Dim dsResult As New DataSet
                dsResult = FillCostgrid()
                If dsResult.Tables.Count > 0 Then
                    If dsResult.Tables(0).Rows.Count > 0 Then
                        gv_CostPosting.DataSource = dsResult.Tables(0)
                        gv_CostPosting.DataBind()
                        div_cost.Style("height") = 200
                        txtgridrowscp.Value = gv_CostPosting.Rows.Count
                        '    txtmaxacclineno.Value = dsResult.Tables(1).Rows(0)("acc_lineno")
                        Session.Add("cost", dsResult.Tables(0))
                        btnAdd.Visible = True
                        btnDelLine.Visible = True
                        hdnSS.Value = 0
                    End If
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("FreeForm_Invoice.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

            End Try

        End If
    End Function
#End Region

    Protected Sub gv_CostPosting_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_CostPosting.RowDataBound

        Dim lblCurrCode As Label
        Dim lblChkIn As Label
        Dim lblChkOut As Label

        Dim txtExchRate As HtmlInputText
        Dim txtcredit As HtmlInputText
        Dim txtKWDcredit As HtmlInputText
        Dim hdnCurrCode As HiddenField

        Dim ddlACode As HtmlSelect
        Dim ddlAName As HtmlSelect
        Dim ddlAccType As HtmlSelect


        Dim hdnAccType As HiddenField

        Dim lblCtrlAccCode As Label
        Dim ddlConAccCode As HtmlSelect
        Dim ddlConAccName As HtmlSelect
        Dim hdnConAccCode As HiddenField

        Dim txtERate As HtmlInputText

        Dim txtdbt As HtmlInputText
        Dim txtcrt As HtmlInputText
        Dim lblACode As Label
        Dim lblAName As Label
        Dim txtkwddbt As HtmlInputText
        Dim txtkwdcrt As HtmlInputText
        Dim lblAcctype As Label
        Dim actype As String = ""
        Dim sqlstr1 As String = ""
        Dim sqlstr2 As String = ""
        Dim txtnarration1 As HtmlInputText

        'Dim lngTotalDebit As Decimal
        'Dim lngTotalCredit As Decimal
        Try

            If (e.Row.RowType = DataControlRowType.Header) Then
                e.Row.Cells(10).Text = "Debit" + "[" + txtbasecurr.Value + "]"
                e.Row.Cells(11).Text = "Credit" + "[" + txtbasecurr.Value + "]"
            End If


            If (e.Row.RowType = DataControlRowType.DataRow) Then
                lblCurrCode = e.Row.FindControl("lblCurrCode")
                lblChkIn = e.Row.FindControl("lblCheckIn")
                lblChkOut = e.Row.FindControl("lblCheckOut")

                txtExchRate = e.Row.FindControl("txtExchRate")
                txtcredit = e.Row.FindControl("txtcredit")
                txtKWDcredit = e.Row.FindControl("txtKWDcredit")
                hdnCurrCode = e.Row.FindControl("hdnCurrCode")

                ddlACode = e.Row.FindControl("ddlAcctCode")
                ddlAName = e.Row.FindControl("ddlAcctName")
                ddlAccType = e.Row.FindControl("ddlAccType")
                hdnAccType = e.Row.FindControl("hdnAccType")

                lblCtrlAccCode = e.Row.FindControl("lblCtrlAccCode")
                ddlConAccCode = e.Row.FindControl("ddlConAccCode")
                ddlConAccName = e.Row.FindControl("ddlConAccName")
                hdnConAccCode = e.Row.FindControl("hdnConAccCode")

                txtERate = e.Row.FindControl("txtExchRate")

                lblACode = e.Row.FindControl("lblAcctCode")
                lblAName = e.Row.FindControl("lblAcctName")
                lblAcctype = e.Row.FindControl("lblAcctype")

                txtkwddbt = e.Row.FindControl("txtKWDDebit")
                txtkwdcrt = e.Row.FindControl("txtKWDCredit")

                txtdbt = e.Row.FindControl("txtDebit")
                txtcrt = e.Row.FindControl("txtCredit")
                txtnarration1 = e.Row.FindControl("txtnarration")

                If txtTotalKWDCreditCP.Value = "" Then
                    txtTotalKWDCreditCP.Value = 0
                End If
                If txtTotalKWDDebitCP.Value = "" Then
                    txtTotalKWDDebitCP.Value = 0
                End If

                If txtkwddbt.Value = "" Or txtkwddbt.Value = "&nbsp;" Then
                    txtkwddbt.Value = 0
                End If
                If txtkwdcrt.Value = "" Or txtkwdcrt.Value = "&nbsp;" Then
                    txtkwdcrt.Value = 0
                End If

                txtTotalKWDCreditCP.Value = Math.Round(CType(txtTotalKWDCreditCP.Value, Decimal) + CType(txtkwdcrt.Value, Decimal), 3)
                txtTotalKWDDebitCP.Value = Math.Round(CType(txtTotalKWDDebitCP.Value, Decimal) + CType(txtkwddbt.Value, Decimal), 3)

                txtPL.Value = objUtils.RoundwithParameternew(Session("dbconnectionName"), txtTotalKWDCredit.Value) - objUtils.RoundwithParameternew(Session("dbconnectionName"), txtTotalKWDDebitCP.Value)


                'txtTotalKWDCreditCP.Value = txtTotalKWDCreditCP.Value + CType(lblkwddbt.Text, Decimal)
                'txtTotalKWDDebitCP.Value = txtTotalKWDDebitCP.Value + CType(lblkwdcrt.Text, Decimal)
                txtERate.Disabled = False
                If CType(e.Row.Cells(GridCol.AccoutType).Text, String) = "G" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlACode, "code", "des", "select code,des from  view_account where type='G' and controlyn='N' order by code")
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAName, "des", "code", "select des,code from  view_account where type='G' and controlyn='N' order by des")
                    ddlACode.Disabled = False
                    ddlAName.Disabled = False
                    txtERate.Disabled = True
                Else
                    ''         actype = CType(e.Row.Cells(GridCol.AccoutType).Text, String)
                    actype = CType(lblAcctype.Text, String)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlACode, "code", "des", "select code,des from  view_account where type='" & actype & "' and controlyn='N' order by des")
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAName, "des", "code", "select des,code from  view_account where type='" & actype & "' and controlyn='N' order by des")
                    'ddlACode.Disabled = True
                    'ddlAName.Disabled = True
                    If txtbasecurr.Value = CType(e.Row.Cells(GridCol.CurrencyCode).Text, String) Then
                        txtERate.Disabled = True
                    End If
                End If

                If e.Row.Cells(GridCol.RequestType).Text <> "G" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlACode, "code", "des", "select code,des from  view_account where controlyn='N' order by code")
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAName, "des", "code", "select des,code from  view_account where controlyn='N' order by des")
                End If

                'ddlACode.Value = CType(lblAName.Text, String)
                'ddlAName.Value = CType(lblACode.Text, String)

                Dim a As ListItem
                ddlAName.Value = CType(lblACode.Text, String)
                For Each a In ddlACode.Items
                    If a.Text = CType(lblACode.Text, String) Then
                        ddlACode.Value = a.Value
                        Exit For
                    End If
                Next

                'ddlACode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"view_account", "des", "code", CType(lblACode.Text, String)) 'e.Row.Cells(3).Text
                'ddlAName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"view_account", "code", "des", CType(lblAName.Text, String))  'e.Row.Cells(3).Text

                'ddlACode.Attributes.Add("onchange", "javascript:FillAccCode1('" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "')")
                'ddlAName.Attributes.Add("onchange", "javascript:FillAccName1('" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "')")

                If txtnarration1.Value = "" Then
                    txtnarration1.Value = txtRequestNo.Text + ":" + "Customer Ref : " + txtReferenceNo.Text + dpFromCheckindate.txtDate.Text + ":" + dpFromCheckOut.txtDate.Text + ":" + txtGuestName.Text + ":" + txtNarration.Value
                End If

                lblChkIn.Text = Format(CType(dpFromCheckindate.txtDate.Text, Date), "yyyy/MM/dd")
                lblChkOut.Text = Format(CType(dpFromCheckOut.txtDate.Text, Date), "yyyy/MM/dd")


                If ddlAccType Is Nothing = False Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccType, "acc_type_des", "acc_type_name", "select acc_type_des,acc_type_name from  acc_type_master where acc_type_mode<>'G' order by acc_type_name", True)
                    ddlAccType.Value = lblAcctype.Text
                End If
                If e.Row.Cells(GridCol.RequestType).Text <> "G" Then
                    ddlConAccCode.Style("visibility") = "visible"
                    ddlConAccName.Style("visibility") = "visible"

                    lblCtrlAccCode.Style("visibility") = "hidden"

                    sqlstr1 = GetSql1(lblAcctype.Text, lblACode.Text)
                    sqlstr2 = GetSql2(lblAcctype.Text, lblACode.Text)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True)
                    ddlConAccName.Value = CType(lblCtrlAccCode.Text, String)
                    For Each a In ddlConAccCode.Items
                        If a.Text = CType(lblCtrlAccCode.Text, String) Then
                            ddlConAccCode.Value = a.Value
                            Exit For
                        End If
                    Next
                    ddlConAccCode.Disabled = True
                    ddlConAccName.Disabled = True


                    ddlAccType.Style("visibility") = "visible"
                    lblAcctype.Style("visibility") = "hidden"

                    ddlAccType.Attributes.Add("onchange", "javascript:LoadAcc('" + CType(hdnAccType.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "')")
                    ddlAccType.Value = lblAcctype.Text
                    'If lblAcctype.Text = "G" Then
                    '    ddlACode.Disabled = True
                    '    ddlAName.Disabled = True
                    'Else
                    '    ddlACode.Disabled = False
                    '    ddlAName.Disabled = False
                    'End If
                Else
                    lblCtrlAccCode.Style("visibility") = "visible"
                    ddlConAccCode.Style("visibility") = "hidden"
                    ddlConAccName.Style("visibility") = "hidden"

                    ddlAccType.Style("visibility") = "hidden"
                    lblAcctype.Style("visibility") = "visible"
                End If


                ddlConAccCode.Attributes.Add("onchange", "javascript:FillCode('" + CType(hdnConAccCode.ClientID, String) + "','" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "')")
                ddlConAccName.Attributes.Add("onchange", "javascript:FillName('" + CType(hdnConAccCode.ClientID, String) + "','" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "')")

                'ddlACode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"view_account", "des", "code", CType(lblACode.Text, String)) 'e.Row.Cells(3).Text
                'ddlAName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"view_account", "code", "des", CType(lblAName.Text, String))  'e.Row.Cells(3).Text


                ddlACode.Attributes.Add("onchange", "javascript:FillAccCode('" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "','" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(lblCurrCode.ClientID, String) + "','" + CType(txtExchRate.ClientID, String) + "','" + CType(txtcredit.ClientID, String) + "','" + CType(txtKWDcredit.ClientID, String) + "','" + CType(hdnCurrCode.ClientID, String) + "','" + CType(hdnConAccCode.ClientID, String) + "','" + e.Row.RowIndex.ToString() + "')")
                ddlAName.Attributes.Add("onchange", "javascript:FillAccName('" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "','" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(lblCurrCode.ClientID, String) + "','" + CType(txtExchRate.ClientID, String) + "','" + CType(txtcredit.ClientID, String) + "','" + CType(txtKWDcredit.ClientID, String) + "','" + CType(hdnCurrCode.ClientID, String) + "','" + CType(hdnConAccCode.ClientID, String) + "','" + e.Row.RowIndex.ToString() + "')")

                txtERate.Attributes.Add("onchange", "javascript:GetKWDDrCr('" + CType(txtdbt.ClientID, String) + "','" + CType(txtcrt.ClientID, String) + "','" + CType(txtkwddbt.ClientID, String) + "','" + CType(txtkwdcrt.ClientID, String) + "','" + CType(txtERate.ClientID, String) + "','" + CType("CP", String) + "')")
                txtdbt.Attributes.Add("onchange", "javascript:GetKWDDrCr('" + CType(txtdbt.ClientID, String) + "','" + CType(txtcrt.ClientID, String) + "','" + CType(txtkwddbt.ClientID, String) + "','" + CType(txtkwdcrt.ClientID, String) + "','" + CType(txtERate.ClientID, String) + "','" + CType("CP", String) + "')")
                txtcrt.Attributes.Add("onchange", "javascript:GetKWDDrCr('" + CType(txtdbt.ClientID, String) + "','" + CType(txtcrt.ClientID, String) + "','" + CType(txtkwddbt.ClientID, String) + "','" + CType(txtkwdcrt.ClientID, String) + "','" + CType(txtERate.ClientID, String) + "','" + CType("CP", String) + "')")
                NumbersHtml(txtERate)
                NumbersDecimalRoundHtml(txtdbt)
                NumbersDecimalRoundHtml(txtcrt)

                Dim typ As Type
                typ = GetType(DropDownList)
                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                End If
                ddlACode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                ddlAName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RequestForInvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnadd_CP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnadd_CP.Click
        SaveGridcost()

        Dim i As Integer = 0
        Dim lblrequestlineno As Label
        Dim requestlineno As String = "1"
        Dim maxtlineno As Integer = 1
        For Each gvRow In gv_CostPosting.Rows
            lblrequestlineno = CType(gvRow.FindControl("lblrequestlineno"), Label)
            If lblrequestlineno Is Nothing = False Then
                If lblrequestlineno.Text <> "" Then
                    requestlineno = lblrequestlineno.Text
                    If CType(lblrequestlineno.Text, Integer) > maxtlineno Then
                        maxtlineno = CType(lblrequestlineno.Text, Integer)
                    End If
                End If
            End If
        Next
        dt = CType(Session("cost"), DataTable)

        Dim dr As DataRow
        Dim drnew As DataRow = dt.NewRow()

        drnew("requesttype") = ""
        drnew("requestlineno") = 0
        drnew("acc_lineno") = maxtlineno + 1
        drnew("acc_type") = "G"
        drnew("acc_code") = "[Select]"
        drnew("controlcode") = "[Select]"
        drnew("currcode") = txtbasecurr.Value
        drnew("convrate") = "1.000000000000"
        drnew("debit") = 0
        drnew("credit") = 0
        drnew("basedebit") = 0
        drnew("basecredit") = 0
        dt.Rows.Add(drnew)
        dt.AcceptChanges()
        Session.Add("income", dt)

        txtmaxacclineno.Value = maxtlineno + 1
        txtTotalKWDCredit.Value = 0
        txtTotalKWDDebit.Value = 0

        BindData()
    End Sub

#Region "Public Function SaveGridcost()"
    Public Function SaveGridcost()
        If gv_IncomePosting.Rows.Count > 0 Then
            Dim gvrow As GridViewRow
            Dim dr As DataRow
            dt = CType(Session("cost"), DataTable)
            Dim lblrequesttype As Label
            Dim lblrequestlineno As Label
            Dim ddlAccType As HtmlSelect
            Dim ddlAcctCode As HtmlSelect
            Dim ddlConAccCode As HtmlSelect
            Dim lblCurrCode As Label
            Dim txtExchRate, txtCredit, txtBaseCredit, txtBaseDebit, txtDebit As HtmlInputText
            Dim lblAccLineno As Label

            For Each gvrow In gv_CostPosting.Rows
                lblrequesttype = gvrow.FindControl("lblrequesttype")
                lblrequestlineno = gvrow.FindControl("lblrequestlineno")
                lblAccLineno = gvrow.FindControl("lblAccLineno")
                ddlAccType = gvrow.FindControl("ddlAccType")
                ddlAcctCode = gvrow.FindControl("ddlAcctCode")
                ddlConAccCode = gvrow.FindControl("ddlConAccCode")
                lblCurrCode = gvrow.FindControl("lblCurrCode")
                txtExchRate = gvrow.FindControl("txtExchRate")
                txtDebit = gvrow.FindControl("txtDebit")
                txtCredit = gvrow.FindControl("txtCredit")
                txtBaseDebit = gvrow.FindControl("txtKWDDebit")
                txtBaseCredit = gvrow.FindControl("txtKWDCredit")
                For Each dr In dt.Rows
                    If dr("acc_lineno") = lblAccLineno.Text Then
                        dr("requesttype") = lblrequesttype.Text
                        dr("requestlineno") = lblrequestlineno.Text
                        dr("acc_lineno") = lblAccLineno.Text
                        dr("acc_type") = ddlAccType.Value
                        dr("acc_code") = ddlAcctCode.Items(ddlAcctCode.SelectedIndex).Text
                        dr("controlcode") = ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text
                        dr("currcode") = lblCurrCode.Text
                        dr("convrate") = txtExchRate.Value
                        dr("debit") = txtDebit.Value
                        dr("credit") = txtCredit.Value
                        dr("basedebit") = txtBaseDebit.Value
                        dr("basecredit") = txtBaseCredit.Value
                    End If
                Next
                dt.AcceptChanges()
            Next
            Session.Add("cost", dt)
        End If
        SaveGridcost = ""
    End Function
#End Region


End Class
