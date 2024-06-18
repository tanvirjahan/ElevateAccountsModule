Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Color

Partial Class WebAdminModule_HelpText
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Or CType(Session("Userpwd"), String) = "" Then
                    Response.Redirect("~\Login.aspx", False)
                    Exit Sub
                End If
                If Session("State") = "New" Then
                    SetFocus(txtHelpId)
                    lblHeading.Text = "Add New Help Text"
                    btnSave.Text = "Save"
                    ' btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf Session("State") = "Edit" Then
                    SetFocus(FCKeditor2)
                    lblHeading.Text = "Edit Help Text"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(Session("RefCode"), String))
                    '  btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf Session("State") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Help Text"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(Session("RefCode"), String))
                ElseIf Session("State") = "Delete" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "Delete Help Text"
                    btnSave.Text = "Delete"
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(Session("RefCode"), String))
                End If
                '   btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("HelpText.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        End If
    End Sub
    Private Sub DisableControl()
        If Session("State") = "View" Or Session("State") = "Delete" Then
            txtHelpId.ReadOnly = True
            FCKeditor2.Visible = False
            tblHelpText.BorderColor = "Gray"
            tblHelpText.Border = 1
            tblHelpText.CellSpacing = 0
            tblHelpText.CellPadding = 0
        ElseIf Session("State") = "Edit" Then
            txtHelpId.ReadOnly = True
            FCKeditor2.Visible = True
            tblHelpText.Height = 0
            tblHelpText.Width = 0
        End If
    End Sub
    Private Sub ShowRecord(ByVal RefHelpID As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from help_text Where help_id='" & RefHelpID & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("help_id")) = False Then
                        Me.txtHelpId.Text = CType(mySqlReader("help_id"), String)
                    Else
                        Me.txtHelpId.Text = ""
                    End If
                    If Session("State") = "Edit" Then
                        If IsDBNull(mySqlReader("help_text")) = False Then
                            FCKeditor2.Content = mySqlReader("help_text")
                        End If
                    Else
                        If IsDBNull(mySqlReader("help_text")) = False Then
                            TD1.InnerHtml = mySqlReader("help_text")
                        End If
                    End If
                End If
            End If
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        Catch ex As Exception
            objUtils.WritErrorLog("HelpText.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try

            If Page.IsValid = True Then
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~\Login.aspx", False)
                    Exit Sub
                End If
                If checkForDuplicate() = False Then
                    Exit Sub
                End If
                If Session("State") = "New" Or Session("State") = "Edit" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("State") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_help_text", mySqlConn, sqlTrans)
                    ElseIf Session("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_help_text", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@help_id", SqlDbType.VarChar, 100)).Value = CType(txtHelpId.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@help_text", SqlDbType.Text)).Value = FCKeditor2.Content
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 100)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                ElseIf Session("State") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_delete_help_text", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@help_id", SqlDbType.VarChar, 20)).Value = CType(txtHelpId.Text.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                Response.Redirect("HelpTextSearch.aspx", False)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            End If
            objUtils.WritErrorLog("HelpText.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("HelpTextSearch.aspx", False)
    End Sub
    Public Function checkForDuplicate() As Boolean
        If Session("State") = "New" Then
            'If objUtils.isDuplicatenew(Session("dbconnectionName"),"help_text  ", "help_id", help_id, CType(txtHelpId.Text.Trim, String)) Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "help_text", "help_id", CType(txtHelpId.Text.Trim, String), "help_id='" & txtHelpId.Text.Trim & "'") Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This help text is already present.');", True)
                SetFocus(txtHelpId)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
End Class
