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
    Dim mydataadapter As SqlDataAdapter
#End Region

#Region "Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            Try
                If CType(Session("GlobalUserName"), String) = "" Then
                    Response.Redirect("~/Login.aspx", False)
                    Exit Sub
                End If

               

                ViewState.Add("excServiceSelltypeState", Request.QueryString("State"))
                ViewState.Add("excServiceSelltypeRefCode", Request.QueryString("RefCode"))


                If ViewState("excServiceSelltypeState") = "New" Then
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexccode, "othtypcode", "othtypname", " select othtypcode,othtypname from vw_language order by othtypcode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexcname, "othtypname", "othtypcode", " select othtypcode,othtypname from vw_language order by othtypname", True)
                Else
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexccode, "othtypcode", "othtypname", " select othtypcode,othtypname from view_language_search order by othtypcode", True)
                    objUtils.FillDropDownListHTMLNEW(Session("dbconnectionName"), ddlexcname, "othtypname", "othtypcode", " select othtypcode,othtypname from view_language_search order by othtypname", True)
                End If

                If ViewState("excServiceSelltypeState") = "New" Then

                    lblHeading.Text = "Add New - Excursion   Days of the Week"
                    Page.Title = Page.Title + " " + "New Excursion   Days of the Week"
                    btnSave.Text = "Save"
                    btnSave.Visible = False
                    FillGrid()
                    fildaysgrid()
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want Return to Search ?')==false)return false;")
                    'btnSave.Attributes.Add("onclick", "return FormValidation('New')")
                ElseIf ViewState("excServiceSelltypeState") = "Edit" Then


                    lblHeading.Text = "Edit -Excursion  Days ot the Week"
                    Page.Title = Page.Title + " " + "Edit Excursion   Days of the Week"
                    btnSave.Text = "Update"
                    btnSave.Visible = False
                    DisableControl()

                    ShowRecord(CType(ViewState("excServiceSelltypeRefCode"), String))
                    FillGrid()
                    fildaysgrid()
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want Return to Search?')==false)return false;")
                    'btnSave.Attributes.Add("onclick", "return FormValidation('Edit')")
                ElseIf ViewState("excServiceSelltypeState") = "View" Then
                    SetFocus(btnCancel)



                    lblHeading.Text = "View - Excursion Days of the Week"
                    Page.Title = Page.Title + " " + "View Excursion Days of the Week "
                    btnSave.Visible = False
                    btnCancel.Text = "Return to Search"
                    DisableControl()

                    ShowRecord(CType(ViewState("excServiceSelltypeRefCode"), String))
                    FillGrid()
                    fildaysgrid()
                ElseIf ViewState("excServiceSelltypeState") = "Delete" Then
                    SetFocus(btnSave)
                    lblHeading.Text = "Delete - Excursion  Days of the Week "


                    btnSave.Text = "Delete"
                    DisableControl()

                    ShowRecord(CType(ViewState("excServiceSelltypeRefCode"), String))
                    FillGrid()
                    fildaysgrid()
                    '   btnSave.Attributes.Add("onclick", "return FormValidation('Delete')")
                    btnSave.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to delete Excursion Days of the Week ?')==false)return false;")
                End If
                btnCancel.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want Return to Search ?')==false)return false;")
               
               
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
                objUtils.WritErrorLog("ExcursionTypesSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))
            End Try
        End If

        Dim typ As Type
        typ = GetType(DropDownList)

        If (Page.ClientScript.IsClientScriptIncludeRegistered(typ, "TADDScript.js")) = False Then
            Page.ClientScript.RegisterClientScriptInclude(typ, "TADDScript.js", "TADDScript.js")

            ddlexccode.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
            ddlexcname.Attributes.Add("onKeyDown", "TADD_OnKeyDown(this);")
        End If

    End Sub
#End Region

#Region "Private Sub DisableControl()"
    Private Sub DisableControl()
        If ViewState("excServiceSelltypeState") = "View" Or ViewState("excServiceSelltypeState") = "Delete" Then
           
            ddlexccode.Disabled = True
            ddlexcname.Disabled = True
            gv_SearchResult.Enabled = False

           
        ElseIf ViewState("excServiceSelltypeState") = "Edit" Then

            ddlexccode.Disabled = True
            ddlexcname.Disabled = True
        ElseIf ViewState("excServiceSelltypeState") = "New" Then
            ddlexccode.Disabled = False
            ddlexcname.Disabled = False
        End If
    End Sub
#End Region

#Region " Validate Page"
    Public Function ValidatePage() As Boolean
        Try
            
            If ddlexccode.Value = "[Select]" Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select Group code.');", True)
                SetFocus(ddlexccode)
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
                If ViewState("excServiceSelltypeState") = "New" Or ViewState("excServiceSelltypeState") = "Edit" Then

                    'If ValidatePage() = False Then
                    '    Exit Sub
                    'End If
                    'If checkForDuplicate() = False Then
                    '    Exit Sub
                    'End If

                    'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    'sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                    'If ViewState("excServiceSelltypeState") = "New" Then
                    '    mySqlCmd = New SqlCommand("sp_add_exctypes", mySqlConn, sqlTrans)
                    '    frmmode = 1
                    'ElseIf ViewState("excServiceSelltypeState") = "Edit" Then
                    '    mySqlCmd = New SqlCommand("sp_mod_exctypes", mySqlConn, sqlTrans)
                    '    frmmode = 2
                    'End If
                    'mySqlCmd.CommandType = CommandType.StoredProcedure




                    'If ViewState("excServiceSelltypeState") = "New" Then
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@userlogged", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    'ElseIf ViewState("excServiceSelltypeState") = "Edit" Then
                    '    mySqlCmd.Parameters.Add(New SqlParameter("@moduser", SqlDbType.VarChar, 10)).Value = CType(Session("GlobalUserName"), String)
                    'End If



                    'mySqlCmd.ExecuteNonQuery()
                    Dim strscript As String = ""
                    strscript = "window.opener.__doPostBack('OtherSerSelltypeWindowPostBack', '');window.opener.focus();window.close();"
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)



                ElseIf ViewState("excServiceSelltypeState") = "Delete" Then

                    mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                    sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start
                    frmmode = 3
                    mySqlCmd = New SqlCommand("sp_del_othypmast_language_dayofweek", mySqlConn, sqlTrans)
                    mySqlCmd.CommandType = CommandType.StoredProcedure
                    mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(ddlexcname.Value, String)
                    mySqlCmd.ExecuteNonQuery()


                    sqlTrans.Commit()    'SQl Tarn Commit
                    clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                    clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                    clsDBConnect.dbConnectionClose(mySqlConn)           'connection close
                    ' Response.Redirect("Other Services Selling Types Search.aspx", False)
                    Dim strscript As String = ""
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

#Region " Private Sub ShowRecord(ByVal RefCode As String)"
    Private Sub ShowRecord(ByVal RefCode As String)
        Try
            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
            mySqlCmd = New SqlCommand("Select * from view_language_search Where othtypcode='" & RefCode & "'", mySqlConn)
            mySqlReader = mySqlCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If mySqlReader.HasRows Then
                If mySqlReader.Read() = True Then
                    
                    If IsDBNull(mySqlReader("othtypcode")) = False Then
                        Me.ddlexcname.Value = CType(mySqlReader("othtypcode"), String)
                        Me.ddlexccode.Value = CType(mySqlReader("othtypname"), String)  'objUtils.GetDBFieldFromStringnew(Session("dbconnectionName"),"ctrymast", "ctryname", "ctrycode", CType(mySqlReader("ctrycode"), String))
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

        'ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", "window.close();", True)
        Dim strscript As String = ""
        strscript = "window.opener.__doPostBack('OtherSerSelltypeWindowPostBack', '');window.opener.focus();window.close();"
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "popup", strscript, True)




    End Sub
#End Region

#Region "Public Function checkForDuplicate() As Boolean"
    Public Function checkForDuplicate() As Boolean
        If ViewState("excServiceSelltypeState") = "New" Then
            'If objUtils.isDuplicatenew(Session("dbconnectionName"), "excsellmast ", "excsellcode", CType(txtcode.Value.Trim, String)) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This Excursion Types code is already present.');", True)
            '    SetFocus(txtcode)
            '    checkForDuplicate = False
            '    Exit Function
            'End If
            'If objUtils.isDuplicatenew(Session("dbconnectionName"), "excsellmast", "excsellname", txtname.Value.Trim) Then
            '    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('This  Excursion Types name is already present.');", True)
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

    Private Sub FillGrid()
        Dim myDS As New DataSet


      

        gv_SearchResult.Visible = True


        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try

            strSqlQry = ""




            'If ViewState("excServiceSelltypeState") = "Edit" Then
            '    strSqlQry = "select nationalitycode,nationlanguage,daysofweek from view_language_grid where othtypcode ='" & ddlexcname.Value & "'"
            'ElseIf ViewState("excServiceSelltypeState") = "New" Then
            '    strSqlQry = "select nationalitycode,nationlanguage,'' daysofweek from nationality_master where showinexcursions=1 "
            'End If

            myDS = objUtils.GetDataFromDatasetnew(Session("dbconnectionName"), "Exec sp_language_grid '" & ddlexcname.Value & "' ")

            'mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            'mydataadapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            'mydataadapter.Fill(myDS)
            gv_SearchResult.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_SearchResult.DataBind()
            Else
                gv_SearchResult.PageIndex = 0
                gv_SearchResult.DataBind()
                'lblMsg.Visible = True
                'lblMsg.Text = "Records not found, Please redefine search criteria."
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionDaysoftheweekSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub
    Private Sub fildaysgrid()
        Dim myDS As New DataSet

        gv_days.Visible = True


        If gv_SearchResult.PageIndex < 0 Then
            gv_SearchResult.PageIndex = 0
        End If
        strSqlQry = ""
        Try

            strSqlQry = ""



            strSqlQry = "select * from dayweek "


            mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))                             'Open connection
            mydataadapter = New SqlDataAdapter(strSqlQry, mySqlConn)
            mydataadapter.Fill(myDS)
            gv_days.DataSource = myDS

            If myDS.Tables(0).Rows.Count > 0 Then
                gv_days.DataBind()
            Else
                gv_days.PageIndex = 0
                gv_days.DataBind()
               
            End If

        Catch ex As Exception
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("ExcursionDaysoftheweekSearch.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))

        Finally
            'clsDBConnect.dbAdapterClose(myDataAdapter)                       'Close adapter
            'clsDBConnect.dbConnectionClose(SqlConn)                          'Close connection
        End Try
    End Sub

    Protected Sub btnhelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnhelp.Click
        ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "window.open('../Help.aspx?hi=ExcursionTypes','_blank','status=1,scrollbars=1,top=135,left=760,width=250,height=500');", True)
    End Sub


    Protected Sub gv_SearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_SearchResult.RowCommand
        '  daysgrid.Visible = True

        '  Dim gvr As GridViewRow = DirectCast(DirectCast(e.CommandSource, ButtonField).NamingContainer, GridViewRow)
        Dim RowIndex As Integer = Convert.ToInt32(e.CommandArgument)
        RowIndex = RowIndex
        chkselectall.Checked = False
        Dim RowText As String = gv_SearchResult.Rows(RowIndex).Cells(3).Text
        RowText = RowText

        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim Str() As String = RowText.Split(",")
        Dim Language As String = String.Empty
        Session("Languagecd") = gv_SearchResult.Rows(RowIndex).Cells(1).Text
        Session("Languagenm") = gv_SearchResult.Rows(RowIndex).Cells(2).Text
        If gv_days.Rows.Count > 0 Then
            For i = 0 To gv_days.Rows.Count - 1
                If Str.Length > 0 Then
                    For j = 0 To Str.Length - 1
                        Dim chksel As New HtmlInputCheckBox
                        chksel = gv_days.Rows(i).FindControl("chkdays")
                        If gv_days.Rows(i).Cells(2).Text = Str(j).ToString() Then

                            chksel.Checked = True
                            Exit For
                        Else
                            chksel.Checked = False
                        End If
                    Next
                End If
            Next
        End If

        ModalPopupDays.Show()
        'gv_SearchResult.Rows(CType(e.CommandArgument.ToString, String)).FindControl("daysofweek")
        '  

    End Sub

    Protected Sub exit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexit.Click

        'ModalPopupDays.Hide()
    End Sub

  
    Protected Sub chkselectall_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkselectall.CheckedChanged
        Dim chksel As HtmlInputCheckBox
        For Each GvRow In gv_days.Rows
            chksel = GvRow.FindControl("chkdays")
            If chkselectall.Checked = True Then
                chksel.Checked = True
            Else
                chksel.Checked = False
            End If
        Next
        ModalPopupDays.Show()
    End Sub

   
    Protected Sub save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save.Click
        Dim frmmode As Integer
        Dim strlan As String
        Dim i As Integer
        Dim chksel As HtmlInputCheckBox

        Try
            For Each GvRow In gv_days.Rows
                chksel = GvRow.FindControl("chkdays")

                If chksel.Checked = True Then
                    strlan = strlan + gv_days.Rows(i).Cells(2).Text + ","
                
                End If
                i = i + 1
            Next


       
            If ViewState("excServiceSelltypeState") = "New" Or ViewState("excServiceSelltypeState") = "Edit" Then

                If ValidatePage() = False Then
                    Exit Sub
                End If

                If strlan Is Nothing Then
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('Please select at least one day.');", True)

                    Exit Sub
                End If

                mySqlConn = clsDBConnect.dbConnectionnew(Session("dbconnectionName"))           'connection open
                sqlTrans = mySqlConn.BeginTransaction           'SQL  Trans start

                'If ViewState("excServiceSelltypeState") = "New" Then
                '    mySqlCmd = New SqlCommand("sp_add_othypmast_language_dayofweek", mySqlConn, sqlTrans)
                '    frmmode = 1
                'ElseIf ViewState("excServiceSelltypeState") = "Edit" Then
                '    mySqlCmd = New SqlCommand("sp_mod_othypmast_language_dayofweek", mySqlConn, sqlTrans)
                '    frmmode = 2
                'End If
                mySqlCmd = New SqlCommand("sp_add_othypmast_language_dayofweek", mySqlConn, sqlTrans)
                mySqlCmd.CommandType = CommandType.StoredProcedure

                mySqlCmd.Parameters.Add(New SqlParameter("@othtypcode", SqlDbType.VarChar, 20)).Value = CType(ddlexcname.Value, String)
                mySqlCmd.Parameters.Add(New SqlParameter("@nationalitycode", SqlDbType.VarChar, 20)).Value = Trim(Session("languagecd"))
                mySqlCmd.Parameters.Add(New SqlParameter("@nationlanguage", SqlDbType.VarChar, 20)).Value = Session("languagenm")
                mySqlCmd.Parameters.Add(New SqlParameter("@daysofweek", SqlDbType.VarChar, 200)).Value = strlan.Substring(0, Len(strlan) - 1)



                mySqlCmd.ExecuteNonQuery()

                sqlTrans.Commit()    'SQl Tarn Commit
                clsDBConnect.dbSqlTransation(sqlTrans)              ' sql Transaction disposed 
                clsDBConnect.dbCommandClose(mySqlCmd)               'sql command disposed
                clsDBConnect.dbConnectionClose(mySqlConn)

                FillGrid()
            End If
           


        Catch ex As Exception

            If mySqlConn.State = ConnectionState.Open Then
                sqlTrans.Rollback()
            End If
            ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "", "alert('" & "Error Description " + ex.Message.Replace("'", " ") & "' );", True)
            objUtils.WritErrorLog("Excursiondaysoftheweek.aspx", Server.MapPath("ErrorLog.txt"), ex.Message.ToString, Session("GlobalUserName"))


        End Try
    End Sub

    Protected Sub gv_SearchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_SearchResult.RowDataBound
        Try
            e.Row.Cells(1).Style.Add("display", "none")
            
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gv_days_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_days.RowDataBound
       
    End Sub
End Class
