'------------================--------------=======================------------------================
'   Page Name       :   MealPlans.aspx
'   Developer Name  :   Sandeep Indulkar
'   Date            :   21June 2008
'   
'
'------------================--------------=======================------------------================
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.ArrayList
Imports System.Collections.Generic

Partial Class MealPlans
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
                Dim mealcode As String = Request.QueryString("RefCode")
                If mealcode Is Nothing Then 'changed by mohamed on 17/04/2018
                    mealcode = ""
                End If
                ViewState("MealplansRefCode") = mealcode.ToString.Replace("%2b", "+")

                '  ViewState("MealplansRefCode") = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), "select dbo.pwddecript('" & Request.QueryString("RefCode") & "')")


                ViewState.Add("MealplansState", Request.QueryString("State"))
                'ViewState.Add("MealplansRefCode", Request.QueryString("RefCode"))
                FillDropDownListHTMLNEW()
                If ViewState("MealplansState") = "New" Then

                    charcters(txtCode)
                    SetFocus(txtCode)
                    charcters(txtCode)
                    lblHeading.Text = "Add New Meal"
                    Page.Title = Page.Title + " " + "New Meal Master"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")


                ElseIf ViewState("MealplansState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Meal"
                    Page.Title = Page.Title + " " + "Edit Meal Master"
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("MealplansRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")


                ElseIf ViewState("MealplansState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Meal"
                    Page.Title = Page.Title + " " + "View Meal Master"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()

                    ShowRecord(CType(ViewState("MealplansRefCode"), String))

                ElseIf ViewState("MealplansState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Meal"
                    Page.Title = Page.Title + " " + "Delete Meal Master"
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("MealplansRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")

                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
                Numbers(txtOrder)
                '                btnSave.Attributes.Add("onclick", "return FormValidation()")

                '   ValidateOnlyNumber()


                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlSPTypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlSPTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("MealPlans.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        Page.Title = "MealPlan Entry"
    End Sub
#End Region
#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region
#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtOrder.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region


#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        If ViewState("MealplansState") = "View" Or ViewState("MealplansState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            txtOrder.Disabled = True
            chkActive.Disabled = True
            ddlSPTypeCode.Disabled = True
            ddlSPTypeName.Disabled = True
        ElseIf ViewState("MealplansState") = "Edit" Then
            txtCode.Disabled = True
        End If

    End Sub

#End Region
    Private Function ValidateSave() As Boolean
        Dim strscript As String = ""
        If txtCode.Value = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter Code.');", True)
            ValidateSave = False
            Exit Function
        End If

        If txtName.Value = "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please enter Name.');", True)
            ValidateSave = False
            Exit Function
        End If

      
        ValidateSave = True
    End Function
#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sptype As String
        Try

            If Page.IsValid = True Then
                If ViewState("MealplansState") = "New" Or ViewState("MealplansState") = "Edit" Then
                    If ValidateSave() = False Then
                        Exit Sub
                    End If
                    If txtmainmeal.Text = "" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Main Meal Plan .');", True)
                        Exit Sub
                    End If

                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If txtOrder.Value = "" Then
                        txtOrder.Value = 0
                    End If

                    If ViewState("MealplansState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_meal", mySqlConn, sqlTrans)
                    ElseIf ViewState("MealplansState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_meal", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@mealname", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
                    sptype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "458")
                    mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = sptype
                    'mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = CType(ddlSPTypeCode.Items(ddlSPTypeCode.SelectedIndex).Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@rankorder", SqlDbType.Int)).Value = CType(txtOrder.Value, Integer)

                    If txtmainmeal.Text <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@mainmealcode", SqlDbType.VarChar, 20)).Value = txtmainmeal.Text
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@mainmealcode", SqlDbType.VarChar, 20)).Value = System.DBNull.Value
                    End If
                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("MealplansState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_meal", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("MealPlansSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('MealplanWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("MealPlans.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

    '#Region " Validate Page"

    '    'Public Function ValidatePage() As Boolean
    '    '    Try
    '    '        If txtCode.Value = "" Then
    '    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Code field can not be blank.');", True)
    '    '            SetFocus(txtCode)
    '    '            ValidatePage = False
    '    '            Exit Function
    '    '        End If
    '    '        If txtName.Value.Trim = "" Then
    '    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name field can not be blank.');", True)
    '    '            SetFocus(txtName)
    '    '            ValidatePage = False
    '    '            Exit Function
    '    '        End If

    '    '        ValidatePage = True
    '    '    Catch ex As Exception

    '    '    End Try
    '    'End Function
    '#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from mealmast Where mealcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("mealcode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("mealcode"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("mealname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("mealname"), String)
                    Else
                        Me.txtName.Value = ""
                    End If
                    If IsDBNull(mySqlReader("rankorder")) = False Then
                        Me.txtOrder.Value = CType(mySqlReader("rankorder"), String)
                    Else
                        Me.txtOrder.Value = ""
                    End If
                    If IsDBNull(mySqlReader("mainmealcode")) = False Then
                        Me.txtmainmeal.Text = CType(mySqlReader("mainmealcode"), String)
                    Else
                        Me.txtmainmeal.Text = ""
                    End If

                    If IsDBNull(mySqlReader("sptypecode")) = False Then
                        'If objUtils.DDLFieldAvliable(ddlSPTypeCode, CType(mySqlReader("sptypecode"), String)) = True Then
                        ddlSPTypeCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", CType(mySqlReader("sptypecode"), String))
                        ddlSPTypeName.Value = CType(mySqlReader("sptypecode"), String)
                    Else
                        ddlSPTypeCode.Value = ""
                        ddlSPTypeName.Value = ""
                        'End If
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
            objUtils.WritErrorLog("MealPlans.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("MealPlansSearch.aspx", False)
        '   ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('MealplanWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
    End Sub
#End Region

    '#Region "Validation for enter only number"
    '    Private Sub ValidateOnlyNumber()
    '        txtConversion.Attributes.Add("onkeypress", "return checkNumber(event)")
    '    End Sub
    '#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean

        If ViewState("MealplansState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "mealmast", "mealcode", txtCode.Value.Trim) Then
                'objUtils.MessageBox("This currency code is already present.", Me.Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This meal code is already present.');", True)
                SetFocus(txtCode)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "mealmast", "mealname", txtName.Value.Trim) Then
                'objUtils.MessageBox("This currency name is already present.", Me.Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This meal name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("MealplansState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "mealmast", "mealcode", "mealname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                'objUtils.MessageBox("This currency name is already present.", Me.Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This meal name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region
#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "cancel_detail", "mealcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Meal is already used for a cancellationPolicies, cannot delete this Meal');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "compare_ratesd", "mealcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Meal is already used for a DetailsOfCompetitorsRates, cannot delete this Meal');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymeal", "mealcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Meal is already used for a MealofSuppliers, cannot delete this Meal');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "promotion_detail", "mealcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Meal is already used for a Promotions, cannot delete this Meal');", True)
            checkForDeletion = False
            Exit Function

        End If

        checkForDeletion = True
    End Function
#End Region


    Private Sub FillDropDownListHTMLNEW()
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeCode, "sptypecode", "sptypename", "select sptypecode,sptypename from sptypemast  where active =1 order by sptypecode", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "sptypename", "sptypecode", "select sptypename,sptypecode from sptypemast  where active =1 order by sptypecode", True)
        Dim sptype As String = ""
        sptype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "458")
        If sptype <> "" Then
            ddlSPTypeName.Value = sptype
            ddlSPTypeCode.Value = ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text
        End If
    End Sub


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=MealPlans','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    Protected Sub txtmainmeal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtmainmeal.TextChanged
        Session("mealplans_mainmealcode_for_filter") = tCode.Text
    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function GetMainMeals(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim MealNames As New List(Of String)
        Try

            strSqlQry = "select distinct(mainmealcode) mainmealcode from mainmealplans where mainmealcode like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    MealNames.Add(myDS.Tables(0).Rows(i)("mainmealcode").ToString())
                    'MealNames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("mealname").ToString(), myDS.Tables(0).Rows(i)("mealcode").ToString()))
                Next

            End If

            Return MealNames
        Catch ex As Exception
            Return MealNames
        End Try
    End Function

End Class
