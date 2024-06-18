'------------================--------------=======================------------------================
'   Module Name    :    User Master.aspx
'   Developer Name :    sandeep indulkar
'   Date           :    
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Imports System.Collections.Generic
#End Region

Partial Class UserMaster
    Inherits System.Web.UI.Page

    <System.Web.Script.Services.ScriptMethod()> _
          <System.Web.Services.WebMethod()> _
    Public Shared Function Getdeptslist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim deptnames As New List(Of String)
        Dim divid As String = ""
        Try

          

            strSqlQry = "select Deptcode,DeptName from DeptMaster  where active=1 and deptname like  '" & Trim(prefixText) & "%' order by Deptcode "
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    deptnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("DeptName").ToString(), myDS.Tables(0).Rows(i)("Deptcode").ToString()))

                Next
            End If
            Return deptnames
        Catch ex As Exception
            Return deptnames
        End Try

    End Function
    <System.Web.Script.Services.ScriptMethod()> _
        <System.Web.Services.WebMethod()> _
    Public Shared Function Getusergrpslist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim usergrpnames As New List(Of String)
        Dim divid As String = ""
        Try



            strSqlQry = "select groupname,groupid from groupmaster  where active=1 and  groupname like  '" & Trim(prefixText) & "%' order by groupid "
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    usergrpnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("groupname").ToString(), myDS.Tables(0).Rows(i)("groupid").ToString()))

                Next
            End If
            Return usergrpnames
        Catch ex As Exception
            Return usergrpnames
        End Try

    End Function
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim objDate As New clsDateTime
    Dim strGroupId As String
    Dim objUser As New clsUser
    Dim ValidImageSize As Boolean = True
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            txtSignature.Visible = False
            Try
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ViewState.Add("UserMastState", Request.QueryString("State"))
                ViewState.Add("UserMastRefCode", Request.QueryString("RefCode"))



                chkresstatus.Checked = False

                If ViewState("UserMastState") = "New" Then
                    SetFocus(txtUserCode)
                    lblHeading.Text = "Add New User Master"
                    btnSave.Text = "Save"
                    DisableControl()
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")

                ElseIf ViewState("UserMastState") = "Edit" Then
                    SetFocus(txtUserCode)
                    lblHeading.Text = "Edit User Master"
                    btnSave.Text = "Update"

                    ShowRecord(CType(ViewState("UserMastRefCode"), String))
                    DisableControl()
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")

                ElseIf ViewState("UserMastState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View User Master"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"

                    ShowRecord(CType(ViewState("UserMastRefCode"), String))
                    DisableControl()
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                ElseIf ViewState("UserMastState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete User Master"
                    btnSave.Text = "Delete"

                    ShowRecord(CType(ViewState("UserMastRefCode"), String))
                    DisableControl()
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")


                btnViewimage.Attributes.Add("onclick", "return PopUpImageView('" & txtimg.Value & "')")


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("UserMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else


        End If


    End Sub
#End Region

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("UserMastState") = "View" Or ViewState("UserMastState") = "Delete" Then
            TxtdeptName.Enabled = False 'ddlDepartmentCode.Disabled = True
            'ddlDepartmentName.Disabled = True
            TxtusergrpName.Enabled = False 'ddlgroupcode.Disabled = True
            TxtDesignation.Disabled = True
            TxtEmailID.Disabled = True
            txtMobile.Disabled = True
            TxtPassword.Enabled = False
            TxtRPassword.Enabled = False
            txtMobile1.Disabled = True
            txtMobile2.Disabled = True
            'txtSignature.Disabled = True
            txtUserCode.Disabled = True
            TxtUserName.Disabled = True
            chkActive.Disabled = True
            chkresstatus.Disabled = True
            FileUpload1.Enabled = False
            Btnremove.Enabled = False
            UserImage.Enabled = False

            ' btnBrowse.Visible = False
        ElseIf ViewState("UserMastState") = "Edit" Then
            txtUserCode.Disabled = True
        End If
        If UserImage.FileName = "" And txtimg.Value = "" Then
            btnViewimage.Visible = False
            Btnremove.Visible = False
        End If
    End Sub

#End Region

    Private Sub SaveImage(ByVal strpath_logo1 As String)
        Dim strpath1 As String
        strpath1 = Server.MapPath("UploadImage/" & strpath_logo1)
        UserImage.PostedFile.SaveAs(strpath1)
        txtimg.Value = strpath_logo1
        hdnFileName.Text = txtimg.Value
    End Sub
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strpath_logo1 As String
        Try

            If Page.IsValid = True Then
                If ViewState("UserMastState") = "New" Or ViewState("UserMastState") = "Edit" Then
                    If checkForDuplicate() = False Then
                        If txtUserCode.Value <> "" Then
                            txtimg.Value = txtUserCode.Value & "_" & UserImage.FileName
                        Else
                            txtimg.Value = UserImage.FileName
                        End If
                        Exit Sub
                    End If
                    If Validation() = False Then
                        If UserImage.FileName <> "" And ValidImageSize Then
                            If txtUserCode.Value <> "" Then
                                txtimg.Value = txtUserCode.Value & "_" & UserImage.FileName
                            Else
                                txtimg.Value = UserImage.FileName
                            End If
                        End If
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("UserMastState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_usermaster", mySqlConn, sqlTrans)
                    ElseIf ViewState("UserMastState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_usermaster", mySqlConn, sqlTrans)
                    End If

                    If UserImage.FileName <> "" Then
                        strpath_logo1 = txtUserCode.Value & "_" & UserImage.FileName
                        SaveImage(strpath_logo1)
                    ElseIf ViewState("UserMastState") = "New" And txtimg.Value <> "" Then
                        strpath_logo1 = txtUserCode.Value & "_" + txtimg.Value
                        SaveImage(strpath_logo1)
                    Else
                        txtimg.Value = IIf(txtimg.Value = "", UserImage.FileName, txtimg.Value)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@usercode", SqlDbType.VarChar, 10)).Value = CType(txtUserCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@username", SqlDbType.VarChar, 40)).Value = CType(TxtUserName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userpwd", SqlDbType.VarChar, 10)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.pwdencript('" & CType(TxtPassword.Text.Trim, String) & "')")
                    mySqlCmd.Parameters.Add(New SqlParameter("@groupid", SqlDbType.Int)).Value = CType(TxtusergrpCode.Text, Long)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userdesign", SqlDbType.VarChar, 40)).Value = CType(TxtDesignation.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@deptcode", SqlDbType.VarChar, 10)).Value = CType(TxtdeptCode.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@usemail", SqlDbType.VarChar, 100)).Value = CType(TxtEmailID.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ustel", SqlDbType.VarChar, 100)).Value = CType(txtMobile.Value, String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    If chkresstatus.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@chkresstatus", SqlDbType.Int)).Value = 1
                    ElseIf chkresstatus.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@chkresstatus", SqlDbType.Int)).Value = 0
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = String.Empty

                    If ViewState("UserMastState") = "New" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objDate.GetSystemDateTime(Session("dbconnectionName"))
                    ElseIf ViewState("UserMastState") = "Edit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@moddate", SqlDbType.DateTime)).Value = objDate.GetSystemDateTime(Session("dbconnectionName"))
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If FileUpload1.HasFile = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@usersign", SqlDbType.VarChar, 100)).Value = CType(txtUserCode.Value.Trim, String) & "_" & FileUpload1.FileName
                    ElseIf txtSignature.Text <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@usersign", SqlDbType.VarChar, 100)).Value = txtSignature.Text
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@usersign", SqlDbType.VarChar, 100)).Value = DBNull.Value
                    End If
                    'mySqlCmd.Parameters.Add(New SqlParameter("@usersign", SqlDbType.VarChar, 100)).Value = CType(txtSignature.Value, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@modreason", SqlDbType.VarChar, 200)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@ustel1", SqlDbType.VarChar, 100)).Value = CType(txtMobile1.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ustel2", SqlDbType.VarChar, 100)).Value = CType(txtMobile2.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userimage", SqlDbType.VarChar, 100)).Value = CType(txtimg.Value, String)
                    ''' Added Shahul 27/05/18
                    mySqlCmd.Parameters.Add(New SqlParameter("@emailusername", SqlDbType.VarChar, 100)).Value = CType(txtemailusername.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@emailpwd", SqlDbType.VarChar, 100)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.pwdencript('" & CType(txtemailpassword.Text.Trim, String) & "')")
                    mySqlCmd.ExecuteNonQuery()
                    '-------------------------------------------------------
                    If FileUpload1.HasFile Then
                        Dim strFileName As String
                        Dim strpath As String
                        strFileName = CType(txtUserCode.Value.Trim, String) & "_" & FileUpload1.FileName
                        'If File.Exists(Server.MapPath(".") + "//UploadImage/" + strFileName) = True Then
                        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('File alredy exist. Select other file name.');", True)
                        '    SetFocus(FileUpload1)
                        '    Exit Sub
                        'End If
                        strpath = Server.MapPath("UploadImage\" & strFileName)
                        FileUpload1.PostedFile.SaveAs(Server.MapPath("UploadImage\" & strFileName))
                        'txtSignature.Value = FileUpload1.FileName
                    End If

                ElseIf ViewState("UserMastState") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("sp_del_usermaster", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@usercode", SqlDbType.VarChar, 10)).Value = CType(txtUserCode.Value.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@deldate", SqlDbType.DateTime)).Value = objDate.GetSystemDateTime(Session("dbconnectionName"))
                    'mySqlCmd.Parameters.Add(New SqlParameter("@delreason", SqlDbType.VarChar, 200)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)

                'connection close

                If CType(Session("GlobalUserName"), String) = CType(txtUserCode.Value.Trim, String) And ViewState("UserMastState") = "Edit" Then

                    Session.Remove("Userpwd")
                    Session.Add("Userpwd", CType(TxtPassword.Text.Trim, String))
                End If
                'Response.Redirect("UserMasterSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('UserMastWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            objUtils.WritErrorLog("UserMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select usercode,active,usersign,username,ustel,dbo.pwddecript(userpwd) userpwd,groupid,userdesign,deptcode,usemail,isnull(resstatus,0) resstatus,plgrpcode,ustel1,ustel2,userimage, " _
                                      & " dbo.pwddecript(emailpwd) emailpwd,emailusername from UserMaster Where usercode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("usercode")) = False Then
                        Me.txtUserCode.Value = CType(mySqlReader("usercode"), String)
                    Else
                        Me.txtUserCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("username")) = False Then
                        Me.TxtUserName.Value = CType(mySqlReader("username"), String)
                    Else
                        Me.TxtUserName.Value = ""
                    End If


                    If IsDBNull(mySqlReader("userpwd")) = False Then
                        TxtPassword.Attributes.Add("value", CType(mySqlReader("userpwd"), String))
                        TxtRPassword.Attributes.Add("value", CType(mySqlReader("userpwd"), String))
                        ' Me.TxtPassword.Text = CType(mySqlReader("userpwd"), String)
                        Me.TxtPassword.TextMode = TextBoxMode.Password
                    Else
                        Me.TxtPassword.Text = ""
                    End If
                    If IsDBNull(mySqlReader("groupid")) = False Then
                        TxtusergrpCode.Text = CType(mySqlReader("groupid"), String)

                        TxtusergrpName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "groupmaster", "groupname", "groupid", CType(mySqlReader("groupid"), String))
                    Else
                        Me.TxtusergrpCode.Text = ""
                        Me.TxtusergrpName.Text = ""
                    End If

                    If IsDBNull(mySqlReader("userdesign")) = False Then
                        Me.TxtDesignation.Value = CType(mySqlReader("userdesign"), String)
                    Else
                        Me.TxtDesignation.Value = ""
                    End If
                    If IsDBNull(mySqlReader("deptcode")) = False Then
                        TxtdeptCode.Text = mySqlReader("deptcode")
                        TxtdeptName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "DeptMaster", "DeptName", "Deptcode", CType(mySqlReader("deptcode"), String))
                    End If

                    If IsDBNull(mySqlReader("usemail")) = False Then
                        Me.TxtEmailID.Value = CType(mySqlReader("usemail"), String)
                    Else
                        Me.TxtEmailID.Value = ""
                    End If

                    If IsDBNull(mySqlReader("ustel")) = False Then
                        Me.txtMobile.Value = CType(mySqlReader("ustel"), String)
                    Else
                        Me.txtMobile.Value = ""
                    End If

                    If IsDBNull(mySqlReader("usersign")) = False Then
                        Me.txtSignature.Text = CType(mySqlReader("usersign"), String)
                    Else
                        Me.txtSignature.Text = ""
                    End If

                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If
                End If
                If IsDBNull(mySqlReader("resstatus")) = False Then
                    If CType(mySqlReader("resstatus"), String) = "1" Then
                        chkresstatus.Checked = True
                    ElseIf CType(mySqlReader("resstatus"), String) = "0" Then
                        chkresstatus.Checked = False
                    End If
                End If
                'If IsDBNull(mySqlReader("plgrpcode")) = False Then
                '    ddlMarketNM.Value = mySqlReader("plgrpcode")
                '    ddlmarketCD.Items(ddlmarketCD.SelectedIndex).Text = ddlMarketNM.Value
                'End If

                If IsDBNull(mySqlReader("ustel1")) = False Then
                    Me.txtMobile1.Value = CType(mySqlReader("ustel1"), String)
                Else
                    Me.txtMobile1.Value = ""
                End If

                If IsDBNull(mySqlReader("ustel2")) = False Then
                    Me.txtMobile2.Value = CType(mySqlReader("ustel2"), String)
                Else
                    Me.txtMobile2.Value = ""
                End If

                If IsDBNull(mySqlReader("userimage")) = False Then
                    Me.txtimg.Value = CType(mySqlReader("userimage"), String)
                Else
                    Me.txtimg.Value = ""
                End If

                '' Added shahul 27/05/18
                If IsDBNull(mySqlReader("emailpwd")) = False Then
                    txtemailpassword.Attributes.Add("value", CType(mySqlReader("emailpwd"), String))
                    Me.txtemailpassword.TextMode = TextBoxMode.Password
                Else
                    Me.txtemailpassword.Text = ""
                End If

                If IsDBNull(mySqlReader("emailusername")) = False Then
                    Me.txtemailusername.Value = CType(mySqlReader("emailusername"), String)
                Else
                    Me.txtemailusername.Value = ""
                End If

            End If

        Catch ex As Exception
            objUtils.WritErrorLog("UserMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("UserMasterSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean

        If ViewState("UserMastState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "UserMaster", "UserCode", CType(txtUserCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This user code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "UserMaster", "UserName", TxtUserName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This user name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("UserMastState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "UserMaster", "UserCode", "UserName", TxtUserName.Value.Trim, CType(txtUserCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This user name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region

#Region "Private Function Validation() As Boolean"
    Private Function Validation() As Boolean
        Try
            If TxtPassword.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(' Password field can not be blank.');", True)
                ' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + TxtPassword.ClientID + "');", True)
                Validation = False
                Exit Function
            End If
            If TxtRPassword.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Re-enter Password field can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "FocusScr", "WebForm_AutoFocus('" + TxtRPassword.ClientID + "');", True)
                Validation = False
                Exit Function
            End If
            'If FileUpload1.HasFile = False And txtSignature.Text = "" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Browse Image File.');", True)
            '    SetFocus(FileUpload1)
            '    Validation = False
            '    Exit Function
            'End If

            If UserImage.FileName <> "" Then
                If System.Drawing.Image.FromStream(UserImage.PostedFile.InputStream).Height <> 400 Or System.Drawing.Image.FromStream(UserImage.PostedFile.InputStream).Width <> 400 Then

                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select an Image With  (400px X 400px) Dimensions');", True)
                    SetFocus(txtimg)
                    ValidImageSize = False
                    Validation = False

                    Exit Function
                End If
            End If

            If TxtPassword.Text = TxtRPassword.Text Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Correct Password');", True)
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Re-enter same Password');", True)
                ' objUtils.MessageBox("Please Re-enter your password.", Me.Page)
                Validation = False
                Exit Function
            End If

            'If ddlmarketCD.Value = "[Select]" Or ddlMarketNM.Value = "[select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Select the Market');", True)
            '    ' objUtils.MessageBox("Please Re-enter your password.", Me.Page)
            '    Validation = False
            '    Exit Function

            'End If
            Validation = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Error Description: - " & ex.Message & "');", True)
            objUtils.WritErrorLog("UserMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=UserMaster','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub Btnremove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnremove.Click
        txtimg.Value = ""
    End Sub
End Class
