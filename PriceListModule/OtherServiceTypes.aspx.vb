'------------================--------------=======================------------------================
'   Module Name    :    OtherServiceTypes.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    16 June 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region



Partial Class OtherServiceTypes
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
                'If Not Session("CompanyName") Is Nothing Then
                '    Me.Page.Title = CType(Session("CompanyName"), String)
                'End If

                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                txtconnection.Value = Session("dbconnectionName")
                ViewState.Add("OthtypeState", Request.QueryString("State"))
                ViewState.Add("OthtypeRefCode", Request.QueryString("RefCode"))
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlGroup, "othgrpcode", "select othgrpcode from othgrpmast where active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpName, "othgrpname", "othgrpcode", "select othgrpcode,othgrpname from othgrpmast where active=1 order by othgrpcode", True)
                txtTrns.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id =564")
                If ViewState("OthtypeState") = "New" Then
                    SetFocus(txtCode)
                    lblHeading.Text = "Add New Other Types"
                    Page.Title = Page.Title + " " + "New Other Types Master"
                    btnSave.Text = "Save"
                    txtOrder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(rankorder),0) from othtypmast") + 1

                    btnSave.Attributes.Add("onclick", "return Validate('New')")
                    ' btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    ddlPName.Disabled = True
                    ddlDName.Disabled = True
                    ddlType.Disabled = True
                ElseIf ViewState("OthtypeState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Other Types"
                    Page.Title = Page.Title + " " + "Edit Other Types Master"
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("OthtypeRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return Validate('Edit')")
                    'btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("OthtypeState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Other Types"
                    Page.Title = Page.Title + " " + "View Other Types Master"
                    btnSave.Visible = False
                    'btnCancel.Text = "Return to Other Types"
                    DisableControl()

                    ShowRecord(CType(ViewState("OthtypeRefCode"), String))

                ElseIf ViewState("OthtypeState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Other Types"
                    Page.Title = Page.Title + " " + "Delete Other Types Master"
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("OthtypeRefCode"), String))
                    'btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "return Validate('Delete')")
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
                Numbers(txtOrder)
                Numbers(txtMinPax)
                charcters1(txtRemark)


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("OtherServiceTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            If ddlType.Value = "0" Then
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='A' order by name", True, hdnP.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='S' order by name", True, hdnD.Value)
            ElseIf ddlType.Value = "1" Then
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='S' order by name", True, hdnP.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='A' order by name", True, hdnD.Value)
            ElseIf ddlType.Value = "2" Then
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='S' order by name", True, hdnP.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='S' order by name", True, hdnD.Value)
            ElseIf ddlType.Value = "3" Then
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='A' order by name", True, hdnP.Value)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='A' order by name", True, hdnD.Value)
            End If
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
        If ViewState("OthtypeState") = "View" Or ViewState("OthtypeState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            'ddlGroup.Enabled = False
            'txtOtherGroup.Disabled = True
            ddlOtherGrpCode.Disabled = True
            ddlOtherGrpName.Disabled = True
            txtRemark.Enabled = False
            txtOrder.Disabled = True
            txtMinPax.Disabled = True
            ChkInactive.Disabled = True
            ChkPrnConfirm.Disabled = True
            ChkPakReq.Disabled = True
            ChkPrnRemark.Disabled = True
            ChkAutoCancel.Disabled = True
            ddlPName.Disabled = True
            ddlDName.Disabled = True
            ddlType.Disabled = True
        ElseIf ViewState("OthtypeState") = "Edit" Then
            txtCode.Disabled = True
        End If

    End Sub

#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Try
            If txtCode.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Code field can not bee blank.');", True)
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
            If ddlOtherGrpName.Value = txtTrns.Value Then
                If ddlType.Value = "" Or ddlType.Value = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('please Select Type.');", True)
                    SetFocus(ddlType)
                    ValidatePage = False
                    Exit Function
                End If
                If ddlPName.Value = "" Or ddlPName.Value = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select pick up.');", True)
                    SetFocus(ddlPName)
                    ValidatePage = False
                    Exit Function
                End If
                If ddlDName.Value = "" Or ddlDName.Value = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select drop off.');", True)
                    SetFocus(ddlDName)
                    ValidatePage = False
                    Exit Function
                End If
            End If

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
            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Page.IsValid = True Then
                If ViewState("OthtypeState") = "New" Or ViewState("OthtypeState") = "Edit" Then
                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("OthtypeState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_othtyp", mySqlConn, sqlTrans)
                    ElseIf ViewState("OthtypeState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_othtyp", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypname", SqlDbType.VarChar, 150)).Value = CType(txtName.Value.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlGroup.SelectedValue, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@rankorder", SqlDbType.Int, 4)).Value = CType(txtOrder.Value.Trim, Long)
                    mySqlCmd.Parameters.Add(New SqlParameter("@minpax", SqlDbType.Int, 4)).Value = CType(txtMinPax.Value.Trim, Long)




                    If ChkPrnConfirm.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@printconf", SqlDbType.Int)).Value = 1
                    ElseIf ChkPrnConfirm.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@printconf", SqlDbType.Int)).Value = 0
                    End If
                    If ChkPakReq.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@paxcheckreq", SqlDbType.Int)).Value = 1
                    ElseIf ChkPakReq.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@paxcheckreq", SqlDbType.Int)).Value = 0
                    End If
                    If ChkPrnRemark.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@printremarks", SqlDbType.Int)).Value = 1
                    ElseIf ChkPrnRemark.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@printremarks", SqlDbType.Int)).Value = 0
                    End If

                    If ChkAutoCancel.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@autocancelreq", SqlDbType.Int)).Value = 1
                    ElseIf ChkAutoCancel.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@autocancelreq", SqlDbType.Int)).Value = 0
                    End If
                    If ChkInactive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf ChkInactive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(txtRemark.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    If ddlOtherGrpName.Value = txtTrns.Value Or ddlOtherGrpCode.Items(ddlOtherGrpCode.SelectedIndex).Text = txtTrns.Value Then
                        If ddlType.Value = "" Or ddlType.Value = "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@transfertype", SqlDbType.VarChar, 10)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@transfertype", SqlDbType.VarChar, 10)).Value = ddlType.Value
                        End If
                        If ddlPName.Value = "" Or ddlPName.Value = "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@pickuppoint", SqlDbType.VarChar, 10)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@pickuppoint", SqlDbType.VarChar, 10)).Value = ddlPName.Value
                        End If
                        If ddlDName.Value = "" Or ddlDName.Value = "[Select]" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@dropoffpoint", SqlDbType.VarChar, 10)).Value = DBNull.Value
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@dropoffpoint", SqlDbType.VarChar, 10)).Value = ddlDName.Value
                        End If
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@transfertype", SqlDbType.VarChar, 10)).Value = DBNull.Value
                        mySqlCmd.Parameters.Add(New SqlParameter("@pickuppoint", SqlDbType.VarChar, 10)).Value = DBNull.Value
                        mySqlCmd.Parameters.Add(New SqlParameter("@dropoffpoint", SqlDbType.VarChar, 10)).Value = DBNull.Value
                    End If


                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("OthtypeState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_othtyp", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("OtherServiceTypesSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('OthtypeWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If

            objUtils.WritErrorLog("OtherServiceTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from othtypmast Where othtypcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("othtypcode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("othtypcode"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("othtypname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("othtypname"), String)
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


                    If IsDBNull(mySqlReader("rankorder")) = False Then
                        txtOrder.Value = mySqlReader("rankorder")
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

                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            ChkInactive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            ChkInactive.Checked = False
                        End If
                    End If


                    If IsDBNull(mySqlReader("printconf")) = False Then
                        If CType(mySqlReader("printconf"), String) = "1" Then
                            ChkPrnConfirm.Checked = True
                        ElseIf CType(mySqlReader("printconf"), String) = "0" Then
                            ChkPrnConfirm.Checked = False
                        End If
                    End If


                    If IsDBNull(mySqlReader("paxcheckReq")) = False Then
                        If CType(mySqlReader("paxcheckReq"), String) = "1" Then
                            ChkPakReq.Checked = True
                        ElseIf CType(mySqlReader("paxcheckReq"), String) = "0" Then
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

                    If IsDBNull(mySqlReader("autocancelreq")) = False Then
                        If CType(mySqlReader("autocancelreq"), String) = "1" Then
                            ChkAutoCancel.Checked = True
                        ElseIf CType(mySqlReader("autocancelreq"), String) = "0" Then
                            ChkAutoCancel.Checked = False
                        End If
                    End If

                    If mySqlReader("othgrpcode") = txtTrns.Value Then
                        If IsDBNull(mySqlReader("pickuppoint")) = False Then
                            hdnP.Value = mySqlReader("pickuppoint")
                        End If
                        If IsDBNull(mySqlReader("dropoffpoint")) = False Then
                            hdnD.Value = mySqlReader("dropoffpoint")
                        End If
                        If IsDBNull(mySqlReader("transfertype")) = False Then
                            ddlType.Value = mySqlReader("transfertype")
                            If mySqlReader("transfertype") = "0" Then
                                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='A' order by name", True, hdnP.Value)
                                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='S' order by name", True, hdnD.Value)
                            ElseIf mySqlReader("transfertype") = "1" Then
                                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='S' order by name", True, hdnP.Value)
                                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='A' order by name", True, hdnD.Value)
                            ElseIf mySqlReader("transfertype") = "2" Then
                                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='S' order by name", True, hdnP.Value)
                                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='S' order by name", True, hdnD.Value)
                            ElseIf mySqlReader("transfertype") = "3" Then
                                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='A' order by name", True, hdnP.Value)
                                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='A' order by name", True, hdnD.Value)
                            End If
                        End If
                    Else
                        ddlPName.Disabled = True
                        ddlDName.Disabled = True
                        ddlType.Disabled = True
                    End If

                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServiceTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("OtherServiceTypesSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If ViewState("OthtypeState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "othtypmast", "othtypcode", CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other type code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "othtypmast", "othtypname", txtName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other type name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "othtypmast", "rankorder", CType(txtOrder.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other type Order is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("OthtypeState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "othtypmast", "othtypcode", "othtypname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other type name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "othtypmast", "othtypcode", "rankorder", CType(txtOrder.Value.Trim, String), CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other type Order is already present.');", True)
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
    '    Catch ex As Exception

    '    End Try
    'End Sub

#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyothtyp", "othtypcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This ServiceType is already used for a OtherServicesTypes of suppliers, cannot delete this ServiceTypes');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), " oplistdnew", "othtypcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This ServiceType is already used for a OtherServicesPriceList, cannot delete this ServiceTypes');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), " oplist_costd", "othtypcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This ServiceType is already used for a OtherServices CostPriceList, cannot delete this ServiceTypes');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), " oplistd", "othtypcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This ServiceType is already used for a DetailsofOtherServices CostPriceList, cannot delete this ServiceTypes');", True)
            checkForDeletion = False
            Exit Function
        End If

        checkForDeletion = True
    End Function
#End Region


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherServiceTypes','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
