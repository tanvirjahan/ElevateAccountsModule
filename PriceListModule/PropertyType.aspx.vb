#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
#End Region

Partial Class PropertyType
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
                ViewState.Add("PropertyState", Request.QueryString("State"))
                ViewState.Add("CitiesRefCode", Request.QueryString("RefCode"))
                ViewState.Add("CitiesValue", Request.QueryString("Value"))
                'txtcon.Disabled = True
                ''objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlcountry, "ctryname", "ctrycode", "select * from ctrymast where active=1 order by ctryname", True)
                'objUtils.FillDropDownListnew(Session("dbconnectionName"), ddlcountry, "ctrycode", "select ctrycode from ctrymast where active=1 order by ctrycode", True)



                '   txtAutoComplete.Attributes.Add("onkeyup", "SetContextKey()")
                If ViewState("PropertyState") = "New" Then
                    SetFocus(txtCode)
                    lblHeading.Text = "Add New  Property Type"
                    Page.Title = Page.Title + " " + "New Property Type"
                    btnSave.Text = "Save"




                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Property Type?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("PropertyState") = "Edit" Then
                    SetFocus(txtName)
                    lblHeading.Text = "Edit Property Type"
                    Page.Title = Page.Title + " " + "Edit Property Type"
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("CitiesRefCode"), String))
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update Property Type?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("PropertyState") = "View" Then
                    SetFocus(btnCancel)
                    lblHeading.Text = "View Property Type"
                    Page.Title = Page.Title + " " + "View Property Type"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()
                    ShowRecord(CType(ViewState("CitiesRefCode"), String))
                    ' btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("PropertyState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete Property Type"
                    Page.Title = Page.Title + " " + "Delete Property Type"
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("CitiesRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Property Type?')==false)return false;")
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


                End If

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("PropertyType.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

        If (Me.IsPostBack And Me.Request("__EVENTTARGET") = "CtryWindowPostBack") Then
            If Session("addcountry") = "New" Then
                If Session("CountryName") IsNot Nothing Then
                    If Session("CountryCode") IsNot Nothing Then
                        Dim countryname As String = Session("CountryName")

                        Session.Remove("addregion")
                        Session.Remove("CountryName")
                        Session.Remove("CountryCode")
                    End If
                End If
            End If
        End If

        Page.Title = "Property Entry"
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
        If ViewState("PropertyState") = "View" Or ViewState("PropertyState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            'ddlcountry.Enabled = False

            chkActive.Disabled = True
            'txtCountryName.Disabled = True

        ElseIf ViewState("PropertyState") = "Edit" Then
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
                If ViewState("PropertyState") = "New" Or ViewState("PropertyState") = "Edit" Then
                    'If ValidatePage() = False Then
                    '    Exit Sub
                    'End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("PropertyState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_propertytype", mySqlConn, sqlTrans)
                        frmmode = 1

                    ElseIf ViewState("PropertyState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_propertytype", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@propertytypecode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@propertytypename", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@ctrycode", SqlDbType.VarChar, 20)).Value = CType(ddlCCode.Items(ddlCCode.SelectedIndex).Text, String)



                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("PropertyState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_del_propertytype ", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@propertytypecode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
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
                If ViewState("PropertyState") = "New" Or ViewState("PropertyState") = "View" Or ViewState("PropertyState") = "Edit" Or ViewState("PropertyState") = "Delete" Then
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
#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymast", "propertytype", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Property Type is already used for  partymast, cannot delete this Property Type');", True)
            checkForDeletion = False
            Exit Function


        End If

        checkForDeletion = True
    End Function
#End Region
#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from hotel_propertytype Where propertytypecode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("propertytypecode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("propertytypecode"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("propertytypename")) = False Then
                        Me.txtName.Value = CType(mySqlReader("propertytypename"), String)
                    Else
                        Me.txtName.Value = ""
                    End If








                    'If IsDBNull(mySqlReader("ctrycode")) = False Then
                    '    Me.ddlCCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))                        'Me.ddlcountry.SelectedValue = CType(mySqlReader("ctrycode"), String)
                    '    Me.ddlcname.Value = CType(mySqlReader("ctrycode"), String)

                    '    'Me.txtCountryName.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))
                    'End If

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
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "PopupScript", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("propertytype.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        If ViewState("PropertyState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "hotel_propertytype", "propertytypecode", CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This property type code is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "hotel_propertytype", "propertytypename", txtName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This property type name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("PropertyState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "hotel_propertytype ", "propertytypecode", "propertytypename", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This property type name is already present.');", True)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function
#End Region
    '#Region "Public Function checkForDeletion() As Boolean"
    '    Public Function checkForDeletion() As Boolean
    '        checkForDeletion = True
    '        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "agentmast", "citycode", CType(txtCode.Value.Trim, String)) Then
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This city is already used for a Customers, cannot delete this city');", True)
    '            checkForDeletion = False
    '            Exit Function

    '        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymast", "citycode", CType(txtCode.Value.Trim, String)) Then
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This city is already used for a Suppliers, cannot delete this city');", True)
    '            checkForDeletion = False
    '            Exit Function

    '        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "sectormaster", "citycode", CType(txtCode.Value.Trim, String)) Then
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This city is already used for a SupplierSectors, cannot delete this city');", True)
    '            checkForDeletion = False
    '            Exit Function


    '        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "supplier_agents", "citycode", CType(txtCode.Value.Trim, String)) Then
    '            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This city is already used for a SupplierAgents, cannot delete this city');", True)
    '            checkForDeletion = False
    '            Exit Function

    '        End If
    '        checkForDeletion = True
    '    End Function
    '#End Region

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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=PropertyEntry','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub



End Class

