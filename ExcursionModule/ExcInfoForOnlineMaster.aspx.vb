



Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Web.UI.WebControls


Partial Class ExcInfoForOnlineMaster
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
                pnlimage.Visible = False
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
                    lblHeading.Text = "Add New - Excursion   Types"
                    Page.Title = Page.Title + " " + "New Excursion   Types"
                    btnSave.Text = "Save"
                    txtCode.ReadOnly = True

                    txtName.ReadOnly = True
                    btnviewimage.Visible = False
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                    'End If
                ElseIf Session("ExcTypesState") = "Edit" Then
                    SetFocus(txtName)
                    txtCode.ReadOnly = True
                    txtName.ReadOnly = True
                    ' lblHeading.Text = "Edit Supplier Category"
                    Page.Title = Page.Title + " " + "Edit Excursion Types"
                    btnSave.Text = "Update"
                    DisableControl()
                    ShowRecord(CType(Session("ExcTypesRefCode"), String))
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf Session("ExcTypesState") = "View" Then
                    SetFocus(btnCancel)
                    txtCode.ReadOnly = True
                    txtName.ReadOnly = True
                    ' lblHeading.Text = "View Supplier Category"
                    Page.Title = Page.Title + " " + "View Excursion Types"
                    btnSave.Visible = False
                    'btnCancel.Text = "Return to Supplier Category"
                    DisableControl()
                    ShowRecord(CType(Session("ExcTypesRefCode"), String))
                ElseIf Session("ExcTypesState") = "Delete" Then
                    SetFocus(btnSave)
                    txtCode.ReadOnly = True
                    txtName.ReadOnly = True
                    'lblHeading.Text = "Delete Supplier Category"
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
            Page.Title = "Excursion Types Entry"
        End If
    End Sub

    Private Sub DisableControl()
        If Session("ExcTypesState") = "View" Or Session("ExcTypesState") = "Delete" Then
            txtCode.Enabled = False
            txtName.Enabled = False
            txtimg.Disabled = True
            txtdesc.Enabled = False
        
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
            mySqlCmd = New SqlCommand("Select * from excursiontypes_infoonline Where exctypcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("description")) = False Then
                        Me.txtdesc.Text = CType(mySqlReader("description"), String)
                    Else
                        Me.txtdesc.Text = ""
                    End If


                    If IsDBNull(mySqlReader("imagename")) = False Then
                        btnviewimage.Visible = True
                        Me.txtimg.Value = mySqlReader("imagename")
                    Else
                        Me.txtimg.Value = ""

                    End If


                End If
            Else
                btnviewimage.Visible = False
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcInfoForOnline.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        Finally
            clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
            clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
            clsDBConnect.dbReaderClose(mySqlReader)             ' sql reader disposed    
        End Try
    End Sub

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
                If Session("ExcTypesState") = "New" Or Session("ExcTypesState") = "Edit" Then

                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    'If checkForDuplicate() = False Then
                    '    Exit Sub
                    'End If

                    If fileImage.FileName <> "" Then
                        strpath_logo1 = CType(txtCode.Text.Trim.Replace("ET/", ""), String) & "_" & fileImage.FileName

                        strpath1 = Server.MapPath("Excursionimages/" & strpath_logo1)
                        fileImage.PostedFile.SaveAs(strpath1)
                     
                    Else
                        txtimg.Value = IIf(txtimg.Value = "", fileImage.FileName, txtimg.Value)

                        ' SendImageToWebService(strpath1, strpath_logo1)
                    End If



                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If Session("ExcTypesState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_exctypinfoonline", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf Session("ExcTypesState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_add_exctypinfoonline", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@exctypcode", SqlDbType.VarChar, 20)).Value = CType(txtCode.Text.Trim, String)
                    If txtimg.Value = "" Then

                        mySqlCmd.Parameters.Add(New SqlParameter("@imagename", SqlDbType.VarChar, 100)).Value = CType(strpath_logo1.Trim, String)
                        txtimg.Value = CType(strpath_logo1.Trim, String)
                    Else
                        mySqlCmd.Parameters.Add(New SqlParameter("@imagename", SqlDbType.VarChar, 100)).Value = CType(txtimg.Value, String)
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@description", SqlDbType.VarChar, 2000)).Value = CType(txtdesc.Text.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    mySqlCmd.ExecuteNonQuery()


                ElseIf ViewState("ExcTypesState") = "Delete" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_del_exctypes", mySqlConn, sqlTrans)
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

#End Region


    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Response.Redirect("SupplierCategoriesSearch.aspx", False)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
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
            'If ddlchildallowed.SelectedValue = "Yes" Then
            '    If txtchildagefrm.Text = "" Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Child Age From .');", True)
            '        SetFocus(txtchildagefrm)
            '        ValidatePage = False
            '        Exit Function
            '    End If
            '    If txtchildageto.Text = "" Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Child Age From .');", True)
            '        SetFocus(txtchildageto)
            '        ValidatePage = False
            '        Exit Function
            '    End If
            '    If txtmaxchild.Text = "" Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Enter Max Child .');", True)
            '        SetFocus(txtmaxchild)
            '        ValidatePage = False
            '        Exit Function
            '    End If
            'End If


            'If ddlsrcitizen.SelectedValue = "Yes" Then
            '    If txtsrcitizenage.Text = "" Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please Select Sr. Citizen Age.');", True)
            '        SetFocus(txtsrcitizenage)
            '        ValidatePage = False
            '        Exit Function

            '    End If
            'End If


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
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=excursioninfo','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
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


    Protected Sub fileImage_Load(sender As Object, e As System.EventArgs) Handles fileImage.Load


    End Sub

    Protected Sub btnviewimage_Click(sender As Object, e As System.EventArgs) Handles btnviewimage.Click
        If txtimg.Value <> "" Then
            Dim delimg As String = Server.MapPath("Excursionimages/" & txtimg.Value)
            If System.IO.File.Exists(delimg) Then
                pnlimage.Visible = True
                imginfo.ImageUrl = "../ExcursionModule/Excursionimages/" + txtimg.Value
            Else
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Image To Display..');", True)
            End If
        Else
            pnlimage.Visible = False
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('No Image To Display.');", True)

        End If


    End Sub

    Protected Sub btnremoveimg_Click(sender As Object, e As System.EventArgs) Handles btnremoveimg.Click
        txtimg.Value = ""
        'If txtimg.Value <> "" Then
        '    Dim delimg As String = Server.MapPath("Excursionimages/" & txtimg.Value)
        '    If System.IO.File.Exists(delimg) Then
        '        System.IO.File.Delete(delimg)
        '        txtimg.Value = ""
        '        pnlimage.Visible = False
        '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Image Removed Successfully.');", True)
        '    End If
        'End If
    End Sub
End Class




