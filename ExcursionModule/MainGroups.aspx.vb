Imports System.Data
Imports System.Data.SqlClient
Partial Class ExcursionModule_MainGroups
    Inherits System.Web.UI.Page

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
        If Page.IsPostBack = False Then


            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                ViewState.Add("MainGroupState", Request.QueryString("State"))
                ViewState.Add("MainGroupRefCode", Request.QueryString("RefCode"))
                


                'If ViewState("MainGroupState") = "New" Then
                If ViewState("MainGroupState") = "New" Then

                    SetFocus(txtCode)
                    lblHeading.Text = "Add New Excursion Main Group"
                    Page.Title = Page.Title + " " + "Excursion Main Group"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")

                ElseIf ViewState("MainGroupState") = "Edit" Then

                    SetFocus(txtName)
                    lblHeading.Text = "Edit Excursion Main Group"
                    Page.Title = Page.Title + " " + "Edit Excursion Main Group"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("MainGroupRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("MainGroupState") = "View" Then

                    SetFocus(btnCancel)
                    lblHeading.Text = "View Excursion Main Group"
                    Page.Title = Page.Title + " " + "View Excursion Main Group"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()

                    ShowRecord(CType(ViewState("MainGroupRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('View')")
                ElseIf ViewState("MainGroupState") = "Delete" Then

                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Excursion Main Group"
                    Page.Title = Page.Title + " " + "Delete Excursion Main Group"
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("MainGroupRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")

                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")



            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("MainGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        End If
    End Sub
#End Region

#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        If ViewState("MainGroupState") = "View" Or ViewState("MainGroupState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True

            chkActive.Disabled = True
        ElseIf ViewState("MainGroupState") = "Edit" Then
            txtCode.Disabled = True
        End If

    End Sub

#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim result1 As Integer = 0
        Dim frmmode As String = 0

        Try

            If Page.IsValid = True Then

                If ViewState("MainGroupState") = "New" Or ViewState("MainGroupState") = "Edit" Then
                    If checkForDuplicate() = False Then
                        Exit Sub

                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("MainGroupState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_excMainGroup", mySqlConn, sqlTrans)

                    ElseIf ViewState("MainGroupState") = "Edit" Then

                        mySqlCmd = New SqlCommand("sp_mod_excMainGroup", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@groupcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@groupname", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                   

                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("MainGroupState") = "Delete" Then

                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_excMainGroup", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@groupcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)

                

                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('MainGroupWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MainGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("Select * from othmaingrpmast Where othmaingrpcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("othmaingrpcode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("othmaingrpcode"), String)

                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("othmaingrpname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("othmaingrpname"), String)
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
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MainGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("CurrenciesSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

   
#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "othgrpmast", "othmaingrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This maingroupcode is already used for excursion group code, cannot delete this group code');", True)
            checkForDeletion = False
            Exit Function
        End If

        checkForDeletion = True
    End Function
#End Region




#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean

        If ViewState("MainGroupState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "othmaingrpmast", "othmaingrpcode", txtCode.Value.Trim) Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This group code is already present.');", True)
                SetFocus(txtCode)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "othmaingrpmast", "othmaingrpname", txtName.Value.Trim) Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This group name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("MainGroupState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "othmaingrpmast", "othmaingrpcode", "othmaingrpname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This group name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        End If

        checkForDuplicate = True
    End Function
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ' Response.Write("<script language='javascript'> nw=window.open('../Help.aspx?hi=Currency','_blank','status=1,scrollbars=1,top=54,left=760,width=250,height=600'); </script>")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Main Groups','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

End Class
