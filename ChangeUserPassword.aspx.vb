Imports System.Data
Imports System.Data.SqlClient

Partial Class ChangeUserPassword
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objUser As New clsUser
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim mySqlCmd As SqlCommand
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
        If Page.IsPostBack = False Then
           
            SetFocus(txtOriginalPassword)

            If Not (Request.QueryString("UserCode") Is Nothing) Then
                txtUserCode.Text = Request.QueryString("UserCode")
                txtUserName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "usermaster", "UserName", "UserCode", txtUserCode.Text)
            End If
        End If
    End Sub
#End Region

    Private Function ValidateChangePwd() As Boolean
        ValidateChangePwd = True
        
        If txtUserCode.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Cannot Proceed -User Code can not be empty!');", True)

            ValidateChangePwd = False
            Exit Function
        End If

        If txtOriginalPassword.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Original Password field can not be blank.');", True)

            ValidateChangePwd = False
            Exit Function
        End If
        If txtNewPassword.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('New Password field can not be blank.');", True)

            ValidateChangePwd = False
            Exit Function
        End If
        If txtReNewPassword.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Re-Type New Password field can not be blank.');", True)

            ValidateChangePwd = False
            Exit Function
        End If
        'username and old password is correct

        If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from UserMaster where UserCode ='" & txtUserCode.Text & "' and dbo.pwddecript (userpwd)='" & txtOriginalPassword.Text & "'") = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Username and Original Password Does not Match!');", True)

            ValidateChangePwd = False
            Exit Function
        End If
        'check new password and retype is same
        If txtNewPassword.Text <> txtReNewPassword.Text Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('New Password and ReType Password should be same.');", True)

            ValidateChangePwd = False
            Exit Function
        End If

    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try

            If Page.IsValid = True Then
                 
                If ValidateChangePwd() = False Then
                    Exit Sub
                End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction

                mySqlCmd = New SqlCommand("sp_user_ChangePassword", mySqlConn, sqlTrans)

                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@usercode", SqlDbType.VarChar, 10)).Value = CType(txtUserCode.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@userpwd", SqlDbType.VarChar, 100)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.pwdencript('" & CType(txtNewPassword.Text.Trim, String) & "')")
                mySqlCmd.ExecuteNonQuery()

                'insert to log table
                mySqlCmd = New SqlCommand("sp_add_password_change_log", mySqlConn, sqlTrans)

                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@usercode", SqlDbType.VarChar, 10)).Value = CType(txtUserCode.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@originalpwd", SqlDbType.VarChar, 100)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.pwdencript('" & CType(txtOriginalPassword.Text.Trim, String) & "')")
                mySqlCmd.Parameters.Add(New SqlParameter("@changepwd", SqlDbType.VarChar, 100)).Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select dbo.pwdencript('" & CType(txtNewPassword.Text.Trim, String) & "')")
                mySqlCmd.Parameters.Add(New SqlParameter("@addDate", SqlDbType.DateTime)).Value = CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select GETDATE ()"), DateTime)

                mySqlCmd.ExecuteNonQuery()

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('You can now log in with your new password.');window.close();", True)

            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            'objUtils.WritErrorLog("UserMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.close();", True)
    End Sub
End Class
