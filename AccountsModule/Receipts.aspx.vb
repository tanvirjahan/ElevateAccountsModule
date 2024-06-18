
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
#End Region
Partial Class Receipts
    Inherits System.Web.UI.Page
    'For accounts posting
    Dim caccounts As clssave = Nothing
    Dim cacc As clsAccounts = Nothing
    Dim ctran As clstran = Nothing
    Dim csubtran As clsSubTran = Nothing

    Dim ScriptOpenModalDialog As String = "OpenModalDialog('{0}','{1}');"
    Dim mbasecurrency As String = ""
    'For accounts posting

#Region "Global Declarations"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objDateTime As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader

    Dim gvRow As GridViewRow
    Dim strTranType As String

    Dim intTabindex As Integer = 0
    Enum grd_col
        AccType = 0
        AccCode = 1
        AccName = 2
        ctrolcode = 3
        ctrolname = 4
        costcentercode = 5
        costcentername = 6
        Narration = 7
        Currency = 8
        CnvtRate = 9
        debit = 10
        Credit = 11
        BaseDebit = 12
        BaseCredit = 13
        AdBill = 14
        LienNo = 15
        t1 = 16
        t2 = 17
        t3 = 18
        oldlineno = 19
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
    Public Sub TextLockhtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onKeyPress", "return chkTextLock(event)")
        txtbox.Attributes.Add("onKeyDown", "return chkTextLock1(event)")
        txtbox.Attributes.Add("onKeyUp", "return chkTextLock(event)")
    End Sub
#End Region

#Region "Public Sub fillgrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)"
    Public Sub fillDategrd(ByVal grd As GridView, Optional ByVal blnload As Boolean = False, Optional ByVal count As Integer = 1)
        Dim lngcnt As Long
        If blnload = True Then
            lngcnt = 10
        Else
            lngcnt = count
        End If
        grd.DataSource = CreateDataSource(lngcnt)
        grd.DataBind()
        grd.Visible = True
        txtgridrows.Value = grd.Rows.Count
    End Sub
#End Region
#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("LineNo", GetType(Integer)))

        For i = 1 To lngcount
            dr = dt.NewRow()
            dr(0) = i
            dt.Rows.Add(dr)
        Next
        'return a DataView to the DataTable
        CreateDataSource = New DataView(dt)
        'End If
    End Function
#End Region
#Region "Protected Sub grdAcc_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAcc.RowDataBound"
    Protected Sub grdReceipt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdReceipt.RowDataBound

        Dim ddlAccType As HtmlSelect
        Dim ddlgAccCode As HtmlSelect
        Dim ddlgAccName As HtmlSelect
        Dim ddlConAccCode As HtmlSelect
        Dim ddlConAccName As HtmlSelect
        Dim ddlCCCode As HtmlSelect
        Dim ddlCCName As HtmlSelect

        '15122014
        Dim ddldept As HtmlSelect

        Dim txtCurrCode As HtmlInputText
        Dim txtauto As HtmlInputText
        Dim txtCurrRate As HtmlInputText
        Dim txtDebit, txtBaseDebit, txtCredit, txtBaseCredit, txtOldLineno, txtgnarration, txtrequestid As HtmlInputText
        Dim btnBill As HtmlInputButton
        Dim chckDeletion As CheckBox

        Dim lblno As HtmlInputText
        Dim txtacctcode, txtacctname, txtctrolaccode, txtcontrolacname As HtmlInputText

        Dim txtautoCode As HtmlInputText

        Dim strOpti, sqlstr1, sqlstr2 As String
        Dim i As Integer = 0
        gvRow = e.Row

        gvRow = e.Row
        If e.Row.RowIndex = -1 Then
            strOpti = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
            gvRow.Cells(grd_col.BaseDebit).Text = strOpti & " Debit"
            gvRow.Cells(grd_col.BaseCredit).Text = strOpti & " Credit"
            Exit Sub
        End If
        txtauto = gvRow.FindControl("accSearch")
        txtacctcode = gvRow.FindControl("txtacctcode")
        txtacctname = gvRow.FindControl("txtacctname")
        txtctrolaccode = gvRow.FindControl("txtctrolaccode")
        txtcontrolacname = gvRow.FindControl("txtcontrolacname")


        ddlAccType = gvRow.FindControl("ddlType")
        ddlgAccCode = gvRow.FindControl("ddlgAccCode")
        ddlgAccName = gvRow.FindControl("ddlgAccName")
        txtCurrCode = gvRow.FindControl("txtCurrency")
        txtCurrRate = gvRow.FindControl("txtConvRate")

        ddlConAccCode = gvRow.FindControl("ddlConAccCode")
        ddlConAccName = gvRow.FindControl("ddlConAccName")
        ddlCCCode = gvRow.FindControl("ddlCostCode")
        ddlCCName = gvRow.FindControl("ddlCostName")

        ddldept = gvRow.FindControl("ddldept")

        txtDebit = gvRow.FindControl("txtDebit")
        txtCredit = gvRow.FindControl("txtCredit")
        txtBaseDebit = gvRow.FindControl("txtBaseDebit")
        txtBaseCredit = gvRow.FindControl("txtBaseCredit")

        btnBill = gvRow.FindControl("btnAd")
        lblno = gvRow.FindControl("txtlineno")
        txtOldLineno = gvRow.FindControl("txtOldLineno")
        txtgnarration = gvRow.FindControl("txtgnarration")
        chckDeletion = gvRow.FindControl("chckDeletion")

        txtautoCode = gvRow.FindControl("accCodeSearch")

        txtrequestid = gvRow.FindControl("txtrequestid")

        Session("TabIndex") = intTabindex
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccType, "acc_type_des", "acc_type_name", "select acc_type_des,acc_type_name from  acc_type_master where acc_type_mode<>'G' order by acc_type_name", True)

        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccCode, "Code", "des", "select top 10 Code,des from view_account where view_account.div_code='" & ViewState("divcode") & "'  order by code", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccName, "des", "Code", "select top 10 Code,des from view_account where view_account.div_code='" & ViewState("divcode") & "' order by des", True)

        sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
        sqlstr2 = " select  '' as acctname , '' as controlacctcode "

        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)

        '15122014

        'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "othmaingrpname", "othmaingrpcode", "select othmaingrpcode,othmaingrpname from othmaingrpmast where active=1 order by othmaingrpcode ", True)

        'Added ny Riswan 25/06/2015
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)


        ddlAccType.Attributes.Add("onchange", "javascript:fill_acountcode('" + CType(ddlAccType.ClientID, String) + "','" + CType(ddlgAccCode.ClientID, String) + "','" + CType(ddlgAccName.ClientID, String) + "','" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(txtgnarration.ClientID, String) + "','" + CType(ddlCCCode.ClientID, String) + "','" + CType(ddlCCName.ClientID, String) + "','" + CType(txtauto.ClientID, String) + "')")
        ddlgAccCode.Attributes.Add("onchange", "javascript:FillGACode('" + CType(ddlgAccCode.ClientID, String) + "','" + CType(ddlgAccName.ClientID, String) + "','" + CType(txtCurrCode.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(txtacctcode.ClientID, String) + "','" + CType(txtacctname.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtctrolaccode.ClientID, String) + "','" + CType(txtcontrolacname.ClientID, String) + "','" + CType(txtauto.ClientID, String) + "')")
        ddlgAccName.Attributes.Add("onchange", "javascript:FillGAName('" + CType(ddlgAccCode.ClientID, String) + "','" + CType(ddlgAccName.ClientID, String) + "','" + CType(txtCurrCode.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(txtacctcode.ClientID, String) + "','" + CType(txtacctname.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtctrolaccode.ClientID, String) + "','" + CType(txtcontrolacname.ClientID, String) + "','" + CType(txtauto.ClientID, String) + "')")

        ddlConAccCode.Attributes.Add("onchange", "javascript:FillCTCode('" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(txtctrolaccode.ClientID, String) + "','" + CType(txtcontrolacname.ClientID, String) + "')")
        ddlConAccName.Attributes.Add("onchange", "javascript:FillCTName('" + CType(ddlConAccCode.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(txtctrolaccode.ClientID, String) + "','" + CType(txtcontrolacname.ClientID, String) + "')")

        'txtauto.Attributes.Add("onfocus", "javascript:OnChangeType('" + CType(txtauto.ClientID, String) + "','" + CType(ddlgAccName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "')")
        txtauto.Attributes.Add("onfocus", "javascript:OnChangeType('" + CType(txtauto.ClientID, String) + "','" + CType(ddlgAccName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(txtautoCode.ClientID, String) + "')")

        txtDebit.Attributes.Add("onchange", "javascript:convertInRate('" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType("Debit", String) + "')")
        txtCredit.Attributes.Add("onchange", "javascript:convertInRate('" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType("Credit", String) + "')")
        txtCurrRate.Attributes.Add("onchange", "javascript:convertInRate('" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType("Currate", String) + "')")

        'btnBill.Attributes.Add("onClick", "javascript:openAdjustBill('" + CType(ddlgAccName.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(txtDocNo.ClientID, String) + "','" + CType(lblno.ClientID, String) + "','" + CType(txtCurrCode.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtOldLineno.ClientID, String) + "','" + CType(txtrequestid.ClientID, String) + "')")

        'btnBill.Attributes.Add("onClick", "javascript:openAdjustBill('" + CType(ddlgAccName.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(txtDocNo.ClientID, String) + "','" + CType(lblno.ClientID, String) + "','" + CType(txtCurrCode.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtDebit.ClientID, String) + "','" + CType(txtrequestid.ClientID, String) + "')")

        'btnBill.Attributes.Add("onClick", "javascript:openAdjustBill('" + CType(ddlgAccName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(txtDocNo.ClientID, String) + "','" + CType(lblno.ClientID, String) + "','" + CType(txtCurrCode.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "')")

        btnBill.Attributes.Add("onClick", "javascript:openAdjustBill('" + CType(ddlgAccName.ClientID, String) + "','" + CType(ddlConAccName.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(txtDocNo.ClientID, String) + "','" + CType(lblno.ClientID, String) + "','" + CType(txtCurrCode.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtOldLineno.ClientID, String) + "','" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtrequestid.ClientID, String) + "')")

        ddlCCCode.Attributes.Add("onchange", "javascript:FillCodeName( '" + CType(ddlCCCode.ClientID, String) + "','" + CType(ddlCCName.ClientID, String) + "')")
        ddlCCName.Attributes.Add("onchange", "javascript:FillCodeName('" + CType(ddlCCName.ClientID, String) + "','" + CType(ddlCCCode.ClientID, String) + "')")

        txtBaseDebit.Attributes.Add("onchange", "javascript:convertInRateBase('" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType("Debit", String) + "')")
        txtBaseCredit.Attributes.Add("onchange", "javascript:convertInRateBase('" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType("Credit", String) + "')")


        txtautoCode.Attributes.Add("onfocus", "javascript:OnChangeType('" + CType(txtautoCode.ClientID, String) + "','" + CType(ddlgAccCode.ClientID, String) + "','" + CType(ddlAccType.ClientID, String) + "','" + CType(txtauto.ClientID, String) + "','1')")

        'TextLockhtml(txtBaseDebit)
        'TextLockhtml(txtBaseCredit)
        NumbersHtml(txtBaseDebit)
        NumbersHtml(txtBaseCredit)
        NumbersHtml(txtCurrRate)
        NumbersHtml(txtCurrRate)
        NumbersDecimalRoundHtml(txtCredit)
        NumbersDecimalRoundHtml(txtDebit)

        Dim typ As Type
        typ = GetType(DropDownList)
        ' If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
        Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
        ddlAccType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        ddlgAccCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        ddlgAccName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        ddlCCCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        ddlCCName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        'End If

    End Sub
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim stracccode As String = ""

        ViewState.Add("ReceiptsRVPVTranType", Request.QueryString("RVPVTranType"))
        ViewState.Add("ReceiptsState", Request.QueryString("State"))
        ViewState.Add("divcode", Request.QueryString("divid"))
        txtMode.Value = Trim(ViewState("ReceiptsState"))
        txtDivCode.Value = ViewState("divcode")

        If Page.IsPostBack = False Then
            Try


                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                txtconnection.Value = Session("dbconnectionName")

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                txtTranType.Value = CType(ViewState("ReceiptsRVPVTranType"), String)
                SetFocus(ddlCashBank)

                Dim adjcolno As String
                adjcolno = objUtils.GetAutoDocNoWTnew(Session("dbconnectionName"), "ADJCOL")
                txtAdjcolno.Value = adjcolno
                Session.Add("Collection" & ":" & adjcolno, "")

                txtDate.Text = Format(objDateTime.GetSystemDateTime(Session("dbconnectionName")), "dd/MM/yyyy")
                txtChequeDate.Text = Format(objDateTime.GetSystemDateTime(Session("dbconnectionName")), "dd/MM/yyyy")
                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
                txtDivCode.Value = ViewState("divcode") ' objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
                txtbasecurr.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 457)

                lblBaseAmt.Text = txtbasecurr.Value & " Amount"
                'lblBaseTotal.Text = txtbasecurr.Value & " Total"
                'lblBaseTot.Text = txtbasecurr.Value & " Total"
                'lblBaseDiff.Text = txtbasecurr.Value & " Diff."

                Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select top 1  sealdate from  sealing_master where div_code='" & txtDivCode.Value & "'")
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("sealdate")) = False Then
                            txtpdate.Text = CType(ds.Tables(0).Rows(0)("sealdate"), String)
                        End If
                    Else
                        txtpdate.Text = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 508)
                    End If
                End If


                strSqlQry = "SELECT   distinct  receipt_received_from,receipt_received_from  FROM  receipt_master_new where tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "'"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlRecveidfrom, "receipt_received_from", "receipt_received_from", strSqlQry, True)

                strSqlQry = "select  narration,narration from narration where active=1"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlNarration, "narration", "narration", strSqlQry, True)

                If Trim(txtTranType.Value) = "CPV" Then
                    ddlCashBank.Value = "Cash"
                Else
                    ddlCashBank.Value = "Bank"
                End If

                If ddlCashBank.Value = "Bank" Then
                    strSqlQry = "select acctcode,acctname from acctmast ,bank_master_type where acctmast.div_code='" & txtDivCode.Value & "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
                    " and  bankyn='Y' and bank_master_type.cashbanktype = 'B' order by acctcode "
                    lbldate.Visible = False
                    lbldate1.Visible = True
                Else
                    strSqlQry = "select acctcode,acctname from acctmast ,bank_master_type where acctmast.div_code='" & txtDivCode.Value & "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
                    " and  bankyn='Y' and bank_master_type.cashbanktype = 'C' order by acctcode "
                    lbldate.Visible = True
                    lbldate1.Visible = False

                End If
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccCode, "acctcode", "acctname", strSqlQry, True)


                If ddlCashBank.Value = "Bank" Then
                    strSqlQry = "select acctname, acctcode from acctmast ,bank_master_type where acctmast.div_code='" & txtDivCode.Value & "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
          " and  bankyn='Y'  and bank_master_type.cashbanktype = 'B' order by acctname"
                Else
                    strSqlQry = "select acctname, acctcode from acctmast ,bank_master_type where acctmast.div_code='" & txtDivCode.Value & "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
          " and  bankyn='Y'  and bank_master_type.cashbanktype = 'C' order by acctname"
                End If


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccName, "acctname", "acctcode", strSqlQry, True)


                strSqlQry = "select  Currcode,acctcode  from acctmast ,bank_master_type where  acctmast.div_code='" & txtDivCode.Value & "'and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
                " and    bankyn='Y'  order by acctcode"
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrCode, "Currcode", "acctcode", strSqlQry, True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCustBank, "other_bank_master_des", "other_bank_master_code", "select other_bank_master_des,other_bank_master_code from customer_bank_master where active=1 ", True)

                ddlCustBank.SelectedIndex = 0

                '14122014

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSMktCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active=1 order by plgrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSMktName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)



                Dim headerCaption As String
                headerCaption = ""
                If Trim(txtTranType.Value) = "RV" Then
                    headerCaption = "Receipt"
                    lblRecfrom.Text = "Received From"
                    ddlCustBank.Style("VISIBILITY") = "visible"
                    txtChequeNo.Style("VISIBILITY") = "visible"
                    txtChequeDate.Style("VISIBILITY") = "visible"
                    ImageButton1.Style("VISIBILITY") = "visible"
                    lblChN.Style("VISIBILITY") = "visible"
                    lblChB.Style("VISIBILITY") = "visible"
                    lblChD.Style("VISIBILITY") = "visible"
                ElseIf Trim(txtTranType.Value) = "CPV" Then
                    headerCaption = "Cash Payment"
                    lblRecfrom.Text = "Paid To"
                    btnclientreceipt.Visible = False

                    ddlCashBank.Disabled = True


                    FillCashBankDetails()

                    stracccode = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id =1084")
                    ddlAccName.Value = stracccode
                    ddlAccCode.Value = ddlAccName.Items(ddlAccName.SelectedIndex).Text
                    ddlCurrCode.Value = stracccode

                    txtConvRate.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select convrate  from currrates,acctmast  where  acctmast.div_code='" & txtDivCode.Value & "' and currrates.currcode=acctmast.currcode and Tocurr='" + txtbasecurr.Value + "' and acctmast.acctcode='" + stracccode + "'")
                    If txtConvRate.Value <> "" Then
                        txtConvRate.Value = Math.Round(CType(txtConvRate.Value, Decimal), 0)
                    Else
                        txtConvRate.Value = ""
                    End If
                    txtConvRate.Disabled = True

                    txtAccCode.Value = ddlAccCode.Value
                    txtAccName.Value = stracccode
                    txtCurrCode.Value = stracccode

                    ddlCustBank.Style("VISIBILITY") = "hidden"
                    lblChB.Style("VISIBILITY") = "hidden"
                    txtChequeNo.Style("VISIBILITY") = "hidden"
                    txtChequeDate.Style("VISIBILITY") = "hidden"
                    ImageButton1.Style("VISIBILITY") = "hidden"
                    lblChN.Style("VISIBILITY") = "hidden"
                    lblChD.Style("VISIBILITY") = "hidden"
                    ddlCashBank.Value = "Cash"

                ElseIf Trim(txtTranType.Value) = "BPV" Then
                    btnclientreceipt.Visible = False

                    headerCaption = "Bank Payment"
                    lblRecfrom.Text = "Paid To"

                    ddlCashBank.Value = "Bank"
                    ddlCashBank.Disabled = True
                    FillCashBankDetails()

                    ddlCustBank.Style("VISIBILITY") = "hidden"
                    lblChB.Style("VISIBILITY") = "hidden"
                    txtChequeNo.Style("VISIBILITY") = "visible"
                    txtChequeDate.Style("VISIBILITY") = "visible"
                    ImageButton1.Style("VISIBILITY") = "visible"
                    lblChN.Style("VISIBILITY") = "visible"
                    lblChD.Style("VISIBILITY") = "visible"
                ElseIf Trim(txtTranType.Value) = "DEP" Then
                    headerCaption = "Deposit"
                    lblRecfrom.Text = "Deposit To"
                    ddlCustBank.Style("VISIBILITY") = "hidden"
                    lblChB.Style("VISIBILITY") = "hidden"
                    txtChequeNo.Style("VISIBILITY") = "visible"
                    txtChequeDate.Style("VISIBILITY") = "visible"
                    ImageButton1.Style("VISIBILITY") = "visible"
                    lblChN.Style("VISIBILITY") = "visible"
                    lblChD.Style("VISIBILITY") = "visible"
                End If
                ViewState.Add("ReceiptsRefCode", Request.QueryString("RefCode"))

                If ViewState("ReceiptsState") = "New" Then
                    fillDategrd(grdReceipt, False, 5)
                    txtDocNo.Value = ""
                    chkPost.Checked = True
                    btnclientreceipt.Visible = False

                    lblHeading.Text = "Add New " & headerCaption
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save')==false)return false;")
                ElseIf ViewState("ReceiptsState") = "Copy" Then
                    txtDocNo.Value = ViewState("ReceiptsRefCode")
                    show_record(ViewState("ReceiptsRefCode"))
                    ShowFillGrid(ViewState("ReceiptsRefCode"))
                    'fillcollection(ViewState("ReceiptsRefCode")) No need bill adjustments
                    txtDocNo.Value = ""
                    btnclientreceipt.Visible = False
                    lblHeading.Text = "Copy " & headerCaption
                    btnSave.Text = "Update"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update')==false)return false;")
                ElseIf ViewState("ReceiptsState") = "Edit" Then
                    txtDocNo.Value = ViewState("ReceiptsRefCode")
                    show_record(txtDocNo.Value)
                    ShowFillGrid(txtDocNo.Value)
                    fillcollection(ViewState("ReceiptsRefCode"))
                    lblHeading.Text = "Edit " & headerCaption
                    btnSave.Text = "Update"
                    btnclientreceipt.Visible = False
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update')==false)return false;")
                ElseIf ViewState("ReceiptsState") = "Delete" Then
                    txtDocNo.Value = ViewState("ReceiptsRefCode")
                    show_record(txtDocNo.Value)
                    ShowFillGrid(txtDocNo.Value)
                    fillcollection(ViewState("ReceiptsRefCode"))
                    lblHeading.Text = "Delete " & lblHeading.Text
                    btnSave.Text = "Delete"
                    btnclientreceipt.Visible = False
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete')==false)return false;")
                ElseIf ViewState("ReceiptsState") = "View" Then
                    txtDocNo.Value = ViewState("ReceiptsRefCode")
                    show_record(txtDocNo.Value)
                    ShowFillGrid(txtDocNo.Value)
                    If Trim(txtTranType.Value) = "RV" Then
                        btnclientreceipt.Visible = False
                    End If
                    fillcollection(ViewState("ReceiptsRefCode"))
                    lblHeading.Text = "View" & headerCaption
                ElseIf ViewState("ReceiptsState") = "Cancel" Then
                    txtDocNo.Value = ViewState("ReceiptsRefCode")
                    show_record(txtDocNo.Value)
                    ShowFillGrid(txtDocNo.Value)
                    fillcollection(ViewState("ReceiptsRefCode"))
                    lblHeading.Text = "Cancel " & lblHeading.Text
                    btnSave.Text = "Cancel"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel')==false)return false;")
                ElseIf ViewState("ReceiptsState") = "undoCancel" Then
                    txtDocNo.Value = ViewState("ReceiptsRefCode")
                    show_record(txtDocNo.Value)
                    ShowFillGrid(txtDocNo.Value)
                    fillcollection(ViewState("ReceiptsRefCode"))
                    lblHeading.Text = "Undo Cancel " & lblHeading.Text
                    btnSave.Text = " Undo "
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Undo Cancel')==false)return false;")

                End If

                txtDivCode.Value = ViewState("divcode")
                'btnPrint.Attributes.Add("onclick", "return ReprintDoc()")
                Dim appname As String = ""
                Dim appidnew As String = ""
                If txtDivCode.Value = "01" Then
                    appname = "ColumbusCommon" + " " + CType("Accounts Module", String)
                    appidnew = "4"
                Else
                    appname = "ColumbusCommon Gulf " + CType("Accounts Module", String)
                    appidnew = "14"
                End If

                CheckPostUnpostRight(CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType(appname, String), "AccountsModule\ReceiptsSearch.aspx?tran_type=" & txtTranType.Value & "&appid=" + appidnew)
                Disabled_Control()


                btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")

                '"onchange", "javascript:convertInRate('" + CType(txtDebit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "','" + CType("Debit", String) + "')"

                txtAmount.Attributes.Add("onchange", "javascript:convertInRateAmount('" + CType(txtAmount.ClientID, String) + "','" + CType(txtConvRate.ClientID, String) + "','" + CType(txtCnvAmount.ClientID, String) + "')")
                txtConvRate.Attributes.Add("onchange", "javascript:convertInRateAmount('" + CType(txtAmount.ClientID, String) + "','" + CType(txtConvRate.ClientID, String) + "','" + CType(txtCnvAmount.ClientID, String) + "')")

                txtCnvAmount.Attributes.Add("onchange", "javascript:convertInRateBaseAmount('" + CType(txtCnvAmount.ClientID, String) + "','" + CType(txtConvRate.ClientID, String) + "','" + CType(txtAmount.ClientID, String) + "')")

                ddlAccCode.Attributes.Add("onchange", "javascript:FillCode('" + CType(ddlAccCode.ClientID, String) + "','" + CType(ddlAccName.ClientID, String) + "')")
                ddlAccName.Attributes.Add("onchange", "javascript:FillName('" + CType(ddlAccCode.ClientID, String) + "','" + CType(ddlAccName.ClientID, String) + "')")

                ddlCashBank.Attributes.Add("onchange", "javascript:OnchangeCashBank('" + CType(ddlCashBank.ClientID, String) + "')")

                ddlRecveidfrom.Attributes.Add("onchange", "javascript:FillCombotoText('" + CType(ddlRecveidfrom.ClientID, String) + "','" + CType(txtReceived.ClientID, String) + "')")
                ddlNarration.Attributes.Add("onchange", "javascript:FillCombotoText('" + CType(ddlNarration.ClientID, String) + "','" + CType(txtnarration.ClientID, String) + "')")

                NumbersHtml(txtConvRate)
                NumbersDecimalRoundHtml(txtAmount)
                NumbersDecimalRoundHtml(txtBalance)

                Dim typ As Type
                typ = GetType(DropDownList)
                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlAccCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlAccName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCashBank.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCurrCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCustBank.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

                btnSave.Attributes.Add("onclick", "return validate_click()")
                btnPrint.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to print?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("Receipts.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If check_Privilege() = 1 Then

            chkPost.Enabled = True
            chkPost.Checked = True
        Else

            chkPost.Enabled = False
            chkPost.Checked = True
        End If

        'Added check_Privilege() and chkpost enabled by Archana on 01/04/2015

        If IsPostBack = True Then

            Allowanyway()
            If Session("Allowanyway") = "Yes" Then
                chkadjust.Visible = False
            End If





            FillGrid()

            FillGridConvRate()
            txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
            txtDivCode.Value = ViewState("divcode") ' objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
            txtbasecurr.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 457)
            FillCashBankDetails()
            FillHeaderconvRate()
            Dim txtcr, txtbasecr, txtcrate, txtdr, txtbasedr As HtmlInputText

            Dim rowind As Integer
            ' If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "AdjBillWindowPostBack") = False Then
            If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "AdjBillWindowPostBack") Then
                rowind = Val(Session("LineNo")) - 1
                txtcr = grdReceipt.Rows(rowind).FindControl("txtCredit")
                txtbasecr = grdReceipt.Rows(rowind).FindControl("txtBaseCredit")
                txtdr = grdReceipt.Rows(rowind).FindControl("txtDebit")
                txtbasedr = grdReceipt.Rows(rowind).FindControl("txtBaseDebit")
                txtcrate = grdReceipt.Rows(rowind).FindControl("txtConvRate")
                If Session("Gridtype") = "Debit" Then
                    txtdr.Value = Session("AmountAdjusted")
                    txtbasedr.Value = Session("BaseAmountAdjusted")
                    txtcrate.Value = CType(DecRound(txtbasedr.Value) / DecRound(txtdr.Value), Decimal)
                ElseIf Session("Gridtype") = "Credit" Then
                    txtcr.Value = Session("AmountAdjusted")
                    txtbasecr.Value = Session("BaseAmountAdjusted")
                    txtcrate.Value = CType(DecRound(txtbasecr.Value) / DecRound(txtcr.Value), Decimal)
                End If
            End If
            GrandToatal()
        Else
            GrandToatal()
        End If
    End Sub
#End Region

    Public Function check_Privilege() As Integer
        Try
            Dim strSql As String
            Dim usrCode As String
            usrCode = CType(Session("GlobalUserName"), String)
            'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            strSql = "select appid from group_privilege_Detail where privilegeid='3' and appid='4' and "
            strSql += "groupid=(SELECT groupid FROM UserMaster WHERE UserCode='" + usrCode + "')"
            'mySqlCmd = New SqlCommand(strSql, mySqlConn)
            'mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)

            Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strSql)
            If ds1.Tables.Count > 0 Then
                If ds1.Tables(0).Rows.Count > 0 Then
                    Return 1
                Else
                    Return 0
                End If
            End If


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("TransfersRequestSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            'clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            'clsDBConnect.dbConnectionClose(mySqlConn)           'connection close 
        End Try
    End Function

#Region "Public Function Allowanyway() As Boolean"
    Public Function Allowanyway()
        Try
            Dim strSql As String
            Dim usrCode As String
            usrCode = CType(Session("GlobalUserName"), String)
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open

            strSql = "select appid from group_privilege_Detail where (privilegeid='2' and appid='4')  and "
            strSql += "groupid=(SELECT groupid FROM UserMaster WHERE UserCode='" + usrCode + "')"
            myCommand = New SqlCommand(strSql, SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                Session.Add("Allowanyway", "Yes")
            Else
                Session.Add("Allowanyway", "No")
            End If
            Allowanyway = ""
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Reservation.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(SqlConn)           'connection close 
            Allowanyway = ""
        End Try
    End Function
#End Region

    Private Sub FillGridConvRate()
        Dim strQry As String = ""
        Dim gvrow As GridViewRow
        Dim ddlAccType As HtmlSelect
        Dim ddlgAccCode As HtmlSelect
        Dim txtCurrRate As HtmlInputText

        For Each gvrow In grdReceipt.Rows
            txtCurrRate = gvrow.FindControl("txtConvRate")
            ddlAccType = gvrow.FindControl("ddlType")
            ddlgAccCode = gvrow.FindControl("ddlgAccCode")

            'strQry = "select cur,convrate,controlacctcode from  view_account left outer join " & _
            '                   " currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where code = '" & ddlgAccCode.Items(ddlgAccCode.SelectedIndex).Text & "' and type='" & ddlAccType.Items(ddlAccType.SelectedIndex).Text & "' "
            If ddlAccType.Value <> "[Select]" And ddlgAccCode.Value <> "[Select]" Then
                If Val(txtCurrRate.Value) = 0 Then
                    strQry = "select convrate from  view_account left outer join " & _
                                    " currrates on  view_account.cur=currrates.currcode and tocurr = (select option_selected from reservation_parameters where param_id=457) where view_account.div_code='" & ViewState("divcode") & "' and  code = '" & ddlgAccCode.Items(ddlgAccCode.SelectedIndex).Text & "' and type='" & ddlAccType.Value & "' "
                    txtCurrRate.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry)
                End If
            End If
        Next

    End Sub

    Private Sub FillHeaderconvRate()
        Dim strQry As String = ""
        strQry = "select convrate  from currrates,acctmast  where  acctmast.div_code='" & ViewState("divcode") & "' and currrates.currcode=acctmast.currcode and Tocurr='" & txtbasecurr.Value & "' and acctmast.acctcode='" & ddlAccCode.Items(ddlAccCode.SelectedIndex).Text & "' "
        If txtConvRate.Value = "" Or Val(txtConvRate.Value) = 0 Then
            txtConvRate.Value = DecRound(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strQry))
        End If

        If txtAmount.Value = "" Then
            txtAmount.Value = 0
        End If
        If txtConvRate.Value = "" Then
            txtConvRate.Value = 0
        End If

        txtCnvAmount.Value = DecRound(CType(txtAmount.Value, Decimal) * CType(txtConvRate.Value, Decimal))

        Dim sqlstr As String
        Dim cbalancamt, balamt As Decimal
        sqlstr = "sp_get_account_balance  '" + txtDivCode.Value + "','G','" + txtAccName.Value + "'"
        cbalancamt = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), sqlstr)
        If (DecRound(cbalancamt) > 0) Then
            lblBalCrDr.Text = "Cr"
        Else
            lblBalCrDr.Text = "Dr"
        End If

        txtBalance.Value = Math.Abs(DecRound(cbalancamt)) 'DecRound(cbalancamt)
        If (txtMode.Value = "Edit") Then
            If Trim(txtTranType.Value) = "RV" Then
                balamt = DecRound(DecRound(cbalancamt) - DecRound(Val(txtOldAmount.Value)))
            Else 'If Trim(txtTranType.Value) = "PV" Then
                balamt = DecRound(DecRound(cbalancamt) + DecRound(Val(txtOldAmount.Value)))
            End If
            txtBalance.Value = DecRound(Math.Abs(balamt))
        End If
    End Sub
    Private Sub FillCashBankDetails()
        Dim strSqlQry1, strSqlQry2, strSqlQry3 As String
        If ddlCashBank.Value = "Bank" Then
            strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='B'  and  bankyn='Y' order by acctcode "
            strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='B'  and  bankyn='Y'  order by acctname"
            strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where  acctmast.div_code='" & ViewState("divcode") & "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='B'  and  bankyn='Y'  order by acctcode"
        ElseIf ddlCashBank.Value = "Cash" Then
            strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='C'  and  bankyn='Y' order by acctcode "
            strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='C'  and  bankyn='Y'  order by acctname"
            strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and bank_master_type.cashbanktype='C'  and  bankyn='Y'  order by acctcode"
        Else
            strSqlQry1 = "select acctcode,acctname from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and  bankyn='Y' order by acctcode "
            strSqlQry2 = "select acctname, acctcode from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and  bankyn='Y'  order by acctname"
            strSqlQry3 = "select  Currcode,acctcode  from acctmast ,bank_master_type where acctmast.div_code='" & ViewState("divcode") & "' and  acctmast.bank_master_type_code = bank_master_type.bank_master_type_code" & _
            " and    bankyn='Y'  order by acctcode"
        End If
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccCode, "acctcode", "acctname", strSqlQry1, True, txtAccCode.Value)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccName, "acctname", "acctcode", strSqlQry2, True, txtAccName.Value)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrCode, "Currcode", "acctcode", strSqlQry3, True, txtCurrCode.Value)
        'If txtBalance.Value <> "" Then
        '    If DecRound(txtBalance.Value) > 0 Then
        '        lblBalCrDr.Text = "Cr"
        '    Else
        '        lblBalCrDr.Text = "Dr"
        '    End If
        'Else
        '    lblBalCrDr.Text = ""
        'End If

    End Sub
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)  "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sqlTrans As SqlTransaction
        Dim strdiv, strcostcentercode As String
        'btnSave.Enabled = False
        Try
            If Page.IsValid = True Then
                If Trim(txtTranType.Value) = "BPV" Then
                    If ViewState("IsCheckedValidateCheque") <> 1 Then
                        If ValidateChequeno(txtChequeNo.Value, txtDocNo.Value) = False Then
                            ViewState("IsCheckedValidateCheque") = 1
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "confirma", "ConfirmContinue('Cheque no is already present, Do you want to continue?','" & btnSave.ClientID & "');", True)
                            btnSave.Enabled = True
                            Exit Sub
                        End If
                    End If
                End If

                If ViewState("ReceiptsState") = "Edit" Or ViewState("ReceiptsState") = "Delete" Or ViewState("ReceiptsState") = "Cancel" Or ViewState("ReceiptsState") = "undoCancel" Then
                    If validate_BillAgainst() = False Then
                        btnSave.Enabled = True
                        Exit Sub
                    End If
                End If

                If ViewState("ReceiptsState") = "New" Or ViewState("ReceiptsState") = "Edit" Or ViewState("ReceiptsState") = "Copy" Or ViewState("ReceiptsState") = "undoCancel" Then
                    If validate_page() = False Then
                        btnSave.Enabled = True
                        Exit Sub
                    End If
                End If

                If ViewState("ReceiptsState") = "New" Or ViewState("ReceiptsState") = "Edit" Or ViewState("ReceiptsState") = "Delete" Or ViewState("ReceiptsState") = "Cancel" Or ViewState("ReceiptsState") = "undoCancel" Then
                    If Validateseal() = False Then
                        btnSave.Enabled = True

                        Exit Sub
                    End If
                End If

                If ViewState("ReceiptsState") = "New" Or ViewState("ReceiptsState") = "Edit" Or ViewState("ReceiptsState") = "Copy" Then
                    If chkPost.Checked = True And chkadjust.Checked = False Then
                        If checkopenmode() = False Then
                            btnSave.Enabled = True

                            Exit Sub
                        End If
                    End If
                End If

                strdiv = ViewState("divcode") ' objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
                strcostcentercode = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 510)
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                sqlTrans = SqlConn.BeginTransaction

                If chkPost.Checked = True Then
                    'For Accounts posting
                    initialclass(SqlConn, sqlTrans)
                    'For Accounts posting
                End If

                If ViewState("ReceiptsState") = "New" Or ViewState("ReceiptsState") = "Edit" Or ViewState("ReceiptsState") = "Copy" Then
                    If ViewState("ReceiptsState") = "New" Or ViewState("ReceiptsState") = "Copy" Then
                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNodiv(CType(ViewState("ReceiptsRVPVTranType"), String), SqlConn, sqlTrans, strdiv)
                        txtDocNo.Value = optionval.Trim
                        myCommand = New SqlCommand("sp_add_receipt_master", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    ElseIf ViewState("ReceiptsState") = "Edit" Then
                        myCommand = New SqlCommand("sp_mod_receipt_master", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    End If

                    myCommand.Parameters.Add(New SqlParameter("@receipt_div_id ", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)

                    myCommand.Parameters.Add(New SqlParameter("@receipt_date", SqlDbType.DateTime)).Value = Format(CType(txtDate.Text, Date), "yyyy/MM/dd")
                    If ddlCashBank.Value = "Bank" Then
                        myCommand.Parameters.Add(New SqlParameter("@receipt_tran_date", SqlDbType.DateTime)).Value = Format(CType(txtChequeDate.Text, Date), "yyyy/MM/dd")
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@receipt_tran_date", SqlDbType.DateTime)).Value = Format(CType(txtDate.Text, Date), "yyyy/MM/dd")
                    End If

                    If Val(txtMRV.Value) <> 0 Or txtMRV.Value.Trim <> "" Then
                        myCommand.Parameters.Add(New SqlParameter("@receipt_mrv", SqlDbType.VarChar, 10)).Value = txtMRV.Value.Trim
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@receipt_mrv", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    End If
                    myCommand.Parameters.Add(New SqlParameter("@receipt_salesperson_code", SqlDbType.VarChar, 10)).Value = DBNull.Value

                    myCommand.Parameters.Add(New SqlParameter("@receipt_narration", SqlDbType.VarChar, 200)).Value = txtnarration.Text
                    myCommand.Parameters.Add(New SqlParameter("@receipt_received_from", SqlDbType.VarChar, 100)).Value = txtReceived.Value

                    If ddlCashBank.Value = "Bank" Or (ddlCashBank.Value = "Cash" And CType(ViewState("ReceiptsRVPVTranType"), String) = "RV") Then
                        myCommand.Parameters.Add(New SqlParameter("@receipt_cheque_number", SqlDbType.VarChar, 100)).Value = txtChequeNo.Value
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@receipt_cheque_number", SqlDbType.VarChar, 100)).Value = DBNull.Value
                    End If

                    '15122014

                    If ddlSMktCode.Value <> "[Select]" Then
                        myCommand.Parameters.Add(New SqlParameter("@mktcode", SqlDbType.VarChar, 20)).Value = ddlSMktCode.Items(ddlSMktCode.SelectedIndex).Text
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@mktcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If

                    If ddlCustBank.Value <> "[Select]" Then
                        If CType(ViewState("ReceiptsRVPVTranType"), String) = "RV" And ddlCashBank.Value = "Bank" Or ddlCashBank.Value = "Cash" Then
                            myCommand.Parameters.Add(New SqlParameter("@receipt_customer_bank", SqlDbType.VarChar, 20)).Value = ddlCustBank.Items(ddlCustBank.SelectedIndex).Value
                        Else
                            myCommand.Parameters.Add(New SqlParameter("@receipt_customer_bank", SqlDbType.VarChar, 20)).Value = DBNull.Value
                        End If
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@receipt_customer_bank", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If

                    myCommand.Parameters.Add(New SqlParameter("@receipt_authorized_state", SqlDbType.VarChar, 1)).Value = "A"
                    myCommand.Parameters.Add(New SqlParameter("@receipt_pdc_state", SqlDbType.VarChar, 1)).Value = "P"

                    myCommand.Parameters.Add(New SqlParameter("@receipt_number", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    myCommand.Parameters.Add(New SqlParameter("@receipt_contra_ref_no", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    myCommand.Parameters.Add(New SqlParameter("@receipt_tran_state", SqlDbType.VarChar, 1)).Value = DBNull.Value
                    myCommand.Parameters.Add(New SqlParameter("@receipt_tran_delete_reason", SqlDbType.VarChar, 200)).Value = DBNull.Value


                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objDateTime.GetSystemDateTime(Session("dbconnectionName"))

                    If txtChequeDate.Text <> "" Then
                        myCommand.Parameters.Add(New SqlParameter("@cheque_date", SqlDbType.DateTime)).Value = Format(CType(txtChequeDate.Text, Date), "yyyy/MM/dd")
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@cheque_date", SqlDbType.DateTime)).Value = DBNull.Value
                    End If

                    myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int)).Value = 1

                    If ddlCashBank.Value = "Bank" Then
                        myCommand.Parameters.Add(New SqlParameter("@receipt_cashbank_type", SqlDbType.VarChar, 1)).Value = "B"
                    ElseIf ddlCashBank.Value = "Cash" Then
                        myCommand.Parameters.Add(New SqlParameter("@receipt_cashbank_type", SqlDbType.VarChar, 1)).Value = "C"
                    End If

                    myCommand.Parameters.Add(New SqlParameter("@receipt_cashbank_code", SqlDbType.VarChar, 20)).Value = ddlAccName.Value
                    myCommand.Parameters.Add(New SqlParameter("@receipt_currency_id", SqlDbType.VarChar, 10)).Value = ddlCurrCode.Items(ddlCurrCode.SelectedIndex).Text
                    myCommand.Parameters.Add(New SqlParameter("@receipt_currency_rate", SqlDbType.Decimal, 18, 12)).Value = CType(txtConvRate.Value, Decimal)
                    If CType(ViewState("ReceiptsRVPVTranType"), String) = "RV" Then
                        myCommand.Parameters.Add(New SqlParameter("@receipt_debit", SqlDbType.Money)).Value = 0
                        myCommand.Parameters.Add(New SqlParameter("@receipt_credit", SqlDbType.Money)).Value = DecRound(CType(Val(txtAmount.Value), Decimal))
                        myCommand.Parameters.Add(New SqlParameter("@base_debit", SqlDbType.Money)).Value = 0
                        myCommand.Parameters.Add(New SqlParameter("@base_credit", SqlDbType.Money)).Value = DecRound(CType(Val(txtCnvAmount.Value), Decimal))
                    Else 'If CType(ViewState("ReceiptsRVPVTranType"), String) = "PV" Then
                        myCommand.Parameters.Add(New SqlParameter("@receipt_debit", SqlDbType.Money)).Value = DecRound(CType(Val(txtAmount.Value), Decimal))
                        myCommand.Parameters.Add(New SqlParameter("@receipt_credit", SqlDbType.Money)).Value = 0
                        myCommand.Parameters.Add(New SqlParameter("@base_debit", SqlDbType.Money)).Value = DecRound(CType(Val(txtCnvAmount.Value), Decimal))
                        myCommand.Parameters.Add(New SqlParameter("@base_credit", SqlDbType.Money)).Value = 0
                    End If

                    If chkPost.Checked = True Then
                        myCommand.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "P"
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "U"
                    End If

                    myCommand.ExecuteNonQuery()
                    If chkPost.Checked = True Then

                        'For Accounts Posting
                        caccounts.clraccounts()
                        cacc.acc_tran_id = txtDocNo.Value
                        cacc.acc_tran_type = CType(ViewState("ReceiptsRVPVTranType"), String)
                        If ddlCashBank.Value = "Bank" Then
                            cacc.acc_tran_date = Format(CType(txtChequeDate.Text, Date), "yyyy/MM/dd")
                        Else
                            cacc.acc_tran_date = Format(CType(txtDate.Text, Date), "yyyy/MM/dd")
                        End If
                        cacc.acc_div_id = strdiv

                        'Posting for the Bank/Cash account
                        ctran = New clstran
                        ctran.acc_tran_id = cacc.acc_tran_id
                        ctran.acc_code = ddlAccName.Value
                        ctran.acc_type = "G"
                        ctran.acc_currency_id = ddlCurrCode.Items(ddlCurrCode.SelectedIndex).Text
                        ctran.acc_currency_rate = CType(txtConvRate.Value, Decimal)
                        ctran.acc_div_id = strdiv
                        ctran.acc_narration = txtnarration.Text
                        ctran.acc_tran_date = cacc.acc_tran_date
                        ctran.acc_tran_lineno = 0
                        ctran.acc_tran_type = cacc.acc_tran_type
                        ctran.pacc_gl_code = ""
                        ctran.acc_ref1 = txtChequeNo.Value
                        ctran.acc_ref2 = Format(CType(txtChequeDate.Text, Date), "yyyy/MM/dd")
                        ctran.acc_ref3 = ""
                        ctran.acc_ref4 = ""
                        cacc.addtran(ctran)

                        csubtran = New clsSubTran
                        csubtran.acc_against_tran_id = cacc.acc_tran_id
                        csubtran.acc_against_tran_lineno = 0
                        csubtran.acc_against_tran_type = cacc.acc_tran_type
                        If CType(ViewState("ReceiptsRVPVTranType"), String) = "RV" Then
                            csubtran.acc_debit = DecRound(CType(Val(txtAmount.Value), Decimal))
                            csubtran.acc_credit = 0
                        Else
                            csubtran.acc_credit = DecRound(CType(Val(txtAmount.Value), Decimal))
                            csubtran.acc_debit = 0
                        End If
                        csubtran.acc_tran_date = cacc.acc_tran_date
                        csubtran.acc_due_date = cacc.acc_tran_date
                        csubtran.acc_field1 = ""
                        csubtran.acc_field2 = ""
                        csubtran.acc_field3 = ""
                        csubtran.acc_field4 = ""
                        csubtran.acc_field5 = ""
                        csubtran.acc_tran_id = cacc.acc_tran_id
                        csubtran.acc_tran_lineno = 0
                        csubtran.acc_tran_type = cacc.acc_tran_type
                        csubtran.acc_narration = txtnarration.Text
                        csubtran.acc_type = "G"
                        csubtran.currate = CType(txtConvRate.Value, Decimal)
                        If CType(ViewState("ReceiptsRVPVTranType"), String) = "RV" Then
                            csubtran.acc_base_debit = DecRound(CType(Val(txtCnvAmount.Value), Decimal))
                            csubtran.acc_base_credit = 0
                        Else
                            csubtran.acc_base_credit = DecRound(CType(Val(txtCnvAmount.Value), Decimal))
                            csubtran.acc_base_debit = 0
                        End If
                        csubtran.costcentercode = ""
                        cacc.addsubtran(csubtran)
                    End If
                    If ViewState("ReceiptsState") = "Edit" Then
                        If chkPost.Checked = False Then
                            myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure
                            myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "receipt_master_new"
                            myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                            myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)
                            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                            myCommand.ExecuteNonQuery()
                        End If
                        myCommand = New SqlCommand("sp_del_mod_receipt", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                        myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)
                        myCommand.ExecuteNonQuery()

                        myCommand = New SqlCommand("sp_del_open_detail_new", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                        myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)
                        myCommand.ExecuteNonQuery()
                    End If

                    Dim ddlAccType As HtmlSelect
                    Dim ddlgAccCode As HtmlSelect
                    Dim ddlgAccName As HtmlSelect
                    Dim ddlConAccCode As HtmlSelect
                    Dim ddlCCCode As HtmlSelect
                    '14122014
                    Dim ddldept As HtmlSelect

                    Dim txtCredit, txtDebit, txtBaseDebit, txtCurrCode, txtCurrRate, txtNarr, txtBaseCredit, lblno, txtrequestid As HtmlInputText

                    For Each gvRow In grdReceipt.Rows
                        txtCurrCode = gvRow.FindControl("txtCurrency")
                        txtCurrRate = gvRow.FindControl("txtConvRate")
                        txtNarr = gvRow.FindControl("txtgnarration")
                        ddlConAccCode = gvRow.FindControl("ddlConAccCode")
                        ddlCCCode = gvRow.FindControl("ddlCostCode")
                        ddlAccType = gvRow.FindControl("ddlType")
                        ddlgAccCode = gvRow.FindControl("ddlgAccCode")
                        ddlgAccName = gvRow.FindControl("ddlgAccName")
                        lblno = gvRow.FindControl("txtlineno")
                        txtrequestid = gvRow.FindControl("txtrequestid")

                        txtDebit = gvRow.FindControl("txtDebit")
                        txtCredit = gvRow.FindControl("txtCredit")
                        txtBaseDebit = gvRow.FindControl("txtBaseDebit")
                        txtBaseCredit = gvRow.FindControl("txtBaseCredit")


                        '14122014
                        ddldept = gvRow.FindControl("ddldept")

                        If ddlAccType.Value <> "[Select]" And ddlgAccCode.Value <> "[Select]" Then
                            myCommand = New SqlCommand("sp_add_receipt_detail", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure
                            myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int)).Value = lblno.Value
                            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 10)).Value = txtDocNo.Value
                            myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)
                            myCommand.Parameters.Add(New SqlParameter("@receipt_acc_type", SqlDbType.VarChar, 1)).Value = ddlAccType.Value
                            myCommand.Parameters.Add(New SqlParameter("@receipt_acc_code", SqlDbType.VarChar, 20)).Value = ddlgAccName.Value
                            If ddlAccType.Value <> "G" Then
                                If ddlConAccCode.Value <> "[Select]" Then
                                    myCommand.Parameters.Add(New SqlParameter("@receipt_gl_code", SqlDbType.VarChar, 20)).Value = ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@receipt_gl_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                End If
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@receipt_gl_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            End If

                            myCommand.Parameters.Add(New SqlParameter("@receipt_narration", SqlDbType.VarChar, 200)).Value = txtNarr.Value
                            myCommand.Parameters.Add(New SqlParameter("@receipt_currency_id", SqlDbType.VarChar, 10)).Value = txtCurrCode.Value.Trim
                            myCommand.Parameters.Add(New SqlParameter("@receipt_currency_rate", SqlDbType.Decimal, 18, 12)).Value = CType(txtCurrRate.Value, Decimal)
                            If CType(ViewState("ReceiptsRVPVTranType"), String) = "RV" Then
                                myCommand.Parameters.Add(New SqlParameter("@receipt_debit", SqlDbType.Money)).Value = DecRound(CType(Val(txtDebit.Value), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@receipt_credit", SqlDbType.Money)).Value = DecRound(CType(Val(txtCredit.Value), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@base_debit", SqlDbType.Money)).Value = DecRound(CType(Val(txtBaseDebit.Value), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@base_credit", SqlDbType.Money)).Value = DecRound(CType(Val(txtBaseCredit.Value), Decimal))
                            Else 'If CType(ViewState("ReceiptsRVPVTranType"), String) = "PV" Then
                                myCommand.Parameters.Add(New SqlParameter("@receipt_debit", SqlDbType.Money)).Value = DecRound(CType(Val(txtDebit.Value), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@receipt_credit", SqlDbType.Money)).Value = DecRound(CType(Val(txtCredit.Value), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@base_debit", SqlDbType.Money)).Value = DecRound(CType(Val(txtBaseDebit.Value), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@base_credit", SqlDbType.Money)).Value = DecRound(CType(Val(txtBaseCredit.Value), Decimal))
                            End If
                            myCommand.Parameters.Add(New SqlParameter("@receipt_group", SqlDbType.VarChar, 10)).Value = DBNull.Value
                            myCommand.Parameters.Add(New SqlParameter("@receipt_contra_ref_no", SqlDbType.VarChar, 10)).Value = DBNull.Value
                            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                            If ddlAccType.Value = "G" Then
                                If ddlCCCode.Value <> "[Select]" Then
                                    myCommand.Parameters.Add(New SqlParameter("@costcenter_code", SqlDbType.VarChar, 20)).Value = ddlCCCode.Items(ddlCCCode.SelectedIndex).Text
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@costcenter_code", SqlDbType.VarChar, 20)).Value = strcostcentercode
                                End If
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@costcenter_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            End If
                            If txtrequestid.Value <> "" Then
                                myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = txtrequestid.Value
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@requestid", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            End If

                            If ddldept.Items(ddldept.SelectedIndex).Text <> "[Select]" Then
                                myCommand.Parameters.Add(New SqlParameter("@dept", SqlDbType.VarChar, 20)).Value = ddldept.Items(ddldept.SelectedIndex).Value
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@dept", SqlDbType.VarChar, 20)).Value = DBNull.Value
                            End If


                            myCommand.ExecuteNonQuery()
                            If chkPost.Checked = True Then
                                'Posting for the Grid Accounts
                                ctran = New clstran
                                ctran.acc_tran_id = cacc.acc_tran_id
                                ctran.acc_code = ddlgAccName.Value
                                ctran.acc_type = ddlAccType.Value
                                ctran.acc_currency_id = txtCurrCode.Value.Trim
                                ctran.acc_currency_rate = CType(txtCurrRate.Value, Decimal)
                                ctran.acc_div_id = strdiv
                                ctran.acc_narration = txtNarr.Value
                                ctran.acc_tran_date = cacc.acc_tran_date
                                ctran.acc_tran_lineno = lblno.Value
                                ctran.acc_tran_type = cacc.acc_tran_type
                                If ddlAccType.Value <> "G" Then
                                    If ddlConAccCode.Value <> "[Select]" Then
                                        ctran.pacc_gl_code = ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text
                                    Else
                                        ctran.pacc_gl_code = ""
                                    End If
                                Else
                                    ctran.pacc_gl_code = ""
                                End If

                                ctran.acc_ref1 = txtChequeNo.Value
                                ctran.acc_ref2 = Format(CType(txtChequeDate.Text, Date), "yyyy/MM/dd")
                                ctran.acc_ref3 = ""
                                ctran.acc_ref4 = ""
                                cacc.addtran(ctran)

                                If ddlAccType.Value = "G" Then
                                    csubtran = New clsSubTran
                                    csubtran.acc_against_tran_id = cacc.acc_tran_id
                                    csubtran.acc_against_tran_lineno = lblno.Value
                                    csubtran.acc_against_tran_type = cacc.acc_tran_type
                                    If CType(ViewState("ReceiptsRVPVTranType"), String) = "RV" Then
                                        'csubtran.acc_debit = DecRound(IIf(CType(txtCredit.Value, Decimal) < 0, Math.Abs(CType(txtCredit.Value, Decimal)), 0))
                                        'csubtran.acc_credit = DecRound(IIf(CType(txtCredit.Value, Decimal) > 0, CType(txtCredit.Value, Decimal), 0))
                                        'csubtran.acc_base_debit = DecRound(IIf(CType(txtBaseCredit.Value, Decimal) < 0, Math.Abs(CType(txtBaseCredit.Value, Decimal)), 0))
                                        'csubtran.acc_base_credit = DecRound(IIf(CType(txtBaseCredit.Value, Decimal) > 0, CType(txtBaseCredit.Value, Decimal), 0))

                                        csubtran.acc_debit = CType(Val(txtDebit.Value), Decimal)
                                        csubtran.acc_credit = CType(Val(txtCredit.Value), Decimal)
                                        csubtran.acc_base_debit = CType(Val(txtBaseDebit.Value), Decimal)
                                        csubtran.acc_base_credit = CType(Val(txtBaseCredit.Value), Decimal)



                                    Else 'If CType(ViewState("ReceiptsRVPVTranType"), String) = "PV" Then
                                        'csubtran.acc_credit = DecRound(IIf(CType(txtCredit.Value, Decimal) < 0, Math.Abs(CType(txtCredit.Value, Decimal)), 0))
                                        'csubtran.acc_debit = DecRound(IIf(CType(txtCredit.Value, Decimal) > 0, CType(txtCredit.Value, Decimal), 0))
                                        'csubtran.acc_base_credit = DecRound(IIf(CType(txtBaseCredit.Value, Decimal) < 0, Math.Abs(CType(txtBaseCredit.Value, Decimal)), 0))
                                        'csubtran.acc_base_debit = DecRound(IIf(CType(txtBaseCredit.Value, Decimal) > 0, CType(txtBaseCredit.Value, Decimal), 0))

                                        csubtran.acc_credit = CType(Val(txtCredit.Value), Decimal)
                                        csubtran.acc_debit = CType(Val(txtDebit.Value), Decimal)
                                        csubtran.acc_base_credit = CType(Val(txtBaseCredit.Value), Decimal)
                                        csubtran.acc_base_debit = CType(Val(txtBaseDebit.Value), Decimal)


                                    End If
                                    csubtran.acc_tran_date = cacc.acc_tran_date
                                    csubtran.acc_due_date = cacc.acc_tran_date
                                    csubtran.acc_field1 = ""
                                    csubtran.acc_field2 = ""
                                    csubtran.acc_field3 = ""
                                    csubtran.acc_field4 = ""
                                    csubtran.acc_field5 = ""
                                    csubtran.acc_tran_id = cacc.acc_tran_id
                                    csubtran.acc_tran_lineno = lblno.Value
                                    csubtran.acc_tran_type = cacc.acc_tran_type
                                    csubtran.acc_narration = txtNarr.Value
                                    csubtran.acc_type = ddlAccType.Value
                                    csubtran.currate = CType(txtCurrRate.Value, Decimal)
                                    'if it is blank then post to default 510
                                    If ddlCCCode.Value <> "[Select]" Then
                                        csubtran.costcentercode = ddlCCCode.Items(ddlCCCode.SelectedIndex).Text
                                    Else
                                        csubtran.costcentercode = strcostcentercode
                                    End If
                                    cacc.addsubtran(csubtran)

                                End If
                            End If
                            Save_Open_detail(lblno.Value, ddlgAccName.Value, ddlAccType.Value, ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text, SqlConn, sqlTrans)

                            If CType(ViewState("ReceiptsRVPVTranType"), String) = "RV" Then
                                'Update the receipt has adjusted from invoice
                                myCommand = New SqlCommand("sp_del_invoice_billadjust_receipt", SqlConn, sqlTrans)
                                myCommand.CommandType = CommandType.StoredProcedure
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.VarChar, 20)).Value = lblno.Value
                                myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)
                                myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                                myCommand.ExecuteNonQuery()
                            End If

                        End If

                    Next

                    'Update the receipt has adjusted from invoice for master
                    If CType(ViewState("ReceiptsRVPVTranType"), String) = "RV" Then
                        myCommand = New SqlCommand("sp_update_invoice_billadjust_master", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@against_tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                        myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                        myCommand.Parameters.Add(New SqlParameter("@amount", SqlDbType.Money)).Value = DecRound(CType(txtAmount.Value, Decimal))
                        myCommand.Parameters.Add(New SqlParameter("@baseamount", SqlDbType.Money)).Value = DecRound(CType(txtCnvAmount.Value, Decimal))
                        myCommand.ExecuteNonQuery()
                    End If

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

                ElseIf ViewState("ReceiptsState") = "Delete" Then
                    If chkPost.Checked = True Then
                        myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "receipt_master_new"
                        myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                        myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                        myCommand.ExecuteNonQuery()
                    End If

                    myCommand = New SqlCommand("sp_del_open_detail_new", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)
                    myCommand.ExecuteNonQuery()


                    myCommand = New SqlCommand("sp_del_receipt", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)
                    '   myCommand.Parameters.Add(New SqlParameter("@user", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()

                ElseIf ViewState("ReceiptsState") = "Cancel" Then
                    If chkPost.Checked = True Then
                        myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "receipt_master_new"
                        myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                        myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                        myCommand.ExecuteNonQuery()
                    End If

                    myCommand = New SqlCommand("sp_cancel_open_detail_new", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)
                    myCommand.ExecuteNonQuery()

                    myCommand = New SqlCommand("sp_cancel_receipt", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)
                    myCommand.Parameters.Add(New SqlParameter("@user", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()
                ElseIf ViewState("ReceiptsState") = "undoCancel" Then
                    myCommand = New SqlCommand("sp_undocancel_receipt", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)
                    myCommand.ExecuteNonQuery()
                End If

                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)
                clsDBConnect.dbCommandClose(myCommand)
                clsDBConnect.dbConnectionClose(SqlConn)
                'Response.Redirect("ReceiptsSearch.aspx?tran_type=" & CType( ViewState("ReceiptsRVPVTranType"), String) & "", False)

                If ViewState("ReceiptsState") = "Delete" Or ViewState("ReceiptsState") = "Cancel" Or ViewState("ReceiptsState") = "undoCancel" Then
                    '   Response.Redirect("ReceiptsSearch.aspx?tran_type=" & CType(ViewState("ReceiptsRVPVTranType"), String) & "", False)
                    btnSave.Enabled = True
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('ReceiptsWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                Else
                    If ViewState("ReceiptsState") = "New" Or ViewState("ReceiptsState") = "Copy" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record save successfully');", True)

                        Dim strscript As String = ""
                        strscript = "window.opener.__doPostBack('ReceiptsWindowPostBack', '');window.opener.focus();window.close();"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                    ElseIf ViewState("ReceiptsState") = "Edit" Then



                        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record update successfully');", True)
                        Dim strURL As String = ""
                        'strURL = "Accnt_trn_amendlog.aspx?tid=" & txtDocNo.Value & "&ttype=" & txtTranType.Value.Trim & "&tdate=" & txtDate.Text.Trim


                        '  ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", String.Format(ScriptOpenModalDialog, strURL, 300), True)

                        strURL = "window.open('Accnt_trn_amendlog.aspx?tid=" & txtDocNo.Value & "&ttype=" & txtTranType.Value.Trim & "&tdate=" & txtDate.Text.Trim + "','Log','width=100,height=100');"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strURL, True)

                        Dim strscript As String = ""
                        strscript = "window.opener.__doPostBack('ReceiptsWindowPostBack', '');window.opener.focus();window.close();"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

                    End If

                    btnPrint.Visible = True
                    If Trim(txtTranType.Value) = "RV" Then
                        chkPrntInclude.Visible = True
                        lblPrntInclude.Visible = True
                    End If
                    btnSave.Enabled = True


                    ViewState("ReceiptsState") = "View"


                    Disabled_Control()
                    'btnPrint_Click(sender, e)
                End If
                Session.Remove("Collection" & ":" & txtAdjcolno.Value)




            End If

            'Dim strscript As String = ""
            'strscript = "window.opener.__doPostBack('ReceiptsWindowPostBack', '');window.opener.focus();window.close();"
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Receipts.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Private Function ValidateChequeno(ByVal chqno As String, ByVal tranid As String) As Boolean
        Dim mReturnVal As Boolean = True
        Dim StrValQry As String = String.Empty
        Dim Count As Integer
        If ViewState("ReceiptsState") = "New" Then
            StrValQry = "SELECT   Count(receipt_cheque_number) FROM receipt_master_new Where receipt_div_id='" & ViewState("divcode") & "' and receipt_cheque_number='" & chqno & "' and tran_type='" & txtTranType.Value & "'"
            Count = CInt(Val(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), StrValQry)))
        ElseIf ViewState("ReceiptsState") = "Edit" Then
            StrValQry = "SELECT   Count(receipt_cheque_number) FROM receipt_master_new Where receipt_div_id='" & ViewState("divcode") & "' and receipt_cheque_number='" & chqno & "' and tran_id <>'" & tranid & "'"
            Count = CInt(Val(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), StrValQry)))
        End If
        If Count > 0 Then
            mReturnVal = False
        End If
        Return mReturnVal
    End Function


#Region "Public Sub Save_Open_detail(ByVal intReceiptLinNo As String, ByVal SqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)"
    Public Sub Save_Open_detail(ByVal intReceiptLinNo As String, ByVal strAccCode As String, ByVal strAccType As String, ByVal strGlCode As String, ByVal SqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction)
        Dim collectionDate As Collection
        Dim spersoncode As String
        Dim strdiv As String
        Dim strLineKey As String
        Dim MainGrdCount As Integer = grdReceipt.Rows.Count

        ' If Session("Collection").ToString <> "" Then
        'collectionDate = CType(Session("Collection"), Collection)
        collectionDate = GetCollectionFromSession()
        If collectionDate.Count <> 0 Then
            strdiv = ViewState("divcode") ' objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
            Dim intcount As Integer = collectionDate.Count / 21
            Dim intLinNo, MainRowidx As Integer
            MainRowidx = 1
            For MainRowidx = 1 To MainGrdCount
                If MainRowidx = intReceiptLinNo Then
                    For intLinNo = 1 To intcount
                        strLineKey = intLinNo & ":" & intReceiptLinNo
                        If colexists(collectionDate, "AgainstTranLineNo" & strLineKey) = True Then
                            If collectionDate("AccCode" & strLineKey).ToString = strAccCode And collectionDate("AccType" & strLineKey).ToString = strAccType And collectionDate("AccGLCode" & strLineKey).ToString = strGlCode Then
                                myCommand = New SqlCommand("sp_add_open_detail_new", SqlConn, sqlTrans)
                                myCommand.CommandType = CommandType.StoredProcedure
                                myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = collectionDate("TranId" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = collectionDate("TranType" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@tran_date ", SqlDbType.DateTime)).Value = Format(CType(collectionDate("TranDate" & strLineKey).ToString, Date), "yyyy/MM/dd")
                                myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int)).Value = collectionDate("AccTranLineNo" & strLineKey)

                                myCommand.Parameters.Add(New SqlParameter("@against_tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int)).Value = intReceiptLinNo 'collectionDate("AgainstTranLineNo" & strLineKey) 'intReceiptLinNo
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_date ", SqlDbType.DateTime)).Value = Format(CType(txtDate.Text, Date), "yyyy/MM/dd")
                                If collectionDate("DueDate" & strLineKey).ToString = "" Then
                                    myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = Format(CType(txtDate.Text, Date), "yyyy/MM/dd") 'DBNull.Value
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@open_due_date ", SqlDbType.DateTime)).Value = Format(CType(collectionDate("DueDate" & strLineKey).ToString, Date), "yyyy/MM/dd")
                                End If

                                If strAccType = "C" Then
                                    spersoncode = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "agentmast", "spersoncode", "agentcode", strAccCode)
                                    myCommand.Parameters.Add(New SqlParameter("@open_sales_code", SqlDbType.VarChar, 10)).Value = spersoncode
                                Else
                                    myCommand.Parameters.Add(New SqlParameter("@open_sales_code", SqlDbType.VarChar, 10)).Value = ""
                                End If
                                myCommand.Parameters.Add(New SqlParameter("@open_debit", SqlDbType.Money)).Value = DecRound(CType(collectionDate("Debit" & strLineKey), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@open_credit", SqlDbType.Money)).Value = DecRound(CType(collectionDate("Credit" & strLineKey), Decimal))

                                myCommand.Parameters.Add(New SqlParameter("@open_field1", SqlDbType.VarChar, 100)).Value = collectionDate("RefNo" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_field2", SqlDbType.VarChar, 100)).Value = collectionDate("Field2" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_field3", SqlDbType.VarChar, 100)).Value = collectionDate("Field3" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_field4", SqlDbType.VarChar, 100)).Value = collectionDate("Field4" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_field5", SqlDbType.VarChar, 100)).Value = collectionDate("Field5" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_mode", SqlDbType.Char, 1)).Value = collectionDate("OpenMode" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@open_exchg_diff", SqlDbType.Money)).Value = 0
                                myCommand.Parameters.Add(New SqlParameter("@dr_cr", SqlDbType.Char, 1)).Value = ""
                                myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                                myCommand.Parameters.Add(New SqlParameter("@currency_rate", SqlDbType.Decimal, 18, 12)).Value = CType(collectionDate("CurrRate" & strLineKey), Decimal)
                                myCommand.Parameters.Add(New SqlParameter("@base_debit", SqlDbType.Money)).Value = DecRound(CType(collectionDate("BaseDebit" & strLineKey), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@base_credit", SqlDbType.Money)).Value = DecRound(CType(collectionDate("BaseCredit" & strLineKey), Decimal))
                                myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.Char, 1)).Value = collectionDate("AccType" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = collectionDate("AccCode" & strLineKey).ToString
                                myCommand.Parameters.Add(New SqlParameter("@acc_gl_code", SqlDbType.VarChar, 20)).Value = collectionDate("AccGLCode" & strLineKey).ToString

                                myCommand.ExecuteNonQuery()
                            End If
                        End If
                    Next
                End If
            Next
        End If
        'End If
    End Sub
#End Region
#Region "Public Function validate_page() As Boolean"
    Public Function validate_page() As Boolean
        validate_page = True
        If ddlCashBank.Value = "[Select]" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select cash or bank type.');", True)
            SetFocus(ddlCashBank)
            validate_page = False
            Exit Function
        End If
        If ddlCashBank.Value = "Bank" Then
            If ddlAccCode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select bank.');", True)
                SetFocus(ddlCashBank)
                validate_page = False
                Exit Function
            End If
        End If
        If CType(Val(txtConvRate.Value), Decimal) = 0 And chkBlank.Checked = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid conversion rate.');", True)
            SetFocus(txtConvRate)
            validate_page = False
            Exit Function
        End If
        If CType(Val(txtCnvAmount.Value), Decimal) = 0 And chkBlank.Checked = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid amount .');", True)
            SetFocus(txtCnvAmount)
            validate_page = False
            Exit Function
        End If
        If txtReceived.Value.Trim = "" And chkBlank.Checked = False Then
            Dim Msg As String = "Please enter"
            If Trim(txtTranType.Value) = "RV" Then
                Msg = Msg + " Received From."
            ElseIf Trim(txtTranType.Value) = "CPV" Then
                Msg = Msg + " Paid To."
            ElseIf Trim(txtTranType.Value) = "BPV" Then

                Msg = Msg + " Paid To."
            ElseIf Trim(txtTranType.Value) = "DEP" Then

                Msg = Msg + " Deposit To."
            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & Msg & "');", True)
            SetFocus(txtReceived)
            validate_page = False
            Exit Function
        End If
        If ddlCashBank.Value = "Bank" And chkBlank.Checked = False Then
            If txtChequeNo.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter cheque no.');", True)
                SetFocus(txtChequeNo)
                validate_page = False
                Exit Function
            End If
            If CType(ViewState("ReceiptsRVPVTranType"), String) = "RV" Then
                If ddlCustBank.Value = "[Select]" And chkBlank.Checked = False Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter customer bank.');", True)
                    SetFocus(ddlCustBank)
                    validate_page = False
                    Exit Function
                End If
            End If
        End If

        'If DecRound(Val(txtCredit.Value)) <> DecRound(Val(txtCnvAmount.Value)) Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('The total of Base credit in grid should be equal to Base amount.');", True)
        '    SetFocus(ddlCustBank)
        '    validate_page = False
        '    Exit Function
        'End If

        Dim ddlAccType As HtmlSelect
        Dim ddlgAccCode As HtmlSelect
        Dim ddlgAccName As HtmlSelect
        Dim ddlConAccCode As HtmlSelect
        Dim ddldept As HtmlSelect
        'Dim txtCredit As HtmlInputControl
        'Dim txtCurrCode As HtmlInputControl
        'Dim txtCurrRate As HtmlInputControl
        Dim lblno, txtCredit, txtCurrCode, txtCurrRate, txtBaseCredit, txtDebit, txtBaseDebit As HtmlInputText

        Dim dfalg As Boolean = True

        For Each gvRow In grdReceipt.Rows
            txtCurrCode = gvRow.FindControl("txtCurrency")
            txtCurrRate = gvRow.FindControl("txtConvRate")

            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")


            ddlAccType = gvRow.FindControl("ddlType")
            ddlgAccCode = gvRow.FindControl("ddlgAccCode")
            ddlgAccName = gvRow.FindControl("ddlgAccName")
            ddlConAccCode = gvRow.FindControl("ddlConAccCode")

            ddldept = gvRow.FindControl("ddldept")

            If ddlAccType.Value.Trim <> "[Select]" And txtCurrRate.Value.Trim <> "" And ddlgAccName.Value.Trim <> "[Select]" Then
                dfalg = False
                If ddlAccType.Value = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select account type.');", True)
                    SetFocus(ddlAccType)
                    validate_page = False
                    Exit Function
                End If
                If ddlgAccName.Value = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select account code.');", True)
                    SetFocus(ddlAccType)
                    validate_page = False
                    Exit Function
                End If
                If ddlAccType.Value <> "G" Then
                    If ddlConAccCode.Value = "[Select]" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Controlaccount code.');", True)
                        SetFocus(ddlAccType)
                        validate_page = False
                        Exit Function
                    End If
                End If

                If txtCurrCode.Value = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select currancy code.');", True)
                    SetFocus(txtCurrCode)
                    validate_page = False
                    Exit Function
                End If
                If CType(Val(txtCurrRate.Value), Decimal) = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid exachange rate.');", True)
                    SetFocus(ddlAccType)
                    validate_page = False
                    Exit Function
                End If

                Dim strMsg As String = ""
                strMsg = "Account Currency  and BaseCurrency are same so Conversion rate should be 1. for this account " + ddlgAccCode.Value
                If txtbasecurr.Value = txtCurrCode.Value And CType(Val(txtCurrRate.Value), Decimal) <> 1 Then
                    'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Account Currency  and BaseCurrency are same so Conversion rate should be 1.' + ddlgAccName.value);", True)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strMsg & "');", True)
                    SetFocus(ddlAccType)
                    validate_page = False
                    Exit Function
                End If





                If CType(Val(txtCredit.Value), Decimal) = 0 And CType(Val(txtDebit.Value), Decimal) = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid amount.');", True)
                    SetFocus(txtCredit)
                    validate_page = False
                    Exit Function
                End If
                Dim AdjVal As Decimal

                If ddlAccType.Value <> "G" Then
                    lblno = gvRow.FindControl("txtlineno")
                    'If Trim(txtTranType.Value) = "RV" Then
                    '    txtBaseCredit = gvRow.FindControl("txtBaseCredit")
                    'Else
                    '    txtBaseCredit = gvRow.FindControl("txtBaseDebit")
                    'End If

                    If CType(Val(txtBaseCredit.Value), Decimal) <> 0 Then
                        AdjVal = CType(Val(txtBaseCredit.Value), Decimal)
                    ElseIf CType(Val(txtBaseCredit.Value), Decimal) = 0 Then
                        AdjVal = CType(Val(txtBaseDebit.Value), Decimal)
                    End If

                    If validate_AdjustBill(lblno.Value, ddlgAccName.Value, ddlAccType.Value, ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text, AdjVal) = False Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid Bill Adjust amount.');", True)
                        SetFocus(txtCredit)
                        validate_page = False
                        Exit Function
                    End If
                End If


                'Dim crdsqlstr As String = "SELECT COUNT(acctcode) FROM acctgroup Where childid IN (115,120) and acctcode='" + ddlgAccCode.Items(ddlgAccCode.SelectedIndex).Text + "'"
                'Dim validMarketCount As Integer = GetScalarValue(Session("dbconnectionName"), crdsqlstr)

                'If validMarketCount > 0 And ddldept.Value = "[Select]" Then
                '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select market name.');", True)
                '    SetFocus(ddldept)
                '    validate_page = False
                '    Exit Function
                'End If

            End If
        Next

        If chkBlank.Checked = True And chkPost.Checked = False Then
            validate_page = True
        Else

            If dfalg = True Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter more than one receipts in grid.');", True)
                validate_page = False
                Exit Function
            End If
        End If

    End Function

    Private Function checkopenmode() As Boolean
        checkopenmode = True
        Dim collectionDate As Collection
        Dim spersoncode As String
        Dim strdiv As String
        Dim strLineKey As String
        Dim MainGrdCount As Integer = grdReceipt.Rows.Count
        Dim dfalg As Boolean = True

        Dim lblno As HtmlInputText
        Dim ddlgAccName As HtmlSelect
        Dim ddlAccType As HtmlSelect
        Dim ddlConAccCode As HtmlSelect
        Dim openmode, popenmode As String

        For Each gvRow In grdReceipt.Rows

            ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            ddlAccType = gvRow.FindControl("ddlType")
            ddlgAccName = gvRow.FindControl("ddlgAccName")
            lblno = gvRow.FindControl("txtlineno")
            If ddlAccType.Value <> "G" And ddlAccType.Value <> "[Select]" Then
                collectionDate = GetCollectionFromSession()
                If collectionDate.Count <> 0 Then
                    strdiv = ViewState("divcode") 'objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
                    Dim intcount As Integer = collectionDate.Count / 21
                    Dim intLinNo, MainRowidx As Integer
                    intLinNo = 1
                    MainRowidx = 1
                    For MainRowidx = 1 To MainGrdCount
                        If MainRowidx = CType(lblno.Value, Integer) Then

                            strLineKey = intLinNo & ":" & CType(lblno.Value, Integer)
                            popenmode = collectionDate("OpenMode" & strLineKey).ToString()
                            For intLinNo = 1 To intcount
                                strLineKey = intLinNo & ":" & CType(lblno.Value, Integer)
                                If colexists(collectionDate, "AgainstTranLineNo" & strLineKey) = True Then
                                    If collectionDate("AccCode" & strLineKey).ToString = CType(ddlgAccName.Value, String) And collectionDate("AccType" & strLineKey).ToString = CType(ddlAccType.Value, String) And collectionDate("AccGLCode" & strLineKey).ToString = CType(ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text, String) Then
                                        openmode = collectionDate("OpenMode" & strLineKey).ToString()
                                        If popenmode <> openmode Then
                                            'Commended
                                            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot create the advance due to amount difference..' );", True)
                                            '    checkopenmode = False
                                            '    chkadjust.Visible = True
                                            '    Exit Function
                                        Else
                                            popenmode = collectionDate("OpenMode" & strLineKey).ToString()
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
            End If
        Next
    End Function
#End Region

    Private Sub GrandToatal()
        Dim gvrow As GridViewRow
        Dim txtbasecr, txtbasedr As HtmlInputText
        Dim totalcr, totaldr As Decimal
        For Each gvrow In grdReceipt.Rows
            txtbasecr = gvrow.FindControl("txtBaseCredit")
            totalcr = totalcr + CType(Val(txtbasecr.Value), Decimal)

            txtbasedr = gvrow.FindControl("txtBaseDebit")
            totaldr = totaldr + CType(Val(txtbasedr.Value), Decimal)
        Next
        txtTotBaseCredit.Value = DecRound(totalcr)
        txtTotBaseDebit.Value = DecRound(totaldr)
        txtTotBaseDiff.Value = Math.Abs(DecRound(CType(Val(txtTotBaseCredit.Value), Decimal) - CType(Val(txtTotBaseDebit.Value), Decimal)))
    End Sub

    Private Function validate_BillAgainst() As Boolean
        Try
            validate_BillAgainst = True
            Dim Alflg As Integer
            Dim ErrMsg, strdiv As String
            strdiv = ViewState("divcode") 'objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myCommand = New SqlCommand("sp_Check_AgainstBills", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
            myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("ReceiptsRVPVTranType"), String)

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
            objUtils.WritErrorLog("requestforinvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function

#Region "Public Function validate_AdjustBill(ByVal intReceiptLinNo As String,  ByVal strGlCode As String) as Boolean"
    Public Function validate_AdjustBill(ByVal intReceiptLinNo As String, ByVal strAccCode As String, ByVal strAccType As String, ByVal strGlCode As String, ByVal Adjustamt As Decimal) As Boolean
        validate_AdjustBill = True
        Dim collectionDate As Collection
        Dim strLineKey As String
        Dim MainGrdCount As Integer = grdReceipt.Rows.Count
        Dim base_debit, base_credit As Decimal
        'If Session("Collection").ToString <> "" Then
        'collectionDate = CType(Session("Collection"), Collection)
        collectionDate = GetCollectionFromSession()
        If collectionDate.Count <> 0 Then
            Dim intcount As Integer = collectionDate.Count / 21
            Dim intLinNo, MainRowidx As Integer
            MainRowidx = 1
            For MainRowidx = 1 To MainGrdCount
                If MainRowidx = intReceiptLinNo Then
                    base_debit = 0
                    base_credit = 0
                    For intLinNo = 1 To intcount
                        strLineKey = intLinNo & ":" & intReceiptLinNo
                        If colexists(collectionDate, "AgainstTranLineNo" & strLineKey) = True Then
                            If collectionDate("OpenMode" & strLineKey).ToString = "B" Then
                                If collectionDate("AccCode" & strLineKey).ToString = strAccCode And collectionDate("AccType" & strLineKey).ToString = strAccType And collectionDate("AccGLCode" & strLineKey).ToString = strGlCode Then
                                    base_debit = DecRound(DecRound(base_debit) + DecRound(CType(collectionDate("BaseDebit" & strLineKey), Decimal)))
                                    base_credit = DecRound(DecRound(base_credit) + DecRound(CType(collectionDate("BaseCredit" & strLineKey), Decimal)))
                                Else
                                    validate_AdjustBill = False
                                    Exit Function

                                End If
                            ElseIf collectionDate("OpenMode" & strLineKey).ToString = "F" Then
                                If collectionDate("AccCode" & strLineKey).ToString = strAccCode And collectionDate("AccType" & strLineKey).ToString = strAccType And collectionDate("AccGLCode" & strLineKey).ToString = strGlCode Then
                                    base_debit = DecRound(DecRound(base_debit) + DecRound(CType(collectionDate("BaseDebit" & strLineKey), Decimal)))
                                    base_credit = DecRound(DecRound(base_credit) + DecRound(CType(collectionDate("BaseCredit" & strLineKey), Decimal)))
                                Else
                                    validate_AdjustBill = False
                                    Exit Function

                                End If

                            Else
                                base_debit = DecRound(DecRound(base_debit) + DecRound(CType(collectionDate("BaseDebit" & strLineKey), Decimal)))
                                base_credit = DecRound(DecRound(base_credit) + DecRound(CType(collectionDate("BaseCredit" & strLineKey), Decimal)))
                            End If
                        End If
                    Next
                End If
            Next
        End If
        'End If
        'If DecRound(Adjustamt) = DecRound(base_debit) Or DecRound(Adjustamt) = DecRound(base_credit) Or Math.Abs(DecRound(base_debit) - DecRound(base_credit)) = DecRound(Adjustamt) Then
        'Else
        '    validate_AdjustBill = False
        '    Exit Function
        'End If


        Dim totamt As Decimal
        ''If Trim(txtTranType.Value) = "RV" Then 'txtGridType.Value = "Credit" Then
        ''    totamt = DecRound(DecRound(base_credit) - DecRound(base_debit))
        ''Else
        ''    totamt = DecRound(DecRound(base_debit) - DecRound(base_credit))
        ''End If

        If base_credit <> 0 Then
            totamt = DecRound(DecRound(base_credit) - DecRound(base_debit))
        ElseIf base_credit = 0 Then
            totamt = DecRound(DecRound(base_debit) - DecRound(base_credit))
        End If
        If Math.Abs(DecRound(Adjustamt)) = Math.Abs(DecRound(totamt)) Then
        Else
            validate_AdjustBill = False
            Exit Function
        End If


        '  If DecRound(Adjustamt) = DecRound(base_debit) Or DecRound(Adjustamt) = DecRound(base_credit) Then
        'Dim totamt As Decimal
        'If txtGridType.Value = "Credit" Then
        '    totamt = DecRound(DecRound(base_credit) - DecRound(base_debit))
        'ElseIf txtGridType.Value = "Debit" Then
        '    totamt = DecRound(DecRound(base_debit) - DecRound(base_credit))
        'End If
        'If DecRound(Adjustamt) = DecRound(totamt) Then
        'Else
        '    validate_AdjustBill = False
        '    Exit Function
        'End If
    End Function
#End Region
#Region "Public Sub show_record()"
    Public Sub show_record(ByVal RefCode As String)
        Dim mySqlReader As SqlDataReader
        Try
            chkPost.Checked = False
            Dim myDS As New DataSet

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            myCommand = New SqlCommand("select * from receipt_master_new Where receipt_div_id='" & ViewState("divcode") & "' and tran_id='" & RefCode & "' and tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "'", SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)

            If mySqlReader.HasRows Then

                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("post_state")) = False And ViewState("ReceiptsState") <> "Copy" Then
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

                    If IsDBNull(mySqlReader("cancel_state")) = False Then
                        If CType(mySqlReader("cancel_state"), String) = "Y" Then
                            lblPostmsg.Text = "Cancelled"
                            lblPostmsg.ForeColor = Drawing.Color.Green
                        End If
                    End If

                    If IsDBNull(mySqlReader("tran_id")) = False Then
                        If ViewState("ReceiptsState") = "Copy" Then
                            txtDocNo.Value = ""
                        Else
                            txtDocNo.Value = CType(mySqlReader("tran_id"), String)
                        End If
                    End If
                    If IsDBNull(mySqlReader("receipt_date")) = False Then
                        txtDate.Text = Format(CType(mySqlReader("receipt_date"), Date), "dd/MM/yyyy")
                    End If
                    If IsDBNull(mySqlReader("receipt_mrv")) = False Then
                        txtMRV.Value = mySqlReader("receipt_mrv")
                    End If
                    If IsDBNull(mySqlReader("receipt_narration")) = False Then
                        txtnarration.Text = mySqlReader("receipt_narration")
                    End If
                    If IsDBNull(mySqlReader("receipt_received_from")) = False Then
                        txtReceived.Value = mySqlReader("receipt_received_from")
                    End If
                    If CType(ViewState("ReceiptsRVPVTranType"), String) = "RV" Then
                        If IsDBNull(mySqlReader("receipt_customer_bank")) = False Then
                            ddlCustBank.Value = mySqlReader("receipt_customer_bank")
                        End If
                    End If
                    If IsDBNull(mySqlReader("receipt_cheque_number")) = False Then
                        txtChequeNo.Value = mySqlReader("receipt_cheque_number")
                    End If
                    If IsDBNull(mySqlReader("cheque_date")) = False Then
                        txtChequeDate.Text = Format(CType(mySqlReader("cheque_date"), Date), "dd/MM/yyyy")
                    End If

                    '15122014
                    If IsDBNull(mySqlReader("mktcode")) = False Then
                        ddlSMktName.Value = mySqlReader("mktcode").ToString

                        ddlSMktCode.Value = ddlSMktName.Items(ddlSMktName.SelectedIndex).Text

                    End If

                    'If IsDBNull(mySqlReader("receipt_cheque_number")) = False Then
                    '    txtChequeNo.Value = mySqlReader("receipt_cheque_number")
                    'End If

                    If mySqlReader("receipt_cashbank_type").ToString = "C" Then
                        ddlCashBank.Value = "Cash"

                        If CType(ViewState("ReceiptsRVPVTranType"), String) = "RV" Then
                            ddlCustBank.Style("VISIBILITY") = "visible"
                            txtChequeNo.Style("VISIBILITY") = "visible"
                            txtChequeDate.Style("VISIBILITY") = "visible"
                            ImageButton1.Style("VISIBILITY") = "visible"
                            lblChN.Style("VISIBILITY") = "visible"
                            lblChB.Style("VISIBILITY") = "visible"
                            lblChD.Style("VISIBILITY") = "visible"
                        Else
                            ddlCustBank.Style("VISIBILITY") = "hidden"
                            txtChequeNo.Style("VISIBILITY") = "hidden"
                            txtChequeDate.Style("VISIBILITY") = "hidden"
                            ImageButton1.Style("VISIBILITY") = "hidden"
                            lblChN.Style("VISIBILITY") = "hidden"
                            lblChB.Style("VISIBILITY") = "hidden"
                            lblChD.Style("VISIBILITY") = "hidden"
                        End If

                    Else
                        ddlCashBank.Value = "Bank"

                        txtChequeNo.Style("VISIBILITY") = "visible"
                        txtChequeDate.Style("VISIBILITY") = "visible"
                        ImageButton1.Style("VISIBILITY") = "visible"
                        lblChN.Style("VISIBILITY") = "visible"
                        lblChD.Style("VISIBILITY") = "visible"
                        If CType(ViewState("ReceiptsRVPVTranType"), String) = "RV" Then
                            ddlCustBank.Style("VISIBILITY") = "visible"
                            lblChB.Style("VISIBILITY") = "visible"
                        Else 'If CType(ViewState("ReceiptsRVPVTranType"), String) = "PV" Then
                            ddlCustBank.Style("VISIBILITY") = "hidden"
                            lblChB.Style("VISIBILITY") = "hidden"
                        End If
                    End If

                    If IsDBNull(mySqlReader("receipt_cashbank_code")) = False Then
                        txtAccName.Value = mySqlReader("receipt_cashbank_code")
                        txtAccCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast  where div_code='" & ViewState("divcode") & "' and acctcode ='" & CType(mySqlReader("receipt_cashbank_code"), String) & "'")
                        txtCurrCode.Value = mySqlReader("receipt_cashbank_code")
                    End If

                    'ddlAccName.Value = mySqlReader("receipt_cashbank_code").ToString
                    'ddlAccCode.Value = ddlAccName.Items(ddlAccName.SelectedIndex).Text
                    'ddlCurrCode.Value = ddlAccName.Value

                    If IsDBNull(mySqlReader("receipt_currency_rate")) = False Then
                        txtConvRate.Value = CType(mySqlReader("receipt_currency_rate"), Decimal)
                    End If
                    If Trim(txtTranType.Value) = "RV" Then
                        txtAmount.Value = DecRound(Val(mySqlReader("receipt_credit")))
                        txtCnvAmount.Value = DecRound(Val(mySqlReader("basecredit")))
                    Else 'If Trim(txtTranType.Value) = "PV" Then
                        txtAmount.Value = DecRound(Val(mySqlReader("receipt_debit")))
                        txtCnvAmount.Value = DecRound(Val(mySqlReader("basedebit")))
                    End If

                    'txtTotalBCredit.Value = DecRound(Val(txtCnvAmount.Value))
                    txtOldAmount.Value = DecRound(Val(txtCnvAmount.Value))


                    Dim sqlstr As String
                    Dim cbalancamt, balamt As Decimal
                    ' sqlstr = "sp_get_account_balance  '" + ViewState("divcode") & "' and  'G','" + txtAccName.Value + "'"
                    sqlstr = "sp_get_account_balance  '" + txtDivCode.Value + "','G','" + txtAccName.Value + "'"
                    cbalancamt = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), sqlstr)
                    If (DecRound(cbalancamt) > 0) Then

                        lblBalCrDr.Text = "Cr"

                    Else

                        lblBalCrDr.Text = "Dr"

                    End If

                    '//txtbalan.value=Math.abs(balance);
                    '//alert(String(Math.abs(balance)));
                    '//txtbalan.value=DecFormat(String(Math.abs(balance)));

                    txtBalance.Value = Math.Abs(DecRound(cbalancamt)) 'DecRound(cbalancamt)
                    If (txtMode.Value = "Edit") Then
                        If Trim(txtTranType.Value) = "RV" Then
                            balamt = DecRound(DecRound(cbalancamt) - DecRound(Val(txtOldAmount.Value)))
                        Else 'If Trim(txtTranType.Value) = "PV" Then
                            balamt = DecRound(DecRound(cbalancamt) + DecRound(Val(txtOldAmount.Value)))
                        End If
                        txtBalance.Value = Math.Abs(DecRound(balamt))
                    End If
                    FillCashBankDetails()
                    txtConvRate.Disabled = False
                    If Trim(txtbasecurr.Value) = Trim(ddlCurrCode.Items(ddlCurrCode.SelectedIndex).Text) Then
                        txtConvRate.Disabled = True
                    End If
                End If

            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Receipts.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try
    End Sub
#End Region
#Region "Public Sub Disabled_Control()"
    Public Sub Disabled_Control()
        txtDocNo.Disabled = True
        If ViewState("ReceiptsState") = "New" Or ViewState("ReceiptsState") = "Copy" Then
            btnPrint.Visible = False
            If Trim(txtTranType.Value) = "RV" Then
                chkPrntInclude.Visible = False
                lblPrntInclude.Visible = False
            End If
        ElseIf ViewState("ReceiptsState") = "Edit" Then
            btnPrint.Visible = False
            If Trim(txtTranType.Value) = "RV" Then
                chkPrntInclude.Visible = False
                lblPrntInclude.Visible = False
            End If
        ElseIf ViewState("ReceiptsState") = "Delete" Or ViewState("ReceiptsState") = "View" Or ViewState("ReceiptsState") = "Cancel" Or ViewState("ReceiptsState") = "undoCancel" Then
            ddlRecveidfrom.Disabled = True
            ddlNarration.Disabled = True
            ddlCashBank.Disabled = True
            ddlAccCode.Disabled = True
            ddlAccName.Disabled = True
            ddlCustBank.Disabled = True
            txtChequeDate.Enabled = False
            txtDate.Enabled = False
            txtChequeNo.Disabled = True
            txtBalance.Disabled = True
            txtAmount.Disabled = True
            txtCnvAmount.Disabled = True
            txtConvRate.Disabled = True
            txtnarration.Enabled = False
            txtReceived.Disabled = True
            txtMRV.Disabled = True
            ddlSMktCode.Disabled = True
            ddlSMktName.Disabled = True

            btnPrint.Visible = False
            If Trim(txtTranType.Value) = "RV" Then
                chkPrntInclude.Visible = False
                lblPrntInclude.Visible = False
            End If
            If ViewState("ReceiptsState") = "undoCancel" Then
                btnSave.Visible = True
                btnPrint.Visible = False
                If Trim(txtTranType.Value) = "RV" Then
                    chkPrntInclude.Visible = False
                    lblPrntInclude.Visible = False
                End If
            Else
                btnSave.Visible = False
            End If

            btnAdd.Visible = False
            btnDelLine.Visible = False
            chkPost.Visible = False
            DisableGrid()
        End If

        If ViewState("ReceiptsState") = "View" Then
            btnPrint.Visible = True
            If Trim(txtTranType.Value) = "RV" Then
                chkPrntInclude.Visible = True
                lblPrntInclude.Visible = True
            End If
        ElseIf ViewState("ReceiptsState") = "Delete" Or ViewState("ReceiptsState") = "Cancel" Then
            btnPrint.Visible = False
            If Trim(txtTranType.Value) = "RV" Then
                chkPrntInclude.Visible = False
                lblPrntInclude.Visible = False
            End If
            btnSave.Visible = True
        End If

    End Sub
#End Region
    Private Sub DisableGrid()

        Dim ddlAccType As HtmlSelect
        Dim ddlgAccCode As HtmlSelect
        Dim ddlgAccName As HtmlSelect
        Dim ddlConAccCode As HtmlSelect
        Dim ddlConAccName As HtmlSelect
        Dim ddlCCCode As HtmlSelect
        Dim ddlCCName As HtmlSelect
        Dim txtCredit As HtmlInputControl
        Dim txtCurrCode As HtmlInputControl
        Dim txtCurrRate As HtmlInputControl

        '15122014
        Dim ddldept As HtmlSelect

        Dim txtNarr As HtmlInputControl
        Dim txtBaseCredit As HtmlInputText
        Dim lblno As HtmlInputText
        Dim btnBill As HtmlInputButton
        Dim txtrequestid As HtmlInputControl
        Dim txtDebit, txtBaseDebit As HtmlInputText

        For Each gvRow In grdReceipt.Rows
            txtCurrCode = gvRow.FindControl("txtCurrency")
            txtCurrRate = gvRow.FindControl("txtConvRate")

            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtNarr = gvRow.FindControl("txtgnarration")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")
            ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            ddlConAccName = gvRow.FindControl("ddlConAccName")
            ddlAccType = gvRow.FindControl("ddlType")
            ddlgAccCode = gvRow.FindControl("ddlgAccCode")
            ddlgAccName = gvRow.FindControl("ddlgAccName")
            ddlCCCode = gvRow.FindControl("ddlCostCode")
            ddlCCName = gvRow.FindControl("ddlCostName")
            lblno = gvRow.FindControl("txtlineno")
            btnBill = gvRow.FindControl("btnAd")
            txtrequestid = gvRow.FindControl("txtrequestid")
            '15122014
            ddldept = gvRow.FindControl("ddldept")

            btnBill.Disabled = False
            ddlAccType.Disabled = True
            ddlgAccCode.Disabled = True
            ddlgAccName.Disabled = True
            ddlConAccCode.Disabled = True
            ddlConAccName.Disabled = True
            txtCredit.Disabled = True
            txtCurrCode.Disabled = True
            txtCurrRate.Disabled = True
            txtNarr.Disabled = True
            ddlCCCode.Disabled = True
            ddlCCName.Disabled = True
            txtrequestid.Disabled = True
            ddldept.Disabled = True
        Next

    End Sub
#Region "Public Sub fillcollection(ByVal tranid As String, ByVal lineno As Integer)"
    Public Sub fillcollection(ByVal tranid As String)

        Dim clAdBill As New Collection
        Dim strLineKey As String
        Dim intLineNo As Long = 1
        Dim MainRowct As Long
        Dim MainRowindex As Long
        Dim myDS As New DataSet
        Dim mySqlReader As SqlDataReader
        Dim rowbasetotal As Decimal
        MainRowct = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select count(*) from receipt_detail Where   div_id='" & ViewState("divcode") & "' and  receipt_acc_type<>'G' and tran_id='" & tranid & "' and tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "'")
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
        myCommand = New SqlCommand("select * from  open_detail Where  div_id='" & ViewState("divcode") & "' and against_tran_id='" & tranid & "' and against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' order by against_tran_lineno,tran_lineno ", SqlConn)
        mySqlReader = myCommand.ExecuteReader()
        If mySqlReader.HasRows Then
            While mySqlReader.Read()
                If MainRowindex <> mySqlReader("against_tran_lineno") Then
                    intLineNo = 1
                End If
                strLineKey = intLineNo & ":" & mySqlReader("against_tran_lineno")
                AddCollection(clAdBill, "AgainstTranLineNo" & strLineKey, mySqlReader("against_tran_lineno")) 'intLineNo.ToString)
                AddCollection(clAdBill, "AccTranLineNo" & strLineKey, mySqlReader("tran_lineno"))
                AddCollection(clAdBill, "TranId" & strLineKey, mySqlReader("tran_id"))
                AddCollection(clAdBill, "TranDate" & strLineKey, Format(CType(mySqlReader("tran_date"), Date), "dd/MM/yyyy"))
                AddCollection(clAdBill, "TranType" & strLineKey, mySqlReader("tran_type"))
                AddCollection(clAdBill, "DueDate" & strLineKey, Format(CType(mySqlReader("open_due_date"), Date), "dd/MM/yyyy"))
                AddCollection(clAdBill, "CurrRate" & strLineKey, mySqlReader("currency_rate"))
                AddCollection(clAdBill, "Credit" & strLineKey, DecRound(mySqlReader("open_credit")))
                AddCollection(clAdBill, "Debit" & strLineKey, DecRound(mySqlReader("open_debit")))
                AddCollection(clAdBill, "BaseCredit" & strLineKey, DecRound(mySqlReader("Base_Credit")))
                AddCollection(clAdBill, "BaseDebit" & strLineKey, DecRound(mySqlReader("Base_Debit")))
                AddCollection(clAdBill, "RefNo" & strLineKey, mySqlReader("open_field1"))
                AddCollection(clAdBill, "Field2" & strLineKey, mySqlReader("open_field2"))
                AddCollection(clAdBill, "Field3" & strLineKey, mySqlReader("open_field3"))
                AddCollection(clAdBill, "Field4" & strLineKey, mySqlReader("open_field4"))
                AddCollection(clAdBill, "Field5" & strLineKey, mySqlReader("open_field5"))
                AddCollection(clAdBill, "OpenMode" & strLineKey, mySqlReader("open_mode"))
                AddCollection(clAdBill, "AccType" & strLineKey, mySqlReader("Acc_Type"))
                AddCollection(clAdBill, "AccCode" & strLineKey, mySqlReader("Acc_Code"))
                AddCollection(clAdBill, "AccGLCode" & strLineKey, mySqlReader("Acc_GL_Code"))
                rowbasetotal = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select case isnull(tran_type,'') when 'RV' then basecredit  else basedebit end basetotal   from receipt_detail Where   receipt_acc_type<>'G' and tran_id='" & tranid & "' and tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' and tran_lineno='" & mySqlReader("against_tran_lineno") & "'")
                AddCollection(clAdBill, "AdjustBaseTotal" & strLineKey, DecRound(rowbasetotal)) 'mySqlReader("Acc_GL_Code"))

                MainRowindex = mySqlReader("against_tran_lineno")
                intLineNo = intLineNo + 1
            End While
        End If
        myCommand.Dispose()
        SqlConn.Close()

        Session.Add("Collection" & ":" & txtAdjcolno.Value, clAdBill)
        ' Session.Add("Collection", clAdBill)

    End Sub
#End Region
#Region "Public Sub AddCollection(ByVal dataCollection As Collection, ByVal strKey As String, ByVal strVal As String)"
    Public Sub AddCollection(ByVal dataCollection As Collection, ByVal strKey As String, ByVal strVal As String)
        If colexists(dataCollection, strKey) = False Then
            dataCollection.Add(strVal, strKey, Nothing, Nothing)
        Else
            dataCollection.Remove(strKey)
            dataCollection.Add(strVal, strKey, Nothing, Nothing)
        End If
    End Sub
#End Region
#Region "Private Function colexists(ByVal newcol As Collection, ByVal newkey As String) As Boolean"
    Private Function colexists(ByVal newcol As Collection, ByVal newkey As String) As Boolean
        Try
            Dim k As Integer
            colexists = False
            If newcol.Count > 0 Then
                For k = 1 To newcol.Count
                    ' If newcol(newkey).ToString <> "" Then
                    If newcol.Contains(newkey) = True Then
                        colexists = True
                        Exit Function
                    End If
                Next
            End If
        Catch ex As Exception
            colexists = False
        End Try
    End Function
#End Region

#Region "Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click"
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim n As Integer = 0
        Dim count As Integer
        count = grdReceipt.Rows.Count + 1
        Dim lineno(count) As String

        Dim Olineno(count) As String
        Dim acctype(count) As String
        Dim acccode(count) As String
        Dim accname(count) As String
        Dim controlcode(count) As String
        Dim controlname(count) As String
        Dim CCCode(count) As String
        Dim CCName(count) As String

        Dim narration(count) As String
        Dim crate(count) As String
        Dim currcode(count) As String

        Dim credit(count) As String
        Dim bcredit(count) As String

        Dim debit(count) As String
        Dim bdebit(count) As String

        Dim requestid(count) As String

        Dim dept(count) As String

        Dim ddlAccType As HtmlSelect
        Dim ddlgAccCode As HtmlSelect
        Dim ddlgAccName As HtmlSelect
        Dim ddlConAccCode As HtmlSelect
        Dim ddlConAccName As HtmlSelect
        Dim txtCredit As HtmlInputControl
        Dim txtCurrCode As HtmlInputControl
        Dim txtCurrRate As HtmlInputControl
        Dim ddlCCCode As HtmlSelect
        Dim ddlCCName As HtmlSelect

        '15122014
        Dim ddldept As HtmlSelect

        Dim txtNarr As HtmlInputControl
        Dim txtBaseCredit As HtmlInputText
        Dim lblno As HtmlInputText
        Dim txtrequestid As HtmlInputText

        Dim txtacctcode, txtacctname, txtctrolaccode, txtcontrolacname, txtOldLineno, txtDebit, txtBaseDebit As HtmlInputText
        Dim sqlstr1, sqlstr2 As String

        For Each gvRow In grdReceipt.Rows
            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")
            ddlAccType = gvRow.FindControl("ddlType")
            'ddlgAccCode = gvRow.FindControl("ddlgAccCode")
            'ddlgAccName = gvRow.FindControl("ddlgAccName")

            'ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            'ddlConAccName = gvRow.FindControl("ddlConAccName")
            ddlCCCode = gvRow.FindControl("ddlCostCode")
            ddlCCName = gvRow.FindControl("ddlCostName")

            txtCurrCode = gvRow.FindControl("txtCurrency")
            txtCurrRate = gvRow.FindControl("txtConvRate")

            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtNarr = gvRow.FindControl("txtgnarration")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtacctcode = gvRow.FindControl("txtacctcode")
            txtacctname = gvRow.FindControl("txtacctname")
            txtctrolaccode = gvRow.FindControl("txtctrolaccode")
            txtcontrolacname = gvRow.FindControl("txtcontrolacname")
            txtrequestid = gvRow.FindControl("txtrequestid")

            ddldept = gvRow.FindControl("ddldept")

            '  If ddlgAccName.Value <> "[Select]" And ddlAccType.Value <> "[Select]" Then
            If txtacctcode.Value <> "[Select]" And ddlAccType.Value <> "[Select]" Then
                Olineno(n) = txtOldLineno.Value
                acctype(n) = ddlAccType.Value
                acccode(n) = txtacctcode.Value
                accname(n) = txtacctname.Value 'ddlgAccName.Value
                controlcode(n) = txtctrolaccode.Value
                controlname(n) = txtcontrolacname.Value
                CCCode(n) = CType(ddlCCCode.Value, String)
                CCName(n) = CType(ddlCCName.Value, String)

                currcode(n) = txtCurrCode.Value
                crate(n) = txtCurrRate.Value
                narration(n) = txtNarr.Value
                credit(n) = txtCredit.Value
                bcredit(n) = txtBaseCredit.Value
                debit(n) = txtDebit.Value
                bdebit(n) = txtBaseDebit.Value
                requestid(n) = txtrequestid.Value

                dept(n) = CType(ddldept.Value, String)

                n = n + 1
            End If
        Next

        fillDategrd(grdReceipt, False, grdReceipt.Rows.Count + 1)
        Dim i As Integer = n
        n = 0

        For Each gvRow In grdReceipt.Rows
            If n = i Then
                Exit For
            End If

            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")

            ddlAccType = gvRow.FindControl("ddlType")
            ddlgAccCode = gvRow.FindControl("ddlgAccCode")
            ddlgAccName = gvRow.FindControl("ddlgAccName")
            ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            ddlConAccName = gvRow.FindControl("ddlConAccName")
            ddlCCCode = gvRow.FindControl("ddlCostCode")
            ddlCCName = gvRow.FindControl("ddlCostName")

            txtCurrCode = gvRow.FindControl("txtCurrency")
            txtCurrRate = gvRow.FindControl("txtConvRate")

            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtNarr = gvRow.FindControl("txtgnarration")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtacctcode = gvRow.FindControl("txtacctcode")
            txtacctname = gvRow.FindControl("txtacctname")
            txtctrolaccode = gvRow.FindControl("txtctrolaccode")
            txtcontrolacname = gvRow.FindControl("txtcontrolacname")
            txtrequestid = gvRow.FindControl("txtrequestid")

            ddldept = gvRow.FindControl("ddldept")


            txtOldLineno.Value = Olineno(n)

            txtacctcode.Value = acccode(n)
            txtacctname.Value = accname(n)
            txtctrolaccode.Value = controlcode(n)
            txtcontrolacname.Value = controlname(n)

            ddlAccType.Value = acctype(n)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccCode, "Code", "des", "select Code,des from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "'   order by code", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccName, "des", "Code", "select Code,des from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "'  order by des", True)



            ddlgAccName.Value = accname(n)
            ddlgAccCode.Value = acccode(n)

            ddlConAccCode.Disabled = False
            ddlConAccName.Disabled = False
            ddlCCCode.Disabled = True
            ddlCCName.Disabled = True
            txtCurrRate.Disabled = False

            If acctype(n) = "G" Then
                sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
                sqlstr2 = " select  '' as acctname , '' as controlacctcode "
                ddlConAccCode.Disabled = True
                ddlConAccName.Disabled = True
                ddlCCCode.Disabled = False
                ddlCCName.Disabled = False
            ElseIf acctype(n) = "C" Then
                sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where acctmast.div_code=view_account.div_code and acctmast.div_code='" & ViewState("divcode") & "' and    view_account.controlacctcode= acctmast.acctcode  and type= '" + acctype(n) + "' and view_account.code='" + accname(n) + "' order by  view_account.controlacctcode"
                sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where acctmast.div_code=view_account.div_code and acctmast.div_code='" & ViewState("divcode") & "' and  view_account.controlacctcode= acctmast.acctcode  and type= '" + acctype(n) + "' and view_account.code='" + accname(n) + "' order by  acctmast.acctname"
            ElseIf acctype(n) = "S" Then
                sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where  acctmast.div_code='" & ViewState("divcode") & "' and  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' "
                sqlstr1 = sqlstr1 + " Union all  select distinct partymast.accrualacctcode as controlacctcode, acctmast.acctname  from acctmast ,partymast where acctmast.div_code='" & ViewState("divcode") & "' and  partymast.accrualacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' order by controlacctcode "

                sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where acctmast.div_code='" & ViewState("divcode") & "' and  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' "
                sqlstr2 = sqlstr2 + " Union all  select distinct acctmast.acctname ,partymast.accrualacctcode as controlacctcode   from acctmast ,partymast where acctmast.div_code='" & ViewState("divcode") & "' and partymast.accrualacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' order by acctmast.acctname "
            ElseIf acctype(n) = "A" Then
                sqlstr1 = " select distinct supplier_agents.controlacctcode    , acctmast.acctname  from acctmast ,supplier_agents where acctmast.div_code='" & ViewState("divcode") & "' and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' "
                sqlstr1 = sqlstr1 + " Union all  select distinct supplier_agents.accrualacctcode as controlacctcode, acctmast.acctname  from acctmast.div_code='" & ViewState("divcode") & "' and acctmast ,supplier_agents where   supplier_agents.accrualacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' order by controlacctcode "

                sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where acctmast.div_code='" & ViewState("divcode") & "' and   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' "
                sqlstr2 = sqlstr2 + " Union all  select distinct acctmast.acctname ,supplier_agents.accrualacctcode as controlacctcode   from acctmast ,supplier_agents where  acctmast.div_code='" & ViewState("divcode") & "' and  supplier_agents.accrualacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' order by acctmast.acctname "

            End If
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True)


            ddlConAccCode.Value = controlcode(n)
            ddlConAccName.Value = controlname(n)

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)


            '15122014
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "othmaingrpname", "othmaingrpcode", "select othmaingrpcode,othmaingrpname from othmaingrpmast where active=1 order by othmaingrpcode ", True)

            'Added By Riswan
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)


            ddlCCCode.Value = CCCode(n)
            ddlCCName.Value = CCName(n)

            txtCurrCode.Value = currcode(n)
            txtCurrRate.Value = crate(n)
            txtNarr.Value = narration(n)
            'txtCredit.Value = credit(n)
            'txtBaseCredit.Value = bcredit(n)


            'If Trim(txtTranType.Value) = "RV" Then
            '    txtCredit.Value = credit(n)
            '    txtBaseCredit.Value = bcredit(n)
            'ElseIf Trim(txtTranType.Value) = "BPV" Or Trim(txtTranType.Value) = "CPV" Then
            '    txtDebit.Value = debit(n)
            '    txtBaseDebit.Value = bdebit(n)
            'End If

            If Val(credit(n)) <> 0 Then
                txtCredit.Value = credit(n)
                txtBaseCredit.Value = bcredit(n)
            ElseIf Val(credit(n)) = 0 Then
                txtDebit.Value = debit(n)
                txtBaseDebit.Value = bdebit(n)
            End If

            txtrequestid.Value = requestid(n)

            ddldept.Value = dept(n)

            If Trim(txtbasecurr.Value) = Trim(txtCurrCode.Value) Then
                txtCurrRate.Disabled = True
            End If
            n = n + 1
        Next
    End Sub
#End Region

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Response.Redirect("ReceiptsSearch.aspx?tran_type=" & CType(ViewState("ReceiptsRVPVTranType"), String) & "", False)
        Session.Remove("Collection" & ":" & txtAdjcolno.Value)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ReceiptsWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

    End Sub


    Private Sub FillGrid()
        Dim n As Integer = 0
        Dim count As Integer
        count = grdReceipt.Rows.Count
        Dim lineno(count) As String
        Dim Olineno(count) As String
        Dim acctype(count) As String
        Dim acccode(count) As String
        Dim accname(count) As String
        Dim controlcode(count) As String
        Dim controlname(count) As String
        Dim CCCode(count) As String
        Dim CCName(count) As String
        Dim narration(count) As String
        Dim crate(count) As String
        Dim currcode(count) As String
        Dim debit(count) As String
        Dim credit(count) As String
        Dim bdebit(count) As String
        Dim bcredit(count) As String
        Dim ckDeletion(count) As String

        Dim dept(count) As String

        Dim ddlAccType As HtmlSelect
        Dim ddlgAccCode As HtmlSelect
        Dim ddlgAccName As HtmlSelect
        Dim ddlConAccCode As HtmlSelect
        Dim ddlConAccName As HtmlSelect
        Dim ddlCCCode As HtmlSelect
        Dim ddlCCName As HtmlSelect
        Dim txtCredit, txtCurrCode, txtCurrRate, txtNarr, txtBaseCredit, txtOldLineno, txtDebit, txtBaseDebit As HtmlInputText
        Dim lblno As HtmlInputText
        Dim chckDeletion As CheckBox

        Dim ddldept As HtmlSelect

        Dim txtacctcode, txtacctname, txtctrolaccode, txtcontrolacname As HtmlInputText
        Dim sqlstr1, sqlstr2 As String

        For Each gvRow In grdReceipt.Rows
            chckDeletion = gvRow.FindControl("chckDeletion")
            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")

            ddlAccType = gvRow.FindControl("ddlType")
            ddlCCCode = gvRow.FindControl("ddlCostCode")
            ddlCCName = gvRow.FindControl("ddlCostName")

            'ddlgAccCode = gvRow.FindControl("ddlgAccCode")
            'ddlgAccName = gvRow.FindControl("ddlgAccName")

            'ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            'ddlConAccName = gvRow.FindControl("ddlConAccName")

            txtCurrCode = gvRow.FindControl("txtCurrency")
            txtCurrRate = gvRow.FindControl("txtConvRate")

            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtNarr = gvRow.FindControl("txtgnarration")
            txtacctcode = gvRow.FindControl("txtacctcode")
            txtacctname = gvRow.FindControl("txtacctname")
            txtctrolaccode = gvRow.FindControl("txtctrolaccode")
            txtcontrolacname = gvRow.FindControl("txtcontrolacname")

            ddldept = gvRow.FindControl("ddldept")

            If txtacctcode.Value <> "[Select]" And ddlAccType.Value <> "[Select]" Then
                If chckDeletion.Checked = True Then
                    ckDeletion(n) = 1
                Else
                    ckDeletion(n) = 0
                End If
                Olineno(n) = txtOldLineno.Value
                acctype(n) = ddlAccType.Value
                acccode(n) = txtacctcode.Value
                accname(n) = txtacctname.Value 'ddlgAccName.Value
                controlcode(n) = txtctrolaccode.Value
                controlname(n) = txtcontrolacname.Value
                CCCode(n) = CType(ddlCCCode.Value, String)
                CCName(n) = CType(ddlCCName.Value, String)

                currcode(n) = txtCurrCode.Value
                crate(n) = txtCurrRate.Value

                credit(n) = txtCredit.Value
                debit(n) = txtDebit.Value
                bcredit(n) = txtBaseCredit.Value
                bdebit(n) = txtBaseDebit.Value
                narration(n) = txtNarr.Value
                dept(n) = CType(ddldept.Value, String)
                n = n + 1
            End If
        Next

        fillDategrd(grdReceipt, False, grdReceipt.Rows.Count)
        Dim i As Integer = n
        n = 0

        For Each gvRow In grdReceipt.Rows
            If n = i Then
                Exit For
            End If
            chckDeletion = gvRow.FindControl("chckDeletion")

            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")
            ddlAccType = gvRow.FindControl("ddlType")
            ddlgAccCode = gvRow.FindControl("ddlgAccCode")
            ddlgAccName = gvRow.FindControl("ddlgAccName")
            ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            ddlConAccName = gvRow.FindControl("ddlConAccName")
            ddlCCCode = gvRow.FindControl("ddlCostCode")
            ddlCCName = gvRow.FindControl("ddlCostName")

            ddldept = gvRow.FindControl("ddldept")

            txtCurrCode = gvRow.FindControl("txtCurrency")
            txtCurrRate = gvRow.FindControl("txtConvRate")


            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtNarr = gvRow.FindControl("txtgnarration")
            txtacctcode = gvRow.FindControl("txtacctcode")
            txtacctname = gvRow.FindControl("txtacctname")
            txtctrolaccode = gvRow.FindControl("txtctrolaccode")
            txtcontrolacname = gvRow.FindControl("txtcontrolacname")

            If ckDeletion(n) = 1 Then
                chckDeletion.Checked = True
            Else
                chckDeletion.Checked = False
            End If
            txtOldLineno.Value = Olineno(n)
            txtacctcode.Value = acccode(n)
            txtacctname.Value = accname(n)
            txtctrolaccode.Value = controlcode(n)
            txtcontrolacname.Value = controlname(n)


            ddlAccType.Value = acctype(n)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccCode, "Code", "des", "select Code,des from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "'   order by code", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccName, "des", "Code", "select des,Code from view_account where div_code='" & ViewState("divcode") & "' and  type = '" & ddlAccType.Value & "'  order by des", True)





            ddlgAccName.Value = accname(n)
            ddlgAccCode.Value = acccode(n)

            ddlConAccCode.Disabled = False
            ddlConAccName.Disabled = False
            ddlCCCode.Disabled = True
            ddlCCName.Disabled = True
            txtCurrRate.Disabled = False
            If acctype(n) = "G" Then
                sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
                sqlstr2 = " select  '' as acctname , '' as controlacctcode "
                ddlConAccCode.Disabled = True
                ddlConAccName.Disabled = True
                ddlCCCode.Disabled = False
                ddlCCName.Disabled = False
            ElseIf acctype(n) = "C" Then
                sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where view_account.div_code=acctmast.div_code and  acctmast.div_code='" & ViewState("divcode") & "' and  view_account.controlacctcode= acctmast.acctcode  and type= '" + acctype(n) + "' and view_account.code='" + accname(n) + "' order by  view_account.controlacctcode"
                sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where view_account.div_code=acctmast.div_code and  acctmast.div_code='" & ViewState("divcode") & "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + acctype(n) + "' and view_account.code='" + accname(n) + "' order by  acctmast.acctname"
            ElseIf acctype(n) = "S" Then
                sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where  acctmast.div_code='" & ViewState("divcode") & "' and  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' order by controlacctcode"

                sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where  acctmast.div_code='" & ViewState("divcode") & "' and  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' order by acctmast.acctname"
            ElseIf acctype(n) = "A" Then
                sqlstr1 = " select distinct supplier_agents.controlacctcode    , acctmast.acctname  from acctmast ,supplier_agents where acctmast.div_code='" & ViewState("divcode") & "' and   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' order by controlacctcode"
                sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where acctmast.div_code='" & ViewState("divcode") & "' and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' order by acctmast.acctname "

            End If
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True)

            ddlConAccCode.Value = controlcode(n)
            ddlConAccName.Value = controlname(n)

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)


            '15122014
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "othmaingrpname", "othmaingrpcode", "select othmaingrpcode,othmaingrpname from othmaingrpmast where active=1 order by othmaingrpcode ", True)


            ddlCCCode.Value = CCCode(n)
            ddlCCName.Value = CCName(n)
            txtCurrCode.Value = currcode(n)
            txtCurrRate.Value = crate(n)

            txtCredit.Value = credit(n)
            txtBaseCredit.Value = bcredit(n)
            txtDebit.Value = debit(n)
            txtBaseDebit.Value = bdebit(n)

            txtNarr.Value = narration(n)

            ddldept.Value = dept(n)

            If Trim(txtbasecurr.Value) = Trim(txtCurrCode.Value) Then
                txtCurrRate.Disabled = True
            End If

            n = n + 1
        Next
    End Sub


    Private Sub ShowFillGrid(ByVal RefCode As String)
        Dim mySqlReader As SqlDataReader
        Try
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            Dim ddlAccType As HtmlSelect
            Dim ddlgAccCode As HtmlSelect
            Dim ddlgAccName As HtmlSelect
            Dim ddlConAccCode As HtmlSelect
            Dim ddlConAccName As HtmlSelect
            Dim ddlCCCode As HtmlSelect
            Dim ddlCCName As HtmlSelect


            Dim ddldept As HtmlSelect

            Dim txtCredit, txtDebit, txtCurrCode, txtCurrRate, txtNarr, txtOldLineno, txtBaseCredit, lblno, txtBaseDebit, txtrequestid As HtmlInputText
            Dim credittot, debittot, basecredittot, basedebittot As Decimal
            Dim txtacctcode, txtacctname, txtctrolaccode, txtcontrolacname As HtmlInputText
            Dim sqlstr1, sqlstr2 As String

            Dim lngCnt As Long
            Dim dcridit As Decimal
            lngCnt = Val(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "Select count(*) from receipt_detail Where div_id='" & ViewState("divcode") & "' and  tran_id='" & RefCode & "'and tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "'"))
            If lngCnt = 0 Then lngCnt = 1
            fillDategrd(grdReceipt, False, lngCnt)
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            strSqlQry = "Select * from receipt_detail Where div_id='" & ViewState("divcode") & "' and  tran_id='" & RefCode & "'and tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' order by tran_lineno"
            myCommand = New SqlCommand(strSqlQry, SqlConn)
            mySqlReader = myCommand.ExecuteReader()
            If mySqlReader.HasRows Then
                While mySqlReader.Read()
                    For Each gvRow In grdReceipt.Rows
                        lblno = gvRow.FindControl("txtlineno")
                        If mySqlReader("tran_lineno") = CType(lblno.Value, Integer) Then
                            txtOldLineno = gvRow.FindControl("txtOldLineno")
                            txtCurrCode = gvRow.FindControl("txtCurrency")
                            txtCurrRate = gvRow.FindControl("txtConvRate")


                            txtDebit = gvRow.FindControl("txtDebit")
                            txtCredit = gvRow.FindControl("txtCredit")
                            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
                            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

                            txtDebit = gvRow.FindControl("txtDebit")

                            txtNarr = gvRow.FindControl("txtgnarration")
                            'txtBaseCredit = gvRow.FindControl("txtBaseCredit")

                            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
                            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

                            ddlConAccCode = gvRow.FindControl("ddlConAccCode")
                            ddlConAccName = gvRow.FindControl("ddlConAccName")
                            ddlCCCode = gvRow.FindControl("ddlCostCode")
                            ddlCCName = gvRow.FindControl("ddlCostName")

                            ddldept = gvRow.FindControl("ddldept")

                            ddlAccType = gvRow.FindControl("ddlType")
                            ddlgAccCode = gvRow.FindControl("ddlgAccCode")
                            ddlgAccName = gvRow.FindControl("ddlgAccName")

                            txtacctcode = gvRow.FindControl("txtacctcode")
                            txtacctname = gvRow.FindControl("txtacctname")
                            txtctrolaccode = gvRow.FindControl("txtctrolaccode")
                            txtcontrolacname = gvRow.FindControl("txtcontrolacname")
                            txtrequestid = gvRow.FindControl("txtrequestid")

                            txtOldLineno.Value = lblno.Value
                            ddlAccType.Value = mySqlReader("receipt_acc_type").ToString.Trim

                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccCode, "Code", "des", "select Code,des from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "'   order by code", True)
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccName, "des", "Code", "select Code,des from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "'  order by code", True)



                            ddlgAccName.Value = mySqlReader("receipt_acc_code").ToString
                            ddlgAccCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select des from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "' and code ='" & mySqlReader("receipt_acc_code").ToString & "' ")

                            txtacctname.Value = mySqlReader("receipt_acc_code").ToString
                            txtacctcode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select des from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "' and code ='" & mySqlReader("receipt_acc_code").ToString & "' ")


                            ddlConAccCode.Disabled = False
                            ddlConAccName.Disabled = False
                            ddlCCCode.Disabled = True
                            ddlCCName.Disabled = True
                            txtCurrRate.Disabled = False


                            If ddlAccType.Value = "G" Then
                                sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
                                sqlstr2 = " select  '' as acctname , '' as controlacctcode "
                                ddlConAccCode.Disabled = True
                                ddlConAccName.Disabled = True
                                ddlCCCode.Disabled = False
                                ddlCCName.Disabled = False
                            ElseIf ddlAccType.Value = "C" Then
                                sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where  view_account.div_code=acctmast.div_code and  acctmast.div_code='" & ViewState("divcode") & "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + ddlAccType.Value + "' and view_account.code='" + ddlgAccName.Value + "' order by  view_account.controlacctcode"
                                sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where  view_account.div_code=acctmast.div_code and  acctmast.div_code='" & ViewState("divcode") & "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + ddlAccType.Value + "' and view_account.code='" + ddlgAccName.Value + "' order by  acctmast.acctname"
                            ElseIf ddlAccType.Value = "S" Then
                                sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where acctmast.div_code='" & ViewState("divcode") & "' and  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + ddlgAccName.Value + "' order by controlacctcode "
                                sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where acctmast.div_code='" & ViewState("divcode") & "' and  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + ddlgAccName.Value + "' order by acctmast.acctname "
                            ElseIf ddlAccType.Value = "A" Then
                                sqlstr1 = " select distinct supplier_agents.controlacctcode    , acctmast.acctname  from acctmast ,supplier_agents where acctmast.div_code='" & ViewState("divcode") & "' and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + ddlgAccName.Value + "' order by controlacctcode "
                                sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where acctmast.div_code='" & ViewState("divcode") & "' and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + ddlgAccName.Value + "' order by acctmast.acctname "
                            End If
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True)
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True)

                            If ddlAccType.Value <> "G" Then
                                ddlConAccName.Value = mySqlReader("receipt_gl_code").ToString
                                ddlConAccCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where acctmast.div_code='" & ViewState("divcode") & "' and acctcode ='" & mySqlReader("receipt_gl_code").ToString & "' ")
                                txtcontrolacname.Value = mySqlReader("receipt_gl_code").ToString
                                txtctrolaccode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where  acctmast.div_code='" & ViewState("divcode") & "' and acctcode ='" & mySqlReader("receipt_gl_code").ToString & "' ")
                            Else
                                ddlConAccName.Value = "[select]"
                                ddlConAccCode.Value = "[select]"
                                txtctrolaccode.Value = ""
                                txtcontrolacname.Value = ""
                            End If
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)

                            '15122014
                            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)
                            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "othmaingrpname", "othmaingrpcode", "select othmaingrpcode,othmaingrpname from othmaingrpmast where active=1 order by othmaingrpcode ", True)

                            ddldept.Value = mySqlReader("dept").ToString

                            ddlCCCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select costcenter_name from costcenter_master where costcenter_code ='" & mySqlReader("costcenter_code").ToString & "' ")
                            ddlCCName.Value = mySqlReader("costcenter_code").ToString

                            txtCurrCode.Value = mySqlReader("receipt_currency_id").ToString
                            txtCurrRate.Value = Val(mySqlReader("receipt_currency_rate"))

                            txtNarr.Value = mySqlReader("receipt_narration").ToString

                            If Trim(txtTranType.Value) = "RV" Then
                                txtCredit.Value = DecRound(Val(mySqlReader("receipt_credit")))
                                txtBaseCredit.Value = DecRound(Val(mySqlReader("basecredit")))
                                txtDebit.Value = DecRound(Val(mySqlReader("receipt_debit")))
                                txtBaseDebit.Value = DecRound(Val(mySqlReader("basedebit")))
                            Else 'If Trim(txtTranType.Value) = "PV" Then
                                txtCredit.Value = DecRound(Val(mySqlReader("receipt_credit")))
                                txtBaseCredit.Value = DecRound(Val(mySqlReader("basecredit")))
                                txtDebit.Value = DecRound(Val(mySqlReader("receipt_debit")))
                                txtBaseDebit.Value = DecRound(Val(mySqlReader("basedebit")))
                            End If
                            If IsDBNull(mySqlReader("requestid")) = False Then
                                txtrequestid.Value = mySqlReader("requestid").ToString
                            Else
                                txtrequestid.Value = ""
                            End If

                            'dcridit = DecRound(DecRound(dcridit) + DecRound(CType(txtCredit.Value, Decimal)))
                            credittot = CType(Val(credittot), Decimal) + CType(Val(txtCredit.Value), Decimal)
                            debittot = CType(Val(debittot), Decimal) + CType(Val(txtDebit.Value), Decimal)
                            basecredittot = CType(Val(basecredittot), Decimal) + CType(Val(txtBaseCredit.Value), Decimal)
                            basedebittot = CType(Val(basedebittot), Decimal) + CType(Val(txtBaseDebit.Value), Decimal)

                            If ViewState("ReceiptsState") = "Edit" Or ViewState("ReceiptsState") = "Copy" Then
                                If Trim(txtbasecurr.Value) = Trim(txtCurrCode.Value) Then
                                    txtCurrRate.Disabled = True
                                End If
                            End If
                            Exit For
                        End If
                    Next
                End While
            End If
            'txtTotalCredit.Value = DecRound(Val(dcridit))
            txtTotalCredit.Value = DecRound(credittot)
            txtTotalDebit.Value = DecRound(debittot)
            txtTotBaseCredit.Value = DecRound(basecredittot)
            txtTotBaseDebit.Value = DecRound(basedebittot)
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Receipts.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try
    End Sub

    Public Function DecRound(ByVal Ramt As Decimal) As Decimal
        Dim Rdamt As Decimal
        Rdamt = Math.Round(Val(Ramt), CType(txtdecimal.Value, Integer))
        Return Rdamt
    End Function

    Private Sub initialclass(ByVal con As SqlConnection, ByVal stran As SqlTransaction)
        caccounts = Nothing
        cacc = Nothing
        ctran = Nothing
        csubtran = Nothing
        caccounts = New clssave
        cacc = New clsAccounts
        cacc.clropencol()
        cacc.tran_mode = IIf(ViewState("ReceiptsState") = "New", 1, 2)
        mbasecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
        cacc.start()

    End Sub

    Protected Sub btnDelLine_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim clAdBill As New Collection
        Dim clAdBillnew As New Collection

        Dim intLineNo As Integer = 1
        Dim strLineKey As String

        Dim MainGrdCount As Integer
        Dim credittot, debittot, basecredittot, basedebittot As Decimal
        Dim n As Integer = 0
        Dim count As Integer
        count = grdReceipt.Rows.Count + 1
        Dim lineno(count) As String
        Dim Olineno(count) As String
        Dim acctype(count) As String
        Dim acccode(count) As String
        Dim accname(count) As String
        Dim controlcode(count) As String
        Dim controlname(count) As String
        Dim CCCode(count) As String
        Dim CCName(count) As String

        Dim narration(count) As String
        Dim crate(count) As String
        Dim currcode(count) As String
        Dim credit(count) As String
        Dim bcredit(count) As String
        Dim debit(count) As String
        Dim bdebit(count) As String
        Dim requestid(count) As String

        Dim dept(count) As String


        Dim ddlAccType As HtmlSelect
        Dim ddlgAccCode As HtmlSelect
        Dim ddlgAccName As HtmlSelect
        Dim ddlConAccCode As HtmlSelect
        Dim ddlConAccName As HtmlSelect
        Dim txtCredit As HtmlInputControl
        Dim txtCurrCode As HtmlInputControl
        Dim txtCurrRate As HtmlInputControl
        Dim ddlCCCode As HtmlSelect
        Dim ddlCCName As HtmlSelect

        Dim ddldept As HtmlSelect


        Dim txtNarr As HtmlInputControl
        Dim txtBaseCredit As HtmlInputText
        Dim lblno As HtmlInputText
        Dim chckDeletion As CheckBox
        Dim txtrequestid As HtmlInputControl

        Dim txtacctcode, txtacctname, txtctrolaccode, txtcontrolacname, txtOldLineno, txtDebit, txtBaseDebit As HtmlInputText
        Dim sqlstr1, sqlstr2 As String
        Dim cntcont, j As Long
        'If Session("Collection").ToString <> "" Then
        '    clAdBill = CType(Session("Collection"), Collection)
        'End If
        clAdBill = GetCollectionFromSession()

        For Each gvRow In grdReceipt.Rows
            chckDeletion = gvRow.FindControl("chckDeletion")
            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")

            ddlAccType = gvRow.FindControl("ddlType")
            'ddlgAccCode = gvRow.FindControl("ddlgAccCode")
            'ddlgAccName = gvRow.FindControl("ddlgAccName")

            'ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            'ddlConAccName = gvRow.FindControl("ddlConAccName")
            ddlCCCode = gvRow.FindControl("ddlCostCode")
            ddlCCName = gvRow.FindControl("ddlCostName")

            txtCurrCode = gvRow.FindControl("txtCurrency")
            txtCurrRate = gvRow.FindControl("txtConvRate")
            txtNarr = gvRow.FindControl("txtgnarration")

            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtacctcode = gvRow.FindControl("txtacctcode")
            txtacctname = gvRow.FindControl("txtacctname")
            txtctrolaccode = gvRow.FindControl("txtctrolaccode")
            txtcontrolacname = gvRow.FindControl("txtcontrolacname")
            txtrequestid = gvRow.FindControl("txtrequestid")

            ddldept = gvRow.FindControl("ddldept")

            If chckDeletion.Checked = True Then

                cntcont = clAdBill.Count / 21
                For j = 1 To cntcont
                    strLineKey = j & ":" & lblno.Value
                    DeleteCollection(clAdBill, "AgainstTranLineNo" & strLineKey)
                    DeleteCollection(clAdBill, "AccTranLineNo" & strLineKey)
                    DeleteCollection(clAdBill, "TranId" & strLineKey)
                    DeleteCollection(clAdBill, "TranDate" & strLineKey)
                    DeleteCollection(clAdBill, "TranType" & strLineKey)
                    DeleteCollection(clAdBill, "DueDate" & strLineKey)
                    DeleteCollection(clAdBill, "CurrRate" & strLineKey)
                    DeleteCollection(clAdBill, "Credit" & strLineKey)
                    DeleteCollection(clAdBill, "Debit" & strLineKey)
                    DeleteCollection(clAdBill, "BaseCredit" & strLineKey)
                    DeleteCollection(clAdBill, "BaseDebit" & strLineKey)
                    DeleteCollection(clAdBill, "RefNo" & strLineKey)
                    DeleteCollection(clAdBill, "Field2" & strLineKey)
                    DeleteCollection(clAdBill, "Field3" & strLineKey)
                    DeleteCollection(clAdBill, "Field4" & strLineKey)
                    DeleteCollection(clAdBill, "Field5" & strLineKey)
                    DeleteCollection(clAdBill, "OpenMode" & strLineKey)
                    DeleteCollection(clAdBill, "AccType" & strLineKey)
                    DeleteCollection(clAdBill, "AccCode" & strLineKey)
                    DeleteCollection(clAdBill, "AccGLCode" & strLineKey)
                    DeleteCollection(clAdBill, "AdjustBaseTotal" & strLineKey)
                Next
            Else
                If txtacctcode.Value <> "[Select]" And ddlAccType.Value <> "[Select]" Then
                    lineno(n) = lblno.Value
                    Olineno(n) = txtOldLineno.Value
                    acctype(n) = ddlAccType.Value
                    acccode(n) = txtacctcode.Value
                    accname(n) = txtacctname.Value 'ddlgAccName.Value
                    controlcode(n) = txtctrolaccode.Value
                    controlname(n) = txtcontrolacname.Value
                    CCCode(n) = CType(ddlCCCode.Value, String)
                    CCName(n) = CType(ddlCCName.Value, String)

                    currcode(n) = txtCurrCode.Value
                    crate(n) = txtCurrRate.Value
                    narration(n) = txtNarr.Value
                    credit(n) = txtCredit.Value
                    bcredit(n) = txtBaseCredit.Value

                    debit(n) = txtDebit.Value
                    bdebit(n) = txtBaseDebit.Value
                    requestid(n) = txtrequestid.Value
                    n = n + 1
                End If
            End If
        Next
        ' Session.Add("Collection", clAdBill)
        Dim collectionDate As Collection
        Dim strLineKeynew As String
        Dim sno As Integer
        'If Session("Collection").ToString <> "" Then
        '    collectionDate = CType(Session("Collection"), Collection)
        'End If
        collectionDate = clAdBill

        Dim grdct As Long
        grdct = n
        If grdct = 0 Then
            grdct = 1
        End If
        fillDategrd(grdReceipt, False, grdct)
        Dim i As Integer = n
        n = 0

        For Each gvRow In grdReceipt.Rows
            If n = i Then
                Exit For
            End If

            lblno = gvRow.FindControl("txtlineno")
            txtOldLineno = gvRow.FindControl("txtOldLineno")

            ddlAccType = gvRow.FindControl("ddlType")
            ddlgAccCode = gvRow.FindControl("ddlgAccCode")
            ddlgAccName = gvRow.FindControl("ddlgAccName")
            ddlConAccCode = gvRow.FindControl("ddlConAccCode")
            ddlConAccName = gvRow.FindControl("ddlConAccName")
            ddlCCCode = gvRow.FindControl("ddlCostCode")
            ddlCCName = gvRow.FindControl("ddlCostName")

            txtCurrCode = gvRow.FindControl("txtCurrency")
            txtCurrRate = gvRow.FindControl("txtConvRate")
            txtNarr = gvRow.FindControl("txtgnarration")

            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

            txtacctcode = gvRow.FindControl("txtacctcode")
            txtacctname = gvRow.FindControl("txtacctname")
            txtctrolaccode = gvRow.FindControl("txtctrolaccode")
            txtcontrolacname = gvRow.FindControl("txtcontrolacname")
            txtrequestid = gvRow.FindControl("txtrequestid")

            ddldept = gvRow.FindControl("ddldept")

            txtOldLineno.Value = Olineno(n)
            txtacctcode.Value = acccode(n)
            txtacctname.Value = accname(n)
            txtctrolaccode.Value = controlcode(n)
            txtcontrolacname.Value = controlname(n)

            ddlAccType.Value = acctype(n)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccCode, "Code", "des", "select Code,des from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "'   order by code", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgAccName, "des", "Code", "select Code,des from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlAccType.Value & "'  order by des", True)


            ddlgAccName.Value = accname(n)
            ddlgAccCode.Value = acccode(n)

            ddlConAccCode.Disabled = False
            ddlConAccName.Disabled = False
            ddlCCCode.Disabled = True
            ddlCCName.Disabled = True
            txtCurrRate.Disabled = False
            If acctype(n) = "G" Then
                sqlstr1 = " select ''  as controlacctcode, '' as acctname  "
                sqlstr2 = " select  '' as acctname , '' as controlacctcode "
                ddlConAccCode.Disabled = True
                ddlConAccName.Disabled = True
                ddlCCCode.Disabled = False
                ddlCCName.Disabled = False
            ElseIf acctype(n) = "C" Then
                sqlstr1 = " select distinct view_account.controlacctcode, acctmast.acctname  from acctmast ,view_account where acctmast.div_code=view_account.div_code and acctmast.div_code='" & ViewState("divcode") & "' and  view_account.controlacctcode= acctmast.acctcode  and type= '" + acctype(n) + "' and view_account.code='" + accname(n) + "' order by  view_account.controlacctcode"
                sqlstr2 = " select distinct  acctmast.acctname,view_account.controlacctcode  from acctmast ,view_account where acctmast.div_code=view_account.div_code and acctmast.div_code='" & ViewState("divcode") & "' and   view_account.controlacctcode= acctmast.acctcode  and type= '" + acctype(n) + "' and view_account.code='" + accname(n) + "' order by  acctmast.acctname"
            ElseIf acctype(n) = "S" Then
                sqlstr1 = " select distinct partymast.controlacctcode  , acctmast.acctname  from acctmast ,partymast where acctmast.div_code='" & ViewState("divcode") & "' and   partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' order by controlacctcode "

                sqlstr2 = " select distinct acctmast.acctname ,partymast.controlacctcode      from acctmast ,partymast where  acctmast.div_code='" & ViewState("divcode") & "' and  partymast.controlacctcode= acctmast.acctcode   and partymast.partycode='" + accname(n) + "' order by acctmast.acctname "
            ElseIf acctype(n) = "A" Then
                sqlstr1 = " select distinct supplier_agents.controlacctcode    , acctmast.acctname  from acctmast ,supplier_agents where  acctmast.div_code='" & ViewState("divcode") & "' and  supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' order by controlacctcode "

                sqlstr2 = " select distinct acctmast.acctname ,supplier_agents.controlacctcode     from acctmast ,supplier_agents where acctmast.div_code='" & ViewState("divcode") & "' and   supplier_agents.controlacctcode= acctmast.acctcode   and supplier_agents.supagentcode='" + accname(n) + "' order by acctmast.acctname "

            End If
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccCode, "controlacctcode", "acctname", sqlstr1, True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlConAccName, "acctname", "controlacctcode", sqlstr2, True)


            ddlConAccCode.Value = controlcode(n)
            ddlConAccName.Value = controlname(n)

            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCCode, "costcenter_code", "costcenter_name", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_code ", True)
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCName, "costcenter_name", "costcenter_code", "select costcenter_code, costcenter_name from costcenter_master where active=1 order by costcenter_name ", True)

            '15122014
            objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active=1 order by plgrpname", True)
            'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddldept, "othmaingrpname", "othmaingrpcode", "select othmaingrpcode,othmaingrpname from othmaingrpmast where active=1 order by othmaingrpcode ", True)

            ddldept.Value = dept(n)

            ddlCCCode.Value = CCCode(n)
            ddlCCName.Value = CCName(n)

            txtCurrCode.Value = currcode(n)
            txtCurrRate.Value = crate(n)
            txtNarr.Value = narration(n)
            txtCredit.Value = credit(n)
            txtBaseCredit.Value = bcredit(n)

            txtDebit.Value = debit(n)
            txtBaseDebit.Value = bdebit(n)
            txtrequestid.Value = requestid(n)

            If txtCredit.Value = "" Then txtCredit.Value = 0
            If txtBaseCredit.Value = "" Then txtBaseCredit.Value = 0
            If txtDebit.Value = "" Then txtDebit.Value = 0
            If txtBaseDebit.Value = "" Then txtBaseDebit.Value = 0

            credittot = DecRound(DecRound(credittot) + DecRound(CType(txtCredit.Value, Decimal)))
            basecredittot = DecRound(DecRound(basecredittot) + DecRound(CType(txtBaseCredit.Value, Decimal)))
            debittot = DecRound(DecRound(debittot) + DecRound(CType(txtDebit.Value, Decimal)))
            basedebittot = DecRound(DecRound(basedebittot) + DecRound(CType(txtBaseDebit.Value, Decimal)))

            If Trim(txtbasecurr.Value) = Trim(txtCurrCode.Value) Then
                txtCurrRate.Disabled = True
            End If
            '   lineno(count)
            sno = 1
            If acctype(n) <> "G" Then
                MainGrdCount = grdReceipt.Rows.Count  ' lineno.Length
                Dim MainRowidx As Integer
                MainRowidx = 1
                For MainRowidx = 0 To MainGrdCount - 1
                    cntcont = collectionDate.Count / 21
                    sno = 1
                    For j = 1 To cntcont
                        strLineKey = j & ":" & lineno(MainRowidx)
                        strLineKeynew = sno & ":" & lblno.Value
                        If colexists(clAdBillnew, "AgainstTranLineNo" & strLineKeynew) = False Then
                            If colexists(collectionDate, "AgainstTranLineNo" & strLineKey) = True Then
                                If collectionDate("AccCode" & strLineKey).ToString = accname(n) And collectionDate("AccType" & strLineKey).ToString = acctype(n) And collectionDate("AccGLCode" & strLineKey).ToString = controlname(n) And (DecRound(CType(collectionDate("AdjustBaseTotal" & strLineKey), Decimal)) = bcredit(n)) And CType(collectionDate("CurrRate" & strLineKey), Decimal) = Val(CType(crate(n), Decimal)) And collectionDate("AgainstTranLineNo" & strLineKey) = lineno(MainRowidx) Then
                                    AddCollection(clAdBillnew, "AgainstTranLineNo" & strLineKeynew, lblno.Value) 'collectionDate("AgainstTranLineNo" & strLineKey))
                                    AddCollection(clAdBillnew, "AccTranLineNo" & strLineKeynew, collectionDate("AccTranLineNo" & strLineKey))
                                    AddCollection(clAdBillnew, "TranId" & strLineKeynew, collectionDate("TranId" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "TranDate" & strLineKeynew, Format(CType(collectionDate("TranDate" & strLineKey).ToString, Date), "dd/MM/yyyy"))
                                    AddCollection(clAdBillnew, "TranType" & strLineKeynew, collectionDate("TranType" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "DueDate" & strLineKeynew, Format(CType(collectionDate("DueDate" & strLineKey).ToString, Date), "dd/MM/yyyy"))
                                    AddCollection(clAdBillnew, "CurrRate" & strLineKeynew, CType(collectionDate("CurrRate" & strLineKey), Decimal))
                                    AddCollection(clAdBillnew, "Credit" & strLineKeynew, DecRound(CType(collectionDate("Credit" & strLineKey), Decimal)))
                                    AddCollection(clAdBillnew, "Debit" & strLineKeynew, DecRound(CType(collectionDate("Debit" & strLineKey), Decimal)))
                                    AddCollection(clAdBillnew, "BaseCredit" & strLineKeynew, DecRound(CType(collectionDate("BaseCredit" & strLineKey), Decimal)))
                                    AddCollection(clAdBillnew, "BaseDebit" & strLineKeynew, DecRound(CType(collectionDate("BaseDebit" & strLineKey), Decimal)))
                                    AddCollection(clAdBillnew, "RefNo" & strLineKeynew, collectionDate("RefNo" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "Field2" & strLineKeynew, collectionDate("Field2" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "Field3" & strLineKeynew, collectionDate("Field3" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "Field4" & strLineKeynew, collectionDate("Field4" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "Field5" & strLineKeynew, collectionDate("Field5" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "OpenMode" & strLineKeynew, collectionDate("OpenMode" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "AccType" & strLineKeynew, collectionDate("AccType" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "AccCode" & strLineKeynew, collectionDate("AccCode" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "AccGLCode" & strLineKeynew, collectionDate("AccGLCode" & strLineKey).ToString)
                                    AddCollection(clAdBillnew, "AdjustBaseTotal" & strLineKeynew, DecRound(CType(collectionDate("AdjustBaseTotal" & strLineKey), Decimal)))


                                    DeleteCollection(collectionDate, "AgainstTranLineNo" & strLineKey)
                                    DeleteCollection(collectionDate, "AccTranLineNo" & strLineKey)
                                    DeleteCollection(collectionDate, "TranId" & strLineKey)
                                    DeleteCollection(collectionDate, "TranDate" & strLineKey)
                                    DeleteCollection(collectionDate, "TranType" & strLineKey)
                                    DeleteCollection(collectionDate, "DueDate" & strLineKey)
                                    DeleteCollection(collectionDate, "CurrRate" & strLineKey)
                                    DeleteCollection(collectionDate, "Credit" & strLineKey)
                                    DeleteCollection(collectionDate, "Debit" & strLineKey)
                                    DeleteCollection(collectionDate, "BaseCredit" & strLineKey)
                                    DeleteCollection(collectionDate, "BaseDebit" & strLineKey)
                                    DeleteCollection(collectionDate, "RefNo" & strLineKey)
                                    DeleteCollection(collectionDate, "Field2" & strLineKey)
                                    DeleteCollection(collectionDate, "Field3" & strLineKey)
                                    DeleteCollection(collectionDate, "Field4" & strLineKey)
                                    DeleteCollection(collectionDate, "Field5" & strLineKey)
                                    DeleteCollection(collectionDate, "OpenMode" & strLineKey)
                                    DeleteCollection(collectionDate, "AccType" & strLineKey)
                                    DeleteCollection(collectionDate, "AccCode" & strLineKey)
                                    DeleteCollection(collectionDate, "AccGLCode" & strLineKey)
                                    sno = sno + 1
                                End If
                            End If
                            'Else
                            '    Exit For
                        End If
                    Next
                Next
            End If


            n = n + 1
        Next
        '  Session.Add("Collection", clAdBillnew)
        Session.Add("Collection" & ":" & txtAdjcolno.Value, clAdBillnew)

        txtTotalCredit.Value = DecRound(Val(credittot))
        txtTotalDebit.Value = DecRound(Val(debittot))
        txtTotBaseCredit.Value = DecRound(Val(basecredittot))
        txtTotBaseDebit.Value = DecRound(Val(basedebittot))

        'txtTotalCredit.Value = DecRound(Val(cridittot))
        'txtTotalBCredit.Value = DecRound(Val(basecridittot))
    End Sub
    Public Sub DeleteCollection(ByVal dataCollection As Collection, ByVal strKey As String)
        If colexists(dataCollection, strKey) = True Then
            dataCollection.Remove(strKey)
        End If
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            'If MsgBox("Do you want to print", MsgBoxStyle.YesNo, "Doc Print") = MsgBoxResult.No Then
            '    Exit Sub
            'End If
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Do you want to print' );", True)
            'Dim strReportTitle As String = ""
            'Dim strSelectionFormula As String = ""
            'Session.Add("RefCode", CType(txtDocNo.Value.Trim, String))
            'Session.Add("Pageame", "ReceiptDoc")
            'Session.Add("BackPageName", "~\AccountsModule\Receipts.aspx?tran_type=" & CType(ViewState("ReceiptsRVPVTranType"), String) & "")

            ' strSelectionFormula = ""
            'If txtDocNo.Value.Trim <> "" Then
            '    If Trim(strSelectionFormula) = "" Then
            '        'strReportTitle = "Doc No : " & txtDocNo.Value.Trim
            '        strSelectionFormula = " {receipt_master_new.tran_id}='" & txtDocNo.Value.Trim & "'"
            '    Else
            '        'strReportTitle = strReportTitle & "Doc No : " & txtDocNo.Value.Trim & "'"
            '        strSelectionFormula = strSelectionFormula & " AND {receipt_master_new.tran_id}='" & txtDocNo.Value.Trim & "'"
            '    End If
            'Else
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Doc No' );", True)
            '    Exit Sub
            'End If

            'If Trim(strSelectionFormula) = "" Then
            '    strSelectionFormula = " {receipt_master_new.tran_type} = '" & ViewState("ReceiptsRVPVTranType") & "' " & _
            '    " and  {trdpurchase_master.div_id} = '" & CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
            'Else
            '    strSelectionFormula = strSelectionFormula & " AND {receipt_master_new.tran_type} = '" & ViewState("ReceiptsRVPVTranType") & "'" & _
            '    " and  {receipt_master_new.div_id} = '" & CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
            'End If
            'Dim lblstr, cashbank_typestr As String
            'lblstr = ""
            'cashbank_typestr = ""
            'If ViewState("ReceiptsRVPVTranType") = "RV" Then
            '    lblstr = "Receipt Voucher"
            'ElseIf ViewState("ReceiptsRVPVTranType") = "PV" Then
            '    ' If Trim(strSelectionFormula) = "" Then
            '    If ddlCashBank.Value = "Bank" Then
            '        'strSelectionFormula = " {receipt_master_new.receipt_cashbank_type}='B'"
            '        lblstr = "Cheque Payment Voucher"
            '        cashbank_typestr = "B"
            '    ElseIf ddlCashBank.Value = "Cash" Then
            '        'strSelectionFormula = " {receipt_master_new.receipt_cashbank_type}='C'"
            '        lblstr = "Cash Payment Voucher"
            '        cashbank_typestr = "C"
            '    End If
            'Else
            '    If ddlCashBank.Value = "Bank" Then
            '        strSelectionFormula = strSelectionFormula & " AND {receipt_master_new.receipt_cashbank_type}='B'"
            '        lblstr = "Cheque Payment Voucher"
            '    ElseIf ddlCashBank.Value = "Cash" Then
            '        lblstr = "Cash Payment Voucher"
            '        strSelectionFormula = strSelectionFormula & " AND {receipt_master_new.receipt_cashbank_type}='C'"
            '    End If
            'End If

            'End If
            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Session.Add("PrinDocTitle", lblstr)
            Disabled_Control()
            Dim ScriptStr As String
            Dim prntsec As Decimal = 0
            'ScriptStr = "<script language=""javascript"">var win=window.open('../PriceListModule/PrintDoc.aspx','printdoc','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"

            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)

            '    Dim ScriptStr As String
            Dim lblstr, cashbank_typestr As String
            lblstr = ""
            cashbank_typestr = ""
            If ViewState("ReceiptsRVPVTranType") = "RV" Then
                lblstr = "Receipt Voucher"
                cashbank_typestr = ""
            ElseIf ViewState("ReceiptsRVPVTranType") = "DEP" Then
                lblstr = "Deposit Voucher"
                If ddlCashBank.Value = "Bank" Then
                    cashbank_typestr = "B"
                ElseIf ddlCashBank.Value = "Cash" Then
                    cashbank_typestr = "C"
                End If
            Else 'If ViewState("ReceiptsRVPVTranType") = "PV" Then
                If ddlCashBank.Value = "Bank" Then
                    lblstr = "Bank Payment Voucher"
                    cashbank_typestr = "B"
                ElseIf ddlCashBank.Value = "Cash" Then
                    lblstr = "Cash Payment Voucher"
                    cashbank_typestr = "C"
                End If
            End If
            If chkPrntInclude.Checked = True Then
                prntsec = 1
            End If
            Dim backPname As String

            backPname = "~\AccountsModule\Receipts.aspx?&divid=" & ViewState("divcode") & "&tran_type=" & CType(ViewState("CNDNOpen_type"), String)


            ScriptStr = "<script language=""javascript"">var win=window.open('PrintDocNew.aspx?Pageame=ReceiptDoc&BackPageName=~\AccountsModule\Receipts.aspx&Tranid=" & txtDocNo.Value & "&TranType=" & ViewState("ReceiptsRVPVTranType") & "&divid=" & ViewState("divcode") & "&PrinDocTitle=" & lblstr & "&CashBankType=" & cashbank_typestr & "&PrntSec=" & prntsec & "','printdoc','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Receipts.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ViewState("ReceiptsRVPVTranType") = "DEP" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Deposits','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        ElseIf ViewState("ReceiptsRVPVTranType") = "RV" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Receipts','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        ElseIf ViewState("ReceiptsRVPVTranType") = "CPV" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CashPayment','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        ElseIf ViewState("ReceiptsRVPVTranType") = "BPV" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=BankPayments','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        End If

    End Sub
    Private Function GetCollectionFromSession() As Collection
        Dim adjcolnostr As String
        adjcolnostr = txtAdjcolno.Value
        Dim collectionDate1 As New Collection
        If Not Session("Collection" & ":" & adjcolnostr) Is Nothing Then
            If Session("Collection" & ":" & adjcolnostr).ToString <> "" Then
                collectionDate1 = CType(Session("Collection" & ":" & adjcolnostr), Collection)
            End If
        End If
        Return collectionDate1
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
            If ViewState("ReceiptsState") = "Edit" Then
                If chkPost.Checked = True Then
                    ViewState.Add("ReceiptsState", "View")
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This transaction has been posted, you do not have rights to edit.' );", True)
                End If
            End If
        End If
    End Sub

    Protected Sub grdReceipt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdReceipt.SelectedIndexChanged

    End Sub

    Protected Sub btnclientreceipt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclientreceipt.Click
        Try
            ''If MsgBox("Do you want to print", MsgBoxStyle.YesNo, "Doc Print") = MsgBoxResult.No Then
            ''    Exit Sub
            ''End If
            ''ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Do you want to print' );", True)
            ''Dim strReportTitle As String = ""
            ''Dim strSelectionFormula As String = ""
            ''Session.Add("RefCode", CType(txtDocNo.Value.Trim, String))
            ''Session.Add("Pageame", "ReceiptDoc")
            ''Session.Add("BackPageName", "~\AccountsModule\Receipts.aspx?tran_type=" & CType(ViewState("ReceiptsRVPVTranType"), String) & "")

            '' strSelectionFormula = ""
            ''If txtDocNo.Value.Trim <> "" Then
            ''    If Trim(strSelectionFormula) = "" Then
            ''        'strReportTitle = "Doc No : " & txtDocNo.Value.Trim
            ''        strSelectionFormula = " {receipt_master_new.tran_id}='" & txtDocNo.Value.Trim & "'"
            ''    Else
            ''        'strReportTitle = strReportTitle & "Doc No : " & txtDocNo.Value.Trim & "'"
            ''        strSelectionFormula = strSelectionFormula & " AND {receipt_master_new.tran_id}='" & txtDocNo.Value.Trim & "'"
            ''    End If
            ''Else
            ''    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Doc No' );", True)
            ''    Exit Sub
            ''End If

            ''If Trim(strSelectionFormula) = "" Then
            ''    strSelectionFormula = " {receipt_master_new.tran_type} = '" & ViewState("ReceiptsRVPVTranType") & "' " & _
            ''    " and  {trdpurchase_master.div_id} = '" & CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
            ''Else
            ''    strSelectionFormula = strSelectionFormula & " AND {receipt_master_new.tran_type} = '" & ViewState("ReceiptsRVPVTranType") & "'" & _
            ''    " and  {receipt_master_new.div_id} = '" & CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
            ''End If
            ''Dim lblstr, cashbank_typestr As String
            ''lblstr = ""
            ''cashbank_typestr = ""
            ''If ViewState("ReceiptsRVPVTranType") = "RV" Then
            ''    lblstr = "Receipt Voucher"
            ''ElseIf ViewState("ReceiptsRVPVTranType") = "PV" Then
            ''    ' If Trim(strSelectionFormula) = "" Then
            ''    If ddlCashBank.Value = "Bank" Then
            ''        'strSelectionFormula = " {receipt_master_new.receipt_cashbank_type}='B'"
            ''        lblstr = "Cheque Payment Voucher"
            ''        cashbank_typestr = "B"
            ''    ElseIf ddlCashBank.Value = "Cash" Then
            ''        'strSelectionFormula = " {receipt_master_new.receipt_cashbank_type}='C'"
            ''        lblstr = "Cash Payment Voucher"
            ''        cashbank_typestr = "C"
            ''    End If
            ''Else
            ''    If ddlCashBank.Value = "Bank" Then
            ''        strSelectionFormula = strSelectionFormula & " AND {receipt_master_new.receipt_cashbank_type}='B'"
            ''        lblstr = "Cheque Payment Voucher"
            ''    ElseIf ddlCashBank.Value = "Cash" Then
            ''        lblstr = "Cash Payment Voucher"
            ''        strSelectionFormula = strSelectionFormula & " AND {receipt_master_new.receipt_cashbank_type}='C'"
            ''    End If
            ''End If

            ''End If
            ''Session.Add("SelectionFormula", strSelectionFormula)
            ''Session.Add("ReportTitle", strReportTitle)
            ''Session.Add("PrinDocTitle", lblstr)
            Disabled_Control()
            'Dim ScriptStr As String
            Dim prntsec As Decimal = 0
            ''ScriptStr = "<script language=""javascript"">var win=window.open('../PriceListModule/PrintDoc.aspx','printdoc','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"

            ''ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)

            Dim ScriptStr As String
            Dim lblstr, cashbank_typestr As String
            lblstr = ""
            cashbank_typestr = ""
            If ViewState("ReceiptsRVPVTranType") = "RV" Then
                lblstr = "Receipt Voucher"
                cashbank_typestr = ""
                'ElseIf ViewState("ReceiptsRVPVTranType") = "DEP" Then
                '    lblstr = "Deposit Voucher"
                '    If ddlCashBank.Value = "Bank" Then
                '        cashbank_typestr = "B"
                '    ElseIf ddlCashBank.Value = "Cash" Then
                '        cashbank_typestr = "C"
                '    End If
                'Else 'If ViewState("ReceiptsRVPVTranType") = "PV" Then
                '    If ddlCashBank.Value = "Bank" Then
                '        lblstr = "Bank Payment Voucher"
                '        cashbank_typestr = "B"
                '    ElseIf ddlCashBank.Value = "Cash" Then
                '        lblstr = "Cash Payment Voucher"
                '        cashbank_typestr = "C"
                '    End If
            End If
            If chkPrntInclude.Checked = True Then
                prntsec = 1
            End If

            ScriptStr = "<script language=""javascript"">var win=window.open('PrintDocNew.aspx?Pageame=ReceiptClientDoc&BackPageName=~\AccountsModule\Receipts.aspx&Tranid=" & txtDocNo.Value & "&TranType=" & ViewState("ReceiptsRVPVTranType") & "&PrinDocTitle=" & lblstr & "&CashBankType=" & cashbank_typestr & "&PrntSec=" & prntsec & "','printdoc','toolbar=0,scrollbars=yes,resizable=yes,titlebar=0,menubar=0,locationbar=0,modal=1,statusbar=1');</script>"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Receipts.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub hdnValidate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ViewState("IsCheckedValidateCheque") = Nothing
    End Sub


    Private Function GetScalarValue(ByVal dbconnection As String, ByVal strQry As String) As Object
        Try
            SqlConn = clsDBConnect.dbConnectionnew(dbconnection)
            myCommand = New SqlCommand(strQry, SqlConn)
            Return myCommand.ExecuteScalar()
        Catch ex As Exception
            Return ""
        End Try
    End Function

End Class


'    sqlTrans.Commit()    'SQl Tarn Commit
'    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
'    clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
'    clsDBConnect.dbConnectionClose(SqlConn)           'connection close
'    Response.Redirect("ReceiptsSearch.aspx?tran_type=" & CType( ViewState("ReceiptsRVPVTranType"), String) & "", False)
'End If

'Dim strbanktype As String

'strbanktype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"acctmast", "cust_supp", "acctcode", ddlAccName.Value)
'Dim glCode As String
'glCode = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"),"select isnull(controlacctcode,0) from dbo.view_account where type='" & ddlAccType.Items(ddlAccType.SelectedIndex).Text & "'and code='" & ddlgAccName.Value & "'")

'sqlTrans.Commit()    'SQl Tarn Commit
'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
'clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
'clsDBConnect.dbConnectionClose(SqlConn)           'connection close
'Response.Redirect("ReceiptsSearch.aspx?tran_type=" & CType( ViewState("ReceiptsRVPVTranType"), String) & "", False)

''Delete Record
'strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511)
'SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
'sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start
'objDateTime.ConvertDateromTextBoxToDatabase(txtChequeDate.Text)
'objDateTime.ConvertDateromTextBoxToDatabase(txtDate.Text)
'objDateTime.ConvertDateromTextBoxToDatabase(txtChequeDate.Text)
' objDateTime.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("receipt_date"))
'lngCnt = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"openparty_detail", "count(tran_id)", "tran_id", RefCode)
'txtDebit = gvRow.FindControl("txtDebit")
'txtBaseDebit = gvRow.FindControl("txtBaseDebit")

' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCurrCode, "Currcode", "acctcode", "select acctcode,Currcode from acctmast  where bankyn='Y' ", True)

'gvRow.Cells(grd_col.BaseDebit).Text = gvRow.Cells(grd_col.BaseDebit).Text & "(" & strOpti & ")"
'gvRow.Cells(grd_col.BaseCredit).Text = "KWD " & gvRow.Cells(grd_col.BaseCredit).Text

'select Code,des from view_account where type = '"+ strtp +"' order by code

' grdAcc.Columns(grd_col.LienNo).Visible = False

'ddlgAccName.Items(ddlgAccName.SelectedIndex).Text


'txtConvRate.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
'txtAmount.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
'txtBalance.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")

' txtCurrRate.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
'txtCredit.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")