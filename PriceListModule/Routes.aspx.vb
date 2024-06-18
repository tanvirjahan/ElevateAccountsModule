#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class PriceListModule_Routes
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
                btnselect.Visible = False
                btnSelectAll.Visible = False
                btnunselect.Visible = False
                btnUnselectAll.Visible = False

                txtCode.Disabled = True


                ddlServerType.Items.Clear()
                ddlServerType.Items.Add("[Select]")
                ddlServerType.Items.Add("Arrival Borders")
                ddlServerType.Items.Add("Departure Borders")
                ddlServerType.Items.Add("Internal Transfer/Excursion")
                ddlServerType.Items.Add("Arrival/Departure Transfer Borders")

                ddlNewServerType.Items.Clear()
                ddlNewServerType.Items.Add("[Select]")
                ddlNewServerType.Items.Add("Airport / Borders")
                ddlNewServerType.Items.Add("Hotel Sectors")
                ddlNewServerType.Items.Add("Hotels")


                ddlNewServerType0.Items.Clear()
                ddlNewServerType0.Items.Add("[Select]")
                ddlNewServerType0.Items.Add("Airport / Borders")
                ddlNewServerType0.Items.Add("Hotel Sectors")
                ddlNewServerType0.Items.Add("Hotels")

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
                txtTrns.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id =1001")
                If ViewState("OthtypeState") = "New" Then
                    SetFocus(txtCode)
                    txtMinPax.Value = 1
                    DisableControl()
                    lblHeading.Text = "Add New Routes"
                    Page.Title = Page.Title + " " + "New Routes Master"
                    btnSave.Text = "Save"
                    txtOrder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(rankorder),0) from othtypmast") + 1

                    btnSave.Attributes.Add("onclick", "return Validate('New')")
                    ' btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    'ddlPName.Disabled = True
                    'ddlDName.Disabled = True
                    'ddlType.Disabled = True
                ElseIf ViewState("OthtypeState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Routes"
                    Page.Title = Page.Title + " " + "Edit Routes Master"
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("OthtypeRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return Validate('Edit')")
                    'btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("OthtypeState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Routes"
                    Page.Title = Page.Title + " " + "View Routes Master"
                    btnSave.Visible = False
                    'btnCancel.Text = "Return to Other Types"
                    DisableControl()

                    ShowRecord(CType(ViewState("OthtypeRefCode"), String))

                ElseIf ViewState("OthtypeState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Routes"
                    Page.Title = Page.Title + " " + "Delete Routes Master"
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
                'charcters(txtRemark)
                'txtRemark.Attributes.Add("onkeypress", "return checkCharacter(event)")


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("RoutesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        Else
            'If ddlType.Value = "0" Then
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='A' order by name", True, hdnP.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='S' order by name", True, hdnD.Value)
            'ElseIf ddlType.Value = "1" Then
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='S' order by name", True, hdnP.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='A' order by name", True, hdnD.Value)
            'ElseIf ddlType.Value = "2" Then
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='S' order by name", True, hdnP.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='S' order by name", True, hdnD.Value)
            'ElseIf ddlType.Value = "3" Then
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlPName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='A' order by name", True, hdnP.Value)
            '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDName, "name", "code", "select code,name from view_pickupdropoff where Pickuptype='A' order by name", True, hdnD.Value)
            'End If
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
        'currently all chk boxes are not necessary-- so disabled and unchkd (for all conditons)

        ChkPrnConfirm.Checked = False
        ChkPakReq.Checked = False
        ChkPrnRemark.Checked = False
        ChkAutoCancel.Checked = False

        If ViewState("OthtypeState") = "View" Or ViewState("OthtypeState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            'ddlGroup.Enabled = False
            'txtOtherGroup.Disabled = True
            'ddlOtherGrpCode.Disabled = True
            'ddlOtherGrpName.Disabled = True
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
            ddlNewServerType0.Enabled = False
            ddlNewServerType.Enabled = False

            btnselect.Visible = False
            btnselectAll.Visible = False
            btnunselect.Visible = False
            btnUnselectAll.Visible = False

            '  gv_SearchResult.Enabled = False
            ddlServerType.Enabled = False
            'ddlType.Disabled = True
            Chkshuttle.Disabled = True
        ElseIf ViewState("OthtypeState") = "Edit" Then
            txtCode.Disabled = True
        End If

    End Sub

#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Try
            'If txtCode.Value = "" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Code field can not bee blank.');", True)
            '    SetFocus(txtCode)
            '    ValidatePage = False
            '    Exit Function
            'End If
            If txtName.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name field can not be blank.');", True)
                SetFocus(txtName)
                ValidatePage = False
                Exit Function
            End If
            If ddlServerType.SelectedIndex <= 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('please Select Transfer Type.');", True)
                SetFocus(ddlServerType)
                ValidatePage = False
                Exit Function
            End If
            If ddlNewServerType.SelectedIndex <= 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('please Select Pick Up type.');", True)
                SetFocus(ddlNewServerType)
                ValidatePage = False
                Exit Function
            End If
            If ddlNewServerType0.SelectedIndex <= 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('please Select Drop type.');", True)
                SetFocus(ddlNewServerType0)
                ValidatePage = False
                Exit Function
            End If
            'If ddlOtherGrpName.Value = txtTrns.Value Then
            '    If ddlType.Value = "" Or ddlType.Value = "[Select]" Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('please Select Type.');", True)
            '        SetFocus(ddlType)
            '        ValidatePage = False
            '        Exit Function
            '    End If
            '    If ddlPName.Value = "" Or ddlPName.Value = "[Select]" Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select pick up.');", True)
            '        SetFocus(ddlPName)
            '        ValidatePage = False
            '        Exit Function
            '    End If
            '    If ddlDName.Value = "" Or ddlDName.Value = "[Select]" Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select drop off.');", True)
            '        SetFocus(ddlDName)
            '        ValidatePage = False
            '        Exit Function
            '    End If
            'End If

            'If ddlOtherGrpCode.Value = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select group.');", True)
            '    SetFocus(ddlOtherGrpCode)
            '    ValidatePage = False
            '    Exit Function
            'End If
            Dim chkSel As New CheckBox
            Dim FlgCk As Boolean = False
            For Each GvRow In GrdPicDrop.Rows
                chkSel = GvRow.FindControl("chkSelect")
                If chkSel.Checked = True Then
                    FlgCk = True
                    Exit For
                End If
            Next

            If FlgCk = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Any One Drop Point');", True)
                ValidatePage = False
                Exit Function
            End If
            FlgCk = False
            For Each GvRow In gv_SearchResult.Rows
                chkSel = GvRow.FindControl("chkSelect")
                If chkSel.Checked = True Then
                    FlgCk = True
                    Exit For
                End If
            Next
            If FlgCk = False Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Any One Pick Point');", True)
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
            If txtRemark.Text = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Brief description of Itinerary can not be blank.');", True)
                SetFocus(txtRemark)
                ValidatePage = False
                Exit Function
            End If
            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            ValidatePage = False
        End Try
    End Function
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

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
                        Dim optionval As String

                        optionval = objUtils.GetAutoDocNo("ROUTEMAST", mySqlConn, sqlTrans)
                        txtCode.Value = optionval.Trim

                        mySqlCmd = New SqlCommand("sp_add_othtyp", mySqlConn, sqlTrans)
                    ElseIf ViewState("OthtypeState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_othtyp", mySqlConn, sqlTrans)
                    End If

                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypname", SqlDbType.VarChar, 150)).Value = CType(txtName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(txtTrns.Value, String)
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
                    If ddlServerType.SelectedIndex > 0 Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@transfertype", SqlDbType.Int)).Value = ddlServerType.SelectedIndex
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@transfertype", SqlDbType.Int)).Value = DBNull.Value
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@PickUpPoint", SqlDbType.Int)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@DropOffPoint", SqlDbType.Int)).Value = DBNull.Value
                    If Chkshuttle.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@shuttle", SqlDbType.Int)).Value = 1
                    ElseIf ChkPakReq.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@shuttle", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.ExecuteNonQuery()


                    If ViewState("OthtypeState") = "Edit" Then
                        mySqlCmd = New SqlCommand("Sp_Del_othtyp_Transfers", mySqlConn, sqlTrans)
                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                        mySqlCmd.ExecuteNonQuery()
                    End If
                    Dim chksel As New CheckBox

                    For Each GvRow In gv_SearchResult.Rows

                        chksel = GvRow.FindControl("chkSelect")
                        If chksel.Checked = True Then
                            If ViewState("OthtypeState") = "New" Then
                                mySqlCmd = New SqlCommand("Sp_Add_Othtyp_Transfers ", mySqlConn, sqlTrans)
                            ElseIf ViewState("OthtypeState") = "Edit" Then
                                'mySqlCmd = New SqlCommand("Sp_Mod_othtyp_Transfers", mySqlConn, sqlTrans)
                                mySqlCmd = New SqlCommand("Sp_Add_Othtyp_Transfers ", mySqlConn, sqlTrans)
                            End If
                            mySqlCmd.CommandType = CommandType.StoredProcedure

                            mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)

                            If ddlNewServerType.SelectedIndex > 0 Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@PickType", SqlDbType.Int)).Value = ddlNewServerType.SelectedIndex
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@PickType", SqlDbType.Int)).Value = DBNull.Value
                            End If
                            If ddlNewServerType0.SelectedIndex > 0 Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@DropType", SqlDbType.Int)).Value = ddlNewServerType0.SelectedIndex
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@DropType", SqlDbType.Int)).Value = DBNull.Value
                            End If
                            mySqlCmd.Parameters.Add(New SqlParameter("@PickupDropUppoint", SqlDbType.VarChar, 100)).Value = gv_SearchResult.Rows(GvRow.RowIndex).Cells(1).Text
                            mySqlCmd.Parameters.Add(New SqlParameter("@PickDrop", SqlDbType.VarChar, 20)).Value = "P"
                            mySqlCmd.ExecuteNonQuery()

                        End If
                    Next

                    For Each GvRow In GrdPicDrop.Rows

                        chksel = GvRow.FindControl("chkSelect")
                        If chksel.Checked = True Then

                            If ViewState("OthtypeState") = "New" Then
                                mySqlCmd = New SqlCommand("Sp_Add_Othtyp_Transfers ", mySqlConn, sqlTrans)
                            ElseIf ViewState("OthtypeState") = "Edit" Then
                                mySqlCmd = New SqlCommand("Sp_Add_Othtyp_Transfers ", mySqlConn, sqlTrans)
                            End If
                            mySqlCmd.CommandType = CommandType.StoredProcedure
                            mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)

                            If ddlNewServerType.SelectedIndex > 0 Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@PickType", SqlDbType.Int)).Value = ddlNewServerType.SelectedIndex
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@PickType", SqlDbType.Int)).Value = DBNull.Value
                            End If
                            If ddlNewServerType0.SelectedIndex > 0 Then
                                mySqlCmd.Parameters.Add(New SqlParameter("@DropType", SqlDbType.Int)).Value = ddlNewServerType0.SelectedIndex
                            Else
                                mySqlCmd.Parameters.Add(New SqlParameter("@DropType", SqlDbType.Int)).Value = DBNull.Value
                            End If
                            mySqlCmd.Parameters.Add(New SqlParameter("@PickupDropUppoint", SqlDbType.VarChar, 100)).Value = GrdPicDrop.Rows(GvRow.RowIndex).Cells(1).Text
                            mySqlCmd.Parameters.Add(New SqlParameter("@PickDrop", SqlDbType.VarChar, 20)).Value = "D"
                            mySqlCmd.ExecuteNonQuery()

                        End If
                    Next



                ElseIf ViewState("OthtypeState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("Sp_Del_othtyp_Transfers", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()

                    mySqlCmd = New SqlCommand("sp_del_othtyp", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()


                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("RoutesSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('OthtypeWindowPostBack', '');window.close();window.opener.focus();"
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

                    If IsDBNull(mySqlReader("TransferType")) = False Then
                        ddlServerType.SelectedIndex = CType(mySqlReader("TransferType"), Integer)
                    Else
                        ddlServerType.SelectedIndex = 0
                    End If
                    If IsDBNull(mySqlReader("shuttle")) = False Then
                        If mySqlReader("shuttle") = "1" Then
                            Chkshuttle.Checked = True
                        Else
                            Chkshuttle.Checked = False
                        End If
                    End If


                End If
            End If




            Dim chksel As New CheckBox

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from othtypmastTransfers Where othtypcode='" & RefCode & "' And PickDrop='D'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read() = True

               

                    If IsDBNull(mySqlReader("DropType")) = False Then
                        ddlNewServerType0.SelectedIndex = CType(mySqlReader("DropType"), Integer)
                    Else
                        ddlNewServerType0.SelectedIndex = 0
                    End If

                   

                    If GrdPicDrop.Rows.Count = 0 Then
                        Dim Dt As DataSet = New DataSet
                        If ddlNewServerType0.SelectedIndex = 1 Then
                            Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select airportbordercode As Code,airportbordername As [Name] from airportbordersmaster order by airportbordername")
                        ElseIf ddlNewServerType0.SelectedIndex = 2 Then
                            Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select S.SectorCode As Code,S.SectorName from sectormaster S order by S.SectorName ")

                            ' Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select S.SectorCode As Code,S.SectorName,P.PartyName As [Hotel Name] from partymast P Inner Join sectormaster S On S.SectorCode=P.SectorCode where P.SpTypeCode='HOT' order by S.SectorName")
                        ElseIf ddlNewServerType0.SelectedIndex = 3 Then
                            '  Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select P.PartyCode As Code,P.PartyName As [Hotel Name] from partymast P  where P.SpTypeCode='HOT' order by P.PartyCode")
                            Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select P.PartyCode As Code,S.SectorName,P.PartyName As [Hotel Name] from partymast P Inner Join sectormaster S On S.SectorCode=P.SectorCode where P.SpTypeCode='HOT' order by S.SectorName")

                        End If
                        If Dt.Tables.Count > 0 Then
                            GrdPicDrop.DataSource = Dt.Tables(0)
                            GrdPicDrop.DataBind()
                            If ViewState("OthtypeState") = "View" Or ViewState("OthtypeState") = "Delete" Then
                                btnSelectAll.Visible = False
                                btnUnselectAll.Visible = False
                            Else
                                btnSelectAll.Visible = True
                                btnUnselectAll.Visible = True
                            End If
                          
                        Else
                            GrdPicDrop.DataSource = Nothing
                            GrdPicDrop.DataBind()
                            btnSelectAll.Visible = False
                            btnUnselectAll.Visible = False
                            ddlNewServerType0.SelectedIndex = 0
                        End If
                    End If

                    Try
                        For Each GvRow In GrdPicDrop.Rows
                            If CType(mySqlReader("PickUpDropUpPoint"), String) = GvRow.Cells(1).Text.ToString Then
                                chksel = GvRow.FindControl("chkSelect")
                                chksel.Checked = True
                                Exit For
                            End If
                        Next
                    Catch ex As Exception
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)
                    End Try
                End While
            End If

            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from othtypmastTransfers Where othtypcode='" & RefCode & "' And PickDrop='P'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                While mySqlReader.Read() = True
                    If IsDBNull(mySqlReader("PickType")) = False Then
                        ddlNewServerType.SelectedIndex = CType(mySqlReader("PickType"), Integer)
                    Else
                        ddlNewServerType.SelectedIndex = 0
                    End If

                    If gv_SearchResult.Rows.Count = 0 Then
                        Dim Dt As DataSet = New DataSet
                        If ddlNewServerType.SelectedIndex = 1 Then
                            Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select airportbordercode As Code,airportbordername As [Name] from airportbordersmaster order by airportbordername")
                        ElseIf ddlNewServerType.SelectedIndex = 2 Then
                            Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select S.SectorCode As Code,S.SectorName from sectormaster S order by S.SectorName ")

                            '  Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select S.SectorCode As Code,S.SectorName,P.PartyName As [Hotel Name] from partymast P Inner Join sectormaster S On S.SectorCode=P.SectorCode where P.SpTypeCode='HOT' order by S.SectorName")
                        ElseIf ddlNewServerType.SelectedIndex = 3 Then
                            '  Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select P.PartyCode As Code,P.PartyName As [Hotel Name] from partymast P  where P.SpTypeCode='HOT' order by P.PartyCode")
                            Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select P.PartyCode As Code,S.SectorName,P.PartyName As [Hotel Name] from partymast P Inner Join sectormaster S On S.SectorCode=P.SectorCode where P.SpTypeCode='HOT' order by S.SectorName")

                        End If
                        If Dt.Tables.Count > 0 Then
                            gv_SearchResult.DataSource = Dt.Tables(0)
                            gv_SearchResult.DataBind()
                            If ViewState("OthtypeState") = "View" Or ViewState("OthtypeState") = "Delete" Then
                                btnselect.Visible = False
                                btnunselect.Visible = False
                            Else
                                btnselect.Visible = True
                                btnunselect.Visible = True
                            End If
                          

                        Else
                            gv_SearchResult.DataSource = Nothing
                            gv_SearchResult.DataBind()
                            btnselect.Visible = False
                            btnunselect.Visible = False
                            ddlNewServerType.SelectedIndex = 0
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Records Found');", True)
                        End If
                    End If
                    Try
                        For Each GvRow In gv_SearchResult.Rows
                            If CType(mySqlReader("PickUpDropUpPoint"), String) = GvRow.Cells(1).Text.ToString Then
                                chksel = GvRow.FindControl("chkSelect")
                                chksel.Checked = True
                                Exit For
                            End If
                        Next
                    Catch ex As Exception
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)
                    End Try

                End While
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Routes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("RoutesSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        Dim strFilter As String
        strFilter = "othgrpcode= (select option_selected  from reservation_parameters where param_id =1001)"

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
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "othtypmast", "rankorder", CType(txtOrder.Value.Trim, String), strFilter) Then
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
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "othtypmast", "othtypcode", "rankorder", CType(txtOrder.Value.Trim, String), CType(txtCode.Value.Trim, String), strFilter) Then
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


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Routes','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

   

    Protected Sub ddlNewServerType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlNewServerType.SelectedIndexChanged
        Try
            Dim Dt As DataSet = New DataSet
            If ddlNewServerType.SelectedIndex = 1 Then
                Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select airportbordercode As Code,airportbordername As [Name] from airportbordersmaster order by airportbordername")
            ElseIf ddlNewServerType.SelectedIndex = 2 Then
                Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select S.SectorCode As Code,S.SectorName from sectormaster S order by S.SectorName ")

                ' Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select S.SectorCode As Code,S.SectorName,P.PartyName As [Hotel Name] from partymast P Inner Join sectormaster S On S.SectorCode=P.SectorCode where P.SpTypeCode='HOT' order by S.SectorName")
            ElseIf ddlNewServerType.SelectedIndex = 3 Then
                '  Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select P.PartyCode As Code,P.PartyName As [Hotel Name] from partymast P  where P.SpTypeCode='HOT' order by P.PartyCode")
                Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select P.PartyCode As Code,S.SectorName,P.PartyName As [Hotel Name] from partymast P Inner Join sectormaster S On S.SectorCode=P.SectorCode where P.SpTypeCode='HOT' order by S.SectorName")

            End If
            If Dt.Tables.Count > 0 Then
                gv_SearchResult.DataSource = Dt.Tables(0)
                gv_SearchResult.DataBind()
                btnselect.Visible = True
                btnunselect.Visible = True
               
            Else
                gv_SearchResult.DataSource = Nothing
                gv_SearchResult.DataBind()
                btnselect.Visible = False
                btnunselect.Visible = False
                ddlNewServerType.SelectedIndex = 0
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Records Found');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)

        End Try
    End Sub

    Protected Sub ddlServerType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlServerType.SelectedIndexChanged
        Try
            Dim Dt As DataSet = New DataSet
            Dim Dt1 As DataSet = New DataSet
            If ddlServerType.SelectedIndex = 1 Then
                ddlNewServerType.SelectedIndex = 1
                ddlNewServerType0.SelectedIndex = 2
                Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select airportbordercode As Code,airportbordername As [Name] from airportbordersmaster order by airportbordername")
                ' Dt1 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select S.SectorCode As Code,S.SectorName,P.PartyName As [Hotel Name] from partymast P Inner Join sectormaster S On S.SectorCode=P.SectorCode where P.SpTypeCode='HOT' order by S.SectorName")
                Dt1 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select S.SectorCode As Code,S.SectorName from sectormaster S order by S.SectorName ")

            ElseIf ddlServerType.SelectedIndex = 2 Then
                ddlNewServerType.SelectedIndex = 2
                ddlNewServerType0.SelectedIndex = 1
                Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select S.SectorCode As Code,S.SectorName from sectormaster S order by S.SectorName ")

                ' Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select S.SectorCode As Code,S.SectorName,P.PartyName As [Hotel Name] from partymast P Inner Join sectormaster S On S.SectorCode=P.SectorCode where P.SpTypeCode='HOT' order by S.SectorName")
                Dt1 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select airportbordercode As Code,airportbordername As [Name] from airportbordersmaster order by airportbordername")

            ElseIf ddlServerType.SelectedIndex = 3 Then
                ddlNewServerType.SelectedIndex = 2
                ddlNewServerType0.SelectedIndex = 2
                Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select S.SectorCode As Code,S.SectorName from sectormaster S order by S.SectorName ")
                Dt1 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select S.SectorCode As Code,S.SectorName from sectormaster S order by S.SectorName ")


                'Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select S.SectorCode As Code,S.SectorName,P.PartyName As [Hotel Name] from partymast P Inner Join sectormaster S On S.SectorCode=P.SectorCode where P.SpTypeCode='HOT' order by S.SectorName")
                'Dt1 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select S.SectorCode As Code,S.SectorName,P.PartyName As [Hotel Name] from partymast P Inner Join sectormaster S On S.SectorCode=P.SectorCode where P.SpTypeCode='HOT' order by S.SectorName")

            ElseIf ddlServerType.SelectedIndex = 4 Then
                'ddlNewServerType.SelectedIndex = 0
                'ddlNewServerType0.SelectedIndex = 0
                ddlNewServerType.SelectedIndex = 1
                ddlNewServerType0.SelectedIndex = 1
                Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select airportbordercode As Code,airportbordername As [Name] from airportbordersmaster order by airportbordername")
                Dt1 = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select airportbordercode As Code,airportbordername As [Name] from airportbordersmaster order by airportbordername")

            End If
            Try
                If Dt.Tables.Count > 0 Then
                    gv_SearchResult.DataSource = Dt.Tables(0)
                    gv_SearchResult.DataBind()
                    btnselect.Visible = True
                    btnunselect.Visible = True
                Else
                    gv_SearchResult.DataSource = Nothing
                    gv_SearchResult.DataBind()
                    btnselect.Visible = False
                    btnunselect.Visible = False
                    ddlServerType.SelectedIndex = 0
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Records Found');", True)
                End If
                If Dt1.Tables.Count > 0 Then
                    GrdPicDrop.DataSource = Dt1.Tables(0)
                    GrdPicDrop.DataBind()
                    btnSelectAll.Visible = True
                    btnUnselectAll.Visible = True
                Else
                    GrdPicDrop.DataSource = Nothing
                    GrdPicDrop.DataBind()
                    btnSelectAll.Visible = False
                    btnUnselectAll.Visible = False
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Records Found');", True)
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)
            End Try


        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)
        End Try
    End Sub

    Protected Sub ddlNewServerType0_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlNewServerType0.SelectedIndexChanged
        Try
            Try
                Dim Dt As DataSet = New DataSet
                If ddlNewServerType0.SelectedIndex = 1 Then
                    Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select airportbordercode As Code,airportbordername As [Name] from airportbordersmaster order by airportbordername")
                ElseIf ddlNewServerType0.SelectedIndex = 2 Then
                    Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select S.SectorCode As Code,S.SectorName from sectormaster S order by S.SectorName ")
                ElseIf ddlNewServerType0.SelectedIndex = 3 Then
                    Dt = objUtils.ExecuteQuerySqlnew(Session("dbconnectionName"), "select P.PartyCode As Code,S.SectorName,P.PartyName As [Hotel Name] from partymast P Inner Join sectormaster S On S.SectorCode=P.SectorCode where P.SpTypeCode='HOT' order by S.SectorName")
                End If
                If Dt.Tables.Count > 0 Then
                    GrdPicDrop.DataSource = Dt.Tables(0)
                    GrdPicDrop.DataBind()
                    btnSelectAll.Visible = True
                    btnUnselectAll.Visible = True
                Else
                    GrdPicDrop.DataSource = Nothing
                    GrdPicDrop.DataBind()
                    btnSelectAll.Visible = False
                    btnUnselectAll.Visible = False
                    ddlNewServerType0.SelectedIndex = 0
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Records Found');", True)
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)

            End Try
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)

        End Try
    End Sub

    Protected Sub btnselect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnselect.Click
        Try
            Dim chksel As New CheckBox
            For Each GvRow In gv_SearchResult.Rows
                chksel = GvRow.FindControl("chkSelect")
                chksel.Checked = True
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)
        End Try
    End Sub

    Protected Sub btnSelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectAll.Click
        Try
            Dim chksel As New CheckBox
            For Each GvRow In GrdPicDrop.Rows
                chksel = GvRow.FindControl("chkSelect")
                chksel.Checked = True
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)
        End Try
    End Sub

    Protected Sub btnunselect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnunselect.Click
        Try
            Dim chksel As New CheckBox
            For Each GvRow In gv_SearchResult.Rows
                chksel = GvRow.FindControl("chkSelect")
                chksel.Checked = False
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)
        End Try
    End Sub

    Protected Sub btnUnselectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnselectAll.Click
        Try
            Dim chksel As New CheckBox
            For Each GvRow In GrdPicDrop.Rows
                chksel = GvRow.FindControl("chkSelect")
                chksel.Checked = False
            Next
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & ex.Message.ToString() & "');", True)
        End Try
    End Sub

   
    
    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(1).Visible = False
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(1).Visible = False
                If ViewState("OthtypeState") = "View" Or ViewState("OthtypeState") = "Delete" Then
                    e.Row.Cells(0).Enabled = False
                End If
            End If
          
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GrdPicDrop_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GrdPicDrop.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(1).Visible = False
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(1).Visible = False
                If ViewState("OthtypeState") = "View" Or ViewState("OthtypeState") = "Delete" Then
                    e.Row.Cells(0).Enabled = False
                End If
            End If
         
        Catch ex As Exception

        End Try
    End Sub
End Class
