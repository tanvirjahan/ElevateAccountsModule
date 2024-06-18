'----------------------------------------------------------------------------------------------------
'   Module Name    :    OpeningSupplierBalance
'   Developer Name :    Mangesh 
'   Date           :    
'   
'
'----------------------------------------------------------------------------------------------------
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Collections
Imports System.Collections.Generic
#End Region
Partial Class OpeningSupplierBalance
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

    Dim lblLineNo As Label

    Dim txtBillType As HtmlInputText
    ' Dim dtDueDate As EclipseWebSolutions.DatePicker.DatePicker
    'Dim dtDate As EclipseWebSolutions.DatePicker.DatePicker
    Dim txtDate As TextBox
    Dim txtDueDate As TextBox
    Dim txtDoNo As HtmlInputText
    Dim txtotherRef As HtmlInputText
    Dim txtDr As HtmlInputText
    Dim txtCr As HtmlInputText
    Dim txtBaseDr As HtmlInputText
    Dim txtBaseCr As HtmlInputText
    Dim txtfield1 As HtmlInputText
    Dim txtfield2 As HtmlInputText
    Dim txtfield3 As HtmlInputText
    Dim chkDel As CheckBox

    Enum grd_col
        LienNo = 0
        dtDate = 1
        BillType = 2
        DueDate = 3
        DocNo = 4

        Debit = 5
        Credit = 6
        BaseDebit = 7
        BaseCredit = 8
    End Enum
#End Region



    <System.Web.Script.Services.ScriptMethod()> _
        <System.Web.Services.WebMethod()> _
    Public Shared Function Getcustlist(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Custnames As New List(Of String)
        Try

            If HttpContext.Current.Session("OpeningSupplierBalanceState") = "New" Then
                strSqlQry = "select agentcode,agentname from  agentmast where divcode='" & HttpContext.Current.Session("div_code") & "' and  active=1 and agentcode not in(Select open_code from openparty_master where div_id='" & HttpContext.Current.Session("div_code") & "' and open_type='C') and   agentname like  '" & Trim(prefixText) & "%'  order by agentname "
            Else
                strSqlQry = "select agentcode,agentname from agentmast where active=1 and  agentname like  '" & Trim(prefixText) & "%' and divcode in (" & HttpContext.Current.Session("div_code") & ") order by agentname"
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
                    Custnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("agentname").ToString(), myDS.Tables(0).Rows(i)("agentcode").ToString()))

                Next
            End If
            Return Custnames
        Catch ex As Exception
            Return Custnames
        End Try

    End Function


    <System.Web.Script.Services.ScriptMethod()> _
       <System.Web.Services.WebMethod()> _
    Public Shared Function Getsupplist(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppnames As New List(Of String)
        Try
            If HttpContext.Current.Session("OpeningSupplierBalanceState") = "New" Then
                strSqlQry = "select Partycode,Partyname from  partymast where active=1 and partycode not in(Select open_code from openparty_master where   div_id='" & HttpContext.Current.Session("div_code") & "' and open_type='S') and   partyname like  '" & Trim(prefixText) & "%'  order by partyname"
            Else
                strSqlQry = "select partycode,partyname from partymast where active=1 and  partyname like  '" & Trim(prefixText) & "%' order by partyname"
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
                    suppnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("partyname").ToString(), myDS.Tables(0).Rows(i)("partycode").ToString()))

                Next
            End If
            Return suppnames
        Catch ex As Exception
            Return suppnames
        End Try

    End Function


    <System.Web.Script.Services.ScriptMethod()> _
      <System.Web.Services.WebMethod()> _
    Public Shared Function Getsuppagentlist(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppnames As New List(Of String)
        Try
            If HttpContext.Current.Session("OpeningSupplierBalanceState") = "New" Then
                strSqlQry = "select supagentcode,supagentname from  Supplier_agents where active=1 and supagentcode not in(Select open_code from openparty_master where div_id='" & HttpContext.Current.Session("div_code") & "' and open_type='A') and   supagentname like  '" & Trim(prefixText) & "%'   order by supagentname"
            Else
                strSqlQry = "select supagentcode,supagentname from Supplier_agents  where active=1 and  supagentname like  '" & Trim(prefixText) & "%' order by supagentname"
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
                    suppnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("supagentname").ToString(), myDS.Tables(0).Rows(i)("supagentcode").ToString()))

                Next
            End If
            Return suppnames
        Catch ex As Exception
            Return suppnames
        End Try

    End Function
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
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If ViewState("OpeningSupplierBalanceOpenType") = "S" Then

            TxtCustName_AutoCompleteExtender.ServiceMethod = "Getsupplist"
        ElseIf ViewState("OpeningSupplierBalanceOpenType") = "C" Then

            TxtCustName_AutoCompleteExtender.ServiceMethod = "Getcustlist"
        ElseIf ViewState("OpeningSupplierBalanceOpenType") = "A" Then
            TxtCustName_AutoCompleteExtender.ServiceMethod = "Getsuppagentlist"

        End If
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

                ViewState.Add("OpeningSupplierBalanceState", Request.QueryString("State"))
                ViewState.Add("OpeningSupplierBalanceRefCode", Request.QueryString("RefCode"))
                ViewState.Add("OpeningSupplierBalanceOpenType", Request.QueryString("OpenType"))
                ViewState.Add("divcode", Request.QueryString("divid"))

                Session("OpeningSupplierBalanceState") = Request.QueryString("State")
                Session("div_code") = Request.QueryString("divid")
                'set reservation_parameters 
                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
                'Fill Supplier
                SetFocus(txtsuppcode)


                If ViewState("OpeningSupplierBalanceOpenType") = "S" Then
                    'Session.Add("tran_type", "OBS")
                    ViewState.Add("OpeningSupplierBalanceTranType", "OBS")
                    lblHeading.Text = "Opening Supplier Balance"
                    lblcode.Text = "Supplier"
                    Label3.Text = "OBS NO"
                    txttype.Value = "S"
                ElseIf ViewState("OpeningSupplierBalanceOpenType") = "C" Then
                    lblcode.Text = "Customer"

                    lblHeading.Text = "Opening Customer Balance"
                    'Session.Add("tran_type", "OBC")
                    ViewState.Add("OpeningSupplierBalanceTranType", "OBC")
                    Label3.Text = "OBC NO"
                    txttype.Value = "C"
                ElseIf ViewState("OpeningSupplierBalanceOpenType") = "A" Then
                    lblcode.Text = "Supplier Agent"

                    lblHeading.Text = "Opening Supplier Agent Balance"
                    txttype.Value = "A"
                    Label3.Text = "OBSA NO"
                    'Session.Add("tran_type", "OBSA")
                    ViewState.Add("OpeningSupplierBalanceTranType", "OBSA")
                End If





                If ViewState("OpeningSupplierBalanceOpenType") = "S" Then

                    TxtCustName_AutoCompleteExtender.ServiceMethod = "Getsupplist"
                ElseIf ViewState("OpeningSupplierBalanceOpenType") = "C" Then

                    TxtCustName_AutoCompleteExtender.ServiceMethod = "Getcustlist"
                ElseIf ViewState("OpeningSupplierBalanceOpenType") = "A" Then
                    TxtCustName_AutoCompleteExtender.ServiceMethod = "Getsuppagentlist"

                End If


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

                'Base Cuurncy
                txtbasecurr.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 457)
                'Doc Date
                txtDocDate.Text = Format(CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 508).ToString, Date), "dd/MM/yyyy")



                txtConvRate.Attributes.Add("onchange", "changeRate()")
                If ViewState("OpeningSupplierBalanceState") = "New" Then
                    fillDategrd(grdRecord, True, 10)
                    txtDocNo.Value = ""
                    lblHeading.Text = "Add " & lblHeading.Text
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save')==false)return false;")
                ElseIf ViewState("OpeningSupplierBalanceState") = "Edit" Then
                    txtDocNo.Value = ViewState("OpeningSupplierBalanceRefCode")
                    show_record(txtDocNo.Value)
                    lblHeading.Text = "Edit " & lblHeading.Text
                    btnSave.Text = "Update"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update')==false)return false;")
                ElseIf ViewState("OpeningSupplierBalanceState") = "View" Then
                    txtDocNo.Value = ViewState("OpeningSupplierBalanceRefCode")
                    show_record(txtDocNo.Value)
                    lblHeading.Text = "View " & lblHeading.Text
                ElseIf ViewState("OpeningSupplierBalanceState") = "Delete" Then
                    txtDocNo.Value = ViewState("OpeningSupplierBalanceRefCode")
                    show_record(txtDocNo.Value)
                    lblHeading.Text = "Delete " & lblHeading.Text
                    btnSave.Text = "Delete"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete')==false)return false;")
                End If

                '  CheckPostUnpostRight(CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType("Accounts Module", String), "AccountsModule\OpeningSupplierBalanceSearch.aspx?tran_type=" & ViewState("OpeningSupplierBalanceOpenType"))
                DisableAllControls()

                btnExit.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")


                'Set Attribut For Supplier

                NumbersHtml(txtConvRate)
                Dim typ As Type
                typ = GetType(DropDownList)


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OpeningSupplierBalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region
#Region "Public Sub show_record()"
    Public Sub show_record(ByVal RefCode As String)
        Dim myDS As New DataSet
        Dim mySqlReader As SqlDataReader
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
        myCommand = New SqlCommand("Select * from openparty_master Where div_id='" & ViewState("divcode") & "' and tran_id='" & RefCode & "'", SqlConn)
        mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)

        If mySqlReader.HasRows Then
            If mySqlReader.Read() = True Then
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

                txtDocNo.Value = CType(mySqlReader("tran_id"), String)
                txtDocDate.Text = Format(CType(mySqlReader("tran_date"), Date), "dd/MM/yyyy")


                txtsuppcode.Text = mySqlReader("open_code").ToString


                Select Case ViewState("OpeningSupplierBalanceOpenType")

                    Case "S"
                        txtsuppname.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select Partyname From partymast where partycode='" & txtsuppcode.Text & "'"), String)

                    Case "C"
                        txtsuppname.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select agentname From agentmast where agentcode='" & txtsuppcode.Text & "'"), String)

                    Case "A"
                        txtsuppname.Text = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select supagentname From Supplier_agents where supagentcode='" & txtsuppcode.Text & "'"), String)

                End Select


                TxtCurrCode.Value = mySqlReader("currcode")




                txtConvRate.Value = mySqlReader("currrate")

                txtConvRate.Disabled = False
                If Trim(txtbasecurr.Value) = Trim(TxtCurrCode.Value) Then
                    txtConvRate.Disabled = True
                End If

                txtsuppname.Enabled = False

            End If
        End If
        mySqlReader.Close()
        myCommand.Dispose()
        SqlConn.Close()


        Dim d As Double
        'Open connection
        Dim lngCnt As Long
        lngCnt = objUtils.GetDBFieldFromStringnewdiv(Session("dbconnectionName"), "openparty_detail", "count(tran_id)", "tran_id", RefCode, "div_id", ViewState("divcode"))
        If lngCnt = 0 Then lngCnt = 1
        fillDategrd(grdRecord, False, lngCnt)
        SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'co
        strSqlQry = "Select * from openparty_detail Where div_id='" & ViewState("divcode") & "' and tran_id='" & RefCode & "' order by tran_lineno"
        myCommand = New SqlCommand(strSqlQry, SqlConn)
        mySqlReader = myCommand.ExecuteReader()
        If mySqlReader.HasRows Then
            While mySqlReader.Read()
                For Each gvRow In grdRecord.Rows
                    lblLineNo = gvRow.FindControl("lblLineNo")
                    If mySqlReader("tran_lineno") = CType(lblLineNo.Text, Integer) Then
                        txtDate = gvRow.FindControl("txtDate")
                        txtBillType = gvRow.FindControl("txtBillType")
                        txtDueDate = gvRow.FindControl("txtDueDate")
                        txtDoNo = gvRow.FindControl("txtDocNo")
                        txtotherRef = gvRow.FindControl("txtOtherRef")

                        '30112014
                        txtfield1 = gvRow.FindControl("txtfield1")
                        txtfield2 = gvRow.FindControl("txtfield2")
                        txtfield3 = gvRow.FindControl("txtfield3")

                        txtDr = gvRow.FindControl("txtDebit")
                        txtCr = gvRow.FindControl("txtCredit")

                        txtBaseDr = gvRow.FindControl("txtBaseDebit")
                        txtBaseCr = gvRow.FindControl("txtbaseCredit")

                        If IsDBNull(mySqlReader("against_tran_date")) = False Then
                            txtDate.Text = Format(mySqlReader("against_tran_date"), "dd/MM/yyyy")
                        End If
                        If IsDBNull(mySqlReader("open_due_date")) = False Then
                            txtDueDate.Text = Format(mySqlReader("open_due_date"), "dd/MM/yyyy")
                        End If

                        If IsDBNull(mySqlReader("tran_lineno")) = False Then
                            lblLineNo.Text = mySqlReader("tran_lineno")
                        End If

                        If IsDBNull(mySqlReader("against_bill_type")) = False Then
                            txtBillType.Value = mySqlReader("against_bill_type").ToString
                        End If
                        If IsDBNull(mySqlReader("against_tran_id")) = False Then
                            txtDoNo.Value = mySqlReader("against_tran_id").ToString
                        End If
                        If IsDBNull(mySqlReader("Other_reference")) = False Then
                            txtotherRef.Value = mySqlReader("Other_reference").ToString
                        End If

                        If IsDBNull(mySqlReader("open_debit")) = False Then
                            d = mySqlReader("open_debit")
                            txtDr.Value = d.ToString("0.00")
                        End If
                        If IsDBNull(mySqlReader("open_credit")) = False Then
                            d = Val(mySqlReader("open_credit"))
                            txtCr.Value = d.ToString("0.00")
                        End If

                        '30122014
                        If IsDBNull(mySqlReader("field1")) = False Then
                            txtfield1.Value = mySqlReader("field1").ToString
                        End If
                        If IsDBNull(mySqlReader("field2")) = False Then
                            txtfield2.Value = mySqlReader("field2").ToString
                        End If
                        If IsDBNull(mySqlReader("field3")) = False Then
                            txtfield3.Value = mySqlReader("field3").ToString
                        End If



                        If Val(txtDr.Value) <> 0 Then txtCr.Attributes.Add("readonly", "readonly")
                        If Val(txtCr.Value) <> 0 Then txtDr.Attributes.Add("readonly", "readonly")


                        'TextBox1.Attributes.Add("readonly", "readonly");

                        If IsDBNull(mySqlReader("open_base_debit")) = False Then
                            d = Val(mySqlReader("open_base_debit"))
                            txtBaseDr.Value = d.ToString("0.00")
                        End If
                        If IsDBNull(mySqlReader("open_base_credit")) = False Then
                            d = Val(mySqlReader("open_base_credit"))
                            txtBaseCr.Value = d.ToString("0.00")
                        End If
                    End If
                Next
                d = CType(Val(txtTotCredit.Value), Double) + CType(Val(txtCr.Value), Double)
                txtTotCredit.Value = d.ToString("0.00")
                d = CType(Val(txtTotDebit.Value), Double) + CType(Val(txtDr.Value), Double)
                txtTotDebit.Value = d.ToString("0.00")

                d = CType(Val(txtTotBaseCredit.Value), Double) + CType(Val(txtBaseCr.Value), Double)
                txtTotBaseCredit.Value = d.ToString("0.00")
                d = CType(Val(txtTotBaseDebit.Value), Double) + CType(Val(txtBaseDr.Value), Double)
                txtTotBaseDebit.Value = d.ToString("0.00")
            End While
            d = CType(Val(txtTotCredit.Value), Double) - CType(Val(txtTotDebit.Value), Double)
            txtNetBal.Value = d.ToString("0.00")
            d = CType(Val(txtTotBaseCredit.Value), Double) - CType(Val(txtTotBaseDebit.Value), Double)
            txtNetBalBase.Value = d.ToString("0.00")
        End If
        mySqlReader.Close()
        SqlConn.Close()
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
        dt.Columns.Add(New DataColumn("tran_lineno", GetType(Integer)))

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
#Region "Protected Sub grdRecord_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRecord.RowDataBound"
    Protected Sub grdRecord_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRecord.RowDataBound

        Dim txtDebit As HtmlInputText
        Dim txtCredit As HtmlInputText

        Dim txtBaseDebit As HtmlInputText
        Dim txtBaseCredit As HtmlInputText
        Dim txtDate As TextBox
        Dim txtDueDate As TextBox
        'Dim ImgBtnDt As ImageButton
        'Dim ImgBtnDueDate As ImageButton

        Dim strOpti As String
        Dim strtable As String = ""
        Dim strfiled As String = ""
        Dim i As Integer = 0
        gvRow = e.Row

        If e.Row.RowIndex = -1 Then
            strOpti = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
            gvRow.Cells(grd_col.BaseDebit).Text = "Debit(" & strOpti & ")"
            gvRow.Cells(grd_col.BaseCredit).Text = "Credit(" & strOpti & ")"
            Exit Sub
        End If

        txtDate = gvRow.FindControl("txtDate")
        txtDueDate = gvRow.FindControl("txtDueDate")
        'ImgBtnDt = gvRow.FindControl("ImgBtnDt")
        'ImgBtnDueDate = gvRow.FindControl("ImgBtnDueDate")
        'CalendarExtender1.PopupButtonID = ImgBtnDt.ClientID
        'CalendarExtender1.TargetControlID = txtDate.ClientID

        'CalendarExtender2.PopupButtonID = ImgBtnDueDate.ClientID
        'CalendarExtender1.TargetControlID = txtDueDate.ClientID

        txtDebit = gvRow.FindControl("txtDebit")
        txtCredit = gvRow.FindControl("txtCredit")

        txtBaseDebit = gvRow.FindControl("txtBaseDebit")
        txtBaseCredit = gvRow.FindControl("txtbaseCredit")

        txtDebit.Attributes.Add("onchange", "javascript:convertInRate('" + CType(txtCredit.ClientID, String) + "','" + CType(txtDebit.ClientID, String) + "','" + CType(txtConvRate.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "')")
        txtCredit.Attributes.Add("onchange", "javascript:convertInRate('" + CType(txtDebit.ClientID, String) + "','" + CType(txtCredit.ClientID, String) + "','" + CType(txtConvRate.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "')")

        NumbersDecimalRoundHtml(txtDebit)
        NumbersDecimalRoundHtml(txtCredit)

        If ViewState("OpeningSupplierBalanceOpenType") = "S" Then
            strtable = "partymast"
            strfiled = "Partycode"
        ElseIf ViewState("OpeningSupplierBalanceOpenType") = "C" Then
            strtable = "agentmast"
            strfiled = "agentcode"
        ElseIf ViewState("OpeningSupplierBalanceOpenType") = "A" Then
            strtable = "Supplier_agents"
            strfiled = "supagentcode"
        End If
        txtDate.Attributes.Add("onchange", "javascript:FillDueDate('" + CType(txtDate.ClientID, String) + "','" + CType(txtDueDate.ClientID, String) + "','" + strtable + "','" + strfiled + "')")


    End Sub
#End Region
#Region " Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        save_record()
    End Sub
#End Region
#Region "Public Sub save_record()"
    Public Sub save_record()
        Dim intRow As Integer = 0

        Dim sqlTrans As SqlTransaction
        Dim strdiv As String
        Dim strcontrolacct As String

        'Changes 12/11/2008******************
        Dim balancingaccount As String = ""
        balancingaccount = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=522"), String)
        If objUtils.GetDBFieldFromStringnewdiv(Session("dbconnectionName"), "acctmast", "acctcode", "acctcode", balancingaccount, "div_code", ViewState("divcode")) = Nothing Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Balancing account not defined in Accounts Master');", True)
            Exit Sub
        End If
        'Changes 12/11/2008*******************

        If objUtils.GetDBFieldFromStringnewdiv(Session("dbconnectionName"), "docgen_div", "optionname", "optionname", ViewState("OpeningSupplierBalanceTranType"), "div_id", ViewState("divcode")) = Nothing Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('user couldn’t  save record.');", True)
            Exit Sub
        End If

        strdiv = ViewState("divcode") 'objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
        strcontrolacct = objUtils.GetDBFieldFromMultipleCriterianewdiv(Session("dbconnectionName"), "view_account", "isnull(controlacctcode,11)as controlacctcode", " code= '" & txtsuppcode.Text & "'   and type='" & ViewState("OpeningSupplierBalanceOpenType") & "'", "div_code", ViewState("divcode"))


        Try
            If Page.IsValid = True Then
                If ViewState("OpeningSupplierBalanceState") = "New" Or ViewState("OpeningSupplierBalanceState") = "Edit" Then
                    If validate_page() = False Then
                        Exit Sub
                    End If
                End If
                If ViewState("OpeningSupplierBalanceState") = "Edit" Or ViewState("OpeningSupplierBalanceState") = "Delete" Then
                    If validate_BillAgainst() = False Then
                        Exit Sub
                    End If
                End If


                If ViewState("OpeningSupplierBalanceState") = "New" Or ViewState("OpeningSupplierBalanceState") = "Edit" Or ViewState("OpeningSupplierBalanceState") = "Delete" Then
                    If Validateseal() = False Then
                        Exit Sub
                    End If
                End If



                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = SqlConn.BeginTransaction           'SQL  Trans start

                If chkPost.Checked = True Then
                    'For Accounts posting
                    initialclass(SqlConn, sqlTrans)
                    'For Accounts posting
                End If
                If (ViewState("OpeningSupplierBalanceState") = "New" Or ViewState("OpeningSupplierBalanceState") = "Edit") Then
                    If ViewState("OpeningSupplierBalanceState") = "New" Then
                        'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                        Dim optionval As String

                        optionval = objUtils.GetAutoDocNodiv(ViewState("OpeningSupplierBalanceTranType").ToString, SqlConn, sqlTrans, ViewState("divcode"))
                        txtDocNo.Value = optionval.Trim
                        myCommand = New SqlCommand("sp_add_openparty_master", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    ElseIf ViewState("OpeningSupplierBalanceState") = "Edit" Then
                        myCommand = New SqlCommand("sp_mod_openparty_master", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@adddate ", SqlDbType.DateTime)).Value = DBNull.Value
                    End If


                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = ViewState("OpeningSupplierBalanceTranType").ToString
                    myCommand.Parameters.Add(New SqlParameter("@tran_date ", SqlDbType.DateTime)).Value = objDateTime.ConvertDateromTextBoxToDatabase(txtDocDate.Text)
                    myCommand.Parameters.Add(New SqlParameter("@open_type", SqlDbType.VarChar, 1)).Value = ViewState("OpeningSupplierBalanceOpenType")

                    myCommand.Parameters.Add(New SqlParameter("@open_code", SqlDbType.VarChar, 20)).Value = txtsuppcode.Text
                    myCommand.Parameters.Add(New SqlParameter("@controlacctcode ", SqlDbType.VarChar, 20)).Value = strcontrolacct
                    myCommand.Parameters.Add(New SqlParameter("@open_narration", SqlDbType.VarChar, 200)).Value = "Balance-B/F"
                    myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = TxtCurrCode.Value.ToString
                    myCommand.Parameters.Add(New SqlParameter("@currrate", SqlDbType.Decimal, 18, 12)).Value = CType(txtConvRate.Value, Double)

                    myCommand.Parameters.Add(New SqlParameter("@open_debit", SqlDbType.Money)).Value = CType(Val(txtTotDebit.Value), Double)
                    myCommand.Parameters.Add(New SqlParameter("@open_credit", SqlDbType.Money)).Value = CType(Val(txtTotCredit.Value), Double)
                    myCommand.Parameters.Add(New SqlParameter("@openbase_debit", SqlDbType.Money)).Value = CType(txtTotBaseDebit.Value, Double)
                    myCommand.Parameters.Add(New SqlParameter("@openbase_Credit", SqlDbType.Money)).Value = CType(Val(txtTotBaseCredit.Value), Double)

                    myCommand.Parameters.Add(New SqlParameter("@open_group", SqlDbType.VarChar, 1)).Value = ""
                    myCommand.Parameters.Add(New SqlParameter("@open_tran_state", SqlDbType.VarChar, 1)).Value = ""

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
                        cacc.acc_tran_type = ViewState("OpeningSupplierBalanceTranType").ToString
                        cacc.acc_tran_date = objDateTime.ConvertDateromTextBoxToDatabase(txtDocDate.Text)
                        cacc.acc_div_id = strdiv

                        'Posting for the Balancing account
                        ctran = New clstran
                        ctran.acc_tran_id = cacc.acc_tran_id
                        'Changes 12/11/2008******************
                        'ctran.acc_code = "3-02-01-02"
                        ctran.acc_code = balancingaccount
                        'Changes 12/11/2008******************
                        ctran.acc_type = "G"
                        ctran.acc_currency_id = mbasecurrency
                        ctran.acc_currency_rate = 1
                        ctran.acc_div_id = strdiv
                        ctran.acc_narration = "BALANCE B/F"
                        ctran.acc_tran_date = cacc.acc_tran_date
                        ctran.acc_tran_lineno = 1
                        ctran.acc_tran_type = cacc.acc_tran_type
                        ctran.pacc_gl_code = ""
                        ctran.acc_ref1 = ""
                        ctran.acc_ref2 = ""
                        ctran.acc_ref3 = ""
                        ctran.acc_ref4 = ""
                        cacc.addtran(ctran)

                        csubtran = New clsSubTran
                        csubtran.acc_against_tran_id = cacc.acc_tran_id
                        csubtran.acc_against_tran_lineno = 1
                        csubtran.acc_against_tran_type = cacc.acc_tran_type
                        csubtran.acc_debit = IIf(CType(txtNetBalBase.Value, Double) > 0, CType(txtNetBalBase.Value, Double), 0)
                        csubtran.acc_credit = IIf(CType(txtNetBalBase.Value, Double) < 0, Math.Abs(CType(txtNetBalBase.Value, Double)), 0)
                        csubtran.acc_tran_date = cacc.acc_tran_date
                        csubtran.acc_due_date = cacc.acc_tran_date
                        csubtran.acc_field1 = ""
                        csubtran.acc_field2 = ""
                        csubtran.acc_field3 = ""
                        csubtran.acc_field4 = ""
                        csubtran.acc_field5 = ""
                        csubtran.acc_tran_id = cacc.acc_tran_id
                        csubtran.acc_tran_lineno = 1
                        csubtran.acc_tran_type = cacc.acc_tran_type
                        csubtran.acc_narration = "BALANCE B/F"
                        csubtran.acc_type = "G"
                        csubtran.currate = 1
                        csubtran.acc_base_debit = IIf(CType(txtNetBalBase.Value, Double) > 0, CType(txtNetBalBase.Value, Double), 0)
                        csubtran.acc_base_credit = IIf(CType(txtNetBalBase.Value, Double) < 0, Math.Abs(CType(txtNetBalBase.Value, Double)), 0)
                        csubtran.costcentercode = ""
                        cacc.addsubtran(csubtran)


                        'Posting only for the Supplier/Customer/Supplier Agent account
                        ctran = New clstran
                        ctran.acc_tran_id = cacc.acc_tran_id
                        ctran.acc_code = txtsuppcode.Text
                        ctran.acc_type = ViewState("OpeningSupplierBalanceOpenType")
                        ctran.acc_currency_id = TxtCurrCode.Value.ToString
                        ctran.acc_currency_rate = CType(txtConvRate.Value, Double)
                        ctran.acc_div_id = strdiv
                        ctran.acc_narration = "BALANCE B/F"
                        ctran.acc_tran_date = cacc.acc_tran_date
                        ctran.acc_tran_lineno = 2
                        ctran.acc_tran_type = cacc.acc_tran_type
                        ctran.pacc_gl_code = strcontrolacct
                        ctran.acc_ref1 = ""
                        ctran.acc_ref2 = ""
                        ctran.acc_ref3 = ""
                        ctran.acc_ref4 = ""
                        cacc.addtran(ctran)
                        'For Accounts Posting
                    End If

                    If ViewState("OpeningSupplierBalanceState") = "Edit" Then
                        If chkPost.Checked = False Then
                            myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure
                            myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "openparty_master"
                            myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                            myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = ViewState("OpeningSupplierBalanceTranType").ToString
                            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                            myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                            myCommand.ExecuteNonQuery()
                        End If

                        myCommand = New SqlCommand("sp_del_open_party_detail", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                        myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = ViewState("OpeningSupplierBalanceTranType").ToString
                        myCommand.ExecuteNonQuery()
                    End If


                    'Save In Detail Table
                    For Each gvRow In grdRecord.Rows

                        lblLineNo = gvRow.FindControl("lblLineNo")
                        If Val(lblLineNo.Text) = 0 Then
                            lblLineNo.Text = intRow
                        End If

                        txtDate = gvRow.FindControl("txtDate")
                        txtBillType = gvRow.FindControl("txtBillType")
                        txtDueDate = gvRow.FindControl("txtDueDate")
                        txtDoNo = gvRow.FindControl("txtDocNo")
                        txtotherRef = gvRow.FindControl("txtOtherRef")

                        txtDr = gvRow.FindControl("txtDebit")
                        txtCr = gvRow.FindControl("txtCredit")

                        '30112014
                        txtfield1 = gvRow.FindControl("txtfield1")
                        txtfield2 = gvRow.FindControl("txtfield2")
                        txtfield3 = gvRow.FindControl("txtfield3")


                        txtBaseDr = gvRow.FindControl("txtBaseDebit")
                        txtBaseCr = gvRow.FindControl("txtbaseCredit")

                        If txtDate.Text.Trim <> "" Then
                            intRow = 1 + intRow
                            myCommand = New SqlCommand("sp_add_open_party_detail", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure
                            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 10)).Value = txtDocNo.Value
                            myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int)).Value = intRow
                            myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = ViewState("OpeningSupplierBalanceTranType").ToString
                            myCommand.Parameters.Add(New SqlParameter("@tran_date ", SqlDbType.DateTime)).Value = Format(CType(txtDocDate.Text, Date), "yyyy/MM/dd")


                            myCommand.Parameters.Add(New SqlParameter("@open_narration", SqlDbType.VarChar, 100)).Value = "Balance B/F"
                            myCommand.Parameters.Add(New SqlParameter("@against_tran_date", SqlDbType.DateTime)).Value = Format(CType(txtDate.Text, Date), "yyyy/MM/dd")

                            myCommand.Parameters.Add(New SqlParameter("@against_bill_type", SqlDbType.VarChar, 10)).Value = txtBillType.Value
                            myCommand.Parameters.Add(New SqlParameter("@open_due_date", SqlDbType.DateTime)).Value = Format(CType(txtDueDate.Text, Date), "yyyy/MM/dd")
                            myCommand.Parameters.Add(New SqlParameter("@against_tran_id", SqlDbType.VarChar, 20)).Value = txtDoNo.Value.Trim

                            myCommand.Parameters.Add(New SqlParameter("@other_reference", SqlDbType.VarChar, 200)).Value = txtotherRef.Value.Trim

                            myCommand.Parameters.Add(New SqlParameter("@open_debit", SqlDbType.Money)).Value = CType(Val(txtDr.Value), Double)
                            myCommand.Parameters.Add(New SqlParameter("@open_credit", SqlDbType.Money)).Value = CType(Val(txtCr.Value), Double)
                            myCommand.Parameters.Add(New SqlParameter("@open_base_debit", SqlDbType.Money)).Value = CType(Val(txtBaseDr.Value), Double)
                            myCommand.Parameters.Add(New SqlParameter("@open_base_credit", SqlDbType.Money)).Value = CType(Val(txtBaseCr.Value), Double)

                            myCommand.Parameters.Add(New SqlParameter("@open_mode", SqlDbType.VarChar, 1)).Value = DBNull.Value
                            myCommand.Parameters.Add(New SqlParameter("@open_Exchg_diff", SqlDbType.Money)).Value = DBNull.Value

                            myCommand.Parameters.Add(New SqlParameter("@field1", SqlDbType.VarChar, 100)).Value = txtfield1.Value.Trim
                            myCommand.Parameters.Add(New SqlParameter("@field2", SqlDbType.VarChar, 100)).Value = txtfield2.Value.Trim
                            myCommand.Parameters.Add(New SqlParameter("@field3", SqlDbType.VarChar, 100)).Value = txtfield3.Value.Trim

                            myCommand.ExecuteNonQuery()
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
                ElseIf ViewState("OpeningSupplierBalanceState") = "Delete" Then
                    myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "openparty_master"
                    myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = ViewState("OpeningSupplierBalanceTranType").ToString
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()

                    myCommand = New SqlCommand("sp_del_open_party_master", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = ViewState("OpeningSupplierBalanceTranType").ToString
                    myCommand.ExecuteNonQuery()

                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                clsDBConnect.dbConnectionClose(SqlConn)           'connection close
                'Response.Redirect("OpeningSupplierBalanceSearch.aspx?tran_type=" & ViewState("OpeningSupplierBalanceOpenType"), False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('OpeningSupplierBalanceWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)


            End If

        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OpeningSupplierBalance.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub
#End Region
    Public Function Validateseal() As Boolean
        Try

            Validateseal = True
            Dim invdate As DateTime
            Dim sealdate As DateTime
            Dim MyCultureInfo As New CultureInfo("fr-Fr")
            invdate = DateTime.Parse(txtDocDate.Text, MyCultureInfo, DateTimeStyles.None)
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
        Dim Strrs As String = ""
        Dim strdis As String
        If txtsuppcode.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select " & lblcode.Text & ".');", True)
            SetFocus(txtsuppname)
            validate_page = False
            Exit Function
        End If
        If TxtCurrCode.Value.Trim = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select currency code.');", True)
            SetFocus(TxtCurrCode)
            validate_page = False
            Exit Function
        End If
        If txtConvRate.Value <> "" Then
            If CType(Val(txtConvRate.Value), Double) = 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid conversion rate.');", True)
                SetFocus(txtConvRate)
                validate_page = False
                Exit Function
            End If
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter valid conversion rate.');", True)
            SetFocus(txtConvRate)
            validate_page = False
            Exit Function
        End If
        If ViewState("OpeningSupplierBalanceState") = "New" Then
            strSqlQry = "select 't' from openparty_master where div_id='" & ViewState("divcode") & "' and  open_type='" & ViewState("OpeningSupplierBalanceOpenType") & "'" & _
                            "and tran_type='" & ViewState("OpeningSupplierBalanceTranType") & "' and open_code='" & txtsuppcode.Text & "' "

            Strrs = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strSqlQry)
            strdis = ""
            If Strrs = "t" Then
                If ViewState("OpeningSupplierBalanceOpenType") = "S" Then
                    strdis = "This Supplier Opening Balance already exist "
                ElseIf ViewState("OpeningSupplierBalanceOpenType") = "C" Then
                    strdis = "This Customer Opening Balancealready exist"
                ElseIf ViewState("OpeningSupplierBalanceOpenType") = "A" Then
                    strdis = "This Supplier Agent Opening Balance already exist"
                End If
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & strdis & "');", True)
                SetFocus(txtsuppname)
                validate_page = False
                Exit Function
            End If
        End If

        Dim blFlag As Boolean = False
        For Each gvRow In grdRecord.Rows
            lblLineNo = gvRow.FindControl("lblLineNo")
            txtDate = gvRow.FindControl("txtDate")
            txtBillType = gvRow.FindControl("txtBillType")
            txtDueDate = gvRow.FindControl("txtDueDate")
            txtDoNo = gvRow.FindControl("txtDocNo")
            txtotherRef = gvRow.FindControl("txtOtherRef")
            txtDr = gvRow.FindControl("txtDebit")
            txtCr = gvRow.FindControl("txtCredit")
            txtBaseDr = gvRow.FindControl("txtBaseDebit")
            txtBaseCr = gvRow.FindControl("txtbaseCredit")

            If txtDate.Text.Trim <> "" Or txtDoNo.Value.Trim <> "" Or txtDueDate.Text.Trim <> "" Or txtBillType.Value <> "" Or txtDr.Value <> "" Or txtCr.Value <> "" Then
                blFlag = True
                If txtDate.Text.Trim = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter date.');", True)
                    SetFocus(txtDate)
                    validate_page = False
                    Exit Function
                End If
                If txtBillType.Value = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter bill type.');", True)
                    SetFocus(txtBillType)
                    validate_page = False
                    Exit Function
                End If
                If txtDueDate.Text.Trim = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter due date.');", True)
                    SetFocus(txtDueDate)
                    validate_page = False
                    Exit Function
                End If
                If txtDoNo.Value = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter document no.');", True)
                    SetFocus(txtDoNo)
                    validate_page = False
                    Exit Function
                End If

                If Val(txtDr.Value) <= 0 And Val(txtCr.Value) <= 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter debit or credit amount.');", True)
                    SetFocus(txtDr)
                    validate_page = False
                    Exit Function
                End If
                Dim dFromDate, dToDate As Date
                dFromDate = objDateTime.ConvertDateromTextBoxToDatabase(txtDate.Text)
                dToDate = objDateTime.ConvertDateromTextBoxToDatabase(txtDueDate.Text)
                If dFromDate <= dToDate Then

                Else
                    validate_page = False
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select due dates should be greater than date.');", True)
                    SetFocus(dToDate)
                    Exit Function
                End If

                Dim Alflg As Integer
                Dim ErrMsg, strdiv As String
                strdiv = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                myCommand = New SqlCommand("sp_Check_Duplicate_refnos", SqlConn)
                myCommand.CommandType = CommandType.StoredProcedure
                myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDoNo.Value
                myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = txtBillType.Value
                myCommand.Parameters.Add(New SqlParameter("@tran_lineno ", SqlDbType.Int)).Value = 2
                myCommand.Parameters.Add(New SqlParameter("@acc_type ", SqlDbType.VarChar, 10)).Value = ViewState("OpeningSupplierBalanceOpenType").ToString
                myCommand.Parameters.Add(New SqlParameter("@acc_code ", SqlDbType.VarChar, 10)).Value = txtsuppcode.Text

                Dim nparam1 As SqlParameter
                Dim nparam2 As SqlParameter
                nparam1 = New SqlParameter
                nparam1.ParameterName = "@allowflg"
                nparam1.Direction = ParameterDirection.Output
                nparam1.DbType = DbType.Int16
                nparam1.Size = 9
                myCommand.Parameters.Add(nparam1)
                nparam2 = New SqlParameter
                nparam2.ParameterName = "@errmsg"
                nparam2.Direction = ParameterDirection.Output
                nparam2.DbType = DbType.String
                nparam2.Size = 200
                myCommand.Parameters.Add(nparam2)
                myDataAdapter = New SqlDataAdapter(myCommand)
                myCommand.ExecuteNonQuery()

                Alflg = nparam1.Value
                ErrMsg = nparam2.Value

                If Alflg = 1 And ErrMsg <> "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ErrMsg & "');", True)
                    validate_page = False
                    Exit Function
                End If


            End If

        Next
        If blFlag = False Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter more than one record .');", True)
            validate_page = False
            Exit Function
        End If



    End Function
#End Region
#Region "Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click"
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        'Dim strscript As String = ""
        'strscript = "window.opener.__doPostBack('OpeningSupplierBalanceWindowPostBack', '');window.opener.focus();window.close();"
        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        'Response.Redirect("OpeningSupplierBalanceSearch.aspx?tran_type=" & ViewState("OpeningSupplierBalanceOpenType"), False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)

    End Sub
#End Region
#Region "Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click"
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim n As Integer = 0
        Dim count As Integer
        count = grdRecord.Rows.Count + 1
        Dim lineno(count) As String
        Dim docdate(count) As String
        Dim billtype(count) As String
        Dim dudate(count) As String
        Dim DocNo(count) As String
        Dim OtherRef(count) As String
        Dim debit(count) As String
        Dim credit(count) As String
        Dim bdebit(count) As String
        Dim bcredit(count) As String


        For Each gvRow In grdRecord.Rows
            lblLineNo = gvRow.FindControl("lblLineNo")
            txtDate = gvRow.FindControl("txtDate")
            txtBillType = gvRow.FindControl("txtBillType")
            txtDueDate = gvRow.FindControl("txtDueDate")
            txtDoNo = gvRow.FindControl("txtDocNo")
            txtotherRef = gvRow.FindControl("txtOtherRef")
            txtDr = gvRow.FindControl("txtDebit")
            txtCr = gvRow.FindControl("txtCredit")
            txtBaseDr = gvRow.FindControl("txtBaseDebit")
            txtBaseCr = gvRow.FindControl("txtbaseCredit")

            lineno(n) = lblLineNo.Text
            docdate(n) = txtDate.Text
            billtype(n) = txtBillType.Value
            dudate(n) = txtDueDate.Text
            DocNo(n) = txtDoNo.Value
            OtherRef(n) = txtotherRef.Value
            debit(n) = txtDr.Value
            credit(n) = txtCr.Value
            bdebit(n) = txtBaseDr.Value
            bcredit(n) = txtBaseCr.Value

            n = n + 1
        Next

        fillDategrd(grdRecord, False, grdRecord.Rows.Count + 1)
        Dim i As Integer = n
        n = 0

        For Each gvRow In grdRecord.Rows
            If n = i Then
                Exit For
            End If

            lblLineNo = gvRow.FindControl("lblLineNo")
            txtDate = gvRow.FindControl("txtDate")
            txtBillType = gvRow.FindControl("txtBillType")
            txtDueDate = gvRow.FindControl("txtDueDate")
            txtDoNo = gvRow.FindControl("txtDocNo")
            txtotherRef = gvRow.FindControl("txtOtherRef")
            txtDr = gvRow.FindControl("txtDebit")
            txtCr = gvRow.FindControl("txtCredit")
            txtBaseDr = gvRow.FindControl("txtBaseDebit")
            txtBaseCr = gvRow.FindControl("txtbaseCredit")

            lblLineNo.Text = lineno(n)
            txtDate.Text = docdate(n)
            txtBillType.Value = billtype(n)
            txtDueDate.Text = dudate(n)
            txtDoNo.Value = DocNo(n)
            txtotherRef.Value = OtherRef(n)
            txtDr.Value = debit(n)
            txtCr.Value = credit(n)
            txtBaseDr.Value = bdebit(n)
            txtBaseCr.Value = bcredit(n)

            n = n + 1
        Next

    End Sub
#End Region
#Region "Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click"
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim n As Integer = 0
        Dim count As Integer
        count = grdRecord.Rows.Count
        Dim lineno(count) As String
        Dim docdate(count) As String
        Dim billtype(count) As String
        Dim dudate(count) As String
        Dim DocNo(count) As String
        Dim OtherRef(count) As String
        Dim debit(count) As String
        Dim credit(count) As String
        Dim bdebit(count) As String
        Dim bcredit(count) As String

        '30112014
        Dim field1(count) As String
        Dim field2(count) As String
        Dim field3(count) As String

        Dim d As Double

        For Each gvRow In grdRecord.Rows

            chkDel = gvRow.FindControl("chkDel")
            If chkDel.Checked = False Then

                lblLineNo = gvRow.FindControl("lblLineNo")
                txtDate = gvRow.FindControl("txtDate")
                txtBillType = gvRow.FindControl("txtBillType")
                txtDueDate = gvRow.FindControl("txtDueDate")
                txtDoNo = gvRow.FindControl("txtDocNo")
                txtotherRef = gvRow.FindControl("txtOtherRef")
                txtDr = gvRow.FindControl("txtDebit")
                txtCr = gvRow.FindControl("txtCredit")
                txtBaseDr = gvRow.FindControl("txtBaseDebit")
                txtBaseCr = gvRow.FindControl("txtbaseCredit")

                '30112014
                txtfield1 = gvRow.FindControl("txtfield1")
                txtfield2 = gvRow.FindControl("txtfield2")
                txtfield3 = gvRow.FindControl("txtfield3")

                lineno(n) = lblLineNo.Text
                docdate(n) = txtDate.Text
                billtype(n) = txtBillType.Value
                dudate(n) = txtDueDate.Text
                DocNo(n) = txtDoNo.Value
                OtherRef(n) = txtotherRef.Value
                debit(n) = txtDr.Value
                credit(n) = txtCr.Value
                bdebit(n) = txtBaseDr.Value
                bcredit(n) = txtBaseCr.Value
                field1(n) = txtfield1.Value
                field2(n) = txtfield2.Value
                field3(n) = txtfield3.Value

                n = n + 1
            End If
        Next
        Dim ct As Integer
        ct = n
        If n = 0 Then
            ct = 0
        End If

        fillDategrd(grdRecord, False, ct)
        Dim i As Integer = n
        n = 0

        txtTotCredit.Value = 0
        txtTotDebit.Value = 0
        txtTotBaseCredit.Value = 0
        txtTotBaseDebit.Value = 0
        txtNetBalBase.Value = 0
        For Each gvRow In grdRecord.Rows
            If n = i Then
                Exit For
            End If

            lblLineNo = gvRow.FindControl("lblLineNo")
            txtDate = gvRow.FindControl("txtDate")
            txtBillType = gvRow.FindControl("txtBillType")
            txtDueDate = gvRow.FindControl("txtDueDate")
            txtDoNo = gvRow.FindControl("txtDocNo")
            txtotherRef = gvRow.FindControl("txtOtherRef")
            txtDr = gvRow.FindControl("txtDebit")
            txtCr = gvRow.FindControl("txtCredit")
            txtBaseDr = gvRow.FindControl("txtBaseDebit")
            txtBaseCr = gvRow.FindControl("txtbaseCredit")

            '30112014
            txtfield1 = gvRow.FindControl("txtfield1")
            txtfield2 = gvRow.FindControl("txtfield2")
            txtfield3 = gvRow.FindControl("txtfield3")

            lblLineNo.Text = lineno(n)
            txtDate.Text = docdate(n)
            txtBillType.Value = billtype(n)
            txtDueDate.Text = dudate(n)
            txtDoNo.Value = DocNo(n)
            txtotherRef.Value = OtherRef(n)
            txtDr.Value = debit(n)
            txtCr.Value = credit(n)
            txtBaseDr.Value = bdebit(n)
            txtBaseCr.Value = bcredit(n)

            '30112014

            txtfield1.Value = field1(n)
            txtfield2.Value = field2(n)
            txtfield3.Value = field3(n)

            d = CType(Val(txtTotCredit.Value), Double) + CType(Val(txtCr.Value), Double)
            txtTotCredit.Value = d.ToString("0.00")
            d = CType(Val(txtTotDebit.Value), Double) + CType(Val(txtDr.Value), Double)
            txtTotDebit.Value = d.ToString("0.00")

            d = CType(Val(txtTotBaseCredit.Value), Double) + CType(Val(txtBaseCr.Value), Double)
            txtTotBaseCredit.Value = d.ToString("0.00")
            d = CType(Val(txtTotBaseDebit.Value), Double) + CType(Val(txtBaseDr.Value), Double)
            txtTotBaseDebit.Value = d.ToString("0.00")


            n = n + 1
        Next

        d = CType(Val(txtTotCredit.Value), Double) - CType(Val(txtTotDebit.Value), Double)
        txtNetBal.Value = d.ToString("0.00")
        d = CType(Val(txtTotBaseCredit.Value), Double) - CType(Val(txtTotBaseDebit.Value), Double)
        txtNetBalBase.Value = d.ToString("0.00")
    End Sub
#End Region

    Private Sub DisableAllControls()
        Dim ImgBtnDt, ImgBtnDueDate As ImageButton
        If ViewState("OpeningSupplierBalanceState") = "New" Then

        ElseIf ViewState("OpeningSupplierBalanceState") = "Edit" Then
            chkPost.Visible = True
        ElseIf ViewState("OpeningSupplierBalanceState") = "View" Or ViewState("OpeningSupplierBalanceState") = "Delete" Then
            txtDocNo.Disabled = True
            txtConvRate.Disabled = True
            TxtCurrCode.Disabled = True
            btnSave.Visible = False
            btnAdd.Visible = False
            btnDelete.Visible = False
            chkPost.Visible = False
            txtDocDate.Enabled = False
            txtTotDebit.Disabled = True

            txtTotCredit.Disabled = True

            txtTotBaseDebit.Disabled = True

            txtTotBaseCredit.Disabled = True

            txtNetBal.Disabled = True

            txtNetBalBase.Disabled = True




            For Each gvRow In grdRecord.Rows
                txtDate = gvRow.FindControl("txtDate")
                txtDate.Enabled = False
                ImgBtnDt = gvRow.FindControl("ImgBtnDt")
                ImgBtnDt.Enabled = False
                txtBillType = gvRow.FindControl("txtBillType")
                txtBillType.Disabled = True
                txtDueDate = gvRow.FindControl("txtDueDate")
                txtDueDate.Enabled = False
                ImgBtnDueDate = gvRow.FindControl("ImgBtnDueDate")
                ImgBtnDueDate.Enabled = False
                '30112014
                txtfield1 = gvRow.FindControl("txtfield1")
                txtfield2 = gvRow.FindControl("txtfield2")
                txtfield3 = gvRow.FindControl("txtfield3")
                txtfield1.Disabled = True
                txtfield2.Disabled = True
                txtfield3.Disabled = True


                txtDoNo = gvRow.FindControl("txtDocNo")
                txtDoNo.Disabled = True
                txtotherRef = gvRow.FindControl("txtOtherRef")
                txtotherRef.Disabled = True
                txtDr = gvRow.FindControl("txtDebit")
                txtDr.Disabled = True
                txtCr = gvRow.FindControl("txtCredit")
                txtCr.Disabled = True
                txtBaseDr = gvRow.FindControl("txtBaseDebit")
                txtBaseDr.Disabled = True
                txtBaseCr = gvRow.FindControl("txtbaseCredit")
                txtBaseCr.Disabled = True
                chkDel = gvRow.FindControl("chkDel")
                chkDel.Enabled = False
            Next
        End If
        If ViewState("OpeningSupplierBalanceState") = "Delete" Then
            btnSave.Visible = True
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
        cacc.tran_mode = IIf(ViewState("OpeningSupplierBalanceState") = "New", 1, 2)
        mbasecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
        cacc.start()

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OpeningSupplierBalance','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
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
            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = txtDocNo.Value
            myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = ViewState("OpeningSupplierBalanceTranType").ToString

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
            If ViewState("OpeningSupplierBalanceState") = "Edit" Then
                If chkPost.Checked = True Then
                    ViewState.Add("OpeningSupplierBalanceState", "View")
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This transaction has been posted, you do not have rights to edit.' );", True)
                End If
            End If
        End If
    End Sub
End Class


'objDateTime.ConvertDateromTextBoxToDatabase(txtDocDate.Value)
'objDateTime.ConvertDateromTextBoxToDatabase(dtDueDate.txtDate.Text)
'objDateTime.ConvertDateromTextBoxToDatabase(dtDate.txtDate.Text)
'CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("frmdate")), String)
'CType(ObjDate.ConvertDateFromDatabaseToTextBoxFormat(mySqlReader("todate")), String)
' objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSupplierCode, "Partycode", "Partyname", "select Partyname,Partycode from  partymast where active=1", True)
'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSuppilerName, "Partyname", "Partycode", "select Partyname,Partycode from  partymast where active=1", True)
'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSupplierCode, "agentcode", "agentname", "select agentname,agentcode from  agentmast where active=1", True)
'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSuppilerName, "agentname", "agentcode", "select agentname,agentcode from  agentmast where active=1", True)

'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSupplierCode, "supagentcode", "supagentname", "select supagentname,supagentcode from  Supplier_agents where active=1", True)
'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"),ddlSuppilerName, "supagentname", "supagentcode", "select supagentname,supagentcode from  Supplier_agents where active=1", True)
