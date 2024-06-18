Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.IO
Imports System.Collections.Generic

Partial Class CustAcctDet
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection
    Dim objUser As New clsUser
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction

#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        If IsPostBack = False Then
            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            'changed Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
            If CType(Request.QueryString("appid"), String) = "1" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

                elseif CType(Request.QueryString("appid"), String) = "11" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomerGroupSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "4" Or CType(Request.QueryString("appid"), String) = "14" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=1", String), "1")

                End If

          

                PanelAccounts.Visible = True

                charcters(txtCustomerCode)
                charcters(txtCustomerName)
            ' GetValuesForAccountDetails()
            Dim acclist As List(Of String) = Getpostacclist("")
            If (acclist.Count > 0) Then
                If (acclist(0).Split(",").Length > 0) Then
                    TxtPostAccName.Text = acclist(0).Split(",")(0).Replace("{", "").Replace(ControlChars.Quote, "").Replace("First:", "")
                End If
            End If

                If CType(Session("custState"), String) = "New" Then
                    SetFocus(txtCustomerCode)
                    lblHeading.Text = "Add New Customer - Account Details"
                    Page.Title = Page.Title + " " + "New Customer - Account Details"
                    BtnAccSave.Attributes.Add("onclick", "return FormValidationMainDetail('New')")
                    'BtnAccSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Customer?')==false)return false;")
                ElseIf CType(Session("custState"), String) = "Edit" Then

                    BtnAccSave.Text = "Update"

                    RefCode = CType(Session("custrefcode"), String)
                    ShowRecord(RefCode)
                    'If CType(Session("ExistClient"), String) = "1" Then
                    '    BtnAccSave.Visible = False
                    '    DisableControl()
                    'End If
                    txtCustomerCode.Disabled = True
                    txtCustomerName.Disabled = True
                    lblHeading.Text = "Edit Customer - Account Details"
                    Page.Title = Page.Title + " " + "Edit Customer  -Account Details"
                    BtnAccSave.Attributes.Add("onclick", "return FormValidationMainDetail('Edit')")
                    'BtnAccSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Customer?')==false)return false;")
                ElseIf CType(Session("custState"), String) = "View" Then

                    RefCode = CType(Session("custrefcode"), String)
                    ShowRecord(RefCode)
                    txtCustomerCode.Disabled = True
                    txtCustomerName.Disabled = True
                    DisableControl()
                    lblHeading.Text = "View Customer - Account Details"
                    Page.Title = Page.Title + " " + "View Customer - Account Details"
                    BtnAccSave.Visible = False
                    BtnAccCancel.Text = "Return to Search"
                    BtnAccCancel.Focus()

                ElseIf CType(Session("custState"), String) = "Delete" Then

                    RefCode = CType(Session("custrefcode"), String)
                    ShowRecord(RefCode)
                    txtCustomerCode.Disabled = True
                    txtCustomerName.Disabled = True
                    DisableControl()
                    lblHeading.Text = "Delete Customer - Account Details"
                    Page.Title = Page.Title + " " + "Delete Customer - Account Details"
                    BtnAccSave.Text = "Delete"
                    BtnAccSave.Attributes.Add("onclick", "return FormValidationMainDetail('Delete')")
                    'BtnAccSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Customer?')==false)return false;")
                End If
            BtnAccCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Dim divid As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select divcode from agentmast  where agentcode='" & CType(txtCustomerCode.Value, String) & "'")
            Session.Add("divcode", divid)
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                End If
            End If
            Session.Add("submenuuser", "CustomersSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnAccCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnAccCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Response.Redirect("CustomersSearch.aspx")
        If Session("postback") <> "Addclient" Then
            Dim strscript As String = ""
            strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
        Else

            Dim strscript As String = ""
            strscript = "window.close();"
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)


        End If
    End Sub
#End Region

    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function Getcontrolacclist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Controlaccnames As New List(Of String)
        Dim divid As String = ""
        Try

            If Not HttpContext.Current.Session("divcode") Is Nothing Then
                divid = Convert.ToString(HttpContext.Current.Session("divcode").ToString())
            End If

            If divid = "" Then
                strSqlQry = "select acctname,acctcode from acctmast where upper(controlyn)='Y'  and cust_supp='C'  and  acctname like  '" & Trim(prefixText) & "%'"
            Else
                strSqlQry = "select acctname,acctcode from acctmast where div_code='" & divid & "' and upper(controlyn)='Y'  and cust_supp='C'  and  acctname like  '" & Trim(prefixText) & "%'"
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
                    Controlaccnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Controlaccnames
        Catch ex As Exception
            Return Controlaccnames
        End Try
    End Function
    <System.Web.Script.Services.ScriptMethod()> _
   <System.Web.Services.WebMethod()> _
    Public Shared Function Getbankacclist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Controlaccnames As New List(Of String)
        Dim divid As String = ""
        Try
            If Not HttpContext.Current.Session("divcode") Is Nothing Then
                divid = Convert.ToString(HttpContext.Current.Session("divcode").ToString())
            End If

            If divid = "" Then
                strSqlQry = "select acctcode,acctname from acctmast where upper(BankYN)='Y'  and acctcode in(select bankcode from bankdetails_master where active=1)  and  acctname like  '" & Trim(prefixText) & "%' order by acctcode"
            Else
                strSqlQry = "select acctcode,acctname from acctmast where div_code='" & divid & "' and upper(BankYN)='Y'  and acctcode in(select bankcode from bankdetails_master where active=1 and div_id='" & divid & "')  and  acctname like  '" & Trim(prefixText) & "%' order by acctcode"
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
                    Controlaccnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Controlaccnames
        Catch ex As Exception
            Return Controlaccnames
        End Try
    End Function
    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function Getpostacclist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Controlaccnames As New List(Of String)
        Dim divid As String = ""
        Try
            If Not HttpContext.Current.Session("divcode") Is Nothing Then
                divid = Convert.ToString(HttpContext.Current.Session("divcode").ToString())
            End If

            If divid = "" Then
                strSqlQry = "select acctname,acctcode from acctmast where cust_supp='C'  and  acctname like  '" & Trim(prefixText) & "%'"
            Else
                strSqlQry = "select acctname,acctcode from acctmast where div_code='" & divid & "' and  cust_supp='C' and controlYn='Y' and  acctname like  '" & Trim(prefixText) & "%'"
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
                    Controlaccnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("acctname").ToString(), myDS.Tables(0).Rows(i)("acctcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Controlaccnames
        Catch ex As Exception
            Return Controlaccnames
        End Try
    End Function
#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If Session("custState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "agentmast", "agentcode", CType(txtCustomerCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "agentmast", "agentname", txtCustomerName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf Session("custState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "agentmast", "agentcode", "agentname", txtCustomerName.Value.Trim, CType(txtCustomerCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region
#Region " Protected Sub BtnAccSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAccSave.Click"
    Protected Sub BtnAccSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAccSave.Click

        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim mode As String = ""
        Dim strPassQry As String = "false"
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")
     
        Try
            If Page.IsValid = True Then
               

                If Session("custState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("custState") = "Edit" Then

                    '-----------    Validate Page   ---------------
                    If TxtControlAccCode.Text.Trim = "" Or TxtControlAccName.Text.Trim = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Control A/C Code field can not be blank.');", True)
                        SetFocus(TxtControlAccName)
                        Exit Sub
                    End If
                    If TxtBankAccCode.Text.Trim = "" Or TxtBankAccName.Text.Trim = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Bank A/C Code field can not be blank.');", True)
                        SetFocus(TxtBankAccName)
                        Exit Sub
                    End If
                    'If TxtPostAccCode.Text.Trim = "" Or TxtPostAccName.Text.Trim = "" Then
                    '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Post A/C Code field can not be blank.');", True)
                    '    SetFocus(TxtPostAccName)
                    '    Exit Sub
                    'End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("custState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updateacc_agentmast", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf Session("custState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_updateacc_agentmast", mySqlConn, sqlTrans)
                        frmmode = 2

                        'Dim mySqlCmdAmend As SqlCommand
                        'mySqlCmdAmend = New SqlCommand("execute sp_add_agentmast_history '" & CType(txtCustomerCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                        'mySqlCmdAmend.CommandType = CommandType.Text
                        'mySqlCmdAmend.ExecuteNonQuery()

                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@atel1", SqlDbType.VarChar, 50)).Value = CType(txtAccTelephone1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@atel2", SqlDbType.VarChar, 50)).Value = CType(txtAccTelephone2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@amobile", SqlDbType.VarChar, 50)).Value = CType(txtAccMobile.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@afax", SqlDbType.VarChar, 50)).Value = CType(txtAccFax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@acontact1", SqlDbType.VarChar, 100)).Value = CType(txtAccContact1.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@acontact2", SqlDbType.VarChar, 100)).Value = CType(txtAccContact2.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@acontact3", SqlDbType.VarChar, 100)).Value = CType(txtAccContact3.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@acontact4", SqlDbType.VarChar, 100)).Value = CType(txtAccContact4.Value.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@aemail", SqlDbType.VarChar, 100)).Value = CType(txtAccEmail.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@accemail", SqlDbType.VarChar, 500)).Value = CType(txtAcc_ccEmail.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@controlacctcode", SqlDbType.VarChar, 20)).Value = CType(TxtControlAccCode.Text.Trim, String)

                    ' mySqlCmd.Parameters.Add(New SqlParameter("@controlacctcode", SqlDbType.VarChar, 20)).Value = CType(ddlAccCode.Items(ddlAccCode.SelectedIndex).Text.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@invoicebankcode", SqlDbType.VarChar, 20)).Value = IIf(CType(ddlBnkdetail.Items(ddlBnkdetail.SelectedIndex).Text.Trim, String) = "[Select]", "", CType(ddlBnkdetail.Items(ddlBnkdetail.SelectedIndex).Text.Trim, String))
                    If TxtBankAccCode.Text.Trim = "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@invoicebankcode", SqlDbType.VarChar, 20)).Value = ""
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@invoicebankcode", SqlDbType.VarChar, 20)).Value = CType(TxtBankAccCode.Text.Trim, String)
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@crdays", SqlDbType.Int, 4)).Value = CType(Val(TxtAccCreditDays.Value.Trim), Long)
                    mySqlCmd.Parameters.Add(New SqlParameter("@crlimit", SqlDbType.Int, 4)).Value = CType(Val(txtAccCreditLimit.Value.Trim), Long)
                    If ChkCashSup.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@cashclient", SqlDbType.Int, 4)).Value = 0
                    ElseIf ChkCashSup.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@cashclient", SqlDbType.Int, 4)).Value = 1
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@bookingcrlimit", SqlDbType.Money)).Value = CType(Val(txtAccBooking.Value.Trim), Decimal)
                    If TxtPostAccCode.Text.Trim = "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@postaccount", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@postaccount", SqlDbType.VarChar, 20)).Value = CType(TxtPostAccCode.Text.Trim, String)
                    End If

                    'mySqlCmd.Parameters.Add(New SqlParameter("@postaccount", SqlDbType.VarChar, 20)).Value = CType(ddlPostCode.Items(ddlPostCode.SelectedIndex).Text.Trim, String)
                    If ChkAccBooking2.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@bookcrlimitchk", SqlDbType.Int, 4)).Value = 0
                    ElseIf ChkAccBooking2.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@bookcrlimitchk", SqlDbType.Int, 4)).Value = 1
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 500)).Value = CType(txtremarks.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@opinion", SqlDbType.VarChar, 500)).Value = CType(txtclientop.Value, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()

                    ElseIf Session("custState") = "Delete" Then
                        If checkForDeletion() = False Then
                            Exit Sub
                        End If
                        mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                        sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                        frmmode = 3

                        'Dim mySqlCmdAmend As SqlCommand
                        'mySqlCmdAmend = New SqlCommand("execute sp_add_agentmast_history '" & CType(txtCustomerCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                        'mySqlCmdAmend.CommandType = CommandType.Text
                        'mySqlCmdAmend.ExecuteNonQuery()

                        mySqlCmd = New SqlCommand("sp_del_agentmast", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.ExecuteNonQuery()
                    'mySqlCmd = New SqlCommand("delete from agentmast_mulltiemail where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    'mySqlCmd.CommandType = CommandType.Text
                    'mySqlCmd.ExecuteNonQuery()
                    End If

                    strPassQry = ""





                    sqlTrans.Commit()    'SQl Tarn Commit
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                    If Session("custState") = "New" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)
                    ElseIf Session("custState") = "Edit" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                    End If
                    If Session("custState") = "Delete" Then
                        'Response.Redirect("CustomersSearch.aspx", False)
                        Dim strscript As String = ""
                        strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                    End If


                End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()


              
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustAcctDet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region

    Protected Sub TxtControlAccName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtControlAccName.TextChanged
        Session("custmain_controlaccode_for_filter") = TxtControlAccCode.Text
    End Sub
    Protected Sub TxtBankAccName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtBankAccName.TextChanged
        Session("custmain_bankaccode_for_filter") = TxtBankAccCode.Text
    End Sub
    Protected Sub TxtPostAccName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtPostAccName.TextChanged
        Session("custmain_postaccode_for_filter") = TxtPostAccCode.Text
    End Sub


#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from agentmast Where agentcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("agentcode")) = False Then
                        Me.txtCustomerCode.Value = mySqlReader("agentcode")
                    End If
                    If IsDBNull(mySqlReader("agentname")) = False Then
                        Me.txtCustomerName.Value = mySqlReader("agentname")
                    End If
                    '---------  Account Details ------------------------------------
                    If IsDBNull(mySqlReader("atel1")) = False Then
                        txtAccTelephone1.Value = mySqlReader("atel1")
                    Else
                        txtAccTelephone1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("atel1")) = False Then
                        txtAccTelephone1.Value = mySqlReader("atel1")
                    Else
                        txtAccTelephone1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("atel2")) = False Then
                        txtAccTelephone2.Value = mySqlReader("atel2")
                    Else
                        txtAccTelephone2.Value = ""
                    End If

                    If IsDBNull(mySqlReader("amobile")) = False Then
                        txtAccMobile.Value = mySqlReader("amobile")
                    Else
                        txtAccMobile.Value = ""
                    End If

                    If IsDBNull(mySqlReader("afax")) = False Then
                        txtAccFax.Value = mySqlReader("afax")
                    Else
                        txtAccFax.Value = ""
                    End If
                    If IsDBNull(mySqlReader("acontact1")) = False Then
                        txtAccContact1.Value = mySqlReader("acontact1")
                    Else
                        txtAccContact1.Value = ""
                    End If
                    If IsDBNull(mySqlReader("acontact2")) = False Then
                        txtAccContact2.Value = mySqlReader("acontact2")
                    Else
                        txtAccContact2.Value = ""
                    End If
                    If IsDBNull(mySqlReader("acontact3")) = False Then
                        If mySqlReader("acontact3") <> "" Then
                            txtAccContact3.Value = mySqlReader("acontact1")
                        End If

                    Else
                        txtAccContact3.Value = ""
                    End If
                    If IsDBNull(mySqlReader("acontact4")) = False Then
                        txtAccContact4.Value = mySqlReader("acontact4")
                    Else
                        txtAccContact4.Value = ""
                    End If
                    If IsDBNull(mySqlReader("aemail")) = False Then
                        txtAccEmail.Value = mySqlReader("aemail")
                    Else
                        txtAccEmail.Value = ""
                    End If
                    If IsDBNull(mySqlReader("accemail")) = False Then
                        txtAcc_ccEmail.Value = mySqlReader("accemail")
                    Else
                        txtAcc_ccEmail.Value = ""
                    End If
                    'If IsDBNull(mySqlReader("controlacctcode")) = False Then
                    '    ddlAccName.Value = mySqlReader("controlacctcode")
                    '    ddlAccCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", mySqlReader("controlacctcode"))
                    'Else
                    '    ddlAccCode.Value = "[Select]"
                    '    ddlAccName.Value = "[Select]"
                    'End If
                    If IsDBNull(mySqlReader("controlacctcode")) = False Then
                        Me.TxtControlAccName.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname,acctcode from acctmast where upper(controlyn)='Y'  and cust_supp='C'  and acctcode='" & mySqlReader("controlacctcode") & "'")
                        Me.TxtControlAccCode.Text = CType(mySqlReader("controlacctcode"), String)
                    Else
                        Me.TxtControlAccName.Text = ""
                        Me.TxtControlAccName.Text = ""
                    End If
                    If IsDBNull(mySqlReader("invoicebankcode")) = False Then
                        Me.TxtBankAccName.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where upper(BankYN)='Y'  and acctcode in(select bankcode from bankdetails_master where active=1) and cust_supp='C' and acctcode='" & mySqlReader("invoicebankcode") & "'")
                        Me.TxtBankAccCode.Text = CType(mySqlReader("invoicebankcode"), String)
                    Else
                        Me.TxtBankAccName.Text = ""
                        Me.TxtBankAccName.Text = ""
                    End If
       

                    If IsDBNull(mySqlReader("crdays")) = False Then
                        TxtAccCreditDays.Value = mySqlReader("crdays")
                    Else
                        TxtAccCreditDays.Value = ""
                    End If
                    If IsDBNull(mySqlReader("crlimit")) = False Then
                        txtAccCreditLimit.Value = mySqlReader("crlimit")
                    Else
                        txtAccCreditLimit.Value = ""
                    End If

                    If IsDBNull(mySqlReader("cashclient")) = False Then
                        If mySqlReader("cashclient") = 1 Then
                            ChkCashSup.Checked = True
                        Else
                            ChkCashSup.Checked = False
                        End If
                    End If
                    If IsDBNull(mySqlReader("bookingcrlimit")) = False Then
                        txtAccBooking.Value = mySqlReader("bookingcrlimit")
                    Else
                        txtAccBooking.Value = ""
                    End If
                  
                    If IsDBNull(mySqlReader("postaccount")) = False Then
                        Me.TxtPostAccName.Text = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select acctname from acctmast where cust_supp='C'   and acctcode='" & mySqlReader("postaccount") & "'")
                        Me.TxtPostAccCode.Text = CType(mySqlReader("postaccount"), String)
                    Else
                        Me.TxtPostAccName.Text = ""
                        Me.TxtPostAccName.Text = ""
                    End If
                
                    If IsDBNull(mySqlReader("bookcrlimitchk")) = False Then
                        If mySqlReader("bookcrlimitchk") = 1 Then
                            ChkAccBooking2.Checked = True
                        Else
                            ChkAccBooking2.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("agentremarks")) = False Then
                        txtremarks.Value = mySqlReader("agentremarks")
                    Else
                        txtremarks.Value = ""
                    End If

                    If IsDBNull(mySqlReader("openion")) = False Then
                        txtclientop.Value = mySqlReader("openion")
                    Else
                        txtclientop.Value = ""
                    End If


                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustAcctDet.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region
#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region
#Region "telepphone"
    Public Sub telepphone(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkTelephoneNumber(event)")
    End Sub
#End Region

#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        TxtControlAccName.Enabled = False
        TxtPostAccName.Enabled = False
        TxtControlAccCode.Enabled = False
        TxtPostAccCode.Enabled = False
        txtAccTelephone1.Disabled = True
        txtAccTelephone2.Disabled = True
        txtAccMobile.Disabled = True
        txtAccFax.Disabled = True
        txtAccContact1.Disabled = True
        txtAccContact2.Disabled = True
        txtAccEmail.Disabled = True
        TxtAccCreditDays.Disabled = True
        txtAccCreditLimit.Disabled = True

        txtAccContact3.Disabled = True
        txtAccContact4.Disabled = True

        TxtBankAccName.Enabled = False
        txtAcc_ccEmail.Disabled = True
        TxtControlAccCode.Enabled = True
        TxtPostAccCode.Enabled = True
        txtremarks.Disabled = True
        txtclientop.Disabled = True
        ChkAccBooking2.Disabled = True
        txtAccBooking.Disabled = True
        ChkCashSup.Disabled = True

    End Sub
#End Region


#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "earlypromagent_detail", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a DetailsOfEarlyBirdPromotions, cannot delete this Customer');", True)
            checkForDeletion = False
            Exit Function
        End If
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "Promo_agent", "agentcode", CType(txtCustomerCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer is already used for a CustomerPromotions, cannot delete this Customer');", True)
            checkForDeletion = False
            Exit Function
        End If

        checkForDeletion = True
    End Function
#End Region



    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustAcctDet','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    '<WebMethod()> _
    'Public Function UpdateCustGen(ByVal constr As String, ByVal custcode As String, ByVal AccTelephone1 As String,
    '                ByVal AccTelephone2 As String, ByVal AccMobile As String, ByVal AccFax As String, ByVal AccContact1 As String,
    '                 ByVal AccContact2 As String, ByVal AccEmail As String, ByVal Acc_ccEmail As String, ByVal ctrlacctcode As String,
    '                 ByVal AccCreditDays As String, ByVal AccCreditLimit As String, ByVal CashSup As Integer, ByVal AccBooking As Decimal,
    '                 ByVal PostCode As String, ByVal AccBooking2 As Integer, ByVal userlogged As String, ByVal remarks As String,
    '                 ByVal clientop As String, ByVal frmmode As String) As String

    '    'Dim retlist As New List(Of clsMaster)

    '    If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
    '        UpdateCustGen = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
    '        Exit Function
    '    End If

    '    Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


    '    If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
    '        UpdateCustGen = "Permission Denied"
    '        Exit Function
    '    End If
    '    Dim result_temp As String

    '    result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_agentmast")

    '    Dim result As Integer
    '    Dim sqlstr As String

    '    Dim spname As String
    '    Dim p As Integer
    '    Dim parms As New List(Of SqlParameter)
    '    Dim parm(3) As SqlParameter
    '    parm(3) = New SqlParameter
    '    parm(0) = New SqlParameter("@agentcode", CType(custcode, String))
    '    'parm(1) = New SqlParameter("@general", CType(custGeneral, String))

    '    parm(1) = New SqlParameter("@atel1", CType(AccTelephone1.Trim, String))
    '    parm(2) = New SqlParameter("@atel2", CType(AccTelephone2.Trim, String))
    '    parm(3) = New SqlParameter("@amobile", CType(AccMobile.Trim, String))
    '    parm(4) = New SqlParameter("@afax", CType(AccFax.Trim, String))
    '    parm(5) = New SqlParameter("@acontact1", CType(AccContact1.Trim, String))
    '    parm(6) = New SqlParameter("@acontact2", CType(AccContact2.Trim, String))
    '    parm(7) = New SqlParameter("@aemail", CType(AccEmail.Trim, String))
    '    parm(8) = New SqlParameter("@accemail", CType(Acc_ccEmail.Trim, String))

    '    parm(9) = New SqlParameter("@controlacctcode", CType(ctrlacctcode, String))
    '    parm(10) = New SqlParameter("@crdays", CType(AccCreditDays.Trim,Long )
    '    parm(11) = New SqlParameter("@crlimit", CType(AccCreditLimit.Trim, Long))
    '    parm(12) = New SqlParameter("@cashclient", CashSup)

    '    parm(13) = New SqlParameter("@bookingcrlimit", CType(AccBooking, Decimal))
    '    parm(14) = New SqlParameter("@postaccount", CType(PostCode.Trim, String))

    '    parm(15) = New SqlParameter("@bookcrlimitchk", AccBooking2)

    '    parm(16) = New SqlParameter("@userlogged", CType(Session("GlobalUserName"), String))
    '    parm(17) = New SqlParameter("@remarks", CType(remarks, String))
    '    parm(18) = New SqlParameter("@openion", CType(clientop, String))
    '    parm(19) = New SqlParameter("@userlogged", CType(userlogged, String))

    '    If frmmode = 1 Then
    '        For p = 0 To 20
    '            parms.Add(parm(p))
    '        Next
    '        spname = "sp_updateacc_agentmast"
    '        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")

    '    End If

    '    If frmmode = 2 Then

    '        For p = 0 To 20
    '            parms.Add(parm(p))
    '        Next
    '        spname = "sp_updateacc_agentmast"
    '        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")

    '    End If
    '    If frmmode = 3 Then
    '        parms.Add(parm(0))
    '        spname = "sp_del_agentmast"
    '        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")
    '        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesaveagentmast_mulltiemail select * from agentmast_mulltiemail where agentcode='" & CType(custcode, String) & "'")
    '        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast_visit select * from agentmast_visit where agentcode='" & CType(custcode, String) & "'")
    '        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast_survey select * from agentmast_survey   where agentcode='" & CType(custcode, String) & "'")

    '    End If



    '    result = objUtils.ExecuteNonQuerynew(constr, spname, parms)
    '    If result = Nothing Or IsDBNull(result) Then
    '        result = ""

    '    End If

    '    Return result
    'End Function





End Class
