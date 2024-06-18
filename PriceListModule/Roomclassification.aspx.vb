Imports System.Data
Imports System.Data.SqlClient
Partial Class PriceListModule_Roomclassification
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
                ViewState.Add("RoomTypeClass", Request.QueryString("State"))
                ViewState.Add("RoomTypeRefCode", Request.QueryString("RefCode"))

                'If ViewState("RoomTypeClass") = "New" Then
                If ViewState("RoomTypeClass") = "New" Then

                    SetFocus(txtCode)
                    lblHeading.Text = "Add New Room classification"
                    Page.Title = Page.Title + " " + "New Room classification Master"
                    btnSave.Text = "Save"

                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save currency?')==false)return false;")
                    ' btnSave.Attributes.Add("onclick", "return ValidationForExchate('New')")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("RoomTypeClass") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Room classification"
                    Page.Title = Page.Title + " " + "Edit Room classification"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("RoomTypeRefCode"), String))
                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update currency?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("RoomTypeClass") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Room classification"
                    Page.Title = Page.Title + " " + "View Room classification"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("RoomTypeRefCode"), String))
                ElseIf ViewState("RoomTypeClass") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Room classification"
                    Page.Title = Page.Title + " " + "Delete Room classification Master"
                    btnSave.Text = "Delete"
                    DisableControl()
                    ShowRecord(CType(ViewState("RoomTypeRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete currency?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("Currencies.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else

        End If
        Page.Title = "RoomClassification Entry"
    End Sub
#End Region

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("RoomTypeClass") = "View" Or ViewState("RoomTypeClass") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            chkActive.Disabled = True
        ElseIf ViewState("RoomTypeClass") = "Edit" Then
            txtCode.Disabled = True
        End If

    End Sub
#End Region
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"
        Try
            If Page.IsValid = True Then
                'If ViewState("RoomTypeClass") = "New" Or ViewState("RoomTypeClass") = "Edit" Then
                If ViewState("RoomTypeClass") = "New" Or ViewState("RoomTypeClass") = "Edit" Then
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    'SQL  Trans start
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                    sqlTrans = mySqlConn.BeginTransaction
                    If ViewState("RoomTypeClass") = "New" Then
                        Dim optionval As String
                        optionval = objUtils.GetAutoDocNo("ROOMCLASS", mySqlConn, sqlTrans)
                        txtCode.Value = optionval.Trim
                    End If

                    If ViewState("RoomTypeClass") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_roomclassification", mySqlConn, sqlTrans)
                    ElseIf ViewState("RoomTypeClass") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_roomclassification", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@roomclasscode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@roomclassname", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("RoomTypeClass") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("sp_del_roomclassification", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@roomclasscode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If
                strPassQry = ""
                '                result1 = strPassQry
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("CurrenciesSearch.aspx", False)

                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('CurrWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Roomclassification.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try





    End Sub

#End Region
#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))          'connection open
            mySqlCmd = New SqlCommand("Select * from room_classification Where roomclasscode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("roomclasscode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("roomclasscode"), String)

                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("roomclassname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("roomclassname"), String)
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
            objUtils.WritErrorLog("Roomclassification.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If ViewState("RoomTypeClass") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "room_classification", "roomclasscode", txtCode.Value.Trim) Then
                'objUtils.MessageBox("This currency code is already present.", Me.Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This roomclass code is already present.');", True)
                SetFocus(txtCode)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "room_classification", "roomclassname", txtName.Value.Trim) Then
                'objUtils.MessageBox("This currency name is already present.", Me.Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This roomclass name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("RoomTypeClass") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "room_classification", "roomclasscode", "roomclassname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                'objUtils.MessageBox("This currency name is already present.", Me.Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This roomclass name is already present.');", True)
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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Roomclass','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub



End Class
