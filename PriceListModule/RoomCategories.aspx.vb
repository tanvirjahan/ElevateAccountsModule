'------------================--------------=======================------------------================
'   Page Name       :   RoomCategories.aspx
'   Developer Name  :    Pramod Desai
'   Date            :    16 June 2008
'------------================--------------=======================------------------================
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.ArrayList
Imports System.Collections.Generic

Partial Class RoomCategories
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim sptype As String = ""
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                'If Not Session("CompanyName") Is Nothing Then
                '    Me.Page.Title = CType(Session("CompanyName"), String)
                'End If
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ViewState.Add("RmcatsState", Request.QueryString("State"))
                ViewState.Add("RmcatsRefCode", Request.QueryString("RefCode"))
                ViewState.Add("Type", Request.QueryString("Type"))
                Dim heading As String


                If ViewState("Type") = "Acc" Then
                    heading = "Accommodation Category"
                    ddlAllotmentRequired.Enabled = False
                    lblmealplan.Style("display") = "none"
                    ddlMealPlan.Style("display") = "none"
                    ddlcattype.Items.Add(New ListItem("[Select]", "0"))
                    ddlcattype.Items.Add(New ListItem("Adult Accommodation", "1"))
                    ddlcattype.Items.Add(New ListItem("Child Accommodation", "2"))


                ElseIf ViewState("Type") = "Supp" Then
                    lblallotment.Style("display") = "none"
                    ddlAllotmentRequired.Style("display") = "none"
                    heading = "Supplement Category"
                    ddlcattype.Items.Add(New ListItem("[Select]", "0"))
                    ddlcattype.Items.Add(New ListItem("Adult Meal Supplements", "1"))
                    ddlcattype.Items.Add(New ListItem("Child Meal Supplements", "2"))
                    ddlcattype.Items.Add(New ListItem("Extra", "3"))
                    ddlMealPlan.Enabled = False

                End If
                ' FillDropDownListHTMLNEW()
                If ViewState("RmcatsState") = "New" Then
                    SetFocus(txtCode)
                    lblHeading.Text = "Add New " + heading
                    Page.Title = Page.Title + " " + "New " + heading + "Master"
                    btnSave.Text = "Save"
                    txtOrder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(rankorder),0) from rmcatmast") + 1

                    'btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("RmcatsState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit " + heading
                    Page.Title = Page.Title + " " + "Edit" + heading + "Master"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("RmcatsRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("RmcatsState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View " + heading
                    Page.Title = Page.Title + " " + "View" + heading + "Master"
                    btnSave.Visible = False
                    'btnCancel.Text = "Return to Room Category"
                    DisableControl()
                    ShowRecord(CType(ViewState("RmcatsRefCode"), String))

                ElseIf ViewState("RmcatsState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete " + heading
                    Page.Title = Page.Title + " " + "View" + heading + "Master"
                    btnSave.Text = "Delete"
                    DisableControl()
                    ShowRecord(CType(ViewState("RmcatsRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                Dim extrapax As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=566") 'objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select  stuff((select distinct ',' + option_selected from reservation_parameters  where param_id in (566,1139)  for xml path('')),1,1,'' ) ")
                Dim adulteb As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1139")
                'objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id in (566,1139)")
                If (txtCode.Value = extrapax Or txtCode.Value = adulteb) And ViewState("Type") = "Acc" Then
                    ddlAllotmentRequired.SelectedValue = "No"
                ElseIf (txtCode.Value <> extrapax Or txtCode.Value <> adulteb) And ViewState("Type") = "Acc" Then
                    ddlAllotmentRequired.SelectedValue = "Yes"

                End If

                Dim typ As Type
                typ = GetType(DropDownList)
                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    'ddlSPTypeCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlSPTypeName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("RoomCategories.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        If ViewState("Type") = "Acc" Then
            Page.Title = "AccommodationCategory Entry"
        End If
        If ViewState("Type") = "Supp" Then
            Page.Title = "SupplementCategory Entry"
        End If
    End Sub

    Private Sub DisableControl()
        If ViewState("RmcatsState") = "View" Or ViewState("RmcatsState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            txtPrintName.Disabled = True
            ddlMealPlan.Enabled = False
            txtNoPerson.Disabled = True
            ddlAllotmentRequired.Enabled = False
            txtOrder.Disabled = True
            ddlCalculatebyPax.Enabled = False
            txtDisplayinWeb.Disabled = True
            txtUnitName.Disabled = True
            chkAutoConfirm.Disabled = True
            chkActive.Disabled = True
            'ddlSPTypeCode.Disabled = True
            'ddlSPTypeName.Disabled = True
            ddlcattype.Enabled = False
            txtDefaultAdultNo.Disabled = True
        ElseIf ViewState("RmcatsState") = "Edit" Then
            txtCode.Disabled = True
        End If
    End Sub
    Private Sub FillDropDownListHTMLNEW()
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeCode, "sptypecode", "sptypename", "select sptypecode,sptypename from sptypemast  where active =1 order by sptypecode", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSPTypeName, "sptypename", "sptypecode", "select sptypename,sptypecode from sptypemast  where active =1 order by sptypecode", True)

        sptype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "458")
        If sptype <> "" Then
            ddlSPTypeName.Value = sptype
            ddlSPTypeCode.Value = ddlSPTypeName.Items(ddlSPTypeName.SelectedIndex).Text
        End If
    End Sub
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from rmcatmast Where rmcatcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("rmcatcode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("rmcatcode"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("rmcatname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("rmcatname"), String)
                    Else
                        Me.txtName.Value = ""
                    End If
                    Select Case mySqlReader("accom_extra")
                        Case "A"
                            ddlcattype.SelectedValue = 1
                            lbllinkedmeal.Visible = False
                            txtmealname.Visible = False
                            txtmealcode.Visible = False
                        Case "C"
                            ddlcattype.SelectedValue = 2
                            lbllinkedmeal.Visible = False
                            txtmealname.Visible = False
                            txtmealcode.Visible = False

                        Case "M" ' Adult Meal Supplements
                            ddlcattype.SelectedValue = 1
                            lbllinkedmeal.Visible = True
                            txtmealname.Visible = True
                            txtmealcode.Visible = True
                        Case "L" 'Child Meal Supplements
                            ddlcattype.SelectedValue = 2
                            lbllinkedmeal.Visible = True
                            txtmealname.Visible = True
                            txtmealcode.Visible = True
                        Case "E" ' Extra
                            ddlcattype.SelectedValue = 3
                            lbllinkedmeal.Visible = False
                            txtmealname.Visible = False
                            txtmealcode.Visible = False
                        Case Else
                            ddlcattype.SelectedValue = 0
                            lbllinkedmeal.Visible = False
                            txtmealname.Visible = False
                            txtmealcode.Visible = False
                    End Select
                    'If IsDBNull(mySqlReader("sptypecode")) = False Then
                    '    'If objUtils.DDLFieldAvliable(ddlSPTypeCode, CType(mySqlReader("sptypecode"), String)) = True Then
                    '    ddlSPTypeCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "sptypemast", "sptypename", "sptypecode", CType(mySqlReader("sptypecode"), String))
                    '    ddlSPTypeName.Value = CType(mySqlReader("sptypecode"), String)
                    'Else
                    '    ddlSPTypeCode.Value = ""
                    '    ddlSPTypeName.Value = ""
                    '    'End If
                    'End If
                    If IsDBNull(mySqlReader("prnname")) = False Then
                        Me.txtPrintName.Value = CType(mySqlReader("prnname"), String)
                    Else
                        Me.txtPrintName.Value = ""
                    End If


                    If IsDBNull(mySqlReader("units")) = False Then
                        Me.txtNoPerson.Value = CType(mySqlReader("units"), String)
                    Else
                        Me.txtNoPerson.Value = ""
                    End If
                    If IsDBNull(mySqlReader("allotreqd")) = False Then
                        ddlAllotmentRequired.SelectedValue = CType(mySqlReader("allotreqd"), String)
                    End If
                    If ViewState("Type") = "Acc" Then
                        ddlAllotmentRequired.Enabled = False
                    End If
                    Select Case CType(mySqlReader("allotreqd"), String)
                        Case "Yes"
                            If IsDBNull(mySqlReader("calcyn")) = False Then
                                ddlCalculatebyPax.SelectedValue = CType(mySqlReader("calcyn"), String)
                            End If
                            If IsDBNull(mySqlReader("mealyn")) = False Then
                                ddlMealPlan.SelectedValue = CType(mySqlReader("mealyn"), String)
                            End If
                            ddlCalculatebyPax.Enabled = True
                            ddlMealPlan.Enabled = True
                            txtDefaultAdultNo.Style("visibility") = "visible"
                            lblDefaultNo.Style("visibility") = "visible"
                        Case "No"
                            ddlCalculatebyPax.SelectedValue = CType(mySqlReader("calcyn"), String)
                            ddlCalculatebyPax.Enabled = False
                            'ddlMealPlan.Enabled = False
                            ddlMealPlan.SelectedValue = CType(mySqlReader("mealyn"), String)
                            'txtDefaultAdultNo.Style("visibility") = "hidden"
                            'lblDefaultNo.Style("visibility") = "hidden"
                            txtDefaultAdultNo.Style("visibility") = "visible"
                            lblDefaultNo.Style("visibility") = "visible"
                    End Select


                    If IsDBNull(mySqlReader("rankorder")) = False Then
                        Me.txtOrder.Value = CType(mySqlReader("rankorder"), String)
                    Else
                        Me.txtOrder.Value = ""
                    End If


                    If IsDBNull(mySqlReader("defaultadults")) = False Then
                        Me.txtDefaultAdultNo.Value = CType(mySqlReader("defaultadults"), String)
                    Else
                        Me.txtDefaultAdultNo.Value = ""
                    End If

                    If IsDBNull(mySqlReader("mealcode")) = False Then
                        Me.txtmealcode.Text = CType(mySqlReader("mealcode"), String)
                        Me.txtmealname.Text = objUtils.GetString(Session("dbconnectionName"), "select mealname from mealmast where mealcode='" & txtmealcode.Text & "'")
                    Else
                        Me.txtmealcode.Text = ""
                        Me.txtmealname.Text = ""
                    End If


                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If
                    If IsDBNull(mySqlReader("webname")) = False Then
                        Me.txtDisplayinWeb.Value = CType(mySqlReader("webname"), String)
                    Else
                        Me.txtDisplayinWeb.Value = ""
                    End If
                    If IsDBNull(mySqlReader("unitname")) = False Then
                        Me.txtUnitName.Value = CType(mySqlReader("unitname"), String)
                    Else
                        Me.txtUnitName.Value = ""
                    End If
                    If IsDBNull(mySqlReader("autoconfirm")) = False Then
                        If CType(mySqlReader("autoconfirm"), String) = "1" Then
                            chkAutoConfirm.Checked = True
                        ElseIf CType(mySqlReader("autoconfirm"), String) = "0" Then
                            chkAutoConfirm.Checked = False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RoomTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
    Private Function ValidateSave() As Boolean

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

        If (txtmealname.Text = "" And txtmealcode.Text <> "" And ViewState("Type") = "Supp") Or (txtmealname.Text <> "" And txtmealcode.Text = "" And ViewState("Type") = "Supp") Or (txtmealname.Text = "" And txtmealcode.Text = "" And ViewState("Type") = "Supp") Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Linked Meal plan.');", True)
            ValidateSave = False
            Exit Function
        End If
        ValidateSave = True
    End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Page.IsValid = True Then
                If ViewState("RmcatsState") = "New" Or ViewState("RmcatsState") = "Edit" Then
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    If ValidateSave() = False Then
                        Exit Sub
                    End If
                   

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    If ViewState("RmcatsState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_rmcat", mySqlConn, sqlTrans)
                    ElseIf ViewState("RmcatsState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_rmcat", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@rmcatname", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
                    sptype = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "458")
                    mySqlCmd.Parameters.Add(New SqlParameter("@sptypecode", SqlDbType.VarChar, 20)).Value = sptype
                    mySqlCmd.Parameters.Add(New SqlParameter("@prnname", SqlDbType.VarChar, 100)).Value = CType(txtPrintName.Value.Trim, String)

                    'Select Case ddlcattype.SelectedValue
                    '    Case 0
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@accom_extra", SqlDbType.Char)).Value = "A"

                    '    Case 1
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@accom_extra", SqlDbType.Char)).Value = "C"

                    '    Case 2
                    '        mySqlCmd.Parameters.Add(New SqlParameter("@accom_extra", SqlDbType.Char)).Value = "E"
                    'End Select
                    If ddlcattype.SelectedItem.Text = "Adult Accommodation" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@accom_extra", SqlDbType.Char)).Value = "A"
                    ElseIf ddlcattype.SelectedItem.Text = "Child Accommodation" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@accom_extra", SqlDbType.Char)).Value = "C"

                    ElseIf ddlcattype.SelectedItem.Text = "Adult Meal Supplements" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@accom_extra", SqlDbType.Char)).Value = "M"
                    ElseIf ddlcattype.SelectedItem.Text = "Child Meal Supplements" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@accom_extra", SqlDbType.Char)).Value = "L"

                    ElseIf ddlcattype.SelectedItem.Text = "Extra" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@accom_extra", SqlDbType.Char)).Value = "E"
                    End If


                    If txtOrder.Value.Trim = "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@rankorder", SqlDbType.Int, 9)).Value = System.DBNull.Value
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@rankorder", SqlDbType.Int, 9)).Value = CType(txtOrder.Value.Trim, Integer)
                    End If
                    If ViewState("Type") = "Acc" Then
                        If ddlcattype.SelectedItem.Text = "Adult Accommodation" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@allotreqd", SqlDbType.VarChar, 10)).Value = "Yes"
                            mySqlCmd.Parameters.Add(New SqlParameter("@calcyn", SqlDbType.VarChar, 10)).Value = "No"
                        ElseIf ddlcattype.SelectedItem.Text = "Child Accommodation" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@allotreqd", SqlDbType.VarChar, 10)).Value = "No"
                            mySqlCmd.Parameters.Add(New SqlParameter("@calcyn", SqlDbType.VarChar, 10)).Value = "Yes"
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@mealyn", SqlDbType.VarChar, 10)).Value = "No"

                    ElseIf ViewState("Type") = "Supp" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@allotreqd", SqlDbType.VarChar, 10)).Value = "No"
                        mySqlCmd.Parameters.Add(New SqlParameter("@calcyn", SqlDbType.VarChar, 10)).Value = "Yes"
                        mySqlCmd.Parameters.Add(New SqlParameter("@mealyn", SqlDbType.VarChar, 10)).Value = CType(ddlMealPlan.SelectedValue.Trim, String)
                    End If
                    ' CType(ddlAllotmentRequired.SelectedValue.Trim, String)
                    'If txtNoPerson.Value.Trim = "" Then
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@units", SqlDbType.Int, 9)).Value = System.DBNull.Value
                    'Else
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@units", SqlDbType.Int, 9)).Value = CType(txtNoPerson.Value.Trim, Integer)
                    'End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@units", SqlDbType.Int, 9)).Value = 1
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@webName", SqlDbType.VarChar, 100)).Value = CType(txtDisplayinWeb.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@UnitName", SqlDbType.VarChar, 100)).Value = System.DBNull.Value 'CType(txtUnitName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@autoconfirm", SqlDbType.Int)).Value = 0
                    'If chkAutoConfirm.Checked = True Then
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@autoconfirm", SqlDbType.Int)).Value = 1
                    'ElseIf chkAutoConfirm.Checked = False Then
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@autoconfirm", SqlDbType.Int)).Value = 0
                    'End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    If txtDefaultAdultNo.Value <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@defaultadults", SqlDbType.Int)).Value = CType(txtDefaultAdultNo.Value, Integer)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@defaultadults", SqlDbType.Int)).Value = 0
                    End If
                    If ViewState("Type") = "Supp" Then
                        If txtmealcode.Text.Trim <> "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = txtmealcode.Text.Trim
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = System.DBNull.Value
                        End If
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@mealcode", SqlDbType.VarChar, 20)).Value = System.DBNull.Value
                    End If

                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("RmcatsState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_rmcat", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@rmcatcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("RoomCategoriesSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('RmcatsWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("RoomCategories.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("RoomCategoriesSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
    Public Function checkForDuplicate() As Boolean
        If ViewState("RmcatsState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "rmcatmast", "rmcatcode", txtCode.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This room category code is already present.');", True)
                SetFocus(txtCode)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "rmcatmast", "rmcatname", txtName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This room category name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("RmcatsState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "rmcatmast", "rmcatcode", "rmcatname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This room category name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "promotion_detail", "rmcatcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This RoomCategory is already used for a Promotions, cannot delete this RoomCategory');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "compare_ratesd", "rmcatcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This RoomCategory is already used for a DetailsOfCompetitorsRates, cannot delete this RoomCategory');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyrmcat", "rmcatcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This RoomCategory is already used for a RoomCategoriesOfSupplier, cannot delete this RoomCategory');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "earlypromrmcat_detail", "rmcatcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This RoomCategory is already used for a EarlyBirdPromotion, cannot delete this RoomCategory');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymaxaccomodation", "rmcatcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This RoomCategory is already used for a MaximumAccomodation, cannot delete this RoomCategory');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sellspcatd", "rmcatcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This RoomCategory is already used for a SellingformulaForCategory, cannot delete this RoomCategory');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sellspd", "rmcatcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This RoomCategory is already used for a SellingformulaForSuppliers, cannot delete this RoomCategory');", True)
            checkForDeletion = False
            Exit Function

        End If

        checkForDeletion = True
    End Function
#End Region

    Protected Sub ddlAllotmentRequired_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Select Case ddlAllotmentRequired.SelectedIndex
            Case 0
                ddlCalculatebyPax.Enabled = True

            Case 1
                ddlCalculatebyPax.Enabled = False
                ddlCalculatebyPax.SelectedIndex = 1


        End Select


    End Sub

    Protected Sub ddlAllotmentRequired_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAllotmentRequired.TextChanged
        Select Case ddlAllotmentRequired.SelectedIndex
            Case 0
                ddlCalculatebyPax.Enabled = True

            Case 1
                ddlCalculatebyPax.Enabled = False
                ddlCalculatebyPax.SelectedIndex = 1


        End Select
    End Sub

  

    Protected Sub ddlAllotmentRequired_SelectedIndexChanged2(ByVal sender As Object, ByVal e As System.EventArgs)
        Select Case ddlAllotmentRequired.SelectedIndex
            Case 0
                ddlCalculatebyPax.Enabled = True

            Case 1
                ddlCalculatebyPax.Enabled = False
                ddlCalculatebyPax.SelectedIndex = 1


        End Select
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ViewState("Type") = "Acc" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=RoomCategories','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        ElseIf ViewState("Type") = "Supp" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SuppCategory','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
        End If
    End Sub

    Protected Sub ddlcattype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlcattype.SelectedIndexChanged




        If ViewState("Type") = "Acc" Then
            If ddlcattype.SelectedValue = 1 Then
                ddlAllotmentRequired.SelectedValue = "Yes"

            ElseIf ddlcattype.SelectedValue = 2 Then
                ddlAllotmentRequired.SelectedValue = "No"
            Else
                ddlAllotmentRequired.SelectedValue = "Yes"
            End If
        End If
    End Sub

    Protected Sub ddlcattype_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlcattype.TextChanged
        If (ddlcattype.SelectedIndex = 1 Or ddlcattype.SelectedIndex = 2) Then
            ddlMealPlan.SelectedItem.Text = "Yes"
            If ViewState("Type") = "Supp" Then
                lbllinkedmeal.Visible = True
                txtmealname.Visible = True
                txtmealcode.Visible = True
                txtmealcode.Text = ""
                txtmealname.Text = ""
            End If
        Else
            ddlMealPlan.SelectedItem.Text = "No"
            lbllinkedmeal.Visible = False
            txtmealname.Visible = False
            txtmealcode.Visible = False
            txtmealcode.Text = ""
            txtmealname.Text = ""
        End If
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
 <System.Web.Services.WebMethod()> _
    Public Shared Function GetMeals(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim MealNames As New List(Of String)
        Try

            If Trim(prefixText) = "" Then
                strSqlQry = "select mealname,mealcode from mealmast "
            Else
                strSqlQry = "select mealname,mealcode from mealmast where mealname like  " & "'%" & prefixText & "%'"
            End If

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    'MealNames.Add(myDS.Tables(0).Rows(i)("mealname").ToString())
                    MealNames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("mealname").ToString(), myDS.Tables(0).Rows(i)("mealcode").ToString()))
                Next

            End If

            Return MealNames
        Catch ex As Exception
            Return MealNames
        End Try

    End Function


End Class
