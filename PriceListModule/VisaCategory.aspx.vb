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
Partial Class Visa_Selling_Types
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
                '    objUtils.FillDropDownListWithValuenew(Session("dbconnectionName"), ddlCurrencyCd, "currcode", "currname", "select * from currmast where active=1 order by currcode", True)

                ViewState.Add("visaServiceCategoryState", Request.QueryString("State"))
                ViewState.Add("visaServiceCategoryRefCode", Request.QueryString("RefCode"))




                If ViewState("visaServiceCategoryState") = "New" Then
                    SetFocus(TxtOtherServiceCode)
                    'If Request.QueryString("Type") = "HF" Then
                    '    lblHeading.Text = "Add New Handling Fees Selling Types"
                    '    Page.Title = Page.Title + " " + "New Handling Fees Selling Types"

                    'Else
                    '    lblHeading.Text = "Add New Other Services Selling Types"
                    '    Page.Title = Page.Title + " " + "New Other Services Selling Types"

                    'End If
                    lblHeading.Text = "Add New - Visa Category"
                    Page.Title = Page.Title + " " + "New Visa Category"
                    btnSave.Text = "Save"

                    'btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to save Visa Selling Types?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("visaServiceCategoryState") = "Edit" Then
                    SetFocus(txtOtherServiceSelling)
                    'If Request.QueryString("Type") = "HF" Then
                    '    lblHeading.Text = "Edit Handling Fees Selling Types"
                    '    Page.Title = Page.Title + " " + "Edit Handling Fees Selling Types"

                    'Else
                    '    lblHeading.Text = "Edit Other Services Selling Types"
                    '    Page.Title = Page.Title + " " + "Edit Other Services Selling Types"

                    'End If
                    lblHeading.Text = "Edit -Visa Category"
                    Page.Title = Page.Title + " " + "Edit Visa Category"
                    btnSave.Text = "Update"
                    DisableControl()

                    ShowRecord(CType(ViewState("visaServiceCategoryRefCode"), String))
                    ' btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to update Other Services Selling Types?')==false)return false;")
                    btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("visaServiceCategoryState") = "View" Then
                    SetFocus(btnCancel)


                    'If Request.QueryString("Type") = "HF" Then
                    '    lblHeading.Text = "View Handling Fees Selling Types"
                    '    Page.Title = Page.Title + " " + "View Handling Fees Selling Types"

                    'Else
                    '    lblHeading.Text = "View Other Services Selling Types"
                    '    Page.Title = Page.Title + " " + "View Other Services Selling Types"

                    'End If
                    lblHeading.Text = "View - Visa Category"
                    Page.Title = Page.Title + " " + "View Visa Category"
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()

                    ShowRecord(CType(ViewState("visaServiceCategoryRefCode"), String))

                ElseIf ViewState("visaServiceCategoryState") = "Delete" Then
                    SetFocus(btnSave)

                    'If Request.QueryString("Type") = "HF" Then
                    '    lblHeading.Text = "Delete Handling Fees Selling Types"
                    '    Page.Title = Page.Title + " " + "Delete Handling Fees Selling Types"

                    'Else
                    '    lblHeading.Text = "Delete Other Services Selling Types"
                    '    Page.Title = Page.Title + " " + "Delete Other Services Selling Types"

                    'End If

                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("visaServiceCategoryRefCode"), String))
                    '   btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Visa Selling Types?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to cancel?')==false)return false;")
                Dim typ As Type
                typ = GetType(DropDownList)

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("VisaSellingTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

    End Sub
#End Region

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("visaServiceCategoryState") = "View" Or ViewState("visaServiceCategoryState") = "Delete" Then
            TxtOtherServiceCode.Disabled = True
            txtOtherServiceSelling.Disabled = True

            chkActive.Disabled = True
        ElseIf ViewState("visaServiceCategoryState") = "Edit" Then
            TxtOtherServiceCode.Disabled = True

            chkActive.Disabled = False
        End If
    End Sub
#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Try
            If TxtOtherServiceCode.Value = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Code can not be blank.');", True)
                SetFocus(TxtOtherServiceCode)
                ValidatePage = False
                Exit Function
            End If
            If txtOtherServiceSelling.Value.Trim = "" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Name can not be blank.');", True)
                SetFocus(txtOtherServiceSelling)
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

        Try
            If Page.IsValid = True Then
                If ViewState("visaServiceCategoryState") = "New" Or ViewState("visaServiceCategoryState") = "Edit" Then

                    If ValidatePage() = False Then
                        Exit Sub
                    End If
                    If checkForDuplicate() = False Then                        
                        Exit Sub
                    End If

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    If ViewState("visaServiceCategoryState") = "New" Then
                        mySqlCmd = New SqlCommand("sp_add_visacategory", mySqlConn, sqlTrans)
                        frmmode = 1
                    ElseIf ViewState("visaServiceCategoryState") = "Edit" Then
                        mySqlCmd = New SqlCommand("sp_mod_visacategory", mySqlConn, sqlTrans)
                        frmmode = 2
                    End If
                    mySqlCmd.CommandType = CommandType.StoredProcedure

                    mySqlCmd.Parameters.Add(New SqlParameter("@visacategorycode", SqlDbType.VarChar, 20)).Value = CType(TxtOtherServiceCode.Value.Trim, String)
                    mySqlCmd.Parameters.Add(New SqlParameter("@visacategoryname", SqlDbType.VarChar, 100)).Value = CType(txtOtherServiceSelling.Value.Trim, String)

                    If chkActive.Checked = True Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 1
                    ElseIf chkActive.Checked = False Then
                        mySqlCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int)).Value = 0
                    End If

                    mySqlCmd.Parameters.Add(New SqlParameter("@price", SqlDbType.Decimal)).Value = CType(txtPrice.Value.Trim, String)

                    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)

                    mySqlCmd.ExecuteNonQuery()

                ElseIf ViewState("visaServiceCategoryState") = "Delete" Then
                    If checkForDeletion() = False Then
                        Exit Sub
                    End If
                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_del_visacategory", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@visacategorycode", SqlDbType.VarChar, 20)).Value = CType(TxtOtherServiceCode.Value.Trim, String)
                    mySqlCmd.ExecuteNonQuery()
                End If

               
                strPassQry = ""
             

              



                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                ' Response.Redirect("Other Services Selling Types Search.aspx", False)
                Dim strscript As String = ""
                strscript = "window.opener.__doPostBack('OtherSerSelltypeWindowPostBack', '');window.opener.focus();window.close();"
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)




                ''vij

                'sqlTrans.Commit()    'SQl Tarn Commit
                'clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                'clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                'clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                '' Response.Redirect("Other Services Selling Types Search.aspx", False)
                'Dim strscript As String = ""
                'strscript = "window.opener.__doPostBack('OtherSerSelltypeWindowPostBack', '');window.opener.focus();window.close();"
                'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)

            End If
        Catch ex As Exception
            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("VisaSellingTypes.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
        End Try

    End Sub

#End Region

#Region " Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from visacategory_master Where visacategorycode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    If IsDBNull(mySqlReader("visacategorycode")) = False Then
                        Me.TxtOtherServiceCode.Value = CType(mySqlReader("visacategorycode"), String)
                    Else
                        Me.TxtOtherServiceCode.Value = ""
                    End If
                    If IsDBNull(mySqlReader("visacategoryname")) = False Then
                        Me.txtOtherServiceSelling.Value = CType(mySqlReader("visacategoryname"), String)
                    Else
                        Me.txtOtherServiceSelling.Value = ""
                    End If

                    If IsDBNull(mySqlReader("price")) = False Then
                        Me.txtPrice.Value = CType(mySqlReader("price"), String)
                    Else
                        Me.txtPrice.Value = ""
                    End If
                    'ctrycode


                    If IsDBNull(mySqlReader("active")) = False Then
                        If CType(mySqlReader("active"), String) = "1" Then
                            chkActive.Checked = True
                        ElseIf CType(mySqlReader("active"), String) = "0" Then
                            chkActive.Checked = False
                        End If
                    End If




                    'If IsDBNull(mySqlReader("costyesno")) = False Then
                    '    If CType(mySqlReader("costyesno"), String) = "1" Then
                    '        chkHandlingFees.Checked = True
                    '    ElseIf CType(mySqlReader("costyesno"), String) = "0" Then
                    '        chkHandlingFees.Checked = False
                    '    End If
                    'End If
                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("VisaSellingTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
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
        If ViewState("visaServiceCategoryState") = "New" Then
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "VisaCategory_Master ", "VisaCategoryCode", CType(TxtOtherServiceCode.Value.Trim, String)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Category code is already present.');", True)
                SetFocus(TxtOtherServiceCode)
                checkForDuplicate = False
                Exit Function
            End If
            If objUtils.isDuplicatenew(Session("dbconnectionName"), "VisaCategory_Master ", "VisaCategoryName", txtOtherServiceSelling.Value.Trim) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Category name is already present.');", True)
                SetFocus(txtOtherServiceSelling)
                checkForDuplicate = False
                Exit Function
            End If
            'ElseIf Session("State") = "Edit" Then
            '    If objUtils.isDuplicateForModifynew(Session("dbconnectionName"),"othsellmast", "othsellcode", "othsellname", txtOtherServiceSelling.Value.Trim, CType(txtOtherServiceSelling.Value.Trim, String)) Then
            '        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Other Services Selling Types name is already present.');", True)
            '        SetFocus(txtOtherServiceSelling)
            '        checkForDuplicate = False
            '        Exit Function
            '    End If
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

        If objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "agentmast", "visasellingtype", CType(TxtOtherServiceCode.Value.Trim, String)) = True Then
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Visa SellingType is already used for a Customer, cannot delete this Visa SellingType');", True)
            checkForDeletion = False
            Exit Function

            'ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplistd", "othsellcode", CType(TxtOtherServiceCode.Value.Trim, String)) = True Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This ServiceType is already used for a Details of OtherServicePricelist, cannot delete this ServiceType');", True)
            '    checkForDeletion = False
            '    Exit Function

            'ElseIf objUtils.GetDBFieldValueExistnew(Session("dbconnectionName"), "oplisth", "othsellcode", CType(TxtOtherServiceCode.Value.Trim, String)) = True Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This ServiceType is already used for a OtherServicePricelist, cannot delete this ServiceType');", True)
            '    checkForDeletion = False
            '    Exit Function


        End If

        checkForDeletion = True
    End Function
#End Region

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=OtherServicesSellingTypes','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub
End Class
