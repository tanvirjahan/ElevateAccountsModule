'------------================--------------=======================------------------================
'   Page Name       :   CustomerEmailText.aspx
'   Developer Name  :   Amit Survase
'   Date            :   4 July 2008
'   
'
'------------================--------------=======================------------------================


Imports System.Data
Imports System.Data.SqlClient
Partial Class CustomersEmailText
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



                Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                Dim AppName As HiddenField = CType(Master.FindControl("hdnAppName"), HiddenField)
                Dim strappid As String = ""
                Dim strappname As String = ""
                If AppId Is Nothing = False Then
                    strappid = AppId.Value
                End If
                If AppName Is Nothing = False Then
                    strappname = AppName.Value
                End If



                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If


                SetFocus(txtSubject)
                lblHeading.Text = "Edit Customer Email Text"
                btnSave.Text = "Update"


                ShowRecord(CType(Session("RefCode"), String))

                btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update Customer Email Text?')==false)return false;else return true;")
                btnSave.Attributes.Add("onclick", "return FormValidation('Edit');")

                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CustomersEmailText.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

            End Try
        End If
    End Sub
#End Region


    

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try

            If Page.IsValid = True Then
                If ValidateEmail() = False Then
                    Exit Sub
                End If
                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                mySqlCmd = New SqlCommand("sp_save_emailText", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure
                mySqlCmd.Parameters.Add(New SqlParameter("@emailtext", SqlDbType.VarChar, 1250)).Value = CType(txtEmailText.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@subject", SqlDbType.VarChar, 500)).Value = CType(txtSubject.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@footertext", SqlDbType.VarChar, 1250)).Value = CType(txtFooterText.Text.Trim, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@fromemailid", SqlDbType.VarChar, 100)).Value = CType(txtFromEmailId.Text.Trim, String)
                mySqlCmd.ExecuteNonQuery()
            End If

            sqlTrans.Commit()    'SQl Tarn Commit
            clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Record Updated Successfully' );", True)

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("CustomersEmailText.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region


#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open

            mySqlCmd = New SqlCommand("Select * from email_text ", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then

                    If IsDBNull(mySqlReader("subject")) = False Then
                        Me.txtSubject.Text = CType(mySqlReader("subject"), String)
                    Else
                        Me.txtSubject.Text = ""
                    End If

                    If IsDBNull(mySqlReader("emailtext")) = False Then
                        Me.txtEmailText.Text = CType(mySqlReader("emailtext"), String)
                    Else
                        Me.txtEmailText.Text = ""
                    End If

                    If IsDBNull(mySqlReader("footertext")) = False Then
                        Me.txtFooterText.Text = CType(mySqlReader("footertext"), String)
                    Else
                        Me.txtFooterText.Text = ""
                    End If
                    If IsDBNull(mySqlReader("fromemailid")) = False Then
                        Me.txtFromEmailId.Text = CType(mySqlReader("fromemailid"), String)
                    Else
                        Me.txtFromEmailId.Text = ""
                    End If
                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("CustomersEmailText.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region


#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("~/MainPage.aspx", False)
    End Sub
#End Region
    Private Function ValidateEmail() As Boolean
        If txtEmailText.Text = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('From Email ID can not be blank.');", True)
            SetFocus(txtEmailText)
            ValidateEmail = False
            Exit Function
        Else
            If EmailValidate(txtFromEmailId.Text.Trim, txtFromEmailId) = False Then
                SetFocus(txtEmailText)
                ValidateEmail = False
                Exit Function
            End If
        End If
        ValidateEmail = True
    End Function
    Private Function EmailValidate(ByVal email As String, ByVal txt As TextBox) As Boolean
        Try
            Dim email1length As Integer
            email1length = Len(email.Trim)
            If email1length > 255 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Email length is too large..please enter valid email exampele(abc@abc.com).');", True)
                SetFocus(txt)
                Me.Page.SetFocus(txt)
                EmailValidate = False
                Exit Function
            Else
                Dim atpos As String
                Dim dotpos As String
                Dim s1 As String
                Dim s As String
                s1 = email
                atpos = s1.LastIndexOf("@")
                dotpos = s1.LastIndexOf(".")
                s = s1.LastIndexOf(".")
                If atpos < 1 Or dotpos < 2 Or s < 4 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid E-Mail Id [e.g abc@abc.com].');", True)
                    SetFocus(txt)
                    EmailValidate = False
                    Exit Function
                Else
                    Dim sp As String()
                    Dim at As String()
                    Dim dot As String()
                    Dim chkcom As String
                    Dim chkyahoo As String
                    Dim test As String
                    Dim t As String
                    sp = s1.Split(".")
                    at = s1.Split("@")
                    chkcom = sp.GetValue(sp.Length() - 1)
                    chkyahoo = at.GetValue(at.Length() - 1)
                    dot = chkyahoo.Split(".")
                    If dot.Length() > 2 Then
                        t = dot.GetValue(dot.Length() - 3)
                        test = sp.GetValue(sp.Length() - 2)
                        If test <> "co" Or chkcom.Length() > 2 Or IsNumeric(t) = True Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid E-Mail Id [e.g abc@abc.com].');", True)
                            SetFocus(txt)
                            EmailValidate = False
                            Exit Function
                        End If
                    Else
                        t = dot.GetValue(dot.Length() - 2)
                        test = sp.GetValue(sp.Length() - 1)
                        If test.Length < 2 Or IsNumeric(t) = True Or IsNumeric(test) = True Then
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Valid E-Mail Id [e.g abc@abc.com].');", True)
                            SetFocus(txt)
                            EmailValidate = False
                            Exit Function
                        End If
                    End If
                End If
            End If
            EmailValidate = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function

    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CustomersEmailText','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class

'If Session("State") = "Edit" Then
'DisableControl()

'ElseIf Session("State") = "View" Then
'    SetFocus(btnCancel)
'    lblHeading.Text = "View Customer Email Text"
'    btnSave.Visible = False
'    btnCancel.Text = "Return to Search"
'    DisableControl()

'    ShowRecord(CType(Session("RefCode"), String))

'  End If

'                btnSave.Attributes.Add("onclick", "return FormValidation()")

'   ValidateOnlyNumber()

'#Region "Private Sub DisableControl()"

'    Private Sub DisableControl()
'        If Session("State") = "View" Then
'            txtSubject.Enabled = False
'            txtEmailText.Enabled = False
'            txtFooterText.Enabled = False
'        End If

'    End Sub

'#End Region

'  If Session("State") = "Edit" Then
'If checkForDuplicate() = False Then
'    Exit Sub
'End If

' If Session("State") = "Edit" Then
'End If
' End If
'  Response.Redirect("CustomersEmailText.aspx", False)
'mySqlCmd = New SqlCommand("Select * from email_text Where emailtext='" & RefCode & "'", mySqlConn)