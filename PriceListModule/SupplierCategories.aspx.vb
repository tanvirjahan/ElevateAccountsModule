'------------================--------------=======================------------------================
'   Page Name       :   SupplierType.aspx
'   Developer Name  :    Pramod Desai
'   Date            :    14 June 2008
'------------================--------------=======================------------------================
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

Partial Class SupplierCategories
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
                ViewState.Add("SupcatsState", Request.QueryString("State"))
                ViewState.Add("SupcatsRefCode", Request.QueryString("RefCode"))
                ViewState.Add("SupValue", Request.QueryString("Value"))
                ' FillDDL()
                If ViewState("SupcatsState") = "New" Then
                    SetFocus(txtCode)
                    lblHeading.Text = "Add New Supplier Category"
                    Page.Title = Page.Title + " " + "New Supplier Category Master"
                    btnSave.Text = "Save"
                    txtOrder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(rankorder),0) from catmast") + 1
                    'If ViewState("SupcatsState") = "New" Then
                    'btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    'ElseIf ViewState("SupcatsState") = "Addfrom" Then
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    'End If
                ElseIf ViewState("SupcatsState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Supplier Category"
                    Page.Title = Page.Title + " " + "Edit Supplier Category Master"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("SupcatsRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("SupcatsState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Supplier Category"
                    Page.Title = Page.Title + " " + "View Supplier Category Master"
                    btnSave.Visible = False
                    'btnCancel.Text = "Return to Supplier Category"
                    DisableControl()
                    ShowRecord(CType(ViewState("SupcatsRefCode"), String))
                ElseIf ViewState("SupcatsState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Supplier Category"
                    Page.Title = Page.Title + " " + "Delete Supplier Category Master"
                    btnSave.Text = "Delete"
                    DisableControl()
                    ShowRecord(CType(ViewState("SupcatsRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If



                    Dim typ As Type
                    typ = GetType(DropDownList)

                    If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                        Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                        ddlSupplierType.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                        ddlSupplierTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    End If

                    btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SupplierCategories.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        Page.Title = "SupplierCategories Entry"
    End Sub
    Private Sub FillDDL()
        strSqlQry = ""
        strSqlQry = "SELECT sptypecode,sptypename FROM sptypemast WHERE active=1 ORDER BY sptypecode"
        'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlSupplierType, "sptypecode", strSqlQry, True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierType, "sptypecode", "sptypename", strSqlQry, True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSupplierTypeName, "sptypename", "sptypecode", strSqlQry, True)
    End Sub
    Private Sub DisableControl()
        If ViewState("SupcatsState") = "View" Or ViewState("SupcatsState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            'ddlSupplierType.Enabled = False
            'ddlSupplierType.Disabled = True
            'ddlSupplierTypeName.Disabled = True
            txthotelname.Enabled = False
            txtOrder.Disabled = True
            chkActive.Disabled = True
        ElseIf ViewState("SupcatsState") = "Edit" Then
            txtCode.Disabled = True
        End If
    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
  <System.Web.Services.WebMethod()> _
    Public Shared Function Getpropertytypelist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim propertytype As New List(Of String)
        Try

            strSqlQry = "select propertytypename,propertytypecode from hotel_propertytype where  propertytypename like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    propertytype.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("propertytypename").ToString(), myDS.Tables(0).Rows(i)("propertytypecode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return propertytype
        Catch ex As Exception
            Return propertytype
        End Try

    End Function
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from catmast Where catcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("catcode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("catcode"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("catname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("catname"), String)
                    Else
                        Me.txtName.Value = ""
                    End If
                    If IsDBNull(mySqlReader("sptypecode")) = False Then
                        'If objUtils.DDLFieldAvliable(ddlSupplierType, CType(mySqlReader("sptypecode"), String)) = True Then
                        '    ddlSupplierType.SelectedValue = CType(mySqlReader("sptypecode"), String)
                        '    txtSupplierType.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sptypemast", "sptypename", "sptypecode", CType(mySqlReader("sptypecode"), String))
                        'Else
                        '    ddlSupplierType.SelectedValue = "[Select]"
                        '    txtSupplierType.Value = ""
                        'End If
                        'ddlSupplierTypeName.Value = CType(mySqlReader("sptypecode"), String)
                        ' ddlSupplierType.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", CType(mySqlReader("sptypecode"), String))
                        txthotelcode.Text = CType(mySqlReader("sptypecode"), String)
                        txthotelname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", mySqlReader("sptypecode"))

                    Else
                        'ddlSupplierType.Value = "[Select]"
                        'ddlSupplierTypeName.Value = "[Select]"
                        txthotelname.Text = ""
                        txthotelcode.Text = ""

                    End If
                    If IsDBNull(mySqlReader("rankorder")) = False Then
                        Me.txtOrder.Value = CType(mySqlReader("rankorder"), String)
                    Else
                        Me.txtOrder.Value = ""
                    End If

                    If IsDBNull(mySqlReader("propertytype")) = False Then
                        txtpropertytypecode.Text = mySqlReader("propertytype")
                        txtpropertytypename.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "hotel_propertytype", "propertytypename", "propertytypecode", mySqlReader("propertytype"))
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
            objUtils.WritErrorLog("SupplierCategories.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Page.IsValid = True Then
                If ViewState("SupcatsState") = "New" Or ViewState("SupcatsState") = "Edit" Then
                    'If ValidatePage() = False Then
                    '    Exit Sub
                    'End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    If ViewState("SupcatsState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_cat", mySqlConn, sqlTrans)
                    ElseIf ViewState("SupcatsState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_cat", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@catcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@catname", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlSupplierType.SelectedValue.Trim, String)
                    ' mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlSupplierType.Items(ddlSupplierType.SelectedIndex).Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(txthotelcode.Text, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlSupplierType.Items(ddlSupplierType.SelectedIndex).Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@rnkord", SqlDbType.Int, 9)).Value = CType(txtOrder.Value.Trim, Integer)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If txtpropertytypename.Text <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@propertytype", SqlDbType.VarChar, 20)).Value = txtpropertytypecode.Text
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@propertytype", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If
                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("SupcatsState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    mySqlCmd = New SqlCommand("sp_del_cat", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@catcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If
                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("SupplierCategoriesSearch.aspx", False)
                If ViewState("SupValue") = "Addfrom" Then
                    Session.Add("SupCategoryCode", txtCode.Value)
                    Session.Add("SupCategoryName", txtName.Value)
                    Dim strscript1 As String = ""
                    strscript1 = "window.opener.__doPostBack('SupcatsfromWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript1, True)
                End If
                If ViewState("SupcatsState") = "New" Or ViewState("SupcatsState") = "View" Or ViewState("SupcatsState") = "Edit" Or ViewState("SupcatsState") = "Delete" Then
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('SupcatsWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierCategories.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("SupplierCategoriesSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
    Public Function checkForDuplicate() As Boolean
        If ViewState("SupcatsState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "catmast", "catcode", txtCode.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This supplier category code is already present.');", True)
                SetFocus(txtCode)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "catmast", "catname", txtName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This  supplier category name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("SupcatsState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "catmast", "catcode", "catname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This  supplier category name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function

    'Protected Sub ddlSupplierType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    txtSupplierType.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sptypemast", "sptypename", "sptypecode", ddlSupplierType.SelectedValue.Trim.ToString)
    'End Sub
#Region "Public Function ValidatePage() As Boolean"
    Public Function ValidatePage() As Boolean
        Try
            If txtOrder.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Order field can not be blank.');", True)
                SetFocus(txtOrder)
                ValidatePage = False
                Exit Function
            End If
            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SupplierCategories.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#End Region

#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyinfo", "catcode", CType(txtCode.Value.Trim, String)) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierCategory is already used for a SuppliersWebInformation, cannot delete this SupplierCategory');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymast", "catcode", CType(txtCode.Value.Trim, String)) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierCategory is already used for a Suppliers, cannot delete this SupplierCategory');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "supplier_agents", "catcode", CType(txtCode.Value.Trim, String)) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierCategory is already used for a SupplierAgents, cannot delete this SupplierCategory');", True)
            checkForDeletion = False
            Exit Function


        End If
        checkForDeletion = True
    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupplierCategories','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function Gethoteltypelist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Hotelnames As New List(Of String)
        Try

            strSqlQry = "select sptypename,sptypecode from sptypemast where  sptypename like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    Hotelnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("sptypename").ToString(), myDS.Tables(0).Rows(i)("sptypecode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Hotelnames
        Catch ex As Exception
            Return Hotelnames
        End Try

    End Function




End Class
