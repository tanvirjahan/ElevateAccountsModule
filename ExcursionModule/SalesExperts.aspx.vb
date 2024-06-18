'------------================--------------=======================------------------================
'   Module Name    :    CustomerSector .aspx
'   Developer Name :    Amit Survase
'   Date           :    30 June 2008
'   
''------------================--------------=======================------------------================
#Region "namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region
Partial Class Other_Services_Selling_Types
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

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), , "usercode", "username", "select usercode,username from usermaster where active=1 order by usercode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlname, "username", "usercode", "select username,usercode from usermaster where active=1 order by username", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpcode, "deptcode", "deptname", "select deptcode,deptname from deptmaster where active=1 order by deptcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpname, "deptname", "deptcode", "select deptname,deptcode from deptmaster where active=1 order by deptname", True)

                ViewState.Add("excServiceSelltypeState", Request.QueryString("State"))
                ViewState.Add("excServiceSelltypeRefCode", Request.QueryString("RefCode"))




                If ViewState("excServiceSelltypeState") = "New" Then
                    SetFocus(txtcode)
                 
                    lblHeading.Text = "Add New - Sales Expert "
                    Page.Title = Page.Title + " " + "New Sales Expert"
                    btnSave.Text = "Save"

                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save ?')==false)return false;")
                    'btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("excServiceSelltypeState") = "Edit" Then
                    SetFocus(txtname)
                    
                    lblHeading.Text = "Edit -Sales Experts"
                    Page.Title = Page.Title + " " + "Edit Sales Experts"
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("excServiceSelltypeRefCode"), String))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update Excursion Types?')==false)return false;")
                    'btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("excServiceSelltypeState") = "View" Then
                    SetFocus(btnCancel)


                    
                    lblHeading.Text = "View - Sales Experts "
                    Page.Title = Page.Title + " " + "View Sales Experts"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()

                    ShowRecord(CType(ViewState("excServiceSelltypeRefCode"), String))

                ElseIf ViewState("excServiceSelltypeState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete - Sales Experts"


                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("excServiceSelltypeRefCode"), String))
                    '   btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete ?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
                Dim typ As Type
                typ = GetType(DropDownList)

                If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                    'ddlgpcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                    'ddlgpname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                End If

                txtcommi.Attributes.Add("onkeypress", "return checkNumber(event)")
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ExcursionTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub
#End Region

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("excServiceSelltypeState") = "View" Or ViewState("excServiceSelltypeState") = "Delete" Then
            txtcode.Disabled = True
            txtname.Disabled = True
            ddlgpcode.Disabled = True
            ddlgpname.Disabled = True
            
            txtcommi.Disabled = True
         

        ElseIf ViewState("excServiceSelltypeState") = "Edit" Then
            txtcode.Disabled = True
            txtname.Disabled = False
            ddlgpcode.Disabled = False
            ddlgpname.Disabled = False
            chkActive.Disabled = False

          
            txtcommi.Disabled = False
         
        End If
    End Sub
#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Try
            If txtcode.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Code can not be blank.');", True)
                SetFocus(txtcode)
                ValidatePage = False
                Exit Function
            End If
            If txtname.Value.Trim = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name can not be blank.');", True)
                SetFocus(txtname)
                ValidatePage = False
                Exit Function
            End If
            If ddlgpcode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Dept code.');", True)
                SetFocus(ddlgpcode)
                ValidatePage = False
                Exit Function
            End If

            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)

        End Try
    End Function

#End Region

#Region "Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strPassQry As String = "false"
        Dim frmmode As String = 0
          Try
            If Page.IsValid = True Then
                If ViewState("excServiceSelltypeState") = "New" Or ViewState("excServiceSelltypeState") = "Edit" Then

                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("excServiceSelltypeState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_salesexperts", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf ViewState("excServiceSelltypeState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mode_salesexperts", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@spersoncode", SqlDbType.VarChar, 20)).Value = CType(txtcode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@spersonname", SqlDbType.VarChar, 100)).Value = CType(txtname.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@deptcode", SqlDbType.VarChar, 20)).Value = CType(ddlgpname.Value, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@commperc", SqlDbType.Decimal, 18, 4)).Value = 0
                   
                    If ViewState("excServiceSelltypeState") = "New" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    ElseIf ViewState("excServiceSelltypeState") = "Edit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    End If

                    mySqlCmd.ExecuteNonQuery()


                ElseIf ViewState("excServiceSelltypeState") = "Delete" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_del_salesexperts", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@spersoncode", SqlDbType.VarChar, 20)).Value = CType(txtcode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                ' Response.Redirect("Other Services Selling Types Search.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('CityWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

               


            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region "Numbers"
    Public Sub Numbers(ByVal txtbox As HtmlInputText)
        'txtcommi.Attributes.Add("onkeypress", "return checkNumber(event)")
    End Sub
#End Region


#Region " Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from spersonmast_office Where spersoncode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("spersoncode")) = False Then
                        Me.txtcode.Value = CType(mySqlReader("spersoncode"), String)
                    Else
                        Me.txtcode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("spersonname")) = False Then
                        Me.txtname.Value = CType(mySqlReader("spersonname"), String)
                    Else
                        Me.txtname.Value = ""
                    End If


                        
                    End If

                    If IsDBNull(mySqlReader("deptcode")) = False Then
                        ddlgpname.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "deptmaster", "deptcode", "deptcode", mySqlReader("deptcode"))
                        ddlgpcode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "deptmaster", "deptname", "deptcode", ddlgpname.Value)



                    End If

                    If IsDBNull(mySqlReader("commperc")) = False Then
                        txtcommi.Value = CType(mySqlReader("commperc"), String)
                    End If
                End If



        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub

#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("Other Services Selling Types Search.aspx", False)

        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)

    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If ViewState("excServiceSelltypeState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "spersonmast_office ", "spersoncode", CType(txtcode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Sales Person Already Exists.');", True)
                SetFocus(txtcode)
                checkForDuplicate = False
                Exit Function
            End If
            'If objUtils.isDuplicatenew(Session("dbconnectionName"), "spersonmast", "deptcode", ddlgpcode.Items(ddlgpcode.SelectedIndex).Text) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This  Sales Person is already Exist.');", True)
            '    SetFocus(txtname)
            '    checkForDuplicate = False
            '    Exit Function
            'End If


        End If
        checkForDuplicate = True
    End Function
#End Region


#Region "Protected Sub ddlcountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)"

    'Protected Sub ddlCurrencyCd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrencyCd.SelectedIndexChanged
    '    Try
    '        strSqlQry = ""
    '        TxtCurrencyNm.Value = ddlCurrencyCd.SelectedValue
    '    Catch ex As Exception

    '    End Try
    'End Sub
#End Region

#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True

        'If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "agentmast", "othsellcode", CType(TxtOtherServiceCode.Value.Trim, String)) = True Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This ServiceType is already used for a Customer, cannot delete this ServiceType');", True)
        '    checkForDeletion = False
        '    Exit Function

        'ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplistd", "othsellcode", CType(TxtOtherServiceCode.Value.Trim, String)) = True Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This ServiceType is already used for a Details of OtherServicePricelist, cannot delete this ServiceType');", True)
        '    checkForDeletion = False
        '    Exit Function

        'ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplisth", "othsellcode", CType(TxtOtherServiceCode.Value.Trim, String)) = True Then
        '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This ServiceType is already used for a OtherServicePricelist, cannot delete this ServiceType');", True)
        '    checkForDeletion = False
        '    Exit Function


        'End If

        checkForDeletion = True
    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ExcursionTypes','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

 
End Class
