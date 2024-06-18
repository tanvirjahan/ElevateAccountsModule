Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Partial Class PriceListModule_CustCreditLimits
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

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region
#Region "   Private Sub GetValuesForcreditlimitDetails()"
    Private Sub GetValuesForcreditlimitDetails()
        Try
            'If session("custState") = "Edit" Then
            If Session("custState") = "Edit" Or Session("custState") = "View" Or Session("custState") = "Delete" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from agentmast_creditlimits Where agentcode='" & Session("custrefcode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    If mySqlReader.Read() = True Then
                        '---------  General Details ------------------------------------
                        If IsDBNull(mySqlReader("creditdays")) = False Then
                            txtcreditdays.Value = mySqlReader("creditdays")
                        Else
                            txtcreditdays.Value = ""
                        End If

                        If IsDBNull(mySqlReader("creditlimit")) = False Then
                            txtcreditlimit.Value = mySqlReader("creditlimit")
                        Else
                            txtcreditlimit.Value = ""
                        End If

                        If IsDBNull(mySqlReader("gracecrdays")) = False Then
                            txtgrcreditdays.Value = mySqlReader("gracecrdays")
                        Else
                            txtgrcreditdays.Value = ""
                        End If

                        If IsDBNull(mySqlReader("gracecrlimit")) = False Then
                            txtgrlimit.Value = mySqlReader("gracecrlimit")
                        Else
                            txtgrlimit.Value = ""
                        End If

                        If CType(mySqlReader("enforcecrlimit"), Integer) = 1 Then
                            chkencrlimit.Checked = True
                        Else
                            chkencrlimit.Checked = False
                        End If
                        If CType(mySqlReader("allowcancelperiod"), Integer) = 1 Then
                            chkcancel.Checked = True
                        Else
                            chkcancel.Checked = False
                        End If

                        If IsDBNull(mySqlReader("blocked")) = False Then
                            If CType(mySqlReader("blocked"), Integer) = 1 Then
                                ddlblocked.SelectedIndex = 1
                            ElseIf CType(mySqlReader("blocked"), Integer) = 0 Then
                                ddlblocked.SelectedIndex = 2
                            End If
                        End If
                    Else
                        ddlblocked.SelectedIndex = 0
                    End If
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Sub
#End Region
#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region
#Region " Private Sub DisableControl()"
    Private Sub DisableControl()
        txtcreditdays.Disabled = True
        txtcreditlimit.Disabled = True
        txtgrcreditdays.Disabled = True
        txtgrlimit.Disabled = True
        chkencrlimit.Enabled = False
        chkcancel.Enabled = False
        ddlblocked.Enabled = False
    End Sub
#End Region
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        txtcreditdays.Focus()
        If IsPostBack = False Then
            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            ' changed Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
            If CType(Request.QueryString("appid"), String) = "1" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

            ElseIf CType(Request.QueryString("appid"), String) = "11" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomerGroupSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
            ElseIf CType(Request.QueryString("appid"), String) = "4" Or CType(Request.QueryString("appid"), String) = "14" Then
                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=1", String), "1")
            End If


                PanelGeneral.Visible = True

                charcters(txtCustomerCode)
                charcters(txtCustomerName)
                Numbers(txtcreditdays)
                Numbers(txtcreditlimit)
                Numbers(txtgrcreditdays)
                Numbers(txtgrlimit)
                GetValuesForcreditlimitDetails()

                If CType(Session("custState"), String) = "New" Then
                    SetFocus(txtCustomerCode)
                    lblHeading.Text = "Add New Customer - General"
                    Page.Title = Page.Title + " " + "New Customer - General"
                    BtnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save customer general?')==false)return false;")
                ElseIf CType(Session("custState"), String) = "Edit" Then

                    BtnSave.Text = "Update"

                    RefCode = CType(Session("custrefcode"), String)
                    ShowRecord(RefCode)
                    txtCustomerCode.Disabled = True
                    txtCustomerName.Disabled = True
                    lblHeading.Text = "Edit Customer - Credit Limits and Blocking"
                    Page.Title = Page.Title + " " + "Edit Customer - CreditLimits and Blocking"
                    BtnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update Customer Credit Limits?')==false)return false;")
                ElseIf CType(Session("custState"), String) = "View" Then

                    RefCode = CType(Session("custrefcode"), String)
                    ShowRecord(RefCode)
                    txtCustomerCode.Disabled = True
                    txtCustomerName.Disabled = True
                    DisableControl()
                    lblHeading.Text = "View Customer - CreditLimits and Blocking"
                    Page.Title = Page.Title + " " + "View Customer - CreditLimits and Blocking"
                    BtnSave.Visible = False
                    BtnCancel.Text = "Return to Search"
                    BtnCancel.Focus()

                ElseIf CType(Session("custState"), String) = "Delete" Then

                    RefCode = CType(Session("custrefcode"), String)
                    ShowRecord(RefCode)
                    txtCustomerCode.Disabled = True
                    txtCustomerName.Disabled = True
                    DisableControl()
                    lblHeading.Text = "Delete Customer - CreditLimits and Blocking"
                    Page.Title = Page.Title + " " + "Delete Customer - CreditLimits and Blocking"
                    BtnSave.Text = "Delete"
                    BtnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete customer Credit Limits?')==false)return false;")
                End If
                BtnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

            End If
            Session.Add("submenuuser", "CustomersSearch.aspx")
    End Sub

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustCredit','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

#Region "Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select agentcode,agentname,active from agentmast Where agentcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("agentcode")) = False Then
                        Me.txtCustomerCode.Value = mySqlReader("agentcode")
                    End If
                    If IsDBNull(mySqlReader("agentname")) = False Then
                        Me.txtCustomerName.Value = mySqlReader("agentname")
                    End If
                    If CType(mySqlReader("active"), Integer) = 1 Then
                        lblstatus.Text = "Active"
                    Else
                        lblstatus.Text = "In Active"
                    End If
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustCreditLimits.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            If mySqlConn.State = ConnectionState.Open Then mySqlConn.Close()
        End Try
    End Sub
#End Region

#Region "Public Function checkForcrdays As Boolean"
    Public Function checkForcrdays() As Boolean
        If chkencrlimit.Checked Then
            If CType(txtcreditdays.Value, String) = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Credit Days');", True)
                SetFocus(txtcreditdays)
                checkForcrdays = False
                Exit Function
                checkForcrdays = False
                Exit Function
            End If
            If CType(txtcreditlimit.Value, String) = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Credit Limit');", True)
                SetFocus(txtcreditlimit)
                checkForcrdays = False
                Exit Function
                checkForcrdays = False
                Exit Function
            End If
        End If
        If CType(txtcreditdays.Value, String) = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Credit Days');", True)
            SetFocus(txtcreditdays)
            checkForcrdays = False
            Exit Function
        End If
        If CType(txtcreditlimit.Value, String) = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Credit Limit');", True)
            SetFocus(txtcreditlimit)
            checkForcrdays = False
            Exit Function
        End If
        If CType(txtgrcreditdays.Value, String) <> "" Then
            If CType(txtgrlimit.Value, String) = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Grace Credit Limit.');", True)
                SetFocus(txtgrlimit)
                checkForcrdays = False
                Exit Function
            End If
        End If
        If CType(txtgrlimit.Value, String) <> "" Then
            If CType(txtgrcreditdays.Value, String) = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Grace Credit Days.');", True)
                SetFocus(txtgrcreditdays)
                checkForcrdays = False
                Exit Function
            End If
        End If
        checkForcrdays = True
    End Function
#End Region
#Region " Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click"
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"

        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")

        Try
            If Page.IsValid = True Then
                If Session("custState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("custState") = "Edit" Then


                    If checkForcrdays() = False Then ' To be Checked
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("custState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_mod_agentmast_crlimit", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf Session("custState") = "Edit" Then

                        mySqlCmd = New SqlCommand("sp_mod_agentmast_crlimit", mySqlConn, sqlTrans)
                        frmmode = 2



                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    If txtcreditdays.Value <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@creditdays", SqlDbType.Int)).Value = CType(txtcreditdays.Value, Integer)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@creditdays", SqlDbType.Int)).Value = DBNull.Value
                    End If

                    If txtcreditlimit.Value <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@creditlimit", SqlDbType.Decimal)).Value = CType(txtcreditlimit.Value, Decimal)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@creditlimit", SqlDbType.Decimal)).Value = DBNull.Value
                    End If
                    If txtgrcreditdays.Value <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@gracecrdays", SqlDbType.Int)).Value = CType(txtgrcreditdays.Value, Integer)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@gracecrdays", SqlDbType.Int)).Value = DBNull.Value
                    End If
                    If txtgrlimit.Value <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@gracecrlimit", SqlDbType.Decimal)).Value = CType(txtgrlimit.Value, Decimal)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@gracecrlimit", SqlDbType.Decimal)).Value = DBNull.Value
                    End If
                    If chkencrlimit.Checked Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@enforcecrlimit", SqlDbType.Int)).Value = 1
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@enforcecrlimit", SqlDbType.Int)).Value = 0
                    End If

                    If (CType(ddlblocked.SelectedItem.Text, String) <> "[Select]") Then
                        If (CType(ddlblocked.SelectedItem.Text, String) = "YES") Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@blocked", SqlDbType.Int)).Value = 1
                        ElseIf (CType(ddlblocked.SelectedItem.Text, String) = "NO") Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@blocked", SqlDbType.Int)).Value = 0
                        End If
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@blocked", SqlDbType.Int)).Value = DBNull.Value

                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    If chkcancel.Checked Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@allowcancelperiod", SqlDbType.Int)).Value = 1
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@allowcancelperiod", SqlDbType.Int)).Value = 0
                    End If

                    mySqlCmd.ExecuteNonQuery()
                ElseIf Session("custState") = "Delete" Then
                    ''If checkForDeletion() = False Then
                    ''    Exit Sub
                    ''End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction


                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_del_agentmast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                End If
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
                ' Response.Redirect("CustomersSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If


        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()



            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustGen.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region
    Protected Sub ddlblocked_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlblocked.SelectedIndexChanged

    End Sub
#Region "Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('CustomersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

    End Sub
#End Region


End Class
