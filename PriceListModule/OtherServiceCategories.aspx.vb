'------------================--------------=======================------------------================
'   Module Name    :    OtherServiceCategories.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    16 June 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region



Partial Class OtherServiceCategories
    Inherits System.Web.UI.Page

#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim FindPage As String
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("VehiclePage") = "Yes" Then
            FindPage = "Yes"
        Else
            FindPage = "No"
        End If

        If Page.IsPostBack = False Then

            Try
                'If Not Session("CompanyName") Is Nothing Then
                '    Me.Page.Title = CType(Session("CompanyName"), String)
                'End If

                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                txtconnection.Value = Session("dbconnectionName")

                ViewState.Add("OthcatState", Request.QueryString("State"))
                ViewState.Add("OthcatRefCode", Request.QueryString("RefCode"))
                ' objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlGroup, "othgrpcode", "select othgrpcode from othgrpmast where active=1 order by othgrpcode", True)


                If FindPage = "Yes" Then
                    'strSqlQry = strSqlQry & " WHERE dbo.othcatmast.othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') And  " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othgrpcode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpName, "othgrpname", "othgrpcode", "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othgrpcode", True)
                    If ViewState("OthcatState") = "New" Then
                        ddlOtherGrpCode.SelectedIndex = 0
                        ddlOtherGrpName.SelectedIndex = 0
                    End If
                Else
                    'strSqlQry = strSqlQry & " WHERE dbo.othcatmast.othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') And  " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othgrpcode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpName, "othgrpname", "othgrpcode", "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othgrpcode", True)

                End If

                Numbers(txtOrder)
                Numbers(txtMinPax)
                Numbers(txtMaxPax)
                'charcters1(txtRemark)
                charcters(txtUnitName)
                If ViewState("OthcatState") = "New" Then
                    SetFocus(txtCode)
                    If FindPage = "Yes" Then
                        lblHeading.Text = "Add New Vehicle Type"
                        Page.Title = Page.Title + " " + "New Vehicle Type Master"
                    Else
                        lblHeading.Text = "Add New Other Category"
                        Page.Title = Page.Title + " " + "New Other Category Master"
                    End If
                    
                    btnSave.Text = "Save"
                    txtOrder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(grporder),0) from othcatmast") + 1

                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save category?')==false)return false;")

                    'btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("OthcatState") = "Edit" Then
                    SetFocus(txtName)
                    If FindPage = "Yes" Then
                        lblHeading.Text = "Edit Vehicle Type"
                        Page.Title = Page.Title + " " + "Edit Vehicle Type Master"
                    Else
                        lblHeading.Text = "Edit Other Category"
                        Page.Title = Page.Title + " " + "Edit Other Category Master"
                    End If
                   
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("OthcatRefCode"), String))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update category?')==false)return false;")
                    '                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("OthcatState") = "View" Then
                    SetFocus(btnCancel)
                    If FindPage = "Yes" Then
                        lblHeading.Text = "View Vehicle Type"
                        Page.Title = Page.Title + " " + "View Vehicle Type Master"
                    Else
                        lblHeading.Text = "View Other Category"
                        Page.Title = Page.Title + " " + "View Other Category Master"
                    End If
                   

                    btnSave.Visible = False
                    'btnCancel.Text = "Return to Other Category"
                    DisableControl()

                    ShowRecord(CType(ViewState("OthcatRefCode"), String))

                ElseIf ViewState("OthcatState") = "Delete" Then
                    SetFocus(btnSave)

                    If FindPage = "Yes" Then
                        lblHeading.Text = "Delete Vehicle Type"
                        Page.Title = Page.Title + " " + "Delete Vehicle Type Master"
                    Else
                        lblHeading.Text = "Delete Other Category"
                        Page.Title = Page.Title + " " + "Delete Other Category Master"
                    End If
                  

                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("OthcatRefCode"), String))
                    'btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete category?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                '                btnSave.Attributes.Add("onclick", "return FormValidation()")

                '   ValidateOnlyNumber()
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlOtherGrpCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlOtherGrpName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                'charcters(txtCode)
                'charcters(txtName)
              

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OtherServiceCategoriesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
    End Sub
#End Region


#Region "charcters1"
    Public Sub charcters1(ByVal txtbox As TextBox)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        If ViewState("OthcatState") = "View" Or ViewState("OthcatState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            'ddlGroup.Enabled = False
            'txtOtherGroup.Disabled = True
            ddlOtherGrpCode.Disabled = True
            ddlOtherGrpName.Disabled = True
            txtRemark.Enabled = False
            ddlCalcPax.Enabled = False
            ddladchild.Enabled = False
            txtOrder.Disabled = True
            txtMinPax.Disabled = True
            ChkActive.Disabled = True
            txtUnitName.Disabled = True
            ChkPakReq.Disabled = True
            ChkPrnRemark.Disabled = True
            txtMaxPax.Disabled = True

        ElseIf ViewState("OthcatState") = "Edit" Then
            txtCode.Disabled = True
        End If

    End Sub

#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Try
            If txtCode.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Code field can not be blank.');", True)
                SetFocus(txtCode)
                ValidatePage = False
                Exit Function
            End If
            If txtName.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name field can not be blank.');", True)
                SetFocus(txtName)
                ValidatePage = False
                Exit Function
            End If
            'If ddlGroup.SelectedValue = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select group.');", True)
            '    SetFocus(ddlGroup)
            '    ValidatePage = False
            '    Exit Function
            'End If
            If ddlOtherGrpCode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select group.');", True)
                SetFocus(ddlOtherGrpCode)
                ValidatePage = False
                Exit Function
            End If
            If txtOrder.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Order field can not be blank.');", True)
                SetFocus(txtOrder)
                ValidatePage = False
                Exit Function
            Else
                If Val(txtOrder.Value) <= 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Order can not be less than or equal to zero.');", True)
                    SetFocus(txtOrder)
                    ValidatePage = False
                    Exit Function
                End If
            End If

            If txtdispname.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Display Name field can not be blank.');", True)
                SetFocus(txtdispname)
                ValidatePage = False
                Exit Function
            End If


            If txtMinPax.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('MinPax field can not be blank.');", True)
                SetFocus(txtMinPax)
                ValidatePage = False
                Exit Function
            Else
                If Val(txtMinPax.Value) <= 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('MinPax can not be less than or equal to zero.');", True)
                    SetFocus(txtMinPax)
                    ValidatePage = False
                    Exit Function
                End If
            End If
            If txtMaxPax.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('MaxPax field can not be blank.');", True)
                SetFocus(txtMaxPax)
                ValidatePage = False
                Exit Function
            Else
                If Val(txtMaxPax.Value) < 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('MaxPax can not be less than zero.');", True)
                    SetFocus(txtMaxPax)
                    ValidatePage = False
                    Exit Function
                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
        ValidatePage = True
    End Function
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Try

            If Page.IsValid = True Then
                If ViewState("OthcatState") = "New" Or ViewState("OthcatState") = "Edit" Then
                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("OthcatState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_othcat", mySqlConn, sqlTrans)
                    ElseIf ViewState("OthcatState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_othcat", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othcatname", SqlDbType.VarChar, 150)).Value = CType(txtName.Value.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroup.SelectedValue, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@grporder", SqlDbType.Int, 4)).Value = CType(Val(txtOrder.Value.Trim), Long)
                    mySqlCmd.Parameters.Add(New SqlParameter("@minpax", SqlDbType.Int, 4)).Value = CType(Val(txtMinPax.Value.Trim), Long)
                    mySqlCmd.Parameters.Add(New SqlParameter("@maxpax", SqlDbType.Int, 4)).Value = CType(Val(txtMaxPax.Value.Trim), Long)
                    mySqlCmd.Parameters.Add(New SqlParameter("@unitname", SqlDbType.VarChar, 10)).Value = CType(txtUnitName.Value.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(txtRemark.Text.Trim, String)


                    If ChkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf ChkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If

                    If ChkPrnRemark.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@printremarks", SqlDbType.Int)).Value = 1
                    ElseIf ChkPrnRemark.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@printremarks", SqlDbType.Int)).Value = 0
                    End If


                    If ChkPakReq.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@paxcheckReqd", SqlDbType.Int)).Value = 1
                    ElseIf ChkPakReq.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@paxcheckReqd", SqlDbType.Int)).Value = 0
                    End If


                    mySqlCmd.Parameters.Add(New SqlParameter("@calcyn", SqlDbType.VarChar, 10)).Value = "" 'CType(ddlCalcPax.SelectedValue, String)"
                    'Select Case CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ratesbasedonpax from othgrpmast where active=1 and othgrpcode='" & CType(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text, String) & "'"), String)
                    '    Case "1"
                    '        Select Case CType(ddladchild.SelectedIndex, String)
                    '            Case 0
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@adultchild", SqlDbType.VarChar, 10)).Value = "A"

                    '            Case 1
                    '                mySqlCmd.Parameters.Add(New SqlParameter("@adultchild", SqlDbType.VarChar, 10)).Value = "C"

                    '        End Select



                    '    Case Else
                    mySqlCmd.Parameters.Add(New SqlParameter("@adultchild", SqlDbType.VarChar, 10)).Value = ""

                    'End Select
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@dispname", SqlDbType.VarChar, 50)).Value = CType(txtdispname.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@Capacity", SqlDbType.Text)).Value = ""
                    mySqlCmd.Parameters.Add(New SqlParameter("@Options", SqlDbType.Text)).Value = ""
                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("OthcatState") = "Delete" Then

                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_othcat", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@othcatcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("OtherServiceCategoriesSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('OthcatWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("OtherServiceCategories.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from othcatmast Where othcatcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("othcatcode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("othcatcode"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("othcatname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("othcatname"), String)
                    Else
                        Me.txtName.Value = ""
                    End If
                    'If IsDBNull(mySqlReader("othgrpcode")) = False Then
                    '    Me.ddlGroup.SelectedValue = CType(mySqlReader("othgrpcode"), String)
                    '    Me.txtOtherGroup.Value = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"othgrpmast", "othgrpname", "othgrpcode", ddlGroup.SelectedValue), String)
                    'End If
                    If IsDBNull(mySqlReader("othgrpcode")) = False Then
                        ddlOtherGrpCode.Value = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othgrpmast", "othgrpname", "othgrpcode", CType(mySqlReader("othgrpcode"), String)), String)
                        ddlOtherGrpName.Value = CType(mySqlReader("othgrpcode"), String)
                    Else
                        ddlOtherGrpCode.Value = "[Select]"
                        ddlOtherGrpName.Value = "[Select]"
                    End If

                    If IsDBNull(mySqlReader("grporder")) = False Then
                        txtOrder.Value = mySqlReader("grporder")
                    Else
                        Me.txtOrder.Value = ""
                    End If
                    If IsDBNull(mySqlReader("remarks")) = False Then
                        txtRemark.Text = mySqlReader("remarks")
                    Else
                        Me.txtRemark.Text = ""
                    End If
                    If IsDBNull(mySqlReader("minpax")) = False Then
                        Me.txtMinPax.Value = mySqlReader("minpax")
                    Else
                        Me.txtMinPax.Value = ""
                    End If
                    If IsDBNull(mySqlReader("unitname")) = False Then
                        Me.txtUnitName.Value = mySqlReader("unitname")
                    Else
                        Me.txtUnitName.Value = ""
                    End If
                    If IsDBNull(mySqlReader("dispname")) = False Then
                        Me.txtdispname.Value = mySqlReader("dispname")
                    Else
                        Me.txtdispname.Value = ""
                    End If



                    If IsDBNull(mySqlReader("calcyn")) = False Then
                        Me.ddlCalcPax.SelectedValue = mySqlReader("calcyn")
                    End If

                    'Select Case CType(objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select ratesbasedonpax from othgrpmast where active=1 and othgrpcode='" & CType(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text, String) & "'"), String)
                    '    Case "1"

                    '        Select Case mySqlReader("adultchild")
                    '            Case "A"
                    '                Me.ddladchild.SelectedIndex = 0
                    '            Case "C"
                    '                Me.ddladchild.SelectedIndex = 1
                    '        End Select

                    '    Case Else

                    '        Me.ddladchild.Style("display") = "none"
                    '        Me.lbladult.Style("display") = "none"
                    'End Select





                    If IsDBNull(mySqlReader("maxpax")) = False Then
                        Me.txtMaxPax.Value = mySqlReader("maxpax")
                    Else
                        Me.txtMaxPax.Value = ""
                    End If

                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            ChkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            ChkActive.Checked = False
                        End If
                    End If



                    If IsDBNull(mySqlReader("paxcheckreqd")) = False Then
                        If CType(mySqlReader("paxcheckreqd"), String) = "1" Then
                            ChkPakReq.Checked = True
                        ElseIf CType(mySqlReader("paxcheckreqd"), String) = "0" Then
                            ChkPakReq.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("printRemarks")) = False Then
                        If CType(mySqlReader("printRemarks"), String) = "1" Then
                            ChkPrnRemark.Checked = True
                        ElseIf CType(mySqlReader("printRemarks"), String) = "0" Then
                            ChkPrnRemark.Checked = False
                        End If
                    End If



                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServiceCategories.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("OtherServiceCategoriesSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If ViewState("OthcatState") = "New" Then
            'If objUtils.isDuplicatenew(Session("dbconnectionName"), "othcatmast", "othcatcode", CType(txtCode.Value.Trim, String)) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other categories code is already present.');", True)
            '    checkForDuplicate = False
            '    Exit Function
            'End If
            If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from othcatmast where othcatcode ='" & CType(txtCode.Value.Trim, String) & "' and othgrpcode ='" & CType(ddlOtherGrpName.Value, String) & "'") <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other categories code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If

            'If objUtils.isDuplicatenew(Session("dbconnectionName"), "othcatmast", "othcatname", txtName.Value.Trim) Then
            If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from othcatmast where othcatname ='" & CType(txtName.Value.Trim, String) & "' and othgrpcode ='" & CType(ddlOtherGrpName.Value, String) & "'") <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other categories name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("OthcatState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "othcatmast", "othcatcode", "othcatname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other categories name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
          





        End If
        checkForDuplicate = True
    End Function
#End Region



    'Protected Sub ddlGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        Me.txtOtherGroup.Value = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"othgrpmast", "othgrpname", "othgrpcode", ddlGroup.SelectedValue), String)
    '        SetFocus(txtOrder)
    '    Catch ex As Exception

    '    End Try
    'End Sub

#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        'If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyothcat", "othcatcode", CType(txtCode.Value.Trim, String)) = True Then
        If objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from partyothcat where othcatcode ='" & CType(txtCode.Value.Trim, String) & "' and othgrpcode ='" & CType(ddlOtherGrpName.Value, String) & "'") <> "" Then

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Category is already used for a OtherServices Category of suppliers, cannot delete this Category');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplistdnew", "othcatcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Category is already used for a OtherServices PriceList, cannot delete this Category');", True)
            checkForDeletion = False
            Exit Function

            'ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplist_costd", "othcatcode", CType(txtCode.Value.Trim, String)) = True Then
        ElseIf objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from othplist_selld where othcatcode ='" & CType(txtCode.Value.Trim, String) & "' and othgrpcode ='" & CType(ddlOtherGrpName.Value, String) & "'") <> "" Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Category is already used for a OtherServices  PriceList, cannot delete this Category');", True)
            checkForDeletion = False
            Exit Function

            'ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplistd", "othcatcode", CType(txtCode.Value.Trim, String)) = True Then
        ElseIf objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select 't' from oplistd where othcatcode ='" & CType(txtCode.Value.Trim, String) & "' and othgrpcode ='" & CType(ddlOtherGrpName.Value, String) & "'") <> "" Then

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Category is already used for a Details of OtherServices  PriceList, cannot delete this Category');", True)
            checkForDeletion = False
            Exit Function
        End If

        checkForDeletion = True
    End Function
#End Region

    Protected Sub ddlCalcPax_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherServiceCategories','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
