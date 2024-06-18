'------------================--------------=======================------------------================
'   Page Name       :   DefineGroups.aspx
'   Developer Name  :   Sandeep Indulkar
'   Date            :   
'   
'------------================--------------=======================------------------================
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Globalization

Partial Class DepartmentMaster
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
                ViewState.Add("DepartmentsState", Request.QueryString("State"))
                ViewState.Add("DepartmentsRefCode", Request.QueryString("RefCode"))

                txtDeptcode.Attributes.Add("readonly", "readonly")

                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                 
                End If


                If ViewState("DepartmentsState") = "New" Then
                    SetFocus(txtCode)
                    lblHeading.Text = "Add New Department Master"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("DepartmentsState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Department Master"
                    btnSave.Text = "Update"
                    txtDeptcode.Enabled = False
                    DisableControl()
                    ShowRecord(CType(ViewState("DepartmentsRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("DepartmentsState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Department Master"
                    btnSave.Visible = False
                    txtDeptcode.Attributes.Add("readonly", "readonly")
                    txtDeptname.Attributes.Add("readonly", "readonly")
                    
                 
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("DepartmentsRefCode"), String))
                ElseIf ViewState("DepartmentsState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Department Master"
                    btnSave.Text = "Delete"
                    txtDeptcode.Attributes.Add("readonly", "readonly")
                    txtDeptname.Attributes.Add("readonly", "readonly")

                    DisableControl()
                    ShowRecord(CType(ViewState("DepartmentsRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("DepartmentMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("DepartmentsState") = "View" Or ViewState("DepartmentsState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            chkActive.Disabled = True
            txtemail.Disabled = True
            txtfax.Disabled = True
            txtphone.Disabled = True
            txturl.Disabled = True
            txtDeptcode.Enabled = True
            txtDeptname.Enabled = True
            ddlShowinweb.Enabled = False
        ElseIf ViewState("DepartmentsState") = "Edit" Then
            txtCode.Disabled = True
        End If

    End Sub

#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Page.IsValid = True Then
                If ViewState("DepartmentsState") = "New" Or ViewState("DepartmentsState") = "Edit" Then
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("DepartmentsState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_deptmaster", mySqlConn, sqlTrans)
                    ElseIf ViewState("DepartmentsState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_deptmaster", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@deptcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@deptname", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 25)).Value = CType(txtemail.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@DeptHead", SqlDbType.VarChar, 10)).Value = txtDeptcode.Text
                    mySqlCmd.Parameters.Add(New SqlParameter("@Phone", SqlDbType.VarChar, 20)).Value = CType(txtphone.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@Fax", SqlDbType.VarChar, 20)).Value = CType(txtfax.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@URL", SqlDbType.VarChar, 100)).Value = CType(txturl.Value.Trim, String)
                    If ViewState("DepartmentsState") = "New" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
                    ElseIf ViewState("DepartmentsState") = "Edit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@moddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    If ddlShowinweb.SelectedValue = "Yes" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@showinresn", SqlDbType.Int)).Value = 1
                    ElseIf ddlShowinweb.SelectedValue = "No" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@showinresn", SqlDbType.Int)).Value = 0
                    End If

                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("DepartmentsState") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("sp_del_deptmaster", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@deptcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("DepartmentMasterSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('DeptWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DepartmentMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from DeptMaster Where deptcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("deptcode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("deptcode"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("deptname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("deptname"), String)
                    Else
                        Me.txtName.Value = ""
                    End If
                    If IsDBNull(mySqlReader("email")) = False Then
                        Me.txtemail.Value = CType(mySqlReader("email"), String)
                    Else
                        Me.txtemail.Value = ""
                    End If
                    If IsDBNull(mySqlReader("DeptHead")) = False Then
                        txtDeptname.Text = mySqlReader("DeptHead")
                        txtDeptcode.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "usermaster", "username", "usercode", mySqlReader("DeptHead"))
                    End If
                    If IsDBNull(mySqlReader("Phone")) = False Then
                        Me.txtphone.Value = CType(mySqlReader("Phone"), String)
                    Else
                        Me.txtphone.Value = ""
                    End If
                    If IsDBNull(mySqlReader("Fax")) = False Then
                        Me.txtfax.Value = CType(mySqlReader("Fax"), String)
                    Else
                        Me.txtfax.Value = ""
                    End If
                    If IsDBNull(mySqlReader("URL")) = False Then
                        Me.txturl.Value = CType(mySqlReader("URL"), String)
                    Else
                        Me.txturl.Value = ""
                    End If
                    If IsDBNull(mySqlReader("showinresn")) = False Then
                        If CType(mySqlReader("showinresn"), String) = "1" Then
                            ddlShowinweb.SelectedValue = "Yes"
                        ElseIf CType(mySqlReader("showinresn"), String) = "0" Then
                            ddlShowinweb.SelectedValue = "No"
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
            objUtils.WritErrorLog("DepartmentMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("DepartmentMasterSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        Try
            If ViewState("DepartmentsState") = "New" Then
                If objUtils.isDuplicatenew(Session("dbconnectionName"), "DeptMaster", "Deptcode", txtCode.Value.Trim) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Department code is already present.');", True)
                    SetFocus(txtCode)
                    checkForDuplicate = False
                    Exit Function
                End If
                If objUtils.isDuplicatenew(Session("dbconnectionName"), "DeptMaster", "DeptName", txtName.Value.Trim) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Department name is already present.');", True)
                    SetFocus(txtName)
                    checkForDuplicate = False
                    Exit Function
                End If
            ElseIf ViewState("DepartmentsState") = "Edit" Then
                If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "DeptMaster", "Deptcode", "DeptName", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Department name is already present.');", True)
                    SetFocus(txtName)
                    checkForDuplicate = False
                    Exit Function
                End If
            End If
            checkForDuplicate = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("DepartmentMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=DepartmentMaster','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
<System.Web.Services.WebMethod()> _
    Public Shared Function GetDeptHead(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim deptheadname As New List(Of String)
        Try

            strSqlQry = "select username,usercode from usermaster where active=1 and  username like '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    deptheadname.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("username").ToString(), myDS.Tables(0).Rows(i)("usercode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return deptheadname
        Catch ex As Exception
            Return deptheadname
        End Try

    End Function


End Class



'objUtils.MessageBox("This currency code is already present.", Me.Page)