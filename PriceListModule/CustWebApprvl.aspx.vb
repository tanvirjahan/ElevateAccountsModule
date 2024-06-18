Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient


Partial Class CustWebApprvl
    Inherits System.Web.UI.Page
    Private Connection As SqlConnection
    Dim objUser As New clsUser
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objEmail As New clsEmail
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim myDataAdapter As New SqlDataAdapter
    Dim ddlPcode As DropDownList


#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RefCode As String
        If IsPostBack = False Then
            Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)
            Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("PriceListModule\CustomersSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))

            PanelWebApproval.Visible = True
            charcters(txtCustomerCode)
            charcters(txtCustomerName)
            GetValuesForWebApprv()
            If CType(Session("custState"), String) = "New" Then
                SetFocus(txtCustomerCode)
                lblHeading.Text = "Add New Customer - Web Approval"
                Page.Title = Page.Title + " " + "New Customer - Web Approval"
                BtnWebAppSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save customer web approval?')==false)return false;")
            ElseIf CType(Session("custState"), String) = "Edit" Then

                BtnWebAppSave.Text = "Update"

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblHeading.Text = "Edit Customer - Web Approval"
                Page.Title = Page.Title + " " + "Edit Customer - Web Approval"
                BtnWebAppSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update customer web approval?')==false)return false;")
            ElseIf CType(Session("custState"), String) = "View" Then

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                DisableControl()
                lblHeading.Text = "View Customer - Web Approval"
                Page.Title = Page.Title + " " + "View Customer - Web Approval"
                BtnWebAppSave.Visible = False
                BtnWebAppCancel.Text = "Return to Search"
                BtnWebAppCancel.Focus()

            ElseIf CType(Session("custState"), String) = "Delete" Then

                RefCode = CType(Session("custrefcode"), String)
                ShowRecord(RefCode)
                txtCustomerCode.Disabled = True
                txtCustomerName.Disabled = True
                lblHeading.Text = "Delete Customer - Web Approval"
                Page.Title = Page.Title + " " + "Delete Customer - Web Approval"
                BtnWebAppSave.Text = "Delete"
                BtnWebAppSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete customer web approval?')==false)return false;")
            End If
            BtnWebAppCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Dim typ As Type
            typ = GetType(DropDownList)

            If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            End If
        End If
        Session.Add("submenuuser", "CustomersSearch.aspx")
    End Sub
#End Region

#Region "Protected Sub BtnWebAppCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnWebAppCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Response.Redirect("CustomersSearch.aspx")
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

#Region "Protected Sub BtnWebAppSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnWebAppSave.Click"

    Protected Sub BtnWebAppSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnWebAppSave.Click
        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strusPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")

        Try
            If Page.IsValid = True Then
                If Session("custState") = "New" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                    Exit Sub
                ElseIf Session("custState") = "Edit" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("custState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_updateweb_agentmast", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf Session("custState") = "Edit" Then

                        mySqlCmd = New SqlCommand("sp_updateweb_agentmast", mySqlConn, sqlTrans)
                        frmmode = 2


                        Dim mySqlCmdAmend As SqlCommand
                        mySqlCmdAmend = New SqlCommand("execute sp_add_agentmast_history '" & CType(txtCustomerCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                        mySqlCmdAmend.CommandType = CommandType.Text
                        mySqlCmdAmend.ExecuteNonQuery()

                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@webusername ", SqlDbType.VarChar, 20)).Value = CType(txtWebAppUsername.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@webpassword ", SqlDbType.VarChar, 10)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.pwdencript('" & CType(txtWebAppPassword.Value.Trim, String) & "')")
                    mySqlCmd.Parameters.Add(New SqlParameter("@webcontact", SqlDbType.VarChar, 100)).Value = CType(txtWebAppContact.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@webemail", SqlDbType.VarChar, 100)).Value = CType(txtWebAppEmail.Value.Trim, String)
                    If ChkWebApprove.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@webapprove", SqlDbType.Int, 4)).Value = 0
                    ElseIf ChkWebApprove.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@webapprove", SqlDbType.Int, 4)).Value = 1
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@mode", SqlDbType.Int, 9)).Value = 1

                    mySqlCmd.ExecuteNonQuery()

                ElseIf Session("custState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    frmmode = 3



                    Dim mySqlCmdAmend As SqlCommand
                    mySqlCmdAmend = New SqlCommand("execute sp_add_agentmast_history '" & CType(txtCustomerCode.Value.Trim, String) & "'", mySqlConn, sqlTrans)
                    mySqlCmdAmend.CommandType = CommandType.Text
                    mySqlCmdAmend.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_agentmast", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                    mySqlCmd = New SqlCommand("delete from agentmast where agentcode='" & txtCustomerCode.Value & "'", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.Text
                    mySqlCmd.ExecuteNonQuery()
                End If
                strPassQry = ""

            

               
                'result1 = strPassQry


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
            objUtils.WritErrorLog("Customers.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub
#End Region



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
                End If
                mySqlCmd.Dispose()
                mySqlReader.Close()
                mySqlConn.Close()
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustWebApprvl.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        txtWebAppUsername.Disabled = True
        txtWebAppPassword.Disabled = True
        txtWebAppContact.Disabled = True
        txtWebAppEmail.Disabled = True
        ChkWebApprove.Disabled = True

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

#Region "Private Sub GetValuesForWebApprv()"
    Private Sub GetValuesForWebApprv()
        Try
            'If session("custState") = "Edit" Or session("custState") = "Edit" Or session("custState") = "Delete" Then
            If Session("custState") = "Edit" Or Session("custState") = "View" Or Session("custState") = "Delete" Then
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from agentmast Where agentcode='" & Session("custrefcode") & "'", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    If mySqlReader.Read() = True Then
                        '---------  Web Approval ------------------------------------
                        If IsDBNull(mySqlReader("webusername")) = False Then
                            txtWebAppUsername.Value = mySqlReader("webusername")
                        Else
                            txtWebAppUsername.Value = ""
                        End If
                        If IsDBNull(mySqlReader("webpassword")) = False Then
                            txtWebAppPassword.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.pwddecript('" & mySqlReader("webpassword") & "')")
                        Else
                            txtWebAppPassword.Value = ""
                        End If
                        If IsDBNull(mySqlReader("webcontact")) = False Then
                            txtWebAppContact.Value = mySqlReader("webcontact")
                        Else
                            txtWebAppContact.Value = ""
                        End If
                        If IsDBNull(mySqlReader("webemail")) = False Then
                            txtWebAppEmail.Value = mySqlReader("webemail")
                        Else
                            txtWebAppEmail.Value = ""
                        End If
                        If IsDBNull(mySqlReader("webapprove")) = False Then
                            If CType(mySqlReader("webapprove"), String) = "1" Then
                                ChkWebApprove.Checked = True
                            ElseIf CType(mySqlReader("webapprove"), String) = "0" Then
                                ChkWebApprove.Checked = False
                            End If
                        End If

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

#Region "Protected Sub BtnShowPassword_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnShowPassword_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

            mySqlCmd = New SqlCommand("GenerateRandomString", mySqlConn, sqlTrans)
            mySqlCmd.CommandType = CommandType.StoredProcedure

            mySqlCmd.Parameters.Add(New SqlParameter("@useNumbers", SqlDbType.Bit)).Value = 1
            mySqlCmd.Parameters.Add(New SqlParameter("@useLowerCase", SqlDbType.Bit)).Value = 0
            mySqlCmd.Parameters.Add(New SqlParameter("@useUpperCase", SqlDbType.Bit)).Value = 1
            mySqlCmd.Parameters.Add(New SqlParameter("@charactersToUse", SqlDbType.VarChar, 100)).Value = System.DBNull.Value
            mySqlCmd.Parameters.Add(New SqlParameter("@passwordLength", SqlDbType.SmallInt, 9)).Value = 7

            Dim param As SqlParameter
            param = New SqlParameter
            param.ParameterName = "@password"
            param.Direction = ParameterDirection.Output
            param.DbType = DbType.String
            param.Size = 50
            mySqlCmd.Parameters.Add(param)
            myDataAdapter = New SqlDataAdapter(mySqlCmd)
            mySqlCmd.ExecuteNonQuery()
            'lblPwd.Text = param.Value
            txtWebAppPassword.Value = param.Value
            'End If
            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close



        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try


    End Sub
#End Region

#Region "Protected Sub BtnWebInviteCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnWebInviteCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnWebInviteCustomer.Click
        Dim strEmailText As String = ""
        Dim strfooterText As String = ""
        Dim strSubject As String = ""
        Dim to_email As String = ""
        'Dim from_email As String = ""
        Dim strcc As String = ""
        Dim strFromEmailID As String = ""
        Dim tocc As String = ""

        Dim strEmailText1 As String = ""
        Dim strEmailText_cc1 As String = ""
        Dim strSubject1 As String = ""
        Dim strfootertext1 As String = ""

        Try
            If ChkWebApprove.Checked = True Then

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                mySqlCmd = New SqlCommand("Select * from email_text", mySqlConn)
                mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If mySqlReader.HasRows Then
                    If mySqlReader.Read() = True Then
                        If IsDBNull(mySqlReader("emailtext")) = False Then
                            strEmailText = CType(mySqlReader("emailtext"), String)
                        End If
                        If IsDBNull(mySqlReader("footertext")) = False Then
                            strfooterText = CType(mySqlReader("footertext"), String)
                        End If
                        If IsDBNull(mySqlReader("fromemailid")) = False Then
                            strFromEmailID = CType(mySqlReader("fromemailid"), String)
                        End If
                        If IsDBNull(mySqlReader("subject")) = False Then
                            strSubject = CType(mySqlReader("subject"), String)
                        End If
                    End If
                End If

                If strEmailText <> "" And strSubject <> "" Then
                    Dim Mail_Message As New MailMessage()
                    Dim msClient As New SmtpClient
                    strcc = "Select option_selected from reservation_parameters where param_id =1006"
                    Dim ds1 As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strcc)
                    If ds1.Tables.Count > 0 Then
                        If ds1.Tables(0).Rows.Count > 0 Then
                            If IsDBNull(ds1.Tables(0).Rows(0)(0)) = False Then
                                tocc = CType(ds1.Tables(0).Rows(0)(0), String)
                            End If
                        End If
                    End If

                    If txtWebAppEmail.Value.Trim <> "" Then

                        strEmailText1 = ""
                        strEmailText1 = strEmailText
                        'strEmailText_cc1 = strEmailText_cc
                        strSubject1 = strSubject
                        strfootertext1 = strfooterText

                        strEmailText1 = strEmailText1 + "<br /><br /> "
                        strEmailText1 = strEmailText1 + "User Name : " & txtWebAppUsername.Value.Trim & "<br />"
                        strEmailText1 = strEmailText1 + "Password : " & txtWebAppPassword.Value.Trim & "<br /><br /> "



                        strEmailText1 += "This is to inform you that you are now a registered User with  .  To add any sub users within your organisation and obtain an ID and password for them, this needs to be done by the Main User; please follow the following procedure:"


                        ''strEmailText1 += " To create sub user please do the following steps.  "
                        'strEmailText1 += "<br /> 2-      Go to Admin Menu"
                        'strEmailText1 += "<br /> 3-      Open Create Sub user/Assign right"
                        'strEmailText1 += "<br /> 4-      Select Create Sub user"
                        'strEmailText1 += "<br /> 5-      Enter Sub user name & password"
                        'strEmailText1 += "<br /> 6-      Refer to below Assign Application Rights"
                        'strEmailText1 += "<br /> 7-      Select application right which sub user has to access."
                        ''strEmailText1 += "<br /> 8-      Dear Main User to create a sub user please go online and create the sub users In Jordan & Middle East Country ,<br /> then please to log on same main user name/password to create the sub users at UAE & GCC country, <br />we are apologize for those steps but be sure that we are developing it shortly "

                        'strEmailText1 += " To create sub user please do the following steps.  "
                        strEmailText1 += "<br /><br /> 1-       Log-in to Online System  "
                        strEmailText1 += "<br /> 2-      Go to Admin Menu"
                        strEmailText1 += "<br /> 3-      Open Create Sub user/Assign right"
                        strEmailText1 += "<br /> 4-      Select Create Sub user"
                        strEmailText1 += "<br /> 5-      Enter Sub user name & password"
                        strEmailText1 += "<br /> 6-      Refer to below Assign Application Rights"
                        strEmailText1 += "<br /> 7-      Select application right which sub user has to access."
                        'strEmailText1 += "<br /> 8-      Dear Main User to create a sub user please go online and create the sub users In Jordan & Middle East Country ,<br /> then please to log on same main user name/password to create the sub users at UAE & GCC country, <br />we are apologize for those steps but be sure that we are developing it shortly "


                        ''strEmailText1 += "<br /><br /><br /> Note: Important for sub user for your agency to retrieve booking handler.<br /><br />"
                        'strEmailText1 += "<br /><br /> "
                        'strEmailText1 += "<br /><br />For any technical support on the online system please contact below phone number or email address 24/7<br/>"
                        strEmailText1 += "<br /><br />For any technical support on the online system please contact us on the number or email address below <br/>"
                        strEmailText1 += " phone number: 00962 79 714 7748 <br />"
                        strEmailText1 += " email address :  <br /><br />"

                        'strEmailText1 += "<br /><br /><br /> If you have any query please feel free to contact us."




                        strEmailText1 = strEmailText1 + "<br /> " + strfootertext1

                        'strEmailText1 += "<br /><br /><br />Best Regards & Thanks<br /><br />"
                        strEmailText1 += "<br /><br /><br />Best Regards <br /><br />"
                        strEmailText1 += " <br /><br /> <br /><br /> "


                        'strEmailText = "<html> Dear Mr. " & txtWebAppContact.Value.Trim & "<BR><BR> You are approved to access the   online. Your Userid and password information as below.  <BR><BR> UserID = " & txtWebAppUsername.Value.Trim & "<BR> Password = " & txtWebAppPassword.Value.Trim & "<BR><BR> Thanks & Regards, <BR><BR> </html>"
                        ' from_email = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "Select fromemailid from email_text")
                        'from_email = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='563'")
                        to_email = txtWebAppEmail.Value.Trim

                        If objEmail.SendEmailCC(strFromEmailID, to_email, tocc, strSubject1, strEmailText1) Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "2", "alert('Mail Sent Sucessfully to " + to_email + "');", True)

                            'add code to save to pwd mail log table
                            PwdSendmailLog_Entry(to_email + ";" + tocc, "Customer WebApproval-Invite customer")
                        Else
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "3", "alert('Failed to Send the mail to " + to_email + "');", True)
                        End If
                    Else
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter e-mail id.');", True)
                        SetFocus(txtWebAppEmail)
                        Exit Sub
                    End If
                End If

            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Customer has not been approved!');", True)
                SetFocus(txtWebAppEmail)
                Exit Sub
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Customer.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            'clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            'clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            'clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Protected Sub BtnWebResendPasswprd_Click(ByVal sender As Object, ByVal e As System.EventArgs)"
    Protected Sub BtnWebResendPasswprd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strEmailText As String = ""
        Dim strSubject As String = ""
        Dim from_email As String = ""
        Dim to_email As String = ""
        Dim tocc_email As String = ""
        Dim Mail_Message As New MailMessage()
        Dim msClient As New SmtpClient
        If ChkWebApprove.Checked = True Then


            If txtWebAppEmail.Value.Trim <> "" Then

                strEmailText = "<html> Dear Mr. " & txtWebAppContact.Value.Trim & "<BR><BR> You are approved to access the  online. Your User Id and password information as below.  <BR><BR> User Id = " & txtWebAppUsername.Value.Trim & "<BR> Password = " & txtWebAppPassword.Value.Trim & "<BR><BR> Thanks & Regards, <BR><BR> Elevate Tourism </html>"
                'from_email = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "Select fromemailid from email_text")
                from_email = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1070'")
                to_email = txtWebAppEmail.Value.Trim
                tocc_email = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id='1006'")


                If objEmail.SendEmailCC(from_email, to_email, tocc_email, "Approve for Web", strEmailText) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "2", "alert('Mail Sent Sucessfully to " + to_email + "');", True)

                    'add code to save to pwd mail log table
                    PwdSendmailLog_Entry(to_email + ";" + tocc_email, "Customer WebApproval-Resend Password")
                Else
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "3", "alert('Failed to Send the mail to " + to_email + "');", True)
                End If
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Enter e-mail id.');", True)
                SetFocus(txtWebAppEmail)
                Exit Sub
            End If
        Else
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Customer has not been approved!');", True)
            SetFocus(txtWebAppEmail)
            Exit Sub
        End If
    End Sub
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustWebApprvl','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Private Sub PwdSendmailLog_Entry(ByVal prm_stremails As String, ByVal prm_pagename As String)
        Try

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            sqlTrans = mySqlConn.BeginTransaction


            mySqlCmd = New SqlCommand("sp_add_PwdMailSend_Log", mySqlConn, sqlTrans)
            mySqlCmd.CommandType = CommandType.StoredProcedure
            mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCustomerCode.Value.Trim, String)
            mySqlCmd.Parameters.Add(New SqlParameter("@mailDateTime", SqlDbType.DateTime)).Value = CType(System.DateTime.Now, DateTime)
            mySqlCmd.Parameters.Add(New SqlParameter("@mailSendPageName", SqlDbType.VarChar, 200)).Value = prm_pagename.ToString

            mySqlCmd.Parameters.Add(New SqlParameter("@usercode", SqlDbType.VarChar, 20)).Value = CType(Session("GlobalUserName"), String)
            mySqlCmd.Parameters.Add(New SqlParameter("@agent_email", SqlDbType.VarChar, 50)).Value = CType(prm_stremails.Trim, String)

            mySqlCmd.ExecuteNonQuery()

            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            End If
        End Try
    End Sub


    '<WebMethod()> _
    'Public Function UpdateWebApprvl(ByVal constr As String, ByVal custcode As String, ByVal WebAppUsername As String,
    '                ByVal strpwd As String, ByVal WebAppContact As String, ByVal WebAppEmail As String, ByVal WebApprove As Integer,
    '                ByVal mode As Integer, ByVal userlogged As String, ByVal frmmode As String) As String

    '    Dim retlist As New List(Of clsMaster)

    '    If Trim(ConfigurationManager.AppSettings("CustomerStopService")) <> "" Then
    '        UpdateWebApprvl = Trim(ConfigurationManager.AppSettings("CustomerStopService"))
    '        Exit Function
    '    End If

    '    Dim ClientIP As String = Context.Request.ServerVariables("REMOTE_ADDR")


    '    If objUtils.ExecuteQueryReturnStringValuenew(constr, "select 1 from service_userpermissions  where userip='" & ClientIP & "' and active=1") <> "1" Then
    '        UpdateWebApprvl = "Permission Denied"
    '        Exit Function
    '    End If
    '    Dim result_temp As String

    '    result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "delete from tempservicesave_agentmast")

    '    Dim result As Integer
    '    Dim sqlstr As String

    '    Dim spname As String
    '    Dim p As Integer
    '    Dim parms As New List(Of SqlParameter)
    '    Dim parm(8) As SqlParameter
    '    parm(8) = New SqlParameter
    '    parm(0) = New SqlParameter("@agentcode", CType(custcode, String))
    '    parm(1) = New SqlParameter("@webusername", CType(WebAppUsername, String))

    '    parm(2) = New SqlParameter("@webpassword", CType(strpwd.Trim, String))
    '    parm(3) = New SqlParameter("@webcontact", CType(WebAppContact.Trim, String))
    '    parm(4) = New SqlParameter("@webemail", CType(WebAppEmail.Trim, String))
    '    parm(5) = New SqlParameter("@webapprove", CType(WebApprove.Trim, Integer))
    '    parm(6) = New SqlParameter("@userlogged", CType(Session("GlobalUserName"), String))
    '    parm(7) = New SqlParameter("@mode", CType(mode, Integer))

    '    If frmmode = 1 Then
    '        For p = 0 To 7
    '            parms.Add(parm(p))
    '        Next
    '        spname = "sp_updateweb_agentmast"
    '        result_temp = objUtils.ExecuteQueryReturnSingleValuenew(constr, "insert into  tempservicesave_agentmast select * from agentmast where agentcode='" & CType(custcode, String) & "'")

    '    End If

    '    If frmmode = 2 Then

    '        For p = 0 To 20
    '            parms.Add(parm(p))
    '        Next
    '        spname = "sp_updateweb_agentmast"
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
