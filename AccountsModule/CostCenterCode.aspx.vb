'------------================--------------=======================------------------================
'   Page Name       :   CostCenterCode.aspx
'   Developer Name  :   Sandeep Indulkar
'   Date            :   
'   
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
#End Region

Partial Class CostCenterCode
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
    <System.Web.Script.Services.ScriptMethod()> _
        <System.Web.Services.WebMethod()> _
    Public Shared Function Getctrylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim costcentergrpnames As New List(Of String)
        Try
            strSqlQry = "select costcentergrp_code,costcentergrp_name from costcentergroup_master where active=1 and  costcentergrp_name like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    costcentergrpnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("costcentergrp_name").ToString(), myDS.Tables(0).Rows(i)("costcentergrp_code").ToString()))

                Next
            End If
            Return costcentergrpnames
        Catch ex As Exception
            Return costcentergrpnames
        End Try

    End Function
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
                ViewState.Add("CostCodeState", Request.QueryString("State"))
                ViewState.Add("CostCodeRefCode", Request.QueryString("RefCode"))
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupcode, "costcentergrp_code", "costcentergrp_name", "select costcentergrp_code,costcentergrp_name from costcentergroup_master  where active=1 order by costcentergrp_code", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlGroupname, "costcentergrp_name", "costcentergrp_code", "select costcentergrp_name,costcentergrp_code from costcentergroup_master  where active=1 order by costcentergrp_name", True)



                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    'ddlGroupcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlGroupname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                If ViewState("CostCodeState") = "New" Then
                    SetFocus(txtCode)
                    lblHeading.Text = "Add New Cost Center Code"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("CostCodeState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Cost Center Code"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("CostCodeRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("CostCodeState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Cost Center Code"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("CostCodeRefCode"), String))
                ElseIf ViewState("CostCodeState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Cost Center Code"
                    btnSave.Text = "Delete"
                    DisableControl()
                    ShowRecord(CType(ViewState("CostCodeRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
                charcters(txtCode)
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("CostCenterCode.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        If ViewState("CostCodeState") = "View" Or ViewState("CostCodeState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            chkActive.Disabled = True
            TxtCostGrpName.Enabled = False
        ElseIf ViewState("CostCodeState") = "Edit" Then
            txtCode.Disabled = True
        End If

    End Sub

#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Page.IsValid = True Then
                If ViewState("CostCodeState") = "New" Or ViewState("CostCodeState") = "Edit" Then
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("CostCodeState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_costcentercode", mySqlConn, sqlTrans)
                    ElseIf ViewState("CostCodeState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_costcentercode", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@code", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@grpcode", SqlDbType.VarChar, 20)).Value = CType(TxtCostgrpCode.Text.Trim, String)
                    If ViewState("CostCodeState") = "New" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@adddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
                    ElseIf ViewState("CostCodeState") = "Edit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@moddate", SqlDbType.DateTime)).Value = objdate.GetSystemDateTime(Session("dbconnectionName"))
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.ExecuteNonQuery()
                ElseIf ViewState("CostCodeState") = "Delete" Then
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_costcentercode", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@code", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("CostCenterCodeSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('CostCenterCodeWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            objUtils.WritErrorLog("CostCenterCode.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from costcenter_master Where costcenter_code='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("costcenter_code")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("costcenter_code"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("costcenter_name")) = False Then
                        Me.txtName.Value = CType(mySqlReader("costcenter_name"), String)
                    Else
                        Me.txtName.Value = ""
                    End If
                    If IsDBNull(mySqlReader("costcentergrp_code")) = False Then
                        TxtCostgrpCode.Text = mySqlReader("costcentergrp_code")
                        TxtCostGrpName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "costcentergroup_master", "costcentergrp_name", "costcentergrp_code", mySqlReader("costcentergrp_code"))
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
            objUtils.WritErrorLog("CostCenterCode.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("CostCenterCodeSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        Try
            If ViewState("CostCodeState") = "New" Then
                If objUtils.isDuplicatenew(Session("dbconnectionName"), "costcenter_master", "costcenter_code", txtCode.Value.Trim) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Code is already present.');", True)
                    SetFocus(txtCode)
                    checkForDuplicate = False
                    Exit Function
                End If
                If objUtils.isDuplicatenew(Session("dbconnectionName"), "costcenter_master", "costcenter_name", txtName.Value.Trim) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Name is already present.');", True)
                    SetFocus(txtName)
                    checkForDuplicate = False
                    Exit Function
                End If

                If TxtCostgrpCode.Text.Trim = "" Or TxtCostGrpName.Text.Trim = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Group.');", True)
                    SetFocus(TxtCostGrpName)
                    checkForDuplicate = False
                    Exit Function
                End If
            ElseIf ViewState("CostCodeState") = "Edit" Then
                If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "costcenter_master", "costcenter_code", "costcenter_name", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Name is already present.');", True)
                    SetFocus(txtName)
                    checkForDuplicate = False
                    Exit Function
                End If
                If TxtCostgrpCode.Text.Trim = "" Or TxtCostGrpName.Text.Trim = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Group.');", True)
                    SetFocus(TxtCostGrpName)
                    checkForDuplicate = False
                    Exit Function
                End If
            End If
            checkForDuplicate = True
        Catch ex As Exception
            objUtils.WritErrorLog("CostCenterCode.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=CostCenterCode','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
