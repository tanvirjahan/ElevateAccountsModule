Imports System.Data
Imports System.Data.SqlClient
Partial Class ExcursionModule_PaymentModes
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

                ViewState.Add("PaymentMode", Request.QueryString("State"))
                ViewState.Add("PaymentModeRefCode", Request.QueryString("RefCode"))

                If ViewState("PaymentMode") = "New" Then

                    SetFocus(txtCode)
                    lblHeading.Text = "Add New Payment Mode"
                    Page.Title = Page.Title + " " + "Payment Mode"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")

                ElseIf ViewState("PaymentMode") = "Edit" Then

                    SetFocus(txtName)
                    lblHeading.Text = "Edit Payment Mode"
                    Page.Title = Page.Title + " " + "Edit Payment Mode"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("PaymentModeRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("PaymentMode") = "View" Then

                    SetFocus(btnCancel)
                    lblHeading.Text = "View Payment Mode"
                    Page.Title = Page.Title + " " + "View Payment Mode"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()

                    ShowRecord(CType(ViewState("PaymentModeRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('View')")
                ElseIf ViewState("PaymentMode") = "Delete" Then

                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Excursion Main Group"
                    Page.Title = Page.Title + " " + "Delete Payment Mode"
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("PaymentModeRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")

                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")



            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("PaymentModes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        End If
    End Sub
#End Region

#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        If ViewState("PaymentMode") = "View" Or ViewState("PaymentMode") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            chkPayment.Disabled = True
            chkActive.Disabled = True
        ElseIf ViewState("PaymentMode") = "Edit" Then
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

                If ViewState("PaymentMode") = "New" Or ViewState("PaymentMode") = "Edit" Then
                    If checkForDuplicate() = False Then
                        Exit Sub

                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("PaymentMode") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_PaymentGroup", mySqlConn, sqlTrans)

                    ElseIf ViewState("PaymentMode") = "Edit" Then

                        mySqlCmd = New SqlCommand("sp_mod_PaymentMode", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@paycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@payname", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    If chkPayment.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@performa", SqlDbType.Int)).Value = 1
                    ElseIf chkPayment.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@performa", SqlDbType.Int)).Value = 0
                    End If


                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If


                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("PaymentMode") = "Delete" Then

                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_PaymentMode", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@paycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)



                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('PaymentModeWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("PaymentModes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("Select * from paymentmodemaster Where paycode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("paycode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("paycode"), String)

                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("payname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("payname"), String)
                    Else
                        Me.txtName.Value = ""
                    End If
                    If IsDBNull(mySqlReader("profreqd")) = False Then
                        If CType(mySqlReader("profreqd"), String) = "1" Then

                            chkPayment.Checked = True
                        ElseIf CType(mySqlReader("profreqd"), String) = "0" Then
                            chkPayment.Checked = False
                        End If
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
            objUtils.WritErrorLog("PaymentModes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

        If ViewState("PaymentMode") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "paymentmodemaster", "paycode", txtCode.Value.Trim) Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Payment Mode code is already present.');", True)
                SetFocus(txtCode)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "paymentmodemaster", "payname", txtName.Value.Trim) Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Payment Mode name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("PaymentMode") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "paymentmodemaster", "paycode", "payname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then

                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Payment Mode  Name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        End If

        checkForDuplicate = True
    End Function
#End Region

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Payment Mode','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

End Class
