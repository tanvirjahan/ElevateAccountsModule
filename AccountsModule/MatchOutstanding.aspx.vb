'------------================--------------=======================------------------================
'   Module Name    :    MatchOutstanding.aspx
'   Developer Name :    Govardhan
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

Partial Class MatchOutstanding
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim ObjDate As New clsDateTime
    Dim strSqlQry As String
    Dim strWhereCond As String
    Dim SqlConn As SqlConnection
    Dim myCommand As SqlCommand
    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim sqlTrans As SqlTransaction
    Dim gvRow As GridViewRow
    'For accounts posting
    Dim caccounts As clssave = Nothing
    Dim cacc As clsAccounts = Nothing
    Dim ctran As clstran = Nothing
    Dim csubtran As clsSubTran = Nothing
    Dim mbasecurrency As String = ""
    Dim objdatetime As New clsDateTime
    Dim type As String

    Dim ScriptOpenModalDialog As String = "OpenModalDialog('{0}','{1}');"
    'For accounts posting
    Enum grd_col
        Doctype = 0
        docno_new = 1
        Docno = 2
        filenumber = 3
        Docdate = 4
        Duedate = 5
        balanceamt = 6
        currate = 7
        Debit = 8
        Credit = 9
        chselection = 10
        Particulars = 11
        BaseDebit = 12
        BaseCredit = 13
        LineNo = 14
        LineId = 15


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
    Public Sub NumbersDecimal(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event,this)")
    End Sub
    Public Sub NumbersDecimalHtml(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event,this)")
    End Sub

    'checkNumberDecimal
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
    <System.Web.Script.Services.ScriptMethod()> _
       <System.Web.Services.WebMethod()> _
    Public Shared Function Getcustlist(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Custnames As New List(Of String)
        Try

           
            strSqlQry = "select agentcode,agentname from agentmast where active=1 and  agentname like  '" & Trim(prefixText) & "%' and divcode in (" & HttpContext.Current.Session("div_code") & ") order by agentname"


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
            
            strSqlQry = "select partycode,partyname from partymast where active=1 and  partyname like  '" & Trim(prefixText) & "%' order by partyname "


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
    Public Shared Function Getexchglist(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim suppnames As New List(Of String)
        Try

            strSqlQry = "select code, des from view_account where div_code='" & HttpContext.Current.Session("div_code") & "' and type = 'G'  and  des like  '" & Trim(prefixText) & "%' order by des"


            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    suppnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("des").ToString(), myDS.Tables(0).Rows(i)("code").ToString()))

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
           
            strSqlQry = "select supagentcode,supagentname from Supplier_agents  where active=1 and  supagentname like  '" & Trim(prefixText) & "%' order by supagentname"


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
    Private Sub createjournaldatatable()
        Dim dataTable As New DataTable()
        dataTable.Columns.Add("TRANID", GetType(String))
        dataTable.Columns.Add("tran_type", GetType(String))
        dataTable.Columns.Add("tran_date", GetType(String))
        dataTable.Columns.Add("tran_lineno", GetType(Integer))
        dataTable.Columns.Add("against_tran_id", GetType(String))
        dataTable.Columns.Add("against_tran_lineno", GetType(Integer))
        dataTable.Columns.Add("against_tran_type", GetType(String))
        dataTable.Columns.Add("against_tran_date", GetType(String))
        dataTable.Columns.Add("open_due_date", GetType(String))
        dataTable.Columns.Add("open_debit", GetType(Decimal))
        dataTable.Columns.Add("open_credit", GetType(Decimal))
        ' dataTable.Columns.Add("open_due_date", GetType(String))
        dataTable.Columns.Add("open_field1", GetType(String))
        dataTable.Columns.Add("open_field2", GetType(String))
        dataTable.Columns.Add("open_field3", GetType(String))
        dataTable.Columns.Add("open_field4", GetType(String))
        dataTable.Columns.Add("open_field5", GetType(String))
        dataTable.Columns.Add("open_mode", GetType(String))
        dataTable.Columns.Add("currency_rate", GetType(Decimal))
        dataTable.Columns.Add("div_id", GetType(String))
        dataTable.Columns.Add("base_debit", GetType(Decimal))
        dataTable.Columns.Add("base_CREDIT", GetType(Decimal))
        dataTable.Columns.Add("acc_type", GetType(String))
        dataTable.Columns.Add("acc_code", GetType(String))
        dataTable.Columns.Add("acc_gl_code", GetType(String))
        dataTable.Columns.Add("AgainstTranLineNo", GetType(Integer))
        dataTable.Columns.Add("currcode", GetType(String))
        Session("Adjustedrecords_mos") = dataTable
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState.Add("MatchOutStandingState", Request.QueryString("State"))
        ViewState.Add("MatchOutStandingRefCode", Request.QueryString("RefCode"))
        ViewState.Add("TranType", "MOS")
        ViewState.Add("divcode", Request.QueryString("divid"))

        If IsPostBack = False Then
            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If

            txtconnection.Value = Session("dbconnectionName")

            If CType(Session("GlobalUserName"), String) = "" Then
                Response.Redirect("~/Login.aspx", False)
                Exit Sub
            End If
            'Tanvir 24102023
            createjournaldatatable()
            Session("freeform_Detail_mos") = Nothing
            Session("Adjustedrecords_mos") = Nothing
            'Tanvir 24102023
            Try
                imgicon.Style("visibility") = "hidden"
               
                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
                txtDivCode.Value = ViewState("divcode") ' objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
                txtbasecurr.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 457)
                Session("div_code") = ViewState("divcode")

                Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select top 1  sealdate from  sealing_master where div_code='" & ViewState("divcode") & "'")
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("sealdate")) = False Then
                            txtpdate.Text = CType(ds.Tables(0).Rows(0)("sealdate"), String)
                        End If
                    Else
                        txtpdate.Text = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 508)
                    End If
                End If
                txtDate.Text = Format(objdatetime.GetSystemDateTime(Session("dbconnectionName")), "dd/MM/yyyy")
                txtPfromdate.Text = Format(objdatetime.GetSystemDateTime(Session("dbconnectionName")), "dd/MM/yyyy")
                txtPtodate.Text = Format(objdatetime.GetSystemDateTime(Session("dbconnectionName")), "dd/MM/yyyy")


                'Added check_Privilege() and chkpost enabled by Archana on 01/04/2015
                If check_Privilege() = 1 Then

                    chkPost.Enabled = True
                    chkPost.Checked = True
                Else

                    chkPost.Enabled = False
                    chkPost.Checked = True
                End If



                If ViewState("MatchOutStandingState") = "New" Then
                    SetFocus(txtDate)
                    lblHeading.Text = "Add New Match Outstanding"
                    btnSave.Text = "Save"
                     'FillGrids()
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    btnDisplay.Attributes.Add("onclick", "return FormValidation('View')")
                    btnclientprint.Visible = False
                ElseIf ViewState("MatchOutStandingState") = "Copy" Then
                    SetFocus(txtDate)
                    lblHeading.Text = "Copy Match Outstanding"
                    btnSave.Text = "Save"
                    ShowRecord(CType(ViewState("MatchOutStandingRefCode"), String))
                    ShowGridData()
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    btnDisplay.Attributes.Add("onclick", "return FormValidation('View')")
                    btnclientprint.Visible = False
                ElseIf ViewState("MatchOutStandingState") = "Edit" Then
                    SetFocus(txtDate)
                    lblHeading.Text = "Edit Match Outstanding"
                    btnSave.Text = "Update"
                    ShowRecord(CType(ViewState("MatchOutStandingRefCode"), String))
                    ShowGridData()
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                    btnDisplay.Attributes.Add("onclick", "return FormValidation('View')")
                    btnclientprint.Visible = False
                ElseIf ViewState("MatchOutStandingState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Match Outstanding"
                    btnSave.Visible = False
                    ShowRecord(CType(ViewState("MatchOutStandingRefCode"), String))
                    ShowGridData()
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                    btnclientprint.Visible = True
                ElseIf ViewState("MatchOutStandingState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Match Outstanding"
                    btnSave.Text = "Delete"
                    ShowRecord(CType(ViewState("MatchOutStandingRefCode"), String))
                    ShowGridData()
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnclientprint.Visible = False
                ElseIf ViewState("MatchOutStandingState") = "Cancel" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Cancel Match Outstanding"
                    btnSave.Text = "Cancel"
                    ShowRecord(CType(ViewState("MatchOutStandingRefCode"), String))
                    ShowGridData()
                    btnSave.Attributes.Add("onclick", "return FormValidation('Cancel')")
                    btnclientprint.Visible = False
                ElseIf ViewState("MatchOutStandingState") = "UndoCancel" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Undocancel Match Outstanding"
                    btnSave.Text = "Undo"
                    ShowRecord(CType(ViewState("MatchOutStandingRefCode"), String))
                    ShowGridData()
                    btnSave.Attributes.Add("onclick", "return FormValidation('Cancel')")
                    btnclientprint.Visible = False

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

                CheckPostUnpostRight(CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String), CType("Accounts Module", String), "AccountsModule\MatchOutstandingSearch.aspx?appid=" + appidnew, appidnew)
                DisableControl()

                ddlType.Attributes.Add("onchange", "javascript:FillCustDDL('" + CType(ddlType.ClientID, String) + "','" + CType(lblCustCode.ClientID, String) + "')")

                'txtDate.Attributes.Add("onchange", "javascript:CallWebMethod('customercode')")
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to exit?')==false)return false;")

                NumbersHtml(txtConversion)
                Dim typ As Type
                typ = GetType(DropDownList)

                ' If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
               
                ddlType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

              
                'End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("MatchOutstanding.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

            End Try
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            'If check_Privilege() = 1 Then

            '    chkPost.Enabled = True
            '    chkPost.Checked = True
            'Else

            '    chkPost.Enabled = False
            '    chkPost.Checked = True
            'End If

            'Added check_Privilege() and chkpost enabled by Archana on 01/04/2015
            If IsPostBack = True Then
                '    If ddlType.Value <> "[Select]" Then
                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccount, "code", "des", "select code, des from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlType.Value & "' order by code", True, txtcustcode.Value)
                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccountName, "des", "code", "select  des,code from view_account where div_code='" & ViewState("divcode") & "' and type = '" & ddlType.Value & "' order by des", True, txtcustname.Value)



                '        lblCustCode.Text = ddlType.Items(ddlType.SelectedIndex).Text & "  <font color='Red'> *</font>"


                '    Else
                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccount, "code", "des", "select top 10  code,des from view_account where div_code='" & ViewState("divcode") & "'   order by code", True)
                '        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccountName, "des", "code", "select top 10 des,code from view_account where div_code='" & ViewState("divcode") & "'   order by des", True)


                '        lblCustCode.Text = "Type <font color='Red'> *</font>"

                '    End If
                txtdecimal.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 509)
                'txtDivCode.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)
                txtbasecurr.Value = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_Selected", "param_id", 457)
            Else
                GrandTotal()
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MatchOutstanding.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub


    Private Sub GrandTotal()
        Try
            Dim totCr As Integer = 0
            Dim totDr As Integer = 0
            Dim totBaseCr As Integer = 0
            Dim totBaseDr As Integer = 0
            Dim j As Integer = 0
            Dim txtrowcnt As Integer = CInt(Val(txtgridrows.Value))

            For Each row As GridViewRow In grdMatchOut.Rows
                Dim valDr As Integer = CInt(Val(CType(row.FindControl("txtDebitAmt"), TextBox).Text))
                Dim valCr As Integer = CInt(Val(CType(row.FindControl("txtCreditAmt"), TextBox).Text))
                Dim chksel As HtmlInputCheckBox = CType(row.FindControl("chkSelect"), HtmlInputCheckBox)
                Dim valBaseDr As Integer = CInt(Val(CType(row.FindControl("txtBaseDebit"), TextBox).Text))
                Dim valBaseCr As Integer = CInt(Val(CType(row.FindControl("txtBaseCredit"), TextBox).Text))

                If chksel.Checked = True Then
                    totDr = DecRound(totDr) + DecRound(valDr)
                    totCr = DecRound(totCr) + DecRound(valCr)
                    totBaseDr = DecRound(totBaseDr) + DecRound(valBaseDr)
                    totBaseCr = DecRound(totBaseCr) + DecRound(valBaseCr)
                End If
            Next

            txtDebitTotal.Value = DecRound(totDr)
            txtCreditTotal.Value = DecRound(totCr)
            txtBaseDebitTotal.Value = DecRound(totBaseDr)
            txtBaseCreditTotal.Value = DecRound(totBaseCr)

            If DecRound(txtBaseDebitTotal.Value) > DecRound(txtBaseCreditTotal.Value) Then
                exchcredit.Value = DecRound(txtBaseDebitTotal.Value - txtBaseCreditTotal.Value)
                exchdebit.Value = 0
            ElseIf DecRound(txtBaseCreditTotal.Value) > DecRound(txtBaseDebitTotal.Value) Then
                exchdebit.Value = DecRound(txtBaseCreditTotal.Value - txtBaseDebitTotal.Value)
                exchcredit.Value = 0
            Else
                exchdebit.Value = 0
                exchcredit.Value = 0
            End If
        Catch ex As Exception

        End Try
    End Sub

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
 
#Region "Private Sub FillGrid()"
    Private Sub FillGrid()
        Dim myDS As New DataSet

        grdMatchOut.Visible = True

        If grdMatchOut.PageIndex < 0 Then
            grdMatchOut.PageIndex = 0
        End If

        strSqlQry = ""
        Try
            'strSqlQry = "exec sp_getadjust " 'sp_getadjust_MatchOutstanding
            ' changed sp as , new field being displayed docno_new 
            strSqlQry = "exec  sp_getadjust_MatchOutstanding" 'sp_getadjust

            If TxtCustCode_auto.Text <> "" Then
                strSqlQry = strSqlQry & " '" & TxtCustCode_auto.Text & "',"
            Else
                strSqlQry = strSqlQry & " '',"
            End If

            strSqlQry = strSqlQry & " '" & ddlType.SelectedValue & "','' ,'',null, "
            strSqlQry = strSqlQry & " '" & txtControlacct.Value & "',"

            If ddlType.Text = "C" Then
                strSqlQry = strSqlQry & " 'C',"
            Else
                strSqlQry = strSqlQry & " 'D',"
            End If


            strSqlQry = strSqlQry & "'" & txtDivCode.Value & "'" + ","

            If chkPost.Checked = True Then
                strSqlQry = strSqlQry & " 'P'" + ","
            Else
                strSqlQry = strSqlQry & " 'U'" + ","
            End If

            If txtPfromdate.Text <> "" Then
                strSqlQry = strSqlQry & "'" + Format(CType(txtPfromdate.Text, Date), "yyyy/MM/dd") + "'" + ","
            End If

            If txtPtodate.Text <> "" Then
                strSqlQry = strSqlQry & "'" + Format(CType(txtPtodate.Text, Date), "yyyy/MM/dd") + "'"
            End If


            'If txtConversion.Value <> "" Then
            '    strSqlQry = strSqlQry & " " & CType(txtConversion.Value, Decimal)
            'Else
            '    strSqlQry = strSqlQry & " null"
            'End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.SelectCommand.CommandTimeout = 0
            myDataAdapter.Fill(myDS, "AdjustBill")
            grdMatchOut.DataSource = myDS.Tables("AdjustBill")
            grdMatchOut.DataBind()

            Dim i As Integer
            Dim chksel As HtmlInputCheckBox
            For i = 0 To myDS.Tables(0).Rows.Count - 1
                chksel = grdMatchOut.Rows(i).FindControl("chkSelect")
                If Val(myDS.Tables(0).Rows(i)(7).ToString) <> 0 Then
                    chksel.Checked = True
                Else
                    chksel.Checked = False
                End If
            Next

            txtgridrows.Value = grdMatchOut.Rows.Count
            If myDS.Tables(0).Rows.Count > 0 Then
                EnableDisableControl(1)
            Else
                EnableDisableControl(2)
            End If
            imgicon.Style("visibility") = "hidden"
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MatchOutstanding.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try

    End Sub
#End Region

    Private Sub exchangediff()
        TxtExchName_auto.Enabled = True
    End Sub
    Protected Sub btnDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Dim frmdate As DateTime
        Dim todate As DateTime

        Dim MyCultureInfo As New CultureInfo("fr-Fr")

        If txtPfromdate.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('from date cannot be blank')", True)
            Exit Sub
        End If


        If txtPtodate.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Todate cannot be blank')", True)
            Exit Sub
        End If

        frmdate = DateTime.Parse(txtPfromdate.Text, MyCultureInfo, DateTimeStyles.None)
        todate = DateTime.Parse(txtPtodate.Text, MyCultureInfo, DateTimeStyles.None)

        If todate < frmdate Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('To date should be greater than from date')", True)
            Exit Sub
        End If


        FillGrid()
        exchangediff()
        txtDebitTotal.Value = ""
        txtCreditTotal.Value = ""
    End Sub

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("MatchOutStandingState") = "New" Then
            btnPrint.Visible = False
            TxtExchName_auto.Enabled = False
        ElseIf ViewState("MatchOutStandingState") = "Copy" Then
            btnPrint.Visible = False
            TxtExchName_auto.Enabled = False
        ElseIf ViewState("MatchOutStandingState") = "Edit" Then
            txtDocNo.Enabled = False
            'txtDate.Enabled = False
            txtDueDate.Disabled = True
            txtCurrency.Disabled = True
            txtConversion.Disabled = True
            ddlType.Enabled = False
            ' ddlAccount.Disabled = True
            TxtcustName_auto.Enabled = False
            'ddlConAccCode.Disabled = True
            txtControlacctname.Disabled = True
            'ImgBtnFrmDt.Enabled = False
            btnClear.Visible = False
            btnDisplay.Visible = False
            TxtExchName_auto.Enabled = False
            btnPrint.Visible = False

        ElseIf ViewState("MatchOutStandingState") = "Delete" Or ViewState("MatchOutStandingState") = "View" Or ViewState("MatchOutStandingState") = "Cancel" Or ViewState("MatchOutStandingState") = "UndoCancel" Then
            imgicon.Style("visibility") = "hidden"
            txtDocNo.Enabled = False
            txtDate.Enabled = False
            txtDueDate.Disabled = True
            txtnarration.Enabled = True
            txtCurrency.Disabled = True
            txtConversion.Disabled = True
            ddlType.Enabled = False
            TxtExchName_auto.Enabled = False
            'ddlAccount.Disabled = True
            TxtcustName_auto.Enabled = False
            ImgBtnFrmDt.Enabled = False
            btnDisplay.Enabled = False
            ' ddlNarration.Disabled = True
            'ddlConAccCode.Disabled = True
            txtControlacctname.Disabled = True
            btnSave.Visible = False
            btnPrint.Visible = False
            If ViewState("MatchOutStandingState") = "View" Then
                btnPrint.Visible = True
            ElseIf ViewState("MatchOutStandingState") = "Delete" Then
                btnSave.Visible = True
            ElseIf ViewState("MatchOutStandingState") = "Cancel" Or ViewState("MatchOutStandingState") = "UndoCancel" Then
                btnSave.Visible = True
                btnPrint.Visible = False
            End If



            btnClear.Visible = False
            chkPost.Visible = False
            DisableGrid()

        End If
    End Sub
    Private Sub EnableDisableControl(ByVal endisable As Integer)
        If endisable = 1 Then
            txtCurrency.Disabled = True
            txtConversion.Disabled = True
            ddlType.Enabled = False
            'ddlAccount.Disabled = True
            TxtcustName_auto.Enabled = False
            ' ddlConAccCode.Disabled = True
            txtControlacctname.Disabled = True
        Else
            txtCurrency.Disabled = False
            txtConversion.Disabled = False
            ddlType.Enabled = True
            ' ddlAccount.Disabled = False
            TxtcustName_auto.Enabled = True
            ' ddlConAccCode.Disabled = False
            txtControlacctname.Disabled = False
        End If
    End Sub

#End Region
    Private Sub DisableGrid()
        Dim txtPartic, txtCrVlaue, txtDrValue As TextBox
        Dim chksel As HtmlInputCheckBox
        Try
            For Each gvRow In grdMatchOut.Rows
                txtPartic = gvRow.FindControl("txtParticulars")
                txtCrVlaue = gvRow.FindControl("txtCreditAmt")
                txtDrValue = gvRow.FindControl("txtDebitAmt")
                chksel = gvRow.FindControl("chkSelect")

                txtPartic.Enabled = False
                txtCrVlaue.Enabled = False
                txtDrValue.Enabled = False
                chksel.Disabled = True

            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MatchOutstanding.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub grdMatchOut_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdMatchOut.RowDataBound
        Try
            Dim GVrow As GridViewRow
            Dim txtCrVlaue, txtDrValue, txtBalanceAmt, txtBaseDebit, txtBaseCredit, txtCurrRate As TextBox
            Dim chksel As HtmlInputCheckBox
            Dim strOpti As String
            GVrow = e.Row

            If e.Row.RowIndex = -1 Then
                strOpti = objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 457)
                GVrow.Cells(grd_col.BaseCredit).Text = strOpti & " Credit"
                GVrow.Cells(grd_col.BaseDebit).Text = strOpti & "Debit"
                lblBaseTotal.Text = strOpti & " Total"
                Exit Sub
            End If
            txtCrVlaue = GVrow.FindControl("txtCreditAmt")
            txtDrValue = GVrow.FindControl("txtDebitAmt")
            chksel = GVrow.FindControl("chkSelect")
            txtBaseDebit = GVrow.FindControl("txtBaseDebit")
            txtBaseCredit = GVrow.FindControl("txtBaseCredit")
            txtBalanceAmt = GVrow.FindControl("txtBalanceAmt")
            txtCurrRate = GVrow.FindControl("txtCurrRate")
            txtCrVlaue.Attributes.Add("onchange", "javascript:ChangeCr('" + CType(chksel.ClientID, String) + "','" + CType(txtCrVlaue.ClientID, String) + "','" + CType(txtDrValue.ClientID, String) + "','" + CType(txtBalanceAmt.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "')")
            txtDrValue.Attributes.Add("onchange", "javascript:ChangeDr('" + CType(chksel.ClientID, String) + "','" + CType(txtCrVlaue.ClientID, String) + "','" + CType(txtDrValue.ClientID, String) + "','" + CType(txtBalanceAmt.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "')")
            NumbersDecimal(txtCrVlaue)
            NumbersDecimal(txtDrValue)
            TextLock(txtCurrRate)
            TextLock(txtBalanceAmt)
            TextLock(txtBaseDebit)
            TextLock(txtBaseCredit)
            chksel.Attributes.Add("OnClick", "javascript:FillCrDr('" + CType(chksel.ClientID, String) + "','" + CType(txtCrVlaue.ClientID, String) + "','" + CType(txtDrValue.ClientID, String) + "','" + CType(txtBalanceAmt.ClientID, String) + "','" + CType(txtBaseCredit.ClientID, String) + "','" + CType(txtBaseDebit.ClientID, String) + "','" + CType(txtCurrRate.ClientID, String) + "')")
            If chksel.Checked = False Then
                txtCrVlaue.Enabled = False
                txtDrValue.Enabled = False
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MatchOutstanding.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("MatchOutstandingSearch.aspx", False)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('MatchOutStandingWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try

            
            Dim strdiv As String
            strdiv = ViewState("divcode") ' objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", 511)

            imgicon.Style("visibility") = "hidden"
            If Page.IsValid = True Then
                If ViewState("MatchOutStandingState") = "New" Or ViewState("MatchOutStandingState") = "Edit" Or ViewState("MatchOutStandingState") = "Copy" Or ViewState("MatchOutStandingState") = "UndoCancel" Then
                    If ValidatePage() = False Then
                        Exit Sub
                    End If

                    If ViewState("MatchOutStandingState") = "New" Or ViewState("MatchOutStandingState") = "Edit" Or ViewState("MatchOutStandingState") = "Delete" Or ViewState("MatchOutStandingState") = "Cancel" Or ViewState("MatchOutStandingState") = "UndoCancel" Then
                        If Validateseal() = False Then
                            Exit Sub
                        End If
                    End If

                End If
                SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
                sqlTrans = SqlConn.BeginTransaction
                If chkPost.Checked = True Then
                    'For Accounts posting
                    initialclass(SqlConn, sqlTrans)
                    'For Accounts posting
                End If
                If ViewState("MatchOutStandingState") = "New" Or ViewState("MatchOutStandingState") = "Edit" Or ViewState("MatchOutStandingState") = "Copy" Then

                    If ViewState("MatchOutStandingState") = "New" Or ViewState("MatchOutStandingState") = "Copy" Then
                        Dim optionval As String
                        txtDocNo.Text = ""
                        optionval = objUtils.GetAutoDocNodiv(CType(ViewState("TranType"), String), SqlConn, sqlTrans, ViewState("divcode"))
                        txtDocNo.Text = optionval.Trim
                        myCommand = New SqlCommand("sp_add_matchos_master", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    ElseIf ViewState("MatchOutStandingState") = "Edit" Then
                        myCommand = New SqlCommand("sp_mod_matchos_master", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                    End If

                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = txtDivCode.Value
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("TranType"), String)
                    If txtDate.Text = "" Then
                        myCommand.Parameters.Add(New SqlParameter("@matchos_date", SqlDbType.DateTime)).Value = DBNull.Value
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@matchos_date", SqlDbType.DateTime)).Value = Format(CType(txtDate.Text, Date), "yyyy/MM/dd")
                    End If
                    If txtDate.Text = "" Then
                        myCommand.Parameters.Add(New SqlParameter("@tran_date", SqlDbType.DateTime)).Value = DBNull.Value
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@tran_date", SqlDbType.DateTime)).Value = Format(CType(txtDate.Text, Date), "yyyy/MM/dd")
                    End If
                    myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = CType(ddlType.SelectedValue, String)
                    If TxtCustCode_auto.Text <> "" Then
                        myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(TxtCustCode_auto.Text, String)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If
                    If txtControlacct.Value <> "" Then
                        myCommand.Parameters.Add(New SqlParameter("@gl_code", SqlDbType.VarChar, 20)).Value = CType(txtControlacct.Value, String)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@gl_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If
                    myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(txtCurrency.Value, String)
                    myCommand.Parameters.Add(New SqlParameter("@currency_rate", SqlDbType.Decimal, 18, 12)).Value = CType(Val(txtConversion.Value), Decimal)
                    myCommand.Parameters.Add(New SqlParameter("@amount", SqlDbType.Money)).Value = DecRound(CType(Val(txtCreditTotal.Value), Double))
                    myCommand.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 200)).Value = CType(txtnarration.Text.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@matchos_tran_state", SqlDbType.VarChar, 1)).Value = "U"
                    myCommand.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = ObjDate.GetSystemDateTime(Session("dbconnectionName"))
                    myCommand.Parameters.Add(New SqlParameter("@adduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If chkPost.Checked = True Then
                        myCommand.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "P"
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@post_state", SqlDbType.VarChar, 1)).Value = "U"
                    End If

                    If TxtExchCode_auto.Text <> "" Then
                        myCommand.Parameters.Add(New SqlParameter("@exch_code", SqlDbType.VarChar, 20)).Value = CType(TxtExchCode_auto.Text, String)
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@exch_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If

                    If txtPfromdate.Text = "" Then
                        myCommand.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = DBNull.Value
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@fromdate", SqlDbType.DateTime)).Value = Format(CType(txtPfromdate.Text, Date), "yyyy/MM/dd")
                    End If

                    If txtPtodate.Text = "" Then
                        myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = DBNull.Value
                    Else
                        myCommand.Parameters.Add(New SqlParameter("@todate", SqlDbType.DateTime)).Value = Format(CType(txtPtodate.Text, Date), "yyyy/MM/dd")
                    End If

                    If ViewState("MatchOutStandingState") = "Edit" Then
                        myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    End If


                    myCommand.ExecuteNonQuery()

                    If chkPost.Checked = True Then

                        'For Accounts Posting
                        caccounts.clraccounts()
                        cacc.acc_tran_id = txtDocNo.Text
                        cacc.acc_tran_type = CType(ViewState("TranType"), String)
                        If txtDate.Text = "" Then
                            cacc.acc_tran_date = ""
                        Else
                            cacc.acc_tran_date = Format(CType(txtDate.Text, Date), "yyyy/MM/dd")
                        End If
                        cacc.acc_div_id = txtDivCode.Value

                        'Posting for the Header level
                        ctran = New clstran
                        ctran.acc_tran_id = cacc.acc_tran_id
                        ctran.acc_code = CType(TxtCustCode_auto.Text, String)
                        ctran.acc_type = CType(ddlType.SelectedValue, String)
                        ctran.acc_currency_id = CType(txtCurrency.Value, String)
                        ctran.acc_currency_rate = CType(Val(txtConversion.Value), Decimal)
                        ctran.acc_div_id = txtDivCode.Value
                        ctran.acc_narration = txtnarration.Text
                        ctran.acc_tran_date = cacc.acc_tran_date
                        ctran.acc_tran_lineno = 1
                        ctran.acc_tran_type = cacc.acc_tran_type
                        If txtControlacct.Value <> "" Then
                            ctran.pacc_gl_code = CType(txtControlacct.Value, String)
                        Else
                            ctran.pacc_gl_code = ""
                        End If
                        ctran.acc_ref1 = ""
                        ctran.acc_ref2 = ""
                        ctran.acc_ref3 = ""
                        ctran.acc_ref4 = ""
                        cacc.addtran(ctran)



                        ''match outstanding differences
                        If Val(exchdebit.Value) - Val(exchcredit.Value) <> 0 And TxtExchCode_auto.Text <> "" Then
                            ''                        txtDiffAmount = CType(gvRow.FindControl("txtDiffAmount"), HtmlInputText)
                            ctran = New clstran
                            ctran.acc_tran_id = cacc.acc_tran_id
                            ctran.acc_code = CType(TxtExchCode_auto.Text, String)
                            ctran.acc_type = "G"
                            ctran.acc_currency_id = mbasecurrency
                            ctran.acc_currency_rate = 1
                            ctran.acc_div_id = strdiv
                            ctran.acc_narration = txtnarration.Text
                            ctran.acc_tran_date = cacc.acc_tran_date
                            ctran.acc_tran_lineno = 2
                            ctran.acc_tran_type = cacc.acc_tran_type
                            ctran.pacc_gl_code = ""
                            ctran.acc_ref1 = ""
                            ctran.acc_ref2 = ""
                            ctran.acc_ref3 = ""
                            ctran.acc_ref4 = ""
                            cacc.addtran(ctran)

                            csubtran = New clsSubTran
                            csubtran.acc_against_tran_id = cacc.acc_tran_id
                            csubtran.acc_against_tran_lineno = 2
                            csubtran.acc_against_tran_type = cacc.acc_tran_type
                            csubtran.acc_debit = Math.Abs(CType(exchdebit.Value, Decimal))
                            csubtran.acc_credit = Math.Abs(CType(exchcredit.Value, Decimal))
                            csubtran.acc_base_debit = Math.Abs(CType(exchdebit.Value, Decimal))
                            csubtran.acc_base_credit = Math.Abs(CType(exchcredit.Value, Decimal))

                            csubtran.acc_tran_date = cacc.acc_tran_date
                            csubtran.acc_due_date = cacc.acc_tran_date
                            csubtran.acc_field1 = ""
                            csubtran.acc_field2 = ""
                            csubtran.acc_field3 = ""
                            csubtran.acc_field4 = ""
                            csubtran.acc_field5 = ""
                            csubtran.acc_tran_id = cacc.acc_tran_id
                            csubtran.acc_tran_lineno = 2 ' acc_tranlinenno
                            csubtran.acc_tran_type = cacc.acc_tran_type
                            csubtran.acc_narration = txtnarration.Text
                            csubtran.acc_type = "G"
                            csubtran.currate = 1
                            csubtran.costcentercode = ""
                            cacc.addsubtran(csubtran)
                            ' acc_tranlinenno = acc_tranlinenno + 1
                            'acc_against_tranlinenno = acc_against_tranlinenno + 1
                        End If


                    End If

                    'csubtran = New clsSubTran
                    'csubtran.acc_against_tran_id = cacc.acc_tran_id
                    'csubtran.acc_against_tran_lineno = 0
                    'csubtran.acc_against_tran_type = cacc.acc_tran_type
                    'csubtran.acc_debit = DecRound(CType(txtDebitTotal.Value, Decimal))
                    'csubtran.acc_credit = DecRound(CType(txtCreditTotal.Value, Decimal))
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
                    'csubtran.acc_narration = txtnarration.Value
                    'csubtran.acc_type = CType(ddlType.Value, String)
                    'csubtran.currate = CType(txtConversion.Value, Decimal)
                    'csubtran.acc_base_debit = DecRound(CType(txtBaseDebitTotal.Value, Decimal))
                    'csubtran.acc_base_credit = DecRound(CType(txtBaseCreditTotal.Value, Decimal))
                    'csubtran.costcentercode = ""
                    'cacc.addsubtran(csubtran)







                    If ViewState("MatchOutStandingState") = "Edit" Then
                        If chkPost.Checked = False Then
                            myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure
                            myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "matchos_master"
                            myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                            myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("TranType"), String)
                            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = txtDivCode.Value
                            myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                            myCommand.ExecuteNonQuery()
                        End If

                        myCommand = New SqlCommand("sp_del_open_detail_new", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("TranType"), String)
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = txtDivCode.Value
                        myCommand.ExecuteNonQuery()



                        If Not Session("freeform_Detail_mos") Is Nothing Then
                            Dim dataTable As DataTable = DirectCast(Session("freeform_Detail_mos"), DataTable)
                            '   Dim filterExpression As String = "  div_code = " & strdiv & " " ' and against_tran_lineno=1 ' against_tran_id =" & txtDocNo.Value & "  and   against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And 
                            '   Dim filteredRows() As DataRow = dataTable.Select(filterExpression)
                            Dim advpayment As Integer = 0
                            If ViewState("MatchOutStandingState") = "Edit" Then
                                If dataTable.Rows.Count > 0 Then




                                    For Each row As DataRow In dataTable.Rows
                                        If Val(row("Adjustedamount").ToString) <> 0 Then


                                            myCommand = New SqlCommand("sp_reverse_pending_invoices", SqlConn, sqlTrans)
                                            myCommand.CommandType = CommandType.StoredProcedure
                                            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                                            myCommand.Parameters.Add(New SqlParameter("@doclineno", SqlDbType.Int)).Value = CType(row("acc_TRAN_LINENO"), String)
                                            'If CType(row("tran_type"), String) = "RV" Then 'collectionDate("TranType" & strLineKey).ToString = "RV" Then
                                            myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = CType(row("docno"), String) ' collectionDate("TranId" & strLineKey).ToString
                                            myCommand.Parameters.Add(New SqlParameter("@doctype ", SqlDbType.VarChar, 10)).Value = CType(row("type"), String) 'collectionDate("TranType" & strLineKey).ToString

                                            If TxtCustCode_auto.Text <> "" Then
                                                myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(TxtCustCode_auto.Text, String)
                                            Else
                                                myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                            End If
                                            myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = CType(ddlType.SelectedValue, String)
                                            If DecRound(CType(row("adjusteddebit"), Decimal)) <> 0.0 Then
                                                myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(row("adjusteddebit"), Decimal))   ' DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(open_debit,0))open_debit  from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))
                                                myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = 0
                                                myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(row("adjusteddebit"), Decimal)) * DecRound(CType(row("currrate"), Decimal)) ' DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(base_debit,0))basedebit from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))
                                                myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = 0
                                            End If

                                            If DecRound(CType(row("adjustedcredit"), Decimal)) <> 0.0 Then
                                                myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = 0
                                                myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = DecRound(CType(row("adjustedcredit"), Decimal)) 'DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(open_credit,0))open_credit from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))
                                                myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = 0
                                                myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = DecRound(CType(row("adjustedcredit"), Decimal)) * DecRound(CType(row("currrate"), Decimal))
                                            End If


                                            'DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(base_credit,0))basecredit from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))

                                            '  End If
                                            myCommand.ExecuteNonQuery()
                                        End If
                                    Next

                                End If
                            End If

                        End If
                    End If









                    For Each gvRow In grdMatchOut.Rows
                        Dim txtPartic, txtCredit, txtDebit, txtBaseDebit, txtBaseCredit, txtCurrRate As TextBox
                        Dim chksel As HtmlInputCheckBox
                        Dim lbl As Label

                        chksel = gvRow.FindControl("chkSelect")
                        If chksel.Checked = True Then
                            lbl = gvRow.FindControl("lblLineID")
                            txtPartic = gvRow.FindControl("txtParticulars")
                            txtCredit = gvRow.FindControl("txtCreditAmt")
                            txtDebit = gvRow.FindControl("txtDebitAmt")
                            txtCurrRate = gvRow.FindControl("txtCurrRate")
                            chksel = gvRow.FindControl("chkSelect")
                            txtBaseDebit = gvRow.FindControl("txtBaseDebit")
                            txtBaseCredit = gvRow.FindControl("txtBaseCredit")

                            myCommand = New SqlCommand("sp_add_open_detail_new", SqlConn, sqlTrans)
                            myCommand.CommandType = CommandType.StoredProcedure

                            Dim txtdocNoGrid As TextBox = gvRow.FindControl("txtdocNo1")


                            myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtdocNoGrid.Text.Trim, String) 'CType(gvRow.Cells(grd_col.Docno).Text, String) 'changed by mohamed on 17/11/2020
                            If gvRow.Cells(grd_col.Docdate).Text = "&nbsp;" Then
                                myCommand.Parameters.Add(New SqlParameter("@tran_date", SqlDbType.DateTime)).Value = DBNull.Value
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@tran_date", SqlDbType.DateTime)).Value = Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(gvRow.Cells(grd_col.Docdate).Text), Date), "yyyy/MM/dd")
                            End If
                            myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(gvRow.Cells(grd_col.Doctype).Text, String)
                            myCommand.Parameters.Add(New SqlParameter("@tran_lineno", SqlDbType.Int, 9)).Value = Val(lbl.Text)
                            myCommand.Parameters.Add(New SqlParameter("@against_tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                            myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int, 9)).Value = 1
                            myCommand.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("TranType"), String)
                            If txtDate.Text = "" Then
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_date", SqlDbType.DateTime)).Value = DBNull.Value
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@against_tran_date", SqlDbType.DateTime)).Value = ObjDate.ConvertDateromTextBoxToDatabase(txtDate.Text)
                            End If
                            If gvRow.Cells(grd_col.Duedate).Text = "&nbsp;" Then
                                myCommand.Parameters.Add(New SqlParameter("@open_due_date", SqlDbType.DateTime)).Value = Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(gvRow.Cells(grd_col.Docdate).Text), Date), "yyyy/MM/dd") 'DBNull.Value
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@open_due_date", SqlDbType.DateTime)).Value = Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(gvRow.Cells(grd_col.Duedate).Text), Date), "yyyy/MM/dd")
                            End If
                            myCommand.Parameters.Add(New SqlParameter("@open_sales_code", SqlDbType.VarChar, 10)).Value = ""
                            If Trim(txtDebit.Text) <> "" Then
                                myCommand.Parameters.Add(New SqlParameter("@open_debit", SqlDbType.Money)).Value = DecRound(CType(txtDebit.Text, Decimal))
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@open_debit", SqlDbType.Money)).Value = DBNull.Value
                            End If
                            If Trim(txtCredit.Text) <> "" Then
                                myCommand.Parameters.Add(New SqlParameter("@open_credit", SqlDbType.Money)).Value = DecRound(CType(txtCredit.Text, Decimal))
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@open_credit", SqlDbType.Money)).Value = DBNull.Value
                            End If
                            myCommand.Parameters.Add(New SqlParameter("@open_field1", SqlDbType.VarChar, 100)).Value = ""
                            myCommand.Parameters.Add(New SqlParameter("@open_field2", SqlDbType.VarChar, 100)).Value = ""
                            myCommand.Parameters.Add(New SqlParameter("@open_field3", SqlDbType.VarChar, 100)).Value = CType(txtPartic.Text.Trim, String)
                            myCommand.Parameters.Add(New SqlParameter("@open_field4", SqlDbType.VarChar, 100)).Value = ""
                            myCommand.Parameters.Add(New SqlParameter("@open_field5", SqlDbType.VarChar, 100)).Value = ""
                            myCommand.Parameters.Add(New SqlParameter("@open_mode", SqlDbType.Char, 1)).Value = "B"
                            myCommand.Parameters.Add(New SqlParameter("@open_exchg_diff", SqlDbType.Money)).Value = 0
                            If ddlType.Text = "C" Then
                                myCommand.Parameters.Add(New SqlParameter("@dr_cr", SqlDbType.Char, 1)).Value = "C"
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@dr_cr", SqlDbType.Char, 1)).Value = "D"
                            End If
                            myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = txtDivCode.Value
                            If Trim(txtCurrRate.Text) <> "" Then
                                myCommand.Parameters.Add(New SqlParameter("@currency_rate", SqlDbType.Decimal, 18, 12)).Value = CType(txtCurrRate.Text, Decimal)
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@currency_rate", SqlDbType.Decimal, 18, 12)).Value = DBNull.Value
                            End If
                            If Trim(txtBaseDebit.Text) <> "" Then
                                myCommand.Parameters.Add(New SqlParameter("@base_debit", SqlDbType.Money)).Value = DecRound(CType(txtBaseDebit.Text, Decimal))
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@base_debit", SqlDbType.Money)).Value = DBNull.Value
                            End If
                            If Trim(txtBaseCredit.Text) <> "" Then
                                myCommand.Parameters.Add(New SqlParameter("@base_credit", SqlDbType.Money)).Value = DecRound(CType(txtBaseCredit.Text, Decimal))
                            Else
                                myCommand.Parameters.Add(New SqlParameter("@base_credit", SqlDbType.Money)).Value = DBNull.Value
                            End If

                            myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.Char, 1)).Value = CType(ddlType.SelectedValue, String)
                            myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(TxtCustCode_auto.Text, String)
                            myCommand.Parameters.Add(New SqlParameter("@acc_gl_code", SqlDbType.VarChar, 20)).Value = CType(txtControlacct.Value, String)
                            myCommand.ExecuteNonQuery()



                            'Dim newRow As DataRow = DataTable.NewRow()
                            '' newRow("TRANID") = intReceiptLinNo
                            'newRow("TRANID") = CType(txtdocNoGrid.Text.Trim, String)
                            'newRow("tran_type") = CType(gvRow.Cells(grd_col.Doctype).Text, String)
                            'newRow("tran_lineno") = Val(lbl.Text)
                            'If gvRow.Cells(grd_col.Docdate).Text = "&nbsp;" Then
                            '    newRow("tran_date") = DBNull.Value
                            'Else
                            '    newRow("tran_date") = Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(gvRow.Cells(grd_col.Docdate).Text), Date), "yyyy/MM/dd")
                            'End If

                            'newRow("against_tran_id") = CType(txtDocNo.Text.Trim, String)
                            'newRow("against_tran_lineno") = 1
                            'newRow("against_tran_type") = CType(ViewState("TranType"), String)
                            'If txtDate.Text = "" Then
                            '    newRow("against_tran_date") = DBNull.Value
                            'Else
                            '    newRow("against_tran_date") = ObjDate.ConvertDateromTextBoxToDatabase(txtDate.Text)
                            'End If
                            'If gvRow.Cells(grd_col.Duedate).Text = "&nbsp;" Then
                            '    newRow("open_due_date") = Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(gvRow.Cells(grd_col.Docdate).Text), Date), "yyyy/MM/dd") 'DBNull.Value
                            'Else
                            '    newRow("open_due_date") = Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(gvRow.Cells(grd_col.Duedate).Text), Date), "yyyy/MM/dd")
                            'End If
                            'If Trim(txtDebit.Text) <> "" Then
                            '    newRow("open_debit") = DecRound(CType(txtDebit.Text, Decimal))
                            'Else
                            '    newRow("open_debit") = DBNull.Value
                            'End If
                            'If Trim(txtCredit.Text) <> "" Then
                            '    newRow("open_credit") = DecRound(CType(txtCredit.Text, Decimal))
                            'Else
                            '    newRow("open_credit") = DBNull.Value
                            'End If
                            'newRow("open_field1") = ""
                            'newRow("open_field2") = ""
                            'newRow("open_field3") = CType(txtPartic.Text.Trim, String)
                            'newRow("open_field4") = ""
                            'newRow("open_field5") = ""
                            'newRow("open_mode") = "B"

                            'newRow("div_id") = txtDivCode.Value
                            'If Trim(txtCurrRate.Text) <> "" Then
                            '    newRow("currency_rate") = CType(txtCurrRate.Text, Decimal)
                            'Else
                            '    newRow("currency_rate") = DBNull.Value
                            'End If
                            'If Trim(txtBaseDebit.Text) <> "" Then
                            '    newRow("base_debit") = DecRound(CType(txtBaseDebit.Text, Decimal))
                            'Else
                            '    newRow("base_debit") = DBNull.Value
                            'End If
                            'If Trim(txtBaseCredit.Text) <> "" Then
                            '    newRow("base_CREDIT") = DecRound(CType(txtBaseCredit.Text, Decimal))
                            'Else
                            '    newRow("base_CREDIT") = DBNull.Value
                            'End If



                            'newRow("acc_type") = CType(ddlType.SelectedValue, String)
                            'newRow("acc_code") = CType(TxtCustCode_auto.Text, String)
                            'newRow("acc_gl_code") = CType(txtControlacct.Value, String)
                            'newRow("currcode") = CType(txtCurrency.Value, String)
                            'dataTable.Rows.Add(newRow)


                            'Posting for the Grid Accounts
                            'ctran = New clstran
                            'ctran.acc_tran_id = cacc.acc_tran_id
                            'ctran.acc_code = CType(ddlAccount.Items(ddlAccount.SelectedIndex).Text, String)
                            'ctran.acc_type = CType(ddlType.Value, String)
                            'ctran.acc_currency_id = CType(txtCurrency.Value, String)
                            'ctran.acc_currency_rate = DecRound(CType(txtConversion.Value, Decimal))
                            'ctran.acc_div_id = txtDivCode.Value
                            'ctran.acc_narration = txtPartic.Text
                            'ctran.acc_tran_date = cacc.acc_tran_date
                            'ctran.acc_tran_lineno = Val(lbl.Text)
                            'ctran.acc_tran_type = CType(gvRow.Cells(grd_col.Doctype).Text, String) 'cacc.acc_tran_type
                            'If ddlConAccCode.Value <> "[Select]" Then
                            '    ctran.pacc_gl_code = ddlConAccCode.Items(ddlConAccCode.SelectedIndex).Text
                            'Else
                            '    ctran.pacc_gl_code = ""
                            'End If
                            'ctran.acc_ref1 = ""
                            'ctran.acc_ref2 = ""
                            'ctran.acc_ref3 = ""
                            'ctran.acc_ref4 = ""
                            'cacc.addtran(ctran)

                            'If ddlAccType.Items(ddlAccType.SelectedIndex).Text = "G" Then
                            '    csubtran = New clsSubTran
                            '    csubtran.acc_against_tran_id = cacc.acc_tran_id
                            '    csubtran.acc_against_tran_lineno = lblno.Value
                            '    csubtran.acc_against_tran_type = cacc.acc_tran_type
                            '    If CType(Session("RVPVTranType"), String) = "RV" Then
                            '        csubtran.acc_debit = DecRound(IIf(CType(txtCredit.Value, Decimal) < 0, Math.Abs(CType(txtCredit.Value, Decimal)), 0))
                            '        csubtran.acc_credit = DecRound(IIf(CType(txtCredit.Value, Decimal) > 0, CType(txtCredit.Value, Decimal), 0))
                            '        csubtran.acc_base_debit = DecRound(IIf(CType(txtBaseCredit.Value, Decimal) < 0, Math.Abs(CType(txtBaseCredit.Value, Decimal)), 0))
                            '        csubtran.acc_base_credit = DecRound(IIf(CType(txtBaseCredit.Value, Decimal) > 0, CType(txtBaseCredit.Value, Decimal), 0))
                            '    ElseIf CType(Session("RVPVTranType"), String) = "PV" Then
                            '        csubtran.acc_credit = DecRound(IIf(CType(txtCredit.Value, Decimal) < 0, Math.Abs(CType(txtCredit.Value, Decimal)), 0))
                            '        csubtran.acc_debit = DecRound(IIf(CType(txtCredit.Value, Decimal) > 0, CType(txtCredit.Value, Decimal), 0))
                            '        csubtran.acc_base_credit = DecRound(IIf(CType(txtBaseCredit.Value, Decimal) < 0, Math.Abs(CType(txtBaseCredit.Value, Decimal)), 0))
                            '        csubtran.acc_base_debit = DecRound(IIf(CType(txtBaseCredit.Value, Decimal) > 0, CType(txtBaseCredit.Value, Decimal), 0))
                            '    End If
                            '    csubtran.acc_tran_date = cacc.acc_tran_date
                            '    csubtran.acc_due_date = cacc.acc_tran_date
                            '    csubtran.acc_field1 = ""
                            '    csubtran.acc_field2 = ""
                            '    csubtran.acc_field3 = ""
                            '    csubtran.acc_field4 = ""
                            '    csubtran.acc_field5 = ""
                            '    csubtran.acc_tran_id = cacc.acc_tran_id
                            '    csubtran.acc_tran_lineno = lblno.Value
                            '    csubtran.acc_tran_type = cacc.acc_tran_type
                            '    csubtran.acc_narration = txtNarr.Value
                            '    csubtran.acc_type = ddlAccType.Items(ddlAccType.SelectedIndex).Text
                            '    csubtran.currate = DecRound(CType(txtCurrRate.Value, Decimal))
                            '    'if it is blank then post to default 510
                            '    If ddlCCCode.Value <> "[Select]" Then
                            '        csubtran.costcentercode = ddlCCCode.Items(ddlCCCode.SelectedIndex).Text
                            '    Else
                            '        csubtran.costcentercode = strcostcentercode
                            '    End If

                            '    cacc.addsubtran(csubtran)
                            'End If

                        End If
                    Next
                    '  Session("AdjustedRecords") = dataTable
                    If chkPost.Checked = True Then
                        'For Accounts Posting
                        cacc.table_name = ""
                        caccounts.Addaccounts(cacc)
                        If caccounts.saveaccounts(Session("dbconnectionName"), SqlConn, sqlTrans, Me.Page) <> 0 Then
                            Err.Raise(vbObjectError + 100)
                        End If

                        If ViewState("MatchOutStandingState") <> "Delete" Then
                            For Each gvRow In grdMatchOut.Rows
                                Dim txtPartic, txtCredit, txtDebit, txtBaseDebit, txtBaseCredit, txtCurrRate, txtdocno1 As TextBox
                                Dim chksel As HtmlInputCheckBox
                                Dim lbl As Label

                                chksel = gvRow.FindControl("chkSelect")
                                If chksel.Checked = True Then
                                    lbl = gvRow.FindControl("lblLineID")
                                    txtPartic = gvRow.FindControl("txtParticulars")
                                    txtCredit = gvRow.FindControl("txtCreditAmt")
                                    txtDebit = gvRow.FindControl("txtDebitAmt")
                                    txtCurrRate = gvRow.FindControl("txtCurrRate")
                                    chksel = gvRow.FindControl("chkSelect")
                                    txtBaseDebit = gvRow.FindControl("txtBaseDebit")
                                    txtBaseCredit = gvRow.FindControl("txtBaseCredit")
                                    txtdocno1 = gvRow.FindControl("txtdocno1")
                                    myCommand = New SqlCommand("sp_update_Pending_Invoices", SqlConn, sqlTrans)
                                    myCommand.CommandType = CommandType.StoredProcedure

                                    Dim txtdocNoGrid As TextBox = gvRow.FindControl("txtdocNo1")
                                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = txtDivCode.Value
                                    myCommand.Parameters.Add(New SqlParameter("@doclineno", SqlDbType.Int)).Value = Val(lbl.Text)
                                    myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = CType(txtdocno1.Text.Trim, String)
                                    myCommand.Parameters.Add(New SqlParameter("@doctype ", SqlDbType.VarChar, 10)).Value = (CType(gvRow.Cells(grd_col.Doctype).Text, String))


                                    If gvRow.Cells(grd_col.Docdate).Text = "&nbsp;" Then
                                        myCommand.Parameters.Add(New SqlParameter("@docdate", SqlDbType.DateTime)).Value = DBNull.Value


                                    Else
                                        myCommand.Parameters.Add(New SqlParameter("@docdate", SqlDbType.DateTime)).Value = Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(gvRow.Cells(grd_col.Docdate).Text), Date), "yyyy/MM/dd")

                                    End If
                                    myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(TxtCustCode_auto.Text, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = CType(ddlType.SelectedValue, String) ' IIf(ddlType.Text = "Customer", "C", "S") ' "G"      '"G"
                                    myCommand.Parameters.Add(New SqlParameter("@against_tran_id ", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                                    myCommand.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = (CType(ViewState("TranType"), String))
                                    myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int)).Value = 1

                                    If txtControlacct.Value <> "" Then
                                        myCommand.Parameters.Add(New SqlParameter("@acc_gl_code", SqlDbType.VarChar, 20)).Value = CType(txtControlacct.Value, String)
                                    Else
                                        myCommand.Parameters.Add(New SqlParameter("@acc_gl_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                    End If
                                    If gvRow.Cells(grd_col.Duedate).Text = "&nbsp;" Then
                                        myCommand.Parameters.Add(New SqlParameter("@duedate", SqlDbType.DateTime)).Value = DBNull.Value
                                    Else
                                        myCommand.Parameters.Add(New SqlParameter("@duedate", SqlDbType.DateTime)).Value = Format(CType(ObjDate.ConvertDateromTextBoxToDatabase(gvRow.Cells(grd_col.Duedate).Text), Date), "yyyy/MM/dd")
                                    End If
                                    If Trim(txtCurrRate.Text) <> "" Then
                                        myCommand.Parameters.Add(New SqlParameter("@currrate", SqlDbType.Decimal, 18, 12)).Value = CType(txtCurrRate.Text, Decimal)
                                    Else
                                        myCommand.Parameters.Add(New SqlParameter("@currrate", SqlDbType.Decimal, 18, 12)).Value = DBNull.Value
                                    End If

                                    myCommand.Parameters.Add(New SqlParameter("@field1", SqlDbType.VarChar, 500)).Value = ""
                                    myCommand.Parameters.Add(New SqlParameter("@field2", SqlDbType.VarChar, 500)).Value = ""
                                    myCommand.Parameters.Add(New SqlParameter("@field3 ", SqlDbType.VarChar, 500)).Value = CType(txtPartic.Text.Trim, String)
                                    myCommand.Parameters.Add(New SqlParameter("@field4", SqlDbType.VarChar, 500)).Value = ""
                                    myCommand.Parameters.Add(New SqlParameter("@field5", SqlDbType.VarChar, 500)).Value = ""
                                    myCommand.Parameters.Add(New SqlParameter("@accmode", SqlDbType.VarChar, 1)).Value = "B"
                                    myCommand.Parameters.Add(New SqlParameter("@open_field3 ", SqlDbType.VarChar, 500)).Value = ""
                                    myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 10)).Value = CType(txtCurrency.Value, String)
                                    myCommand.Parameters.Add(New SqlParameter("@acc_State", SqlDbType.VarChar, 10)).Value = "P"



                                    If Trim(txtDebit.Text) <> "" Then
                                        myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(txtDebit.Text, Decimal))
                                    Else
                                        myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DBNull.Value
                                    End If
                                    If Trim(txtCredit.Text) <> "" Then
                                        myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = DecRound(CType(txtCredit.Text, Decimal))
                                    Else
                                        myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = DBNull.Value
                                    End If

                                    If Trim(txtBaseDebit.Text) <> "" Then
                                        myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(txtBaseDebit.Text, Decimal))
                                    Else
                                        myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DBNull.Value
                                    End If
                                    If Trim(txtBaseCredit.Text) <> "" Then
                                        myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = DecRound(CType(txtBaseCredit.Text, Decimal))
                                    Else
                                        myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = DBNull.Value
                                    End If


                                    myCommand.ExecuteNonQuery()


                                    ''  Dim dataTabledt As DataTable = DirectCast(Session("Adjustedrecords"), DataTable)
                                    'If dataTable.Rows.Count > 0 Then
                                    '    For Each row As DataRow In dataTabledt.Rows
                                    '        myCommand = New SqlCommand("sp_update_Pending_Invoices", SqlConn, sqlTrans)
                                    '        myCommand.CommandType = CommandType.StoredProcedure
                                    '        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = txtDivCode.Value
                                    '        myCommand.Parameters.Add(New SqlParameter("@doclineno", SqlDbType.Int)).Value = CType(row("tran_lineno"), Integer)
                                    '        myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = CType(row("docno"), Integer)
                                    '        myCommand.Parameters.Add(New SqlParameter("@doctype ", SqlDbType.VarChar, 10)).Value = CType(row("doctype"), Integer)
                                    '        myCommand.Parameters.Add(New SqlParameter("@docdate", SqlDbType.DateTime)).Value = CType(row("docdate"), Integer)

                                    '        myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(row("acc_code"), Integer)

                                    '        myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = IIf(ddlType.Text = "Customer", "C", "S") ' "G"      '"G"
                                    '        myCommand.Parameters.Add(New SqlParameter("@against_tran_id ", SqlDbType.VarChar, 20)).Value = CType(row("against_tran_id"), Integer)
                                    '        myCommand.Parameters.Add(New SqlParameter("@against_tran_type", SqlDbType.VarChar, 10)).Value = CType(row("against_tran_type"), Integer)
                                    '        myCommand.Parameters.Add(New SqlParameter("@against_tran_lineno", SqlDbType.Int)).Value = CType(row("against_tran_lineno"), Integer)
                                    '        If txtControlacct.Value <> "" Then
                                    '            myCommand.Parameters.Add(New SqlParameter("@acc_gl_code", SqlDbType.VarChar, 20)).Value = CType(row("acc_gl_code"), Integer)
                                    '        Else
                                    '            myCommand.Parameters.Add(New SqlParameter("@acc_gl_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                    '        End If
                                    '        If txtDate.Text = "" Then
                                    '            myCommand.Parameters.Add(New SqlParameter("@duedate", SqlDbType.DateTime)).Value = DBNull.Value
                                    '        Else
                                    '            myCommand.Parameters.Add(New SqlParameter("@duedate", SqlDbType.DateTime)).Value = CType(row("currrate"), Decimal)
                                    '        End If

                                    '        myCommand.Parameters.Add(New SqlParameter("@currrate", SqlDbType.Decimal, 18, 12)).Value = CType(row("currrate"), Decimal)
                                    '        myCommand.Parameters.Add(New SqlParameter("@field1", SqlDbType.VarChar, 500)).Value = CType(row("open_field1"), String)
                                    '        myCommand.Parameters.Add(New SqlParameter("@field2", SqlDbType.VarChar, 500)).Value = CType(row("open_field2"), String)
                                    '        myCommand.Parameters.Add(New SqlParameter("@field3 ", SqlDbType.VarChar, 500)).Value = CType(row("open_field3"), String)
                                    '        myCommand.Parameters.Add(New SqlParameter("@field4", SqlDbType.VarChar, 500)).Value = CType(row("open_field4"), String)
                                    '        myCommand.Parameters.Add(New SqlParameter("@field5", SqlDbType.VarChar, 500)).Value = CType(row("open_field5"), Decimal)
                                    '        myCommand.Parameters.Add(New SqlParameter("@accmode", SqlDbType.VarChar, 1)).Value = CType(row("open_field3"), Decimal)
                                    '        myCommand.Parameters.Add(New SqlParameter("@open_field3 ", SqlDbType.VarChar, 500)).Value = CType(row("open_field3"), Decimal)
                                    '        myCommand.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 10)).Value = CType(row("open_field3"), String)
                                    '        myCommand.Parameters.Add(New SqlParameter("@acc_State", SqlDbType.VarChar, 10)).Value = "P"

                                    '        If CType(ViewState("CNDNOpen_type"), String) = "IN" Then
                                    '            myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(row("debit"), Decimal))
                                    '            myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = 0
                                    '            myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(row("basedebit"), Decimal))
                                    '            myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = 0
                                    '        Else
                                    '            If CType(ViewState("CNDNOpen_type"), String) = "DN" Then

                                    '                myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(row("debit"), Decimal))
                                    '                myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = 0
                                    '                myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(row("basedebit"), Decimal))
                                    '                myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = 0

                                    '            Else
                                    '                myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = 0
                                    '                myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = DecRound(CType(row("credit"), Decimal))
                                    '                myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = 0
                                    '                myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = DecRound(CType(row("basecredit"), Decimal))
                                    '            End If
                                    '        End If

                                    '        '  myCommand.Parameters.Add(New SqlParameter("@basecurrency", SqlDbType.Decimal, 18, 12)).Value = 0
                                    '        myCommand.ExecuteNonQuery()
                                End If
                            Next
                            'End If


                        End If
                        'For Accounts Posting
                        lblPostmsg.Text = "Posted"
                        lblPostmsg.ForeColor = Drawing.Color.Red
                    Else
                        lblPostmsg.Text = "UnPosted"
                        lblPostmsg.ForeColor = Drawing.Color.Green
                    End If


                ElseIf ViewState("MatchOutStandingState") = "Delete" Then

                    myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "matchos_master"
                    myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("TranType"), String)
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = txtDivCode.Value
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()
                    If Not Session("freeform_Detail_mos") Is Nothing Then
                        Dim dataTable As DataTable = DirectCast(Session("freeform_Detail_mos"), DataTable)
                        '   Dim filterExpression As String = "  div_code = " & strdiv & " " ' and against_tran_lineno=1 ' against_tran_id =" & txtDocNo.Value & "  and   against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And 
                        '   Dim filteredRows() As DataRow = dataTable.Select(filterExpression)
                        Dim advpayment As Integer = 0
                        'If ViewState("MatchOutStandingState") = "Edit" Then
                        If dataTable.Rows.Count > 0 Then
                            For Each row As DataRow In dataTable.Rows
                                If Val(row("Adjustedamount").ToString) <> 0 Then


                                    myCommand = New SqlCommand("sp_reverse_pending_invoices", SqlConn, sqlTrans)
                                    myCommand.CommandType = CommandType.StoredProcedure
                                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                                    myCommand.Parameters.Add(New SqlParameter("@doclineno", SqlDbType.Int)).Value = CType(row("acc_TRAN_LINENO"), String)
                                    'If CType(row("tran_type"), String) = "RV" Then 'collectionDate("TranType" & strLineKey).ToString = "RV" Then
                                    myCommand.Parameters.Add(New SqlParameter("@docno", SqlDbType.VarChar, 20)).Value = CType(row("docno"), String) ' collectionDate("TranId" & strLineKey).ToString
                                    myCommand.Parameters.Add(New SqlParameter("@doctype ", SqlDbType.VarChar, 10)).Value = CType(row("type"), String) 'collectionDate("TranType" & strLineKey).ToString

                                    If TxtCustCode_auto.Text <> "" Then
                                        myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = CType(TxtCustCode_auto.Text, String)
                                    Else
                                        myCommand.Parameters.Add(New SqlParameter("@acc_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                                    End If
                                    myCommand.Parameters.Add(New SqlParameter("@acc_type", SqlDbType.VarChar, 1)).Value = CType(ddlType.SelectedValue, String)
                                    If DecRound(CType(row("adjusteddebit"), Decimal)) <> 0.0 Then
                                        myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = DecRound(CType(row("adjusteddebit"), Decimal))   ' DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(open_debit,0))open_debit  from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))
                                        myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = 0
                                        myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = DecRound(CType(row("adjusteddebit"), Decimal)) * DecRound(CType(row("currrate"), Decimal)) ' DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(base_debit,0))basedebit from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))
                                        myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = 0
                                    End If

                                    If DecRound(CType(row("adjustedcredit"), Decimal)) <> 0.0 Then
                                        myCommand.Parameters.Add(New SqlParameter("@debit", SqlDbType.Money)).Value = 0
                                        myCommand.Parameters.Add(New SqlParameter("@credit", SqlDbType.Money)).Value = DecRound(CType(row("adjustedcredit"), Decimal)) 'DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(open_credit,0))open_credit from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))
                                        myCommand.Parameters.Add(New SqlParameter("@basedebit", SqlDbType.Money)).Value = 0
                                        myCommand.Parameters.Add(New SqlParameter("@basecredit", SqlDbType.Money)).Value = DecRound(CType(row("adjustedcredit"), Decimal)) * DecRound(CType(row("currrate"), Decimal))
                                    End If


                                    'DecRound(CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select sum(isnull(base_credit,0))basecredit from open_detail od(nolock)  where od.against_tran_id =" & txtDocNo.Value & "  and od.against_tran_lineno= " & intReceiptLinNo & " and  od.against_tran_type='" & CType(ViewState("ReceiptsRVPVTranType"), String) & "' And od.div_id = " & strdiv & " "), Decimal))

                                    '  End If
                                    myCommand.ExecuteNonQuery()
                                End If
                            Next

                        End If
                    End If


                    myCommand = New SqlCommand("sp_del_matchos", SqlConn, sqlTrans)
                    myCommand.CommandType = CommandType.StoredProcedure
                    myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                    myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("TranType"), String)
                    myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = txtDivCode.Value
                    myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    myCommand.ExecuteNonQuery()


                    ElseIf ViewState("MatchOutStandingState") = "Cancel" Then

                        myCommand = New SqlCommand("sp_delvoucher", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@tablename", SqlDbType.VarChar, 20)).Value = "matchos_master"
                        myCommand.Parameters.Add(New SqlParameter("@tranid", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                        myCommand.Parameters.Add(New SqlParameter("@trantype ", SqlDbType.VarChar, 10)).Value = CType(ViewState("TranType"), String)
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = txtDivCode.Value
                        myCommand.ExecuteNonQuery()

                        myCommand = New SqlCommand("sp_cancel_matchos", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = txtDivCode.Value
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_type", SqlDbType.VarChar, 10)).Value = CType(ViewState("TranType"), String)
                        myCommand.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        myCommand.ExecuteNonQuery()

                        myCommand = New SqlCommand("sp_cancel_matchos_open_detail_new", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("TranType"), String)
                        myCommand.ExecuteNonQuery()

                    ElseIf ViewState("MatchOutStandingState") = "UndoCancel" Then
                        myCommand = New SqlCommand("sp_undocancel_matchos", SqlConn, sqlTrans)
                        myCommand.CommandType = CommandType.StoredProcedure
                        myCommand.Parameters.Add(New SqlParameter("@div_id", SqlDbType.VarChar, 10)).Value = strdiv
                        myCommand.Parameters.Add(New SqlParameter("@tran_id", SqlDbType.VarChar, 20)).Value = CType(txtDocNo.Text.Trim, String)
                        myCommand.Parameters.Add(New SqlParameter("@tran_type ", SqlDbType.VarChar, 10)).Value = CType(ViewState("TranType"), String)
                        myCommand.ExecuteNonQuery()

                    End If

                    sqlTrans.Commit()    'SQl Tarn Commit
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(myCommand)               'sql command disposed
                    clsDBConnect.dbConnectionClose(SqlConn)           'connection close
                    'Response.Redirect("MatchOutstandingSearch.aspx", False)

                    If ViewState("MatchOutStandingState") = "Delete" Or ViewState("MatchOutStandingState") = "Cancel" Or ViewState("MatchOutStandingState") = "UndoCancel" Then
                        '  Response.Redirect("MatchOutstandingSearch.aspx", False)
                        Dim strscript As String = ""
                        strscript = "window.opener.__doPostBack('MatchOutStandingWindowPostBack', '');window.opener.focus();window.close();"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                    Else
                        If ViewState("MatchOutStandingState") = "New" Or ViewState("MatchOutStandingState") = "Copy" Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record save successfully');", True)
                        ElseIf ViewState("MatchOutStandingState") = "Edit" Then
                            Dim strURL As String = ""




                            'strURL = "Accnt_trn_amendlog.aspx?tid=" & txtDocNo.Text & "&ttype=" & ViewState("TranType").ToString & "&tdate=" & txtDate.Text.Trim

                            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", String.Format(ScriptOpenModalDialog, strURL, 300), True)

                            strURL = "window.open('Accnt_trn_amendlog.aspx?tid=" & txtDocNo.Text & "&ttype=" & ViewState("TranType").ToString & "&tdate=" & txtDate.Text.Trim + "','Log','width=100,height=100 left=20,top=20 status=1,toolbar=no,menubar=no,resizable=no,scrollbars=yes');"
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strURL, True)


                            'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record update successfully');", True)
                        End If

                        btnPrint.Visible = True
                        ViewState("MatchOutStandingState") = "View"
                        DisableControl()
                        'btnPrint_Click(sender, e) 

                    End If

                End If
        Catch ex As Exception
            If SqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MatchOutstanding.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

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

#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            chkPost.Checked = False
            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            myCommand = New SqlCommand("Select * from matchos_master Where div_id='" & ViewState("divcode") & "' and tran_id='" & RefCode & "' and tran_type='MOS' ", SqlConn)
            mySqlReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If ViewState("MatchOutStandingState") <> "Copy" Then
                        If IsDBNull(mySqlReader("tran_id")) = False Then
                            Me.txtDocNo.Text = CType(mySqlReader("tran_id"), String)
                        Else
                            Me.txtDocNo.Text = ""
                        End If
                    End If
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


                    If IsDBNull(mySqlReader("cancel_state")) = False Then
                        If CType(mySqlReader("cancel_state"), String) = "Y" Then
                            lblPostmsg.Text = "Cancelled"
                            lblPostmsg.ForeColor = Drawing.Color.Green
                        End If
                    End If

                    If IsDBNull(mySqlReader("tran_date")) = False Then
                        Me.txtDate.Text = CType(Format(CType(mySqlReader("tran_date"), Date), "dd/MM/yyyy"), String)
                    Else
                        Me.txtDate.Text = ""
                    End If
                    If IsDBNull(mySqlReader("acc_type")) = False Then

                        Me.ddlType.Text = CType(mySqlReader("acc_type"), String)
                        type = Me.ddlType.Text
                        



                    End If

                    If IsDBNull(mySqlReader("fromdate")) = False Then
                        Me.txtPfromdate.Text = CType(Format(CType(mySqlReader("fromdate"), Date), "dd/MM/yyyy"), String)
                    Else
                        Me.txtPfromdate.Text = ""
                    End If

                    If IsDBNull(mySqlReader("todate")) = False Then
                        Me.txtPtodate.Text = CType(Format(CType(mySqlReader("todate"), Date), "dd/MM/yyyy"), String)
                    Else
                        Me.txtPtodate.Text = ""
                    End If

                    
                    If IsDBNull(mySqlReader("acc_code")) = False Then

                        Me.TxtCustCode_auto.Text = CType(mySqlReader("acc_code"), String)


                        'Me.ddlCustomer.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"view_account", "des", "code", CType(mySqlReader("supcode"), String))
                        TxtcustName_auto.Text = objUtils.GetDBFieldFromMultipleCriterianewdiv(Session("dbconnectionName"), "view_account", "des", "code='" & CType(mySqlReader("acc_code") & "' and type='" & type & "'", String), "div_code", ViewState("divcode"))
                        'txtcustcode.Value = objUtils.GetDBFieldFromMultipleCriterianewdiv(Session("dbconnectionName"), "view_account", "des", "code='" & CType(mySqlReader("acc_code") & "' and type='" & ddlType.Value & "'", String), "div_code", ViewState("divcode"))
                        'txtcustname.Value = CType(mySqlReader("acc_code"), String)
                    Else
                        Me.TxtCustCode_auto.Text = ""
                        Me.TxtcustName_auto.Text = ""
                        'txtcustcode.Value = ""
                        'txtcustname.Value = ""
                    End If
                    If IsDBNull(mySqlReader("currcode")) = False Then
                        Me.txtCurrency.Value = CType(mySqlReader("currcode"), String)
                     
                        Me.txtCurrencyname.Value = objUtils.GetDBFieldFromMultipleCriterianew(Session("dbconnectionName"), "currmast", "currname", "currcode='" & CType(mySqlReader("currcode"), String) & "'")
                    Else
                        Me.txtCurrency.Value = ""
                        Me.txtCurrencyname.Value = ""
                    End If
                    If IsDBNull(mySqlReader("currency_rate")) = False Then
                        Me.txtConversion.Value = CType(mySqlReader("currency_rate"), Decimal)
                    Else
                        Me.txtConversion.Value = ""
                    End If

                    If IsDBNull(mySqlReader("amount")) = False Then
                        Me.txtCreditTotal.Value = Format(CType(mySqlReader("amount"), Double), "00.00")
                        Me.txtDebitTotal.Value = Format(CType(mySqlReader("amount"), Double), "00.00")
                    Else
                        Me.txtCreditTotal.Value = ""
                        Me.txtDebitTotal.Value = ""
                    End If
                    If IsDBNull(mySqlReader("narration")) = False Then
                        Me.txtnarration.Text = CType(mySqlReader("narration"), String)
                    Else
                        Me.txtnarration.Text = ""
                    End If
                    If IsDBNull(mySqlReader("gl_code")) = False Then
                        'ddlConAccName.Value = mySqlReader("trd_gl_code").ToString
                        'ddlConAccCode.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"),"select acctname from acctmast where acctcode ='" & mySqlReader("trd_gl_code").ToString & "' ")
                        txtControlacct.Value = mySqlReader("gl_code").ToString
                        txtControlacctname.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where div_code='" & ViewState("divcode") & "' and acctcode ='" & mySqlReader("gl_code").ToString & "' ")
                    Else

                        txtControlacct.Value = ""
                        txtControlacctname.Value = ""
                    End If
                    ''Exchange difference
                  
                    If IsDBNull(mySqlReader("diffpostac")) = False Then

                        TxtExchCode_auto.Text = CType(mySqlReader("diffpostac"), String)

                        'Me.ddlCustomer.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"view_account", "des", "code", CType(mySqlReader("supcode"), String))
                        Me.TxtExchName_auto.Text = objUtils.GetDBFieldFromMultipleCriterianewdiv(Session("dbconnectionName"), "view_account", "des", "code='" & CType(mySqlReader("diffpostac") & "' and type='G'", String), "div_code", ViewState("divcode"))
                       
                    Else
                        TxtExchName_auto.Text = ""
                        TxtExchCode_auto.Text = ""
                    End If


                    txtConversion.Disabled = False
                    If Trim(txtbasecurr.Value) = Trim(txtCurrency.Value) Then
                        txtConversion.Disabled = True
                    End If
                    lblCustCode.Text = ddlType.Items(ddlType.SelectedIndex).Text & " <font color='Red'> *</font>"

                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MatchOutstanding.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(myCommand)
            clsDBConnect.dbReaderClose(mySqlReader)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try
    End Sub
#End Region

#Region "Private Sub ShowGridData()"
    Private Sub ShowGridData()
        Dim myDS As New DataSet

        grdMatchOut.Visible = True

        If grdMatchOut.PageIndex < 0 Then
            grdMatchOut.PageIndex = 0
        End If

        strSqlQry = ""
        Try
            'strSqlQry = "exec sp_getadjust "
            strSqlQry = "exec   sp_getadjust_MatchOutstanding"

            If TxtCustCode_auto.Text <> "" Then
                strSqlQry = strSqlQry & " '" & TxtCustCode_auto.Text & "',"
            Else
                strSqlQry = strSqlQry & " '',"
            End If

            strSqlQry = strSqlQry & " '" & ddlType.SelectedValue & "', '" & txtDocNo.Text & "', 1,'" & CType(ViewState("TranType"), String) & "' ,"

            If txtControlacct.Value <> "" Then
                strSqlQry = strSqlQry & " '" & txtControlacct.Value & "',"
            Else
                strSqlQry = strSqlQry & " '',"
            End If
            If ddlType.Text = "C" Then
                strSqlQry = strSqlQry & " 'C',"
            Else
                strSqlQry = strSqlQry & " 'D',"
            End If



            strSqlQry = strSqlQry & "'" & txtDivCode.Value & "'" + ","

            If chkPost.Checked = True Then
                strSqlQry = strSqlQry & " 'P'" + ","
            Else
                strSqlQry = strSqlQry & " 'U'" + ","
            End If

            If txtPfromdate.Text <> "" Then
                strSqlQry = strSqlQry & "'" + Format(CType(txtPfromdate.Text, Date), "yyyy/MM/dd") + "'" + ","
            End If

            If txtPtodate.Text <> "" Then
                strSqlQry = strSqlQry & "'" + Format(CType(txtPtodate.Text, Date), "yyyy/MM/dd") + "'"
            End If

            'If txtConversion.Value <> "" Then
            '    strSqlQry = strSqlQry & " " & DecRound(CType(txtConversion.Value, Decimal))
            'Else
            '    strSqlQry = strSqlQry & " null"
            'End If

            SqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.SelectCommand.CommandTimeout = 0
            myDataAdapter.Fill(myds)

            'Tanvir 11102023
            If myDS.Tables(0).Rows.Count > 0 Then
                Session("freeform_Detail_mos") = CType(myDS.Tables(0), DataTable)
            End If
            'Tanvir 11102023
            grdMatchOut.DataSource = myDS
            grdMatchOut.DataBind()

            txtgridrows.Value = grdMatchOut.Rows.Count

            Dim i As Integer
            Dim txtPartic, txtCrVlaue, txtDrValue, txtBaseDebit, txtBaseCredit, txtBalanceAmt As TextBox
            Dim chksel As HtmlInputCheckBox
            Dim basecrtotal As Decimal
            Dim basedbtotal As Decimal

            For i = 0 To myDS.Tables(0).Rows.Count - 1
                txtPartic = grdMatchOut.Rows(i).FindControl("txtParticulars")
                txtCrVlaue = grdMatchOut.Rows(i).FindControl("txtCreditAmt")
                txtDrValue = grdMatchOut.Rows(i).FindControl("txtDebitAmt")
                chksel = grdMatchOut.Rows(i).FindControl("chkSelect")
                txtBaseDebit = grdMatchOut.Rows(i).FindControl("txtBaseDebit")
                txtBaseCredit = grdMatchOut.Rows(i).FindControl("txtBaseCredit")
                txtBalanceAmt = grdMatchOut.Rows(i).FindControl("txtBalanceAmt")

                txtCrVlaue.Enabled = False
                txtDrValue.Enabled = False

                If txtdecimal.Value = 2 Then

                    If IsDBNull(myDS.Tables(0).Rows(i)(15).ToString) = False Then
                        txtDrValue.Text = Format(DecRound(CType(myDS.Tables(0).Rows(i)(15).ToString, Double)), "00.00")
                        txtBaseDebit.Text = Format(DecRound(CType(myDS.Tables(0).Rows(i)(15).ToString, Double) * CType(myDS.Tables(0).Rows(i)(5).ToString, Double)), "00.00")
                        ''     txtBaseDebit.Text = DecRound(CType(myDS.Tables(0).Rows(i)(15).ToString, Double) * CType(myDS.Tables(0).Rows(i)(5).ToString, Double))
                    End If

                    If IsDBNull(myDS.Tables(0).Rows(i)(14).ToString) = False Then
                        txtCrVlaue.Text = Format(DecRound(CType(myDS.Tables(0).Rows(i)(14).ToString, Double)), "00.00")
                        txtBaseCredit.Text = Format(DecRound(CType(myDS.Tables(0).Rows(i)(14).ToString, Double) * CType(myDS.Tables(0).Rows(i)(5).ToString, Double)), "00.00")
                        ''  txtBaseCredit.Text = DecRound(CType(myDS.Tables(0).Rows(i)(14).ToString, Double) * CType(myDS.Tables(0).Rows(i)(5).ToString, Double))
                    End If
                Else
                    If IsDBNull(myDS.Tables(0).Rows(i)(15).ToString) = False Then
                        txtDrValue.Text = Format(DecRound(CType(myDS.Tables(0).Rows(i)(15).ToString, Double)), "00.000")
                        txtBaseDebit.Text = Format(DecRound(CType(myDS.Tables(0).Rows(i)(15).ToString, Double) * CType(myDS.Tables(0).Rows(i)(5).ToString, Double)), "00.000")
                        ''     txtBaseDebit.Text = DecRound(CType(myDS.Tables(0).Rows(i)(15).ToString, Double) * CType(myDS.Tables(0).Rows(i)(5).ToString, Double))
                    End If
                    If IsDBNull(myDS.Tables(0).Rows(i)(14).ToString) = False Then
                        txtCrVlaue.Text = Format(DecRound(CType(myDS.Tables(0).Rows(i)(14).ToString, Double)), "00.000")
                        txtBaseCredit.Text = Format(DecRound(CType(myDS.Tables(0).Rows(i)(14).ToString, Double) * CType(myDS.Tables(0).Rows(i)(5).ToString, Double)), "00.000")
                        ''  txtBaseCredit.Text = DecRound(CType(myDS.Tables(0).Rows(i)(14).ToString, Double) * CType(myDS.Tables(0).Rows(i)(5).ToString, Double))
                    End If
                End If


                basedbtotal = DecRound(DecRound(basedbtotal) + DecRound(txtBaseDebit.Text))
                basecrtotal = DecRound(DecRound(basecrtotal) + DecRound(txtBaseCredit.Text))



                If IsDBNull(myDS.Tables(0).Rows(i)(16).ToString) = False Then
                    txtPartic.Text = CType(myDS.Tables(0).Rows(i)(16).ToString, String)
                End If

                If Val(myDS.Tables(0).Rows(i)(7).ToString) <> 0 Then
                    chksel.Checked = True
                Else
                    chksel.Checked = False
                End If

                If chksel.Checked = True Then
                    If txtCrVlaue.Text <> "" Then
                        If Val(txtCrVlaue.Text) > 0 Then
                            txtCrVlaue.Enabled = True
                            txtDrValue.Enabled = False
                        End If
                    End If
                    If txtDrValue.Text <> "" Then
                        If Val(txtDrValue.Text) > 0 Then
                            txtCrVlaue.Enabled = False
                            txtDrValue.Enabled = True
                        End If
                    End If
                End If

            Next
            txtBaseCreditTotal.Value = DecRound(basecrtotal)
            txtBaseDebitTotal.Value = DecRound(basedbtotal)

            If Val(txtBaseDebitTotal.Value) > Val(txtBaseCreditTotal.Value) Then
                exchcredit.Value = DecRound(Val(txtBaseDebitTotal.Value)) - DecRound(Val(txtBaseCreditTotal.Value))
                exchdebit.Value = 0
            Else
                If Val(txtBaseCreditTotal.Value) > Val(txtBaseDebitTotal.Value) Then
                    exchdebit.Value = DecRound(Val(txtBaseCreditTotal.Value)) - DecRound(Val(txtBaseDebitTotal.Value))
                    exchcredit.Value = 0
                Else
                    exchdebit.Value = 0
                    exchcredit.Value = 0
                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MatchOutstanding.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbAdapterClose(myDataAdapter)
            clsDBConnect.dbConnectionClose(SqlConn)
        End Try
    End Sub
#End Region

#Region "ValidatePage()"
    Public Function ValidatePage() As Boolean
        Try
            'comment due to some matchoutstanding has to be post for the previous year.as per accounts dept.request
            'If txtDate.Text <> "" Then
            '    If CType(txtDate.Text, Date).Year <> ObjDate.GetSystemDateOnly(Session("dbconnectionName")).Year Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Posting Date should be valid current year date.');", True)
            '        SetFocus(txtDate)
            '        ValidatePage = False
            '        Exit Function
            '    End If
            'End If

            'If Math.Abs(CType(txtDebitTotal.Value, Decimal)) = 0 Or Math.Abs(CType(txtCreditTotal.Value, Decimal)) = 0 Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Total amount should not be zero.');", True)
            '    SetFocus(grdMatchOut)
            '    ValidatePage = False
            '    Exit Function
            'End If
            If txtBaseDebitTotal.Value = "" Then
                txtBaseDebitTotal.Value = "0"
            End If
            If exchdebit.Value = "" Then
                exchdebit.Value = "0"
            End If
            If txtBaseCreditTotal.Value = "" Then
                txtBaseCreditTotal.Value = "0"
            End If
            If exchcredit.Value = "" Then
                exchcredit.Value = "0"
            End If
            If (Math.Abs(CType(txtBaseDebitTotal.Value, Decimal)) + Math.Abs(CType(exchdebit.Value, Decimal))) <> (Math.Abs(CType(txtBaseCreditTotal.Value, Decimal)) + Math.Abs(CType(exchcredit.Value, Decimal))) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Total base debit and Total base credit should be equal.');", True)
                SetFocus(grdMatchOut)
                ValidatePage = False
                Exit Function
            End If


            If Math.Abs(CType(exchdebit.Value, Decimal)) <> Math.Abs(CType(exchcredit.Value, Decimal)) Then
                If TxtExchCode_auto.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select the Exchange Diff. code.');", True)
                    SetFocus(exchdebit)
                    ValidatePage = False
                    Exit Function
                End If
            End If

            'If Math.Abs(CType(exchdebit.Value, Decimal)) = Math.Abs(CType(exchcredit.Value, Decimal)) Then
            '    If TxtExchCode_auto.Text <> "" Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No need to select Exchange code');", True)
            '        SetFocus(exchdebit)
            '        ValidatePage = False
            '        Exit Function
            '    End If
            'End If


            Dim flgCheck As Boolean = False
            Dim chksel As HtmlInputCheckBox
            For Each gvRow In grdMatchOut.Rows
                chksel = gvRow.FindControl("chkSelect")
                If chksel.Checked = True Then
                    flgCheck = True
                    Exit For
                End If
            Next

            If flgCheck = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select atleast one record in grid.');", True)
                SetFocus(grdMatchOut)
                ValidatePage = False
                Exit Function
            End If

            ValidatePage = True

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MatchOutstanding.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#End Region



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
        cacc.tran_mode = IIf(ViewState("MatchOutStandingState") = "New", 1, 2)
        mbasecurrency = CType(objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=457"), String)
        cacc.start()

    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            'If MsgBox("Do you want to print", MsgBoxStyle.YesNo, "Doc Print") = MsgBoxResult.No Then
            '    Exit Sub
            'End If
            'Dim strReportTitle As String = ""
            'Dim strSelectionFormula As String = ""
            'Session.Add("RefCode", CType(txtDocNo.Text.Trim, String))
            'Session.Add("Pageame", "MatchOutstandingDoc")
            'Session.Add("BackPageName", "~\AccountsModule\MatchOutstanding.aspx")

            'strSelectionFormula = ""
            'If txtDocNo.Text.Trim <> "" Then
            '    If Trim(strSelectionFormula) = "" Then
            '        ' strReportTitle = "Doc No : " & txtDocNo.Text.Trim
            '        strSelectionFormula = " {matchos_master.tran_id}='" & txtDocNo.Text.Trim & "'"
            '    Else
            '        'strReportTitle = strReportTitle & "Doc No : " & txtDocNo.Text.Trim & "'"
            '        strSelectionFormula = strSelectionFormula & " {matchos_master.tran_id}='" & txtDocNo.Text.Trim & "'"
            '    End If
            'Else

            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Doc No' );", True)
            '    Exit Sub
            'End If
            'If Trim(strSelectionFormula) = "" Then
            '    strSelectionFormula = " {matchos_master.tran_type} = '" & CType(Session("TranType"), String) & "' " & _
            '    " and  {matchos_master.div_id} = '" & CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
            'Else
            '    strSelectionFormula = strSelectionFormula & " AND {matchos_master.tran_type} = '" & CType(Session("TranType"), String) & "'" & _
            '    " and  {matchos_master.div_id} = '" & CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
            'End If
            'Dim lblstr As String
            'lblstr = ""

            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Session.Add("PrinDocTitle", lblstr)

            Dim ScriptStr As String
            ScriptStr = "<script language=""javascript"">var win=window.open('../PriceListModule/PrintDocNew.aspx?Pageame=MatchOutstandingDoc&BackPageName=~\AccountsModule\MatchOutstanding.aspx&TranId=" & txtDocNo.Text & "&TranType=" & CType(ViewState("TranType"), String) & "','printdoc');</script>"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)


        Catch ex As Exception
            objUtils.WritErrorLog("MatchOutstanding.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub btnclientprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnclientprint.Click
        Try
            'If MsgBox("Do you want to print", MsgBoxStyle.YesNo, "Doc Print") = MsgBoxResult.No Then
            '    Exit Sub
            'End If
            'Dim strReportTitle As String = ""
            'Dim strSelectionFormula As String = ""
            'Session.Add("RefCode", CType(txtDocNo.Text.Trim, String))
            'Session.Add("Pageame", "MatchOutstandingDoc")
            'Session.Add("BackPageName", "~\AccountsModule\MatchOutstanding.aspx")

            'strSelectionFormula = ""
            'If txtDocNo.Text.Trim <> "" Then
            '    If Trim(strSelectionFormula) = "" Then
            '        ' strReportTitle = "Doc No : " & txtDocNo.Text.Trim
            '        strSelectionFormula = " {matchos_master.tran_id}='" & txtDocNo.Text.Trim & "'"
            '    Else
            '        'strReportTitle = strReportTitle & "Doc No : " & txtDocNo.Text.Trim & "'"
            '        strSelectionFormula = strSelectionFormula & " {matchos_master.tran_id}='" & txtDocNo.Text.Trim & "'"
            '    End If
            'Else

            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Doc No' );", True)
            '    Exit Sub
            'End If
            'If Trim(strSelectionFormula) = "" Then
            '    strSelectionFormula = " {matchos_master.tran_type} = '" & CType(Session("TranType"), String) & "' " & _
            '    " and  {matchos_master.div_id} = '" & CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
            'Else
            '    strSelectionFormula = strSelectionFormula & " AND {matchos_master.tran_type} = '" & CType(Session("TranType"), String) & "'" & _
            '    " and  {matchos_master.div_id} = '" & CType(objUtils.GetDBFieldFromLongnew(Session("dbconnectionName"),"reservation_parameters", "option_selected", "param_id", 511), String) & "'"
            'End If
            'Dim lblstr As String
            'lblstr = ""

            'Session.Add("SelectionFormula", strSelectionFormula)
            'Session.Add("ReportTitle", strReportTitle)
            'Session.Add("PrinDocTitle", lblstr)

            Dim ScriptStr As String
            ScriptStr = "<script language=""javascript"">var win=window.open('../PriceListModule/PrintDocNew.aspx?Pageame=MatchOutstandingDoc&BackPageName=~\AccountsModule\MatchOutstanding.aspx&TranId=" & txtDocNo.Text & "&TranType=" & CType(ViewState("TranType"), String) & "&divid=" & ViewState("divcode") & "&Curr=" & CType(txtCurrency.Value, String) & "','printdoc');</script>"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)


        Catch ex As Exception
            objUtils.WritErrorLog("MatchOutstanding.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=MatchOutstanding','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        EnableDisableControl(2)
        grdMatchOut.Visible = False
    End Sub
    Private Sub CheckPostUnpostRight(ByVal UserName As String, ByVal UserPwd As String, ByVal AppName As String, ByVal PageName As String, ByVal appid As String)
        Dim PostUnpostFlag As Boolean = False
        PostUnpostFlag = objUser.PostUnpostRightnew(Session("dbconnectionName"), UserName, UserPwd, AppName, PageName, appid)
        If PostUnpostFlag = True Then
            chkPost.Visible = True
            lblPostmsg.Visible = True
        Else
            chkPost.Visible = False
            lblPostmsg.Visible = False
            If ViewState("MatchOutStandingState") = "Edit" Then
                If chkPost.Checked = True Then
                    ViewState.Add("MatchOutStandingState", "View")
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This transaction has been posted, you do not have rights to edit.' );", True)
                End If
            End If
        End If
    End Sub

   
    Protected Sub ddlType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlType.TextChanged
        TxtcustName_auto.Text = ""
        TxtCustCode_auto.Text = ""
        txtControlacct.Value = ""
        txtControlacctname.Value = ""
        txtCurrency.Value = ""
        txtConversion.Value = ""
        txtCurrencyname.Value = ""
        Select Case ddlType.Text
            Case "S"
                TxtCustName_AutoCompleteExtender.ServiceMethod = "Getsupplist"
                lblCustCode.Text = "Supplier"
            Case "C"
                TxtCustName_AutoCompleteExtender.ServiceMethod = "Getcustlist"
                lblCustCode.Text = "Customer"
            Case "A"
                TxtCustName_AutoCompleteExtender.ServiceMethod = "Getsuppagentlist"
                lblCustCode.Text = "Supplier Agent"
        End Select

    End Sub

    Protected Sub PdfReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PdfReport.Click
        Try
            Dim ScriptStr As String
            ScriptStr = "<script language=""javascript"">var win=window.open('TransactionReports.aspx?printId=MatchOutStanding&TranId=" & txtDocNo.Text & "&TranType=" & CType(ViewState("TranType"), String) & "','printdoc');</script>"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)


        Catch ex As Exception
            objUtils.WritErrorLog("MatchOutstanding.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
    Protected Sub btnclientpdf_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclientpdf.Click
        Try

            Dim ScriptStr As String
            ScriptStr = "<script language=""javascript"">var win=window.open('TransactionReports.aspx?printId=MatchOutStanding&TranId=" & txtDocNo.Text & "&TranType=" & CType(ViewState("TranType"), String) & "&divid=" & ViewState("divcode") & "&Curr=" & CType(txtCurrency.Value, String) & "','printdoc');</script>"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", ScriptStr, False)


        Catch ex As Exception
            objUtils.WritErrorLog("MatchOutstanding.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

End Class

 