


'------------================--------------=======================------------------================
'   Page Name       :   SupplierType.aspx
'   Developer Name  :    Pramod Desai
'   Date            :    14 June 2008
'------------================--------------=======================------------------================
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

Partial Class ExcOccHtWtMaster
    Inherits System.Web.UI.Page


#Region "Global Declaration"
    Dim objUtils As New clsUtils
    Dim strSqlQry As String
    Dim mySqlCmd As SqlCommand
    Dim mySqlReader As SqlDataReader
    Dim mySqlConn As SqlConnection
    Dim sqlTrans As SqlTransaction
    Private Connection As SqlConnection
    Dim objUser As New clsUser
    Private strImgName As String
#End Region
#Region "Numberssrvctrl"
    Private Sub Numberssrvctrl(ByVal txtbox As WebControls.TextBox)
        txtbox.Attributes.Add("onkeypress", "return  checkNumber(event)")
    End Sub
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If
                Me.SubMenuUserControl1.appval = CType(Request.QueryString("appid"), String)

                Me.SubMenuUserControl1.menuidval = objUser.GetMenuId(Session("dbconnectionName"), CType("ExcursionModule\ExcursionSearch.aspx?appid=" + CType(Request.QueryString("appid"), String), String), CType(Request.QueryString("appid"), Integer))
                If Request.QueryString("State") <> "" Then
                    Session.Add("ExcTypesState", Request.QueryString("State"))
                    Session.Add("ExcTypesRefCode", Request.QueryString("RefCode"))
                End If
                If Session("ExcTypesState") = "New" Then
                    SetFocus(txtCode)
                    lblHeading.Text = "Add New -Excursion-Occupancy Height/Weight"
                    Page.Title = Page.Title + " " + "New Excursion  Types"
                    btnSave.Text = "Save"
                    txtCode.ReadOnly = True
                    txtName.ReadOnly = True
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    'End If
                ElseIf Session("ExcTypesState") = "Edit" Then
                    SetFocus(txtName)
                    txtCode.ReadOnly = True
                    txtName.ReadOnly = True
                    lblHeading.Text = "Edit Excursion-Occupancy Height/Weight"
                    Page.Title = Page.Title + " " + "Edit Excursion Types"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(Session("ExcTypesRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf Session("ExcTypesState") = "View" Then
                    SetFocus(btnCancel)
                    txtCode.ReadOnly = True
                    txtName.ReadOnly = True
                    lblHeading.Text = "View Excursion-Occupancy Height/Weight"
                    Page.Title = Page.Title + " " + "View Excursion Types"
                    btnSave.Visible = False
                    'btnCancel.Text = "Return to Supplier Category"
                    DisableControl()
                    ShowRecord(CType(Session("ExcTypesRefCode"), String))
                ElseIf Session("ExcTypesState") = "Delete" Then
                    SetFocus(btnSave)
                    txtCode.ReadOnly = True
                    txtName.ReadOnly = True
                    lblHeading.Text = "Delete Excursion-Occupancy Height/Weight"
                    Page.Title = Page.Title + " " + "Delete Excursion Types"
                    btnSave.Text = "Delete"
                    DisableControl()
                    ShowRecord(CType(Session("ExcTypesRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                End If

                Dim typ As Type
                typ = GetType(DropDownList)
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
                '   Numberssrvctrl(txtmaxpax)


            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("SupplierCategories.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try

        End If
    End Sub

    Private Sub DisableControl()
        If Session("ExcTypesState") = "View" Or Session("ExcTypesState") = "Delete" Then
            txtCode.Enabled = False
            txtName.Enabled = False
            txtchildagefrm.Enabled = False
            txtchildageto.Enabled = False
            txtchildmaxht.Enabled = False
            txtchildmaxwt.Enabled = False
            txtchildminht.Enabled = False
            txtchildminwt.Enabled = False
            txtmaxchild.Enabled = False
            txtmaxpax.Enabled = False
            txtsrcitizenage.Enabled = False
            ddlchildallowed.Enabled = False
            ddlsrcitizen.Enabled = False

        ElseIf Session("ExcTypesState") = "Edit" Then
            txtCode.Enabled = True
        End If
    End Sub
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from excursiontypes Where exctypcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("exctypcode")) = False Then
                        Me.txtCode.Text = CType(mySqlReader("exctypcode"), String)
                    Else
                        Me.txtCode.Text = ""
                    End If
                    If IsDBNull(mySqlReader("exctypname")) = False Then
                        Me.txtName.Text = CType(mySqlReader("exctypname"), String)
                    Else
                        Me.txtName.Text = ""
                    End If
                End If
            End If

            mySqlCmd.Dispose()
            mySqlReader.Close()
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from excursiontypes_occupancy Where exctypcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("maxpaxperbooking")) = False Then
                        Me.txtmaxpax.Text = CType(mySqlReader("maxpaxperbooking"), String)
                    Else
                        Me.txtmaxpax.Text = ""
                    End If
                    If IsDBNull(mySqlReader("minpaxperbooking")) = False Then
                        Me.txtminpax.Text = CType(mySqlReader("minpaxperbooking"), String)
                    Else
                        Me.txtmaxpax.Text = ""
                    End If

                    If IsDBNull(mySqlReader("childallowed")) = False Then
                        ddlchildallowed.SelectedValue = CType(mySqlReader("childallowed"), String)
                        If CType(mySqlReader("childallowed"), String) = "No" Then
                            txtchildagefrm.Enabled = False
                            txtchildageto.Enabled = False
                            txtmaxchild.Enabled = False

                        ElseIf CType(mySqlReader("childallowed"), String) = "Yes" Then
                            txtchildagefrm.Enabled = True
                            txtchildageto.Enabled = True
                            txtmaxchild.Enabled = True
                        End If

                    End If
                    If IsDBNull(mySqlReader("maxchild")) = False And CType(mySqlReader("maxchild"), Integer) <> "0" Then
                        Me.txtmaxchild.Text = CType(mySqlReader("maxchild"), String)
                    Else
                        Me.txtmaxchild.Text = ""
                    End If

                    If IsDBNull(mySqlReader("chidlagefrom")) = False And CType(mySqlReader("chidlagefrom"), Integer) <> "0" Then
                        Me.txtchildagefrm.Text = CType(mySqlReader("chidlagefrom"), String)
                    Else
                        Me.txtchildagefrm.Text = ""
                    End If

                    If IsDBNull(mySqlReader("childageto")) = False And CType(mySqlReader("childageto"), Integer) <> "0" Then
                        Me.txtchildageto.Text = CType(mySqlReader("childageto"), String)
                    Else
                        Me.txtchildageto.Text = ""
                    End If

                    If IsDBNull(mySqlReader("minheight")) = False And CType(mySqlReader("minheight"), Integer) <> "0" Then
                        Me.txtchildminht.Text = CType(mySqlReader("minheight"), String)
                    Else
                        Me.txtchildminht.Text = ""
                    End If
                    If IsDBNull(mySqlReader("maxheight")) = False And CType(mySqlReader("maxheight"), Integer) <> "0" Then
                        Me.txtchildmaxht.Text = CType(mySqlReader("maxheight"), String)
                    Else
                        Me.txtchildmaxht.Text = ""
                    End If
                    If IsDBNull(mySqlReader("maxweight")) = False And CType(mySqlReader("maxweight"), Integer) <> "0" Then
                        Me.txtchildmaxwt.Text = CType(mySqlReader("maxweight"), String)
                    Else
                        Me.txtchildmaxwt.Text = ""
                    End If
                    If IsDBNull(mySqlReader("minweight")) = False And CType(mySqlReader("minweight"), Integer) <> "0" Then
                        Me.txtchildminwt.Text = CType(mySqlReader("minweight"), String)
                    Else
                        Me.txtchildminwt.Text = ""
                    End If

                        If IsDBNull(mySqlReader("seniorallowed")) = False Then
                            If CType(mySqlReader("seniorallowed"), String) = "No" Then
                                txtsrcitizenage.Enabled = False
                            End If
                            If CType(mySqlReader("seniorallowed"), String) = "Yes" Then
                                txtsrcitizenage.Enabled = True
                            End If
                            ddlsrcitizen.SelectedValue = CType(mySqlReader("seniorallowed"), String)
                        Else
                            ddlsrcitizen.SelectedValue = "[Select]"
                        End If
                    If IsDBNull(mySqlReader("seniorfromage")) = False And CType(mySqlReader("seniorfromage"), Integer) <> "0" Then
                        Me.txtsrcitizenage.Text = CType(mySqlReader("seniorfromage"), String)
                    Else
                        Me.txtsrcitizenage.Text = ""
                    End If

                    End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcOccHtWtMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strPassQry As String = "false"
        Dim frmmode As String = 0
        Dim strUsrnm As String = objUtils.ExecuteQueryReturnStringValuenew(Session("dbconnectionName"), "select option_selected    from reservation_parameters where param_id =1088")

        Try
            If Page.IsValid = True Then
                If Session("ExcTypesState") = "New" Or Session("ExcTypesState") = "Edit" Then

                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    'If checkForDuplicate() = False Then
                    '    Exit Sub
                    'End If


                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    Dim optionval As String
                    Dim lastno As String
                    If Session("ExcTypesState").ToString() = "New" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Save First Main Details.');", True)
                        Exit Sub

                    ElseIf Session("ExcTypesState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_add_exctypoccu", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Text.Trim, String)
                    If txtminpax.Text.Trim <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@minpaxperbooking", SqlDbType.Int)).Value = CType(txtminpax.Text.Trim, Integer)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@minpaxperbooking", SqlDbType.Int)).Value = "0"
                    End If
                    If txtmaxpax.Text.Trim <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@maxpaxperbooking", SqlDbType.Int)).Value = CType(txtmaxpax.Text.Trim, Integer)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@maxpaxperbooking", SqlDbType.Int)).Value = "0"
                    End If
                    If ddlchildallowed.SelectedValue.Trim <> "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@childallowed", SqlDbType.VarChar, 10)).Value = CType(ddlchildallowed.SelectedValue.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@childallowed", SqlDbType.VarChar, 10)).Value = ""
                    End If

                    ' If ddlchildallowed.SelectedValue <> "[Select]" Then
                    If ddlchildallowed.SelectedValue = "Yes" Then
                        If txtmaxchild.Text.Trim <> "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@maxchild", SqlDbType.Int)).Value = CType(txtmaxchild.Text.Trim, Integer)
                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@maxchild", SqlDbType.Int)).Value = "0"
                        End If
                        If txtchildagefrm.Text.Trim <> "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@chidlagefrom", SqlDbType.Decimal)).Value = CType(txtchildagefrm.Text.Trim, Decimal)
                        Else

                            mySqlCmd.Parameters.Add(New SqlParameter("@chidlagefrom", SqlDbType.Decimal)).Value = "0"
                        End If
                        If txtchildageto.Text.Trim <> "" Then
                            mySqlCmd.Parameters.Add(New SqlParameter("@childageto", SqlDbType.Decimal)).Value = CType(txtchildageto.Text.Trim, Decimal)

                        Else
                            mySqlCmd.Parameters.Add(New SqlParameter("@childageto", SqlDbType.Decimal)).Value = "0"
                        End If


                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@maxchild", SqlDbType.Int)).Value = "0"
                        mySqlCmd.Parameters.Add(New SqlParameter("@chidlagefrom", SqlDbType.Int)).Value = "0"
                        mySqlCmd.Parameters.Add(New SqlParameter("@childageto", SqlDbType.Int)).Value = 0


                        'mySqlCmd.Parameters.Add(New SqlParameter("@minheight", SqlDbType.Int)).Value = "0"
                        'mySqlCmd.Parameters.Add(New SqlParameter("@maxheight", SqlDbType.Int)).Value = "0"

                        'mySqlCmd.Parameters.Add(New SqlParameter("@minweight", SqlDbType.Int)).Value = "0"
                        'mySqlCmd.Parameters.Add(New SqlParameter("@maxweight", SqlDbType.Int)).Value = "0"

                    End If


                    If txtchildminht.Text.Trim <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@minheight", SqlDbType.Decimal)).Value = CType(txtchildminht.Text.Trim, Decimal)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@minheight", SqlDbType.Decimal)).Value = "0"
                    End If
                    If txtchildmaxht.Text.Trim <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@maxheight", SqlDbType.Decimal)).Value = CType(txtchildmaxht.Text.Trim, Decimal)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@maxheight", SqlDbType.Decimal)).Value = "0"
                    End If
                    If txtchildminwt.Text.Trim <> "" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@minweight", SqlDbType.Decimal)).Value = CType(txtchildminwt.Text.Trim, Integer)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@minweight", SqlDbType.Decimal)).Value = "0"
                    End If
                    If txtchildmaxwt.Text.Trim <> "" Then

                        mySqlCmd.Parameters.Add(New SqlParameter("@maxweight", SqlDbType.Decimal)).Value = CType(txtchildmaxwt.Text.Trim, Decimal)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@maxweight", SqlDbType.Decimal)).Value = "0"
                    End If
                    If ddlsrcitizen.SelectedValue.Trim <> "[Select]" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@seniorallowed", SqlDbType.VarChar, 10)).Value = CType(ddlsrcitizen.SelectedValue.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@seniorallowed", SqlDbType.VarChar, 10)).Value = ""
                    End If
                    If ddlsrcitizen.SelectedValue = "Yes" Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@seniorfromage", SqlDbType.Decimal)).Value = CType(txtsrcitizenage.Text.Trim, Decimal)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@seniorfromage", SqlDbType.Decimal)).Value = 0
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)


                    mySqlCmd.ExecuteNonQuery()


                ElseIf Session("ExcTypesState") = "Delete" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_del_exctypmaster", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()
                End If

                    sqlTrans.Commit()    'SQl Tarn Commit
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                    ' Response.Redirect("Other Services Selling Types Search.aspx", False)



                    Session.Add("ExcTypesRefCode", txtCode.Text.Trim)

                    If Session("ExcTypesState") = "New" Then

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Saved Successfully.');", True)

                        Session.Add("ExcTypesState", "Edit")



                        'txtCode.Value = txtCode.Value 'added by sribish


                    ElseIf Session("ExcTypesState") = "Edit" Then
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Updated Successfully.');", True)
                        Session.Add("ExcTypesState", "Edit")



                    End If


                    If Session("ExcTypesState") = "Delete" Then

                        Dim strscript As String = ""

                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Record Deleted Successfully.');", True)

                        strscript = "window.opener.__doPostBack('OtherSerSelltypeWindowPostBack', '');window.opener.focus();window.close();"
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)
                    End If

                End If

        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try



    End Sub


    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        'Response.Redirect("SupplierSearch.aspx")
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('ExcursionSuppliersWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

    End Sub
    Public Function checkForDuplicate() As Boolean
        If Session("ExcTypesState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "excursiontypes", "exctypcode", txtCode.Text.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Excursion code is already present.');", True)
                SetFocus(txtCode)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "excursiontypes", "exctypname", txtName.Text.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Excursion name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        ElseIf Session("ExcTypesState") = "Edit" Then
            If objUtils.isDuplicateForModifynew(Session("dbconnectionName"), "excursiontypes", "exctypcode", "exctypname", txtName.Text.Trim, CType(txtCode.Text.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This  Excursion name is already present.');", True)
                SetFocus(txtName)
                checkForDuplicate = False
                Exit Function
            End If
        End If
        checkForDuplicate = True
    End Function

    'Protected Sub ddlSupplierType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    txtSupplierType.Value = objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"sptypemast", "sptypename", "sptypecode", ddlSupplierType.SelectedValue.Trim.ToString)
    'End Sub
#Region "Public Function ValidatePage() As Boolean"
    Public Function ValidatePage() As Boolean
        Try
            'If txtclassificationname.Text.Trim = "" Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Classification Name.');", True)
            '    SetFocus(txtclassificationname.Text)
            '    ValidatePage = False
            '    Exit Function
            'End If
            If ddlchildallowed.SelectedValue = "Yes" Then
                If txtchildagefrm.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Child Age From .');", True)
                    SetFocus(txtchildagefrm)
                    ValidatePage = False
                    Exit Function
                End If
                If txtchildageto.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Child Age From .');", True)
                    SetFocus(txtchildageto)
                    ValidatePage = False
                    Exit Function
                End If
                If txtmaxchild.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Max Child .');", True)
                    SetFocus(txtmaxchild)
                    ValidatePage = False
                    Exit Function
                End If
            End If


            If ddlsrcitizen.SelectedValue = "Yes" Then
                If txtsrcitizenage.Text = "" Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Sr. Citizen Age.');", True)
                    SetFocus(txtsrcitizenage)
                    ValidatePage = False
                    Exit Function

                End If
            End If
 

            ValidatePage = True
        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionOccHtWtMaster.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try
    End Function
#End Region

#Region "Public Function checkForDeletion() As Boolean"
    Public Function checkForDeletion() As Boolean
        checkForDeletion = True
        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partyinfo", "catcode", CType(txtCode.Text.Trim, String)) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierCategory is already used for a SuppliersWebInformation, cannot delete this SupplierCategory');", True)
            checkForDeletion = False
            Exit Function

        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "partymast", "catcode", CType(txtCode.Text.Trim, String)) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierCategory is already used for a Suppliers, cannot delete this SupplierCategory');", True)
            checkForDeletion = False
            Exit Function


        ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "supplier_agents", "catcode", CType(txtCode.Text.Trim, String)) Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This SupplierCategory is already used for a SupplierAgents, cannot delete this SupplierCategory');", True)
            checkForDeletion = False
            Exit Function


        End If
        checkForDeletion = True
    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=SupplierCategories','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub

    <System.Web.Script.Services.ScriptMethod()> _
    <System.Web.Services.WebMethod()> _
    Public Shared Function Gethoteltypelist(ByVal prefixText As String) As List(Of String)

        Dim strSqlQry As String = ""
        Dim myDS As New DataSet
        Dim Hotelnames As New List(Of String)
        Try

            strSqlQry = "select classificationname,classificationcode from excclassification_header where  active=1 and classificationname like  '" & Trim(prefixText) & "%'"
            Dim SqlConn As New SqlConnection
            Dim myDataAdapter As New SqlDataAdapter
            ' SqlConn = clsDBConnect.dbConnectionnew(objclsConnectionName.ConnectionName)
            SqlConn = clsDBConnect.dbConnectionnew("strDBConnection")
            'Open connection
            myDataAdapter = New SqlDataAdapter(strSqlQry, SqlConn)
            myDataAdapter.Fill(myDS)

            If myDS.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To myDS.Tables(0).Rows.Count - 1

                    Hotelnames.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(myDS.Tables(0).Rows(i)("classificationname").ToString(), myDS.Tables(0).Rows(i)("classificationcode").ToString()))
                    'Hotelnames.Add(myDS.Tables(0).Rows(i)("partyname").ToString() & "<span style='display:none'>" & i & "</span>")
                Next
            End If

            Return Hotelnames
        Catch ex As Exception
            Return Hotelnames
        End Try

    End Function




    Protected Sub ddlchildallowed_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlchildallowed.SelectedIndexChanged
        If ddlchildallowed.SelectedValue.Trim = "Yes" Then

            txtmaxchild.Enabled = True
            txtchildagefrm.Enabled = True
            txtchildageto.Enabled = True
        End If

        If ddlchildallowed.SelectedValue.Trim = "No" Then
            txtmaxchild.Text = ""
            txtchildagefrm.Text = ""
            txtchildageto.Text = ""
            txtmaxchild.Enabled = False
            txtchildagefrm.Enabled = False
            txtchildageto.Enabled = False
        End If
    End Sub

    Protected Sub ddlsrcitizen_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlsrcitizen.SelectedIndexChanged
        If ddlsrcitizen.SelectedItem.Value = "Yes" Then
            txtsrcitizenage.Enabled = True
        End If
        If ddlsrcitizen.SelectedItem.Value = "No" Then
            txtsrcitizenage.Text = ""
            txtsrcitizenage.Enabled = False
        End If
    End Sub
End Class



