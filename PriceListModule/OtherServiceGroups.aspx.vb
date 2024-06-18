'------------================--------------=======================------------------================
'   Module Name    :    OtherServiceGroups.aspx
'   Developer Name :    Nilesh Sawant
'   Date           :    16 June 2008
'   
'
'------------================--------------=======================------------------================

#Region "Namespace"
Imports System.Data
Imports System.Data.SqlClient
#End Region


Partial Class OtherServiceGroups
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
                ViewState.Add("OthgrpState", Request.QueryString("State"))
                ViewState.Add("OthgrpRefCode", Request.QueryString("RefCode"))
                ViewState.Add("Type", Request.QueryString("Type"))
                Dim otypecode1 As String
                Dim otypecode2 As String
                otypecode1 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1104")
                otypecode2 = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected from reservation_parameters where param_id=1105")

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMainGroupCode, "othmaingrpcode", "othmaingrpname", "select othmaingrpcode,othmaingrpname from othmaingrpmast where active=1 and othmaingrpcode  in('" & otypecode1 & "'" & ",'" & otypecode2 & "')  order by othmaingrpcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlMainGroupName, "othmaingrpname", "othmaingrpcode", "select othmaingrpname,othmaingrpcode from othmaingrpmast where active=1 and othmaingrpcode  in('" & otypecode1 & "'" & ",'" & otypecode2 & "') order by othmaingrpname", True)

                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDeptCode, "Deptcode", "deptname", "select Deptcode,deptname from DeptMaster where active=1 order by Deptcode", True)
                'objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlDeptName, "deptname", "Deptcode", "select Deptcode,deptname from DeptMaster where active=1 order by Deptcode", True)

                If ViewState("OthgrpState") = "New" Then
                    SetFocus(txtCode)
                    If Request.QueryString("Type") = "EXU" Then
                        lblHeading.Text = "Add New Excursion Group"
                        Page.Title = Page.Title + " " + "Add New Excursion Group"

                    Else
                        lblHeading.Text = "Add New Other Services Group"
                        Page.Title = Page.Title + " " + "Add New Other Services Group"

                    End If





                    btnSave.Text = "Save"
                    'btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("OthgrpState") = "Edit" Then
                    SetFocus(txtName)
                    If ViewState("Type") = "EXU" Then
                        lblHeading.Text = "Edit Excursion Group"
                        Page.Title = Page.Title + " " + "Edit Excursion Group"

                    Else
                        lblHeading.Text = "Edit Other Services Group"
                        Page.Title = Page.Title + " " + "Edit Other Services Group"

                    End If


                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(ViewState("OthgrpRefCode"), String))

                    CheckPaxCalcDone(txtCode.Value.Trim)

                    ' btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("OthgrpState") = "View" Then
                    SetFocus(btnCancel)
                    If ViewState("Type") = "EXU" Then
                        lblHeading.Text = "View Excursion Group"
                        Page.Title = Page.Title + " " + "View Excursion Group"

                    Else
                        lblHeading.Text = "View Other Services Group"
                        Page.Title = Page.Title + " " + "View Other Services Group"

                    End If
                    btnSave.Visible = False

                    DisableControl()

                    ShowRecord(CType(ViewState("OthgrpRefCode"), String))

                ElseIf ViewState("OthgrpState") = "Delete" Then
                    SetFocus(btnSave)
                    If ViewState("Type") = "EXU" Then
                        lblHeading.Text = "Delete Excursion Group"
                        Page.Title = Page.Title + " " + "Delete Excursion Group"

                    Else
                        lblHeading.Text = "Delete Other Services Group"
                        Page.Title = Page.Title + " " + "Delete Other Services Group"

                    End If
                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("OthgrpRefCode"), String))
                    ' btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If

                If ViewState("Type") = "OTH" Then
                    ddlMainGroupCode.Visible = False
                    ddlMainGroupName.Visible = False
                    lblmaingrpcode.Visible = False

                    Label1.Visible = False

                    ' ecode.visible = False
                    'ename.visible = False
                End If


                Dim typ As Type
                typ = GetType(DropDownList)



                ' btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")

                'charcters(txtCode)
                'charcters(txtName)

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SubSeason.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If
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
        If ViewState("OthgrpState") = "View" Or ViewState("OthgrpState") = "Delete" Then
            txtCode.Disabled = True
            txtName.Disabled = True
            chkActive.Disabled = True

            txtterms.Enabled = False
            ddlMainGroupCode.Disabled = True
            ddlMainGroupName.Disabled = True
           

        ElseIf ViewState("OthgrpState") = "Edit" Then
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
            'If ddlDept.SelectedValue = "[Select]" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select department.');", True)
            '    SetFocus(ddlDept)
            '    ValidatePage = False
            '    Exit Function
            'End If

            If ViewState("Type") = "EXU" Then
                If ddlMainGroupCode.Value = "[Select]" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Main Excursion Group Code');", True)
                    SetFocus(ddlMainGroupCode)
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

                If ViewState("OthgrpState") = "New" Or ViewState("OthgrpState") = "Edit" Then

                    If ValidatePage() = False Then
                        Exit Sub
                    End If


                    If checkForDuplicate() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction

                    'SQL  Trans start

                    If ViewState("Type") = "OTH" Then

                        If ViewState("OthgrpState") = "New" Then

                            mySqlCmd = New SqlCommand("sp_add_othgroupmast", mySqlConn, sqlTrans)

                        ElseIf ViewState("OthgrpState") = "Edit" Then
                            mySqlCmd = New SqlCommand("sp_mod_othgroupmast", mySqlConn, sqlTrans)

                        End If

                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@othmaingrpcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@othmaingrpname", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)

                        If chkActive.Checked = True Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                        ElseIf chkActive.Checked = False Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                        End If
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        'End If


                        mySqlCmd.ExecuteNonQuery()
                    End If

                    If ViewState("OthgrpState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_othgrp", mySqlConn, sqlTrans)

                    ElseIf ViewState("OthgrpState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_othgrp", mySqlConn, sqlTrans)
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpname", SqlDbType.VarChar, 100)).Value = CType(txtName.Value.Trim, String)
                    If ViewState("Type") = "OTH" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@othmaingrpcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@othmaingrpcode", SqlDbType.VarChar, 20)).Value = CType(ddlMainGroupCode.Items(ddlMainGroupCode.SelectedIndex).Text, String)
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@deptcode", SqlDbType.VarChar, 20)).Value = DBNull.Value
                    mySqlCmd.Parameters.Add(New SqlParameter("@ratesbasedonpax", SqlDbType.VarChar, 20)).Value = 1 'ddlpax.SelectedValue -CType(ddlpax.Items(ddlpax.SelectedIndex).Text, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@paxrate", SqlDbType.VarChar, 20)).Value = 0
                    mySqlCmd.Parameters.Add(New SqlParameter("@terms", SqlDbType.VarChar, 1000)).Value = txtterms.Text
                    mySqlCmd.Parameters.Add(New SqlParameter("@printgrp", SqlDbType.Int)).Value = 1
                    mySqlCmd.Parameters.Add(New SqlParameter("@printtype", SqlDbType.Int)).Value = 1
                    mySqlCmd.Parameters.Add(New SqlParameter("@printcat", SqlDbType.Int)).Value = 1

                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()



                ElseIf ViewState("OthgrpState") = "Delete" Then


                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    mySqlCmd = New SqlCommand("sp_del_othgrp", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                    If ViewState("Type") = "OTH" Then
                        mySqlCmd = New SqlCommand("sp_del_othgroupmast", mySqlConn, sqlTrans)


                        mySqlCmd.CommandType = CommandType.StoredProcedure
                        mySqlCmd.Parameters.Add(New SqlParameter("@othgrpcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Value.Trim, String)
                        mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                        mySqlCmd.ExecuteNonQuery()
                    End If
                End If

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                'Response.Redirect("OtherServiceGroupsSearch.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('OthgrpWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("OtherServiceGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"

    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from othgrpmast Where othgrpcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("othgrpcode")) = False Then
                        Me.txtCode.Value = CType(mySqlReader("othgrpcode"), String)
                    Else
                        Me.txtCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("othgrpname")) = False Then
                        Me.txtName.Value = CType(mySqlReader("othgrpname"), String)
                    Else
                        Me.txtName.Value = ""
                    End If

                    If IsDBNull(mySqlReader("othmaingrpcode")) = False Then
                        Me.ddlMainGroupName.Value = CType(mySqlReader("othmaingrpcode"), String)
                        Me.ddlMainGroupCode.Value = ddlMainGroupName.Items(ddlMainGroupName.SelectedIndex).Text
                    Else

                        ddlMainGroupCode.Value = "[Select]"
                        ddlMainGroupName.Value = "[Select]"
                    End If
                  
                   
                    txtterms.Text = mySqlReader("terms")
                 


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
            objUtils.WritErrorLog("OtherServiceGroups.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close           
        End Try
    End Sub
#End Region

#Region "Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("OtherServiceGroupsSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
    End Sub
#End Region

    Private Sub CheckPaxCalcDone(ByVal prm_strGrpCode As String)
        Dim strOptionaName As String

        strOptionaName = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", "param_id", "1001")
        'TRFS'
        If prm_strGrpCode = strOptionaName Then
            'Dim strQry As String = " select 't' from trfplist_selld"
            If objUtils.EntryExists(Session("dbconnectionName"), "trfplist_selld", "oclineno", "") = True Then
                '  ddlRate.Enabled = False
                Exit Sub
            ElseIf objUtils.EntryExists(Session("dbconnectionName"), "trfplist_othcat_slabs", "oclineno", "") = True Then
                '  ddlRate.Enabled = False
                Exit Sub
            End If
        End If
    End Sub
#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
      
        If ViewState("OthgrpState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "othgrpmast", "othgrpcode", CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other group code is already present.');", True)
                SetFocus(txtCode)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "othgrpmast", "othgrpname", txtName.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other group name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf ViewState("OthgrpState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "othgrpmast", "othgrpcode", "othgrpname", txtName.Value.Trim, CType(txtCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This other group name is already present.');", True)
                SetFocus(txtName)
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

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "othserv_policy", "othgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Service is already used for a OtherServicesPolicy, cannot delete this Service');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "othplisth", "othgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Service is already used for a OtherServicesPriceList, cannot delete this Service');", True)
            checkForDeletion = False
            Exit Function


            'ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplist_costh", "othgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Service is already used for a OtherSevices CostPriceList, cannot delete this Service');", True)
            '    checkForDeletion = False
            '    Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "othtypmast", "othgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Service is already used for a OtherServicesTypes, cannot delete this Service');", True)
            checkForDeletion = False
            Exit Function

            'ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyothgrp", "othgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Service is already used for a OtherServicesgroup of Suppliers, cannot delete this Service');", True)
            '    checkForDeletion = False
            '    Exit Function

            'ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyothtyp", "othgrpcode", CType(txtCode.Value.Trim, String)) = True Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Service is already used for a OtherServicesTypes of Suppilers, cannot delete this Service');", True)
            '    checkForDeletion = False
            '    Exit Function

            'ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "reservation_parameters", "option_selected", CType(txtCode.Value.Trim, String)) = True Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This group cannot be deleted');", True)
            '    checkForDeletion = False
            '    Exit Function
        Else
            Dim strQry As String = "select 't' from reservation_parameters where option_selected ='" & txtCode.Value.Trim & "' and param_id  in (1001,1002,1003,1021,1022,1023,1024,1025)"
            Dim strVal As String
            strVal = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), strQry)
            If strVal <> Nothing Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This group cannot be deleted');", True)
                checkForDeletion = False
                Exit Function
            End If
        End If

        'select 't' from reservation_parameters where option_selected ='TRFS' and param_id  in (1001,1002,1003,1021,1022,1023)

        'select option_selected from reservation_parameters where option_selected ='VISA'


        checkForDeletion = True
    End Function
#End Region
    
     
    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherServiceGroups','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
