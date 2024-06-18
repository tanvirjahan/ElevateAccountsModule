'------------================--------------=======================------------------================
'   Page Name       :   CostCenterGroups.aspx
'   Developer Name  :   Sandeep Indulkar
'   Date            :   
'   
'------------================--------------=======================------------------================
#Region "Namespace "
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class CostCenterGroups
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
                ViewState.Add("CostGrpState", Request.QueryString("State"))
                ViewState.Add("CostGrpRefCode", Request.QueryString("RefCode"))
                If ViewState("CostGrpState") = "New" Then
                    SetFocus(txtCode)
                    lblHeading.Text = "Add New Cost Center Groups"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("CostGrpState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Cost Center Groups"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("CostGrpRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("CostGrpState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Cost Center Groups"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("CostGrpRefCode"), String))
                ElseIf ViewState("CostGrpState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Cost Center Groups"
                    btnSave.Text = "Delete"
                    DisableControl()
                    ShowRecord(CType(ViewState("CostGrpRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If
                charcters(txtCode)
                charcters(txtName)
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CostCenterGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        If ViewState("CostGrpState") = "View" Or ViewState("CostGrpState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            chkActive.Disabled = True
        ElseIf ViewState("CostGrpState") = "Edit" Then
            txtCode.Disabled = True
        End If
    End Sub
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                If ViewState("CostGrpState") = "New" Or ViewState("CostGrpState") = "Edit" Then
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("CostGrpState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_costcentergrp", mySqlConn, sqlTrans)
                    ElseIf ViewState("CostGrpState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_costcentergrp", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@code", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)

                    If ViewState("CostGrpState") = "New" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
                    ElseIf ViewState("CostGrpState") = "Edit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@moddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If

                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("CostGrpState") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_costcentergrp", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@code", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("CostCenterGroupsSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('CostCenterGroupWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            objUtils.WritErrorLog("CostCenterGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from costcentergroup_master Where costcentergrp_code='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("costcentergrp_code")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("costcentergrp_code"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("costcentergrp_name")) = False Then
                        Me.txtName.Value = CType(mySqlReader("costcentergrp_name"), String)
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
            objUtils.WritErrorLog("CostCenterGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("CostCenterGroupsSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        Try
            If ViewState("CostGrpState") = "New" Then
                If objUtils.isDuplicatenew(Session("dbconnectionName"), "costcentergroup_master", "costcentergrp_code", txtCode.Value.Trim) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Group Code is already present.');", True)
                    SetFocus(txtCode)
                    checkForDuplicate = False
                    Exit Function
                End If
                If objUtils.isDuplicatenew(Session("dbconnectionName"), "costcentergroup_master", "costcentergrp_name", txtName.Value.Trim) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Group name is already present.');", True)
                    SetFocus(txtName)
                    checkForDuplicate = False
                    Exit Function
                End If
            ElseIf ViewState("CostGrpState") = "Edit" Then
                If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "costcentergroup_master", "costcentergrp_code", "costcentergrp_name", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Group name is already present.');", True)
                    SetFocus(txtName)
                    checkForDuplicate = False
                    Exit Function
                End If
            End If
            checkForDuplicate = True
        Catch ex As Exception
            objUtils.WritErrorLog("CostCenterGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CostCenterGroups','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
