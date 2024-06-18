#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class PriceListModule_Vehicletype
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
        If Page.IsPostBack = False Then

            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

                txtconnection.Value = Session("dbconnectionName")

                'ViewState.Add("OthcatState", Request.QueryString("State"))
                'ViewState.Add("OthcatRefCode", Request.QueryString("RefCode"))
                ViewState.Add("VTState", Request.QueryString("State"))
                ViewState.Add("VTRefCode", Request.QueryString("RefCode"))


                ' objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlGroup, "othgrpcode", "select othgrpcode from othgrpmast where active=1 order by othgrpcode", True)


                'If FindPage = "Yes" Then
                '    'strSqlQry = strSqlQry & " WHERE dbo.othcatmast.othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') And  " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othgrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpName, "othgrpname", "othgrpcode", "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode=(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othgrpcode", True)
                If ViewState("VTState") = "New" Then
                    ddlOtherGrpCode.SelectedIndex = 0
                    'ddlOtherGrpName.SelectedIndex = 0
                End If
                'Else
                ''strSqlQry = strSqlQry & " WHERE dbo.othcatmast.othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') And  " & BuildCondition() & " ORDER BY " & strorderby & " " & strsortorder
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpCode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othgrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlOtherGrpName, "othgrpname", "othgrpcode", "select othgrpcode,othgrpname from othgrpmast where active=1 And othgrpcode<>(Select Option_Selected From Reservation_ParaMeters Where Param_Id='1001') order by othgrpcode", True)

                'End If

                Numbers(txtOrder)
                Numbers(txtMinPax)
                Numbers(txtMaxPax)
                'charcters1(txtRemark)
                charcters(txtCapacity)
                charcters(txtOptions)


                If ViewState("VTState") = "New" Then
                    SetFocus(txtCode)
                    lblHeading.Text = "Add New Vehicle Type"
                    Page.Title = Page.Title + " " + "New Vehicle Type Master"
                    btnSave.Text = "Save"
                    Dim strQry As String = "select isnull (max(grporder),0) from othcatmast where dbo.othcatmast.othgrpcode=(Select Option_Selected From Reservation_ParaMeters " & _
                        "   Where Param_Id='1001') "

                    txtOrder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry) + 1

                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Vehicle Type?')==false)return false;")

                'btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("VTState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Vehicle Type"
                    Page.Title = Page.Title + " " + "Edit Vehicle Type Master"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("VTRefCode"), String))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update Vehicle Type?')==false)return false;")
                '                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("VTState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Vehicle Type"
                    Page.Title = Page.Title + " " + "View Vehicle Type Master"
                    btnSave.Visible = False
                'btnCancel.Text = "Return to Other Category"
                    DisableControl()
                    ShowRecord(CType(ViewState("VTRefCode"), String))

                ElseIf ViewState("VTState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Vehicle Type"
                    Page.Title = Page.Title + " " + "Delete Vehicle Type Master"
                    btnSave.Text = "Delete"
                    DisableControl()
                    ShowRecord(CType(ViewState("VTRefCode"), String))
                'btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Vehicle Type?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                '                btnSave.Attributes.Add("onclick", "return FormValidation()")

                '   ValidateOnlyNumber()
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlOtherGrpCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlOtherGrpName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
                'charcters(txtCode)
                'charcters(txtName)
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("VehicleTypeSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        If ViewState("VTState") = "View" Or ViewState("VTState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            ddlOtherGrpCode.Disabled = True
            txtRemark.Enabled = False
            'ddlCalcPax.Enabled = False
            'ddladchild.Enabled = False
            txtOrder.Disabled = True
            txtMinPax.Disabled = True
            ChkActive.Disabled = True
            txtCapacity.Disabled = True
            txtOptions.Disabled = True
            ChkPakReq.Disabled = True
            'ChkPrnRemark.Disabled = True
            txtMaxPax.Disabled = True
            chkshuttle.Disabled = True
        ElseIf ViewState("VTState") = "Edit" Then
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
           
            'If ddlOtherGrpCode.Value = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select group.');", True)
            '    SetFocus(ddlOtherGrpCode)
            '    ValidatePage = False
            '    Exit Function
            'End If
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

            'If txtCapacity.Value.Trim = "" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Capacity field can not be blank.');", True)
            '    SetFocus(txtCapacity)
            '    ValidatePage = False
            '    Exit Function
            'End If


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
            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Page.IsValid = True Then
                If ViewState("VTState") = "New" Or ViewState("VTState") = "Edit" Then
                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("VTState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_othcat", mySqlConn, sqlTrans)
                    ElseIf ViewState("VTState") = "Edit" Then
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
                    mySqlCmd.Parameters.Add(New SqlParameter("@unitname", SqlDbType.VarChar, 10)).Value = " "
                    mySqlCmd.Parameters.Add(New SqlParameter("@remarks", SqlDbType.Text)).Value = CType(txtRemark.Text.Trim, String)
                    If ChkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf ChkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If

                    ' Print Remark not present in ths screen , but saved as '1' in othr cat table (same table for 'Othrcat' & 'Vehicle Type')
                    'If ChkPrnRemark.Checked = True Then
                    mySqlCmd.Parameters.Add(New SqlParameter("@printremarks", SqlDbType.Int)).Value = 1
                    'ElseIf ChkPrnRemark.Checked = False Then
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@printremarks", SqlDbType.Int)).Value = 0
                    'End If
                    If ChkPakReq.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@paxcheckReqd", SqlDbType.Int)).Value = 1
                    ElseIf ChkPakReq.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@paxcheckReqd", SqlDbType.Int)).Value = 0
                    End If
                    ' 'Calc by pax' not present in ths screen , but saved as 'No' in othr cat table (same table for 'Othrcat' & 'Vehicle Type')
                    mySqlCmd.Parameters.Add(New SqlParameter("@calcyn", SqlDbType.VarChar, 10)).Value = "No"
                    mySqlCmd.Parameters.Add(New SqlParameter("@adultchild", SqlDbType.VarChar, 10)).Value = ""
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@dispname", SqlDbType.VarChar, 50)).Value = " "
                    mySqlCmd.Parameters.Add(New SqlParameter("@Capacity", SqlDbType.Text)).Value = CType(txtCapacity.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@Options", SqlDbType.Text)).Value = CType(txtOptions.Value.Trim, String)
                    If chkshuttle.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@shuttle", SqlDbType.Int)).Value = 1
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@shuttle", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("VTState") = "Delete" Then
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

            objUtils.WritErrorLog("VehicleType.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
                        'ddlOtherGrpName.Value = CType(mySqlReader("othgrpcode"), String)
                    Else
                        ddlOtherGrpCode.Value = "[Select]"
                        'ddlOtherGrpName.Value = "[Select]"
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
                    If IsDBNull(mySqlReader("capacity")) = False Then
                        Me.txtCapacity.Value = mySqlReader("capacity")
                    Else
                        Me.txtCapacity.Value = ""
                    End If
                    If IsDBNull(mySqlReader("options")) = False Then
                        Me.txtOptions.Value = mySqlReader("options")
                    Else
                        Me.txtOptions.Value = ""
                    End If
                    'If IsDBNull(mySqlReader("calcyn")) = False Then
                    '    Me.ddlCalcPax.SelectedValue = mySqlReader("calcyn")
                    'End If

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

                    If IsDBNull(mySqlReader("shuttle")) = False Then
                        If mySqlReader("shuttle") = 1 Then
                            chkshuttle.Checked = True
                        Else
                            chkshuttle.Checked = False
                        End If

                    End If

                    'If IsDBNull(mySqlReader("printRemarks")) = False Then
                    '    If CType(mySqlReader("printRemarks"), String) = "1" Then
                    '        ChkPrnRemark.Checked = True
                    '    ElseIf CType(mySqlReader("printRemarks"), String) = "0" Then
                    '        ChkPrnRemark.Checked = False
                    '    End If
                    'End If



                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("VehicleType.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        If ViewState("VTState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "othcatmast", "othcatcode", CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Vehicle Type code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "othcatmast", "othcatname", txtName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Vehicle Type name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("VTState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "othcatmast", "othcatcode", "othcatname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Vehicle Type name is already present.');", True)
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

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), " partyothcat", "othcatcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This ServiceType is already used for a Supplier Other category, cannot delete this ServiceTypes');", True)
            checkForDeletion = False
            Exit Function
        End If
        checkForDeletion = True

    End Function
#End Region

   

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=VehicleType','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

End Class
