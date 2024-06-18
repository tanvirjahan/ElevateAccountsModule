'----------------------------------------------------------------------------------------------------
'   Module Name    :    OpeningTrailBalance
'   Developer Name :    Mangesh 
'   Date           :    
'   
'
'----------------------------------------------------------------------------------------------------
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
#End Region
Partial Class OpeningTrailBalance
    Inherits System.Web.UI.Page
    'For accounts posting
    Dim caccounts As clssave = Nothing
    Dim cacc As clsAccounts = Nothing
    Dim ctran As clstran = Nothing
    Dim csubtran As clsSubTran = Nothing
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
    Dim gvRow As GridViewRow
    Dim NoDecRound As Integer
    Enum grd_col
        LienNo = 0
        AccCode = 1
        AccName = 2
        CostCentCode = 3
        CostCentName = 4
        Narration = 5
        Debit = 6
        Credit = 7
        Currency = 8
        ConvRate = 9
        BaseDebit = 10
        BaseCredit = 11
    End Enum
#End Region
#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region
#Region "Numbers"
    Public Sub NumbersHtml(ByVal txtbox As HtmlInputText)
        ' txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
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
#End Region
#Region "NumbersInt"
    Public Sub NumbersInt(ByVal txtbox As TextBox)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber1(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region
#Region "NumbersDateInt"
    Public Sub NumbersDateInt(ByVal txtbox As TextBox)
        Try
            txtbox.Attributes.Add("onkeypress", "return checkNumber2(event)")
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
        End Try
    End Sub
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                If CType(Session("GlobalUserName"), String) = Nothing Or CType(Session("Userpwd"), String) = Nothing Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ViewState.Add("divcode", Request.QueryString("divid"))
                txtconnection.Value = Session("dbconnectionName")

                txtAccType.Value = "G"
                txtAccName.Value = "General Ledger"

                '      objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcurr, "currcode", "acctcode", "select currcode,acctcode from  acctmast order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcurr, "cur", "code", "select cur,code from  view_account where div_code='" & ViewState("divcode") & "' order by code", True)

                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
                NoDecRound = CInt(txtdecimal.Value)
                Fill_grdAcc2()

                txtPostDate.Value = Format(CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 508).ToString, Date), "dd/MM/yyyy")


                txtTotalPartyBal.Value = Math.Round(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(sum(isnull(openbase_debit,0))- sum(isnull(openbase_credit,0) ),0) from  openparty_master where div_id='" & ViewState("divcode") & "'"), Decimal), 3)
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




                ViewState.Add("OpeningTrailBalanceState", Request.QueryString("State"))
                ViewState.Add("OpeningTrailBalanceRefCode", Request.QueryString("RefCode"))
                ViewState.Add("type", Request.QueryString("type"))

                If ViewState("OpeningTrailBalanceState") = "New" Then

                    txtDocNo.Value = ""
                    txtDocDesc.Value = "BALANCE B/F"

                    SetFocus(txtDocDesc)
                    lblHeading.Text = "Add Opening Trial Balance "
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save')==false)return false;")
                ElseIf ViewState("OpeningTrailBalanceState") = "Edit" Then
                    txtDocNo.Value = ViewState("OpeningTrailBalanceRefCode")
                    Fill_grdAcc1("code asc")

                    'txtTotalNetAmt.Value = CType(txtTotCrBace.Value, Double) - CType(txtTotDbBace.Value, Double)
                    'txtDiffAmt.Value = CType(txtTotalNetAmt.Value, Double) - CType(txtTotalPartyBal.Value, Double)
                    txtTotalNetAmt.Value = Math.Round(CType(CType(txtTotCrBace.Value, Double) - CType(txtTotDbBace.Value, Double), Decimal), NoDecRound)
                    txtDiffAmt.Value = Math.Round(CType(CType(txtTotalNetAmt.Value, Double) - CType(txtTotalPartyBal.Value, Double), Decimal), NoDecRound)

                    txtDocDesc.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acccommon_master", "narration", "tran_id", txtDocNo.Value).ToString
                    lblHeading.Text = "Edit Opening Trial Balance "
                    btnSave.Text = "Update"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update')==false)return false;")
                ElseIf ViewState("OpeningTrailBalanceState") = "View" Then
                    SetFocus(txtDocDesc)
                    txtDocNo.Value = ViewState("OpeningTrailBalanceRefCode")
                    Fill_grdAcc1("code asc")
                    'txtTotalNetAmt.Value = CType(txtTotCrBace.Value, Double) - CType(txtTotDbBace.Value, Double)
                    'txtDiffAmt.Value = CType(txtTotalNetAmt.Value, Double) - CType(txtTotalPartyBal.Value, Double)
                    txtTotalNetAmt.Value = Math.Round(CType(CType(txtTotCrBace.Value, Double) - CType(txtTotDbBace.Value, Double), Decimal), NoDecRound)
                    txtDiffAmt.Value = Math.Round(CType(CType(txtTotalNetAmt.Value, Double) - CType(txtTotalPartyBal.Value, Double), Decimal), NoDecRound)

                    txtDocDesc.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acccommon_master", "narration", "tran_id", txtDocNo.Value).ToString
                    txtDocDesc.Disabled = True
                    lblHeading.Text = "View Opening Trial Balance "

                ElseIf ViewState("OpeningTrailBalanceState") = "Delete" Then
                    SetFocus(txtDocDesc)
                    txtDocNo.Value = ViewState("OpeningTrailBalanceRefCode")
                    Fill_grdAcc1("code asc")
                    'txtTotalNetAmt.Value = CType(txtTotCrBace.Value, Double) - CType(txtTotDbBace.Value, Double)
                    'txtDiffAmt.Value = CType(txtTotalNetAmt.Value, Double) - CType(txtTotalPartyBal.Value, Double)
                    txtTotalNetAmt.Value = Math.Round(CType(CType(txtTotCrBace.Value, Double) - CType(txtTotDbBace.Value, Double), Decimal), NoDecRound)
                    txtDiffAmt.Value = Math.Round(CType(CType(txtTotalNetAmt.Value, Double) - CType(txtTotalPartyBal.Value, Double), Decimal), NoDecRound)

                    txtDocDesc.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acccommon_master", "narration", "tran_id", txtDocNo.Value).ToString
                    lblHeading.Text = "Delete Opening Trial Balance "
                    btnSave.Text = "Delete"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete')==false)return false;")
                End If
                If Val(txtTotalNetAmt.Value) = 0 Then
                    txtDr.Value = ""
                ElseIf Val(txtTotalNetAmt.Value) > 0 Then
                    txtDr.Value = "Cr"
                Else
                    txtDr.Value = "Dr"
                End If

                'CheckPostUnpostRight(CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType("Accounts Module", String), "AccountsModule\OpeningTrailBalanceSearch.aspx")
                DisableControls()

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OpeningTrailBalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")
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
    End Sub
#End Region
#Region "Private Function CreateDataSource(ByVal lngcount As Long) As DataView"
    Private Function CreateDataSource(ByVal lngcount As Long) As DataView
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        dt = New DataTable
        dt.Columns.Add(New DataColumn("LienNo", GetType(Integer)))
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
#Region " Public Sub Fill_grdAcc1()"
    Public Sub Fill_grdAcc1(ByVal orderby As String)
        Dim myDS As New DataSet
        Dim ordby As Integer
        Dim strdiv As String
        If ddlOrderBy.SelectedIndex = 2 Then
            ordby = 1
        Else
            ordby = 0
        End If

        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
        If ViewState("OpeningTrailBalanceState") = "Edit" Or ViewState("OpeningTrailBalanceState") = "Delete" Or ViewState("OpeningTrailBalanceState") = "View" Then
            If ViewState("type") = "OB1" Then
                strSqlQry = "exec sp_fill_gridTrialBal '" & txtDocNo.Value.Trim & "','OB1','G','" & ordby & "','" & ViewState("divcode") & "'"
            ElseIf ViewState("type") = "JV1" Then
                strSqlQry = "exec sp_fill_closeyearTrialBal '" & txtDocNo.Value.Trim & "','JV1','G','" & ordby & "','" & ViewState("divcode") & "'"
            End If
        Else
            If ViewState("type") = "OB1" Then
                strSqlQry = "exec sp_fill_gridTrialBal '','OB1','G','" & ordby & "','" & ViewState("divcode") & "'"
            ElseIf ViewState("type") = "JV1" Then
                strSqlQry = "exec sp_fill_closeyearTrialBal '','JV1','G','" & ordby & "','" & ViewState("divcode") & "'"
            End If
        End If
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS, "AccTable")
            'New Account Code Record Add In Last 

            myDS.Tables(0).DefaultView.Sort = orderby

            grdAcc1.DataSource = myDS.Tables("AccTable")

            grdAcc1.DataBind()
            SqlConn.Close()

            txtgridrows1.Value = grdAcc1.Rows.Count

            strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
            chkPost.Checked = False
        If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull(post_state,'')  from acccommon_master(nolock) where tran_id='" & txtDocNo.Value & "' and div_id='" & ViewState("divcode") & "' and tran_type ='OB1' ") = "P" Then
            chkPost.Checked = True
            lblPostmsg.Text = "Posted"
            lblPostmsg.ForeColor = Drawing.Color.Red
            chkPost.Checked = True
        Else
            lblPostmsg.Text = "UnPosted"
            lblPostmsg.ForeColor = Drawing.Color.Green
        End If
    End Sub
#End Region
#Region " Fill_grdAcc2()"
    Public Sub Fill_grdAcc2()
        fillDategrd(grdAcc2, False, 10)
        txtgridrows2.Value = grdAcc2.Rows.Count
    End Sub
#End Region
#Region "Protected Sub btnFillGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFillGrid.Click"
    Protected Sub btnFillGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFillGrid.Click
        If ddlOrderBy.SelectedIndex = 0 Then
            Fill_grdAcc1(ddlOrderBy.Items(1).Value)
        Else
            Fill_grdAcc1(ddlOrderBy.Items(ddlOrderBy.SelectedIndex).Value)
        End If
    End Sub
#End Region
#Region "Protected Sub grdAcc1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAcc1.RowDataBound"
    Protected Sub grdAcc1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAcc1.RowDataBound
        Dim txtCurrency As HtmlInputControl
        Dim txtDebit As HtmlInputControl
        Dim txtCredit As HtmlInputControl
        Dim txtconvrate As HtmlInputControl
        Dim lblLineNo As Label
        Dim txtBaseDebit As HtmlInputText
        Dim txtBaseCredit As HtmlInputText
        Dim strOpti As String

        Dim i As Integer = 0
        gvRow = e.Row

        If e.Row.RowIndex = -1 Then
            strOpti = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
            gvRow.Cells(grd_col.BaseCredit).Text = gvRow.Cells(grd_col.BaseCredit).Text & "(" & strOpti & ")"
            gvRow.Cells(grd_col.BaseDebit).Text = gvRow.Cells(grd_col.BaseDebit).Text & "(" & strOpti & ")"
            Exit Sub
        End If


        txtCurrency = gvRow.FindControl("txtCurrency1")
        txtDebit = gvRow.FindControl("txtDebit1")
        txtCredit = gvRow.FindControl("txtCredit1")
        txtconvrate = gvRow.FindControl("txtconvrate1")

        txtBaseDebit = gvRow.FindControl("txtBaseDebit1")
        txtBaseCredit = gvRow.FindControl("txtbaseCredit1")
        lblLineNo = gvRow.FindControl("lblLineNo")

        'txtDebit.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
        'txtCredit.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
        'txtconvrate.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
        NumbersHtml(txtDebit)
        NumbersHtml(txtCredit)
        NumbersHtml(txtconvrate)


        txtDebit.Attributes.Add("onchange", "javascript:convertInRate('" + CType(txtCredit.ClientID, String) + "','" + CType(txtDebit.ClientID, String) + "','" + CType(txtconvrate.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "')")
        txtCredit.Attributes.Add("onchange", "javascript:convertInRate('" + CType(txtDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtconvrate.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "')")
        txtconvrate.Attributes.Add("onchange", "javascript:convertInRateChange('" + CType(txtDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtconvrate.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "')")


        If ViewState("OpeningTrailBalanceState") = "Edit" Or ViewState("OpeningTrailBalanceState") = "Delete" Or ViewState("OpeningTrailBalanceState") = "View" Then
            'txtTotCrBace1.Value = CType(Val(txtTotCrBace1.Value), Double) + CType(Val(txtBaseCredit.Value), Double)
            'txtTotDbBace1.Value = CType(Val(txtTotDbBace1.Value), Double) + CType(Val(txtBaseDebit.Value), Double)
            'txtTotDbBace.Value = txtTotDbBace1.Value
            'txtTotCrBace.Value = txtTotCrBace1.Value
            txtTotCrBace1.Value = Math.Round(CType(CType(Val(txtTotCrBace1.Value), Double) + CType(Val(txtBaseCredit.Value), Double), Decimal), 3)
            txtTotDbBace1.Value = Math.Round(CType(CType(Val(txtTotDbBace1.Value), Double) + CType(Val(txtBaseDebit.Value), Double), Decimal), 3)
            txtTotDbBace.Value = Math.Round(CType(txtTotDbBace1.Value, Decimal), 3)
            txtTotCrBace.Value = Math.Round(CType(txtTotCrBace1.Value, Decimal), 3)
        Else
            lblLineNo.Text = e.Row.RowIndex + 1
        End If
    End Sub
#End Region
#Region "Protected Sub grdAcc2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAcc2.RowDataBound"
    Protected Sub grdAcc2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAcc2.RowDataBound
        Dim ddlAccCode As HtmlSelect
        Dim ddlAccName As HtmlSelect
        Dim ddlContCntCode As HtmlSelect
        Dim ddlCostCntName As HtmlSelect

        Dim txtDebit As HtmlInputControl
        Dim txtCredit As HtmlInputControl

        Dim txtCurrency As HtmlInputControl
        Dim txtconvrate As HtmlInputControl

        Dim txtBaseDebit As HtmlInputText
        Dim txtBaseCredit As HtmlInputText

        Dim txtNarration As HtmlInputText
        Dim txtauto As HtmlInputText
        Dim strOpti As String


        gvRow = e.Row
        If e.Row.RowIndex = -1 Then
            strOpti = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
            gvRow.Cells(grd_col.BaseCredit).Text = gvRow.Cells(grd_col.BaseCredit).Text & "(" & strOpti & ")"
            gvRow.Cells(grd_col.BaseDebit).Text = gvRow.Cells(grd_col.BaseDebit).Text & "(" & strOpti & ")"
            Exit Sub
        End If
        txtauto = gvRow.FindControl("accSearch")
        ddlAccCode = gvRow.FindControl("ddlAccCode")
        ddlAccName = gvRow.FindControl("ddlAccName")
        ddlContCntCode = gvRow.FindControl("ddlContCntCode")
        ddlCostCntName = gvRow.FindControl("ddlCostCntName")

        txtCurrency = gvRow.FindControl("txtCurrency")

        txtDebit = gvRow.FindControl("txtDebit")
        txtCredit = gvRow.FindControl("txtCredit")
        txtconvrate = gvRow.FindControl("txtconvrate")

        txtBaseDebit = gvRow.FindControl("txtBaseDebit")
        txtBaseCredit = gvRow.FindControl("txtbaseCredit")
        txtNarration = gvRow.FindControl("txtNarration")


        'txtDebit.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
        'txtCredit.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
        'txtconvrate.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
        'txtDebit.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")
        'txtCredit.Attributes.Add("onkeypress", "return checkNumberDecimal(event,this)")


        NumbersHtml(txtDebit)
        NumbersHtml(txtCredit)
        NumbersHtml(txtconvrate)


        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccCode, "acctcode", "acctname", "select acctcode,acctname from  acctmast where div_code='" & ViewState("divcode") & "' order by acctcode ", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccName, "acctname", "acctcode", "select acctname,acctcode from  acctmast where div_code='" & ViewState("divcode") & "' order by acctname", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlContCntCode, "costcenter_code", "costcenter_name", "select costcenter_code,costcenter_name from  costcenter_master  where active=1 and costcenter_code not in(select option_selected from reservation_parameters where param_id=510) order by costcenter_code", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCostCntName, "costcenter_name", "costcenter_code", "select costcenter_name,costcenter_code from  costcenter_master  where active=1 and costcenter_code not in(select option_selected from reservation_parameters where param_id=510) order by costcenter_name", True)

        ddlAccCode.Attributes.Add("onchange", "javascript:FillAccountCode('" + CType(ddlAccCode.ClientID, String) + "','" + CType(ddlAccName.ClientID, String) + "' ,'" + CType(txtCurrency.ClientID, String) + "','" + CType(txtconvrate.ClientID, String) + "','" + CType(txtNarration.ClientID, String) + "')")
        ddlAccName.Attributes.Add("onchange", "javascript:FillAccountName('" + CType(ddlAccCode.ClientID, String) + "','" + CType(ddlAccName.ClientID, String) + "' ,'" + CType(txtCurrency.ClientID, String) + "','" + CType(txtconvrate.ClientID, String) + "','" + CType(txtNarration.ClientID, String) + "')")


        ddlContCntCode.Attributes.Add("onchange", "javascript:FillCostCentCode('" + CType(ddlContCntCode.ClientID, String) + "','" + CType(ddlCostCntName.ClientID, String) + "' )")
        ddlCostCntName.Attributes.Add("onchange", "javascript:FillCostCentName('" + CType(ddlContCntCode.ClientID, String) + "','" + CType(ddlCostCntName.ClientID, String) + "' )")


        txtDebit.Attributes.Add("onchange", "javascript:convertInRate('" + CType(txtCredit.ClientID, String) + "','" + CType(txtDebit.ClientID, String) + "','" + CType(txtconvrate.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "')")
        txtCredit.Attributes.Add("onchange", "javascript:convertInRate('" + CType(txtDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtconvrate.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "')")

        txtconvrate.Attributes.Add("onchange", "javascript:convertInRateChange('" + CType(txtDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtconvrate.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "')")
        txtauto.Attributes.Add("onfocus", "javascript:MyAutoaccountsFillArray('" + CType(txtauto.ClientID, String) + "','" + CType(ddlAccName.ClientID, String) + "')")


        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
            ddlAccCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlAccName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlContCntCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlCostCntName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        End If
    End Sub
#End Region
#Region "Public Sub save_record()"
    Public Sub save_record()
        Dim intRow As Integer
        Dim txtCurrency As HtmlInputText
        Dim txtDebit As HtmlInputText
        Dim txtCredit As HtmlInputText
        Dim txtconvrate As HtmlInputText
        Dim txtnarration As HtmlInputText
        Dim txtBaseDebit As HtmlInputText
        Dim txtBaseCredit As HtmlInputText
        Dim ddlAccCode As HtmlSelect
        Dim ddlAccName As HtmlSelect
        Dim ddlContCntCode As HtmlSelect
        Dim ddlCostCntName As HtmlSelect
        Dim lblLineNo As Label
        Dim sqlTrans As SqlTransaction
        Dim strdiv As String



        'Changes 12/11/2008******************
        Dim balancingaccount As String = ""
        If ViewState("divcode") = "02" Then
            balancingaccount = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=522"), String)
        Else
            balancingaccount = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1145"), String)
        End If
        If objUtils.GetDBFieldFromStringnewdiv(Session("dbconnectionName"), "acctmast", "acctcode", "acctcode", balancingaccount, "div_code", ViewState("divcode")) = Nothing Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Balancing account not defined in Accounts Master');", True)
            Exit Sub
        End If
        'Changes 12/11/2008*******************


        If objUtils.GetDBFieldFromStringnewdiv(Session("dbconnectionName"), "docgen_div", "optionname", "optionname", "OB1", "div_id", ViewState("divcode")) = Nothing Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Transaction Type not defined in Number Generation');", True)
            Exit Sub
        End If

        strdiv = ViewState("divcode") 'objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

        Try
            If Page.IsValid = True Then
                If ViewState("OpeningTrailBalanceState") = "New" Or ViewState("OpeningTrailBalanceState") = "Edit" Then
                    If validate_page() = False Then
                        Exit Sub
                    End If
                End If
                If ViewState("OpeningTrailBalanceState") = "Edit" Or ViewState("OpeningTrailBalanceState") = "Delete" Then
                    If validate_BillAgainst() = False Then
                        Exit Sub
                    End If

                    If ViewState("OpeningTrailBalanceState") = "New" Or ViewState("OpeningTrailBalanceState") = "Edit" Or ViewState("OpeningTrailBalanceState") = "Delete" Then
                        If Validateseal() = False Then
                            Exit Sub
                        End If
                    End If

                End If
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start
                If chkPost.Checked = True Then
                    'For Accounts posting
                    initialclass(SqlConn, sqlTrans)
                    'For Accounts posting
                End If
                If (ViewState("OpeningTrailBalanceState") = "New" Or ViewState("OpeningTrailBalanceState") = "Edit") Then
                    If ViewState("OpeningTrailBalanceState") = "New" Then
                        'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                        Dim optionval As String

                        optionval = objUtils.GetAutoDocNodiv("OB1", SqlConn, sqlTrans, CType(ViewState("divcode"), String))
                        txtDocNo.Value = optionval.Trim
                        myCommand = New SqlCommand("sp_add_acc_common_master", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    ElseIf ViewState("OpeningTrailBalanceState") = "Edit" Then
                        myCommand = New SqlCommand("sp_mod_acc_common_master", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int)).Value = 0
                    End If

                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)

                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = "OB1"
                    myCommand.Parameters.Add(New SqlParameter("@tran_date ", SqlDbType.DateTime)).Value = txtPostDate.Value
                    myCommand.Parameters.Add(New SqlParameter("@type", SqlDbType.VarChar, 1)).Value = "G"
                    myCommand.Parameters.Add(New SqlParameter("@gl_code", SqlDbType.VarChar, 20)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@code", SqlDbType.VarChar, 20)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 200)).Value = txtDocDesc.Value
                    myCommand.Parameters.Add(New SqlParameter("@amount", SqlDbType.Money)).Value = CType(txtTotalNetAmt.Value, Double)
                    myCommand.Parameters.Add(New SqlParameter("@baseamount", SqlDbType.Money)).Value = CType(txtTotalNetAmt.Value, Double)
                    myCommand.Parameters.Add(New SqlParameter("@accgroup", SqlDbType.VarChar, 1)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@tran_state", SqlDbType.VarChar, 1)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@unknowncommon1", SqlDbType.VarChar, 50)).Value = "G"
                    myCommand.Parameters.Add(New SqlParameter("@unknowncommon2", SqlDbType.VarChar, 50)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@unknowncommon3", SqlDbType.VarChar, 50)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@unknowncommon4", SqlDbType.VarChar, 50)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@unknowncommon5", SqlDbType.VarChar, 50)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@unknowncommon6", SqlDbType.VarChar, 50)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@unknowncommon7", SqlDbType.VarChar, 50)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@unknowncommon8", SqlDbType.VarChar, 50)).Value = ""

                    myCommand.Parameters.Add(New SqlParameter("@unknowndate1", SqlDbType.DateTime)).Value = DBNull.Value
                    myCommand.Parameters.Add(New SqlParameter("@unknowndate2", SqlDbType.DateTime)).Value = DBNull.Value
                    myCommand.Parameters.Add(New SqlParameter("@unknowndate3", SqlDbType.DateTime)).Value = DBNull.Value

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
                        cacc.acc_tran_id = txtDocNo.Value
                        cacc.acc_tran_type = "OB1"
                        cacc.acc_tran_date = txtPostDate.Value
                        cacc.acc_div_id = strdiv




                        'Changes 12/11/2008******************
                        If CType(txtTotalNetAmt.Value, Double) <> 0 Then
                            ctran = New clstran
                            ctran.acc_tran_id = cacc.acc_tran_id
                            ctran.acc_code = balancingaccount
                            ctran.acc_type = "G"
                            ctran.acc_currency_id = mbasecurrency
                            ctran.acc_currency_rate = 1
                            ctran.acc_div_id = strdiv
                            ctran.acc_narration = "BALANCE B/F"
                            ctran.acc_tran_date = cacc.acc_tran_date
                            ctran.acc_tran_lineno = 0
                            ctran.acc_tran_type = cacc.acc_tran_type
                            ctran.pacc_gl_code = ""
                            ctran.acc_ref1 = ""
                            ctran.acc_ref2 = ""
                            ctran.acc_ref3 = ""
                            ctran.acc_ref4 = ""
                            cacc.addtran(ctran)

                            csubtran = New clsSubTran
                            csubtran.acc_against_tran_id = cacc.acc_tran_id
                            csubtran.acc_against_tran_lineno = 0
                            csubtran.acc_against_tran_type = cacc.acc_tran_type
                            csubtran.acc_debit = IIf(CType(txtTotalNetAmt.Value, Double) > 0, CType(txtTotalNetAmt.Value, Double), 0)
                            csubtran.acc_credit = IIf(CType(txtTotalNetAmt.Value, Double) < 0, Math.Abs(CType(txtTotalNetAmt.Value, Double)), 0)
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
                            csubtran.acc_narration = "BALANCE B/F"
                            csubtran.acc_type = "G"
                            csubtran.currate = 1
                            csubtran.acc_base_debit = IIf(CType(txtTotalNetAmt.Value, Double) > 0, CType(txtTotalNetAmt.Value, Double), 0)
                            csubtran.acc_base_credit = IIf(CType(txtTotalNetAmt.Value, Double) < 0, Math.Abs(CType(txtTotalNetAmt.Value, Double)), 0)
                            csubtran.costcentercode = ""
                            cacc.addsubtran(csubtran)

                        End If
                        'ctran = New clstran
                        'ctran.acc_tran_id = cacc.acc_tran_id
                        'ctran.acc_code = "3-02-01-02"
                        'ctran.acc_type = "G"
                        'ctran.acc_currency_id = mbasecurrency
                        'ctran.acc_currency_rate = 1
                        'ctran.acc_div_id = strdiv
                        'ctran.acc_narration = "BALANCE B/F"
                        'ctran.acc_tran_date = cacc.acc_tran_date
                        'ctran.acc_tran_lineno = 0
                        'ctran.acc_tran_type = cacc.acc_tran_type
                        'ctran.pacc_gl_code = ""
                        'ctran.acc_ref1 = ""
                        'ctran.acc_ref2 = ""
                        'ctran.acc_ref3 = ""
                        'ctran.acc_ref4 = ""
                        'cacc.addtran(ctran)

                        'csubtran = New clsSubTran
                        'csubtran.acc_against_tran_id = cacc.acc_tran_id
                        'csubtran.acc_against_tran_lineno = 0
                        'csubtran.acc_against_tran_type = cacc.acc_tran_type
                        'csubtran.acc_debit = IIf(CType(txtTotalNetAmt.Value, Double) > 0, CType(txtTotalNetAmt.Value, Double), 0)
                        'csubtran.acc_credit = IIf(CType(txtTotalNetAmt.Value, Double) < 0, Math.Abs(CType(txtTotalNetAmt.Value, Double)), 0)
                        'csubtran.acc_tran_date = cacc.acc_tran_date
                        'csubtran.acc_due_date = cacc.acc_tran_date
                        'csubtran.acc_field1 = ""
                        'csubtran.acc_field2 = ""
                        'csubtran.acc_field3 = ""
                        'csubtran.acc_field4 = ""
                        'csubtran.acc_field5 = ""
                        'csubtran.acc_tran_id = cacc.acc_tran_id
                        'csubtran.acc_tran_lineno = 0
                        'csubtran.acc_tran_type = cacc.acc_tran_type
                        'csubtran.acc_narration = "BALANCE B/F"
                        'csubtran.acc_type = "G"
                        'csubtran.currate = 1
                        'csubtran.acc_base_debit = IIf(CType(txtTotalNetAmt.Value, Double) > 0, CType(txtTotalNetAmt.Value, Double), 0)
                        'csubtran.acc_base_credit = IIf(CType(txtTotalNetAmt.Value, Double) < 0, Math.Abs(CType(txtTotalNetAmt.Value, Double)), 0)
                        'csubtran.costcentercode = ""
                        'cacc.addsubtran(csubtran)

                        'Changes 12/11/2008******************

                        'For Accounts Posting
                    End If

                    'Save In Detail Table
                    If ViewState("OpeningTrailBalanceState") = "Edit" Then
                        If chkPost.Checked = False Then
                            myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure
                            myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "acccommon_master"
                            myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                            myCommand.Parameters.Add(New SqlParameter("@trantype", SqlDbType.VarChar, 10)).Value = "OB1"
                            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                            myCommand.ExecuteNonQuery()
                        End If

                        myCommand = New SqlCommand("sp_del_acc_common_detail", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 10)).Value = txtDocNo.Value
                        'myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int)).Value = tran_lineno
                        myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "OB1"
                        myCommand.ExecuteNonQuery()
                    End If

                    For Each gvRow In grdAcc1.Rows
                        intRow = 1 + intRow
                        lblLineNo = gvRow.FindControl("lblLineNo")
                        'If Val(lblLineNo.Text) = 0 Then ''''comntd as ,if new accnt comes in between , thn error of primary key 
                        lblLineNo.Text = intRow
                        'End If
                        txtCurrency = gvRow.FindControl("txtCurrency1")
                        txtDebit = gvRow.FindControl("txtDebit1")
                        txtCredit = gvRow.FindControl("txtCredit1")
                        txtconvrate = gvRow.FindControl("txtconvrate1")
                        txtnarration = gvRow.FindControl("txtnarration1")
                        txtBaseDebit = gvRow.FindControl("txtBaseDebit1")
                        txtBaseCredit = gvRow.FindControl("txtbaseCredit1")

                        lblLineNo = gvRow.FindControl("lblLineNo")
                        If Val(txtDebit.Value) <> 0 Or Val(txtCredit.Value) <> 0 Then
                            If save_detail_record(SqlConn, sqlTrans, _
                                strdiv, txtDocNo.Value.Trim, CType(lblLineNo.Text, Integer), "OB1", "G", gvRow.Cells(1).Text _
                                , gvRow.Cells(3).Text, "", txtnarration.Value, txtCurrency.Value _
                                , txtconvrate.Value, "", CType(Val(txtDebit.Value), Double) _
                                , CType(Val(txtCredit.Value), Double), CType(Val(txtBaseDebit.Value), Double), _
                                CType(Val(txtBaseCredit.Value), Double)) = False Then
                                'sqlTrans.Rollback()
                                Exit Sub
                            End If
                        End If

                    Next






                    For Each gvRow In grdAcc2.Rows
                        intRow = 1 + intRow

                        ddlAccCode = gvRow.FindControl("ddlAccCode")
                        ddlAccName = gvRow.FindControl("ddlAccName")
                        ddlContCntCode = gvRow.FindControl("ddlContCntCode")
                        ddlCostCntName = gvRow.FindControl("ddlCostCntName")
                        If ddlAccCode.Value <> "[Select]" Then
                            txtCurrency = gvRow.FindControl("txtCurrency")
                            txtDebit = gvRow.FindControl("txtDebit")
                            txtCredit = gvRow.FindControl("txtCredit")
                            txtconvrate = gvRow.FindControl("txtconvrate")
                            txtnarration = gvRow.FindControl("txtnarration")
                            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
                            txtBaseCredit = gvRow.FindControl("txtbaseCredit")
                            If Val(txtDebit.Value) <> 0 Or Val(txtCredit.Value) <> 0 Then
                                If save_detail_record(SqlConn, sqlTrans, _
                                     strdiv, txtDocNo.Value.Trim, intRow, "OB1", "G", ddlAccName.Value.ToString _
                                     , ddlCostCntName.Value.ToString, "", txtnarration.Value, txtCurrency.Value _
                                     , txtconvrate.Value, "", CType(Val(txtDebit.Value), Double) _
                                     , CType(Val(txtCredit.Value), Double), CType(Val(txtBaseDebit.Value), Double), _
                                     CType(Val(txtBaseCredit.Value), Double)) = False Then
                                    'sqlTrans.Rollback()
                                    Exit Sub
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
                    End If
                    ' End If
                ElseIf ViewState("OpeningTrailBalanceState") = "Delete" Then

                    myCommand = New SqlCommand("sp_del_acccommon_master", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 10)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "OB1"
                    myCommand.ExecuteNonQuery()

                    'For Accounts Posting
                    myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@trantype", SqlDbType.VarChar, 10)).Value = "OB1"
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                    myCommand.ExecuteNonQuery()
                    'For Accounts Posting
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)           'connection close
                ' Response.Redirect("OpeningTrailBalanceSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('OpeningTrailBalanceWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If

        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OpeningTrailBalanceSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region
#Region "Public Function save_detail_record (...)"

    Public Function save_detail_record(ByVal SqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction, _
                ByVal div_id As String, ByVal tran_id As String, ByVal tran_lineno As Integer, ByVal tran_type As String, _
                ByVal type As String, ByVal code As String, ByVal costcentercode As String, ByVal gl_code As String, _
                ByVal narration As String, ByVal currcode As String, ByVal currency_rate As Decimal, _
                ByVal accgroup As String, ByVal debit As Double, ByVal credit As Double, _
                ByVal basedebit As Double, ByVal basecredit As Double) As Boolean
        save_detail_record = True

        myCommand = New SqlCommand("sp_add_acc_common_detail", SqlConn, sqlTrans)
        myCommand.CommandType = CommandType.StoredProcedure
        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = div_id
        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 10)).Value = tran_id
        myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int)).Value = tran_lineno

        myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = tran_type
        myCommand.Parameters.Add(New SqlParameter("@type", SqlDbType.VarChar, 1)).Value = type
        myCommand.Parameters.Add(New SqlParameter("@code", SqlDbType.VarChar, 20)).Value = code
        myCommand.Parameters.Add(New SqlParameter("@costcentercode", SqlDbType.VarChar, 20)).Value = "GEN" 'costcentercode

        myCommand.Parameters.Add(New SqlParameter("@gl_code", SqlDbType.VarChar, 20)).Value = gl_code

        myCommand.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 200)).Value = narration
        myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = currcode

        myCommand.Parameters.Add(New SqlParameter("@currency_rate", SqlDbType.Decimal, 18, 12)).Value = currency_rate
        myCommand.Parameters.Add(New SqlParameter("@accgroup", SqlDbType.VarChar, 1)).Value = accgroup

        myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = debit
        myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = credit
        myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = basedebit
        myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = basecredit

        Dim param1 As SqlParameter
        param1 = New SqlParameter

        param1.ParameterName = "@allowflg"
        param1.Direction = ParameterDirection.Output
        param1.DbType = DbType.Int16
        param1.Size = 9
        myCommand.Parameters.Add(param1)

        Dim param2 As SqlParameter
        param2 = New SqlParameter
        param2.ParameterName = "@errmsg"
        param2.Direction = ParameterDirection.Output
        param2.DbType = DbType.String
        param2.DbType = DbType.String
        param2.Size = 500

        myCommand.Parameters.Add(param2)
        myCommand.CommandTimeout = 120
        Dim da As New SqlDataAdapter(myCommand)
        myCommand.ExecuteNonQuery()
        Dim Alflg As Integer
        Dim ErrMsg As String
        Alflg = param1.Value
        ErrMsg = param2.Value
        If Alflg = 1 And ErrMsg <> "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg & "');", True)
            sqlTrans.Rollback()
            save_detail_record = False
            Exit Function
        End If
        If chkPost.Checked = True Then
            'For Accounts Posting

            ctran = New clstran
            ctran.acc_tran_id = cacc.acc_tran_id
            ctran.acc_code = code
            ctran.acc_type = "G"
            ctran.acc_currency_id = currcode
            ctran.acc_currency_rate = currency_rate
            ctran.acc_div_id = div_id
            ctran.acc_narration = narration
            ctran.acc_tran_date = cacc.acc_tran_date
            ctran.acc_tran_lineno = tran_lineno
            ctran.acc_tran_type = tran_type
            ctran.pacc_gl_code = ""
            ctran.acc_ref1 = ""
            ctran.acc_ref2 = ""
            ctran.acc_ref3 = ""
            ctran.acc_ref4 = ""
            cacc.addtran(ctran)

            csubtran = New clsSubTran
            csubtran.acc_against_tran_id = cacc.acc_tran_id
            csubtran.acc_against_tran_lineno = tran_lineno
            csubtran.acc_against_tran_type = tran_type
            csubtran.acc_credit = credit
            csubtran.acc_debit = debit
            csubtran.acc_tran_date = cacc.acc_tran_date
            csubtran.acc_due_date = cacc.acc_tran_date
            csubtran.acc_field1 = ""
            csubtran.acc_field2 = ""
            csubtran.acc_field3 = ""
            csubtran.acc_field4 = ""
            csubtran.acc_field5 = ""
            csubtran.acc_tran_id = cacc.acc_tran_id
            csubtran.acc_tran_lineno = tran_lineno
            csubtran.acc_tran_type = tran_type
            csubtran.acc_narration = narration
            csubtran.acc_type = "G"
            csubtran.currate = currency_rate
            csubtran.acc_base_credit = basecredit
            csubtran.acc_base_debit = basedebit
            csubtran.costcentercode = costcentercode
            cacc.addsubtran(csubtran)
            'For Accounts Posting
        End If

    End Function
#End Region

    Public Function Validateseal() As Boolean
        Try

            Validateseal = True
            Dim invdate As DateTime
            Dim sealdate As DateTime
            Dim MyCultureInfo As New CultureInfo("fr-Fr")
            invdate = DateTime.Parse(txtPostDate.Value, MyCultureInfo, DateTimeStyles.None)
            sealdate = DateTime.Parse(txtpdate.Text, MyCultureInfo, DateTimeStyles.None)
            If invdate <= DateAdd(DateInterval.Day, -1, sealdate) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Document has sealed in this period cannot make entry.Close the entry and make with another date')", True)
                Validateseal = False
            End If

        Catch ex As Exception
            Validateseal = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("requestforinvoicing.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#Region "Public Function validate_page() As Boolean"
    Public Function validate_page() As Boolean
        validate_page = True
        'If Val(txtTotDbBace2.Value) = 0 And Val(txtTotCrBace.Value) = 0 Then 'need to check this
        If Val(txtTotDbBace.Value) = 0 And Val(txtTotCrBace.Value) = 0 Then
            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please fill valid debit or credit amount.');", True)
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Either debit or credit amount should be entered .');", True)
            'validate_page = False
            Exit Function
        End If
        Dim txtCurrency As HtmlInputText
        Dim txtDebit As HtmlInputText
        Dim txtCredit As HtmlInputText
        Dim txtconvrate As HtmlInputText
        Dim txtnarration As HtmlInputText
        Dim txtBaseDebit As HtmlInputText
        Dim txtBaseCredit As HtmlInputText
        Dim ddlAccCode As HtmlSelect
        Dim ddlAccName As HtmlSelect
        Dim ddlContCntCode As HtmlSelect
        Dim ddlCostCntName As HtmlSelect

        For Each gvRow In grdAcc1.Rows

            'txtCurrency = gvRow.FindControl("txtCurrency1")
            'txtDebit = gvRow.FindControl("txtDebit1")
            'txtCredit = gvRow.FindControl("txtCredit1")
            'txtconvrate = gvRow.FindControl("txtconvrate1")
            'txtnarration = gvRow.FindControl("txtnarration1")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit1")
            txtBaseCredit = gvRow.FindControl("txtbaseCredit1")
            If Val(txtBaseDebit.Value) < 0 Or Val(txtBaseCredit.Value) < 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please fill valid debit or credit amount.');", True)
                SetFocus(txtBaseDebit)
                validate_page = False
                Exit Function
            End If
        Next

        For Each gvRow In grdAcc2.Rows
            ddlAccCode = gvRow.FindControl("ddlAccCode")
            ddlAccName = gvRow.FindControl("ddlAccName")
            ddlContCntCode = gvRow.FindControl("ddlContCntCode")
            ddlCostCntName = gvRow.FindControl("ddlCostCntName")

            txtCurrency = gvRow.FindControl("txtCurrency")
            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtconvrate = gvRow.FindControl("txtconvrate")
            txtnarration = gvRow.FindControl("txtnarration")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtbaseCredit")


            If ddlAccCode.Value <> "[Select]" Then
                If ddlContCntCode.Value = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select cost center code.');", True)
                    SetFocus(ddlContCntCode)
                    validate_page = False
                    Exit Function
                End If

                If Val(txtBaseDebit.Value) <= 0 And Val(txtBaseCredit.Value) <= 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please fill valid debit or credit amount.');", True)
                    SetFocus(txtBaseDebit)
                    validate_page = False
                    Exit Function
                End If
            End If

            If ddlContCntCode.Value <> "[Select]" Then
                If ddlAccCode.Value = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select account code.');", True)
                    SetFocus(ddlAccCode)
                    validate_page = False
                    Exit Function
                End If
            End If

        Next

    End Function
#End Region
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) "
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        save_record()

    End Sub
#End Region
#Region "Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) "
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Response.Redirect("OpeningTrailBalanceSearch.aspx", False)
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();setTimeout('self.close();',30000); ", True)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        'Dim strscript As String = ""
        'strscript = "window.opener.__doPostBack('OpeningTrailBalanceWindowPostBack', '');window.opener.focus();window.close();"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

    End Sub
#End Region
    Private Sub DisableControls()
        If ViewState("OpeningTrailBalanceState") = "New" Then
        ElseIf ViewState("OpeningTrailBalanceState") = "Edit" Then
        ElseIf ViewState("OpeningTrailBalanceState") = "View" Or ViewState("OpeningTrailBalanceState") = "Delete" Then
            ' ddlcurr.Disabled = True
            txtAccName.Disabled = True
            txtAccType.Disabled = True
            'txtdecimal.Disabled = True
            ' txtDiffAmt.Disabled = True
            txtDocDesc.Disabled = True
            txtDocNo.Disabled = True
            'txtgridrows1.Disabled = True
            'txtgridrows2.Disabled = True
            txtPostDate.Disabled = True
            'grdAcc1.Enabled = False
            'grdAcc2.Enabled = False
            btnSave.Visible = False
            chkPost.Visible = False
            DisableGrid()
        End If
        If ViewState("OpeningTrailBalanceState") = "Delete" Then
            btnSave.Visible = True
        End If
    End Sub
    Private Sub DisableGrid()

        Dim txtCurrency As HtmlInputText
        Dim txtDebit As HtmlInputText
        Dim txtCredit As HtmlInputText
        Dim txtconvrate As HtmlInputText
        Dim txtnarration As HtmlInputText
        Dim txtBaseDebit As HtmlInputText
        Dim txtBaseCredit As HtmlInputText
        Dim ddlAccCode As HtmlSelect
        Dim ddlAccName As HtmlSelect
        Dim ddlContCntCode As HtmlSelect
        Dim ddlCostCntName As HtmlSelect




        For Each gvRow In grdAcc1.Rows
            txtCurrency = gvRow.FindControl("txtCurrency1")
            txtDebit = gvRow.FindControl("txtDebit1")
            txtCredit = gvRow.FindControl("txtCredit1")
            txtconvrate = gvRow.FindControl("txtconvrate1")
            txtnarration = gvRow.FindControl("txtnarration1")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit1")
            txtBaseCredit = gvRow.FindControl("txtbaseCredit1")

            txtCurrency.Disabled = True
            txtDebit.Disabled = True
            txtCredit.Disabled = True
            txtconvrate.Disabled = True
            txtnarration.Disabled = True
            txtBaseDebit.Disabled = True
            txtBaseCredit.Disabled = True
        Next

        For Each gvRow In grdAcc2.Rows
            ddlAccCode = gvRow.FindControl("ddlAccCode")
            ddlAccName = gvRow.FindControl("ddlAccName")
            ddlContCntCode = gvRow.FindControl("ddlContCntCode")
            ddlCostCntName = gvRow.FindControl("ddlCostCntName")

            txtCurrency = gvRow.FindControl("txtCurrency")
            txtDebit = gvRow.FindControl("txtDebit")
            txtCredit = gvRow.FindControl("txtCredit")
            txtconvrate = gvRow.FindControl("txtconvrate")
            txtnarration = gvRow.FindControl("txtnarration")
            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
            txtBaseCredit = gvRow.FindControl("txtbaseCredit")

            ddlAccCode.Disabled = True
            ddlAccName.Disabled = True
            ddlContCntCode.Disabled = True
            ddlCostCntName.Disabled = True
            txtCurrency.Disabled = True
            txtDebit.Disabled = True
            txtCredit.Disabled = True
            txtconvrate.Disabled = True
            txtBaseDebit.Disabled = True
            txtBaseCredit.Disabled = True
        Next

    End Sub

    Protected Sub ddlOrderBy_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddlOrderBy.SelectedIndex = 0 Then
            Fill_grdAcc1(ddlOrderBy.Items(1).Value)
        Else
            Fill_grdAcc1(ddlOrderBy.Items(ddlOrderBy.SelectedIndex).Value)
        End If


    End Sub
    Private Sub initialclass(ByVal con As SqlConnection, ByVal stran As SqlTransaction)
        caccounts = Nothing
        cacc = Nothing
        ctran = Nothing
        csubtran = Nothing
        caccounts = New clssave
        cacc = New clsAccounts
        cacc.clropencol()
        cacc.tran_mode = IIf(ViewState("OpeningTrailBalanceState") = "New", 1, 2)
        mbasecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
        cacc.start()

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OpeningTrailBalance','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Private Function validate_BillAgainst() As Boolean
        Try
            validate_BillAgainst = True
            Dim Alflg As Integer
            Dim ErrMsg, strdiv As String
            strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myCommand = New SqlCommand("sp_Check_AgainstBills", SqlConn)
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = ViewState("divcode")
            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
            myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = "OB1"

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
            objUtils.WritErrorLog("OpeningSupplierBalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
            If ViewState("OpeningTrailBalanceState") = "Edit" Then
                If chkPost.Checked = True Then
                    ViewState.Add("OpeningTrailBalanceState", "View")
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This transaction has been posted, you do not have rights to edit.' );", True)
                End If
            End If
        End If
    End Sub

End Class


'myDS.Tables(0).DefaultView.Sort = "tran_lineno DESC"
'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlContCntCode, "costcenter_code", "costcenter_name", "select * from  costcenter_master  where active=1", True)
'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlCostCntName, "costcenter_name", "costcenter_code", "select * from  costcenter_master  where active=1", True)

'txtTotalPartyBal.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"),"select isnull(sum(isnull(openbase_debit,0))- sum(isnull(openbase_credit,0) ),0) from  openparty_master")

'Delete Record
'      SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
'     sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

'For Each gvRow In grdAcc1.Rows
'    intRow = 1 + intRow
'    lblLineNo = gvRow.FindControl("lblLineNo")
'    If Val(lblLineNo.Text) = 0 Then
'        lblLineNo.Text = intRow
'    End If

'myCommand = New SqlCommand("sp_del_acc_common_detail", SqlConn, sqlTrans)
'myCommand.CommandType = CommandType.StoredProcedure
'myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
'myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 10)).Value = txtDocNo.Value
'myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int)).Value = CType(lblLineNo.Text, Integer)
'myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = "OB1"
'myCommand.ExecuteNonQuery()

'Next
