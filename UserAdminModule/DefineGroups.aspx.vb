'------------================--------------=======================------------------================
'   Page Name       :   DefineGroups.aspx
'   Developer Name  :   Sandeep Indulkar
'   Date            :   10 Aug 2008
'   
'------------================--------------=======================------------------================

Imports System.Data
Imports System.Data.SqlClient

Partial Class DefineGroups
    Inherits System.Web.UI.Page


#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim objdate As New clsDateTime
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If Not Session("CompanyName") Is Nothing Then
                    Me.Page.Title = CType(Session("CompanyName"), String)
                End If

                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ViewState.Add("DefGrpState", Request.QueryString("State"))
                ViewState.Add("DefGrpRefCode", Request.QueryString("RefCode"))
                If ViewState("DefGrpState") = "New" Then
                    SetFocus(txtCode)
                    lblHeading.Text = "Add Define Groups"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("DefGrpState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Define Groups"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("DefGrpRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("DefGrpState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Define Groups"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("DefGrpRefCode"), String))
                ElseIf ViewState("DefGrpState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Define Groups"
                    btnSave.Text = "Delete"
                    DisableControl()
                    ShowRecord(CType(ViewState("DefGrpRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("DefineGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        If Viewstate("DefGrpState") = "View" Or Viewstate("DefGrpState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            chkActive.Disabled = True
        ElseIf Viewstate("DefGrpState") = "Edit" Then
            txtCode.Disabled = True
        End If

    End Sub

#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Page.IsValid = True Then
                If Viewstate("DefGrpState") = "New" Or Viewstate("DefGrpState") = "Edit" Then
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Viewstate("DefGrpState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_groupmaster", mySqlConn, sqlTrans)
                    ElseIf Viewstate("DefGrpState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_groupmaster", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@groupid", SqlDbType.Int, 9)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@groupname", SqlDbType.VarChar, 25)).Value = CType(txtName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("DefGrpState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_Del_groupmaster", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@groupid", SqlDbType.Int, 9)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("DefineGroupSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('UserGrpWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            objUtils.WritErrorLog("DefineGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region


    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "group_app_detail", "groupid", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This GroupId is already used for Assign Aplication Rights,cannot Delete this GroupId');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "group_app_master", "groupid", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This GroupId is already used for Assign Application Rights, cannot Delete this GroupId');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "usermaster", "groupid", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This GroupId is already used for UserMaster, cannot delete this GroupId');", True)
            checkForDeletion = False
            Exit Function
            
           

        End If

        checkForDeletion = True
    End Function
#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from groupmaster Where groupid='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("groupid")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("groupid"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("groupname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("groupname"), String)
                    Else
                        Me.txtName.Value = ""
                    End If

                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            objUtils.WritErrorLog("DefineGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("DefineGroupSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        Try
            If ViewState("DefGrpState") = "New" Then
                If objUtils.isDuplicatenew(Session("dbconnectionName"), "groupmaster", "groupid", txtCode.Value.Trim) Then
                    'objUtils.MessageBox("This currency code is already present.", Me.Page)
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Group ID is already present.');", True)
                    SetFocus(txtCode)
                    checkForDuplicate = False
                    Exit Function
                End If
                If objUtils.isDuplicatenew(Session("dbconnectionName"), "groupmaster", "groupname", txtName.Value.Trim) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Group name is already present.');", True)
                    SetFocus(txtName)
                    checkForDuplicate = False
                    Exit Function
                End If
            ElseIf ViewState("DefGrpState") = "Edit" Then
                If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "groupmaster", "groupid", "groupname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Group name is already present.');", True)
                    SetFocus(txtName)
                    checkForDuplicate = False
                    Exit Function
                End If
            End If
            checkForDuplicate = True
        Catch ex As Exception
            objUtils.WritErrorLog("DefineGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Function
#End Region




    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=DefineGroups','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
