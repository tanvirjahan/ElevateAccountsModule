'------------================--------------=======================------------------================
'   Module Name    :    CountriesSearch.aspx
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

Partial Class Countries
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
        Dim CurrCount As Integer
        Dim RegCount As Integer
        Dim lngCount As Int16
        Dim CurrlngCount As Int16
        Dim strappname As String = ""
        Dim strTempUserFunctionalRight As String()
        Dim strTempRegUserFunctionalRight As String()
        Dim strRegRights As String
        Dim strCurrRights As String

        ViewState.Add("AppId", Request.QueryString("AppId"))
        Dim strappid As String = ViewState("AppId")

        Dim groupid As String = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))

        Dim menuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\CurrenciesSearch.aspx?appid=" + strappid, strappid)
        Dim functionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, menuid)

        Dim Regmenuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\MarketsSearch.aspx?appid=" + strappid, strappid)
        Dim Regfunctionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, Regmenuid)
        If functionalrights <> "" Then
            strTempUserFunctionalRight = functionalrights.Split(";")
            For CurrlngCount = 0 To strTempUserFunctionalRight.Length - 1
                strCurrRights = strTempUserFunctionalRight.GetValue(CurrlngCount)

                If strCurrRights = "01" Then
                    CurrCount = 1
                End If
            Next
            If CurrCount = 1 Then
                btnCurrency.Visible = True
            Else
                btnCurrency.Visible = False
            End If
        End If

        If Regfunctionalrights <> "" Then
            strTempRegUserFunctionalRight = Regfunctionalrights.Split(";")

            For lngCount = 0 To strTempRegUserFunctionalRight.Length - 1
                strRegRights = strTempRegUserFunctionalRight.GetValue(lngCount)

                If strRegRights = "01" Then
                    RegCount = 1
                End If
            Next
            If RegCount = 1 Then
                btnRegion.Visible = True
            Else
                btnRegion.Visible = False
            End If
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
                'FillDropDownList()
                FillDropDownListHTMLNEW()
                ViewState.Add("CountriesState", Request.QueryString("State"))
                ViewState.Add("CountriesRefCode", Request.QueryString("RefCode"))
                ViewState.Add("CountryValue", Request.QueryString("Value"))
                'FillDropDownListHTMLNEW(Session("dbconnectionName"),)
                If ViewState("CountriesState") = "New" Then
                    SetFocus(txtcountrycode)
                    lblHeading.Text = "Add New Country"
                    Page.Title = Page.Title + " " + "New Country Master"
                    btnSave.Text = "Save"
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save country?')==false)return false;")
                ElseIf ViewState("CountriesState") = "Edit" Then
                    SetFocus(txtcountrycode)
                    lblHeading.Text = "Edit Country"
                    Page.Title = Page.Title + " " + "Edit Country Master"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("CountriesRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update country?')==false)return false;")
                ElseIf ViewState("CountriesState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Country"
                    Page.Title = Page.Title + " " + "View Country Master"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("CountriesRefCode"), String))
                    'btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                ElseIf ViewState("CountriesState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Country"
                    Page.Title = Page.Title + " " + "Delete Country Master"
                    btnSave.Text = "Delete"
                    DisableControl()
                    ShowRecord(CType(ViewState("CountriesRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete country?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
                'charcters(txtcountrycode)
                'charcters(TxtCountryName)
                Dim typ As Type
                typ = GetType(DropDownList)
                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlCurCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCurName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlMktCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlMktName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlNationalityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlNationalityCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("Countries.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
        'If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CurrWindowPostBack") Then
        '    If Session("addregion") = "new" Then
        '        If Session("CurrencyName") IsNot Nothing Then
        '            If Session("CurrencyCode") IsNot Nothing Then
        '                Dim CurrencyName As String = Session("CurrencyName")
        '                Txtcurrencycode.Text = Session("CurrencyCode")
        '                TxtCurrencyName.Text = CurrencyName
        '                Session.Remove("addcurrency")
        '                Session.Remove("CurrencyName")
        '                Session.Remove("CurrencyCode")
        '            End If
        '        End If
        '    End If

        'End If
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "MarketWindowPostBack") Then
            If Session("addregion") = "New" Then
                If Session("RegionName") IsNot Nothing Then
                    If Session("RegionCode") IsNot Nothing Then
                        Dim RegionName As String = Session("RegionName")
                        TxtRegionCode.Text = Session("RegionCode")
                        TxtRegionName.Text = RegionName
                        Session.Remove("addregion")
                        Session.Remove("RegionName")
                        Session.Remove("RegionCode")
                    End If
                End If
            End If
        End If
        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "SupcatsfromWindowPostBack") Then
            If Session("addcurrency") = "New" Then
                If Session("CurrName") IsNot Nothing Then
                    If Session("CurrCode") IsNot Nothing Then
                        Dim Currencyname As String = Session("CurrName")
                        Txtcurrencycode.Text = Session("CurrCode")
                        TxtCurrencyName.Text = Currencyname
                        Session.Remove("addcurrency")
                        Session.Remove("CurrName")
                        Session.Remove("CurrCode")
                    End If
                End If
            End If
        End If



        Page.Title = "Country Entry"
    End Sub
#End Region

#Region "charcters"
    Public Sub charcters(ByVal txtbox As HtmlInputText)
        txtbox.Attributes.Add("onkeypress", "return checkCharacter(event)")
    End Sub
#End Region

#Region "Private Sub FillDropDownListHTMLNEW()"
    Private Sub FillDropDownListHTMLNEW()
        'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlcurrency, "currcode", "select currcode from currmast  where active =1 order by currcode", True)
        'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlMarket, "plgrpcode", "select plgrpcode from plgrpmast where active =1 order by plgrpcode", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurCode, "currcode", "currname", "select currcode,currname from currmast  where active =1 order by currcode", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCurName, "currname", "currcode", "select currname,currcode from currmast  where active =1 order by currcode", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMktCode, "plgrpcode", "plgrpname", "select plgrpcode,plgrpname from plgrpmast where active =1 order by plgrpcode", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMktName, "plgrpname", "plgrpcode", "select plgrpname,plgrpcode from plgrpmast where active =1 order by plgrpcode", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlNationalityCode, "nationalitycode", "nationalityname", "select nationalitycode,nationalityname from nationality_master where active =1 order by nationalitycode", True)
        objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlNationalityName, "nationalityname", "nationalitycode", "select nationalityname,nationalitycode from nationality_master where active =1 order by nationalityname", True)

    End Sub
#End Region

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("CountriesState") = "View" Or ViewState("CountriesState") = "Delete" Then
            txtcountrycode.Disabled = True
            'ddlCurrency.Enabled = False
            'ddlMarket.Enabled = False
            ddlCurCode.Disabled = True
            ddlMktCode.Disabled = True
            ddlNationalityName.Disabled = True
            ddlNationalityCode.Disabled = True
            TxtCountryName.Disabled = True
            ddlCurName.Disabled = True
            ddlMktName.Disabled = True
            ddlWO1from.Enabled = False
            ddlWO1to.Enabled = False
            ddlWO2from.Enabled = False
            ddlWO2to.Enabled = False
            chkActive.Disabled = True
            chkpromotion.Disabled = True
            chkbirdpromotion.Disabled = True
            btnCurrency.Enabled = False
            btnRegion.Enabled = False
            TxtCurrencyName.Enabled = False
            TxtRegionName.Enabled = False
            btnCurrency.Enabled = False
            btnRegion.Enabled = False
        ElseIf ViewState("CountriesState") = "Edit" Then
            txtcountrycode.Disabled = True
            chkpromotion.Disabled = True
            ' txtcodename.Disabled = True
        End If

    End Sub

#End Region


#Region "Private Function ValidatePage() As Boolean"
    Private Function ValidatePage() As Boolean

        Try
            If txtcountrycode.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Code field can not be blank.');", True)
                SetFocus(txtcountrycode)

                ValidatePage = False
                Exit Function
            End If
            If TxtCountryName.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name field can not be blank.');", True)
                SetFocus(TxtCountryName)
                ValidatePage = False
                Exit Function
            End If
            If ddlCurCode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select currency code.');", True)
                SetFocus(ddlCurCode)
                ValidatePage = False
                Exit Function
            End If
            If ddlCurName.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert(Currency name field can not be blank.');", True)
                SetFocus(ddlCurName)
                ValidatePage = False
                Exit Function
            End If
            If ddlMktCode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select region code.');", True)
                SetFocus(ddlMktCode)
                ValidatePage = False
                Exit Function
            End If
            If ddlMktName.Value.Trim = "" Then
                objUtils.MessageBox("Please enter region name.", Me.Page)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Region name field can not be blank.');", True)
                SetFocus(ddlMktName)
                ValidatePage = False
                Exit Function
            End If
            'If ddlNationalityName.Value = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Nationality field can not be blank.');", True)
            '    SetFocus(ddlNationalityName)
            '    ValidatePage = False
            '    Exit Function
            'End If
            If ddlWO1from.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select week end from 1.');", True)
                SetFocus(ddlWO1from)
                ValidatePage = False
                Exit Function
            End If
            If ddlWO1to.SelectedValue = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select week end to 1.');", True)
                SetFocus(ddlWO1to)
                ValidatePage = False
                Exit Function
            End If
            'If ddlWO2from.SelectedValue = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select week end from 2.');", True)
            '    SetFocus(ddlWO2from)
            '    ValidatePage = False
            '    Exit Function
            'End If
            'If ddlWO2to.SelectedValue = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select week end to 2.');", True)
            '    SetFocus(ddlWO2to)
            '    ValidatePage = False
            '    Exit Function
            'End If
            If ddlWO1from.SelectedValue <> "[Select]" And ddlWO1to.SelectedValue <> "[Select]" Then

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
                If ViewState("CountriesState") = "New" Or ViewState("CountriesState") = "Edit" Then
                    'If ValidatePage() = False Then
                    '    Exit Sub
                    'End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("CountriesState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_ctry", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf ViewState("CountriesState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_ctry", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType(txtcountrycode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ctryname", SqlDbType.VarChar, 100)).Value = CType(TxtCountryName.Value.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(ddlcurrency.SelectedValue, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@currcode", SqlDbType.VarChar, 20)).Value = CType(Txtcurrencycode.Text.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@wkfrmday1", SqlDbType.VarChar, 15)).Value = CType(ddlWO1from.SelectedValue, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@wktoday1", SqlDbType.VarChar, 15)).Value = CType(ddlWO1to.SelectedValue, String)
                    If ddlWO2from.SelectedValue = "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@wkfrmday2", SqlDbType.VarChar, 15)).Value = System.DBNull.Value

                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@wkfrmday2", SqlDbType.VarChar, 15)).Value = CType(ddlWO2from.SelectedValue, String)
                    End If

                    If ddlWO2to.SelectedValue = "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@wktoday2", SqlDbType.VarChar, 15)).Value = System.DBNull.Value

                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@wktoday2", SqlDbType.VarChar, 15)).Value = CType(ddlWO2to.SelectedValue, String)
                    End If

                    'mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlMarket.SelectedValue, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@plgrpcode", SqlDbType.VarChar, 20)).Value = CType(TxtRegionCode.Text.Trim, String)
                    'If ddlNationalityCode.Value <> "[Select]" Then
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@nationalitycode", SqlDbType.VarChar, 20)).Value = CType(ddlNationalityName.Value, String)
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@nationalityname", SqlDbType.VarChar, 100)).Value = CType(ddlNationalityCode.Value, String)
                    'Else
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@nationalitycode", SqlDbType.VarChar, 20)).Value = System.DBNull.Value
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@nationalityname", SqlDbType.VarChar, 100)).Value = System.DBNull.Value
                    'End If



                    mySqlCmd.Parameters.Add(New SqlParameter("@nationalitycode", SqlDbType.VarChar, 20)).Value = System.DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@nationalityname", SqlDbType.VarChar, 100)).Value = System.DBNull.Value













                    If chkpromotion.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@inclpromotion", SqlDbType.Int, 18)).Value = 1
                    ElseIf chkpromotion.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@inclpromotion", SqlDbType.Int, 18)).Value = 0
                    End If
                    If chkbirdpromotion.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@inclearlypromotion", SqlDbType.Int, 18)).Value = 1
                    ElseIf chkbirdpromotion.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@inclearlypromotion", SqlDbType.Int, 18)).Value = 0
                    End If
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@visaremarks", SqlDbType.VarChar, 500)).Value = CType(txtvisaremarks.Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()


                   


                ElseIf ViewState("CountriesState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_del_ctry", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType(txtcountrycode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If
                strPassQry = ""






                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("CountriesSearch.aspx", False)

                If ViewState("CountryValue") = "Addfrom" Then
                    Session.Add("CountryCode", txtcountrycode.Value)
                    Session.Add("CountryName", TxtCountryName.Value)
                    Dim strscript1 As String = ""
                    strscript1 = "window.opener.__doPostBack('CtryWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript1, True)
                End If

                If ViewState("CountriesState") = "New" Or ViewState("CountriesState") = "View" Or ViewState("CountriesState") = "Edit" Or ViewState("CountriesState") = "Delete" Then
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('CtryWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If
            End If






        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()

            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Countries.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from ctrymast Where ctrycode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("ctrycode")) = False Then
                        Me.txtcountrycode.Value = CType(mySqlReader("ctrycode"), String)
                    Else
                        Me.txtcountrycode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("ctryname")) = False Then
                        Me.TxtCountryName.Value = CType(mySqlReader("ctryname"), String)
                    Else
                        Me.TxtCountryName.Value = ""
                    End If


                    If IsDBNull(mySqlReader("visaremarks")) = False Then
                        Me.txtvisaremarks.Text = CType(mySqlReader("visaremarks"), String)
                    Else
                        Me.txtvisaremarks.Text = ""
                    End If

                    'If IsDBNull(mySqlReader("currcode")) = False Then
                    '    Me.ddlCurCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", CType(mySqlReader("currcode"), String))
                    '    Me.ddlCurName.Value = CType(mySqlReader("currcode"), String)
                    'Else
                    '    Me.ddlCurName.Value = ""
                    '    Me.ddlCurCode.Value = ""
                    'End If





                    If IsDBNull(mySqlReader("currcode")) = False Then
                        Me.TxtCurrencyName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "currmast", "currname", "currcode", CType(mySqlReader("currcode"), String))
                        Me.Txtcurrencycode.Text = CType(mySqlReader("currcode"), String)
                    Else
                        Me.TxtCurrencyName.Text = ""
                        Me.Txtcurrencycode.Text = ""
                    End If


                    If IsDBNull(mySqlReader("plgrpcode")) = False Then
                        Me.TxtRegionName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "plgrpmast", "plgrpname", "plgrpcode", CType(mySqlReader("plgrpcode"), String))
                        Me.TxtRegionCode.Text = CType(mySqlReader("plgrpcode"), String)

                    Else
                        Me.TxtRegionName.Text = ""
                        Me.TxtRegionCode.Text = ""
                    End If

                    'If IsDBNull(mySqlReader("nationalitycode")) = False Then
                    '    Me.ddlNationalityName.Value = CType(mySqlReader("nationalitycode"), String)
                    'Else
                    '    Me.ddlNationalityName.Value = "[Select]"
                    'End If







                    'If IsDBNull(mySqlReader("nationality")) = False Then
                    '    Me.ddlNationalityCode.Value = CType(mySqlReader("nationality"), String)
                    'Else
                    '    Me.ddlNationalityCode.Value = "[Select]"
                    'End If


                    If IsDBNull(mySqlReader("wkfrmday1")) = False Then
                        Me.ddlWO1from.SelectedValue = CType(mySqlReader("wkfrmday1"), String)
                    Else
                        Me.ddlWO1from.SelectedValue = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("wktoday1")) = False Then
                        Me.ddlWO1to.SelectedValue = CType(mySqlReader("wktoday1"), String)
                    Else
                        Me.ddlWO1to.SelectedValue = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("wkfrmday2")) = False Then
                        Me.ddlWO2from.SelectedValue = CType(mySqlReader("wkfrmday2"), String)
                    Else
                        Me.ddlWO2from.SelectedValue = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("wktoday2")) = False Then
                        Me.ddlWO2to.SelectedValue = CType(mySqlReader("wktoday2"), String)
                    Else
                        Me.ddlWO2to.SelectedValue = "[Select]"
                    End If
                    If IsDBNull(mySqlReader("inclpromotion")) = False Then
                        If CType(mySqlReader("inclpromotion"), String) = "1" Then
                            chkpromotion.Checked = True
                        ElseIf CType(mySqlReader("inclpromotion"), String) = "0" Then
                            chkpromotion.Checked = False
                        End If
                    End If
                    If IsDBNull(mySqlReader("inclearlypromotion")) = False Then
                        If CType(mySqlReader("inclearlypromotion"), String) = "1" Then
                            chkbirdpromotion.Checked = True
                        ElseIf CType(mySqlReader("inclearlypromotion"), String) = "0" Then
                            chkbirdpromotion.Checked = False
                        End If
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
            objUtils.WritErrorLog("Countries.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("CountriesSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean

        If ViewState("CountriesState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "ctrymast", "ctrycode", CType(txtcountrycode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This country code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "ctrymast", "ctryname", TxtCountryName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This country name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("CountriesState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "ctrymast", "ctrycode", "ctryname", TxtCountryName.Value.Trim, CType(txtcountrycode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This country name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region

    '#Region "Protected Sub ddlcurrency_SelectedIndexChanged(rByVal sender As Object, ByVal e As System.EventArgs)"
    '    Protected Sub ddlcurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '        Try
    '            strSqlQry = ""
    '            txtcurrencyname.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"currmast", "currname", "currcode", ddlcurrency.SelectedValue)
    '        Catch ex As Exception

    '        End Try
    '    End Sub
    '#End Region

    '#Region "Protected Sub ddlmarket_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    '    Protected Sub ddlmarket_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '        Try
    '            strSqlQry = ""
    '            txtmarketname.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"plgrpmast", "plgrpname", "plgrpcode", ddlMarket.SelectedValue)
    '        Catch ex As Exception

    '        End Try
    '    End Sub
    '#End Region
    

    Protected Sub TxtRegionName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtRegionName.TextChanged
        Session("countries_plgrpcode_for_filter") = TxtRegionCode.Text()
    End Sub


    Protected Sub TxtCurrencyName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtCurrencyName.TextChanged
        Session("countries_currcode_for_filter") = Txtcurrencycode.Text()
    End Sub



#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "agent_sectormaster", "ctrycode", CType(txtcountrycode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Country is already used for a Customer Sectors, cannot delete this Country');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "agentmast", "ctrycode", CType(txtcountrycode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Country is already used for a Customers, cannot delete this Country');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "compare_ratesh", "ctrycode", CType(txtcountrycode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Country is already used for a CompetetorRates, cannot delete this Country');", True)
            checkForDeletion = False
            Exit Function
        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "citymast", "ctrycode", CType(txtcountrycode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Country is already used for a Cities, cannot delete this Country');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymast", "ctrycode", CType(txtcountrycode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Country is already used for a Suppliers, cannot delete this Country');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "Promo_countries", "ctrycode", CType(txtcountrycode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Country is already used for a Promotions, cannot delete this Country');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sectormaster", "ctrycode", CType(txtcountrycode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Country is already used for a SupplierSectors, cannot delete this Country');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "supplier_agents", "ctrycode", CType(txtcountrycode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Country is already used for a SupplierAgents, cannot delete this Country');", True)
            checkForDeletion = False
            Exit Function

        End If

        checkForDeletion = True
    End Function
#End Region
    Protected Sub ddlCurCode_ServerChange(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub


    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function GetRegionlist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Regionames As New List(Of String)
        Try
            strSqlQry = "select plgrpname,plgrpcode from plgrpmast where plgrpname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Regionames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("plgrpname").ToString(), myDS.Tables(0).Rows(i)("plgrpcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Regionames
        Catch ex As Exception
            Return Regionames
        End Try
    End Function
    <System.Web.Script.Services.ScriptMethod()> _
      <System.Web.Services.WebMethod()> _
    Public Shared Function Getcurrencylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Currencynames As New List(Of String)
        Try
            strSqlQry = "select currname,currcode from currmast where  currname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Currencynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("currname").ToString(), myDS.Tables(0).Rows(i)("currcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If
            Return Currencynames
        Catch ex As Exception
            Return Currencynames
        End Try

    End Function
    Protected Sub cmdhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Countries','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    Protected Sub btnCurrency_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strpop As String = ""
        strpop = "window.open('Currencies.aspx?State=New&Value=Addfrom','currency');"
        Session.Add("addcurrency", "New")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub

    Protected Sub btnRegion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRegion.Click
        Dim strpop As String = ""
        strpop = "window.open('Markets.aspx?State=New&Value=Addfrom','region');"
        Session.Add("addregion", "New")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)

    End Sub



   
End Class


