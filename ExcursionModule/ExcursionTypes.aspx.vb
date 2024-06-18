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
            
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpcode, "othgrpcode", "othgrpname", "select othgrpcode,othgrpname from othgrpmast where active=1 and othmaingrpcode in ('EXU','SAFARI') order by othgrpcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlgpname, "othgrpname", "othgrpcode", "select othgrpcode,othgrpname from othgrpmast where active=1 and othmaingrpcode in ('EXU','SAFARI') order by othgrpname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlpartycode, "partycode", "partyname", "select partycode,partyname from partymast  where active=1 order by partycode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlpartyname, "partyname", "partycode", "select partycode,partyname from partymast where active=1  order by partyname", True)

                ViewState.Add("excServiceSelltypeState", Request.QueryString("State"))
                ViewState.Add("excServiceSelltypeRefCode", Request.QueryString("RefCode"))


                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccCode, "acctcode", "acctname", "select acctcode,acctname from acctgroup where childid=38 order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccName, "acctname", "acctcode", "select  acctname,acctcode from acctgroup where childid=38 order by acctname", True)

                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccrualCode, "acctcode", "acctname", "select acctcode,acctname from acctgroup where childid=39 order by acctcode", True)
                objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlAccrualName, "acctname", "acctcode", "select  acctname,acctcode from acctgroup where childid=39 order by acctname", True)

                If ViewState("excServiceSelltypeState") = "New" Then
                    SetFocus(txtcode)

                    ddlAccName.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id =923")
                    ddlAccCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", ddlAccName.Value)

                    ddlAccrualName.Value = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id =924")
                    ddlAccrualCode.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", ddlAccrualName.Value)
                 
                    lblHeading.Text = "Add New - Excursion   Types"
                    Page.Title = Page.Title + " " + "New Excursion   Types"
                    btnSave.Text = "Save"
                    chktkt.Checked = False
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Excursion Types?')==false)return false;")
                    'btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("excServiceSelltypeState") = "Edit" Then
                    SetFocus(txtname)
                    
                    lblHeading.Text = "Edit -Excursion  Types"
                    Page.Title = Page.Title + " " + "Edit Excursion   Types"
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("excServiceSelltypeRefCode"), String))
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update Excursion Types?')==false)return false;")
                    'btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("excServiceSelltypeState") = "View" Then
                    SetFocus(btnCancel)


                    
                    lblHeading.Text = "View - Excursion  Types"
                    Page.Title = Page.Title + " " + "View Excursion    Types"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()

                    ShowRecord(CType(ViewState("excServiceSelltypeRefCode"), String))

                ElseIf ViewState("excServiceSelltypeState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete - Excursion  Types"


                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("excServiceSelltypeRefCode"), String))
                    '   btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Excursion Types?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
                Dim typ As Type
                typ = GetType(DropDownList)

                'If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
                '    Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

                '    ddlgpcode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                '    ddlgpname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
                'End If
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
            chktkt.Disabled = True
            chkActive.Disabled = True

            chkreq.Disabled = True
            chkprntconf.Disabled = True
            txtattention.Disabled = True
            ddlpartycode.Disabled = True
            ddlpartyname.Disabled = True

        ElseIf ViewState("excServiceSelltypeState") = "Edit" Then
            txtcode.Disabled = True
            ddlgpcode.Disabled = False
            ddlgpname.Disabled = False
            chkActive.Disabled = False

            chkreq.Disabled = False
            chkprntconf.Disabled = False
            txtattention.Disabled = False
            ddlpartycode.Disabled = False
            ddlpartyname.Disabled = False
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
            If txtname.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name can not be blank.');", True)
                SetFocus(txtname)
                ValidatePage = False
                Exit Function
            End If
            If ddlgpcode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Group code.');", True)
                SetFocus(ddlgpcode)
                ValidatePage = False
                Exit Function
            End If

            If ddlAccCode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Income Code');", True)
                SetFocus(ddlAccCode)
                ValidatePage = False
                Exit Function
            End If

            If ddlAccrualCode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Expense Code ');", True)
                SetFocus(ddlAccrualCode)
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
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")
        Dim strPwd As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1089")
        Dim strpath1 As String = ""
        Dim strpath2 As String = ""
        Dim strpath3 As String = ""
        Dim strpath4 As String = ""
        Dim strpath_logo1 As String = ""
        Dim strpath_logo2 As String = ""
        Dim strpath_logo3 As String = ""
        Dim strpath_logo4 As String = ""
        Try
            If Page.IsValid = True Then
                If ViewState("excServiceSelltypeState") = "New" Or ViewState("excServiceSelltypeState") = "Edit" Then

                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If

                    If fileVehicleImage.FileName <> "" Then
                        strpath_logo1 = fileVehicleImage.FileName ' IIf(txtimg.Value = "", fileVehicleImage.FileName, txtimg.Value)

                        strpath1 = Server.MapPath("Excursionimages/" & strpath_logo1)
                        fileVehicleImage.PostedFile.SaveAs(strpath1)
                        txtimg.Value = strpath_logo1
                    Else
                        txtimg.Value = IIf(txtimg.Value = "", fileVehicleImage.FileName, txtimg.Value)

                        ' SendImageToWebService(strpath1, strpath_logo1)
                    End If



                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("excServiceSelltypeState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_exctypes", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf ViewState("excServiceSelltypeState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_exctypes", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtcode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypname", SqlDbType.VarChar, 100)).Value = CType(txtname.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlgpname.Value, String)

                    If chkprntconf.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@printconf", SqlDbType.Int)).Value = 1
                    ElseIf chkprntconf.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@printconf", SqlDbType.Int)).Value = 0
                    End If

                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If

                    If ViewState("excServiceSelltypeState") = "New" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    ElseIf ViewState("excServiceSelltypeState") = "Edit" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    End If

                    If chktkt.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@ticketsreqd", SqlDbType.Int)).Value = 1
                    ElseIf chktkt.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@ticketsreqd", SqlDbType.Int)).Value = 0
                    End If

                    If chkreq.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@uponrequest", SqlDbType.Int)).Value = 1
                    ElseIf chkreq.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@uponrequest", SqlDbType.Int)).Value = 0
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@partycode", SqlDbType.VarChar, 20)).Value = CType(ddlpartyname.Value, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@attn", SqlDbType.VarChar, 200)).Value = CType(txtattention.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@imagename", SqlDbType.VarChar, 100)).Value = CType(txtimg.Value, String) ' CType(fileVehicleImage.FileName, String)
                    'mySqlCmd.Parameters.Add(New SqlParameter("@imagename", SqlDbType.VarChar, 100)).Value = CType(fileVehicleImage.FileName, String)
                    'fileVehicleImage.SaveAs(Server.MapPath("Excursionimages/" & fileVehicleImage.FileName))

                    If ddlAccCode.Value <> "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@incomecode", SqlDbType.VarChar, 20)).Value = CType(ddlAccCode.Items(ddlAccCode.SelectedIndex).Text.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@incomecode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If
                    If ddlAccrualCode.Value <> "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@expensecode", SqlDbType.VarChar, 20)).Value = CType(ddlAccrualCode.Items(ddlAccrualCode.SelectedIndex).Text.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@expensecode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    End If


                    mySqlCmd.ExecuteNonQuery()


                ElseIf ViewState("excServiceSelltypeState") = "Delete" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_del_exctypes", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(txtcode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                ' Response.Redirect("Other Services Selling Types Search.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('OtherSerSelltypeWindowPostBack', '');window.opener.focus();window.close();"
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

#Region " Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from othtypmast Where othtypcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("othtypcode")) = False Then
                        Me.txtcode.Value = CType(mySqlReader("othtypcode"), String)
                    Else
                        Me.txtcode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("othtypname")) = False Then
                        Me.txtname.Value = CType(mySqlReader("othtypname"), String)
                    Else
                        Me.txtname.Value = ""
                    End If

                    If IsDBNull(mySqlReader("othgrpcode")) = False Then
                        Me.ddlgpname.Value = CType(mySqlReader("othgrpcode"), String)
                        'Me.ddlgpcode.SelectedIndex = Me.ddlgpname.SelectedIndex     'objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))
                        Me.ddlgpcode.Value = Me.ddlgpname.Items(ddlgpname.SelectedIndex).Text
                    End If
                    'partycode
                    If IsDBNull(mySqlReader("partycode")) = False Then
                        Me.ddlpartyname.Value = CType(mySqlReader("partycode"), String)
                        'Me.ddlpartycode.SelectedIndex = Me.ddlpartyname.SelectedIndex     'objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))

                        Me.ddlpartycode.Value = Me.ddlpartyname.Items(ddlpartyname.SelectedIndex).Text
                    End If

                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("ticketsreqd")) = False Then
                        If CType(mySqlReader("ticketsreqd"), String) = "1" Then
                            chktkt.Checked = True
                        ElseIf CType(mySqlReader("ticketsreqd"), String) = "0" Then
                            chktkt.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("uponrequest")) = False Then
                        If CType(mySqlReader("uponrequest"), String) = "1" Then
                            chkreq.Checked = True
                        ElseIf CType(mySqlReader("uponrequest"), String) = "0" Then
                            chkreq.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("incomecode")) = False Then
                        ddlAccCode.Value = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", CType(mySqlReader("incomecode"), String)), String)
                        ddlAccName.Value = CType(mySqlReader("incomecode"), String)
                    Else
                        ddlAccCode.Value = "[Select]"
                        ddlAccName.Value = "[Select]"
                    End If

                    If IsDBNull(mySqlReader("expensecode")) = False Then
                        ddlAccrualCode.Value = CType(objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "acctmast", "acctname", "acctcode", CType(mySqlReader("expensecode"), String)), String)
                        ddlAccrualName.Value = CType(mySqlReader("expensecode"), String)
                    Else
                        ddlAccrualCode.Value = "[Select]"
                        ddlAccrualName.Value = "[Select]"
                    End If

                    If IsDBNull(mySqlReader("printconf")) = False Then
                        If CType(mySqlReader("printconf"), String) = "1" Then
                            chkprntconf.Checked = True
                        ElseIf CType(mySqlReader("printconf"), String) = "0" Then
                            chkprntconf.Checked = False
                        End If
                    End If

                    If IsDBNull(mySqlReader("attn")) = False Then
                        Me.txtattention.Value = CType(mySqlReader("attn"), String)

                    End If
                    If IsDBNull(mySqlReader("imagename")) = False Then
                        Me.txtimg.Value = mySqlReader("imagename")


                    Else
                        Me.txtimg.Value = ""
                    End If
                    
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
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "excsellmast ", "excsellcode", CType(txtcode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Excursion Types code is already present.');", True)
                SetFocus(txtcode)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "excsellmast", "excsellname", txtname.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This  Excursion Types name is already present.');", True)
                SetFocus(txtname)
                checkForDuplicate = False
                Exit Function
            End If
            
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
