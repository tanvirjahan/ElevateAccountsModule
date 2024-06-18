'------------================--------------=======================------------------================
'   Page Name       :   NarrationTemplate.aspx
'   Developer Name  :   Sandeep Indulkar
'   Date            :   
'   
'------------================--------------=======================------------------================
#Region "Namespace "
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class NarrationTemplate
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
                txtCode.Disabled = True
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ViewState.Add("NarnTempState", Request.QueryString("State"))
                ViewState.Add("NarnTempRefCode", Request.QueryString("RefCode"))
                If ViewState("NarnTempState") = "New" Then
                    SetFocus(txtName)
                    Page.Title = "Add New Narration Template"
                    lblHeading.Text = "Add New Narration Template"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("NarnTempState") = "Edit" Then
                    SetFocus(txtName)
                    Page.Title = "Edit Narration Template"
                    lblHeading.Text = "Edit Narration Template"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("NarnTempRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("NarnTempState") = "View" Then
                    SetFocus(btnCancel)
                    Page.Title = "View Narration Template"
                    lblHeading.Text = "View Narration Template"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("NarnTempRefCode"), String))
                ElseIf ViewState("NarnTempState") = "Delete" Then
                    SetFocus(btnSave)
                    Page.Title = "Delete Narration Template"
                    lblHeading.Text = "Delete Narration Template"
                    btnSave.Text = "Delete"
                    DisableControl()
                    ShowRecord(CType(ViewState("NarnTempRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If
                'charcters(txtCode)
                'charcters1(txtName)
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("NarrationTemplate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region



#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        If ViewState("NarnTempState") = "View" Or ViewState("NarnTempState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Enabled = False
            chkActive.Disabled = True
        ElseIf ViewState("NarnTempState") = "Edit" Then
            txtCode.Disabled = True
        End If

    End Sub

#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                If ViewState("NarnTempState") = "New" Or ViewState("NarnTempState") = "Edit" Then
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("NarnTempState") = "New" Then
                        Dim optionval As String

                        optionval = objUtils.GetAutoDocNo("NARRATION", mySqlConn, sqlTrans)
                        txtCode.Value = optionval.Trim

                        mySqlCmd = New SqlCommand("sp_add_narration", mySqlConn, sqlTrans)
                    ElseIf ViewState("NarnTempState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_narration", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@code", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@narration", SqlDbType.VarChar, 500)).Value = CType(txtName.Text.Trim, String)
                    If ViewState("NarnTempState") = "New" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
                    ElseIf ViewState("NarnTempState") = "Edit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@moddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If

                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("NarnTempState") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_narartion", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@code", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("NarrationTemplateSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('NarrationTempWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            objUtils.WritErrorLog("NarrationTemplate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from narration Where code='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("code")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("code"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("narration")) = False Then
                        Me.txtName.Text = CType(mySqlReader("narration"), String)
                    Else
                        Me.txtName.Text = ""
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
            objUtils.WritErrorLog("NarrationTemplate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("NarrationTemplateSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        Try
            If ViewState("NarnTempState") = "New" Then
                If objUtils.isDuplicatenew(Session("dbconnectionName"), "narration", "code", txtCode.Value.Trim) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already present.');", True)
                    'SetFocus(txtCode)
                    checkForDuplicate = False
                    Exit Function
                End If
            End If
            checkForDuplicate = True
        Catch ex As Exception
            objUtils.WritErrorLog("NarrationTemplate.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Function
#End Region



    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=NarrationTemplate','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
