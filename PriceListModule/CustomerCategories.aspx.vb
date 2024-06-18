'------------================--------------=======================------------------================
'   Module Name    :    CustomerCategories
'   Developer Name :    D'Silva Azia
'   Date           :    2 July 2008
'   
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class CustomerCategories
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
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
             
                ViewState.Add("CustcatState", Request.QueryString("State"))
                ViewState.Add("CustcatRefCode", Request.QueryString("RefCode"))
                ViewState.Add("CustValue", Request.QueryString("Value"))

                If ViewState("CustcatState") = "New" Then
                    SetFocus(txtCode)
                    lblCustCatHead.Text = "Add New Customer Categories"
                    Page.Title = Page.Title + " " + "New Customer Categories"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")

                ElseIf ViewState("CustcatState") = "Edit" Then
                    txtCode.Visible = True
                    SetFocus(txtCodeName)
                    lblCustCatHead.Text = "Edit Customer Categories"
                    Page.Title = Page.Title + " " + "Edit Customer Categories"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("CustcatRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                    '   btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to Update Customer Category?')==false)return false;")

                ElseIf ViewState("CustcatState") = "View" Then
                    SetFocus(txtCode)
                    lblCustCatHead.Text = "View Customer Categories"
                    Page.Title = Page.Title + " " + "View Customer Categories"
                    btnSave.Visible = False
                    btnCancel.Text = "Return To Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("CustcatRefCode"), String))

                ElseIf ViewState("CustcatState") = "Delete" Then
                    SetFocus(txtCode)
                    lblCustCatHead.Text = "Delete Customer Categories"
                    Page.Title = Page.Title + " " + "Delete Customer Categories"
                    btnSave.Text = "Delete"
                    btnCancel.Text = "Return To Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("CustcatRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Customer Category?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel')==false)return false;")

             
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CustomerCategories.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub

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

#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        If ViewState("CustcatState") = "View" Or ViewState("CustcatState") = "Delete" Then
            txtCode.Disabled = True
            txtCodeName.Disabled = True
         
            chkActive.Disabled = True

        ElseIf ViewState("CustcatState") = "Edit" Then
            txtCode.Disabled = True
            txtCode.Visible = True
          
        End If

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from agentcatmast Where agentcatcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("agentcatcode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("agentcatcode"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("agentcatname")) = False Then
                        Me.txtCodeName.Value = CType(mySqlReader("agentcatname"), String)
                    Else
                        Me.txtCodeName.Value = ""
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
            objUtils.WritErrorLog("CustomerCategories.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

 

  

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If CType(ViewState("CustcatState"), String) = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "agentcatmast", "agentcatcode", CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "agentcatmast", "agentcatname", txtCodeName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf CType(ViewState("CustcatState"), String) = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "agentcatmast", "agentcatcode", "agentcatname", txtCodeName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Customer Name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region


  




    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")
        Try
            If Page.IsValid = True Then
                If ViewState("CustcatState") = "New" Or ViewState("CustcatState") = "Edit" Then
                    ''If ValidatePage() = False Then
                    ''    Exit Sub
                    ''End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("CustcatState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_agentcat", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf ViewState("CustcatState") = "Edit" Then

                        mySqlCmd = New SqlCommand("sp_mod_agentcat", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcatcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcatname", SqlDbType.VarChar, 100)).Value = CType(txtCodeName.Value.Trim, String)
                 
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
       
                    mySqlCmd.ExecuteNonQuery()

                
                ElseIf ViewState("CustcatState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_del_agentcat", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcatcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If
                strPassQry = ""
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("CustomerCategoriesSearch.aspx", False)


                If ViewState("CustValue") = "Addfrom" Then
                    Session.Add("AgentCategoryCode", txtCode.Value)
                    Session.Add("AgentCategoryName", txtCodeName.Value)
                    Dim strscript1 As String = ""
                    strscript1 = "window.opener.__doPostBack('custcatfromWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript1, True)
                End If

                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('CustcatWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

            End If


        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()


            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomerCategories.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub





#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "earlypromagentcat_detail", "agentcatcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This category is already used for a EarlyBirdPromotion Of  CustomerCategory, cannot delete this category');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "agentmast", "catcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This category is already used for a Customers, cannot delete this category');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "Promo_agentcat", "agentcatcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This category is already used for a Promotion Of CustomerCategory, cannot delete this category');", True)
            checkForDeletion = False
            Exit Function



        End If

        checkForDeletion = True
    End Function
#End Region



    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustomerCategories','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("CustomerCategoriesSearch.aspx", False)


        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
End Class
