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
Partial Class Sector
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
        ViewState.Add("appid", Request.QueryString("appid"))
        Dim strappid As String = ViewState("appid")




        Dim strctryTempUserFunctionalRight As String()
        Dim strcityTempUserFunctionalRight As String()
        Dim cityCount As Integer
        Dim ctryCount As Integer
        Dim citylngCount As Int16
        Dim CtrylngCount As Int16
        Dim strcityRights As String
        Dim strctryRights As String
        Dim groupid As String = objUser.GetGroupId(Session("dbconnectionName"), CType(Session("GlobalUserName"), String), CType(Session("Userpwd"), String))
        Dim ctrymenuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\CountriesSearch.aspx?appid=" + strappid, strappid)
        Dim ctryfunctionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, ctrymenuid)

        Dim citymenuid As Integer = objUser.GetMenuId(Session("dbconnectionName"), "PriceListModule\CitiesSearch.aspx?appid=" + strappid, strappid)
        Dim cityfunctionalrights As String = objUser.GetUserFunctionalRight(Session("dbconnectionName"), groupid, strappid, citymenuid)


        If ctryfunctionalrights <> "" Then
            strctryTempUserFunctionalRight = ctryfunctionalrights.Split(";")
            For CtrylngCount = 0 To strctryTempUserFunctionalRight.Length - 1
                strctryRights = strctryTempUserFunctionalRight.GetValue(CtrylngCount)

                If strctryRights = "01" Then
                    ctryCount = 1
                End If
            Next
            If ctryCount = 1 Then
                btnCountry.Visible = True
            Else
                btnCountry.Visible = False
            End If
        End If

        If ctryfunctionalrights <> "" Then
            strcityTempUserFunctionalRight = cityfunctionalrights.Split(";")
            For citylngCount = 0 To strcityTempUserFunctionalRight.Length - 1
                strcityRights = strcityTempUserFunctionalRight.GetValue(citylngCount)

                If strcityRights = "01" Then
                    cityCount = 1
                End If
            Next
            If cityCount = 1 Then
                btnCity.Visible = True
            Else
                btnCity.Visible = False
            End If
        End If


        Dim default_country As String
        Dim default_group As String
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
                ViewState.Add("SectorsState", Request.QueryString("State"))
                ViewState.Add("SectorsRefCode", Request.QueryString("RefCode"))
                ViewState.Add("SectorValue", Request.QueryString("Value"))
                default_group = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("1001", String))
                'objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlcountry, "ctryname", "ctrycode", "select * from ctrymast where active=1 order by ctryname", True)
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCountryCode, "ctrycode", "select ctrycode from ctrymast where active=1 order by ctrycode", True)
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCityCode', "citycode", "select citycode from citymast where active=1 order by citycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorGroupCode, "othtypcode", "othtypname", "select othtypcode,othtypname from othtypmast where active=1 and othgrpcode ='" & default_group & "' order by othtypcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlSectorGroupName, "othtypname", "othtypcode", "select othtypname,othtypcode from othtypmast where active=1 and othgrpcode ='" & default_group & "' order by othtypname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryCode, "ctrycode", "ctryname", "select ctrycode,ctryname from ctrymast where active=1 order by ctrycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCountryName, "ctryname", "ctrycode", "select ctryname,ctrycode from ctrymast where active=1 order by ctryname", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname  from citymast where active=1  order by citycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode  from citymast where active=1  order by cityname", True)
                default_group = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("1001", String))
                ' Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)
                default_country = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("459", String))
                ddlCountryName.Value = CType(default_country, String)
                ddlCountryCode.Value = ddlCountryName.Items(ddlCountryName.SelectedIndex).Text
                txtcountrycode.Text = CType(default_country, String)
                txtcountryname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", CType(default_country, String))
                Session("countrycode") = txtcountrycode.Text
                If ViewState("SectorsState") = "New" Then
                    SetFocus(txtSectorCode)
                    lblHeading.Text = "Add New Sector"
                    Page.Title = Page.Title + " " + "New Sector Master"
                    btnSave.Text = "Save"
                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save sector?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("SectorsState") = "Edit" Then
                    SetFocus(txtSectorName)
                    lblHeading.Text = "Edit Sector"
                    Page.Title = Page.Title + " " + "Edit Sector Master"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("SectorsRefCode"), String))
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update sector?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("SectorsState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Sector"
                    Page.Title = Page.Title + " " + "View Sector Master"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("SectorsRefCode"), String))
                ElseIf ViewState("SectorsState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Sector"
                    Page.Title = Page.Title + " " + "Delete Sector Master"
                    btnSave.Text = "Delete"
                    DisableControl()
                    ShowRecord(CType(ViewState("SectorsRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete sector?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")



                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlCountryCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCountryName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCityCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If


                '                btnSave.Attributes.Add("onclick", "return FormValidation()")

                '   ValidateOnlyNumber()

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("Cities.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        Else
            Try
                If ddlCountryCode.Value <> "[Select]" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname  from citymast where ctrycode='" & ddlCountryName.Value & "' and active=1 order by citycode", True, ddlCityCode.Value)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode  from citymast where ctrycode='" & ddlCountryName.Value & "' and active=1 order by cityname", True, ddlCityName.Value)
                Else
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname  from citymast where active=1 order by citycode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode  from citymast where active=1 order by cityname", True)
                End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "SupcatsfromWindowPostBack") Then
            If Session("addcategory") = "new" Then
                If Session("CitiesName") IsNot Nothing Then
                    If Session("CitiesCode") IsNot Nothing Then
                        Dim cityname As String = Session("CitiesName")
                        txtcitycode.Text = Session("CitiesCode")
                        txtcityname.Text = cityname
                        Session.Remove("addcategory")
                        Session.Remove("CitiesName")
                        Session.Remove("CitiesCode")
                    End If
                End If
            End If
        End If

        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CtryWindowPostBack") Then
            If Session("addcategory") = "new" Then
                If Session("CountryName") IsNot Nothing Then
                    If Session("CountryCode") IsNot Nothing Then
                        Dim countryname As String = Session("CountryName")
                        txtcountrycode.Text = Session("CountryCode")
                        txtcountryname.Text = countryname
                        Session.Remove("addcategory")
                        Session.Remove("CountryName")
                        Session.Remove("CountryCode")
                    End If
                End If
            End If
        End If




    End Sub
#End Region


#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        If ViewState("SectorsState") = "View" Or ViewState("SectorsState") = "Delete" Then
            txtSectorCode.Disabled = True
            txtSectorName.Disabled = True
            'ddlCountryCode.Enabled = False
            ddlCountryCode.Disabled = True
            ddlSectorGroupCode.Disabled = True
            ddlSectorGroupName.Disabled = True
            txtSectorGroupName.Enabled = False
            txtcountryname.Enabled = False
            txtcityname.Enabled = False
            btnCity.Enabled = False
            btnCountry.Enabled = False
            chkshow.Disabled = True

            'ddlCityCode.Enabled = False
            ddlCityCode.Disabled = True
            chkActive.Disabled = True
            'TxtCountryName.Disabled = True
            ddlCountryName.Disabled = True
            'TxtCityName.Disabled = True
            ddlCityName.Disabled = True
        ElseIf Session("State") = "Edit" Then
            txtSectorCode.Disabled = True
        End If

    End Sub

#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Try
            If txtSectorCode.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Code can not be blank.');", True)
                SetFocus(txtSectorCode)
                ValidatePage = False
                Exit Function
            End If
            If txtSectorName.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name can not be blank.');", True)
                SetFocus(txtSectorName)
                ValidatePage = False
                Exit Function
            End If

            'If ddlCountryCode.SelectedValue = "[Select]" Then
            If ddlCountryCode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select country code.');", True)
                SetFocus(ddlCountryCode)
                ValidatePage = False
                Exit Function
            End If
            'If TxtCountryName.Value = "" Then
            If ddlCountryName.Value = "[Select]" Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Country name can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Country name .');", True)
                'SetFocus(TxtCountryName)
                SetFocus(ddlCountryName)
                ValidatePage = False
                Exit Function
            End If

            'If ddlCityCode.SelectedValue = "[Select]" Then
            If ddlCityCode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select city code.');", True)
                SetFocus(ddlCityCode)
                ValidatePage = False
                Exit Function
            End If
            'If TxtCityName.Value = "" Then
            If ddlCityName.Value = "[Select]" Then
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('City name can not be blank.');", True)
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Pleae select City name.');", True)
                'SetFocus(TxtCityName)
                SetFocus(ddlCityName)
                ValidatePage = False
                Exit Function
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
        ValidatePage = True
    End Function
#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Page.IsValid = True Then
                If ViewState("SectorsState") = "New" Or ViewState("SectorsState") = "Edit" Then
                    'If ValidatePage() = False Then
                    '    Exit Sub
                    'End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("SectorsState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_sectormaster", mySqlConn, sqlTrans)

                    ElseIf ViewState("SectorsState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_sectormaster", mySqlConn, sqlTrans)

                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    'mySqlCmd.Parameters.Add(New SqlParameter("@div_code", SqlDbType.VarChar, 10)).Value = CType(txtSectorCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = CType(txtSectorCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorname", SqlDbType.VarChar, 150)).Value = CType(txtSectorName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorgroupcode", SqlDbType.VarChar, 20)).Value = CType(txtSectorGroupCode.Text, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType(ddlCountryCode.SelectedValue, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType(txtcountrycode.Text, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = CType(ddlCityCode.SelectedValue, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = CType(txtcitycode.Text, String)
                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@showinweb", SqlDbType.Int)).Value = IIf(chkshow.Checked = True, 1, 0)

                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("SectorsState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_sectormaster", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@sectorcode", SqlDbType.VarChar, 20)).Value = CType(txtSectorCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If



                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                If ViewState("SectorValue") = "Addfrom" Then
                    Session.Add("SectorCode", txtSectorCode.Value)
                    Session.Add("SectorName", txtSectorName.Value)
                    Dim strscript1 As String = ""
                    strscript1 = "window.opener.__doPostBack('SupcatsfromWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript1, True)
                End If
                If ViewState("SectorsState") = "New" Or ViewState("SectorsState") = "View" Or ViewState("SectorsState") = "Edit" Or ViewState("SectorsState") = "Delete" Then
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('SectSupWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                End If

            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()

            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from sectormaster Where sectorcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("sectorcode")) = False Then
                        Me.txtSectorCode.Value = CType(mySqlReader("sectorcode"), String)
                    Else
                        Me.txtSectorCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("sectorname")) = False Then
                        Me.txtSectorName.Value = CType(mySqlReader("sectorname"), String)
                    Else
                        Me.txtSectorName.Value = ""
                    End If
                    'ctrycode
                    If IsDBNull(mySqlReader("ctrycode")) = False Then
                        'Me.ddlCountryCode.SelectedValue = CType(mySqlReader("ctrycode"), String)
                        'Me.TxtCountryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))
                        Me.ddlCountryCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))
                        Me.ddlCountryName.Value = CType(mySqlReader("ctrycode"), String)
                        Me.txtcountryname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))
                        Me.txtcountrycode.Text = CType(mySqlReader("ctrycode"), String)
                    End If
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname  from citymast where active=1 and ctrycode='" & ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text & "' order by citycode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode  from citymast where active=1 and ctrycode='" & ddlCountryCode.Items(ddlCountryCode.SelectedIndex).Text & "' order by cityname", True)
                    If IsDBNull(mySqlReader("citycode")) = False Then
                        Me.ddlCityCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "citymast", "cityname", "citycode", CType(mySqlReader("citycode"), String))
                        Me.ddlCityName.Value = CType(mySqlReader("citycode"), String)
                        Me.txtcityname.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "citymast", "cityname", "citycode", CType(mySqlReader("citycode"), String))
                        Me.txtcitycode.Text = CType(mySqlReader("citycode"), String)

                    End If
                    If IsDBNull(mySqlReader("sectorgroupcode")) = False Then
                        Me.ddlSectorGroupName.Value = CType(mySqlReader("sectorgroupcode"), String)
                        Me.ddlSectorGroupCode.Value = ddlSectorGroupName.Items(ddlSectorGroupName.SelectedIndex).Text
                        Me.txtSectorGroupName.Text = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "othtypmast", "othtypname", "othtypcode", CType(mySqlReader("sectorgroupcode"), String))
                        Me.txtSectorGroupCode.Text = CType(mySqlReader("sectorgroupcode"), String)

                    End If

                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If
                    If IsDBNull(mySqlReader("showinweb")) = False Then
                        If CType(mySqlReader("showinweb"), String) = "1" Then
                            chkshow.Checked = True
                        ElseIf CType(mySqlReader("showinweb"), String) = "0" Then
                            chkshow.Checked = False
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("SectorSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region
#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region
#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If ViewState("SectorsState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "sectormaster", "sectorcode", CType(txtSectorCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This sector code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "sectormaster", "sectorname", txtSectorName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This sector name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("SectorsState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "sectormaster", "sectorcode", "sectorname", txtSectorName.Value.Trim, CType(txtSectorCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This sector name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region

    '#Region "Protected Sub ddlcountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    '    Protected Sub ddlcountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '        Try
    '            strSqlQry = ""
    '            TxtCountryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", ddlCountryCode.SelectedValue)
    '        Catch ex As Exception

    '        End Try
    '    End Sub
    '#End Region

    '#Region "Protected Sub ddlCityCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"
    '    Protected Sub ddlCityCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '        Try
    '            strSqlQry = ""
    '            TxtCityName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"citymast", "cityname", "citycode", ddlCityCode.SelectedValue)
    '        Catch ex As Exception

    '        End Try
    '    End Sub
    '#End Region

    Public Sub New()

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymast", "sectorcode", CType(txtSectorCode.Value.Trim, String)) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierSector is already used for a Suppliers, cannot delete this Sector');", True)
            checkForDeletion = False
            Exit Function
        End If
        checkForDeletion = True
    End Function
#End Region


    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Sector','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
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
    <System.Web.Script.Services.ScriptMethod()> _
     <System.Web.Services.WebMethod()> _
    Public Shared Function Getcitylist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""
        Dim ctry As String = "" 'contextKey
        Dim myDS As New DataSet
        Dim Citynames As New List(Of String)
        Try
            'If HttpContext.Current.Session("") IsNot Nothing Then

            'End If

            If HttpContext.Current.Session("countrycode") IsNot Nothing Then
                ctry = HttpContext.Current.Session("countrycode")
            End If

            strSqlQry = "select cityname,citycode from citymast where active=1"
            If Trim(ctry) <> "" Then
                strSqlQry = strSqlQry + " and ctrycode='" & Trim(ctry) & "'"
            End If
            strSqlQry = strSqlQry + " and cityname like  '" & Trim(prefixText) & "%'"

            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Citynames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("cityname").ToString(), myDS.Tables(0).Rows(i)("citycode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Citynames
        Catch ex As Exception
            Return Citynames
        End Try
    End Function
    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function Getsectorgrouplist(ByVal prefixText As String) As List(Of String)
        Dim strSqlQry As String = ""

        Dim myDS As New DataSet
        Dim Sectornames As New List(Of String)
        Try
            strSqlQry = "select othtypname,othtypcode from othtypmast where active=1  and othgrpcode='TRFS' and othtypname like '" & Trim(prefixText) & "%'"
            ' strSqlQry = strSqlQry + " and sectorname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)
            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1
                    Sectornames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("othtypname").ToString(), myDS.Tables(0).Rows(i)("othtypcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            Else



            End If
            Return Sectornames
        Catch ex As Exception
            Return Sectornames
        End Try
    End Function
    Protected Sub txtSectorGroupName_TextChanged(sender As Object, e As System.EventArgs) Handles txtSectorGroupName.TextChanged
        Session("sectorgroupcode") = txtSectorGroupCode.Text
    End Sub
    Protected Sub txtcountryname_TextChanged(sender As Object, e As System.EventArgs) Handles txtcountryname.TextChanged
        Session("countrycode") = txtcountrycode.Text
    End Sub

    Protected Sub btnCountry_Click(sender As Object, e As System.EventArgs) Handles btnCountry.Click
        Dim strpop As String = ""
        strpop = "window.open('Countries.aspx?State=New&Value=Addfrom','Country');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub

    Protected Sub btnCity_Click(sender As Object, e As System.EventArgs) Handles btnCity.Click
        Dim strpop As String = ""
        strpop = "window.open('Cities.aspx?State=New&Value=Addfrom','City');"
        Session.Add("addcategory", "new")
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strpop, True)
    End Sub
End Class
