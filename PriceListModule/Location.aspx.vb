#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class PriceListModule_Location
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

                default_country = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("459", String))

                ViewState.Add("LocationsState", Request.QueryString("State"))
                ViewState.Add("LocationsRefCode", Request.QueryString("RefCode"))
                default_group = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("1001", String))
                'objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlcountry, "ctryname", "ctrycode", "select * from ctrymast where active=1 order by ctryname", True)
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCountryCode, "ctrycode", "select ctrycode from ctrymast where active=1 order by ctrycode", True)
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlCityCode', "citycode", "select citycode from citymast where active=1 order by citycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname  from citymast where active=1  and ctrycode ='" & default_country & "' order by citycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode  from citymast where active=1  and ctrycode ='" & default_country & "' order by cityname", True)
                default_group = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", CType("1001", String))
                ' Dim AppId As HiddenField = CType(Master.FindControl("hdnAppId"), HiddenField)







                If ViewState("LocationsState") = "New" Then
                    SetFocus(txtAreaCode)
                    lblHeading.Text = "Add New Area"
                    Page.Title = Page.Title + " " + "New Area Master"
                    btnSave.Text = "Save"

                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save area?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("LocationsState") = "Edit" Then
                    SetFocus(txtAreaName)
                    lblHeading.Text = "Edit Area"
                    Page.Title = Page.Title + " " + "Edit Area Master"
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("LocationsRefCode"), String))
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update area?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("LocationsState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Area"
                    Page.Title = Page.Title + " " + "View Area Master"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()

                    ShowRecord(CType(ViewState("LocationsRefCode"), String))

                ElseIf ViewState("LocationsState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Area"
                    Page.Title = Page.Title + " " + "Delete Area Master"
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("LocationsRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete area?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")



                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")
                    ddlCityCode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    ddlCityName.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If


                ' btnSave.Attributes.Add("onclick", "return FormValidation()")

                ' ValidateOnlyNumber()

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("Cities.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        Else
            Try
                'If ddlCityCode.Value <> "[Select]" Then
                '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityCode, "citycode", "cityname", "select citycode,cityname  from citymast where active=1 order by citycode", True)
                '    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlCityName, "cityname", "citycode", "select cityname,citycode  from citymast where active=1 order by cityname", True)
                'End If
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("LocationMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub
#End Region

#Region "Private Sub DisableControl()"

    Private Sub DisableControl()
        If ViewState("LocationsState") = "View" Or ViewState("LocationsState") = "Delete" Then
            txtAreaCode.Disabled = True
            txtAreaName.Disabled = True
            ddlCityCode.Disabled = True
            ddlCityName.Disabled = True
            chkActive.Disabled = True
        ElseIf Session("State") = "Edit" Then
            txtAreaCode.Disabled = True
        End If

    End Sub

#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Try
            If txtAreaCode.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Code can not be blank.');", True)
                SetFocus(txtAreaCode)
                ValidatePage = False
                Exit Function
            End If
            If txtAreaName.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name can not be blank.');", True)
                SetFocus(txtAreaName)
                ValidatePage = False
                Exit Function
            End If

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
                If ViewState("LocationsState") = "New" Or ViewState("LocationsState") = "Edit" Then
                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("LocationsState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_areamaster", mySqlConn, sqlTrans)

                    ElseIf ViewState("LocationsState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_areamaster", mySqlConn, sqlTrans)

                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@areacode", SqlDbType.VarChar, 20)).Value = CType(txtAreaCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@areaname", SqlDbType.VarChar, 150)).Value = CType(txtAreaName.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@citycode", SqlDbType.VarChar, 20)).Value = CType(ddlCityCode.Items(ddlCityCode.SelectedIndex).Text, String)

                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("LocationsState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_areamaster", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@areacode", SqlDbType.VarChar, 20)).Value = CType(txtAreaCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                End If



                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('LocSupWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)


            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()


            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("LocationMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from areamaster Where areacode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("areacode")) = False Then
                        Me.txtAreaCode.Value = CType(mySqlReader("areacode"), String)
                    Else
                        Me.txtAreaCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("areaname")) = False Then
                        Me.txtAreaName.Value = CType(mySqlReader("areaname"), String)
                    Else
                        Me.txtAreaName.Value = ""
                    End If

                    If IsDBNull(mySqlReader("citycode")) = False Then
                        Me.ddlCityCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "citymast", "cityname", "citycode", CType(mySqlReader("citycode"), String))
                        Me.ddlCityName.Value = CType(mySqlReader("citycode"), String)
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
            objUtils.WritErrorLog("LocationMasterSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        If ViewState("LocationsState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "areamaster", "areacode", CType(txtAreaCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This area code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            Dim mcount As Integer = 0
            Dim mstrVal As String = "SELECT COUNT(areacode) FROM areamaster Where areaname = '" & txtAreaName.Value.Trim & "' and citycode='" & ddlCityCode.Items(ddlCityCode.SelectedIndex).Text & "'"
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                     'Open connection
            mySqlCmd = New SqlCommand(mstrVal, mySqlConn)
            mcount = mySqlCmd.ExecuteScalar
            If mcount > 0 Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This area name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            'If objUtils.isDuplicatenew(Session("dbconnectionName"), "areamaster", "areaname", txtAreaName.Value.Trim) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This area name is already present.');", True)
            '    checkForDuplicate = False
            '    Exit Function
            'End If
        ElseIf ViewState("LocationsState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "areamaster", "areacode", "areaname", txtAreaName.Value.Trim, CType(txtAreaCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This area name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymast", "areacode", CType(txtAreaCode.Value.Trim, String)) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierArea is already used for a Suppliers, cannot delete this Area');", True)
            checkForDeletion = False
            Exit Function
        End If
        checkForDeletion = True
    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=Location','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
