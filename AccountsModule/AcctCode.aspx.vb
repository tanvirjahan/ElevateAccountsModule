'------------------------------------------------------------------------------------------------
'   Module Name    :    AcctCode 
'   Developer Name :    Mangesh
'   Date           :    
'   
'------------------------------------------------------------------------------------------------
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
#End Region
Partial Class AcctCode
    Inherits System.Web.UI.Page
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim objdate As New clsDateTime
    Dim strQry As String

    Dim myDataAdapter As SqlDataAdapter
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim mySqlCmd As SqlCommand
    <System.Web.Script.Services.ScriptMethod()> _
      <System.Web.Services.WebMethod()> _
    Public Shared Function Getctrylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim currnames As New List(Of String)
        Try
            strSqlQry = "select currcode,currname from currmast where active=1 and  currname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    currnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("currname").ToString(), myDS.Tables(0).Rows(i)("currcode").ToString()))

                Next
            End If
            Return currnames
        Catch ex As Exception
            Return currnames
        End Try

    End Function
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Session("CompanyName") Is Nothing Then
                Me.Page.Title = CType(Session("CompanyName"), String)
            End If

            ViewState.Add("AcctcodeState", Request.QueryString("State"))
            ViewState.Add("AcctcodeRefCode", Request.QueryString("RefCode"))
            ViewState.Add("AcctcodeParentId", Request.QueryString("ParentId"))
            ViewState.Add("AcctcodeChildId", Request.QueryString("ChildId"))
            ViewState.Add("AcctcodeAccLevel", Request.QueryString("AccLevel"))
            ViewState.Add("AcctcodeLevel", Request.QueryString("Level"))
            ViewState.Add("divcode", Request.QueryString("divid"))
            If Page.IsPostBack = False Then
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                'Numbers(txtAccCode)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlBankType, "bank_master_type_des", "bank_master_type_code", "select bank_master_type_des,bank_master_type_code from bank_master_type order by bank_master_type_des ")
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurrency, "currname", "currcode", "select currname,currcode from Currmast order by currname")
                'lblCustSupp.Style.Add("visibility", "hidden")
                'ddCustSupp.Style.Add("visibility", "hidden")
                'lblbantypename.Style.Add("visibility", "hidden")
                'lblCurrency.Style.Add("visibility", "hidden")
                'ddlBankType.Style.Add("visibility", "hidden")
                ' banktype.Visible = False
          
                'TxtCurrencyName.Style.Add("visibility", "hidden")

                banktype.Style("display") = "none"
                lblCustSupp.Style("display") = "none"
                ddCustSupp.Style("display") = "none"

                If ViewState("AcctcodeState") = "New" Then
                    SetFocus(txtAccCode)
                    Page.Title = "Add Account Code"
                    lblHeading.Text = "Add Account Code"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save')==false)return false;")
                ElseIf ViewState("AcctcodeState") = "Edit" Then
                    Page.Title = "Edit Account Code"
                    lblHeading.Text = "Edit Account Code"
                    btnSave.Text = "Update"
                    ShowRecord(CType(ViewState("AcctcodeRefCode"), String))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update')==false)return false;")
                ElseIf ViewState("AcctcodeState") = "View" Then
                    Page.Title = "View Account Code"
                    lblHeading.Text = "View Account Code"
                    ShowRecord(CType(ViewState("AcctcodeRefCode"), String))
                ElseIf ViewState("AcctcodeState") = "Delete" Then
                    Page.Title = "Delete Account Code"
                    lblHeading.Text = "Delete Account Code"
                    btnSave.Text = "Delete"
                    ShowRecord(CType(ViewState("AcctcodeRefCode"), String))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete')==false)return false;")
                End If
                disableControls()

                ' ddlControlAc.Attributes.Add("onchange", "sel_control_ac()")
                'ddlBankAc.Attributes.Add("onchange", "sel_bank_ac()")

                'DropdownList Aplphabetical order----   
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlBankType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlCurrency.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If


            End If



            If ddlBankAc.Value = "Y" Then
                banktype.Style("display") = "block"

            End If

            If ddlControlAc.Value = "Y" Then

                lblCustSupp.Style("display") = "block"
                ddCustSupp.Style("display") = "block"
            End If

        Catch ex As Exception
            mySqlConn.Close()
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("AcctCode.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
    Private Sub disableControls()
        If ViewState("AcctcodeState") = "New" Then
        ElseIf ViewState("AcctcodeState") = "Edit" Then
            txtAccCode.Disabled = True
        ElseIf ViewState("AcctcodeState") = "View" Or ViewState("AcctcodeState") = "Delete" Then
            txtAccCode.Disabled = True
            txtAccName.Disabled = True
            ddlBankType.Disabled = True
            TxtCurrencyName.Enabled = False
            'ddlCurrency.Disabled = True
            ddlBankAc.Disabled = True
            ddlControlAc.Disabled = True
            ddlCustSupp.Disabled = True
            btnSave.Visible = False
        End If
        If ViewState("AcctcodeState") = "Delete" Then
            btnSave.Visible = True
        End If

    End Sub
#Region " Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from acctmast Where div_code='" & ViewState("divcode") & "' and acctcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.Read Then
                txtAccCode.Value = mySqlReader("acctcode")
                txtAccName.Value = IIf(IsDBNull(mySqlReader("acctname")) = True, "", mySqlReader("acctname"))

                If mySqlReader("controlyn").ToString = "Y" Then
                    ddlControlAc.Value = "Y"
                    ddlCustSupp.Value = mySqlReader("cust_supp")
                    lblCustSupp.Style.Add("display", "block")
                    ddCustSupp.Style.Add("display", "block")
                ElseIf mySqlReader("controlyn").ToString = "N" Then
                    ddlControlAc.Value = "N"
                    lblCustSupp.Style.Add("display", "none")
                    ddCustSupp.Style.Add("display", "none")
                End If

                If mySqlReader("bankyn").ToString = "Y" Then
                    ddlBankAc.Value = "Y"
                    ddlBankType.Value = mySqlReader("bank_master_type_code")
                    'ddlCurrency.Value = mySqlReader("currcode")
                    TxtCurrencyCode.Text = mySqlReader("currcode")
                    TxtCurrencyName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", mySqlReader("currcode"))
                    banktype.Style.Add("display", "block")

                    'lblbantypename.Style.Add("visibility", "visible")
                    'lblCurrency.Style.Add("visibility", "visible")
                    'ddlBankType.Style.Add("visibility", "visible")

                    '  TxtCurrencyName.Visible = True
                    'TxtCurrencyName.Style.Add("visibility", "visible")
                    ' TxtCurrencyName.Visible = True
                Else
                    ddlBankAc.Value = "N"
                    banktype.Style.Add("display", "none")
                    'lblbantypename.Style.Add("visibility", "hidden")
                    'lblCurrency.Style.Add("visibility", "hidden")
                    'ddlBankType.Style.Add("visibility", "hidden")
                    'TxtCurrencyName.Style.Add("visibility", "hidden")
                    '  ddlCurrency.Style.Add("visibility", "hidden")
                End If
            End If



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("AcctCode.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             'sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region
#Region "Public Function fnValidate() As Boolean"
    Public Function fnValidate() As Boolean
        If txtAccCode.Value.Trim = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter the account code.');", True)
            SetFocus(txtAccCode)
            fnValidate = False
            Exit Function
        End If
        If txtAccName.Value.Trim = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter the account name.');", True)
            SetFocus(txtAccName)
            fnValidate = False
            Exit Function
        End If

        If ViewState("AcctcodeState") = "New" Then
            If objUtils.isDuplicatenewdiv(Session("dbconnectionName"), "view_account", "code", txtAccCode.Value.Trim, "div_code", ViewState("divcode")) = 1 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This account code is already present...');", True)
                SetFocus(txtAccCode)
                fnValidate = False
                Exit Function
            End If
            If objUtils.isDuplicatenewdiv(Session("dbconnectionName"), "view_account", "des", txtAccName.Value.Trim, "div_code", ViewState("divcode")) = 1 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This account name is already present.');", True)
                SetFocus(txtAccCode)
                fnValidate = False
                Exit Function
            End If
        ElseIf ViewState("AcctcodeState") = "Edit" Then
            'If objUtils.isDuplicatenew(Session("dbconnectionName"),"acctgroup", "acctcode", "acctcode", txtAccCode.Value.Trim, CType(Session("RefCode"), String)) = 1 Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This account code is already present.');", True)
            '    SetFocus(txtAccCode)
            '    fnValidate = False
            'End If
            If objUtils.isDuplicateForModifynewdiv(Session("dbconnectionName"), "view_account", "code", "des", txtAccName.Value.Trim, CType(Session("RefCode"), String), "div_code", ViewState("divcode")) = 1 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This account name is already present.');", True)
                SetFocus(txtAccCode)
                fnValidate = False
                Exit Function
            End If
        End If


        Dim strDelimiter As String
        strDelimiter = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "123")

        If txtAccCode.Value.Contains(strDelimiter) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This account code  canot enter - " & strDelimiter & " ');", True)
            SetFocus(txtAccCode)
            fnValidate = False
            Exit Function
        End If

        If txtAccName.Value.Contains(strDelimiter) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This account name  canot enter - " & strDelimiter & " ');", True)
            SetFocus(txtAccName)
            fnValidate = False
            Exit Function
        End If

        fnValidate = True

    End Function
#End Region



#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim strscript As String = ""
            If fnValidate() = False Then
                Exit Sub
            End If
            'Delete Record
            If ViewState("AcctcodeState") = "Delete" Then
                If checkForDeletion() = False Then
                    Exit Sub
                End If



                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                mySqlCmd = New SqlCommand("sp_del_acctgroup", mySqlConn, sqlTrans)
                mySqlCmd.CommandText = "sp_del_acctgroup"
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@parentid", SqlDbType.Int)).Value = CType(ViewState("AcctcodeParentId"), Long)
                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
                mySqlCmd.ExecuteNonQuery()
                sqlTrans.Commit()
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)
                'Response.Redirect("AcctCodesSearch.aspx", False)


                strscript = "window.opener.__doPostBack('AcctcodeWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                Exit Sub
            End If

            '// Add & Edit Record
            Dim intParentId As Integer = ViewState("AcctcodeParentId")
            Dim intChildId As Integer = ViewState("AcctcodeChildId")
            Dim intAcctType As Integer = 2
            Dim intAccLevel As Integer = ViewState("AcctcodeAccLevel")
            Dim intacctbsorder As Integer

            Dim intAcctOrder As Integer = objUtils.GetDBFieldFromLongnewdiv(Session("dbconnectionName"), "acctgroup", "acctorder", "parentid", intChildId, "div_code", ViewState("divcode"))
            intacctbsorder = objUtils.GetDBFieldFromLongnewdiv(Session("dbconnectionName"), "acctgroup", "acctbsorder", "parentid", intChildId, "div_code", ViewState("divcode"))

            Dim strControl As String = ddlControlAc.Value
            Dim strbankyn As String = ddlBankAc.Value


            Dim strLevel As String = ViewState("AcctcodeLevel")
            Dim strSplLevel As String() = strLevel.Split("-")

            Dim intLevel1 As Long = CType(strSplLevel.GetValue(0), Long)
            Dim intLevel2 As Long = CType(strSplLevel.GetValue(1), Long)
            Dim intLevel3 As Long = CType(strSplLevel.GetValue(2), Long)
            Dim intLevel4 As Long = CType(strSplLevel.GetValue(3), Long)
            Dim intLevel5 As Long = CType(strSplLevel.GetValue(4), Long)
            Dim intLevel6 As Long = CType(strSplLevel.GetValue(5), Long)
            Dim intLevel7 As Long = CType(strSplLevel.GetValue(6), Long)



            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

            '//Save acctgroup
            If ViewState("AcctcodeState") = "New" Then
                mySqlCmd = New SqlCommand("sp_add_acctgroup", mySqlConn, sqlTrans)
                mySqlCmd.CommandText = "sp_add_acctgroup"
            ElseIf ViewState("AcctcodeState") = "Edit" Then
                ' intParentId = CType(Session("RefCode"), String)
                mySqlCmd = New SqlCommand("sp_mod_acctgroup", mySqlConn, sqlTrans)
                mySqlCmd.CommandText = "sp_mod_acctgroup"
            End If

            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@acctcode", SqlDbType.VarChar, 20)).Value = txtAccCode.Value.ToString.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@acctname", SqlDbType.VarChar, 100)).Value = txtAccName.Value.ToString.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@parentid", SqlDbType.Int)).Value = intParentId
            mySqlCmd.Parameters.Add(New SqlParameter("@childid", SqlDbType.Int)).Value = intChildId
            mySqlCmd.Parameters.Add(New SqlParameter("@accttype", SqlDbType.Int)).Value = intAcctType
            mySqlCmd.Parameters.Add(New SqlParameter("@acctlevel", SqlDbType.Int)).Value = intAccLevel
            mySqlCmd.Parameters.Add(New SqlParameter("@acctorder", SqlDbType.Int)).Value = intAcctOrder
            If strControl = "Y" Then
                mySqlCmd.Parameters.Add(New SqlParameter("@control", SqlDbType.VarChar, 1)).Value = "Y"
                mySqlCmd.Parameters.Add(New SqlParameter("@custorsupp", SqlDbType.VarChar, 1)).Value = ddlCustSupp.Value
            Else
                mySqlCmd.Parameters.Add(New SqlParameter("@control", SqlDbType.VarChar, 1)).Value = "N"
                mySqlCmd.Parameters.Add(New SqlParameter("@custorsupp", SqlDbType.VarChar, 1)).Value = DBNull.Value
            End If

            If strbankyn = "Y" Then
                mySqlCmd.Parameters.Add(New SqlParameter("@bankyn", SqlDbType.VarChar, 1)).Value = "Y"
                mySqlCmd.Parameters.Add(New SqlParameter("@bank_master_type_code", SqlDbType.VarChar, 20)).Value = ddlBankType.Value
                mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = TxtCurrencyCode.Text.Trim 'ddlCurrency.Value
            Else
                mySqlCmd.Parameters.Add(New SqlParameter("@bankyn", SqlDbType.VarChar, 1)).Value = "N"
                mySqlCmd.Parameters.Add(New SqlParameter("@bank_master_type_code", SqlDbType.VarChar, 20)).Value = DBNull.Value
                mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
            End If


            mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
            mySqlCmd.Parameters.Add(New SqlParameter("@lvlno", SqlDbType.Int)).Value = 0
            If intacctbsorder = 0 Then
                mySqlCmd.Parameters.Add(New SqlParameter("@acctbsorder", SqlDbType.Int)).Value = DBNull.Value
            Else
                mySqlCmd.Parameters.Add(New SqlParameter("@acctbsorder", SqlDbType.Int)).Value = intacctbsorder
            End If
            mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)

            mySqlCmd.ExecuteNonQuery()

            If ViewState("AcctcodeState") = "New" Then
                mySqlCmd = New SqlCommand("sp_add_accrep", mySqlConn, sqlTrans)
                mySqlCmd.CommandText = "sp_add_accrep"
            ElseIf ViewState("AcctcodeState") = "Edit" Then
                mySqlCmd = New SqlCommand("sp_mod_accrep", mySqlConn, sqlTrans)
                mySqlCmd.CommandText = "sp_mod_accrep"
            End If

            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@level1", SqlDbType.Int)).Value = intLevel1
            mySqlCmd.Parameters.Add(New SqlParameter("@level2", SqlDbType.Int)).Value = intLevel2
            mySqlCmd.Parameters.Add(New SqlParameter("@level3", SqlDbType.Int)).Value = intLevel3
            mySqlCmd.Parameters.Add(New SqlParameter("@level4", SqlDbType.Int)).Value = intLevel4
            mySqlCmd.Parameters.Add(New SqlParameter("@level5", SqlDbType.Int)).Value = intLevel5
            mySqlCmd.Parameters.Add(New SqlParameter("@level6", SqlDbType.Int)).Value = intLevel6
            mySqlCmd.Parameters.Add(New SqlParameter("@level7", SqlDbType.Int)).Value = intLevel7
            mySqlCmd.Parameters.Add(New SqlParameter("@acccode", SqlDbType.VarChar, 20)).Value = txtAccCode.Value.ToString.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@accname", SqlDbType.VarChar, 100)).Value = txtAccName.Value.ToString.Trim
            mySqlCmd.Parameters.Add(New SqlParameter("@acctlevel", SqlDbType.Int)).Value = intAccLevel
            If ViewState("AcctcodeState") = "Edit" Then
                mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
            End If
            mySqlCmd.Parameters.Add(New SqlParameter("@divcode", SqlDbType.VarChar, 10)).Value = CType(ViewState("divcode"), String)
            mySqlCmd.ExecuteNonQuery()

            sqlTrans.Commit()
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

            'Response.Redirect("AcctCodesSearch.aspx", False)

            strscript = "window.opener.__doPostBack('AcctcodeWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("AcctCode.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
#Region "Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click"
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        'Response.Redirect("AcctCodesSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnewdiv(Session("dbconnectionName"), "receipt_master_new", "receipt_cashbank_code", CType(txtAccCode.Value.Trim, String), "receipt_div_id", ViewState("divcode")) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for a receipt transaction, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If

        If objUtils.GetDBFieldValueExistnewdiv(Session("dbconnectionName"), "receipt_detail", "receipt_acc_code", CType(txtAccCode.Value.Trim, String), "div_id", ViewState("divcode")) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for a receipt transaction, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If


        If objUtils.GetDBFieldValueExistnewdiv(Session("dbconnectionName"), "journal_detail", "journal_acc_code", CType(txtAccCode.Value.Trim, String), "div_id", ViewState("divcode")) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for a accounts transaction, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If

        If objUtils.GetDBFieldValueExistnewdiv(Session("dbconnectionName"), "acccommon_detail", "code", CType(txtAccCode.Value.Trim, String), "div_id", ViewState("divcode")) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for a accounts openingbalance transaction, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If

        'If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "reservation_invoice_detail", "acc_code", CType(txtAccCode.Value.Trim, String)) = True Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for  Invoice, cannot delete this Code');", True)
        '    checkForDeletion = False
        '    Exit Function
        'End If

        'If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "acctmast", "acctcode", CType(txtAccCode.Value.Trim, String)) = True Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for accounts , cannot delete this Code');", True)
        '    checkForDeletion = False
        '    Exit Function
        'End If

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymast", "accrualacctcode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for Supplier , cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymast", "controlacctcode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for Supplier , cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "agentmast", "controlacctcode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for Customer , cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "agentmast", "postaccount", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for Customer , cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If


        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "profitcentremast", "incomecode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for profit center , cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "profitcentremast", "costcode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for profit center , cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "profitcentremast", "refundincomecode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for profit center , cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "profitcentremast", "refundcostcode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for profit center , cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "profitcentremast", "complcode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for profit center , cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnewdiv(Session("dbconnectionName"), "bankdetails_master", "bankcode", CType(txtAccCode.Value.Trim, String), "div_id", ViewState("divcode")) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for bank master , cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If

        If objUtils.GetDBFieldValueExistnewdiv(Session("dbconnectionName"), "trdpurchase_master", "supcode", CType(txtAccCode.Value.Trim, String), "div_id", ViewState("divcode")) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for debit note , cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If

        If objUtils.GetDBFieldValueExistnewdiv(Session("dbconnectionName"), "open_detail", "acc_code", CType(txtAccCode.Value.Trim, String), "div_id", ViewState("divcode")) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for accounts, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "rmtypmast", "incomecode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for Define room type, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "rmtypmast", "expensecode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for Define room type, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "rmtypmast", "profitcode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for Define room type, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "rmtypmast", "refundincomecode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for Define room type, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "rmtypmast", "refundcostcode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for Define room type, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "othgrpmast", "incomecode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for Define Otherservice type, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "othgrpmast", "expensecode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for Define Otherservice type, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "othgrpmast", "profitcode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for Define Otherservice type, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "othgrpmast", "refundincomecode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for Define Otherservice type, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "othgrpmast", "refundcostcode", CType(txtAccCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already used for Define Otherservice type, cannot delete this Code');", True)
            checkForDeletion = False
            Exit Function
        End If


        checkForDeletion = True
    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=AcctCode','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub ddlBankAc_ServerChange(sender As Object, e As System.EventArgs) Handles ddlBankAc.ServerChange
        'If ddlBankAc.Value = "Yes" Then
        '    banktype.Style.Add("display", "block")
        'Else
        '    banktype.Style.Add("display", "none")
        'End If
    End Sub
End Class
'If Session("State") = "New" Then
'                   SetFocus(txtAccCode)
'                   lblHeading.Text = "Add Account Code"
'                   btnSave.Text = "Save"
'                   btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save')==false)return false;")
'               ElseIf Session("State") = "Edit" Then
'                   txtAccCode.Disabled = True
'                   lblHeading.Text = "Edit Account Code"
'                   btnSave.Text = "Update"
'                   ShowRecord(CType(Session("RefCode"), String))
'                   btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update')==false)return false;")
'               ElseIf Session("State") = "View" Then
'                   lblHeading.Text = "View Account Code"

'                   ShowRecord(CType(Session("RefCode"), String))
'                   txtAccCode.Disabled = True
'                   txtAccName.Disabled = True

'                   ddlBankAc.Disabled = True
'                   ddlControlAc.Disabled = True
'                   ddlCustSupp.Disabled = True
'                   btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete')==false)return false;")

'               ElseIf Session("State") = "Delete" Then
'                   lblHeading.Text = "Delete Account Code"
'                   btnSave.Text = "Delete"
'                   ShowRecord(CType(Session("RefCode"), String))
'                   txtAccCode.Disabled = True
'                   txtAccName.Disabled = True

'                   ddlBankAc.Disabled = True
'                   ddlControlAc.Disabled = True
'                   ddlCustSupp.Disabled = True
'                   btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete')==false)return false;")
'               End If