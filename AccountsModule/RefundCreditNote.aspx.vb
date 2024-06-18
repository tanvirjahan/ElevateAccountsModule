#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Globalization
#End Region
Partial Class AccountsModule_RefundCreditNote
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objdatetime As New clsDateTime
    Dim objUser As New clsUser
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    'For accounts posting
    Dim caccounts As clssave = Nothing
    Dim cacc As clsAccounts = Nothing
    Dim ctran As clstran = Nothing
    Dim csubtran As clsSubTran = Nothing
    Dim mbasecurrency As String = ""
    'For accounts posting
#End Region

#Region "Enum parameter"
    Enum parameter
        requestid = 0
        invoiceno = 1
    End Enum
#End Region

#Region "Enum EnumHeader"
    Enum EnumHeader
        requestid = 0
        requestdate = 1
        usercode = 2
        agentcode = 3
        agentname = 4
        plgrpcode = 5
        plgrpname = 6
        sellcode = 7
        sellname = 8
        currcode = 9
        convrate = 10
        agentsubuser = 11
        subusername = 12
        agentref = 13
    End Enum
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
    End Enum
#End Region
#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
    Public Sub NumbersHtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
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
    Public Sub TextLockhtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            txtconnection.Value = Session("dbconnectionName")
            ViewState.Add("ResState", Request.QueryString("State"))
            ViewState.Add("ResCreditNote", Request.QueryString("CreditNoteNo"))
            ViewState.Add("ResRequestId", Request.QueryString("RequestId"))
            If Page.IsPostBack = False Then
                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                txtbasecurr.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 457)
                dpCreditNoteDate.txtDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy")
                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
                'txtRequestId.Value = CType(ViewState("ResRequestId"), String)
                'txtInvoiceNo.Value = CType(ViewState("ResInvoiceNo"), String)''''''and isnull(ih.amended,0)=0
                'CheckPostUnpostRight(CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType(Session("AppName"), String), "Reservation\ReservationInvoiceSearch.aspx")
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRefundId, "refundreq", "refundreqid", "select h.refundreqid+'-'+a.des as refundreq,h.refundreqid as refundreqid from reservation_invoice_header ih,refund_request_header h left outer join view_account a on h.agentcode= a.code and type='C' where ih.requestid=h.requestid   and h.creditnoteno is null", True)

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


                If ViewState("ResState") = "Edit" Or ViewState("ResState") = "View" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRefundId, "refundreq", "refundreqid", "select h.refundreqid+'-'+a.des as refundreq,h.refundreqid as refundreqid from  refund_request_header h left outer join view_account a on h.agentcode= a.code and type='C'", True)
                    If ViewState("ResCreditNote") Is Nothing = False Then
                        LoadHeader()
                        FillAllData()
                    End If
                ElseIf ViewState("ResState") = "Cancel" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRefundId, "refundreq", "refundreqid", "select h.refundreqid+'-'+a.des as refundreq,h.refundreqid as refundreqid from refund_request_header h left outer join view_account a on h.agentcode= a.code and type='C'", True)
                    If ViewState("ResCreditNote") Is Nothing = False Then
                        LoadHeader()
                        FillAllData()
                        btnSave.Text = "Cancel"
                    End If
                ElseIf ViewState("ResState") = "UndoCancel" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRefundId, "refundreq", "refundreqid", "select h.refundreqid+'-'+a.des as refundreq,h.refundreqid as refundreqid from refund_request_header h left outer join view_account a on h.agentcode= a.code and type='C'", True)
                    If ViewState("ResCreditNote") Is Nothing = False Then
                        LoadHeader()
                        FillAllData()
                        btnSave.Text = "Undo"
                    End If

                End If
                DisableControl()
            End If
            btnReturn.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            btnSave.Attributes.Add("onclick", "return ValidatePage()")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RequestForInvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Private Sub LoadHeader()
        Dim strSql As String = "select  cr.creditnotedate,h.refundreqid,h.creditnoteno, h.requestid,ih.invoiceno,h.refundtype,h.agentcode,a.des agentname,h.currcode,"
        strSql += " cr.refundamount,cr.refundcharges,cr.othercharges from refund_request_header h left outer join view_account a "
        strSql += " on h.agentcode= a.code ,reservation_invoice_header ih ,reservation_creditnote_header cr  where ih.requestid = h.requestid And cr.creditnoteno = h.creditnoteno "
        strSql += " and h.creditnoteno='" + CType(ViewState("ResCreditNote"), String) + "'"

        Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strSql)
        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                If IsDBNull(ds.Tables(0).Rows(0)("creditnoteno")) = False Then
                    txtCreditNoteNo.Value = CType(ds.Tables(0).Rows(0)("creditnoteno"), String)
                End If
                If IsDBNull(ds.Tables(0).Rows(0)("creditnotedate")) = False Then
                    dpCreditNoteDate.txtDate.Text = CType(ds.Tables(0).Rows(0)("creditnotedate"), Date).ToShortDateString()
                End If
                If IsDBNull(ds.Tables(0).Rows(0)("refundreqid")) = False Then
                    ddlRefundId.Value = CType(ds.Tables(0).Rows(0)("refundreqid"), String)
                End If
                If IsDBNull(ds.Tables(0).Rows(0)("requestid")) = False Then
                    txtRequestId.Value = CType(ds.Tables(0).Rows(0)("requestid"), String)
                End If
                If IsDBNull(ds.Tables(0).Rows(0)("invoiceno")) = False Then
                    txtInvoiceNo.Value = CType(ds.Tables(0).Rows(0)("invoiceno"), String)
                End If
                If IsDBNull(ds.Tables(0).Rows(0)("refundtype")) = False Then
                    ddlRefundType.Value = CType(ds.Tables(0).Rows(0)("refundtype"), String)
                End If
                If IsDBNull(ds.Tables(0).Rows(0)("agentcode")) = False Then
                    txtCustomerCode.Value = CType(ds.Tables(0).Rows(0)("agentcode"), String)
                End If
                If IsDBNull(ds.Tables(0).Rows(0)("agentname")) = False Then
                    txtCustomerName.Value = CType(ds.Tables(0).Rows(0)("agentname"), String)
                End If
                If IsDBNull(ds.Tables(0).Rows(0)("currcode")) = False Then
                    txtCurrency.Value = CType(ds.Tables(0).Rows(0)("currcode"), String)
                End If
                Dim decTotal As Decimal = 0
                If IsDBNull(ds.Tables(0).Rows(0)("refundamount")) = False Then
                    txtRefundSaleValue.Value = CType(ds.Tables(0).Rows(0)("refundamount"), String)
                    decTotal = decTotal + CType(ds.Tables(0).Rows(0)("refundamount"), Decimal)
                End If
                If IsDBNull(ds.Tables(0).Rows(0)("refundcharges")) = False Then
                    txtRefundCharges.Value = CType(ds.Tables(0).Rows(0)("refundcharges"), String)
                    decTotal = decTotal + CType(ds.Tables(0).Rows(0)("refundcharges"), Decimal)
                End If
                If IsDBNull(ds.Tables(0).Rows(0)("othercharges")) = False Then
                    txtRefundOther.Value = CType(ds.Tables(0).Rows(0)("othercharges"), String)
                    decTotal = decTotal + CType(ds.Tables(0).Rows(0)("othercharges"), Decimal)
                End If
                txtRefundTotal.Value = decTotal.ToString()
                ddlRefundId.Disabled = True
                txtRequestId.Disabled = True
                txtInvoiceNo.Disabled = True
                ddlRefundType.Disabled = True
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                ddlRefundId.Disabled = True
                txtCurrency.Disabled = True
                'txtRefundCharges.Disabled = True
                'txtRefundSaleValue.Disabled = True
                'txtRefundOther.Disabled = True
                txtRefundTotal.Disabled = True
                dpCreditNoteDate.txtDate.Enabled = False
            End If
        End If
    End Sub

    Private Sub DisableControl()
        If ViewState("ResState") = "View" Then
            dpCreditNoteDate.Enabled = False
            gv_IncomePosting.Enabled = False
            gv_CostPosting.Enabled = False
            btnSave.Visible = False
            btnCancel.Visible = False
            btndisPos.Enabled = False
            txtRefundCharges.Disabled = False
            txtRefundOther.Disabled = False

            chkPost.Visible = False

        ElseIf ViewState("ResState") = "Cancel" Or ViewState("ResState") = "UndoCancel" Then
            dpCreditNoteDate.Enabled = False
            gv_IncomePosting.Enabled = False
            gv_CostPosting.Enabled = False
            btnSave.Visible = False
            btnCancel.Visible = False
            btndisPos.Enabled = False
            txtRefundCharges.Disabled = False
            txtRefundOther.Disabled = False
            chkPost.Visible = False
            BtnPrint.Visible = False
            btnSave.Visible = True
        End If

    End Sub
    Public Function doSearch() As DataSet
        doSearch = Nothing
        Try
            Dim parms As New List(Of SqlParameter)
            Dim i As Integer
            Dim parm(6) As SqlParameter

            If ddlRefundId.Value <> "" And ddlRefundId.Value <> "[Select]" Then
                parm(0) = New SqlParameter("@refundid", CType(ddlRefundId.Value.Trim, String))
            Else
                parm(0) = New SqlParameter("@refundid", String.Empty)
            End If
            If Not (txtRequestId.Value = "") Then
                parm(1) = New SqlParameter("@requestid", CType(txtRequestId.Value.Trim, String))
            Else
                parm(1) = New SqlParameter("@requestid", String.Empty)
            End If
            If Not (txtCreditNoteNo.Value = "") Then
                parm(2) = New SqlParameter("@creditnoteno", CType(txtCreditNoteNo.Value.Trim, String))
            Else
                parm(2) = New SqlParameter("@creditnoteno", String.Empty)
            End If
            If Not (txtRefundSaleValue.Value = "") Then
                parm(3) = New SqlParameter("@refundsalevalue", CType(txtRefundSaleValue.Value.Trim, String))
            Else
                parm(3) = New SqlParameter("@refundsalevalue", Decimal.Zero)
            End If
            If Not (txtRefundCharges.Value = "") Then
                parm(4) = New SqlParameter("@refundcharges", CType(txtRefundCharges.Value.Trim, String))
            Else
                parm(4) = New SqlParameter("@refundcharges", Decimal.Zero)
            End If
            If Not (txtRefundOther.Value = "") Then
                parm(5) = New SqlParameter("@othercharges", CType(txtRefundOther.Value.Trim, String))
            Else
                parm(5) = New SqlParameter("@othercharges", Decimal.Zero)
            End If

            For i = 0 To 5
                parms.Add(parm(i))
            Next

            Dim ds As New DataSet
            ds = objUtils.ExecuteQuerynew(Session("dbconnectionName"), "sp_get_request_creditnote", parms)
            Return ds
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RequestForInvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
    Private Sub FillAllData()
        Try
            Dim dsResult As New DataSet
            lblMessageIP.Visible = False
            dsResult = doSearch()
            If dsResult.Tables.Count > 0 Then
                If dsResult.Tables(0).Rows.Count > 0 Then
                    gv_IncomePosting.DataSource = dsResult.Tables(0)
                    gv_IncomePosting.Visible = True
                    gv_IncomePosting.DataBind()
                    txtgridrowsip.Value = gv_IncomePosting.Rows.Count
                    gv_IncomePosting.Enabled = False
                    lblMessageIP.Text = ""
                Else
                    gv_IncomePosting.Visible = False
                    lblMessageIP.Visible = True
                    lblMessageIP.Text = "No Records Found"
                End If
                If dsResult.Tables(1).Rows.Count > 0 Then
                    gv_CostPosting.DataSource = dsResult.Tables(1)
                    gv_CostPosting.Visible = True
                    gv_CostPosting.DataBind()
                    txtgridrowscp.Value = gv_CostPosting.Rows.Count
                    gv_CostPosting.Enabled = False
                    lblMessageCP.Text = ""
                Else
                    gv_CostPosting.Visible = False
                    lblMessageCP.Visible = True
                    lblMessageCP.Text = "No Records Found"
                End If
            Else
                If dsResult.Tables(0).Rows.Count <= 0 Then
                    gv_IncomePosting.Visible = False
                    lblMessageIP.Visible = True
                    lblMessageIP.Text = "No Records Found"
                    gv_CostPosting.Visible = False
                    lblMessageCP.Visible = True
                    lblMessageCP.Text = "No Records Found"
                End If
            End If
            'chkPost.Checked = False
            'If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"),"select isnull(post_state,'')  from reservation_invoice_header(nolock) where invoiceno='" & txtInvoiceNo.Value & "' and requestid='" & txtRequestId.Value & "'") = "P" Then
            '    chkPost.Checked = True
            '    lblPostmsg.Text = "Posted"
            '    lblPostmsg.ForeColor = Drawing.Color.Red
            '    chkPost.Checked = True
            'Else
            '    lblPostmsg.Text = "UnPosted"
            '    lblPostmsg.ForeColor = Drawing.Color.Green
            'End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RequestForInvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub gv_IncomePosting_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_IncomePosting.RowDataBound
        Dim ddlACode As HtmlSelect
        Dim ddlAName As HtmlSelect
        Dim ddlCtrlAName As HtmlSelect
        Dim txtERate As HtmlInputText
        Dim lblControlAcctCode As Label
        Dim hdnCtrlAccCode As HiddenField

        Dim txtdbt As HtmlInputText
        Dim txtcrt As HtmlInputText
        Dim lblACode As Label
        Dim lblAName As Label
        Dim txtkwddbt As HtmlInputText
        Dim txtkwdcrt As HtmlInputText
        Dim actype As String = ""
        'Dim lngTotalDebit As Decimal
        'Dim lngTotalCredit As Decimal
        Try
            If (e.Row.RowType = DataControlRowType.Header) Then
                e.Row.Cells(10).Text = "Debit" + "[" + txtbasecurr.Value + "]"
                e.Row.Cells(11).Text = "Credit" + "[" + txtbasecurr.Value + "]"
            End If


            If (e.Row.RowType = DataControlRowType.DataRow) Then

                ddlACode = e.Row.FindControl("ddlAcctCode")
                ddlAName = e.Row.FindControl("ddlAcctName")
                ddlCtrlAName = e.Row.FindControl("ddlCtrlAName")
                txtERate = e.Row.FindControl("txtExchRate")
                lblControlAcctCode = e.Row.FindControl("lblControlAcctCode")
                hdnCtrlAccCode = e.Row.FindControl("hdnCtrlAccCode")

                lblACode = e.Row.FindControl("lblAcctCode")
                lblAName = e.Row.FindControl("lblAcctName")
                txtkwddbt = e.Row.FindControl("txtKWDDebit")
                txtkwdcrt = e.Row.FindControl("txtKWDCredit")

                txtdbt = e.Row.FindControl("txtDebit")
                txtcrt = e.Row.FindControl("txtCredit")
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

                txtTotalKWDCredit.Value = Math.Round(CType(txtTotalKWDCredit.Value, Decimal) + CType(txtkwdcrt.Value, Decimal), 3, MidpointRounding.AwayFromZero)
                txtTotalKWDDebit.Value = Math.Round(CType(txtTotalKWDDebit.Value, Decimal) + CType(txtkwddbt.Value, Decimal), 3, MidpointRounding.AwayFromZero)

                txtERate.Disabled = False
                If CType(e.Row.Cells(GridCol.AccoutType).Text, String) = "G" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlACode, "code", "des", "select code,des from  view_account where type='G' and controlyn='N' order by code")
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAName, "des", "code", "select des,code from  view_account where type='G' and controlyn='N' order by des")
                    ''ddlACode.Disabled = False
                    ''ddlAName.Disabled = False 
                    ddlCtrlAName.Visible = False
                    lblControlAcctCode.Visible = False
                    txtERate.Disabled = True
                Else
                    actype = CType(e.Row.Cells(GridCol.AccoutType).Text, String)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlACode, "code", "des", "select code,des from  view_account where type='" & actype & "' and controlyn='N' order by code")
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAName, "des", "code", "select des,code from  view_account where type='" & actype & "' and controlyn='N' order by des")
                    ''ddlACode.Disabled = True
                    ''ddlAName.Disabled = True
                    If txtbasecurr.Value = CType(e.Row.Cells(GridCol.CurrencyCode).Text, String) Then
                        txtERate.Disabled = True
                    End If
                    If CType(e.Row.Cells(GridCol.AccoutType).Text, String) = "S" Or CType(e.Row.Cells(GridCol.AccoutType).Text, String) = "A" Then
                        ddlCtrlAName.Visible = True
                        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCtrlAName, "controlacctcode", "controlacctcode", "select p.controlacctcode from partymast p where p.partycode='" + lblACode.Text + "' union all select p.accrualacctcode from partymast p where p.partycode='" + lblACode.Text + "'")
                        If hdnPI.Value = 1 Then
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCtrlAName, "controlacctcode", "controlacctcode", " select p.controlacctcode from partymast p where p.partycode='" + lblACode.Text + "' union all  select p.controlacctcode from supplier_agents p where p.supagentcode='" + lblACode.Text + "' ")
                        Else
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCtrlAName, "accrualacctcode", "accrualacctcode", " select p.accrualacctcode from partymast p where p.partycode='" + lblACode.Text + "' union all  select p.accrualacctcode from supplier_agents p where p.supagentcode='" + lblACode.Text + "' ")
                        End If

                        ddlCtrlAName.Value = lblControlAcctCode.Text
                        lblControlAcctCode.Visible = False
                    Else
                        ddlCtrlAName.Visible = False
                        lblControlAcctCode.Visible = True
                    End If
                End If
                Dim strAcctType As String = CType(e.Row.Cells(GridCol.AccoutType).Text, String)
                ddlACode.Value = CType(lblAName.Text, String)
                ddlAName.Value = CType(lblACode.Text, String)

                'ddlACode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"view_account", "des", "code", CType(lblACode.Text, String)) 'e.Row.Cells(3).Text
                'ddlAName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"view_account", "code", "des", CType(lblAName.Text, String))  'e.Row.Cells(3).Text

                ddlACode.Attributes.Add("onchange", "javascript:FillAccCode('" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "','" + strAcctType + "','" + CType(ddlCtrlAName.ClientID, String) + "','" + CType(lblControlAcctCode.ClientID, String) + "','" + CType(hdnCtrlAccCode.ClientID, String) + "')")
                ddlAName.Attributes.Add("onchange", "javascript:FillAccName('" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "','" + strAcctType + "','" + CType(ddlCtrlAName.ClientID, String) + "','" + CType(lblControlAcctCode.ClientID, String) + "','" + CType(hdnCtrlAccCode.ClientID, String) + "')")

                txtERate.Attributes.Add("onchange", "javascript:GetKWDDrCr('" + CType(txtdbt.ClientID, String) + "','" + CType(txtcrt.ClientID, String) + "','" + CType(txtkwddbt.ClientID, String) + "','" + CType(txtkwdcrt.ClientID, String) + "','" + CType(txtERate.ClientID, String) + "','" + CType("IP", String) + "')")
                txtdbt.Attributes.Add("onchange", "javascript:GetKWDDrCr('" + CType(txtdbt.ClientID, String) + "','" + CType(txtcrt.ClientID, String) + "','" + CType(txtkwddbt.ClientID, String) + "','" + CType(txtkwdcrt.ClientID, String) + "','" + CType(txtERate.ClientID, String) + "','" + CType("IP", String) + "')")
                txtcrt.Attributes.Add("onchange", "javascript:GetKWDDrCr('" + CType(txtdbt.ClientID, String) + "','" + CType(txtcrt.ClientID, String) + "','" + CType(txtkwddbt.ClientID, String) + "','" + CType(txtkwdcrt.ClientID, String) + "','" + CType(txtERate.ClientID, String) + "','" + CType("IP", String) + "')")
                NumbersHtml(txtERate)
                NumbersDecimalRoundHtml(txtdbt)
                NumbersDecimalRoundHtml(txtcrt)

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RequestForInvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try


    End Sub

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

        Dim hdnCtrlAccCode As HiddenField

        Dim ddlCtrlAName As HtmlSelect
        Dim lblControlAcctCode As Label

        Dim ddlACode As HtmlSelect
        Dim strdiv, strcostcentercode, strglcode As String
        Dim acc_tranlinenno, acc_against_tranlinenno As Long
        acc_tranlinenno = 0
        acc_against_tranlinenno = 0
        Try
            If Page.IsValid = True Then
                If ViewState("ResState") = "Edit" Or ViewState("ResState") = "Delete" Or ViewState("ResState") = "Cancel" Or ViewState("ResState") = "UndoCancel" Then
                    If validate_BillAgainst() = False Then
                        Exit Sub
                    End If

                    If ViewState("ResState") = "New" Or ViewState("ResState") = "Edit" Or ViewState("ResState") = "Delete" Or ViewState("ResState") = "Cancel" Or ViewState("ResState") = "UndoCancel" Then
                        If Validateseal() = False Then
                            Exit Sub
                        End If
                    End If
                End If
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
                strcostcentercode = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 510)
                strglcode = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select controlacctcode from agentmast where agentcode='" & txtCustomerCode.Value & "'")
                If chkPost.Checked = True Then
                    'For Accounts posting
                    initialclass(mySqlConn, sqlTrans)
                    'For Accounts posting
                End If

                If ViewState("ResState") = "New" Or ViewState("ResState") = "Copy" Or ViewState("ResState") = "Edit" Then

                    If ViewState("ResState") = "New" Or ViewState("ResState") = "Copy" Then
                        txtCreditNoteNo.Value = objUtils.GetAutoDocNo("CRNOTE", mySqlConn, sqlTrans)
                        mySqlCmd = New SqlCommand("sp_add_reservation_creditnote_header", mySqlConn, sqlTrans)
                    ElseIf ViewState("ResState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_reservation_creditnote_header", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@creditnoteno", SqlDbType.VarChar, 20)).Value = CType(txtCreditNoteNo.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@creditnotetype", SqlDbType.VarChar, 10)).Value = CType("RCN", String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@creditnotedate", SqlDbType.DateTime)).Value = CType(objdatetime.ConvertDateromTextBoxToDatabase(dpCreditNoteDate.txtDate.Text), Date)
                    mySqlCmd.Parameters.Add(New SqlParameter("@invoiceno", SqlDbType.VarChar, 20)).Value = CType(txtInvoiceNo.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(txtRequestId.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@agentref", SqlDbType.VarChar, 20)).Value = CType(txtCustomerRef.Value.Trim, String)
                    If txtRefundSaleValue.Value <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@refundamount", SqlDbType.VarChar, 20)).Value = CType(txtRefundSaleValue.Value.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@refundamount", SqlDbType.VarChar, 20)).Value = "0"
                    End If
                    If txtRefundCharges.Value <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@refundcharges", SqlDbType.VarChar, 20)).Value = CType(txtRefundCharges.Value.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@refundcharges", SqlDbType.VarChar, 20)).Value = "0"
                    End If
                    If txtRefundOther.Value <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@othercharges", SqlDbType.VarChar, 20)).Value = CType(txtRefundOther.Value.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@othercharges", SqlDbType.VarChar, 20)).Value = "0"
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If chkPost.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "P"
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "U"
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@refundreqid", SqlDbType.VarChar, 20)).Value = CType(ddlRefundId.Value.Trim, String)

                    mySqlCmd.ExecuteNonQuery()

                    If chkPost.Checked = True Then
                        'For Accounts Posting
                        caccounts.clraccounts()
                        cacc.acc_tran_id = txtCreditNoteNo.Value
                        cacc.acc_tran_type = CType("RCN", String)
                        cacc.acc_tran_date = Format(CType(dpCreditNoteDate.txtDate.Text, Date), "yyyy/MM/dd")
                        cacc.acc_div_id = strdiv
                    End If

                    If ViewState("ResState") = "Edit" Then
                        If chkPost.Checked = False Then
                            mySqlCmd = New SqlCommand("sp_delvoucher", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "reservation_creditnote_header"
                            mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtCreditNoteNo.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType("RCN", String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                            mySqlCmd.ExecuteNonQuery()

                            'Updating the Refund Credit Note no. to show in the customer statement
                            mySqlCmd = New SqlCommand("sp_remove_recnno", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@creditnoteno", SqlDbType.VarChar, 20)).Value = CType(txtCreditNoteNo.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@invoiceno", SqlDbType.VarChar, 20)).Value = CType(txtInvoiceNo.Value.Trim, String)
                            mySqlCmd.ExecuteNonQuery()
                        End If

                        mySqlCmd = New SqlCommand("sp_del_reservation_creditnote_detail", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@creditnoteno", SqlDbType.VarChar, 20)).Value = CType(txtCreditNoteNo.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(txtRequestId.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()


                        myCommand = New SqlCommand("sp_del_open_detail_new", mySqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtCreditNoteNo.Value.Trim, String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType("RCN", String)
                        myCommand.ExecuteNonQuery()


                    End If
                    '--------------------------------------sp_add_reservation_invoice_detail for grid 1
                    For Each gvRow In gv_IncomePosting.Rows
                        mySqlCmd = New SqlCommand("sp_add_reservation_creditnote_detail", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@creditnoteno", SqlDbType.VarChar, 20)).Value = CType(txtCreditNoteNo.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@creditnotetype", SqlDbType.VarChar, 10)).Value = CType("RCN", String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@creditnotedate", SqlDbType.DateTime)).Value = CType(objdatetime.ConvertDateromTextBoxToDatabase(dpCreditNoteDate.txtDate.Text), Date)
                        mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(txtRequestId.Value.Trim, String)

                        lblALineno = gvRow.FindControl("lblAccLineno")
                        mySqlCmd.Parameters.Add(New SqlParameter("@acc_lineno", SqlDbType.Int, 9)).Value = CType(lblALineno.Text, Integer)

                        mySqlCmd.Parameters.Add(New SqlParameter("@requesttype", SqlDbType.VarChar, 1)).Value = CType(gvRow.Cells(GridCol.RequestType).Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@requestlineno", SqlDbType.Int, 9)).Value = CType(gvRow.Cells(GridCol.SNo).Text, Integer)


                        mySqlCmd.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.Char, 1)).Value = CType(gvRow.Cells(GridCol.AccoutType).Text, Char)
                        ddlACode = gvRow.FindControl("ddlAcctCode")
                        mySqlCmd.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(ddlACode.Items(ddlACode.SelectedIndex).Text, String)


                        If CType(gvRow.Cells(GridCol.AccoutType).Text, Char) = "C" Then
                            hdnCtrlAccCode = gvRow.FindControl("hdnCtrlAccCode")
                            mySqlCmd.Parameters.Add(New SqlParameter("@controlcode", SqlDbType.VarChar, 20)).Value = CType(hdnCtrlAccCode.Value, String)
                        ElseIf CType(gvRow.Cells(GridCol.AccoutType).Text, Char) = "S" Or CType(gvRow.Cells(GridCol.AccoutType).Text, Char) = "A" Then
                            ddlCtrlAName = gvRow.FindControl("ddlCtrlAName")
                            mySqlCmd.Parameters.Add(New SqlParameter("@controlcode", SqlDbType.VarChar, 20)).Value = CType(ddlCtrlAName.Value, String)
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@controlcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        End If

                        mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(gvRow.Cells(GridCol.CurrencyCode).Text, String)
                        txtERate = gvRow.FindControl("txtExchRate")

                        mySqlCmd.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal)).Value = CType(txtERate.Value, Decimal)


                        txtdbt = gvRow.FindControl("txtDebit")
                        mySqlCmd.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = CType(txtdbt.Value, Decimal)

                        txtkwddbt = gvRow.FindControl("txtKWDDebit")
                        mySqlCmd.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = CType(txtkwddbt.Value, Decimal)

                        txtcrt = gvRow.FindControl("txtCredit")
                        mySqlCmd.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = CType(txtcrt.Value, Decimal)

                        txtkwdcrt = gvRow.FindControl("txtKWDCredit")
                        mySqlCmd.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = CType(txtkwdcrt.Value, Decimal)

                        mySqlCmd.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 500)).Value = CType(gvRow.Cells(GridCol.Narration).Text, String)

                        lblSupName = gvRow.FindControl("lblSupplierName")
                        mySqlCmd.Parameters.Add(New SqlParameter("@partyname", SqlDbType.VarChar, 100)).Value = CType(lblSupName.Text, String)

                        lblChkIn = gvRow.FindControl("lblCheckIn")
                        mySqlCmd.Parameters.Add(New SqlParameter("@datein", SqlDbType.VarChar, 10)).Value = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(lblChkIn.Text), String)

                        lblChkOut = gvRow.FindControl("lblCheckOut")
                        mySqlCmd.Parameters.Add(New SqlParameter("@dateout", SqlDbType.VarChar, 10)).Value = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(lblChkOut.Text), String)

                        lblRCNo = gvRow.FindControl("lblReConfNo")
                        mySqlCmd.Parameters.Add(New SqlParameter("@reconfno", SqlDbType.VarChar, 100)).Value = CType(lblRCNo.Text, String)

                        lblRlNo = gvRow.FindControl("lblRlineNo")
                        mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int, 9)).Value = CType(lblRlNo.Text, Integer)

                        lblSLno = gvRow.FindControl("lblSLineno")
                        mySqlCmd.Parameters.Add(New SqlParameter("@slineno", SqlDbType.Int, 9)).Value = CType(lblSLno.Text, Integer)

                        mySqlCmd.ExecuteNonQuery()

                        acc_tranlinenno = CType(lblALineno.Text, Integer)
                        If chkPost.Checked = True Then
                            'Posting for the Grid Accounts
                            ctran = New clstran
                            ctran.acc_tran_id = cacc.acc_tran_id
                            ctran.acc_code = CType(ddlACode.Items(ddlACode.SelectedIndex).Text, String)
                            ctran.acc_type = CType(gvRow.Cells(GridCol.AccoutType).Text, Char)
                            ctran.acc_currency_id = CType(gvRow.Cells(GridCol.CurrencyCode).Text, String)
                            ctran.acc_currency_rate = CType(txtERate.Value, Decimal)
                            ctran.acc_div_id = strdiv
                            ctran.acc_narration = CType(gvRow.Cells(GridCol.Narration).Text, String)
                            ctran.acc_tran_date = cacc.acc_tran_date
                            ctran.acc_tran_lineno = acc_tranlinenno
                            ctran.acc_tran_type = cacc.acc_tran_type
                            If CType(gvRow.Cells(GridCol.AccoutType).Text, Char) <> "G" Then
                                If CType(gvRow.Cells(GridCol.AccoutType).Text, Char) = "C" Then
                                    hdnCtrlAccCode = gvRow.FindControl("hdnCtrlAccCode")
                                    ctran.pacc_gl_code = CType(hdnCtrlAccCode.Value, String)
                                ElseIf CType(gvRow.Cells(GridCol.AccoutType).Text, Char) = "S" Or CType(gvRow.Cells(GridCol.AccoutType).Text, Char) = "A" Then
                                    ddlCtrlAName = gvRow.FindControl("ddlCtrlAName")
                                    ctran.pacc_gl_code = CType(ddlCtrlAName.Value, String)
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

                            If CType(gvRow.Cells(GridCol.AccoutType).Text, Char) = "G" Then
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
                                csubtran.acc_field1 = ""
                                csubtran.acc_field2 = ""
                                csubtran.acc_field3 = ""
                                csubtran.acc_field4 = ""
                                csubtran.acc_field5 = ""
                                csubtran.acc_tran_id = cacc.acc_tran_id
                                csubtran.acc_tran_lineno = acc_tranlinenno
                                csubtran.acc_tran_type = cacc.acc_tran_type
                                csubtran.acc_narration = CType(gvRow.Cells(GridCol.Narration).Text, String)
                                csubtran.acc_type = CType(gvRow.Cells(GridCol.AccoutType).Text, Char)
                                csubtran.currate = CType(txtERate.Value, Decimal)
                                csubtran.costcentercode = IIf(ctran.acc_type = "G", strcostcentercode, "")
                                cacc.addsubtran(csubtran)
                            Else
                                Dim invoicedate As String
                                invoicedate = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select invoicedate from reservation_invoice_header where invoiceno='" & CType(txtInvoiceNo.Value.Trim, String) & "'")
                                myCommand = New SqlCommand("sp_add_open_detail_new", mySqlConn, sqlTrans)
                                myCommand.CommandType = CommandType.StoredProcedure
                                myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtInvoiceNo.Value.Trim, String)
                                myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "IN"
                                myCommand.Parameters.Add(New SqlParameter("@tran_date ", SqlDbType.DateTime)).Value = Format(CType(invoicedate, Date), "yyyy/MM/dd")
                                myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int)).Value = acc_tranlinenno

                                myCommand.Parameters.Add(New SqlParameter("@against_tran_id", SqlDbType.VarChar, 20)).Value = cacc.acc_tran_id
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int)).Value = acc_tranlinenno
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = cacc.acc_tran_type
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_date ", SqlDbType.DateTime)).Value = cacc.acc_tran_date
                                If invoicedate = "" Then
                                    myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = DBNull.Value
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = Format(CType(invoicedate, Date), "yyyy/MM/dd")
                                End If
                                myCommand.Parameters.Add(New SqlParameter("@open_sales_code", SqlDbType.VarChar, 10)).Value = "" 'DBNull.Value
                                myCommand.Parameters.Add(New SqlParameter("@open_debit", SqlDbType.Money)).Value = DecRound(CType(txtdbt.Value, Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@open_credit", SqlDbType.Money)).Value = DecRound(CType(txtcrt.Value, Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@open_field1", SqlDbType.VarChar, 100)).Value = CType(txtRequestId.Value.Trim, String)
                                myCommand.Parameters.Add(New SqlParameter("@open_field2", SqlDbType.VarChar, 100)).Value = CType(txtInvoiceNo.Value.Trim, String)
                                myCommand.Parameters.Add(New SqlParameter("@open_field3", SqlDbType.VarChar, 100)).Value = CType(gvRow.Cells(GridCol.Narration).Text, String)
                                myCommand.Parameters.Add(New SqlParameter("@open_field4", SqlDbType.VarChar, 100)).Value = ""
                                myCommand.Parameters.Add(New SqlParameter("@open_field5", SqlDbType.VarChar, 100)).Value = ""
                                myCommand.Parameters.Add(New SqlParameter("@open_mode", SqlDbType.Char, 1)).Value = "B"
                                myCommand.Parameters.Add(New SqlParameter("@open_exchg_diff", SqlDbType.Money)).Value = 0
                                myCommand.Parameters.Add(New SqlParameter("@dr_cr", SqlDbType.Char, 1)).Value = "C"
                                myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                                myCommand.Parameters.Add(New SqlParameter("@currency_rate", SqlDbType.Decimal)).Value = CType(txtERate.Value, Decimal)
                                myCommand.Parameters.Add(New SqlParameter("@base_debit", SqlDbType.Money)).Value = DecRound(CType(txtkwddbt.Value, Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@base_credit", SqlDbType.Money)).Value = DecRound(CType(txtkwdcrt.Value, Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.Char, 1)).Value = CType(gvRow.Cells(GridCol.AccoutType).Text, Char)
                                myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(ddlACode.Items(ddlACode.SelectedIndex).Text, String)
                                myCommand.Parameters.Add(New SqlParameter("@acc_gl_code", SqlDbType.VarChar, 20)).Value = ctran.pacc_gl_code
                                myCommand.ExecuteNonQuery()

                            End If
                            acc_tranlinenno = acc_tranlinenno + 1
                        End If
                    Next
                    '--------------------------------------sp_add_reservation_invoice_detail for grid 2

                    acc_against_tranlinenno = acc_tranlinenno
                    For Each gvRow In gv_CostPosting.Rows
                        mySqlCmd = New SqlCommand("sp_add_reservation_creditnote_detail", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure

                        mySqlCmd.Parameters.Add(New SqlParameter("@creditnoteno", SqlDbType.VarChar, 20)).Value = CType(txtCreditNoteNo.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@creditnotetype", SqlDbType.VarChar, 10)).Value = CType("RCN", String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@creditnotedate", SqlDbType.DateTime)).Value = CType(objdatetime.ConvertDateromTextBoxToDatabase(dpCreditNoteDate.txtDate.Text), Date)
                        mySqlCmd.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = CType(txtRequestId.Value.Trim, String)

                        lblALineno = gvRow.FindControl("lblAccLineno")
                        mySqlCmd.Parameters.Add(New SqlParameter("@acc_lineno", SqlDbType.Int, 9)).Value = CType(lblALineno.Text, Integer)

                        mySqlCmd.Parameters.Add(New SqlParameter("@requesttype", SqlDbType.VarChar, 1)).Value = CType(gvRow.Cells(GridCol.RequestType).Text, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@requestlineno", SqlDbType.Int, 9)).Value = CType(gvRow.Cells(GridCol.SNo).Text, Integer)
                        mySqlCmd.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.Char, 1)).Value = CType(gvRow.Cells(GridCol.AccoutType).Text, Char)
                        ddlACode = gvRow.FindControl("ddlAcctCode")
                        mySqlCmd.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(ddlACode.Items(ddlACode.SelectedIndex).Text, String)


                        If CType(gvRow.Cells(GridCol.AccoutType).Text, Char) = "C" Then
                            lblControlAcctCode = gvRow.FindControl("lblControlAcctCode")
                            mySqlCmd.Parameters.Add(New SqlParameter("@controlcode", SqlDbType.VarChar, 20)).Value = CType(lblControlAcctCode.Text, String)
                        ElseIf CType(gvRow.Cells(GridCol.AccoutType).Text, Char) = "S" Or CType(gvRow.Cells(GridCol.AccoutType).Text, Char) = "A" Then
                            ddlCtrlAName = gvRow.FindControl("ddlCtrlAName")
                            mySqlCmd.Parameters.Add(New SqlParameter("@controlcode", SqlDbType.VarChar, 20)).Value = CType(ddlCtrlAName.Value, String)
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@controlcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        End If

                        'If CType(gvRow.Cells(GridCol.AccoutType).Text, Char) <> "G" Then
                        '    mySqlCmd.Parameters.Add(New SqlParameter("@controlcode", SqlDbType.VarChar, 20)).Value = CType(gvRow.Cells(GridCol.ControlAccountCode).Text, String)
                        'Else
                        '    mySqlCmd.Parameters.Add(New SqlParameter("@controlcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        'End If


                        mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(gvRow.Cells(GridCol.CurrencyCode).Text, String)

                        txtERate = gvRow.FindControl("txtExchRate")
                        mySqlCmd.Parameters.Add(New SqlParameter("@convrate", SqlDbType.Decimal)).Value = CType(txtERate.Value, Decimal)

                        txtdbt = gvRow.FindControl("txtDebit")
                        mySqlCmd.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = CType(txtdbt.Value, Decimal)

                        txtkwddbt = gvRow.FindControl("txtKWDDebit")
                        mySqlCmd.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = CType(txtkwddbt.Value, Decimal)

                        txtcrt = gvRow.FindControl("txtCredit")
                        mySqlCmd.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = CType(txtcrt.Value, Decimal)

                        txtkwdcrt = gvRow.FindControl("txtKWDCredit")
                        mySqlCmd.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = CType(txtkwdcrt.Value, Decimal)

                        mySqlCmd.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 500)).Value = CType(gvRow.Cells(GridCol.Narration).Text, String)

                        lblSupName = gvRow.FindControl("lblSupplierName")
                        mySqlCmd.Parameters.Add(New SqlParameter("@partyname", SqlDbType.VarChar, 100)).Value = CType(lblSupName.Text, String)

                        lblChkIn = gvRow.FindControl("lblCheckIn")
                        mySqlCmd.Parameters.Add(New SqlParameter("@datein", SqlDbType.VarChar, 10)).Value = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(lblChkIn.Text), String)

                        lblChkOut = gvRow.FindControl("lblCheckOut")
                        mySqlCmd.Parameters.Add(New SqlParameter("@dateout", SqlDbType.VarChar, 10)).Value = CType(objdatetime.ConvertDateromTextBoxToTextYearMonthDay(lblChkOut.Text), String)

                        lblRCNo = gvRow.FindControl("lblReConfNo")
                        mySqlCmd.Parameters.Add(New SqlParameter("@reconfno", SqlDbType.VarChar, 100)).Value = CType(lblRCNo.Text, String)

                        lblRlNo = gvRow.FindControl("lblRlineNo")
                        mySqlCmd.Parameters.Add(New SqlParameter("@rlineno", SqlDbType.Int, 9)).Value = CType(lblRlNo.Text, Integer)

                        lblSLno = gvRow.FindControl("lblSLineno")
                        mySqlCmd.Parameters.Add(New SqlParameter("@slineno", SqlDbType.Int, 9)).Value = CType(lblSLno.Text, Integer)

                        mySqlCmd.ExecuteNonQuery()
                        acc_tranlinenno = CType(lblALineno.Text, Integer)
                        If chkPost.Checked = True Then
                            'Posting for the Grid Accounts
                            ctran = New clstran
                            ctran.acc_tran_id = cacc.acc_tran_id
                            ctran.acc_code = CType(ddlACode.Items(ddlACode.SelectedIndex).Text, String)
                            ctran.acc_type = CType(gvRow.Cells(GridCol.AccoutType).Text, Char)
                            ctran.acc_currency_id = CType(gvRow.Cells(GridCol.CurrencyCode).Text, String)
                            ctran.acc_currency_rate = CType(txtERate.Value, Decimal)
                            ctran.acc_div_id = strdiv
                            ctran.acc_narration = CType(gvRow.Cells(GridCol.Narration).Text, String)
                            ctran.acc_tran_date = cacc.acc_tran_date
                            ctran.acc_tran_lineno = acc_tranlinenno ' CType(lblRlNo.Text, Integer)
                            ctran.acc_tran_type = cacc.acc_tran_type
                            If CType(gvRow.Cells(GridCol.AccoutType).Text, Char) <> "G" Then
                                If CType(gvRow.Cells(GridCol.AccoutType).Text, Char) = "C" Then
                                    lblControlAcctCode = gvRow.FindControl("lblControlAcctCode")
                                    ctran.pacc_gl_code = CType(lblControlAcctCode.Text, String)
                                ElseIf CType(gvRow.Cells(GridCol.AccoutType).Text, Char) = "S" Or CType(gvRow.Cells(GridCol.AccoutType).Text, Char) = "A" Then
                                    ddlCtrlAName = gvRow.FindControl("ddlCtrlAName")
                                    ctran.pacc_gl_code = CType(ddlCtrlAName.Value, String)
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

                            If ctran.acc_type = "G" Then
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
                                csubtran.acc_field1 = ""
                                csubtran.acc_field2 = ""
                                csubtran.acc_field3 = ""
                                csubtran.acc_field4 = ""
                                csubtran.acc_field5 = ""
                                csubtran.acc_tran_id = cacc.acc_tran_id
                                csubtran.acc_tran_lineno = acc_tranlinenno ' CType(lblRlNo.Text, Integer)
                                csubtran.acc_tran_type = cacc.acc_tran_type
                                csubtran.acc_narration = CType(gvRow.Cells(GridCol.Narration).Text, String)
                                csubtran.acc_type = CType(gvRow.Cells(GridCol.AccoutType).Text, Char)
                                csubtran.currate = CType(txtERate.Value, Decimal)
                                csubtran.costcentercode = IIf(ctran.acc_type = "G", strcostcentercode, "")
                                cacc.addsubtran(csubtran)
                                acc_tranlinenno = acc_tranlinenno + 1
                            Else
                                Dim invoicedate As String
                                invoicedate = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select invoicedate from reservation_invoice_header where invoiceno='" & CType(txtInvoiceNo.Value.Trim, String) & "'")
                                myCommand = New SqlCommand("sp_add_open_detail_new", mySqlConn, sqlTrans)
                                myCommand.CommandType = CommandType.StoredProcedure
                                myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtInvoiceNo.Value.Trim, String)
                                myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "IN"
                                myCommand.Parameters.Add(New SqlParameter("@tran_date ", SqlDbType.DateTime)).Value = Format(CType(invoicedate, Date), "yyyy/MM/dd")
                                myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int)).Value = acc_tranlinenno

                                myCommand.Parameters.Add(New SqlParameter("@against_tran_id", SqlDbType.VarChar, 20)).Value = cacc.acc_tran_id
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int)).Value = acc_tranlinenno
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = cacc.acc_tran_type
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_date ", SqlDbType.DateTime)).Value = cacc.acc_tran_date
                                If invoicedate = "" Then
                                    myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = DBNull.Value
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = Format(CType(invoicedate, Date), "yyyy/MM/dd")
                                End If
                                myCommand.Parameters.Add(New SqlParameter("@open_sales_code", SqlDbType.VarChar, 10)).Value = "" 'DBNull.Value
                                myCommand.Parameters.Add(New SqlParameter("@open_debit", SqlDbType.Money)).Value = DecRound(CType(txtdbt.Value, Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@open_credit", SqlDbType.Money)).Value = DecRound(CType(txtcrt.Value, Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@open_field1", SqlDbType.VarChar, 100)).Value = CType(txtRequestId.Value.Trim, String)
                                myCommand.Parameters.Add(New SqlParameter("@open_field2", SqlDbType.VarChar, 100)).Value = CType(txtInvoiceNo.Value.Trim, String)
                                myCommand.Parameters.Add(New SqlParameter("@open_field3", SqlDbType.VarChar, 100)).Value = CType(gvRow.Cells(GridCol.Narration).Text, String)
                                myCommand.Parameters.Add(New SqlParameter("@open_field4", SqlDbType.VarChar, 100)).Value = ""
                                myCommand.Parameters.Add(New SqlParameter("@open_field5", SqlDbType.VarChar, 100)).Value = ""
                                myCommand.Parameters.Add(New SqlParameter("@open_mode", SqlDbType.Char, 1)).Value = "B"
                                myCommand.Parameters.Add(New SqlParameter("@open_exchg_diff", SqlDbType.Money)).Value = 0
                                myCommand.Parameters.Add(New SqlParameter("@dr_cr", SqlDbType.Char, 1)).Value = "D"
                                myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                                myCommand.Parameters.Add(New SqlParameter("@currency_rate", SqlDbType.Decimal)).Value = CType(txtERate.Value, Decimal)
                                myCommand.Parameters.Add(New SqlParameter("@base_debit", SqlDbType.Money)).Value = DecRound(CType(txtkwddbt.Value, Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@base_credit", SqlDbType.Money)).Value = DecRound(CType(txtkwdcrt.Value, Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.Char, 1)).Value = CType(gvRow.Cells(GridCol.AccoutType).Text, Char)
                                myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(ddlACode.Items(ddlACode.SelectedIndex).Text, String)
                                myCommand.Parameters.Add(New SqlParameter("@acc_gl_code", SqlDbType.VarChar, 20)).Value = ctran.pacc_gl_code
                                myCommand.ExecuteNonQuery()
                            End If
                        End If


                    Next

                    'Updating the Refund Credit Note no. to show in the customer statement
                    If ViewState("ResState") = "New" Or ViewState("ResState") = "Edit" Then
                        If chkPost.Checked = True Then
                            mySqlCmd = New SqlCommand("sp_update_recnno", mySqlConn, sqlTrans)
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@creditnoteno", SqlDbType.VarChar, 20)).Value = CType(txtCreditNoteNo.Value.Trim, String)
                            mySqlCmd.Parameters.Add(New SqlParameter("@invoiceno", SqlDbType.VarChar, 20)).Value = CType(txtInvoiceNo.Value.Trim, String)
                            mySqlCmd.ExecuteNonQuery()
                        End If
                    End If


                    If chkPost.Checked = True Then
                        'For Accounts Posting
                        cacc.table_name = ""
                        caccounts.Addaccounts(cacc)
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

                ElseIf ViewState("ResState") = "Cancel" Then

                    If chkPost.Checked = False Then
                        mySqlCmd = New SqlCommand("sp_delvoucher", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "reservation_creditnote_header"
                        mySqlCmd.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtCreditNoteNo.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType("RCN", String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                        mySqlCmd.ExecuteNonQuery()
                    End If

                    mySqlCmd = New SqlCommand("sp_cancel_reservation_creditnote", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@creditnoteno", SqlDbType.VarChar, 20)).Value = CType(txtCreditNoteNo.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()


                    myCommand = New SqlCommand("sp_cancel_reservation_creditnote_open_detail_new", mySqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtCreditNoteNo.Value.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType("RCN", String)
                    myCommand.ExecuteNonQuery()

                    'Updating the Refund Credit Note no. to show in the customer statement
                    mySqlCmd = New SqlCommand("sp_remove_recnno", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@creditnoteno", SqlDbType.VarChar, 20)).Value = CType(txtCreditNoteNo.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@invoiceno", SqlDbType.VarChar, 20)).Value = CType(txtInvoiceNo.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("ResState") = "UndoCancel" Then
                    mySqlCmd = New SqlCommand("sp_undocancel_reservation_creditnote", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@creditnoteno", SqlDbType.VarChar, 20)).Value = CType(txtCreditNoteNo.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()                                           'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)                      ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)                       'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)                   'connection close
                'Change 12/11/2008 ****************************
                'Response.Redirect("ReservationInvoice.aspx", False)
                'Response.Redirect("InvoicePrint.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('ReservationWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                'Change 12/11/2008 ****************************
                'btnSave.Visible = False
                'BtnPrint.Visible = True
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)                      ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)                       'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)                   'connection close
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RequestForInvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
        'Change 12/11/2008 ****************************
        'Response.Redirect("InvoicePrint.aspx", False)
        'Change 12/11/2008 ****************************
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("ReservationInvoice.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub

    Protected Sub gv_CostPosting_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_CostPosting.RowDataBound
        Dim ddlACode As HtmlSelect
        Dim ddlAName As HtmlSelect
        Dim txtERate As HtmlInputText
        Dim ddlCtrlAName As HtmlSelect
        Dim lblControlAcctCode As Label
        Dim hdnCtrlAccCode As HiddenField

        Dim txtdbt As HtmlInputText
        Dim txtcrt As HtmlInputText
        Dim lblACode As Label
        Dim lblAName As Label
        Dim txtkwddbt As HtmlInputText
        Dim txtkwdcrt As HtmlInputText
        Dim actype As String = ""

        'Dim lngTotalDebit As Decimal
        'Dim lngTotalCredit As Decimal
        Try

            If (e.Row.RowType = DataControlRowType.Header) Then
                e.Row.Cells(10).Text = "Debit" + "[" + txtbasecurr.Value + "]"
                e.Row.Cells(11).Text = "Credit" + "[" + txtbasecurr.Value + "]"
            End If

            If (e.Row.RowType = DataControlRowType.DataRow) Then

                ddlACode = e.Row.FindControl("ddlAcctCode")
                ddlAName = e.Row.FindControl("ddlAcctName")
                txtERate = e.Row.FindControl("txtExchRate")
                ddlCtrlAName = e.Row.FindControl("ddlCtrlAName")
                lblControlAcctCode = e.Row.FindControl("lblControlAcctCode")

                hdnCtrlAccCode = e.Row.FindControl("hdnCtrlAccCode")

                lblACode = e.Row.FindControl("lblAcctCode")
                lblAName = e.Row.FindControl("lblAcctName")

                txtkwddbt = e.Row.FindControl("txtKWDDebit")
                txtkwdcrt = e.Row.FindControl("txtKWDCredit")

                txtdbt = e.Row.FindControl("txtDebit")
                txtcrt = e.Row.FindControl("txtCredit")

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

                txtTotalKWDCreditCP.Value = Math.Round(CType(txtTotalKWDCreditCP.Value, Decimal) + CType(txtkwdcrt.Value, Decimal), 3, MidpointRounding.AwayFromZero)
                txtTotalKWDDebitCP.Value = Math.Round(CType(txtTotalKWDDebitCP.Value, Decimal) + CType(txtkwddbt.Value, Decimal), 3, MidpointRounding.AwayFromZero)
                If txtTotalKWDCredit.Value <> "" And txtTotalKWDDebitCP.Value <> "" Then
                    txtPL.Value = objUtils.RoundwithParameternew(Session("dbconnectionName"), txtTotalKWDCredit.Value) - objUtils.RoundwithParameternew(Session("dbconnectionName"), txtTotalKWDDebitCP.Value)
                End If


                'txtTotalKWDCreditCP.Value = txtTotalKWDCreditCP.Value + CType(lblkwddbt.Text, Decimal)
                'txtTotalKWDDebitCP.Value = txtTotalKWDDebitCP.Value + CType(lblkwdcrt.Text, Decimal)
                txtERate.Disabled = False
                If CType(e.Row.Cells(GridCol.AccoutType).Text, String) = "G" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlACode, "code", "des", "select code,des from  view_account where type='G' and controlyn='N' order by code")
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAName, "des", "code", "select des,code from  view_account where type='G' and controlyn='N' order by des")
                    'ddlACode.Disabled = False
                    'ddlAName.Disabled = False
                    txtERate.Disabled = True
                    ddlCtrlAName.Visible = False
                    lblControlAcctCode.Visible = False
                Else
                    actype = CType(e.Row.Cells(GridCol.AccoutType).Text, String)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlACode, "code", "des", "select code,des from  view_account where type='" & actype & "' and controlyn='N' order by des")
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAName, "des", "code", "select des,code from  view_account where type='" & actype & "' and controlyn='N' order by des")
                    'ddlACode.Disabled = True
                    'ddlAName.Disabled = True
                    If txtbasecurr.Value = CType(e.Row.Cells(GridCol.CurrencyCode).Text, String) Then
                        txtERate.Disabled = True
                    End If
                    If CType(e.Row.Cells(GridCol.AccoutType).Text, String) = "S" Or CType(e.Row.Cells(GridCol.AccoutType).Text, String) = "A" Then
                        ddlCtrlAName.Visible = True
                        ' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCtrlAName, "controlacctcode", "controlacctcode", " select p.controlacctcode from partymast p where p.partycode='" + lblACode.Text + "' union all  select p.controlacctcode from supplier_agents p where p.supagentcode='" + lblACode.Text + "' ")
                        If hdnPI.Value = 1 Then
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCtrlAName, "controlacctcode", "controlacctcode", " select p.controlacctcode from partymast p where p.partycode='" + lblACode.Text + "' union all  select p.controlacctcode from supplier_agents p where p.supagentcode='" + lblACode.Text + "' ")
                        Else
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCtrlAName, "accrualacctcode", "accrualacctcode", " select p.accrualacctcode from partymast p where p.partycode='" + lblACode.Text + "' union all  select p.accrualacctcode from supplier_agents p where p.supagentcode='" + lblACode.Text + "' ")
                        End If
                        ddlCtrlAName.Value = lblControlAcctCode.Text
                        lblControlAcctCode.Visible = False
                    Else
                        ddlCtrlAName.Visible = False
                        lblControlAcctCode.Visible = True
                    End If
                    End If
                ddlACode.Value = CType(lblAName.Text, String)
                ddlAName.Value = CType(lblACode.Text, String)
                Dim strAcctType As String = CType(e.Row.Cells(GridCol.AccoutType).Text, String)

                'ddlACode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"view_account", "des", "code", CType(lblACode.Text, String)) 'e.Row.Cells(3).Text
                'ddlAName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"view_account", "code", "des", CType(lblAName.Text, String))  'e.Row.Cells(3).Text

                'ddlACode.Attributes.Add("onchange", "javascript:FillAccCode('" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "')")
                'ddlAName.Attributes.Add("onchange", "javascript:FillAccName('" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "')")

                ddlACode.Attributes.Add("onchange", "javascript:FillAccCode('" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "','" + strAcctType + "','" + CType(ddlCtrlAName.ClientID, String) + "','" + CType(lblControlAcctCode.ClientID, String) + "','" + CType(hdnCtrlAccCode.ClientID, String) + "')")
                ddlAName.Attributes.Add("onchange", "javascript:FillAccName('" + CType(ddlACode.ClientID, String) + "','" + CType(ddlAName.ClientID, String) + "','" + strAcctType + "','" + CType(ddlCtrlAName.ClientID, String) + "','" + CType(lblControlAcctCode.ClientID, String) + "','" + CType(hdnCtrlAccCode.ClientID, String) + "')")


                txtERate.Attributes.Add("onchange", "javascript:GetKWDDrCr('" + CType(txtdbt.ClientID, String) + "','" + CType(txtcrt.ClientID, String) + "','" + CType(txtkwddbt.ClientID, String) + "','" + CType(txtkwdcrt.ClientID, String) + "','" + CType(txtERate.ClientID, String) + "','" + CType("CP", String) + "')")
                txtdbt.Attributes.Add("onchange", "javascript:GetKWDDrCr('" + CType(txtdbt.ClientID, String) + "','" + CType(txtcrt.ClientID, String) + "','" + CType(txtkwddbt.ClientID, String) + "','" + CType(txtkwdcrt.ClientID, String) + "','" + CType(txtERate.ClientID, String) + "','" + CType("CP", String) + "')")
                txtcrt.Attributes.Add("onchange", "javascript:GetKWDDrCr('" + CType(txtdbt.ClientID, String) + "','" + CType(txtcrt.ClientID, String) + "','" + CType(txtkwddbt.ClientID, String) + "','" + CType(txtkwdcrt.ClientID, String) + "','" + CType(txtERate.ClientID, String) + "','" + CType("CP", String) + "')")
                NumbersHtml(txtERate)
                NumbersDecimalRoundHtml(txtdbt)
                NumbersDecimalRoundHtml(txtcrt)

            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RequestForInvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturn.Click
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ReservationWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        'Response.Redirect("ReservationInvoiceSearch.aspx", False)
    End Sub
    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal
        Rdamt = Math.Round(Val(Ramt), CType(txtdecimal.Value, Integer), MidpointRounding.AwayFromZero)
        Return Rdamt
    End Function

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ReservationInvoice','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Private Sub initialclass(ByVal con As SqlConnection, ByVal stran As SqlTransaction)
        caccounts = Nothing
        cacc = Nothing
        ctran = Nothing
        csubtran = Nothing
        caccounts = New clssave
        cacc = New clsAccounts
        cacc.clropencol()
        cacc.tran_mode = IIf(ViewState("ResState") = "New", 1, 2)
        mbasecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
        cacc.start()
    End Sub
    Public Function Validateseal() As Boolean
        Try

            Validateseal = True
            Dim invdate As DateTime
            Dim sealdate As DateTime
            Dim MyCultureInfo As New CultureInfo("fr-Fr")
            invdate = DateTime.Parse(dpCreditNoteDate.txtDate.Text, MyCultureInfo, DateTimeStyles.None)
            sealdate = DateTime.Parse(txtpdate.Text, MyCultureInfo, DateTimeStyles.None)
            If invdate <= sealdate Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed in this period cannot make entry.Close the entry and make with another date')", True)
                Validateseal = False
            End If

        Catch ex As Exception
            Validateseal = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Refundcreditnote.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
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
            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtCreditNoteNo.Value
            myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType("RCN", String)

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
            objUtils.WritErrorLog("Receipts.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try
    End Function
    Private Sub CheckPostUnpostRight(ByVal UserName As String, ByVal UserPwd As String, ByVal AppName As String, ByVal PageName As String)
        Dim intGroupID As Integer
        Dim intAppID As Integer
        Dim intMenuID As Integer
        Dim strUserFunctionalRight As String = ""
        Dim strTempUserFunctionalRight As String()
        Dim lngCount As Int16
        Dim strGetUserFunctionalRightValue As String
        Dim PostUnpostFlag As Boolean = False

        If CType(UserName, String) = "" Or CType(UserPwd, String) = "" Then
            Exit Sub
        Else
            intGroupID = objUser.GetGroupId(Session("dbconnectionName"), CType(UserName, String), CType(UserPwd, String))
        End If
        If CType(AppName, String) = "" Or CType(AppName, String) = Nothing Then
            Exit Sub
        Else
            intAppID = objUser.GetAppId(Session("dbconnectionName"), CType(AppName, String))
        End If
        intMenuID = objUser.GetMenuId(Session("dbconnectionName"), PageName, intAppID)

        If Val(intGroupID) = 0 And Val(intAppID) = 0 And Val(intMenuID) = 0 Then
        Else
            strUserFunctionalRight = objUser.GetUserFunctionalRight(Session("dbconnectionName"), intGroupID, intAppID, intMenuID)
            If strUserFunctionalRight <> "" Then
                strTempUserFunctionalRight = strUserFunctionalRight.Split(";")
                For lngCount = 0 To strTempUserFunctionalRight.Length - 1
                    strGetUserFunctionalRightValue = strTempUserFunctionalRight.GetValue(lngCount)
                    If CType(strGetUserFunctionalRightValue, String) = "08" Or CType(strGetUserFunctionalRightValue, String) = "8" Then
                        PostUnpostFlag = True
                    End If
                Next
            Else
                PostUnpostFlag = False
            End If
        End If

        If PostUnpostFlag = True Then
            'chkPost.Visible = True
            'lblPostmsg.Visible = True
        Else
            'chkPost.Visible = False
            'lblPostmsg.Visible = False
            'If ViewState("ResState") = "Edit" Then
            'If chkPost.Checked = True Then
            'ViewState.Add("ResState", "View")
            ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This invoice has been posted, you do not have rights to edit.' );", True)
            'End If
            'End If
        End If
    End Sub

    Protected Sub ddlRefundId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub btndisPos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndisPos.Click
        Dim dspurchase As New DataSet
        Dim strSql As String
        strSql = "select 1 PI from providerinv_detail where requestid='" + txtRequestId.Value.Trim + "' "
        dspurchase = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strSql)
        If dspurchase.Tables(0).Rows.Count > 0 Then
            hdnPI.Value = dspurchase.Tables(0).Rows(0)("PI")
        End If

        txtTotalKWDCredit.Value = 0
        txtTotalKWDDebit.Value = 0
        txtTotalKWDCreditCP.Value = 0
        txtTotalKWDDebitCP.Value = 0

        FillAllData()
        txtRefundSaleValue.Disabled = True
        txtRefundCharges.Disabled = True
        txtRefundOther.Disabled = True
        dpCreditNoteDate.Enabled = False
    End Sub

    Protected Sub BtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
End Class
