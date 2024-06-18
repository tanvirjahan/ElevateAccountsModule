'------------================--------------=======================------------------================
'   Module Name    :    Cities.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    11 June 2008
'   
'
'------------================--------------=======================------------------================
#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
#End Region

Partial Class Cities
    Inherits System.Web.UI.Page


#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Dim objUser As New clsUser
#End Region
#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim lngCount As Int16
        Dim strTempUserFunctionalRight As String()
        Dim strRights As String
        Dim count As Integer


        ViewState.Add("AppId", Request.QueryString("AppId"))
        Dim strappid As String = ViewState("AppId")


        Dim groupid As String = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
        Dim menuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\CountriesSearch.aspx?appid=" + strappid, strappid)
        Dim functionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, menuid)

        If functionalrights <> "" Then
            strTempUserFunctionalRight = functionalrights.Split(";")
            For lngCount = 0 To strTempUserFunctionalRight.Length - 1
                strRights = strTempUserFunctionalRight.GetValue(lngCount)

                If strRights = "01" Then
                    count = 1
                End If
            Next
            If count = 1 Then
                btnCountry.Visible = True
            Else
                btnCountry.Visible = False
            End If
        End If
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                ViewState.Add("CitiesState", Request.QueryString("State"))
                ViewState.Add("CitiesRefCode", Request.QueryString("RefCode"))
                ViewState.Add("CitiesValue", Request.QueryString("Value"))
                'txtcon.Disabled = True
                ''objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlcountry, "ctryname", "ctrycode", "select * from ctrymast where active=1 order by ctryname", True)
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlcountry, "ctrycode", "select ctrycode from ctrymast where active=1 order by ctrycode", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1 order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlcname, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1 order by ctryname", True)

                '   txtAutoComplete.Attributes.Add("onkeyup", "SetContextKey()")
                If ViewState("CitiesState") = "New" Then
                    SetFocus(txtCode)
                    lblHeading.Text = "Add New Cities"
                    Page.Title = Page.Title + " " + "New City Master"
                    btnSave.Text = "Save"
                    txtorder.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select isnull (max(rankorder),0) from citymast") + 1



                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save city?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("CitiesState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Cities"
                    Page.Title = Page.Title + " " + "Edit City Master"
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("CitiesRefCode"), String))
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update city?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("CitiesState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Cities"
                    Page.Title = Page.Title + " " + "View City Master"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("CitiesRefCode"), String))
                    ' btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("CitiesState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Cities"
                    Page.Title = Page.Title + " " + "Delete City Master"
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("CitiesRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete city?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                'btnSave.Attributes.Add("onclick", "return FormValidation()")

                '   ValidateOnlyNumber()
                'charcters(txtCode)
                'charcters(txtName)
                'Numbers(txtorder)

                'DropdownList Aplphabetical order----   Azia
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    ddlCCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlcname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")

                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("Cities.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CtryWindowPostBack") Then
            If Session("addcountry") = "New" Then
                If Session("CountryName") IsNot Nothing Then
                    If Session("CountryCode") IsNot Nothing Then
                        Dim countryname As String = Session("CountryName")
                        txtcountrycode.Text = Session("CountryCode")
                        txtcountryname.Text = countryname
                        Session.Remove("addregion")
                        Session.Remove("CountryName")
                        Session.Remove("CountryCode")
                    End If
                End If
            End If
        End If
        Page.Title = "Cities Entry"
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
        If ViewState("CitiesState") = "View" Or ViewState("CitiesState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            'ddlcountry.Enabled = False
            chk_showinweb.Disabled = True
            chkActive.Disabled = True
            'txtCountryName.Disabled = True
            txtorder.Disabled = True
            ddlCCode.Disabled = True
            ddlcname.Disabled = True
            btnCountry.Enabled = False
        ElseIf ViewState("CitiesState") = "Edit" Then
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
            If ddlCCode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select country code.');", True)
                SetFocus(ddlCCode)
                ValidatePage = False
                Exit Function
            End If
            If ddlcname.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('County name can not be blank.');", True)
                SetFocus(ddlcname)
                ValidatePage = False
                Exit Function
            End If

            'If ddlcountry.SelectedValue = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select country code.');", True)
            '    SetFocus(ddlcountry)
            '    ValidatePage = False
            '    Exit Function
            'End If
            'If txtCountryName.Value = "" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('County name can not be blank.');", True)
            '    SetFocus(txtCountryName)
            '    ValidatePage = False
            '    Exit Function
            'End If 
            If txtorder.Value <> "" Then
                If txtorder.Value <= 0 Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Order can not be less than zero or equal to Zero');", True)
                    SetFocus(txtorder)
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

        Dim result1 As Integer = 0
        Dim frmmode As String = 0
        Dim strPassQry As String = "false"
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")
        Try

            If Page.IsValid = True Then
                If ViewState("CitiesState") = "New" Or ViewState("CitiesState") = "Edit" Then
                    'If ValidatePage() = False Then
                    '    Exit Sub
                    'End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("CitiesState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_city", mySqlConn, sqlTrans)
                        frmmode = 1

                    ElseIf ViewState("CitiesState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_city", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@cityname", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType(ddlCCode.Items(ddlCCode.SelectedIndex).Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType(txtcountrycode.Text.Trim, String)
                    If txtorder.Value = "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@rnkord", SqlDbType.Int, 4)).Value = DBNull.Value
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@rnkord", SqlDbType.Int, 4)).Value = CType(txtorder.Value.Trim, Integer)
                    End If

                    If chk_showinweb.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@showweb", SqlDbType.Int)).Value = 1
                    ElseIf chk_showinweb.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@showweb", SqlDbType.Int)).Value = 0
                    End If

                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@ServiceChargePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtServiceCharges.Value)
                    mySqlCmd.Parameters.Add(New SqlParameter("@MunicipalityFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(TxtMunicipalityFees.Value)
                    mySqlCmd.Parameters.Add(New SqlParameter("@TourismFeePerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtTourismFees.Value)
                    mySqlCmd.Parameters.Add(New SqlParameter("@VATPerc", SqlDbType.Decimal, 10, 3)).Value = Val(txtVAT.Value)

                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("CitiesState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_del_city", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                strPassQry = ""


            

                'result1 = strPassQry






                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("CitiesSearch.aspx", False)

                If ViewState("CitiesValue") = "Addfrom" Then
                    Session.Add("CitiesCode", txtCode.Value)
                    Session.Add("CitiesName", txtName.Value)
                    Dim strscript1 As String = ""
                    strscript1 = "window.opener.__doPostBack('SupcatsfromWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript1, True)
                End If
                If ViewState("CitiesValue") = "AddCityfrom" Then
                    Session.Add("CitiesCode", txtCode.Value)
                    Session.Add("CitiesName", txtName.Value)
                    Dim strscript1 As String = ""
                    strscript1 = "window.opener.__doPostBack('CityWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript1, True)
                End If
                If ViewState("CitiesState") = "New" Or ViewState("CitiesState") = "View" Or ViewState("CitiesState") = "Edit" Or ViewState("CitiesState") = "Delete" Then
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('CityWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()






            End If

            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

            objUtils.WritErrorLog("Cities.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from citymast Where citycode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("citycode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("citycode"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("cityname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("cityname"), String)
                    Else
                        Me.txtName.Value = ""
                    End If
                    'ctrycode()
                    If IsDBNull(mySqlReader("ctrycode")) = False Then
                        Me.txtcountrycode.Text = CType(mySqlReader("ctrycode"), String)
                        Me.txtcountryname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))
                        'Me.d
                        'Me.txtCountryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))
                    End If







                    'If IsDBNull(mySqlReader("ctrycode")) = False Then
                    '    Me.ddlCCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))                        'Me.ddlcountry.SelectedValue = CType(mySqlReader("ctrycode"), String)
                    '    Me.ddlcname.Value = CType(mySqlReader("ctrycode"), String)

                    '    'Me.txtCountryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))
                    'End If
                    If IsDBNull(mySqlReader("rankorder")) = False Then
                        Me.txtorder.Value = CType(mySqlReader("rankorder"), String)
                    Else
                        Me.txtorder.Value = ""
                    End If

                    If IsDBNull(mySqlReader("showweb")) = False Then
                        If CType(mySqlReader("showweb"), String) = "1" Then
                            chk_showinweb.Checked = True
                        ElseIf CType(mySqlReader("showweb"), String) = "0" Then
                            chk_showinweb.Checked = False
                        End If
                    End If
                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If


                    Me.txtServiceCharges.Value = 0
                    Me.TxtMunicipalityFees.Value = 0
                    Me.txtTourismFees.Value = 0
                    Me.txtVAT.Value = 0
                    If IsDBNull(mySqlReader("ServiceChargePerc")) = False Then
                        Me.txtServiceCharges.Value = Val(CType(mySqlReader("ServiceChargePerc"), String))
                    End If
                    If IsDBNull(mySqlReader("MunicipalityFeePerc")) = False Then
                        Me.TxtMunicipalityFees.Value = Val(CType(mySqlReader("MunicipalityFeePerc"), String))
                    End If
                    If IsDBNull(mySqlReader("TourismFeePerc")) = False Then
                        Me.txtTourismFees.Value = Val(CType(mySqlReader("TourismFeePerc"), String))
                    End If
                    If IsDBNull(mySqlReader("VATPerc")) = False Then
                        Me.txtVAT.Value = Val(CType(mySqlReader("VATPerc"), String))
                    End If


                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "PopupScript", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Cities.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("CitiesSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

    '#Region "Protected Sub ddlcountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    '    Protected Sub ddlcountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlcountry.SelectedIndexChanged
    '        Try
    '            strSqlQry = ""
    '            txtCountryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", ddlcountry.SelectedValue)
    '        Catch ex As Exception

    '        End Try
    '    End Sub
    '#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If ViewState("CitiesState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "citymast", "citycode", CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This city code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            'If objUtils.isDuplicatenew(Session("dbconnectionName"), "citymast", "cityname", txtName.Value.Trim) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This city name is already present.');", True)
            '    checkForDuplicate = False
            '    Exit Function
            'End If
            Dim strQryValue As String
            Dim strSelectQry As String = "SELECT citycode FROM citymast(nolock) WHERE  cityname='" & txtName.Value.Trim & "' and ctrycode='" & txtcountrycode.Text & "'"
            strQryValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSelectQry) 'Use Aggregate
            If strQryValue <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This city name is already present.');", True)
                checkForDuplicate = False
                Exit Function

            End If
        ElseIf ViewState("CitiesState") = "Edit" Then

            Dim strQryValue As String
            Dim strSelectQry As String = "SELECT citycode FROM citymast(nolock) WHERE citycode<> '" & txtCode.Value.Trim & "' and cityname='" & txtName.Value.Trim & "' and ctrycode='" & txtcountrycode.Text & "'"
            strQryValue = objUtils.ExecuteQueryReturnSingleValuenew(Session("dbconnectionName"), strSelectQry) 'Use Aggregate
            If strQryValue <> "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This city name is already present.');", True)
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
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "agentmast", "citycode", CType(txtCode.Value.Trim, String)) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This city is already used for a Customers, cannot delete this city');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymast", "citycode", CType(txtCode.Value.Trim, String)) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This city is already used for a Suppliers, cannot delete this city');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sectormaster", "citycode", CType(txtCode.Value.Trim, String)) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This city is already used for a SupplierSectors, cannot delete this city');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "supplier_agents", "citycode", CType(txtCode.Value.Trim, String)) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This city is already used for a SupplierAgents, cannot delete this city');", True)
            checkForDeletion = False
            Exit Function

        End If
        checkForDeletion = True
    End Function
#End Region

    'Protected Sub ddlCCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCCode.SelectedIndexChanged
    '    Try
    '        'strSqlQry = ""
    '        ' txtcon.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", ddlCCode.SelectedValue)
    '        txtcon.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", ddlCCode.SelectedValue.Trim.ToString)
    '        'ddlCCode
    '        ' ddlCCode.Focus()
    '        SetFocus(ddlCCode)

    '    Catch ex As Exception
    '    End Try
    'End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Cities','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
    <System.Web.Script.Services.ScriptMethod()> _
         <System.Web.Services.WebMethod()> _
    Public Shared Function Getcountrylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Countrynames As New List(Of String)
        Try
            strSqlQry = "select ctryname,ctrycode from ctrymast  where  ctryname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Countrynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("ctryname").ToString(), myDS.Tables(0).Rows(i)("ctrycode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Countrynames
        Catch ex As Exception
            Return Countrynames
        End Try
    End Function
    Protected Sub TxtCountryName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtcountryname.TextChanged
        Session("countries_ctrycode_for_filter") = txtcountrycode.Text()
    End Sub
    Protected Sub btnCountry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCountry.Click
        Dim strpop As String = ""
        strpop = "window.open('Countries.aspx?State=New&Value=Addfrom','country');"
        Session.Add("addcountry", "New")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub

End Class
