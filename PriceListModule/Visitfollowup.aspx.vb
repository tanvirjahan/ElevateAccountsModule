'------------================--------------=======================------------------================
'   Page Name       :   Visitfollowup.aspx
'   Developer Name  :    sharfudeen
'   Date            :    
'   
'
'------------================--------------=======================------------------================
Imports System.Data
Imports System.Data.SqlClient

Partial Class Visitfollowup
    Inherits System.Web.UI.Page
#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim objdatetime As New clsDateTime
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlAdapter As SqlDataAdapter
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

                'dpcontactDate.txtDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy")
                Dim strtel As String = ""

                txtconnection.Value = Session("dbconnectionName")

                ViewState.Add("State", Request.QueryString("State"))
                ViewState.Add("agentcode", Request.QueryString("agentcode"))
                ViewState.Add("RefCode", Request.QueryString("RefCode"))

                If Not Request.QueryString("RefCode") = Nothing Then
                    txtvisitid.Value = Request.QueryString("RefCode")
                Else
                    txtCode.Value = ""
                End If


                If Not Request.QueryString("agentcode") = Nothing Then
                    txtCode.Value = Request.QueryString("agentcode")
                Else
                    txtCode.Value = ""
                End If

                If Not Request.QueryString("agentname") = Nothing Then
                    txtName.Value = Request.QueryString("agentname")
                Else
                    txtName.Value = ""
                End If

                If Not Session("GlobalUserName") = Nothing Then
                    txtspersoncode.Value = CType(Session("GlobalUserName"), String)
                Else
                    txtspersoncode.Value = ""
                End If

                Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select username from  usermaster where usercode='" + txtspersoncode.Value + "' ")
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(ds.Tables(0).Rows(0)("username")) = False Then
                            txtspersonname.Value = CType(ds.Tables(0).Rows(0)("username"), String)
                        End If
                    End If
                End If

                If ViewState("State") = "New" Then
                    dpVDate.txtDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy")

                    SetFocus(txtCode)
                    lblHeading.Text = "Add Visit Follow Up"
                    btnSave.Text = "Save"

                    DisableControl()
                    ShowRecord(CType(ViewState("RefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")

                ElseIf ViewState("State") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Update Visit Follow Up"
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("RefCode"), String))

                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update currency?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("State") = "Delete" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "Delete Visit Follow Up"
                    btnSave.Text = "Delete"
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("RefCode"), String))

                ElseIf ViewState("State") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Visit Follow Up"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("RefCode"), String))
                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("Visitfollowup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("State") = "View" Then
            dpVDate.Enabled = False
            txtdesc.ReadOnly = True
            txtcontact.ReadOnly = True
            txtaction.ReadOnly = True
            btnSave.Visible = False
        End If

    End Sub

#End Region

#Region "Public Function ValidatePage() As Boolean"
    Public Function ValidatePage() As Boolean
        Try
            If ViewState("State") = "New" Then
                strSqlQry = "select agentcode,visitdate,spersoncode,remarks,cperson,reqaction from agentmast_visit where visitid= '" & txtvisitid.Value & "'"

                Dim ds As DataSet = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), strSqlQry)
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Entry already exist');", True)
                        ValidatePage = False
                        Exit Function
                    End If
                End If
            End If
            ValidatePage = True
        Catch ex As Exception
            objUtils.WritErrorLog("Visitfollowup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Function
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try

            If Page.IsValid = True Then
                If ValidatePage() = False Then
                    Exit Sub
                End If

                Dim regno As String = ""
                If ViewState("State") = "New" Or ViewState("State") = "Edit" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("State") = "New" Then
                        regno = objUtils.GetAutoDocNo("VISITOR", mySqlConn, sqlTrans)

                        mySqlCmd = New SqlCommand("sp_add_visitfollowup", mySqlConn, sqlTrans)
                    ElseIf ViewState("State") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_visitfollowup", mySqlConn, sqlTrans)
                        regno = txtvisitid.Value
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@visitid", SqlDbType.VarChar, 20)).Value = CType(regno, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@agentcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@spersoncode", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    If dpVDate.txtDate.Text = "" = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@visitdate", SqlDbType.DateTime)).Value = CType(objdatetime.ConvertDateromTextBoxToDatabase(dpVDate.txtDate.Text), Date)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@visitdate", SqlDbType.DateTime)).Value = DBNull.Value
                    End If
                    If txtcontact.Text.Trim = "" = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@cperson", SqlDbType.VarChar, 100)).Value = CType(txtcontact.Text.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@cperson", SqlDbType.VarChar, 100)).Value = DBNull.Value
                    End If
                    If txtdesc.Text.Trim = "" = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 1000)).Value = CType(txtdesc.Text.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.VarChar, 1000)).Value = DBNull.Value
                    End If
                    If txtdesc.Text.Trim = "" = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@reqaction", SqlDbType.VarChar, 1000)).Value = CType(txtaction.Text.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@reqaction", SqlDbType.VarChar, 1000)).Value = DBNull.Value
                    End If
                    mySqlCmd.ExecuteNonQuery()
                ElseIf ViewState("State") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("sp_del_visitfollowup", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@visitid", SqlDbType.VarChar, 20)).Value = CType(txtvisitid.Value, String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("CurrenciesSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('VisitfolloupWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Visitfollowup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            strSqlQry = "select agentcode,visitdate,spersoncode,remarks,cperson,reqaction from agentmast_visit where visitid= '" & RefCode & "' and spersoncode='" & CType(Session("GlobalUserName"), String) & "'"


            mySqlCmd = New SqlCommand(strSqlQry, mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then

                    If ViewState("State") = "View" Or ViewState("State") = "Edit" Or ViewState("State") = "Delete" Then


                        If IsDBNull(mySqlReader("visitdate")) = False Then
                            Me.dpVDate.txtDate.Text = CType(mySqlReader("visitdate"), String)
                        Else
                            Me.dpVDate.txtDate.Text = ""
                        End If

                        If IsDBNull(mySqlReader("cperson")) = False Then
                            Me.txtcontact.Text = CType(mySqlReader("cperson"), String)
                        Else
                            Me.txtcontact.Text = ""
                        End If
                        If IsDBNull(mySqlReader("remarks")) = False Then
                            Me.txtdesc.Text = CType(mySqlReader("remarks"), String)
                        Else
                            Me.txtdesc.Text = ""
                        End If
                        If IsDBNull(mySqlReader("reqaction")) = False Then
                            Me.txtaction.Text = CType(mySqlReader("reqaction"), String)
                        Else
                            Me.txtaction.Text = ""
                        End If

                    End If


                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Visitfollowup.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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


    Protected Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ' Response.Write("<script language='javascript'> nw=window.open('../Help.aspx?hi=Currency','_blank','status=1,scrollbars=1,top=54,left=760,width=250,height=600'); </script>")
        '' ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Currency','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
